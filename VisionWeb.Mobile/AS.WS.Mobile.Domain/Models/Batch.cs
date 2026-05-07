using System;
using System.Collections.Generic;

namespace AS.WS.Mobile.Domain.Models
{
    public class Batches
    {
        public List<Batch> Items { get; set; }

        public Batches()
        {
            Items = new List<Batch>();
        }
    }  

    public class Batch
    {
        public long ID { get; set; }

        public DateTime ReportDate { get; set; }

        public long DateTick { get { return ReportDate.Ticks; } }

        public decimal NetAmount { get; set; }

        public string Mid { get; set; }
    }

    public class BatchDetails
    {
        public List<BatchDetail> Details { get; set; }
        public List<CardInfo> CardInfos { get; set; }

        public BatchDetails()
        {
            Details = new List<BatchDetail>();
            CardInfos = new List<CardInfo>();
        }
    }

    public class BatchDetail
    {
        public long ID { get; set; }

        public DateTime TransactionDate { get; set; }

        public string TransactionTime { get; set; }

        public decimal NetAmount { get; set; }

        public string BatchNumber { get; set; }

        public DateTime ReportDate { get; set; }
    }

    public class CardInfo
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }
        public long TransactionCount { get; set; }
    }

    public class BatchNumberDetail
    {
        public long RecordID { get; set; }
        public string BatchNumber { get; set; }
        public decimal NetAmount { get; set; }
        public long TransactionCount { get; set; }
    }

    public class BatchNumberDetails
    {
        public List<BatchNumberDetail> Details { get; set; }
    }
}