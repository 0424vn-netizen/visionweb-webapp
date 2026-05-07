using AS.Controls.Exporter;
using AS.Controls.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Controls.Pages;

[PagePermission("RskMerchantWorked,MSRskMerchantWorked")]
public partial class rm_MCF_MerchantWorkedReportPopup : ReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
    }

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
    }
}