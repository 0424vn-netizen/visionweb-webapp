using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AS.VW.Scheduler.G2ML
{
    class DBExecute
    {
        public DBConnection db = new DBConnection();
        public DataTable GetDataG2(string SpaName,int DateFilterMode,  DateTime ReportDate)
        {
            SqlParameter[] _parames = new SqlParameter[2];
            _parames[0] = new SqlParameter("@DateFilterMode ", DateFilterMode);
            _parames[1] = new SqlParameter("@BeginDate ", ReportDate);
            DataTable data = db.Execute(SpaName, _parames);
            return data;
        }

        public void UpdateExtractReportLog(ExtractReportLogModel extractReportLogModel)
        {
            SqlParameter[] _parames = new SqlParameter[7];
            _parames[0] = new SqlParameter("@ASClient ", extractReportLogModel.ASClient);
            _parames[1] = new SqlParameter("@DateFrom ", extractReportLogModel.ReportDate);
            _parames[2] = new SqlParameter("@FileName ", extractReportLogModel.FileName);
            _parames[3] = new SqlParameter("@FileType ", extractReportLogModel.FileType);
            _parames[4] = new SqlParameter("@FilePath ", extractReportLogModel.FilePath);
            _parames[5] = new SqlParameter("@Status ", extractReportLogModel.Status);
            _parames[6] = new SqlParameter("@NumberOfMatches ", extractReportLogModel.NumberOfMatches);
            db.ExecuteNonQuery(extractReportLogModel.SpaName, _parames);
        }
    }
}
