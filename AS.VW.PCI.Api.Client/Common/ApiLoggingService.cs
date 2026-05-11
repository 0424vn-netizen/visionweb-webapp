using AS.VW.Api.RestClient.Logging;
using AS.VW.Api.RestClient.Models;
using System;
using System.Web;

namespace AS.VW.PCI.Api.Client.Common
{
    public sealed class ApiLoggingService : DefaultLoggingService
    {
        private static readonly Lazy<ApiLoggingService> _lazyInstance =
            new Lazy<ApiLoggingService>(() => new ApiLoggingService());

        public static ApiLoggingService Instance => _lazyInstance.Value;

        private ApiLoggingService() : base(VWLogger.Instance) { }

        public override void LogRequest(ApiTrackingInfo trackingInfo)
        {
            if (trackingInfo == null) return;

            try
            {
                base.LogRequest(trackingInfo);
            }
            catch (Exception exception)
            {
                this.Logger.Error(exception);
            }
        }

        private string GetServerIpAddress() =>
            HttpContext.Current?.Request.ServerVariables["LOCAL_ADDR"] ?? string.Empty;

        private string GetClientIpAddress()
        {
            if (HttpContext.Current == null) return string.Empty;
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            return string.IsNullOrWhiteSpace(ip)
                ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                : ip;
        }
    }
}
