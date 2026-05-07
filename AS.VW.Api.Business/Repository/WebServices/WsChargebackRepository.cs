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
    public class WsChargebackRepository : ReportRepositoryBase, IChargebackRepository
    {
        public ReportResponse<ChargebackHierarchySummary> GetChargebackSummaryByEntity(GenericReportFilter filteringOptions)
        {
            return _apiWebService.GetChargebackSummaryByEntity(PrincipalUser, filteringOptions);
        }

        public ReportResponse<ChargebackDetail> GetChargebackDetail(MerchantSummaryFilter filteringOptions)
        {
            return _apiWebService.GetChargebackDetail(PrincipalUser, filteringOptions);
        }
    }
}
