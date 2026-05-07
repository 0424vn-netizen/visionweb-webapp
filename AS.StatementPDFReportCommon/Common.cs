using AS.Archive;
using AS.Common.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace AS.StatementPDFReportCommon
{
    public static class Common
    {
        public static void Zip(string zipFile, ICollection<string> files, string password)
        {
            DirectoryInfo dir = new DirectoryInfo(zipFile);

            if (!dir.Exists)
                dir.Create();

            SevenZip.SevenZipDllPath = Path.Combine(Application.StartupPath, "7z.dll");
            SevenZip.Pack(ArchiveType.SevenZip, zipFile, files, password);
        }

        public static bool CreateZipFile(string fileName, IList<string> files)
        {
            try
            { 
                var zip = ZipFile.Open(fileName, System.IO.Compression.ZipArchiveMode.Create);
                 
                foreach (string f in files)
                {
                    zip.CreateEntryFromFile(f, Path.GetFileName(f), CompressionLevel.Optimal);
                    
                }
                
                zip.Dispose(); 
                return true;
            }
            catch(Exception e)
            {
                LoggerManager.Error("Error within zipping file, error msg: " + e.Message + ", at: " + DateTime.Now);
                return false;
            }
        } 
    }
}
