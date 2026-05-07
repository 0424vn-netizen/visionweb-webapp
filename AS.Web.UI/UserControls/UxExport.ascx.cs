using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;

public partial class UserControls_UxExport : AS.Controls.Global.Exporter
{   
    
    public bool IsOnTop { get; set; }
    
    protected override void OnPreRender(EventArgs e)
    {
        this.GridTitle = this.GridTitle.Trim().TrimEnd('-');
        litGridTitle.Text = this.GridTitle.Trim().TrimEnd('-');
        h2GridTitle.Visible = divExport.Visible = this.Visible;
       

        if (IsOnTop)
        {
            h2GridTitle.Attributes.Add("class", "grid-title on-top");
            divExport.Attributes.Add("class", "report-export dropdown pull-right on-top");
        }
        else
        {
            h2GridTitle.Attributes.Add("class", "grid-title");
            divExport.Attributes.Add("class", "report-export dropdown pull-right");
        }

        h2GridTitle.Attributes.Add("data-target", "#" + this.ClientID.Replace(this.ID, GridID));
        base.OnPreRender(e);
    }
    protected override void OnNeedExportConfig(AS.Controls.Exporter.ExportConfig exportConfig)
    {
        //Remove temp column when export
        GridColumn temColumn = this.Grid.Columns.FindByUniqueNameSafe(WebSiteConstants.GRID_COLUMN_TEMP);
        if (temColumn != null)
            temColumn.Visible = false;

        base.OnNeedExportConfig(exportConfig);
        if (Request.Browser.Browser.ToUpper() == WebSiteConstants.BROWSER_INTERNETEXPLORER)
        {
            exportConfig.FileName = Server.UrlPathEncode(exportConfig.FileName);
        }
    }
}