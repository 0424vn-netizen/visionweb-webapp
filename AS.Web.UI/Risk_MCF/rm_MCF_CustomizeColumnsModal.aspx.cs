using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

[PagePermission("RskRP,MSRskRP")]
public partial class rm_MCF_CustomizeColumnsModal : NonReportPage
{

    private string _customViewID
    {
        get
        {
            if (SecureQueryString["CustomViewID"] != null) return SecureQueryString["CustomViewID"].ToString();
            return string.Empty;
        }
    }
        
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        Page.Title = GetLocalResourceObject("PageResource1.Title").ToString();
        if (!IsPostBack)
        {
            uxCustomizeColumnsModal.CustomViewID = _customViewID;
            uxCustomizeColumnsModal.IsCreate = false;
            uxCustomizeColumnsModal.GetData();
        }
        
    }
}
