using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common;

public partial class ReportFiltering : GlobalUserControl
{
    public event EventHandler Filtering;

    public string ExtendHierarchyMode
    {
        get
        {
            return uxReportFilter.ExtendHierarchyMode;
        }
        set
        {
            uxReportFilter.ExtendHierarchyMode = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void OnReportFilterActionUxReportFilter(object sender, AS.Web.UI.Controls.ReportFilterEventArgs e)
    {
        if (e.ActionType == AS.Web.UI.Controls.ReportFilterEventType.Submit && Filtering != null)
        {
            Filtering(sender, new EventArgs());
        }
    }
}
