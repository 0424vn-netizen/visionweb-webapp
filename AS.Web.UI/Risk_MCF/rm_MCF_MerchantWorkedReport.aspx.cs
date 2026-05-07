using AS.Common;
using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Web.UI.Controls;
using AS.Controls.UserControls;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Controls.Pages;
using AS.Controls.Exporter;

[PagePermission("RskMerchantWorked,MSRskMerchantWorked")]
public partial class rm_MCF_MerchantWorkedReport : ReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
    }
}