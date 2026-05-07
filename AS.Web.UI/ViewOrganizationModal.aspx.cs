using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class ViewOrganizationModal : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            DataTable dtOrgs = GeneralFuncsLib.GetListOrganizationsView(WebSiteEnums.OrganizationMode.BYSALESREP.ToString(), SecureQueryString["SaleRepCode"]);
            uxNoDataMessage.Visible = !dtOrgs.HasData();
            uxOrganizationList.DataSource = dtOrgs;
            uxOrganizationList.DataBind();
        }
    }
}