using AS.Web.DocServer;
using System;
using System.IO;

namespace AS.VW.DownloadDocuments
{
    public class AppConfigurations
    {
        public static string GetAppConfigValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
        
        private static Services documentWebService = null;
        public static Services DocumentService
        {
            get
            {
                if (documentWebService == null)
                {
                    documentWebService = new Services(AppConfigurations.DocServerURL, AppConfigurations.DocServerDowloadURL);
                }
                return documentWebService;
            }
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
        }

        public static string ClientIdDefault
        {
            get { return GetAppConfigValue("CLIENT_ID"); }
        }
        
        public static string BaseDirectory
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static int NumOfDocumentInFolder
        {
            get
            {
                int number = 500;
                if (GetAppConfigValue("NUMBER_OF_DOCUMENT_IN_FORDER") != null)
                    int.TryParse(GetAppConfigValue("NUMBER_OF_DOCUMENT_IN_FORDER"), out number);

                return number;
            }
        }
        
        public static string DocumentTempFolder
        {
            get
            {
                if (GetAppConfigValue("Document_Temp_Folder") != null)
                {
                    string expTempFolder = GetAppConfigValue("Document_Temp_Folder");
                    string directory = string.Empty;
                    if (expTempFolder.StartsWith("~"))
                    {
                        directory = BaseDirectory;
                        expTempFolder = expTempFolder.Replace("~", "");
                    }

                    string physicalFolder = string.Format("{0}{1}", directory, expTempFolder);
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
                        string physicalFolder = string.Format("{0}\\{1}\\{2}", BaseDirectory, "Document_Temp_Folder", DateTime.Now.Ticks);
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
        }
        
        public static int MaxThread
        {
            get { return int.Parse(GetAppConfigValue("MAXIMUM_THREAD")); }
        }
    }
}
