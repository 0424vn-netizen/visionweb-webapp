using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rm_MCF_WorkWarning : NonReportPage
{
    public string Message
    {
        get
        {
            if (SecureQueryString["Message"] != null)
            {
                return SecureQueryString["Message"];
            }
            else
            {
                return string.Empty;
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
       
        if (!IsPostBack)
        {          
            if (IsSecureQueryString)
            {
                uxLtWarning.Text = Message;
            } 
        }
    }

    protected void uxRefresh_Click(object sender, EventArgs e)
    {
       
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"parent.refreshDataEvent();"));
    }
}