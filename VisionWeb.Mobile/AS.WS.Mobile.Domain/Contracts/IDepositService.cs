using AS.WS.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Mobile.Domain.Contracts
{
    public partial interface IMobileService
    {
        Deposits GetDepositsByLastDays(MobileParameters ps);
        DepositDetail GetDepositsByReportDate(MobileParameters ps);
    }
}
