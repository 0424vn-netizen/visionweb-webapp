using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Controls.Grid;
using Telerik.Web.UI;
using System.IO;
using System.Text;
using AS.Common;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Controls.Exporter;
using AS.Controls.UserControls;
using AS.Common.Logger;

[PagePermission("BatchRpt,MSBatchRpt")]
public partial class CardSummaryModal : ReportPage
{
    enum DataBindAction
    {
        BindReportGrid
    }
    string _BatchNumber = string.Empty;
    string _TerminalNumber = string.Empty;
    DateTime _ReportDate = DateTime.Today;
    string _SourceName = string.Empty;
    string _KeyName = string.Empty;
    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;
        this.ExporterIDs.Add("uxExporter");
        this.ExporterIDs.Add("uxExporter_Btm");
        base.PageInitialize();
    }
    private void ProcessQueryString()
    {
        LoggerManager.Info(string.Format("CardSummaryModal.aspx - ProcessQueryString - IsSecureQueryString ={0}", IsSecureQueryString));
        if (IsSecureQueryString)
        {
            _BatchNumber = SecureQueryString["BatchNumber"];
            LoggerManager.Info(string.Format("CardSummaryModal.aspx - ProcessQueryString - _BatchNumber ={0}", _BatchNumber));

            _TerminalNumber = SecureQueryString["TerminalNumber"];
            LoggerManager.Info(string.Format("CardSummaryModal.aspx - ProcessQueryString - _TerminalNumber ={0}", _TerminalNumber));

            _SourceName = SecureQueryString[WebSiteConstants.INTRUDER_SOURCE_PARAM_NAME];
            _KeyName = SecureQueryString[WebSiteConstants.INTRUDER_KEY_PARAM_NAME];

            string _Entity = SecureQueryString["Entity"];
            LoggerManager.Info(string.Format("CardSummaryModal.aspx - ProcessQueryString - _Entity ={0}", _Entity));

            string temp = this.SecureQueryString["ReportDate"];
            LoggerManager.Info(string.Format("CardSummaryModal.aspx - ProcessQueryString - temp(ReportDate) ={0}", temp));

            if (!String.IsNullOrEmpty(temp))
            {
                if (!DateTime.TryParse(temp, out _ReportDate))
                {
                    IsIntruderDetected = true;
                    return;
                }
            }
            // continue to check data if intruder is not detected yet
            if (!String.IsNullOrEmpty(_Entity))
            {
                CheckDataIntruders(_SourceName, _KeyName.Split(WebSiteConstants.INTRUDER_KEY_SEPERATOR.ToCharArray()), new object[] { _Entity });
            }
            else
            {
                CheckDataIntruders(_SourceName, _KeyName.Split(WebSiteConstants.INTRUDER_KEY_SEPERATOR.ToCharArray()), new object[] { _BatchNumber, _TerminalNumber });
            }
        }
    }

    private string _GridTitle()
    {
        string HierarchyModeValue = this.SavedReportFilterValue.HierarchyMode;
        string HierarchyValue = GetHierarchyValue();

        string GridTitleName = string.Empty;
        string HierarchyName = this.SavedReportFilterValue.HierarchyMode;//GetGridTitleName();
        string entityNumber = this.SecureQueryString["Entity"];
        string hierarchyMode = this.SecureQueryString["HierarchyMode"];

        string hierarchyModeValue = string.Empty;
        foreach (DataRow row in SessionManager.HierarchyFilterDrillDown.Rows)
        {
            if (row["CurrentHierarchyMode"].ToString() == hierarchyMode)
            {

                hierarchyModeValue = row["CurrentHierarchyGridName"].ToString();
            }
        }
        if (string.IsNullOrEmpty(HierarchyValue) || HierarchyValue == SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", hierarchyMode)["HierarchyPrefix"].ToString())
        {
            if (entityNumber.IsNullOrEmpty())
                GridTitleName = "All " + hierarchyMode + "s";// so nhieu
            else
            {
                if (HierarchyModeValue == GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode)
                {
                    GridTitleName = GetLocalResourceObject("CardSummaryModal_aspx_cs_Merchant").ToString() + ": " + entityNumber + " - " + GeneralFuncsLib.GetMerchantName(entityNumber);
                }
                else
                    GridTitleName = hierarchyModeValue + ": " + entityNumber;
            }
        }
        else
        {
            if (HierarchyModeValue == GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode)
            {
                GridTitleName = GetLocalResourceObject("CardSummaryModal_aspx_cs_Merchant").ToString() + ": " + HierarchyValue + " - " + GeneralFuncsLib.GetMerchantName(HierarchyValue);
            }
            else
            {
                if (entityNumber.IsNullOrEmpty())
                    GridTitleName = "All " + hierarchyMode + "s";
                else
                {
                    DataTable HierarchyFilterLevel = SessionManager.HierarchyFilterDrillDown;
                    DataRow row = HierarchyFilterLevel.FindObject("CurrentHierarchyMode", hierarchyMode);

                    string nextID = row["NextHierarchyID"].ToString();
                    string nextMode = row["NextHierarchyMode"].ToString();

                    string hierarchyNextModeValue = string.Empty;
                    foreach (DataRow irow in SessionManager.HierarchyFilterDrillDown.Rows)
                    {
                        if (irow["CurrentHierarchyMode"].ToString() == nextMode)
                        {

                            hierarchyNextModeValue = irow["CurrentHierarchyGridName"].ToString();
                        }
                    }

                    if (nextMode == GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode)
                    {
                        GridTitleName = GetLocalResourceObject("CardSummaryModal_aspx_cs_Merchant").ToString() + ": " + entityNumber + " - " + GeneralFuncsLib.GetMerchantName(entityNumber);
                    }
                    else
                        GridTitleName = hierarchyNextModeValue + ": " + entityNumber;
                }
            }
        }
        return GridTitleName;
    }
    private string HandleHeader(string hierarchyMode, string entityNumber)
    {
        DateTime fromDate = this.SavedReportFilterValue.DateOptionValue.From;
        DateTime toDate = this.SavedReportFilterValue.DateOptionValue.To;
        if (this.SavedReportFilterValue.DateOption == AS.Web.UI.Controls.DateOptionMode.Daily)
            toDate = fromDate;
        if (this.SavedReportFilterValue.DateOption == AS.Web.UI.Controls.DateOptionMode.Monthly)
        {
            fromDate = fromDate.GetFirstDayOfMonth();
            if (fromDate.Year == DateTime.Today.Year && fromDate.Month == DateTime.Today.Month)
                toDate = DateTime.Today;
            else
                toDate = fromDate.GetLastDayOfMonth();
        }
        string period = GetLocalResourceObject("CardSummaryModal_aspx_cs_ReportPeriod").ToString() + ": (" + fromDate.ToShortDateString() + " - " + toDate.ToShortDateString() + ")";
        return period;
    }
    public string GetGridHeader()
    {
        string oldHeader = uxExporter.GridHeader;
        string newHeader = String.Empty;
        FilterParameterCollection paras = new FilterParameterCollection();
        if (IsSecureQueryString)
        {
            string batchNumber = this.SecureQueryString["BatchNumber"];
            string entityNumber = this.SecureQueryString["Entity"];
            string hierarchyMode = this.SecureQueryString["HierarchyMode"];
            string terminalNumber = this.SecureQueryString["TerminalNumber"];
            string reportDate = this.SecureQueryString["ReportDate"];
            if (!String.IsNullOrEmpty(batchNumber))
            {
                DateTime newreportDate = Convert.ToDateTime(reportDate);
                string batchDetails = GetLocalResourceObject("CardSummaryModal_aspx_cs_BatchNumber").ToString() + ": " + batchNumber + "  " + GetLocalResourceObject("CardSummaryModal_aspx_cs_TerminalNumber").ToString() + ": " + terminalNumber + "  " + GetLocalResourceObject("CardSummaryModal_aspx_cs_ReportDate").ToString() + ": " + newreportDate.ToShortDateString();
                newHeader = batchDetails;
            }
            if (!String.IsNullOrEmpty(entityNumber) && !String.IsNullOrEmpty(hierarchyMode))
            {
                newHeader = HandleHeader(hierarchyMode, entityNumber);
            }
        }
        else
        {
            newHeader = HandleHeader(this.SavedReportFilterValue.HierarchyMode, GetHierarchyValue());

        }
        return newHeader;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!IsIntruderDetected) ProcessQueryString();
        if (IsIntruderDetected) return;

        IsBindDataOnLoad = true;
        if (!Page.IsPostBack)
        {
            string newHeader = GetGridHeader();
            string topHeader = String.Empty;
            if (IsSecureQueryString)
                ltrHeaderTop.Text = VeraCodeSolution.DoVeraCode(_GridTitle());
            uxExporter.GridHeader = VeraCodeSolution.ValidateResponseData(newHeader);
            uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(newHeader);
        }


    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender.ID == "uxReportGrid")
        {
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        ASGrid grid = (ASGrid)sender;
        FilterParameterCollection parameters = new FilterParameterCollection();

        string entityNumber = this.SecureQueryString["Entity"];
        LoggerManager.Info(string.Format("CardSummaryModal.aspx - OnDataBindControls - entityNumber ={0}", entityNumber));
        if (IsSecureQueryString)
        {
            string batchNumber = this.SecureQueryString["BatchNumber"];
            LoggerManager.Info(string.Format("CardSummaryModal.aspx - OnDataBindControls - batchNumber ={0}", batchNumber));

            string hierarchyMode = this.SecureQueryString["HierarchyMode"];//this.SavedReportFilterValue.HierarchyMode;
            LoggerManager.Info(string.Format("CardSummaryModal.aspx - OnDataBindControls - hierarchyMode ={0}", hierarchyMode));

            //if (hierarchyMode == "SYS" &&
            //           !string.IsNullOrEmpty(GetHierarchyValue()))
            //    entityNumber = entityNumber + "_" + GetHierarchyValue().Trim();
            string terminalNumber = this.SecureQueryString["TerminalNumber"];
            LoggerManager.Info(string.Format("CardSummaryModal.aspx - OnDataBindControls - terminalNumber ={0}", terminalNumber));

            string reportDate = this.SecureQueryString["ReportDate"];
            LoggerManager.Info(string.Format("CardSummaryModal.aspx - OnDataBindControls - reportDate ={0}", reportDate));

            ViewState["ReportDate"] = reportDate;

            parameters.AddLoggedInUserReportingParams();
            if (!String.IsNullOrEmpty(batchNumber))
            {
                DateTime newreportDate = Convert.ToDateTime(reportDate);
                parameters.Add("@HierarchyFilterMode", this.SavedReportFilterValue.HierarchyMode, DbType.AnsiString);
                parameters.Add("@HierarchyFilterValue", GetHierarchyValue(), System.Data.DbType.String);
                parameters.Add("@DateFilterMode", 1, DbType.Int32);
                parameters.Add("@BeginDate", newreportDate, DbType.DateTime);
                parameters.Add("@EndDate", newreportDate, DbType.DateTime);
                parameters.Add("@BatchNumber", batchNumber, DbType.String);
                parameters.Add("@TerminalNumber", terminalNumber ?? string.Empty, DbType.String);
                ViewState["Hierarchy"] = this.SavedReportFilterValue.HierarchyMode + "_" + GetHierarchyValue();
                //uxExporter.GridHeader = "Batch #: " + batchNumber + " Terminal #: " + terminalNumber + " Report Date: " + reportDate;
            }
            if (!String.IsNullOrEmpty(entityNumber) && !String.IsNullOrEmpty(hierarchyMode))
            {

                string nextMode = hierarchyMode;
                DataTable HierarchyFilterLevel = SessionManager.HierarchyFilterDrillDown;
                DataRow currentHierarchyRow = HierarchyFilterLevel.FindObject("CurrentHierarchyMode", hierarchyMode);
                if (!string.IsNullOrEmpty(GetHierarchyValue()) && GetHierarchyValue() != SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", currentHierarchyRow["CurrentHierarchyMode"].ToString())["HierarchyPrefix"].ToString())
                {
                    string nextID = currentHierarchyRow["NextHierarchyID"].ToString();
                    nextMode = currentHierarchyRow["NextHierarchyMode"].ToString();
                }

                if (SessionManager.CurrentUser.ASClient == 29
                    && nextMode.Equals("GROUP", StringComparison.OrdinalIgnoreCase)
                    && this.SavedReportFilterValue.HierarchyMode.Equals("POR", StringComparison.OrdinalIgnoreCase)
                    && (!string.IsNullOrEmpty(GetHierarchyValue()) && GetHierarchyValue() != SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", currentHierarchyRow["CurrentHierarchyMode"].ToString())["HierarchyPrefix"].ToString()))
                {
                    nextMode = "POR" + (char)241 + "GROUP";
                    entityNumber = GetHierarchyValue() + (char)241 + entityNumber;
                }

                parameters.Add("@HierarchyFilterMode", nextMode, System.Data.DbType.String);
                parameters.Add("@HierarchyFilterValue", entityNumber, System.Data.DbType.String);
                parameters.Add("@DateFilterMode", this.SavedReportFilterValue.DateOption, System.Data.DbType.Int32);
                parameters.Add("@BeginDate", this.SavedReportFilterValue.DateOptionValue.From, System.Data.DbType.DateTime);
                if (this.SavedReportFilterValue.DateOption == AS.Web.UI.Controls.DateOptionMode.DateRange)
                {
                    parameters.Add("@EndDate", this.SavedReportFilterValue.DateOptionValue.To, System.Data.DbType.DateTime);
                    ViewState["ReportDate"] = this.SavedReportFilterValue.DateOptionValue.From.ToShortDateString() + "_" + this.SavedReportFilterValue.DateOptionValue.To.ToShortDateString();
                }
                else
                {
                    parameters.Add("@EndDate", this.SavedReportFilterValue.DateOptionValue.From, System.Data.DbType.DateTime);
                    ViewState["ReportDate"] = this.SavedReportFilterValue.DateOptionValue.From.ToShortDateString() + "_" + this.SavedReportFilterValue.DateOptionValue.From.ToShortDateString();
                }
                ViewState["Hierarchy"] = nextMode + "_" + entityNumber;

            }
        }
        else
        {
            ViewState["Hierarchy"] = this.SavedReportFilterValue.HierarchyMode + "_" + GetHierarchyValue();
            parameters.Add("@HierarchyFilterMode", this.SavedReportFilterValue.HierarchyMode, DbType.AnsiString);
            parameters.Add("@HierarchyFilterValue", GetHierarchyValue(), System.Data.DbType.String);
            parameters.Add("@DateFilterMode", this.SavedReportFilterValue.DateOption, DbType.Int32);
            parameters.Add("@BeginDate", this.SavedReportFilterValue.DateOptionValue.From, DbType.DateTime);
            if (this.SavedReportFilterValue.DateOption == AS.Web.UI.Controls.DateOptionMode.DateRange)
            {
                parameters.Add("@EndDate", this.SavedReportFilterValue.DateOptionValue.To, System.Data.DbType.DateTime);
                ViewState["ReportDate"] = this.SavedReportFilterValue.DateOptionValue.From.ToShortDateString() + "_" + this.SavedReportFilterValue.DateOptionValue.To.ToShortDateString();
            }
            else
            {
                ViewState["ReportDate"] = this.SavedReportFilterValue.DateOptionValue.From.ToShortDateString() + "_" + this.SavedReportFilterValue.DateOptionValue.From.ToShortDateString();
                parameters.Add("@EndDate", this.SavedReportFilterValue.DateOptionValue.From, System.Data.DbType.DateTime);
            }
        }

        string spaName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    parameters.AddLanguageID();
                    spaName = "spa_ms_GetCardSummary";
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
        }
    }
    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        if (IsIntruderDetected) return;
        base.DoNeedExportConfig(sender, exportConfig);
        string strHeader = GetLocalResourceObject("CardSummaryModal_aspx_cs_CardSummary").ToString() + "\r\n" + ltrHeaderTop.Text + "\r\n" + Server.HtmlDecode(sender.GridHeader);
        string strFileName = GetLocalResourceObject("CardSummaryModal_aspx_cs_CardSummaryFileName").ToString();
        if (ViewState["Hierarchy"] != null)
        {
            strFileName += "_" + ViewState["Hierarchy"].ToString() + "_" + ViewState["ReportDate"].ToString();
        }

        string strCSVHeader = strHeader;
        strFileName = strFileName.Replace("\r\n", "");
        strFileName = strFileName.Replace("<strong>", "");
        strFileName = strFileName.Replace("</strong>", "");

        strCSVHeader = strCSVHeader.Replace("\r\n", Environment.NewLine);
        strCSVHeader = strCSVHeader.Replace("<strong>", "");
        strCSVHeader = strCSVHeader.Replace("</strong>", "");

        if (sender.ExportButtonType != UxExport.ExportType.Excel)
        {
            exportConfig.ReportHeader = strCSVHeader;
        }
        else exportConfig.ReportHeader = strHeader;
        exportConfig.FileName = GeneralFuncsLib.GetFileName(strFileName);
    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            if (sender == uxReportGrid && sender.Visible)
            {
                //handle for CardType
                string cardDesc = dataRow["CardDescription"].ToString();
                if (!String.IsNullOrEmpty(cardDesc))
                    dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(cardDesc);

            }
        }
    }

    private string GetHierarchyValue()
    {
        var sqStrHierarchyValue = this.SecureQueryString["HierarchyValue"];
        return string.IsNullOrEmpty(sqStrHierarchyValue) ? this.SavedReportFilterValue.Value : sqStrHierarchyValue;
    }
}
