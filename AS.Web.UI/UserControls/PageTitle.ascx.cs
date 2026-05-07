using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AS.Common;
using AS.Common.Logger;

//using AS.Common;

public partial class UserControls_PageTitle : GlobalUserControl
{
    public UserControls_PageTitle()
    {
        HasFilteringOption = true;
        HasShowHierarchy = true;
        HasMarginBottom = true;
    }
    public string HierarchyTitle { get; set; }   
    public bool HasFilteringOption { get; set; }
    public bool HasShowHierarchy  { get; set; }
    public bool HasMarginBottom { get; set; }
    public bool HasPageTitle { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

        }
        catch (ViewStateException)
        {
            LoggerManager.Error(string.Format("UserControls_PageTitle - ViewStateException - Request={0}; SessionID={1}", Context.Request.Url, Session.SessionID));
            throw;
        }
    }
    private void ShowHierarchy()
    {
        string hiearchy = SessionManager.CurrentUser.EntityID;
        if (!SessionManager.HierarchyName.IsNullOrEmpty())
            hiearchy += " - " + SessionManager.HierarchyName;
        uxLtrHierarchy.Text = VeraCodeSolution.ValidateResponseData(hiearchy);
    }
    protected override void OnPreRender(EventArgs e)
    {
        try
        {
            uxTitle.Visible = !string.IsNullOrEmpty(ReportTitle);
            if (SessionManager.CurrentUserType.ToString() == WebSiteEnums.UserHierarchyMode.Hierarchy.ToString())
            {
                pnlHierarchyTitle.Visible = true;
                if (!HasMarginBottom)
                {
                    pnlHierarchyTitle.Attributes.Add("class", "hierarchy-title no-margin-bottom");
                    if (ReportTitle.IsNullOrEmpty())
                    {
                        pnlHierarchyTitle.Attributes.Add("class", "hierarchy-title no-margin-bottom no-report-title");
                    }
                }

                if (HasShowHierarchy) ShowHierarchy();
            }
            else
            {
                pnlHierarchyTitle.Visible = false;
            }
            string marginBottomClass = !HasPageTitle && !HasMarginBottom ? " no-margin-bottom" : "";
            if (HasFilteringOption)
            {

                uxTitle.Attributes.Add("class", "report-title" + marginBottomClass);
            }
            else
            {
                uxTitle.Attributes.Add("class", "report-title-no-filter" + marginBottomClass);
            }
            lblReportTitle.Text = ReportTitle;
            base.OnPreRender(e);
        }
        catch (ViewStateException)
        {
            LoggerManager.Error(string.Format("UserControls_PageTitle - ViewStateException - Request={0}; SessionID={1}", Context.Request.Url, Session.SessionID));
            throw;
        }
    }
    public string ReportTitle { get; set; }
   
}
