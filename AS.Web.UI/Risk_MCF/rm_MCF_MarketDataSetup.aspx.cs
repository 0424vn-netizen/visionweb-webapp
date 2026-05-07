using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Common.DBManager;
using Telerik.Web.UI;
using AS.Controls.Exporter;
using AS.Common;
using System.Collections.Generic;
using AS.Controls.Pages;

[PagePermission("RskMarketData,MSRskMarketData")]
public partial class rm_MCF_MarketDataSetup : NonReportPage
{
    #region Enums
    enum DataBindAction
    {
        LoadParameters,
        LoadMarketDataType,
        CheckMatchAll
    }
    enum PostBackAction
    {
        SelectMarketData,
        Exporting,
        SaveFiltering
    }
    #endregion

    #region Properties
    private string _AssignmentListofMarketData = string.Empty;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        if (!IsPostBack)
        {

            //BinDataCombobox();
            OnDataBindControls(DataBindAction.LoadMarketDataType);
            VisibleControls();
        }
        uxParam.MarketData = uxComboMarketDataType.SelectedValue;
    }

    private void getAssignmentListByMarketData()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@MarketDataElement", uxComboMarketDataType.SelectedValue, DbType.String));
        DataTable td = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAssignmentsListByMarketData", parameters);
        _AssignmentListofMarketData = td.Rows[0]["Assignments"].ToString();
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.SelectMarketData:
                VisibleControls();
                uxParam.Rebind();
                getAssignmentListByMarketData();
                break;
            case PostBackAction.Exporting:
                uxParam.MarketData = uxComboMarketDataType.SelectedValue;
                ProcessSave();
                (sender as ExportConfig).TableSource = GetAssignmentParameters();
                (sender as ExportConfig).FileName = GetLocalResourceObject("rm_MarketDataSetup_aspx_cs_FileName").ToString();
                (sender as ExportConfig).ReportHeader = GetLocalResourceObject("rm_MarketDataSetup_aspx_cs_ReportHeader").ToString();
                break;
            case PostBackAction.SaveFiltering:
                SaveMatchAllAndRiskScore();
                
                if (uxChbRiskScore.Checked)
                {
                    txtRiskScoreTo.Enabled = txtRiskScoreFrom.Enabled = true;
                }
                else
                {
                    txtRiskScoreTo.Enabled = txtRiskScoreFrom.Enabled = false;
                }
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadMarketDataType:
                BinDataCombobox();
                break;
            case DataBindAction.CheckMatchAll:
                loadDataMatchAll();
                break;
            case DataBindAction.LoadParameters:
                DataTable table = GetAssignmentParameters();
                (sender as UserControls_rm_MCF_Parameter_MarketData).DataSource = table;
                break;
        }
    }

    private void VisibleControls()
    {
        string marketData = uxComboMarketDataType.SelectedValue;
        if (string.IsNullOrEmpty(marketData))
        {
            FirstFilter.Visible = false;
            uxParam.Visible = false;
            uxFooter.Visible = false;
        }
        else
        {
            FirstFilter.Visible = true;
            uxParam.Visible = true;
            uxFooter.Visible = true;
            uxParam.MarketData = marketData;
            //loadDataMatchAll();
            OnDataBindControls(DataBindAction.CheckMatchAll);
        }
    }

    private void loadDataMatchAll()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        string marketDataType = uxComboMarketDataType.SelectedValue;
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add("@MarketDataType", marketDataType, DbType.String);

        DataTable tb = WebServices.RiskServices.GetReports("spa_RM_MCF_GetDefaultRiskScore", parameters);
        if (tb != null && tb.Rows.Count > 0)
        {
            bool isMatchAllParameter = Convert.ToBoolean(tb.Rows[0]["IsMatchAll"].ToString());
            if (isMatchAllParameter)
            {
                radioYes.Checked = true;
                radioNo.Checked = false;
            }
            else
            {
                radioYes.Checked = false;
                radioNo.Checked = true;
            }

            txtRiskScoreFrom.Text = VeraCodeSolution.DoVeraCode(tb.Rows[0]["From"].ToString());
            txtRiskScoreTo.Text = VeraCodeSolution.DoVeraCode(tb.Rows[0]["To"].ToString());
            bool chkRS = Convert.ToBoolean(tb.Rows[0]["Status"].ToString());
            uxChbRiskScore.Checked = chkRS;
            if (chkRS)
            {
                txtRiskScoreTo.Enabled = txtRiskScoreFrom.Enabled = true;
            }
            else
            {
                txtRiskScoreTo.Enabled = txtRiskScoreFrom.Enabled = false;
            }
        }
    }

    protected void uxComboMarketDataType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        OnPostBackActions(PostBackAction.SelectMarketData);
    }

    private void BinDataCombobox()
    {
        uxComboMarketDataType.DataValueField = "MarketDataType";
        uxComboMarketDataType.DataTextField = "Description";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        uxComboMarketDataType.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_GetMarketDataList", parameters);
        uxComboMarketDataType.DataBind();
        uxComboMarketDataType.Items.Insert(0, new RadComboBoxItem("Select a Market Data to Update"));
    }

    private DataTable GetOriginalAssignmentParameters()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        string marketDataType = uxComboMarketDataType.SelectedValue;
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add("@MarketDataType", marketDataType, DbType.String);
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetParametersListbyMarketData", parameters);
    }

    private string GetDouble4Precision(string doubleStr)
    {
        if (string.IsNullOrEmpty(doubleStr))
            return doubleStr;
        if (doubleStr.Contains("."))
        {
            if (!doubleStr.EndsWith("0000"))
                return double.Parse(doubleStr).ToString("#,#0.0000");
        }

        return double.Parse(doubleStr).ToString("#,#0");
    }

    public DataTable GetAssignmentParameters()
    {
        const string PARAM_GROUPNAMEDUMMY = "ParameterGroupNameDummy";
        const string THRES_MIN = "ThresholdMin";
        const string THRES_MAX = "ThresholdMax";
        const string IND_MIN = "IndicatorMin";
        const string IND_MAX = "IndicatorMax";
        const string PARAM_VALUE_EXP = "ParameterValueExport";
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
        const string FILTERTYPE_MODAL = "FilterTypeModal";
        const string PARAM_VALUE_HIGH = "ParameterValueHigh";

        //const string THRES_PREC = "ThresholdPrecision";

        Type stringType = typeof(string);
        Type intType = System.Type.GetType("System.Int32");
        Type booleanType = System.Type.GetType("System.Boolean");
        Type DBNull = Type.GetType("System.DBNull");

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
        newTable.Columns.Add(IND_MIN, intType);
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
        newTable.Columns.Add(PARAM_THRES_EXP, stringType);
        newTable.Columns.Add(ISINDICATORNEGATIVE, booleanType);
        newTable.Columns.Add(ISTHRESHOLDNEGATIVE, booleanType);
        newTable.Columns.Add(THRESHOLDTYPE, stringType);
        newTable.Columns.Add(PARAMETER_THRESHOLD_TYPE, stringType);
        newTable.Columns.Add(PARAMETER_THRESHOLD_HIGH, stringType);
        newTable.Columns.Add(FILTERTYPE_MODAL, stringType);
        newTable.Columns.Add(PARAM_VALUE_HIGH, stringType);

        DataTable oldTable = GetOriginalAssignmentParameters();

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
                newRow[FILTERTYPE_MODAL] = row[FILTERTYPE_MODAL];
                newRow[PARAM_VALUE_HIGH] = row[PARAM_VALUE_HIGH];
                //indicator for export

                var Indicator_Value = row[PARAM_VALUE].ToString();
                var Indicator_Value_High = row[PARAM_VALUE_HIGH];
                var Indicator_Type = row[PARAM_DATATYPE].ToString();

                if (!Indicator_Value.IsNullOrEmpty() && !Indicator_Value_High.ToString().IsNullOrEmpty())
                {
                    var IndHigh = Convert.ToInt32(Indicator_Value_High);
                    newRow[PARAM_VALUE_EXP] = GeneralFuncsLib.FormatLowHighThreshold(Indicator_Value, IndHigh.ToString(), Indicator_Type);
                }
                else
                {
                    newRow[PARAM_VALUE_EXP] = GeneralFuncsLib.FormatParameterDataType(Indicator_Type, Indicator_Value);
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


                int groupID = 0;

                Int32.TryParse(row["GroupID"].ToString(), out groupID);

                switch (groupID)
                {
                    case 1://"Volume/Batch/Ticket":
                        newRow[PARAM_GROUPNAMEDUMMY] = "A";
                        break;
                    case 2://"Duplicates":
                        newRow[PARAM_GROUPNAMEDUMMY] = "B";
                        break;
                    case 3://"Credits":
                        newRow[PARAM_GROUPNAMEDUMMY] = "C";
                        break;
                    case 4://"Contract":
                        newRow[PARAM_GROUPNAMEDUMMY] = "D";
                        break;
                    case 5://"Chargebacks/Retrievals":
                        newRow[PARAM_GROUPNAMEDUMMY] = "E";
                        break;
                    case 6://"Attrition":
                        newRow[PARAM_GROUPNAMEDUMMY] = "F";
                        break;
                    case 7://"ACH Rejects":
                        newRow[PARAM_GROUPNAMEDUMMY] = "G";
                        break;
                    case 8://"Auth":
                        newRow[PARAM_GROUPNAMEDUMMY] = "H";
                        break;
                    case 9://"Deposits":
                        newRow[PARAM_GROUPNAMEDUMMY] = "I";
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
            var paramStr = paramList.ToListXmlString();
            string marketDataType = uxComboMarketDataType.SelectedValue;

            var filterParameters = new FilterParameterCollection();
            filterParameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            filterParameters.Add("MarketDataType", marketDataType, DbType.String);
            filterParameters.Add("ParameterList", paramStr, DbType.Xml);

            WebServices.RiskServices.GetReports("spa_RM_MCF_SaveParameterForMarketData", filterParameters);
        }
    }

    protected void uxParam_ProcessSave(object sender, EventArgs e)
    {
        ProcessSave();
    }

    private void SaveMatchAllAndRiskScore()
    {
        string marketDataType = uxComboMarketDataType.SelectedValue;
        var filterParameters = new FilterParameterCollection();
        filterParameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        filterParameters.Add("MarketDataType", marketDataType, DbType.String);
        filterParameters.Add("IsCheckRiskScore", uxChbRiskScore.Checked, DbType.Boolean);
        filterParameters.Add("RiskScoreFrom", txtRiskScoreFrom.Text.Trim(), DbType.String);
        filterParameters.Add("RiskScoreTo", txtRiskScoreTo.Text.Trim(), DbType.String);
        if (radioYes.Checked)
            filterParameters.Add("IsMatchAll", true, DbType.Boolean);
        else
            filterParameters.Add("IsMatchAll", false, DbType.Boolean);
        FilterParameterCollection paramOuts = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveMatchAllAndRiskScore", filterParameters, out paramOuts);
    }

    protected void uxBntSaveFirtFilter_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SaveFiltering);
    }

    protected void uxBntSave_Click(object sender, EventArgs e)
    {
        getAssignmentListByMarketData();
        AjaxAddResponseScript("btnSaveClick('" + _AssignmentListofMarketData + "')");
    }
}
