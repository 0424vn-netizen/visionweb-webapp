using System;
using System.Web.UI.WebControls;

public partial class UserControls_UxExportQueue : GlobalUserControl
{
    public UserControls_UxExportQueue()
    {
        ShowExcel = true;
        ShowCSV = true;
        ShowPDF = false;
    }

    public string GridID { get; set; }
    public string GridTitle
    {
        get { return ViewState["GridTitle"] != null ? ViewState["GridTitle"].ToString() : string.Empty; }
        set { ViewState["GridTitle"] = value; }
    }
    public string GridSubTitle
    {
        get { return ViewState["GridSubTitle"] != null ? ViewState["GridSubTitle"].ToString() : string.Empty; }
        set { ViewState["GridSubTitle"] = value; }
    }
    public bool IsOnTop { get; set; }

    public bool ShowExcel { get; set; }

    public bool ShowCSV { get; set; }

    public bool ShowPDF { get; set; }

    public string PageName { get; set; }

    public bool HasData
    {
        get { return ViewState["HasData"] == null || (bool)ViewState["HasData"]; }
        set { ViewState["HasData"] = value; }
    }

    public string GridHeader
    {
        get { return ViewState["GridHeader"] != null ? ViewState["GridHeader"].ToString() : string.Empty; }
        set { ViewState["GridHeader"] = value; }
    }

    public string FilterParams
    {
        get { return ViewState["FilterParams"] != null ? ViewState["FilterParams"].ToString() : string.Empty; }
        set { ViewState["FilterParams"] = value; }
    }

    protected override void OnPreRender(EventArgs e)
    {
        litGridTitle.Text = (GridTitle ?? string.Empty).Trim().TrimEnd('-');
        litGridSubTitle.Text = GridSubTitle ?? string.Empty;

        var hasTitle = !string.IsNullOrEmpty(GridTitle);
        h2GridTitle.Visible = hasTitle;
        phTitleRow.Visible = hasTitle;
        phTitleRowClose.Visible = hasTitle;

        h2GridTitle.Attributes["class"] = IsOnTop ? "grid-title on-top" : "grid-title";
        var exportCss = hasTitle ? "report-export" : "report-export-no-title";
        divExport.Attributes["class"] = IsOnTop ? exportCss + " dropdown on-top" : exportCss + " dropdown";
        if (!hasTitle)
        {
            divExportWrapper.Style["margin-top"] = "38px";
            divExportWrapper.Style["margin-bottom"] = "10px";
            divExport.Style["margin"] = "0";
        }

        if (!string.IsNullOrEmpty(GridID))
            h2GridTitle.Attributes["data-target"] = "#" + this.ClientID.Replace(this.ID, GridID);

        uxLiExcel.Visible = ShowExcel;
        uxLiCSV.Visible = ShowCSV;
        uxLiPDF.Visible = ShowPDF;

        hddFilterParams.Value = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(FilterParams));

        this.Page.PreRenderComplete += (s, args) => { divExportWrapper.Visible = HasData; };

        base.OnPreRender(e);
    }
}