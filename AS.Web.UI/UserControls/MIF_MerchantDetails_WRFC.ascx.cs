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
using AS.Controls.Pages;
using System.Collections.Generic;
using System.Configuration;

public partial class UserControls_MIF_MerchantDetails_WRFC : ExportMultiSections
{
    #region Constants

    // Path
    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string PATH_BANK_INFO_TPL_FILE = "~/App_Data/tpl_BankInformation_WRFC.htm";
    private const string PATH_HI_TPL_FILE = "~/App_Data/tpl_HierarchyInformation_WRFC.htm";
    private const string PATH_BI_TPL_FILE = "~/App_Data/tpl_BusinessInformation_WRFC.htm";
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation_WRFC_NTS.htm";
    private const string FILE_NAME_MERCHANT_INFO_REP = "~/App_Data/tpl_MerchantInformation_WRFC_NTS_Rep.htm";
    // File Name
    private const string FILE_NAME_BI = "Business Information";
    private const string FILE_NAME_HI = "Hierarchy Information";
    private const string FILE_NAME_BANK_INFO = "Bank Information";

    // SPA Name
    private const string SPA_GET_MERCHANT_PROFILE = "spa_cs_GetMerchantProfile_WRFC";

    private const string OPTED_IN = "Opted In";

    #endregion Constants

    #region Fields

    private bool _isCaseManagement = false;
    private bool _isShowRelationshipManager = GeneralFuncsLib.IsShowRelationshipManager();
    private DataTable _merchantInfor;
    private string[] noprefix = { "mr ", "ms ", "sir ", "jr ", "mr.", "ms.", "sir.", "jr." };

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

    public DataTable MifTable
    {
        get
        {
            return ViewState["MerchantInfomation"] != null ?
                (DataTable)(ViewState["MerchantInfomation"]) : null;
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

    public bool CheckIsExsistPrefix(string text, string[] prefix)
    {
        bool r = false;
        foreach (string pf in prefix)
        {
            if (text.ToLower().StartsWith(pf.ToLower()) || text.ToLower().EndsWith(pf.ToLower()))
            {
                r = true;
                break;
            }
        }
        return r;
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
        uxpnlAddEdit.Visible = !uxpnlSaveCancel.Visible;

    }

    protected void uxbtnEdit_click(object sender, EventArgs e)
    {
        uxbtnAdd.Visible = false;
        uxbtnEdit.Visible = false;
        uxpnlSaveCancel.Visible = true;
        uxpnlAddEdit.Visible = !uxpnlSaveCancel.Visible;
        uxtxtRelationShipManager.Text = RelationShipManager;
        uxlbRelationshipManager.Visible = false;
    }

    protected void uxbtnAdd_click(object sender, EventArgs e)
    {
        uxbtnAdd.Visible = false;
        uxbtnEdit.Visible = false;
        uxpnlSaveCancel.Visible = true;
        uxpnlAddEdit.Visible = !uxpnlSaveCancel.Visible;
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
        uxpnlAddEdit.Visible = !uxpnlSaveCancel.Visible;

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
        return _merchantInfor == null || _merchantInfor.Rows.Count <= 0
            ? string.Empty : _merchantInfor.Rows[0][colName].ToString();
    }

    protected void lnkLastBatch_Click(object sender, EventArgs e)
    {
        BuildBatchHistoryLink();
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
                parameters.Add(new FilterParameter("@MerchantNumber", GetMerchantNrParameter(), DbType.AnsiString));
                parameters.Add(new FilterParameter("@FlagPermission", SessionManager.CurrentUserViewMode, DbType.Int32));
                parameters.AddLanguageID();
                DataTable dtMerch = WebServices.CsReportServices.GetReports(SPA_GET_MERCHANT_PROFILE, parameters);

                if (dtMerch.Rows.Count > 0)
                {
                    MifTable = dtMerch;
                    this.OptedIn = dtMerch.Rows[0]["SiteAccess"].ToString().Equals(Resources.Template.OptedIn, StringComparison.OrdinalIgnoreCase) ? "1" : "0";
                    this.MifEmail = dtMerch.Rows[0]["Email"].ToString();
                    phdMerchantDetail.Visible = true;
                    if (dtMerch.Columns.Contains("RelationShipManager"))
                    {
                        RelationShipManager = dtMerch.Rows[0]["RelationShipManager"].ToString();
                    }

                    // ShowRelationshipManager
                    ShowHideRelationshipManager();
                    this._merchantInfor = dtMerch;
                    lnkLastBatch.Text = FormatDate(BindValue("LastBactchActivity"));
                    // END ShowRelationshipManager

                    if (string.IsNullOrEmpty(MerchantNumber)
                        || (ReportPage.ReportFilter != null
                            && !MerchantNumber.Equals(ReportPage.ReportFilter.CurrentValue.Value)))
                    {
                        DateTime tempDate = new DateTime(1900, 1, 1);
                        DateTime.TryParse(FormatDate(dtMerch.Rows[0]["LastBactchActivity"]), out tempDate);
                        LastBatchDate = tempDate.Year == 1900 ? "0" : tempDate.Ticks.ToString();
                        if (!IsCaseManagement)
                            MerchantNumber = ReportPage.ReportFilter.CurrentValue.Value;
                    }
                    this.rptMerchantInfo.DataSource = dtMerch;
                    this.rptMerchantInfo.DataBind();

                    

                    foreach (RepeaterItem item in rptMerchantInfo.Items)
                    {
                        if (dtMerch.Rows[0]["Association"].ToString().IsNullOrEmpty())
                        {
                            PlaceHolder plhHGroup = item.FindControl("uxHGroup") as PlaceHolder;
                            plhHGroup.Visible = false;
                        }
                        if (dtMerch.Rows[0]["Chain"].ToString().IsNullOrEmpty())
                        {
                            PlaceHolder plhHGroup = item.FindControl("uxPlaceChain") as PlaceHolder;
                            plhHGroup.Visible = false;
                        }
                        HtmlAnchor aPartnerID = (HtmlAnchor)item.FindControl("uxLinkPartnerID");
                        if (aPartnerID != null && CheckHierarchy("PARTNERID"))
                        {
                            aPartnerID.Attributes.Add("onclick",
                                string.Format("return rf_SubmitReportFilterValues({0}, 'PARTNERID', '{1}');",
                                GeneralFuncsLib.GetHierarchyInfo("PARTNERID").HierarchyID,
                                dtMerch.Rows[0]["EntityNumber1"].ToString()));
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
                        && SessionManager.CurrentUserPermissions.Contains("AddEditChain")
                        && MerchantProfileHelper.SiteAccessIsOptInOut(row["SiteAccess"]))
                    {
                        plChain.Visible = false;
                        plChainNew.Visible = true;
                        ltChain.Visible = false;
                    }
                    else
                    {
                        plChainNew.Visible = false;
                        if (CheckHierarchy(HierarchyMode.WRFC_CHAIN))
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

    protected string FormatDate(DataRow row, string key)
    {
        if (row == null || row[key] == DBNull.Value || row[key] == null)
        {
            return string.Empty;
        }
        else
        {
            return ((DateTime)row[key]).ToGenericDateString();
        }
    }

    protected bool setButton(object merchantStatusDesc)
    {
        bool result = false;
        if (!merchantStatusDesc.ToString().IsNullOrEmpty())
        {
            result = !merchantStatusDesc.ToString().ToLower().Equals("closed");
        }
        return result;
    }

    protected string setOptInStatus(object optIn, string merchantStatus)
    {
        return MerchantProfileHelper.SetOptInStatus(optIn, merchantStatus);
    }

    public string FormatSIC(object sic, object sicDesc)
    {
        return MerchantProfileHelper.FormatSIC(sic, sicDesc);
    }

    public string GetRoutingNumber(object full, object partial)
    {
        return MerchantProfileHelper.GetRoutingNumberByDecrypt(full, partial);
    }

    public string GetDDANumber(object full, object partial, string permissionCode)
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

        if (IsRiskInformation)
        {
            var uxRiskInfo = rptMerchantInfo.Items[0].FindControl("uxRiskInfo") as UserControls_MIF_RiskInformationSection;
            if (uxRiskInfo != null)
                exportFucntions.Add(2, () => { return uxRiskInfo.ExcelRiskInformation(); });

            exportFucntions.Add(3, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(4, ButtonExcelBankInformation_Click);
        }
        else
        {
            exportFucntions.Add(2, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(3, ButtonExcelBankInformation_Click);
        }
        return exportFucntions;
    }

    public void DoMultiExportExcel()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();
        exportNames.Add(0, GetLocalResourceObject("LiteralResource1.Text").ToString());
        exportNames.Add(1, GetLocalResourceObject("MIF_MerchantDetails_WRFCCS_Text_BusinessInformation").ToString());
        if (IsRiskInformation)
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_Text_RiskInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_WRFCCS_Text_HierarchyInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_WRFCCS_Text_BankInformation").ToString());
        }
        else
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_WRFCCS_Text_HierarchyInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_WRFCCS_Text_BankInformation").ToString());
        }
        ExportSectionNames = exportNames;
    }

    #region Processing Method

    protected string BindAddress(object add1, object add2, object add3,
        object city, object state, object zip)
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
        BuildBatchHistoryLink();
    }

    private void DownloadFile(string fileName, string fileType, string content)
    {
        Response.BufferOutput = true;
        switch (fileType)
        {
            case "xls":
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader(
                    "Content-Disposition",
                    "attachment; filename=\"" + VeraCodeSolution.RemoveCRLF(fileName) + ".xls\"");
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
            File.ReadAllText(Server.MapPath(PATH_BI_TPL_FILE)));
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessInformation]", Resources.Template.tpl_BusinessInformation_htm_BusinessInformation);
        strExcelTemplate.Replace("[tpl_BusinessInformation_ORION_htm_ApprovalDate]", Resources.Template.tpl_BusinessInformation_ORION_htm_ApprovalDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ClosedDate]", Resources.Template.tpl_BusinessInformation_htm_ClosedDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Status]", Resources.Template.tpl_BusinessInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SiteAccess]", Resources.Template.tpl_BusinessInformation_htm_SiteAccess);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_TaxID]", Resources.Template.tpl_BusinessInformation_htm_TaxID);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SICMCC]", Resources.Template.tpl_BusinessInformation_htm_SICMCC);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_AverageSalesAmount]", Resources.Template.tpl_BusinessInformation_EMS_htm_AverageSalesAmount);
        strExcelTemplate.Replace("BI_APPROVAL_DATE",
            GeneralFuncsLib.NBSP + FormatDate(dataSource.Rows[0]["ApprovalDate"]));
        strExcelTemplate.Replace("BI_CLOSE_DATE",
            GeneralFuncsLib.NBSP + FormatDate(dataSource.Rows[0]["ClosedDate"]));
        strExcelTemplate.Replace("BI_STATUS",
            GeneralFuncsLib.NBSP + dataSource.Rows[0]["Status"].ToString());
        strExcelTemplate.Replace("BI_SITE_ACCESS",
            HasMSProductEnvironment() ? GeneralFuncsLib.NBSP + dataSource.Rows[0]["SiteAccess"]
                                      : GeneralFuncsLib.NBSP + "Not Available");
        strExcelTemplate.Replace("BI_TAX_ID",
            GeneralFuncsLib.NBSP + dataSource.Rows[0]["PartialTaxID"].ToString());
        strExcelTemplate.Replace("BI_SIC_MCC",
            GeneralFuncsLib.NBSP
            + FormatSIC(dataSource.Rows[0]["SICCode"].ToString(),
                        dataSource.Rows[0]["SICDescription"].ToString()));
        strExcelTemplate.Replace("BI_AVER_SALES_AMT",
            GeneralFuncsLib.NBSP +
            FormatCurrency(dataSource.Rows[0]["Avg_Ticket_Amt"]));
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_WRFCCS_Text_BusinessInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelHierarchyInformation_Click()
    {
        DataTable datasource = (DataTable)(MifTable);
        if (datasource == null || datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(PATH_HI_TPL_FILE)));
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", Resources.Template.tpl_HierarchyInformation_htm_HierarchyInformation);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientName]", Resources.Template.tpl_HierarchyInformation_htm_ClientName);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientLogin]", Resources.Template.tpl_HierarchyInformation_htm_ClientLogin);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_ORION_htm_Group]", Resources.Template.tpl_HierarchyInformation_ORION_htm_Group);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_Fulton_htm_Association]", Resources.Template.tpl_HierarchyInformation_Fulton_htm_Association);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_Fulton_htm_Chain]", Resources.Template.tpl_HierarchyInformation_Fulton_htm_Chain);
        strExcelTemplate.Replace("HI_CLIENT_NAME",
            GeneralFuncsLib.NBSP + datasource.Rows[0]["ClientName"].ToString());
        strExcelTemplate.Replace("HI_CLIENT_LOGIN",
            GeneralFuncsLib.NBSP + datasource.Rows[0]["ClientLogin"].ToString());
        strExcelTemplate.Replace("HI_GROUP",
            GeneralFuncsLib.NBSP + datasource.Rows[0]["Group"].ToString());
        strExcelTemplate.Replace("HI_ASSO",
            GeneralFuncsLib.NBSP + datasource.Rows[0]["Association"].ToString());
        strExcelTemplate.Replace("HI_CHAIN_CODE",
            GeneralFuncsLib.NBSP + datasource.Rows[0]["Chain"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_WRFCCS_Text_HierarchyInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelBankInformation_Click()
    {
        DataTable datasource = (DataTable)(MifTable);
        if (datasource == null || datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(PATH_BANK_INFO_TPL_FILE)));
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankInformation]", Resources.Template.tpl_BankInformation_htm_BankInformation);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankName]", Resources.Template.tpl_BankInformation_htm_BankName);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_RoutingSharp]", Resources.Template.tpl_BankInformation_htm_RoutingSharp);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DDASharp]", Resources.Template.tpl_BankInformation_htm_DDASharp);
        strExcelTemplate.Replace("BI_BANK_NAME",
            GeneralFuncsLib.NBSP + datasource.Rows[0]["BankName"].ToString());
        strExcelTemplate.Replace("BI_ROUTING_#",
            GeneralFuncsLib.NBSP + datasource.Rows[0]["PartialRoutingNumber"].ToString());
        strExcelTemplate.Replace("BI_DDA_#",
            GeneralFuncsLib.NBSP + datasource.Rows[0]["PartialDDANumber"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_WRFCCS_Text_BankInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
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

        strExcelTemplate.Replace("HI_MERCHANT_NUMBER", "&nbsp;" + _datasource.Rows[0]["MerchantNumber"].ToString());
        strExcelTemplate.Replace("HI_CONTACT", "&nbsp;" + _datasource.Rows[0]["Contact"].ToString());
        strExcelTemplate.Replace("HI_STATUS", "&nbsp;" + _datasource.Rows[0]["Status"].ToString());
        strExcelTemplate.Replace("HI_MERCHANT_NAME", "&nbsp;" + _datasource.Rows[0]["MerchantName"].ToString());
        strExcelTemplate.Replace("HI_PHONE", "&nbsp;" + FormatPhone(_datasource.Rows[0]["Phone"]));
        strExcelTemplate.Replace("HI_LAST_BATCH_ACTIVITY", "&nbsp;" + FormatDate(_datasource.Rows[0]["LastBactchActivity"]));
        strExcelTemplate.Replace("HI_ADDRESS", "&nbsp;" + BindAddress(_datasource.Rows[0]["Address1"],
            _datasource.Rows[0]["Address2"], _datasource.Rows[0]["Address3"],
            _datasource.Rows[0]["City"], _datasource.Rows[0]["State"], _datasource.Rows[0]["Zip"]));
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

    protected bool CheckPermissionAddEditChain(object siteAccess)
    {
        return SessionManager.CurrentUserPermissions.Contains("AddEditChain")
            && MerchantProfileHelper.SiteAccessIsOptInOut(siteAccess);
    }

    protected bool CheckHierarchy(string hierarchy)
    {
        return MerchantProfileHelper.CheckHierarchy(hierarchy);
    }

    #endregion

    private void ShowHideRelationshipManager()
    {
        uxlbRelationshipManager.Text = string.Empty;
        if (_isShowRelationshipManager)
        {
            uxpnlRMLabel.Visible = true;
            uxpnlRMCtrls.Visible = true;
            if (RelationShipManager.IsNullOrEmpty())
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

    private void BuildBatchHistoryLink()
    {
        string merchantNr = null;
        merchantNr = MerchantNumber.IsNullOrEmpty()
            ? Page.SecureQueryString["MerchantNumber"] : MerchantNumber;
        if (merchantNr != null)
        {
            Response.Redirect(GeneralFuncsLib.BuildBatchHistoryUrl((SecurePage)Page, LastBatchDate, merchantNr));
        }
    }

    private string GetMerchantNrParameter()
    {
        return IsCaseManagement ? Page.SecureQueryString["MerchantNumber"]
            : ReportPage.ReportFilter.CurrentValue.Value;
    }

    #endregion Methods
}
