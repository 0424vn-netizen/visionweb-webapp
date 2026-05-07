using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AS.VW.PDFStatementCreator
{
    public class CustomDataBinder
    {
        public object FormatHandler { get; set; }

        public string Pattern { get; set; }

        public object Eval(object dataSource, string dataKey)
        {
            if (dataSource is DataTable)
            {
                var dataDt = dataSource as DataTable;
                if (dataDt.Rows.Count > 0)
                    return dataDt.Rows[0][dataKey];
            }
            else
            {
                PropertyInfo propertyInfo = dataSource.GetType().GetProperty(dataKey);
                if (propertyInfo == null)
                    return null;
                return propertyInfo.GetValue(dataSource);
            }
            return null;
        }

        public void Bind(string template, object dataSource)
        {
            if(string.IsNullOrEmpty(Pattern))
                Pattern = "<%#\\s*([\\w]*)\\s*\\(*\\s*Eval\\s*\\(\\s*\"([\\w]+)\"\\s*\\)\\s*\\)*\\s*%>";

            string formatHandlerText = "FormatHandler";
            MatchCollection cols = Regex.Matches(template, Pattern, RegexOptions.Multiline, System.TimeSpan.FromMilliseconds(1000));
            foreach (Match col in cols)
            {
                if (col.Groups.Count == 3)
                {
                    var func = col.Groups[1].Value;
                    var dataKey = col.Groups[2].Value;
                    string value = null;
                    if (string.IsNullOrEmpty(func))
                        value = Eval(dataSource, dataKey).ToString();
                    else
                    {
                        if (FormatHandler == null)
                            throw new ArgumentNullException(formatHandlerText, "FormatHandler is null");
                        MethodInfo method = FormatHandler.GetType().GetMethod(func);
                        if (method == null)
                            throw new ArgumentNullException(func,string.Format("Cannot find '{0}' method",func));
                        value = method.Invoke(FormatHandler, new[] { Eval(dataSource, dataKey) }).ToString();
                    }
                    template = template.Replace(col.Value, value);
                }
            }
        }
    }
}
