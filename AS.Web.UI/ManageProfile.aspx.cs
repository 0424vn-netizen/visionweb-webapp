using AS.Controls.Pages;
using System.Web.Services;

namespace As.VisionWeb.Web
{
    [PagePermission("UserProfile,MSUserProfile")]
    public partial class ManageProfile : NonReportPage
    {
        protected override void PageInitialize()
        {
            base.PageInitialize();
            this.IsSecureCSRF = true;
        }

        [WebMethod]
        public static void UpdateHeaderMenu()
        {
            //Reload header menu 
            SessionManager.GetHeaderMenu();
        }

    }
}

