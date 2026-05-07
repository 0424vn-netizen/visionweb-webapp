using AS.Common.DBManager;
using System;
using System.Data;
using Telerik.Web.UI;
using AS.Controls.Pages;

[PagePermission("RskBrandFraudReports")]
public partial class rm_MCF_MasterCardAcquirerFraudReport : ReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        IsBindDataOnLoad = true;
    }

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;
        base.PageInitialize();
    }
}