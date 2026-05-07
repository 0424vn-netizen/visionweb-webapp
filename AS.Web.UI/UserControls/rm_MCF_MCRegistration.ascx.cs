using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_MCRegistration : GlobalUserControl
{
    #region Properties

    public int IsMCRegistration
    {
        get
        {
            if (rdMCRegistrationNA.Checked) return (int)WebSiteEnums.MCRegistration.All;
            if (rdMCRegistrationYes.Checked) return (int)WebSiteEnums.MCRegistration.Yes;
            return (int)WebSiteEnums.MCRegistration.No;
        }
    }

    #endregion Properties

    #region Methods
    public void SetMCRegistration(int MCRegistration)
    {
        switch (MCRegistration)
        {
            case (int)WebSiteEnums.MCRegistration.No:
                rdMCRegistrationNo.Checked = true;
                break;
            case (int)WebSiteEnums.MCRegistration.Yes:
                rdMCRegistrationYes.Checked = true;
                break;
            case (int)WebSiteEnums.MCRegistration.All:
                rdMCRegistrationNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioMCRegistration()
    {
        rdMCRegistrationNA.Enabled = false;
        rdMCRegistrationYes.Enabled = false;
        rdMCRegistrationNo.Enabled = false;
    }

    #endregion Methods
}