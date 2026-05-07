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
using AS.Controls.Pages;

[PagePermission("RskManAss,MSRskManAss")]
public partial class rm_MCF_AssignmentUsers_Modal : NonReportPage
{
    #region Enums
    enum DataBindAction { }
    enum PostBackAction
    {
        Save
    }
    #endregion

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.Save:
                int errorCode = uxAssignmentUser.Save();
                ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", "parent.ClosePopupModal(1);", true);
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        string PrimaryID = "0";
        if (base.SecureQueryString != null)
        {
            PrimaryID = base.SecureQueryString["AssignmentID"];
        }
        uxAssignmentUser.Mode = WebSiteEnums.ParamFilterMode.Assignment;
        uxAssignmentUser.PrimaryID = PrimaryID;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Save, sender);
    }
}
