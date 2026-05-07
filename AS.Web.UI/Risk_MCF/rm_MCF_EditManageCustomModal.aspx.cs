using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using Newtonsoft.Json;

using System.Linq;

public partial class rm_MCF_EditManageCustomModal : NonReportPage
{
    public string IsSelected
    {
        get
        {
            if (SecureQueryString["IsSelected"] != null)
            {
                return SecureQueryString["IsSelected"];
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public int ViewType { get; set; }

    private string MerchantNumber { get; set; }

    private string _customViewID
    {
        get
        {
            if (SecureQueryString["CustomViewID"] != null) return SecureQueryString["CustomViewID"].ToString();
            return string.Empty;
        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            GetData();
        }
    }


    public void GetData()
    {

        GetDisplayedColumn();
    }

    private void GetDisplayedColumn()
    {
        var displayColumns = GetCustomDisplayedColumns();
        var data = RM_MCF_GeneralFuncsLib.GetAssignmentCustomizeColumnsToXML();
        var displayColumnList = new List<string>();
        var dataTemp = new DataTable();
        List<ColumnDisplayedConfigurationItem> items = new List<ColumnDisplayedConfigurationItem>();

        if (displayColumns.Rows.Count > 0)
        {
            displayColumnList = displayColumns.Rows[0]["ViewData"].ToString().Split(',').ToList();
        }
        dataTemp = data.AsEnumerable().Where(x => x["IsDefault"].ToString().ToLower() == "true").CopyToDataTable();

        for (int i = 0; i < displayColumnList.Count; i++)
        {
            var dataColumn = data.AsEnumerable().Where(x => x.Field<string>("Key") == displayColumnList[i] &&
                  x["IsDefault"].ToString().ToLower() != "true").FirstOrDefault();
            if (dataColumn != null)
            {
                dataTemp.Rows.Add(dataColumn.ItemArray);
            }
        }

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
                dataTemp = dataTemp.AsEnumerable().Where(x => x["Key"].ToString().ToLower() != item["Key"].ToString().ToLower()).CopyToDataTable();
            }
        }

        if (dataTemp.Rows.Count > 0)
        {
            txtViewName.Value = displayColumns.Rows[0]["ViewName"].ToString();
            ViewType = displayColumns.Rows[0]["ViewType"].ToInt();
            displayColumns = dataTemp;
        }

        displayColumns.Columns.Add("ColumnText", typeof(string));
        displayColumns.Columns.Add("ColumnToolTip", typeof(string));

        foreach (DataRow item in displayColumns.Rows)
        {
            item["ColumnText"] = RM_MCF_GeneralFuncsLib.GetResourceValue(item["Key"].ToString() + "_CustomView_Text");
            item["ColumnToolTip"] = RM_MCF_GeneralFuncsLib.GetResourceValue(item["Key"].ToString() + "_Tooltip");
        }
        uxRightGrid.DataSource = displayColumns;
        uxRightGrid.DataBind();

    }

    public void RebindData()
    {
        uxRightGrid.DataBind();
    }

    private DataTable GetCustomDisplayedColumns()
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@CustomViewID", _customViewID, DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_Get_CustomView", parameters);
    }

    private List<string> GenerateTransactionVolumeAnalysis()
    {
        DataTable tb = new DataTable();

        tb = GetCustomDisplayedColumns();
        if (tb.Rows.Count > 0)
        {
            string listColumns = tb.Rows[0]["ViewData"].ToString();
            txtViewName.Value = tb.Rows[0]["ViewName"].ToString();
            ViewType = tb.Rows[0]["ViewType"].ToInt();
            string[] columns = listColumns.Split(',');
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = columns[i].Trim();
            }
            return columns.ToList<string>();
        }
        else return new List<string>();
    }


    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        var displayedItems = uxRightGrid.Items.Cast<RadListBoxItem>().Select(x => x.Value).ToList();
        string viewData = string.Join(",", displayedItems);

        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        parameters.Add(new FilterParameter("@CustomViewID", _customViewID, DbType.Int32));
        parameters.Add(new FilterParameter("@ViewName", txtViewName.Value, DbType.String));
        parameters.Add(new FilterParameter("@ViewType", ViewType, DbType.Int32));
        parameters.Add(new FilterParameter("@ViewData", viewData, DbType.String));
        parameters.Add(new FilterParameter("@PageType", "Assignment", DbType.String));
        WebServices.RiskServices.GetReports("spa_RM_MCF_UpdateCustomView", parameters);

        //if (IsSelected == "true")
        //{
        //    ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"window.EditColumnModal.changeCustomView();"));
        //}
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"window.EditColumnModal.CloseEditCustomViewModal();"));
    }

    protected void uxRightGrid_ItemDataBound(object sender, RadListBoxItemEventArgs e)
    {
        DataRowView dataSourceRow = (DataRowView)e.Item.DataItem;

        AS.Controls.Global.RadToolTip uxRadToolTipUnused = new AS.Controls.Global.RadToolTip();
        uxRadToolTipUnused.TargetControlID = dataSourceRow.Row["Key"].ToString();
        uxRadToolTipUnused.Text = "<span style='color:#333333;'>" + RM_MCF_GeneralFuncsLib.GetResourceValue(dataSourceRow.Row["Key"].ToString() + "_Tooltip") + "</span>";
        uxRadToolTipUnused.RenderMode = RenderMode.Lightweight;
        uxRadToolTipUnused.CssClass = "customview-tooltip";
        uxRadToolTipUnused.RelativeTo = ToolTipRelativeDisplay.Element;
        uxRadToolTipUnused.Position = ToolTipPosition.BottomCenter;
        uxRadToolTipUnused.IsClientID = true;
        uxRadToolTipUnused.AutoCloseDelay = 0;
        uxRadToolTipUnused.HideDelay = 0;
        e.Item.Controls.Add(uxRadToolTipUnused);
        e.Item.Attributes["IsDefault"] = (dataSourceRow.Row["IsDefault"].ToString().ToLower() == "true") ? "item-customview-isdefault disable-item-customview" : string.Empty;
        e.Item.Attributes["IsNotCustomizable"] = (dataSourceRow.Row["IsDefault"].ToString().ToLower() == "true") ? GetLocalResourceObject("txtNotCustomizable").ToString() : string.Empty;
        e.Item.Attributes["IdTooltip"] = uxRadToolTipUnused.ClientID.ToString();

    }
}