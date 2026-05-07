using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using Resources;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System.ComponentModel;

public partial class UserControls_RiskTemporarilyExcludeWorkQueue : GlobalUserControl
{
    #region var and properties
    string _mode = "";
    string _validateTemplate = "return ValidateData('{0}','{1}','{2}','{3}');";

    private DateTime _OldFromDate
    {
        get
        {
            if (ViewState["_OldFromDate"] == null) ViewState["_OldFromDate"] = DateTime.MinValue;
            return Convert.ToDateTime(ViewState["_OldFromDate"]);
        }
        set
        {
            ViewState["_OldFromDate"] = value;
        }
    }

    private DateTime _OldToDate
    {
        get
        {
            if (ViewState["_OldToDate"] == null) ViewState["_OldToDate"] = DateTime.MinValue;
            return Convert.ToDateTime(ViewState["_OldToDate"]);
        }
        set
        {
            ViewState["_OldToDate"] = value;
        }
    }

    protected bool _UpdateMode = false;
    public bool UpdateMode { get { return _UpdateMode; } set { _UpdateMode = value; } }
    public event AfterSubmitHandler AfterSubmit;
    public event AfterCancelHandler AfterCancel;
    public delegate void AfterSubmitHandler(object sender);
    public delegate void AfterCancelHandler(object sender);

    [Category("Behavior"), DefaultValue(""), Description("Container css for outer div"), NotifyParentProperty(true),]
    public string ContainerCss { get; set; }
    #endregion

    #region event
    protected override void OnPreRender(EventArgs e)
    {
        if ((ContainerCss != null) && (ContainerCss != String.Empty))
        {
            dvOuter.Attributes.Add("class", String.Format("{0}", ContainerCss));
        }

        base.OnPreRender(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page.IsIntruderDetected) return;
        if (!IsPostBack)
        {
            DoBindData(null, null, null, null, null, null);
        }

        if (_UpdateMode)
        {
            _mode = "update";
            uxExcludeWorkQueue.Visible = false;
            uxExcludeWorkQueueEdit.Visible = true;
        }
        else
        {
            uxExcludeWorkQueue.Visible = true;
            uxExcludeWorkQueueEdit.Visible = false;
            _mode = "add";
            uxSDate.MinDate = uxEDate.MinDate = DateTime.Now.AddDays(1);
        }
        uxUpdate.OnClientClick = String.Format(_validateTemplate, uxExcludeWorkQueueEdit.Text, uxSDate.ClientID, uxEDate.ClientID, _mode);
    }

    protected void uxCancel_Click(object sender, EventArgs e)
    {
        if (!_UpdateMode)//create mode
        {
            this.Visible = false;

        }
        if (AfterCancel != null)
        {
            AfterCancel(this);
        }
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        uxAutoQueueNameErrMsg.Message = String.Empty;
        uxExcludeWorkQueueErrMsg.Message = String.Empty;
        if (this.Page.IsIntruderDetected) return;
        if (!DoValidateInput()) return;

        var assignmentId = _UpdateMode ? hddAssignmentID.Value.ToLong() : uxExcludeWorkQueue.SelectedValue.ToLong();
        var exclusionId = _UpdateMode ? uxUpdate.CommandArgument.ToLong() : 0;
        string xmlAutoQueue = ConvertXMLAutoQueue(assignmentId, uxAutoQueueName.SelectedItems);
        var result = CheckExclusionOverlap(assignmentId, xmlAutoQueue, uxSDate.SelectedDate.Value,
            uxEDate.SelectedDate.Value, exclusionId);

        if (result)
        {
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(
                String.Format("setTimeout(\"showRadMessage('alert', '{0}','','{1}');\", 100);",
                UserMaintenanceMessage.TemporaryExcludeWorkQueue_Overlap, Resources.ValMsg.modalWarning));
            return;
        }

        if (_UpdateMode)
        {
            UpdateExclude(uxUpdate.CommandArgument.ToLong(), assignmentId, xmlAutoQueue,
                uxSDate.SelectedDate.Value, uxEDate.SelectedDate.Value);
        }
        else
        {
            CreateExclude(uxExcludeWorkQueue.SelectedValue.ToLong(), xmlAutoQueue, uxSDate.SelectedDate.Value,
                uxEDate.SelectedDate.Value);
        }

        if (AfterSubmit != null)
            AfterSubmit(this);
    }

    protected void uxExcludeWorkQueue_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (uxExcludeWorkQueue.SelectedIndex < 0 || uxExcludeWorkQueue.Text == string.Empty)
        {
            uxAutoQueueName.Items.Clear();
        }
        else
        {
            BindAutoQueueList(uxExcludeWorkQueue.SelectedValue.ToLong());
        }
    }
    #endregion

    #region method
    private string ConvertXMLAutoQueue(long assignmentId, ListItemCollection items)
    {
        var autoQueueList = new StringBuilder();
        autoQueueList.Append("<Exclusions>");
        foreach (ListItem item in items)
        {
            autoQueueList.Append(string.Format("<Exclusion><AutoQueueID>{0}</AutoQueueID><AssignmentID>{1}</AssignmentID></Exclusion>",
                item.Value, assignmentId));
        }
        autoQueueList.Append("</Exclusions>");
        return autoQueueList.ToString();
    }

    public bool DoValidateInput()
    {
        if (!uxSDate.SelectedDate.HasValue)
        {
            this.Page.IsIntruderDetected = true;
            return false;
        }
        if (!uxEDate.SelectedDate.HasValue)
        {
            this.Page.IsIntruderDetected = true;
            return false;
        }
        if (uxEDate.SelectedDate.Value < uxSDate.SelectedDate.Value)
        {
            this.Page.IsIntruderDetected = true;
            return false;
        }
        if (_UpdateMode)
        {
            if (DateTime.Compare(_OldFromDate.Date, DateTime.Now.Date) > 0 &&
                DateTime.Compare(_OldToDate.Date, DateTime.Now.Date) > 0 &&
                (DateTime.Compare(uxSDate.SelectedDate.Value.Date, DateTime.Now.Date) <= 0 ||
                DateTime.Compare(uxEDate.SelectedDate.Value.Date, DateTime.Now.Date) <= 0))
            {
                this.Page.IsIntruderDetected = true;
                return false;
            }
            if (DateTime.Compare(_OldFromDate.Date, DateTime.Now.Date) <= 0 &&
                DateTime.Compare(_OldToDate.Date, DateTime.Now.Date) >= 0 &&
                (DateTime.Compare(uxSDate.SelectedDate.Value.Date, DateTime.Now.Date) > 0 ||
                DateTime.Compare(uxEDate.SelectedDate.Value.Date, DateTime.Now.Date) < 0))
            {
                this.Page.IsIntruderDetected = true;
                return false;
            }
        }
        else if (DateTime.Compare(uxSDate.SelectedDate.Value.Date, DateTime.Now.Date) <= 0 ||
                DateTime.Compare(uxEDate.SelectedDate.Value.Date, DateTime.Now.Date) <= 0)
        {
            this.Page.IsIntruderDetected = true;
            return false;
        }
        return true;
    }

    public void DoBindData(string exclusionId, string assignmentId, string selectWorkQueue, string selectAutoQueue, DateTime? fromDate, DateTime? toDate)
    {
        if (this.Page.IsIntruderDetected) return;

        BindExcludeWorkQueueList();
        if (_UpdateMode)
        {
            BindAutoQueueList(assignmentId.ToLong());
            uxUpdate.CommandArgument = exclusionId;
            if (selectWorkQueue != null)
            {
                uxExcludeWorkQueueEdit.Text = AS.Common.VeraCodeSolution.DoVeraCode(selectWorkQueue);
            }
            if (selectAutoQueue != null)
            {
                var queueIDs = selectAutoQueue.Trim().TrimEnd(',').Split(',');
                uxAutoQueueName.SetSelectedValue(queueIDs);
            }

            hddAssignmentID.Value = assignmentId;

            uxSDate.SelectedDate = fromDate;
            if (DateTime.Compare(fromDate.Value, DateTime.Now.Date) <= 0)
                uxSDate.Enabled = false;
            else
            {
                uxSDate.Enabled = true;
                uxSDate.MinDate = DateTime.Now.AddDays(1);
            }

            uxEDate.SelectedDate = toDate;

            if (!uxSDate.Enabled)
                uxEDate.MinDate = DateTime.Now;
            else
                uxEDate.MinDate = DateTime.Now.AddDays(1);

            _OldFromDate = fromDate.HasValue ? fromDate.Value : DateTime.MinValue;
            _OldToDate = toDate.HasValue ? toDate.Value : DateTime.MinValue;
        }

        uxUpdate.OnClientClick = String.Format(_validateTemplate, selectWorkQueue, uxSDate.ClientID, uxEDate.ClientID, _mode);
    }

    public void ResetForm()
    {
        uxExcludeWorkQueue.DataBind();
        uxExcludeWorkQueue.SelectedIndex = -1;
        uxAutoQueueName.Items.Clear();
        uxSDate.Clear();
        uxEDate.Clear();
    }

    private void BindExcludeWorkQueueList()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add("@AssignmentTypeID", 1, DbType.Int32);
        //IsExpired = 0 - not show expired Assignment
        parameters.Add("@stFilter", "IsExpired = 0", DbType.String);
        DataTable dtQueue = WebServices.RiskServices.GetReports("spa_RM_MCF_AQ_Get_AssignmentList", parameters);

        uxExcludeWorkQueue.DataSource = dtQueue;
        uxExcludeWorkQueue.DataValueField = "AssignmentID";
        uxExcludeWorkQueue.DataTextField = "AssignmentName";
        uxExcludeWorkQueue.DataBind();
        uxExcludeWorkQueue.Items.Insert(0, new RadComboBoxItem("\u00A0"));
    }

    private void BindAutoQueueList(long assignmentID)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add("@AssignmentID", assignmentID, DbType.Int64);
        parameters.Add("@IsActived", true, DbType.Boolean);

        uxAutoQueueName.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_AQ_Get_RequestList", parameters);
        uxAutoQueueName.DataValueField = "AutoQueueID";
        uxAutoQueueName.DataTextField = "AutoQueueName";
        uxAutoQueueName.DataBind();
    }
    #endregion

    #region Data
    private void CreateExclude(long assignmentID, string xmlAutoQueue, DateTime from, DateTime to)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int64));
        parameters.Add(new FilterParameter("@FromDate", from, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ToDate", to, DbType.AnsiString));
        parameters.Add(new FilterParameter("@XMLAutoQueue", xmlAutoQueue, DbType.Xml));
        parameters.Add(new FilterParameter("@ExclusionID", 0, DbType.Int32, true));
        FilterParameterCollection outparameters = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_AQ_Insert_Exclusion", parameters, out outparameters);
    }

    private void UpdateExclude(long exclusionID, long assignmentId, string xmlAutoQueue, DateTime from, DateTime to)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ExclusionID", exclusionID, DbType.Int64));
        parameters.Add(new FilterParameter("@AssignmentID", assignmentId, DbType.Int64));
        parameters.Add(new FilterParameter("@FromDate", from, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ToDate", to, DbType.AnsiString));
        parameters.Add(new FilterParameter("@XMLAutoQueue", xmlAutoQueue, DbType.Xml));
        FilterParameterCollection outparameters = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_AQ_Update_Exclusion", parameters, out outparameters);
    }

    private bool CheckExclusionOverlap(long assignmentId, string xmlAutoQueue, DateTime from, DateTime to, long exclusionId = 0)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        if (exclusionId != 0)
            parameters.Add(new FilterParameter("@ExclusionID", exclusionId.ToLong(), DbType.Int64));
        parameters.Add(new FilterParameter("@AssignmentID", assignmentId, DbType.Int64));
        parameters.Add(new FilterParameter("@FromDate", from, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ToDate", to, DbType.AnsiString));
        parameters.Add(new FilterParameter("@XMLAutoQueue", xmlAutoQueue, DbType.Xml));

        DataTable dtExclusion = WebServices.RiskServices.GetReports("spa_RM_MCF_AQ_Check_Exclusion", parameters);
        return dtExclusion.HasData();
    }
    #endregion Data
}
