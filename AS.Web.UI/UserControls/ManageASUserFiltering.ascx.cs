using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_ManageASUserFiltering : GlobalUserControl
{
    #region Constants

    private const string SEARCH_TYPE_ALL = "ALL";
    private const string SEARCH_TYPE_USER_TYPE = "USERTYPE";
    private const string SEARCH_TYPE_USER_ROLE = "USERROLE";

    private const string SPA_GET_FILTER_USER_TYPE = "spa_REF_GetFilterUserType";
    private const string SPA_GET_HIERARCHY_FOR_ACCESS_REPORT =
        "spa_SEC_GetHierarchiesForAccessReport";

    #endregion Constants

    #region Enums

    public enum PostbackAction
    {
        Submit
    }

    public enum DataBindAction
    {
        BindUserType,
        BindUserRole
    }

    #endregion Enums

    #region Events

    public event EventHandler Filtering;

    #endregion Events

    #region Properties


    public ManageUserFilterOptions ManageUserFilterOption
    {
        set
        {
            SessionManager.ManageUserFilterOption = value;
        }
        get
        {
            ManageUserFilterOptions opt = new ManageUserFilterOptions();
            opt.SearchType = uxSearchType.SelectedValue;
            opt.SearchValue = null;
            opt.FilterMode = 0;
            opt.RoleId = 0;
            if (uxSearchType.SelectedValue.ToUpper().Trim().Equals(SEARCH_TYPE_USER_TYPE))
            {
                int filterMode = 0;
                int.TryParse(uxcbbSearchValue.SelectedValue, out filterMode);
                opt.FilterMode = filterMode;
            }
            else if (uxSearchType.SelectedValue.ToUpper().Trim().Equals(SEARCH_TYPE_USER_ROLE))
            {
                int roleId = 0;
                int.TryParse(uxcbbUserRole.SelectedValue, out roleId);
                opt.RoleId = roleId;
            }
            else
            {
                opt.SearchValue = uxSearchValue.Text;
            }
            SessionManager.ManageUserFilterOption = opt;
            return opt;
        }
    }

    #endregion Properties

    #region Methods

    #region Protected Methods

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        OnRegisterEvents();
    }

    protected void OnRegisterEvents()
    {
        uxbtnSearch.Click += (s, e) =>
        {
            OnPostBackActions(PostbackAction.Submit);
        };
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindUserType);
            OnDataBindControls(DataBindAction.BindUserRole);
        }
        string searchType = uxSearchType.SelectedValue.ToUpper().Trim();
        switch (searchType)
        {
            case SEARCH_TYPE_ALL:
                pnltxtSearchValue.Attributes.CssStyle.Add("display", "none");
                pnlcbbSearch.Attributes.CssStyle.Add("display", "none");
                pnlcbbSearchRole.Attributes.CssStyle.Add("display", "none");
                break;
            case SEARCH_TYPE_USER_TYPE:
                pnltxtSearchValue.Attributes.CssStyle.Add("display", "none");
                pnlcbbSearchRole.Attributes.CssStyle.Add("display", "none");
                pnlcbbSearch.Attributes.CssStyle.Add("display", "block");
                break;
            case SEARCH_TYPE_USER_ROLE:
                pnltxtSearchValue.Attributes.CssStyle.Add("display", "none");
                pnlcbbSearch.Attributes.CssStyle.Add("display", "none");
                pnlcbbSearchRole.Attributes.CssStyle.Add("display", "block");
                break;
            default:
                pnltxtSearchValue.Attributes.CssStyle.Add("display", "block");
                pnlcbbSearch.Attributes.CssStyle.Add("display", "none");
                pnlcbbSearchRole.Attributes.CssStyle.Add("display", "none");
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindUserType:
                parameters.AddLanguageID();
                uxcbbSearchValue.DataTextField = "DisplayName";
                uxcbbSearchValue.DataValueField = "FilterMode";
                uxcbbSearchValue.DataSource = WebServices.SecurityServices.GetReports(
                    SPA_GET_FILTER_USER_TYPE, parameters);
                uxcbbSearchValue.DataBind();
                uxcbbSearchValue.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(GetLocalResourceObject("ManageUserFilteringASCX_ItemCombobox_All.Text").ToString(), "0"));
                break;
            case DataBindAction.BindUserRole:
                uxcbbUserRole.DataTextField = "HierarchyName";
                uxcbbUserRole.DataValueField = "HierarchyID";
                uxcbbUserRole.DataSource = WebServices.SecurityServices.GetReports(SPA_GET_HIERARCHY_FOR_ACCESS_REPORT, parameters);
                uxcbbUserRole.DataBind();
                uxcbbUserRole.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(GetLocalResourceObject("ManageUserFilteringASCX_ItemCombobox_All.Text").ToString(), "0"));
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostbackAction)type)
        {
            case PostbackAction.Submit:
                if (Filtering != null)
                {
                    Filtering(sender, new EventArgs());
                }
                break;
        }
    }

    #endregion Protected Methods

    #endregion Methods
}

