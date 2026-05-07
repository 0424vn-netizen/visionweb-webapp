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

public partial class UserControls_MIF_MechantDetails_Cayan : ExportMultiSections
{

    #region Propertise using for Case Management
    private DataTable _MerchantInfor;
    public string MechantMemoExport;
    
    #endregion

    #region Enums

    protected enum DataBindAction
    {
        BindMerchantDetails,
        UpdateMerchantStatus
    }

    enum PostBackAction
    {
        UpdateEmail,
    }

    #endregion Enums

    #region Constants

    private string _merchantCardInformationPinDebitStr = string.Empty;
    private int _countRowForPinDebit;
    private string _merchantCardInformationStr = string.Empty;
    private int _countRowForMerchantCart;

    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string FILE_NAME_MER_CARD_PIN_DEBIT_INFO =
        "~/App_Data/tpl_MerchantCardPinDebitInformation_CAYAN.htm";

    private const string FILE_NAME_MER_CARD_PIN_DEBIT_PART =
       "~/App_Data/tpl_MerchantCardPinDebitInformation_Part_CAYAN.htm";

    private const string FILE_NAME_MERCHANT_CARD_INFO =
        "~/App_Data/tpl_MerchantCardInformation_CAYAN.htm";
    private const string FILE_NAME_MERCHANT_CARD_PART =
        "~/App_Data/tpl_MerchantCardInformation_Part_CAYAN.htm";

    private const string FILE_NAME_MERCHANT_CARD_PRIVATELABEL_INFO =
       "~/App_Data/tpl_MerchantCardInformation_PrivateLabel_CAYAN.htm";

    private const string FILE_NAME_MERCHANT_CARD_PRIVATELABEL_PART =
        "~/App_Data/tpl_MerchantCardInformation_PrivateLabel_Part_CAYAN.htm";

    private const string EMPTY_VALUE = "&nbsp;";

    private const string FILE_NAME_PRICING_INFO = "~/App_Data/tpl_PricingInformation_CAYAN.htm";
    private const string FILE_NAME_OTHER_CARD_INFO = "~/App_Data/tpl_OtherCardInformation_CAYAN.htm";
    private const string FILE_NAME_BANK_INFO = "~/App_Data/tpl_BankInformation.htm";
    private const string FILE_NAME_HIERARCHY_INFO = "~/App_Data/tpl_HierarchyInformation.htm";
    private const string FILE_NAME_BUSINESS_INFO = "~/App_Data/tpl_BusinessInformation.htm";
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation_CAYAN.htm";
    private const string FILE_NAME_MERCHANT_INFO_REP = "~/App_Data/tpl_MerchantInformation_CAYAN.htm";
    private const string FILE_NAME_TRANSACTION_ESTIMATES = "~/App_Data/tpl_Transaction_Estimates_CAYAN.htm";


    private const string OPTED_IN = "Opted In";

    // SPA Name
    private const string SPA_GET_MERCHANT_PROFILE = "spa_cs_GetMerchantProfile_FDR";
    private const string SPA_UPDATE_CUSTOM_CHAIN_REPORT = "spa_ms_UpdateCustomChainReport";

    private const string SALES_AGENT = "SalesAgent";
    private const string SYS_PRIN_AGENT = "SysPrinAgent";
    private const string HEAD_QUARTER = "Headquarter";

    private const string DELIMITER = ",";


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

    protected bool CheckMerchantCardInformation(string card)
    {
        bool isAvailable = _merchantCardInformationStr.Contains(DELIMITER + card + DELIMITER);
        if (isAvailable)
            _countRowForMerchantCart++;
        return isAvailable;
    }

    protected bool CheckMerchantCardInformationPinDebit(string card)
    {
        bool isAvailable = _merchantCardInformationPinDebitStr.Contains(DELIMITER + card + DELIMITER);
        if (isAvailable)
            _countRowForPinDebit++;
        return isAvailable;
    }
    private string Table2String(DataTable data)
    {
        var str = new StringBuilder(DELIMITER);
        foreach (DataRow item in data.Rows)
        {
            str.Append(item["CardType"].ToString() + DELIMITER);
        }
        return str.ToString();
    }

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
        if (!IsPostBack)
        {
            if(GeneralFuncsLib.IsUserSignOn()){
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

    public void ShowHideReportAccess(bool showReport)
    {
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "HideReportAccess", "$(document).ready(function() { ShowHideReportAccess('" + showReport.ToString() + "');});", true);
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
                parameters.Add(new FilterParameter("@MerchantNumber", GetMerchantNrParameter(), DbType.AnsiString));
                parameters.Add(new FilterParameter("@FlagPermission", SessionManager.CurrentUserViewMode, DbType.Int32));
                parameters.AddLanguageID();
                parameters.AddDecryptDataParams("RoutingNumber,DDANumber,TaxID");
                string spaName = "spa_cs_GetMerchantProfile_CAYAN_OMAHA";
                DataTable dtMerch = WebServices.CsReportServices.GetReports(spaName, parameters);

                if (dtMerch.Rows.Count > 0)
                {

                    var parames = new FilterParameterCollection();
                    parames.AddLoggedInUserReportingParams()
                           .Add("@MerchantNumber", GetMerchantNrParameter(), System.Data.DbType.String);
                    DataTable merchCardDataSource = WebServices.CsReportServices.GetReports("spa_cs_GetListCardTypeCodeByMerchant", parames);
                    DataTable merchCardPinDebitDataSource = WebServices.CsReportServices.GetReports("spa_cs_GetListCardTypeCodeByMerchant_PinDebit", parames);

                    if (merchCardDataSource == null | merchCardDataSource.Rows.Count == 0)
                    {
                        uxMerchantCardInformationNoRecords.Visible = true;
                        uxMerchantCardInformation.Visible = false;
                    }
                    else
                    {
                        uxMerchantCardInformationNoRecords.Visible = false;
                        _merchantCardInformationStr = Table2String(merchCardDataSource);
                    }

                    if (merchCardPinDebitDataSource == null | merchCardPinDebitDataSource.Rows.Count == 0)
                    {
                        uxMerchantCardPinDebitInformationNorecords.Visible = true;
                    }
                    else
                    {
                        uxMerchantCardPinDebitInformationNorecords.Visible = false;
                        _merchantCardInformationPinDebitStr = Table2String(merchCardPinDebitDataSource);
                    }

                    _MifTable = dtMerch;
                    this.OptedIn = dtMerch.Rows[0]["SiteAccess"].ToString().Equals(Resources.Template.OptedIn, StringComparison.OrdinalIgnoreCase) ? "1" : "0";
                    this.MifEmail = dtMerch.Rows[0]["Email"].ToString();


                     uxBusinessInformationCayan.DataBind();
                    uxBankInformation.DataBind();
                    uxOtherCardInformation.DataSource = _MifTable;
                    uxOtherCardInformation.DataBind();
                    uxMerchantCardPinDebitInformation.DataSource = _MifTable;
                    uxMerchantCardPinDebitInformation.DataBind();
                    uxHierarchyInformation.DataSource = _MifTable;
                    uxHierarchyInformation.DataBind();
                    uxTransactionEstGrid.DataSource = _MifTable;
                    uxTransactionEstGrid.DataBind();
                    uxMerchantCardInformation.DataSource = _MifTable;
                    uxMerchantCardInformation.DataBind();
                    BindMerchantInfoPrivateLabel();
                    uxPricingInformation.DataSource = _MifTable;
                    uxPricingInformation.DataBind();

                    phdMerchantDetail.Visible = true;
                    if (string.IsNullOrEmpty(MerchantNumber)
                        || (ReportPage.ReportFilter != null
                            && !MerchantNumber.Equals(ReportPage.ReportFilter.CurrentValue.Value)))
                    {


                        DateTime tempDate = new DateTime(1900, 1, 1);
                        DateTime.TryParse(FormatDate(dtMerch.Rows[0]["LastBactchActivity"]), out tempDate);
                        LastBatchDate = tempDate.Year == 1900 ? "0" : tempDate.Ticks.ToString();
                        MerchantNumber = ReportPage.ReportFilter.CurrentValue.Value;
                    }
                    else
                    {
                        //CustomReport = chkCustomReport.Checked;
                        // RecipientEmail = CustomReport ? uxEmailRecipents.Text.Trim() : string.Empty;
                    }
                    // uxEmailRecipents.Enabled = chkCustomReport.Checked = (bool)CustomReport;
                    // uxEmailRecipents.Text = RecipientEmail;

                    this._MerchantInfor = dtMerch;

                    if (BindValue("LastBactchActivity").IsNullOrEmpty())
                    {
                        lnkLastBatch.Visible = false;
                    }
                    else
                    {
                        lnkLastBatch.Text = FormatDate(BindValue("LastBactchActivity"));
                    }
                }
                else
                {
                    phdMerchantDetail.Visible = false;
                    // uxCustomReport.Visible = false;
                    // Visibility of Report Access based on uxCustomReport panel 
                }
                break;
        }
    }

    private void BindMerchantInfoPrivateLabel()
    {
        var parames = new FilterParameterCollection();
        parames.Add(new FilterParameter("@MerchantNumber", GetMerchantNrParameter(), DbType.String));
        parames.AddLoggedInUserReportingParams();
        DataTable data = WebServices.CsReportServices.GetReports("spa_cs_GetMerchantCardInformation_PrivateLabel", parames);
        rptPrivateLabel.DataSource = data;
        rptPrivateLabel.DataBind();
        plhPrivateLabelNoRecords.Visible = (data.Rows.Count == 0);
    }

    protected void GetDataSource(AS.Controls.KeyValueTable sender)
    {
        DataTable dt = _MifTable;
        sender.DataSource = dt;
    }


    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.UpdateEmail:

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


    #region Format Methods
    private object ProcessNullValue(object obj)
    {
        return obj.GetType() == typeof(DBNull) ? null : obj;
    }
    /// <summary>
    /// CR: “MC ICA AVS Income” and “MC ICA AVS Expense” keep 6 decimals after character "."
    /// And remove character "$".
    /// </summary>
    protected string Format_MC_ICA_AVS(object abc)
    {
        if (abc == DBNull.Value)
            return string.Empty;
        else
        {
            var result = string.Format("{0:#,0.000000;#,0.000000}", abc);
            var temp = Convert.ToDecimal(abc);
            if (temp < 0)
                result = string.Format("<span class=\"negative\">({0})</span>", result.Replace("-", ""));
            return result;
        }
    }
    protected string FormatDate(object dt, bool isFormatReOpenedDate = false)
    {
        dt = ProcessNullValue(dt);
        if (dt.IsNullOrEmpty())
            return string.Empty;
        if (isFormatReOpenedDate)
        {
            return Convert.ToDateTime(dt).ToString("MM/dd/yy");
        }
        else
        {
            return Convert.ToDateTime(dt).ToString("MM/dd/yyyy");
        }
    }
    protected string FormatPhone(object phone)
    {
        phone = ProcessNullValue(phone);
        if (phone == null)
            return string.Empty;
        return FormatData.FormatPhoneNumber(phone.ToString());
    }
    protected string FormatCurrency(object value)
    {
        if (value.IsNullOrEmpty())
            return string.Empty;
        else return FormatData.FormatCurrency(value, SessionManager.CurrencyFortmat);
    }
    protected string FormatCurrency(object value, int count)
    {
        if (value.IsNullOrEmpty())
            return string.Empty;
        else return FormatData.FormatCurrency(value, count, SessionManager.CurrencyFortmat);
    }
    protected string FormatPercent(object value)
    {
        if (value.IsNullOrEmpty())
            return string.Empty;
        return FormatData.FormatPercent(value);
    }
    protected string FormatInteger(object value)
    {
        if (value.IsNullOrEmpty())
            return string.Empty;
        return FormatData.FormatInteger(value);
    }
    #endregion




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
         uxBusinessInformationCayan.DataBind();
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
        exportFucntions.Add(1, uxExportBankInformation.DoExport);
        exportFucntions.Add(2, uxExportBusinessInformation.DoExport);
        exportFucntions.Add(3, ButtonExcelTransactionEstimates_Click);
        if (IsRiskInformation)
        {
            exportFucntions.Add(4, () => { return uxRiskInfo.ExcelRiskInformation(); });
        }
        exportFucntions.Add(5, ButtonExcelHierarchyInformation_Click);
        exportFucntions.Add(6, ButtonExcelOtherCardInformation_Click);
        exportFucntions.Add(7, ButtonExcelMerchantCardPinDebitInformation_Click);
        exportFucntions.Add(8, ButtonExcelMerchantCardInformation_Click);
        exportFucntions.Add(9, ButtonExcelMerchantCardInformationPrivateLabel_Click);
        exportFucntions.Add(10, ButtonExcelPricingInformation_Click);

        return exportFucntions;
    }

    public void DoMultiExportExcel()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();
        exportNames.Add(0, GetLocalResourceObject("LiteralResource1.Text").ToString());
        exportNames.Add(1, GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_BankInformation").ToString());
        exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_BusinessInformation").ToString());
        exportNames.Add(3, GetLocalResourceObject("LiteralResourceTransactionEstimates_Title.Text").ToString());
        if (IsRiskInformation)
        {
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_Text_RiskInformation").ToString());
        }

        exportNames.Add(5, GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_HierarchyInformation").ToString());
        exportNames.Add(6, GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_OtherCardInformation").ToString());
        exportNames.Add(7, GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_MerchantCardInformation").ToString() + " - " +
           GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_PINDebit").ToString());
        exportNames.Add(8, GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_MerchantCardInformation").ToString());
        exportNames.Add(9, GetLocalResourceObject("LiteralResourceMerchCardInfoPrivateLabel_Title.Text").ToString());
        exportNames.Add(10, GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_PricingInformation").ToString());

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
        if (_MerchantInfor == null || _MerchantInfor.Rows.Count <= 0)
            return string.Empty;
        DataRow dr = _MerchantInfor.Rows[0];
        return dr[colName].ToString();

    }
    protected string BindAddress(object add1, object add2, object add3, object city, object state, object zip)
    {
        return MerchantProfileHelper.BindAddress(add1, add2, add3, city, state, zip);
    }


    protected string FormatSIC(object sic, object sicDesc)
    {
        return MerchantProfileHelper.FormatSIC(sic, sicDesc);
    }

    protected string ExportTooltip(string value, int type)
    {
        if (value == "N" && type == 1)
            return GetLocalResourceObject("MIF_MerchantDetails_CAYAN_ExportTooltop1").ToString();
        else if (value == "Y" && type == 1)
            return GetLocalResourceObject("MIF_MerchantDetails_CAYAN_ExportTooltop2").ToString();
        else if (value == string.Empty && (type == 1 || type == 2))
            return string.Empty;
        else if (value == "N" && type == 2)
            return GetLocalResourceObject("MIF_MerchantDetails_CAYAN_ExportTooltop3").ToString();
        else if (value == "Y" && type == 2)
            return GetLocalResourceObject("MIF_MerchantDetails_CAYAN_ExportTooltop4").ToString();
        else
            return string.Empty;
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

    protected string ButtonExcelHierarchyInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_HIERARCHY_INFO)));
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", GetLocalResourceObject("LiteralResource34.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientName]", GetLocalResourceObject("LiteralResource37.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientLogin]", GetLocalResourceObject("LiteralResource38.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_SalesAgent]", GetLocalResourceObject("LiteralResource39.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Sys_Prin_Agent]", GetLocalResourceObject("LiteralResource40.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_Headqrtr_Merchant]", GetLocalResourceObject("LiteralResource41.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_MerchantType]", GetLocalResourceObject("LiteralResource42.Text").ToString());
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ChainCode]", GetLocalResourceObject("LiteralResource43.Text").ToString());

        strExcelTemplate.Replace("HI_CLIENT_NAME", EMPTY_VALUE + _datasource.Rows[0]["ClientName"].ToString());
        strExcelTemplate.Replace("HI_CLIENT_LOGIN", EMPTY_VALUE + _datasource.Rows[0]["ClientLogin"].ToString());
        strExcelTemplate.Replace("HI_SALES_AGENT", EMPTY_VALUE + _datasource.Rows[0]["SalesAgent"].ToString());
        strExcelTemplate.Replace("HI_SYS_PRIN_AGENT", EMPTY_VALUE + _datasource.Rows[0]["SysPrinAgent"].ToString());
        strExcelTemplate.Replace("HI_HEADQUARTER_MERCHANT", EMPTY_VALUE + _datasource.Rows[0]["Headquarter"].ToString());
        strExcelTemplate.Replace("HI_MERCHANT_TYPE", _datasource.Rows[0]["MerchantType"].ToString());
        strExcelTemplate.Replace("HI_CHAIN_CODE", EMPTY_VALUE + _datasource.Rows[0]["ChainCode"].ToString());

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_HierarchyInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelTransactionEstimates_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_TRANSACTION_ESTIMATES)));

        strExcelTemplate.Replace("[tpl_BankInformation_htm_TransactionEstimates]", GetLocalResourceObject("LiteralResourceTransactionEstimates_Title.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_ChainHdqtrPct_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_ChainHdqtrPct_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_ExcludeAdjChargebacks_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_ExcludeAdjChargebacks_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_DailyAuthDeclinePct_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_DailyAuthDeclinePct_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_DailyTransPerCard_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_DailyTransPerCard_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_MTDChargebackPct_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_MTDChargebackPct_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_MaxAuthAmt_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_MaxAuthAmt_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_DailyAuthsCount_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_DailyAuthsCount_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_RoundDiscount_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_RoundDiscount_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_DailyAuthsCardPct_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_DailyAuthsCardPct_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_AuthsSameAmt_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_AuthsSameAmt_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_BelowFloorAmt_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_BelowFloorAmt_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_VoiceAuthPct_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_VoiceAuthPct_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_SameAmtPct_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_SameAmtPct_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_DailyRetrievalCount_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_DailyRetrievalCount_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_AchIncExcl_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_AchIncExcl_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_DailyRetrievalAmt_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_DailyRetrievalAmt_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_ReturnVolPct_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_ReturnVolPct_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_BatchReturnCount_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_BatchReturnCount_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_ReturnCountPct_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_ReturnCountPct_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_BatchReturnAmt_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_BatchReturnAmt_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_MaxTKTOs_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_MaxTKTOs_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_MaxReturnAmt_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_MaxReturnAmt_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_ChargebackOccurrences_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_ChargebackOccurrences_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_FundLmtDay_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_FundLmtDay_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_WeeklyBatches_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_WeeklyBatches_Header.Text").ToString());
        strExcelTemplate.Replace("LiteralResourceTransactionEstimates_Limit30Day_Header", GetLocalResourceObject("LiteralResourceTransactionEstimates_Limit30Day_Header.Text").ToString());

        strExcelTemplate.Replace("ChainHdqtrPct", EMPTY_VALUE + FormatPercent(_datasource.Rows[0]["ChainHdqtrPct"].ToString()));
        strExcelTemplate.Replace("ExcludeAdjChargebacks", EMPTY_VALUE + _datasource.Rows[0]["ExcludeAdjChargebacks"].ToString());
        strExcelTemplate.Replace("DailyAuthDeclinePct", EMPTY_VALUE + FormatPercent(_datasource.Rows[0]["DailyAuthDeclinePct"].ToString()));
        strExcelTemplate.Replace("NumberDailyTransPerCard", EMPTY_VALUE + FormatInteger(_datasource.Rows[0]["NumberDailyTransPerCard"].ToString()));
        strExcelTemplate.Replace("MTDChargebackPct", EMPTY_VALUE + FormatPercent(_datasource.Rows[0]["MTDChargebackPct"].ToString()));
        strExcelTemplate.Replace("MaxAuthAmt", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["MaxAuthAmt"]));
        strExcelTemplate.Replace("DailyAuthsCount", EMPTY_VALUE + FormatInteger(_datasource.Rows[0]["DailyAuthsCount"].ToString()));
        strExcelTemplate.Replace("RoundDiscount", EMPTY_VALUE + _datasource.Rows[0]["RoundDiscount"].ToString());
        strExcelTemplate.Replace("DailyAuthsCardPct", EMPTY_VALUE + FormatPercent(_datasource.Rows[0]["DailyAuthsCardPct"].ToString()));
        strExcelTemplate.Replace("NumberAuthsSameAmt", EMPTY_VALUE + FormatInteger(_datasource.Rows[0]["NumberAuthsSameAmt"].ToString()));
        strExcelTemplate.Replace("BelowFloorAmt", EMPTY_VALUE + FormatPercent(_datasource.Rows[0]["BelowFloorAmt"].ToString()));
        strExcelTemplate.Replace("VoiceAuthPct", EMPTY_VALUE + FormatPercent(_datasource.Rows[0]["VoiceAuthPct"].ToString()));
        strExcelTemplate.Replace("SameAmtPct", EMPTY_VALUE + FormatPercent(_datasource.Rows[0]["SameAmtPct"].ToString()));
        strExcelTemplate.Replace("DailyRetrievalCount", EMPTY_VALUE + FormatInteger(_datasource.Rows[0]["DailyRetrievalCount"].ToString()));
        strExcelTemplate.Replace("AchIncExcl", EMPTY_VALUE + _datasource.Rows[0]["AchIncExcl"].ToString());
        strExcelTemplate.Replace("DailyRetrievalAmt", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["DailyRetrievalAmt"]));
        strExcelTemplate.Replace("ReturnVolPct", EMPTY_VALUE + FormatPercent(_datasource.Rows[0]["ReturnVolPct"].ToString()));
        strExcelTemplate.Replace("BatchReturnCount", EMPTY_VALUE + FormatInteger(_datasource.Rows[0]["BatchReturnCount"].ToString()));
        strExcelTemplate.Replace("ReturnCountPct", EMPTY_VALUE + FormatPercent(_datasource.Rows[0]["ReturnCountPct"].ToString()));
        strExcelTemplate.Replace("BatchReturnAmt", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["BatchReturnAmt"]));
        strExcelTemplate.Replace("MaxNumberTKTOs", EMPTY_VALUE + FormatInteger(_datasource.Rows[0]["MaxNumberTKTOs"].ToString()));
        strExcelTemplate.Replace("MaxReturnAmt", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["MaxReturnAmt"]));
        strExcelTemplate.Replace("NumberChargebackOccurrences", EMPTY_VALUE + FormatInteger(_datasource.Rows[0]["NumberChargebackOccurrences"].ToString()));
        strExcelTemplate.Replace("FundLmtDay", EMPTY_VALUE + _datasource.Rows[0]["FundLmtDay"].ToString());
        strExcelTemplate.Replace("NumberWeeklyBatches", EMPTY_VALUE + FormatInteger(_datasource.Rows[0]["NumberWeeklyBatches"].ToString()));
        strExcelTemplate.Replace("Limit30Day", EMPTY_VALUE + _datasource.Rows[0]["Limit30Day"].ToString());

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("LiteralResourceTransactionEstimates_Title.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantCardInformationPrivateLabel_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_MERCHANT_CARD_PRIVATELABEL_INFO)));


        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_MerchantCardInformation_PrivateLabel]", GetLocalResourceObject("LiteralResourceMerchCardInfoPrivateLabel_Title.Text").ToString());

        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_MerchantCardInformation]", GetLocalResourceObject("LiteralResourceMerchCardInfo_TitleExport").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_ProcSW]", GetLocalResourceObject("LiteralResource135.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_FeeClass]", GetLocalResourceObject("LiteralResource136.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_QualRate]", GetLocalResourceObject("LiteralResource137.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Mid_QualRate]", GetLocalResourceObject("LiteralResource138.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Non_QualRate]", GetLocalResourceObject("LiteralResource139.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Dues_AssessmentFlag]", GetLocalResourceObject("LiteralResourceMerchantCardInfo_DAFlag_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_DiscountMethod]", GetLocalResourceObject("LiteralResourceMerchantCardInfo_DiscMethod_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_MerchantPricingGrid]", GetLocalResourceObject("LiteralResourceMerchantCardInfo_MerchPricingGrid_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_OtherVolumePercent]", GetLocalResourceObject("LiteralResourceMerchantCardInfo_OtherVol_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_OtherItemRate]", GetLocalResourceObject("LiteralResource150.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_RebateRate1]", GetLocalResourceObject("LiteralResourceMerchantCardInfo_RebateRate1_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_RebateRate2]", GetLocalResourceObject("LiteralResourceMerchantCardInfo_RebateRate2_Header.Text").ToString());


        StringBuilder strPaths = new StringBuilder();

        StringBuilder strTemplatePath = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_MERCHANT_CARD_PRIVATELABEL_PART)));

        if (rptPrivateLabel.DataSource != null)
        {
            DataTable dt = rptPrivateLabel.DataSource as DataTable;
            foreach (DataRow r in dt.Rows)
            {
                StringBuilder str = new StringBuilder(strTemplatePath.ToString());
                str.Replace("[Card_Name]", r["CardType"].ToString());
                str.Replace("[ProcSW]", EMPTY_VALUE + r["ProcessSW"].ToString());
                str.Replace("[FeeClass]", EMPTY_VALUE + r["FeeClass"].ToString());
                str.Replace("[QualRate]", EMPTY_VALUE + r["QualRate"].ToString());
                str.Replace("[MidQualRate]", EMPTY_VALUE + r["MQualRate"].ToString());
                str.Replace("[NonQualRate]", EMPTY_VALUE + r["NQualRate"].ToString());
                str.Replace("[DAFlag]", EMPTY_VALUE + r["DuesAssessmentFlag"].ToString());
                str.Replace("[DiscountMethod]", EMPTY_VALUE + r["DiscountMethod"].ToString());
                str.Replace("[OtherVolume]", EMPTY_VALUE + r["OtherVolume"].ToString());
                str.Replace("[OtherItemRate]", EMPTY_VALUE + r["OtherItemRate"].ToString());
                str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + r["MerchantPricingGrid"].ToString());
                str.Replace("[RebateRate1]", EMPTY_VALUE + r["RebateRate1"].ToString());
                str.Replace("[RebateRate2]", EMPTY_VALUE + r["RebateRate2"].ToString());
                strPaths.Append(str.ToString());
            }
        }
        strExcelTemplate.Replace("[CARD_DETAIL]", strPaths.ToString());

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("LiteralResourceMerchCardInfoPrivateLabel_TitleExport").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelOtherCardInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_OTHER_CARD_INFO)));
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_OtherCardInformation]", GetLocalResourceObject("LiteralResource57.Text").ToString());
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_AMEX]", GetLocalResourceObject("LiteralResource60.Text").ToString());
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_PinDebit]", GetLocalResourceObject("LiteralResource61.Text").ToString());
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_AMEXONEPOINT]", GetLocalResourceObject("LiteralResource62.Text").ToString());
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_JCB]", GetLocalResourceObject("LiteralResource63.Text").ToString());
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_Discover]", GetLocalResourceObject("LiteralResource64.Text").ToString());
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_WrightExpress]", GetLocalResourceObject("LiteralResource65.Text").ToString());
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_DiscoverFullAQC]", GetLocalResourceObject("LiteralResource66.Text").ToString());
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_Voyager]", GetLocalResourceObject("LiteralResource67.Text").ToString());
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_Paypal]", GetLocalResourceObject("LiteralResourceOtherCard_Paypal_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_OtherCardInformation_htm_AMEXPASSTHRU]", GetLocalResourceObject("LiteralResource127.Text").ToString());

        strExcelTemplate.Replace("OCI_AMEX_ONE_POINT", EMPTY_VALUE + _datasource.Rows[0]["AMEXONPOINT"].ToString());
        strExcelTemplate.Replace("OCI_AMEX", EMPTY_VALUE + _datasource.Rows[0]["AMEX"].ToString());
        strExcelTemplate.Replace("OCI_PIN_DEBIT", EMPTY_VALUE + _datasource.Rows[0]["PinDebit"].ToString());
        strExcelTemplate.Replace("OCI_JCB", EMPTY_VALUE + _datasource.Rows[0]["JCB"].ToString());
        strExcelTemplate.Replace("OCI_DISCOVER_FULL_AQC", EMPTY_VALUE + _datasource.Rows[0]["DiscoverFullAQC"].ToString());
        strExcelTemplate.Replace("OCI_DISCOVER", EMPTY_VALUE + _datasource.Rows[0]["Discover"].ToString());
        strExcelTemplate.Replace("OCI_WRIGHT_EXPRESS", EMPTY_VALUE + _datasource.Rows[0]["WrightExpress"].ToString());
        strExcelTemplate.Replace("OCI_VOYAGER", EMPTY_VALUE + _datasource.Rows[0]["Voyager"].ToString());
        strExcelTemplate.Replace("AMEX_PASS_THRU", EMPTY_VALUE + _datasource.Rows[0]["AMEXPassThrough"].ToString());
        strExcelTemplate.Replace("PAYPAL", EMPTY_VALUE + _datasource.Rows[0]["PayPalEnabledDI"].ToString());

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_OtherCardInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    private void CustomReplace(StringBuilder originalStr, string startStr, string endStr, bool isRemove)
    {
        int firstIdx = originalStr.ToString().IndexOf(startStr);
        int lastIdx = originalStr.ToString().IndexOf(endStr);
        if (isRemove)
        {
            originalStr.Remove(firstIdx, lastIdx + endStr.Length - firstIdx);
        }
        else
        {
            originalStr.Replace(startStr, string.Empty).Replace(endStr, string.Empty);
        }
    }

    protected string ButtonExcelPricingInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_PRICING_INFO)));


        CustomReplace(strExcelTemplate, "PI_VISA_BEGIN", "PI_VISA_END", !CheckMerchantCardInformation("VISA"));
        CustomReplace(strExcelTemplate, "PI_MC_BEGIN", "PI_MC_END", !CheckMerchantCardInformation("MC"));
        CustomReplace(strExcelTemplate, "PI_DISFULLACQ_BEGIN", "PI_DISFULLACQ_END", !CheckMerchantCardInformation("DISC FULL ACQ"));
        CustomReplace(strExcelTemplate, "PI_AMEX_BEGIN", "PI_AMEX_END", !CheckMerchantCardInformation("AMEX"));

        strExcelTemplate.Replace("[tpl_PricingInformation_htm_PricingInformation]", GetLocalResourceObject("LiteralResource68.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_DailyDeposit]", GetLocalResourceObject("LiteralResource71.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_WeeklyDeposit]", GetLocalResourceObject("LiteralResource72.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_DailyAuthAmount]", GetLocalResourceObject("LiteralResource73.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_Avg_TicketAmount]", GetLocalResourceObject("LiteralResource74.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AuthGrid]", GetLocalResourceObject("LiteralResource75.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_UserDefinedGrid]", GetLocalResourceObject("LiteralResource76.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_UserFeeControlGrid]", GetLocalResourceObject("LiteralResourcePricing_FeeControl_Header.Text").ToString());

        strExcelTemplate.Replace("[tpl_PricingInformation_htm_IncomeFactors]", GetLocalResourceObject("LiteralResource77.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AccountFee1]", GetLocalResourceObject("LiteralResource78.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AccountFee2]", GetLocalResourceObject("LiteralResource79.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AccountFee3]", GetLocalResourceObject("LiteralResource80.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AccountFee4]", GetLocalResourceObject("LiteralResource81.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AccountFee5]", GetLocalResourceObject("LiteralResource82.Text").ToString());

        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RecurringFeeFlag]", GetLocalResourceObject("LiteralResource83.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RecurFeeInd]", GetLocalResourceObject("LiteralResource84.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RecurFeeAmt]", GetLocalResourceObject("LiteralResource85.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RecurFeeDesc]", GetLocalResourceObject("LiteralResource86.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_StmtBundleOption]", GetLocalResourceObject("LiteralResourcePricing_StmtBundle_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_BundlePct]", GetLocalResourceObject("LiteralResource88.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_BundleRate]", GetLocalResourceObject("LiteralResource90.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ACHRejectFee]", GetLocalResourceObject("LiteralResourcePricing_ACHRejectFee_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_eIDSFee]", GetLocalResourceObject("LiteralResource_Pricing_eIDSFee_Header.Text").ToString());


        strExcelTemplate.Replace("[tpl_PricingInformation_htm_BatchHeaderFee]", GetLocalResourceObject("LiteralResource90.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ReturnTransactionFee]", GetLocalResourceObject("LiteralResource91.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ChargebackFee]", GetLocalResourceObject("LiteralResource92.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RetrievalFee]", GetLocalResourceObject("LiteralResource93.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_SalesTransFee]", GetLocalResourceObject("LiteralResource94.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_OtherVolumePercent]", GetLocalResourceObject("LiteralResource95.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_OtherItemCharge]", GetLocalResourceObject("LiteralResource96.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_eIDSIndicator]", GetLocalResourceObject("LiteralResourcePricing_eIDSIndicator_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RetrievalFaxFlag]", GetLocalResourceObject("LiteralResourcePricing_RetrievalFaxFlag_Header.Text").ToString());


        strExcelTemplate.Replace("[tpl_PricingInformation_htm_WebsiteUsage]", GetLocalResourceObject("LiteralResource97.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_IVRUsage]", GetLocalResourceObject("LiteralResource98.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_UnregPct]", GetLocalResourceObject("LiteralResource99.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_UnregRate]", GetLocalResourceObject("LiteralResource100.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RegPct]", GetLocalResourceObject("LiteralResource101.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RegRate]", GetLocalResourceObject("LiteralResource102.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_Payeezy_Setup_Fee]", GetLocalResourceObject("LiteralResourcePricing_PayeezySetupFee_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_Payeezy_Monthly_Fee]", GetLocalResourceObject("LiteralResourcePricing_PayeezyMonthlyFee_Header.Text").ToString());
        //

        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RCChg]", GetLocalResourceObject("LiteralResourcePricing_RCChg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_TermChg]", GetLocalResourceObject("LiteralResourcePricing_TermChg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_TermChgStartDate]", GetLocalResourceObject("LiteralResourcePricing_TermChgStartDate_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_TermChgStopDate]", GetLocalResourceObject("LiteralResourcePricing_TermChgStopDate_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ImprintChg]", GetLocalResourceObject("LiteralResourcePricing_ImprintChg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_IntraFeeFlg]", GetLocalResourceObject("LiteralResourcePricing_IntraFeeFlg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_FloatFlg]", GetLocalResourceObject("LiteralResourcePricing_FloatFlg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcctChg2StartDate]", GetLocalResourceObject("LiteralResourcePricing_AcctChg2StartDate_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcctChg2StopDate]", GetLocalResourceObject("LiteralResourcePricing_AcctChg2StopDate_Header.Text").ToString());
        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_PrintChg]", GetLocalResourceObject("LiteralResourcePricing_PrintChg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_HelpDeskChg]", GetLocalResourceObject("LiteralResourcePricing_HelpDeskChg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AsstServChg]", GetLocalResourceObject("LiteralResourcePricing_AsstServChg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ETCConfLtrChg]", GetLocalResourceObject("LiteralResourcePricing_ETCConfLtrChg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_OneTimeChg]", GetLocalResourceObject("LiteralResourcePricing_OneTimeChg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_MerchantAdvChg]", GetLocalResourceObject("LiteralResourcePricing_MerchantAdvChg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AchChg]", GetLocalResourceObject("LiteralResourcePricing_AchChg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcctChg1StartDate]", GetLocalResourceObject("LiteralResourcePricing_AcctChg1StartDate_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcctChg1StopDate]", GetLocalResourceObject("LiteralResourcePricing_AcctChg1StopDate_Header.Text").ToString());
        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_MinVolFeeFlag]", GetLocalResourceObject("LiteralResourcePricing_MinVolFeeFlag_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_MinVolFeeChg]", GetLocalResourceObject("LiteralResourcePricing_MinVolFeeChg_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_OneTimeSetupFeeAmt]", GetLocalResourceObject("LiteralResourcePricing_OneTimeSetupFeeAmt_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcctChg3StartDate]", GetLocalResourceObject("LiteralResourcePricing_AcctChg3StartDate_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcctChg3StopDate]", GetLocalResourceObject("LiteralResourcePricing_AcctChg3StopDate_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcctChg4StartDate]", GetLocalResourceObject("LiteralResourcePricing_AcctChg4StartDate_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcctChg4StopDate]", GetLocalResourceObject("LiteralResourcePricing_AcctChg4StopDate_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_StarDebitNetworkFee]", GetLocalResourceObject("LiteralResourcePricing_StarDebitNetworkFee_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_PulseDebitNetworkFee]", GetLocalResourceObject("LiteralResourcePricing_PulseDebitNetworkFee_Header.Text").ToString());
        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_MFCOtherFee1]", GetLocalResourceObject("LiteralResourcePricing_MFCOtherFee1_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_MFCOtherFee2]", GetLocalResourceObject("LiteralResourcePricing_MFCOtherFee2_Header.Text").ToString()); 
        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_BatchCst]", GetLocalResourceObject("LiteralResourcePricing_BatchCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ICItemCst]", GetLocalResourceObject("LiteralResourcePricing_ICItemCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_IntraItemCst]", GetLocalResourceObject("LiteralResourcePricing_IntraItemCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_TapeCst]", GetLocalResourceObject("LiteralResourcePricingTapeCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_TermCst]", GetLocalResourceObject("LiteralResourcePricingTermCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_RCCst]", GetLocalResourceObject("LiteralResourcePricingRCCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ChgbkCst]", GetLocalResourceObject("LiteralResourcePricingChgbkCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ImprntCst]", GetLocalResourceObject("LiteralResourcePricingImprntCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_OtherPctCst]", GetLocalResourceObject("LiteralResourcePricingOtherPctCst_Header.Text").ToString()); 
        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_WBCstMC]", GetLocalResourceObject("LiteralResourcePricingWBCstMC_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_WBCstVS]", GetLocalResourceObject("LiteralResourcePricingWBCstVS_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_FixCst]", GetLocalResourceObject("LiteralResourcePricingFixCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ItemCst]", GetLocalResourceObject("LiteralResourcePricingItemCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ETCItemCst]", GetLocalResourceObject("LiteralResourcePricingETCItemCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AuthExpGridID]", GetLocalResourceObject("LiteralResourcePricingAuthExpGridID_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_OneTimeCst]", GetLocalResourceObject("LiteralResourcePricingOneTimeCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_12BLtrCst]", GetLocalResourceObject("LiteralResourcePricing12BLtrCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_PrintCst]", GetLocalResourceObject("LiteralResourcePricingPrintCst_Header.Text").ToString());
        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_HelpDeskCst]", GetLocalResourceObject("LiteralResourcePricingHelpDeskCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AsstServCst]", GetLocalResourceObject("LiteralResourcePricingAsstServCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ETCConfLtrCst]", GetLocalResourceObject("LiteralResourcePricingETCConfLtrCst_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_UserDepExpGridID]", GetLocalResourceObject("LiteralResourcePricingUserDepExpGridID_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AVSCst]", GetLocalResourceObject("LiteralResourcePricingAVSCst_Header.Text").ToString());
        //


        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_VisaAssocFees]", GetLocalResourceObject("LiteralResourcePricingVisaAssocFees_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcqrPrFee]", GetLocalResourceObject("LiteralResource104.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_MisuseAuthFee]", GetLocalResourceObject("LiteralResource105.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ISA]", GetLocalResourceObject("LiteralResource106.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ZeroFloor]", GetLocalResourceObject("LiteralResource107.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_IntlAcq]", GetLocalResourceObject("LiteralResource108.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_TransIntegrityFee]", GetLocalResourceObject("LiteralResourcePricingTransIntegrityFee_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_FANFFeeFlag]", GetLocalResourceObject("LiteralResourcePricingFANFFeeFlag_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_FANFCPSurcharge]", GetLocalResourceObject("LiteralResourcePricingFANFCPSurcharge_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_FANFCNPSurcharge]", GetLocalResourceObject("LiteralResourcePricingFANFCPSurcharge_Header.Text").ToString());
        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_KilobyteFeeIndicator]", GetLocalResourceObject("LiteralResourcePricingKilobyteFeeIndicator_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_KilobyteSurcharge]", GetLocalResourceObject("LiteralResourcePricingKilobyteSurcharge_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ProcessFeeIncome]", GetLocalResourceObject("LiteralResourcePricingProcessFeeIncome_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ProcessFeeExpense]", GetLocalResourceObject("LiteralResourcePricingProcessFeeExpense_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_BINICAFeeIncome]", GetLocalResourceObject("LiteralResourcePricingBINICAFeeIncome_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_BINICAFeeExpense]", GetLocalResourceObject("LiteralResourcePricingBINICAFeeExpense_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AFDNonParticipationFee]", GetLocalResourceObject("LiteralResourcePricingAFDNonParticipationFee_Header.Text").ToString());
        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_CVC2FeeIndicator]", GetLocalResourceObject("LiteralResourcePricingCVC2FeeIndicator_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_CVC2Surcharge]", GetLocalResourceObject("LiteralResourcePricingCVC2FeeSurcharge_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_LicensePerItemIncome]", GetLocalResourceObject("LiteralResourcePricingLicensePerItemIncome_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_LicensePerItemExpense]", GetLocalResourceObject("LiteralResourcePricingLicensePerItemExpense_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_LicenseRateIncome]", GetLocalResourceObject("LiteralResourcePricingLicenseRateIncome_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_LicenseRateExpense]", GetLocalResourceObject("LiteralResourcePricingLicenseRateExpense_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_LicenseFlatIncome]", GetLocalResourceObject("LiteralResourcePricingLicenseFlatIncome_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_LicenseFlatExpense]", GetLocalResourceObject("LiteralResourcePricingLicenseFlatExpense_Header.Text").ToString());
        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AuthFeeflag]", GetLocalResourceObject("LiteralResourcePricingAuthFeeflag_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AuthSurcharge]", GetLocalResourceObject("LiteralResourcePricingAuthSurcharge_Header.Text").ToString());
        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AMEXAssocFees]", GetLocalResourceObject("LiteralResourcePricingAMEXAssocFees_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AVSNetworkFee]", GetLocalResourceObject("LiteralResourcePricingAMEXNetworkFee_Header.Text").ToString());
        //
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_MCAssocFees]", GetLocalResourceObject("LiteralResourcePricingMCAssocFees_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_NABUFee]", GetLocalResourceObject("LiteralResource110.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_CrossBorderFee]", GetLocalResourceObject("LiteralResource111.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AcqSupportFee]", GetLocalResourceObject("LiteralResource112.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ReversalFee]", GetLocalResourceObject("LiteralResource113.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_DiscFullAcqAssoc]", GetLocalResourceObject("LiteralResourcePricingDiscFullAcqAssoc_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_INTLProcessFlag]", GetLocalResourceObject("LiteralResource115.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_INTLServiceFlag]", GetLocalResourceObject("LiteralResource116.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_DataUsageFlag]", GetLocalResourceObject("LiteralResource117.Text").ToString());

        strExcelTemplate.Replace("[tpl_PricingInformation_htm_ExpenseFactors]", GetLocalResourceObject("LiteralResourcePricing_ExpenseFactors_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_DiscFullAcqAssoc]", GetLocalResourceObject("LiteralResourcePricingDiscFullAcqAssoc_Header.Text").ToString());

        strExcelTemplate.Replace("[tpl_PricingInformation_htm_LicenseFlatOccurrenceIndicator]", GetLocalResourceObject("LiteralResourcePricingLicenseFlatOccurrenceIndicator_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_MCICAAVSIncome]", GetLocalResourceObject("LiteralResourcePricingMCICAAVSIncome_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_MCICAAVSExpense]", GetLocalResourceObject("LiteralResourcePricingMCICAAVSExpense_Header.Text").ToString());
        strExcelTemplate.Replace("[tpl_PricingInformation_htm_AVSAcquirerMCFee]", GetLocalResourceObject("LiteralResourcePricingAVSAcquirerMCFee_Header.Text").ToString());
        //



        if (CheckMerchantCardInformation("VISA"))
        {
            strExcelTemplate.Replace("PI_ACQR_PR_FEE", EMPTY_VALUE + _datasource.Rows[0]["AcqrPrFee"].ToString());
            strExcelTemplate.Replace("PI_MISUSE_AUTH_FEE", EMPTY_VALUE + _datasource.Rows[0]["MisuseAuthFee"].ToString());
            strExcelTemplate.Replace("PI_ISA", EMPTY_VALUE + _datasource.Rows[0]["ISA"].ToString());
            strExcelTemplate.Replace("PI_ZERO_FLOOR", EMPTY_VALUE + _datasource.Rows[0]["ZeroFloor"].ToString());
            strExcelTemplate.Replace("PI_INTL_ACQ", EMPTY_VALUE + _datasource.Rows[0]["IntlAcq"].ToString());
            strExcelTemplate.Replace("PI_TRAN_INT_FEE", EMPTY_VALUE + _datasource.Rows[0]["TransIntegrityFee"].ToString());
            strExcelTemplate.Replace("PI_FANF_FEE_IND", EMPTY_VALUE + _datasource.Rows[0]["NPFFeeFlag"].ToString());
            strExcelTemplate.Replace("PI_FANF_CP_SRCHG", EMPTY_VALUE + _datasource.Rows[0]["NPFCPSurcharge"].ToString());
            strExcelTemplate.Replace("PI_FANF_CNP_SRCHG", EMPTY_VALUE + _datasource.Rows[0]["NPFCNPSurcharge"].ToString());

            strExcelTemplate.Replace("PI_KILOBYTE_FEE_INDICATOR", EMPTY_VALUE + _datasource.Rows[0]["KilobyteFeeIndicator"].ToString());
            strExcelTemplate.Replace("PI_KILOBYTE_SURCHARGE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["KilobyteSurcharge"]));
            strExcelTemplate.Replace("PI_PROCESS_FEE_INCOME", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ProcessFeeExpense"]));
            strExcelTemplate.Replace("PI_PROCESS_FEE_EXPENSE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ProcessFeeIncome"]));
            strExcelTemplate.Replace("PI_BIN_ICA_FEE_INCOME", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["BINICAFeeExpense"]));
            strExcelTemplate.Replace("PI_BIN_ICA_FEE_EXPENSE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["BINICAFeeIncome"]));
            strExcelTemplate.Replace("PI_AFD_NonParticipation_Fee", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AFDNonPartFee"]));
        }
        if (CheckMerchantCardInformation("MC"))
        {
            strExcelTemplate.Replace("PI_NABU_FEE", EMPTY_VALUE + _datasource.Rows[0]["NABUFee"].ToString());
            strExcelTemplate.Replace("PI_CROSS_BORDER_FEE", EMPTY_VALUE + _datasource.Rows[0]["CrossBorderFee"].ToString());
            strExcelTemplate.Replace("PI_ACQ_SUPPORT_FEE", EMPTY_VALUE + _datasource.Rows[0]["AcqSupportFee"].ToString());
            strExcelTemplate.Replace("PI_REVERSAL_FEE", EMPTY_VALUE + _datasource.Rows[0]["ReversalFee"].ToString());

            strExcelTemplate.Replace("PI_MC_KILOBYTE_FEE_INDICATOR", EMPTY_VALUE + _datasource.Rows[0]["MC_KilobyteFeeIndicator"].ToString());
            strExcelTemplate.Replace("PI_MC_KILOBYTE_SURCHARGE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["MC_KilobyteSurcharge"]));
            strExcelTemplate.Replace("PI_MC_PROCESS_FEE_INCOME", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["MC_ProcessFeeExpense"]));
            strExcelTemplate.Replace("PI_MC_PROCESS_FEE_EXPENSE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["MC_ProcessFeeIncome"]));
            strExcelTemplate.Replace("PI_MC_BIN_ICA_FEE_INCOME", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["MC_BINICAFeeExpense"]));
            strExcelTemplate.Replace("PI_MC_BIN_ICA_FEE_EXPENSE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["MC_BINICAFeeIncome"]));
            strExcelTemplate.Replace("PI_CVC2_FEE_INDICATOR", EMPTY_VALUE + _datasource.Rows[0]["CVC2FeeIndicator"].ToString());
            strExcelTemplate.Replace("PI_CVC2_SURCHARGE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["CVC2Surcharge"]));
            strExcelTemplate.Replace("PI_LICENSE_PER_ITEM_INCOME", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["LicensePerItemIncome"]));
            strExcelTemplate.Replace("PI_LICENSE_PER_ITEM_EXPENSE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["LicensePerItemExpense"]));
            strExcelTemplate.Replace("PI_LICENSE_RATE_INCOME", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["LicenseRateIncome"]));
            strExcelTemplate.Replace("PI_LICENSE_RATE_EXPENSE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["LicenseRateExpense"]));
            strExcelTemplate.Replace("PI_LICENSE_FLAT_INCOME", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["LicenseFlatIncome"]));
            strExcelTemplate.Replace("PI_LICENSE_FLAT_EXPENSE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["LicenseFlatExpense"]));
            strExcelTemplate.Replace("PI_LICENSE_FLAT_OCCURRENCE_INDICATOR", EMPTY_VALUE + _datasource.Rows[0]["LicenseFlatOccurrenceIndicator"].ToString());
            strExcelTemplate.Replace("PI_MC_ICA_AVS_INCOME", EMPTY_VALUE + Format_MC_ICA_AVS(_datasource.Rows[0]["MCICAAVSIncome"]));
            strExcelTemplate.Replace("PI_MC_ICA_AVS_EXPENSE", EMPTY_VALUE + Format_MC_ICA_AVS(_datasource.Rows[0]["MCICAAVSExpense"]));
            strExcelTemplate.Replace("PI_AVS_Acquirer_MC_Fee", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AVSAcquirerMCFee"]));
        }
        if (CheckMerchantCardInformation("DISC FULL ACQ"))
        {
            strExcelTemplate.Replace("PI_INTL_PROCESS_FLAG", EMPTY_VALUE + _datasource.Rows[0]["INTLProcessFlag"].ToString());
            strExcelTemplate.Replace("PI_INTL_SERVICE_FLAG", EMPTY_VALUE + _datasource.Rows[0]["INTLServiceFlag"].ToString());
            strExcelTemplate.Replace("PI_DATA_USAGE_FLAG", EMPTY_VALUE + _datasource.Rows[0]["DataUsageFlag"].ToString());

            strExcelTemplate.Replace("PI_AUTH_FEE_FLAG", EMPTY_VALUE + _datasource.Rows[0]["AuthFeeFlag"].ToString());
            strExcelTemplate.Replace("PI_AUTH_SURCHARGE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AuthSurcharge"]));
        }
        if (CheckMerchantCardInformation("AMEX"))
        {
            strExcelTemplate.Replace("PI_AVS_Network_Fee", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AVSNetworkFee"]));
        }
        strExcelTemplate.Replace("PI_DAILY_DEPOSIT", EMPTY_VALUE + _datasource.Rows[0]["DailyDeposit"].ToString());
        strExcelTemplate.Replace("PI_ACCOUNT_FEE1", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AccountFee1"]));
        strExcelTemplate.Replace("PI_BATCH_HEADER_FEE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["BatchHeaderFee"], 4));
        strExcelTemplate.Replace("PI_WEEKLY_DEPOSIT", EMPTY_VALUE + _datasource.Rows[0]["WeeklyDeposit"].ToString());
        strExcelTemplate.Replace("PI_ACCOUNT_FEE2", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AccountFee2"]));
        strExcelTemplate.Replace("PI_RETURN_TRANSACTION_FEE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ReturnTransFee"], 4));
        strExcelTemplate.Replace("PI_DAILY_AUTH_AMOUNT", EMPTY_VALUE + _datasource.Rows[0]["DailyAuthAmount"].ToString());
        strExcelTemplate.Replace("PI_ACCOUNT_FEE3", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AccountFee3"]));
        strExcelTemplate.Replace("PI_CHARGEBACK_FEE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ChargebackFee"], 4));
        strExcelTemplate.Replace("PI_AVG_TICKET_AMOUNT", EMPTY_VALUE + _datasource.Rows[0]["AvgTicketAmount"].ToString());
        strExcelTemplate.Replace("PI_ACCOUNT_FEE4", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AccountFee4"]));
        strExcelTemplate.Replace("PI_RETRIEVAL_FEE", EMPTY_VALUE + _datasource.Rows[0]["RetrievalFee"].ToString());
        strExcelTemplate.Replace("PI_AUTH_GRID", EMPTY_VALUE + _datasource.Rows[0]["AuthGrid"].ToString());
        strExcelTemplate.Replace("PI_RECURRING_FEE_FLAG", EMPTY_VALUE + _datasource.Rows[0]["RecurFeeFlag"].ToString());
        strExcelTemplate.Replace("PI_SALES_TRANS_FEE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["SaleTransFee"], 4));
        strExcelTemplate.Replace("PI_USER_DEFINED_GRID", EMPTY_VALUE + _datasource.Rows[0]["UserDefinedGrid"].ToString());
        strExcelTemplate.Replace("PI_MFCGridID", EMPTY_VALUE + _datasource.Rows[0]["MFCGridID"].ToString());
        strExcelTemplate.Replace("PI_RECUR_FEE_IND", EMPTY_VALUE + _datasource.Rows[0]["RecurFeeInd"].ToString());
        strExcelTemplate.Replace("PI_OTHER_VOLUME", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumnPercent"].ToString());
        strExcelTemplate.Replace("PI_RECUR_FEE_AMT", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["RecurFeeAmt"]));
        strExcelTemplate.Replace("PI_OTHER_ITEM_CHARGE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["OtherItemCharge"], 4));
        strExcelTemplate.Replace("PI_RECUR_FEE_DESC", EMPTY_VALUE + _datasource.Rows[0]["RecurFeeDesc"].ToString());
        strExcelTemplate.Replace("PI_WEBSITE_USAGE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["WebsiteUsage"], 2));
        strExcelTemplate.Replace("PI_IVR_USAGE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["IVRUsage"], 2));
        strExcelTemplate.Replace("PI_ACCOUNT_FEE5", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AccountFee5"]));
        strExcelTemplate.Replace("PI_STMT_BUNDLE_OPTION", EMPTY_VALUE + _datasource.Rows[0]["StmtBundleOption"].ToString());
        strExcelTemplate.Replace("PI_BUNDLE_PCT", EMPTY_VALUE + _datasource.Rows[0]["BundlePct"].ToString());
        strExcelTemplate.Replace("PI_BUNDLE_RATE", EMPTY_VALUE + _datasource.Rows[0]["BundleRate"].ToString());
        strExcelTemplate.Replace("PI_UNREG_PCT", EMPTY_VALUE + _datasource.Rows[0]["UnregPct"].ToString());
        strExcelTemplate.Replace("PI_UNREG_RATE", EMPTY_VALUE + _datasource.Rows[0]["UnregRate"].ToString());
        strExcelTemplate.Replace("PI_REG_PCT", EMPTY_VALUE + _datasource.Rows[0]["RegPct"].ToString());
        strExcelTemplate.Replace("PI_REG_RATE", EMPTY_VALUE + _datasource.Rows[0]["RegRate"].ToString());
        strExcelTemplate.Replace("PI_ACH_REJECT_FEE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ACHRejectFee"], 2));
        strExcelTemplate.Replace("PI_EIDS_FEE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["eIDSFee"], 2));
        strExcelTemplate.Replace("PI_EIDS_INDICATOR", EMPTY_VALUE + _datasource.Rows[0]["eIDSIndicator"].ToString());
        strExcelTemplate.Replace("PI_RETRIEVAL_FAX_FLAG", EMPTY_VALUE + _datasource.Rows[0]["RetrievalFaxFlag"].ToString());

        strExcelTemplate.Replace("PI_GGE4_SETUP_FEE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["GGe4SetupFee"], 4));
        strExcelTemplate.Replace("PI_GGE4_MONTHLY_FEE", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["GGe4MonthlyFee"], 4));

        strExcelTemplate.Replace("PI_RC_CHG", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["RCChg"]));
        strExcelTemplate.Replace("PI_TERM_CHG_FIX", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["TermChg"]));
        strExcelTemplate.Replace("PI_TERM_CHG_START_DATE", EMPTY_VALUE + _datasource.Rows[0]["TermChgStartDate"].ToString());
        strExcelTemplate.Replace("PI_TERM_CHG_STOP_DATE", EMPTY_VALUE + _datasource.Rows[0]["TermChgStopDate"].ToString());
        strExcelTemplate.Replace("PI_IMPRINT_CHG", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ImprintChg"]));
        strExcelTemplate.Replace("PI_INTRA_FEE_FLG", EMPTY_VALUE + _datasource.Rows[0]["IntraFeeFlg"].ToString());
        strExcelTemplate.Replace("PI_FLOAT_FLG", EMPTY_VALUE + _datasource.Rows[0]["FloatFlg"].ToString());
        strExcelTemplate.Replace("PI_ACCT_CHG2_START_DATE", EMPTY_VALUE + _datasource.Rows[0]["AcctChg2StartDate"].ToString());
        strExcelTemplate.Replace("PI_ACCT_CHG2_STOP_DATE", EMPTY_VALUE + _datasource.Rows[0]["AcctChg2StopDate"].ToString());
        strExcelTemplate.Replace("PI_PRINT_CHG", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["PrintChg"]));
        strExcelTemplate.Replace("PI_HELP_DESK_CHG", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["HelpDeskChg"]));
        strExcelTemplate.Replace("PI_ASST_SERV_CHG", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AsstServChg"]));
        strExcelTemplate.Replace("PI_ETC_CONF_LTR_CHG", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ETCConfLtrChg"]));
        strExcelTemplate.Replace("PI_ONE_TIME_CHG", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["OneTimeChg"]));
        strExcelTemplate.Replace("PI_MERCHANT_ADV_CHG", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["MerchantAdvChg"]));
        strExcelTemplate.Replace("PI_ACH_CHG", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ACHChg"]));
        strExcelTemplate.Replace("PI_ACCT_CHG1_START_DATE", EMPTY_VALUE + _datasource.Rows[0]["AcctChg1StartDate"].ToString());
        strExcelTemplate.Replace("PI_ACCT_CHG1_STOP_DATE", EMPTY_VALUE + _datasource.Rows[0]["AcctChg1StopDate"].ToString());
        strExcelTemplate.Replace("PI_MIN_VOL_FEE_FLAG", EMPTY_VALUE + _datasource.Rows[0]["MinVolFeeFlag"].ToString());
        strExcelTemplate.Replace("PI_MIN_VOL_FEE_CHG", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["MinVolFeeChg"]));
        strExcelTemplate.Replace("PI_ONE_TIME_SETUP_FEE_AMT", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["OneTimeSetupFeeAmt"]));
        strExcelTemplate.Replace("PI_ACCT_CHG3_START_DATE", EMPTY_VALUE + _datasource.Rows[0]["AcctChg3StartDate"].ToString());
        strExcelTemplate.Replace("PI_ACCT_CHG3_STOP_DATE", EMPTY_VALUE + _datasource.Rows[0]["AcctChg3StopDate"].ToString());
        strExcelTemplate.Replace("PI_ACCT_CHG4_START_DATE", EMPTY_VALUE + _datasource.Rows[0]["AcctChg4StartDate"].ToString());
        strExcelTemplate.Replace("PI_ACCT_CHG4_STOP_DATE", EMPTY_VALUE + _datasource.Rows[0]["AcctChg4StopDate"].ToString());
        strExcelTemplate.Replace("PI_Star_Debit_Network_Fee", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["StarDebitNetworkFee"]));
        strExcelTemplate.Replace("PI_Pulse_Debit_Network_Fee", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["PulseDebitNetworkFee"]));
        strExcelTemplate.Replace("PI_MFC_Other_Fee_1", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["MFCOtherFee1"]));
        strExcelTemplate.Replace("PI_MFC_Other_Fee_2", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["MFCOtherFee2"]));
         
        strExcelTemplate.Replace("PI_BATCH_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["BatchCst"]));
        strExcelTemplate.Replace("PI_IC_ITEM_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ICItemCst"]));
        strExcelTemplate.Replace("PI_INTRA_ITEM_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["IntraItemCst"]));
        strExcelTemplate.Replace("PI_TAPE_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["TapeCst"]));
        strExcelTemplate.Replace("PI_TERM_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["TermCst"]));
        strExcelTemplate.Replace("PI_RC_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["RCCst"]));
        strExcelTemplate.Replace("PI_CHGBK_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ChgbkCst"]));
        strExcelTemplate.Replace("PI_IMPRNT_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ImprntCst"]));
        strExcelTemplate.Replace("PI_OTHER_PCT_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["OtherPctCst"]));
        strExcelTemplate.Replace("PI_WB_CST_MC", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["WBCstMC"]));
        strExcelTemplate.Replace("PI_WB_CST_VS", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["WBCstVS"]));
        strExcelTemplate.Replace("PI_FIX_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["FixCst"]));
        strExcelTemplate.Replace("PI_ITEM_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ItemCst"]));
        strExcelTemplate.Replace("PI_ETC_ITEM_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ETCItemCst"]));
        strExcelTemplate.Replace("PI_AUTH_EXP_GRID_ID", EMPTY_VALUE + _datasource.Rows[0]["AuthExpGridID"].ToString());
        strExcelTemplate.Replace("PI_ONE_TIME_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["OneTimeCst"]));
        strExcelTemplate.Replace("PI_12B_LTR_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["12BLtrCst"]));
        strExcelTemplate.Replace("PI_PRINT_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["PrintCst"]));
        strExcelTemplate.Replace("PI_HELP_DESK_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["HelpDeskCst"]));
        strExcelTemplate.Replace("PI_ASST_SERV_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AsstServCst"]));
        strExcelTemplate.Replace("PI_ETC_CONF_LTR_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["ETCConfLtrCst"]));
        strExcelTemplate.Replace("PI_USER_DEP_EXP_GRID_ID", EMPTY_VALUE + _datasource.Rows[0]["UserDepExpGridID"].ToString());
        strExcelTemplate.Replace("PI_AVS_CST", EMPTY_VALUE + FormatCurrency(_datasource.Rows[0]["AVSCst"]));
        
 
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_PricingInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantCardInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_MERCHANT_CARD_INFO)));
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_MerchantCardInformation]", GetLocalResourceObject("LiteralResource118.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_ProcSW]", GetLocalResourceObject("LiteralResource135.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_FeeClass]", GetLocalResourceObject("LiteralResource136.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_QualRate]", GetLocalResourceObject("LiteralResource137.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Mid_QualRate]", GetLocalResourceObject("LiteralResource138.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Non_QualRate]", GetLocalResourceObject("LiteralResource139.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_InterchangeFeeFlag]", GetLocalResourceObject("LiteralResource140.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Dues_AssessmentFlag]", GetLocalResourceObject("LiteralResource141.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Dues_AssessmentFlagVol_st_1000]", GetLocalResourceObject("LiteralResource142.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Dues_AssessmentFlagItem_st_1000]", GetLocalResourceObject("LiteralResource143.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Dues_AssessmentFlagVol_lt_1000]", GetLocalResourceObject("LiteralResource144.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_Dues_AssessmentFlagItem_lt_1000]", GetLocalResourceObject("LiteralResource145.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_MerchantPricingGrid]", GetLocalResourceObject("LiteralResource146.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_TieredDiscountGrid]", GetLocalResourceObject("LiteralResource147.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_ERRPercent]", GetLocalResourceObject("LiteralResource148.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_OtherVolumePercent]", GetLocalResourceObject("LiteralResource149.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_OtherItemRate]", GetLocalResourceObject("LiteralResource150.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_DiscountMethod]", GetLocalResourceObject("LiteralResource151.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_AmexOnePTRate]", GetLocalResourceObject("LiteralResource152.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_AmexOnePTPerItemFee]", GetLocalResourceObject("LiteralResource153.Text").ToString());

        StringBuilder strPaths = new StringBuilder();

        StringBuilder strTemplatePath = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_MERCHANT_CARD_PART)));

        if (CheckMerchantCardInformation("MC"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_MC);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWMC"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassMC"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateMC"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateMC"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateMC"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagMC"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagMC"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtVolLower1MC"].ToString());
            str.Replace("[DAItemLT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtItemLower1MC"].ToString());
            str.Replace("[DAVolGT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtVolGreater1MC"].ToString());
            str.Replace("[DAItemGT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtItemGreater1MC"].ToString());
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridMC"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridMC"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRMC"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeMC"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateMC"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodMC"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformation("MC DB"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_MCDB);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWMCDB"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassMCDB"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateMCDB"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateMCDB"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateMCDB"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagMCDB"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagMCDB"].ToString());

            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);

            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridMCDB"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridMCDB"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRMCDB"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeMCDB"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateMCDB"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodMCDB"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformation("VISA"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_VISA);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWVS"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassVS"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateVS"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateVS"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateVS"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagVS"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagVS"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridVS"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridVS"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRVS"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeVS"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateVS"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodVS"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("VISA DB"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_VISADB);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWVSDB"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassVSDB"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateVSDB"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateVSDB"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateVSDB"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagVSDB"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagVSDB"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridVSDB"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridVSDB"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRVSDB"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeVSDB"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateVSDB"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodVSDB"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("DISC FULL ACQ"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_DISCFULLACQ);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWDI"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassDIDB"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateDI"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateDI"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateDI"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagDI"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagDI"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridDI"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridDI"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRDI"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeDI"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateDI"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodDI"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("DISC FULL ACQ DB"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", GetLocalResourceObject("LiteralResource124.Text").ToString());
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWDIDB"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassDIDB"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateDIDB"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateDIDB"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateDIDB"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagDIDB"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagDIDB"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridDIDB"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridDIDB"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRDIDB"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeDIDB"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateDIDB"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodDIDB"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("DISC PASS THRU"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_DISCPASSTHRU);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWDIPassThru"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassDIPassThru"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateDIPassThru"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateDIPassThru"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateDIPassThru"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagDIPassThru"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagDIPassThru"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridDIPassThru"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridDIPassThru"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRDIPassThru"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeDIPassThru"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateDIPassThru"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodDIPassThru"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("AMEXONEPT"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_AMEXONEPT);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWAmexOnePT"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassAmexOnePT"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateAmexOnePT"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateAmexOnePT"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateAmexOnePT"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagAmexOnePT"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagAmexOnePT"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtVolLower1MC"].ToString());
            str.Replace("[DAItemLT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtItemLower1MC"].ToString());
            str.Replace("[DAVolGT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtVolGreater1MC"].ToString());
            str.Replace("[DAItemGT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtItemGreater1MC"].ToString());
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridAmexOnePT"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridAmexOnePT"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRAmexOnePT"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeAmexOnePT"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateAmexOnePT"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodAmexOnePT"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE + _datasource.Rows[0]["AmexOnePTRateAmexOnePT"].ToString());
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE + _datasource.Rows[0]["AmexOnePTPerItemFeeAmexOnePT"].ToString());
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("AMEX PASS THRU"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_AMEXPASSTHRU);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWAmesPassThru"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassAmesPassThru"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateAmesPassThru"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateAmesPassThru"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateAmesPassThru"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagAmesPassThru"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagAmesPassThru"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridAmesPassThru"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridAmesPassThru"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRAmesPassThru"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeAmesPassThru"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateAmesPassThru"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodAmesPassThru"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("AMEX"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_AMEX);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWAmexBlue"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassAmexBlue"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateAmexBlue"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateAmexBlue"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateAmexBlue"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagAmexBlue"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagAmexBlue"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridAmexBlue"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridAmexBlue"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRAmexBlue"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeAmexBlue"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateAmexBlue"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodAmexBlue"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("WRIGHT EXPRESS"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_WRIGHTEXPRESS);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWWrightExpress"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassWrightExpress"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateWrightExpress"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateWrightExpress"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateWrightExpress"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagWrightExpress"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagWrightExpress"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridWrightExpress"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridWrightExpress"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRWrightExpress"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeWrightExpress"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateWrightExpress"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodWrightExpress"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("VOYAGER"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_VOYAGER);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWVoyager"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassVoyager"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateVoyager"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateVoyager"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateVoyager"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagVoyager"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagVoyager"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtVolLower1Voyager"].ToString());
            str.Replace("[DAItemLT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtItemLower1Voyager"].ToString());
            str.Replace("[DAVolGT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtVolGreater1Voyager"].ToString());
            str.Replace("[DAItemGT1000]", EMPTY_VALUE + _datasource.Rows[0]["DueAsmtItemGreater1Voyager"].ToString());
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridVoyager"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridVoyager"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRVoyager"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeVoyager"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateVoyager"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodVoyager"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("GENERIC DEBIT"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_GENERICDEBIT);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWGenericDebit"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassGenericDebit"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateGenericDebit"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateGenericDebit"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateGenericDebit"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagGenericDebit"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagGenericDebit"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridGenericDebit"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridGenericDebit"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRGenericDebit"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeGenericDebit"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateGenericDebit"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodGenericDebit"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("DINERS"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_DINERS);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWDinner"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassDinner"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateDinner"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateDinner"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateDinner"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagDinner"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagDinner"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridDinner"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridDinner"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRDinner"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeDinner"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateDinner"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodDinner"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformation("EBT CASH/B"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_EBTCASH_B);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWEBT_CASH_B"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassEBT_CASH_B"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateEBT_CASH_B"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateEBT_CASH_B"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateEBT_CASH_B"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagEBT_CASH_B"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagEBT_CASH_B"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridEBT_CASH_B"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridEBT_CASH_B"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERREBT_CASH_B"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeEBT_CASH_B"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateEBT_CASH_B"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodEBT_CASH_B"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformation("EBT F/STMP"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_EBTF_STMP);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWEBT_F_STMP"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassEBT_F_STMP"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateEBT_F_STMP"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateEBT_F_STMP"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateEBT_F_STMP"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagEBT_F_STMP"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagEBT_F_STMP"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridEBT_F_STMP"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridEBT_F_STMP"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERREBT_F_STMP"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeEBT_F_STMP"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateEBT_F_STMP"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodEBT_F_STMP"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformation("EBT-TAPE"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_EBT_TAPE);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWEBT_TAPE"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassEBT_TAPE"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateEBT_TAPE"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateEBT_TAPE"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateEBT_TAPE"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagEBT_TAPE"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagEBT_TAPE"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridEBT_TAPE"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridEBT_TAPE"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERREBT_TAPE"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeEBT_TAPE"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateEBT_TAPE"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodEBT_TAPE"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformation("JCB"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[Card_Name]", Resources.Template.tpl_MerchantCardInformation_htm_JCB);
            str.Replace("[ProcSW]", EMPTY_VALUE + _datasource.Rows[0]["ProcessSWJCB"].ToString());
            str.Replace("[FeeClass]", EMPTY_VALUE + _datasource.Rows[0]["FeeClassJCB"].ToString());
            str.Replace("[QualRate]", EMPTY_VALUE + _datasource.Rows[0]["QualRateJCB"].ToString());
            str.Replace("[MidQualRate]", EMPTY_VALUE + _datasource.Rows[0]["MidQualRateJCB"].ToString());
            str.Replace("[NonQualRate]", EMPTY_VALUE + _datasource.Rows[0]["NonQualRateJCB"].ToString());
            str.Replace("[InterchangeFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["InterchangeFeeFlagJCB"].ToString());
            str.Replace("[DAFlag]", EMPTY_VALUE + _datasource.Rows[0]["DuesAssessmentFlagJCB"].ToString());
            str.Replace("[DAVolLT1000]", EMPTY_VALUE);
            str.Replace("[DAItemLT1000]", EMPTY_VALUE);
            str.Replace("[DAVolGT1000]", EMPTY_VALUE);
            str.Replace("[DAItemGT1000]", EMPTY_VALUE);
            str.Replace("[MerchantPricingGrid]", EMPTY_VALUE + _datasource.Rows[0]["MerchantPricingGridJCB"].ToString());
            str.Replace("[TieredDiscountGrid]", EMPTY_VALUE + _datasource.Rows[0]["TieredDiscountGridJCB"].ToString());
            str.Replace("[ERR]", EMPTY_VALUE + _datasource.Rows[0]["ERRJCB"].ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + _datasource.Rows[0]["OtherVolumeJCB"].ToString());
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateJCB"].ToString());
            str.Replace("[DiscountMethod]", EMPTY_VALUE + _datasource.Rows[0]["DiscountMethodJCB"].ToString());
            str.Replace("[AmexOnePTRate]", EMPTY_VALUE);
            str.Replace("[AmexOnePTPerItemFee]", EMPTY_VALUE);
            strPaths.Append(str.ToString());
        }

        strExcelTemplate.Replace("[CARD_DETAIL]", strPaths.ToString());

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_MerchantCardInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantCardPinDebitInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_MER_CARD_PIN_DEBIT_INFO)));

        StringBuilder strPaths = new StringBuilder();

        StringBuilder strTemplatePath = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_MER_CARD_PIN_DEBIT_PART)));

        if (CheckMerchantCardInformationPinDebit("ACCEL"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_ACCEL_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeACCEL"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateACCEL"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagACCEL"].ToString());
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformationPinDebit("AFFN"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_AFFN_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeAFFN"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateAFFN"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagAFFN"].ToString());
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformationPinDebit("ALASKA OPTION"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_ALASKAOPTION_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeALASKAOPTION"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateALASKAOPTION"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagAlaskaOpt"].ToString());
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformationPinDebit("CU24"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_CU24_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeCU24"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateCU24"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagCU24"].ToString());
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformationPinDebit("INTERLINK"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_INTERLINK_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeINTERLINK"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateINTERLINK"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagINTERLINK"].ToString());
            strPaths.Append(str.ToString());
        }

        if (CheckMerchantCardInformationPinDebit("JEANIE"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_JEANIE_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeJEANIE"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateJEANIE"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagJEANIE"].ToString());
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformationPinDebit("MAESTRO"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_MAESTRO_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeMAESTRO"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateMAESTRO"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagMAESTRO"].ToString());
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformationPinDebit("NYCE"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_NYCE_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeNYCE"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateNYCE"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagNYCE"].ToString());
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformationPinDebit("PULSE"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_PULSE_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumePULSE"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRatePULSE"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagPULSE"].ToString());
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformationPinDebit("SHAZAM"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_SHAZAM_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeSHAZAM"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateSHAZAM"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagSHAZAM"].ToString());
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformationPinDebit("STAR013"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_STAR_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeSTAR013"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateSTAR013"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagSTAR013"].ToString());
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformationPinDebit("STAR018"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_STAR_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeSTAR018"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateSTAR018"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagSTAR018"].ToString());
            strPaths.Append(str.ToString());
        }
        if (CheckMerchantCardInformationPinDebit("STAR021"))
        {
            StringBuilder str = new StringBuilder(strTemplatePath.ToString());
            str.Replace("[CardType]", GetLocalResourceObject("LiteralResourceMerchantCardInformation_STAR_Header.Text").ToString());
            str.Replace("[OtherVolume]", EMPTY_VALUE + (_datasource.Rows[0]["OtherVolumeSTAR021"]));
            str.Replace("[OtherItemRate]", EMPTY_VALUE + _datasource.Rows[0]["OtherItemRateSTAR021"].ToString());
            str.Replace("[OnlDbtFeeFlag]", EMPTY_VALUE + _datasource.Rows[0]["OnlDbtFeeFlagSTAR021"].ToString());
            strPaths.Append(str.ToString());
        }
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_MCI_PINDebit]", GetLocalResourceObject("LiteralResource154.Text").ToString());

        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_OtherVolumePercent]", GetLocalResourceObject("LiteralResource170.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardInformation_htm_OtherItemRate]", GetLocalResourceObject("LiteralResource171.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantCardPinDebitInformation_htm_OnlineDebitFeeFlag]", GetLocalResourceObject("LiteralResource172.Text").ToString());

        strExcelTemplate.Replace("[CARDTYPE_DETAIL]", strPaths.ToString());

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_MerchantCardInformation").ToString() +
           GetLocalResourceObject("MIF_MerchantDetails_FDRCS_Text_PINDebit").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantInformation_Click()
    {
        DataTable _datasource = (DataTable)(_MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;

        StringBuilder strExcelTemplate;

        strExcelTemplate = new StringBuilder(
            File.ReadAllText(Server.MapPath(FILE_NAME_MERCHANT_INFO)));

        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantInformation]", GetLocalResourceObject("LiteralResource1.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantNumber]", GetLocalResourceObject("LiteralResource2.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Address]", GetLocalResourceObject("LiteralResource8.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Contact]", GetLocalResourceObject("LiteralResource3.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantName]", GetLocalResourceObject("LiteralResource5.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_City_State_Zip]", GetLocalResourceObject("LiteralResource181.Text").ToString());

        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Email]", GetLocalResourceObject("LiteralResource9.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_ClientName]", GetLocalResourceObject("LiteralResource37.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_BankCard_City]", GetLocalResourceObject("LiteralResource182.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Phone]", GetLocalResourceObject("LiteralResource6.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_ReOpenDate]", GetLocalResourceObject("LiteralResource183.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_BankCard_State]", GetLocalResourceObject("LiteralResource184.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_LastBatchActivity]", GetLocalResourceObject("LiteralResource7.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_LastNonMonDate]", GetLocalResourceObject("LiteralResource185.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_RetailCity]", GetLocalResourceObject("LiteralResource186.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_ContactPhone]", GetLocalResourceObject("LiteralResource187.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_LastActiveDate]", GetLocalResourceObject("LiteralResource188.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_RetailState]", GetLocalResourceObject("LiteralResource189.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_LEGENNAME]", GetLocalResourceObject("LiteralResource190.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_LastReviewDate]", GetLocalResourceObject("LiteralResource191.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Status]", GetLocalResourceObject("LiteralResource4.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_CSPhone]", GetLocalResourceObject("LiteralResource192.Text").ToString());


        strExcelTemplate.Replace("HI_MERCHANT_NUMBER", EMPTY_VALUE + _datasource.Rows[0]["MerchantNumber"].ToString());
        strExcelTemplate.Replace("HI_ADDRESS", EMPTY_VALUE + BindAddress(_datasource.Rows[0]["Address1"].ToString(),
            _datasource.Rows[0]["Address2"].ToString(),
            _datasource.Rows[0]["Address3"].ToString(),
            _datasource.Rows[0]["City"].ToString(),
             _datasource.Rows[0]["State"].ToString(),
             _datasource.Rows[0]["Zip"].ToString()
            ));
        strExcelTemplate.Replace("HI_CONTACT", EMPTY_VALUE + _datasource.Rows[0]["Contact"].ToString());
        strExcelTemplate.Replace("HI_MERCHANT_NAME", EMPTY_VALUE + _datasource.Rows[0]["MerchantName"].ToString());
        strExcelTemplate.Replace("HI_CITY_STATE_ZIP", EMPTY_VALUE + _datasource.Rows[0]["City"].ToString() + "/" + _datasource.Rows[0]["State"].ToString() + "/" + _datasource.Rows[0]["Zip"].ToString());
        strExcelTemplate.Replace("HI_EMAIL", EMPTY_VALUE + _datasource.Rows[0]["Email"].ToString());
        strExcelTemplate.Replace("HI_CLIENTNAME", EMPTY_VALUE + _datasource.Rows[0]["ClientName"].ToString());
        strExcelTemplate.Replace("HI_BANKCARD_CITY", EMPTY_VALUE + _datasource.Rows[0]["BankcardCity"].ToString());
        strExcelTemplate.Replace("HI_PHONE", EMPTY_VALUE + FormatPhone(_datasource.Rows[0]["Phone"].ToString()));
        strExcelTemplate.Replace("HI_REOPENDATE", EMPTY_VALUE + _datasource.Rows[0]["ReOpenedDate"].ToString());
        strExcelTemplate.Replace("HI_BANKCARD_STATE", EMPTY_VALUE + _datasource.Rows[0]["BankcardState"].ToString());
        strExcelTemplate.Replace("HI_LASTBATCHACTIVITY", EMPTY_VALUE + FormatDate(_datasource.Rows[0]["LastActiveDate"].ToString()));
        strExcelTemplate.Replace("HI_LASTNONMONDATE", EMPTY_VALUE + _datasource.Rows[0]["LastNonMonDate"].ToString());
        strExcelTemplate.Replace("HI_RETAILCITY", EMPTY_VALUE + _datasource.Rows[0]["RetailCity"].ToString());
        strExcelTemplate.Replace("HI_CTPHONE", EMPTY_VALUE + _datasource.Rows[0]["ContactPhone"].ToString());
        strExcelTemplate.Replace("HI_LASTACTIVEDATE", EMPTY_VALUE + _datasource.Rows[0]["LastActiveDate"].ToString());
        strExcelTemplate.Replace("HI_RETAILSTATE", EMPTY_VALUE + _datasource.Rows[0]["RetailState"].ToString());

        strExcelTemplate.Replace("HI_LEGENNAME", EMPTY_VALUE + _datasource.Rows[0]["LegalName"].ToString());
        strExcelTemplate.Replace("HI_LASTREVIEWDATE", EMPTY_VALUE + _datasource.Rows[0]["LastReviewDate"].ToString());
        strExcelTemplate.Replace("HI_STATUS", EMPTY_VALUE + _datasource.Rows[0]["Status"].ToString());
        strExcelTemplate.Replace("HI_CSPHONE", EMPTY_VALUE + _datasource.Rows[0]["CSPhone"].ToString());
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

   

    private string GetMerchantNrParameter()
    {
        return ReportPage.ReportFilter.CurrentValue.Value;
    }

    #endregion Private Methods

    #endregion Methods
    
    protected void uxHierarchyInformation_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        HtmlAnchor linkSalesAgent = (HtmlAnchor)e.Item.FindControl("uxLinkHSalesAgent");
        HtmlAnchor linkHAgent = (HtmlAnchor)e.Item.FindControl("uxLinkHAgent");
        HtmlAnchor linkHHeadquaster = (HtmlAnchor)e.Item.FindControl("uxLinkHHeadquaster");
        HtmlAnchor linkChain = (HtmlAnchor)e.Item.FindControl("uxLinkChainCode");
        DataRowView row = e.Item.DataItem as DataRowView;
        string SalesAgentValue =  row["SalesAgent"].ToString();
        string SysPrinAgentValue = row["SysPrinAgent"].ToString();
        string HeadquarterValue = row["Headquarter"].ToString();
        string ChainCode = row["ChainCode"].ToString();

        if (linkSalesAgent != null && CheckHierarchy(HierarchyMode.CAYAN_SALESAGENT))
        {
            linkSalesAgent.Attributes.Add(
                               "onclick",
                               MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                   HierarchyMode.CAYAN_SALESAGENT, SalesAgentValue));
        }
        if (linkChain != null && CheckHierarchy(HierarchyMode.CAYAN_CHAINCODE))
        {
            linkChain.Attributes.Add(
                               "onclick",
                               MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                   HierarchyMode.CAYAN_CHAINCODE, ChainCode));
        }
        if (linkHAgent != null && CheckHierarchy(HierarchyMode.CAYAN_AGENT))
        {
            linkHAgent.Attributes.Add(
                              "onclick",
                              MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                  HierarchyMode.CAYAN_AGENT, SysPrinAgentValue));
        }
        if (linkHHeadquaster != null && CheckHierarchy(HierarchyMode.CAYAN_HEADQUARTER))
        {
            linkHHeadquaster.Attributes.Add(
                              "onclick",
                              MerchantProfileHelper.BuildSubmitReportFilterJavascriptCall(
                                  HierarchyMode.CAYAN_HEADQUARTER, HeadquarterValue));
        }


    }
    protected void uxBankInformation_ItemDataBound(AS.Controls.KeyValueTable sender, AS.Controls.KeyValueTableItemEventArgs args)
    {
        foreach (var row in sender.RowCollection)
        {
            //Routing number
            if (CheckRoutingAccess())
            {
                if (row.UniqueName == "PartialRoutingNumber")
                {
                    row.Visible = false;
                    continue;
                }
            }
            else
            {
                if (row.UniqueName == "RoutingNumber")
                {
                    row.Visible = false;
                    continue;
                }
            }

            // DDA number
            if (!CheckPermisson(WebSiteConstants.SEC_PERMISSION_DDA))
            {
                if (row.UniqueName == "DDANumber")
                {
                    row.DataField = "PartialDDANumber";
                    continue;
                }
            }
        }
    }
    protected void uxBusinessInformationCayan_ItemDataBound(AS.Controls.KeyValueTable sender, AS.Controls.KeyValueTableItemEventArgs args)
    {
        foreach (var row in sender.RowCollection)
        {
            //Tax
            if (!CheckPermisson(WebSiteConstants.SEC_PERMISSION_TAX_ID))
            {
                if (row.UniqueName == "TaxID")
                {
                    row.DataField = "PartialTaxID";
                    break;
                }
            }

        }
    }


    protected bool CheckRoutingAccess()
    {
        return SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS;
    }
    protected bool CheckPermisson(string permissionCode)
    {
        return SessionManager.CurrentUserViewMode > 0 && (Page.IsUserWithPermission(permissionCode) || Page.IsUserWithPermission("MS" + permissionCode));

    }
}