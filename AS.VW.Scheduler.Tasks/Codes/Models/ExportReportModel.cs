using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Tasks.Codes.Models
{
    public class ExportReportConfigRequest
    {
        public int AsClientId { get; set; }
        public string CategoryCode { get; set; }
        public string SubCategoryCode { get; set; } 
    }

    public class ExportReportConfigResponse
    {
       public ExportReportFilterModel ExportReportFilter { get; set; } = new ExportReportFilterModel(); 
    }

    public class ExportReportFilterModel
    {
        public List<ParamInput> ParamInputs { get; set; } = new List<ParamInput>();
        public ValueItem FilterItem { get; set; }
        public ValueItem SortItem { get; set; }
        public ValueItem CustomView { get; set; }
        public ValueItem ReportHeader { get; set; }

    }

    public class ParamInput
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    
    public class ValueItem
    {
        public string Value { get; set; }
    }

}
