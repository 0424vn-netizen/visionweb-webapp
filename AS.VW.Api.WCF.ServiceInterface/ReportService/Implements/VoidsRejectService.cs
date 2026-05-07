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
    public partial class ReportService : IVoidsRejectService
    {
        public ReportResponse<VoidRejectHierarchySummary> GetVoidsRejectsSummary(GenericReportFilter req)
        {
            req.SetDefaultValuesAndValidate(ReportFilterValidator.ALL);
            return _VoidsRejectRepository.GetVoidsRejectSummaryByEntity(req);
        }

        public ReportResponse<VoidRejectDetail> GetVoidsRejectsDetail(MerchantSummaryFilter req)
        {
            req.SetDefaultValuesAndValidateForMerchantFilter(MerchantSummaryFilterValidator.ALL);
            return _VoidsRejectRepository.GetVoidsRejectDetail(req);
        }
    }
}
