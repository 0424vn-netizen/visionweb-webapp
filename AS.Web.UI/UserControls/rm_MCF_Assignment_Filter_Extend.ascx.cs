using System;
using System.Data;
using AS.Controls.Global;
using AS.Common.DBManager;

public partial class UserControls_rm_MCF_Assignment_Filter_DaystoFund : GlobalUserControl, IRiskParamFilter
{

    #region Enums
    enum DataBindAction
    {
        LoadTransactionalFilter,
        LoadFutureDeliveryIndicator,
        FICOScore
    }
    enum PostBackAction { }
    #endregion

    public WebSiteEnums.FeatureMode FeatureMode { get; set; }

    public bool DisplayControl { get; set; }

    public int IsFutureDeliveryIndicator
    {
        get
        {
            if (rdWatchStatusNA.Checked)
                return (int)WebSiteEnums.FutureDeliveryIndicator.All;
            else if (rdWatchStatusOn.Checked)
                return (int)WebSiteEnums.FutureDeliveryIndicator.Yes;
            else
                return (int)WebSiteEnums.FutureDeliveryIndicator.No;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        VisibleControls();
        InitValidation();
    }

    private void VisibleControls()
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            cbDaystoFund.Enabled = false;

            cbxDaystoFund.OnClientDropDownOpening = "CancelDropDown";
            txtDTFBetweenFrom.ReadOnly = txtDTFBetweenTo.ReadOnly = txtDTFGt.ReadOnly = txtDTFLt.ReadOnly = FeatureMode == WebSiteEnums.FeatureMode.View;
            plhCreditScoreCustom.Visible = uxChkIsFromCustom.Enabled = uxChkIsToCustom.Enabled = FeatureMode == WebSiteEnums.FeatureMode.Edit;
            RejectOnClickEventForRadioWatchStatus();
        }

        pnlDaystoFund.Visible = false;
        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.DaystoFund))
        {
            pnlDaystoFund.Visible = true;
        }
        pnlFutureDeliveryIndicator.Visible = false;
        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.FutureDeliveryIndicator))
        {
            pnlFutureDeliveryIndicator.Visible = true;
        }

        pnCreditScoreCustom.Visible = false;
        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.FICOScore))
        {
            pnCreditScoreCustom.Visible = true;
        }
    }

    protected void InitializeTransactionFilterType()
    {
        cbxDaystoFund.Items.Add(new RadComboBoxItem() { Value = "Between", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_Between").ToString() });
        cbxDaystoFund.Items.Add(new RadComboBoxItem() { Value = "GreaterThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_GreaterThan").ToString() });
        cbxDaystoFund.Items.Add(new RadComboBoxItem() { Value = "LessThan", Text = GetLocalResourceObject("Risk_Assignment_Filter_Transactional_ascx_cs_LessThan").ToString() });
        cbxDaystoFund.DataBind();

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
                    int? DaystoFundTo = string.IsNullOrEmpty(dt.Rows[0]["DaystoFundTo"].ToString()) ? null : (int?)dt.Rows[0]["DaystoFundTo"];

                    if (daystoFundFrom != null || DaystoFundTo != null)
                    {
                        cbDaystoFund.Checked = true;

                        if (daystoFundFrom == null)
                        {
                            cbxDaystoFund.SelectedValue = "GreaterThan";
                            txtDTFGt.Value = DaystoFundTo;

                        }
                        else if (DaystoFundTo == null)
                        {
                            cbxDaystoFund.SelectedValue = "LessThan";
                            txtDTFLt.Value = daystoFundFrom;
                        }
                        else
                        {
                            cbxDaystoFund.SelectedValue = "Between";
                            txtDTFBetweenFrom.Value = daystoFundFrom;
                            txtDTFBetweenTo.Value = DaystoFundTo;
                        }
                    }

                }
                break;

            case DataBindAction.LoadFutureDeliveryIndicator:
                SetFutureDeliveryIndicator();
                break;

            case DataBindAction.FICOScore:
                BuildFicoScore();
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
        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.FICOScore))
        {
            SaveFICoScore();
        }


        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.DaystoFund) ||
            (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.FutureDeliveryIndicator))
            )
        {
            int? transactionalTransactionCountFrom = null;
            int? transactionalTransactionCountTo = null;

            if (cbDaystoFund.Checked)
            {
                var transactionCountType = cbxDaystoFund.SelectedValue;

                switch (transactionCountType)
                {
                    case "Between":
                        transactionalTransactionCountFrom = (int)txtDTFBetweenFrom.Value;
                        transactionalTransactionCountTo = (int)txtDTFBetweenTo.Value;
                        break;
                    case "GreaterThan":
                        transactionalTransactionCountTo = (int)txtDTFGt.Value;
                        break;
                    case "LessThan":
                        transactionalTransactionCountFrom = (int)txtDTFLt.Value;
                        break;
                }
            }

            FilterParameterCollection parameter = new FilterParameterCollection();
            FilterParameterCollection parameterOut;
            parameter.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameter.Add(new FilterParameter("@AssignmentID", int.Parse(this.PrimaryID), DbType.Int32));
            parameter.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
            if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.DaystoFund))
            {
                parameter.Add(new FilterParameter("@DaystoFundFrom", transactionalTransactionCountFrom, DbType.Int32));
                parameter.Add(new FilterParameter("@DaystoFundTo", transactionalTransactionCountTo, DbType.Int32));
            }
            if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.FutureDeliveryIndicator))
            {
                parameter.Add(new FilterParameter("@FutureDeliveryIndicator", IsFutureDeliveryIndicator, DbType.Int32));
            }
            WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveAssignmentValueFilter ", parameter, out parameterOut);
        }

        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion

    #region Validate

    public void InitValidation()
    {
        SixtydayTransCntMsg.ShowOnLoad = false;
    }

    #endregion

    public void Rebind()
    {
        OnDataBindControls(DataBindAction.LoadTransactionalFilter);
        OnDataBindControls(DataBindAction.LoadFutureDeliveryIndicator);
        OnDataBindControls(DataBindAction.FICOScore);
        VisibleControls();
        InitValidation();
    }

    public void SetWatchStatus(int watchStatus)
    {
        switch (watchStatus)
        {
            case (int)WebSiteEnums.FutureDeliveryIndicator.No:
                rdWatchStatusOff.Checked = true;
                break;
            case (int)WebSiteEnums.FutureDeliveryIndicator.Yes:
                rdWatchStatusOn.Checked = true;
                break;
            case (int)WebSiteEnums.FutureDeliveryIndicator.All:
                rdWatchStatusNA.Checked = true;
                break;
        }
    }

    public void RejectOnClickEventForRadioWatchStatus()
    {
        rdWatchStatusNA.Enabled = false;
        rdWatchStatusOn.Enabled = false;
        rdWatchStatusOff.Enabled = false;
    }

    private void SetFutureDeliveryIndicator()
    {
        var dataInfo = GetDataTable();
        int futureDeliveryIndicator =
            dataInfo.Rows[0]["FutureDeliveryIndicator"] == DBNull.Value ?
            (int)WebSiteEnums.FutureDeliveryIndicator.All : Convert.ToInt32(dataInfo.Rows[0]["FutureDeliveryIndicator"]);
        SetWatchStatus(futureDeliveryIndicator);
    }

    protected void BuildFicoScore()
    {
        DataTable dt = GeneralFuncsLib.GetCreditScoreByFilter(PrimaryID.ToInt(), this.Mode, true);
        DataTable dataCredit = GeneralFuncsLib.GetRiskCreditScore(PrimaryID.ToInt(), WebSiteEnums.AssignmentFilterModes.All, WebSiteEnums.ParamFilterMode.Assignment, true);
        uxCbCreditScoreToCustom.DataSource = dataCredit;
        uxCbCreditScoreToCustom.DataBind();
        uxCbCreditScoreFromCustom.DataSource = dataCredit;
        uxCbCreditScoreFromCustom.DataBind();
        if (dt.HasData())
        {
            string creditScoreTo = dt.Rows[0]["CreditScoreTo"].ToString();
            string creditScoreFrom = dt.Rows[0]["CreditScoreFrom"].ToString();
            if (!creditScoreTo.IsNullOrEmpty() && !creditScoreTo.Equals("0"))
            {
                uxChkIsToCustom.Checked = true;
                if (uxChkIsToCustom.Checked)
                {
                    uxCbCreditScoreToCustom.Enabled = true;
                }
                uxCbCreditScoreToCustom.SelectedValue = creditScoreTo;
            }
            if (!creditScoreFrom.IsNullOrEmpty() && !creditScoreFrom.Equals("0"))
            {
                uxChkIsFromCustom.Checked = true;
                if (uxChkIsFromCustom.Checked)
                {
                    uxCbCreditScoreFromCustom.Enabled = true;
                }
                uxCbCreditScoreFromCustom.SelectedValue = creditScoreFrom;
            }
        }
    }

    public void SaveFICoScore()
    {
        if ((uxChkIsToCustom.Checked && !string.IsNullOrEmpty(uxCbCreditScoreToCustom.SelectedValue))
            || (uxChkIsFromCustom.Checked && !string.IsNullOrEmpty(uxCbCreditScoreFromCustom.SelectedValue)))
        {
            FilterParameterCollection paramsIn = new FilterParameterCollection();
            paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paramsIn.Add(new FilterParameter("@PrimaryID", PrimaryID, DbType.Int32));
            paramsIn.Add(new FilterParameter("@IsCreditScoreTo", uxChkIsToCustom.Checked, DbType.Boolean));
            paramsIn.Add(new FilterParameter("@IsCreditScoreFrom", uxChkIsFromCustom.Checked, DbType.Boolean));

            if (uxChkIsToCustom.Checked && !string.IsNullOrEmpty(uxCbCreditScoreToCustom.SelectedValue))
            {                
                paramsIn.Add(new FilterParameter("@Value_CreditScoreTo", uxCbCreditScoreToCustom.SelectedValue, DbType.Int32));
            }

            if (uxChkIsFromCustom.Checked && !string.IsNullOrEmpty(uxCbCreditScoreFromCustom.SelectedValue))
            {                
                paramsIn.Add(new FilterParameter("@Value_CreditScoreFrom", uxCbCreditScoreFromCustom.SelectedValue, DbType.Int32));
            }
            
            paramsIn.Add(new FilterParameter("@FilterMode", (int)Mode, DbType.Int32));
            FilterParameterCollection paramsOut;
            WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_InsertUpdateCreditScore", paramsIn, out paramsOut);
        }
    }
}
