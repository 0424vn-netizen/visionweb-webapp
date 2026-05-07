using AS.Controls.Pages;
using AS.Web.Business.Shared.Constants;
using AS.Web.Business.Shared.Enums;
using AS.Web.Business.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace AS.Web.Business.General
{
    /// <summary>
    /// Summary description for GeneralFuncsLib
    /// </summary>
    public static partial class GeneralFuncsLib
    {
        public static List<string> SplitToArray(string inputData, char separator)
        {
            if (inputData == null) return new List<string>();
            var arr = inputData.Split(separator);
            return arr
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
        }

        public static string GetDoubleValue(string inputValue, int precision, bool isDecimal)
        {
            if (string.IsNullOrEmpty(inputValue))
                return null;
            string p = precision >= 1 ? "0".PadRight(precision, '0') : string.Empty;
            if (isDecimal && !inputValue.EndsWith("." + p))
                return double.Parse(inputValue).ToString("#,#0." + p);
            return double.Parse(inputValue).ToString("#,#0");
        }

        /// <summary>
        /// Check Exists In Split To Array
        /// </summary>
        /// <param name="inputData">The inputData convert to array</param>
        /// <param name="separator">The separator is Slipt</param>
        /// <param name="key">The key is check exists</param>
        /// <returns></returns>
        public static bool CheckExistsInSplitToArray(string inputData, char separator, string key)
        {
            if (string.IsNullOrEmpty(inputData) || string.IsNullOrEmpty(key))
                return false;
            var arrParrams = SplitToArray(inputData, separator);
            return arrParrams.Any(x => x.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        public static string SliptByCapitalLetter(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
                return null;
            var textData = Regex.Replace(inputData, "([A-Z]{1})", " $1").Trim();
            return Regex.Replace(textData, "([A-Z]{1}) ", "$1").Trim();
        }
        public static Tuple<string, string> BuildToHtml(List<BuildToHtmlModel> lstData, string template)
        {
            if (string.IsNullOrEmpty(template) || lstData == null || !lstData.Any())
                return new Tuple<string, string>(template, string.Empty);

            StringBuilder queryString = new StringBuilder();
            foreach (var item in lstData)
            {
                if (new List<EnumToHtml> { EnumToHtml.Template, EnumToHtml.All }.Contains(item.EnumToHtml))
                {
                    template = template.Replace("[" + item.Key + "]", item.Value);
                }
                if (!new List<EnumToHtml> { EnumToHtml.Param, EnumToHtml.All }.Contains(item.EnumToHtml))
                {
                    continue;
                }
                queryString.Append(string.Format("{0}={1}{2}", item.Key, item.Value, "&"));
            }
            return new Tuple<string, string>(template, queryString.ToString().TrimEnd('&'));
        }

        public static string GetValueDataRow(DataRow row, string columnName)
        {
            return GetValueDataRow(row, columnName, "");
        }
        public static string GetValueDataRow(DataRow row, string columnName, string defaultVal)
        {
            if (row == null || string.IsNullOrEmpty(columnName)) return string.Empty;
            var data = row[columnName];
            return data.IsNullOrEmpty() ? defaultVal : data.ToString();
        }
        public static string GetValueDataRowView(DataRowView rowView, string columnName)
        {
            if (rowView == null || string.IsNullOrEmpty(columnName)) return string.Empty;
            if (!rowView.Row.Table.Columns.Contains(columnName)) return string.Empty;
            var data = rowView[columnName];
            return data.IsNullOrEmpty() ? string.Empty : data.ToString();
        }
        public static bool IsNullOrEmpty(this object value)
        {
            return value.IsNullData() || string.IsNullOrEmpty(value.ToString());
        }
        public static bool IsNullData(this object value)
        {
            return value == null || value == DBNull.Value;
        }
       
        public static DateTime GetDateToNow(EnumTimeZone timeZone = EnumTimeZone.Default)
        {
            switch (timeZone)
            {
                case EnumTimeZone.Utc:
                    return DateTime.UtcNow;
                case EnumTimeZone.Default:
                default:
                    return DateTime.Now;
            }
        }
    }
}