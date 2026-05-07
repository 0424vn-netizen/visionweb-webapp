using AS.Web.Business.PCI;
using AS.Web.Business.Shared.Constants;
using System.Collections.Generic;

/// <summary>
/// Summary description for PciWebServices
/// </summary>
public class PciWebServices
{
    private static PciServices _pciReportServices = null;
    public static PciServices PciReportServices
    {
        get
        {
            string asClientId = GeneralFuncsLib.GetCurrentASClientId();
            if (_pciReportServices == null)
            {
                _pciReportServices = new PciServices(WebSiteSettings.PCI_WS_URL, WebSiteSettings.PCI_WS_TOKEN_1, WebSiteSettings.PCI_WS_TOKEN_2, WebSiteSettings.PCI_WS_SECURITY_PROTOCOL);
                _pciReportServices.AddRequestHeaders(new Dictionary<string, string>
                {
                    { PciConstants.RequestHeader_ASEnvironment, WebSiteSettings.PCI_ENVIROMENT },
                    { PciConstants.RequestHeader_ASClientID, asClientId }
                });
                return _pciReportServices;
            }
            _pciReportServices.InsertOrUpdateRequestHeader(PciConstants.RequestHeader_ASClientID, asClientId);
            return _pciReportServices;
        }
    }
}