using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    public class InterchangeHierarchySummary
    {
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public int DowngradesCount { get; set; }
        public int UpgradesCount { get; set; }
        public int FeesCount { get; set; }
        public int TransactionsCount { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal ReturnsAmount { get; set; }
        public decimal NetAmount { get; set; }
    }
}
