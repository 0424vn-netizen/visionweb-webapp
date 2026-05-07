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
using System.Web;
using AS.Common.Formater;

public partial class UserControls_MIF_MerchantDetails_ORI : ExportMultiSections
{
    #region Propertise using for Case Management

    private bool _IsCaseManagement = false;
    //private string _PricingCardTypeExportingHtmlTemplate = string.Empty;
    //private string DDANumber;
    public bool IsCaseManagement
    {
        get
        {
            return _IsCaseManagement;
        }
        set
        {
            _IsCaseManagement = value;
        }
    }
    #endregion

    protected enum DataBindAction
    {
        BindMerchantDetails,
        UpdateMerchantStatus
    }

    #region Constants

    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string FILE_NAME_BUSINESS_INFO = "~/App_Data/tpl_BusinessInformation_ORION.htm";
    private const string FILE_NAME_BANK_INFO = "~/App_Data/tpl_BankInformation_ORION.htm";
    private const string FILE_NAME_HIERARCHY_INFO = "~/App_Data/tpl_HierarchyInformation_ORION.htm";
    private const string FILE_NAME_PRICING_INFO = "~/App_Data/tpl_PricingInformation_ORION.htm";
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation.htm";
    private const string FILE_NAME_MERCHANT_INFO_REP = "~/App_Data/tpl_MerchantInformation_ORI_Rep.htm";

    private const string OPTED_IN = "Opted In";

    // SPA
    private const string SPA_GET_MERCHANT_CARD_TYPE = "spa_cs_GetMerchantCardTypes_ORION";
    private const string SPA_GET_MERCHANT_PROFILE = "spa_cs_GetMerchantProfile_ORION";

    #endregion Constants

    #region Fields

    private string[] noprefix = { "mr ", "ms ", "sir ", "jr ", "mr.", "ms.", "sir.", "jr." };
    private bool isShowRelationshipManager = GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RELATIONSHIP_MANAGER").Equals("true") ? true : false;
    private DataTable _MerchantInfor;

    #endregion Fields

    #region Properties

    protected DataTable _MifTable
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

    protected DataTable _MifCardTable
    {
        get
        {
            if (ViewState["MerchantCardInfo"] != null)
                return (DataTable)(ViewState["MerchantCardInfo"]);
            else
                return null;
        }
        set
        {
            ViewState["MerchantCardInfo"] = value;
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

    static string _noMatchingData = string.Empty;
    #endregion

    #region Methods

    protected string SetURLForSiteAccessButton()
    {
        return MerchantProfileHelper.BuildURLForSiteAccessInMIF(
            (SecurePage)Page, MerchantNumber, _OptedIn, _MifEmail);
    }

    protected void SetMerchantStatus(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.UpdateMerchantStatus);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _noMatchingData = GetLocalResourceObject("MIF_MerchantDetails_ORIJS_Text_NoMatchingData").ToString();
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
        parameters.Add(new FilterParameter("@MerchantNumber", GetMerchantNrParameter(), DbType.AnsiString));
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
        if (_MerchantInfor == null || _MerchantInfor.Rows.Count <= 0)
            return string.Empty;
        DataRow dr = _MerchantInfor.Rows[0];
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
                DataTable dtMerch = WebServices.CsReportServices.GetReports(SPA_GET_MERCHANT_PROFILE, parameters);

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
                    _MerchantInfor = dtMerch;
                    if (BindValue("LastBactchActivity").IsNullOrEmpty())
                    {
                        lnkLastBatch.Visible = false;
                    }
                    else
                    {
                        lnkLastBatch.Text = FormatDate(BindValue("LastBactchActivity"));
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
                }
                else
                    phdMerchantDetail.Visible = false;

                break;
        }
    }

    protected void uxMerchantInfoItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        Repeater uxOtherCardTypes = ((Repeater)e.Item.FindControl("uxOtherCardTypes"));
        DataTable pricingCardTypes = GetPricingCardTypes();
        uxOtherCardTypes.DataSource = pricingCardTypes;
        uxOtherCardTypes.DataBind();

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

    protected string DoVeraCode(DataRow row, string key)
    {
        if (row == null)
        {
            return string.Empty;
        }
        object obj = row[key];
        return (obj == DBNull.Value || obj == null) ? string.Empty : obj.ToString();
    }

    protected string FormatDate(DataRow row, string key)
    {
        if (row == null)
        {
            return string.Empty;
        }

        object obj = row[key];
        return (obj == DBNull.Value || obj == null)
            ? string.Empty : ((DateTime)obj).ToGenericDateString();
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

        if (IsRiskInformation)
        {
            var uxRiskInfo = rptMerchantInfo.Items[0].FindControl("uxRiskInfo") as UserControls_MIF_RiskInformationSection;
            if (uxRiskInfo != null)
                exportFucntions.Add(2, () => { return uxRiskInfo.ExcelRiskInformation(); });
            exportFucntions.Add(3, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(4, ButtonExcelBankInformation_Click);
            exportFucntions.Add(5, ButtonExcelPricingInformation_Click);
        }
        else
        {
            exportFucntions.Add(2, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(3, ButtonExcelBankInformation_Click);
            exportFucntions.Add(4, ButtonExcelPricingInformation_Click);
        }
        return exportFucntions;
    }

    public void DoMultiExportExcel()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();
        exportNames.Add(0, GetLocalResourceObject("LiteralResource1.Text").ToString());
        exportNames.Add(1, GetLocalResourceObject("MIF_MerchantDetails_ORICS_Text_BusinessInformation").ToString());

        if (IsRiskInformation)
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_Text_RiskInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_ORICS_Text_HierarchyInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_ORICS_Text_BankInformation").ToString());
            exportNames.Add(5, GetLocalResourceObject("MIF_MerchantDetails_ORICS_Text_PricingInformation").ToString());
        }
        else
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_ORICS_Text_HierarchyInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_ORICS_Text_BankInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_ORICS_Text_PricingInformation").ToString());
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
        return abc == DBNull.Value ? string.Empty : GeneralFuncsLib.FormatCurrency(abc);
    }

    protected string FormatCurrency(object abc, string nullText)
    {
        return abc == DBNull.Value ? nullText : GeneralFuncsLib.FormatCurrency(abc);
    }

    protected string FormatCurrency(object abc, int count)
    {
        return abc == DBNull.Value ? string.Empty : GeneralFuncsLib.FormatCurrency(abc);
    }

    protected string FormatPercent(object abc)
    {
        return abc == DBNull.Value ? string.Empty
            : GeneralFuncsLib.FormatPercentage(1, double.Parse(abc.ToString()), "<span>{0:#,0.00}%</span>");
    }

    protected string FormatPercent(object abc, string nullText)
    {
        return abc == DBNull.Value ? nullText
            : GeneralFuncsLib.FormatPercentage(1, double.Parse(abc.ToString()), "<span>{0:#,0.00}%</span>");
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
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_BUSINESS_INFO)));
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessInformation]", Resources.Template.tpl_BusinessInformation_htm_BusinessInformation);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_CorporateName]", Resources.Template.tpl_BusinessInformation_htm_CorporateName);
        strExcelTemplate.Replace("[tpl_BusinessInformation_ORION_htm_BillingAddress]", Resources.Template.tpl_BusinessInformation_ORION_htm_BillingAddress);
        strExcelTemplate.Replace("[tpl_BusinessInformation_ORION_htm_Phone]", Resources.Template.tpl_BusinessInformation_ORION_htm_Phone);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Fax]", Resources.Template.tpl_BusinessInformation_htm_Fax);
        strExcelTemplate.Replace("[tpl_BusinessInformation_ORION_htm_ApprovalDate]", Resources.Template.tpl_BusinessInformation_ORION_htm_ApprovalDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ClosedDate]", Resources.Template.tpl_BusinessInformation_htm_ClosedDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Status]", Resources.Template.tpl_BusinessInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SiteAccess]", Resources.Template.tpl_BusinessInformation_htm_SiteAccess);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_TaxID]", Resources.Template.tpl_BusinessInformation_htm_TaxID);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SICMCC]", Resources.Template.tpl_BusinessInformation_htm_SICMCC);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_AverageSalesAmount]", Resources.Template.tpl_BusinessInformation_EMS_htm_AverageSalesAmount);

        strExcelTemplate.Replace("BI_CORPORATE_NAME", "&nbsp;" + dataSource.Rows[0]["CorporateName"].ToString());
        strExcelTemplate.Replace("BI_CORPORATE_ADDRESS", "&nbsp;" + dataSource.Rows[0]["CorporateAddress"].ToString());
        strExcelTemplate.Replace("BI_PHONE", "&nbsp;" + FormatPhone(dataSource.Rows[0]["Phone"]));
        strExcelTemplate.Replace("BI_FAX", "&nbsp;" + FormatPhone(dataSource.Rows[0]["Fax"]));
        strExcelTemplate.Replace("BI_APPROVAL_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["ApprovalDate"]));
        strExcelTemplate.Replace("BI_CLOSE_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["ClosedDate"]));
        strExcelTemplate.Replace("BI_STATUS", "&nbsp;" + dataSource.Rows[0]["ActivityStatus"].ToString());
        if (HasMSProductEnvironment())
        {
            strExcelTemplate.Replace("BI_SITE_ACCESS", "&nbsp;" + dataSource.Rows[0]["SiteAccess"]);
        }
        else
        {
            strExcelTemplate.Replace("BI_SITE_ACCESS", "&nbsp;" + GetLocalResourceObject("MIF_MerchantDetails_ORIJS_Text_NotAvailable").ToString());
        }
        strExcelTemplate.Replace("BI_TAX_ID", "&nbsp;" + dataSource.Rows[0]["PartialTaxID"].ToString());
        strExcelTemplate.Replace("BI_SIC_MCC", "&nbsp;" + FormatSIC(dataSource.Rows[0]["SICCode"].ToString(), dataSource.Rows[0]["SICCodeDesc"].ToString()));
        strExcelTemplate.Replace("BI_AVER_SALES_AMT", "&nbsp;" + FormatCurrency(dataSource.Rows[0]["Avg_Ticket_Amt"]));
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_ORICS_Text_BusinessInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelHierarchyInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_HIERARCHY_INFO)));
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", Resources.Template.tpl_HierarchyInformation_htm_HierarchyInformation);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientName]", Resources.Template.tpl_HierarchyInformation_htm_ClientName);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientLogin]", Resources.Template.tpl_HierarchyInformation_htm_ClientLogin);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_ORION_htm_Porfolio]", Resources.Template.tpl_HierarchyInformation_ORION_htm_Porfolio);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_ORION_htm_Group]", Resources.Template.tpl_HierarchyInformation_ORION_htm_Group);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_FIS_htm_Chain]", Resources.Template.tpl_HierarchyInformation_FIS_htm_Chain);
        strExcelTemplate.Replace("HI_CLIENT_NAME", "&nbsp;" + _datasource.Rows[0]["ClientName"].ToString());
        strExcelTemplate.Replace("HI_CLIENT_LOGIN", "&nbsp;" + _datasource.Rows[0]["ClientLogin"].ToString());
        strExcelTemplate.Replace("HI_PORFOLIO", "&nbsp;" + _datasource.Rows[0]["Porfolio"].ToString());
        strExcelTemplate.Replace("HI_GROUP", "&nbsp;" + _datasource.Rows[0]["Group"].ToString());
        strExcelTemplate.Replace("HI_CHAIN_CODE", "&nbsp;" + _datasource.Rows[0]["Chain"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_ORICS_Text_HierarchyInformation").ToString());
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
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_ORICS_Text_BankInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelPricingInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_PRICING_INFO)));
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_PricingInformation]", Resources.Template.tpl_PricingInformation_htm_PricingInformation);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_WeeklyDeposit]", Resources.Template.tpl_PricingInformation_htm_WeeklyDeposit);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AuthGrid]", Resources.Template.tpl_PricingInformation_htm_AuthGrid);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_MasterCard]", Resources.Template.tpl_PricingInformation_ORION_htm_MasterCard);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_Visa]", Resources.Template.tpl_PricingInformation_ORION_htm_Visa);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_QualRate]", Resources.Template.tpl_PricingInformation_ORION_htm_QualRate);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_Mid_QualRate]", Resources.Template.tpl_PricingInformation_ORION_htm_Mid_QualRate);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_Non_QualRate]", Resources.Template.tpl_PricingInformation_ORION_htm_Non_QualRate);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_MinMonthlyFee]", Resources.Template.tpl_PricingInformation_ORION_htm_MinMonthlyFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_Merch_ServiceFee]", Resources.Template.tpl_PricingInformation_ORION_htm_Merch_ServiceFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_DepositType]", Resources.Template.tpl_PricingInformation_ORION_htm_DepositType);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_FeeClasses]", Resources.Template.tpl_PricingInformation_ORION_htm_FeeClasses);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_FeeFlag1]", Resources.Template.tpl_PricingInformation_ORION_htm_FeeFlag1);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_FeeFlag2]", Resources.Template.tpl_PricingInformation_ORION_htm_FeeFlag2);
        strExcelTemplate.Replace("[tpl_PricingInformation_ORION_htm_OtherCardTypes]", Resources.Template.tpl_PricingInformation_ORION_htm_OtherCardTypes);
        strExcelTemplate.Replace("PI_WEEKLY_DEPOSIT", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["weekly_deposit_amt"]));
        strExcelTemplate.Replace("PI_AUTH_GRID", "&nbsp;" + _datasource.Rows[0]["auth_inc_grid_id"].ToString());
        strExcelTemplate.Replace("PI_MC_QUAL_RATE", "&nbsp;" + FormatPercent(_datasource.Rows[0]["MCQualRate"], "N/A"));
        strExcelTemplate.Replace("PI_VS_QUAL_RATE", "&nbsp;" + FormatPercent(_datasource.Rows[0]["VSQualRate"], "N/A"));
        strExcelTemplate.Replace("PI_MC_MID_QUAL_RATE", "&nbsp;" + FormatPercent(_datasource.Rows[0]["MCMQualRate"], "N/A"));
        strExcelTemplate.Replace("PI_VS_MID_QUAL_RATE", "&nbsp;" + FormatPercent(_datasource.Rows[0]["VSMQualRate"], "N/A"));
        strExcelTemplate.Replace("PI_MC_NON_QUAL_RATE", "&nbsp;" + FormatPercent(_datasource.Rows[0]["MCNQualRate"], "N/A"));
        strExcelTemplate.Replace("PI_VS_NON_QUAL_RATE", "&nbsp;" + FormatPercent(_datasource.Rows[0]["VSNQualRate"], "N/A"));
        strExcelTemplate.Replace("PI_MONTHLY_FEE", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["acct_fee_chg2"], FormatData.FormatCurrency("0.00", SessionManager.CurrencyFortmat)));
        strExcelTemplate.Replace("PI_MERCH_SERVICE_FEE", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["acct_fee_chg"], FormatData.FormatCurrency("0.00", SessionManager.CurrencyFortmat)));
        strExcelTemplate.Replace("PI_DEPOSIT_TYPE", "&nbsp;" + _datasource.Rows[0]["deposit_type"].ToString());
        strExcelTemplate.Replace("PI_FEE_FLAG1", "&nbsp;" + _datasource.Rows[0]["FeeFlag1"].ToString());
        strExcelTemplate.Replace("PI_FEE_FLAG2", "&nbsp;" + _datasource.Rows[0]["FeeFlag2"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_ORICS_Text_PricingInformation").ToString());

        string strTemplate = @"<tr class='OTHER_CARD_TYPE_CSS'> 
                                <td>OTHER_CARD_TYPE_NAME</td>
                                <td colspan='2'>OTHER_CARD_TYPE_ACC</td>
                              </tr>";
        string temp = string.Empty;
        DataTable pricingCardTypes = GetPricingCardTypes();

        StringBuilder exportingCardTypes = new StringBuilder();

        for (int i = 0; i < pricingCardTypes.Rows.Count; i++)
        {
            temp = strTemplate;
            temp = temp.Replace("OTHER_CARD_TYPE_NAME", pricingCardTypes.Rows[i]["Card_type"].ToString());
            temp = temp.Replace("OTHER_CARD_TYPE_ACC", "&nbsp;" + pricingCardTypes.Rows[i]["MerchantNumber"].ToString());
            if (i == pricingCardTypes.Rows.Count - 1)
            {
                temp = temp.Replace("OTHER_CARD_TYPE_CSS", "borderBottom");
            }
            else
                temp = temp.Replace("OTHER_CARD_TYPE_CSS", "");
            exportingCardTypes.Append(temp);
        }

        strExcelTemplate.Replace("PI_OTHER_CARD_TYPE", exportingCardTypes.ToString());
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
        strExcelTemplate.Replace("HI_STATUS", "&nbsp;" + _datasource.Rows[0]["ActivityStatus"].ToString());
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

    //Get other cardtype
    protected DataTable GetPricingCardTypes()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@MerchantNumber", GetMerchantNrParameter(), DbType.AnsiString));
        DataTable dt = WebServices.CsReportServices.GetReports(SPA_GET_MERCHANT_CARD_TYPE, parameters);
        return dt;
    }

    private static string convertTable2String(DataTable dt)
    {
        string result = string.Empty;
        if (dt == null || dt.Rows.Count == 0)
            return _noMatchingData;
        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            strBuilder.Append(dt.Rows[i]["Card_type"] + ", ");
        }
        result = strBuilder.ToString().Trim().TrimEnd(',').TrimStart(',');
        result = result.Replace(",", @"<br />");
        if (string.IsNullOrEmpty(result))
            return _noMatchingData;
        return result;
    }
    #endregion

    protected bool CheckPermissionAddEditChain(object siteAccess)
    {
        return SessionManager.CurrentUserPermissions.Contains("AddEditChain")
            && MerchantProfileHelper.SiteAccessIsOptInOut(siteAccess);
    }

    protected bool CheckHierarchy(string hierarchy)
    {
        return MerchantProfileHelper.CheckHierarchy(hierarchy);
    }

    #region Private Methods

    private void ShowHideRelationshipManager()
    {
        uxlbRelationshipManager.Text = string.Empty;
        if (isShowRelationshipManager)
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
