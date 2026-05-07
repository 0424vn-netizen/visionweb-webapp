using System;
using System.Data;
using System.Text;
using System.IO;
using System.Web.UI.WebControls;
using AS.Common;
using AS.Common.DBManager;
using AS.Web.Business;
using AS.Controls.Grid;
using AS.Security.WS.Entities;
using System.Web.UI.HtmlControls;
using AS.Common.Formater;
using AS.Controls.Pages;
using System.Collections.Generic;
using System.Configuration;

public partial class UserControls_MIF_MechantDetails_IPMT_North : ExportMultiSections
{
    #region Propertise using for Case Management
    private DataTable _MerchantInfor;
    private DataTable _ProgramCodeDetails;
    private string CSSRow;
    #endregion

    protected enum DataBindAction
    {
        BindMerchantDetails,
        BindProgramCodeDetails,
    }

    #region Constants

    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string FILE_NAME_HIERARCHY_FDR_NORTH_INFO = "~/App_Data/tpl_HierarchyInformation_FDR_NORTH.htm";
    private const string OPTED_IN = "Opted In";
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation_NORTH.htm";
    private const string FILE_NAME_FUNDING_CATEGORY = "~/App_Data/tpl_FundingCategory_NORTH.htm";
    private const string FILE_NAME_PROGRAM_DETAILS = "~/App_Data/tpl_ProgramDetails_NORTH.htm";

    // SPA Name
    private const string SPA_GET_MERCHANT_FDR_NORTH_PROFILE = "spa_cs_GetMerchantProfile_FDR_NORTH_V2";
    private const string SPA_UPDATE_CUSTOM_CHAIN_REPORT = "spa_ms_UpdateCustomChainReport";

    #endregion Constants

    #region Properties

    public DataTable _MifTable
    {
        get
        {
            if (ViewState["MerchantInfomation"] != null)
                return (DataTable)(ViewState["MerchantInfomation"]);
            else
                return null;
        }
        set
        {
            ViewState["MerchantInfomation"] = value;
        }
    }

    protected string OptedIn
    {
        get
        {
            return (string)ViewState["OptStatus"];
        }
        set
        {
            ViewState["OptStatus"] = value;
        }
    }

    public string MerchantNumber
    {
        get
        {
            return (string)ViewState["MerchantNumber"];
        }
        set
        {
            ViewState["MerchantNumber"] = value;
        }
    }

    #endregion Properties

    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (GeneralFuncsLib.IsUserSignOn())
            {
                uxlblUserID.Text = VeraCodeSolution.DoVeraCode(Resources.Template.UserID);
                uxSiteAccess.Text = VeraCodeSolution.DoVeraCode(Resources.Template.SiteAccess);
                uxPnlUserID.Visible = true;
                uxPnlSiteAccess.Visible = MerchantProfileHelper.HasSiteAccessPermission(this.Page);
            }
        }
    }

    protected void uxSiteAccess_click(object sender, EventArgs e)
    {
        string userID = string.Empty;
        if (_MifTable.IsNotNullData() && _MifTable.Columns.Contains("UserID"))
        {
            userID = _MifTable.Rows[0]["UserID"].ToString();
        }
        MerchantProfileHelper.SiteAccess(this.Page, userID);
    }

    protected void ResponseScript(string script)
    {
        this.Page.RegisterStartupScript("script", script);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        base.OnDataBindControls(type, sender);

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMerchantDetails:
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams();
                parameters.AddLoggedInUserPrimaryUserID();
                parameters.Add(new FilterParameter("@MerchantNumber", GetMerchantNrParameter(), DbType.AnsiString));
                parameters.Add(new FilterParameter("@FlagPermission", SessionManager.CurrentUserViewMode, DbType.Int32));
                parameters.AddLanguageID();
                parameters.AddDecryptDataParams("RoutingNumber");
                DataTable dtMerch = WebServices.CsReportServices.GetReports(SPA_GET_MERCHANT_FDR_NORTH_PROFILE, parameters);

                if (dtMerch.Rows.Count > 0)
                {
                    _MifTable = dtMerch;
                    this.OptedIn = dtMerch.Rows[0]["SiteAccess"].ToString().Equals(Resources.Template.OptedIn, StringComparison.OrdinalIgnoreCase) ? "1" : "0";

                    this._MerchantInfor = dtMerch;

                    HtmlAnchor bank = (HtmlAnchor)uxBank.FindControl("uxLinkBank");
                    HtmlAnchor agent = (HtmlAnchor)uxAgent.FindControl("uxLinkAgent");
                    HtmlAnchor corp = (HtmlAnchor)uxNCorp.FindControl("uxLinkNCorp");
                    HtmlAnchor chain = (HtmlAnchor)uxChain.FindControl("uxLinkChain");

                    string bankValue = dtMerch.Rows[0]["Bank"].ToString();
                    string agentValue = dtMerch.Rows[0]["Agent"].ToString();
                    string corpValue = dtMerch.Rows[0]["Corp"].ToString();
                    string chainValue = dtMerch.Rows[0]["Chain"].ToString();

                    if (bank != null && CheckHierarchy(HierarchyMode.IPMT_BANK))
                    {
                        if (!string.IsNullOrEmpty(bankValue))
                        {
                            bank.Visible = true;
                            bank.Attributes.Add(
                                "onclick",
                                MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                    HierarchyMode.IPMT_BANK, bankValue));
                        }
                        else
                        {
                            bank.Visible = false;
                        }
                    }

                    if (agent != null && CheckHierarchy(HierarchyMode.IPMT_AGENT))
                    {
                        if (!string.IsNullOrEmpty(agentValue))
                        {
                            agent.Visible = true;
                            agent.Attributes.Add(
                                "onclick",
                                MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                    HierarchyMode.IPMT_AGENT, agentValue));
                        }
                        else
                        {
                            agent.Visible = false;
                        }
                    }
                    if (corp != null && CheckHierarchy(HierarchyMode.IPMT_CORP))
                    {
                        if (!string.IsNullOrEmpty(corpValue))
                        {
                            corp.Visible = true;
                            corp.Attributes.Add(
                                "onclick",
                                MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                    HierarchyMode.IPMT_CORP, corpValue));
                        }
                        else
                        {
                            corp.Visible = false;
                        }
                    }

                    if (chain != null && CheckHierarchy(HierarchyMode.IPMT_CHAIN))
                    {
                        if (!string.IsNullOrEmpty(chainValue))
                        {
                            chain.Visible = true;
                            chain.Attributes.Add(
                                "onclick",
                                MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                    HierarchyMode.IPMT_CHAIN, chainValue));
                        }
                        else
                        {
                            chain.Visible = false;
                        }
                    }

                }
                else
                {
                    phdMerchantDetail.Visible = false;
                }
                break;
            case DataBindAction.BindProgramCodeDetails:
                FilterParameterCollection param = new FilterParameterCollection();
                param.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                param.Add(new FilterParameter("@MerchantNumber", GetMerchantNrParameter(), DbType.AnsiString));
                DataTable dtProgramCodeDetails = WebServices.RiskServices.GetReports("spa_cs_GetMerchantProgramInformation_FDR_NORTH", param);
                this._ProgramCodeDetails = dtProgramCodeDetails;
                if (dtProgramCodeDetails.Rows.Count == 0)
                {
                    programDetailNodata.Visible = true;
                }
                this.rptProgramDetail.DataSource = dtProgramCodeDetails;
                this.rptProgramDetail.DataBind();
                break;
        }
    }

    protected string DoVeraCode(DataRow row, string key)
    {
        if (row == null)
        {
            return string.Empty;
        }

        object obj = row[key];
        if (obj == DBNull.Value || obj == null)
        {
            return string.Empty;
        }
        else
        {
            return obj.ToString();
        }
    }

    protected string FormatDate(DataRow row, string key)
    {
        if (row == null)
        {
            return string.Empty;
        }

        object obj = row[key];
        if (obj == DBNull.Value || obj == null)
        {
            return string.Empty;
        }
        else
        {
            return ((DateTime)obj).ToGenericDateString();
        }
    }

    protected string setOptInStatus(object OptIn, string MerchantStatus)
    {
        return MerchantProfileHelper.SetOptInStatus(OptIn, MerchantStatus);
    }

    public string GetRoutingNumber(string full, string partial)
    {
        return MerchantProfileHelper.GetRoutingNumberWithoutDecrypt(full, partial);
    }

    public string GetTaxDDANumber(string full, string partial)
    {
        full = WebServices.CsReportServices.DecryptText(full, SessionManager.CurrentUser.ASClient);
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
        {
            return partial;
        }
        return full;
    }

    public void Rebind()
    {
        this.DoMultiExportExcel();
        this.OnDataBindControls(DataBindAction.BindMerchantDetails, this);
        this.OnDataBindControls(DataBindAction.BindProgramCodeDetails, this);

        uxBusinessInformation.DataBind();
        uxBillingInformation.DataBind();
        uxBillback.DataBind();
        uxInterchangeCompliance.DataBind();
        uxBillingPricingType.DataBind();
        uxSupplyBilling.DataBind();
        uxEarlyTerminatioFees.DataBind();
        uxImprinter.DataBind();
        uxFees.DataBind();
        uxMerchantCardInformation.DataBind();
        uxAccountCancellation.DataBind();
        uxTerminalNetworkInformation.DataBind();
        uxPTSSettings.DataBind();
        uxAuthorizationReversals.DataBind();
        uxSignatureCapture.DataBind();
        uxTrustKeeper.DataBind();
        uxDiscountInformation.DataBind();
        uxProcessingDates.DataBind();
        uxProgramServicesParticipation.DataBind();
        uxGlobalePricing.DataBind();
        uxPayeezy.DataBind();
        uxTransArmor.DataBind();
        uxLegalIRSInformation.DataBind();
        uxBankingInformation.DataBind();
        uxFundingDetails.DataBind();
        uxACH.DataBind();
        uxFundingExclusion.DataBind();
        uxRevolvingPaymentsPlan.DataBind();
        uxBankwireInformation.DataBind();
        uxSplitFunding.DataBind();
        uxProcessingEmail.DataBind();
        uxStatement.DataBind();
        uxReportingSettings.DataBind();
        uxChargebackInformation.DataBind();
    }
    public bool IsRiskInformation
    {
        get
        {
            return ((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RISK_INFO)
                || ((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_MSRISK_INFO);
        }
    }
    protected override Dictionary<int, Func<string>> GetListFunctions()
    {
        Dictionary<int, Func<string>> exportFucntions = new Dictionary<int, Func<string>>();
        exportFucntions.Add(0, ButtonExcelMerchantInformation_Click);
        if (IsRiskInformation)
        {
            exportFucntions.Add(1, () => { return uxRiskInfo.ExcelRiskInformation(); });
        }
        exportFucntions.Add(2, ButtonExcelHierarchyInformation_Click);
        exportFucntions.Add(3, uxExportLegalIRSInformation.DoExport);
        exportFucntions.Add(4, uxExportBillingInformation.DoExport);
        exportFucntions.Add(5, uxExportBusinessInformation.DoExport);
        exportFucntions.Add(6, uxExportBankingInformation.DoExport);
        //Billing
        exportFucntions.Add(7, uxExportBillback.DoExport);
        exportFucntions.Add(8, uxExportInterchangeCompliance.DoExport);
        exportFucntions.Add(9, uxExportBillingPricingType.DoExport);
        exportFucntions.Add(10, uxExportSupplyBilling.DoExport);
        exportFucntions.Add(11, uxExportEarlyTerminatioFees.DoExport);
        exportFucntions.Add(12, uxExportImprinter.DoExport);
        exportFucntions.Add(13, uxExportFees.DoExport);
        //Funding Information
        exportFucntions.Add(14, ButtonExcelFundingCategory_Click);
        exportFucntions.Add(15, uxExportFundingDetails.DoExport);
        exportFucntions.Add(16, uxExportACH.DoExport);
        exportFucntions.Add(17, uxExportFundingExclusion.DoExport);
        exportFucntions.Add(18, uxExportRevolvingPaymentsPlan.DoExport);
        exportFucntions.Add(19, uxExportBankwireInformation.DoExport);
        exportFucntions.Add(20, uxExporSplitFunding.DoExport);
        //Merchant Card Information
        exportFucntions.Add(21, uxExportMerchantCardInformation.DoExport);
        //Account Cancellation
        exportFucntions.Add(22, uxExportAccountCancellation.DoExport);
        //Transaction Processing
        exportFucntions.Add(23, uxExportTerminalNetworkInformation.DoExport);
        exportFucntions.Add(24, uxExportPTSSettings.DoExport);
        exportFucntions.Add(25, uxExportAuthorizationReversals.DoExport);
        exportFucntions.Add(26, uxExportSignatureCapture.DoExport);
        exportFucntions.Add(27, uxExportTrustKeeper.DoExport);
        //Reporting
        exportFucntions.Add(28, uxExportProcessingEmail.DoExport);
        exportFucntions.Add(29, uxExportStatement.DoExport);
        exportFucntions.Add(30, uxExportReportingSettings.DoExport);
        exportFucntions.Add(31, uxExportChargebackInformation.DoExport);
        //Discount Information
        exportFucntions.Add(32, uxExportDiscountInformation.DoExport);
        //Processing Dates
        exportFucntions.Add(33, uxExportProcessingDates.DoExport);
        //Program & Products
        exportFucntions.Add(34, uxExportProgramServicesParticipation.DoExport);
        exportFucntions.Add(35, uxExportGlobalePricing.DoExport);
        exportFucntions.Add(36, uxExportPayeezy.DoExport);
        exportFucntions.Add(37, uxExportTransArmor.DoExport);
        exportFucntions.Add(38, ButtonExcelProgramDetails_Click);

        return exportFucntions;
    }

    public void DoMultiExportExcel()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();
        exportNames.Add(0, GetLocalResourceObject("Literal1.Text").ToString());
        if (IsRiskInformation)
        {
            exportNames.Add(1, GetLocalResourceObject("MIF_MerchantDetails_Text_RiskInformation").ToString());
        }
        exportNames.Add(2, GetLocalResourceObject("Literal28.Text").ToString());
        exportNames.Add(3, GetLocalResourceObject("uxLegalIRSInformation.FileName").ToString());
        exportNames.Add(4, GetLocalResourceObject("uxExportBillingInformation.Title").ToString());
        exportNames.Add(5, GetLocalResourceObject("uxExportBusinessInformation.Title").ToString());
        exportNames.Add(6, GetLocalResourceObject("uxExportBankingInformation.Title").ToString());
        //Billing
        exportNames.Add(7, string.Format("{0} - {1}", GetLocalResourceObject("uxExportBilling.Title").ToString(), GetLocalResourceObject("uxExportBillback.Title").ToString()));
        exportNames.Add(8, string.Format("{0} - {1}", GetLocalResourceObject("uxExportBilling.Title").ToString(), GetLocalResourceObject("uxExportInterchangeCompliance.Title").ToString()));
        exportNames.Add(9, string.Format("{0} - {1}", GetLocalResourceObject("uxExportBilling.Title").ToString(), GetLocalResourceObject("uxExportBillingPricingType.Title").ToString()));
        exportNames.Add(10, string.Format("{0} - {1}", GetLocalResourceObject("uxExportBilling.Title").ToString(), GetLocalResourceObject("uxExportSupplyBilling.Title").ToString()));
        exportNames.Add(11, string.Format("{0} - {1}", GetLocalResourceObject("uxExportBilling.Title").ToString(), GetLocalResourceObject("uxExportEarlyTerminatioFees.Title").ToString()));
        exportNames.Add(12, string.Format("{0} - {1}", GetLocalResourceObject("uxExportBilling.Title").ToString(), GetLocalResourceObject("uxExportImprinter.Title").ToString()));
        exportNames.Add(13, string.Format("{0} - {1}", GetLocalResourceObject("uxExportBilling.Title").ToString(), GetLocalResourceObject("uxExportFees.Title").ToString()));
        //Funding Information
        exportNames.Add(14, string.Format("{0} - {1}", GetLocalResourceObject("uxExportFundingInformation.Title").ToString(), GetLocalResourceObject("Literal69.Text").ToString()));
        exportNames.Add(15, string.Format("{0} - {1}", GetLocalResourceObject("uxExportFundingInformation.Title").ToString(), GetLocalResourceObject("uxExportFundingDetails.Title").ToString()));
        exportNames.Add(16, string.Format("{0} - {1}", GetLocalResourceObject("uxExportFundingInformation.Title").ToString(), GetLocalResourceObject("uxExportACH.FileName").ToString()));
        exportNames.Add(17, string.Format("{0} - {1}", GetLocalResourceObject("uxExportFundingInformation.Title").ToString(), GetLocalResourceObject("uxExportFundingExclusion.Title").ToString()));
        exportNames.Add(18, string.Format("{0} - {1}", GetLocalResourceObject("uxExportFundingInformation.Title").ToString(), GetLocalResourceObject("uxExportRevolvingPaymentsPlan.Title").ToString()));
        exportNames.Add(19, string.Format("{0} - {1}", GetLocalResourceObject("uxExportFundingInformation.Title").ToString(), GetLocalResourceObject("uxExportBankwireInformation.FileName").ToString()));
        exportNames.Add(20, string.Format("{0} - {1}", GetLocalResourceObject("uxExportFundingInformation.Title").ToString(), GetLocalResourceObject("uxExporSplitFunding.Title").ToString()));
        //Merchant Card Information
        exportNames.Add(21, GetLocalResourceObject("uxExportMerchantCardInformation.FileName").ToString());
        //Account Cancellation
        exportNames.Add(22, GetLocalResourceObject("uxExportAccountCancellation.Title").ToString());
        //Transaction Processing
        exportNames.Add(23, string.Format("{0} - {1}", GetLocalResourceObject("uxExportTransactionProcessing.Title").ToString(), GetLocalResourceObject("uxExportTerminalNetworkInformation.Title").ToString()));
        exportNames.Add(24, string.Format("{0} - {1}", GetLocalResourceObject("uxExportTransactionProcessing.Title").ToString(), GetLocalResourceObject("uxExportPTSSettings.Title").ToString()));
        exportNames.Add(25, string.Format("{0} - {1}", GetLocalResourceObject("uxExportTransactionProcessing.Title").ToString(), GetLocalResourceObject("uxExportAuthorizationReversals.Title").ToString()));
        exportNames.Add(26, string.Format("{0} - {1}", GetLocalResourceObject("uxExportTransactionProcessing.Title").ToString(), GetLocalResourceObject("uxExportSignatureCapture.Title").ToString()));
        exportNames.Add(27, string.Format("{0} - {1}", GetLocalResourceObject("uxExportTransactionProcessing.Title").ToString(), GetLocalResourceObject("uxExportTrustKeeper.Title").ToString()));
        //Reporting
        exportNames.Add(28, string.Format("{0} - {1}", GetLocalResourceObject("uxExportReporting.Title").ToString(), GetLocalResourceObject("uxExportProcessingEmail.FileName").ToString()));
        exportNames.Add(29, string.Format("{0} - {1}", GetLocalResourceObject("uxExportReporting.Title").ToString(), GetLocalResourceObject("uxExportStatement.Title").ToString()));
        exportNames.Add(30, string.Format("{0} - {1}", GetLocalResourceObject("uxExportReporting.Title").ToString(), GetLocalResourceObject("uxExportReportingSettings.Title").ToString()));
        exportNames.Add(31, string.Format("{0} - {1}", GetLocalResourceObject("uxExportReporting.Title").ToString(), GetLocalResourceObject("uxExportChargebackInformation.Title").ToString()));
        //Discount Information
        exportNames.Add(32, GetLocalResourceObject("uxExportDiscountInformation.Title").ToString());
        //Processing Dates
        exportNames.Add(33, GetLocalResourceObject("uxExportProcessingDates.Title").ToString());
        //Program & Products
        exportNames.Add(34, string.Format("{0} - {1}", GetLocalResourceObject("uxExportProgramProducts.Title").ToString(), GetLocalResourceObject("uxExportProgramServicesParticipation.FileName").ToString()));
        exportNames.Add(35, string.Format("{0} - {1}", GetLocalResourceObject("uxExportProgramProducts.Title").ToString(), GetLocalResourceObject("uxExportGlobalePricing.Title").ToString()));
        exportNames.Add(36, string.Format("{0} - {1}", GetLocalResourceObject("uxExportProgramProducts.Title").ToString(), GetLocalResourceObject("uxExportPayeezy.Title").ToString()));
        exportNames.Add(37, string.Format("{0} - {1}", GetLocalResourceObject("uxExportProgramProducts.Title").ToString(), GetLocalResourceObject("uxExportTransArmor.Title").ToString()));
        exportNames.Add(38, string.Format("{0} - {1}", GetLocalResourceObject("uxExportProgramProducts.Title").ToString(), GetLocalResourceObject("Literal68.Text").ToString()));

        ExportSectionNames = exportNames;
    }

    #region Processing Method

    public string BindValue(string colName)
    {
        if (_MerchantInfor == null || _MerchantInfor.Rows.Count <= 0 || !_MerchantInfor.Columns.Contains(colName))
            return string.Empty;
        DataRow dr = _MerchantInfor.Rows[0];
        return dr[colName].ToString();

    }

    protected string BindAddress(object add1, object add2, object add3, object city, object state, object zip)
    {
        return MerchantProfileHelper.BindAddress(add1, add2, add3, city, state, zip);
    }

    protected string FormatCurrency(object abc)
    {
        return MerchantProfileHelper.FormatCurrency(abc);
    }

    protected string FormatCurrency(object abc, int count)
    {
        return MerchantProfileHelper.FormatCurrency(abc);
    }

    protected string FormatDate(object dt)
    {
        return MerchantProfileHelper.FormatDate(dt);
    }

    protected string FormatPhone(object phone)
    {
        return AS.Common.Formater.FormatData.FormatPhoneNumber(phone.ToString());
    }

    protected string FormatSIC(object sic, object sicDesc)
    {
        return MerchantProfileHelper.FormatSIC(sic, sicDesc);
    }

    protected string ButtonExcelHierarchyInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_HIERARCHY_FDR_NORTH_INFO)));

        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", GetLocalResourceObject("Literal28.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientName]", GetLocalResourceObject("Literal29.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientLogin]", GetLocalResourceObject("Literal32.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_SalesAgent]", GetLocalResourceObject("Literal33.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_MarkerBank]", GetLocalResourceObject("Literal36.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Entitlement_Sales_Agent]", GetLocalResourceObject("Literal37.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Business]", GetLocalResourceObject("Literal39.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Relationship_Mananger]", GetLocalResourceObject("Literal40.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Master_Sales_Agent]", GetLocalResourceObject("Literal43.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Agent]", GetLocalResourceObject("Literal46.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Branch_Number]", GetLocalResourceObject("Literal47.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Corp]", GetLocalResourceObject("Literal49.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Chain]", GetLocalResourceObject("Literal30.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Bank]", GetLocalResourceObject("Literal42.Text").ToString());

        strExcelTemplate.Replace("HI_CLIENT_NAME", "&nbsp;" + _datasource.Rows[0]["ClientName"].ToString());
        strExcelTemplate.Replace("HI_CLIENT_LOGIN", "&nbsp;" + _datasource.Rows[0]["ClientLogin"].ToString());
        strExcelTemplate.Replace("HI_SALES_AGENT", "&nbsp;" + _datasource.Rows[0]["SalesAgent"].ToString());
        strExcelTemplate.Replace("HI_MARKER_BANK", "&nbsp;" + _datasource.Rows[0]["MarkerBank"].ToString());
        strExcelTemplate.Replace("HI_ENTI_SALES_AGENT", "&nbsp;" + _datasource.Rows[0]["EntitlementSalesAgent"].ToString());
        strExcelTemplate.Replace("HI_BUSINESS", "&nbsp;" + _datasource.Rows[0]["Business"].ToString());
        strExcelTemplate.Replace("HI_RELATIONSHIP_MANAGER", "&nbsp;" + _datasource.Rows[0]["RelationshipManager"].ToString());
        strExcelTemplate.Replace("HI_BANK", "&nbsp;" + _datasource.Rows[0]["Bank"].ToString());
        strExcelTemplate.Replace("HI_MASTER_SALES_AGENT", "&nbsp;" + _datasource.Rows[0]["MasterSalesAgent"].ToString());
        strExcelTemplate.Replace("HI_AGENT", "&nbsp;" + _datasource.Rows[0]["Agent"].ToString());
        strExcelTemplate.Replace("HI_BRANCH_NUMBER", "&nbsp;" + _datasource.Rows[0]["HBranchNumber"].ToString());
        strExcelTemplate.Replace("HI_CORP", "&nbsp;" + _datasource.Rows[0]["Corp"].ToString());
        strExcelTemplate.Replace("HI_CHAIN", "&nbsp;" + _datasource.Rows[0]["Chain"].ToString());

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("Literal28.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelProgramDetails_Click()
    {
        DataTable _datasource = _ProgramCodeDetails;

        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_PROGRAM_DETAILS)));

        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ProgramDetails]", GetLocalResourceObject("Literal68.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ProgramCode]", GetLocalResourceObject("Literal31.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ProgramName]", GetLocalResourceObject("Literal34.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_InsertTimeStamp]", GetLocalResourceObject("Literal35.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_UpdateTimeStamp]", GetLocalResourceObject("Literal38.Text").ToString());

        string contentTemplate = "<tr class='borderTop'>"
                                + "<td>{0}</td>"
                                + "<td>{1}</td>"
                                + "<td>{2}</td>"
                                + "<td>{3}</td>"
                                + "</tr>";
        string emptyData = "<tr class='borderTop'>"
                               + "<td colspan='5'>{0}</td>"
                               + "</tr>";
        string result = string.Empty;
        if (_datasource != null && _datasource.Rows.Count > 0)
        {
            for (int i = 0; i < _datasource.Rows.Count; i++)
            {
                result += string.Format(contentTemplate, "&nbsp;" + _datasource.Rows[i]["ProgramCode"].ToString(), "&nbsp;" + _datasource.Rows[i]["ProgramName"].ToString(),
                    "&nbsp;" + FormatDate(_datasource.Rows[i]["InsertTimeStamp"].ToString()), "&nbsp;" + FormatDate(_datasource.Rows[i]["UpdateTimeStamp"].ToString()));
            }
        }
        else
        {
            result += string.Format(emptyData, "&nbsp;" + GetLocalResourceObject("Literal70.Text").ToString());
        }
        strExcelTemplate.Replace("[Content_Template]", result);

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("Literal68.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelFundingCategory_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_FUNDING_CATEGORY)));

        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_FundingCategory]", GetLocalResourceObject("Literal69.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_DDAs]", GetLocalResourceObject("Literal44.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_TransactionRollup]", GetLocalResourceObject("Literal45.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_DivertFunding]", GetLocalResourceObject("Literal48.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Deposits]", GetLocalResourceObject("Literal41.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_NonBankAdjustment]", GetLocalResourceObject("Literal50.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_DepositAdjustment]", GetLocalResourceObject("Literal51.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Chargebacks]", GetLocalResourceObject("Literal52.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Chargeback_Reversal]", GetLocalResourceObject("Literal53.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Interchange_Asmt]", GetLocalResourceObject("Literal54.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_DiscServ]", GetLocalResourceObject("Literal55.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Fees]", GetLocalResourceObject("Literal56.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_FinancialAdjustment]", GetLocalResourceObject("Literal57.Text").ToString());

        strExcelTemplate.Replace("FC_Deposits", "&nbsp;" + _datasource.Rows[0]["Deposits"].ToString());
        strExcelTemplate.Replace("FC_TransRUDeposits", "&nbsp;" + _datasource.Rows[0]["TransRUDeposits"].ToString());
        strExcelTemplate.Replace("FC_DivertDeposits", "&nbsp;" + _datasource.Rows[0]["DivertDeposits"].ToString());
        strExcelTemplate.Replace("FC_NonBankAdjustment", "&nbsp;" + _datasource.Rows[0]["NonBankAdjustment"].ToString());
        strExcelTemplate.Replace("FC_TransRUNonBankAdjustment", "&nbsp;" + _datasource.Rows[0]["TransRUNonBankAdjustment"].ToString());
        strExcelTemplate.Replace("FC_DivertNonBankAdjustment", "&nbsp;" + _datasource.Rows[0]["DivertNonBankAdjustment"].ToString());
        strExcelTemplate.Replace("FC_DepositAdjustment", "&nbsp;" + _datasource.Rows[0]["DepositAdjustment"].ToString());
        strExcelTemplate.Replace("FC_TransRUDepositAdjustment", "&nbsp;" + _datasource.Rows[0]["TransRUDepositAdjustment"].ToString());
        strExcelTemplate.Replace("FC_DivertDepositAdjustment", "&nbsp;" + _datasource.Rows[0]["DivertDepositAdjustment"].ToString());
        strExcelTemplate.Replace("FC_Chargebacks", "&nbsp;" + _datasource.Rows[0]["Chargebacks"].ToString());
        strExcelTemplate.Replace("FC_TransRUChargebacks", "&nbsp;" + _datasource.Rows[0]["TransRUChargebacks"].ToString());
        strExcelTemplate.Replace("FC_DivertChargebacks", "&nbsp;" + _datasource.Rows[0]["DivertChargebacks"].ToString());
        strExcelTemplate.Replace("FC_Chargeback_Reversal", "&nbsp;" + _datasource.Rows[0]["Chargeback_Reversal"].ToString());
        strExcelTemplate.Replace("FC_TransRUChargeback_Reversal", "&nbsp;" + _datasource.Rows[0]["TransRUChargeback_Reversal"].ToString());
        strExcelTemplate.Replace("FC_DivertChargeback_Reversal", "&nbsp;" + _datasource.Rows[0]["DivertChargeback_Reversal"].ToString());
        strExcelTemplate.Replace("FC_Interchange_Asmt", "&nbsp;" + _datasource.Rows[0]["Interchange_Asmt"].ToString());
        strExcelTemplate.Replace("FC_TransRUInterchange_Asmt", "&nbsp;" + _datasource.Rows[0]["TransRUInterchange_Asmt"].ToString());
        strExcelTemplate.Replace("FC_DivertInterchange_Asmt", "&nbsp;" + _datasource.Rows[0]["DivertInterchange_Asmt"].ToString());
        strExcelTemplate.Replace("FC_DiscServ", "&nbsp;" + _datasource.Rows[0]["DiscServ"].ToString());
        strExcelTemplate.Replace("FC_TransRUDiscServ", "&nbsp;" + _datasource.Rows[0]["TransRUDiscServ"].ToString());
        strExcelTemplate.Replace("FC_DivertDiscServ", "&nbsp;" + _datasource.Rows[0]["DivertDiscServ"].ToString());
        strExcelTemplate.Replace("FC_Interchange_Fees", "&nbsp;" + _datasource.Rows[0]["Fees"].ToString());
        strExcelTemplate.Replace("FC_TransRUFees", "&nbsp;" + _datasource.Rows[0]["TransRUFees"].ToString());
        strExcelTemplate.Replace("FC_DivertFees", "&nbsp;" + _datasource.Rows[0]["DivertFees"].ToString());
        strExcelTemplate.Replace("FC_FinancialAdjustment", "&nbsp;" + _datasource.Rows[0]["FinancialAdjustment"].ToString());
        strExcelTemplate.Replace("FC_TransRUFinancialAdjustment", "&nbsp;" + _datasource.Rows[0]["TransRUFinancialAdjustment"].ToString());
        strExcelTemplate.Replace("FC_DivertFinancialAdjustment", "&nbsp;" + _datasource.Rows[0]["DivertFinancialAdjustment"].ToString());

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("Literal69.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_MERCHANT_INFO)));

        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantInformation]", GetLocalResourceObject("Literal1.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_InternalMerchantNumber]", GetLocalResourceObject("Literal2.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_ExternalMerchantNumber]", GetLocalResourceObject("Literal5.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_ExternalAgentInd]", GetLocalResourceObject("Literal8.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_ChannelName]", GetLocalResourceObject("Literal11.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_DBAName]", GetLocalResourceObject("Literal14.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Attention]", GetLocalResourceObject("Literal17.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Url]", GetLocalResourceObject("Literal20.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_PaperlessMerchantIndicator]", GetLocalResourceObject("Literal23.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Address]", GetLocalResourceObject("Literal3.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Phone]", GetLocalResourceObject("Literal4.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_City]", GetLocalResourceObject("Literal6.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Seasonal]", GetLocalResourceObject("Literal26.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Zip2]", GetLocalResourceObject("Literal15.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Country]", GetLocalResourceObject("Literal18.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Status]", GetLocalResourceObject("Literal21.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_StoreNumber]", GetLocalResourceObject("Literal24.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_State]", GetLocalResourceObject("Literal9.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Zip1]", GetLocalResourceObject("Literal12.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_FaxInd]", GetLocalResourceObject("Literal7.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_FaxNumber]", GetLocalResourceObject("Literal10.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_PrimaryEmail]", GetLocalResourceObject("Literal13.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_DescriptorName]", GetLocalResourceObject("Literal16.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_DescriptorPhone]", GetLocalResourceObject("Literal19.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_DescriptorCity]", GetLocalResourceObject("Literal22.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_DescriptorState]", GetLocalResourceObject("Literal25.Text").ToString());

        strExcelTemplate.Replace("HI_INTERNAL_MERCHANT_NUMBER", "&nbsp;" + _datasource.Rows[0]["InternalMerchantNumber"].ToString());
        strExcelTemplate.Replace("HI_ADDRESS", "&nbsp;" + _datasource.Rows[0]["AffiliateAddress"].ToString());
        strExcelTemplate.Replace("HI_PHONE", "&nbsp;" + FormatPhone(_datasource.Rows[0]["MerchantPhone"].ToString()));
        strExcelTemplate.Replace("HI_EXTERNAL_MERCHANT_NUMBER", "&nbsp;" + _datasource.Rows[0]["ExternalMerchantNumber"].ToString());
        strExcelTemplate.Replace("HI_CITY", "&nbsp;" + _datasource.Rows[0]["AffiliateCity"].ToString());
        strExcelTemplate.Replace("HI_FAXIND", "&nbsp;" + _datasource.Rows[0]["FaxType"].ToString());
        strExcelTemplate.Replace("HI_EXTERNAL_AGENT_IND", "&nbsp;" + _datasource.Rows[0]["ExternalAgentInd"].ToString());
        strExcelTemplate.Replace("HI_STATE", "&nbsp;" + _datasource.Rows[0]["AffiliateState"].ToString());
        strExcelTemplate.Replace("HI_FAX_NUMBE", "&nbsp;" + FormatPhone(_datasource.Rows[0]["FaxNumber"].ToString()));
        strExcelTemplate.Replace("HI_CHANNEL_NAME", "&nbsp;" + _datasource.Rows[0]["ChannelName"].ToString());
        strExcelTemplate.Replace("HI_ZIP1", "&nbsp;" + _datasource.Rows[0]["Zip1"].ToString());
        strExcelTemplate.Replace("HI_PRIMARY_EMAIL", "&nbsp;" + _datasource.Rows[0]["PrimaryEmail"].ToString());
        strExcelTemplate.Replace("HI_DBA_NAME", "&nbsp;" + _datasource.Rows[0]["DBAName"].ToString());
        strExcelTemplate.Replace("HI_ZIP2", "&nbsp;" + _datasource.Rows[0]["Zip2"].ToString());
        strExcelTemplate.Replace("HI_DESCRIPTOR_NAME", "&nbsp;" + _datasource.Rows[0]["DescriptorName"].ToString());
        strExcelTemplate.Replace("HI_ATTENTION", "&nbsp;" + _datasource.Rows[0]["Attention"].ToString());
        strExcelTemplate.Replace("HI_COUNTRY", "&nbsp;" + _datasource.Rows[0]["Country"].ToString());
        strExcelTemplate.Replace("HI_DESCRIPTOR_PHONE", "&nbsp;" + FormatPhone(_datasource.Rows[0]["DescriptorPhone"].ToString()));
        strExcelTemplate.Replace("HI_URL", "&nbsp;" + _datasource.Rows[0]["URL"].ToString());
        strExcelTemplate.Replace("HI_STATUS", "&nbsp;" + _datasource.Rows[0]["Status"].ToString());
        strExcelTemplate.Replace("HI_DESCRIPTOR_CITY", "&nbsp;" + _datasource.Rows[0]["DescriptorCity"].ToString());
        strExcelTemplate.Replace("HI_PMI", "&nbsp;" + _datasource.Rows[0]["PaperlessMerchantInd"].ToString());
        strExcelTemplate.Replace("HI_STORE_NUMBER", "&nbsp;" + _datasource.Rows[0]["StoreNumber"].ToString());
        strExcelTemplate.Replace("HI_DESCRIPTOR_STATE", "&nbsp;" + _datasource.Rows[0]["DescriptorState"].ToString());
        strExcelTemplate.Replace("HI_SEASONAL", "&nbsp;" + _datasource.Rows[0]["Seasonal"].ToString());

        MerchantProfileHelper.FormatExportUserInfo(strExcelTemplate, _datasource);
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("Literal1.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected bool SetVisible(object obj)
    {
        return MerchantProfileHelper.SetVisibleSiteAccessLink(obj, (SecurePage)Page);
    }

    protected string CheckPermisson(object obj, string permissionCode)
    {
        return MerchantProfileHelper.CheckPermisson(obj, permissionCode, Page);
    }

    protected bool CheckHierarchy(string hierarchy)
    {
        return MerchantProfileHelper.CheckHierarchy(hierarchy);
    }

    #endregion

    #region Private Methods

    private string GetMerchantNrParameter()
    {
        return ReportPage.ReportFilter.CurrentValue.Value;
    }

    #endregion Private Methods

    #endregion Methods

    protected void GetDataSource(AS.Controls.KeyValueTable sender)
    {
        DataTable dt = _MifTable;
        sender.DataSource = dt;
    }

}