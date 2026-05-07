using System;
using System.Collections.Generic;

namespace AS.WS.Mobile.Domain.Models
{
    public class Chargebacks
    {
        public List<Chargeback> Items { get; set; }

        public Chargebacks()
        {
            Items = new List<Chargeback>();
        }
    }

    public class Chargeback
    {
        public long RecordID { get; set; }

        public DateTime ReportDate { get; set; }

        public string Mid { get; set; }

        public decimal ChargebackAmount { get; set; }
        public string ReferenceNumber { get; set; }
    }

    public class ChargebackDetail
    {
        public string CardNumber { get; set; }
        public string ReasonText { get; set; }
        public string CardDescription { get; set; }
    }
}