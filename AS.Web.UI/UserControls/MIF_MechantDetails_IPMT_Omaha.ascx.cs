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

public partial class UserControls_MIF_MechantDetails_IPMT_Omaha : ExportMultiSections
{
    private DataTable _MerchantInfor;

    #region Enums

    protected enum DataBindAction
    {
        BindMerchantDetails,
        UpdateMerchantStatus
    }

    #endregion Enums

    #region Constants

    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string FILE_NAME_MER_CARD_PIN_DEBIT_INFO =
        "~/App_Data/tpl_MerchantCardPinDebitInformation.htm";
    private const string FILE_NAME_MERCHANT_CARD_INFO =
        "~/App_Data/tpl_MerchantCardInformation_OMAHA.htm";
    private const string FILE_NAME_PRICING_INFO = "~/App_Data/tpl_PricingInformation.htm";
    private const string FILE_NAME_OTHER_CARD_INFO = "~/App_Data/tpl_OtherCardInformation.htm";
    private const string FILE_NAME_BANK_INFO = "~/App_Data/tpl_BankInformation.htm";
    private const string FILE_NAME_HIERARCHY_OMAHA_INFO = "~/App_Data/tpl_HierarchyInformation_OMAHA.htm";
    private const string FILE_NAME_BUSINESS_INFO = "~/App_Data/tpl_BusinessInformation.htm";
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation.htm";
    private const string FILE_NAME_MERCHANT_INFO_REP = "~/App_Data/tpl_MerchantInformation_ORI_Rep.htm";
    private const string OPTED_IN = "Opted In";

    // SPA Name
    private const string SPA_GET_MERCHANT_PROFILE = "spa_cs_GetMerchantProfile";
    private const string SPA_UPDATE_CUSTOM_CHAIN_REPORT = "spa_ms_UpdateCustomChainReport";

    private const string SYS = "Sys";
    private const string SYS_PRIN = "SysPrin";
    private const string SYS_PRIN_AGENT = "SysPrinAgent";


    #endregion Constants

    #region Fields

    string[] noprefix = { "mr ", "ms ", "sir ", "jr ", "mr.", "ms.", "sir.", "jr." };
    bool isShowRelationshipManager = GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RELATIONSHIP_MANAGER").Equals("true") ? true : false;

    #endregion Fields

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

    private string RelationShipManager
    {
        get;
        set;
    }

    #endregion Properties

    #region Methods

    protected string SetURLForSiteAccessButton()
    {
        return MerchantProfileHelper.BuildURLForSiteAccessInMIF(
            (SecurePage)Page, ReportPage.ReportFilter.CurrentValue.Value, OptedIn, MifEmail);
    }

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
        uxpnlSaveCancel.Visible = true;
        uxpnlAddEdit.Visible = false;
        uxtxtRelationShipManager.Text = RelationShipManager;
        uxlbRelationshipManager.Visible = false;
    }

    protected void uxbtnAdd_click(object sender, EventArgs e)
    {
        uxbtnAdd.Visible = false;
        uxbtnEdit.Visible = false;
        uxpnlSaveCancel.Visible = true;
        uxpnlAddEdit.Visible = false;
        uxtxtRelationShipManager.Text = string.Empty;
        uxlbRelationshipManager.Text = string.Empty;
        uxlbRelationshipManager.Visible = false;
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
                DataTable dtMerch = WebServices.CsReportServices.GetReports(SPA_GET_MERCHANT_PROFILE, parameters);

                if (dtMerch.Rows.Count > 0)
                {
                    _MifTable = dtMerch;
                    this.OptedIn = dtMerch.Rows[0]["SiteAccess"].ToString().Equals(Resources.Template.OptedIn, StringComparison.OrdinalIgnoreCase) ? "1" : "0";
                    this.MifEmail = dtMerch.Rows[0]["Email"].ToString();

                    if (dtMerch.Columns.Contains("RelationShipManager"))
                    {
                        RelationShipManager = dtMerch.Rows[0]["RelationShipManager"].ToString();
                    }

                    // ShowRelationshipManager
                    ShowHideRelationshipManager();

                    phdMerchantDetail.Visible = true;
                    if (string.IsNullOrEmpty(MerchantNumber)
                        || (ReportPage.ReportFilter != null
                            && !MerchantNumber.Equals(ReportPage.ReportFilter.CurrentValue.Value)))
                    {
                        CustomReport = (bool)dtMerch.Rows[0]["CustomReport"];
                        RecipientEmail = dtMerch.Rows[0]["RecipientEmail"].ToString();
                        DateTime tempDate = new DateTime(1900, 1, 1);
                        DateTime.TryParse(FormatDate(dtMerch.Rows[0]["LastBactchActivity"]), out tempDate);
                        LastBatchDate = tempDate.Year == 1900 ? "0" : tempDate.Ticks.ToString();

                    }
                    this._MerchantInfor = dtMerch;
                    this.rptMerchantInfo.DataSource = dtMerch;
                    this.rptMerchantInfo.DataBind();
                    if (BindValue("LastBactchActivity").IsNullOrEmpty())
                    {
                        lnkLastBatch.Visible = false;
                    }
                    else
                    {
                        lnkLastBatch.Text = FormatDate(BindValue("LastBactchActivity"));
                    }
                    foreach (RepeaterItem item in rptMerchantInfo.Items)
                    {
                        HtmlAnchor aSys = (HtmlAnchor)item.FindControl("uxLinkSys");
                        HtmlAnchor aSysPrin = (HtmlAnchor)item.FindControl("uxLinkSysPrin");
                        HtmlAnchor aSysPrinAgent = (HtmlAnchor)item.FindControl("uxLinkSysPrinAgent");

                        if (aSys != null && CheckHierarchy(HierarchyMode.IPMT_SYS))
                        {
                            if (!string.IsNullOrEmpty(dtMerch.Rows[0][SYS].ToString()))
                            {
                                aSys.Attributes.Add(
                                    "onclick",
                                    MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                        HierarchyMode.IPMT_SYS,
                                        dtMerch.Rows[0][SYS].ToString()));
                            }
                            else
                            {
                                PlaceHolder plSys = ((PlaceHolder)item.FindControl("uxSys"));
                                plSys.Visible = false;
                            }
                        }
                        if (aSysPrin != null && CheckHierarchy(HierarchyMode.IPMT_SYSPRIN))
                        {
                            if (!string.IsNullOrEmpty(dtMerch.Rows[0][SYS_PRIN].ToString()))
                            {
                                aSysPrin.Attributes.Add(
                                    "onclick",
                                    MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                        HierarchyMode.IPMT_SYSPRIN,
                                        dtMerch.Rows[0][SYS_PRIN].ToString()));
                            }
                            else
                            {
                                PlaceHolder plSysPrint = ((PlaceHolder)item.FindControl("uxSysPrin"));
                                plSysPrint.Visible = false;
                            }
                        }
                        if (aSysPrinAgent != null && CheckHierarchy(HierarchyMode.IPMT_SYSPRINAGENT))
                        {
                            if (!string.IsNullOrEmpty(dtMerch.Rows[0][SYS_PRIN_AGENT].ToString()))
                            {
                                aSysPrinAgent.Attributes.Add(
                                    "onclick",
                                    MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                        HierarchyMode.IPMT_SYSPRINAGENT,
                                        dtMerch.Rows[0][SYS_PRIN_AGENT].ToString()));
                            }
                            else
                            {
                                PlaceHolder plAgent = ((PlaceHolder)item.FindControl("uxSysPrinAgent"));
                                plAgent.Visible = false;
                            }
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
            exportFucntions.Add(6, ButtonExcelPricingInformation_Click);
            exportFucntions.Add(7, ButtonExcelMerchantCardInformation_Click);
            exportFucntions.Add(8, ButtonExcelMerchantCardPinDebitInformation_Click);
            exportFucntions.Add(9, ButtonExcelMerchantMemos_Click);
        }
        else
        {
            exportFucntions.Add(2, ButtonExcelHierarchyInformation_Click);
            exportFucntions.Add(3, ButtonExcelBankInformation_Click);
            exportFucntions.Add(4, ButtonExcelOtherCardInformation_Click);
            exportFucntions.Add(5, ButtonExcelPricingInformation_Click);
            exportFucntions.Add(6, ButtonExcelMerchantCardInformation_Click);
            exportFucntions.Add(7, ButtonExcelMerchantCardPinDebitInformation_Click);
            exportFucntions.Add(8, ButtonExcelMerchantMemos_Click);
        }
        return exportFucntions;
    }

    public void DoMultiExportExcel()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();
        exportNames.Add(0, GetLocalResourceObject("LiteralResource1.Text").ToString());
        exportNames.Add(1, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_BusinessInformation").ToString());

        if (IsRiskInformation )
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_Text_RiskInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_HierarchyInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_BankInformation").ToString());
            exportNames.Add(5, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_OtherCardInformation").ToString());
            exportNames.Add(6, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_PricingInformation").ToString());
            exportNames.Add(7, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_MerchantCardInformation").ToString());
            exportNames.Add(8, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_MerchantCardInformation").ToString() + " - " +
                GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_PINDebit").ToString());
            exportNames.Add(9, GetLocalResourceObject("MIF_MerchantDetails_IPMTASCX_Text_Memos").ToString());
        }
        else
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_HierarchyInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_BankInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_OtherCardInformation").ToString());
            exportNames.Add(5, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_PricingInformation").ToString());
            exportNames.Add(6, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_MerchantCardInformation").ToString());
            exportNames.Add(7, GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_MerchantCardInformation").ToString() + " - " +
                GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_PINDebit").ToString());
            exportNames.Add(8, GetLocalResourceObject("MIF_MerchantDetails_IPMTASCX_Text_Memos").ToString());
        }
        ExportSectionNames = exportNames;
    }

    public string ButtonExcelMerchantMemos_Click()
    {
        string result = this.Page.GetType().InvokeMember("ButtonExcelMerchantMemos_Click", System.Reflection.BindingFlags.InvokeMethod, null, this.Page, new object[] { }).ToString();
        return result;
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

    //protected void lnkLastBatch_Click(object sender, EventArgs e)
    //{
    //    if (string.IsNullOrEmpty(MerchantNumber))
    //    {
    //        if (Page.SecureQueryString["MerchantNumber"] != null)
    //        {
    //            Response.Redirect(GeneralFuncsLib.BuildBatchHistoryUrl(
    //               (SecurePage)Page, LastBatchDate, Page.SecureQueryString["MerchantNumber"]));
    //        }
    //    }
    //    else
    //    {
    //        Response.Redirect(GeneralFuncsLib.BuildBatchHistoryUrl(
    //                (SecurePage)Page, LastBatchDate, MerchantNumber));
    //    }
    //}

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
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_CorporateAddress]", Resources.Template.tpl_BusinessInformation_htm_CorporateAddress);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Fax]", Resources.Template.tpl_BusinessInformation_htm_Fax);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_OpenDate]", Resources.Template.tpl_BusinessInformation_htm_OpenDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ClosedDate]", Resources.Template.tpl_BusinessInformation_htm_ClosedDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Status]", Resources.Template.tpl_BusinessInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SiteAccess]", Resources.Template.tpl_BusinessInformation_htm_SiteAccess);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_TaxID]", Resources.Template.tpl_BusinessInformation_htm_TaxID);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SICMCC]", Resources.Template.tpl_BusinessInformation_htm_SICMCC);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_DepositType]", Resources.Template.tpl_BusinessInformation_htm_DepositType);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ETCType]", Resources.Template.tpl_BusinessInformation_htm_ETCType);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ETCCutoff]", Resources.Template.tpl_BusinessInformation_htm_ETCCutoff);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SeasonalMerchant]", Resources.Template.tpl_BusinessInformation_htm_SeasonalMerchant);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_StatementMailFlag]", Resources.Template.tpl_BusinessInformation_htm_StatementMailFlag);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_StatementICPrintOptions]", Resources.Template.tpl_BusinessInformation_htm_StatementICPrintOptions);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_OnlineDebitFeePrintOption]", Resources.Template.tpl_BusinessInformation_htm_OnlineDebitFeePrintOption);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_StatementHoldFlag]", Resources.Template.tpl_BusinessInformation_htm_StatementHoldFlag);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Misc1]", Resources.Template.tpl_BusinessInformation_htm_Misc1);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Misc2]", Resources.Template.tpl_BusinessInformation_htm_Misc2);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_TinType]", Resources.Template.tpl_BusinessInformation_htm_TinType);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Spark_Excl_ID]", Resources.Template.tpl_BusinessInformation_htm_Spark_Excl_ID);
        strExcelTemplate.Replace("BI_CORPORATE_NAME", "&nbsp;" + dataSource.Rows[0]["CorporateName"].ToString());
        strExcelTemplate.Replace("BI_CORPORATE_ADDRESS", "&nbsp;" + dataSource.Rows[0]["CorporateAddress"].ToString());
        strExcelTemplate.Replace("BI_FAX", "&nbsp;" + FormatPhone(dataSource.Rows[0]["Fax"]));
        strExcelTemplate.Replace("BI_OPEN_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["OpenDate"]));
        strExcelTemplate.Replace("BI_CLOSE_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["ClosedDate"]));
        strExcelTemplate.Replace("BI_STATUS", "&nbsp;" + dataSource.Rows[0]["Status"].ToString());
        strExcelTemplate.Replace("BI_SITE_ACCESS", "&nbsp;" + dataSource.Rows[0]["SiteAccess"]);
        strExcelTemplate.Replace("BI_TAX_ID", "&nbsp;" + dataSource.Rows[0]["PartialTaxID"].ToString());
        strExcelTemplate.Replace("BI_SIC_MCC", "&nbsp;" + FormatSIC(dataSource.Rows[0]["SICMCC"].ToString(), dataSource.Rows[0]["SICDescription"].ToString()));
        strExcelTemplate.Replace("BI_DEPOSIT_TYPE", "&nbsp;" + dataSource.Rows[0]["DepositType"].ToString());
        strExcelTemplate.Replace("BI_ETC_TYPE", "&nbsp;" + dataSource.Rows[0]["ETCType"].ToString());
        strExcelTemplate.Replace("BI_ETC_CUTOFF", "&nbsp;" + dataSource.Rows[0]["ETCCutoff"].ToString());
        strExcelTemplate.Replace("BI_SEASONAL_MERCHANT", "&nbsp;" + dataSource.Rows[0]["SeasonalMerchant"].ToString());
        strExcelTemplate.Replace("BI_STATEMENT_MAIL_FLAG", "&nbsp;" + dataSource.Rows[0]["StatementMailFlag"].ToString());
        strExcelTemplate.Replace("BI_STATEMENT_IC_PRINT_OPTIONS", "&nbsp;" + dataSource.Rows[0]["StatementICPrintOptions"].ToString());
        strExcelTemplate.Replace("BI_STATEMENT_ONLINE_DEBIT_FEE_PRINT_OPTION", "&nbsp;" + dataSource.Rows[0]["StatementOnlineDebitFeePrintOption"].ToString());
        strExcelTemplate.Replace("BI_STATEMENT_HOLD_FLAG", "&nbsp;" + dataSource.Rows[0]["StatementHoldFlag"].ToString());
        strExcelTemplate.Replace("BI_MISC_1", "&nbsp;" + dataSource.Rows[0]["Misc1"].ToString());
        strExcelTemplate.Replace("BI_MISC_2", "&nbsp;" + dataSource.Rows[0]["Misc2"].ToString());
        strExcelTemplate.Replace("BI_TIN_TYPE", "&nbsp;" + dataSource.Rows[0]["TinTypeDesc"].ToString());
        strExcelTemplate.Replace("BI_SPARK_EXCL_ID", "&nbsp;" + dataSource.Rows[0]["SparkExclID"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_BusinessInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelHierarchyInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_HIERARCHY_OMAHA_INFO)));

        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", Resources.Template.tpl_HierarchyInformation_htm_HierarchyInformation);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Sys]", Resources.Template.tpl_HierarchyInformation_htm_Sys);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Sys_Prin]", Resources.Template.tpl_HierarchyInformation_htm_Sys_Prin);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Sys_Prin_Agent]", Resources.Template.tpl_HierarchyInformation_htm_Sys_Prin_Agent);
        strExcelTemplate.Replace("[HI_SYS]", "&nbsp;" + _datasource.Rows[0]["Sys"].ToString());
        strExcelTemplate.Replace("[HI_SYSPRIN]", "&nbsp;" + _datasource.Rows[0]["SysPrin"].ToString());
        strExcelTemplate.Replace("[HI_SYSPRINAGENT]", "&nbsp;" + _datasource.Rows[0]["SysPrinAgent"].ToString());

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_HierarchyInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelBankInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_BANK_INFO)));

        var routingNumber = GetRoutingNumber(_datasource.Rows[0]["RoutingNumber"].ToString(), _datasource.Rows[0]["PartialRoutingNumber"].ToString());
        var dDANumber = CheckPermisson(_datasource.Rows[0]["DDANumber"], WebSiteConstants.SEC_PERMISSION_DDA);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankInformation]", Resources.Template.tpl_BankInformation_htm_BankInformation);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankName]", Resources.Template.tpl_BankInformation_htm_BankName);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_RoutingSharp]", Resources.Template.tpl_BankInformation_htm_RoutingSharp);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DDASharp]", Resources.Template.tpl_BankInformation_htm_DDASharp);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DaysHold]", Resources.Template.tpl_BankInformation_htm_DaysHold);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_ACHMonthly]", Resources.Template.tpl_BankInformation_htm_ACHMonthly);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DisbursementMethod_Deposit]", Resources.Template.tpl_BankInformation_htm_DisbursementMethod_Deposit);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DisbursementMethod_Adjustments]", Resources.Template.tpl_BankInformation_htm_DisbursementMethod_Adjustments);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DisbursementMethod_Discount]", Resources.Template.tpl_BankInformation_htm_DisbursementMethod_Discount);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DisbursementMethod_GenericDebit]", Resources.Template.tpl_BankInformation_htm_DisbursementMethod_GenericDebit);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DisbursementDetail]", Resources.Template.tpl_BankInformation_htm_DisbursementDetail);
        strExcelTemplate.Replace("BI_BANK_NAME", "&nbsp;" + _datasource.Rows[0]["BankName"].ToString());
        strExcelTemplate.Replace("BI_ROUTING_#", "&nbsp;" + routingNumber);
        strExcelTemplate.Replace("BI_DDA_#", "&nbsp;" + dDANumber);
        strExcelTemplate.Replace("BI_DAYS_HOLD", "&nbsp;" + _datasource.Rows[0]["DaysHold"].ToString());
        strExcelTemplate.Replace("BI_ACH_MONTHLY", "&nbsp;" + _datasource.Rows[0]["MonthlyACH"].ToString());
        strExcelTemplate.Replace("BI_DISBURSEMENT_METHOD_DEPOSIT", "&nbsp;" + _datasource.Rows[0]["DisbursementDeposit"].ToString());
        strExcelTemplate.Replace("BI_DISBURSEMENT_METHOD_ADJUSTMENTS", "&nbsp;" + _datasource.Rows[0]["DisbursementAdjustments"].ToString());
        strExcelTemplate.Replace("BI_DISBURSEMENT_METHOD_DISCOUNT", "&nbsp;" + _datasource.Rows[0]["DisbursementDiscount"].ToString());
        strExcelTemplate.Replace("BI_DISBURSEMENT_METHOD_GENERIC_DEBIT", "&nbsp;" + _datasource.Rows[0]["DisbursementGenericDebit"].ToString());
        strExcelTemplate.Replace("BI_DISBURSEMENT_DETAIL", "&nbsp;" + _datasource.Rows[0]["DisbursementDetail"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_BankInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelOtherCardInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_OTHER_CARD_INFO)));
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_OtherCardInformation]", Resources.Template.tpl_OtherCardInformation_htm_OtherCardInformation);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_AMEX]", Resources.Template.tpl_OtherCardInformation_htm_AMEX);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_PinDebit]", Resources.Template.tpl_OtherCardInformation_htm_PinDebit);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_AMEXONEPOINT]", Resources.Template.tpl_OtherCardInformation_htm_AMEXONEPOINT);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_JCB]", Resources.Template.tpl_OtherCardInformation_htm_JCB);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_Discover]", Resources.Template.tpl_OtherCardInformation_htm_Discover);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_WrightExpress]", Resources.Template.tpl_OtherCardInformation_htm_WrightExpress);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_DiscoverFullAQC]", Resources.Template.tpl_OtherCardInformation_htm_DiscoverFullAQC);
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_Voyager]", Resources.Template.tpl_OtherCardInformation_htm_Voyager);
        strExcelTemplate.Replace("OCI_AMEX_ONE_POINT", "&nbsp;" + _datasource.Rows[0]["AMEXONPOINT"].ToString());
        strExcelTemplate.Replace("OCI_AMEX", "&nbsp;" + _datasource.Rows[0]["AMEX"].ToString());
        strExcelTemplate.Replace("OCI_PIN_DEBIT", "&nbsp;" + _datasource.Rows[0]["PinDebit"].ToString());
        strExcelTemplate.Replace("OCI_JCB", "&nbsp;" + _datasource.Rows[0]["JCB"].ToString());
        strExcelTemplate.Replace("OCI_DISCOVER_FULL_AQC", "&nbsp;" + _datasource.Rows[0]["DiscoverFullAQC"].ToString());
        strExcelTemplate.Replace("OCI_DISCOVER", "&nbsp;" + _datasource.Rows[0]["Discover"].ToString());
        strExcelTemplate.Replace("OCI_WRIGHT_EXPRESS", "&nbsp;" + _datasource.Rows[0]["WrightExpress"].ToString());
        strExcelTemplate.Replace("OCI_VOYAGER", "&nbsp;" + _datasource.Rows[0]["Voyager"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_OtherCardInformation").ToString());
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
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_DailyDeposit]", Resources.Template.tpl_PricingInformation_htm_DailyDeposit);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_WeeklyDeposit]", Resources.Template.tpl_PricingInformation_htm_WeeklyDeposit);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_DailyAuthAmount]", Resources.Template.tpl_PricingInformation_htm_DailyAuthAmount);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_Avg_TicketAmount]", Resources.Template.tpl_PricingInformation_htm_Avg_TicketAmount);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AuthGrid]", Resources.Template.tpl_PricingInformation_htm_AuthGrid);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_UserDefinedGrid]", Resources.Template.tpl_PricingInformation_htm_UserDefinedGrid);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_IncomeFactors]", Resources.Template.tpl_PricingInformation_htm_IncomeFactors);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AccountFee1]", Resources.Template.tpl_PricingInformation_htm_AccountFee1);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AccountFee2]", Resources.Template.tpl_PricingInformation_htm_AccountFee2);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AccountFee3]", Resources.Template.tpl_PricingInformation_htm_AccountFee3);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AccountFee4]", Resources.Template.tpl_PricingInformation_htm_AccountFee4);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AccountFee5]", Resources.Template.tpl_PricingInformation_htm_AccountFee5);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RecurringFeeFlag]", Resources.Template.tpl_PricingInformation_htm_RecurringFeeFlag);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RecurFeeInd]", Resources.Template.tpl_PricingInformation_htm_RecurFeeInd);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RecurFeeAmt]", Resources.Template.tpl_PricingInformation_htm_RecurFeeAmt);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RecurFeeDesc]", Resources.Template.tpl_PricingInformation_htm_RecurFeeDesc);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_StmtBundleOption]", Resources.Template.tpl_PricingInformation_htm_StmtBundleOption);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_BundlePct]", Resources.Template.tpl_PricingInformation_htm_BundlePct);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_BundleRate]", Resources.Template.tpl_PricingInformation_htm_BundleRate);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_BatchHeaderFee]", Resources.Template.tpl_PricingInformation_htm_BatchHeaderFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ReturnTransactionFee]", Resources.Template.tpl_PricingInformation_htm_ReturnTransactionFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ChargebackFee]", Resources.Template.tpl_PricingInformation_htm_ChargebackFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RetrievalFee]", Resources.Template.tpl_PricingInformation_htm_RetrievalFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_SalesTransFee]", Resources.Template.tpl_PricingInformation_htm_SalesTransFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_OtherVolumePercent]", Resources.Template.tpl_PricingInformation_htm_OtherVolumePercent);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_OtherItemCharge]", Resources.Template.tpl_PricingInformation_htm_OtherItemCharge);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_WebsiteUsage]", Resources.Template.tpl_PricingInformation_htm_WebsiteUsage);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_IVRUsage]", Resources.Template.tpl_PricingInformation_htm_IVRUsage);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_UnregPct]", Resources.Template.tpl_PricingInformation_htm_UnregPct);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_UnregRate]", Resources.Template.tpl_PricingInformation_htm_UnregRate);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RegPct]", Resources.Template.tpl_PricingInformation_htm_RegPct);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RegRate]", Resources.Template.tpl_PricingInformation_htm_RegRate);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_VisaAssocFees]", Resources.Template.tpl_PricingInformation_htm_VisaAssocFees);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcqrPrFee]", Resources.Template.tpl_PricingInformation_htm_AcqrPrFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_MisuseAuthFee]", Resources.Template.tpl_PricingInformation_htm_MisuseAuthFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ISA]", Resources.Template.tpl_PricingInformation_htm_ISA);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ZeroFloor]", Resources.Template.tpl_PricingInformation_htm_ZeroFloor);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_IntlAcq]", Resources.Template.tpl_PricingInformation_htm_IntlAcq);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_MCAssocFees]", Resources.Template.tpl_PricingInformation_htm_MCAssocFees);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_NABUFee]", Resources.Template.tpl_PricingInformation_htm_NABUFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_CrossBorderFee]", Resources.Template.tpl_PricingInformation_htm_CrossBorderFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcqSupportFee]", Resources.Template.tpl_PricingInformation_htm_AcqSupportFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ReversalFee]", Resources.Template.tpl_PricingInformation_htm_ReversalFee);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_DiscFullAcqAssoc]", Resources.Template.tpl_PricingInformation_htm_DiscFullAcqAssoc);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_INTLProcessFlag]", Resources.Template.tpl_PricingInformation_htm_INTLProcessFlag);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_INTLServiceFlag]", Resources.Template.tpl_PricingInformation_htm_INTLServiceFlag);
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_DataUsageFlag]", Resources.Template.tpl_PricingInformation_htm_DataUsageFlag);

        strExcelTemplate.Replace("PI_DAILY_DEPOSIT", "&nbsp;" + _datasource.Rows[0]["DailyDeposit"].ToString());
        strExcelTemplate.Replace("PI_ACCOUNT_FEE1", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["AccountFee1"]));
        strExcelTemplate.Replace("PI_ACQR_PR_FEE", "&nbsp;" + _datasource.Rows[0]["AcqrPrFee"].ToString());
        strExcelTemplate.Replace("PI_NABU_FEE", "&nbsp;" + _datasource.Rows[0]["NABUFee"].ToString());
        strExcelTemplate.Replace("PI_INTL_PROCESS_FLAG", "&nbsp;" + _datasource.Rows[0]["INTLProcessFlag"].ToString());
        strExcelTemplate.Replace("PI_BATCH_HEADER_FEE", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["BatchHeaderFee"], 4));
        strExcelTemplate.Replace("PI_WEEKLY_DEPOSIT", "&nbsp;" + _datasource.Rows[0]["WeeklyDeposit"].ToString());
        strExcelTemplate.Replace("PI_ACCOUNT_FEE2", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["AccountFee2"]));
        strExcelTemplate.Replace("PI_MISUSE_AUTH_FEE", "&nbsp;" + _datasource.Rows[0]["MisuseAuthFee"].ToString());
        strExcelTemplate.Replace("PI_CROSS_BORDER_FEE", "&nbsp;" + _datasource.Rows[0]["CrossBorderFee"].ToString());
        strExcelTemplate.Replace("PI_INTL_SERVICE_FLAG", "&nbsp;" + _datasource.Rows[0]["INTLServiceFlag"].ToString());
        strExcelTemplate.Replace("PI_RETURN_TRANSACTION_FEE", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["ReturnTransFee"]));
        strExcelTemplate.Replace("PI_DAILY_AUTH_AMOUNT", "&nbsp;" + _datasource.Rows[0]["DailyAuthAmount"].ToString());
        strExcelTemplate.Replace("PI_ACCOUNT_FEE3", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["AccountFee3"]));
        strExcelTemplate.Replace("PI_ISA", "&nbsp;" + _datasource.Rows[0]["ISA"].ToString());
        strExcelTemplate.Replace("PI_ACQ_SUPPORT_FEE", "&nbsp;" + _datasource.Rows[0]["AcqSupportFee"].ToString());
        strExcelTemplate.Replace("PI_DATA_USAGE_FLAG", "&nbsp;" + _datasource.Rows[0]["DataUsageFlag"].ToString());
        strExcelTemplate.Replace("PI_CHARGEBACK_FEE", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["ChargebackFee"]));
        strExcelTemplate.Replace("PI_AVG_TICKET_AMOUNT", "&nbsp;" + _datasource.Rows[0]["AvgTicketAmount"].ToString());
        strExcelTemplate.Replace("PI_ACCOUNT_FEE4", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["AccountFee4"]));
        strExcelTemplate.Replace("PI_ZERO_FLOOR", "&nbsp;" + _datasource.Rows[0]["ZeroFloor"].ToString());
        strExcelTemplate.Replace("PI_RETRIEVAL_FEE", "&nbsp;" + _datasource.Rows[0]["RetrievalFee"].ToString());
        strExcelTemplate.Replace("PI_AUTH_GRID", "&nbsp;" + _datasource.Rows[0]["AuthGrid"].ToString());
        strExcelTemplate.Replace("PI_RECURRING_FEE_FLAG", "&nbsp;" + _datasource.Rows[0]["RecurFeeFlag"].ToString());
        strExcelTemplate.Replace("PI_INTL_ACQ", "&nbsp;" + _datasource.Rows[0]["IntlAcq"].ToString());
        strExcelTemplate.Replace("PI_SALES_TRANS_FEE", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["SaleTransFee"], 4));
        strExcelTemplate.Replace("PI_USER_DEFINED_GRID", "&nbsp;" + _datasource.Rows[0]["UserDefinedGrid"].ToString());
        strExcelTemplate.Replace("PI_RECUR_FEE_IND", "&nbsp;" + _datasource.Rows[0]["RecurFeeInd"].ToString());
        strExcelTemplate.Replace("PI_OTHER_VOLUME", "&nbsp;" + _datasource.Rows[0]["OtherVolumnPercent"].ToString());
        strExcelTemplate.Replace("PI_RECUR_FEE_AMT", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["RecurFeeAmt"]));
        strExcelTemplate.Replace("PI_OTHER_ITEM_CHARGE", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["OtherItemCharge"], 4));
        strExcelTemplate.Replace("PI_RECUR_FEE_DESC", "&nbsp;" + _datasource.Rows[0]["RecurFeeDesc"].ToString());
        strExcelTemplate.Replace("PI_WEBSITE_USAGE", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["WebsiteUsage"], 2));
        strExcelTemplate.Replace("PI_IVR_USAGE", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["IVRUsage"], 2));

        strExcelTemplate.Replace("PI_ACCOUNT_FEE5", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["AccountFee5"]));
        strExcelTemplate.Replace("PI_REVERSAL_FEE", "&nbsp;" + _datasource.Rows[0]["ReversalFee"].ToString());
        strExcelTemplate.Replace("PI_STMT_BUNDLE_OPTION", "&nbsp;" + _datasource.Rows[0]["StmtBundleOption"].ToString());
        strExcelTemplate.Replace("PI_BUNDLE_PCT", "&nbsp;" + _datasource.Rows[0]["BundlePct"].ToString());
        strExcelTemplate.Replace("PI_BUNDLE_RATE", "&nbsp;" + _datasource.Rows[0]["BundleRate"].ToString());
        strExcelTemplate.Replace("PI_UNREG_PCT", "&nbsp;" + _datasource.Rows[0]["UnregPct"].ToString());
        strExcelTemplate.Replace("PI_UNREG_RATE", "&nbsp;" + _datasource.Rows[0]["UnregRate"].ToString());
        strExcelTemplate.Replace("PI_REG_PCT", "&nbsp;" + _datasource.Rows[0]["RegPct"].ToString());
        strExcelTemplate.Replace("PI_REG_RATE", "&nbsp;" + _datasource.Rows[0]["RegRate"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_PricingInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantCardInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_MERCHANT_CARD_INFO)));
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_MerchantCardInformation]", Resources.Template.tpl_MerchantCardInformation_htm_MerchantCardInformation);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_MC]", Resources.Template.tpl_MerchantCardInformation_htm_MC);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_MCDB]", Resources.Template.tpl_MerchantCardInformation_htm_MCDB);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_VISA]", Resources.Template.tpl_MerchantCardInformation_htm_VISA);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_VISADB]", Resources.Template.tpl_MerchantCardInformation_htm_VISADB);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_DISCFULLACQ]", Resources.Template.tpl_MerchantCardInformation_htm_DISCFULLACQ);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_DISCFULLACQDB]", Resources.Template.tpl_MerchantCardInformation_htm_DISCFULLACQDB);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_DISCPASSTHRU]", Resources.Template.tpl_MerchantCardInformation_htm_DISCPASSTHRU);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_AMEXONEPT]", Resources.Template.tpl_MerchantCardInformation_htm_AMEXONEPT);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_AMEXPASSTHRU]", Resources.Template.tpl_MerchantCardInformation_htm_AMEXPASSTHRU);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_AMEX]", Resources.Template.tpl_MerchantCardInformation_htm_AMEX);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_WRIGHTEXPRESS]", Resources.Template.tpl_MerchantCardInformation_htm_WRIGHTEXPRESS);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_VOYAGER]", Resources.Template.tpl_MerchantCardInformation_htm_VOYAGER);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_GENERICDEBIT]", Resources.Template.tpl_MerchantCardInformation_htm_GENERICDEBIT);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_DINERS]", Resources.Template.tpl_MerchantCardInformation_htm_DINERS);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_EBTCASH_B]", Resources.Template.tpl_MerchantCardInformation_htm_EBTCASH_B);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_EBTF_STMP]", Resources.Template.tpl_MerchantCardInformation_htm_EBTF_STMP);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_EBT_TAPE]", Resources.Template.tpl_MerchantCardInformation_htm_EBT_TAPE);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_JCB]", Resources.Template.tpl_MerchantCardInformation_htm_JCB);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_ProcSW]", Resources.Template.tpl_MerchantCardInformation_htm_ProcSW);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_FeeClass]", Resources.Template.tpl_MerchantCardInformation_htm_FeeClass);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_QualRate]", Resources.Template.tpl_MerchantCardInformation_htm_QualRate);

        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Mid_QualRate]", Resources.Template.tpl_MerchantCardInformation_htm_Mid_QualRate);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Non_QualRate]", Resources.Template.tpl_MerchantCardInformation_htm_Non_QualRate);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_InterchangeFeeFlag]", Resources.Template.tpl_MerchantCardInformation_htm_InterchangeFeeFlag);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Dues_AssessmentFlag]", Resources.Template.tpl_MerchantCardInformation_htm_Dues_AssessmentFlag);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Dues_AssessmentFlagVol_st_1000]", Resources.Template.tpl_MerchantCardInformation_htm_Dues_AssessmentFlagVol_st_1000);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Dues_AssessmentFlagItem_st_1000]", Resources.Template.tpl_MerchantCardInformation_htm_Dues_AssessmentFlagItem_st_1000);

        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Dues_AssessmentFlagVol_lt_1000]", Resources.Template.tpl_MerchantCardInformation_htm_Dues_AssessmentFlagVol_lt_1000);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Dues_AssessmentFlagItem_lt_1000]", Resources.Template.tpl_MerchantCardInformation_htm_Dues_AssessmentFlagItem_lt_1000);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_MerchantPricingGrid]", Resources.Template.tpl_MerchantCardInformation_htm_MerchantPricingGrid);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_TieredDiscountGrid]", Resources.Template.tpl_MerchantCardInformation_htm_TieredDiscountGrid);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_ERRPercent]", Resources.Template.tpl_MerchantCardInformation_htm_ERRPercent);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_OtherVolumePercent]", Resources.Template.tpl_MerchantCardInformation_htm_OtherVolumePercent);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_OtherItemRate]", Resources.Template.tpl_MerchantCardInformation_htm_OtherItemRate);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_DiscountMethod]", Resources.Template.tpl_MerchantCardInformation_htm_DiscountMethod);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_AmexOnePTRate]", Resources.Template.tpl_MerchantCardInformation_htm_AmexOnePTRate);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_AmexOnePTPerItemFee]", Resources.Template.tpl_MerchantCardInformation_htm_AmexOnePTPerItemFee);

        strExcelTemplate.Replace("PROCESS_SWMCDB", "&nbsp;" + _datasource.Rows[0]["ProcessSWMCDB"].ToString());
        strExcelTemplate.Replace("PROCESS_SWMC", "&nbsp;" + _datasource.Rows[0]["ProcessSWMC"].ToString());
        strExcelTemplate.Replace("PROCESS_SWVSDB", "&nbsp;" + _datasource.Rows[0]["ProcessSWVSDB"].ToString());
        strExcelTemplate.Replace("PROCESS_SWVS", "&nbsp;" + _datasource.Rows[0]["ProcessSWVS"].ToString());
        strExcelTemplate.Replace("PROCESS_SWDIDB", "&nbsp;" + _datasource.Rows[0]["ProcessSWDIDB"].ToString());
        strExcelTemplate.Replace("PROCESS_SWDIPASSTHRU", "&nbsp;" + _datasource.Rows[0]["ProcessSWDIPassThru"].ToString());
        strExcelTemplate.Replace("PROCESS_SWDI", "&nbsp;" + _datasource.Rows[0]["ProcessSWDI"].ToString());
        strExcelTemplate.Replace("PROCESS_SWAMEXONEPT", "&nbsp;" + _datasource.Rows[0]["ProcessSWAmexOnePT"].ToString());
        strExcelTemplate.Replace("PROCESS_SWAMESPASSTHRU", "&nbsp;" + _datasource.Rows[0]["ProcessSWAmesPassThru"].ToString());
        strExcelTemplate.Replace("PROCESS_SWAMEXBLUE", "&nbsp;" + _datasource.Rows[0]["ProcessSWAmexBlue"].ToString());
        strExcelTemplate.Replace("PROCESS_SWWRIGHTEXPRESS", "&nbsp;" + _datasource.Rows[0]["ProcessSWWrightExpress"].ToString());
        strExcelTemplate.Replace("PROCESS_SWVOYAGER", "&nbsp;" + _datasource.Rows[0]["ProcessSWVoyager"].ToString());
        strExcelTemplate.Replace("PROCESS_SWGENERICDEBIT", "&nbsp;" + _datasource.Rows[0]["ProcessSWGenericDebit"].ToString());
        strExcelTemplate.Replace("PROCESS__SWDINNER", "&nbsp;" + _datasource.Rows[0]["ProcessSWDinner"].ToString());
        strExcelTemplate.Replace("PROCESS_SWEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["ProcessSWEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("PROCESS_SWEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["ProcessSWEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("PROCESS_SWEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["ProcessSWEBT_TAPE"].ToString());
        strExcelTemplate.Replace("PROCESS_SWJCB", "&nbsp;" + _datasource.Rows[0]["ProcessSWJCB"].ToString());
        strExcelTemplate.Replace("FeeClassMCDB", "&nbsp;" + _datasource.Rows[0]["FeeClassMCDB"].ToString());
        strExcelTemplate.Replace("FeeClassMC", "&nbsp;" + _datasource.Rows[0]["FeeClassMC"].ToString());
        strExcelTemplate.Replace("FeeClassVSDB", "&nbsp;" + _datasource.Rows[0]["FeeClassVSDB"].ToString());
        strExcelTemplate.Replace("FeeClassVS", "&nbsp;" + _datasource.Rows[0]["FeeClassVS"].ToString());
        strExcelTemplate.Replace("FeeClassDIDB", "&nbsp;" + _datasource.Rows[0]["FeeClassDIDB"].ToString());
        strExcelTemplate.Replace("FeeClassDIPassThru", "&nbsp;" + _datasource.Rows[0]["FeeClassDIPassThru"].ToString());
        strExcelTemplate.Replace("FeeClassDI", "&nbsp;" + _datasource.Rows[0]["FeeClassDI"].ToString());
        strExcelTemplate.Replace("FeeClassAmexOnePT", "&nbsp;" + _datasource.Rows[0]["FeeClassAmexOnePT"].ToString());
        strExcelTemplate.Replace("FeeClassAmesPassThru", "&nbsp;" + _datasource.Rows[0]["FeeClassAmesPassThru"].ToString());
        strExcelTemplate.Replace("FeeClassAmexBlue", "&nbsp;" + _datasource.Rows[0]["FeeClassAmexBlue"].ToString());
        strExcelTemplate.Replace("FeeClassWrightExpress", "&nbsp;" + _datasource.Rows[0]["FeeClassWrightExpress"].ToString());
        strExcelTemplate.Replace("FeeClassVoyager", "&nbsp;" + _datasource.Rows[0]["FeeClassVoyager"].ToString());
        strExcelTemplate.Replace("FeeClassGenericDebit", "&nbsp;" + _datasource.Rows[0]["FeeClassGenericDebit"].ToString());
        strExcelTemplate.Replace("FeeClassDinner", "&nbsp;" + _datasource.Rows[0]["FeeClassDinner"].ToString());
        strExcelTemplate.Replace("FeeClassEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["FeeClassEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("FeeClassEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["FeeClassEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("FeeClassEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["FeeClassEBT_TAPE"].ToString());
        strExcelTemplate.Replace("FeeClassJCB", "&nbsp;" + _datasource.Rows[0]["FeeClassJCB"].ToString());
        strExcelTemplate.Replace("MidQualRateMCDB", "&nbsp;" + _datasource.Rows[0]["MidQualRateMCDB"].ToString());
        strExcelTemplate.Replace("MidQualRateMC", "&nbsp;" + _datasource.Rows[0]["MidQualRateMC"].ToString());
        strExcelTemplate.Replace("MidQualRateVSDB", "&nbsp;" + _datasource.Rows[0]["MidQualRateVSDB"].ToString());
        strExcelTemplate.Replace("MidQualRateVS", "&nbsp;" + _datasource.Rows[0]["MidQualRateVS"].ToString());
        strExcelTemplate.Replace("MidQualRateDIDB", "&nbsp;" + _datasource.Rows[0]["MidQualRateDIDB"].ToString());
        strExcelTemplate.Replace("MidQualRateDIPassThru", "&nbsp;" + _datasource.Rows[0]["MidQualRateDIPassThru"].ToString());
        strExcelTemplate.Replace("MidQualRateDI", "&nbsp;" + _datasource.Rows[0]["MidQualRateDI"].ToString());
        strExcelTemplate.Replace("MidQualRateAmexOnePT", "&nbsp;" + _datasource.Rows[0]["MidQualRateAmexOnePT"].ToString());
        strExcelTemplate.Replace("MidQualRateAmesPassThru", "&nbsp;" + _datasource.Rows[0]["MidQualRateAmesPassThru"].ToString());
        strExcelTemplate.Replace("MidQualRateAmexBlue", "&nbsp;" + _datasource.Rows[0]["MidQualRateAmexBlue"].ToString());
        strExcelTemplate.Replace("MidQualRateWrightExpress", "&nbsp;" + _datasource.Rows[0]["MidQualRateWrightExpress"].ToString());
        strExcelTemplate.Replace("MidQualRateVoyager", "&nbsp;" + _datasource.Rows[0]["MidQualRateVoyager"].ToString());
        strExcelTemplate.Replace("MidQualRateGenericDebit", "&nbsp;" + _datasource.Rows[0]["MidQualRateGenericDebit"].ToString());
        strExcelTemplate.Replace("MidQualRateDinner", "&nbsp;" + _datasource.Rows[0]["MidQualRateDinner"].ToString());
        strExcelTemplate.Replace("MidQualRateEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["MidQualRateEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("MidQualRateEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["MidQualRateEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("MidQualRateEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["MidQualRateEBT_TAPE"].ToString());
        strExcelTemplate.Replace("MidQualRateJCB", "&nbsp;" + _datasource.Rows[0]["MidQualRateJCB"].ToString());
        strExcelTemplate.Replace("NonQualRateMCDB", "&nbsp;" + _datasource.Rows[0]["NonQualRateMCDB"].ToString());
        strExcelTemplate.Replace("NonQualRateMC", "&nbsp;" + _datasource.Rows[0]["NonQualRateMC"].ToString());
        strExcelTemplate.Replace("NonQualRateVSDB", "&nbsp;" + _datasource.Rows[0]["NonQualRateVSDB"].ToString());
        strExcelTemplate.Replace("NonQualRateVS", "&nbsp;" + _datasource.Rows[0]["NonQualRateVS"].ToString());
        strExcelTemplate.Replace("NonQualRateDIDB", "&nbsp;" + _datasource.Rows[0]["NonQualRateDIDB"].ToString());
        strExcelTemplate.Replace("NonQualRateDIPassThru", "&nbsp;" + _datasource.Rows[0]["NonQualRateDIPassThru"].ToString());
        strExcelTemplate.Replace("NonQualRateDI", "&nbsp;" + _datasource.Rows[0]["NonQualRateDI"].ToString());
        strExcelTemplate.Replace("NonQualRateAmexOnePT", "&nbsp;" + _datasource.Rows[0]["NonQualRateAmexOnePT"].ToString());
        strExcelTemplate.Replace("NonQualRateAmesPassThru", "&nbsp;" + _datasource.Rows[0]["NonQualRateAmesPassThru"].ToString());
        strExcelTemplate.Replace("NonQualRateAmexBlue", "&nbsp;" + _datasource.Rows[0]["NonQualRateAmexBlue"].ToString());
        strExcelTemplate.Replace("NonQualRateWrightExpress", "&nbsp;" + _datasource.Rows[0]["NonQualRateWrightExpress"].ToString());
        strExcelTemplate.Replace("NonQualRateVoyager", "&nbsp;" + _datasource.Rows[0]["NonQualRateVoyager"].ToString());
        strExcelTemplate.Replace("NonQualRateGenericDebit", "&nbsp;" + _datasource.Rows[0]["NonQualRateGenericDebit"].ToString());
        strExcelTemplate.Replace("NonQualRateDinner", "&nbsp;" + _datasource.Rows[0]["NonQualRateDinner"].ToString());
        strExcelTemplate.Replace("NonQualRateEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["NonQualRateEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("NonQualRateEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["NonQualRateEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("NonQualRateEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["NonQualRateEBT_TAPE"].ToString());
        strExcelTemplate.Replace("NonQualRateJCB", "&nbsp;" + _datasource.Rows[0]["NonQualRateJCB"].ToString());
        strExcelTemplate.Replace("QualRateMCDB", "&nbsp;" + _datasource.Rows[0]["QualRateMCDB"].ToString());
        strExcelTemplate.Replace("QualRateMC", "&nbsp;" + _datasource.Rows[0]["QualRateMC"].ToString());
        strExcelTemplate.Replace("QualRateVSDB", "&nbsp;" + _datasource.Rows[0]["QualRateVSDB"].ToString());
        strExcelTemplate.Replace("QualRateVS", "&nbsp;" + _datasource.Rows[0]["QualRateVS"].ToString());
        strExcelTemplate.Replace("QualRateDIDB", "&nbsp;" + _datasource.Rows[0]["QualRateDIDB"].ToString());
        strExcelTemplate.Replace("QualRateDIPassThru", "&nbsp;" + _datasource.Rows[0]["QualRateDIPassThru"].ToString());
        strExcelTemplate.Replace("QualRateDI", "&nbsp;" + _datasource.Rows[0]["QualRateDI"].ToString());
        strExcelTemplate.Replace("QualRateAmexOnePT", "&nbsp;" + _datasource.Rows[0]["QualRateAmexOnePT"].ToString());
        strExcelTemplate.Replace("QualRateAmesPassThru", "&nbsp;" + _datasource.Rows[0]["QualRateAmesPassThru"].ToString());
        strExcelTemplate.Replace("QualRateAmexBlue", "&nbsp;" + _datasource.Rows[0]["QualRateAmexBlue"].ToString());
        strExcelTemplate.Replace("QualRateWrightExpress", "&nbsp;" + _datasource.Rows[0]["QualRateWrightExpress"].ToString());
        strExcelTemplate.Replace("QualRateVoyager", "&nbsp;" + _datasource.Rows[0]["QualRateVoyager"].ToString());
        strExcelTemplate.Replace("QualRateGenericDebit", "&nbsp;" + _datasource.Rows[0]["QualRateGenericDebit"].ToString());
        strExcelTemplate.Replace("QualRateDinner", "&nbsp;" + _datasource.Rows[0]["QualRateDinner"].ToString());
        strExcelTemplate.Replace("QualRateEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["QualRateEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("QualRateEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["QualRateEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("QualRateEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["QualRateEBT_TAPE"].ToString());
        strExcelTemplate.Replace("QualRateJCB", "&nbsp;" + _datasource.Rows[0]["QualRateJCB"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagMCDB", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagMCDB"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagMC", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagMC"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagVSDB", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagVSDB"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagVS", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagVS"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagDIDB", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagDIDB"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagDIPassThru", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagDIPassThru"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagDI", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagDI"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagAmexOnePT", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagAmexOnePT"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagAmesPassThru", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagAmesPassThru"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagAmexBlue", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagAmexBlue"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagWrightExpress", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagWrightExpress"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagVoyager", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagVoyager"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagGenericDebit", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagGenericDebit"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagDinner", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagDinner"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagEBT_TAPE"].ToString());
        strExcelTemplate.Replace("DuesAssessmentFlagJCB", "&nbsp;" + _datasource.Rows[0]["DuesAssessmentFlagJCB"].ToString());

        strExcelTemplate.Replace("DueAsmtVolLower1MC", "&nbsp;" + _datasource.Rows[0]["DueAsmtVolLower1MC"].ToString());
        strExcelTemplate.Replace("DueAsmtItemLower1MC", "&nbsp;" + _datasource.Rows[0]["DueAsmtItemLower1MC"].ToString());
        strExcelTemplate.Replace("DueAsmtVolGreater1MC", "&nbsp;" + _datasource.Rows[0]["DueAsmtVolGreater1MC"].ToString());
        strExcelTemplate.Replace("DueAsmtItemGreater1MC", "&nbsp;" + _datasource.Rows[0]["DueAsmtItemGreater1MC"].ToString());

        strExcelTemplate.Replace("MerchantPricingGridMCDB", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridMCDB"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridMC", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridMC"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridVSDB", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridVSDB"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridVS", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridVS"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridDIDB", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridDIDB"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridDIPassThru", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridDIPassThru"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridDI", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridDI"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridAmexOnePT", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridAmexOnePT"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridAmesPassThru", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridAmesPassThru"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridAmexBlue", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridAmexBlue"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridWrightExpress", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridWrightExpress"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridVoyager", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridVoyager"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridGenericDebit", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridGenericDebit"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridDinner", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridDinner"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridEBT_TAPE"].ToString());
        strExcelTemplate.Replace("MerchantPricingGridJCB", "&nbsp;" + _datasource.Rows[0]["MerchantPricingGridJCB"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridMCDB", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridMCDB"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridMC", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridMC"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridVSDB", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridVSDB"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridVS", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridVS"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridDIDB", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridDIDB"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridDIPassThru", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridDIPassThru"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridDI", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridDI"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridAmexOnePT", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridAmexOnePT"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridAmesPassThru", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridAmesPassThru"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridAmexBlue", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridAmexBlue"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridWrightExpress", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridWrightExpress"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridVoyager", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridVoyager"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridGenericDebit", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridGenericDebit"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridDinner", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridDinner"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridEBT_TAPE"].ToString());
        strExcelTemplate.Replace("TieredDiscountGridJCB", "&nbsp;" + _datasource.Rows[0]["TieredDiscountGridJCB"].ToString());
        strExcelTemplate.Replace("ERRMCDB", "&nbsp;" + _datasource.Rows[0]["ERRMCDB"].ToString());
        strExcelTemplate.Replace("ERRMC", "&nbsp;" + _datasource.Rows[0]["ERRMC"].ToString());
        strExcelTemplate.Replace("ERRVSDB", "&nbsp;" + _datasource.Rows[0]["ERRVSDB"].ToString());
        strExcelTemplate.Replace("ERRVS", "&nbsp;" + _datasource.Rows[0]["ERRVS"].ToString());
        strExcelTemplate.Replace("ERRDIDB", "&nbsp;" + _datasource.Rows[0]["ERRDIDB"].ToString());
        strExcelTemplate.Replace("ERRDIPassThru", "&nbsp;" + _datasource.Rows[0]["ERRDIPassThru"].ToString());
        strExcelTemplate.Replace("ERRDI", "&nbsp;" + _datasource.Rows[0]["ERRDI"].ToString());
        strExcelTemplate.Replace("ERRAmexOnePT", "&nbsp;" + _datasource.Rows[0]["ERRAmexOnePT"].ToString());
        strExcelTemplate.Replace("ERRAmesPassThru", "&nbsp;" + _datasource.Rows[0]["ERRAmesPassThru"].ToString());
        strExcelTemplate.Replace("ERRAmexBlue", "&nbsp;" + _datasource.Rows[0]["ERRAmexBlue"].ToString());
        strExcelTemplate.Replace("ERRWrightExpress", "&nbsp;" + _datasource.Rows[0]["ERRWrightExpress"].ToString());
        strExcelTemplate.Replace("ERRVoyager", "&nbsp;" + _datasource.Rows[0]["ERRVoyager"].ToString());
        strExcelTemplate.Replace("ERRGenericDebit", "&nbsp;" + _datasource.Rows[0]["ERRGenericDebit"].ToString());
        strExcelTemplate.Replace("ERRDinner", "&nbsp;" + _datasource.Rows[0]["ERRDinner"].ToString());
        strExcelTemplate.Replace("ERREBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["ERREBT_CASH_B"].ToString());
        strExcelTemplate.Replace("ERREBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["ERREBT_F_STMP"].ToString());
        strExcelTemplate.Replace("ERREBT_TAPE", "&nbsp;" + _datasource.Rows[0]["ERREBT_TAPE"].ToString());
        strExcelTemplate.Replace("ERRJCB", "&nbsp;" + _datasource.Rows[0]["ERRJCB"].ToString());
        strExcelTemplate.Replace("OtherVolumeMCDB", "&nbsp;" + _datasource.Rows[0]["OtherVolumeMCDB"].ToString());
        strExcelTemplate.Replace("OtherVolumeMC", "&nbsp;" + _datasource.Rows[0]["OtherVolumeMC"].ToString());
        strExcelTemplate.Replace("OtherVolumeVSDB", "&nbsp;" + _datasource.Rows[0]["OtherVolumeVSDB"].ToString());
        strExcelTemplate.Replace("OtherVolumeVS", "&nbsp;" + _datasource.Rows[0]["OtherVolumeVS"].ToString());
        strExcelTemplate.Replace("OtherVolumeDIDB", "&nbsp;" + _datasource.Rows[0]["OtherVolumeDIDB"].ToString());
        strExcelTemplate.Replace("OtherVolumeDIPassThru", "&nbsp;" + _datasource.Rows[0]["OtherVolumeDIPassThru"].ToString());
        strExcelTemplate.Replace("OtherVolumeDI", "&nbsp;" + _datasource.Rows[0]["OtherVolumeDI"].ToString());
        strExcelTemplate.Replace("OtherVolumeAmexOnePT", "&nbsp;" + _datasource.Rows[0]["OtherVolumeAmexOnePT"].ToString());
        strExcelTemplate.Replace("OtherVolumeAmesPassThru", "&nbsp;" + _datasource.Rows[0]["OtherVolumeAmesPassThru"].ToString());
        strExcelTemplate.Replace("OtherVolumeAmexBlue", "&nbsp;" + _datasource.Rows[0]["OtherVolumeAmexBlue"].ToString());
        strExcelTemplate.Replace("OtherVolumeWrightExpress", "&nbsp;" + _datasource.Rows[0]["OtherVolumeWrightExpress"].ToString());
        strExcelTemplate.Replace("OtherVolumeVoyager", "&nbsp;" + _datasource.Rows[0]["OtherVolumeVoyager"].ToString());
        strExcelTemplate.Replace("OtherVolumeGenericDebit", "&nbsp;" + _datasource.Rows[0]["OtherVolumeGenericDebit"].ToString());
        strExcelTemplate.Replace("OtherVolumeDinner", "&nbsp;" + _datasource.Rows[0]["OtherVolumeDinner"].ToString());
        strExcelTemplate.Replace("OtherVolumeEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["OtherVolumeEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("OtherVolumeEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["OtherVolumeEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("OtherVolumeEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["OtherVolumeEBT_TAPE"].ToString());
        strExcelTemplate.Replace("OtherVolumeJCB", "&nbsp;" + _datasource.Rows[0]["OtherVolumeJCB"].ToString());
        strExcelTemplate.Replace("OtherItemRateMCDB", "&nbsp;" + _datasource.Rows[0]["OtherItemRateMCDB"].ToString());
        strExcelTemplate.Replace("OtherItemRateMC", "&nbsp;" + _datasource.Rows[0]["OtherItemRateMC"].ToString());
        strExcelTemplate.Replace("OtherItemRateVSDB", "&nbsp;" + _datasource.Rows[0]["OtherItemRateVSDB"].ToString());
        strExcelTemplate.Replace("OtherItemRateVS", "&nbsp;" + _datasource.Rows[0]["OtherItemRateVS"].ToString());
        strExcelTemplate.Replace("OtherItemRateDIDB", "&nbsp;" + _datasource.Rows[0]["OtherItemRateDIDB"].ToString());
        strExcelTemplate.Replace("OtherItemRateDIPassThru", "&nbsp;" + _datasource.Rows[0]["OtherItemRateDIPassThru"].ToString());
        strExcelTemplate.Replace("OtherItemRateDI", "&nbsp;" + _datasource.Rows[0]["OtherItemRateDI"].ToString());
        strExcelTemplate.Replace("OtherItemRateAmexOnePT", "&nbsp;" + _datasource.Rows[0]["OtherItemRateAmexOnePT"].ToString());
        strExcelTemplate.Replace("OtherItemRateAmesPassThru", "&nbsp;" + _datasource.Rows[0]["OtherItemRateAmesPassThru"].ToString());
        strExcelTemplate.Replace("OtherItemRateAmexBlue", "&nbsp;" + _datasource.Rows[0]["OtherItemRateAmexBlue"].ToString());
        strExcelTemplate.Replace("OtherItemRateWrightExpress", "&nbsp;" + _datasource.Rows[0]["OtherItemRateWrightExpress"].ToString());
        strExcelTemplate.Replace("OtherItemRateVoyager", "&nbsp;" + _datasource.Rows[0]["OtherItemRateVoyager"].ToString());
        strExcelTemplate.Replace("OtherItemRateGenericDebit", "&nbsp;" + _datasource.Rows[0]["OtherItemRateGenericDebit"].ToString());
        strExcelTemplate.Replace("OtherItemRateDinner", "&nbsp;" + _datasource.Rows[0]["OtherItemRateDinner"].ToString());
        strExcelTemplate.Replace("OtherItemRateEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["OtherItemRateEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("OtherItemRateEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["OtherItemRateEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("OtherItemRateEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["OtherItemRateEBT_TAPE"].ToString());
        strExcelTemplate.Replace("OtherItemRateJCB", "&nbsp;" + _datasource.Rows[0]["OtherItemRateJCB"].ToString());
        strExcelTemplate.Replace("DiscountMethodMCDB", "&nbsp;" + _datasource.Rows[0]["DiscountMethodMCDB"].ToString());
        strExcelTemplate.Replace("DiscountMethodMC", "&nbsp;" + _datasource.Rows[0]["DiscountMethodMC"].ToString());
        strExcelTemplate.Replace("DiscountMethodVSDB", "&nbsp;" + _datasource.Rows[0]["DiscountMethodVSDB"].ToString());
        strExcelTemplate.Replace("DiscountMethodVS", "&nbsp;" + _datasource.Rows[0]["DiscountMethodVS"].ToString());
        strExcelTemplate.Replace("DiscountMethodDIDB", "&nbsp;" + _datasource.Rows[0]["DiscountMethodDIDB"].ToString());
        strExcelTemplate.Replace("DiscountMethodDIPassThru", "&nbsp;" + _datasource.Rows[0]["DiscountMethodDIPassThru"].ToString());
        strExcelTemplate.Replace("DiscountMethodDI", "&nbsp;" + _datasource.Rows[0]["DiscountMethodDI"].ToString());
        strExcelTemplate.Replace("DiscountMethodAmexOnePT", "&nbsp;" + _datasource.Rows[0]["DiscountMethodAmexOnePT"].ToString());
        strExcelTemplate.Replace("DiscountMethodAmesPassThru", "&nbsp;" + _datasource.Rows[0]["DiscountMethodAmesPassThru"].ToString());
        strExcelTemplate.Replace("DiscountMethodAmexBlue", "&nbsp;" + _datasource.Rows[0]["DiscountMethodAmexBlue"].ToString());
        strExcelTemplate.Replace("DiscountMethodWrightExpress", "&nbsp;" + _datasource.Rows[0]["DiscountMethodWrightExpress"].ToString());
        strExcelTemplate.Replace("DiscountMethodVoyager", "&nbsp;" + _datasource.Rows[0]["DiscountMethodVoyager"].ToString());
        strExcelTemplate.Replace("DiscountMethodGenericDebit", "&nbsp;" + _datasource.Rows[0]["DiscountMethodGenericDebit"].ToString());
        strExcelTemplate.Replace("DiscountMethodDinner", "&nbsp;" + _datasource.Rows[0]["DiscountMethodDinner"].ToString());
        strExcelTemplate.Replace("DiscountMethodEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["DiscountMethodEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("DiscountMethodEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["DiscountMethodEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("DiscountMethodEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["DiscountMethodEBT_TAPE"].ToString());
        strExcelTemplate.Replace("DiscountMethodJCB", "&nbsp;" + _datasource.Rows[0]["DiscountMethodJCB"].ToString());
        strExcelTemplate.Replace("AmexOnePTRateAmexOnePT", "&nbsp;" + _datasource.Rows[0]["AmexOnePTRateAmexOnePT"].ToString());
        strExcelTemplate.Replace("AmexOnePTPerItemFeeAmexOnePT", "&nbsp;" + _datasource.Rows[0]["AmexOnePTPerItemFeeAmexOnePT"].ToString());

        //Add more
        strExcelTemplate.Replace("InterchangeFeeFlagMC", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagMC"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlag_MCDB", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagMCDB"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagVS", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagVS"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlag_VSDB", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagVSDB"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagDI", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagDI"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlag_DIDB", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagDIDB"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlag_DIPassThru", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagDIPassThru"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagAmexOnePT", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagAmexOnePT"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagAmesPassThru", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagAmesPassThru"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagAmexBlue", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagAmexBlue"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagWrightExpress", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagWrightExpress"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagVoyager", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagVoyager"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagGenericDebit", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagGenericDebit"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagDinner", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagDinner"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagEBT_CASH_B", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagEBT_CASH_B"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagEBT_F_STMP", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagEBT_F_STMP"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagEBT_TAPE", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagEBT_TAPE"].ToString());
        strExcelTemplate.Replace("InterchangeFeeFlagJCB", "&nbsp;" + _datasource.Rows[0]["InterchangeFeeFlagJCB"].ToString());

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_MerchantCardInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantCardPinDebitInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_MER_CARD_PIN_DEBIT_INFO)));
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_MCI_PINDebit]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_MCI_PINDebit);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_ACCEL]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_ACCEL);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_AFFN]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_AFFN);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_ALASKAOPTION]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_ALASKAOPTION);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_CU24]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_CU24);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_INTERLINK]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_INTERLINK);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_JEANIE]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_JEANIE);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_MAESTRO]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_MAESTRO);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_NYCE]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_NYCE);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_PULSE]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_PULSE);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_SHAZAM]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_SHAZAM);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_STAR]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_STAR);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_OtherVolumePercent]", Resources.Template.tpl_MerchantCardInformation_htm_OtherVolumePercent);
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_OtherItemRate]", Resources.Template.tpl_MerchantCardInformation_htm_OtherItemRate);
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_OnlineDebitFeeFlag]", Resources.Template.tpl_MerchantCardPinDebitInformation_htm_OnlineDebitFeeFlag);

        strExcelTemplate.Replace("OtherVolumeACCEL", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["OtherVolumeACCEL"]));
        strExcelTemplate.Replace("OtherVolumeAFFN", "&nbsp;" + _datasource.Rows[0]["OtherVolumeAFFN"].ToString());
        strExcelTemplate.Replace("OtherVolumeAlaskaOption", "&nbsp;" + _datasource.Rows[0]["OtherVolumeAlaskaOption"].ToString());
        strExcelTemplate.Replace("OtherVolumeCU24", "&nbsp;" + _datasource.Rows[0]["OtherVolumeCU24"].ToString());
        strExcelTemplate.Replace("OtherVolumeInterlink", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["OtherVolumeInterlink"]));
        strExcelTemplate.Replace("OtherVolumeJeanie", "&nbsp;" + _datasource.Rows[0]["OtherVolumeJeanie"].ToString());
        strExcelTemplate.Replace("OtherVolumeMaestro", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["OtherVolumeJeanie"]));
        strExcelTemplate.Replace("OtherVolumeNYCE", "&nbsp;" + _datasource.Rows[0]["OtherVolumeNYCE"].ToString());
        strExcelTemplate.Replace("OtherVolumePULSE", "&nbsp;" + _datasource.Rows[0]["OtherVolumePULSE"].ToString());
        strExcelTemplate.Replace("OtherVolumeShazam", "&nbsp;" + _datasource.Rows[0]["OtherVolumeShazam"].ToString());
        strExcelTemplate.Replace("OtherVolumeStar13", "&nbsp;" + _datasource.Rows[0]["OtherVolumeStar13"].ToString());
        strExcelTemplate.Replace("OtherVolumeStar18", "&nbsp;" + _datasource.Rows[0]["OtherVolumeStar18"].ToString());
        strExcelTemplate.Replace("OtherVolumeStar21", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["OtherVolumeStar21"], 4));
        strExcelTemplate.Replace("OtherItemRateACCEL", "&nbsp;" + _datasource.Rows[0]["OtherItemRateACCEL"].ToString());
        strExcelTemplate.Replace("OtherItemRateAFFN", "&nbsp;" + _datasource.Rows[0]["OtherItemRateAFFN"].ToString());
        strExcelTemplate.Replace("OtherItemRateAlaskaOption", "&nbsp;" + _datasource.Rows[0]["OtherItemRateAlaskaOption"].ToString());
        strExcelTemplate.Replace("OtherItemRateCU24", "&nbsp;" + FormatCurrency(_datasource.Rows[0]["OtherItemRateCU24"]));
        strExcelTemplate.Replace("OtherItemRateInterlink", "&nbsp;" + _datasource.Rows[0]["OtherItemRateInterlink"].ToString());
        strExcelTemplate.Replace("OtherItemRateJeanie", "&nbsp;" + _datasource.Rows[0]["OtherItemRateJeanie"].ToString());
        strExcelTemplate.Replace("OtherItemRateMaestro", "&nbsp;" + _datasource.Rows[0]["OtherItemRateMaestro"].ToString());
        strExcelTemplate.Replace("OtherItemRateNYCE", "&nbsp;" + _datasource.Rows[0]["OtherItemRateNYCE"].ToString());
        strExcelTemplate.Replace("OtherItemRatePULSE", "&nbsp;" + _datasource.Rows[0]["OtherItemRatePULSE"].ToString());
        strExcelTemplate.Replace("OtherItemRateShazam", "&nbsp;" + _datasource.Rows[0]["OtherItemRateShazam"].ToString());
        strExcelTemplate.Replace("OtherItemRateStar13", "&nbsp;" + _datasource.Rows[0]["OtherItemRateStar13"].ToString());
        strExcelTemplate.Replace("OtherItemRateStar18", "&nbsp;" + _datasource.Rows[0]["OtherItemRateStar18"].ToString());
        strExcelTemplate.Replace("OtherItemRateStar21", "&nbsp;" + _datasource.Rows[0]["OtherItemRateStar21"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagACCEL", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagACCEL"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagAFFN", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagAFFN"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagAlaskaOpt", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagAlaskaOpt"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagCU24", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagCU24"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagInterlink", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagInterlink"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagJeanie", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagJeanie"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagMaestro", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagMaestro"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagNYCE", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagNYCE"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagPULSE", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagPULSE"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagShazam", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagShazam"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagStar13", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagStar13"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagStar18", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagStar18"].ToString());
        strExcelTemplate.Replace("OnlDbtFeeFlagStar21", "&nbsp;" + _datasource.Rows[0]["OnlDbtFeeFlagStar21"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_MerchantCardInformation").ToString() +
            GetLocalResourceObject("MIF_MerchantDetails_IPMTCS_Text_PINDebit").ToString());
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
        return GeneralFuncsLib.HasOptInOutPermission((SecurePage)Page)
            && MerchantProfileHelper.SiteAccessIsOptInOut(obj);
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
        return ReportPage.ReportFilter.CurrentValue.Value;
    }

    #endregion Private Methods

    #endregion Methods
}