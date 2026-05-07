using AS.Common.DBManager;
using AS.VW.Api.RestClient.Logging;
using AS.VW.Api.RestClient.Models;
using AS.Web.Business;
using System;
using System.Configuration;
using System.Data;
using System.Web;

namespace AS.VW.PCI.Api.Client.Common
{
    /// <summary>
    /// The API logging service
    /// </summary>
    /// <seealso cref="DefaultLoggingService" />
    public sealed class ApiLoggingService : DefaultLoggingService
    {
        private readonly ReportServices _reportServices = null;

        /// <summary>
        /// The lazy instance
        /// </summary>
        private static readonly Lazy<ApiLoggingService> _lazyInstance = new Lazy<ApiLoggingService>(() => new ApiLoggingService());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static ApiLoggingService Instance
        {
            get { return _lazyInstance.Value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiLoggingService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        private ApiLoggingService()
            : base(VWLogger.Instance)
        {
            var appSettings = ConfigurationManager.AppSettings;

            this._reportServices = new ReportServices(appSettings["CS_Report_WS_URL"], appSettings["CS_Report_WS_Token1"], appSettings["CS_Report_WS_Token2"]);
        }

        /// <summary>
        /// Logs the API request.
        /// </summary>
        /// <param name="trackingInfo">The tracking information.</param>
        public override void LogRequest(ApiTrackingInfo trackingInfo)
        {
            if (trackingInfo == null)
            {
                return;
            }

            try
            {
                base.LogRequest(trackingInfo);

                if (_reportServices != null)
                {
                    var parameters = new FilterParameterCollection();

                    parameters.Add(new FilterParameter("@TrackingId", trackingInfo.TrackingId, DbType.String));
                    parameters.Add(new FilterParameter("@Source", trackingInfo.Source, DbType.String));
                    parameters.Add(new FilterParameter("@Resource", trackingInfo.Resource, DbType.String));
                    parameters.Add(new FilterParameter("@Method", trackingInfo.Method, DbType.String));
                    parameters.Add(new FilterParameter("@RequestParameters", trackingInfo.RequestParameters, DbType.String));
                    parameters.Add(new FilterParameter("@RequestHeaders", trackingInfo.RequestHeaders, DbType.String));
                    parameters.Add(new FilterParameter("@ResponseStatusCode", trackingInfo.ResponseStatusCode, DbType.String));
                    parameters.Add(new FilterParameter("@ResponseHeaders", trackingInfo.ResponseHeaders, DbType.String));
                    parameters.Add(new FilterParameter("@ResponseContent", trackingInfo.ResponseContent, DbType.String));
                    parameters.Add(new FilterParameter("@Exception", trackingInfo.Exception, DbType.String));
                    parameters.Add(new FilterParameter("@ErrorMessage", trackingInfo.ErrorMessage, DbType.String));
                    parameters.Add(new FilterParameter("@OtherInfo", trackingInfo.OtherInfo, DbType.String));
                    parameters.Add(new FilterParameter("@ClientIPAddress", this.GetClientIpAddress(), DbType.String));
                    parameters.Add(new FilterParameter("@ServerIPAddress", this.GetServerIpAddress(), DbType.String));
                    parameters.Add(new FilterParameter("@Duration", trackingInfo.Duration, DbType.Double));

                    //this._reportServices.ExecuteNonQueryCommand("spa_LOG_InsertRequestAPITracking", parameters, out FilterParameterCollection outParameters);
                }
            }
            catch (Exception exception)
            {
                this.Logger.Error(exception);
            }
        }

        /// <summary>
        /// Gets the IP Address of current server.
        /// </summary>
        /// <returns></returns>
        private string GetServerIpAddress()
        {
            if (HttpContext.Current != null)
                return HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            return string.Empty;
        }

        /// <summary>
        /// Gets the IP Address of client.
        /// </summary>
        /// <returns></returns>
        private string GetClientIpAddress()
        {
            if (HttpContext.Current != null)
            {
                var clientIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrWhiteSpace(clientIPAddress))
                {
                    clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                return clientIPAddress;
            }
            return string.Empty;
        }

    }
}
