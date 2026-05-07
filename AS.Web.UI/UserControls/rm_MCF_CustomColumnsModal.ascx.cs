using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.VW.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_rm_MCF_CustomColumnsModal : GlobalUserControl
{
    #region ---- Member variable ----
    private String ViewData { get; set; }
    protected const string FULL_VIEW = "full view";
    const string COL_RESULT = "Result";
    const string COL_ASSIGNMENT_LIST = "AssignmentList";
    const string COL_USER_LIST = "UserList";
    #endregion ---- Member variable ----
    #region ---- Properties ----
    private CustomViewMessageResourceType GetCustomViewMessageResourceTypeMode
    {
        get
        {
            if (Page.SecureQueryString["CustomViewMessageResourceTypeMode"] != null)
            {
                return (CustomViewMessageResourceType)Enum.Parse(typeof(CustomViewMessageResourceType), Page.SecureQueryString["CustomViewMessageResourceTypeMode"]);
            }
            else
            {
                return CustomViewMessageResourceType.Assignments;
            }
        }
    }


    public string IsSelected
    {
        get
        {
            if (Page.SecureQueryString["IsSelected"] != null)
            {
                return Page.SecureQueryString["IsSelected"];
            }
            else
            {
                return string.Empty;
            }
        }
    }
    public string CustomViewID
    {
        set { ViewState["CustomViewID"] = value; }
        get
        {
            return (ViewState["CustomViewID"]) != null ? ViewState["CustomViewID"].ToString() : string.Empty;
        }
    }
    public string PageMode
    {
        get
        {
            if (Page.SecureQueryString["PageMode"].ToString() != null)
            {
                return Page.SecureQueryString["PageMode"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
    public string ViewName { get; set; }
    public int ViewType { get; set; }
    public bool IsCreate
    {
        set { ViewState["IsCreate"] = value; }
        get
        {
            return (ViewState["IsCreate"]).ToBoolean();
        }
    }
    #endregion ---- Properties ----
    #region ---- Events ----
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            uxLeftGrid.DataBind();
            uxRightGrid.DataBind();
            DisplayViewType();
            if (PageMode == "RiskReport")
            {
                Label1.Text = "Choose up to 20 columns";
            }
            else if (PageMode == "TransactionHistory")
            {
                Label1.Text = "Choose up to 16 columns";
            }
        }
    }
    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        if (!ValidateData())
            return;
        var displayedItems = uxRightGrid.Items.Cast<RadListBoxItem>().Where(x => string.IsNullOrEmpty(x.Attributes["IsDefault"]))
            .Select(x => x.Value).ToList();
        ViewData = string.Join(",", displayedItems);
        if (IsCreate)
        {
            AddCustomView();
        }
        else
        {
            if (!ViewType.Equals((int)WebSiteEnums.ManageCustomView.Private) && GetViewType().Equals((int)WebSiteEnums.ManageCustomView.Private))
            {
                DataTable warningData = GeneralFuncsLib.GetWarningEditCustomView(CustomViewID.ToInt());
                if (warningData.Rows.Count > 0 && warningData.Rows[0][COL_RESULT].ToString().Equals("0"))
                {
                    string assignmentList = warningData.Rows[0][COL_ASSIGNMENT_LIST].ToString();
                    string userList = warningData.Rows[0][COL_USER_LIST].ToString();
                    string queryString = Page.BuildSecureQueryString("customViewMessageResourceTypeMode=" + GetCustomViewMessageResourceTypeMode.ToString() + "&Mode=1&Data=" + assignmentList + "," + userList);
                    string action = "CustomColumnModal.doOpenEditMessagePopup('rm_MCF_MessageCustomModal.aspx?" + queryString + "')";
                    ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(action);
                    ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"window.CountDisplayedColumn();"));
                    return;
                }
                else
                {
                    UpdateCustomView();
                }
            }
            else
                UpdateCustomView();
        }
        if (PageMode == "TransactionHistory")
        {
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"window.CustomColumnModal.SubmitTransactionHistoryColumnConfigurationModal();"));
        }
        else
        {
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"window.CustomColumnModal.SubmitColumnConfigurationModal();"));
        }
    }

    protected void uxRightGrid_ItemDataBound(object sender, RadListBoxItemEventArgs e)
    {
        DataRowView dataSourceRow = (DataRowView)e.Item.DataItem;
        RadToolTip uxRadToolTipUnused = new RadToolTip();
        uxRadToolTipUnused.TargetControlID = dataSourceRow.Row["Key"].ToString();

        if (PageMode != "RiskReport")
        {
            uxRadToolTipUnused.Text = "<span class='radtooltip-content'>" + RM_MCF_GeneralFuncsLib.GetResourceValue(dataSourceRow.Row["Key"].ToString() + "_Tooltip") + "</span>";
        }
        else
        {
            uxRadToolTipUnused.Text = "<span class='radtooltip-content'>" + RM_MCF_GeneralFuncsLib.GetResourceValue(dataSourceRow.Row["Key"].ToString() + "_RiskTooltip") + "</span>";
        }
        uxRadToolTipUnused.RenderMode = RenderMode.Lightweight;
        uxRadToolTipUnused.CssClass = "customview-tooltip";
        uxRadToolTipUnused.RelativeTo = ToolTipRelativeDisplay.Element;
        uxRadToolTipUnused.OffsetX = 43;
        uxRadToolTipUnused.Position = ToolTipPosition.MiddleLeft;
        uxRadToolTipUnused.IsClientID = true;
        uxRadToolTipUnused.ShowDelay = 500;
        uxRadToolTipUnused.AutoCloseDelay = 0;
        uxRadToolTipUnused.HideDelay = 0;
        e.Item.Controls.Add(uxRadToolTipUnused);

        e.Item.Attributes["IsDefault"] = (dataSourceRow.Row["IsDefault"].ToString().ToLower() == "true") ? "item-customview-isdefault" : string.Empty;
        e.Item.Attributes["IsNotCustomizable"] = (dataSourceRow.Row["IsDefault"].ToString().ToLower() == "true") ? GetLocalResourceObject("txtNotCustomizable").ToString() : string.Empty;
        e.Item.Attributes["IdTooltip"] = uxRadToolTipUnused.ClientID.ToString();
    }
    protected void uxLeftGrid_ItemDataBound(object sender, RadListBoxItemEventArgs e)
    {
        DataRowView dataSourceRow = (DataRowView)e.Item.DataItem;
        RadToolTip uxRadToolTipUnused = new RadToolTip();
        uxRadToolTipUnused.TargetControlID = dataSourceRow.Row["Key"].ToString();
        if (PageMode != "RiskReport")
        {
            uxRadToolTipUnused.Text = "<span class='radtooltip-content'>" + RM_MCF_GeneralFuncsLib.GetResourceValue(dataSourceRow.Row["Key"].ToString() + "_Tooltip") + "</span>";
        }
        else
        {
            uxRadToolTipUnused.Text = "<span class='radtooltip-content'>" + RM_MCF_GeneralFuncsLib.GetResourceValue(dataSourceRow.Row["Key"].ToString() + "_RiskTooltip") + "</span>";
        }
        uxRadToolTipUnused.RenderMode = RenderMode.Lightweight;
        uxRadToolTipUnused.CssClass = "customview-tooltip";
        uxRadToolTipUnused.RelativeTo = ToolTipRelativeDisplay.Element;
        uxRadToolTipUnused.Position = ToolTipPosition.MiddleRight;
        uxRadToolTipUnused.OffsetX = 30;
        uxRadToolTipUnused.IsClientID = true;
        uxRadToolTipUnused.ShowDelay = 500;
        uxRadToolTipUnused.AutoCloseDelay = 0;
        uxRadToolTipUnused.HideDelay = 0;
        e.Item.Controls.Add(uxRadToolTipUnused);
        e.Item.Attributes["IsDefault"] = (dataSourceRow.Row["IsDefault"].ToString().ToLower() == "true") ? "item-customview-isdefault" : string.Empty;
        e.Item.Attributes["IsNotCustomizable"] = (dataSourceRow.Row["IsDefault"].ToString().ToLower() == "true") ? GetLocalResourceObject("txtNotCustomizable").ToString() : string.Empty;
        e.Item.Attributes["IdTooltip"] = uxRadToolTipUnused.ClientID.ToString();
    }
    protected void uxSaveEdit_Click(object sender, EventArgs e)
    {
        var displayedItems = uxRightGrid.Items.Cast<RadListBoxItem>().Where(x => string.IsNullOrEmpty(x.Attributes["IsDefault"]))
           .Select(x => x.Value).ToList();
        ViewData = string.Join(",", displayedItems);
        UpdateCustomView();
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"window.CustomColumnModal.SubmitColumnConfigurationModal();"));
    }
    #endregion ---- Events ----
    #region ---- Private Methods ----
    protected void UpdateCustomView()
    {
        var parameters = new FilterParameterCollection();

        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@ViewName", txtNameCustomView.Text.Trim(), DbType.String));
        parameters.Add(new FilterParameter("@ViewType", GetViewType(), DbType.Int32));
        parameters.Add(new FilterParameter("@ViewData", ViewData, DbType.String));
        parameters.Add(new FilterParameter("@CustomViewID", CustomViewID, DbType.Int32));
        if (PageMode == "TransactionHistory")
        {
            parameters.Add(new FilterParameter("@PageType", "RiskReport_TransactionHistory", DbType.String));
        }
        else if (PageMode != "RiskReport")
        {
            parameters.Add(new FilterParameter("@PageType", "Assignment", DbType.String));
        }
        WebServices.RiskServices.GetReports("spa_RM_MCF_UpdateCustomView", parameters);
    }
    private void DisplayViewType()
    {
        bool isPublic = Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PUBLIC_VIEW_MS);
        bool isPrivate = isPublic || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW) || Page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RSK_MNG_PRIVATE_VIEW_MS);
        rdPublicView.Visible = isPublic;
        rdPrivateView.Visible = isPrivate;
        if (IsCreate)
        {
            rdPublicView.Checked = rdPublicView.Visible;
            rdPrivateView.Checked = !rdPublicView.Checked;
        }
        else
        {
            rdPublicView.Checked = !(rdPrivateView.Checked = ViewType.Equals((int)WebSiteEnums.ManageCustomView.Private));
        }

        uxViewTypePanel.Visible = PageMode == "Assignment" ? false : true;
    }
    private bool ValidateData()
    {
        if (!CheckCustomViewName().Equals(1))
        {
            ShowMessageError(GetLocalResourceObject("ValidationMessages_Require").ToString());
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"AdjustModalSize();"));
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"CountDisplayedColumn();"));
            return false;
        }
        else
            txtNameErrMsg.Message = string.Empty;
        string strRegex = WebSiteConstants.REG_SPECIAL_CHARACTERS;
        Regex re = new Regex(strRegex);
        string customView = txtNameCustomView.Text;
        if (!re.IsMatch(customView))
        {
            ShowMessageError(GetLocalResourceObject("ValidationMessages_V1.Message").ToString());
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"AdjustModalSize();"));
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"CountDisplayedColumn();"));
            return false;
        }
        if (FULL_VIEW.Equals(customView.Trim().ToLower()))
        {
            ShowMessageError(GetLocalResourceObject("ValidationMessages_DuplicateFullView.Message").ToString());
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"AdjustModalSize();"));
            ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"CountDisplayedColumn();"));
            return false;
        }

        return true;
    }
    private void ShowMessageError(string message)
    {
        txtNameErrMsg.Message = VeraCodeSolution.DoVeraCode(message);
        txtNameErrMsg.ShowOnLoad = true;
        lbtNameCustomView.CssClass = "control-label label-error";
    }
    /// <summary>
    /// Get Unused Columns
    /// </summary>
    private void GetUnusedColumn()
    {
        DataTable data = new DataTable();
        switch (PageMode)
        {
            case "RiskReport":
                data = RM_MCF_GeneralFuncsLib.GetRiskReportCustomizeColumnsToXML();
                break;
            case "TransactionHistory":
                data = RM_MCF_GeneralFuncsLib.GetRiskReportTransactionHistoryCustomizeColumnsToXML();
                break;
            default:
                data = RM_MCF_GeneralFuncsLib.GetAssignmentCustomizeColumnsToXML();
                break;
        }
        var displayColumns = new DataTable();
        var displayColumnList = new List<string>();
        var unusedColumns = new DataTable();

        var dataTemp = data.AsEnumerable().Where(x => x["IsDefault"].ToString().ToLower() != "true");

        if (!string.IsNullOrEmpty(CustomViewID))
        {
            displayColumns = GetCustomDisplayedColumns();
            if (displayColumns.Rows.Count > 0)
            {
                displayColumnList = displayColumns.Rows[0]["ViewData"].ToString().Split(',').ToList();
            }
            foreach (var item in displayColumnList)
            {
                dataTemp = dataTemp.Where(x => item != (x.Field<string>("Key")));
            }
        }

        if (PageMode != "RiskReport" && PageMode != "TransactionHistory")
        {
            var listExtendColumn = RM_MCF_GeneralFuncsLib.ExtendCustomColumn();

            var dataExtend = data.AsEnumerable().Where(x => x["IsHide"].ToString().ToLower() == "true");
            if (listExtendColumn != null)
            {
                foreach (var itemEx in listExtendColumn)
                {
                    dataExtend = dataExtend.Where(x => !x["Key"].ToString().ToLower().Equals(itemEx.ToLower()));
                }
            }

            if (dataExtend.Count() > 0)
            {
                foreach (var item in dataExtend)
                {
                    dataTemp = dataTemp.Where(x => x["Key"].ToString().ToLower() != item["Key"].ToString().ToLower());
                }
            }
        }

        var excludeValues = new HashSet<string>();
        if (PageMode == "RiskReport" && !GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR").ToLower().Equals("true"))
        {
            excludeValues = new HashSet<string> { "FirstChargebackRDRCount", "FirstChargebackRDRAmount"
                ,"PostChargebackRDRCount", "PostChargebackRDRAmount" };
        }

        if (PageMode == "RiskReport" && !GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RDR_PERCENT").ToLower().Equals("true"))
        {
            excludeValues.Add("PostChargebackRDRCountPercent");
            excludeValues.Add("PostChargebackRDRAmountPercent");
        }

        if (PageMode == "Assignment" || PageMode == "DetectionQueue")
        {
            excludeValues.Add("SaleGroup");
        }

        if (PageMode == "DetectionQueue")
        {
            excludeValues.Add("ModelScoreValue");
            excludeValues.Add("ModelAlertValue");
        }

        dataTemp = dataTemp.Where(row => !excludeValues.Contains(row["Key"].ToString()));

        if (dataTemp.Count() > 0)
        {
            unusedColumns = dataTemp.CopyToDataTable();
        }

        unusedColumns.Columns.Add("ColumnText", typeof(string));

        foreach (DataRow item in unusedColumns.Rows)
        {
            item["ColumnText"] = RM_MCF_GeneralFuncsLib.GetResourceValue(item["Key"].ToString() + "_CustomView_Text");
        }

        //CR - 43841 - Sort unused column
        unusedColumns.DefaultView.Sort = "ColumnText asc";
        DataTable dt = unusedColumns.DefaultView.ToTable();

        uxLeftGrid.DataSource = dt;
        uxLeftGrid.DataBind();
    }
    /// <summary>
    /// Get Displayed Columns
    /// </summary>
    private void GetDisplayedColumn()
    {

        DataTable data = new DataTable();
        switch (PageMode)
        {
            case "RiskReport":
                data = RM_MCF_GeneralFuncsLib.GetRiskReportCustomizeColumnsToXML();
                break;
            case "TransactionHistory":
                data = RM_MCF_GeneralFuncsLib.GetRiskReportTransactionHistoryCustomizeColumnsToXML();
                break;
            default:
                data = RM_MCF_GeneralFuncsLib.GetAssignmentCustomizeColumnsToXML();
                break;
        }
        var displayData = new DataTable();
        var displayColumns = new DataTable();
        var displayColumnList = new List<string>();
        if (!string.IsNullOrEmpty(CustomViewID))
        {
            displayData = GetCustomDisplayedColumns();

            if (displayData.Rows.Count > 0)
            {
                displayColumnList = displayData.Rows[0]["ViewData"].ToString().Split(',').ToList();
            }


            displayColumns = data.AsEnumerable().Where(x =>
          x["IsDefault"].ToString().ToLower() == "true").CopyToDataTable();
            if (PageMode == "RiskReport" || PageMode == "TransactionHistory")
            {
                foreach (DataRow row in displayColumns.Rows) // remove default data for risk report page
                {
                    row.Delete();
                }
                displayColumns.AcceptChanges();
            }

            for (int i = 0; i < displayColumnList.Count; i++)
            {
                var dataColumn = data.AsEnumerable().Where(x => x.Field<string>("Key") == displayColumnList[i] &&
                      x["IsDefault"].ToString().ToLower() != "true").FirstOrDefault();
                if (dataColumn != null)
                {
                    displayColumns.Rows.Add(dataColumn.ItemArray);
                }
            }

            ViewName = displayData.Rows[0]["ViewName"].ToString();
            ViewType = displayData.Rows[0]["ViewType"].ToInt();
            txtOldNameCustomView.Value = ViewName;
            txtNameCustomView.Text = ViewName;
        }
        else if (PageMode != "RiskReport" && PageMode != "TransactionHistory")
        {
            displayColumns = data.AsEnumerable().Where(x => x["IsDefault"].ToString().ToLower() == "true").CopyToDataTable();
        }


        var listExtendColumn = RM_MCF_GeneralFuncsLib.ExtendCustomColumn();

        if (PageMode != "RiskReport" && PageMode != "TransactionHistory")
        {
            var dataExtend = data.AsEnumerable().Where(x => x["IsHide"].ToString().ToLower() == "true");
            if (listExtendColumn != null)
            {
                foreach (var itemEx in listExtendColumn)
                {
                    dataExtend = dataExtend.Where(x => !x["Key"].ToString().ToLower().Equals(itemEx.ToLower()));
                }
            }

            if (dataExtend.Count() > 0)
            {
                foreach (var item in dataExtend)
                {
                    displayColumns = displayColumns.AsEnumerable().Where(x => x["Key"].ToString().ToLower() != item["Key"].ToString().ToLower()).CopyToDataTable();
                }
            }
        }


        displayColumns.Columns.Add("ColumnText", typeof(string));

        foreach (DataRow item in displayColumns.Rows)
        {
            item["ColumnText"] = RM_MCF_GeneralFuncsLib.GetResourceValue(item["Key"].ToString() + "_CustomView_Text");
        }

        uxRightGrid.DataSource = displayColumns;
        uxRightGrid.DataBind();
    }
    /// <summary>
    /// Get Data Columns
    /// </summary>
    public void GetData()
    {
        GetUnusedColumn();
        GetDisplayedColumn();
    }
    public void RebindData()
    {
        uxRightGrid.DataBind();
    }
    private void AddCustomView()
    {
        var parameters = new FilterParameterCollection();

        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@ViewName", txtNameCustomView.Text.Trim(), DbType.String));
        parameters.Add(new FilterParameter("@ViewType", GetViewType(), DbType.Int32));
        parameters.Add(new FilterParameter("@ViewData", ViewData, DbType.String));
        if (PageMode == "TransactionHistory")
        {
            parameters.Add(new FilterParameter("@PageType", "RiskReport_TransactionHistory", DbType.String));
        }
        else if (PageMode != "RiskReport")
        {
            parameters.Add(new FilterParameter("@PageType", "Assignment", DbType.String));
        }
        WebServices.RiskServices.GetReports("spa_RM_MCF_AddCustomView", parameters);
    }
    private int GetViewType()
    {
        int viewType = (int)WebSiteEnums.ManageCustomView.Public;
        if (!rdPublicView.Checked)
        {
            viewType = (int)WebSiteEnums.ManageCustomView.Private;
        }
        return viewType;
    }
    private int CheckCustomViewName()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection outParameters = new FilterParameterCollection();

        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@ViewName", txtNameCustomView.Text.Trim(), DbType.String));
        parameters.Add(new FilterParameter("@ViewType", GetViewType(), DbType.Int32));
        if (PageMode == "TransactionHistory")
        {
            parameters.Add(new FilterParameter("@PageType", "RiskReport_TransactionHistory", DbType.String));
        }
        else if (PageMode != "RiskReport")
        {
            parameters.Add(new FilterParameter("@PageType", "Assignment", DbType.String));
        }
        if (!IsCreate)
        {
            parameters.Add(new FilterParameter("@CustomViewID", CustomViewID.ToInt(), DbType.Int32));
        }
        DataTable result = WebServices.RiskServices.GetReports("spa_RM_MCF_CheckCustomViewName", parameters);
        return result.Rows[0].Field<int>(0);
    }
    private DataTable GetCustomDisplayedColumns()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@CustomViewID", CustomViewID.ToInt(), DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_Get_CustomView", parameters);
    }

    #endregion ---- Private Methods ----
}
