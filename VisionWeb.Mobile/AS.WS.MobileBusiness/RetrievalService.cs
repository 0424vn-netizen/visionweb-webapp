using System.Linq;
using AS.WS.Mobile.Domain.Models;
using AS.VW.Common;

namespace AS.WS.MobileBusiness
{
    public partial class MobileService
    {
        private static readonly string SP_GET_RETRIEVALS_BY_REPORT_DATE = "spa_api_GetRetrievalByReportDate";
        private static readonly string SP_GET_RETRIEVALS_DETAIL = "spa_api_GetRetrievalDetail";
        private static readonly string SP_GET_RETRIEVALS_SUM_BY_DAYS = "spa_api_GetRetrievalSumByDays";

        public Retrievals GetRetrievalsByLastDays(MobileParameters ps)
        {
            var data = GetReportByLastDays(ps, SP_GET_RETRIEVALS_SUM_BY_DAYS);
            if (data == null)
                return null;
            
            var result = new Retrievals()
            {
                Items = data.To<Retrieval>().ToList()
            };
            return result;
        }

        public Retrievals GetRetrievalsByReportDate(MobileParameters ps)
        {
            var data = GetReportByReportDate(ps, SP_GET_RETRIEVALS_BY_REPORT_DATE);
            if (data == null)
            {
                return null;
            }

            var result = new Retrievals()
            {
                Items = data.To<Retrieval>().ToList()
            };

            return result;
        }


        public RetrievalDetail GetRetrievalDetails(MobileParameters ps)
        {
            var data = GetRecordDetailsByRecId(ps, SP_GET_RETRIEVALS_DETAIL);
            if (data == null || data.Rows.Count == 0)
            {
                return null;
            }

            var rowData = data.Rows[0];
            return rowData.To<RetrievalDetail>();
            
        }
    }
}
