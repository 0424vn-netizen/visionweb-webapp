using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    public class InterchangeDetail
    {
        public long RecordID { get; set; }
        public int AsClientID { get; set; }
        public int SiteID { get; set; }
        public string MerchantNumber { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionCodeDescription { get; set; }
        public string TransactionID { get; set; }
        public string QualificationCode { get; set; }
        public string QualificationCodeDescription { get; set; }
        public decimal AuthorizationAmount { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
