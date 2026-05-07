using System.Linq;
using AS.WS.Mobile.Domain.Models;
using System.Collections.Generic;
using AS.VW.Common;

namespace AS.WS.MobileBusiness
{
    public partial class MobileService
    {
        private static readonly string SP_GET_DEPOSIT_BY_REPORT_DATE = "spa_api_GetDepositByReportDate";
        private static readonly string SP_GET_DEPOSIT_SUM_BY_DAYS = "spa_api_GetDepositSumByDays";

        public Deposits GetDepositsByLastDays(MobileParameters ps)
        {
            var data = GetReportByLastDays(ps, SP_GET_DEPOSIT_SUM_BY_DAYS);
            if (data == null)
                return null;

            var result = new Deposits()
            {
                Items = data.To<Deposit>().ToList()
            };

            foreach (var item in result.Items)
            {
                ps.Mid = item.Mid;
                ps.ReportDate = item.Date;
            }

            return result;
        }

        public DepositDetail GetDepositsByReportDate(MobileParameters ps)
        {
            var data = GetReportByReportDate2Table(ps, SP_GET_DEPOSIT_BY_REPORT_DATE);
            var depositDetail = new DepositDetail();

            if (data == null || data.Length < 1)
            {
                return depositDetail;
            }
            if (data[0] != null || data[0].Rows.Count > 0)
            {
                var rowData = data[0].Rows[0];
                var debitDeposit = new DebitDeposit()
                {
                    ReturnAmount = (decimal)rowData["ReturnAmount"],
                    SaleAmount = (decimal)rowData["SalesAmount"],
                    ReturnCount = (long)rowData["ReturnCount"],
                    SaleCount = (long)rowData["SalesCount"]
                };

                depositDetail.DebitDeposit = debitDeposit;
            }
            //
            if (data[1] != null || data[1].Rows.Count > 0)
            {
                depositDetail.ListTransactionTypes = data[1].To<TransactionTypes>().ToList();
            }
            return depositDetail;
        }
    }
}
