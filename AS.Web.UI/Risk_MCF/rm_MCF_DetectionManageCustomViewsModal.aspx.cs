using AS.Common.DBManager;
using AS.Core.Common.VeraCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class rm_MCF_DetectionManageCustomViewsModal : NonReportPage
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        Page.Title = GetLocalResourceObject("PageResource1.Title").ToString();
        if (!IsPostBack)
        {
            uxCustomizeColumnsModal.GetData();
        }
    }
}