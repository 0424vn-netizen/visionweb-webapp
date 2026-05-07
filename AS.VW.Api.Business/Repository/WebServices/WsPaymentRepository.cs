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
    public class WsPaymentRepository:ReportRepositoryBase, IPaymentRepository
    {
        public ReportResponse<DepositHierarchySummary> GetPaymentSummaryByEntity(GenericReportFilter filteringOptions)
        {
            return _apiWebService.GetPaymentSummaryByEntity(PrincipalUser, filteringOptions);
        }

        public ReportResponse<DepositSummary> GetPaymentSummaryByMerchant(MerchantSummaryFilter filteringOptions)
        {
            return _apiWebService.GetPaymentSummaryByMerchant(PrincipalUser, filteringOptions);
        }

        public ReportResponse<DepositDetail> GetPaymentDetail(MerchantSummaryFilter filteringOptions)
        {
            return _apiWebService.GetPaymentDetail(PrincipalUser, filteringOptions);
        }
    }
}
