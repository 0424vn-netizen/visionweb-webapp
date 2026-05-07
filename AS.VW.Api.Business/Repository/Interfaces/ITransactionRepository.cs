using AS.VW.Api.Model.Dto;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;

namespace AS.VW.Api.Business.Repository
{
    public interface ITransactionRepository : IAuthenticatedRepository
    {
        ReportResponse<TransactionDetail> GetTransactions(TransactionSearchFilter filteringOptions);

        Voucher GetVoucher(VoucherFilter filteringOptions);
    }
}
