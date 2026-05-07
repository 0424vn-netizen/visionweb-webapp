using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Dto;

namespace AS.VW.Api.Business.Repository
{
    public interface IBatchRepository : IAuthenticatedRepository
    {
        ReportResponse<BatchHierarchySummary> GetBatchSummaryByEntity(GenericReportFilter filteringOptions);

        ReportResponse<BatchSummary> GetBatchSummaryByMerchant(MerchantSummaryFilter filteringOptions);

        ReportResponse<TransactionDetailBatch> GetBatchDetail(BatchDetailFilter filteringOptions);

        ReportResponse<CardSummary> GetCardSummary(CardSummaryFilter filteringOptions);
    }
}
