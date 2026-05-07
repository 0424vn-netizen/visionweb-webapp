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
using System.Web.UI.HtmlControls;

public partial class UserControls_MIF_MerchantDetails_ALLIEDWALLET : ExportMultiSections
{
    #region Enums

    protected enum DataBindAction
    {
        BindMerchantDetails,
        BindMerchantStatus,
        UpdateMerchantStatus,
    }

    protected enum PostBackAction
    {
        ViewAllWebsites
    }

    #endregion Enums

    #region Constants
    // Export file
    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation_ALLIEDWALLET.htm";
    private const string FILE_NAME_BUSINESS_INFO = "~/App_Data/tpl_BusinessInformation_ALLIEDWALLET.htm";

    // SPA
    private const string SPA_GET_MERCHANT_PROFILE = "spa_cs_GetMerchantProfile_AlliedWallet";
    private const string SPA_MERCHANT_STATUS_FOR_UPDATE = "spa_MerchantStatusForUpdate";
    private const string SPA_MERCHANT_UPDATE_STATUS = "spa_MerchantInformationUpdateStatus";


    #endregion Constants

    #region Fields

    private DataTable _merchantInfor;

    #endregion Fields

    #region Properties

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
    DataTable WebSiteTable
    {
        get
        {
            DataTable td = new DataTable();
            td.Columns.Add("BusinessWebSite", typeof(string));
            td.Columns.Add("RiskLevel", typeof(string));
            if (MifTable != null && MifTable.Rows.Count > 0)
            {
                DataRow row = td.NewRow();
                row[0] = MifTable.Rows[0]["BusinessWebSite"] == DBNull.Value || MifTable.Rows[0]["BusinessWebSite"].ToString().IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : MifTable.Rows[0]["BusinessWebSite"].ToString();
                row[1] = MifTable.Rows[0]["RiskLevel"] == DBNull.Value || MifTable.Rows[0]["RiskLevel"].ToString().IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : MifTable.Rows[0]["RiskLevel"].ToString();
                td.Rows.Add(row);

                for (int i = 2; i <= 10; i++)
                {
                    if (MifTable.Columns.Contains("BusinessWebSite" + i) && !MifTable.Rows[0]["BusinessWebSite" + i].IsNullData() && !string.IsNullOrEmpty(MifTable.Rows[0]["BusinessWebSite" + i].ToString()))
                    {
                        DataRow row2 = td.NewRow();
                        row2[0] = MifTable.Rows[0]["BusinessWebSite" + i];
                        td.Rows.Add(row2);
                    }
                }
            }
            return td;
        }
    }
    #endregion Properties

    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSwitchToUpdateStatus.Visible = Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_ADDNEWMERCHANT) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_OPEN_CASE);
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        base.OnDataBindControls(type, sender);
        FilterParameterCollection parameters = new FilterParameterCollection();
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMerchantDetails:
                parameters.Clear();
                parameters.AddLoggedInUserReportingParams();
                parameters.AddLoggedInUserPrimaryUserID();
                parameters.AddLanguageID();
                parameters.Add(new FilterParameter("@MerchantNumber", ReportPage.ReportFilter.CurrentValue.Value, DbType.AnsiString));
                parameters.Add(new FilterParameter("@FlagPermission", SessionManager.CurrentUserViewMode, DbType.Int32));
                parameters.AddDecryptDataParams("RoutingNumber");
                DataTable dtMerch = WebServices.CsReportServices.GetReports(SPA_GET_MERCHANT_PROFILE, parameters);

                if (dtMerch.Rows.Count > 0)
                {
                    MifTable = dtMerch;
                    phdMerchantDetail.Visible = true;
                    _merchantInfor = dtMerch;

                    if (dtMerch.Rows.Count > 0)
                    {
                        lbMerchantNumber.Text = BindValue("MerchantNumber");
                        lbAgentReferral.Text = BindValue("AgentReferral");
                        lbMerchantName.Text = BindValue("MerchantName");
                        lbResellerEmail.Text = BindValue("ResellerEmail");
                        lbResellerPhone.Text = BindValue("ResellerPhone");
                        lbResellerName.Text = BindValue("ResellerName");
                        uxCurrentStatus.Text = BindValue("Status");

                        if (dtMerch.Rows[0]["LastBatchActivity"] != DBNull.Value && !string.IsNullOrEmpty(dtMerch.Rows[0]["LastBatchActivity"].ToString()))
                        {
                            lnkLastBatch.Text = FormatDate(BindValue("LastBactchActivity"), true);

                            DateTime tempDate = new DateTime(1900, 1, 1);
                            DateTime.TryParse(FormatDate(dtMerch.Rows[0]["LastBactchActivity"]), out tempDate);
                            LastBatchDate = tempDate.Year == 1900 ? "0" : tempDate.Ticks.ToString(); 
                            lbLastBatchEmdash.Visible = false;
                            lnkLastBatch.Visible = true;

                        }
                        else
                        {
                            lbLastBatchEmdash.Visible = true;
                            lnkLastBatch.Visible = false;
                        }
                    }
                    uxWebSites.DataSource = WebSiteTable;
                    uxWebSites.DataBind();
                }
                else
                {
                    phdMerchantDetail.Visible = false;
                }
                break;
            case DataBindAction.BindMerchantStatus:
                //parameters.Clear();
                //parameters.AddLoggedInUserReportingParams();
                //parameters.AddLanguageID();
                // parameters.Add(new FilterParameter("@MerchantNumber", ReportPage.ReportFilter.CurrentValue.Value, DbType.AnsiString));
                // uxcbbStatus.DataTextField = "Description";
                //uxcbbStatus.DataValueField = "Status";
                //uxcbbStatus.DataSource = WebServices.CsReportServices.GetReports(SPA_MERCHANT_STATUS_FOR_UPDATE, parameters);
                //uxcbbStatus.DataBind();
                break;
            case DataBindAction.UpdateMerchantStatus:
                // parameters.Clear();
                // parameters.AddLoggedInUserReportingParams();
                // parameters.AddLanguageID();
                // parameters.Add(new FilterParameter("@MerchantNumber", ReportPage.ReportFilter.CurrentValue.Value, DbType.AnsiString));
                // parameters.Add(new FilterParameter("@Status", uxcbbStatus.SelectedValue.ToString(), DbType.AnsiString));
                // WebServices.CsReportServices.ExecuteNonQueryCommand(SPA_MERCHANT_UPDATE_STATUS, parameters, out parameters);
                //// uxCurrentStatus.Text = uxcbbStatus.SelectedItem.Text;
                // uxCurrentStatus.Visible = true;
                // uxPanelViewStatus.Visible = true;
                ////z uxPanelUpdateStatus.Visible = false;

                // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "rebindMerchantNote", "triggerApplyFilter()", true);
                break;
        }
    }

    protected void lnkLastBatch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ReportPage.ReportFilter.CurrentValue.Value))
        {
            if (Page.IsSecureQueryString && Page.SecureQueryString["MerchantNumber"] != null)
            {
                Response.Redirect(GeneralFuncsLib.BuildBatchHistoryUrl(
                   (SecurePage)Page, LastBatchDate, Page.SecureQueryString["MerchantNumber"]));
            }
        }
        else
        {
            Response.Redirect(GeneralFuncsLib.BuildBatchHistoryUrl(
                   (SecurePage)Page, LastBatchDate, ReportPage.ReportFilter.CurrentValue.Value));
        }
    }

    protected string FormatDateForExport(object obj)
    {
        if (obj.IsNullOrEmpty())
        {
            return string.Empty;
        }
        return ((DateTime)obj).ToGenericDateString();

    }

    public string GetRoutingNumber(string full, string partial)
    {
        return MerchantProfileHelper.GetRoutingNumberWithoutDecrypt(FormatDataToMDash(full), FormatDataToMDash(partial));
    }

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

        uxPanelViewStatus.Visible = true;
        // uxPanelUpdateStatus.Visible = false;
    }

    protected override Dictionary<int, Func<string>> GetListFunctions()
    {
        Dictionary<int, Func<string>> exportFucntions = new Dictionary<int, Func<string>>();
        exportFucntions.Add(0, ButtonExcelMerchantInformation_Click);
        exportFucntions.Add(1, ButtonExcelBusinessInformation_Click);
        var uxRiskInfo2 = rptMerchantInfo.FindControl("uxRiskInfo") as UserControls_MIF_RiskInformationSection;
        if (uxRiskInfo2 != null)
            exportFucntions.Add(2, () => { return uxRiskInfo2.ExcelRiskInformation(); });

        return exportFucntions;
    }

    public void DoMultiExportExcel()
    {
        Dictionary<int, string> exportNames = new Dictionary<int, string>();
        exportNames.Add(0, GetLocalResourceObject("MIF_MerchantDetails_Info").ToString());
        exportNames.Add(1, GetLocalResourceObject("MIF_MerchantDetails_BusinessInformation").ToString());
        exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_Text_RiskInformation").ToString());
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
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_OpenDate]", GetLocalResourceObject("LiteralResource8.Text").ToString());
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ClosedDate]", GetLocalResourceObject("LiteralResource1ClosedDate.Text").ToString());

        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SICMSS]", GetLocalResourceObject("LiteralResourceSICMCC.Text").ToString());
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_MerchantCorporateLegalName]", GetLocalResourceObject("LiteralResourceMerchantCorporateLegalName.Text").ToString());
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessType]", GetLocalResourceObject("LiteralResourceBusinessType.Text").ToString());
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Country]", GetLocalResourceObject("LiteralResourceCountry.Text").ToString());
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessRegistration]", GetLocalResourceObject("LiteralResourceBusinessRegistration.Text").ToString());
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessAddress]", GetLocalResourceObject("LiteralResourceBusinessAddress.Text").ToString());
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ProductSold]", GetLocalResourceObject("LiteralResourceProductSold.Text").ToString());
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessEmail]", GetLocalResourceObject("LiteralResourceBusinessEmail.Text").ToString());


        strExcelTemplate.Replace("BI_OPEN_DATE", "&nbsp;" + FormatDateForExport(dataSource.Rows[0]["OpenDate"]));
        strExcelTemplate.Replace("BI_CLOSED_DATE", "&nbsp;" + FormatDataToMDash(FormatDateForExport(dataSource.Rows[0]["ClosedDate"])));
        strExcelTemplate.Replace("BI_SICMCC", "&nbsp;" + FormatDataToMDash(string.Format("{0} - {1}", dataSource.Rows[0]["SICMCC"], dataSource.Rows[0]["SICDescription"])));
        strExcelTemplate.Replace("BI_COMPANYNAME", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["CompanyName"]));
        strExcelTemplate.Replace("BI_BUSINESSTYPE", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["BusinessTypeNotes"]));
        strExcelTemplate.Replace("BI_COUNTRY", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["CountryName"]));
        strExcelTemplate.Replace("BI_BUSINESSREGISTRATION", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["BusinessRegistration"]));

        string businessAddress = BindAddress(BindValueNoEMDash("Line1_Business"), BindValueNoEMDash("Line2_Business"), BindValueNoEMDash("Address3"), BindValueNoEMDash("City"), BindValueNoEMDash("State"), BindValueNoEMDash("Zip"));
        strExcelTemplate.Replace("BI_BUSINESS_ADDRESS", "&nbsp;" + FormatDataToMDash(businessAddress));
        strExcelTemplate.Replace("BI_PRODUCT_SOLD", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["ProductSold"]));
        strExcelTemplate.Replace("BI_BUSINESS_EMAIL", "&nbsp;" + FormatDataToMDash(dataSource.Rows[0]["BusinessEmail"]));

        StringBuilder strwebSite = new StringBuilder();
        int rowTotal = WebSiteTable.Rows.Count;
        for (int i = 0; i < rowTotal; i++)
        {
            if (i == 0)
            {
                strwebSite.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                    string.Format(GetLocalResourceObject("LiteralResourceBusinessWebsite").ToString(), i == 0 ? ":" : " " + (i + 1) + ":"), "&nbsp;" + WebSiteTable.Rows[i]["BusinessWebsite"].ToString(),
                    GetLocalResourceObject("MIF_RiskLevel").ToString(), "&nbsp;" + WebSiteTable.Rows[i]["RiskLevel"].ToString());
            }
            else
            {
                strwebSite.AppendFormat("<tr><td>{0}</td><td>{1}</td><td></td><td></td></tr>", string.Format(GetLocalResourceObject("LiteralResourceBusinessWebsite").ToString(), i == 0 ? ":" : " " + (i + 1) + ":"), "&nbsp;" + WebSiteTable.Rows[i]["BusinessWebsite"].ToString());
            }
        }
        strExcelTemplate.Replace("[BI_BUSINESSWEBSITES]", "&nbsp;" + strwebSite.ToString());

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_BusinessInformation").ToString());
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
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_Status]", Resources.Template.tpl_MerchantInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MerchantName]", Resources.Template.tpl_MerchantInformation_htm_MerchantName);
        strExcelTemplate.Replace("[tpl_MerchantInformation_html_ResellerName]", GetLocalResourceObject("LiteralResourceResellerName.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_html_ResellerPhone]", GetLocalResourceObject("LiteralResourceResellerPhone.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_html_ResellerEmail]", GetLocalResourceObject("LiteralResourceResellerEmail.Text").ToString());
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_AgentReferral]", GetLocalResourceObject("LiteralResourceAgentReferral.Text").ToString());

        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_LastBatchActivity]", GetLocalResourceObject("lnkLastBatchResource.Text").ToString());



        strExcelTemplate.Replace("HI_MERCHANT_NUMBER", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["MerchantNumber"].ToString()));
        strExcelTemplate.Replace("HI_STATUS", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["Status"].ToString()));
        strExcelTemplate.Replace("HI_MERCHANT_NAME", "&nbsp;" + FormatDataToMDash(_datasource.Rows[0]["MerchantName"].ToString()));
        strExcelTemplate.Replace("HI_RESELLER_NAME", "&nbsp;" + FormatPhone(_datasource.Rows[0]["ResellerName"], true));

        strExcelTemplate.Replace("HI_RESELLER_PHONE", "&nbsp;" + FormatPhone(_datasource.Rows[0]["ResellerPhone"], true));
        strExcelTemplate.Replace("HI_RESELLER_EMAIL", "&nbsp;" + FormatPhone(_datasource.Rows[0]["ResellerEmail"], true));
        strExcelTemplate.Replace("HI_MERCHANT_AGENTREFERRAL", "&nbsp;" + FormatPhone(_datasource.Rows[0]["AgentReferral"], true));
        strExcelTemplate.Replace("HI_LASTBATCHACTIVITY", "&nbsp;" + FormatDataToMDash(FormatDate(_datasource.Rows[0]["LastBatchActivity"].ToString(), true)));

        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_Info").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    #region Processing Method

    public string BindValue(string colName)
    {
        if (_merchantInfor == null || _merchantInfor.Rows.Count <= 0)
            return WebSiteConstants.HTML_EM_DASH_ENCODE;
        DataRow dr = _merchantInfor.Rows[0];
        return dr[colName] == DBNull.Value || dr[colName].ToString().IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : dr[colName].ToString();
    }
    public object BindValueAsObject(string colName)
    {
        if (_merchantInfor == null || _merchantInfor.Rows.Count <= 0)
            return WebSiteConstants.HTML_EM_DASH_ENCODE;
        DataRow dr = _merchantInfor.Rows[0];
        return dr[colName] == DBNull.Value || dr[colName].ToString().IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : dr[colName];
    }



    public string BindValueNoEMDash(string colName)
    {
        return (_merchantInfor == null || _merchantInfor.Rows.Count <= 0)
            ? string.Empty : _merchantInfor.Rows[0][colName].ToString();
    }

    protected string BindAddress(object add1, object add2, object add3, object city, object state, object zip, bool? isExport = false)
    {
        string em_dash = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        string address = MerchantProfileHelper.BindAddress(add1, add2, add3, city, state, zip);
        return address.IsNullOrEmpty() ? em_dash : address;
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

    protected string FormatDate(object data)
    {
        if (data is DateTime)
        {
            return (DateTime.Parse(data.ToString())).ToString(WebSiteConstants.DATE_FORMAT);
        }
        return data.ToString();
    }
    protected string FormatDate(object data, bool converttoDateTime = false)
    {
        if (!converttoDateTime)
        {
            if (data is DateTime)
            {
                return (DateTime.Parse(data.ToString())).ToString(WebSiteConstants.DATE_FORMAT);
            }
            return data.ToString();
        }
        else
        {
            DateTime d = new DateTime();
            if (DateTime.TryParse(data.ToString(), out d))
            {
                return d.ToString(WebSiteConstants.DATE_FORMAT);
            }
            return string.Empty;
        }
    }



    protected string FormatPhone(object phone, bool? isExport = false)
    {
        string em_dash = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        return phone.IsNullOrEmpty() ? em_dash : AS.Common.Formater.FormatData.FormatPhoneNumber(phone.ToString());
    }

    protected bool CheckHierarchy(string hierarchy)
    {
        return MerchantProfileHelper.CheckHierarchy(hierarchy);
    }

    protected string FormatSIC(object sic, object sicDesc, bool? isExport = false)
    {
        string em_dash = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        string strSic = MerchantProfileHelper.FormatSIC(sic, sicDesc);
        return strSic.IsNullOrEmpty() ? em_dash : strSic;
    }

    protected bool HasMSProductEnvironment()
    {
        return GeneralFuncsLib.HasMSProductEnvironment();
    }

    protected string CheckPermisson(object obj, string permissionCode)
    {
        string data = MerchantProfileHelper.CheckPermisson(obj, permissionCode, Page);
        return data.IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : data;
    }

    private string FormatDataToMDash(object value)
    {
        return value.IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH : value.ToString();
    }

    #endregion





    #endregion Methods
    protected void btnSwitchToUpdateStatus_Click(object sender, EventArgs e)
    {
        uxPanelViewStatus.Visible = false;
        // uxPanelUpdateStatus.Visible = true;
        this.OnDataBindControls(DataBindAction.BindMerchantStatus, this);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        uxPanelViewStatus.Visible = true;
        // uxPanelUpdateStatus.Visible = false;
    }
    protected void btnUpdateStatus_Click(object sender, EventArgs e)
    {
        this.OnDataBindControls(DataBindAction.UpdateMerchantStatus, this);

    }
    protected void uxWebSites_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        switch (e.Item.ItemType)
        {
            case ListItemType.Item:
            case ListItemType.AlternatingItem:
                {
                    DataRowView row = e.Item.DataItem as DataRowView;
                    Literal lbBusinessWebsite = ((Literal)e.Item.FindControl("lbBusinessWebsite"));
                    Literal uxBusinessWebsite = ((Literal)e.Item.FindControl("uxBusinessWebsite"));
                    LinkButton uxBtnViewWebSites = ((LinkButton)e.Item.FindControl("uxLinkButtonViewWebSites"));
                    Literal uxlbRiskLevel = ((Literal)e.Item.FindControl("uxlbRiskLevel"));
                    Literal uxRiskLevel = ((Literal)e.Item.FindControl("uxRiskLevel"));
                    HtmlTableRow tr = ((HtmlTableRow)e.Item.FindControl("uxwebSiteRow"));
                    if (tr != null && e.Item.ItemIndex % 2 == 0)
                    {
                        tr.Attributes["class"] = "AltRow";
                    }
                    if (tr != null && e.Item.ItemIndex % 2 != 0)
                    {
                        tr.Attributes["class"] = "Row";
                    }
                    int index = e.Item.ItemIndex + 1;
                    lbBusinessWebsite.Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("BusinessWebsites").ToString());
                    uxBusinessWebsite.Text = VeraCodeSolution.ValidateResponseData(row["BusinessWebsite"].ToString());
                    if (Page.IsUserWithPermission("ViewWebsites"))
                    {
                        uxBtnViewWebSites.Visible = true;
                    }
                    else
                    {
                        uxBtnViewWebSites.Visible = false;
                    }
                    uxRiskLevel.Text = VeraCodeSolution.DoVeraCode(row["RiskLevel"].ToString());

                }
                break;
        }
    }
}