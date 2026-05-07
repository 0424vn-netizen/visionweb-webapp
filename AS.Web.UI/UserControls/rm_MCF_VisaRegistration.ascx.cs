using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_VisaRegistration : GlobalUserControl
{
    #region Properties

    public int IsVisaRegistration
    {
        get
        {
            if (rdVisaRegistrationNA.Checked) return (int)WebSiteEnums.VisaRegistration.All;
            if (rdVisaRegistrationYes.Checked) return (int)WebSiteEnums.VisaRegistration.Yes;
            return (int)WebSiteEnums.VisaRegistration.No;
        }
    }

    #endregion Properties

    #region Methods
    public void SetVisaRegistration(int VisaRegistration)
    {
        switch (VisaRegistration)
        {
            case (int)WebSiteEnums.VisaRegistration.No:
                rdVisaRegistrationNo.Checked = true;
                break;
            case (int)WebSiteEnums.VisaRegistration.Yes:
                rdVisaRegistrationYes.Checked = true;
                break;
            case (int)WebSiteEnums.VisaRegistration.All:
                rdVisaRegistrationNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioVisaRegistration()
    {
        rdVisaRegistrationNA.Enabled = false;
        rdVisaRegistrationYes.Enabled = false;
        rdVisaRegistrationNo.Enabled = false;
    }

    #endregion Methods
}