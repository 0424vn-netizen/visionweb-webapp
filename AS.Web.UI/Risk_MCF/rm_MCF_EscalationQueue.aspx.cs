using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

[PagePermission("RskEscQueue,MSRskEscQueue")]
public partial class rm_MCF_EscalationQueue : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        AssignedList,
        StatusList,
        ReasonList,
        Resolution,
        Grid
    }

    enum PostBackAction
    {
        Search,
        SelectedTicket,
        SelectedMerchant
    }

    #endregion

    #region Constants

    private const string SESSION_FILTERING_OPTIONS = "EscalationQueueFilteringOptions";
    private const string ESCALATION_ID = "EscalationID";
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string MERCHANT_NAME = "MerchantName";
    private const string SORT_DEFAULT = "fe.EscalationID";
    #endregion Constants

    #region Fields

    private string _escalationHistoryIntruderQuery = string.Empty;
    private string _riskReportIntruderQuery = string.Empty;

    #endregion Fields

    #region Properties

    private string AssignedToList
    {
        get
        {
            string list = string.Empty;

            foreach (RadListBoxItem item in uxFilterAssignedToList.CheckedItems)
            {
                list += item.DataKey + ",";
            }

            return list.TrimEnd(',');
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                return;

            string list = string.Format(",{0},", value);
            foreach (RadListBoxItem item in uxFilterAssignedToList.Items)
            {
                item.Checked = (list.IndexOf(string.Format(",{0},", item.DataKey)) >= 0);
            }
        }
    }


    private string ReasonList
    {
        get
        {
            string list = string.Empty;

            foreach (RadListBoxItem item in uxFilterReason.CheckedItems)
            {
                list += item.DataKey + ",";
            }

            return list.TrimEnd(',');
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                return;

            string list = string.Format(",{0},", value);
            foreach (RadListBoxItem item in uxFilterReason.Items)
            {
                item.Checked = (list.IndexOf(string.Format(",{0},", item.DataKey)) >= 0);
            }
        }
    }

    private string ResolutionList
    {
        get
        {
            string list = string.Empty;

            foreach (RadListBoxItem item in uxFilterResolutionList.CheckedItems)
            {
                list += item.DataKey + ",";
            }

            return list.TrimEnd(',');
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                return;

            string list = string.Format(",{0},", value);
            foreach (RadListBoxItem item in uxFilterResolutionList.Items)
            {
                item.Checked = (list.IndexOf(string.Format(",{0},", item.DataKey)) >= 0);
            }
        }
    }

    private string StatusList
    {
        get
        {
            string list = string.Empty;

            foreach (RadListBoxItem item in uxFilterStatusList.CheckedItems)
            {
                list += item.DataKey + ",";
            }

            return list.TrimEnd(',');
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                return;

            string list = string.Format(",{0},", value);
            foreach (RadListBoxItem item in uxFilterStatusList.Items)
            {
                item.Checked = (list.IndexOf(string.Format(",{0},", item.DataKey)) >= 0);
            }
        }
    }

    private string KeyType
    {
        get
        {
            return uxFilterOption.SelectedValue;
        }

        set
        {
            uxFilterOption.SelectedValue = value;

        }
    }

    private string KeyValue
    {
        get
        {
            return VeraCodeSolution.DoVeraCode(uxFilterSearchKeyText.Text.Trim());
        }
        set
        {
            uxFilterSearchKeyText.Text = VeraCodeSolution.DoVeraCode(value);
        }
    }

    private string EscalationHistoryIntruderQuery
    {
        get
        {
            if (_escalationHistoryIntruderQuery.IsNullOrEmpty())
            {
                _escalationHistoryIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                  this.ID, new string[] { ESCALATION_ID });
            }
            return _escalationHistoryIntruderQuery;
        }
    }

    private string RiskReportIntruderQuery
    {
        get
        {
            if (_riskReportIntruderQuery.IsNullOrEmpty())
            {
                _riskReportIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                       this.ID, new string[] { MERCHANT_NUMBER });
            }
            return _riskReportIntruderQuery;
        }
    }

    #endregion Properties

    #region Methods

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterTop");
        base.PageInitialize();
        this.IsSecureCSRF = true;
        IsBindDataOnLoad = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            RiskSessionManager.RiskReportReferrer = null;
            RiskSessionManager.EscalationQueueReferrer = null;

            OnDataBindControls(DataBindAction.AssignedList);
            OnDataBindControls(DataBindAction.ReasonList);
            OnDataBindControls(DataBindAction.StatusList);
            OnDataBindControls(DataBindAction.Resolution);

            SetDefaultValue();
            SetSessionValueToControl();
        }
        if (GeneralFuncsLib.GetDataOfExtendedSetting("GainLossInvestigationAmount").ToLower().Equals("true"))
        {
            GridColumn colSavingLoss = uxReportGrid.MasterTableView.GetColumnSafe("SavingLoss");
            if (colSavingLoss != null)
            {
                colSavingLoss.Visible = true;
            }
            RadComboBoxItem itemSavingLoss = uxFilterOpenClosed.FindItemByValue("SavingLossAmtBetween");
            if (itemSavingLoss != null)
            {
                itemSavingLoss.Visible = true;
            }
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected)
            return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.AssignedList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@Mode", 0, DbType.Int32));
                    uxFilterAssignedToList.DataSource = WebServices.RiskServices.GetReports(
                        "spa_RM_MCF_GetEscalationUsers", parameters);
                    uxFilterAssignedToList.DataBind();
                }
                break;
            case DataBindAction.ReasonList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@IsActive", -1, DbType.Int32));
                    uxFilterReason.DataSource = WebServices.RiskServices.GetReports(
                        "spa_RM_MCF_GetEscalationReason", parameters);
                    uxFilterReason.DataBind();
                }
                break;
            case DataBindAction.StatusList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
                    parameters.Add(new FilterParameter("@SortOrder", "Resolution ASC", DbType.AnsiString));
                    uxFilterResolutionList.DataSource = WebServices.RiskServices.GetReports(
                        "spa_RM_MCF_GetResolution", parameters);
                    uxFilterResolutionList.DataBind();
                }
                break;
            case DataBindAction.Resolution:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@IsActive", 1, DbType.Int32));
                    parameters.Add(new FilterParameter("@SortOrder", "Status ASC", DbType.AnsiString));
                    uxFilterStatusList.DataSource = WebServices.RiskServices.GetReports(
                        "spa_RM_MCF_GetEscalationStatus", parameters);
                    uxFilterStatusList.DataBind();
                }
                break;
            case DataBindAction.Grid:
                {
                    BindData();
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected)
            return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.Search:
                {
                    SetSessionValueToControl();
                    uxReportGrid.CurrentPageIndex = 0;
                    uxReportGrid.Rebind();
                }
                break;
            case PostBackAction.SelectedTicket:
            case PostBackAction.SelectedMerchant:
                {
                    string[] parms = hddProcessData.Value.Split(';');
                    if (parms.Length > 1)
                    {
                        if (string.Compare(parms[0], "escalation") == 0)
                        {
                            string escalationNumber = parms[1];
                            RiskSessionManager.EscalationQueueReferrer = "EscalationQueue";
                            RiskSessionManager.EscalationQueueReferrerInfo =
                                new ReferrerInfo(escalationNumber, "rm_MCF_EscalationQueue.aspx", "Escalation Queue");
                            string queryString = BuildSecureQueryString(string.Format("{0}={1}{2}",
                                ESCALATION_ID, escalationNumber, this.EscalationHistoryIntruderQuery));

                            Response.Redirect("rm_MCF_EscalationHistory.aspx?" + queryString, true);
                        }
                        else if (string.Compare(parms[0], "merchant") == 0)
                        {
                            string merchantNumber = parms[1];
                            RiskSessionManager.RiskReportReferrer = "EscalationQueue";
                            RiskSessionManager.RiskReportReferrerInfo =
                                new ReferrerInfo(merchantNumber, "rm_MCF_EscalationQueue.aspx", "Escalation Queue");
                            string queryString = BuildSecureQueryString(string.Format("{0}={1}{2}",
                                MERCHANT_NUMBER, merchantNumber, this.RiskReportIntruderQuery));

                            Response.Redirect("rm_MCF_RiskReport.aspx?" + queryString, true);
                        }
                    }
                }
                break;
        }
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.Grid);
    }
    protected void uxReportGrid_OnSortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        //if (e.SortExpression == "FollowupDate" && e.NewSortOrder == GridSortOrder.Ascending) 
        //    IsSortFollowupDate = true; 
        //else 
        //    IsSortFollowupDate = false; 
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        string formatValue = "<span>{0}</span>";

        if (IsIntruderDetected)
            return;
        if (sender == uxReportGrid)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;

                // Tkt column
                string url = string.Format("<a href='#' onclick=\"escalation_Click('{0}'); return false;\">{0}</a>",
                    dataRow[ESCALATION_ID]);
                dataItem[ESCALATION_ID].Text = VeraCodeSolution.GetOutputHtmlString(url);

                // Merchant Name column
                if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MGMT)
                    || IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MGMT_MS))
                {
                    url = RiskGeneral.BuildMerchantHyperlinkInRisk((SecurePage)Page,
                        dataRow[MERCHANT_NUMBER], true, RiskReportIntruderQuery,
                        GeneralFuncsLib.NvlString(dataRow[MERCHANT_NAME]), true);
                }
                else
                {
                    url = dataRow[MERCHANT_NAME].ToString();
                }
                dataItem[MERCHANT_NAME].Text = VeraCodeSolution.GetOutputHtmlString(url);
                dataItem[MERCHANT_NAME].ToolTip = VeraCodeSolution.DoVeraCode(dataRow[MERCHANT_NUMBER].ToString());

                if (dataRow["Closed"].ToString().ToLower() == "no"
                    && dataRow["FollowupDate"] != DBNull.Value
                    && ((DateTime)dataRow["FollowupDate"]).Date <= DateTime.Now.Date)
                {
                    // Format 
                    GridColumnCollection colGrid = uxReportGrid.MasterTableView.Columns;
                    foreach (GridColumn col in colGrid)
                    {
                        if (col.Visible)
                        {
                            dataItem[col.UniqueName].Text = dataItem[col.UniqueName].Text.Replace("&nbsp;", "");                            
                            dataItem[col.UniqueName].Text = string.Format(formatValue, dataItem[col.UniqueName].Text);
                        }
                    }

                    dataItem.CssClass = GetCssEscalationQueue(e.Item.ItemIndex);
                }
            }
        }
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, ExportConfig exportConfig)
    {
        if (IsIntruderDetected)
            return;
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = uxExporterTop.GridHeader;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExporterTop.GridHeader);
    }

    protected void uxSearchButton_OnClick(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Search);
    }

    private void SetSessionValueToControl()
    {
        Session[SESSION_FILTERING_OPTIONS] = string.Format(
            "{0};{1};{2};{3};{4:MM/dd/yyyy};{5:MM/dd/yyyy};{6};{7};{8};{9};{10:MM/dd/yyyy};{11:MM/dd/yyyy};{12};{13}",
            this.AssignedToList, this.ResolutionList, this.StatusList,
            uxFilterOpenClosed.SelectedValue,
            (uxOpenCloseFromDate.SelectedDate != null && uxOpenCloseFromDate.SelectedDate.HasValue
                ? uxOpenCloseFromDate.SelectedDate.Value : DateTime.Now),
            (uxOpenCloseToDate.SelectedDate != null && uxOpenCloseToDate.SelectedDate.HasValue
                ? uxOpenCloseToDate.SelectedDate.Value : DateTime.Now),
            this.KeyType, this.KeyValue, ReasonList,
            uxFilterOption.SelectedValue == EscalationFilterOption.FOLLOWUP
                ? uxcbbFollowUp.SelectedValue : "NOTSELECT",
            uxcbbFollowUp.SelectedValue == FollowUpCode.DATE_RANGE
                ? uxDateRangeFollowupFrom.SelectedDate : DateTime.Now,
            uxcbbFollowUp.SelectedValue == FollowUpCode.DATE_RANGE
                ? uxDateRangeFollowupTo.SelectedDate : DateTime.Now,
            uxSavingsLossFrom.Value,
            uxSavingsLossTo.Value
            );
    }

    protected void btnProcess_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SelectedTicket);
    }

    private void SetDefaultValue()
    {

        if (Session[SESSION_FILTERING_OPTIONS] != null)
        {
            string[] parts = GeneralFuncsLib.NvlString(Session[SESSION_FILTERING_OPTIONS]).Split(';');

            if (parts.Length == 14)
            {
                DateTime openClosedate = DateTime.Now;

                this.AssignedToList = parts[0];
                this.ResolutionList = parts[1];
                this.StatusList = parts[2];
                uxFilterOpenClosed.SelectedValue = parts[3];
                if (DateTime.TryParse(parts[4], out openClosedate))
                {
                    uxOpenCloseFromDate.SelectedDate = openClosedate.Year == 1
                        ? DateTime.Now : openClosedate;
                }
                if (DateTime.TryParse(parts[5], out openClosedate))
                {
                    uxOpenCloseToDate.SelectedDate = openClosedate.Year == 1
                       ? DateTime.Now : openClosedate;
                }
                this.KeyType = parts[6];
                this.KeyValue = parts[7];
                this.ReasonList = parts[8];
                if (parts[9] != "NOTSELECT")
                {
                    uxFilterOption.SelectedValue = EscalationFilterOption.FOLLOWUP;
                    uxcbbFollowUp.SelectedValue = parts[9];
                }

                DateTime followdate = DateTime.Now;
                if (DateTime.TryParse(parts[10], out followdate))
                {
                    uxDateRangeFollowupFrom.SelectedDate = followdate;
                }
                if (DateTime.TryParse(parts[11], out followdate))
                {
                    uxDateRangeFollowupTo.SelectedDate = followdate;
                }
                uxSavingsLossFrom.Text = parts[12];
                uxSavingsLossTo.Text = parts[13];
            }
        }
    }

    private void BindData()
    {
        string assignedToList = string.Empty;
        string resolutionList = string.Empty;
        string statusList = string.Empty;
        string openClosedCode = string.Empty;
        DateTime openClosedFromDate = DateTime.Now;
        DateTime openClosedToDate = DateTime.Now;
        string keyType = string.Empty;
        string reasonlist = string.Empty;
        int escalationID = 0;
        string keyValue = string.Empty;

        string followUpCode = FollowUpCode.ALL;
        DateTime followFromDate = DateTime.Now;
        DateTime followToDate = DateTime.Now;
        double savingsLossFrom = 0;
        double savingsLossTo = 0;
        string corporateName = EscalationFilterOption.CNAME;
        if (GeneralFuncsLib.GetDataOfExtendedSetting("VisibleFilterCorpName").ToLower().Equals("true"))
        {
            uxFilterOption.Items.Insert(4, new AS.Controls.Global.RadComboBoxItem()
            {
                Value = EscalationFilterOption.CNAME.ToString(),
                Text = GetLocalResourceObject("RadComboBoxItemResource23.Text").ToString()
            });
        }
        if (Session[SESSION_FILTERING_OPTIONS] == null)
        {
            assignedToList = this.AssignedToList;
            resolutionList = this.ResolutionList;
            reasonlist = this.ReasonList;
            statusList = this.StatusList;
            keyType = this.KeyType;
            keyValue = (keyType.Length > 0 ? this.KeyValue : string.Empty);
            openClosedCode = uxFilterOpenClosed.SelectedValue;
            if (uxFilterOpenClosed.Attributes["xValuesReqFromToDates"].IndexOf(string.Format("[{0}]", openClosedCode)) >= 0)
            {
                openClosedFromDate = uxOpenCloseFromDate.SelectedDate != null
                    && uxOpenCloseFromDate.SelectedDate.HasValue
                    ? uxOpenCloseFromDate.SelectedDate.Value : DateTime.Now;
                openClosedToDate = uxOpenCloseToDate.SelectedDate != null
                    && uxOpenCloseToDate.SelectedDate.HasValue
                    ? uxOpenCloseToDate.SelectedDate.Value : DateTime.Now;
            }
            
            if (uxFilterOption.SelectedValue == EscalationFilterOption.TNO)
            {
                // make sure TicketNumber entered (keyValue) is a valid int
                Int32.TryParse(keyValue, out escalationID);
                keyValue = escalationID.ToString();
            }
            if (uxFilterOption.SelectedValue == EscalationFilterOption.FOLLOWUP)
            {
                keyType = EscalationFilterOption.FOLLOWUP;
                followUpCode = uxcbbFollowUp.SelectedValue;
            }
            if (uxcbbFollowUp.SelectedValue == FollowUpCode.DATE_RANGE)
            {
                followFromDate = uxDateRangeFollowupFrom.SelectedDate != null
                    && uxDateRangeFollowupFrom.SelectedDate.HasValue
                    ? uxDateRangeFollowupFrom.SelectedDate.Value : DateTime.Now;
                followToDate = uxDateRangeFollowupTo.SelectedDate != null
                    && uxDateRangeFollowupTo.SelectedDate.HasValue
                    ? uxDateRangeFollowupTo.SelectedDate.Value : DateTime.Now;
            }
            savingsLossFrom = uxSavingsLossFrom.Value != null ? (double)uxSavingsLossFrom.Value : 0;
            savingsLossTo = uxSavingsLossTo.Value != null ? (double)uxSavingsLossTo.Value : 0;
        }
        else
        {
            string[] parts = GeneralFuncsLib.NvlString(Session[SESSION_FILTERING_OPTIONS]).Split(';');

            if (parts.Length == 14)
            {
                DateTime openClosedate = DateTime.Now;

                assignedToList = parts[0];
                resolutionList = parts[1];
                statusList = parts[2];
                openClosedCode = parts[3];
                if (DateTime.TryParse(parts[4], out openClosedate))
                {
                    openClosedFromDate = (openClosedate.Year == 1 ? DateTime.Now : openClosedate);
                }
                if (DateTime.TryParse(parts[5], out openClosedate))
                    openClosedToDate = (openClosedate.Year == 1 ? DateTime.Now : openClosedate);
                keyType = parts[6];
                keyValue = parts[7];
                reasonlist = parts[8];
                followUpCode = parts[9];
                DateTime.TryParse(parts[10], out followFromDate);
                DateTime.TryParse(parts[11], out followToDate);
                double.TryParse(parts[12], out savingsLossFrom);
                double.TryParse(parts[13], out savingsLossTo);
            }
        }

        string spName = "spa_RM_MCF_GetEscalation";
        string methodName = "GetReports";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignedToList", assignedToList, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ReasonList", reasonlist, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ResolutionList", resolutionList, DbType.String));
        parameters.Add(new FilterParameter("@StatusList", statusList, DbType.String));
        parameters.Add(new FilterParameter("@OpenClosedCode", openClosedCode, DbType.String));
        parameters.Add(new FilterParameter("@OpenClosedFromDate", openClosedFromDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@OpenClosedToDate", openClosedToDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@KeyType", keyType, DbType.String));
        parameters.Add(new FilterParameter("@KeyValue", keyValue, DbType.String));
        parameters.Add(new FilterParameter("@FollowUpCode", followUpCode, DbType.String));
        parameters.Add(new FilterParameter("@FollowUpFromDate", followFromDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@FollowUpToDate", followToDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@SavingsLossAmtFrom", savingsLossFrom, DbType.Currency));
        parameters.Add(new FilterParameter("@SavingsLossAmtTo", savingsLossTo, DbType.Currency));

        uxReportGrid.DataSourceInvoker = new ASFuncInvoker(
            WebServices.RiskServices,
            methodName,
            new object[] { spName, ReportServices.ConvertToFilterParamWSArray(parameters) });
    }

    private string GetCssEscalationQueue(int index)
    {
        return index % 2 == 0 ? "rgRow highlighted" : "rgAltRow highlighted";
    }

    #endregion Methods
}
