using AS.Common.DBManager;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI.Controls;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

[PagePermission("MSSendMsg,MSViewMsg,SendSrvMsg,ViewSrvMsg")]
public partial class Message_ViewUnreadMessagesModal : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindUnreadMessage
    }
    enum PostBackAction
    {
        ReviewPastMessages,
        ReadMesage
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();

        string data = string.Empty;
        string _localQueryString = string.Empty;

        switch ((PostBackAction)type)
        {
            case PostBackAction.ReviewPastMessages:
                if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || this.IsUserWithPermission("ViewSrvMsg"))
                    Response.Redirect("ServiceMessage_View.aspx");
                else
                    Response.Redirect("ViewMessage.aspx");
                break;
            case PostBackAction.ReadMesage:
                string messageID = this.uxMessageID.Value.ToString();
                this.RemoveUnreadMessage(messageID);
                this.uxReportGrid.Rebind();
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindUnreadMessage:
                DataTable info = new DataTable();
                string spaName = string.Empty;
                if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS)
                    spaName = "spa_cs_GetMySrvMessages";
                else
                    spaName = "spa_GetMyMessages";
                parameters.Add(new FilterParameter("@DateFilterMode", (int)DateOptionMode.DateRange, DbType.Int32));
                parameters.Add(new FilterParameter("@BeginDate", DateTime.Now.GetFirstDayOfMonth(), DbType.DateTime));
                parameters.Add(new FilterParameter("@EndDate", DateTime.Now, DbType.DateTime));
                parameters.Add(new FilterParameter("@View", "N", DbType.AnsiString));
                ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                break;

        }
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid)
        {
            OnDataBindControls(DataBindAction.BindUnreadMessage, sender);
        }
    }

    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {
        this.uxReportGrid.Columns.FindByUniqueName("MessageID").Visible = false;
        exportConfig.ReportHeader = GetLocalResourceObject("Message_ViewUnreadMessagesModal_apsx_cs_NewMessages").ToString();
        exportConfig.FileName = GetLocalResourceObject("Message_ViewUnreadMessagesModal_apsx_cs_UnreadMessage").ToString();
    }

    protected void uxReviewPastMessages_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ReviewPastMessages);
    }

    private void RemoveUnreadMessage(string messageID)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();

        parameters.Add(new FilterParameter("@MessageList", messageID, DbType.AnsiString));
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_UpdateUnreadMessages", parameters, out parameterOut);
    }

    protected void uxReadMesage_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ReadMesage);
    }
}