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
using System.Collections.Generic;
using System.Text;
using System.IO;
using AS.Common;
using System.Text.RegularExpressions;
using WebSupergoo.ABCpdf9;

public partial class rm_MCF_ExportAssignment : NonReportPage
{
    private  string EXPORT_FILE_HEADER = string.Empty;
    bool isExporting = true;
    private  string FILE_NAME = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        EXPORT_FILE_HEADER = GetLocalResourceObject("rm_ExportAssignment_aspx_cs_ExportHeader").ToString();
        FILE_NAME = GetLocalResourceObject("rm_ExportAssignment_aspx_cs_FileName").ToString();
        if (Request.UrlReferrer == null)
        {
            Response.Redirect("rm_MCF_Assignment_Management.aspx");
        }
        this.IsNoCache = false;
        Page.Header.Visible = false;
        if (!IsPostBack)
        {
            if( this.SecureQueryString != null)
            {
                string str = this.SecureQueryString["exp"];
                bool hasQueuingMechanism = this.SecureQueryString["hasQueuing"].ToBoolean();
                if (str != "c")
                {
                    SetVisibileRepeater(hasQueuingMechanism);
                    BindData(hasQueuingMechanism);
                }
            }
        }
    }
    protected override void Render(HtmlTextWriter writer)
    {
        if (this.SecureQueryString != null)
        {
            string htmlString = GetHtmlOutput(writer);
            if (SecureQueryString["exp"] == "p")
                ExportToPDF(htmlString);
            else if (SecureQueryString["exp"] == "e")
                ExportToExcel(htmlString);
            else if (SecureQueryString["exp"] == "c")
            {
                ExportToCSV(FILE_NAME, EXPORT_FILE_HEADER, (StringBuilder)SessionManager.DataCSV);
                SessionManager.DataCSV = null;
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="headerText"></param>
    /// <param name="content"></param>
    private static void ExportToCSV(string fileName, string headerText, StringBuilder content)
    {
        //response object init 
        string attachment = "attachment; filename=" + fileName + ".csv";
        HttpContext.Current.Response.Clear();
        try
        {
            HttpContext.Current.Response.ClearHeaders();
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error("ClearHeaders: rm_MCF_ExportAssignment - ExportToCSV:\n" + ex.ToString());
        }
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", AS.Common.VeraCodeSolution.RemoveCRLF(attachment));
        HttpContext.Current.Response.ContentType = "text/csv";
        HttpContext.Current.Response.AddHeader("Pragma", "public");
        //write common header
        content.Insert(0, Environment.NewLine);
        content.Insert(0, Environment.NewLine);
        content.Insert(0, headerText);

        HttpContext.Current.Response.Write(content.ToString());
        HttpContext.Current.Response.End();
    }
    private void ExportToPDF(string htmlString)
    {
        string storedFile = System.Guid.NewGuid().ToString();
        htmlString = htmlString.Replace("thin", "1px");
        htmlString = htmlString.Replace("font-family:Arial !important;", "font-family:Times New Roman !important;font-size:17px;");
        htmlString = htmlString.Replace(GeneralFuncsLib.BaseUrl, Request.PhysicalApplicationPath);
        Regex regex = new Regex(@"<script [^>]*>[\s\S]*?</script>");
        htmlString = regex.Replace(htmlString, "");

        Regex regex2 = new Regex(@"<input [\s\S]*?/>");
        htmlString = regex2.Replace(htmlString, "");

        htmlString = htmlString.Replace("<tbody>", "");
        htmlString = htmlString.Replace("</tbody>", "");
        htmlString = htmlString.Replace("<thead>", "");
        htmlString = htmlString.Replace("</thead>", "");

        ExportPDF(htmlString, Server.MapPath(WebSiteSettings.ExportTempFolder + storedFile + ".pdf"));
        TransferFileToClient(Server.MapPath(WebSiteSettings.ExportTempFolder + storedFile + ".pdf"), FILE_NAME + ".pdf");
    }
    private void ExportToExcel(string htmlString)
    {
        htmlString = Regex.Replace(htmlString, "<link(.|\n)*?/>", string.Empty);//remove link tag
        htmlString = Regex.Replace(htmlString, "<input(.|\n)*?/>", string.Empty);//remove input tag
        Response.Clear(); //this clears the Response of any headers or previous output
        Response.Buffer = true; //make sure that the entire output is rendered simultaneously
        Response.ContentType = "application/vnd.ms-excel";
        string filename = Server.MapPath(ConfigurationManager.AppSettings["ExportTempFolder"] + FILE_NAME);
        HttpContext.Current.Response.AppendHeader("content-disposition", AS.Common.VeraCodeSolution.RemoveCRLF(string.Format("attachment; filename={0}.xls", FILE_NAME)));
        Response.Write(htmlString);
        Response.End();
    }
    public string GetHtmlOutput(HtmlTextWriter writer)
    {
        if (!isExporting)
        {
            base.Render(writer);
            return string.Empty;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter hWriter = new HtmlTextWriter(sw);

        base.Render(hWriter);
        string pageResult = sb.ToString();
        writer.Write(pageResult);

        return pageResult;
    }

    public void TransferFileToClient(string fileName, string exportedName)
    {
        HttpResponse response = HttpContext.Current.Response;
        response.Clear();
        try
        {

            response.ContentType = "application/pdf";
            string header = "attachment; filename=" + exportedName;
            //TODO: remember to do veracode VeraCodeSolution.RemoveCRLF(header)
            response.AddHeader("content-disposition", VeraCodeSolution.RemoveCRLF(header));
            response.TransmitFile(fileName);
            response.Flush();
        }
        catch
        {
            //TODO: write log here.
        }
        finally
        {

            if (File.Exists(fileName))
                File.Delete(fileName);
            response.End();
        }
    }
    public void ExportPDF(string html, string fileName)
    {
        Doc theDoc = new Doc();

        int theID = 0;
        // set up document
        html = html.Trim();
        theDoc.Rect.SetRect(15, 15, 587, 765);
        theID = theDoc.AddImageHtml(html);
        theDoc.Rect.SetRect(15, 15, 587, 715);

        for (int i = 0; i < 500; i++)
        {
            if (theDoc.GetInfo(theID, "Truncated") != "1") break;
            theDoc.Page = theDoc.AddPage();
            theID = theDoc.AddImageToChain(theID);
        }

        int theCount = theDoc.PageCount;
        theDoc.PageNumber = 1;
        theDoc.Rect.SetRect(0, -110, 588, 750);
        theDoc.FontSize = 12;
        theDoc.HPos = 1;
        theDoc.PageNumber = 1;
        theDoc.Save(fileName);
        theDoc.Clear();
    }
    private void BindData(bool hasQueuingMechanism)
    {
        if (SessionManager.AssignmentGroupTables != null && SessionManager.HeaderList != null)
        {
            Repeater uxRepeater = hasQueuingMechanism ? uxRequeuedReportRepeater : uxReportRepeater;
            List<DataTable> tables = (List<DataTable>)SessionManager.AssignmentGroupTables;
            List<string> headers = (List<string>)SessionManager.HeaderList;
            DataTable tblHeaders = new DataTable();
            tblHeaders.Columns.Add(new DataColumn("RowTitle"));
            foreach (string s in headers)
            {
                DataRow row = tblHeaders.NewRow();
                row["RowTitle"] = s;
                tblHeaders.Rows.Add(row);
            }
            uxRepeater.DataSource = tblHeaders;
            uxRepeater.DataBind();
            if (tblHeaders.Rows.Count > 0)
            {
                int index = 0;
                foreach (Control rptReportItem in uxRepeater.Controls)
                {
                    if (rptReportItem is RepeaterItem)
                    {
                        foreach (Control ctrl in ((RepeaterItem)rptReportItem).Controls)
                        {
                            if (ctrl is Repeater && ctrl.ID == "uxDataRepeater")
                            {
                                Repeater dataRepeater = ctrl as Repeater;
                                dataRepeater.DataSource = tables[index];
                                dataRepeater.DataBind();
                            }
                        }
                        index++;
                    }
                }
            }
            SessionManager.AssignmentGroupTables = null;
            SessionManager.HeaderList = null;
        }
        //else
        //{
        //    return;
        //}
    }
    protected string FormatDate(object obj)
    {
        if (obj != DBNull.Value)
            return ((DateTime)obj).ToShortDateString();
        return "";
    }
    protected string FormatDateTime(object obj)
    {
        if (obj != DBNull.Value)
            return ((DateTime)obj).ToString("MM/dd/yyyy hh:mm:ss tt");
        return "";
    }
    private void SetVisibileRepeater(bool hasQueuingMechanism)
    {
        uxReportRepeater.Visible = !hasQueuingMechanism;
        uxRequeuedReportRepeater.Visible = hasQueuingMechanism;
    }
}
