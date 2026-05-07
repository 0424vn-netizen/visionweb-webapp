using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;
using Telerik.Web.UI;
using BuGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;

[PagePermission("RskParams,MSRskParams")]
public partial class rm_MCF_Parameters : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        LoadParameters
    }
    enum PostBackAction
    {
        Exporting
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RiskSessionManager.IsUsingMarketData)
            uxNoteMPS.Text = GetLocalResourceObject("rm_Parameters_aspx_cs_String1").ToString();
        else
            uxNoteMPS.Text = GetLocalResourceObject("rm_Parameters_aspx_cs_String2").ToString();
        //46652 - AW - Multi-Currency Transaction Display
        Literal2.Text = Literal2.Text.ToCurrencySymbol();
    }

    protected override void PageInitialize()
    {
        base.PageInitialize();
        this.IsSecureCSRF = true;
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Exporting:
                //ProcessSave();
                (sender as ExportConfig).TableSource = GetAssignmentParameters(-1);
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadParameters:
                DataTable table = GetAssignmentParameters(-1);
                (sender as UserControls_rm_MCF_Parameter).DataSource = table;
                break;
        }
    }

    private static DataTable GetOriginalAssignmentParameters(int ActivityStatus)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ActivityStatus", ActivityStatus, DbType.Int32));
        if (RiskSessionManager.IsUsingMarketData)
            return WebServices.RiskServices.GetReports("spa_RM_MCF_GetParametersList_MarketData", parameters);
        parameters.AddLanguageID();
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetParametersList", parameters);
    }

    public DataTable GetAssignmentParameters(int activityStatus)
    {
        const string PARAM_GROUPNAMEDUMMY = "ParameterGroupNameDummy";
        const string THRES_MIN = "ThresholdMin";
        const string THRES_MAX = "ThresholdMax";
        const string IND_MIN = "IndicatorMin";
        const string IND_MAX = "IndicatorMax";
        const string PARAM_VALUE_EXP = "ParameterValueExport";
        const string PARAM_VALUE_EXP_CSV = "ParameterValueExportCSV";
        const string PARAM_THRES_EXP = "ParameterThresholdExport";
        const string PARAM_ROMNUM = "ParameterRowNumber";
        const string PARAM_ID = "ParameterID";
        const string PARAM_KEY = "ParameterKey";
        const string PARAM_NAME = "ParameterName";
        const string PARAM_VALUE = "ParameterValue";
        const string PARAM_FILTERTYPE = "ParameterFilter";
        const string PARAM_DATATYPE = "ParameterDataType";
        const string PARAM_THRES = "ParameterThreshold";
        const string PARAM_DESC = "ParameterDescription";
        const string PARAM_PREC = "ParameterPrecision";
        const string PARAM_GROUPNAME = "ParameterGroupName";
        const string PARAM_CODE = "ParameterCode";
        const string ISASSIGNED = "IsAssigned";
        const string ISSELECTED = "IsSelected";
        const string ISACTIVE = "IsActive";
        const string ACT_STATUS = "ActivityStatus";
        const string ISNEW = "IsNew";
        const string USEDBY_ASS = "UsedByAssignments";
        const string USEDBY_RC = "UsedByRiskScores";
        const string ISINDICATORNEGATIVE = "IndicatorNegative";
        const string ISTHRESHOLDNEGATIVE = "ThresholdNegative";
        const string THRESHOLDTYPE = "ThresholdType";
        const string PARAMETER_THRESHOLD_TYPE = "ParameterThresholdType";
        const string PARAMETER_THRESHOLD_HIGH = "ParameterThresholdHigh";
        const string PARAM_VALUE_HIGH = "ParameterValueHigh";
        const string REALERT_PARAM_VALUE = "ReAlertParameterIndicator";
        const string REALERT_PARAM_THRES = "ReAlertParameterThreshold";
        const string THRES_MIN_NRT = "ReAlertParameterThresholdMin";
        const string THRES_MAX_NRT = "ReAlertParameterThresholdMax";
        const string IND_MIN_NRT = "ReAlertParameterIndicatorMin";
        const string IND_MAX_NRT = "ReAlertParameterIndicatorMax";
        const string PARAM_VALUE_EXP_NRT = "ParameterValueExportNRT";
        const string PARAM_VALUE_EXP_CSV_NRT = "ParameterValueExportCSVNRT";
        const string PARAM_THRES_EXP_NRT = "ParameterThresholdExportNRT";
        const string PARAM_VALUE_IS_EDIT = "IsEditReAlertParameterIndicator";
        const string PARAM_THRES_IS_EDIT = "IsEditReAlertParameterThreshold";
        const string PARAMETER_THRESHOLD_HIGH_NRT = "ReAlertParameterThresholdHigh";
        const string PARAMETER_INDICATOR_HIGH_NRT = "ReAlertParameterIndicatorHigh";
        //const string THRES_PREC = "ThresholdPrecision";

        Type stringType = typeof(string);
        Type intType = System.Type.GetType("System.Int32");
        Type booleanType = System.Type.GetType("System.Boolean");
        Type DBNull = Type.GetType("System.DBNull");
        Type doubleType = System.Type.GetType("System.Double");
        DataTable newTable = new DataTable("tblParameter");


        newTable.Columns.Add(PARAM_ROMNUM, stringType);
        newTable.Columns.Add(PARAM_ID, stringType);
        newTable.Columns.Add(PARAM_KEY, stringType);
        newTable.Columns.Add(PARAM_NAME, stringType);
        newTable.Columns.Add(PARAM_VALUE, stringType);
        newTable.Columns.Add(PARAM_FILTERTYPE, intType);
        newTable.Columns.Add(PARAM_DATATYPE, stringType);
        newTable.Columns.Add(PARAM_THRES, stringType);
        newTable.Columns.Add(PARAM_PREC, intType);
        newTable.Columns.Add(PARAM_GROUPNAME, stringType);
        newTable.Columns.Add(PARAM_CODE, stringType);
        newTable.Columns.Add(ISASSIGNED, booleanType);
        newTable.Columns.Add(PARAM_GROUPNAMEDUMMY, stringType);
        newTable.Columns.Add(THRES_MIN, intType);
        newTable.Columns.Add(THRES_MAX, intType);
        newTable.Columns.Add(IND_MIN, doubleType);
        newTable.Columns.Add(IND_MAX, intType);
        newTable.Columns.Add(PARAM_DESC, stringType);
        newTable.Columns.Add(ISSELECTED, stringType);
        newTable.Columns.Add(ISACTIVE, stringType);
        newTable.Columns.Add(ACT_STATUS, intType);
        newTable.Columns.Add(ISNEW, booleanType);
        newTable.Columns.Add(USEDBY_ASS, stringType);
        newTable.Columns.Add(USEDBY_RC, booleanType);
        //newTable.Columns.Add(THRES_PREC, intType);
        newTable.Columns.Add(PARAM_VALUE_EXP, stringType);
        newTable.Columns.Add(PARAM_VALUE_EXP_CSV, stringType);
        newTable.Columns.Add(PARAM_THRES_EXP, stringType);
        newTable.Columns.Add(ISINDICATORNEGATIVE, booleanType);
        newTable.Columns.Add(ISTHRESHOLDNEGATIVE, booleanType);
        newTable.Columns.Add(THRESHOLDTYPE, stringType);
        newTable.Columns.Add(PARAMETER_THRESHOLD_TYPE, stringType);
        newTable.Columns.Add(PARAMETER_THRESHOLD_HIGH, stringType);
        newTable.Columns.Add(PARAM_VALUE_HIGH, stringType);
        newTable.Columns.Add(REALERT_PARAM_THRES, stringType);
        newTable.Columns.Add(REALERT_PARAM_VALUE, stringType);
        newTable.Columns.Add(THRES_MIN_NRT, intType);
        newTable.Columns.Add(THRES_MAX_NRT, intType);
        newTable.Columns.Add(IND_MIN_NRT, doubleType);
        newTable.Columns.Add(IND_MAX_NRT, intType);
        newTable.Columns.Add(PARAM_VALUE_EXP_NRT, stringType);
        newTable.Columns.Add(PARAM_VALUE_EXP_CSV_NRT, stringType);
        newTable.Columns.Add(PARAM_THRES_EXP_NRT, stringType);
        newTable.Columns.Add(PARAM_VALUE_IS_EDIT, booleanType);
        newTable.Columns.Add(PARAM_THRES_IS_EDIT, booleanType);
        newTable.Columns.Add(PARAMETER_THRESHOLD_HIGH_NRT, stringType);
        newTable.Columns.Add(PARAMETER_INDICATOR_HIGH_NRT, stringType);

        DataTable oldTable = GetOriginalAssignmentParameters(activityStatus);

        if (oldTable.Rows.Count > 0)
        {
            int rowNumber = 1;
            foreach (DataRow row in oldTable.Rows)
            {
                DataRow newRow = newTable.NewRow();
                string parameterThresholdExport = string.Empty;

                newRow[PARAM_DESC] = row["Description"];
                newRow[IND_MIN] = row[IND_MIN];
                newRow[IND_MAX] = row["IndicatorMax"];
                newRow[THRES_MIN] = row[THRES_MIN];
                newRow[THRES_MAX] = row["ThresholdMax"];
                newRow[PARAM_ROMNUM] = rowNumber;
                newRow[PARAM_ID] = row[PARAM_ID];
                newRow[PARAM_KEY] = row[PARAM_KEY];
                newRow[PARAM_NAME] = row[PARAM_NAME];
                newRow[PARAM_VALUE] = row[PARAM_VALUE];
                newRow[PARAM_FILTERTYPE] = row["ParameterFilter"];
                newRow[PARAM_DATATYPE] = row[PARAM_DATATYPE];
                newRow[PARAM_THRES] = (row[PARAM_THRES].GetType() == DBNull || row[PARAM_THRES].ToString() != string.Empty
                        || row[THRES_MAX].GetType() == DBNull || decimal.Parse(row[THRES_MAX].ToString()) == 0 ?
                        row[PARAM_THRES] : "0dds0");
                newRow[USEDBY_ASS] = row[USEDBY_ASS];
                newRow[USEDBY_RC] = row[USEDBY_RC];
                //newRow[THRES_PREC] = row[THRES_PREC];
                newRow[PARAM_PREC] = (row[PARAM_PREC].GetType() == DBNull ? 0 : row[PARAM_PREC]);
                newRow[ISASSIGNED] = row[ISASSIGNED];
                newRow[PARAM_GROUPNAME] = row["Group"];
                newRow[ISACTIVE] = row[ISACTIVE];
                newRow[ISSELECTED] = row[ISSELECTED];
                newRow[ACT_STATUS] = row[ACT_STATUS];
                newRow[ISNEW] = row[ISNEW];
                newRow[ISINDICATORNEGATIVE] = row[ISINDICATORNEGATIVE];
                newRow[ISTHRESHOLDNEGATIVE] = row[ISTHRESHOLDNEGATIVE];
                newRow[THRESHOLDTYPE] = row[THRESHOLDTYPE];
                newRow[PARAMETER_THRESHOLD_TYPE] = row[PARAMETER_THRESHOLD_TYPE];
                newRow[PARAMETER_THRESHOLD_HIGH] = row[PARAMETER_THRESHOLD_HIGH];
                newRow[PARAM_VALUE_HIGH] = row[PARAM_VALUE_HIGH];
                newRow[REALERT_PARAM_VALUE] = row[REALERT_PARAM_VALUE];
                newRow[REALERT_PARAM_THRES] = (row[REALERT_PARAM_THRES].GetType() == DBNull || row[REALERT_PARAM_THRES].ToString() != string.Empty
                      || row[THRES_MAX].GetType() == DBNull || decimal.Parse(row[THRES_MAX].ToString()) == 0 ?
                      row[REALERT_PARAM_THRES] : "0dds0");
                newRow[THRES_MIN_NRT] = row[THRES_MIN_NRT];
                newRow[THRES_MAX_NRT] = row[THRES_MAX_NRT];
                newRow[IND_MIN_NRT] = row[IND_MIN_NRT];
                newRow[IND_MAX_NRT] = row[IND_MAX_NRT];
                newRow[PARAM_VALUE_IS_EDIT] = row[PARAM_VALUE_IS_EDIT];
                newRow[PARAM_THRES_IS_EDIT] = row[PARAM_THRES_IS_EDIT];
                newRow[PARAMETER_THRESHOLD_HIGH_NRT] = row[PARAMETER_THRESHOLD_HIGH_NRT];
                newRow[PARAMETER_INDICATOR_HIGH_NRT] = row[PARAMETER_INDICATOR_HIGH_NRT];
                //indicator for export

                var Indicator_Value = row[PARAM_VALUE].ToString();
                var Indicator_Value_High = row[PARAM_VALUE_HIGH];
                var Indicator_Type = row[PARAM_DATATYPE].ToString();

                if (!Indicator_Value.IsNullOrEmpty() && !Indicator_Value_High.ToString().IsNullOrEmpty())
                {
                    string indHigh = string.Empty;
                    string valueExp = string.Empty;
                    string valueExpCsv = string.Empty;
                    if (BuGeneralFuncsLib.CheckExistsInSplitToArray(WebSiteSettings.ParametersAllowDecimal, ',', newRow[PARAM_KEY].ToString()))
                    {
                        indHigh = Convert.ToDecimal(Indicator_Value_High).ToString();
                        valueExp = GeneralFuncsLib.FormatLowHighThreshold(Indicator_Value, indHigh, Indicator_Type, false, 2);
                        valueExpCsv = GeneralFuncsLib.FormatLowHighThreshold(Indicator_Value, indHigh, Indicator_Type, false, 2);
                    }
                    else
                    {
                        indHigh = Convert.ToInt32(Indicator_Value_High).ToString();
                        valueExp = GeneralFuncsLib.FormatLowHighThreshold(Indicator_Value, indHigh, Indicator_Type);
                        valueExpCsv = GeneralFuncsLib.FormatLowHighThreshold(Indicator_Value, indHigh, Indicator_Type);

                    }

                    newRow[PARAM_VALUE_EXP] = valueExp;
                    newRow[PARAM_VALUE_EXP_CSV] = valueExpCsv;
                }
                else
                {
                    newRow[PARAM_VALUE_EXP] = GeneralFuncsLib.FormatParameterExport(Indicator_Type, Indicator_Value, int.Parse(newRow[PARAM_PREC].ToString()));

                    newRow[PARAM_VALUE_EXP_CSV] = GeneralFuncsLib.FormatParameterExport(Indicator_Type, Indicator_Value, int.Parse(newRow[PARAM_PREC].ToString()));
                }
                //threshold for export

                var Threshold_Type = row[THRESHOLDTYPE].ToString();
                var Threshold_Value = row[PARAM_THRES].ToString();

                var ThresholdLow_Value = row[PARAM_THRES].ToString();
                var ThresholdHigh_Value = row[PARAMETER_THRESHOLD_HIGH].ToString();

                string ParameterThresholdType = row[PARAMETER_THRESHOLD_TYPE].ToString();

                switch (ParameterThresholdType)
                {
                    case "LowHigh":
                        newRow[PARAM_THRES_EXP] = GeneralFuncsLib.FormatLowHighThreshold(ThresholdLow_Value, ThresholdHigh_Value, Threshold_Type);
                        break;
                    default:
                        newRow[PARAM_THRES_EXP] = GeneralFuncsLib.FormatParameterDataType(Threshold_Type, Threshold_Value);
                        break;
                }

                //ReAlert
                //indicator for export

                var Indicator_Value_NRT = row[REALERT_PARAM_VALUE].ToString();
                var Indicator_Value_High_NRT = row[PARAMETER_INDICATOR_HIGH_NRT];
                var Indicator_Type_NRT = row[PARAM_DATATYPE].ToString();
                var IsEditIndicatorNRT = row[PARAM_VALUE_IS_EDIT].ToBoolean();
                var IsEditThresholdNRT = row[PARAM_THRES_IS_EDIT].ToBoolean();

                if (!Indicator_Value_NRT.IsNullOrEmpty() && !Indicator_Value_High_NRT.ToString().IsNullOrEmpty())
                {
                    var IndHigh_NRT = Convert.ToInt32(Indicator_Value_High_NRT);
                    newRow[PARAM_VALUE_EXP_NRT] = GeneralFuncsLib.FormatLowHighThreshold(Indicator_Value_NRT, IndHigh_NRT.ToString(), Indicator_Type_NRT);
                    newRow[PARAM_VALUE_EXP_CSV_NRT] = GeneralFuncsLib.FormatLowHighThreshold(Indicator_Value_NRT, IndHigh_NRT.ToString(), Indicator_Type_NRT);

                }
                else
                {
                    newRow[PARAM_VALUE_EXP_NRT] = GeneralFuncsLib.FormatParameterExport(Indicator_Type_NRT, Indicator_Value_NRT, int.Parse(newRow[PARAM_PREC].ToString()), IsEditIndicatorNRT);

                    newRow[PARAM_VALUE_EXP_CSV_NRT] = GeneralFuncsLib.FormatParameterExport(Indicator_Type_NRT, Indicator_Value_NRT, int.Parse(newRow[PARAM_PREC].ToString()), IsEditIndicatorNRT);
                }
                //threshold for export

                var Threshold_Type_NRT = row[THRESHOLDTYPE].ToString();
                var Threshold_Value_NRT = row[REALERT_PARAM_THRES].ToString();

                var ThresholdLow_Value_NRT = row[REALERT_PARAM_THRES].ToString();
                var ThresholdHigh_Value_NRT = row[PARAMETER_THRESHOLD_HIGH_NRT].ToString();

                string ParameterThresholdType_NRT = row[PARAMETER_THRESHOLD_TYPE].ToString();

                switch (ParameterThresholdType_NRT)
                {
                    case "LowHigh":
                        newRow[PARAM_THRES_EXP_NRT] = GeneralFuncsLib.FormatLowHighThreshold(ThresholdLow_Value_NRT, ThresholdHigh_Value_NRT, Threshold_Type_NRT, IsEditThresholdNRT);
                        break;
                    default:
                        newRow[PARAM_THRES_EXP_NRT] = GeneralFuncsLib.FormatParameterDataType(Threshold_Type_NRT, Threshold_Value_NRT, 0, false, IsEditThresholdNRT);
                        break;
                }

                int groupID = 0;

                Int32.TryParse(row["GroupID"].ToString(), out groupID);

                switch (groupID)
                {
                    case 9://"Deposits":
                        newRow[PARAM_GROUPNAMEDUMMY] = "I";
                        break;
                    case 8://"Auth":
                        newRow[PARAM_GROUPNAMEDUMMY] = "H";
                        break;
                    case 6://"Attrition":
                        newRow[PARAM_GROUPNAMEDUMMY] = "F";
                        break;
                    case 5://"Chargeback/Retrievals":
                        newRow[PARAM_GROUPNAMEDUMMY] = "E";
                        break;
                    case 4://"Contract":
                        newRow[PARAM_GROUPNAMEDUMMY] = "D";
                        break;
                    case 2://"Duplicates":
                        newRow[PARAM_GROUPNAMEDUMMY] = "B";
                        break;
                    case 1://"Volume/Batch/Ticket":
                        newRow[PARAM_GROUPNAMEDUMMY] = "A";
                        break;
                    case 3://"Credits":
                        newRow[PARAM_GROUPNAMEDUMMY] = "C";
                        break;
                    case 7://"ACH Rejects":
                        newRow[PARAM_GROUPNAMEDUMMY] = "G";
                        break;
                    case 10://"BIN":
                        newRow[PARAM_GROUPNAMEDUMMY] = "J";
                        break;
                    case 11://"Merchant Filter"
                        newRow[PARAM_GROUPNAMEDUMMY] = "K";
                        break;
                    default: break;
                }
                newTable.Rows.Add(newRow);
                rowNumber++;
            }
        }
        return newTable;
    }

    protected void uxParam_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.LoadParameters, source);
    }

    protected void uxParam_NeedExportConfig(object sender, ExportConfig exportConfig)
    {
        OnPostBackActions(PostBackAction.Exporting, exportConfig);
    }

    /// <summary>
    /// Save changed parameters to database.
    /// </summary>
    private void ProcessSave()
    {
        List<RiskParameter> paramList = this.uxParam.ParameterList;
        if (paramList != null)
        {
            var paramStr = paramList.ToListXmlString(true);

            var filterParameters = new FilterParameterCollection();
            filterParameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            filterParameters.Add("ParameterList", paramStr, DbType.Xml);

            WebServices.RiskServices.GetReports("spa_RM_MCF_UpdateParamaters", filterParameters);
        }
    }

    protected void uxParam_ProcessSave(object sender, EventArgs e)
    {
        ProcessSave();
        this.uxParam.Rebind();
    }

    [WebMethod(EnableSession = true)]
    public static string[] GetParamInfo(string parameterKey, bool isCheckAll)
    {
        DataTable paramInfoData = GetOriginalAssignmentParameters(-1);
        if (isCheckAll)
        {
            var data = paramInfoData.AsEnumerable().Where(x => !string.IsNullOrEmpty(x.Field<string>("UsedByAssignments")) || x.Field<bool>("UsedByRiskScores")).Select(x => x.Field<string>("ParameterKey")).ToArray();
            string paramList = string.Join(",", data);
            return new string[] { paramList };

        }
        else
        {
            var item = paramInfoData.AsEnumerable().Where(x => x.Field<string>("ParameterKey") == parameterKey).ToArray();
            string assignmentList = item != null && item.Count() > 0 ? item[0]["UsedByAssignments"].ToString() : string.Empty;
            string riskScoreList = item != null && item.Count() > 0 ? item[0]["UsedByRiskScores"].ToString().ToLower() : string.Empty;
            return new string[] { assignmentList, riskScoreList };
        }
    }
}
