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

public partial class UserControls_rm_MCF_FilterState : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        LoadStates
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
    #endregion

    static string _defaultValueNA = string.Empty;    

    protected void Page_Load(object sender, EventArgs e)
    {
        _defaultValueNA = GetLocalResourceObject("Risk_FilterState_ascx_cs_NA").ToString();
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.LoadStates);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadStates:
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
        uxFilterState.DataSourceOrigination = GetFilterList(WebSiteEnums.AssignmentFilterModes.NotAssigned);
        uxFilterState.DataSourceDestination = GetFilterList(WebSiteEnums.AssignmentFilterModes.Assigned);
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
        parames.Add(new FilterParameter("@PrimaryID", PrimaryID, DbType.AnsiString));
        parames.Add(new FilterParameter("@Option", whichMode, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetFilterState", parames);
    }

    #endregion

    #region Static method

    public static Dictionary<string, string> GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, string FilterID)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@PrimaryID", primaryID, DbType.Int32));
        parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", mode, DbType.Int32));
        DataTable dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetFilterState", parames);
        return convertTable2String(dt);
    }
    
    private static Dictionary<string, string> convertTable2String(DataTable dt)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        var lablels = new List<string>();
        var trackings = new List<string>();

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                lablels.Add(row["DataText"].ToString());
                trackings.Add(row["DataKey"].ToString());
            }
        }
        else 
        {
            lablels.Add("N/A");
            trackings.Add("N/A");
        }

        data.Add("Label", string.Join(",", lablels));
        data.Add("Tracking", string.Join(",", trackings));

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

        string _CodeList = uxFilterState.GetSelectedItemsAsString(",");

        FilterParameterCollection paramesIn = new FilterParameterCollection();
        paramesIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramesIn.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        paramesIn.Add(new FilterParameter("@PrimaryID", primaryid, DbType.Int32));
        paramesIn.Add(new FilterParameter("@FilterValues", _CodeList, DbType.String));

        FilterParameterCollection paramesOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveFilterSate", paramesIn, out paramesOut);
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
}
