using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Dto;

namespace AS.VW.Api.Business.Repository.WebService
{
    public class WsRetrievalRepository : ReportRepositoryBase, IRetrievalRepository
    {
        public ReportResponse<RetrievalHierarchySummary> GetRetrievalSummaryByEntity(GenericReportFilter filteringOptions)
        {
            return _apiWebService.GetRetrievalSummaryByEntity(PrincipalUser, filteringOptions);
        }

        public ReportResponse<RetrievalDetail> GetRetrievalDetail(MerchantSummaryFilter filteringOptions)
        {
            return _apiWebService.GetRetrievalDetail(PrincipalUser, filteringOptions);
        }
    }
}
