using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Dto;
using AS.VW.Api.Business.Repository;
using AS.VW.Api.Validation;
using FluentValidation.Results;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public partial class ReportService:IBatchService
    {
        public ReportResponse<BatchHierarchySummary> GetBatchSummaryByEntity(GenericReportFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidate(ReportFilterValidator.ALL);
            if (results.IsValid)
            {
                return _BatchRepository.GetBatchSummaryByEntity(req);
            }
            else
            {
                ReportResponse<BatchHierarchySummary> data = new ReportResponse<BatchHierarchySummary>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }

        public ReportResponse<BatchSummary> GetBatchSummaryByMerchant(MerchantSummaryFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidateForMerchantFilter(MerchantSummaryFilterValidator.ALL);
            if (results.IsValid)
            {
                return _BatchRepository.GetBatchSummaryByMerchant(req);
            }
            else
            {
                ReportResponse<BatchSummary> data = new ReportResponse<BatchSummary>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }

        public ReportResponse<TransactionDetailBatch> GetBatchDetail(BatchDetailFilter req)
        {
            ValidationResult results = req.SetDefaultValuesAndValidateForDetailFilterPaging<BatchDetailFilterValidator, BatchDetailFilter>(DetailFilterValidator.DEFAULT_RULES, BatchDetailFilterValidator.BATCH_NUMBER_RULES);
            if (results.IsValid)
            {
                return _BatchRepository.GetBatchDetail(req);
            }
            else
            {
                ReportResponse<TransactionDetailBatch> data = new ReportResponse<TransactionDetailBatch>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }

        public ReportResponse<CardSummary> GetCardSummary(CardSummaryFilter req)
        {            
            req.SetDefaultValueForDateFilter();
            req.SetDefaultValueForPagingFilter();
            ValidationResult results = req.ValidateFilter<CardSummaryFilterValidator, CardSummaryFilter>(CardSummaryFilterValidator.ALL, CardSummaryFilterValidator.BATCH_NUMBER_RULES, CardSummaryFilterValidator.HIERARCHY_FILTER_MODE_NOT_ACCEPT, HierarchyFilterValidator.REQUIRE_ALL);
            if (results.IsValid)
            {
                return _BatchRepository.GetCardSummary(req);
            }
            else
            {
                ReportResponse<CardSummary> data = new ReportResponse<CardSummary>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }

            
        }
    }
}
