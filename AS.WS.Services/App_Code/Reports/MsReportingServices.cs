using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

/// <summary>
/// Summary description for MsReportingServices
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class MsReportingServices : BaseService
{

    public MsReportingServices()
        : base()
    {
        // TODO retrieve
        _ReportingBusiness.InitializeForMS(GeneralFuncsLib.GetConnStringSettings(RequestHeaders["ClientId"], "MS_DBCONN"));
    }
}

