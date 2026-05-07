using AS.CaseMgt.API;
using AS.Security.Web.SecurityServices;
using AS.Web.Business;
using AS.Web.Business.PCI;
using AS.Web.Business.Shared.Constants;
using AS.Web.LogServices;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

/// <summary>
/// Summary description for WebServiceSettings
/// </summary>
public class WebServices
{
    public static LogService LogServices
    {
        get
        {
            LogService logServices = new LogService();
            logServices.AddRequestHeader("ClientId", SessionManager.CurrentClient.ToString());
            return logServices;
        }
    }

    public static SecurityService SecurityServices
    {
        get
        {
            SecurityService securityServices = new SecurityService();
            securityServices.AddRequestHeader("ClientId", SessionManager.CurrentClient.ToString());
            return securityServices;
        }
    }
    public static ReportServices CsReportServices
    {
        get
        {
            ReportServices csReportServices = new ReportServices(ConfigurationManager.AppSettings["CS_Report_WS_URL"], ConfigurationManager.AppSettings["CS_Report_WS_Token1"], ConfigurationManager.AppSettings["CS_Report_WS_Token2"]);
            if (HttpContext.Current != null) csReportServices.AddRequestHeader("ClientId", SessionManager.CurrentClient.ToString());
            return csReportServices;
        }
    }
    public static ReportServices CsReportServices_MPS
    {
        get
        {
            ReportServices csReportServices = new ReportServices(ConfigurationManager.AppSettings["CS_Report_WS_URL"], ConfigurationManager.AppSettings["CS_Report_WS_Token1"], ConfigurationManager.AppSettings["CS_Report_WS_Token2"]);
            if (HttpContext.Current != null) csReportServices.AddRequestHeader("ClientId", "23");
            return csReportServices;
        }
    }
    public static ReportServices MsReportServices
    {
        get
        {
            ReportServices services = new ReportServices(ConfigurationManager.AppSettings["MS_Report_WS_URL"], ConfigurationManager.AppSettings["MS_Report_WS_Token1"], ConfigurationManager.AppSettings["MS_Report_WS_Token2"]);
            if (HttpContext.Current != null) services.AddRequestHeader("ClientId", SessionManager.CurrentClient.ToString());
            return services;
        }
    }

    public static ReportServices RiskServices
    {
        get
        {
            ReportServices riskServices = new ReportServices(ConfigurationManager.AppSettings["RM_Report_WS_URL"], ConfigurationManager.AppSettings["RM_Report_WS_Token1"], ConfigurationManager.AppSettings["RM_Report_WS_Token2"]);
            if (HttpContext.Current != null) riskServices.AddRequestHeader("ClientId", SessionManager.CurrentClient.ToString());
            return riskServices;
        }
    }

    private static DocServices _DocServices = null;
    public static DocServices DocServices
    {
        get
        {
            if (_DocServices == null)
                _DocServices = new DocServices(WebSiteSettings.DocServicesUrl, WebSiteSettings.DocServicesDownloadUrl);
            return _DocServices;
        }
    }

    public static APIService ApiServices
    {
        get
        {
            APIService apiServices = new APIService(ConfigurationManager.AppSettings["CaseAPI_WS_Url"]);
            apiServices.ClientId = SessionManager.CurrentClient;
            return apiServices;
        }
    }

    public static AS.SocialMedia.API.APIService SMApiServices
    {
        get
        {
            AS.SocialMedia.API.APIService apiServices = new AS.SocialMedia.API.APIService(ConfigurationManager.AppSettings["SMAPI_WS_Url"]);
            apiServices.ClientId = SessionManager.CurrentClient;
            return apiServices;
        }
    }
}
