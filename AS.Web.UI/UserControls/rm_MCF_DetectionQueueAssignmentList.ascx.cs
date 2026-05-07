using AS.Common;
using AS.Common.DBManager;
using AS.Security.WS.Entities.Utility;
using AS.Utilities;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_rm_MCF_DetectionQueueAssignmentList : GlobalUserControl
{
    enum DataBindAction
    {
        BindAssignmentList
    }

    enum PostBackAction
    {
        BindAssignmentEvent,
        ViewAssignmentCommand
    }

    public event GridNeedDataSourceEventHandler NeedDataSource;
    public event GridItemEventHandler ItemDataBound;

    private const string ASSIGNMENT_ID = "AssignmentID";
    private const string ASSIGNMENT_TYPE = "AssignmentType";
    private const string FEATURE_MODE = "FeatureMode";
    private const string REVIEW_MODE = "ReviewMode";

    protected virtual void OnNeedDataSource(GridNeedDataSourceEventArgs e)
    {
        NeedDataSource.Fire(d => (d as GridNeedDataSourceEventHandler)(this, e));
    }

    protected virtual void OnItemDataBound(GridItemEventArgs e)
    {
        ItemDataBound.Fire(d => (d as GridItemEventHandler)(this, e));
    }

    public GridTableView MasterTableView
    {
        get
        {
            return this.uxAssignmentList.MasterTableView;
        }
    }

    public int AssignmentID { get; set; }
    public DateTime ReportDate { get; set; }
    public string ApplyFilterId { get; set; }
    public string NoDataMessage { get; set; }
    public string FromPage { get; set; }
    public object DataSource
    {
        get { return this.uxAssignmentList.DataSource; }
        set { this.uxAssignmentList.DataSource = value; }
    }

    string _AssignmentIntruderQuery = string.Empty;
    private string AssignmentIntruderQuery
    {
        get
        {
            if (_AssignmentIntruderQuery.Length == 0)
                _AssignmentIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { ASSIGNMENT_ID });
            return _AssignmentIntruderQuery;
        }
    }

    bool _autoBindData = true;
    public bool AutoBindData
    {
        get
        {
            return _autoBindData;
        }
        set
        {
            _autoBindData = value;
        }
    }

    public void Rebind()
    {
        if (!string.IsNullOrEmpty(NoDataMessage))
        {
            uxAssignmentList.MasterTableView.NoDetailRecordsText = NoDataMessage;
            uxAssignmentList.MasterTableView.NoMasterRecordsText = NoDataMessage;
        }

        AutoBindData = true;
        uxAssignmentList.Rebind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        var noResultMessage = Resources.LanguageResource.AS_ASRepeater_NoDataFound;
        uxAssignmentList.MasterTableView.NoDetailRecordsText = noResultMessage;
        uxAssignmentList.MasterTableView.NoMasterRecordsText = noResultMessage;
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;

        switch ((PostBackAction)type)
        {
            case PostBackAction.BindAssignmentEvent:
                {
                    uxAssignmentList.Rebind();
                }
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;

        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindAssignmentList:
                {
                    DataTable table = new DataTable();
                    if (AutoBindData)
                    {
                        table = GetData();
                    }

                    uxAssignmentList.DataSource = table;
                    uxAssignmentList.ClientSettings.Scrolling.ScrollBarWidth = Unit.Pixel(4000);

                    if (table.Rows.Count > 0 && table.Rows.Count > 5)
                    {
                        uxAssignmentList.ClientSettings.Scrolling.ScrollHeight = Unit.Pixel(180);
                    }
                    else if (table.Rows.Count == 0)
                    {
                        uxAssignmentList.ClientSettings.Scrolling.ScrollHeight = Unit.Pixel(30);
                    }
                    else
                    {
                        uxAssignmentList.ClientSettings.Scrolling.ScrollHeight = Unit.Pixel(30 * table.Rows.Count);
                    }

                    SetVisibleForRequeuedColumns();
                }
                break;
        }
    }

    protected void uxAssignmentList_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindAssignmentList);
        //fire event
        this.OnNeedDataSource(e);
    }

    private void SetVisibleForRequeuedColumns()
    {
        // Re-queued Assignment count, Re-queued Assignment Volume
        uxAssignmentList.Columns.FindByUniqueName("RequeueCount").Visible = GeneralFuncsLib.HasQueuingMechanismFeature;
        uxAssignmentList.Columns.FindByUniqueName("RequeueVolume").Visible = GeneralFuncsLib.HasQueuingMechanismFeature;
    }

    protected void uxAssignmentList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            var assignmentID = dataRow[ASSIGNMENT_ID].ToString().Trim();
            if (int.Parse(assignmentID) > 0)
            {
                if (dataRow[ASSIGNMENT_TYPE] != null
                    && !string.IsNullOrEmpty(dataRow[ASSIGNMENT_TYPE].ToString())
                    && dataRow[ASSIGNMENT_TYPE].ToString() == ((int)WebSiteEnums.AssignmentType.DetectionQueueDistinct).ToString())
                {
                    dataItem["AssignmentName"].Text = VeraCodeSolution.DoVeraCode(
                       string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"alert('{0}'); return false;\">{1}</a>",
                       Resources.RiskMessageManager.Risk_CannotEditDTAssignment, dataRow["AssignmentName"]));
                }

                if (dataRow[ASSIGNMENT_TYPE] != null
                    && !string.IsNullOrEmpty(dataRow[ASSIGNMENT_TYPE].ToString())
                    && dataRow[ASSIGNMENT_TYPE].ToString() == ((int)WebSiteEnums.AssignmentType.AggregateQueue).ToString())
                {
                    dataItem["AssignmentName"].Text = VeraCodeSolution.DoVeraCode(dataRow["AssignmentName"].ToString());
                }

                if (dataItem["AlertMerchantVolume"].Text.Contains("("))
                {
                    dataItem["AlertMerchantVolume"].ForeColor = Color.Red;
                }

                if (dataItem["WKVolume"].Text.Contains("("))
                {
                    dataItem["WKVolume"].ForeColor = Color.Red;
                }

                dataItem[ASSIGNMENT_TYPE].Text = GeneralFuncsLib.BuildAssignmentTypeAbbr(dataRow[ASSIGNMENT_TYPE]);
            }
        }
    }

    private DataTable GetData()
    {
        DataTable assignmentList = new DataTable();

        FilterParameterCollection parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.Add(new FilterParameter("@AssignmentID", AssignmentID, DbType.Int32));
        parameterList.Add(new FilterParameter("@ReportDate", ReportDate, DbType.Date));
        parameterList.Add(new FilterParameter("@GroupBy", "Assignment", DbType.String));
        //Mode = 0 --- ALL
        parameterList.Add(new FilterParameter("@Mode", "0", DbType.String));
        parameterList.Add(new FilterParameter("@ApplyFilterId", ApplyFilterId, DbType.String));
        string spName = "spa_RM_MCF_Get_Assignment";

        if (AssignmentID > 0)
        {
            assignmentList = WebServices.RiskServices.GetReports(spName, parameterList);

            if(assignmentList.Rows.Count == 0)
            {
                LogHepler.WriteLogWarn("DetectionQueueAssignmentList GetData GetReports", "Assignment list is null", spName, parameterList);
            }
        }
        else 
        {
            LogHepler.WriteLogWarn("DetectionQueueAssignmentList GetData", "AssignmentID is invalid: " + AssignmentID, spName, parameterList);
        }

        return assignmentList;
    }

    protected void btnViewAssignment_Click(object sender, EventArgs e)
    {
        if (FromPage.Equals("Barometer"))
        {
            Rebind();
        }
        if (!string.IsNullOrEmpty(hddAssignmentValue.Value))
        {
            int assignmentID = RM_MCF_GeneralFuncsLib.MoveDataFromFinalTables(int.Parse(hddAssignmentValue.Value));
            string queryString = this.Page.BuildSecureQueryString(string.Format("{0}={1}&{2}={3}&{4}={5}{6}",
               ASSIGNMENT_ID, assignmentID, FEATURE_MODE, (int)WebSiteEnums.FeatureMode.View, REVIEW_MODE, 1, this.AssignmentIntruderQuery));

            if (RiskSessionManager.IsUsingMarketData)
            {
                ((ReportPage)Page).AjaxAddResponseScript("ShowPopupModal('rm_MCF_Assignment_CreateNew_MarketData.aspx?" + queryString + "','auto');");
            }
            else
            {
                ((ReportPage)Page).AjaxAddResponseScript("ShowPopupModal('rm_MCF_Assignment_CreateNew.aspx?" + queryString + "','auto');");
            }
        }
    }
}