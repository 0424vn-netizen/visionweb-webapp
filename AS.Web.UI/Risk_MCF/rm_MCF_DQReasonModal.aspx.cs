/* Create by: Hao Dang
 * Ticket: 43784 Queue Enhancements 
 */

using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Linq;
using BuGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;

namespace As.VisionWeb.Web
{
    [PagePermission("RskRP,MSRskRP,RskQueue,MSRskQueue")]
    public partial class RiskManagementDetectionQueueReasonModal : ReportPage
    {
        #region --- Variable & Enum ---
        private const string PARAMETER_VALUE = "ParameterValue";
        private const string ACTUAL_VALUE = "ActualValue";
        private const string PARAMETER_THRESHOLD = "ParameterThreshold";
        private const string ACTUAL_THRESHOLD = "ActualThreshold";
        private const string ALERT_DATE = "AlertDate";
        private const string WORKED_DATE = "WorkedDate";
        private const string ASSIGNMENT_NAME = "AssignmentName";
        private const string IS_PARAMETERS_AND = "IsParametersAnd";
        private const string IS_PARAMETER_GROUP = "IsParameterGroup";
        private const string PARAMETER_KEY = "ParameterKey";
        private const string PARAMETER_NAME = "ParameterDescription";
        private const string IS_SHOW_LINK = "IsShowLink";
        enum DataBindAction
        {
            BindTitleInfo,
            BindDQDetailsGrid_Current
        }

        #region Properties

        private const string MERCHANT_NUMBER = "MerchantNumber";
        private const string REPORT_DATE = "ReportDate";
        private const string ASSIGNMENT_ID = "AssignmentID";
        private string _keyGroupGrid = string.Empty;
        private int _groupIndex = 1;
        private readonly string[] _columnGroup = new string[] { "CheckBoxColumn", "AlertDate", "AssignmentName", "Disposition", "WorkedDate", "UserName" };
        private string merchantName = string.Empty;

        protected string MerchantNumber
        {
            get
            {
                if (string.IsNullOrEmpty(SecureQueryString[MERCHANT_NUMBER]))
                    return "";
                return SecureQueryString[MERCHANT_NUMBER];
            }
        }

        protected DateTime ReportDate
        {
            get
            {
                if (string.IsNullOrEmpty(SecureQueryString[REPORT_DATE]))
                    return DateTime.Now;
                return DateTime.Parse(SecureQueryString[REPORT_DATE]);
            }
        }
        private List<string> _DisplayTextForParameters { get; set; }
        protected List<string> DisplayTextForParameters
        {
            get
            {
                if (_DisplayTextForParameters == null)
                {
                    var paramsDisplayText = GeneralFuncsLib.GetDataOfExtendedSetting("DisplayTextForActualValue1", "0");

                    if (string.IsNullOrEmpty(paramsDisplayText))
                        paramsDisplayText = "P147,P148";

                    _DisplayTextForParameters = paramsDisplayText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }

                return _DisplayTextForParameters;
            }
        }

        #endregion

        #endregion --- Variable & Enum ---

        #region ---- Private Methods -----

        // 10/10/2018 |41044 PIVOT New Parameter
        private string GetTopCardText(string number)
        {
            int topCardNumber;
            number = number.Substring(0, number.IndexOf(".") >= 0 ? number.IndexOf(".") : number.Length);
            int.TryParse(number, out topCardNumber);
            string topCardText = null;
            if (topCardNumber > 0)
            {
                topCardText = GetLocalResourceObject("rm_DQReasonModal_aspx_cs_TopCard").ToString();
                if (topCardNumber > 1)
                {
                    topCardText = string.Format(GetLocalResourceObject("rm_DQReasonModal_aspx_cs_TopCards").ToString(), topCardNumber);
                }
            }

            return topCardText;
        }

        private string GetActualText(string number, string paramKey)
        {
            if (string.IsNullOrEmpty(number)) return string.Empty;

            int numberVal;
            number = number.Substring(0, number.IndexOf(".") >= 0 ? number.IndexOf(".") : number.Length);
            int.TryParse(number, out numberVal);

            if (IsDisplayAsText(paramKey))
            {
                return string.Format("({0})", number);
            }

            return string.Format("(${0})", numberVal);
        }

        private bool IsDisplayAsText(string paramKey)
        {
            if (!string.IsNullOrEmpty(paramKey) && DisplayTextForParameters != null && DisplayTextForParameters.Any())
            {
                var isDisplayText = DisplayTextForParameters.Any(x => x.Equals(paramKey, StringComparison.OrdinalIgnoreCase));
                return isDisplayText;
            }

            return false;
        }

        private void ItemDataBound_GridView(GridDataItem dataItem, DataRow rowItem)
        {
            //process workState
            GenWorkState(dataItem, rowItem);
            dataItem["Parameter"].ToolTip = rowItem["ParameterDescription"].ToString();
            string parameterCount = "1";
            if (!string.IsNullOrEmpty(GeneralFuncsLib.NvlString(rowItem["ParameterCount"])))
            {
                parameterCount = GeneralFuncsLib.NvlString(rowItem["ParameterCount"]);
            }

            string group = GeneralFuncsLib.NvlString(rowItem["Rank"]);

            //process Group
            if (string.IsNullOrEmpty(_keyGroupGrid))
            {
                _keyGroupGrid = group;
                AddGroupColum(dataItem, true, parameterCount);
            }
            else
            {
                string keyGroup = group;
                if (_keyGroupGrid.Equals(keyGroup))
                {
                    AddGroupColum(dataItem, false);

                }
                else
                {
                    _groupIndex++;
                    _keyGroupGrid = keyGroup;
                    AddGroupColum(dataItem, true, parameterCount);
                }
            }


            if (_groupIndex % 2 != 0)
            {
                dataItem.CssClass = "rgRow";
            }
            else
            {
                dataItem.CssClass = "rgAltRow";
            }

            var fileTypecolumn = dataItem["FileType"];
            fileTypecolumn.ToolTip = rowItem["FileName"].ToString();
            //51270 - Risk DQ Details modal to support GroupedMatch All
            if (!rowItem[IS_PARAMETERS_AND].IsNullOrEmpty())
            {
                var assignmentName = HttpUtility.UrlEncode(rowItem[ASSIGNMENT_NAME].ToString());
                var queryString = "MerchantNumber=" + MerchantNumber + "&AssignmentName="
                                                                + assignmentName + "&AssignmentID=" + rowItem[ASSIGNMENT_ID]
                                                                + "&ReportDate=" + ReportDate.ToString()
                                                                + "&AlertDate=" + rowItem[ALERT_DATE].ToString();
                string text;
                if (!Convert.ToBoolean(rowItem[IS_PARAMETERS_AND].ToString()))
                {
                    queryString += "&Group=1";
                    text = this.GetLocalResourceObject("rm_mcf_Grouped_Parameter").ToString();
                }
                else
                {
                    queryString += "&Group=0";
                    text = this.GetLocalResourceObject("rm_MCF_Match_Every_Parameter").ToString();
                }

                if (!rowItem[IS_SHOW_LINK].IsNullOrEmpty() && Convert.ToBoolean(rowItem[IS_SHOW_LINK]))
                {
                    queryString = this.BuildSecureQueryString(queryString);
                    string url = "rm_MCF_MatchEveryParameter.aspx?" + queryString;
                    string link = string.Format("{0}<br/><a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"showMatchEveryParameter('{1}'); return false;\">({2})</a>"
                                                , rowItem[ASSIGNMENT_NAME], url, text);
                    dataItem[ASSIGNMENT_NAME].Text = link;
                }
                else
                {
                    dataItem[ASSIGNMENT_NAME].Text = string.Format("{0}<br/>({1})", rowItem[ASSIGNMENT_NAME], text);
                }
            }
            var borderBottomRed = "";
            if (!rowItem[IS_PARAMETER_GROUP].IsNullOrEmpty()
                && Convert.ToBoolean(rowItem[IS_PARAMETER_GROUP].ToString()))
            {
                borderBottomRed = "border-bottom: 3px red solid;";
                dataItem[PARAMETER_KEY].Text = string.Format("<span style=\"{1}\">{0}</span>", rowItem[PARAMETER_KEY].ToString(), borderBottomRed);
            }

            if (!rowItem[PARAMETER_KEY].IsNullOrEmpty() && IsParametersViolationInConfig(rowItem[PARAMETER_KEY].ToString()))
            {
                var assignmentName = HttpUtility.UrlEncode(rowItem[ASSIGNMENT_NAME].ToString());
                var queryString = "MerchantNumber=" + MerchantNumber + "&AssignmentName="
                                                                + assignmentName + "&AssignmentID=" + rowItem[ASSIGNMENT_ID]
                                                                + "&ReportDate=" + ReportDate.ToString()
                                                                + "&ParameterKey=" + rowItem[PARAMETER_KEY].ToString()
                                                                + "&ParameterName=" + rowItem["Parameter"].ToString();

                queryString = this.BuildSecureQueryString(queryString);
                string url = "rm_MCF_ParameterViolationDetailsModal.aspx?" + queryString;
                string link = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer; {0}\" onclick=\"showParameterViolationDetailsModal('{1}'); return false;\">{2}</a>"
                                            , borderBottomRed, url, rowItem[PARAMETER_KEY].ToString());

                dataItem[PARAMETER_KEY].Text = link;
            }

            if (!rowItem[ALERT_DATE].IsNullOrEmpty())
            {
                string alertDt = rowItem[ALERT_DATE].ToString();
                string dt = alertDt.Split(' ')[0];
                dataItem[ALERT_DATE].Text = string.Format("{0}{1}{2}", dt, "<br/>", alertDt.Substring(dt.Length).Trim());
            }

            if (!rowItem[WORKED_DATE].IsNullOrEmpty())
            {
                string workedDt = rowItem[WORKED_DATE].ToString();
                string dt = workedDt.Split(' ')[0];
                dataItem[WORKED_DATE].Text = string.Format("{0}{1}{2}", dt, "<br/>", workedDt.Substring(dt.Length).Trim());
            }

            Dictionary<string, string> dicValue = new Dictionary<string, string>();
            ProcessData(rowItem, dicValue, dataItem);
            dataItem[PARAMETER_VALUE].Text = dicValue[PARAMETER_VALUE];
            dataItem[ACTUAL_VALUE].Text = dicValue[ACTUAL_VALUE];
            dataItem[PARAMETER_THRESHOLD].Text = dicValue[PARAMETER_THRESHOLD];
            dataItem[ACTUAL_THRESHOLD].Text = dicValue[ACTUAL_THRESHOLD];

            // text alignment
            GridTableCell tableCellParameterKey = (GridTableCell)dataItem["ParameterKey"];
            tableCellParameterKey.HorizontalAlign = HorizontalAlign.Center;
            GridTableCell tableCellDisposition = (GridTableCell)dataItem["Disposition"];
            tableCellDisposition.HorizontalAlign = HorizontalAlign.Left;
        }

        private void ProcessData(DataRow rowItem, Dictionary<string, string> dicValue, GridDataItem dataItem = null)
        {
            dicValue.Add(PARAMETER_VALUE, string.Empty);
            dicValue.Add(ACTUAL_VALUE, string.Empty);
            dicValue.Add(PARAMETER_THRESHOLD, string.Empty);
            dicValue.Add(ACTUAL_THRESHOLD, string.Empty);

            string parameterThreshold = rowItem[PARAMETER_THRESHOLD].ToString();
            string actualThreshold = rowItem[ACTUAL_THRESHOLD].ToString();
            string dataType = rowItem["DataType"].ToString().ToLower();
            string parameterValue = rowItem[PARAMETER_VALUE].ToString();
            string parameterValueHigh = rowItem["ParameterValueHigh"].ToString();
            string actualValue = rowItem[ACTUAL_VALUE].ToString();
            int parameterPrecision = int.Parse(rowItem["ParameterPrecision"].ToString());
            string indicatorFormat = "#,##0" + (parameterPrecision > 0 ? ".".PadRight(parameterPrecision + 1, '0') : string.Empty);
            string thresholdType = rowItem["ThresholdType"].ToString().ToLower();
            string parameterThresholdHigh = rowItem["ParameterThresholdHigh"].ToString();
            string parameterThresholdType = rowItem["ParameterThresholdType"].ToString();


            if (!parameterValue.Equals("0") && !string.IsNullOrEmpty(parameterValue))
            {
                parameterValue = decimal.Parse(parameterValue).ToString(indicatorFormat);
            }
            if (!parameterValueHigh.Equals("0") && !string.IsNullOrEmpty(parameterValueHigh))
            {
                parameterValueHigh = decimal.Parse(parameterValueHigh).ToString(indicatorFormat);
            }

            if (!actualValue.Equals("0") && !string.IsNullOrEmpty(actualValue))
            {
                actualValue = decimal.Parse(actualValue).ToString(indicatorFormat);
            }

            if (!parameterThreshold.Equals("0") && !string.IsNullOrEmpty(parameterThreshold))
            {
                parameterThreshold = decimal.Parse(parameterThreshold).ToString(indicatorFormat);
            }

            if (!actualThreshold.Equals("0") && !string.IsNullOrEmpty(actualThreshold))
            {
                actualThreshold = decimal.Parse(actualThreshold).ToString(indicatorFormat);
            }

            if (!parameterThresholdHigh.Equals("0") && !string.IsNullOrEmpty(parameterThresholdHigh))
            {
                parameterThresholdHigh = decimal.Parse(parameterThresholdHigh).ToString(indicatorFormat);
            }

            // Format for export
            if (dataItem == null)
            {
                if (!string.IsNullOrEmpty(parameterValueHigh))
                {
                    parameterValue = GeneralFuncsLib.FormatLowHighThreshold(parameterValue, parameterValueHigh, dataType);
                }
                else
                {
                    parameterValue = GeneralFuncsLib.FormatParameterExport(dataType, parameterValue, parameterPrecision);
                }

                actualValue = GeneralFuncsLib.FormatParameterExport(dataType, actualValue, parameterPrecision);

                switch (parameterThresholdType)
                {
                    case "LowHigh":
                        parameterThreshold = GeneralFuncsLib.FormatLowHighThreshold(parameterThreshold, parameterThresholdHigh, thresholdType);
                        break;
                    default:
                        parameterThreshold = GeneralFuncsLib.FormatParameterExport(thresholdType, parameterThreshold, 0);
                        break;
                }

                actualThreshold = GeneralFuncsLib.FormatParameterExport(thresholdType, actualThreshold, parameterPrecision);
            }
            else
            {
                //Format for Grid
                if (!parameterValue.IsNullOrEmpty()
                    && dataItem != null
                    && decimal.Parse(parameterValue) < 0)
                {
                    dataItem[PARAMETER_VALUE].Style.Add("color", "Red");
                }

                if (!actualValue.IsNullOrEmpty()
                    && dataItem != null
                    && (decimal.Parse(actualValue) < 0))
                {
                    dataItem[ACTUAL_VALUE].Style.Add("color", "Red");
                }

                if (!parameterThreshold.IsNullOrEmpty()
                    && dataItem != null
                    && decimal.Parse(parameterThreshold) < 0)
                {
                    dataItem[PARAMETER_THRESHOLD].Style.Add("color", "Red");
                }

                if (!actualThreshold.IsNullOrEmpty()
                    && dataItem != null
                    && decimal.Parse(actualThreshold) < 0)
                {
                    dataItem[ACTUAL_THRESHOLD].Style.Add("color", "Red");
                }

                //process indicator
                if (rowItem["ParameterValueHigh"] != DBNull.Value)
                {
                    parameterValue = VeraCodeSolution.ValidateResponseData(
                             GeneralFuncsLib.FormatLowHighThreshold(parameterValue, parameterValueHigh, dataType));
                }
                else if (!rowItem[PARAMETER_VALUE].Equals(DBNull.Value))
                {
                    parameterValue = VeraCodeSolution.ValidateResponseData(
                        GeneralFuncsLib.FormatParameterDataType(dataType, parameterValue, parameterPrecision));
                }

                actualValue = VeraCodeSolution.ValidateResponseData(
                      GeneralFuncsLib.FormatParameterDataType(dataType, actualValue, parameterPrecision));

                switch (parameterThresholdType)
                {
                    case "LowHigh":
                        parameterThreshold = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.FormatLowHighThreshold(parameterThreshold, parameterThresholdHigh, thresholdType));
                        break;
                    default:
                        parameterThreshold = VeraCodeSolution.ValidateResponseData(
                            GeneralFuncsLib.FormatParameterDataType(thresholdType, parameterThreshold));
                        break;
                }

                actualThreshold = VeraCodeSolution.ValidateResponseData(
                    GeneralFuncsLib.FormatParameterDataType(thresholdType, actualThreshold, parameterPrecision));
            }

            // 10/10/2018 |41044 PIVOT New Parameter
            string topCard = GetTopCardText(rowItem["ActualThreshold1"].ToSafeString());
            if (!topCard.IsNullOrEmpty())
            {
                actualThreshold = string.Format("{0} {1} {2}", actualThreshold, dataItem == null ? Environment.NewLine : "<br/>", topCard);
            }

            if (rowItem["ActualValue1"] != null)
            {
                var paramKey = rowItem[PARAMETER_KEY].ToString();
                string actualValue1 = GetActualText(rowItem["ActualValue1"].ToSafeString(), paramKey);

                if (!actualValue1.IsNullOrEmpty())
                {
                    if (IsDisplayAsText(paramKey))
                    {
                        actualValue = string.Format("{0}{1}{2}", actualValue, dataItem == null ? Environment.NewLine : " - ", actualValue1);
                    }
                    else
                    {
                        actualValue = string.Format("{0} {1} {2}", actualValue, dataItem == null ? Environment.NewLine : "<br/>", actualValue1);
                    }
                }
            }


            // Check to show N/A
            if (rowItem[PARAMETER_VALUE].Equals(DBNull.Value))
            {
                parameterValue = VeraCodeSolution.DoVeraCode("N/A");
            }

            if (rowItem[ACTUAL_VALUE].Equals(DBNull.Value))
            {
                actualValue = VeraCodeSolution.DoVeraCode("N/A");
            }

            if (rowItem[PARAMETER_THRESHOLD].Equals(DBNull.Value))
            {
                parameterThreshold = VeraCodeSolution.DoVeraCode("N/A");
            }

            if (rowItem[ACTUAL_THRESHOLD].Equals(DBNull.Value))
            {
                actualThreshold = VeraCodeSolution.DoVeraCode("N/A");
            }

            //PARAMETER_VALUE
            if (dicValue.ContainsKey(PARAMETER_VALUE))
                dicValue[PARAMETER_VALUE] = parameterValue;
            else
                dicValue.Add(PARAMETER_VALUE, parameterValue);

            //ACTUAL_VALUE
            if (dicValue.ContainsKey(ACTUAL_VALUE))
                dicValue[ACTUAL_VALUE] = actualValue;
            else
                dicValue.Add(ACTUAL_VALUE, actualValue);

            //PARAMETER_THRESHOLD
            if (dicValue.ContainsKey(PARAMETER_THRESHOLD))
                dicValue[PARAMETER_THRESHOLD] = parameterThreshold;
            else
                dicValue.Add(PARAMETER_THRESHOLD, parameterThreshold);

            // ACTUAL_THRESHOLD
            if (dicValue.ContainsKey(ACTUAL_THRESHOLD))
                dicValue[ACTUAL_THRESHOLD] = actualThreshold;
            else
                dicValue.Add(ACTUAL_THRESHOLD, actualThreshold);
        }

        private void AddGroupColum(GridDataItem dataItem, bool isGroup, string rowSpan = "1")
        {
            foreach (string item in _columnGroup)
            {
                GridTableCell tableCell = (GridTableCell)dataItem[item];
                if (isGroup)
                {
                    tableCell.Attributes["rowspan"] = rowSpan;
                    tableCell.Style.Add("vertical-align", "top");
                }
                else
                {
                    tableCell.Style.Add("display", "none");
                }
            }
        }

        private void GenWorkState(GridDataItem dataItem, DataRow rowItem)
        {
            var workStateID = string.IsNullOrEmpty(rowItem["WorkStateID"].ToString()) ?
                 0 : int.Parse(rowItem["WorkStateID"].ToString());
            Label ctrl = dataItem.FindControl("uxlblWork") as Label;

            var workState = (WebSiteEnums.WorkStatus)workStateID;
            switch (workState)
            {
                case WebSiteEnums.WorkStatus.Work:
                    ctrl.Text = GetLocalResourceObject("uxReadyToWorkResource_Text").ToString();
                    break;
                case WebSiteEnums.WorkStatus.Worked:
                    ctrl.Text = GetLocalResourceObject("uxWorkedResource_Text").ToString();
                    break;
                case WebSiteEnums.WorkStatus.WorkInProgress:
                    ctrl.Text = GetLocalResourceObject("uxWIPResource_Text").ToString();
                    break;
                case WebSiteEnums.WorkStatus.WorkInProgressByOther:
                    ctrl.Text = GetLocalResourceObject("uxWIPByOtherResource_Text1").ToString();
                    break;

            }
        }

        #region Export

        private DataTable GetDataExport(DataTable tb)
        {

            DataTable tb1 = tb.Clone();
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                Dictionary<string, string> dicValue = new Dictionary<string, string>();
                ProcessData(tb.Rows[i], dicValue);

                DataRow row = tb1.NewRow();
                if (tb.Rows[i]["IsParametersAnd"].IsNullOrEmpty() || !Convert.ToBoolean(tb.Rows[i]["IsParametersAnd"].ToString()))
                {
                    row["AssignmentName"] = tb.Rows[i]["AssignmentName"] + Environment.NewLine + string.Format("({0})", this.GetLocalResourceObject("rm_mcf_Grouped_Parameter"));
                }
                else
                {
                    row["AssignmentName"] = tb.Rows[i]["AssignmentName"] + Environment.NewLine + string.Format("({0})", this.GetLocalResourceObject("rm_MCF_Match_Every_Parameter"));
                }
                row["Parameter"] = tb.Rows[i]["Parameter"];
                row["AlertDate"] = tb.Rows[i]["AlertDate"];
                row["ReAlert"] = tb.Rows[i]["ReAlert"];
                row[PARAMETER_VALUE] = dicValue[PARAMETER_VALUE];
                row[ACTUAL_VALUE] = dicValue[ACTUAL_VALUE];
                row[PARAMETER_THRESHOLD] = dicValue[PARAMETER_THRESHOLD];
                row[ACTUAL_THRESHOLD] = dicValue[ACTUAL_THRESHOLD];
                row["Code"] = tb.Rows[i]["Code"];
                row["FileType"] = tb.Rows[i]["FileType"];
                row["UserName"] = tb.Rows[i]["UserName"];
                row["WorkedDate"] = tb.Rows[i]["WorkedDate"];
                row["Disposition"] = tb.Rows[i]["Disposition"];
                row["Disposition"] = tb.Rows[i]["Disposition"];
                row["ParameterKey"] = tb.Rows[i]["ParameterKey"];
                row["CurrentStatusDesc"] = tb.Rows[i]["CurrentStatusDesc"];

                tb1.Rows.Add(row);
            }
            return tb1;
        }
        private DataTable GetDataExportSheet2(DataTable tb)
        {

            DataTable tb1 = tb.Clone();
            tb1.Columns[PARAMETER_VALUE].DataType = typeof(string);
            tb1.Columns[ACTUAL_VALUE].DataType = typeof(string);
            tb1.Columns[PARAMETER_THRESHOLD].DataType = typeof(string);
            tb1.Columns[ACTUAL_THRESHOLD].DataType = typeof(string);
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                Dictionary<string, string> dicValue = new Dictionary<string, string>();
                ProcessData(tb.Rows[i], dicValue);

                DataRow row = tb1.NewRow();
                if (tb.Rows[i]["IsParametersAnd"].IsNullOrEmpty() || !Convert.ToBoolean(tb.Rows[i]["IsParametersAnd"].ToString()))
                {
                    row["AssignmentName"] = tb.Rows[i]["AssignmentName"] + Environment.NewLine + string.Format("({0})", this.GetLocalResourceObject("rm_mcf_Grouped_Parameter"));
                }
                else
                {
                    row["AssignmentName"] = tb.Rows[i]["AssignmentName"] + Environment.NewLine + string.Format("({0})", this.GetLocalResourceObject("rm_MCF_Match_Every_Parameter"));
                }
                row["Parameter"] = tb.Rows[i]["Parameter"];
                row["ReAlert"] = tb.Rows[i]["ReAlert"];
                row[PARAMETER_VALUE] = dicValue[PARAMETER_VALUE];
                row[ACTUAL_VALUE] = dicValue[ACTUAL_VALUE];
                row[PARAMETER_THRESHOLD] = dicValue[PARAMETER_THRESHOLD];
                row[ACTUAL_THRESHOLD] = dicValue[ACTUAL_THRESHOLD];
                row["ParameterKey"] = tb.Rows[i]["ParameterKey"];
                row["ViolatedOn"] = tb.Rows[i]["ViolatedOn"];
                row["FileTypeCodes"] = tb.Rows[i]["FileTypeCodes"];
                tb1.Rows.Add(row);
            }
            return tb1;
        }

        private DataTable GetDataExportSheet3(DataTable tb)
        {
            DataTable tb1 = tb.Clone();
            tb1.Columns["ParameterName"].DataType = typeof(string);
            tb1.Columns["ParameterDescription"].DataType = typeof(string);

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                DataRow row = tb1.NewRow();
                row["ParameterName"] = tb.Rows[i]["ParameterKey"] + "-" + tb.Rows[i]["ParameterName"];
                row["ParameterDescription"] = tb.Rows[i]["ParameterDescription"];
                tb1.Rows.Add(row);
            }
            return tb1;
        }

        private List<string> GetColumnSheet1()
        {
            StringBuilder columnHeaders = new StringBuilder();
            StringBuilder ColumnNames = new StringBuilder();
            StringBuilder ColumnFormat = new StringBuilder();
            StringBuilder ColumnTitle = new StringBuilder();
            List<string> result = new List<string>();
            columnHeaders.Append(this.GetLocalResourceObject("ASGridBoundColumnResource01.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource02.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource03.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource04.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource05.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource14.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource06.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource07.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource08.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource09.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource10.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource11.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource12.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource13.HeaderText"));

            ColumnNames.Append("CurrentStatusDesc");
            ColumnNames.Append(",AlertDate");
            ColumnNames.Append(",AssignmentName");
            ColumnNames.Append(",ParameterKey");
            ColumnNames.Append(",Parameter");
            ColumnNames.Append(",ReAlert");
            ColumnNames.Append(",ParameterValue");
            ColumnNames.Append(",ActualValue");
            ColumnNames.Append(",ParameterThreshold");
            ColumnNames.Append(",ActualThreshold");
            ColumnNames.Append(",Disposition");
            ColumnNames.Append(",WorkedDate");
            ColumnNames.Append(",UserName");
            ColumnNames.Append(",FileType");

            ColumnFormat.Append("dynamicstring");
            ColumnFormat.Append(",datetimeshorttime");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",datetimeshorttime");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");

            merchantName = GeneralFuncsLib.GetMerchantName(MerchantNumber);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text7"));
            ColumnTitle.Append(Environment.NewLine);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text5"));
            ColumnTitle.Append(Environment.NewLine);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text2") + MerchantNumber.ToString() + " - " + merchantName);
            ColumnTitle.Append(Environment.NewLine);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text3") + ReportDate.ToString("MM/dd/yyyy"));
            ColumnTitle.Append(Environment.NewLine);

            result.Add(columnHeaders.ToString());
            result.Add(ColumnNames.ToString());
            result.Add(ColumnFormat.ToString());
            result.Add(ColumnTitle.ToString());
            return result;
        }
        private List<string> GetColumnSheet2()
        {
            StringBuilder columnHeaders = new StringBuilder();
            StringBuilder ColumnNames = new StringBuilder();
            StringBuilder ColumnFormat = new StringBuilder();
            List<string> result = new List<string>();
            columnHeaders.Append(this.GetLocalResourceObject("Assignment_Name_Export"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource04.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource05.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource14.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("The_Latest_Violated_Time_Export"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource06.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource07.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource08.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource09.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource13.HeaderText"));

            ColumnNames.Append("AssignmentName");
            ColumnNames.Append(",ParameterKey");
            ColumnNames.Append(",Parameter");
            ColumnNames.Append(",ReAlert");
            ColumnNames.Append(",ViolatedOn");
            ColumnNames.Append(",ParameterValue");
            ColumnNames.Append(",ActualValue");
            ColumnNames.Append(",ParameterThreshold");
            ColumnNames.Append(",ActualThreshold");
            ColumnNames.Append(",FileTypeCodes");

            ColumnFormat.Append("dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",datetimeshorttime");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");
            ColumnFormat.Append(",dynamicstring");

            StringBuilder ColumnTitle = new StringBuilder();
            merchantName = GeneralFuncsLib.GetMerchantName(MerchantNumber);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_mcf_Grouped_Parameter"));
            ColumnTitle.Append("|");
            ColumnTitle.Append(this.GetLocalResourceObject("rm_MCF_Match_Every_Parameter"));
            ColumnTitle.Append(Environment.NewLine);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text5"));
            ColumnTitle.Append(Environment.NewLine);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text2") + MerchantNumber.ToString() + " - " + merchantName);
            ColumnTitle.Append(Environment.NewLine);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text3") + ReportDate.ToString("MM/dd/yyyy"));
            ColumnTitle.Append(Environment.NewLine);

            result.Add(columnHeaders.ToString());
            result.Add(ColumnNames.ToString());
            result.Add(ColumnFormat.ToString());
            result.Add(ColumnTitle.ToString());
            return result;
        }

        private List<string> GetColumnSheet3()
        {
            StringBuilder columnHeaders = new StringBuilder();
            StringBuilder ColumnNames = new StringBuilder();
            StringBuilder ColumnFormat = new StringBuilder();
            List<string> result = new List<string>();
            columnHeaders.Append(this.GetLocalResourceObject("ASGridBoundColumnResource05.HeaderText"));
            columnHeaders.Append("," + this.GetLocalResourceObject("ASGridBoundColumnResource15.HeaderText"));

            ColumnNames.Append("ParameterName");
            ColumnNames.Append(",ParameterDescription");

            ColumnFormat.Append("dynamicstring");
            ColumnFormat.Append(",dynamicstring");

            StringBuilder ColumnTitle = new StringBuilder();
            merchantName = GeneralFuncsLib.GetMerchantName(MerchantNumber);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_mcf_Grouped_Parameter"));
            ColumnTitle.Append("|");
            ColumnTitle.Append(this.GetLocalResourceObject("rm_MCF_Match_Every_Parameter"));
            ColumnTitle.Append(Environment.NewLine);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text5"));
            ColumnTitle.Append(Environment.NewLine);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text2") + MerchantNumber.ToString() + " - " + merchantName);
            ColumnTitle.Append(Environment.NewLine);
            ColumnTitle.Append(this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text3") + ReportDate.ToString("MM/dd/yyyy"));
            ColumnTitle.Append(Environment.NewLine);

            result.Add(columnHeaders.ToString());
            result.Add(ColumnNames.ToString());
            result.Add(ColumnFormat.ToString());
            result.Add(ColumnTitle.ToString());
            return result;
        }

        private static bool IsParametersViolationInConfig(string paramID)
        {
            if (paramID == null || string.IsNullOrEmpty(paramID))
            {
                return false;
            }
            //Risk Parameters Violation config
            return BuGeneralFuncsLib.CheckExistsInSplitToArray(WebSiteSettings.RiskParametersViolation, ',', paramID);
        }
        #endregion

        #endregion ---- Private Methods -----

        #region ---- Events Handle ----

        protected override void PageInitialize()
        {
            this.GridIDs.Add("uxDQOthers");
            this.ExporterIDs.Add("uxExportDQOthersTop");
            base.PageInitialize();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            uxh2Export.Attributes.Add("data-target", "#" + uxReportGrid.ClientID);
            uxh2Export.InnerHtml = this.GetLocalResourceObject("uxExporterResource1.GridHeader").ToString() + "<span class=\"text - muted\"></span>";
            IsBindDataOnLoad = true;

            OnDataBindControls(DataBindAction.BindTitleInfo);

            //46652 - AW - Multi-Currency Transaction Display
            uxReportGrid.Columns.FindByUniqueName(PARAMETER_VALUE).HeaderText = uxReportGrid.Columns.FindByUniqueName(PARAMETER_VALUE).HeaderText.ToCurrencySymbol();
            uxReportGrid.Columns.FindByUniqueName(ACTUAL_VALUE).HeaderText = uxReportGrid.Columns.FindByUniqueName(ACTUAL_VALUE).HeaderText.ToCurrencySymbol();
        }

        protected DataTable GetDQReasonData()
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
            parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
            string spaName = "spa_RM_MCF_GetDQReasons";
            parameters.AddLanguageID();
            return WebServices.RiskServices.GetReports(spaName, parameters);
        }

        protected DataSet GetDQReasonDataExport()
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
            parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
            string spaName = "spa_RM_MCF_GetDQReasons_Export";
            parameters.AddLanguageID();
            return WebServices.RiskServices.GetReportsAsDataSet(spaName, parameters);
        }

        protected override void OnDataBindControls(Enum type, object sender)
        {
            if (this.IsIntruderDetected) return;

            switch ((DataBindAction)type)
            {
                case DataBindAction.BindDQDetailsGrid_Current:
                    {
                        ASGrid grid = (ASGrid)sender;

                        GridSortExpression sortExpr = new GridSortExpression();

                        if (grid.MasterTableView.SortExpressions.GetSortString() != null)
                        {
                            sortExpr.FieldName = grid.MasterTableView.SortExpressions.GetSortString();
                        }

                        DataTable table = GetDQReasonData();
                        grid.DataSource = table;
                    }
                    break;
                case DataBindAction.BindTitleInfo:
                    {
                        merchantName = GeneralFuncsLib.GetMerchantName(MerchantNumber);
                        ltrMerchantInfor.Text = VeraCodeSolution.DoVeraCode(string.Format("{0} <p class='font-20 text-default-gray'>#{1}</p><div class=\"height-18\"></div><p class='font-24'>{2} {3}</p>",
                                                            VeraCodeSolution.ValidateResponseData(merchantName), MerchantNumber,
                                                            GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text3").ToString(),
                                                             ReportDate.ToString(WebSiteConstants.DATE_FORMAT)));
                        Title = string.Format(GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text4").ToString(), MerchantNumber, merchantName);
                    }
                    break;

            }
        }

        protected void uxbtnExport_Click(object sender, EventArgs e)
        {
            if (IsIntruderDetected) return;

            DataSet dataExport = GetDQReasonDataExport();
            var DetectionQueueReasonData = GetDataExport(dataExport.Tables[0]);
            var GroupData = GetDataExportSheet2(dataExport.Tables[1]);
            var ParameterDescriptionData = GetDataExportSheet3(dataExport.Tables[2]);
            List<string> listSheet1 = GetColumnSheet1();
            List<string> listSheet2 = GetColumnSheet2();
            List<string> listSheet3 = GetColumnSheet3();
            List<MultipleSheet> multis = new List<MultipleSheet>();
            multis.Add(new MultipleSheet
            {
                SheetName = this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text7").ToString(),
                ColumnFormatter = string.Empty,
                ExportColumnFormatsForAuto = listSheet1[2],
                ExportColumnHeaders = listSheet1[0],
                ExportColumnNames = listSheet1[1],
                Data = DetectionQueueReasonData,
                ReportTitle = listSheet1[3]
            });
            multis.Add(new MultipleSheet
            {
                SheetName = this.GetLocalResourceObject("rm_DQReasonModal_aspx_cs_Text8").ToString(),
                ColumnFormatter = string.Empty,
                ExportColumnFormatsForAuto = listSheet2[2],
                ExportColumnHeaders = listSheet2[0],
                ExportColumnNames = listSheet2[1],
                Data = GroupData,
                ReportTitle = listSheet2[3]
            });

            multis.Add(new MultipleSheet
            {
                SheetName = GetLocalResourceObject("ASGridBoundColumnResource15.HeaderText").ToString(),
                ColumnFormatter = string.Empty,
                ExportColumnFormatsForAuto = listSheet3[2],
                ExportColumnHeaders = listSheet3[0],
                ExportColumnNames = listSheet3[1],
                Data = ParameterDescriptionData,
                ReportTitle = listSheet3[3]
            });

            ExportHelper.CreateExcelMultipleSheets(string.Format("Detection_Queue_Detail_{0}_{1}.xlsx", MerchantNumber.ToString(), ReportDate.ToString("MM_dd_yyyy")), multis);
        }

        #region Grid events

        protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
        {
            OnDataBindControls(DataBindAction.BindDQDetailsGrid_Current, sender);

        }

        protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                var rowItem = (e.Item.DataItem as DataRowView).Row;

                ItemDataBound_GridView(dataItem, rowItem);
            }
        }
        #endregion

        #endregion ---- Events Handle ----
    }
}
