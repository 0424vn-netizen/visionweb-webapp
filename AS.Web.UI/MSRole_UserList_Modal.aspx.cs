using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Common;
using AS.Controls.Exporter;
using AS.Controls.Pages;
using AS.Common.DBManager;
using Telerik.Web.UI;
using AS.Security.WS.Entities;


[PagePermission("DeleteMSRole")]
public partial class MSRole_UserList_Modal : ReportPage
{
    enum DataBindAction
    {
        BindGrid
    }

    private int _hierarchyID = -1;
    private string roleName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        roleName = GetLocalResourceObject("MSRole_UserList_Modal_aspx_cs_ThisRole").ToString();
        _hierarchyID = int.Parse(SecureQueryString["HierarchyID"]);
        Hierarchy hierarchy = WebServices.SecurityServices.GetHierarchyById(_hierarchyID);
        if (hierarchy != null)
        {
            roleName = hierarchy.HierarchyName;
        }
        
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExportTop");
        //this.ExporterIDs.Add("uxExportBottom");
        base.PageInitialize();
        IsBindDataOnLoad = true;
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindGrid, sender);
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = GetLocalResourceObject("MSRole_UserList_Modal_aspx_cs_ListUsersAssignedTo").ToString() + " " + roleName;
        exportConfig.FileName = GeneralFuncsLib.GetLegalFileName(GetLocalResourceObject("MSRole_UserList_Modal_aspx_cs_ListUsersAssignedTo").ToString() + " " + roleName);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        string spaName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGrid:
                {
                    spaName = "spa_SEC_GetUserListByHierarchyID";
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@FilterType", "USERROLE", DbType.AnsiString));
                    parameters.Add(new FilterParameter("@RoleId", _hierarchyID, DbType.Int32));
                    parameters.AddLanguageID();
                    uxReportGrid.DataSource = WebServices.SecurityServices.GetReports(spaName, parameters);
                }
                break;
        }
    }
}
