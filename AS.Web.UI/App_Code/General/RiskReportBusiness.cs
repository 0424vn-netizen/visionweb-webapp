using AS.Common.DBManager;
using AS.Common.Logger;
using AS.Threading;
using AS.Web.Business;
using AS.Web.LogServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// Summary description for RiskReportBusiness
/// </summary>
public class RiskReportBusiness
{
    #region Enhance Risk Report Performance    
    public int _ThreadCompleteCount = 0;


    public List<DataSourceParallelResponse> GetDataSourceParallel(List<DataSourceParallelRequest> requests)
    {
        if (requests == null || !requests.Any())
            return new List<DataSourceParallelResponse>();

        bool useEnhanced = WebSiteSettings.GetBoolConfig("RiskReportBusiness_GetDataSourceParallel_EnableEnhanced", true);

        bool benchmarkTestEnable = WebSiteSettings.GetBoolConfig("RiskReportBusiness_GetDataSourceParallel_BenchmarkTest_Enable", false);

        int numberCalls = WebSiteSettings.GetIntConfig("RiskReportBusiness_GetDataSourceParallel_BenchmarkTest_NumberCalls", 30);

        if (benchmarkTestEnable)
        {
            string filePath = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/Logs/RiskReportBusiness_GetDataSourceParallel_Benchmark_{0}.csv", DateTime.Now.ToString("yyyy-MM-dd_HHmmss")));
            ParallelBenchmarkTester.ComparePerformance(requests, numberCalls, filePath);
        }

        if (useEnhanced)
        {
            return GetDataSourceParallel_Enhanced(requests);
        }
        else
        {
            return GetDataSourceParallel_Legacy(requests);
        }        
    }

    public List<DataSourceParallelResponse> GetDataSourceParallel_Legacy(List<DataSourceParallelRequest> requests)
    {
        int maxThread = ConfigurationManager.AppSettings["MaxThread"].IsNullOrEmpty() ? 4
            : ConfigurationManager.AppSettings["MaxThread"].ToInt();
        _ThreadCompleteCount = 0;
        int clientId = SessionManager.CurrentClient;
        AsyncCaller Caller = new AsyncCaller();
        List<DataSourceParallelResponse> dataSourceResponse = new List<DataSourceParallelResponse>();
        CallbackMethod DoNothingCallBack = new CallbackMethod(DoCallback);
        var totalRequest = requests.Count;

        if (totalRequest < maxThread)
            maxThread = totalRequest;

        List<MethodCaller> callerList = new List<MethodCaller>();
        List<CallbackMethod> callbackList = new List<CallbackMethod>();

        using (var metrics = new MetricsLoggerManager(logPrefix: "LEGACY"))
        {
            foreach (var thread in requests)
            {
                MethodCaller methodCaller = new MethodCaller(() =>
                {
                    ReportServices service = WebServices.RiskServices;
                    service.AddRequestHeader("ClientId", clientId.ToString());

                    foreach (var spa in thread.SpasInfo)
                    {
                        DataTable dt = metrics.Log<DataTable>(
                            spa.SpaName,
                            () => service.GetReports(spa.SpaName, spa.Parameters),
                            spa.Parameters
                        );

                        dataSourceResponse.Add(new DataSourceParallelResponse()
                        {
                            FeatureName = spa.FeatureName,
                            DataSource = dt
                        });
                    }
                    return null;
                });

                callerList.Add(methodCaller);
                callbackList.Add(DoNothingCallBack);

                //Each excution contains max thread number.
                if (callerList.Count == maxThread)
                {
                    Caller.Start(callerList, callbackList);
                    callerList = new List<MethodCaller>();
                    callbackList = new List<CallbackMethod>();
                }
            }

            Caller.Start(callerList, callbackList);

            //Wait for utill all datasources are ready
            while (_ThreadCompleteCount < totalRequest)
            {
                Thread.Sleep(10);
            }
        }

        return dataSourceResponse;
    }

    public List<DataSourceParallelResponse> GetDataSourceParallel_Enhanced(List<DataSourceParallelRequest> requests)
    {
        if (requests == null || requests.Count == 0)
            return new List<DataSourceParallelResponse>();

        int maxThread;
        string maxThreadStr = ConfigurationManager.AppSettings["MaxThread"];
        if (string.IsNullOrEmpty(maxThreadStr))
            maxThread = 4;
        else
            int.TryParse(maxThreadStr, out maxThread);


        int clientId = SessionManager.CurrentClient;


        List<SpaInfo> spaItems = new List<SpaInfo>();
        foreach (var req in requests)
        {
            if (req.SpasInfo != null)
                spaItems.AddRange(req.SpasInfo);
        }

        var responses = new ConcurrentBag<DataSourceParallelResponse>();
        int globalIndex = 0;

        using (var metrics = new MetricsLoggerManager(logPrefix: "ENHANCED"))
        {
            Parallel.ForEach(
                spaItems,
                new ParallelOptions { MaxDegreeOfParallelism = maxThread },
                spa =>
                {
                    int currentIndex = Interlocked.Increment(ref globalIndex);
                    try
                    {
                        var service = WebServices.RiskServices;
                        service.AddRequestHeader("ClientId", clientId.ToString());

                        DataTable dt = metrics.Log<DataTable>(
                            spa.SpaName,
                            () => service.GetReports(spa.SpaName, spa.Parameters),
                            spa.Parameters
                        );

                        if (dt == null)
                            return;

                        responses.Add(new DataSourceParallelResponse
                        {
                            Index = currentIndex,
                            FeatureName = spa.FeatureName,
                            DataSource = dt
                        });
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Error(string.Format(
                            "[ENHANCED] SPA={0} failed: {1}",
                            spa.SpaName,
                            ex.Message));

                        responses.Add(new DataSourceParallelResponse
                        {
                            Index = currentIndex,
                            FeatureName = spa.FeatureName,
                            DataSource = new DataTable()
                        });
                    }
                });
        }

        List<DataSourceParallelResponse> ordered = responses.OrderBy(r => r.Index).ToList();
        return ordered;
    }


    public void DoCallback(CallbackArgs args)
    {
        _ThreadCompleteCount++;
        if (args.Status == MethodCallStatus.Exception)
        {
            LoggerManager.Error(args.Exception.ToString());
        }
    }
    #endregion
}

public class DataSourceParallelResponse
{
    public int Index { get; set; }
    public string FeatureName { get; set; }
    public DataTable DataSource { get; set; }
}

public class DataSourceParallelRequest
{
    public int Thread { get; set; }
    public List<SpaInfo> SpasInfo { get; set; }
}

public class SpaInfo
{
    public string FeatureName { get; set; }
    public string SpaName { get; set; }
    public FilterParameterCollection Parameters { get; set; }
}

public static class ParallelBenchmarkTester
{
    public static void ComparePerformance(
        List<DataSourceParallelRequest> requests,
        int runs,
        string outputCsvPath)
    {

        RiskReportBusiness business = new RiskReportBusiness();

        List<long> legacyDurations = new List<long>();
        List<long> enhancedDurations = new List<long>();

        Console.WriteLine("Starting benchmark test...");
        Console.WriteLine(string.Format("Test runs: {0}", runs));
        Console.WriteLine(string.Format("Requests: {0}", requests.Count));
        Console.WriteLine();

        for (int i = 0; i < runs; i++)
        {
            Console.WriteLine(string.Format("Run #{0}", i + 1));



            // Run ENHANCED
            Stopwatch swEnhanced = Stopwatch.StartNew();
            business.GetDataSourceParallel_Enhanced(requests);
            swEnhanced.Stop();
            enhancedDurations.Add(swEnhanced.ElapsedMilliseconds);
            Console.WriteLine(string.Format("   ENHANCED : {0} ms", swEnhanced.ElapsedMilliseconds));

            // Run LEGACY
            Stopwatch swLegacy = Stopwatch.StartNew();
            business.GetDataSourceParallel_Legacy(requests);
            swLegacy.Stop();
            legacyDurations.Add(swLegacy.ElapsedMilliseconds);
            Console.WriteLine(string.Format("   LEGACY   : {0} ms", swLegacy.ElapsedMilliseconds));

            Console.WriteLine();
        }

        // Summary
        double legacyAvg = Average(legacyDurations);
        double legacyStd = StdDev(legacyDurations);
        double enhancedAvg = Average(enhancedDurations);
        double enhancedStd = StdDev(enhancedDurations);

        Console.WriteLine("=== Benchmark Summary ===");
        Console.WriteLine(string.Format("LEGACY   : Avg = {0:F2} ms | StdDev = {1:F2}", legacyAvg, legacyStd));
        Console.WriteLine(string.Format("ENHANCED : Avg = {0:F2} ms | StdDev = {1:F2}", enhancedAvg, enhancedStd));

        double diff = enhancedAvg - legacyAvg;
        double diffPercent = (legacyAvg > 0) ? (diff / legacyAvg) * 100 : 0;
        Console.WriteLine(string.Format("➡️ Difference: {0:F2} ms ({1:F2}%)", diff, diffPercent));

        // Export CSV
        try
        {
            using (StreamWriter writer = new StreamWriter(outputCsvPath, false))
            {
                writer.WriteLine("Run,Legacy(ms),Enhanced(ms)");
                for (int i = 0; i < runs; i++)
                {
                    writer.WriteLine(string.Format("{0},{1},{2}",
                        i + 1,
                        legacyDurations[i],
                        enhancedDurations[i]));
                }

                writer.WriteLine();
                writer.WriteLine(string.Format("Average,{0:F2},{1:F2}", legacyAvg, enhancedAvg));
                writer.WriteLine(string.Format("StdDev,{0:F2},{1:F2}", legacyStd, enhancedStd));
                writer.WriteLine(string.Format("Diff(ms),,{0:F2}", diff));
                writer.WriteLine(string.Format("Diff(%) ,,{0:F2}", diffPercent));
            }

            Console.WriteLine(string.Format("\nResults exported to: {0}", outputCsvPath));
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format("Failed to write CSV: {0}", ex.Message));
        }
    }

    private static double Average(IEnumerable<long> values)
    {
        if (values == null) return 0;
        int count = 0;
        double sum = 0;
        foreach (var v in values)
        {
            sum += v;
            count++;
        }
        if (count == 0) return 0;
        return sum / count;
    }

    private static double StdDev(IEnumerable<long> values)
    {
        if (values == null) return 0;
        var list = new List<long>(values);
        if (list.Count == 0) return 0;
        double avg = Average(list);
        double sumSq = 0;
        foreach (var v in list)
        {
            sumSq += Math.Pow(v - avg, 2);
        }
        return Math.Sqrt(sumSq / list.Count);
    }
}