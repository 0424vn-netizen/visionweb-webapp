using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rm_MCF_CreateManageCustomViewModal : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            uxCustomColumnsModal.GetData();
            uxCustomColumnsModal.CustomViewID = "1";
            uxCustomColumnsModal.IsCreate = true;
        }
    }
}