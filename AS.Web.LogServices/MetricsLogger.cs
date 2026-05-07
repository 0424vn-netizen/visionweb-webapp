using AS.Common.DBManager;
using AS.Common.Logger;
using AS.Web.LogServices.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace AS.Web.LogServices
{
    public sealed class MetricsLoggerManager : IDisposable
    {
        private readonly string _operationName;
        private readonly string _logPrefix;
        private readonly Stopwatch _totalWatch;
        private readonly ConcurrentDictionary<string, long> _durations;
        private readonly ConcurrentDictionary<string, long> _counts;
        private bool _disposed;

        // ================== CONFIG ==================
        private static readonly bool EnableBatch = GetBoolConfig("MetricsLogger_EnableBatch", true);
        private static readonly int MaxQueueSize = GetIntConfig("MetricsLogger_MaxQueueSize", 20000);
        private static readonly int BatchSize = GetIntConfig("MetricsLogger_BatchSize", 20);
        private static readonly int FlushIntervalMs = GetIntConfig("MetricsLogger_FlushIntervalMs", 50);
        private static readonly bool EnableStats = GetBoolConfig("MetricsLogger_EnableStats", false);
        private static readonly string LogLevel = GetStringConfig("MetricsLogger_LogLevel", "Summary");
        private static readonly int DetailMaxRows = GetIntConfig("MetricsLogger_DetailMaxRows", 3);

        private static readonly int QueueWarningThreshold = (int)(MaxQueueSize * 0.8);
        private static bool QueueWarningShown = false;

        // ========== QUEUE & WORKER ==========
        private static readonly BlockingCollection<string> _logQueue =
            new BlockingCollection<string>(new ConcurrentQueue<string>(), MaxQueueSize);
        private static readonly Thread _workerThread;
        private static readonly Thread _statsThread;
        private static volatile bool _running = true;

        private static long _enqCount;
        private static long _flushCount;
        private static long _totalFlushed;
        private static double _avgBatchSize;
        private static double _avgFlushMs;

        // ================== STATIC CTOR ==================
        static MetricsLoggerManager()
        {
            _workerThread = new Thread(ProcessLogQueue)
            {
                IsBackground = true,
                Name = "MetricsLogger.Logs"
            };
            _workerThread.Start();

            if (EnableStats)
            {
                _statsThread = new Thread(WriteStats)
                {
                    IsBackground = true,
                    Name = "MetricsLogger.Stats"
                };
                _statsThread.Start();
            }
        }

        // ================== WORKER LOOP ==================
        private static void ProcessLogQueue()
        {
            var batch = new List<string>(BatchSize);
            var lastFlush = Stopwatch.StartNew();

            while (_running || _logQueue.Count > 0)
            {
                try
                {
                    string msg;
                    if (_logQueue.TryTake(out msg, 20))
                    {
                        Interlocked.Increment(ref _enqCount);
                        if (EnableBatch)
                        {
                            batch.Add(msg);
                            if (batch.Count >= BatchSize || lastFlush.ElapsedMilliseconds >= FlushIntervalMs)
                                FlushBatch(batch, ref lastFlush);
                        }
                        else
                        {
                            LoggerManager.Info(msg);
                            Interlocked.Increment(ref _flushCount);
                            Interlocked.Increment(ref _totalFlushed);
                        }
                    }
                    else if (EnableBatch && batch.Count > 0 && lastFlush.ElapsedMilliseconds >= FlushIntervalMs)
                    {
                        FlushBatch(batch, ref lastFlush);
                    }

                    if (!QueueWarningShown && _logQueue.Count >= QueueWarningThreshold)
                    {
                        QueueWarningShown = true;
                        LoggerManager.Warn(string.Format(
                            "[MetricsLogger] Queue usage {0}/{1} ({2:P0}) - Suggestion: Increase BatchSize or reduce log detail.",
                            _logQueue.Count, MaxQueueSize, (double)_logQueue.Count / MaxQueueSize));
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Error("[MetricsLogger] Worker error: " + ex.Message);
                }
            }

            if (EnableBatch && batch.Count > 0)
                FlushBatch(batch, ref lastFlush);
        }

        private static void FlushBatch(List<string> batch, ref Stopwatch lastFlush)
        {
            if (batch == null || batch.Count == 0) return;
            var sw = Stopwatch.StartNew();
            try
            {
                foreach (var line in batch)
                {
                    LoggerManagerExtensions.InfoRaw(line);
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Error("[MetricsLogger] FlushBatch failed: " + ex.Message);
            }
            finally
            {
                long f = Interlocked.Increment(ref _flushCount);
                Interlocked.Add(ref _totalFlushed, batch.Count);
                _avgBatchSize = ((_avgBatchSize * (f - 1)) + batch.Count) / f;
                _avgFlushMs = ((_avgFlushMs * (f - 1)) + sw.ElapsedMilliseconds) / f;

                batch.Clear();
                lastFlush.Restart();
            }
        }

        // ================== STATS MONITOR ==================
        private static void WriteStats()
        {
            while (_running)
            {
                Thread.Sleep(10000); // every 10s
                try
                {
                    long enq = Interlocked.Exchange(ref _enqCount, 0);
                    long flush = Interlocked.Exchange(ref _flushCount, 0);
                    int q = _logQueue.Count;
                    double usage = (double)q / MaxQueueSize;
                    string suggestion = GetSuggestion(usage, _avgFlushMs, _avgBatchSize);
                    
                    LoggerManager.Info(string.Format(
                        "{0} [MetricsLogger] Stats: Enq/s={1}, Flush/s={2}, AvgBatch={3:F1}, FlushTime={4:F2}ms, Queue={5}/{6} ({7:P0}) → {8}",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        enq / 10.0, flush / 10.0, _avgBatchSize, _avgFlushMs, q, MaxQueueSize, usage, suggestion));
                }
                catch (Exception ex)
                {
                    LoggerManager.Error("[MetricsLogger] Stats error: " + ex.Message);
                }
            }
        }

        private static string GetSuggestion(double usage, double flushMs, double avgBatch)
        {
            if (usage > 0.9)
                return "Queue nearly full → reduce log level or increase BatchSize/QueueSize.";
            if (flushMs > 10)
                return "Flush slow → check LoggerManager target or disk.";
            if (avgBatch < BatchSize * 0.5)
                return "Batch small → lower BatchSize or increase log volume.";
            if (usage < 0.2)
                return "All good.";
            return "Stable.";
        }

        // ================== CONFIG HELPERS ==================
        private static bool GetBoolConfig(string key, bool def)
        {
            try
            {
                string v = ConfigurationManager.AppSettings[key];
                return !string.IsNullOrEmpty(v) && v.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            catch { return def; }
        }

        private static int GetIntConfig(string key, int def)
        {
            try
            {
                string v = ConfigurationManager.AppSettings[key];
                int n; return int.TryParse(v, out n) ? n : def;
            }
            catch { return def; }
        }

        private static string GetStringConfig(string key, string def)
        {
            try
            {
                string v = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(v) ? def : v;
            }
            catch { return def; }
        }

        private static void Enqueue(string msg)
        {
            try
            {                
                string line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff ") + msg;
                if (!_logQueue.TryAdd(line))
                    LoggerManager.Warn("[MetricsLogger] Queue full → dropping log. Suggestion: increase MaxQueueSize or lower log verbosity.");
            }
            catch (Exception ex)
            {
                LoggerManager.Error(ex);
            }
        }

        // ================== INSTANCE ==================
        public MetricsLoggerManager(string logPrefix = "", [CallerMemberName] string operationName = null)
        {
            _logPrefix = string.IsNullOrWhiteSpace(logPrefix) ? "" : $"[{logPrefix}]";
            _operationName = string.IsNullOrWhiteSpace(operationName) ? "MetricsLogger" : operationName;
            _totalWatch = Stopwatch.StartNew();
            _durations = new ConcurrentDictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            _counts = new ConcurrentDictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            _counts.TryAdd("Success", 0);
            _counts.TryAdd("Error", 0);
            _counts.TryAdd("Timeout", 0);
            _counts.TryAdd("TotalCalls", 0);
            Enqueue(string.Format("{0} [{1}] START", _logPrefix, _operationName));
        }

        public T Log<T>(string spaName, Func<T> func, FilterParameterCollection parameters = null)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                object result = func();
                sw.Stop();

                RecordSuccess(spaName, sw.ElapsedMilliseconds);

                if (result == null)
                {
                    Enqueue($"{_logPrefix} SPA={spaName} returned NULL");
                    return default(T);
                }

                if (result is DataSet)
                    LogDataSet(spaName, (DataSet)result, parameters, sw.ElapsedMilliseconds);
                else if (result is DataTable)
                    LogDataTable(spaName, (DataTable)result, parameters, sw.ElapsedMilliseconds);
                else
                    LogGeneric(spaName, result, parameters, sw.ElapsedMilliseconds);

                return (T)result;
            }
            catch (TimeoutException tex)
            {
                sw.Stop();
                RecordTimeout(spaName, sw.ElapsedMilliseconds);
                Enqueue($"{_logPrefix} Timeout in {spaName}: {tex.Message}");
                return default(T);
            }
            catch (Exception ex)
            {
                sw.Stop();
                RecordError(spaName, sw.ElapsedMilliseconds, ex);
                return default(T);
            }
        }

        // ================== LOG DETAIL ==================
        private void LogDataSet(string spaName, DataSet ds, FilterParameterCollection parameters, long duration)
        {
            if (ds == null)
            {
                Enqueue($"[{_logPrefix}] SPA={spaName} | Duration={duration}ms | Params=[{GetParamSummary(parameters)}] | DataSet=NULL");
                return;
            }

            int index = 0;
            foreach (DataTable dt in ds.Tables)
            {
                string tableName = !string.IsNullOrEmpty(dt.TableName)
                    ? dt.TableName
                    : $"{spaName}_Table{++index}";
                LogTableWithOptionalDuration($"[{_logPrefix}] SPA={spaName} - TableName: {tableName}", dt, parameters, null);
            }
        }

        private void LogDataTable(string spaName, DataTable dt, FilterParameterCollection parameters, long duration)
        {
            if (dt == null)
            {
                Enqueue($"[{_logPrefix}] SPA={spaName} | Duration={duration}ms | Params=[{GetParamSummary(parameters)}] | DataTable=NULL");
                return;
            }

            LogTableWithOptionalDuration($"{_logPrefix} SPA={spaName}", dt, parameters, duration);
        }

        private void LogTableWithOptionalDuration(string labelPrefix, DataTable dt, FilterParameterCollection parameters, long? duration)
        {
            if (dt == null)
            {
                Enqueue($"{labelPrefix} | Params=[{GetParamSummary(parameters)}] | DataTable=NULL");
                return;
            }

            string cols = GetColumnsString(dt);
            string baseInfo;
            if (duration.HasValue)
            {
                baseInfo = string.Format("{0} | Duration={1}ms | Params=[{2}] | Rows={3} | Cols={4} | Columns[{5}]",
                    labelPrefix, duration.Value, GetParamSummary(parameters), dt.Rows.Count, dt.Columns.Count, cols);
            }
            else
            {
                baseInfo = string.Format("{0} | Params=[{1}] | Rows={2} | Cols={3} | Columns[{4}]",
                    labelPrefix, GetParamSummary(parameters), dt.Rows.Count, dt.Columns.Count, cols);
            }

            Enqueue(baseInfo);

            if (LogLevel.Equals("Detail", StringComparison.OrdinalIgnoreCase))
            {
                int previewCount = Math.Min(DetailMaxRows, dt.Rows.Count);

                Enqueue($"{labelPrefix} | MetricsLogger_DetailMaxRows={DetailMaxRows}");

                for (int i = 0; i < previewCount; i++)
                {
                    string rowValues = string.Join(" | ", dt.Rows[i].ItemArray.Select(v => v == null ? "NULL" : v.ToString()));
                    Enqueue(string.Format("{0} -> Row {1}: {2}", labelPrefix, (i + 1), rowValues));
                }
            }
        }

        private string GetColumnsString(DataTable dt)
        {
            if (dt == null || dt.Columns.Count == 0) return string.Empty;
            try
            {
                return string.Join(", ", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            }
            catch (Exception ex)
            {
                LoggerManager.Error("Error getting columns string", ex);
                return string.Empty;
            }
        }

        private void LogGeneric(string spaName, object result, FilterParameterCollection parameters, long duration)
        {
            Enqueue(string.Format("{0} SPA={1} | Type={2} | Duration={3}ms | Params={4}",
                _logPrefix, spaName, result.GetType().Name, duration, GetParamSummary(parameters)));
        }

        private string GetParamSummary(FilterParameterCollection parameters)
        {
            if (parameters == null || parameters.Count == 0) return "(none)";
            var sb = new StringBuilder();
            foreach (FilterParameter p in parameters)
                sb.AppendFormat("{0}={1},", p.ParameterName, p.ParameterValue);
            if (sb.Length > 0) sb.Length--;
            return sb.ToString();
        }

        // ================== COUNTERS ==================
        private void RecordSuccess(string key, long elapsed)
        {
            _counts.AddOrUpdate("Success", 1, (k, v) => v + 1);
            _counts.AddOrUpdate("TotalCalls", 1, (k, v) => v + 1);
            _durations.AddOrUpdate(key, elapsed, (k, v) => Math.Max(v, elapsed));
        }

        private void RecordTimeout(string key, long elapsed)
        {
            _counts.AddOrUpdate("Timeout", 1, (k, v) => v + 1);
            _counts.AddOrUpdate("TotalCalls", 1, (k, v) => v + 1);
            _durations.AddOrUpdate(key, elapsed, (k, v) => Math.Max(v, elapsed));
        }

        private void RecordError(string key, long elapsed, Exception ex)
        {
            _counts.AddOrUpdate("Error", 1, (k, v) => v + 1);
            _counts.AddOrUpdate("TotalCalls", 1, (k, v) => v + 1);
            _durations.AddOrUpdate(key, elapsed, (k, v) => Math.Max(v, elapsed));
            Enqueue($"{_logPrefix} Error in {key}: {ex.Message}");
        }

        // ================== DISPOSE ==================
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _totalWatch.Stop();

            Enqueue(string.Format("{0} [{1}] Summary: Calls={2}, Success={3}, Timeout={4}, Error={5}, Duration={6}ms",
                _logPrefix, _operationName,
                Get("TotalCalls"), Get("Success"), Get("Timeout"), Get("Error"),
                _totalWatch.ElapsedMilliseconds));

            if (_durations.Count >= 2)
            {
                var slowest = _durations.OrderByDescending(d => d.Value).Take(3);
                string msg = string.Join(", ", slowest.Select(s => string.Format("{0}:{1}ms", s.Key, s.Value)));
                Enqueue(string.Format("{0} [{1}] Top slowest: {2}", _logPrefix, _operationName, msg));
            }

            Enqueue(string.Format("{0} [{1}] END", _logPrefix, _operationName));
        }

        private long Get(string key)
        {
            long v; return _counts.TryGetValue(key, out v) ? v : 0;
        }
    }
}