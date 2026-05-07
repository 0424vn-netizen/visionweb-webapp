using AS.Controls.Pages;
using AS.Web.Business.Shared.Constants;
using AS.Web.Business.Shared.Enums;
using AS.Web.Business.Shared.Models;
using AS.Web.UI.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using DataTable = System.Data.DataTable;
using Formatting = Newtonsoft.Json.Formatting;

/// <summary>
/// Summary description for GeneralFuncsLib
/// </summary>
public static partial class GeneralFuncsLib
{
    public static T ReadJsonConfig<T>(string filePath)
    {
        try
        {
            if (HttpRuntime.Cache[filePath] == null)
            {
                var fullFilePath = HttpContext.Current.Server.MapPath("~/" + filePath);

                string jsonContent = File.ReadAllText(fullFilePath);

                T result = JsonConvert.DeserializeObject<T>(jsonContent);

                HttpRuntime.Cache.Insert(filePath, result, new CacheDependency(fullFilePath));
            }

            var data = (T)HttpRuntime.Cache[filePath];
            return data;
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error(ex);
            return default(T);
        }
    }

    public static void WriteObjectToJsonFile<T>(T obj, string filePath)
    {
        try
        {
            filePath = HttpContext.Current.Server.MapPath("~/" + filePath);
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error(ex);
        }
    }

    public static T GetValueByFilters<T>(DataTable dt, string[] columnNames, string[] columnValues, string columnResult)
    {
        if (columnNames.Length != columnValues.Length)
        {
            throw new ArgumentException("The length of columnNames and columnValues must be the same.");
        }

        IEnumerable<DataRow> query = dt.AsEnumerable();

        for (int i = 0; i < columnNames.Length; i++)
        {
            string columnName = columnNames[i];
            string columnValue = columnValues[i];
            query = query.Where(x => x.Field<string>(columnName).Equals(columnValue, StringComparison.OrdinalIgnoreCase));
        }

        DataRow dr = query.FirstOrDefault();
        if (dr != null)
        {
            return dr.Field<T>(columnResult);
        }

        return default(T);
    }

    public static List<string> ConvertStringToList(string str, char separator, bool isRemoveEmptyEntries)
    {
        if (string.IsNullOrEmpty(str))
        {
            return new List<string>();
        }

        var options = isRemoveEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
        var parts = str.Split(new[] { separator }, options);
        var result = parts.Select(part => part.Trim()).ToList();
        return result;
    }

    public static DateTime GetEndDate(DateRange dateRange, DateOptionMode dateOptionMode)
    {
        if (dateRange == null)
        {
            return DateTime.UtcNow;
        }

        return (dateOptionMode == DateOptionMode.Daily || dateOptionMode == DateOptionMode.Monthly)
            ? dateRange.From : dateRange.To;
    }

    public static Tuple<string, string> BuildToHtmlWithSecurePage(List<BuildToHtmlModel> lstData, string template, EnumPage pageName, SecurePage securePage)
    {
        if (securePage == null || string.IsNullOrEmpty(template) || lstData == null || !lstData.Any())
            return new Tuple<string, string>(template, string.Empty);

        var resultHtml = AS.Web.Business.General.GeneralFuncsLib.BuildToHtml(lstData, template);
        string secureQueryString = securePage.BuildSecureQueryString(resultHtml.Item2);
        switch (pageName)
        {
            case EnumPage.RM_MCF_DQReasonModal:
                secureQueryString = "rm_MCF_DQReasonModal.aspx?" + secureQueryString;
                break;
            default:
                break;
        }
        var resultTemplate = resultHtml.Item1.Replace(RiskReportConstants.KEY_SECURE_PARAM_HTML, secureQueryString);
        return new Tuple<string, string>(resultTemplate, secureQueryString);
    }
    public static string GetCurrentASClientId()
    {
        return SessionManager.CurrentUser != null && SessionManager.CurrentUser.ASClient > PciConstants.ID_ZERO ? SessionManager.CurrentUser.ASClient.ToString() : PciConstants.SSO_CLIENT_ID_DEFAULT;
    }
}