using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

[PagePermission("RskGroup,MSRskGroup")]
public partial class rm_MCF_Group_ActiveModal : ReportPage
{
    enum DataBindAction
    {
        BindGrid
    }
    private int _GroupID = -1;

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterBottom");
        base.PageInitialize();
        IsBindDataOnLoad = true;
        _GroupID = int.Parse(SecureQueryString["GroupID"]);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (IsIntruderDetected) return;

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindGrid, sender);
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.GetLegalFileName("ManageUsers");
        exportConfig.ReportHeader = GetLocalResourceObject("rm_Group_ActiveModal_aspx_cs_ActiveAssignments").ToString();
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        string spaName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGrid:
                {
                    spaName = "spa_RM_MCF_Get_Assignment";
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@UserIDFilter", string.Empty, DbType.String));
                    parameters.Add(new FilterParameter("@GroupID", _GroupID, DbType.Int32));
                    parameters.Add(new FilterParameter("@AssignmentID", 0, DbType.Int32));
                    parameters.Add(new FilterParameter("@GroupBy", "Assignment", DbType.String));
                    parameters.Add(new FilterParameter("@Mode", 0, DbType.Int32));
                    uxReportGrid.DataSource = WebServices.RiskServices.GetReports(spaName, parameters);
                }
                break;
        }
    }
}
