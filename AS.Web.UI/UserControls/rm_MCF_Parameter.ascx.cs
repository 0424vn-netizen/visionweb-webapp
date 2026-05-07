using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using System.Collections.Generic;
using Telerik.Web.UI;
using AS.Controls.UserControls;
using AS.Controls.Exporter;
using System.Drawing;
using AS.Common;
using System.Text;
using BuGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;

public partial class UserControls_rm_MCF_Parameter : GlobalUserControl
{
    #region constants

    private const string SP_GETDEFAULTRISKSCORE = "spa_RM_MCF_GetDefaultRiskScore";

    private const string PARAM_CHECKBOX = "ParameterCheckBox";
    private const string PARAM_NAME = "ParameterName";
    private const string IND_MAXVALUE = "IndicatorMax";
    private const string PARAM_VALUE = "ParameterValue";

    private const string PARAM_THRESHOLD = "ParameterThreshold";
    private const string THRESHOLD_MAXVAL = "ThresholdMaxValue";
    private const string THRESHOLD_MINVAL = "ThresholdMinValue";
    private const string MAX_VAL = "MaxValue";
    private const string MIN_VAL = "MinValue";
    private const string ERR_CONTROLID = "ErrControlID";
    private const string PRECISION = "Precision";
    //private const string THRESHOLD_PREC = "ThresholdPrecision";
    private const string NBSP = "&nbsp;";

    private const string PARAM_KEY = "ParameterKey";
    private const string PARAM_PRECISION = "ParameterPrecision";
    private const string IND_MINVALUE = "IndicatorMin";
    private const string ACTIVITY_STATUS = "ActivityStatus";
    private const string IS_SELECTED = "IsSelected";
    private const string GROUP_NAME = "GroupNameDummy";
    private const string PARAM_THRESHOLDHIDE = "ParameterThresholdHide";
    private const string USEDBY_ASS = "UsedByAssignments";
    private const string USEDBY_RISKSCORE = "UsedByRiskScores";
    private const string PARAM_CODE = "ParameterCode";
    private const string REALERT_PARAM_VALUE = "ReAlertParameterIndicator";
    private const string REALERT_PARAM_THRES = "ReAlertParameterThreshold";
    private const string THRES_MIN_MCF = "ReAlertParameterThresholdMin";
    private const string THRES_MAX_MCF = "ReAlertParameterThresholdMax";
    private const string IND_MIN_MCF = "ReAlertParameterIndicatorMin";
    private const string IND_MAX_MCF = "ReAlertParameterIndicatorMax";
    private const string PARAM_VALUE_EXP_MCF = "ParameterValueExportNRT";
    private const string PARAM_VALUE_EXP_CSV_MCF = "ParameterValueExportCSVNRT";
    private const string PARAM_THRES_EXP_MCF = "ParameterThresholdExportNRT";
    private const string PARAM_THRESHOLDHIDEMCF = "ParameterThresholdHideNRT";
    private const string PARAM_VALUE_IS_EDIT = "IsEditReAlertParameterIndicator";
    private const string PARAM_THRES_IS_EDIT = "IsEditReAlertParameterThreshold";
    #endregion

    #region Properties
    private string NA = string.Empty;
    private int _AssignmentID = 0;
    /// <summary>
    /// Gets the assignment ID.
    /// </summary>
    /// <value>The assignment ID.</value>
    public int AssignmentID
    {
        get
        {
            if (Request.Params["assignmentid"] != null)
                _AssignmentID = Convert.ToInt32(Request.Params["assignmentid"]);
            return _AssignmentID;

        }

    }
    /// <summary>
    /// Gets or sets a value indicating whether [temp disable duplicates].
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [temp disable duplicates]; otherwise, <c>false</c>.
    /// </value>
    public bool TempDisableDuplicates
    {
        get
        {
            return this.ViewState["TempDisableDuplicates"] != null ?
                (bool)this.ViewState["TempDisableDuplicates"] : false;
        }
        set
        {
            this.ViewState["TempDisableDuplicates"] = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is from assignment.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is from assignment; otherwise, <c>false</c>.
    /// </value>
    public bool IsFromAssignment
    {
        get
        {
            return this.ViewState["UxParam_IsFromAssignment"] != null ?
                (bool)this.ViewState["UxParam_IsFromAssignment"] : false;
        }
        set
        {
            this.ViewState["UxParam_IsFromAssignment"] = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [show row number column].
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [show row number column]; otherwise, <c>false</c>.
    /// </value>
    public bool ShowRowNumberColumn
    {
        get { return this.uxParameterList.Columns.FindByUniqueName("ParameterRowNumber").Visible; }
        set { this.uxParameterList.Columns.FindByUniqueName("ParameterRowNumber").Visible = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [show export].
    /// </summary>
    /// <value><c>true</c> if [show export]; otherwise, <c>false</c>.</value>
    public bool ShowExport
    {
        get { return this.uxExportTop.Visible; }
        set
        {
            this.uxExportTop.Visible = value;

        }
    }

    /// <summary>
    /// Get a changes parameter list
    /// Note: this function get a changed parameter list which is edited (select checkbox) by user on the UI
    /// </summary>
    public List<RiskParameter> ParameterChangeList
    {
        get
        {
            return this.Session[this.ClientID + "ParameterChangeList"] != null ?
                (List<RiskParameter>)this.Session[this.ClientID + "ParameterChangeList"] : null;
        }
        private set
        {
            this.Session[this.ClientID + "ParameterChangeList"] = value;
        }
    }

    /// <summary>
    /// Get all RiskParameter list.
    /// </summary>
    public List<RiskParameter> ParameterList
    {
        get
        {
            return this.Session[this.ClientID + "ParameterList"] != null ?
                this.Session[this.ClientID + "ParameterList"] as List<RiskParameter> : null;
        }
        private set
        {
            this.Session[this.ClientID + "ParameterList"] = value;
        }
    }

    /// <summary>
    /// Gets the selected parameters (checked parameters).
    /// </summary>
    /// <value>The selected parameters.</value>
    public List<RiskParameter> SelectedParameters
    {
        get
        {
            if (this.ParameterList != null)
            {
                var selParams = this.ParameterList.Where(p => p.ActivityStatus == 1);
                return selParams.ToList();
            }

            return null;
        }
    }

    /// <summary>
    /// Gets or sets the no record text.
    /// </summary>
    /// <value>The no record text.</value>
    public string NoRecordText
    {
        get
        {
            return this.uxParameterList.MasterTableView.NoMasterRecordsText;
        }
        set
        {
            this.uxParameterList.MasterTableView.NoMasterRecordsText = value;
        }
    }

    /// <summary>
    /// Data source of grid
    /// </summary>
    public object DataSource
    {
        get { return this.uxParameterList.DataSource; }
        set { this.uxParameterList.DataSource = value; }
    }
    /// <summary>
    /// Rebind all grid
    /// </summary>
    public void Rebind()
    {
        this.ParameterInfo.Clear();
        this.uxParameterList.Rebind();
    }
    /// <summary>
    /// Rebind mastertableview of grid
    /// </summary>
    public void RebindMasterTableView()
    {
        this.ParameterInfo.Clear();
        this.uxParameterList.MasterTableView.Rebind();
    }

    /// <summary>
    /// NeedDataSource event for UxParameter
    /// </summary>
    public event GridNeedDataSourceEventHandler NeedDataSource;
    /// <summary>
    /// ItemDataBound event for UxParameter
    /// </summary>
    public event GridItemEventHandler ItemDataBound;
    /// <summary>
    /// NeedExportConfig event for UxParameter
    /// </summary>
    public event NeedExportConfigHandler NeedExportConfig;
    /// <summary>
    /// Occurs when [process save].
    /// </summary>
    public event EventHandler ProcessSave;

    private List<string> _ParameterInfo = new List<string>();
    /// <summary>
    /// Contain the RiskParameter informations for generate to client browser
    /// </summary>
    private List<string> ParameterInfo
    {
        get
        {

            if (Session[this.ClientID + "_ParameterInfo"] == null)
                Session[this.ClientID + "_ParameterInfo"] = new List<string>();

            return (List<string>)Session[this.ClientID + "_ParameterInfo"];
        }
        set
        {
            Session[this.ClientID + "_ParameterInfo"] = value;
        }
    }

    private string _MarketData = string.Empty;

    public string MarketData
    {
        get { return _MarketData; }
        set { _MarketData = value; }
    }


    /// <summary>
    /// Indicate that this control is in sorting or not.
    /// </summary>
    private bool _IsSort = false;

    #endregion

    protected string setParamterType(object sender)
    {
        if (sender != null)
        {
            string result = sender.ToString().ToLower();
            if (result.Equals("days"))
                return GetLocalResourceObject("Risk_Parameter_ascx_cs_Days").ToString();
            return result;
        }
        return string.Empty;
    }

    private FilterParameterCollection GetDefaultRiskScoreParameters()
    {
        var filterParameters = new FilterParameterCollection();
        return filterParameters;
    }

    private FilterParameterCollection GetAssListUseRiskScoreParameters()
    {
        var filterParameters = new FilterParameterCollection();
        filterParameters
            .Add("ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32)
            .Add("UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString);

        return filterParameters;
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        OnProcessSave(e);
    }

    protected void PageLoad()
    {
        NA = GetLocalResourceObject("Literal2Resource2.Text").ToString();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        //visible columns
        this.uxParameterList.Columns.FindByUniqueName(PARAM_CODE).Visible = false;

        if (this.IsPostBack)
        {
            var list = this.GetAllParameterFromUI();
            //update all RiskParameter list
            this.ParameterList = list;
        }

    }

    public List<RiskParameter> GetAllParameterFromUI()
    {
        var dataItems = this.uxParameterList.MasterTableView.Items;
        var list = new List<RiskParameter>();
        foreach (GridDataItem dataItem in dataItems)
        {
            RiskParameter p = new RiskParameter();
            string key = dataItem[PARAM_KEY].Text.Trim();
            RiskParameter referer = this.ParameterList.GetRiskParameterByKey(key);
            var isEditIndicator = dataItem[PARAM_VALUE_IS_EDIT].Text.Trim().ToLower() == "yes" ? true : false;
            var isEditThreshold = dataItem[PARAM_THRES_IS_EDIT].Text.Trim().ToLower() == "yes" ? true : false;
            p.GetDataFromGridDataItemNRT(dataItem, referer, isEditIndicator, isEditThreshold);

            list.Add(p);
        }
        return list;
    }

    //mode: -1=all, 0=not selected 1=selected
    public string GetParameterKeysFromUI(int mode)
    {
        var dataItems = this.uxParameterList.MasterTableView.Items;
        StringBuilder sb = new StringBuilder();

        foreach (GridDataItem dataItem in dataItems)
        {
            HtmlInputCheckBox chkBox = dataItem[PARAM_CHECKBOX].FindControl("chkRowIndex") as HtmlInputCheckBox;
            if (mode == -1 || (mode == 0 && chkBox != null && !chkBox.Checked) || (mode == 1 && chkBox != null && chkBox.Checked))
            {
                sb.AppendFormat("{0},", dataItem[PARAM_KEY].Text.Trim());
            }
        }

        return sb.ToString().TrimEnd(',');
    }

    private List<RiskParameter> GetAllParameterFromTable(DataTable table)
    {
        List<RiskParameter> paramList = new List<RiskParameter>();
        for (int i = 0; i < table.Rows.Count; i++)
        {
            RiskParameter p = new RiskParameter();
            p.GetDataFromTableRowNRT(table.Rows[i]);
            paramList.Add(p);
        }
        return paramList;
    }

    private List<RiskParameter> GetAllParameterFromTable2(DataTable table)
    {
        List<RiskParameter> paramList = new List<RiskParameter>();
        for (int i = 0; i < table.Rows.Count; i++)
        {
            RiskParameter p = new RiskParameter();
            p.GetDataFromTableRow2(table.Rows[i]);
            paramList.Add(p);
        }
        return paramList;
    }

    protected void uxParameterList_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.OnNeedDataSource(e);

        if (e.RebindReason == GridRebindReason.PostBackEvent || e.RebindReason == GridRebindReason.ExplicitRebind)
            this._IsSort = true;

        DataTable table = null;

        if (this.DataSource != null)
        {
            this.ParameterInfo.Clear();

            table = (this.DataSource is DataTable ? this.DataSource as DataTable : (this.DataSource as DataView).Table);

            //if (this.ShowRiskScore && !this.IsFromAssignment)
            //{
            //    for (int i = 0; i < table.Rows.Count; i++)
            //        table.Rows[i][ACTIVITY_STATUS] = 0;
            //}

            //get all parameters for the first time and save into session
            if (!this._IsSort)
                this.ParameterList = GetAllParameterFromTable(table);

            //disable sorting if the grid has no records.
            this.uxParameterList.AllowSorting = true;
            if (table.Rows.Count == 0)
                this.uxParameterList.AllowSorting = false;

        }

        if (table == null || table.Rows.Count == 0)
            this.uxParameterList.Columns.FindByUniqueName(PARAM_CHECKBOX).HeaderText = string.Empty;
    }

    protected void uxParameterList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            var headerItem = e.Item as GridHeaderItem;
            ProcessHeaderItemStyle(headerItem);

        }
        else if (e.Item is GridDataItem)
        {
            var dataItem = e.Item as GridDataItem;
            ProcessDataItemStyle(dataItem);

            //================== process Indicator column==========================
            TextBox txtParameterValue = e.Item.FindControl("txtParameterValue") as TextBox;
            Label lblIndicatorType = e.Item.FindControl("IndicatorType") as Label;
            Label lblIndicatorMCFType = e.Item.FindControl("IndicatorMCFType") as Label;
            Literal lblType = e.Item.FindControl("lblParameterType") as Literal;
            RegularExpressionValidator RequiredFieldValidator1 = e.Item.FindControl("RequiredFieldValidator1") as RegularExpressionValidator;
            Label lblIndicatorDollar = e.Item.FindControl("lblIndicatorDollar") as Label;
            HtmlGenericControl spanErrMessage = e.Item.FindControl("spanErrMsg") as HtmlGenericControl;

            //=================== process Threshold column =======================
            TextBox txtThreshold = e.Item.FindControl("txtThreshold") as TextBox;
            DataRowView rowView = e.Item.DataItem as DataRowView;
            Label lblDollar = e.Item.FindControl("lblDollar") as Label;
            Label lblpercent = e.Item.FindControl("lblpercent") as Label;
            HtmlGenericControl spanErrMessageThs = e.Item.FindControl("spanErrMsgThs") as HtmlGenericControl;

            TextBox txtThresholdLow = e.Item.FindControl("txtThresholdLow") as TextBox;
            TextBox txtThresholdHigh = e.Item.FindControl("txtThresholdHigh") as TextBox;

            Label lblDollarThresholdLow = e.Item.FindControl("lblDollarThresholdLow") as Label;
            Label lblPercentThresholdLow = e.Item.FindControl("lblPercentThresholdLow") as Label;

            Label lblDollarThresholdHigh = e.Item.FindControl("lblDollarThresholdHigh") as Label;
            Label lblPercentThresholdHigh = e.Item.FindControl("lblPercentThresholdHigh") as Label;

            HtmlGenericControl spanErrMsgThresholdLow = e.Item.FindControl("spanErrMsgThresholdLow") as HtmlGenericControl;
            HtmlGenericControl spanErrMsgThresholdHigh = e.Item.FindControl("spanErrMsgThresholdHigh") as HtmlGenericControl;

            DataRowView row = e.Item.DataItem as DataRowView;
            var ParameterThresholdType = row["ParameterThresholdType"].ToString();
            var ParameterThreshold = row["ParameterThreshold"].ToString();
            var ParameterThresholdLow = row["ParameterThreshold"].ToString();
            var ParameterThresholdHigh = row["ParameterThresholdHigh"].ToString();

            TextBox txtFrom = e.Item.FindControl("txtFrom") as TextBox;
            TextBox txtTo = e.Item.FindControl("txtTo") as TextBox;
            if (row["ParameterValue"] != DBNull.Value && !row["ParameterValue"].ToString().IsNullOrEmpty()
                && row["ParameterValueHigh"] != DBNull.Value && !row["ParameterValueHigh"].ToString().IsNullOrEmpty())
            {
                var ParameterValue = row["ParameterValue"].ToString();
                var ParameterValueHigh = row["ParameterValueHigh"].ToString();
                HtmlGenericControl div = e.Item.FindControl("div1") as HtmlGenericControl;
                HtmlGenericControl uxMerchantfilter = e.Item.FindControl("uxMerchantFilter") as HtmlGenericControl;
                div.Visible = false;
                uxMerchantfilter.Visible = true;

                string paramKey = dataItem[PARAM_KEY].Text;
                int parameterPrecision = !string.IsNullOrEmpty(row[PARAM_PRECISION].ToString()) ? int.Parse(row[PARAM_PRECISION].ToString()) : 0;
                var isDecimal = BuGeneralFuncsLib.CheckExistsInSplitToArray(WebSiteSettings.ParametersAllowDecimal, ',', paramKey);

                txtFrom.Text = isDecimal ? BuGeneralFuncsLib.GetDoubleValue(ParameterValue, parameterPrecision, isDecimal) : VeraCodeSolution.DoVeraCode(Convert.ToInt32(GetParameterNumber(ParameterValue)).ToString());
                txtTo.Text = isDecimal ? BuGeneralFuncsLib.GetDoubleValue(ParameterValueHigh, parameterPrecision, isDecimal) : VeraCodeSolution.DoVeraCode(Convert.ToInt32(GetParameterNumber(ParameterValueHigh)).ToString());

                //add to check indicator max, min, precision, error controlID value
                if (dataItem[IND_MAXVALUE].Text.Replace(NBSP, string.Empty).Trim() != string.Empty)
                    txtFrom.Attributes[MAX_VAL] = txtTo.Attributes[MAX_VAL] = row[IND_MAXVALUE].ToString();
                txtFrom.Attributes[MIN_VAL] = txtTo.Attributes[MIN_VAL] = row[IND_MINVALUE].ToString();
                txtFrom.Attributes[PRECISION] = txtTo.Attributes[PRECISION] = row[PARAM_PRECISION].ToString();
                txtFrom.Attributes[ERR_CONTROLID] = txtTo.Attributes[ERR_CONTROLID] = spanErrMessage.ClientID;
            }
            bool isNA = false;
            if (ParameterThresholdType == "LowHigh")
            {
                if (ParameterThresholdLow.IsNullOrEmpty() && ParameterThresholdHigh.IsNullOrEmpty())
                {
                    isNA = true;
                }
            }
            else
            {
                if (ParameterThreshold.IsNullOrEmpty())
                {
                    isNA = true;
                }
            }


            if (isNA)
            {
                ((PlaceHolder)e.Item.FindControl("uxThresholdNormal")).Visible =
                         ((PlaceHolder)e.Item.FindControl("uxThresholdLowHigh")).Visible = !isNA;
                ((PlaceHolder)e.Item.FindControl("lblNA")).Visible = isNA;
            }
            else
            {
                switch (ParameterThresholdType)
                {
                    case "LowHigh":
                        ((PlaceHolder)e.Item.FindControl("uxThresholdNormal")).Visible = false;
                        ((PlaceHolder)e.Item.FindControl("uxThresholdLowHigh")).Visible = true;

                        ProcessThresholdColumn(txtThresholdLow, lblDollarThresholdLow, lblPercentThresholdLow, spanErrMsgThresholdLow, e);
                        ProcessThresholdColumn(txtThresholdHigh, lblDollarThresholdHigh, lblPercentThresholdHigh, spanErrMsgThresholdHigh, e, true);

                        break;
                    default:
                        ((PlaceHolder)e.Item.FindControl("uxThresholdNormal")).Visible = true;
                        ((PlaceHolder)e.Item.FindControl("uxThresholdLowHigh")).Visible = false;

                        ProcessThresholdColumn(txtThreshold, lblDollar, lblpercent, spanErrMessageThs, e);
                        break;
                }
            }

            ProcessIndicatorColumn(txtParameterValue, lblType, lblIndicatorDollar, spanErrMessage, lblIndicatorType, e);

            ProcessCheckBoxColumn(txtParameterValue, txtThreshold, txtThresholdLow, txtThresholdHigh, txtFrom, txtTo, e);

            //================== Re Alert process Indicator column==========================
            TextBox txtNRTParameterValue = e.Item.FindControl("txtNRTParameterValue") as TextBox;
            Literal lblTypeNRT = e.Item.FindControl("lblNRTParameterType") as Literal;
            // RegularExpressionValidator RequiredFieldValidator1 = e.Item.FindControl("RequiredFieldValidator1") as RegularExpressionValidator;
            Label lblNRTIndicatorDollar = e.Item.FindControl("lblNRTIndicatorDollar") as Label;
            HtmlGenericControl spanErrMessageNRT = e.Item.FindControl("spanNRTErrMsg") as HtmlGenericControl;

            //=================== Re Alert process Threshold column =======================
            TextBox txtNRTThreshold = e.Item.FindControl("txtNRTThreshold") as TextBox;
            DataRowView rowReView = e.Item.DataItem as DataRowView;
            Label lblNRTDollar = e.Item.FindControl("lblNRTDollar") as Label;
            Label lblNRTpercent = e.Item.FindControl("lblNRTpercent") as Label;
            HtmlGenericControl spanErrMessageThsNRT = e.Item.FindControl("spanNRTErrMsgThs") as HtmlGenericControl;

            TextBox txtNRTThresholdLow = e.Item.FindControl("txtNRTThresholdLow") as TextBox;
            TextBox txtNRTThresholdHigh = e.Item.FindControl("txtNRTThresholdHigh") as TextBox;

            Label lblNRTDollarThresholdLow = e.Item.FindControl("lblNRTDollarThresholdLow") as Label;
            Label lblNRTPercentThresholdLow = e.Item.FindControl("lblNRTPercentThresholdLow") as Label;

            Label lblNRTDollarThresholdHigh = e.Item.FindControl("lblNRTDollarThresholdHigh") as Label;
            Label lblNRTPercentThresholdHigh = e.Item.FindControl("lblNRTPercentThresholdHigh") as Label;

            HtmlGenericControl spanNRTErrMsgThresholdLow = e.Item.FindControl("spanNRTErrMsgThresholdLow") as HtmlGenericControl;
            HtmlGenericControl spanNRTErrMsgThresholdHigh = e.Item.FindControl("spanNRTErrMsgThresholdHigh") as HtmlGenericControl;


            var ParameterThresholdTypeNRT = row["ParameterThresholdType"].ToString();
            var ParameterThresholdNRT = row["ReAlertParameterThreshold"].ToString();
            var ParameterThresholdLowNRT = row["ReAlertParameterThreshold"].ToString();
            var ParameterThresholdHighNRT = row["ReAlertParameterThresholdHigh"].ToString();
            var IsEditIndicatorNRT = row["IsEditReAlertParameterIndicator"].ToBoolean();
            var IsEditThresholdNRT = row["IsEditReAlertParameterThreshold"].ToBoolean();

            TextBox txtNRTFrom = e.Item.FindControl("txtNRTFrom") as TextBox;
            TextBox txtNRTTo = e.Item.FindControl("txtNRTTo") as TextBox;
            if (row["ReAlertParameterIndicator"] != DBNull.Value && !row["ReAlertParameterIndicator"].ToString().IsNullOrEmpty()
                && row["ReAlertParameterIndicatorHigh"] != DBNull.Value && !row["ReAlertParameterIndicatorHigh"].ToString().IsNullOrEmpty())
            {
                var ParameterValueNRT = row["ReAlertParameterIndicator"].ToString();
                var ParameterValueHighNRT = row["ReAlertParameterIndicatorHigh"].ToString();
                HtmlGenericControl divNRT = e.Item.FindControl("div1_NRT") as HtmlGenericControl;
                HtmlGenericControl uxNRTMerchantFilter = e.Item.FindControl("uxNRTMerchantFilter") as HtmlGenericControl;
                divNRT.Visible = false;
                uxNRTMerchantFilter.Visible = true;

                txtNRTFrom.Text = VeraCodeSolution.DoVeraCode(Convert.ToInt32(GetParameterNumber(ParameterValueNRT)).ToString());
                txtNRTTo.Text = VeraCodeSolution.DoVeraCode(Convert.ToInt32(GetParameterNumber(ParameterValueHighNRT)).ToString());
            }
            bool isNANRT = false;
            if (ParameterThresholdTypeNRT == "LowHigh")
            {
                if (ParameterThresholdLowNRT.IsNullOrEmpty() && ParameterThresholdHighNRT.IsNullOrEmpty() && !IsEditThresholdNRT)
                {
                    isNANRT = true;
                }
            }
            else
            {
                if (ParameterThresholdNRT.IsNullOrEmpty() && !IsEditThresholdNRT)
                {
                    isNANRT = true;
                }
            }

            if (isNANRT)
            {
                ((PlaceHolder)e.Item.FindControl("uxNRTThresholdNormal")).Visible =
                         ((PlaceHolder)e.Item.FindControl("uxNRTThresholdLowHigh")).Visible = !isNANRT;
                ((PlaceHolder)e.Item.FindControl("lblNRTNA")).Visible = isNANRT;
            }
            else
            {
                switch (ParameterThresholdTypeNRT)
                {
                    case "LowHigh":
                        ((PlaceHolder)e.Item.FindControl("uxNRTThresholdNormal")).Visible = false;
                        ((PlaceHolder)e.Item.FindControl("uxNRTThresholdLowHigh")).Visible = true;

                        ProcessThresholdColumnNRT(txtNRTThresholdLow, lblNRTDollarThresholdLow, lblNRTPercentThresholdLow, spanNRTErrMsgThresholdLow, e, IsEditThresholdNRT);
                        ProcessThresholdColumnNRT(txtNRTThresholdHigh, lblNRTDollarThresholdHigh, lblNRTPercentThresholdHigh, spanNRTErrMsgThresholdHigh, e, IsEditThresholdNRT, true);

                        break;
                    default:
                        ((PlaceHolder)e.Item.FindControl("uxNRTThresholdNormal")).Visible = true;
                        ((PlaceHolder)e.Item.FindControl("uxNRTThresholdLowHigh")).Visible = false;

                        ProcessThresholdColumnNRT(txtNRTThreshold, lblNRTDollar, lblNRTpercent, spanErrMessageThsNRT, e, IsEditThresholdNRT);
                        break;
                }
            }

            ProcessIndicatorColumnNRT(txtNRTParameterValue, lblTypeNRT, lblNRTIndicatorDollar, spanErrMessageNRT, lblIndicatorMCFType, e, IsEditIndicatorNRT);
            ProcessCheckBoxColumn(txtNRTParameterValue, txtNRTThreshold, txtNRTThresholdLow, txtNRTThresholdHigh, txtNRTFrom, txtNRTTo, e);

        }
        else if (e.Item is GridGroupHeaderItem)
        {
            var gHeaderItem = e.Item as GridGroupHeaderItem;
            ProcessGroupHeaderItemStyle(gHeaderItem);

        }

        //fire event
        this.OnItemDataBound(e);
    }

    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {
        uxParameterList.Columns.FindByUniqueName("ParameterCheckBox").Visible = false;
        uxParameterList.Columns.FindByUniqueName("ParameterValue").Visible = false;
        uxParameterList.Columns.FindByUniqueName("ParameterThreshold").Visible = false;
        uxParameterList.Columns.FindByUniqueName("ParameterDescriptionExport").Visible = true;

        if (((UxExport)sender).ExportButtonType == UxExport.ExportType.Excel)
        {
            uxParameterList.Columns.FindByUniqueName("ParameterValueExportCSV").Visible = false;
            uxParameterList.Columns.FindByUniqueName("ParameterValueExport").Visible = true;
            uxParameterList.Columns.FindByUniqueName("ParameterValueExportCSVNRT").Visible = false;
            uxParameterList.Columns.FindByUniqueName("ParameterValueExportNRT").Visible = true;
        }
        else
        {
            uxParameterList.Columns.FindByUniqueName("ParameterValueExportCSV").Visible = true;
            uxParameterList.Columns.FindByUniqueName("ParameterValueExport").Visible = false;
            uxParameterList.Columns.FindByUniqueName("ParameterValueExportCSVNRT").Visible = true;
            uxParameterList.Columns.FindByUniqueName("ParameterValueExportNRT").Visible = false;
            uxParameterList.Columns.FindByUniqueName("ParameterDescription").Visible = false;

        }

        this.uxParameterList.Columns.FindByUniqueName("GroupNameDummy").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("GroupName").Visible = true;
        this.uxParameterList.Columns.FindByUniqueName("ParamRowNumber").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ParameterID").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ParameterCode").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("IsAssigned").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ActivityStatus").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ParameterValue").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ParameterValueHide").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ParameterThresholdHide").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ParameterDataTypeHide").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ParameterNameHide").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ThresholdMinValue").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ThresholdMaxValue").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ReAlertParameterThresholdMin").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ReAlertParameterThresholdMax").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("IndicatorMin").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("IndicatorMax").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ParameterDescription").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("IsSelected").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("IsActive").Visible = false;

        this.uxParameterList.Columns.FindByUniqueName("IndicatorNegative").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ThresholdNegative").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("UsedByAssignments").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("UsedByRiskScores").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ParameterThresholdType").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ThresholdNegative").Visible = false;

        this.uxParameterList.Columns.FindByUniqueName("ParameterThresholdExportNRT").Visible = true;
        this.uxParameterList.Columns.FindByUniqueName("ReAlertParameterIndicator").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ReAlertParameterThreshold").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("IsEditReAlertParameterIndicator").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("IsEditReAlertParameterThreshold").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ParameterValueHideNRT").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ParameterThresholdHideNRT").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ReAlertParameterIndicatorMin").Visible = false;
        this.uxParameterList.Columns.FindByUniqueName("ReAlertParameterIndicatorMax").Visible = false;

        UxExport uxExport = sender as UxExport;
        exportConfig.FileName = GetLocalResourceObject("Risk_Parameter_ascx_cs_RiskMngAssignPara_FileName").ToString();
        exportConfig.ReportHeader = GetLocalResourceObject("Risk_Parameter_ascx_cs_RiskMngAssignPara_ReportHeader").ToString();
        if (this.NeedExportConfig != null)
            this.NeedExportConfig(sender, exportConfig);
    }

    private void ProcessGroupHeaderItemStyle(GridGroupHeaderItem gHeaderItem)
    {
        gHeaderItem.Cells[0].Visible = false;
        gHeaderItem.Cells[1].Attributes.Add("colspan", "9");

        gHeaderItem.Cells[1].Style.Add(HtmlTextWriterStyle.TextAlign, "center");
        var groupText = gHeaderItem.Cells[1].Text;
        groupText = groupText.Substring(groupText.IndexOf(":") + 1).Trim();
        switch (groupText)
        {
            case "A":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ascx_cs_VolumeBatchTicket").ToString();
                break;
            case "B":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ascx_cs_Duplicates").ToString();
                break;
            case "C":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ascx_cs_Credits").ToString();
                break;
            case "D":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ascx_cs_Contract").ToString();
                break;
            case "E":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ascx_cs_CbRt").ToString();
                break;
            case "F":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ascx_cs_Attrition").ToString();
                break;
            case "G":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ascx_cs_ACHReturns").ToString();
                break;
            case "H":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ascx_cs_Auth").ToString();
                break;
            case "I":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ascx_cs_Deposits").ToString();
                break;
            case "J":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ascx_cs_BIN").ToString();
                break;
            case "K":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ascx_cs_MerchantFilter").ToString();
                break;
            default:
                break;
        }
    }

    private void ProcessHeaderItemStyle(GridHeaderItem headerItem)
    {
        if (this.Request.Browser.Type.Contains("IE"))
            headerItem[PARAM_CHECKBOX].Text = VeraCodeSolution.DoVeraCode(headerItem[PARAM_CHECKBOX].Text.Replace("-3px", "-6px"));
        //headerItem[PARAM_NAME].Style.Add(HtmlTextWriterStyle.TextAlign, "left !important");
        //headerItem[PARAM_NAME].Style.Add(HtmlTextWriterStyle.PaddingBottom, "4px !important");
        if (this.uxParameterList.DataSource == null ||
            (this.uxParameterList.DataSource != null && (this.uxParameterList.DataSource as DataTable).Rows.Count == 0))
        {
            headerItem[PARAM_CHECKBOX].Text = string.Empty;
            headerItem[PARAM_CHECKBOX].Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px");
            this.uxParameterList.Columns.FindByUniqueName(PARAM_CHECKBOX).HeaderText = string.Empty;
        }
        else
            headerItem[PARAM_CHECKBOX].Style.Add("text-align", "left");

    }

    private void ProcessDataItemStyle(GridDataItem dataItem)
    {

        //dataItem[PARAM_CHECKBOX].Style.Add("border-left", "0");
        //dataItem[PARAM_CHECKBOX].Style.Add("margin-left", "0");
        //dataItem[PARAM_CHECKBOX].Style.Add("padding-left", "0");

    }

    private void ProcessIndicatorColumn(TextBox txtParameterValue, Literal lblType, Label lblIndicatorDollar, HtmlGenericControl errControl, Label lblIndicatorType, GridItemEventArgs e)
    {
        DataRowView dataRow = e.Item.DataItem as DataRowView;
        var dataItem = e.Item as GridDataItem;
        var type = lblType.Text.Replace(NBSP, string.Empty).Trim();
        string paramKey = dataItem[PARAM_KEY].Text;

        //[44432]: Bug #37335
        int ParameterPrecision = !string.IsNullOrEmpty(dataRow[PARAM_PRECISION].ToString()) ? int.Parse(dataRow[PARAM_PRECISION].ToString()) : 0;

        if (txtParameterValue.Text.Trim() != string.Empty)
        {
            //add to check indicator max, min, precision, error controlID value
            if (dataItem[IND_MAXVALUE].Text.Replace(NBSP, string.Empty).Trim() != string.Empty)
                txtParameterValue.Attributes[MAX_VAL] = dataRow[IND_MAXVALUE].ToString();
            txtParameterValue.Attributes[MIN_VAL] = dataRow[IND_MINVALUE].ToString();
            txtParameterValue.Attributes[PRECISION] = dataRow[PARAM_PRECISION].ToString();
            txtParameterValue.Attributes[ERR_CONTROLID] = errControl.ClientID;

            if (this._IsSort)
                txtParameterValue.Text = VeraCodeSolution.DoVeraCode(this.GetIndicatorFromParameterList(paramKey));

            if (type == "$")
            {

                lblType.Text = NBSP;
                lblIndicatorDollar.Visible = true;
                //[38605] 
                lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + type);
                //[44432]: Bug #37335
                txtParameterValue.Text = this.GetDouble4Precision(txtParameterValue.Text.Trim().TrimStart('-'), ParameterPrecision);
            }
            else
            {
                lblType.Visible = true;
                //lblIndicatorDollar.Visible = false;
                //[38605] 
                lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP);
                //lblIndicatorDollar.Attributes.CssStyle["visibility"] = "hidden";

                //[44432]: Bug #37335
                if (type == "#" || type == "%")
                    txtParameterValue.Text = VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtParameterValue.Text, ParameterPrecision).Trim().TrimStart('-'));
                else if (type == "days")
                    txtParameterValue.Text = VeraCodeSolution.DoVeraCode(this.GetIntegerNumber(txtParameterValue.Text).Trim().TrimStart('-'));
                else if (type.Length == 0)
                {
                    //lblIndicatorDollar.Visible = false;
                    txtParameterValue.Visible = true;
                    txtParameterValue.Text = string.Empty;
                    txtParameterValue.Font.Bold = true;
                    dataItem[PARAM_VALUE].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            if (dataItem["IndicatorNegative"].Text.ToString() != "" && dataItem["IndicatorNegative"].Text.ToString() == "Yes")
            {
                HtmlGenericControl div = e.Item.FindControl("div1") as HtmlGenericControl;
                div.Attributes.CssStyle["color"] = "Red";
                lblIndicatorDollar.Visible = true;
                txtParameterValue.ForeColor = Color.Red;
                if (type == "$")
                {
                    lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + "$(");
                }
                else
                {
                    //[38605] 
                    lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + "(");
                }
                lblType.Text = ")" + lblType.Text;
            }
        }
        else
        {
            txtParameterValue.Visible = false;

            lblType.Visible = true;
            lblType.Text = string.Empty;
            lblIndicatorType.Visible = false;
            // HtmlGenericControl div = e.Item.FindControl("div1") as HtmlGenericControl;
            // div.Attributes.CssStyle["text-align"] = "center";
            dataItem[PARAM_VALUE].HorizontalAlign = HorizontalAlign.Center;
        }
    }

    private void ProcessIndicatorColumnNRT(TextBox txtParameterValue, Literal lblType, Label lblIndicatorDollar, HtmlGenericControl errControl, Label lblIndicatorType, GridItemEventArgs e, bool IsEditIndicator)
    {

        DataRowView dataRow = e.Item.DataItem as DataRowView;
        var dataItem = e.Item as GridDataItem;
        var type = lblType.Text.Replace(NBSP, string.Empty).Trim();
        string paramKey = dataItem[PARAM_KEY].Text;

        //[44432]: Bug #37335
        int ParameterPrecision = !string.IsNullOrEmpty(dataRow[PARAM_PRECISION].ToString()) ? int.Parse(dataRow[PARAM_PRECISION].ToString()) : 0;

        if (txtParameterValue.Text.Trim() != string.Empty || IsEditIndicator)
        {
            //add to check indicator max, min, precision, error controlID value
            if (dataItem[IND_MAX_MCF].Text.Replace(NBSP, string.Empty).Trim() != string.Empty)
                txtParameterValue.Attributes[MAX_VAL] = dataRow[IND_MAX_MCF].ToString();
            txtParameterValue.Attributes[MIN_VAL] = dataRow[IND_MIN_MCF].ToString();
            txtParameterValue.Attributes[PRECISION] = dataRow[PARAM_PRECISION].ToString();
            txtParameterValue.Attributes[ERR_CONTROLID] = errControl.ClientID;

            if (this._IsSort)
                txtParameterValue.Text = VeraCodeSolution.DoVeraCode(this.GetIndicatorFromParameterList(paramKey, true));

            if (type == "$")
            {
                lblType.Text = NBSP;
                lblIndicatorDollar.Visible = true;
                //[38605] 
                lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + type);
                //[44432]: Bug #37335
                txtParameterValue.Text = string.IsNullOrEmpty(txtParameterValue.Text.Trim()) ? string.Empty : this.GetDouble4Precision(txtParameterValue.Text.Trim().TrimStart('-'), ParameterPrecision);
            }
            else
            {
                lblType.Visible = true;
                //lblIndicatorDollar.Visible = false;
                //[38605] 
                lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP);
                //lblIndicatorDollar.Attributes.CssStyle["visibility"] = "hidden";

                //[44432]: Bug #37335
                if (type == "#" || type == "%")
                    txtParameterValue.Text = (IsEditIndicator && string.IsNullOrEmpty(txtParameterValue.Text)) ? string.Empty :
                        VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtParameterValue.Text, ParameterPrecision).Trim().TrimStart('-'));
                else if (type == "days")
                    txtParameterValue.Text = (IsEditIndicator && string.IsNullOrEmpty(txtParameterValue.Text)) ? string.Empty :
                        VeraCodeSolution.DoVeraCode(this.GetIntegerNumber(txtParameterValue.Text).Trim().TrimStart('-'));
                else if (type.Length == 0)
                {
                    //lblIndicatorDollar.Visible = false;
                    txtParameterValue.Visible = true;
                    txtParameterValue.Text = string.Empty;
                    txtParameterValue.Font.Bold = true;
                    dataItem[REALERT_PARAM_VALUE].HorizontalAlign = HorizontalAlign.Center;

                }
            }
            if (dataItem["IndicatorNegative"].Text.ToString() != "" && dataItem["IndicatorNegative"].Text.ToString() == "Yes")
            {
                HtmlGenericControl div = e.Item.FindControl("div1_NRT") as HtmlGenericControl;
                div.Attributes.CssStyle["color"] = "Red";
                lblIndicatorDollar.Visible = true;
                txtParameterValue.ForeColor = Color.Red;
                if (type == "$")
                {
                    lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + "$(");
                }
                else
                {
                    //[38605] 
                    lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + "(");
                }
                lblType.Text = ")" + lblType.Text;
            }
        }
        else
        {
            txtParameterValue.Visible = false;

            lblType.Visible = true;
            lblType.Text = string.Empty;
            // HtmlGenericControl div = e.Item.FindControl("div1_NRT") as HtmlGenericControl;
            lblIndicatorType.Visible = false;
            //  div.Attributes.CssStyle["text-align"] = "center";
            dataItem[REALERT_PARAM_VALUE].HorizontalAlign = HorizontalAlign.Center;
        }
    }

    private void ProcessThresholdColumn(TextBox txtThreshold, Label lblDollar, Label lblpercent, HtmlGenericControl errControl, GridItemEventArgs e)
    {
        ProcessThresholdColumn(txtThreshold, lblDollar, lblpercent, errControl, e, false);
    }

    private void ProcessThresholdColumn(TextBox txtThreshold, Label lblDollar, Label lblpercent, HtmlGenericControl errControl, GridItemEventArgs e, bool isHighValue)
    {
        var dataItem = e.Item as GridDataItem;
        var rowView = dataItem.DataItem as DataRowView;

        var type = lblpercent.Text.Replace(NBSP, string.Empty).Trim();
        string paramKey = dataItem[PARAM_KEY].Text;

        Type DBNull = Type.GetType("System.DBNull");
        bool hasNoThreshold = false;

        hasNoThreshold = rowView[PARAM_THRESHOLD].GetType() == DBNull || rowView["ThresholdMax"].GetType() == DBNull
                        || decimal.Parse(rowView["ThresholdMax"].ToString()) == 0;

        //mean that this field is 'empty' but allow user to enter value
        if (dataItem[PARAM_THRESHOLDHIDE].Text == "0dds0") // current sys dont have this case 
        {
            if (dataItem[THRESHOLD_MAXVAL].Text.Replace(NBSP, string.Empty).Trim() != string.Empty)
                txtThreshold.Attributes[MAX_VAL] = VeraCodeSolution.DoVeraCode(dataItem[THRESHOLD_MAXVAL].Text);

            if (this.ParameterChangeList != null && this.ParameterChangeList.Count > 0)
            {
                //in this case update
                RiskParameter param = this.ParameterChangeList.GetRiskParameterByName(dataItem["ParameterNameHide"].Text.Trim());
                if (param != null)
                {
                    if (param.ThresholdValue != null && !double.IsNaN(param.ThresholdValue.Value))
                    {
                        txtThreshold.Text = VeraCodeSolution.DoVeraCode(param.ThresholdValue.Value.ToString("#,#0.0000").Trim().TrimStart('-'));
                    }
                    else
                        txtThreshold.Text = string.Empty;
                }
                else
                    txtThreshold.Text = string.Empty;
            }
            else
            {
                txtThreshold.Text = string.Empty;
            }

        }
        else if (hasNoThreshold)
        {
            ((PlaceHolder)e.Item.FindControl("lblNA")).Visible = true;
            lblDollar.Visible = false;
            txtThreshold.Visible = false;
            lblpercent.Visible = false;
        }
        else
        {
            //add to check threshold min, max value
            if (dataItem[THRESHOLD_MAXVAL].Text.Replace(NBSP, string.Empty).Trim() != string.Empty)
                txtThreshold.Attributes[MAX_VAL] = VeraCodeSolution.DoVeraCode(dataItem[THRESHOLD_MAXVAL].Text);
            txtThreshold.Attributes[MIN_VAL] = VeraCodeSolution.DoVeraCode(dataItem[THRESHOLD_MINVAL].Text);
            txtThreshold.Attributes[ERR_CONTROLID] = VeraCodeSolution.DoVeraCode(errControl.ClientID);
            if (this._IsSort)
                txtThreshold.Text = (!isHighValue) ? VeraCodeSolution.DoVeraCode(GetThresholdFromParameterList(paramKey).Trim().TrimStart('-')) :
                    VeraCodeSolution.DoVeraCode(GetThresholdHighFromParameterList(paramKey).Trim().TrimStart('-'));

            txtThreshold.Text = VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtThreshold.Text).Trim().TrimStart('-'));
            //dataItem[PARAM_THRESHOLD].HorizontalAlign = HorizontalAlign.Center;

            if (type == "$")
            {
                var parameterThresholdType = dataItem["ParameterThresholdType"].Text;
                lblDollar.Visible = true;
                lblDollar.Text = parameterThresholdType.Equals("LowHigh") ? VeraCodeSolution.DoVeraCode(type) : VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + type);
                lblpercent.Visible = false;
            }
            else
            {
                //lblDollar.Visible = false;
                lblpercent.Visible = true;
                //lblIndicatorDollar.Visible = false;
                //[38605] 
                lblDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP);
                //lblIndicatorDollar.Attributes.CssStyle["visibility"] = "hidden";

                if (type == "#" || type == "%")
                    txtThreshold.Text = VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtThreshold.Text).Trim().TrimStart('-'));
                else if (type == "days")
                    txtThreshold.Text = VeraCodeSolution.DoVeraCode(this.GetIntegerNumber(txtThreshold.Text).Trim().TrimStart('-'));
            }
        }

        if (dataItem["ThresholdNegative"].Text.ToString() != "" && dataItem["ThresholdNegative"].Text.ToString() == "Yes" && txtThreshold.Text != "")
        {
            HtmlGenericControl div = e.Item.FindControl("dvThreshold") as HtmlGenericControl;
            div.Attributes.CssStyle["color"] = "Red";
            txtThreshold.ForeColor = Color.Red;
            lblpercent.Visible = true;
            if (type == "$")
            {
                lblDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + "$(");
                lblpercent.Text = ")";
            }
            else
            {
                lblDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + "(");
                lblpercent.Text = ")" + lblpercent.Text;
            }
        }
    }

    private void ProcessThresholdColumnNRT(TextBox txtThreshold, Label lblDollar, Label lblpercent, HtmlGenericControl errControl, GridItemEventArgs e, bool isEditThreshold)
    {
        ProcessThresholdColumnNRT(txtThreshold, lblDollar, lblpercent, errControl, e, isEditThreshold, false);
    }

    private void ProcessThresholdColumnNRT(TextBox txtThreshold, Label lblDollar, Label lblpercent, HtmlGenericControl errControl, GridItemEventArgs e, bool isEditThreshold, bool isHighValue)
    {
        var dataItem = e.Item as GridDataItem;
        var rowView = dataItem.DataItem as DataRowView;

        var type = lblpercent.Text.Replace(NBSP, string.Empty).Trim();
        string paramKey = dataItem[PARAM_KEY].Text;

        Type DBNull = Type.GetType("System.DBNull");
        bool hasNoThreshold = false;

        hasNoThreshold = rowView[REALERT_PARAM_THRES].GetType() == DBNull || rowView["ReAlertParameterThresholdMax"].GetType() == DBNull
                        || decimal.Parse(rowView["ReAlertParameterThresholdMax"].ToString()) == 0;


        //mean that this field is 'empty' but allow user to enter value
        if (dataItem[PARAM_THRESHOLDHIDEMCF].Text == "0dds0") // current sys dont have this case 
        {
            if (dataItem[THRES_MAX_MCF].Text.Replace(NBSP, string.Empty).Trim() != string.Empty)
                txtThreshold.Attributes[MAX_VAL] = VeraCodeSolution.DoVeraCode(dataItem[THRES_MAX_MCF].Text);

            if (this.ParameterChangeList != null && this.ParameterChangeList.Count > 0)
            {
                //in this case update
                RiskParameter param = this.ParameterChangeList.GetRiskParameterByName(dataItem["ParameterNameHide"].Text.Trim());
                if (param != null)
                {
                    if (param.ThresholdValue != null && !double.IsNaN(param.ThresholdValue.Value))
                    {
                        txtThreshold.Text = VeraCodeSolution.DoVeraCode(param.ThresholdValue.Value.ToString("#,#0.0000").Trim().TrimStart('-'));
                    }
                    else
                        txtThreshold.Text = string.Empty;
                }
                else
                    txtThreshold.Text = string.Empty;
            }
            else
            {
                txtThreshold.Text = string.Empty;
            }

        }
        else if (hasNoThreshold && !isEditThreshold)
        {
            ((PlaceHolder)e.Item.FindControl("lblNRTNA")).Visible = true;
            lblDollar.Visible = false;
            txtThreshold.Visible = false;
        }
        else
        {
            //add to check threshold min, max value
            if (dataItem[THRESHOLD_MAXVAL].Text.Replace(NBSP, string.Empty).Trim() != string.Empty)
                txtThreshold.Attributes[MAX_VAL] = VeraCodeSolution.DoVeraCode(dataItem[THRES_MAX_MCF].Text);
            txtThreshold.Attributes[MIN_VAL] = VeraCodeSolution.DoVeraCode(dataItem[THRES_MIN_MCF].Text);
            txtThreshold.Attributes[ERR_CONTROLID] = VeraCodeSolution.DoVeraCode(errControl.ClientID);
            if (this._IsSort)
                txtThreshold.Text = (isEditThreshold && string.IsNullOrEmpty(txtThreshold.Text)) ? string.Empty :
                    (!isHighValue) ? VeraCodeSolution.DoVeraCode(GetThresholdFromParameterList(paramKey, true).Trim().TrimStart('-')) :
                                     VeraCodeSolution.DoVeraCode(GetThresholdHighFromParameterList(paramKey, true).Trim().TrimStart('-'));

            txtThreshold.Text = (isEditThreshold && string.IsNullOrEmpty(txtThreshold.Text)) ? string.Empty :
                VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtThreshold.Text).Trim().TrimStart('-'));
            //dataItem[PARAM_THRESHOLD].HorizontalAlign = HorizontalAlign.Center;

            if (type == "$")
            {
                var parameterThresholdType = dataItem["ParameterThresholdType"].Text;
                lblDollar.Visible = true;
                lblDollar.Text = parameterThresholdType.Equals("LowHigh") ? VeraCodeSolution.DoVeraCode(type) : VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + type);
                lblpercent.Visible = false;
            }
            else
            {
                //lblDollar.Visible = false;
                lblpercent.Visible = true;
                //lblIndicatorDollar.Visible = false;
                //[38605] 
                lblDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP);
                //lblIndicatorDollar.Attributes.CssStyle["visibility"] = "hidden";

                if (type == "#" || type == "%")
                    txtThreshold.Text = (isEditThreshold && rowView[REALERT_PARAM_THRES].GetType() == DBNull) ? string.Empty :
                        VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtThreshold.Text).Trim().TrimStart('-'));
                else if (type == "days")
                    txtThreshold.Text = (isEditThreshold && rowView[REALERT_PARAM_THRES].GetType() == DBNull) ? string.Empty :
                        VeraCodeSolution.DoVeraCode(this.GetIntegerNumber(txtThreshold.Text).Trim().TrimStart('-'));
            }
        }

        if (dataItem["ThresholdNegative"].Text.ToString() != "" && dataItem["ThresholdNegative"].Text.ToString() == "Yes" && txtThreshold.Text != "")
        {
            HtmlGenericControl div = e.Item.FindControl("dvThresholdNRT") as HtmlGenericControl;
            div.Attributes.CssStyle["color"] = "Red";
            txtThreshold.ForeColor = Color.Red;
            lblpercent.Visible = true;
            if (type == "$")
            {
                lblDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + "$(");
                lblpercent.Text = ")";
            }
            else
            {
                lblDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + "(");
                lblpercent.Text = ")" + lblpercent.Text;
            }
        }
    }

    // Hide group column
    protected void uxParameterList_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            e.Column.Visible = false;
        }
    }

    private void ProcessCheckBoxColumn(TextBox txtParameterValue, TextBox txtThreshold, TextBox txtThresholdLow, TextBox txtThresholdHigh, TextBox txtFrom, TextBox txtTo, GridItemEventArgs e)
    {
        DataRowView dataRow = e.Item.DataItem as DataRowView;
        var dataItem = e.Item as GridDataItem;
        bool isNew = false;
        string paramKey = dataItem[PARAM_KEY].Text;
        string paramValue = txtParameterValue.Text;

        string paramThreshold = dataItem[PARAM_THRESHOLDHIDE].Text;
        HtmlInputCheckBox checkBox = e.Item.FindControl("chkRowIndex") as HtmlInputCheckBox;
        string indicatorID = string.Empty;
        string thresholdID = string.Empty;
        string activityStatus = string.Empty;
        if (!this.IsFromAssignment)
            activityStatus = this._IsSort ? GetActivityStatusFromParameterList(paramKey)
                                             : dataItem[ACTIVITY_STATUS].Text.Trim();
        else
            activityStatus = this._IsSort ? GetActivityStatusFromParameterList(paramKey)
                                             : dataItem[IS_SELECTED].Text.Trim();

        indicatorID = !isNew && paramValue.Trim() == string.Empty ? GetLocalResourceObject("Risk_Parameter_ascx_cs_NewParameter").ToString() : txtParameterValue.ClientID;

        thresholdID = txtThreshold.ClientID;

        checkBox.Attributes["onclick"] = string.Format("parameterCheckBox_Click(this, '{0}', '{1}', '{2}', {3});",
            paramKey, indicatorID, thresholdID, (txtThreshold.Text.Trim() == string.Empty).ToString().ToLower());

        if (activityStatus == "1")
        {
            checkBox.Checked = true;
        }
        else
        {
            checkBox.Checked = false;
        }

        txtParameterValue.Enabled = txtThreshold.Enabled = txtThresholdLow.Enabled = txtThresholdHigh.Enabled = txtFrom.Enabled = txtTo.Enabled = checkBox.Checked;

        if (this.TempDisableDuplicates == true && dataItem[GROUP_NAME].Text == "B") //"B" mean Duplicates group.
        {
            checkBox.Disabled = true;
            dataItem.ToolTip = GetLocalResourceObject("Risk_Parameter_ascx_cs_DuplicateQuickSearch").ToString();
        }

        string assList = string.Empty;
        assList = dataRow[USEDBY_ASS].ToString().Replace(",", ", ");

        string isRiskScore = dataRow["UsedByRiskScores"].ToString().ToLower();

        this.ParameterInfo.Add(
        string.Format("{{ CbClientId:\"{0}\", ParamKey:\"{1}\", IndClientId:\"{2}\", ThsClientId:\"{3}\", IsUseThs:{4}, AssList:\"{5}\", RiskScoreList:\"{6}\", ThsLowID:\"{7}\", ThsHighID:\"{8}\", IndFromID:\"{9}\", IndToID:\"{10}\" }}",
                checkBox.ClientID, paramKey, txtParameterValue.ClientID, txtThreshold.ClientID, (txtThreshold.Text.Trim() != string.Empty).ToString().ToLower(), assList, isRiskScore, txtThresholdLow.ClientID, txtThresholdHigh.ClientID, txtFrom.ClientID, txtTo.ClientID));

    }

    private string GetActivityStatusFromParameterList(string paramKey)
    {
        var param = this.ParameterList.Where(p => p.Key == paramKey);
        return param.First().ActivityStatus.ToString();
    }

    private string GetThresholdFromParameterList(string paramKey)
    {
        return GetThresholdFromParameterList(paramKey, false);
    }

    private string GetThresholdFromParameterList(string paramKey, bool IsReAlert)
    {
        var param = this.ParameterList.Where(p => p.Key == paramKey).FirstOrDefault();
        if (IsReAlert)
        {
            return param == null || param.ReAlertThresholdValue == null ? string.Empty : param.ReAlertThresholdValue.Value.ToString("#,#0.0000");
        }
        return param == null || param.ThresholdValue == null ? string.Empty : param.ThresholdValue.Value.ToString("#,#0.0000");
    }

    private string GetThresholdHighFromParameterList(string paramKey)
    {
        return GetThresholdHighFromParameterList(paramKey, false);
    }

    private string GetThresholdHighFromParameterList(string paramKey, bool IsReAlert)
    {
        var param = this.ParameterList.Where(p => p.Key == paramKey).FirstOrDefault();
        if (IsReAlert)
        {
            return param == null || param.ReAlertThresholdHigh == null ? string.Empty : param.ReAlertThresholdHigh.Value.ToString("#,#0.0000");
        }
        return param == null || param.ThresholdHigh == null ? string.Empty : param.ThresholdHigh.Value.ToString("#,#0.0000");
    }

    private string GetIndicatorFromParameterList(string paramKey)
    {
        return GetIndicatorFromParameterList(paramKey, false);
    }

    private string GetIndicatorFromParameterList(string paramKey, bool IsReAlert)
    {
        var param = this.ParameterList.Where(p => p.Key == paramKey).FirstOrDefault();
        if (IsReAlert)
        {
            return param == null || param.ReAlertIndicatorValue == null ? string.Empty : param.ReAlertIndicatorValue.Value.ToString("#,#0.0000");
        }
        return param == null || param.IndicatorValue == null ? string.Empty : param.IndicatorValue.Value.ToString("#,#0.0000");
    }

    private string GetIndicatorHighFromParameterList(string paramKey)
    {
        return GetIndicatorHighFromParameterList(paramKey, false);
    }

    private string GetIndicatorHighFromParameterList(string paramKey, bool IsReAlert)
    {
        var param = this.ParameterList.Where(p => p.Key == paramKey).FirstOrDefault();
        if (IsReAlert)
        {
            return param == null || param.ReAlertIndicatorHigh == null ? string.Empty : param.ReAlertIndicatorHigh.Value.ToString("#,#0.0000");
        }
        return param == null || param.IndicatorHigh == null ? string.Empty : param.IndicatorHigh.Value.ToString("#,#0.0000");
    }

    protected virtual void OnNeedDataSource(Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (this.NeedDataSource != null)
            this.NeedDataSource(this, e);
    }

    protected virtual void OnItemDataBound(Telerik.Web.UI.GridItemEventArgs e)
    {
        if (this.ItemDataBound != null)
            this.ItemDataBound(this, e);
    }

    protected virtual void OnProcessSave(EventArgs e)
    {
        if (this.ProcessSave != null)
            this.ProcessSave(this, e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        //RiskParameter infor list
        string paramInfo = "ParameterInfo = [];";
        foreach (string info in this.ParameterInfo)
        {
            paramInfo += "ParameterInfo.push(" + info + ");";
        }

        this.Page.ClientScript.RegisterStartupScript(
            this.GetType(), "jsParameterInfo", paramInfo, true);

        var ram = RadAjaxManager.GetCurrent(this.Page);
        if (ram != null)
            ram.ResponseScripts.Add("populateCheckBoxAllCtrl();");

        //check the checkbox all
        bool isAllChecked = this.ParameterList.Count(p => p.ActivityStatus == 0) == 0;
        if (isAllChecked)
            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(), "jsTickCheckBoxAll", "populateCheckBoxAllCtrl();", true);

        //using for market data
        //string usedMarketData = "usedMarketData = " + SessionManager.IsUsingMarketData.ToString();
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "jsUsedMarketData", "var usedMarketData = " + RiskSessionManager.IsUsingMarketData.ToString().ToLower(), true);
    }

    private string GetIntegerNumber(string intStr)
    {
        if (intStr.Contains("."))
            return intStr.Substring(0, intStr.IndexOf('.'));

        return intStr;
    }
    private string GetDouble4Precision(string doubleStr)
    {
        if (doubleStr.Contains("."))
        {
            if (!doubleStr.EndsWith("0000"))
                return double.Parse(doubleStr).ToString("#,#0.0000");
        }

        return string.IsNullOrEmpty(doubleStr) ? string.Empty : double.Parse(doubleStr).ToString("#,#0");
    }

    //[44432]: Bug #37335
    private string GetDouble4Precision(string doubleStr, int PrecisionCount)
    {
        string p = string.Empty;
        for (int i = 0; i < PrecisionCount; i++)
        {
            p = p + "0";
        }

        if (doubleStr.Contains("."))
        {
            if (!doubleStr.EndsWith("." + p))
                return double.Parse(doubleStr).ToString("#,#0." + p);
        }

        return double.Parse(doubleStr).ToString("#,#0");
    }

    protected double? GetParameterNumber(string number)
    {
        if (number == "")
            return null;
        else if (number == "0dds0")
            return double.NaN;

        return double.Parse(number);
    }
}
