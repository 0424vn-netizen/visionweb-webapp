using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Controls.AdhocFiltering;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Utilities;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using GeneralFuncsLibBusiness = AS.Web.Business.General.GeneralFuncsLib;
using RiskMCFGeneralFuntionBusiness = AS.Web.Business.Risk.RM_MCF_GeneralFuncsLib;

/// <summary>
/// Summary description for rm_MCF_GeneralFuncsLib
/// </summary>
public static class RM_MCF_GeneralFuncsLib
{
    #region ----- Variable & Const ----

    private const string COL_KEY = "key";
    private const string COL_IS_DEFAULT = "IsDefault";
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string DESCEND = "Descending";
    private const string ASCEND = "Ascending";
    public const string INCLUDED_KEY = "I #-IE-#";
    public const string EXCLUDED_KEY = "E #-IE-#";

    #endregion ----- Variable & Const ----

    #region ----- Private Methods ----

    /// <summary>
    /// get column  Assignment Customize
    /// </summary>
    /// <param name="listColumns"></param>
    /// <returns></returns>
    private static Dictionary<string, RiskCustomizeColumn> GetAssignmentCustomizeColumns(string[] dicColumns, bool isNotDefault = false)
    {
        var dt = GetAssignmentCustomizeColumnsToXML();

        Dictionary<string, RiskCustomizeColumn> result = new Dictionary<string, RiskCustomizeColumn>();
        if (dt.Rows.Count > 0)
        {
            //convert data to list
            var listRiskCustomize = DataTableToList<RiskCustomizeColumn>(dt);

            //get column config
            var data = (from a in dicColumns
                        join b in listRiskCustomize.Where(x => !x.IsDefault) on a equals b.Key
                        select b).ToDictionary(x => x.Key);
            if (isNotDefault)
            {
                //get columnDefault before all First
                var dataDefault = listRiskCustomize.Where(x => x.IsDefault).ToDictionary(x => x.Key);

                if (dataDefault != null && dataDefault.Count > 0)
                {
                    return dataDefault.Union(data).ToDictionary(pair => pair.Key, pair => pair.Value);
                }
            }
            return data;
        }

        return result;
    }

    /// <summary>
    /// get column  Assignment Customize for all full
    /// </summary>
    /// <param name="listColumns"></param>
    /// <returns></returns>
    private static Dictionary<string, RiskCustomizeColumn> GetAssignmentCustomizeColumnsExportAllView()
    {
        var dt = GetAssignmentCustomizeColumnsToXML();
        Dictionary<string, RiskCustomizeColumn> result = new Dictionary<string, RiskCustomizeColumn>();
        if (dt.Rows.Count > 0)
        {
            //convert data to list
            var listRiskCustomize = DataTableToList<RiskCustomizeColumn>(dt);
            //get column config
            var data = listRiskCustomize.OrderBy(x => x.OrderNo).ToDictionary(x => x.Key);
            return data;
        }

        return result;
    }
    /// <summary>
    /// get ASFormat
    /// </summary>
    /// <param name="aSFormat"></param>
    /// <returns></returns>
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
                case "staticstring":
                    result = FormatType.StaticString;
                    break;
            }
        }
        return result;
    }

    private static string GetVertical(FormatType asformat)
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
    #endregion ----- Private Methods ----

    #region ----- Public Methods ----

    public static Dictionary<string, RiskCustomizeColumn> GetColumnCustomView(int CustomViewID)
    {
        Dictionary<string, RiskCustomizeColumn> customizeColumns = new Dictionary<string, RiskCustomizeColumn>();
        DataTable dataResult = null;
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.AddLoggedInUserParamsWithRecId();

        paras.Add(new FilterParameter("@CustomViewID", CustomViewID, DbType.Int32));
        dataResult = WebServices.CsReportServices.GetReports("spa_RM_MCF_Get_CustomView", paras);

        if (dataResult == null || dataResult.Rows.Count == 0)
            return customizeColumns;

        var strColumns = GeneralFuncsLib.NvlString(dataResult.Rows[0]["ViewData"]);
        if (string.IsNullOrEmpty(strColumns))
        {
            return customizeColumns;
        }
        if (!string.IsNullOrEmpty(strColumns))
        {
            customizeColumns = GetAssignmentCustomizeColumns(strColumns.Split(','));
            StringBuilder listColumn = new StringBuilder();

            foreach (var item in customizeColumns)
            {
                item.Value.ASFormatType = GetASFormat(item.Value.ASFormat);
                item.Value.ReSourceKey = GetResourceValue(string.Format("{0}_Text", string.IsNullOrEmpty(item.Value.ReSourceKey) ? item.Key : item.Value.ReSourceKey));
            }
        }
        return customizeColumns;
    }

    public static string GenGridBarometerNextQueue(AS.Controls.Global.ASRepeater repeter, int CustomViewID, DataRow dataItem)
    {
        string result = "<tr>{0}</tr>";
        Dictionary<string, RiskCustomizeColumn> customizeColumns = new Dictionary<string, RiskCustomizeColumn>();
        DataTable dataResult = null;
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.AddLoggedInUserParamsWithRecId();
        paras.Add(new FilterParameter("@CustomViewID", CustomViewID, DbType.Int32));
        dataResult = WebServices.CsReportServices.GetReports("spa_RM_MCF_Get_CustomView", paras);

        if (dataResult == null || dataResult.Rows.Count == 0)
        {
            return string.Empty;
        }

        var strColumns = GeneralFuncsLibBusiness.NvlString(dataResult.Rows[0]["ViewData"]);
        if (string.IsNullOrEmpty(strColumns))
        {
            return string.Empty;
        }

        if (!string.IsNullOrEmpty(strColumns))
        {
            customizeColumns = GetAssignmentCustomizeColumns(strColumns.Split(','));
            StringBuilder listColumn = new StringBuilder();

            foreach (var item in customizeColumns)
            {
                item.Value.ASFormatType = RiskMCFGeneralFuntionBusiness.GetASFormat(item.Value.ASFormat);
                if (string.IsNullOrEmpty(item.Value.ReSourceKey))
                {
                    item.Value.ReSourceKey = item.Key;
                }
                string guid = Guid.NewGuid().ToString();
                string styleCss = item.Value.Width > 0 ? string.Concat("min-width:", item.Value.Width, "px;") : "";
                listColumn.Append(string.Format("<th class=\"\" title=\"{0}\" id=\"{2}\" style=\"{3}\">{1}</th>",
                    Regex.Replace(GetResourceValue(string.Format("{0}_Tooltip", item.Value.ReSourceKey)), "<.*?>", string.Empty),
                    GetResourceValue(string.Format("{0}_Text", item.Value.ReSourceKey)), guid, styleCss));

                GenerateTooltipHeaderColumnRepeter(repeter, guid, GetResourceValue(string.Format("{0}_Tooltip", item.Value.ReSourceKey)));
            }
            result = string.Format(result, listColumn.ToString());
        }

        string content = GenColumnBarometerNextQueue(dataItem, customizeColumns);
        result += content;
        return result;
    }

    public static string GenColumnBarometerNextQueue(DataRow dataItem, Dictionary<string, RiskCustomizeColumn> dicRicCustomizeColumn)
    {
        ReportPage page = new ReportPage();
        string result = "<tr>{0}</tr>";
        string url = string.Empty;
        StringBuilder listColumn = new StringBuilder();

        if (dicRicCustomizeColumn == null)
            return string.Empty;

        RiskCustomizeColumn riskCustomizeColumn = null;

        foreach (var item in dicRicCustomizeColumn)
        {

            if (item.Key.Equals("Disposition"))
            {
                var dipositionCol = dataItem["Disposition"].ToString();
                listColumn.Append(string.Format("<td align=\"center\" class=\"ellipsis\" title=\"{0}\" style=\"max-width:100px;\"><span data-selector=\"disposition\">{1}</span></td>", dipositionCol, dipositionCol));
            }
            else if (item.Key.Equals("ProfileDescription"))
            {
                //profile
                var profile = dataItem["ProfileDescription"].ToString();
                listColumn.Append(string.Format("<td align=\"center\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", VeraCodeSolution.DoVeraCode(profile), profile));
            }
            else if (item.Key.Equals("RiskScore"))
            {
                var riskScore = dataItem["RiskScore"];

                Color riskScoreColor = dataItem["RiskScoreColor"].ToString().ToColor();

                if (!GeneralFuncsLibBusiness.NvlString(dataItem["RiskScore"]).Equals("0") && dataItem["RiskScore"] != DBNull.Value
                    && !dataItem["RiskScore"].ToString().IsNullOrEmpty())
                {
                    string queryString = page.BuildSecureQueryString("MerchantNumber=" + dataItem[MERCHANT_NUMBER] + "&ReportDate=" + dataItem["ReportDate"]);
                    string urlRiskScoreDetail = "rm_MCF_RiskScoreDetailModal.aspx?" + queryString;
                    url = "<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"doOpenNewPopup('" + urlRiskScoreDetail + "'); return false;\">";

                    listColumn.Append(string.Format("<td align=\"right\" class=\"\" title=\"\" style=\"\">{0}</td>",
                  RiskMCFGeneralFuntionBusiness.FormatBorderText(VeraCodeSolution.GetOutputHtmlString(url + Convert.ToDecimal(dataItem["RiskScore"]).ToString("#,##0") + "</a>"), riskScoreColor)));
                }
                else
                {
                    listColumn.Append(string.Format("<td align=\"center\" class=\"\" title=\"\" style=\"\">{0}</td>",
                 RiskMCFGeneralFuntionBusiness.FormatBorderText(VeraCodeSolution.DoVeraCode(dataItem["RiskScore"].ToString().Trim() != string.Empty ? dataItem["RiskScore"].ToString() : "0"), riskScoreColor)));
                }
            }
            else if (item.Key.Equals("VolumePercent"))
            {
                riskCustomizeColumn = dicRicCustomizeColumn["VolumePercent"];

                var volumePercent = dataItem["VolumePercent"];
                string volumePercentToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", dataItem["TodayVolume"]).ToCurrencySymbol());
                Color volPercentColor = dataItem["VolumeColor"].ToString().ToColor();
                string rptDate = DateTime.Parse(dataItem["ReportDate"].ToString()).ToShortDateString();

                if (!GeneralFuncsLibBusiness.NvlString(dataItem["VolumePercent"]).Equals("0") && GeneralFuncsLibBusiness.NvlString(dataItem["VolumePercent"]).Length > 0)
                {
                    string queryString1 = page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                    dataItem["MerchantNumber"], dataItem["ReportDate"]));
                    string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"openPopupWindowOnMenu(this,'rm_MCF_TransactionDetailsModal.aspx?{0}','DQMCFWindow2'); return false;\">", queryString1);

                    listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>",
                      volumePercentToolTip, RiskMCFGeneralFuntionBusiness.FormatBorderText(VeraCodeSolution.DoVeraCode(url1 + FormatValue(dataItem["VolumePercent"], riskCustomizeColumn, false) + "</a>"), volPercentColor)));
                }
                else
                {
                    listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", volumePercentToolTip,
                   RiskMCFGeneralFuntionBusiness.FormatBorderText(VeraCodeSolution.DoVeraCode(dataItem["VolumePercent"].ToString().Trim() != string.Empty ?
                          FormatValue(dataItem["VolumePercent"], riskCustomizeColumn) : string.Empty), volPercentColor)));

                }
            }
            else if (item.Key.Equals("ContractualVolume"))
            {
                var contractualVolume = dataItem["ContractualVolume"];
                Color contractualVolumeColor = dataItem["CVColor"].ToString().ToColor();
                string contractualVolumeToolTip = string.Empty;
                if (dataItem["ContractualVolume"] != DBNull.Value)
                {
                    contractualVolumeToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", dataItem["ContractualDailyVolume"]).ToCurrencySymbol());
                }
                else
                {
                    contractualVolumeToolTip = GetResourceValue("ContractualValuenotBeenProvided_Resource");
                }

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", contractualVolumeToolTip,
                    RiskMCFGeneralFuntionBusiness.FormatBorderText(VeraCodeSolution.DoVeraCode(dataItem["ContractualVolume"] != DBNull.Value ?
                                 FormatValue(dataItem["ContractualVolume"], item.Value) : WebSiteConstants.HTML_EM_DASH_ENCODE), contractualVolumeColor)));
            }
            else if (item.Key.Equals("AverageTicketPercent"))
            {

                var avgTicket = dataItem["AverageTicketPercent"];
                Color avgTicketColor = dataItem["AvgTktColor"].ToString().ToColor();
                string avgTicketToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", dataItem["AVGTicket"]));
                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", avgTicketToolTip,
                    RiskMCFGeneralFuntionBusiness.FormatBorderText(FormatValue(dataItem["AverageTicketPercent"], item.Value, false), avgTicketColor)));
            }
            else if (item.Key.Equals("AuthorizationPercent"))
            {
                var authPercent = dataItem["AuthorizationPercent"];
                Color authPercentColor = dataItem["AuthColor"].ToString().ToColor();
                string authPercentToolTip = string.Format("{0:C} ({1})", dataItem["TodayAuthorizationVolume"], dataItem["TodayAuthorizationCount"]).ToCurrencySymbol();
                if (!GeneralFuncsLibBusiness.NvlString(dataItem["AuthorizationPercent"]).Equals("0") && GeneralFuncsLibBusiness.NvlString(dataItem["AuthorizationPercent"]).Length > 0)
                {
                    string queryString1 = page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                    dataItem["MerchantNumber"], dataItem["ReportDate"]));
                    string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"openPopupWindowOnMenu(this,'rm_MCF_NewRiskReport_TransactionHistoryModal.aspx?{0}','DQMCFWindow2'); return false;\">", queryString1);
                    listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", authPercentToolTip,
                     RiskMCFGeneralFuntionBusiness.FormatBorderText(VeraCodeSolution.DoVeraCode(url1 + FormatValue(dataItem["AuthorizationPercent"], item.Value, false) + "</a>"), authPercentColor)));
                }
                else
                {
                    listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", authPercentToolTip,
                      RiskMCFGeneralFuntionBusiness.FormatBorderText(VeraCodeSolution.DoVeraCode(FormatValue(dataItem["AuthorizationPercent"], item.Value)), authPercentColor)));
                }

            }
            else if (item.Key.Equals("DeclinedAuthorizationPercent"))
            {

                var decPercent = dataItem["DeclinedAuthorizationPercent"];
                Color decPercentColor = dataItem["DeclAuthPctColor"].ToString().ToColor();
                string decPercentToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", dataItem["TodayDeclinedAuthorizationVolume"]).ToCurrencySymbol());
                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", decPercentToolTip,
                                        RiskMCFGeneralFuntionBusiness.FormatBorderText(VeraCodeSolution.DoVeraCode(dataItem["DeclinedAuthorizationPercent"].ToString().Trim() != string.Empty ?
                    VeraCodeSolution.DoVeraCode(FormatValue(dataItem["DeclinedAuthorizationPercent"], item.Value)) : string.Empty), decPercentColor)));
            }
            else if (item.Key.Equals("RepeatAuthorizationCount"))
            {
                var rptAuth = FormatValue(dataItem["RepeatAuthorizationCount"], item.Value);
                Color rptAuthColor = dataItem["RptAuthColor"].ToString().ToColor();
                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"\" style=\"\">{0}</td>",
                  RiskMCFGeneralFuntionBusiness.FormatBorderText(rptAuth.ToString(), rptAuthColor, true)));
            }
            else if (item.Key.Equals("TodayForeignCardCount"))
            {
                var fc = FormatValue(dataItem["TodayForeignCardCount"], item.Value);
                Color fcColor = dataItem["FCColor"].ToString().ToColor();
                string fcToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", dataItem["TodayForeignCardVolume"]).ToCurrencySymbol());

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", fcToolTip,
                    RiskMCFGeneralFuntionBusiness.FormatBorderText(fc.ToString(), fcColor, true)));
            }
            else if (item.Key.Equals("KeyPercent"))
            {
                var keyPercent = dataItem["KeyPercent"];
                Color keyPercentColor = dataItem["KeyColor"].ToString().ToColor();
                string keyPercentToolTip = string.Format("{0} ({1})", GeneralFuncsLib.FormatCurrencyTooltip(dataItem["TodayKeyVolume"], SessionManager.CurrencyFortmat), dataItem["TodayKeyCount"]).ToCurrencySymbol();


                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", keyPercentToolTip,
                    RiskMCFGeneralFuntionBusiness.FormatBorderText(VeraCodeSolution.DoVeraCode(dataItem["KeyPercent"].ToString().Trim() != string.Empty ?
                    VeraCodeSolution.DoVeraCode(FormatValue(keyPercent, item.Value)) : string.Empty), keyPercentColor, true)));
            }
            else if (item.Key.Equals("EvenDollarTransactionPercent"))
            {
                var evenPercent = dataItem["EvenDollarTransactionPercent"];
                Color evenPercentColor = dataItem["EvenColor"].ToString().ToColor();

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"\" style=\"\">{0}</td>",
              RiskMCFGeneralFuntionBusiness.FormatBorderText(VeraCodeSolution.DoVeraCode(dataItem["EvenDollarTransactionPercent"].ToString() != string.Empty ?
                    VeraCodeSolution.DoVeraCode(FormatValue(evenPercent, item.Value)) : string.Empty), evenPercentColor, true)));
            }
            else if (item.Key.Equals("DuplicateDollarTransactionPercent"))
            {
                var dupPercent = dataItem["DuplicateDollarTransactionPercent"];
                Color dupPercentColor = dataItem["DupColor"].ToString().ToColor();
                string dupPercentToolTip = dataItem["DuplicateDollarTransactionCount"].ToString().ToCurrencySymbol();

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", dupPercentToolTip,
                     RiskMCFGeneralFuntionBusiness.FormatBorderText(FormatValue(dataItem["DuplicateDollarTransactionPercent"], item.Value), dupPercentColor)));
            }
            else if (item.Key.Equals("DuplicateBin"))
            {
                var dupBin = FormatValue(dataItem["DuplicateBin"], item.Value);
                Color dupBinColor = dataItem["SixDupBinColor"].ToString().ToColor();
                string dupBinToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", dataItem["DuplicateBin6TransactionAmount"]).ToCurrencySymbol());

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", dupBinToolTip,
                    RiskMCFGeneralFuntionBusiness.FormatBorderText(dupBin.ToString(), dupBinColor)));
            }
            else if (item.Key.Equals("NegativeBatchCount"))
            {
                var negative = FormatValue(dataItem["NegativeBatchCount"], item.Value);
                Color negColor = dataItem["NegColor"].ToString().ToColor();

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"\" style=\"\">{0}</td>",
                    RiskMCFGeneralFuntionBusiness.FormatBorderText(negative.ToString(), negColor)));

            }
            else if (item.Key.Equals("ZeroBatchCount"))
            {
                var zero = FormatValue(dataItem["ZeroBatchCount"], item.Value);
                Color zeroColor = dataItem["ZeroColor"].ToString().ToColor();

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"\" style=\"\">{0}</td>",
                 RiskMCFGeneralFuntionBusiness.FormatBorderText(zero.ToString(), zeroColor)));
            }
            else if (item.Key.Equals("TodayVolume"))
            {
                var todayVolumeCtrl = FormatValue(dataItem["TodayVolume"], item.Value);
                Color todayColor = dataItem["VolumeColor"].ToString().ToColor();

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"\" style=\"\">{0}</td>",
                 RiskMCFGeneralFuntionBusiness.FormatBorderText(todayVolumeCtrl.ToString(), todayColor)));
            }
            else if (item.Key.Equals("TodayFirstTimeRetrievalVolume"))
            {
                var rtvl = FormatValue(dataItem["TodayFirstTimeRetrievalVolume"], item.Value);
                Color rtvlColor = dataItem["RTVLColor"].ToString().ToColor();
                string rtvlToolTip = VeraCodeSolution.DoVeraCode(dataItem["TodayFirstTimeRetrievalCount"].ToString().ToCurrencySymbol());

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", rtvlToolTip,
                    RiskMCFGeneralFuntionBusiness.FormatBorderText(rtvl.ToString(), rtvlColor)));
            }
            else if (item.Key.Equals("TodayChargebackVolume"))
            {
                var cb = FormatValue(dataItem["TodayChargebackVolume"], item.Value);
                Color cbColor = dataItem["CBColor"].ToString().ToColor();
                string cbToolTip = VeraCodeSolution.DoVeraCode(dataItem["TodayChargebackCount"].ToString().ToCurrencySymbol());

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", cbToolTip,
                  RiskMCFGeneralFuntionBusiness.FormatBorderText(cb.ToString(), cbColor)));
            }
            else if (item.Key.Equals("ReturnPercent"))
            {
                var rtnPercent = dataItem["ReturnPercent"];
                Color rtnPercentColor = dataItem["RtnColor"].ToString().ToColor();
                string rtnPercentToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(dataItem["TodayReturnAmount"], SessionManager.CurrencyFortmat).ToCurrencySymbol());

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", rtnPercentToolTip,
                RiskMCFGeneralFuntionBusiness.FormatBorderText(VeraCodeSolution.DoVeraCode(FormatValue(dataItem["ReturnPercent"], item.Value)), rtnPercentColor)));
            }
            else if (item.Key.Equals("TodayHighestTransactionAmount"))
            {
                var maxTkt = dataItem["TodayHighestTransactionAmount"];
                Color maxTktColor = dataItem["MaxTktColor"].ToString().ToColor();
                var maxTktText = FormatValue(dataItem["TodayHighestTransactionAmount"], item.Value);
                listColumn.Append(string.Format("<td align=\"right\" class=\"\" title=\"\" style=\"\">{0}</td>",
                 RiskMCFGeneralFuntionBusiness.FormatBorderText(maxTktText, maxTktColor, true)));
            }
            else if (item.Key.Equals("TodayTransactionCount"))
            {
                var tkt = FormatValue(dataItem["TodayTransactionCount"], item.Value);
                Color tktColor = dataItem["TktsColor"].ToString().ToColor();

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"\" style=\"\">{0}</td>",
                 RiskMCFGeneralFuntionBusiness.FormatBorderText(tkt.ToString(), tktColor, true)));
            }
            else if (item.Key.Equals("TodayBatchCount"))
            {
                var batch = FormatValue(dataItem["TodayBatchCount"], item.Value);
                Color batchColor = dataItem["BatchColor"].ToString().ToColor();

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"\" style=\"\">{0}</td>",
                RiskMCFGeneralFuntionBusiness.FormatBorderText(batch.ToString(), batchColor, true)));
            }
            else if (item.Key.Equals("SICCode"))
            {
                var sic = dataItem["SICCode"];
                string sicToolTip = VeraCodeSolution.DoVeraCode(dataItem["SICDescription"].ToString().Replace("&nbsp;", string.Empty));

                listColumn.Append(string.Format("<td align=\"right\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", sicToolTip,
                  sic.ToString()));
            }
            else if (item.Key.Equals("SingleCardTransToday"))
            {
                var sc = dataItem["SingleCardTransToday"];
                Color scColor = dataItem["DupBinColor"].ToString().ToColor();

                string scToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", dataItem["VolumeOfSimilarCardTransaction"]).ToCurrencySymbol());

                listColumn.Append(string.Format("<td align=\"right\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", scToolTip,
                RiskMCFGeneralFuntionBusiness.FormatBorderText(sc.ToString(), scColor, true)));
            }
            else if (item.Key.Equals("RulesViolated"))
            {
                var pv = dataItem["RulesViolated"];
                string pvToolTip = VeraCodeSolution.DoVeraCode(dataItem["RulesViolated"].ToString());

                listColumn.Append(string.Format("<td align=\"center\" class=\"ellipsis\" title=\"{0}\" style=\"{2}\">{1}</td>", pvToolTip, pv.ToString(), "max-width:100px;"));
            }
            else if (item.Key.Equals("BusinessAge"))
            {
                var ba = dataItem["BusinessAge"];
                listColumn.Append(string.Format("<td align=\"center\" class=\"\" title=\"\" style=\"\">{0}</td>", ba.ToString()));
            }
            else if (item.Key.Equals("WorkQueueName"))
            {
                var wq = dataItem["WorkQueueName"];
                listColumn.Append(string.Format("<td align=\"center\" class=\"ellipsis\" title=\"{0}\" style=\"{2}\">{1}</td>", wq, wq.ToString(), "max-width:100px;"));
            }
            else if (item.Key.Equals("AttritionScore"))
            {
                var attr = FormatValue(dataItem["AttritionScore"], item.Value);
                var style = string.Empty;
                Color rtvlColor = dataItem["ARSColor"].ToString().ToColor();

                if (!(rtvlColor.Name.IsNullOrEmpty() || rtvlColor.Name.Equals(Color.White.Name)))
                {
                    style = string.Format(" style='border-bottom: 2px solid {0}'", rtvlColor.Name);
                }
                listColumn.Append(string.Format("<td align=\"center\" class=\"\" title=\"{0}\"><span {1}>{2}</span></td>",
                    dataItem["AttritionReasonCodes"].ToString(), style, attr.ToString()));
            }
            else if (item.Key.Equals("ReserveScore"))
            {
                var attr = FormatValue(dataItem["ReserveScore"], item.Value);
                var style = string.Empty;
                Color rtvlColor = dataItem["RRSColor"].ToString().ToColor();

                if (!(rtvlColor.Name.IsNullOrEmpty() || rtvlColor.Name.Equals(Color.White.Name)))
                {
                    style = string.Format(" style='border-bottom: 2px solid {0}'", rtvlColor.Name);
                }
                listColumn.Append(string.Format("<td align=\"center\" class=\"\" title=\"{0}\"><span {1}>{2}</span></td>",
                    dataItem["ReserveReasonCodes"].ToString(), style, attr.ToString()));
            }
            else if (item.Key.Equals("ACHRejects"))
            {
                var sc = FormatValue(dataItem["ACHRejects"], item.Value);
                Color scColor = dataItem["ACHReturnAmountColor"].ToString().ToColor();
                listColumn.Append(string.Format("<td align=\"right\" class=\"\" style=\"\">{0}</td>",
                RiskMCFGeneralFuntionBusiness.FormatBorderText(sc.ToString(), scColor, true)));
            }
            else if (item.Key.Equals("DeclinedAuthorizationCount"))
            {
                var sc = FormatValue(dataItem["DeclinedAuthorizationCount"], item.Value);
                Color scColor = dataItem["DeclAuthCountColor"].ToString().ToColor();
                listColumn.Append(string.Format("<td align=\"right\" class=\"\" style=\"\">{0}</td>",
                RiskMCFGeneralFuntionBusiness.FormatBorderText(sc.ToString(), scColor, true)));
            }
            else if (item.Key.Equals("MTDMasterCardChargebackCountPercent"))
            {
                var mtdMasterCardChargebackCountPercent = FormatValue(dataItem["MTDMasterCardChargebackCountPercent"], item.Value);
                Color mtdMasterCardChargebackCountPercentColor = dataItem["MTDMasterCardChargebackCountPercentColor"].ToString().ToColor();
                string mtdMasterCardChargebackCountPercentToolTip = dataItem["MTDMasterCardChargebackCount"].ToString().ToCurrencySymbol();

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", mtdMasterCardChargebackCountPercentToolTip,
                     RiskMCFGeneralFuntionBusiness.FormatBorderText(mtdMasterCardChargebackCountPercent.ToString(), mtdMasterCardChargebackCountPercentColor)));
            }
            else if (item.Key.Equals("MTDVisaChargebackCountPercent"))
            {
                var mtdVisaChargebackCountPercent = FormatValue(dataItem["MTDVisaChargebackCountPercent"], item.Value);
                Color mtdVisaChargebackCountPercentColor = dataItem["MTDVisaChargebackCountPercentColor"].ToString().ToColor();
                string mtdVisaChargebackCountPercentToolTip = dataItem["MTDVisaChargebackCount"].ToString().ToCurrencySymbol();

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", mtdVisaChargebackCountPercentToolTip,
                     RiskMCFGeneralFuntionBusiness.FormatBorderText(mtdVisaChargebackCountPercent.ToString(), mtdVisaChargebackCountPercentColor)));
            }
            else if (item.Key.Equals("ModelScoreValue"))
            {
                var rmsValue = dataItem["ModelScoreValue"];
                var rmsText = FormatValue(rmsValue, item.Value);
                Color rmsColor = dataItem["ModelScoreValueColor"].ToString().ToColor();
                string rmsToolTip = dataItem["ModelScoreReason"].ToString();

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", rmsToolTip,
                     RiskMCFGeneralFuntionBusiness.FormatBorderText(rmsText, rmsColor)));
            }
            else if (item.Key.Equals("ModelAlertValue"))
            {
                var rmavValue = dataItem["ModelAlertValue"];
                var rmavText = FormatValue(rmavValue, item.Value);
                Color rmavColor = dataItem["ModelAlertValueColor"].ToString().ToColor();
                string rmavToolTip = dataItem["ModelAlertReason"].ToString();

                listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", rmavToolTip,
                     RiskMCFGeneralFuntionBusiness.FormatBorderText(rmavText, rmavColor)));
            }
            else
            {
                if (item.Value.IsAutoText)
                {
                    var cellValue = GetColumnValue(dataItem, item.Value);
                    var toolTipText = item.Value.DefaultValue != cellValue ? GetColumnToolTip(dataItem, item.Value) : string.Empty;

                    cellValue = GetColumnBorderColor(cellValue, dataItem, item.Value);
                    var vertical = !string.IsNullOrEmpty(item.Value.Vertical) ? item.Value.Vertical : RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType);
                    listColumn.Append(string.Format("<td align=\"" + vertical + "\" class=\"\" title=\"{0}\" style=\"\">{1}</td>", toolTipText, cellValue));
                }
                else
                {
                    var contentCol = dataItem[item.Key];
                    var vertical = !string.IsNullOrEmpty(item.Value.Vertical) ? item.Value.Vertical : RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType);
                    listColumn.Append(string.Format("<td align=\"" + RiskMCFGeneralFuntionBusiness.GetVertical(item.Value.ASFormatType) + "\" class=\"\" title=\"\" style=\"\">{0}</td>",
                   FormatValue(contentCol, item.Value)));
                }
            }
        }

        result = string.Format(result, listColumn.ToString());
        return result;
    }

    #region ---- Process Grid ----
    /// <summary>
    /// get Colum and gen column grid view and card view
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="strColumns"></param>
    /// <returns></returns>
    public static Dictionary<string, RiskCustomizeColumn> GenGridAssignmentCustomizeColumns(ASGrid grid, int CustomViewID,
        bool isFullView = false, bool isNotDefault = false, bool hasRQColumn = false, bool isDistinctQueue = false)
    {
        Dictionary<string, RiskCustomizeColumn> customizeColumns = new Dictionary<string, RiskCustomizeColumn>();

        if (CustomViewID == -1)
            return customizeColumns;

        DataTable dataResult = null;
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.AddLoggedInUserParamsWithRecId();
        if (!isFullView)
        {
            paras.Add(new FilterParameter("@CustomViewID", CustomViewID, DbType.Int32));
            dataResult = WebServices.CsReportServices.GetReports("spa_RM_MCF_Get_CustomView", paras);
        }

        if (!isFullView && (dataResult == null || dataResult.Rows.Count == 0))
            return new Dictionary<string, RiskCustomizeColumn>();

        var strColumns = isFullView ? string.Empty : GeneralFuncsLib.NvlString(dataResult.Rows[0]["ViewData"]);
        if (!string.IsNullOrEmpty(strColumns) || isFullView)
        {
            customizeColumns = isFullView ? GetAssignmentCustomizeColumnsExportAllView() :
                GetAssignmentCustomizeColumns(strColumns.Split(','), isNotDefault);

            const string EXCLUDE_SALEGROUP_COLUMN = "SaleGroup";
            customizeColumns.Remove(EXCLUDE_SALEGROUP_COLUMN);

            var listExtendColumn = RM_MCF_GeneralFuncsLib.ExtendCustomColumn();
            var extendCol = customizeColumns.Where(i => i.Value.IsHide == true).ToDictionary(i => i.Key, p => p.Value);

            if (listExtendColumn != null)
            {
                foreach (var itemEx in listExtendColumn)
                {
                    extendCol = extendCol.Where(x => !x.Key.Trim().ToLower().Equals(itemEx.Trim().ToLower())).ToDictionary(x => x.Key, x => x.Value);
                }
            }

            if (extendCol.Count() > 0)
            {
                foreach (var item in extendCol)
                {
                    customizeColumns = customizeColumns.Where(x => x.Key != item.Key).ToDictionary(x => x.Key, x => x.Value);
                }
            }

            int orderno = grid.Columns.Count;
            foreach (var item in customizeColumns)
            {
                // Add new CurrentStatusDesc, IsRequeuedDesc for Export FullView. Not exist in CustomizeColumn
                if (item.Key.Equals("Work") && isFullView && !isDistinctQueue)
                {
                    ASGridBoundColumn currentStatusColumn = new ASGridBoundColumn();
                    currentStatusColumn.DataField = "CurrentStatusDesc";
                    currentStatusColumn.UniqueName = "CurrentStatusDesc";
                    currentStatusColumn.HeaderText = GetResourceValue(string.Format("{0}_Text", "Work"));
                    currentStatusColumn.Visible = false;
                    currentStatusColumn.OrderIndex = item.Value.OrderNo;
                    grid.MasterTableView.Columns.Add(currentStatusColumn);
                }

                if (item.Key.Equals("Requeued") && hasRQColumn && isFullView)
                {
                    ASGridBoundColumn isRequeuedDesc = new ASGridBoundColumn();
                    isRequeuedDesc.DataField = "IsRequeuedDesc";
                    isRequeuedDesc.UniqueName = "IsRequeuedDesc";
                    isRequeuedDesc.HeaderText = GetResourceValue(string.Format("{0}_Text", "IsRequeuedDesc"));
                    isRequeuedDesc.Visible = false;
                    isRequeuedDesc.OrderIndex = item.Value.OrderNo;
                    grid.MasterTableView.Columns.Add(isRequeuedDesc);
                }

                ASGridBoundColumn boundColumn = new ASGridBoundColumn();
                boundColumn.DataField = item.Key;
                boundColumn.UniqueName = item.Key;
                if (string.IsNullOrEmpty(item.Value.ReSourceKey))
                {
                    item.Value.ReSourceKey = item.Key;
                }
                boundColumn.HeaderTooltip = string.Empty;
                boundColumn.HeaderText = isFullView ? GetResourceValue(string.Format("{0}_Text", item.Value.ReSourceKey)) :
                    "<span id=" + item.Key + ">" + GetResourceValue(string.Format("{0}_Text", item.Value.ReSourceKey)) + "</span>";

                item.Value.ASFormatType = GetASFormat(item.Value.ASFormat);
                boundColumn.ASFormat = item.Value.ASFormatType;
                boundColumn.SortExpression = item.Key;
                boundColumn.OrderIndex = orderno;
                boundColumn.ASDefaultNullValue = item.Value.DefaultValue;
                boundColumn.HeaderStyle.Width = new Unit(item.Value.Width == 0 ? 80 : Convert.ToInt32(item.Value.Width), UnitType.Pixel);
                grid.MasterTableView.Columns.Add(boundColumn);
                orderno++;
            }
        }

        return customizeColumns;
    }
    public static void ItemRptDataBound_CardView(RiskCustomizeColumn riskCustomizeColumn, DataRow rowItem, HtmlGenericControl control, SecurePage page)
    {
        var url = string.Empty;
        //default value
        if (rowItem.Table.Columns.Contains(riskCustomizeColumn.Key))
        {
            control.InnerHtml = FormatValue(rowItem[riskCustomizeColumn.Key], riskCustomizeColumn);
        }

        //customcolumn
        //disposition
        if (riskCustomizeColumn.Key.Equals("Disposition"))
        {
            control.Attributes.Add("data-selector", "disposition");
            control.Attributes.Add("class", "ellipsis");
            control.Style.Add("width", "100px");
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["disposition"].ToString()));
        }

        if (riskCustomizeColumn.Key.Equals("WorkQueueName"))
        {
            control.Attributes.Add("class", "ellipsis");
            control.Style.Add("width", "100px");
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["WorkQueueName"].ToString()));
        }

        //riskscore
        if (riskCustomizeColumn.Key.Equals("RiskScore"))
        {
            Color riskScoreColor = rowItem["RiskScoreColor"].ToString().ToColor();
            if (!GeneralFuncsLibBusiness.NvlString(rowItem["RiskScore"]).Equals("0") && rowItem["RiskScore"] != DBNull.Value
                && !rowItem["RiskScore"].ToString().IsNullOrEmpty())
            {
                string queryString = page.BuildSecureQueryString("MerchantNumber=" + rowItem[MERCHANT_NUMBER] + "&ReportDate=" + rowItem["ReportDate"]);
                string urlRiskScoreDetail = "rm_MCF_RiskScoreDetailModal.aspx?" + queryString;
                url = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer;\" onclick=\"doOpenNewPopup('{0}'); return false;\">",
                    urlRiskScoreDetail);
                AddHtml(control, VeraCodeSolution.GetOutputHtmlString(url + (Convert.ToDecimal(rowItem["RiskScore"]).ToString("#,##0")) + "</a>"));
            }
            else
            {
                AddText(control, VeraCodeSolution.DoVeraCode(rowItem["RiskScore"].ToString().Trim() != string.Empty ?
                    rowItem["RiskScore"].ToString() : "0"));
            }
            control.InnerHtml = FormatBorderText(control.InnerHtml, riskScoreColor);
        }
        //volume percent
        else if (riskCustomizeColumn.Key.Equals("VolumePercent"))
        {
            Color volPercentColor = rowItem["VolumeColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayVolume"])));
            string rptDate = DateTime.Parse(rowItem["ReportDate"].ToString()).ToShortDateString();
            if (!GeneralFuncsLibBusiness.NvlString(rowItem["VolumePercent"]).Equals("0") && GeneralFuncsLibBusiness.NvlString(rowItem["VolumePercent"]).Length > 0)
            {
                string queryString1 = page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                rowItem["MerchantNumber"], rowItem["ReportDate"]));
                string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer;\" onclick=\"openPopupWindowOnMenu(event, 'rm_MCF_TransactionDetailsModal.aspx?{0}','DQMCFWindow1'); return false;\">", queryString1);
                AddHtml(control, VeraCodeSolution.DoVeraCode(url1 + FormatValue(rowItem["VolumePercent"], riskCustomizeColumn, false)) + "</a>");
            }
            else
            {
                AddText(control, VeraCodeSolution.DoVeraCode(rowItem["VolumePercent"].ToString().Trim() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["VolumePercent"], riskCustomizeColumn)) : string.Empty));
            }
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, volPercentColor);
        }

        //40965 Contractual Volume
        else if (riskCustomizeColumn.Key.Equals("ContractualVolume"))
        {
            Color contractualVolumeColor = rowItem["CVColor"].ToString().ToColor();
            string toolTip = GetResourceValue("ContractualValuenotBeenProvided_Resource");
            if (rowItem["ContractualVolume"] != DBNull.Value)
            {
                toolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["ContractualDailyVolume"]).ToCurrencySymbol());
            }

            AddTooltip(control, toolTip);
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["ContractualVolume"].ToString() != string.Empty ?
                            FormatValue(rowItem["ContractualVolume"], riskCustomizeColumn) : WebSiteConstants.HTML_EM_DASH_ENCODE));

            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, contractualVolumeColor);
        }
        //auth percent
        else if (riskCustomizeColumn.Key.Equals("AuthorizationPercent"))
        {
            Color authPercentColor = rowItem["AuthColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(string.Format("{0:C} ({1})", rowItem["TodayAuthorizationVolume"], rowItem["TodayAuthorizationCount"]).ToCurrencySymbol()));
            if (!GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Length > 0)
            {
                string queryString1 = page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                rowItem["MerchantNumber"], rowItem["ReportDate"]));
                string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer;\" onclick=\"openPopupWindowOnMenu(this,'rm_MCF_NewRiskReport_TransactionHistoryModal.aspx?{0}','DQMCFWindow2'); return false;\">", queryString1);
                AddHtml(control, VeraCodeSolution.DoVeraCode(url1 + FormatValue(rowItem["AuthorizationPercent"], riskCustomizeColumn, false) + "</a>"));
            }
            else
            {
                AddText(control, VeraCodeSolution.DoVeraCode(rowItem["AuthorizationPercent"].ToString().Trim() != string.Empty ?
                    FormatValue(rowItem["AuthorizationPercent"], riskCustomizeColumn) : string.Empty));
            }
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, authPercentColor);
        }

        //profile
        else if (riskCustomizeColumn.Key.Equals("ProfileDescription"))
        {
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["ProfileDescription"].ToSafeString()));
        }
        //average ticket
        else if (riskCustomizeColumn.Key.Equals("AverageTicketPercent"))
        {
            Color avgTicketColor = rowItem["AvgTktColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["AVGTicket"]).ToCurrencySymbol()));
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["AverageTicketPercent"].ToString() != string.Empty ?
                                            FormatValue(rowItem["AverageTicketPercent"], riskCustomizeColumn, false) : WebSiteConstants.HTML_EM_DASH_ENCODE));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, avgTicketColor);
        }
        //decline percent
        else if (riskCustomizeColumn.Key.Equals("DeclinedAuthorizationPercent"))
        {
            Color decPercentColor = rowItem["DeclAuthPctColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayDeclinedAuthorizationVolume"]).ToCurrencySymbol()));
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["DeclinedAuthorizationPercent"].ToString().Trim() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["DeclinedAuthorizationPercent"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, decPercentColor);
        }
        //# repeat auth
        else if (riskCustomizeColumn.Key.Equals("RptAuthColor"))
        {
            Color rptAuthColor = rowItem["RptAuthColor"].ToString().ToColor();
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, rptAuthColor, true);
        }
        //FC (foreign card)
        else if (riskCustomizeColumn.Key.Equals("TodayForeignCardCount"))
        {
            Color fCardCoutColor = rowItem["FCColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayForeignCardVolume"]).ToCurrencySymbol()));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, fCardCoutColor, true);
        }
        //key percent
        else if (riskCustomizeColumn.Key.Equals("KeyPercent"))
        {
            Color keyPercentColor = rowItem["KeyColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(string.Format("{0} ({1})", GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayKeyVolume"], SessionManager.CurrencyFortmat), rowItem["TodayKeyCount"]).ToCurrencySymbol()));
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["KeyPercent"].ToString().Trim() != string.Empty
                                ? VeraCodeSolution.DoVeraCode(FormatValue(rowItem["KeyPercent"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, keyPercentColor, true);
        }
        //even percent
        else if (riskCustomizeColumn.Key.Equals("EvenDollarTransactionPercent"))
        {
            Color evenPercentColor = rowItem["EvenColor"].ToString().ToColor();
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["EvenDollarTransactionPercent"].ToString() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["EvenDollarTransactionPercent"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, evenPercentColor, true);
        }
        //duplicate percent
        else if (riskCustomizeColumn.Key.Equals("DuplicateDollarTransactionPercent"))
        {
            Color dupPercentColor = rowItem["DupColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionCount"].ToString().ToCurrencySymbol()));
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionPercent"].ToString() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["DuplicateDollarTransactionPercent"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, dupPercentColor);
        }
        //duplicate bin
        else if (riskCustomizeColumn.Key.Equals("DuplicateBin"))
        {
            Color dupBinColor = rowItem["SixDupBinColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["DuplicateBin6TransactionAmount"]).ToCurrencySymbol()));
            AddText(control, VeraCodeSolution.DoVeraCode(FormatValue(rowItem["DuplicateBin"], riskCustomizeColumn)));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, dupBinColor);
        }
        //# negative
        else if (riskCustomizeColumn.Key.Equals("NegativeBatchCount"))
        {
            Color negColor = rowItem["NegColor"].ToString().ToColor();
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, negColor);
        }
        //# zero
        else if (riskCustomizeColumn.Key.Equals("ZeroBatchCount"))
        {
            Color zeroColor = rowItem["ZeroColor"].ToString().ToColor();
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, zeroColor);
        }
        //today volume
        else if (riskCustomizeColumn.Key.Equals("TodayVolume"))
        {
            Color todayVolumeColor = rowItem["VolumeColor"].ToString().ToColor();
            AddText(control, VeraCodeSolution.DoVeraCode(FormatValue(rowItem["TodayVolume"], riskCustomizeColumn)));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, todayVolumeColor);
        }
        ////RTVL
        else if (riskCustomizeColumn.Key.Equals("TodayFirstTimeRetrievalVolume"))
        {
            Color rtvlColor = rowItem["RTVLColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["TodayFirstTimeRetrievalCount"].ToString().ToCurrencySymbol()));
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["TodayFirstTimeRetrievalVolume"].ToString() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["TodayFirstTimeRetrievalVolume"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, rtvlColor);
        }
        ////CB
        else if (riskCustomizeColumn.Key.Equals("TodayChargebackVolume"))
        {
            Color cbColor = rowItem["CBColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["TodayChargebackCount"].ToString().ToCurrencySymbol()));
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["TodayChargebackVolume"].ToString() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["TodayChargebackVolume"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, cbColor);
        }
        ////Rtn Percent
        else if (riskCustomizeColumn.Key.Equals("ReturnPercent"))
        {
            Color rtnPercentColor = rowItem["RtnColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayReturnAmount"], SessionManager.CurrencyFortmat).ToCurrencySymbol()));
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["ReturnPercent"].ToString() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["ReturnPercent"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, rtnPercentColor);
        }
        ////Max ticket $
        else if (riskCustomizeColumn.Key.Equals("TodayHighestTransactionAmount"))
        {
            Color maxTktColor = rowItem["MaxTktColor"].ToString().ToColor();
            AddText(control, VeraCodeSolution.DoVeraCode(FormatValue(rowItem["TodayHighestTransactionAmount"], riskCustomizeColumn)));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, maxTktColor);
        }
        ////# Tkts
        else if (riskCustomizeColumn.Key.Equals("TodayTransactionCount"))
        {
            Color tktColor = rowItem["TktsColor"].ToString().ToColor();
            AddText(control, VeraCodeSolution.DoVeraCode(FormatValue(rowItem["TodayTransactionCount"], riskCustomizeColumn)));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, tktColor, true);
        }
        ////# batch
        else if (riskCustomizeColumn.Key.Equals("TodayBatchCount"))
        {
            Color batchColor = rowItem["BatchColor"].ToString().ToColor();
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, batchColor, true);
        }
        //SIC
        else if (riskCustomizeColumn.Key.Equals("SICDescription"))
        {
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["SICDescription"].ToString().Replace("&nbsp;", string.Empty)));
        }
        ////SC
        else if (riskCustomizeColumn.Key.Equals("SingleCardTransToday"))
        {
            Color scColor = rowItem["DupBinColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["VolumeOfSimilarCardTransaction"]).ToCurrencySymbol()));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, scColor);
        }
        else if (riskCustomizeColumn.Key.Equals("RulesViolated"))
        {
            var pv = rowItem["RulesViolated"].ToString();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["RulesViolated"].ToString().ToCurrencySymbol()));
            control.Attributes.Add("class", "ellipsis");
            control.Style.Add("width", "100px");
        }
        ////ARS 
        else if (riskCustomizeColumn.Key.Equals("AttritionScore"))
        {
            Color rtvlColor = rowItem["ARSColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["AttritionReasonCodes"].ToString()));
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["AttritionScore"].ToString() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["AttritionScore"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, rtvlColor);
        }
        else if (riskCustomizeColumn.Key.Equals("ReserveScore"))
        {
            Color rtvlColor = rowItem["RRSColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["ReserveReasonCodes"].ToString()));
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["ReserveScore"].ToString() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["ReserveScore"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, rtvlColor);
        }
        else if (riskCustomizeColumn.Key.Equals("ACHRejects"))
        {
            Color rtvlColor = rowItem["ACHReturnAmountColor"].ToString().ToColor();
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["ACHRejects"].ToString() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["ACHRejects"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = System.Net.WebUtility.HtmlDecode(FormatBorderText(control.InnerHtml, rtvlColor));
        }
        else if (riskCustomizeColumn.Key.Equals("DeclinedAuthorizationCount"))
        {
            Color rtvlColor = rowItem["DeclAuthCountColor"].ToString().ToColor();
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["DeclinedAuthorizationCount"].ToString() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["DeclinedAuthorizationCount"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, rtvlColor);
        }
        else if (riskCustomizeColumn.Key.Equals("MTDMasterCardChargebackCountPercent"))
        {
            Color mtdMasterCardChargebackCountPercentColor = rowItem["MTDMasterCardChargebackCountPercentColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["MTDMasterCardChargebackCount"].ToString().ToCurrencySymbol()));
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["MTDMasterCardChargebackCountPercent"].ToString().Trim() != string.Empty
                                ? VeraCodeSolution.DoVeraCode(FormatValue(rowItem["MTDMasterCardChargebackCountPercent"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, mtdMasterCardChargebackCountPercentColor);
        }
        else if (riskCustomizeColumn.Key.Equals("MTDVisaChargebackCountPercent"))
        {
            Color mtdVisaChargebackCountPercentColor = rowItem["MTDVisaChargebackCountPercentColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["MTDVisaChargebackCount"].ToString().ToCurrencySymbol()));
            AddText(control, VeraCodeSolution.DoVeraCode(rowItem["MTDVisaChargebackCountPercent"].ToString().Trim() != string.Empty
                                ? VeraCodeSolution.DoVeraCode(FormatValue(rowItem["MTDVisaChargebackCountPercent"], riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, mtdVisaChargebackCountPercentColor);
        }
        else if (riskCustomizeColumn.Key.Equals("ModelScoreValue"))
        {
            var rmsValue = rowItem["ModelScoreValue"];
            Color rmsColor = rowItem["ModelScoreValueColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["ModelScoreReason"].ToString()));
            AddText(control, VeraCodeSolution.DoVeraCode(rmsValue.ToString().Trim() != string.Empty
                                ? VeraCodeSolution.DoVeraCode(FormatValue(rmsValue, riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, rmsColor);
        }
        else if (riskCustomizeColumn.Key.Equals("ModelAlertValue"))
        {
            var rmavValue = rowItem["ModelAlertValue"];
            Color rmavColor = rowItem["ModelAlertValueColor"].ToString().ToColor();
            AddTooltip(control, VeraCodeSolution.DoVeraCode(rowItem["ModelAlertReason"].ToString()));
            AddText(control, VeraCodeSolution.DoVeraCode(rmavValue.ToString().Trim() != string.Empty
                                ? VeraCodeSolution.DoVeraCode(FormatValue(rmavValue, riskCustomizeColumn)) : string.Empty));
            control.InnerHtml = RiskMCFGeneralFuntionBusiness.FormatBorderText(control.InnerHtml, rmavColor);
        }
        else
        {
            if (riskCustomizeColumn.IsAutoText)
            {
                var cellValue = GetColumnValue(rowItem, riskCustomizeColumn);
                var toolTipText = riskCustomizeColumn.DefaultValue != cellValue ? GetColumnToolTip(rowItem, riskCustomizeColumn) : string.Empty;

                cellValue = GetColumnBorderColor(cellValue, rowItem, riskCustomizeColumn);

                AddTooltip(control, toolTipText);
                control.InnerHtml = VeraCodeSolution.DoVeraCode(cellValue);
            }
            else
            {
                var cellValue = GetColumnValue(rowItem, riskCustomizeColumn);
                control.InnerHtml = VeraCodeSolution.DoVeraCode(cellValue);
            }
        }
    }

    public static void ItemDataBound_GridView(GridDataItem dataItem, DataRow rowItem, SecurePage page, Dictionary<string, RiskCustomizeColumn> dicRicCustomizeColumn)
    {

        var url = "";
        var listColumnCustomizes = new List<string>();
        RiskCustomizeColumn riskCustomizeColumn = null;
        var currencyFortmat = SessionManager.CurrencyFortmat;

        if (dicRicCustomizeColumn == null)
            return;

        if (dicRicCustomizeColumn.ContainsKey("Disposition"))
        {
            var dipositionCol = dataItem["Disposition"];
            dipositionCol.Attributes.Add("data-selector", "disposition");
            dipositionCol.Attributes.Add("class", "ellipsis");
            dipositionCol.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["Disposition"].ToString());
        }

        if (dicRicCustomizeColumn.ContainsKey("ProfileDescription"))
        {
            listColumnCustomizes.Add("ProfileDescription");
            //profile
            var profile = dataItem["ProfileDescription"];
            string profiletext = rowItem["ProfileDescription"].ToString();
            profile.ToolTip = VeraCodeSolution.DoVeraCode(profiletext);
        }

        //riskscore
        if (dicRicCustomizeColumn.ContainsKey("RiskScore"))
        {
            listColumnCustomizes.Add("RiskScore");
            var riskScore = dataItem["RiskScore"];
            Color riskScoreColor = rowItem["RiskScoreColor"].ToString().ToColor();

            if (!GeneralFuncsLibBusiness.NvlString(rowItem["RiskScore"]).Equals("0") && rowItem["RiskScore"] != DBNull.Value
                && !rowItem["RiskScore"].ToString().IsNullOrEmpty())
            {
                string queryString = page.BuildSecureQueryString("MerchantNumber=" + rowItem[MERCHANT_NUMBER] + "&ReportDate=" + rowItem["ReportDate"]);
                string urlRiskScoreDetail = "rm_MCF_RiskScoreDetailModal.aspx?" + queryString;
                url = "<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"doOpenNewPopup('" + urlRiskScoreDetail + "'); return false;\">";

                riskScore.Text = VeraCodeSolution.GetOutputHtmlString(url + dataItem["RiskScore"].Text + "</a>");
            }
            else
            {
                riskScore.Text = VeraCodeSolution.DoVeraCode(rowItem["RiskScore"].ToString().Trim() != string.Empty ?
                    dataItem["RiskScore"].Text : "0");
            }
            riskScore.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(riskScore.Text, riskScoreColor);
        }
        //volume percent
        if (dicRicCustomizeColumn.ContainsKey("VolumePercent"))
        {
            listColumnCustomizes.Add("VolumePercent");
            riskCustomizeColumn = dicRicCustomizeColumn["VolumePercent"];

            var volumePercent = dataItem["VolumePercent"];
            volumePercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayVolume"]).ToCurrencySymbol());
            Color volPercentColor = rowItem["VolumeColor"].ToString().ToColor();
            string rptDate = DateTime.Parse(rowItem["ReportDate"].ToString()).ToShortDateString();
            if (!GeneralFuncsLibBusiness.NvlString(rowItem["VolumePercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["VolumePercent"]).Length > 0)
            {
                string queryString1 = page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                rowItem["MerchantNumber"], rowItem["ReportDate"]));
                string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"openPopupWindowOnMenu(this,'rm_MCF_TransactionDetailsModal.aspx?{0}','DQMCFWindow2'); return false;\">", queryString1);
                dataItem["VolumePercent"].Text = VeraCodeSolution.DoVeraCode(url1 + FormatValue(rowItem["VolumePercent"], riskCustomizeColumn, false) + "</a>");
            }
            else
            {
                volumePercent.Text = VeraCodeSolution.DoVeraCode(rowItem["VolumePercent"].ToString().Trim() != string.Empty ?
                      FormatValue(rowItem["VolumePercent"], riskCustomizeColumn) : string.Empty);
            }
            volumePercent.Text = FormatBorderText(volumePercent.Text, volPercentColor);
        }

        // 40965
        //ContractualVolume
        if (dicRicCustomizeColumn.ContainsKey("ContractualVolume"))
        {
            listColumnCustomizes.Add("ContractualVolume");
            riskCustomizeColumn = dicRicCustomizeColumn["ContractualVolume"];
            var contractualVolume = dataItem["ContractualVolume"];
            Color contractualVolumeColor = rowItem["CVColor"].ToString().ToColor();
            if (rowItem["ContractualVolume"] != DBNull.Value)
            {
                contractualVolume.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["ContractualDailyVolume"]).ToCurrencySymbol());
            }
            else
            {
                contractualVolume.ToolTip = GetResourceValue("ContractualValuenotBeenProvided_Resource");
            }
            contractualVolume.Text = VeraCodeSolution.DoVeraCode(rowItem["ContractualVolume"] != DBNull.Value ?
                             FormatValue(rowItem["ContractualVolume"], riskCustomizeColumn) : WebSiteConstants.HTML_EM_DASH_ENCODE);
            contractualVolume.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(contractualVolume.Text, contractualVolumeColor);
            contractualVolume.HorizontalAlign = HorizontalAlign.Right;
        }
        //average ticket
        if (dicRicCustomizeColumn.ContainsKey("AverageTicketPercent"))
        {
            listColumnCustomizes.Add("AverageTicketPercent");
            riskCustomizeColumn = dicRicCustomizeColumn["AverageTicketPercent"];

            var avgTicket = dataItem["AverageTicketPercent"];
            Color avgTicketColor = rowItem["AvgTktColor"].ToString().ToColor();
            avgTicket.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["AVGTicket"]));
            avgTicket.Text = VeraCodeSolution.DoVeraCode(rowItem["AverageTicketPercent"].ToString() != string.Empty ?
                (FormatValue(rowItem["AverageTicketPercent"], riskCustomizeColumn, false)) : string.Empty);
            avgTicket.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(avgTicket.Text, avgTicketColor);

        }
        //auth percent
        if (dicRicCustomizeColumn.ContainsKey("AuthorizationPercent"))
        {
            listColumnCustomizes.Add("AuthorizationPercent");
            riskCustomizeColumn = dicRicCustomizeColumn["AuthorizationPercent"];
            var authPercent = dataItem["AuthorizationPercent"];
            Color authPercentColor = rowItem["AuthColor"].ToString().ToColor();
            authPercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C} ({1})", rowItem["TodayAuthorizationVolume"], rowItem["TodayAuthorizationCount"]).ToCurrencySymbol());
            if (!GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Equals("0") && GeneralFuncsLib.NvlString(rowItem["AuthorizationPercent"]).Length > 0)
            {
                string queryString1 = page.BuildSecureQueryString(string.Format("MerchantNumber={0}&ReportDate={1}",
                rowItem["MerchantNumber"], rowItem["ReportDate"]));
                string url1 = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"openPopupWindowOnMenu(this,'rm_MCF_NewRiskReport_TransactionHistoryModal.aspx?{0}','DQMCFWindow2'); return false;\">", queryString1);
                dataItem["AuthorizationPercent"].Text = VeraCodeSolution.DoVeraCode(url1 + FormatValue(rowItem["AuthorizationPercent"], riskCustomizeColumn, false) + "</a>");
            }
            else
            {
                authPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["AuthorizationPercent"].ToString().Trim() != string.Empty ?
               FormatValue(rowItem["AuthorizationPercent"], riskCustomizeColumn) : string.Empty);
            }
            authPercent.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(authPercent.Text, authPercentColor);
        }
        //decline percent
        if (dicRicCustomizeColumn.ContainsKey("DeclinedAuthorizationPercent"))
        {
            listColumnCustomizes.Add("DeclinedAuthorizationPercent");
            riskCustomizeColumn = dicRicCustomizeColumn["DeclinedAuthorizationPercent"];

            var decPercent = dataItem["DeclinedAuthorizationPercent"];
            Color decPercentColor = rowItem["DeclAuthPctColor"].ToString().ToColor();
            decPercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayDeclinedAuthorizationVolume"]));
            decPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["DeclinedAuthorizationPercent"].ToString().Trim() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["DeclinedAuthorizationPercent"], riskCustomizeColumn)) : string.Empty);
            decPercent.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(decPercent.Text, decPercentColor);
        }
        //# repeat auth
        if (dicRicCustomizeColumn.ContainsKey("RepeatAuthorizationCount"))
        {
            listColumnCustomizes.Add("RepeatAuthorizationCount");
            var rptAuth = dataItem["RepeatAuthorizationCount"];
            Color rptAuthColor = rowItem["RptAuthColor"].ToString().ToColor();
            rptAuth.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(rptAuth.Text, rptAuthColor, true);
        }
        //FC (foreign card)
        if (dicRicCustomizeColumn.ContainsKey("TodayForeignCardCount"))
        {
            listColumnCustomizes.Add("TodayForeignCardCount");
            var fc = dataItem["TodayForeignCardCount"];
            Color fcColor = rowItem["FCColor"].ToString().ToColor();
            fc.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["TodayForeignCardVolume"]).ToCurrencySymbol());
            fc.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(fc.Text, fcColor, true);
        }
        //key percent
        if (dicRicCustomizeColumn.ContainsKey("KeyPercent"))
        {
            listColumnCustomizes.Add("KeyPercent");
            riskCustomizeColumn = dicRicCustomizeColumn["KeyPercent"];
            var keyPercent = dataItem["KeyPercent"];
            Color keyPercentColor = rowItem["KeyColor"].ToString().ToColor();
            keyPercent.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0} ({1})", GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayKeyVolume"], currencyFortmat), rowItem["TodayKeyCount"]).ToCurrencySymbol());
            keyPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["KeyPercent"].ToString().Trim() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["KeyPercent"], riskCustomizeColumn)) : string.Empty);
            keyPercent.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(keyPercent.Text, keyPercentColor, true);
        }
        //even percent
        if (dicRicCustomizeColumn.ContainsKey("EvenDollarTransactionPercent"))
        {
            listColumnCustomizes.Add("EvenDollarTransactionPercent");
            riskCustomizeColumn = dicRicCustomizeColumn["EvenDollarTransactionPercent"];
            var evenPercent = dataItem["EvenDollarTransactionPercent"];
            Color evenPercentColor = rowItem["EvenColor"].ToString().ToColor();
            evenPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["EvenDollarTransactionPercent"].ToString() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["EvenDollarTransactionPercent"], riskCustomizeColumn)) : string.Empty);
            evenPercent.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(evenPercent.Text, evenPercentColor, true);
        }
        //duplicate percent
        if (dicRicCustomizeColumn.ContainsKey("DuplicateDollarTransactionPercent"))
        {
            listColumnCustomizes.Add("DuplicateDollarTransactionPercent");
            riskCustomizeColumn = dicRicCustomizeColumn["DuplicateDollarTransactionPercent"];
            var dupPercent = dataItem["DuplicateDollarTransactionPercent"];
            Color dupPercentColor = rowItem["DupColor"].ToString().ToColor();
            dupPercent.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionCount"].ToString().ToCurrencySymbol());
            dupPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["DuplicateDollarTransactionPercent"].ToString() != string.Empty ?
                VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["DuplicateDollarTransactionPercent"].ToString())).ToString("#,#0") + "%") : string.Empty);
            dupPercent.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(dupPercent.Text, dupPercentColor);
        }
        //duplicate bin
        if (dicRicCustomizeColumn.ContainsKey("DuplicateBin"))
        {
            listColumnCustomizes.Add("DuplicateBin");
            var dupBin = dataItem["DuplicateBin"];
            Color dupBinColor = rowItem["SixDupBinColor"].ToString().ToColor();
            dupBin.ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["DuplicateBin6TransactionAmount"], currencyFortmat));
            dupBin.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(dupBin.Text, dupBinColor);
        }

        //# negative
        if (dicRicCustomizeColumn.ContainsKey("NegativeBatchCount"))
        {
            listColumnCustomizes.Add("NegativeBatchCount");
            var negative = dataItem["NegativeBatchCount"];
            Color negColor = rowItem["NegColor"].ToString().ToColor();
            negative.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(negative.Text, negColor);
        }
        //# zero
        if (dicRicCustomizeColumn.ContainsKey("ZeroBatchCount"))
        {
            listColumnCustomizes.Add("ZeroBatchCount");
            var zero = dataItem["ZeroBatchCount"];
            Color zeroColor = rowItem["ZeroColor"].ToString().ToColor();
            zero.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(zero.Text, zeroColor);
        }
        //today volume
        if (dicRicCustomizeColumn.ContainsKey("TodayVolume"))
        {
            listColumnCustomizes.Add("TodayVolume");
            var todayVolumeCtrl = dataItem["TodayVolume"];
            Color todayColor = rowItem["VolumeColor"].ToString().ToColor();
            todayVolumeCtrl.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(todayVolumeCtrl.Text, todayColor);
        }
        //RTVL
        if (dicRicCustomizeColumn.ContainsKey("TodayFirstTimeRetrievalVolume"))
        {
            listColumnCustomizes.Add("TodayFirstTimeRetrievalVolume");
            var rtvl = dataItem["TodayFirstTimeRetrievalVolume"];
            Color rtvlColor = rowItem["RTVLColor"].ToString().ToColor();
            rtvl.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["TodayFirstTimeRetrievalCount"].ToString().ToCurrencySymbol());
            rtvl.Text = rtvl.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(rtvl.Text, rtvlColor);
        }
        //CB
        if (dicRicCustomizeColumn.ContainsKey("TodayChargebackVolume"))
        {
            listColumnCustomizes.Add("TodayChargebackVolume");
            var cb = dataItem["TodayChargebackVolume"];
            Color cbColor = rowItem["CBColor"].ToString().ToColor();
            cb.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["TodayChargebackCount"].ToString().ToCurrencySymbol());
            cb.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(cb.Text, cbColor);
        }
        //Rtn Percent
        if (dicRicCustomizeColumn.ContainsKey("ReturnPercent"))
        {
            listColumnCustomizes.Add("ReturnPercent");
            var rtnPercent = dataItem["ReturnPercent"];
            Color rtnPercentColor = rowItem["RtnColor"].ToString().ToColor();
            rtnPercent.ToolTip = VeraCodeSolution.DoVeraCode(GeneralFuncsLib.FormatCurrencyTooltip(rowItem["TodayReturnAmount"], currencyFortmat)).ToCurrencySymbol();
            rtnPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["ReturnPercent"].ToString().Trim() != string.Empty ?
                VeraCodeSolution.DoVeraCode((decimal.Parse(rowItem["ReturnPercent"].ToString())).ToString("#,#0") + "%") : string.Empty);
            rtnPercent.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(rtnPercent.Text, rtnPercentColor);
        }
        //Max ticket $
        if (dicRicCustomizeColumn.ContainsKey("TodayHighestTransactionAmount"))
        {
            listColumnCustomizes.Add("TodayHighestTransactionAmount");
            var maxTkt = dataItem["TodayHighestTransactionAmount"];
            Color maxTktColor = rowItem["MaxTktColor"].ToString().ToColor();
            maxTkt.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(maxTkt.Text, maxTktColor, true);
        }
        //# Tkts
        if (dicRicCustomizeColumn.ContainsKey("TodayTransactionCount"))
        {
            listColumnCustomizes.Add("TodayTransactionCount");
            var tkt = dataItem["TodayTransactionCount"];
            Color tktColor = rowItem["TktsColor"].ToString().ToColor();
            tkt.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(tkt.Text, tktColor, true);
        }
        //# batch
        if (dicRicCustomizeColumn.ContainsKey("TodayBatchCount"))
        {
            listColumnCustomizes.Add("TodayBatchCount");
            var batch = dataItem["TodayBatchCount"];
            Color batchColor = rowItem["BatchColor"].ToString().ToColor();
            batch.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(batch.Text, batchColor, true);
        }
        //SIC
        if (dicRicCustomizeColumn.ContainsKey("SICCode"))
        {
            listColumnCustomizes.Add("SICCode");
            var sic = dataItem["SICCode"];
            sic.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["SICDescription"].ToString().Replace("&nbsp;", string.Empty)).ToCurrencySymbol();
            sic.HorizontalAlign = HorizontalAlign.Right;
        }
        //*/
        //SC
        if (dicRicCustomizeColumn.ContainsKey("SingleCardTransToday"))
        {
            listColumnCustomizes.Add("SingleCardTransToday");
            var sc = dataItem["SingleCardTransToday"];
            Color scColor = rowItem["DupBinColor"].ToString().ToColor();
            sc.ToolTip = VeraCodeSolution.DoVeraCode(string.Format("{0:C}", rowItem["VolumeOfSimilarCardTransaction"]).ToCurrencySymbol());
            sc.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(sc.Text, scColor, true);
        }
        //PV
        if (dicRicCustomizeColumn.ContainsKey("RulesViolated"))
        {
            listColumnCustomizes.Add("RulesViolated");
            var pv = dataItem["RulesViolated"];
            pv.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["RulesViolated"].ToString());
            pv.Attributes.Add("class", "ellipsis");
        }
        //BA
        if (dicRicCustomizeColumn.ContainsKey("BusinessAge"))
        {
            listColumnCustomizes.Add("BusinessAge");
            var ba = dataItem["BusinessAge"];
            ba.HorizontalAlign = HorizontalAlign.Center;
        }
        //WQ
        if (dicRicCustomizeColumn.ContainsKey("WorkQueueName"))
        {
            listColumnCustomizes.Add("WorkQueueName");
            var wq = dataItem["WorkQueueName"];
            wq.Attributes.Add("class", "ellipsis");
            wq.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["WorkQueueName"].ToString());
        }

        //ARS
        if (dicRicCustomizeColumn.ContainsKey("AttritionScore"))
        {
            listColumnCustomizes.Add("AttritionScore");
            var sc = dataItem["AttritionScore"];
            Color scColor = rowItem["ARSColor"].ToString().ToColor();
            sc.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["AttritionReasonCodes"].ToString());
            sc.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(sc.Text, scColor, true);
        }

        //RRS
        if (dicRicCustomizeColumn.ContainsKey("ReserveScore"))
        {
            listColumnCustomizes.Add("ReserveScore");
            var sc = dataItem["ReserveScore"];
            Color scColor = rowItem["RRSColor"].ToString().ToColor();
            sc.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["ReserveReasonCodes"].ToString());
            sc.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(sc.Text, scColor, true);
        }

        //ACH Return Amount
        if (dicRicCustomizeColumn.ContainsKey("ACHRejects"))
        {
            listColumnCustomizes.Add("ACHRejects");
            var sc = dataItem["ACHRejects"];
            Color scColor = rowItem["ACHReturnAmountColor"].ToString().ToColor();
            sc.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(sc.Text, scColor, true);
        }

        //Auth Decl #
        if (dicRicCustomizeColumn.ContainsKey("DeclinedAuthorizationCount"))
        {
            listColumnCustomizes.Add("DeclinedAuthorizationCount");
            var sc = dataItem["DeclinedAuthorizationCount"];
            Color scColor = rowItem["DeclAuthCountColor"].ToString().ToColor();
            sc.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(sc.Text, scColor, true);
        }

        //MC MTD CB % (MTDMasterCardChargebackCountPercent)
        if (dicRicCustomizeColumn.ContainsKey("MTDMasterCardChargebackCountPercent"))
        {
            listColumnCustomizes.Add("MTDMasterCardChargebackCountPercent");
            riskCustomizeColumn = dicRicCustomizeColumn["MTDMasterCardChargebackCountPercent"];
            var mtdMasterCardChargebackCountPercent = dataItem["MTDMasterCardChargebackCountPercent"];

            Color mtdMasterCardChargebackCountPercentColor = rowItem["MTDMasterCardChargebackCountPercentColor"].ToString().ToColor();

            mtdMasterCardChargebackCountPercent.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["MTDMasterCardChargebackCount"].ToString().ToCurrencySymbol());

            mtdMasterCardChargebackCountPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["MTDMasterCardChargebackCountPercent"].ToString().Trim() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["MTDMasterCardChargebackCountPercent"], riskCustomizeColumn)) : string.Empty);
            mtdMasterCardChargebackCountPercent.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(mtdMasterCardChargebackCountPercent.Text, mtdMasterCardChargebackCountPercentColor);
        }

        // Visa MTD CB % (MTDVisaChargebackCountPercent)
        if (dicRicCustomizeColumn.ContainsKey("MTDVisaChargebackCountPercent"))
        {
            listColumnCustomizes.Add("MTDVisaChargebackCountPercent");
            riskCustomizeColumn = dicRicCustomizeColumn["MTDVisaChargebackCountPercent"];
            var mtdVisaChargebackCountPercent = dataItem["MTDVisaChargebackCountPercent"];

            Color mtdVisaChargebackCountPercentColor = rowItem["MTDVisaChargebackCountPercentColor"].ToString().ToColor();

            mtdVisaChargebackCountPercent.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["MTDVisaChargebackCount"].ToString().ToCurrencySymbol());

            mtdVisaChargebackCountPercent.Text = VeraCodeSolution.DoVeraCode(rowItem["MTDVisaChargebackCountPercent"].ToString().Trim() != string.Empty ?
                VeraCodeSolution.DoVeraCode(FormatValue(rowItem["MTDVisaChargebackCountPercent"], riskCustomizeColumn)) : string.Empty);
            mtdVisaChargebackCountPercent.Text = RiskMCFGeneralFuntionBusiness.FormatBorderText(mtdVisaChargebackCountPercent.Text, mtdVisaChargebackCountPercentColor);
        }

        //ModelScoreValue
        if (dicRicCustomizeColumn.ContainsKey("ModelScoreValue"))
        {
            var rmsGird = dataItem["ModelScoreValue"];
            var rmsItem = rowItem["ModelScoreValue"];
            riskCustomizeColumn = dicRicCustomizeColumn["ModelScoreValue"];
            Color rmsColor = rowItem["ModelScoreValueColor"].ToString().ToColor();
            var rmsText = FormatValue(rmsItem, riskCustomizeColumn);
            rmsGird.ToolTip = rowItem["ModelScoreReason"].ToString();
            rmsGird.Text = GeneralFuncsLib.FormatBorderText(rmsText, rmsColor);
        }

        //ModelAlertValue
        if (dicRicCustomizeColumn.ContainsKey("ModelAlertValue"))
        {
            var rmavGird = dataItem["ModelAlertValue"];
            var rmavItem = rowItem["ModelAlertValue"];
            riskCustomizeColumn = dicRicCustomizeColumn["ModelAlertValue"];
            Color rmavColor = rowItem["ModelAlertValueColor"].ToString().ToColor();
            var rmavText = FormatValue(rmavItem, riskCustomizeColumn);
            rmavGird.ToolTip = rowItem["ModelAlertReason"].ToString();
            rmavGird.Text = GeneralFuncsLib.FormatBorderText(rmavText, rmavColor);
        }

        //process column not exists listColumnCustomizes and have Format
        var data = dicRicCustomizeColumn.Where(x => !string.IsNullOrEmpty(x.Value.CustomFormat)
                                                    && x.Value.IsAutoText == false
                                                    && !listColumnCustomizes.Any(y => y == x.Key)).ToList();
        foreach (var customiozeColumn in data)
        {
            var item = dataItem[customiozeColumn.Key];
            if (item != null)
            {
                item.Text = FormatValue(rowItem[customiozeColumn.Key], customiozeColumn.Value);
            }
        }

        //auto generate text
        var autoColumns = dicRicCustomizeColumn.Where(x => x.Value.IsAutoText
                                                    && !listColumnCustomizes.Any(y => y == x.Key)).ToList();
        foreach (var item in autoColumns)
        {
            if (!string.IsNullOrEmpty(item.Value.Key))
            {
                var columnKey = FormatCell(item, dataItem, rowItem);
                if (!string.IsNullOrEmpty(columnKey))
                    listColumnCustomizes.Add(columnKey);
            }
        }
    }

    public static string FormatCell(KeyValuePair<string, RiskCustomizeColumn> item, GridDataItem dataItem, DataRow rowItem)
    {
        var columnKey = string.Empty;
        var column = dataItem.OwnerTableView.Columns.FindByDataField(item.Value.Key);
        if (column != null && rowItem.Table.Columns.Contains(item.Value.Key))
        {
            columnKey = item.Value.Key;
            var alignVertical = GetVertical(item.Value.ASFormatType);
            var cellAlign = HorizontalAlign.Left;

            if (alignVertical.Equals("right"))
                cellAlign = HorizontalAlign.Right;
            else if (alignVertical.Equals("center"))
                cellAlign = HorizontalAlign.Center;

            var cell = dataItem[item.Value.Key];
            cell.HorizontalAlign = cellAlign;

            var rowData = GeneralFuncsLib.NvlString(rowItem[item.Value.Key]);
            if (!string.IsNullOrEmpty(rowData))
            {
                var columnValue = GetColumnValue(rowItem, item.Value);
                cell.Text = columnValue;
                cell.ToolTip = item.Value.DefaultValue != columnValue ? GetColumnToolTip(rowItem, item.Value) : string.Empty;
                cell.Text = GetColumnBorderColor(cell.Text, rowItem, item.Value);
            }
            else
            {
                cell.Text = item.Value.DefaultValue;
            }
        }

        return columnKey;
    }
    public static string GetColumnValue(DataRow rowItem, RiskCustomizeColumn config)
    {
        var columnValue = config.DefaultValue;

        if (rowItem.Table.Columns.Contains(config.Key))
        {
            var rowData = GeneralFuncsLib.NvlString(rowItem[config.Key]);
            if (!string.IsNullOrEmpty(rowData) && rowData != config.DisplayDashValue)
            {
                columnValue = FormatValue(rowData, config);
            }
        }

        return columnValue;
    }
    public static string GetColumnToolTip(DataRow rowItem, RiskCustomizeColumn config)
    {
        var toolTip = config.ToolTipKey;

        if (!string.IsNullOrEmpty(config.ToolTipKey))
        {
            string pattern = @"{{[a-zA-Z0-9_:#$| ]*}}";
            //var toolTipKeys = config.ToolTipKey;
            RegexOptions options = RegexOptions.Multiline;
            var groups = Regex.Matches(config.ToolTipKey, pattern, options);

            foreach (Match m in groups)
            {
                if (!string.IsNullOrEmpty(m.Value))
                {
                    var originValue = m.Value;
                    var keyValue = originValue.Replace("{{", string.Empty).Replace("}}", string.Empty);
                    var keys = keyValue.Split(':').ToList();

                    if (keys != null && keys.Any())
                    {
                        var fieldName = keys[0];
                        var fieldFormat = keys.Count > 1 ? keys[1] : string.Empty;

                        var toolTipValue = GetToolTipValue(fieldName, fieldFormat, rowItem);
                        toolTip = toolTip.Replace(originValue, toolTipValue);
                    }
                }
            }
        }

        return toolTip;
    }

    private static string GetToolTipValue(string key, string format, DataRow rowItem)
    {
        var toolTip = string.Empty;

        if (rowItem.Table.Columns.Contains(key))
        {
            var toolTipValue = GeneralFuncsLib.NvlString(rowItem[key]);
            var formatType = GetASFormat(format);

            if (formatType != FormatType.None && !string.IsNullOrEmpty(toolTipValue))
            {
                toolTip = VeraCodeSolution.DoVeraCode(FormatValue(toolTipValue, formatType, true));
            }
            else
            {
                if (!string.IsNullOrEmpty(toolTipValue))
                    toolTip = VeraCodeSolution.DoVeraCode(toolTipValue);
            }
        }

        return toolTip;
    }
    public static string GetColumnBorderColor(string value, DataRow rowItem, RiskCustomizeColumn config)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        string cellColor = value;
        var colorKey = config.Key + "Color";

        if (config.HasBorder && rowItem.Table.Columns.Contains(colorKey))
        {
            var colorValue = GeneralFuncsLib.NvlString(rowItem[colorKey]);
            if (!string.IsNullOrEmpty(colorValue))
            {
                Color color = rowItem[colorKey].ToString().ToColor();
                cellColor = FormatBorderText(value, color);
            }
        }

        return cellColor;
    }
    #endregion ---- Process Grid ----

    /// <summary>
    /// gen grid list column setting and column default
    /// </summary>
    /// <returns></returns>
    public static DataTable GetAssignmentCustomizeColumnsToXML()
    {
        return GeneralFuncsLib.GetDataByXML(HttpContext.Current.Server.MapPath("~/App_Data/Risk/AssignmentCustomizeColumns.xml"));
    }

    /// <summary>
    /// gen risk report grid list column setting and column default
    /// </summary>
    /// <returns></returns>
    public static DataTable GetRiskReportCustomizeColumnsToXML()
    {
        return GeneralFuncsLib.GetDataByXML(HttpContext.Current.Server.MapPath("~/App_Data/Risk/RiskReportCustomizeColumns.xml"));
    }

    /// <summary>
    /// gen risk report transaction history grid list column setting and column default
    /// </summary>
    /// <returns></returns>
    public static DataTable GetRiskReportTransactionHistoryCustomizeColumnsToXML()
    {
        return GeneralFuncsLib.GetDataByXML(HttpContext.Current.Server.MapPath("~/App_Data/Risk/RiskReportTransactionHistoryCustomizeColumns.xml"));
    }

    /// <summary>
    /// convert to datatable to list object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static List<T> DataTableToList<T>(DataTable dt)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        var columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
        var objectPropeties = typeof(T).GetProperties(flags);
        var targetList = dt.AsEnumerable().Select(dataRow =>
        {
            var instanceOfT = Activator.CreateInstance<T>();
            foreach (var properties in objectPropeties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
            {
                var value = GeneralFuncsLib.NvlString(dataRow[properties.Name]);
                if (properties.PropertyType.Name == "Boolean")
                    properties.SetValue(instanceOfT, (value == "true" ? true : false), null);
                else if (properties.PropertyType.Name == "Int32")
                    properties.SetValue(instanceOfT, Convert.ToInt32(string.IsNullOrEmpty(value) ? "0" : value), null);
                else
                    properties.SetValue(instanceOfT, value, null);
            }
            return instanceOfT;
        }).ToList();
        return targetList;
    }

    /// <summary>
    /// get language of RiskCustomizeColumnResource
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetResourceValue(string name)
    {
        if (string.IsNullOrEmpty(name))
            return "";
        var resource = Resources.RiskCustomizeColumnResource.ResourceManager;
        if (resource != null)
        {
            var value = resource.GetString(name);
            return !string.IsNullOrEmpty(value) ? value : name;
        }
        return name;
    }

    /// <summary>
    /// FormatBorderText
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string FormatBorderText(string value, Color color, bool checkWhiteColor = false)
    {
        string result = string.Empty;
        if (value.IsNullOrEmpty() || value.Equals("&nbsp;"))
            return result;

        string style;
        if (color.Name.IsNullOrEmpty() || (checkWhiteColor && color.Name.Equals(Color.White.Name)))
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

    /// <summary>
    /// add class ctrl
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="className"></param>
    public static void AddClass(HtmlGenericControl ctrl, string className)
    {
        var current = ctrl.Attributes["class"];
        if (current == null)
        {
            ctrl.Attributes["class"] = className;
        }
        else
        {
            if (current.Contains(className))
            {
                current = current.Replace(className, "").Trim();
            }
            ctrl.Attributes["class"] = (current + " " + className).Trim();
        }
    }

    /// <summary>
    /// remove class of control
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="className"></param>
    public static void RemoveClass(HtmlGenericControl ctrl, string className)
    {
        var current = ctrl.Attributes["class"];
        if (current != null && current.Contains(className))
        {
            current = current.Replace(className, "").Trim();
        }
        ctrl.Attributes["class"] = current;
    }

    /// <summary>
    /// add style color control
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="color"></param>
    public static void AddColor(HtmlGenericControl ctrl, string color)
    {
        ctrl.Style.Add("color", color);
    }

    /// <summary>
    /// add background color of control
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="color"></param>
    public static void AddBackColor(HtmlGenericControl ctrl, string color)
    {
        ctrl.Style.Add("background-color", color);
    }
    /// <summary>
    /// add tooltip of control
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="tooltip"></param>
    public static void AddTooltip(HtmlGenericControl ctrl, string tooltip)
    {
        ctrl.Attributes["title"] = tooltip;
    }
    /// <summary>
    /// add text of control 
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="text"></param>
    public static void AddText(HtmlGenericControl ctrl, string text)
    {
        ctrl.InnerText = text;
    }
    /// <summary>
    /// add innert html of control
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="html"></param>
    public static void AddHtml(HtmlGenericControl ctrl, string html)
    {
        ctrl.InnerHtml = html;
    }

    /// <summary>
    /// format data setting or ASFormat
    /// </summary>
    /// <param name="valueItem"></param>
    /// <param name="format"></param>
    /// <param name="asFormat"></param>
    /// <returns></returns>
    public static string FormatValue(object valueItem, RiskCustomizeColumn riskCustomizeColumn)
    {
        return FormatValue(valueItem, riskCustomizeColumn, true);
    }
    public static string FormatValue(object valueItem, RiskCustomizeColumn riskCustomizeColumn, bool isRemoveNegative)
    {
        var result = string.Empty;
        var value = GeneralFuncsLib.NvlString(valueItem);
        if (!string.IsNullOrEmpty(value))
        {
            if (!string.IsNullOrEmpty(riskCustomizeColumn.CustomFormat))
            {
                if (riskCustomizeColumn.CustomFormat.Contains("0:"))
                    result = string.Format(riskCustomizeColumn.CustomFormat, Convert.ToDecimal(value));
                else
                    result = string.Format("{0:" + riskCustomizeColumn.CustomFormat + "}", Convert.ToDecimal(value));


                if (!string.IsNullOrEmpty(riskCustomizeColumn.ASFormat))
                {
                    switch (riskCustomizeColumn.ASFormatType)
                    {
                        case FormatType.Percentage:
                        case FormatType.Percentage0Digits:
                        case FormatType.Percentage4Digits:
                            result = result + "%";
                            if (Convert.ToDecimal(value) < 0)
                                result = !isRemoveNegative ? result : "<span class=\"negative\">(" + result.Replace("-", "") + ")<span>";
                            break;
                        case FormatType.Currency4Digits:
                        case FormatType.Currency:
                            result = "$" + result;
                            if (Convert.ToDecimal(value) < 0)
                                result = !isRemoveNegative ? result : "<span class=\"negative\">(" + result.Replace("-", "") + ")<span>";
                            break;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(riskCustomizeColumn.ASFormat))
                {
                    switch (riskCustomizeColumn.ASFormatType)
                    {
                        case FormatType.Integer:
                            result = FormatData.FormatInteger(Convert.ToDecimal(value));
                            break;
                        case FormatType.Percentage:
                            result = FormatData.FormatPercent(value);
                            break;
                        case FormatType.Percentage0Digits:
                            result = FormatData.FormatPercent(value, 0);
                            break;
                        case FormatType.Percentage4Digits:
                            result = FormatData.FormatPercent(value, 4);
                            break;
                        case FormatType.Currency:
                            result = FormatData.FormatCurrency(value).ToCurrencySymbol();
                            break;
                        case FormatType.Currency4Digits:
                            result = FormatData.FormatCurrency(value, 4);
                            break;
                        case FormatType.Date:
                            result = FormatData.FormatDate(value);
                            break;
                        case FormatType.DateAndTime:
                            result = FormatData.FormatDate(value);
                            break;
                        case FormatType.Time:
                            result = value;
                            break;
                        case FormatType.Number2Digit:
                            var number = decimal.Parse(value);
                            result = string.Format("{0:#,0.00;#,0.00}", number);
                            break;
                        default:
                            result = value;
                            break;
                    }
                }
            }

        }
        if (string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(riskCustomizeColumn.DefaultValue))
            return riskCustomizeColumn.DefaultValue;
        return result;
    }
    public static string FormatValue(object valueItem, FormatType format, bool isTooltip = false)
    {
        var result = string.Empty;
        var value = GeneralFuncsLib.NvlString(valueItem);
        if (!string.IsNullOrEmpty(value))
        {
            switch (format)
            {
                case FormatType.Integer:
                    result = FormatData.FormatInteger(Convert.ToDecimal(value));
                    break;
                case FormatType.Percentage:
                    result = FormatData.FormatPercent(value);
                    break;
                case FormatType.Percentage0Digits:
                    result = FormatData.FormatPercent(value, 0);
                    break;
                case FormatType.Percentage4Digits:
                    result = FormatData.FormatPercent(value, 4);
                    break;
                case FormatType.Currency:
                    result = isTooltip ? GeneralFuncsLib.FormatCurrencyTooltip(value, SessionManager.CurrencyFortmat) : FormatData.FormatCurrency(value).ToCurrencySymbol();
                    break;
                case FormatType.Currency4Digits:
                    result = FormatData.FormatCurrency(value, 4);
                    break;
                case FormatType.Date:
                    result = FormatData.FormatDate(value);
                    break;
                case FormatType.DateAndTime:
                    result = FormatData.FormatDate(value);
                    break;
                case FormatType.Time:
                    result = value;
                    break;
                default:
                    result = value;
                    break;
            }
        }

        return result;
    }
    /// <summary>
    /// add column gen Export
    /// </summary>
    /// <param name="asGrid"></param>
    /// <param name="isExportFullView"></param>
    public static void NeedExportGridConfig(ASGrid asGrid, bool isExportFullView, bool hasRQColumn, bool isDistinctQueue)
    {
        List<GridColumn> listCols = new List<GridColumn>();
        foreach (GridColumn col in asGrid.MasterTableView.Columns)
        {
            col.HeaderText = Regex.Replace(col.HeaderText, @"<[a-zA-Z\/].*?>", string.Empty);
            if (isExportFullView)
            {
                if (col.UniqueName.Equals("MerchantName") || col.UniqueName.Equals("MerchantNumber"))
                    listCols.Add(col);
            }
            else if (col.Display && col.Visible)
            {
                listCols.Add(col);
            }

            if (!isDistinctQueue && col.UniqueName.Equals("CurrentStatusDesc") || (hasRQColumn && col.UniqueName.Equals("IsRequeuedDesc")))
            {
                listCols.Add(col);
            }
        }

        asGrid.MasterTableView.Columns.Clear();
        if (!isExportFullView)
        {
            listCols = listCols.OrderBy(c => c.OrderIndex).ToList();
            foreach (GridColumn col in listCols)
            {
                if (col != null)
                {
                    asGrid.MasterTableView.Columns.Add(col);

                }
            }
        }

        if (isExportFullView)
        {
            //call SPA get Column Parameter
            GenGridAssignmentCustomizeColumns(asGrid, 0, true, true, hasRQColumn, isDistinctQueue);

        }
    }

    public static void GenerateTooltipHeaderColumn(ASGrid targetGrid, string tagertControlId, string toolTipContent)
    {
        RadToolTip headerTooltip = new RadToolTip();
        headerTooltip.TargetControlID = tagertControlId;
        headerTooltip.Text = string.Format("<span class='radtooltip-content'>{0}</span>", toolTipContent);
        headerTooltip.RenderMode = RenderMode.Lightweight;
        headerTooltip.CssClass = "customview-tooltip";
        headerTooltip.RelativeTo = ToolTipRelativeDisplay.Element;
        headerTooltip.Position = ToolTipPosition.BottomCenter;
        headerTooltip.AutoCloseDelay = 0;
        headerTooltip.HideDelay = 0;
        headerTooltip.IsClientID = true;
        targetGrid.Controls.Add(headerTooltip);
    }

    public static void GenerateTooltipHeaderColumnRepeter(AS.Controls.Global.ASRepeater repeter, string tagertControlId, string toolTipContent)
    {
        RadToolTip headerTooltip = new RadToolTip();
        headerTooltip.TargetControlID = tagertControlId;
        headerTooltip.Text = string.Format("<span class='radtooltip-content'>{0}</span>", toolTipContent);
        headerTooltip.RenderMode = RenderMode.Lightweight;
        headerTooltip.CssClass = "customview-tooltip";
        headerTooltip.RelativeTo = ToolTipRelativeDisplay.Element;
        headerTooltip.Position = ToolTipPosition.BottomCenter;
        headerTooltip.AutoCloseDelay = 0;
        headerTooltip.HideDelay = 0;
        headerTooltip.IsClientID = true;
        repeter.Controls.Add(headerTooltip);
    }

    public static void GenerateTooltipHeaderDefaulColumn(GridColumn targetColumn, string headerText, ASGrid targetGrid, string tagertControlId, string toolTipContent)
    {
        targetColumn.HeaderTooltip = string.Empty;
        targetColumn.HeaderText = "<span id='" + headerText
            + "'>" + headerText + "</span>";
        RadToolTip headerTooltip = new RadToolTip();
        headerTooltip.TargetControlID = tagertControlId;
        headerTooltip.Text = string.Format("<span class='radtooltip-content'>{0}</span>", toolTipContent); ;
        headerTooltip.RenderMode = RenderMode.Lightweight;
        headerTooltip.CssClass = "customview-tooltip";
        headerTooltip.RelativeTo = ToolTipRelativeDisplay.Element;
        headerTooltip.Position = ToolTipPosition.BottomCenter;
        headerTooltip.AutoCloseDelay = 0;
        headerTooltip.HideDelay = 0;
        headerTooltip.IsClientID = true;
        targetGrid.Controls.Add(headerTooltip);
    }

    public static void GenerateTooltipHeaderDefaulColumnID(GridColumn targetColumn, string headerText, ASGrid targetGrid, string tagertControlId, string toolTipContent, ToolTipPosition position)
    {
        GenerateTooltipHeader(targetColumn, headerText, targetGrid, tagertControlId, toolTipContent, position);
    }
    public static void GenerateTooltipHeaderDefaulColumnID(GridColumn targetColumn, string headerText, ASGrid targetGrid, string tagertControlId, string toolTipContent)
    {
        GenerateTooltipHeader(targetColumn, headerText, targetGrid, tagertControlId, toolTipContent, ToolTipPosition.BottomCenter);
    }
    private static void GenerateTooltipHeader(GridColumn targetColumn, string headerText, ASGrid targetGrid, string tagertControlId, string toolTipContent, ToolTipPosition position)
    {
        targetColumn.HeaderTooltip = string.Empty;
        targetColumn.HeaderText = "<span id='" + tagertControlId
            + "'>" + headerText + "</span>";
        RadToolTip headerTooltip = new RadToolTip();
        headerTooltip.TargetControlID = tagertControlId;
        headerTooltip.Text = string.Format("<span class='radtooltip-content'>{0}</span>", toolTipContent); ;
        headerTooltip.RenderMode = RenderMode.Lightweight;
        headerTooltip.CssClass = "customview-tooltip";
        headerTooltip.RelativeTo = ToolTipRelativeDisplay.Element;
        headerTooltip.Position = position;
        headerTooltip.AutoCloseDelay = 0;
        headerTooltip.HideDelay = 0;
        headerTooltip.IsClientID = true;
        targetGrid.Controls.Add(headerTooltip);
    }
    public static void GenerateWorkPopover(Dictionary<string, string> dicAttrToolTip, DataTable dataDisposition, HtmlGenericControl workItem, WebSiteEnums.PageSectionEnums pageSection)
    {
        string title = GetResourceValue("Barometer_WorkPopoverText");
        foreach (var item in dicAttrToolTip)
        {
            workItem.Attributes.Add(item.Key, item.Value);
        }

        var workItemID = Guid.NewGuid().ToString();
        bool isWIPByOther = dicAttrToolTip["CurrentStatus"] == WebSiteEnums.WorkStatus.WorkInProgressByOther.ToString();
        string textPopover = isWIPByOther ? dicAttrToolTip["WorkingStatusMessage"] : RM_MCF_GeneralFuncsLib.GetDataWorkPopover(title, int.Parse(dicAttrToolTip["WorkStateID"].ToString()),
            dataDisposition, workItemID, dicAttrToolTip["MerchantNumber"].ToString());
        workItem.Attributes.Add("id", workItemID);
        RadToolTip uxRadTooltipWork = new RadToolTip();
        uxRadTooltipWork.TargetControlID = workItemID;
        uxRadTooltipWork.Text = textPopover;
        uxRadTooltipWork.HideEvent = ToolTipHideEvent.LeaveTargetAndToolTip;
        uxRadTooltipWork.ShowEvent = ToolTipShowEvent.OnMouseOver;
        uxRadTooltipWork.AutoCloseDelay = 0;
        uxRadTooltipWork.RenderMode = RenderMode.Lightweight;
        uxRadTooltipWork.CssClass = pageSection == WebSiteEnums.PageSectionEnums.SecurityReport && isWIPByOther ? "ml-20 wip-other" : pageSection == WebSiteEnums.PageSectionEnums.SecurityReport && !isWIPByOther ? "ml-20 marker-diposition" :
            isWIPByOther ? "wip-other" : "marker-diposition";
        uxRadTooltipWork.OnClientBeforeShow = isWIPByOther ? string.Empty : "workPopBeforeShow";
        uxRadTooltipWork.OnClientBeforeHide = isWIPByOther ? "wipOtherHideShow" : "workPopBeforeHide";
        uxRadTooltipWork.OnClientShow = isWIPByOther ? "wipOtherHideShow" : "workPopShow";
        uxRadTooltipWork.RelativeTo = ToolTipRelativeDisplay.Element;
        uxRadTooltipWork.Position = pageSection == WebSiteEnums.PageSectionEnums.SecurityReport ? ToolTipPosition.TopCenter : isWIPByOther ? ToolTipPosition.TopCenter : ToolTipPosition.MiddleRight;
        uxRadTooltipWork.IsClientID = true;
        uxRadTooltipWork.ShowCallout = !isWIPByOther;
        workItem.Controls.Add(uxRadTooltipWork);
    }

    public static string GetDataWorkPopover(string title, int workStateID, DataTable dataSource, string targetID, string checkboxName)
    {
        return GetDataWorkPopover(title, workStateID, dataSource, targetID, checkboxName, false);
    }
    public static string GetDataWorkPopover(string title, int workStateID, DataTable dataSource, string targetID, string checkboxName, bool isNexQueue)
    {
        string result = string.Empty;
        string dispositionItems = string.Empty;
        bool isDisposition = (workStateID == 2);
        bool isWorkProgress = (workStateID == 1);
        bool isDisable = false;
        bool isDefaultValue = false;

        foreach (DataRow item in dataSource.Rows)
        {
            isDefaultValue = item["IsDefault"].ToString().ToLower().Equals("true");
            isDisable = (!isNexQueue && item["Checked"].ToBoolean() && !item["IsActive"].ToBoolean()) ? true : false;
            dispositionItems += "<li" + (isDisable ? " class='item-disable'" : string.Empty) + ">" +
                "<div class='display-flex'><div class='display-flex disposition-content'>" +
                    "<input onchange='changeWorkedState(this,3);' type='Checkbox' " +
                      (isDefaultValue ? "isdefault='true' " : string.Empty) +
                    ((item["Checked"].ToString().ToLower().Equals("true")) ? "checked" : string.Empty) +
                    " name='" + item["DispositionName"].ToString() +
                    "' value='" + item["DispositionID"].ToString() + "'>" +
                    "<span class='ml-5' onclick='changeWorked(this);'>" + item["DispositionName"].ToString() +
                    (isDefaultValue ? "<span class='lbl-default'>" + GetResourceValue("DefaultValueResource_Text") + "</span>" : string.Empty) + "</span></div></li>";
        }

        result +=
           "<div class='disposition-dialog' data-selector='disposition-dialog' targetid='" + targetID + "'>" +
             "<div class='work-progress' data-selector='work-progress'>" +
                 "<div class='title'>" +
                   "<span>" + title + "</span>" +
                   (isNexQueue ? string.Empty : "<button class='btn-submit-disposition btn-disabled' onclick='saveDisposition(this); return false;'>" + GetResourceValue("btnSubmitResource") + "</button>") +
                 "</div>" +
                 "<div class='display-flex mt-10'> <div class='display-flex ml-10'onclick='changeWorkedState(this,1);'>" +
                 "<input type='radio' class='rd-work' data-selector='rd-work' name='" + checkboxName + "' value='WIP' " + (isWorkProgress ? "checked" : string.Empty) + "><span class='text-opt'>" + GetResourceValue("WorkInProgress_Text") + "</span>" +
                 "</div></div>" +
             "</div>" +
             "<div class='work-disposition' data-selector='work-disposition'>" +
               "<div class='display-flex mt-10'> <div class='display-flex ml-10' onclick='changeWorkedState(this,2);'>" +
               "<input type='radio' class='rd-dis' data-selector='rd-dis' name='" + checkboxName + "' value='Disposition'" + (isDisposition ? "checked" : string.Empty) + "><span class='text-opt'>" + GetResourceValue("DispositionWork_Text") + "</span>" +
               "</div></div>" +
               "<ul>" +
                    dispositionItems +
               "</ul>" +
             "</div>" +
           "</div>";
        return result;
    }

    public static void KeepAliveWIP()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        WebServices.RiskServices.GetReports("spa_RM_MCF_KeepSessionActive", parameters);
    }

    public static void ClearWIPStatus()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        WebServices.RiskServices.GetReports("spa_RM_MCF_ClearSession", parameters);
    }

    public static int CreateTemporaryWorkQueueSession(DateTime reportDate, int assignmentID, string applyFilterId, WebSiteEnums.PAGE_CODE pageMode)
    {
        FilterParameterCollection parameterList = new FilterParameterCollection();
        FilterParameterCollection outParameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
        parameterList.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int64));
        parameterList.Add(new FilterParameter("@ApplyFilterId", applyFilterId, DbType.String));
        parameterList.Add(new FilterParameter("@SessionID", 0, DbType.Int32, true));

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_WQ_AddTemporaryAssignmentToStaging", parameterList, out outParameterList);
        int sessionID = 0;
        if (outParameterList != null && outParameterList.Count > 0)
        {
            sessionID = outParameterList[0].ParameterValue.ToInt();
        }

        switch (pageMode)
        {
            case WebSiteEnums.PAGE_CODE.NQ:
                RiskSessionManager.MCF_DQTemporaryNextQueue.RequeueSessionID = sessionID;
                break;
            case WebSiteEnums.PAGE_CODE.DQ:
                RiskSessionManager.MCF_DQTemporaryBarometerReport.RequeueSessionID = sessionID;
                RiskSessionManager.MCF_DQTemporaryBarometerReport.IsRequeueAll = false;
                RiskSessionManager.MCF_DQTemporaryBarometerReport.RequeuedMerchantList.Clear();
                break;
            case WebSiteEnums.PAGE_CODE.SR:
                RiskSessionManager.MCF_DQTemporarySecurityReport.RequeueSessionID = sessionID;
                RiskSessionManager.MCF_DQTemporarySecurityReport.IsRequeueAll = false;
                RiskSessionManager.MCF_DQTemporarySecurityReport.RequeuedMerchantList.Clear();
                break;
        }

        return sessionID;
    }

    public static string DataSetToJSON(DataSet ds)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        foreach (DataTable dt in ds.Tables)
        {
            object[] arr = new object[dt.Rows.Count + 1];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                arr[i] = dt.Rows[i].ItemArray;
            }
            dict.Add(dt.TableName, arr);
        }
        JavaScriptSerializer json = new JavaScriptSerializer();
        return json.Serialize(dict);
    }

    public static string DataTableToJSON(DataTable dt)
    {
        List<Dictionary<string, object>> dict = new List<Dictionary<string, object>>();
        Dictionary<string, object> childRow;
        foreach (DataRow row in dt.Rows)
        {
            childRow = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                childRow.Add(col.ColumnName, row[col]);
            }

            dict.Add(childRow);
        }
        JavaScriptSerializer json = new JavaScriptSerializer();
        return json.Serialize(dict);
    }

    public static DataTable UpdateMerchantWorked(string dispositionList, string workStateID, string feWorkStateID, string cycleID, string ParentCycleID,
        string reportDate, string assignmentID, string merchantNumber, string viewCode)
    {
        var parameters = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.AddLanguageID();

        parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
        parameters.Add(new FilterParameter("@ViewCode", viewCode, DbType.String));
        if (!string.IsNullOrEmpty(dispositionList))
        {
            parameters.Add(new FilterParameter("@DispositionList", dispositionList, DbType.String));
        }
        parameters.Add(new FilterParameter("@ReportDate", Convert.ToDateTime(reportDate), DbType.Date));
        parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
        parameters.Add(new FilterParameter("@CycleID", int.Parse(cycleID), DbType.Int32));
        parameters.Add(new FilterParameter("@ParentCycleID", int.Parse(ParentCycleID), DbType.Int32));
        parameters.Add(new FilterParameter("@WorkStateID", int.Parse(workStateID), DbType.Int32));
        parameters.Add(new FilterParameter("@FEWorkStateID", int.Parse(feWorkStateID), DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_UpdateMerchantWorked", parameters);
    }

    public static DataTable GetDipositionDiaLog(string workingMerchantID, string reportDate)
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@WorkingMerchantID", int.Parse(workingMerchantID), DbType.Int32));
        parameters.Add(new FilterParameter("@ReportDate", Convert.ToDateTime(reportDate), DbType.Date));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetDispositionForReport", parameters);
    }

    public static string CheckDisposition(string Id, string reportDate)
    {
        DataSet ds = new DataSet();

        ds.Tables.Add(GetDipositionDiaLog(Id, reportDate));

        return DataSetToJSON(ds);
    }

    public static string UpdateDisposition(string dispositionList, string workStateID, string feWorkStateID, string cycleID, string ParentCycleID,
        string reportDate, string assignmentID, string merchantNumber, string viewCode)
    {
        DataSet ds = new DataSet();
        DataTable result = UpdateMerchantWorked(dispositionList, workStateID, feWorkStateID, cycleID, ParentCycleID, reportDate,
            assignmentID, merchantNumber, viewCode);
        ds.Tables.Add(result);
        //update session NextQueue
        if (viewCode == "NQ" && result.Rows.Count > 0)
        {
            var currentRMMCFNextQueue = RiskSessionManager.MCF_CurrentNextQueue;
            if (currentRMMCFNextQueue != null && currentRMMCFNextQueue.MerchantInfo.Rows.Count > 0)
            {
                currentRMMCFNextQueue.MerchantInfo.Rows[0]["Worked"] = result.Rows[0]["Worked"];
                currentRMMCFNextQueue.MerchantInfo.Rows[0]["ParametersWorked"] = result.Rows[0]["ParametersWorked"];
                RiskSessionManager.MCF_CurrentNextQueue = currentRMMCFNextQueue;
            }
        }
        return DataSetToJSON(ds);
    }

    public static string GetAssignmentsForDetectionQueue(string assignmentID, string reportDate, string applyFilterId)
    {
        DataTable assignmentList = new DataTable();
        if (!string.IsNullOrEmpty(assignmentID) && Int32.Parse(assignmentID) > 0)
        {
            FilterParameterCollection parameterList = new FilterParameterCollection();
            parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameterList.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
            parameterList.Add(new FilterParameter("@ReportDate", Convert.ToDateTime(reportDate), DbType.Date));
            parameterList.Add(new FilterParameter("@ApplyFilterId", applyFilterId, DbType.String));
            parameterList.Add(new FilterParameter("@GroupBy", "Assignment", DbType.String));
            parameterList.Add(new FilterParameter("@Mode", "0", DbType.String));
            string spName = "spa_RM_MCF_Get_Assignment";
            assignmentList = WebServices.RiskServices.GetReports(spName, parameterList);
        }

        return DataTableToJSON(assignmentList);
    }

    public static string BuildUrlRequeue(bool isAdd, DateTime reportDate, int assignmentID,
        WebSiteEnums.MCF_MerchantWorkingStatus filterWorkingStatus, string applyFilterId,
        WebSiteEnums.PAGE_CODE pageMode)
    {
        ReportPage page = new ReportPage();
        var url = string.Format("IsAddToQueue={0}&ReportDate={1}&AssignmentID={2}&FilterWorkingStatus={3}&ApplyFilterId={4}&PageMode={5}",
            isAdd, reportDate, assignmentID, filterWorkingStatus, applyFilterId, pageMode);
        string encodeURL = string.Format("rm_MCF_DQAddToQueueModal.aspx?{0}", page.BuildSecureQueryString(url));
        string method = pageMode == WebSiteEnums.PAGE_CODE.NQ ? "AddOrRemoveWorkQueue" : "ShowPopupModal";
        return string.Format("return {0}('" + encodeURL + "', 'auto'); return false;", method);
    }

    public static string[] UpdateRequeuedMerchant(int requeueSessionID, string status, string merchantNumber, string cycleId, string parentCycleID, WebSiteEnums.PAGE_CODE pageMode, string applyFilteredId)
    {
        bool isRequeued = Convert.ToBoolean(status);
        FilterParameterCollection parameterList = new FilterParameterCollection();
        FilterParameterCollection outParameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.Add(new FilterParameter("@SessionID", requeueSessionID, DbType.Int64));
        parameterList.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.AnsiString));
        parameterList.Add(new FilterParameter("@Included", isRequeued, DbType.Boolean));
        parameterList.Add(new FilterParameter("@CycleID", cycleId, DbType.Int32));
        parameterList.Add(new FilterParameter("@ParentCycleID", parentCycleID, DbType.Int32));
        parameterList.Add(new FilterParameter("@ApplyFilterID", applyFilteredId, DbType.String));

        parameterList.Add(new FilterParameter("@MerchantCount", 0, DbType.Int32, true));
        parameterList.Add(new FilterParameter("@TotalVolume", 0, DbType.Currency, true));
        parameterList.Add(new FilterParameter("@IsRequeued", 0, DbType.Boolean, true));

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_Check_RequeuedMerchant", parameterList, out outParameterList);

        bool isAllSelected = false;
        var merchantCount = outParameterList[0].ParameterValue.ToString();
        var totalVolume = outParameterList[1].ParameterValue.ToString();
        var merchantIsWorkQueued = outParameterList[2].ParameterValue.ToString();
        switch (pageMode)
        {
            case WebSiteEnums.PAGE_CODE.DQ:
                if (isRequeued)
                {
                    RiskSessionManager.MCF_DQTemporaryBarometerReport.RequeuedMerchantList.Add(merchantNumber);
                }
                else
                {
                    RiskSessionManager.MCF_DQTemporaryBarometerReport.RequeuedMerchantList.Remove(merchantNumber);
                }
                isAllSelected = RiskSessionManager.MCF_DQTemporaryBarometerReport.TotalAlertMerchant == merchantCount.ToInt();
                break;
            case WebSiteEnums.PAGE_CODE.SR:
                if (isRequeued)
                {
                    RiskSessionManager.MCF_DQTemporarySecurityReport.RequeuedMerchantList.Add(merchantNumber);
                }
                else
                {
                    RiskSessionManager.MCF_DQTemporarySecurityReport.RequeuedMerchantList.Remove(merchantNumber);
                }

                isAllSelected = RiskSessionManager.MCF_DQTemporarySecurityReport.TotalAlertMerchant == merchantCount.ToInt();
                break;
        }

        return new string[] { merchantCount, totalVolume, isAllSelected.ToString(), merchantIsWorkQueued };
    }

    public static string[] UpdateAllRequeuedMerchant(int requeueSessionID, string status, int filterWorkingStatus, WebSiteEnums.PAGE_CODE pageMode, string applyFilteredId)
    {
        bool isRequeued = Convert.ToBoolean(status);
        FilterParameterCollection parameterList = new FilterParameterCollection();
        FilterParameterCollection outParameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.Add(new FilterParameter("@SessionID", requeueSessionID, DbType.Int64));
        parameterList.Add(new FilterParameter("@IsAllMerchantChecked", isRequeued, DbType.Boolean));
        parameterList.Add(new FilterParameter("@FilterMode", filterWorkingStatus, DbType.Int32));
        parameterList.Add(new FilterParameter("@ApplyFilterID", applyFilteredId, DbType.String));


        parameterList.Add(new FilterParameter("@MerchantCount", 0, DbType.Int32, true));
        parameterList.Add(new FilterParameter("@TotalVolume", 0, DbType.Currency, true));
        parameterList.Add(new FilterParameter("@IsRequeued", 0, DbType.Boolean, true));

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_Check_RequeuedMerchant", parameterList, out outParameterList);

        var merchantCount = outParameterList[0].ParameterValue.ToString();
        var totalVolume = outParameterList[1].ParameterValue.ToString();
        var isWorkQueued = outParameterList[2].ParameterValue.ToString();

        if (!isRequeued)
        {
            merchantCount = "0";
            totalVolume = "0";
        }

        switch (pageMode)
        {
            case WebSiteEnums.PAGE_CODE.DQ:
                RiskSessionManager.MCF_DQTemporaryBarometerReport.IsRequeueAll = isRequeued;
                RiskSessionManager.MCF_DQTemporaryBarometerReport.RequeuedMerchantList.Clear();
                break;
            case WebSiteEnums.PAGE_CODE.SR:
                RiskSessionManager.MCF_DQTemporarySecurityReport.IsRequeueAll = isRequeued;
                RiskSessionManager.MCF_DQTemporarySecurityReport.RequeuedMerchantList.Clear();
                break;
        }

        return new string[] { merchantCount.ToString(), totalVolume.ToString(), isWorkQueued };
    }

    public static DataTable GetMerchantListForRequeue(DateTime reportDate, int assignmentID, int requeueSessionID, bool isRequeueAll, WebSiteEnums.MCF_MerchantWorkingStatus filterWorkingStatus, bool isAddQueue)
    {
        string spaName = "spa_RM_MCF_Get_RequeuedMerchant";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ReportDate", reportDate, DbType.Date));
        parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int64));
        parameters.Add(new FilterParameter("@SessionID", requeueSessionID, DbType.Int64));
        parameters.Add(new FilterParameter("@StyleID", 1, DbType.Int64));
        parameters.Add(new FilterParameter("@IsCheckAll", isRequeueAll, DbType.Boolean));
        parameters.Add(new FilterParameter("@Mode", (int)filterWorkingStatus, DbType.Int64));
        parameters.Add(new FilterParameter("@IsAdded", isAddQueue ? 1 : 0, DbType.Int16));

        DataTable merchantLists = WebServices.RiskServices.GetReports(spaName, parameters);

        return merchantLists;
    }

    public static List<string> ExtendCustomColumn()
    {
        var configValue = GeneralFuncsLib.GetDataOfExtendedSetting("ExtendCustomColumn");
        if (!string.IsNullOrEmpty(configValue))
            return configValue.Split(',').ToList();
        return null;
    }

    public static int MoveDataFromFinalTables(int assignmentID, int styleID = 1)
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@SrcAssignmentID", assignmentID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@StyleID", styleID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@StgAssignmentID", 0, DbType.Int32, true));
        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_Copy_Assignment", paramsIn, out paramsOut);
        int temporaryID = Int32.Parse(paramsOut[0].ParameterValue.ToString());
        return temporaryID;
    }
    #endregion ----- Public Methods ----
}