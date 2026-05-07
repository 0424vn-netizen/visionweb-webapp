//----------------------------------------------------------------------------
// <copyright file="gen_CreateNewUser.aspx.cs" company="Data Delivery Services, Inc.">
//     Copyright (c) Data Delivery Services, Inc. All rights reserved.
// </copyright>
// <author>Hung Ho</author>
// <summary>Manage Create User</summary>
//----------------------------------------------------------------------------

using System;
using System.Web.UI;
using AS.Controls.Pages;

[PagePermission("ASLandingPage")]
public partial class _mps_CreateNewASUser : NonReportPage
{
    protected override void PageInitialize()
    {
        base.PageInitialize();
        this.IsSecureCSRF = true;       
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;

        if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["u"]))
        {
            uxReportTitle.ReportTitle = "Edit Aperia User";
        }   

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

}
