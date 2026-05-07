using System.Configuration;

namespace AS.StatementPDFReportCommon
{
    public static class Configs
    {
        public static string DocServicesDownloadUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["DocServer_DownloadUrl"];
            }
        }
    }
}
