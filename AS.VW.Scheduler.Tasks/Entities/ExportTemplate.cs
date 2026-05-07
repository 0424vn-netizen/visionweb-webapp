using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Tasks.Entities
{
    public class ExportTemplate
    {
        public string Key { get; set; }
        public string Path { get; set; }
        public bool IsDefault { get; set; }
        public bool HasFilterSheet { get; set; }
        public bool IsCreateNewTemplate { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyType { get; set; }
        public List<DataSheet> Sheets { get; set; }
    }

    public class ColumnTemplate
    {
        public string DataKey { get; set; }
        public string Name { get; set; }
        public string ASFormat { get; set; }

        public string PartialColumn { get; set; }
    }

    public class DataSheet
    {
        public DataSheet()
        {
            Name = string.Empty;
            Columns = new List<ColumnTemplate>();
            MaxWidthColumns = new List<double>();
        }
        public string Name { get; set; }
        public List<ColumnTemplate> Columns { get; set; }
        public List<double> MaxWidthColumns { get; set; }
    }
}
