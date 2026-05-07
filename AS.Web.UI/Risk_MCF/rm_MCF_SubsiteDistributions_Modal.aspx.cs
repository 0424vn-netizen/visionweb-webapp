using System;
using System.Data;
using AS.Common.DBManager;
using AS.Controls.Pages;

namespace As.VisionWeb.Web
{
    [PagePermission("RskManAss,MSRskManAss")]
    public partial class SubsiteDistributionsModal : NonReportPage
    {
        public string ClientIDbtn { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            PageType = SecurePageType.Modal;
            uxSubsiteDistributions.PrimaryID = SecureQueryString["AssignmentID"];
            uxSubsiteDistributions.Mode = WebSiteEnums.ParamFilterMode.Assignment;
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            uxSubsiteDistributions.Save();
            ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", "parent.ClosePopupModal(1);", true);
        }       
    }
}

