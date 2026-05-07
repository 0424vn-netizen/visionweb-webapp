using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class UserControls_MgmtReport_MerchantFilter : GlobalUserControl
{

    #region Enums
    enum DataBindAction
    {
        BindMerchantGrid
    }
    #endregion


    private void RemoveFilterMenuItem()
    {
        //show filter menu
        var grids = new RadGrid[] { uxMerchantGrid };
        var removedItems = new string[] { 
            "GreaterThan",
            "LessThan", "GreaterThanOrEqualTo", "LessThanOrEqualTo", "Between", "NotBetween",
            "IsEmpty", "NotIsEmpty", "IsNull", "NotIsNull" 
        };
        foreach (var grid in grids)
        {
            for (int i = 0; i < removedItems.Length; i++)
            {
                var mi = grid.FilterMenu.Items.FindItemByText(removedItems[i]);
                if (mi != null)
                    grid.FilterMenu.Items.Remove(mi);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            uxMerchantGrid.Rebind();
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMerchantGrid:
                FilterParameterCollection _params = new FilterParameterCollection();
                _params.AddLoggedInUserReportingParams();
                _params.Add(new FilterParameter("@HierarchyFilterMode", "MERCHANTNUMBER", DbType.AnsiString));
                _params.Add(new FilterParameter("@HierarchyFilterValue", string.Empty, DbType.AnsiString));
                _params.AddLanguageID();
                string spaName = "spa_GetMerchantList";
                this.uxMerchantGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME,
                    new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(_params) });
                break;
            default:
                break;
        }
    }

    #region ControlEvent
    protected void uxMerchantGrid_OnInit(object sender, EventArgs e)
    {
        RemoveFilterMenuItem();
    }
    protected void uxMerchantGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindMerchantGrid, sender);
    }

    protected void uxMerchantGrid_DataSourceReady(object sender, EventArgs e)
    {
        uxSubmit.Visible = uxMerchantGrid.MasterTableView.VirtualItemCount > 0;
    }


    #endregion
}
