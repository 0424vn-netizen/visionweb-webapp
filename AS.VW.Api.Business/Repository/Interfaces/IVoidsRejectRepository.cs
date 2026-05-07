using AS.VW.Api.Model.Dto;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;

namespace AS.VW.Api.Business.Repository
{
    public interface IVoidsRejectRepository : IAuthenticatedRepository
    {
        ReportResponse<VoidRejectHierarchySummary> GetVoidsRejectSummaryByEntity(GenericReportFilter filteringOptions);

        ReportResponse<VoidRejectDetail> GetVoidsRejectDetail(MerchantSummaryFilter filteringOptions);
    }
}
