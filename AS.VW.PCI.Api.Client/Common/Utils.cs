using AS.VW.Api.RestClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace AS.VW.PCI.Api.Client.Common
{
    public static class Utils
    {
        /// <summary>
        /// Gets the standard request headers. Get hard header from tag requestHeaders on config file
        /// </summary>
        /// <param name="apiSetting">The API setting.</param>
        /// <returns></returns>
        public static IDictionary<string, string> GetStandardRequestHeaders(ApiSetting apiSetting)
        {
            var standardRequestHeaders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var requestHeaders = apiSetting.GetApiSettingSection("requestHeaders");
            var settings = requestHeaders == null ? null : requestHeaders.Settings;
            if (settings == null || settings.Count == 0)
            {
                return standardRequestHeaders;
            }

            foreach (var setting in settings)
            {
                standardRequestHeaders.Add(setting.Key, setting.Value.Value);
            }

            return standardRequestHeaders;
        }

        public static string GetTrackingId()
        {
            return (HttpContext.Current == null || HttpContext.Current.Session == null) ? Guid.NewGuid().ToString() : HttpContext.Current.Session.SessionID;
        }

        public static string GetServerMapPath(string includePath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(includePath);
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, includePath);
        }

        public static IDictionary<string, string> GetCamelcaseName(IDictionary<string, string> dic)
        {
            if (dic == null || dic.Count == 0)
                return dic;

            var dicStr = JsonConvert.SerializeObject(dic, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = true,
                        OverrideSpecifiedNames = true
                    }
                }
            });

            Dictionary<string, string> newDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(dicStr);

            return newDic;
        }

        public static T DeepClone<T>(T obj)
        {
            if (obj == null)
                return default(T);
            var serializeObj = JsonConvert.SerializeObject(obj);
            var cloneObj = JsonConvert.DeserializeObject<T>(serializeObj);
            return cloneObj;
        }
    }
}
