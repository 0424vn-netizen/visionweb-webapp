using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using AS.Common.WSE;
using System.Web.Services;
using System.Collections.Generic;
using System.Linq;

[PagePermission("MerchProfile,MSMerchProfile")]
public partial class gen_MerchantProfile : ReportPage
{
    #region Enum
    enum DataBindAction
    {
        BindGrid,
        BindMemoGrid,
        BindCommentGrid,

    }

    enum PostBackAction
    {
        AddComment,
        ResetPage,
    }

    enum MerchantDetailType
    {
        FDR,
        TSYS,
        PLANET,
        MPS,
        ORI,
        SNET,
        FIS,
        FISMPS,
        EMS,
        Fulton,
        PPI,
        TNBCI,
        North,
        NTS,
        WRFC,
        FDR_NORTH,
        OMAHA,
        CAYAN,
        //42782 – VW – CAYAN - Implement New TSYS Processing Platform - add
        CAYAN_TSYS,
        SOFTEK,
        TNBCI_TSYS,
        MONETARY,
        CLEARENT,
        GREENBOX,
        // 44005
        SignaPay,
        ALLIEDWALLET,
        FultonDemo,
        SPHERE,
        RS2,
        FIPS,
        Paya,
        MLS,
        PCS
    }

    #endregion Enum

    #region Constants

    private const string DRILL_DOWN_COL_NAME = "DrilldownColumn";
    private const string MER_NUMBER_COL_NAME = "MerchantNumber";
    private const string STATUS_COL_NAME = "Status";
    private const string DBANAME_COL_NAME = "DBAName";
    private const string ISO_COL_NAME = "ISO";
    private const string SALE_OFFICE_COL_NAME = "SALESOFFICE";
    private const string BANK_NR_COL_NAME = "BANKNUMBER";
    private const string MCCSIC_COL_NAME = "MCCSIC";
    private const string BANK_COL_NAME = "Bank";
    private const string ASSO_COL_NAME = "Association";
    private const string AGENT_COL_NAME = "Agent";
    private const string MICChain_COL_NAME = "MICChain";
    private const string CHAIN_COL_NAME = "Chain";
    private const string MER_STATUS_COL_NAME = "MerchantStatus";
    private const string LAST_BATCH_ACTIVITY_COL_NAME = "LastBatchActivity";
    private const string ADDR_COL_NAME = "Address";
    private const string PHONE_COL_NAME = "Phone";
    private const string EMAIL_COL_NAME = "Email";
    private const string OPEN_DATE_COL_NAME = "OpenDate";
    private const string OPT_IN_COL_NAME = "OptIn";
    private const string SYS_PRIN_AGENT_COL_NAME = "SysPrinAgent";
    private const string SALES_AGENT_COL_NAME = "SalesAgent";
    private const string HEAD_QUARTER_COL_NAME = "Headquarter";
    private const string CHN_COL_NAME = "CHN";
    private const string PARTNER_ID_COL_NAME = "PartnerID";
    private const string SALES_UNIT_COL_NAME = "SalesUnit";
    private const string PLANET_AGENT_COL_NAME = "PlanetAgent";
    private const string LAST4_TAXID_COL_NAME = "Last4TaxID";
    private const string GRID_TITLE_CSR_COMMENT = "MerchantProfile_aspx_cs_CSRComments";
    private const string GRID_TITLE_MERCHANT_LIST = "MerchantProfile_aspx_cs_MerchantList";
    private const string GRID_TITLE_MERCHANT_MEMMO = "MerchantProfile_aspx_cs_MerchantMemos";
    private const string IPMT_BANK_COL_NAME = "NBANK";
    private const string IPMT_AGENT_COL_NAME = "NAGENT";
    private const string IPMT_CORP_COL_NAME = "NCORP";
    private const string IPMT_CHAIN_COL_NAME = "NCHAIN";
    private const string IPMT_SYSPRINAGENT_COL_NAME = "IPMTSysPrinAgent";
    private const string CAYAN_SALESAGENT_COL_NAME = "CayanSalesAgent";
    private const string CAYAN_SUB_SALESAGENT_COL_NAME = "CayanSubSalesAgent";
    private const string CAYAN_CHAIN_COL_NAME = "CayanChain";
    private const string CAYAN_CNAME_COL_NAME = "CorporateName";
    //42782 – VW – CAYAN - Implement New TSYS Processing Platform - add
    private const string CAYAN_CBANK_COL_NAME = "EntityNumber5";
    private const string CAYAN_CASSO_COL_NAME = "EntityNumber6";

    private const string USER_ID_COL_NAME = "UserID";
    //eVANCE
    private const string eVANCE_SPON_BANK_COL_NAME = "eVANCESponBank";
    private const string eVANCE_ASSOCIATION_COL_NAME = "eVANCEAssociation";
    private const string eVANCE_CHAIN_COL_NAME = "eVANCEChain";
    private const string eVANCE_OPENDATE_COL_NAME = "OpenDate";
    private const string eVANCE_EMAIL_COL_NAME = "eVANCEEmail";
    private const string eVANCE_STATUS_COL_NAME = "eVANCEStatus";
    private const string eVANCE_CS_TAXID_COL_NAME = "CSTaxID";
    private const string eVANCE_MS_TAXID_COL_NAME = "MSTaxID";
    private const string eVANCE_Sales_COL_NAME = "eVANCESales";

    private const string MONETARY_BANK_COL_NAME = "MONETARYBank";
    private const string MONETARY_ASS_COL_NAME = "MONETARYAssociation";

    // Clearent
    private const string CLEARENT_SPON_BANK_COL_NAME = "ClearentSponBank";
    private const string CLEARENT_SPON_BANK_BIN_COL_NAME = "ClearentSponBankBIN";
    private const string CLEARENT_PROCESS_PLATFORM_COL_NAME = "ClearentProcessPlatform";
    private const string CLEARENT_CHAIN_COL_NAME = "ClearentChain";
    private const string CLEARENT_MCCSIC_COL_NAME = "MCCSIC_Clearent";
    private const string CLEARENT_RESELLER_NAME = "ResellerName";
    private const string CLEARENT_PARTNER_NAME = "PartnerName";
    // SPR 
    private const string SPR_PROCESS_PLATFORM = "SPRProcessPlatform";
    private const string SPR_PROCESS_REFERRALPARTNER = "ReferralPartner";
    private const string SPR_ADDR_COL_NAME = "SPRAddress";
    private const string SPRCSTaxID_COL_NAME = "SPRCSTaxID";
    private const string SPRMSTaxID_COL_NAME = "SPRMSTaxID";
    private const string SPRMCCSIC_COL_NAME = "SPRMCCSIC";
    private const string SPREmail_COL_NAME = "SPREmail";
    private const string SPRFUNDINGMETHOD_COL_NAME = "SPRFundingMethod";
    private const string SPRASSOCIATION_COL_NAME = "SPRAssociation";
    private const string SPRBACKENDPROCESSOR_COL_NAME = "SPRBackEndProcessor";
    private const string SPRGROUP_COL_NAME = "SPRGroup";
    private const string SPRREFERRALSOURCE_COL_NAME = "SPRReferralSource";
    // SPHEREP 
    private const string SPHERE_PROCESS_PLATFORM = "SPHEREProcessPlatform";
    private const string SPHERE_PROCESS_REFERRALPARTNER = "ReferralPartner";
    private const string SPHERE_ADDR_COL_NAME = "SPHEREAddress";
    private const string SPHERECSTaxID_COL_NAME = "SPHERECSTaxID";
    private const string SPHEREMSTaxID_COL_NAME = "SPHEREMSTaxID";
    private const string SPHEREMCCSIC_COL_NAME = "SPHEREMCCSIC";
    private const string SPHEREEmail_COL_NAME = "SPHEREEmail";
    private const string SPHEREFUNDINGMETHOD_COL_NAME = "SPHEREFundingMethod";
    private const string SPHEREASSOCIATION_COL_NAME = "SPHEREAssociation";
    private const string SPHEREBACKENDPROCESSOR_COL_NAME = "SPHEREBackEndProcessor";
    private const string SPHEREGROUP_COL_NAME = "SPHEREGroup";
    private const string SPHEREREFERRALSOURCE_COL_NAME = "SPHEREReferralSource";

    private const string RS2BANK_COL_NAME = "RS2Bank";
    private const string RS2BANKNAME_COL_NAME = "RS2BankName";
    private const string RS2ACQUIRER_COL_NAME = "Acquirer";
    private const string RS2ACQUIRERNAME_COL_NAME = "AcquirerName";
    private const string RS2SUBACQUIRER_COL_NAME = "SubAcquirer";
    private const string RS2SUBACQUIRERNAME_COL_NAME = "SubAcquirerName";

    //paya
    private const string PAYA_CS_TAXID_COL_NAME = "PAYACSTaxID";
    private const string PAYA_MS_TAXID_COL_NAME = "PAYAMSTaxID";
    private const string PAYA_MCCSIC_COL_NAME = "PAYAMCCSIC";
    private const string PAYA_CHAIN_COL_NAME = "PayaChain";
    private const string PAYA_PHONE_COL_NAME = "PAYAPhone";
    private const string PAYA_MAIL_COL_NAME = "PAYAEmail";
    private const string PAYA_OPENDATE_COL_NAME = "PAYAOpenDate";
    private const string PAYA_STATUS_COL_NAME = "PAYAStatus";
    // MLS
    private const string Group_COL_NAME = "Group";
    private const string SECONDARYACCESSCHAIN_COL_NAME = "SecondaryAccessChain";
    // SIGNAPAY
    private const string SIGNAPAY_MCCSIC_COL_NAME = "SIGNAPAY_MCCSIC";
    //Allied Wallet
    private const string ALLIEDWALLET_COM_NAME_COL_NAME = "CompanyName";

    //FIPS
    private const string FIPS_TENANT_COL_NAME = "TenantName";
    private const string FIPS_SUBMERCHANT_COL_NAME = "SubMerchantName";

    // SPAs
    private const string SPA_GET_BE_PROCESSOR = "spa_cs_GetBEProcessor";
    private const string SPA_CHECK_MERCHANT_BELONG_TO_USER = "spa_SEC_CheckMerchantBelongtoUser";

    private const int GREENBOX_CLIENT = 165;

    #endregion Constants

    #region Properties

    private string MerchantNumber
    {
        get
        {
            if (ViewState["MerchantNumber"] != null)
                return ViewState["MerchantNumber"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["MerchantNumber"] = value;
        }
    }

    private string MerchantName
    {
        get
        {
            if (ViewState["MerchantName"] != null)
                return ViewState["MerchantName"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["MerchantName"] = value;
        }
    }

    private string BEProcessor
    {
        get
        {
            if (ViewState["BEProcessor"] != null)
                return ViewState["BEProcessor"].ToString();
            else
                return string.Empty;
        }
        set
        {
            ViewState["BEProcessor"] = value;
        }
    }

    private string GetMerchantNumberName()
    {
        if (!string.IsNullOrEmpty(MerchantNumber))
            return " - " + MerchantNumber + ": " + MerchantName;
        else

            return string.Empty;
    }

    private string _GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter);
        }
    }

    private bool HasMSProductEnvironment()
    {
        return GeneralFuncsLib.HasMSProductEnvironment();
    }

    public string scrollingValue
    {
        get
        {
            if (Page.Request.UserAgent.IndexOf("MSIE 7.0") > 0)
            {
                return "auto";
            }
            else
            {
                return "no";
            }
        }
    }

    private bool IsForceInvisibleMemo;

    public bool IsHideHeaderMenuAndLeftNav
    {
        get
        {
            if (IsSecureQueryString)
                return SecureQueryString["IsHideMenu"].ToBoolean();
            return false;
        }
    }

    private int _HierarchySelected
    {
        get
        {
            if (IsSecureQueryString && SecureQueryString["FilterMode"] != null)
            {
                return SecureQueryString["FilterMode"].ToInt();
            }
            return -1;
        }
    }

    private string _FilterValue
    {
        get
        {
            if (IsSecureQueryString && SecureQueryString["FilterValue"] != null)
            {
                return SecureQueryString["FilterValue"].ToString();
            }
            return string.Empty;
        }
    }
    #endregion Properties

    #region Methods

    #region Protected Methods

    protected override void PageInitialize()
    {
        //check has config for new client
        var clientConfig = MerchantProfileHelper.GetClientConfig();
        if (clientConfig != null)
        {
            var merchantInfoUrl = "MerchantInformation.aspx";
            string paramUrl = Request.QueryString.Count == 0 ? string.Empty : Request.QueryString.ToString();

            if (!string.IsNullOrEmpty(paramUrl))
                merchantInfoUrl += "?" + paramUrl;

            Response.Redirect(merchantInfoUrl, true);
        }

        //TK39919 - Remove
        this.GridIDs.Add("uxMemoGrid");
        //TK39919 - Add
        this.GridIDs.Add("uxMerchantNoteGrid");
        this.ExporterIDs.Add("uxExporterCommentTop");
        this.ExporterIDs.Add("uxExporterCommentBottom");
        this.ExporterIDs.Add("uxExportTop");
        this.ExporterIDs.Add("uxExportBottom");
        this.ExporterIDs.Add("uxExportMemolistTop");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        IsBindDataOnLoad = true;
        CheckMerchantFromQueryString();//Check merchant exists in query string
        MaintainScrollPositionOnPostBack = true;
        uxMIF_MerchantDetails_FDR.HierachyMode = ReportFilter.CurrentValue.HierarchyMode;
        if (!IsPostBack)
        {
            GoBackSetting();
        }
        //for SNET Readonly
        //TK39919 - Remove

        ((MasterPageNormal)Page.Master).HideHeaderMenu = true;

    }


    protected override void DoSwitchView()
    {
        if (!string.IsNullOrEmpty(MerchantNumber) && !IsPostBack)
        {
            HierarchyDetail merchant = GeneralFuncsLib.GetMerchantHierarchyInfo();
            ReportFilter.CurrentValue.ID = merchant.HierarchyID;
            ReportFilter.CurrentValue.Value = MerchantNumber;
            ReportFilter.CurrentValue.HierarchyMode = merchant.HierarchyMode;
        }
        else if (_HierarchySelected != -1 && !_FilterValue.IsNullOrEmpty() && !IsPostBack)
        {
            ReportFilter.CurrentValue.HierarchyMode = GeneralFuncsLib.GetHierarchyInfo(_HierarchySelected).HierarchyMode.ToString();
            ReportFilter.CurrentValue.ID = GeneralFuncsLib.GetHierarchyInfo(_HierarchySelected).HierarchyID;
            ReportFilter.CurrentValue.Value = _FilterValue;

        }

        this.EnableMerchantGridColumns();

        string entityIDDisable = "," + GeneralFuncsLib.GetInvisibleHyperlinkByEntityTypeIDs() + ",";
        string entityTypeCurrent = "," + SessionManager.CurrentUser.EntityType.ToString() + ",";
        if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode)
           && !string.IsNullOrEmpty(ReportFilter.CurrentValue.Value)
            && !string.IsNullOrEmpty(GeneralFuncsLib.GetMerchantName(ReportFilter.CurrentValue.Value))
            && IsMerchantBelongToUser()
            && !entityIDDisable.Contains(entityTypeCurrent))
        {
            this.uxPanelMerchantList.Visible = false;
            this.uxPanelDetail.Visible = true;
            this.LoadUserControls(ReportFilter.CurrentValue.Value);
            uxPanelCaseHistory.Visible = ((ReportPage)Page).IsUserWithPermission("CMOpenCase")
                || ((ReportPage)Page).IsUserWithPermission("CMSearchCase");
            //10/4/2018 | 45128 Merchant Notes Issue - Prod
            //TK39919 - Add

            ucMerchantNote.MerchantNumber = ReportFilter.CurrentValue.Value;
            ucCaseHistory.MerchantNumber = ReportFilter.CurrentValue.Value;
            if (SessionManager.CurrentMerchantNumber.IsNullOrEmpty()
                || SessionManager.CurrentMerchantNumber != ReportFilter.CurrentValue.Value)
            {
                SessionManager.CurrentMerchantNumber = ReportFilter.CurrentValue.Value;
                ucMerchantNote.BindDataMultiSelector();
                ucCaseHistory.BindDataMultiSelector();
                ucCaseHistory.BindGridCaseHistory();
            }
            else
            {
                // OP #7060
                // With secondary user in the first time add note at ms site, 
                // back to merchant profile and click search again
                // Must rebind role list to update the role of this user
                this.AjaxAddResponseScript("rebindRole()");
            }
        }
        else
        {
            this.uxPanelMerchantList.Visible = true;
            this.uxPanelDetail.Visible = false;
            uxPanelCaseHistory.Visible = false;
        }

        ((MasterPageNormal)Page.Master).HideHeaderMenu = IsHideHeaderMenuAndLeftNav;
        ((MasterPageNormal)Page.Master).ShowLeftNaviControl = !IsHideHeaderMenuAndLeftNav;

    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindGrid, sender);
        }
        //TK39919 - Remove
        if (uxMemoGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindMemoGrid, sender);
            //TK39919 - Remove
        }
    }

    public string ButtonExcelMerchantMemos_Click()
    {
        OnDataBindControls(DataBindAction.BindMemoGrid, uxExportMemolistTop.Grid);
        return uxExportMemolistTop.ExportExcelManual();
    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView rowView = e.Item.DataItem as DataRowView;
            if (sender == uxDrilldownGrid && sender.Visible)
            {
                if (rowView["LastBatchActivity"] != null && !string.IsNullOrEmpty(rowView["LastBatchActivity"].ToString()))
                {
                    if (((ReportPage)Page).IsUserWithPermission("BatchRpt") || ((ReportPage)Page).IsUserWithPermission("MSBatchRpt"))
                    {
                        DateTime tempDate;
                        DateTime.TryParse(rowView["LastBatchActivity"].ToString(), out tempDate);
                        dataItem["LastBatchActivity"].Text = VeraCodeSolution.DoVeraCode(
                            GeneralFuncsLib.BuildLastBatchHistoryLink(
                            (SecurePage)Page, tempDate.Ticks.ToString(), rowView["Entity"].ToString(), dataItem["LastBatchActivity"].Text));
                    }
                }
                else
                {
                    dataItem["LastBatchActivity"].Text = string.Empty;
                }
            }
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGrid:
                ASGrid grid = (ASGrid)sender;
                string spaName = "spa_GetMerchantList";
                FilterParameterCollection paramss = new FilterParameterCollection();
                paramss.AddLoggedInUserReportingParams();
                paramss.AddLanguageID();
                paramss.AddHierarchyFilterParamsWithoutDate(this);
                paramss.AddDecryptDataParams("EncryptedTaxID");
                grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(paramss) });

                uxExportTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject(GRID_TITLE_MERCHANT_LIST) + " - ");
                uxExportTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                uxExportTop.GridHeader = uxExportTop.GridTitle + uxExportTop.GridSubTitle;
                break;
            case DataBindAction.BindCommentGrid:
                {
                    FilterParameterCollection _Parameters = new FilterParameterCollection();
                    _Parameters.AddLoggedInUserReportingParams();
                    MerchantNumber = ReportFilter.CurrentValue.Value;
                    MerchantName = GeneralFuncsLib.GetMerchantName(MerchantNumber);
                    _Parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
                }
                break;
            case DataBindAction.BindMemoGrid:
                {
                    if (GeneralFuncsLib.GetDataOfExtendedSetting("InvisibleMemosGrid") != "true" && !IsForceInvisibleMemo)
                    {
                        uxMemoSection.Visible = true;
                        FilterParameterCollection _Parameters = new FilterParameterCollection();
                        _Parameters.AddLoggedInUserReportingParams();
                        MerchantNumber = ReportFilter.CurrentValue.Value;
                        MerchantName = GeneralFuncsLib.GetMerchantName(MerchantNumber);
                        _Parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
                        this.uxMemoGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_ms_GetMerchantMemos", ReportServices.ConvertToFilterParamWSArray(_Parameters) });

                        uxExportMemolistTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject(GRID_TITLE_MERCHANT_MEMMO) + " - ");
                        uxExportMemolistTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                        uxExportMemolistTop.GridHeader = uxExportMemolistTop.GridTitle + uxExportMemolistTop.GridSubTitle;
                    }
                    else
                    {
                        uxMemoSection.Visible = false;
                    }
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.AddComment:
                AjaxAddResponseScript("ClearMessage();");
                break;
        }
    }

    protected void uxAddComment_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.AddComment);
    }

    protected void uxReset_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ResetPage);
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        VisibleColumnForExport(sender, exportConfig);
        base.DoNeedExportConfig(sender, exportConfig);

        // If filter by taxid => just get partial Taxid
        switch (ReportFilter.CurrentValue.HierarchyMode)
        {
            case HierarchyMode.TAXID:
                string filename = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject(GRID_TITLE_MERCHANT_LIST) + " - ");
                exportConfig.FileName = GeneralFuncsLib.FormatFileName(filename + GeneralFuncsLib.GetPartialTaxId(ReportFilter.CurrentValue.Value));
                exportConfig.ReportHeader = Server.HtmlDecode(sender.GridTitle);
                break;
            default:
                exportConfig.FileName = GeneralFuncsLib.FormatFileName(Server.HtmlDecode(sender.GridHeader));
                exportConfig.ReportHeader = Server.HtmlDecode(exportConfig.ReportHeader);
                break;
        }
    }

    protected override void DoReportFilterAction(AS.Web.UI.Controls.ReportFilterEventArgs e)
    {
        this.MerchantNumber = ReportFilter.CurrentValue.Value;
        if (e.ActionType == AS.Web.UI.Controls.ReportFilterEventType.Submit)
        {
            string merchantName = GeneralFuncsLib.GetMerchantName(e.HierachyValue.Value);
            string activity = GetActivity(!string.IsNullOrEmpty(merchantName));
            string merchantNumber = string.IsNullOrEmpty(merchantName) ? "" : e.HierachyValue.Value;
            GeneralFuncsLib.SaveUserActivity(merchantNumber, activity);
        }
        base.DoReportFilterAction(e);

    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        ((MasterPageNormal)this.Master).ShowLeftNaviControl = uxPanelDetail.Visible && !IsHideHeaderMenuAndLeftNav;
    }
    protected void uxReloadHierachy_Click(object sender, EventArgs e)
    {
        LoadUserControls(ReportFilter.CurrentValue.Value);
        this.AjaxAddResponseScript("bindSourceAndRole()");
    }

    #endregion Protected Methods

    #region Private Methods

    private void VisibleColumnForExport(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        if (ShowPartialTaxIdExport())
        {
            uxDrilldownGrid.Columns.FindByUniqueName("CSTaxID").Visible = false;
            uxDrilldownGrid.Columns.FindByUniqueName("MSTaxID").Visible = true;

            switch (SessionManager.CurrentClient)
            {
                case WebSiteConstants.SPHERE_CLIENT:
                case WebSiteConstants.MLS_CLIENT:
                    {
                        uxDrilldownGrid.Columns.FindByUniqueName("CSTaxID").Visible = false;
                        uxDrilldownGrid.Columns.FindByUniqueName("MSTaxID").Visible = false;
                        uxDrilldownGrid.Columns.FindByUniqueName("SPHERECSTaxID").Visible = false;
                        uxDrilldownGrid.Columns.FindByUniqueName("SPHEREMSTaxID").Visible = true;
                        break;
                    }
            }


        }
        switch (SessionManager.CurrentClient)
        {
            case WebSiteConstants.FULTON_CLIENT:
                {
                    if (sender.ExportButtonType.ToString().ToLower() == ExportFileType.EXCEL.ToString().ToLower()
                        && WebSiteSettings.WebSiteType.ToUpper() == WebSiteConstants.WEBSITE_TYPE_CS)
                    {
                        for (int i = 0; i < uxDrilldownGrid.Columns.Count; i++)
                        {
                            if (uxDrilldownGrid.Columns[i].UniqueName.Contains("_Fulton"))
                            {
                                uxDrilldownGrid.Columns[i].Visible = true;
                            }
                        }
                    }
                    break;
                }

            case WebSiteConstants.CLEARENT_CLIENT:
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("CSTaxID").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("MSTaxID").Visible = true;
                    break;
                }

            case GREENBOX_CLIENT:
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("CSTaxID").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("MSTaxID").Visible = false;

                    uxDrilldownGrid.Columns.FindByUniqueName("PAYACSTaxID").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("PAYAMSTaxID").Visible = true;
                    break;
                }

            case WebSiteConstants.SPHERE_CLIENT:
            case WebSiteConstants.MLS_CLIENT:
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("CSTaxID").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("MSTaxID").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("SPHERECSTaxID").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("SPHEREMSTaxID").Visible = true;

                    break;

                }

            case WebSiteConstants.SPR_CLIENT:
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("CSTaxID").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("MSTaxID").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("SPRCSTaxID").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("SPRMSTaxID").Visible = true;

                    break;

                }
            case WebSiteConstants.PAYA_CLIENT:
            case WebSiteConstants.FIPS_CLIENT:
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("PAYACSTaxID").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("PAYAMSTaxID").Visible = true;
                    break;
                }
        }
    }

    private bool ShowPartialTaxIdExport()
    {
        if (GeneralFuncsLib.GetDataOfExtendedSetting("Show_Partial_TaxID_Export").Equals("true"))
        {
            return true;
        }
        return false;
    }
    private void GridNotShow_All()
    {
        for (int i = 0; i < uxDrilldownGrid.Columns.Count; i++)
        {
            uxDrilldownGrid.Columns[i].Visible = false;
        }
    }

    private void GridShow_TSYS()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, ISO_COL_NAME, SALE_OFFICE_COL_NAME,
            BANK_NR_COL_NAME, MCCSIC_COL_NAME, MER_STATUS_COL_NAME,
            LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME, PHONE_COL_NAME,
            EMAIL_COL_NAME, GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : string.Empty);
    }

    private void GridShow_FDR()
    {
        SetVisibleForDrilldownGridColumns(
           DBANAME_COL_NAME, SYS_PRIN_AGENT_COL_NAME,
           SALES_AGENT_COL_NAME, HEAD_QUARTER_COL_NAME, MCCSIC_COL_NAME,
           MER_STATUS_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME,
           ADDR_COL_NAME, PHONE_COL_NAME, EMAIL_COL_NAME,
           GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : string.Empty);
    }

    private void GridShow_MPS()
    {
        SetVisibleForDrilldownGridColumns(
           DBANAME_COL_NAME, CHN_COL_NAME, PHONE_COL_NAME,
           LAST4_TAXID_COL_NAME, MER_STATUS_COL_NAME,
           LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME,
           GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : string.Empty);
    }

    private void GridShow_PLANET()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, SALES_UNIT_COL_NAME, PLANET_AGENT_COL_NAME,
            MCCSIC_COL_NAME, MER_STATUS_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME,
            ADDR_COL_NAME, PHONE_COL_NAME, EMAIL_COL_NAME,
            GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : string.Empty);
    }

    private void GridShow_ORI()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, CHN_COL_NAME, PHONE_COL_NAME,
            LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME,
            GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : MER_STATUS_COL_NAME);
    }

    private void GridShow_SNET()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, PARTNER_ID_COL_NAME, PHONE_COL_NAME,
            LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME,
            GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : MER_STATUS_COL_NAME);
    }

    private void GridShow_FIS()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, MICChain_COL_NAME, CHAIN_COL_NAME, PHONE_COL_NAME, BANK_COL_NAME,
            ASSO_COL_NAME, AGENT_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME,
            OPEN_DATE_COL_NAME, MER_STATUS_COL_NAME, GeneralFuncsLib.HasMSProductEnvironment() ? OPT_IN_COL_NAME : MER_STATUS_COL_NAME);

        uxDrilldownGrid.Columns.FindByUniqueName(LAST_BATCH_ACTIVITY_COL_NAME).HeaderText = GetLocalResourceObject("MerchantProfile_aspx_cs_Lastbatch").ToString();
        uxDrilldownGrid.Columns.FindByUniqueName(MER_STATUS_COL_NAME).HeaderText = GetLocalResourceObject("MerchantProfile_aspx_cs_Status").ToString();
        uxDrilldownGrid.Columns.FindByUniqueName(MER_STATUS_COL_NAME).HeaderTooltip = GetLocalResourceObject("MerchantProfile_aspx_cs_Status").ToString();
    }

    private void GridShow_WRFC()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, PHONE_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME,
            ADDR_COL_NAME, CHN_COL_NAME,
            GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : MER_STATUS_COL_NAME);
    }

    private void GridShow_TNBCI()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, PHONE_COL_NAME, MER_STATUS_COL_NAME,
            LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME, CHN_COL_NAME);

        uxDrilldownGrid.Columns.FindByUniqueName(MER_STATUS_COL_NAME).HeaderText = GetLocalResourceObject("MerchantProfile_aspx_cs_AccountStatus").ToString();
        uxDrilldownGrid.Columns.FindByUniqueName(MER_STATUS_COL_NAME).HeaderTooltip = GetLocalResourceObject("MerchantProfile_aspx_cs_AccountStatus").ToString();
    }

    private void GridShow_NTS()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, PHONE_COL_NAME, MER_STATUS_COL_NAME,
            LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME);

        uxDrilldownGrid.Columns.FindByUniqueName(MER_STATUS_COL_NAME).HeaderText = GetLocalResourceObject("MerchantProfile_aspx_cs_AccountStatus").ToString();
        uxDrilldownGrid.Columns.FindByUniqueName(MER_STATUS_COL_NAME).HeaderTooltip = GetLocalResourceObject("MerchantProfile_aspx_cs_AccountStatus").ToString();
    }

    private void GridShow_IPMT()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, HEAD_QUARTER_COL_NAME, MCCSIC_COL_NAME, IPMT_BANK_COL_NAME, IPMT_AGENT_COL_NAME, IPMT_CORP_COL_NAME,
            IPMT_CHAIN_COL_NAME, IPMT_SYSPRINAGENT_COL_NAME, MER_STATUS_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME,
            ADDR_COL_NAME, PHONE_COL_NAME, EMAIL_COL_NAME,
            GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : string.Empty);
    }

    private void GridShow_FULTON()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, CHN_COL_NAME, PHONE_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME,
            GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : MER_STATUS_COL_NAME);
    }

    private void GridShow_FULTONDemo()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, CHN_COL_NAME, PHONE_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME,
            GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : MER_STATUS_COL_NAME);
    }


    //42782 – VW – CAYAN - Implement New TSYS Processing Platform - modify
    private void GridShow_CAYAN()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, MCCSIC_COL_NAME, CAYAN_SALESAGENT_COL_NAME, CAYAN_CHAIN_COL_NAME,
            CAYAN_CNAME_COL_NAME, CAYAN_CBANK_COL_NAME, CAYAN_CASSO_COL_NAME, IPMT_SYSPRINAGENT_COL_NAME, MER_STATUS_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME,
            ADDR_COL_NAME, PHONE_COL_NAME, EMAIL_COL_NAME, CAYAN_SUB_SALESAGENT_COL_NAME,
            GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : string.Empty);

    }

    private void GridShow_SOFTEK()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, CHN_COL_NAME, PHONE_COL_NAME,
            LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME,
            GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : MER_STATUS_COL_NAME);
    }

    private void GridShow_MONETARY()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, MONETARY_BANK_COL_NAME, MONETARY_ASS_COL_NAME, eVANCE_CHAIN_COL_NAME, eVANCE_STATUS_COL_NAME,
            PHONE_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME, eVANCE_OPENDATE_COL_NAME, eVANCE_EMAIL_COL_NAME, eVANCE_STATUS_COL_NAME,
            GeneralFuncsLib.HasMSProductEnvironment() ? STATUS_COL_NAME : MER_STATUS_COL_NAME,
            CheckCSViewFullCard() ? eVANCE_CS_TAXID_COL_NAME : eVANCE_MS_TAXID_COL_NAME);
    }

    private void GridShow_SIGNAPAY()
    {
        SetVisibleForDrilldownGridColumns(
             DBANAME_COL_NAME, CHN_COL_NAME, PHONE_COL_NAME, MONETARY_BANK_COL_NAME, MONETARY_ASS_COL_NAME,
            LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME, SIGNAPAY_MCCSIC_COL_NAME, eVANCE_EMAIL_COL_NAME, eVANCE_STATUS_COL_NAME, eVANCE_OPENDATE_COL_NAME,
              CheckCSViewFullCard() ? eVANCE_CS_TAXID_COL_NAME : eVANCE_MS_TAXID_COL_NAME
             );
    }

    private void GridShow_CLEARENT()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, CLEARENT_SPON_BANK_COL_NAME, CLEARENT_SPON_BANK_BIN_COL_NAME, CLEARENT_PROCESS_PLATFORM_COL_NAME,
            CLEARENT_RESELLER_NAME, CLEARENT_PARTNER_NAME, eVANCE_CHAIN_COL_NAME,
            PHONE_COL_NAME, ADDR_COL_NAME, eVANCE_OPENDATE_COL_NAME, eVANCE_EMAIL_COL_NAME
            , eVANCE_STATUS_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME,
            CheckCSViewFullCard() ? eVANCE_CS_TAXID_COL_NAME : eVANCE_MS_TAXID_COL_NAME, CLEARENT_MCCSIC_COL_NAME);
    }
    private void GridShow_GREENBOX()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, "GreenboxBusinessChain", "GreenboxBankChain", "GreenboxCorporateChain", "GreenboxAgentChain", SPHERE_ADDR_COL_NAME, eVANCE_CHAIN_COL_NAME,
             CheckCSViewFullTextView() ? PAYA_CS_TAXID_COL_NAME : PAYA_MS_TAXID_COL_NAME, PAYA_MCCSIC_COL_NAME, PHONE_COL_NAME, SPHEREEmail_COL_NAME, eVANCE_OPENDATE_COL_NAME, eVANCE_STATUS_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME);
    }


    private void GridShow_ALLIEDWALLET()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, ADDR_COL_NAME, eVANCE_OPENDATE_COL_NAME, eVANCE_STATUS_COL_NAME, ALLIEDWALLET_COM_NAME_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME);
    }

    private void GridShow_SPHERE()
    {
        SetVisibleForDrilldownGridColumns(
              DBANAME_COL_NAME, CHN_COL_NAME, SPHEREBACKENDPROCESSOR_COL_NAME, SPHEREGROUP_COL_NAME, SPHEREFUNDINGMETHOD_COL_NAME, SPHEREASSOCIATION_COL_NAME, SPHERE_PROCESS_REFERRALPARTNER, SPHEREREFERRALSOURCE_COL_NAME, PHONE_COL_NAME, MONETARY_BANK_COL_NAME,
             LAST_BATCH_ACTIVITY_COL_NAME, SPHERE_ADDR_COL_NAME, SPHEREMCCSIC_COL_NAME, SPHEREEmail_COL_NAME, eVANCE_STATUS_COL_NAME, eVANCE_OPENDATE_COL_NAME,
               CheckCSViewFullCard() ? SPHERECSTaxID_COL_NAME : SPHEREMSTaxID_COL_NAME
              );
    }


    private void GridShow_RS2()
    {
        SetVisibleForDrilldownGridColumns(
             DBANAME_COL_NAME, PHONE_COL_NAME, RS2BANK_COL_NAME, RS2BANKNAME_COL_NAME, RS2ACQUIRERNAME_COL_NAME, RS2SUBACQUIRERNAME_COL_NAME, RS2ACQUIRER_COL_NAME, RS2SUBACQUIRER_COL_NAME, CHN_COL_NAME,
            LAST_BATCH_ACTIVITY_COL_NAME, ADDR_COL_NAME, SIGNAPAY_MCCSIC_COL_NAME, eVANCE_EMAIL_COL_NAME, eVANCE_STATUS_COL_NAME, eVANCE_OPENDATE_COL_NAME,
            CheckCSViewFullCard() ? eVANCE_CS_TAXID_COL_NAME : eVANCE_MS_TAXID_COL_NAME
             );
    }

    private void GridShow_FIPS()
    {
        SetVisibleForDrilldownGridColumns(
             DBANAME_COL_NAME, FIPS_TENANT_COL_NAME, FIPS_SUBMERCHANT_COL_NAME, SPHERE_ADDR_COL_NAME, SPR_ADDR_COL_NAME, eVANCE_CHAIN_COL_NAME, CheckCSViewFullTextView() ? PAYA_CS_TAXID_COL_NAME : PAYA_MS_TAXID_COL_NAME,
             PAYA_MCCSIC_COL_NAME, PAYA_PHONE_COL_NAME, PAYA_MAIL_COL_NAME
             , PAYA_OPENDATE_COL_NAME, PAYA_STATUS_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME);
    }

    private void GridShow_PAYA()
    {
        SetVisibleForDrilldownGridColumns(
             DBANAME_COL_NAME, SPHERE_ADDR_COL_NAME, SPR_ADDR_COL_NAME, PAYA_CHAIN_COL_NAME, CheckCSViewFullTextView() ? PAYA_CS_TAXID_COL_NAME : PAYA_MS_TAXID_COL_NAME, PAYA_MCCSIC_COL_NAME, PAYA_PHONE_COL_NAME, PAYA_MAIL_COL_NAME,
             PAYA_STATUS_COL_NAME, PAYA_OPENDATE_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME
             );

    }

    private void GridShow_MLS()
    {
        SetVisibleForDrilldownGridColumns(
            DBANAME_COL_NAME, BANK_COL_NAME, Group_COL_NAME, SPHEREASSOCIATION_COL_NAME, SPHERE_ADDR_COL_NAME, SPRASSOCIATION_COL_NAME, SPR_ADDR_COL_NAME, CHN_COL_NAME, SECONDARYACCESSCHAIN_COL_NAME,
            CheckCSViewFullCard() ? SPHERECSTaxID_COL_NAME : SPHEREMSTaxID_COL_NAME, SPHEREMCCSIC_COL_NAME, PHONE_COL_NAME,
            CheckCSViewFullCard() ? SPRCSTaxID_COL_NAME : SPRMSTaxID_COL_NAME, SPRMCCSIC_COL_NAME,
            SPHEREEmail_COL_NAME, SPREmail_COL_NAME, eVANCE_OPENDATE_COL_NAME, eVANCE_STATUS_COL_NAME, LAST_BATCH_ACTIVITY_COL_NAME);
    }


    private bool CheckCSViewFullCard()
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
        {
            return true;
        }
        return false;
    }

    private bool CheckCSViewFullTextView()
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_TAX_ID))
        {
            return true;
        }
        return false;
    }

    private void SetVisibleForDrilldownGridColumns(params string[] columns)
    {
        // Set visible for Drilldown & MerchantNumber columns
        string entityIDDisable = "," + GeneralFuncsLib.GetDataOfExtendedSetting("InvisibleHyperlinkByEntityTypeIDs") + ",";
        string entityTypeCurrent = "," + SessionManager.CurrentUser.EntityType.ToString() + ",";
        if (entityIDDisable.Contains(entityTypeCurrent))
        {
            uxDrilldownGrid.Columns.FindByUniqueName(DRILL_DOWN_COL_NAME).Visible = false;
            uxDrilldownGrid.Columns.FindByUniqueName(MER_NUMBER_COL_NAME).Visible = true;
        }
        else
        {
            uxDrilldownGrid.Columns.FindByUniqueName(DRILL_DOWN_COL_NAME).Visible = true;
            uxDrilldownGrid.Columns.FindByUniqueName(MER_NUMBER_COL_NAME).Visible = false;
        }

        // Set visible for columns of each client
        foreach (var colName in columns)
        {
            if (string.IsNullOrEmpty(colName))
            {
                continue;
            }
            uxDrilldownGrid.Columns.FindByUniqueName(colName).Visible = true;
        }
    }

    private void EnableMerchantGridColumns()
    {
        GridNotShow_All();
        if (SessionManager.CurrentUser.SiteID == 0)
        {

            switch (SessionManager.CurrentUser.ASClient)
            {
                case WebSiteConstants.PIVOT_CLIENT:
                case WebSiteConstants.DEMO_FDR_CLIENT:
                    {
                        GridShow_FDR();
                        GridShow_TSYS();
                    }
                    break;
                case WebSiteConstants.FULTON_CLIENT:
                    GridShow_FULTON();
                    break;
                case WebSiteConstants.MPS_CLIENT:
                    GridShow_MPS();
                    break;
                case WebSiteConstants.ORION_CLIENT:
                case WebSiteConstants.SOFTEK_CLIENT:
                    GridShow_SOFTEK();
                    break;
                case WebSiteConstants.DEMO_GLOBAL_CLIENT:
                    GridShow_FULTONDemo();
                    break;
                case WebSiteConstants.SNET_CLIENT:
                    GridShow_SNET();
                    break;
                case WebSiteConstants.FIS_CLIENT:
                case WebSiteConstants.FIS_CLIENT_DEMO:
                    GridShow_FIS();
                    break;
                case WebSiteConstants.PPI_CLIENT:
                    GridShow_ORI();
                    break;
                case WebSiteConstants.WRFC_CLIENT:
                    GridShow_WRFC();
                    break;
                case WebSiteConstants.TNBCI_CLIENT:
                    GridShow_TNBCI();
                    break;
                case WebSiteConstants.NTS_CLIENT:
                    GridShow_NTS();
                    break;
                case WebSiteConstants.IPMT_CLIENT:
                    {
                        GridShow_IPMT();
                    }
                    break;
                case WebSiteConstants.CAYAN_CLIENT:
                    {
                        GridShow_CAYAN();
                    }
                    break;
                // Remove Payline
                case WebSiteConstants.MONETARY_CLIENT:
                    {
                        GridShow_MONETARY();
                    }
                    break;
                case WebSiteConstants.CLEARENT_CLIENT:
                    {
                        GridShow_CLEARENT();
                    }
                    break;
                case GREENBOX_CLIENT:
                    {
                        GridShow_GREENBOX();
                    }
                    break;
                case WebSiteConstants.SIGNAPAY_CLIENT:
                    {
                        GridShow_SIGNAPAY();
                    }
                    break;
                case WebSiteConstants.ALLIEDWALLET_CLIENT:
                    {
                        GridShow_ALLIEDWALLET();
                    }
                    break;
                case WebSiteConstants.SPHERE_CLIENT:
                    {
                        GridShow_SPHERE();
                    }
                    break;
                case WebSiteConstants.RS2_CLIENT:
                    {
                        GridShow_RS2();
                    }
                    break;

                case WebSiteConstants.FIPS_CLIENT:
                    {
                        GridShow_FIPS();
                    }
                    break;
                case WebSiteConstants.PAYA_CLIENT:
                    {
                        GridShow_PAYA();
                    }
                    break;
                case WebSiteConstants.MLS_CLIENT:
                    GridShow_MLS();
                    break;
            }
        }
        else
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new AS.Common.DBManager.FilterParameter("@SiteID", SessionManager.CurrentUser.SiteID, System.Data.DbType.Int32));
            parameters.Add(new AS.Common.DBManager.FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, System.Data.DbType.Int32));
            DataTable table = WebServices.CsReportServices.GetReports(SPA_GET_BE_PROCESSOR, parameters);
            if (table != null && table.Rows.Count > 0)
            {
                //TK: 42811 - VW - Risk CR-Design Change to 39919 (Issue #34910)
                string expression = "BEProcessor = '{0}'";
                if (table.Select(string.Format(expression, BackEndProcessor.FDR)).Length > 0)
                    BEProcessor = BackEndProcessor.FDR.ToString();
                else if (table.Select(string.Format(expression, BackEndProcessor.TSYS)).Length > 0)
                    BEProcessor = BackEndProcessor.TSYS.ToString();
                else if (table.Select(string.Format(expression, BackEndProcessor.PLANET)).Length > 0)
                    BEProcessor = BackEndProcessor.PLANET.ToString();

                switch (SessionManager.CurrentUser.ASClient)
                {
                    case WebSiteConstants.PIVOT_CLIENT:
                    case WebSiteConstants.DEMO_FDR_CLIENT:
                        {
                            if (BEProcessor.Equals(BackEndProcessor.FDR, StringComparison.OrdinalIgnoreCase))
                            {
                                GridShow_FDR();
                            }
                            else if (BEProcessor.Equals(BackEndProcessor.TSYS, StringComparison.OrdinalIgnoreCase))
                            {
                                GridShow_TSYS();
                            }
                            else if (BEProcessor.Equals(BackEndProcessor.PLANET, StringComparison.OrdinalIgnoreCase))
                            {
                                GridShow_PLANET();
                            }
                        }
                        break;
                    case WebSiteConstants.FULTON_CLIENT:
                        GridShow_ORI();
                        break;
                    case WebSiteConstants.MPS_CLIENT:
                        GridShow_MPS();
                        break;
                    case WebSiteConstants.ORION_CLIENT:
                    case WebSiteConstants.SOFTEK_CLIENT:
                        GridShow_SOFTEK();
                        break;
                    case WebSiteConstants.DEMO_GLOBAL_CLIENT:
                        GridShow_FULTONDemo();
                        break;
                    case WebSiteConstants.SNET_CLIENT:
                        GridShow_ORI();
                        break;
                    case WebSiteConstants.FIS_CLIENT:
                    case WebSiteConstants.FIS_CLIENT_DEMO:
                        GridShow_FIS();
                        break;
                    case WebSiteConstants.PPI_CLIENT:
                        GridShow_ORI();
                        break;
                    case WebSiteConstants.TNBCI_CLIENT:
                        GridShow_TNBCI();
                        break;
                    case WebSiteConstants.WRFC_CLIENT:
                        GridShow_WRFC();
                        break;
                    case WebSiteConstants.NTS_CLIENT:
                        GridShow_NTS();
                        break;
                    case WebSiteConstants.IPMT_CLIENT:
                        {

                            GridShow_IPMT();
                        }
                        break;
                    case WebSiteConstants.CAYAN_CLIENT:
                        {
                            GridShow_CAYAN();
                        }
                        break;
                    // Remove Payline
                    case WebSiteConstants.MONETARY_CLIENT:
                        {
                            GridShow_MONETARY();
                        }
                        break;
                    case WebSiteConstants.CLEARENT_CLIENT:
                        {
                            GridShow_CLEARENT();
                        }
                        break;
                    case GREENBOX_CLIENT:
                        {
                            GridShow_GREENBOX();
                        }
                        break;
                    case WebSiteConstants.SIGNAPAY_CLIENT:
                        {
                            GridShow_SIGNAPAY();
                        }
                        break;

                    case WebSiteConstants.ALLIEDWALLET_CLIENT:
                        {
                            GridShow_ALLIEDWALLET();
                        }
                        break;
                    case WebSiteConstants.SPHERE_CLIENT:
                        {
                            GridShow_SPHERE();
                        }
                        break;
                    case WebSiteConstants.RS2_CLIENT:
                        {
                            GridShow_RS2();
                        }
                        break;

                    case WebSiteConstants.FIPS_CLIENT:
                        {
                            GridShow_FIPS();
                        }
                        break;

                    case WebSiteConstants.PAYA_CLIENT:
                        {
                            GridShow_PAYA();
                        }
                        break;
                    case WebSiteConstants.MLS_CLIENT:
                        {
                            GridShow_MLS();
                        }
                        break;
                }
            }
        }
        EnableUserIDColumn();
        ExcludeConfiguredColumns();
    }

    private void EnableUserIDColumn()
    {
        uxDrilldownGrid.Columns.FindByUniqueName(USER_ID_COL_NAME).Visible = GeneralFuncsLib.IsUserSignOn();
    }

    private void ExcludeConfiguredColumns()
    {
        if (GeneralFuncsLib.HasExtendedSetting("MerchantProfile_ExcludedColumns"))
        {
            string[] excludedColumns = GeneralFuncsLib.GetDataOfExtendedSetting("MerchantProfile_ExcludedColumns")
                .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string column in excludedColumns)
            {
                if (!column.IsNullOrEmpty())
                {
                    uxDrilldownGrid.Columns.FindByUniqueName(column).Visible = false;
                }
            }
        }
    }

    private void ShowMerchantDetail(MerchantDetailType merchantDetail)
    {
        uxMIF_MerchantDetails_TSYS.Visible = false;
        uxMIF_MerchantDetails_FDR.Visible = false;
        uxMIF_MerchantDetails_PLANET.Visible = false;
        uxMIF_MerchantDetails_MPS.Visible = false;
        uxMIF_MerchantDetails_ORI.Visible = false;
        uxMIF_MerchantDetails_FIS.Visible = false;
        uxMIF_MerchantDetails_FISMPS.Visible = false;
        uxMIF_MerchantDetails_EMS.Visible = false;
        uxMIF_MerchantDetails_Fulton.Visible = false;
        uxMIF_MerchantDetails_PPI.Visible = false;
        uxMIF_MerchantDetails_TNBCI.Visible = false;
        uxMIF_MerchantDetails_NORTH.Visible = false;
        uxMIF_MerchantDetails_NTS.Visible = false;
        uxMIF_MerchantDetails_WRFC.Visible = false;
        uxMIF_MechantDetails_IPMT_Omaha.Visible = false;
        uxMIF_MechantDetails_IPMT_North.Visible = false;
        uxMIF_MechantDetails_CAYAN.Visible = false;
        //42782 – VW – CAYAN - Implement New TSYS Processing Platform - add
        uxMIF_MechantDetails_TSYS_CAYAN.Visible = false;
        uxMIF_MechantDetails_SOFTEK.Visible = false;
        uxMIF_MechantDetails_TNBCI_TSYS.Visible = false;
        // Remove Payline
        uxMIF_MechantDetails_MONETARY.Visible = false;
        uxMIF_MerchantDetails_CLEARENT.Visible = false;
        uxMIF_MerchantDetails_GREENBOX.Visible = false;
        uxMIF_MerchantDetails_ALLIEDWALLET.Visible = false;
        // 44005
        uxMIF_MerchantDetails_SignaPay.Visible = false;
        uxMIF_MerchantDetails_FultonDemo.Visible = false;
        uxMIF_MerchantDetails_SPHERE.Visible = false;

        uxMIF_MerchantDetails_RS2.Visible = false;
        uxMIF_MerchantDetails_FIPS.Visible = false;
        uxMIF_MerchantDetails_PAYA.Visible = false;
        uxMIF_MerchantDetails_MLS.Visible = false;
        uxMIF_MerchantDetails_PCS.Visible = false;

        AjaxSetting ajax = new AjaxSetting();
        switch (merchantDetail)
        {
            case MerchantDetailType.TSYS:
                uxMIF_MerchantDetails_TSYS.Visible = true;
                uxMIF_MerchantDetails_TSYS.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_TSYS);
                break;
            case MerchantDetailType.FDR:
                uxMIF_MerchantDetails_FDR.Visible = true;
                uxMIF_MerchantDetails_FDR.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_FDR);
                break;
            case MerchantDetailType.PLANET:
                uxMIF_MerchantDetails_PLANET.Visible = true;
                uxMIF_MerchantDetails_PLANET.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_PLANET);
                break;
            case MerchantDetailType.MPS:
                uxMIF_MerchantDetails_MPS.Visible = true;
                uxMIF_MerchantDetails_MPS.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_MPS);
                break;
            case MerchantDetailType.ORI:
                uxMIF_MerchantDetails_ORI.Visible = true;
                uxMIF_MerchantDetails_ORI.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_ORI);
                break;
            case MerchantDetailType.FIS:
                uxMIF_MerchantDetails_FIS.Visible = true;
                uxMIF_MerchantDetails_FIS.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_FIS);
                break;
            case MerchantDetailType.FISMPS:
                uxMIF_MerchantDetails_FISMPS.Visible = true;
                uxMIF_MerchantDetails_FISMPS.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_FISMPS);
                break;
            case MerchantDetailType.EMS:
                uxMIF_MerchantDetails_EMS.Visible = true;
                uxMIF_MerchantDetails_EMS.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_EMS);
                break;
            case MerchantDetailType.Fulton:
                uxMIF_MerchantDetails_Fulton.Visible = true;
                uxMIF_MerchantDetails_Fulton.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_Fulton);
                break;
            case MerchantDetailType.PPI:
                uxMIF_MerchantDetails_PPI.Visible = true;
                uxMIF_MerchantDetails_PPI.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_PPI);
                break;
            case MerchantDetailType.TNBCI:
                uxMIF_MerchantDetails_TNBCI.Visible = true;
                uxMIF_MerchantDetails_TNBCI.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_TNBCI);
                break;
            case MerchantDetailType.North:
                uxMIF_MerchantDetails_NORTH.Visible = true;
                uxMIF_MerchantDetails_NORTH.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_NORTH);
                break;
            case MerchantDetailType.NTS:
                uxMIF_MerchantDetails_NTS.Visible = true;
                uxMIF_MerchantDetails_NTS.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_NTS);
                break;
            case MerchantDetailType.WRFC:
                uxMIF_MerchantDetails_WRFC.Visible = true;
                uxMIF_MerchantDetails_WRFC.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_WRFC);
                break;
            case MerchantDetailType.FDR_NORTH:
                IsForceInvisibleMemo = true;
                uxMIF_MechantDetails_IPMT_North.Visible = true;
                uxMIF_MechantDetails_IPMT_North.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MechantDetails_IPMT_North);
                break;
            case MerchantDetailType.OMAHA:
                uxMIF_MechantDetails_IPMT_Omaha.Visible = true;
                uxMIF_MechantDetails_IPMT_Omaha.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MechantDetails_IPMT_Omaha);
                break;
            case MerchantDetailType.CAYAN:
                uxMIF_MechantDetails_CAYAN.Visible = true;
                uxMIF_MechantDetails_CAYAN.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MechantDetails_CAYAN);
                break;
            //42782 – VW – CAYAN - Implement New TSYS Processing Platform - add
            case MerchantDetailType.CAYAN_TSYS:
                uxMIF_MechantDetails_TSYS_CAYAN.Visible = true;
                uxMIF_MechantDetails_TSYS_CAYAN.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MechantDetails_TSYS_CAYAN);
                break;
            case MerchantDetailType.SOFTEK:
                uxMIF_MechantDetails_SOFTEK.Visible = true;
                uxMIF_MechantDetails_SOFTEK.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MechantDetails_SOFTEK);
                break;
            case MerchantDetailType.TNBCI_TSYS:
                uxMIF_MechantDetails_TNBCI_TSYS.Visible = true;
                uxMIF_MechantDetails_TNBCI_TSYS.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MechantDetails_TNBCI_TSYS);
                break;
            // Remove Payline
            case MerchantDetailType.MONETARY:
                uxMIF_MechantDetails_MONETARY.Visible = true;
                uxMIF_MechantDetails_MONETARY.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MechantDetails_MONETARY);
                break;
            case MerchantDetailType.CLEARENT:
                uxMIF_MerchantDetails_CLEARENT.Visible = true;
                uxMIF_MerchantDetails_CLEARENT.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_CLEARENT);
                break;
            case MerchantDetailType.GREENBOX:
                uxMIF_MerchantDetails_GREENBOX.Visible = true;
                uxMIF_MerchantDetails_GREENBOX.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_GREENBOX);
                break;
            case MerchantDetailType.SignaPay:
                uxMIF_MerchantDetails_SignaPay.Visible = true;
                uxMIF_MerchantDetails_SignaPay.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_SignaPay);
                break;
            case MerchantDetailType.ALLIEDWALLET:
                uxMIF_MerchantDetails_ALLIEDWALLET.Visible = true;
                uxMIF_MerchantDetails_ALLIEDWALLET.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_ALLIEDWALLET);
                break;
            case MerchantDetailType.FultonDemo:
                uxMIF_MerchantDetails_FultonDemo.Visible = true;
                uxMIF_MerchantDetails_FultonDemo.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_FultonDemo);
                break;
            case MerchantDetailType.SPHERE:
                uxMIF_MerchantDetails_SPHERE.Visible = true;
                uxMIF_MerchantDetails_SPHERE.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_SPHERE);
                break;
            case MerchantDetailType.RS2:
                uxMIF_MerchantDetails_RS2.Visible = true;
                uxMIF_MerchantDetails_RS2.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_RS2);
                break;
            case MerchantDetailType.FIPS:
                uxMIF_MerchantDetails_FIPS.Visible = true;
                uxMIF_MerchantDetails_FIPS.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_FIPS);
                break;
            case MerchantDetailType.Paya:
                uxMIF_MerchantDetails_PAYA.Visible = true;
                uxMIF_MerchantDetails_PAYA.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_PAYA);
                break;
            case MerchantDetailType.MLS:
                uxMIF_MerchantDetails_MLS.Visible = true;
                uxMIF_MerchantDetails_MLS.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_MLS);
                break;
            case MerchantDetailType.PCS:
                uxMIF_MerchantDetails_PCS.Visible = true;
                uxMIF_MerchantDetails_PCS.Rebind();
                BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_PCS);
                break;
        }
    }

    private void LoadUserControls(string merchantNumber)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new AS.Common.DBManager.FilterParameter("@MerchantNumber", merchantNumber, System.Data.DbType.String));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, System.Data.DbType.Int32));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@SiteID", SessionManager.CurrentUser.SiteID, System.Data.DbType.Int32));
        DataTable table = WebServices.CsReportServices.GetReports(SPA_GET_BE_PROCESSOR, parameters);
        if (table != null && table.Rows.Count > 0)
        {
            string beProcessor = table.Rows[0]["BEProcessor"].ToString();

            // If client has multi left menu then we will switch menu left by processor.
            MecchantProfileNavigator.Processor = beProcessor;

            switch (SessionManager.CurrentUser.ASClient)
            {
                case WebSiteConstants.PIVOT_CLIENT:
                case WebSiteConstants.DEMO_FDR_CLIENT:
                    if (beProcessor.Equals(BackEndProcessor.TSYS, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowMerchantDetail(MerchantDetailType.TSYS);
                    }
                    else if (beProcessor.Equals(BackEndProcessor.FDR, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowMerchantDetail(MerchantDetailType.FDR);
                    }
                    else if (beProcessor.Equals(BackEndProcessor.PLANET, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowMerchantDetail(MerchantDetailType.PLANET);
                    }
                    else if (beProcessor.Equals(BackEndProcessor.NORTH, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowMerchantDetail(MerchantDetailType.North);
                    }
                    else if (beProcessor.Equals(BackEndProcessor.PCS, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowMerchantDetail(MerchantDetailType.PCS);
                    }
                    break;
                case WebSiteConstants.MPS_CLIENT:
                case WebSiteConstants.DEMO_GLOBAL_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.FultonDemo);
                    break;
                case WebSiteConstants.ORION_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.ORI);
                    break;
                case WebSiteConstants.SNET_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.SNET);
                    break;
                case WebSiteConstants.FIS_CLIENT:
                case WebSiteConstants.FIS_CLIENT_DEMO:
                    if (beProcessor.Equals(BackEndProcessor.MPS, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowMerchantDetail(MerchantDetailType.FISMPS);
                    }
                    else
                    {
                        ShowMerchantDetail(MerchantDetailType.FIS);
                    }
                    break;
                case WebSiteConstants.FULTON_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.Fulton);
                    break;
                case WebSiteConstants.PPI_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.PPI);
                    break;
                case WebSiteConstants.WRFC_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.WRFC);
                    break;
                case WebSiteConstants.TNBCI_CLIENT:
                    if (beProcessor.Equals(BackEndProcessor.TSYS, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowMerchantDetail(MerchantDetailType.TNBCI_TSYS);
                    }
                    else
                    {
                        ShowMerchantDetail(MerchantDetailType.TNBCI);
                    }
                    break;
                case WebSiteConstants.NTS_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.NTS);
                    break;
                case WebSiteConstants.IPMT_CLIENT:
                    if (beProcessor.Equals(BackEndProcessor.NORTH, StringComparison.OrdinalIgnoreCase))
                    {
                        ShowMerchantDetail(MerchantDetailType.FDR_NORTH);
                    }
                    else
                    {
                        ShowMerchantDetail(MerchantDetailType.OMAHA);
                    }
                    break;
                case WebSiteConstants.CAYAN_CLIENT:
                    {
                        //42782 – VW – CAYAN - Implement New TSYS Processing Platform - modify
                        if (beProcessor.Equals(BackEndProcessor.TSYS, StringComparison.OrdinalIgnoreCase))
                        {
                            ShowMerchantDetail(MerchantDetailType.CAYAN_TSYS);
                        }
                        else
                        {
                            ShowMerchantDetail(MerchantDetailType.CAYAN);
                        }
                    }
                    break;
                case WebSiteConstants.SOFTEK_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.SOFTEK);
                    break;
                // Remove Payline
                case WebSiteConstants.MONETARY_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.MONETARY);
                    break;
                case WebSiteConstants.CLEARENT_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.CLEARENT);
                    break;
                case GREENBOX_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.GREENBOX);
                    break;
                case WebSiteConstants.SIGNAPAY_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.SignaPay);
                    break;
                case WebSiteConstants.ALLIEDWALLET_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.ALLIEDWALLET);
                    break;

                case WebSiteConstants.SPHERE_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.SPHERE);
                    break;
                case WebSiteConstants.RS2_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.RS2);
                    break;

                case WebSiteConstants.FIPS_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.FIPS);
                    break;
                case WebSiteConstants.PAYA_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.Paya);
                    break;
                case WebSiteConstants.MLS_CLIENT:
                    ShowMerchantDetail(MerchantDetailType.MLS);
                    break;
            }
        }
    }

    private void CheckMerchantFromQueryString()
    {
        if (IsSecureQueryString)
        {
            if (SecureQueryString["MerchantNumber"] != null)
            {
                MerchantNumber = SecureQueryString["MerchantNumber"];
                DoSwitchView();
            }
            else if (_HierarchySelected != -1 && !_FilterValue.IsNullOrEmpty())
            {
                DoSwitchView();
            }
            if (SecureQueryString["FromExCM"] == "1")
            {
                uxReportFiltering.Visible = false;
                uxPageTitleCM.Visible = true;
                uxPageTitle.Visible = false;
            }
            else
            {
                uxReportFiltering.Visible = true;
                uxPageTitleCM.Visible = false;
                uxPageTitle.Visible = true;
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(Request["mc"]))
            {
                MerchantNumber = WebServices.ApiServices.DecryptText(Request["mc"]);
                DoSwitchView();
            }
        }
    }

    private bool IsMerchantBelongToUser()
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserReportingParams(true);
        paramsIn.AddHierarchyFilterParams(this);
        DataTable dt = WebServices.SecurityServices.GetReports(SPA_CHECK_MERCHANT_BELONG_TO_USER, paramsIn);
        // Belonged to user
        return dt != null && dt.Rows.Count > 0 && (bool)dt.Rows[0][0];
    }

    private string GetActivity(bool isMerchant)
    {
        string hierachyMode = ReportFilter.CurrentValue.HierarchyMode;
        string hierachyValue = ReportFilter.CurrentValue.Value;
        string value = string.Empty;
        string hierarchyModeValue = string.Empty;
        foreach (DataRow row in SessionManager.HierarchyFilterDrillDown.Rows)
        {
            if (row["CurrentHierarchyMode"].ToString() == hierachyMode)
            {

                hierarchyModeValue = row["CurrentHierarchyGridName"].ToString();
            }
        }
        if (isMerchant)
        {
            value = GetLocalResourceObject("MerchantProfile_aspx_cs_View").ToString() + " " + hierachyValue;
        }
        else
        {
            if (string.IsNullOrEmpty(hierachyValue))
            {
                if (GeneralFuncsLib.IsMerchantMode(hierachyMode))
                    value = GetLocalResourceObject("MerchantProfile_aspx_cs_SearchAllMerchants").ToString();
                else
                    value = "Search all " + hierarchyModeValue + "s";
            }
            else
            {
                value = string.Format(GetLocalResourceObject("MerchantProfile_aspx_cs_SearchBy").ToString(), hierarchyModeValue, hierachyValue);
            }
        }
        return string.Format(GetLocalResourceObject("MerchantProfile_aspx_cs_MerchantProfile").ToString(), value);
    }

    private void GoBackSetting()
    {
        if (RiskSessionManager.MerchantProfileReferrer.Length > 0 && RiskSessionManager.MerchantProfileReferrerInfo.Key.Length > 0
            && string.Compare(RiskSessionManager.MerchantProfileReferrerInfo.Key, this.MerchantNumber) == 0
            && RiskSessionManager.MerchantProfileReferrerInfo.Url.Length > 0)
        {
            int isMCFRisk = RiskSessionManager.Risk_ModeMCF;
            var prefix = isMCFRisk == (int)ShowRiskMCF.MCFRisk ? "/risk_MCF/{0}" : "/risk/{0}";
            var url = string.Format(prefix, RiskSessionManager.MerchantProfileReferrerInfo.Url);
            uxGoBack.NavigateUrl = VeraCodeSolution.ValidateResponseData(url);
            uxGoBack.Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("MerchantProfile_aspx_cs_BackTo").ToString() + " " + RiskSessionManager.MerchantProfileReferrerInfo.Title);
            uxGoBack.Visible = true;
        }
        else
        {
            uxGoBack.Visible = false;
            RiskSessionManager.MerchantProfileReferrer = null;
            //Fix issue SP #275
            if (!GeneralFuncsLib.IsAddGeographicData())
            {
                RiskSessionManager.MerchantProfileReferrerInfo = null;
            }
        }
    }

    private void BindAjaxuxMIF_MerchantDetail(GlobalUserControl control)
    {
        var ajax = new AjaxSetting();
        ajax.AjaxControlID = uxReloadHierachy.ID;
        ajax.UpdatedControls.Add(new AjaxUpdatedControl(control.ID, string.Empty));
        if (!uxRadAjaxManager.AjaxSettings.Contains(ajax))
            uxRadAjaxManager.AjaxSettings.Add(ajax);
    }
    #endregion Methods

    #endregion Methods
    //TK39919 - Add
    protected void uxReportFiltering_Filtering(object sender, EventArgs e)
    {
        //10/4/2018 | 45128 Merchant Notes Issue - Prod
        //Remove code
        if (ReportFilter.CurrentValue.Value.IsNullOrEmpty())
            SessionManager.CurrentMerchantNumber = string.Empty;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string[] CheckSensitiveData(string comment)
    {
        return GeneralFuncsLib.DetectSesitiveData(comment);
    }

    //39919 - Performance PROP Issue
    [WebMethod]
    public static object[] GetRoleAndAddedBy(string sources)
    {
        UserControls_MerchantNote ucMerchantNote = new UserControls_MerchantNote();
        DataTable roles = ucMerchantNote.GetRoles(sources);
        DataTable users = ucMerchantNote.GetAddedBy(sources, string.Empty);

        object[] results = new object[2];
        results[0] = GeneralFuncsLib.DataTableToJson(roles);
        results[1] = GeneralFuncsLib.DataTableToJson(users);
        return results;
    }

    [WebMethod]
    public static object GetAddedBy(string sources, string roles)
    {
        UserControls_MerchantNote ucMerchantNote = new UserControls_MerchantNote();
        DataTable users = ucMerchantNote.GetAddedBy(sources, roles);

        return GeneralFuncsLib.DataTableToJson(users);
    }

    [WebMethod]
    public static object GetSourceAndRole()
    {
        UserControls_MerchantNote ucMerchantNote = new UserControls_MerchantNote();
        string defaultSources = PersonalDataHelper.GetJSONConfig<string>(UserConfigNames.CONFIG_MERCHANT_PROFILE_SOURCE_DEFAULT_SETTING);
        DataTable sources = ucMerchantNote.GetSources();
        object[] results = new object[4];

        results[0] = defaultSources;
        results[1] = GeneralFuncsLib.DataTableToJson(sources);
        results[2] = GeneralFuncsLib.DataTableToJson(ucMerchantNote.GetRoles(defaultSources));
        results[3] = GeneralFuncsLib.DataTableToJson(ucMerchantNote.GetAddedBy(defaultSources, string.Empty));

        return results;
    }

    [WebMethod]
    public static object GetRoles(string sources)
    {
        UserControls_MerchantNote ucMerchantNote = new UserControls_MerchantNote();
        DataTable roles = ucMerchantNote.GetRoles(sources);

        return GeneralFuncsLib.DataTableToJson(roles);
    }

    protected void uxDrilldownGrid_PreRender(object sender, EventArgs e)
    {
        if (_HierarchySelected != -1 && !_FilterValue.IsNullOrEmpty() && !IsPostBack)
        {
            AjaxAddResponseScript(string.Format("rf_DrilldownReportFilterValuesCM('{0}', '{1}', '{2}');", GeneralFuncsLib.GetHierarchyInfo(_HierarchySelected).HierarchyID,
                GeneralFuncsLib.GetHierarchyInfo(_HierarchySelected).HierarchyMode.ToString(), _FilterValue));
        }
    }
}
