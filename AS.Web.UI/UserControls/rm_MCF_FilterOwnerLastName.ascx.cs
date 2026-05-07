using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_FilterOwnerLastName : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        LoadProfiles
    }
    #endregion
    static string PAGE_PATH = "~/UserControls/rm_MCF_FilterOwnerLastName.ascx";
    static string MESSAGE_MODE = string.Empty;
    static string _includedIn = string.Empty;
    static string _excludedFrom = string.Empty;
    static string _defaultValueNA = string.Empty;
    #region Properties
    private int PrimaryId = 2;
    public string ProfileSelectedString
    {
        get { return uxOwnerLastName.GetSelectedItemsAsString(","); }
    }
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
    public bool IsInclude { get; set; }
    private static bool IsIncluded;
    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //containerProfile.Width = _WidthUC.ToString();
        //uxOwnerLastName.WidthSelector = (_WidthUC - 75) / 2;
        //uxOwnerLastName.HeightSelector = _HeightUC;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        MESSAGE_MODE = "<b>" + GetLocalResourceObject("Risk_FilterOwnerLastName_ascx_cs_MessageCode").ToString() + "</b><br />";
        _includedIn = GetLocalResourceObject("Risk_Filter_ascx_cs_IncludedIn").ToString();
        _excludedFrom = GetLocalResourceObject("Risk_Filter_ascx_cs_ExcludedFrom").ToString();
        _defaultValueNA = GetLocalResourceObject("Risk_Filter_ascx_cs_NA").ToString();
        if (!IsPostBack)
        {
            //BindData();
            OnDataBindControls(DataBindAction.LoadProfiles);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadProfiles:
                BindData();
                break;
        }
    }

    private void BindData()
    {
        if (string.IsNullOrEmpty(PrimaryID))
            return;
        if (!Int32.TryParse(this.PrimaryID, out PrimaryId))
            return;

        DataTable listOri = null;
        DataTable listDes = null;
        if (Int32.Parse(PrimaryID) == 0)
        {
            listOri = GetFilterList(WebSiteEnums.AssignmentFilterModes.All);
            listDes = listOri.Clone();
        }
        else
        {
            listOri = GetFilterList(WebSiteEnums.AssignmentFilterModes.NotAssigned);
            listDes = GetFilterList(WebSiteEnums.AssignmentFilterModes.Assigned);
        }
        foreach (DataRow item in listOri.Rows)
        {
            item["DataKey"] = EncodeString(item["DataKey"].ToString());
        }

        foreach (DataRow item in listDes.Rows)
        {
            item["DataKey"] = EncodeString(item["DataKey"].ToString());
        }
        uxOwnerLastName.DataSourceOrigination = listOri;
        uxOwnerLastName.DataSourceDestination = listDes;
    }

    private string EncodeString(string val)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(val));
    }

    private string DecodeString(string val)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(val));
    }

    /// <summary>
    /// Get Filter List
    /// </summary>
    /// <param name="whichMode">whichMode</param>
    /// <returns>Datatable</returns>
    private DataTable GetFilterList(WebSiteEnums.AssignmentFilterModes whichMode)
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.AddLanguageID();
        paramsIn.Add(new FilterParameter("@PrimaryID", Int32.Parse(this.PrimaryID), DbType.Int32));
        paramsIn.Add(new FilterParameter("@Option", whichMode, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetFilterOwnerLastName", paramsIn);
    }


    protected void MultiSelector_OnMovedData(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (SelectedChanged != null)//if event register
                this.SelectedChanged(this, e);//raise event
        }
    }

    public static Dictionary<string, string> GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, string FilterID)
    {
        bool merchantRankIncluded = false;  //dummy value for now, but this will be the output parameter of the query

        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.AddLanguageID();
        paramsIn.Add(new FilterParameter("@PrimaryID", primaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Mode", (int)mode, DbType.Int32));
        DataSet ds = WebServices.RiskServices.GetReportsAsDataSet("spa_RM_MCF_GetFilterOwnerLastName", paramsIn);

        //there could be missing rows. Use default value if this is the case
        if (ds.Tables[1].Rows.Count > 0)
            merchantRankIncluded = ds.Tables[1].Rows[0]["IsIncluded"].ToBoolean(); //TK 30650: this is required for include/exclude on profile assignment

        return BuildeSelectedValues(ds.Tables[0], merchantRankIncluded);
    }

    private static Dictionary<string, string> BuildeSelectedValues(DataTable dt, bool isInclude)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        MESSAGE_MODE = "<b>" + HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_FilterOwnerLastName_ascx_cs_MessageCode").ToString() + "</b><br />";
        _includedIn = HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_Filter_ascx_cs_IncludedIn").ToString();
        _excludedFrom = HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_Filter_ascx_cs_ExcludedFrom").ToString();
        _defaultValueNA = HttpContext.GetLocalResourceObject(PAGE_PATH, "Risk_Filter_ascx_cs_NA").ToString();

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
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str.Append(dt.Rows[i]["DataText"] + ", ");
            tracking += dt.Rows[i]["DataKey"].ToString() + ", ";
        }

        string value = string.IsNullOrEmpty(str.ToString()) ? _defaultValueNA : str.ToString().Trim().TrimEnd(",".ToCharArray());
        tracking = string.IsNullOrEmpty(tracking) ? _defaultValueNA : tracking.Trim().TrimEnd(",".ToCharArray());

        data.Add("Label", value);
        data.Add("Tracking", tracking);

        return data;
    }
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

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    public int Save()
    {
        if (string.IsNullOrEmpty(PrimaryID))
            return 1;
        if (!Int32.TryParse(this.PrimaryID, out PrimaryId))
            return 2;

        List<string> encodeProfileString = new List<string>();

        foreach (string item in this.ProfileSelectedString.Split(','))
        {
            encodeProfileString.Add(DecodeString(item));
        }
        string ProfileList = string.Join(",", encodeProfileString);
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        FilterParameterCollection paramsOut = new FilterParameterCollection();

        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32);
        paramsIn.Add("@ProfileList", ProfileList, DbType.String);
        paramsIn.Add("@Mode", this.Mode, DbType.Int32);
        paramsIn.Add("@IsIncluded", IsInclude, DbType.Boolean);
        IsIncluded = IsInclude;
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveOwnerLastNameAssignment", paramsIn, out paramsOut);

        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
}