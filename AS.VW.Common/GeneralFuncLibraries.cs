using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Common
{
    public static class GeneralFuncLibraries
    {
        
        public static List<T> ConvertDataTable<T>(this DataTable dtTable)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dtTable.Rows)
            {
                T item = row.ConvertDataRow<T>();
                data.Add(item);
            }
            return data;
        }

        public static T ConvertDataRow<T>(this DataRow dr)
        {
            T item = (T)GetItem(typeof(T), dr);
            return item;
        }
        public static object GetItem(Type t,DataRow dr, string prefix = "")
        {
            object obj = Activator.CreateInstance(t);
            foreach (PropertyInfo pro in t.GetProperties())
            {
                if (!pro.CanWrite) continue;
               
                foreach (DataColumn column in dr.Table.Columns)
                {
                    if ((prefix + pro.Name).ToLower() == (column.ColumnName).ToLower())
                    {

                        if (pro.PropertyType.BaseType.FullName == "System.Enum")
                        {

                            if (dr[column.ColumnName] == System.DBNull.Value)
                            {
                                pro.SetValue(obj, Enum.Parse(pro.PropertyType, default(int).ToString(), true), null);
                            }
                            else
                            {
                                pro.SetValue(obj, Enum.Parse(pro.PropertyType, dr[column.ColumnName].ToString(), true), null);
                            }
                        }
                        else if (pro.PropertyType.FullName == "System.DateTime" && dr[column.ColumnName] == System.DBNull.Value)
                        {

                            pro.SetValue(obj, Convert.ChangeType(default(DateTime), pro.PropertyType), null);
                        }
                        else
                        {
                            pro.SetValue(obj, Convert.ChangeType(dr[column.ColumnName], pro.PropertyType), null);
                        }

                        break;
                    }
                }
            }

            return obj;
        }
    }
}
