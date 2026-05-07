using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_ComplianceRegulatoryReputational : GlobalUserControl
{
    #region Properties

    public int IsComplianceRegulatoryReputational
    {
        get
        {
            if (rdComplianceRegulatoryReputationalNA.Checked) return (int)WebSiteEnums.ComplianceRegulatoryReputational.All;
            if (rdComplianceRegulatoryReputationalYes.Checked) return (int)WebSiteEnums.ComplianceRegulatoryReputational.Yes;
            return (int)WebSiteEnums.ComplianceRegulatoryReputational.No;
        }
    }

    #endregion Properties

    #region Methods
    public void SetComplianceRegulatoryReputational(int ComplianceRegulatoryReputational)
    {
        switch (ComplianceRegulatoryReputational)
        {
            case (int)WebSiteEnums.ComplianceRegulatoryReputational.No:
                rdComplianceRegulatoryReputationalNo.Checked = true;
                break;
            case (int)WebSiteEnums.ComplianceRegulatoryReputational.Yes:
                rdComplianceRegulatoryReputationalYes.Checked = true;
                break;
            case (int)WebSiteEnums.ComplianceRegulatoryReputational.All:
                rdComplianceRegulatoryReputationalNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioComplianceRegulatoryReputational()
    {
        rdComplianceRegulatoryReputationalNA.Enabled = false;
        rdComplianceRegulatoryReputationalYes.Enabled = false;
        rdComplianceRegulatoryReputationalNo.Enabled = false;
    }

    #endregion Methods
}