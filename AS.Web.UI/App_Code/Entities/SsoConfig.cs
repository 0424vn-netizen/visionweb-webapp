using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AS.SSO
{
    [Serializable]
    [XmlRoot("sso")]
    public class SsoConfig
    {
        [XmlElement("client")]
        public List<SsoClient> Clients { get; set; }
    }

    public class SsoClient
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("setting")]
        public ClientSetting Setting { get; set; }
    }

    public class ClientSetting
    {
        [XmlAttribute("certPath")]
        public string CertificatePath { get; set; }

        [XmlAttribute("certPassword")]
        public string CertificatePassword { get; set; }

        [XmlAttribute("signResponse")]
        public bool IsResponseSigned { get; set; }

        [XmlAttribute("signAssertion")]
        public bool IsAssertionSigned { get; set; }

        [XmlAttribute("encryptAssertion")]
        public bool IsAssertionEncrypted { get; set; }

        [XmlElement("add")]
        public List<ClientExtendSetting> ExtendSettings { get; set; }

        public string GetExtendSetting(string key)
        {
            var setting = this.ExtendSettings.FirstOrDefault(x => key.Equals(x.Key, StringComparison.OrdinalIgnoreCase));
            return setting == null ? null : setting.Value;
        }
    }

    public class ClientExtendSetting
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    public class SsoConfiguration
    {
        public List<string> SsoTrackingForClient { get; set; }
        public bool IsDebug { get; set; }
        public bool IsLogRequest { get; set; } 
        public List<OtherConfigs> OtherConfigs { get; set; }
    }
    public class OtherConfigs
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}