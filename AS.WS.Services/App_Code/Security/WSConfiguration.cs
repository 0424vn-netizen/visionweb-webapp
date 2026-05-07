using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for WSConfiguration
/// </summary>
public static class WSConfiguration
{

    public static int ASClient
    {
        get { return int.Parse(ConfigurationManager.AppSettings.Get("ASClient")); }
    }
    public static string DES_VI
    {
        get { return ConfigurationManager.AppSettings.Get("DES_VI"); }
    }
    public static string DES_Key
    {
        get { return ConfigurationManager.AppSettings.Get("DES_Key"); }
    }
}
