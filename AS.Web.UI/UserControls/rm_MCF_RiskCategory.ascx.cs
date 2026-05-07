using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_RiskCategory : GlobalUserControl
{
    #region Properties

    public WebSiteEnums.FeatureMode FeatureMode { get; set; }
    public bool RiskCategoryLow
    {
        get
        {
            return rdRiskCategoryLow.Checked;
        }
        set
        {
            rdRiskCategoryLow.Checked = value;
        }
    }

    public bool RiskCategoryModerate
    {
        get
        {
            return rdRiskCategoryModerate.Checked;
        }
        set
        {
            rdRiskCategoryModerate.Checked = value;
        }
    }

    public bool RiskCategoryHigh
    {
        get
        {
            return rdRiskCategoryHigh.Checked;
        }
        set
        {
            rdRiskCategoryHigh.Checked = value;
        }
    }

    #endregion Properties

    #region Methods
    public void RejectOnClickEventForRiskLevel()
    {
        rdRiskCategoryLow.Enabled = false;
        rdRiskCategoryModerate.Enabled = false;
        rdRiskCategoryHigh.Enabled = false;
    }

    #endregion Methods
}