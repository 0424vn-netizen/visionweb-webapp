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
using AS.Common.DBManager;
using System.Text;
using AS.Controls.Pages;

public partial class UserControls_Risk_FilterMarketData : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        LoadMarketData
    }
    #endregion

    #region properties

    private int _WidthUC = 800;
    private int _HeightUC = 200;

    public int HeightUC
    {
        get { return _HeightUC; }
        set { _HeightUC = value; }
    }
    public int WidthUC
    {
        get { return _WidthUC; }
        set { _WidthUC = value; }
    }

    private string _MarketDataType = string.Empty;

    public string MarketDataType
    {
        get { return _MarketDataType; }
        set { _MarketDataType = value; }
    }

    static string _defaultValueNA = string.Empty;
    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        asContainer.Width = _WidthUC.ToString();
        uxMarketDataFilter.WidthSelector = (_WidthUC - 75) / 2;
        uxMarketDataFilter.HeightSelector = _HeightUC;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _defaultValueNA = GetLocalResourceObject("Risk_FilterMarketData_ascx_cs_NA").ToString();
        if (!IsPostBack)
        {
            uxComboCodeSet.SelectedValue = MarketDataType;
            //BindMultiSelector();
            OnDataBindControls(DataBindAction.LoadMarketData);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadMarketData:
                BindMultiSelector();
                break;
        }
    }

    /// <summary>
    /// Bind data for Multiselector
    /// </summary>
    private void BindMultiSelector()
    {
        uxMarketDataFilter.DataSourceOrigination = GetFilterList(WebSiteEnums.AssignmentFilterModes.NotAssigned);
        uxMarketDataFilter.DataSourceDestination = GetFilterList(WebSiteEnums.AssignmentFilterModes.Assigned);
    }

    /// <summary>
    /// Get Filter List
    /// </summary>
    /// <param name="whichMode">whichMode</param>
    /// <returns>Datatable</returns>
    private DataTable GetFilterList(WebSiteEnums.AssignmentFilterModes whichMode)
    {
        string codeSet = uxComboCodeSet.SelectedValue;
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@PrimaryID", PrimaryID, DbType.Int32));
        parames.Add(new FilterParameter("@Option", whichMode, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        parames.Add(new FilterParameter("@CodeSet", codeSet, DbType.String));
        return WebServices.RiskServices.GetReports("spa_rm_cs_GetFilterMarketData", parames);
    }

    /// <summary>
    /// Save filter when List Filters changed
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">e</param>
    protected void MultiSelector_OnMovedData(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (SelectedChanged != null)//if event register
                this.SelectedChanged(this, e);//raise event
        }
    }

    /// <summary>
    /// get list Corp/Reg/Prin/Asso had assigned
    /// </summary>
    /// <param name="mode">feature mode</param>
    /// <param name="primaryID">AssignmentId or AdhocId</param>
    /// <param name="paramID">Paramterkey if any</param>
    /// <param name="FilterID">filterId if any</param>
    /// <returns>string</returns>
    public static string GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, string FilterID, out string marketdata)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@PrimaryID", primaryID, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", mode, DbType.Int32));
        parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        parames.Add(new FilterParameter("@IsUseCodeSet", false, DbType.Boolean));
        DataTable dt = WebServices.RiskServices.GetReports("spa_rm_cs_GetFilterMarketData", parames);
        marketdata = "1";
        if (dt != null && dt.Rows.Count > 0)
            marketdata = dt.Rows[0]["MarketData"].ToString();
        return convertTable2String(dt);
    }

    
    /// <summary>
    /// convert table to string, using comma to separate element
    /// </summary>
    /// <param name="dt">data table</param>
    /// <returns>string</returns>
    private static string convertTable2String(DataTable dt)
    {
        string result = string.Empty;
        if (dt == null || dt.Rows.Count == 0)
            return _defaultValueNA;
        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            strBuilder.Append(dt.Rows[i]["DataText1"] + ", ");
        }
        result = strBuilder.ToString().Trim().TrimEnd(',');
        if (string.IsNullOrEmpty(result))
            return _defaultValueNA;
        return result;
    }

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    public int Save()
    {
        int primaryid = 0;
        if (string.IsNullOrEmpty(PrimaryID) || !int.TryParse(PrimaryID, out primaryid))
            return 1;

        string _CodeList = uxMarketDataFilter.GetSelectedItemsAsString(",");
        _CodeList = AddedCodeSet(_CodeList);

        if (!validateMarketData(_CodeList))
        {
            return 2;
        }

        FilterParameterCollection paramesIn = new FilterParameterCollection();
        paramesIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramesIn.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        paramesIn.Add(new FilterParameter("@PrimaryID", primaryid, DbType.Int32));
        paramesIn.Add(new FilterParameter("@FilterValues", _CodeList, DbType.String));

        FilterParameterCollection paramesOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_SaveFilterMarketData", paramesIn, out paramesOut);
        return 0;
    }

    public event EventHandler SelectedChanged;

    private string AddedCodeSet(string str)
    {
        string result = string.Empty;
        string[] array = str.Split(',');
        for (int i = 0; i < array.Length; i++)
        {
            result += array[i] + uxComboCodeSet.SelectedValue + ",";
        }
        result = result.Trim().TrimEnd(',');
        if (result.Length < 2)
            return string.Empty;
        return result;
    }

    private bool validateMarketData(string codelist)
    {
        if (string.IsNullOrEmpty(codelist))
            return true;
        FilterParameterCollection paramesIn = new FilterParameterCollection();
        paramesIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramesIn.Add(new FilterParameter("@CodeList", codelist, DbType.String));
        DataTable dt = WebServices.RiskServices.GetReports("spa_rm_cs_CheckMarketDataSetup", paramesIn);
        if (dt != null)
        {
            string result = dt.Rows[0][0].ToString();
            if (!string.IsNullOrEmpty(result))
            {
                (Page.Master as BaseMasterPage).AjaxAddResponseScript("checkMarketData('" + string.Format(GetLocalResourceObject("Risk_FilterMarketData_ascx_cs_MessageJavascript").ToString(),result) + "');");
                return false;
            }
        }
        return true;
    }

    #endregion
}
