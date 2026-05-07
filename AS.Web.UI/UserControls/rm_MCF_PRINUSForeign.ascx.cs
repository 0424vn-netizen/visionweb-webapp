using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_PRINUSForeign : GlobalUserControl
{
    #region Properties

    public int IsPRINUSForeign
    {
        get
        {
            if (rdPRINUSForeignNA.Checked) return (int)WebSiteEnums.PRINUSForeign.All;
            if (rdPRINUSForeignYes.Checked) return (int)WebSiteEnums.PRINUSForeign.Yes;
            return (int)WebSiteEnums.PRINUSForeign.No;
        }
    }

    #endregion Properties

    #region Methods
    public void SetPRINUSForeign(int PRINUSForeign)
    {
        switch (PRINUSForeign)
        {
            case (int)WebSiteEnums.PRINUSForeign.No:
                rdPRINUSForeignNo.Checked = true;
                break;
            case (int)WebSiteEnums.PRINUSForeign.Yes:
                rdPRINUSForeignYes.Checked = true;
                break;
            case (int)WebSiteEnums.PRINUSForeign.All:
                rdPRINUSForeignNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioPRINUSForeign()
    {
        rdPRINUSForeignNA.Enabled = false;
        rdPRINUSForeignYes.Enabled = false;
        rdPRINUSForeignNo.Enabled = false;
    }

    #endregion Methods
}