using AS.VW.Api.RestClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.PCI.Api.Client.Models.Common
{
    /// <summary>
    /// The API response Interface
    /// </summary>
    public interface IApiResponse
    {
        /// <summary>
        /// Gets or sets the API response raw.
        /// </summary>
        string RawResponse { get; set; }

        /// <summary>
        /// Gets or sets the API response status.
        /// </summary>
        ApiResponseStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the response http status code.
        /// </summary>
        HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        List<ApiMessage> Messages { get; set; }

        /// <summary>
        /// Gets or sets the tracking identifier.
        /// </summary>
        string TrackingId { get; set; }
    }

    /// <summary>
    /// The API response with data Interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApiResponse<T> : IApiResponse
    {
        /// <summary>
        /// The API response data.
        /// </summary>
        T Data { get; set; }
    }

    /// <summary>
    /// The API response
    /// </summary>
    /// <seealso cref="IApiResponse" />
    [Serializable]
    public class ApiResponse : IApiResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets the API response raw.
        /// </summary>
        public string RawResponse { get; set; }

        /// <summary>
        /// Gets or sets the API response status.
        /// </summary>
        public ApiResponseStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the response http status code.
        /// </summary>
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        public List<ApiMessage> Messages { get; set; }

        /// <summary>
        /// Gets or sets the tracking identifier.
        /// </summary>
        public string TrackingId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        public ApiResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        /// <param name="response">The API response.</param>
        public ApiResponse(ApiResponse response)
        {
            if (response == null)
            {
                return;
            }

            this.Status = response.Status;
            this.StatusCode = response.StatusCode;
            this.TrackingId = response.TrackingId;
        }

        #endregion
    }

    /// <summary>
    /// The API response with data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ApiResponse<T> : ApiResponse, IApiResponse<T>
    {
        /// <summary>
        /// The response data.
        /// </summary>
        public T Data { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        public ApiResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        /// <param name="response">The API response.</param>
        public ApiResponse(ApiResponse response)
            : base(response)
        {

        }

        #endregion

    }

    [Serializable]
    public enum ApiResponseStatus
    {
        /// <summary>
        /// Success
        /// </summary>
        Success,

        /// <summary>
        /// Error
        /// </summary>
        Error,

        /// <summary>
        /// Business Error
        /// </summary>
        BusinessError,

        /// <summary>
        /// The current operation is blocked due to invalid or incomplete input data.
        /// Please review the provided values and ensure all required fields are correctly filled before proceeding.
        /// </summary>
        InputError

    }
}
