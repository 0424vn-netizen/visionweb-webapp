using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_CBproducing : GlobalUserControl
{
    #region Properties

    public int IsCBproducing
    {
        get
        {
            if (rdCBproducingNA.Checked) return (int)WebSiteEnums.CBproducing.All;
            if (rdCBproducingYes.Checked) return (int)WebSiteEnums.CBproducing.Yes;
            return (int)WebSiteEnums.CBproducing.No;
        }
    }

    #endregion Properties

    #region Methods
    public void SetCBproducing(int CBproducing)
    {
        switch (CBproducing)
        {
            case (int)WebSiteEnums.CBproducing.No:
                rdCBproducingNo.Checked = true;
                break;
            case (int)WebSiteEnums.CBproducing.Yes:
                rdCBproducingYes.Checked = true;
                break;
            case (int)WebSiteEnums.CBproducing.All:
                rdCBproducingNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioCBproducing()
    {
        rdCBproducingNA.Enabled = false;
        rdCBproducingYes.Enabled = false;
        rdCBproducingNo.Enabled = false;
    }

    #endregion Methods
}