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
using AS.Controls.Pages;

[PagePermission("ManCase,MSManCase")]
public partial class CaseModal2 : NonReportPage
{
    #region Enums
    enum PostBackAction
    {
        ClickYes
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.ClickYes:
                var parameters = new FilterParameterCollection();

                FilterParameterCollection tempParameters = null;
                parameters.AddLoggedInUserParams(-1);
                parameters.Add(new FilterParameter("@SiteID", int.Parse(SecureQueryString["SiteID"]), DbType.Int32));
                const string ASSIGN_TO = "@AssignedTo";
                const string TCK_NUMBER = "@TicketNumber";
                const string STATUS = "@Status";
                const string RESOL = "@Resolution";

                parameters
                    .Add(ASSIGN_TO, SecureQueryString["ato"], DbType.AnsiString)
                    .Add(TCK_NUMBER, int.Parse(SecureQueryString["TicketNumber"]), DbType.Int32)
                    .Add(STATUS, int.Parse(SecureQueryString["sta"]), DbType.Int32)
                    .Add(RESOL, int.Parse(SecureQueryString["res"]), DbType.Int32);

                string issueList = SecureQueryString["issueList"];
                const string ISSUE_LIST_PARA = "@IssueList";
                //const string TID = "TicketNumber";

                if (issueList.Length > 0)
                {
                    issueList.Remove(issueList.Length - 1, 1);
                    parameters.Add(new FilterParameter(ISSUE_LIST_PARA, issueList.ToString(), DbType.AnsiString));
                }
                else
                {
                    parameters.Add(new FilterParameter(ISSUE_LIST_PARA, "", DbType.AnsiString));
                }

                WebServices.CsReportServices.ExecuteNonQueryCommand("spa_cm_UpdateTicketCaseManagement", parameters, out tempParameters);

                // End of Manage Ticket-----------------------------------------------

                // Insert Comment--------------------------------------
                parameters.Clear();
                parameters.AddLoggedInUserParams(-1);
                parameters.Add(new FilterParameter("@TicketNumber", int.Parse(SecureQueryString["TicketNumber"]), DbType.Int32));
                parameters.Add(new FilterParameter("@CommentText", SessionManager.CurrentTicketComment, DbType.String));
                parameters.Add(new FilterParameter("@SiteID", int.Parse(SecureQueryString["SiteID"]), DbType.Int32));
                WebServices.CsReportServices.ExecuteNonQueryCommand("spa_cm_InsertCommentForTicketCaseManagement", parameters, out tempParameters);

                Response.Redirect("CaseModal1.aspx?" + BuildSecureQueryString("type=close&mode=redirect&TicketNumber=" +
                    SecureQueryString["TicketNumber"] + "&MerchantNumber=" + SecureQueryString["MerchantNumber"] +
                    "&SiteID=" + SecureQueryString["SiteID"]));
                break;
        }
    }

    protected void uxYes_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ClickYes);
    }
}
