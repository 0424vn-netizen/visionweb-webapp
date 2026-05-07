using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AS.VW.Scheduler.Tasks.Codes.Models;

namespace AS.VW.Scheduler.Tasks
{
    class TaskManager_Mock : ITaskManager
    {
        public List<Processer> GetDataProcessInQueue()
        {
            List<Processer> result = new List<Processer>();
            result.Add(new Processer { LogID = 1, ASClientID = 22, CreatedBy = "aaaa", FileName = "abc.csv" });
            result.Add(new Processer { LogID = 2, ASClientID = 22, CreatedBy = "bbb", FileName = "def.csv" });
            return result;
        }

        public void UpdateStatus(Processer pro, string docId, ProcessStatus status, string fileType)
        {
        }

        public IDataReader GetExtractReport(Processer param)
        {
            return null;
            //DataTable result = new DataTable();
            //result.Columns.Add("column1");
            //result.Columns.Add("column2");
            //for (int i = 0; i < 10; i++)
            //{
            //    result.Rows.Add(new object[] { "1", "2" });
            //    result.Rows.Add(new object[] { "3", "4" });
            //}
            //return result;
        }
        public void UpdateFileName(int LogID, ProcessStatus status)
        {
            //do nothing
        }

        public List<Processer> GetDataProcessInQueue(string mode)
        {
            throw new NotImplementedException();
        }

        public DataSet GetExtractReport(Processer pro, bool isGetTotal)
        {
            throw new NotImplementedException();
        }


        public DataTable GetExtractReport(Processer pro, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }


        public void UpdateStatus(Processer pro, string docId, ProcessStatus status, string fileType, string mode)
        {
            throw new NotImplementedException();
        }

        IDataReader ITaskManager.GetExtractReport(Processer pro, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetExtractReport(Processer pro, DateTime reportDate)
        {
            throw new NotImplementedException();
        }

        public List<ExportReportProcesser> GetExportReportProcessInQueue(string mode)
        {
            throw new NotImplementedException();
        }
    }
}
