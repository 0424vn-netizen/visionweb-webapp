using AS.Web.Business.RiskReport;
using AS.Web.Business.RiskReport.Models;
using System;
using System.Web.UI;

public partial class UserControls_rm_MCF_MerchantNoteToDelete : GlobalUserControl
{
    private readonly IRiskReportNoteBussiness _riskReportNoteBussiness;
    public UserControls_rm_MCF_MerchantNoteToDelete() : this(new RiskReportNoteBussiness(WebServices.RiskServices))
    {
    }
    public UserControls_rm_MCF_MerchantNoteToDelete(IRiskReportNoteBussiness riskReportNoteBussiness)
    {
        _riskReportNoteBussiness = riskReportNoteBussiness;
    }
    protected string MerchantNumber
    {
        get
        {
            if (Page.IsSecureQueryString && !string.IsNullOrEmpty(Page.SecureQueryString["MerchantNumber"]))
                return Page.SecureQueryString["MerchantNumber"];
            else
                return string.Empty;
        }
    }
    protected string MerchantNoteID
    {
        get
        {
            if (Page.IsSecureQueryString && !string.IsNullOrEmpty(Page.SecureQueryString["MerchantNoteID"]))
                return Page.SecureQueryString["MerchantNoteID"];
            else
                return string.Empty;
        }
    }
    protected string NoteSourceID
    {
        get
        {
            if (Page.IsSecureQueryString && !string.IsNullOrEmpty(Page.SecureQueryString["NoteSourceID"]))
                return Page.SecureQueryString["NoteSourceID"];
            else
                return string.Empty;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = GetLocalResourceObject("PageResource1.Title").ToString();
        if (!IsPostBack)
        {
        }
    }

    protected void uxDelete_Click(object sender, EventArgs e)
    {  
        _riskReportNoteBussiness.DeleteRiskReportNote(new DeleteRiskReportNoteRequest
        {
            UserMode = GeneralFuncsLib.GetUserMode(),
            MerchantNoteID = MerchantNoteID,
            MerchantNumber = MerchantNumber
        }, SessionManager.CurrentUser);

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "doneSubmit", "ClosePopupModal();", true);
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "refeshMerchantNotes", "parent.RefeshMerchantNotes()", true);
    }
}