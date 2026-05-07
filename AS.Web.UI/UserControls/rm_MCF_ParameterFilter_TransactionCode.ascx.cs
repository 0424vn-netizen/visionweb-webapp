using System;
using System.Data;
using AS.Common.DBManager;
using System.Collections.Generic;
using BuGeneralFuncsLib =AS.Web.Business.General.GeneralFuncsLib;

public partial class UserControls_rm_MCF_ParameterFilter_TransactionCode : GlobalUserControl, IRiskParamFilter
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
        var results = WebServices.RiskServices.GetReports(GetSpaNameByParameterId(this.ParamID, SpaType.Get), parames);
        return MapToDataTableByParameter(results, this.ParamID);
    }
    private DataTable GetFilterList_MarketData(WebSiteEnums.AssignmentFilterModes whichMode)
    {

        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@ParameterID", this.ParamID, DbType.String));
        parames.Add(new FilterParameter("@MarketData", this.MarketData, DbType.String));
        parames.Add(new FilterParameter("@Option", whichMode, DbType.Int32));
        var results = WebServices.RiskServices.GetReports(GetSpaNameByParameterId(this.ParamID, SpaType.Get), parames);
        return MapToDataTableByParameter(results, this.ParamID);
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
    public static Dictionary<string, string> GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, string FilterID)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        parames.Add(new FilterParameter("@ParameterID", paramID, DbType.String));
        parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", mode, DbType.Int32));
        DataTable dt = WebServices.RiskServices.GetReports(GetSpaNameByParameterId(paramID, SpaType.Get), parames);
        return ConvertTable2String(dt, paramID);
    }
    //For Market Data
    public static Dictionary<string, string> GetSelectedValuesString_MarketData(string marketData, string paramID)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@ParameterID", paramID, DbType.String));
        parames.Add(new FilterParameter("@MarketData", marketData, DbType.String));
        parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        DataTable dt = WebServices.RiskServices.GetReports(GetSpaNameByParameterId(paramID, SpaType.Get), parames);
        return ConvertTable2String(dt, paramID);
    }

    /// <summary>
    /// convert table to string, using comma to separate element
    /// </summary>
    /// <param name="dt">data table</param>
    /// <returns>string</returns>
    private static Dictionary<string, string> ConvertTable2String(DataTable dt, string paramKey)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        string result = string.Empty;
        string tracking = string.Empty;
        
        if (dt == null || dt.Rows.Count == 0)
        {
            result = tracking = _textNA;
            data.Add("Label", result);
            data.Add("Tracking", tracking);
            return data;
        }
        var isModelType = IsModelTypeInConfig(paramKey);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            var dataText = isModelType ? BuGeneralFuncsLib.SliptByCapitalLetter(dt.Rows[i]["DataText"].ToString()) : dt.Rows[i]["DataText"];
            result += dataText + ", ";
            tracking += dt.Rows[i]["DataKey"] + ", ";
        }

        result = result.Trim().TrimEnd(',');
        tracking = result.Trim().TrimEnd(',');

        data.Add("Label", result);
        data.Add("Tracking", tracking);

        return data;
    }
    private static DataTable MapToDataTableByParameter(DataTable inputData, string paramKey)
    {
        var isModelType = IsModelTypeInConfig(paramKey);

        if (!isModelType || inputData == null || inputData.Rows.Count <= 0)
        {
            return inputData;
        }
        for (int i = 0; i < inputData.Rows.Count; i++)
        {
            inputData.Rows[i]["DataText"] = BuGeneralFuncsLib.SliptByCapitalLetter(inputData.Rows[i]["DataText"].ToString());
        }
        return inputData;
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
        WebServices.RiskServices.ExecuteNonQueryCommand(GetSpaNameByParameterId(ParamID, SpaType.Save), paramesIn, out paramesOut);
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
        WebServices.RiskServices.ExecuteNonQueryCommand(GetSpaNameByParameterId(ParamID, SpaType.Save), paramesIn, out paramesOut);
        return 0;
    }
    #region private
    private static string GetSpaNameByParameterId(string paramID, SpaType spaType)
    {
        var isModelType = IsModelTypeInConfig(paramID);
        switch (spaType)
        {
            case SpaType.Get:
                if (isModelType)
                    return "spa_RM_MCF_ParameterFilter_ModelType_Get";
                return "spa_RM_MCF_ParameterFilter_AuthTransactionCode_Get";
            case SpaType.Save:
                if (isModelType)
                    return "spa_RM_MCF_ParameterFilter_ModelType_Save";
                return "spa_RM_MCF_ParameterFilter_AuthTransactionCode_Save";
            default:
                return null;
        }
    }
    private static bool IsModelTypeInConfig(string paramID)
    {
        return BuGeneralFuncsLib.CheckExistsInSplitToArray(WebSiteSettings.ParametersAllowDecimal, ',', paramID);
    }
    #endregion

}
