using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PCIMaintenanceModal : ReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ((MasterPageNormal)Page.Master).HideHeaderMenu = true;
        uxMessage.Text = GetLocalResourceObject("PCIMaintenanceModal_aspx_cs_MessageText").ToString();
    }
}