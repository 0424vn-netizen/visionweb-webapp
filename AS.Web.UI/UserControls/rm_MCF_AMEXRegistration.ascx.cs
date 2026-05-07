using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_AMEXRegistration : GlobalUserControl
{
    #region Properties

    public int IsAMEXRegistration
    {
        get
        {
            if (rdAMEXRegistrationNA.Checked) return (int)WebSiteEnums.AMEXRegistration.All;
            if (rdAMEXRegistrationYes.Checked) return (int)WebSiteEnums.AMEXRegistration.Yes;
            return (int)WebSiteEnums.AMEXRegistration.No;
        }
    }

    #endregion Properties

    #region Methods
    public void SetAMEXRegistration(int AMEXRegistration)
    {
        switch (AMEXRegistration)
        {
            case (int)WebSiteEnums.AMEXRegistration.No:
                rdAMEXRegistrationNo.Checked = true;
                break;
            case (int)WebSiteEnums.AMEXRegistration.Yes:
                rdAMEXRegistrationYes.Checked = true;
                break;
            case (int)WebSiteEnums.AMEXRegistration.All:
                rdAMEXRegistrationNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioAMEXRegistration()
    {
        rdAMEXRegistrationNA.Enabled = false;
        rdAMEXRegistrationYes.Enabled = false;
        rdAMEXRegistrationNo.Enabled = false;
    }

    #endregion Methods
}