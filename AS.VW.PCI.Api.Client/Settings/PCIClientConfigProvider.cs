using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace AS.VW.PCI.Api.Client.Settings
{
    public sealed class PCIClientConfigProvider
    {
        private static readonly Lazy<PCIClientConfigProvider> _lazyInstance =
            new Lazy<PCIClientConfigProvider>(() => new PCIClientConfigProvider());

        public static PCIClientConfigProvider Instance => _lazyInstance.Value;

        private readonly Dictionary<int, PCIClientConfig> _configMap;

        private PCIClientConfigProvider()
        {
            _configMap = LoadConfigs();
        }

        public PCIClientConfig GetConfig(int applicationId)
        {
            if (_configMap.TryGetValue(applicationId, out var config))
                return config;

            throw new InvalidOperationException(
                $"No PCI client config found for ApplicationId={applicationId}. Check App_Data/PciClients/pciClients.json.");
        }

        private static Dictionary<int, PCIClientConfig> LoadConfigs()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var postfix = ConfigurationManager.AppSettings["PciClientConfigPostfix"] ?? string.Empty;

            var envPath = Path.Combine(baseDir, "App_Data", "PciClients", $"pciClients{postfix}.json");
            var fallbackPath = Path.Combine(baseDir, "App_Data", "PciClients", "pciClients.json");

            var path = !string.IsNullOrEmpty(postfix) && File.Exists(envPath) ? envPath : fallbackPath;

            if (!File.Exists(path))
                throw new FileNotFoundException($"PCI client config file not found at: {path}");

            var json = File.ReadAllText(path);
            var file = JsonConvert.DeserializeObject<PCIClientConfigFile>(json);

            if (file?.Clients == null || file.Clients.Count == 0)
                throw new InvalidOperationException($"{Path.GetFileName(path)} is empty or malformed.");

            return file.Clients.ToDictionary(c => c.ApplicationId);
        }

        private class PCIClientConfigFile
        {
            [JsonProperty("clients")]
            public List<PCIClientConfig> Clients { get; set; }
        }
    }
}
