using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Web.UI.AppCode.General;
using AS.Web.UI.Controls;
using System;
using System.Data;
using Telerik.Web.UI;
using CardTypes = WebSiteEnums.CardTypes;

public partial class UserControls_rm_MCF_AcquirerFraudReport : GlobalUserControl
{    
    private HierarchyFilterValue _reportValue = null;

    protected HierarchyFilterValue SavedReportFilterValue
    {
        get
        {
            return SessionManager.CurrentReportFilter;
        }
        set
        {
            SessionManager.CurrentReportFilter = value;
        }
    }    

    private bool IsSearch = false;   

    enum PostBackAction
    {
        DoSearching,
    }

    public CardTypes CardType { get; set; }

    private string OrderDefault
    {
        get
        {
            return ViewState["OrderDefault"].ToString();
        }
        set
        {
            ViewState["OrderDefault"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SavedReportFilterSyncManager.RestoreBrandFraudReportFilter(new UCBrandFraudReportFilterControl
            {
                UxAcquirerList = uxAcquirerList,
                UxDaily = uxDaily,
                UxDate = uxDate,
                UxEndDate = uxEndDate,
                UxFromDate = uxFromDate,
                UxMonthly = uxMonthly,
                UxRange = uxRange,
            }, CardType, ref _reportValue);

            if (CardType == CardTypes.Visa)
            {
                uxExport.GridTitle = string.Format(GetLocalResourceObject("GridVisaTitle.Text").ToString());
            }
            else
            {
                uxExport.GridTitle = string.Format(GetLocalResourceObject("GridMasterCardTitle.Text").ToString());
            }

            uxExport.GridSubTitle = string.Format("{0} ({1})", string.Format(GetLocalResourceObject("GridSubTitle.Text").ToString()), GetDateTimeSubTitle());
            uxDate.MaxDate = DateTime.Now;
            uxFromDate.MaxDate = DateTime.Now;
            uxEndDate.MaxDate = DateTime.Now;

            OrderDefault = "FraudAmount DESC";
        }
    }

    public void uxReportGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        SavedReportFilterSyncManager.SaveBrandFraudReportFilter(new UCBrandFraudReportFilterControl
        {
            UxDaily = uxDaily,
            UxMonthly = uxMonthly,
            UxRange = uxRange,
            UxDate = uxDate,
            UxEndDate = uxEndDate,
            UxFromDate = uxFromDate,
            UxAcquirerList = uxAcquirerList
        }, UCBrandFraudReportFilterMode.AcquirerFraudReport, ref _reportValue, IsSearch);

        FilterParameterCollection _parames = new FilterParameterCollection();
        _parames.AddLoggedInUserReportingParams(true);        

        if (uxDaily.Checked)
        {
            _parames.Add(new FilterParameter("@ToDate", _reportValue.DateOptionValue.From, DbType.Date));
        }
        else if (uxMonthly.Checked)
        {
            _parames.Add(new FilterParameter("@ToDate", _reportValue.DateOptionValue.From, DbType.Date));
            _parames.Add(new FilterParameter("@DateRange", 30, DbType.Int32));
        }
        else if (uxRange.Checked)
        {
            _parames.Add(new FilterParameter("@FromDate", _reportValue.DateOptionValue.From, DbType.Date));
            _parames.Add(new FilterParameter("@ToDate", _reportValue.DateOptionValue.To, DbType.Date));
        }

        if (!string.IsNullOrEmpty(uxAcquirerList.SelectedValue))
        {
            _parames.Add(new FilterParameter("@AcquirerID", uxAcquirerList.SelectedValue, DbType.String));
            uxExport.GridSubTitle = string.Format("{0} {1} ({2})", string.Format(GetLocalResourceObject("GridAcquirerSubTitle.Text").ToString()), uxAcquirerList.SelectedValue, GetDateTimeSubTitle());
        }
        else
        {
            if (string.IsNullOrEmpty(uxAcquirerList.Text))
            {
                uxExport.GridSubTitle = string.Format("{0} ({1})", string.Format(GetLocalResourceObject("GridSubTitle.Text").ToString()), GetDateTimeSubTitle());
            }
            else
            {
                _parames.Add(new FilterParameter("@AcquirerID", uxAcquirerList.Text, DbType.String));
                uxExport.GridSubTitle = string.Format("{0} {1} ({2})", string.Format(GetLocalResourceObject("GridAcquirerSubTitle.Text").ToString()), uxAcquirerList.Text, GetDateTimeSubTitle());
            }
        }

        string cardTypeCode = CardType == CardTypes.Visa ? "VI" : "MC";
        _parames.Add(new FilterParameter("@CardTypeCode", cardTypeCode, DbType.String));

        if (!string.IsNullOrEmpty(OrderDefault))
        {
            uxReportGrid.AS_SortExpression = OrderDefault;
            uxReportGrid.MasterTableView.SortExpressions.AddSortExpression(OrderDefault);
        }

        uxReportGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices,
                            WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] { "spa_RM_RCF_GetAcquirerFraudReport", ReportServices.ConvertToFilterParamWSArray(_parames) });
    }

    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            dataItem["AcquirerID"].Text = string.Format("<span title=\"{0}\">{1}</span>", dataRow["AcquirerName"], dataRow["AcquirerID"]);            

            string detailQueryString = Page.BuildSecureQueryString(string.Format("ToDate={0}&AcquirerID={1}&MerchantNumber={2}&DateTimeSubTitle={3}", GetToDateValue(), dataRow["AcquirerID"], string.Empty, GetDateTimeSubTitle()));

            string detailPage = "rm_MCF_VisaMerchantFraudDetail_ViewModal.aspx";

            if (CardType == CardTypes.MasterCard)
            {
                detailPage = "rm_MCF_MasterCardMerchantFraudDetail_ViewModal.aspx";
            }

            string detailUrlString = string.Format("{0}?", detailPage) + detailQueryString;            
            dataItem["Detail"].Text = string.Format("<a href=\"#\" style=\"cursor:pointer\" onclick=\"return ShowPopupModal('{0}','auto');\">{1}</a>", detailUrlString, "Details");
        }
    }

    private DateTime GetToDateValue()
    {
        DateTime toDate;
        if (uxDaily.Checked || uxMonthly.Checked)
        {
            toDate = uxDate.SelectedDate.Value;
        }
        else
        {
            toDate = uxEndDate.SelectedDate.Value;
        }

        return toDate;
    }

    protected void uxSearchButton_Click(object sender, EventArgs e)
    {
        IsSearch = true;
        OnPostBackActions(PostBackAction.DoSearching);
        IsSearch = false;
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoSearching:
                {
                    uxReportGrid.Rebind();
                }
                break;
        }
    }

    public void uxAcquirerList_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text.Length >= 1)
        {
            DataTable dt = GeneralFuncsLib.GetAcquirers(CardType, e.Text);
            uxAcquirerList.DataSource = dt;
            uxAcquirerList.DataBind();
        }
        else
        {
            uxAcquirerList.Items.Clear();
        }
    }

    private string GetDateTimeSubTitle()
    {
        if (uxDaily.Checked || uxMonthly.Checked)
        {
            return _reportValue.DateOptionValue.From.ToString("MM/dd/yyyy");
        }
        else if (uxRange.Checked)
        {
            return string.Format("{0} - {1}", _reportValue.DateOptionValue.From.ToString("MM/dd/yyyy"), _reportValue.DateOptionValue.To.ToString("MM/dd/yyyy"));
        }

        return string.Empty;
    }

    protected void uxExporter_NeedExportConfig(object sender, ExportConfig exportConfig)
    {        
        string filename = CardType == CardTypes.Visa ? "VisaAcquirerFraudReport" : "MasterCardAcquirerFraudReport";
        exportConfig.FileName = string.Format("{0}_{1}", filename, DateTime.Now.ToString("yyyy-MM-dd_hhmmss"));
        exportConfig.ReportHeader = string.Format("{0} \n{1}", uxExport.GridTitle, uxExport.GridSubTitle);
        uxReportGrid.Columns.FindByUniqueName("Detail").Visible = false;

    }

    protected void uxReportGrid_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        OrderDefault = string.Empty;
    }
}