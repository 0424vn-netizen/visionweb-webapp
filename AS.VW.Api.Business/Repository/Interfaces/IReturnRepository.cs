using AS.VW.Api.Model.Dto;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;

namespace AS.VW.Api.Business.Repository
{
    public interface IReturnRepository : IAuthenticatedRepository
    {
        ReportResponse<ReturnHierarchySummary> GetReturnSummaryByEntity(GenericReportFilter filteringOptions);

        ReportResponse<ReturnSummary> GetReturnSummaryByMerchant(MerchantSummaryFilter filteringOptions);
    }
}
