using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Common.DBManager;
using Telerik.Web.UI;
using AS.Common;
using System.Drawing;
using AS.Controls.Pages;

[PagePermission("RskManAss,MSRskManAss")]
public partial class rm_MCF_AssignmentProcessingStatus : ReportPage
{
    #region Enums
    enum DataBindAction
    {
    }
    enum PostBackAction
    {
        Submit,
        SubmitAjax
    }
    #endregion

    #region Constants

    private const string ASSIGNMENT_TYPE_COL = "AssignmentType";

    #endregion Constants

    #region Properties

    public bool HasQueuingMechanism
    {
        get
        {
            return GeneralFuncsLib.HasQueuingMechanismFeature;
        }
    }

    #endregion Properties

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxGrid");
        IsBindDataOnLoad = true;
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            DisplayLastJobQueueInfo();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (this.IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.Submit:
                ViewState["statusList"] = null;
                DisplayLastJobQueueInfo();
                uxGrid.Rebind();
                break;
            case PostBackAction.SubmitAjax:
                DisplayLastJobQueueInfo();
                uxGrid.Rebind();
                break;
        }
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (sender == uxGrid && uxGrid.Visible)
        {
            string statusList = string.Empty;
            if (ViewState["statusList"] == null)
            {
                //foreach (ListItem item in uxStatusList.Items)
                //{
                //    if (item.Selected)
                //        statusList += string.Format("{0},", item.Value);
                //}
                if (chbxQueued.Checked) statusList += string.Format("{0},", 0);
                if (chbxInProgress.Checked) statusList += string.Format("{0},", 1);
                if (chbxCompleted.Checked) statusList += string.Format("{0},", 3);
                if (chbxCancelled.Checked) statusList += string.Format("{0},", 2);
                if (chbxError.Checked) statusList += string.Format("{0},", 4);

                ViewState["statusList"] = statusList = statusList.TrimEnd(',');

            }
            else
            {
                statusList = (string)ViewState["statusList"];
            }
            if (statusList != string.Empty)
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.AddLanguageID();
                parameters.Add(new FilterParameter("@StatusList", statusList, DbType.String));
                uxGrid.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAssignmentProcStatusList", parameters);
            }
            else
            {
                DataTable dt = new DataTable();
                uxGrid.DataSource = dt;
            }
            if ((uxGrid.DataSource as DataTable).Rows.Count > 0)
            {
                uxGrid.AllowSorting = true;
            }
            else
            {
                uxGrid.AllowSorting = false;
                //uxLastJobQueueDate.Text = VeraCodeSolution.DoVeraCode("N/A");
            }
            SetVisibleForRequeuedColumns();
        }
    }
    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        if (sender == uxGrid && uxGrid.Visible)
        {
            if (e.Item is GridDataItem)
            {
                var dataItem = e.Item as GridDataItem;
                var rowData = e.Item.DataItem as DataRowView;
                if (GeneralFuncsLib.NvlString(rowData["ProcessingStatusCode"]) == "4")
                {
                    dataItem["ProcessingStatus"].ForeColor = Color.Red;
                    if (string.Compare(SessionManager.CurrentUser.UserSecRole, "FDUSER") == 0)
                        dataItem["ProcessingStatus"].ToolTip = GeneralFuncsLib.NvlString(rowData["ProcessingStatusNotes"]);
                }
                // Format AssignmentType
                dataItem[ASSIGNMENT_TYPE_COL].Text =
                    GeneralFuncsLib.BuildAssignmentTypeAbbr(rowData[ASSIGNMENT_TYPE_COL]);
            }
        }
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Submit, sender);
    }
    protected void uxSubmitAjax_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.SubmitAjax, sender);
    }

    private void DisplayLastJobQueueInfo()
    {
        //get generic job info
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@JobType", "ASSIGNMENT_PROCESSING", DbType.String));
        parameters.Add(new FilterParameter("@JobID", -1, DbType.Int32));
        parameters.Add(new FilterParameter("@ReferenceID", "", DbType.String));
        DataTable data = WebServices.RiskServices.GetReports("spa_RM_MCF_GetJobList", parameters);

        if (data != null && data.Rows.Count > 0)
        {
            uxLastJobQueueDate.Text = string.Format("{0:MM/dd/yyyy hh:mm:ss tt}", data.Rows[0]["LastStatusDate"]);
        }
    }
    private void SetVisibleForRequeuedColumns()
    {
        // Visibility of column are based on HasQueuingMechanism: Assignment Type
        uxGrid.Columns.FindByUniqueName(ASSIGNMENT_TYPE_COL).Visible = HasQueuingMechanism;
    }
}
