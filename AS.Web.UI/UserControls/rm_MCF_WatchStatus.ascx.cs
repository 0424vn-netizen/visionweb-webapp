using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_WatchStatus : GlobalUserControl
{
    #region Properties

    public int IsMerchantsOnWatch
    {
        get
        {
            return rdWatchStatusNA.Checked ? (int)WebSiteEnums.WatchStatus.NA
                : rdWatchStatusOn.Checked ? (int)WebSiteEnums.WatchStatus.On
                                          : (int)WebSiteEnums.WatchStatus.Off;
        }
    }

    #endregion Properties

    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void SetWatchStatus(int watchStatus)
    {
        switch (watchStatus)
        {
            case (int)WebSiteEnums.WatchStatus.Off:
                rdWatchStatusOff.Checked = true;
                break;
            case (int)WebSiteEnums.WatchStatus.On:
                rdWatchStatusOn.Checked = true;
                break;
            case (int)WebSiteEnums.WatchStatus.NA:
                rdWatchStatusNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioWatchStatus()
    {
        rdWatchStatusNA.Enabled = false;
        rdWatchStatusOn.Enabled = false;
        rdWatchStatusOff.Enabled = false;
    }

    #endregion Methods
}