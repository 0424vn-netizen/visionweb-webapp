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

public partial class UserControls_rm_MCF_ParameterFilter_ACHReturnCode : GlobalUserControl, IRiskParamFilter
{

    #region Enums
    static string PAGE_PATH = "~/UserControls/rm_MCF_ParameterFilter_ACHReturnCode.ascx";
    enum DataBindAction
    {
        LoadHierarchysList
    }
    #endregion

    #region properties

    private int _WidthUC = 800;
    private int _HeightUC = 200;
    private int _ItemID = 2002;

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
    public bool IsInclude { get; set; }
    public bool Enabled {  set{ uxHierarchyFilter.Enabled = value; } }


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
        _textNA = "N/A";
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
                BindMultiSelector();
                break;
        }
    }
    /// <summary>
    /// Bind data for Multiselector
    /// </summary>
    private void BindMultiSelector()
    {
        uxHierarchyFilter.DataSourceOrigination = GetFilterList(WebSiteEnums.AssignmentFilterModes.All);
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
        parames.AddLanguageID();
        parames.Add(new FilterParameter("@AssignmentID", PrimaryID, DbType.Int32));
        parames.Add(new FilterParameter("@ParameterCode", this.ParamID, DbType.String));
        parames.Add(new FilterParameter("@IsSelected", whichMode, DbType.Boolean));
        parames.Add(new FilterParameter("@PageID", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_Get_AssignmentParameterFilter", parames);
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
    public static string GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, ref bool included)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.AddLanguageID();
        parames.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        parames.Add(new FilterParameter("@ParameterCode", paramID, DbType.String));
        parames.Add(new FilterParameter("@IsSelected", true, DbType.Boolean));
        parames.Add(new FilterParameter("@PageID", mode, DbType.Int32));
        DataSet ds = WebServices.RiskServices.GetReportsAsDataSet("spa_RM_MCF_Get_AssignmentParameterFilter", parames);

        //there could be missing rows. Use default value if this is the case
        if (ds.Tables[1].Rows.Count > 0)
            included = ds.Tables[1].Rows[0]["IsIncluded"].ToBoolean(); //TK 30650: this is required for include/exclude on profile assignment

        return BuildeSelectedValues(ds.Tables[0], included);
    }

    private static string BuildeSelectedValues(DataTable dt, bool isInclude)
    {
        var includedIn = "Included";// HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_Filter_ACHReturnCode_ascx_cs_IncludedIn").ToString();
        var excludedFrom = "Excluded"; // HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_Filter_ACHReturnCode_ascx_cs_ExcludedFrom").ToString();
        StringBuilder str = new StringBuilder();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str.Append(dt.Rows[i]["DataKey"] + ", ");
        }

        return string.IsNullOrEmpty(str.ToString()) ? string.Empty : (isInclude? includedIn: excludedFrom) + " - "+ str.ToString().Trim().TrimEnd(",".ToCharArray());
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
        paramesIn.Add(new FilterParameter("@ParameterCode ", this.ParamID, DbType.String));
        paramesIn.Add(new FilterParameter("@PageID", Mode, DbType.Int32));
        paramesIn.Add(new FilterParameter("@SelectedItems", _CodeList, DbType.String));
        paramesIn.Add("@IsIncluded", IsInclude, DbType.Boolean);

        FilterParameterCollection paramesOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_Save_AssignmentParameterFilter", paramesIn, out paramesOut);
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
 
}
