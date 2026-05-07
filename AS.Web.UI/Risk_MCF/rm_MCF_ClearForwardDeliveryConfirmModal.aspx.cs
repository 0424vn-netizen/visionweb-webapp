using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rm_MCF_ClearForwardDeliveryConfirmModal : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
    }
}