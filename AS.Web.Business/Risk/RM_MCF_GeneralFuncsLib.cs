using AS.Controls.Grid;
using System;
using System.Drawing;
using System.Globalization;

namespace AS.Web.Business.Risk
{
    public static class RM_MCF_GeneralFuncsLib
    {
        private const string HTML_EM_DASH_ENCODE = "—";
        private const string FORMAT_OF_CURRENCY = "C";
        private const string FORMAT_PROVIDER = "en-US";
        public static FormatType GetASFormat(string aSFormat)
        {
            FormatType result = FormatType.DynamicString;
            if (!string.IsNullOrEmpty(aSFormat))
            {
                switch (aSFormat.ToLower())
                {
                    case "integer":
                        result = FormatType.Integer;
                        break;
                    case "percentage":
                        result = FormatType.Percentage;
                        break;
                    case "currency":
                        result = FormatType.Currency;
                        break;
                    case "date":
                        result = FormatType.Date;
                        break;
                    case "dateandtime":
                        result = FormatType.DateAndTime;
                        break;
                    case "time":
                        result = FormatType.Time;
                        break;
                    case "percentage0digits":
                        result = FormatType.Percentage0Digits;
                        break;
                    case "percentage4digits":
                        result = FormatType.Percentage4Digits;
                        break;
                    case "currency4digits":
                        result = FormatType.Currency4Digits;
                        break;
                    case "number":
                        result = FormatType.Number;
                        break;
                    case "number1digit":
                        result = FormatType.Number1Digit;
                        break;
                    case "mumber4digits":
                        result = FormatType.Number4Digits;
                        break;
                    case "phone":
                        result = FormatType.Phone;
                        break;
                    case "number2digit":
                        result = FormatType.Number2Digit;
                        break;
                }
            }
            return result;
        }

        public static string GetVertical(FormatType asformat)
        {
            string result = "center";
            switch (asformat)
            {
                case FormatType.StaticString:
                case FormatType.DynamicString:
                case FormatType.None:
                    result = "left";
                    break;
                case FormatType.Currency:
                case FormatType.Currency4Digits:
                case FormatType.Integer:
                case FormatType.Number1Digit:
                case FormatType.Number:
                case FormatType.Number2Digit:
                case FormatType.Number4Digits:
                case FormatType.Percentage:
                case FormatType.Percentage0Digits:
                case FormatType.Percentage4Digits:
                    result = "right";
                    break;
            }
            return result;
        }

        public static string FormatBorderText(string value, Color color, bool checkWhiteColor = false)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(value) || value.Equals("&nbsp;"))
            {
                return result;
            }

            string style;
            if (string.IsNullOrEmpty(color.Name) || (checkWhiteColor && color.Name.Equals(Color.White.Name)))
            {
                style = string.Empty;
            }
            else
            {
                style = string.Format(" style='border-bottom: 2px solid {0}'", color.Name);
            }
            result = string.Format("<span{0}>{1}</span>", style, value);

            return result;
        }

        public static string FormatCurrencyText(object val, string formatProvider = FORMAT_PROVIDER, string format = FORMAT_OF_CURRENCY, string defaultVal = HTML_EM_DASH_ENCODE)
        {
            if (val == null || string.IsNullOrEmpty(val.ToString()))
            {
                return defaultVal;
            }
            if (string.IsNullOrEmpty(formatProvider))
            {
                formatProvider = FORMAT_PROVIDER;
            }
            var resultDecimal = val.ToDecimal();
            var cultureInfoValue = CultureInfo.CreateSpecificCulture(formatProvider);
            return resultDecimal.ToString(format, cultureInfoValue);
        }
        public static decimal ToDecimal(this object value, decimal defaultValue = 0m)
        {
            if (value == null || value.IsNullOrEmpty())
            {
                return defaultValue;
            }
            decimal.TryParse(value.ToString(), out decimal resultVaule);
            return resultVaule;
        }

        public static bool IsNullOrEmpty(this object value)
        {
            return value.IsNullData() || string.IsNullOrEmpty(value.ToString().Trim());
        }

        public static bool IsNullData(this object value)
        {
            return value == null || value == DBNull.Value;
        }
    }
}
