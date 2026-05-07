using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;
using System.Web;

[PagePermission("Statistics,MSStatistics")]
public partial class rm_MCF_Statistics : ReportPage
{
    enum DataBindAction
    {
        BindReportGrid,
    }

    public DateTime _FilterDate = DateTime.Now;

    protected override void PageInitialize()
    {
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // 38605: Advanced Filter
        uxAdvancedFilter.FilterPage = FilterPageEnums.PortfolioStatistics;
        if (IsIntruderDetected) return;
        IsBindDataOnLoad = true;
        uxAdvancedFilter.IsBindData = true;
        // End - 38605

    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {

        if (sender == uxSatisticsReport && uxSatisticsReport.Visible)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserReportingParams();                   
                    parameters.AddLanguageID();
                    DataTable dt = WebServices.RiskServices.GetReports("spa_RM_SR_Get_StatisticsReportsList", parameters);
                    ((ASGrid)sender).DataSource = dt;
                    AjaxAddResponseScript(string.Format("UpdateTotalRecord({0});", dt.Rows.Count));
                }
                break;
        }

    }


    protected void uxSatisticsReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected void DeleteFileExport(object sender, CommandEventArgs e)
    {
        int recordID = Int32.Parse(e.CommandArgument.ToString().Split(',')[0]);
        int docId = 0;
        Int32.TryParse(e.CommandArgument.ToString().Split(',')[1], out docId);
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection paramOuts = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        parameters.Add(new FilterParameter("@RecordID ", recordID, DbType.Int32));
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_SR_Delete_StatisticsReports", parameters, out paramOuts);
        if (docId != 0)
        {
            WebServices.DocServices.DeleteFileOnDocServer(SessionManager.CurrentUser.ASClient.ToString(), SessionManager.CurrentUser.UserID, docId.ToString());
        }
        uxSatisticsReport.Rebind();
    }

    #region HELPER METHODS
    private void DownloadFile(int docID, string fileName)
    {
        byte[] buffer = WebServices.DocServices.DownloadDoc(docID);
        this.TransferFileToClient(buffer, GeneralFuncsLib.FormatFileName(fileName));
    }
    #endregion

    protected void uxSatisticsReport_DataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            switch ((int)dataRow["Status"])
            {
                case 0: // Export In Processing
                case 1:
                    ((LinkButton)dataItem["ReportType"].FindControl("uxDownloadReport")).Visible = false;
                    ((Literal)dataItem["ReportType"].FindControl("uxLitReportType")).Visible = true;
                    ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = false;
                    break;
                case 2: // Exported Completely
                    ((LinkButton)dataItem["ReportType"].FindControl("uxDownloadReport")).Visible = true;
                    ((Literal)dataItem["ReportType"].FindControl("uxLitReportType")).Visible = false;
                    ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = true;
                    break;
                case 3: // Failed in exporting
                    ((LinkButton)dataItem["ReportType"].FindControl("uxDownloadReport")).Visible = false;
                    ((Literal)dataItem["ReportType"].FindControl("uxLitReportType")).Visible = true;
                    ((LinkButton)dataItem["Delete"].FindControl("uxDelete")).Visible = true;
                    break;
            }

        }
    }
    
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        uxSatisticsReport.Rebind();
    }

    protected void uxbtnDownload_Click(object sender, EventArgs e)
    {
        int docId = Int32.Parse(uxDocId.Value);
        string fileName = uxFileName.Value;
        DownloadFile(docId, fileName);
    }
}