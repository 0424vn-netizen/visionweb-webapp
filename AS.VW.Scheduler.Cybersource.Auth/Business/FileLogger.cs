using AS.VW.Scheduler.Cybersource.Auth.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Cybersource.Auth.Business
{
    public class FileLogger
    {
        private ClientConfig ClientConfig { get; set; }        
        private DateTime ReportDate { get; set; } = DateTime.Now;
        public FileLogger(ClientConfig config)
        {
            ClientConfig = config;
        }
        public void WriteInvalidFile(string text)
        {
            var fileName = $"{ClientConfig.PrefixFileName}{ReportDate.ToString("yyyyMMddHHmmss")}_Invalid";
            var outputFolder = GetOutputFolder();
            var filePath = Path.Combine(outputFolder, fileName);
            var invalidContent = text;

            if (!string.IsNullOrEmpty(invalidContent))
                File.WriteAllText(filePath, invalidContent);
        }
        public void WriteSummaryFile(int totalRecords, int processedTotalNumber, int inValidNumber)
        {
            var fileName = $"{ClientConfig.PrefixFileName}{ReportDate.ToString("yyyyMMddHHmmss")}_Summary";
            var outputFolder = GetOutputFolder();
            var filePath = Path.Combine(outputFolder, fileName);
            var summary = new StringBuilder();
            summary.AppendLine($"Total Records from Api: {totalRecords}");
            summary.AppendLine($"Processed Total Records: {processedTotalNumber}");
            summary.AppendLine($"Valid Records: {processedTotalNumber - inValidNumber}");
            summary.AppendLine($"Invalid Records: {inValidNumber}");
            File.WriteAllText(filePath, summary.ToString());
        }

        public void WriteDebugFile(string fileName, string content)
        {
            var outputFolder = GetDebugFolder();
            var filePath = Path.Combine(outputFolder, fileName);
            File.WriteAllText(filePath, content);
        }
        
        public string GetOutputFolder()
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string outputName = "/App_Data/Output";
            var outputFolder = Path.GetFullPath($"{rootPath}{outputName}");
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            return outputFolder;
        }
        public string GetDebugFolder()
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string outputName = "/App_Data/Debug";
            var outputFolder = Path.GetFullPath($"{rootPath}{outputName}");
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            return outputFolder;
        }
    }
}
