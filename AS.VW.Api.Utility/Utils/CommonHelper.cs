using System;
using System.Configuration;
using System.Globalization;

namespace AS.VW.Api.Utility.Utils
{
    public static class CommonHelper
    {
        public static T GetConfig<T>(string configName, T defaultValue)
        {
            var config = ConfigurationManager.AppSettings[configName];
            if (string.IsNullOrEmpty(config))
                return defaultValue;

            return (T)Convert.ChangeType(config, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
