using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web;
using System.Web.Caching;

public enum SitemapSettingKeys
{
    RemoveDuplicateSitemap,
}

public class Sitemap
{
    //Direct parent and nearest
    public string Parent { get; set; }
    public string Title { get; set; }
}
/// <summary>
/// Summary description for SitemapHandler
/// </summary>
public class SitemapHandler
{
    public const string SITEMAP_CONFIG_PATH = "~/App_Data/SitemapSetting.xml";
    public static List<Sitemap> LoadSitemapConfig(SitemapSettingKeys sitemapKey)
    {
        XmlDocument doc = new XmlDocument();
        HttpContext context = HttpContext.Current;
        doc.Load(context.Server.MapPath(SITEMAP_CONFIG_PATH));
        XmlNodeList nodes = null;
        List<Sitemap> sitemaps = new List<Sitemap>();

        nodes = doc.SelectNodes("//sitemapsettings/sitemaps[@key='" + sitemapKey + "']/sitemap");
        if (nodes.Count == 0) return null;
        for (int i = 0; i < nodes.Count; i++)
        {
            sitemaps.Add(new Sitemap()
            {
                Parent = nodes[i].Attributes["parent"].Value,
                Title = nodes[i].Attributes["title"].Value
            });
        }
        return sitemaps;
    }

    public static bool IsRemoveFromSitemaps(string parent, string title)
    {
        title = title.Trim();
        parent = parent.Trim();

        List<Sitemap> sitemaps = new List<Sitemap>();
        if (HttpRuntime.Cache["SitemapSettings"] == null)
        {
            sitemaps = LoadSitemapConfig(SitemapSettingKeys.RemoveDuplicateSitemap);
            if (sitemaps.IsNotNullData())
            {
                HttpRuntime.Cache.Insert("SitemapSettings", sitemaps,
                    new CacheDependency(HttpContext.Current.Server.MapPath(SITEMAP_CONFIG_PATH)));
            }
        }

        sitemaps = (List<Sitemap>)HttpRuntime.Cache["SitemapSettings"];
        var sitemapByTitles = sitemaps.Where(m => m.Title.ToLower() == title.ToLower() && (m.Parent.ToLower() == parent.ToLower() || m.Parent.IsNullOrEmpty()));
        if (sitemapByTitles != null && sitemapByTitles.Count() > 0)
            return true;
        return false;
    }

}