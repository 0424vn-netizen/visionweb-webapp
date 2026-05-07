using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_PersonalGuarantee : GlobalUserControl
{
    #region Properties

    public int IsPersonalGuarantee
    {
        get
        {
            if (rdPersonalGuaranteeNA.Checked) return (int)WebSiteEnums.PersonalGuarantee.All;
            if (rdPersonalGuaranteeYes.Checked) return (int)WebSiteEnums.PersonalGuarantee.Yes;
            return (int)WebSiteEnums.PersonalGuarantee.No;
        }
    }

    #endregion Properties

    #region Methods
    public void SetPersonalGuarantee(int PersonalGuarantee)
    {
        switch (PersonalGuarantee)
        {
            case (int)WebSiteEnums.PersonalGuarantee.No:
                rdPersonalGuaranteeNo.Checked = true;
                break;
            case (int)WebSiteEnums.PersonalGuarantee.Yes:
                rdPersonalGuaranteeYes.Checked = true;
                break;
            case (int)WebSiteEnums.PersonalGuarantee.All:
                rdPersonalGuaranteeNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioPersonalGuarantee()
    {
        rdPersonalGuaranteeNA.Enabled = false;
        rdPersonalGuaranteeYes.Enabled = false;
        rdPersonalGuaranteeNo.Enabled = false;
    }

    #endregion Methods
}