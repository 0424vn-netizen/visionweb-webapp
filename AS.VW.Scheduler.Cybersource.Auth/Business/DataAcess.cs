using AS.Common.DBManager;
using AS.VW.Repository;
using AS.WS.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Cybersource.Auth.Business
{
    public class DataAcess
    {
        protected ReportingBusiness _ReportingBusiness = null;
        public DataAcess()
        {
            var timeout = AppConfigurations.GetIntAppSettings("SqlCommandTimeout", 0);

            if (timeout > 0)
            {
                ASSqlDatabase.CommandTimeout = timeout;
            }

            _ReportingBusiness = new ReportingBusiness();
            _ReportingBusiness.InitializeForCS(ConfigurationManager.ConnectionStrings["VisionWebDB"].ConnectionString);
        }
    }
}
