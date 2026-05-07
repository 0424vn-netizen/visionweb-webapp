using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using Telerik.Web.UI;
using System.Data;
using AS.Common.DataProtection;
using AS.Common;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Common.DBManager;
using System.Text.RegularExpressions;

[PagePermission("TCCapture,TCFlatCapture")]
public partial class CaptureSearch : ReportPage
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
    private string _FullNumber
    {
        get { return (string)ViewState["_FullNumber"]; }
        set { ViewState["_FullNumber"] = value; }
    }
    private string _Last4Number
    {
        get { return (string)ViewState["_Last4Number"]; }
        set { ViewState["_Last4Number"] = value; }

    }
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
    private string _AuthID
    {
        get { return (string)ViewState["_AuthID"]; }
        set { ViewState["_AuthID"] = value; }
    }
    private string _RefferenceNumber
    {
        get { return (string)ViewState["_RefferenceNumber"]; }
        set { ViewState["_RefferenceNumber"] = value; }
    }
    private string _BatchNumber
    {
        get { return (string)ViewState["_BatchNumber"]; }
        set { ViewState["_BatchNumber"] = value; }
    }

    private decimal? _TransAmount
    {
        get { return (decimal?)ViewState["_TransAmount"]; }
        set { ViewState["_TransAmount"] = value; }
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
        set { ViewState["_BeginDate"] = value; }
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

    const string CAPTURE_DETAIL = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return ShowPopupModal('{0}','auto');\"><img src='res/images/view.png' border=\"0\"/></a>";
    private string _MerchNumber = string.Empty;
    
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
            if (!IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
            {
                uxRadAccountNumber.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("CaptureSearch_aspx_AccountNumberLast4").ToString());
            }
            SetDefaultDate();
            ProcessParams();
        }
        uxFromDate.MaxDate = uxEndDate.MaxDate = DateTime.Now;
    }


    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        uxReportGrid.Columns.FindByUniqueName("CaptureDetail").Visible = false;

        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExporter.GridHeader);
        exportConfig.ReportHeader = uxExporter.GridHeader;
    }

    protected override void DoSwitchView()
    {
        if (this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
        {
            uxChangeOption.Value = "1";
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
        }
        else
        {
            uxChangeOption.Value = "0";
            if (uxRadAccountNumber.Checked)
            {
                uxFilterValue.MaxLength = 4;
                uxFilterValue.Width = 100;
            }
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        }
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

            if (dataItem["MerchantNumber"].Text.Equals(_MerchNumber))
            {
                dataItem["MerchantNumber"].Text = dataItem["MerchantName"].Text = string.Empty;
            }
            else
            {
                _MerchNumber = VeraCodeSolution.ValidateResponseData(dataItem["MerchantNumber"].Text);
            }

            string captureUrl = "CaptureScreenModalDetail.aspx?{0}";
            captureUrl = string.Format(captureUrl, BuildSecureQueryString("RecordID=" + dataRow["RecordID"].ToString() + "&ReportDate=" + dataRow["ReportDate"].ToSafeString() + CaptureIntruderQuery));
            dataItem["CaptureDetail"].Text = VeraCodeSolution.DoVeraCode(string.Format(CAPTURE_DETAIL, captureUrl));
            //dataItem["TransType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionDescription"].ToString());
            dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardTypeDescription"].ToString());
            dataItem["ExpDate"].ToolTip = "MM/YY";
        }
    }

    private void ProcessParams()
    {
        _BeginDate = uxFromDate.SelectedDate != null ? uxFromDate.SelectedDate.Value : DateTime.Now.AddDays(-7);
        _EndDate = uxEndDate.SelectedDate != null ? uxEndDate.SelectedDate.Value : DateTime.Now;
        _MerchantNumber = VeraCodeSolution.DoVeraCode(uxRadMerchantNumber.Checked ? uxFilterValue.Text.Trim() != string.Empty ? uxFilterValue.Text.Trim() : null : null);
        _MerchantName = VeraCodeSolution.DoVeraCode(uxRadMerchantName.Checked ? uxFilterValue.Text.Trim() != string.Empty ? "%" + uxFilterValue.Text.Trim() + "%" : null : null);
        if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
            _FullNumber = VeraCodeSolution.DoVeraCode(uxRadAccountNumber.Checked ? uxFilterValue.Text.Trim() != string.Empty ? uxFilterValue.Text.Trim() : null : null);
        else
            _Last4Number = VeraCodeSolution.DoVeraCode(uxRadAccountNumber.Checked ? uxFilterValue.Text.Trim() != string.Empty ? uxFilterValue.Text.Trim() : null : null);
        _AuthID = VeraCodeSolution.DoVeraCode(uxAuthID.Text.Trim() != string.Empty ? uxAuthID.Text.Trim() : null);
        _TerminalNumber = VeraCodeSolution.DoVeraCode(uxTerminalNumber.Text.Trim() != string.Empty ? uxTerminalNumber.Text.Trim() : null);
        _RefferenceNumber = VeraCodeSolution.DoVeraCode(uxRefNumber.Text.Trim() != string.Empty ? uxRefNumber.Text.Trim() : null);
        _BatchNumber = VeraCodeSolution.DoVeraCode(uxBatchNumber.Text.Trim() != string.Empty ? uxBatchNumber.Text.Trim() : null);

        if (uxTransAmount.Text != string.Empty)
        {
            _TransAmount = Convert.ToDecimal(uxTransAmount.Value);
        }
        else
        {
            _TransAmount = null;
        }

    }

    private void SetDefaultDate()
    {
        uxRange.Checked = true;
        uxEndDate.SelectedDate = DateTime.Now;
        uxFromDate.SelectedDate = DateTime.Now.AddDays(-7);
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

                    if (this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
                    {
                        _params.AddDecryptDataParams("AccountNumber", _isExporting);

                        _params.AddEncryptedInputParams("FullCardNumber");
                        _params.Add(new FilterParameter("@FullCardNumber", _FullNumber, DbType.AnsiString));
                    }
                    else
                    {
                        _params.Add(new FilterParameter("@Last4CardNumber", _Last4Number, DbType.AnsiString));
                    }
                    _params.Add(new FilterParameter("@AuthorizationID", _AuthID, DbType.AnsiString));
                    _params.Add(new FilterParameter("@TerminalNumber", _TerminalNumber, DbType.AnsiString));
                    _params.Add(new FilterParameter("@ReferenceNumber", _RefferenceNumber, DbType.AnsiString));
                    _params.Add(new FilterParameter("@BatchNumber", _BatchNumber, DbType.AnsiString));
                    _params.Add(new FilterParameter("@TransactionAmount", _TransAmount, DbType.Decimal));
                    _params.AddLanguageID();
                    string spaName = "spa_cs_GetCaptureSearch";
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
    protected void uxSearch_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoSearching);
    }

    private string _CaptureIntruderQuery = string.Empty;
    private string CaptureIntruderQuery
    {
        get
        {
            if (this._CaptureIntruderQuery == string.Empty)
                this._CaptureIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportGrid.ID, new string[] { "RecordID" });
            return this._CaptureIntruderQuery;
        }
    }

    #region "Validate Data"
    private bool ValidateData()
    {
        string filterValue = uxFilterValue.Text.Trim();
        if (uxFilterValue.Text.Trim().IsNullOrEmpty())
            return false;

        if (uxRadMerchantNumber.Checked)
            return GeneralFuncsLib.IsValidWithRegularExpression("[0-9]{1,16}", filterValue);

        if (uxRadMerchantName.Checked)
            return GeneralFuncsLib.IsValidWithRegularExpression("[ ,.A-Za-z0-9]{1,55}", filterValue);

        if (uxRadAccountNumber.Checked)
        {
            if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
                return GeneralFuncsLib.IsValidWithRegularExpression("[0-9]{15,16}", filterValue);
            else
                return GeneralFuncsLib.IsValidWithRegularExpression("[0-9]{4}", filterValue);
        }

        return true;
    }
    #endregion
}
