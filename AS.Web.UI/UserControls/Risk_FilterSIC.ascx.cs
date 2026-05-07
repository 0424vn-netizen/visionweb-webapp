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
using AS.Common.DBManager;
using AS.Web.Business;
using System.Text;
using AS.Common;
using AS.Controls.Grid;

public partial class UserControls_Risk_FilterSIC : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        BindStatus,
        CountSIC,
        BindSICs
    }
    enum PostBackAction
    {
        ClearAll,
        SelectItem,
        SelectStatus
    }
    #endregion

    #region Const
    const int MAX_ITEMS = 7;
    const string FILTER_TYPE = "4";
    protected const string IS_SELECT_ALL_FLAG = "true";
    protected const string IS_DESELECT_ALL_FLAG = "false";
    const string IS_ADD_FLAG = "true";
    static string MESSAGE_MODE = string.Empty;
    static string _includedIn = string.Empty;
    static string _excludedFrom = string.Empty;
    static string _defaultValueNA = string.Empty;
    #endregion

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
            return this.uxSICCode.Value.Trim();
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

    protected void uxGridSIC_Init(object sender, EventArgs e)
    {
        this.uxGridSIC.PageSize = 10;
    }
    private void RemoveFilterMenuItem()
    {
        //show filter menu
        var grids = new RadGrid[] { uxGridSIC };
        var removedItems = new string[] { 
            "Between", "NotBetween",
            "IsNull", "NotIsNull" 
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
        MESSAGE_MODE = "<b>" + GetLocalResourceObject("Risk_FilterSIC_ascx_cs_MessageMode").ToString() + "</b><br />";
        _includedIn = GetLocalResourceObject("Risk_FilterSIC_ascx_cs_IncludedIn").ToString();
        _excludedFrom = GetLocalResourceObject("Risk_FilterSIC_ascx_cs_ExcludedFrom").ToString();
        _defaultValueNA = GetLocalResourceObject("Risk_FilterSIC_ascx_cs_NA").ToString();
        RemoveFilterMenuItem();
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindStatus);
            OnDataBindControls(DataBindAction.CountSIC);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindStatus:
                BindStatus();
                break;
            case DataBindAction.CountSIC:
                CountSIC();
                break;
            case DataBindAction.BindSICs:
                FilterParameterCollection _params = new FilterParameterCollection();
                _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                _params.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
                _params.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(PrimaryID), DbType.Int32));

                string spaName = "spa_rm_cs_GetFilterSICCode";
                this.uxGridSIC.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME,
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
                this.uxGridSIC.Rebind();
                break;
            case PostBackAction.SelectItem:
                this.SaveSIC(IsAdded, IsSelectAll, SICCode, this.uxGridSIC.AS_FilterExpression);
                break;
            case PostBackAction.SelectStatus:
                this.UpdateIncludeStatus(IsIncluded);
                break;
        }
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

    protected void uxGridSIC_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem headerItem = e.Item as GridHeaderItem;
            AS.Controls.Global.CheckBox chkHeader = headerItem.FindControl("chkHeader") as AS.Controls.Global.CheckBox;
            chkHeader.Visible = !string.IsNullOrEmpty(this.uxGridSIC.MasterTableView.FilterExpression) && (this.uxGridSIC.MasterTableView.DataSourceCount > 0);
            chkHeader.Checked = IsAllFlag && chkHeader.Visible;

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
            chkItem.Attributes["onclick"] = string.Format(chkItemEvent, "this", "'" + dataView["SICCode"].ToString() + "'");
        }
    }

    protected void uxGridSIC_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.FilterCommandName)
        {
            this.ClearFilterCheckAll();
        }
    }

    protected void uxGridSIC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindSICs);
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
            str.Append(dt.Rows[i]["SICCode"].ToString() + " - " + dt.Rows[i]["Description"].ToString());
            str.Append(", ");
        }

        return string.IsNullOrEmpty(str.ToString()) ? _defaultValueNA : str.ToString().Trim().TrimEnd(",".ToCharArray());
    }

    private static string BuildeSelectedValues(DataTable dt, bool isInclude)
    {

        StringBuilder str = new StringBuilder();

        if (dt != null && dt.Rows.Count > 0)
        {
            if (isInclude)
                str.Append(string.Format(MESSAGE_MODE, _includedIn));
            else
                str.Append(string.Format(MESSAGE_MODE, _excludedFrom));

            str.Append("\n");
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str.Append(dt.Rows[i]["SICCode"].ToString() + " - " + dt.Rows[i]["Description"].ToString());
            str.Append(", ");
        }

        return string.IsNullOrEmpty(str.ToString()) ? _defaultValueNA : str.ToString().Trim().TrimEnd(",".ToCharArray());
    }

    public static string GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, string FilterID, out bool isViewMore)
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _params.Add(new FilterParameter("@Mode", mode, DbType.Int32));
        _params.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(primaryID), DbType.Int32));
        _params.Add(new FilterParameter("@IsViewMore", false, DbType.Boolean, true));
        _params.Add(new FilterParameter("@MaxItems", MAX_ITEMS, DbType.Int32));

        FilterParameterCollection _paramsOut = new FilterParameterCollection();

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_GetFilterSICCode_IsViewMore", _params, out _paramsOut);
        FilterParameter isVMParam = _paramsOut.FindFilterParameterByName("@IsViewMore", true);
        isViewMore = false;
        if (isVMParam != null)
        {
            isViewMore = Convert.ToBoolean(isVMParam.ParameterValue);
        }
        // Get data for preview

        FilterParameter param = _params.FindFilterParameterByName("@IsViewMore", true);
        _params.Remove(param);
        DataTable dt = WebServices.RiskServices.GetReports("spa_rm_cs_GetFilterSICCode_Preview", _params);


        bool isInclude = GetIncludeStatus(primaryID, (int)mode);

        return BuildeSelectedValues(dt, isInclude);
    }

    #endregion

    #region Helpers

    private void BindStatus()
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(PrimaryID), DbType.Int32));
        parames.Add(new FilterParameter("@Mode", Convert.ToInt32(Mode), DbType.Int32));
        parames.Add(new FilterParameter("@IsAllSIC", false, DbType.Boolean, true));
        parames.Add(new FilterParameter("@IsIncluded", false, DbType.Boolean, true));

        FilterParameterCollection paramesOut = new FilterParameterCollection();

        // Get OutParams value
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_GetFilterSICCode_Info", parames, out paramesOut);

        FilterParameter isIncluded = paramesOut.FindFilterParameterByName("@IsIncluded", true);

        if (isIncluded != null)
        {
            uxRadioMode.Items[0].Selected = Convert.ToBoolean(isIncluded.ParameterValue);
            uxRadioMode.Items[1].Selected = !uxRadioMode.Items[0].Selected;
        }

    }

    private static bool GetIncludeStatus(string primaryID, int mode)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(primaryID), DbType.Int32));
        parames.Add(new FilterParameter("@Mode", Convert.ToInt32(mode), DbType.Int32));
        parames.Add(new FilterParameter("@IsAllSIC", false, DbType.Boolean, true));
        parames.Add(new FilterParameter("@IsIncluded", false, DbType.Boolean, true));

        FilterParameterCollection paramesOut = new FilterParameterCollection();

        // Get OutParams value
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_GetFilterSICCode_Info", parames, out paramesOut);

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
        _params.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        _params.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(PrimaryID), DbType.Int32));

        DataTable dt = WebServices.RiskServices.GetReports("spa_rm_cs_GetFilterSICCode", _params);
        return dt;
    }

    private int SaveSIC(bool isAdded, bool isSelectAll, string sicCode, string filter)
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _params.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        _params.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(PrimaryID), DbType.Int32));
        _params.Add(new FilterParameter("@IsSelectAll", isSelectAll, DbType.Boolean));
        _params.Add(new FilterParameter("@IsAdded", isAdded, DbType.Boolean));
        _params.Add(new FilterParameter("@SICCode", sicCode, DbType.AnsiString));
        _params.Add(new FilterParameter("@strFilter", filter, DbType.AnsiString));
        bool isInclude = IsIncluded;
        _params.Add(new FilterParameter("@isIncluded", isInclude, DbType.Boolean));
        FilterParameterCollection _paramsOut = new FilterParameterCollection();

        int res = WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_SaveFilterSICCode", _params, out _paramsOut);

        CountSIC();

        return res;
    }

    private int UpdateIncludeStatus(bool isIncluded)
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _params.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        _params.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(PrimaryID), DbType.Int32));
        _params.Add(new FilterParameter("@IsIncluded", isIncluded, DbType.Boolean));

        FilterParameterCollection _paramsOut = new FilterParameterCollection();
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_UpdateIncludeStatus", _params, out _paramsOut);
    }

    private int ClearAllSIC()
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _params.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        _params.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(PrimaryID), DbType.Int32));

        FilterParameterCollection _paramsOut = new FilterParameterCollection();

        int res = WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_DeleteAllFilterSICCode", _params, out _paramsOut);

        CountSIC();

        return res;
    }

    private void CountSIC()
    {
        FilterParameterCollection _paramsIn = new FilterParameterCollection();
        _paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _paramsIn.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        _paramsIn.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(PrimaryID), DbType.Int32));
        _paramsIn.Add(new FilterParameter("@Count", 0, DbType.Int32, true));

        FilterParameterCollection _paramsOut = new FilterParameterCollection();

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_CountFilterSICCode", _paramsIn, out _paramsOut);

        FilterParameter param = _paramsOut.FindFilterParameterByName("@Count", true);

        string count = string.Empty;
        if (param != null)
            count = GetLocalResourceObject("Risk_FilterSIC_ascx_cs_SelectCount").ToString() + " <b>" + param.ParameterValue.ToString() + "</b>";
        lblCount.Text = VeraCodeSolution.DoVeraCode(count);

    }

    private void ClearFilterCheckAll()
    {
        this.uxAllFlag.Value = IS_DESELECT_ALL_FLAG;
    }
    #endregion

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    public int Save()
    {
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
}
