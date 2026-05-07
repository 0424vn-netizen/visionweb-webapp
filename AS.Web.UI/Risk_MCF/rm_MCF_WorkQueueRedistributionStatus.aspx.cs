using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Controls.Grid;
using Telerik.Web.UI;

[PagePermission("RskAutoQueue,MSRskAutoQueue")]
public partial class rm_MCF_WorkQueueRedistributionStatus : NonReportPage
{
    enum DistributionStatus
    {
        Queued = 0,
        Completed = 1,
        InProcess = 2
    }

    protected override void PageInitialize()
    {
        PageType = SecurePageType.Modal;
        base.PageInitialize();
    }

    protected void uxGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        var status = GetStatusFilter();

        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@QueueStatusIDs", status, DbType.AnsiString));
        uxGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] { "spa_RM_MCF_AQ_Get_RedistributionList", ReportServices.ConvertToFilterParamWSArray(parameters) });
        //uxGrid.DataSource = WebServices.RiskServices.GetReports("spa_RM_AutoQueue_Get_RedistributionList", parameters);
        //uxGrid.DataBind();
    }
    protected void uxSubmitAjax_Click(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        uxGrid.Rebind();
        NonReportPage page = (NonReportPage)this.Page;
        page.AjaxAddResponseScript("ResetRefesh();");
    }

    private string GetStatusFilter()
    {
        string statusCheck = ",";
        if (chbxQueued.Checked) statusCheck += ((int)DistributionStatus.Queued).ToString() + ",";
        if (chbxInProgress.Checked) statusCheck += ((int)DistributionStatus.InProcess).ToString() + ",";
        if (chbxCompleted.Checked) statusCheck += ((int)DistributionStatus.Completed).ToString() + ",";
        return statusCheck;
    }

    protected void uxGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = dataItem.DataItem as DataRowView;
            dataItem["FromWorkQueue"].Text = VeraCodeExtensions.DoVeraCode(dataRow["SourceAssignmentNames"].ToString().TrimEnd(' ').TrimEnd(','));
            dataItem["ToWorkQueue"].Text = VeraCodeExtensions.DoVeraCode(dataRow["DestinationAssignmentNames"].ToString().TrimEnd(' ').TrimEnd(','));
            if (dataRow["QueueStatusID"].ToInt() == 1)// 1 - completed
            {
                if (dataRow["QueueDesc"] == DBNull.Value)
                    dataItem["Description"].Text = string.Empty;
                else if (dataRow["QueueDesc"].ToInt() > 1)
                    dataItem["Description"].Text = VeraCodeExtensions.DoVeraCode(string.Format(
                        GetLocalResourceObject("ManyMerchantsRedistributed").ToString(),
                        dataRow["QueueDesc"]));
                else if (dataRow["QueueDesc"].ToInt() <= 1)
                    dataItem["Description"].Text = VeraCodeExtensions.DoVeraCode(string.Format(
                        GetLocalResourceObject("OneMerchantRedistributed").ToString(),
                        dataRow["QueueDesc"]));
                else
                    dataItem["Description"].Text = string.Empty;

                dataItem["ProcessedOn"].Text = VeraCodeExtensions.DoVeraCode(string.Format("{0:MM/dd/yyyy hh:mm tt}",
                    dataRow["RedistributedDTS"]));
            }
            else
            {
                dataItem["Description"].Text = string.Empty;
                dataItem["ProcessedOn"].Text = string.Empty;
            }
        }
    }
    protected void chbxQueued_CheckedChanged(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        uxGrid.Rebind();
    }
}