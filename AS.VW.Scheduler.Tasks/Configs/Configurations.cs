using System;
using System.IO;

namespace AS.VW.Scheduler.Tasks
{
    public static class AppConfigurations
    {
        public static string GetAppConfigValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
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

        public static uint NumOfRowsToFlush
        {
            get
            {
                uint number = 500;
                if (GetAppConfigValue("NUMBER_OF_ROWS_TO_FLUSH") != null)
                    uint.TryParse(GetAppConfigValue("NUMBER_OF_ROWS_TO_FLUSH"), out number);

                return number;
            }
        }

        public static int ThresholdToZip
        {
            get
            {
                int number = -1;
                if (GetAppConfigValue("THRESHOLD_TO_ZIP") != null)
                    int.TryParse(GetAppConfigValue("THRESHOLD_TO_ZIP"), out number);

                return number;
            }
        }

        public static int NumberOfRowToSwitch
        {
            get
            {
                int number = 1000000;
                if (GetAppConfigValue("NUMBER_OF_ROWS_TO_SWITCH_EXPORTTYPE") != null)
                    int.TryParse(GetAppConfigValue("NUMBER_OF_ROWS_TO_SWITCH_EXPORTTYPE"), out number);

                return number;
            }
        }

        public static int NumberOfRowPerFile
        {
            get
            {
                int number = 3000000;
                if (GetAppConfigValue("NUMBER_OF_ROWS_PER_PAGE") != null)
                    int.TryParse(GetAppConfigValue("NUMBER_OF_ROWS_PER_PAGE"), out number);

                return number;
            }
        }

        public static int ChunkSizeInKB
        {
            get
            {
                int number = 51200;
                if (GetAppConfigValue("CHUNK_SIZE_IN_KB") != null)
                    int.TryParse(GetAppConfigValue("CHUNK_SIZE_IN_KB"), out number);

                return number;
            }
        }

        public static int EXPORTREPORTS_PAGESIZE
        {
            get
            {
                int number = 1000;
                if (GetAppConfigValue("EXPORTREPORTS_PAGESIZE") != null)
                    int.TryParse(GetAppConfigValue("EXPORTREPORTS_PAGESIZE"), out number);

                return number;
            }
        }

        public static int EXPORTREPORTS_PAGES_PER_FILE
        {
            get
            {
                int number = 30;
                if (GetAppConfigValue("EXPORTREPORTS_PAGES_PER_FILE") != null)
                    int.TryParse(GetAppConfigValue("EXPORTREPORTS_PAGES_PER_FILE"), out number);

                return number;
            }
        }

        public static string ExportTempFolderReport
        {
            get
            {
                return GetExportTempFolderReport();
            }
        }
        private static string GetExportTempFolderReport()
        {
            if (GetAppConfigValue("Export_Temp_Folder") != null)
            {
                string expTempFolder = GetAppConfigValue("Export_Temp_Folder");
                string directory = string.Empty;
                if (expTempFolder.StartsWith("~"))
                {
                    directory = BaseDirectory;
                    expTempFolder = expTempFolder.Replace("~", "");
                }

                string physicalFolder = string.Format("{0}{1}\\{2}", directory, expTempFolder, DateTime.Now.Ticks);
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
                    string physicalFolder = string.Format("{0}\\{1}\\{2}", BaseDirectory, "ExportedFiles", DateTime.Now.Ticks);
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

        public static string ExtractMode
        {
            get { return GetAppConfigValue("EXTRACT_MODE"); }
        }

        public static bool IsDeleteUploadFile
        {
            get { return GetAppConfigValue("IS_DELETE_UPLOAD_FILE") == "true"; }
        }

        public static bool IsDebug
        {
            get { return GetAppConfigValue("IsDebug") == "true"; }
        }

        public static int MaxThread
        {
            get { return int.Parse(GetAppConfigValue("MAXIMUM_THREAD_EXPORT")); }
        }
    }
}
