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


public partial class UserControls_Risk_Assignment_User : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        LoadUsers
    }
    enum PostBackAction
    {
    }
    #endregion

    #region properties
    const string USER_LIST = "USER_LIST";
    const string USER_NAME_FIRST = "UserNameFirst";
    const string USER_NAME_LAST = "UserNameLast";
    const string USER_ID = "UserID";


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

    public string UserSelectedAsString
    {
        get { return uxUsersSelector.GetSelectedItemsAsString(","); }
    }

    private static string _defaultValueNA = "";
    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //uxContainer.Width = _WidthUC.ToString();
        //uxUsersSelector.WidthSelector = (_WidthUC - 75) / 2;
        //uxUsersSelector.HeightSelector = _HeightUC;
    }

    
    protected void Page_Load(object sender, EventArgs e)
    {
        _defaultValueNA = GetLocalResourceObject("Risk_Assignment_User_ascx_cs_NA").ToString();
        if (!IsPostBack)
            OnDataBindControls(DataBindAction.LoadUsers);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadUsers:
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
        //Bind Users
        if (PrimaryId == 0)
        {
            listOri = GetUserList(PrimaryId, WebSiteEnums.AssignmentFilterModes.All);
            listDes = listOri.Clone();
        }
        else
        {
            listOri = GetUserList(PrimaryId, WebSiteEnums.AssignmentFilterModes.NotAssigned);
            listDes = GetUserList(PrimaryId, WebSiteEnums.AssignmentFilterModes.Assigned);
        }
        uxUsersSelector.DataSourceOrigination = listOri;
        uxUsersSelector.DataSourceDestination = listDes;
    }

    private DataTable GetUserList(int AssignmentID, WebSiteEnums.AssignmentFilterModes whichMode)
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();

        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", AssignmentID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Mode", whichMode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_rm_cs_GetUserListByAssignment", paramsIn);
    }

    protected void uxUserSelector_OnMovedData(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (SelectedChanged != null)//if event register
                this.SelectedChanged(this, e);//raise event
        }
    }

    private static string DataTableToString(DataTable list)
    {
        string info = string.Empty;
        foreach (DataRow row in list.Rows)
        {
            info += row["DisplayName"].ToString() + "; ";
        }
        return info.Trim().TrimEnd(';');
    }
    
    public static string GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, string paramID, string FilterID)
    {
        string result = string.Empty;
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Mode", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        DataTable tmp = WebServices.RiskServices.GetReports("spa_rm_cs_GetUserListByAssignment", paramsIn);
        result = DataTableToString(tmp);
        
        if (result == string.Empty) 
            result = _defaultValueNA;

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
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", PrimaryId, DbType.Int32));
        paramsIn.Add(new FilterParameter("@UserList", this.UserSelectedAsString, DbType.AnsiString));
        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_AddAssignmentUsers", paramsIn, out paramsOut);
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion
}
