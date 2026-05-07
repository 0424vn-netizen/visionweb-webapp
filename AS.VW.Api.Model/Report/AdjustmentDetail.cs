using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    public class AdjustmentDetail
    {
        public long RecordID { get; set; }
        public string MerchantNumber { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string BatchNumber { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionCodeDescription { get; set; }
        public string OriginalReferenceID { get; set; }
        public int SalesCount { get; set; }
        public decimal SalesAmount { get; set; }
        public int ReturnsCount { get; set; }
        public decimal ReturnsAmount { get; set; }
    }
}
