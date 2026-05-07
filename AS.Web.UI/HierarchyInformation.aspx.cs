using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using Telerik.Web.UI;

[PagePermission("HierarchyInfo")]
public partial class HierarchyInformation : ReportPage
{
    enum DataBindAction
    {
        BindGridDrillDown,
        BindGridMerchant
    }
    #region Properties
    private string _GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter);
        }
    }
    private string _GridDrillDownHeader
    {
        get
        {
            return GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
        }
    }
    #endregion

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExportTop");
        base.PageInitialize();
    }
    protected bool CheckMerchantMode()
    {
        if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode)
            || ReportFilter.CurrentValue.HierarchyMode == "LAST6MERCHNUMBER"
             || ReportFilter.CurrentValue.HierarchyMode == "PARTIALMERCHNUMBER"
            || (!ReportFilter.CurrentValue.Value.IsNullOrEmpty() && ReportFilter.CurrentValue.Value != SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", ReportFilter.CurrentValue.HierarchyMode)["HierarchyPrefix"].ToString()
            && (ReportFilter.CurrentValue.HierarchyMode == "MERCHANTNAME" || ReportFilter.CurrentValue.HierarchyMode == "CHAIN"
            || ReportFilter.CurrentValue.HierarchyMode == "GROUP")))
            return true;
        else
            return false;
    }
    protected override void DoSwitchView()
    {
        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader);
        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderTooltip = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader);
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderText = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader + " " + GetLocalResourceObject("HierarchyInformation_aspx_cs_Name").ToString());
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderTooltip = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader + " " + GetLocalResourceObject("HierarchyInformation_aspx_cs_Name").ToString());
        GridNotShow_All();
        if (CheckMerchantMode())
        {
            GridShow_Merchant();
        }
        else
        {
            GridShow_Hierarchy();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        IsBindDataOnLoad = true;
    }
    private void GridNotShow_All()
    {
        for (int i = 0; i < uxDrilldownGrid.Columns.Count; i++)
        {
            uxDrilldownGrid.Columns[i].Visible = false;
        }
    }

    private void GridShow_Hierarchy()
    {
        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("Contact").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("OptedIn").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("OptedOut").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("Closed").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("Status").Visible = true;
    }

    private void GridShow_Merchant()
    {
        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("MerchantName").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("Chain").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("MerchantStatus").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("OpenDate").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("ClosedDate").Visible = true;
        uxDrilldownGrid.Columns.FindByUniqueName("OnlineStatus").Visible = true;
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
            OnDataBindControls(DataBindAction.BindGridDrillDown, sender);
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddHierarchyFilterParams(this);
        parameters.AddLoggedInUserReportingParams();

        string spaName = string.Empty;

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGridDrillDown:
                {
                    if (!CheckMerchantMode())
                    {
                        parameters.Add("@ReportType", "HierarchyInfo", DbType.String);
                        spaName = WebSiteConstants.GET_REPORT_SPA_NAME;
                        (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                        uxExportTop.GridTitle = GetLocalResourceObject("HierarchyInformation_aspx_cs_HierarchyList").ToString() + " - ";
                    }
                    else
                    {
                        parameters.Add("@ReportType", "HierarchyInfoDetail", DbType.String);
                        spaName = WebSiteConstants.GET_REPORT_SPA_NAME;
                        (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                        uxExportTop.GridTitle = GetLocalResourceObject("HierarchyInformation_aspx_cs_MerchantList").ToString() + " - ";
                    }
                    uxExportTop.GridSubTitle = _GridTitle;
                    uxExportTop.GridHeader = uxExportTop.GridTitle + uxExportTop.GridSubTitle;
                }
                break;
        }
    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
        {
            if (e.Item is GridDataItem)
            {
                if (CheckMerchantMode())
                {
                    const string MERCHANT_NUMBER = "Entity";
                    GridDataItem dataItem = e.Item as GridDataItem;
                    DataRowView rowView = e.Item.DataItem as DataRowView;
                    string queryString = BuildSecureQueryString(string.Format("MerchantNumber={0}", rowView[MERCHANT_NUMBER]));
                    string url = string.Format("<a href='MerchantProfile.aspx?{0}'>{1}</a>", queryString, rowView[MERCHANT_NUMBER]);
                    dataItem["DrilldownColumn"].Text = VeraCodeSolution.GetOutputHtmlString(url);
                }
            }
        }
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(((AS.Controls.UserControls.UxExport)sender).GridHeader.ToString());
    }
}

