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

public partial class UserControls_Risk_ParameterFilter_TransactionCode : GlobalUserControl, IRiskParamFilter
{

    #region Enums
    enum DataBindAction
    {
        LoadHierarchysList
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
    public string MarketData { get; set; }

    #endregion

    private static string _textNA = string.Empty;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //asContainer.Width = _WidthUC.ToString();
        //uxHierarchyFilter.WidthSelector = (_WidthUC - 75) / 2;
        //uxHierarchyFilter.HeightSelector = _HeightUC;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _textNA = GetLocalResourceObject("Risk_ParameterFilter_TransactionCode_ascx_cs_NA").ToString();
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.LoadHierarchysList);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadHierarchysList:
                if (!MarketData.IsNullOrEmpty())
                {
                    BindMultiSelector_MarketData();
                }
                else
                {
                    BindMultiSelector();
                }
                break;
        }
    }
    /// <summary>
    /// Bind data for Multiselector Market Data
    /// </summary>
    private void BindMultiSelector_MarketData()
    {
        uxHierarchyFilter.DataSourceOrigination = GetFilterList_MarketData(WebSiteEnums.AssignmentFilterModes.NotAssigned);
        uxHierarchyFilter.DataSourceDestination = GetFilterList_MarketData(WebSiteEnums.AssignmentFilterModes.Assigned);
    }
    /// <summary>
    /// Bind data for Multiselector
    /// </summary>
    private void BindMultiSelector()
    {
        uxHierarchyFilter.DataSourceOrigination = GetFilterList(WebSiteEnums.AssignmentFilterModes.NotAssigned);
        uxHierarchyFilter.DataSourceDestination = GetFilterList(WebSiteEnums.AssignmentFilterModes.Assigned);
    }

    /// <summary>
    /// Get Filter List
    /// </summary>
    /// <param name="whichMode">whichMode</param>
    /// <returns>Datatable</returns>
    private DataTable GetFilterList(WebSiteEnums.AssignmentFilterModes whichMode)
    {

        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@AssignmentID", PrimaryID, DbType.Int32));
        parames.Add(new FilterParameter("@ParameterID", this.ParamID, DbType.String));
        parames.Add(new FilterParameter("@Option", whichMode, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_rm_ParameterFilter_AuthTransactionCode_Get", parames);
    }
    private DataTable GetFilterList_MarketData(WebSiteEnums.AssignmentFilterModes whichMode)
    {

        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@ParameterID", this.ParamID, DbType.String));
        parames.Add(new FilterParameter("@MarketData", this.MarketData, DbType.String));
        parames.Add(new FilterParameter("@Option", whichMode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_rm_ParameterFilter_AuthTransactionCode_Get", parames);
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
    public static string GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, string FilterID)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        parames.Add(new FilterParameter("@ParameterID", paramID, DbType.String));
        parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", mode, DbType.Int32));
        DataTable dt = WebServices.RiskServices.GetReports("spa_rm_ParameterFilter_AuthTransactionCode_Get", parames);
        return convertTable2String(dt);
    }
    //For Market Data
    public static string GetSelectedValuesString_MarketData(string marketData, string paramID)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@ParameterID", paramID, DbType.String));
        parames.Add(new FilterParameter("@MarketData", marketData, DbType.String));
        parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        DataTable dt = WebServices.RiskServices.GetReports("spa_rm_ParameterFilter_AuthTransactionCode_Get", parames);
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
            return _textNA;
        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            strBuilder.Append(dt.Rows[i]["DataText"] + ", ");
        }
        result = strBuilder.ToString().Trim().TrimEnd(',');
        if (string.IsNullOrEmpty(result))
            return _textNA;
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

        string _CodeList = uxHierarchyFilter.GetSelectedItemsAsString(",");

        FilterParameterCollection paramesIn = new FilterParameterCollection();
        paramesIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramesIn.Add(new FilterParameter("@AssignmentID", PrimaryID, DbType.Int32));
        paramesIn.Add(new FilterParameter("@ParameterID", this.ParamID, DbType.String));
        paramesIn.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        paramesIn.Add(new FilterParameter("@FilterValues", _CodeList, DbType.String));

        FilterParameterCollection paramesOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_ParameterFilter_AuthTransactionCode_Save", paramesIn, out paramesOut);
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
    public int Save_MarketData()
    {
        
        string _CodeList = uxHierarchyFilter.GetSelectedItemsAsString(",");

        FilterParameterCollection paramesIn = new FilterParameterCollection();
        paramesIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramesIn.Add(new FilterParameter("@ParameterID", this.ParamID, DbType.String));
        paramesIn.Add(new FilterParameter("@MarketData", this.MarketData, DbType.String));
        paramesIn.Add(new FilterParameter("@FilterValues", _CodeList, DbType.String));

        FilterParameterCollection paramesOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_ParameterFilter_AuthTransactionCode_Save", paramesIn, out paramesOut);
        return 0;
    }
 
}
