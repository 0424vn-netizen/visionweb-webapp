using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Dto;
using AS.VW.Api.Validation;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public  partial class ReportService : IMerchantInformationService
    {
        #region General
        public ReportResponse<MerchantSummary> GetMerchantList(HierarchyPagingFilter req)
        {
            req.SetDefaultValueForPagingFilter();
            req.SetDefaultValueForHierarchyFilter();
            req.ValidateFilter<HierarchyPagingFilterValidator, HierarchyPagingFilter>(HierarchyFilterValidator.ALL_RULES);
            return _MerchantInformationRepository.GetMerchantList(req);
        }

        public ReportResponse<MerchantComment> GetMerchantComments(MerchantInformationPagingFilter req)
        {
            req.SetDefaultValuesAndValidateForMerchantInformationPagingFilter(MerchantNumberFilterValidator.ALL_RULES);
            return _MerchantInformationRepository.GetMerchantComments(req);
        }

        #endregion
               
    }
}
