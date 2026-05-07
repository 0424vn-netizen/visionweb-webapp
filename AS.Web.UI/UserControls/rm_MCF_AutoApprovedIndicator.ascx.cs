using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_AutoApprovedIndicator : GlobalUserControl
{
    #region Properties

    public int IsAutoApprovedIndicator
    {
        get
        {
            if (rdAutoApprovedIndicatorNA.Checked) return (int)WebSiteEnums.AutoApprovedIndicator.All;
            if (rdAutoApprovedIndicatorYes.Checked) return (int)WebSiteEnums.AutoApprovedIndicator.Yes;
            return (int)WebSiteEnums.AutoApprovedIndicator.No;
        }
    }

    #endregion Properties

    #region Methods
    public void SetAutoApprovedIndicator(int AutoApprovedIndicator)
    {
        switch (AutoApprovedIndicator)
        {
            case (int)WebSiteEnums.AutoApprovedIndicator.No:
                rdAutoApprovedIndicatorNo.Checked = true;
                break;
            case (int)WebSiteEnums.AutoApprovedIndicator.Yes:
                rdAutoApprovedIndicatorYes.Checked = true;
                break;
            case (int)WebSiteEnums.AutoApprovedIndicator.All:
                rdAutoApprovedIndicatorNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioAutoApprovedIndicator()
    {
        rdAutoApprovedIndicatorNA.Enabled = false;
        rdAutoApprovedIndicatorYes.Enabled = false;
        rdAutoApprovedIndicatorNo.Enabled = false;
    }

    #endregion Methods
}