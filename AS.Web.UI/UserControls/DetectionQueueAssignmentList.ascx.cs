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
using AS.Utilities;
using Telerik.Web.UI;
using AS.Controls.Exporter;
using AS.Common;
using AS.Common.DBManager;
using System.Drawing;
using AS.Controls.Pages;

public partial class UserControls_DetectionQueueAssignmentList : GlobalUserControl
{
    enum DataBindAction
    {
        BindAssignmentList
    }

    enum PostBackAction
    {
        BindAssignmentEvent,
        ViewAssignmentCommand
    }



    public event GridNeedDataSourceEventHandler NeedDataSource;
    public event GridItemEventHandler ItemDataBound;

    private const string ASSIGNMENT_ID = "AssignmentID";
    private const string ASSIGNMENT_NAME = "AssignmentName";
    private const string ASSIGNMENT_TYPE = "AssignmentType";
    private const string FEATURE_MODE = "FeatureMode";
    private const string REVIEW_MODE = "ReviewMode";
    private const string DETECTION_QUEUE_MODE = "DetectionQueueMode";

    protected virtual void OnNeedDataSource(GridNeedDataSourceEventArgs e)
    {
        NeedDataSource.Fire(d => (d as GridNeedDataSourceEventHandler)(this, e));
    }

    protected virtual void OnItemDataBound(GridItemEventArgs e)
    {
        ItemDataBound.Fire(d => (d as GridItemEventHandler)(this, e));
    }
    private bool _showExport = false;
    public bool ShowExport
    {
        set
        {
            this._showExport = value;
        }
        get
        {
            return this._showExport;
        }
    }

    public GridTableView MasterTableView
    {
        get
        {
            return this.uxAssignmentList.MasterTableView;
        }
    }

    public object DataSource
    {
        get { return this.uxAssignmentList.DataSource; }
        set { this.uxAssignmentList.DataSource = value; }
    }

    public WebSiteEnums.AssignmentType _AssignmentType
    {
        get
        {
            if (RiskSessionManager.DetectionQueue != null)
            {
                return (WebSiteEnums.AssignmentType)RiskSessionManager.DetectionQueue.AssignmentType;
            }
            return WebSiteEnums.AssignmentType.DetectionQueue;
        }
    }

    public bool HasQueuingMechanism
    {
        get
        {
            return GeneralFuncsLib.HasQueuingMechanismFeature;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SwitchViewFollowAssignmentType();
        }
    }

    public void Rebind()
    {
        SwitchViewFollowAssignmentType();
        uxAssignmentList.Rebind();
    }

    protected override void OnPostBackActions(Enum type, object param)
    {
        if (Page.IsIntruderDetected) return;

        switch ((PostBackAction)type)
        {
            case PostBackAction.BindAssignmentEvent:
                {
                    uxAssignmentList.Rebind();
                }
                break;
            case PostBackAction.ViewAssignmentCommand:
                {
                    CommandEventArgs e = (CommandEventArgs)param;
                    int AssignmentID = MoveDataFromFinalTables(int.Parse(e.CommandArgument.ToString()));

                    string queryString = this.Page.BuildSecureQueryString(string.Format("{0}={1}&{2}={3}&{4}={5}{6}",
                       ASSIGNMENT_ID, AssignmentID, FEATURE_MODE, (int)WebSiteEnums.FeatureMode.View, REVIEW_MODE, 1, this.AssignmentIntruderQuery));
                    if (RiskSessionManager.IsUsingMarketData)
                        ((ReportPage)Page).AjaxAddResponseScript("ShowPopupModal('rm_Assignment_CreateNew_MarketData.aspx?" + queryString + "','auto');");
                    else
                        ((ReportPage)Page).AjaxAddResponseScript("ShowPopupModal('rm_Assignment_CreateNew.aspx?" + queryString + "','auto');");
                }
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected) return;

        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindAssignmentList:
                {
                    DataTable table = GetData();

                    uxAssignmentList.DataSource = table;

                    this.uxAssignmentList.ClientSettings.Scrolling.ScrollBarWidth = System.Web.UI.WebControls.Unit.Pixel(4000);
                    if (table.Rows.Count > 0 && table.Rows.Count > 5)
                        this.uxAssignmentList.ClientSettings.Scrolling.ScrollHeight = System.Web.UI.WebControls.Unit.Pixel(180);
                    else if (table.Rows.Count == 0)
                        this.uxAssignmentList.ClientSettings.Scrolling.ScrollHeight = System.Web.UI.WebControls.Unit.Pixel(30);
                    else
                    {
                        this.uxAssignmentList.ClientSettings.Scrolling.ScrollHeight =
                                System.Web.UI.WebControls.Unit.Pixel(30 * table.Rows.Count);
                    }
                }
                break;
        }
    }

    protected void lnkViewAssignment_Command(object sender, CommandEventArgs e)
    {
        Rebind();
        OnPostBackActions(PostBackAction.ViewAssignmentCommand, e);
    }

    // return the temporary ID on staging tables
    public int MoveDataFromFinalTables(int AssignmentID)
    {
        FilterParameterCollection paramsIn = new FilterParameterCollection();
        paramsIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramsIn.Add(new FilterParameter("@AssignmentID", AssignmentID, DbType.Int32));
        paramsIn.Add(new FilterParameter("@TemporaryAssignmentID", 0, DbType.Int32, true));

        FilterParameterCollection paramsOut = new FilterParameterCollection();

        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_MoveAssignmentToStagingTables", paramsIn, out paramsOut);

        int TemporaryID = Int32.Parse(paramsOut[0].ParameterValue.ToString());

        return TemporaryID;
    }

    //display grid follows assignment type
    private void SwitchViewFollowAssignmentType()
    {
        //if (this.Visible)
        //{
        uxAssignmentList.Visible = true;
        if (HasQueuingMechanism)
        {
            if (_AssignmentType == WebSiteEnums.AssignmentType.DetectionQueueDistinct)
            {
                uxAssignmentList.Visible = false;
            }
            else if (_AssignmentType == WebSiteEnums.AssignmentType.WorkQueue)
            {
                uxAssignmentList.Columns.FindByUniqueName("RequeueCount").Visible = false;
                uxAssignmentList.Columns.FindByUniqueName("RequeueVolume").Visible = false;
            }
        }
        else
        {
            uxAssignmentList.Columns.FindByUniqueName("AssignmentType").Visible =
            uxAssignmentList.Columns.FindByUniqueName("RequeueCount").Visible =
            uxAssignmentList.Columns.FindByUniqueName("RequeueVolume").Visible = false;
        }
        //}
    }


    protected void uxAssignmentList_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindAssignmentList);
        //fire event
        this.OnNeedDataSource(e);
    }

    protected void uxAssignmentList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            var assignmentID = dataRow[ASSIGNMENT_ID].ToString().Trim();
            if (int.Parse(assignmentID) > 0)
            {
                if (dataItem["TotalMerchantVolume"].Text.Contains("("))
                    dataItem["TotalMerchantVolume"].ForeColor = Color.Red;

                if (dataItem["WorkedVolume"].Text.Contains("("))
                    dataItem["WorkedVolume"].ForeColor = Color.Red;
                dataItem[ASSIGNMENT_TYPE].Text = GeneralFuncsLib.BuildAssignmentTypeAbbr(dataRow[ASSIGNMENT_TYPE]);

                if (dataRow[ASSIGNMENT_TYPE].ToInt() == (int)WebSiteEnums.AssignmentType.WorkQueue)
                    dataItem["TotalMerchantCount"].Text = "N/A";

                //RiskSessionManager.DetectionQueueTemporaryRequeueInfo.TotalAlertMerchant = dataRow["AlertMerchantCount"].ToInt();
                //RiskSessionManager.DetectionQueueTemporaryRequeueInfo.TotalVolume = decimal.Parse(dataRow["TotalMerchantVolume"].ToString());
            }
        }


    }

    private DataTable GetData()
    {
        DataTable assignmentList = new DataTable();
        if (RiskSessionManager.DetectionQueue != null && RiskSessionManager.DetectionQueue.AssignmentID > 0)
        {
            FilterParameterCollection parameterList = new FilterParameterCollection();
            parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameterList.Add(new FilterParameter("@AssignmentID", RiskSessionManager.DetectionQueue.AssignmentID, DbType.Int32));
            parameterList.Add(new FilterParameter("@ReportDate", RiskSessionManager.DetectionQueue.ReportDate, DbType.Date));
            parameterList.Add(new FilterParameter("@FilterType", "ALL", DbType.AnsiString));
            parameterList.Add(new FilterParameter("@FilterValue", string.Empty, DbType.AnsiString));
            string spName = "spa_rm_cs_GetAssignmentsForDetectionQueue";
            assignmentList = WebServices.RiskServices.GetReports(spName, parameterList);
        }
        return assignmentList;
    }


    #region Export Data

    private DataTable _allData
    {
        get
        {
            return this.Session["_allData"] != null ?
                (DataTable)this.Session["_allData"] : null;
        }
        set
        {
            this.Session["_allData"] = value;
        }
    }

    protected void UxExport_ExportConfigCreated(ExportConfig ec)
    {
        ec.ReportHeader = "Assignments";
        ec.TableSource = _allData ?? GetData();
    }

    protected void UxExport_ExportCoreCreated(ExportCore exportcore)
    {
        exportcore.ExportHeaderLine += (sender, headers) =>
        {
            exportcore.ExpColumnHeaders = this.GetLocalResourceObject("DetectionQueueAssignmentListCS_Text_ExpColHeader").ToString();
        };
    }

    protected void UxExport_DataSourceFilled(DataTable table)
    {

    }

    #endregion

    string _AssignmentIntruderQuery = string.Empty;
    private string AssignmentIntruderQuery
    {
        get
        {
            if (_AssignmentIntruderQuery.Length == 0)
                _AssignmentIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.ID, new string[] { ASSIGNMENT_ID });
            return _AssignmentIntruderQuery;
        }
    }
}
