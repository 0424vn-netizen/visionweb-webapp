using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AS.Web.UI.Controls;

/// <summary>
/// Summary description for MgmtReportFilter
/// </summary>
/// 
[Serializable]
public class MgmtReportFilter
{
    public MgmtReportFilter()
    {
        DateType = DateOptionMode.Daily;
        FromDate = DateTime.Today;
        ToDate = DateTime.Today;
    }

    public MgmtReportFilter(DateOptionMode dateType, DateTime fromDate, DateTime toDate)
    {
        DateType = dateType;
        FromDate = fromDate;
        ToDate = toDate;
    }

    public DateOptionMode DateType { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public bool KeepSession { get; set; }

    public string AssignmentFilterValue { get; set; }
    public string ParameterFilterValue { get; set; }
    public bool IsAgentSearch { get; set; }
    public string AgentFilterValue { get; set; }
    public bool IsGroupSearch { get; set; }
    public string GroupFilterValue { get; set; }
    public string MerchantNumber { get; set; }
    public string MerchantName { get; set; }
    public string DispositionFilterValue { get; set; }
    public string UserNameFilterValue { get; set; }
}
