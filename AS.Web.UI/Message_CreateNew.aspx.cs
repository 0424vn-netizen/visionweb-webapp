using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

[PagePermission("SendMsg,MSSendMsg,MSViewMsg")]
public partial class Message_CreateNew : NonReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindHierarchyFilter,
        BindMerchantHierarchyFilter,
        BindMessageInfo,
    }
    enum PostBackAction
    {
        BindHierarchyFilterValue,
        CheckMerchantFilter,
        SaveMessage,
        CancelMessage
    }
    #endregion

    private string ALL_MERCHANT = string.Empty;
    private const string MERCHANT_NUMBER = "MERCHANTNUMBER";
    private string _CurrentHierarchyFilterMode;

    private string _GroupHierarchy = string.Empty;
    private bool _IsShowGroupHierarchy = false;
    private bool _IsShowGroup = false;
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
    public string HierarchyFilterQueryString(string HierarchyFilterMode, string clientIDbtn)
    {
        return BuildSecureQueryString(string.Format("MessageID={0}&hierarchyMode={1}&clientID={2}", MessageID,
            HierarchyFilterMode, clientIDbtn));
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        ALL_MERCHANT = GetLocalResourceObject("Message_CreateNew_aspx_cs_AllMerchants").ToString();
        CHECKBOX_TOOLTIP = GetLocalResourceObject("Message_CreateNew_aspx_cs_SelectToSendMessage").ToString();
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindHierarchyFilter, null);
            CountSelectedFilter();
            OnDataBindControls(DataBindAction.BindMessageInfo);
        }
        if (FeatureMode == WebSiteEnums.FeatureMode.View)
        {
            Page.Title = GetLocalResourceObject("Message_CreateNew_aspx_ViewList").ToString();
        }
        else
        {
            Page.Title = GetLocalResourceObject("PageResource1.Title").ToString();
        }
        VisibleControls();

        //ajaxify each literal control on repeater.
        foreach (RepeaterItem item in uxHierarchyFilterRepeater.Items)
        {
            Panel div = item.FindControl("divHierarchy") as Panel;
            Button btn = item.FindControl("btnRefreshHierarchy") as Button;
            CheckBox cbx = item.FindControl("cbxIsMerchant") as CheckBox;
            Panel pnl = item.FindControl("pnlHierarchyFilterModal") as Panel;
            if (cbx.Checked)
            {
                HiddenField hierarchyMode = (HiddenField)cbx.Parent.FindControl("hddHierarchyFilterMode");

                if (hierarchyMode.Value.ToString().Equals(MERCHANT_NUMBER))
                {
                    Literal ltr = item.FindControl("lblHierarchy") as Literal;
                    ltr.Text = VeraCodeSolution.DoVeraCode(ALL_MERCHANT);
                }
            }

            RadAjaxManagerProxyReview.AjaxSettings.AddAjaxSetting(btn, div);

            RadAjaxManagerProxyReview.AjaxSettings.AddAjaxSetting(cbx, div);
            RadAjaxManagerProxyReview.AjaxSettings.AddAjaxSetting(cbx, pnl);
            RadAjaxManagerProxyReview.AjaxSettings.AddAjaxSetting(cbx, uxCountSelectedFilter);
            RadAjaxManagerProxyReview.AjaxSettings.AddAjaxSetting(btn, uxCountSelectedFilter);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void cbxIsMerchant_CheckedChanged(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.CheckMerchantFilter, sender);
    }


    string CHECKBOX_TOOLTIP = string.Empty;

    protected void uxHierarchyFilterRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                DataRowView row = e.Item.DataItem as DataRowView;
                if (_IsShowGroup == true)
                {
                    if (row["BEProcessor"] == null)
                    {
                        _GroupHierarchy = string.Empty;
                    }
                    else
                    {
                        if (row["BEProcessor"].ToString() != _GroupHierarchy)
                        {
                            _GroupHierarchy = row["BEProcessor"].ToString();
                            _IsShowGroupHierarchy = true;
                        }
                    }
                    if (_IsShowGroupHierarchy)
                    {
                        string hierarchyTitle = string.Empty;
                        if (!_GroupHierarchy.Trim().IsNullOrEmpty())
                        {
                            hierarchyTitle = row["BEProcessor"].ToString().TrimEnd() + " - " + GetLocalResourceObject("Message_CreateNew_aspx_Hierarchy").ToString();
                        }
                        else
                        {
                            hierarchyTitle = GetLocalResourceObject("Message_CreateNew_aspx_Merchant").ToString();
                        }
                        if (FeatureMode == WebSiteEnums.FeatureMode.View)
                        {
                            ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyView")).Visible = true;
                            ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyEdit")).Visible = false;
                            ((Literal)e.Item.FindControl("uxGroupHierarchyTitleView")).Text = VeraCodeSolution.ValidateResponseData(hierarchyTitle);
                        }
                        else
                        {
                            ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyView")).Visible = false;
                            ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyEdit")).Visible = true;
                            ((Literal)e.Item.FindControl("uxGroupHierarchyTitleEdit")).Text = VeraCodeSolution.ValidateResponseData(hierarchyTitle);
                        }
                    }
                    else
                    {
                        ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyEdit")).Visible = false;
                        ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyView")).Visible = false;

                    }
                    _IsShowGroupHierarchy = false;
                }
                else
                {
                    ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyEdit")).Visible = false;
                    ((PlaceHolder)e.Item.FindControl("uxGroupHierarchyView")).Visible = false;
                }
                break;
        }
        string data = string.Empty;
        string _localQueryString = string.Empty;

        Literal ltr = (Literal)e.Item.FindControl("lblHierarchy");
        CheckBox cbx = e.Item.FindControl("cbxIsMerchant") as CheckBox;
        HiddenField hierarchyMode = (HiddenField)cbx.Parent.FindControl("hddHierarchyFilterMode");


        if (FeatureMode == WebSiteEnums.FeatureMode.Edit && (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
        {
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            cbx.ToolTip = string.Format(CHECKBOX_TOOLTIP, dataRow["DisplayedText"].ToString());
        }

        bool IsViewMore = false;
        data = VeraCodeSolution.GetOutputHtmlString(UserControls_Message_HierarchyFilter.GetSelectedValuesAsString(MessageID.ToString(), out IsViewMore, hierarchyMode.Value.ToString()));
        if (IsViewMore)
        {
            _localQueryString = BuildSecureQueryString(string.Format("MessageID={0}&typemodal={1}&hierarchyMode={2}",
                                                                            MessageID,
                                                                            "1",
                                                                            hierarchyMode.Value.ToString()));
            data += "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallHierarchyFilterModal('Message_HierarchyFilter_VMModal.aspx?" + _localQueryString + "','auto');\" >view more...</a>";
        }
        ltr.Text = data;

        //BIND ISMERCHANT
        PlaceHolder plh = e.Item.FindControl("plhHierarchy") as PlaceHolder;
        plh.Visible = FeatureMode == WebSiteEnums.FeatureMode.Edit;

        cbx.Enabled = FeatureMode == WebSiteEnums.FeatureMode.Edit;

        string RecipientMode = hierarchyMode.Value.ToString();

        //if (RecipientMode == "MERCHANTNUMBER")
        //{
        //    cbx.Visible = false;
        //}
        FilterParameterCollection param = new FilterParameterCollection();
        param.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        param.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));
        param.Add(new FilterParameter("@RecipientMode", RecipientMode, DbType.AnsiString));
        param.Add(new FilterParameter("@IsSelected", "true", DbType.Boolean, true));

        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_GetMessageRecipientLevels", param, out paramsOut);

        cbx.Checked = Convert.ToBoolean(paramsOut[0].ParameterValue.ToString());
    }


    protected override void OnPostBackActions(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        string data = string.Empty;
        string _localQueryString = string.Empty;

        switch ((PostBackAction)type)
        {
            case PostBackAction.BindHierarchyFilterValue:
                Button btn = (Button)sender;
                Literal ltr = (Literal)btn.Parent.FindControl("lblHierarchy");


                bool IsViewMore = false;
                data = VeraCodeSolution.GetOutputHtmlString(UserControls_Message_HierarchyFilter.GetSelectedValuesAsString(MessageID.ToString(), out IsViewMore, _CurrentHierarchyFilterMode));
                if (IsViewMore)
                {
                    _localQueryString = BuildSecureQueryString(string.Format("MessageID={0}&typemodal={1}&hierarchyMode={2}",
                                                                                    MessageID,
                                                                                    "1",
                                                                                    _CurrentHierarchyFilterMode));
                    data += "&nbsp;&nbsp<a href=\"#\" onclick=\"return CallHierarchyFilterModal('Message_HierarchyFilter_VMModal.aspx?" + _localQueryString + "','auto');\" >" + GetLocalResourceObject("Message_CreateNew_aspx_ViewMore").ToString() + "</a>";
                }
                ltr.Text = VeraCodeSolution.DoVeraCode(data);
                this.AjaxAddResponseScript("AdjustModalSize();");
                break;
            case PostBackAction.CheckMerchantFilter:
                CheckBox cbx = (CheckBox)sender;
                HiddenField hierarchyMode = (HiddenField)cbx.Parent.FindControl("hddHierarchyFilterMode");
                string RecipientMode = hierarchyMode.Value.ToString();

                if (RecipientMode.Equals(MERCHANT_NUMBER))
                {
                    RepeaterItem item = (RepeaterItem)cbx.NamingContainer;
                    Literal ltl = item.FindControl("lblHierarchy") as Literal;
                    Panel plh = item.FindControl("pnlHierarchyFilterModal") as Panel;
                    if (cbx.Checked)
                    {
                        ltl.Text = VeraCodeSolution.DoVeraCode(ALL_MERCHANT);
                        plh.Visible = false;
                    }
                    else
                    {
                        ltl.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("lblHierarchyResource2.Text").ToString());
                        plh.Visible = true;
                    }
                }

                parameters.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));
                parameters.Add(new FilterParameter("@ChildRecipientMode", "MERCHANTNUMBER", DbType.AnsiString));
                parameters.Add(new FilterParameter("@RecipientMode", RecipientMode, DbType.AnsiString));
                parameters.Add(new FilterParameter("@IsSelected", cbx.Checked, DbType.Boolean));

                FilterParameterCollection paramsOut = new FilterParameterCollection();
                WebServices.RiskServices.ExecuteNonQueryCommand("spa_InsertUpdateRecipientLevels", parameters, out paramsOut);

                break;
            case PostBackAction.SaveMessage:
                UpdateMessage();
                SaveMessageToFinalTables();

                if (FeatureMode == WebSiteEnums.FeatureMode.View)
                    AjaxAddResponseScript("parent.HidePopupModal();");
                else
                    AjaxAddResponseScript("parent.CloseModalAndRebindGrid();");

                break;
            case PostBackAction.CancelMessage:
                parameters.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));
                FilterParameterCollection param = new FilterParameterCollection();
                WebServices.RiskServices.ExecuteNonQueryCommand("spa_EliminateTemporaryMessage", parameters, out param);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseParentModal", "parent.HidePopupModal();", true);
                break;
        }

        CountSelectedFilter();

    }



    protected override void OnDataBindControls(Enum type, object sender)
    {
        string data1 = string.Empty;
        string _localQueryString1 = string.Empty;



        switch ((DataBindAction)type)
        {
            case DataBindAction.BindHierarchyFilter:
                DataTable info = new DataTable();
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                //parameters.AddLanguageID();
                SessionManager.MessageHierarchyFilter = WebServices.RiskServices.GetReports("spa_GetMessageHierarchyList", parameters);
                info = SessionManager.MessageHierarchyFilter;
                int countBEProcessor = 0;
                string beProcessor = string.Empty;
                if (info != null && info.Rows.Count > 0)
                {
                    for (int i = 0; i < info.Rows.Count; i++)
                    {
                        if (!info.Rows[i]["BEProcessor"].ToString().IsNullOrEmpty() && beProcessor != info.Rows[i]["BEProcessor"].ToString())
                        {
                            beProcessor = info.Rows[i]["BEProcessor"].ToString();
                            countBEProcessor++;
                        }
                    }
                }
                if (countBEProcessor > 1)
                {
                    _IsShowGroup = true;
                }
                else
                {
                    _IsShowGroup = false;
                }
                uxHierarchyFilterRepeater.DataSource = info;
                uxHierarchyFilterRepeater.DataBind();
                break;
            case DataBindAction.BindMerchantHierarchyFilter:
                foreach (RepeaterItem item in uxHierarchyFilterRepeater.Items)
                {
                    CheckBox cbx = item.FindControl("cbxIsMerchant") as CheckBox;
                    HiddenField hierarchyMode = (HiddenField)cbx.Parent.FindControl("hddHierarchyFilterMode");

                    string RecipientMode = hierarchyMode.Value.ToString();
                    FilterParameterCollection param = new FilterParameterCollection();
                    param.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    param.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));
                    param.Add(new FilterParameter("@RecipientMode", RecipientMode, DbType.AnsiString));
                    param.Add(new FilterParameter("@IsSelected", "true", DbType.Boolean, true));

                    FilterParameterCollection paramsOut = new FilterParameterCollection();
                    WebServices.RiskServices.ExecuteNonQueryCommand("spa_GetMessageRecipientLevels", param, out paramsOut);

                    cbx.Checked = Convert.ToBoolean(paramsOut[0].ParameterValue.ToString());


                }
                break;
            case DataBindAction.BindMessageInfo:
                DataTable dt = new DataTable();
                FilterParameterCollection parames = new FilterParameterCollection();
                parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parames.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));
                dt = WebServices.RiskServices.GetReports("spa_GetMessageInfo", parames);
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

    protected void btnRefreshHierarchy_Command(object sender, CommandEventArgs e)
    {

        _CurrentHierarchyFilterMode = e.CommandArgument.ToString();
        OnPostBackActions(PostBackAction.BindHierarchyFilterValue, sender);
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
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_UpdateMessage", paramsIn, out paramsOut);
    }

    protected void SaveMessageToFinalTables()
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

        paramsIn.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));

        FilterParameterCollection paramsOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_SaveMessageToFinalTables", paramsIn, out paramsOut);

    }


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

    protected void CountSelectedFilter()
    {
        int count = 0;
        foreach (RepeaterItem item in uxHierarchyFilterRepeater.Items)
        {
            Literal ltr = item.FindControl("lblHierarchy") as Literal;
            if (ltr.Text.ToString() != "N/A")
                count++;
        }

        uxCountSelectedFilter.Value = VeraCodeSolution.ValidateResponseData(count.ToString());
    }
}
