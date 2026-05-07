using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Controls.Grid;
using Telerik.Web.UI;
using System.IO;
using System.Text;
using AS.Common;
using AS.Controls.Pages;
using AS.Web.Business;
using WebSupergoo.ABCpdf9;


[PagePermission("StatementRpt,MSStatementRpt")]
public partial class StatementDetail_FDR : NonReportPage
{
    string _SourceName = string.Empty;
    bool isExportPDF = false;
    string _KeyName = string.Empty;
    private FilterParameterCollection _parameter = null;
    DataTable table = new DataTable();
    private string MerchantNumber { get { return ViewState["vsMerchantNumber"] != null ? (string)ViewState["vsMerchantNumber"] : ""; } set { ViewState["vsMerchantNumber"] = value; } }
    private DateTime dt { get { return ViewState["vsDate"] != null ? (DateTime)ViewState["vsDate"] : DateTime.MinValue; } set { ViewState["vsDate"] = value; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;

        if (!Page.IsPostBack)
        {
            if (SecureQueryString["MerchantNumber"] != null)
            {
                if (!IsIntruderDetected) ProcessQueryString();
            }
            else
            {
                MerchantNumber = SessionManager.CurrentMerchantNumber;
                long temp;
                if (!long.TryParse(SecureQueryString["ReportDate"].ToString(), out temp))
                {
                    IsIntruderDetected = true;
                    return;
                }
                dt = new DateTime(long.Parse(SecureQueryString["ReportDate"]));
            }
            if (IsIntruderDetected) return;
            string EntityName = "" + GeneralFuncsLib.GetMerchantName(MerchantNumber);
            uxTitle.Text = VeraCodeSolution.ValidateResponseData(MerchantNumber + " - " + EntityName);
            uxTitle1.Text = VeraCodeSolution.DoVeraCode(dt.ToString("MMMM, yyyy"));
            JoinStrings();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    private void ProcessQueryString()
    {
        _SourceName = SecureQueryString[WebSiteConstants.INTRUDER_SOURCE_PARAM_NAME];
        _KeyName = SecureQueryString[WebSiteConstants.INTRUDER_KEY_PARAM_NAME];
        MerchantNumber = SecureQueryString["MerchantNumber"].ToString().Trim();
        string Temp = this.SecureQueryString["ReportDate"];
        long temp;
        if (!long.TryParse(SecureQueryString["ReportDate"].ToString(), out temp))
        {
            IsIntruderDetected = true;
            return;
        }
        dt = new DateTime(long.Parse(SecureQueryString["ReportDate"]));
        // continue to check data if intruder is not detected yet
        //CheckDataIntruders(_SourceName, _KeyName.Split(WebSiteConstants.INTRUDER_KEY_SEPERATOR.ToCharArray()), new object[] { dt, MerchantNumber });

    }
    protected override void DoPagePreInit()
    {
        base.DoPagePreInit();
        this.IsUsingTheme = false;
    }

    public Control GetPostBackControl(Page CurrentPage)
    {
        Control control = null;
        string controlName = Page.Request.Params.Get("__EVENTTARGET");
        if (controlName != null && controlName != string.Empty)
        {
            control = this.Page.FindControl(controlName);
        }
        else
        {

            foreach (string formControl in Page.Request.Form)
            {
                string temp = formControl.TrimEnd('x').TrimEnd('.');
                Control ctrl = Page.FindControl(temp);
                if (ctrl is System.Web.UI.WebControls.Button || ctrl is System.Web.UI.WebControls.ImageButton)
                {
                    control = ctrl;
                    break;
                }
            }
        }
        return control;
    }
    private void BuildParameters()
    {
        dt = new DateTime(long.Parse(SecureQueryString["ReportDate"]));
        _parameter = new FilterParameterCollection();
        _parameter.AddLoggedInUserReportingParams();
        _parameter.Add(new FilterParameter("@DateFilterMode", 2, DbType.Int32));
        _parameter.Add(new FilterParameter("@BeginDate", dt, DbType.DateTime));
        _parameter.Add(new FilterParameter("@EndDate", dt, DbType.DateTime));
        _parameter.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
    }
    private void JoinStrings()
    {
        BuildParameters();
        table = WebServices.CsReportServices.GetReports("spa_GetStatements", _parameter);

        StringBuilder buider = new StringBuilder();
        StringBuilder vb = new StringBuilder();//view content
        buider.Append(@"<pre class='PrintMode'>");
        vb.Append(@"<div style='text-align:left;height:100%; ' class = 'divTable' >");
        vb.Append(@"<pre style='overflow:hidden'>");
        for (int k = 0; k < table.Rows.Count; k++)
        {

            if (k != 0 && table.Rows[k]["LineBreak"].ToString().StartsWith("1"))
            {
                buider.Append("<div style='page-break-before:always;'>&nbsp;" + table.Rows[k]["LineData"].ToString() + "</div>");
                vb.Remove(vb.Length - 5, 5);
                vb.Append(@"</pre >");
                vb.Append(@"</div>");
                vb.Append("<div style='border-bottom:1px solid black;height:1px; width:100%'></div>");
                vb.Append(@"<div style='text-align:left;height:100%; ' class = 'divTable' >");
                vb.Append(@"<pre style='overflow:hidden'>");
                vb.Append("&nbsp;" + table.Rows[k]["LineData"].ToString());
                vb.Append(@"<br/>");
            }
            else
            {
                buider.Append("&nbsp;" + table.Rows[k]["LineData"].ToString());
                buider.Append(@"<br/>");
                vb.Append("&nbsp;" + table.Rows[k]["LineData"].ToString());
                vb.Append(@"<br/>");
            }
        }
        buider.Append(@"</pre>");
        vb.Append(@"</pre>");
        vb.Append(@"</div>");
        buider = buider.Replace("\t", @"&#09;");
        vb = vb.Replace("\t", @"&#09;");
        uxContentsSafari.Text = uxContents.Text = VeraCodeSolution.DoVeraCode(buider.ToString());
        uxViewContent.Text = VeraCodeSolution.DoVeraCode(vb.ToString());
    }
    protected void uxExportPDF_Click(object sender, ImageClickEventArgs e)
    {
        if (IsIntruderDetected) return;
        ExportToPDF();
    }
    protected void uxExportExcel_Click(object sender, ImageClickEventArgs e)
    {
        if (IsIntruderDetected) return;
        ExportToExcel();
    }
    private void ExportToPDF()
    {
        if (IsIntruderDetected) return;
        IsNoCache = false;
        isExportPDF = true;
    }
    private void ExportToExcel()
    {
        this.IsNoCache = false;
        this.divExport.Visible = false;
        this.divViewContent.Visible = false;
        this.divContent.Visible = false;
        this.tblContents.Visible = true;
        //bind data to data bound controls and do other stuff
        Response.Clear(); //this clears the Response of any headers or previous output
        Response.Buffer = true; //make sure that the entire output is rendered simultaneously
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used
        HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
        string trueFileName = GetLocalResourceObject("StatementDetail_FDR_aspx_cs_StatementFilename").ToString() + "_" + MerchantNumber + "_" + dt.Month.ToString("0#") + "_" + dt.Year.ToString();
        string filename = Server.MapPath(WebSiteSettings.ExportTempFolder + trueFileName);
        HttpContext.Current.Response.AppendHeader("content-disposition", AS.Common.VeraCodeSolution.RemoveCRLF(string.Format("attachment; filename={0}.xls", trueFileName)));

        this.RenderControl(htmlTextWriter);
        string htmlContent = stringWriter.ToString();
        htmlContent = htmlContent.Replace(@"<pre>", @"<pre style='font-family:Courier;'>");
        Regex rx = new Regex(@"<link\s*rel='stylesheet'\s*type='text/css'.*/>");
        if (rx.IsMatch(htmlContent))
        {
            htmlContent = rx.Replace(htmlContent, string.Empty);
        }
        //Response.Write(DDS.Common.VeraCodeSolution.GetOutputHtmlString(htmlContent));
        Response.Write(GetOriginalContent(htmlContent));
        Response.End();
    }
    private string GetOriginalContent(string content)
    {
        content = content.Replace("<div style='page-break-before:always;'>&nbsp;", "");
        content = content.Replace("</div>&nbsp;", "<br/>&nbsp;");
        return content;
    }
    protected override void OnPreRenderComplete(EventArgs e)
    {
        base.OnPreRenderComplete(e);

        if (isExportPDF)
        {
            divExport.Visible = false;
            string trueFileName = GetLocalResourceObject("StatementDetail_FDR_aspx_cs_StatementFilename").ToString() + "_" + MerchantNumber + "_" + dt.Month.ToString("0#") + "_" + dt.Year.ToString();

            Doc theDoc = new Doc();

            const int docH = 770;
            const int docW = 630;
            theDoc.MediaBox.Width = docW;
            theDoc.MediaBox.Height = docH;
            string headerText = string.Format(@"       
               <style>
                            .reporttitle {{
                            color:Black;
                            font-family:Arial;
                            font-size:19px;
                            font-weight:bold;
                            text-align:left;
                            }}
                            .gridtitle {{
                            color:Black;
                            font-family:Arial;
                            font-size:15px;
                            font-weight:bold;
                            }}
            </style>
                <div align=""left"">
                     <span class=""reporttitle"">{0}</span>
                </div>
                    <div align=""left"">
                        <span class=""gridtitle"">{1}&nbsp;</span>
                        {2}
                        <br />
                        <span class=""gridtitle"">{3}&nbsp;</span>
                        {4}
                        <br />
                    </div>
                    <br /> "
                        , GetLocalResourceObject("Literal1Resource1.Text").ToString()
                        , GetLocalResourceObject("Literal2Resource1.Text").ToString()
                        , uxTitle.Text
                        , GetLocalResourceObject("StatementDetail_FDR_aspx_cs_ReportDate").ToString()
                        , uxTitle1.Text);

            const int margin = 25;
            const int marginTB = 12;
            const int headerHeight = 70;
            theDoc.Rect.SetRect(margin - 8, docH - marginTB - headerHeight, docW - margin * 2, headerHeight);
            theDoc.AddImageHtml(headerText);

            theDoc.Rect.SetRect(margin, marginTB, docW - margin * 2, docH - marginTB * 2 - headerHeight);
            int theID = theDoc.AddImageHtml(uxContents.Text);
            theDoc.FrameRect();
            theDoc.Rect.SetRect(margin, marginTB, docW - margin * 2, docH - marginTB * 2);


            while (theDoc.Chainable(theID))
            {
                theDoc.Page = theDoc.AddPage();

                theID = theDoc.AddImageToChain(theID);
                theDoc.FrameRect();
            }

            theDoc.PageNumber = 1;
            this.IsNoCache = false;
            Response.ContentType = "application/pdf";
            string header = "attachment; filename=" + trueFileName + ".pdf";
            Response.AddHeader("content-disposition", AS.Common.VeraCodeSolution.RemoveCRLF(header));
            theDoc.Save(Response.OutputStream);
            Response.End();
            divExport.Visible = true;
        }

    }
}
