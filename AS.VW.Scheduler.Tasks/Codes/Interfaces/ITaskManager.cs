using AS.VW.Scheduler.Tasks.Codes.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace AS.VW.Scheduler.Tasks
{
    public interface ITaskManager
    {
        List<Processer> GetDataProcessInQueue();
        List<Processer> GetDataProcessInQueue(string mode);        
       
        ExportReportConfigResponse GetReportConfig(ExportReportConfigRequest reportConfigRequest);
        DataTable GetExtractReportAutoBindingParameters(Processer pro, bool isGetTotal);
        IDataReader GetExtractReportAutoBindingParameters(Processer pro, int pageSize, int pageNo);

        void UpdateStatus(Processer pro, string docId, ProcessStatus status, string fileType);
        void UpdateStatus(Processer pro, string docId, ProcessStatus status, string fileType, string mode);
        IDataReader GetExtractReport(Processer pro);
        IDataReader GetExtractReport(Processer pro, int pageSize, int pageNo);
        IDataReader GetExtractReport(Processer pro, DateTime reportDate);
        DataSet GetExtractReport(Processer pro, bool isGetTotal);
        void UpdateFileName(int logID, ProcessStatus status);
    }
}
