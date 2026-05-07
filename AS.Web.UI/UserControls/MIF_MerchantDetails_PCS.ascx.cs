using AS.Common;
using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class UserControls_MIF_MerchantDetails_PCS : ExportMultiSections
{
    #region Fields

    private DataTable _merchantInfor;
    private bool _isCaseManagement = false;
    private bool isShowRelationshipManager =
        GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RELATIONSHIP_MANAGER").Equals("true") ? true : false;
    string[] noprefix = { "mr ", "ms ", "sir ", "jr ", "mr.", "ms.", "sir.", "jr." };
    private RelationshipManagerMode _currentMode = RelationshipManagerMode.View;

    #endregion Fields

    #region Constants

    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string PATH_BI_TEMPLATE = "~/App_Data/tpl_BusinessInformation_NORTH.htm";
    private const string PATH_HI_TEMPLATE = "~/App_Data/tpl_HierarchyInformation_NORTH.htm";
    private const string PATH_BANK_INFO_TEMPLATE = "~/App_Data/tpl_BankInformation_NORTH.htm";
    private const string PATH_OTHER_CARD_INFO_TEMPLATE = "~/App_Data/tpl_OtherCardInformation_NORTH.htm";
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation_ORI_Rep.htm";
    private const string SPA_GET_MERCHANT_PROFILE = "spa_cs_GetMerchantProfile_PCS";
    private const string SPA_UPDATE_RELATIONSHIP_MANAGER = "spa_UpdateRelationshipManager";

    #endregion Constants

    #region Enums

    protected enum DataBindAction
    {
        BindMerchantDetails,
        UpdateMerchantStatus
    }

    protected enum RelationshipManagerMode
    {
        ReadOnly,
        View,
        Edit,
        New,
    }

    #endregion Enums

    #region Properties

    public bool IsCaseManagement
    {
        get { return _isCaseManagement; }
        set { _isCaseManagement = value; }
    }

    private bool RelationshipManagerIsEmpty
    {
        get
        {
            return string.IsNullOrEmpty(RelationShipManager);
        }
    }

    public DataTable MifTable
    {
        get
        {
            if (ViewState["MerchantInfomation"] != null)
            {
                return (DataTable)(ViewState["MerchantInfomation"]);
            }
            return null;
        }
        set { ViewState["MerchantInfomation"] = value; }
    }

    protected string TempStr
    {
        get { return (string)ViewState["TempStr"]; }
        set { ViewState["TempStr"] = value; }
    }

    public string MerchantNumber
    {
        get { return (string)ViewState["MerchantNumber"]; }
        set { ViewState["MerchantNumber"] = value; }
    }

    private string RecipientEmail
    {
        get { return (string)ViewState["RecipientEmail"]; }
        set { ViewState["RecipientEmail"] = value; }
    }

    private string LastBatchDate
    {
        get { return (string)ViewState["LastBatchDate"]; }
        set { ViewState["LastBatchDate"] = value; }
    }

    private bool CustomReport
    {
        get { return (bool)ViewState["CustomReport"]; }
        set { ViewState["CustomReport"] = value; }
    }

    public string HierachyMode
    {
        get;
        set;
    }

    private string RelationShipManager
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
        uxlbRelationshipManager.Text = string.Empty;
        uxpnlSaveCancel.Visible = false;
        SetVisibleRelationshipManager();

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

    public bool CheckIsExsistPrefix(string text, string[] prefix)
    {
        bool r = false;
        foreach (string pf in prefix)
        {
            if (text.ToLower().StartsWith(pf.ToLower())
                || text.ToLower().EndsWith(pf.ToLower()))
            {
                r = true;
                break;
            }
        }
        return r;
    }

    protected void uxbtnCancel_click(object sender, EventArgs e)
    {
        UpdateRelationshipMngMode(RelationshipManagerMode.View);
    }

    protected void uxbtnEdit_click(object sender, EventArgs e)
    {
        UpdateRelationshipMngMode(RelationshipManagerMode.Edit);
    }

    protected void uxbtnAdd_click(object sender, EventArgs e)
    {
        UpdateRelationshipMngMode(RelationshipManagerMode.New);
    }

    protected void uxbtnSave_click(object sender, EventArgs e)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient",
            SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new FilterParameter("@MerchantNumber",
            GetMerchantNumber(), DbType.AnsiString));
        parameters.Add(new FilterParameter("@RelationshipManager",
            uxtxtRelationShipManager.Text.Trim(), DbType.String));

        WebServices.CsReportServices.ExecuteNonQueryCommand(
            SPA_UPDATE_RELATIONSHIP_MANAGER, parameters, out parameters);

        RelationShipManager = uxtxtRelationShipManager.Text.Trim();
        UpdateRelationshipMngMode(RelationshipManagerMode.View);
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
                DataTable dtMerch = LoadMerchantProfile();

                if (dtMerch.Rows.Count > 0)
                {
                    MifTable = dtMerch;

                    // PlaceHolder MerchantDetail
                    phdMerchantDetail.Visible = true;
                    if (dtMerch.Columns.Contains("RelationShipManager"))
                    {
                        RelationShipManager = dtMerch.Rows[0]["RelationShipManager"].ToString();
                    }
                    SetVisibleRelationshipManager();

                    // Set CustomReport, RecipientEmail, LastBactchActivity & ReportFilter
                    if (string.IsNullOrEmpty(MerchantNumber)
                        || (ReportPage.ReportFilter != null
                            && !MerchantNumber.Equals(ReportPage.ReportFilter.CurrentValue.Value)))
                    {
                        CustomReport = (bool)dtMerch.Rows[0]["CustomReport"];
                        RecipientEmail = dtMerch.Rows[0]["RecipientEmail"].ToString();
                        GetLastBatchDate(dtMerch.Rows[0]["LastBactchActivity"]);
                        if (!IsCaseManagement)
                        {
                            MerchantNumber = ReportPage.ReportFilter.CurrentValue.Value;
                        }
                    }

                    // Bind to Repeater Merchant Information
                    _merchantInfor = dtMerch;
                    rptMerchantInfo.DataSource = dtMerch;
                    rptMerchantInfo.DataBind();

                    lnkLastBatch.Text = FormatDate(BindValue("LastBactchActivity"));
                    FormatHyperlinkOfHierarchyInfo(dtMerch);
                }
                else
                {
                    phdMerchantDetail.Visible = false;
                }
                break;
        }
    }

    protected bool setButton(object MerchantStatusDesc)
    {
        if (MerchantStatusDesc.ToString() != string.Empty)
        {
            string status = MerchantStatusDesc.ToString().ToLower();
            return status.Equals("closed") ? false : true;
        }
        return false;
    }

    public string GetRoutingNumber(string full, string partial)
    {
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
        {
            return full;
        }
        else
        {
            return partial;
        }
    }

    public string GetTaxDDANumber(string full, string partial)
    {
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
        {
            return partial;
        }
        return WebServices.CsReportServices.DecryptText(full, SessionManager.CurrentUser.ASClient);
    }

    public string GetHierarchyNumber(string strInput, bool isChain)
    {
        if (strInput.Length >= 4)
        {
            if (isChain)
            {
                strInput = strInput.Remove(0, 3);
            }
            else
            {
                strInput = strInput.Remove(0, 4);
            }
        }
        return strInput;
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

        if (IsRiskInformation)
        {
            var uxRiskInfo = rptMerchantInfo.Items[0].FindControl("uxRiskInfo") as UserControls_MIF_RiskInformationSection;
            if (uxRiskInfo != null)
                exportFucntions.Add(2, () => { return uxRiskInfo.ExcelRiskInformation(); });
            exportFucntions.Add(3, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(4, ButtonExcelBankInformation_Click);
            exportFucntions.Add(5, ButtonExcelOtherCardInformation_Click);
            exportFucntions.Add(6, ButtonExcelMerchantMemos_Click);
        }
        else
        {
            exportFucntions.Add(2, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(3, ButtonExcelBankInformation_Click);
            exportFucntions.Add(4, ButtonExcelOtherCardInformation_Click);
            exportFucntions.Add(5, ButtonExcelMerchantMemos_Click);
        }
        return exportFucntions;
    }

    public void DoMultiExportExcel()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();
        exportNames.Add(0, GetLocalResourceObject("ltMerchantInfoResource1.Text").ToString());
        exportNames.Add(1, GetLocalResourceObject("Literal8Resource1.Text").ToString());

        if (IsRiskInformation)
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_Text_RiskInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("Literal23Resource1.Text").ToString());
            exportNames.Add(4, GetLocalResourceObject("Literal18Resource1.Text").ToString());
            exportNames.Add(5, GetLocalResourceObject("Literal32Resource1.Text").ToString());
            exportNames.Add(6, GetLocalResourceObject("MIF_MerchantDetails_Text_NORTH").ToString());
        }
        else
        {
            exportNames.Add(2, GetLocalResourceObject("Literal23Resource1.Text").ToString());
            exportNames.Add(3, GetLocalResourceObject("Literal18Resource1.Text").ToString());
            exportNames.Add(4, GetLocalResourceObject("Literal32Resource1.Text").ToString());
            exportNames.Add(5, GetLocalResourceObject("MIF_MerchantDetails_Text_NORTH").ToString());
        }
        ExportSectionNames = exportNames;
    }

    public string ButtonExcelMerchantMemos_Click()
    {
        string result = this.Page.GetType().InvokeMember("ButtonExcelMerchantMemos_Click", System.Reflection.BindingFlags.InvokeMethod, null, this.Page, new object[] { }).ToString();
        return result;
    }
    public bool CheckHierarchy(string hierarchy)
    {
        return GeneralFuncsLib.CheckHierarchy(hierarchy);
    }

    #region Processing Method

    public string BindValue(string colName)
    {
        if (_merchantInfor == null || _merchantInfor.Rows.Count <= 0)
        {
            return string.Empty;
        }
        DataRow dr = _merchantInfor.Rows[0];
        return dr[colName].ToString();
    }

    protected string BindAddress(object add1, object add2, object add3, object city, object state, object zip)
    {
        add1 = GeneralFuncsLib.NvlString(add1);
        add2 = GeneralFuncsLib.NvlString(add2);
        add3 = GeneralFuncsLib.NvlString(add3);
        city = GeneralFuncsLib.NvlString(city);
        state = GeneralFuncsLib.NvlString(state);
        zip = GeneralFuncsLib.NvlString(zip);
        string address1 = string.IsNullOrEmpty((string)add1) ? string.Empty : (add1 + "<br />");
        string address2 = string.IsNullOrEmpty((string)add2) ? string.Empty : (add2 + "<br />");
        if (!string.IsNullOrEmpty((string)add3))
        {
            return address1 + address2 + add3;
        }
        else
        {
            string cityAddress = string.IsNullOrEmpty((string)city) ? string.Empty : (city + ", ");
            return address1 + address2 + cityAddress + state + " " + zip;
        }
    }

    protected string FormatCurrency(object obj)
    {
        return obj == DBNull.Value
            ? string.Empty : GeneralFuncsLib.FormatCurrency(obj);
    }

    protected string FormatDate(object dt)
    {
        return GeneralFuncsLib.FormatDate(dt);
    }

    protected string FormatPhone(object phone)
    {
        return AS.Common.Formater.FormatData.FormatPhoneNumber(phone.ToString());
    }

    protected string FormatSIC(object sic, object sicDesc)
    {
        string sicDescription = GeneralFuncsLib.NvlString(sicDesc);
        if (sicDescription.IsNullOrEmpty())
        {
            return GeneralFuncsLib.NvlString(sic);
        }
        else
        {
            return string.Format("{0} - {1}",
                GeneralFuncsLib.NvlString(sic), sicDescription);
        }
    }

    protected void lnkLastBatch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(MerchantNumber))
        {
            if (Page.SecureQueryString["MerchantNumber"] != null)
            {
                Response.Redirect("~/BatchHistory.aspx?"
                    + Page.BuildSecureQueryString(
                        string.Format("date={0}&merchantnumber={1}",
                                      LastBatchDate,
                                      Page.SecureQueryString["MerchantNumber"])));
            }
        }
        else
        {
            Response.Redirect("~/BatchHistory.aspx?"
                + Page.BuildSecureQueryString(
                    string.Format("date={0}&merchantnumber={1}",
                                  LastBatchDate, MerchantNumber)));
        }
    }

    #region Export Methods

    protected string ButtonExcelBusinessInformation_Click()
    {
        DataTable dataSource = (DataTable)(MifTable);
        if (dataSource == null || dataSource.Rows.Count == 0)
        {
            return string.Empty;
        }
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(PATH_BI_TEMPLATE)));
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessInformation]", Resources.Template.tpl_BusinessInformation_htm_BusinessInformation);
        strExcelTemplate.Replace("[tpl_BusinessInformation_ORION_htm_ApprovalDate]", Resources.Template.tpl_BusinessInformation_ORION_htm_ApprovalDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ClosedDate]", Resources.Template.tpl_BusinessInformation_htm_ClosedDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Status]", Resources.Template.tpl_BusinessInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_TaxID]", Resources.Template.tpl_BusinessInformation_htm_TaxID);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SICMCC]", Resources.Template.tpl_BusinessInformation_htm_SICMCC);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_AverTicket]", Resources.Template.tpl_BusinessInformation_htm_AverTicket);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_AnnualVol]", Resources.Template.tpl_BusinessInformation_htm_AnnualVol);

        strExcelTemplate.Replace("BI_APPROVAL_DATE",
            "&nbsp;" + FormatDate(dataSource.Rows[0]["ApprovalDate"]));
        strExcelTemplate.Replace("BI_CLOSE_DATE",
            "&nbsp;" + FormatDate(dataSource.Rows[0]["ClosedDate"]));
        strExcelTemplate.Replace("BI_STATUS",
            "&nbsp;" + dataSource.Rows[0]["Status"]);
        strExcelTemplate.Replace("BI_TaxID",
            "&nbsp;" + dataSource.Rows[0]["PartialTaxID"]);
        strExcelTemplate.Replace("BI_SIC_MCC",
            "&nbsp;" + FormatSIC(dataSource.Rows[0]["SICMCC"],
                                 dataSource.Rows[0]["SICDescription"]));
        strExcelTemplate.Replace("BI_AVER_TICKET",
            "&nbsp;" + FormatCurrency(dataSource.Rows[0]["AverageTicket"]));
        strExcelTemplate.Replace("BI_ANNUAL_VOLUME",
            "&nbsp;" + FormatCurrency(dataSource.Rows[0]["AnnualVolume"]));
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("Literal8Resource1.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelHierarchyInformation_Click()
    {
        DataTable datasource = (DataTable)(MifTable);
        if (datasource == null || datasource.Rows.Count == 0)
        {
            return string.Empty;
        }

        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(PATH_HI_TEMPLATE)));
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", Resources.Template.tpl_HierarchyInformation_htm_HierarchyInformation);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientName]", Resources.Template.tpl_HierarchyInformation_htm_ClientName);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientLogin]", Resources.Template.tpl_HierarchyInformation_htm_ClientLogin);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_SalesAgent]", Resources.Template.tpl_HierarchyInformation_htm_SalesAgent);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Sys_Prin_Agent]", Resources.Template.tpl_HierarchyInformation_htm_Sys_Prin_Agent);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Headqrtr_Merchant]", Resources.Template.tpl_HierarchyInformation_htm_Headqrtr_Merchant);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_MerchantType]", Resources.Template.tpl_HierarchyInformation_htm_MerchantType);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ChainCode]", Resources.Template.tpl_HierarchyInformation_htm_ChainCode);
        strExcelTemplate.Replace("HI_CLIENT_NAME", "&nbsp;" + datasource.Rows[0]["ClientName"].ToString());
        strExcelTemplate.Replace("HI_CLIENT_LOGIN", "&nbsp;" + datasource.Rows[0]["ClientLogin"].ToString());
        strExcelTemplate.Replace("HI_SALES_AGENT", "&nbsp;" + datasource.Rows[0]["SalesAgent"].ToString());
        strExcelTemplate.Replace("HI_SYS_PRIN_AGENT", "&nbsp;" + datasource.Rows[0]["SysPrinAgent"].ToString());
        strExcelTemplate.Replace("HI_HEADQUARTER_MERCHANT", "&nbsp;" + datasource.Rows[0]["Headquarter"].ToString());
        strExcelTemplate.Replace("HI_MERCHANT_TYPE", datasource.Rows[0]["MerchantType"].ToString());
        strExcelTemplate.Replace("HI_CHAIN_CODE", "&nbsp;" + datasource.Rows[0]["ChainCode"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("Literal23Resource1.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelBankInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(PATH_BANK_INFO_TEMPLATE)));
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankInformation]", Resources.Template.tpl_BankInformation_htm_BankInformation);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankName]", Resources.Template.tpl_BankInformation_htm_BankName);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_RoutingSharp]", Resources.Template.tpl_BankInformation_htm_RoutingSharp);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DDASharp]", Resources.Template.tpl_BankInformation_htm_DDASharp);
        strExcelTemplate.Replace("BI_BANK_NAME", "&nbsp;" + _datasource.Rows[0]["BankName"].ToString());
        strExcelTemplate.Replace("BI_ROUTING_#", "&nbsp;" + _datasource.Rows[0]["PartialRoutingNumber"].ToString());
        strExcelTemplate.Replace("BI_DDA_#", "&nbsp;" + _datasource.Rows[0]["PartialDDANumber"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("Literal18Resource1.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelOtherCardInformation_Click()
    {
        DataTable datasource = (DataTable)(MifTable);
        if (datasource == null || datasource.Rows.Count == 0)
        {
            return string.Empty;
        }

        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(PATH_OTHER_CARD_INFO_TEMPLATE)));
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_OtherCardInformation]", Resources.Template.tpl_OtherCardInformation_htm_OtherCardInformation);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_NORTH_htm_AMEXMID]", Resources.Template.tpl_OtherCardInformation_FIS_htm_AMEXMID);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_NORTH_htm_DiscoverMID]", Resources.Template.tpl_OtherCardInformation_FIS_htm_DiscoverMID);
        strExcelTemplate.Replace("OI_AMEX_MID", "&nbsp;" + datasource.Rows[0]["AMEXMID"].ToString());
        strExcelTemplate.Replace("OI_Discover_MID", "&nbsp;" + datasource.Rows[0]["DiscoverMID"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("Literal32Resource1.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_MERCHANT_INFO)));
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantInformation]", Resources.Template.tpl_MerchantInformation_htm_MerchantInformation);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantNumber]", Resources.Template.tpl_MerchantInformation_htm_MerchantNumber);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Contact]", Resources.Template.tpl_MerchantInformation_htm_Contact);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Status]", Resources.Template.tpl_MerchantInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantName]", Resources.Template.tpl_MerchantInformation_htm_MerchantName);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Phone]", Resources.Template.tpl_MerchantInformation_htm_Phone);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_LastBatchActivity]", Resources.Template.tpl_MerchantInformation_htm_LastBatchActivity);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Address]", Resources.Template.tpl_MerchantInformation_htm_Address);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Email]", Resources.Template.tpl_MerchantInformation_htm_Email);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_RelationshipManager]", Resources.Template.tpl_MerchantInformation_htm_RelationshipManager);

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
        strExcelTemplate.Replace("HI_RELATIONSHIP_MANAGER", "&nbsp;" + RelationShipManager);

        MerchantProfileHelper.FormatExportUserInfo(strExcelTemplate, _datasource);
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("ltMerchantInfoResource1.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    #endregion Export Methods

    protected string CheckPermisson(object obj, string permissionCode)
    {
        TempStr = GeneralFuncsLib.NvlString(obj);
        if (!string.IsNullOrEmpty(TempStr))
        {
            if (SessionManager.CurrentUserViewMode > 0)
            {
                if (Page.IsUserWithPermission(permissionCode)
                    || Page.IsUserWithPermission("MS" + permissionCode))
                {
                    TempStr = WebServices.CsReportServices.DecryptText(
                        this.TempStr, SessionManager.CurrentUser.ASClient);
                }
            }
        }
        return TempStr;
    }

    #endregion Processing Method

    #region Private Methods

    private void DownloadFile(string fileName, string fileType, string content)
    {
        Response.BufferOutput = true;
        switch (fileType)
        {
            case "xls":
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition",
                    "attachment; filename=\"" + VeraCodeSolution.RemoveCRLF(fileName) + ".xls\"");
                break;
        }
        Response.Write(content);
        Response.Flush();
        Response.End();
    }

    private string GetMerchantNumber()
    {
        return IsCaseManagement ? Page.SecureQueryString["MerchantNumber"]
            : ReportPage.ReportFilter.CurrentValue.Value;
    }

    private DataTable LoadMerchantProfile()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.AddLoggedInUserPrimaryUserID();
        parameters.Add(new FilterParameter("@MerchantNumber", GetMerchantNumber(), DbType.AnsiString));
        parameters.Add(new FilterParameter("@FlagPermission", SessionManager.CurrentUserViewMode, DbType.Int32));
        parameters.AddLanguageID();
        parameters.AddDecryptDataParams("RoutingNumber");
        DataTable dtMerch = WebServices.CsReportServices.GetReports(SPA_GET_MERCHANT_PROFILE, parameters);
        return dtMerch;
    }

    private void SetVisibleRelationshipManager()
    {
        if (isShowRelationshipManager)
        {
            //uxpnlRMLabel.Visible = true;
            // uxpnlRMCtrls.Visible = true;
            if (GeneralFuncsLib.HasRelationshipManagerPermission((ReportPage)this.Page))
            {
                UpdateRelationshipMngMode(RelationshipManagerMode.View);
            }
            else
            {
                UpdateRelationshipMngMode(RelationshipManagerMode.ReadOnly);
            }

        }
        else
        {
            // uxpnlRMLabel.Visible = false;
            //  uxpnlRMCtrls.Visible = false;
        }
    }

    private void UpdateLabelRelationshipManager()
    {
        uxlbRelationshipManager.Visible = !RelationshipManagerIsEmpty;
        uxlbRelationshipManager.Text = RelationshipManagerIsEmpty
            ? string.Empty : RelationShipManager;
    }

    private string GetSubmitReportFilterValuesJS(string hierarchyMode, string value)
    {
        return string.Format("return rf_SubmitReportFilterValues({0}, '{1}', '{2}');",
            GeneralFuncsLib.GetHierarchyInfo(hierarchyMode).HierarchyID,
            hierarchyMode,
            value);
    }

    private void UpdateRelationshipMngMode(RelationshipManagerMode newMode)
    {
        switch (newMode)
        {
            case RelationshipManagerMode.ReadOnly:
                uxbtnAdd.Visible = false;
                uxbtnEdit.Visible = false;
                uxpnlSaveCancel.Visible = false;
                UpdateLabelRelationshipManager();
                break;
            case RelationshipManagerMode.View:
                uxbtnAdd.Visible = RelationshipManagerIsEmpty;
                uxbtnEdit.Visible = !uxbtnAdd.Visible;
                uxpnlSaveCancel.Visible = false;
                UpdateLabelRelationshipManager();
                break;
            case RelationshipManagerMode.New:
                uxbtnAdd.Visible = false;
                uxbtnEdit.Visible = false;
                uxpnlSaveCancel.Visible = true;
                uxtxtRelationShipManager.Text = string.Empty;
                uxlbRelationshipManager.Text = string.Empty;
                uxlbRelationshipManager.Visible = false;
                break;
            case RelationshipManagerMode.Edit:
                uxbtnAdd.Visible = false;
                uxbtnEdit.Visible = false;
                uxpnlSaveCancel.Visible = true;
                uxtxtRelationShipManager.Text = RelationShipManager;
                uxlbRelationshipManager.Visible = false;
                break;
        }
    }

    private void GetLastBatchDate(object lastBatchActivity)
    {
        DateTime tempDate = new DateTime(1900, 1, 1);
        DateTime.TryParse(FormatDate(lastBatchActivity), out tempDate);
        LastBatchDate = tempDate.Year == 1900 ? "0" : tempDate.Ticks.ToString();
    }

    private void FormatHyperlinkOfHierarchyInfo(DataTable dtMerch)
    {
        foreach (RepeaterItem item in rptMerchantInfo.Items)
        {
            HtmlAnchor aSalesAgent = (HtmlAnchor)item.FindControl("uxLinkHSalesAgent");
            HtmlAnchor aAgent = (HtmlAnchor)item.FindControl("uxLinkHAgent");
            HtmlAnchor aHead = (HtmlAnchor)item.FindControl("uxLinkHHeadquaster");

            if (aSalesAgent != null && GeneralFuncsLib.CheckHierarchy(HierarchyMode.MCPS_SALESAGENT))
            {
                aSalesAgent.Attributes.Add("onclick",
                    GetSubmitReportFilterValuesJS(
                        HierarchyMode.MCPS_SALESAGENT,
                        dtMerch.Rows[0]["SalesAgent"].ToString())
                    );
            }

            if (aAgent != null && GeneralFuncsLib.CheckHierarchy(HierarchyMode.MCPS_AGENT))
            {
                aAgent.Attributes.Add("onclick",
                    GetSubmitReportFilterValuesJS(
                        HierarchyMode.MCPS_AGENT,
                        dtMerch.Rows[0]["SysPrinAgent"].ToString())
                    );
            }
            if (aHead != null && GeneralFuncsLib.CheckHierarchy(HierarchyMode.MCPS_HEADQUARTER))
            {
                aHead.Attributes.Add("onclick",
                    GetSubmitReportFilterValuesJS(
                        HierarchyMode.MCPS_HEADQUARTER,
                        dtMerch.Rows[0]["Headquarter"].ToString())
                    );
            }
        }
    }
    #endregion Private Methods

    #endregion Methods
}
