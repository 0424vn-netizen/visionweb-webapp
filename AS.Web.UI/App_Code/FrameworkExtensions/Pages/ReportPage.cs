using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Security.WS.Entities;
using AS.Web.UI.Controls;
using Telerik.Web.UI;
using System.Threading;
using System.Globalization;
using System.Linq;
using AS.Web.SharedSession;
using AS.VW.Entities;
using AS.Web.UI.AppCode.General;

public class ReportPage : ReportBasePage
{
    private const string SELECTABLE = "Selectable";

    protected CustomViewMessageResourceType GetCustomViewMessageResourceTypeMode
    {
        get
        {
            if (SecureQueryString["CustomViewMessageResourceTypeMode"] != null)
            {
                return (CustomViewMessageResourceType)Enum.Parse(typeof(CustomViewMessageResourceType), SecureQueryString["CustomViewMessageResourceTypeMode"]);
            }
            else
            {
                return CustomViewMessageResourceType.Assignments;
            }
        }
    }

    public bool IsModal
    {
        get { return PageType == SecurePageType.Modal; }
    }

    public ReportPage()
    {
        this.ASPXTrackingLog = new AspxTracking();
        this.IntruderLog = new Intruders();
        this.DefaultTheme = "Default";
        this.ValidPagesForForceResetPassword = ",manageprofile.aspx,";
        this.ResetPasswordPage = "~/ManageProfile.aspx";
    }
    public void AjaxAddResponseScript(string script)
    {
        ((BaseMasterPage)Master).AjaxAddResponseScript(script);
    }
    #region Page Event Handlers
    protected override void PageInitialize()
    {
        if (GeneralFuncsLib.IsIFrameSupported())
        {
            PageType = SecurePageType.None;
        }
        this.IsSecureCSRF = false;
        this.ReportFilterID = "uxReportFilter";
        this.GridIDs.Add("uxReportGrid");
        this.GridIDs.Add("uxDrilldownGrid");

        this.ExporterIDs.Add("uxExporter");

        base.PageInitialize();
        if (ReportFilter != null)
        {
            SettingFilteringOptions();
        }
        RadGrid uxDrilldownGrid = (RadGrid)ExtFindControl(this, "uxDrilldownGrid");
        if (uxDrilldownGrid != null) uxDrilldownGrid.ItemDataBound += new GridItemEventHandler(uxDrilldownGrid_ItemDataBound);
    }
    protected override void InitializeCulture()
    {
        string selectedLanguage = string.Empty;
        if (SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.English)
        {
            selectedLanguage = WebSiteConstants.USCulture;
        }
        else if (SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.Spanish)
        {
            selectedLanguage = WebSiteConstants.SpanishCulture;
        }
        else
        {
            // default is English
            selectedLanguage = WebSiteConstants.USCulture;
        }

        Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);
        base.InitializeCulture();
    }
    void uxDrilldownGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case GridItemType.Item:
            case GridItemType.AlternatingItem:
                {
                    GridDataItem rowItem = e.Item as GridDataItem;
                    DataRowView dataItem = e.Item.DataItem as DataRowView;
                    if (rowItem["DrilldownColumn"] != null)
                    {
                        rowItem["DrilldownColumn"].Text = BuildDrilldownLink(rowItem["DrilldownColumn"].Text);
                    }

                }
                break;
        }
    }
    string BuildDrilldownLink(string text)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();

        //get all hierarchy filter
        if (SessionManager.HierarchyFilterDrillDown == null)
        {
            parameters.AddLoggedInUserReportingParams();
            parameters.AddLanguageID();
            SessionManager.HierarchyFilterDrillDown = WebServices.RiskServices.GetReports("spa_GetHierarchyFilterLevel", parameters);
        }

        DataTable HierarchyFilterLevel = SessionManager.HierarchyFilterDrillDown;
        DataRow currentHierarchyRow = HierarchyFilterLevel.FindObject("CurrentHierarchyMode", ReportFilter.CurrentValue.HierarchyMode);
        int len = HierarchyFilterLevel.Rows.Count;

        const string tpl = "<a href=\"#\" onclick=\"return rf_DrilldownReportFilterValues('{0}', '{1}', '{2}', '{4}', '{5}', '{6}');\">{3}</a>";
        string nextID = string.Empty;
        string nextMode = string.Empty;
        string value = string.Empty;
        value = text;
        string pageRequest = Request.CurrentExecutionFilePath.Substring(Request.CurrentExecutionFilePath.LastIndexOf('/') + 1).ToLower();

        if (pageRequest == "merchantprofile.aspx" || pageRequest == "statement.aspx" || pageRequest == "merchantinformation.aspx")
        {
            HierarchyDetail merchantHierarchy = GeneralFuncsLib.GetMerchantHierarchyInfo();
            nextID = merchantHierarchy.HierarchyID;
            nextMode = merchantHierarchy.HierarchyMode;
        }
        else if (ReportFilter.CurrentValue.Value == "" || ReportFilter.CurrentValue.Value == SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", currentHierarchyRow["CurrentHierarchyMode"].ToString())["HierarchyPrefix"].ToString())
        {
            //get the current hierarchy mode

            nextID = currentHierarchyRow["CurrentHierarchyID"].ToString();
            nextMode = currentHierarchyRow["CurrentHierarchyMode"].ToString();
            currentHierarchyRow = SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", nextMode);
            if (currentHierarchyRow["ValidInput"] is DBNull)
            {
            }
            else
            {
                if ((bool)currentHierarchyRow["ValidInput"])
                {
                    value = Regex.Replace(value, "[^" + toHexExpressionString(currentHierarchyRow["ValidateExpression"].ToString()) + "]", "");
                }
                else
                {
                    value = Regex.Replace(value, "[" + toHexExpressionString(currentHierarchyRow["ValidateExpression"].ToString()) + "]", "");
                }
            }
        }
        else
        {
            nextID = currentHierarchyRow["NextHierarchyID"].ToString();
            nextMode = currentHierarchyRow["NextHierarchyMode"].ToString();
            currentHierarchyRow = SessionManager.AllHierarchyFilter.FindObject("HierarchyMode", nextMode);
            if (currentHierarchyRow["ValidInput"] is DBNull)
            {
            }
            else
            {
                if ((bool)currentHierarchyRow["ValidInput"])
                {
                    value = Regex.Replace(value, "[^" + toHexExpressionString(currentHierarchyRow["ValidateExpression"].ToString()) + "]", "");
                }
                else
                {
                    value = Regex.Replace(value, "[" + toHexExpressionString(currentHierarchyRow["ValidateExpression"].ToString()) + "]", "");
                }
            }
        }

        var currentValueFilter = ReportFilter.CurrentValue.Value;
        if (!string.IsNullOrEmpty(currentValueFilter))
        {
            currentValueFilter = currentValueFilter.Replace("'", "\\'");
        }

        return string.Format(tpl, nextID, nextMode, value, text, ReportFilter.CurrentValue.ID, ReportFilter.CurrentValue.HierarchyMode, currentValueFilter);
    }
    string toHexExpressionString(string exp)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in exp)
        {
            sb.Append("\\x" + ((int)c).ToString("X"));

        }

        return sb.ToString();
    }

    protected virtual void FilteringOptionDateSwitchView()
    {

    }
    protected override void OnLoad(EventArgs e)
    {
        if (aperia.controls.KendoChart.IsDataRequest) return;
        this.Form.SetCssClass();
        SharedProvider.UpdateLastActive();
        base.OnLoad(e);

    }



    protected override void OnUnload(EventArgs e)
    {
        base.OnUnload(e);
        if (ReportFilter != null && ReportFilter.CurrentValue != null) SavedReportFilterValue = ReportFilter.CurrentValue;
    }

    public override void ProcessRequest(HttpContext context)
    {
        AS.Web.SharedSession.SharedProvider.RedirectLoginUrlWhenTimeout();

        base.ProcessRequest(context);
    }

    #endregion
    void ResetDateValue()
    {
        ReportFilter.CurrentValue.DateOptionValue.From = DateTime.Now;
        ReportFilter.CurrentValue.DateOptionValue.To = DateTime.Now;
        if (ReportFilter.DateOptionDateRangeVisible)
        {
            if (GeneralFuncsLib.HasExtendedSetting("FILTERING_OPTIONS_DATERANGE"))
            {
                ReportFilter.CurrentValue.DateOptionValue.From = DateTime.Now.AddDays(-90);
                ReportFilter.CurrentValue.DateOptionValue.To = DateTime.Now;
                ReportFilter.CurrentValue.DateOption = DateOptionMode.DateRange;
            }
            else
            {
                ReportFilter.CurrentValue.DateOptionValue.From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                ReportFilter.CurrentValue.DateOptionValue.To = DateTime.Now;
                ReportFilter.CurrentValue.DateOption = DateOptionMode.DateRange;
            }
        }
        else if (ReportFilter.DateOptionDailyVisible)
        {
            ReportFilter.CurrentValue.DateOption = DateOptionMode.Daily;
        }
        else if (ReportFilter.DateOptionMonthlyVisible)
        {
            ReportFilter.CurrentValue.DateOption = DateOptionMode.Monthly;

        }
    }

    public void ShowServerErrorMessage(AS.Controls.Global.ValidatorMessage val, String customMessage = null)
    {
        if (customMessage != null)
        {
            val.Message = AS.Common.VeraCodeSolution.DoVeraCode(customMessage);
        }
        val.ShowOnLoad = true;
    }

    void SettingFilteringOptions()
    {
        SavedReportFilterSyncManager.SyncMerchantNumberToMPSReportFilter();

        FilteringOptionDateSwitchView();

        DataTable HierarchyFilter = SessionManager.HierarchyFilter;


        if (!string.IsNullOrEmpty(SessionManager.ShareMerchantNumber) )
        {
            if(SavedReportFilterValue != null)
            {
                SavedReportFilterValue.HierarchyMode = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;
                SavedReportFilterValue.Value = SessionManager.ShareMerchantNumber;
            }
            else
            {
                SavedReportFilterValue = new AS.Web.UI.Controls.HierarchyFilterValue();
                SavedReportFilterValue.Value = SessionManager.ShareMerchantNumber;
                SavedReportFilterValue.ID = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;
                SavedReportFilterValue.HierarchyMode = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;
                SavedReportFilterValue.DateOption = AS.Web.UI.Controls.DateOptionMode.DateRange;
                SavedReportFilterValue.DateOptionValue.From = SavedReportFilterValue.DateOptionValue.To = DateTime.Now;

            }
        }

        if (SavedReportFilterValue != null)
        {
            ReportFilter.CurrentValue = SavedReportFilterValue;
            if (
                (!ReportFilter.DateOptionDailyVisible && ReportFilter.CurrentValue.DateOption == DateOptionMode.Daily)
                ||
                (!ReportFilter.DateOptionMonthlyVisible && ReportFilter.CurrentValue.DateOption == DateOptionMode.Monthly)
                ||
                (!ReportFilter.DateOptionDateRangeVisible && ReportFilter.CurrentValue.DateOption == DateOptionMode.DateRange)
                )
            {
                ResetDateValue();
            }
            if (ReportFilter is AS.Controls.Global.MPSReportFilter)
            {

                string pageRequest = Request.CurrentExecutionFilePath.Substring(Request.CurrentExecutionFilePath.LastIndexOf('/') + 1).ToLower();
                if ((pageRequest != "merchantprofile.aspx" && pageRequest != "statement.aspx" && pageRequest != "merchantinformation.aspx")
                    || string.IsNullOrEmpty(ReportFilter.CurrentValue.ID)
                    )
                {
                    if (!HierarchyFilter.CheckExistObject("HierarchyID", ReportFilter.CurrentValue.ID)
                        && SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.Merchant)
                    {
                        foreach (DataRow row in HierarchyFilter.Rows)
                        {
                            if (!(row[SELECTABLE] is DBNull) && !row[SELECTABLE].ToBoolean())
                                continue;
                            ReportFilter.CurrentValue.ID = row["HierarchyID"].ToString();
                            if (!(row["ShowPrefix"] is DBNull) && ((bool)row["ShowPrefix"]))
                            {
                                ReportFilter.CurrentValue.Value = row["HierarchyPrefix"].ToString();
                            }
                            else
                            {
                                ReportFilter.CurrentValue.Value = "";
                            }
                            ReportFilter.CurrentValue.HierarchyMode = row["HierarchyMode"].ToString();
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ReportFilter.CurrentValue.ID))
                {
                    DataRow currentRow = HierarchyFilter.AsEnumerable().SingleOrDefault(r => r.Field<int>("HierarchyID") == ReportFilter.CurrentValue.ID.ToInt());
                    if (currentRow != null && GeneralFuncsLib.ToBoolean(currentRow["IsExtend"]))
                    {
                        AS.Controls.Global.MPSReportFilter mpsReportFilter = ReportFilter as AS.Controls.Global.MPSReportFilter;
                        if (mpsReportFilter != null && !mpsReportFilter.ExtendHierarchyMode.Split(',').Contains(ReportFilter.CurrentValue.HierarchyMode))
                        {
                            foreach (DataRow row in HierarchyFilter.Rows)
                            {
                                if (!(row[SELECTABLE] is DBNull) && !row[SELECTABLE].ToBoolean())
                                    continue;
                                ReportFilter.CurrentValue.ID = row["HierarchyID"].ToString();
                                ReportFilter.CurrentValue.Value = row["HierarchyPrefix"].ToString();
                                ReportFilter.CurrentValue.HierarchyMode = row["HierarchyMode"].ToString();
                                break;
                            }
                        }
                    }
                }
            }

        }
        else if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
        {

            HierarchyDetail info = GeneralFuncsLib.GetMerchantHierarchyInfo();
            ReportFilter.CurrentValue.ID = info.HierarchyID;
            ReportFilter.CurrentValue.Value = SessionManager.CurrentUser.EntityID;
            ReportFilter.CurrentValue.HierarchyMode = info.HierarchyMode;
            ResetDateValue();
            SavedReportFilterValue = ReportFilter.CurrentValue;
        }
        else
        {
            string hierarchyMode = GeneralFuncsLib.GetDataOfExtendedSetting("DEFAULT_HIERARCHY_FILTER_MODE");
            bool isDefaultFilter = false;
            if (!string.IsNullOrEmpty(hierarchyMode))
            {
                var hierarchyDefault = HierarchyFilter.Select("HierarchyMode = '" + hierarchyMode + "'").FirstOrDefault();
                if (hierarchyDefault != null)
                {
                    ReportFilter.CurrentValue.ID = hierarchyDefault["HierarchyID"].ToString();
                    if (!(hierarchyDefault["ShowPrefix"] is DBNull) && ((bool)hierarchyDefault["ShowPrefix"]))
                    {
                        ReportFilter.CurrentValue.Value = hierarchyDefault["HierarchyPrefix"].ToString();
                    }
                    else
                    {
                        ReportFilter.CurrentValue.Value = string.Empty;
                    }
                    ReportFilter.CurrentValue.HierarchyMode = hierarchyMode;
                    isDefaultFilter = true;
                }
            }
            if (!isDefaultFilter)
            {
                foreach (DataRow row in HierarchyFilter.Rows)
                {
                    if (!(row[SELECTABLE] is DBNull) && !row[SELECTABLE].ToBoolean())
                        continue;
                    ReportFilter.CurrentValue.ID = row["HierarchyID"].ToString();
                    if (!(row["ShowPrefix"] is DBNull) && ((bool)row["ShowPrefix"]))
                    {
                        ReportFilter.CurrentValue.Value = row["HierarchyPrefix"].ToString();
                    }
                    else
                    {
                        ReportFilter.CurrentValue.Value = string.Empty;
                    }
                    ReportFilter.CurrentValue.HierarchyMode = row["HierarchyMode"].ToString();
                    break;
                }
            }
            ResetDateValue();
            SavedReportFilterValue = ReportFilter.CurrentValue;
        }
    }

    protected HierarchyFilterValue SavedReportFilterValue
    {
        get
        {
            return SessionManager.CurrentReportFilter;
        }
        set
        {
            SessionManager.CurrentReportFilter = value;
        }
    }
    /// <summary>
    /// Return true if FilterMode=MerchantNumber & MerchantNumber!=null
    /// </summary>


    #region Exporting Event Handlers
    protected override void DoExportingGridHeader(UxExport sender, LineArgs e)
    {
    }

    protected override void DoExportingReportHeader(UxExport sender, LineArgs e)
    {
    }

    protected override void DoExportingTotalLine(UxExport sender, LineArgs e)
    {
    }

    protected override void DoInitializeExport(UxExport sender, ExportEventArgs e)
    {
    }

    protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
    {
        //exportConfig.AllowHtmlEncoded = true;
        exportConfig.PageDirection = PageDirection.Landscape;
        exportConfig.FileName = sender.GridHeader;//Utilities.FormatFileName(sender.GridHeader);
        exportConfig.ReportHeader = sender.GridHeader;
    }
    #endregion

    #region ReportFilter11 Event Handlers

    protected override void DoReportFilterAction(ReportFilterEventArgs e)
    {
        SavedReportFilterValue = e.HierachyValue;
        switch (e.ActionType)
        {
            case ReportFilterEventType.DrillDown:
                {
                    SessionManager.FromHierarchyDrilldown = e.DrillDownFrom;
                    DoSearchReport(e);
                    break;
                }
            case ReportFilterEventType.Submit:
                {
                    SessionManager.FromHierarchyDrilldown = null;
                    if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
                    {
                        SessionManager.ShareMerchantNumber = ReportFilter.CurrentValue.Value;
                    }
                    else
                    {
                        SessionManager.ShareMerchantNumber = string.Empty;
                    }
                    DoSearchReport(e);
                    break;
                }

        }

    }


    #endregion

    #region Grid Event Handlers

    protected override void DoGridDataSourceReady(ASGrid sender, EventArgs e)
    {

    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
    }


    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {

    }

    #endregion

    #region Page Methods


    private void DoSearchReport(ReportFilterEventArgs e)
    {
        for (int i = 0; i < ReportGrids.Length; i++)
        {
            if (ReportGrids[i] != null)
            {
                ReportGrids[i].CurrentPageIndex = 0;
                ReportGrids[i].AS_SortExpression = "";
            }
        }
    }

    protected override void DoSwitchView()
    {
    }
    #endregion

    #region Secured Functions
    public override AS.Common.WebUI.ICryptor Cryptor
    {
        get { return CryptorServices.Current; }
    }

    protected override void DoIntruderDetected(IntruderType type)
    {
        switch (type)
        {
            case IntruderType.Permission:
                string rootURL = this.ResolveUrl("~");
                Response.Redirect(rootURL + "403.aspx");
                break;
            default:
                Session.Clear();
                Session.Abandon();
                HttpContext.Current.Response.Cookies.Clear();

                FormsAuthentication.SignOut();
                if (this.LoginUrl == "[jumpsite]")
                {
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("logout_opt", "3"));
                    HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
                }
                else
                {
                    if (string.IsNullOrEmpty(this.LoginUrl))
                    {
                        HttpContext.Current.Response.Cookies.Add(new HttpCookie("logout_opt", "3"));
                        HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect(this.LoginUrl);
                    }
                }
                break;
        }
    }

    protected override void OnAspxWriteLog()
    {
        if (User.Identity.IsAuthenticated && SessionManager.IsLoggedIn)
        {
            ASPXTrackingLog.LogSystemId = SessionManager.CurrentSystem;
            ASPXTrackingLog.LogClientId = SessionManager.CurrentUser.ASClient;
            ASPXTrackingLog.LogFullName = SessionManager.CurrentUser.UserNameFull;
            ASPXTrackingLog.LogId1 = SessionManager.CurrentUser.UserID;


        }
        else
        {
            ASPXTrackingLog.LogSystemId = WebSiteSettings.DefaultSystem;
            ASPXTrackingLog.LogClientId = WebSiteSettings.DefaultClient;
        }
        int SuspectedBotCode = WebServices.LogServices.InsertASPXTrackingLog(ASPXTrackingLog);

        if (SuspectedBotCode == 1)
        {
            Session.Abandon();
            Session.Clear();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage(LoginUrl);
        }
    }

    protected override void OnIntruderWriteLog()
    {
        if (User.Identity.IsAuthenticated && SessionManager.IsLoggedIn)
        {
            ASPXTrackingLog.LogSystemId = SessionManager.CurrentSystem;
            ASPXTrackingLog.LogClientId = SessionManager.CurrentClient;
            ASPXTrackingLog.LogFullName = SessionManager.CurrentUser.UserNameFull;
            ASPXTrackingLog.LogId1 = SessionManager.CurrentUser.UserID;
        }
        else
        {
            ASPXTrackingLog.LogSystemId = WebSiteSettings.DefaultSystem;
            ASPXTrackingLog.LogClientId = WebSiteSettings.DefaultClient;
        }
        WebServices.SecurityServices.InsertIntruderLog(IntruderLog);
    }

    #endregion

    #region Common functions

    protected virtual void OnDataBindControls(Enum type, object sender) { }
    protected void OnDataBindControls(Enum type) { OnDataBindControls(type, null); }
    protected virtual void OnPostBackActions(Enum type, object sender) { }
    protected void OnPostBackActions(Enum type) { OnPostBackActions(type, null); }
    protected virtual bool OnValidateInputs(Enum type, object sender) { return true; }
    protected bool OnValidateInputs(Enum type) { return OnValidateInputs(type, null); }

    #endregion

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        GeneralFuncsLib.RegisterHtmlMeta(this);
    }
    protected override void OnPreRenderComplete(EventArgs e)
    {
        base.OnPreRenderComplete(e);
        bool isModal = (PageType == SecurePageType.Modal) ? true : false;
        GeneralFuncsLib.RegisterCssFile(this, isModal);
    }

    protected FilterParameterCollection GetLoggedInUserParams()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        return parameters;
    }

}

