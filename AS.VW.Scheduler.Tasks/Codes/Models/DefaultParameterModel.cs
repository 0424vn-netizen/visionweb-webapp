using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Tasks.Codes.Models
{
    public class DefaultParameterModel    
    {
        public Int32 ASClient { get; set; }  
        public string UserMode { get; set; }    
        public string UserID { get; set; }          
        public Int32 SiteID { get; set; }
        public Int32 LanguageID { get; set; }

        public string StOrder { get; set; }
        public string StFilter { get; set; }  
    }
}
