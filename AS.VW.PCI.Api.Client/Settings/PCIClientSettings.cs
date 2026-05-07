using AS.VW.Api.RestClient.Models;
using AS.VW.Api.RestClient.Providers;
using System;

namespace AS.VW.PCI.Api.Client.Settings
{
    public sealed class PCIClientSettings : RestClientSettings
    {
        private static readonly Lazy<PCIClientSettings> _lazyInstance = new Lazy<PCIClientSettings>(() => new PCIClientSettings());

        public static PCIClientSettings Instance => _lazyInstance.Value;

        /// <summary>
        /// Prevents a default instance of the <see cref="PCIClientSettings"/> class from being created.
        /// </summary>
        private PCIClientSettings()
        {
            this.AuthenticatorProvider = new SimpleAuthenticatorProvider();
        }
    }
}
