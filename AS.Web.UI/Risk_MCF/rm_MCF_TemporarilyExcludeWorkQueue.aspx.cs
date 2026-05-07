using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.UserControls;
using AS.Controls.Pages;

[PagePermission("RskAutoQueue,MSRskAutoQueue")]
public partial class rm_MCF_TemporarilyExcludeWorkQueue : ReportPage
{
    #region var and enum
    private long _exclusionId;
    enum DataBindAction
    {
        BindExclusion,
        RebindListAfterCreateClick,
        RebindListAfterCancelClick
    }
    enum PostBackAction
    {
        RebindListClick,
        ViewActiveClick,
        ViewAllClick,
        DeleteExcludeClick,
        CreateClick        
    }
    #endregion

    #region Event
    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindExclusion:
                uxReportGrid.DataSource = GetExclusionList(uxViewActiveOnly.Checked);
                break;
            case DataBindAction.RebindListAfterCreateClick:
                uxExcludeWorkQueueCreate.Visible = false;
                uxReportGrid.MasterTableView.ClearEditItems();
                uxReportGrid.Rebind();
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.ViewAllClick:
            case PostBackAction.ViewActiveClick:                
                uxReportGrid.MasterTableView.ClearEditItems();
                uxReportGrid.Rebind();
                break;
            case PostBackAction.DeleteExcludeClick:
                DeleteExclusion(_exclusionId);
                uxReportGrid.MasterTableView.ClearEditItems();
                uxExcludeWorkQueueCreate.Visible = false;
                uxReportGrid.Rebind();
                break;
            case PostBackAction.CreateClick:
                uxExcludeWorkQueueCreate.Visible = true;
                uxReportGrid.MasterTableView.ClearEditItems();
                uxExcludeWorkQueueCreate.ResetForm();
                uxReportGrid.Rebind();
                break;
            
        }
    }

    protected override void PageInitialize()
    {
        base.PageInitialize();
        this.IsBindDataOnLoad = true;
        //SessionManager.CurrentLanguage = (int)WebSiteEnums.LanguageCode.Spanish;
    }
    
    protected void uxViewActiveOnly_CheckedChanged(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ViewActiveClick);
    }

    protected void uxViewAll_CheckedChanged(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ViewAllClick);
    }

    protected void uxReportGrid_PageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        uxReportGrid.MasterTableView.ClearEditItems();
    }

    protected void uxExcludeWorkQueue_AfterCancel(object sender)
    {
        OnDataBindControls(DataBindAction.RebindListAfterCreateClick);
    }

    protected void uxExcludeWorkQueue_AfterSubmit(object sender)
    {
        OnDataBindControls(DataBindAction.RebindListAfterCreateClick);
    }

    protected void uxCreateMode_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CreateClick);
    }

    protected void uxReportGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindExclusion);
    }

    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case GridItemType.Item:
            case GridItemType.AlternatingItem:
                {
                    GridDataItem item = e.Item as GridDataItem;
                    DataRowView dataItem = (DataRowView)e.Item.DataItem;

                    if (DateTime.Compare(Convert.ToDateTime(dataItem["ToDate"]).Date, DateTime.Now.Date) < 0)
                    {
                        item["EditCommandColumn"].Text = string.Empty;
                    }

                    if (item.Edit)
                    {
                        item["EditCommandColumn"].CssClass = "text-gray-light";
                        item["EditCommandColumn"].Enabled = false;
                    }
                    item["AutoQueueNames"].Text = VeraCodeExtensions.DoVeraCode(item["AutoQueueNames"].Text.TrimEnd(' ').TrimEnd(','));
                }
                break;            
            case GridItemType.EditFormItem:
                if (e.Item.IsInEditMode)
                {                   

                    UserControls_RiskTemporarilyExcludeWorkQueue uxEditExcludeWorkQueue =
                        (UserControls_RiskTemporarilyExcludeWorkQueue)e.Item.FindControl("uxEditExcludeWorkQueue");
                    DataRowView dataItem = (DataRowView)e.Item.DataItem;
                    uxEditExcludeWorkQueue.DoBindData(dataItem["ExclusionID"].ToString(), dataItem["AssignmentID"].ToString(), 
                        dataItem["WorkQueueName"].ToString(), dataItem["AutoQueueIDs"].ToString(), (DateTime)dataItem["FromDate"], 
                        (DateTime)dataItem["ToDate"]);
                    uxExcludeWorkQueueCreate.Visible = false;

                }
                break;
        }
    }

    protected void uxReportGrid_UpdateCommand(object source, GridCommandEventArgs e)
    {
        e.Item.Edit = false;
        uxExcludeWorkQueueCreate.Visible = false;
    }

    protected void DeleteExclude_Command(object sender, CommandEventArgs e)
    {
        _exclusionId = e.CommandArgument.ToLong();
        OnPostBackActions(PostBackAction.DeleteExcludeClick);
    }

    #endregion

    #region Data
    private void DeleteExclusion(long exclusionId)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ExclusionID", exclusionId, DbType.Int64));
        
        FilterParameterCollection outparameters = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_AQ_Delete_Exclusion", parameters, out outparameters);
    }
    private DataTable GetExclusionList(bool isActive)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@IsActive", isActive, DbType.Boolean));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_AQ_Get_ExclusionList", parameters);
    }
    #endregion
}
