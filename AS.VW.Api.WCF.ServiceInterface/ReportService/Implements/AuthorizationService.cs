using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Dto;
using AS.VW.Api.Validation;
using FluentValidation.Results;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public partial class ReportService : IAuthorizationService
    {       
        public ReportResponse<AuthorizationLogSummary> GetAuthorizationLogSummary(GenericReportFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidate(ReportFilterValidator.ALL);
            if (results.IsValid)
            {
                return _AuthorizationRepository.GetAuthorizationLogSummary(req);
            }
            else
            {
                ReportResponse<AuthorizationLogSummary> data = new ReportResponse<AuthorizationLogSummary>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }

        public ReportResponse<AuthorizationDetail> GetAuthorizationLogDetail(MerchantSummaryFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidateForMerchantFilter(MerchantSummaryFilterValidator.ALL);
            if (results.IsValid)
            {
                return _AuthorizationRepository.GetAuthorizationLogDetail(req);
            }
            else
            {
                ReportResponse<AuthorizationDetail> data = new ReportResponse<AuthorizationDetail>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }

    }
}
