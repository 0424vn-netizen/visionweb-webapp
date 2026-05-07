using AS.Common.DBManager;
using AS.VW.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

public partial class UserControls_rm_MCF_CustomView : GlobalUserControl
{
    #region Variable & Enums

    public DataTable DataSource { get; set; }

    public string CustomViewID
    {
        set { ViewState["_CustomViewID"] = value; }
        get
        {
            if (ViewState["_CustomViewID"] == null)
                return string.Empty;
            else return ViewState["_CustomViewID"].ToString();
        }
    }

    public WebSiteEnums.PageModeEnums PageMode { get; set; }
    public WebSiteEnums.PageSectionEnums PageSection { get; set; }

    public int AssignmentID
    {
        get
        {
            if (this.Page.SecureQueryString["AssignmentID"] != null)
            {
                return Convert.ToInt32(this.Page.SecureQueryString["AssignmentID"].ToString());
            }

            return 0;
        }
    }

    public bool IsRebindData { get; set; }

    #endregion

    protected override void OnInit(EventArgs e)
    {
        if (!IsPostBack)
        {
            GetData();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsRebindData)
        {
            RebindData();
        }

    }

    protected void uxCustomView_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        CustomViewID = uxCustomView.SelectedValue;
        BuildCustomViewLink();

        //save session 
        if (PageMode == WebSiteEnums.PageModeEnums.DetectionQueue)
        {
            var session = RiskSessionManager.RiskMCFDQRainbowReport;
            if (session != null && session.ContainsKey(AssignmentID))
            {
                session[AssignmentID] = e.Value;
            }
            RiskSessionManager.RiskMCFDQRainbowReport = session;

            ReportPage page = (ReportPage)this.Page;
            switch (PageSection)
            {
                case WebSiteEnums.PageSectionEnums.SecurityReport:
                    page.AjaxAddResponseScript("parent.RebindGrid_CustomViewChange();");
                    break;
                case WebSiteEnums.PageSectionEnums.NextQueue:
                    page.AjaxAddResponseScript("parent.RebindBarometer_NextQueue();");
                    break;
                default:
                    page.AjaxAddResponseScript("parent.RebindGrid_CustomViewChange();");
                    break;
            }

        }
    }

    public void GetData()
    {
        DataTable tb = GetViewColumn();
        DataSource = tb;
        uxCustomView.DataSource = tb;
        uxCustomView.DataBind();

        DataRow[] defaultRow = tb.Select("IsDefault = 1");
        string defaultView = string.Empty;
        if (defaultRow != null && defaultRow.Count() > 0)
        {
            defaultView = defaultRow[0]["CustomViewID"].ToString();
            uxCustomView.SelectedValue = defaultView;
            CustomViewID = defaultView;
        }

        BuildCustomViewLink();

        if (tb != null && tb.Rows.Count > 0 && PageMode == WebSiteEnums.PageModeEnums.DetectionQueue)
        {
            bool isExist = false;
            Dictionary<int, string> session = RiskSessionManager.RiskMCFDQRainbowReport;
            if (session != null && session.ContainsKey(AssignmentID))
            {
                DataRow[] checkExsit = tb.Select("CustomViewID = " + session[AssignmentID].ToString());
                isExist = (checkExsit != null && checkExsit.Count() > 0);
            }

            if (isExist)
            {
                uxCustomView.SelectedValue = Convert.ToString(session[AssignmentID]);
            }
            else
            {
                uxCustomView.SelectedValue = defaultView;
                session.Remove(AssignmentID);
                session.Add(AssignmentID, defaultView);
                RiskSessionManager.RiskMCFDQRainbowReport = session;
            }
        }
    }

    private DataTable GetViewColumn()
    {
        int viewMode = PageMode == WebSiteEnums.PageModeEnums.Assignment ? 2 : 3;

        var parameters = new FilterParameterCollection();
        parameters.AddLanguageID();
        parameters.AddLoggedInUserParamsWithRecId();
        parameters.Add(new FilterParameter("@ViewMode", viewMode, System.Data.DbType.Int32));
        parameters.Add(new FilterParameter("@PageType", "Assignment", System.Data.DbType.String));
        parameters.Add(new FilterParameter("@AssignmentID", AssignmentID, DbType.Int32));

        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetCustomViewList", parameters);
    }

    private void BuildCustomViewLink()
    {
        if (Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW_MS) ||
                Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW_MS) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW))
        {
            uxCustomizeColumnLink.Visible = true;
            string url = string.Format("AssignmentID={0}&PageMode={1}&CustomViewID={2}&customViewMessageResourceTypeMode={3}", AssignmentID, PageMode.ToString(), CustomViewID, CustomViewMessageResourceType.Assignments.ToString());
            string encodeURL = string.Format("rm_MCF_ManageCustomModal.aspx?{0}", this.Page.BuildSecureQueryString(url));
            uxCustomizeColumnLink.OnClientClick = "return doOpenCustomViewPopup('" + encodeURL + "'); return false;";
        }
        else
        {
            uxCustomizeColumnLink.Visible = false;
        }
    }

    protected void btnRebind_Click(object sender, EventArgs e)
    {
        RebindData();
    }

    private void BuildJSClient()
    {
        var page = this.Page;

        string js = string.Empty;
        js = "var rm_MCF_CustomView_hdCustomViewID ='" + hdCustomViewID.ClientID + "';" +
            "var rm_MCF_CustomView_PageMode ='" + PageMode.ToString() + "';" +
            "var rm_MCF_CustomView_btnRebind ='" + btnRebind.ClientID + "';" +
            "var rm_MCF_CustomView_PageSection ='" + PageSection + "';";

        page.ClientScript.RegisterStartupScript(this.GetType(), "test", js, true);
        page.ClientScript.RegisterClientScriptInclude("Registration", ResolveUrl("~/res/js/risk_MCF/rm_MCF_CustomView.js"));
    }
    private void RebindData()
    {
        if (!string.IsNullOrEmpty(hdCustomViewID.Value))
        {
            DataTable tb = GetViewColumn();
            DataSource = tb;
            uxCustomView.DataSource = tb;
            uxCustomView.DataBind();

            int customViewSelected = int.Parse(CryptorServices.Current.DecryptText(hdCustomViewID.Value));
            uxCustomView.SelectedValue = customViewSelected.ToString();
            CustomViewID = customViewSelected.ToString();

            BuildCustomViewLink();

            if (PageMode == WebSiteEnums.PageModeEnums.DetectionQueue)
            {
                Dictionary<int, string> session = RiskSessionManager.RiskMCFDQRainbowReport;
                if (session != null && session.ContainsKey(AssignmentID))
                {
                    session[AssignmentID] = CustomViewID;
                }

                ReportPage page = (ReportPage)this.Page;
                switch (PageSection)
                {
                    case WebSiteEnums.PageSectionEnums.NextQueue:
                        page.AjaxAddResponseScript("parent.RebindBarometer_NextQueue();");
                        break;
                    case WebSiteEnums.PageSectionEnums.Assignment:
                    case WebSiteEnums.PageSectionEnums.BarometerReport:
                        page.AjaxAddResponseScript("parent.RebindGrid_CustomViewChange();");
                        break;
                }
            }
        }
        else
        {
            DataTable tb = GetViewColumn();
            DataSource = tb;
            string seletedView = uxCustomView.SelectedValue;
            DataRow[] seletedRow = tb.Select("CustomViewID = " + seletedView);

            uxCustomView.DataSource = tb;
            uxCustomView.DataBind();

            if (seletedRow != null && seletedRow.Count() > 0)
            {
                uxCustomView.SelectedValue = seletedView;
                CustomViewID = seletedView;
            }
            else
            {
                DataRow[] defaultRow = tb.Select("IsDefault = 1");
                string defaultView = defaultRow[0]["CustomViewID"].ToString();
                uxCustomView.SelectedValue = defaultView;
                CustomViewID = defaultView;
            }

            BuildCustomViewLink();

            if (PageMode == WebSiteEnums.PageModeEnums.DetectionQueue)
            {
                if (!(seletedRow != null && seletedRow.Count() > 0))
                {
                    Dictionary<int, string> session = RiskSessionManager.RiskMCFDQRainbowReport;
                    if (session != null && session.ContainsKey(AssignmentID))
                    {
                        session[AssignmentID] = CustomViewID;
                    }
                }

                ReportPage page = (ReportPage)this.Page;
                switch (PageSection)
                {
                    case WebSiteEnums.PageSectionEnums.NextQueue:
                        page.AjaxAddResponseScript("parent.RebindBarometer_NextQueue();");
                        break;
                    case WebSiteEnums.PageSectionEnums.Assignment:
                    case WebSiteEnums.PageSectionEnums.BarometerReport:
                        page.AjaxAddResponseScript("parent.RebindGrid_CustomViewChange();");
                        break;
                }
            }
        }

        hdCustomViewID.Value = string.Empty;
    }

    protected void uxLoadJs_Load(object sender, EventArgs e)
    {
        BuildJSClient();
    }
}