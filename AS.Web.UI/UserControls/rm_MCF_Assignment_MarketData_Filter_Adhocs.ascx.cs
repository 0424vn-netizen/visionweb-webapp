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
using System.Collections.Generic;

public partial class UserControls_rm_MCF_Assignment_MarketData_Filter_Adhocs : GlobalUserControl, IRiskParamFilter
{
    #region Enums
    enum DataBindAction
    {
        BindState,
        BindSIC,
        BindZip,
        BindProfile,
        BindHierarchyFilter,
        BindMarketData
    }
    enum PostBackAction
    {
        BindState,
        BindSIC,
        BindZip,
        BindProfile,
        CountMerchant,
        BindHierarchyFilter,
        BindMarketData,
        ShowModalMarketData,
    }
    #endregion

    #region Properties
    private const string ASSIGNMENT_ID = "AssignmentID";
    const string MERCHANT_COUNT = "MerchantCount";

    private string _CurrentHierarchyFilterMode;


    string _AssignmentIntruderQuery = string.Empty;
    private string AssignmentIntruderQuery
    {
        get
        {
            if (_AssignmentIntruderQuery.Length == 0)
                _AssignmentIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { ASSIGNMENT_ID });
            return _AssignmentIntruderQuery;
        }
    }

    private string _QueryString;

    public string QueryString
    {
        get
        {

            _QueryString = Page.BuildSecureQueryString(string.Format("PrimaryID={0}&mode={1}", PrimaryID, Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment));
            return _QueryString;
        }
        set { _QueryString = value; }
    }

    private string _QueryString1;

    public string QueryString1
    {
        get
        {
            _QueryString1 = Page.BuildSecureQueryString(string.Format("PrimaryID={0}&mode={1}&codeset={2}",
                                                                       PrimaryID,
                                                                       Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
                                                                       uxMarketData.Value));
            return _QueryString1;
        }
        set { _QueryString1 = value; }
    }

    public string HierarchyFilterQueryString(string HierarchyFilterMode, string clientIDbtn)
    {
        return Page.BuildSecureQueryString(string.Format("PrimaryID={0}&mode={1}&hierarchyMode={2}&clientID={3}", PrimaryID, Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
            HierarchyFilterMode, clientIDbtn));
    }



    public bool IsMerchantsOnWatch
    {
        get { return this.chkIsMerchantsOnWatch.Checked; }
    }


    private DataTable _MerchantFilterInfo = null;

    public DataTable MerchantFilterInfo
    {
        get { return _MerchantFilterInfo; }
        set { _MerchantFilterInfo = value; }
    }

    public MerchantRange MerchantRangeList
    {
        get
        {
            MerchantRange list = new MerchantRange();
            long from = -1;
            long to = -1;
            if (txtMerchantRangeFrom.Text.Trim() != string.Empty && txtMerchantRangeTo.Text.Trim() != string.Empty)
            {
                long.TryParse(txtMerchantRangeFrom.Text.Trim(), out from);
                long.TryParse(txtMerchantRangeTo.Text.Trim(), out to);
            }
            list.MerchantRangeFrom = (from < 0 ? "-1" : from.ToString());
            list.MerchantRangeTo = (to < 0 ? "-1" : to.ToString());
            return list;
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

    public WebSiteEnums.FeatureMode FeatureMode { get; set; }

    protected int MaxLengthViewMore
    {
        get
        {
            return Convert.ToInt32(WebSiteSettings.RiskViewMore);
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        uxApprovalDate.PrimaryID = PrimaryID;
        uxApprovalDate.Mode = Mode;
        uxApprovalDate.FeatureMode = FeatureMode;
        //set default values for transactional filter
        uxTransactionalFilter.PrimaryID = PrimaryID;
        uxTransactionalFilter.Mode = Mode;
        uxTransactionalFilter.FeatureMode = FeatureMode;

        if (!IsPostBack)
        {
            //set default filter for MS user
            switch (SessionManager.CurrentUserType)
            {
                case WebSiteEnums.UserHierarchyMode.Hierarchy:
                case WebSiteEnums.UserHierarchyMode.Headquarter:
                    {
                        string HierarchyFilterMode = GeneralFuncsLib.GetHierarchyInfo(SessionManager.CurrentUser.EntityType).HierarchyMode;
                        string EntityID = SessionManager.CurrentUser.EntityID;

                        //DEFAULT
                        string HierarchyFilterValue = EntityID;
                        //special cases
                        if (HierarchyFilterMode == "HEADQUARTER")
                        {

                            FilterParameterCollection pIn = new FilterParameterCollection();
                            pIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                            pIn.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
                            pIn.Add(new FilterParameter("@EntityID", EntityID, DbType.AnsiString));
                            pIn.Add(new FilterParameter("@HierarchyFilterValue", string.Empty, DbType.AnsiString, true));
                            FilterParameterCollection pOut = new FilterParameterCollection();

                            DataTable Info = WebServices.RiskServices.GetReports("spa_RM_MCF_GetRealHierarchyFilterValueByHierarchyID", pIn);

                            HierarchyFilterValue = Info.Rows[0]["HierarchyFilterValue"].ToString();
                        }

                        //ends
                        FilterParameterCollection paramsIn = new FilterParameterCollection();
                        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        paramsIn.Add(new FilterParameter("@PrimaryID", PrimaryID, DbType.Int32));
                        paramsIn.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
                        paramsIn.Add(new FilterParameter("@FilterValues", HierarchyFilterValue, DbType.AnsiString));
                        paramsIn.Add(new FilterParameter("@Mode", 3, DbType.Int32));
                        FilterParameterCollection paramsOut = new FilterParameterCollection();
                        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveHierarchyFilter", paramsIn, out paramsOut);
                    }
                    break;
            }

            CalculateMerchantCount();
            GetMerchantFilterInfo();
            OnDataBindControls(DataBindAction.BindState);
            OnDataBindControls(DataBindAction.BindSIC);
            OnDataBindControls(DataBindAction.BindZip);
            OnDataBindControls(DataBindAction.BindProfile);
            OnDataBindControls(DataBindAction.BindHierarchyFilter);
            OnDataBindControls(DataBindAction.BindMarketData);
        }
        VisibleControls();

        //ajaxify each literal control on repeater.
        foreach (RepeaterItem item in uxHierarchyFilterRepeater.Items)
        {
            Panel div = item.FindControl("divHierarchy") as Panel;
            Button btn = item.FindControl("btnRefreshHierarchy") as Button;

            RadAjaxManagerProxyReview.AjaxSettings.AddAjaxSetting(btn, div);
        }
    }

    protected void uxHierarchyFilterRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string data = string.Empty;
        string _localQueryString = string.Empty;
        string HierarchyFilterMode = SessionManager.RiskHierarchyFilter.Rows[e.Item.ItemIndex]["HierarchyFilterMode"].ToString();

        Literal ltr = (Literal)e.Item.FindControl("lblHierarchy");

        Dictionary<string, string> dataItem = UserControls_rm_MCF_FilterHierarchy.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID, HierarchyFilterMode);
        data = dataItem["Label"];
        if (data.Length > MaxLengthViewMore)
        {
            _localQueryString = Page.BuildSecureQueryString(string.Format("primaryid={0}&mode={1}&typemodal={2}&hierarchyMode={3}",
                                                                            PrimaryID,
                                                                            Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
                                                                            "1",
                                                                            HierarchyFilterMode));
            data = data.Substring(0, MaxLengthViewMore) + "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?" + _localQueryString + "', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>";
        }
        ltr.Text = data;
    }


    protected override void OnPostBackActions(Enum type, object sender)
    {
        string data = string.Empty;
        string _localQueryString = string.Empty;

        switch ((PostBackAction)type)
        {
            case PostBackAction.BindHierarchyFilter:
                Button btn = (Button)sender;
                Literal ltr = (Literal)btn.Parent.FindControl("lblHierarchy");

                Dictionary<string, string> dataItem = UserControls_rm_MCF_FilterHierarchy.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID, _CurrentHierarchyFilterMode);

                data = dataItem["Label"];
                if (data.Length > MaxLengthViewMore)
                {
                    _localQueryString = Page.BuildSecureQueryString(string.Format("primaryid={0}&mode={1}&typemodal={2}&hierarchyMode={3}",
                                                                                    PrimaryID,
                                                                                    Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
                                                                                    "1",
                                                                                    _CurrentHierarchyFilterMode));
                    data = data.Substring(0, MaxLengthViewMore) + "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?" + _localQueryString + "', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>";
                }
                ltr.Text = data;
                break;
            case PostBackAction.BindState:
                data = VeraCodeSolution.ValidateResponseData(UserControls_rm_MCF_FilterState.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID)["Label"]);
                if (data.Length > MaxLengthViewMore)
                {
                    _localQueryString = Page.BuildSecureQueryString(string.Format("primaryid={0}&mode={1}&typemodal={2}",
                                                                                    PrimaryID,
                                                                                    Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
                                                                                    "2"));
                    data = data.Substring(0, MaxLengthViewMore) + "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?" + _localQueryString + "', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>";
                }
                lblState.Text = data;
                break;
            case PostBackAction.BindSIC:
                bool IsViewMore = false;
                string fullData = string.Empty;
                data = VeraCodeSolution.DoVeraCode(UserControls_rm_MCF_FilterSIC.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID, out IsViewMore,out fullData)["Label"]);
                if (IsViewMore)
                {
                    data += "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal1('rm_MCF_Filter_SIC_ModalVM.aspx', 1000, 506);\" >" + GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>";
                }
                lblSIC.Text = data;
                break;
            //case PostBackAction.BindZip:
            //    data = VeraCodeSolution.ValidateResponseData(UserControls_Risk_NRT_FilterZipCode.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID));

            //    if (data.Length > MaxLengthViewMore)
            //    {
            //        _localQueryString = Page.BuildSecureQueryString(string.Format("primaryid={0}&mode={1}&typemodal={2}",
            //                                                                        PrimaryID,
            //                                                                        Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
            //                                                                        "4"));
            //        data = data.Substring(0, MaxLengthViewMore) + "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_Filter_CommonViewMore_Modal.aspx?" + _localQueryString + "', 500, 430);\" >view more...</a>";
            //    }
            //    lblZipCode.Text = data;
            //    break;
            case PostBackAction.BindProfile:
                data = VeraCodeSolution.DoVeraCode(UserControls_rm_MCF_FilterProfile.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID)["Label"]);
                if (data.Length > MaxLengthViewMore)
                {
                    _localQueryString = Page.BuildSecureQueryString(string.Format("primaryid={0}&mode={1}&typemodal={2}",
                                                                                    PrimaryID,
                                                                                    Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
                                                                                    "5"));
                    data = data.Substring(0, MaxLengthViewMore) + "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?" + _localQueryString + "', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>";
                }
                lblProfile.Text = data;
                break;
            case PostBackAction.BindMarketData:
                string marketdata = string.Empty;
                data = VeraCodeSolution.ValidateResponseData(UserControls_rm_MCF_FilterMarketData.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID, out marketdata));
                if (data.Length > MaxLengthViewMore)
                {
                    _localQueryString = Page.BuildSecureQueryString(string.Format("primaryid={0}&mode={1}&typemodal={2}",
                                                                                    PrimaryID,
                                                                                    Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
                                                                                    "6"));
                    data = data.Substring(0, MaxLengthViewMore) + "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?" + _localQueryString + "', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>";
                }
                lblMarketData.Text = data;

                uxMarketData.Value = marketdata;
                break;
            case PostBackAction.ShowModalMarketData:
                _QueryString1 = Page.BuildSecureQueryString(string.Format("PrimaryID={0}&mode={1}&codeset={2}",
                                                                       PrimaryID,
                                                                       Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
                                                                       uxMarketData.Value));

                (Page.Master as BaseMasterPage).AjaxAddResponseScript("CallFilterModal2('rm_MCF_Filter_MarketData_Modal.aspx?" + QueryString1 + "', 650, 760);");
                break;
            case PostBackAction.CountMerchant:
                int error = Save();
                if (error != 0)
                    return;
                CalculateMerchantCount();
                (Page.Master as BaseMasterPage).AjaxAddResponseScript("MerchantCountConfirm(" + MerchantCount + ");");
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        string data1 = string.Empty;
        string _localQueryString1 = string.Empty;

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindState:
                data1 = VeraCodeSolution.ValidateResponseData(UserControls_rm_MCF_FilterState.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID)["Label"]);
                if (data1.Length > MaxLengthViewMore)
                {
                    _localQueryString1 = Page.BuildSecureQueryString(string.Format("primaryid={0}&mode={1}&typemodal={2}",
                                                                                    PrimaryID,
                                                                                    Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
                                                                                    "2"));
                    data1 = data1.Substring(0, MaxLengthViewMore) + "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?" + _localQueryString1 + "', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>";
                }
                lblState.Text = data1;
                break;
            case DataBindAction.BindSIC:
                bool IsViewMore = false;
                string fullData = string.Empty;
                data1 = VeraCodeSolution.DoVeraCode(UserControls_rm_MCF_FilterSIC.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID, out IsViewMore,out fullData)["Label"]);
                if (IsViewMore)
                {
                    data1 += "&nbsp;<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_SIC_ModalVM.aspx', 1000, 506);\" >" + GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>";
                }
                lblSIC.Text = data1;
                break;
            //case DataBindAction.BindZip:
            //    data1 = VeraCodeSolution.ValidateResponseData(UserControls_Risk_NRT_FilterZipCode.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID));
            //    if (data1.Length > MaxLengthViewMore)
            //    {
            //        _localQueryString1 = Page.BuildSecureQueryString(string.Format("primaryid={0}&mode={1}&typemodal={2}",
            //                                                                        PrimaryID,
            //                                                                        Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
            //                                                                        "4"));
            //        data1 = data1.Substring(0, MaxLengthViewMore) + "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_Filter_CommonViewMore_Modal.aspx?" + _localQueryString1 + "', 500, 430);\" >view more...</a>";
            //    }
            //    lblZipCode.Text = data1;
            //    break;
            case DataBindAction.BindProfile:
                data1 = VeraCodeSolution.DoVeraCode(UserControls_rm_MCF_FilterProfile.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID)["Label"]);
                if (data1.Length > MaxLengthViewMore)
                {
                    _localQueryString1 = Page.BuildSecureQueryString(string.Format("primaryid={0}&mode={1}&typemodal={2}",
                                                                                    PrimaryID,
                                                                                    Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
                                                                                    "5"));
                    data1 = data1.Substring(0, MaxLengthViewMore) + "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?" + _localQueryString1 + "', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>";
                }
                lblProfile.Text = data1;
                break;
            case DataBindAction.BindMarketData:
                string marketdata = string.Empty;
                data1 = VeraCodeSolution.ValidateResponseData(UserControls_rm_MCF_FilterMarketData.GetSelectedValuesAsString(Mode, PrimaryID, ParamID, FilterID, out marketdata));
                if (data1.Length > MaxLengthViewMore)
                {
                    _localQueryString1 = Page.BuildSecureQueryString(string.Format("primaryid={0}&mode={1}&typemodal={2}",
                                                                                    PrimaryID,
                                                                                    Mode == WebSiteEnums.ParamFilterMode.Adhoc ? (int)WebSiteEnums.ParamFilterMode.Adhoc : (int)WebSiteEnums.ParamFilterMode.Assignment,
                                                                                    "6"));
                    data1 = data1.Substring(0, MaxLengthViewMore) + "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallFilterModal2('rm_MCF_Filter_CommonViewMore_Modal.aspx?" + _localQueryString1 + "', 500, 430);\" >" + GetLocalResourceObject("Risk_Assignment_MarketData_Filter_Adhocs_ascx_cs_ViewMore").ToString() + "</a>";
                }
                lblMarketData.Text = data1;
                uxMarketData.Value = marketdata;
                break;
            case DataBindAction.BindHierarchyFilter:
                DataTable info = new DataTable();
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.AddLanguageID();

                SessionManager.RiskHierarchyFilter = WebServices.RiskServices.GetReports("spa_RM_MCF_GetHierarchyFilterList", parameters);
                info = SessionManager.RiskHierarchyFilter;
                uxHierarchyFilterRepeater.DataSource = info;
                uxHierarchyFilterRepeater.DataBind();
                break;
        }
    }

    /// <summary>
    /// Bind data for Merchant Range: From, To
    /// </summary>
    private void BindMerchantRange()
    {
        if (_MerchantFilterInfo != null && _MerchantFilterInfo.Rows.Count > 0)
        {
            chkIsMerchantsOnWatch.Checked = _MerchantFilterInfo.Rows[0]["IsMerchantsOnWatch"] != DBNull.Value
                && Convert.ToBoolean(_MerchantFilterInfo.Rows[0]["IsMerchantsOnWatch"]);

            //uxReportDate.SelectedDate = Convert.ToDateTime(_MerchantFilterInfo.Rows[0]["ReportDate"] == DBNull.Value ? DateTime.Now : _MerchantFilterInfo.Rows[0]["ReportDate"]);

            long mer = -1;
            long.TryParse(_MerchantFilterInfo.Rows[0]["MerchantNumber"].ToString(), out mer);
            txtMerchantNumber.Text = VeraCodeSolution.ValidateResponseData(mer < 0 ? string.Empty : mer.ToString());

            long from = -1;
            long to = -1;

            long.TryParse(_MerchantFilterInfo.Rows[0]["MerchantRangeFrom"].ToString(), out from);
            long.TryParse(_MerchantFilterInfo.Rows[0]["MerchantRangeTo"].ToString(), out to);

            txtMerchantRangeFrom.Text = VeraCodeSolution.GetOutputHtmlString(from < 0 ? string.Empty : from.ToString());
            txtMerchantRangeTo.Text = VeraCodeSolution.GetOutputHtmlString(to < 0 ? string.Empty : to.ToString());
        }
    }

    public void GetMerchantFilterInfo()
    {
        //get merchant filter  

        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        paramsIn.Add(new FilterParameter("@AssignmentID", PrimaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@Mode", (int)Mode, DbType.Int32));

        _MerchantFilterInfo = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAssignmentMerchantFilter", paramsIn);

        BindMerchantRange();
    }

    public int SaveMerchantFilter()
    {
        long MerchantRangeFrom = Convert.ToInt64(MerchantRangeList.MerchantRangeFrom);
        long MerchantRangeTo = Convert.ToInt64(MerchantRangeList.MerchantRangeTo);

        string MerchantNumber = VeraCodeSolution.DoVeraCode(txtMerchantNumber.Text) == string.Empty ? "-1" : VeraCodeSolution.DoVeraCode(txtMerchantNumber.Text);
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", PrimaryID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@IsMerchantsOnWatch", IsMerchantsOnWatch, DbType.AnsiString));
        paramsIn.Add(new FilterParameter("@MerchantRangeFrom", MerchantRangeFrom, DbType.AnsiString));
        paramsIn.Add(new FilterParameter("@MerchantRangeTo", MerchantRangeTo, DbType.AnsiString));
        paramsIn.Add(new FilterParameter("@ReportDate", DateTime.Now, DbType.DateTime));
        paramsIn.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
        paramsIn.Add(new FilterParameter("@Mode", (int)Mode, DbType.Int32));

        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_SaveAssignmentMerchantFilter", paramsIn, out paramsOut);

        return 0;
    }

    private void VisibleControls()
    {
        plhState.Visible = plhMarketData.Visible =
            // plhZipCode.Visible = 
            plhProfile.Visible = plhSIC.Visible
            = plhRefesh.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;
        //uxReportDate.DatePopupButton.Visible = false;

        foreach (RepeaterItem item in uxHierarchyFilterRepeater.Items)
        {
            PlaceHolder HierarchyPlaceHolder = item.FindControl("plhHierarchy") as PlaceHolder;
            HierarchyPlaceHolder.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;

        }


        if (FeatureMode == WebSiteEnums.FeatureMode.View)
            chkIsMerchantsOnWatch.Enabled = false;
        txtMerchantRangeFrom.ReadOnly = txtMerchantNumber.ReadOnly = txtMerchantRangeTo.ReadOnly = FeatureMode == WebSiteEnums.FeatureMode.View;
        //uxReportDate.DateInput.ReadOnly = true;
    }

    #region Merchant Count

    bool _HasCounted = false;
    protected void CalculateMerchantCount()
    {
        if (int.Parse(PrimaryID) == 0 && !IsPostBack) return;
        int assignmentID = 0;

        assignmentID = int.Parse(PrimaryID);
        MerchantCount = GetAssignmentMerchantCount(assignmentID);
        DisplayMerchantCount();
        _HasCounted = true;
    }

    private int GetAssignmentMerchantCount(int assignmentID)
    {
        int merchantCount = 0;

        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        paramsIn.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@FilterMode", (int)WebSiteEnums.ParamFilterMode.Adhoc, DbType.Int32));
        paramsIn.Add(new FilterParameter("@MerchantTotal", 0, DbType.Int32, true));

        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_GetAssignmentMerchantCount", paramsIn, out paramsOut);

        int.TryParse(paramsOut[0].ParameterValue.ToString(), out merchantCount);

        return merchantCount;
    }

    private void DisplayMerchantCount()
    {
        pnlMerchantCountOnFilter.InnerHtml = VeraCodeSolution.ValidateResponseData(string.Format("{0}", AS.Common.Formater.FormatData.FormatInteger(MerchantCount)));

    }



    #endregion

    #region Control Event
    protected void uxBntMarketData_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ShowModalMarketData, sender);
    }

    protected void btnRefreshMarketData_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.BindMarketData, sender);
    }

    protected void btnCalculateMerchantCount_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CountMerchant, sender);
    }

    protected void btnRefreshHierarchy_Click(object sender, EventArgs e)
    {

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


    protected void btnRefreshHierarchy_Command(object sender, CommandEventArgs e)
    {

        _CurrentHierarchyFilterMode = e.CommandArgument.ToString();
        OnPostBackActions(PostBackAction.BindHierarchyFilter, sender);
    }


    #endregion

    #region IRiskParamFilter Members

    public WebSiteEnums.ParamFilterMode Mode { get; set; }

    public string PrimaryID { get; set; }

    public string ParamID { get; set; }

    public string FilterID { get; set; }

    public int Save()
    {
        ///if (CheckMarketDataSelected())
        //{
        SaveMerchantFilter();
        uxApprovalDate.Save();
        uxTransactionalFilter.Save();
        //}
        //else
        //    return 1;
        return 0;
    }

    public event EventHandler SelectedChanged;

    #endregion

    public string GetItemIndexAsString(int index)
    {
        string result;
        if (index < 10)
        {
            result = "0" + index.ToString();
        }
        else
        {
            result = index.ToString();
        }

        return result;
    }
}
