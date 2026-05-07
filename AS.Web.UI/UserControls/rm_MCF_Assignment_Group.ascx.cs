using System;
using System.Collections;
using System.Collections.Generic;
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

public partial class UserControls_rm_MCF_Assignment_Group : GlobalUserControl, IRiskParamFilter
{
    #region properties

    static string _defaultValueNA = "N/A";

    const string GROUP_NAME = "GroupName";


    private int PrimaryId = 0;
    private int _WidthUC = 800;
    private int _HeightUC = 300;

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

    public string GroupSelectedAsString
    {
        get { return uxGroupsSelector.GetSelectedItemsAsString(","); }
    }
    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //uxContainer.Width = _WidthUC.ToString();
        //uxGroupsSelector.WidthSelector = (_WidthUC - 75) / 2;
        //uxGroupsSelector.HeightSelector = _HeightUC;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _defaultValueNA = GetLocalResourceObject("Risk_Assignment_Group_ascx_cs_NA").ToString();
        if (!IsPostBack)
            this.BindData();
    }

    private void BindData()
    {
        if (string.IsNullOrEmpty(PrimaryID))
            return;
        if (!Int32.TryParse(this.PrimaryID, out PrimaryId))
            return;

        DataTable listOri = null;
        DataTable listDes = null;
        //Bind Users
        if (PrimaryId == 0)
        {
            listOri = GetGroupList(PrimaryId, WebSiteEnums.AssignmentFilterModes.All);
            listDes = listOri.Clone();
        }
        else
        {
            listOri = GetGroupList(PrimaryId, WebSiteEnums.AssignmentFilterModes.NotAssigned);
            listDes = GetGroupList(PrimaryId, WebSiteEnums.AssignmentFilterModes.Assigned);
        }
        uxGroupsSelector.DataSourceOrigination = listOri;
        uxGroupsSelector.DataSourceDestination = listDes;
    }

    private DataTable GetGroupList(int AssignmentID, WebSiteEnums.AssignmentFilterModes whichMode)
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", AssignmentID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Mode", whichMode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetGroupListByAssignment", paramsIn);
    }

    protected void uxGroupsSelector_OnMovedData(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (SelectedChanged != null)//if event register
                this.SelectedChanged(this, e);//raise event
        }
    }

    private static Dictionary<string, string> DataTableToString(DataTable list)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        string info = string.Empty;
        string tracking = string.Empty;
        if(list.Rows.Count <= 0)
        {
            info = tracking = _defaultValueNA;
        }

        foreach (DataRow row in list.Rows)
        {
            info += row["GroupName"].ToString() + ", ";
            tracking += row["GroupName"].ToString() + ", ";
        }

        data.Add("Label", info.Trim().TrimEnd(','));
        data.Add("Tracking", tracking.Trim().TrimEnd(','));

        return data;
    }

    public static Dictionary<string, string> GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, string FilterID)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Mode", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        DataTable tmp = WebServices.RiskServices.GetReports("spa_RM_MCF_GetGroupListByAssignment", paramsIn);
        
        return DataTableToString(tmp);
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

        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", PrimaryId, DbType.Int32));
        paramsIn.Add(new FilterParameter("@GroupList", this.GroupSelectedAsString, DbType.AnsiString));

        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_AddAssignmentGroups", paramsIn, out paramsOut);

        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
}
