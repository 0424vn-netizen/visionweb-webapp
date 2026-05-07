using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Web;
using System.Data;
using System.Reflection;

namespace AS.VW.Repository
{
    public static class AppConfigurations
    {       
        public static string GetAppConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];            
        }                
        public static string GetStringAppSettings(string key, string defaultValue = "")
        {
            var config = GetAppConfigValue(key);

            return config == null ? defaultValue : config.ToString();
        }
        public static int GetIntAppSettings(string key)
        {
            return GetIntAppSettings(key, 0);
        }
        public static int GetIntAppSettings(string key, int defaultValue)
        {
            var config = GetAppConfigValue(key);
            int intconfig;
            int.TryParse(config, out intconfig);

            if (intconfig == 0)
                intconfig = defaultValue;
            return intconfig;
        }        
        public static bool GetBoolAppSettings(string key, bool defaultValue = false)
        {
            var config = GetAppConfigValue(key);
            if (config == null)
                return defaultValue;

            return config == "true" || config == "1";
        }
        public static string ReportWSURL
        {
            get { return GetAppConfigValue("Report_WS_URL"); }
        }
        public static string ReportWSToken1
        {
            get { return GetAppConfigValue("Report_WS_Token1"); }
        }
        public static string ReportWSToken2
        {
            get { return GetAppConfigValue("Report_WS_Token2"); }
        }
        public static string DocServerURL
        {
            get { return GetAppConfigValue("DocServer_WS_URL"); }
        }
        public static string DocServerDowloadURL
        {
            get { return GetAppConfigValue("DocServer_DownloadUrl"); }
        }
        public static string ExportTempFolder
        {
            get
            {
                return GetExportTempFolder();
            }
        }
        private static string GetExportTempFolder()
        {
            if (GetAppConfigValue("Export_Temp_Folder") != null)
            {
                string expTempFolder = GetAppConfigValue("Export_Temp_Folder");
                string physicalFolder = BaseDirectory + expTempFolder;
                if (!Directory.Exists(physicalFolder))
                {
                    Directory.CreateDirectory(physicalFolder);
                }
                return physicalFolder;

            }
            else
            {
                try
                {
                    string physicalFolder = BaseDirectory + "ExportedFiles";
                    Directory.CreateDirectory(physicalFolder);
                    return physicalFolder;
                }
                catch
                {
                    throw new DirectoryNotFoundException("Task Error: cannot find the folder to generate file");
                }
            }
            throw new DirectoryNotFoundException("Task Error: cannot find the folder to generate file");
        }
        public static string ClientIdDefault
        {
            get { return GetAppConfigValue("Client_Id_Default"); }
        }
        public static int ExportThreadMax
        {
            get
            {
                int number = 10;
                if (GetAppConfigValue("Export_Thread_Max") != null)
                    int.TryParse(GetAppConfigValue("Export_Thread_Max"), out number);
                return number;
            }
        }
        public static string ExportFileType
        {
            get
            {
                if (GetAppConfigValue("Export_File_Type") != null)
                    return GetAppConfigValue("Export_File_Type");
                return ".csv";
            }
        }
        public static string BaseDirectory
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}
