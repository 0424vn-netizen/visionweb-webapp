using System.Net;

namespace AS.VW.PCI.Api.Client.Models.Common
{
    public class PCIApiResponse<T>
    {
        public bool IsSuccess { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string TrackingId { get; set; }

        public string ErrorMessage { get; set; }

        public T Data { get; set; }

        public string RawResponse { get; set; }
    }

}
