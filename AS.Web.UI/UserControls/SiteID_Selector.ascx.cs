using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using System.Collections.Specialized;

public partial class UserControls_SiteID_Selector : GlobalUserControl
{
    public event EventHandler SelectedChanged;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (!IsPostBack)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams(false);
            uxSiteIDs.DataSource = WebServices.CsReportServices.GetReports("spa_rm_GetAllRiskSites", parameters);
            uxSiteIDs.DataBind();
            if (uxSiteIDs.Items.Count <= 1)
            {
                if (uxSiteIDs.Items.Count == 1) SessionManager.CurrentRiskSiteID = int.Parse(uxSiteIDs.SelectedValue);
            }
            else
            {
                uxSiteIDs.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(GetLocalResourceObject("SiteID_Selector_ascx_cs_All").ToString(), "0"));
                uxSiteIDs.SelectedValue = SessionManager.CurrentRiskSiteID.ToString();
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (uxSiteIDs.Items.Count <= 1)
            this.Visible = false;
        else
            this.Visible = true;
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        if (SessionManager.CurrentRiskSiteID != int.Parse(uxSiteIDs.SelectedValue))
        {
            if (RiskSessionManager.DetectionQueue != null)
            {
                //RiskSessionManager.DetectionQueue = null;
                RiskSessionManager.DetectionQueue.AssignmentID = -1;
            }
            if (SelectedChanged != null)//checked event have been defined
            {
                SelectedChanged(this, e);//fire event
            }
        }

        SessionManager.CurrentRiskSiteID = int.Parse(uxSiteIDs.SelectedValue);
        if (Request.QueryString.ToString().IsNullOrEmpty())
        {
            Response.Redirect(Request.RawUrl + "?t=" + DateTime.Now.Ticks);
        }
        else
        {
            string q = "";
            NameValueCollection queries = Request.QueryString;
            for (int i = 0; i < queries.Count; i++)
            {
                if (queries.Keys[i].ToLower() != "t")
                {
                    q += queries.Keys[i] + "=" + HttpUtility.UrlEncode(queries[i]) + "&";
                }
            }
            q = q.Trim('&');
            if (q.IsNullOrEmpty())
            {
                Response.Redirect(Request.Url.AbsolutePath + "?t=" + DateTime.Now.Ticks);
            }
            else
            {
                Response.Redirect(Request.Url.AbsolutePath + "?" + q + "&t=" + DateTime.Now.Ticks);
            }
        }
    }
}
