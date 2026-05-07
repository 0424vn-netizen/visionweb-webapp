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
using AS.Common;
using AS.Controls.Pages;
using AS.Controls.Grid;
using AS.Common.DBManager;
using Telerik.Web.UI;
using AS.Controls.UserControls;
using AS.Controls.Exporter;
using AS.Web.Business;
using System.Collections.Generic;

[PagePermission("RskWatch,MSRskWatch")]
public partial class rm_MCF_MultiMerchantWatch : ReportPage
{
    private const string DATA_KEY = "DataKey";
    private const string DATA_TEXT = "DataText";
    private const string SELECTABLE = "Selectable";
    private const string CHILD_OF = "ChildOf";

    enum DataBindAction
    {
        BindMultiWatchTypeGrid,
        BindMerchantOnWatchGrid,
        BindFilterType

    }

    enum PostBackAction
    {
        ProcessEvent,
        RefreshEvent,
        SubmitHierarchyEvent,
        GridWatchGroupItemCommand,
        FilterTypeSelectedIndexChanged,
        SearchValueOnFilterType
    }

    #region Properties
    private const string MODE = "Mode";
    private const string FILTER_TYPE = "FilterType";
    private const string FILTER_VALUE = "FilterValue";
    private const string FILTER_VALUE_TEXT = "FilterValueText";
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string MERCHANT_NAME = "MerchantName";

    private const string FILTER_TYPE_QUERTY = "FilterType";
    private const string FILTER_VALUE_QUERTY = "FilterValue";
  
    private string _CurrentSortExpr = string.Empty;
    private string _CurrentSortOrder = string.Empty;
    private string _CurrentSortControls = string.Empty;


    private bool hasScroll;
    private string _searchValue = string.Empty;

    private WebSiteEnums.WatchFilterTypes _currentFilterMode;
    public WebSiteEnums.WatchFilterTypes CurrentFilterMode
    {
        get { return (ViewState["CurrentViewState"] == null ? WebSiteEnums.WatchFilterTypes.NONE : (WebSiteEnums.WatchFilterTypes)ViewState["CurrentViewState"]); }
        set { ViewState["CurrentViewState"] = value; }
    }

    WebSiteEnums.WatchFilterTypes _FilterTypes = WebSiteEnums.WatchFilterTypes.NONE;

    public string EntityHierarchyMode
    {
        get { return (ViewState["EntityHierarchyMode"] == null ? string.Empty : (string)ViewState["EntityHierarchyMode"]);  }
        set { ViewState["EntityHierarchyMode"] = value; }
    }

    public string EntityHierarchyValue
    {
        get { return (ViewState["EntityHierarchyValue"] == null ? string.Empty : (string)ViewState["EntityHierarchyValue"]); }
        set { ViewState["EntityHierarchyValue"] = value; }
    }
    public string ModeValue
    {
        get { return (ViewState[MODE] == null ? string.Empty : (string)ViewState[MODE]); }
        set { ViewState[MODE] = value; }
    }

    public string FilterType
    {
        get { return (ViewState[FILTER_TYPE] == null ? "ALL" : (string)ViewState[FILTER_TYPE]); }
        set { ViewState[FILTER_TYPE] = value; }
    }

    public string FilterValue
    {
        get { return (ViewState[FILTER_VALUE] == null ? string.Empty : (string)ViewState[FILTER_VALUE]); }
        set { ViewState[FILTER_VALUE] = value; }
    }

   

    string _RiskReportIntruderQuery = string.Empty;
    private string RiskReportIntruderQuery
    {
        get
        {
            if (_RiskReportIntruderQuery.Length == 0)
                _RiskReportIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { FILTER_TYPE, FILTER_VALUE });
            return _RiskReportIntruderQuery;
        }
    }

    private DataTable  _SearchLenghtInfo = new DataTable();
    public DataTable SearchLenghtInfo
    {
        get
        {
            
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams(false);
                DataTable dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetWatchFilterTypesLengthSearching", parameters);

                Session[this.ClientID + "_SearchLenghtInfo"] = dt;
            
            return (DataTable)Session[this.ClientID + "_SearchLenghtInfo"];
        }
        set
        {
            Session[this.ClientID + "_SearchLenghtInfo"] = value;
        }
    }

    #endregion

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;

        InitializeGridColumn();

        this.GridIDs.Add("uxGrid");
        this.GridIDs.Add("uxReportGrid");
        this.ExporterIDs.Add("uxExport");       
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        BuildScriptFilterTypeList();
        IsBindDataOnLoad = true;
        uxReportGrid.IsIntruder = true;
        uxReportGrid.IntruderSourceName = GeneralFuncsLib.GetRequestFileName() + uxReportGrid.ID;

        if (!IsPostBack)
        {
            //set default filter for MS user
            switch (SessionManager.CurrentUserType)
            {
                case WebSiteEnums.UserHierarchyMode.Hierarchy:
                case WebSiteEnums.UserHierarchyMode.Headquarter:
                    {
                        string HierarchyFilterMode = GeneralFuncsLib.GetHierarchyInfo(SessionManager.CurrentUser.EntityType).HierarchyMode;
                        string EntityID = SessionManager.CurrentUser.EntityID;

                        //DEFAULT
                        string HierarchyFilterValue = EntityID;
                        //special cases
                        if (HierarchyFilterMode == HierarchyMode.HEADQUARTER)
                        {

                            FilterParameterCollection pIn = new FilterParameterCollection();
                            pIn.AddLoggedInUserReportingParams(false);
                            HierarchyFilterValue = WebServices.RiskServices.GetRealHierarchyFilterValue(
                                pIn, HierarchyFilterMode, EntityID, true);
                        }
                        //ends

                        EntityHierarchyMode = HierarchyFilterMode;
                        EntityHierarchyValue = HierarchyFilterValue;
                    }
                    break;
            }



            FilterType = WebSiteEnums.WatchFilterTypes.ALL.ToString();
            OnDataBindControls(DataBindAction.BindFilterType);  
            CurrentFilterMode = WebSiteEnums.WatchFilterTypes.ALL;

            RebindWatchTypeGrid();
            //GetLenghtSearching();
            if (base.SecureQueryString != null)
            {
                string filterValue = base.SecureQueryString[FILTER_VALUE_QUERTY];

                string filterType = base.SecureQueryString[FILTER_TYPE_QUERTY];

                if (!filterType.IsNullOrEmpty() && filterValue.Length > 0)
                {
                   
                    FilterType = filterType;
                    FilterValue = filterValue;
                }
            }
            
            RebindMerchantOnWatchGrid();            
        }
        AsContainer1.Visible = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);

    }

    protected void BuildScriptFilterTypeList()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);

        DataTable FilterTypeList = WebServices.RiskServices.GetReports("spa_RM_MCF_GetWatchFilterTypes", parameters);

        string script = "var FilterTypeList = [ { Type: \"0\", TypeName: \"\", Prompt: \"\" }";
    

        foreach (DataRow row in FilterTypeList.Rows)
        {
            script += " ,{ Type: \"" + row["DataKey"].ToString() + "\", TypeName: \"" + row["DataText"].ToString() + "\", Prompt: \"" + GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String1").ToString() + " " + row["DataText"].ToString() + ":\" }";
        }
        script += "];";
       
        ClientScript.RegisterStartupScript(this.GetType(), "ScriptFilterTypeList", script, true);
                    
    }

    protected void InitializeGridColumn()
    {
       
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.AddLanguageID();
        DataTable info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetHierarchyInfoOnGridWatchList", parameters);
        SessionManager.RiskHierarchyMultiwatchGird = info;


        //initialize cols
        
        foreach (DataRow row in SessionManager.RiskHierarchyMultiwatchGird.Rows)
        {
            ASGridBoundColumn boundColumn = new ASGridBoundColumn();
            boundColumn.DataField = row["HierarchyGridDataField"].ToString();
            boundColumn.UniqueName = row["HierarchyGridDataField"].ToString();
            boundColumn.HeaderText = row["HierarchyGridHeaderText"].ToString();
            boundColumn.HeaderTooltip = row["HierarchyHeaderTooltip"].ToString();
            boundColumn.ASFormat = FormatType.StaticString;
            boundColumn.SortExpression = row["HierarchyGridDataField"].ToString();
            
            if (boundColumn.DataField.Equals("SIC", StringComparison.OrdinalIgnoreCase))
            {
                boundColumn.HeaderStyle.Width = new Unit(80, UnitType.Pixel);
            }
            else
            {
                boundColumn.HeaderStyle.Width = new Unit(130, UnitType.Pixel);
            }

            if (!((boundColumn.DataField.Equals("SYS", StringComparison.OrdinalIgnoreCase)
                || boundColumn.DataField.Equals("SYSPRIN", StringComparison.OrdinalIgnoreCase)) 
                && SessionManager.CurrentClient.Equals(WebSiteConstants.IPMT_CLIENT)))
            uxReportGrid.MasterTableView.Columns.Add(boundColumn);
        }
        

        ASGridBoundColumn AddressCol = new ASGridBoundColumn();
        AddressCol.DataField = "Address";
        AddressCol.UniqueName = "Address";
        AddressCol.HeaderText = GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String2").ToString();
        AddressCol.HeaderTooltip = GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String2").ToString();
        AddressCol.ASFormat = FormatType.DynamicString;
        AddressCol.SortExpression = "Address";
        AddressCol.HeaderStyle.Width = 200;
        uxReportGrid.MasterTableView.Columns.Add(AddressCol);
    }
    

    protected void btnProcess_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ProcessEvent);
    }

    protected void uxFilterType_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        OnPostBackActions(PostBackAction.FilterTypeSelectedIndexChanged, e);
    }

    protected void uxSubmit_OnClick(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SubmitHierarchyEvent);
    }
   

    protected void uxReloadGrids_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.RefreshEvent);
    }

    protected override void DoSwitchView()
    {

    }


    #region Grid events
    protected override void DoGridDataSourceReady(ASGrid sender, EventArgs e)
    {
        if (sender == uxReportGrid)
        {
            if ((uxReportGrid.AS_DataSource).Rows.Count == 0)
            {
                uxReportGrid.AllowSorting = false;
            }
            else
            {
                uxReportGrid.AllowSorting = true;
            }
        }

    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid)
        {
            OnDataBindControls(DataBindAction.BindMerchantOnWatchGrid, sender);
        }
        else if (sender == uxGrid)
        {
            OnDataBindControls(DataBindAction.BindMultiWatchTypeGrid, sender);
        }

    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (sender == uxGrid)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;

                string queryString = this.BuildSecureQueryString(string.Format("{0}={1}&{2}={3}{4}",
                    FILTER_VALUE, dataRow[FILTER_VALUE],
                    FILTER_TYPE, dataRow[FILTER_TYPE], this.RiskReportIntruderQuery));
                string url = string.Format("<a class=\"link\" href=\"#\" onclick=\"loadList('" + dataRow[FILTER_TYPE] + "','" + dataRow[FILTER_VALUE] + "'); return false;\">", queryString);
                LinkButton uxRemoveCommand = e.Item.FindControl("uxRemove") as LinkButton;
                
                string filterType = dataRow[FILTER_TYPE].ToString();

                dataItem[FILTER_VALUE_TEXT].Text = VeraCodeSolution.GetOutputHtmlString(url + dataRow[FILTER_VALUE_TEXT].ToString() + "</a>");

                if ( filterType != "ALL" && filterType != "MERCHANT" && filterType != "NONE")
                {
                    uxRemoveCommand.CommandArgument = VeraCodeSolution.DoVeraCode(string.Format("{0}|{1}|{2}", dataRow[FILTER_TYPE].ToString(), dataRow[FILTER_TYPE].ToString(), dataRow[FILTER_VALUE]));
                }
                else
                {
                    uxRemoveCommand.Visible = false;
                }

            }
        }
        else if (sender == uxReportGrid)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;

                string url = RiskGeneral.BuildMerchantHyperlinkInRisk((SecurePage)Page,
                    dataRow[MERCHANT_NUMBER], true, RiskReportIntruderQuery,
                    GeneralFuncsLib.NvlString(dataRow[MERCHANT_NAME]), true);

                dataItem[MERCHANT_NAME].Text = VeraCodeSolution.GetOutputHtmlString(url);
                dataItem[MERCHANT_NAME].ToolTip = VeraCodeSolution.DoVeraCode(dataRow[MERCHANT_NUMBER].ToString());            
            }
        }
    }

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        if (sender == uxExport)
        {
            base.DoNeedExportConfig(sender, exportConfig);
            exportConfig.FileName = GeneralFuncsLib.GetLegalFileName(exportConfig.ReportHeader);
            uxReportGrid.Columns.FindByUniqueName("MerchantNumber").Visible = false;
        }
    }

    #endregion

    protected override void OnPostBackActions(Enum type, object param)
    {
        if (this.IsIntruderDetected) return;

        switch ((PostBackAction)type)
        {
            case PostBackAction.ProcessEvent:
                {
                    string[] parms = hddProcessData.Value.Split(';');

                    if (parms.Length > 1)
                    {
                        if (parms[0] == "merchant")
                        {
                            string merchantNumber = parms[1];

                            RiskSessionManager.RiskReportReferrer = "MultiMerchantWatch";
                            RiskSessionManager.RiskReportReferrerInfo = new ReferrerInfo(merchantNumber, "rm_MCF_MultiMerchantWatch.aspx", "Multi-Merchant Watch");


                            string queryString = this.BuildSecureQueryString(string.Format("{0}={1}{2}",
                                MERCHANT_NUMBER, merchantNumber, this.RiskReportIntruderQuery));

                            Response.Redirect("~/risk_MCF/rm_MCF_RiskReport.aspx?" + queryString, true);
                        }
                        else
                        {
                            if (parms[0] == "loadList")
                            {
                                FilterType = parms[1];

                                FilterValue = parms[2];
                                if (FilterValue != string.Empty)
                                {
                                    uxExport.GridHeader = VeraCodeSolution.ValidateResponseData(FilterValue + " - " + GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String3").ToString());
                                    uxExport.GridTitle = GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String3").ToString()+" - ";
                                    uxExport.GridSubTitle = VeraCodeSolution.ValidateResponseData(FilterValue);
                                }
                                else
                                {
                                    uxExport.GridHeader =GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String3").ToString()+ " ";
                                    uxExport.GridTitle = GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String3").ToString()+" ";
                                }
                                uxReportGrid.CurrentPageIndex = 0;
                                uxReportGrid.Rebind();
                            }
                            else
                            {
                                if (parms[0] == "loadListByMode")
                                {
                                    FilterValue = "%";
                                    uxExport.GridSubTitle = string.Empty;
                                    uxReportGrid.CurrentPageIndex = 0;
                                    uxReportGrid.Rebind();
                                }
                            }
                        }
                    }
                }
                break;
            case PostBackAction.RefreshEvent:
                {
                    uxGrid.Rebind();
                    uxReportGrid.Rebind();
                }
                break;
            case PostBackAction.SubmitHierarchyEvent:
                {
                   
                    string filterValue = string.Empty;

                    string watchFilterType = uxFilterType.SelectedValue.ToString();
                    CurrentFilterMode = WebSiteEnums.WatchFilterTypes.ALL;
                    filterValue = uxSearchValueCommonKey.SelectedValue;
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.Add(new FilterParameter("@FilterType", watchFilterType, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@FilterValue", filterValue, DbType.String));
                    parameters.Add(new FilterParameter("@EntityHierarchyMode", EntityHierarchyMode, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@EntityHierarchyValue", EntityHierarchyValue, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@IsAdd", true, DbType.Boolean, true));
                    FilterParameterCollection parameterOut = new FilterParameterCollection();
                    WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_AddToWatch", parameters, out parameterOut);
                    // Save UserActivity
                    string activityText = string.Format(GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String4").ToString(), watchFilterType, filterValue);
                    GeneralFuncsLib.SaveUserActivity("", activityText, true);

                    if (!(bool)parameterOut[0].ParameterValue)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myScript", "$(document).ready(function(){alert('" + GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String5").ToString() + "');});", true);
                    }
    
                    FilterValue = "%";
                   
                    CurrentFilterMode = WebSiteEnums.WatchFilterTypes.ALL;
                    FilterType = WebSiteEnums.WatchFilterTypes.ALL.ToString();

                    RebindWatchTypeGrid();
                    RebindMerchantOnWatchGrid();

                    uxFilterType.Items[0].Selected = true;
                }
                break;
            case PostBackAction.GridWatchGroupItemCommand:
                {
                    GridCommandEventArgs e = (GridCommandEventArgs)param;

                    if (string.Compare(e.CommandName, "Remove") == 0)
                    {
                        string[] filterData = e.CommandArgument.ToString().Split('|');
                        string filterType = filterData[0];

                        if (filterData.Length == 3 && !filterType.IsNullOrEmpty())
                        {
                            FilterParameterCollection parameters = new FilterParameterCollection();
                            parameters.AddLoggedInUserReportingParams(false);
                            parameters.Add(new FilterParameter("@FilterType", filterData[1], DbType.AnsiString));
                            parameters.Add(new FilterParameter("@FilterValue", filterData[2], DbType.String));
                            parameters.Add(new FilterParameter("@EntityHierarchyMode", EntityHierarchyMode, DbType.AnsiString));
                            parameters.Add(new FilterParameter("@EntityHierarchyValue", EntityHierarchyValue, DbType.AnsiString));

                            FilterParameterCollection parameterOut = new FilterParameterCollection();
                            WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_RemoveFromWatch", parameters, out parameterOut);
                            uxGrid.Rebind();
                            // Save UserActivity
                            string activityText = string.Format(GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String9").ToString(), filterData[1], filterData[2]);
                            GeneralFuncsLib.SaveUserActivity("", activityText, true);
                            //

                            AjaxAddResponseScript("loadListByMode('" + _FilterTypes.ToString() + "');");
                        }
                    }
                }
                break;
            case PostBackAction.FilterTypeSelectedIndexChanged:
                {
                    RadComboBoxSelectedIndexChangedEventArgs e = (RadComboBoxSelectedIndexChangedEventArgs)param;
                    WebSiteEnums.WatchFilterTypes watchFilterType = WebSiteEnums.WatchFilterTypes.NONE;

                    string HierarchyFilter = e.Value.ToString();

                    if (HierarchyFilter != WebSiteEnums.WatchFilterTypes.NONE.ToString() &&
                        HierarchyFilter != WebSiteEnums.WatchFilterTypes.ALL.ToString() &&
                        HierarchyFilter != WebSiteEnums.WatchFilterTypes.MERCHANT.ToString() &&
                        HierarchyFilter != WebSiteEnums.WatchFilterTypes.SELECT_ONE.ToString())
                    {
                        watchFilterType = WebSiteEnums.WatchFilterTypes.HIERARCHY;
                    }
                    else
                    {
                        watchFilterType = (WebSiteEnums.WatchFilterTypes)Enum.Parse(typeof(WebSiteEnums.WatchFilterTypes), HierarchyFilter, true);
                    }
                    uxSearchValueCommonKey.ClearSelection();
                    uxSearchValueCommonKey.Items.Clear();
                    uxSearchValueCommonKey.Text = string.Empty;
                  
                    uxSearchValueCommonKey.Width = (HierarchyFilter == "SIC" ? new Unit("400px") : new Unit("350px"));
                  
                    CurrentFilterMode = WebSiteEnums.WatchFilterTypes.ALL;

                    switch (watchFilterType)
                    {
                        case WebSiteEnums.WatchFilterTypes.ALL:
                            Response.Redirect("rm_MCF_MultiMerchantWatch.aspx");
                            break;
                        case WebSiteEnums.WatchFilterTypes.MERCHANT:
                            FilterValue = "%";
                            FilterType = WebSiteEnums.WatchFilterTypes.MERCHANT.ToString();
                            CurrentFilterMode = WebSiteEnums.WatchFilterTypes.MERCHANT;
                            RebindWatchTypeGrid();
                            RebindMerchantOnWatchGrid();
                            break;
                        case WebSiteEnums.WatchFilterTypes.NONE:
                        case WebSiteEnums.WatchFilterTypes.SELECT_ONE:
                            break;
                        case WebSiteEnums.WatchFilterTypes.HIERARCHY:
                            int length = int.Parse(SearchLenghtInfo.FindObject("DataKey", HierarchyFilter)["MinCharactersSearch"].ToString());
                           
                            if (length != 0)
                                uxSearchValueCommonKey.EmptyMessage = GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String6").ToString() + " " + length.ToString() + " " + GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String7").ToString();
                            else
                                uxSearchValueCommonKey.EmptyMessage = GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String8").ToString();

                            BindDataIntoComboSearchValue(length, HierarchyFilter);
                            break;
                        default:
                            BindDataIntoComboSearchValue(0, HierarchyFilter);
                            break;
                    }
                    this.AjaxAddResponseScript("addCheckSpecialCharacters();");
                }
                break;
            case PostBackAction.SearchValueOnFilterType://searching when fill some characters
                {
                    string filterType = string.Empty;
                    filterType = uxFilterType.SelectedValue;

                    if (_searchValue.Length >= int.Parse(SearchLenghtInfo.FindObject("DataKey", filterType)["MinCharactersSearch"].ToString()))
                    {
                        FilterParameterCollection parameters = new FilterParameterCollection();
                        parameters.AddLoggedInUserReportingParams(false);
                        parameters.AddLanguageID();
                        parameters.Add(new FilterParameter("@HierarchyFilterMode", filterType.ToString(), DbType.String));
                        parameters.Add(new FilterParameter("@SearchValue", _searchValue, DbType.String));
                        parameters.Add(new FilterParameter("@EntityHierarchyMode", EntityHierarchyMode, DbType.AnsiString));
                        parameters.Add(new FilterParameter("@EntityHierarchyValue", EntityHierarchyValue, DbType.AnsiString));

                        uxSearchValueCommonKey.DataSource = WebServices.RiskServices.GetReports("spa_REF_RM_MCF_Get_MultiMerchantWatch_Filter", parameters);
                        if ((uxSearchValueCommonKey.DataSource as DataTable).Rows.Count > 10)
                        {
                            uxSearchValueCommonKey.Height = Unit.Pixel(200);
                        }
                        else
                        {
                            uxSearchValueCommonKey.Height = 0;
                        }
                        uxSearchValueCommonKey.DataBind();
                    }
                }
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (this.IsIntruderDetected) return;

        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMultiWatchTypeGrid:
                {
                    ASGrid grid = (ASGrid)sender;
                    grid.Columns.FindByUniqueName("RemoveCommand").Visible = GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus((SecurePage)Page);
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.Add(new FilterParameter("@FilterType", CurrentFilterMode.ToString(), DbType.AnsiString));
                    parameters.Add(new FilterParameter("@EntityHierarchyMode", EntityHierarchyMode, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@EntityHierarchyValue", EntityHierarchyValue, DbType.AnsiString));
                    parameters.AddLanguageID();
                    DataTable dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetGroupWatchList", parameters);
                    grid.DataSource = dt;
                    if (dt.HasData())
                    {
                        hasScroll = dt.Rows.Count > 8;
                        if (hasScroll)
                        {
                            divGrid.Style.Add("height", "282px");
                        }
                        else
                        {
                            divGrid.Style.Remove("height");
                        }

                        grid.Height = (hasScroll ? Unit.Pixel(257) : Unit.Empty);
                        grid.ClientSettings.Scrolling.AllowScroll = hasScroll;
                        grid.ClientSettings.Scrolling.ScrollHeight = Unit.Pixel(257);
                        grid.ClientSettings.Scrolling.UseStaticHeaders = hasScroll;
                        
                    }
                }
                break;

            case DataBindAction.BindMerchantOnWatchGrid:
                {
                    ASGrid grid = (ASGrid)sender;

                    parameters.AddLoggedInUserReportingParams(false);
                    if (FilterValue == "%")
                    {
                        parameters.Add(new FilterParameter("@FilterType", CurrentFilterMode.ToString(), DbType.AnsiString));
                    }
                    else
                    {
                        parameters.Add(new FilterParameter("@FilterType", FilterType, DbType.AnsiString));
                    }

                    parameters.Add(new FilterParameter("@FilterValue", FilterValue, DbType.String));
                    parameters.Add(new FilterParameter("@EntityHierarchyMode", EntityHierarchyMode, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@EntityHierarchyValue", EntityHierarchyValue, DbType.AnsiString));
                    parameters.AddLanguageID();
                    uxReportGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, "GetReports", new object[] { "spa_RM_MCF_GetMerchantsOnWatchList", ReportServices.ConvertToFilterParamWSArray(parameters) });
                    if (FilterValue == "%")
                    {
                        uxExport.GridHeader = GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String3").ToString();
                        uxExport.GridTitle = GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String3").ToString();
                    }
                }
                break;
            case DataBindAction.BindFilterType:
                {
                    parameters.AddLoggedInUserReportingParams(false);
                    parameters.AddLanguageID();
                    DataTable data = WebServices.RiskServices.GetReports("spa_RM_MCF_GetWatchFilterTypes", parameters);
                    BindFilterData(data);
                    uxFilterType.Items.Insert(0, new RadComboBoxItem(GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_ComboItem1").ToString(), "ALL"));
                    uxFilterType.Items.Insert(0, new RadComboBoxItem(GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_ComboItem2").ToString(), "SELECT_ONE"));
                    uxFilterType.Items.Add(new RadComboBoxItem(GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_ComboItem3").ToString(), "MERCHANT"));
                    RadComboBoxItem item = uxFilterType.Items.FindItemByValue((Enum.Parse(typeof(WebSiteEnums.WatchFilterTypes), WebSiteEnums.WatchFilterTypes.ALL.ToString())).ToString());
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
                break;
        }
    }

    private void BindFilterData(DataTable data)
    {        
        foreach (DataRow dtRow in data.Rows)
        {
            bool isSelectable = dtRow[SELECTABLE].ToBoolean();
            if (isSelectable)
            {
                if(dtRow[CHILD_OF] is DBNull)
                    uxFilterType.Items.Add(new RadComboBoxItem() { Text = dtRow[DATA_TEXT].ToString(), Value = dtRow[DATA_KEY].ToString()});
                else
                    uxFilterType.Items.Add(new RadComboBoxItem() { Text = dtRow[DATA_TEXT].ToString(), Value = dtRow[DATA_KEY].ToString(), CssClass = "rcbPrimary" });
            }
            else
            {
                uxFilterType.Items.Add(new RadComboBoxItem() { Text = dtRow[DATA_TEXT].ToString(), Value = dtRow[DATA_KEY].ToString(), IsSeparator = true });
            }
        }              
    }

    protected void uxSearchValueCommonKey_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        _searchValue = e.Text.Trim();
        OnPostBackActions(PostBackAction.SearchValueOnFilterType);
    }

    protected void uxGrid_OnItemCommand(object source, GridCommandEventArgs e)
    {
        if (this.IsIntruderDetected) return;
        OnPostBackActions(PostBackAction.GridWatchGroupItemCommand, e);    
    }

    public void RebindWatchTypeGrid()
    {
        OnPostBackActions(PostBackAction.RefreshEvent);
    }

    public void RebindMerchantOnWatchGrid()
    {
        OnPostBackActions(PostBackAction.RefreshEvent);
    }

    private void BindDataIntoComboSearchValue(int MinCharacter, string watchFilterType)
    {
        if (MinCharacter != 0)
        {
            DataTable dt = null;
            uxSearchValueCommonKey.DataSource = dt;
            uxSearchValueCommonKey.Items.Clear();
            uxSearchValueCommonKey.MarkFirstMatch = false;
            uxSearchValueCommonKey.EnableLoadOnDemand = true;
            uxSearchValueCommonKey.AppendDataBoundItems = true;
            uxSearchValueCommonKey.Height = 0;
        }
        else
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams(false);
            parameters.Add(new FilterParameter("@HierarchyFilterMode", watchFilterType.ToString(), DbType.String));
            parameters.Add(new FilterParameter("@EntityHierarchyMode", EntityHierarchyMode, DbType.AnsiString));
            parameters.Add(new FilterParameter("@EntityHierarchyValue", EntityHierarchyValue, DbType.AnsiString));

            uxSearchValueCommonKey.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_GetHierarchyFilterValueForMultiMerchantWatch", parameters);
            uxSearchValueCommonKey.DataBind();
            RadComboBoxItem item = new RadComboBoxItem(string.Empty);
            item.Height = Unit.Pixel(12);
            uxSearchValueCommonKey.Items.Insert(0, item);
            if ((uxSearchValueCommonKey.DataSource as DataTable).Rows.Count > 10)
            {
                uxSearchValueCommonKey.Height = Unit.Pixel(200);
            }
            else
            {
                uxSearchValueCommonKey.Height = 0;
            }
            uxSearchValueCommonKey.EmptyMessage = GetLocalResourceObject("rm_MultiMerchantWatch_aspx_cs_String8").ToString();
        }
    }
}
