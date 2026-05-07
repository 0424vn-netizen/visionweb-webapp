using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class rm_MCF_MgmtReport_Exports : ReportPage
{
    #region Constants

    private const string SPA_GET_EXPORT_QUEUE = "spa_ExportingService_GetProcessList";
    private const string SPA_DELETE_EXPORT_QUEUE = "spa_ExportingService_DeleteProcessQueue";

    #endregion Constants

    #region Enums

    enum DataBindAction
    {
        BindExportGrid,
    }

    #endregion Enums

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        bool isPopup = IsSecureQueryString && SecureQueryString["isPopup"].ToBoolean();
        if (isPopup)
        {
            ((MasterPageNormal)Page.Master).HideHeaderMenu = true;
        }
    }

    #endregion Page Events

    #region Grid Events

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindExportGrid:
                var parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams();
                parameters.Add(new FilterParameter("@IsCountPageTotal", true, DbType.Boolean));

                uxExportQueueGrid.DataSourceInvoker = new ASFuncInvoker(
                    WebServices.RiskServices,
                    WebSiteConstants.GET_REPORT_METHOD_NAME,
                    new object[] { SPA_GET_EXPORT_QUEUE, ReportServices.ConvertToFilterParamWSArray(parameters) });
                RegisterScriptRefresh();
                break;
        }
    }

    protected void uxExportQueueGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindExportGrid, sender);
    }

    protected void uxExportQueueGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (!(e.Item is GridDataItem))
            return;

        GridDataItem dataItem = (GridDataItem)e.Item;
        DataRowView dataRow = (DataRowView)e.Item.DataItem;

        Literal uxLitReportName = (Literal)dataItem["Download"].FindControl("uxLitReportName");
        LinkButton uxDownloadReport = (LinkButton)dataItem["Download"].FindControl("uxDownloadReport");

        switch ((int)dataRow["Status"])
        {
            case 0: // Export In Processing
            case 1:
                uxDownloadReport.Visible = false;
                uxLitReportName.Visible = true;
                ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = false;
                break;
            case 2: // Exported Completely
                uxDownloadReport.Visible = true;
                uxLitReportName.Visible = false;
                ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = true;
                break;
            case 3: // Failed in exporting
                uxDownloadReport.Visible = false;
                uxLitReportName.Visible = true;
                ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = true;
                dataItem["StatusDesc"].ForeColor = System.Drawing.Color.Red;
                break;
        }
    }

    #endregion Grid Events

    #region Button Events

    protected void btnRefreshGrid_Click(object sender, EventArgs e)
    {
        uxExportQueueGrid.Rebind();
    }

    protected void DeleteExportQueue(object sender, CommandEventArgs e)
    {
        int processLogId = 0;
        int.TryParse(e.CommandArgument.ToString(), out processLogId);

        var parameters = new FilterParameterCollection();
        var paramOuts = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@ProcessLogId", processLogId, DbType.Int32));

        WebServices.CsReportServices.ExecuteNonQueryCommand(SPA_DELETE_EXPORT_QUEUE, parameters, out paramOuts);

        uxExportQueueGrid.Rebind();
    }

    protected void uxbtnDownload_Click(object sender, EventArgs e)
    {
        string listDocId = uxDocId.Value;
        string fileName = uxFileName.Value;
        DownloadFile(listDocId, fileName);
    }

    #endregion Button Events

    #region Helper Methods

    private void DownloadFile(string listDocID, string fileName)
    {
        List<int> docIds = listDocID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => int.Parse(id.Trim())).ToList();

        if (docIds.Count == 1)
        {
            byte[] buffer = WebServices.DocServices.DownloadDoc(docIds[0]);
            this.TransferFileToClient(buffer, GeneralFuncsLib.FormatFileName(fileName));
            return;
        }

        using (MemoryStream mergedStream = new MemoryStream())
        {
            foreach (int docId in docIds)
            {
                byte[] chunkData = WebServices.DocServices.DownloadDoc(docId);
                mergedStream.Write(chunkData, 0, chunkData.Length);
            }

            mergedStream.Position = 0;
            this.TransferFileToClient(mergedStream, GeneralFuncsLib.FormatFileName(fileName));
        }
    }

    private void RegisterScriptRefresh()
    {
        if (IsPostBack)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "refreshGrid", "onRefreshExportGrid()", true);
        }

        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "refreshGrid", "onRefreshExportGrid()", true);
        }
    }

    #endregion Helper Methods
}