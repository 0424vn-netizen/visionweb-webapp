using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Cybersource.Auth.Model
{
    public class SensitiveTag
    {
        public string tagName { get; set; }
        public string pattern { get; set; }
        public string replacement { get; set; }
        public bool disableMask { get; set; }
        public SensitiveTag(string tagName, string pattern, string replacement, bool disableMask = false)
        {
            this.tagName = tagName;
            this.pattern = pattern;
            this.replacement = replacement;
            this.disableMask = disableMask;
        }
    }
}
