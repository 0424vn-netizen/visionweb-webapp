using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_DiscoverRegistration : GlobalUserControl
{
    #region Properties

    public int IsDiscoverRegistration
    {
        get
        {
            if (rdDiscoverRegistrationNA.Checked) return (int)WebSiteEnums.DiscoverRegistration.All;
            if (rdDiscoverRegistrationYes.Checked) return (int)WebSiteEnums.DiscoverRegistration.Yes;
            return (int)WebSiteEnums.DiscoverRegistration.No;
        }
    }

    #endregion Properties

    #region Methods
    public void SetDiscoverRegistration(int DiscoverRegistration)
    {
        switch (DiscoverRegistration)
        {
            case (int)WebSiteEnums.DiscoverRegistration.No:
                rdDiscoverRegistrationNo.Checked = true;
                break;
            case (int)WebSiteEnums.DiscoverRegistration.Yes:
                rdDiscoverRegistrationYes.Checked = true;
                break;
            case (int)WebSiteEnums.DiscoverRegistration.All:
                rdDiscoverRegistrationNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioDiscoverRegistration()
    {
        rdDiscoverRegistrationNA.Enabled = false;
        rdDiscoverRegistrationYes.Enabled = false;
        rdDiscoverRegistrationNo.Enabled = false;
    }

    #endregion Methods
}