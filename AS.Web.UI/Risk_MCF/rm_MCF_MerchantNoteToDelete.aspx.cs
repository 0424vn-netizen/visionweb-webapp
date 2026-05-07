using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[PagePermission(WebSiteConstants.PERMISSION_ACCESS_FUNC_RISK_DELETE_MERCHANT_NOTE + "," + WebSiteConstants.PERMISSION_ACCESS_FUNC_RISK_DELETE_MERCHANT_NOTE_MS)]
public partial class rm_MCF_MerchantNoteToDelete : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
    }
}