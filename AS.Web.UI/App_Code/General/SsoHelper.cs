using System.IO;
using System.Web;
using AS.SSO;
using Newtonsoft.Json;

namespace As.VisionWeb.Web.Helper
{
    public static class SsoHelper
    {
        public static SsoConfiguration GetSsoConfiguration()
        {
            var physicalFilePath = HttpContext.Current.Server.MapPath("~/App_Data/SAML/sso.config.json");
            SsoConfiguration config;

            if (!File.Exists(physicalFilePath))
            {
                return null;
            }

            var key = physicalFilePath.Replace(@"\", "_").Replace(" ", "_");

            if (HttpRuntime.Cache[key] == null)
            {
                var fileCotent = File.ReadAllText(physicalFilePath);
                config = JsonConvert.DeserializeObject<SsoConfiguration>(fileCotent);
                HttpRuntime.Cache.Insert(key, fileCotent, new System.Web.Caching.CacheDependency(physicalFilePath));
            }
            else
            {
                var content = HttpRuntime.Cache[key].ToString();
                config = JsonConvert.DeserializeObject<SsoConfiguration>(content);
            }

            return config;
        }
    }
}
