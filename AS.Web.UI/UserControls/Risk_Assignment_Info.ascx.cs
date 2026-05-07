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

public partial class UserControls_Risk_Assignment_Info : GlobalUserControl, IRiskParamFilter
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
    private const string SPA_UPDATE_ASSIGNMENT = "spa_rm_cs_UpdateAssignment";
    /// <summary>
    /// spa_rm_cs_CheckDuplicateAssignmentName
    /// </summary>
    private const string SPA_CHECK_DUPLICATE_ASSIGNMENT_NAME = "spa_rm_cs_CheckDuplicateAssignmentName";
    /// <summary>
    /// spa_rm_cs_GetAssignmentInfo
    /// </summary>
    private const string SPA_GET_ASSIGNMENT_INFO = "spa_rm_cs_GetAssignmentInfo";

    #endregion Constants

    #region Events

    public event EventHandler SelectedAssignmentTypeChanged;

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
                || uxComboAssignmentType.Text.Equals(GetLocalResourceObject("LiteralDetectionQueue.Text").ToString());
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

    #endregion

    private void VisibleControls()
    {
        bool isEditMode = FeatureMode == WebSiteEnums.FeatureMode.Edit;
        plhGroups.Visible = isEditMode;
        plhUsers.Visible = isEditMode;
        uxExpirationDate.DatePopupButton.Visible = isEditMode;
        // Set visibility for AssignmentType dropdownbox.
        uxPlaceHolderAssignmentType.Visible = HasQueuingMechanism;

        bool isViewMode = FeatureMode == WebSiteEnums.FeatureMode.View;
        uxAssignmentName.ReadOnly = isViewMode;
        uxExpirationDate.DateInput.ReadOnly = isViewMode;
        uxradExpirationDate.Enabled = uxradNerverExpirationDate.Enabled
                = uxChkDistinctExclude.Enabled = uxComboAssignmentType.Enabled = !isViewMode;
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
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.GetGroupsSelected:
                uxLabelAssignmentGroups.Text = VeraCodeSolution.GetOutputHtmlString(
                    UserControls_Risk_Assignment_Group.GetSelectedValuesAsString(Mode, PrimaryID, null, null));
                break;
            case DataBindAction.GetUsersSelected:
                uxLabelAssignmentUsers.Text = VeraCodeSolution.GetOutputHtmlString(
                    UserControls_Risk_Assignment_User.GetSelectedValuesAsString(Mode, PrimaryID, null, null));
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

        if (Convert.ToDateTime(dt.Rows[0]["ExpirationDate"]).Year == 2999)
        {
            uxradNerverExpirationDate.Checked = true;
            uxradExpirationDate.Checked = false;
            uxExpirationDate.Enabled = false;
            uxExpirationDate.SelectedDate = DateTime.Now.AddDays(7);
        }
        else
        {
            uxradNerverExpirationDate.Checked = false;
            uxradExpirationDate.Checked = true;
            uxExpirationDate.Enabled = true;
            uxExpirationDate.SelectedDate = Convert.ToDateTime(dt.Rows[0]["ExpirationDate"]); 
        }
        // Set text for ComboBox Assignment Type
        if (HasQueuingMechanism)
        {
            var assignmentType = GeneralFuncsLib.NvlString(dt.Rows[0]["AssignmentType"]);
            string selectedValue = string.IsNullOrEmpty(assignmentType)
                || assignmentType.Equals(((int)WebSiteEnums.AssignmentType.DetectionQueue).ToString())
                ? ((int)WebSiteEnums.AssignmentType.DetectionQueue).ToString()
                : ((int)WebSiteEnums.AssignmentType.WorkQueue).ToString();
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

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    public int Save()
    {
        if (CheckDuplicatedAssignmentName())
            return -1;
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", Int32.Parse(PrimaryID), DbType.Int32));
        paramsIn.Add(new FilterParameter("@AssignmentName", uxAssignmentName.Text, DbType.AnsiString));
        paramsIn.Add(new FilterParameter("@ExpirationDate", uxradExpirationDate.Checked ? uxExpirationDate.SelectedDate : new DateTime(2999, 12, 12), DbType.DateTime));
        if (HasQueuingMechanism)
        {
            paramsIn.Add(new FilterParameter(
                "@AssignmentType",
                Int32.Parse(uxComboAssignmentType.SelectedValue.ToString()),
                DbType.Int32));
        }
        if (EnableDetectionQueueDistinct)
        {
            paramsIn.Add(new FilterParameter(
                "@DistinctExclude",
                uxChkDistinctExclude.Checked
                ? ((int)WebSiteEnums.DetectionQueueExcludeType.Exclude).ToString()
                : ((int)WebSiteEnums.DetectionQueueExcludeType.NotExclude).ToString(),
                DbType.Int32));
        }

        FilterParameterCollection paramsOut = new FilterParameterCollection();
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
    }

    private void SetVisibilityOfDistinctExcludeCheckBox()
    {
        uxPlaceHolderDistinctExclude.Visible = EnableDetectionQueueDistinct
            && uxComboAssignmentType.SelectedValue ==
                ((int)WebSiteEnums.AssignmentType.DetectionQueue).ToString();
    }

    private void SetLabels()
    {
        uxLabelAssignmentGroups.Text = VeraCodeSolution.GetOutputHtmlString(
            UserControls_Risk_Assignment_Group.GetSelectedValuesAsString(Mode, PrimaryID, null, null));
        uxLabelAssignmentUsers.Text = VeraCodeSolution.GetOutputHtmlString(
            UserControls_Risk_Assignment_User.GetSelectedValuesAsString(Mode, PrimaryID, null, null));
    }

    #endregion
}
