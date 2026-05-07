using AS.VW.Api.Model.Dto;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Business.Repository
{
    public interface IMerchantInformationRepository : IAuthenticatedRepository
    {
        ReportResponse<MerchantSummary> GetMerchantList(HierarchyPagingFilter filter);

        ReportResponse<MerchantComment> GetMerchantComments(MerchantInformationPagingFilter filter);

    }
}
