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
using AS.Controls.Pages;
using System.Collections.Generic;
using System.Text;
using AS.Web.SharedSession;

[PagePermission("RskAdhoc,MSRskAdhoc")]
public partial class rm_MCF_AdhocCreate : NonReportPage
{
    #region Enums
    enum DataBindAction { }
    enum PostBackAction
    {
        SaveAdhoc
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (IsIntruderDetected) return;
        uxCancel.PrimaryID = uxFilters.PrimaryID = uxParam.PrimaryID = SecureQueryString["AssignmentID"];
        int reviewMode = 0;
        if (SecureQueryString["ReviewMode"] != null)
            reviewMode = int.Parse(SecureQueryString["ReviewMode"]);
        
        uxCancel.Mode = WebSiteEnums.ParamFilterMode.Adhoc;
        uxCancel.ReviewMode = reviewMode;

        int featureMode = 0;
        if (SecureQueryString["FeatureMode"] != null)
        {
            featureMode = int.Parse(SecureQueryString["FeatureMode"]);
        }
        uxFilters.FeatureMode = uxParam.FeatureMode = (WebSiteEnums.FeatureMode)featureMode;
        uxSave.Visible = (WebSiteEnums.FeatureMode)featureMode == WebSiteEnums.FeatureMode.Edit;
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
            case PostBackAction.SaveAdhoc:
                int error = uxFilters.Save();
                if (error != 0)
                    return;

                uxParam.Save();
                ActivateAssignment();
                AjaxAddResponseScript("parent.CloseAndRebind();");
                break;
        }
    }

    protected void uxSave_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SaveAdhoc, sender);
    }
    protected void ActivateAssignment()
    {
        string primaryID = SecureQueryString["AssignmentID"];
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        paramsIn.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@FilterMode", (int)WebSiteEnums.ParamFilterMode.Adhoc, DbType.Int32));
        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_ActivateAssignment", paramsIn, out paramsOut);
    }

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
    public static void ResetTimeOut()
    {
        SharedProvider.UpdateLastActive();
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GeACHReturnCode(string mode, string assignmentID, string paramKey, string filterID)
    {
        return UserControls_rm_MCF_Assignment_Parameters_Adhoc.GetACHReturnCode(mode, WebServices.SecurityServices.DecryptText(assignmentID), paramKey);
    }
}
