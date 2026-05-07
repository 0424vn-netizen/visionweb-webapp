using System.Collections.Generic;
using System.Data;
using System.Linq;
using AS.VW.Common;
using AS.WS.Mobile.Domain.Models;

namespace AS.WS.MobileBusiness
{
    public partial class MobileService
    {
        public Monthly GetMonthlyVolume(MobileParameters ps)
        {
            var data = GetReportByMonth(ps, SP_GET_MONTHLY_VOLUME);
            if (data == null)
            {
                return null;
            }
            var result = new Monthly()
            {
                Items = data.To<MonthlyVolume>().ToList()
            };
            return result;
        }

        public Analysis GetVolumeAnalysis(MobileParameters ps)
        {
            var data = GetReportByMonth(ps, SP_GET_VOLUME_ANALYSIS);
            if (data == null)
            {
                return null;
            }

            var result = new Analysis()
            {
                Items = data.To<VolumeAnalysis>().ToList()
            };
            return result;
        }

        public Card GetCardVolume(MobileParameters ps)
        {
            var data = GetReportByMonth(ps, SP_GET_CARD_VOLUME);
            if (data == null)
            {
                return null;
            }
            var result = new Card()
            {
                Items = data.To<CardVolume>().ToList()
            };
            return result;
        }

        public ChartItem GetChartsVolume(MobileParameters ps)
        {
            var data = GetReportByChart(ps, SP_GET_CHART_VOLUME);
            if (data == null)
            {
                return null;
            }
            var result = new ChartItem()
            {
                Items = data.To<ChartVolume>().ToList()
            };
            return result;
        }
    }
}
