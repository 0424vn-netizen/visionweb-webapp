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
    public partial class ReportService : ITransactionService
    {
        public ReportResponse<TransactionDetail> GetTransactions(TransactionSearchFilter req)
        {
            req.SetDefaultValueForDateFilter();
          
            req.HierarchyFilterValue = req.HierarchyFilterValue ?? string.Empty;

            req.SetDefaultValueForPagingFilter();
            ValidationResult results = req.ValidateFilter<TransactionSearchFilterValidator, TransactionSearchFilter>(TransactionSearchFilterValidator.ALL_AND_EXTEND_RULES);
            if (results.IsValid)
            {
                return _TransactionRepository.GetTransactions(req);
            }
            else
            {
                ReportResponse<TransactionDetail> data = new ReportResponse<TransactionDetail>();
                data.Validation = ValidatorFilterExtensions.ProcessError(results);
                return data;
            }
        }
    }
}
