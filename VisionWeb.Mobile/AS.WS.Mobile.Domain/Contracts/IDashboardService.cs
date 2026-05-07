using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.WS.Mobile.Domain.Models;

namespace AS.WS.Mobile.Domain.Contracts
{
    public partial interface IMobileService
    {
        Monthly GetMonthlyVolume(MobileParameters ps);
        Card GetCardVolume(MobileParameters ps);
        Analysis GetVolumeAnalysis(MobileParameters ps);
        ChartItem GetChartsVolume(MobileParameters ps);
    }
}
