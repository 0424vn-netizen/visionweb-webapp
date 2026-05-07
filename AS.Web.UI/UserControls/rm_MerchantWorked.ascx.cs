using AS.Common;
using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Web.UI.Controls;
using AS.Controls.UserControls;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Controls.Pages;
using AS.Controls.Exporter;

public partial class UserControls_rm_MerchantWorked : GlobalUserControl
{
    #region Constants
    private const string USER_NAME_FIELD = "UpdatedByUserID";
    private const string USER_ID_FIELD = "UpdatedByUserID";
    private const string USER_DEFAULT_ID_FIELD = "All";
    private const string VIEW_ALL_MERCHANT_WORKED = ",ViewAllMerchantWorked";
    private const string MS_VIEW_ALL_MERCHANT_WORKED = ",MSViewAllMerchantWorked,";
    private const string QUERY_STRING_FOR_RISK_REPORT = "merchantNumber={0}&IsPopup=true";
    private const string QUERY_STRING = "merchantNumber={0}&assignmentId={1}&isFromMerchantWorked={2}&reportDate={3}&assignmentName={4}";
    #endregion

    #region Enum

    enum DataBindAction
    {
        BindingMerchantWorkedGrid,
        BindUserList
    }

    #endregion

    #region Properties

    public enum MerchantWorkedType
    {
        MerchantWorkedPage,
        MerchantWorkedModal
    }

    public MerchantWorkedType Type
    {
        get;
        set;
    }

    bool _isExporting = false;
    private string _GridSubTitle
    {
        get
        {
            string currentDate = DateTime.Now.ToString("MM/dd/yyyy");
            return string.Format(GetLocalResourceObject("GridSubtitle.Text").ToString(), currentDate);
        }
    }

    private string _GridTitle
    {
        get
        {
            return string.Format(GetLocalResourceObject("GridTitle.Text").ToString());
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            OnDataBindControls(DataBindAction.BindUserList, sender);
        var userPermission = SessionManager.CurrentUserPermissions;
        if (userPermission.Contains(VIEW_ALL_MERCHANT_WORKED) || userPermission.Contains(MS_VIEW_ALL_MERCHANT_WORKED))
        {
            uxUserList.Visible = true;
            uxGroupList.Columns.FindByUniqueName("UserName").Visible = true;
        }
        if (Type.Equals(MerchantWorkedType.MerchantWorkedModal))
        {
            uxExporter.IsOnTop = true;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindingMerchantWorkedGrid:
                BindingMerchantWorked(sender);
                break;
            case DataBindAction.BindUserList:
                BindingUserList();
                break;
        }
    }

    protected void uxGroupList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxGroupList)
        {
            OnDataBindControls(DataBindAction.BindingMerchantWorkedGrid, sender);
        }
    }

    protected void uxGroupList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        switch ((MerchantWorkedType)Type)
        {
            case MerchantWorkedType.MerchantWorkedPage:
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    DataRowView dataRow = e.Item.DataItem as DataRowView;
                    string queryString = ReportPage.BuildSecureQueryString(string.Format(QUERY_STRING_FOR_RISK_REPORT, dataRow["MerchantNumber"].ToString()));
                    string urlCard = string.Format("<a href=\"javascript:void(0)\" onclick=\"openPopupWindow('rm_RiskReport.aspx?{0}','RiskReport'); return false;\">", queryString);
                    dataItem["MerchantNumber"].Text = VeraCodeSolution.DoVeraCode(string.Format("{0}{1}</a>", urlCard, dataRow["MerchantNumber"].ToString()));
                }
                break;
            case MerchantWorkedType.MerchantWorkedModal:
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    DataRowView dataRow = e.Item.DataItem as DataRowView;
                    var assignmentName = HttpUtility.UrlEncode(dataRow["AssignmentName"].ToString());
                    string queryString = ReportPage.BuildSecureQueryString(string.Format(QUERY_STRING, dataRow["MerchantNumber"].ToString(), dataRow["AssignmentID"].ToString(), true, dataRow["ReportDate"].ToString(), assignmentName));
                    string urlCard = string.Format("<a onclick='CloseModal(this)' href ='rm_DQNextQReportPopup.aspx?{0}'>", queryString);
                    dataItem["MerchantNumber"].Text = VeraCodeSolution.DoVeraCode(string.Format("{0}{1}</a>", urlCard, dataRow["MerchantNumber"].ToString()));
                }
                break;
        }

    }

    protected void uxUserList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        uxGroupList.Rebind();
    }

    private void BindingMerchantWorked(object sender)
    {
        string spaName = "spa_rm_Mgmt_MerchantWorkedReport";
        string userId = uxUserList.Visible ? uxUserList.SelectedValue : SessionManager.CurrentUser.UserID;

        HierarchyFilterValue reportFilter = SessionManager.CurrentReportFilter ?? new HierarchyFilterValue();
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@UserIDFilter", userId, DbType.String));
        parameters.Add(new FilterParameter("@WorkDate", DateTime.Now, DbType.Date));
        ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME,
            new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
        //title
        uxExporter.GridTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
        uxExporter.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridSubTitle);
    }

    private void BindingUserList()
    {
        string spaName = "spa_rm_Mgmt_GetUserListWorkedReport";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        DataTable userList = WebServices.RiskServices.GetReports(spaName, parameters);
        uxUserList.DataSource = userList;
        uxUserList.DataTextField = USER_NAME_FIELD;
        uxUserList.DataValueField = USER_ID_FIELD;
        uxUserList.DataBind();
        uxUserList.Items.Insert(0, new RadComboBoxItem(GetLocalResourceObject("AllUsers.Text").ToString(), USER_DEFAULT_ID_FIELD));
    }
    protected void uxExporter_NeedExportConfig(object sender, ExportConfig exportConfig)
    {
        var filename = string.Format("{0} _ {1} _ {2}", SessionManager.ClientInfo.ClientName, _GridTitle, DateTime.Now.ToString("MMddyyyyhhmmss"));
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(filename);
        exportConfig.ReportHeader = string.Format("{0} - {1}", _GridTitle, DateTime.Now.ToString("MM/dd/yyyy"));
    }
}