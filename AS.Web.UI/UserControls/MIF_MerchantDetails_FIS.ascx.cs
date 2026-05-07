using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class UserControls_MIF_MerchantDetails_FIS : ExportMultiSections
{
    #region Constants

    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string FILE_NAME_BI = "~/App_Data/tpl_BusinessInformation_FIS.htm";
    private const string FILE_NAME_HIERARCHY_INFO = "~/App_Data/tpl_HierarchyInformation_FIS.htm";
    private const string FILE_NAME_BANK_INFO = "~/App_Data/tpl_BankInformation_ORION.htm";
    private const string FILE_NAME_OTHER_CARD_INFO = "~/App_Data/tpl_OtherCardInformation_FIS.htm";
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation.htm";
    private const string FILE_NAME_MERCHANT_INFO_REP = "~/App_Data/tpl_MerchantInformation_FISMPS_Rep.htm";
    private const string OPTED_IN = "Opted In";

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

    protected enum DataBindAction
    {
        BindMerchantDetails,
        UpdateMerchantStatus
    }

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

    protected string _OptedIn
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

    protected string _MifEmail
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

    protected string _TempStr
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
        uxpnlAddEdit.Visible = true;
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
        if (_MifTable.IsNotNullData() && _MifTable.Columns.Contains("UserID"))
        {
            userID = _MifTable.Rows[0]["UserID"].ToString();
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
        uxpnlAddEdit.Visible = true;

    }

    protected void uxbtnEdit_click(object sender, EventArgs e)
    {
        uxbtnAdd.Visible = false;
        uxbtnEdit.Visible = false;
        uxpnlAddEdit.Visible = false;
        uxpnlSaveCancel.Visible = true;
        uxtxtRelationShipManager.Text = RelationShipManager;
        uxlbRelationshipManager.Visible = false;
    }

    protected void uxbtnAdd_click(object sender, EventArgs e)
    {
        uxbtnAdd.Visible = false;
        uxbtnEdit.Visible = false;
        uxpnlAddEdit.Visible = false;
        uxpnlSaveCancel.Visible = true;
        uxtxtRelationShipManager.Text = string.Empty;
    }

    protected void uxbtnSave_click(object sender, EventArgs e)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new FilterParameter("@MerchantNumber", GetMerchantNrParameter(), DbType.AnsiString));
        parameters.Add(new FilterParameter("@RelationshipManager", uxtxtRelationShipManager.Text.Trim(), DbType.String));
        WebServices.CsReportServices.ExecuteNonQueryCommand(
            MerchantProfileHelper.SPA_UPDATE_RELATIONSHIP_MANAGER, parameters, out parameters);

        uxpnlSaveCancel.Visible = false;
        uxpnlAddEdit.Visible = true;
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
            (SecurePage)Page, MerchantNumber, _OptedIn, _MifEmail);
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
                string spa_name = "spa_cs_GetMerchantProfile_TSYS";
                if (SessionManager.CurrentClient == WebSiteConstants.FULTON_CLIENT)
                {
                    spa_name = "spa_cs_GetMerchantProfile_TSYS_Detail";
                }
                /*
                else
                {
                    parameters.AddLanguageID();
                }
                */
                DataTable dtMerch = WebServices.CsReportServices.GetReports(spa_name, parameters);

                if (dtMerch.Rows.Count > 0)
                {
                    _MifTable = dtMerch;
                    _OptedIn = dtMerch.Rows[0]["SiteAccess"].ToString().Equals(Resources.Template.OptedIn, StringComparison.OrdinalIgnoreCase) ? "1" : "0";
                    _MifEmail = dtMerch.Rows[0]["Email"].ToString();
                    phdMerchantDetail.Visible = true;

                    if (dtMerch.Columns.Contains("RelationShipManager"))
                    {
                        RelationShipManager = dtMerch.Rows[0]["RelationShipManager"].ToString();
                    }

                    // ShowRelationshipManager
                    ShowHideRelationshipManager();
                    _merchantInfor = dtMerch;
                    lnkLastBatch.Visible = !BindValue("LastBactchActivity").Trim().IsNullOrEmpty();
                    if (lnkLastBatch.Visible)
                    {
                        lnkLastBatch.Text = FormatDate(BindValue("LastBactchActivity").Trim());
                    }
                    // END ShowRelationshipManager

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
                    this.rptMerchantInfo.DataSource = dtMerch;
                    this.rptMerchantInfo.DataBind();
                    foreach (RepeaterItem item in rptMerchantInfo.Items)
                    {
                        HtmlAnchor aChain = (HtmlAnchor)item.FindControl("uxLinkHChainNumber");
                        if (aChain != null && CheckHierarchy(HierarchyMode.CHAIN_NR))
                        {
                            aChain.Attributes.Add("onclick",
                                 MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                    HierarchyMode.CHAIN_NR,
                                    dtMerch.Rows[0]["Chain"].ToString()));
                        }
                    }
                }
                else
                {
                    phdMerchantDetail.Visible = false;
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

    public string GetRoutingNumber(string full, string partial)
    {
        return MerchantProfileHelper.GetRoutingNumberWithoutDecrypt(full, partial);
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

        if ( IsRiskInformation )
        {
            var uxRiskInfo = rptMerchantInfo.Items[0].FindControl("uxRiskInfo") as UserControls_MIF_RiskInformationSection;
            if (uxRiskInfo != null)
                exportFucntions.Add(2, () => { return uxRiskInfo.ExcelRiskInformation(); });
            exportFucntions.Add(3, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(4, ButtonExcelBankInformation_Click);
            exportFucntions.Add(5, ButtonExcelOtherCardInformation_Click);
        }
        else
        {
            exportFucntions.Add(2, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(3, ButtonExcelBankInformation_Click);
            exportFucntions.Add(4, ButtonExcelOtherCardInformation_Click);
        }
        return exportFucntions;
    }
    public void DoMultiExportExcel()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();
        exportNames.Add(0, GetLocalResourceObject("LiteralResource1.Text").ToString());
        exportNames.Add(1, GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_BusinessInformation").ToString());

        if ( IsRiskInformation )
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_Text_RiskInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_HierarchyInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_BankInformation").ToString());
            exportNames.Add(5, GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_OtherCardInformation").ToString());
        }
        else
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_HierarchyInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_BankInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_OtherCardInformation").ToString());
        }
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
        DataTable dataSource = (DataTable)(_MifTable);
        if (dataSource == null || dataSource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_BI)));
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessInformation]", Resources.Template.tpl_BusinessInformation_htm_BusinessInformation);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_OpenDate]", Resources.Template.tpl_BusinessInformation_htm_OpenDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ClosedDate]", Resources.Template.tpl_BusinessInformation_htm_ClosedDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Status]", Resources.Template.tpl_BusinessInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SiteAccess]", Resources.Template.tpl_BusinessInformation_htm_SiteAccess);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_TaxID]", Resources.Template.tpl_BusinessInformation_htm_TaxID);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SICMCC]", Resources.Template.tpl_BusinessInformation_htm_SICMCC);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_AverageSalesAmount]", Resources.Template.tpl_BusinessInformation_EMS_htm_AverageSalesAmount);
        strExcelTemplate.Replace("BI_APPROVAL_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["ApprovalDate"]));
        strExcelTemplate.Replace("BI_CLOSE_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["ClosedDate"]));
        strExcelTemplate.Replace("BI_STATUS", "&nbsp;" + dataSource.Rows[0]["Status"].ToString());
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
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_BusinessInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelHierarchyInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_HIERARCHY_INFO)));

        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", Resources.Template.tpl_HierarchyInformation_htm_HierarchyInformation);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientName]", Resources.Template.tpl_HierarchyInformation_htm_ClientName);                
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientLogin]", Resources.Template.tpl_HierarchyInformation_htm_ClientLogin);        
        strExcelTemplate.Replace("[tpl_HierarchyInformation_FIS_htm_ASSOCIATION]", Resources.Template.tpl_HierarchyInformation_FIS_htm_ASSOCIATION);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_FIS_htm_ServicedBy]", Resources.Template.tpl_HierarchyInformation_FIS_htm_ServicedBy); 
        strExcelTemplate.Replace("[tpl_HierarchyInformation_FIS_htm_Bank]", Resources.Template.tpl_HierarchyInformation_FIS_htm_Bank);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_FIS_htm_Chain]", Resources.Template.tpl_HierarchyInformation_FIS_htm_MICChain);
        
        strExcelTemplate.Replace("HI_CLIENT_NAME", "&nbsp;" + _datasource.Rows[0]["HifBankName"].ToString());
        strExcelTemplate.Replace("HI_CLIENT_LOGIN", "&nbsp;" + _datasource.Rows[0]["ClientLogin"].ToString());
        strExcelTemplate.Replace("HI_BANK", "&nbsp;" + _datasource.Rows[0]["Bank"].ToString());
        strExcelTemplate.Replace("HI_ASSOCIATION", "&nbsp;" + _datasource.Rows[0]["ASSOCIATION"].ToString());
        strExcelTemplate.Replace("HI_CHAIN_CODE", "&nbsp;" + _datasource.Rows[0]["Chain"].ToString());
        strExcelTemplate.Replace("HI_SERVICED_BY", "&nbsp;" + _datasource.Rows[0]["ServicedBy"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_HierarchyInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelBankInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_BANK_INFO)));
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankInformation]", Resources.Template.tpl_BankInformation_htm_BankInformation);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankName]", Resources.Template.tpl_BankInformation_htm_BankName);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_RoutingSharp]", Resources.Template.tpl_BankInformation_htm_RoutingSharp);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DDASharp]", Resources.Template.tpl_BankInformation_htm_DDASharp);
        strExcelTemplate.Replace("BI_BANK_NAME", "&nbsp;" + _datasource.Rows[0]["BankName"].ToString());
        strExcelTemplate.Replace("BI_ROUTING_#", "&nbsp;" + _datasource.Rows[0]["PartialRoutingNumber"].ToString());
        strExcelTemplate.Replace("BI_DDA_#", "&nbsp;" + _datasource.Rows[0]["PartialDDANumber"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_BankInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelOtherCardInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_OTHER_CARD_INFO)));
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_OtherCardInformation]", Resources.Template.tpl_OtherCardInformation_htm_OtherCardInformation);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_FIS_htm_AMEXMID]", Resources.Template.tpl_OtherCardInformation_FIS_htm_AMEXMID);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_FIS_htm_DiscoverMID]", Resources.Template.tpl_OtherCardInformation_FIS_htm_DiscoverMID);
        strExcelTemplate.Replace("OI_AMEX_MID", "&nbsp;" + _datasource.Rows[0]["AMEX"].ToString());
        strExcelTemplate.Replace("OI_Discover_MID", "&nbsp;" + _datasource.Rows[0]["Discover"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FISCS_Text_OtherCardInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate;
        if (uxpnlRMLabel.Visible)
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
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("LiteralResource1.Text").ToString());
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

    protected bool CheckPermissionAddEditChain(object siteAccess)
    {
        return MerchantProfileHelper.CheckPermissionAddEditChain(siteAccess, (SecurePage)Page);
    }

    protected void uxMerchantInfoItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        PlaceHolder plChain = ((PlaceHolder)e.Item.FindControl("uxPlaceChain"));
        PlaceHolder plChainNew = ((PlaceHolder)e.Item.FindControl("uxPlaceChainNew"));
        Literal ltChain = ((Literal)e.Item.FindControl("uxLHChain"));
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

    #endregion

    #region Private Methods

    private void ShowHideRelationshipManager()
    {
        uxlbRelationshipManager.Text = string.Empty;
        if (_isShowRelationshipManager)
        {
            uxpnlRMLabel.Visible = true;
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
            uxpnlRMLabel.Visible = false;
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
