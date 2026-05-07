using AS.Common.DBManager;
using AS.WS.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.DownloadDocuments
{
    public class TaskManager 
    {
        protected ReportingBusiness _ReportingBusiness = null;
        private int _SqlCommandTimeout = 0;
        public TaskManager()
        {
            if (Int32.TryParse(ConfigurationManager.AppSettings["SqlCommandTimeout"], out _SqlCommandTimeout))
            {
                ASSqlDatabase.CommandTimeout = _SqlCommandTimeout;
            }

            _ReportingBusiness = new ReportingBusiness();
            try
            {
                _ReportingBusiness.InitializeForCS(ConfigurationManager.ConnectionStrings["DBCONN"].ConnectionString);
            }
            catch
            {
                AS.Common.Logger.LoggerManager.Debug("Get ConnectionStrings:Failed. \n");
                AS.Common.Logger.LoggerManager.Debug("Maybe name file config exists(*.dll.config). You should change to (*.config). \n");
            }
        }

        public DataTable GetDocumentList(string asClientId)
        {
            FilterParameterCollection param = new FilterParameterCollection();
            param.Add(new FilterParameter("@ASClientID", asClientId, DbType.Int32));
            return _ReportingBusiness.GetReports("spp_Addhoc_DocumentExportNotes", param);
        }
    }
}
