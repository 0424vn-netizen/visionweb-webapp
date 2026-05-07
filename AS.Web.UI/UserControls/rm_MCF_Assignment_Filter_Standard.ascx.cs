using AS.Common.DBManager;
using AS.Controls.Global;
using System;
using System.Collections.Generic;
using System.Data;

public partial class UserControls_rm_MCF_Assignment_Filter_Standard : GlobalUserControl, IRiskParamFilter
{

    #region Enums

    enum DataBindAction
    {
        LoadTransactionalFilter,
        FICOScore,
        RiskRating
    }
    enum PostBackAction { }

    #endregion

    public WebSiteEnums.FeatureMode FeatureMode { get; set; }

    public bool DisplayControl { get; set; }

    private string RiskRating = "RiskRating";

    protected int MaxLengthViewMore
    {
        get
        {
            return Convert.ToInt32(WebSiteSettings.RiskViewMore);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        VisibleControls();
        InitValidation();

        if (!IsPostBack)
        {
            if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.RiskRatingStandard))
            {
                SetRiskRatingInfo();
            }
        }
    }

    private void SetRiskRatingInfo()
    {
        Dictionary<string, string> result = UserControls_rm_MCF_FilterRiskRating.GetSelectedValuesAsString(Mode, PrimaryID, null, null, RiskRating);

        string data = result["Label"];
        lblRiskRating.Attributes.Add("tracking-value", result["Tracking"]);
        var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);

        if ((temp.Length > 1 ? temp[1].Trim() : data).Length > MaxLengthViewMore)
        {
            lblRiskRating.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lblRiskRating.Attributes.Remove("tracking-more");
        }
        lblRiskRating.Text = BuildLabel(data, ViewMoreModalType.LoadRiskLevel);
    }

    private string BuildLabel(string data, ViewMoreModalType modalType)
    {
        string localQueryString = string.Empty;
        var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
        if ((temp.Length > 1 ? temp[1].Trim() : data).Length > MaxLengthViewMore)
        {
            localQueryString = BuildQueryStringForViewMoreModal(modalType);
            if (temp.Length > 1)
            {
                data = temp[0].Trim() + "<br />" + temp[1].Trim().Substring(0, MaxLengthViewMore)
                    + string.Format(
                        "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?{0}', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_Filters_ascx_cs_ViewMore").ToString() + "</a>",
                        localQueryString);
            }
            else
            {
                data = data.Substring(0, MaxLengthViewMore)
                + string.Format(
                    "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?{0}', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_Filters_ascx_cs_ViewMore").ToString() + "</a>",
                    localQueryString);
            }
        }
        return data;
    }

    private string BuildQueryStringForViewMoreModal(ViewMoreModalType modalType)
    {
        return Page.BuildSecureQueryString(string.Format(
                "primaryid={0}&mode={1}&typemodal={2}",
                PrimaryID,
                (int)WebSiteEnums.ParamFilterMode.Assignment,
                ((int)modalType).ToString())
            );
    }

    private void VisibleControls()
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            cbDaystoFundStandard.Enabled = false;

            cbxDaystoFundStandard.OnClientDropDownOpening = "CancelDropDown";
            txtDTFBetweenFromStandard.ReadOnly = txtDTFBetweenToStandard.ReadOnly = txtDTFGtStandard.ReadOnly = txtDTFLtStandard.ReadOnly = FeatureMode == WebSiteEnums.FeatureMode.View;
            plhCreditScoreStandard.Visible = uxChkIsFromStandard.Enabled = uxChkIsToStandard.Enabled = FeatureMode == WebSiteEnums.FeatureMode.Edit;
            pnlRiskRatingStandardEdit.Visible = false;
        }

        pnlDaystoFundStandard.Visible = false;
        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.DaystoFundStandard))
        {
            pnlDaystoFundStandard.Visible = true;
        }

        pnCreditScoreStandard.Visible = false;
        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.FICOScoreStandard))
        {
            pnCreditScoreStandard.Visible = true;
        }

        pnlRiskRatingStandard.Visible = false;
        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.RiskRatingStandard))
        {
            pnlRiskRatingStandard.Visible = true;
        }
    }

    protected void uxRebindRiskRating_Click(object sender, EventArgs e)
    {
        OnDataBindControls(DataBindAction.RiskRating);
    }

    protected void InitializeTransactionFilterType()
    {
        cbxDaystoFundStandard.Items.Add(new RadComboBoxItem() { Value = "Between", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_Between").ToString() });
        cbxDaystoFundStandard.Items.Add(new RadComboBoxItem() { Value = "GreaterThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_GreaterThan").ToString() });
        cbxDaystoFundStandard.Items.Add(new RadComboBoxItem() { Value = "LessThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_LessThan").ToString() });
        cbxDaystoFundStandard.DataBind();

    }

    private DataTable GetDataTable()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32));
        parameters.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetAssignmentValueFilter", parameters);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.LoadTransactionalFilter:
                InitializeTransactionFilterType();
                DataTable dt = this.GetDataTable();

                if (dt.Rows.Count > 0)
                {
                    int? daystoFundFrom = string.IsNullOrEmpty(dt.Rows[0]["DaystoFundFrom"].ToString()) ? null : (int?)dt.Rows[0]["DaystoFundFrom"];
                    int? daystoFundTo = string.IsNullOrEmpty(dt.Rows[0]["DaystoFundTo"].ToString()) ? null : (int?)dt.Rows[0]["DaystoFundTo"];

                    if (daystoFundFrom != null || daystoFundTo != null)
                    {
                        cbDaystoFundStandard.Checked = true;

                        if (daystoFundFrom == null)
                        {
                            cbxDaystoFundStandard.SelectedValue = "GreaterThan";
                            txtDTFGtStandard.Value = daystoFundTo;

                        }
                        else if (daystoFundTo == null)
                        {
                            cbxDaystoFundStandard.SelectedValue = "LessThan";
                            txtDTFLtStandard.Value = daystoFundFrom;
                        }
                        else
                        {
                            cbxDaystoFundStandard.SelectedValue = "Between";
                            txtDTFBetweenFromStandard.Value = daystoFundFrom;
                            txtDTFBetweenToStandard.Value = daystoFundTo;
                        }
                    }

                }
                break;

            case DataBindAction.FICOScore:
                BuildFicoScore();
                break;

            case DataBindAction.RiskRating:
                SetRiskRatingInfo();
                break;
        }
    }

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    public int Save()
    {
        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.FICOScoreStandard))
        {
            SaveFICoScore();
        }


        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.DaystoFundStandard))
        {
            int? transactionalTransactionCountFrom = null;
            int? transactionalTransactionCountTo = null;

            if (cbDaystoFundStandard.Checked)
            {
                var transactionCountType = cbxDaystoFundStandard.SelectedValue;

                switch (transactionCountType)
                {
                    case "Between":
                        transactionalTransactionCountFrom = (int)txtDTFBetweenFromStandard.Value;
                        transactionalTransactionCountTo = (int)txtDTFBetweenToStandard.Value;
                        break;
                    case "GreaterThan":
                        transactionalTransactionCountTo = (int)txtDTFGtStandard.Value;
                        break;
                    case "LessThan":
                        transactionalTransactionCountFrom = (int)txtDTFLtStandard.Value;
                        break;
                }
            }

            FilterParameterCollection parameter = new FilterParameterCollection();
            FilterParameterCollection parameterOut;
            parameter.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameter.Add(new FilterParameter("@AssignmentID", int.Parse(this.PrimaryID), DbType.Int32));
            parameter.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
            parameter.Add(new FilterParameter("@DaystoFundFrom", transactionalTransactionCountFrom, DbType.Int32));
            parameter.Add(new FilterParameter("@DaystoFundTo", transactionalTransactionCountTo, DbType.Int32));
            WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveAssignmentValueFilter ", parameter, out parameterOut);
        }

        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion

    #region Validate

    public void InitValidation()
    {
        SixtydayTransCntMsgStandard.ShowOnLoad = false;
    }

    #endregion

    public void Rebind()
    {
        OnDataBindControls(DataBindAction.LoadTransactionalFilter);
        OnDataBindControls(DataBindAction.FICOScore);
        VisibleControls();
        InitValidation();
    }

    protected void BuildFicoScore()
    {
        DataTable dt = GeneralFuncsLib.GetCreditScoreByFilter(PrimaryID.ToInt(), this.Mode, true);
        DataTable dataCredit = GeneralFuncsLib.GetRiskCreditScore(PrimaryID.ToInt(), WebSiteEnums.AssignmentFilterModes.All, WebSiteEnums.ParamFilterMode.Assignment, true);
        uxCbCreditScoreToStandard.DataSource = dataCredit;
        uxCbCreditScoreToStandard.DataBind();
        uxCbCreditScoreFromStandard.DataSource = dataCredit;
        uxCbCreditScoreFromStandard.DataBind();
        if (dt.HasData())
        {
            string creditScoreTo = dt.Rows[0]["CreditScoreTo"].ToString();
            string creditScoreFrom = dt.Rows[0]["CreditScoreFrom"].ToString();
            if (!creditScoreTo.IsNullOrEmpty() && !creditScoreTo.Equals("0"))
            {
                uxChkIsToStandard.Checked = true;
                if (uxChkIsToStandard.Checked)
                {
                    uxCbCreditScoreToStandard.Enabled = true;
                }
                uxCbCreditScoreToStandard.SelectedValue = creditScoreTo;
            }
            if (!creditScoreFrom.IsNullOrEmpty() && !creditScoreFrom.Equals("0"))
            {
                uxChkIsFromStandard.Checked = true;
                if (uxChkIsFromStandard.Checked)
                {
                    uxCbCreditScoreFromStandard.Enabled = true;
                }
                uxCbCreditScoreFromStandard.SelectedValue = creditScoreFrom;
            }
        }
    }

    public void SaveFICoScore()
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@PrimaryID", PrimaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@IsCreditScoreTo", uxChkIsToStandard.Checked, DbType.Boolean));
        paramsIn.Add(new FilterParameter("@IsCreditScoreFrom", uxChkIsFromStandard.Checked, DbType.Boolean));

        if (uxChkIsToStandard.Checked && !string.IsNullOrEmpty(uxCbCreditScoreToStandard.SelectedValue))
        {
            paramsIn.Add(new FilterParameter("@Value_CreditScoreTo", uxCbCreditScoreToStandard.SelectedValue, DbType.Int32));
        }

        if (uxChkIsFromStandard.Checked && !string.IsNullOrEmpty(uxCbCreditScoreFromStandard.SelectedValue))
        {
            paramsIn.Add(new FilterParameter("@Value_CreditScoreFrom", uxCbCreditScoreFromStandard.SelectedValue, DbType.Int32));
        }

        paramsIn.Add(new FilterParameter("@FilterMode", (int)Mode, DbType.Int32));
        FilterParameterCollection paramsOut;
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_InsertUpdateCreditScore", paramsIn, out paramsOut);

    }
}
