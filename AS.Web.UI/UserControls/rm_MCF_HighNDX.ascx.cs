using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_HighNDX : GlobalUserControl
{
    #region Properties

    public int IsHighNDX
    {
        get
        {
            if (rdHighNDXNA.Checked) return (int)WebSiteEnums.HighNDX.All;
            if (rdHighNDXYes.Checked) return (int)WebSiteEnums.HighNDX.Yes;
            return (int)WebSiteEnums.HighNDX.No;
        }
    }

    #endregion Properties

    #region Methods
    public void SetHighNDX(int HighNDX)
    {
        switch (HighNDX)
        {
            case (int)WebSiteEnums.HighNDX.No:
                rdHighNDXNo.Checked = true;
                break;
            case (int)WebSiteEnums.HighNDX.Yes:
                rdHighNDXYes.Checked = true;
                break;
            case (int)WebSiteEnums.HighNDX.All:
                rdHighNDXNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioHighNDX()
    {
        rdHighNDXNA.Enabled = false;
        rdHighNDXYes.Enabled = false;
        rdHighNDXNo.Enabled = false;
    }

    #endregion Methods
}