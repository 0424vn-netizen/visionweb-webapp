using AS.Common.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace AS.VW.DownloadDocuments
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                LoggerManager.Debug("App Start Download Document");
                Console.Write("App Start Download Document \n");

                DateTime requestTracking = DateTime.Now;
                //Get list document.
                TaskManager taskManager = new TaskManager();
                var dataProcess = taskManager.GetDocumentList(AppConfigurations.ClientIdDefault);
                int totalRow = dataProcess.Rows.Count;
                LoggerManager.Debug("Total Document Download:" + totalRow);
                if (totalRow <= 0) return;

                Console.Write("Processing ... \n");
                List<Task> listTask = new List<Task>();
                string mainFolder = string.Format("{0}\\Folder", AppConfigurations.DocumentTempFolder);
                int pageIndex = 0;
                int numberTask = 0;
                int pageSize = AppConfigurations.NumOfDocumentInFolder;
                int totalPage = (totalRow + pageSize - 1)/pageSize;
                do
                {
                    var pagingItem = dataProcess.AsEnumerable().Skip(pageIndex * pageSize).Take(pageSize).CopyToDataTable();
                    string documentFolderName = string.Format("{0}{1}", mainFolder, pageIndex);
                    if (!Directory.Exists(documentFolderName))
                    {
                        Directory.CreateDirectory(documentFolderName);
                    }

                    foreach (DataRow item in pagingItem.Rows)
                    {
                        string documentId = item["DocServerId"].ToString();
                        string documentName = item["DocName"].ToString();

                        //Check if file exist in folder: Not process download
                        string existFile = string.Format("{0}\\{1}", documentFolderName, documentName);
                        if (File.Exists(existFile)) continue;

                        while (numberTask >= AppConfigurations.MaxThread)
                        {
                            Task.WaitAll(listTask.ToArray());
                            numberTask = 0; // Reset task list
                        }

                        numberTask++;
                        var task = new Task(() =>
                        {
                            DownloadDocument(documentFolderName, documentId, documentName);
                        });

                        listTask.Add(task);
                        task.Start();
                    }

                    Task.WaitAll(listTask.ToArray());

                    totalPage--;
                    pageIndex++;
                }
                while (totalPage > 0);

                Console.Write("App End");
                TimeSpan durationRequest = DateTime.Now.Subtract(requestTracking);
                LoggerManager.Debug("App End Download Document: Time Tracking - " + (int)durationRequest.TotalMinutes);
                LoggerManager.Debug("Have Dowload File Error - " + isHasError + "\n");

            }
            catch (Exception ex)
            {
                LoggerManager.Error("Get list document : Failed.\n" + ex.ToString());
            }
        }

        static bool isHasError = false;
        private static void DownloadDocument(string documentFolderName,string documentId, string documentName)
        {
            long docId = 0;
            long.TryParse(documentId, out docId);
            try
            {
                byte[] buffers = AppConfigurations.DocumentService.DownloadDoc(docId);
                string fileSaveLocation = Path.Combine(string.Format("{0}\\{1}", documentFolderName, documentName));
                File.WriteAllBytes(fileSaveLocation, buffers);
            }
            catch(Exception ex)
            {
                isHasError = true;
                LoggerManager.Error("Error - Document Id: " + documentId + " Document Name: " + documentName);
                LoggerManager.Error("Download document : Failed.\n" + ex.ToString());
            }
            
        }
    }
}
