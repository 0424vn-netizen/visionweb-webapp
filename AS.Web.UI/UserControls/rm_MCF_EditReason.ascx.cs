using AS.Common;
using AS.Common.DBManager;
using Resources;
using System;
using System.Data;

public partial class UserControls_rm_MCF_EditReason : System.Web.UI.UserControl
{
    public string ReasonID
    {
        set { this.uxReasonID.Value = value; }
    }
    public string ReasonText
    {
        set { this.uxEscalationText.Text = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void uxEscalationUpdate_Click(object sender, EventArgs e)
    {
        int escalationStatusID = Int32.Parse(uxReasonID.Value.ToString().Trim());
        string status = VeraCodeSolution.DoVeraCode(uxEscalationText.Text.ToString().Trim());

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ReasonID", escalationStatusID, DbType.Int32));
        parameters.Add(new FilterParameter("@Description", status, DbType.String));
        parameters.Add(new FilterParameter("@IsActive", -1, DbType.Int32));
        parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));

        FilterParameterCollection parameterOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_UpdateEscalationReason", parameters, out parameterOut);
        int result = Convert.ToInt32(parameterOut[0].ParameterValue);

        if (result == 0)
        {
            uxEscalationTextErrMsg.Message = AS.Common.VeraCodeSolution.DoVeraCode(MessageManager.Generic_ProcessingFailed);
            uxEscalationTextErrMsg.ShowOnLoad = true;
            uxEscalationTextLabel.CssClass = "control-label label-error";
            return;
        }
        else if (result == 2)
        {            
            uxEscalationTextErrMsg.Message = AS.Common.VeraCodeSolution.DoVeraCode(MessageManager.Field_RequireAndUnique);
            uxEscalationTextErrMsg.ShowOnLoad = true;
            uxEscalationTextLabel.CssClass = "control-label label-error";
            return;
        }
        Response.Redirect(Request.RawUrl);
    }
    public delegate void CancelEvent(object sender, EventArgs e);
    public event CancelEvent cancelEvt;

    protected void uxEscalationCancel_Click(object sender, EventArgs e)
    {
        cancelEvt(sender, e);
        this.Visible = false;
    }
}