/* Create by: Hao Dang
 * Ticket: 43784 Queue Enhancements 
 */
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

public partial class UserControls_RQMerchantSelected : GlobalUserControl
{
    private static int RequeueSessionID
    {
        get
        {
            return RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeueSessionID;
        }
        set { RiskSessionManager.DetectionQueueTemporaryRequeueInfo.RequeueSessionID = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindMerchantSelectedGrid(uxMerchantSelected);
        }
    }

    private void BindMerchantSelectedGrid(object sender)
    {
        var reportType = RiskSessionManager.DetectionQueue.ReportType;
        var assignmentType = RiskSessionManager.DetectionQueue.AssignmentType;

        string spaName = string.Empty;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ReportDate", RiskSessionManager.DetectionQueue.ReportDate, DbType.Date));
        parameters.Add(new FilterParameter("@AssignmentID", RiskSessionManager.DetectionQueue.AssignmentID, DbType.Int32));
        parameters.Add(new FilterParameter("@SessionID", RequeueSessionID, DbType.Int64));
        //StyleID: 0 - Default value, Get for report, 1 - get list merchant selected for requeue
        parameters.Add(new FilterParameter("@StyleID", 1, DbType.Boolean));
        parameters.Add(new FilterParameter("@IsCheckAll", RiskSessionManager.DetectionQueueTemporaryRequeueInfo.IsRequeueAll, DbType.Boolean));

        switch (assignmentType)
        {
            case WebSiteEnums.AssignmentType.AggregateQueue:
                {
                    
                    spaName = "spa_RM_AutoQueue_Get_BarometerReport";
                    break;
                }
            case WebSiteEnums.AssignmentType.DetectionQueueDistinct:
                {
                    
                    spaName = "spa_rm_cs_GetDQDistinctMerchantListReport";
                    break;
                }
            case WebSiteEnums.AssignmentType.DetectionQueue:
            case WebSiteEnums.AssignmentType.WorkQueue:
            case WebSiteEnums.AssignmentType.All:
                {
                    if (reportType == WebSiteEnums.RiskReportType.RainbowReport)
                    {
                        
                        spaName = "spa_rm_cs_GetRainbowReport";
                    }
                    else if (reportType == WebSiteEnums.RiskReportType.FlatReport)
                    {
                        parameters.Add(new FilterParameter("@IsExport", 0, DbType.Boolean));
                        spaName = "spa_rm_cs_GetSecurityReportForDetectionQueue";
                    }
                    else
                    {
                        parameters.AddUserSessionID();
                        parameters.Add(new FilterParameter("@MerchantNumber", RiskSessionManager.currentMerchantNumber, DbType.String));
                        spaName = "spa_rq_cs_GetNextQueueDetails";
                    }
                    break;
                }
        }

        ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
    }

    protected void uxMerchantSelected_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindMerchantSelectedGrid(uxMerchantSelected);
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
        }
    }
}