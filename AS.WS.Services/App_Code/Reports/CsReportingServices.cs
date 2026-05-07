using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Data;
using System.Configuration;

using AS.Common.Logger;
using AS.Common.DBManager;
using AS.WS.Business;

/// <summary>
/// Summary description for fdc_ReportingServices
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[Microsoft.Web.Services3.Policy("ServerPolicy")]

public class CsReportingServices : BaseService
{

    public CsReportingServices()
        : base()
    {
        
        _ReportingBusiness.InitializeForCS(GeneralFuncsLib.GetConnStringSettings(RequestHeaders["ClientId"], "CS_DBCONN"));
    }

}

