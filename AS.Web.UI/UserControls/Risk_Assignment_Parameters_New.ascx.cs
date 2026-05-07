using AS.Common;
using AS.Common.DBManager;
//using AS.Controls.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Controls.Pages;

public partial class UserControls_Risk_Assignment_Parameters_New : GlobalUserControl, IRiskParamFilter
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

    public string MatchAllMode
    {
        get
        {
            return rdMatchAll.Checked ? "hide" : string.Empty;
        }
        set { }
    }

    private DataTable source = null;
    private DataTable groups = null;
    private List<ParameterFE> Parameters { get; set; }

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
        return 1;
    }//return error code, if needed or else return 0
    public event EventHandler SelectedChanged;
    private string NA = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        NA = GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_NA").ToString();
        if (!IsPostBack)
        {
            uxAssignmentID.Value = WebServices.SecurityServices.EncryptText(this.PrimaryID);
            uxMode.Value = ((int)this.Mode).ToString();
            uxFilterID.Value = this.FilterID;
            this.GetAssignmentParameterFilter();
            MatchAllMode = rdMatchAll.Checked ? "hide" : string.Empty;
            OnDataBindControls(DataBindAction.BindParameterList);
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

    #region Save
    private int SaveAssignment()
    {
        var paramsIn = new FilterParameterCollection();
        var paramOut = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32);
        if (chkRiskScore.Checked && txtRCFrom.Text != "" && txtRCTo.Text != "")
        {
            paramsIn.Add("@RiskScoreFrom", Int32.Parse(txtRCFrom.Text), DbType.Int32);
            paramsIn.Add("@RiskScoreTo", Int32.Parse(txtRCTo.Text), DbType.Int32);
        }
        paramsIn.Add("@IsParametersAnd", rdMatchAll.Checked, DbType.Boolean);
        if (rdMatchAll.Checked)
        {
            paramsIn.Add("@IsParametersAnd", true, DbType.Boolean);
        }
        else
        {
            paramsIn.Add("@IsParametersAnd", false, DbType.Boolean);
        }
        paramsIn.Add("@Mode", (int)this.Mode, DbType.Int32);
        return WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_SaveRiskScoreFilter", paramsIn, out paramOut);

    }
    
    #endregion
    private void VisibleControls()
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            chkRiskScore.Enabled = false;
            rdMatchAll.Enabled = rdMatchSpecific.Enabled = false;
        }
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            txtRCFrom.Enabled = txtRCTo.Enabled = false;
            txtRCFrom.CssClass += " form-control-disabled";
            txtRCTo.CssClass += " form-control-disabled";
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
                    groups = new DataTable();
                    groups = GetListGroup();
                    //Bind group parameter
                    if (groups.HasData())
                    {
                        uxGroupParameterRepeater.DataSource = GetListGroup();
                        uxGroupParameterRepeater.DataBind();
                    }
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "showIcon", "showFinishButton()", true);
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object param)
    {
        if (Page.IsIntruderDetected) return;

        switch ((PostBackAction)type)
        {
            case PostBackAction.RefreshParameterList:
                {
                    groups = new DataTable();
                    groups = GetListGroup();
                    source = new DataTable();
                    source = GetAssignmentParameters();
                    uxGroupParameterRepeater.DataSource = GetListGroup();
                    uxGroupParameterRepeater.DataBind();
                    CountItemRepeater.Value = VeraCodeSolution.ValidateResponseData(uxGroupParameterRepeater.Items.Count.ToString());
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "showFinish", "showFinishButton()", true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "showGroup", "showGroupButton()", true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "matchChange", string.Format("onMatchChange('{0}')", rdMatchAll.Checked), true);
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
                    WebServices.RiskServices.GetReports("spa_rm_cs_DeleteParameterAssignment", parameters);
                    CountItemRepeater.Value = VeraCodeSolution.ValidateResponseData(uxGroupParameterRepeater.Items.Count.ToString());
                }
                break;

        }
    }

    protected void uxClose_Click(object sender, EventArgs e)
    {
        switch (uxModalType.Value)
        {
            case "rm_ParameterFilter_TransactionCodeModal.aspx":
                string codes = VeraCodeSolution.ValidateResponseData(UserControls_Risk_ParameterFilter_TransactionCode.GetSelectedValuesAsString(Mode, PrimaryID, uxPramKey.Value, FilterID));
                break;
        }
    }

    protected void btnRefreshParamList_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.RefreshParameterList);
    }

    public void RefreshParamList()
    {
        OnPostBackActions(PostBackAction.RefreshParameterList);
    }

    public string hideUnGroupBntClass = string.Empty;
    protected void uxGroupParameterRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = e.Item.DataItem as DataRowView;
        HtmlGenericControl divGroup = e.Item.FindControl("divGroup") as HtmlGenericControl;

        DataTable paramInGroup = new DataTable();
        paramInGroup = source.Select("GroupID = " + (string.IsNullOrEmpty( row["GroupID"].ToString()) ? "''" : row["GroupID"])).CopyToDataTable();
        if (paramInGroup.HasData())
        {
            Repeater uxParameterInGroupRepeater = (Repeater)e.Item.FindControl("uxParameterInGroupRepeater");
            hideUnGroupBntClass = paramInGroup.Rows.Count == 1 ? "hide" : string.Empty;
            uxParameterInGroupRepeater.DataSource = paramInGroup;
            uxParameterInGroupRepeater.DataBind();
        }
    }

    protected void uxParameterInGroupRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //================== process Indicator column==========================
        CheckBox ckGroup = e.Item.FindControl("chkGroup") as CheckBox;

        RadNumericTextBox txtParameterValue = e.Item.FindControl("txtParameterValue") as RadNumericTextBox;
        Literal lblType = e.Item.FindControl("lblParameterType") as Literal;
        System.Web.UI.WebControls.RegularExpressionValidator RequiredFieldValidator1 = e.Item.FindControl("RequiredFieldValidator1") as System.Web.UI.WebControls.RegularExpressionValidator;
        Literal lblIndicatorDollar = e.Item.FindControl("lblIndicatorDollar") as Literal;
        HtmlGenericControl spanErrMessage = e.Item.FindControl("spanErrMsg") as HtmlGenericControl;
        Literal lblIndicatorNA = e.Item.FindControl("lblIndicatorNA") as Literal;

        //Set format number is decimal for type=%
        DataRowView dataRow = e.Item.DataItem as DataRowView;
        if (lblType.Text.Equals("%"))
        {
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

        //Label
        Literal lbParameterValue = e.Item.FindControl("lbParameterValue") as Literal;
        Literal lbFrom = e.Item.FindControl("lbFrom") as Literal;
        Literal lbTo = e.Item.FindControl("lbTo") as Literal;
        Literal lbThreshold = e.Item.FindControl("lbThreshold") as Literal;
        Literal lbThresholdLow = e.Item.FindControl("lbThresholdLow") as Literal;
        Literal lbThresholdHigh = e.Item.FindControl("lbThresholdHigh") as Literal;

        //Hidden field
        HiddenField uxCriteriaValueString = e.Item.FindControl("uxCriteriaValueString") as HiddenField;
        HiddenField uxParameterPrecision = e.Item.FindControl("uxParameterPrecision") as HiddenField;
        HiddenField uxIsThresholdNegative = e.Item.FindControl("uxIsThresholdNegative") as HiddenField;
        HiddenField uxIsIndicatorNagative = e.Item.FindControl("uxIsIndicatorNagative") as HiddenField;
        HiddenField uxParameterThresholdType = e.Item.FindControl("uxParameterThresholdType") as HiddenField;
        HtmlGenericControl uxParameterValue = e.Item.FindControl("ucParameterValue") as HtmlGenericControl;
        HtmlGenericControl ucFrom = e.Item.FindControl("ucFrom") as HtmlGenericControl;
        HtmlGenericControl ucTo = e.Item.FindControl("ucTo") as HtmlGenericControl;
        HtmlGenericControl ucThreshold = e.Item.FindControl("ucThreshold") as HtmlGenericControl;
        PlaceHolder uxPanelMatchAll = e.Item.FindControl("uxPanelMatchAll") as PlaceHolder;
        HtmlGenericControl uxStandAloneIcon = e.Item.FindControl("uxStandAloneIcon") as HtmlGenericControl;
        HtmlGenericControl ucThresholdLow = e.Item.FindControl("ucThresholdLow") as HtmlGenericControl;
        HtmlGenericControl ucThresholdHigh = e.Item.FindControl("ucThresholdHigh") as HtmlGenericControl;
        
        uxPanelMatchAll.Visible = !rdMatchAll.Checked;
        uxCriteriaValueString.Visible = uxParameterPrecision.Visible = uxIsThresholdNegative.Visible = uxIsIndicatorNagative.Visible = uxParameterThresholdType.Visible
            = txtParameterValue.Visible = txtFrom.Visible = txtTo.Visible = txtThreshold.Visible = txtThresholdLow.Visible = txtThresholdHigh.Visible
            = dataRow["IsSourceParam"].ToBoolean();

        uxParameterValue.Attributes["class"] = ucFrom.Attributes["class"] = ucTo.Attributes["class"] = ucThreshold.Attributes["class"] = ucThresholdLow.Attributes["class"] = ucThresholdHigh.Attributes["class"]
            = dataRow["IsSourceParam"].ToBoolean() ? "non-edit hide" : "non-edit";
        uxStandAloneIcon.Attributes["class"] = "icon-warning hide";
        uxStandAloneIcon.Attributes["title"] = GetLocalResourceObject("Duplicate_StandAlone_Msg").ToString();

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
            txtThresholdLow.Attributes["onblur"] = "return txtThresholdLow_Blur(event);";
            txtThresholdLow.Attributes["onfocus"] = "return txtThreshold_Focus(event);";
            txtThresholdHigh.Attributes["onkeypress"] = "return txtThreshold_KeyPress(event);";
            txtThresholdHigh.Attributes["onblur"] = "return txtThresholdHigh_Blur(event);";
            txtThresholdHigh.Attributes["onfocus"] = "return txtThreshold_Focus(event);";
            txtFrom.Attributes["onkeypress"] = "return txtParameterValue_KeyPress(event);";
            txtFrom.Attributes["onblur"] = "return txtFrom_Blur(event);";
            txtFrom.Attributes["onfocus"] = "return txtParameterValue_Focus(event);";
            txtTo.Attributes["onkeypress"] = "return txtParameterValue_KeyPress(event);";
            txtTo.Attributes["onblur"] = "return txtTo_Blur(event);";
            txtParameterValue.Attributes["onfocus"] = "return txtParameterValue_Focus(event);";
        }
        txtParameterValue.ReadOnly = txtThreshold.ReadOnly = txtThresholdLow.ReadOnly = txtThresholdHigh.ReadOnly = isView;

        switch (e.Item.ItemType)
        {
            case System.Web.UI.WebControls.ListItemType.Item:
            case System.Web.UI.WebControls.ListItemType.AlternatingItem:
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
                        ((PlaceHolder)e.Item.FindControl("uxSpecialPanel")).Visible = true;

                        Literal lblParamfilter = e.Item.FindControl("lblParamfilter") as Literal;
                        uxModalType.Value = VeraCodeSolution.ValidateResponseData(modalType.ToString());
                        string queryString = Page.BuildSecureQueryString(string.Format("PrimaryID=" + PrimaryID + "&Mode=" + (Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment) + "&ParamID=" + row["ParameterKey"].ToString()));
                        AS.Controls.Global.Container asContainer = (AS.Controls.Global.Container)e.Item.FindControl("asContainer");

                        string ht = string.Empty;
                        if (isView)
                        {
                            ht = "<div class=\"row\"><div class=\"col-xs-10\"><strong>{0}</strong><span id='transactionCode'>: {2}</span></div><div class=\"col-xs-2 text-right\"><a class=\"display-none\" onclick=\"parameter_ShowFilter({1})\">" + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_Edit").ToString() + "</a></div></div>";
                        }
                        else
                        {
                            if (dataRow["IsSourceParam"].ToBoolean())
                                ht = "<div class=\"row\"><div class=\"col-xs-10\"><strong>{0}</strong><span id='transactionCode'>{2}</span></div><div class=\"col-xs-2 text-right\"><a id=\"edit-transactioncode\" class=\"text-button dark-blue pointer\" onclick=\"parameter_ShowFilter({1})\">" + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_Edit").ToString() + "</a></div></div>";
                            else
                                ht = "<div class=\"row\"><div class=\"col-xs-10\"><strong>{0}</strong><span id='transactionCode'>{2}</span></div><div class=\"col-xs-2 text-right\"><a id=\"edit-transactioncode\" class=\"text-button dark-blue pointer hide\" onclick=\"parameter_ShowFilter({1})\">" + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_Edit").ToString() + "</a></div></div>";
                        }

                        string paramId = row["ParameterKey"].ToString();
                        switch (modalType)
                        {
                            case "rm_ParameterFilter_TransactionCodeModal.aspx":
                                {
                                    string code = VeraCodeSolution.ValidateResponseData(UserControls_Risk_ParameterFilter_TransactionCode.GetSelectedValuesAsString(Mode, PrimaryID, row["ParameterKey"].ToString(), FilterID));
                                    string transactionCode = string.Format(": {0}", !code.IsNullOrEmpty()? code : NA);
                                    ht = string.Format(ht, "Auth Transaction Code", "'rm_ParameterFilter_TransactionCodeModal.aspx?{0}','{1}','{2}', 850, 725", transactionCode);
                                    asContainer.HeaderText = VeraCodeSolution.DoVeraCode(string.Format(ht, queryString, e.Item.ItemIndex, paramId));
                                }
                                break;
                        }

                    }
                }

                break;
        }
    }

    protected void rdMatch_CheckedChanged(object sender, EventArgs e)
    {
        DeleteParameterOnStagging();
        OnPostBackActions(PostBackAction.RefreshParameterList);
        MatchAllMode = rdMatchAll.Checked ? "hide" : string.Empty;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "matchChange", string.Format("onMatchChange('{0}')", rdMatchAll.Checked), true);
    }

    #region Private method
    private void DeleteParameterOnStagging()
    {
        var filterParameters = new FilterParameterCollection();
        var paramOut = new FilterParameterCollection();
        filterParameters.Add("@ASClient", SessionManager.CurrentClient, DbType.Int32);
        filterParameters.Add("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString);
        filterParameters.Add("@UserMode", SessionManager.CurrentUser.UserSecRole, DbType.AnsiString);
        filterParameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        filterParameters.Add("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32);
        filterParameters.Add("@FilterMode", (int)this.Mode, DbType.Int32);

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_DeleteAssignmentCriteriasStaging", filterParameters, out paramOut);
    }

    private void GetAssignmentParameterFilter()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", int.Parse(this.PrimaryID), DbType.Int32));
        parameters.Add(new FilterParameter("@FilterMode", (int)this.Mode, DbType.Int32));
        DataTable dt = WebServices.RiskServices.GetReports("spa_rm_cs_GetAssignmentForParameterList", parameters);
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
                    txtRCFrom.CssClass += " form-control-disabled";
                    txtRCTo.CssClass += " form-control-disabled";
                }
            }
            else
            {
                chkRiskScore.Checked = false;
                txtRCFrom.Enabled = false;
                txtRCTo.Enabled = false;
                txtRCFrom.CssClass += " form-control-disabled";
                txtRCTo.CssClass += " form-control-disabled";
            }

            if (dt.Rows[0]["IsParametersAnd"].ToString() != "")
            {
                if (dt.Rows[0]["IsParametersAnd"].ToString() == "False")
                {
                    rdMatchSpecific.Checked = true;
                    rdMatchAll.Checked = false;
                }
                else
                {
                    rdMatchAll.Checked = true;
                    rdMatchSpecific.Checked = false;
                }
            }
            else
            {
                rdMatchSpecific.Checked = true;
                rdMatchAll.Checked = false;
            }
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
            if (type == SessionManager.CurrencySymbol)
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
            if (type == SessionManager.CurrencySymbol)
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
                lblpercent.Visible = true;
                lblpercent.Text = "<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + GetLocalResourceObject("Risk_Assignment_Parameters_ascx_cs_NA").ToString() + "</b>";
                return;
            }

        }
        DataRowView dataRow = e.Item.DataItem as DataRowView;
        string m = dataRow["ThresholdMax"].ToString();
        if (m.Replace("&nbsp", string.Empty).Trim() != string.Empty)
            txtThreshold.Attributes["MaxValue"] = VeraCodeSolution.DoVeraCode(dataRow["ThresholdMax"].ToString());
        txtThreshold.Attributes["MinValue"] = VeraCodeSolution.DoVeraCode(dataRow["ThresholdMin"].ToString());
        txtThreshold.Attributes["ErrControlID"] = VeraCodeSolution.DoVeraCode(errControl.ClientID);
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

    private DataTable GetAssignmentParameters()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32));
        parameters.Add(new FilterParameter("@FilterMode", (int)this.Mode, DbType.Int32));
        parameters.AddLanguageID();
        DataTable dt = WebServices.RiskServices.GetReports("spa_rm_cs_GetParametersByAssignment", parameters);
        return dt;
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

    protected string GetParameterNumberForLabel(string number, string paramPrecision = "0")
    {
        int precision = 0;
        int.TryParse(paramPrecision, out precision);
        if (number == "")
            return null;
        else if (number == "0dds0")
            return double.NaN.ToString();
        else if (double.Parse(number) < 0)
            return string.Format("<span style='color:red'>({0})</span>", string.Format("{0:n0}", double.Parse(number) * -1));
        else if (number.Contains('.') && precision > 0)
        {
            if (precision == 2)
                return double.Parse(number).ToString("#,#0.00");
            if (precision == 4)
                return double.Parse(number).ToString("#,#0.0000");
        }
        return string.Format("{0:n0}", double.Parse(number));
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

    private DataTable GetListGroup()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.String));
        parameters.Add(new FilterParameter("@UserMode", SessionManager.CurrentUser.UserSecRole, DbType.String));
        parameters.Add(new FilterParameter("@Mode", (int)this.Mode, DbType.Int32));
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(PrimaryID), DbType.Int32));
        //parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
        DataTable dt = WebServices.RiskServices.GetReports("spa_rm_cs_GetListGroupParameterByAssignment", parameters);
        return dt;
    }
    #endregion

}