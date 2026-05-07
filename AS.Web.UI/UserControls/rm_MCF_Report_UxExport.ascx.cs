using AS.Common.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

public partial class UserControls_rm_MCF_Report_UxExport : AS.Controls.Global.Exporter
{
    protected override void OnPreRender(EventArgs e)
    {
        try
        {
            imgPDF.Visible = ShowPDF;
            base.OnPreRender(e);
        }
        catch (ViewStateException)
        {
            LoggerManager.Error(string.Format("UserControls_rm_MCF_Report_UxExport - ViewStateException - Request={0}; SessionID={1}", Context.Request.Url, Session.SessionID));
            throw;
        }
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