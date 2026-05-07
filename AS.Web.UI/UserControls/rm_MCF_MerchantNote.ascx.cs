
using AS.Common.DBManager;
using AS.Controls.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Common.Logger;
using AS.Web.Business.RiskReport;
using AS.Web.Business.RiskReport.Models;
using GeneralFuntionBusiness = AS.Web.Business.General.GeneralFuncsLib;

public partial class UserControls_rm_MCF_MerchantNote : GlobalUserControl
{
    #region contanst
    private const string MERCHANT_PROFILE_PAGE = "MerchantProfile.aspx";
    private const string RISK_REPORT_PAGE = "RiskReport.aspx";
    private const string SPA_GET_ROLE = "spa_MerchantNotes_GetRole";
    private const string SPA_GET_SOURCE = "spa_MerchantNotes_GetSource";
    private const string SPA_GET_ADDED_USERS = "spa_MerchantNotes_GetUser";
    #endregion
    #region enum
    public enum DataBindAction
    {
        BindSourceList,
        BindRoleList,
        BindAddedByList,
        BindGridMerchantNote,
        //BindSortList
    }

    enum PostBackAction
    {
        AddNote,
        Rebind
    }

    //43358 - VW Risk CR-Design Change #2 to 39919 New Sort
    enum Sort
    {
        Asc,
        Desc
    }
    #endregion

    #region Properties

    private bool _isExporting = false;

    public bool IsAlreadyGetSourceRoleUserDataMerchantNote
    {
        get
        {
            if (ViewState["IsAlreadyGetSourceRoleUserDataMerchantNote"] == null)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(ViewState["IsAlreadyGetSourceRoleUserDataMerchantNote"]);
            }

        }
        set
        {
            ViewState["IsAlreadyGetSourceRoleUserDataMerchantNote"] = value;
        }
    }

    private DataTable GetDataTableByViewState(string viewStateKey, string featureName, string storedProcedure, DataBindAction action)
    {
        if (ViewState[viewStateKey] != null)
        {
            return (DataTable)ViewState[viewStateKey];
        }

        DataTable result = null;

        if (DataSources.IsNotNullData())
        {
            var data = DataSources.SingleOrDefault(m => m.FeatureName == featureName);
            if (data != null)
            {
                result = data.DataSource;
            }
        }

        if (result == null)
        {
            result = WebServices.RiskServices.GetReports(storedProcedure, GetMerchantNoteParameters(action));
            ViewState[viewStateKey] = result;
        }

        return result;
    }

    private DataTable SourceListViewState
    {
        get
        {
            return GetDataTableByViewState("SourceListViewState", DataBindAction.BindSourceList.ToString(), SPA_GET_SOURCE, DataBindAction.BindSourceList);
        }
    }

    private DataTable RoleListViewState
    {
        get
        {
            return GetDataTableByViewState("RoleListViewState", DataBindAction.BindRoleList.ToString(), SPA_GET_ROLE, DataBindAction.BindRoleList);
        }
    }

    private DataTable AddedByListViewState
    {
        get
        {
            return GetDataTableByViewState("AddedByListViewState", DataBindAction.BindAddedByList.ToString(), SPA_GET_ADDED_USERS, DataBindAction.BindAddedByList);
        }
    }


    public string MerchantNumber
    {
        get
        {
            if (ViewState["MerchantNumber"] != null)
            {
                return ViewState["MerchantNumber"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            ViewState["MerchantNumber"] = value;
        }
    }

    public bool IsHasMerchantProfile
    {
        get
        {
            return (SessionManager.CurrentUserPermissions.Contains("MerchProfile")
                || SessionManager.CurrentUserPermissions.Contains("MSMerchProfile"));
        }
    }

    public bool IsHasRiskReport
    {
        get
        {
            return (SessionManager.CurrentUserPermissions.Contains("RskRP")
                || SessionManager.CurrentUserPermissions.Contains("MSRskRP"));
        }
    }

    public bool IsHasOpenNewCase
    {
        get
        {
            return SessionManager.CurrentUserPermissions.Contains("CMOpenCase");
        }
    }

    public bool IsHasOpenRiskCase
    {
        get
        {
            return SessionManager.CurrentUserPermissions.Contains("CMOpenRiskCase");
        }
    }

    public bool IsHasShadowUnderwriting
    {
        get
        {
            return SessionManager.CurrentUserPermissions.Contains("SponsorMerchantShadowUnderwriting");
        }
    }

    public MerchantNoteFilterOption MerchantNoteFilterOption
    {
        get
        {
            if (ViewState["MerchantNoteFilterOption"] == null)
            {
                return new MerchantNoteFilterOption()
                {
                    NotesSourceList = string.Empty,
                    RoleList = string.Empty,
                    UserList = string.Empty
                };
            }
            else
            {
                return ViewState["MerchantNoteFilterOption"] as MerchantNoteFilterOption;
            }

        }
        set
        {
            ViewState["MerchantNoteFilterOption"] = value;
        }
    }

    public string DefaultSourceConfig
    {
        get
        {
            if (NoteSourceDefaultForIns == 1)
                return UserConfigNames.CONFIG_MERCHANT_PROFILE_SOURCE_DEFAULT_SETTING;
            return UserConfigNames.CONFIG_RISK_REPORT_SOURCE_DEFAULT_SETTING;
        }
    }

    //1: Merchant Profile, 2: Risk Report, 3: CMS
    public string NoteSourceDefault
    {
        get
        {
            //Get default source setting by config
            var defaultSetting = ExcludeDefaultSettingByPermission(PersonalDataHelper.GetJSONConfig<string>(DefaultSourceConfig));
            return defaultSetting;
        }
    }

    //1: Merchant profile, 0: Risk report
    public int NoteSourceDefaultForIns
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

    public string OpenDefaultSettingUrlForMerchantNote
    {
        get
        {
            return Page.BuildSecureQueryString(string.Format("fromPage={0}&module=1", NoteSourceDefaultForIns));
        }
    }

    //Contains data tables after multi-thread excuted
    public List<DataSourceParallelResponse> DataSources { get; set; }

    private static bool EditMerchantNotePermission
    {
        get
        {
            var isPermission = GeneralFuntionBusiness.CheckUserPermissions(SessionManager.CurrentUserPermissions
                    , ','
                    , new List<string> {
                        WebSiteConstants.PERMISSION_ACCESS_FUNC_RISK_EDIT_MERCHANT_NOTE,
                        WebSiteConstants.PERMISSION_ACCESS_FUNC_RISK_EDIT_MERCHANT_NOTE_MS
                    }
                );
            return isPermission;
        }
    }

    private static bool DeleteMerchantNotePermission
    {
        get
        {
            var isPermission = GeneralFuntionBusiness.CheckUserPermissions(SessionManager.CurrentUserPermissions
                   , ','
                   , new List<string> {
                        WebSiteConstants.PERMISSION_ACCESS_FUNC_RISK_DELETE_MERCHANT_NOTE,
                        WebSiteConstants.PERMISSION_ACCESS_FUNC_RISK_DELETE_MERCHANT_NOTE_MS
                   }
               );
            return isPermission;
        }
    }

    #endregion
    private int CurrentSortRowIndex = 0;
    private string CurrentSortColumn = string.Empty;

    private readonly IRiskReportNoteBussiness _riskReportNoteBussiness;
    public UserControls_rm_MCF_MerchantNote() : this(new RiskReportNoteBussiness(WebServices.RiskServices))
    {
    }
    public UserControls_rm_MCF_MerchantNote(IRiskReportNoteBussiness riskReportNoteBussiness)
    {
        _riskReportNoteBussiness = riskReportNoteBussiness;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                BindDataMultiSelector();
                SessionManager.MerchantNoteSort = string.Empty;
                uxDefaultSettingLink.OnClientClick = string.Format("return openDefaulSettingModalMN();");
                if (NoteSourceDefaultForIns == 2) // risk report page hide the add note session
                {
                    addnote.Visible = false;
                }
            }
        }
        catch (ViewStateException)
        {
            LoggerManager.Error(string.Format("UserControls_rm_MCF_MerchantNote - ViewStateException - Request={0}; SessionID={1}", Context.Request.Url, Session.SessionID));
            throw;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindSourceList:
                {
                    DataTable dt = SourceListViewState;
                    uxSourceList.DataValueField = "NoteSourceID";
                    uxSourceList.DataTextField = "Description";
                    uxSourceList.DataSource = dt;
                    uxSourceList.DataBind();
                    //Hide Default Setting link when source has only one item
                    uxDefaultSettingLink.Visible = dt.Rows.Count > 1;
                    break;
                }
            case DataBindAction.BindRoleList:
                {
                    DataTable dt = RoleListViewState;
                    uxRoleList.DataValueField = "HierarchyID";
                    uxRoleList.DataTextField = "HierarchyName";
                    uxRoleList.DataSource = dt;
                    uxRoleList.DataBind();
                    break;
                }
            case DataBindAction.BindAddedByList:
                {
                    DataTable dt = AddedByListViewState;
                    uxAddedByList.DataValueField = "UserID";
                    uxAddedByList.DataTextField = "UserNameFull";
                    uxAddedByList.DataSource = dt;
                    uxAddedByList.DataBind();
                    break;
                }
            case DataBindAction.BindGridMerchantNote:
                {
                    var rqRiskReportNote = new GetRiskReportNoteRequest
                    {
                        UserMode = GeneralFuncsLib.GetUserMode(),
                        MerchantNumber = MerchantNumber,
                        NotesSourceList = MerchantNoteFilterOption.NotesSourceList,
                        RoleList = MerchantNoteFilterOption.RoleList,
                        UserList = MerchantNoteFilterOption.UserList,
                        IsHasMerchantProfile = IsHasMerchantProfile,
                        IsHasRiskReport = IsHasRiskReport,
                        IsHasOpenNewCase = IsHasOpenNewCase,
                        IsHasOpenRiskCase = IsHasOpenRiskCase,
                        IsHasShadowUnderwriting = IsHasShadowUnderwriting,
                        MerchantNoteSort = SessionManager.MerchantNoteSort,
                        HasMultiLanguageFeature = GeneralFuncsLib.HasMultiLanguageFeature,
                        CurrentLanguage = SessionManager.CurrentLanguage,
                        IsExporting = _isExporting
                    };

                    ((ASGrid)sender).DataSourceInvoker = _riskReportNoteBussiness.GetRiskReportNotes(rqRiskReportNote, SessionManager.CurrentUser);
                    break;
                }
        }
    }

    public DataTable GetSources()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@IsHasMerchantProfile", IsHasMerchantProfile, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsHasRiskReport", IsHasRiskReport, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsHasOpenNewCase", IsHasOpenNewCase, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsHasOpenRiskCase", IsHasOpenRiskCase, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsHasShadowUnderwriting", IsHasShadowUnderwriting, DbType.Boolean));
        parameters.AddLanguageID();
        return WebServices.RiskServices.GetReports(SPA_GET_SOURCE, parameters);
    }

    public DataTable GetRoles(string sources)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@MerchantNumber", SessionManager.CurrentMerchantNumber, DbType.AnsiString));
        parameters.Add(new FilterParameter("@NotesSourceList", sources, DbType.AnsiString));
        parameters.Add(new FilterParameter("@IsHasMerchantProfile", IsHasMerchantProfile, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsHasRiskReport", IsHasRiskReport, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsHasOpenNewCase", IsHasOpenNewCase, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsHasOpenRiskCase", IsHasOpenRiskCase, DbType.Boolean));

        return WebServices.RiskServices.GetReports(SPA_GET_ROLE, parameters);
    }

    public DataTable GetAddedBy(string sources, string roles)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@MerchantNumber", SessionManager.CurrentMerchantNumber, DbType.AnsiString));
        parameters.Add(new FilterParameter("@NotesSourceList", sources, DbType.AnsiString));
        parameters.Add(new FilterParameter("@RoleList", roles, DbType.AnsiString));
        parameters.Add(new FilterParameter("@IsHasMerchantProfile", IsHasMerchantProfile, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsHasRiskReport", IsHasRiskReport, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsHasOpenNewCase", IsHasOpenNewCase, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsHasOpenRiskCase", IsHasOpenRiskCase, DbType.Boolean));

        return WebServices.RiskServices.GetReports(SPA_GET_ADDED_USERS, parameters);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.AddNote:
                FilterParameterCollection parameters = new FilterParameterCollection();
                FilterParameterCollection outParameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.Add(new FilterParameter("@NoteSourceId", NoteSourceDefaultForIns, DbType.Int32));
                parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
                parameters.Add(new FilterParameter("@Comment", EncryptComment(uxComment.Content), DbType.AnsiString));
                parameters.Add(new FilterParameter("@CommentPlainText", EncryptComment(uxComment.Text.Replace("\n", "")), DbType.String));
                parameters.Add(new FilterParameter("@CommentType", null, DbType.AnsiString));

                WebServices.CsReportServices.ExecuteNonQueryCommand("spa_MerchantNotes_InsNote", parameters, out outParameters);
                uxComment.Content = string.Empty;
                //43358 - VW Risk CR-Design Change #2 to 39919 New Sort
                RebindNoteGrid();
                break;
            case PostBackAction.Rebind: //rebind the note grid from the risk page
                RebindNoteGrid();
                break;
        }
    }

    private void RebindNoteGrid()
    {
        DataRow[] datarow = GetRoles(HandleString(uxSourceList.SelectedItems)).Select(string.Format("HierarchyID = '{0}'", SessionManager.CurrentHierarchyId));

        //43358 - VW Risk CR-Design Change #2 to 39919 New Sort
        ClearSort();

        //With user add comment in the first time must rebind role list OP #7060
        if (datarow.Count() == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "rebindRoleAndUser", "bindRoleAndUser(false)", true);
        }
        uxMerchantNoteGrid.Rebind();
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.AddNote, sender);
    }

    protected void uxBtnRefresh_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Rebind, sender);
    }
    public void BindMerchantNoteGirdInitiateFromRiskReport()
    {
        BindDataMultiSelector();
        uxMerchantNoteGrid.Rebind();
    }

    public void BindDataMultiSelector()
    {
        //10/4/2018 | 45128 Merchant Notes Issue - Prod
        if (!string.IsNullOrEmpty(this.MerchantNumber) && !IsAlreadyGetSourceRoleUserDataMerchantNote)
        {
            OnDataBindControls(DataBindAction.BindSourceList, uxSourceList);
            uxSourceList.SetSelectedValue(NoteSourceDefault.Split(',').ToArray());
            KeepFilterMerchantNote();
            OnDataBindControls(DataBindAction.BindRoleList, uxRoleList);
            OnDataBindControls(DataBindAction.BindAddedByList, uxAddedByList);
            IsAlreadyGetSourceRoleUserDataMerchantNote = true;
        }
    }

    protected void uxApplyFilter_Click(object sender, EventArgs e)
    {
        //43358 - VW Risk CR-Design Change #2 to 39919 New Sort
        ClearSort();
        KeepFilterMerchantNote();
        uxMerchantNoteGrid.Rebind();
    }

    protected void uxSourceList_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.BindRoleList);
        OnDataBindControls(DataBindAction.BindAddedByList);
    }

    protected void uxRoleList_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.BindAddedByList);
    }

    protected void uxExportTop_NeedExportConfig(object sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        var fileName = string.Format("{0}-{1}_{2}", GetLocalResourceObject("uxMerchantNoteFileName").ToString(), MerchantNumber,
            DateTime.Now.ToString("MM/dd/yyyy"));
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(fileName);
        exportConfig.ReportHeader = string.Format("{0} - {1} - {2}", GetLocalResourceObject("uxMerchantNoteTitleResource1.Text").ToString(), MerchantNumber,
            DateTime.Now.ToString("MM/dd/yyyy"));

        uxMerchantNoteGrid.Columns.FindByUniqueName("CardView").Visible = false;
        uxMerchantNoteGrid.Columns.FindByUniqueName("NotesSourceDesc").Visible = true;
        uxMerchantNoteGrid.Columns.FindByUniqueName("Comment").Visible = true;
        uxMerchantNoteGrid.Columns.FindByUniqueName("CaseNumberTitle").Visible = true;
        uxMerchantNoteGrid.Columns.FindByUniqueName("HierarchyName").Visible = true;
        uxMerchantNoteGrid.Columns.FindByUniqueName("CreatedByFullName").Visible = true;
        uxMerchantNoteGrid.Columns.FindByUniqueName("CreatedDate").Visible = true;
        uxMerchantNoteGrid.Columns.FindByUniqueName("UpdatedByFullName").Visible = true;
        uxMerchantNoteGrid.Columns.FindByUniqueName("UpdatedDate").Visible = true;
    }

    protected void uxMerchantNoteGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindGridMerchantNote, sender);
    }

    protected void uxMerchantNoteGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {

        GridDataItem dataItem = e.Item as GridDataItem;
        if (dataItem.IsNotNullData())
        {
            DataRowView rowView = e.Item.DataItem as DataRowView;
            var cardViewColumnCtr = dataItem["CardView"];
            HtmlGenericControl colCaseIdColumn = cardViewColumnCtr.FindControl("caseIdCol") as HtmlGenericControl;
            HtmlGenericControl colAddedByColumn = cardViewColumnCtr.FindControl("ucAddedBy") as HtmlGenericControl;
            HtmlGenericControl colcomment = cardViewColumnCtr.FindControl("ucComment") as HtmlGenericControl;


            colCaseIdColumn.InnerHtml = GetCaseInfo(rowView, cardViewColumnCtr);
            colcomment.InnerHtml = GetRiskNote(rowView);
            colAddedByColumn.InnerText = GetCreatedInfo(rowView);
            if (IsPermissionMerchantNote(rowView, EditMerchantNotePermission))
            {
                var lnkEdit = cardViewColumnCtr.FindControl("uxEditNote") as AS.Controls.Global.LinkButton;
                var urlEditNote = ProcessMerchantNoteUrl("rm_MCF_MerchantNoteToEdit", SessionManager.CurrentMerchantNumber, rowView["MerchantNoteID"].ToString(), rowView["NoteSourceID"].ToString());
                lnkEdit.OnClientClick = string.Format("return ShowPopupModal('{0}','auto');", urlEditNote);
                lnkEdit.Visible = true;
            }
            if (IsPermissionMerchantNote(rowView, DeleteMerchantNotePermission))
            {
                var lnkDelete = cardViewColumnCtr.FindControl("uxDeleteNote") as AS.Controls.Global.LinkButton;
                var urlDeleteNote = ProcessMerchantNoteUrl("rm_MCF_MerchantNoteToDelete", SessionManager.CurrentMerchantNumber, rowView["MerchantNoteID"].ToString(), rowView["NoteSourceID"].ToString());
                lnkDelete.OnClientClick = string.Format("return ShowPopupModal('{0}','auto');", urlDeleteNote);
                lnkDelete.Visible = true;
            }
            AS.Controls.Global.Button sortIcon = cardViewColumnCtr.FindControl("uxSortIcon_" + SessionManager.CurrentSortColumn) as AS.Controls.Global.Button;
            if (sortIcon.IsNotNullData())
            {
                sortIcon.CssClass = SortIconClass(SessionManager.CurrentSortColumn);
            }
            GetEditInfo(rowView, cardViewColumnCtr);
        }
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

    private string ExcludeDefaultSettingByPermission(string defaultSetting)
    {
        //Doesn't config before
        if (string.IsNullOrEmpty(defaultSetting))
            return string.Empty;

        string[] arrDefaultSetting = defaultSetting.Split(',');
        if (!IsHasMerchantProfile)
            arrDefaultSetting = arrDefaultSetting.Where(m => m != "1").ToArray();
        if (!IsHasRiskReport)
        {
            arrDefaultSetting = arrDefaultSetting.Where(m => m != "2").ToArray();
        }
        if (!IsHasOpenNewCase)
        {
            arrDefaultSetting = arrDefaultSetting.Where(m => m != "3").ToArray();
        }
        if (!IsHasOpenRiskCase)
        {
            arrDefaultSetting = arrDefaultSetting.Where(m => m != "4").ToArray();
        }
        if (!IsHasShadowUnderwriting)
        {
            arrDefaultSetting = arrDefaultSetting.Where(m => m != "5").ToArray();
        }
        return string.Join(",", arrDefaultSetting);
    }

    public void KeepFilterMerchantNote()
    {
        MerchantNoteFilterOption m = new MerchantNoteFilterOption();
        m.NotesSourceList = HandleString(uxSourceList.SelectedItems);
        m.RoleList = uxhdRole.Value; //HandleString(uxRoleList.SelectedItems);
        m.UserList = uxhdAddedBy.Value;
        MerchantNoteFilterOption = m;
    }
    #endregion

    protected void uxMerchantNoteGrid_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "refeshMultiSelector", "reCreateMultiSelector()", true);
    }
    protected void uxMerchantNoteGrid_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "refeshMultiSelector", "reCreateMultiSelector()", true);
    }
    protected void uxMerchantNoteGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.StartsWith("Sort_"))
        {
            var columnName = e.CommandName.Split('_')[1];
            string asc = string.Format("{0} {1}", columnName, Sort.Asc.ToString());
            string desc = string.Format("{0} {1}", columnName, Sort.Desc.ToString());
            //clear sort with another column
            if (!SessionManager.MerchantNoteSort.IsNullOrEmpty() && !SessionManager.MerchantNoteSort.Contains(columnName))
                SessionManager.MerchantNoteSort = string.Empty;
            if (SessionManager.MerchantNoteSort.IsNullOrEmpty())
                SessionManager.MerchantNoteSort = desc;
            else if (SessionManager.MerchantNoteSort.Contains(desc))
                SessionManager.MerchantNoteSort = asc;
            else if (SessionManager.MerchantNoteSort.Contains(asc))
                SessionManager.MerchantNoteSort = string.Empty;

            SessionManager.CurrentSortColumn = columnName;
            uxMerchantNoteGrid.CurrentPageIndex = 0;
            uxMerchantNoteGrid.Rebind();
        }
    }

    public string SortIconClass(string columnName)
    {
        if (SessionManager.MerchantNoteSort.Contains(string.Format("{0} {1}", columnName, Sort.Desc.ToString())))
            return "rgSortAsc";
        if (SessionManager.MerchantNoteSort.Contains(string.Format("{0} {1}", columnName, Sort.Asc.ToString())))
            return "rgSortDesc";
        return "hide";
    }

    public void ClearSort()
    {
        SessionManager.CurrentSortColumn = string.Empty;
        SessionManager.MerchantNoteSort = string.Empty;
        uxMerchantNoteGrid.CurrentPageIndex = 0;
    }

    private string EncryptComment(string comment)
    {
        string lstCard = hdCardDetected.Value.TrimEnd(',');
        if (!string.IsNullOrEmpty(lstCard))
        {
            foreach (var item in lstCard.Split(','))
            {
                string realItem = item.Replace(" ", "").Replace("-", "");
                string replaceItem = realItem.Substring(0, 6) + "xxxxxx" + realItem.Substring(realItem.Length - 4, 4);
                comment = comment.Replace(item, replaceItem);
            }
        }

        return comment;
    }

    protected void uxFinishSaveDefaultSettingMN_Click(object sender, EventArgs e)
    {
        ClearSort();
        KeepFilterMerchantNote();
        uxMerchantNoteGrid.Rebind();
    }

    //Enhance using multi-thread
    public FilterParameterCollection GetMerchantNoteParameters(DataBindAction action)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        switch (action)
        {
            case DataBindAction.BindSourceList:
                {
                    parameters.Add(new FilterParameter("@IsHasMerchantProfile", IsHasMerchantProfile, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasRiskReport", IsHasRiskReport, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasOpenNewCase", IsHasOpenNewCase, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasOpenRiskCase", IsHasOpenRiskCase, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasShadowUnderwriting", IsHasShadowUnderwriting, DbType.Boolean));
                    parameters.AddLanguageID();
                    break;
                }
            case DataBindAction.BindRoleList:
                {
                    parameters.Add(new FilterParameter("@MerchantNumber", SessionManager.CurrentMerchantNumber, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@NotesSourceList", NoteSourceDefault.ToString(), DbType.AnsiString));
                    parameters.Add(new FilterParameter("@IsHasMerchantProfile", IsHasMerchantProfile, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasRiskReport", IsHasRiskReport, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasOpenNewCase", IsHasOpenNewCase, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasOpenRiskCase", IsHasOpenRiskCase, DbType.Boolean));
                    break;
                }
            case DataBindAction.BindAddedByList:
                {
                    parameters.Add(new FilterParameter("@MerchantNumber", SessionManager.CurrentMerchantNumber, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@NotesSourceList", NoteSourceDefault.ToString(), DbType.AnsiString));
                    parameters.Add(new FilterParameter("@RoleList", HandleString(uxRoleList.SelectedItems), DbType.AnsiString));
                    parameters.Add(new FilterParameter("@IsHasMerchantProfile", IsHasMerchantProfile, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasRiskReport", IsHasRiskReport, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasOpenNewCase", IsHasOpenNewCase, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasOpenRiskCase", IsHasOpenRiskCase, DbType.Boolean));
                    break;
                }
        }

        return parameters;
    }

    private string ProcessMerchantNoteUrl(string ctrlName, string merchantNumber, string merchantNoteId, string noteSourceId)
    {
        string urlModal = ResolveUrl("~/Risk_MCF/" + ctrlName + ".aspx?") + Page.BuildSecureQueryString(string.Format("MerchantNumber={0}&MerchantNoteID={1}&NoteSourceID={2}", merchantNumber, merchantNoteId, noteSourceId));
        return urlModal;
    }

    private static bool IsPermissionMerchantNote(DataRowView rowView, bool rolePermission)
    {
        if (rowView == null)
            return false;
        return rolePermission && rowView["NoteSourceID"].ToString() == WebSiteConstants.NoteSourceRiskReport && !rowView["IsDeleted"].ToBoolean();
    }

    private static string GetCreatedInfo(DataRowView rowView)
    {
        return string.Format("{0} - {1}", rowView["CreatedByFullName"].ToString(), rowView["HierarchyName"].ToString());
    }
    private string GetRiskNote(DataRowView rowView)
    {
        if (rowView["NoteSourceID"].ToString() == WebSiteConstants.NoteSourceRiskReport && rowView["IsDeleted"].ToBoolean())
        {
            string resourceCommentofDelete = string.Format(GetLocalResourceObject("uxCommentOfDeletedUser.Text").ToString(), rowView["DeletedByFullName"].ToString(), rowView["DeletedDate"].ToString());
            return string.Format("<div class='red'>{0}</div>", resourceCommentofDelete);
        }
        return rowView["Comment"].ToString().Replace("&nbsp;", " ");
    }
    private string GetCaseInfo(DataRowView rowView, TableCell cardViewColumnCtr)
    {
        string url = ResolveUrl("~/JumpToCase.aspx?") + string.Format("type=6&cid={0}", rowView["CaseID"]);
        string caseIdHyperlink = string.Format("<a onclick=\"parent.openPopupWindowOnMenu(event,'{0}', 'OpenNewCase'); return false;\" href=\"#\">{1}</a>", url, rowView["CaseNumberTitle"]);
        if (rowView["CaseID"].IsNullOrEmpty())
        {
            HtmlGenericControl divCase = cardViewColumnCtr.FindControl("ucCase") as HtmlGenericControl;
            divCase.Attributes.Add("class", "vis-hidden col-md-1");
        }
        return caseIdHyperlink;
    }

    private static void GetEditInfo(DataRowView rowView, TableCell cardViewColumnCtr)
    {
        if (rowView["NoteSourceID"].ToString() != WebSiteConstants.NoteSourceRiskReport)
        {
            HtmlGenericControl divUpdatedDate = cardViewColumnCtr.FindControl("ucUpdatedDate") as HtmlGenericControl;
            divUpdatedDate.Attributes.Add("class", "vis-hidden");
            HtmlGenericControl divUpdatedBy = cardViewColumnCtr.FindControl("ucUpdatedBy") as HtmlGenericControl;
            divUpdatedBy.Attributes.Add("class", "vis-hidden");
            return;
        }
        var updatedDate = rowView["UpdatedDate"].ToString();
        var updatedByFullName = rowView["UpdatedByFullName"].ToString();
        if (string.IsNullOrEmpty(updatedDate))
        {
            HtmlGenericControl ucTextUpdatedDate = cardViewColumnCtr.FindControl("ucTextUpdatedDate") as HtmlGenericControl;
            ucTextUpdatedDate.InnerHtml = WebSiteConstants.HTML_EM_DASH_ENCODE;
        }
        if (string.IsNullOrEmpty(updatedByFullName))
        {
            HtmlGenericControl ucTextUpdatedBy = cardViewColumnCtr.FindControl("ucTextUpdatedBy") as HtmlGenericControl;
            ucTextUpdatedBy.InnerHtml = WebSiteConstants.HTML_EM_DASH_ENCODE;
        }
    }
    #region SPA INFO

    public SpaInfo SpaGetSource
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindSourceList.ToString(),
                SpaName = SPA_GET_SOURCE,
                Parameters = GetMerchantNoteParameters(DataBindAction.BindSourceList)
            };
        }
    }

    public SpaInfo SpaGetRole
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindRoleList.ToString(),
                SpaName = SPA_GET_ROLE,
                Parameters = GetMerchantNoteParameters(DataBindAction.BindRoleList)
            };
        }
    }

    public SpaInfo SpaGetUser
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindAddedByList.ToString(),
                SpaName = SPA_GET_ADDED_USERS,
                Parameters = GetMerchantNoteParameters(DataBindAction.BindAddedByList)
            };
        }
    }

    #endregion
}

[Serializable]
public class MerchantNoteFilterOption
{
    public string NotesSourceList { get; set; }
    public string RoleList { get; set; }
    public string UserList { get; set; }
}
