using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using AS.VW.Api.Model;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Filter;
using System.Text.RegularExpressions;

namespace AS.VW.Api.Validation
{

    public class ReportFilterValidator<T> : CompositeValidator<T> where T: GenericReportFilterNoViewLevel
    {
        public const string ALL = DateFilterValidator.DATE_RULES + "," + HierarchyFilterValidator.ALL_RULES + "," + PagingFilterValidator.PAGING_RULES;
        public const string HIERARCHYFILTER_ALLOW_EMPTY= DateFilterValidator.DATE_RULES + "," + HierarchyFilterValidator.FORMAT_RULES;

        public ReportFilterValidator()
        {
            RegisterValidator(new HierarchyFilterValidator<GenericReportFilterNoViewLevel>());
            RegisterValidator(new DateFilterValidator());
            RegisterValidator(new PagingFilterValidator());
        }
    }

    public class ReportFilterNoViewLevelValidator : ReportFilterValidator<GenericReportFilterNoViewLevel>
    { 
    
    }

    public class ReportFilterValidator : ReportFilterValidator<GenericReportFilter>
    {

    }
}
