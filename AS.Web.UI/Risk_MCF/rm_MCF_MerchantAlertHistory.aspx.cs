using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Core.Common.Utilities;
using AS.Web.Business;
using AS.Web.UI.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

[PagePermission("RskMerchantAlertDetails,MSRskMerchantAlertDetails")]
public partial class rm_MCF_MerchantAlertHistory : NonReportPage
{
    #region CONSTANTS
    private const string LIMIT_EXTRACT_MERCHANT_ALERT_HISTORY = "LimitExtractMerchanAlertHistory";
    private const string NUMBER_EXTRACT_MERCHANT_REPORT = "NumberExtractMerchantReport";
    #endregion

    #region Enums

    enum DataBindAction
    {
        BindReportGrid,
    }

    enum PostBackAction
    {
        DoFilterAction,
    }

    enum ReportMode
    {
        Group,
        Item
    }
    #endregion

    #region Fields

    private string _MerchantNumber;
    private string _merchantProfileIntruderQuery = string.Empty;

    #endregion Fields

    #region --- Private Methods ---
    #region Properties
    private MerchantAlertReportEntities _MerchantAlertReportEntities;
    private FilterParameterCollection parameters;
    private FilterParameterCollection paramOuts;

    private int _NumberExtractMerchantReport
    {
        get
        {
            if (ViewState[NUMBER_EXTRACT_MERCHANT_REPORT] != null)
                return (int)ViewState[NUMBER_EXTRACT_MERCHANT_REPORT];
            else
                return 0;
        }
        set { ViewState[NUMBER_EXTRACT_MERCHANT_REPORT] = value; }
    }
    public int LimitExtractMerchantReport
    {
        get
        {
            int result = 0;
            if (GeneralFuncsLib.GetDataOfExtendedSetting(LIMIT_EXTRACT_MERCHANT_ALERT_HISTORY) != null)
            {
                Int32.TryParse(GeneralFuncsLib.GetDataOfExtendedSetting(LIMIT_EXTRACT_MERCHANT_ALERT_HISTORY), out result);
            }
            return result;
        }
    }

    public bool IsSearch = false;
    #endregion Properties

    #endregion --- Private Methods ----

    #region --- Event Handles ----
    protected void Page_Load()
    {
        if (!IsPostBack)
        {
            lblSubTitle.Visible = false;
            litGridTitle.Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("uxPageTitleResource.ReportTitle").ToString());
        }
    }


    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoFilterAction:
                IsSearch = true;
                if (_NumberExtractMerchantReport >= LimitExtractMerchantReport)
                {
                    string msg = string.Format(Resources.MessageManager.LimitFileByUser, LimitExtractMerchantReport);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "showMessage", string.Format("alert('{0}')", msg), true);
                    uxReportGrid.Rebind();
                    return;
                }
                parameters = new FilterParameterCollection();
                paramOuts = new FilterParameterCollection();
                List<FilterItem> filterList = new List<FilterItem>();
                BuildDataXML(filterList);
                parameters.AddLoggedInUserReportingParams(true);
                parameters.Add(new FilterParameter("@FilterContent", General.SerializeObjectToXML<List<FilterItem>>(filterList, true), DbType.Xml));
                parameters.Add(new FilterParameter("@LogID ", 0, DbType.Int32));
                parameters.Add(new FilterParameter("@IsCheckData", false, DbType.Boolean, true));
                WebServices.RiskServices.ExecuteNonQueryCommand(_MerchantAlertReportEntities.SPAName, parameters, out paramOuts);
                Boolean result = false;
                Boolean.TryParse(paramOuts[0].ParameterValue.ToString(), out result);
                if (result)
                {
                    parameters = new FilterParameterCollection();
                    paramOuts = new FilterParameterCollection();
                    parameters.AddLoggedInUserReportingParams(true);
                    filterList = new List<FilterItem>();
                    BuildDataXML(filterList);
                    parameters.AddLoggedInUserReportingParams(true);
                    parameters.Add(new FilterParameter("@FilterContent", General.SerializeObjectToXML<List<FilterItem>>(filterList, true), DbType.Xml));
                    parameters.Add(new FilterParameter("@SPAName", _MerchantAlertReportEntities.SPAName, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@CategoryCode", _MerchantAlertReportEntities.CategoryCode, DbType.Int32));
                    parameters.Add(new FilterParameter("@SubCategoryCode", _MerchantAlertReportEntities.SubCategoryCode, DbType.Int32));
                    WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_InsertExportExtractMerchant", parameters, out paramOuts);
                    uxReportGrid.Rebind();
                }
                else
                {
                    string msg = Resources.MessageManager.NoResultsFound;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "showMessage", string.Format("alert('{0}')", msg), true);
                }
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams();
                parameters.Add(new FilterParameter("@IsAll", true, DbType.Boolean));
                parameters.Add(new FilterParameter("@PageType", "MerchantAlertWorkedPage", DbType.String));
                parameters.AddLanguageID();
                DataTable dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetExportExtractMerchantLog", parameters);
                _NumberExtractMerchantReport = dt.Rows.Count;
                uxReportGrid.DataSource = dt;
                break;
            default:
                break;
        }
    }

    protected void OnSearchEvent(object sender, MerchantAlertReportEntities merchantAlertReportEntities)
    {
        _MerchantAlertReportEntities = merchantAlertReportEntities;
        OnPostBackActions(PostBackAction.DoFilterAction);
        lblSubTitle.Visible = true;
    }
    #endregion --- Event Handles ----

    protected void uxReportGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    private void BuildDataXML(List<FilterItem> filterList)
    {
        FilterItem filterItem = new FilterItem();

        HierarchyFilterValue reportFilter = SessionManager.CurrentAlertHistoryReportFilter;

        int DateFilterMode = (int)reportFilter.DateOption;
        DateTime dateFrom = reportFilter.DateOptionValue.From;
        DateTime endDate = reportFilter.DateOption == DateOptionMode.DateRange ? reportFilter.DateOptionValue.To : reportFilter.DateOptionValue.From;

        filterItem.Key = "DateFilterMode";
        filterItem.Value = DateFilterMode.ToString();
        filterItem.Operator = OperatorEnums.Equal;
        filterList.Add(filterItem);

        filterItem = new FilterItem();
        filterItem.Key = "DateTime";
        filterItem.Value = dateFrom.ToDateTimeString();
        filterItem.Value2 = endDate.ToDateTimeString();
        filterItem.Operator = OperatorEnums.Between;
        filterList.Add(filterItem);

        filterItem = new FilterItem();
        filterItem.Key = "SiteID";
        filterItem.Value = SessionManager.CurrentUser.SiteID.ToString();
        filterItem.Operator = OperatorEnums.In;
        filterList.Add(filterItem);

        if (!string.IsNullOrEmpty(uxReportFilter.MerchantName))
        {
            filterItem = new FilterItem();
            filterItem.Key = "MerchantName";
            filterItem.Value = uxReportFilter.MerchantName;
            filterItem.Operator = OperatorEnums.Equal;
            filterList.Add(filterItem);
        }

        if (!string.IsNullOrEmpty(uxReportFilter.MerchantNumber))
        {
            filterItem = new FilterItem();
            filterItem.Key = "MerchantNumberList";
            filterItem.Value = uxReportFilter.MerchantNumber;
            filterItem.Operator = OperatorEnums.In;
            filterList.Add(filterItem);
        }

        if (!string.IsNullOrEmpty(uxReportFilter.AssignmentFilterValue))
        {
            filterItem = new FilterItem();
            filterItem.Key = "AssignmentList";
            filterItem.Value = uxReportFilter.AssignmentFilterValue;
            filterItem.Operator = OperatorEnums.In;
            filterList.Add(filterItem);
        }
        if (!string.IsNullOrEmpty(uxReportFilter.ParameterFilterValue))
        {
            filterItem = new FilterItem();
            filterItem.Key = "ParameterKeyList";
            filterItem.Value = uxReportFilter.ParameterFilterValue;
            filterItem.Operator = OperatorEnums.In;
            filterList.Add(filterItem);
        }
        if (!string.IsNullOrEmpty(uxReportFilter.UserNameFilterValue))
        {
            filterItem = new FilterItem();
            filterItem.Key = "UserList";
            filterItem.Value = uxReportFilter.UserNameFilterValue;
            filterItem.Operator = OperatorEnums.In;
            filterList.Add(filterItem);
        }

        if (!string.IsNullOrEmpty(uxReportFilter.DispositionFilterValue))
        {
            filterItem = new FilterItem();
            filterItem.Key = "DispositionList";
            filterItem.Value = uxReportFilter.DispositionFilterValue;
            filterItem.Operator = OperatorEnums.In;
            filterList.Add(filterItem);
        }
    }

    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            Literal uxLitReportName = (Literal)dataItem["Download"].FindControl("uxLitReportName");
            LinkButton uxDownloadReport = (LinkButton)dataItem["Download"].FindControl("uxDownloadReport");
            uxLitReportName.Text = uxLitReportName.Text.Remove(uxLitReportName.Text.LastIndexOf('.'), uxLitReportName.Text.Split('.').LastOrDefault().Length + 1);
            uxDownloadReport.Text = uxDownloadReport.Text.Remove(uxDownloadReport.Text.LastIndexOf('.'), uxDownloadReport.Text.Split('.').LastOrDefault().Length + 1);

            var retryButton = ((LinkButton)dataItem["Delete"].FindControl("uxRetry"));
            retryButton.Visible = false;
            switch ((int)dataRow["Status"])
            {
                case 0: // Export In Processing
                case 1:
                    uxDownloadReport.Visible = false;
                    uxLitReportName.Visible = true;
                    ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = false;
                    break;
                case 2: // Exported Completely
                    uxDownloadReport.Visible = true;
                    uxLitReportName.Visible = false;
                    ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = true;
                    break;
                case 3: // Failed in exporting
                    uxDownloadReport.Visible = false;
                    uxLitReportName.Visible = true;
                    ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = true;
                    retryButton.Visible = true;
                    dataItem["StatusDesc"].ForeColor = System.Drawing.Color.Red;
                    break;
            }

        }
    }

    protected void DeleteFileExport(object sender, CommandEventArgs e)
    {
        string[] parts = e.CommandArgument.ToString().Split('-');
        int logId = Int32.Parse(parts[0]);
        List<int> docIds = new List<int>();
        if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
        {
            string[] items = parts[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                int id;
                if (int.TryParse(item.Trim(), out id))
                {
                    docIds.Add(id);
                }
            }
        }

        parameters = new FilterParameterCollection();
        paramOuts = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        parameters.Add(new FilterParameter("@LogId", logId, DbType.Int32));
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_DeleteExportExtractMerchant", parameters, out paramOuts);
        if (docIds.Count > 0)
        {
            foreach (var docId in docIds)
            {
                WebServices.DocServices.DeleteFileOnDocServer(SessionManager.CurrentUser.ASClient.ToString(), SessionManager.CurrentUser.UserID, docId.ToString());
            }
        }
        uxReportGrid.Rebind();
    }

    protected void RetryReport(object sender, CommandEventArgs e)
    {
        int logId = Int32.Parse(e.CommandArgument.ToString());

        FilterParameterCollection paramIn = new FilterParameterCollection();
        paramIn.AddLoggedInUserReportingParams(true);
        paramIn.Add(new FilterParameter("@LogID", logId, DbType.Int32));
        paramIn.Add(new FilterParameter("@Status", 0, DbType.Int32));
        paramIn.Add(new FilterParameter("@Mode", 3, DbType.Int32));
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_UpdateStatusExportExtractMerchant", paramIn, out paramIn);

        uxReportGrid.Rebind();
    }

    protected void btnRefreshGrid_Click(object sender, EventArgs e)
    {
        uxReportGrid.Rebind();
    }

    protected void uxbtnDownload_Click(object sender, EventArgs e)
    {
        string listDocId = uxDocId.Value;
        string fileName = uxFileName.Value;

        DownloadFile(listDocId, fileName);
    }

    #region HELPER METHODS
    private void DownloadFile(string listDocID, string fileName)
    {
        List<int> docIds = listDocID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => int.Parse(id.Trim())).ToList();

        if (docIds.Count == 1)
        {
            byte[] buffer = WebServices.DocServices.DownloadDoc(docIds[0]);
            this.TransferFileToClient(buffer, GeneralFuncsLib.FormatFileName(fileName));
            return;
        }

        using (MemoryStream mergedStream = new MemoryStream())
        {
            foreach (int docId in docIds)
            {
                byte[] chunkData = WebServices.DocServices.DownloadDoc(docId);
                mergedStream.Write(chunkData, 0, chunkData.Length);
            }

            mergedStream.Position = 0;
            this.TransferFileToClient(mergedStream, GeneralFuncsLib.FormatFileName(fileName));
        }
    }
    #endregion
}
