using System;
using AS.Web.Business.Shared.Enums;
public partial class SensitiveDataDetectedModal : ReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        EnumAction enumAction;
        Enum.TryParse(base.Request["Action"], out enumAction);       
        btnSubmit.OnClientClick = "submitNote(false, '" + enumAction + "'); return false;";
        btnDisregard.OnClientClick = "submitNote(true, '" + enumAction + "'); return false;";
    }
}