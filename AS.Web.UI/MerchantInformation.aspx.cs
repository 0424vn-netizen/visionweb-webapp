using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

namespace As.VisionWeb.Web
{
    [PagePermission("MerchProfile,MSMerchProfile")]
    public partial class MerchantInformation : ReportPage
    {
        #region Enum
        enum DataBindAction
        {
            BindGrid,
            BindMemoGrid,
            BindCommentGrid,

        }

        enum PostBackAction
        {
            AddComment,
            ResetPage,
        }

        #endregion Enum

        #region Constants

        private const string SPA_CHECK_MERCHANT_BELONG_TO_USER = "spa_SEC_CheckMerchantBelongtoUser";
        private const string GRID_TITLE_MERCHANT_LIST = "MerchantProfile_aspx_cs_MerchantList";
        private const string GRID_TITLE_MERCHANT_MEMMO = "MerchantProfile_aspx_cs_MerchantMemos";
        private const string EXTENDED_SETTING_SHOW_EXPORT_QUEUE = "SHOW_EXPORT_QUEUE";


        #endregion Constants

        #region Properties
        private MerchantColumn EntityKey { get; set; }
        private bool HasDrillDownLink { get; set; }
        private string MerchantNumber
        {
            get
            {
                if (ViewState["MerchantNumber"] != null)
                    return ViewState["MerchantNumber"].ToString();
                else
                    return string.Empty;
            }
            set
            {
                ViewState["MerchantNumber"] = value;
            }
        }

        private string GridTitle
        {
            get
            {
                return GeneralFuncsLib.GetFullGridTitleName(ReportFilter);
            }
        }

        public bool IsHideHeaderMenuAndLeftNav
        {
            get
            {
                if (IsSecureQueryString)
                    return SecureQueryString["IsHideMenu"].ToBoolean();
                return false;
            }
        }

        private int HierarchySelected
        {
            get
            {
                if (IsSecureQueryString && SecureQueryString["FilterMode"] != null)
                {
                    return SecureQueryString["FilterMode"].ToInt();
                }
                return -1;
            }
        }

        private string FilterValue
        {
            get
            {
                if (IsSecureQueryString && SecureQueryString["FilterValue"] != null)
                {
                    return SecureQueryString["FilterValue"].ToString();
                }
                return string.Empty;
            }
        }
        private string DecryptColumns { get; set; }

        private bool IsShowNewExport
        {
            get
            {
                return GeneralFuncsLib.GetDataOfExtendedSetting(EXTENDED_SETTING_SHOW_EXPORT_QUEUE).ToLower().Equals("true");
            }
        }

        #endregion Properties

        #region Methods

        #region Protected Methods

        protected override void PageInitialize()
        {
            InitGrid();
            this.GridIDs.Add("uxMerchantListGrid");
            this.GridIDs.Add("uxMemoGrid");
            this.GridIDs.Add("uxMerchantNoteGrid");
            this.ExporterIDs.Add("uxExporterCommentTop");
            this.ExporterIDs.Add("uxExporterCommentBottom");

            this.ExporterIDs.Add("uxExportTop");
            this.ExporterIDs.Add("uxExportBottom");

            this.ExporterIDs.Add("uxExportMemolistTop");

            base.PageInitialize();
        }

        private void InitGrid()
        {
            var clientConfig = MerchantProfileHelper.GetClientConfig();
            if (clientConfig != null && clientConfig.Columns.Any())
            {
                int columnIndex = 0;
                foreach (var column in clientConfig.Columns)
                {
                    var boundColumn = new ASGridBoundColumn
                    {
                        DataField = column.Key,
                        UniqueName = column.UniqueName
                    };

                    column.HeaderAlign = !string.IsNullOrEmpty(column.HeaderAlign) ? column.HeaderAlign : "center";
                    boundColumn.HeaderStyle.HorizontalAlign = column.HeaderAlign.ToEnum(HorizontalAlign.Center);

                    column.ItemAlign = !string.IsNullOrEmpty(column.ItemAlign) ? column.ItemAlign : "center";
                    boundColumn.ItemStyle.HorizontalAlign = column.ItemAlign.ToEnum(HorizontalAlign.Center);

                    if (!string.IsNullOrEmpty(column.ReSourceKey))
                    {
                        var headerText = GetLocalResourceObject(string.Format("{0}.HeaderText", column.ReSourceKey));
                        var headerTooltip = GetLocalResourceObject(string.Format("{0}.HeaderTooltip", column.ReSourceKey));

                        boundColumn.HeaderText = headerText != null ? headerText.ToString() : string.Empty;
                        boundColumn.HeaderTooltip = headerTooltip != null ? headerTooltip.ToString() : string.Empty;
                    }

                    boundColumn.ASFormat = RM_MCF_GeneralFuncsLib.GetASFormat(column.ASFormat);
                    boundColumn.SortExpression = column.Key;
                    boundColumn.OrderIndex = columnIndex;
                    boundColumn.ASDefaultNullValue = column.DefaultValue;
                    boundColumn.HeaderStyle.Width = new Unit((column.Width == 0) ? 80 : Convert.ToInt32(column.Width), UnitType.Pixel);
                    uxMerchantListGrid.Columns.Add(boundColumn);
                    columnIndex++;

                    //set entity Key
                    if (column.UniqueName.Equals("MerchantID", StringComparison.OrdinalIgnoreCase))
                    {
                        EntityKey = column;
                    }
                }

                //display tax & partial tax column        
                ShowFullTaxColumn(clientConfig.Columns, HasFullTaxView());

                //display user Id column
                var userIdColumn = FindGridColumn(uxMerchantListGrid, "UserID");

                if (userIdColumn != null)
                    userIdColumn.Visible = GeneralFuncsLib.IsUserSignOn();

                DecryptColumns = clientConfig.DecryptColumns;
            }

            string entityIDDisable = "," + GeneralFuncsLib.GetDataOfExtendedSetting("InvisibleHyperlinkByEntityTypeIDs") + ",";
            string entityTypeCurrent = "," + SessionManager.CurrentUser.EntityType.ToString() + ",";
            if (entityIDDisable.Contains(entityTypeCurrent))
            {
                HasDrillDownLink = false;
            }
            else
            {
                HasDrillDownLink = true;
            }

            uxMerchantListGrid.MasterTableView.AllowCustomSorting = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IsBindDataOnLoad = true;
            CheckMerchantFromQueryString();//Check merchant exists in query string
            MaintainScrollPositionOnPostBack = true;

            if (!IsPostBack)
            {
                GoBackSetting();
            }

            ((MasterPageNormal)Page.Master).HideHeaderMenu = true;

        }

        protected override void DoSwitchView()
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(MerchantNumber))
                {
                    HierarchyDetail merchant = GeneralFuncsLib.GetMerchantHierarchyInfo();
                    ReportFilter.CurrentValue.ID = merchant.HierarchyID;
                    ReportFilter.CurrentValue.Value = MerchantNumber;
                    ReportFilter.CurrentValue.HierarchyMode = merchant.HierarchyMode;
                }
                else if (HierarchySelected != -1 && !FilterValue.IsNullOrEmpty())
                {
                    ReportFilter.CurrentValue.HierarchyMode = GeneralFuncsLib.GetHierarchyInfo(HierarchySelected).HierarchyMode.ToString();
                    ReportFilter.CurrentValue.ID = GeneralFuncsLib.GetHierarchyInfo(HierarchySelected).HierarchyID;
                    ReportFilter.CurrentValue.Value = FilterValue;
                }

                if (IsDetailMode())
                {
                    ShowMerchantDetail();
                }
                else
                {
                    this.uxPanelMerchantList.Visible = true;
                    this.uxPanelDetail.Visible = false;
                    uxPanelCaseHistory.Visible = false;
                }
            }

            ((MasterPageNormal)Page.Master).HideHeaderMenu = IsHideHeaderMenuAndLeftNav;
            ((MasterPageNormal)Page.Master).ShowLeftNaviControl = !IsHideHeaderMenuAndLeftNav;
        }

        protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
        {
            if (sender == uxMerchantListGrid && uxMerchantListGrid.Visible)
            {
                OnDataBindControls(DataBindAction.BindGrid, sender);
            }
            //TK39919 - Remove
            if (uxMemoGrid.Visible)
            {
                OnDataBindControls(DataBindAction.BindMemoGrid, sender);
                //TK39919 - Remove
            }
        }

        public string ButtonExcelMerchantMemos_Click()
        {
            OnDataBindControls(DataBindAction.BindMemoGrid, uxExportMemolistTop.Grid);
            return uxExportMemolistTop.ExportExcelManual();
        }
        protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView rowView = e.Item.DataItem as DataRowView;

                if (sender == uxMerchantListGrid && sender.Visible)
                {
                    //build Last Batch Activity link
                    var activityView = rowView.DataView.Table.Columns.Contains("LastBatchActivity");
                    var activityItem = dataItem.OwnerTableView.Columns.FindByUniqueNameSafe("LastBatchActivity");

                    if (activityView && activityItem != null)
                    {
                        if (rowView["LastBatchActivity"] != null && !string.IsNullOrEmpty(rowView["LastBatchActivity"].ToString()))
                        {
                            if (((ReportPage)Page).IsUserWithPermission("BatchRpt") || ((ReportPage)Page).IsUserWithPermission("MSBatchRpt"))
                            {
                                DateTime tempDate;
                                if (DateTime.TryParse(rowView["LastBatchActivity"].ToString(), out tempDate))
                                {
                                    dataItem["LastBatchActivity"].Text = VeraCodeSolution.DoVeraCode(
                                   GeneralFuncsLib.BuildLastBatchHistoryLink(
                                   (SecurePage)Page, tempDate.Ticks.ToString(), rowView["Entity"].ToString(), dataItem["LastBatchActivity"].Text));
                                }
                            }
                        }
                        else
                        {
                            dataItem["LastBatchActivity"].Text = string.Empty;
                        }
                    }

                    //build merchant number link
                    if (EntityKey != null)
                    {
                        var merchantIDView = rowView.DataView.Table.Columns.Contains(EntityKey.Key);
                        var MerchantIDItem = dataItem.OwnerTableView.Columns.FindByDataField(EntityKey.Key);

                        if (merchantIDView && MerchantIDItem != null && HasDrillDownLink)
                        {
                            var merchantNumber = dataItem[EntityKey.UniqueName];
                            merchantNumber.Text = BuildDrilldownLink(merchantNumber.Text);
                        }
                    }
                }
            }
        }

        protected override void OnDataBindControls(Enum type, object sender)
        {
            switch ((DataBindAction)type)
            {
                case DataBindAction.BindGrid:
                    ASGrid grid = (ASGrid)sender;
                    string spaName = "spa_GetMerchantList";
                    FilterParameterCollection paramss = new FilterParameterCollection();
                    paramss.AddLoggedInUserReportingParams();
                    paramss.AddLanguageID();
                    paramss.AddHierarchyFilterParamsWithoutDate(this);

                    if (!string.IsNullOrEmpty(DecryptColumns))
                    {
                        paramss.AddDecryptDataParams(DecryptColumns);
                    }
                    else
                    {
                        paramss.AddDecryptDataParams("EncryptedTaxID");
                    }

                    grid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(paramss) });

                    string merchantListTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject(GRID_TITLE_MERCHANT_LIST) + " - ");
                    string merchantListSubTitle = VeraCodeSolution.ValidateResponseData(GridTitle);

                    uxExportTop.GridTitle = merchantListTitle;
                    uxExportTop.GridSubTitle = merchantListSubTitle;
                    uxExportTop.GridHeader = uxExportTop.GridTitle + uxExportTop.GridSubTitle;

                    if (IsShowNewExport)
                    {
                        uxExportQueueTop.GridTitle = merchantListTitle;
                        uxExportQueueTop.GridSubTitle = merchantListSubTitle;
                        uxExportQueueTop.GridHeader = merchantListTitle + merchantListSubTitle;
                        uxExportQueueTop.FilterParams = BuildExportFilterParams();
                    }

                    break;
                case DataBindAction.BindCommentGrid:
                    {
                        FilterParameterCollection _Parameters = new FilterParameterCollection();
                        _Parameters.AddLoggedInUserReportingParams();
                        MerchantNumber = ReportFilter.CurrentValue.Value;
                        _Parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
                    }
                    break;
                case DataBindAction.BindMemoGrid:
                    {
                        if (GeneralFuncsLib.GetDataOfExtendedSetting("InvisibleMemosGrid") != "true")
                        {
                            uxMemoSection.Visible = true;
                            FilterParameterCollection _Parameters = new FilterParameterCollection();
                            _Parameters.AddLoggedInUserReportingParams();
                            MerchantNumber = ReportFilter.CurrentValue.Value;
                            _Parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
                            this.uxMemoGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_ms_GetMerchantMemos", ReportServices.ConvertToFilterParamWSArray(_Parameters) });

                            uxExportMemolistTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject(GRID_TITLE_MERCHANT_MEMMO) + " - ");
                            uxExportMemolistTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(GridTitle);
                            uxExportMemolistTop.GridHeader = uxExportMemolistTop.GridTitle + uxExportMemolistTop.GridSubTitle;
                        }
                        else
                        {
                            uxMemoSection.Visible = false;
                        }
                    }
                    break;
            }
        }
        protected override void OnPostBackActions(Enum type, object sender)
        {
            switch ((PostBackAction)type)
            {
                case PostBackAction.AddComment:
                    AjaxAddResponseScript("ClearMessage();");
                    break;
            }
        }

        protected void uxAddComment_Click(object sender, EventArgs e)
        {
            OnPostBackActions(PostBackAction.AddComment);
        }

        protected void uxReset_Click(object sender, EventArgs e)
        {
            OnPostBackActions(PostBackAction.ResetPage);
        }

        protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
        {
            //display tax & partial tax column
            var clientConfig = MerchantProfileHelper.GetClientConfig();
            ShowFullTaxColumn(clientConfig.Columns, false);

            base.DoNeedExportConfig(sender, exportConfig);

            // If filter by taxid => just get partial Taxid
            switch (ReportFilter.CurrentValue.HierarchyMode)
            {
                case HierarchyMode.TAXID:
                    string filename = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject(GRID_TITLE_MERCHANT_LIST) + " - ");
                    exportConfig.FileName = GeneralFuncsLib.FormatFileName(filename + GeneralFuncsLib.GetPartialTaxId(ReportFilter.CurrentValue.Value));
                    exportConfig.ReportHeader = Server.HtmlDecode(sender.GridTitle);
                    break;
                default:
                    exportConfig.FileName = GeneralFuncsLib.FormatFileName(Server.HtmlDecode(sender.GridHeader));
                    exportConfig.ReportHeader = Server.HtmlDecode(exportConfig.ReportHeader);
                    break;
            }
        }

        protected override void DoReportFilterAction(ReportFilterEventArgs e)
        {
            this.MerchantNumber = ReportFilter.CurrentValue.Value;
            if (e.ActionType == ReportFilterEventType.Submit)
            {
                string merchantName = GeneralFuncsLib.GetMerchantName(e.HierachyValue.Value);
                string activity = GetActivity(!string.IsNullOrEmpty(merchantName));
                string merchantNumber = string.IsNullOrEmpty(merchantName) ? "" : e.HierachyValue.Value;
                GeneralFuncsLib.SaveUserActivity(merchantNumber, activity);
            }

            if (e.ActionType == ReportFilterEventType.Submit || e.ActionType == ReportFilterEventType.DrillDown)
            {
                if (IsDetailMode())
                {
                    ShowMerchantDetail();
                }
                else
                {
                    this.uxPanelMerchantList.Visible = true;
                    this.uxPanelDetail.Visible = false;
                    uxPanelCaseHistory.Visible = false;
                }
            }
            base.DoReportFilterAction(e);

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            uxExportTop.Visible = !IsShowNewExport;
            uxExportQueueTop.Visible = IsShowNewExport;
            ((MasterPageNormal)this.Master).ShowLeftNaviControl = uxPanelDetail.Visible && !IsHideHeaderMenuAndLeftNav;
        }
        protected void uxReloadHierachy_Click(object sender, EventArgs e)
        {
            ShowMerchantDetail();
            this.AjaxAddResponseScript("bindSourceAndRole()");
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HasFullTaxView()
        {
            if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
                || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
                && IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_TAX_ID))
            {
                return true;
            }
            return false;
        }
        private void ShowMerchantDetail()
        {
            this.uxPanelMerchantList.Visible = false;
            this.uxPanelDetail.Visible = true;

            uxPanelCaseHistory.Visible = ((ReportPage)Page).IsUserWithPermission("CMOpenCase")
                || ((ReportPage)Page).IsUserWithPermission("CMSearchCase");

            ucMerchantNote.MerchantNumber = ReportFilter.CurrentValue.Value;
            ucCaseHistory.MerchantNumber = ReportFilter.CurrentValue.Value;
            if (SessionManager.CurrentMerchantNumber.IsNullOrEmpty()
                || SessionManager.CurrentMerchantNumber != ReportFilter.CurrentValue.Value)
            {
                SessionManager.CurrentMerchantNumber = ReportFilter.CurrentValue.Value;
                ucMerchantNote.BindDataMultiSelector();
                ucCaseHistory.BindDataMultiSelector();
                ucCaseHistory.BindGridCaseHistory();
            }
            else
            {
                // OP #7060
                // With secondary user in the first time add note at ms site, 
                // back to merchant profile and click search again
                // Must rebind role list to update the role of this user
                this.AjaxAddResponseScript("rebindRole()");
            }

            uxMIF_MerchantDetails_Generic.Rebind();
            var processor = uxMIF_MerchantDetails_Generic.Processor;
            MecchantProfileNavigator.Processor = processor;
            BindAjaxuxMIF_MerchantDetail(uxMIF_MerchantDetails_Generic);
        }

        private void CheckMerchantFromQueryString()
        {
            if (IsSecureQueryString)
            {
                if (SecureQueryString["MerchantNumber"] != null)
                {
                    MerchantNumber = SecureQueryString["MerchantNumber"];
                    DoSwitchView();
                }
                else if (HierarchySelected != -1 && !FilterValue.IsNullOrEmpty())
                {
                    DoSwitchView();
                }
                if (SecureQueryString["FromExCM"] == "1")
                {
                    uxReportFiltering.Visible = false;
                    uxPageTitleCM.Visible = true;
                    uxPageTitle.Visible = false;
                }
                else
                {
                    uxReportFiltering.Visible = true;
                    uxPageTitleCM.Visible = false;
                    uxPageTitle.Visible = true;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Request["mc"]))
                {
                    MerchantNumber = WebServices.ApiServices.DecryptText(Request["mc"]);
                    DoSwitchView();
                }
            }
        }

        private bool IsMerchantBelongToUser()
        {
            FilterParameterCollection paramsIn = new FilterParameterCollection();
            paramsIn.AddLoggedInUserReportingParams(true);
            paramsIn.AddHierarchyFilterParams(this);
            DataTable dt = WebServices.SecurityServices.GetReports(SPA_CHECK_MERCHANT_BELONG_TO_USER, paramsIn);
            // Belonged to user
            return dt != null && dt.Rows.Count > 0 && (bool)dt.Rows[0][0];
        }

        private string GetActivity(bool isMerchant)
        {
            string hierachyMode = ReportFilter.CurrentValue.HierarchyMode;
            string hierachyValue = ReportFilter.CurrentValue.Value;
            string hierarchyModeValue = string.Empty;
            foreach (DataRow row in SessionManager.HierarchyFilterDrillDown.Rows)
            {
                if (row["CurrentHierarchyMode"].ToString() == hierachyMode)
                {

                    hierarchyModeValue = row["CurrentHierarchyGridName"].ToString();
                }
            }
            string value;
            if (isMerchant)
            {
                value = GetLocalResourceObject("MerchantProfile_aspx_cs_View").ToString() + " " + hierachyValue;
            }
            else
            {
                if (string.IsNullOrEmpty(hierachyValue))
                {
                    if (GeneralFuncsLib.IsMerchantMode(hierachyMode))
                        value = GetLocalResourceObject("MerchantProfile_aspx_cs_SearchAllMerchants").ToString();
                    else
                        value = "Search all " + hierarchyModeValue + "s";
                }
                else
                {
                    value = string.Format(GetLocalResourceObject("MerchantProfile_aspx_cs_SearchBy").ToString(), hierarchyModeValue, hierachyValue);
                }
            }
            return string.Format(GetLocalResourceObject("MerchantProfile_aspx_cs_MerchantProfile").ToString(), value);
        }

        private void GoBackSetting()
        {
            if (RiskSessionManager.MerchantProfileReferrer.Length > 0 && RiskSessionManager.MerchantProfileReferrerInfo.Key.Length > 0
                && string.Compare(RiskSessionManager.MerchantProfileReferrerInfo.Key, this.MerchantNumber) == 0
                && RiskSessionManager.MerchantProfileReferrerInfo.Url.Length > 0)
            {
                var url = string.Format("/risk_MCF/{0}", RiskSessionManager.MerchantProfileReferrerInfo.Url);
                uxGoBack.NavigateUrl = VeraCodeSolution.ValidateResponseData(url);
                uxGoBack.Text = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("MerchantProfile_aspx_cs_BackTo").ToString() + " " + RiskSessionManager.MerchantProfileReferrerInfo.Title);
                uxGoBack.Visible = true;
            }
            else
            {
                uxGoBack.Visible = false;
                RiskSessionManager.MerchantProfileReferrer = null;
                //Fix issue SP #275
                if (!GeneralFuncsLib.IsAddGeographicData())
                {
                    RiskSessionManager.MerchantProfileReferrerInfo = null;
                }
            }
        }

        private void BindAjaxuxMIF_MerchantDetail(GlobalUserControl control)
        {
            var ajax = new AjaxSetting
            {
                AjaxControlID = uxReloadHierachy.ID
            };
            ajax.UpdatedControls.Add(new AjaxUpdatedControl(control.ID, string.Empty));
            if (!uxRadAjaxManager.AjaxSettings.Contains(ajax))
                uxRadAjaxManager.AjaxSettings.Add(ajax);
        }
        #endregion Methods

        #endregion Methods
        //TK39919 - Add
        protected void uxReportFiltering_Filtering(object sender, EventArgs e)
        {
            //10/4/2018 | 45128 Merchant Notes Issue - Prod
            //Remove code
            if (ReportFilter.CurrentValue.Value.IsNullOrEmpty())
                SessionManager.CurrentMerchantNumber = string.Empty;
        }
        private bool IsDetailMode()
        {
            string entityIDDisable = "," + GeneralFuncsLib.GetInvisibleHyperlinkByEntityTypeIDs() + ",";
            string entityTypeCurrent = "," + SessionManager.CurrentUser.EntityType.ToString() + ",";
            if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode)
               && !string.IsNullOrEmpty(ReportFilter.CurrentValue.Value)
                && !string.IsNullOrEmpty(GeneralFuncsLib.GetMerchantName(ReportFilter.CurrentValue.Value))
                && IsMerchantBelongToUser()
                && !entityIDDisable.Contains(entityTypeCurrent))
                return true;

            return false;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string[] CheckSensitiveData(string comment)
        {
            return GeneralFuncsLib.DetectSesitiveData(comment);
        }

        //39919 - Performance PROP Issue
        [WebMethod]
        public static object[] GetRoleAndAddedBy(string sources)
        {
            UserControls_MerchantNote ucMerchantNoteRoleAndAddedBy = new UserControls_MerchantNote();
            DataTable roles = ucMerchantNoteRoleAndAddedBy.GetRoles(sources);
            DataTable users = ucMerchantNoteRoleAndAddedBy.GetAddedBy(sources, string.Empty);

            object[] results = new object[2];
            results[0] = GeneralFuncsLib.DataTableToJson(roles);
            results[1] = GeneralFuncsLib.DataTableToJson(users);
            return results;
        }

        [WebMethod]
        public static object GetAddedBy(string sources, string roles)
        {
            DataTable users = new UserControls_MerchantNote().GetAddedBy(sources, roles);

            return GeneralFuncsLib.DataTableToJson(users);
        }

        [WebMethod]
        public static object GetSourceAndRole()
        {
            UserControls_MerchantNote ucMerchantNoteSourceAndRole = new UserControls_MerchantNote();
            string defaultSources = PersonalDataHelper.GetJSONConfig<string>(UserConfigNames.CONFIG_MERCHANT_PROFILE_SOURCE_DEFAULT_SETTING);
            DataTable sources = ucMerchantNoteSourceAndRole.GetSources();
            object[] results = new object[4];

            results[0] = defaultSources;
            results[1] = GeneralFuncsLib.DataTableToJson(sources);
            results[2] = GeneralFuncsLib.DataTableToJson(ucMerchantNoteSourceAndRole.GetRoles(defaultSources));
            results[3] = GeneralFuncsLib.DataTableToJson(ucMerchantNoteSourceAndRole.GetAddedBy(defaultSources, string.Empty));

            return results;
        }

        [WebMethod]
        public static object GetRoles(string sources)
        {
            DataTable roles = new UserControls_MerchantNote().GetRoles(sources);

            return GeneralFuncsLib.DataTableToJson(roles);
        }

        [WebMethod(EnableSession = true)]
        public static string GetSiteJumpUrl(string encUser)
        {
            var url = MerchantDetailsGenericControl.GetSiteJumpUrl(encUser);
            return url;
        }

        protected void uxMerchantListGrid_PreRender(object sender, EventArgs e)
        {
            if (IsShowNewExport && uxMerchantListGrid.AS_DataSource != null)
            {
                uxExportQueueTop.HasData = uxMerchantListGrid.AS_DataSource.Rows.Count > 0;
            }

            if (HierarchySelected != -1 && !FilterValue.IsNullOrEmpty() && !IsPostBack)
            {
                AjaxAddResponseScript(string.Format("rf_DrilldownReportFilterValuesCM('{0}', '{1}', '{2}');", GeneralFuncsLib.GetHierarchyInfo(HierarchySelected).HierarchyID,
                    GeneralFuncsLib.GetHierarchyInfo(HierarchySelected).HierarchyMode.ToString(), FilterValue));
            }
        }

        private GridColumn FindGridColumn(ASGrid grid, string colName)
        {
            var columns = grid.Columns.Cast<GridColumn>().ToList();
            if (columns != null && columns.Any())
            {
                var column = columns.FirstOrDefault(x => x.UniqueName.Equals(colName, StringComparison.OrdinalIgnoreCase));
                return column;
            }
            return null;
        }
        private string BuildDrilldownLink(string text)
        {
            //get all hierarchy filter
            if (SessionManager.HierarchyFilterDrillDown == null)
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams();
                parameters.AddLanguageID();
                SessionManager.HierarchyFilterDrillDown = WebServices.RiskServices.GetReports("spa_GetHierarchyFilterLevel", parameters);
            }

            const string tpl = "<a href=\"#\" onclick=\"return rf_DrilldownReportFilterValues('{0}', '{1}', '{2}', '{4}', '{5}', '{6}');\">{3}</a>";
            var value = text;
            HierarchyDetail merchantHierarchy = GeneralFuncsLib.GetMerchantHierarchyInfo();
            var nextID = merchantHierarchy.HierarchyID;
            var nextMode = merchantHierarchy.HierarchyMode;

            return string.Format(tpl, nextID, nextMode, value, text, ReportFilter.CurrentValue.ID, ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
        }

        private string BuildExportFilterParams()
        {
            string hierarchyMode = ReportFilter.CurrentValue.HierarchyMode ?? string.Empty;
            string hierarchyValue = string.Empty;

            if (!ReportFilter.CurrentValue.Value.IsNullOrEmpty())
            {
                var hierarchyInfo = GeneralFuncsLib.GetHierarchyInfo(hierarchyMode);
                if (!hierarchyInfo.ShowPrefix)
                {
                    hierarchyValue = hierarchyInfo.Prefix + ReportFilter.CurrentValue.Value;
                }
                else
                {
                    hierarchyValue = ReportFilter.CurrentValue.Value.ToLower() != hierarchyInfo.Prefix.ToLower()
                        ? ReportFilter.CurrentValue.Value
                        : string.Empty;
                }

                if (hierarchyMode == HierarchyMode.MERCHANT_NAME ||
                    hierarchyMode == HierarchyMode.CAYAN_CORPNAME ||
                    hierarchyMode.Equals(HierarchyMode.USERID, StringComparison.OrdinalIgnoreCase))
                {
                    hierarchyValue = GeneralFuncsLib.ReplaceSpecialCharacter(hierarchyValue);
                }
                else
                {
                    hierarchyValue = hierarchyValue.Replace("*", "%");
                }
            }

            var doc = new XmlDocument();
            var root = doc.CreateElement("ExportFilter");
            doc.AppendChild(root);

            var reportTitleItem = doc.CreateElement("ReportHeader");
            reportTitleItem.SetAttribute("Value", string.Join(",", uxExportQueueTop.GridHeader));
            root.AppendChild(reportTitleItem);

            var paramInputs = doc.CreateElement("ParamInputs");
            root.AppendChild(paramInputs);
            AppendParamInput(doc, paramInputs, "HierarchyFilterMode", hierarchyMode);
            AppendParamInput(doc, paramInputs, "HierarchyFilterValue", hierarchyValue);
            AppendParamInput(doc, paramInputs, "UserID", SessionManager.CurrentUser.UserID);
            AppendParamInput(doc, paramInputs, "UserMode", GeneralFuncsLib.GetUserMode());
            AppendParamInput(doc, paramInputs, "SiteID", SessionManager.CurrentUser.SiteID.ToString());

            var filterItem = doc.CreateElement("FilterItem");
            filterItem.SetAttribute("Value", uxMerchantListGrid.AS_FilterExpression ?? string.Empty);
            root.AppendChild(filterItem);

            var sortItem = doc.CreateElement("SortItem");
            sortItem.SetAttribute("Value", uxMerchantListGrid.AS_SortExpression ?? string.Empty);
            root.AppendChild(sortItem);

            return doc.OuterXml;
        }

        private static void AppendParamInput(XmlDocument doc, XmlElement parent, string key, string value)
        {
            var item = doc.CreateElement("ParamInput");
            if (!string.IsNullOrEmpty(key))
            {
                item.SetAttribute("Key", key);
            }
            item.SetAttribute("Value", value);
            parent.AppendChild(item);
        }


        private void ShowFullTaxColumn(List<MerchantColumn> columns, bool isShow)
        {
            if (columns != null && columns.Any())
            {
                var taxViewColumns = columns.Where(x => x.IsTaxView).ToList();
                if (taxViewColumns != null && taxViewColumns.Any())
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        MerchantColumn item = columns[i];
                        //display tax & partial tax column
                        var taxColumn = FindGridColumn(uxMerchantListGrid, item.UniqueName);
                        var partialTaxColumn = FindGridColumn(uxMerchantListGrid, "Partial" + item.UniqueName);

                        if (taxColumn != null && partialTaxColumn != null)
                        {
                            if (isShow)
                            {
                                taxColumn.Visible = true;
                                partialTaxColumn.Visible = false;
                            }
                            else
                            {
                                taxColumn.Visible = false;
                                partialTaxColumn.Visible = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
