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
using AS.Common;
using AS.Controls.Pages;
using Telerik.Web.UI;
using AS.WS.Entities;

public partial class UserControls_Risk_Assignment_Filters : GlobalUserControl, IRiskParamFilter
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
        MerchantClassifications
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
        MerchantClassifications
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

    #endregion

    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        uxApprovalDate.PrimaryID = PrimaryID;
        uxApprovalDate.Mode = Mode;
        uxApprovalDate.FeatureMode = FeatureMode;
        
        //set default values for transactional filter
        uxTransactionalFilter.PrimaryID = PrimaryID;
        uxTransactionalFilter.Mode = Mode;
        uxTransactionalFilter.FeatureMode = FeatureMode;
        //end
        //
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            pnlMerchantClassificationsEdit.Visible = false;
        }

        if (!IsPostBack)
        {
            //set default filter for MS user
            switch (SessionManager.CurrentUserType)
            {
                case WebSiteEnums.UserHierarchyMode.Hierarchy:
                case WebSiteEnums.UserHierarchyMode.Headquarter:
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
                            (int)WebSiteEnums.ParamFilterMode.Assignment);
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

            //36801 – MCPS – VW Supplemental MIF – FE - Enabled
            BindSupplemetal();
            //End

            if (SessionManager.CurrentClient.Equals(WebSiteConstants.IPMT_CLIENT))
            {
                OnDataBindControls(DataBindAction.BindHRCode, null);
            }
            CountSelectedFilter();
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
            DataTable dt = GeneralFuncsLib.GetCreditScoreByFilter(PrimaryID.ToInt(), WebSiteEnums.ParamFilterMode.Assignment);
            uxCbCreditScoreTo.DataSource = GeneralFuncsLib.GetRiskCreditScore(PrimaryID.ToInt(), WebSiteEnums.AssignmentFilterModes.All, WebSiteEnums.ParamFilterMode.Assignment);
            uxCbCreditScoreTo.DataBind();
            uxCbCreditScoreFrom.DataSource = GeneralFuncsLib.GetRiskCreditScore(PrimaryID.ToInt(), WebSiteEnums.AssignmentFilterModes.All, WebSiteEnums.ParamFilterMode.Assignment);
            uxCbCreditScoreFrom.DataBind();
            if (dt.HasData())
            {
                string creditScoreTo = dt.Rows[0]["CreditScoreTo"].ToString();
                string creditScoreFrom = dt.Rows[0]["CreditScoreFrom"].ToString();
                if (!creditScoreTo.IsNullOrEmpty() && !creditScoreTo.Equals("0"))
                {
                    uxChkIsTo.Checked = true;
                    uxCbCreditScoreTo.SelectedValue = creditScoreTo;
                }
                if (!creditScoreFrom.IsNullOrEmpty() && !creditScoreFrom.Equals("0"))
                {
                    uxChkIsFrom.Checked = true;
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
            Literal ltr = (Literal)item.FindControl("lblHierarchy");
            if (ltr.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            {
                count++;
            }
        }
        if (lblState.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;
        if (lblSIC.Text.ToString() != GeneralFuncsLib.NA_VALUE)
            count++;
        if (lblProfile.Text.ToString() != GeneralFuncsLib.NA_VALUE)
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
                        ((Literal)e.Item.FindControl("uxGroupHierarchyTitleView")).Text =
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
                (Page.Master as BaseMasterPage).AjaxAddResponseScript("MerchantCountConfirm(" + MerchantCount + ");");
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
                SessionManager.RiskHierarchyFilter = WebServices.RiskServices.GetHierarchyFilterList(parameters);
                DataTable info = SessionManager.RiskHierarchyFilter;
                uxHierarchyFilterRepeater.DataSource = info;
                uxHierarchyFilterRepeater.DataBind();
                break;
            case DataBindAction.MerchantClassifications:
                BindMerchantClassificationsLabel();
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

        }
    }

    public void GetMerchantFilterInfo()
    {
        //get merchant filter
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _merchantFilterInfo = WebServices.RiskServices.GetAssignmentMerchantFilter(
            paramsIn, int.Parse(PrimaryID), null);
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
        WebServices.RiskServices.SaveAssignmentMerchantFilter(paramsIn, assignmentMerchantFilterModel);

        return 0;
    }

    //TK36801 – MCPS – VW Supplemental MIF – FE
    public void SaveCreditScore()
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@PrimaryID", PrimaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@IsCreditScoreTo", uxChkIsTo.Checked, DbType.Boolean));
        paramsIn.Add(new FilterParameter("@IsCreditScoreFrom", uxChkIsFrom.Checked, DbType.Boolean));
        paramsIn.Add(new FilterParameter("@Value_CreditScoreTo", uxCbCreditScoreTo.SelectedValue, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Value_CreditScoreFrom", uxCbCreditScoreFrom.SelectedValue, DbType.Int32));
        paramsIn.Add(new FilterParameter("@FilterMode", (int)Mode, DbType.Int32));
        FilterParameterCollection paramsOut;

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_InsertUpdateCreditScore", paramsIn, out paramsOut);
    }
    //End

    private void VisibleControls()
    {
        plhState.Visible = plhProfile.Visible = plhSIC.Visible = plhRefesh.Visible =
            plhMerchantRank.Visible = plhAchHoldDay.Visible = plhOwnerLastName.Visible =
            plhZipCode.Visible = plhMerchantFunding.Visible = plhCreditScore.Visible = uxChkIsFrom.Enabled = uxChkIsTo.Enabled =
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
            if (SessionManager.CurrentClient != 23)  // Show for all clients except MPS
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
            if (SessionManager.CurrentClient != 23) // Show for all clients except MPS
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

        DataTable dt = SessionManager.RiskHierarchyFilter;
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["HierarchyFilterMode"].ToString().Equals("PORTFOLIO"))
                {
                    _isPortfolioAvailable = true;
                    break;
                }
            }
        }

        plhAllMerchants.Visible = !_isPortfolioAvailable;
        if (_isPortfolioAvailable)
        {
            //cidMerchantCount.RowSpan = 3;// when All Merchants available then RowSpan = 4
        }
        // 44810 - VW - Attribute Risk Score Section - Modify Permissions to View-FE - 2019/02/11
        // 38600
        if (GeneralFuncsLib.CheckOnOffModule(WebSiteConstants.AttributeRiskScoreModuleName))
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
            paramsIn, assignmentID, (int)WebSiteEnums.ParamFilterMode.Assignment);
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

    protected void btnRefreshHierarchy_Click(object sender, EventArgs e)
    {
        //OnPostBackActions(PostBackAction.BindHierarchy, sender)
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
        uxTransactionalFilter.Save();
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion  IRiskParamFilter Members

    #region Validate

    #endregion Validate

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
                pIn, hierarchyFilterMode, entityID);
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
                    "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_Filter_CommonViewMore_Modal.aspx?{0}', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_Filters_ascx_cs_ViewMore").ToString() + "</a>",
                    localQueryString);
        }
        return data;
    }

    private void BindSICLable()
    {
        bool isViewMore = false;
        string data = VeraCodeSolution.DoVeraCode(
            UserControls_Risk_FilterSIC.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID, out isViewMore));
        if (isViewMore)
        {
            data += "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal1('rm_Filter_SIC_ModalVM.aspx', 1000, 506);\" >" + GetLocalResourceObject("Risk_Assignment_Filters_ascx_cs_ViewMore").ToString() + "</a>";
        }
        lblSIC.Text = data;
    }

    private void BindHierarchyLabel(Literal ltr, string hierarchyFilterMode)
    {
        string data = VeraCodeSolution.DoVeraCode(
            UserControls_Risk_FilterHierarchy.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID, hierarchyFilterMode));
        ltr.Text = BuildHierarchyStateProfileLabel(data, ViewMoreModalType.LoadHierarchys,
            hierarchyFilterMode);
    }

    private void BindProfileLabel()
    {
        string data = VeraCodeSolution.DoVeraCode(
            UserControls_Risk_FilterProfile.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID));
        lblProfile.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadProfiles, string.Empty);
    }

    private void BindStateLabel()
    {
        string data = VeraCodeSolution.ValidateResponseData(
            UserControls_Risk_FilterState.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID));
        lblState.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadStates, string.Empty);
    }

    private void BindHRCodeLabel()
    {
        var HRCodePage = new UserControls_rm_Filter_HRCode_Modal();
        string data = VeraCodeSolution.DoVeraCode(
            HRCodePage.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID));
        lblHRCode.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHighRisk, string.Empty);
    }

    private void BindMerchantRankLabel()
    {
        string data = VeraCodeSolution.DoVeraCode(
            UserControls_Risk_FilterMerchantRank.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID));
        lbMerchantRank.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindACHHoldDaysLabel()
    {
        string data = VeraCodeSolution.DoVeraCode(
            UserControls_Risk_FilterACHHoldDays.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID));
        lbAchHoldDays.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindOwnerLastNameLabel()
    {
        string data = VeraCodeSolution.DoVeraCode(
            UserControls_Risk_FilterOwnerLastName.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID));
        lbOwnerLastName.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindZipCodeLabel()
    {
        string data = VeraCodeSolution.DoVeraCode(
            UserControls_Risk_FilterZipCode.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID));
        lbZipCode.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindMerchantFundingStatusLabel()
    {
        string data = VeraCodeSolution.DoVeraCode(
            UserControls_Risk_MerchantFundingStatus.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID));
        lbMerchantFundingStatus.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadHierarchys, string.Empty);
    }

    private void BindMerchantClassificationsLabel()
    {
        var uxMerchantClassificationsControl = new UserControls_rm_Filter_MerchantClassifications_Modal();
        string data = VeraCodeSolution.DoVeraCode(
            uxMerchantClassificationsControl.GetSelectedValuesAsString(
                Mode, PrimaryID, ParamID, FilterID));
        uxMerchantClassificationsLabel.Text = BuildHierarchyStateProfileLabel(data,
            ViewMoreModalType.LoadMerchantClassifications, string.Empty);
    }
    #endregion Private Methods

    #endregion Methods
}
