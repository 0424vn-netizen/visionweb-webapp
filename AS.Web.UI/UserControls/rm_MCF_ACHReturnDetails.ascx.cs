using AS.Common.DBManager;
using AS.Common.Logger;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

public partial class UserControls_rm_MCF_ACHReturnDetails : GlobalUserControl
{
    private const string SPA_GET_ACH_HISTORY = "spa_RM_MCF_RiskReport_GetACHHistory";
    enum DataBindAction
    {
        BindACHReturnDetailGrid
    }
    public string MerchantNumber
    {
        get
        {
            if (ViewState["MerchantNum"] != null) return ViewState["MerchantNum"].ToString();
            else return string.Empty;
        }
        set { ViewState["MerchantNum"] = value; }
    }
    public DateTime ReportDate
    {
        get
        {
            if (ViewState["ReportDate"] != null) return DateTime.Parse(ViewState["ReportDate"].ToString());
            else return DateTime.Now.Date;
        }
        set { ViewState["ReportDate"] = value; }
    }
    private DataTable dtACHReturnDetailGrid = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                BindACHReturnDateRange();
                uxACHReturnDetailGrid.Rebind();
            }
        }
        catch (ViewStateException)
        {
            LoggerManager.Error(string.Format("UserControls_rm_MCF_ACHReturnDetails - ViewStateException - Request={0}; SessionID={1}", Context.Request.Url, Session.SessionID));
            throw;
        }
    }

    public void GetData()
    {
        uxACHReturnDetailGrid.Rebind();
    }

    private void BindACHReturnDateRange()
    {
        List<RadComboBoxItem> items = new List<RadComboBoxItem>(){
            new RadComboBoxItem("1", "1"),
            new RadComboBoxItem("2", "2"),
            new RadComboBoxItem("3", "3"),
            new RadComboBoxItem("5", "5"),
            new RadComboBoxItem("7", "7"),
            new RadComboBoxItem("14", "14"),
            new RadComboBoxItem("30", "30"),
            new RadComboBoxItem("60", "60"),
            new RadComboBoxItem("90", "90")
        };
        uxACHReturnDetailRange.Items.AddRange(items);

        string defaultDays = RiskSessionManager.ACHReturnDateRangeModal.ToString();
        RadComboBoxItem item = items.Find(i => i.Value == defaultDays);
        item.Selected = true;
    }

    protected void uxACHReturnDetailRange_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RiskSessionManager.ACHReturnDateRangeModal = int.Parse(uxACHReturnDetailRange.SelectedValue);
        uxACHReturnDetailGrid.Rebind();
    }


    protected void uxACHReturnDetailGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (string.IsNullOrEmpty(this.MerchantNumber))
        {
            uxACHReturnDetailGrid.DataSource = new DataTable();
            return;
        }
        bool isShow = false;
        isShow = GeneralFuncsLib.CheckCSViewFullCard(this.Page) ? true : false;
        switchDisplaySensitiveData(isShow);
        if (dtACHReturnDetailGrid != null)
        {
            if (dtACHReturnDetailGrid.Rows.Count > 0 && dtACHReturnDetailGrid.Columns.Contains("TotalRows"))
                uxACHReturnDetailGrid.VirtualItemCount = Convert.ToInt32(dtACHReturnDetailGrid.Rows[0]["TotalRows"].ToString());
            uxACHReturnDetailGrid.MasterTableView.AllowCustomPaging = true;
            uxACHReturnDetailGrid.DataSource = dtACHReturnDetailGrid;
        }
        else
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserRiskParams();
            parameters.AddDecryptDataParams("RoutingNumber,DDANumber");
            parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
            parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
            parameters.Add(new FilterParameter("@DateRange", RiskSessionManager.ACHReturnDateRangeModal, DbType.Int32));
            uxACHReturnDetailGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] { SPA_GET_ACH_HISTORY,
                ReportServices.ConvertToFilterParamWSArray(parameters) });
        }
    }

    protected void switchDisplaySensitiveData(bool isShow)
    {
        uxACHReturnDetailGrid.Columns.FindByUniqueName("RoutingNumber").Visible = isShow;
        uxACHReturnDetailGrid.Columns.FindByUniqueName("PartialRoutingNumber").Visible = !isShow;
        uxACHReturnDetailGrid.Columns.FindByUniqueName("DDANumber").Visible = isShow;
        uxACHReturnDetailGrid.Columns.FindByUniqueName("PartialDDANumber").Visible = !isShow;
    }

    protected void uxExportACHReturnDetail_NeedExportConfig(object sender, ExportConfig exportConfig)
    {
        switchDisplaySensitiveData(false);

        string fileName = GeneralFuncsLib.FormatFileName(exportConfig.FileName);
        exportConfig.FileName = HttpUtility.UrlEncode(fileName);
        exportConfig.ReportHeader = GetLocalResourceObject("uxACHReturnDetailResource.GridTitle").ToString();
    }
    private FilterParameterCollection GetAchReturnDetailsParameters(DataBindAction action)
    {
        //DataBindAction actiontemp = (DataBindAction)action; 
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserRiskParams();
        switch (action)
        {
            case DataBindAction.BindACHReturnDetailGrid:
                {
                    parameters.AddDecryptDataParams("RoutingNumber,DDANumber");
                    parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                    parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
                    parameters.Add(new FilterParameter("@DateRange", RiskSessionManager.ACHReturnDateRangeModal, DbType.Int32));
                    parameters.Add(new FilterParameter("@PageNo", 1, DbType.Int32));
                    parameters.Add(new FilterParameter("@PageSize", 10, DbType.Int32));
                    parameters.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
                    //spa_RM_MCF_RiskReport_GetACHHistory
                    break;
                }

        }
        return parameters;
    }
    public void BindDataFromThread(List<DataSourceParallelResponse> dataSources)
    {
        if (dataSources.IsNotNullData())
        {
            var data = dataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindACHReturnDetailGrid.ToString());
            if (data.IsNotNullData())
                dtACHReturnDetailGrid = data.DataSource;
        }
        GetData();
    }

    #region SPA INFO
    public SpaInfo SpaGetACHHistory
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindACHReturnDetailGrid.ToString(),
                SpaName = SPA_GET_ACH_HISTORY,
                Parameters = GetAchReturnDetailsParameters(DataBindAction.BindACHReturnDetailGrid)
            };
        }
    }
    #endregion
}