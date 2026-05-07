using System.Collections.Generic;

namespace AS.NetCore.Api.Client.Models
{
    public class AppSetting
    {
        public string AppName { get; set; }
        public string RootUrl { get; set; }
        public string LoginUrl { get; set; }
        public string HomeUrl { get; set; }
        public string ErrorUrl { get; set; }
        public string AccessDeniedUrl { get; set; }
        public string AuthCookie { get; set; }
        public string SessionName { get; set; }
        public string CookieName { get; set; }
        public int UserSessionExpiredTime { get; set; }
        public int ApiTimeoutInSec { get; set; } = 20;
        public int SessionCacheTime { get; set; }
        public int ApiExportTimeoutInSec { get; set; } = 100;
        public string EnvironmentName { get; set; }
        public bool ApiByPassServerCertificateValidation { get; set; } = true;
        public string WsDataEncryptKey { get; set; }

        public Dictionary<string, string> BusinessSettings { get; set; }

        public string FeatureSettings { get; set; }

        public string DomainName { get; set; }

        public LoginMode LoginMode { get; set; }

        public string GetBusinessSetting(string key, string defaultValue = "")
        {
            if (BusinessSettings?.ContainsKey(key) == true)
            {
                return BusinessSettings[key];
            }
            return defaultValue;
        }
        public int ApiSlownessInSec { get; set; } = 1;
    }

    public enum LoginMode
    {
        LdapOnly = 0,
        MockOnly = 1,
        Both = 2
    }
}
