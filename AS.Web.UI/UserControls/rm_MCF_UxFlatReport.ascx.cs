using System;
using System.Data;
using System.Web;
using System.Web.UI;
using AS.Common.DBManager;
using AS.Common;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Newtonsoft.Json;
using AS.Controls.Pages;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;

namespace As.VisionWeb.Web
{
    public partial class FlatReportControl : GlobalUserControl
    {
        #region ---- Variable & Enum ----

        #region Enums
        enum DataBindAction
        {
            BindMerchantInfo,
            BindBarometer
        }
        enum PostBackAction
        {
            Export,
            SelectedAccountNumber
        }
        #endregion

        #region Properties
        private const int PAGE_SIZE = 1;
        private const string SPA_GET_SECURITY_REPORT = "spa_RM_MCF_Get_SecurityReport";

        public int PageNo
        {
            get
            {
                if (ViewState["PageNo"] != null)
                    return ViewState["PageNo"].ToInt();
                return 1;
            }
            set
            {
                ViewState["PageNo"] = value;
            }
        }

        public bool HasRQColumn
        {
            get
            {
                return ReportDate == DateTime.Today
                    && GeneralFuncsLib.HasQueuingMechanismFeature;
            }
        }

        public int AssignmentID
        {
            get
            {

                if (!string.IsNullOrEmpty(Page.SecureQueryString["AssignmentID"]))
                {
                    return Convert.ToInt32(Page.SecureQueryString["AssignmentID"]);
                }
                else
                {
                    return -1;
                }
            }
        }

        public string AssignmentName
        {
            get
            {

                if (!string.IsNullOrEmpty(Page.SecureQueryString["AssignmentName"]))
                {
                    return Convert.ToString(Page.SecureQueryString["AssignmentName"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private bool IsWQ
        {
            get
            {
                return _AssignmentType == WebSiteEnums.AssignmentType.WorkQueue;
            }
        }

        private WebSiteEnums.AssignmentType _AssignmentType
        {
            get
            {
                if (!string.IsNullOrEmpty(Page.SecureQueryString["AssignmentType"]))
                {
                    return (WebSiteEnums.AssignmentType)Enum.Parse(typeof(WebSiteEnums.AssignmentType), Page.SecureQueryString["AssignmentType"]);
                }
                else
                {
                    return WebSiteEnums.AssignmentType.All;
                }
            }
        }
        public string Header { get { return string.Format("{0} ({1})", AssignmentName, ReportDate.ToString(WebSiteConstants.DATE_FORMAT)); } }

        public DateTime ReportDate
        {
            get
            {

                if (!string.IsNullOrEmpty(Page.SecureQueryString["ReportDate"]))
                {
                    return Convert.ToDateTime(Page.SecureQueryString["ReportDate"]);
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }

        public string ApplyFilterId
        {
            get
            {

                if (!string.IsNullOrEmpty(Page.SecureQueryString["ApplyFilterId"]))
                {
                    return Convert.ToString(Page.SecureQueryString["ApplyFilterId"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string OrderBy
        {
            get
            {

                if (!string.IsNullOrEmpty(Page.SecureQueryString["OrderBy"]))
                {
                    return Convert.ToString(Page.SecureQueryString["OrderBy"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public int CustomViewID
        {
            get
            {
                var session = RiskSessionManager.RiskMCFDQRainbowReport;
                if (session != null && session.ContainsKey(AssignmentID))
                {
                    return Convert.ToInt32(session[AssignmentID]);
                }
                return -1;
            }

        }

        public WebSiteEnums.MCF_MerchantWorkingStatus FilterWorkingStatus
        {
            get
            {
                if (!string.IsNullOrEmpty(filterWorkingStatus.Value))
                {
                    return (WebSiteEnums.MCF_MerchantWorkingStatus)Convert.ToInt32(filterWorkingStatus.Value);
                }
                else
                {
                    return WebSiteEnums.MCF_MerchantWorkingStatus.All;
                }
            }
        }

        public string MerchantNumber
        {
            get
            {
                return RiskSessionManager.MCF_Security_CurrentMerchant;
            }
            set
            {
                RiskSessionManager.MCF_Security_CurrentMerchant = value;
            }
        }

        private static DataTable _dispositionList
        {
            get
            {
                var disposition = RiskSessionManager.Risk_MCF_DispositionList;
                if (disposition == null)
                    return RiskSessionManager.Risk_MCF_DispositionList = GetAllDispostion();

                return RiskSessionManager.Risk_MCF_DispositionList;
            }
        }

        public bool IsCSViewFullCard
        {
            get
            {
                return GeneralFuncsLib.CheckCSViewFullCard(Page);
            }
        }

        private bool IsReloadRequeue = false;

        #endregion

        #endregion ---- Variable & Enum ----

        #region ---- Event Handles ----

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateTemporaryWorkQueueSession();
                GetData();
            }

            if (IsPostBack)
            {
                var eventtarget = this.Request.Form["__EVENTTARGET"];
                if (!string.IsNullOrEmpty(eventtarget) && eventtarget.EndsWith(btnSecurityRebindCustomView.ID))
                {
                    uxCustomView.IsRebindData = true;
                }
            }

            SetInfo();
        }

        protected override void OnDataBindControls(Enum type, object sender)
        {
            if (Page.IsIntruderDetected) return;
            switch ((DataBindAction)type)
            {
                case DataBindAction.BindMerchantInfo:
                    {
                        GetSecurityReport();
                        if (!string.IsNullOrEmpty(Header.Trim()))
                            litGridTitle.Text = VeraCodeSolution.DoVeraCode(Header);
                    }
                    break;
                case DataBindAction.BindBarometer:
                    {
                        if (!string.IsNullOrEmpty(MerchantNumber))
                        {
                            DataTable data = GetBarometerInfo(MerchantNumber);
                            uxBarometerGrid.DataSource = data;
                            uxBarometerGrid.DataBind();

                            // Reload Work Button
                            if (IsReloadRequeue)
                            {
                                var rowItem = data.Rows[0];
                                var currentStatus = (WebSiteEnums.WorkStatus)Enum.Parse(typeof(WebSiteEnums.WorkStatus), rowItem["CurrentStatus"].ToString());
                                if (!string.IsNullOrEmpty(rowItem["WorkingMerchantID"].ToString()))
                                {
                                    var workStateID = string.IsNullOrEmpty(rowItem["WorkStateID"].ToString()) ?
                                    0 : int.Parse(rowItem["WorkStateID"].ToString());

                                    hddCurrentStatus.Value = workStateID.ToString();

                                    Dictionary<string, string> _dicAttrToolTip = new Dictionary<string, string>
                                {
                                    { "CycleID", rowItem["CycleID"].ToString() },
                                    { "ParentCycleID", rowItem["ParentCycleID"].ToString() },
                                    { "MerchantNumber", rowItem["MerchantNumber"].ToString() },
                                    { "TodayVolume", rowItem["todayVolume"].ToString() },
                                    { "AssignmentID", AssignmentID.ToString() },
                                    { "ReportDate", ReportDate.ToString() },
                                    { "WorkStateID", workStateID.ToString() },
                                    { "WorkingStatusMessage", rowItem["WorkingStatusMessage"].ToString() },
                                    { "WorkingMerchantID", rowItem["WorkingMerchantID"].ToString() },
                                    { "CurrentStatus", currentStatus.ToString() }
                                };

                                    AS.Controls.Global.ASMCFWorkContent lblWork = new AS.Controls.Global.ASMCFWorkContent
                                    {
                                        DicAttrToolTip = _dicAttrToolTip,
                                        DispositionChecked = rowItem["DispositionIDs"].ToString(),
                                        DispositionTable = _dispositionList,
                                        PageSection = WebSiteEnums.PageSectionEnums.SecurityReport
                                    };
                                    uxWork.Controls.Clear();
                                    uxWork.Controls.Add(lblWork);
                                }

                                IsReloadRequeue = false;
                            }
                        }
                    }
                    break;
            }
        }

        protected override void OnPostBackActions(Enum type, object sender)
        {
            if (Page.IsIntruderDetected) return;
            switch ((PostBackAction)type)
            {
                case PostBackAction.Export:
                    _isExporting = true;

                    //Step1: Create file on WS
                    string fileName = Guid.NewGuid().ToString() + GeneralFuncsLib.FormatFileName(Header) + ".xls";
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
                    parameters.Add(new FilterParameter("@AssignmentID", AssignmentID, DbType.Int32));
                    parameters.Add(new FilterParameter("@Mode", (int)FilterWorkingStatus, DbType.Int32));
                    parameters.Add(new FilterParameter("@ApplyFilterId", ApplyFilterId, DbType.String));
                    parameters.Add(new FilterParameter("@IsPaging", false, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsCountPageTotal", false, DbType.Boolean));
                    parameters.Add(new FilterParameter("@PageSize", 0, DbType.Int32));
                    parameters.Add(new FilterParameter("@PageNo", 0, DbType.Int32));
                    parameters.Add(new FilterParameter("@stOrder", OrderBy, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@stFilter", string.Empty, DbType.AnsiString));

                    if (IsCSViewFullCard)
                    {
                        parameters.AddDecryptDataParams("AccountNumber", _isExporting);
                    }

                    parameters.Add(new FilterParameter("@IsExport", 1, DbType.Boolean));
                    parameters.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));

                    // Get Resource
                    string resources = this.GetResources();
                    int cusTomView = CustomViewID;
                    Dictionary<string, RiskCustomizeColumn> dicColumnCustomView = RM_MCF_GeneralFuncsLib.GetColumnCustomView(cusTomView);

                    WebServices.RiskServices.ExportFlatReport(SPA_GET_SECURITY_REPORT, parameters, fileName, resources, SessionManager.CurrencyFortmat, HasRQColumn, JsonConvert.SerializeObject(dicColumnCustomView));
                    //Step 2: Download file from WS and transfer to client
                    var assignmentName = HttpUtility.UrlEncode(AssignmentName);
                    string queryString = this.Page.BuildSecureQueryString(string.Format("fn={0}&AssignmentName={1}&ReportDate={2}", HttpUtility.UrlEncode(fileName), assignmentName, ReportDate));
                    Response.Redirect("rm_MCF_ExportFlatReport.aspx?" + queryString);
                    break;
                case PostBackAction.SelectedAccountNumber:
                    string queryStr = uxAccountValue.Value;
                    Response.Redirect(queryStr, true);
                    break;
            }
        }

        bool _isExporting = false;
        protected void ImageButtonExcel_Click(object sender, EventArgs e)
        {
            OnPostBackActions(PostBackAction.Export);
        }

        protected void uxMerchantInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView rowItem = e.Item.DataItem as DataRowView;

                var currentStatus = (WebSiteEnums.WorkStatus)Enum.Parse(typeof(WebSiteEnums.WorkStatus), rowItem["CurrentStatus"].ToString());

                if (!string.IsNullOrEmpty(rowItem["WorkingMerchantID"].ToString()))
                {
                    var workStateID = string.IsNullOrEmpty(rowItem["WorkStateID"].ToString()) ?
                    0 : int.Parse(rowItem["WorkStateID"].ToString());

                    hddCurrentStatus.Value = workStateID.ToString();

                    Dictionary<string, string> _dicAttrToolTip = new Dictionary<string, string>
                {
                    { "CycleID", rowItem["CycleID"].ToString() },
                    { "ParentCycleID", rowItem["ParentCycleID"].ToString() },
                    { "MerchantNumber", rowItem["MerchantNumber"].ToString() },
                    { "TodayVolume", rowItem["todayVolume"].ToString() },
                    { "AssignmentID", AssignmentID.ToString() },
                    { "ReportDate", ReportDate.ToString() },
                    { "WorkStateID", workStateID.ToString() },
                    { "WorkingStatusMessage", rowItem["WorkingStatusMessage"].ToString() },
                    { "WorkingMerchantID", rowItem["WorkingMerchantID"].ToString() },
                    { "CurrentStatus", currentStatus.ToString() }
                };

                    AS.Controls.Global.ASMCFWorkContent lblWork = new AS.Controls.Global.ASMCFWorkContent
                    {
                        DicAttrToolTip = _dicAttrToolTip,
                        DispositionChecked = rowItem["DispositionIDs"].ToString(),
                        DispositionTable = _dispositionList,
                        PageSection = WebSiteEnums.PageSectionEnums.SecurityReport
                    };
                    uxWork.Controls.Clear();
                    uxWork.Controls.Add(lblWork);
                }

                LinkButton uxMerchantNumLink = e.Item.FindControl("uxMerchantNumLink") as System.Web.UI.WebControls.LinkButton;
                if (SessionManager.CurrentUserPermissions.Contains(WebSiteConstants.VIEW_RISK_REPORT))
                {
                    uxMerchantNumLink.OnClientClick = "openPopupWindow('rm_MCF_RiskReport.aspx?" + Page.BuildSecureQueryString(string.Format("merchantNumber={0}&IsPopup=true", rowItem["MerchantNumber"].ToString())) + "','RiskReport'); return false;";
                }
                uxMerchantNumLink.Text = string.Format("{0}<span class='inline-block text-right pull-right'><strong class='red'>{1}</strong></span>", rowItem["MerchantNumber"].ToString(), rowItem["Watch"].ToString());

                Label lblProfile = e.Item.FindControl("lblProfile") as Label;
                string profileDescription = rowItem["ProfileDescription"].ToString();
                lblProfile.Text = VeraCodeSolution.DoVeraCode(!string.IsNullOrEmpty(profileDescription) ? profileDescription : WebSiteConstants.HTML_EM_DASH_ENCODE.ToString());
                if (rowItem["ProfileType"].ToString() != "0")
                    lblProfile.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["ProfileType"] + " - " + rowItem["ProfileDescription"]);

                Label lblSIC = e.Item.FindControl("lblSIC") as Label;
                if (!string.IsNullOrEmpty(rowItem["SIC"].ToString()))
                {
                    lblSIC.Text = VeraCodeSolution.DoVeraCode(rowItem["SIC"].ToString());
                    lblSIC.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["SIC"].ToString());
                }
                else
                {
                    lblSIC.Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
                }

                Label lblHierarchyName = e.Item.FindControl("lblHierarchyName") as Label;
                lblHierarchyName.Text = VeraCodeSolution.DoVeraCode(rowItem["HierarchyName"].ToString());
                if (!string.IsNullOrEmpty(rowItem["HierarchyName"].ToString()))
                    lblHierarchyName.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["HierarchyName"].ToString());

                Label lblHierarchyValue = e.Item.FindControl("lblHierarchyValue") as Label;
                lblHierarchyValue.Text = VeraCodeSolution.DoVeraCode(rowItem["HierarchyValue"].ToString());
                if (!string.IsNullOrEmpty(rowItem["HierarchyValue"].ToString()))
                    lblHierarchyValue.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["HierarchyValue"].ToString());

                Label lblClassificationName = e.Item.FindControl("lblClassificationName") as Label;
                if (!string.IsNullOrEmpty(rowItem["ClassificationName"].ToString()))
                {
                    lblClassificationName.Text = VeraCodeSolution.DoVeraCode(rowItem["ClassificationName"].ToString());
                    lblClassificationName.ToolTip = VeraCodeSolution.DoVeraCode(rowItem["ClassificationName"].ToString());
                }
                else
                {
                    lblClassificationName.Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
                }

                colRQColumn.Visible = HasRQColumn;
                colRQColumn.Attributes.Add("style", "");
                if (bool.Parse(rowItem["IsRequeued"].ToString()))
                {
                    colRQColumn.Attributes.Add("style", "background-color: #BBB;");
                }

                if (HasRQColumn)
                {
                    HtmlInputCheckBox chkRequeueSingleMerchant = colRQColumn.FindControl("chkItemCV") as HtmlInputCheckBox;
                    if (chkRequeueSingleMerchant != null)
                    {
                        chkRequeueSingleMerchant.Checked = false;
                        if (currentStatus != WebSiteEnums.WorkStatus.WorkInProgressByOther)
                        {
                            chkRequeueSingleMerchant.Value = rowItem["MerchantNumber"].ToString();
                            chkRequeueSingleMerchant.Attributes.Add("onclick", string.Format("RequeueSingleMerchant(this," + IsWQ.ToString().ToLower() + "," + rowItem["CycleID"].ToString() + "," + rowItem["ParentCycleID"].ToString() + ")"));
                            chkRequeueSingleMerchant.Disabled = !GeneralFuncsLib.ReadOnlyRiskManagementEnableStatus(Page);
                            chkRequeueSingleMerchant.Visible = true;
                        }
                        else
                        {
                            chkRequeueSingleMerchant.Attributes.Add("rqwip", "other");
                            chkRequeueSingleMerchant.Attributes.Add("disabled", "disabled");
                        }
                    }
                }

                //Format data
                Label uxTodayVolume = e.Item.FindControl("uxTodayVolume") as Label;
                FormatCurrency(uxTodayVolume, VeraCodeSolution.DoVeraCode(rowItem["TodayVolume"].ToString()));
                Label uxDailyVolume = e.Item.FindControl("uxDailyVolume") as Label;
                FormatCurrency(uxDailyVolume, VeraCodeSolution.DoVeraCode(rowItem["ExpectedDailyVolume"].ToString()));
                Label uxTodayAverageTicket = e.Item.FindControl("uxTodayAverageTicket") as Label;
                FormatCurrency(uxTodayAverageTicket, VeraCodeSolution.DoVeraCode(rowItem["TodayAverageTicket"].ToString()));
                Label uxTodayHighestTransactionAmount = e.Item.FindControl("uxTodayHighestTransactionAmount") as Label;
                FormatCurrency(uxTodayHighestTransactionAmount, VeraCodeSolution.DoVeraCode(rowItem["TodayHighestTransactionAmount"].ToString()));
                Label uxContractHighestTicket = e.Item.FindControl("uxContractHighestTicket") as Label;
                FormatCurrency(uxContractHighestTicket, VeraCodeSolution.DoVeraCode(rowItem["ContractHighestTicket"].ToString()));
                Label uxExpectedAverageTicket = e.Item.FindControl("uxExpectedAverageTicket") as Label;
                FormatCurrency(uxExpectedAverageTicket, VeraCodeSolution.DoVeraCode(rowItem["ExpectedAverageTicket"].ToString()));
                Label uxMTDVolume = e.Item.FindControl("uxMTDVolume") as Label;
                FormatCurrency(uxMTDVolume, VeraCodeSolution.DoVeraCode(rowItem["MTDVolume"].ToString()));
                Label uxMV1 = e.Item.FindControl("uxMV1") as Label;
                FormatCurrency(uxMV1, VeraCodeSolution.DoVeraCode(rowItem["MV1"].ToString()));
                Label uxYTDVolume = e.Item.FindControl("uxYTDVolume") as Label;
                FormatCurrency(uxYTDVolume, VeraCodeSolution.DoVeraCode(rowItem["YTDVolume"].ToString()));
                Label uxMV2 = e.Item.FindControl("uxMV2") as Label;
                FormatCurrency(uxMV2, VeraCodeSolution.DoVeraCode(rowItem["MV2"].ToString()));
                Label uxMV3 = e.Item.FindControl("uxMV3") as Label;
                FormatCurrency(uxMV3, VeraCodeSolution.DoVeraCode(rowItem["MV3"].ToString()));
                Label uxMonthlyNetAmtContract = e.Item.FindControl("uxMonthlyNetAmtContract") as Label;
                FormatCurrency(uxMonthlyNetAmtContract, VeraCodeSolution.DoVeraCode(rowItem["ExpectedMonthlyVolume"].ToString()));

                Label uxKeyedTransPctContract = e.Item.FindControl("uxKeyedTransPctContract") as Label;
                var KeyedTransPctContract = rowItem["ExpectedPercentSWP"].ToString();
                KeyedTransPctContract = !string.IsNullOrEmpty(KeyedTransPctContract) ? AS.Common.Formater.FormatData.FormatPercent(KeyedTransPctContract) : WebSiteConstants.HTML_EM_DASH_ENCODE;
                uxKeyedTransPctContract.Text = KeyedTransPctContract;

                var KeyedTransPctToday = rowItem["KeyPercent"].ToString();
                KeyedTransPctToday = !string.IsNullOrEmpty(KeyedTransPctToday) ? AS.Common.Formater.FormatData.FormatPercent(KeyedTransPctToday) : WebSiteConstants.HTML_EM_DASH_ENCODE;
                Label uxKeyedTransPctToday = e.Item.FindControl("uxKeyedTransPctToday") as Label;
                uxKeyedTransPctToday.Text = KeyedTransPctToday;
            }
        }

        protected void uxBarometerGrid_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.AlternatingItem:
                case ListItemType.Item:
                    {
                        RepeaterItem dataItem = e.Item;
                        var rowItem = (e.Item.DataItem as DataRowView).Row;
                        HtmlGenericControl itemTemplate = new HtmlGenericControl
                        {
                            InnerHtml = RM_MCF_GeneralFuncsLib.GenGridBarometerNextQueue(uxBarometerGrid, CustomViewID, rowItem)
                        };
                        dataItem.Controls.Add(itemTemplate);
                    }
                    break;
            }
        }

        protected string ConvertDate(object date)
        {
            if (date == DBNull.Value)
                return string.Empty;
            return Convert.ToDateTime(date).ToString("MM/dd/yyyy");
        }

        protected void uxAccount_Click(object sender, EventArgs e)
        {
            OnPostBackActions(PostBackAction.SelectedAccountNumber);
        }

        protected void btnFilterWorkingStatus_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            CreateTemporaryWorkQueueSession();
            GetData();
        }

        protected void uxOpenWarning_Click(object sender, EventArgs e)
        {
            var message = Page.BuildSecureQueryString("Message=" + hddMessage.Value);
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"openWarning('" + message + "');"));
        }

        protected void uxRefreshBtn_Click(object sender, EventArgs e)
        {
            // Method intentionally left empty.
        }

        protected void uxRefreshPage_Click(object sender, EventArgs e)
        {
            GetData();
        }

        protected void uxAccountNumberClick_Click(object sender, EventArgs e)
        {
            string[] arg = uxHiddenAccountNumberClick.Value.Split(';');
            string reportType = arg[1];
            string recordID = arg[2];
            string partialCardNum = arg[3];
            DateTime rpDate;
            if (!DateTime.TryParse(arg[4], out rpDate))
            {
                rpDate = DateTime.Now;
            }
            string merchantNumber = arg[5];

            string fullCC = string.Empty;
            FilterParameterCollection parameters_FullCard = new FilterParameterCollection
        {
            new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString),
            new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32),
            new FilterParameter("@RecordID", Convert.ToInt32(recordID), DbType.Int32),
            new FilterParameter("@ReportType", reportType, DbType.AnsiString),
            new FilterParameter("@ReportDate", rpDate, DbType.DateTime)
        };

            if (IsCSViewFullCard)
            {
                parameters_FullCard.AddDecryptDataParams("AccountNumber");
            }

            DataTable dt_FullCard = WebServices.CsReportServices.GetReports("spa_cs_GetFullCardNumber", parameters_FullCard);
            if (dt_FullCard != null && dt_FullCard.Rows.Count > 0)
            {
                fullCC = dt_FullCard.Rows[0][0].ToString();
            }

            string queryString = Page.BuildSecureQueryString("cn=" + partialCardNum + "&cnf=" + fullCC + "&merch=" + merchantNumber + "&isRisk=1");
            string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
            Telerik.Web.UI.RadAjaxManager ajax = Telerik.Web.UI.RadAjaxManager.GetCurrent(Page);
            ajax.ResponseScripts.Add("openPopupCardWindow('" + urlCardDetail + "');");
        }

        protected void uxAuthClick_Click(object sender, EventArgs e)
        {
            string[] arg = uxHiddenAuthClick.Value.Split(';');

            string pageUrl = ResolveUrl("~/") + "risk_MCF/rm_MCF_AuthorizationDetailsModal.aspx?";
            string param = Page.BuildSecureQueryString(
                string.Format("AuthNumber={0}&merch={1}&merchname={2}&transactiondate={3}&idx={4}",
                    arg[0],
                    arg[1],
                    string.Empty,
                    arg[2],
                    1));

            string urlAuth = string.Format("{0}{1}", pageUrl, param);
            Telerik.Web.UI.RadAjaxManager ajax = Telerik.Web.UI.RadAjaxManager.GetCurrent(this.Page);
            ajax.ResponseScripts.Add(string.Format("ShowPopupModalChild('{0}', '{1}', 'auto')", 1, urlAuth));
        }

        protected void btnReloadCustomView_Click(object sender, EventArgs e)
        {
            OnDataBindControls(DataBindAction.BindBarometer);
        }
        protected void btnReloadRequeue_Click(object sender, EventArgs e)
        {
            // Reload Work button
            IsReloadRequeue = true;

            //Bind Merchant Information
            OnDataBindControls(DataBindAction.BindBarometer);
        }
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo = hddCurrentPageNo.Value.ToInt() - 1;
            CreateTemporaryWorkQueueSession();
            GetData();
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (!btnNext.Enabled)
            {
                string message = String.Format("alert('{0}');", GetLocalResourceObject("msgNoMerchant")).ToString();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlerMessage", message, true);
            }

            CheckPageNo();
            CreateTemporaryWorkQueueSession();
            GetData();
        }
        protected void btnLoadChargebacks_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MerchantNumber))
            {
                Control control = Page.LoadControl("~/UserControls/rm_MCF_Chargebacks.ascx");
                UserControls_rm_MCF_Chargebacks userControl = ((UserControls_rm_MCF_Chargebacks)control);
                userControl.MerchantList = MerchantNumber;
                userControl.FilterWorkingStatus = (int)FilterWorkingStatus;
                userControl.ReportDate = ReportDate;
                userControl.IsCSViewFullCard = IsCSViewFullCard;

                pnlChargeback.Controls.Add(userControl);

                ReportPage page = (ReportPage)this.Page;
                page.AjaxAddResponseScript("LoadTransaction()");
            }
        }
        protected void btnLoadTransaction_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MerchantNumber))
            {
                Control control = Page.LoadControl("~/UserControls/rm_MCF_Transaction.ascx");
                UserControls_rm_MCF_Transaction userControl = ((UserControls_rm_MCF_Transaction)control);
                userControl.MerchantList = MerchantNumber;
                userControl.FilterWorkingStatus = (int)FilterWorkingStatus;
                userControl.ReportDate = ReportDate;
                userControl.IsCSViewFullCard = IsCSViewFullCard;
                userControl.PageIndex = 1;

                pnlTransaction.Controls.Add(userControl);
            }
        }

        #endregion ---- Event Handles ----

        #region Public Methods

        public string BuildUrlImage(string currentMerchantNumber)
        {
            string queryStringImage = this.Page.BuildSecureQueryString("MerchantNumber=" + currentMerchantNumber + "&ReportDate=" + ReportDate + "&AssignmentID=" + AssignmentID);
            return "rm_MCF_DQReasonModal.aspx?" + queryStringImage;
        }

        #endregion

        #region Private Methods

        private static DataTable GetAllDispostion()
        {
            FilterParameterCollection parameterList = new FilterParameterCollection();
            parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            return WebServices.RiskServices.GetReports("spa_RM_MCF_GetAllDispositionForReport", parameterList);
        }

        private DataTable GetBarometerInfo(string merchantList)
        {
            FilterParameterCollection parameterList = new FilterParameterCollection();
            parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameterList.AddLanguageID();
            parameterList.Add(new FilterParameter("@AssignmentID", AssignmentID, DbType.Int32));
            parameterList.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
            parameterList.Add(new FilterParameter("@Mode", (int)FilterWorkingStatus, DbType.Int32));
            parameterList.Add(new FilterParameter("@MerchantNumber", merchantList, DbType.String));

            return WebServices.RiskServices.GetReports("spa_RM_MCF_Get_NextQueueReport_Barometer", parameterList);
        }

        private string GetResources()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string filePath = string.Format("{0}UserControls/App_LocalResources/rm_MCF_UxFlatReport.ascx{1}.resx", ResolveUrl("~/"), (SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.English ? "" : "." + CultureInfo.CurrentUICulture.ToString()));
            XmlDocument xDoc = new XmlDocument
            {
                XmlResolver = null
            };
            xDoc.Load(Server.MapPath(filePath));
            XmlNodeList nList = xDoc.SelectNodes("root/data");

            foreach (XmlNode node in nList)
            {
                dic.Add(node.Attributes[0].Value, node.InnerText);
            }

            string resource = JsonConvert.SerializeObject(dic, Newtonsoft.Json.Formatting.Indented);

            return resource;
        }

        private void FormatCurrency(Label sender, string value)
        {
            if (value.IsNullOrEmpty())
            {
                sender.Text = WebSiteConstants.HTML_EM_DASH_ENCODE;
                return;
            }
            double temp = double.Parse(value);
            if (temp < 0)
            {
                sender.ForeColor = Color.Red;
                temp *= -1;
                sender.Text = "(" + AS.Common.Formater.FormatData.FormatCurrency(temp) + ")";
            }
            else
            {
                sender.Text = AS.Common.Formater.FormatData.FormatCurrency(temp);
            }
        }

        private DataTable GetMerchantList()
        {
            FilterParameterCollection parameterList = new FilterParameterCollection();
            parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameterList.AddLanguageID();
            parameterList.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
            parameterList.Add(new FilterParameter("@AssignmentID", AssignmentID, DbType.Int32));
            parameterList.Add(new FilterParameter("@Mode", (int)FilterWorkingStatus, DbType.Int32));
            parameterList.Add(new FilterParameter("@ApplyFilterId", ApplyFilterId, DbType.String));

            parameterList.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
            parameterList.Add(new FilterParameter("@IsCountPageTotal", 0, DbType.Boolean));
            parameterList.Add(new FilterParameter("@PageSize", PAGE_SIZE, DbType.Int32));
            parameterList.Add(new FilterParameter("@PageNo", PageNo, DbType.Int32));
            parameterList.Add(new FilterParameter("@stOrder", OrderBy, DbType.AnsiString));
            parameterList.Add(new FilterParameter("@stFilter", "", DbType.AnsiString));

            if (IsCSViewFullCard)
            {
                parameterList.AddDecryptDataParams("AccountNumber", _isExporting);
            }
            parameterList.Add(new FilterParameter("@IsExport", 0, DbType.Boolean));

            return WebServices.RiskServices.GetReports(SPA_GET_SECURITY_REPORT, parameterList);
        }

        private void GetSecurityReport()
        {
            int totalRows = 0;
            DataTable merchantList = GetMerchantList();
            if (merchantList != null && merchantList.HasData())
            {
                int.TryParse(merchantList.Rows[0]["TotalRows"].ToString(), out totalRows);
                MerchantNumber = merchantList.Rows[0]["MerchantNumber"].ToString();

                uxMerchantInfo.DataSource = merchantList;
                uxMerchantInfo.DataBind();

                ReportPage page = (ReportPage)this.Page;
                page.AjaxAddResponseScript("LoadChargeback();");
            }

            if (totalRows > 0)
            {
                ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("btnExportShow()");
                pnlFlatReportInfo.Visible = true;
                pnlNoDataFound.Visible = false;

                hddCurrentPageNo.Value = PageNo.ToString();
                btnNext.Enabled = !(totalRows == PageNo || totalRows == 1);
                btnPrevious.Enabled = PageNo != 1;
            }
            else
            {
                ((BaseMasterPage)Page.Master).AjaxAddResponseScript("btnExportHide()");
                pnlFlatReportInfo.Visible = false;
                pnlNoDataFound.Visible = true;
            }
        }

        private void GetData()
        {
            //Bind Merchant Information
            OnDataBindControls(DataBindAction.BindMerchantInfo);

            //Bind Barometer
            OnDataBindControls(DataBindAction.BindBarometer);
        }

        private void CreateTemporaryWorkQueueSession()
        {
            RM_MCF_GeneralFuncsLib.CreateTemporaryWorkQueueSession(ReportDate, AssignmentID, ApplyFilterId, WebSiteEnums.PAGE_CODE.SR);
        }

        private void SetInfo()
        {
            btnAddWorkQueue.OnClientClick = RM_MCF_GeneralFuncsLib.BuildUrlRequeue(true, ReportDate, AssignmentID, FilterWorkingStatus, ApplyFilterId, WebSiteEnums.PAGE_CODE.SR);
            btnRemoveWorkQueue.OnClientClick = RM_MCF_GeneralFuncsLib.BuildUrlRequeue(false, ReportDate, AssignmentID, FilterWorkingStatus, ApplyFilterId, WebSiteEnums.PAGE_CODE.SR);
            optRequeued.Visible = GeneralFuncsLib.HasQueuingMechanismFeature;
        }

        private void CheckPageNo()
        {
            string currentStatus = hddCurrentStatus.Value;
            string convertFilterStatus;
            if ((int)FilterWorkingStatus == 1)
            {
                convertFilterStatus = "2";
            }
            else if ((int)FilterWorkingStatus == 2)
            {
                convertFilterStatus = "1";
            }
            else
            {
                convertFilterStatus = "0";
            }

            if (FilterWorkingStatus != WebSiteEnums.MCF_MerchantWorkingStatus.All &&
                FilterWorkingStatus != WebSiteEnums.MCF_MerchantWorkingStatus.Requeued
                && currentStatus != convertFilterStatus)
            {
                PageNo = hddCurrentPageNo.Value.ToInt();
            }
            else
            {
                PageNo = hddCurrentPageNo.Value.ToInt() + 1;
            }
        }
        #endregion
    }
}