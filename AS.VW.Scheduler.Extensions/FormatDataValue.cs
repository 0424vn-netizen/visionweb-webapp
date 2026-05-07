using AS.VW.Scheduler.Extensions.General;
using AS.VW.Scheduler.Tasks;
using System;
using System.Data;

namespace AS.VW.Scheduler.Extensions
{
    public class FormatDataValue : IExtension
    {
        #region --- Variable & Enum ---
        private const string PARAMETER_VALUE = "ParameterValue";
        private const string PARAMETER_THRESHOLD = "ParameterThreshold";
        private const string RE_PARAMETER_VALUE = "ReParameterValue";
        private const string RE_PARAMETER_THRESHOLD = "ReParameterThreshold";
        private const string ACTUAL_VALUE = "ActualValue";
        private const string ACTUAL_THRESHOLD = "ActualThreshold";
        private const string ACTUAL_THRESHOLD_1 = "ActualThreshold1";
        #endregion

        public object Excute(object inputData)
        {
            DataTable result = new DataTable();
            result.Load(inputData as IDataReader);

            DataTable dtCloned = result.Clone();
            dtCloned.SetColumnDataType(PARAMETER_VALUE, typeof(string));
            dtCloned.SetColumnDataType(ACTUAL_VALUE, typeof(string));
            dtCloned.SetColumnDataType(RE_PARAMETER_VALUE, typeof(string));
            dtCloned.SetColumnDataType(RE_PARAMETER_THRESHOLD, typeof(string));
            dtCloned.SetColumnDataType(PARAMETER_THRESHOLD, typeof(string));
            dtCloned.SetColumnDataType(ACTUAL_THRESHOLD, typeof(string));
            dtCloned.SetColumnDataType(ACTUAL_THRESHOLD_1, typeof(string));

            foreach (DataColumn col in dtCloned.Columns)
            {
                col.ReadOnly = false;
            }

            foreach (DataRow row in result.Rows)
            {
                dtCloned.ImportRow(row);
                DataRow lastRow = dtCloned.Rows[dtCloned.Rows.Count - 1];
                GeneralFunction.ProcessData(lastRow);
            }

            return dtCloned;
        }

    }
}
