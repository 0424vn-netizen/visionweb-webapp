using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

[PagePermission("MyDocuments,MSMyDocuments")]
public partial class DataShare_MyDocuments : NonReportPage
{
    protected enum DataBindAction
    {
        BindMyDocumentGrid
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        base.OnDataBindControls(type, sender);
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMyDocumentGrid:
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                uxMyDocumentGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_cs_DataShare_GetListMyDocuments", ReportServices.ConvertToFilterParamWSArray(parameters) });
                break;
        }
    }

    protected void uxMyDocumentGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindMyDocumentGrid);
    }

    protected void uxbtnDownload_Click(object sender, EventArgs e)
    {
        int docId = Int32.Parse(uxDocId.Value);
        string fileName = uxFileName.Value;

        byte[] buffer = WebServices.DocServices.DownloadDoc(docId);
        this.TransferFileToClient(buffer, GeneralFuncsLib.FormatFileName(fileName));
    }
}