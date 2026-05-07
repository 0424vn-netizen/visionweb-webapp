using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using System.Data;
using AS.Common;
using Telerik.Web.UI;
using AS.Controls.Grid;
using AS.Controls.Pages;
using System.Web.Services;
using AS.Web.Business;

[PagePermission("SendSrvMsg,ViewSrvMsg")]
public partial class ServiceMessage_CreateNew : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindMessageInfo,
        BindUserList,
    }
    enum PostBackAction
    {
        SaveMessage,
        CancelMessage,
        SelectItem,
    }
    #endregion

    #region Const

    protected const string IS_SELECT_ALL_FLAG = "true";
    protected const string IS_DESELECT_ALL_FLAG = "false";
    const string IS_ADD_FLAG = "true";

    #endregion
    protected bool IsAllFlag
    {
        get
        {
            if (this.uxAllFlag.Value.Trim().Equals(IS_SELECT_ALL_FLAG))
                return true;
            else
                return false;
        }
    }

    protected bool IsSelectAll
    {
        get
        {
            if (this.uxIsSelectAll.Value.Trim().Equals(IS_SELECT_ALL_FLAG))
                return true;
            else
                return false;
        }
    }

    protected bool IsAdded
    {
        get
        {
            if (this.uxIsAdded.Value.Trim().Equals(IS_ADD_FLAG))
                return true;
            else
                return false;
        }
    }

    protected string SelectedValue
    {
        get
        {
            return uxValueCode.Value.Trim();
        }
    }

    public int MessageID
    {
        get
        {
            if (SecureQueryString["MessageID"] != null)
            {
                int msgID = int.Parse(SecureQueryString["MessageID"]);
                return msgID;
            }
            else
            {
                return 0;
            }
        }
    }

    protected WebSiteEnums.FeatureMode FeatureMode
    {
        get
        {
            if (SecureQueryString["FeatureMode"] != null)
            {
                int featureMode = int.Parse(SecureQueryString["FeatureMode"]);
                return featureMode == null || featureMode == 0 ? WebSiteEnums.FeatureMode.Edit : WebSiteEnums.FeatureMode.View;
            }
            else
            {
                return WebSiteEnums.FeatureMode.Edit;
            }
        }
    }

    protected int MaxLengthViewMore
    {
        get
        {
            return Convert.ToInt32(WebSiteSettings.RiskViewMore);
        }
    }

    private void VisibleControls()
    {
        uxPostedByEdit.Visible = txtMessage.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;
        ltrPostedBy.Visible = ltrMessage.Visible = FeatureMode == WebSiteEnums.FeatureMode.View;
        uxPlhBottom.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;
    }

    protected override void PageInitialize()
    {

        this.GridIDs.Add("uxUserList");
        base.PageInitialize();
    }
    private void RemoveFilterMenuItem()
    {
        //show filter menu
        var grids = new RadGrid[] { uxUserList };
        var removedItems = new string[] { 
            "GreaterThan",
            "LessThan", "GreaterThanOrEqualTo", "LessThanOrEqualTo", "Between", "NotBetween",
            "IsEmpty", "NotIsEmpty", "IsNull", "NotIsNull" 
        };
        foreach (var grid in grids)
        {
            for (int i = 0; i < removedItems.Length; i++)
            {
                var mi = grid.FilterMenu.Items.FindItemByText(removedItems[i]);
                if (mi != null)
                    grid.FilterMenu.Items.Remove(mi);
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (IsIntruderDetected) return;
        IsBindDataOnLoad = true;
        RemoveFilterMenuItem();

        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindMessageInfo);
            uxMessageID.Value = MessageID.ToString();
        }

        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            Page.Title = GetLocalResourceObject("ServiceMessage_CreateNew_aspx_ViewList").ToString();
        }
        else
        {
            Page.Title = GetLocalResourceObject("ServiceMessage_CreateNew_aspx_SendNewMessage").ToString();
        }

        VisibleControls();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindUserList, sender);
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            AS.Controls.Global.CheckBox chkHeader = e.Item.FindControl("chkHeader") as AS.Controls.Global.CheckBox;
            chkHeader.Visible = !string.IsNullOrEmpty(uxUserList.MasterTableView.FilterExpression) && (uxUserList.MasterTableView.DataSourceCount > 0);
            chkHeader.Checked = IsAllFlag && chkHeader.Visible;

            string chkHeaderEvent = "doHeaderCheck({0})";
            chkHeader.Attributes["onclick"] = string.Format(chkHeaderEvent, "this");
        }
        else if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            AS.Controls.Global.CheckBox chkItem = dataItem.FindControl("chkItem") as AS.Controls.Global.CheckBox;
            if (FeatureMode == WebSiteEnums.FeatureMode.Edit)
                chkItem.Checked = dataRow["Assigned"].ToString().Equals("1");

            string chkItemEvent = "doItemCheck({0},{1})";
            chkItem.Attributes["onclick"] = string.Format(chkItemEvent, "this", "'" + dataRow["UserID"].ToString() + "'");

        }

    }

    protected void uxUserList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.FilterCommandName)
        {
            ClearFilterCheckAll();
        }
    }

    protected override void DoSwitchView()
    {
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            uxUserList.AllowFilteringByColumn = false;
            uxUserList.Columns.FindByUniqueName("Assigned").Visible = false;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        string data = string.Empty;
        string _localQueryString = string.Empty;

        switch ((PostBackAction)type)
        {
            case PostBackAction.SaveMessage:
                UpdateMessage();
                SaveMessageToFinalTables();

                if (FeatureMode == WebSiteEnums.FeatureMode.View)
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideModal", "parent.HidePopupModal();", true);
                else
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseAndRebind", "parent.CloseModalAndRebindGrid();", true);

                break;
            case PostBackAction.CancelMessage:
                parameters.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));
                FilterParameterCollection param = new FilterParameterCollection();
                WebServices.CsReportServices.ExecuteNonQueryCommand("spa_EliminateTemporaryMessage", parameters, out param);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseParentModal", "parent.HidePopupModal();", true);
                break;
            case PostBackAction.SelectItem:
                SaveRecipients(IsAdded, IsSelectAll, SelectedValue, this.uxUserList.AS_FilterExpression);
                break;
        }

    }



    protected override void OnDataBindControls(Enum type, object sender)
    {
        string data1 = string.Empty;
        string _localQueryString1 = string.Empty;



        switch ((DataBindAction)type)
        {
            case DataBindAction.BindUserList:
                {
                    string spaName = "";
                    FilterParameterCollection _parames = new FilterParameterCollection();
                    _parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    _parames.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));
                    if (FeatureMode == WebSiteEnums.FeatureMode.Edit)
                    {
                        spaName = "spa_cs_GetSrvMessage_UserList";
                    }
                    else
                    {
                        spaName = "spa_cs_GetSrvMessage_Recipients";
                    }
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(_parames) });
                }
                break;
            case DataBindAction.BindMessageInfo:
                DataTable dt = new DataTable();
                FilterParameterCollection parames = new FilterParameterCollection();
                parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parames.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));
                dt = WebServices.CsReportServices.GetReports("spa_cs_GetSrvMessage_Info", parames);
                if (dt.Rows.Count > 0)
                {
                    ltrPostedBy.Text = VeraCodeSolution.ValidateResponseData(dt.Rows[0]["PostedBy"].ToString());
                    ltrMessage.Text = VeraCodeSolution.ValidateResponseData(dt.Rows[0]["Message"].ToString());
                }
                else
                {
                    ltrMessage.Text = ltrPostedBy.Text = string.Empty;
                }

                break;
        }
    }

    protected void btnHidden_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SelectItem);
    }

    protected void uxCancel_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CancelMessage);
    }

    protected void uxSave_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SaveMessage);
    }


    protected void UpdateMessage()
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@MessageText", txtMessage.Text.ToString(), DbType.AnsiString));
        paramsIn.Add(new FilterParameter("@FullUserName", txtPostedBy.Text.ToString(), DbType.AnsiString));
        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.CsReportServices.ExecuteNonQueryCommand("spa_UpdateMessage", paramsIn, out paramsOut);
    }

    protected void SaveMessageToFinalTables()
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));

        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.CsReportServices.ExecuteNonQueryCommand("spa_cs_SaveSrvMessageToFinalTables", paramsIn, out paramsOut);

    }

    private void ClearFilterCheckAll()
    {
        this.uxAllFlag.Value = IS_DESELECT_ALL_FLAG;
    }

    private int SaveRecipients(bool isAdded, bool isSelectAll, string value, string filter)
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _params.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));
        _params.Add(new FilterParameter("@IsSelectAll", isSelectAll, DbType.Boolean));
        _params.Add(new FilterParameter("@IsAdded", isAdded, DbType.Boolean));
        _params.Add(new FilterParameter("@SelectedValue", value, DbType.AnsiString));
        _params.Add(new FilterParameter("@strFilter", filter, DbType.AnsiString));

        FilterParameterCollection _paramsOut = new FilterParameterCollection();

        int res = WebServices.CsReportServices.ExecuteNonQueryCommand("spa_cs_SaveSrvMessage_Recipients", _params, out _paramsOut);

        return res;
    }
    [WebMethod(EnableSession = true)]
    public static string[] ValidateHasRecipients(string buttonId, string messageID)
    {
        int _messageID = 0;
        Int32.TryParse(messageID, out _messageID);
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@MessageID", _messageID, DbType.Int32));
        parameters.Add(new FilterParameter("@Count", 0, DbType.Int32, true));
        FilterParameterCollection parameterOut = new FilterParameterCollection();

        int r = WebServices.CsReportServices.ExecuteNonQueryCommand("spa_cs_GetSrvMessage_Validate", parameters, out parameterOut);

        int result = Convert.ToInt32(parameterOut[0].ParameterValue);

        bool valid = (result > 0 ? true : false);
        return new string[] { valid.ToString().ToLower(), buttonId.Replace('_', '$') };
    }
}
