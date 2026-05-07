using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;

[PagePermission("SendMsg,MSSendMsg,MSViewMsg")]
public partial class Message_Management : ReportPage
{

    #region Enums
    enum DataBindAction
    {
        BindSentMessageGrid,
    }
    enum PostBackAction
    {
        CreateMessage,
        ViewMessage,
        SearchMessage,
        RebindGrid,
    }
    #endregion

    #region properties
    private const string MESSAGE_ID = "MessageID";

    string _AssignmentIntruderQuery = string.Empty;
    private string AssignmentIntruderQuery
    {
        get
        {
            if (_AssignmentIntruderQuery.Length == 0)
                _AssignmentIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { MESSAGE_ID });
            return _AssignmentIntruderQuery;
        }
    }

    private HierarchyFilterValue _reportValue = null;
    #endregion
    

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxReportGrid");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsIntruderDetected) return;
        
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
        {
            uxPageTitle.ReportTitle = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("Message_Management_aspx_cs_CustomerMessage").ToString());
        }
        else
        {
            uxPageTitle.ReportTitle = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("Message_Management_aspx_cs_Message").ToString());
        }
        modal = modal.Replace("[_Message_Management_aspx_cs_ViewList_]", GetLocalResourceObject("Message_Management_aspx_cs_ViewList").ToString());
        uxMessagePlaceHolder.Visible = true;
        uxLnkCreateMessage.Visible = true;
        IsBindDataOnLoad = true;
        uxReportGrid.AS_SortExpression = string.Empty;
        if (!IsPostBack)
        {
            GetDateFilter();
            SetDateFilter();
        }
    }
    // Get Date from Session if not null and assign to control
    private void GetDateFilter()
    {
        this._reportValue = SavedReportFilterValue;
        if (this._reportValue == null)
        {
            this._reportValue = new HierarchyFilterValue();
            this._reportValue.DateOption = DateOptionMode.DateRange;
            this._reportValue.DateOptionValue.To = DateTime.Now;
            if (GeneralFuncsLib.HasExtendedSetting("FILTERING_OPTIONS_DATERANGE"))
            {
                this._reportValue.DateOptionValue.From = DateTime.Now.AddDays(-90);
            }
            else
            {
                this._reportValue.DateOptionValue.From = DateTime.Now.GetFirstDayOfMonth();
            }
        }

        switch (_reportValue.DateOption)
        {
            case DateOptionMode.Daily:
                this.uxDaily.Checked = true;
                this.uxDate.SelectedDate = this._reportValue.DateOptionValue.From;
                break;
            case DateOptionMode.Monthly:
                this.uxMonthly.Checked = true;
                this.uxDate.SelectedDate = this._reportValue.DateOptionValue.From;
                break;
            case DateOptionMode.DateRange:
                this.uxRange.Checked = true;
                this.uxFromDate.SelectedDate = this._reportValue.DateOptionValue.From;
                this.uxEndDate.SelectedDate = this._reportValue.DateOptionValue.To;
                break;
        }
    }

    // Set Date to Session
    private void SetDateFilter()
    {
        //Get report filter form session
        if (SavedReportFilterValue != null)
            this._reportValue = SavedReportFilterValue;
        else
            this._reportValue = new HierarchyFilterValue();
        // Set Date Option         
        if (uxDaily.Checked)
        {
            this._reportValue.DateOption = DateOptionMode.Daily;
            this._reportValue.DateOptionValue.From = this._reportValue.DateOptionValue.To = uxDate.SelectedDate.Value;
        }
        else if (uxMonthly.Checked)
        {
            this._reportValue.DateOption = DateOptionMode.Monthly;
            this._reportValue.DateOptionValue.From = uxDate.SelectedDate.Value;
        }
        else
        {
            this._reportValue.DateOption = DateOptionMode.DateRange;
            this._reportValue.DateOptionValue.From = uxFromDate.SelectedDate.Value;
            this._reportValue.DateOptionValue.To = uxEndDate.SelectedDate.Value;
        }

        // Set report filter
        if (SavedReportFilterValue != null)
            SavedReportFilterValue = this._reportValue;
        uxDate.MaxDate = uxEndDate.MaxDate = uxFromDate.MaxDate = DateTime.Now;
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        uxReportGrid.Columns.FindByUniqueName("Recipients").Visible = false;
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(uxExporter.GridHeader);
        exportConfig.ReportHeader = uxExporter.GridHeader;
    }

    protected void uxSearch_Click(object sender, EventArgs e)
    {
        SetDateFilter();
        OnPostBackActions(PostBackAction.SearchMessage);
    }

    protected override void DoGridNeedDataSource(ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindSentMessageGrid, sender);
    }
    string modal = "<a href='#' onclick=\"ShowPopupModal('{0}', 'max');return false;\" >[_Message_Management_aspx_cs_ViewList_]</a>";
    protected override void DoItemDataBound(ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        uxExporter.GridTitle = uxExporter.GridHeader;
    }

    string _ViewMessageIntruderQuery = string.Empty;
    private string ViewMessageIntruderQuery
    {
        get
        {
            if (_ViewMessageIntruderQuery == string.Empty)
                _ViewMessageIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportGrid.ID, new string[] { MESSAGE_ID });
            return _ViewMessageIntruderQuery;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindSentMessageGrid:
                SetDateFilter();
                FilterParameterCollection _parames = new FilterParameterCollection();
                _parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                _parames.Add(new FilterParameter("@DateFilterMode", (int)this._reportValue.DateOption, DbType.Int32));
                _parames.Add(new FilterParameter("@BeginDate", this._reportValue.DateOptionValue.From, DbType.Date));
                DateTime endDate = this._reportValue.DateOption == DateOptionMode.DateRange ? this._reportValue.DateOptionValue.To : this._reportValue.DateOptionValue.From;
                _parames.Add(new FilterParameter("@EndDate", endDate, DbType.Date));
                ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_ms_GetSentMessages", ReportServices.ConvertToFilterParamWSArray(_parames) });

                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (this.IsIntruderDetected) return;
        int msgID = 0;
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection outparameters = new FilterParameterCollection();
        switch ((PostBackAction)type)
        {
            case PostBackAction.CreateMessage:
                msgID = Save();
                string queryString = BuildSecureQueryString(string.Format("{0}={1}{2}", MESSAGE_ID, msgID, AssignmentIntruderQuery));
                AjaxAddResponseScript("ShowPopupModal('Message_CreateNew.aspx?" + queryString + "','auto'); return false;");
                break;
            case PostBackAction.SearchMessage:
                uxReportGrid.Rebind();
                break;
            case PostBackAction.RebindGrid:
                uxReportGrid.Rebind();
                break;
            case PostBackAction.ViewMessage:
                int finalMsgId = int.Parse((sender as CommandEventArgs).CommandArgument.ToString());
                msgID = MoveDataFromFinalToStaging(finalMsgId);
                string query = BuildSecureQueryString(string.Format("{0}={1}&{2}={3}{4}", MESSAGE_ID, msgID, "FeatureMode", (int)WebSiteEnums.FeatureMode.View, ViewMessageIntruderQuery));
                AjaxAddResponseScript("ShowPopupModal('Message_CreateNew.aspx?" + query + "','auto'); return false;");
                break;
        }
    }

    protected void uxRebind_Click(object sender, EventArgs e)
    {
        SetDateFilter();
        OnPostBackActions(PostBackAction.RebindGrid);
    }

    protected void lnkViewMessage_Command(object sender, CommandEventArgs e)
    {
        OnPostBackActions(PostBackAction.ViewMessage, e);
    }

    protected void uxLnkCreateMessage_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CreateMessage, sender);
    }

    private int MoveDataFromFinalToStaging(int messageID)
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@MessageID", messageID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@TemporaryMessageID", 0, DbType.Int32, true));
        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_MoveMessageToStagingTables", paramsIn, out paramsOut);
        int TemporaryID = Int32.Parse(paramsOut[0].ParameterValue.ToString());
        return TemporaryID;
    }


    #region methods
    private int Save()
    {
        int MessageID = 0;
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@MessageID", 0, DbType.Int32, true));
        paramsIn.Add(new FilterParameter("@MessageText", "", DbType.AnsiString));
        paramsIn.Add(new FilterParameter("@FullUserName", "", DbType.AnsiString));
        //paramsIn.Add(new FilterParameter("@SendToCS", "", DbType.Boolean));

        FilterParameterCollection paramsOut = new FilterParameterCollection();

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_AddMessage", paramsIn, out paramsOut);

        MessageID = Int32.Parse(paramsOut[0].ParameterValue.ToString());
        return MessageID;
    }
    #endregion

}
