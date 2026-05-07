using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Data;
using System.Xml;
using System.Web.Caching;
using AS.WS.Business;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

/// <summary>
/// Summary description for GeneralFuncsLib
/// </summary>
public static class GeneralFuncsLib
{
    #region Connection String
    /// <summary>
    /// Get extended settings list by client
    /// </summary>
    /// <param name="xmlFilePath"></param>
    /// <returns></returns>
    public static string GetConnStringSettings(string clientID, string connName)
    {
        XmlDocument xmlDoc = new XmlDocument();
        if (System.Web.HttpRuntime.Cache["ConnStringSettings"] == null)
        {
            xmlDoc.Load(HttpContext.Current.Server.MapPath("~/App_Data/ConnStringSettings.xml"));
            System.Web.HttpRuntime.Cache.Insert("ConnStringSettings", xmlDoc, new System.Web.Caching.CacheDependency(HttpContext.Current.Server.MapPath("~/App_Data/ConnStringSettings.xml")));
        }
        else
        {
            xmlDoc = (XmlDocument)System.Web.HttpRuntime.Cache["ConnStringSettings"];
        }
        XmlNode node = xmlDoc.SelectSingleNode("//ConnectionSettings/Client[@id='" + clientID + "']/ConnectionSetting[@name='" + connName + "']");
        if (node != null) return node.Attributes["connectionString"].Value;
        return null;
    }

    /// <summary>
    /// Get global resource text for reports
    /// </summary>
    /// <param name="resourceKey"></param>
    /// <param name="cultureName"></param>
    /// <returns></returns>
    public static string GetReportResource(string resourceKey, string cultureName = null)
    {
        if (string.IsNullOrEmpty(cultureName))
        {
            return Resources.Reporting.ResourceManager.GetString(resourceKey);
        }
        return Resources.Reporting.ResourceManager.GetString(resourceKey, CultureInfo.CreateSpecificCulture(cultureName));
    }

    /// <summary>
    /// Load all report resources
    /// </summary>
    /// <param name="cultureName"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetAllReportResources(string cultureName = null)
    {
        string cacheKey = "ReportResources" + cultureName ?? string.Empty;
        //if (System.Web.HttpRuntime.Cache[cacheKey] != null)
        //{
        //    return (Dictionary<string, string>)System.Web.HttpRuntime.Cache[cacheKey];
        //}

        if (string.IsNullOrEmpty(cultureName))
        {
            cultureName = CultureInfo.CurrentCulture.Name;
        }

        //Get all resource keys
        var keys =
            typeof(ReportResourceKeys).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly).Select(fi => fi.GetRawConstantValue().ToString()).ToList();

        var result = keys.ToDictionary(key => key, key => GetReportResource(key, cultureName));
        System.Web.HttpRuntime.Cache.Insert(cacheKey, result);

        return result;
    }

    public static DataTable ExtendedClientSettings()
    {
        DataTable extendedSettings = GetDataByXML();
        return extendedSettings;
    }

    public static DataTable GetDataByXML()
    {
        DataTable data = new DataTable();
        var xmlFilePath = HttpContext.Current.Server.MapPath("~/App_Data/ClientExtendedSettings.xml");
        if (System.IO.File.Exists(xmlFilePath))
        {
            if (HttpRuntime.Cache[xmlFilePath] == null)
            {
                DataSet serversList = new DataSet();
                serversList.ReadXml(xmlFilePath);
                HttpRuntime.Cache.Add(xmlFilePath, serversList.Tables[0], new CacheDependency(xmlFilePath), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            data = HttpRuntime.Cache[xmlFilePath] as DataTable;
        }

        return data;
    }
    public static string GetKeyPathForClient(int clientId, string path)
    {
        if (string.IsNullOrEmpty(clientId.ToString())) return "";
        XmlDocument doc = new XmlDocument
        {
            XmlResolver = null
        };
        try
        {
            doc.Load(path);
        }
        catch (Exception)
        {
            AS.Common.Logger.LoggerManager.Error("Not found KeyConfig.xml, location: " + path);
        }
        XmlNodeList nodes = doc.SelectNodes("/Clients/client");
        if (nodes.Count == 0) return "";
        for (int i = 0; i < nodes.Count; i++)
        {
            string nodeClientID = nodes[i].Attributes["id"].Value;
            if (nodeClientID.Equals(clientId.ToString()))
            {
                return nodes[i].Attributes["keyPath"].Value;
            }
        }

        return "";
    }
    #endregion
}
