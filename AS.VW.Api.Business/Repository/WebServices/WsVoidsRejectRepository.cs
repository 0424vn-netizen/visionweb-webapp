using AS.VW.Api.Model.Dto;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;

namespace AS.VW.Api.Business.Repository.WebService
{
    public class WsVoidsRejectRepository : ReportRepositoryBase, IVoidsRejectRepository
    {
        public ReportResponse<VoidRejectHierarchySummary> GetVoidsRejectSummaryByEntity(GenericReportFilter filteringOptions)
        {
            return _apiWebService.GetVoidRejectSummaryByEntity(PrincipalUser, filteringOptions);
        }

        public ReportResponse<VoidRejectDetail> GetVoidsRejectDetail(MerchantSummaryFilter filteringOptions)
        {
            return _apiWebService.GetVoidsRejectDetail(PrincipalUser, filteringOptions);
        }
    }
}
