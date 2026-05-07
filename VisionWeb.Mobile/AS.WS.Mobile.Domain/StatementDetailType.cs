using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Mobile.Domain
{
    public enum StatementDetailType
    {
        Plan = 1,
        Deposit = 2,
        Adjustment = 3,
        Chargeback = 4,
        Card = 5,
        SettlementDiscount = 6,
        OtherFees = 7,
        More = 8,
        DepositItemSummary = 9,
        MonthlyMessages = 10,
        Surcharge = 11
    }
}
