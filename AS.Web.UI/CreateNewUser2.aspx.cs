//----------------------------------------------------------------------------
// <copyright file="gen_CreateNewUser.aspx.cs" company="Data Delivery Services, Inc.">
//     Copyright (c) Data Delivery Services, Inc. All rights reserved.
// </copyright>
// <author>Hung Ho</author>
// <summary>Manage Create User</summary>
//----------------------------------------------------------------------------

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
using AS.Common;

[PagePermission("ManUser,MSManUser,ASLandingPage")]
public partial class _mps_CreateNewUser2 : NonReportPage
{
    private const string PASSWORD_ADMIN_RESET_EXPIRE_DAYS = "PasswordAdminResetExpiredDays";
    protected string timeToExpired = string.Empty;
    protected string isFromCreateNewChainModal = "false";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        if (SessionManager.ResetPasswordUser != null)
        {
            uxUserName.Text = VeraCodeSolution.DoVeraCode(SessionManager.ResetPasswordUser.UserID);
            uxPassword.Text = VeraCodeSolution.DoVeraCode(SessionManager.ResetPasswordUser.UserPassword);

            if (!string.IsNullOrEmpty(((Validation)WebSiteSettings.PwdValidationRule.Get(PASSWORD_ADMIN_RESET_EXPIRE_DAYS)).Key))
            {
                int configValue = ((Validation)WebSiteSettings.PwdValidationRule.Get(PASSWORD_ADMIN_RESET_EXPIRE_DAYS)).Value;

                timeToExpired = (configValue * 24) + " " + GetLocalResourceObject("CreateNewUser2_aspx_Hour").ToString();
            }
            else
            {
                timeToExpired = "72 " + GetLocalResourceObject("CreateNewUser2_aspx_Hour").ToString();
            }
            if (IsSecureQueryString)
            {
                if (SecureQueryString["IsFromCreateNewChainModal"] != null)
                {
                    isFromCreateNewChainModal = SecureQueryString["IsFromCreateNewChainModal"].ToString();
                }
            }
        }
        else
        {
            //intruder
            base.IsIntruderDetected = true;
            base.ASPXTrackingLog.LogData4 += GetLocalResourceObject("CreateNewUser2_aspx_LogNull").ToString();

        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
}
