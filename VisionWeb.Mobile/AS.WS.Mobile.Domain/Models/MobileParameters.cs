using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Mobile.Domain.Models
{
    public class MobileParameters
    {
        //Common parameters
        public int ASClientID { get; set; }
        public string UserID { get; set; }
        public int SiteID { get; set; }
        public string UserMode { get; set; }
        public string HierarchyFilterMode { get; set; }
        public string HierarchyFilterValue { get; set; }

        // For Statement
        public int StatementType { get; set; }
        public Guid RecId { get; set; }
        public int StatementDetailType { get; set; }

        //Custom parameters
        public string Mid { get; set; }
        public string BatchNumber { get; set; }
        public int Day { get; set; }
        public int Months { get; set; }
        public DateTime ReportDate { get; set; }
        public bool IsPaging { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public long RecordId { get; set; }

        public int EntityTypeID { get; set; }
        public string EntityNumber { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Processors { get; set; }

        public string Mode { get; set; }
        public int BeginYearMonth { get; set; }
        public int EndYearMonth { get; set; }

        public string DocId { get; set; }
        public string HierarchyStatementMode { get; set; }
        public string HierarchyStatementValue { get; set; }
        public string StatementName { get; set; }
    }
}
