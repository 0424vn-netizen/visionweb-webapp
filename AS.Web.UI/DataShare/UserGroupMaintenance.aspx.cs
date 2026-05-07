using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Telerik.Web.UI;
using AS.Common;
using AS.Controls.Exporter;
using AS.Controls.Pages;
using AS.Common.DBManager;
using Resources;
using System.Text.RegularExpressions;
using System.Web;
using AS.Web.Business;
using AS.Controls.Grid;

[PagePermission("UserGroupMaint,MSUserGroupMaint")]
public partial class UserGroupMaintenance : ReportPage
{
    #region Enum

    enum DataBindAction
    {
        BindDataGroups
    }

    enum PostBackAction
    {
        ChangeFilter
    }

    #endregion

    private int FilterStatus
    {
        get
        {
            int filterStatusValue = -1;

            foreach (Control ctl in uxFilterStatusContainer.Controls)
            {
                if (ctl is RadioButton)
                {
                    RadioButton radioButton = ctl as RadioButton;

                    if (radioButton.Checked)
                    {
                        if (Int32.TryParse(radioButton.Attributes["xValue"], out filterStatusValue))
                        {
                            return filterStatusValue;
                        }
                    }
                }
            }

            return filterStatusValue;
        }
        set
        {
            int filterStatusValue = -1;

            foreach (Control ctl in uxFilterStatusContainer.Controls)
            {
                if (ctl is RadioButton)
                {
                    RadioButton radioButton = ctl as RadioButton;

                    if (Int32.TryParse(radioButton.Attributes["xValue"], out filterStatusValue))
                    {
                        if (filterStatusValue == value)
                            radioButton.Checked = true;
                        else
                            radioButton.Checked = false;
                    }
                }
            }

            if (this.FilterStatus < 0)
                uxFilterStatusAll.Checked = true;
        }
    }

    #region Methods

    #region Protected Methods

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxUserGroupList");
        this.ExporterIDs.Add("uxExportTop");
        base.PageInitialize();
        IsBindDataOnLoad = true;
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (sender == uxUserGroupList)
        {
            OnDataBindControls(DataBindAction.BindDataGroups, sender);
        }
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            var rowData = e.Item.DataItem as DataRowView;
            int groupID = int.Parse(rowData["UserGroupID"].ToString());
            var editLink = dataItem["Edit"];
            var memberCountItem = dataItem["Members"];

            string queryString = BuildSecureQueryString(string.Format("UserGroupID={0}", groupID));
            memberCountItem.Text = VeraCodeSolution.DoVeraCode(string.Format(
                "<a href=\"#\" onclick=\"ShowPopupModal('MemberListDataShare.aspx?{0}', 'auto'); return false;\">{1}</a>",
                queryString,
                rowData["Members"]));

            editLink.Text = VeraCodeSolution.DoVeraCode(string.Format(
                "<a href=\"#\" onclick=\"ShowPopupModal('AddEditUserGroup.aspx?{0}', 'auto'); return false;\">{1}</a>",
                queryString,
                GetLocalResourceObject("LinkButton1Resource1.Text").ToString()));
        }
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        uxUserGroupList.Columns.FindByUniqueName("Edit").Visible = false;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDataGroups:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.AddLanguageID();
                    parameters.Add(new FilterParameter("@Status", FilterStatus, DbType.Int32));
                    uxUserGroupList.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_cs_DataShare_GetUserGroupList", ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.ChangeFilter:
                {
                    uxUserGroupList.Rebind();
                }
                break;
        }
    }

    protected void uxChangeFilterStatus_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeFilter);
    }

    #endregion Protected Methods
    
    #endregion Methods
}
