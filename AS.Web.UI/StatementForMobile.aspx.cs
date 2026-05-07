using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using AS.Common;
using WebSupergoo.ABCpdf9;

/// <summary>
/// This function is for only download SecondStatementFile and HistoricalStatement
/// The priority is: SecondStatementFile -> HistoricalStatement -> DocId (handld in mobile site)
/// </summary>
public partial class StatementForMobile : ReportPage
{
    // Init PDF file
    private int docH = 770;
    private int docW = 630;
    private int margin = 25;
    private int marginTB = 12;
    private int headerHeight = 0;
    private string headerCssText = @"       
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
                </style>&nbsp;";

    private string _MerchantNumber;
    private string SecondStatementFile;
    private string HistoricalStatement;
    private DateTime _ReportDate;

    private FilterParameterCollection _parameter = null;
    DataTable table = new DataTable();
    private DateTime dt { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        _MerchantNumber = Request["merchantNumber"];
        _ReportDate = new DateTime(long.Parse(Request["reportDate"]));
        SecondStatementFile = Request["secondStatement"];
        HistoricalStatement = Request["hierarchyValue"];

        //if (!string.IsNullOrWhiteSpace(SecondStatementFile))
        //{
        //    // To do
        //}
        if (!string.IsNullOrWhiteSpace(HistoricalStatement))
        {
            ViewStatementFISByData(_ReportDate, HistoricalStatement);
        }
    }

    /// <summary>
    /// Download Historical Statement
    /// </summary>
    /// <param name="reportDate">Report date</param>
    /// <param name="hierarchyFilterValue">Hierarchy Filter Value</param>
    private void ViewStatementFISByData(DateTime reportDate, string hierarchyFilterValue)
    {
        string urlstm = string.Empty;
        string processor = this.GetBEProcessor(hierarchyFilterValue, reportDate);
        if (!processor.IsNullOrEmpty())
        {
            urlstm = this.GetStatementURL(processor);
        }
        // If TYSY
        if (string.IsNullOrEmpty(urlstm))
        {
            ExportPDF();
        }
    }

    /// <summary>
    /// Get processor type
    /// </summary>
    /// <param name="merchantNumber">Merchant ID</param>
    /// <param name="date">The date</param>
    /// <returns></returns>
    private string GetBEProcessor(string merchantNumber, DateTime date)
    {
        DataTable dt = WebServices.CsReportServices.GetBEProcessor(SessionManager.CurrentUser.ASClient,
            SessionManager.CurrentUser.SiteID, merchantNumber, date);
        if (dt != null && dt.Rows.Count > 0)
        {
            return dt.Rows[0]["BEProcessor"].ToString();
        }
        return string.Empty;
    }

    /// <summary>
    /// Get statement url
    /// </summary>
    /// <param name="processor">Processor type</param>
    /// <returns>Statement url</returns>
    protected string GetStatementURL(string processor)
    {
        foreach (DataRow dr in SessionManager.Processors.Rows)
        {
            if ((int)dr["ASClientID"] == SessionManager.CurrentUser.ASClient
                && (processor.IsNullOrEmpty()
                    || dr["BEProcessor"].ToString().Equals(processor, StringComparison.OrdinalIgnoreCase)))
            {
                return dr["StatementURL"].ToString();
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// Build parameters for spa
    /// </summary>
    private void BuildParameters()
    {
        dt = _ReportDate;
        _parameter = new FilterParameterCollection();
        _parameter.AddLoggedInUserReportingParams();
        _parameter.Add(new FilterParameter("@DateFilterMode", 2, DbType.Int32));
        _parameter.Add(new FilterParameter("@BeginDate", dt, DbType.DateTime));
        _parameter.Add(new FilterParameter("@EndDate", dt, DbType.DateTime));
        _parameter.Add(new FilterParameter("@MerchantNumber", _MerchantNumber, DbType.AnsiString));
    }

    /// <summary>
    /// Render Html tag
    /// </summary>
    /// <returns>The Html contains data</returns>
    private string RenderHtmlText()
    {
        BuildParameters();
        table = WebServices.CsReportServices.GetReports("spa_GetStatements_FIS", _parameter);

        StringBuilder buider = new StringBuilder();
        buider.Append(@"<pre class='PrintMode'>");
        for (int k = 0; k < table.Rows.Count; k++)
        {
            buider.Append("&nbsp;" + table.Rows[k]["LineData"].ToString());
            buider.Append(@"<br/>");
        }
        buider.Append(@"</pre>");
        buider = buider.Replace("\t", @"&#09;");
        return VeraCodeSolution.DoVeraCode(buider.ToString());
    }

    /// <summary>
    /// Export pdf file
    /// </summary>
    private void ExportPDF()
    {
        string trueFileName = "Statement_" + _MerchantNumber + "_" + _ReportDate.Month.ToString("0#") + "_" + _ReportDate.Year.ToString();

        Doc theDoc = new Doc();
        theDoc.HtmlOptions.Engine = EngineType.Gecko;

        theDoc.MediaBox.Width = docW;
        theDoc.MediaBox.Height = docH;
        
        
        theDoc.Rect.SetRect(margin - 8, docH - marginTB - headerHeight, docW - margin * 2, headerHeight);
        theDoc.AddImageHtml(headerCssText);

        theDoc.Rect.SetRect(margin, marginTB, docW - margin * 2, docH - marginTB * 2 - headerHeight);

        var htmlText = RenderHtmlText();
        int theID = theDoc.AddImageHtml(htmlText);
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
        string header = "attachment; filename=\"" + trueFileName + ".pdf\"";
        Response.AddHeader("content-disposition", VeraCodeSolution.RemoveCRLF(header));
        theDoc.Save(Response.OutputStream);
        Response.End();
    }
}