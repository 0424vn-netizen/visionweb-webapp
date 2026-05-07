using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageBank_Modal : NonReportPage
{
    const string BANKTYPE = "bank";
    protected void Page_Load(object sender, EventArgs e)
    {

        SecurePage _parentPage = (SecurePage)this.Page;

        var type = _parentPage.SecureQueryString["type"];
        Page.Title = type.Equals(BANKTYPE) ? GetLocalResourceObject("Label_Add_Bank_Title").ToString() : GetLocalResourceObject("Label_Add_Branch_Title").ToString(); ;

        PageType = SecurePageType.Modal;
        if (IsIntruderDetected)    return;
    }

    //protected override void PageInitialize()
    //{
    //    base.PageInitialize();
    //}
}