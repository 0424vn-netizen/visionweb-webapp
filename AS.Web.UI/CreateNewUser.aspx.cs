using System;
using AS.Controls.Pages;

namespace As.VisionWeb.Web
{
    [PagePermission("ManUser,MSManUser,MSUser")]
    public partial class CreateNewUser : NonReportPage
    {
        protected override void PageInitialize()
        {
            base.PageInitialize();
            this.IsSecureCSRF = true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsIntruderDetected) return;
            PageType = SecurePageType.Modal;
        }
    }
}

