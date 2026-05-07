using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.VW.Common;
using AS.WS.Data;
using AS.WS.Entities;
using DDS.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace AS.WS.Business
{
    public class ReportingBusiness
    {
        public void InitializeForCS(string connString)
        {
            _ReportingDao = new CsReportingDao(connString);
        }
        public void InitializeForMS(string connString)
        {
            _ReportingDao = new MsReportingDao(connString);
        }
        public void InitializeForRisk(string connString)
        {
            _ReportingDao = new RiskDao(connString);
        }
        private IReportingDao _ReportingDao = null;
        private Dictionary<string, string> _resources = null;

        public DataTable GetReports(string spName, FilterParameterCollection _paramaters)
        {
            return _ReportingDao.GetReports(spName, _paramaters);
        }

        public IDataReader GetReportsByDataReader(string spName, FilterParameterCollection _paramaters)
        {
            return _ReportingDao.GetReportsAsDataReader(spName, _paramaters);
        }

        public int ExecuteNonQueryCommand(string spName, FilterParameterCollection _paramaters, out FilterParameterCollection OutputParams)
        {
            return _ReportingDao.ExecuteNonQueryCommand(spName, _paramaters, out OutputParams);
        }

        public DataSet ExecuteQueryCommand(string spName, FilterParameterCollection _paramaters, out FilterParameterCollection OutputParams)
        {
            return _ReportingDao.ExecuteQueryCommand(spName, _paramaters, out OutputParams);
        }

        public DataSet GetReportsAsDataSet(string spName, FilterParameterCollection _paramaters)
        {
            return _ReportingDao.GetReportsAsDataSet(spName, _paramaters);
        }

        public void ExportFlatReport(DataSet dataSource, string filePath, string fileName, string resources, string currencyFormat, bool hasRQColumn, string columnsCustomView)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            
            filePath = Path.Combine(filePath, fileName);
            
            FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);

            var _ExportData = new StringBuilder();
            _ExportData.Append("<html><meta http-equiv=\"content-type\" content=\"application/xhtml+xml; charset=UTF-8\" /><body>");

            Dictionary<string, string> dicRes = JsonConvert.DeserializeObject<Dictionary<string, string>>(resources);

            Dictionary<string, CustomizeColumn> dicColumnCustomView = null;
            if (!string.IsNullOrEmpty(columnsCustomView))
                dicColumnCustomView = JsonConvert.DeserializeObject<Dictionary<string, CustomizeColumn>>(columnsCustomView);

            DataTable tblMerchant = dataSource.Tables[0];
            foreach (DataRow row in tblMerchant.Rows)
            {
                string merchantNumber = row["MerchantNumber"].ToString();

                //Get Data For Exporting data
                DataRow[] merchantInfo = dataSource.Tables[0].Select(string.Format("MerchantNumber = '{0}'", merchantNumber));
                DataRow[] barometerList = dataSource.Tables[1].Select(string.Format("MerchantNumber = '{0}'", merchantNumber));
                DataRow[] transactionList = dataSource.Tables[2].Select(string.Format("MerchantNumber = '{0}'", merchantNumber));
                DataRow[] chargebackList = dataSource.Tables[3].Select(string.Format("MerchantNumber = '{0}'", merchantNumber));

                if (dicColumnCustomView != null)
                    _ExportData = _ExportData.Append("<div style='font-weight:bold;font-size:20px'>" + merchantInfo[0]["MerchantName"].ToString() + "</div>");
                _ExportData = _ExportData.Append("<div style='font-weight:bold;font-size: 18px;'>" + dicRes["litGridTitle2.Text"] + "</div>");

                //Merchant Info
                _ExportData = _ExportData.Append(GetMerchantInfoHTMLTemplate(merchantInfo, dicRes, currencyFormat, barometerList));
                _ExportData = _ExportData.Append("<div>&nbsp;</div>");

                //First Column: Barometer Report
                //Barometer Report
                #region Barometer Report table
                _ExportData = _ExportData.Append("<table width='100%'><tr><td valign='top'>");
                _ExportData = _ExportData.Append("<div style='font-weight:bold;font-size: 18px;'>" + dicRes["litHeaderBarometerReportResource1.Text"] + "</div>");
                _ExportData = _ExportData.Append(GetBarometerHTML(barometerList
                    , "BusinessAge,RiskScore,VolumePercent,ContractualVolume,AverageTicketPercent,AuthorizationPercent,DeclinedAuthorizationPercent,RepeatAuthorizationCount," +
                            "TodayForeignCardCount,KeyPercent,EvenDollarTransactionPercent,DuplicateDollarTransactionPercent,DuplicateBin,NegativeBatchCount," +
                            "ZeroBatchCount,TodayVolume,TodayFirstTimeRetrievalVolume,TodayChargebackVolume,ReturnPercent,TodayHighestTransactionAmount," +
                            "TodayTransactionCount,TodayBatchCount,SingleCardTransToday,SICCode,ProfileDescription,AttritionScore,ReserveScore,ACHRejects," +
                            "DeclinedAuthorizationCount"
                    , currencyFormat, dicColumnCustomView));
                _ExportData = _ExportData.Append("</td></tr></table>");
                _ExportData = _ExportData.Append("<div>&nbsp;</div>");
                #endregion

                #region Chargeback table
                //First Column: Chargeback List; ACH Rejects; Daily FC Analysis
                _ExportData = _ExportData.Append("<table width='100%'><tr><td valign='top'>");
                //Chargeback List
                _ExportData = _ExportData.Append("<div style='font-weight:bold;font-size: 18px;'>" + dicRes["litHeaderChargeback90daysResource1.Text"] + "</div>");
                _ExportData = _ExportData.Append(GetChargebackHTML(chargebackList,
                    "PartialAccountNumber,CardType,ReportDate,TransactionDate,ReasonCode,Keyed,TransactionAmount",
                    dicRes, currencyFormat));
                _ExportData = _ExportData.Append("</td></tr></table>");
                _ExportData = _ExportData.Append("<div>&nbsp;</div>");
                #endregion

                #region Transaction table
                //Second Column: Transaction List
                //Transaction List
                //Update export column value Transaction Description instead of Transaction Code, OP #34817
                _ExportData = _ExportData.Append("<table width='100%'><tr><td valign='top'>");
                _ExportData = _ExportData.Append("<div style='font-weight:bold;font-size: 18px;'>" + dicRes["litHeaderTransactionTodayResource1.Text"] + "</div>");
                _ExportData = _ExportData.Append(GetTransactionHTML(transactionList,
                    "PartialAccountNumber,CardType,CountryCode,FileSource,TransactionDate,TransactionTime,DupeCount,AuthorizationNumber,Keyed,TransactionDescription,MatchCode,TransactionAmount",
                    dicRes, currencyFormat));
                _ExportData = _ExportData.Append("</td></tr></table>");
                _ExportData = _ExportData.Append("<div>&nbsp;</div>");
                #endregion
            }

            _ExportData = _ExportData.Append("</body></html>");
            sw.Write(_ExportData);
            sw.Close();
        }

        private StringBuilder GetMerchantInfoHTMLTemplate(DataRow[] merchantInfo, Dictionary<string, string> resources, string currencyFormat, DataRow[] bareometerInfo = null)
        {
            StringBuilder htmlTag = new StringBuilder();
            CultureInfo ci = new CultureInfo(currencyFormat);
            string currencySymbol = ci.NumberFormat.CurrencySymbol;
            string rowBackground = "white";
            string altRowBackground = "white";
            var templatConfig = ConfigurationManager.AppSettings["ExportExcelTemplateSecurityReportMerchantInfomation"];
            templatConfig = string.IsNullOrEmpty(templatConfig) ? "~/App_Data/SecurityReportMerchantInfomationExcelTemplate.html" : templatConfig;
            string strTemplate = LoadTemplateContent(HttpContext.Current.Server.MapPath(templatConfig), currencySymbol);

            // Update and format data values
            foreach (DataRow row in merchantInfo)
            {
                string sApprovalDate = string.Empty;
                var IsRequeuedDesc = row["IsRequeued"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase) ? resources["lblYes.Text"] : resources["lblNo.Text"];
                string currentStatusDesc = bareometerInfo[0]["CurrentStatusDesc"].ToString();

                if (!string.IsNullOrEmpty(row["ApprovalDate"].ToString()))
                    sApprovalDate = ((DateTime)row["ApprovalDate"]).ToString("MM/dd/yyyy") + "&nbsp;";

                strTemplate = strTemplate.Replace("[MerchantNumber]", row["MerchantNumber"].ToString() + "&nbsp;")
                                .Replace("[CityState]", GetValueWithDefault(row, "CityState"))
                                .Replace("[SIC]", GetValueWithDefault(row, "SIC"))
                                .Replace("[BackEndProcessor]", GetValueWithDefault(row, "BackEndProcessor"))
                                .Replace("[Status]", GetValueWithDefault(row, "Status"))
                                .Replace("[ApprovalDate]", sApprovalDate)
                                .Replace("[ChainNumber]", GetValueWithDefault(row, "ChainNumber") + "&nbsp;")
                                .Replace("[HierarchyValue]", GetValueWithDefault(row, "HierarchyValue"))
                                .Replace("[RiskScore]", !string.IsNullOrEmpty(row["RiskScore"].ToString()) ? FormatData.FormatNumber(row["RiskScore"], 0) : "—")
                                .Replace("[ClassificationName]", GetValueWithDefault(row, "ClassificationName"))
                                .Replace("[ProfileDescription]", GetValueWithDefault(row, "ProfileDescription"))
                                .Replace("[Worked]", GetValueWithDefault(row, "Worked"))
                                .Replace("[ParametersWorked]", GetValueWithDefault(row, "ParametersWorked"))
                                .Replace("[MTDVolume]", GetCurrencyValueWithDefault(row, "MTDVolume", currencyFormat))
                                .Replace("[MV1]", GetCurrencyValueWithDefault(row, "MV1", currencyFormat))
                                .Replace("[MV2]", GetCurrencyValueWithDefault(row, "MV2", currencyFormat))
                                .Replace("[MV3]", GetCurrencyValueWithDefault(row, "MV3", currencyFormat))
                                .Replace("[YTDVolume]", GetCurrencyValueWithDefault(row, "YTDVolume", currencyFormat))
                                .Replace("[ExpectedPercentSWP]", GetPercentValueWithDefault(row, "ExpectedPercentSWP"))
                                .Replace("[KeyPercent]", GetPercentValueWithDefault(row, "KeyPercent"))
                                .Replace("[ExpectedAverageTicket]", GetCurrencyValueWithDefault(row, "ExpectedAverageTicket", currencyFormat))
                                .Replace("[TodayAverageTicket]", GetCurrencyValueWithDefault(row, "TodayAverageTicket", currencyFormat))
                                .Replace("[TodayHighestTransactionAmount]", GetCurrencyValueWithDefault(row, "TodayHighestTransactionAmount", currencyFormat))
                                .Replace("[ContractHighestTicket]", GetCurrencyValueWithDefault(row, "ContractHighestTicket", currencyFormat))
                                .Replace("[TodayVolume]", GetCurrencyValueWithDefault(row, "TodayVolume", currencyFormat))
                                .Replace("[ExpectedDailyVolume]", GetCurrencyValueWithDefault(row, "ExpectedDailyVolume", currencyFormat))
                                .Replace("[AltBackground]", altRowBackground).Replace("[RowBackground]", rowBackground)
                                .Replace("[IsRequeuedDesc]", IsRequeuedDesc)
                                .Replace("[lituxMerchantInfoHeaderBankAssociationAgentResource1.Text]", GetValueWithDefault(row, "HierarchyName"))
                                .Replace("[WK]", currentStatusDesc)
                                .Replace("[ExpectedMonthlyVolume]", GetCurrencyValueWithDefault(row, "ExpectedMonthlyVolume", currencyFormat));
            }

            // Update Resources Header
            strTemplate = strTemplate.Replace("[lituxMerchantInfoHeaderMerchantNumberResource1.Text]", resources["lituxMerchantInfoHeaderMerchantNumberResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderCityStateResource1.Text]", resources["lituxMerchantInfoHeaderCityStateResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderSICCodeResource1.Text]", resources["lituxMerchantInfoHeaderSICCodeResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderBEProcResource1.Text]", resources["lituxMerchantInfoHeaderBEProcResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderStatusResource1.Text]", resources["lituxMerchantInfoHeaderStatusResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderApprDateResource1.Text]", resources["lituxMerchantInfoHeaderApprDateResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderChainNOResource1.Text]", resources["lituxMerchantInfoHeaderChainNOResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderRiskResource1.Text]", resources["lituxMerchantInfoHeaderRiskResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderRiskScoreResource1.Text]", resources["lituxMerchantInfoHeaderRiskScoreResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderMerchantClassificationResource1.Text]", resources["lituxMerchantInfoHeaderMerchantClassificationResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderProfileResource1.Text]", resources["lituxMerchantInfoHeaderProfileResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderTimesWorkedResource1.Text]", resources["lituxMerchantInfoHeaderTimesWorkedResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderParametersOfWorkedResource1.Text]", resources["lituxMerchantInfoHeaderParametersOfWorkedResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderHistoricPerformanceResource1.Text]", resources["lituxMerchantInfoHeaderHistoricPerformanceResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderMTDNetAmtResource1.Text]", resources["lituxMerchantInfoHeaderMTDNetAmtResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderMv1Resource1.Text]", resources["lituxMerchantInfoHeaderMv1Resource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderMv2Resource1.Text]", resources["lituxMerchantInfoHeaderMV2Resource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderMv3Resource1.Text]", resources["lituxMerchantInfoHeaderMV3Resource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderYTDVolResource1.Text]", resources["lituxMerchantInfoHeaderYTDVolResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderMetricsResource1.Text]", resources["lituxMerchantInfoHeaderMetricsResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderContractResource1.Text]", resources["lituxMerchantInfoHeaderContractResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderTodaysResource1.Text]", resources["lituxMerchantInfoHeaderTodaysResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderKeyedTransPctResource1.Text]", resources["lituxMerchantInfoHeaderKeyedTransPctResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderAvgTransAmtResource1.Text]", resources["lituxMerchantInfoHeaderAvgTransAmtResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderLargestTransAmtResource1.Text]", resources["lituxMerchantInfoHeaderLargestTransAmtResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderTodayVolResource1.Text]", resources["lituxMerchantInfoHeaderTodayVolResource1.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderRequeuedResource1.Text]", resources["lituxMerchantInfoHeaderRequeuedResource1.Text"].Trim())
                             .Replace("[WKResource.Text]", resources["WKResource.Text"].Trim())
                             .Replace("[lituxMerchantInfoHeaderMonthlyNetAmtResource1.Text]", resources["lituxMerchantInfoHeaderMonthlyNetAmtResource1.Text"].Trim());

            htmlTag.Append(strTemplate);
            return htmlTag.Replace("$", currencySymbol);
        }        

        private StringBuilder GetBarometerHTML(DataRow[] dataSource, string columnIDList, string currencyFormat, Dictionary<string, CustomizeColumn> dicColumnCustomView)
        {
            CultureInfo ci = new CultureInfo(currencyFormat);
            string currencySymbol = ci.NumberFormat.CurrencySymbol;

            string[] columnsID = columnIDList.Split(',');

            StringBuilder htmlTag = new StringBuilder();
            htmlTag.Append(@"<table cellspacing='0' border='1' style='width: 100%;'>");

            //Column Header
            htmlTag.Append("<tr>");
            if (dicColumnCustomView != null && dicColumnCustomView.Count > 0)
            {
                foreach (var item in dicColumnCustomView)
                {
                    htmlTag.Append(string.Format(@"
                    <th style='border: solid 1px black;background-color: #ddd'>
                        {0}
                    </th>", item.Value.ReSourceKey));
                }
                columnsID = dicColumnCustomView.Select(x => x.Key).ToArray();
            }
            htmlTag.Replace("$", currencySymbol).Append("</tr>");
            htmlTag.Append("</thead>");//End Header
            //Data Item
            htmlTag.Append("<tbody>");
            foreach (DataRow row in dataSource)
            {
                string bgRow = "";
                htmlTag.Append("<tr>");
                foreach (string column in columnsID)
                {
                    CustomizeColumn riskColumn = (dicColumnCustomView == null || dicColumnCustomView.Count == 0) ? null : dicColumnCustomView[column];
                    bgRow = GetBackgroundColorOfBarometer(column, row);
                    htmlTag.Append(GetBarometerItem(column, row, riskColumn, bgRow));                    
                }
                htmlTag.Append("</tr>");

            }
            htmlTag.Append("</tbody>");
            htmlTag.Append("</table>");
            return htmlTag;
        }
        private string GetBackgroundColorOfBarometer(string columnName, DataRow row)
        {
            var configs = GetBarometerConfigColumn();
            var bgStyle = string.Empty;
            if (configs != null && configs.Configs != null && configs.Configs.Any())
            {
                var columnConfig = configs.Configs.FirstOrDefault(x => x.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
                if (columnConfig != null && !string.IsNullOrEmpty(GetValueWithDefault(row, columnName, string.Empty)))
                {
                    var bgColor = GetValueWithDefault(row, columnConfig.BgColor, string.Empty);
                    bgColor = !string.IsNullOrEmpty(bgColor) ? "background-color:" + bgColor + ";" : string.Empty;

                    var color = GetValueWithDefault(row, columnConfig.Color, string.Empty);
                    color = !string.IsNullOrEmpty(color) ? "color:" + color + ";" : string.Empty;

                    bgStyle = string.Format("{0}{1}", bgColor, color);
                }
            }

            return bgStyle;
        }
        private string GetBarometerItem(string columnName, DataRow row, CustomizeColumn riskColumn, string bgRow)
        {
            if (columnName == "BusinessAge")
            {
                return string.Format(
                    "<td style='text-align:center'>{0}</td>", row[columnName]);
            }
            else if (columnName == "TodayFirstTimeRetrievalVolume" || columnName == "TodayChargebackVolume"
                || columnName == "TodayHighestTransactionAmount" || columnName == "TodayVolume" || riskColumn != null && (riskColumn.ASFormat.ToLower() == "currency"
                || riskColumn.ASFormat.ToLower() == "currency4digits"))
            {
                return string.Format(
                    "<td style='border: solid 1px black;{0}'>{1}</td>",
                    bgRow, FormatData.FormatCurrency(row[columnName]));
            }
            else if (columnName == "VolumePercent" 
                || columnName == "ContractualVolume" 
                || columnName == "AverageTicketPercent"
                || columnName == "AuthorizationPercent" 
                || columnName == "DeclinedAuthorizationPercent"
                || columnName == "KeyPercent" 
                || columnName == "EvenDollarTransactionPercent"
                || columnName == "DuplicateDollarTransactionPercent" 
                || columnName == "ReturnPercent"
                || columnName == "TodayPrepaidCardSalesPercent" 
                || columnName == "MaxBINAuth#" 
                || columnName == "MaxBINTrans#"
                || riskColumn != null && (riskColumn.ASFormat.ToLower() == "percentage"
                || riskColumn.ASFormat.ToLower() == "percentage0digits"
                || riskColumn.ASFormat.ToLower() == "percentage4digits"))
            {
                if (columnName == "ContractualVolume" || columnName == "TodayPrepaidCardSalesPercent")
                {
                    return string.Format(
                         "<td style='border: solid 1px black;text-align:right;{0}'>{1}</td>",
                         bgRow, (row[columnName] != DBNull.Value ? row[columnName].ToString() + "%" : "—"));
                }
                else if (columnName == "MaxBINAuth#" || columnName == "MaxBINTrans#")
                {
                    return string.Format(
                       "<td style='border: solid 1px black;text-align:right;{0}'>{1}</td>",
                       bgRow, (row[columnName] != DBNull.Value ? row[columnName].ToString() : "—"));
                }
                else
                {
                    return string.Format(
                         "<td style='border: solid 1px black;{0}'>{1}</td>",
                         bgRow, row[columnName] + "%");
                }
            }
            else
            {
                return string.Format(
                   "<td style='border: solid 1px black;{0}'>{1}</td>",
                   bgRow, row[columnName]);
            }
        }
        private StringBuilder GetTransactionHTML(DataRow[] dataSource, string columnIDList, Dictionary<string, string> resources, string currencyFormat = "es-US")
        {
            List<string> _columnNameList = columnIDList.Split(',').ToList();
            StringBuilder htmlTag = GetHeaderExport(resources, 24, 36);
            
            //Data Item
            int keyCount = 0;
            decimal transAmount = 0;
            htmlTag.Append("<tbody>");

            if(dataSource != null && dataSource.Any() && dataSource.Length > 0)
            {
                List<DataColumn> tableColumns = dataSource[0].Table.Columns.Cast<DataColumn>().ToList();
                foreach (DataRow row in dataSource)
                {
                    string bgRow = "";
                    htmlTag.Append("<tr>");

                    foreach (string item in _columnNameList)
                    {
                        var column = tableColumns.FirstOrDefault(col => item.Equals(col.ColumnName, StringComparison.OrdinalIgnoreCase));
                        if (column != null)
                        {
                            bgRow = GetTransactionBackgroundColor(column.ColumnName, row);
                            htmlTag.Append(FormatText(column.ColumnName, row[column.ColumnName], bgRow, currencyFormat));
                        }                       
                    }

                    htmlTag.Append("</tr>");
                    if (row["Keyed"].ToString() == "Y")
                        keyCount++;
                    transAmount += Convert.ToDecimal(row["TransactionAmount"]);
                }

                if (dataSource.Length > 0)
                {
                    //Footer
                    htmlTag.Append(string.Format(@"<tr>
                    <td style='border: solid 1px black;background-color: #999'>Total: {0}</td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'>{1}</td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'>{2}</td></tr>", dataSource.Length, keyCount, FormatData.FormatCurrency(transAmount, currencyFormat)));
                }
            }

            htmlTag.Append("</tbody>");
            htmlTag.Append("</table>");
            return htmlTag;
        }

        private StringBuilder GetHeaderExport(Dictionary<string, string> resources, int resourceFrom, int resourceTo)
        {
            StringBuilder header = new StringBuilder();
            header.Append(@"<table cellspacing='0' border='1' style='width: 100%;'>");
            header.Append("<tr>");
            for (int i = resourceFrom; i < resourceTo; i++)
            {

                header.Append(string.Format(@"
                    <th style='border: solid 1px black;background-color: #ddd'>
                        {0}
                    </th>", resources["ASGridBoundColumnResource" + i.ToString() + ".HeaderText"]));

            }

            header.Append("</tr>");
            header.Append("</thead>");
            return header;
        }

        private string GetTransactionBackgroundColor(string columnName, DataRow row)
        {
            string bgRow = "";
            if (columnName == "MatchCode")
            {
                if (row["MatchCode"].ToString() == "P")
                    bgRow = "background-color: Blue;color:White";
                else if (row["MatchCode"].ToString() == "U")
                    bgRow = "background-color:Red;color:White";
                else if (row["MatchCode"].ToString() == "M")
                    bgRow = "background-color: #BBFF99";
                else if (row["DuplicateFlag"].ToString().Contains("True"))
                    bgRow = "background-color:#FFFF99";
                else
                    bgRow = "";
            }
            else if (columnName == "TransactionAmount")
            {
                if (row["HighestTransactionAmountFlag"].ToString().Contains("True"))
                    bgRow = "background-color: Green;color:White";
                else if (row["DuplicateFlag"].ToString().Contains("True"))
                    bgRow = "background-color:#FFFF99";
            }
            else if (row["DuplicateFlag"].ToString().Contains("True"))
                bgRow = "background-color:#FFFF99";

            return bgRow;
        }

        private StringBuilder GetChargebackHTML(DataRow[] dataSource, string columnIDList, Dictionary<string, string> resources, string currencyFormat = "es-US")
        {
            List<string> _columnNameList = columnIDList.Split(',').ToList();
            StringBuilder htmlTag = GetHeaderExport(resources, 37, 44);           

            //Data Item
            int keyCount = 0;
            decimal transAmount = 0;
            htmlTag.Append("<tbody>");
            if (dataSource != null && dataSource.Any() && dataSource.Length > 0)
            {
                List<DataColumn> tableColumns = dataSource[0].Table.Columns.Cast<DataColumn>().ToList();
                foreach (DataRow row in dataSource)
                {
                    htmlTag.Append("<tr>");
                    foreach (string item in _columnNameList)
                    {
                        var column = tableColumns.FirstOrDefault(col => item.Equals(col.ColumnName, StringComparison.OrdinalIgnoreCase));
                        if (column != null)
                        {
                            htmlTag.Append(FormatText(column.ColumnName, row[column.ColumnName], string.Empty, currencyFormat));
                        }
                    }                    

                    htmlTag.Append("</tr>");
                    if (row["Keyed"].ToString() == "Y")
                        keyCount++;
                    transAmount += Convert.ToDecimal(row["TransactionAmount"]);
                }

                if (dataSource.Length > 0)
                {
                    //Footer
                    htmlTag.Append(string.Format(@"<tr>
                    <td style='border: solid 1px black;background-color: #999'>Total: {0}</td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'></td>
                    <td style='border: solid 1px black;background-color: #999'>{1}</td>
                    <td style='border: solid 1px black;background-color: #999'>{2}</td></tr>", dataSource.Length, keyCount, FormatData.FormatCurrency(transAmount, currencyFormat)));
                }
            }

            htmlTag.Append("</tbody>");
            htmlTag.Append("</table>");
            return htmlTag;
        }

        #region Data Protection

        private string GetKeyPathForClient(int ClientID, string path)
        {
            if (string.IsNullOrEmpty(ClientID.ToString())) return "";
            XmlDocument doc = new XmlDocument
            {
                XmlResolver = null
            };
            try
            {
                doc.Load(path);
            }
            catch (Exception)
            {
                Common.Logger.LoggerManager.Error("Not found KeyConfig.xml, location: " + path);
            }
            XmlNodeList nodes = doc.SelectNodes("/Clients/client");
            if (nodes.Count == 0) return "";
            for (int i = 0; i < nodes.Count; i++)
            {
                string nodeClientID = nodes[i].Attributes["id"].Value;
                if (nodeClientID.Equals(ClientID.ToString()))
                {
                    return nodes[i].Attributes["keyPath"].Value;
                }
            }

            return "";
        }

        public string ExcryptText(string plainText, int clientID, string keyFileFolder)
        {
            string keyPath = GetKeyPathForClient(clientID, keyFileFolder + "KeyConfig.xml");
            return DataProtection.EncryptTextWithKeyFile(keyPath, plainText);
        }

        public DataTable ExcryptTexts(DataTable encryptTable, int clientID, string keyFileFolder)
        {
            string keyPath = GetKeyPathForClient(clientID, keyFileFolder + "KeyConfig.xml");
            var nCols = encryptTable.Columns.Count;

            for (int i = 0; i < nCols; i++)
            {
                DataColumn column = encryptTable.Columns[i];
                encryptTable.Columns.Add(column.ColumnName + "_Original", typeof(string));

                foreach (DataRow row in encryptTable.Rows)
                {
                    row[column.ColumnName + "_Original"] = row[column.ColumnName];
                    row[column.ColumnName] = DataProtection.EncryptTextWithKeyFile(keyPath, row[column.ColumnName]?.ToString());
                }
            }

            return encryptTable;
        }

        public string DecryptText(string cipherText, int clientID, string keyFileFolder)
        {
            string keyPath = GetKeyPathForClient(clientID, keyFileFolder + "KeyConfig.xml");
            return DataProtection.DecryptTextWithKeyFile(keyPath, cipherText);
        }

        public string EncryptTextWithMultiKey(string plainText, int clientID, string keyFileFolder)
        {
            string keyPath = GetKeyPathForClient(clientID, keyFileFolder + "KeyConfig.xml");
            string[] test = DataProtection.EncryptTextWithAllKeysWithKeyFile(keyPath, plainText);
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < test.Length; i++)
            {
                strBuilder.Append($"{test[i]},");
            }
            string Temp = strBuilder.ToString();
            if (Temp.EndsWith(",")) Temp = Temp.TrimEnd(new char[] { ',' });
            return Temp;
        }

        #endregion

        #region "Risk Management Export"

        public string ExportDataForRiskManagementReporting(ExportRiskReportRequest request, FilterParameterCollection parameters)
        {
            CultureInfo ci = new CultureInfo(request.CurrencyFormat);
            string currencySymbol = ci.NumberFormat.CurrencySymbol;
            _resources = request.Resources;
            string fileName = $"{request.FilePath}\\{DateTime.Now.Ticks}.mct";
            if (request.ExportType == "excel")
            {
                ExportToExcel(request.ReportTitle, request.ReportType, parameters, request.TemplatePath, fileName, request.IsNRTRisk, currencySymbol);
            }
            else if (request.ExportType == "csv")
            {
                ExportToCsv(request.ReportTitle, request.ReportType, parameters, fileName, request.IsNRTRisk, currencySymbol);
            }
            else
            {
                StringBuilder errString = new StringBuilder();
                errString.Append("This export type is not support.");
                WriteToFile(fileName, errString);
            }
            return fileName;
        }

        private void WriteToFile(string fileName, StringBuilder partialData)
        {
            FileStream file = null;
            try
            {
                file = new FileStream(fileName, FileMode.Append);
                using (StreamWriter writeFile = new StreamWriter(file, Encoding.Default))
                {
                    writeFile.Write(partialData.ToString());
                    writeFile.Flush();
                    writeFile.Close();
                }
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
        }

        #region Export Excel        
        private void ExportToExcel(string reportTitle, string reportType,
            FilterParameterCollection parameters, string templatePath, string tmpFileName, bool isNRTRisk, string currencySymbol = "$")
        {
            switch (reportType)
            {
                case "WorkedDetail":
                    ExportExcelWorkedDetail(reportTitle, parameters, templatePath, tmpFileName, isNRTRisk, currencySymbol);
                    break;
                case "WorkedSummary":
                    ExportExcelWorkedSummary(reportTitle, parameters, templatePath, tmpFileName, isNRTRisk, currencySymbol);
                    break;
                case "AssignmentVolumeSummary":
                    ExportExcelAssignmentVolumeSummary(reportTitle, parameters, templatePath, tmpFileName, isNRTRisk, currencySymbol);
                    break;
                case "AssignmentAlertSummary":
                    ExportExcelAssignmentAlertSummary(reportTitle, parameters, templatePath, tmpFileName, isNRTRisk, currencySymbol);
                    break;
                default:
                    StringBuilder strHtml = new StringBuilder();
                    strHtml.Append(GetReportResource(ReportResourceKeys.ReportTypeNotFound));
                    WriteToFile(tmpFileName, strHtml);
                    break;
            }
        }
        private void ExportExcelWorkedDetail(string reportTitle, FilterParameterCollection parameters, 
            string templatePath, string tmpFileName, bool isNRTRisk, string currencySymbol = "$")
        {
            StringBuilder strHtmlWorkedDetail = new StringBuilder();
            string strTemplate = LoadTemplateContent(templatePath + "WorkedDetailExcelTemplate.html", currencySymbol);
            string tmpHeader = GetTemplateElement(strTemplate, "<!--BEGIN_HEADER-->", "<!--END_HEADER-->");
            string tmpGroup = GetTemplateElement(strTemplate, "<!--BEGIN_GROUP-->", "<!--END_GROUP-->");
            string tmpAgent = GetTemplateElement(strTemplate, "<!--BEGIN_AGENT-->", "<!--END_AGENT-->");
            string tmpDetailTitle = GetTemplateElement(strTemplate, "<!--BEGIN_DETAIL_TITLE-->", "<!--END_DETAIL_TITLE-->");
            string tmpDetail = GetTemplateElement(strTemplate, "<!--BEGIN_DETAIL-->", "<!--END_DETAIL-->");
            string tmpFooter = GetTemplateElement(strTemplate, "<!--BEGIN_FOOTER-->", "<!--END_FOOTER-->");
            string strCurrentAgent = "";
            string strCurrentGroup = "";
            strHtmlWorkedDetail.Append(tmpHeader.Replace("[REPORT_TITLE]", reportTitle));//Report title
            WriteToFile(tmpFileName, strHtmlWorkedDetail);
            strHtmlWorkedDetail = new StringBuilder();
            int viewMode = int.Parse(GetParamValue(parameters, "@ViewMode"));
            string spa = isNRTRisk ? "spa_RM_MCF_Mgmt_GetWorkedDetail" : "spa_rm_Mgmt_GetWorkedDetail";
            IDataReader dataReader = GetReportsByDataReader(spa, parameters);
            while (dataReader.Read())
            {
                if (viewMode == 0 && strCurrentGroup != dataReader["GroupID"].ToString())
                {
                    string strGroup = tmpGroup;
                    strGroup = strGroup.Replace("[GROUP_NAME]", FormatString(dataReader["GroupName"]));
                    strGroup = strGroup.Replace("[TOTAL_WORKED]", FormatInteger(dataReader["GroupTotal"]));
                    strHtmlWorkedDetail.Append(strGroup);
                    WriteToFile(tmpFileName, strHtmlWorkedDetail);
                    strHtmlWorkedDetail = new StringBuilder();
                    strCurrentGroup = dataReader["GroupID"].ToString();
                }
                if (strCurrentAgent != dataReader["UserID"].ToString())
                {
                    string strAgent = tmpAgent;
                    strAgent = strAgent.Replace("[AGENT_NAME]", FormatString(dataReader["FullName"]));
                    strAgent = strAgent.Replace("[TOTAL_WORKED]", FormatInteger(dataReader["AgentTotal"]));
                    strHtmlWorkedDetail.Append(strAgent);
                    strHtmlWorkedDetail.Append(tmpDetailTitle);
                    WriteToFile(tmpFileName, strHtmlWorkedDetail);
                    strHtmlWorkedDetail = new StringBuilder();
                    strCurrentAgent = dataReader["UserID"].ToString();
                }
                string strDetail = tmpDetail;
                strDetail = strDetail.Replace("[WORKED_ON]", FormatDateTime(dataReader["WorkedOn"]));
                strDetail = strDetail.Replace("[MERCHANT_NUMBER]", dataReader["MerchantNumber"].ToString() + "&nbsp;");
                strDetail = strDetail.Replace("[MERCHANT_NAME]", FormatString(dataReader["MerchantName"]));
                strDetail = strDetail.Replace("[CASE_NUMBER]", FormatInteger(dataReader["CaseNumber"]));
                strDetail = strDetail.Replace("[INVESTIGATION_OPENED_ON]", FormatDateTime(dataReader["InvestigationOpenedOn"]));
                strDetail = strDetail.Replace("[STATUS]", FormatString(dataReader["StatusDesc"]));
                strDetail = strDetail.Replace("[INVESTIGATION_CLOSED_ON]", FormatDateTime(dataReader["InvestigationClosedOn"]));
                strDetail = strDetail.Replace("[RESOLUTION]", FormatString(dataReader["ResolutionDesc"]));
                strHtmlWorkedDetail.Append(strDetail);
                WriteToFile(tmpFileName, strHtmlWorkedDetail);
                strHtmlWorkedDetail = new StringBuilder();
            }
            strHtmlWorkedDetail.Append(tmpFooter);
            WriteToFile(tmpFileName, strHtmlWorkedDetail);
            dataReader.Close();
        }
        private void ExportExcelWorkedSummary(string reportTitle, FilterParameterCollection parameters,
            string templatePath, string tmpFileName, bool isNRTRisk, string currencySymbol = "$")
        {
            StringBuilder strHtmlWorkedSummary = new StringBuilder();
            string strTemplate = "";
            string tmpGroup = "";
            int viewMode = int.Parse(GetParamValue(parameters, "@ViewMode"));
            if (viewMode == 0)
            {
                strTemplate = LoadTemplateContent(templatePath + "WorkedSummaryGroupExcelTemplate.html", currencySymbol);
                tmpGroup = GetTemplateElement(strTemplate, "<!--BEGIN_GROUP-->", "<!--END_GROUP-->");
            }
            if (viewMode == 1)
            {
                strTemplate = LoadTemplateContent(templatePath + "WorkedSummaryAgentExcelTemplate.html", currencySymbol);
            }
            string tmpHeader = GetTemplateElement(strTemplate, "<!--BEGIN_HEADER-->", "<!--END_HEADER-->");
            string tmpFooter = GetTemplateElement(strTemplate, "<!--BEGIN_FOOTER-->", "<!--END_FOOTER-->");
            string tmpAgent = GetTemplateElement(strTemplate, "<!--BEGIN_AGENT-->", "<!--END_AGENT-->");
            string tmpAssignment = GetTemplateElement(strTemplate, "<!--BEGIN_ASSIGNMENT-->", "<!--END_ASSIGNMENT-->");
            string tmpReportHeader = GetTemplateElement(strTemplate, "<!--BEGIN_REPORT_HEADER-->", "<!--END_REPORT_HEADER-->");
            strHtmlWorkedSummary.Append(tmpHeader.Replace("[REPORT_TITLE]", reportTitle));//Report title
            strHtmlWorkedSummary.Append(tmpReportHeader);
            WriteToFile(tmpFileName, strHtmlWorkedSummary);
            strHtmlWorkedSummary = new StringBuilder();
            string spa = isNRTRisk ? "spa_RM_MCF_Mgmt_GetWorkedSummary" : "spa_rm_Mgmt_GetWorkedSummary";
            IDataReader dataReader = GetReportsByDataReader(spa, parameters);
            while (dataReader.Read())
            {
                switch (int.Parse(dataReader["RecordLevel"].ToString())) //1: Group, 2: Agent, 3: Assignment
                {
                    case 1:
                        string strGroup = tmpGroup;
                        strGroup = strGroup.Replace("[RECORD_NAME]", FormatString(dataReader["Name"]));
                        strGroup = strGroup.Replace("[TOTAL_ASSIGNED]", FormatInteger(dataReader["TotalAssigned"]));
                        strGroup = strGroup.Replace("[TOTAL_WORKED]", FormatInteger(dataReader["TotalWorked"]));
                        strGroup = strGroup.Replace("[TOTAL_WORKED_PRIOR_TO_DATE_RANGE]", FormatInteger(dataReader["TotalWorkedPrior"]));

                        strGroup = strGroup.Replace("[PER_WORKED]", FormatPercent(dataReader["PercentWorked"]));
                        strGroup = strGroup.Replace("[CASE_OPENED]", FormatInteger(dataReader["CasesOpened"]));
                        strGroup = strGroup.Replace("[CASE_CLOSED]", FormatInteger(dataReader["CasesClosed"]));
                        strGroup = strGroup.Replace("[PER_PENDING]", FormatPercent(dataReader["PercentPending"]));
                        strGroup = strGroup.Replace("[TOTAL_PENDING]", FormatInteger(dataReader["TotalPending"]));
                        strHtmlWorkedSummary.Append(strGroup);
                        WriteToFile(tmpFileName, strHtmlWorkedSummary);
                        strHtmlWorkedSummary = new StringBuilder();
                        break;
                    case 2:
                        string strAgent = tmpAgent;
                        strAgent = strAgent.Replace("[RECORD_NAME]", FormatString(dataReader["Name"]));
                        strAgent = strAgent.Replace("[TOTAL_ASSIGNED]", FormatInteger(dataReader["TotalAssigned"]));
                        strAgent = strAgent.Replace("[TOTAL_WORKED]", FormatInteger(dataReader["TotalWorked"]));

                        strAgent = strAgent.Replace("[TOTAL_WORKED_PRIOR_TO_DATE_RANGE]", FormatInteger(dataReader["TotalWorkedPrior"]));

                        strAgent = strAgent.Replace("[PER_WORKED]", FormatPercent(dataReader["PercentWorked"]));
                        strAgent = strAgent.Replace("[CASE_OPENED]", FormatInteger(dataReader["CasesOpened"]));
                        strAgent = strAgent.Replace("[CASE_CLOSED]", FormatInteger(dataReader["CasesClosed"]));
                        strAgent = strAgent.Replace("[PER_PENDING]", FormatPercent(dataReader["PercentPending"]));
                        strAgent = strAgent.Replace("[TOTAL_PENDING]", FormatInteger(dataReader["TotalPending"]));
                        strHtmlWorkedSummary.Append(strAgent);
                        WriteToFile(tmpFileName, strHtmlWorkedSummary);
                        strHtmlWorkedSummary = new StringBuilder();
                        break;
                    case 3:
                        string strAssignment = tmpAssignment;
                        strAssignment = strAssignment.Replace("[RECORD_NAME]", String.Format("{0:MM/dd/yyyy}", dataReader["ReportDate"]) + " - " + FormatString(dataReader["Name"]));
                        strAssignment = strAssignment.Replace("[TOTAL_ASSIGNED]", FormatInteger(dataReader["TotalAssigned"]));
                        strAssignment = strAssignment.Replace("[TOTAL_WORKED]", FormatInteger(dataReader["TotalWorked"]));

                        strAssignment = strAssignment.Replace("[TOTAL_WORKED_PRIOR_TO_DATE_RANGE]", FormatInteger(dataReader["TotalWorkedPrior"]));


                        strAssignment = strAssignment.Replace("[PER_WORKED]", FormatPercent(dataReader["PercentWorked"]));
                        strAssignment = strAssignment.Replace("[CASE_OPENED]", FormatInteger(dataReader["CasesOpened"]));
                        strAssignment = strAssignment.Replace("[CASE_CLOSED]", FormatInteger(dataReader["CasesClosed"]));
                        strAssignment = strAssignment.Replace("[PER_PENDING]", FormatPercent(dataReader["PercentPending"]));
                        strAssignment = strAssignment.Replace("[TOTAL_PENDING]", FormatInteger(dataReader["TotalPending"]));
                        strHtmlWorkedSummary.Append(strAssignment);
                        WriteToFile(tmpFileName, strHtmlWorkedSummary);
                        strHtmlWorkedSummary = new StringBuilder();
                        break;
                }
            }
            strHtmlWorkedSummary.Append(tmpFooter);
            WriteToFile(tmpFileName, strHtmlWorkedSummary);
            dataReader.Close();
        }
        private void ExportExcelAssignmentVolumeSummary(string reportTitle, FilterParameterCollection parameters,
            string templatePath, string tmpFileName, bool isNRTRisk, string currencySymbol = "$")
        {
            StringBuilder strHtmlAssignmentVolumeSummary = new StringBuilder();
            string strTemplate = LoadTemplateContent(templatePath + "AssignmentVolumeSummaryExcelTemplate.html", currencySymbol);
            string tmpHeader = GetTemplateElement(strTemplate, "<!--BEGIN_HEADER-->", "<!--END_HEADER-->");
            string tmpAssignmentHeader = GetTemplateElement(strTemplate, "<!--BEGIN_ASSIGNMENT_HEADER-->", "<!--END_ASSIGNMENT_HEADER-->");
            string tmpBlankRow = GetTemplateElement(strTemplate, "<!--BEGIN_BLANK_ROW-->", "<!--END_BLANK_ROW-->");
            string tmpDate = GetTemplateElement(strTemplate, "<!--BEGIN_DATE-->", "<!--END_DATE-->");
            string tmpAssignment = GetTemplateElement(strTemplate, "<!--BEGIN_ASSIGNMENT-->", "<!--END_ASSIGNMENT-->");
            string tmpFooter = GetTemplateElement(strTemplate, "<!--BEGIN_FOOTER-->", "<!--END_FOOTER-->");
            strHtmlAssignmentVolumeSummary.Append(tmpHeader.Replace("[REPORT_TITLE]", reportTitle));//Report title
            WriteToFile(tmpFileName, strHtmlAssignmentVolumeSummary);
            strHtmlAssignmentVolumeSummary = new StringBuilder();
            int intCurrentLevel = 100;
            string spa = isNRTRisk ? "spa_RM_MCF_Mgmt_GetAssignmentVolumeSummary" : "spa_rm_Mgmt_GetAssignmentVolumeSummary";
            IDataReader dataReader = GetReportsByDataReader(spa, parameters);
            while (dataReader.Read())
            {
                switch (int.Parse(dataReader["RecordLevel"].ToString())) //1: Date, 2: Assignment
                {
                    case 1:
                        if (intCurrentLevel == 2)
                        {
                            strHtmlAssignmentVolumeSummary.Append(tmpBlankRow);
                        }
                        string strDate = tmpDate;
                        strDate = strDate.Replace("[REPORT_DATE]", FormatString(dataReader["Name"]));
                        strDate = strDate.Replace("[TOTAL_ASSIGNED]", FormatInteger(dataReader["TotalAssigned"]));
                        strDate = strDate.Replace("[PER_WORKED]", FormatPercent(dataReader["PercentWorked"]));
                        strDate = strDate.Replace("[PER_PENDING]", FormatPercent(dataReader["PercentNotWorked"]));
                        strHtmlAssignmentVolumeSummary.Append(strDate);
                        WriteToFile(tmpFileName, strHtmlAssignmentVolumeSummary);
                        strHtmlAssignmentVolumeSummary = new StringBuilder();
                        break;
                    case 2:
                        if (intCurrentLevel != 2)
                        {
                            strHtmlAssignmentVolumeSummary.Append(tmpAssignmentHeader);
                        }
                        string strAssignment = tmpAssignment;
                        strAssignment = strAssignment.Replace("[RECORD_NAME]", FormatString(dataReader["Name"]));
                        strAssignment = strAssignment.Replace("[TOTAL_ASSIGNED]", FormatInteger(dataReader["TotalAssigned"]));
                        strAssignment = strAssignment.Replace("[TOTAL_WORKED]", FormatInteger(dataReader["TotalWorked"]));
                        strAssignment = strAssignment.Replace("[PER_WORKED]", FormatPercent(dataReader["PercentWorked"]));
                        strAssignment = strAssignment.Replace("[CASE_OPENED]", FormatInteger(dataReader["CasesOpened"]));
                        strAssignment = strAssignment.Replace("[PER_OPENED]", FormatPercent(dataReader["PercentCasesOpened"]));
                        strAssignment = strAssignment.Replace("[CASE_CLOSED]", FormatInteger(dataReader["CasesClosed"]));
                        strAssignment = strAssignment.Replace("[PER_CLOSED]", FormatPercent(dataReader["PercentCasesClosed"]));
                        strAssignment = strAssignment.Replace("[PER_PENDING]", FormatPercent(dataReader["PercentNotWorked"]));
                        strAssignment = strAssignment.Replace("[TOTAL_PENDING]", FormatInteger(dataReader["TotalPending"]));
                        strHtmlAssignmentVolumeSummary.Append(strAssignment);
                        WriteToFile(tmpFileName, strHtmlAssignmentVolumeSummary);
                        strHtmlAssignmentVolumeSummary = new StringBuilder();
                        break;
                }
                intCurrentLevel = int.Parse(dataReader["RecordLevel"].ToString());
            }
            strHtmlAssignmentVolumeSummary.Append(tmpFooter);
            WriteToFile(tmpFileName, strHtmlAssignmentVolumeSummary);
            dataReader.Close();
        }
        private void ExportExcelAssignmentAlertSummary(string reportTitle, FilterParameterCollection parameters,
            string templatePath, string tmpFileName, bool isNRTRisk, string currencySymbol = "$")
        {
            StringBuilder strHtmlAssignmentAlertSummary = new StringBuilder();
            string strTemplate = LoadTemplateContent(templatePath + "AssignmentAlertSummaryExcelTemplate.html", VeraCodeSolution.ValidateResponseData(currencySymbol));
            string tmpHeader = GetTemplateElement(strTemplate, "<!--BEGIN_HEADER-->", "<!--END_HEADER-->");
            string tmpReportHeader = GetTemplateElement(strTemplate, "<!--BEGIN_REPORT_HEADER-->", "<!--END_REPORT_HEADER-->");
            string tmpDate = GetTemplateElement(strTemplate, "<!--BEGIN_DATE-->", "<!--END_DATE-->");
            string tmpAssignment = GetTemplateElement(strTemplate, "<!--BEGIN_ASSIGNMENT-->", "<!--END_ASSIGNMENT-->");
            string tmpParameterHeader = GetTemplateElement(strTemplate, "<!--BEGIN_PARAMETER_HEADER-->", "<!--END_PARAMETER_HEADER-->");
            string tmpParameter = GetTemplateElement(strTemplate, "<!--BEGIN_PARAMETER-->", "<!--END_PARAMETER-->");
            string tmpFooter = GetTemplateElement(strTemplate, "<!--BEGIN_FOOTER-->", "<!--END_FOOTER-->");
            strHtmlAssignmentAlertSummary.Append(tmpHeader.Replace("[REPORT_TITLE]", reportTitle));//Report title
            WriteToFile(tmpFileName, strHtmlAssignmentAlertSummary);
            strHtmlAssignmentAlertSummary = new StringBuilder();
            int intCurrentLevel = 100;
            string spa = isNRTRisk ? "spa_RM_MCF_Mgmt_GetAssignmentAlertSummary" : "spa_rm_Mgmt_GetAssignmentAlertSummary";
            IDataReader dataReader = GetReportsByDataReader(spa, parameters);
            while (dataReader.Read())
            {
                switch (int.Parse(dataReader["RecordLevel"].ToString())) //1: Date, 2: Assignment, 3: Parameter
                {
                    case 1://Date
                        string strDate = tmpDate;
                        strDate = strDate.Replace("[REPORT_DATE]", FormatString(dataReader["Name"]));
                        strHtmlAssignmentAlertSummary.Append(strDate);
                        strHtmlAssignmentAlertSummary.Append(tmpReportHeader);
                        WriteToFile(tmpFileName, strHtmlAssignmentAlertSummary);
                        strHtmlAssignmentAlertSummary = new StringBuilder();
                        break;
                    case 2://Assignment
                        string strAssignment = tmpAssignment;
                        strAssignment = strAssignment.Replace("[ASSIGNMENT_NAME]", FormatString(dataReader["Name"]));
                        strAssignment = strAssignment.Replace("[TOTAL_VIOLATIONS]", FormatInteger(dataReader["TotalAssigned"]));
                        strAssignment = strAssignment.Replace("[DISTINCT_OF_MERCHANTS]", FormatInteger(dataReader["TotalDistinctAssigned"]));
                        strAssignment = strAssignment.Replace("[TOTAL_MERCHANTS_WORKED]", FormatInteger(dataReader["TotalWorked"]));
                        strAssignment = strAssignment.Replace("[PER_WORKED]", FormatPercent(dataReader["PercentWorked"]));
                        strAssignment = strAssignment.Replace("[PER_PENDING]", FormatPercent(dataReader["PercentNotWorked"]));
                        strHtmlAssignmentAlertSummary.Append(strAssignment);
                        WriteToFile(tmpFileName, strHtmlAssignmentAlertSummary);
                        strHtmlAssignmentAlertSummary = new StringBuilder();
                        break;
                    case 3://Parameter
                        ExportExcelAssignmentAlertSummaryForParameters(dataReader, intCurrentLevel, tmpFileName, tmpParameterHeader, tmpParameter, currencySymbol);
                        break;
                }
                intCurrentLevel = int.Parse(dataReader["RecordLevel"].ToString());
            }
            strHtmlAssignmentAlertSummary.Append(tmpFooter);
            WriteToFile(tmpFileName, strHtmlAssignmentAlertSummary);
            dataReader.Close();          
        }
        private void ExportExcelAssignmentAlertSummaryForParameters(IDataReader dataReader, int intCurrentLevel, string tmpFileName, string tmpParameterHeader, string tmpParameter, string currencySymbol = "$")
        {
            StringBuilder strHtmlAssignmentAlertSummary = new StringBuilder();
            if (intCurrentLevel != 3)
            {
                strHtmlAssignmentAlertSummary.Append(tmpParameterHeader);
            }

            int parameterPrecision = int.Parse(NvlString(dataReader["ParameterPrecision"]));
            string indicatorFormat = "#,##0" + (parameterPrecision > 0 ? ".".PadRight(parameterPrecision + 1, '0') : string.Empty);

            string parameterIndicator = NvlString(dataReader["ParameterIndicator"]);
            string dataType = NvlString(dataReader["ParameterDataType"]).ToLower();

            string parameterThreshold = NvlString(dataReader["ParameterThreshold"]);
            string thresholdType = NvlString(dataReader["ThresholdType"]).ToLower();

            string parameterThresholdHigh = NvlString(dataReader["ParameterThresholdHigh"]);
            string parameterThresholdType = NvlString(dataReader["ParameterThresholdType"]);


            if (!parameterIndicator.Equals("0") && !string.IsNullOrEmpty(parameterIndicator))
            {
                parameterIndicator = decimal.Parse(parameterIndicator).ToString(indicatorFormat);
            }


            if (!parameterThreshold.Equals("0") && !string.IsNullOrEmpty(parameterThreshold))
            {
                parameterThreshold = decimal.Parse(parameterThreshold).ToString(indicatorFormat);
            }

            if (!parameterThresholdHigh.Equals("0") && !string.IsNullOrEmpty(parameterThresholdHigh))
            {
                parameterThresholdHigh = decimal.Parse(parameterThresholdHigh).ToString(indicatorFormat);
            }

            //process indicator
            string strTempIndicator = VeraCodeSolution.ValidateResponseData(
                FormatParameterDataType(dataType, parameterIndicator, parameterPrecision, currencySymbol));


            if (!string.IsNullOrEmpty(parameterIndicator) && decimal.Parse(parameterIndicator) < 0)
            {
                strTempIndicator = "<font color='red'>" + strTempIndicator + "</font>";
            }

            string strTempThreshold;
            switch (parameterThresholdType)
            {
                case "LowHigh":
                    strTempThreshold = VeraCodeSolution.ValidateResponseData(
                        FormatLowHighThreshold(parameterThreshold, parameterThresholdHigh, thresholdType, currencySymbol));
                    break;
                default:
                    strTempThreshold = VeraCodeSolution.ValidateResponseData(
                        FormatParameterDataType(thresholdType, parameterThreshold, currencySymbol));
                    break;
            }

            if (!string.IsNullOrEmpty(parameterThreshold) && decimal.Parse(parameterThreshold) < 0)
            {
                strTempThreshold = "<font color='red'>" + strTempThreshold + "</font>";
            }

            string strParameter = tmpParameter;
            strParameter = strParameter.Replace("[PARAMETER_ID]", FormatString(dataReader["Name"]));
            strParameter = strParameter.Replace("[PARAMETER_NAME]", FormatString(dataReader["SubName"]));
            strParameter = strParameter.Replace("[PARAMETER_INDICATOR]", "&nbsp;" + strTempIndicator);
            strParameter = strParameter.Replace("[PARAMETER_THRESHOLD]", strTempThreshold);
            strParameter = strParameter.Replace("[TOTAL_VIOLATIONS]", FormatInteger(dataReader["TotalAssigned"]));
            strHtmlAssignmentAlertSummary.Append(strParameter);
            WriteToFile(tmpFileName, strHtmlAssignmentAlertSummary);
        }
        #endregion

        #region Export CSV        
        private void ExportToCsv(string reportTitle, string reportType, FilterParameterCollection parameters, 
            string tmpFileName, bool isNRTRisk, string currencySymbol = "$")
        {
            switch (reportType)
            {
                case "WorkedDetail":
                    ExportCSVWorkedDetail(reportTitle, parameters, tmpFileName, isNRTRisk);
                    break;
                case "WorkedSummary":
                    ExportCSVWorkedSummary(reportTitle, parameters, tmpFileName, isNRTRisk);
                    break;
                case "AssignmentVolumeSummary":
                    ExportCSVAssignmentVolumeSummary(reportTitle, parameters, tmpFileName, isNRTRisk);
                    break;
                case "AssignmentAlertSummary":
                    ExportCSVAssignmentAlertSummary(reportTitle, parameters, tmpFileName, isNRTRisk, currencySymbol);
                    break;
                default:
                    StringBuilder strContent = new StringBuilder();
                    strContent.Append(GetReportResource(ReportResourceKeys.ReportTypeNotFound));
                    WriteToFile(tmpFileName, strContent);
                    break;
            }
        }
        private void ExportCSVWorkedDetail(string reportTitle, FilterParameterCollection parameters, string tmpFileName, bool isNRTRisk)
        {
            StringBuilder strContentWorkedDetail = new StringBuilder();
            string strCurrentAgent = "";
            string strCurrentGroup = "";

            strContentWorkedDetail.Append(reportTitle + Environment.NewLine); //Report title
            WriteToFile(tmpFileName, strContentWorkedDetail);
            strContentWorkedDetail = new StringBuilder();
            int viewMode = int.Parse(GetParamValue(parameters, "@ViewMode"));
            string spa = isNRTRisk ? "spa_RM_MCF_Mgmt_GetWorkedDetail" : "spa_rm_Mgmt_GetWorkedDetail";
            IDataReader dataReader = GetReportsByDataReader(spa, parameters);
            while (dataReader.Read())
            {
                if (viewMode == 0 && strCurrentGroup != dataReader["GroupID"].ToString())
                {
                    strContentWorkedDetail.AppendFormat("{0}, {1}{2}",
                        GetReportResource(ReportResourceKeys.Group),
                        GetReportResource(ReportResourceKeys.TotalWorked),
                        Environment.NewLine);
                    strContentWorkedDetail.Append("\"" + dataReader["GroupName"].ToString() + "\",\"" + (dataReader["GroupTotal"] == DBNull.Value ? "" : dataReader["GroupTotal"].ToString()) + "\"" + Environment.NewLine);
                    WriteToFile(tmpFileName, strContentWorkedDetail);
                    strContentWorkedDetail = new StringBuilder();
                    strCurrentGroup = dataReader["GroupID"].ToString();
                }

                if (strCurrentAgent != dataReader["UserID"].ToString())
                {
                    strContentWorkedDetail.AppendFormat("{0}, {1}{2}",
                            GetReportResource(ReportResourceKeys.Agent),
                            GetReportResource(ReportResourceKeys.TotalWorked),
                            Environment.NewLine);
                    strContentWorkedDetail.Append("\"" + dataReader["FullName"].ToString() + "\", \"" + dataReader["AgentTotal"].ToString() + "\"" + Environment.NewLine);
                    strContentWorkedDetail.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}{8}",
                        GetReportResource(ReportResourceKeys.WorkedOn),
                        GetReportResource(ReportResourceKeys.MerchantNumber),
                        GetReportResource(ReportResourceKeys.MerchantName),
                        GetReportResource(ReportResourceKeys.CaseNumber),
                        GetReportResource(ReportResourceKeys.InvestigationOpenedOn),
                        GetReportResource(ReportResourceKeys.Status),
                        GetReportResource(ReportResourceKeys.InvestigationClosedOn),
                        GetReportResource(ReportResourceKeys.Resolution),
                        Environment.NewLine);
                    WriteToFile(tmpFileName, strContentWorkedDetail);
                    strContentWorkedDetail = new StringBuilder();
                    strCurrentAgent = dataReader["UserID"].ToString();
                }
                strContentWorkedDetail.Append("\"" + FormatDateTime(dataReader["WorkedOn"]) + "\",");
                strContentWorkedDetail.Append("\"" + FormatString(dataReader["MerchantNumber"]) + "\",");
                strContentWorkedDetail.Append("\"" + FormatString(dataReader["MerchantName"]) + "\",");
                strContentWorkedDetail.Append("\"" + FormatInteger(dataReader["CaseNumber"]) + "\",");
                strContentWorkedDetail.Append("\"" + FormatDateTime(dataReader["InvestigationOpenedOn"]) + "\",");
                strContentWorkedDetail.Append("\"" + FormatString(dataReader["StatusDesc"]) + "\",");
                strContentWorkedDetail.Append("\"" + FormatDateTime(dataReader["InvestigationClosedOn"]) + "\",");
                strContentWorkedDetail.Append("\"" + FormatString(dataReader["ResolutionDesc"]) + "\"");
                strContentWorkedDetail.Append(Environment.NewLine);
                WriteToFile(tmpFileName, strContentWorkedDetail);
                strContentWorkedDetail = new StringBuilder();
            }
            dataReader.Close();
        }
        private void ExportCSVWorkedSummary(string reportTitle, FilterParameterCollection parameters, string tmpFileName, bool isNRTRisk)
        {
            StringBuilder strContentWorkedSummary = new StringBuilder();
            strContentWorkedSummary.Append(reportTitle + Environment.NewLine); //Report title
            WriteToFile(tmpFileName, strContentWorkedSummary);
            strContentWorkedSummary = new StringBuilder();
            int currentLevel = 100; //Check for assigments chain
            string spa = isNRTRisk ? "spa_RM_MCF_Mgmt_GetWorkedSummary" : "spa_rm_Mgmt_GetWorkedSummary";
            IDataReader dataReader = GetReportsByDataReader(spa, parameters);
            while (dataReader.Read())
            {
                switch (int.Parse(dataReader["RecordLevel"].ToString())) //1: Group, 2: Agent, 3: Assignment
                {
                    case 1:
                        strContentWorkedSummary.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}{9}",
                        GetReportResource(ReportResourceKeys.GroupName),
                        GetReportResource(ReportResourceKeys.TotalAssigned),
                        GetReportResource(ReportResourceKeys.TotalWorked),
                        GetReportResource(ReportResourceKeys.TotalWorkedPriorToDateRange),
                        GetReportResource(ReportResourceKeys.WorkedPercentage),
                        GetReportResource(ReportResourceKeys.CasesClosed),
                        GetReportResource(ReportResourceKeys.CasesOpened),
                        GetReportResource(ReportResourceKeys.PendingPercent),
                        GetReportResource(ReportResourceKeys.TotalPending),
                        Environment.NewLine);

                        strContentWorkedSummary.Append("\"" + FormatString(dataReader["Name"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalAssigned"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalWorked"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalWorkedPrior"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatPercent(dataReader["PercentWorked"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["CasesClosed"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["CasesOpened"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatPercent(dataReader["PercentPending"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalPending"]) + "\"" + Environment.NewLine);
                        WriteToFile(tmpFileName, strContentWorkedSummary);
                        strContentWorkedSummary = new StringBuilder();
                        break;
                    case 2:
                        strContentWorkedSummary.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}{9}",
                        GetReportResource(ReportResourceKeys.AgentName),
                        GetReportResource(ReportResourceKeys.TotalAssigned),
                        GetReportResource(ReportResourceKeys.TotalWorked),
                        GetReportResource(ReportResourceKeys.TotalWorkedPriorToDateRange),
                        GetReportResource(ReportResourceKeys.WorkedPercentage),
                        GetReportResource(ReportResourceKeys.CasesClosed),
                        GetReportResource(ReportResourceKeys.CasesOpened),
                        GetReportResource(ReportResourceKeys.PendingPercent),
                        GetReportResource(ReportResourceKeys.TotalPending),
                        Environment.NewLine);

                        strContentWorkedSummary.Append("\"" + FormatString(dataReader["Name"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalAssigned"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalWorked"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalWorkedPrior"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatPercent(dataReader["PercentWorked"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["CasesClosed"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["CasesOpened"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatPercent(dataReader["PercentPending"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalPending"]) + "\"" + Environment.NewLine);
                        WriteToFile(tmpFileName, strContentWorkedSummary);
                        strContentWorkedSummary = new StringBuilder();
                        break;
                    case 3:
                        if (currentLevel == 2)
                        {
                            strContentWorkedSummary.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}{9}",
                                GetReportResource(ReportResourceKeys.Assignment),
                                GetReportResource(ReportResourceKeys.TotalAssigned),
                                GetReportResource(ReportResourceKeys.TotalWorked),
                                GetReportResource(ReportResourceKeys.TotalWorkedPriorToDateRange),
                                GetReportResource(ReportResourceKeys.WorkedPercentage),
                                GetReportResource(ReportResourceKeys.CasesClosed),
                                GetReportResource(ReportResourceKeys.CasesOpened),
                                GetReportResource(ReportResourceKeys.PendingPercent),
                                GetReportResource(ReportResourceKeys.TotalPending),
                                Environment.NewLine);
                        }
                        strContentWorkedSummary.Append("\"" + String.Format("{0:MM/dd/yyyy}", dataReader["ReportDate"]) + " - " + FormatString(dataReader["Name"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalAssigned"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalWorked"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalWorkedPrior"]) + "\",");

                        strContentWorkedSummary.Append("\"" + FormatPercent(dataReader["PercentWorked"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["CasesClosed"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["CasesOpened"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatPercent(dataReader["PercentPending"]) + "\",");
                        strContentWorkedSummary.Append("\"" + FormatInteger(dataReader["TotalPending"]) + "\"" + Environment.NewLine);
                        WriteToFile(tmpFileName, strContentWorkedSummary);
                        strContentWorkedSummary = new StringBuilder();
                        break;
                }
                currentLevel = int.Parse(dataReader["RecordLevel"].ToString());
            }
            dataReader.Close();
        }
        private void ExportCSVAssignmentVolumeSummary(string reportTitle, FilterParameterCollection parameters, string tmpFileName, bool isNRTRisk)
        {
            StringBuilder strContentAssignmentVolumeSummary = new StringBuilder();
            strContentAssignmentVolumeSummary.Append(reportTitle + Environment.NewLine); //Report title
            WriteToFile(tmpFileName, strContentAssignmentVolumeSummary);
            strContentAssignmentVolumeSummary = new StringBuilder();
            int currentLevel = 100; //Check for assigments chain
            string spa = isNRTRisk ? "spa_RM_MCF_Mgmt_GetAssignmentVolumeSummary" : "spa_rm_Mgmt_GetAssignmentVolumeSummary";
            IDataReader dataReader = GetReportsByDataReader(spa, parameters);
            while (dataReader.Read())
            {
                switch (int.Parse(dataReader["RecordLevel"].ToString())) //1: Date, 2: Assignment
                {
                    case 1: //Date
                        strContentAssignmentVolumeSummary.AppendFormat("{0}, {1}, {2}, {3}{4}",
                                GetReportResource(ReportResourceKeys.Date),
                                GetReportResource(ReportResourceKeys.AssignmentVolume),
                                GetReportResource(ReportResourceKeys.Worked),
                                GetReportResource(ReportResourceKeys.NotWorked),
                                Environment.NewLine);

                        strContentAssignmentVolumeSummary.Append("\"" + FormatString(dataReader["Name"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatInteger(dataReader["TotalAssigned"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatPercent(dataReader["PercentWorked"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatPercent(dataReader["PercentNotWorked"]) + "\"" + Environment.NewLine);
                        WriteToFile(tmpFileName, strContentAssignmentVolumeSummary);
                        strContentAssignmentVolumeSummary = new StringBuilder();
                        break;
                    case 2: //Assignment
                        if (currentLevel != 2)
                        {
                            strContentAssignmentVolumeSummary.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}{10}",
                                GetReportResource(ReportResourceKeys.Assignment),
                                GetReportResource(ReportResourceKeys.TotalAssigned),
                                GetReportResource(ReportResourceKeys.TotalWorked),
                                GetReportResource(ReportResourceKeys.WorkedPercentage),
                                GetReportResource(ReportResourceKeys.CasesOpened),
                                GetReportResource(ReportResourceKeys.OpenedPercentage),
                                GetReportResource(ReportResourceKeys.CasesClosed),
                                GetReportResource(ReportResourceKeys.ClosedPercentage),
                                GetReportResource(ReportResourceKeys.PendingPercent),
                                GetReportResource(ReportResourceKeys.TotalPending),
                                Environment.NewLine);
                        }
                        strContentAssignmentVolumeSummary.Append("\"" + FormatString(dataReader["Name"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatInteger(dataReader["TotalAssigned"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatInteger(dataReader["TotalWorked"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatPercent(dataReader["PercentWorked"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatInteger(dataReader["CasesOpened"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatPercent(dataReader["PercentCasesOpened"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatInteger(dataReader["CasesClosed"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatPercent(dataReader["PercentCasesClosed"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatPercent(dataReader["PercentNotWorked"]) + "\",");
                        strContentAssignmentVolumeSummary.Append("\"" + FormatInteger(dataReader["TotalPending"]) + "\"" + Environment.NewLine);
                        WriteToFile(tmpFileName, strContentAssignmentVolumeSummary);
                        strContentAssignmentVolumeSummary = new StringBuilder();
                        break;
                }
                currentLevel = int.Parse(dataReader["RecordLevel"].ToString());
            }
            dataReader.Close();
        }
        private void ExportCSVAssignmentAlertSummary(string reportTitle, FilterParameterCollection parameters, string tmpFileName, bool isNRTRisk, string currencySymbol = "$")
        {
            StringBuilder strContentAssignmentAlertSummary = new StringBuilder();
            strContentAssignmentAlertSummary.Append(reportTitle + Environment.NewLine); //Report title
            WriteToFile(tmpFileName, strContentAssignmentAlertSummary);
            strContentAssignmentAlertSummary = new StringBuilder();
            int currentLevel = 100; //Check for assigments chain
            string spa = isNRTRisk ? "spa_RM_MCF_Mgmt_GetAssignmentAlertSummary" : "spa_rm_Mgmt_GetAssignmentAlertSummary";
            IDataReader dataReader = GetReportsByDataReader(spa, parameters);
            while (dataReader.Read())
            {
                switch (int.Parse(dataReader["RecordLevel"].ToString())) //1: Date, 2: Assignment, 3: Parameter
                {
                    case 1: //Date
                        if (currentLevel != 1)
                        {
                            strContentAssignmentAlertSummary.AppendFormat("{0}",
                                GetReportResource(ReportResourceKeys.Date));
                            strContentAssignmentAlertSummary.Append(Environment.NewLine);
                        }
                        strContentAssignmentAlertSummary.Append("\"" + FormatString(dataReader["Name"]) + "\"");
                        strContentAssignmentAlertSummary.Append(Environment.NewLine);
                        WriteToFile(tmpFileName, strContentAssignmentAlertSummary);
                        strContentAssignmentAlertSummary = new StringBuilder();
                        break;
                    case 2: //Assignment
                        if (currentLevel != 2)
                        {
                            strContentAssignmentAlertSummary.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}{6}",
                                GetReportResource(ReportResourceKeys.Assignment),
                                GetReportResource(ReportResourceKeys.TotalViolations),
                                GetReportResource(ReportResourceKeys.DistinctNumberOfMerchants),
                                GetReportResource(ReportResourceKeys.TotalMerchantsWorked),
                                GetReportResource(ReportResourceKeys.WorkedPercentage),
                                GetReportResource(ReportResourceKeys.PendingPercent),
                                Environment.NewLine);
                        }
                        strContentAssignmentAlertSummary.Append("\"" + FormatString(dataReader["Name"]) + "\",");
                        strContentAssignmentAlertSummary.Append("\"" + FormatInteger(dataReader["TotalAssigned"]) + "\",");
                        strContentAssignmentAlertSummary.Append("\"" + FormatInteger(dataReader["TotalDistinctAssigned"]) + "\",");
                        strContentAssignmentAlertSummary.Append("\"" + FormatInteger(dataReader["TotalWorked"]) + "\",");
                        strContentAssignmentAlertSummary.Append("\"" + FormatPercent(dataReader["PercentWorked"]) + "\",");
                        strContentAssignmentAlertSummary.Append("\"" + FormatPercent(dataReader["PercentNotWorked"]) + "\"");
                        strContentAssignmentAlertSummary.Append(Environment.NewLine);
                        WriteToFile(tmpFileName, strContentAssignmentAlertSummary);
                        strContentAssignmentAlertSummary = new StringBuilder();
                        break;
                    case 3:
                        ExportCSVAssignmentAlertSummaryForParameters(dataReader, currentLevel, tmpFileName, currencySymbol);
                        break;
                }
                currentLevel = int.Parse(dataReader["RecordLevel"].ToString());
            }
            dataReader.Close();
        }
        private void ExportCSVAssignmentAlertSummaryForParameters(IDataReader dataReader, int currentLevel, string tmpFileName, string currencySymbol = "$")
        {
            var strContentAssignmentAlertSummary = new StringBuilder();
            if (currentLevel != 3)
            {
                strContentAssignmentAlertSummary.AppendFormat("{0}, {1}, {2}, {3}, {4}{5}",
                    GetReportResource(ReportResourceKeys.ParamNumber),
                    GetReportResource(ReportResourceKeys.ParamName),
                    GetReportResource(ReportResourceKeys.ParamPercentageNumberMoney).Replace("$", currencySymbol),
                    GetReportResource(ReportResourceKeys.ParamTH),
                    GetReportResource(ReportResourceKeys.TotalViolations),
                    Environment.NewLine);
            }

            int parameterPrecision = int.Parse(NvlString(dataReader["ParameterPrecision"]));
            string indicatorFormat = "#,##0" + (parameterPrecision > 0 ? ".".PadRight(parameterPrecision + 1, '0') : string.Empty);

            string parameterIndicator = NvlString(dataReader["ParameterIndicator"]);
            string dataType = NvlString(dataReader["ParameterDataType"]).ToLower();

            string parameterThreshold = NvlString(dataReader["ParameterThreshold"]);
            string thresholdType = NvlString(dataReader["ThresholdType"]).ToLower();

            string parameterThresholdHigh = NvlString(dataReader["ParameterThresholdHigh"]);
            string parameterThresholdType = NvlString(dataReader["ParameterThresholdType"]);


            if (!parameterIndicator.Equals("0") && !string.IsNullOrEmpty(parameterIndicator))
            {
                parameterIndicator = decimal.Parse(parameterIndicator).ToString(indicatorFormat);
            }


            if (!parameterThreshold.Equals("0") && !string.IsNullOrEmpty(parameterThreshold))
            {
                parameterThreshold = decimal.Parse(parameterThreshold).ToString(indicatorFormat);
            }

            if (!parameterThresholdHigh.Equals("0") && !string.IsNullOrEmpty(parameterThresholdHigh))
            {
                parameterThresholdHigh = decimal.Parse(parameterThresholdHigh).ToString(indicatorFormat);
            }

            //process indicator
            string strTempIndicator = FormatParameterDataType(dataType, parameterIndicator, currencySymbol);
            string strTempThreshold;
            switch (parameterThresholdType)
            {
                case "LowHigh":
                    strTempThreshold = FormatLowHighThreshold(parameterThreshold, parameterThresholdHigh, thresholdType, currencySymbol);
                    break;
                default:
                    strTempThreshold =
                        FormatParameterDataType(thresholdType, parameterThreshold, currencySymbol);
                    break;
            }

            strContentAssignmentAlertSummary.Append("\"" + FormatString(dataReader["Name"]) + "\",");
            strContentAssignmentAlertSummary.Append("\"" + FormatString(dataReader["SubName"]) + "\",");
            strContentAssignmentAlertSummary.Append("\"" + strTempIndicator + "\",");
            strContentAssignmentAlertSummary.Append("\"" + strTempThreshold + "\",");
            strContentAssignmentAlertSummary.Append("\"" + FormatInteger(dataReader["TotalAssigned"]) + "\"");
            strContentAssignmentAlertSummary.Append(Environment.NewLine);
            WriteToFile(tmpFileName, strContentAssignmentAlertSummary);
        }
        #endregion
        private string LoadTemplateContent(string templatePath, string currencySymbol = "$")
        {
            if (!File.Exists(templatePath))
            {
                Exception exception = new Exception("Template file does not exist. Path: " + templatePath);
                throw exception;
            }
            string strTemplate = File.ReadAllText(templatePath);
            if (string.IsNullOrEmpty(strTemplate))
            {
                Exception exception = new Exception("Template content is empty. Path: " + templatePath);
                throw exception;
            }

            if (_resources == null)
                _resources = new Dictionary<string, string>();

            foreach (var key in _resources.Keys)
            {
                var templateKey = string.Format("[{0}]", key);
                if (strTemplate.Contains(templateKey))
                {
                    strTemplate = strTemplate.Replace(templateKey, _resources[key]);
                }
            }
            return strTemplate.Replace("$", currencySymbol);
        }
        private string GetReportResource(string key)
        {
            if (_resources == null)
                return string.Empty;
            return _resources.ContainsKey(key) ? _resources[key] : key;
        }

        private string GetParamValue(FilterParameterCollection parameters, string keyName)
        {
            foreach (FilterParameter p in parameters)
            {
                string currentKey = p.ParameterName.ToString();
                if (currentKey == keyName && ((DbType)p.ParameterType == DbType.DateTime))
                {
                    return Convert.ToDateTime(p.ParameterValue).ToString("MM/dd/yyyy");
                }
                else if (currentKey == keyName)
                {
                    return p.ParameterValue.ToString();
                }
            }
            return string.Empty;
        }

        private string GetTemplateElement(string strTemplate, string strBegin, string strEnd)
        {
            int indexOfBegin = strTemplate.IndexOf(strBegin) + strBegin.Length + 2;
            int indexOfEnd = strTemplate.IndexOf(strEnd);
            string strElement = strTemplate.Substring(indexOfBegin, indexOfEnd - indexOfBegin);
            return strElement;
        }

        private string FormatDateTime(object val)
        {
            return (val == null || val == DBNull.Value || string.IsNullOrEmpty(val.ToString())) ? string.Empty : Convert.ToDateTime(val).ToString("MM/dd/yyyy hh:mm:ss tt");
        }
        private string FormatDate(object val)
        {
            return (val == null || val == DBNull.Value || string.IsNullOrEmpty(val.ToString())) ? string.Empty : Convert.ToDateTime(val).ToShortDateString();
        }
        private string FormatCurrency(object val, string currencyFormat)
        {
            return (val == null || val == DBNull.Value || string.IsNullOrEmpty(val.ToString())) ? string.Empty : FormatData.FormatCurrency(val, currencyFormat);
        }
        private static string FormatPercent(object val)
        {
            return (val == null || val == DBNull.Value || string.IsNullOrEmpty(val.ToString())) ? string.Empty : decimal.Parse(val.ToString()).ToString("0.00") + "%";
        }
        private static string FormatInteger(object val)
        {
            return (val == null || val == DBNull.Value || string.IsNullOrEmpty(val.ToString())) ? string.Empty : decimal.Parse(val.ToString()).ToString("#,#0");
        }
        private static string FormatString(object val)
        {
            return (val == null || val == DBNull.Value) ? string.Empty : val.ToString();
        }
        private static string NvlString(object val)
        {
            return (val == null ? string.Empty : val.ToString());
        }

        private string FormatParameterDataType(string dataType, string dataValue, string currencySymbol = "$")
        {
            return FormatParameterDataType(dataType, dataValue, 0, currencySymbol);
        }

        private string FormatParameterDataType(string dataType, string dataValue, int precision, string currencySymbol = "$")
        {
            string displayedValue;
            string value;

            if (dataValue.IsNullOrEmpty())
                displayedValue = "N/A";
            else
            {
                value = precision > 0 ? FormatPrecision(dataValue, precision) : GetDouble4Precision(dataValue);
                if (dataType == currencySymbol)
                {
                    if (decimal.Parse(dataValue) >= 0)
                    {
                        displayedValue = string.Format("{1}{0}", value, dataType);
                    }
                    else
                    {
                        displayedValue = string.Format("({1}{0})", value.Replace("-", string.Empty), dataType);
                    }
                }
                else if (dataType == "days")
                    displayedValue = string.Format("{0} {1}", value, "day(s)");
                else if (dataType == "%")
                    displayedValue = string.Format("{0}{1}", value, dataType);
                else
                    displayedValue = string.Format("{0}", value);
            }

            return displayedValue;
        }

        private string FormatLowHighThreshold(string ThresholdLow, string ThresholdHigh, string ThresholdType, string currencySymbol = "$")
        {
            string displayedValue;

            var low = FormatParameterDataType(ThresholdType, ThresholdLow, currencySymbol);
            var high = FormatParameterDataType(ThresholdType, ThresholdHigh, currencySymbol);

            if (low == "N/A" || high == "N/A")
            {
                displayedValue = "N/A";
            }
            else
            {
                displayedValue = FormatParameterDataType(ThresholdType, ThresholdLow, currencySymbol) + " - " + FormatParameterDataType(ThresholdType, ThresholdHigh, currencySymbol);
            }
            return displayedValue;
        }

        private string GetDouble4Precision(string doubleStr)
        {
            if (string.IsNullOrEmpty(doubleStr))
                return doubleStr;
            if (doubleStr.Contains(".") && !doubleStr.EndsWith("0000"))
                return double.Parse(doubleStr).ToString("#,#0.0000");

            return double.Parse(doubleStr).ToString("#,#0");
        }

        public string FormatPrecision(string doubleStr, int precision)
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
        private string GetValueWithDefault(DataRow row, string columnName, string defaultValue = "—")
        {
            if (string.IsNullOrEmpty(columnName) || !row.Table.Columns.Contains(columnName))
                return defaultValue;
            return !string.IsNullOrEmpty(row[columnName].ToString()) ? row[columnName].ToString() : defaultValue;
        }

        private string GetCurrencyValueWithDefault(DataRow row, string columnName, string currencyFormat)
        {
            var value = GetValueWithDefault(row, columnName);
            
            if (!value.Equals("—"))
            {
                return FormatData.FormatCurrency(value, currencyFormat);
            }

            return value;
        }
        private string GetPercentValueWithDefault(DataRow row, string columnName, int precise = 2)
        {
            var value = GetValueWithDefault(row, columnName);

            if (!value.Equals("—"))
            {
                return FormatData.FormatPercent(value, precise);
            }

            return value;
        }
        private string FormatText(string columnName, object value, string backgroundColor, string currencyFormat = "en-US")
        {
            var dateColumns = new List<string>() { "TransactionDate" };            
            var currencyColumns = new List<string>() { "TransactionAmount" };
            var numberColumns = new List<string>() { "ReasonCode", "AuthorizationNumber" };
            var alignCenter = "align='center'";
            var htmlTemplate = "<td style='border: solid 1px black;{0}' {1}>{2}</td>";

            if (dateColumns.Any(x => x.Equals(columnName, StringComparison.OrdinalIgnoreCase)))
                return string.Format(htmlTemplate, backgroundColor, alignCenter, FormatDate(value));           
            else if (currencyColumns.Any(x => x.Equals(columnName, StringComparison.OrdinalIgnoreCase)))
                return string.Format(htmlTemplate, backgroundColor, string.Empty, FormatCurrency(value, currencyFormat));
            else if (numberColumns.Any(x => x.Equals(columnName, StringComparison.OrdinalIgnoreCase)))
            {
                var text = value != null && !string.IsNullOrEmpty(value.ToString()) ? value.ToString() + "&nbsp;" : string.Empty;
                return string.Format(htmlTemplate, backgroundColor, alignCenter, text);
            }
            else
            {
                var text = value != null && !string.IsNullOrEmpty(value.ToString()) ? value.ToString() : string.Empty;
                return string.Format(htmlTemplate, backgroundColor, alignCenter, text);
            }
        }
        private BarometerConfig GetBarometerConfigColumn()
        {
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/Risk/BaroReportConfig.xml");
            var fileContent = LoadFile(filePath);
            if (string.IsNullOrEmpty(fileContent))
                return null;

            return (BarometerConfig)ConvertXMLToObject(fileContent, typeof(BarometerConfig));
        }
        public object ConvertXMLToObject(string xml, Type type)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return null;
            }

            XmlSerializer ser = new XmlSerializer(type);
            using (MemoryStream memStream = new MemoryStream(Encoding.ASCII.GetBytes(xml)))
            {
                object obj = ser.Deserialize(memStream);
                return obj;
            }
        }

        private string LoadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;

            if(!File.Exists(filePath))
                return string.Empty;

            var key = filePath.Replace(@"\", "_").Replace(" ", "_");

            if (HttpRuntime.Cache[key] == null)
            {
                var fileContent = File.ReadAllText(filePath);
                HttpRuntime.Cache.Insert(key, fileContent, new System.Web.Caching.CacheDependency(filePath));
            }

            return HttpRuntime.Cache[key].ToString();
        }

        #endregion

        public DataTable HashDataTable(DataTable data)
        {
            Security.Web.StatServices.StatService stat = new AS.Security.Web.StatServices.StatService();

            var nCols = data.Columns.Count;

            for (int i = 0; i < nCols; i++)
            {
                DataColumn column = data.Columns[i];
                data.Columns.Add(column.ColumnName + "_Original", typeof(string));

                foreach (DataRow row in data.Rows)
                {
                    row[column.ColumnName + "_Original"] = row[column.ColumnName];
                    if (!string.IsNullOrEmpty(row[column.ColumnName]?.ToString()))
                    {
                        row[column.ColumnName] = stat.GetHashData(row[column.ColumnName]?.ToString());
                    }
                }
            }

            return data;
        }

        public string GetHashData(string plainText)
        {
            try
            {
                Security.Web.StatServices.StatService stat = new AS.Security.Web.StatServices.StatService();
                return stat.GetHashData(plainText);
            }
            catch (Exception ex)
            {
                Common.Logger.LoggerManager.Error("Can not connect to stat service: " + ex.Message);
                return string.Empty;
            }
        }

        public DataTable GetHashDataTable(string hashValues)
        {
            try
            {
                Security.Web.StatServices.StatService stat = new AS.Security.Web.StatServices.StatService();
                return stat.GetData(hashValues);
            }
            catch (Exception ex)
            {
                AS.Common.Logger.LoggerManager.Error("Can not connect to stat service: " + ex.Message);
                return new DataTable();
            }
        }
        private void GetHashValue(DataRow row, List<HashColEntity> columns)
        {
            foreach (var column in columns)
            {
                var cellValue = row[column.ColumnName].ToString();
                if (!string.IsNullOrEmpty(cellValue))
                {
                    row[column.ColumnName + "_Original"] = cellValue;
                    column.AddHashValue(cellValue);
                }                
            }
        }
        private void GetDecryptedValue(int asClientId, DataRow row, List<string> columns, string path)
        {
            foreach (var column in columns)
            {
                var cellValue = row[column].ToString();
                if (!string.IsNullOrEmpty(cellValue))
                {
                    row[column + "_Original"] = cellValue;
                    row[column] = DecryptText(cellValue, asClientId, path);
                }                
            }
        }
        private void AddOriginalColumn(DataTable data, List<string> columns)
        {
            if (columns != null && columns.Any())
            {
                foreach (var column in columns)
                {
                    data.Columns.Add(column + "_Original", typeof(string));
                }
            }            
        }
        public void HashData(int ASClientID, DataTable data, string[] encryptedCols, string hashColumns, string path)
        {
            if (string.IsNullOrEmpty(hashColumns) || data == null || data.Rows.Count == 0)
                return;
            hashColumns = string.Format(",{0},", hashColumns);

            //Collect the hash columns need to decrypt
            var hasDataModel = GetHashColumns(encryptedCols, hashColumns);
            var listHash = hasDataModel.HashColumns;
            var listDecrypt = hasDataModel.DecryptColumns;

            //Collect the hash value of the hash columns
            GetAllHashValue(data, listHash);

            //Get clear text from list hash values
            GetClearTextValue(data, listHash);            

            //Get clear text from list decrypted values
            foreach (string col in listDecrypt.Where(x => data.Columns.Contains(x)))
            {
                foreach (DataRow dtRow in data.Rows)
                {
                    dtRow[col] = DecryptText(dtRow[col].ToString(), ASClientID, path);
                }
            }

        }
        public void HashData(ReportRequestModel request, DataTable data, string path)
        {
            if ((request.HashColumns.Any() || request.DecryptColumns.Any()) && data != null && data.Rows.Count > 0 && !request.IsExport)
            {
                var headers = data.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                request.HashColumns = request.HashColumns.Where(x => headers.Any(header => header.Equals(x.ColumnName, StringComparison.OrdinalIgnoreCase))).ToList();
                request.DecryptColumns = request.DecryptColumns.Where(x => headers.Any(header => header.Equals(x, StringComparison.OrdinalIgnoreCase))).ToList();

                if (!request.HashColumns.Any() && !request.DecryptColumns.Any())
                {
                    return;
                }
                //add original column
                AddOriginalColumn(data, request.DecryptColumns);
                AddOriginalColumn(data, request.HashColumns.Select(x => x.ColumnName).ToList());

                foreach (DataRow row in data.Rows)
                {
                    GetDecryptedValue(request.AsClientId, row, request.DecryptColumns, path);
                    GetHashValue(row, request.HashColumns);
                }

                if (request.HashColumns.Any())
                {
                    //Get clear text from list hash values
                    foreach (HashColEntity entity in request.HashColumns)
                    {
                        entity.TrimHash();
                        if (!string.IsNullOrEmpty(entity.HashValue))
                        {
                            DataTable hashTable = GetHashDataTable(entity.HashValue);
                            string query = "Stat = '{0}'";
                            foreach (DataRow dtRow in data.Rows)
                            {
                                DataRow hRow = hashTable != null && hashTable.Rows.Count > 0 ? hashTable.Select(string.Format(query, dtRow[entity.ColumnName].ToString())).FirstOrDefault() : null;
                                if (hRow != null)
                                    dtRow[entity.ColumnName] = hRow["StatData"];
                                else
                                    dtRow[entity.ColumnName] = string.Empty;
                            }
                        }
                    }
                }
            }
        }
        private void HashRowData(DataRow row, ReportRequestModel request)
        {
            string query = "Stat = '{0}'";
            foreach (var entity in request.HashColumns)
            {
                if (!string.IsNullOrEmpty(entity.HashValue) && entity.RawData != null && entity.RawData.Rows.Count > 0)
                {
                    DataRow hRow = entity.RawData.Select(string.Format(query, row[entity.ColumnName].ToString())).FirstOrDefault();
                    if (hRow != null)
                        row[entity.ColumnName] = hRow["StatData"];
                    else
                        row[entity.ColumnName] = string.Empty;
                }
                else
                    row[entity.ColumnName] = string.Empty;
            }
        }

        private HashDataModel GetHashColumns(string[] encryptedCols, string hashColumns)
        {
            var result = new HashDataModel() { DecryptColumns = new List<string>(), HashColumns = new List<HashColEntity>() };

            //Collect the hash columns need to decrypt
            foreach (string encryptedColumn in encryptedCols)
            {
                if (!string.IsNullOrEmpty(encryptedColumn))
                {
                    if (hashColumns.Contains(string.Format(",{0},", encryptedColumn)))
                        result.HashColumns.Add(new HashColEntity(encryptedColumn, ""));
                    else
                        result.DecryptColumns.Add(encryptedColumn);
                }
            }

            return result;
        }
        private void GetAllHashValue(DataTable data, List<HashColEntity> hashColumns)
        {
            if (hashColumns!= null && hashColumns.Any())
            {
                foreach (DataRow row in data.Rows)
                {
                    foreach (HashColEntity entity in hashColumns)
                    {
                        if (data.Columns.Contains(entity.ColumnName) && !string.IsNullOrEmpty(row[entity.ColumnName].ToString()))
                            entity.AddHashValue(row[entity.ColumnName].ToString());
                    }
                }
            }
        }
        private void GetClearTextValue(DataTable data, List<HashColEntity> hashColumns)
        {
            if (hashColumns != null && hashColumns.Any())
            {
                foreach (HashColEntity entity in hashColumns)
                {
                    entity.TrimHash();
                    if (!string.IsNullOrEmpty(entity.HashValue))
                    {
                        DataTable hashTable = GetHashDataTable(entity.HashValue);
                        string query = "Stat = '{0}'";
                        foreach (DataRow dtRow in data.Rows)
                        {
                            DataRow hRow = hashTable != null && hashTable.Rows.Count > 0 ? hashTable.Select(string.Format(query, dtRow[entity.ColumnName].ToString())).FirstOrDefault() : null;
                            if (hRow != null)
                                dtRow[entity.ColumnName] = hRow["StatData"];
                            else
                                dtRow[entity.ColumnName] = string.Empty;
                        }
                    }
                }
            }            
        }
    }
}
