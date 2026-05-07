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
using AS.Controls.Pages;
using Telerik.Web.UI;

public partial class ViewChangeLog : ReportPage
{
    enum DataBindAction
    {
        BindReportGrid,
    }

    public int HierarchyId
    {
        get
        {
            if (SecureQueryString.IsNotNullData())
                return SecureQueryString["hierarchyId"].ToInt();
            return 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, uxReportGrid);
            uxHdUrlViewDetail.Value = BuildUrlViewDetails();
            btnViewDetails.Visible = IsUserWithPermission("UserAuditReport") || IsUserWithPermission("MSUserAuditReport");
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    FilterParameterCollection parames = new FilterParameterCollection();
                    parames.AddLoggedInUserReportingParams();
                    parames.Add(new FilterParameter("@HierarchyID", HierarchyId, DbType.Int32));
                    //((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_SEC_GetChangeLogForHierarchy", ReportServices.ConvertToFilterParamWSArray(parames) });
                    
                    //46568 Change Log modal missing data 
                    //Use security service instead of report service to get log data. This is temporary fixing.
                    ((ASGrid)sender).DataSource = WebServices.SecurityServices.GetReports("spa_SEC_GetChangeLogForHierarchy", parames);
                    break;
                }
        }
    }

    protected override void DoItemDataBound(ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            DateTime reportDate = new DateTime();
            DateTime.TryParse(dataRow["ReportDate"].ToString(), out reportDate);
            dataItem["ReportDate"].Text = reportDate.ToString("MM/dd/yyyy - hh:mm tt");
        }
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    private string BuildUrlViewDetails()
    {
        string queryString = BuildSecureQueryString(string.Format("hierarchyId={0}", HierarchyId));
        return string.Format("{0}?{1}", ResolveUrl("~") + "UserAuditReport.aspx", queryString);
    }
}