using AS.Common.DBManager;
using AS.Common.Logger;
using AS.Controls.Pages;
using AS.Web.SharedSession;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

// When user have permision of Detection Queue also access Assignment page with view mode
[PagePermission("RskManAss,MSRskManAss,RskQueue,MSRskQueue")]
public partial class rm_MCF_Assignment_CreateNew : NonReportPage
{
    #region Enums
    enum DataBindAction { }
    enum PostBackAction
    {
        SaveAssignment,
        CheckNameAssignment
    }
    #endregion
    #region Constants

    /// <summary>
    /// spa_rm_cs_SaveAssignmentToFinalTables
    /// </summary>
    private const string SPA_SAVE_ASSIGNMENT = "spa_RM_MCF_Save_Assignment";
    /// <summary>
    /// spa_rm_cs_ActivateAssignment
    /// </summary>
    private const string SPA_ACTIVATE_ASSIGNMENT = "spa_RM_MCF_ActivateAssignment";

    #endregion Constants

    #region Fields

    #endregion Fields
    #region Properties

    public string CustomViewID { get; set; }

    public bool IsDetectionQueueAssignment
    {
        get
        {
            return uxAssInfo.IsDetectionQueueAssignment;
        }
    }

    public WebSiteEnums.FeatureMode FeatureMode
    {
        get
        {
            if (SecureQueryString["FeatureMode"] != null)
            {
                int featureMode = int.Parse(SecureQueryString["FeatureMode"]);
                return featureMode == null || featureMode == 0 ? WebSiteEnums.FeatureMode.Edit : WebSiteEnums.FeatureMode.View;
            }
            else
            {
                return WebSiteEnums.FeatureMode.Edit;
            }
        }
    }
    public bool Duplicate
    {
        get
        {
            if (SecureQueryString["Duplicate"] != null && SecureQueryString["Duplicate"].ToString().Equals("1"))
            {
                return true;
            }

            return false;
        }
    }
 
    public bool IsCreateNewAssignment
    {
        get; set;
    }

    public int ReviewMode
    {
        get
        {
            if (SecureQueryString["ReviewMode"] != null)
            {
                int reviewMode = int.Parse(SecureQueryString["ReviewMode"]);
                return reviewMode == null || reviewMode == 0 ? 0 : 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public WebSiteEnums.ParamFilterMode Mode
    {
        get
        {
            if (SecureQueryString["Mode"] != null)
            {
                int mode = int.Parse(SecureQueryString["Mode"]);
                return (WebSiteEnums.ParamFilterMode)mode;
            }
            else
            {
                return WebSiteEnums.ParamFilterMode.Assignment;
            }
        }
    }

    public string AssignmentID
    {
        get
        {
            if (SecureQueryString["AssignmentID"] != null)
            {
                return SecureQueryString["AssignmentID"].ToString();
            }

            return string.Empty;
        }
    }
    #endregion Properties

    #region Methods
    public bool IsFirstLoad { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        if (!IsPostBack)
        {
            IsFirstLoad = true;
        }

        IsCreateNewAssignment = SecureQueryString["FeatureMode"] == null;
        PageType = SecurePageType.Modal;
        uxCancel.PrimaryID = uxAssInfo.PrimaryID = uxFilters.PrimaryID = uxParam.PrimaryID = AssignmentID;
        uxCancel.Mode = Mode;
        uxCancel.ReviewMode = ReviewMode;
        uxAssInfo.FeatureMode = uxFilters.FeatureMode = uxParam.FeatureMode = FeatureMode;
        uxFilters.IsCreateNewAssignment = IsCreateNewAssignment;
        uxSave.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;
        uxAssInfo.SelectedAssignmentTypeChanged += OnAssignmentTypeChangeEventHandler;
        //46430 VW Add Manage Assignment Modal to User Audit Report
        uxAssInfo.GetAssInfo += GetAssInfo;
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.CheckNameAssignment:
                int IsDuplicate = 0;
                if (uxAssInfo.CheckDuplicatedAssignmentName() == true)
                {
                    IsDuplicate = 1;
                    uxParam.RefreshParamList();
                }
                else
                    IsDuplicate = 0;
                AjaxAddResponseScript("checkDuplicateAssignmentName(" + IsDuplicate + ");");
                break;
            case PostBackAction.SaveAssignment:
                uxAssInfo.CustomViewID = uxCustomView.CustomViewID;
                int error = uxAssInfo.Save();
                if (error == -1)
                    return;
                // If create a detection queue assignment, perform these steps (Filters & Param saving).
                if (uxFilters.Visible)
                {
                    error = uxFilters.Save();
                    var errorPMA = uxFilters.SavePauseMerchantAlert();
                    if (error != 0 || errorPMA!=0)
                    {
                        return;
                    }
                }
                if (uxParam.Visible)
                {
                    uxParam.Save();
                }
                ActivateAssignment();
                //TK36801 – MCPS – VW Supplemental MIF – FE
                if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.CreaditCore))
                {
                    uxFilters.SaveCreditScore();
                }
                SaveAssignmentToFinalTables();

                if (FeatureMode == WebSiteEnums.FeatureMode.View)
                    AjaxAddResponseScript("parent.HidePopupModal();");
                else
                    AjaxAddResponseScript("parent.RebindAndShowStausWhenCloseModal();");
                break;
        }
    }

    protected void uxSave_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SaveAssignment, sender);
    }

    protected void uxCheckDuplicateName_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CheckNameAssignment, sender);
    }

    protected void ActivateAssignment()
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", AssignmentID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@FilterMode", (int)Mode, DbType.Int32));
        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand(SPA_ACTIVATE_ASSIGNMENT, paramsIn, out paramsOut);
    }

    protected void SaveAssignmentToFinalTables()
    {
        int TotalMerchant = uxFilters.MerchantCount;
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        paramsIn.Add(new FilterParameter("@StgAssignmentID", AssignmentID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@TotalMerchant", TotalMerchant, DbType.Int32));

        LoggerManager.Info(string.Format("uxAssignmentAuditLog: {0}", uxAssignmentTrackingJson.Value));

        var auditEntities = uxFilters.AppendPauseMerchantAlertAuditEntities(uxAssignmentTrackingJson.Value);

        LoggerManager.Info(string.Format("uxAssignmentAuditLog and pauseMerchantAlertsAuditLog: {0}", auditEntities));

        paramsIn.Add(new FilterParameter("@AuditLog", auditEntities, DbType.String));
        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand(SPA_SAVE_ASSIGNMENT, paramsIn, out paramsOut);
    }
    protected void OnAssignmentTypeChangeEventHandler(object sender, EventArgs e)
    {
        var cboAssignmentType = (AS.Controls.Global.RadComboBox)sender;       
        var assignmentType = cboAssignmentType.SelectedValue;
        SetVisibleFilterAndParamControl(assignmentType);
        if (!IsCreateNewAssignment)
            this.AjaxAddResponseScript(string.Format("onChangeAssignmentType({0});", cboAssignmentType.SelectedValue));

        ProcessForSubsite(assignmentType);

        //clear assignment filter when assignment is subsite
        Clear_Assignment(assignmentType);
    }

    protected void uxCustomView_PreRender(object sender, EventArgs e)
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            var displayColumns = uxCustomView.DataSource.AsEnumerable().
                Where(x => x.Field<int>("IsDefault") == 1).CopyToDataTable();

            uxCustomViewReadOnly.DataSource = displayColumns;
            uxCustomViewReadOnly.DataBind();
            uxCustomView.Visible = false;
            uxCustomViewModeReadOnly.Visible = true;
        }

    }

    private void SetVisibleFilterAndParamControl(string assignmentType)
    {
        bool isDetection = assignmentType.Equals(((int)WebSiteEnums.AssignmentType.DetectionQueue).ToString());
        bool isSubsite = assignmentType.Equals(((int)WebSiteEnums.AssignmentType.Subsite).ToString());
        var visible = isDetection || isSubsite;

        pnlFilters.Visible = visible;
        pnlParams.Visible = visible;
        //46430 show hide link
        plAuditMaster.Visible = !visible
                                && (this.IsUserWithPermission("AssignmentAuditReport") || this.IsUserWithPermission("MSAssignmentAuditReport"))
                                && !string.IsNullOrEmpty(ltAutitReportDetail.Text);
    }
    /// <summary>
    /// 46430 VW Add Manage Assignment Modal to User Audit Report
    /// </summary>
    /// <param name="dt"></param>
    protected void GetAssInfo(DataTable dt)
    {
        uxParam.BinAssignmentAudit(dt);
        GeneralFuncsLib.BinAssignmentAuditLink(dt, this, this.GetLocalResourceObject("LasModifyAuditTemplate").ToString(),
            plAuditMaster, ltAutitReportDetail, hdLinkAuditMaster);

        if (Duplicate)
        {
            uxAssInfo.SetInfoDuplicate();
        }

        var assignmentType = dt.Rows[0]["AssignmentType"].ToString();
        uxFilters.AssignmentType = assignmentType;
        uxParam.AssignmentType = assignmentType;
    }

    #endregion Methods

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void SaveParametersToStagging(int mode, string assignmentID, List<ParameterFE> parameters)
    {
        ParameterService.SaveStagging(mode, assignmentID, parameters, true);
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static Dictionary<string, string> GetTransactionCode(string mode, string assignmentID, string paramKey, string filterID)
    {
        return ParameterService.GetTransactionCode(mode, assignmentID, paramKey, filterID, true);
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GeACHReturnCode(string mode, string assignmentID, string paramKey, string filterID)
    {
        return UserControls_rm_MCF_Assignment_Parameters_New.GetACHReturnCode(mode, WebServices.SecurityServices.DecryptText(assignmentID), paramKey);
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void SaveAssignmentTracking()
    {

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void ResetTimeOut()
    {
        SharedProvider.UpdateLastActive();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void RemoveParametersToStagging(string assignmentID, string paramKey)
    {
        ParameterService.RemoveParametersToStagging(assignmentID, paramKey);
    }

    protected void uxLoadAssignmentFilters_Click(object sender, EventArgs e)
    {
        uxFilters.Rebind();
        this.AjaxAddResponseScript("LoadAssignmentParameters();");
    }

    protected void uxLoadAssignmentParameters_Click(object sender, EventArgs e)
    {
        if (pnlParams.Visible)
        {
            uxParam.Rebind();
            if (!IsCreateNewAssignment)
            {
                this.AjaxAddResponseScript("TrackingAssignment();");
            }
        }
    }

    protected void ProcessForSubsite(string assignmentType)
    {
        //rebind assignment filters
        bool isSubsite = assignmentType.Equals(((int)WebSiteEnums.AssignmentType.Subsite).ToString());
        uxFilters.AssignmentType = assignmentType;

        if (isSubsite)
        {
            uxFilters.ProcessForSubsite(false);

            var currentUserType = SessionManager.CurrentUserType;
            if (currentUserType == WebSiteEnums.UserHierarchyMode.CS || currentUserType == WebSiteEnums.UserHierarchyMode.AS)
                uxPhCustomView.Visible = false;

            uxParam.ProcessForSubsite(false, FeatureMode);
            uxParam.Rebind();
        }
        else
        {
            uxFilters.ProcessForSubsite(true);
            uxPhCustomView.Visible = true;
            uxParam.ProcessForSubsite(true, FeatureMode);
            uxParam.Rebind();
        }
    }
    private void Clear_Assignment(string assignmentType)
    {
        if (IsCreateNewAssignment && assignmentType.Equals(((int)WebSiteEnums.AssignmentType.Subsite).ToString()))
        {
            var currentUser = SessionManager.CurrentUser;
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter("@UserMode", GeneralFuncsLib.GetUserMode(), DbType.AnsiString));
            parameters.Add(new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString));
            parameters.Add(new FilterParameter("@ASClientID", currentUser.ASClient, DbType.Int32));
            parameters.Add(new FilterParameter("@SiteID", currentUser.SiteID, DbType.Int32));
            parameters.Add(new FilterParameter("@StgAssignmentID", AssignmentID, DbType.Int32));
            FilterParameterCollection paramsOut;
            WebServices.RiskServices.ExecuteNonQueryCommand("spa_MCF_Clear_Assignment", parameters, out paramsOut);
        }
    }
}
