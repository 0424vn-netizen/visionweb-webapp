using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

[PagePermission("ManageDocumentTypes,ManageDocumentTypesAlice,ManageDocumentTypesCMS,MSManageDocumentTypesCMS,SponsorManageDocumentTypes")]
public partial class ManageDocumentTypes : ReportPage
{
    #region enum

    enum DataBindAction
    {
        BindAliceDocumentType,
        BindCMDocumentType,
    }
    #endregion
    #region events

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExportAliceDocumebntGrid"); 
        this.ExporterIDs.Add("uxExportuxCMDocumentTypeGrid");
        this.IsBindDataOnLoad = true;
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsUserWithPermission("ManageDocumentTypesAlice") && CheckCMSPermission())
        {
            uxSourceFilterContainer.Visible = true;
        }
        else
        {
            uxAliceCMDocumebntGrid.Columns.FindByUniqueName("SourceName").Visible = false;
            uxAliceCMDocumebntGrid.MasterTableView.NoMasterRecordsText = GetLocalResourceObject("NoResultsFound.Text").ToString();
            uxSourceFilterContainer.Visible = false;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {

    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParamsWithRecId();
        parameters.AddLanguageID();
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindAliceDocumentType:
                int sourceID = 2;
                if (IsUserWithPermission("ManageDocumentTypesAlice") 
                    && CheckCMSPermission())
                {
                    if (uxSourceAlice.Checked)
                        sourceID = 1;
                    if (uxSourceCM.Checked)
                        sourceID = 2;
                    if (uxSourceAll.Checked)
                        sourceID = 0;
                }
                else if (IsUserWithPermission("ManageDocumentTypesAlice")
                        && !CheckCMSPermission())
                {
                    sourceID = 1;
                }
                else
                {
                    sourceID = 2;
                }
                parameters.Add("@SourceID", sourceID, DbType.Int32);
                uxAliceCMDocumebntGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_MDT_Get_ManageDocumentTypeList", ReportServices.ConvertToFilterParamWSArray(parameters) });
                break;
        }
    }
    #endregion
    protected void uxExportAliceDocumebntGrid_NeedExportConfig(object sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        if (IsUserWithPermission("ManageDocumentTypesAlice") && CheckCMSPermission())
        {
            uxSourceFilterContainer.Visible = true;
        }
        else
        {
            uxAliceCMDocumebntGrid.Columns.FindByUniqueName("SourceName").Visible = false;
        }
        uxAliceCMDocumebntGrid.Columns.FindByUniqueName("Edit").Visible = false;
    }

    protected void uxAliceDocumebntGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindAliceDocumentType);

    }
    protected void uxAliceDocumebntGrid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            var lnkEdit = e.Item.FindControl("lnkEdit") as LinkButton;
            string url = ResolveUrl("CreateEditManageDocumentTypes.aspx?") + BuildSecureQueryString(string.Format("Source={0}&DocumentTypeID={1}&SourceName={2}", dataRow["SourceID"].ToString(), dataRow["DocumentTypeID"].ToString(), dataRow["SourceName"].ToString()));
            lnkEdit.OnClientClick = string.Format("ShowPopupModal('{0}','auto'); return false;", url);
        }
    }

    protected void uxReloadGrid_Click(object sender, EventArgs e)
    {
        uxAliceCMDocumebntGrid.Rebind();
    }
    protected void uxSourceAll_CheckedChanged(object sender, EventArgs e)
    {
        uxAliceCMDocumebntGrid.MasterTableView.CurrentPageIndex = 0;
        uxAliceCMDocumebntGrid.Rebind();

    }

    private bool CheckCMSPermission()
    {
        return IsUserWithPermission("ManageDocumentTypesCMS") || IsUserWithPermission("MSManageDocumentTypesCMS");
    }
}