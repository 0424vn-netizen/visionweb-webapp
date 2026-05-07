using System;
using System.Web.UI;

//[PagePermission("")]
public partial class rm_MCF_CreateNewDispositionModal : ReportPage
{
    public string DispositionID
    {
        get
        {
            if (IsSecureQueryString && SecureQueryString["DispositionID"] != null)
                return SecureQueryString["DispositionID"].ToString();
            return string.Empty;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!Page.IsPostBack)
        {
            this.Page.Title = (!string.IsNullOrEmpty(DispositionID)) ? GetLocalResourceObject("PageResource2.Title").ToString() :
                 GetLocalResourceObject("PageResource1.Title").ToString();
        }
    }
}