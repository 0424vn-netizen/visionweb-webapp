using System;
using System.Data;

public partial class UserControls_rm_AutoQueueDetail : GlobalUserControl
{
    #region Fields
    protected RiskAutoQueueModel AutoQueueItem
    {
        get
        {
            if (RiskSessionManager.AutoQueue != null)
            {
                return RiskSessionManager.AutoQueue;
            }

            return new RiskAutoQueueModel();
        }
    }

    protected string LastRun
    {
        get
        {
            string lastRun = AutoQueueItem.lastRunText;
            return !string.IsNullOrEmpty(lastRun) ? lastRun : "--";
        }
    }
    protected string AqStatusCode
    {
        get { return RiskSessionManager.AutoQueueStatus; }
    }
    protected string AqEditLink
    {
        get
        {
            string urlString = string.Empty;
            if (AutoQueueItem.ID > 0)
            {
                long autoQueueID = AutoQueueItem.ID;
                var page = this.Page as ReportPage;
                string queryString = page.BuildSecureQueryString(string.Format("AutoQueueID={0}", autoQueueID));
                urlString = "rm_CreateEditAutoQueueModal.aspx?" + queryString;                                
            }

            return urlString;
        }
    }
    protected string AqAllChangeLog
    {
        get
        {
            string urlString = string.Empty;
            if (AutoQueueItem.ID > 0)
            {
                long autoQueueID = AutoQueueItem.ID;
                var page = this.Page as ReportPage;
                string queryString = page.BuildSecureQueryString(string.Format("AutoQueueID={0}", autoQueueID));
                urlString = "rm_AutoQueueMonitoringViewAllChanges.aspx?" + queryString;
            }

            return urlString;
        }
    }

    private DataTable _dataSourceAssignments;
    private DataTable _dataSourceWorkQueues;
    private DataTable _dataSourceChangelog;
    #endregion

    #region Function   

    private DataTable GetDataChangeLog(long queueId)
    {
        if (_dataSourceChangelog != null)
        {
            return _dataSourceChangelog;
        }

        _dataSourceChangelog = Rm_AutoQueueBusiness.GetDataChangeLog(queueId, false);

        return _dataSourceChangelog;
    }
    private DataTable GetWorkQueueList(long queueId)
    {
        if (_dataSourceWorkQueues != null)
        {
            return _dataSourceWorkQueues;
        }

        DataTable dataList = Rm_AutoQueueBusiness.GetDataAssignments(1, queueId);
        //dataList.Columns.Add("ClassText", typeof(string));
        foreach (DataRow row in dataList.Rows)
        {            
            bool isExpired = row["IsExpired"].ToInt() == 1 ? true : false;
            bool isExcluded = row["IsExcluded"].ToInt() == 1 ? true : false;
            if (isExpired || isExcluded)
            {
                //row["ClassText"] = "unSelectedItem";
                string textNote = string.Empty;
                if (isExpired)
                {
                    textNote = GetLocalResourceObject("textExpired").ToString();
                }
                if (isExcluded)
                {
                    textNote = GetLocalResourceObject("textExcluded").ToString();
                }
                if (isExcluded && isExpired)
                {
                    textNote = GetLocalResourceObject("textExpiredExcluded").ToString();
                }

                row["AssignmentName"] = string.Format(row["AssignmentName"].ToString() + "<span class=\"unSelectedItem\"> ({0})</span>", textNote);
            }
           
        }

        _dataSourceWorkQueues = dataList;

        return _dataSourceWorkQueues;
    }
    private DataTable GetAssignments(long queueId)
    {
        if (_dataSourceAssignments != null)
        {
            return _dataSourceAssignments;
        }

        _dataSourceAssignments = Rm_AutoQueueBusiness.GetDataAssignments(0, queueId);

        return _dataSourceAssignments;
    }

    public void BindDataDetail(long queueId)
    {       
        AssignmentsList.DataSource = GetAssignments(queueId);
        WorkQueueList.DataSource = GetWorkQueueList(queueId);
        WorkQueueList.DataBind();
        AssignmentsList.DataBind();
      
        ChangeLogGrid.DataSource = GetDataChangeLog(queueId);
        ChangeLogGrid.DataBind();
       
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (IsPostBack)
        {
            WorkQueueList.DataBind();
            AssignmentsList.DataBind();
        }     
    }
   
    #endregion
  
}