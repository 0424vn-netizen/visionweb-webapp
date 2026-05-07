using AS.Common;
using AS.Common.DBManager;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_EditProfile : System.Web.UI.UserControl
{
    public string ProfileID
    {
        set { this.uxProfileID.Value = value; }
    }
    public string ProfileName
    {
        set { this.uxProfileText.Text = value; }
    }
    public string Description
    {
        set { this.uxDescription.Text = value; }
    } 
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void uxUpdate_Click(object sender, EventArgs e)
    {
        int groupID = Int32.Parse(uxProfileID.Value.ToString().Trim());
        string groupName = VeraCodeSolution.DoVeraCode(uxProfileText.Text.ToString().Trim());
        string description = VeraCodeSolution.DoVeraCode(uxDescription.Text.ToString().Trim());

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@RecordID", groupID, DbType.Int32));
        parameters.Add(new FilterParameter("@ProfileName", groupName, DbType.String));
        parameters.Add(new FilterParameter("@Description", description, DbType.String));
        parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));

        FilterParameterCollection parameterOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_UpdateProfile", parameters, out parameterOut);

        int result = Convert.ToInt32(parameterOut[0].ParameterValue);
        if (result == 0)
        {
            uxProfileTextErrMsg.Message = AS.Common.VeraCodeSolution.DoVeraCode(MessageManager.Generic_ProcessingFailed);
            uxProfileTextErrMsg.ShowOnLoad = true;
            uxProfileTextLabel.CssClass = "control-label label-error";
            return;
        }
        else if (result == 2)
        {
            uxProfileTextErrMsg.Message = AS.Common.VeraCodeSolution.DoVeraCode(MessageManager.Field_RequireAndUnique);
            uxProfileTextErrMsg.ShowOnLoad = true;
            uxProfileTextLabel.CssClass = "control-label label-error";
            return;
        }
        Response.Redirect(Request.RawUrl);
    }

    public delegate void CancelEvent(object sender, EventArgs e);
    public event CancelEvent cancelEvt;

    protected void uxCancel_Click(object sender, EventArgs e)
    {
        cancelEvt(sender, e);
        this.Visible = false;
    }
}