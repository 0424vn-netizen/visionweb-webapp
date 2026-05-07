using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Controls.Pages;
using AS.Common.DBManager;

[PagePermission("RskQueue,MSRskQueue")]
public partial class rm_MCF_DQAssignWorkQueueModal : NonReportPage
{
    #region Enums

    enum DataBindAction
    {
        WQAssignmentList,
    }

    #endregion Enums

    #region Constants

    private const string WQ_ASSIGN_ID = "AssignmentID";
    private const string WQ_ASSIGN_NAME = "AssignmentName";
    private const string SPA_GET_WQ_ASSIGNMENT = "spa_RM_MCF_wq_GetWorkQueueAssignmentList";
    private const string SPA_PROCESS_REQUEUED_MER = "spa_RM_MCF_Requeue_Merchant";

    #endregion Constants

    #region Properties

    protected int MerchantCount
    {
        get
        {
            return RiskSessionManager.DetectionQueueTemporaryRequeueInfo.CurrentRequeuedMerchants;
        }
    }

    #endregion Properties

    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;

        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.WQAssignmentList);
            BindMerchantCount();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.WQAssignmentList:
                DataBindWQAssignment();
                break;
        }
    }

    protected void btnRequeue_Click(object sender, EventArgs e)
    {
        var parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.Add(new FilterParameter(
            "@SessionID",
            RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeueSessionID,
            DbType.Int32));
        parameterList.Add(new FilterParameter("@IsAdded",
            !uxCbkRemoveRequeue.Checked,
            DbType.Boolean));
        if (uxComboWorkQueueList.Enabled)
        {
            parameterList.Add(new FilterParameter(
                "@WorkQueueAssignmentID",
                uxComboWorkQueueList.SelectedValue,
                DbType.Int32));

        }
        var outValue = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand(
            SPA_PROCESS_REQUEUED_MER, parameterList, out outValue);

        AjaxAddResponseScript("parent.RebindAndShowStausWhenCloseModal();");
    }

    protected void uxCbkRemoveRequeue_CheckedChanged(object sender, EventArgs e)
    {
        uxComboWorkQueueList.Enabled = !uxCbkRemoveRequeue.Checked;
    }

    private void DataBindWQAssignment()
    {
        uxComboWorkQueueList.DataTextField = WQ_ASSIGN_NAME;
        uxComboWorkQueueList.DataValueField = WQ_ASSIGN_ID;

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@IsNotExpire", 1, DbType.Boolean));
        DataTable assignTable = WebServices.RiskServices.GetReports(SPA_GET_WQ_ASSIGNMENT, parameters);
        if (assignTable.Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "HasNoWorkQueue",
                string.Format("alert('{0}'); parent.HidePopupModal();", Resources.RiskMessageManager.Risk_HasNoWorkQueueAs),
                true);
        }
        else
        {
            uxComboWorkQueueList.DataSource = assignTable;
            uxComboWorkQueueList.DataBind();
        }
    }

    private void BindMerchantCount()
    {
        uxMerchantCount.Text = MerchantCount.ToString();
    }

    #endregion Methods
}
