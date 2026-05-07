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

public partial class UserControls_Risk_FilterZipCode : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        LoadProfiles
    }
    #endregion
    static string MESSAGE_MODE = string.Empty;
    static string _includedIn = string.Empty;
    static string _excludedFrom = string.Empty;
    static string _defaultValueNA = string.Empty;
    #region Properties
    private int PrimaryId = 2;
    public string ProfileSelectedString
    {
        get { return uxZipCode.GetSelectedItemsAsString(","); }
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
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        MESSAGE_MODE = "<b>" + GetLocalResourceObject("Risk_FilterZipCode2_ascx_cs_MessageCode").ToString() + "</b><br />";
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
        uxZipCode.DataSourceOrigination = listOri;
        uxZipCode.DataSourceDestination = listDes;
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
        return WebServices.RiskServices.GetReports("spa_rm_cs_GetFilterZip", paramsIn);
    }


    protected void MultiSelector_OnMovedData(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (SelectedChanged != null)//if event register
                this.SelectedChanged(this, e);//raise event
        }
    }

    public static string GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, string FilterID)
    {
        bool merchantRankIncluded = false;  //dummy value for now, but this will be the output parameter of the query

        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.AddLanguageID();
        paramsIn.Add(new FilterParameter("@PrimaryID", primaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Mode", (int)mode, DbType.Int32));
        DataSet ds = WebServices.RiskServices.GetReportsAsDataSet("spa_rm_cs_GetFilterZip", paramsIn);

        //there could be missing rows. Use default value if this is the case
        if (ds.Tables[1].Rows.Count > 0)
            merchantRankIncluded = ds.Tables[1].Rows[0]["IsIncluded"].ToBoolean(); //TK 30650: this is required for include/exclude on profile assignment

        return BuildeSelectedValues(ds.Tables[0], merchantRankIncluded);
        //return BuildeSelectedValues(dt, IsIncluded);
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
            str.Append(dt.Rows[i]["DataText"] + ", ");
        }

        return string.IsNullOrEmpty(str.ToString()) ? _defaultValueNA : str.ToString().Trim().TrimEnd(",".ToCharArray());
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
        string ProfileList = this.ProfileSelectedString;
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        FilterParameterCollection paramsOut = new FilterParameterCollection();

        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32);
        paramsIn.Add("@ProfileList", ProfileList, DbType.String);
        paramsIn.Add("@Mode", this.Mode, DbType.Int32);
        paramsIn.Add("@IsIncluded", IsInclude, DbType.Boolean);
        IsIncluded = IsInclude;
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_SaveZipAssignment", paramsIn, out paramsOut);

        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
}
