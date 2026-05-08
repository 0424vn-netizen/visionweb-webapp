using System;
using System.Data;
using System.Configuration;
using System.Web;
using AS.Common.Mail;
using AS.Common.DataProtection;
using System.Collections.Generic;
using AS.Common.DBManager;
using Newtonsoft.Json;
using AS.Web.UI.AppCode.Entities;
using AS.Web.Business.Shared.Constants;

/// <summary>
/// Summary description for WebSiteSettings
/// </summary>
public class WebSiteSettings
{
    private const string CONFIG_SEPERATOR = ",";
    public static int DefaultSystem { get { return Convert.ToInt32(ConfigurationManager.AppSettings["DefaultSystem"]); } }
    public static int DefaultClient { get { return Convert.ToInt32(ConfigurationManager.AppSettings["DefaultClient"]); } }
    public static string WebSiteType { get { return ConfigurationManager.AppSettings["WebSiteType"]; } }
    public static string WebSiteGroup { get { return ConfigurationManager.AppSettings["WebSiteGroup"]; } }

    public static string ClientName { get { return ConfigurationManager.AppSettings["ClientName"]; } }
    public static string ClientAddress1 { get { return ConfigurationManager.AppSettings["ClientAddress1"]; } }
    public static string ClientAddress2 { get { return ConfigurationManager.AppSettings["ClientAddress2"]; } }
    public static string ClientZipCode { get { return ConfigurationManager.AppSettings["ClientZipCode"]; } }
    public static string ClientPhone { get { return ConfigurationManager.AppSettings["ClientPhone"]; } }
    public static string ClientFax { get { return ConfigurationManager.AppSettings["ClientFax"]; } }
    public static string ClientEmail { get { return ConfigurationManager.AppSettings["ClientEmail"]; } }

    public static string NoReplyEmail { get { return ConfigurationManager.AppSettings["NoReplyEmail"]; } }
    public static string ContactEmail { get { return ConfigurationManager.AppSettings["ContactEmail"]; } }

    public static string MsGate
    {
        //get { return ConfigurationManager.AppSettings["MsGate"]; } 
        get
        {
            string RemoteIp = RemoteIpAddress;
            string MsUrl = string.Empty;
            string[] ClientIps = WebSiteSettings.ClientNatIpAddresses.Split(new string[] { CONFIG_SEPERATOR }, StringSplitOptions.RemoveEmptyEntries);
            string[] ClientUrls = WebSiteSettings.MsGates.Split(new string[] { CONFIG_SEPERATOR }, StringSplitOptions.RemoveEmptyEntries);
            string currentRequestUrl = HttpContext.Current.Request.Url.Host;
            for (int i = 0; i < ClientIps.Length; i++)
            {
                //if (ClientIps[i] == RemoteIp)
                if (RemoteIp.StartsWith(ClientIps[i]) || currentRequestUrl.StartsWith(ClientIps[i]))
                {
                    MsUrl = ClientUrls[i];
                    break;
                }
            }

            if (MsUrl == string.Empty) MsUrl = ClientUrls[0];
            return MsUrl;
        }
    }
    public static string TaxGate
    {
        //get { return ConfigurationManager.AppSettings["MsGate"]; } 
        get
        {
            string RemoteIp = RemoteIpAddress;
            string TaxUrl = string.Empty;
            string[] ClientIps = null;
            if (!string.IsNullOrEmpty(WebSiteSettings.ClientNatIpAddresses))
            {
                WebSiteSettings.ClientNatIpAddresses.Split(new string[] { CONFIG_SEPERATOR }, StringSplitOptions.RemoveEmptyEntries);
            }
            string[] ClientUrls = WebSiteSettings.TaxGates.Split(new string[] { CONFIG_SEPERATOR }, StringSplitOptions.RemoveEmptyEntries);
            string currentRequestUrl = HttpContext.Current.Request.Url.Host;
            if (ClientIps != null)
            {
                for (int i = 0; i < ClientIps.Length; i++)
                {
                    //if (ClientIps[i] == RemoteIp)
                    if (RemoteIp.StartsWith(ClientIps[i]) || currentRequestUrl.StartsWith(ClientIps[i]))
                    {
                        TaxUrl = ClientUrls[i];
                        break;
                    }
                }
            }

            if (TaxUrl == string.Empty) TaxUrl = ClientUrls[0];
            return TaxUrl;
        }
    }
    public static string ExportTempFolder { get { return ConfigurationManager.AppSettings["ExportTempFolder"]; } }

    public static string RiskViewMore { get { return ConfigurationManager.AppSettings["RiskViewMore"]; } }
    public static string RiskParameterKeysAllowDecimal
    {
        get
        {
            var riskParameterKeysAllowDecimal = ConfigurationManager.AppSettings["RiskParameterKeysAllowDecimal"];
            return string.Concat(riskParameterKeysAllowDecimal, ",", ParametersAllowDecimal).Trim().TrimEnd(',');
        }
    }

    public static string MessageHierarchyViewMore { get { return ConfigurationManager.AppSettings["MessageHierarchyViewMore"]; } }

    public static string ExportThresholdOfRows { get { return ConfigurationManager.AppSettings["ExportThresholdOfRowsOfWS"]; } }

    public static SmtpMailSettings MailSettings
    {
        get
        {
            SmtpMailSettings mailSetting = new SmtpMailSettings();
            SmtpAuthentication authen;
            switch (ConfigurationManager.AppSettings["SmtpAuthentication"].ToLower())
            {
                case "anonymous":
                    authen = SmtpAuthentication.Anonymous;
                    break;
                case "basic":
                    authen = SmtpAuthentication.Basic;
                    break;
                case "ntlm":
                    authen = SmtpAuthentication.NTLM;
                    break;
                default:
                    authen = SmtpAuthentication.Anonymous;
                    break;
            }

            mailSetting.AuthenticationType = authen;
            mailSetting.EnableSSL = ConfigurationManager.AppSettings["SmtpSSL"] == "true" ? true : false;
            mailSetting.Password = Cryptophy.DecryptText(ConfigurationManager.AppSettings["SmtpUsername"]);
            mailSetting.Username = Cryptophy.DecryptText(ConfigurationManager.AppSettings["SmtpPassword"]);
            mailSetting.Server = ConfigurationManager.AppSettings["SmtpServer"];
            mailSetting.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);

            if (GeneralFuncsLib.HasExtendedSetting("CLIENT_SMTP_SETTING"))
            {
                SmtpMailSettings Client_MailSetting = JsonConvert.DeserializeObject<SmtpMailSettings>(GeneralFuncsLib.GetDataOfExtendedSetting("CLIENT_SMTP_SETTING"));
                if (Client_MailSetting.Server == null)
                {
                    return mailSetting;
                }
                mailSetting.AuthenticationType = Client_MailSetting.AuthenticationType != SmtpAuthentication.Anonymous ? Client_MailSetting.AuthenticationType : mailSetting.AuthenticationType;
                mailSetting.EnableSSL = Client_MailSetting.EnableSSL;
                mailSetting.Server = Client_MailSetting.Server != null ? Client_MailSetting.Server : mailSetting.Server;
                mailSetting.Port = Client_MailSetting.Port != 0 ? Client_MailSetting.Port : mailSetting.Port;
                mailSetting.Password = Client_MailSetting.Password != null ? Client_MailSetting.Password: mailSetting.Password;
                mailSetting.Username = Client_MailSetting.Username != null ? Client_MailSetting.Username : mailSetting.Username;
            }
            return mailSetting;
        }
    }

    public static string SourceIP
    {
        get
        {
            return ConfigurationManager.AppSettings["ClientSourceIPs"];
        }
    }

    public static string DDS_IP_Address
    {
        get
        {
            return ConfigurationManager.AppSettings["DDS_IP_Address"];
        }
    }

    public static string DDS_PCI_JumpUrl
    {
        get
        {
            return ConfigurationManager.AppSettings["DDS_PCI_JumpUrl"];
        }
    }

    public static string Client_IP_Address
    {
        get
        {
            return ConfigurationManager.AppSettings["Client_IP_Address"];
        }
    }

    public static string Client_PCI_JumpUrl
    {
        get
        {
            return ConfigurationManager.AppSettings["Client_PCI_JumpUrl"];
        }
    }


    public static string NonDDS_PCI_JumpUrl
    {
        get
        {
            return ConfigurationManager.AppSettings["NonDDS_PCI_JumpUrl"];
        }
    }



    private static string RemoteIpAddress
    {
        get
        {
            string clientIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(clientIPAddress)) clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return clientIPAddress;
        }
    }
    public static string ClientNatIpAddresses { get { return ConfigurationManager.AppSettings["ClientNatIpAddresses"]; } }
    public static string MsGates { get { return ConfigurationManager.AppSettings["MsGate"]; } }
    public static string TaxGates { get { return ConfigurationManager.AppSettings["TaxGate"]; } }
    public static string ParametersAllowDecimal
    {
        get
        {
            var data = ConfigurationManager.AppSettings["Parameters_Allow_Decimal"];
            if (data == null)
                return string.Empty;
            return data.ToString();
        }
    }
    public static string RiskReportShowOriginallyContractValues
    {
        get
        {
            var data = ConfigurationManager.AppSettings["RiskReportShowOriginallyContractValues"];
            if (data == null)
                return string.Empty;
            return data.ToString();
        }
    }
    public static int DefaultRiskPageSize
    {
        get
        {
            if (ConfigurationManager.AppSettings["DefaultRiskPageSize"] != null)
                return Convert.ToInt32(ConfigurationManager.AppSettings["DefaultRiskPageSize"]);
            return 500;
        }
    }

    public static string NPC_PCI_JumpUrl
    {
        get
        {
            string RemoteIp = RemoteIdAddress;
            string JumpUrl = string.Empty;
            if (RemoteIp.StartsWith(DDS_IP_Address)) JumpUrl = DDS_PCI_JumpUrl;
            else //if (DDSWebConfiguration.Client_IP_Address.Contains(RemoteIp))
            {
                string[] ClientIps = Client_IP_Address.Split(new string[] { CONFIG_SEPERATOR }, StringSplitOptions.RemoveEmptyEntries);
                string[] ClientUrls = Client_PCI_JumpUrl.Split(new string[] { CONFIG_SEPERATOR }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ClientIps.Length; i++)
                {
                    //if (ClientIps[i] == RemoteIp)
                    if (RemoteIp.StartsWith(ClientIps[i]))
                    {
                        JumpUrl = ClientUrls[i];
                        break;
                    }
                }

                if (JumpUrl == string.Empty) JumpUrl = NonDDS_PCI_JumpUrl;
            }

            return JumpUrl;
        }
    }

    /// <summary>
    /// Get PCI reskin site url - the logic referred to WebSiteSettings.NPC_PCI_JumpUrl
    /// </summary>
    /// <returns></returns>
    public static string PCIReskin_SiteJumpUrl
    {
        get
        {
            string remoteIp = WebSiteSettings.RemoteIdAddress;

            string ddsPCIUrl = ConfigurationManager.AppSettings["Reskin_DDS_PCI_JumpUrl"];
            if (remoteIp.StartsWith(WebSiteSettings.DDS_IP_Address))
            {
                return ddsPCIUrl;
            }

            string clientUrls = ConfigurationManager.AppSettings["Reskin_Client_PCI_JumpUrl"];
            string noneDDSPCIUrl = ConfigurationManager.AppSettings["Reskin_NonDDS_PCI_JumpUrl"];

            if (!string.IsNullOrEmpty(WebSiteSettings.Client_IP_Address) && !string.IsNullOrEmpty(WebSiteSettings.Client_PCI_JumpUrl))
            {
                var ips = WebSiteSettings.Client_IP_Address.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var urls = WebSiteSettings.Client_PCI_JumpUrl.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < ips.Length; i++)
                {
                    if (remoteIp.StartsWith(ips[i]) && i <= (urls.Length - 1))
                    {
                        return urls[i];
                    }
                }
            }

            return noneDDSPCIUrl;
        }
    }

    private static string RemoteIdAddress
    {
        get
        {
            string clientIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(clientIPAddress)) clientIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return clientIPAddress;
        }
    }

    public static string DocServicesUrl
    {
        get
        {
            return ConfigurationManager.AppSettings["DocServer_WS_URL"];
        }
    }

    public static string DocServicesDownloadUrl
    {
        get
        {
            return ConfigurationManager.AppSettings["DocServer_DownloadUrl"];
        }
    }

    public static string CurrentDomain
    {
        get
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host.Trim('/');
        }
    }
    public static string CurrentDomainAndAppPath
    {
        get
        {
            string domainAndPath = WebSiteSettings.CurrentDomain;

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ApplicationPath)
               && !string.IsNullOrEmpty(HttpContext.Current.Request.ApplicationPath.Trim('/')))
            {
                domainAndPath = domainAndPath + "/" + HttpContext.Current.Request.ApplicationPath.Trim('/');
            }
            return domainAndPath;
        }
    }

    //
    public static string SocialMediaGateUrl
    {
        get
        {
            string gateUrl = ConfigurationManager.AppSettings["SocialMediaGateUrl"].Trim().Trim('/');
            if (!gateUrl.StartsWith("http"))
            {
                gateUrl = WebSiteSettings.CurrentDomainAndAppPath + "/" + gateUrl.Trim('/');
            }
            return gateUrl;
        }
    }

    public static string CaseMgmtGateUrl
    {
        get
        {
            string gateUrl = ConfigurationManager.AppSettings["CaseGate"].Trim().Trim('/');
            if (!gateUrl.StartsWith("http"))
            {
                gateUrl = WebSiteSettings.CurrentDomainAndAppPath + "/" + gateUrl.Trim('/');
            }
            return gateUrl;
        }
    }
    public static string CaseMgmtDomain
    {
        get
        {
            string cmDomain = ConfigurationManager.AppSettings["CMDomain"].Trim().Trim('/');
            if (string.IsNullOrEmpty(cmDomain))
            {
                cmDomain = WebSiteSettings.CurrentDomain;
            }
            return cmDomain;
        }
    }
    public static PasswordValidationRule PwdValidationRule
    {
        get
        {
            if (HttpContext.Current.Session["PwdValidationRule"] == null)
            {

                FilterParameterCollection _parames = new FilterParameterCollection();
                _parames.AddLanguageID();
                _parames.Add(new FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int32));
                //_parames.AddLanguageID();
                PasswordValidationRule pwdrule = new PasswordValidationRule(WebServices.SecurityServices.GetReports("spa_GetPasswordSecurityRule", _parames));
                HttpContext.Current.Session["PwdValidationRule"] = pwdrule;
                return pwdrule;
            }
            else
            {
                return (PasswordValidationRule)HttpContext.Current.Session["PwdValidationRule"];
            }
        }
        set
        {
            HttpContext.Current.Session["PwdValidationRule"] = value;
        }
    }

    public static int NextQueue_ErrorTryGetLimited
    {
        get
        {
            return ConfigurationManager.AppSettings["NextQueue_ErrorTryGetLimited"] == null ? 1 : int.Parse(ConfigurationManager.AppSettings["NextQueue_ErrorTryGetLimited"].ToString());
        }
    }

    public static string RiskMCFKeepAlive
    {
        get
        {
            return ConfigurationManager.AppSettings["RiskMCFKeepAlive"] == null ? "3000" : ConfigurationManager.AppSettings["RiskMCFKeepAlive"].ToString();
        }
    }

    public static string RiskParametersViolation
    {
        get
        {
            return ConfigurationManager.AppSettings["RiskParametersViolation"] == null ? null : ConfigurationManager.AppSettings["RiskParametersViolation"].ToString();
        }
    }

    public static string SYSTEM_USER
    {
        get
        {
            return ConfigurationManager.AppSettings["SYSTEM_USER"] == null ? GlobalConstants.SYSTEM_USER_DEFAULT : ConfigurationManager.AppSettings["SYSTEM_USER"].ToString();
        }
    }
    public static string PCI_SSO_URL
    {
        get
        {
            return ConfigurationManager.AppSettings["PCI_SSO_URL"] == null ? null : ConfigurationManager.AppSettings["PCI_SSO_URL"].ToString();
        }
    }
    public static string PCI_Certificate_URL
    {
        get
        {
            return ConfigurationManager.AppSettings["PCI_Certificate_URL"] == null ? null : ConfigurationManager.AppSettings["PCI_Certificate_URL"];
        }
    }
    public static string PCI_Certificate_Pass
    {
        get
        {
            return ConfigurationManager.AppSettings["PCI_Certificate_Pass"] == null ? null : ConfigurationManager.AppSettings["PCI_Certificate_Pass"];
        }
    }
    public static string PCI_WS_URL
    {
        get
        {
            return ConfigurationManager.AppSettings["PCI_WS_URL"] ?? null;
        }
    }
    public static string PCI_WS_TOKEN_1
    {
        get
        {
            return ConfigurationManager.AppSettings["PCI_WS_Token1"] ?? null;
        }
    }
    public static string PCI_WS_TOKEN_2
    {
        get
        {
            return ConfigurationManager.AppSettings["PCI_WS_Token2"] ?? null;
        }
    }
    public static bool PCI_WS_SECURITY_PROTOCOL
    {
        get
        {
            return ConfigurationManager.AppSettings["PCI_WS_Security_Protocol"] != null && ConfigurationManager.AppSettings["PCI_WS_Security_Protocol"].Equals("true", StringComparison.OrdinalIgnoreCase);
        }
    }
    public static string PCI_ENVIROMENT
    {
        get
        {
            return ConfigurationManager.AppSettings["PCI_Enviroment"] ?? null;
        }
    }
    public static int PCIApplicationId
    {
        get
        {
            return int.TryParse(ConfigurationManager.AppSettings["PCI_ApplicationId"], out int id) ? id : 0;
        }
    }
    public static WebAppConfig GetWebAppConfig()
    {
        var configFile = "App_Data/AppConfig/AppConfig.json";
        var config = GeneralFuncsLib.ReadJsonConfig<WebAppConfig>(configFile);
        return config;
    }

    public static bool GetBoolConfig(string key, bool def)
    {
        try
        {
            string v = ConfigurationManager.AppSettings[key];
            return !string.IsNullOrEmpty(v) && v.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
        catch { return def; }
    }

    public static int GetIntConfig(string key, int def)
    {
        try
        {
            string v = ConfigurationManager.AppSettings[key];
            int n; return int.TryParse(v, out n) ? n : def;
        }
        catch { return def; }
    }

}
[Serializable]
public class Validation
{
    public string Key { get; set; }
    public int Value { get; set; }
    public string Msg { get; set; }
    public Validation()
    {
        Value = -1;
    }

}
[Serializable]
public class PasswordValidationRule
{
    List<Validation> lst = new List<Validation> { };
    public PasswordValidationRule() { }
    public PasswordValidationRule(DataTable tb)
    {
        if (tb.Columns.Contains("Key") && tb.Columns.Contains("Value") && tb.Columns.Contains("Msg"))
        {
            foreach (DataRow row in tb.Rows)
            {
                Validation v = new Validation();
                v.Key = row["Key"].ToString();
                v.Msg = row["Msg"].ToString();
                v.Value = row["Value"] == DBNull.Value ? -1 : int.Parse(row["Value"].ToString());
                lst.Add(v);
            }
        }
    }
    public void Add(string key, int value, string msg)
    {
        Validation v = new Validation();
        v.Key = key;
        v.Msg = msg;
        v.Value = value;
        lst.Add(v);
    }
    public void Add(DataTable tb)
    {
        if (tb.Columns.Contains("Key") && tb.Columns.Contains("Value") && tb.Columns.Contains("Msg"))
        {
            foreach (DataRow row in tb.Rows)
            {
                Validation v = new Validation();
                v.Key = row["Key"].ToString();
                v.Msg = row["Msg"].ToString();
                v.Value = row["Value"] != DBNull.Value ? 0 : int.Parse(row["Value"].ToString());
                lst.Add(v);
            }
        }
    }
    public Validation Get(string key)
    {
        Validation validation = new Validation();
        foreach (Validation v in lst)
        {
            if (v.Key.ToLower() == key.ToLower())
            {
                validation = v;
                break;
            }
        }
        return validation;
    }
}