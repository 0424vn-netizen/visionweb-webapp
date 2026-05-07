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
    public class WsBatchRepository : ReportRepositoryBase, IBatchRepository
    {
        public ReportResponse<BatchHierarchySummary> GetBatchSummaryByEntity(GenericReportFilter filteringOptions)
        {            
            return _apiWebService.GetBatchSummaryByEntity(PrincipalUser, filteringOptions); 
        }

        public ReportResponse<BatchSummary> GetBatchSummaryByMerchant(MerchantSummaryFilter filteringOptions)
        {
            return _apiWebService.GetBatchSummaryByMerchant(PrincipalUser, filteringOptions);
        }

        public ReportResponse<TransactionDetailBatch> GetBatchDetail(BatchDetailFilter filteringOptions)
        {
            return _apiWebService.GetBatchDetail(PrincipalUser, filteringOptions);
        }

        public ReportResponse<CardSummary> GetCardSummary(CardSummaryFilter filteringOptions)
        {
            return _apiWebService.GetCardSummary(PrincipalUser, filteringOptions);
        }
    }
}
