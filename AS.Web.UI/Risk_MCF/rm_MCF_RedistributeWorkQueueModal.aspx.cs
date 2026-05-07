using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Pages;
using AS.Utilities;
using Org.BouncyCastle.Asn1.Cms;
using Telerik.Web.UI;

[PagePermission("RskAutoQueue,MSRskAutoQueue")]
public partial class rm_MCF_RedistributeWorkQueueModal : NonReportPage
{
    #region Constants

    private const string SPA_AUTOQUEUE_LIST = "spa_RM_MCF_AQ_Get_RequestList";

    private const string SPA_ASSIGNMENT_LIST = "spa_RM_MCF_AQ_Get_AssignmentList";

    private const string XML_TEMPLATE_AUTOQUEUE = @"<Redistribution>{0}</Redistribution>";

    private const string SPA_INSERT_REDISTRIBUTE = "spa_RM_MCF_AQ_Insert_Redistribution";

    private const string SPA_CHECK_REDISTRIBUTE = "spa_RM_MCF_AQ_Check_Redistribution";

    private const string XML_TEMPLATE_AUTOQUEUE_ITEM = @"<Assignment><AssignmentID>{0}</AssignmentID><AssignmentTypeID>{1}</AssignmentTypeID></Assignment>";

    #endregion

    #region Properties

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

    protected override void OnLoad(EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            DataBindAutoQueues();

        }
        base.OnLoad(e);
    }

    #endregion

    #region Privates

    private List<RadListBoxItem> CheckRedistributed()
    {
        var names = new List<RadListBoxItem>();
        var autoQueue = lbAutoQueue.SelectedItems.FirstOrDefault();
        var xmlData = CreateXmlData(lbWorkQueueRedis.SelectedItems, 1);

        DataTable dt = GetListRedistributed(autoQueue.Value.ToLong(), xmlData);
        if (dt != null && dt.Rows.Count > 0)
        {
            dt.AsEnumerable().ForEach(r =>
            {
                var itemRow = new RadListBoxItem();
                itemRow.Value = r["AssignmentID"].ToString();
                itemRow.Text = r["AssignmentName"].ToString();
                names.Add(itemRow);
            });
        }
        return names;
    }

    private void DataBindAutoQueues()
    {
        DataTable tb = GetDataAutoQueues();
        lbAutoQueue.DataSource = tb;
        lbAutoQueue.DataBind();
    }

    private void DataBindWorkQueuesRedis(long autoQueueID)
    {
        DataTable tb = GetDataWorkQueuesByAutoQueueID(autoQueueID);
        lbWorkQueueRedis.DataSource = tb;
        lbWorkQueueRedis.DataBind();
    }

    private void DataBindWorkQueuesDest(long autoQueueId)
    {
        DataTable tb = GetDataWorkQueuesByAutoQueueID(autoQueueId);
        lbWorkQueueDes.DataSource = tb;
        lbWorkQueueDes.DataBind();
    }

    private string CreateXmlData(List<UserControls_rm_MCF_CustomListBoxItem> assigments, int type)
    {
        string itemAssigments = string.Empty;
        assigments.ForEach(a => itemAssigments += string.Format(XML_TEMPLATE_AUTOQUEUE_ITEM, a.Value.Trim(), type));

        string xml = string.Format(XML_TEMPLATE_AUTOQUEUE, itemAssigments);
        return xml;
    }

    private bool CheckValidForm()
    {
        var isValid = lbAutoQueue.SelectedItems.Count > 0
            && lbWorkQueueRedis.SelectedItems.Count > 0
            && lbWorkQueueDes.SelectedItems.Count > 0;

        var redistributes = CheckRedistributed().Select(x => x.Value).ToArray();
        if (redistributes.Length > 0)
            isValid = false;
        isValid &= IsValidForm;
        return isValid;
    }

    #endregion

    #region Private SPAs

    private void CreateRedistribute(long autoQueueId, string xmlSource, string xmlSourceDes)
    {
        var paramIns = new FilterParameterCollection();
        var paramOuts = new FilterParameterCollection();
        paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramIns.Add(new FilterParameter("@AutoQueueID", autoQueueId, DbType.Int64));
        paramIns.Add(new FilterParameter("@XMLSourceAssignment", xmlSource, DbType.String));
        paramIns.Add(new FilterParameter("@XMLDestinationAssignment", xmlSourceDes, DbType.String));
        paramIns.Add(new FilterParameter("@ReportDate", DateTime.Now.Date, DbType.Date));
        paramIns.Add(new FilterParameter("@RedistributionID", "", DbType.Int64, true));

        WebServices.RiskServices.ExecuteNonQueryCommand(SPA_INSERT_REDISTRIBUTE, paramIns, out paramOuts);
    }

    private DataTable GetListRedistributed(long autoQueueId, string xml)
    {
        var paramIns = new FilterParameterCollection();
        paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramIns.Add(new FilterParameter("@AutoQueueID", autoQueueId, DbType.Int64));
        paramIns.Add(new FilterParameter("@XMLSourceAssignment", xml, DbType.String));
        DataTable dt = WebServices.RiskServices.GetReports(SPA_CHECK_REDISTRIBUTE, paramIns);

        return dt;
    }

    private DataTable GetDataWorkQueuesByAutoQueueID(long autoQueueId)
    {
        var paramIns = new FilterParameterCollection();
        paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramIns.Add(new FilterParameter("@AssignmentTypeID", 1, DbType.Int32));
        paramIns.Add(new FilterParameter("@IsRedistributeWQ", true, DbType.Boolean));
        paramIns.Add(new FilterParameter("@AutoQueueID", autoQueueId, DbType.Int64));
        DataTable dt = WebServices.RiskServices.GetReports(SPA_ASSIGNMENT_LIST, paramIns);
        return dt;
    }

    private DataTable GetDataAutoQueues()
    {
        var paramIns = new FilterParameterCollection();
        paramIns.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramIns.Add(new FilterParameter("@IsActived", true, DbType.Boolean));

        DataTable dt = WebServices.RiskServices.GetReports(SPA_AUTOQUEUE_LIST, paramIns);
        return dt;
    }

    #endregion

    #region Events

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        if (!CheckValidForm())
            return;
        var autoQueueId = lbAutoQueue.SelectedItems.FirstOrDefault().Value.ToLong();
        var xmlSource = CreateXmlData(lbWorkQueueRedis.SelectedItems, 1);
        var xmlSourceDes = CreateXmlData(lbWorkQueueDes.SelectedItems, 1);

        CreateRedistribute(autoQueueId, xmlSource, xmlSourceDes);
        var requestSecurity = "rm_MCF_WorkQueueRedistributionStatus.aspx?" + BuildSecureQueryString("FromDistribution=1");
        var url = string.Format("parent.ShowPopupModal('{0}','auto');", requestSecurity);
        AjaxAddResponseScript(url);
    }

    protected void lbAutoQueue_OnItemCommand(object sender, string e, bool isCheck)
    {
        lbWorkQueueRedis.Items.Clear();
        lbWorkQueueRedis.DataBind();

        lbWorkQueueDes.Items.Clear();
        lbWorkQueueDes.DataBind();

        if (isCheck)
        {
            long autoQueueId = e.ToLong();
            DataBindWorkQueuesRedis(autoQueueId);
        }
    }

    protected void lbWorkQueueRedis_OnItemCommand(object sender, string e, bool isCheck)
    {
        UserControls_rm_MCF_CustomListBoxItem itemAutoQueue = lbAutoQueue.SelectedItems.FirstOrDefault();
        if (lbWorkQueueRedis.SelectedItems.Any() && itemAutoQueue != null)
        {
            long autoQueueId = itemAutoQueue.Value.ToLong();
            DataBindWorkQueuesDest(autoQueueId);
        }
        else
        {
            lbWorkQueueDes.Items.Clear();
            lbWorkQueueDes.DataBind();
        }
    }

    protected void lbWorkQueueRedis_OnItemDataBound(UserControls_rm_MCF_CustomListBox sender, UserControls_rm_MCF_CustomListBoxItemEventArgs e)
    {
        var listBoxItem = e.Item;
        var dataItem = listBoxItem.DataItem as DataRowView;
        var dataIsRedistributed = dataItem["IsRedistributed"] != null && dataItem["IsRedistributed"].ToString() == "1" ? GetLocalResourceObject("isRedistributed").ToString() : string.Empty;

        if (!string.IsNullOrEmpty(dataIsRedistributed))
        {
            var msg = string.Format(" ({0})", dataIsRedistributed);
            listBoxItem.Text += msg;
            listBoxItem.Selectable = false;
        }
    }

    protected void lbWorkQueueDest_OnItemDataBound(UserControls_rm_MCF_CustomListBox sender, UserControls_rm_MCF_CustomListBoxItemEventArgs e)
    {
        var listBoxItem = e.Item;
        var itemSelected = lbWorkQueueRedis.SelectedItems.FirstOrDefault(x => x.Value.Contains(e.Item.Value));
        var itemRedistributed = lbWorkQueueRedis.Items.FirstOrDefault(x => x.Selectable == false && x.Value.Contains(e.Item.Value));

        if (itemSelected != null)
            listBoxItem.Selectable = false;

        if (itemRedistributed != null)
        {
            var msg = string.Format(" ({0})", GetLocalResourceObject("isRedistributed").ToString());
            listBoxItem.Text += msg;
            listBoxItem.Selectable = false;
        }
    }

    protected void btnCheckRedistributeAvailable_CreateResponseData(ASCommandControl sender, string eventargument)
    {
        var redistributes = CheckRedistributed().Select(x => x.Text).ToArray();
        
        var msg = string.Empty;
        if (redistributes.Length > 0)
        {
            msg = string.Join(",", redistributes);
            IsValidForm = false;
        }
        else
        {
            IsValidForm = true;
        }
           

        sender.ResponseString = msg;
    }

    #endregion
}
