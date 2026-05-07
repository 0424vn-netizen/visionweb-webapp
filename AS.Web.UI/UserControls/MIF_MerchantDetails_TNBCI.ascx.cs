using System;
using System.Data;
using System.IO;
using System.Text;
using AS.Common;
using AS.Common.DBManager;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Configuration;

public partial class UserControls_MIF_MerchantDetails_TNBCI : ExportMultiSections
{
    #region Constants

    // SPA Name
    private const string SPA_GET_MERCHANT_PROFILE = "spa_cs_GetMerchantProfile_TOTAL";
    private const string SPA_UPDATE_RELATIONSHIP_MANAGER = "spa_UpdateRelationshipManager";

    // Path
    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string PATH_BANK_INFO_TPL_FILE = "~/App_Data/tpl_BankInformation_TNBCI.htm";
    private const string PATH_HI_TPL_FILE = "~/App_Data/tpl_HierarchyInformation_TNBCI.htm";
    private const string PATH_BI_TPL_FILE = "~/App_Data/tpl_BusinessInformation_TNBCI.htm";
    private const string FILE_NAME_MERCHANT_INFO = "~/App_Data/tpl_MerchantInformation.htm";
    private const string FILE_NAME_MERCHANT_INFO_REP = "~/App_Data/tpl_MerchantInformation_ORI_Rep.htm";
    // Status
    private const string MERCHANT_STATUS_DEACTIVATED = "Deactivated";
    private const string MERCHANT_STATUS_CLOSED = "Closed";

    // Permission
    private const string PER_RELATIONSHIP_MANAGER = "RelationshipMannager";
    private const string PER_MS_RELATIONSHIP_MANAGER = "MSRelationshipMannager";

    private const string OPTED_IN = "Opted In";

    #endregion Constants

    #region Fields

    private bool _isCaseManagement = false;
    private string[] noprefix = { "mr ", "ms ", "sir ", "jr ", "mr.", "ms.", "sir.", "jr." };
    private bool isShowRelationshipManager =
        GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RELATIONSHIP_MANAGER").Equals("true") ? true : false;
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
        parameters.Add(new FilterParameter("@ASClient",
            SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new FilterParameter("@MerchantNumber",
            IsCaseManagement ? Page.SecureQueryString["MerchantNumber"]
                             : ReportPage.ReportFilter.CurrentValue.Value,
            DbType.AnsiString));
        parameters.Add(new FilterParameter("@RelationshipManager",
            uxtxtRelationShipManager.Text.Trim(),
            DbType.String));
        WebServices.CsReportServices.ExecuteNonQueryCommand(
            SPA_UPDATE_RELATIONSHIP_MANAGER, parameters, out parameters);

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
        {
            return string.Empty;
        }
        return _merchantInfor.Rows[0][colName].ToString();
    }

    protected void lnkLastBatch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(MerchantNumber))
        {
            if (Page.SecureQueryString["MerchantNumber"] != null)
            {
                Response.Redirect("~/BatchHistory.aspx?" + Page.BuildSecureQueryString(
                    string.Format("date={0}&merchantnumber={1}",
                                  LastBatchDate,
                                  Page.SecureQueryString["MerchantNumber"])));
            }
        }
        else
        {
            Response.Redirect("~/BatchHistory.aspx?" + Page.BuildSecureQueryString(
                string.Format("date={0}&merchantnumber={1}",
                              LastBatchDate, MerchantNumber)));
        }
    }

    protected string SetURLForSiteAccessButton()
    {
        return "return ShowPopupModal('MerchantProfileModal.aspx?"
            + Page.BuildSecureQueryString(
                string.Format("merchant={0}&m={1}&e={2}",
                              MerchantNumber, OptedIn, MifEmail))
            + "', 'auto')";
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
                parameters.Add(new FilterParameter("@FlagPermission",
                    SessionManager.CurrentUserViewMode, DbType.Int32));
                parameters.Add(new FilterParameter("@MerchantNumber",
                    IsCaseManagement ? Page.SecureQueryString["MerchantNumber"]
                                     : ReportPage.ReportFilter.CurrentValue.Value,
                    DbType.AnsiString));
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

                    ShowHideRelationshipManager();

                    _merchantInfor = dtMerch;
                    lnkLastBatch.Text = FormatDate2(BindValue("LastBactchActivity"));
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
        return row == null ? string.Empty : FormatDate(row[key]);
    }

    protected bool setButton(object MerchantStatusDesc)
    {
        if (MerchantStatusDesc.ToString() != string.Empty)
        {
            return !MerchantStatusDesc.ToString().ToLower().Equals("closed");
        }
        return false;
    }

    protected string setOptInStatus(object OptIn, string MerchantStatus)
    {
        if (OptIn.ToString() != string.Empty)
        {
            if (OptIn.ToString().Equals("1"))
            {
                return "Opted Out";
            }
            else if (OptIn.ToString().Equals("0"))
            {
                return "Opted In";
            }
            if (MerchantStatus == MERCHANT_STATUS_DEACTIVATED
                || MerchantStatus == MERCHANT_STATUS_CLOSED)
            {
                return MerchantStatus;
            }
        }
        return string.Empty;
    }

    public string GetRoutingNumber(string full, string partial)
    {
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
        {
            return WebServices.CsReportServices.DecryptText(full,
                SessionManager.CurrentUser.ASClient);
        }
        else
        {
            return partial;
        }
    }

    public string GetHierarchyNumber(string strInput, bool isChain)
    {
        if (strInput.Length >= 4)
        {
            strInput = isChain ? strInput.Remove(0, 3) : strInput.Remove(0, 4);
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
        exportNames.Add(1, GetLocalResourceObject("MIF_MerchantDetails_TNBCICS_Text_BusinessInformation").ToString());

        if (IsRiskInformation)
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_Text_RiskInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_TNBCICS_Text_HierarchyInformation").ToString());
            exportNames.Add(4, GetLocalResourceObject("MIF_MerchantDetails_TNBCICS_Text_BankInformation").ToString());
        }
        else
        {
            exportNames.Add(2, GetLocalResourceObject("MIF_MerchantDetails_TNBCICS_Text_HierarchyInformation").ToString());
            exportNames.Add(3, GetLocalResourceObject("MIF_MerchantDetails_TNBCICS_Text_BankInformation").ToString());
        }
        ExportSectionNames = exportNames;
    }

    #region Processing Methods

    protected string BindAddress(object add1, object add2, object add3,
        object city, object state, object zip)
    {
        add1 = ProcessNullValue(add1);
        add2 = ProcessNullValue(add2);
        add3 = ProcessNullValue(add3);
        city = ProcessNullValue(city);
        state = ProcessNullValue(state);
        zip = ProcessNullValue(zip);
        string address1 = string.IsNullOrEmpty((string)add1) ? string.Empty : (add1 + "<br />");
        string address2 = string.IsNullOrEmpty((string)add2) ? string.Empty : (add2 + "<br />");
        if (!string.IsNullOrEmpty((string)add3))
        {
            return address1 + address2 + add3;
        }
        else
        {
            string cityAddress = string.IsNullOrEmpty((string)city)
                ? string.Empty : (city + ", ");
            return address1 + address2 + cityAddress + state + " " + zip;
        }
    }

    private object ProcessNullValue(object obj)
    {
        return obj.GetType() == typeof(DBNull) ? null : obj;
    }

    protected string FormatCurrency(object abc)
    {
        return abc == DBNull.Value ? string.Empty : GeneralFuncsLib.FormatCurrency(abc);
    }

    protected string FormatCurrency(object abc, int count)
    {
        return abc == DBNull.Value ? string.Empty : GeneralFuncsLib.FormatCurrency(abc);
    }

    protected string FormatDate(object dt)
    {
        dt = ProcessNullValue(dt);
        return GeneralFuncsLib.FormatDate(dt);
    }

    /// <summary>
    /// Return MM/dd/yyyy
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    protected string FormatDate2(object dt)
    {
        dt = ProcessNullValue(dt);

        if (dt == null)
            return string.Empty;
        if (dt.ToString() == string.Empty)
            return string.Empty;
        return Convert.ToDateTime(dt).ToString("MM/dd/yyyy");
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
                                  LastBatchDate,
                                  MerchantNumber)));
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
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(PATH_BI_TPL_FILE)));
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_BusinessInformation]", Resources.Template.tpl_BusinessInformation_htm_BusinessInformation);
        strExcelTemplate.Replace("[tpl_BusinessInformation_ORION_htm_ApprovalDate]", Resources.Template.tpl_BusinessInformation_ORION_htm_ApprovalDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_ClosedDate]", Resources.Template.tpl_BusinessInformation_htm_ClosedDate);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_Status]", Resources.Template.tpl_BusinessInformation_htm_Status);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_TaxID]", Resources.Template.tpl_BusinessInformation_htm_TaxID);
        strExcelTemplate.Replace("[tpl_BusinessInformation_htm_SICMCC]", Resources.Template.tpl_BusinessInformation_htm_SICMCC);
        strExcelTemplate.Replace("[tpl_BusinessInformation_EMS_htm_AverageSalesAmount]", Resources.Template.tpl_BusinessInformation_EMS_htm_AverageSalesAmount);
        strExcelTemplate.Replace("BI_APPROVAL_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["ApprovalDate"]));
        strExcelTemplate.Replace("BI_CLOSE_DATE", "&nbsp;" + FormatDate(dataSource.Rows[0]["ClosedDate"]));
        strExcelTemplate.Replace("BI_STATUS", "&nbsp;" + dataSource.Rows[0]["ActivityStatus"].ToString());
        strExcelTemplate.Replace("BI_TAX_ID", "&nbsp;" + dataSource.Rows[0]["PartialTaxID"].ToString());
        strExcelTemplate.Replace("BI_SIC_MCC", "&nbsp;"
            + FormatSIC(dataSource.Rows[0]["SICCode"].ToString(), dataSource.Rows[0]["SICCodeDesc"].ToString()));
        strExcelTemplate.Replace("BI_AVER_SALES_AMT", "&nbsp;" + FormatCurrency(dataSource.Rows[0]["Avg_Ticket_Amt"]));
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_TNBCICS_Text_BusinessInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelHierarchyInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(PATH_HI_TPL_FILE)));
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_HierarchyInformation]", Resources.Template.tpl_HierarchyInformation_htm_HierarchyInformation);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientName]", Resources.Template.tpl_HierarchyInformation_htm_ClientName);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_htm_ClientLogin]", Resources.Template.tpl_HierarchyInformation_htm_ClientLogin);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_Fulton_htm_Corporate]", Resources.Template.tpl_HierarchyInformation_Fulton_htm_Corporate);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_Fulton_htm_Region]", Resources.Template.tpl_HierarchyInformation_Fulton_htm_Region);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_TNBCI_htm_Principal]", Resources.Template.tpl_HierarchyInformation_TNBCI_htm_Principal);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_Fulton_htm_Association]", Resources.Template.tpl_HierarchyInformation_Fulton_htm_Association);
        strExcelTemplate.Replace("[tpl_HierarchyInformation_FIS_htm_Chain]", Resources.Template.tpl_HierarchyInformation_FIS_htm_Chain);
        strExcelTemplate.Replace("HI_CLIENT_NAME", "&nbsp;" + _datasource.Rows[0]["ClientName"].ToString());
        strExcelTemplate.Replace("HI_CLIENT_LOGIN", "&nbsp;" + _datasource.Rows[0]["ClientLogin"].ToString());
        strExcelTemplate.Replace("HI_CORPORATE", "&nbsp;" + _datasource.Rows[0]["Corporate"].ToString());
        strExcelTemplate.Replace("HI_REGION", "&nbsp;" + _datasource.Rows[0]["Region"].ToString());
        strExcelTemplate.Replace("HI_PRINCIPAL", "&nbsp;" + _datasource.Rows[0]["Principal"].ToString());
        strExcelTemplate.Replace("HI_ASSOCIATION", "&nbsp;" + _datasource.Rows[0]["Association"].ToString());
        strExcelTemplate.Replace("HI_CHAIN_CODE", "&nbsp;" + _datasource.Rows[0]["Chain"].ToString());
        
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_TNBCICS_Text_HierarchyInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelBankInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(PATH_BANK_INFO_TPL_FILE)));
        strExcelTemplate.Replace("[tpl_BankInformation_htm_BankInformation]", Resources.Template.tpl_BankInformation_htm_BankInformation);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_RoutingSharp]", Resources.Template.tpl_BankInformation_htm_RoutingSharp);
        strExcelTemplate.Replace("[tpl_BankInformation_htm_DDASharp]", Resources.Template.tpl_BankInformation_htm_DDASharp);
        strExcelTemplate.Replace("BI_ROUTING_#", "&nbsp;" + _datasource.Rows[0]["PartialRoutingNumber"].ToString());
        strExcelTemplate.Replace("BI_DDA_#", "&nbsp;" + _datasource.Rows[0]["PartialDDANumber"].ToString());
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("MIF_MerchantDetails_TNBCICS_Text_BankInformation").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

    protected string ButtonExcelMerchantInformation_Click()
    {
        DataTable _datasource = (DataTable)(MifTable);
        if (_datasource == null || _datasource.Rows.Count == 0)
            return string.Empty;
        StringBuilder strExcelTemplate;
        if (pnlRelationshipmanager.Visible)
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
        obj = ProcessNullValue(obj);
        string tempStr = string.Empty;
        if (String.Compare((string)obj, "opted in", true) == 0)
        {
            tempStr = "Opt Out";
        }
        else if (String.Compare((string)obj, "opted out", true) == 0)
        {
            tempStr = "Opt In";
        }
        return tempStr;
    }

    protected bool HasMSProductEnvironment()
    {
        return GeneralFuncsLib.GetClientExtendedSetting(WebSiteConstants.PRODUCT_ENVIRONMENT_KEY)
            .Data.Contains(WebSiteEnums.ProductEnvironment.MS.ToString());
    }

    protected bool SetVisible(object obj)
    {
        if (!HasMSProductEnvironment())
        {
            return false;
        }
        return ((Page.IsUserWithPermission("OptInOut")
                || Page.IsUserWithPermission("MSOptInOut"))
            && (String.Compare(obj.ToString(), "opted in", true) == 0
                || String.Compare(obj.ToString(), "opted out", true) == 0));
    }

    protected string CheckPermisson(object obj, string permissionCode)
    {
        if (obj == null)
            return string.Empty;
        TempStr = obj.ToString();
        if (String.IsNullOrEmpty(this.TempStr))
            return string.Empty;
        if (SessionManager.CurrentUserViewMode > 0)
        {
            if (Page.IsUserWithPermission(permissionCode)
                || Page.IsUserWithPermission("MS" + permissionCode))
            {
                TempStr = WebServices.CsReportServices.DecryptText(
                    TempStr, SessionManager.CurrentUser.ASClient);
            }
            return TempStr;
        }
        return TempStr;
    }

    protected bool CheckPermissionAddEditChain(object siteAccess)
    {
        return SessionManager.CurrentUserPermissions.Contains("AddEditChain")
            && GeneralFuncsLib.SiteAccessIsOptInOut(siteAccess);
    }

    protected bool CheckHierarchy(string hierarchy)
    {
        DataTable hrc = SessionManager.HierarchyFilter;
        if (hrc != null && hrc.Rows.Count > 0)
        {
            for (int i = 0; i < hrc.Rows.Count; i++)
            {
                if (hrc.Rows[i]["HierarchyMode"].ToString().ToUpper() == hierarchy.ToUpper())
                    return true;
            }
        }
        return false;
    }

    protected string FormatSIC(object sic, object sicDesc)
    {
        sic = ProcessNullValue(sic);
        return sic == null ? sicDesc.ToString()
            : string.Format("{0} - {1}", sic.ToString(), sicDesc.ToString());
    }

    #endregion

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

    #region Private Methods

    private void ShowHideRelationshipManager()
    {
        pnlRelationshipmanager.Visible = isShowRelationshipManager;
        uxlbRelationshipManager.Text = string.Empty;
        if (isShowRelationshipManager)
        {
            uxpnlRMLabel.Visible = true;
            uxpnlRMCtrls.Visible = true;
            if (string.IsNullOrEmpty(RelationShipManager))
            {
                if (((ReportPage)this.Page).IsUserWithPermission(PER_RELATIONSHIP_MANAGER)
                    || ((ReportPage)this.Page).IsUserWithPermission(PER_MS_RELATIONSHIP_MANAGER))
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
                if (((ReportPage)this.Page).IsUserWithPermission(PER_RELATIONSHIP_MANAGER)
                    || ((ReportPage)this.Page).IsUserWithPermission(PER_MS_RELATIONSHIP_MANAGER))
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
                        && GeneralFuncsLib.SiteAccessIsOptInOut(row["SiteAccess"]))
                    {
                        plChain.Visible = false;
                        plChainNew.Visible = true;
                        ltChain.Visible = false;
                    }
                    else
                    {
                        plChainNew.Visible = false;
                        if (CheckHierarchy("CHAIN"))
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

    #endregion Private Methods

    #endregion Methods
}
