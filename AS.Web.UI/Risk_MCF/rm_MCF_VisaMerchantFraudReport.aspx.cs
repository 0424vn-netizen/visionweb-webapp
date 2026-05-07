using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;
using Telerik.Web.UI;

[PagePermission("RskBrandFraudReports")]
public partial class rm_MCF_VisaMerchantFraudReport : ReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) 
            return;

        IsBindDataOnLoad = true;

    }
}