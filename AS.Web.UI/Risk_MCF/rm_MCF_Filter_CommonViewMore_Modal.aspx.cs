using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Text;

[PagePermission("RskManAss,RskAdhoc,MSRskManAss,MSRskAdhoc")]
public partial class rm_MCF_Filter_CommonViewMore_Modal : NonReportPage, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        LoadHierarchys = 1,
        LoadStates = 2,
        LoadZips = 4,
        LoadProfiles = 5,
        LoadMarketDatas = 6,
        LoadIncludeExcludeISONumbers = 7,
        LoadHighRisk = 8,
        LoadMerchantClassifications = 9,
        LoadLeadSource = 10,
        LoadReferralSource = 11,
        LoadRiskLevel = 12,
        LoadRiskCategory = 13,
        LoadCampaignID = 14
    }
    enum PostBackAction { }
    #endregion

    public int FilterTypeModal { get; set; }
    public string HierarchyFilterMode { get; set; }

    private string RiskLevel = "RiskLevel";
    private string RiskCategory = "RiskCategory";
    private string CampaignID = "CampaignID";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        if (SecureQueryString != null && SecureQueryString["mode"] != null)
            Mode = (WebSiteEnums.ParamFilterMode)Int16.Parse(SecureQueryString["mode"]);
        if (SecureQueryString != null && SecureQueryString["primaryid"] != null)
            PrimaryID = SecureQueryString["primaryid"];
        if (SecureQueryString != null && SecureQueryString["typemodal"] != null)
            FilterTypeModal = Int16.Parse(SecureQueryString["typemodal"]);
        if (SecureQueryString != null && SecureQueryString["hierarchyMode"] != null)
            HierarchyFilterMode = SecureQueryString["hierarchyMode"];

        OnDataBindControls((DataBindAction)FilterTypeModal);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    private FilterParameterCollection getCustomFilterParameter(string filterName)
    {
        FilterParameterCollection _parames = new FilterParameterCollection();
        _parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _parames.Add(new FilterParameter("@AssignmentID", PrimaryID, DbType.Int32));
        _parames.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        _parames.Add(new FilterParameter("@FilterName", filterName, DbType.AnsiString));
        _parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));

        return _parames;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@PrimaryID", PrimaryID, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));

        DataTable dt = new DataTable();

        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadHierarchys:
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_Selected").ToString() + " " + GeneralFuncsLib.GetRiskHierarchyFilterText(HierarchyFilterMode);

                parames.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetHierarchyFilter", parames);
                //uxContent.Text = convertTable2String(dt, "DataKey");
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataText";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;
            case DataBindAction.LoadStates:
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_SelectedState").ToString();
                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetFilterState", parames);
                //uxContent.Text = convertTable2String(dt, "DataText");
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataText";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;
            case DataBindAction.LoadZips:
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_SelectedZip").ToString();
                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetFilterZipCode", parames);
                //uxContent.Text = convertTable2String(dt, "DataKey");
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataKey";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;
            case DataBindAction.LoadProfiles:
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_SelectedProfile").ToString();
                parames.Clear();
                parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parames.Add(new FilterParameter("@AssignmentID", PrimaryID, DbType.Int32));
                parames.Add(new FilterParameter("@Mode", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
                parames.Add(new FilterParameter("@FilterMode", Mode, DbType.Int32));
                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetProfileListByAssignment", parames);
                //uxContent.Text = convertTable2String(dt, "DataText");
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataText";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;
            case DataBindAction.LoadHighRisk:
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_SelectedHighRisk").ToString();
                parames.Clear();
                parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parames.AddLanguageID();
                parames.Add(new FilterParameter("@PrimaryID", Int32.Parse(this.PrimaryID), DbType.Int32));
                parames.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
                parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetFilterHRCode", parames);
                //uxContent.Text = convertTable2String(dt, "DataText");
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataText";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;
            case DataBindAction.LoadMarketDatas:
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_SelectedMarketData").ToString();
                parames.Add(new FilterParameter("@IsUseCodeSet", false, DbType.Boolean));
                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetFilterMarketData", parames);
                //uxContent.Text = convertTable2String(dt, "DataText1");
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataText1";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;
            case DataBindAction.LoadIncludeExcludeISONumbers:
                this.Title = "Selected Item";
                pnTitle.Visible = true;
                ltTitle.Text = " " + (SessionManager.IsIncludeItem ? "Include" : "Exclude");
                uxClose.OnClientClick = "return ClosePopupModal();";
                if (SessionManager.IncludeExcludeItem != null)
                {
                    string[] includeExcludeISONumbers = SessionManager.IncludeExcludeItem.Split(',');
                    dt.Columns.Add("DataKey", typeof(string));
                    dt.Columns.Add("DataText", typeof(string));
                    foreach (string iso in includeExcludeISONumbers)
                    {
                        DataRow row = dt.NewRow();
                        row["DataKey"] = iso;
                        row["DataText"] = iso;
                        dt.Rows.Add(row);
                    }
                    uxContent.DataValueField = "DataKey";
                    uxContent.DataTextField = "DataText";
                    uxContent.DataSource = dt;
                    uxContent.DataBind();
                }
                break;
            case DataBindAction.LoadMerchantClassifications:
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_Selected").ToString() + " " + GeneralFuncsLib.GetRiskHierarchyFilterText(HierarchyFilterMode);

                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_ListMIFClassificationFilter", parames);
                //uxContent.Text = convertTable2String(dt, "DataKey");
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataText";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;
            case DataBindAction.LoadLeadSource:
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_SelectedLeadSource").ToString() + " " + GeneralFuncsLib.GetRiskHierarchyFilterText(HierarchyFilterMode);

                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetFilterLeadSource", parames);
                //uxContent.Text = convertTable2String(dt, "DataKey");
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataText";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;
            case DataBindAction.LoadReferralSource:
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_SelectedReferralSource").ToString() + " " + GeneralFuncsLib.GetRiskHierarchyFilterText(HierarchyFilterMode);

                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetFilterReferralSource", parames);
                //uxContent.Text = convertTable2String(dt, "DataKey");
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataText";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;
            case DataBindAction.LoadRiskLevel:
                var _paramesRiskLevel = getCustomFilterParameter(RiskLevel);
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_SelectedRiskLevel").ToString();
                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetElementFilter", getCustomFilterParameter(RiskLevel));
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataText";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;
            case DataBindAction.LoadRiskCategory:
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_SelectedRiskCategory").ToString();
                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetElementFilter", getCustomFilterParameter(RiskCategory));
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataText";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;
            case DataBindAction.LoadCampaignID:
                this.Title = GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_SelectedCampaignID").ToString();
                dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetElementFilter", getCustomFilterParameter(CampaignID));
                uxContent.DataValueField = "DataKey";
                uxContent.DataTextField = "DataText";
                uxContent.DataSource = dt;
                uxContent.DataBind();
                break;


        }
    }

    private string convertTable2String(DataTable dt, string getDataColumn)
    {
        string result = string.Empty;
        if (dt == null || dt.Rows.Count == 0)
            return GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_NA").ToString();
        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            strBuilder.Append(dt.Rows[i][getDataColumn] + ", ");
        }
        result = strBuilder.ToString().Trim().TrimEnd(',');
        if (string.IsNullOrEmpty(result))
            return GetLocalResourceObject("rm_Filter_CommonViewMore_Modal_aspx_cs_NA").ToString();
        return result;
    }

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    public int Save()
    {
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
}
