using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Controls.Pages;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Common;
using Resources;
using AS.Controls.UserControls;

[PagePermission("RskReassign,MSRskReassign")]
public partial class rm_MCF_TempReassignUser : ReportPage
{
    private int _reassignmentID;
    enum DataBindAction
    {
        BindUserListGrid,
        RebindUserListAfterCreateClick,
        RebindUserListAfterCancelClick

    }
    enum PostBackAction
    {
        RebindUserListClick,
        ViewActiveClick,
        ViewAllClick,
        DeleteReassignmentClick,
        CreateClick

    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindUserListGrid:
                uxReportGrid.DataSource = GetAllReassignments(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, uxViewAll.Checked);
                break;
            case DataBindAction.RebindUserListAfterCreateClick:
                uxReassignUser.Visible = false;
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
            case PostBackAction.DeleteReassignmentClick:
                DeleteReassignment(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, _reassignmentID);
                uxReportGrid.MasterTableView.ClearEditItems();
                uxReassignUser.Visible = false;
                uxReportGrid.Rebind();
                break;
            case PostBackAction.CreateClick:
                uxReassignUser.Visible = true;
                uxReportGrid.MasterTableView.ClearEditItems();
                uxReassignUser.ResetForm();
                uxReportGrid.Rebind();
                break;

        }
    }

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporter_Btm");
        base.PageInitialize();
        this.IsBindDataOnLoad = true;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
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
    protected void uxReassignUser_AfterCancel(object sender)
    {
        OnDataBindControls(DataBindAction.RebindUserListAfterCreateClick);
    }
    protected void uxReassignUser_AfterSubmit(object sender, string userName, string userReassign, DateTime fromDate, DateTime toDate)
    {
        OnDataBindControls(DataBindAction.RebindUserListAfterCreateClick);
    }

    protected void uxCreateMode_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CreateClick);
    }
    protected void uxReportGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindUserListGrid);

    }

    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
        }
        switch (e.Item.ItemType)
        {
            case GridItemType.Item:
            case GridItemType.AlternatingItem:
                {
                    GridDataItem item = e.Item as GridDataItem;
                    DataRowView dataItem = (DataRowView)e.Item.DataItem;
                    if (!Convert.ToBoolean(dataItem["Edit"]))
                        item["EditCommandColumn"].Text = "";
                }
                break;
            case GridItemType.EditFormItem:
                if (e.Item.IsInEditMode)
                {
                    UserControls_RiskReassignUser uxEditReassignUser = (UserControls_RiskReassignUser)e.Item.FindControl("uxEditReassignUser");
                    DataRowView dataItem = (DataRowView)e.Item.DataItem;
                    uxEditReassignUser.DoBindData(dataItem["ReassignmentID"].ToString(), dataItem["UserID"].ToString(), dataItem["ReassignedUserID"].ToString(), (DateTime)dataItem["FromDate"], (DateTime)dataItem["ToDate"]);
                    uxReassignUser.Visible = false;
                }
                break;
        }
    }

    protected void uxReportGrid_UpdateCommand(object source, GridCommandEventArgs e)
    {
        e.Item.Edit = false;
        uxReassignUser.Visible = false;
    }

    protected void DeleteReassignment_Command(object sender, CommandEventArgs e)
    {
        _reassignmentID = int.Parse(e.CommandArgument.ToString());
        OnPostBackActions(PostBackAction.DeleteReassignmentClick);
    }

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        uxReportGrid.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("Delete").Visible = false;
        exportConfig.AllowHtmlEncoded = true;
        exportConfig.ReportHeader = GetLocalResourceObject("rm_TempResassignUser_js_Temporarily").ToString();
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(GetLocalResourceObject("rm_TempResassignUser_js_Temporarily").ToString());
    }


    #region Data
    public int DeleteReassignment(int dDSClient, string userID, int reassignmentId)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@ReassignmentID", reassignmentId, DbType.Int32));
        parameters.Add(new FilterParameter("@ReturnValue", 0, DbType.Int32, true));
        FilterParameterCollection outparameters = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_DeleteReassignment", parameters, out outparameters);
        return (int)outparameters[0].ParameterValue;
    }
    public DataTable GetAllReassignments(int dDSClient, string userID, bool all)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@All", all ? 1 : 0, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_rm_cs_GetAllReassignments", parameters);
    }
    #endregion


}
