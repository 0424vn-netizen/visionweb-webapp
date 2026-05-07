using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AS.VW.Repository
{
    public static class WebServices
    {
        public static ReportServices ReportServices
        {
            get
            {
                ReportServices reportServices = new ReportServices(AppConfigurations.ReportWSURL, AppConfigurations.ReportWSToken1, AppConfigurations.ReportWSToken2);
                reportServices.AddRequestHeader("ClientId", AppConfigurations.ClientIdDefault);
                return reportServices;
            }
        }        

        public static DocServices DocServices
        {
            get
            {
                DocServices docServices = new DocServices(AppConfigurations.DocServerURL, AppConfigurations.DocServerDowloadURL);
                return docServices;
            }
        }
       
    }
}
