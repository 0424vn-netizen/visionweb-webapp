using AS.VW.Audit.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Audit
{    
    public class DataTableOneLineAudit : IEntityAudit<DataTable>
    {
        private readonly string _rowFormat;
        private readonly Dictionary<string, string> _columnFormats;


        /// <summary>
        /// rowFormat base on index of propertyNames
        /// </summary>
        /// <param name="rowFormat"></param>
        /// <param name="columnFormats"></param>
        public DataTableOneLineAudit(string rowFormat = null, Dictionary<string, string> columnFormats = null)
        {
            _rowFormat = rowFormat;
            _columnFormats = columnFormats;
        }

        public List<AuditEntityModel> CompareEntities(DataTable oldTable, DataTable newTable, string primaryKey, string[] propertyNames)
        {
            var results = new List<AuditEntityModel>();

            var oldDict = new Dictionary<string, DataRow>();
            var newDict = new Dictionary<string, DataRow>();

            foreach (DataRow row in oldTable.Rows)
            {
                oldDict[row[primaryKey].ToString()] = row;
            }

            foreach (DataRow row in newTable.Rows)
            {
                newDict[row[primaryKey].ToString()] = row;
            }

            foreach (var newEntry in newDict)
            {
                if (oldDict.ContainsKey(newEntry.Key))
                {
                    var oldRow = oldDict[newEntry.Key];
                    var newRow = newEntry.Value;
                    var comparisonResults = CompareDataRows(oldRow, newRow, primaryKey, propertyNames);
                    results.AddRange(comparisonResults);
                }
                else
                {
                    var newValueList = propertyNames.Select(col => FormatValue(newEntry.Value[col], col) ?? "null").ToList();

                    results.Add(new AuditEntityModel
                    {
                        Id = newEntry.Key,
                        AuditAction = AuditAction.Added,
                        OldValues = "",
                        NewValues = string.Format(_rowFormat, newValueList.ToArray())
                    });
                }
            }

            foreach (var oldEntry in oldDict)
            {
                if (!newDict.ContainsKey(oldEntry.Key))
                {
                    var oldValueList = propertyNames.Select(col => FormatValue(oldEntry.Value[col], col) ?? "null").ToList();

                    results.Add(new AuditEntityModel
                    {
                        Id = oldEntry.Key,
                        AuditAction = AuditAction.Deleted,
                        OldValues = string.Format(_rowFormat, oldValueList.ToArray()),
                        NewValues = ""
                    });
                }
            }

            return results;
        }

        private List<AuditEntityModel> CompareDataRows(DataRow oldRow, DataRow newRow, string primaryKey, string[] columnNames)
        {
            var results = new List<AuditEntityModel>();

            var oldValueList = new List<string>();
            var newValueList = new List<string>();
            bool updated = false;

            foreach (var colName in columnNames)
            {
                var oldValue = FormatValue(oldRow[colName], colName);
                var newValue = FormatValue(newRow[colName], colName);

                if (!oldValue.Equals(newValue))
                {
                    updated = true;
                }

                oldValueList.Add(oldValue);
                newValueList.Add(newValue);
            }

            if (updated)
            {
                results.Add(new AuditEntityModel
                {
                    Id = oldRow[primaryKey].ToString(),
                    AuditAction = AuditAction.Updated,
                    OldValues = string.Format(_rowFormat, oldValueList.ToArray()),
                    NewValues = string.Format(_rowFormat, newValueList.ToArray())
                });
            }

            return results;
        }

        private string FormatValue(object value, string propertyName)
        {
            if (value == null)
                return "null";

            if (_columnFormats != null && _columnFormats.ContainsKey(propertyName))
            {
                if (value is DateTime dateValue)
                {
                    return dateValue.ToString(_columnFormats[propertyName]);
                }
                return string.Format(_columnFormats[propertyName], value);
            }

            return value.ToString();
        }
    }
}
