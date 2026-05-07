using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_RiskLevel : GlobalUserControl
{
    #region Properties

    public bool RiskLevelLowLv1
    {
        get
        {
            return rdRiskLevelLowLv1.Checked;
        }
        set
        {
            rdRiskLevelLowLv1.Checked = value;
        }
    }

    public bool RiskLevelLowLv2
    {
        get
        {
            return rdRiskLevelLowLv2.Checked;
        }
        set
        {
            rdRiskLevelLowLv2.Checked = value;
        }
    }

    public bool RiskLevelHighLv1
    {
        get
        {
            return rdRiskLevelHighLv1.Checked;
        }
        set
        {
            rdRiskLevelHighLv1.Checked = value;
        }
    }

    public bool RiskLevelHighLv2
    {
        get
        {
            return rdRiskLevelHighLv2.Checked;
        }
        set
        {
            rdRiskLevelHighLv2.Checked = value;
        }
    }

    #endregion Properties

    #region Methods
    public void RejectOnClickEventForRiskLevel()
    {
        rdRiskLevelLowLv1.Enabled = false;
        rdRiskLevelLowLv2.Enabled = false;
        rdRiskLevelHighLv1.Enabled = false;
        rdRiskLevelHighLv2.Enabled = false;
    }

    #endregion Methods
}