using System;

namespace AS.NetCore.Api.Client.Models
{
    /// <summary>
    /// Header
    /// </summary>
    public class BaseResponseData<T> : DataModel where T : ResponseBody
    {
        /// <summary>
        /// Header
        /// </summary>
        public ResponseHeader Header { get; set; } = new ResponseHeader();

        /// <summary>
        /// Meta
        /// </summary>
        public ResponseMetaData Meta { get; set; } = new ResponseMetaData();

        /// <summary>
        /// Data
        /// </summary>
        public T Data { get; set; }

        public ResponseMessage Messages { get; set; }
    }

    public class ResponseMessage
    {
        public string Field { get; set; }
        public string Code { get; set; }
        public string Content { get; set; }
    }
}
