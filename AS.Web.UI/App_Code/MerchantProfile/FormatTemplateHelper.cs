using AS.Common.Formater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FormatTemplateHelper
/// </summary>
public static class FormatTemplateHelper
{
    public static string FormatSIC(object sic, object sicDesc)
    {
        return MerchantProfileHelper.FormatSIC(sic, sicDesc);
    }
    public static string BindAddress(object add1, object add2, object add3, object city, object state, object zip, bool? isExport = false)
    {
        string defaultValue = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        string address = MerchantProfileHelper.BindAddress(add1, add2, add3, city, state, zip);
        return address.IsNullOrEmpty() ? defaultValue : address;
    }

    public static string FormatInteger(object value, bool? isExport = false)
    {
        string defaultValue = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        return value == DBNull.Value ? defaultValue : FormatData.FormatInteger(value);
    }

    public static string FormatCurrency(object value, bool? isExport = false)
    {
        string defaultValue = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        return value == DBNull.Value ? defaultValue : GeneralFuncsLib.FormatCurrency(value);
    }
    public static string FormatPercent(object value, bool? isExport = false)
    {
        string defaultValue = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        return value.IsNullOrEmpty() ? defaultValue : FormatData.FormatPercent(value);
    }
    public static string FormatPhone(object phone, bool? isExport = false)
    {
        string defaultValue = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        return phone.IsNullOrEmpty() ? defaultValue : FormatData.FormatPhoneNumber(phone.ToString());
    }
    public static string FormatDate(object date, bool? isExport = false)
    {
        string defaultValue = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        date = MerchantProfileHelper.ProcessNullValue(date);
        return (date == null || date.ToString().IsNullOrEmpty())
            ? defaultValue : Convert.ToDateTime(date).ToString(WebSiteConstants.DATE_FORMAT);
    }
    public static string GetStatusText(object obj)
    {
        return MerchantProfileHelper.SetStatusForSiteAccess(MerchantProfileHelper.TranslateStatusText(obj));
    }

    public static string FormatDataOrEmDash(object value, bool? isExport = false)
    {
        string defaultValue = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        return value.IsNullOrEmpty() ? defaultValue : value.ToString();
    }

    public static string FormatCurrencyOrEmDashOrNegative(object value, bool? isExport = false)
    {
        string defaultValue = isExport.HasValue && isExport.Value ? WebSiteConstants.HTML_EM_DASH : WebSiteConstants.HTML_EM_DASH_ENCODE;
        return value.IsNullOrEmpty() ? defaultValue : FormatData.FormatCurrency(value);
    }

}