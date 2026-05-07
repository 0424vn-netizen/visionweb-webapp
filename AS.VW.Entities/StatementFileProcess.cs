using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Entities
{
    public class DownLoadItem
    {
        public long RecordID { get; set; }
        public string DocIdOfMerchantStatements { get; set; }
        public string FileName { get; set; }
        public string Language { get; set; }
        public string CreatedBy { get; set; }
        public int ASClientID { get; set; }
        public int SiteID { get; set; }
        public string UserMode { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
