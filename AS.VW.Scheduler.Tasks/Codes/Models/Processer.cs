using AS.VW.Scheduler.Tasks.Codes.Models;
using System;

namespace AS.VW.Scheduler.Tasks
{
    public enum FileTypes
    {
        Csv = 1,
        Excel = 2        
    }

    public class Processer
    {
        public int LogID { get; set; }

        //New prop, but it is same as LogID, just for ExportReport.cs
        public int ProcessLogID { get; set; }
        public string CreatedBy { get; set; }
        public int ASClientID { get; set; }

        public string UserMode { get; set; }
        public string UserID { get; set; }
        public Int32 SiteID { get; set; }
        public Int32 LanguageID { get; set; }


        public string FileName { get; set; }
        public int ReportType { get; set; }
        public string SpaName { get; set; }
        public string TitleText { get; set; }
        public string ExtractMode { get; set; }

        public FileTypes FileType { get; set; }    
        public string CategoryCode { get; set; }
        public string SubCategoryCode { get; set; }
        public string SubCategoryDescription { get; set; }
        public ExportReportConfigResponse ReportConfig { get; set; } = new ExportReportConfigResponse();
    }
   
    public enum ProcessStatus
    {        
        InQueue = 0,
        Processing = 1,
        Success = 2,
        Fail = 3,
        Zipping = 4
    }

    #region ExportReports
    public class ReportConfig
    {
        public int ASClientID { get; set; }
        public string FilterContent { get; set; }
    }
    #endregion
}
