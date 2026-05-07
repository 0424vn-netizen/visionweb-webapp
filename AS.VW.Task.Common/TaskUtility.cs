using AS.Common;
using AS.VW.Task.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Task.Common
{
    public static class TaskUtility
    {
        /// <summary>
        /// Validate response data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ValidateResponseData(this string data)
        {
            return VeraCodeSolution.ValidateResponseData(data);
        }

        /// <summary>
        /// Format Saft URL
        /// </summary>
        /// <param name="url">url</param>
        public static string FormatSafeURL(this string url, bool removeRelativeSymbol = false)
        {
            if (removeRelativeSymbol)
            {
                url = url.Replace("~", string.Empty);
            }
            Uri uri = new Uri(url);
            string absolutePath = uri.AbsolutePath.Replace("\\", "/").Replace("//", "/");
            return uri.Scheme + "://" + uri.Authority + absolutePath;
        }

        /// <summary>
        /// Check if a object list has any values
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool HasData(this IEnumerable<object> objs)
        {
            return objs != null && objs.Any();
        }

        public static TimeSpan ExecuteWithWatch(this Action action)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Do action
            action();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        public static string GetConnection(int clientId)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string conectionPath = "/App_Data/Connection.json";
            var filePath = Path.GetFullPath($"{rootPath}{conectionPath}");
            var connections = GetJsonConfiguration<ConnectionModel>(filePath);            
            return connections.FirstOrDefault(x => x.Clients.Any(c => c == clientId))?.Connection;
        }
        public static List<T> GetJsonConfiguration<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<T>();

            return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(filePath));
        }
    }
}