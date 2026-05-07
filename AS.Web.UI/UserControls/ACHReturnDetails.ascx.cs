using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ACHReturnDetails : GlobalUserControl
{
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindACHReturnDateRange();
            uxACHReturnDetailGrid.Rebind();
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

        RadComboBoxItem item = items.Find(i => i.Value == GeneralFuncsLib.GetDefaultDaysOfACHReturnDetail());
        item.Selected = true;
    }

    protected void uxACHReturnDetailRange_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RiskSessionManager.ACHReturnDateRangeModal = int.Parse(uxACHReturnDetailRange.SelectedValue);
        uxACHReturnDetailGrid.Rebind();
    }


    protected void uxACHReturnDetailGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        bool isShow = false;
        isShow = GeneralFuncsLib.CheckCSViewFullCard(this.Page) ? true : false;
        switchDisplaySensitiveData(isShow);

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserRiskParams();
        parameters.AddDecryptDataParams("RoutingNumber,DDANumber");
        parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@DateRange", RiskSessionManager.ACHReturnDateRangeModal, DbType.Int32));
        uxACHReturnDetailGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices,
                    WebSiteConstants.GET_REPORT_METHOD_NAME,
                    new object[] { "spa_rm_cs_RiskReport_GetACHHistory", 
                ReportServices.ConvertToFilterParamWSArray(parameters) });
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
}