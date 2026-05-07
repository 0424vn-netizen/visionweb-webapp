using AS.VW.Api.Model.Dto;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Business.Repository.WebService
{
    public class WsAuthorizationRepository : ReportRepositoryBase, IAuthorizationRepository
    {

        public ReportResponse<AuthorizationLogSummary> GetAuthorizationLogSummary(GenericReportFilter filteringOptions)
        {
            return _apiWebService.GetAuthorizationLogSummary(PrincipalUser, filteringOptions);
        }

        public ReportResponse<AuthorizationDetail> GetAuthorizationLogDetail(MerchantSummaryFilter filteringOptions)
        {
            return _apiWebService.GetAuthorizationLogDetail(PrincipalUser, filteringOptions);
        }
              
    }
}
