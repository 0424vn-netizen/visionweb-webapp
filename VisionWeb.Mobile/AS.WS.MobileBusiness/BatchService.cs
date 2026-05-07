using System.Collections.Generic;
using System.Linq;
using System.Data;
using AS.WS.Mobile.Domain.Models;
using System;
using AS.WS.Mobile.Domain;
using AS.VW.Common;

namespace AS.WS.MobileBusiness
{
    public partial class MobileService
    {
        public Batches GetBatchesByLastDays(MobileParameters ps)
        {
            var data = GetReportByLastDays(ps, SP_GET_BATCH_SUM_BY_DAYS);
            if (data == null)
            {
                return null;
            }
            return new Batches() {
                Items = data.To<Batch>().ToList()
            };
        }

        public BatchDetails GetBatchDetail(MobileParameters ps)
        {
            var data = GetBatchDetail(ps, SP_GET_BATCH_BY_BATCHNUMBER);
            if (data == null || data.Length < 2)
            {
                return null;
            }

            var batchDetails = new BatchDetails ();
            if (data[1].Rows.Count > 0)
            {
                batchDetails.CardInfos = Create(data[1].Rows[0]);
            }
            return batchDetails;
        }

        public BatchNumberDetails GetBatchsByReportDate(MobileParameters ps)
        {
            var data = GetReportByReportDate(ps, SP_GET_BATCH_BY_REPORT_DATE);
            if (data == null)
            {
                return null;
            }

            var batchNumberDetails = new BatchNumberDetails();
            batchNumberDetails.Details = data.To<BatchNumberDetail>().ToList();

            return batchNumberDetails;
        }

        private static List<CardInfo> Create(DataRow detailCardData)
        {
            var cards = new List<CardInfo>();

            if (detailCardData["MasterCardNetAmount"].IsNotNullData())
            {
                cards.Add(new CardInfo
                {
                    Name = WSConstants.MASTER_CARD,
                    Amount = (decimal)detailCardData["MasterCardNetAmount"],
                    TransactionCount = Convert.ToInt64(detailCardData["MasterCardTransactionCount"])
                });
            }

            if (detailCardData["VisaNetAmount"].IsNotNullData())
            {
                cards.Add(new CardInfo
                {
                    Name = WSConstants.VISA_CARD,
                    Amount = (decimal)detailCardData["VisaNetAmount"],
                    TransactionCount = Convert.ToInt64(detailCardData["VisaTransactionCount"])
                });
            }

            if (detailCardData["AmericanExpressNetAmount"].IsNotNullData())
            {
                cards.Add(new CardInfo
                {
                    Name = WSConstants.AMERICAN_EXPRESS_CARD,
                    Amount = (decimal)detailCardData["AmericanExpressNetAmount"],
                    TransactionCount = Convert.ToInt64(detailCardData["AmericanExpressTransactionCount"])
                });
            }


            if (detailCardData["DiscoverNetAmount"].IsNotNullData())
            {
                cards.Add(new CardInfo
                {
                    Name = WSConstants.DISCOVER_CARD,
                    Amount = (decimal)detailCardData["DiscoverNetAmount"],
                    TransactionCount = Convert.ToInt64(detailCardData["DiscoverTransactionCount"])
                });
            }


            if (detailCardData["DebitCardNetAmount"].IsNotNullData())
            {
                cards.Add(new CardInfo
                {
                    Name = WSConstants.DEBIT_CARD,
                    Amount = (decimal)detailCardData["DebitCardNetAmount"],
                    TransactionCount = Convert.ToInt64(detailCardData["DebitCardTransactionCount"])
                });
            }

            if (detailCardData["OtherNetAmount"].IsNotNullData())
            {
                cards.Add(new CardInfo
                {
                    Name = WSConstants.OTHER_CARD,
                    Amount = (decimal)detailCardData["OtherNetAmount"],
                    TransactionCount = Convert.ToInt64(detailCardData["OtherTransactionCount"])
                });
            }
            return cards;
        }
    }
}
