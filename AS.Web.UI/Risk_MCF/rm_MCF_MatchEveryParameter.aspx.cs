using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Linq;
using System.Web;
using BuGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;

namespace As.VisionWeb.Web
{
    [PagePermission("RskRP,MSRskRP,RskQueue,MSRskQueue")]
    public partial class RiskManagementMatchEveryParameter : ReportPage
    {
        #region --- Variable & Enum ---
        private const string PARAMETER_VALUE = "ParameterValue";
        private const string ACTUAL_VALUE = "ActualValue";
        private const string PARAMETER_THRESHOLD = "ParameterThreshold";
        private const string ACTUAL_THRESHOLD = "ActualThreshold";
        private const string ALERT_DATE = "AlertDate";
        private const string MERCHANT_NUMBER = "MerchantNumber";
        private const string REPORT_DATE = "ReportDate";
        private const string ASSIGNMENT_ID = "AssignmentID";
        private const string ASSIGNMENT_NAME = "AssignmentName";
        private const string GROUP = "Group";
        private const string PARAMETER_KEY = "ParameterKey";
        private const string PARAMETER_NAME = "Parameter";

        enum DataBindAction
        {
            BindTitleInfo,
            BindDQDetailsGrid_Current
        }

        #region Properties


        protected string MerchantNumber
        {
            get
            {
                if (string.IsNullOrEmpty(SecureQueryString[MERCHANT_NUMBER]))
                    return "";
                return SecureQueryString[MERCHANT_NUMBER];
            }
        }
        protected string AssignmentName
        {
            get
            {
                if (string.IsNullOrEmpty(SecureQueryString[ASSIGNMENT_NAME]))
                    return "";
                return SecureQueryString[ASSIGNMENT_NAME];
            }
        }
        protected string AssignmentID
        {
            get
            {
                if (string.IsNullOrEmpty(SecureQueryString[ASSIGNMENT_ID]))
                    return "";
                return SecureQueryString[ASSIGNMENT_ID];
            }
        }
        protected bool Group
        {
            get
            {
                if (string.IsNullOrEmpty(SecureQueryString[GROUP]))
                    return false;
                return Convert.ToInt32(SecureQueryString[GROUP]) == 1;
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
        protected DateTime AlertDate
        {
            get
            {
                if (string.IsNullOrEmpty(SecureQueryString[ALERT_DATE]))
                    return DateTime.Now;
                return DateTime.Parse(SecureQueryString[ALERT_DATE]);
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

            Dictionary<string, string> dicValue = new Dictionary<string, string>();
            ProcessData(rowItem, dicValue, dataItem);
            dataItem[PARAMETER_VALUE].Text = dicValue[PARAMETER_VALUE];
            dataItem[ACTUAL_VALUE].Text = dicValue[ACTUAL_VALUE];
            dataItem[PARAMETER_THRESHOLD].Text = dicValue[PARAMETER_THRESHOLD];
            dataItem[ACTUAL_THRESHOLD].Text = dicValue[ACTUAL_THRESHOLD];

            // text alignment
            GridTableCell tableCellParameterKey = (GridTableCell)dataItem["ParameterKey"];
            tableCellParameterKey.HorizontalAlign = HorizontalAlign.Center;

            if (!rowItem[PARAMETER_KEY].IsNullOrEmpty() && IsParametersViolationInConfig(rowItem[PARAMETER_KEY].ToString()))
            {
                var assignmentName = HttpUtility.UrlEncode(rowItem[ASSIGNMENT_NAME].ToString());
                var queryString = "MerchantNumber=" + MerchantNumber + "&AssignmentName="
                                                                + assignmentName + "&AssignmentID=" + rowItem[ASSIGNMENT_ID]
                                                                + "&ReportDate=" + ReportDate.ToString()
                                                                + "&ParameterKey=" + rowItem[PARAMETER_KEY].ToString()
                                                                + "&ParameterName=" + rowItem[PARAMETER_NAME].ToString();

                queryString = this.BuildSecureQueryString(queryString);
                string url = "rm_MCF_ParameterViolationDetailsModal.aspx?" + queryString;
                string link = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"showParameterViolationDetailsModal('{0}'); return false;\">{1}</a>"
                                            , url, rowItem[PARAMETER_KEY].ToString());

                dataItem[PARAMETER_KEY].Text = link;
            }


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
                if (!parameterValue.IsNullOrEmpty() && dataItem != null
                    && decimal.Parse(parameterValue) < 0)
                {
                    dataItem[PARAMETER_VALUE].Style.Add("color", "Red");
                }

                if (!actualValue.IsNullOrEmpty() && dataItem != null
                    && decimal.Parse(actualValue) < 0)
                {
                    dataItem[ACTUAL_VALUE].Style.Add("color", "Red");
                }

                if (!parameterThreshold.IsNullOrEmpty() && dataItem != null
                    && decimal.Parse(parameterThreshold) < 0)
                {
                    dataItem[PARAMETER_THRESHOLD].Style.Add("color", "Red");
                }

                if (!actualThreshold.IsNullOrEmpty() && dataItem != null
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
                var paramKey = rowItem["ParameterKey"].ToString();
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

        private static bool IsParametersViolationInConfig(string paramID)
        {
            if (paramID == null || string.IsNullOrEmpty(paramID))
            {
                return false;
            }
            //Risk Parameters Violation config
            return BuGeneralFuncsLib.CheckExistsInSplitToArray(WebSiteSettings.RiskParametersViolation, ',', paramID);
        }

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
            PageType = SecurePageType.Modal;
            IsBindDataOnLoad = true;

            OnDataBindControls(DataBindAction.BindTitleInfo);

            //46652 - AW - Multi-Currency Transaction Display
            uxReportGrid.Columns.FindByUniqueName(PARAMETER_VALUE).HeaderText = uxReportGrid.Columns.FindByUniqueName(PARAMETER_VALUE).HeaderText.ToCurrencySymbol();
            uxReportGrid.Columns.FindByUniqueName(ACTUAL_VALUE).HeaderText = uxReportGrid.Columns.FindByUniqueName(ACTUAL_VALUE).HeaderText.ToCurrencySymbol();
        }
        protected DataTable GetDQReasonDataDetail()
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
            parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
            parameters.Add(new FilterParameter("@AlertDate", AlertDate, DbType.DateTime));
            parameters.Add(new FilterParameter("@AssignmentID", AssignmentID, DbType.String));
            string spaName = "spa_RM_MCF_GetDQReasons_Detail";
            parameters.AddLanguageID();
            return WebServices.RiskServices.GetReports(spaName, parameters);
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

                        DataTable table = GetDQReasonDataDetail();
                        grid.DataSource = table;
                    }
                    break;
                case DataBindAction.BindTitleInfo:
                    {
                        lblAssignmentName.Text = AssignmentName;
                        Title = Group ? GetLocalResourceObject("rm_mcf_Grouped_Parameter").ToString() : GetLocalResourceObject("rm_MCF_Match_Every_Parameter").ToString();
                    }
                    break;

            }
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

