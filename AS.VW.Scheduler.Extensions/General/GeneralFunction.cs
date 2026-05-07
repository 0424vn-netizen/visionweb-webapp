
using System;
using System.Data;
namespace AS.VW.Scheduler.Extensions.General
{
    public static class GeneralFunction
    {
        #region --- Variable & Enum ---
        private const string PARAMETER_VALUE = "ParameterValue";
        private const string PARAMETER_VALUE_HIGH = "ParameterValueHigh";
        private const string PARAMETER_PRECISION = "ParameterPrecision";

        private const string PARAMETER_THRESHOLD = "ParameterThreshold";
        private const string PARAMETER_THRESHOLD_HIGH = "ParameterThresholdHigh";
        private const string PARAMETER_THRESHOLD_TYPE = "ParameterThresholdType";

        private const string RE_PARAMETER_VALUE = "ReParameterValue";
        private const string RE_PARAMETER_THRESHOLD = "ReParameterThreshold";

        private const string ACTUAL_VALUE = "ActualValue";
        private const string ACTUAL_VALUE_1 = "ActualValue1";
        private const string ACTUAL_THRESHOLD = "ActualThreshold";
        private const string ACTUAL_THRESHOLD_1 = "ActualThreshold1";
        private const string DATA_TYPE = "DataType";
        private const string THRESHOLD_TYPE = "ThresholdType";

        #endregion

        public static void ProcessData(DataRow rowItem)
        {

            string parameterThreshold = rowItem.GetColumnValue(PARAMETER_THRESHOLD);
            string parameterValue = rowItem.GetColumnValue(PARAMETER_VALUE);
            string parameterValueHigh = rowItem.GetColumnValue(PARAMETER_VALUE_HIGH);
            string parameterThresholdHigh = rowItem.GetColumnValue(PARAMETER_THRESHOLD_HIGH);
            string parameterThresholdType = rowItem.GetColumnValue(PARAMETER_THRESHOLD_TYPE);
            int parameterPrecision = string.IsNullOrEmpty(rowItem.GetColumnValue(PARAMETER_PRECISION)) ? 0 : int.Parse(rowItem.GetColumnValue(PARAMETER_PRECISION));
            string indicatorFormat = "#,##0" + (parameterPrecision > 0 ? ".".PadRight(parameterPrecision + 1, '0') : string.Empty);

            string reParameterThreshold = rowItem.GetColumnValue(RE_PARAMETER_THRESHOLD);
            string reParameterValue = rowItem.GetColumnValue(RE_PARAMETER_VALUE);

            string actualThreshold = rowItem.GetColumnValue(ACTUAL_THRESHOLD);
            string actualThreshold1 = rowItem.GetColumnValue(ACTUAL_THRESHOLD_1);
            string actualValue = rowItem.GetColumnValue(ACTUAL_VALUE);
            string actualValue1 = rowItem.GetColumnValue(ACTUAL_VALUE_1);
            string dataType = rowItem.GetColumnValue(DATA_TYPE).ToLower();
            string thresholdType = rowItem.GetColumnValue(THRESHOLD_TYPE).ToLower();

            if (!parameterValue.Equals("0") && !String.IsNullOrEmpty(parameterValue))
            {
                parameterValue = decimal.Parse(parameterValue).ToString(indicatorFormat);
            }
            if (!parameterValueHigh.Equals("0") && !String.IsNullOrEmpty(parameterValueHigh))
            {
                parameterValueHigh = decimal.Parse(parameterValueHigh).ToString(indicatorFormat);
            }

            if (!actualValue.Equals("0") && !String.IsNullOrEmpty(actualValue))
            {
                actualValue = decimal.Parse(actualValue).ToString(indicatorFormat);
            }

            if (!parameterThreshold.Equals("0") && !String.IsNullOrEmpty(parameterThreshold))
            {
                parameterThreshold = decimal.Parse(parameterThreshold).ToString(indicatorFormat);
            }

            if (!actualThreshold.Equals("0") && !String.IsNullOrEmpty(actualThreshold))
            {
                actualThreshold = decimal.Parse(actualThreshold).ToString(indicatorFormat);

            }

            if (!parameterThresholdHigh.Equals("0") && !String.IsNullOrEmpty(parameterThresholdHigh))
            {
                parameterThresholdHigh = decimal.Parse(parameterThresholdHigh).ToString(indicatorFormat);
            }

            // Format for export
            if (!String.IsNullOrEmpty(parameterValueHigh))
            {
                rowItem.SetColumnValue(PARAMETER_VALUE, FormatLowHighThreshold(parameterValue, parameterValueHigh, dataType));
                rowItem.SetColumnValue(RE_PARAMETER_VALUE, FormatLowHighThreshold(reParameterValue, parameterValueHigh, dataType));
            }
            else
            {
                rowItem.SetColumnValue(PARAMETER_VALUE, FormatParameterExport(dataType, parameterValue, parameterPrecision));
                rowItem.SetColumnValue(RE_PARAMETER_VALUE, FormatParameterExport(dataType, reParameterValue, parameterPrecision));
            }

            switch (parameterThresholdType)
            {
                case "LowHigh":
                    rowItem.SetColumnValue(PARAMETER_THRESHOLD, FormatLowHighThreshold(parameterThreshold, parameterThresholdHigh, thresholdType));
                    rowItem.SetColumnValue(RE_PARAMETER_THRESHOLD, FormatLowHighThreshold(reParameterThreshold, parameterThresholdHigh, thresholdType));
                    break;
                default:
                    rowItem.SetColumnValue(PARAMETER_THRESHOLD, FormatParameterExport(thresholdType, parameterThreshold, 0));
                    rowItem.SetColumnValue(RE_PARAMETER_THRESHOLD, FormatParameterExport(thresholdType, reParameterThreshold, 0));
                    break;
            }

            var actualDisplay = FormatParameterExport(dataType, actualValue, parameterPrecision);
            if (!string.IsNullOrEmpty(actualDisplay) && !string.IsNullOrEmpty(actualValue1))
            {
                actualValue1 = GetActualText(actualValue1);
                actualDisplay = string.Format("{0}{1}({2})", actualDisplay, Environment.NewLine, actualValue1);
            }

            rowItem.SetColumnValue(ACTUAL_VALUE, actualDisplay);
            string topcardText = getTopCardText(actualThreshold1);
            rowItem.SetColumnValue(ACTUAL_THRESHOLD, FormatParameterExport(thresholdType, actualThreshold, parameterPrecision)
                                                      + (!string.IsNullOrEmpty(topcardText) ? Environment.NewLine + topcardText : string.Empty));
        }

        public static string FormatLowHighThreshold(string ThresholdLow, string ThresholdHigh, string ThresholdType)
        {
            return FormatLowHighThreshold(ThresholdLow, ThresholdHigh, ThresholdType, false);
        }

        public static string FormatLowHighThreshold(string ThresholdLow, string ThresholdHigh, string ThresholdType, bool isEdit)
        {
            string displayedValue;

            var low = FormatParameterDataType(ThresholdType, ThresholdLow, 0, false, isEdit);
            var high = FormatParameterDataType(ThresholdType, ThresholdHigh, 0, false, isEdit);

            if (low.IsNullOrEmpty() || high.IsNullOrEmpty())
            {
                displayedValue = string.Empty;
            }
            else
            {
                var ThresholdLowFormat = FormatParameterDataType(ThresholdType, ThresholdLow, 0, false, isEdit);
                var ThresholdHighFormat = FormatParameterDataType(ThresholdType, ThresholdHigh, 0, false, isEdit);
                if (string.IsNullOrEmpty(ThresholdLowFormat) || string.IsNullOrEmpty(ThresholdHighFormat))
                {
                    displayedValue = string.Empty;
                }
                else
                {
                    displayedValue = ThresholdLowFormat + " - " + ThresholdHighFormat;
                }

            }
            return displayedValue;
        }

        public static string FormatParameterDataType(string dataType, string dataValue)
        {
            return FormatParameterDataType(dataType, dataValue, 0);
        }
        public static string FormatParameterDataType(string dataType, string dataValue, int precision)
        {
            return FormatParameterDataType(dataType, dataValue, precision, false);
        }
        public static string FormatParameterDataType(string dataType, string dataValue, int precision, bool isHtmlMode, bool isEdit = false)
        {
            string displayedValue;
            string value;

            if (dataValue.IsNullOrEmpty())
            {
                displayedValue = string.Empty;
            }
            else
            {
                value = precision > 0 ? FormatPrecision(dataValue, precision) : GetDouble4Precision(dataValue);
                if (dataType == "$" || dataType == "%")
                {
                    string formatString = "{1}{0}";
                    if (dataType == "%")
                        formatString = "{0}{1}";

                    if (decimal.Parse(dataValue) >= 0)
                    {
                        displayedValue = string.Format(formatString, value, dataType);
                    }
                    else
                    {
                        displayedValue = string.Format("(" + formatString + ")", value.Replace("-", string.Empty), dataType);
                    }
                }
                else if (dataType == "days")
                    displayedValue = string.Format("{0} {1}", value, "day(s)");

                else
                    displayedValue = string.Format("{0}", value);
            }

            return displayedValue;
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static string FormatPrecision(string doubleStr, int precision)
        {
            if (string.IsNullOrEmpty(doubleStr))
                return doubleStr;
            if (precision == 2)
                return double.Parse(doubleStr).ToString("#,#0.00");
            else if (precision == 4)
                return double.Parse(doubleStr).ToString("#,#0.0000");
            else
                return double.Parse(doubleStr).ToString("#,#0");
        }
        public static string GetDouble4Precision(string doubleStr)
        {
            if (string.IsNullOrEmpty(doubleStr))
                return doubleStr;

            return double.Parse(doubleStr).ToString("#,#0");
        }
        public static string FormatParameterExport(string dataType, string dataValue, int precision)
        {
            return FormatParameterExport(dataType, dataValue, precision, false);
        }
        public static string FormatParameterExport(string dataType, string dataValue, int precision, bool isEdit)
        {
            string displayedValue;
            string value;

            if (dataValue.IsNullOrEmpty())
            {
                displayedValue = string.Empty;
            }
            else
            {
                value = precision > 0 ? FormatPrecision(dataValue, precision) : GetDouble4Precision(dataValue);
                if (dataType == "$")
                {
                    if (decimal.Parse(dataValue) >= 0)
                    {
                        displayedValue = string.Format("{1}{0}", value, dataType);
                    }
                    else
                    {
                        displayedValue = string.Format("-{1}{0}", value.Replace("-", string.Empty), dataType);
                    }
                }
                else if (dataType == "%")
                {
                    displayedValue = string.Format("{0}{1}", value, dataType);
                }
                else if (dataType == "days")
                    displayedValue = string.Format("{0} {1}", value, "day(s)");

                else
                    displayedValue = string.Format("{0}", value);
            }

            return displayedValue;
        }

        public static string GetColumnValue(this DataRow rowItem, string columnName)
        {
            return rowItem.Table.Columns.Contains(columnName) ? rowItem[columnName].ToString() : string.Empty;
        }

        public static void SetColumnValue(this DataRow rowItem, string columnName, object value)
        {
            if (rowItem.Table.Columns.Contains(columnName))
            {
                rowItem[columnName] = value;
            }
        }

        public static void SetColumnDataType(this DataTable source, string columnName, Type value)
        {
            if (source.Columns.Contains(columnName))
            {
                source.Columns[columnName].DataType = value;
            }
        }

        public static string getTopCardText(string number)
        {
            double topCardNumber = 0;
            double.TryParse(number, out topCardNumber);
            string topCardText = null;
            if (topCardNumber > 0)
            {
                topCardText = "(top card)";
                if (topCardNumber > 1)
                {
                    topCardText = string.Format("(top {0} cards)", topCardNumber);
                }
            }

            return topCardText;
        }

        private static string GetActualText(string actualText)
        {
            if (string.IsNullOrEmpty(actualText)) 
                return string.Empty;

            actualText = actualText.Substring(0, actualText.IndexOf(".") >= 0 ? actualText.IndexOf(".") : actualText.Length);

            return actualText;
        }
    }
}
