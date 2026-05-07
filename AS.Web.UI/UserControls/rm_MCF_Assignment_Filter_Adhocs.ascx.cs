using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.WS.Entities;

public partial class UserControls_rm_MCF_Assignment_Filter_Adhocs : GlobalUserControl, IRiskParamFilter
{
    #region Enums

    enum DataBindAction
    {
        BindState,
        BindSIC,
        BindZip,
        BindProfile,
        BindHierarchyFilter,
        BindHRCode,
        BindMerchantRank,
        BindACHHoldDays,
        BindOwnerLastName,
        ZipCode,
        MerchantFundingStatus,
        MerchantClassifications,
        GverifyCode,
        GauthenticateCode,
        G2CompassAutoApprovalIndicator,
        FutureDeliveryIndicator

    }

    enum PostBackAction
    {
        BindState,
        BindSIC,
        BindZip,
        BindProfile,
        CountMerchant,
        BindHierarchyFilter,
        BindHRCode,
        BindMerchantRank,
        BindACHHoldDays,
        BindOwnerLastName,
        ZipCode,
        MerchantFundingStatus,
        MerchantClassifications,
        GverifyCode,
        GauthenticateCode,
        G2CompassAutoApprovalIndicator,
        FutureDeliveryIndicator
    }
    #endregion

    #region Constants

    private const string ASSIGNMENT_ID = "AssignmentID";
    private const string MERCHANT_COUNT = "MerchantCount";

    #endregion Constants

    #region Fields

    private string _currentHierarchyFilterMode;
    private string _assignmentIntruderQuery = string.Empty;
    private string _queryString;
    private string _queryString1;
    private DataTable _merchantFilterInfo = null;
    private string _groupHierarchy = string.Empty;
    private bool _isShowGroupHierarchy = false;

    #endregion Fields

    #region Properties

    private string AssignmentIntruderQuery
    {
        get
        {
            if (_assignmentIntruderQuery.Length == 0)
            {
                _assignmentIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                      this.ID, new string[] { ASSIGNMENT_ID });
            }
            return _assignmentIntruderQuery;
        }
    }

    public string QueryString
    {
        get
        {
            _queryString = Page.BuildSecureQueryString(string.Format(
                "PrimaryID={0}&mode={1}", PrimaryID,
                Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc
                                                           : (int)WebSiteEnums.ParamFilterMode.Assignment));
            return _queryString;
        }
        set
        {
            _queryString = value;
        }
    }

    public string QueryString1
    {
        get
        {
            _queryString1 = Page.BuildSecureQueryString(string.Format(
                "PrimaryID={0}&mode={1}&codeset={2}",
                PrimaryID,
                Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc
                                                           : (int)WebSiteEnums.ParamFilterMode.Assignment,
                uxMarketData.Value));
            return _queryString1;
        }
        set
        {
            _queryString1 = value;
        }
    }

    public int IsMerchantsOnWatch
    {
        get
        {
            return uxWatchStatus.IsMerchantsOnWatch;
        }
    }

    public DataTable MerchantFilterInfo
    {
        get
        {
            return _merchantFilterInfo;
        }
        set
        {
            _merchantFilterInfo = value;
        }
    }
    public int MerchantCount
    {
        get
        {
            if (ViewState[MERCHANT_COUNT] == null)
            {
                ViewState[MERCHANT_COUNT] = 0;
            }
            return (int)ViewState[MERCHANT_COUNT];
        }
        set
        {
            ViewState[MERCHANT_COUNT] = value;
        }
    }

    public WebSiteEnums.FeatureMode FeatureMode
    {
        get;
        set;
    }

    protected int MaxLengthViewMore
    {
        get
        {
            return Convert.ToInt32(WebSiteSettings.RiskViewMore);
        }
    }
   

    #endregion Properties

    #region Methods

    #region Public Methods

    public string HierarchyFilterQueryString(string HierarchyFilterMode, string clientIDbtn)
    {
        return Page.BuildSecureQueryString(string.Format(
            "PrimaryID={0}&mode={1}&hierarchyMode={2}&clientID={3}",
            PrimaryID,
            Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc
                                                       : (int)WebSiteEnums.ParamFilterMode.Assignment,
            HierarchyFilterMode,
            clientIDbtn));
    }

    #endregion Public Methods

    #region Protected Methods

    protected void Page_Load(object sender, EventArgs e)
    {

        ShowHideForHRCodeConfig();
        uxApprovalDate.PrimaryID = PrimaryID;
        uxApprovalDate.Mode = Mode;
        uxApprovalDate.FeatureMode = FeatureMode;

        uxFirstBatchDate.PrimaryID = PrimaryID;
        uxFirstBatchDate.Mode = Mode;
        uxFirstBatchDate.FeatureMode = FeatureMode;
        //set default values for transactional filter
        uxTransactionalFilter.PrimaryID = PrimaryID;
        uxTransactionalFilter.Mode = Mode;
        uxTransactionalFilter.FeatureMode = FeatureMode;

        uxECommerceFilter.PrimaryID = PrimaryID;
        uxECommerceFilter.Mode = Mode;
        uxECommerceFilter.FeatureMode = FeatureMode;

        uxSourceHierarchyFilter.PrimaryID = PrimaryID;
        uxSourceHierarchyFilter.Mode = Mode;
        uxSourceHierarchyFilter.FeatureMode = FeatureMode;

        Risk_ExtendFilter.PrimaryID = PrimaryID;
        Risk_ExtendFilter.Mode = Mode;
        Risk_ExtendFilter.FeatureMode = FeatureMode;
        //end
        if (!IsPostBack)
        {
            //set default filter for MS user
            switch (SessionManager.CurrentUserType)
            {
                case WebSiteEnums.UserHierarchyMode.Hierarchy:
                case WebSiteEnums.UserHierarchyMode.Headquarter:
                    {
                        string hierarchyFilterMode = GeneralFuncsLib.GetHierarchyInfo(SessionManager.CurrentUser.EntityType).HierarchyMode;
                        string entityID = SessionManager.CurrentUser.EntityID;
                        string hierarchyFilterValue = GetHierarchyFilterValue(hierarchyFilterMode, entityID);

                        FilterParameterCollection paramsIn = new FilterParameterCollection();
                        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        WebServices.RiskServices.SaveHierarchyFilter(paramsIn,
                            PrimaryID, hierarchyFilterMode, hierarchyFilterValue,
                            (int)WebSiteEnums.ParamFilterMode.Adhoc, true);
                    }
                    break;
            }

            CalculateMerchantCount();
            GetMerchantFilterInfo();
            OnDataBindControls(DataBindAction.BindState, null);
            OnDataBindControls(DataBindAction.BindSIC, null);
            OnDataBindControls(DataBindAction.BindZip, null);
            OnDataBindControls(DataBindAction.BindProfile, null);
            OnDataBindControls(DataBindAction.BindHierarchyFilter, null);
            if (Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RISK_ATTRIBUTE) ||
                Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RISK_ATTRIBUTE_MS))
                OnDataBindControls(DataBindAction.MerchantClassifications, null);
            if (!GeneralFuncsLib.GetDataOfExtendedSetting("HIDE_ASSIGNMENT_FILTERS_HIGHRISK_HRCODE").Equals("true"))
            {
                OnDataBindControls(DataBindAction.BindHRCode, null);
            }
            //36801 – MCPS – VW Supplemental MIF – FE - Enabled
            BindSupplemetal();
            //End
            BindFilterExtend();


        }
        VisibleControls();
        InitValidation();

        //ajaxify each literal control on repeater.
        foreach (RepeaterItem item in uxHierarchyFilterRepeater.Items)
        {
            Panel div = item.FindControl("divHierarchy") as Panel;
            Button btn = item.FindControl("btnRefreshHierarchy") as Button;

            RadAjaxManagerProxyReview.AjaxSettings.AddAjaxSetting(btn, div);
            RadAjaxManagerProxyReview.AjaxSettings.AddAjaxSetting(btn, hdnCountSelectedFilter);
        }
    }
    protected void CountSelectedFilter()
    {
        int count = 0;
        foreach (RepeaterItem item in uxHierarchyFilterRepeater.Items)
        {
            Literal ltr = (Literal)item.FindControl("lblHierarchy");
            if (!string.IsNullOrEmpty(ltr.Text) && ltr.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            {
                count++;
            }
        }
        if (!string.IsNullOrEmpty(lblState.Text) && lblState.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;
        if (!string.IsNullOrEmpty(lblSIC.Text) && lblSIC.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;
        if (!string.IsNullOrEmpty(lblProfile.Text) && lblProfile.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;
        if (!string.IsNullOrEmpty(lbMerchantRank.Text) && lbMerchantRank.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;
        if (!string.IsNullOrEmpty(lbAchHoldDays.Text) && lbAchHoldDays.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;
        if (!string.IsNullOrEmpty(lbOwnerLastName.Text) && lbOwnerLastName.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;
        if (!string.IsNullOrEmpty(lbZipCode.Text) && lbZipCode.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;
        if (!string.IsNullOrEmpty(lbMerchantFundingStatus.Text) && lbMerchantFundingStatus.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;
        if (!string.IsNullOrEmpty(lblHRCode.Text) && lblHRCode.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;
        if (!string.IsNullOrEmpty(uxMerchantClassificationsLabel.Text) && uxMerchantClassificationsLabel.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;

        hdnCountSelectedFilter.Value = VeraCodeSolution.ValidateResponseData(count.ToString());
    }
    protected void ShowHideForHRCodeConfig()
    {
        uxHRCode.Visible = !GeneralFuncsLib.GetDataOfExtendedSetting("HIDE_ASSIGNMENT_FILTERS_HIGHRISK_HRCODE").Equals("true");
    }

    //Bind supplemetal
    protected void BindSupplemetal()
    {
        if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.MerchantRank))
        {
            pnMerchantRank.Visible = true;
            OnDataBindControls(DataBindAction.BindMerchantRank, null);
        }

        if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.ACHHoldDays))
        {
            pnACHHoldDay.Visible = true;
            OnDataBindControls(DataBindAction.BindACHHoldDays, null);
        }

        if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.OwnerLastName))
        {
            pnOwnerLastName.Visible = true;
            OnDataBindControls(DataBindAction.BindOwnerLastName, null);
        }

        if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.Zip3))
        {
            pnZip3.Visible = true;
            OnDataBindControls(DataBindAction.ZipCode, null);
        }

        if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.MerchantFundingStatus))
        {
            pnMerchantFundingStatus.Visible = true;
            OnDataBindControls(DataBindAction.MerchantFundingStatus, null);
        }

        if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.CreaditCore))
        {
            pnCreditScore.Visible = true;
            //Bind Credit Score
            DataTable dt = GeneralFuncsLib.GetCreditScoreByFilter(PrimaryID.ToInt(), WebSiteEnums.ParamFilterMode.Adhoc, true);
            DataTable dataCredit = GeneralFuncsLib.GetRiskCreditScore(PrimaryID.ToInt(), WebSiteEnums.AssignmentFilterModes.All, WebSiteEnums.ParamFilterMode.Adhoc, true);
            uxCbCreditScoreTo.DataSource = dataCredit;
            uxCbCreditScoreTo.DataBind();
            uxCbCreditScoreFrom.DataSource = dataCredit;
            uxCbCreditScoreFrom.DataBind();
            if (dt.HasData())
            {
                string creditScoreTo = dt.Rows[0]["CreditScoreTo"].ToString();
                string creditScoreFrom = dt.Rows[0]["CreditScoreFrom"].ToString();
                if (!creditScoreTo.IsNullOrEmpty() && !creditScoreTo.Equals("0"))
                {
                    uxChkIsTo.Checked = true;
                    if (uxChkIsTo.Checked)
                    {
                        uxCbCreditScoreTo.Enabled = false;
                    }
                    uxCbCreditScoreTo.SelectedValue = creditScoreTo;
                }
                if (!creditScoreFrom.IsNullOrEmpty() && !creditScoreFrom.Equals("0"))
                {
                    uxChkIsFrom.Checked = true;
                    if (uxChkIsFrom.Checked)
                    {
                        uxCbCreditScoreFrom.Enabled = false;
                    }
                    uxCbCreditScoreFrom.SelectedValue = creditScoreFrom;
                }
            }
        }
    }

    protected void uxHierarchyFilterRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string hierarchyFilterMode = SessionManager.RiskHierarchyFilter
            .Rows[e.Item.ItemIndex]["HierarchyFilterMode"].ToString();

        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                DataRowView row = e.Item.DataItem as DataRowView;

                if (row["BEProcessor"] == null)
                {
                    _groupHierarchy = string.Empty;
                }
                else
                {
                    if (row["BEProcessor"].ToString() != _groupHierarchy)
                    {

                        _groupHierarchy = row["BEProcessor"].ToString();
                        _isShowGroupHierarchy = true;
                    }
                }

                if (_isShowGroupHierarchy)
                {
                    if (FeatureMode == WebSiteEnums.FeatureMode.View)
                    {
                        ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyView")).Visible = true;
                        ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyEdit")).Visible = false;
                        ((Literal)e.Item.FindControl("uxGroupHierarchyTitleView")).Text =
                            VeraCodeSolution.DoVeraCode(row["BEProcessor"].ToString().TrimEnd() + " - " + GetLocalResourceObject("Risk_Assignment_Filter_Adhocs_ascx_cs_Hierarchy").ToString());
                    }
                    else
                    {
                        ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyView")).Visible = false;
                        ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyEdit")).Visible = true;
                        ((Literal)e.Item.FindControl("uxGroupHierarchyTitleEdit")).Text =
                            VeraCodeSolution.DoVeraCode(row["BEProcessor"].ToString().TrimEnd() + " - " + GetLocalResourceObject("Risk_Assignment_Filter_Adhocs_ascx_cs_Hierarchy").ToString());
                    }
                }
                else
                {
                    ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyEdit")).Visible = false;
                    ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyView")).Visible = false;

                }
                _isShowGroupHierarchy = false;
                BindHierarchyLabel((Literal)e.Item.FindControl("lblHierarchy"), hierarchyFilterMode);
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.BindHierarchyFilter:
                Button btn = (Button)sender;
                BindHierarchyLabel((Literal)btn.Parent.FindControl("lblHierarchy"),
                    _currentHierarchyFilterMode);
                break;
            case PostBackAction.BindState:
                BindStateLabel();
                break;
            case PostBackAction.BindSIC:
                BindSICLabel();
                break;
            case PostBackAction.BindProfile:
                BindProfileLabel();
                break;
            case PostBackAction.BindMerchantRank:
                BindMerchantRankLabel();
                break;
            case PostBackAction.BindACHHoldDays:
                BindACHHoldDaysLabel();
                break;
            case PostBackAction.BindOwnerLastName:
                BindOwnerLastNameLabel();
                break;
            case PostBackAction.ZipCode:
                BindZipCodeLabel();
                break;
            case PostBackAction.MerchantFundingStatus:
                BindMerchantFundingStatusLabel();
                break;
            case PostBackAction.MerchantClassifications:
                BindMerchantClassificationsLabel();
                break;
            case PostBackAction.BindHRCode:
                BindHRCodeLabel();
                break;
            case PostBackAction.CountMerchant:
                int error = Save();
                if (error != 0)
                    return;
                CalculateMerchantCount();
                (Page.Master as BaseMasterPage).AjaxAddResponseScript("MerchantCountConfirm(" + MerchantCount + ");");
                break;

            case PostBackAction.GverifyCode:
                BindGverifyCodeLabel();
                break;

            case PostBackAction.GauthenticateCode:
                BindGauthenticateCodeLabel();
                break;
            case PostBackAction.G2CompassAutoApprovalIndicator:
                BindG2CompassLabel();
                break;
        }
        CountSelectedFilter();
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindState:
                BindStateLabel();
                break;
            case DataBindAction.BindSIC:
                BindSICLabel();
                break;
            case DataBindAction.BindProfile:
                BindProfileLabel();
                break;
            case DataBindAction.BindMerchantRank:
                BindMerchantRankLabel();
                break;
            case DataBindAction.BindACHHoldDays:
                BindACHHoldDaysLabel();
                break;
            case DataBindAction.BindOwnerLastName:
                BindOwnerLastNameLabel();
                break;
            case DataBindAction.ZipCode:
                BindZipCodeLabel();
                break;
            case DataBindAction.MerchantFundingStatus:
                BindMerchantFundingStatusLabel();
                break;
            case DataBindAction.MerchantClassifications:
                BindMerchantClassificationsLabel();
                break;
            case DataBindAction.BindHRCode:
                BindHRCodeLabel();
                break;
            case DataBindAction.BindHierarchyFilter:
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.AddLanguageID();
                SessionManager.RiskHierarchyFilter = WebServices.RiskServices.GetHierarchyFilterList(parameters, true);

                var info = SessionManager.RiskHierarchyFilter;
                uxHierarchyFilterRepeater.DataSource = info;
                uxHierarchyFilterRepeater.DataBind();
                break;

            case DataBindAction.GverifyCode:
                BindGverifyCodeLabel();
                break;

            case DataBindAction.GauthenticateCode:
                BindGauthenticateCodeLabel();
                break;
            case DataBindAction.G2CompassAutoApprovalIndicator:
                BindG2CompassLabel();
                break;
        }
        int count = 0;
        hdnCountSelectedFilter.Value = VeraCodeSolution.ValidateResponseData(count.ToString());
    }

    /// <summary>
    /// Bind data for Merchant Range: From, To
    /// </summary>
    private void BindMerchantRange()
    {
        if (_merchantFilterInfo != null && _merchantFilterInfo.Rows.Count > 0)
        {
            SetWatchStatus();

            long mer = -1;
            long.TryParse(_merchantFilterInfo.Rows[0]["MerchantNumber"].ToString(), out mer);
            txtMerchantNumber.Text = VeraCodeSolution.ValidateResponseData(mer < 0 ? string.Empty : mer.ToString());

            long from = -1;
            long to = -1;

            long.TryParse(_merchantFilterInfo.Rows[0]["MerchantRangeFrom"].ToString(), out from);
            long.TryParse(_merchantFilterInfo.Rows[0]["MerchantRangeTo"].ToString(), out to);

            chkExcludeMerchantsClosedStatus.Checked = _merchantFilterInfo.Rows[0]["IsExcludeMerchantsClosedStatus"] != DBNull.Value
               && Convert.ToBoolean(_merchantFilterInfo.Rows[0]["IsExcludeMerchantsClosedStatus"]);
        }
    }

    public void GetMerchantFilterInfo()
    {
        //get merchant filter
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _merchantFilterInfo = WebServices.RiskServices.GetAssignmentMerchantFilter(
            paramsIn, int.Parse(PrimaryID), (int)Mode, true);

        BindMerchantRange();
    }

    public int SaveMerchantFilter()
    {
        string MerchantNumber = VeraCodeSolution.DoVeraCode(txtMerchantNumber.Text) == string.Empty
            ? "-1" : VeraCodeSolution.DoVeraCode(txtMerchantNumber.Text);

        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        AssignmentMerchantFilterModel assignmentMerchantFilterModel = new AssignmentMerchantFilterModel()
        {
            AssignmentId = int.Parse(PrimaryID),
            IsMerchantsOnWatch = IsMerchantsOnWatch,
            IsAllMerchants = null,
            ReportDate = DateTime.Now,
            MerchantNumber = MerchantNumber,
            Mode = (int)Mode
        };
        WebServices.RiskServices.SaveAssignmentMerchantFilter(paramsIn, assignmentMerchantFilterModel, true, chkExcludeMerchantsClosedStatus.Checked);

        return 0;
    }


    //TK36801 – MCPS – VW Supplemental MIF – FE
    //Save Credit Score
    public void SaveCreditScore()
    {
        if ((uxChkIsTo.Checked && !string.IsNullOrEmpty(uxCbCreditScoreTo.SelectedValue)) 
            || (uxChkIsFrom.Checked && !string.IsNullOrEmpty(uxCbCreditScoreFrom.SelectedValue)))
        {
            FilterParameterCollection paramsIn = new FilterParameterCollection();
            paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paramsIn.Add(new FilterParameter("@PrimaryID", PrimaryID, DbType.Int32));
            paramsIn.Add(new FilterParameter("@IsCreditScoreFrom", uxChkIsFrom.Checked, DbType.Boolean));
            paramsIn.Add(new FilterParameter("@IsCreditScoreTo", uxChkIsTo.Checked, DbType.Boolean));

            if (uxChkIsTo.Checked && !string.IsNullOrEmpty(uxCbCreditScoreTo.SelectedValue))
            {                
                paramsIn.Add(new FilterParameter("@Value_CreditScoreTo", uxCbCreditScoreTo.SelectedValue, DbType.Int32));
            }

            if (uxChkIsFrom.Checked && !string.IsNullOrEmpty(uxCbCreditScoreFrom.SelectedValue))
            {               
                paramsIn.Add(new FilterParameter("@Value_CreditScoreFrom", uxCbCreditScoreFrom.SelectedValue, DbType.Int32));
            }

            paramsIn.Add(new FilterParameter("@FilterMode", (int)Mode, DbType.Int32));
            FilterParameterCollection paramsOut;
            WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_InsertUpdateCreditScore", paramsIn, out paramsOut);
        }       
    }

    private void VisibleControls()
    {
        plhState.Visible = plhProfile.Visible = plhSIC.Visible = plhRefesh.Visible = plhMerchantRank.Visible
            = plhAchHoldDay.Visible = plhOwnerLastName.Visible = plhZipCode.Visible = plhMerchantFundingStatus.Visible
            = uxChkIsFrom.Enabled = uxChkIsTo.Enabled = plhCreditScore.Visible 
            = chkExcludeMerchantsClosedStatus.Enabled = FeatureMode == WebSiteEnums.FeatureMode.Edit;

        pnlbtnEditMerchantClassifications.Visible = pnlGverifyCodeEdit.Visible = pnlGauthenticateCodeEdit.Visible = pnlG2CompassEdit.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;

        foreach (RepeaterItem item in uxHierarchyFilterRepeater.Items)
        {
            PlaceHolder HierarchyPlaceHolder = item.FindControl("plhHierarchy") as PlaceHolder;
            HierarchyPlaceHolder.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;
        }

        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            uxWatchStatus.RejectOnClickEventForRadioWatchStatus();
            // Show for all clients except MPS
            if (SessionManager.CurrentClient != WebSiteConstants.MPS_CLIENT)
            {
                uxGroupDemographicView.Visible = true;
            }
            if (GeneralFuncsLib.GetDataOfExtendedSetting("Show_HR").ToLower().Equals("true"))
            {
                uxHighRisk.Visible = true;
                uxHighRiskView.Visible = true;
            }
        }
        else
        {
            // Show for all clients except MPS
            if (SessionManager.CurrentClient != WebSiteConstants.MPS_CLIENT)
            {
                uxGroupDemographicEdit.Visible = true;
            }
            if (GeneralFuncsLib.GetDataOfExtendedSetting("Show_HR").ToLower().Equals("true"))
            {
                uxHighRisk.Visible = true;
                uxHighRiskEdit.Visible = true;
                plhHRCode.Visible = true;
            }
        }
        // 38600
        if (GeneralFuncsLib.CheckOnOffModule(WebSiteConstants.AttributeRiskScoreModuleName)
                && (Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RISK_MERCHANT_CLASSIFICATION) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RISK_MERCHANT_CLASSIFICATION_MS)))
            pnlMerchantClassifications.Visible = true;
        else
            pnlMerchantClassifications.Visible = false;

    }

    #region Merchant Count

    bool _HasCounted = false;
    protected void CalculateMerchantCount()
    {
        if (int.Parse(PrimaryID) == 0 && !IsPostBack)
            return;
        int assignmentID = 0;

        assignmentID = int.Parse(PrimaryID);
        MerchantCount = GetAssignmentMerchantCount(assignmentID);
        DisplayMerchantCount();
        _HasCounted = true;
    }

    private int GetAssignmentMerchantCount(int assignmentID)
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        return WebServices.RiskServices.GetAssignmentMerchantCount(
             paramsIn, assignmentID, (int)WebSiteEnums.ParamFilterMode.Adhoc, true);
    }

    private void DisplayMerchantCount()
    {
        pnlMerchantCountOnFilter.InnerHtml = VeraCodeSolution.ValidateResponseData(
            string.Format("{0}", AS.Common.Formater.FormatData.FormatInteger(MerchantCount)));
    }

    #endregion Merchant Count

    #region Control Event

    protected void btnCalculateMerchantCount_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CountMerchant, sender);
    }

    protected void btnRefreshState_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.BindState, sender);
    }

    protected void btnRefreshZip_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.BindZip, sender);
    }

    protected void btnRefreshSIC_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.BindSIC, sender);
    }

    protected void btnRefreshProfile_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.BindProfile, sender);
    }

    protected void btnRefreshMerchantRank_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.BindMerchantRank, sender);
    }

    protected void btnRefreshACHHoldDays_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.BindACHHoldDays, sender);
    }

    protected void btnRefreshOwnerLastName_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.BindOwnerLastName, sender);
    }

    protected void btnRefreshZipCode_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ZipCode, sender);
    }

    protected void btnRefreshMerchantFundingStatus_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.MerchantFundingStatus, sender);
    }

    protected void btnRefreshHighRisk_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.BindHRCode, sender);
    }

    protected void btnRefreshHierarchy_Command(object sender, CommandEventArgs e)
    {
        _currentHierarchyFilterMode = e.CommandArgument.ToString();
        OnPostBackActions(PostBackAction.BindHierarchyFilter, sender);
    }

    protected void btnbtnRefreshMC_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.MerchantClassifications, sender);
    }

    #endregion Control Event

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode
    {
        get;
        set;
    }

    public string PrimaryID
    {
        get;
        set;
    }

    public string ParamID
    {
        get;
        set;
    }

    public string FilterID
    {
        get;
        set;
    }

    public int Save()
    {
        SaveMerchantFilter();
        if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.CreaditCore))
        {
            SaveCreditScore();
        }

        uxApprovalDate.Save();
        uxFirstBatchDate.Save();
        uxTransactionalFilter.Save();
        uxECommerceFilter.Save();
        Risk_ExtendFilter.Save();
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion IRiskParamFilter Members

    public string GetItemIndexAsString(int index)
    {
        return index < 10 ? "0" + index.ToString() : index.ToString();
    }

    #region Validate

    public void InitValidation()
    {
        txtMerchantNumberMsg.ShowOnLoad = false;
    }

    #endregion Validate

    #endregion Protected Methods

    #region Private Methods

    private void SetWatchStatus()
    {
        int watchStatus =
            _merchantFilterInfo.Rows[0]["IsMerchantsOnWatch"] == DBNull.Value ?
            (int)WebSiteEnums.WatchStatus.Off
            : Convert.ToInt32(_merchantFilterInfo.Rows[0]["IsMerchantsOnWatch"]);

        uxWatchStatus.SetWatchStatus(watchStatus);
    }

    private static string GetHierarchyFilterValue(string hierarchyFilterMode, string entityID)
    {
        //DEFAULT
        string hierarchyFilterValue = entityID;
        //special cases
        if (hierarchyFilterMode == HierarchyMode.HEADQUARTER)
        {

            FilterParameterCollection pIn = new FilterParameterCollection();
            pIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            hierarchyFilterValue = WebServices.RiskServices.GetRealHierarchyFilterValue(
                pIn, hierarchyFilterMode, entityID, true);
        }
        return hierarchyFilterValue;
    }

    private void BindHierarchyLabel(Literal ltr, string hierarchyFilterMode)
    {
        var data = UserControls_rm_MCF_FilterHierarchy.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID, hierarchyFilterMode);
        ltr.Text = BuildHierarchyStateProfileLabel(data["Label"], ViewMoreModalType.LoadHierarchys,
            hierarchyFilterMode);
    }

    /// <summary>
    /// If ModalType = LoadHierarchys, then we must pass hierarchyFilterMode value
    /// In other cases, we pass empty value.
    /// </summary>
    private string BuildHierarchyStateProfileLabel(string data, ViewMoreModalType modalType,
        string hierarchyFilterMode)
    {
        string localQueryString = string.Empty;
        if (data.Length > MaxLengthViewMore)
        {
            localQueryString = BuildQueryStringForViewMoreModal(modalType, hierarchyFilterMode);
            data = data.Substring(0, MaxLengthViewMore)
                + string.Format(
                    "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?{0}', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>",
                    localQueryString);
        }
        return data;
    }

    private string BuildQueryStringForViewMoreModal(ViewMoreModalType modalType,
        string hierarchyMode)
    {
        string localQueryString = string.Empty;
        if (modalType == ViewMoreModalType.LoadHierarchys)
        {
            localQueryString = Page.BuildSecureQueryString(string.Format(
                "primaryid={0}&mode={1}&typemodal={2}&hierarchyMode={3}",
                PrimaryID,
                Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc
                                                           : (int)WebSiteEnums.ParamFilterMode.Assignment,
                ((int)modalType).ToString(),
                hierarchyMode)
            );
        }
        else if (modalType == ViewMoreModalType.LoadProfiles)
        {
            localQueryString = Page.BuildSecureQueryString(string.Format(
                "primaryid={0}&mode={1}&typemodal={2}",
                PrimaryID,
                Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc
                                                           : (int)WebSiteEnums.ParamFilterMode.Assignment,
                ((int)modalType).ToString())
            );
        }
        else if (modalType == ViewMoreModalType.LoadHighRisk)
        {
            localQueryString = Page.BuildSecureQueryString(string.Format(
                    "primaryid={0}&mode={1}&typemodal={2}",
                    PrimaryID,
                    Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc
                                                               : (int)WebSiteEnums.ParamFilterMode.Assignment,
                    ((int)modalType).ToString())
                );
        }
        else if (modalType == ViewMoreModalType.LoadMerchantClassifications)
        {
            localQueryString = Page.BuildSecureQueryString(string.Format(
                    "primaryid={0}&mode={1}&typemodal={2}",
                    PrimaryID,
                    Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc
                                                               : (int)WebSiteEnums.ParamFilterMode.Assignment,
                    ((int)modalType).ToString())
                );
        }
        return localQueryString;
    }

    private void BindStateLabel()
    {
        string data = UserControls_rm_MCF_FilterState.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID)["Label"];
        lblState.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadStates, string.Empty);
    }

    private void BindProfileLabel()
    {
        string data = UserControls_rm_MCF_FilterProfile.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID)["Label"];
        lblProfile.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadProfiles, string.Empty);
    }

    private void BindMerchantRankLabel()
    {
        string data = UserControls_rm_MCF_FilterMerchantRank.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID)["Label"];
        lbMerchantRank.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindACHHoldDaysLabel()
    {
        string data = UserControls_rm_MCF_FilterACHHoldDays.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID)["Label"];
        lbAchHoldDays.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindOwnerLastNameLabel()
    {
        string data = UserControls_rm_MCF_FilterOwnerLastName.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID)["Label"];
        lbOwnerLastName.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindZipCodeLabel()
    {
        string data = UserControls_rm_MCF_FilterZipCode.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID)["Label"];
        lbZipCode.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindMerchantFundingStatusLabel()
    {
        string data = UserControls_rm_MCF_MerchantFundingStatus.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID)["Label"];
        lbMerchantFundingStatus.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindMerchantClassificationsLabel()
    {
        var uxMerchantClassificationsControl = new UserControls_rm_MCF_Filter_MerchantClassifications_Modal();
        string data = uxMerchantClassificationsControl.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID)["Label"];

        uxMerchantClassificationsLabel.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadMerchantClassifications, string.Empty);
    }


    private void BindHRCodeLabel()
    {
        UserControls_rm_MCF_Filter_HRCode_Modal modal = new UserControls_rm_MCF_Filter_HRCode_Modal();
        string data = modal.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID)["Label"];
        lblHRCode.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHighRisk, string.Empty);
    }

    private void BindSICLabel()
    {
        bool isViewMore = false;
        string fullData = string.Empty;
        string data = UserControls_rm_MCF_FilterSIC.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID, out isViewMore, out fullData)["Label"];
        if (isViewMore)
        {
            data += "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal1('rm_MCF_Filter_SIC_ModalVM.aspx', 1000, 506);\" >" + GetLocalResourceObject("Risk_Assignment_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>";
        }
        lblSIC.Text = data;
    }

    private void BindGverifyCodeLabel()
    {
        string data = VeraCodeSolution.DoVeraCode(
            GetSelectedValuesAsString(
                Mode, PrimaryID, WebSiteEnums.Filter_Extend.GverifyCode));
        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            lblGverifyCode.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lblGverifyCode.Attributes.Remove("tracking-more");
        }
        lblGverifyCode.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }
    protected void btnRefreshGverifyCode_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.GverifyCode, sender);
    }

    private void BindGauthenticateCodeLabel()
    {
        string data = VeraCodeSolution.DoVeraCode(
            GetSelectedValuesAsString(
                Mode, PrimaryID, WebSiteEnums.Filter_Extend.GauthenticateCode));
        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            lblGauthenticateCode.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lblGauthenticateCode.Attributes.Remove("tracking-more");
        }
        lblGauthenticateCode.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    protected void btnGauthenticateCode_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.GauthenticateCode, sender);
    }

    private void BindG2CompassLabel()
    {
        string data = VeraCodeSolution.DoVeraCode(
            GetSelectedValuesAsString(
                Mode, PrimaryID, WebSiteEnums.Filter_Extend.G2Compass));
        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            lblG2Compass.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lblG2Compass.Attributes.Remove("tracking-more");
        }
        lblG2Compass.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private string GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, WebSiteEnums.Filter_Extend itemCode)
    {
        bool gverifyCodeRankIncluded = false;

        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.AddLanguageID();
        paramsIn.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Mode", (int)mode, DbType.Int32));
        paramsIn.Add(new FilterParameter("@ItemCode", itemCode.ToString(), DbType.String));

        DataSet ds = WebServices.RiskServices.GetReportsAsDataSet("spa_RM_MCF_Get_FilterItemValue", paramsIn);

        if (ds.Tables[1].Rows.Count > 0)
            gverifyCodeRankIncluded = ds.Tables[1].Rows[0]["IsIncluded"].ToBoolean();

        string itemString = string.Empty;
        switch (itemCode)
        {
            case WebSiteEnums.Filter_Extend.GverifyCode:
                itemString = GetLocalResourceObject("lblGverifyCode").ToString();
                break;
            case WebSiteEnums.Filter_Extend.GauthenticateCode:
                itemString = GetLocalResourceObject("lblGauthenticateCode").ToString();
                break;
            case WebSiteEnums.Filter_Extend.G2Compass:
                itemString = GetLocalResourceObject("lblG2Compass").ToString();
                break;
        }

        return BuildeSelectedValues(ds.Tables[0], gverifyCodeRankIncluded, itemString);
    }

    private string BuildeSelectedValues(DataTable dt, bool isInclude, string itemString)
    {
        string MESSAGE_MODE = "<b>" + GetLocalResourceObject("Risk_ascx_cs_MessageCode").ToString() + "</b><br />";
        string _includedIn = GetLocalResourceObject("Risk_Filter_ascx_cs_IncludedIn").ToString();
        string _excludedFrom = GetLocalResourceObject("Risk_Filter_ascx_cs_ExcludedFrom").ToString();
        string _defaultValueNA = GetLocalResourceObject("Risk_Filter_ascx_cs_NA").ToString();

        StringBuilder str = new StringBuilder();

        if (dt != null && dt.Rows.Count > 0)
        {
            if (isInclude)
                str.Append(string.Format(MESSAGE_MODE, itemString, _includedIn));
            else
                str.Append(string.Format(MESSAGE_MODE, itemString, _excludedFrom));

            str.Append("\n");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str.Append(dt.Rows[i]["DataText"] + ", ");
            }
        }        

        return string.IsNullOrEmpty(str.ToString()) ? _defaultValueNA : str.ToString().Trim().TrimEnd(",".ToCharArray());
    }

    protected void btnG2Compass_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.G2CompassAutoApprovalIndicator, sender);
    }
    private DataTable GetDataTable()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@AssignmentID", Int32.Parse(this.PrimaryID), DbType.Int32));
        parameters.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_GetAssignmentValueFilter", parameters);
    }


    protected void BindFilterExtend()
    {
        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.GverifyCode))
        {
            OnDataBindControls(DataBindAction.GverifyCode, null);
            pnlGverifyCode.Visible = true;
        }
        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.GauthenticateCode))
        {
            OnDataBindControls(DataBindAction.GauthenticateCode, null);
            pnlGauthenticateCode.Visible = true;
        }
        if (GeneralFuncsLib.AssignmentFilterExtend(WebSiteEnums.Filter_Extend.G2Compass))
        {
            OnDataBindControls(DataBindAction.G2CompassAutoApprovalIndicator, null);
            pnlG2Compass.Visible = true;
        }

        Risk_ExtendFilter.Rebind();
    }
    #endregion Private Methods

    #endregion Methods
}
