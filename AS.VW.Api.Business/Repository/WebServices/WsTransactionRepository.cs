using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Model.Dto;

namespace AS.VW.Api.Business.Repository.WebService
{
    public class WsTransactionRepository : ReportRepositoryBase, ITransactionRepository
    {
        public ReportResponse<TransactionDetail> GetTransactions(TransactionSearchFilter filteringOptions)
        {
            return _apiWebService.GetTransactions(PrincipalUser, filteringOptions);
        }

        public Voucher GetVoucher(VoucherFilter filteringOptions)
        {
            return _apiWebService.GetVoucher(PrincipalUser, filteringOptions);
        }
    }
}
