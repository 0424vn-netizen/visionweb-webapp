using AS.VW.Repository;
using System;
using System.IO;

namespace AS.VW.PDFStatementCreator
{
    public static class FileHandler
    {
        public static string SaveFile(byte[] content, string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                string _path = AppConfigurations.ExportTempFolder;
                fileName = _path + "\\" + Guid.NewGuid().ToString();
            }
               
            File.WriteAllBytes(fileName, content);            
            return fileName;
        }

        public static byte[] ReadFile(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }      
    }
}
