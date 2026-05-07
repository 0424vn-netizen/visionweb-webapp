using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DefaultSettingModal : ReportPage
{
    public string Module { 
        get 
        { 
            //1: Merchant Note, 2: Case History
            if (IsSecureQueryString)
                return SecureQueryString["module"].ToString();
            return string.Empty;
        } 
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            ucMerchantNoteDefaultSetting.Visible = Module == "1";
            ucCaseHistoryDefaultSetting.Visible = Module == "2";
        }
    }
}