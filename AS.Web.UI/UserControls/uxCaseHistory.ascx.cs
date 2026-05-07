using AS.Common;
using AS.Common.DBManager;
using AS.Common.Logger;
using AS.Controls.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

public partial class UserControls_uxCaseHistory : GlobalUserControl
{
    public const string CMSType = "cms";
    public const string TYPE_RISK = "2";
    public const string TYPE_CMS = "1";
    public const string TYPE_CMS_AND_RISK = "1,2";
    public const int NOT_ASSIGNED_ID = 0;
    public static char SEPARATOR = ',';
    private const string MERCHANT_PROFILE_PAGE = "MerchantProfile.aspx";
    private const string RISK_REPORT_PAGE = "RiskReport.aspx";
    private const string SPA_GET_STATUS_NAME = "spa_CM_GetStatusName";
    private const string SPA_GEt_CASE_TYPES = "spa_CM_GetCaseTypes";
    private const string SPA_GET_PRIORITY_BY_NAME = "spa_CM_GetPrioritiesByName";
    private const string SPA_GET_CASE_HISTORY_BY_MERCHANT = "spa_CM_GetCaseHistoryByMerchant";
    //43358 - VW Risk CR-Design Change #2 to 39919 New Sort
    enum Sort
    {
        Asc,
        Desc
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

    public enum DataBindAction
    {
        BindStatuses,
        BindPriorityLevel,
        BindCaseType,
        BindCaseHistoryGrid
    }

    public string OpenDefaultSettingUrlForCH
    {
        get
        {
            return Page.BuildSecureQueryString(string.Format("fromPage={0}&module=2", FromPage));
        }
    }

    public int FromPage
    {
        get
        {
            var url = Request.Url.AbsoluteUri.ToLower();
            if (url.Contains(MERCHANT_PROFILE_PAGE.ToLower()) || url.Contains("MerchantInformation.aspx".ToLower()))
                return 1;
            if (url.Contains(RISK_REPORT_PAGE.ToLower()))
                return 2;
            return 0;
        }
    }

    public string MerchantNumber
    {
        get
        {
            if (ViewState["MerchantNumber"] != null)
                return ViewState["MerchantNumber"].ToString();
            return string.Empty;
        }
        set
        {
            ViewState["MerchantNumber"] = value;
        }
    }

    public CaseFilteringOptions FilerValues
    {
        get
        {
            return new CaseFilteringOptions()
            {
                Statuses = GetListOfValues(uxStatuses.SelectedItems),
                CaseTypes = GetCaseType(uxTypes.SelectedItems),
                PriorityLevels = GetListOfValues(uxPriorityLevel.SelectedItems)
            };
        }

        set
        {
            uxStatuses.SetSelectedValue(value.Statuses.Split(',').ToArray());
            uxTypes.SetSelectedValue(value.CaseTypes.Split(',').ToArray());
            uxPriorityLevel.SetSelectedValue(value.PriorityLevels.Split(',').ToArray());
        }
    }

    CaseFilteringOptions FilerValuesViewState
    {
        get
        {
            if (ViewState["FilerValuesViewState"] == null)
            {
                return new CaseFilteringOptions()
                {
                    Statuses = GetListOfValues(uxStatuses.SelectedItems),
                    CaseTypes = GetCaseType(uxTypes.SelectedItems),
                    PriorityLevels = GetListOfValues(uxPriorityLevel.SelectedItems)
                };
            }
            else
            {
                return ViewState["FilerValuesViewState"] as CaseFilteringOptions;
            }

        }
        set
        {
            ViewState["FilerValuesViewState"] = value;
        }
    }

    public string DefaultCHStatusConfig
    {
        get
        {
            if (FromPage == 1)
                return UserConfigNames.CONFIG_MERCHANT_PROFILE_CH_STATUS_DEFAULT_SETTING;
            return UserConfigNames.CONFIG_RISK_REPORT_CH_STATUS_DEFAULT_SETTING;
        }
    }

    public string DefaultCHTypeConfig
    {
        get
        {
            if (FromPage == 1)
                return UserConfigNames.CONFIG_MERCHANT_PROFILE_CH_TYPE_DEFAULT_SETTING;
            return UserConfigNames.CONFIG_RISK_REPORT_CH_TYPE_DEFAULT_SETTING;
        }
    }

    public string DefaultCHPriorityConfig
    {
        get
        {
            if (FromPage == 1)
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

    private int CurrentTypeId { get; set; }

    public List<DataSourceParallelResponse> DataSources { get; set; }

    private bool HasRiskPermission
    {
        get
        {
            return ((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_RISK_CASE)
                || ((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_MS_CM_RISK_CASE);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //Clear sort
                ClearSort();
                BindDataMultiSelector();
                FilerValuesViewState = FilerValues;
                if (Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_OPEN_CASE))
                {
                    uxbtnOpenNewCase2.Visible = true;
                    //string url = ResolveUrl("~/cm/OpenNewCase.aspx?") + Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&IsCloseWhenSubmited=1&FromIframe=1", MerchantNumber));
                    string url = ResolveUrl("~/JumpToCase.aspx?") + string.Format("type=6");
                    uxbtnOpenNewCase2.Attributes.Add("onclick", string.Format("parent.openPopupWindowOnMenu(event,'{0}','{1}'); return false;", url, "OpenNewCase"));
                }
                uxDefaultSettingLink.OnClientClick = string.Format("return openDefaulSettingModal();");
            }
        }
        catch (ViewStateException)
        {
            LoggerManager.Error(string.Format("UserControls_PageTitle - ViewStateException - Request={0}; SessionID={1}", Context.Request.Url, Session.SessionID));
            throw;
        }
    }

    public void BindDataMultiSelector()
    {
        //10/4/2018 | 45128 Merchant Notes Issue - Prod
        if (!string.IsNullOrEmpty(this.MerchantNumber) && (((ReportPage)Page).IsUserWithPermission("CMOpenCase")
                || ((ReportPage)Page).IsUserWithPermission("CMSearchCase")))
        {
            OnDataBindControls(DataBindAction.BindCaseType);
            uxTypes.SetSelectedValue(CHTypeDefault.Split(',').ToArray());
            CurrentTypeId = GetCurrentType();
            OnDataBindControls(DataBindAction.BindStatuses);
            OnDataBindControls(DataBindAction.BindPriorityLevel);
            uxStatuses.SetSelectedValue(CHStatusDefault.Split(',').ToArray());
            uxPriorityLevel.SetSelectedValue(CHPriorityDefault.Split(',').ToArray());           
            OnDataBindControls(DataBindAction.BindCaseHistoryGrid, uxReportGridCaseHistory);
        }
    }
    protected void uxExportTop_NeedExportConfig(object sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        exportConfig.AllowHtmlEncoded = true;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(string.Format(GetLocalResourceObject("uxExporterResourcekeyGridTile.Text").ToString(), MerchantNumber, DateTime.Today.ToString("MM/dd/yyyy")));
        uxReportGridCaseHistory.Columns.FindByUniqueName("CardView").Visible = false;
        uxReportGridCaseHistory.Columns.FindByUniqueName("CaseNumberTitle").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("Status").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("Prioritylevel").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("OwnershipGroup").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("Assignedto").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("OpenedBy").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("Role").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("CaseType").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("OpenedOn").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("FollowUpOn").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("LastUpdateOn").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("ClosedOn").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("ReopenedOn").Visible = true;
        uxReportGridCaseHistory.Columns.FindByUniqueName("LastClosedOn").Visible = true;
    }
    protected void uxReportGridCaseHistory_DataSourceReady(object sender, EventArgs e)
    {
        uxReportGridCaseHistory.CurrentPageIndex = 0;
        uxReportGridCaseHistory.PageSize = 10;
    }

    protected void uxReportGridCaseHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindCaseHistoryGrid, sender);
    }
    
    public void ItemDataBoundHistory(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            var item = e.Item as GridDataItem;
            var cardViewColumn = item["CardView"];
            DataRowView dv = e.Item.DataItem as DataRowView;
            string descriptText = dv["Description"].ToString().Length > 0 ? " - " + Server.HtmlDecode(dv["Description"].ToString()) : string.Empty;
            string textLink = dv["CaseNumber"] + descriptText;
            // case number
            var caseNumberCtr = cardViewColumn.FindControl("CaseNumber") as HtmlGenericControl;

            if (Page.IsUserWithPermission("CMOpenRiskCase") || Page.IsUserWithPermission("MSCMOpenRiskCase") ||
                (Page.IsUserWithPermission("CMOpenCase") && dv["CaseTypeDescription"].ToString().ToLower().Equals(CMSType)))
            {
                string url = ResolveUrl("~/JumpToCase.aspx?") + string.Format("type=6&cid={0}", dv["CaseID"]);
                //string url1 = ResolveUrl("~/cm/OpenNewCase.aspx?") +
                //             Page.BuildSecureQueryString(
                //                 string.Format("CaseID={0}&MerchantNumber={1}&FromIframe=1", dv["CaseID"],
                //                     dv["MerchantNumber"]));
                string fullLink = string.Format(
                        "<a onclick=\"parent.openPopupWindowOnMenu(event,'{0}', 'OpenNewCase')\" href=\"#\">{1}</a>",
                        url, textLink);
                AddHtml(caseNumberCtr, fullLink);

            }
            else
            {
                AddText(caseNumberCtr, textLink);
            }

            var priorityLabel = cardViewColumn.FindControl("divPrioritylevel") as HtmlGenericControl;
            string strColor = dv["PriorityColor"].ToString();
            string bgColor = "Transparent";
            string borderColor = "#000000";

            if (!string.IsNullOrEmpty(strColor))
            {
                bgColor = GeneralFuncsLib.GenerateRgba("#" + strColor, "0.2");
                borderColor = dv["PriorityColor"].ToSafeString();

            }
            string textPriority = dv["Prioritylevel"].ToString() + " ";
            if (!string.IsNullOrEmpty(dv["SLR"].ToString()))
            {
                textPriority = string.Format("{0} - {1}", textPriority, " " + dv["SLR"].ToString());
            }

            string htmlInner = string.Format("<span class=\"priority-level-br\"><span class=\"priority-level\" style=\"background-color:{0};border-color: #{1} !important\" >{2}</span></span>", bgColor, borderColor, textPriority);
            AddHtml(priorityLabel, htmlInner);

            //43358 - VW Risk CR-Design Change #2 to 39919 New Sort
            AS.Controls.Global.Button sortIcon = cardViewColumn.FindControl("uxSortIcon_" + SessionManager.CurrentSortColumnCase) as AS.Controls.Global.Button;
            if (sortIcon.IsNotNullData())
            {
                sortIcon.CssClass = SortIconClass(SessionManager.CurrentSortColumnCase);
            }

        }

    }

    public void BindGridCaseHistory()
    {
        uxReportGridCaseHistory.CurrentPageIndex = 0;
        ResetGrid(uxReportGridCaseHistory);
        //uxReportGridCaseHistory.ResetGrid();
        uxReportGridCaseHistory.Rebind();
    }

    protected void uxApply_Click(object sender, EventArgs e)
    {
        //43358 - VW Risk CR-Design Change #2 to 39919 New Sort
        ClearSort();
        FilerValuesViewState = FilerValues;
        uxReportGridCaseHistory.Rebind();
    }
    
    protected void ChangeCurrentType(object sender, EventArgs e)
    {
        CurrentTypeId = GetCurrentType();
        OnDataBindControls(DataBindAction.BindPriorityLevel);
        OnDataBindControls(DataBindAction.BindStatuses);
    }
    public int GetCurrentType()
    {
        if (!HasRiskPermission)
            return 1; // Only CMS

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
                DataTable bindData = null;
                if (DataSources.IsNotNullData())
                {
                    var data = DataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindPriorityLevel.ToString());
                    if (data.IsNotNullData())
                        bindData = data.DataSource;
                }

                if(bindData.IsNotNullData())
                {
                    uxPriorityLevel.DataSource = bindData;
                }
                else
                {
                    parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParamsForCM(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                    parameters.Add(new FilterParameter("@CaseTypeId", CurrentTypeId, DbType.Int32));
                    uxPriorityLevel.DataSource = WebServices.CsReportServices.GetReports(SPA_GET_PRIORITY_BY_NAME, parameters);
                }
                uxPriorityLevel.DataTextField = "Priority";
                uxPriorityLevel.DataValueField = "PriorityID";
                uxPriorityLevel.DataBind();
                uxPriorityLevel.Items.Insert(0, new ListItem(GetLocalResourceObject("lblNotAssigned").ToString(), NOT_ASSIGNED_ID.ToString()));
                uxPriorityLevel.SelectedValue = null;
                break;
            case DataBindAction.BindStatuses:
                DataTable statuses = null;
                DataTable bindStatusData = null;
                if (DataSources.IsNotNullData())
                {
                    var data = DataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindStatuses.ToString());
                    if (data.IsNotNullData())
                        bindStatusData = data.DataSource;
                }

                if(bindStatusData.IsNotNullData())
                {
                    statuses = bindStatusData;
                }
                else
                {
                    parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParamsForCM(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
                    parameters.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
                    parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                    parameters.Add(new FilterParameter("@CaseTypeId", CurrentTypeId, DbType.Int32));
                    statuses = WebServices.CsReportServices.GetReports(SPA_GET_STATUS_NAME, parameters);
                }
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
                DataTable dtCaseTypes = null;
                DataTable bindCaseData = null;
                if (DataSources.IsNotNullData())
                {
                    var data = DataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindCaseType.ToString());
                    if (data.IsNotNullData())
                        bindCaseData = data.DataSource;
                }

                if (bindCaseData.IsNotNullData())
                {
                    dtCaseTypes = bindCaseData;
                }
                else
                {
                    parameters = new FilterParameterCollection();
                    parameters.Add(new FilterParameter("@PermissionCodes", SessionManager.CurrentUserPermissions, DbType.String));
                    parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                    dtCaseTypes = WebServices.CsReportServices.GetReports(SPA_GEt_CASE_TYPES, parameters);
                }
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
            case DataBindAction.BindCaseHistoryGrid:
                //43358 - VW Risk CR-Design Change #2 to 39919 New Sort
                if (SessionManager.IsAddNewCase && string.IsNullOrEmpty(SessionManager.CaseHistorySort) && string.IsNullOrEmpty(SessionManager.CurrentSortColumnCase))
                {
                    ClearSort();
                    SessionManager.IsAddNewCase = false;
                }
                uxExporter.GridHeader = VeraCodeSolution.ValidateResponseData(string.Format(GetLocalResourceObject("uxExportFileName.Text").ToString(), MerchantNumber, DateTime.Today.ToString("MM/dd/yyyy")));
                uxExporter.GridTitle = VeraCodeSolution.ValidateResponseData(string.Format(GetLocalResourceObject("uxExporterResourcekeyGridTile.Text").ToString(), MerchantNumber, DateTime.Today.ToString("MM/dd/yyyy")));

                DataTable bindHistoryData = null;
                if (DataSources.IsNotNullData())
                {
                    var data = DataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindCaseHistoryGrid.ToString());
                    if (data.IsNotNullData())
                        bindHistoryData = data.DataSource;
                }

                if (bindHistoryData.IsNotNullData())
                {
                    uxReportGridCaseHistory.DataSource = bindHistoryData;
                }
                else
                {
                    parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParamsForCM(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                    parameters.Add(new FilterParameter("@StatusID", FilerValuesViewState.Statuses, DbType.String));
                    parameters.Add(new FilterParameter("@CasePriorityID", FilerValuesViewState.PriorityLevels, DbType.String));
                    parameters.Add(new FilterParameter("@CaseTypeId", FilerValuesViewState.CaseTypes, DbType.String));
                    parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                    parameters.Add(new FilterParameter("@stOrder", SessionManager.CaseHistorySort, DbType.String));
                    uxReportGridCaseHistory.DataSource = WebServices.CsReportServices.GetReports(SPA_GET_CASE_HISTORY_BY_MERCHANT, parameters);
                }

                break;
        }
    }

    protected void uxReportGridCaseHistory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.StartsWith("Sort_"))
        {
            string columnName = e.CommandName.Split('_')[1];
            string asc = string.Format("{0} {1}", columnName, Sort.Asc.ToString());
            string desc = string.Format("{0} {1}", columnName, Sort.Desc.ToString());
            //clear sort with another column
            if (!SessionManager.CaseHistorySort.IsNullOrEmpty() && !SessionManager.CaseHistorySort.Contains(columnName))
                SessionManager.CaseHistorySort = string.Empty;
            //switch (columnName)
            //{
            //    case "OpenedOn":
            //    case "FollowUpOn":
            //    case "LastUpdateOn":
            //    case "ClosedOn":
            //    case "ReopenedDTS":
            //    case "LastClosedOn":
            //        {
            //            if (SessionManager.CaseHistorySort.IsNullOrEmpty())
            //                SessionManager.CaseHistorySort = desc;
            //            else if (SessionManager.CaseHistorySort.Contains(desc))
            //                SessionManager.CaseHistorySort = asc;
            //            else if (SessionManager.CaseHistorySort.Contains(asc))
            //                SessionManager.CaseHistorySort = string.Empty;
            //            break;
            //        }
            //    default:
            //        {
            //            if (SessionManager.CaseHistorySort.IsNullOrEmpty())
            //                SessionManager.CaseHistorySort = asc;
            //            else if (SessionManager.CaseHistorySort.Contains(asc))
            //                SessionManager.CaseHistorySort = desc;
            //            else if (SessionManager.CaseHistorySort.Contains(desc))
            //                SessionManager.CaseHistorySort = string.Empty;
            //            break;
            //        }
            //}
            if (SessionManager.CaseHistorySort.IsNullOrEmpty())
                SessionManager.CaseHistorySort = desc;
            else if (SessionManager.CaseHistorySort.Contains(desc))
                SessionManager.CaseHistorySort = asc;
            else if (SessionManager.CaseHistorySort.Contains(asc))
                SessionManager.CaseHistorySort = string.Empty;

            SessionManager.CurrentSortColumnCase = columnName;
            uxReportGridCaseHistory.CurrentPageIndex = 0;
            uxReportGridCaseHistory.Rebind();
        }
    }

    //43358 - VW Risk CR-Design Change #2 to 39919 New Sort
    public string SortIconClass(string columnName)
    {
        if (SessionManager.CaseHistorySort.Contains(string.Format("{0} {1}", columnName, Sort.Desc.ToString())))
            return "rgSortAsc";
        if (SessionManager.CaseHistorySort.Contains(string.Format("{0} {1}", columnName, Sort.Asc.ToString())))
            return "rgSortDesc";
        return "hide";
    }
    //43358 - VW Risk CR-Design Change #2 to 39919 New Sort
    public void ClearSort()
    {
        SessionManager.CurrentSortColumnCase = string.Empty;
        SessionManager.CaseHistorySort = string.Empty;
        uxReportGridCaseHistory.CurrentPageIndex = 0;
    }

    protected void uxFinishSaveDefaultSettingCH_Click(object sender, EventArgs e)
    {
        ClearSort();
        OnDataBindControls(DataBindAction.BindStatuses);
        uxStatuses.SetSelectedValue(CHStatusDefault.Split(',').ToArray());

        OnDataBindControls(DataBindAction.BindPriorityLevel);
        uxPriorityLevel.SetSelectedValue(CHPriorityDefault.Split(',').ToArray());

        OnDataBindControls(DataBindAction.BindCaseType);
        uxTypes.SetSelectedValue(CHTypeDefault.Split(',').ToArray());

        FilerValuesViewState = FilerValues;
        uxReportGridCaseHistory.Rebind();
    }
    #region Helper

    public void ResetGrid(ASGrid grid)
    {
        string pageSize = GeneralFuncsLib.GetDataOfExtendedSetting("ASGrid_DefaultPageSize");
        if (!string.IsNullOrEmpty(pageSize))
        {
            grid.PageSize = grid.MasterTableView.PageSize = int.Parse(pageSize);
        }
        grid.CurrentPageIndex = 0;
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

    private string GetCaseType(ListItemCollection items)
    {
        string item = GetListOfValues(items);
        if (string.IsNullOrEmpty(item) && !HasRiskPermission) return "1";
        return item;
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

    private string Decrypt(string encryptText)
    {
        return WebServices.ApiServices.DecryptText(encryptText);
    }

    private void AddHtml(HtmlGenericControl ctrl, string html)
    {
        ctrl.InnerHtml = html;
    }

    private void AddText(HtmlGenericControl ctrl, string text)
    {
        ctrl.InnerText = text;
    }

    private void AddColor(HtmlGenericControl ctrl, string color)
    {
        ctrl.Style.Add("color", color);
    }

    private void AddBackColor(HtmlGenericControl ctrl, string color)
    {
        ctrl.Style.Add("background-color", color);
    }

    private void AddTooltip(HtmlGenericControl ctrl, string tooltip)
    {
        ctrl.Attributes["title"] = tooltip;
    }

    #endregion
    #region ---- Multi thread ----
    private FilterParameterCollection GetCaseHistoryParameters(DataBindAction action)
    {
        //DataBindAction actiontemp = (DataBindAction)action; 
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParamsForCM(SessionManager.CurrentUser.SiteID);
        switch (action)
        {
            case DataBindAction.BindStatuses:
                {
                    parameters.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
                    parameters.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
                    parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                    //spa_CM_GetStatusName
                    break;
                }
            case DataBindAction.BindCaseType:
                {
                    parameters = new FilterParameterCollection();
                    parameters.Add(new FilterParameter("@PermissionCodes", SessionManager.CurrentUserPermissions, DbType.String));
                    parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                    //DataTable dtCaseTypes = WebServices.CsReportServices.GetReports(SPA_GEt_CASE_TYPES, parameters);
                    break;
                }
            case DataBindAction.BindPriorityLevel:
                {
                    parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                    //uxPriorityLevel.DataSource = WebServices.CsReportServices.GetReports(SPA_GET_PRIORITY_BY_NAME, parameters);
                    break;
                }
            case DataBindAction.BindCaseHistoryGrid:
                {
                    parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                    parameters.Add(new FilterParameter("@StatusID", CHStatusDefault, DbType.String));
                    parameters.Add(new FilterParameter("@CasePriorityID", CHPriorityDefault, DbType.String));
                    parameters.Add(new FilterParameter("@CaseTypeId", CHTypeDefault, DbType.String));
                    parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
                    parameters.Add(new FilterParameter("@stOrder", SessionManager.CaseHistorySort, DbType.String));
                    //uxReportGridCaseHistory.DataSource = WebServices.CsReportServices.GetReports(SPA_GET_CASE_HISTORY_BY_MERCHANT, parameters);
                    break;
                }

        }
        return parameters;
    }
    public void BindDataFromThread(List<DataSourceParallelResponse> dataSources)
    {
        if (IsPostBack)
        {
            DataSources = dataSources;
            BindDataMultiSelector();
            BindGridCaseHistory();
        }
    }

    #region SPA INFO
    public SpaInfo SpaGetStatusName
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindStatuses.ToString(),
                SpaName = SPA_GET_STATUS_NAME,
                Parameters = GetCaseHistoryParameters(DataBindAction.BindStatuses)
            };
        }
    }
    public SpaInfo SpaGetCaseTypes
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindCaseType.ToString(),
                SpaName = SPA_GEt_CASE_TYPES,
                Parameters = GetCaseHistoryParameters(DataBindAction.BindCaseType)
            };
        }
    }
    public SpaInfo SpaGetPrioritiesByName
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindPriorityLevel.ToString(),
                SpaName = SPA_GET_PRIORITY_BY_NAME,
                Parameters = GetCaseHistoryParameters(DataBindAction.BindPriorityLevel)
            };
        }
    }
    public SpaInfo SpaGetCaseHistoryByMerchant
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindCaseHistoryGrid.ToString(),
                SpaName = SPA_GET_CASE_HISTORY_BY_MERCHANT,
                Parameters = GetCaseHistoryParameters(DataBindAction.BindCaseHistoryGrid)
            };
        }
    }
    #endregion
    #endregion ---- Multi thread ----
}


[Serializable]
public class CaseFilteringOptions
{
    public string BasicSearchTerms { get; set; }

    public string CaseFilterType { get; set; }
    public string CaseFilterValue { get; set; }
    //Row 1
    public string StatusGroups { get; set; }

    public string CaseTypes { get; set; }
    //Row 2
    public bool Subscribed { get; set; }
    // Row 3
    public string DateFilterType { get; set; }
    public DateTime? DateFilterFrom { get; set; }
    public DateTime? DateFilterTo { get; set; }
    public string DateFilterValue { get; set; }
    // Row 4
    public string PriorityLevels { get; set; }
    public string OwnerShipGroups { get; set; }
    public string AssignedTo { get; set; }
    public string OpenBy { get; set; }
    // Row 5
    public string Statuses { get; set; }
    //Row 6
    public string Issues { get; set; }
    //#42812 
    public int AssignedType { get; set; }
}
