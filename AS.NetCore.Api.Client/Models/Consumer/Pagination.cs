using System;

namespace AS.NetCore.Api.Client.Models
{
    public class Pagination : DataModel
    {
        /// <summary>
        /// PageOffset
        /// </summary>
        /// <example>
        /// 1
        /// </example>
        public int PageOffset { get; set; }
        /// <summary>
        /// PageLimit
        /// </summary>
        /// <example>
        /// 10
        /// </example>
        public int PageLimit { get; set; }
        /// <summary>
        /// TotalCount
        /// </summary>
        /// <example>
        /// 0
        /// </example>
        public int TotalCount { get; set; }
    }
}
