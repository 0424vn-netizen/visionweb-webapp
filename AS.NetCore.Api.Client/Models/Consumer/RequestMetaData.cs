using System;

namespace AS.NetCore.Api.Client.Models
{
    public class RequestMetaData : DataModel
    {
        /// <summary>
        /// Paging info
        /// </summary>
        public Pagination Pagination { get; set; }

        /// <summary>
        /// Time of request
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Order
        /// </summary>
        public RequestOrder Order { get; set; }
    }
}
