using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using AS.WS.Entities;

[PagePermission("RskEsca,MSRskEsca")]
public partial class rm_MCF_Escalation_ActiveModal : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindGrid
    }

    #endregion Enums

    #region Const

    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string MERCHANT_NAME = "MerchantName";

    #endregion Const

    #region Fields

    private string _statusID = string.Empty;
    private string _merchantIntruderQuery = string.Empty;

    #endregion Fields

    #region Properties

    private string MerchantIntruderQuery
    {
        get
        {

            if (_merchantIntruderQuery.IsNullOrEmpty())
            {
                _merchantIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    uxReportGrid.ID, new string[] { "MerchantNumber" });
            }
            return _merchantIntruderQuery;
        }
    }

    #endregion Properties

    #region Methods

    #region Protected Methods

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterBottom");
        base.PageInitialize();
        IsBindDataOnLoad = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        PageType = SecurePageType.Modal;
        _statusID = SecureQueryString["StatusID"];
        uxReportGrid.IsIntruder = true;
        uxReportGrid.IntruderSourceName = GeneralFuncsLib.GetRequestFileName() + uxReportGrid.ID;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindGrid, sender);
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.GetLegalFileName(GetLocalResourceObject("rm_Escalation_ActiveModal_aspx_cs_Text1").ToString());
        exportConfig.ReportHeader = GetLocalResourceObject("rm_Escalation_ActiveModal_aspx_cs_Text2").ToString();
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGrid:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    var getEscalationParamsModel = new GetEscalationParamsModel()
                    {
                        AssignedToList = string.Empty,
                        ResolutionList = string.Empty,
                        StatusList = _statusID,
                        OpenClosedCode = OpenClosedCode.OPEN,
                        OpenClosedFromDate = new DateTime(1900, 1, 1),
                        OpenClosedToDate = new DateTime(3000, 1, 1),
                        KeyType = string.Empty,
                        KeyValue = string.Empty,
                        FollowUpCode = "ALL",
                        FollowUpFromDate = null,
                        FollowUpToDate = null,
                        Order = "EscalationDate"
                    };
                    uxReportGrid.DataSource = WebServices.RiskServices.GetEscalation(parameters, getEscalationParamsModel, true);
                }
                break;
        }
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            string url = RiskGeneral.BuildMerchantHyperlinkInRisk((SecurePage)Page,
                dataRow[MERCHANT_NUMBER], true, MerchantIntruderQuery,
                GeneralFuncsLib.NvlString(dataRow[MERCHANT_NAME]), true);

            dataItem[MERCHANT_NAME].Text = VeraCodeSolution.GetOutputHtmlString(url);
            dataItem[MERCHANT_NAME].ToolTip = VeraCodeSolution.DoVeraCode(dataRow[MERCHANT_NUMBER].ToString());
        }
    }

    #endregion Protected Methods

    #endregion Methods
}
