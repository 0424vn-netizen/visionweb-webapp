using AS.Core.Common.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace AS.ApiClient.UnderWriting
{
    public static class Settings
    {
        public static string ApiUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["UnderWritingApiUrl"];
            }
        }
        public static int RequestTimeout
        {
            get
            {
                return ConfigurationManager.AppSettings["RequestTimeout"] == null ? 10000 : Convert.ToInt32(ConfigurationManager.AppSettings["RequestTimeout"]);
            }
        }
        public static bool ServerCertificateValidation
        {
            get
            {
                return ConfigurationManager.AppSettings["ServerCertificateValidation"] == null || Convert.ToBoolean(ConfigurationManager.AppSettings["ServerCertificateValidation"]);
            }
        }
        public static string ApiSettingPath
        {
            get
            {
                var rootUrl = AppDomain.CurrentDomain.BaseDirectory;
                var configUrl = ConfigurationManager.AppSettings["UnderWritingApiSetting"] ?? "/App_Data/ApiSettings/api-settings.xml";
                return Path.GetFullPath($"{rootUrl}{configUrl}");
            }
        }
        public static string ApiToken
        {
            get
            {
                return ConfigurationManager.AppSettings["UnderWritingApiToken"].ToString();
            }
        }
        public static List<ApiSetting> GetApiConfigFile()
        {            
            var key = ApiSettingPath.Replace(@"\", "_").Replace(" ", "_");

            if (HttpRuntime.Cache[key] == null)
            {
                var configs = LoadApiConfig(ApiSettingPath);
                HttpRuntime.Cache.Insert(key, configs, new System.Web.Caching.CacheDependency(ApiSettingPath));
                return configs;
            }

            return (HttpRuntime.Cache[key]) as List<ApiSetting>;
        }
        public static List<ApiSetting> LoadApiConfig(string configFile)
        {
            if (!File.Exists(configFile))
            {
                Logger.Error("ERROR Under Writing Api: Configuration file is not exist.");
                return new List<ApiSetting>();
            }

            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.Load(configFile);
            var settingConfigs = new List<ApiSetting>();
            XmlNodeList groupSettingNotes = doc.SelectNodes("//groupSetting");
            foreach (XmlNode node in groupSettingNotes)
            {
                ApiSetting groupSetting = new ApiSetting();

                if (node.Attributes["rootUrl"] != null)
                    groupSetting.RootUrl = node.Attributes["rootUrl"].Value;                

                if (node.Attributes["cacheTime"] != null)
                    groupSetting.CacheTime = int.Parse(node.Attributes["cacheTime"].Value);               

                XmlNodeList apis = node.SelectNodes("api");
                foreach (XmlNode api in apis)
                {
                    var setting = groupSetting.Clone();

                    setting.Code = api.Attributes["code"].Value.ToLower();
                    setting.Name = api.Attributes["name"].Value.ToLower();

                    if (api.Attributes["cacheTime"] != null)
                        setting.CacheTime = int.Parse(api.Attributes["cacheTime"].Value);                    

                    if (api.Attributes["rootUrl"] != null)
                        setting.RootUrl = api.Attributes["rootUrl"].Value;

                    if (string.IsNullOrEmpty(setting.RootUrl))
                        setting.RootUrl = null;
                    else
                        setting.RootUrl = setting.RootUrl.Trim('/') + '/';

                    settingConfigs.Add(setting);
                }
            }

            return settingConfigs;
        }
    }

    public class ApiSetting
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int? CacheTime { get; set; }
        public bool? MockData { get; set; }
        public string RootUrl { get; set; }
        public int? DelayTime { get; set; }
        public ApiSetting Clone()
        {
            var result = new ApiSetting();
            result.Code = this.Code;
            result.Name = this.Name;
            result.RootUrl = this.RootUrl;
            if (CacheTime != null && CacheTime.HasValue) result.CacheTime = CacheTime.Value;
            return result;
        }
    }
}
