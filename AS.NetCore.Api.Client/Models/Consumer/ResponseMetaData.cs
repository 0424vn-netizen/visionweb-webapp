using System;
using System.Linq;

namespace AS.NetCore.Api.Client.Models
{
    public class ResponseMetaData : DataModel
    {    /// <summary>
         /// Messages
         /// </summary>
        public Message[] Messages { get; set; }

        /// <summary>
        /// Pagination
        /// </summary>
        public Pagination Pagination { get; set; }

        /// <summary>
        /// TimeStamp
        /// </summary>
        public DateTime TimeStamp { get; set; }

        public bool IsSuccess()
        {
            return this.Messages!= null && !this.Messages.Any(q => !q.IsSuccess());
        }
    }
}
