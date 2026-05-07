using AS.Controls.Pages;
using System;

[PagePermission("RskBrandFraudReports")]
public partial class rm_MCF_VisaAcquirerFraudReport : ReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        IsBindDataOnLoad = true;

    }
}