using AS.Core.Common.Log;
using AS.NetCore.Api.Client.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace AS.NetCore.Api.Client.BaseClient
{
    public partial class ApiConsumer
    {
        public static string ApiConfigFile { get; set; }
        protected readonly AppSetting _appSetting;
        public int ResponseCode { get; set; }
        public class ApiSetting
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public int? CacheTime { get; set; }
            public string RootUrl { get; set; }
            public int? DelayTime { get; set; }
            public string ApiToken { get; set; }

            public ApiSetting Clone()
            {
                var result = new ApiSetting();
                result.Code = this.Code;
                result.Name = this.Name;
                result.RootUrl = this.RootUrl;
                if (CacheTime != null && CacheTime.HasValue) result.CacheTime = CacheTime.Value;
                if (DelayTime != null && DelayTime.HasValue) result.DelayTime = DelayTime.Value;
                result.ApiToken = this.ApiToken;
                return result;
            }
        }
        public Dictionary<string, ApiSetting> LoadApiConfig(string configFile)
        {
            configFile = Path.GetFullPath(configFile);
            var key = configFile.Replace(@"\", "_").Replace(" ", "_");

            if (HttpRuntime.Cache[key] == null)
            {
                var configs = LoadConfig(configFile);
                HttpRuntime.Cache.Insert(key, configs, new System.Web.Caching.CacheDependency(configFile));
                return configs;
            }
            return HttpRuntime.Cache[key] as Dictionary<string, ApiSetting>;
        }
        public Dictionary<string, ApiSetting> LoadConfig(string configFile)
        {
            var configs = new Dictionary<string, ApiSetting>();
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.Load(configFile);

            XmlNodeList groupSettingNotes = doc.SelectNodes("//groupSetting");
            foreach (XmlNode node in groupSettingNotes)
            {
                ApiSetting groupSetting = new ApiSetting();

                if (node.Attributes["rootUrl"] != null)
                    groupSetting.RootUrl = node.Attributes["rootUrl"].Value;
                
                if (node.Attributes["apiToken"] != null)
                    groupSetting.ApiToken = node.Attributes["apiToken"].Value;

                XmlNodeList apis = node.SelectNodes("api");
                foreach (XmlNode api in apis)
                {
                    var setting = groupSetting.Clone();

                    setting.Code = api.Attributes["code"].Value.ToLower();
                    setting.Name = api.Attributes["name"].Value.ToLower();

                    if (api.Attributes["rootUrl"] != null)
                        setting.RootUrl = api.Attributes["rootUrl"].Value;

                    if (api.Attributes["apiToken"] != null)
                        setting.ApiToken = api.Attributes["apiToken"].Value;

                    if (string.IsNullOrEmpty(setting.RootUrl))
                        setting.RootUrl = null;
                    else
                        setting.RootUrl = setting.RootUrl.Trim('/') + '/';
                    configs.Add(setting.Code, setting);
                }
            }
            return configs;
        }

        private Dictionary<string, ApiSetting> s_ApiConfig { get { return LoadApiConfig(ApiConfigFile); } }
        public ApiConsumer(AppSetting appSetting)
        {
            var rootUrl = AppDomain.CurrentDomain.BaseDirectory;
            var apiUrl = Path.Combine(rootUrl, "App_Data/ApiSettings/api-settings.xml");
            _appSetting = appSetting;
            ApiConfigFile = apiUrl;
        }
        public ApiConfiguration ApplyConfig(string url)
        {
            int cachedTime = 0;
            int delayTime = 0;
            string apiToken = string.Empty;
            string configKey = GetConfigKey(url);
            if (s_ApiConfig != null && (
                    s_ApiConfig.ContainsKey(configKey)
                    || s_ApiConfig.ContainsKey("*")
                ))
            {
                var specifyConfig = s_ApiConfig.ContainsKey(configKey) ? s_ApiConfig[configKey] : null;
                var globalConfig = s_ApiConfig.ContainsKey("*") ? s_ApiConfig["*"] : null;

                if (!url.StartsWith("http"))
                {
                    if (specifyConfig != null && specifyConfig.RootUrl != null)
                        url = specifyConfig.RootUrl + specifyConfig.Name;
                    else if (globalConfig != null && globalConfig.RootUrl != null)
                        url = globalConfig.RootUrl + url;
                }

                if (specifyConfig != null && specifyConfig.ApiToken != null)
                    apiToken = specifyConfig.ApiToken;
                else if (globalConfig != null && globalConfig.ApiToken != null)
                    apiToken = globalConfig.ApiToken;
            }
            return new ApiConfiguration { CachedTime = cachedTime, Url = url, DelayTime = delayTime, ApiToken = apiToken };
        }
        
        private string GetConfigKey(string url)
        {
            if (url.Contains("?")) url = url.Substring(0, url.IndexOf("?"));
            return url.ToLower();
        }
        public T Execute<T>(string endpointKey, string method = "GET", object requestData = null, int cachedTime = 0, string apiToken = "") where T : DataModel, new()
        {
            T obj = new T();
            var config = ApplyConfig(endpointKey);
            if (cachedTime == 0)
            {
                cachedTime = config.CachedTime;
            }

            dynamic result = HttpWebExecute(config.Url, method: method, requestData: requestData, cachedTime: cachedTime, apiToken: config.ApiToken);
            if (result != null)
            {
                obj = JsonConvert.DeserializeObject<T>(result.ToString());
            }
            return obj;
        }
        public dynamic HttpWebExecute(string endpointKey, string method = "GET", object requestData = null, int cachedTime = 0, string apiToken = "")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (_appSetting.ApiByPassServerCertificateValidation)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyError) =>
                {
                    return _appSetting.ApiByPassServerCertificateValidation;
                };
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpointKey);
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.UseDefaultCredentials = true;

            request.Method = method;
            request.UserAgent = "Aperia-HttpClient";
            request.Accept = "*/*";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.5");
            request.Headers.Add(HttpRequestHeader.Authorization.ToString(), apiToken);

            if (_appSetting.ApiTimeoutInSec > 0)
            {
                request.Timeout = _appSetting.ApiTimeoutInSec * 1000;
            }

            if (requestData != null)
            {
                request.ContentType = "application/json";
                byte[] data = Serialize(requestData);
                request.ContentLength = data.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            HttpWebResponse response = null;
            Stream responseStream = null;
            MemoryStream mem = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                byte[] buffer = new byte[1024 * 1024];
                responseStream = response.GetResponseStream();
                mem = new MemoryStream();
                int readBytes;

                while ((readBytes = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    mem.Write(buffer, 0, readBytes);

                ResponseCode = (int)response.StatusCode;
                buffer = mem.ToArray();

                dynamic output = OnResponse(response, buffer);
                return output;
            }
            catch (Exception ex)
            {
                Logger.Error($"FAILED EXECUTE REQUEST. ApiUrl:{endpointKey} - ResponseCode: {ResponseCode}", ex);
                return null;
            }
            finally
            {
                stopwatch.Stop();

                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }

                if (responseStream != null)
                {
                    responseStream.Dispose();
                }

                if (mem != null)
                {
                    mem.Dispose();
                }
            }
        }

        protected virtual void OnRestRequest(RestRequest request, object requestData)
        {
            if (_appSetting.ApiByPassServerCertificateValidation)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyError) =>
                {
                    return _appSetting.ApiByPassServerCertificateValidation;
                };
            }
        }
        protected byte[] Serialize(object data)
        {
            string json = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(json);
        }
        protected dynamic OnResponse(HttpWebResponse response, byte[] data)
        {
            HttpStatusCode statusCode = response.StatusCode;
            string textResponse = Encoding.UTF8.GetString(data);

            if (200 <= (int)statusCode && (int)statusCode <= 299 && response.ContentType.Contains("json"))
            {
                return textResponse;
            }

            throw new HttpListenerException((int)response.StatusCode, $"url: {response.ResponseUri} - Status code {(int)response.StatusCode}-{response.StatusCode} with Response Type: {response.ContentType} and Response: {textResponse}");
        }
    }
}
