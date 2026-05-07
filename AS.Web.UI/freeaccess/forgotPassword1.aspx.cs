//----------------------------------------------------------------------------
// <copyright file="gen_forgotPassword1.aspx.cs" company="Data Delivery Services, Inc.">
//     Copyright (c) Data Delivery Services, Inc. All rights reserved.
// </copyright>
// <author>Hung Ho</author>
// <summary>Handle forgot password function</summary>
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
using AS.Security.WS.Entities;
using AS.Security.Web.SecurityServices;


public partial class _mps_forgotpassword1 : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        btnContinue.Attributes["onclick"] = "return ValidateInput()";
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
   
    private void ShowMessagePopup(string messageKey)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "OpenMessageWnd",
                string.Format("OpenForgotWndMsg('{0}');",
                "forgotPasswordMsg.aspx?" + BuildSecureQueryString("c=" + messageKey)), true);
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        if (this.IsIntruderDetected) return;

        if (GeneralFuncsLib.IsSSOUser(SessionManager.CurrentClient, uxUsername.Text))
        {
            ShowMessagePopup("UNotF");
            return;
        }

        User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, uxUsername.Text);

        //Case1 : Invalid User
        if (user == null)
        {            
            ShowMessagePopup("UNotF");            
            return;
        }

        //User is deactivated
        if (user.ActvStat.Equals("0"))
        {
            ShowMessagePopup("UNotF");
            return;
        }
        
        //USER LockedOut Or User inactive
        if (WebServices.SecurityServices.IsForgotLockedOut(SessionManager.CurrentClient, uxUsername.Text))
        {
            ShowMessagePopup("FLocked");          
            return;
        }


        //Case 2 : Valid User and Valid Email
        //Show question 2
        if (user.Email.ToLower() == uxEmail.Text.ToLower())
        {
            SessionManager.ForgetPasswordUser = user;
            Response.Redirect("forgotPassword2.aspx");
            return;
        }

        //Case 3 : Valid User and Invalid Email 
        WebServices.SecurityServices.UpdateForgotPassword(SessionManager.CurrentClient, user.UserID, false);
        ShowMessagePopup("UNotF");
    }
}
