using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

[PagePermission("ActivationRpt,MSActivationRpt")]
public partial class ActivationReport : ReportPage
{
    private const string DATE = "Date";
    private const string MONTH = "Month";
    private const string DATE_RANGE = "DateRange";
    private const string OPEN_DATE = "OpenDate";
    private const string BATCH_DATE = "BatchDate";
    private const string AND = "And";
    private const string OR = "Or";

    enum DataBindAction
    {
        BindActivitionReport,
        BindBatchAmount
    }
    enum PostBackAction
    {
        DoSearching,
    }
    #region Fileds
    private HierarchyFilterValue _reportValue = null;
    private bool IsSearch = false;
    #endregion

    #region Properties
    public List<RefFilter> DateTypes
    {
        get
        {
            List<RefFilter> dateTypes = new List<RefFilter>();
            dateTypes.Add(new RefFilter()
            {
                Key = OPEN_DATE,
                Value = GetLocalResourceObject("OpenDateResource").ToString()
            });
            dateTypes.Add(new RefFilter()
            {
                Key = BATCH_DATE,
                Value = GetLocalResourceObject("BatchDateResource").ToString()
            });

            return dateTypes;
        }
    }

    public List<RefFilter> Dates
    {
        get
        {
            List<RefFilter> dates = new List<RefFilter>();
            dates.Add(new RefFilter()
            {
                Key = DATE,
                Value = GetLocalResourceObject("DateResource").ToString()
            });
            dates.Add(new RefFilter()
            {
                Key = MONTH,
                Value = GetLocalResourceObject("MonthResource").ToString()
            });
            dates.Add(new RefFilter()
            {
                Key = DATE_RANGE,
                Value = GetLocalResourceObject("DateRangeResource").ToString()
            });

            return dates;
        }
    }

    public List<RefFilter> Criterions
    {
        get
        {
            List<RefFilter> criterions = new List<RefFilter>();
            criterions.Add(new RefFilter()
            {
                Key = AND,
                Value = GetLocalResourceObject("AndResource").ToString()
            });
            criterions.Add(new RefFilter()
            {
                Key = OR,
                Value = GetLocalResourceObject("OrResource").ToString()
            });
            return criterions;
        }
    }

    public string FirstDateType
    {
        get
        {
            if (ViewState["FirstDateType"].IsNullData())
                return OPEN_DATE;
            return ViewState["FirstDateType"].ToString();
        }
        set
        {
            ViewState["FirstDateType"] = value;
        }
    }


    public string DateSecondFilter
    {
        get
        {
            if (ViewState["DateSecondFilter"].IsNullData())
                return null;
            return ViewState["DateSecondFilter"].ToString();
        }
        set
        {
            ViewState["DateSecondFilter"] = value;
        }
    }

    public DateTime? DateSecond
    {
        get
        {
            if (ViewState["DateSecond"].IsNullData())
                return null;
            else
            {
                DateTime dt;
                DateTime.TryParse(ViewState["DateSecond"].ToString(), out dt);
                return dt;
            }
        }
        set
        {
            ViewState["DateSecond"] = value;
        }
    }

    public DateTime? FromDateSecond
    {
        get
        {
            if (ViewState["FromDateSecond"].IsNullData())
            {
                return null;
            }
            else
            {
                DateTime dt;
                DateTime.TryParse(ViewState["FromDateSecond"].ToString(), out dt);
                return dt;
            }
        }
        set
        {
            ViewState["FromDateSecond"] = value;
        }
    }

    public DateTime? ToDateSecond
    {
        get
        {
            if (ViewState["ToDateSecond"].IsNullData())
                return null;
            else
            {
                DateTime dt;
                DateTime.TryParse(ViewState["ToDateSecond"].ToString(), out dt);
                return dt;
            }
        }
        set
        {
            ViewState["ToDateSecond"] = value;
        }
    }

    public string Operator
    {
        get
        {
            if (ViewState["Operator"].IsNullData())
                return null;
            return ViewState["Operator"].ToString();
        }
        set
        {
            ViewState["Operator"] = value;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        IsBindDataOnLoad = true;

        if (!IsPostBack)
        {
            uxPlSecondFilter.Visible = false;
            GetDateFilter();
            SetDateFilter();

            uxDateSecond.SelectedDate = DateTime.Now;
            BindDataForDropdownFilter();

            OnDataBindControls(DataBindAction.BindBatchAmount);
            OnDataBindControls(DataBindAction.BindActivitionReport, uxReportGrid);
            uxEditAmount.Visible = IsUserWithPermission("EditMinimumBatchAmount") || IsUserWithPermission("MSEditMinimumBatchAmount");

        }
    }

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;
        this.ExporterIDs.Add("uxExport");
        base.PageInitialize();
        InitializeGridColumn();
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindBatchAmount:
                {
                    FilterParameterCollection _parames = new FilterParameterCollection();
                    _parames.AddLoggedInUserReportingParams(true);
                    DataTable dt = WebServices.SecurityServices.GetReports("spa_SEC_GetMinimumBatchAmount", _parames);
                    var batchAmount = AS.Common.Formater.FormatData.FormatCurrency(dt.Rows[0]["MinimumBatchAmount"], SessionManager.CurrencyFortmat);
                    uxBatchAmount.Text = batchAmount;
                    break;
                }

            case DataBindAction.BindActivitionReport:
                {
                    //Paging and change page size
                    FilterParameterCollection _parames = new FilterParameterCollection();
                    _parames.AddLoggedInUserReportingParams();
                    _parames.AddHierarchyFilterParamsWithoutDate(this);

                    //Delele filter but doesn't click search and export
                    var secondDateMode = GetDateOption(!DateSecondFilter.IsNullOrEmpty() ? DateSecondFilter : string.Empty);
                    DateTime firstEndDate = this.ReportFilter.CurrentValue.DateOption == DateOptionMode.DateRange ? this.ReportFilter.CurrentValue.DateOptionValue.To : this.ReportFilter.CurrentValue.DateOptionValue.From;
                    DateTime? secondBeginDate = null;
                    DateTime? secondEndDate = null;

                    if (!DateSecondFilter.IsNullOrEmpty() && secondDateMode.IsNotNullData())
                    {
                        secondBeginDate = secondDateMode == DateOptionMode.DateRange ? FromDateSecond.Value : DateSecond.Value;
                        secondEndDate = secondDateMode == DateOptionMode.DateRange ? ToDateSecond.Value : DateSecond.Value;
                    }

                    if (FirstDateType == OPEN_DATE)
                    {
                        _parames.Add(new FilterParameter("@OpenDateFilterMode", this.ReportFilter.CurrentValue.DateOption, DbType.Int32));
                        _parames.Add(new FilterParameter("@OpenBeginDate", this.ReportFilter.CurrentValue.DateOptionValue.From, DbType.Date));
                        _parames.Add(new FilterParameter("@OpenEndDate", firstEndDate, DbType.Date));
                        _parames.Add(new FilterParameter("@DateFilterMode", secondDateMode, DbType.Int32));
                        _parames.Add(new FilterParameter("@BeginDate", secondBeginDate, DbType.Date));
                        _parames.Add(new FilterParameter("@EndDate", secondEndDate, DbType.Date));
                    }
                    if (FirstDateType == BATCH_DATE)
                    {
                        _parames.Add(new FilterParameter("@DateFilterMode", this.ReportFilter.CurrentValue.DateOption, DbType.Int32));
                        _parames.Add(new FilterParameter("@BeginDate", this.ReportFilter.CurrentValue.DateOptionValue.From, DbType.Date));
                        _parames.Add(new FilterParameter("@EndDate", firstEndDate, DbType.Date));
                        _parames.Add(new FilterParameter("@OpenDateFilterMode", secondDateMode, DbType.Int32));
                        _parames.Add(new FilterParameter("@OpenBeginDate", secondBeginDate, DbType.Date));
                        _parames.Add(new FilterParameter("@OpenEndDate", secondEndDate, DbType.Date));
                    }

                    _parames.Add(new FilterParameter("@Operator", !DateSecondFilter.IsNullOrEmpty() ? Operator : null, DbType.AnsiString));
                    _parames.AddLanguageID();

                    if (IsSearch)
                    {
                        uxReportGrid.MasterTableView.CurrentPageIndex = 0;
                        uxReportGrid.MasterTableView.PageSize = 10;
                        uxReportGrid.AS_SortExpression = string.Empty;
                    }
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_GetActivationReport", ReportServices.ConvertToFilterParamWSArray(_parames) });

                    uxExporter.GridSubTitle = GetGridSubTitle();
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoSearching:
                {
                    FirstDateType = uxDateTypeFirstFilter.SelectedValue;
                    if (uxPlSecondFilter.Visible)
                    {
                        DateSecondFilter = uxDateSecondFilter.SelectedValue;
                        DateSecond = uxDateSecond.SelectedDate;
                        FromDateSecond = uxFromDateSecond.SelectedDate;
                        ToDateSecond = uxToDateSecond.SelectedDate;
                        Operator = uxOperator.SelectedValue;
                    }
                    else
                    {
                        //Reset second filter
                        DateSecondFilter = null;
                        DateSecond = FromDateSecond = ToDateSecond = null;
                        Operator = null;
                    }
                    SetDateFilter();
                    uxReportGrid.Rebind();
                }
                break;
        }
    }

    protected void uxReportGrid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            //Merchant Number hyperlink
            string queryString = this.BuildSecureQueryString(string.Format("merchantnumber={0}", dataRow["MerchantNumber"].ToString()));
            dataItem["MerchantNumber"].Text = string.Format("<a class=\"link\" href='MerchantProfile.aspx?{0}' style=\"cursor:pointer\" >{1}</a>", queryString, dataRow["MerchantNumber"].ToString());

            //Batch hyperlink
            string urlBatchDetail = "BatchDetailModal.aspx?" + BuildSecureQueryString("MerchantNumber=" + dataRow["MerchantNumber"] + "&ReportDate=" + dataRow["BatchDate"] + "&BatchNumber=" + dataRow["BatchNumber"]);
            string url = "<a class=\"link\" href=\"#\" onclick=\"ShowPopupModal('" + urlBatchDetail + "','auto'); return false;\">" + dataRow["BatchNumber"] + "</a>";
            dataItem["BatchNumber"].Text = VeraCodeSolution.DoVeraCode(url);
            if (dataRow["BatchAmount"].IsNotNullData())
            {
                dataItem["BatchAmount"].Text = GeneralFuncsLib.FormatCurrency(dataRow["BatchAmount"], "C");
            }

            string ageDay = dataRow["Age"].ToInt() > 1 ? GetLocalResourceObject("DaysResource").ToString() : GetLocalResourceObject("DayResource").ToString();
            // dataItem["Age"].Text = string.Format("{0} {1}", dataRow["Age"].ToString(), ageDay);

            if (dataRow["Age"].ToInt() < 0)
            {
                dataItem["Age"].Text = string.Format("<span class='negative'>{0}</span>", dataRow["Age"].ToString().Trim('-'));
            }
        }
    }

    //With Activation Report, the default value of date filter is "Daily"
    protected override void FilteringOptionDateSwitchView()
    {
        if (SavedReportFilterValue == null)
        {
            SavedReportFilterValue = new HierarchyFilterValue();
            SavedReportFilterValue.DateOption = DateOptionMode.Daily;
            SavedReportFilterValue.DateOptionValue.To = DateTime.Now;
            SavedReportFilterValue.DateOptionValue.From = DateTime.Now.GetFirstDayOfMonth();
        }
        base.FilteringOptionDateSwitchView();
    }

    protected void uxReportGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindActivitionReport, uxReportGrid);
    }

    protected void uxActivationHirarchyFilter_DoSearch(object sender, EventArgs e)
    {
        IsSearch = true;
        OnPostBackActions(PostBackAction.DoSearching);
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoSearching);
    }

    protected void uxAddFilter_Click(object sender, EventArgs e)
    {
        uxPlSecondFilter.Visible = true;
        uxAddFilter.Visible = false;

        //Date type first filter
        uxDateTypeFirstFilter.DataTextField = "Value";
        uxDateTypeFirstFilter.DataValueField = "Key";
        uxDateTypeFirstFilter.DataSource = DateTypes.Where(m => m.Key == uxDateTypeFirstFilter.SelectedValue).ToList();
        uxDateTypeFirstFilter.DataBind();

        var dateTypesSecond = DateTypes.Where(m => m.Key != uxDateTypeFirstFilter.SelectedValue).ToList();
        //Date type second filter
        uxDateTypeSecondFilter.DataTextField = "Value";
        uxDateTypeSecondFilter.DataValueField = "Key";
        uxDateTypeSecondFilter.DataSource = dateTypesSecond;
        uxDateTypeSecondFilter.DataBind();
        //Date second filter
        uxDateSecondFilter.DataTextField = "Value";
        uxDateSecondFilter.DataValueField = "Key";
        uxDateSecondFilter.DataSource = Dates;
        uxDateSecondFilter.DataBind();

        //Criterion
        uxOperator.DataTextField = "Value";
        uxOperator.DataValueField = "Key";
        uxOperator.DataSource = Criterions;
        uxOperator.DataBind();

        this.AjaxAddResponseScript("addCheckSpecialCharactersForDate();");
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        //uxReportGrid.Columns.FindByUniqueName("Age").Visible = false;
        //uxReportGrid.Columns.FindByUniqueName("AgeString").Visible = true;
         
        base.DoNeedExportConfig(sender, exportConfig);

        if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.CSV)
        {
            uxReportGrid.IsAutoExportTemplate = false;
            uxReportGrid.AllowExportAtWebServices = false;
        }
        exportConfig.FileName = string.Format("{0}-{1}", GeneralFuncsLib.FormatFileName(GetLocalResourceObject("ExportFileName").ToString()), DateTime.Now.ToString("MMddyyyy"));

        string hierarchyValue = !ReportFilter.CurrentValue.Value.IsNullOrEmpty() ? ReportFilter.CurrentValue.Value : GetLocalResourceObject("AllResource").ToString();

        HierarchyDetail hierarchyInfo = GeneralFuncsLib.GetHierarchyInfo(ReportFilter.CurrentValue.HierarchyMode) ?? new HierarchyDetail();

        string hierarchyTitle = string.Format("{0}: {1}", hierarchyInfo.HierarchyName, hierarchyValue);

        string batchAmount = string.Format("{0} {1}", GetLocalResourceObject("uxBatchAmountTextResources.Text").ToString(), uxBatchAmount.Text);
        exportConfig.ReportHeader = GetLocalResourceObject("ExportTitle").ToString() + "\n" + GetGridSubTitle() + "\n" + hierarchyTitle + "\n" + batchAmount;
    }

    protected void uxBtnDeleteFilter_Click(object sender, EventArgs e)
    {
        uxPlSecondFilter.Visible = false;
        uxAddFilter.Visible = true;
        //Date type first filter
        uxDateTypeFirstFilter.DataTextField = "Value";
        uxDateTypeFirstFilter.DataValueField = "Key";
        uxDateTypeFirstFilter.DataSource = DateTypes;
        uxDateTypeFirstFilter.DataBind();

        this.AjaxAddResponseScript("addCheckSpecialCharactersForDate();");
    }

    private void BindDataForDropdownFilter()
    {
        //Date type first filter
        uxDateTypeFirstFilter.DataTextField = "Value";
        uxDateTypeFirstFilter.DataValueField = "Key";
        uxDateTypeFirstFilter.DataSource = DateTypes;
        uxDateTypeFirstFilter.DataBind();
        //Date first filter
        uxDateFirstFilter.DataTextField = "Value";
        uxDateFirstFilter.DataValueField = "Key";
        uxDateFirstFilter.DataSource = Dates;
        uxDateFirstFilter.DataBind();
        //Criterion
        uxOperator.DataTextField = "Value";
        uxOperator.DataValueField = "Key";
        uxOperator.DataSource = Criterions;
        uxOperator.DataBind();

    }

    //Init dynamic column
    protected void InitializeGridColumn()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        parameters.AddLanguageID();
        DataTable hierarchies = WebServices.RiskServices.GetReports("spa_GetActivationReportHierarchy", parameters);
        foreach (DataRow row in hierarchies.Rows)
        {
            AddColToGrid(row["DataKey"].ToString(), row["DataText"].ToString(), row["DataText"].ToString(), FormatType.StaticString, 200);
        }
        AddColToGrid("BatchDate", GetLocalResourceObject("ASGridBoundColumnResource4.HeaderText").ToString(), GetLocalResourceObject("ASGridBoundColumnResource4.HeaderTooltip").ToString(), FormatType.Date, 110);
        AddColToGrid("BatchAmount", GetLocalResourceObject("ASGridBoundColumnResource5.HeaderText").ToString(), GetLocalResourceObject("ASGridBoundColumnResource5.HeaderTooltip").ToString(), FormatType.Currency, 150);
        AddColToGrid("BatchNumber", GetLocalResourceObject("ASGridBoundColumnResource6.HeaderText").ToString(), GetLocalResourceObject("ASGridBoundColumnResource6.HeaderTooltip").ToString(), FormatType.StaticString, 150);
        //Sales Rep Code is only visible for FIS client and CS site
        if (SessionManager.CurrentClient == WebSiteConstants.FIS_CLIENT)
        {
            AddColToGrid("SalesRepCode", GetLocalResourceObject("ASGridBoundColumnResource7.HeaderText").ToString(), GetLocalResourceObject("ASGridBoundColumnResource7.HeaderTooltip").ToString(), FormatType.StaticString, 150);
        }
        AddColToGrid("Contact", GetLocalResourceObject("ASGridBoundColumnResource8.HeaderText").ToString(), GetLocalResourceObject("ASGridBoundColumnResource8.HeaderTooltip").ToString(), FormatType.DynamicString, 200);
        AddColToGrid("Phone", GetLocalResourceObject("ASGridBoundColumnResource9.HeaderText").ToString(), GetLocalResourceObject("ASGridBoundColumnResource9.HeaderTooltip").ToString(), FormatType.Phone, 150);
        AddColToGrid("OpenDate", GetLocalResourceObject("ASGridBoundColumnResource10.HeaderText").ToString(), GetLocalResourceObject("ASGridBoundColumnResource10.HeaderTooltip").ToString(), FormatType.Date, 110);
        AddColToGrid("Age", GetLocalResourceObject("ASGridBoundColumnResource11.HeaderText").ToString(), GetLocalResourceObject("ASGridBoundColumnResource11.HeaderTooltip").ToString(), FormatType.Integer, 100);
        // AddColToGrid("AgeString", GetLocalResourceObject("ASGridBoundColumnResource11.HeaderText").ToString(), GetLocalResourceObject("ASGridBoundColumnResource11.HeaderTooltip").ToString(), FormatType.StaticString, 100, false);
    }

    private void AddColToGrid(string colName, string headerText, string headerToolTip, FormatType type, int width, bool isVisible = true)
    {
        AS.Controls.Global.ASGridBoundColumn newCol = new AS.Controls.Global.ASGridBoundColumn();
        newCol.DataField = colName;
        newCol.UniqueName = colName;
        newCol.HeaderText = headerText;
        newCol.HeaderTooltip = headerToolTip;
        newCol.ASFormat = type;
        newCol.SortExpression = colName;
        newCol.HeaderStyle.Width = width;
        newCol.ItemStyle.Width = width;
        newCol.Visible = isVisible;
        uxReportGrid.Columns.Add(newCol);
    }

    // Get Date from Session if not null and assign to control
    private void GetDateFilter()
    {
        this._reportValue = this.ReportFilter.CurrentValue;
        switch (_reportValue.DateOption)
        {
            case DateOptionMode.Daily:
                this.uxDateFirstFilter.SelectedValue = DATE;
                this.uxDateFirst.SelectedDate = this._reportValue.DateOptionValue.To;
                break;
            case DateOptionMode.Monthly:
                this.uxDateFirstFilter.SelectedValue = MONTH;
                this.uxDateFirst.SelectedDate = this._reportValue.DateOptionValue.To;
                break;
            case DateOptionMode.DateRange:
                this.uxDateFirstFilter.SelectedValue = DATE_RANGE;
                this.uxFromDateFirst.SelectedDate = this._reportValue.DateOptionValue.From;
                this.uxToDateFirst.SelectedDate = this._reportValue.DateOptionValue.To;
                break;
        }
    }

    // Set Date to Session
    private void SetDateFilter()
    {
        //Get report filter form session
        if (this.ReportFilter.CurrentValue != null)
            this._reportValue = this.ReportFilter.CurrentValue;
        else
            this._reportValue = new HierarchyFilterValue();
        // Set Date Option         
        if (uxDateFirstFilter.SelectedValue == DATE)
        {
            this._reportValue.DateOption = DateOptionMode.Daily;
            this._reportValue.DateOptionValue.From = this._reportValue.DateOptionValue.To = uxDateFirst.SelectedDate.Value;
        }
        else if (uxDateFirstFilter.SelectedValue == MONTH)
        {
            this._reportValue.DateOption = DateOptionMode.Monthly;
            this._reportValue.DateOptionValue.From = uxDateFirst.SelectedDate.Value;
        }
        else
        {
            this._reportValue.DateOption = DateOptionMode.DateRange;
            this._reportValue.DateOptionValue.From = uxFromDateFirst.SelectedDate.Value;
            this._reportValue.DateOptionValue.To = uxToDateFirst.SelectedDate.Value;
        }
        this._reportValue.Value = uxActivationHirarchyFilter.HierarchyValue;
        this._reportValue.HierarchyMode = uxActivationHirarchyFilter.HierarchyMode;
        this._reportValue.ID = GeneralFuncsLib.GetHierarchyInfo(uxActivationHirarchyFilter.HierarchyMode).HierarchyID;

        // Set report filter
        this.ReportFilter.CurrentValue = SavedReportFilterValue = this._reportValue;
    }

    protected void uxBtnUpdateBatchAmount_Click(object sender, EventArgs e)
    {
        uxBatchAmountInfo.Visible = true;
        OnDataBindControls(DataBindAction.BindBatchAmount);
    }

    private DateOptionMode? GetDateOption(string mode)
    {
        if (mode.IsNullOrEmpty())
            return null;
        if (mode == DATE)
            return DateOptionMode.Daily;
        else if (mode == MONTH)
            return DateOptionMode.Monthly;
        else
            return DateOptionMode.DateRange;
    }

    private string GetGridSubTitle()
    {
        string firstDateType = string.Empty;
        string secondDateType = string.Empty;
        string firstDate = string.Empty;
        string secondDate = string.Empty;

        string firstTitle = string.Empty;
        string secondTitle = string.Empty;

        firstDateType = FirstDateType == OPEN_DATE ? GetLocalResourceObject("GridSubTitleOpenDateResource").ToString()
            : GetLocalResourceObject("GridSubTitleBatchDateResource").ToString();
        if (this.ReportFilter.CurrentValue.DateOption == DateOptionMode.DateRange)
        {
            firstDate = string.Format("{0} - {1}", this.ReportFilter.CurrentValue.DateOptionValue.From.ToString("MM/dd/yyyy"), this.ReportFilter.CurrentValue.DateOptionValue.To.ToString("MM/dd/yyyy"));
        }
        else if (this.ReportFilter.CurrentValue.DateOption == DateOptionMode.Monthly)
        {
            firstDate = this.ReportFilter.CurrentValue.DateOptionValue.From.ToString("MM/yyyy");
        }
        else
        {
            firstDate = this.ReportFilter.CurrentValue.DateOptionValue.From.ToString("MM/dd/yyyy");
        }

        firstTitle = string.Format(firstDateType, firstDate);

        if (!uxPlSecondFilter.Visible)
            return firstTitle;
        else
        {
            if (DateSecondFilter.IsNullOrEmpty())
                return firstTitle;
            secondDateType = uxDateTypeSecondFilter.SelectedValue == OPEN_DATE ? GetLocalResourceObject("GridSubTitleOpenDateResource").ToString()
            : GetLocalResourceObject("GridSubTitleBatchDateResource").ToString();
            var secondDateMode = GetDateOption(DateSecondFilter);
            if (secondDateMode == DateOptionMode.DateRange)
            {
                secondDate = string.Format("{0} - {1}", FromDateSecond.Value.ToString("MM/dd/yyyy"), ToDateSecond.Value.ToString("MM/dd/yyyy"));
            }
            else if (secondDateMode == DateOptionMode.Monthly)
            {
                secondDate = DateSecond.Value.ToString("MM/yyyy");
            }
            else
            {
                secondDate = DateSecond.Value.ToString("MM/dd/yyyy");
            }
            secondTitle = string.Format(secondDateType, secondDate);
            string operatior = Operator == AND ? GetLocalResourceObject("AndGridTitleResource").ToString()
                : GetLocalResourceObject("OrGridTitleResource").ToString();
            return string.Format("{0} {1} {2}", firstTitle, operatior, secondTitle);
        }
    }
}

public class RefFilter
{
    public string Key { get; set; }
    public string Value { get; set; }
}