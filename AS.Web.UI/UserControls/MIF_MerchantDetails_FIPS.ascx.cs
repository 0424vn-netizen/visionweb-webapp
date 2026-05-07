using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.Formater;

public partial class UserControls_MIF_MerchantDetails_FIPS : ExportMultiSections
{
    #region Enums

    protected enum DataBindAction
    {
        BindMerchantDetails,
        UpdateMerchantStatus
    }

    #endregion Enums

    #region Constants
    // Export file
    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation_CLEARENT.htm";
    private const string FILE_NAME_BUSINESS_INFO = "~/App_Data/tpl_BusinessInformation_CLEARENT.htm";
    private const string FILE_NAME_CONTRACTUAL_INFO = "~/App_Data/tpl_ContractualInformation_FIPS.htm";
    private const string FILE_NAME_HIERARCHY_INFO = "~/App_Data/tpl_HierarchyInformation_FIPS.htm";
    private const string FILE_NAME_ACCOUNT_INFO = "~/App_Data/tpl_AccountInformation_CLEARENT.htm";
    private const string FILE_NAME_BANK_INFO = "~/App_Data/tpl_BankInformation_ORION.htm";
    // SPA
    private const string SPA_GET_MERCHANT_PROFILE = "spa_cs_GetMerchantProfile_Detail_FIPS";
    //TK 41447
    private const string SPA_GET_MERCHANT_PROFILE_BANK_INFORMATION = "spa_cs_GetReserveAccount";

    #endregion Constants

    #region Fields

    private bool _isCaseManagement = false;
    private string[] noprefix = { "mr ", "ms ", "sir ", "jr ", "mr.", "ms.", "sir.", "jr." };
    private bool _isShowRelationshipManager = GeneralFuncsLib.IsShowRelationshipManager();
    private DataTable _merchantInfor;


    #endregion Fields

    #region Properties

    public bool IsCaseManagement
    {
        get
        {
            return _isCaseManagement;
        }
        set
        {
            _isCaseManagement = value;
        }
    }

    public DataTable MifTable
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

    //TK41447
    public DataTable BankInformationTable
    {
        get
        {
            if (ViewState["BankInfomation"] != null)
                return (DataTable)(ViewState["BankInfomation"]);
            else
                return null;
        }
        set
        {
            ViewState["BankInfomation"] = value;
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

    protected string MifEmail
    {
        get
        {
            return (string)ViewState["MifEmail"];
        }
        set
        {
            ViewState["MifEmail"] = value;
        }
    }

    protected string TempStr
    {
        get
        {
            return (string)ViewState["TempStr"];
        }
        set
        {
            ViewState["TempStr"] = value;
        }
    }

    protected string MerchantNumber
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

    private string RecipientEmail
    {
        get
        {
            return (string)ViewState["RecipientEmail"];
        }
        set
        {
            ViewState["RecipientEmail"] = value;
        }
    }

    private string LastBatchDate
    {
        get
        {
            return (string)ViewState["LastBatchDate"];
        }
        set
        {
            ViewState["LastBatchDate"] = value;
        }
    }

    private bool CustomReport
    {
        get
        {
            return (bool)ViewState["CustomReport"];
        }
        set
        {
            ViewState["CustomReport"] = value;
        }
    }

    public string HierachyMode
    {
        get;
        set;
    }

    protected string RelationShipManager
    {
        get;
        set;
    }

    #endregion Properties

    #region Methods

    protected void SetMerchantStatus(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.UpdateMerchantStatus);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void uxSiteAccess_click(object sender, EventArgs e)
    {
        string userID = string.Empty;
        if (MifTable.IsNotNullData() && MifTable.Columns.Contains("UserID"))
        {
            userID = MifTable.Rows[0]["UserID"].ToString();
        }
        MerchantProfileHelper.SiteAccess(this.Page, userID);
    }

    public string BindValueEmpDash(string colName)
    {
        if (_merchantInfor != null && _merchantInfor.Rows.Count > 0)
        {
            return (_merchantInfor.Rows[0][colName] != DBNull.Value
                && !string.IsNullOrEmpty(_merchantInfor.Rows[0][colName].ToString())
                && !_merchantInfor.Rows[0][colName].ToString().Equals("n/a", StringComparison.OrdinalIgnoreCase))
                ? _merchantInfor.Rows[0][colName].ToString() : WebSiteConstants.HTML_EM_DASH;
        }
        else
        {
            return WebSiteConstants.HTML_EM_DASH;
        }

    }

    protected void lnkLastBatch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(MerchantNumber))
        {
            if (Page.SecureQueryString["MerchantNumber"] != null)
            {
                Response.Redirect(GeneralFuncsLib.BuildBatchHistoryUrl(
                    (SecurePage)Page, LastBatchDate, Page.SecureQueryString["MerchantNumber"]));
            }
        }
        else
        {
            Response.Redirect(GeneralFuncsLib.BuildBatchHistoryUrl(
                   (SecurePage)Page, LastBatchDate, MerchantNumber));
        }
    }

    protected string SetURLForSiteAccessButton()
    {
        return MerchantProfileHelper.BuildURLForSiteAccessInMIF(
           (SecurePage)Page, MerchantNumber, OptedIn, MifEmail);
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
                parameters.Add(new FilterParameter("@MerchantNumber", ReportPage.ReportFilter.CurrentValue.Value, DbType.AnsiString));
                parameters.Add(new FilterParameter("@FlagPermission", SessionManager.CurrentUserViewMode, DbType.Int32));
                parameters.AddLanguageID();
                parameters.AddDecryptDataParams("RoutingNumber");
                DataTable dtMerch = WebServices.CsReportServices.GetReports(SPA_GET_MERCHANT_PROFILE, parameters);

                if (dtMerch.Rows.Count > 0)
                {
                    MifTable = dtMerch;
                    OptedIn = dtMerch.Rows[0]["SiteAccess"].ToString().Equals(Resources.Template.OptedIn, StringComparison.OrdinalIgnoreCase)
                        ? MerchantProfileHelper.OPTED_IN_VALUE : MerchantProfileHelper.OPTED_OUT_VALUE;
                    MifEmail = dtMerch.Rows[0]["Email"].ToString();
                    phdMerchantDetail.Visible = true;

                    if (dtMerch.Columns.Contains("RelationShipManager"))
                    {
                        RelationShipManager = dtMerch.Rows[0]["RelationShipManager"].ToString();
                    }

                    string merchantWebsite = dtMerch.Rows[0]["MerchantWebsite"].ToString();

                    if (string.IsNullOrEmpty(merchantWebsite))
                    {
                        uxLinkWebsite.Visible = false;
                        ltrLinkWebsite.Visible = true;
                        ltrLinkWebsite.Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
                    }
                    else
                    {
                        uxLinkWebsite.Visible = true;
                        ltrLinkWebsite.Visible = false;
                        merchantWebsite = merchantWebsite.ToLower().Contains("http") ? merchantWebsite : string.Format("//{0}", merchantWebsite);
                        uxLinkWebsite.Attributes.Add("href", merchantWebsite);
                    }

                    // ShowRelationshipManager
                    _merchantInfor = dtMerch;

                    if (BindValueNoEMDash("LastBactchActivity").IsNullOrEmpty())
                    {
                        lnkLastBatch.Visible = false;
                        ltrLastBatch.Visible = true;
                        ltrLastBatch.Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
                    }
                    else
                    {
                        lnkLastBatch.Visible = true;
                        ltrLastBatch.Visible = false;
                        lnkLastBatch.Text = FormatDate(BindValueNoEMDash("LastBactchActivity"));
                    }

                    // END ShowRelationshipManager

                    if (MerchantNumber.IsNullOrEmpty()
                        || (ReportPage.ReportFilter != null
                            && !MerchantNumber.Equals(ReportPage.ReportFilter.CurrentValue.Value)))
                    {
                        //CustomReport = (bool)dtMerch.Rows[0]["CustomReport"];
                        //RecipientEmail = dtMerch.Rows[0]["RecipientEmail"].ToString();
                        DateTime tempDate = new DateTime(1900, 1, 1);
                        DateTime.TryParse(FormatDate(dtMerch.Rows[0]["LastBactchActivity"]), out tempDate);
                        LastBatchDate = tempDate.Year == 1900 ? "0" : tempDate.Ticks.ToString();
                        MerchantNumber = ReportPage.ReportFilter.CurrentValue.Value;
                    }
                    this.rptMerchantInfo.DataSource = dtMerch;
                    this.rptMerchantInfo.DataBind();
                    foreach (RepeaterItem item in rptMerchantInfo.Items)
                    {
                        MerchantProfileHelper.BuildMIFDetailHierarchyLink(dtMerch, item, "uxLinkTenant", "EntityNumber1", HierarchyMode.FIPS_TENANT);
                        MerchantProfileHelper.BuildMIFDetailHierarchyLink(dtMerch, item, "uxLinkSubMerchant", "EntityNumber2", HierarchyMode.FIPS_SUBMERCHANT);
                        MerchantProfileHelper.BuildMIFDetailHierarchyLink(dtMerch, item, "uxLinkChain", "Chain", HierarchyMode.CHAIN);
                    }
                }
                else
                {
                    phdMerchantDetail.Visible = false;
                }
                break;
        }
    }

    /// <summary>
    /// TK 41447
    /// </summary>
    /// <returns></returns>
    private DataTable GetBankInformation()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@MerchantNumber", ReportPage.ReportFilter.CurrentValue.Value, DbType.AnsiString));

        string strOrder = "";

        if (this.Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA) || this.Page.IsUserWithPermission("MS" + WebSiteConstants.SEC_PERMISSION_DDA))
        {
            if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
            {
                parameters.AddDecryptDataParams("RoutingNumber,DDANumber");
                strOrder = "RoutingNumber,DDANumber";
            }
            else
            {
                parameters.AddDecryptDataParams("DDANumber");
                strOrder = "PartialRoutingNumber,DDANumber";
            }

        }
        else
        {
            if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
            {
                parameters.AddDecryptDataParams("RoutingNumber");
                strOrder = "RoutingNumber,PartialDDANumber";
            }
            else
            {
                strOrder = "PartialRoutingNumber,PartialDDANumber";
            }
        }

        DataTable tb = WebServices.CsReportServices.GetReports(SPA_GET_MERCHANT_PROFILE_BANK_INFORMATION, parameters);

        // Distinct data (Routing&DDA) and Sort 
        DataView dv = tb.DefaultView;
        dv.Sort = strOrder;
        DataTable rt = dv.ToTable(true, "RoutingNumber", "DDANumber", "PartialRoutingNumber", "PartialDDANumber");

        return rt;
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

    protected string FormatDate(object date, bool? isExport = false)
    {
        string em_dash = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        date = MerchantProfileHelper.ProcessNullValue(date);
        return (date == null || date.ToString().IsNullOrEmpty())
            ? em_dash : Convert.ToDateTime(date).ToString(WebSiteConstants.DATE_FORMAT);
    }

    protected bool setButton(object MerchantStatusDesc)
    {
        if (MerchantStatusDesc.ToString() != string.Empty)
        {
            string status = MerchantStatusDesc.ToString().ToLower();
            return !status.Equals("closed");
        }
        return false;
    }

    protected string setOptInStatus(object OptIn, string MerchantStatus)
    {
        return MerchantProfileHelper.SetOptInStatus(OptIn, MerchantStatus);
    }

    //TK 41447
    public string GetRoutingNumber(string full, string partial)
    {
        return MerchantProfileHelper.GetRoutingNumberWithoutDecrypt(FormatDataToMDash(full), FormatDataToMDash(partial));
    }

    //TK 41447
    public string GetDDANumber(string full, string partial)
    {
        if (this.Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA) || this.Page.IsUserWithPermission("MS" + WebSiteConstants.SEC_PERMISSION_DDA))
        {
            return FormatDataToMDash(full);
        }
        else return FormatDataToMDash(partial);

    }

    public void Rebind()
    {
        this.DoMultiExportExcel();
        this.OnDataBindControls(DataBindAction.BindMerchantDetails, this);
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
        exportFucntions.Add(1, ButtonExcelBusinessInformation_Click);
        exportFucntions.Add(2, ButtonExcelContractualInformation_Click);

        if (IsRiskInformation)
        {
            var uxRiskInfo = rptMerchantInfo.Items[0].FindControl("uxRiskInfo") as UserControls_MIF_RiskInformationSection;
            if (uxRiskInfo != null)
                exportFucntions.Add(3, () => { return uxRiskInfo.ExcelRiskInformation(); });
            exportFucntions.Add(4, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(5, ButtonExcelAccountInformation_Click);
            exportFucntions.Add(6, ButtonExcelBankInformation_Click);
        }
        else
        {
            exportFucntions.Add(3, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(4, ButtonExcelAccountInformation_Click);
            exportFucntions.Add(5, ButtonExcelBankInformation_Click);
        }
        return exportFucntions;
    }

    #region Processing Method

    protected string BindAddress(object add1, object add2, object add3, object city, object state, object zip, bool? isExport = false)
    {
        string em_dash = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        string address = MerchantProfileHelper.BindAddress(add1, add2, add3, city, state, zip);
        return address.IsNullOrEmpty() ? em_dash : address;
    }

    public string BindValue(string colName)
    {
        if (_merchantInfor == null || _merchantInfor.Rows.Count <= 0)
            return WebSiteConstants.HTML_EM_DASH_ENCODE;
        DataRow dr = _merchantInfor.Rows[0];
        return dr[colName] == DBNull.Value || dr[colName].ToString().IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : dr[colName].ToString();
    }

    public string BindValueNoEMDash(string colName)
    {
        return (_merchantInfor == null || _merchantInfor.Rows.Count <= 0)
            ? string.Empty : _merchantInfor.Rows[0][colName].ToString();
    }

    protected string FormatCurrency(object abc, bool? isExport = false)
    {
        string em_dash = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        return abc == DBNull.Value ? em_dash : GeneralFuncsLib.FormatCurrency(abc);
    }

    protected string FormatPercent(object value, bool? isExport = false)
    {
        string em_dash = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        return value.IsNullOrEmpty() ? em_dash : FormatData.FormatPercent(value);
    }

    protected string FormatPhone(object phone, bool? isExport = false)
    {
        string em_dash = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        return phone.IsNullOrEmpty() ? em_dash : AS.Common.Formater.FormatData.FormatPhoneNumber(phone.ToString());
    }

    private string FormatDataToMDash(object value)
    {
        return value.IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH : value.ToString();
    }

    protected string FormatSIC(object sic, object sicDesc, bool? isExport = false)
    {
        string em_dash = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        string strSic = MerchantProfileHelper.FormatSIC(sic, sicDesc);
        return strSic.IsNullOrEmpty() ? em_dash : strSic;
    }


    protected void lnkGrid_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
    {
        if (string.IsNullOrEmpty(MerchantNumber))
        {
            if (Page.SecureQueryString["MerchantNumber"] != null)
            {
                Response.Redirect(GeneralFuncsLib.BuildBatchHistoryUrl(
                   (SecurePage)Page, LastBatchDate, Page.SecureQueryString["MerchantNumber"]));
            }
        }
        else
        {
            Response.Redirect(GeneralFuncsLib.BuildBatchHistoryUrl(
                  (SecurePage)Page, LastBatchDate, MerchantNumber));
        }
    }

    private void DownloadFile(string fileName, string fileType, string content)
    {
        Response.BufferOutput = true;
        switch (fileType)
        {
            case "xls":
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + VeraCodeSolution.RemoveCRLF(fileName) + ".xls\"");
                break;
        }
        Response.Write(content);
        Response.Flush();
        Response.End();
    }

    public void DoMultiExportExcel()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();
        exportNames.Add(0, GetLocalResourceObject("LiteralResource1.Text").ToString());
        exportNames.Add(1, GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_BusinessInformation").ToString());
        exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_CLEARENT_Text_ContractualInformation").ToString());
        if (IsRiskInformation)
        {
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_Text_RiskInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_HierarchyInformation").ToString());
            exportNames.Add(5, GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_AccountInformation").ToString());
            exportNames.Add(6, GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_BankInformation").ToString());
        }
        else
        {
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_HierarchyInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_AccountInformation").ToString());
            exportNames.Add(5, GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_BankInformation").ToString());
        }

        ExportSectionNames = exportNames;
    }

    protected string ButtonExcelBusinessInformation_Click()
    {
        DataTable dataSource = (DataTable)(MifTable);
        if (dataSource == null || dataSource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_BUSINESS_INFO)));
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessInformation]", Resources.Template.tpl_BusinessInformation_htm_BusinessInformation);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_CorporateName]", Resources.Template.tpl_BusinessInformation_htm_CorporateName);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_CorporateAddress]", Resources.Template.tpl_BusinessInformation_htm_CorporateAddress);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessType]", Resources.Template.tpl_BusinessInformation_SNET_htm_BusinessType);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ApprovalDate]", Resources.Template.tpl_BusinessInformation_ORION_htm_ApprovalDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ClosedDate]", Resources.Template.tpl_BusinessInformation_htm_ClosedDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Status]", Resources.Template.tpl_BusinessInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Seasonal]", Resources.Template.tpl_BusinessInformation_htm_Seasonal);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SiteAccess]", Resources.Template.tpl_BusinessInformation_htm_SiteAccess);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_TaxID]", Resources.Template.tpl_BusinessInformation_htm_TaxID);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SICMCC]", Resources.Template.tpl_BusinessInformation_htm_SICMCC);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_AverageSalesAmount]", Resources.Template.tpl_BusinessInformation_EMS_htm_AverageSalesAmount);


        strExcelTemplate.Replace("BI_CORPORATE_NAME", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["CorporateName"].ToString()));
        strExcelTemplate.Replace("BI_CORPORATE_ADDRESS", "&nbsp;" + BindAddress(dataSource.Rows[0]["CorporateAddress"],
            string.Empty, string.Empty,
            dataSource.Rows[0]["CorporateCity"], dataSource.Rows[0]["CorporateState"], dataSource.Rows[0]["CorporateZip"], true));

        strExcelTemplate.Replace("BI_BUSINESS_TYPE", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["BusinessType"]));
        strExcelTemplate.Replace("BI_APPROVAL_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["ApprovalDate"], true));
        strExcelTemplate.Replace("BI_CLOSE_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["ClosedDate"], true));
        strExcelTemplate.Replace("BI_STATUS", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["ActivityStatus"].ToString()));
        strExcelTemplate.Replace("BI_SEASONAL", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["Seasonal"].ToString()));
        if (HasMSProductEnvironment())
        {
            strExcelTemplate.Replace("BI_SITE_ACCESS", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["SiteAccess"]));
        }
        else
        {
            strExcelTemplate.Replace("BI_SITE_ACCESS", "&nbsp;" + GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_NotAvailable").ToString());
        }
        strExcelTemplate.Replace("BI_TAX_ID", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["PartialTaxID"].ToString()));
        strExcelTemplate.Replace("BI_SIC_MCC", "&nbsp;" + FormatSIC(dataSource.Rows[0]["SICCode"].ToString(), dataSource.Rows[0]["SICCodeDesc"].ToString(), true));

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_BusinessInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelContractualInformation_Click()
    {
        DataTable dataSource = (DataTable)(MifTable);
        if (dataSource == null || dataSource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_CONTRACTUAL_INFO)));
        strExcelTemplate.Replace("[tpl_ContractualInformation_htm_ContractualInformation]", Resources.Template.tpl_ContractualInformation_htm_ContractualInformation);
        strExcelTemplate.Replace("[tpl_ContractualInformation_htm_AnnualVolume]", Resources.Template.tpl_ContractualInformation_htm_AnnualVolume);
        strExcelTemplate.Replace("[tpl_ContractualInformation_htm_AverageTicket]", Resources.Template.tpl_ContractualInformation_htm_AverageTicket);
        strExcelTemplate.Replace("[tpl_ContractualInformation_htm_HighTicket]", Resources.Template.tpl_ContractualInformation_htm_HighTicket);
        strExcelTemplate.Replace("[tpl_ContractualInformation_htm_MOTO]", Resources.Template.tpl_ContractualInformation_htm_MOTO);
        strExcelTemplate.Replace("[tpl_ContractualInformation_htm_eCommerce]", Resources.Template.tpl_ContractualInformation_htm_eCommerce);
        strExcelTemplate.Replace("[tpl_ContractualInformation_htm_CardNotPresent]", Resources.Template.tpl_ContractualInformation_htm_CardNotPresent);
        strExcelTemplate.Replace("[tpl_ContractualInformation_htm_FICO]", Resources.Template.tpl_ContractualInformation_htm_FICO);

        strExcelTemplate.Replace("CI_ANNUAL_VOLUME", "&nbsp;" + FormatCurrency(dataSource.Rows[0]["AnnualVolume"], true));
        strExcelTemplate.Replace("CI_AVERAGE_TICKET", "&nbsp;" + FormatCurrency(dataSource.Rows[0]["Avg_Ticket_Amt"], true));
        strExcelTemplate.Replace("CI_HIGH_TICKET", "&nbsp;" + FormatCurrency(dataSource.Rows[0]["HighestTicket"], true));
        strExcelTemplate.Replace("CI_MOTO", "&nbsp;" + FormatPercent(dataSource.Rows[0]["MOTO"].ToString(), true));
        strExcelTemplate.Replace("CI_ECOMMERCE", "&nbsp;" + FormatPercent(dataSource.Rows[0]["eCommerce"].ToString(), true));
        strExcelTemplate.Replace("CI_CARD_NOT_PRESENT", "&nbsp;" + FormatPercent(dataSource.Rows[0]["CardNotPresent"], true));
        strExcelTemplate.Replace("CI_FICO", " &nbsp;" + FormatPercent(dataSource.Rows[0]["FICO"], true));

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_CLEARENT_Text_ContractualInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelHierarchyInformation_Click()
    {
        DataTable dataSource = (DataTable)(MifTable);
        if (dataSource == null || dataSource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_HIERARCHY_INFO)));
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", Resources.Template.tpl_HierarchyInformation_htm_HierarchyInformation);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Tenant]", Resources.Template.tpl_HierarchyInformation_htm_Tenant);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_SubMerchant]", Resources.Template.tpl_HierarchyInformation_htm_SubMerchant);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Chain]", Resources.Template.tpl_HierarchyInformation_htm_Chain);
        
        strExcelTemplate.Replace("HI_TENANT", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["EntityNumber1"].ToString()));
        strExcelTemplate.Replace("HI_SUBMERCHANT", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["EntityNumber2"].ToString()));
        strExcelTemplate.Replace("HI_CHAIN", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["CHAIN"].ToString()));

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_HierarchyInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelAccountInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_ACCOUNT_INFO)));
        strExcelTemplate.Replace("[tpl_AccountInformation_FULTON_htm_AccountInformation]", Resources.Template.tpl_AccountInformation_FULTON_htm_AccountInformation);
        strExcelTemplate.Replace("[tpl_AccountInformation_htm_Program]", Resources.Template.tpl_AccountInformation_htm_Program);
        strExcelTemplate.Replace("[tpl_AccountInformation_htm_AccountNumber]", Resources.Template.tpl_AccountInformation_htm_AccountNumber);

        strExcelTemplate.Replace("AI_AMEX_ACCOUNT_INFO", "&nbsp;" + GetLocalResourceObject("MIF_MerchantDetails_CLEARENT_Text_AmericanExress").ToString());
        strExcelTemplate.Replace("AI_DISCOVER_ACCOUNT_INFO", "&nbsp;" + GetLocalResourceObject("MIF_MerchantDetails_CLEARENT_Text_Discover").ToString());
        strExcelTemplate.Replace("AI_DINNER_CLUB_ACCOUNT_INFO", "&nbsp;" + GetLocalResourceObject("MIF_MerchantDetails_CLEARENT_Text_DinnerClub").ToString());

        strExcelTemplate.Replace("AI_AMEX_PROGRAM", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["AmericanExpressProgram"]));
        strExcelTemplate.Replace("AI_DISCOVER_PROGRAM", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["DiscoverProgram"]));
        strExcelTemplate.Replace("AI_DINNER_CLUB_PROGRAM", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["DinersClubProgram"]));


        strExcelTemplate.Replace("AI_AMEX_ACCOUNT_NUMBER", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["AmericanExpressAccountNumber"]));
        strExcelTemplate.Replace("AI_DISCOVER_ACCOUNT_NUMBER", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["DiscoverAccountNumber"]));
        strExcelTemplate.Replace("AI_DINNER_CLUB_ACCOUNT_NUMBER", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["DinersClubAccountNumber"]));

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_AccountInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelBankInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_BANK_INFO)));
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankInformation]", Resources.Template.tpl_BankInformation_htm_BankInformation);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankName]", Resources.Template.tpl_BankInformation_htm_BankName);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_RoutingSharp]", Resources.Template.tpl_BankInformation_htm_RoutingSharp);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DDASharp]", Resources.Template.tpl_BankInformation_htm_DDASharp);
        strExcelTemplate.Replace("BI_BANK_NAME", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["BankName"].ToString()));
        strExcelTemplate.Replace("BI_ROUTING_#", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["PartialRoutingNumber"].ToString()));
        strExcelTemplate.Replace("BI_DDA_#", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["PartialDDANumber"].ToString()));
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_BankInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate;

        strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_MERCHANT_INFO)));
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantInformation]", Resources.Template.tpl_MerchantInformation_htm_MerchantInformation);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantNumber]", Resources.Template.tpl_MerchantInformation_htm_MerchantNumber);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Contact]", Resources.Template.tpl_MerchantInformation_htm_Contact);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Status]", Resources.Template.tpl_MerchantInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantName]", Resources.Template.tpl_MerchantInformation_htm_MerchantName);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Phone]", Resources.Template.tpl_MerchantInformation_htm_Phone);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_LastBatchActivity]", Resources.Template.tpl_MerchantInformation_htm_LastBatchActivity);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Address]", Resources.Template.tpl_MerchantInformation_htm_Address);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Email]", Resources.Template.tpl_MerchantInformation_htm_Email);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_OwnerName]", Resources.Template.tpl_MerchantInformation_htm_OwnerName);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_OwnerSSN]", Resources.Template.tpl_MerchantInformation_htm_OwnerSSN);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Fax]", Resources.Template.tpl_MerchantInformation_htm_Fax);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Website]", Resources.Template.tpl_MerchantInformation_htm_Website);

        strExcelTemplate.Replace("HI_MERCHANT_NUMBER", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["MerchantNumber"].ToString()));
        strExcelTemplate.Replace("HI_CONTACT", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["Contact"].ToString()));
        strExcelTemplate.Replace("HI_STATUS", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["Status"].ToString()));
        strExcelTemplate.Replace("HI_MERCHANT_NAME", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["MerchantName"].ToString()));
        strExcelTemplate.Replace("HI_PHONE", "&nbsp;" + FormatPhone(_datasource.Rows[0]["Phone"], true));
        strExcelTemplate.Replace("HI_LAST_BATCH_ACTIVITY", "&nbsp;" + FormatDate(_datasource.Rows[0]["LastBactchActivity"], true));
        strExcelTemplate.Replace("HI_ADDRESS", "&nbsp;" + BindAddress(_datasource.Rows[0]["Address1"],
            _datasource.Rows[0]["Address2"], _datasource.Rows[0]["Address3"],
            _datasource.Rows[0]["City"], _datasource.Rows[0]["State"], _datasource.Rows[0]["Zip"], true));
        strExcelTemplate.Replace("HI_EMAIL", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["Email"].ToString()));
        strExcelTemplate.Replace("HI_OWNER_NAME", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["OwnerName"].ToString()));
        strExcelTemplate.Replace("HI_OWNER_SSN", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["PartialOwnerSSN"].ToString()));
        strExcelTemplate.Replace("HI_FAX", "&nbsp;" + FormatPhone(_datasource.Rows[0]["Fax"].ToString(), true));
        strExcelTemplate.Replace("HI_WEBSITE", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["MerchantWebsite"].ToString()));

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("LiteralResource1.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string SetStatusText(object obj)
    {
        return MerchantProfileHelper.SetStatusForSiteAccess(MerchantProfileHelper.TranslateStatusText(obj));
    }

    protected bool HasMSProductEnvironment()
    {
        return GeneralFuncsLib.HasMSProductEnvironment();
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

    protected bool CheckPermissionAddEditChain(object siteAccess)
    {
        return MerchantProfileHelper.CheckPermissionAddEditChain(siteAccess, (SecurePage)Page);
    }

    // TK 41447
    //private void BindBankRepeater(Repeater rpt)
    //{
    //    BankInformationTable = GetBankInformation();
    //    rpt.DataSource = BankInformationTable;
    //    rpt.DataBind();

    //    if (BankInformationTable == null || BankInformationTable.Rows.Count <= 0)
    //    {
    //        var control = rpt.Controls[rpt.Controls.Count - 1].Controls[0];
    //        control.FindControl("trEmpty").Visible = true;
    //    }
    //}

    // TK 41447

    protected void uxMerchantInfoItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        PlaceHolder plChain = ((PlaceHolder)e.Item.FindControl("uxChain"));
        PlaceHolder plChainNew = ((PlaceHolder)e.Item.FindControl("uxPlaceChainNew"));
        Literal ltChain = ((Literal)e.Item.FindControl("uxLHChain"));

        //// TK41447
        //Repeater rpt = ((Repeater)e.Item.FindControl("rptBank"));
        //BindBankRepeater(rpt);

        switch (e.Item.ItemType)
        {
            case ListItemType.Item:
            case ListItemType.AlternatingItem:
                {
                    DataRowView row = e.Item.DataItem as DataRowView;
                    if (row["Chain"].ToString().IsNullOrEmpty()
                        && Page.IsUserWithPermission("AddEditChain")
                        && MerchantProfileHelper.SiteAccessIsOptInOut(row["SiteAccess"]))
                    {
                        plChain.Visible = false;
                        plChainNew.Visible = true;
                        ltChain.Visible = false;
                    }
                    else
                    {
                        plChainNew.Visible = false;
                        if (CheckHierarchy("Chain"))
                        {
                            plChain.Visible = true;
                            ltChain.Visible = false;
                        }
                        else
                        {
                            plChain.Visible = false;
                            ltChain.Visible = true;
                        }
                    }
                }
                break;
        }
    }

    protected string FormatSIC(object sic, object sicDesc)
    {
        return MerchantProfileHelper.FormatSIC(sic, sicDesc);
    }

    protected string FormatStatusAccountInfo(object discoverRetained)
    {
        return discoverRetained != DBNull.Value && discoverRetained.ToSafeString().ToBoolean()
            ? GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_Retained").ToString() : GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_NonRetained").ToString();
    }

    protected string FormatEffectiveDate(object effectiveDate, object discoverRetained)
    {
        return effectiveDate == DBNull.Value || discoverRetained == DBNull.Value
            ? GeneralFuncsLib.NA_VALUE : FormatDate(effectiveDate);
    }

    private string FormatHierarchyCol(string hierarchyMode, object data)
    {
        return !CheckHierarchy(hierarchyMode) ? data.ToString() : string.Empty;
    }

    #endregion

    #region Private methods


    private string GetMerchantNrParameter()
    {
        return IsCaseManagement ? Page.SecureQueryString["MerchantNumber"]
            : ReportPage.ReportFilter.CurrentValue.Value;
    }

    #endregion Private methods

    #endregion Methods
}