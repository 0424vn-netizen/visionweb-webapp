using AS.VW.Api.Model.Dto;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Business.Repository;
using AS.VW.Api.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public partial class ReportService:IPaymentService
    {
        public ReportResponse<DepositHierarchySummary> GetPaymentSummaryByEntity(GenericReportFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidate(ReportFilterValidator.ALL);
            if (results.IsValid)
            {
                return _PaymentRepository.GetPaymentSummaryByEntity(req);
            }
            else
            {
                ReportResponse<DepositHierarchySummary> data = new ReportResponse<DepositHierarchySummary>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }

        public ReportResponse<DepositSummary> GetPaymentSummaryByMerchant(MerchantSummaryFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidateForMerchantFilter(MerchantSummaryFilterValidator.ALL);
            if (results.IsValid)
            {
                return _PaymentRepository.GetPaymentSummaryByMerchant(req);
            }
            else
            {
                ReportResponse<DepositSummary> data = new ReportResponse<DepositSummary>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }

        public ReportResponse<DepositDetail> GetPaymentDetail(MerchantSummaryFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidateForMerchantFilter(MerchantSummaryFilterValidator.ALL);
            if (results.IsValid)
            {
                return _PaymentRepository.GetPaymentDetail(req);
            }
            else
            {
                ReportResponse<DepositDetail> data = new ReportResponse<DepositDetail>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }
    }
}
