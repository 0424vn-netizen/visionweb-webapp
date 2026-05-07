using System.Linq;
using AS.WS.Mobile.Domain.Models;
using AS.VW.Common;

namespace AS.WS.MobileBusiness
{
    public partial class MobileService
    {
        public Chargebacks GetChargebacksByLastDays(MobileParameters ps)
        {
            var data = GetReportByLastDays(ps, SP_GET_CHARGEBACK_BY_DAYS);
            if (data == null)
            {
                return null;
            }

            var result = new Chargebacks()
            {
                Items = data.To<Chargeback>().ToList()
            };

            return result;
        }

        public Chargebacks GetChargebacksByReportDate(MobileParameters ps)
        {
            var data = GetReportByReportDate(ps, SP_GET_CHARGEBACK_BY_REPORT_DATE);
            if (data == null)
            {
                return null;
            }

            var result = new Chargebacks()
            {
                Items = data.To<Chargeback>().ToList()
            };

            return result;
        }

        public ChargebackDetail GetChargebackDetails(MobileParameters ps)
        {
            var data = GetRecordDetailsByRecId(ps, SP_GET_CHARGEBACK_DETAIL);
            if (data == null || data.Rows.Count == 0)
            {
                return null;
            }

            var rowData = data.Rows[0];
            return rowData.To<ChargebackDetail>();
        }
    }
}
