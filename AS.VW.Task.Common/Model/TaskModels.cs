using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Task.Common
{
    [Serializable]
    public class TaskResult
    {
        public string TaskName { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int Total { get; set; }
        public Exception Exception { get; set; }
    }
    [Serializable]
    public class TaskFailResult
    {
        public int ASClient { get; set; }
        public string MerchantNumber { get; set; }
        public string Message { get; set; }
    }
}
