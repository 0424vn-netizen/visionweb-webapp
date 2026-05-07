using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using Telerik.Web.UI;

[PagePermission("RskRETCB,MSRskRETCB")]
public partial class rm_MCF_RetCbDetail : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindRetrievalGrid,
        BindCBGrid
    }

    enum PostBackAction
    {
        DoSearching,
        BackToRetCb
    }

    #endregion

    #region Const

    const string MERCHANT_NUMBER = "MerchantNumber";
    const string MERCHANT_NAME = "MerchantName";
    const string BEGIN_DATE = "BeginDate";
    const string END_DATE = "EndDate";

    #endregion

    #region Properties

    string _MerchantNumber = string.Empty;
    private string MerchantNumber
    {
        get
        {
            if (SecureQueryString[MERCHANT_NUMBER] != null)
                _MerchantNumber = SecureQueryString[MERCHANT_NUMBER];

            return _MerchantNumber;
        }
    }

    DateTime _BeginDate = DateTime.Now;
    private DateTime BeginDate
    {
        get
        {
            if (SecureQueryString[BEGIN_DATE] != null)
                _BeginDate = DateTime.Parse(SecureQueryString[BEGIN_DATE]);
            return _BeginDate;
        }
    }

    DateTime _EndDate = DateTime.Now;
    private DateTime EndDate
    {
        get
        {
            if (SecureQueryString[END_DATE] != null)
                _EndDate = DateTime.Parse(SecureQueryString[END_DATE]);
            return _EndDate;
        }
    }

    DateTime _ReportDate = DateTime.Now;
    private DateTime ReportDate
    {
        get
        {
            return uxDateFilter.SelectedDate.Value;
        }
    }

    string _GridHeaderText = string.Empty;
    private string GridHeaderText
    {
        get
        {
            return MerchantNumber + ": " + GeneralFuncsLib.GetMerchantName(MerchantNumber) + " (" + uxDateFilter.SelectedDate.Value.ToGenericDateString() + ")";
        }
    }

    string _CardSearchIntruderQuery_Retrieval = string.Empty;
    private string CardSearchIntruderQuery_Retrieval
    {
        get
        {

            if (_CardSearchIntruderQuery_Retrieval == string.Empty)
            {
                _CardSearchIntruderQuery_Retrieval = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxRetrievalGrid.ID, new string[] { "CardNumber" });
            }
            return _CardSearchIntruderQuery_Retrieval;
        }
    }

    string _CardSearchIntruderQuery_Chargeback = string.Empty;
    private string CardSearchIntruderQuery_Chargeback
    {
        get
        {

            if (_CardSearchIntruderQuery_Chargeback == string.Empty)
            {
                _CardSearchIntruderQuery_Chargeback = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxCBGrid.ID, new string[] { "CardNumber" });
            }
            return _CardSearchIntruderQuery_Chargeback;
        }
    }

    private const string IMAGE = "<a class=\"atab\"href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModal('{0}','auto');\"><img src='../res/images/information.gif' border=\"0\"/></a>&nbsp; &nbsp;";
    #endregion

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxRetrievalGrid");
        this.GridIDs.Add("uxCBGrid");

        this.ExporterIDs.Add("uxExporterRetrieval");
        this.ExporterIDs.Add("uxExporterCB");

        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            if (MerchantNumber.Length == 0)
                Response.Redirect("rm_MCF_RetCb.aspx", true);
            if (BeginDate != DateTime.MinValue)
            {
                uxDateFilter.SelectedDate = BeginDate;
            }
            else
            {
                uxDateFilter.SelectedDate = DateTime.Now.Date;
            }
            uxDateFilter.MaxDate = DateTime.Now;
        }
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoSearching);
    }

    protected void uxButtonGoBack_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.BackToRetCb);
    }

    protected override void DoSwitchView()
    {
        if (CheckCSViewFullCard())
        {
            uxCBGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
            uxCBGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = false;
            uxRetrievalGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
            uxRetrievalGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = false;
        }
        else
        {
            uxCBGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
            uxCBGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = true;
            uxRetrievalGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
            uxRetrievalGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = true;
        }

        if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR").ToLower().Equals("true"))
        {
            uxCBGrid.Columns.FindByUniqueName("FirstChargebackRDRAmount").Visible = true;
            uxCBGrid.Columns.FindByUniqueName("PostChargebackRDRAmount").Visible = true;
        }
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (sender == uxRetrievalGrid && uxRetrievalGrid.Visible)
        {
            uxExporterRetrieval.GridTitle = GetLocalResourceObject("rm_RetCbDetail_aspx_cs_String1").ToString() + " - ";
            uxExporterRetrieval.GridSubTitle = VeraCodeSolution.ValidateResponseData(GridHeaderText);
            uxExporterRetrieval.GridHeader = uxExporterRetrieval.GridTitle + uxExporterRetrieval.GridSubTitle;
            OnDataBindControls(DataBindAction.BindRetrievalGrid, sender);
        }

        if (sender == uxCBGrid && uxCBGrid.Visible)
        {
            uxExporterCB.GridTitle = GetLocalResourceObject("rm_RetCbDetail_aspx_cs_String2").ToString() + " - ";
            uxExporterCB.GridSubTitle = VeraCodeSolution.ValidateResponseData(GridHeaderText);
            uxExporterCB.GridHeader = uxExporterCB.GridTitle + uxExporterCB.GridSubTitle;
            OnDataBindControls(DataBindAction.BindCBGrid, sender);
        }
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if ((sender == uxRetrievalGrid || sender == uxCBGrid) && sender.Visible)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;

                dataItem["ReasonCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ReasonDescription"].ToString());
                string queryString = BuildSecureQueryString("cn=" + dataRow["PartialAccountNumber"] + "&cnf=" + dataRow["AccountNumber"] + "&merch=" + MerchantNumber + "&isRisk=1");
                string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
                string urlCard = "<a href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";


                if (sender == uxCBGrid)
                {
                    if (GeneralFuncsLib.HasIPForFullCard(this))
                    {
                        dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(
                            String.Format(IMAGE, BuildUrlForFullCard("Chargebacks", dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialAccountNumber"].ToString() + "</a>")
                            );
                    }
                    else
                    {
                        dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["AccountNumber"].ToString() + "</a>");
                        dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialAccountNumber"].ToString() + "</a>");
                    }
                }
                else
                {
                    if (GeneralFuncsLib.HasIPForFullCard(this))
                    {
                        dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(
                            String.Format(IMAGE, BuildUrlForFullCard("Retrievals", dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialAccountNumber"].ToString() + "</a>")
                            );
                    }
                    else
                    {
                        dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["AccountNumber"].ToString() + "</a>");
                        dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialAccountNumber"].ToString() + "</a>");
                    }
                }
            }
        }
    }

    bool CheckCSViewFullCard()
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
        {
            return true;
        }
        return false;
    }

    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        uxRetrievalGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxRetrievalGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = true;

        uxCBGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxCBGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = true;
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = (((UxExport)sender).GridID == "uxRetrievalGrid") ? uxExporterRetrieval.GridHeader : uxExporterCB.GridHeader;
        exportConfig.FileName = (((UxExport)sender).GridID == "uxRetrievalGrid") ? GeneralFuncsLib.FormatFileName(uxExporterRetrieval.GridHeader) : GeneralFuncsLib.FormatFileName(uxExporterCB.GridHeader);
        if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR").ToLower().Equals("true"))
        {
            uxCBGrid.Columns.FindByUniqueName("FirstChargebackRDRAmount").Visible = true;
            uxCBGrid.Columns.FindByUniqueName("PostChargebackRDRAmount").Visible = true;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        DataTable dt = null;
        string spaName = "spa_RM_MCF_GetRetrievalsChargebacksDetails";
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserReportingParams(false);
        parames.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
        parames.Add(new FilterParameter("@DateFilterMode", 1, DbType.Int32));
        parames.Add(new FilterParameter("@BeginDate", uxDateFilter.SelectedDate.Value, DbType.DateTime));
        parames.Add(new FilterParameter("@EndDate", uxDateFilter.SelectedDate.Value, DbType.DateTime));
        parames.AddLanguageID();
        // Check permission before decrypt
        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
        {
            parames.AddDecryptDataParams("AccountNumber", _isExporting);
        }

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindRetrievalGrid:
                parames.Add(new FilterParameter("@Type", "Retrieval", DbType.AnsiString));
                ASGrid retrievalGrid = (ASGrid)sender;
                retrievalGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });
                break;
            case DataBindAction.BindCBGrid:
                parames.Add(new FilterParameter("@Type", "Chargeback", DbType.AnsiString));
                ASGrid cbGrid = (ASGrid)sender;
                cbGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });
                break;
        }
    }


    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoSearching:
                {
                    uxRetrievalGrid.Rebind();
                    uxCBGrid.Rebind();
                }
                break;
            case PostBackAction.BackToRetCb:
                {
                    Response.Redirect(RiskSessionManager.RiskReportReferrerInfo.Url, true);
                }
                break;
        }
    }
    private string BuildUrlForFullCard(string reportType, string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=" + reportType + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate);
        queryString = "../FullCC.aspx?" + queryString;
        return queryString;
    }
}
