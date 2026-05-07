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

public partial class UserControls_Risk_Assignment_CancelButton : GlobalUserControl
{
    #region Enums
    enum DataBindAction { }
    enum PostBackAction
    {
        Cancel
    }
    #endregion

    #region properties

    private string _PrimaryID;

    public string PrimaryID
    {
        get { return _PrimaryID; }
        set { _PrimaryID = value; }
    }

    public WebSiteEnums.ParamFilterMode Mode;

    public int ReviewMode = 0;
    public int IsDetectionQueue = 0;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Cancel:
                if (ReviewMode != 1 && Mode != WebSiteEnums.ParamFilterMode.Adhoc)
                    EliminateTemporaryAssignment();
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseParentModal", "parent.HidePopupModal();", true);
                (Page.Master as BaseMasterPage).AjaxAddResponseScript("parent.HidePopupModal();");
                break;
        }
    }

    protected void btnCancelAssignment_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Cancel);
    }

    private void EliminateTemporaryAssignment()
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", PrimaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@FilterMode", (int)Mode, DbType.Int32));
        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_EliminateTemporaryAssignment", paramsIn, out paramsOut);
    }
}
