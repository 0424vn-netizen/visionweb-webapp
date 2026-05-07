using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;

/// <summary>
/// Mobile redirect handler
/// </summary>
public class MobileRedirectHandler
{
    private const string MobileRedirectConfigPath = "~/App_Data/MobileRedirectSettings.xml";
    private const string MobileRedirectConfigCacheKey = "MobileRedirectConfigs";
    private const string MobileRedirectFromSiteKey = "_fs";
    private const string KeepFullSiteKey = "_kfs";
    private const string MobileRedirectFromSiteValue = "mb";

    public static string MobileRedirectConfigFullPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(MobileRedirectConfigPath);
        }
    }

    /// <summary>
    /// Get mobile redirect settings
    /// </summary>
    /// <returns></returns>
    private static MobileRedirectSetting GetMobileRedirectSettings()
    {
        if (HttpRuntime.Cache[MobileRedirectConfigCacheKey] == null)
        {
            MobileRedirectSetting config;
            var deserializer = new XmlSerializer(typeof(MobileRedirectSetting));
            using (var reader = new StreamReader(MobileRedirectConfigFullPath))
            {
                var obj = deserializer.Deserialize(reader);
                config = obj as MobileRedirectSetting;
            }
            HttpRuntime.Cache.Insert(MobileRedirectConfigCacheKey, config, new CacheDependency(MobileRedirectConfigFullPath));
        }
        return HttpRuntime.Cache[MobileRedirectConfigCacheKey] as MobileRedirectSetting;
    }

    /// <summary>
    /// Process mobile direct
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="systemId"></param>
    public static void ProcessMobileRedirect(int clientId)
    {
        if (CheckMobileRedirect(clientId, WebSiteSettings.DefaultSystem))
        {
            HttpContext.Current.Response.Redirect(GetMobileRedirectUrl(clientId));
        }
    }

    /// <summary>
    /// Process mobile direct for SSO
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="systemId"></param>
    public static void ProcessMobileRedirectForSSO(int clientId, string ssoUserId)
    {
        if (CheckMobileRedirect(clientId, WebSiteSettings.DefaultSystem, ssoUserId))
        {
            HttpContext.Current.Response.Redirect(GetMobileRedirectUrlForSSO(clientId, ssoUserId));
        }
    }

    /// <summary>
    /// Check if request can be redirected to mobile site
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="systemId"></param>
    /// <returns></returns>
    public static bool CheckMobileRedirect(int clientId, int systemId, string ssoUserId = "")
    {
        var httpContext = HttpContext.Current;
        if (httpContext == null) return false;

        //Force to work on fullsite
        if (httpContext.Request[MobileRedirectFromSiteKey] != null &&
            httpContext.Request[MobileRedirectFromSiteKey].Equals(MobileRedirectFromSiteValue))
        {
            HttpCookie cookie = new HttpCookie(KeepFullSiteKey, AS.Common.DataProtection.Cryptophy.EncryptText("1"));
            httpContext.Response.Cookies.Add(cookie);
            return false;
        }
        if (httpContext.Request.Cookies[KeepFullSiteKey] != null &&
            httpContext.Request.Cookies[KeepFullSiteKey].Value == AS.Common.DataProtection.Cryptophy.EncryptText("1"))
        {
            return false;
        }

        // Check user which has permission access into the mobile site
        if (!string.IsNullOrEmpty(ssoUserId))
        {
            AS.Security.Web.SecurityServices.SecService.MobileUser mobileUser = WebServices.SecurityServices.GetMobileUser(clientId, ssoUserId);
            if (!mobileUser.HasMobileAccess)
            {
                return false;
            }
        }


        if (!File.Exists(MobileRedirectConfigFullPath))
        {
            //AS.Common.Logger.LoggerManager.Debug("Mobile redirect setting file does not exist!");
            return false;
        }

        var configs = GetMobileRedirectSettings();
        if (configs == null)
        {
            //AS.Common.Logger.LoggerManager.Debug("Mobile redirect settings not loaded!");
            return false;
        }

        //Check user can access mobile site
        //SystemId: 1(CS), 2(MS)
        //ClientUrlCollection
        //RedirectCollection
        if (configs.SystemApplied != systemId ||
            configs.ClientUrlCollection == null ||
            !configs.ClientUrlCollection.ContainsClient(clientId) ||
            configs.RedirectCollection == null ||
            !configs.RedirectCollection.HasValues)
        {
            //AS.Common.Logger.LoggerManager.Debug(string.Format("Mobile redirect is not allowed! ClientId : {0}", clientId));
            if (configs.ClientUrlCollection != null)
            {
                var temp = string.Join(" | ", configs.ClientUrlCollection.Items.Select(i => string.Format("{0}-{1}-{2}", i.ClientId, i.Name, i.Alias)).ToArray());
                //AS.Common.Logger.LoggerManager.Debug(string.Format("Client Url Collection: {0}", temp));
            }

            return false;
        }

        string httpUserAgent = httpContext.Request.UserAgent;
        foreach (var item in configs.RedirectCollection.Items)
        {
            var mobileRegex = new Regex(item.Agent, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
            if (mobileRegex.IsMatch(httpUserAgent))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Get mobile url for mobile redirect
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    public static string GetMobileRedirectUrl(int clientId)
    {
        var configs = GetMobileRedirectSettings();
        if (configs == null) return string.Empty;
        var clientUrl = configs.ClientUrlCollection[clientId];

        return string.Format("{0}/{1}", configs.BaseMobileUrl.TrimEnd('/'), clientUrl.Alias);
    }

    /// <summary>
    /// Get mobile url for mobile redirect
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static string GetMobileRedirectUrlForSSO(int clientId, string userId)
    {
        var configs = GetMobileRedirectSettings();
        if (configs == null) return string.Empty;
        var clientUrl = configs.ClientUrlCollection[clientId];
        var ssoQueryString = string.Format("c={0}&u={1}&t={2}", clientId, userId, DateTime.UtcNow.Ticks);

        var e = WebServices.SecurityServices.EncryptText(ssoQueryString);
        var d = WebServices.SecurityServices.DecryptText(e);

        return string.Format("{0}/{1}?sso={2}",
            configs.BaseMobileUrl.TrimEnd('/'),
            clientUrl.Alias,
            HttpUtility.UrlEncode(WebServices.SecurityServices.EncryptText(ssoQueryString)));
    }
}

[Serializable]
[XmlRoot("MobileRedirectSetting")]
public class MobileRedirectSetting
{
    [XmlAttribute("baseMobileUrl")]
    public string BaseMobileUrl { get; set; }

    [XmlAttribute("systemApplied")]
    public int SystemApplied { get; set; }

    [XmlElement("Redirects")]
    public MobileRedirectCollection RedirectCollection { get; set; }

    [XmlElement("Urls")]
    public MobileRedirectUrlCollection ClientUrlCollection { get; set; }
}

[Serializable]
public class MobileRedirectItem
{
    [XmlAttribute("agent")]
    public string Agent { get; set; }

    [XmlAttribute("active")]
    public bool Active { get; set; }
}

[Serializable]
public class MobileRedirectUrl
{
    [XmlAttribute("clientId")]
    public int ClientId { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("alias")]
    public string Alias { get; set; }
}

[Serializable]
public class MobileRedirectCollection
{
    [XmlElement("Redirect")]
    public List<MobileRedirectItem> Items { get; set; }

    public bool HasValues
    {
        get
        {
            return Items != null && Items.Any();
        }
    }
}

[Serializable]
public class MobileRedirectUrlCollection
{
    [XmlElement("Url")]
    public List<MobileRedirectUrl> Items { get; set; }

    public MobileRedirectUrl this[int clientId]
    {
        get
        {
            return Items.FirstOrDefault(u => u.ClientId == clientId);
        }
    }

    public bool HasValues
    {
        get
        {
            return Items != null && Items.Any();
        }
    }

    public bool ContainsClient(int clientId)
    {
        return HasValues ? Items.Any(i => i.ClientId == clientId) : false;
    }
}