using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace AS.Web.Business
{
    public class DocServices
    {
        readonly DocWebServices.DocProcessor _ws;
        readonly string _downloadUrl;
        public DocServices(string url, string downloadUrl)
        {
            _ws = new DocWebServices.DocProcessor();
            _ws.Url = url;
            _downloadUrl = downloadUrl;
        }
        public long UploadDoc(int clientID, string userID, byte[] buffers, string fileName, string serverIP)
        {           
            return UploadDoc(clientID, userID, buffers, fileName, serverIP, "1099K-Doc");            
        }

        public long UploadDoc(int clientID, string userID, byte[] buffers, string fileName, string serverIP, string docCategory)
        {
            long docId = 0;
            string strStatus, strStorageFile;
            _ws.UploadFileWithDocCategoryEncrypted(clientID.ToString(), userID, docCategory, fileName, "", "", "", buffers, serverIP, out docId, out strStatus, out strStorageFile);
            return docId;
        }
        public byte[] DownloadDoc(int? docId)
        {
            byte[] buffer = new byte[8192];
            string errorMessageDetails = string.Empty; 
            try
            {
                string postData = string.Format("op={0}&param={1}", "id", docId.ToString());
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(_downloadUrl + "?" + postData);
                myRequest.Method = "GET";
                HttpWebResponse myHttpWebResponse = null;                      
                myHttpWebResponse = (HttpWebResponse)myRequest.GetResponse();
                Stream readStream = myHttpWebResponse.GetResponseStream();
                MemoryStream mainBuff = new MemoryStream();
                int bytesRead = 1;
                do
                {
                    bytesRead = readStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        mainBuff.Write(buffer, 0, bytesRead);
                        mainBuff.Flush();
                    }
                } while (bytesRead > 0);
                buffer = mainBuff.ToArray();
                mainBuff.Close();
                myHttpWebResponse.Close();
            }
            catch (WebException error)
            {
                errorMessageDetails = new StreamReader(error.Response.GetResponseStream()).ReadToEnd();
                errorMessageDetails = "This file is currently inaccessible."; 
            }
            return buffer;

        }
        public byte[] DownloadDocByFilePath(string filePath)
        {
            string postData = string.Format("op={0}&param={1}", "path", filePath);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(_downloadUrl + "?" + postData);
            myRequest.Method = "GET";
            byte[] buffer = new byte[8192];
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myRequest.GetResponse();
            Stream readStream = myHttpWebResponse.GetResponseStream();
            MemoryStream mainBuff = new MemoryStream();
            int bytesRead = 1;
            do
            {
                bytesRead = readStream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    mainBuff.Write(buffer, 0, bytesRead);
                    mainBuff.Flush();
                }
            } while (bytesRead > 0);
            buffer = mainBuff.ToArray();
            mainBuff.Close();
            myHttpWebResponse.Close();
            return buffer;

        }

        public bool DeleteFileOnDocServer(string clientID, string entityID, string docID)
        {
            bool success = false;
            try
            {
                success = _ws.DeleteDocumentByID(clientID, entityID, docID);
            }
            catch (Exception ex)
            {

                AS.Common.Logger.LoggerManager.Error("Delete file: Can't delete file on document server.\n" + ex.ToString());
            }
            return success;
        }
    }
}
