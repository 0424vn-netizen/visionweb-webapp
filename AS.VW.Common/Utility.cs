using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Common
{
    public static class Utility
    {
        public static T To<T>(this DataRow row)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (p.CanWrite)
                {
                    bool isExistedColumn = row.Table.Columns.Contains(p.Name);
                    DataToField<T>(row, obj, p, p.Name, isExistedColumn);
                }
            }

            return obj;
        }

        public static T To<T>(this DataRow row, Dictionary<string, string> mapNames)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (p.CanWrite)
                {
                    string colName = p.Name;
                    bool isExistedColumn = row.Table.Columns.Contains(colName);
                    if (!isExistedColumn && mapNames.ContainsKey(colName) && row.Table.Columns.Contains(mapNames[colName]))
                    {
                        isExistedColumn = true;
                        colName = mapNames[colName];
                    }
                    DataToField<T>(row, obj, p, colName, isExistedColumn);
                }
            }

            return obj;
        }

        public static IEnumerable<T> To<T>(this DataTable table)
        {
            List<T> ret = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T t = row.To<T>();
                ret.Add(t);
            }
            return ret;
        }

        public static IEnumerable<T> To<T>(this DataTable table, ref int totalRows, Dictionary<string, string> mapNames = null)
        {
            List<T> ret = new List<T>();
            if (mapNames == null)
            {
                foreach (DataRow row in table.Rows)
                {
                    T t = row.To<T>();
                    ret.Add(t);
                }
            }
            else
            {
                foreach (DataRow row in table.Rows)
                {
                    T t = row.To<T>(mapNames);
                    ret.Add(t);
                }
            }
            if (table.Rows.Count > 0 && table.Columns.Contains("TotalRows"))
                totalRows = Convert.ToInt32(table.Rows[0]["TotalRows"]);
            return ret;

        }

        public static T ToSum<T>(this DataRow row)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (p.CanWrite)
                {
                    string colName = "Sum_" + p.Name;
                    bool isExistedColumn = row.Table.Columns.Contains(colName);
                    DataToField<T>(row, obj, p, colName, isExistedColumn);
                }
            }

            return obj;
        }

        private static void DataToField<T>(DataRow row, T obj, PropertyInfo p, string colName, bool isExistedColumn)
        {
            if (isExistedColumn && row[colName].IsNotNullData())
            {
                if (p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?))
                {
                    p.SetValue(obj, decimal.Parse(row[colName].ToString()), null);
                }
                else if (p.PropertyType == typeof(double) || p.PropertyType == typeof(double?))
                {
                    p.SetValue(obj, double.Parse(row[colName].ToString()), null);
                }
                else if (p.PropertyType == typeof(string))
                {
                    p.SetValue(obj, row[colName].ToString(), null);
                }
                else
                {
                    p.SetValue(obj, row[colName], null);
                }
            }
            else
            {
                p.SetValue(obj, null, null);
            }
        }

        public static bool IsNullData(this object obj)
        {
            return obj == null || obj is DBNull;
        }

        public static bool IsNotNullData(this object obj)
        {
            return obj != null && obj != DBNull.Value;
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool HasData(this DataTable list)
        {
            return list != null && list.Rows.Count > 0;
        }

        public static decimal ToDecimalAmount(this object amount)
        {
            if(amount == null || string.IsNullOrEmpty(amount.ToString()))
            {
                return 0;
            }

            return Convert.ToDecimal(amount.ToString());
        }
    }
}
