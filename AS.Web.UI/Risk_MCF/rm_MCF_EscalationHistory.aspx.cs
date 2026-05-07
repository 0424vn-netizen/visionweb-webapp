using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using AS.WS.Entities;

[PagePermission("RskEscQueue,RskAdhoc,MSRskEscQueue,MSRskAdhoc")]
public partial class rm_MCF_EscalationHistory : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindGrid,
        BindInfo
    }

    #endregion Enums

    #region Fields

    private const string TICKET_NUMBER_KEY_CODE = "TNO";
    private const string ESCALATION_ID = "EscalationID";
    private const string MERCHANT_NUMBER = "MerchantNumber";

    private string m_currentSortExpr = string.Empty;
    private string m_currentSortOrder = string.Empty;
    private string _riskReportIntruderQuery = string.Empty;

    #endregion Fields

    #region Properties

    private string RiskReportIntruderQuery
    {
        get
        {
            if (_riskReportIntruderQuery.IsNullOrEmpty())
            {
                _riskReportIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    this.ID, new string[] { MERCHANT_NUMBER });
            }
            return _riskReportIntruderQuery;
        }
    }

    #endregion Properties

    #region Methods

    #region Protected Methods

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExport1");
        base.PageInitialize();
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGrid:
                if (IsIntruderDetected)
                    return;
                GetData();
                break;
            case DataBindAction.BindInfo:
                int escalationID = 0;
                DataTable escalations = null;
                bool isPopup = false;
                Boolean.TryParse(base.SecureQueryString["IsPopup"], out isPopup);
                if (isPopup)
                    ((MasterPageNormal)Page.Master).HideHeaderMenu = true;

                if (!Page.IsPostBack)
                {
                    if (base.SecureQueryString == null
                        || !Int32.TryParse(base.SecureQueryString[ESCALATION_ID], out escalationID))
                    {
                        Response.Redirect("rm_MCF_EscalationQueue.aspx", true);
                    }

                    ViewState[ESCALATION_ID] = escalationID.ToString();
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    var getEscalationParamsModel = new GetEscalationParamsModel()
                    {
                        AssignedToList = string.Empty,
                        ResolutionList = string.Empty,
                        StatusList = string.Empty,
                        OpenClosedCode = OpenClosedCode.ALL,
                        OpenClosedFromDate = DateTime.Now,
                        OpenClosedToDate = DateTime.Now,
                        KeyType = TICKET_NUMBER_KEY_CODE,
                        KeyValue = escalationID.ToString(),
                        FollowUpCode = null,
                        FollowUpFromDate = null,
                        FollowUpToDate = null,
                        Order = string.Empty
                    };
                    escalations = WebServices.RiskServices.GetEscalation(parameters, getEscalationParamsModel, true);

                    if (escalations.Rows.Count > 0)
                    {
                        DataRow row = escalations.Rows[0];
                        string url = BuildMerchantHyperlinkInRisk((SecurePage)Page,
                            row[MERCHANT_NUMBER], true, RiskReportIntruderQuery,
                            GeneralFuncsLib.NvlString(row[MERCHANT_NUMBER]), true);

                        uxEscalationID.Text = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.NvlString(row[ESCALATION_ID]));
                        uxStatus.Text = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.NvlString(row["Status"]));
                        uxMerchantNumber.Text = VeraCodeSolution.DoVeraCode(url);
                        uxClosed.Text = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.NvlString(row["Closed"]));
                        uxMerchantName.Text = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.NvlString(row["MerchantName"]));
                        uxClosedDate.Text = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.NvlString(row["ClosedDate"]));
                        uxEscalationDate.Text = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.NvlString(row["EscalationDate"]));
                        uxClosedBy.Text = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.NvlString(row["ClosedBy"]));
                        uxAssignedTo.Text = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.NvlString(row["AssignedTo"]));
                        uxFollowupDate.Text = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.NvlString(row["FollowupDate"] == DBNull.Value
                            ? string.Empty : DateTime.Parse(row["FollowupDate"].ToSafeString()).ToString("MM/dd/yyyy")));
                        uxReason.Text = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.NvlString(row["Reason"]));
                    }

                    //has referrer
                    if (RiskSessionManager.EscalationQueueReferrer.Length > 0 && RiskSessionManager.EscalationQueueReferrerInfo.Url.Length > 0)
                    {
                        uxGoBack.NavigateUrl = RiskSessionManager.EscalationQueueReferrerInfo.Url;
                        uxGoBack.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("rm_EscalationHistory_aspx_cs_Backto").ToString() + " " + RiskSessionManager.EscalationQueueReferrerInfo.Title);
                        uxGoBack.Visible = true;
                    }
                    else
                    {
                        uxGoBack.Visible = false;
                        RiskSessionManager.EscalationQueueReferrer = null;
                        RiskSessionManager.EscalationQueueReferrerInfo = null;
                    }
                }
                break;
        }
    }

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = uxExporter.GridHeader;
        exportConfig.FileName = "EscalationHistory";
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid)
            OnDataBindControls(DataBindAction.BindGrid);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.IsBindDataOnLoad = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (GeneralFuncsLib.GetDataOfExtendedSetting("GainLossInvestigationAmount").ToLower().Equals("true"))
        {
            GridColumn colSavingLoss = uxReportGrid.MasterTableView.GetColumnSafe("SavingLoss");
            if (colSavingLoss != null)
            {
                colSavingLoss.Visible = true;
            }
        }
        OnDataBindControls(DataBindAction.BindInfo);
    }
    
    private void GetData()
    {
        int escalationID = 0;

        if (ViewState[ESCALATION_ID] != null)
        {
            Int32.TryParse(ViewState[ESCALATION_ID].ToString(), out escalationID);
        }
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddLanguageID();
        uxReportGrid.DataSource = WebServices.RiskServices.GetEscalationHistory(
            parameters, string.Empty, escalationID, true);
    }

    //SP 224 - Open Risk Report many times with the same window name cause issue "Page Unresponse"
    public string BuildMerchantHyperlinkInRisk(SecurePage page, object merchantNumberCell,
        bool isPopup, string rskRptIntruderQuery, string text, bool isMCFRisk)
    {
        Guid guid = Guid.NewGuid();
        string pageUrl = isMCFRisk ? "rm_MCF_RiskReport.aspx?" : "rm_RiskReport.aspx?";
        string url = pageUrl + page.BuildSecureQueryString(
              string.Format("merchantnumber={0}&IsPopup={1}{2}",
                            GeneralFuncsLib.NvlString(merchantNumberCell),
                            isPopup,
                            rskRptIntruderQuery));
        return string.Format(
            "<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"openPopupWindow('{0}','{2}'); return false;\">{1}</a>",
            url,
            text, guid.ToString());
    }
    #endregion Protected Methods

    #endregion Methods
}
