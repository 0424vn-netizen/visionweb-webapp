//----------------------------------------------------------------------------
// <copyright file="gen_forgotPassword1.aspx.cs" company="Data Delivery Services, Inc.">
//     Copyright (c) Data Delivery Services, Inc. All rights reserved.
// </copyright>
// <author>Hung Ho</author>
// <summary>Handle message box</summary>
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


public partial class _mps_fdc_forgotPasswordMsg : NonReportPage
{
    protected string returnUrl = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        switch (SecureQueryString["c"])
        {

            case "01":
                uxMessage.Text = "The Email address you entered does not match the Email address on record for this User." +
                    "You may try again or contact Customer Service for assistance.";
                returnUrl = "fdc_cs_forgotPassword1.aspx";
                uxPhdTryAgain.Visible = true;
                uxClose.Visible = false;
                uxTryAgain.OnClientClick = "parent.window.dialogs[3].close();";  
                break;

            case "ULocked":
            case "UNotF":
                uxMessage.Text = "You have entered an invalid username or email address." +
                    " You may try again or contact Customer Service for assistance.";                   
                break;

            case "SuMes":
                uxMessage.Text = "A new password has been generated and emailed to the address on file.";
                
                break;

            case "AnNM":
                uxMessage.Text = "You have entered an invalid answer."  +
                    " You may try again or contact Customer Service for assistance.";

                returnUrl = "forgotPassword2.aspx";
                uxTryAgain.OnClientClick = "parent.window.dialogs[3].close();";
                uxPhdTryAgain.Visible = true;
                uxClose.Visible = false;
                break;
            case "OuOfAn":
                uxMessage.Text = "Please contact Customer Service for assistance.";
                break;
            case "FLocked":
                uxMessage.Text = "There have been too many attempts to reset the password. Please contact customer service.";
                break;
            default:
                uxMessage.Text = "No available message.";
                break;

        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
}
