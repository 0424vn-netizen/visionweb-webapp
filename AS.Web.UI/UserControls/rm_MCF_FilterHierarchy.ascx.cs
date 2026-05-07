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

public partial class UserControls_rm_MCF_FilterHierarchy : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        LoadHierarchysList
    }
    #endregion
    static string PAGE_PATH = "~/UserControls/rm_MCF_FilterHierarchy.ascx";
    static string MESSAGE_MODE = string.Empty;
    static string _includedIn = string.Empty;
    static string _excludedFrom = string.Empty;
    static string _defaultValueNA = string.Empty;

    private static string EntityHierarchyFilterMode;
    private static string EntityHierarchyFilterValue;
    public bool IsInclude { get; set; }
    public bool IsIncludeExcludeItem { get; set; }
    public bool GetItemFromSession { get; set; }
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

    public string HierarchyFilterText { get; set; }

    public string HierarchyFilterMode { get; set; }

    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //asContainer.Width = _WidthUC.ToString();
        //uxHierarchyFilter.WidthSelector = (_WidthUC - 75) / 2;
        //uxHierarchyFilter.HeightSelector = _HeightUC;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        MESSAGE_MODE = "<b>" + GetLocalResourceObject("Risk_FilterHierachy_ascx_cs_MessageMode").ToString() + "</b><br />";
        _includedIn = GetLocalResourceObject("Risk_FilterHierachy_ascx_cs_IncludedIn").ToString();
        _excludedFrom = GetLocalResourceObject("Risk_FilterHierachy_ascx_cs_ExcludedFrom").ToString();
        _defaultValueNA = GetLocalResourceObject("Risk_FilterHierachy_ascx_cs_NA").ToString();
        //asContainer.HeaderText = HierarchyFilterText;
        if (!IsPostBack)
        {
            EntityHierarchyFilterMode = string.Empty;
            EntityHierarchyFilterValue = string.Empty;
            if (!IsIncludeExcludeItem)
            {
                switch (SessionManager.CurrentUserType)
                {
                    case WebSiteEnums.UserHierarchyMode.Hierarchy:
                    case WebSiteEnums.UserHierarchyMode.Headquarter:
                        {
                            EntityHierarchyFilterMode = GeneralFuncsLib.GetHierarchyInfo(SessionManager.CurrentUser.EntityType).HierarchyMode;
                            string EntityID = SessionManager.CurrentUser.EntityID;

                            //DEFAULT
                            EntityHierarchyFilterValue = EntityID;
                            //special cases
                            if (EntityHierarchyFilterMode == "HEADQUARTER")
                            {

                                FilterParameterCollection pIn = new FilterParameterCollection();
                                pIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                                pIn.Add(new FilterParameter("@HierarchyFilterMode", EntityHierarchyFilterMode, DbType.AnsiString));
                                pIn.Add(new FilterParameter("@EntityID", EntityID, DbType.AnsiString));
                                pIn.Add(new FilterParameter("@HierarchyFilterValue", string.Empty, DbType.AnsiString, true));
                                FilterParameterCollection pOut = new FilterParameterCollection();

                                DataTable Info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetRealHierarchyFilterValueByHierarchyID", pIn);

                                EntityHierarchyFilterValue = Info.Rows[0]["HierarchyFilterValue"].ToString();
                            }

                            //ends

                        }
                        break;
                }
            }

            OnDataBindControls(DataBindAction.LoadHierarchysList);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadHierarchysList:
                BindMultiSelector();
                break;
        }
    }

    /// <summary>
    /// Bind data for Multiselector
    /// </summary>
    private void BindMultiSelector()
    {
        if (IsIncludeExcludeItem)
        {
            FilterParameterCollection parames = new FilterParameterCollection();
            parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parames.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
            DataTable dsOrigination = WebServices.RiskServices.GetReports("spa_GetHierarchyListFilter", parames);
            DataTable dsDestination = new DataTable();
            dsDestination.Columns.Add("DataKey", typeof(string));
            dsDestination.Columns.Add("DataText", typeof(string));
            if (GetItemFromSession && !string.IsNullOrEmpty(SessionManager.IncludeExcludeItem))
            {
                string[] includeExcludeItem = SessionManager.IncludeExcludeItem.Split(',');
                foreach (string item in includeExcludeItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        DataRow row = dsDestination.NewRow();
                        row["DataKey"] = item;
                        row["DataText"] = item;
                        dsDestination.Rows.Add(row);
                        DataRow delRow = dsOrigination.Select("Datakey = '" + item + "'").FirstOrDefault();
                        if (delRow != null)
                        {
                            dsOrigination.Rows.Remove(delRow);
                        }
                    }
                }
            }
            else
            {
                SessionManager.IncludeExcludeItem = null;
            }
            uxHierarchyFilter.DataSourceOrigination = dsOrigination;
            uxHierarchyFilter.DataSourceDestination = dsDestination;
            //IsInclude = true;
            //if (_bindIncludeExcludeStatus != null)
            //{
            //    _bindIncludeExcludeStatus(this, EventArgs.Empty);
            //}
        }
        else
        {
            uxHierarchyFilter.DataSourceOrigination = GetFilterList(WebSiteEnums.AssignmentFilterModes.NotAssigned);
            uxHierarchyFilter.DataSourceDestination = GetFilterList(WebSiteEnums.AssignmentFilterModes.Assigned);
        }
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
        parames.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        parames.Add(new FilterParameter("@EntityHierarchyFilterMode", EntityHierarchyFilterMode, DbType.AnsiString));
        parames.Add(new FilterParameter("@EntityHierarchyFilterValue", EntityHierarchyFilterValue, DbType.AnsiString));
        parames.Add(new FilterParameter("@PrimaryID", PrimaryID, DbType.Int32));
        parames.Add(new FilterParameter("@Option", whichMode, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetHierarchyFilter", parames);
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
    public static Dictionary<string, string> GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, string FilterID, string hierarchyFilterMode, bool isTracking = false)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@HierarchyFilterMode", hierarchyFilterMode, DbType.AnsiString));
        parames.Add(new FilterParameter("@EntityHierarchyFilterMode", EntityHierarchyFilterMode, DbType.AnsiString));
        parames.Add(new FilterParameter("@EntityHierarchyFilterValue", EntityHierarchyFilterValue, DbType.AnsiString));
        parames.Add(new FilterParameter("@PrimaryID", primaryID, DbType.Int32));
        parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", mode, DbType.Int32));
        DataTable dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetHierarchyFilter", parames);

        bool IsIncluded = GetIncludeStatus(primaryID, (int)mode, hierarchyFilterMode);
        return BuildeSelectedValues(dt, IsIncluded, hierarchyFilterMode);
    }
    private static bool GetIncludeStatus(string primaryID, int mode, string hierarchyFilterMode)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@PrimaryID", Convert.ToInt32(primaryID), DbType.Int32));
        parames.Add(new FilterParameter("@Mode", Convert.ToInt32(mode), DbType.Int32));
        parames.Add(new FilterParameter("@IsIncluded", false, DbType.Boolean, true));
        parames.Add(new FilterParameter("@HierarchyFilterMode", hierarchyFilterMode, DbType.String));

        FilterParameterCollection paramesOut = new FilterParameterCollection();

        // Get OutParams value
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_GetFilterHierarchy_Info", parames, out paramesOut);

        FilterParameter isIncluded = paramesOut.FindFilterParameterByName("@IsIncluded", true);

        bool result = true;
        if (isIncluded != null)
            result = Convert.ToBoolean(isIncluded.ParameterValue);
        return result;
    }

    private static Dictionary<string, string> BuildeSelectedValues(DataTable dt, bool isInclude, string hierarchyFilterMode)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        MESSAGE_MODE = "<b>" + HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_FilterHierachy_ascx_cs_MessageMode").ToString() + "</b><br />";
        _includedIn = HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_FilterHierachy_ascx_cs_IncludedIn").ToString();
        _excludedFrom = HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_FilterHierachy_ascx_cs_ExcludedFrom").ToString();
        _defaultValueNA = HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_FilterHierachy_ascx_cs_NA").ToString();

        string strTracking = string.Empty;
        StringBuilder str = new StringBuilder();
        DataRow row = SessionManager.RiskHierarchyFilter.FindObject("HierarchyFilterMode", hierarchyFilterMode);

        string hierarchyName = row != null ? row["HierarchyFilterText"].ToString() : hierarchyFilterMode;

        if (dt != null && dt.Rows.Count > 0)
        {
            if (isInclude)
            {
                str.Append(string.Format(MESSAGE_MODE, hierarchyName.ToUpper(), _includedIn));
                strTracking = RM_MCF_GeneralFuncsLib.INCLUDED_KEY;
            }
            else
            {
                str.Append(string.Format(MESSAGE_MODE, hierarchyName.ToUpper(), _excludedFrom));
                strTracking = RM_MCF_GeneralFuncsLib.EXCLUDED_KEY;
            }

                str.Append("\n");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str.Append(dt.Rows[i]["DataText"] + ", ");
                strTracking += dt.Rows[i]["DataText"] + ", ";
            }
        }

        string lblVal = string.IsNullOrEmpty(str.ToString()) ? _defaultValueNA : str.ToString().Trim().TrimEnd(",".ToCharArray());
        string tracking = string.IsNullOrEmpty(strTracking.ToString()) ? "N/A" : strTracking.Trim().TrimEnd(",".ToCharArray());

        data.Add("Label", lblVal);
        data.Add("Tracking", tracking);

        return data;
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
            strBuilder.Append(dt.Rows[i]["DataText"] + ", ");
        }
        result = strBuilder.ToString().Trim().TrimEnd(',');
        if (string.IsNullOrEmpty(result))
            return _defaultValueNA;
        return result;
    }

    public void SetResources()
    {
        _includedIn = HttpContext.GetLocalResourceObject("~/UserControls/rm_MCF_FilterHierarchy.ascx", "Risk_FilterHierachy_ascx_cs_IncludedIn").ToString();
        _excludedFrom = HttpContext.GetLocalResourceObject("~/UserControls/rm_MCF_FilterHierarchy.ascx", "Risk_FilterHierachy_ascx_cs_ExcludedFrom").ToString();
        _defaultValueNA = HttpContext.GetLocalResourceObject("~/UserControls/rm_MCF_FilterHierarchy.ascx", "Risk_FilterHierachy_ascx_cs_NA").ToString();
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
        paramesIn.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        paramesIn.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        paramesIn.Add(new FilterParameter("@PrimaryID", primaryid, DbType.Int32));
        paramesIn.Add(new FilterParameter("@FilterValues", _CodeList, DbType.String));
        paramesIn.Add(new FilterParameter("@isIncluded", IsInclude, DbType.Boolean));
        FilterParameterCollection paramesOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveHierarchyFilter", paramesIn, out paramesOut);
        return 0;
    }

    public string SaveIncludeExcludeISONumber()
    {
        string _CodeList = uxHierarchyFilter.GetSelectedItemsAsString(",");

        //FilterParameterCollection paramesIn = new FilterParameterCollection();
        //paramesIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        //paramesIn.Add(new FilterParameter("@FilterValues", _CodeList, DbType.String));
        //paramesIn.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        //paramesIn.Add(new FilterParameter("@Mode", Mode, DbType.Int32));
        //paramesIn.Add(new FilterParameter("@isIncluded", IsInclude, DbType.Boolean));
        //FilterParameterCollection paramesOut = new FilterParameterCollection();
        //WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_SaveHierarchyFilter", paramesIn, out paramesOut);
        //return _CodeList;
        SessionManager.IncludeExcludeItem = _CodeList;
        return _CodeList;
    }

    public event EventHandler SelectedChanged;

    #endregion
}
