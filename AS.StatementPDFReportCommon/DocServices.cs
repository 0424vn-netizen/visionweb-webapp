using System.IO;
using System.Net;
using System.Text;

namespace AS.StatementPDFReportCommon
{
    public static class DocServices
    {
        static string _downloadUrl = Configs.DocServicesDownloadUrl;
       
        public static byte[] DownloadDoc(int? docId)
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
    }
}
