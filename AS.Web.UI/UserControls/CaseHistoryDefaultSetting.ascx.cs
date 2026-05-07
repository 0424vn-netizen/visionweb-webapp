using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class UserControls_CaseHistoryDefaultSetting : GlobalUserControl
{
    public const int NOT_ASSIGNED_ID = 0;
    public const string RiskReportPage = "rm_RiskReport";
    public const string CaseHistory2Page = "CaseHistory2";
    public const string MerchantProfilePage = "MerchantProfile";
    public const string CMSType = "cms";
    public const string TYPE_RISK = "2";
    public const string TYPE_CMS = "1";
    public const string TYPE_CMS_AND_RISK = "1,2";

    public enum DataBindAction
    {
        BindStatuses,
        BindPriorityLevel,
        BindCaseType
    }

    enum JumpType
    {
        Status = 1,
        Issue = 2,
        OwnershipGroup = 3,
        Priority = 4,
        SearchCase = 5,
        ViewAddCase = 6,
        MyCase = 7,
        CaseHistory = 8,
        CaseSettings = 9,
        NotFound = 0,
        OpenCaseFromChat = 10,
        ViewCaseFromChat = 11,
        CaseHierarchyMaintenance = 12,
        TimerSetting = 13,
        ViewCaseFromNotification = 14,
        CaseHistoryOnly = 15
    }


    public string FromPage
    {
        get
        {
            if (Page.SecureQueryString != null)
                return Page.SecureQueryString["fromPage"].ToString();
            return null;
        }
    }

    public string DefaultCHStatusConfig
    {
        get
        {
            if (FromPage == "1")
                return UserConfigNames.CONFIG_MERCHANT_PROFILE_CH_STATUS_DEFAULT_SETTING;
            return UserConfigNames.CONFIG_RISK_REPORT_CH_STATUS_DEFAULT_SETTING;
        }
    }

    public string DefaultCHTypeConfig
    {
        get
        {
            if (FromPage == "1")
                return UserConfigNames.CONFIG_MERCHANT_PROFILE_CH_TYPE_DEFAULT_SETTING;
            return UserConfigNames.CONFIG_RISK_REPORT_CH_TYPE_DEFAULT_SETTING;
        }
    }

    public string DefaultCHPriorityConfig
    {
        get
        {
            if (FromPage == "1")
                return UserConfigNames.CONFIG_MERCHANT_PROFILE_CH_PRIORITY_DEFAULT_SETTING;
            return UserConfigNames.CONFIG_RISK_REPORT_CH_PRIORITY_DEFAULT_SETTING;
        }
    }

    public string CHStatusDefault
    {
        get
        {
            var defaultSetting = PersonalDataHelper.GetJSONConfig<string>(DefaultCHStatusConfig);
            return !defaultSetting.IsNullOrEmpty() ? defaultSetting : string.Empty;
        }
    }

    public string CHTypeDefault
    {
        get
        {
            var defaultSetting = PersonalDataHelper.GetJSONConfig<string>(DefaultCHTypeConfig);
            return !defaultSetting.IsNullOrEmpty() ? defaultSetting : string.Empty;
        }
    }

    public string CHPriorityDefault
    {
        get
        {
            var defaultSetting = PersonalDataHelper.GetJSONConfig<string>(DefaultCHPriorityConfig);
            return !defaultSetting.IsNullOrEmpty() ? defaultSetting : string.Empty;
        }
    }

    public bool IsHasOpenRiskCase
    {
        get
        {
            return Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_RISK_CASE) 
                || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_MS_CM_RISK_CASE);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindCaseType);
            uxTypes.SetSelectedValue(CHTypeDefault.Split(',').ToArray());
            CurrentTypeId = CurrentType();
            OnDataBindControls(DataBindAction.BindStatuses);
            OnDataBindControls(DataBindAction.BindPriorityLevel);
            uxStatuses.SetSelectedValue(CHStatusDefault.Split(',').ToArray());
            uxPriorityLevel.SetSelectedValue(CHPriorityDefault.Split(',').ToArray());
        }
    }
    public string GetListOfValues(ListItemCollection items)
    {
        string result = string.Empty;
        foreach (ListItem i in items)
        {
            result += i.Value + ",";
        }
        return result.Trim(',');
    }
    private int CurrentTypeId { get; set; }
    protected void ChangeStatusAndPriorities(object sender, EventArgs e)
    {
        CurrentTypeId = CurrentType();
        OnDataBindControls(DataBindAction.BindPriorityLevel);
        OnDataBindControls(DataBindAction.BindStatuses);
    }
    public int CurrentType()
    {
        if (!IsHasOpenRiskCase) return 1;

        string currentType = GetListOfValues(uxTypes.SelectedItems);
        int currentTypeId = 0;
        switch (currentType)
        {
            case TYPE_CMS:
                currentTypeId = 1;
                break;
            case TYPE_RISK:
                currentTypeId = 2;
                break;
            default:
                break;
        }
        return currentTypeId;
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindPriorityLevel:
                parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParamsForCM(SessionManager.CurrentUser.SiteID);
                parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                parameters.Add(new FilterParameter("@CaseTypeId", CurrentTypeId, DbType.Int32));
                uxPriorityLevel.DataSource = WebServices.CsReportServices.GetReports("spa_CM_GetPrioritiesByName", parameters);
                uxPriorityLevel.DataTextField = "Priority";
                uxPriorityLevel.DataValueField = "PriorityID";
                uxPriorityLevel.DataBind();
                uxPriorityLevel.Items.Insert(0, new ListItem(GetLocalResourceObject("lblNotAssigned").ToString(), NOT_ASSIGNED_ID.ToString()));
                uxPriorityLevel.SelectedValue = null;
                break;
            case DataBindAction.BindStatuses:
                parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParamsForCM(SessionManager.CurrentUser.SiteID);
                parameters.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
                parameters.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
                parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                parameters.Add(new FilterParameter("@CaseTypeId", CurrentTypeId, DbType.Int32));
                DataTable statuses = WebServices.CsReportServices.GetReports("spa_CM_GetStatusName", parameters);
                uxStatuses.Items.Clear();
                foreach (DataRow dr in statuses.Rows)
                {
                    string name = string.Format("{0}{1}", dr["StatusGroupName"], dr["Status"].IsNullData() ? string.Empty : " - " + dr["Status"]);
                    uxStatuses.Items.Add(new ListItem(name, dr["StatusID"].ToString()));
                }
                uxStatuses.SelectedValue = null;
                break;

            case DataBindAction.BindCaseType:
                string defaultCaseType = GetDefaultCaseType();
                parameters = new FilterParameterCollection();
                parameters.Add(new FilterParameter("@PermissionCodes", SessionManager.CurrentUserPermissions, DbType.String));
                parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                DataTable dtCaseTypes = WebServices.CsReportServices.GetReports("spa_CM_GetCaseTypes", parameters);
                uxTypes.Items.Clear();
                foreach (DataRow row in dtCaseTypes.Rows)
                {
                    uxTypes.Items.Add(new ListItem(row["Description"].ToString(), row["CaseTypeId"].ToString()));
                    if (string.Equals(defaultCaseType, row["Code"].ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        defaultCaseType = row["CaseTypeId"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(defaultCaseType))
                {
                    uxTypes.SelectedValue = defaultCaseType;
                }

                break;
        }
    }

    private string GetDefaultCaseType()
    {
        string xmlFilter = GeneralFuncsLib.BuilXmlFilterNotEncrypt(Tuple.Create("CaseType", ""));
        string result = string.Empty;
        if (!xmlFilter.IsNullOrEmpty())
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlFilter);
            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/XmlFilter/CaseType");
            if (node != null)
                result = node.InnerText;
        }

        return result;
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        PersonalDataHelper.SaveConfigAsJSON(DefaultCHStatusConfig, HandleString(uxStatuses.SelectedItems));
        PersonalDataHelper.SaveConfigAsJSON(DefaultCHTypeConfig, HandleString(uxTypes.SelectedItems));
        PersonalDataHelper.SaveConfigAsJSON(DefaultCHPriorityConfig, HandleString(uxPriorityLevel.SelectedItems));

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "doneSubmit", "ClosePopupModal();", true);
    }

    #region Helpers

    private string HandleString(ListItemCollection items)
    {
        if (items.Count == 0)
            return string.Empty;
        StringBuilder result = new StringBuilder();
        string delim = "";
        foreach (ListItem item in items)
        {
            result.Append(delim); delim = ",";
            result.Append(item.Value);
        }
        return result.ToString();
    }

    #endregion
}