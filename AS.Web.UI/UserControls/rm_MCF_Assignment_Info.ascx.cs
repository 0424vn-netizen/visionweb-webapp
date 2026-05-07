using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Common.DBManager;
using AS.Common;
using System.Collections.Generic;
using System.Text;
using Telerik.Web.UI;
using AS.Web.Business.Shared.Constants;
using BusinessGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;

public partial class UserControls_rm_MCF_Assignment_Info : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        GetUsersSelected,
        GetGroupsSelected,
        LoadAssignmentType
    }
    enum PostBackAction
    {
    }
    #endregion

    #region Constants

    /// <summary>
    /// spa_rm_cs_UpdateAssignment
    /// </summary>
    private const string SPA_UPDATE_ASSIGNMENT = "spa_RM_MCF_UpdateAssignment";
    /// <summary>
    /// spa_rm_cs_CheckDuplicateAssignmentName
    /// </summary>
    private const string SPA_CHECK_DUPLICATE_ASSIGNMENT_NAME = "spa_RM_MCF_CheckDuplicateAssignmentName";
    /// <summary>
    /// spa_rm_cs_GetAssignmentInfo
    /// </summary>
    private const string SPA_GET_ASSIGNMENT_INFO = "spa_RM_MCF_GetAssignmentInfo";

    #endregion Constants

    #region Events

    public event EventHandler SelectedAssignmentTypeChanged;
    public event GetAssInfoEvent GetAssInfo;
    public delegate void GetAssInfoEvent(DataTable data);

    #endregion Events

    #region Properties

    public bool HasQueuingMechanism
    {
        get
        {
            return GeneralFuncsLib.HasQueuingMechanismFeature;
        }
    }

    private bool? _enableDetectionQueueDistinct = null;
    private bool EnableDetectionQueueDistinct
    {
        get
        {
            if (!_enableDetectionQueueDistinct.HasValue)
            {
                _enableDetectionQueueDistinct = GeneralFuncsLib.IsDistinctAssignment();
            }
            return _enableDetectionQueueDistinct.Value;
        }
    }

    public bool IsDetectionQueueAssignment
    {
        get
        {
            return !HasQueuingMechanism
                || uxComboAssignmentType.Text.Equals(GetLocalResourceObject("LiteralDetectionQueue.Text").ToString())
                || uxComboAssignmentType.SelectedValue == ((int)WebSiteEnums.AssignmentType.Subsite).ToString();
        }
    }

    public WebSiteEnums.FeatureMode FeatureMode { get; set; }

    private string _QueryString;
    public string QueryString
    {
        get
        {
            string asTypeQuery = string.Format("&AssignmentType={0}",
                IsDetectionQueueAssignment ? (int)WebSiteEnums.AssignmentType.DetectionQueue
                : (int)WebSiteEnums.AssignmentType.WorkQueue);

            _QueryString = Page.BuildSecureQueryString("AssignmentID=" + PrimaryID + asTypeQuery);
            return _QueryString;
        }
        set
        {
            _QueryString = value;
        }
    }

    public WebSiteEnums.FeatureMode IsCreateMode
    {
        get
        {
            if (Page.SecureQueryString["IsCreateMode"] != null)
            {

                return WebSiteEnums.FeatureMode.Create;
            }
            else
            {
                return FeatureMode;
            }
        }
    }

    public int NewAssignmentID
    {
        get
        {
            if (Page.SecureQueryString["NewAsignmentID"] != null)
            {

                return int.Parse(Page.SecureQueryString["NewAsignmentID"].ToString());
            }
            else
            {
                return -1;
            }
        }
    }


    #endregion


    private void VisibleControls()
    {
        bool isEditMode = FeatureMode == WebSiteEnums.FeatureMode.Edit;
        plhGroups.Visible = isEditMode;
        plhUsers.Visible = isEditMode;
        phSubsite.Visible = isEditMode;
        uxExpirationDate.DatePopupButton.Visible = isEditMode;
        // Set visibility for AssignmentType dropdownbox.
        uxPlaceHolderAssignmentType.Visible = HasQueuingMechanism;

        bool isViewMode = FeatureMode == WebSiteEnums.FeatureMode.View;
        uxAssignmentName.ReadOnly = isViewMode;
        uxExpirationDate.DateInput.ReadOnly = isViewMode;
        uxradExpirationDate.Enabled = uxradNerverExpirationDate.Enabled
                = uxChkDistinctExclude.Enabled = !isViewMode;

        uxComboAssignmentType.Enabled = IsCreateMode == WebSiteEnums.FeatureMode.Create;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Load AssignmentType combobox
            OnDataBindControls(DataBindAction.LoadAssignmentType);
            GetAssignmentInfo();
            SetLabels();
            VisibleControls();
            if (HasQueuingMechanism)
            {
                uxComboAssignmentType_SelectedIndexChanged(uxComboAssignmentType, null);
            }
            ProcessForSubsite();
            ShowHideStartDate();
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.GetGroupsSelected:
                SetGroupInfo();
                break;
            case DataBindAction.GetUsersSelected:
                SetUserInfo();
                break;
            case DataBindAction.LoadAssignmentType:
                if (HasQueuingMechanism)
                {
                    InitializeAssignmentType();
                }
                break;
        }
    }

    protected void uxRebindAssignmentUsers_Click(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.GetUsersSelected);
    }

    protected void uxRebindAssignmentGroups_Click(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.GetGroupsSelected);
    }

    //ASSIGNMENT INFORMATION
    private void GetAssignmentInfo()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(PrimaryID), DbType.Int32));
        DataTable dt = WebServices.RiskServices.GetReports(SPA_GET_ASSIGNMENT_INFO, parameters);
        uxAssignmentName.Text = VeraCodeSolution.GetOutputHtmlString(GeneralFuncsLib.NvlString(dt.Rows[0]["AssignmentName"]));

        if (this.GetAssInfo != null)
        {
            this.GetAssInfo(dt);
        }
        var currentstartDate = GetValueInfoToDate(dt, ManageAssignmentConstanst.COLUMN_NAME_START_DATE, GetDateToNow());
        var tempExpirationDate = currentstartDate.AddDays(7);
        var currentExpirationDate = GetValueInfoToDate(dt, ManageAssignmentConstanst.COLUMN_NAME_EXPIRATION_DATE, tempExpirationDate);

        uxStartDate.SelectedDate = currentstartDate;
        if (currentExpirationDate.Year == 2999)
        {
            uxradNerverExpirationDate.Checked = true;
            uxradExpirationDate.Checked = false;
            uxExpirationDate.Enabled = false;
            currentExpirationDate = tempExpirationDate;
        }
        else
        {
            uxradNerverExpirationDate.Checked = false;
            uxradExpirationDate.Checked = true;
            uxExpirationDate.Enabled = true;
        }
        uxExpirationDate.SelectedDate = currentExpirationDate;
        // Set text for ComboBox Assignment Type
        if (HasQueuingMechanism)
        {
            var assignmentType = GeneralFuncsLib.NvlString(dt.Rows[0]["AssignmentType"]);
            string selectedValue = assignmentType;
            if (string.IsNullOrEmpty(assignmentType))
            {
                selectedValue = ((int)WebSiteEnums.AssignmentType.DetectionQueue).ToString();
            }

            AddSubsiteForAssignmentTypeCombobox(assignmentType);
            uxComboAssignmentType.Items.FindItemByValue(selectedValue).Selected = true;

        }

        // Set value for Distinct Exclude checkbox
        if (EnableDetectionQueueDistinct)
        {
            var distinctExclude = GeneralFuncsLib.NvlString(dt.Rows[0]["DistinctExclude"]);
            uxChkDistinctExclude.Checked = !string.IsNullOrEmpty(distinctExclude)
                && bool.Parse(distinctExclude);

        }

        // AssignmentType can be edited only if we create new Assginment.
        uxComboAssignmentType.Enabled = string.IsNullOrEmpty(uxAssignmentName.Text);
        //Set min-max date for StartDate, ExpirationDate
        RefershCalendarAssignmentOfDate(currentstartDate, currentExpirationDate);
    }

    //CHECK DUPLICATE ASSIGNMENT NAME
    public bool CheckDuplicatedAssignmentName()
    {
        // Set default value before check validation
        uxAssignmentNameMsg.Message = string.Empty;
        uxAssignmentNameMsg.ShowOnLoad = false;
        VltLblAssignmentName.CssClass = VltLblAssignmentName.CssClass.Replace("label-error", string.Empty);

        bool isDuplicated = false;
        if (uxAssignmentName.Text.Length > 0)
        {
            FilterParameterCollection paramsIn = new FilterParameterCollection();
            paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paramsIn.Add(new FilterParameter("@AssignmentID", Int32.Parse(PrimaryID), DbType.Int32));
            paramsIn.Add(new FilterParameter("@AssignmentName", uxAssignmentName.Text, DbType.AnsiString));
            paramsIn.Add(new FilterParameter("@ReturnValue", 0, DbType.Int32, true));
            FilterParameterCollection paramsOut = new FilterParameterCollection();
            WebServices.RiskServices.ExecuteNonQueryCommand(SPA_CHECK_DUPLICATE_ASSIGNMENT_NAME, paramsIn, out paramsOut);
            isDuplicated = paramsOut[0].ParameterValue.ToString().Contains("1");
        }

        if (isDuplicated)
        {
            uxAssignmentNameMsg.Message = AS.Common.VeraCodeSolution.DoVeraCode(Resources.ValMsg.Required_Unique);
            uxAssignmentNameMsg.ShowOnLoad = true;
            VltLblAssignmentName.CssClass += " label-error";
        }

        return isDuplicated;
    }

    public void SetInfoDuplicate()
    {
        uxAssignmentName.Text = string.Empty;
        uxAssignmentName.Attributes.Add("placeholder", GetLocalResourceObject("DuplicateAssignmentNameResource1").ToString());
    }

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    public string CustomViewID { get; set; }

    public int Save()
    {
        if (CheckDuplicatedAssignmentName())
            return -1;

        var assignmentType = int.Parse(uxComboAssignmentType.SelectedValue.ToString());
        var isDistinctExclude = uxChkDistinctExclude.Checked ? WebSiteEnums.DetectionQueueExcludeType.Exclude : WebSiteEnums.DetectionQueueExcludeType.NotExclude;
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", Int32.Parse(PrimaryID), DbType.Int32));
        paramsIn.Add(new FilterParameter("@AssignmentName", uxAssignmentName.Text, DbType.AnsiString));
        paramsIn.Add(new FilterParameter("@StartDate", GeneralFuncsLib.IsFieldStartDateOfRiskMgnt(assignmentType.ToString()) ? uxStartDate.SelectedDate : null, DbType.DateTime));
        paramsIn.Add(new FilterParameter("@ExpirationDate", uxradExpirationDate.Checked ? uxExpirationDate.SelectedDate : new DateTime(2999, 12, 12), DbType.DateTime));

        if (HasQueuingMechanism)
        {
            paramsIn.Add(new FilterParameter("@AssignmentType", assignmentType, DbType.Int32));
        }

        if (EnableDetectionQueueDistinct && uxPlaceHolderDistinctExclude.Visible)
        {
            paramsIn.Add(new FilterParameter("@DistinctExclude", (int)isDistinctExclude, DbType.Int32));
        }

        if (!string.IsNullOrEmpty(CustomViewID))
        {
            paramsIn.Add(new FilterParameter("@CustomViewID", int.Parse(CustomViewID), DbType.Int32));
        }

        FilterParameterCollection paramsOut;
        WebServices.RiskServices.ExecuteNonQueryCommand(SPA_UPDATE_ASSIGNMENT, paramsIn, out paramsOut);
        return Convert.ToInt32(PrimaryID);
    }

    public event EventHandler SelectedChanged;

    protected void InitializeAssignmentType()
    {
        uxComboAssignmentType.Items.Add(new AS.Controls.Global.RadComboBoxItem()
        {
            Value = ((int)WebSiteEnums.AssignmentType.DetectionQueue).ToString(),
            Text = GetLocalResourceObject("LiteralDetectionQueue.Text").ToString()
        });
        uxComboAssignmentType.Items.Add(new AS.Controls.Global.RadComboBoxItem()
        {
            Value = ((int)WebSiteEnums.AssignmentType.WorkQueue).ToString(),
            Text = GetLocalResourceObject("LiteralWorkQueue.Text").ToString()
        });

        uxComboAssignmentType.DataBind();
    }

    /// We will do 2 tasks in this event:
    /// 1. Raise the event to its parent page (rm_Assignment_CreateNew.aspx)
    /// so that the parent page will update visibility of UxParam & UxFilter.
    /// 2.Set visibility of the Distinct Exclude check box which is visible
    /// only if GeneralFuncsLib.IsDistinctAssignment() = true
    /// and the current assignment has type 'Detection Queue'
    protected void uxComboAssignmentType_SelectedIndexChanged(object sender,
        Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        // Raise the event to rm_Assignment_CreateNew.aspx.cs so that it can update
        // the visibility of UxParam and UxFilter.
        if (SelectedAssignmentTypeChanged != null)
        {
            SelectedAssignmentTypeChanged(sender, e);
        }

        SetVisibilityOfDistinctExcludeCheckBox();
        ProcessForSubsite();
        ShowHideStartDate();
        uxExpirationDate.Enabled = uxradExpirationDate.Checked;
        uxExpirationDate.DateInput.ReadOnly = true;
    }

    private void SetVisibilityOfDistinctExcludeCheckBox()
    {
        uxPlaceHolderDistinctExclude.Visible = EnableDetectionQueueDistinct
            && uxComboAssignmentType.SelectedValue ==
                ((int)WebSiteEnums.AssignmentType.DetectionQueue).ToString();
    }

    private void SetLabels()
    {
        SetGroupInfo();

        SetUserInfo();

        SetuSubsiteDistributionInfo();
    }

    private void SetGroupInfo()
    {
        Dictionary<string, string> data = UserControls_rm_MCF_Assignment_Group.GetSelectedValuesAsString(Mode, PrimaryID, null, null);

        uxLabelAssignmentGroups.Text = data["Label"];
        uxLabelAssignmentGroups.Attributes.Add("tracking-value", data["Tracking"]);
    }

    private void SetUserInfo()
    {
        Dictionary<string, string> data = UserControls_rm_MCF_Assignment_User.GetSelectedValuesAsString(Mode, PrimaryID, null, null);

        uxLabelAssignmentUsers.Text = data["Label"];
        uxLabelAssignmentUsers.Attributes.Add("tracking-value", data["Tracking"]);
    }

    private void SetuSubsiteDistributionInfo()
    {
        var datas = GetFilterList(WebSiteEnums.AssignmentFilterModes.Assigned, Int32.Parse(PrimaryID), (int)Mode);
        var trackingData = BuildeSelectedValues(datas);
        var notAvailableText = GetLocalResourceObject("NA").ToString();
        var message = notAvailableText;

        if (trackingData["Label"] != notAvailableText)
            message = GetLocalResourceObject("SubsiteDistributionMessage") + trackingData["Label"];

        uxLabelSubsiteDistribution.Text = message;
        uxLabelSubsiteDistribution.Attributes.Add("tracking-value", trackingData["Tracking"]);
    }

    #endregion

    protected void uxRebindRebindDistribution_Click(object sender, EventArgs e)
    {
        SetuSubsiteDistributionInfo();
    }

    protected static DateTime GetDateToNow()
    {
        return BusinessGeneralFuncsLib.GetDateToNow().Date;
    }

    private DataTable GetFilterList(WebSiteEnums.AssignmentFilterModes whichMode, int primaryID, int mode)
    {
        var currentUser = SessionManager.CurrentUser;
        FilterParameterCollection parames = new FilterParameterCollection();

        parames.Add(new FilterParameter("@UserMode", GeneralFuncsLib.GetUserMode(), DbType.AnsiString));
        parames.Add(new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString));
        parames.Add(new FilterParameter("@ASClientID", currentUser.ASClient, DbType.Int32));

        if (currentUser.SiteID >= 0)
            parames.Add(new FilterParameter("@SiteID", currentUser.SiteID, DbType.Int32));

        parames.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        parames.Add(new FilterParameter("@OptionID", whichMode, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", mode, DbType.Int32));
        parames.Add(new FilterParameter("@ItemCode", "SubSiteDistribution", DbType.String));
        parames.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
        var data = WebServices.RiskServices.GetReportsAsDataSet("spa_MCF_Get_AssignmentItem", parames);
        if (data.Tables.Count > 0)
        {
            return data.Tables[0];
        }

        return new DataTable();
    }
    private Dictionary<string, string> BuildeSelectedValues(DataTable dt)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        var defaultValueNA = GetLocalResourceObject("NA").ToString();

        var trackingList = new List<string>();
        StringBuilder str = new StringBuilder();

        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str.Append(dt.Rows[i]["DataText"] + ", ");
                trackingList.Add(dt.Rows[i]["DataText"].ToString());
            }
        }

        string lblVal = string.IsNullOrEmpty(str.ToString()) ? defaultValueNA : str.ToString().Trim().TrimEnd(",".ToCharArray());
        string tracking;

        if (trackingList.Any())
        {
            tracking = string.Join(",", trackingList);
        }
        else
        {
            tracking = "N/A";
        }

        data.Add("Label", lblVal);
        data.Add("Tracking", tracking);

        return data;
    }
    private void ProcessForSubsite()
    {
        var currentType = uxComboAssignmentType.SelectedValue;
        if (currentType.Equals(((int)WebSiteEnums.AssignmentType.Subsite).ToString()))
        {
            uxPhUserGroup.Visible = false;
            uxPhSubsiteDistribution.Visible = true;

            //process for MS
            var currentUserType = SessionManager.CurrentUserType;
            if (currentUserType != WebSiteEnums.UserHierarchyMode.CS && currentUserType != WebSiteEnums.UserHierarchyMode.AS)
            {
                uxPlaceHolderDistinctExclude.Visible = true;
                uxPhUserGroup.Visible = true;
                uxradNerverExpirationDate.Enabled = false;
                uxradExpirationDate.Enabled = false;
                uxExpirationDate.Enabled = false;
                uxPhSubsiteDistribution.Visible = false;

                if (IsCreateMode != WebSiteEnums.FeatureMode.Create)
                    uxAssignmentName.Enabled = false;
            }
            else
            {
                uxLabelAssignmentGroups.Text = "N/A";
                uxLabelAssignmentUsers.Text = "N/A";
            }
        }
        else
        {
            uxPhUserGroup.Visible = true;
            uxPhSubsiteDistribution.Visible = false;
            uxLabelSubsiteDistribution.Text = "N/A";
        }
    }
    private void AddSubsiteForAssignmentTypeCombobox(string assignmentType)
    {
        var isEditMode = IsCreateMode != WebSiteEnums.FeatureMode.Create;

        if (Page.IsUserWithPermission("SubsiteAssignment")
            || (isEditMode && assignmentType.Equals(((int)WebSiteEnums.AssignmentType.Subsite).ToString())))
        {
            uxComboAssignmentType.Items.Add(new AS.Controls.Global.RadComboBoxItem()
            {
                Value = ((int)WebSiteEnums.AssignmentType.Subsite).ToString(),
                Text = GetLocalResourceObject("LiteralSubsite.Text").ToString()
            });
            uxComboAssignmentType.DataBind();
        }
    }

    private void RefershCalendarAssignmentOfDate(DateTime startDate, DateTime expirationDate)
    {
        uxStartDate.SelectedDate = startDate;
        uxStartDate.Calendar.RangeMinDate = GetDateToNow();
        uxStartDate.DateInput.ReadOnly = true;
        if (!uxradNerverExpirationDate.Checked)
        {
            uxStartDate.Calendar.RangeMaxDate = expirationDate;
            uxExpirationDate.SelectedDate = expirationDate;
        }
        uxExpirationDate.Calendar.RangeMinDate = GetDateToNow();
        uxExpirationDate.DateInput.ReadOnly = true;
    }

    private static DateTime GetValueInfoToDate(DataTable table, string columnName, DateTime defaultDate)
    {
        var isColumn = table.Columns.Contains(columnName);
        if (!isColumn)
        {
            return defaultDate;
        }
        var result = BusinessGeneralFuncsLib.GetValueDataRow(table.Rows[0], columnName);
        if (string.IsNullOrEmpty(result))
        {
            return defaultDate;
        }
        return Convert.ToDateTime(result);
    }

    private void ShowHideStartDate()
    {
        var currentType = uxComboAssignmentType.SelectedValue;
        uxPlaceHolderStartDate.Visible = GeneralFuncsLib.IsFieldStartDateOfRiskMgnt(currentType);
    }

}
