using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Data;
using Telerik.Web.UI;

[PagePermission("UserAct")]
public partial class UserActivity : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindUserList,
        BindUserGrid,
        BindHeaderText
    }

    enum PostBackAction
    {
        DoSearching
    }
    #endregion

    #region Const

    const string MERCHANT_NUMBER = "MerchantNumber";
    const string MERCHANT_NAME = "MerchantName";

    #endregion

    #region Properties
    private HierarchyFilterValue _reportValue = null;
    string _GridHeader = string.Empty;
    string _GridSubHeader = string.Empty;
    private string GridSubHeader
    {
        get
        {
            if (string.IsNullOrEmpty(uxMerchantNumber.Text.Trim()))
                _GridSubHeader = string.Format("{0} - {1}", uxUserList.SelectedValue, uxDate.SelectedDate.Value.ToGenericDateString());
            else
                _GridSubHeader = string.Format("{0} - {1} - {2}", uxUserList.SelectedValue, uxMerchantNumber.Text.Trim(), uxDate.SelectedDate.Value.ToGenericDateString());

            return _GridSubHeader;
        }
    }

    #endregion

    protected override void PageInitialize()
    {
        if (IsIntruderDetected) return;
        this.GridIDs.Add("uxUserActivityGrid");
        this.ExporterIDs.Add("uxExporterTop");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _GridHeader = GetLocalResourceObject("UserActivity_aspx_cs_GridHeader").ToString();
        
        if (!IsPostBack)
        {
            if (SavedReportFilterValue != null && GeneralFuncsLib.IsMerchantMode(SavedReportFilterValue.HierarchyMode))
                uxMerchantNumber.Text = VeraCodeSolution.DoVeraCode(SavedReportFilterValue.Value.Trim());
            OnDataBindControls(DataBindAction.BindUserList);
            uxDate.SelectedDate = DateTime.Today.Date;
        }
        uxDate.MaxDate = DateTime.Now;
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxUserActivityGrid && uxUserActivityGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindUserGrid, sender);
            OnDataBindControls(DataBindAction.BindHeaderText);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindUserGrid:
                string spaName = "spa_rm_cs_GetUserActivityList";
                FilterParameterCollection _parames = new FilterParameterCollection();
                _parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                _parames.Add(new FilterParameter("@RiskUserId", uxUserList.SelectedValue, DbType.AnsiString));
                _parames.Add(new FilterParameter("@MerchantNumber", uxMerchantNumber.Text.Trim(), DbType.AnsiString));
                _parames.Add(new FilterParameter("@ViewDate", uxDate.SelectedDate.Value, DbType.DateTime));

                ASGrid grid = (ASGrid)sender;
                grid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(_parames) });

                break;
            case DataBindAction.BindUserList:

                uxUserList.DataValueField = "UserId";
                uxUserList.DataTextField = "UserText";
                FilterParameterCollection parames = new FilterParameterCollection();
                parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                uxUserList.DataSource = WebServices.RiskServices.GetReports("spa_rm_cs_GetRiskUsers", parames);
                uxUserList.DataBind();
                RadComboBoxItem item = new RadComboBoxItem(string.Empty, string.Empty);
                uxUserList.Items.Insert(0, item);

                break;
            case DataBindAction.BindHeaderText:
                int totalMerchant, totalEscalation;
                GetTotalMerchantAndEscalation(out totalMerchant, out totalEscalation);
                string headerText = "<span style=\"font-size: 12px; font-style: normal;font-weight:normal;\"><b>" + GetLocalResourceObject("UserActivity_aspx_cs_TotalMerchantWork").ToString() + "</b> {0}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + GetLocalResourceObject("UserActivity_aspx_cs_TotalMerchantEscalated").ToString() + "</b> {1}</span>";
                uxExporterTop.GridTitle = _GridHeader;
                uxExporterTop.GridSubTitle = VeraCodeSolution.DoVeraCode(GridSubHeader);
                uxHeaderTotal.Text = VeraCodeSolution.DoVeraCode(string.Format(headerText, totalMerchant, totalEscalation));
                break;
        }
    }

    private void GetTotalMerchantAndEscalation(out int totalMerchant, out int totalEscalation)
    {
        string spaName = "spa_rm_cs_CountUserActivity";
        FilterParameterCollection paramesIn = new FilterParameterCollection();
        paramesIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramesIn.Add(new FilterParameter("@RiskUserId", uxUserList.SelectedValue, DbType.AnsiString));
        paramesIn.Add(new FilterParameter("@MerchantNumber", uxMerchantNumber.Text.Trim(), DbType.AnsiString));
        paramesIn.Add(new FilterParameter("@ViewDate", uxDate.SelectedDate.Value, DbType.DateTime));
        DataTable table = WebServices.RiskServices.GetReports(spaName, paramesIn);
        if (table != null && table.Rows.Count > 0)
        {
            totalMerchant = int.Parse(table.Rows[0]["MerchantNumberCount"].ToString());
            totalEscalation = int.Parse(table.Rows[0]["EscalationCount"].ToString());
        }
        else
        {
            totalMerchant = 0;
            totalEscalation = 0;
        }
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        int totalMerchant, totalEscalation;
        GetTotalMerchantAndEscalation(out totalMerchant, out totalEscalation);
        string headerText = _GridHeader + GridSubHeader + string.Format("    " + GetLocalResourceObject("UserActivity_aspx_cs_TotalMerchantWork").ToString() + " {0}    " + GetLocalResourceObject("UserActivity_aspx_cs_TotalMerchantEscalated").ToString() + " {1}", totalMerchant, totalEscalation);
       
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(_GridHeader + GridSubHeader);
        exportConfig.ReportHeader = headerText;
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.DoSearching:
                uxUserActivityGrid.CurrentPageIndex = 0;
                uxUserActivityGrid.Rebind();
                break;
        }
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.DoSearching);
        if (SavedReportFilterValue != null && GeneralFuncsLib.IsMerchantMode(SavedReportFilterValue.HierarchyMode))
        {
            this._reportValue = new HierarchyFilterValue();
            this._reportValue.DateOptionValue.From = this._reportValue.DateOptionValue.To = (DateTime)uxDate.SelectedDate.Value;
            this._reportValue.HierarchyMode = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;
            this._reportValue.ID = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;
            this._reportValue.DateOption = AS.Web.UI.Controls.DateOptionMode.Daily;
            this._reportValue.Value = uxMerchantNumber.Text.Trim();
            SavedReportFilterValue = _reportValue;
        }
    }
}
