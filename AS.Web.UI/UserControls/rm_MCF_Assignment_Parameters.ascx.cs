using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.UserControls;
using Telerik.Web.UI;
using AS.Common;
using AS.Web.UI.Controls;
using System.Drawing;

public partial class UserControls_rm_MCF_Assignment_Parameters : GlobalUserControl, IRiskParamFilter
{


    enum DataBindAction
    {
        BindParameterList
    }

    enum PostBackAction
    {
        RefreshParameterList,
        DeleteParameter
    }

    #region Properties
    protected bool IsEdit = false;

    private string _QueryString;
    public string QueryString
    {
        get
        {

            _QueryString = Page.BuildSecureQueryString(string.Format("PrimaryID={0}&Mode={1}", PrimaryID, Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment));
            return _QueryString;
        }
        set { _QueryString = value; }
    }


    private DataTable source = null;

    public WebSiteEnums.FeatureMode FeatureMode { get; set; }

    #endregion


    #region IRiskParamFilter
    public WebSiteEnums.ParamFilterMode Mode { get; set; }
    public string PrimaryID { get; set; }//Can be: Assignment ID, ParameterDefineID,  AdhocID base on the mode
    public string ParamID { get; set; }//can be null
    public string FilterID { get; set; }//preserve
    public int Save()
    {
        this.SaveAssignment();
        this.SaveParameters();
        return 1;
    }//return error code, if needed or else return 0
    public event EventHandler SelectedChanged;

    #endregion
    #region Save
    private int SaveAssignment()
    {
        var paramsIn = new FilterParameterCollection();
        var paramOut = new FilterParameterCollection();
        //string cardType;
        //if (rdCardTypeAll.Checked)
        //    cardType = "All";
        //else if (rdCardTypeSettle.Checked)
        //    cardType = "Settled";
        //else
        //    cardType = "NonSettled";

        //paramsIn.Add("@DDSClient", SessionManager.CurrentUser.ASClient, DbType.Int32);
        //paramsIn.Add("@UserId", SessionManager.CurrentUser.UserID, DbType.String);
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32);
        //paramsIn.Add("@CardType", cardType, DbType.String);
        if (chkRiskScore.Checked && txtRCFrom.Text != "" && txtRCTo.Text != "")
        {
            paramsIn.Add("@RiskScoreFrom", Int32.Parse(txtRCFrom.Text), DbType.Int32);
            paramsIn.Add("@RiskScoreTo", Int32.Parse(txtRCTo.Text), DbType.Int32);
        }
        if (rdMatchYes.Checked)
        {
            paramsIn.Add("@IsParametersAnd", true, DbType.Boolean);
        }
        else
        {
            paramsIn.Add("@IsParametersAnd", false, DbType.Boolean);
        }
        paramsIn.Add("@Mode", (int)this.Mode, DbType.Int32);
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveRiskScoreFilter", paramsIn, out paramOut);

    }
    private int SaveParameters()
    {
        List<RiskParameter> paramList = GetAllParameterFromUI();
        if (paramList != null)
        {
            var paramStr = paramList.ToListXmlString();
            var filterParameters = new FilterParameterCollection();
            var paramOut = new FilterParameterCollection();
            filterParameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            filterParameters.Add("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32);
            filterParameters.Add("@ParameterList", paramStr, DbType.Xml);
            filterParameters.Add("@Mode", (int)this.Mode, DbType.Int32);

            return WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveParameterListByAssignment", filterParameters, out paramOut);
        }
        return 0;
    }
    #endregion
    #region constants

    //private const string SP_GETDEFAULTRISKSCORE = "spa_TMS_rm_CS_GetDefaultRiskScoreForAdhoc";
    private string NA = "";
    #endregion
    #region Method

    private void VisibleControls()
    {
        plhParameter.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;

        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            chkRiskScore.Enabled = false;
            rdMatchNo.Enabled = rdMatchYes.Enabled = false;
        }
        txtRCFrom.ReadOnly = txtRCTo.ReadOnly = FeatureMode == WebSiteEnums.FeatureMode.View;
    }


    protected override void OnPostBackActions(Enum type, object param)
    {
        if (Page.IsIntruderDetected) return;

        switch ((PostBackAction)type)
        {
            case PostBackAction.RefreshParameterList:
                {
                    uxParameterRepeater.DataSource = GetAssignmentParameters();
                    uxParameterRepeater.DataBind();
                    CountItemRepeater.Value = VeraCodeSolution.ValidateResponseData(uxParameterRepeater.Items.Count.ToString());
                }
                break;
            case PostBackAction.DeleteParameter:
                {
                    string ParameterKey = param.ToString();

                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(PrimaryID), DbType.Int32));
                    parameters.Add(new FilterParameter("@ParameterKey", ParameterKey, DbType.String));
                    parameters.Add(new FilterParameter("@FilterMode", (int)this.Mode, DbType.Int32));
                    WebServices.RiskServices.GetReports("spa_RM_MCF_DeleteParameterAssignment", parameters);
                    uxParameterRepeater.DataSource = GetAssignmentParameters();
                    uxParameterRepeater.DataBind();
                    CountItemRepeater.Value = VeraCodeSolution.ValidateResponseData(uxParameterRepeater.Items.Count.ToString());
                }
                break;
          
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;

        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindParameterList:
                {
                    source = new DataTable();
                    source = GetAssignmentParameters();
                    if (source != null || source.Rows.Count > 0)
                    {
                        uxParameterRepeater.DataSource = source;
                        uxParameterRepeater.DataBind();
                        CountItemRepeater.Value = VeraCodeSolution.ValidateResponseData(uxParameterRepeater.Items.Count.ToString());
                    }
                }
                break;
        }
    }





    protected void Page_Load()
    {
        NA = GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_NA").ToString();
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindParameterList);
            this.GetAssignmentParameterFilter();

        }
        if (chkRiskScore.Checked == true)
        {
            txtRCFrom.Enabled = true;
            txtRCTo.Enabled = true;
        }
        else
        {
            txtRCFrom.Enabled = false;
            txtRCTo.Enabled = false;
        }

        VisibleControls();
    }
    private void GetAssignmentParameterFilter()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", int.Parse(this.PrimaryID), DbType.Int32));
        parameters.Add(new FilterParameter("@FilterMode", (int)this.Mode, DbType.Int32));
        DataTable dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAssignmentForParameterList", parameters);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["RiskScoreFrom"].ToString() != "" || dt.Rows[0]["RiskScoreTo"].ToString() != "")
            {
                if (dt.Rows[0]["RiskScoreFrom"].ToString() != "-1" || dt.Rows[0]["RiskScoreTo"].ToString() != "-1")
                {
                    chkRiskScore.Checked = true;
                    txtRCFrom.Enabled = true;
                    txtRCTo.Enabled = true;
                    if (dt.Rows[0]["RiskScoreFrom"].ToString() != "-1")
                    {
                        txtRCFrom.Value = double.Parse(dt.Rows[0]["RiskScoreFrom"].ToString());
                    }
                    if (dt.Rows[0]["RiskScoreTo"].ToString() != "-1")
                    {
                        txtRCTo.Value = double.Parse(dt.Rows[0]["RiskScoreTo"].ToString());
                    }
                }
                else
                {
                    chkRiskScore.Checked = false;
                    txtRCFrom.Enabled = false;
                    txtRCTo.Enabled = false;
                }
            }
            else
            {
                chkRiskScore.Checked = false;
                txtRCFrom.Enabled = false;
                txtRCTo.Enabled = false;
            }
            if (dt.Rows[0]["IsParametersAnd"].ToString() != "")
            {
                if (dt.Rows[0]["IsParametersAnd"].ToString() == "False")
                {
                    rdMatchNo.Checked = true;
                    rdMatchYes.Checked = false;
                }
                else
                {
                    rdMatchYes.Checked = true;
                    rdMatchNo.Checked = false;
                }
            }
            else
            {
                rdMatchNo.Checked = true;
                rdMatchYes.Checked = false;
            }
            //switch (dt.Rows[0]["CardType"].ToString())
            //{
            //    case "Settled":
            //        rdCardTypeSettle.Checked = true;
            //        rdCardTypeAll.Checked = false;
            //        rdCardTypeNonsettle.Checked = false;
            //        break;
            //    case "NonSettled":
            //        rdCardTypeNonsettle.Checked = true;
            //        rdCardTypeSettle.Checked = false;
            //        rdCardTypeAll.Checked = false;
            //        break;
            //    default:
            //        rdCardTypeAll.Checked = true;
            //        rdCardTypeSettle.Checked = false;
            //        rdCardTypeNonsettle.Checked = false;
            //        break;

            //}
        }
    }
    public List<RiskParameter> GetAllParameterFromUI()
    {
        var list = new List<RiskParameter>();
        for (int i = 0; i < uxParameterRepeater.Items.Count; i++)
        {
            RiskParameter p = new RiskParameter();

            var ParameterThresholdType = ((HiddenField)uxParameterRepeater.Items[i].FindControl("ParameterThresholdType")).Value.ToString();
            string criteriaValueString = ((HiddenField)uxParameterRepeater.Items[i].FindControl("criteriaValueString")).Value.ToString();
            string criteriaValueNumeric = string.Empty; //((RadNumericTextBox)uxParameterRepeater.Items[i].FindControl("txtParameterValue")).Text.ToString();
            string criteriaValueNumericFrom = string.Empty;
            string criteriaValueNumericTo = string.Empty;
            var criteriaNumeric = ((RadNumericTextBox)uxParameterRepeater.Items[i].FindControl("txtParameterValue"));

            string criteriaValueThreshold = string.Empty;
            string criteriaValueThresholdLow = string.Empty;
            string criteriaValueThresholdHigh = string.Empty;
            string parameterPrecision = ((HiddenField)uxParameterRepeater.Items[i].FindControl("ParameterPrecision")).Value.ToString();
            string isIndicatorNagative = ((HiddenField)uxParameterRepeater.Items[i].FindControl("IsIndicatorNagative")).Value.ToString();
            string isThresholdNegative = ((HiddenField)uxParameterRepeater.Items[i].FindControl("IsThresholdNegative")).Value.ToString();

           

            switch (ParameterThresholdType)
            { 
                case "LowHigh":
                    criteriaValueThresholdLow = ((RadNumericTextBox)uxParameterRepeater.Items[i].FindControl("txtThresholdLow")).Text.ToString();
                    criteriaValueThresholdHigh = ((RadNumericTextBox)uxParameterRepeater.Items[i].FindControl("txtThresholdHigh")).Text.ToString();

                    if (!criteriaValueThresholdLow.IsNullOrEmpty())
                    {
                        double cT = double.Parse(criteriaValueThresholdLow);
                        if (isThresholdNegative != "" && isThresholdNegative == "True" && cT > 0)
                            p.ThresholdValue = -cT;
                        else
                            p.ThresholdValue = cT;
                    }
                    else
                    {
                        p.ThresholdValue = null;
                    }

                    if (!criteriaValueThresholdHigh.IsNullOrEmpty())
                    {
                        double cT = double.Parse(criteriaValueThresholdHigh);
                        if (isThresholdNegative != "" && isThresholdNegative == "True" && cT > 0)
                            p.ThresholdHigh = -cT;
                        else
                            p.ThresholdHigh = cT;
                    }
                    else
                    {
                        p.ThresholdHigh = null;
                    }

                    break;
                default:
                    criteriaValueThreshold = ((RadNumericTextBox)uxParameterRepeater.Items[i].FindControl("txtThreshold")).Text.ToString();
                    if (criteriaValueThreshold != "")
                    {
                        double cT = double.Parse(criteriaValueThreshold);
                        if (isThresholdNegative != "" && isThresholdNegative == "True" && cT > 0)
                            p.ThresholdValue = -cT;
                        else
                            p.ThresholdValue = cT;
                        //p.ThresholdValue = cT;
                    }
                    else
                    {
                        p.ThresholdValue = null;
                    }

                    break;
            }
          

          
            p.Key = criteriaValueString;
            if (criteriaNumeric.Visible)
            {
                criteriaValueNumeric = ((RadNumericTextBox)uxParameterRepeater.Items[i].FindControl("txtParameterValue")).Text.ToString();
                if (criteriaValueNumeric != "")
                {
                    double cN = double.Parse(criteriaValueNumeric);

                    if (isIndicatorNagative != "" && isIndicatorNagative == "True" && cN > 0)
                        p.IndicatorValue = -cN;
                    else
                        p.IndicatorValue = cN;
                    //p.IndicatorValue = cN;
                }
                else
                {
                    p.IndicatorValue = null;
                }
            }
            else
            {
                criteriaValueNumericFrom = ((RadNumericTextBox)uxParameterRepeater.Items[i].FindControl("txtFrom")).Text.ToString();
                criteriaValueNumericTo = ((RadNumericTextBox)uxParameterRepeater.Items[i].FindControl("txtTo")).Text.ToString();
                if (criteriaValueNumericFrom != "" && criteriaValueNumericTo != "")
                {
                    p.IndicatorValue = double.Parse(criteriaValueNumericFrom);
                    p.IndicatorHigh = double.Parse(criteriaValueNumericTo);
                }
                else
                {
                    p.IndicatorValue = null;
                    p.IndicatorHigh = null;
                }
            }

            p.PrecisionValue = int.Parse(parameterPrecision);
            list.Add(p);
        }
        return list;
    }

    private DataTable GetAssignmentParameters()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32));
        parameters.Add(new FilterParameter("@FilterMode", (int)this.Mode, DbType.Int32));
        parameters.AddLanguageID();
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetParametersByAssignment", parameters);
    }
    protected string GetValue(object sender)
    {
        if (sender != null)
        {
            string result = sender.ToString().ToLower();
            if (result.Equals("days"))
                return GetLocalResourceObject("Literal113Resource1.Text").ToString();
            return result;
        }
        return string.Empty;
    }


    protected void rptParameterRepeater_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete" && e.CommandArgument.ToString() != "")
        {
            SaveParameters();
            string ParameterKey = e.CommandArgument.ToString();
            OnPostBackActions(PostBackAction.DeleteParameter, ParameterKey);
        }
    }
    protected void rptParameterRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //================== process Indicator column==========================
        RadNumericTextBox txtParameterValue = e.Item.FindControl("txtParameterValue") as RadNumericTextBox;
        Literal lblType = e.Item.FindControl("lblParameterType") as Literal;
        RegularExpressionValidator RequiredFieldValidator1 = e.Item.FindControl("RequiredFieldValidator1") as RegularExpressionValidator;
        Literal lblIndicatorDollar = e.Item.FindControl("lblIndicatorDollar") as Literal;
        HtmlGenericControl spanErrMessage = e.Item.FindControl("spanErrMsg") as HtmlGenericControl;
        Literal lblIndicatorNA = e.Item.FindControl("lblIndicatorNA") as Literal;

        //Set format number is decimal for type=%

        if (lblType.Text.Equals("%"))
        {
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            int precision = 0;
            int.TryParse(dataRow["ParameterPrecision"].ToString(), out precision);
            txtParameterValue.NumberFormat.DecimalDigits = precision;
        }

        //=================== process Threshold column =======================
        RadNumericTextBox txtThreshold = e.Item.FindControl("txtThreshold") as RadNumericTextBox;
        RadNumericTextBox txtThresholdLow = e.Item.FindControl("txtThresholdLow") as RadNumericTextBox;
        RadNumericTextBox txtThresholdHigh = e.Item.FindControl("txtThresholdHigh") as RadNumericTextBox;
        RadNumericTextBox txtFrom = e.Item.FindControl("txtFrom") as RadNumericTextBox;
        RadNumericTextBox txtTo = e.Item.FindControl("txtTo") as RadNumericTextBox;

        DataRowView rowView = e.Item.DataItem as DataRowView;
        Literal lblDollar = e.Item.FindControl("lblDollar") as Literal;
        Literal lblpercent = e.Item.FindControl("lblpercent") as Literal;
        HtmlGenericControl spanErrMessageThs = e.Item.FindControl("spanErrMsgThs") as HtmlGenericControl;


        Literal lblDollarThresholdLow = e.Item.FindControl("lblDollarThresholdLow") as Literal;
        Literal lblPercentThresholdLow = e.Item.FindControl("lblPercentThresholdLow") as Literal;

        Literal lblDollarThresholdHigh = e.Item.FindControl("lblDollarThresholdHigh") as Literal;
        Literal lblPercentThresholdHigh = e.Item.FindControl("lblPercentThresholdHigh") as Literal;

        HtmlGenericControl spanErrMsgThresholdLow = e.Item.FindControl("spanErrMsgThresholdLow") as HtmlGenericControl;
        HtmlGenericControl spanErrMsgThresholdHigh = e.Item.FindControl("spanErrMsgThresholdHigh") as HtmlGenericControl;

        bool isView = FeatureMode == WebSiteEnums.FeatureMode.View;
        if (!isView)
        {
            txtParameterValue.Attributes["onkeypress"] = "return txtParameterValue_KeyPress(event);";
            txtParameterValue.Attributes["onblur"] = "return txtParameterValue_Blur(event);";
            txtParameterValue.Attributes["onfocus"] = "return txtParameterValue_Focus(event);";
            txtThreshold.Attributes["onkeypress"] = "return txtThreshold_KeyPress(event);";
            txtThreshold.Attributes["onblur"] = "return txtThreshold_Blur(event);";
            txtThreshold.Attributes["onfocus"] = "return txtThreshold_Focus(event);";
            txtThresholdLow.Attributes["onkeypress"] = "return txtThreshold_KeyPress(event);";
            txtThresholdLow.Attributes["onblur"] = "return txtThreshold_Blur(event);";
            txtThresholdLow.Attributes["onfocus"] = "return txtThreshold_Focus(event);";
            txtThresholdHigh.Attributes["onkeypress"] = "return txtThreshold_KeyPress(event);";
            txtThresholdHigh.Attributes["onblur"] = "return txtThreshold_Blur(event);";
            txtThresholdHigh.Attributes["onfocus"] = "return txtThreshold_Focus(event);";
            txtFrom.Attributes["onkeypress"] = "return txtParameterValue_KeyPress(event);";
            txtFrom.Attributes["onblur"] = "return txtParameterValue_Blur(event);";
            txtFrom.Attributes["onfocus"] = "return txtParameterValue_Focus(event);";
            txtTo.Attributes["onkeypress"] = "return txtParameterValue_KeyPress(event);";
            txtTo.Attributes["onblur"] = "return txtParameterValue_Blur(event);";
            txtParameterValue.Attributes["onfocus"] = "return txtParameterValue_Focus(event);";
        }
        txtParameterValue.ReadOnly = txtThreshold.ReadOnly = txtThresholdLow.ReadOnly = txtThresholdHigh.ReadOnly = isView;

        switch (e.Item.ItemType)
        {
            case ListItemType.Item:
            case ListItemType.AlternatingItem:
                {
                    DataRowView row = e.Item.DataItem as DataRowView;

                    var ParameterThresholdType = row["ParameterThresholdType"].ToString();
                    string modalType = row["FilterTypeModal"].ToString();

                    string isThresholdNegative = row["IsThresholdNegative"].ToString();
                    string isIndicatorNagative = row["IsIndicatorNegative"].ToString();

                    var ParameterValue = row["ParameterValue"].ToString();
                    var ParameterValueHigh = row["ParameterValueHigh"].ToString();

                    var ParameterThreshold = row["ParameterThreshold"].ToString();
                    var ParameterThresholdLow = row["ParameterThreshold"].ToString();
                    var ParameterThresholdHigh = row["ParameterThresholdHigh"].ToString();

                    if (row["ParameterValue"] != DBNull.Value && !row["ParameterValue"].ToString().IsNullOrEmpty()
                        && row["ParameterValueHigh"] != DBNull.Value && !row["ParameterValueHigh"].ToString().IsNullOrEmpty())
                    {
                        ((PlaceHolder)e.Item.FindControl("uxIndicatorNomal")).Visible = false;
                        ((PlaceHolder)e.Item.FindControl("uxIndicatorHigh")).Visible = true;

                        txtFrom.Text = VeraCodeSolution.DoVeraCode(Convert.ToInt32(GetParameterNumber(ParameterValue)).ToString());
                        txtTo.Text = VeraCodeSolution.DoVeraCode(Convert.ToInt32(GetParameterNumber(ParameterValueHigh)).ToString());
                    }
                    ProcessIndicatorColumn(txtParameterValue, lblType, lblIndicatorDollar, lblIndicatorNA, spanErrMessage, e);
                    if (isIndicatorNagative != "" && isIndicatorNagative == "True")
                    {
                        FormatNegativeSignForIndicator(txtParameterValue, ParameterValue);
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
                                ProcessThresholdColumn(txtThresholdHigh, lblDollarThresholdHigh, lblPercentThresholdHigh, spanErrMsgThresholdHigh, e);

                                break;
                            default:
                                ((PlaceHolder)e.Item.FindControl("uxThresholdNormal")).Visible = true;
                                ((PlaceHolder)e.Item.FindControl("uxThresholdLowHigh")).Visible = false;

                                ProcessThresholdColumn(txtThreshold, lblDollar, lblpercent, spanErrMessageThs, e);
                                break;
                        }

                        if (isThresholdNegative != "" && isThresholdNegative == "True")
                        {
                            switch (ParameterThresholdType)
                            {
                                case "LowHigh":
                                    FormatNegativeSignForThreshold(txtThresholdLow, ParameterThresholdLow);
                                    FormatNegativeSignForThreshold(txtThresholdHigh, ParameterThresholdHigh);
                                    break;
                                default:
                                    FormatNegativeSignForThreshold(txtThreshold, ParameterThreshold);
                                    break;
                            }

                        }
                    }

                    if (modalType != "")
                    {

                        HtmlControl paramAssignment = e.Item.FindControl("paramAssignment") as HtmlControl;
                        paramAssignment.Attributes["class"] = "risklightgrayrow";
                        ((PlaceHolder)e.Item.FindControl("uxSpecialPanel")).Visible = true;

                        Literal lblParamfilter = e.Item.FindControl("lblParamfilter") as Literal;
                        HiddenField typeModal = e.Item.FindControl("modalType") as HiddenField;
                        typeModal.Value = VeraCodeSolution.ValidateResponseData(modalType.ToString());
                        string queryString = Page.BuildSecureQueryString(string.Format("PrimaryID=" + PrimaryID + "&Mode=" + (Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment) + "&ParamID=" + row["ParameterKey"].ToString()));
                        AS.Controls.Global.Container asContainer = (AS.Controls.Global.Container)e.Item.FindControl("asContainer");

                        string ht = string.Empty;
                        if (isView)
                        {
                            ht = "<div class=\"row\"><div class=\"col-xs-10\">{0}</div><div class=\"col-xs-2 text-right\"><span class=\"display-none\" onclick=\"parameter_ShowFilter({1})\">" + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_Edit").ToString() + "</span></div></div>";
                        }
                        else
                        {
                            ht = "<div class=\"row\"><div class=\"col-xs-10\">{0}</div><div class=\"col-xs-2 text-right\"><span class=\"text-button dark-blue pointer\" onclick=\"parameter_ShowFilter({1})\">" + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_Edit").ToString() + "</span></div></div>";
                        }

                        string paramId = row["ParameterKey"].ToString();
                        switch (modalType)
                        {
                            case "rm_MCF_ParameterFilter_TransactionCodeModal.aspx":
                                {
                                    Dictionary<string, string> data = UserControls_rm_MCF_ParameterFilter_TransactionCode.GetSelectedValuesAsString(Mode, PrimaryID, row["ParameterKey"].ToString(), FilterID);
                                    lblParamfilter.Text = data["Label"];
                                    ht = string.Format(ht, "Auth Transaction Code", "'rm_MCF_ParameterFilter_TransactionCodeModal.aspx?{0}','{1}','{2}', 850, 725");
                                    asContainer.HeaderText = VeraCodeSolution.DoVeraCode(string.Format(ht, queryString, e.Item.ItemIndex, paramId));
                                }
                                break;
                        }

                    }
                }

                break;
        }

    }

    private static void FormatNegativeSignForIndicator(RadNumericTextBox txtParameterValue, string ParameterValue)
    {
        if (ParameterValue != "")
        {
            if (double.Parse(ParameterValue) < 0)
            {
                var indicator = -double.Parse(ParameterValue);
                txtParameterValue.Text = VeraCodeSolution.DoVeraCode(indicator.ToString());
            }
        }
        txtParameterValue.NumberFormat.PositivePattern = "(n)";
        txtParameterValue.NumberFormat.NegativePattern = "(n)";
        txtParameterValue.ForeColor = Color.Red;
    }

    private static void FormatNegativeSignForThreshold(RadNumericTextBox txtThreshold, string ParameterThreshold)
    {
        if (ParameterThreshold != "")
        {
            if (double.Parse(ParameterThreshold) < 0)
            {
                var threshold = -double.Parse(ParameterThreshold);
                txtThreshold.Text = VeraCodeSolution.DoVeraCode(threshold.ToString());
            }
        }
        txtThreshold.NumberFormat.PositivePattern = "(n)";
        txtThreshold.NumberFormat.NegativePattern = "(n)";
        txtThreshold.ForeColor = Color.Red;
    }


    protected void uxClose_Click(object sender, EventArgs e)
    {
        int index = int.Parse(uxCurrentParam.Value);
        Literal lblParamfilter = uxParameterRepeater.Items[index].FindControl("lblParamfilter") as Literal;
        string paramkey = uxPramKey.Value;
        HiddenField modalType = uxParameterRepeater.Items[index].FindControl("modalType") as HiddenField;
        switch (modalType.Value)
        {
            case "rm_MCF_ParameterFilter_TransactionCodeModal.aspx":
                Dictionary<string, string> data = UserControls_rm_MCF_ParameterFilter_TransactionCode.GetSelectedValuesAsString(Mode, PrimaryID, paramkey, FilterID);
                lblParamfilter.Text = data["Label"];
                break;
        }
    }
    protected void btnRefreshParamList_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.RefreshParameterList); 
    }

    protected void btnSaveWorkingParameters_Click(object sender, EventArgs e)
    {
        SaveParameters();
    }

    private void ProcessIndicatorColumn(RadNumericTextBox txtParameterValue, Literal lblType, Literal lblIndicatorDollar, Literal lblIndicatorNA, HtmlGenericControl errControl, RepeaterItemEventArgs e)
    {
        DataRowView dataRow = e.Item.DataItem as DataRowView;

        var type = lblType.Text.Replace("&nbsp", string.Empty).Trim();
        if (txtParameterValue.Text.Trim() != string.Empty)
        {
            string m = dataRow["IndicatorMax"].ToString();
            //add to check indicator max, min, precision, error controlID value
            if (m.Replace("&nbsp", string.Empty).Trim() != string.Empty)
                txtParameterValue.Attributes["MaxValue"] = dataRow["IndicatorMax"].ToString();
            txtParameterValue.Attributes["MinValue"] = dataRow["IndicatorMin"].ToString();
            txtParameterValue.Attributes["Precision"] = dataRow["ParameterPrecision"].ToString();
            txtParameterValue.Attributes["ErrControlID"] = errControl.ClientID;
            if (type == "$")
            {
                lblType.Visible = false;
                lblIndicatorDollar.Visible = true;
                lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(type);
                txtParameterValue.Text = VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtParameterValue.Text));
            }
            else
            {
                lblType.Visible = true;
                //lblIndicatorDollar.Visible = false;

                if (type == "#" || type == "%")
                    txtParameterValue.Text = VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtParameterValue.Text));
                else if (type == "days")
                    txtParameterValue.Text = VeraCodeSolution.DoVeraCode(this.GetIntegerNumber(txtParameterValue.Text));
                else if (type.Length == 0)
                {
                    lblIndicatorDollar.Visible = false;
                    txtParameterValue.Visible = false;

                    //lblType.Visible = true;
                    //lblType.Text = "<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;N/A</b>";
                    lblIndicatorNA.Visible = true;
                    lblIndicatorNA.Text = "<b>" + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_NA").ToString() + "</b>";
                }
            }
        }
        if (lblType.Text.Trim() == string.Empty)
        {
            lblIndicatorDollar.Visible = false;
            txtParameterValue.Visible = false;

            lblIndicatorNA.Visible = true;
            lblIndicatorNA.Text = "<b>" + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_NA").ToString() + "</b>";
        }
    }
    private void ProcessThresholdColumn(RadNumericTextBox txtThreshold, Literal lblDollar, Literal lblpercent, HtmlGenericControl errControl, RepeaterItemEventArgs e)
    {
        var type = lblpercent.Text.Replace("&nbsp", string.Empty).Trim();
        if (txtThreshold.Text.Trim() != string.Empty)
        {
            if (type == "$")
            {
                lblpercent.Visible = false;
                lblDollar.Visible = true;
                lblDollar.Text = VeraCodeSolution.DoVeraCode(type);
                txtThreshold.Text = VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtThreshold.Text));
            }
            else
            {
                lblpercent.Visible = true;
                //lblIndicatorDollar.Visible = false;

                if (type == "#" || type == "%")
                    txtThreshold.Text = VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtThreshold.Text));
                else if (type == "days")
                    txtThreshold.Text = VeraCodeSolution.DoVeraCode(this.GetIntegerNumber(txtThreshold.Text));
                else if (type == "none")
                {
                    txtThreshold.Text = VeraCodeSolution.DoVeraCode(this.GetIntegerNumber(txtThreshold.Text));
                    lblpercent.Text = string.Empty;
                }
                else if (type.Length == 0)
                {
                    lblDollar.Visible = true;
                    txtThreshold.Visible = false;

                    lblpercent.Visible = true;
                    lblpercent.Text = "<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_NA").ToString() + "</b>";

                }
            }
        }

        if (txtThreshold.Text == "")
        {
            DataRowView row = e.Item.DataItem as DataRowView;
            string Threshold = row["ThresholdMin"].ToString(); //get value here 
            if (Threshold == "")
            {
                txtThreshold.Visible = false;
                //lblDollar.Visible = true;
                //lblDollar.Text = "<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;N/A</b>";
                lblpercent.Visible = true;
                lblpercent.Text = "<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_NA").ToString() + "</b>";
                return;
            }

        }
        DataRowView dataRow = e.Item.DataItem as DataRowView;
        //12-26-2011:Fix bug for case P68
        //if (dataRow["ParameterKey"].ToString() == "P68")
        //{
        //    lblDollar.Text = "&nbsp;";
        //    lblpercent.Text = "%";
        //}
        //else
        //{
        //    lblDollar.Text = "$";
        //    lblpercent.Visible = false;
        //}

        //lblDollar.Text = "$";
        //lblpercent.Visible = false;
        string m = dataRow["ThresholdMax"].ToString();
        if (m.Replace("&nbsp", string.Empty).Trim() != string.Empty)
            txtThreshold.Attributes["MaxValue"] = VeraCodeSolution.DoVeraCode(dataRow["ThresholdMax"].ToString());
        txtThreshold.Attributes["MinValue"] = VeraCodeSolution.DoVeraCode(dataRow["ThresholdMin"].ToString());
        txtThreshold.Attributes["ErrControlID"] = VeraCodeSolution.DoVeraCode(errControl.ClientID);
    }



    private string GetIntegerNumber(string intStr)
    {
        if (intStr.Contains("."))
            return intStr.Substring(0, intStr.IndexOf('.'));

        return intStr;
    }
    protected double? GetParameterNumber(string number)
    {
        if (number == "")
            return null;
        else if (number == "0dds0")
            return double.NaN;

        return double.Parse(number);
    }
    private string GetDouble4Precision(string doubleStr)
    {
        if (doubleStr.Contains("."))
        {
            if (!doubleStr.EndsWith("0000"))
                return double.Parse(doubleStr).ToString("#,#0.0000");
        }

        return double.Parse(doubleStr).ToString("#,#0");
    }
    #endregion
}
