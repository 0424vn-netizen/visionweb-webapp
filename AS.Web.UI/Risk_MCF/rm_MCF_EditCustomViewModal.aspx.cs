using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using Newtonsoft.Json;
using System.Linq;

public partial class rm_MCF_EditCustomViewModal : NonReportPage
{
    private string MerchantNumber { get; set; }
    private IEnumerable<ColumnDisplayedConfigurationItem> ColumnConfiguration { get; set; }

    private string _customViewID
    {
        get
        {
            if (SecureQueryString["CustomViewID"] != null) return SecureQueryString["CustomViewID"].ToString();
            return string.Empty;
        }
    }

    public int ViewType { get; set; }

    public string PageMode
    {
        get
        {
            if (Page.SecureQueryString["PageMode"].ToString() != null)
            {
                return Page.SecureQueryString["PageMode"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            GetData();
        }
    }


    public void GetData()
    {
        ColumnConfiguration = PageMode == "TransactionHistory" ? RiskSessionManager.RiskReportTransactionHistoryColumn : RiskSessionManager.RiskReportTransactionVolumeColumn;
        GetDisplayedColumn();
    }

    private void GetDisplayedColumn()
    {
        var displayColumns = GenerateTransactionVolumeAnalysis();

        var data = new List<ColumnDisplayedConfigurationItem>();

        if (ColumnConfiguration != null)
        {
            foreach (string item in displayColumns)
            {            
                var col = ColumnConfiguration.FirstOrDefault(a => a.ColumnName == item);
                if (col != null)
                {
                    col.ColumnText = col.ColumnText.ToCurrencySymbol();
                    data.Add(col);
                }
            }
        }

        data = GetDefaultFullViewOrderby(data);

        uxRightGrid.DataSource = data;
        uxRightGrid.DataBind();

    }

    private List<ColumnDisplayedConfigurationItem> GetDefaultFullViewOrderby(List<ColumnDisplayedConfigurationItem> displayedColumns) 
    {
        var config = GeneralFuncsLib.ReadJsonConfig<FullViewConfig>("App_Data/FullViewConfig/FullViewConfig.json");
        if (config.OrderByAscInitial)
        {
            //Turn off Initital
            config.OrderByAscInitial = false;
            GeneralFuncsLib.WriteObjectToJsonFile(config, "App_Data/FullViewConfig/FullViewConfig.json");

            //Save default fullview
            displayedColumns = displayedColumns.OrderBy(x => x.ColumnText).ToList();
            var newOrder = displayedColumns.Select(x => x.ColumnName).ToList();            
            UpdateCustomView(newOrder);
        }
        return displayedColumns;
    }


    public void RebindData()
    {
        uxRightGrid.DataBind();
    }

    private DataTable GetCustomDisplayedColumns()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@CustomViewID", _customViewID, DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_Get_CustomView", parameters);
    }

    private List<string> GenerateTransactionVolumeAnalysis()
    {
        DataTable tb = GetCustomDisplayedColumns();
        if (tb.Rows.Count > 0)
        {
            string listColumns = tb.Rows[0]["ViewData"].ToString();
            txtViewName.Value = tb.Rows[0]["ViewName"].ToString();
            ViewType = tb.Rows[0]["ViewType"].ToInt();
            string[] columns = listColumns.Split(',');
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = columns[i].Trim();
            }
            return columns.ToList<string>();
        }
        else return new List<string>();
    }


    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        var displayedItems = uxRightGrid.Items.Cast<RadListBoxItem>().Select(x => x.Value).ToList();
        UpdateCustomView(displayedItems);

        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("window.EditColumnModal.CloseEditCustomViewModal();");
    }

    private void UpdateCustomView(List<string> displayedItems)
    {
        string viewData = string.Join(",", displayedItems);

        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@CustomViewID", _customViewID, DbType.Int32));
        parameters.Add(new FilterParameter("@ViewName", txtViewName.Value, DbType.String));
        parameters.Add(new FilterParameter("@ViewType", ViewType, DbType.Int32));
        parameters.Add(new FilterParameter("@ViewData", viewData, DbType.String));
        WebServices.RiskServices.GetReports("spa_RM_MCF_UpdateCustomView", parameters);
    }
}