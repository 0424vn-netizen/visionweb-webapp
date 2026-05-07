using AS.VW.Api.RestClient.Models;
using AS.VW.Api.RestClient.Providers;
using AS.Web.Business;
using AS.Web.Business.PCI;
using System;
using System.Configuration;

namespace AS.VW.PCI.Api.Client.Settings
{
    public sealed class PCIClientSettings : RestClientSettings
    {
        public ReportServices ReportServices { get; set; }

        public PciServices PciReportServices { get; set; }

        private static readonly Lazy<PCIClientSettings> _lazyInstance = new Lazy<PCIClientSettings>(() => new PCIClientSettings());

        public static PCIClientSettings Instance
        {
            get { return _lazyInstance.Value; }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="PCIClientSettings"/> class from being created.
        /// </summary>
        private PCIClientSettings()
        {
            this.AuthenticatorProvider = new SimpleAuthenticatorProvider();

            var appSettings = ConfigurationManager.AppSettings;

            this.ReportServices = new ReportServices(
                appSettings["CS_Report_WS_URL"],
                appSettings["CS_Report_WS_Token1"],
                appSettings["CS_Report_WS_Token2"]);

            bool.TryParse(appSettings["PCI_WS_Security_Protocol"], out bool securityProtocol);
            this.PciReportServices = new PciServices(
                appSettings["PCI_WS_URL"],
                appSettings["PCI_WS_Token1"],
                appSettings["PCI_WS_Token2"],
                securityProtocol);
        }
    }
}
