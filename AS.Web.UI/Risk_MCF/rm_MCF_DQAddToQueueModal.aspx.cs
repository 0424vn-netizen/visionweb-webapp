
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class rm_MCF_DQAddToQueueModal : ReportPage
{
    private int _RequeueSessionID
    {
        get
        {
            if (_PageMode == WebSiteEnums.PAGE_CODE.DQ)
            {
                return RiskSessionManager.MCF_DQTemporaryBarometerReport.RequeueSessionID;
            }
            else if (_PageMode == WebSiteEnums.PAGE_CODE.SR)
            {
                return RiskSessionManager.MCF_DQTemporarySecurityReport.RequeueSessionID;
            }
            else if (_PageMode == WebSiteEnums.PAGE_CODE.NQ)
            {
                return RiskSessionManager.MCF_DQTemporaryNextQueue.RequeueSessionID;
            }
            else
            {
                return -1;
            }
        }
    }

    private int _AssignmentID
    {
        get
        {
            if (!string.IsNullOrEmpty(this.SecureQueryString["AssignmentID"]))
            {
                return Convert.ToInt32(this.SecureQueryString["AssignmentID"]);
            }
            else
            {
                return -1;
            }
        }
    }

    private DateTime _ReportDate
    {
        get
        {

            if (!string.IsNullOrEmpty(this.SecureQueryString["ReportDate"]))
            {
                return Convert.ToDateTime(this.SecureQueryString["ReportDate"]);
            }
            else
            {
                return DateTime.Now;
            }
        }
    }

    private string _ApplyFilterId
    {
        get
        {

            if (!string.IsNullOrEmpty(this.SecureQueryString["ApplyFilterId"]))
            {
                return Convert.ToString(this.SecureQueryString["ApplyFilterId"]);
            }
            else
            {
                return string.Empty;
            }
        }
    }

    private bool _IsAddQueue
    {
        get
        {
            if (!string.IsNullOrEmpty(this.SecureQueryString["IsAddToQueue"]))
            {
                return this.SecureQueryString["IsAddToQueue"].ToBoolean();
            }
            else
            {
                return false;
            }
        }
    }

    private WebSiteEnums.MCF_MerchantWorkingStatus _FilterWorkingStatus
    {
        get
        {
            var data = this.SecureQueryString["FilterWorkingStatus"];
            if (!string.IsNullOrEmpty(data))
            {
                return (WebSiteEnums.MCF_MerchantWorkingStatus)Enum.Parse(typeof(WebSiteEnums.MCF_MerchantWorkingStatus), data);
            }
            else
            {
                return WebSiteEnums.MCF_MerchantWorkingStatus.All;
            }
        }
    }

    private WebSiteEnums.PAGE_CODE _PageMode
    {
        get
        {
            var data = this.SecureQueryString["PageMode"];
            if (!string.IsNullOrEmpty(data))
            {
                return (WebSiteEnums.PAGE_CODE)Enum.Parse(typeof(WebSiteEnums.PAGE_CODE), data);
            }
            else
            {
                return WebSiteEnums.PAGE_CODE.DQ;
            }
        }
    }

    private bool _IsRequeueAll
    {
        get
        {
            if (_PageMode == WebSiteEnums.PAGE_CODE.DQ)
            {
                return RiskSessionManager.MCF_DQTemporaryBarometerReport.IsRequeueAll;
            }
            else if (_PageMode == WebSiteEnums.PAGE_CODE.SR)
            {
                return RiskSessionManager.MCF_DQTemporarySecurityReport.IsRequeueAll;
            }
            else
            {
                return false;
            }
        }
    }

    private int TotalMerchantSelect { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            if (_IsAddQueue)
                DataBindWQAssignment();
        }

        Page.Title = _IsAddQueue ? GetLocalResourceObject("AddToWQTitle").ToString() : GetLocalResourceObject("RemoveToWQTitle").ToString();
        workQueueList.Visible = _IsAddQueue;

        btnRequeue.Text = _PageMode == WebSiteEnums.PAGE_CODE.NQ ? GetLocalResourceObject("btnRequeueAndNextResource1.Text").ToString() :
            btnRequeue.Text = GetLocalResourceObject("btnRequeueResource1.Text").ToString();
    }

    protected void btnRequeue_Click(object sender, EventArgs e)
    {
        var parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.Add(new FilterParameter("@SessionID", _RequeueSessionID, DbType.Int64));
        parameterList.Add(new FilterParameter("@IsAdded", _IsAddQueue, DbType.Boolean));
        parameterList.Add(new FilterParameter("@ViewCode", _PageMode.ToString(), DbType.String));
        if (_IsAddQueue)
            parameterList.Add(new FilterParameter("@WorkQueueAssignmentID", uxComboWorkQueueList.SelectedValue, DbType.Int64));
        var outValue = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_Requeue_Merchant", parameterList, out outValue);

        CreateTemporaryWorkQueueSession();

        RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalRequeueMerchants = uxMerchantSelected.MasterTableView.VirtualItemCount;

        if (_PageMode == WebSiteEnums.PAGE_CODE.NQ)
        {
            AjaxAddResponseScript("parent.DoDismissAndNext(); ClosePopupModal();");
        }
        else
        {
            AjaxAddResponseScript("parent.RebindAndShowStausWhenCloseModal();");
        }
    }

    private void DataBindWQAssignment()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@IsNotExpire", 1, DbType.Boolean));
        DataTable assignTable = WebServices.RiskServices.GetReports("spa_RM_MCF_wq_GetWorkQueueAssignmentList", parameters);
        if (!assignTable.HasData())
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
            uxComboWorkQueueList.DataTextField = "AssignmentName";
            uxComboWorkQueueList.DataValueField = "AssignmentID";
            uxComboWorkQueueList.DataBind();
        }
    }

    private void BindMerchantSelectedGrid(object sender)
    {
        string spaName = "spa_RM_MCF_Get_RequeuedMerchant";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ReportDate", _ReportDate, DbType.Date));
        parameters.Add(new FilterParameter("@AssignmentID", _AssignmentID, DbType.Int64));
        parameters.Add(new FilterParameter("@SessionID", _RequeueSessionID, DbType.Int64));
        parameters.Add(new FilterParameter("@StyleID", 1, DbType.Int64));
        parameters.Add(new FilterParameter("@IsCheckAll", _IsRequeueAll, DbType.Boolean));
        parameters.Add(new FilterParameter("@Mode", (int)_FilterWorkingStatus, DbType.Int64));
        parameters.Add(new FilterParameter("@IsAdded", _IsAddQueue ? 1 : 0, DbType.Int16));

        ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
    }

    protected void uxMerchantSelected_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindMerchantSelectedGrid(sender);
    }
    
    protected void uxMerchantSelected_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            if (dataRow.IsNotNullData() && dataRow["CurrentWQ"].IsNullOrEmpty())
            {
                item["CurrentWQ"].Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
            }
            TotalMerchantSelect = int.Parse(dataRow["TotalMerchantSelect"].ToString());
        }
    }

    private void CreateTemporaryWorkQueueSession()
    {
        RM_MCF_GeneralFuncsLib.CreateTemporaryWorkQueueSession(_ReportDate, _AssignmentID, _ApplyFilterId, _PageMode);
    }

    private bool CheckExistsTemporaryAssignmentStaging()
    {
        FilterParameterCollection parameterList = new FilterParameterCollection();
        FilterParameterCollection outParameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.Add(new FilterParameter("@ReportDate", _ReportDate, DbType.DateTime));
        parameterList.Add(new FilterParameter("@AssignmentID", _AssignmentID, DbType.Int64));
        parameterList.Add(new FilterParameter("@SessionID", _RequeueSessionID, DbType.Int32));
        parameterList.Add(new FilterParameter("@IsExists", 0, DbType.Int32, true));
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_wq_CheckExistsTemporaryAssignmentStaging", parameterList, out outParameterList);
        return outParameterList.FindFilterParameterByName("@IsExists", true).ParameterValue.ToString() == "1";
    }

    protected void uxMerchantSelected_PreRender(object sender, EventArgs e)
    {
        int totalRows = int.Parse(uxMerchantSelected.AS_TotalRecords.ToString());
        if (_IsAddQueue)
        {
            lblMerchantSelected.Text = string.Format(GetLocalResourceObject("MerchantSelectedResource").ToString(), totalRows);
        }
        else
        {
            lblMerchantSelected.Text = string.Format(GetLocalResourceObject("lblNotInWQ").ToString(), TotalMerchantSelect - totalRows, TotalMerchantSelect);
        }
    }
}