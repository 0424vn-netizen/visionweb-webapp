using AS.Controls.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SimpleFormatResolver
/// </summary>
/// 
namespace AS.Controls
{
    public static class FormatResolver
    {
        public static Func<object, string> GetFormatFunc(FormatType formatType)
        {
            switch (formatType)
            {
                case FormatType.Currency:
                    return GeneralFuncsLib.FormatCurrency2;
                case FormatType.Date:
                    return GeneralFuncsLib.FormatDate;
                case FormatType.StaticString:
                case FormatType.DynamicString:
                    return GeneralFuncsLib.FormatString;
                case FormatType.Integer:
                    return GeneralFuncsLib.FormatInteger;
                case FormatType.Percentage:
                    return GeneralFuncsLib.FormatPercent;
                case FormatType.Phone:
                case FormatType.Fax:
                    return GeneralFuncsLib.FormatPhone;
                default:
                    return null;
            }
        }

        public static string Format(FormatType formatType, object value)
        {
            Func<object, string> formatFunc = GetFormatFunc(formatType);
            if (formatFunc != null)
                return formatFunc(value);
            return string.Empty;
        }

        //public static Func<object, string> GetFormatExcelFunc(FormatType formatType)
        //{
        //    switch (formatType)
        //    {
        //        case FormatType.Currency:
        //            return GeneralFuncsLib.FormatCurrency2;
        //        case FormatType.Date:
        //            return GeneralFuncsLib.FormatDate;
        //        case FormatType.DynamicString:
        //        case FormatType.StaticString:
        //            return GeneralFuncsLib.FormatExcelString;
        //        case FormatType.Integer:
        //            return GeneralFuncsLib.FormatInteger;
        //        case FormatType.Percentage:
        //            return GeneralFuncsLib.FormatPercent;
        //        case FormatType.Phone:
        //        case FormatType.Fax:
        //            return GeneralFuncsLib.FormatPhone;
        //        default:
        //            return null;
        //    }
        //}

        //public static string FormatExcel(FormatType formatType, object value)
        //{
        //    Func<object, string> formatFunc = GetFormatExcelFunc(formatType);
        //    if (formatFunc != null)
        //        return formatFunc(value);
        //    return string.Empty;
        //}
    }
}