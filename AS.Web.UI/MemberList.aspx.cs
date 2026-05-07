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
using AS.Controls.Pages;
using AS.Common.DBManager;
using Telerik.Web.UI;
using AS.Common;
using AS.Controls.Global;

public partial class MemberList : ReportPage
{
    enum DataBindAction
    {
        BindDataGroups
    }

    private string _GridTitle
    {
        get
        {
            return string.Format(GetLocalResourceObject("uxExporterResource1.GridTitle").ToString());
        }
    }
    private string _ExportFileName
    {
        get
        {
            return string.Format(GetLocalResourceObject("uxExporterFileNameResource.Text").ToString(), SecureQueryString["OwnershipGroupName"]);
        }
    }
    private string _GridHeader
    {
        get
        {
            return string.Format(GetLocalResourceObject("uxExporterResource1.GridHeader").ToString());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindDataGroups, sender);
        }
    }


    protected override void OnDataBindControls(Enum type, object sender)
    {

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDataGroups:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.RecId, DbType.Guid));
                    parameters.Add(new FilterParameter("@ASClientID", SessionManager.CurrentUser.ASClient, DbType.Int32));
                    parameters.Add(new FilterParameter("@OwnershipGroupID", SecureQueryString["OwnershipGroupID"], DbType.Int32));
                    parameters.Add(new FilterParameter("@CheckPerMod", true, DbType.Boolean));
                    uxMemberGrid.DataSource = WebServices.CsReportServices.GetReports("spa_CM_GetUsersList", parameters);

                    uxExportTop.GridTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
        }
    }
    protected override void OnPostBackActions(Enum type, object sender)
    {
        uxMemberGrid.Rebind();
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(_ExportFileName);
        exportConfig.ReportHeader = VeraCodeSolution.ValidateResponseData(_GridTitle);
    }

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxMemberGrid");
        this.ExporterIDs.Add("uxExportTop");
        base.PageInitialize();
    }
    protected void uxMemberGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        if (sender == uxMemberGrid)
        {
            OnDataBindControls(DataBindAction.BindDataGroups, sender);
        }
    }
}
