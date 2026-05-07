using System;
using System.Data;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using AS.Common;
using AS.Controls.Pages;
using System.Text;
using System.Collections.Generic;
using AS.Common.Formater;
using System.Linq;
using System.Web.UI;
using AS.WS.Entities;

public partial class UserControls_rm_MCF_Assignment_Filters : GlobalUserControl, IRiskParamFilter
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
    private bool _isPortfolioAvailable = false;
    private string _groupHierarchy = string.Empty;
    private bool _isShowGroupHierarchy = false;
    private int _countColumn = 3;

    #endregion Fields

    #region Properties

    private string AssignmentIntruderQuery
    {
        get
        {
            if (_assignmentIntruderQuery.Length == 0)
                _assignmentIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { ASSIGNMENT_ID });
            return _assignmentIntruderQuery;
        }
    }

    public string QueryString
    {
        get
        {
            _queryString = Page.BuildSecureQueryString(string.Format("PrimaryID={0}&mode={1}", PrimaryID, Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment));
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
            _queryString1 = Page.BuildSecureQueryString(string.Format("PrimaryID={0}&mode={1}&codeset={2}",
                                                                       PrimaryID,
                                                                       Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
                                                                       uxMarketData.Value));
            return _queryString1;
        }
        set
        {
            _queryString1 = value;
        }
    }

    public string HierarchyFilterQueryString(string HierarchyFilterMode, string clientIDbtn)
    {
        return Page.BuildSecureQueryString(string.Format("PrimaryID={0}&mode={1}&hierarchyMode={2}&clientID={3}", PrimaryID, Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
            HierarchyFilterMode, clientIDbtn));
    }

    public int IsMerchantsOnWatch
    {
        get
        {
            return uxWatchStatus.IsMerchantsOnWatch;
        }
    }

    private bool IsAllMerchants
    {
        get
        {
            return this.chkAllMerchants.Checked;
        }
    }

    private bool IsHideAssignmentFilterHR
    {
        get
        {
            return GeneralFuncsLib.GetDataOfExtendedSetting("HIDE_ASSIGNMENT_FILTERS_HIGHRISK_HRCODE").Equals("true");
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
                ViewState[MERCHANT_COUNT] = 0;
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

    public bool IsCreateNewAssignment { get; set; }

    protected int MaxLengthViewMore
    {
        get
        {
            return Convert.ToInt32(WebSiteSettings.RiskViewMore);
        }
    }

    public string STATE_LABEL_TEXT
    {
        get
        {
            return GeneralFuncsLib.GetDataOfExtendedSetting("STATE_LABEL_TEXT") != string.Empty ? GetLocalResourceObject("Risk_Assignment_Filter_ascx_State_Territory_Province").ToString() : GeneralFuncsLib.GetDataOfExtendedSetting("STATE_LABEL_TEXT");
        }
    }
    protected bool IsFirstLoad
    {
        get
        {
            if (ViewState["IsFirstLoad"] == null)
                ViewState["IsFirstLoad"] = true;
            return (bool)ViewState["IsFirstLoad"];
        }
        set
        {
            ViewState["IsFirstLoad"] = value;
        }
    }

    public string AssignmentType
    {
        get
        {
            return ViewState["AssignmentType"].ToString();
        }
        set
        {
            ViewState["AssignmentType"] = value;
        }
    }    
    #endregion

    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        uxApprovalDate.PrimaryID = PrimaryID;
        uxApprovalDate.Mode = Mode;
        uxApprovalDate.FeatureMode = FeatureMode;

        uxFirstBatchDate.PrimaryID = PrimaryID;
        uxFirstBatchDate.Mode = Mode;
        uxFirstBatchDate.FeatureMode = FeatureMode;

        Risk_ExtendFilter.PrimaryID = PrimaryID;
        Risk_ExtendFilter.Mode = Mode;
        Risk_ExtendFilter.FeatureMode = FeatureMode;

        RiskStandardFilter.PrimaryID = PrimaryID;
        RiskStandardFilter.Mode = Mode;
        RiskStandardFilter.FeatureMode = FeatureMode;

        //set default values for transactional filter
        uxTransactionalFilter.PrimaryID = PrimaryID;
        uxTransactionalFilter.Mode = Mode;
        uxTransactionalFilter.FeatureMode = FeatureMode;

        //set default values for transactional filter
        uxCustomFilter.PrimaryID = PrimaryID;
        uxCustomFilter.Mode = Mode;
        uxCustomFilter.FeatureMode = FeatureMode;
        //end

        uxEcommerce.PrimaryID = PrimaryID;
        uxEcommerce.Mode = Mode;
        uxEcommerce.FeatureMode = FeatureMode;

        uxSourceHierarchy.PrimaryID = PrimaryID;
        uxSourceHierarchy.Mode = Mode;
        uxSourceHierarchy.FeatureMode = FeatureMode;

        uxPauseMerchantAlert.PrimaryID = PrimaryID;
        uxPauseMerchantAlert.FeatureMode = FeatureMode;
        uxPauseMerchantAlert.IsCreateNewAssignment = IsCreateNewAssignment;

        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            pnlMerchantClassificationsEdit.Visible = false;
        }
        
        VisibleControls();        

        //ajaxify each literal control on repeater.
        foreach (RepeaterItem item in uxHierarchyFilterRepeater.Items)
        {
            Panel div = item.FindControl("divHierarchy") as Panel;
            Button btn = item.FindControl("btnRefreshHierarchy") as Button;

            RadAjaxManagerProxyReview.AjaxSettings.AddAjaxSetting(btn, div);
            RadAjaxManagerProxyReview.AjaxSettings.AddAjaxSetting(btn, hdnCountSelectedFilter);
        }
    }

    public void Rebind()
    {
        //set default filter for MS user
        switch (SessionManager.CurrentUserType)
        {
            case WebSiteEnums.UserHierarchyMode.Hierarchy:
            case WebSiteEnums.UserHierarchyMode.Headquarter:
                {
                    if (!string.IsNullOrEmpty(AssignmentType) 
                        && AssignmentType != ((int)WebSiteEnums.AssignmentType.Subsite).ToString())
                    {
                        string hierarchyFilterMode = GeneralFuncsLib.GetHierarchyInfo(
                        SessionManager.CurrentUser.EntityType).HierarchyMode;
                        string entityID = SessionManager.CurrentUser.EntityID;
                        string hierarchyFilterValue = GetHierarchyFilterValue(
                            hierarchyFilterMode, entityID);

                        FilterParameterCollection paramsIn = new FilterParameterCollection();
                        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        WebServices.RiskServices.SaveHierarchyFilter(paramsIn,
                            PrimaryID, hierarchyFilterMode, hierarchyFilterValue,
                            (int)WebSiteEnums.ParamFilterMode.Assignment, true);
                    }                    
                }
                break;
        }

        CalculateMerchantCount();
        GetMerchantFilterInfo();

        OnDataBindControls(DataBindAction.BindState, null);
        OnDataBindControls(DataBindAction.BindSIC, null);
        OnDataBindControls(DataBindAction.BindProfile, null);
        OnDataBindControls(DataBindAction.BindHierarchyFilter, null);
        if (GeneralFuncsLib.CheckOnOffModule(WebSiteConstants.AttributeRiskScoreModuleName))
            OnDataBindControls(DataBindAction.MerchantClassifications, null);

        BindFilterExtend();
        // 48539
        pnlProfile.Visible = !GeneralFuncsLib.GetDataOfExtendedSetting("HIDE_ASSIGNMENT_FILTERS_DEMOGRAPHIC_PROFILE").Equals("true");
        pnlHRCode.Visible = !IsHideAssignmentFilterHR;
        //36801 – MCPS – VW Supplemental MIF – FE - Enabled
        BindSupplemetal();
        //End

        if (!IsHideAssignmentFilterHR)
        {
            OnDataBindControls(DataBindAction.BindHRCode, null);
        }
        CountSelectedFilter();

        VisibleControls();

        uxApprovalDate.Rebind();
        uxFirstBatchDate.Rebind();
        uxTransactionalFilter.Rebind();

        if(SessionManager.CurrentClient == WebSiteConstants.MVRK_CLIENT) { 
            uxCustomFilter.Rebind();
        }
        Risk_ExtendFilter.Rebind();
        RiskStandardFilter.Rebind();
        uxEcommerce.Rebind();
        uxSourceHierarchy.Rebind();
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
            DataTable dt = GeneralFuncsLib.GetCreditScoreByFilter(PrimaryID.ToInt(), WebSiteEnums.ParamFilterMode.Assignment, true);
            DataTable dataCredit = GeneralFuncsLib.GetRiskCreditScore(PrimaryID.ToInt(), WebSiteEnums.AssignmentFilterModes.All, WebSiteEnums.ParamFilterMode.Assignment, true);
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
                        uxCbCreditScoreTo.Enabled = true;
                    }
                    uxCbCreditScoreTo.SelectedValue = creditScoreTo;
                }
                if (!creditScoreFrom.IsNullOrEmpty() && !creditScoreFrom.Equals("0"))
                {
                    uxChkIsFrom.Checked = true;
                    if (uxChkIsFrom.Checked)
                    {
                        uxCbCreditScoreFrom.Enabled = true;
                    }
                    uxCbCreditScoreFrom.SelectedValue = creditScoreFrom;
                }
            }
        }
    }

    protected void CountSelectedFilter()
    {
        int count = 0;
        foreach (RepeaterItem item in uxHierarchyFilterRepeater.Items)
        {
            Label ltr = (Label)item.FindControl("lblHierarchy");
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
                        ((Label)e.Item.FindControl("uxGroupHierarchyTitleView")).Text =
                            VeraCodeSolution.DoVeraCode(row["BEProcessor"].ToString().TrimEnd() + " - " + GetLocalResourceObject("Risk_Assignment_Filters_ascx_cs_Hierarchy").ToString());
                    }
                    else
                    {
                        ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyView")).Visible = false;
                        ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyEdit")).Visible = true;
                        string hierarchyBE = row["BEProcessor"].ToString().TrimEnd();
                        string hierarchyLocal = GetLocalResourceObject("Risk_Assignment_Filters_ascx_cs_Hierarchy").ToString();
                        ((Literal)e.Item.FindControl("uxGroupHierarchyTitleEdit")).Text =
                            VeraCodeSolution.DoVeraCode(SessionManager.CurrentClient != 142 ? (hierarchyBE + " - " + hierarchyLocal) : hierarchyLocal);
                    }
                }
                else
                {
                    ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyEdit")).Visible = false;
                    ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyView")).Visible = false;

                }
                _isShowGroupHierarchy = false;
                BindHierarchyLabel((Label)e.Item.FindControl("lblHierarchy"), hierarchyFilterMode);
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.BindHierarchyFilter:
                Button btn = (Button)sender;
                BindHierarchyLabel((Label)btn.Parent.FindControl("lblHierarchy"), _currentHierarchyFilterMode);
                break;
            case PostBackAction.BindState:
                BindStateLabel();
                break;
            case PostBackAction.BindSIC:
                BindSICLable();
                break;
            case PostBackAction.BindProfile:
                BindProfileLabel();
                break;
            case PostBackAction.BindHRCode:
                BindHRCodeLabel();
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
            case PostBackAction.GverifyCode:
                BindGverifyCodeLabel();
                break;
            case PostBackAction.GauthenticateCode:
                BindGauthenticateCodeLabel();
                break;
            case PostBackAction.G2CompassAutoApprovalIndicator:
                BindG2CompassLabel();
                break;

            case PostBackAction.CountMerchant:
                int error = Save();
                if (GeneralFuncsLib.EnableSupplementalFeature(WebSiteEnums.SUPPLEMENTAL_FEATURE.CreaditCore))
                {
                    SaveCreditScore();
                }

                if (error != 0)
                {
                    return;
                }
                CalculateMerchantCount();
                (Page.Master as BaseMasterPage).AjaxAddResponseScript("MerchantCountConfirm(" + MerchantCount + ", 1000);");
                break;
        }
        CountSelectedFilter();
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
            case DataBindAction.BindState:
                BindStateLabel();
                break;
            case DataBindAction.BindSIC:
                BindSICLable();
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
            case DataBindAction.BindHRCode:
                BindHRCodeLabel();
                break;
            case DataBindAction.BindHierarchyFilter:
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.AddLanguageID();
                SessionManager.RiskHierarchyFilter = WebServices.RiskServices.GetHierarchyFilterList(parameters, true);
                DataTable info = SessionManager.RiskHierarchyFilter;
                uxHierarchyFilterRepeater.DataSource = info;
                uxHierarchyFilterRepeater.DataBind();
                break;
            case DataBindAction.MerchantClassifications:
                BindMerchantClassificationsLabel();
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
    }

    /// <summary>
    /// Bind data for Merchant Range: From, To
    /// </summary>
    private void BindMerchantRange()
    {
        if (_merchantFilterInfo != null && _merchantFilterInfo.Rows.Count > 0)
        {
            SetWatchStatus();
            chkAllMerchants.Checked = _merchantFilterInfo.Rows[0]["IsAllMerchants"] != DBNull.Value
                && Convert.ToBoolean(_merchantFilterInfo.Rows[0]["IsAllMerchants"]);

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
            paramsIn, int.Parse(PrimaryID), null, true);
        BindMerchantRange();
    }

    public int SaveMerchantFilter()
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        AssignmentMerchantFilterModel assignmentMerchantFilterModel = new AssignmentMerchantFilterModel()
        {
            AssignmentId = int.Parse(PrimaryID),
            IsMerchantsOnWatch = IsMerchantsOnWatch,
            IsAllMerchants = IsAllMerchants,
            ReportDate = null,
            MerchantNumber = null,
            Mode = null
        };
        WebServices.RiskServices.SaveAssignmentMerchantFilter(paramsIn, assignmentMerchantFilterModel, true, chkExcludeMerchantsClosedStatus.Checked);

        return 0;
    }


    //TK36801 – MCPS – VW Supplemental MIF – FE
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
        plhState.Visible = plhProfile.Visible = plhSIC.Visible = plhRefesh.Visible =
            plhMerchantRank.Visible = plhAchHoldDay.Visible = plhOwnerLastName.Visible =
            plhZipCode.Visible = plhMerchantFunding.Visible = plhCreditScore.Visible = uxChkIsFrom.Enabled = uxChkIsTo.Enabled =
            pnlG2CompassEdit.Visible = pnlGauthenticateCodeEdit.Visible = pnlGverifyCodeEdit.Visible =
            FeatureMode == WebSiteEnums.FeatureMode.Edit;

        foreach (RepeaterItem item in uxHierarchyFilterRepeater.Items)
        {
            PlaceHolder HierarchyPlaceHolder = item.FindControl("plhHierarchy") as PlaceHolder;
            HierarchyPlaceHolder.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;
        }

        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            uxWatchStatus.RejectOnClickEventForRadioWatchStatus();
            chkAllMerchants.Enabled = false;
            chkExcludeMerchantsClosedStatus.Enabled = false;

            if (SessionManager.CurrentClient != 23)  // Show for all clients except MPS
            {
                uxGroupDemographicView.Visible = true;
            }
            
            uxCustomFilter.Visible = SessionManager.CurrentClient == WebSiteConstants.MVRK_CLIENT;

            if (GeneralFuncsLib.GetDataOfExtendedSetting("Show_HR").ToLower().Equals("true"))
            {
                uxHighRisk.Visible = true;
                uxHighRiskView.Visible = true;
            }

            btnAddPauseMerchantAlert.Visible = false;
        }
        else
        {
            if (SessionManager.CurrentClient != 23) // Show for all clients except MPS
            {
                uxGroupDemographicEdit.Visible = true;
            }

            uxCustomFilter.Visible = SessionManager.CurrentClient == WebSiteConstants.MVRK_CLIENT;

            if (GeneralFuncsLib.GetDataOfExtendedSetting("Show_HR").ToLower().Equals("true"))
            {
                uxHighRisk.Visible = true;
                uxHighRiskEdit.Visible = true;
                plhHRCode.Visible = true;
            }
        }

        var isShowCheckAllMerchant = GeneralFuncsLib.GetDataOfExtendedSetting("HasCheckAllMerchantOnAssignment");
        if (string.IsNullOrEmpty(isShowCheckAllMerchant))
        {
            DataTable dt = SessionManager.RiskHierarchyFilter;
            if (dt != null && dt.Rows.Count > 0)
            {
                var hasPorfolio = dt.Rows.Cast<DataRow>().FirstOrDefault(x => x["HierarchyFilterMode"].ToString().Equals("PORTFOLIO", StringComparison.OrdinalIgnoreCase));
                plhAllMerchants.Visible = hasPorfolio == null;
            }            
        }
        else
        {
            plhAllMerchants.Visible = isShowCheckAllMerchant.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
        
        // 44810 - VW - Attribute Risk Score Section - Modify Permissions to View-FE - 2019/02/11
        // 38600
        if (GeneralFuncsLib.CheckOnOffModule(WebSiteConstants.AttributeRiskScoreModuleName))
            pnlMerchantClassifications.Visible = true;
        else
            pnlMerchantClassifications.Visible = false;

        if(FeatureMode != WebSiteEnums.FeatureMode.View)
            InitControlForSubsite();
    }

    #region Merchant Count

    bool _HasCounted = false;

    protected void CalculateMerchantCount()
    {
        if (int.Parse(PrimaryID) == 0 && !IsPostBack)
            return;
        int assignmentID = 0;

        assignmentID = int.Parse(PrimaryID);

        if (Page.SecureQueryString["FeatureMode"] == null && IsFirstLoad)
        {
            MerchantCount = 0;
            IsFirstLoad = false;
        }
        else
            MerchantCount = GetAssignmentMerchantCount(assignmentID);

        var formatedMerchantCount = FormatData.FormatInteger(MerchantCount);
        pnlMerchantCountOnFilter.InnerHtml = VeraCodeSolution.ValidateResponseData(formatedMerchantCount);
        _HasCounted = true;
    }

    private int GetAssignmentMerchantCount(int assignmentID)
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        return WebServices.RiskServices.GetAssignmentMerchantCount(
            paramsIn, assignmentID, (int)WebSiteEnums.ParamFilterMode.Assignment, true);
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

    protected void btnRefreshHighRisk_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.BindHRCode, sender);
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
        uxApprovalDate.Save();
        uxFirstBatchDate.Save();
        uxTransactionalFilter.Save();
        if (SessionManager.CurrentClient == WebSiteConstants.MVRK_CLIENT) { 
            uxCustomFilter.Save();
        }
        uxEcommerce.Save();
        Risk_ExtendFilter.Save();
        RiskStandardFilter.Save();
        return 0;
    }

    public int SavePauseMerchantAlert()
    {
        uxPauseMerchantAlert.Save();
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion  IRiskParamFilter Members

    public string GetItemIndexAsString(int index)
    {
        return index < 10 ? string.Format("0{0}", index.ToString()) : index.ToString();
    }

    #region Private Methods

    private void SetWatchStatus()
    {
        int watchStatus =
            _merchantFilterInfo.Rows[0]["IsMerchantsOnWatch"] == DBNull.Value ?
            (int)WebSiteEnums.WatchStatus.Off : Convert.ToInt32(_merchantFilterInfo.Rows[0]["IsMerchantsOnWatch"]);
        uxWatchStatus.SetWatchStatus(watchStatus);
    }

    private string GetHierarchyFilterValue(string hierarchyFilterMode, string entityID)
    {
        // Default cases
        string hierarchyFilterValue = entityID;

        // Special cases
        if (hierarchyFilterMode == HierarchyMode.HEADQUARTER)
        {

            FilterParameterCollection pIn = new FilterParameterCollection();
            pIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            hierarchyFilterValue = WebServices.RiskServices.GetRealHierarchyFilterValue(
                pIn, hierarchyFilterMode, entityID, true);
        }
        return hierarchyFilterValue;
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
        else if (modalType == ViewMoreModalType.LoadStates)
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
                    "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?{0}', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_Filters_ascx_cs_ViewMore").ToString() + "</a>",
                    localQueryString);
        }
        return data;
    }

    private void BindSICLable()
    {
        bool isViewMore = false;
        string fullData = string.Empty;
        Dictionary<string, string> result = UserControls_rm_MCF_FilterSIC.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID, out isViewMore, out fullData);

        string data = result["Label"];
        lblSIC.Attributes.Add("tracking-value", result["Tracking"]);

        if (isViewMore)
        {
            data += "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal1('rm_MCF_Filter_SIC_ModalVM.aspx', 1000, 506);\" >" + GetLocalResourceObject("Risk_Assignment_Filters_ascx_cs_ViewMore").ToString() + "</a>";
            lblSIC.Attributes.Add("tracking-more", fullData);
        }
        else
            lblSIC.Attributes.Remove("tracking-more");
        lblSIC.Text = data;
    }

    private void BindHierarchyLabel(Label ltr, string hierarchyFilterMode)
    {
        Dictionary<string, string> dataItem = UserControls_rm_MCF_FilterHierarchy.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID, hierarchyFilterMode);

        string data = dataItem["Label"];
        ltr.Attributes.Add("tracking-value", dataItem["Tracking"]);

        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            ltr.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            ltr.Attributes.Remove("tracking-more");
        }
        ltr.Text = BuildHierarchyStateProfileLabel(data, ViewMoreModalType.LoadHierarchys,
            hierarchyFilterMode);
    }

    private void BindProfileLabel()
    {
        Dictionary<string, string> result = UserControls_rm_MCF_FilterProfile.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID);

        string data = result["Label"];
        lblProfile.Attributes.Add("tracking-value", result["Tracking"]);

        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            lblProfile.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lblProfile.Attributes.Remove("tracking-more");
        }
        lblProfile.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadProfiles, string.Empty);
    }

    private void BindStateLabel()
    {
        Dictionary<string, string> result = UserControls_rm_MCF_FilterState.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID);

        string data = result["Label"];

        lblState.Attributes.Add("tracking-value", result["Tracking"]);

        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            lblState.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lblState.Attributes.Remove("tracking-more");
        }
        lblState.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadStates, string.Empty);
    }

    private void BindHRCodeLabel()
    {
        UserControls_rm_MCF_Filter_HRCode_Modal modal = new UserControls_rm_MCF_Filter_HRCode_Modal();
        Dictionary<string, string> result = modal.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID);

        string data = result["Label"];

        lblHRCode.Attributes.Add("tracking-value", result["Tracking"]);

        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            lblHRCode.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lblHRCode.Attributes.Remove("tracking-more");
        }
        lblHRCode.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHighRisk, string.Empty);
    }

    private void BindMerchantRankLabel()
    {
        Dictionary<string, string> result = UserControls_rm_MCF_FilterMerchantRank.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID);

        string data = result["Label"];

        lbMerchantRank.Attributes.Add("tracking-value", result["Tracking"]);

        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            lbMerchantRank.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lbMerchantRank.Attributes.Remove("tracking-more");
        }
        lbMerchantRank.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindACHHoldDaysLabel()
    {
        Dictionary<string, string> result = UserControls_rm_MCF_FilterACHHoldDays.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID);

        string data = result["Label"];

        lbAchHoldDays.Attributes.Add("tracking-value", result["Tracking"]);

        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            lbAchHoldDays.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lbAchHoldDays.Attributes.Remove("tracking-more");
        }
        lbAchHoldDays.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindOwnerLastNameLabel()
    {
        Dictionary<string, string> result = UserControls_rm_MCF_FilterOwnerLastName.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID);

        string data = result["Label"];

        lbOwnerLastName.Attributes.Add("tracking-value", result["Tracking"]);

        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            lbOwnerLastName.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lbOwnerLastName.Attributes.Remove("tracking-more");
        }
        lbOwnerLastName.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindZipCodeLabel()
    {
        Dictionary<string, string> result = UserControls_rm_MCF_FilterZipCode.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID);

        string data = result["Label"];

        lbZipCode.Attributes.Add("tracking-value", result["Tracking"]);

        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            lbZipCode.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lbZipCode.Attributes.Remove("tracking-more");
        }
        lbZipCode.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindMerchantFundingStatusLabel()
    {
        Dictionary<string, string> result = UserControls_rm_MCF_MerchantFundingStatus.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID);

        string data = result["Label"];

        lbMerchantFundingStatus.Attributes.Add("tracking-value", result["Tracking"]);

        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            lbMerchantFundingStatus.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            lbMerchantFundingStatus.Attributes.Remove("tracking-more");
        }
        lbMerchantFundingStatus.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindGverifyCodeLabel()
    {
        Dictionary<string, string> result = GetSelectedValuesAsString(
                Mode, PrimaryID, WebSiteEnums.Filter_Extend.GverifyCode);

        string data = result["Label"];

        lblGverifyCode.Attributes.Add("tracking-value", result["Tracking"]);

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

    private void BindGauthenticateCodeLabel()
    {
        Dictionary<string, string> result = GetSelectedValuesAsString(
               Mode, PrimaryID, WebSiteEnums.Filter_Extend.GauthenticateCode);

        string data = result["Label"];
        lblGauthenticateCode.Attributes.Add("tracking-value", result["Tracking"]);

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

    private void BindG2CompassLabel()
    {
        Dictionary<string, string> result = GetSelectedValuesAsString(
               Mode, PrimaryID, WebSiteEnums.Filter_Extend.G2Compass);

        string data = result["Label"];
        lblG2Compass.Attributes.Add("tracking-value", result["Tracking"]);

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

    private void BindMerchantClassificationsLabel()
    {
        UserControls_rm_MCF_Filter_MerchantClassifications_Modal modal = new UserControls_rm_MCF_Filter_MerchantClassifications_Modal();
        Dictionary<string, string> result = modal.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID);

        string data = result["Label"];
        uxMerchantClassificationsLabel.Attributes.Add("tracking-value", result["Tracking"]);

        if (data.Length > MaxLengthViewMore)
        {
            var temp = data.Split(new string[] { "<br />" }, StringSplitOptions.None);
            uxMerchantClassificationsLabel.Attributes.Add("tracking-more", temp.Length > 1 ? temp[1].Trim() : data);
        }
        else
        {
            uxMerchantClassificationsLabel.Attributes.Remove("tracking-more");
        }
        uxMerchantClassificationsLabel.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadMerchantClassifications, string.Empty);
    }

    private Dictionary<string, string> GetSelectedValuesAsString(WebSiteEnums.ParamFilterMode mode, string primaryID, WebSiteEnums.Filter_Extend itemCode)
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

    private Dictionary<string, string> BuildeSelectedValues(DataTable dt, bool isInclude, string itemString)
    {
        string MESSAGE_MODE = "<b>" + GetLocalResourceObject("Risk_ascx_cs_MessageCode").ToString() + "</b><br />";
        string _includedIn = GetLocalResourceObject("Risk_Filter_ascx_cs_IncludedIn").ToString();
        string _excludedFrom = GetLocalResourceObject("Risk_Filter_ascx_cs_ExcludedFrom").ToString();
        string _defaultValueNA = GetLocalResourceObject("Risk_Filter_ascx_cs_NA").ToString();

        Dictionary<string, string> data = new Dictionary<string, string>();
        StringBuilder str = new StringBuilder();
        var trackingValue = new StringBuilder();

        if (dt != null && dt.Rows.Count > 0)
        {
            if (isInclude)
            {
                str.Append(string.Format(MESSAGE_MODE, itemString, _includedIn));
                trackingValue.Append(RM_MCF_GeneralFuncsLib.INCLUDED_KEY);
            }
            else
            {
                str.Append(string.Format(MESSAGE_MODE, itemString, _excludedFrom));
                trackingValue.Append(RM_MCF_GeneralFuncsLib.EXCLUDED_KEY);
            }
            str.Append("\n");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str.Append(dt.Rows[i]["DataText"] + ", ");
                trackingValue.Append(dt.Rows[i]["DataText"].ToString() + ", ");
            }
        }       

        string value = string.IsNullOrEmpty(str.ToString()) ? _defaultValueNA : str.ToString().Trim().TrimEnd(",".ToCharArray());

        var tracking = trackingValue.ToString();
        tracking = string.IsNullOrEmpty(tracking) ? _defaultValueNA : tracking.Trim().TrimEnd(",".ToCharArray());
        data.Add("Label", value);
        data.Add("Tracking", tracking);

        return data;
    }

    #endregion Private Methods

    #endregion Methods

    protected void btnRefreshGverifyCode_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.GverifyCode, sender);
    }

    protected void btnGauthenticateCode_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.GauthenticateCode, sender);
    }

    protected void btnG2Compass_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.G2CompassAutoApprovalIndicator, sender);
    }    

    public void ProcessForSubsite(bool isVisible)
    {
        uxPhatchStatus.Visible = isVisible;
        uxHierarchyFilterRepeater.Visible = isVisible;
        pnlMerchantClassifications.Visible = isVisible;
    }
    private void InitControlForSubsite()
    {
        bool isSubsite = !string.IsNullOrEmpty(AssignmentType) && AssignmentType.Equals(((int)WebSiteEnums.AssignmentType.Subsite).ToString());
        if(isSubsite)
            pnlMerchantClassifications.Visible = false;

        var currentUserType = SessionManager.CurrentUserType;
        if (currentUserType != WebSiteEnums.UserHierarchyMode.CS
            && currentUserType != WebSiteEnums.UserHierarchyMode.AS
            && isSubsite)
        {
            uxPhatchStatus.Visible = false;            
            uxApprovalDate.FeatureMode = WebSiteEnums.FeatureMode.View;
            uxApprovalDate.VisibleControls();

            uxFirstBatchDate.FeatureMode = WebSiteEnums.FeatureMode.View;
            uxFirstBatchDate.VisibleControls();
            chkExcludeMerchantsClosedStatus.Enabled = false;
            chkAllMerchants.Enabled = false;

            //demographic
            RemoveControlsInPanel(plhState);
            RemoveControlsInPanel(plhProfile);
            RemoveControlsInPanel(plhSIC);
            RemoveControlsInPanel(plhAchHoldDay);
            plhCreditScore.Visible = true;
            uxChkIsFrom.Enabled = false;
            uxCbCreditScoreFrom.Enabled = false;
            uxChkIsTo.Enabled = false;
            uxCbCreditScoreTo.Enabled = false;
            uxTransactionalFilter.DisableControls();
            if (SessionManager.CurrentClient == WebSiteConstants.MVRK_CLIENT)
            {
                uxCustomFilter.VisibleControls();
            }
        }
    }

    private void RemoveControlsInPanel(PlaceHolder placeHolder)
    {
        var disabledLink = string.Format("<td class=\"action-column\"><a href=\"#\" class=\"disabled\">{0}</a></td>", GetLocalResourceObject("Literal9Resource1.Text").ToString());
        placeHolder.Controls.Clear();
        placeHolder.Controls.Add(new LiteralControl(disabledLink));
    }

    protected void btnAddPauseMerchantAlert_Click(object sender, EventArgs e)
    {
        uxPauseMerchantAlert.AddFilter();
    }   

    public string AppendPauseMerchantAlertAuditEntities(string auditEntities)
    {
        var result = uxPauseMerchantAlert.GetAuditEntities(auditEntities);
        return result;
    }
}
