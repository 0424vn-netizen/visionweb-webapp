using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using AS.Controls.Pages;
using AS.Controls.Exporter;
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;

[PagePermission("RskGroup,MSRskGroup")]
public partial class rm_MCF_Group_MemberModal : ReportPage
{
    enum DataBindAction
    {
        BindGrid
    }

    private int _GroupID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        _GroupID = int.Parse(SecureQueryString["GroupID"]);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExportTop");
        this.ExporterIDs.Add("uxExportBottom");
        base.PageInitialize();
        IsBindDataOnLoad = true;
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindGrid, sender);
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = GetLocalResourceObject("_cs_MemberList").ToString();
        exportConfig.FileName = GeneralFuncsLib.GetLegalFileName("Member List");
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        string spaName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGrid:
                {
                    spaName = "spa_RM_MCF_GetRiskUsersByGroup";
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@GroupID", _GroupID, DbType.Int32));
                    parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
                    uxReportGrid.DataSource = WebServices.RiskServices.GetReports(spaName, parameters);
                }
                break;           
        }
    }
}
