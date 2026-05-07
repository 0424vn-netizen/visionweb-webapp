using System.Collections.Generic;

namespace AS.VW.Scheduler.Cybersource.Auth.Model
{
    public class RawDataBatch
    {
        public object List { get; set; }

        public List<Dictionary<string, object>> Details { get; set; } = new List<Dictionary<string, object>>();
    }
}
