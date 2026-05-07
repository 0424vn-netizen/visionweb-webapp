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
using AS.Common.DBManager;
using AS.Common;
using Telerik.Web.UI;

public partial class UserControls_rm_MCF_PageHeader : GlobalUserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (SessionManager.CurrentUser.UserSecRole == "FDUSER")
            {
                uxSiteList.Visible = true;
                uxSubmit.Visible = true;
                uxSiteList.DataTextField = "ClientName";
                uxSiteList.DataValueField = "SiteID";
                uxSiteList.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAllRiskSites", new FilterParameterCollection());
                uxSiteList.DataBind();
                RadComboBoxItem EmptyItem = new RadComboBoxItem("", "1");
                EmptyItem.Height = Unit.Pixel(13);
                uxSiteList.Items.Insert(0, EmptyItem);
                uxSiteList.SelectedValue = SessionManager.CurrentUser.ASClient.ToString();
            }
            else
                this.Visible = false;
        }
        this.uxSiteList.Text = VeraCodeSolution.ValidateResponseData(this.hf2.Value);
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        int SiteID = int.Parse(uxSiteList.SelectedValue);
        // RiskSessionManager.SiteID = SiteID;
        //  RiskSessionManager.SiteName = uxSiteList.SelectedItem.Text;
        Response.Redirect(Request.ServerVariables["URL"]);
    }
}
