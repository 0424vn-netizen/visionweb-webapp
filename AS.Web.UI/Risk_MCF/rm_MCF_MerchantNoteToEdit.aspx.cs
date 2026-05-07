using AS.Controls.Pages;
using System;

[PagePermission(WebSiteConstants.PERMISSION_ACCESS_FUNC_RISK_EDIT_MERCHANT_NOTE + "," + WebSiteConstants.PERMISSION_ACCESS_FUNC_RISK_EDIT_MERCHANT_NOTE_MS)]
public partial class rm_MCF_MerchantNoteToEdit : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
    }
}