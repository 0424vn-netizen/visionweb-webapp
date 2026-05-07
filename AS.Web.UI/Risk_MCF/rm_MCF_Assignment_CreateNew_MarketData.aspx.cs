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
using AS.Controls.Pages;
using AS.Common.DBManager;
using Resources;
[PagePermission("RskManAss,MSRskManAss")]
public partial class rm_MCF_Assignment_CreateNew_MarketData : NonReportPage
{
    #region Enums
    enum DataBindAction { }
    enum PostBackAction
    {
        SaveAssignment,
        CheckNameAssignment
    }
    #endregion

    protected bool IsDetectionQueue
    {
        get
        {
            if (SecureQueryString["DetectionQueueMode"] != null)
            {
                int mode = int.Parse(SecureQueryString["DetectionQueueMode"]);
                return (mode == null || mode == 0) ? false : true;
            }
            else
            {
                return false;
            }
        }
    }

    protected WebSiteEnums.FeatureMode FeatureMode
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

    protected int ReviewMode
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

    protected WebSiteEnums.ParamFilterMode Mode
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        uxCancel.PrimaryID = uxAssInfo.PrimaryID = uxFilters.PrimaryID = SecureQueryString["AssignmentID"];
        uxCancel.Mode = Mode;
        uxCancel.ReviewMode = ReviewMode;
        uxAssInfo.FeatureMode = uxFilters.FeatureMode = FeatureMode;
        uxSave.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.CheckNameAssignment:
                int IsDuplicate = 0;
                if (uxAssInfo.CheckDuplicatedAssignmentName() == true)
                    IsDuplicate = 1;
                else
                    IsDuplicate = 0;
                AjaxAddResponseScript("checkDuplicateAssignmentName(" + IsDuplicate + ");");
                break;
            case PostBackAction.SaveAssignment:
                int error = uxAssInfo.Save();
                if (error == -1)
                    return;
                error = uxFilters.Save();
                if (error != 0)
                    return;
                ActivateAssignment();
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
        string primaryID = SecureQueryString["AssignmentID"];
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@FilterMode", (int)Mode, DbType.Int32));
        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_ActivateAssignment", paramsIn, out paramsOut);
    }

    protected void SaveAssignmentToFinalTables()
    {
        string primaryID = SecureQueryString["AssignmentID"];

        int TotalMerchant = uxFilters.MerchantCount;
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        paramsIn.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@TotalMerchant", TotalMerchant, DbType.Int32));
        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveAssignmentToFinalTables", paramsIn, out paramsOut);

    }
}
