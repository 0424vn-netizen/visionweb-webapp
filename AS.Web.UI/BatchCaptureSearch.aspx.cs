using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Controls.Pages;
using System.Text.RegularExpressions;

[PagePermission("TCBatchCapture,TCFlatBatchCapture")]
public partial class BatchCaptureSearch : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindReportGrid,
    }
    enum PostBackAction
    {
        DoSearching,
    }
    #endregion
    #region Properties
    
    private string _MerchantNumber
    {
        get { return (string)ViewState["_MerchantNumber"]; }
        set { ViewState["_MerchantNumber"] = value; }
    }
    private string _MerchantName
    {
        get { return (string)ViewState["_MerchantName"]; }
        set { ViewState["_MerchantName"] = value; }
    }
    private string _TerminalNumber
    {
        get { return (string)ViewState["_TerminalNumber"]; }
        set { ViewState["_TerminalNumber"] = value; }
    }
    private string _BatchNumber
    {
        get { return (string)ViewState["_BatchNumber"]; }
        set { ViewState["_BatchNumber"] = value; }
    }
    private decimal? _BatchTotal
    {
        get { return (decimal?)ViewState["_BatchTotal"]; }
        set { ViewState["_BatchTotal"] = value; }
    }
    private string _CardType
    {
        get { return (string)ViewState["_CardType"]; }
        set { ViewState["_CardType"] = value; }
    }
    private DateTime _BeginDate
    {
        get
        {
            if (ViewState["_BeginDate"] != null)
            {
                return DateTime.Parse(ViewState["_BeginDate"].ToString());
            }
            else
            {
                return DateTime.Now;
            }
        }
        set
        {
            ViewState["_BeginDate"] = value;
        }
    }
    private DateTime _EndDate
    {
        get
        {
            if (ViewState["_EndDate"] != null)
            {
                return DateTime.Parse(ViewState["_EndDate"].ToString());
            }
            else
            {
                return DateTime.Now.AddDays(-7);
            }
        }
        set { ViewState["_EndDate"] = value; }
    }
    #endregion
    const string BATCH_DETAIL = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return ShowPopupModal('{0}','max');\">{1}</a>";
    
    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterBottom");
        base.PageInitialize();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        
        if (!IsPostBack)
        {
            SetDefaultDate();
            ProcessParams();
        }
        uxFromDate.MaxDate = uxEndDate.MaxDate = DateTime.Now;
    }

    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExporter.GridHeader);
        exportConfig.ReportHeader = uxExporter.GridHeader;
    }

    protected override void DoGridNeedDataSource(ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected override void DoItemDataBound(ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            string batchUrl = "BatchCaptureDetailModal.aspx?{0}";
            batchUrl = string.Format(batchUrl, BuildSecureQueryString("BatchNumber=" + dataRow["BatchNumber"].ToString() + "&MerchantNumber=" + dataRow["MerchantNumber"].ToString() +"&ReportDate=" + dataRow["ReportDate"].ToString() + BatchIntruderQuery));
            dataItem["BatchNumber"].Text = VeraCodeSolution.DoVeraCode(string.Format(BATCH_DETAIL, batchUrl, dataRow["BatchNumber"].ToString()));

        }
    }

    private void ProcessParams()
    {
        _BeginDate = uxFromDate.SelectedDate != null ? uxFromDate.SelectedDate.Value : DateTime.Now.AddDays(-7);
        _EndDate = uxEndDate.SelectedDate != null ? uxEndDate.SelectedDate.Value : DateTime.Now;
        _MerchantNumber = VeraCodeSolution.ValidateResponseData(uxRadMerchantNumber.Checked ? uxFilterValue.Text.Trim() != string.Empty ? uxFilterValue.Text.Trim() : null : null);
        _MerchantName = VeraCodeSolution.DoVeraCode(uxRadMerchantName.Checked ? uxFilterValue.Text.Trim() != string.Empty ? "%" + uxFilterValue.Text.Trim() + "%" : null : null);
        _TerminalNumber = VeraCodeSolution.ValidateResponseData(uxTerminalNumber.Text.Trim() != string.Empty ? uxTerminalNumber.Text.Trim() : null);
        _BatchNumber = VeraCodeSolution.ValidateResponseData(uxBatchNumber.Text.Trim() != string.Empty ? uxBatchNumber.Text.Trim() : null);
        _CardType = VeraCodeSolution.ValidateResponseData(uxCardType.Text.Trim() != string.Empty ? uxCardType.Text.Trim() : null);

        if (uxBatchTotal.Text != string.Empty)
        {
            _BatchTotal = Convert.ToDecimal(uxBatchTotal.Value);
        }
        else
        {
            _BatchTotal = null;
        }
    }

    private void SetDefaultDate()
    {
        uxRange.Checked = true;
        uxEndDate.SelectedDate = DateTime.Now;
        uxFromDate.SelectedDate = DateTime.Now.AddDays(-7);
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoSearching);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    if (IsIntruderDetected) return;
                    FilterParameterCollection _params = new FilterParameterCollection();
                    _params.AddLoggedInUserParams(0);
                    _params.Add(new FilterParameter("@BeginDate", _BeginDate, DbType.Date));
                    _params.Add(new FilterParameter("@EndDate", _EndDate, DbType.Date));
                    _params.Add(new FilterParameter("@MerchantNumber", _MerchantNumber, DbType.AnsiString));
                    _params.Add(new FilterParameter("@MerchantName", _MerchantName, DbType.AnsiString));
                    _params.Add(new FilterParameter("@TerminalNumber", _TerminalNumber, DbType.AnsiString));
                    _params.Add(new FilterParameter("@BatchNumber", _BatchNumber, DbType.AnsiString));
                    _params.Add(new FilterParameter("@BatchTotal", _BatchTotal, DbType.Decimal));
                    _params.Add(new FilterParameter("@CardType", _CardType, DbType.AnsiString));

                    string spaName = "spa_cs_GetBatchSearch";
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(_params) });
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoSearching:
                if (!ValidateData())
                {
                    IsIntruderDetected = true;
                    IntruderLog.LogData4 += "&uxFilterValue=" + uxFilterValue.Text.Trim();
                    RaiseIntruderEvent(IntruderType.PostData);
                    return;
                }
                ProcessParams();
                uxReportGrid.CurrentPageIndex = 0;
                uxReportGrid.Rebind();
                break;
        }
    }

    private string _BatchIntruderQuery = string.Empty;
    private string BatchIntruderQuery
    {
        get
        {
            if (this._BatchIntruderQuery == string.Empty)
                this._BatchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportGrid.ID, new string[] { "BatchNumber" });
            return this._BatchIntruderQuery;
        }
    }

    #region "Validate Data"
    
    private bool ValidateData()
    {
        string filterValue = VeraCodeSolution.ValidateResponseData(uxFilterValue.Text.Trim());
        if (uxFilterValue.Text.Trim().IsNullOrEmpty())
            return false;

        if (uxRadMerchantNumber.Checked)
            return GeneralFuncsLib.IsValidWithRegularExpression("[0-9]{1,16}", filterValue);

        if (uxRadMerchantName.Checked)
            return GeneralFuncsLib.IsValidWithRegularExpression("[ ,.A-Za-z0-9]{1,55}", filterValue);

        return true;
    }
    #endregion
}
