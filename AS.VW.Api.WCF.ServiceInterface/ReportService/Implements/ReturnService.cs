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
    public partial class ReportService : IReturnService
    {
        public ReportResponse<ReturnHierarchySummary> GetReturnSummaryByEntity(GenericReportFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidate(ReportFilterValidator.ALL);
            if (results.IsValid)
            {
                return _ReturnRepository.GetReturnSummaryByEntity(req);
            }
            else
            {
                ReportResponse<ReturnHierarchySummary> data = new ReportResponse<ReturnHierarchySummary>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }

        public ReportResponse<ReturnSummary> GetReturnSummaryByMerchant(MerchantSummaryFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidateForMerchantFilter(MerchantSummaryFilterValidator.ALL);
            if (results.IsValid)
            {
                return _ReturnRepository.GetReturnSummaryByMerchant(req);
            }
            else
            {
                ReportResponse<ReturnSummary> data = new ReportResponse<ReturnSummary>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }
    }
}
