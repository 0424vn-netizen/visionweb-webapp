using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using AS.Core.Common;
using AS.Common.DBManager;

public partial class UserControls_rm_MCF_UxDetectionQueueExport : AS.Controls.Global.Exporter
{

    #region ---- Variable & Enum -----
    public bool IsOnTop { get; set; }
    public bool VisibleExport { get; set; }
    #endregion ---- Variable & Enum -----

    #region ---- Private Methods ----

    #endregion ---- Private Methods ----

    #region ---- Public Methods ----

    public void ShowHideCustomViewLink(bool isShow)
    {
        //uxCustomizeColumnLink.Visible = isShow;
    }

    #endregion ---- Public Methods ----

    #region ---- Events Handle ----

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        this.GridTitle = this.GridTitle.Trim().TrimEnd('-');
        litGridTitle.Text = this.GridTitle.Trim().TrimEnd('-');
        h2GridTitle.Visible = divExport.Visible = this.Visible;
        divExport.Visible = VisibleExport;

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
        h2litGridSubTitle.Attributes.Add("data-target", "#" + this.ClientID.Replace(this.ID, GridID));
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
    #endregion ---- Events Handle ----

}