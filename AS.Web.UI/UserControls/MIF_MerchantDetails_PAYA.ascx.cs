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

public partial class UserControls_MIF_MerchantDetails_PAYA : ExportMultiSections
{
    #region Enums

    protected enum DataBindAction
    {
        BindMerchantDetails,
        UpdateMerchantStatus
    }

    protected enum BackEndprocessor
    {
        BankCard,
        ACH
    }

    #endregion Enums

    #region Constants
    // Export file
    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string FILE_NAME_BUSINESS_INFO = "~/App_Data/tpl_BusinessInformation_PAYA.htm";
    private const string FILE_NAME_BANKCARD_HIERARCHY_INFO = "~/App_Data/tpl_HierarchyInformation_BANKCARD_PAYA.htm";
    private const string FILE_NAME_ACH_HIERARCHY_INFO = "~/App_Data/tpl_HierarchyInformation_ACH_PAYA.htm";
    private const string FILE_NAME_OTHER_CARD_INFO = "~/App_Data/tpl_OtherCardInformation_FIS.htm";
    //TK 41447
    private const string FILE_NAME_BANK_INFO = "~/App_Data/tpl_BankInformation_PAYA.htm";
    private const string FILE_NAME_BANK_PART_INFO = "~/App_Data/tpl_BankInformation_MON_Part.htm";
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation_PAYA.htm";
    private const string FILE_NAME_MERCHANT_INFO_REP = "~/App_Data/tpl_MerchantInformation_ORI_Rep.htm";
    // SPA
    private const string SPA_GET_MERCHANT_PROFILE = "spa_cs_GetMerchantProfile_TSYS_Detail_PAYA";
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

    protected string BEProcessor
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
        parameters.Add(new FilterParameter("@MerchantNumber", ReportPage.ReportFilter.CurrentValue.Value, DbType.AnsiString));
        parameters.Add(new FilterParameter("@RelationshipManager", uxtxtRelationShipManager.Text.Trim(), DbType.String));
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
        return (_merchantInfor == null || _merchantInfor.Rows.Count <= 0)
            ? string.Empty : _merchantInfor.Rows[0][colName].ToString();
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
                    BEProcessor = dtMerch.Rows[0]["BackEndProcessor"].ToString();
                    MifTable = dtMerch;
                    OptedIn = dtMerch.Rows[0]["SiteAccess"].ToString().Equals(Resources.Template.OptedIn, StringComparison.OrdinalIgnoreCase)
                        ? MerchantProfileHelper.OPTED_IN_VALUE : MerchantProfileHelper.OPTED_OUT_VALUE;
                    MifEmail = dtMerch.Rows[0]["Email"].ToString();
                    phdMerchantDetail.Visible = true;

                    if (dtMerch.Columns.Contains("RelationShipManager"))
                    {
                        RelationShipManager = dtMerch.Rows[0]["RelationShipManager"].ToString();
                    }

                    // ShowRelationshipManager
                    ShowHideRelationshipManager();
                    _merchantInfor = dtMerch;

                    if (BindValue("LastBactchActivity").IsNullOrEmpty())
                    {
                        lnkLastBatch.Visible = false;
                    }
                    else
                    {
                        lnkLastBatch.Visible = true;
                        lnkLastBatch.Text = FormatDate(BindValue("LastBactchActivity"));
                    }

                    // END ShowRelationshipManager
                    // Check enabled/disabled Site Access Button
                    string userID = string.Empty;
                    if (dtMerch.IsNotNullData() && dtMerch.Columns.Contains("UserID"))
                    {
                        userID = dtMerch.Rows[0]["UserID"].ToString();
                    }
                    uxPnlSiteAccess.Visible = MerchantProfileHelper.HasSiteAccessPermission(this.Page) && !string.IsNullOrEmpty(userID);

                    if (MerchantNumber.IsNullOrEmpty()
                        || (ReportPage.ReportFilter != null
                            && !MerchantNumber.Equals(ReportPage.ReportFilter.CurrentValue.Value)))
                    {
                        CustomReport = (bool)dtMerch.Rows[0]["CustomReport"];
                        RecipientEmail = dtMerch.Rows[0]["RecipientEmail"].ToString();
                        DateTime tempDate = new DateTime(1900, 1, 1);
                        DateTime.TryParse(FormatDate(dtMerch.Rows[0]["LastBactchActivity"]), out tempDate);
                        LastBatchDate = tempDate.Year == 1900 ? "0" : tempDate.Ticks.ToString();
                        MerchantNumber = ReportPage.ReportFilter.CurrentValue.Value;
                    }
                    this.rptMerchantInfo.DataSource = dtMerch;
                    this.rptMerchantInfo.DataBind();
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

    protected string setOptInStatus(object OptIn, string MerchantStatus)
    {
        return MerchantProfileHelper.SetOptInStatus(OptIn, MerchantStatus);
    }

    //TK 41447
    public string GetRoutingNumber(string full, string partial)
    {
        return MerchantProfileHelper.GetRoutingNumberWithoutDecrypt(full, partial);
    }

    //TK 41447
    public string GetDDANumber(string full, string partial, string permissionCode)
    {
        if (SessionManager.CurrentUserViewMode > 0)
        {
            if (Page.IsUserWithPermission(permissionCode)
                || Page.IsUserWithPermission("MS" + permissionCode))
            {
                return WebServices.CsReportServices.DecryptText(
                    (string)full, SessionManager.CurrentUser.ASClient);
            }
        }
        return (string)partial;

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
        exportFucntions.Add(2, ButtonExcelHierarchyInformation_Click);
        exportFucntions.Add(3, ButtonExcelBankInformation_Click);
        exportFucntions.Add(4, ButtonExcelOtherCardInformation_Click);

        return exportFucntions;
    }

    public void DoMultiExportExcel()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();
        exportNames.Add(0, GetLocalResourceObject("LiteralResource1.Text").ToString());
        exportNames.Add(1, GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_BusinessInformation").ToString());
        exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_HierarchyInformation").ToString());
        exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_BankInformation").ToString());
        exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_PAYACS_Text_OtherCardInfo").ToString());
        ExportSectionNames = exportNames;
    }

    #region Processing Method

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
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_BUSINESS_INFO)));
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessInformation]", Resources.Template.tpl_BusinessInformation_htm_BusinessInformation);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_OpenDate]", Resources.Template.tpl_BusinessInformation_htm_OpenDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ClosedDate]", Resources.Template.tpl_BusinessInformation_htm_ClosedDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Status]", Resources.Template.tpl_BusinessInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SiteAccess]", Resources.Template.tpl_BusinessInformation_htm_SiteAccess);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_TaxID]", Resources.Template.tpl_BusinessInformation_htm_TaxID);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SICMCC]", Resources.Template.tpl_BusinessInformation_htm_SICMCC);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_AverageSalesAmount]", Resources.Template.tpl_BusinessInformation_EMS_htm_AverageSalesAmount);
        strExcelTemplate.Replace("[tpl_BusinessInformation_SNET_htm_BusinessType]", Resources.Template.tpl_BusinessInformation_SNET_htm_BusinessType);

        strExcelTemplate.Replace("BI_APPROVAL_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["ApprovalDate"]));
        strExcelTemplate.Replace("BI_CLOSE_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["ClosedDate"]));
        strExcelTemplate.Replace("BI_STATUS", "&nbsp;" + dataSource.Rows[0]["Status"].ToString());
        strExcelTemplate.Replace("BI_BUSINESS_TYPE", "&nbsp;" + dataSource.Rows[0]["BusinessType"].ToString());

        if (HasMSProductEnvironment())
        {
            strExcelTemplate.Replace("BI_SITE_ACCESS", "&nbsp;" + dataSource.Rows[0]["SiteAccess"]);
        }
        else
        {
            strExcelTemplate.Replace("BI_SITE_ACCESS", "&nbsp;" + GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_NotAvailable").ToString());
        }
        strExcelTemplate.Replace("BI_TAX_ID", "&nbsp;" + dataSource.Rows[0]["PartialTaxID"].ToString());
        strExcelTemplate.Replace("BI_SIC_MCC", "&nbsp;" + FormatSIC(dataSource.Rows[0]["SICCode"].ToString(), dataSource.Rows[0]["SICCodeDesc"].ToString()));
        strExcelTemplate.Replace("BI_AVER_SALES_AMT", "&nbsp;" + FormatCurrency(dataSource.Rows[0]["Avg_Ticket_Amt"]));

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_BusinessInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelHierarchyInformation_Click()
    {
        StringBuilder strExcelTemplate = new StringBuilder();
        DataTable dataSource = (DataTable)(MifTable);
        if (dataSource == null || dataSource.Rows.Count == 0)
            return string.Empty;
        if (BEProcessor.ToLower() == BackEndprocessor.ACH.ToString().ToLower())
        {
            strExcelTemplate = new StringBuilder(
                           File.ReadAllText(Server.MapPath(FILE_NAME_ACH_HIERARCHY_INFO)));
            strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", Resources.Template.tpl_HierarchyInformation_htm_HierarchyInformation);
            strExcelTemplate.Replace("[tpl_HierarchyInformation_EMS_htm_ISO]", Resources.Template.tpl_HierarchyInformation_EMS_htm_ISO);
            strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Agent]", Resources.Template.tpl_HierarchyInformation_htm_Agent);
            strExcelTemplate.Replace("[tpl_HierarchyInformation_Paya_htm_Merchant]", Resources.Template.tpl_HierarchyInformation_Paya_htm_Merchant);
            strExcelTemplate.Replace("[tpl_HierarchyInformation_Paya_htm_Parent_Regional]", Resources.Template.tpl_HierarchyInformation_Paya_htm_Parent_Regional);
            strExcelTemplate.Replace("[tpl_HierarchyInformation_FIS_htm_Chain]", Resources.Template.tpl_HierarchyInformation_FIS_htm_Chain);
            strExcelTemplate.Replace("[tpl_HierarchyInformation_Paya_htm_SecondaryChain]", Resources.Template.tpl_HierarchyInformation_Paya_htm_SecondaryChain);


            // Hierarchy Information
            strExcelTemplate.Replace("HI_ISO", "&nbsp;" + dataSource.Rows[0]["EntityNumber5"].ToString());

            // Merchant Hierarchy
            strExcelTemplate.Replace("HI_AGENT", "&nbsp;" + dataSource.Rows[0]["EntityNumber7"].ToString());
            strExcelTemplate.Replace("HI_MERCHANT", "&nbsp;" + dataSource.Rows[0]["EntityNumber8"].ToString());
            strExcelTemplate.Replace("HI_PARENT_REGIONAL", "&nbsp;" + dataSource.Rows[0]["EntityNumber9"].ToString());
            strExcelTemplate.Replace("HI_CHAIN", "&nbsp;" + dataSource.Rows[0]["Chain"].ToString());
            strExcelTemplate.Replace("HI_SECONDARYCHAIN", "&nbsp;" + dataSource.Rows[0]["EntityNumber4"].ToString());
        }
        else
        {
            strExcelTemplate = new StringBuilder(
                File.ReadAllText(Server.MapPath(FILE_NAME_BANKCARD_HIERARCHY_INFO)));
            strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", Resources.Template.tpl_HierarchyInformation_htm_HierarchyInformation);
            strExcelTemplate.Replace("[tpl_HierarchyInformation_Paya_htm_Office]", Resources.Template.tpl_HierarchyInformation_Paya_htm_Office);
            strExcelTemplate.Replace("[tpl_HierarchyInformation_Fulton_htm_Association]", Resources.Template.tpl_HierarchyInformation_Fulton_htm_Association);
            strExcelTemplate.Replace("[tpl_HierarchyInformation_Paya_htm_Contractor]", Resources.Template.tpl_HierarchyInformation_Paya_htm_Contractor);
            strExcelTemplate.Replace("[tpl_HierarchyInformation_Paya_htm_SecondaryChain]", Resources.Template.tpl_HierarchyInformation_Paya_htm_SecondaryChain);
            strExcelTemplate.Replace("[tpl_HierarchyInformation_FIS_htm_Chain]", Resources.Template.tpl_HierarchyInformation_FIS_htm_Chain);


            // Hierarchy Information
            strExcelTemplate.Replace("HI_OFFICE", "&nbsp;" + dataSource.Rows[0]["EntityNumber1"].ToString());
            strExcelTemplate.Replace("HI_ASSOCIATION", "&nbsp;" + dataSource.Rows[0]["EntityNumber2"].ToString());

            // Merchant Hierarchy
            strExcelTemplate.Replace("HI_CONTRACTOR", "&nbsp;" + dataSource.Rows[0]["EntityNumber3"].ToString());
            strExcelTemplate.Replace("HI_CHAIN", "&nbsp;" + dataSource.Rows[0]["Chain"].ToString());
            strExcelTemplate.Replace("HI_SECONDARYCHAIN", "&nbsp;" + dataSource.Rows[0]["EntityNumber4"].ToString());
        }

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_HierarchyInformation").ToString());

        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelOtherCardInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_OTHER_CARD_INFO)));
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_OtherCardInformation]", Resources.Template.tpl_OtherCardInformation_htm_OtherCardInformation);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_FIS_htm_AMEXMID]", Resources.Template.tpl_OtherCardInformation_FIS_htm_AMEXMID);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_FIS_htm_DiscoverMID]", Resources.Template.tpl_OtherCardInformation_FIS_htm_DiscoverMID);
        strExcelTemplate.Replace("OI_AMEX_MID", "&nbsp;" + _datasource.Rows[0]["AMEX"].ToString());
        strExcelTemplate.Replace("OI_Discover_MID", "&nbsp;" + _datasource.Rows[0]["Discover"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_PAYACS_Text_OtherCardInfo").ToString());

        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelBankInformation_Click()
    {
        //TK 41447
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_BANK_INFO)));
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankInformation]", Resources.Template.tpl_BankInformation_htm_BankInformation);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_RoutingSharp]", Resources.Template.tpl_BankInformation_htm_RoutingSharp);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DDASharp]", Resources.Template.tpl_BankInformation_htm_DDASharp);
        strExcelTemplate.Replace("BI_ROUTING_#", "&nbsp;" + _datasource.Rows[0]["PartialRoutingNumber"].ToString());
        strExcelTemplate.Replace("BI_DDA_#", "&nbsp;" + _datasource.Rows[0]["PartialDDANumber"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_BankInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate;
        if (uxPnlRelationshipManager.Visible)
        {
            strExcelTemplate = new StringBuilder(
                File.ReadAllText(Server.MapPath(FILE_NAME_MERCHANT_INFO_REP)));
            strExcelTemplate.Replace("[tpl_MerchantInformation_htm_RelationshipManager]", Resources.Template.tpl_MerchantInformation_htm_RelationshipManager);
            strExcelTemplate.Replace("HI_RELATIONSHIP_MANAGER", "&nbsp;" + RelationShipManager);
        }
        else
        {
            strExcelTemplate = new StringBuilder(
                File.ReadAllText(Server.MapPath(FILE_NAME_MERCHANT_INFO)));
        }
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantInformation]", Resources.Template.tpl_MerchantInformation_htm_MerchantInformation);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantNumber]", Resources.Template.tpl_MerchantInformation_htm_MerchantNumber);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Contact]", Resources.Template.tpl_MerchantInformation_htm_Contact);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Status]", Resources.Template.tpl_MerchantInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantName]", Resources.Template.tpl_MerchantInformation_htm_MerchantName);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Phone]", Resources.Template.tpl_MerchantInformation_htm_Phone);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_LastBatchActivity]", Resources.Template.tpl_MerchantInformation_htm_LastBatchActivity);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Address]", Resources.Template.tpl_MerchantInformation_htm_Address);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Email]", Resources.Template.tpl_MerchantInformation_htm_Email);

        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_URL]", Resources.Template.tpl_MerchantInformation_htm_Url);

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
        strExcelTemplate.Replace("HI_URL", "&nbsp;" + (_datasource.Rows[0]["Url"] != DBNull.Value && !string.IsNullOrEmpty(_datasource.Rows[0]["Url"].ToString()) && !_datasource.Rows[0]["Url"].ToString().Equals("n/a", StringComparison.OrdinalIgnoreCase) ? _datasource.Rows[0]["Url"].ToString() : WebSiteConstants.HTML_EM_DASH));

        MerchantProfileHelper.FormatExportUserInfo(strExcelTemplate, _datasource);
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

    protected bool CheckDisplayHierachyMS(string hierarchy)
    {
        if (WebSiteSettings.WebSiteType.ToLower().IndexOf(WebSiteEnums.ProductEnvironment.MS.ToString().ToLower()) != -1)
            return MerchantProfileHelper.CheckDisplayHierachyMS(hierarchy);
        return true;
    }
    // TK 41447
    //private void BindBankRepeater(Repeater rpt)
    //{
    //    BankInformationTable = GetBankInformation();
    //    rpt.DataSource = BankInformationTable;
    //    rpt.DataBind();

    //    //if (BankInformationTable == null || BankInformationTable.Rows.Count <= 0)
    //    //{
    //    //    var control = rpt.Controls[rpt.Controls.Count - 1].Controls[0];
    //    //    control.FindControl("trEmpty").Visible = true;
    //    //}
    //}

    // TK 41447
    private string FormatDataToMDash(string value)
    {

        if (string.IsNullOrEmpty(value))
        {
            return WebSiteConstants.HTML_EM_DASH;
        }

        else return value.ToString();
    }

    protected void uxMerchantInfoItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        PlaceHolder plChain = ((PlaceHolder)e.Item.FindControl("uxChain"));
        PlaceHolder plChainNew = ((PlaceHolder)e.Item.FindControl("uxPlaceChainNew"));
        Literal ltChain = ((Literal)e.Item.FindControl("uxLHChain"));

        PlaceHolder plACHChain = ((PlaceHolder)e.Item.FindControl("uxACHChain"));
        PlaceHolder plACHChainNew = ((PlaceHolder)e.Item.FindControl("uxACHPlaceChainNew"));
        Literal ltACHChain = ((Literal)e.Item.FindControl("uxACHLHChain"));

        var uxAchInfor = e.Item.FindControl("uxAchInfor");
        var uxBankCardInfo = e.Item.FindControl("uxBankCardInfo");
        if (BEProcessor.ToLower() == BackEndprocessor.ACH.ToString().ToLower())
        {
            uxAchInfor.Visible = true;
            uxBankCardInfo.Visible = false;
        }
        else
        {
            uxAchInfor.Visible = false;
            uxBankCardInfo.Visible = true;
        }

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

                        plACHChain.Visible = false;
                        plACHChainNew.Visible = true;
                        ltACHChain.Visible = false;
                    }
                    else
                    {
                        plChainNew.Visible = false;
                        plACHChainNew.Visible = false;
                        if (CheckHierarchy("Chain"))
                        {
                            plChain.Visible = true;
                            ltChain.Visible = false;

                            plACHChain.Visible = true;
                            ltACHChain.Visible = false;
                        }
                        else
                        {
                            plChain.Visible = false;
                            ltChain.Visible = true;

                            plACHChain.Visible = false;
                            ltACHChain.Visible = true;
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

    private void ShowHideRelationshipManager()
    {
        uxlbRelationshipManager.Text = string.Empty;
        if (_isShowRelationshipManager)
        {
            uxPnlRelationshipManager.Visible = true;
            if (string.IsNullOrEmpty(RelationShipManager))
            {
                if (GeneralFuncsLib.HasRelationshipManagerPermission((ReportPage)this.Page))
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
                if (GeneralFuncsLib.HasRelationshipManagerPermission((ReportPage)this.Page))
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
            uxPnlRelationshipManager.Visible = false;
        }
    }

    private string GetMerchantNrParameter()
    {
        return IsCaseManagement ? Page.SecureQueryString["MerchantNumber"]
            : ReportPage.ReportFilter.CurrentValue.Value;
    }

    #endregion Private methods

    #endregion Methods
}