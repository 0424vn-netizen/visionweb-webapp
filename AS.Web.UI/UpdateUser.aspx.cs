using System;
using AS.Controls.Pages;

namespace As.VisionWeb.Web
{
    [PagePermission("ManUser,MSManUser")]
    public partial class UpdateUser : NonReportPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsIntruderDetected) return;

            if (IsSecureQueryString && SecureQueryString["IsPopup"] != null)
                PageType = SecurePageType.None;
            else
                PageType = SecurePageType.Modal;

        }
        protected override void PageInitialize()
        {
            base.PageInitialize();
            this.IsSecureCSRF = true;
        }
    }
}

