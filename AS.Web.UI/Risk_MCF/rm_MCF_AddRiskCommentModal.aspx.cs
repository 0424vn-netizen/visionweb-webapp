using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI;

[PagePermission("RskRP,MSRskRP")]
public partial class rm_MCF_AddRiskCommentModal : NonReportPage
{
    
    private const int RISK_MANAGEMENT_COMMENT = 1;
    private const int RISK_NORMAL_COMMENT = 2;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsSecureQueryString)
        {
            return;
        }
        
        if (!IsPostBack)
        {
            uxManageComment.Visible = IsUserWithPermission("AddManagementComment") || IsUserWithPermission("MSAddManagementComment");
            uxMerchantName.Text =  VeraCodeSolution.DoVeraCode(GeneralFuncsLib.GetMerchantName(SecureQueryString["merchantNumber"]));
            uxMerchantNumber.Text = VeraCodeSolution.DoVeraCode(SecureQueryString["merchantNumber"]);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        int commentType = RISK_MANAGEMENT_COMMENT;
        if (!uxManageComment.Checked)
        {
            commentType = RISK_NORMAL_COMMENT;
        }

        // Remove invalid Html tags
        var htmlText = uxComment.Content;
        uxComment.Content = htmlText.StripInvalidHtml();

        FilterParameterCollection parameter = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameter.AddLoggedInUserReportingParams();
        parameter.Add(new FilterParameter("@MerchantNumber", SecureQueryString["merchantNumber"], DbType.AnsiString));
        parameter.Add(new FilterParameter("@Comment", uxComment.Content, DbType.String));
        parameter.Add(new FilterParameter("@CommentPlanText", uxComment.Text.Replace("\n", ""), DbType.String));
        parameter.Add(new FilterParameter("@CommentType", commentType, DbType.Int32));
        if (SessionManager.CurrentUser.SiteID != 0)
        {
            parameter.Add(new FilterParameter("@EntityType", SessionManager.CurrentUser.EntityType, DbType.Int32));
            parameter.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
        }
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_InsertCommentsOfMerchant", parameter, out parameterOut);

        ClientScript.RegisterStartupScript(GetType(), "reload", "setTimeout('ReloadParent()',100);", true);
    }
    public static string IsIEBrowser
    {
        get
        {
            return GeneralFuncsLib.GetIEBrowserMode();
        }
    }

   
}
