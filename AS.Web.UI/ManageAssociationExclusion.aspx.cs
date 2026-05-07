using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

[PagePermission("ManageAssociationExclusion")]
public partial class ManageAssociationExclusion : ReportPage
{

    public enum DataBindAction
    {
        BindSourceList
    }

    public enum PostBackAction
    {
        Add,
        Remove
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            uxAssoID.Text = string.Empty;
            OnDataBindControls(DataBindAction.BindSourceList, uxExclusionAssoGrid);
        }
    }

    protected override void PageInitialize()
    {
        //this.GridIDs.Add("uxExclusionAssoGrid");
        this.ExporterIDs.Add("uxExporter");
        base.PageInitialize();
    }

    protected void uxExclusionAssoGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindSourceList, sender);
    }


    protected void uxBntAddAssoID_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Add);
    }


    protected void uxRemove_Command(object sender, CommandEventArgs e)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection outParameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssociationID", e.CommandArgument, DbType.AnsiString));

        WebServices.CsReportServices.ExecuteNonQueryCommand("spa_SEC_DeleteAssociationExclusion", parameters, out outParameters);
        uxExclusionAssoGrid.Rebind();
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindSourceList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.AddLanguageID();
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_SEC_GetAssociationExclusionList", ReportServices.ConvertToFilterParamWSArray(parameters) });
                    break;
                }
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Add:
                {
                    string assoId = string.Format("{0}{1}", "ASSO", uxAssoID.Text);

                    FilterParameterCollection parameters = new FilterParameterCollection();
                    FilterParameterCollection outParameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@AssociationID", assoId, DbType.AnsiString));

                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_SEC_AddAssociationExclusion", parameters, out outParameters);
                    uxAssoID.Text = string.Empty;
                    uxExclusionAssoGrid.Rebind();

                    break;
                }
        }
    }

    [WebMethod]
    public static bool IsAssoIdExsit(string assoId)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@AssociationID", string.Format("{0}{1}", "ASSO", assoId), DbType.AnsiString));

        DataTable result = WebServices.RiskServices.GetReports("spa_SEC_CheckAssociationID", parameters);
        return result.Rows[0].Field<bool>(0);
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);

        exportConfig.FileName = GeneralFuncsLib.FormatFileName(GetLocalResourceObject("ExportFileName").ToString());
        exportConfig.ReportHeader = GetLocalResourceObject("ExportTitle").ToString();
        
        uxExclusionAssoGrid.Columns.FindByUniqueName("Functions").Visible = false;
    }
}