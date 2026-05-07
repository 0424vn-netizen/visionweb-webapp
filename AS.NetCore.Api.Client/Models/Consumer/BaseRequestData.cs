using System;

namespace AS.NetCore.Api.Client.Models
{
    public class BaseRequestData<T> : DataModel where T : RequestBody
    {
        /// <summary>
        /// Header data of request
        /// </summary>
        public RequestHeader Header { get; set; } = new RequestHeader();

        /// <summary>
        /// Metadata cua request
        /// </summary>
        public RequestMetaData Meta { get; set; } = new RequestMetaData();

        /// <summary>
        /// The body data of requests
        /// </summary>
        public T Data { get; set; }

        public BaseRequestData<T> Clone()
        {
            return new BaseRequestData<T>
            {
                Header = this.Header,
                Meta = this.Meta,
                Data = this.Data
            };
        }
    }
}
