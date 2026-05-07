using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using AS.Common.DataProtection;

public partial class rm_MCF_ExportFlatReport : NonReportPage
{
    private string _AssignmentName
    {
        get
        {

            if (!string.IsNullOrEmpty(SecureQueryString["AssignmentName"]))
            {
                return Convert.ToString(SecureQueryString["AssignmentName"]);
            }
            else
            {
                return string.Empty;
            }
        }
    }

    private string _Header { get { return string.Format("{0} ({1})", _AssignmentName, _ReportDate.ToString(WebSiteConstants.DATE_FORMAT)); } }

    private DateTime _ReportDate
    {
        get
        {

            if (!string.IsNullOrEmpty(SecureQueryString["ReportDate"]))
            {
                return Convert.ToDateTime(SecureQueryString["ReportDate"]);
            }
            else
            {
                return DateTime.Now;
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Header.Visible = false;
    }

    protected override void Render(HtmlTextWriter writer)
    {
        exportFile();
    }

    private void exportFile()
    {
        if (IsIntruderDetected) return;
        string fileName = string.Empty;
        if (SecureQueryString != null && !string.IsNullOrEmpty(SecureQueryString["fn"]))
        {
            fileName = SecureQueryString["fn"];
        }
        else
        {
            return;
        }
        string urlWS = ConfigurationManager.AppSettings["ExportWSDownloadUrl"] + "?FileName=" + fileName + "&IsNRTRisk=True";
        const int MAX_BUFFER = 10024;
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.CreateDefault(new Uri(urlWS));
        request.Method = "POST";
        request.ContentLength = 0;
        string userName = Cryptophy.DecryptText(ConfigurationManager.AppSettings["RM_Report_WS_Token1"]);
        string passWord = Cryptophy.DecryptText(ConfigurationManager.AppSettings["RM_Report_WS_Token2"]);

        request.Headers.Add("vtfs3883", userName);
        request.Headers.Add("qbttxpse3883", passWord);

        Response.Clear();
        try
        {
            Response.ClearHeaders();
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error("ClearHeaders: rm_MCF_ExportFlatReport - TransferFileToClient:\n" + ex.ToString());
        }
        Response.ContentType = "application/octet-stream";
        string fileNameDownload = GeneralFuncsLib.FormatFileName(_Header) + ".xls";
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlPathEncode(fileNameDownload)));

        WebResponse response = request.GetResponse();
        System.IO.Stream stream = response.GetResponseStream();
        byte[] buffer = new byte[MAX_BUFFER];
        int readBytes = 0;
        int totalBytes = 0;
        do
        {
            readBytes = stream.Read(buffer, 0, buffer.Length);
            Response.OutputStream.Write(buffer, 0, readBytes);
            //Response.BinaryWrite(buffer);
            Response.Flush();
            totalBytes += readBytes;

        }
        while (readBytes > 0 && totalBytes < response.ContentLength);
        stream.Close();
        Response.End();
    }
}
