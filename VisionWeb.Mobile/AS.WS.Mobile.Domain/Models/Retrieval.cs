using System;
using System.Collections.Generic;

namespace AS.WS.Mobile.Domain.Models
{
    public class Retrievals
    {
        public List<Retrieval> Items { get; set; }

        public Retrievals()
        {
            Items = new List<Retrieval>();
        }
    }

    public class Retrieval
    {
        public long RecordID { get; set; }

        public DateTime ReportDate { get; set; }

        public string Mid { get; set; }

        public decimal RetrievalAmount { get; set; }
        public string ReferenceNumber { get; set; }
    }

    public class RetrievalDetail
    {
        public string CardNumber { get; set; }
        public string ReasonText { get; set; }
        public string CardDescription { get; set; }
    }

}