using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.IO;
using System.Text;
using Microsoft.Web.Services3.Security.Tokens;
using AS.Common.WSE;
using AS.Common;
using AS.Common.DataProtection;
using AS.WS.Business;
using AS.Common.DBManager;
using AS.WS.Entities;

public partial class DownloadExportedFile : System.Web.UI.Page
{
    private string ExportFolder
    {
        get
        {

            if (ConfigurationManager.AppSettings["ExportTempFolder"] != null)
            {
                string expTempFolder = ConfigurationManager.AppSettings["ExportTempFolder"];
                bool isNetworkPath = expTempFolder.StartsWith("\\\\");

                if (!isNetworkPath)
                {
                    if (expTempFolder.Contains("~/"))
                        expTempFolder = expTempFolder.Replace("~/", string.Empty);

                    expTempFolder = HttpContext.Current.Server.MapPath("~/" + expTempFolder);
                }

                if (!Directory.Exists(expTempFolder))
                {
                    Directory.CreateDirectory(VeraCodeSolution.DoVeraCode(expTempFolder));
                }

                return expTempFolder;
            }
            else
            {
                string physicalFolder = HttpContext.Current.Server.MapPath("~/ExportFiles");
                Directory.CreateDirectory(VeraCodeSolution.DoVeraCode(physicalFolder));
                return physicalFolder;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.Params["IsMgmt"]))
        {
            string[] reportTypeArray = { "WorkedDetail", "WorkedSummary", "AssignmentVolumeSummary", "AssignmentAlertSummary" };
            string[] exportTypeArray = { "excel", "csv" };
            string reportType = Cryptophy.DecryptText(Request.Params["reportType"]);
            string exportType = Cryptophy.DecryptText(Request.Params["exportType"]);
            string culture = Cryptophy.DecryptText(Request.Params["culture"]);
            string currencyFormat = Cryptophy.DecryptText(Request.Params["currencyFormat"]);
            string reportTitle = "";
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter("@IsExport", 1, DbType.Int32));
            foreach (string name in Request.Form)
            {
                if (name == "ReportTitle")
                {
                    reportTitle = Encoding.UTF8.GetString(Convert.FromBase64String(Request.Form[name].ToString()));
                }
                else
                {
                    string strPostValue = Request.Form[name].ToString();
                    int paramTypeIndex = strPostValue.IndexOf("[[");
                    string strParamValue = strPostValue.Substring(0, paramTypeIndex);
                    string strParamType = strPostValue.Substring(paramTypeIndex + 2, strPostValue.Length - paramTypeIndex - 4);
                    DateTime valueDateTime;
                    int valueInt;
                    if (DateTime.TryParse(DecryptPostData(strParamValue), out valueDateTime))
                    {
                        parameters.Add(new FilterParameter("@" + name, valueDateTime, GetDbType(strParamType)));
                    }
                    else if (int.TryParse(DecryptPostData(strParamValue), out valueInt))
                    {
                        parameters.Add(new FilterParameter("@" + name, valueInt, GetDbType(strParamType)));
                    }
                    else
                    {
                        parameters.Add(new FilterParameter("@" + name, DecryptPostData(strParamValue), GetDbType(strParamType)));
                    }
                }
            }

            if ((Array.IndexOf(reportTypeArray, reportType) > -1) && (Array.IndexOf(exportTypeArray, exportType) > -1))
            {
                string filePath = this.ExportFolder.ToString();
                string templatePath = Server.MapPath("~/App_Data/");
                ReportingBusiness reportBusiness = new ReportingBusiness();
                reportBusiness.InitializeForRisk(GeneralFuncsLib.GetConnStringSettings(Request.Headers["ClientId"], "RM_DBCONN"));

                var resourceDictionary = GeneralFuncsLib.GetAllReportResources(culture);
                //38605 - Enable NRT Risk module
                bool isNRTRisk = !string.IsNullOrEmpty(Request.Params["IsNRTRisk"]) && bool.Parse(Request.Params["IsNRTRisk"]);
                var reportRequest = new ExportRiskReportRequest()
                {
                    ReportTitle = reportTitle,
                    ReportType = reportType,
                    ExportType = exportType,
                    FilePath = filePath,
                    TemplatePath = templatePath,
                    Resources = resourceDictionary,
                    IsNRTRisk = isNRTRisk,
                    CurrencyFormat = currencyFormat
                };
                string fileName = reportBusiness.ExportDataForRiskManagementReporting(reportRequest, parameters);
                DownloadFile(fileName);
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(Request.Params["FileName"]))
            {
                string userName = string.Empty;
                string password = string.Empty;
                if (!string.IsNullOrEmpty(Request.Headers["vtfs3883"])) //plain text: token1
                    userName = Request.Headers["vtfs3883"];
                if (!string.IsNullOrEmpty(Request.Headers["qbttxpse3883"])) //plain text: token2
                    password = Request.Headers["qbttxpse3883"];

                bool checkResult = CheckParameterBeforeDownloadFile(userName, password);

                if (checkResult)
                {
                    string fileName = Request.Params["FileName"];
                    DownloadFile(Path.Combine(this.ExportFolder, fileName));
                }
                else
                {
                    Response.Write("ConCoBeBeNoDauCanhTre");
                    Response.End();
                }
            }
        }
    }
    private string DecryptPostData(string strInput)
    {
        return Cryptophy.DecryptText(strInput);
    }

    private DbType GetDbType(string typeName)
    {
        return (DbType)Enum.Parse(typeof(DbType), typeName);
    }
    /// <summary>
    /// Checks the parameter before download file.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <param name="password">The password.</param>
    /// <param name="ip">The ip.</param>
    private bool CheckParameterBeforeDownloadFile(string userName, string password)
    {
        UsernameToken token = new UsernameToken(userName, password, PasswordOption.SendPlainText);
        WebServicesSecurity wsSec = new WebServicesSecurity();
        return wsSec.AuthentkToken(token);
    }

    private void DownloadFile(string fileName)
    {

        FileStream iStream = null;
        int BUFFER_LENGTH = GetBufferLength();
        // Buffer to read 500Kbs in chunk:
        byte[] buffer = new Byte[BUFFER_LENGTH];
        // Length of the file:
        int length;
        // Total bytes to read:
        long dataToRead;
        try
        {
            // Open the file.
            iStream = new System.IO.FileStream(VeraCodeSolution.DoVeraCode(fileName), System.IO.FileMode.Open,
                        System.IO.FileAccess.Read, System.IO.FileShare.Read);


            // Total bytes to read:
            dataToRead = iStream.Length;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Length", VeraCodeSolution.RemoveCRLF(iStream.Length.ToString()));
            // Read the bytes.
            while (dataToRead > 0)
            {
                // Verify that the client is connected.
                if (HttpContext.Current.Response.IsClientConnected)
                {
                    // Read the data in buffer.
                    length = iStream.Read(buffer, 0, BUFFER_LENGTH);

                    // Write the data to the current output stream.
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);

                    // Flush the data to the HTML output.
                    HttpContext.Current.Response.Flush();

                    buffer = new Byte[BUFFER_LENGTH];
                    dataToRead = dataToRead - length;
                }
                else
                {
                    //prevent infinite loop if user disconnects
                    dataToRead = -1;
                }
            }
        }
        catch (Exception ex)
        {
            // Trap the error, if any.
            Response.Write(ex.Message);
            AS.Common.Logger.LoggerManager.Error("Downloading process error:\n" + ex.ToString());
        }
        finally
        {
                //Close the file.
                if (iStream != null)
                    iStream.Close();
                //delete file
                if (File.Exists(VeraCodeSolution.DoVeraCode(fileName)))
                    File.Delete(VeraCodeSolution.DoVeraCode(fileName));
                //end the response
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            
        }
    }

    private int GetBufferLength()
    {
        string bufferLenStr = ConfigurationManager.AppSettings["ExportTransferBufferLength"];
        if (string.IsNullOrEmpty(bufferLenStr))
            return 500000;      //default is 500KB
        else
            return int.Parse(bufferLenStr);
    }

}
