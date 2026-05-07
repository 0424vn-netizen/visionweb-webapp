using System;
using AS.Common.DBManager;
using System.Data;
namespace AS.WS.Data
{
    public interface IReportingDao
    {
        int ExecuteNonQueryCommand(string spName, FilterParameterCollection _parameters, out FilterParameterCollection OutputParams);

        DataSet ExecuteQueryCommand(string spName, FilterParameterCollection _parameters, out FilterParameterCollection OutputParams);
        DataTable GetReports(string spName, FilterParameterCollection _parameters);        
   
        DataSet GetReportsAsDataSet(string spName, FilterParameterCollection _parameters);
        IDataReader GetReportsAsDataReader(string spName, FilterParameterCollection _parameters);
        DataSet GetSecurityReportForDetectionQueue(string spaName, FilterParameterCollection _parameters);
    }
}
