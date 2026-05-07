using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Report
{
    public class AdjustmentHierarchySummary
    {
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public int SalesCount { get; set; }
        public decimal SalesAmount { get; set; }
        public int ReturnsCount { get; set; }
        public decimal ReturnsAmount { get; set; }
    }
}
