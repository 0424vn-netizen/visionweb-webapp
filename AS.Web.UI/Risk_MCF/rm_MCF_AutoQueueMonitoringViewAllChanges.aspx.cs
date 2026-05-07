using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AS.Common.DBManager;
using AS.Controls.Pages;

[PagePermission("RskAutoQueue,MSRskAutoQueue")]
public partial class rm_MCF_AutoQueueMonitoringViewAllChanges : NonReportPage
{
    protected override void PageInitialize()
    {
        PageType = SecurePageType.Modal;
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        if (!IsPostBack)
            BindChangeLog();
    }

    private void BindChangeLog()
    {
        if (SecureQueryString["AutoQueueID"] != null)
        {
            long autoQueueId = SecureQueryString["AutoQueueID"].ToLong();
            uxChangeLog.DataSource = Rm_AutoQueueBusiness.GetDataChangeLog(autoQueueId, true, true);
            uxChangeLog.DataBind();
        }
    }
}