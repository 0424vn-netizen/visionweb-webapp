using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;

public partial class UserControls_rm_MCF_RiskReport_UxExport : AS.Controls.Global.Exporter
{
    protected override void OnPreRender(EventArgs e)
    {
        LinkButton4.Visible = ShowPDF;
        base.OnPreRender(e);
    }
    protected override void OnNeedExportConfig(AS.Controls.Exporter.ExportConfig exportConfig)
    {
        //Remove temp column when export
        GridColumn temColumn = this.Grid.Columns.FindByUniqueNameSafe(WebSiteConstants.GRID_COLUMN_TEMP);
        if (temColumn != null)
            temColumn.Visible = false;

        base.OnNeedExportConfig(exportConfig);
    }
}