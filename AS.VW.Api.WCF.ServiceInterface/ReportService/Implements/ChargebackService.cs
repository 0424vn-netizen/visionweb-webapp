using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Dto;
using AS.VW.Api.Validation;
using System.ServiceModel.Web;
using FluentValidation.Results;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public partial class ReportService : IChargebackService
    {
        public ReportResponse<ChargebackHierarchySummary> GetChargebacksSummary(GenericReportFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidate(ReportFilterValidator.ALL);
            if (results.IsValid)
            {
                return _ChargebackRepository.GetChargebackSummaryByEntity(req);
            }
            else
            {
                ReportResponse<ChargebackHierarchySummary> data = new ReportResponse<ChargebackHierarchySummary>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }

        public ReportResponse<ChargebackDetail> GetChargebacksDetail(MerchantSummaryFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidateForMerchantFilter(MerchantSummaryFilterValidator.ALL);
            if (results.IsValid)
            {
                return _ChargebackRepository.GetChargebackDetail(req);
            }
            else
            {
                ReportResponse<ChargebackDetail> data = new ReportResponse<ChargebackDetail>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }
    }
}
