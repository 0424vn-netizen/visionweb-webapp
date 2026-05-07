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
using System.Collections.Generic;

public partial class UserControls_rm_MCF_FilterRiskLevel : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        LoadRiskLevel
    }
    #endregion

    #region properties

    static string PAGE_PATH = "~/UserControls/rm_MCF_FilterRiskLevel.ascx";
    static string MESSAGE_MODE = string.Empty;
    static string _includedIn = string.Empty;
    static string _excludedFrom = string.Empty;
    static string _defaultValueNA = string.Empty;

    private int _WidthUC = 800;
    private int _HeightUC = 200;
    private string FilterName = "RiskLevel";
    public bool IsInclude { get; set; }

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
    #endregion


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        MESSAGE_MODE = "<b>" + GetLocalResourceObject("Risk_FilterRiskLevel_ascx_cs_MessageCode").ToString() + "</b><br />";
        _includedIn = GetLocalResourceObject("Risk_Filter_ascx_cs_IncludedIn").ToString();
        _excludedFrom = GetLocalResourceObject("Risk_Filter_ascx_cs_ExcludedFrom").ToString();
        _defaultValueNA = GetLocalResourceObject("Risk_FilterRiskLevel_ascx_cs_NA").ToString();
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.LoadRiskLevel);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadRiskLevel:
                BindMultiSelector();
                break;
        }
    }

    #region ControlEvent

    /// <summary>
    /// Save filter when List Filters changed
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">e</param>
    protected void MultiSelector_OnMovedData(object sender, EventArgs e)
    {
        if (SelectedChanged != null)//if event register
            this.SelectedChanged(this, e);//raise event
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Bind data for Multiselector
    /// </summary>
    private void BindMultiSelector()
    {
        uxFilterRiskLevel.DataSourceOrigination = GetFilterList(WebSiteEnums.AssignmentFilterModes.NotAssigned);
        uxFilterRiskLevel.DataSourceDestination = GetFilterList(WebSiteEnums.AssignmentFilterModes.Assigned);
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
        parames.Add(new FilterParameter("@AssignmentID", PrimaryID, DbType.AnsiString));
        parames.Add(new FilterParameter("@Option", whichMode, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        parames.Add(new FilterParameter("@FilterName", FilterName, DbType.AnsiString));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetElementFilter", parames);
    }

    #endregion

    #region Static method

    public static Dictionary<string, string> GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, string FilterID, string filterName)
    {
        bool merchantRankIncluded = false;

        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", mode, DbType.Int32));
        parames.Add(new FilterParameter("@FilterName", filterName, DbType.AnsiString));
        DataSet ds = WebServices.RiskServices.GetReportsAsDataSet("spa_RM_MCF_GetElementFilter", parames);

        if(ds.Tables.Count > 0) { 
            if (ds.Tables[0].Rows.Count > 0)
                merchantRankIncluded = ds.Tables[0].Rows[0]["IsIncluded"].ToBoolean();

            return convertTable2String(ds.Tables[0], merchantRankIncluded);
        }

        return new Dictionary<string, string>();
    }
    
    private static Dictionary<string, string> convertTable2String(DataTable dt, bool isInclude)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        MESSAGE_MODE = "<b>" + HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_FilterRiskLevel_ascx_cs_MessageCode").ToString() + "</b><br />";
        _includedIn = HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_Filter_ascx_cs_IncludedIn").ToString();
        _excludedFrom = HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_Filter_ascx_cs_ExcludedFrom").ToString();
        _defaultValueNA = HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_FilterRiskLevel_ascx_cs_NA").ToString();

        string tracking = string.Empty;
        StringBuilder str = new StringBuilder();

        if (dt != null && dt.Rows.Count > 0)
        {
            if (isInclude)
            {
                str.Append(string.Format(MESSAGE_MODE, _includedIn));
                tracking = RM_MCF_GeneralFuncsLib.INCLUDED_KEY;
            }
            else
            {
                str.Append(string.Format(MESSAGE_MODE, _excludedFrom));
                tracking = RM_MCF_GeneralFuncsLib.EXCLUDED_KEY;
            }
            str.Append("\n");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str.Append(dt.Rows[i]["DataText"] + ", ");
                tracking += dt.Rows[i]["DataKey"].ToString() + ", ";
            }
        }

    string value = string.IsNullOrEmpty(str.ToString()) ? _defaultValueNA : str.ToString().Trim().TrimEnd(",".ToCharArray());
        tracking = string.IsNullOrEmpty(tracking) ? _defaultValueNA : tracking.Trim().TrimEnd(",".ToCharArray());

        data.Add("Label", value);
        data.Add("Tracking", tracking);

        return data;
    }

    #endregion

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

        string _CodeList = uxFilterRiskLevel.GetSelectedItemsAsString(",");

        FilterParameterCollection paramesIn = new FilterParameterCollection();
        paramesIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramesIn.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        paramesIn.Add(new FilterParameter("@AssignmentID", primaryid, DbType.Int32));
        paramesIn.Add(new FilterParameter("@FilterName", FilterName, DbType.AnsiString));
        paramesIn.Add(new FilterParameter("@FilterValues", _CodeList, DbType.String));
        paramesIn.Add(new FilterParameter("@IsIncluded", IsInclude, DbType.Boolean));

        FilterParameterCollection paramesOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveElementFilter", paramesIn, out paramesOut);
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
}
