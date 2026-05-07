using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Controls.Pages;
using System.Text.RegularExpressions;
using AS.Web.UI.Controls;

[PagePermission("ManCase,MSManCase")]
public partial class CaseSearch : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindDataToUserList,
        BindDataToStatusList,
        BindDataToResolutionList,
        BindDataSearchDate,
        BindTicketList
    }
    enum PostBackAction
    {
        Search,
        CreateTicket
        //ChooseTicketInGrid,
        //ChooseMerchantInGrid
    }
    #endregion

    #region properties and member
    private const string SESSION_FILTERING_OPTIONS = "CaseManagementFilteringOptions";
    private int _SiteID
    {
        get
        {
            if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
                return SessionManager.CurrentRiskSiteID;
            else
                return SessionManager.CurrentUser.SiteID;
        }
    }
    #endregion
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        
        if (!IsPostBack)
        {
            if (SavedReportFilterValue != null && GeneralFuncsLib.IsMerchantMode(SavedReportFilterValue.HierarchyMode) && !string.IsNullOrEmpty(SavedReportFilterValue.Value))
            {
                uxFilterNone.Checked = false;
                uxFilterTicketNumber.Checked = false;
                uxFilterMerchantName.Checked = false;
                uxFilterMerchantNumber.Checked = true;
                uxFilterSearchKeyText.Text = SavedReportFilterValue.Value;
            }
            else
            {
                uxFilterSearchKeyText.Text = string.Empty;
            }
            SetSessionValueToControl();
            CheckPermissionForIssueMaintenance();
            OnDataBindControls(DataBindAction.BindDataToUserList);
            OnDataBindControls(DataBindAction.BindDataToStatusList);
            OnDataBindControls(DataBindAction.BindDataToResolutionList);
            OnDataBindControls(DataBindAction.BindDataSearchDate);
            SetDefaultValue();
            //SetSessionValueToControl();
        }
    }

    #region override methods
    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;
        this.ExporterIDs.Add("uxExporterTop");
        this.ExporterIDs.Add("uxExporterBottom");
        base.PageInitialize();
        IsBindDataOnLoad = true;
        this.IsSecureCSRF = true;
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(GetLocalResourceObject("CaseSeach_aspx_cs_TicketList").ToString());
        exportConfig.ReportHeader = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("CaseSearch_aspx_cs_CaseManagementTicketList").ToString());
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (sender == uxReportGrid)
        {
            BindData();
        }
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (sender == uxReportGrid)
        {
            const string TICKET_NUMBER = "TicketNumber";
            const string MERCHANT_NUMBER = "MerchantNumber";

            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView rowView = e.Item.DataItem as DataRowView;

            if (e.Item is GridDataItem)
            {
                string str = this.BuildSecureQueryString("TicketNumber=" + rowView[TICKET_NUMBER] + "&MerchantNumber=" + rowView[MERCHANT_NUMBER] + "&SiteID=" + SessionManager.CurrentUser.SiteID + this.CaseManagementIntruderQuery);
                string url = string.Format("<a class='link' href='CaseManagement.aspx?{0}' style = 'cursor:pointer'>{1}</a>", str, rowView[TICKET_NUMBER]);
                dataItem["TicketNumber"].Text = VeraCodeSolution.GetOutputHtmlString(url);
                string queryString = BuildSecureQueryString(string.Format("MerchantNumber={0}", rowView[MERCHANT_NUMBER]));
                url = string.Format("<a class=\"link\" href='MerchantProfile.aspx?{0}' style=\"cursor:pointer\" >{1}</a>", queryString, rowView[MERCHANT_NUMBER]);

                dataItem["MerchantNumber"].Text = VeraCodeSolution.GetOutputHtmlString(url);
            }
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDataToUserList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@Mode", "ASSIGNED", DbType.String));
                    parameters.AddLanguageID();
                    uxFilterAssignedToList.DataSource = WebServices.RiskServices.GetReports("spa_cm_GetRefValueCaseManagement", parameters);
                    uxFilterAssignedToList.DataBind();
                }
                break;
            case DataBindAction.BindDataToStatusList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@Mode", "STATUS", DbType.String));
                    parameters.AddLanguageID();
                    uxFilterStatusList.DataSource = WebServices.RiskServices.GetReports("spa_cm_GetRefValueCaseManagement", parameters);
                    uxFilterStatusList.DataBind();
                }
                break;
            case DataBindAction.BindDataToResolutionList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@Mode", "RESOLUTION", DbType.String));
                    parameters.AddLanguageID();
                    DataTable dt = WebServices.RiskServices.GetReports("spa_cm_GetRefValueCaseManagement", parameters);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //Delete Special Row with Text = string.empty
                        if (dt.Rows[i]["KeyValue"].ToString().Trim() == string.Empty)
                            dt.Rows[i].Delete();
                    }
                    uxFilterResolutionList.DataSource = dt;
                    uxFilterResolutionList.DataBind();
                }
                break;
            case DataBindAction.BindDataSearchDate:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@Mode", "DATESEARCH", DbType.String));
                    parameters.AddLanguageID();
                    DataTable dt = WebServices.RiskServices.GetReports("spa_cm_GetRefValueCaseManagement", parameters);
                    uxFilterOpenClosed.DataSource = dt;
                    uxFilterOpenClosed.DataBind();
                }
                break;
            case DataBindAction.BindTicketList:
                {
                    BindData();
                }
                break;
            default: break;
        }
    }
    #region Validate data
    private bool ValidateData()
    {
        string filterSearch = uxFilterSearchKeyText.Text.Trim();

        if (uxFilterNone.Checked)
            return true;

        if (filterSearch.IsNullOrEmpty())
            return false;

        if (uxFilterTicketNumber.Checked)
            return GeneralFuncsLib.IsValidWithRegularExpression("[0-9]{1,9}", filterSearch);

        if (uxFilterMerchantNumber.Checked)
            return GeneralFuncsLib.IsValidWithRegularExpression("[0-9]{1,16}", filterSearch);

        if (uxFilterMerchantName.Checked)
            return GeneralFuncsLib.IsValidWithRegularExpression("[ ,.A-Za-z0-9]{1,55}", filterSearch);

        return true;
    }
    #endregion
    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.Search:
                {
                    if (!ValidateData())
                    {
                        IsIntruderDetected = true;
                        IntruderLog.LogData4 += "&uxFilterSearchKeyText=" + uxFilterSearchKeyText.Text.Trim();
                        RaiseIntruderEvent(IntruderType.PostData);
                        return;
                    }
                    if (uxFilterMerchantNumber.Checked)
                    {
                        HierarchyFilterValue reportFiler = new HierarchyFilterValue();// SavedReportFilterValue;

                        if (SavedReportFilterValue != null)
                        {
                            reportFiler.DateOption = SavedReportFilterValue.DateOption;// DateOptionMode.DateRange;
                            reportFiler.DateOptionValue.From = SavedReportFilterValue.DateOptionValue.From;// DateTime.Now.GetFirstDayOfMonth();
                            reportFiler.DateOptionValue.To = SavedReportFilterValue.DateOptionValue.To;// DateTime.Now;
                            reportFiler.HierarchyMode = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;// GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;
                            reportFiler.ID = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;// GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;
                            reportFiler.Value = uxFilterSearchKeyText.Text;// string.Empty;                            
                        }
                        else
                        {
                            reportFiler = new HierarchyFilterValue();
                            reportFiler.DateOption = DateOptionMode.DateRange;
                            reportFiler.DateOptionValue.From = DateTime.Now.GetFirstDayOfMonth();
                            reportFiler.DateOptionValue.To = DateTime.Now;
                            reportFiler.HierarchyMode = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;
                            reportFiler.ID = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;
                            reportFiler.Value = uxFilterSearchKeyText.Text;
                        }
                        SavedReportFilterValue = new HierarchyFilterValue();
                        SavedReportFilterValue = reportFiler;
                    }
                    SetSessionValueToControl();
                    uxReportGrid.CurrentPageIndex = 0;
                    uxReportGrid.Rebind();
                }
                break;
            case PostBackAction.CreateTicket:
                {
                    AjaxAddResponseScript("ShowPopupModal('OpenNewTicket.aspx','auto');");
                }
                break;
            //case PostBackAction.ChooseTicketInGrid:
            //case PostBackAction.ChooseMerchantInGrid:
            //    {
            //        string[] parms = hddProcessData.Value.Split(';');

            //        if (parms.Length > 1)
            //        {
            //            if (string.Compare(parms[0], "ticket") == 0)//choose ticketnumber
            //            {
            //                string queryStr = parms[1];
            //                Response.Redirect("CaseManagement.aspx?" + queryStr, true);
            //            }
            //            else if (string.Compare(parms[0], "merchant") == 0)//choose merchantnumber
            //            {
            //                string merchantNumber = parms[1];
            //                string queryString = BuildSecureQueryString(string.Format("MerchantNumber={0}", merchantNumber));
            //                Response.Redirect("MerchantProfile.aspx?" + queryString, true);
            //            }
            //        }
            //    }
            //    break;
            default: break;
        }
    }
    #endregion

    #region methods
    /// <summary>
    /// Check if user have right to access IssuManintenance function
    /// </summary>
    private void CheckPermissionForIssueMaintenance()
    {
        uxIssueMaintenance.Visible = IsUserWithPermission("ManIssue") | IsUserWithPermission("MSManIssue");
    }

    private void SetDefaultValue()
    {
        if (Session[SESSION_FILTERING_OPTIONS] != null)
        {
            string[] parts = GeneralFuncsLib.NvlString(Session[SESSION_FILTERING_OPTIONS]).Split(';');

            if (parts.Length == 8)
            {
                DateTime openClosedate = DateTime.Now;
                this.AssignedToList = parts[0];
                this.ResolutionList = parts[1];
                this.StatusList = parts[2];
                uxFilterOpenClosed.SelectedValue = parts[3];
                if (DateTime.TryParse(parts[4], out openClosedate))
                    uxOpenCloseFromDate.SelectedDate = (openClosedate.Year == 1 ? DateTime.Now : openClosedate);
                if (DateTime.TryParse(parts[5], out openClosedate))
                    uxOpenCloseToDate.SelectedDate = (openClosedate.Year == 1 ? DateTime.Now : openClosedate);

                this.KeyType = parts[6];
                this.KeyValue = parts[7];
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
        int ticketNumber = 0;
        string keyValue = string.Empty;
        if (Session[SESSION_FILTERING_OPTIONS] == null)
        {
            assignedToList = this.AssignedToList;
            resolutionList = this.ResolutionList;
            statusList = this.StatusList;
            keyType = this.KeyType;
            keyValue = (keyType.Length > 0 ? this.KeyValue : string.Empty);
            openClosedCode = uxFilterOpenClosed.SelectedValue;
            if (uxFilterOpenClosed.Attributes["xValuesReqFromToDates"].IndexOf(string.Format("[{0}]", openClosedCode)) >= 0)
            {
                openClosedFromDate = (uxOpenCloseFromDate.SelectedDate != null && uxOpenCloseFromDate.SelectedDate.HasValue ? uxOpenCloseFromDate.SelectedDate.Value : DateTime.Now);
                openClosedToDate = (uxOpenCloseToDate.SelectedDate != null && uxOpenCloseToDate.SelectedDate.HasValue ? uxOpenCloseToDate.SelectedDate.Value : DateTime.Now);
            }

            if (uxFilterTicketNumber.Checked)
            {
                // make sure TicketNumber entered (keyValue) is a valid int
                Int32.TryParse(keyValue, out ticketNumber);
                keyValue = ticketNumber.ToString();
            }
        }
        else
        {
            string[] parts = GeneralFuncsLib.NvlString(Session[SESSION_FILTERING_OPTIONS]).Split(';');

            if (parts.Length == 8)
            {
                DateTime openClosedate = DateTime.Now;

                assignedToList = parts[0];
                resolutionList = parts[1];
                statusList = parts[2];

                openClosedCode = parts[3];
                if (DateTime.TryParse(parts[4], out openClosedate))
                    openClosedFromDate = (openClosedate.Year == 1 ? DateTime.Now : openClosedate);
                if (DateTime.TryParse(parts[5], out openClosedate))
                    openClosedToDate = (openClosedate.Year == 1 ? DateTime.Now : openClosedate);
                keyType = parts[6];
                keyValue = parts[7];
            }

        }

        string spName = "spa_cm_GetTicketsListCaseManagement";
        string methodName = "GetReports";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignedToList", assignedToList, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ResolutionList", resolutionList, DbType.String));
        parameters.Add(new FilterParameter("@StatusList", statusList, DbType.String));
        parameters.Add(new FilterParameter("@OpenClosedCode", openClosedCode, DbType.String));
        parameters.Add(new FilterParameter("@OpenClosedFromDate", openClosedFromDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@OpenClosedToDate", openClosedToDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@KeyType", keyType, DbType.String));
        parameters.Add(new FilterParameter("@KeyValue", keyValue.Replace('*', '%'), DbType.String));
        parameters.AddLanguageID();
        uxReportGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, methodName, new object[] { spName, ReportServices.ConvertToFilterParamWSArray(parameters) });
    }

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
            if (string.IsNullOrEmpty(value)) return;

            string list = string.Format(",{0},", value);
            foreach (RadListBoxItem item in uxFilterAssignedToList.Items)
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
            if (string.IsNullOrEmpty(value)) return;

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
            if (string.IsNullOrEmpty(value)) return;

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
            foreach (Control ctl in uxFilterContainer.Controls)
            {
                if (ctl is RadioButton)
                {
                    RadioButton radioButton = ctl as RadioButton;

                    if (radioButton.Checked)
                    {
                        return radioButton.Attributes["xKeyType"];
                    }
                }
            }

            return string.Empty;
        }

        set
        {
            foreach (Control ctl in uxFilterContainer.Controls)
            {
                if (ctl is RadioButton)
                {
                    RadioButton radioButton = ctl as RadioButton;

                    if (string.Compare(radioButton.Attributes["xKeyType"], value) == 0)
                        radioButton.Checked = true;
                    else
                        radioButton.Checked = false;
                }
            }
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


    string _CaseManagementIntruderQuery = string.Empty;
    private string CaseManagementIntruderQuery
    {
        get
        {
            if (_CaseManagementIntruderQuery.Length == 0)
                _CaseManagementIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { "TicketNumber" });
            return _CaseManagementIntruderQuery;
        }
    }

    string _MerchantProfileIntruderQuery = string.Empty;
    private string MerchantProfileIntruderQuery
    {
        get
        {
            if (_MerchantProfileIntruderQuery.Length == 0)
                _MerchantProfileIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { "MerchantNumber" });
            return MerchantProfileIntruderQuery;
        }
    }

    //protected void btnProcess_Click(object sender, EventArgs e)
    //{
    //    OnPostBackActions(PostBackAction.ChooseTicketInGrid);
    //}

    protected void uxSearchButton_OnClick(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Search);
    }

    protected void uxOpenNewTicket_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CreateTicket);
    }

    private void SetSessionValueToControl()
    {
        Session[SESSION_FILTERING_OPTIONS] = string.Format("{0};{1};{2};{3};{4:MM/dd/yyyy};{5:MM/dd/yyyy};{6};{7}",
                   this.AssignedToList, this.ResolutionList, this.StatusList, uxFilterOpenClosed.SelectedValue,
                   (uxOpenCloseFromDate.SelectedDate != null && uxOpenCloseFromDate.SelectedDate.HasValue ? uxOpenCloseFromDate.SelectedDate.Value : DateTime.Now),
                   (uxOpenCloseToDate.SelectedDate != null && uxOpenCloseToDate.SelectedDate.HasValue ? uxOpenCloseToDate.SelectedDate.Value : DateTime.Now),
                   this.KeyType, this.KeyValue);
    }
    #endregion
}
