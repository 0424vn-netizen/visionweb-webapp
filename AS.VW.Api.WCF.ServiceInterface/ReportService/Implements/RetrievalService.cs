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
    public partial class ReportService : IRetrievalService
    {
        ReportResponse<RetrievalHierarchySummary> IRetrievalService.GetRetrievalsSummary(GenericReportFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidate(ReportFilterValidator.ALL);
            if (results.IsValid)
            {
                return _RetrievalRepository.GetRetrievalSummaryByEntity(req);
            }
            else
            {
                ReportResponse<RetrievalHierarchySummary> data = new ReportResponse<RetrievalHierarchySummary>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }

        ReportResponse<RetrievalDetail> IRetrievalService.GetRetrievalsDetail(MerchantSummaryFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidateForMerchantFilter(MerchantSummaryFilterValidator.ALL);
            if (results.IsValid)
            {
                return _RetrievalRepository.GetRetrievalDetail(req);
            }
            else
            {
                ReportResponse<RetrievalDetail> data = new ReportResponse<RetrievalDetail>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }
    }
}
