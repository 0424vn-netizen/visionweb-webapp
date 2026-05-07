using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Dto;

namespace AS.VW.Api.Business.Repository.WebService
{
    public class WsReturnRepository : ReportRepositoryBase, IReturnRepository
    {
        public ReportResponse<ReturnHierarchySummary> GetReturnSummaryByEntity(GenericReportFilter filteringOptions)
        {
            return _apiWebService.GetReturnSummaryByEntity(PrincipalUser, filteringOptions);
        }

        public ReportResponse<ReturnSummary> GetReturnSummaryByMerchant(MerchantSummaryFilter filteringOptions)
        {
            return _apiWebService.GetReturnSummaryByMerchant(PrincipalUser, filteringOptions);
        }
    }
}
