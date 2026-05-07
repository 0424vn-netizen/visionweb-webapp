using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class UserControls_MIF_MerchantDetails_EMS : ExportMultiSections
{
    #region Enums

    protected enum DataBindAction
    {
        BindMerchantDetails,
        UpdateMerchantStatus
    }

    #endregion Enums

    #region Constants

    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string FILE_NAME_BUSINESS_INFO = "~/App_Data/tpl_BusinessInformation_EMS.htm";
    private const string FILE_NAME_BANK_INFO = "~/App_Data/tpl_BankInformation_TSYS.htm";
    private const string FILE_NAME_HIERARCHY_INFO = "~/App_Data/tpl_HierarchyInformation_EMS.htm";
    private const string FILE_NAME_OTHER_CARD_INFO = "~/App_Data/tpl_TerminalInformation_TSYS.htm";
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation.htm";
    private const string OPTED_IN = "Opted In";

    // SPA Name
    private const string SPA_GET_MERCHANT_PROFILE = "spa_cs_GetMerchantProfile_EMS";
    private const string SPA_GET_MERCHANT_CARD_TYPE = "spa_cs_GetMerchantCardTypes_EMS";

    #endregion Constants

    #region Fields

    private bool _isCaseManagement = false;
    private string[] noprefix = { "mr ", "ms ", "sir ", "jr ", "mr.", "ms.", "sir.", "jr." };
    private bool _isShowRelationshipManager = GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RELATIONSHIP_MANAGER").Equals("true") ? true : false;
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

    public string OtherCardType
    {
        get
        {
            return ViewState["OtherCardType"].ToString();
        }
        set
        {
            ViewState["OtherCardType"] = value;
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
        uxpnlSaveCancel.Visible = false;
        ShowHideRelationshipManager();
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
        if (MifTable.IsNotNullData() && MifTable.Columns.Contains("UserID"))
        {
            userID = MifTable.Rows[0]["UserID"].ToString();
        }
        MerchantProfileHelper.SiteAccess(this.Page, userID);
    }

    protected void uxbtnCancel_click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(RelationShipManager))
        {
            uxbtnAdd.Visible = true;
            uxbtnEdit.Visible = false;
        }
        else
        {
            uxbtnAdd.Visible = false;
            uxbtnEdit.Visible = true;
            uxlbRelationshipManager.Text = RelationShipManager;
            uxlbRelationshipManager.Visible = true;
        }
        uxpnlSaveCancel.Visible = false;

    }

    protected void uxbtnEdit_click(object sender, EventArgs e)
    {
        uxbtnAdd.Visible = false;
        uxbtnEdit.Visible = false;
        uxpnlSaveCancel.Visible = true;
        uxtxtRelationShipManager.Text = RelationShipManager;
        uxlbRelationshipManager.Visible = false;
    }

    protected void uxbtnAdd_click(object sender, EventArgs e)
    {
        uxbtnAdd.Visible = false;
        uxbtnEdit.Visible = false;
        uxpnlSaveCancel.Visible = true;
        uxtxtRelationShipManager.Text = string.Empty;
    }

    protected void uxbtnSave_click(object sender, EventArgs e)
    {

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new FilterParameter("@MerchantNumber",
            GetMerchantNrParameter(), DbType.AnsiString));
        parameters.Add(new FilterParameter("@RelationshipManager",
            uxtxtRelationShipManager.Text.Trim(), DbType.String));
        WebServices.CsReportServices.ExecuteNonQueryCommand(
            MerchantProfileHelper.SPA_UPDATE_RELATIONSHIP_MANAGER, parameters, out parameters);

        uxpnlSaveCancel.Visible = false;
        if (uxtxtRelationShipManager.Text.Trim() == string.Empty)
        {
            uxbtnAdd.Visible = true;
            uxbtnEdit.Visible = false;
            uxlbRelationshipManager.Text = string.Empty;
        }
        else
        {
            uxbtnEdit.Visible = true;
            uxbtnAdd.Visible = false;
            uxlbRelationshipManager.Text = uxtxtRelationShipManager.Text.Trim();
            uxlbRelationshipManager.Visible = true;
        }
    }

    public string BindValue(string colName)
    {
        if (_merchantInfor == null || _merchantInfor.Rows.Count <= 0)
            return string.Empty;
        DataRow dr = _merchantInfor.Rows[0];
        return dr[colName].ToString();
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
                parameters.Add(new FilterParameter("@MerchantNumber",
                    GetMerchantNrParameter(), DbType.AnsiString));
                parameters.Add(new FilterParameter("@FlagPermission",
                    SessionManager.CurrentUserViewMode, DbType.Int32));
                parameters.AddDecryptDataParams("RoutingNumber");
                parameters.AddLanguageID();
                DataTable dtMerch = WebServices.CsReportServices.GetReports(
                    SPA_GET_MERCHANT_PROFILE, parameters);

                if (dtMerch.Rows.Count > 0)
                {
                    MifTable = dtMerch;
                    OptedIn = dtMerch.Rows[0]["SiteAccess"].ToString().Equals(Resources.Template.OptedIn, StringComparison.OrdinalIgnoreCase) ? "1" : "0";
                    MifEmail = dtMerch.Rows[0]["Email"].ToString();
                    phdMerchantDetail.Visible = true;

                    if (dtMerch.Columns.Contains("RelationShipManager"))
                    {
                        RelationShipManager = dtMerch.Rows[0]["RelationShipManager"].ToString();
                    }

                    // ShowRelationshipManager
                    ShowHideRelationshipManager();

                    if (string.IsNullOrEmpty(MerchantNumber)
                        || (ReportPage.ReportFilter != null
                            && !MerchantNumber.Equals(ReportPage.ReportFilter.CurrentValue.Value)))
                    {
                        CustomReport = (bool)dtMerch.Rows[0]["CustomReport"];
                        RecipientEmail = dtMerch.Rows[0]["RecipientEmail"].ToString();
                        DateTime tempDate = new DateTime(1900, 1, 1);
                        DateTime.TryParse(FormatDate(dtMerch.Rows[0]["LastBactchActivity"]), out tempDate);
                        LastBatchDate = tempDate.Year == 1900 ? "0" : tempDate.Ticks.ToString();
                        if (!IsCaseManagement)
                        {
                            MerchantNumber = ReportPage.ReportFilter.CurrentValue.Value;
                        }
                    }
                    _merchantInfor = dtMerch;
                    lnkLastBatch.Text = VeraCodeSolution.DoVeraCode(FormatDate(BindValue("LastBactchActivity")));

                    rptMerchantInfo.DataSource = dtMerch;
                    rptMerchantInfo.DataBind();
                }
                else
                {
                    phdMerchantDetail.Visible = false;
                }
                break;
        }
    }

    protected DataTable GetMerchantCardType()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@MerchantNumber",
            GetMerchantNrParameter(), DbType.AnsiString));
        DataTable dt = WebServices.CsReportServices.GetReports(SPA_GET_MERCHANT_CARD_TYPE, parameters);
        return dt;
    }

    protected void rptMerchantInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.Item:
            case ListItemType.AlternatingItem:
                {
                    Literal ltrOtherCard = (Literal)e.Item.FindControl("uxOtherCardType");
                    ltrOtherCard.Text = string.Empty;
                    DataTable dt = GetMerchantCardType();
                    string otherCardType = string.Empty;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            otherCardType += dt.Rows[i]["CardType"].ToString() + ", ";
                        }
                        otherCardType = otherCardType.Trim().TrimEnd(',');
                        ltrOtherCard.Text = VeraCodeSolution.ValidateResponseData(otherCardType);
                        ViewState["OtherCardType"] = otherCardType;
                    }
                    else
                    {
                        ltrOtherCard.Text = string.Empty;
                        ViewState["OtherCardType"] = "Nothing to report.";//#32975: EMS Terminal Information error when exporting
                    }
                }
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

    protected bool setButton(object MerchantStatusDesc)
    {
        if (MerchantStatusDesc.ToString() != string.Empty)
        {
            string status = MerchantStatusDesc.ToString().ToLower();
            return !status.Equals("closed");
        }
        return false;
    }

    protected string setOptInStatus(object optIn, string merchantStatus)
    {
        return MerchantProfileHelper.SetOptInStatus(optIn, merchantStatus);
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
        
        if ( IsRiskInformation)
        {
            var uxRiskInfo = rptMerchantInfo.Items[0].FindControl("uxRiskInfo") as UserControls_MIF_RiskInformationSection;
            if (uxRiskInfo != null)
                exportFucntions.Add(2, () => { return uxRiskInfo.ExcelRiskInformation(); });
            exportFucntions.Add(3, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(4, ButtonExcelBankInformation_Click);
            exportFucntions.Add(5, ButtonExcelTerminalInformation_Click);
        }
        else
        {
            exportFucntions.Add(2, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(3, ButtonExcelBankInformation_Click);
            exportFucntions.Add(4, ButtonExcelTerminalInformation_Click);
        }
        return exportFucntions;
    }

    public void DoMultiExportExcel()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();
        exportNames.Add(0, GetLocalResourceObject("Resource1.Text").ToString());
        exportNames.Add(1, GetLocalResourceObject("MIF_MerchantDetails_EMSCS_Text_BusinessInformation").ToString());

        if (IsRiskInformation )
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_Text_RiskInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_EMSCS_Text_HierarchyInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_EMSCS_Text_BankInformation").ToString());
            exportNames.Add(5, GetLocalResourceObject("MIF_MerchantDetails_EMSCS_Text_TerminalInformation").ToString());
        }
        else
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_EMSCS_Text_HierarchyInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_EMSCS_Text_BankInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_EMSCS_Text_TerminalInformation").ToString());
        }
        ExportSectionNames = exportNames;
    }

    #region Processing Method

    protected string BindAddress(object add1, object add2, object add3, object city,
        object state, object zip)
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

    protected string ButtonExcelBusinessInformation_Click()
    {
        DataTable dataSource = (DataTable)(MifTable);
        if (dataSource == null || dataSource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_BUSINESS_INFO)));
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessInformation]", Resources.Template.tpl_BusinessInformation_htm_BusinessInformation);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Status]", Resources.Template.tpl_BusinessInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_Date]", Resources.Template.tpl_BusinessInformation_EMS_htm_Date);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_FederalTaxID]", Resources.Template.tpl_BusinessInformation_EMS_htm_FederalTaxID);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_SiteAcess]", Resources.Template.tpl_BusinessInformation_EMS_htm_SiteAcess);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_AverageSalesAmount]", Resources.Template.tpl_BusinessInformation_EMS_htm_AverageSalesAmount);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_CustomerType]", Resources.Template.tpl_BusinessInformation_EMS_htm_CustomerType);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_LeaseNumber]", Resources.Template.tpl_BusinessInformation_EMS_htm_LeaseNumber);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_Lessor]", Resources.Template.tpl_BusinessInformation_EMS_htm_Lessor);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_GiftCardCustomerNumber]", Resources.Template.tpl_BusinessInformation_EMS_htm_GiftCardCustomerNumber);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_GoldCDP]", Resources.Template.tpl_BusinessInformation_EMS_htm_GoldCDP);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_SocialSecuritySharp]", Resources.Template.tpl_BusinessInformation_EMS_htm_SocialSecuritySharp);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_RiskPlan]", Resources.Template.tpl_BusinessInformation_EMS_htm_RiskPlan);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_LeaseDate]", Resources.Template.tpl_BusinessInformation_EMS_htm_LeaseDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_RemainingPayment]", Resources.Template.tpl_BusinessInformation_EMS_htm_RemainingPayment);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_OnHold]", Resources.Template.tpl_BusinessInformation_EMS_htm_OnHold);
        strExcelTemplate.Replace("BI_Status", "&nbsp;" + dataSource.Rows[0]["Status"].ToString());
        strExcelTemplate.Replace("BI_OpenDate", "&nbsp;" + FormatDate(dataSource.Rows[0]["OpenDate"]));
        strExcelTemplate.Replace("BI_TaxID", "&nbsp;" + dataSource.Rows[0]["PartialTaxID"].ToString());
        strExcelTemplate.Replace("BI_SITEACCESS", "&nbsp;" + (HasMSProductEnvironment() ? dataSource.Rows[0]["SiteAccess"].ToString() : "Not Available"));
        strExcelTemplate.Replace("BI_AVGSalesAmount", "&nbsp;" + dataSource.Rows[0]["AVGSalesAmount"].ToString());
        strExcelTemplate.Replace("BI_CustomerType", "&nbsp;" + dataSource.Rows[0]["CustomerType"].ToString());
        strExcelTemplate.Replace("BI_LeaseNumber", "&nbsp;" + dataSource.Rows[0]["LeaseNumber"].ToString());
        strExcelTemplate.Replace("BI_Lessor", "&nbsp;" + dataSource.Rows[0]["Lessor"].ToString());
        strExcelTemplate.Replace("BI_GiftCardCustomerNumber", "&nbsp;" + dataSource.Rows[0]["GiftCardCustomerNumber"].ToString());
        strExcelTemplate.Replace("BI_Gold", "&nbsp;" + dataSource.Rows[0]["Gold"].ToString());
        strExcelTemplate.Replace("BI_SocialSecurityNumber", "&nbsp;" + dataSource.Rows[0]["SocialSecurityNumber"].ToString());
        strExcelTemplate.Replace("BI_RiskPlan", "&nbsp;" + dataSource.Rows[0]["RiskPlan"].ToString());
        strExcelTemplate.Replace("BI_LeaseDate", "&nbsp;" + FormatDate(dataSource.Rows[0]["LeaseDate"]));
        strExcelTemplate.Replace("BI_RemainingPayment", "&nbsp;" + dataSource.Rows[0]["RemainingPayment"].ToString());
        strExcelTemplate.Replace("BI_OnHold", "&nbsp;" + dataSource.Rows[0]["OnHold"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_EMSCS_Text_BusinessInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelHierarchyInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_HIERARCHY_INFO)));
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", Resources.Template.tpl_HierarchyInformation_htm_HierarchyInformation);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientName]", Resources.Template.tpl_HierarchyInformation_htm_ClientName);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientLogin]", Resources.Template.tpl_HierarchyInformation_htm_ClientLogin);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_EMS_htm_ISO]", Resources.Template.tpl_HierarchyInformation_EMS_htm_ISO);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_EMS_htm_SalesOffice]", Resources.Template.tpl_HierarchyInformation_EMS_htm_SalesOffice);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_EMS_htm_BankNumber]", Resources.Template.tpl_HierarchyInformation_EMS_htm_BankNumber);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_MerchantType]", Resources.Template.tpl_HierarchyInformation_htm_MerchantType);
        strExcelTemplate.Replace("HI_CLIENT_NAME", "&nbsp;" + _datasource.Rows[0]["ClientName"].ToString());
        strExcelTemplate.Replace("HI_CLIENT_LOGIN", "&nbsp;" + _datasource.Rows[0]["ClientLogin"].ToString());
        strExcelTemplate.Replace("HI_ISO", "&nbsp;" + _datasource.Rows[0]["ISO"].ToString());
        strExcelTemplate.Replace("HI_SalesOffice", "&nbsp;" + _datasource.Rows[0]["SalesOffice"].ToString());
        strExcelTemplate.Replace("HI_BankNumber", "&nbsp;" + _datasource.Rows[0]["BankNumber"].ToString());
        strExcelTemplate.Replace("HI_MERCHANT_TYPE", _datasource.Rows[0]["MerchantType"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_EMSCS_Text_HierarchyInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelBankInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_BANK_INFO)));
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankInformation]", Resources.Template.tpl_BankInformation_htm_BankInformation);
        strExcelTemplate.Replace("[tpl_BankInformation_TSYS_htm_TransitRoutingSharp]", Resources.Template.tpl_BankInformation_TSYS_htm_TransitRoutingSharp);
        strExcelTemplate.Replace("[tpl_BankInformation_TSYS_htm_AcctSharp]", Resources.Template.tpl_BankInformation_TSYS_htm_AcctSharp);
        strExcelTemplate.Replace("BI_RoutingNumber", "&nbsp;" + _datasource.Rows[0]["RoutingNumber"].ToString());
        strExcelTemplate.Replace("BI_AccountNumber", "&nbsp;" + _datasource.Rows[0]["AccountNumber"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_EMSCS_Text_BankInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelTerminalInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_OTHER_CARD_INFO)));
        strExcelTemplate.Replace("[tpl_TerminalInformation_TSYS_htm_TerminalInformation]", Resources.Template.tpl_TerminalInformation_TSYS_htm_TerminalInformation);
        strExcelTemplate.Replace("[tpl_TerminalInformation_TSYS_htm_Terminal]", Resources.Template.tpl_TerminalInformation_TSYS_htm_Terminal);
        strExcelTemplate.Replace("[tpl_TerminalInformation_TSYS_htm_OtherCardType]", Resources.Template.tpl_TerminalInformation_TSYS_htm_OtherCardType);
        strExcelTemplate.Replace("BI_Terminal", "&nbsp;" + _datasource.Rows[0]["Terminal"].ToString());
        strExcelTemplate.Replace("BI_OtherCardType", "&nbsp;" + OtherCardType);
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_EMSCS_Text_TerminalInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate = new StringBuilder(
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

        strExcelTemplate.Replace("HI_MERCHANT_NUMBER", "&nbsp;" + _datasource.Rows[0]["MerchantNumber"].ToString());
        strExcelTemplate.Replace("HI_CONTACT", "&nbsp;" + _datasource.Rows[0]["Contact"].ToString());
        strExcelTemplate.Replace("HI_STATUS", "&nbsp;" + _datasource.Rows[0]["Status"].ToString());
        strExcelTemplate.Replace("HI_MERCHANT_NAME", "&nbsp;" + _datasource.Rows[0]["MerchantName"].ToString());
        strExcelTemplate.Replace("HI_PHONE", "&nbsp;" + FormatPhone(_datasource.Rows[0]["Phone"]));
        strExcelTemplate.Replace("HI_LAST_BATCH_ACTIVITY", "&nbsp;" + FormatDate(_datasource.Rows[0]["LastBactchActivity"]));
        strExcelTemplate.Replace("HI_ADDRESS", "&nbsp;" + BindAddress(_datasource.Rows[0]["Address1"],
            _datasource.Rows[0]["Address2"], _datasource.Rows[0]["Address3"],
            _datasource.Rows[0]["City"], _datasource.Rows[0]["State"], _datasource.Rows[0]["Zip"]));
        strExcelTemplate.Replace("HI_EMAIL", "&nbsp;" + _datasource.Rows[0]["Email"].ToString());
        MerchantProfileHelper.FormatExportUserInfo(strExcelTemplate, _datasource);
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("Resource1.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string SetStatusText(object obj)
    {
        return MerchantProfileHelper.SetStatusForSiteAccess(obj);
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

    #endregion

    #region Private Methods

    private void ShowHideRelationshipManager()
    {
        uxlbRelationshipManager.Text = string.Empty;
        if (_isShowRelationshipManager)
        {
            uxpnlRMCtrls.Visible = true;
            if (string.IsNullOrEmpty(RelationShipManager))
            {
                if (GeneralFuncsLib.HasRelationshipManagerPermission((ReportPage)Page))
                {
                    uxbtnAdd.Visible = true;
                    uxbtnEdit.Visible = false;
                    uxlbRelationshipManager.Visible = false;
                    uxlbRelationshipManager.Text = string.Empty;
                }
                else
                {
                    uxbtnAdd.Visible = false;
                    uxbtnEdit.Visible = false;
                }
            }
            else
            {
                if (GeneralFuncsLib.HasRelationshipManagerPermission((ReportPage)Page))
                {
                    uxbtnAdd.Visible = false;
                    uxbtnEdit.Visible = true;
                }
                else
                {
                    uxbtnAdd.Visible = false;
                    uxbtnEdit.Visible = false;
                }
                uxlbRelationshipManager.Visible = true;
                uxlbRelationshipManager.Text = RelationShipManager;
            }
        }
        else
        {
            uxpnlRMCtrls.Visible = false;
        }
    }

    private string GetMerchantNrParameter()
    {
        return IsCaseManagement ? Page.SecureQueryString["MerchantNumber"]
            : ReportPage.ReportFilter.CurrentValue.Value;
    }

    #endregion Private Methods

    #endregion Methods
}
