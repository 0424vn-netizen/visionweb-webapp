using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using AS.Common.Utilities;
using AS.Controls.Global;
using AS.Controls.Pages;
using AS.Core.Common.Export.Agent;
using AS.Utilities;
using Telerik.Web.UI;
using AS.Web.Business.Shared.Constants;
using BusGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;

[PagePermission("RskAutoQueue,MSRskAutoQueue")]
public partial class rm_MCF_CreateEditAutoQueueModal : NonReportPage
{
    #region Constants
    // 0: Assigment    // 1: Work queue
    private const string SPA_ASSIGNMENT_LIST = "spa_RM_MCF_AQ_Get_AssignmentList";

    private const string SPA_REQUEST_LIST = "spa_RM_MCF_AQ_Get_RequestList";

    private const string SPA_DELETE_AUTOQUEUE = "spa_RM_MCF_AQ_Delete_Request";

    private const string SPA_UPDATE_AUTOTQUEUE = "spa_RM_MCF_AQ_Update_Request";

    private const string SPA_INSERT_AUTOQUEUE = "spa_RM_MCF_AQ_Insert_Request";

    private const string SPA_CHECK_REQUEST = "spa_RM_MCF_AQ_Check_Request";

    private const string XML_TEMPLATE_AUTOQUEUE = @"<AutoQueue>{0}</AutoQueue>";

    private const string XML_TEMPLATE_AUTOQUEUE_ITEM = @"<Assignment><AssignmentID>{0}</AssignmentID><AssignmentTypeID>{1}</AssignmentTypeID></Assignment>";

    #endregion Constants

    #region Properties

    protected WebSiteEnums.AutoQueueMode AutoQueueMode
    {
        get
        {
            if (SecureQueryString != null && SecureQueryString["AutoQueueID"] != null)
            {
                long autoQueueId = int.Parse(SecureQueryString["AutoQueueID"]);
                return autoQueueId == 0 ? WebSiteEnums.AutoQueueMode.Create : WebSiteEnums.AutoQueueMode.Edit;
            }
            else
            {
                return WebSiteEnums.AutoQueueMode.Create;
            }
        }
    }

    protected long AutoQueueID
    {
        get
        {
            long autoQueueID = 0;
            if (SecureQueryString != null)
            {
                var isNum = Int64.TryParse(SecureQueryString["AutoQueueID"], out autoQueueID);
                if (!isNum)
                    return -1;
            }
            return autoQueueID;
        }
    }

    public bool IsValidForm
    {
        get
        {
            if (ViewState["_IsValidForm"] == null)
                return true;
            else
            {
                return ViewState["_IsValidForm"].ToBoolean();
            }
        }
        set { ViewState["_IsValidForm"] = value; }
    }
    #endregion

    #region Overrides

    #endregion

    #region Privates

    private string GetMessage(string dataIsExpired, string dataIsExcluded)
    {
        string[] extMessages = new[] { dataIsExpired, dataIsExcluded };
        extMessages = extMessages.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        var message = String.Join("&", extMessages);
        return message;
    }

    private void DataBindDetailAutoQueue()
    {
        if (AutoQueueID > 0)
        {
            DataTable dt = GetDetailAutoQueue();
            if (dt.Rows.Count > 0)
            {
                txtName.Text = dt.Rows[0]["AutoQueueName"].ToString().Trim();
                txtNameClientState.Value = dt.Rows[0]["AutoQueueName"].ToString().Trim();
                txtBriefDescription.Text = dt.Rows[0]["AutoQueueDesc"].ToString().Trim();
                rdActive.Checked = dt.Rows[0]["IsActived"].ToBoolean();
                rdInactive.Checked = !rdActive.Checked;
                //Sprint 2 - TK44078 - Remove Code
            }
        }
    }

    private void DataBindAssigment()
    {
        DataTable tb = GetDataAssigmentWorkQueue(0);
        lbAssigment.DataSource = tb;
        lbAssigment.DataBind();
    }

    private void DataBindWorkQueue()
    {
        DataTable tb = GetDataAssigmentWorkQueue(1);
        lbWorkQueue.DataSource = tb;
        lbWorkQueue.DataBind();

    }

    private string CreateXmlData(List<UserControls_rm_MCF_CustomListBoxItem> assigments, int type)
    {
        string itemAssigments = string.Empty;
        assigments.ForEach(a => itemAssigments += string.Format(XML_TEMPLATE_AUTOQUEUE_ITEM, a.Value.Trim(), type));

        string xml = string.Format(XML_TEMPLATE_AUTOQUEUE, itemAssigments);
        return xml;
    }

    private void ChangeNameBtnSubmit()
    {
        btnSubmitCover.Text = AutoQueueMode == WebSiteEnums.AutoQueueMode.Edit ? GetLocalResourceObject("btnSubmitEdit.Text").ToString() : GetLocalResourceObject("btnSubmitCreate.Text").ToString();
    }

    private void ChangeTitle()
    {
        Title = AutoQueueMode == WebSiteEnums.AutoQueueMode.Edit ? GetLocalResourceObject("titleEdit").ToString() : GetLocalResourceObject("titleCreate").ToString();
    }

    private List<RadListBoxItem> CheckRequestAssigment()
    {
        var assignmenstBlocked = new List<RadListBoxItem>();
        var xml = CreateXmlData(lbAssigment.SelectedItems.ToList(), 0);

        DataTable dt = CheckRequest(xml:xml);
        if (dt != null && dt.Rows.Count > 0)
        {
            dt.AsEnumerable().ForEach(r =>
            {
                var itemRow = new RadListBoxItem();
                itemRow.Value = r["AssignmentID"].ToString();
                itemRow.Text = r["AssignmentName"].ToString();
                assignmenstBlocked.Add(itemRow);
            });
        }
        return assignmenstBlocked;
    }

    private List<RadListBoxItem> CheckRequestWorkQueue()
    {
        var workQueuesBlocked = new List<RadListBoxItem>();

        DataTable dt = CheckRequest(action:"Delete");
        if (dt != null && dt.Rows.Count > 0)
        {
            dt.AsEnumerable().ForEach(r =>
            {
                if (r["IsDeleted"].ToString() == "1")
                {
                    var itemRow = new RadListBoxItem();
                    itemRow.Value = r["WorkQueueName"].ToString();
                    itemRow.Text = r["WorkQueueName"].ToString();
                    workQueuesBlocked.Add(itemRow);
                }
            });
        }
        return workQueuesBlocked;
    }

    private List<RadListBoxItem> CheckRequestUniqueName()
    {
        var names = new List<RadListBoxItem>();

        DataTable dt = CheckRequest(autoQueueName:txtName.Text.Trim());
        if (dt != null && dt.Rows.Count > 0)
        {
            dt.AsEnumerable().ForEach(r =>
            {
                var itemRow = new RadListBoxItem();
                itemRow.Value = r["AutoQueueID"].ToString();
                itemRow.Text = r["AutoQueueName"].ToString();
                names.Add(itemRow);
            });
        }
        return names;
    }

    #endregion

    #region Private SPAs

    private DataTable GetDataAssigmentWorkQueue(int type)
    {
        if (AutoQueueID == 0)
        {
            var paramIns = new FilterParameterCollection();
            paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paramIns.Add(new FilterParameter("@AssignmentTypeID", type, DbType.Int32));
            paramIns.Add(new FilterParameter("@AutoQueueID", AutoQueueID, DbType.Int64));
            DataTable dt = WebServices.RiskServices.GetReports(SPA_ASSIGNMENT_LIST, paramIns);
            return dt;
        }
        else
        {
            var paramIns = new FilterParameterCollection();
            paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paramIns.Add(new FilterParameter("@AssignmentTypeID", type, DbType.Int32));

            DataTable dt = WebServices.RiskServices.GetReports(SPA_ASSIGNMENT_LIST, paramIns);
            dt.Columns.Add("IsSelected", typeof(bool));
            dt.Columns.Add("IsExcludedTemp", typeof(bool));

            paramIns.Add(new FilterParameter("@AutoQueueID", AutoQueueID, DbType.Int64));
            DataTable dtWithAutoQueue = WebServices.RiskServices.GetReports(SPA_ASSIGNMENT_LIST, paramIns);
            dtWithAutoQueue.PrimaryKey = new DataColumn[] { dtWithAutoQueue.Columns["AssignmentID"] };

            dt.AsEnumerable().ForEach(r =>
            {
                bool isExist = dtWithAutoQueue.Rows.Contains(r["AssignmentID"]);
                var rowAutoQueque = dtWithAutoQueue.Rows.Find(r["AssignmentID"]);
                if (isExist)
                {
                    r["IsSelected"] = true;
                    r["IsUsed"] = DBNull.Value;
                    if (type == 1)
                        r["IsExcludedTemp"] = rowAutoQueque["IsExcluded"].ToString() == "1" ? true : false;
                }
                else
                    r["IsSelected"] = false;
            });
            return dt;
        }

    }

    private DataTable GetDetailAutoQueue()
    {
        var paramIns = new FilterParameterCollection();
        paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramIns.Add(new FilterParameter("@AutoQueueID", AutoQueueID, DbType.Int64));

        DataTable dt = WebServices.RiskServices.GetReports(SPA_REQUEST_LIST, paramIns);
        return dt;
    }

    private DataTable CheckRequest(string autoQueueName=null, string xml=null, string action=null)
    {
        var paramIns = new FilterParameterCollection();
        paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramIns.Add(new FilterParameter("@AutoQueueID", AutoQueueID, DbType.Int64));
        paramIns.Add(new FilterParameter("@AutoQueueName", autoQueueName, DbType.String));
        paramIns.Add(new FilterParameter("@XMLAssignment", xml, DbType.String));
        paramIns.Add(new FilterParameter("@Action", action, DbType.String));

        return  WebServices.RiskServices.GetReports(SPA_CHECK_REQUEST, paramIns);
    }

    private void CreateAutoQueue(string name, string description, bool isActive, string xmlDataAssignment, string xmlDataWorkQueue)
    {
        var paramIns = new FilterParameterCollection();
        var paramOuts = new FilterParameterCollection();
        paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramIns.Add(new FilterParameter("@AutoQueueName", name, DbType.String));
        paramIns.Add(new FilterParameter("@AutoQueueDesc", description, DbType.String));
        paramIns.Add(new FilterParameter("@IsActived", isActive, DbType.Boolean));
        paramIns.Add(new FilterParameter("@XMLAssignment", xmlDataAssignment, DbType.String));
        paramIns.Add(new FilterParameter("@XMLWorkQueue", xmlDataWorkQueue, DbType.String));
        paramIns.Add(new FilterParameter("@AutoQueueID", "", DbType.Int64, true));

        WebServices.RiskServices.ExecuteNonQueryCommand(SPA_INSERT_AUTOQUEUE, paramIns, out paramOuts);

        if (paramOuts.Count > 0)
        {
            var autoQueueIdOut = paramOuts[0].ParameterValue;
            if (RiskSessionManager.AutoQueue == null)
                RiskSessionManager.AutoQueue = new RiskAutoQueueModel();
            RiskSessionManager.AutoQueue.ID = autoQueueIdOut.ToLong();
        }

    }

    private void UpdateAutoQueue(string name, string description, bool isActive, string xmlDataAssignment, string xmlDataWorkQueue)
    {
        var paramIns = new FilterParameterCollection();
        var paramOuts = new FilterParameterCollection();
        paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramIns.Add(new FilterParameter("@AutoQueueName", name, DbType.String));
        paramIns.Add(new FilterParameter("@AutoQueueDesc", description, DbType.String));
        paramIns.Add(new FilterParameter("@IsActived", isActive, DbType.Boolean));
        paramIns.Add(new FilterParameter("@XMLAssignment", xmlDataAssignment, DbType.String));
        paramIns.Add(new FilterParameter("@XMLWorkQueue", xmlDataWorkQueue, DbType.String));
        paramIns.Add(new FilterParameter("@AutoQueueID", AutoQueueID, DbType.Int64));

        //44078 - VW - Paysafe - Assignment Processing Status = 'Completed' includes AQ allocation to WQ
        paramIns.Add(new FilterParameter("@IsRequiredBE", uxHdRequiredBe.Value.ToString().ToLower() == "true", DbType.Boolean));

        WebServices.RiskServices.ExecuteNonQueryCommand(SPA_UPDATE_AUTOTQUEUE, paramIns, out paramOuts);
    }

    private void DeleteAutoQueue()
    {
        var paramIns = new FilterParameterCollection();
        var paramOuts = new FilterParameterCollection();
        paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramIns.Add(new FilterParameter("@AutoQueueID", AutoQueueID, DbType.Int64));

        WebServices.RiskServices.ExecuteNonQueryCommand(SPA_DELETE_AUTOQUEUE, paramIns, out paramOuts);
    }

    #endregion

    #region Events

    protected override void OnLoad(EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        btnDelete.Visible = btnDeleteCover.Visible = AutoQueueMode == WebSiteEnums.AutoQueueMode.Edit;
        ChangeNameBtnSubmit();
        ChangeTitle();

        if (!IsPostBack)
        {
            DataBindAssigment();
            DataBindWorkQueue();
            DataBindDetailAutoQueue();
        }

        base.OnLoad(e);
    }

    //Sprint 2 - 44078 - VW - Paysafe - Assignment Processing Status = 'Completed' includes AQ allocation to WQ
    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {

        if (!CheckValidForm())
            return;

        List<UserControls_rm_MCF_CustomListBoxItem> assigments = lbAssigment.SelectedItems;
        List<UserControls_rm_MCF_CustomListBoxItem> workQueue = lbWorkQueue.SelectedItems;

        string name = txtName.Text.Trim();
        string description = txtBriefDescription.Text.Trim();
        bool isActive = rdActive.Checked;
        string xmlDataAssignment = CreateXmlData(assigments, 0);
        string xmlDataWorkQueue = CreateXmlData(workQueue, 1);

        if (AutoQueueMode == WebSiteEnums.AutoQueueMode.Edit)
        {
            UpdateAutoQueue(name, description, isActive, xmlDataAssignment, xmlDataWorkQueue);
        }
        else if (AutoQueueMode == WebSiteEnums.AutoQueueMode.Create)
        {
            CreateAutoQueue(name, description, isActive, xmlDataAssignment, xmlDataWorkQueue);
        }

        AjaxAddResponseScript("setTimeout('mdlCreateEdit.showMessageSuccess()', 200);");
    }

    private bool CheckValidForm()
    {
        bool isValid;
        List<RadListBoxItem> assigmentsBlocked = CheckRequestAssigment();

        if (assigmentsBlocked.Count > 0)
            isValid = false;
        else
            isValid = !string.IsNullOrEmpty(txtName.Text) && lbWorkQueue.SelectedItems.Count > 0;

        isValid &= IsValidForm;
        isValid &= lbAssigment.SelectedItems.Count(x => x.Selectable == false) == 0;

        return isValid;
    }

    protected void btnDelete_OnClick(object sender, EventArgs e)
    {
        List<RadListBoxItem> workQueuesBlocked = CheckRequestWorkQueue();
        if (workQueuesBlocked.Count > 0)
            return;

        DeleteAutoQueue();
        Response.Redirect("rm_MCF_AutoQueue.aspx");
    }

    protected void btnReloadAssignments_OnClick(object sender, EventArgs e)
    {
        DataBindAssigment();
    }

    protected void lbAssigment_OnItemDataBound(UserControls_rm_MCF_CustomListBox sender, UserControls_rm_MCF_CustomListBoxItemEventArgs e)
    {
        var listBoxItem = e.Item;
        var dataItem = listBoxItem.DataItem as DataRowView;

        if (AutoQueueMode == WebSiteEnums.AutoQueueMode.Edit)
        {
            listBoxItem.Selected = dataItem["IsSelected"].ToBoolean();
        }

        var dataIsExpired = dataItem["IsExpired"].ToString().Trim() == "1" ? GetLocalResourceObject("expired").ToString() : string.Empty;
        var dataIsUsed = dataItem["IsUsed"].ToString().Trim() == "1" ? GetLocalResourceObject("anotherAutoQueue").ToString() : string.Empty;
        var message = GetMessage(dataIsExpired, dataIsUsed);
        message = GetOtherMessage(dataItem, dataIsUsed, message);
        if (!string.IsNullOrEmpty(message))
        {
            message = string.Format("({0})", message);
            listBoxItem.TextEx = message;
            listBoxItem.Selectable = true;
        }
    }

    protected void lbWorkQueue_OnItemDataBound(UserControls_rm_MCF_CustomListBox sender, UserControls_rm_MCF_CustomListBoxItemEventArgs e)
    {
        var listBoxItem = e.Item;
        var dataItem = listBoxItem.DataItem as DataRowView;
        var dataIsExcluded = string.Empty;
        var dataIsExpired = dataItem["IsExpired"].ToString().Trim() == "1" ? GetLocalResourceObject("expired").ToString() : string.Empty;

        if (AutoQueueMode == WebSiteEnums.AutoQueueMode.Edit)
        {
            listBoxItem.Selected = dataItem["IsSelected"].ToBoolean();
            dataIsExcluded = dataItem["IsExcludedTemp"].ToBoolean() ? GetLocalResourceObject("excluded").ToString() : string.Empty;
        }

        var message = GetMessage(dataIsExpired, dataIsExcluded);
        if (!string.IsNullOrEmpty(message))
        {
            var msg = string.Format("({0})", message);
            listBoxItem.TextEx = msg;
            listBoxItem.Selectable = true;
        }
    }

    protected void btnCheckNameAvailable_CreateResponseData(ASCommandControl sender, string eventargument)
    {
        List<RadListBoxItem> namesBlocked = CheckRequestUniqueName();
        List<RadListBoxItem> assigmentsBlocked = CheckRequestAssigment();
        string[] assignments = assigmentsBlocked.Select(i => i.Text).ToArray();
        var validAssignment = assignments.Length > 0 ? string.Join(",", assignments).TrimEnd(',') : "";
        var validAssignmentNames = namesBlocked.Count > 0 && txtNameClientState.Value.Trim() != namesBlocked[0].Text ? "True" : "False";

        var str = "{{\"name\":\"{0}\",\"assignment\":\"{1}\"}}";
        if (namesBlocked.Count > 0 || assigmentsBlocked.Count > 0)
            IsValidForm = false;
        else
            IsValidForm = true;
        sender.ResponseString = string.Format(str, validAssignmentNames, validAssignment);
    }

    #endregion

    protected void btnCheckDeleteAvailable_CreateResponseData(ASCommandControl sender, string eventargument)
    {
        List<RadListBoxItem> workQueuesBlocked = CheckRequestWorkQueue();
        string[] workQueues = workQueuesBlocked.Select(i => i.Text).ToArray();
        var validWorkQueues = workQueues.Length > 0 ? string.Join(",", workQueues).TrimEnd(',') : "";
        sender.ResponseString = validWorkQueues;
    }

    private string GetOtherMessage(DataRowView dataItem, string dataIsUsed, string currentMessage)
    {
        if (!string.IsNullOrEmpty(currentMessage))
        {
            return currentMessage;
        }
        var futureStartDate = BusGeneralFuncsLib.GetValueDataRowView(dataItem, ManageAssignmentConstanst.COLUMN_NAME_IS_FUTURE_START_DATE);
        if (string.IsNullOrEmpty(futureStartDate) || futureStartDate == ManageAssignmentConstanst.DB_VALUE_TRUE)
        {
            return string.Empty;
        }
        return GetMessage(GetLocalResourceObject(ManageAssignmentConstanst.COLUMN_NAME_IS_FUTURE_START_DATE).ToString(), dataIsUsed);
    }
}