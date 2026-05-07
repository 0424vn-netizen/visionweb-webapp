using AS.Web.Business.RiskReport;
using AS.Web.Business.RiskReport.Models;
using System;
using System.Data;
using System.Web.UI;


public partial class UserControls_rm_MCF_MerchantNoteToEdit : GlobalUserControl
{
    private readonly IRiskReportNoteBussiness _riskReportNoteBussiness;
    public UserControls_rm_MCF_MerchantNoteToEdit() : this(new RiskReportNoteBussiness(WebServices.RiskServices))
    {
    }
    public UserControls_rm_MCF_MerchantNoteToEdit(IRiskReportNoteBussiness riskReportNoteBussiness)
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
            GetMerchantNoteDetails();
        }
    }

    protected void UxSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsIntruderDetected) return;
        _riskReportNoteBussiness.UpdateRiskReportNote(
                                       new UpdateRiskReportNoteRequest
                                       {
                                           UserMode = GeneralFuncsLib.GetUserMode(),
                                           MerchantNoteID = MerchantNoteID,
                                           MerchantNumber = MerchantNumber,
                                           Comment = uxComment.Content,
                                           CommentPlainText = uxComment.Text.Replace("\n", ""),
                                           HdCardDetected = hdCardDetected.Value
                                       }, SessionManager.CurrentUser);
        uxComment.Content = string.Empty;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "doneSubmit", "ClosePopupModal();", true);
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "refeshMerchantNotes", "parent.RefeshMerchantNotes()", true);
    }

    private void GetMerchantNoteDetails()
    {
        DataTable data = _riskReportNoteBussiness.GetDetailRiskReportNote(
            new GetDetailRiskReportNoteRequest
            {
                UserMode = GeneralFuncsLib.GetUserMode(),
                MerchantNoteID = MerchantNoteID,
                MerchantNumber = MerchantNumber
            }, SessionManager.CurrentUser);
        uxComment.Content = data.Rows[0]["Comment"].ToString();
    }
}
