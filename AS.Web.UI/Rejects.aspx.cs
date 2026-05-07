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
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using Telerik.Web.UI;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Controls.Exporter;

[PagePermission("RejectRpt,MSRejectRpt")]
public partial class Rejects : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindDrilldownGrid,
        BindReportGrid,
        BindHeaderText,
    }

    enum PostBackAction
    {
        BatchDetailClick
    }

    #endregion Enum

    #region Constants

    private string REPORT_HEADER = string.Empty;
    private const string SPA_GET_REJECT_DETAILS = "spa_GetRejectDetails";
    private const string BLANK = "&nbsp;";

    // Column Name of Grid
    private const string DRILLDOWN = "DrilldownColumn";
    private const string ENTITY_NAME = "EntityName";
    private const string PARTIAL_CARD_NUMBER = "PartialCardNumber";
    private const string ACCOUNT_NUMBER = "AccountNumber";
    private const string RECORD_ID = "RecordId";
    private const string ISSUE_BANK = "IssueBank";
    private const string REPORT_DATE = "ReportDate";
    private const string CARD_TYPE = "CardType";
    private const string CARD_TYPE_DES = "CardTypeDesc";
    private const string TRANS_CODE = "TransactionCode";
    private const string TRANS_CODE_DES = "TransCodeDescription";
    private const string AUTH_NUMBER = "AuthorizationNumber";

    #endregion Constants

    #region Fields

    private string _Target = "_parent";
    private string _batchDetailIntruderQuery = string.Empty;
    // flag to mark that we are selecting data to export
    private bool _isExporting = false;
    private string _authIntruderQuery = string.Empty;
    private int _curIdx = 0;

    #endregion Fields

    #region Properties

    private string GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter) + " " + GeneralFuncsLib.GetDateFilterText(ReportFilter);
        }
    }

    private string BatchDetailIntruderQuery
    {
        get
        {
            if (_batchDetailIntruderQuery.IsNullOrEmpty())
            {
                _batchDetailIntruderQuery =
                    GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                        uxReportGrid.ID,
                        new string[] { "BatchNumber", "TerminalNumber" });
            }
            return _batchDetailIntruderQuery;
        }
    }

    private string AuthIntruderQuery
    {
        get
        {
            if (_authIntruderQuery.IsNullOrEmpty())
            {
                _authIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    this.uxReportGrid.ID, new string[] { "AuthorizationNumber" });
            }
            return _authIntruderQuery;
        }
    }

    #endregion Properties

    #region Methods

    #region Protected Methods

    protected override void PageInitialize()
    {
        if (IsIntruderDetected)
            return;

        this.ExporterIDs.Add("uxExporterDrilldown");
        this.ExporterIDs.Add("uxExporterDrilldownBottom");
        this.ExporterIDs.Add("uxExporterBottom");

        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        REPORT_HEADER = GetLocalResourceObject("Rejects_aspx_cs_RejectedTransaction").ToString() + " ";
        IsBindDataOnLoad = true;
        if (SessionManager.ClientFrameInfo != string.Empty)
        {
            _Target = SessionManager.ClientFrameInfo;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        OnDataBindControls(DataBindAction.BindHeaderText);
        uxDrilldownGrid.Columns.FindByUniqueName(ENTITY_NAME).Visible = GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME();
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDrilldownGrid:
                {
                    FilterParameterCollection parames = new FilterParameterCollection();
                    parames.AddLoggedInUserReportingParams();
                    parames.AddHierarchyFilterParams((ReportPage)this);
                    parames.Add(new FilterParameter(
                        "@ReportType",
                        ReportType.REJECTS,
                        DbType.AnsiString));

                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(
                        WebServices.CsReportServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] {
                            WebSiteConstants.GET_REPORT_SPA_NAME,
                            ReportServices.ConvertToFilterParamWSArray(parames) });
                }
                break;
            case DataBindAction.BindReportGrid:
                {
                    FilterParameterCollection parames = new FilterParameterCollection();
                    parames.AddHierarchyFilterParams((ReportPage)this);
                    parames.AddLoggedInUserReportingParams();
                    parames.AddLoggedInUserPrimaryUserID();
                    if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)Page))
                    {
                        parames.AddDecryptDataParams("AccountNumber", _isExporting);
                    }
                    parames.AddLanguageID();
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(
                        WebServices.CsReportServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] {
                            SPA_GET_REJECT_DETAILS,
                            ReportServices.ConvertToFilterParamWSArray(parames) });
                }
                break;

            case DataBindAction.BindHeaderText:
                {
                    if (uxDrilldownGrid.Visible)
                    {
                        uxExporterDrilldown.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                        uxExporterDrilldown.GridSubTitle = VeraCodeSolution.ValidateResponseData(GridTitle);
                    }
                    else if (uxReportGrid.Visible)
                    {
                        uxExporter.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                        uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(GridTitle);
                    }
                }
                break;
        }
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindDrilldownGrid, sender);
        }
        if (sender == uxReportGrid && uxReportGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }
    }

    protected override void DoSwitchView()
    {
        if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode)
            && !string.IsNullOrEmpty(ReportFilter.CurrentValue.Value))
        {
            uxReportGrid.Visible = true;
            uxDrilldownGrid.Visible = false;
            if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
            {
                uxReportGrid.Columns.FindByUniqueName(ACCOUNT_NUMBER).Visible = true;
                uxReportGrid.Columns.FindByUniqueName(PARTIAL_CARD_NUMBER).Visible = false;
            }
            else
            {
                uxReportGrid.Columns.FindByUniqueName(ACCOUNT_NUMBER).Visible = false;
                uxReportGrid.Columns.FindByUniqueName(PARTIAL_CARD_NUMBER).Visible = true;
            }
        }
        else
        {
            string colName = VeraCodeSolution.DoVeraCode(
                GeneralFuncsLib.GetGridDrilldownColumnName(
                    ReportFilter.CurrentValue.HierarchyMode,
                    ReportFilter.CurrentValue.Value));
            uxDrilldownGrid.Columns.FindByUniqueName(DRILLDOWN).HeaderText = colName;
            uxDrilldownGrid.Columns.FindByUniqueName(DRILLDOWN).HeaderTooltip = colName;
            uxReportGrid.Visible = false;
            uxDrilldownGrid.Visible = true;
        }
    }

    protected override void DoItemDataBound(ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        GridDataItem dataItem = e.Item as GridDataItem;
        DataRowView dataRow = e.Item.DataItem as DataRowView;
        if (e.Item is GridDataItem)
        {
            if (sender == uxDrilldownGrid && sender.Visible)
            {
                //if (GeneralFuncsLib.IsMerchantModeToSetTooltip(ReportFilter)
                //    && dataRow.DataView.Table.Columns[ENTITY_NAME] != null)
                //{
                //    dataItem[DRILLDOWN].ToolTip
                //        = VeraCodeSolution.DoVeraCode(dataRow[ENTITY_NAME].ToString());
                //}

                if (!GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME())
                {
                    if (GeneralFuncsLib.IsMerchantModeToSetTooltip(ReportFilter) && dataRow.DataView.Table.Columns[ENTITY_NAME] != null)
                    {
                        dataItem[DRILLDOWN].ToolTip = VeraCodeSolution.DoVeraCode(dataRow[ENTITY_NAME].ToString());
                    }
                }
                dataItem[ENTITY_NAME].Text = VeraCodeSolution.DoVeraCode(dataRow[ENTITY_NAME].ToString());



            }
            else if (sender == uxReportGrid && uxReportGrid.Visible)
            {
                FormatItemOfDetailGrid(dataItem, dataRow);
            }
        }
    }

    protected void uxBatchNumberLink_Command(object sender, CommandEventArgs e)
    {
        OnPostBackActions(PostBackAction.BatchDetailClick, sender);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.BatchDetailClick:
                ShowBatchDetailModal((LinkButton)sender);
                break;
        }
    }

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        _isExporting = true;
        uxReportGrid.Columns.FindByUniqueName(PARTIAL_CARD_NUMBER).Visible = true;
        uxReportGrid.Columns.FindByUniqueName(ACCOUNT_NUMBER).Visible = false;

        if (uxDrilldownGrid.Visible)
        {
            uxDrilldownGrid.Columns.FindByUniqueName(ENTITY_NAME).Visible = true;
            if (uxDrilldownGrid.Columns.FindByUniqueName(DRILLDOWN).HeaderText == GetLocalResourceObject("Rejects_aspx_cs_MerchantNumber").ToString()
            || uxDrilldownGrid.Columns.FindByUniqueName(DRILLDOWN).HeaderText == GetLocalResourceObject("Rejects_aspx_cs_Merchant").ToString()
            || GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
            {
                uxDrilldownGrid.Columns.FindByUniqueName(ENTITY_NAME).HeaderText = GetLocalResourceObject("Rejects_aspx_cs_MerchantName").ToString();
            }
        }

        string fileName = ((UxExport)sender).GridTitle.ToString() + GridTitle;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(fileName);
        exportConfig.ReportHeader = fileName;
    }

    #endregion Protected Methods

    #region Private Methods

    private void ShowBatchDetailModal(LinkButton linkButton)
    {
        string[] param = linkButton.CommandArgument.ToString().Split(';');
        string activityText = string.Format(GetLocalResourceObject("Rejects_aspx_cs_BatchDetailBatch").ToString(), param[0], param[1]);
        GeneralFuncsLib.SaveUserActivity(this.ReportFilter.CurrentValue.Value, activityText);

        string queryString = "BatchDetailModal.aspx?"
            + this.BuildSecureQueryString(
                string.Format("MerchantNumber={0}&ReportDate={1}&BatchNumber={2}&TerminalNumber={3}&Type=batchhistory{4}",
                              this.ReportFilter.CurrentValue.Value,
                              param[1], param[0], param[2],
                              BatchDetailIntruderQuery));
        this.AjaxAddResponseScript(string.Format("ShowPopupModal('{0}');", queryString));
    }

    private void FormatItemOfDetailGrid(GridDataItem dataItem, DataRowView dataRow)
    {
        // Build Card# URL
        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
        {
            if (!dataItem[ACCOUNT_NUMBER].Text.Equals(BLANK))
            {
                dataItem[ACCOUNT_NUMBER].Text = VeraCodeSolution.DoVeraCode(
                    GeneralFuncsLib.BuildUrlForCardNumber(
                        (SecurePage)this.Page,
                        dataRow[PARTIAL_CARD_NUMBER],
                        dataRow[ACCOUNT_NUMBER],
                        ReportFilter.CurrentValue.Value,
                        dataRow[ACCOUNT_NUMBER].ToString()));
            }
        }
        else
        {
            if (!dataItem[PARTIAL_CARD_NUMBER].Text.Equals(BLANK))
            {
                if (GeneralFuncsLib.HasIPForFullCard(this))
                {
                    dataItem[PARTIAL_CARD_NUMBER].Text = VeraCodeSolution.DoVeraCode(
                        GeneralFuncsLib.BuildUrlForFullCard(
                            (SecurePage)this.Page,
                            ReportType.REJECTS,
                            dataRow[RECORD_ID].ToString(),
                            dataRow[ISSUE_BANK].ToString(),
                            dataRow[REPORT_DATE].ToString(),
                            dataRow[PARTIAL_CARD_NUMBER],
                            dataRow[ACCOUNT_NUMBER],
                            ReportFilter.CurrentValue.Value,
                            dataRow[PARTIAL_CARD_NUMBER].ToString(),
                            _curIdx + 1,
                            false));
                }
                else
                {
                    dataItem[PARTIAL_CARD_NUMBER].Text = VeraCodeSolution.DoVeraCode(
                        GeneralFuncsLib.BuildUrlForCardNumber(
                            (SecurePage)this.Page,
                            dataRow[PARTIAL_CARD_NUMBER],
                            dataRow[ACCOUNT_NUMBER],
                            ReportFilter.CurrentValue.Value,
                            dataRow[PARTIAL_CARD_NUMBER].ToString()));
                }
            }
        }

        // Build Auth # Hyperlink
        dataItem[AUTH_NUMBER].Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.BuildAuthUrlPopupModalChild(
            (SecurePage)Page, dataRow[AUTH_NUMBER], ReportFilter.CurrentValue.Value,
            _curIdx + 1, AuthIntruderQuery, dataRow[AUTH_NUMBER].ToString(), false)
            );

        // Set tooltip for Card Type
        if (!dataItem[CARD_TYPE].Text.IsNullOrEmpty())
        {
            dataItem[CARD_TYPE].ToolTip = VeraCodeSolution.DoVeraCode(
                GeneralFuncsLib.NvlString(dataRow[CARD_TYPE_DES].ToString()));
        }

        // Set tooltip for Trans Code
        if (!dataItem[TRANS_CODE].Text.IsNullOrEmpty())
        {
            dataItem[TRANS_CODE].ToolTip = VeraCodeSolution.DoVeraCode(
                GeneralFuncsLib.NvlString(dataRow[TRANS_CODE_DES].ToString()));
        }
    }

    #endregion Private Methods

    #endregion Methods
}
