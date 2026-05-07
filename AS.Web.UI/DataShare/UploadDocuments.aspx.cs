using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

[PagePermission("UploadDocuments,MSUploadDocuments")]
public partial class UploadDocuments : ReportPage
{
    enum DataBindAction
    {
        BindGrid
    }

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

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxReportGrid");
        this.ExporterIDs.Add("uxExportTop");
        base.PageInitialize();
        this.IsBindDataOnLoad = true;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGrid:
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams(false);
                parameters.Add(new FilterParameter("@Status", FilterStatus, DbType.String));

                uxReportGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_cs_DataShare_GetListUploadDocuments", ReportServices.ConvertToFilterParamWSArray(parameters) });
                break;
        }
    }

    protected void uxReportGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindGrid);
    }

    protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            var docId = dataRow["DocId"].ToString();
            var documentName = dataRow["DocumentName"].ToString();
            DateTime expirationDate = DateTime.MinValue;

            if (dataRow["ExpirationDate"] != DBNull.Value)
            {
                expirationDate = (DateTime)dataRow["ExpirationDate"];
            }

            var editBtn = (LinkButton)dataItem["EditOrDelete"].FindControl("uxEdit");

            string queryString = BuildSecureQueryString("docId=" + docId + "&DocumentName=" + documentName + "&IsUpdateMode=true");
            string url = "UploadDocumentsModal.aspx?" + queryString;

            if (expirationDate.Date < DateTime.Now.Date)
            {
                editBtn.Enabled = false;
                editBtn.Style.Add("opacity", "0.3");
            }
            else
            {
                editBtn.OnClientClick = string.Format("ShowPopupModal('{0}','auto'); return false;", url);
            }
        }
    }

    protected void uxChangeFilterStatus_Click(object sender, EventArgs e)
    {
        uxReportGrid.Rebind();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int docId = int.Parse(hdDeleteDocument.Value);

        if (docId != 0)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            FilterParameterCollection paramOuts = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams(false);
            parameters.Add(new FilterParameter("@DocID", docId, DbType.Int32));

            WebServices.CsReportServices.ExecuteNonQueryCommand("spa_cs_DataShare_DeleteDocument", parameters, out paramOuts);

            WebServices.DocServices.DeleteFileOnDocServer(SessionManager.CurrentClient.ToString(), SessionManager.CurrentUser.UserID, docId.ToString());

            uxReportGrid.Rebind();
        }
    }
}
