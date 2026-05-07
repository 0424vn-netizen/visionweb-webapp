using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using System;
using System.Data;
using System.Text;
using Telerik.Web.UI;

public partial class UserControls_Message_HierarchyFilter : GlobalUserControl
{

    #region Enums
    enum DataBindAction
    {
        BindStatus,
        CountFilterValue,
        BindFilterValue
    }
    enum PostBackAction
    {
        ClearAll,
        SelectItem,
        SelectStatus
    }
    #endregion

    #region Const
    private const string MERCHANT_NUMBER = "MERCHANTNUMBER";
    protected const string IS_SELECT_ALL_FLAG = "true";
    protected const string IS_DESELECT_ALL_FLAG = "false";
    const string IS_ADD_FLAG = "true";
    string MESSAGE_MODE = string.Empty;

    #endregion

    private int MAX_ITEMS = Convert.ToInt32(WebSiteSettings.MessageHierarchyViewMore);
    public string HierarchyFilterText { get; set; }

    public string HierarchyFilterMode { get; set; }

    public string EntityFilterMode
    {
        get { return ViewState["EntityFilterMode"] == null ? "" : ViewState["EntityFilterMode"].ToString(); }
        set { ViewState["EntityFilterMode"] = value; }
    }

    public string EntityFilterValue
    {
        get { return ViewState["EntityFilterValue"] == null ? "" : ViewState["EntityFilterValue"].ToString(); }
        set { ViewState["EntityFilterValue"] = value; }
    }

    public int MessageID { get; set; }

    #region Properties

    protected bool IsSelectAll
    {
        get
        {
            if (this.uxIsSelectAll.Value.Trim().Equals(IS_SELECT_ALL_FLAG))
                return true;
            else
                return false;
        }
    }

    protected bool IsAdded
    {
        get
        {
            if (this.uxIsAdded.Value.Trim().Equals(IS_ADD_FLAG))
                return true;
            else
                return false;
        }
    }

    protected bool IsIncluded
    {
        get
        {
            return this.uxRadioMode.Items[0].Selected;
        }
    }

    protected string SICCode
    {
        get
        {
            return this.uxValueCode.Value.Trim();
        }
    }

    protected bool IsAllFlag
    {
        get
        {
            if (this.uxAllFlag.Value.Trim().Equals(IS_SELECT_ALL_FLAG))
                return true;
            else
                return false;
        }
    }

    #endregion

    private static string text_All = string.Empty;
    private static string text_Excluded = string.Empty;

    protected void uxGrid_Init(object sender, EventArgs e)
    {

       
     
        this.uxGrid.PageSize = 10;
    }

    protected void InitializeGridColumn()
    {
        //initialize cols
        foreach (DataRow row in SessionManager.MessageHierarchyFilter.Rows)
        {
            if (row["HierarchyFilterMode"].ToString() == HierarchyFilterMode)
            {
                ASGridBoundColumn boundColumn = (ASGridBoundColumn)uxGrid.MasterTableView.Columns.FindByUniqueName("DataKey");
                boundColumn.HeaderText = row["DisplayedText"].ToString();
                boundColumn.HeaderTooltip = row["DisplayedText"].ToString();                
            }
        }

        if (HierarchyFilterMode == "MERCHANTNUMBER")
        {
            ASGridBoundColumn boundColumn = (ASGridBoundColumn)uxGrid.MasterTableView.Columns.FindByUniqueName("DataText");
            boundColumn.HeaderText = GetLocalResourceObject("Message_HierarchyFilter_ascx_cs_MerchantName").ToString();
            boundColumn.HeaderTooltip = GetLocalResourceObject("Message_HierarchyFilter_ascx_cs_MerchantName").ToString();
            boundColumn.Visible = true;
            uxGrid.MasterTableView.Columns.FindByUniqueName("DataKey").HeaderStyle.Width = 150;        
        }
        else if (HierarchyFilterMode == "HEADQUARTER")
        {
            ASGridBoundColumn boundColumn = (ASGridBoundColumn)uxGrid.MasterTableView.Columns.FindByUniqueName("DataText");
            boundColumn.HeaderText = GetLocalResourceObject("Message_HierarchyFilter_ascx_cs_HeadquarterName").ToString();
            boundColumn.HeaderTooltip = GetLocalResourceObject("Message_HierarchyFilter_ascx_cs_HeadquarterName").ToString();
            boundColumn.Visible = true;
            uxGrid.MasterTableView.Columns.FindByUniqueName("DataKey").HeaderStyle.Width = 150;        
        }
    }

    protected void AddDefaultFilterValue()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));
        parameters.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_InsertDefaultFilterValueMessage", parameters, out paramsOut);
    }
    private void RemoveFilterMenuItem()
    {
        //show filter menu
        var grids = new RadGrid[] { uxGrid };
        var removedItems = new string[] { 
            "GreaterThan",
            "LessThan", "GreaterThanOrEqualTo", "LessThanOrEqualTo", "Between", "NotBetween",
            "IsEmpty", "NotIsEmpty", "IsNull", "NotIsNull" 
        };
        foreach (var grid in grids)
        {
            for (int i = 0; i < removedItems.Length; i++)
            {
                var mi = grid.FilterMenu.Items.FindItemByText(removedItems[i]);
                if (mi != null)
                    grid.FilterMenu.Items.Remove(mi);
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        MESSAGE_MODE = "<b>" + GetLocalResourceObject("Message_HierarchyFilter_ascx_cs_TheFollowing").ToString() + "</b><br />";
        text_All = GetLocalResourceObject("Message_HierarchyFilter_ascx_cs_All").ToString();
        text_Excluded = GetLocalResourceObject("Message_HierarchyFilter_ascx_cs_Excluded").ToString();
        InitializeGridColumn();
        RemoveFilterMenuItem(); 
        if (!IsPostBack)
        {
            //
            //set default filter for MS user
            switch (SessionManager.CurrentUserType)
            {
                case WebSiteEnums.UserHierarchyMode.Hierarchy:
                case WebSiteEnums.UserHierarchyMode.Headquarter:
                    {
                        EntityFilterMode = GeneralFuncsLib.GetHierarchyInfo(SessionManager.CurrentUser.EntityType).HierarchyMode;
                        string EntityID = SessionManager.CurrentUser.EntityID;

                        //DEFAULT
                        EntityFilterValue = EntityID;
                        //special cases
                        if (EntityFilterMode == "HEADQUARTER")
                        {

                            FilterParameterCollection pIn = new FilterParameterCollection();
                            pIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                            pIn.Add(new FilterParameter("@HierarchyFilterMode", EntityFilterMode, DbType.AnsiString));
                            pIn.Add(new FilterParameter("@EntityID", EntityID, DbType.AnsiString));
                            pIn.Add(new FilterParameter("@HierarchyFilterValue", string.Empty, DbType.AnsiString, true));
                            FilterParameterCollection pOut = new FilterParameterCollection();

                            DataTable Info = WebServices.RiskServices.GetReports("spa_rm_GetRealHierarchyFilterValueByHierarchyID", pIn);

                            EntityFilterValue = Info.Rows[0]["HierarchyFilterValue"].ToString();
                        }

                        //ends
              
                    }
                    break;
            }
            //
            AddDefaultFilterValue();
            OnDataBindControls(DataBindAction.BindStatus);
            OnDataBindControls(DataBindAction.CountFilterValue);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindStatus:
                BindStatus();
                break;
            case DataBindAction.CountFilterValue:
                CountFilterValue();
                break;
            case DataBindAction.BindFilterValue:
                FilterParameterCollection _params = new FilterParameterCollection();
                _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                _params.Add(new FilterParameter("@EntityHierarchyMode", EntityFilterMode, DbType.AnsiString));
                _params.Add(new FilterParameter("@EntityHierarchyValue", EntityFilterValue, DbType.AnsiString));
                _params.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
                _params.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));

                string spaName = "spa_GetMessageHierarchyFilter";
                this.uxGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME,
                    new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(_params) });
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.ClearAll:
                ClearAllSIC();
                uxRadioMode.Enabled = true;
                this.uxGrid.Rebind();
                break;
            case PostBackAction.SelectItem:
                if (string.IsNullOrEmpty(this.uxGrid.MasterTableView.FilterExpression) && IsSelectAll)
                {
                    if (IsAdded)
                    {
                        UpdateIncludeStatus(true);
                        uxRadioMode.Enabled = false;
                    }
                    else
                    {
                        uxRadioMode.Enabled = true;
                    }
                }
                this.Save(IsAdded, IsSelectAll, SICCode, this.uxGrid.AS_FilterExpression);
                break;
            case PostBackAction.SelectStatus:
                this.UpdateIncludeStatus(IsIncluded);
                break;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        bool haveFilterExpression = string.IsNullOrEmpty(this.uxGrid.MasterTableView.FilterExpression);
        NonReportPage page = (NonReportPage)this.Page;
        page.AjaxAddResponseScript(string.Format("RefreshFilter('{0}');", haveFilterExpression));
        base.OnPreRender(e);
    }

    #region ControlEvent

    protected void uxRadioMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SelectStatus);
    }

    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ClearAll);
    }


    protected void uxGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem headerItem = e.Item as GridHeaderItem;
            AS.Controls.Global.CheckBox chkHeader = headerItem.FindControl("chkHeader") as AS.Controls.Global.CheckBox;
            if (HierarchyFilterMode.Equals(MERCHANT_NUMBER))
            {
                chkHeader.Visible = !string.IsNullOrEmpty(this.uxGrid.MasterTableView.FilterExpression) && (this.uxGrid.MasterTableView.DataSourceCount > 0);
            }
            else
            {
                chkHeader.Visible = this.uxGrid.MasterTableView.DataSourceCount > 0;
            }

            string chkHeaderEvent = "doHeaderCheck({0})";
            chkHeader.Attributes["onclick"] = string.Format(chkHeaderEvent, "this");
        }
        else if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataView = dataItem.DataItem as DataRowView;
            AS.Controls.Global.CheckBox chkItem = dataItem.FindControl("chkItem") as AS.Controls.Global.CheckBox;
            chkItem.Checked = dataView["Assigned"].ToString().Equals("1");

            string chkItemEvent = "doItemCheck({0},{1})";
            chkItem.Attributes["onclick"] = string.Format(chkItemEvent, "this", "'" + dataView["DataKey"].ToString() + "'");
        }
    }



    protected void uxGrid_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.FilterCommandName)
        {
            this.ClearFilterCheckAll();
        }
    }

    protected void uxGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindFilterValue);
    }

    protected void btnHidden_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SelectItem);
    }

    #endregion

    #region StaticMethod

    private static string BuildeSelectedValues(DataTable dt)
    {
        StringBuilder str = new StringBuilder();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str.Append(dt.Rows[i]["DataKey"].ToString());
            str.Append(", ");
        }

        return string.IsNullOrEmpty(str.ToString()) ? "N/A" : str.ToString().Trim().TrimEnd(",".ToCharArray());
    }

    
    private static string BuildeSelectedValues(DataTable dt, bool isInclude, string HierarchyFilterText)
    {

        StringBuilder str = new StringBuilder();

        if (dt != null && dt.Rows.Count > 0)
        {
            if (!isInclude)
            {
                str.Append( "<span class='dark-blue'><b>"+text_All+" "+ HierarchyFilterText +"s - "+text_Excluded+":</b></span>");
                str.Append("<br />");
            }
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str.Append(dt.Rows[i]["DataKey"].ToString());
            str.Append(", ");
        }

        return string.IsNullOrEmpty(str.ToString()) ? "N/A" : str.ToString().Trim().TrimEnd(",".ToCharArray());
    }

    public static string GetSelectedValuesAsString(string MessageID, out bool isViewMore, string HierarchyFilterMode)
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
       
        _params.Add(new FilterParameter("@MessageID", Convert.ToInt32(MessageID), DbType.Int32));
        _params.Add(new FilterParameter("@IsViewMore", false, DbType.Boolean, true));
        _params.Add(new FilterParameter("@MaxItems", Convert.ToInt32(WebSiteSettings.MessageHierarchyViewMore), DbType.Int32));
        _params.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        FilterParameterCollection _paramsOut = new FilterParameterCollection();

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_GetMessageHierarchyFilter_IsViewMore", _params, out _paramsOut);
        FilterParameter isVMParam = _paramsOut.FindFilterParameterByName("@IsViewMore", true);
        isViewMore = false;
        if (isVMParam != null)
        {
            isViewMore = Convert.ToBoolean(isVMParam.ParameterValue);
        }
        // Get data for preview

        FilterParameter param = _params.FindFilterParameterByName("@IsViewMore", true);
        _params.Remove(param);
        DataTable dt = WebServices.RiskServices.GetReports("spa_GetMessageHierarchyFilter_Preview", _params);


        bool isInclude = GetIncludeStatus(MessageID, HierarchyFilterMode);

        string HierarchyFilterText = GeneralFuncsLib.GetMessageHierarchyFilterText(HierarchyFilterMode);

        return BuildeSelectedValues(dt, isInclude, HierarchyFilterText);
    }

    #endregion


    #region Helpers

    private void BindStatus()
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@MessageID", Convert.ToInt32(MessageID), DbType.Int32));
        
        parames.Add(new FilterParameter("@IsAll", false, DbType.Boolean, true));
        parames.Add(new FilterParameter("@IsIncluded", false, DbType.Boolean, true));
        parames.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        FilterParameterCollection paramesOut = new FilterParameterCollection();

        // Get OutParams value
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_GetMessageHierarchyFilter_Info", parames, out paramesOut);

        FilterParameter isIncluded = paramesOut.FindFilterParameterByName("@IsIncluded", true);

        if (isIncluded != null)
        {
            uxRadioMode.Items[0].Selected = Convert.ToBoolean(isIncluded.ParameterValue);
            uxRadioMode.Items[1].Selected = !uxRadioMode.Items[0].Selected;
        }

    }

    private static bool GetIncludeStatus(string MessageID, string HierarchyFilterMode)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@MessageID", Convert.ToInt32(MessageID), DbType.Int32));
        
        parames.Add(new FilterParameter("@IsAll", false, DbType.Boolean, true));
        parames.Add(new FilterParameter("@IsIncluded", false, DbType.Boolean, true));
        parames.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        FilterParameterCollection paramesOut = new FilterParameterCollection();

        // Get OutParams value
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_GetMessageHierarchyFilter_Info", parames, out paramesOut);

        FilterParameter isIncluded = paramesOut.FindFilterParameterByName("@IsIncluded", true);

        bool result = true;
        if (isIncluded != null)
            result = Convert.ToBoolean(isIncluded.ParameterValue);
        return result;
    }

    private DataTable GetDataTable()
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _params.Add(new FilterParameter("@EntityHierarchyMode", EntityFilterMode, DbType.AnsiString));
        _params.Add(new FilterParameter("@EntityHierarchyValue", EntityFilterValue, DbType.AnsiString));
        _params.Add(new FilterParameter("@MessageID", Convert.ToInt32(MessageID), DbType.Int32));
        _params.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));

        DataTable dt = WebServices.RiskServices.GetReports("spa_GetMessageHierarchyFilter", _params);
        return dt;
    }

    private int Save(bool isAdded, bool isSelectAll, string sicCode, string filter)
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _params.Add(new FilterParameter("@EntityHierarchyMode", EntityFilterMode, DbType.AnsiString));
        _params.Add(new FilterParameter("@EntityHierarchyValue", EntityFilterValue, DbType.AnsiString));
        _params.Add(new FilterParameter("@MessageID", Convert.ToInt32(MessageID), DbType.Int32));
        _params.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        _params.Add(new FilterParameter("@IsSelectAll", isSelectAll, DbType.Boolean));
        _params.Add(new FilterParameter("@IsAdded", isAdded, DbType.Boolean));
        _params.Add(new FilterParameter("@FilterValue", sicCode, DbType.AnsiString));
        _params.Add(new FilterParameter("@strFilter", filter, DbType.AnsiString));

        FilterParameterCollection _paramsOut = new FilterParameterCollection();

        int res = WebServices.RiskServices.ExecuteNonQueryCommand("spa_SaveMessageFilterValue", _params, out _paramsOut);

        CountFilterValue();
        OnDataBindControls(DataBindAction.BindStatus);
        return res;
    }

    private int UpdateIncludeStatus(bool isIncluded)
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
       
        _params.Add(new FilterParameter("@MessageID", Convert.ToInt32(MessageID), DbType.Int32));
        _params.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        _params.Add(new FilterParameter("@IsIncluded", isIncluded, DbType.Boolean));

        FilterParameterCollection _paramsOut = new FilterParameterCollection();
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_UpdateIncludedMessageStatus", _params, out _paramsOut);
    }

    private int ClearAllSIC()
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
       
        _params.Add(new FilterParameter("@MessageID", Convert.ToInt32(MessageID), DbType.Int32));
        _params.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        FilterParameterCollection _paramsOut = new FilterParameterCollection();

        int res = WebServices.RiskServices.ExecuteNonQueryCommand("spa_DeleteAllMessageFilter", _params, out _paramsOut);

        CountFilterValue();

        return res;
    }

    private void CountFilterValue()
    {
        FilterParameterCollection _paramsIn = new FilterParameterCollection();
        _paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        
        _paramsIn.Add(new FilterParameter("@MessageID", Convert.ToInt32(MessageID), DbType.Int32));
        _paramsIn.Add(new FilterParameter("@Count", 0, DbType.Int32, true));
        _paramsIn.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        FilterParameterCollection _paramsOut = new FilterParameterCollection();

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_GetMessageFilterCount", _paramsIn, out _paramsOut);

        FilterParameter param = _paramsOut.FindFilterParameterByName("@Count", true);

        string count = string.Empty;
        if (param != null)
            count = GetLocalResourceObject("Message_HierarchyFilter_ascx_cs_SelectCount").ToString() + " <span class=\"merchant-count-value\"><b>" + param.ParameterValue.ToString() + "</b></span>";
        lblCount.Text = VeraCodeSolution.DoVeraCode(count);

    }

    private void ClearFilterCheckAll()
    {
        this.uxAllFlag.Value = IS_DESELECT_ALL_FLAG;
    }
    #endregion

}



