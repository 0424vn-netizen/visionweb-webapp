using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using Telerik.Web.UI;
using System.Data;
using AS.Web.UI.Controls;
using AS.Controls.Exporter;
using AS.Web.Business;
using AS.Controls.UserControls;
using AS.Controls.Telerik;
using System.Globalization;
using System.Drawing;
using AS.Utilities;

[PagePermission("CardSearchRpt,MSCardSearchRpt")]
public partial class TransactionSearch : ReportPage
{
    #region Enums

    enum CardOption
    {
        FullCard,
        PartialCard,
    }

    protected enum TransactionOperator
    {
        None,
        EqualTo,
        Between,
        GreaterThan,
        LessThan,
        PlusMinus5
    }

    enum DataBindAction
    {
        BindReportGrid,
    }
    enum PostBackAction
    {
        DoSearching,
        GoBackClick,
    }

    #endregion
    #region Const
    string BETWEEN_OR_PLUS = "BETWEEN {0} AND {1}";
    const string EQUAL_TO = " = {0} ";
    const string GREATER_THAN = " > {0} ";
    const string LESS_THAN = " < {0} ";
    const int ROW_HEIGHT = 30;
    #endregion
    private string _Target = "_parent";
    private HierarchyFilterValue _reportValue = null;
    private FilterParameterCollection _parameters = null;
    private const string IMAGE = "<a class=\"image-link\" href=\"#\" onclick=\" return ShowPopupModal('{0}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp;";
    private const string MATCHED_CODE = "MatchCode";
    private const string MATCHED_NAME = "MatchName";

    private string _fullCard
    {
        get { return (string)ViewState["_fullCard"]; }
        set { ViewState["_fullCard"] = value; }
    }
    private string _firstCard
    {
        get { return (string)ViewState["_firstCard"]; }
        set { ViewState["_firstCard"] = value; }
    }
    private string _lastCard
    {
        get { return (string)ViewState["_lastCard"]; }
        set { ViewState["_lastCard"] = value; }
    }

    private string _firstRouting
    {
        get { return (string)ViewState["_firstRouting"]; }
        set { ViewState["_firstRouting"] = value; }
    }

    private string _lastAccount
    {
        get { return (string)ViewState["_lastAccount"]; }
        set { ViewState["_lastAccount"] = value; }
    }

    private decimal? _amountFrom
    {
        get { return (decimal?)ViewState["_amountFrom"]; }
        set { ViewState["_amountFrom"] = value; }
    }
    private decimal? _amountTo
    {
        get { return (decimal?)ViewState["_amountTo"]; }
        set { ViewState["_amountTo"] = value; }
    }
    private string _merchantNumber
    {
        get { return (string)ViewState["_merchantNumber"]; }
        set { ViewState["_merchantNumber"] = value; }
    }
    private string _authNumber
    {
        get { return (string)ViewState["_authNumber"]; }
        set { ViewState["_authNumber"] = value; }
    }

    private DateTime _beginDate = new DateTime(1900, 1, 1);
    private DateTime _endDate = DateTime.Now;
    private string _BatchDetailIntruderQuery = string.Empty;
    private string BatchDetailIntruderQuery
    {
        get
        {
            if (this._BatchDetailIntruderQuery == string.Empty) this._BatchDetailIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.uxReportGrid.ID, new string[] { "BatchNumber", "TerminalNumber" });
            return this._BatchDetailIntruderQuery;
        }
    }
    private string _AuthIntruderQuery = string.Empty;
    private string AuthIntruderQuery
    {
        get
        {
            if (this._AuthIntruderQuery == string.Empty) this._AuthIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.uxReportGrid.ID, new string[] { "AuthorizationNumber" });
            return this._AuthIntruderQuery;
        }
    }
    private string _CardSearchIntruderQuery = string.Empty;
    private string CardSearchIntruderQuery
    {
        get
        {
            if (this._CardSearchIntruderQuery == string.Empty) this._CardSearchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.uxReportGrid.ID, new string[] { "AccountNumber" });
            return this._CardSearchIntruderQuery;
        }
    }

    public bool IsCSSite { 
        get {
            return SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS;
        } 
    }

    private bool IsShowRoutingAccount
    {
        get
        {
            if (ViewState["ShowRoutingAccount"] != null)
                return (bool)(ViewState["ShowRoutingAccount"]);
            else
            {
                ViewState["ShowRoutingAccount"] = GeneralFuncsLib.Show_RoutingAccountNumber;
                return (bool)ViewState["ShowRoutingAccount"];
            }
        }
        set
        {
            ViewState["ShowRoutingAccount"] = value;
        }
    }

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;
        this.GridIDs.Add("uxReportGrid");
        base.PageInitialize();
    }
    protected override void DoSwitchView()
    {
        if (CheckCSViewFullCard())
        {
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;

            if (IsShowRoutingAccount)
            {
                uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = false;
            }
        }
        else
        {

            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

            if (IsShowRoutingAccount)
            {
                uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
            }
        }
    }
    private bool CheckCSViewFullCard()
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
        {
            return true;
        }
        return false;
    }
    private void SetViewState()
    {
        this._fullCard = txtFullCard.Text.Trim() == string.Empty ? null : txtFullCard.Text.Trim();
        //this._merchantNumber = uxMerchantNumber.Text.Trim();
        this._firstCard = uxFirst6.Text.Trim() == string.Empty ? null : uxFirst6.Text.Trim();
        this._lastCard = uxLast4.Text.Trim() == string.Empty ? null : uxLast4.Text.Trim();
        this._authNumber = uxAuthNumber.Text.Trim() == string.Empty ? null : uxAuthNumber.Text.Trim();

        if (uxTransAmountFrom.Visible && !uxTransAmountFrom.Text.IsNullOrEmpty())
        {
            this._amountFrom = Convert.ToDecimal(uxTransAmountFrom.Value);
        }
        else
        {
            this._amountFrom = null;
        }
        if (uxTransAmountTo.Visible && !uxTransAmountTo.Text.IsNullOrEmpty())
        {
            this._amountTo = Convert.ToDecimal(uxTransAmountTo.Value);
        }
        else
        {
            this._amountTo = null;
        }
    }
    private void SetCardNumber()
    {
        if (IsSecureQueryString)
        {
            string cardNumber = this.SecureQueryString["cn"];
            string merchantNumber = this.SecureQueryString["merch"];
            if (String.IsNullOrEmpty(cardNumber))
            {
                return;
            }
            if (cardNumber.Contains(GetLocalResourceObject("TransactionSearch_aspx_cs_xxx").ToString()))
            {
                uxFirst6.Text = VeraCodeSolution.DoVeraCode(cardNumber.Substring(0, 6));
                uxLast4.Text = VeraCodeSolution.DoVeraCode(cardNumber.Substring(cardNumber.Length - 4, 4));
            }
            else
            {
                txtFullCard.Text = VeraCodeSolution.DoVeraCode(cardNumber);
            }
            //if (!merchantNumber.IsNullOrEmpty())
            //{
            //    uxMerchantNumber.Text = VeraCodeSolution.DoVeraCode(merchantNumber.Trim());
            //}
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        tltDupeCount.Text = string.Format("<span class='tooltip-text1'>{0}</span>", GetLocalResourceObject("DupeCount.HeaderDescription").ToString());

        if (IsIntruderDetected) return;

        //BETWEEN_OR_PLUS = " " + GetLocalResourceObject("TransactionSearch_aspx_cs_Between").ToString() + " ";
        this.IsBindDataOnLoad = true;
        if (SessionManager.ClientFrameInfo != string.Empty) _Target = SessionManager.ClientFrameInfo;
        ltChooseatleastone.Visible = true;
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant
            || SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS)
        {
            ltChooseatleastone.Visible = false;
        }
        if (!IsPostBack)
        {
            //46652 - AW Multi-currency Transaction Display
            this.UpdateCurrencyFormatNumbericTextbox();
            this.CheckHierarchyLevel();
            this.GetReportFilter();
            if (IsSecureQueryString)
            {
                this.SetCardNumber();
                this.SetViewState();
                if (!this.ValidateData())
                {
                    this.IsIntruderDetected = true;
                }
                this.SetReportFilter();
            }
            this.uxReportGrid.Rebind();
        }
        this.uxDate.MaxDate = this.uxFromDate.MaxDate = this.uxEndDate.MaxDate = DateTime.Now;
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
        uxTransAmount.Items.FindItemByValue("PlusMinus5").Text = uxTransAmount.Items.FindItemByValue("PlusMinus5").Text.ToCurrencySymbol();

    }
    private void CheckHierarchyLevel()
    {
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
        {
            phdMerchantLevel1.Visible = phdMerchantLevel2.Visible = false;
        }

        else if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
        {
            if (!IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
                phdMerchantLevel1.Visible = false;
        }
        else
        {
            phdMerchantLevel1.Visible = false;
        }

        if(IsShowRoutingAccount)
        {
            uxRoutingAccountPanel.Visible = true;
        }
    }
    protected void GetReportFilter()
    {
        this._reportValue = SavedReportFilterValue;
        if (this._reportValue == null)
        {
            this._reportValue = new HierarchyFilterValue();
            this._reportValue.DateOption = DateOptionMode.DateRange;
            if (GeneralFuncsLib.HasExtendedSetting("FILTERING_OPTIONS_DATERANGE"))
            {
                this._reportValue.DateOptionValue.From = DateTime.Now.AddDays(-90);
            }
            else
            {
                this._reportValue.DateOptionValue.From = DateTime.Now.GetFirstDayOfMonth();
            }
            this._reportValue.DateOptionValue.To = DateTime.Now;
            this._reportValue.HierarchyMode = uxFilterOption.HierarchyMode;
            this._reportValue.ID = GeneralFuncsLib.GetHierarchyInfo(uxFilterOption.HierarchyMode).HierarchyID;
            this._reportValue.Value = uxFilterOption.HierarchyValue;
        }

        switch (_reportValue.DateOption)
        {
            case DateOptionMode.Daily:
                this.uxDaily.Checked = true;
                this.uxDate.SelectedDate = this._reportValue.DateOptionValue.From;
                break;
            case DateOptionMode.Monthly:
                this.uxMonthly.Checked = true;
                this.uxDate.SelectedDate = this._reportValue.DateOptionValue.From;
                break;
            case DateOptionMode.DateRange:
                this.uxRange.Checked = true;
                this.uxFromDate.SelectedDate = this._reportValue.DateOptionValue.From;
                this.uxEndDate.SelectedDate = this._reportValue.DateOptionValue.To;
                break;
        }
        //if (SavedReportFilterValue != null)
        //{
        //    if (phdMerchantLevel2.Visible)
        //    {
        //        uxFilterOption.FilterValue = this._reportValue.Value.Trim();
        //    }
        //    else
        //    {
        //        uxFilterOption.FilterValue = string.Empty;
        //    }
        //}
        //if (SavedReportFilterValue != null)
        //{
        //    if (GeneralFuncsLib.IsMerchantMode(this._reportValue.HierarchyMode) && phdMerchantLevel2.Visible)
        //    {
        //        uxMerchantNumber.Text = VeraCodeSolution.DoVeraCode(this._reportValue.Value.Trim());
        //    }
        //    else
        //    {
        //        uxMerchantNumber.Text = VeraCodeSolution.DoVeraCode(string.Empty);
        //    }
        //}

    }
    protected void SetReportFilter()
    {
        ////Get report filter form session
        if (SavedReportFilterValue != null)
            this._reportValue = SavedReportFilterValue;
        else
            this._reportValue = new HierarchyFilterValue();
        //// Set Date Option         
        if (uxDaily.Checked)
        {
            this._reportValue.DateOption = DateOptionMode.Daily;
            this._reportValue.DateOptionValue.From = this._reportValue.DateOptionValue.To = uxDate.SelectedDate.Value;
        }
        else if (uxMonthly.Checked)
        {
            this._reportValue.DateOption = DateOptionMode.Monthly;
            this._reportValue.DateOptionValue.From = uxDate.SelectedDate.Value.GetFirstDayOfMonth();

            if (uxDate.SelectedDate.Value.Year == DateTime.Now.Year && uxDate.SelectedDate.Value.Month == DateTime.Now.Month)
            {
                this._reportValue.DateOptionValue.To = uxDate.SelectedDate.Value;
            }
            else
            {
                this._reportValue.DateOptionValue.To = uxDate.SelectedDate.Value.GetLastDayOfMonth();
            }
        }
        else
        {
            this._reportValue.DateOption = DateOptionMode.DateRange;
            this._reportValue.DateOptionValue.From = uxFromDate.SelectedDate != null ? uxFromDate.SelectedDate.Value : DateTime.Now.AddDays(-90);
            this._reportValue.DateOptionValue.To = uxEndDate.SelectedDate != null ? uxEndDate.SelectedDate.Value : DateTime.Now;
        }

        //this._reportValue.Value = (SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.Merchant) ? this._merchantNumber : SessionManager.CurrentUser.EntityID;
        this._reportValue.Value = uxFilterOption.HierarchyValue;
        this._reportValue.HierarchyMode = uxFilterOption.HierarchyMode;
        this._reportValue.ID = GeneralFuncsLib.GetHierarchyInfo(uxFilterOption.HierarchyMode).HierarchyID;

        ////Set report filter
        SavedReportFilterValue = this._reportValue;
        ReportFilter.CurrentValue = SavedReportFilterValue;
    }
    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }
      
    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    if (IsIntruderDetected) return;

                    ASGrid grid = sender as ASGrid;

                    if (!IsPostBack && !IsSecureQueryString)
                    {
                        grid.DataSource = new DataTable();
                        grid.AllowSorting = false;
                        return;
                    }
                    else
                    {
                        grid.AllowSorting = true;
                    }

                    // Do searching

                    this._parameters = new FilterParameterCollection();
                    this._parameters.AddLoggedInUserReportingParams();
                    this._parameters.AddLoggedInUserPrimaryUserID();

                    this._parameters.AddHierarchyFilterParamsWithoutDate(this);
                    //this._parameters.Add(new FilterParameter("@HierarchyFilterValue", this.uxFilterOption.HierarchyValue, DbType.AnsiString));
                    this._parameters.Add(new FilterParameter("@DateFilterMode", (int)SavedReportFilterValue.DateOption, DbType.Int32));
                    this._parameters.Add(new FilterParameter("@BeginDate", SavedReportFilterValue.DateOptionValue.From, DbType.Date));
                    DateTime endDate = SavedReportFilterValue.DateOption == DateOptionMode.DateRange ? SavedReportFilterValue.DateOptionValue.To : SavedReportFilterValue.DateOptionValue.From;
                    this._parameters.Add(new FilterParameter("@EndDate", endDate, DbType.Date));

                    string spa = "spa_GetCardSearch";
                    var hashParams = new List<string>();

                    if (CheckCSViewFullCard())
                    {
                        this._parameters.Add(new FilterParameter("@FullCardNumber", this._fullCard, DbType.String));
                        this._parameters.AddDecryptDataParams("AccountNumber", _isExporting);
                        hashParams.Add("FullCardNumber");

                        if (IsShowRoutingAccount)
                        {
                            this._parameters.AddDecryptDataParams("RoutingAccountNumber", _isExporting);
                        }
                    }

                    if (!string.IsNullOrEmpty(_firstCard) && _firstCard.Trim().Length == 8)
                    { 
                        hashParams.Add("First6CardNumber");
                    }

                    this._parameters.Add(new FilterParameter("@First6CardNumber", this._firstCard, DbType.String));
                    this._parameters.Add(new FilterParameter("@Last4CardNumber", this._lastCard, DbType.String));
                    this._parameters.Add(new FilterParameter("@AuthNumber", this._authNumber, DbType.String));
                    this._parameters.Add(new FilterParameter("@First6Routing", this._firstRouting, DbType.String));
                    this._parameters.Add(new FilterParameter("@Last4AcountNumber", this._lastAccount, DbType.String));
                    
                    if (hashParams != null && hashParams.Any())
                    {
                        this._parameters.AddEncryptedInputParams(string.Join(",", hashParams));
                    }

                    string transAmount = string.Empty;
                    if (_amountFrom != null)
                    {
                        switch (uxTransAmount.SelectedValue.ToUpper())
                        {
                            case "EQUALTO":
                                transAmount = string.Format(EQUAL_TO, _amountFrom);
                                break;
                            case "BETWEEN":
                                transAmount = string.Format(BETWEEN_OR_PLUS, _amountFrom, _amountTo);
                                break;
                            case "GREATERTHAN":
                                transAmount = string.Format(GREATER_THAN, _amountFrom);
                                break;
                            case "LESSTHAN":
                                transAmount = string.Format(LESS_THAN, _amountFrom);
                                break;
                            case "PLUSMINUS5":
                                transAmount = string.Format(BETWEEN_OR_PLUS, _amountFrom - 5, _amountFrom + 5);
                                break;
                        }
                    }
                    this._parameters.Add(new FilterParameter("@TransAmount", transAmount, DbType.String));
                    this._parameters.AddLanguageID();

                    if (uxReportGrid.AS_SortExpression.Trim() == string.Empty)
                    {
                        uxReportGrid.AS_SortExpression = "TransactionDate DESC,TransactionTime DESC,ReportDate DESC";
                    }

                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spa, ReportServices.ConvertToFilterParamWSArray(this._parameters) });
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
                    if (IsIntruderDetected) return;
                    uxReportGrid.CurrentPageIndex = 0;
                    uxReportGrid.AS_SortExpression = string.Empty;
                    if (!this.ValidateData())
                    {
                        if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS)
                        {
                            this.IsIntruderDetected = true;
                        }
                        else
                            this.IsIntruderDetected = false;
                    }
                    this.SetViewState();
                    this.SetReportFilter();
                    this.uxReportGrid.Visible = true;
                    this.uxReportGrid.Rebind();
                }
                break;
        }

    }
    bool _isExporting = false;

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
        _isExporting = true;
        if (IsIntruderDetected) return;
        uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        if (IsShowRoutingAccount)
        {
            uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
        }
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ExportMethod = ExportMethod.BY_FILESTREAM;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(GetLocalResourceObject("TransactionSearch_aspx_cs_ExportFileName").ToString());
        exportConfig.ReportHeader = GetLocalResourceObject("TransactionSearch_aspx_cs_ExportFileName").ToString();
    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        base.DoItemDataBound(sender, e);

        var dupeColumn = uxReportGrid.MasterTableView.Columns.FindByUniqueName("DupeCount");
        string dupeColumnId = GetLocalResourceObject("DupeCount.HeaderTooltip").ToString();
        if (!dupeColumn.HeaderText.Contains(string.Format("id='{0}'", dupeColumnId)))
        {
            string dupeHeaderTextColumn = GetLocalResourceObject("DupeCount.HeaderText").ToString();
            string dupeColumnToolTip = GetLocalResourceObject("DupeCount.HeaderDescription").ToString();
            RM_MCF_GeneralFuncsLib.GenerateTooltipHeaderDefaulColumnID(dupeColumn, dupeHeaderTextColumn, uxReportGrid, dupeColumnId, dupeColumnToolTip);
            dupeColumn.HeaderTooltip = " ";
        }

        if (e.Item is GridDataItem)
        {
            if (sender == uxReportGrid && sender.Visible)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                string queryString = string.Empty;
                string url = string.Empty;
                if (!dataRow["BatchNumber"].ToString().Trim().IsNullOrEmpty())
                {
                    queryString = BuildSecureQueryString("merchantnumber=" + dataRow["MerchantNumber"] + "&BatchNumber=" + dataRow["BatchNumber"] + "&TerminalNumber=" + dataRow["TerminalNumber"] + "&ReportDate=" + ((DateTime)dataRow["ReportDate"]).ToShortDateString() + BatchDetailIntruderQuery);
                    string urlBatchDetail = "BatchDetailModal.aspx?" + queryString;
                    url = "<a class=\"link\" href=\"#\" onclick=\"ShowPopupModal('" + urlBatchDetail + "','auto'); return false;\">";
                    dataItem["BatchNumber"].Text = VeraCodeSolution.DoVeraCode(url + dataRow["BatchNumber"] + "</a>");
                }

                // Build Auth Link
                if (!dataRow["AuthorizationNumber"].ToString().Trim().IsNullOrEmpty())
                {
                    dataItem["AuthorizationNumber"].Text = VeraCodeSolution.DoVeraCode(
                        GeneralFuncsLib.BuildAuthUrlPopupModal(
                        (SecurePage)Page, dataRow["AuthorizationNumber"].ToString(),
                        dataRow["AuthorizationNumber"], AuthIntruderQuery,
                        dataRow["MerchantNumber"].ToString())
                        );
                }

                string urlCard = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialCardNumber"].ToString(), dataRow["AccountNumber"].ToString(), dataRow["MerchantNumber"].ToString());

                string urlRouting = string.Empty;
                if (IsShowRoutingAccount)
                {
                    urlRouting = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialRoutingACC"].ToString(), dataRow["RoutingAccountNumber"].ToString(), dataRow["MerchantNumber"].ToString());
                }
                
                if (CheckCSViewFullCard())
                {
                    dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["AccountNumber"].ToString() + "</a>");
                    if(IsShowRoutingAccount)
                        dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlRouting + dataRow["RoutingAccountNumber"].ToString() + "</a>");
                }
                else
                {
                    if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
                    {
                        dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(
                            String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>")
                            );

                        if (IsShowRoutingAccount)
                            dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(
                            String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlRouting + dataRow["PartialRoutingACC"].ToString() + "</a>")
                            );
                    }
                    else
                    {
                        dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>");

                        if (IsShowRoutingAccount)
                            dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(urlRouting + dataRow["PartialRoutingACC"].ToString() + "</a>");
                    }
                }

                if (!dataRow["CardType"].ToString().Trim().IsNullOrEmpty())
                {
                    dataItem["CardType"].ToolTip = dataRow["CardDescription"].ToString();
                }
                dataItem["CountryCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CountryName"].ToString());

                if (dataRow["DupeAmount"].IsNotNullData())
                {
                    dataItem["DupeCount"].ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", dataRow["DupeAmount"]));
                }
                dataItem["ADF"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ADFDescription"].ToString());
                dataItem["AVS"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["AVSDescription"].ToString());

                dataItem["TransactionCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionCode"].ToString());
                dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntryModeDescription"].ToString());
                dataItem["ExpirationDate"].ToolTip = "MM/YY";

                if (GeneralFuncsLib.NvlString(dataRow["ADF"]).Equals("F", StringComparison.OrdinalIgnoreCase)
                        || GeneralFuncsLib.NvlString(dataRow["ADF"]).Equals("D", StringComparison.OrdinalIgnoreCase))
                {
                    dataItem["ADF"].Text = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatBorderText(dataRow["ADF"].ToString(), Color.Red));
                }
                if (string.IsNullOrEmpty(dataRow["Settled"].ToString()))
                {
                    dataItem["Settled"].Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
                }

                // 46807
                dataItem[MATCHED_CODE].ToolTip = VeraCodeSolution.DoVeraCode(dataRow[MATCHED_NAME].ToString());

                string matchedCode = dataRow[MATCHED_CODE].ToString();

                if (matchedCode.Equals("P", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("CP", StringComparison.OrdinalIgnoreCase))
                {
                    dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, Color.Blue);
                }
                else if (matchedCode.Equals("U", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("SC", StringComparison.OrdinalIgnoreCase))
                {
                    dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, Color.Red);
                }
                else if (matchedCode.Equals("M", StringComparison.OrdinalIgnoreCase) || matchedCode.Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    dataItem[MATCHED_CODE].Text = GeneralFuncsLib.FormatBorderText(matchedCode, "#3fbf00".ToColor());
                }
            }
        }
    }

    protected void uxFilterOption_DoSearch(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoSearching);
    }
    protected void uxSearch_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoSearching);
    }

    private string BuildUrlForFullCard(string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=TransactionDetail" + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate + "&idx=0");
        queryString = "FullCC.aspx?" + queryString;
        return queryString;
    }

    //46652 - AW Multi-currency Transaction Display
    private void UpdateCurrencyFormatNumbericTextbox()
    {
        CultureInfo ci = new CultureInfo(SessionManager.CurrencyFortmat);
        uxTransAmountFrom.Culture = uxTransAmountTo.Culture = ci;

    }
    //protected void BindOption()
    //{
    //    uxOption.Items.Add(new ASRadComboBoxItem() { Text = "Full Card", Value = CardOption.FullCard.ToString() });
    //    uxOption.Items.Add(new ASRadComboBoxItem() { Text = "Card Number", Value = CardOption.PartialCard.ToString() });
    //}

    //private void SetCardOption(bool isFullCard)
    //{
    //    RadComboBoxItem selectedItem = null;
    //    if (isFullCard)
    //    {
    //        selectedItem = uxOption.Items.FindItemByValue(CardOption.FullCard.ToString());
    //    }
    //    else
    //    {
    //        selectedItem = uxOption.Items.FindItemByValue(CardOption.PartialCard.ToString());
    //    }

    //    if (selectedItem != null)
    //        selectedItem.Selected = true;
    //}
    protected void uxExporter_NeedExportConfig(object sender, ExportConfig exportConfig)
    {
        if (uxReportGrid.AS_SortExpression.Trim() == string.Empty)
        {
            uxReportGrid.AS_SortExpression = "TransactionDate DESC,TransactionTime DESC,ReportDate DESC";
        }
    }
    #region Validate data
    private bool ValidateData()
    {
        this._fullCard = txtFullCard.Text.Trim() == string.Empty ? null : txtFullCard.Text.Trim();
        this._firstCard = uxFirst6.Text.Trim() == string.Empty ? null : uxFirst6.Text.Trim();
        this._lastCard = uxLast4.Text.Trim() == string.Empty ? null : uxLast4.Text.Trim();
        this._firstRouting = txtRouting.Text.Trim() == string.Empty ? null : txtRouting.Text.Trim();
        this._lastAccount = txtAccount.Text.Trim() == string.Empty ? null : txtAccount.Text.Trim();
        this._authNumber = uxAuthNumber.Text.Trim() == string.Empty ? null : uxAuthNumber.Text.Trim();
        if (uxTransAmountFrom.Text != string.Empty)
        {
            this._amountFrom = Convert.ToDecimal(uxTransAmountFrom.Value);
        }
        else
        {
            this._amountFrom = null;
        }
        if (uxTransAmountTo.Text != string.Empty)
        {
            this._amountTo = Convert.ToDecimal(uxTransAmountTo.Value);
        }
        else
        {
            this._amountTo = null;
        }
        if (uxRange.Checked)
        {
            this._beginDate = uxFromDate.SelectedDate.Value;
            this._endDate = uxEndDate.SelectedDate.Value;
        }
        if (SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.Merchant)
        {
            if (CheckCSViewFullCard())
            {
                if (this._fullCard.IsNullOrEmpty() && this._firstCard.IsNullOrEmpty() && this._lastCard.IsNullOrEmpty()
                    && this._authNumber.IsNullOrEmpty() && uxFilterOption.HierarchyValue.IsNullOrEmpty()
                    && (uxTransAmountFrom.Text == string.Empty || uxTransAmount.SelectedValue.ToUpper() != "EQUALTO") && IsCSSite)
                {
                    IsIntruderDetected = true;
                    return false;
                }
                if (uxRange.Checked)
                {
                    if (this._fullCard.IsNullOrEmpty() && this._firstCard.IsNullOrEmpty() && this._lastCard.IsNullOrEmpty()
                    && this._authNumber.IsNullOrEmpty() && uxFilterOption.HierarchyValue.IsNullOrEmpty()
                        && DateTime.Compare(this._endDate, this._beginDate) > 90)
                    {
                        IsIntruderDetected = true;
                        return false;
                    }
                }
            }
            else
            {
                if (this._firstCard.IsNullOrEmpty() && this._lastCard.IsNullOrEmpty() && this._firstRouting.IsNullOrEmpty() && this._lastAccount.IsNullOrEmpty() && this._authNumber.IsNullOrEmpty()
                    && uxFilterOption.HierarchyValue.IsNullOrEmpty() && (uxTransAmountFrom.Text == string.Empty || uxTransAmount.SelectedValue.ToUpper() != "EQUALTO") && IsCSSite)
                {
                    IsIntruderDetected = true;
                    return false;
                }
                if (uxRange.Checked)
                {
                    if (this._firstCard.IsNullOrEmpty() && this._lastCard.IsNullOrEmpty() && this._firstRouting.IsNullOrEmpty() && this._lastAccount.IsNullOrEmpty()
                    && this._authNumber.IsNullOrEmpty() && uxFilterOption.HierarchyValue.IsNullOrEmpty()
                        && DateTime.Compare(this._endDate, this._beginDate) > 90)
                    {
                        IsIntruderDetected = true;
                        return false;
                    }
                }
            }
        }
        else
        {
            if (uxRange.Checked)
            {
                if (DateTime.Compare(this._endDate, this._beginDate) > 90)
                {
                    IsIntruderDetected = true;
                    return false;
                }
            }
        }
        if (!this.CheckDateFilter()) return false;
        return true;
    }
    private bool CheckDateFilter()
    {
        if (uxDaily.Checked || uxMonthly.Checked)
        {
            if (!uxDate.SelectedDate.HasValue)
            {
                IsIntruderDetected = true;
                return false;
            }
            if (uxDaily.Checked)
            {
                this._beginDate = this._endDate = uxDate.SelectedDate.Value;
            }
            else if (uxMonthly.Checked)
            {
                this._beginDate = uxDate.SelectedDate.Value.GetFirstDayOfMonth();
                if (this._beginDate.Year == DateTime.Now.Year && this._beginDate.Month == DateTime.Now.Month)
                {
                    this._endDate = uxDate.SelectedDate.Value;
                }
                else
                {
                    this._endDate = uxDate.SelectedDate.Value.GetLastDayOfMonth();
                }
            }
            return true;
        }
        if (uxFromDate.SelectedDate == null || uxEndDate.SelectedDate == null)
        {
            IsIntruderDetected = true;
            return false;
        }
        this._beginDate = uxFromDate.SelectedDate.Value;
        this._endDate = uxEndDate.SelectedDate.Value;
        if (DateTime.Compare(this._beginDate, this._endDate) > 0)
        {
            IsIntruderDetected = true;
            return false;
        }
        return true;
    }

    #endregion
}
