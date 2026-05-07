using System;
using System.Collections.Generic;

namespace AS.NetCore.Api.Client.Models
{
    /// <summary>
    /// Base Response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResponse<T> : ResponseBody
    {
        public T Data { get; set; }
    }

    /// <summary>
    /// Base Response List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResponseList<T> : ResponseBody
    {
        public List<T> Data { get; set; }
    }

    /// <summary>
    /// Base model for Add/Update data
    /// </summary>
    /// <seealso cref="Aperia.NetCore.Model.Base.ResponseBody" />
    public class BaseModifyResponse : ResponseBody
    {
        /// <summary>
        /// Indicator success or not.
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// The Id of created/modified item.
        /// </summary>
        public int? Id { get; set; }
    }
}
