using AS.Common;
using AS.Common.DataProtection;
using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.Validators;
using AS.LoneStar.Client.StatementApi;
using AS.LoneStar.Client.StatementApi.AccessOneApi;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.ExportInfrastructure;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using AS.VW.StatementCommon;

using ASGrid = AS.Controls.Grid.ASGrid;
using Button = System.Web.UI.WebControls.Button;
using LinkButton = System.Web.UI.WebControls.LinkButton;
using Literal = System.Web.UI.WebControls.Literal;
using RadComboBoxItem = Telerik.Web.UI.RadComboBoxItem;
using RadToolTip = Telerik.Web.UI.RadToolTip;

namespace As.VisionWeb.Web
{
    [PagePermission("StatementRpt,MSStatementRpt")]
    public partial class Statement : ReportPage
    {
        #region Enums & Constants
        private const string FmDateTime = "MM/dd/yyyy hh:mm tt";
        private const string CHAIN_MODE = "CHAIN";
        private const string ACHMERCHANT_MODE = "MERCHANT";
        enum DataBindAction
        {
            BindDrilldownGrid,
            BindDrilldownGridForAllChainStatements,
            BindDrillACHMerchant,
            ExtendMerchant

        }

        enum PostBackAction
        {
            ViewStatement,
            ViewStatementFIS,
            ViewChainStatement,
        }

        enum BEProcessor
        {
            FD,
            TSYS
        }

        private const string STATEMENT_VIEW_UPLOADED_FILE = "1";
        private const string STATEMENT_VIEW_DATA = "2";
        private const string STATEMENT_HISTORICAL = "HistoricalStatement";
        private const string STATEMENT_DATE = "STMT - ";
        private const string SPA_NAME_CHECK_BELONG_TO = "spa_CheckMerchantBelongtoUser";
        private const string IS_BELONG_TO = "IsBelongTo";
        private const int USER_HIERACHY_MODE_CHAIN = 11;

        #endregion Enums & Constants

        #region Fields

        private string _reportDateFormat = null;
        private string _statementDisplayFormat = null;
        private bool isEnhancement
        {
            get
            {
                // EntityType MIC Chain: 11, Chain:5
                return GeneralFuncsLib.IsEnhancementFeature() && SessionManager.CurrentClient == 22 &&
                       (SessionManager.CurrentUser.EntityType == 11 || SessionManager.CurrentUser.EntityType == 5);
            }
        }
        #endregion Fields

        #region Properties

        private string GridTitle
        {
            get
            {
                return GeneralFuncsLib.GetFullGridTitleName(ReportFilter);
            }
        }

        private string ReportDateFormat
        {
            get
            {
                if (_reportDateFormat.IsNullOrEmpty())
                {
                    _reportDateFormat = GeneralFuncsLib.GetStatementReportDateFormat();
                }
                return _reportDateFormat;
            }
        }

        private string StatementDisplayFormat
        {
            get
            {
                if (_statementDisplayFormat.IsNullOrEmpty())
                {
                    _statementDisplayFormat = GeneralFuncsLib.GetStatementDisplayFormat();
                }
                return _statementDisplayFormat;
            }
        }

        public bool IsExistFile
        {
            get
            {
                return SessionManager.CM_IsExistStatement;
            }
            set { SessionManager.CM_IsExistStatement = value; }
        }

        //42782 – VW – CAYAN - Implement New TSYS Processing Platform - modify

        #endregion Properties

        #region Methods

        // 43842 - FD to TSYS Merchant Migration
        private bool EnableStatementApi(string merchantNumber)
        {
            return GeneralFuncsLib.EnabledStatementAPI(merchantNumber);
        }


        private Dictionary<string, string> _dicExtendStatement = null;
        private Dictionary<string, string> dicExtendStatement
        {
            get
            {
                if (_dicExtendStatement != null) return _dicExtendStatement;

                _dicExtendStatement = GeneralFuncsLib.DicExtendStatement;

                return _dicExtendStatement;
            }
        }

        protected override void PageInitialize()
        {
            if (IsIntruderDetected)
                return;

            this.ExporterIDs.Add("uxExportDrilldownGridTop");
            this.ExporterIDs.Add("uxExportDrillDownForAllStatements");
            this.GridIDs.Add("uxGridAllOfStatement");
            base.PageInitialize();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (isEnhancement)
                {
                    divdownload.Visible = true;
                    BindDataReportDate();
                    IsExistFile = true;
                }
                else
                {
                    divdownload.Visible = false;
                }
                if (IsSecureQueryString && SecureQueryString["dl"] != null && SecureQueryString["dl"].Trim() == "1")
                {
                    ViewStatementByDocumentID(int.Parse(SecureQueryString["StmtID"]), SecureQueryString["fileName"], SecureQueryString["tracking"], SecureQueryString["reportDate"], SecureQueryString["merchantNumber"]);
                    return;
                }

                if (IsSecureQueryString && SecureQueryString["dlAPI"] != null && SecureQueryString["dlAPI"].Trim() == "1")
                {
                    viewStatementApi(SecureQueryString["merchantNumber"]);
                }
                IsBindDataOnLoad = true;
                //Redirect from mobile site
                if (IsSecureQueryString && !String.IsNullOrEmpty(SecureQueryString["mb"]))
                {
                    string _merchantnumber = SecureQueryString["merchantnumber"];
                    this.ReportFilter.CurrentValue.Value = _merchantnumber;
                    this.ReportFilter.CurrentValue.HierarchyMode = HierarchyMode.MERCHANT_NR;
                    string hid = string.Empty;
                    DataRow dataRow = SessionManager.HierarchyFilter.Select("HierarchyMode = '" + HierarchyMode.MERCHANT_NR + "'").FirstOrDefault();
                    if (dataRow != null)
                    {
                        hid = dataRow[0].ToString();
                    }
                    this.ReportFilter.CurrentValue.ID = hid;
                }

                this.SearchChainStatement(this.ReportFilter.CurrentValue.HierarchyMode, this.ReportFilter.CurrentValue.Value);              
            }
        }

        private string GetBEProcessor(string merchantNumber, DateTime date)
        {
            DataTable dt = WebServices.CsReportServices.GetBEProcessor(SessionManager.CurrentUser.ASClient,
                SessionManager.CurrentUser.SiteID, merchantNumber, date);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["BEProcessor"].ToString();
            }
            return string.Empty;
        }

        private void LoadUserControls(string merchantNumber)
        {
            string allowBEs = GeneralFuncsLib.GetStatementBEProcessor();
            if (allowBEs.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                SearchStatement(ReportFilter.CurrentValue.Value);
            }
            else
            {
                string beProcessor = GetBEProcessor(merchantNumber, DateTime.MinValue);
                allowBEs = "," + allowBEs.Trim() + ",";
                if (allowBEs.Contains("," + beProcessor + ","))
                {
                    SearchStatement(ReportFilter.CurrentValue.Value);
                }
                else
                {
                    pnlStatement.Visible = false;
                    uxPlaceMss.Visible = true;
                }
            }
        }

        private DataTable GetStatementReportDate(string merchantNum)
        {
            return WebServices.CsReportServices.GetStatementReportDate(
                GetLoggedInUserParams(), merchantNum);
        }

        private void SearchStatement(string merchantNum)
        {
            divNoChainStatement.Visible = false;
            uxPlaceIDChainName.Visible = false;
            if (EnableStatementApi(merchantNum))
            {
                BindStatementApi(merchantNum);
            }
            else
            {
                DataTable tbl = GetStatementReportDate(merchantNum);
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    uxPlaceMss.Visible = false;
                    pnlStatement.Visible = true;

                    tbl.DefaultView.Sort = "ReportDate desc";

                    uxReportDate.Items.Clear();

                    string currentReportDate = string.Empty;
                    int countRpDate = 1;

                    foreach (DataRow row in tbl.Rows)
                    {
                        DateTime reportDate;
                        RadComboBoxItem item = null;

                        if (!DateTime.TryParse(row["ReportDate"].ToString(), out reportDate))
                        {
                            continue;
                        }

                        if (row["SecondStatementFile"].ToString() != string.Empty)
                        {
                            // statement uploaded to Document Server         
                            item = new RadComboBoxItem();
                            item.Text = string.Format(StatementDisplayFormat,
                                reportDate.ToString(ReportDateFormat),
                                "- (" + STATEMENT_VIEW_UPLOADED_FILE + ") ");
                            item.Value = string.Format("{0};{1};{2}",
                                reportDate.ToString(WebSiteConstants.DATE_FORMAT),
                                STATEMENT_VIEW_UPLOADED_FILE,
                                row["SecondStatementFile"].ToString());
                            uxReportDate.Items.Add(item);

                            // regular statement from data file
                            item = new RadComboBoxItem();
                            item.Text = string.Format(StatementDisplayFormat,
                                reportDate.ToString(ReportDateFormat),
                                "- (" + STATEMENT_VIEW_DATA + ") ");
                            item.Value = string.Format("{0};{1};{2}",
                                reportDate.ToString(WebSiteConstants.DATE_FORMAT),
                                STATEMENT_VIEW_DATA,
                                row["SecondStatementFile"].ToString());
                            uxReportDate.Items.Add(item);
                        }
                        else if (row["HistoricalStatement"].ToString() != string.Empty)
                        {
                            if (reportDate.ToString(ReportDateFormat) == currentReportDate)
                            {
                                countRpDate++;
                            }
                            else
                            {
                                countRpDate = 1;
                            }
                            currentReportDate = reportDate.ToString(ReportDateFormat);

                            item = new RadComboBoxItem();
                            if (CountStatement(reportDate, tbl) > 1)
                            {
                                item.Text = string.Format(StatementDisplayFormat,
                                    reportDate.ToString(ReportDateFormat) + " (" + countRpDate.ToString() + ")",
                                    string.Empty);
                            }
                            else
                            {
                                item.Text = string.Format(StatementDisplayFormat,
                                    reportDate.ToString(ReportDateFormat),
                                    string.Empty);
                            }
                            item.Value = string.Format("{0};{1}",
                                reportDate.ToString(WebSiteConstants.DATE_FORMAT),
                                STATEMENT_HISTORICAL);
                            uxReportDate.Items.Add(item);
                        }
                        else if (row["StatementID"].ToString() != string.Empty)
                        {
                            // Assign index of statement
                            if (reportDate.ToString(ReportDateFormat) == currentReportDate)
                            {
                                countRpDate++;
                            }
                            else
                            {
                                countRpDate = 1;
                            }
                            currentReportDate = reportDate.ToString(ReportDateFormat);

                            item = new RadComboBoxItem();

                            var isConvert = GeneralFuncsLib.GetEnabledStatementAPI();

                            if (CountStatement(reportDate, tbl) > 1)
                            {
                                string display = isConvert ? " - " + BEProcessor.TSYS : string.Empty;
                                item.Text = string.Format(StatementDisplayFormat,
                                    reportDate.ToString(ReportDateFormat) + " - " + row["FileIndex"], display);
                            }
                            else
                            {
                                string display = isConvert ? " - " + BEProcessor.TSYS : string.Empty;
                                item.Text = string.Format(StatementDisplayFormat,
                                    reportDate.ToString(ReportDateFormat),
                                    display);
                            }
                            item.Value = string.Format("{0};{1}",
                                reportDate.ToString(WebSiteConstants.DATE_FORMAT),
                                row["StatementID"].ToString());
                            uxReportDate.Items.Add(item);

                        }
                        else
                        {
                            // regular statement from data file
                            item = new RadComboBoxItem();
                            item.Text = string.Format(StatementDisplayFormat,
                                reportDate.ToString(ReportDateFormat),
                                string.Empty);
                            item.Value = string.Format("{0};{1};{2}",
                                reportDate.ToString(WebSiteConstants.DATE_FORMAT),
                                STATEMENT_VIEW_DATA, row["SecondStatementFile"].ToString());
                            uxReportDate.Items.Add(item);
                        }
                    }

                    if (uxReportDate.Items.Count > 10)
                    {
                        uxReportDate.Height = Unit.Pixel(220);
                    }
                }
                else
                {
                    pnlStatement.Visible = false;
                    uxPlaceMss.Visible = true;
                }
            }
        }

        private int CountStatement(DateTime reportDate, DataTable statements)
        {
            int count = 0;
            foreach (DataRow dr in statements.Rows)
            {
                DateTime current;
                DateTime.TryParse(dr["ReportDate"].ToString(), out current);
                if (current.Date == reportDate.Date)
                    count++;
            }
            return count;
        }      

        private string GetGridTitleResource(string mode)
        {
            string result = string.Empty;

            switch (mode)
            {
                case CHAIN_MODE:
                    result = GetLocalResourceObject("chainStatements").ToString();
                    break;
                case ACHMERCHANT_MODE:
                    result = GetLocalResourceObject("uxMerchantHierachyStatement").ToString();
                    break;
                default:
                    break;
            }

            return result;
        }

        private void SetHeaderTextLastStatement()
        {
            string header = GetHeaderText();
            uxGridAllOfStatement.Columns.FindByUniqueName("EntityNumber").HeaderText = header;
            uxGridAllOfStatement.Columns.FindByUniqueName("EntityNumber").HeaderTooltip = header;
        }

        private string GetHeaderText()
        {
            string mode = ReportFilter.CurrentValue.HierarchyMode.Equals(CHAIN_MODE) ? CHAIN_MODE : ACHMERCHANT_MODE;
            switch (mode)
            {
                case ACHMERCHANT_MODE:
                    return GetLocalResourceObject("uxMerchantEntityID").ToString();
                default:
                    return GetLocalResourceObject("uxChainID").ToString();
            }
        }

        protected void SearchChainStatement(string filterMode, string filterValue)
        {
            this.uxViewChainStatementPanel.Visible = false;
            uxPlaceHolderStatementName.Visible = false;
            divNoChainStatement.Visible = false;
            uxPlaceIDChainName.Visible = false;

            // If current client is ORION
            if (SessionManager.CurrentUser.ASClient == WebSiteConstants.ORION_CLIENT)
            {
                // Chain logged in or filter mode is chain
                string hierarchyCode = SessionManager.CurrentUserRoles[0].HierarchyCode.ToUpper();
                if (hierarchyCode.Equals("HUSERS_CHAINUSERS")
                    || hierarchyCode.Equals("HUSERS_CHAINUSERS_SEC")
                    || (filterMode.ToUpper().Equals(CHAIN_MODE) && !string.IsNullOrEmpty(filterValue)))
                {
                    string chainNumber = hierarchyCode.Equals("HUSERS_CHAINUSERS") ?
                        SessionManager.CurrentUser.UserID : filterValue;
                    if (hierarchyCode.Equals("HUSERS_CHAINUSERS_SEC"))
                    {
                        chainNumber = this.GetEntityIDOfUser(SessionManager.CurrentUser.UserID);
                    }
                    DataTable dtService = WebServices.CsReportServices.GetMonthEndChainStatementSummary(
                        GetLoggedInUserParams(), chainNumber);
                    if (dtService != null && dtService.Rows.Count > 0)
                    {
                        this.uxViewChainStatementPanel.Visible = true;
                        this.uxViewChainStatement.OnClientClick = string.Format(
                            "return ShowPopupModal('StatementDetails_Chain.aspx?{0}','auto')",
                            BuildSecureQueryString("Chain=" + chainNumber));
                    }
                }
            }
            //Get all of the chain statement
            else if (!GeneralFuncsLib.ExtendStatement.IsNullOrEmpty())
            {
                string filterModeForMS = GeneralFuncsLib.GetMsSiteWithStatementUseMode(filterValue);

                if (dicExtendStatement.ContainsKey(filterMode) && (dicExtendStatement[filterMode].Equals(filterValue.Trim())
               || string.IsNullOrEmpty(filterValue)))
                {
                    uxPlaceHolderStatementName.Visible = true;
                    uxExportDrillDownForAllStatements.GridTitle = VeraCodeSolution.ValidateResponseData(GetGridTitleResource(filterMode) + " - ");
                    uxExportDrillDownForAllStatements.GridSubTitle = VeraCodeSolution.ValidateResponseData(GridTitle);
                    uxExportDrillDownForAllStatements.GridHeader =
                        uxExportDrillDownForAllStatements.GridTitle + uxExportDrillDownForAllStatements.GridSubTitle;
                }
                else if (!string.IsNullOrEmpty(filterModeForMS) || (dicExtendStatement.ContainsKey(filterMode) && !string.IsNullOrEmpty(filterValue)))
                {
                    if (string.IsNullOrEmpty(filterValue))
                    {
                        filterMode = filterModeForMS;
                        filterValue = SessionManager.CurrentUser.EntityID;
                    }
                    uxTitleChainStatement.InnerText = string.Format("{0}",
                        VeraCodeSolution.ValidateResponseData(GridTitle));
                    uxTitleChainNoData.InnerText = string.Format("{0}",
                        VeraCodeSolution.ValidateResponseData(GridTitle));

                    DataTable listDataOfChainStatement;
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@HierarchyFilterMode", filterMode, DbType.String));
                    parameters.Add(new FilterParameter("@HierarchyFilterValue", filterValue, DbType.String));
                    listDataOfChainStatement = WebServices.CsReportServices.GetReports("spa_GetHierarchyStatement", parameters);
                    if (listDataOfChainStatement.Rows.Count == 0)
                    {
                        divNoChainStatement.Visible = true;
                    }
                    else
                    {
                        cbChainStatement.Items.Clear();
                        uxPlaceIDChainName.Visible = true;
                        for (int i = 0; i < listDataOfChainStatement.Rows.Count; i++)
                        {
                            DateTime reportTime = Convert.ToDateTime(listDataOfChainStatement.Rows[i]["ReportDate"]);
                            RadComboBoxItem radItem = new RadComboBoxItem();
                            radItem.Text = string.Format("{0} - {1} - {2}", reportTime.ToString("MM/yyyy"), listDataOfChainStatement.Rows[i]["Processor"], filterMode);
                            radItem.Value = string.Format("{0};{1}/{2};{3}",
                               listDataOfChainStatement.Rows[i]["DocID"], i, listDataOfChainStatement.Rows[i]["FileName"], listDataOfChainStatement.Rows[i]["ReportDate"]);
                            cbChainStatement.Items.Add(radItem);
                        }
                    }
                }

            }
        }

        protected string GetEntityIDOfUser(string userId)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
            parameters.Add(new FilterParameter("@UserName", userId, DbType.AnsiString));
            DataTable dt = WebServices.SecurityServices.GetReports("spa_SEC_GetUsers", parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0]["EntityID"].ToString() : string.Empty;
        }

        #region Override & Form Events

        protected override void DoSwitchView()
        {
            if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
            {
                uxReportFiltering.Visible = false;
                idTitle.HasFilteringOption = false;
            }
            if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode)
                && !string.IsNullOrEmpty(ReportFilter.CurrentValue.Value))
            {
                uxReportGridPanel.Visible = true;
                uxDrilldownGrid.Visible = false;
                uxHeaderDetail.Text = GridTitle;
                if (!IsPostBack)
                {
                    LoadUserControls(ReportFilter.CurrentValue.Value);
                }
                if (SessionManager.OpenStatementDetailPopup)
                {
                    SessionManager.OpenStatementDetailPopup = false;
                    if (uxReportDate.Items.Count > 0)
                    {
                        OnPostBackActions(PostBackAction.ViewStatement);
                    }
                }
            }
            else
            {
                uxReportGridPanel.Visible = false;
                uxDrilldownGrid.Visible = true;
                SessionManager.OpenStatementDetailPopup = false;
                if (GeneralFuncsLib.HasMSProductEnvironment())
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("Status").Visible = true;
                    uxDrilldownGrid.Columns.FindByUniqueName("LastStatementExport").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("MerchantStatus").Visible = false;
                }
                else
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("Status").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("LastStatementExport").Visible = false;
                    uxDrilldownGrid.Columns.FindByUniqueName("MerchantStatus").Visible = true;
                }
            }
        }

        protected override void DoReportFilterAction(ReportFilterEventArgs e)
        {
            base.DoReportFilterAction(e);
            switch (e.ActionType)
            {
                case ReportFilterEventType.Submit:
                case ReportFilterEventType.DrillDown:
                    if (GeneralFuncsLib.IsMerchantMode(e.HierachyValue.HierarchyMode)
                        && !string.IsNullOrEmpty(e.HierachyValue.Value))
                    {
                        LoadUserControls(e.HierachyValue.Value);
                    }
                    break;
            }
            SearchChainStatement(e.HierachyValue.HierarchyMode.Trim(), e.HierachyValue.Value.Trim());
        }

        protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender,
            Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (sender == uxDrilldownGrid)
            {
                OnDataBindControls(DataBindAction.BindDrilldownGrid, sender);
            }
            else if (sender == uxGridAllOfStatement)
            {
                OnDataBindControls(DataBindAction.ExtendMerchant);
            }

        }

        protected override void OnDataBindControls(Enum type, object sender)
        {
            switch ((DataBindAction)type)
            {
                case DataBindAction.BindDrilldownGrid:
                    {
                        ASGrid grid = (ASGrid)sender;
                        FilterParameterCollection parameters = new FilterParameterCollection();
                        parameters.AddLoggedInUserReportingParams();
                        parameters.AddHierarchyFilterParamsWithoutDate((ReportPage)this.Page);
                        parameters.AddLanguageID();
                        string spaName = "spa_GetMerchantList";
                        grid.DataSourceInvoker = new ASFuncInvoker(
                            WebServices.CsReportServices,
                            WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });

                        uxExportDrilldownGridTop.GridTitle = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("Statement_aspx_cs_MerchantList").ToString() + " - ");
                        uxExportDrilldownGridTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(GridTitle);
                        uxExportDrilldownGridTop.GridHeader =
                            uxExportDrilldownGridTop.GridTitle + uxExportDrilldownGridTop.GridSubTitle;
                    }
                    break;
                case DataBindAction.ExtendMerchant:
                    {
                        var filterMode = this.ReportFilter.CurrentValue.HierarchyMode;
                        FilterParameterCollection parameters = new FilterParameterCollection();
                        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        parameters.Add(new FilterParameter("@HierarchyFilterMode", filterMode, DbType.String));

                        uxGridAllOfStatement.DataSourceInvoker = new ASFuncInvoker(
                            WebServices.CsReportServices,
                            WebSiteConstants.GET_REPORT_METHOD_NAME,
                            new object[] { "spa_GetHierarchyStatementList", ReportServices.ConvertToFilterParamWSArray(parameters) });
                    }
                    break;
            }
        }

        protected override void OnPostBackActions(Enum type, object sender)
        {
            switch ((PostBackAction)type)
            {
                //42782 – VW – CAYAN - Implement New TSYS Processing Platform - modify
                case PostBackAction.ViewStatement:
                    {
                        string[] statementParameters = uxReportDate.SelectedValue.Split(
                            new string[] { ";" }, StringSplitOptions.None);
                        //parameters format: reportDate;1/2;fileName
                        // ex: 01/31/2013;2;   or 01/31/2013;1;0126042_mps_statement_01152013.pdf
                        //43842 - FD to TSYS Merchant Migration
                        string merchantNumber = ReportFilter.CurrentValue.Value;
                        if (EnableStatementApi(merchantNumber) && statementParameters[2] == BEProcessor.FD.ToString())
                        {
                            string realMerchant = GetRealMerchantNum(merchantNumber);
                            DateTime reportDate;
                            if (!DateTime.TryParse(statementParameters[0], out reportDate))
                            {
                                IsIntruderDetected = true;
                                return;
                            }
                            string statementId = statementParameters[1].ToString();
                            //43842 - FD to TSYS Merchant Migration
                            DownloadStatementApi(merchantNumber, realMerchant, reportDate, statementId);
                        }
                        else
                        {
                            DowloadStatement(statementParameters);
                        }
                        break;
                    }

                case PostBackAction.ViewChainStatement:
                    {
                        string[] statementDocID = cbChainStatement.SelectedValue.Split(
                            new string[] { ";" }, StringSplitOptions.None);
                        if (statementDocID.Length > 1)
                        {
                            int docID = Convert.ToInt32(statementDocID[0]);
                            string fileName = statementDocID[1].Substring(statementDocID[1].IndexOf("/") + 1);
                            if (!string.IsNullOrEmpty(SessionManager.CurrentUser.EntityID))
                            {
                                SavedReportFilterValue.Value = SessionManager.CurrentUser.EntityID;
                            }
                            ViewStatementByDocumentID(docID, fileName, "true", statementDocID[2], string.Empty, true);
                        }
                        break;
                    }
            }
        }

        private void DowloadStatement(string[] statementParameters)
        {
            if (statementParameters.Length == 3)
            {
                DateTime reportDate;
                if (!DateTime.TryParse(statementParameters[0], out reportDate))
                {
                    IsIntruderDetected = true;
                    return;
                }
                switch (statementParameters[1])
                {
                    case STATEMENT_VIEW_DATA:
                        ViewStatementByData(reportDate);
                        break;
                    case STATEMENT_VIEW_UPLOADED_FILE:
                        //sample filePath: 23\Statements\filename.pdf
                        string statementFilePath = SessionManager.CurrentUser.ASClient.ToString()
                            + @"\Statements\" + statementParameters[2];
                        ViewStatementByDocumentPath(statementFilePath, statementParameters[2]);
                        break;
                    default:
                        IsIntruderDetected = true;
                        return;
                }

                string activityText = string.Format(GetLocalResourceObject("Statement_aspx_cs_ViewStatement").ToString() + ": {0}",
                    reportDate.ToString(WebSiteConstants.DATE_FORMAT));
                GeneralFuncsLib.SaveUserActivity(ReportFilter.CurrentValue.Value, activityText);
            }
            else if (statementParameters.Length == 2 || statementParameters.Length == 4)
            {
                if (statementParameters[1] == STATEMENT_HISTORICAL)
                {
                    DateTime reportDate;
                    if (!DateTime.TryParse(statementParameters[0], out reportDate))
                    {
                        IsIntruderDetected = true;
                        return;
                    }
                    ViewStatementFISByData(reportDate);
                }
                else
                {
                    int docID = Convert.ToInt32(statementParameters[1]);
                    ViewStatementByDocumentID(docID, string.Format("{0}-{1}.pdf",
                                          SavedReportFilterValue.Value,
                                          uxReportDate.SelectedItem.Text), "true", statementParameters[0], ReportFilter.CurrentValue.Value);
                }
            }
            else
            {
                IsIntruderDetected = true;               
            }
        }

        protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender,
            AS.Controls.Exporter.ExportConfig exportConfig)
        {
            // 43842 - FD to TSYS Merchant Migration
            //42782 – VW – CAYAN - Implement New TSYS Processing Platform - modify
            if (sender.Grid == uxDrilldownGrid)
            {
                uxDrilldownGrid.Columns.FindByUniqueName("ViewStatement").Visible = false;
                uxDrilldownGrid.Columns.FindByUniqueName("LastStatementExport").Visible = true;
            }
            if (sender.Grid == uxGridAllOfStatement)
            {
                uxGridAllOfStatement.Columns.FindByUniqueName("LastStatement").Visible = false;
                string title = GetHeaderText();
                uxGridAllOfStatement.Columns.FindByUniqueName("EntityNumber").HeaderText = title;
            }

            base.DoNeedExportConfig(sender, exportConfig);
            exportConfig.FileName = GeneralFuncsLib.FormatFileName(sender.GridHeader.ToString());
        }

        //42782 – VW – CAYAN - Implement New TSYS Processing Platform - modify
        protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem && sender == uxDrilldownGrid)
            {
                LastStatement lastStatement = new LastStatement();
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView rowView = e.Item.DataItem as DataRowView;

                Literal ltViewStatement = dataItem.FindControl("ltViewStatement") as Literal;
                LinkButton lnkBt = dataItem.FindControl("lbtViewStatement") as LinkButton;
                string stmURL = this.GetStatementURL(rowView["BackEndProcessor"].ToString());
                var lastStatementFormatInfo = new LastStatementFormatInfoModel()
                {
                    StmURL = stmURL,
                    StatementPageUrl = ResolveUrl("~/Statement.aspx?"),
                    StmtResource = GetGlobalResourceObject("LanguageResource", "Statement_aspx_cs_STMT").ToString(),
                    ReportDateFormat = GeneralFuncsLib.GetStatementReportDateFormat(),
                };
                lastStatement.LastStatementFormat((SecurePage)this.Page, ltViewStatement, lnkBt, rowView.Row, lastStatementFormatInfo);
            }
        }

        protected void btnViewStatement_OnCommand(object sender, CommandEventArgs e)
        {
            string[] queryString = e.CommandArgument.ToString().Split(',');
            if (queryString.Length > 2)
            {
                string param = queryString[0];
                string merchantNumber = queryString[1];
                string page = queryString[2];
                if (CheckCupCakeMerchant(merchantNumber))
                {
                    page = "StatementDetails_TOTAL.aspx";
                }
                string script = "ShowPopupModal('" + page + "?" + param + "');";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ViewStatementTotal", script, true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            OnPostBackActions(PostBackAction.ViewStatement);
        }

        #endregion

        #region Show & Download Statements

        protected void ViewStatementByDocumentID(int docId, string fileName, string tracking = "", string reportDate = "", string merchantNumber = "", bool isHierarchy = false)
        {
            if (!string.IsNullOrEmpty(tracking))
            {
                GeneralFuncsLib.StatementTrackingLog(reportDate, merchantNumber, GeneralFuncsLib.FormatFileName(fileName), docId.ToString(), isHierarchy);
            }

            byte[] buffer = WebServices.DocServices.DownloadDoc(docId);
            this.TransferFileToClient(buffer, GeneralFuncsLib.FormatFileName(fileName));
        }

        private void ViewStatementByDocumentPath(string filePath, string fileName)
        {
            byte[] buffer = WebServices.DocServices.DownloadDocByFilePath(filePath);
            this.TransferFileToClient(buffer, fileName);
        }

        private void ViewStatementByData(DateTime reportDate)
        {           
            string urlstm = string.Empty;
            string processor = this.GetBEProcessor(ReportFilter.CurrentValue.Value, reportDate);
            if (!processor.IsNullOrEmpty())
            {
                urlstm = this.GetStatementURL(processor);
            }
            string merchantNumber = ReportFilter.CurrentValue.Value;
            if (CheckCupCakeMerchant(merchantNumber))
            {
                urlstm = "StatementDetails_TOTAL.aspx";
            }
            urlstm = string.IsNullOrEmpty(urlstm) ? "StatementDetail_MPS.aspx" : urlstm;

            string url = urlstm + "?" + BuildSecureQueryString(
                string.Format("ReportDate={0}&MerchantNumber={1}",
                              reportDate.Ticks.ToString(),
                              ReportFilter.CurrentValue.Value));
            ClientScript.RegisterStartupScript(
                this.GetType(),
                "ShowPopUp",
                string.Format(" setTimeout(\"ShowPopupModal('{0}');\", 300);", url),
                true);
        }

        private void ViewStatementFISByData(DateTime reportDate)
        {           
            string urlstm = string.Empty;
            string processor = this.GetBEProcessor(ReportFilter.CurrentValue.Value, reportDate);
            if (!processor.IsNullOrEmpty())
            {
                urlstm = this.GetStatementURL(processor);
            }
            urlstm = string.IsNullOrEmpty(urlstm) ? "StatementDetail_FIS.aspx" : urlstm;

            string url = urlstm + "?" + BuildSecureQueryString(
                string.Format("ReportDate={0}&MerchantNumber={1}",
                              reportDate.Ticks.ToString(),
                              ReportFilter.CurrentValue.Value));
            ClientScript.RegisterStartupScript(
                this.GetType(),
                "ShowPopUp",
                string.Format(" setTimeout(\"ShowPopupModal('{0}');\", 300);", url),
                true);
        }

        protected string GetStatementURL(string processor)
        {
            foreach (DataRow dr in SessionManager.Processors.Rows)
            {
                if ((int)dr["ASClientID"] == SessionManager.CurrentUser.ASClient
                    && (processor.IsNullOrEmpty()
                        || dr["BEProcessor"].ToString().Equals(processor, StringComparison.OrdinalIgnoreCase)))
                {
                    return dr["StatementURL"].ToString();
                }
            }
            return string.Empty;
        }

        private bool CheckCupCakeMerchant(string merchantNumber)
        {
            bool isExisted = false;
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add("@MerchantNumber", merchantNumber, DbType.String);
            parameters.Add("@IsExisted", isExisted, DbType.Boolean, true);

            FilterParameterCollection outparameters = new FilterParameterCollection();
            outparameters.Add("@IsExisted", isExisted, DbType.Boolean, true);
            string spaName = "spa_stmnt_CheckCupcakeMerchant_TOTAL";
            WebServices.CsReportServices.ExecuteNonQueryCommand(spaName, parameters, out outparameters);

            return bool.Parse(outparameters[0].ParameterValue.ToString());
        }
        #endregion

        #endregion Methods

        #region DownLoad All

        public string ConvertDateTime(string dateTime, string formatString = null)
        {
            DateTime dt;
            if (DateTime.TryParse(dateTime, out dt))
            {
                if (string.IsNullOrEmpty(formatString)) formatString = "MM/dd/yyyy";
                return dt.ToString(formatString);
            }

            return dateTime;
        }

        private void BindDataReportDate()
        {
            string spaName = "spa_Statement_GetListReportDate";
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paras.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
            paras.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
            DataTable dataSource = WebServices.CsReportServices.GetReports(spaName, paras);
            DataTable newdata = new DataTable();
            newdata.Columns.Add(new DataColumn("ID", typeof(string)));
            newdata.Columns.Add(new DataColumn("Name", typeof(string)));

            foreach (DataRow row in dataSource.Rows)
            {
                DataRow rowData = newdata.NewRow();
                rowData["Name"] = STATEMENT_DATE + ConvertDateTime(row["ReportDate"].ToString());
                rowData["ID"] = ConvertDateTime(row["ReportDate"].ToString(), "MM_dd_yyyy");
                newdata.Rows.Add(rowData);
            }
            cbbReportDate.DataSource = newdata;
            cbbReportDate.DataValueField = "ID";
            cbbReportDate.DataTextField = "Name";
            cbbReportDate.DataBind();

        }

        private DataTable GetReportList()
        {
            string spaName = "spa_Statement_GetListDownloadItem";
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paras.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
            paras.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
            paras.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
            return WebServices.CsReportServices.GetReports(spaName, paras);
        }

        private void UpdateStatement(string reportDate, string fileName)
        {           
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paras.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
            paras.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
            paras.Add(new FilterParameter("@Language", GeneralFuncsLib.GetCurrentCulture(), DbType.String));
            paras.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
            paras.Add(new FilterParameter("@ReportDate", reportDate, DbType.String));
            paras.Add(new FilterParameter("@FileName", fileName, DbType.String));
          
            uxGridStatement.Rebind();
        }

        private void DeleteStatement(long recordId)
        {
            string spaName = "spa_Statement_DeleteDownloadItem";
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paras.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
            paras.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
            paras.Add(new FilterParameter("@StatementID", recordId, DbType.Int64));
            FilterParameterCollection paramOuts;
            WebServices.CsReportServices.ExecuteNonQueryCommand(spaName, paras, out paramOuts);
        }

        protected void uxGridStatement_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (isEnhancement)
                BindDataSourceReportGrid();
        }

        private void BindDataSourceReportGrid()
        {
            string spaName = "spa_Statement_GetListDownloadItem";
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paras.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
            paras.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
            paras.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));

            uxGridStatement.DataSourceInvoker = new ASFuncInvoker(
                           WebServices.CsReportServices,
                           WebSiteConstants.GET_REPORT_METHOD_NAME,
                           new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(paras) });

            DataTable dt = WebServices.CsReportServices.GetReports(spaName, paras);
            uxGridStatement.DataSource = dt;
            if (dt.HasData())
                RegisterScriptRefresh(dt);
        }

        protected void btnDownLoad_Click(object sender, EventArgs e)
        {
            string reportDate = cbbReportDate.Text.Replace(STATEMENT_DATE, "");
            if (string.IsNullOrEmpty(reportDate)) return;

            string fileName = GetFileName();
            if (IsExistFile)
            {
                long recordID = GetRecordIdByFileName(fileName);
                if (recordID > 0)
                    DeleteStatement(recordID);
            }

            UpdateStatement(reportDate, fileName);
        }

        /// <summary>
        /// Register script to refresh grid if any item has status is not "Completed"
        /// </summary>
        /// <param name="dt">Datatable contains all statement</param>
        private void RegisterScriptRefresh(DataTable dt)
        {
            DataRow[] itemNotComplete = dt.Select("StatusID <> 1");
            if (itemNotComplete.Length > 0 && itemNotComplete.IsNotNullData())
            {
                if (IsPostBack)
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "refreshGrid", "onRefreshStatementGrid()", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "refreshGrid", "onRefreshStatementGrid()", true);
            }
        }

        protected void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            uxGridStatement.Rebind();
        }

        private string GetFileName()
        {
            return SessionManager.CurrentUser.UserID + "-STMT-" + cbbReportDate.GetValue();
        }

        private long GetRecordIdByFileName(string fileName)
        {
            long RecordId = 0;
#pragma warning disable S3267 // Loops should be simplified with "LINQ" expressions
            foreach (GridDataItem item in uxGridStatement.MasterTableView.Items)
#pragma warning restore S3267 // Loops should be simplified with "LINQ" expressions
            {
                if (item["FileName"].Text.Equals(fileName))
                {
                    long.TryParse(item["RecordID"].Text, out RecordId);
                }
            }         

            return RecordId;
        }

        protected void uxGridStatement_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView rowView = e.Item.DataItem as DataRowView;
                Literal lbtAction = dataItem.FindControl("lblAction") as Literal;
                Literal lblLink = dataItem.FindControl("lblDownLoad") as Literal;
                Button btnDelete = dataItem.FindControl("btnDelete") as Button;
                long docID = rowView["ServerDocID"].ToLong();
                string fileName = rowView["FileName"].ToString();
                string reportDate = rowView["ReportDate"].ToString();

                if (docID > 0)
                {
                    string textDelete = GetLocalResourceObject("textDelete").ToString();
                    string textTooltip = GetLocalResourceObject("textToolTip").ToString();

                    string secureString = BuildSecureQueryString(
                               string.Format("StmtId={0}&dl=1&fileName={1}&tracking=true&reportDate={2}", docID, fileName + ".pdf", reportDate));

                    string cellHTML = "<span style=\"display:inline-block; position:relative; \">"
                                + "<a class=\"link\" href=\"{0}\" style=\"cursor:pointer\">{1}</a>"
                                + "<span style=\"display:inline-block; width:40px; right:-33px; position:absolute;\"></span></span>";

                    lblLink.Text = VeraCodeSolution.GetOutputHtmlString(
                                string.Format(cellHTML, ResolveUrl("~/Statement.aspx?") + secureString, fileName));

                    dataItem["FileName1"].ToolTip = textTooltip;

                    //Delete Action               
                    string actionName = textDelete;
                    string actionHTML = "<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"deleteStatementReport('{0}','{1}'); return false;\">{2}</a>";

                    btnDelete.CommandArgument = "delete" + "," + rowView["RecordID"];
                    lbtAction.Text = VeraCodeSolution.GetOutputHtmlString(
                        string.Format(actionHTML, fileName, btnDelete.ClientID, actionName)
                        );
                }
                else
                {
                    lblLink.Text = fileName;
                    lbtAction.Visible = false;
                    btnDelete.Visible = false;
                }

                if (!rowView["Date"].ToString().IsNullOrEmpty())
                    dataItem["ReportDate"].Text = ConvertDateTime(rowView["Date"].ToString(), FmDateTime);
            }
        }

        protected void uxGridStatement_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            string[] queryString = e.CommandArgument.ToString().Split(',');
            if (queryString.Length > 0)
            {
                string action = queryString[0];
                if (action.Equals("delete"))
                {
                    long recordId = int.Parse(queryString[1]);
                    DeleteStatement(recordId);
                    BindDataSourceReportGrid();
                    int rows = uxGridStatement.MasterTableView.Items.Count;
                    int pageindex = uxGridStatement.CurrentPageIndex;

                    if (pageindex > 0 && rows - 1 <= 0)
                    {
                        uxGridStatement.CurrentPageIndex = pageindex - 1;
                    }
                    uxGridStatement.Rebind();
                }
            }
        }

        protected void btnCheckFileAvailable_OnCreateResponseData(ASCommandControl sender, string eventargument)
        {
            sender.ResponseString = CheckFileAvailable();
        }

        private string CheckFileAvailable()
        {
            string msgWarning = string.Empty;

            string spaName = "spa_Statement_CheckExistsDownloadItem";
            FilterParameterCollection paras = new FilterParameterCollection();
            paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            paras.Add(new FilterParameter("@EntityTypeID", SessionManager.CurrentUser.EntityType, DbType.Int32));
            paras.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.String));
            paras.Add(new FilterParameter("@FileName", GetFileName(), DbType.String));

            DataTable checkData = WebServices.CsReportServices.GetReports(spaName, paras);
            if (checkData.HasData())
            {
                IsExistFile = false;
                if (checkData.Rows[0]["FileExists"].ToBoolean())
                {
                    msgWarning = GetLocalResourceObject("textWarningDownLoad").ToString();
                    IsExistFile = true;
                }
            }

            return msgWarning;
        }

        #endregion

        public void viewStatementApi(string merchantNum)
        {
            LastStatement lastStatement = new LastStatement();
            string merchantNumber = Cryptophy.DecryptText(merchantNum);
            Dictionary<string, byte[]> data = lastStatement.ViewStatementApi(merchantNumber, CheckMerchantBelongToUser(merchantNumber), GetLocalResourceObject("Statement_aspx_cs_StatementApi_FileName").ToString());
            if (data != null && data.Count > 0)
            {
                string[] dataItem = data.ToArray()[0].Key.Split('|');
                string reportDate = dataItem[0];
                string fileName = dataItem[1];
                string docId = dataItem[2];

                GeneralFuncsLib.StatementTrackingLog(reportDate, merchantNumber, fileName, docId);

                lastStatement.TransferFileOrNot(true, this.Page, data.ToArray()[0].Value, fileName);
            }
            else
            {
                lastStatement.TransferFileOrNot(false, this.Page);
            }
        }

        private bool CheckMerchantBelongToUser(string merchantNumber)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams();
            parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));
            DataTable dt = WebServices.SecurityServices.GetReports(SPA_NAME_CHECK_BELONG_TO, parameters);
            if (dt != null && dt.Rows.Count > 0 && (bool)dt.Rows[0][IS_BELONG_TO])
                return true;
            return false;
        }

        //43842 - FD to TSYS Merchant Migration
        private void BindStatementApi(string merchantNumber)
        {
            bool result = false;
            if (CheckMerchantBelongToUser(merchantNumber))
            {
                //Get statement list with order
                var items = GetOmahaMerchantStatementList(merchantNumber);

                bool isLastStatement = CheckHasLastStatementTSYS(merchantNumber);
                if (isLastStatement)
                {
                    List<MerchantStatementConvert> lstStatement = new List<MerchantStatementConvert>();
                    if (items != null && items.Any())
                    {
                        foreach (var item in items)
                        {
                            lstStatement.Add(new MerchantStatementConvert
                            {
                                MerchantNumber = item.MerchantNumber,
                                StatementId = item.StatementId,
                                StatementDate = item.StatementDate,
                                BEProcessor = BEProcessor.FD.ToString(),
                            });
                        }

                    }

                    DataTable tbl = GetStatementReportDate(merchantNumber);
                    if (tbl != null && tbl.Rows.Count > 0)
                    {
                        foreach (DataRow row in tbl.Rows)
                        {
                            DateTime reportDate;
                            if (!DateTime.TryParse(row["ReportDate"].ToString(), out reportDate))
                            {
                                continue;
                            }

                            lstStatement.Add(new MerchantStatementConvert
                            {
                                MerchantNumber = merchantNumber,
                                StatementId = row["StatementID"].ToString(),
                                StatementDate = reportDate.ToShortDateString(),
                                BEProcessor = BEProcessor.TSYS.ToString(),
                            });
                        }
                    }

                    if (lstStatement.Any())
                    {
                        lstStatement = lstStatement.OrderByDescending(o => DateTime.Parse(o.StatementDate))
                                                    .ThenBy(o => o.BEProcessor).ToList();

                        uxReportDate.Items.Clear();
                        foreach (MerchantStatementConvert item in lstStatement)
                        {
                            SetItemReportDate(item.StatementDate, item.StatementId, item.BEProcessor);
                        }
                        result = true;
                    }
                }
                else
                {
                    if (items != null && items.Any())
                    {
                        uxReportDate.Items.Clear();
                        foreach (MerchantStatementItem item in items)
                        {
                            SetItemReportDate(item.StatementDate, item.StatementId, BEProcessor.FD.ToString());
                        }
                        result = true;
                    }
                }
            }

            pnlStatement.Visible = result;
            uxPlaceMss.Visible = !result;
        }

        //43842 - FD to TSYS Merchant Migration
        private void SetItemReportDate(string statementDate, string statementId, string beProcessor)
        {
            RadComboBoxItem radItem = new RadComboBoxItem();
            radItem.Text = string.Format(StatementDisplayFormat,
                    ParseDateTime(statementDate).ToString(ReportDateFormat),
                    " - " + beProcessor);

            radItem.Value = string.Format("{0};{1};{2};{3}",
                statementDate,
                statementId,
                beProcessor,
                string.Empty);
            uxReportDate.Items.Add(radItem);
        }


        /// <summary>
        /// Get statment list with order
        /// </summary>
        /// <param name="merchantNumber"></param>
        /// <returns></returns>
        private List<MerchantStatementItem> GetOmahaMerchantStatementList(string merchantNumber)
        {          
            //43842 - FD to TSYS Merchant Migration
            string realMerchant = GetRealMerchantNum(merchantNumber);

            StatementApi statementApi = new StatementApi();
            var items = statementApi.GetOmahaMerchantStatementList(realMerchant);

            if (items != null && items.Any())
            {
                return items.OrderByDescending(o => ParseDateTime(o.StatementDate)).ToList();
            }

            return new List<MerchantStatementItem>();
        }

        private DateTime ParseDateTime(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr)) return DateTime.MinValue;

            DateTime date;
            DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return date;
        }

        private void DownloadStatementApi(string currMerchantNumber, string realMerchantNumber, DateTime reportDate, string statementId)
        {
            StatementApi statementApi = new StatementApi();
            MerchantStatement statement = statementApi.GetOmahaMerchantStatement(realMerchantNumber, statementId);
            byte[] bytes = Convert.FromBase64String(statement.StatementData);
            string fileName = string.Format(GetLocalResourceObject("Statement_aspx_cs_StatementApi_FileName").ToString(), currMerchantNumber, reportDate.Month.ToString("0#"), reportDate.Year.ToString());

            GeneralFuncsLib.StatementTrackingLog(reportDate.ToString(), currMerchantNumber, fileName, statementId);

            this.TransferFileToClient(bytes, fileName);
        }

        //43842 - FD to TSYS Merchant Migration
        private string GetRealMerchantNum(string currMerchant)
        {
            DataTable merchant = GeneralFuncsLib.CheckMerchantIsConvertion(currMerchant);
            if (merchant.HasData())
            {
                string realMerchant = merchant.Rows[0]["FDR_MID"].ToString();
                return string.IsNullOrEmpty(realMerchant) ? currMerchant : realMerchant;
            }

            return currMerchant;
        }

        //43842 - FD to TSYS Merchant Migration
        private bool CheckHasLastStatementTSYS(string currMerchant)
        {
            DataTable merchant = GeneralFuncsLib.CheckMerchantIsConvertion(currMerchant);
            if (merchant.HasData())
            {
                return !string.IsNullOrEmpty(merchant.Rows[0]["LastStatementReportDate"].ToString()) &&
                    !string.IsNullOrEmpty(merchant.Rows[0]["STMT_DocIDList"].ToString());
            }

            return false;
        }

        protected void uxGridAllOfStatement_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {

                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView rowView = e.Item.DataItem as DataRowView;
                string chainID = rowView["EntityNumber"].ToString();
                Literal lblViewIdChain = dataItem.FindControl("lblViewIdChain") as Literal;
                Literal lblLastStatement = dataItem.FindControl("lblLastStatement") as Literal;
                string tplClick = "<a href=\"#\" onclick=\"return rf_DrilldownReportFilterValues('{0}', '{1}', '{2}', '{4}', '{5}', '{6}');\">{3}</a>";

                //Link to          
                string actionHTML = string.Format(tplClick, ReportFilter.CurrentValue.ID, ReportFilter.CurrentValue.HierarchyMode, chainID, chainID
                                                , ReportFilter.CurrentValue.ID, ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
                lblViewIdChain.Text = VeraCodeSolution.GetOutputHtmlString(
                            string.Format(actionHTML, chainID, chainID)
                            );

                string actionHTMLLastStatement = string.Empty;
                string statementIDs = rowView["DocID"].ToString();              
                string fileName = rowView["FileName"].ToString();
                string secureString = BuildSecureQueryString(
                    string.Format("StmtId={0}&dl=1&fileName={1}", statementIDs, fileName));

                actionHTMLLastStatement = "<a class=\"link\" href=\"{0}\" style=\"cursor:pointer\">{1}</a>";
                lblLastStatement.Text = VeraCodeSolution.GetOutputHtmlString(
                string.Format(actionHTMLLastStatement
                    , ResolveUrl("~/Statement.aspx?") + secureString,
                   GetLocalResourceObject("ASGridBoundColumnResourceViewStatement")));
            }
        }

        protected void btnViewChain_Click(object sender, EventArgs e)
        {
            OnPostBackActions(PostBackAction.ViewChainStatement);
        }

        protected void uxGridAllOfStatement_DataSourceReady(object sender, EventArgs e)
        {
            SetHeaderTextLastStatement();
        }

    }
}
