using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Web.UI.Controls;

public partial class UserControls_ActivationReport_Filtering : GlobalUserControl
{
    public string HierarchyMode
    {
        get
        {
            return uxReportFilter.CurrentValue.HierarchyMode;
        }
    }
    public string HierarchyValue
    {
        get
        {
            return uxReportFilter.CurrentValue.Value;
        }
    }
    public event EventHandler DoSearch;
    public delegate void DoSearchHandler(object sender);
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void uxReportFilter_ReportFilterAction(object sender, AS.Web.UI.Controls.ReportFilterEventArgs e)
    {
        if (e.ActionType == AS.Web.UI.Controls.ReportFilterEventType.Submit)
        {
            DoSearch(this, EventArgs.Empty);
        }
    }
}