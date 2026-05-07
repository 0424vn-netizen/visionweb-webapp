using System;

namespace AS.NetCore.Api.Client.Models
{
    public class RequestOrder
    {
        /// <summary>
        /// SortBy: ColumnName
        /// </summary>
        /// <example>
        /// </example>
        public string SortBy { get; set; }

        /// <summary>
        /// OrderBy: ASC/DESC
        /// </summary>
        /// <example>
        /// </example>
        public string OrderBy { get; set; }
    }
}
