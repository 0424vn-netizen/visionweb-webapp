using AS.Common.DBManager;
using AS.Common;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AddOrganizationModal : NonReportPage
{
    #region Properties
    private DataTable _SelectedOrganization
    {
        get
        {
            if (ViewState["_SelectedOrganization"] == null) ViewState["_SelectedOrganization"] = new DataTable();
            return (DataTable)ViewState["_SelectedOrganization"];
        }
        set
        {
            ViewState["_SelectedOrganization"] = value;
        }
    }

    private DataTable _AvailableOrganization
    {
        get
        {
            if (ViewState["_AvailableOrganization"] == null)
                ViewState["_AvailableOrganization"] = GeneralFuncsLib.GetOrganizations(WebSiteEnums.OrganizationMode.ALL.ToString()); 
            return (DataTable)ViewState["_AvailableOrganization"];
        }
        
    }

    private string _FilterOrganizationIDs
    {
        get
        {
            if (ViewState["_FilterOrganizationIDs"] == null) ViewState["_FilterOrganizationIDs"] = string.Empty;
            return ViewState["_FilterOrganizationIDs"].ToString();
        }
        set
        {
            ViewState["_FilterOrganizationIDs"] = value;
        }
    }

    private string _recId
    {
        get
        {
            if (SecureQueryString["recId"] != null) return SecureQueryString["recId"].ToString();
            return string.Empty;
        }
    }

    private string _CurrentFilter
    {
        get
        {
            if (ViewState["_CurrentFilter"] == null) ViewState["_CurrentFilter"] = string.Empty;
            return ViewState["_CurrentFilter"].ToString();
        }
        set
        {
            ViewState["_CurrentFilter"] = value;
        }
    }
    private string _CurrentFilterValue
    {
        get
        {
            if (ViewState["_CurrentFilterValue"] == null) ViewState["_CurrentFilterValue"] = string.Empty;
            return ViewState["_CurrentFilterValue"].ToString();
        }
        set
        {
            ViewState["_CurrentFilterValue"] = value;
        }
    }
    #endregion

    private bool _selectAll = false;
    private bool isFilter = false;
    private bool isSelect = false;

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(SessionManager.Organizations))
            {
                DataTable dtSelectedOrganization = GeneralFuncsLib.GetOrganizations(WebSiteEnums.OrganizationMode.ALL.ToString());
                DataRow[] drArr = dtSelectedOrganization.Select(string.Format("OrganizationID in ({0})", SessionManager.Organizations));
                if (drArr.Length > 0) _SelectedOrganization = drArr.CopyToDataTable();
            }
        }
        isFilter = false;
        isSelect = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SessionManager.Organizations = GetSelectedOrganization(_SelectedOrganization);
        //43401 - Error when creating multiple users on the FE
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("onSaveOrgSuccess();");
    }

    protected void uxAvailableOrganization_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dtAvailable = _AvailableOrganization;
        if (isSelect || _selectAll)
        {
            uxAvailableOrganization.Columns.FindByUniqueName("OrganizationName").CurrentFilterValue = _CurrentFilterValue;
            uxAvailableOrganization.MasterTableView.FilterExpression = _CurrentFilter;
        }
        if (isFilter)
        {
            _CurrentFilterValue = uxAvailableOrganization.Columns.FindByUniqueName("OrganizationName").CurrentFilterValue;
            _CurrentFilter = uxAvailableOrganization.MasterTableView.FilterExpression;
            if (string.IsNullOrEmpty(_CurrentFilterValue)) _FilterOrganizationIDs = string.Empty;
            dtAvailable = DoFilterTable(dtAvailable);
        }

        _FilterOrganizationIDs = string.Empty;
        
        string selectedOrganizationIDs = GetSelectedOrganization(_SelectedOrganization);
        if (selectedOrganizationIDs.Equals(string.Empty))
            uxAvailableOrganization.DataSource = dtAvailable;
        else
            uxAvailableOrganization.DataSource = dtAvailable.Select(string.Format("OrganizationID not in ({0})", selectedOrganizationIDs));
        _selectAll = false;
        if (!_SelectedOrganization.HasData()) _SelectedOrganization = dtAvailable.Clone();
    }

    protected void uxOrganizationSelected_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        uxOrganizationSelected.DataSource = _SelectedOrganization;
        uxOrganizationSelected.Visible = _SelectedOrganization.HasData();
        lbSelectedOrg.Text = _SelectedOrganization.Rows.Count.ToString();
    }

    protected void uxSelect_Command(object sender, CommandEventArgs e)
    {
        DataRow[] row = _AvailableOrganization.Select(string.Format("OrganizationID = {0}", e.CommandArgument));

        if (row.Length > 0) {
           
            _SelectedOrganization.Rows.Add(row[0].ItemArray);
            isSelect = true;
            uxOrganizationSelected.Rebind();
            uxAvailableOrganization.Rebind();
        }
    }

    protected void uxRemove_Command(object sender, CommandEventArgs e)
    {
        DataRow[] drArr = _SelectedOrganization.Select(string.Format("OrganizationID = {0}", e.CommandArgument));
        if (drArr.Length > 0)
        {
            _SelectedOrganization.Rows.Remove(drArr[0]);
            isSelect = true;
            uxOrganizationSelected.Rebind();
            uxAvailableOrganization.Rebind();
        }
    }

    protected void uxSelectAll_Click(object sender, EventArgs e)
    {
        _selectAll = true;
        if (string.IsNullOrEmpty(_FilterOrganizationIDs))
        {
            _SelectedOrganization = _AvailableOrganization;
        }
        else
        {
            DataRow[] drFilterArr = _AvailableOrganization.Select(string.Format("OrganizationID in ({0})", _FilterOrganizationIDs.TrimEnd(',')));
            foreach (DataRow dr in drFilterArr)
            {
                _SelectedOrganization.Rows.Add(dr.ItemArray);
            }
        }
        uxOrganizationSelected.Rebind();
        uxAvailableOrganization.Rebind();
    }

    protected void uxClearAll_Click(object sender, EventArgs e)
    {
        isSelect = true;
        _SelectedOrganization.Rows.Clear();
        uxOrganizationSelected.Rebind();
        uxAvailableOrganization.Rebind();
    }

    protected void uxAvailableOrganization_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Filter")
        {
            isFilter = true;
        }
    }

    protected void uxAvailableOrganization_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem && !string.IsNullOrEmpty(_CurrentFilterValue))
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            _FilterOrganizationIDs += dataRow["OrganizationID"].ToString() + ",";
        }
    }
    #endregion

    #region Methods
    private string GetSelectedOrganization(DataTable dtSelected)
    {
        string organizationIDs = string.Empty;
        foreach (DataRow dr in dtSelected.Rows)
        {
            organizationIDs += dr["OrganizationID"] + ",";
        }
        organizationIDs = organizationIDs.TrimEnd(',');
        return organizationIDs;
    }

    private DataTable DoFilterTable(DataTable dt)
    {
        if (!string.IsNullOrEmpty(uxAvailableOrganization.AS_FilterExpression))
        {
            DataTable dtFiltered = new DataTable();
            foreach (DataColumn c in dt.Columns)
            {
                dtFiltered.Columns.Add(c.ColumnName, c.DataType);
            }
            DataRow[] filtered = dt.Select(uxAvailableOrganization.AS_FilterExpression);
            foreach (DataRow dr in filtered)
            {
                dtFiltered.ImportRow(dr);
            }
            return dtFiltered;
        }
        return dt;
    }
    #endregion
}