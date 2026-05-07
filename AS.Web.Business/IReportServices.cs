using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Web.Business
{
    public interface IReportServices
    {
        DataTable GetReports(string spName, FilterParameterCollection _parameters);

        int ExecuteNonQueryCommand(string spName, FilterParameterCollection _parameters, out FilterParameterCollection OutputParams);
    }
}
