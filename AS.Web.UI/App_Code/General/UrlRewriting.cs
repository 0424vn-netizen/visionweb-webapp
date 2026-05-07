using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Xml;

/// <summary>
/// Summary description for UrlRewriting
/// </summary>
/// 
namespace AS.Web
{
    public class UrlRewriteConfig
    {
        public static UrlRewriteConfig[] LoadUrlConfig(string configFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(configFile);
            XmlNodeList nodes = null;

            nodes = doc.SelectNodes("//configs/page");
            if (nodes.Count == 0) return null;
            UrlRewriteConfig[] ret = new UrlRewriteConfig[nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                ret[i] = new UrlRewriteConfig();
                ret[i].RequestUrl = nodes[i].Attributes["request"].Value;
                ret[i].TransferUrl = nodes[i].Attributes["transferTo"].Value;
            }
            return ret;
        }

        public UrlRewriteConfig()
        {

        }
        string _request = "";
        public string RequestUrl
        {
            get
            {
                return _request;
            }
            set
            {
                _request = value;

            }
        }
        string _transferUrl = "";

        public string TransferUrl
        {
            get
            {
                return _transferUrl;
            }
            set
            {
                _transferUrl = value;

            }
        }
    }
    public class UrlRewriting : IHttpModule
    {

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new System.EventHandler(context_BeginRequest);
            context.PreRequestHandlerExecute += new System.EventHandler(context_PreRequestHandlerExecute);
        }

        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            if (app.Context.CurrentHandler is Page && app.Context.CurrentHandler != null)
            {
                Page pg = (Page)app.Context.CurrentHandler;
                pg.PreRenderComplete += new System.EventHandler(pg_PreRenderComplete);
            }
        }

        void pg_PreRenderComplete(object sender, EventArgs e)
        {
            if (HttpContext.Current.Items.Contains("_OrgUrl"))
            {
                string path = (string)HttpContext.Current.Items["_OrgUrl"];
                HttpContext.Current.RewritePath(path, "", HttpContext.Current.Items["_OrgUrlQuery"].ToString(), true);
            }
        }


        void context_BeginRequest(object sender, EventArgs e)
        {
            UrlRewriteConfig[] config = null;
            HttpContext context = HttpContext.Current;
            if (!context.Request.IsAuthenticated)
            {
                if (System.Web.HttpRuntime.Cache["LoginPageConfigs"] == null)
                {
                    string configFile = context.Server.MapPath("~/App_Data/LoginPageConfigs.xml");
                    config = UrlRewriteConfig.LoadUrlConfig(configFile);
                    System.Web.HttpRuntime.Cache.Insert("LoginPageConfigs", config, new System.Web.Caching.CacheDependency(configFile));
                }
                config = (UrlRewriteConfig[])System.Web.HttpRuntime.Cache["LoginPageConfigs"];
                if (config != null)
                {

                    for (int i = 0; i < config.Length; i++)
                    {
                        string configRequestUrl = config[i].RequestUrl.ToLower();
                        if (configRequestUrl.Contains("~"))
                        {
                            configRequestUrl = VirtualPathUtility.ToAbsolute(configRequestUrl).ToLower();
                            try
                            {
                                if (context.Request.QueryString.Count > 0)
                                    configRequestUrl += "?" + context.Request.QueryString;
                            }
                            catch (Exception ex)
                            {
                                AS.Common.Logger.LoggerManager.Error(string.Format("Request={0}; {2}{1}{2}", context.Request.Url, ex.Message, Environment.NewLine));
                            }
                            if (HttpUtility.UrlDecode(context.Request.RawUrl.ToLower()) == HttpUtility.UrlDecode(configRequestUrl.ToLower()))
                            {
                                string transferUrl = config[i].TransferUrl;
                                string paramUrl = context.Request.QueryString.Count == 0 ? string.Empty : context.Request.QueryString.ToString();
                                if (!string.IsNullOrEmpty(paramUrl))
                                    transferUrl += "&" + paramUrl;
                                context.Items.Add("_OrgUrl", context.Request.Url.AbsolutePath);//save for PreRenderComplete
                                context.Items.Add("_OrgUrlQuery", paramUrl);//save for PreRenderComplete
                                context.RewritePath(transferUrl);
                                break;
                            }
                        }
                        else
                        {
                            //[TIB] - [55527] - Account Lock Out Email Functionality and Broken Link - Custom domain
                            //string requestUrl = context.Request.Url.Host + context.Request.Url.PathAndQuery;
                            string requestUrl = context.Request.Url.Host + context.Request.Url.AbsolutePath; 
                            if (requestUrl.ToLower() == HttpUtility.UrlDecode(configRequestUrl.ToLower()))
                            {
                                string transferUrl = config[i].TransferUrl;
                                string paramUrl = context.Request.QueryString.Count == 0 ? string.Empty : context.Request.QueryString.ToString();
                                if (!string.IsNullOrEmpty(paramUrl))
                                    transferUrl += "&" + paramUrl;
                                context.Items.Add("_OrgUrl", context.Request.Url.AbsolutePath);//save for PreRenderComplete
                                context.Items.Add("_OrgUrlQuery", paramUrl);//save for PreRenderComplete
                                context.RewritePath(transferUrl);
                                break;
                            }
                        }
                    }

                }
            }
        }
        public void Dispose()
        {
        }

    }
}