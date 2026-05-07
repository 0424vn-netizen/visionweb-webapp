using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Security.WS.Entities;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System.Data;
using AS.Common;

[PagePermission("JSAccess")]
public partial class page_SiteJump : ReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;

        if (!IsPostBack)
        {
            if (SavedReportFilterValue != null && GeneralFuncsLib.IsMerchantMode(SavedReportFilterValue.HierarchyMode))
                uxUserID.Text = VeraCodeSolution.DoVeraCode(SavedReportFilterValue.Value);
        }
    }

    protected void uxGo_Click(object sender, EventArgs e)
    {
        this.ASPXTrackingLog.LogData1 = "gen_log_JumpSite.aspx";
        this.ASPXTrackingLog.LogData3 = "";
        if (uxUserID.Text.Trim().ToLower() == SessionManager.CurrentUser.UserID.ToLower())
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "showMessage", "alert('" + GetLocalResourceObject("SiteJump_aspx_cs_MsgInvalidUserAccount").ToString() + "')", true);
            return;
        }

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@UserAccess", uxUserID.Text.Trim(), DbType.String));
        DataTable userInfo = WebServices.SecurityServices.GetReports("spa_SEC_GetUserIDByUserSiteAccess", parameters);
        if (userInfo == null || userInfo.Rows.Count == 0)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "showMessage", "alert('" + GetLocalResourceObject("SiteJump_aspx_cs_MsgInvalidUserAccount").ToString() + "')", true);
            return;
        }
        string userId = userInfo.Rows[0]["UserID"].ToString();

        parameters.Clear();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@UserHierarchyCode", SessionManager.CurrentUserRoles[0].HierarchyCode, DbType.String));
        parameters.Add(new FilterParameter("@UserSiteJump", userId, DbType.String));
        parameters.Add(new FilterParameter("@UserEntityType", SessionManager.CurrentUser.EntityType, DbType.Int32));
        parameters.Add(new FilterParameter("@SystemId", SessionManager.CurrentSystem, DbType.Int32));

        DataTable dt = WebServices.SecurityServices.GetReports("spa_SEC_CheckUserSiteJump", parameters);
        if (dt != null && dt.Rows.Count > 0)
        {
            switch (Int32.Parse(dt.Rows[0][0].ToString()))
            {
                case 0: // Success
                    break;
                case 1: // Invalid user                                       
                case 2:  // Entity not belongs to the current user
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "showMessage", "alert('" + GetLocalResourceObject("SiteJump_aspx_cs_MsgInvalidUserAccount").ToString() + "')", true);
                    return;
                case 3:  // Merchant is opted out
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "showMessage", "alert('" + GetLocalResourceObject("SiteJump_aspx_cs_MerchantIsOptedOut").ToString() + "')", true);
                    return;
                case 4:  // User is opted out
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "showMessage", "alert('" + GetLocalResourceObject("SiteJump_aspx_cs_UserIsOptedOut").ToString() + "')", true);
                    return;
            }
        }
        User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, userId);

        string excludeHierachies = GeneralFuncsLib.GetDataOfExtendedSetting("DisableSiteJumpByEntityTypeIDs");
        if (!string.IsNullOrEmpty(excludeHierachies))
        {
            excludeHierachies = string.Format(",{0},", excludeHierachies);
            if (excludeHierachies.Contains(string.Format(",{0},", user.EntityType)))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "showMessage", "alert('" + GetLocalResourceObject("SiteJump_aspx_cs_MsgInvalidUserAccount").ToString() + "')", true);
                return;
            }
        }

        if ((SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS && user.SiteID == 0) || (SessionManager.CurrentClient == WebSiteConstants.TOTAL_CLIENT && GeneralFuncsLib.IsSSOUser(SessionManager.CurrentClient, user.UserID) == true))
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "showMessage", "alert('" + GetLocalResourceObject("SiteJump_aspx_cs_MsgInvalidUserAccount").ToString() + "')", true);
            return;
        }
        string key = WebServices.SecurityServices.CreateJumpSiteTicket(user.RecId, Request.UserHostAddress, 2, SessionManager.CurrentUser.RecId);
        string url = WebSiteSettings.MsGate;
        url = string.Format(url + "?u={0}&k={1}&j={2}&c={3}&f={4}&lan={5}", HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText(user.UserID)),
            HttpUtility.UrlEncode(key),
            HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText(SessionManager.CurrentUser.RecId.ToString())),
            HttpUtility.UrlEncode(AS.Common.DataProtection.Cryptophy.EncryptText(user.ASClient.ToString())),
            HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText("CS")),
            SessionManager.CurrentLanguage
            );
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format("return parent.HideSiteJumpModal();"));
        ClientScript.RegisterStartupScript(GetType(), "startup", string.Format("window.open('{0}');", url), true);
    }
}
