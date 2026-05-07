using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_AcqAgentSharedLiability : GlobalUserControl
{
    #region Properties

    public int IsAcqAgentSharedLiability
    {
        get
        {
            if (rdAcqAgentSharedLiabilityNA.Checked) return (int)WebSiteEnums.AcqAgentSharedLiability.All;
            if (rdAcqAgentSharedLiabilityYes.Checked) return (int)WebSiteEnums.AcqAgentSharedLiability.Yes;
            return (int)WebSiteEnums.AcqAgentSharedLiability.No;
        }
    }

    #endregion Properties

    #region Methods
    public void SetAcqAgentSharedLiability(int AcqAgentSharedLiability)
    {
        switch (AcqAgentSharedLiability)
        {
            case (int)WebSiteEnums.AcqAgentSharedLiability.No:
                rdAcqAgentSharedLiabilityNo.Checked = true;
                break;
            case (int)WebSiteEnums.AcqAgentSharedLiability.Yes:
                rdAcqAgentSharedLiabilityYes.Checked = true;
                break;
            case (int)WebSiteEnums.AcqAgentSharedLiability.All:
                rdAcqAgentSharedLiabilityNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioAcqAgentSharedLiability()
    {
        rdAcqAgentSharedLiabilityNA.Enabled = false;
        rdAcqAgentSharedLiabilityYes.Enabled = false;
        rdAcqAgentSharedLiabilityNo.Enabled = false;
    }

    #endregion Methods
}