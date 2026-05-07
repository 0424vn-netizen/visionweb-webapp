using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Web.UI.Controls;
using AS.Common.DBManager;

public partial class UserControls_UserAccessFiltering : GlobalUserControl
{
    public const string Filter_All = "All";
    public enum PostbackAction
    {
        Submit
    }
    public enum DataBindAction
    {
        BindUserType,
        BindUserRole
    }

    public event EventHandler Filtering;
    protected DateTime FromDate
    {
        get
        {
            if (GeneralFuncsLib.HasExtendedSetting("FILTERING_OPTIONS_DATERANGE")) 
            {
                return DateTime.Now.AddDays(-90);
            }
            else
            {
                return DateTime.Now.AddDays(-30);
            }
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        OnRegisterEvents();
    }


    public UserAccessFilterOptions UserAccessFilterOption
    {
        set
        {
            SessionManager.UserAccessFilterOption = value;
        }
        get
        {
            UserAccessFilterOptions op = new UserAccessFilterOptions();
            if (uxtadDaily.Checked)
            {
                op.DateFilterMode = (int)DateOptionMode.Daily;
                op.FromDate = uxDateFrom.SelectedDate;
            }
            if (uxradMonthly.Checked)
            {
                op.DateFilterMode = (int)DateOptionMode.Monthly;
                op.FromDate = uxDateFrom.SelectedDate;
            }
            if (uxradDateRange.Checked)
            {
                op.DateFilterMode = (int)DateOptionMode.DateRange;
                op.FromDate = uxDateFrom.SelectedDate;
                op.ToDate = uxDateTo.SelectedDate;

            }

            op.SearchType = uxSearchType.SelectedValue;
            if (uxSearchType.SelectedValue.ToUpper().Trim() == "USERTYPE")
            {
                op.SearchValue = null;
                int filterMode = 0;
                int.TryParse(uxcbbSearchValue.SelectedValue, out filterMode);
                op.FilterMode = filterMode;
                op.FilterValue = uxFilterVal.Text;

                if (uxFilterVal.Text.ToLower() == Filter_All.ToLower())
                    op.FilterValue = string.Empty;
            }
            else if (uxSearchType.SelectedValue.ToUpper().Trim() == "USERROLE")
            {
                op.SearchValue = null;
                op.FilterMode = 0;
                int HierarchyID = 0;
                int.TryParse(uxcbbUserRole.SelectedValue, out HierarchyID);
                op.HierarchyID = HierarchyID;
            }
            else
            {
                op.SearchValue = uxSearchValue.Text;
                op.FilterMode = 0;
            }
            SessionManager.UserAccessFilterOption = op;
            return op;
        }
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
            uxDateFrom.SelectedDate = FromDate;
            uxDateTo.SelectedDate = DateTime.Now;
            OnDataBindControls(DataBindAction.BindUserType);
            OnDataBindControls(DataBindAction.BindUserRole);
        }
        if (uxSearchType.SelectedValue.ToUpper().Trim() == "ALL")
        {
            pnltxtSearchValue.Attributes.CssStyle.Add("display", "none");
            pnlcbbSearch.Attributes.CssStyle.Add("display", "none");
            pnlcbbSearchRole.Attributes.CssStyle.Add("display", "none");
            uxtr.Attributes.Add("class", "display-none");
            pnlFilterVal.Attributes.CssStyle.Add("display", "none");
        }
        else if (uxSearchType.SelectedValue.ToUpper().Trim() == "USERTYPE")
        {
            uxtr.Attributes.Remove("class");
            pnltxtSearchValue.Attributes.CssStyle.Add("display", "none");
            pnlcbbSearchRole.Attributes.CssStyle.Add("display", "none");
            pnlcbbSearch.Attributes.CssStyle.Add("display", "block");

            pnlFilterVal.Attributes.Add("class", "display-none");

            if (!uxcbbSearchValue.SelectedValue.ToString().Equals("0"))
            {
                pnlFilterVal.Attributes.CssStyle.Add("display", "block");
            }
            else
            {
                pnlFilterVal.Attributes.CssStyle.Add("display", "none");
            }
        }
        else if (uxSearchType.SelectedValue.ToUpper().Trim() == "USERROLE")
        {
            uxtr.Attributes.Remove("class");
            pnltxtSearchValue.Attributes.CssStyle.Add("display", "none");
            pnlcbbSearch.Attributes.CssStyle.Add("display", "none");
            pnlcbbSearchRole.Attributes.CssStyle.Add("display", "block");
            pnlFilterVal.Attributes.CssStyle.Add("display", "none");
        }
        else
        {
            uxtr.Attributes.Remove("class");
            pnltxtSearchValue.Attributes.CssStyle.Add("display", "block");
            pnlcbbSearchRole.Attributes.CssStyle.Add("display", "none");
            pnlcbbSearch.Attributes.CssStyle.Add("display", "none");
            pnlFilterVal.Attributes.CssStyle.Add("display", "none");
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
                uxcbbSearchValue.DataSource = WebServices.SecurityServices.GetReports("spa_REF_GetFilterUserType", parameters);
                uxcbbSearchValue.DataBind();
                uxcbbSearchValue.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(GetLocalResourceObject("RadComboBoxItemAll.Text").ToString(), "0"));
                break;
            case DataBindAction.BindUserRole:
                uxcbbUserRole.DataTextField = "HierarchyName";
                uxcbbUserRole.DataValueField = "HierarchyID";
                uxcbbUserRole.DataSource = WebServices.SecurityServices.GetReports("spa_SEC_GetHierarchiesForAccessReport", parameters);
                uxcbbUserRole.DataBind();
                uxcbbUserRole.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(GetLocalResourceObject("RadComboBoxItemAll.Text").ToString(), "0"));

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


}
