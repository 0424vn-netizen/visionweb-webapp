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
    public class WsMerchantInfomationRepository: ReportRepositoryBase, IMerchantInformationRepository
    {
        #region General
        public ReportResponse<MerchantSummary> GetMerchantList(HierarchyPagingFilter filter)
        {
            return _apiWebService.GetMerchantList(PrincipalUser, filter);
        }

        public ReportResponse<MerchantComment> GetMerchantComments(MerchantInformationPagingFilter filter)
        {
            return _apiWebService.GetMerchantComments(PrincipalUser, filter);
        }

        #endregion
                
    }
}
