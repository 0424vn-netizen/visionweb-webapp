using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

[PagePermission("RskExtracts,MSRskExtracts")]
public partial class rm_MCF_MgmtReport_Extracts : NonReportPage
{
    #region CONSTANTS
    private const string LIMIT_EXTRACT_MERCHANT_REPORT = "LimitExtractMerchantReport";
    private const string NUMBER_EXTRACT_MERCHANT_REPORT = "NumberExtractMerchantReport";
    #endregion

    #region FIELDS
    private FilterParameterCollection parameters;
    private FilterParameterCollection paramOuts;
    private MgmtReportEntities _MgmtReportEntities;
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
            if (GeneralFuncsLib.GetDataOfExtendedSetting(LIMIT_EXTRACT_MERCHANT_REPORT) != null)
            {
                Int32.TryParse(GeneralFuncsLib.GetDataOfExtendedSetting(LIMIT_EXTRACT_MERCHANT_REPORT), out result);
            }
            return result;
        }
    }

    public bool IsSearch = false;

    protected enum DataBindAction
    {
        BindGrid,
        Search
    }
    #endregion

    #region EVENTS

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        base.OnDataBindControls(type, sender);
        switch ((DataBindAction)type)
        {
            case DataBindAction.Search:
                IsSearch = true;
                if (_NumberExtractMerchantReport >= LimitExtractMerchantReport)
                {
                    string msg = string.Format(Resources.MessageManager.LimitFileByUser, LimitExtractMerchantReport);
                    //AjaxAddResponseScript(string.Format("ShowMessage('{0}');", msg));
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "showMessage", string.Format("alert('{0}')", msg), true);
                    uxReportGrid.Rebind();
                    return;
                }
                HierarchyFilterValue reportFilter = SessionManager.CurrentReportFilter;
                parameters = new FilterParameterCollection();
                paramOuts = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams(true);
                if (_MgmtReportEntities.HasDateRangeFilter)
                {
                    parameters.Add(new FilterParameter("@DateFilterMode", (int)reportFilter.DateOption, DbType.Int32));
                    parameters.Add(new FilterParameter("@DateFrom", reportFilter.DateOptionValue.From, DbType.Date));
                    DateTime endDate = reportFilter.DateOption == DateOptionMode.DateRange ? reportFilter.DateOptionValue.To : reportFilter.DateOptionValue.From;
                    parameters.Add(new FilterParameter("@DateTo", endDate, DbType.Date));
                }

                if (_MgmtReportEntities.HasMerchantFilter)
                {
                    parameters.Add(new FilterParameter("@MerchantType", _MgmtReportEntities.MerchantType, DbType.Int32));
                    if (_MgmtReportEntities.MerchantType == 2)
                        parameters.Add(new FilterParameter("@MerchantNumber", _MgmtReportEntities.SearchValue, DbType.AnsiString));
                }

                parameters.Add(new FilterParameter("@IsCheckData", false, DbType.Boolean, true));
                WebServices.RiskServices.ExecuteNonQueryCommand(_MgmtReportEntities.SPAName, parameters, out paramOuts);
                Boolean result = false;
                Boolean.TryParse(paramOuts[0].ParameterValue.ToString(), out result);
                if (result)
                {

                    parameters = new FilterParameterCollection();
                    paramOuts = new FilterParameterCollection();
                    parameters.AddLoggedInUserReportingParams(true);
                    if (_MgmtReportEntities.HasDateRangeFilter)
                    {
                        parameters.Add(new FilterParameter("@DateFilterMode", (int)reportFilter.DateOption, DbType.Int32));
                        parameters.Add(new FilterParameter("@DateFrom", reportFilter.DateOptionValue.From, DbType.Date));
                        DateTime toDate = reportFilter.DateOption == DateOptionMode.DateRange ? reportFilter.DateOptionValue.To : reportFilter.DateOptionValue.From;
                        parameters.Add(new FilterParameter("@DateTo", toDate, DbType.Date));
                    }

                    if (_MgmtReportEntities.HasMerchantFilter)
                    {
                        parameters.Add(new FilterParameter("@MerchantType", _MgmtReportEntities.MerchantType, DbType.Int32));
                        if (_MgmtReportEntities.MerchantType == 2)
                            parameters.Add(new FilterParameter("@MerchantNumber", _MgmtReportEntities.SearchValue, DbType.AnsiString));
                    }

                    parameters.Add(new FilterParameter("@SPAName", _MgmtReportEntities.SPAName, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@CategoryCode", _MgmtReportEntities.CategoryCode, DbType.Int32));
                    parameters.Add(new FilterParameter("@SubCategoryCode", _MgmtReportEntities.SubCategoryCode, DbType.Int32));
                    WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_InsertExportExtractMerchant", parameters, out paramOuts);

                    uxReportGrid.Rebind();
                }
                else
                {
                    string msg = Resources.MessageManager.NoResultsFound;
                    //AjaxAddResponseScript(string.Format("ShowMessage('{0}');", msg));
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "showMessage", string.Format("alert('{0}')", msg), true);
                }
                break;
            case DataBindAction.BindGrid:
                parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams();
                parameters.Add(new FilterParameter("@IsAll", true, DbType.Boolean));
                parameters.AddLanguageID();
                DataTable dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetExportExtractMerchantLog", parameters);
                _NumberExtractMerchantReport = dt.Rows.Count;
                uxReportGrid.DataSource = dt;
                if (dt.HasData())
                    RegisterScriptRefresh(dt);
                break;
        }

    }

    protected void uxReportFilter_Search(object sender, MgmtReportEntities mgmtReportEntites)
    {
        _MgmtReportEntities = mgmtReportEntites;
        OnDataBindControls(DataBindAction.Search);
    }

    protected void uxReportGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindGrid);
    }

    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            switch ((int)dataRow["Status"])
            {
                case 0: // Export In Processing
                case 1:
                    ((LinkButton)dataItem["Download"].FindControl("uxDownloadReport")).Visible = false;
                    ((Literal)dataItem["Download"].FindControl("uxLitReportDate")).Visible = true;
                    ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = false;
                    break;
                case 2: // Exported Completely
                    ((LinkButton)dataItem["Download"].FindControl("uxDownloadReport")).Visible = true;
                    ((Literal)dataItem["Download"].FindControl("uxLitReportDate")).Visible = false;
                    ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = true;
                    break;
                case 3: // Failed in exporting
                    ((LinkButton)dataItem["Download"].FindControl("uxDownloadReport")).Visible = false;
                    ((Literal)dataItem["Download"].FindControl("uxLitReportDate")).Visible = true;
                    ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = true;
                    break;
            }

        }
    }

    protected void DeleteFileExport(object sender, CommandEventArgs e)
    {
        int logId = Int32.Parse(e.CommandArgument.ToString().Split(',')[0]);
        int docId = 0;
        Int32.TryParse(e.CommandArgument.ToString().Split(',')[1], out docId);

        parameters = new FilterParameterCollection();
        paramOuts = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        parameters.Add(new FilterParameter("@LogId", logId, DbType.Int32));
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_DeleteExportExtractMerchant", parameters, out paramOuts);
        if (docId != 0)
        {
            WebServices.DocServices.DeleteFileOnDocServer(SessionManager.CurrentUser.ASClient.ToString(), SessionManager.CurrentUser.UserID, docId.ToString());
        }
        uxReportGrid.Rebind();
    }

    //protected void DownloadFileExport(object sender, CommandEventArgs e)
    //{
    //    int docId = Int32.Parse(e.CommandArgument.ToString().Split(',')[0]);
    //    string fileName = (e.CommandArgument.ToString().Split(',')[1]).ToString();
    //    DownloadFile(docId, fileName);
    //}

    protected void btnRefreshGrid_Click(object sender, EventArgs e)
    {
        uxReportGrid.Rebind();
    }

    protected void uxbtnDownload_Click(object sender, EventArgs e)
    {
        int docId = Int32.Parse(uxDocId.Value);
        string fileName = uxFileName.Value;
        DownloadFile(docId, fileName);
    }
    #endregion

    #region HELPER METHODS
    private void DownloadFile(int docID, string fileName)
    {
        byte[] buffer = WebServices.DocServices.DownloadDoc(docID);
        this.TransferFileToClient(buffer, GeneralFuncsLib.FormatFileName(fileName));
    }

    /// <summary>
    /// This function will check on the list of statement, If it contains any item has status is not "Completed"
    /// It will register a javascript function, this function will refresh Statement grid every 1 minute until 
    /// all items have status is "Completed"
    /// </summary>
    /// <param name="dt">Datatable contains all statement</param>
    private void RegisterScriptRefresh(DataTable dt)
    {
        DataRow[] itemNotComplete = dt.Select("Status <> 2");
        if (itemNotComplete.Length > 0 && itemNotComplete.IsNotNullData())
        {
            if (IsPostBack && !IsSearch)
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "refreshGrid", "onRefreshStatementGrid()", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "refreshGrid", "onRefreshStatementGrid()", true);
            IsSearch = false;
        }
    }

    //42924 – VW - Filter Collapse Issue
    [WebMethod(EnableSession = true)]
    public static string[] GetCriteria(string reportType)
    {
        UserControls_rm_MCF_MgmtReporting us = new UserControls_rm_MCF_MgmtReporting();
        return us.GetCriteria(reportType);
    }

    #endregion
}