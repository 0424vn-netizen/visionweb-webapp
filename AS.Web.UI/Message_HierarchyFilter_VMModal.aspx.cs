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
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Controls.Pages;

[PagePermission("SendMsg,MSSendMsg,MSViewMsg")]
public partial class Message_HierarchyFilter_VMModal : NonReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindStatus,
        BindHierarchyFilterValue
    }
    #endregion

    public string HierarchyFilterText { get; set; }

    public string HierarchyFilterMode { get; set; }
    public int MessageID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;


        MessageID = Convert.ToInt32(SecureQueryString["MessageID"]);
        HierarchyFilterMode = SecureQueryString["hierarchyMode"];
        HierarchyFilterText = GeneralFuncsLib.GetMessageHierarchyFilterText(HierarchyFilterMode);
        InitializeGridColumn();

        if (!IsPostBack)
            OnDataBindControls(DataBindAction.BindStatus);

        this.Title = string.Format(GetLocalResourceObject("Message_HierarchyFilter_VMModal_aspx_cs_Select").ToString(),HierarchyFilterText);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void InitializeGridColumn()
    {
        //initialize cols
        foreach (DataRow row in SessionManager.MessageHierarchyFilter.Rows)
        {
            if (row["HierarchyFilterMode"].ToString() == HierarchyFilterMode)
            {
                ASGridBoundColumn boundColumn = (ASGridBoundColumn)uxGrid.MasterTableView.Columns.FindByUniqueName("DataKey");
                boundColumn.HeaderText = row["DisplayedText"].ToString();
                boundColumn.HeaderTooltip = row["DisplayedText"].ToString();
            }
        }

        if (HierarchyFilterMode == "MERCHANTNUMBER")
        {
            ASGridBoundColumn boundColumn = (ASGridBoundColumn)uxGrid.MasterTableView.Columns.FindByUniqueName("DataText");
            boundColumn.HeaderText = GetLocalResourceObject("String1Message_HierarchyFilter_VMModal_aspx_cs_MerchantName").ToString();
            boundColumn.HeaderTooltip = GetLocalResourceObject("String1Message_HierarchyFilter_VMModal_aspx_cs_MerchantName").ToString();
            boundColumn.Visible = true;

        }
        else if (HierarchyFilterMode == "HEADQUARTER")
        {
            ASGridBoundColumn boundColumn = (ASGridBoundColumn)uxGrid.MasterTableView.Columns.FindByUniqueName("DataText");
            boundColumn.HeaderText = GetLocalResourceObject("String1Message_HierarchyFilter_VMModal_aspx_cs_HeadquarterName").ToString();
            boundColumn.HeaderTooltip = GetLocalResourceObject("String1Message_HierarchyFilter_VMModal_aspx_cs_HeadquarterName").ToString();
            boundColumn.Visible = true;
        }



        //initialize cols
        //foreach (DataRow row in SessionManager.MessageHierarchyFilter.Rows)
        //{
        //    if (row["HierarchyFilterMode"].ToString() == HierarchyFilterMode)
        //    {
        //        ASGridBoundColumn boundColumn = new ASGridBoundColumn();
        //        boundColumn.DataField = "DataKey";
        //        boundColumn.UniqueName = "DataKey";
        //        boundColumn.HeaderText = row["DisplayedText"].ToString();
        //        boundColumn.HeaderTooltip = row["DisplayedText"].ToString();
        //        boundColumn.ASFormat = FormatType.StaticString;
        //        boundColumn.SortExpression = "DataKey";

        //        uxGrid.MasterTableView.Columns.Add(boundColumn);
        //    }
        //}

        //if (HierarchyFilterMode == "MERCHANTNUMBER")
        //{
        //    ASGridBoundColumn boundColumn = new ASGridBoundColumn();
        //    boundColumn.DataField = "DataText";
        //    boundColumn.UniqueName = "DataText";
        //    boundColumn.HeaderText = "Merchant Name";
        //    boundColumn.HeaderTooltip = "Merchant Name";
        //    boundColumn.ASFormat = FormatType.DynamicString;
        //    boundColumn.SortExpression = "DataKey";

        //    uxGrid.MasterTableView.Columns.Add(boundColumn);

        //}
        //else if (HierarchyFilterMode == "HEADQUARTER")
        //{
        //    ASGridBoundColumn boundColumn = new ASGridBoundColumn();
        //    boundColumn.DataField = "DataText";
        //    boundColumn.UniqueName = "DataText";
        //    boundColumn.HeaderText = "Headquarter Name";
        //    boundColumn.HeaderTooltip = "Headquarter Name";
        //    boundColumn.ASFormat = FormatType.DynamicString;
        //    boundColumn.SortExpression = "DataKey";

        //    uxGrid.MasterTableView.Columns.Add(boundColumn);
        //}
    }


    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindStatus:
                BindStatus();
                break;
            case DataBindAction.BindHierarchyFilterValue:
                FilterParameterCollection _params = new FilterParameterCollection();
                _params.AddLoggedInUserRiskParams();
                _params.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
                _params.Add(new FilterParameter("@MessageID", MessageID, DbType.Int32));

                string spaName = "spa_GetMessageHierarchyFilter_View";
                this.uxGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME,
                    new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(_params) });
                break;
        }
    }

    #region ControlEvent
    protected void uxGrid_Init(object sender, EventArgs e)
    {
        uxGrid.PageSize = 10;
    }

    protected void uxGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindHierarchyFilterValue);
    }

    #endregion

    private void BindStatus()
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserRiskParams();
        parames.Add(new FilterParameter("@MessageID", Convert.ToInt32(MessageID), DbType.Int32));

        parames.Add(new FilterParameter("@IsAll", false, DbType.Boolean, true));
        parames.Add(new FilterParameter("@IsIncluded", false, DbType.Boolean, true));
        parames.Add(new FilterParameter("@HierarchyFilterMode", HierarchyFilterMode, DbType.AnsiString));
        FilterParameterCollection paramesOut = new FilterParameterCollection();

        // Get OutParams value
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_GetMessageHierarchyFilter_Info", parames, out paramesOut);

        FilterParameter isIncluded = paramesOut.FindFilterParameterByName("@IsIncluded", true);

        if (isIncluded != null)
        {
            bool value = Convert.ToBoolean(isIncluded.ParameterValue);
            if (value)
            {
                uxInclude.Text = GetLocalResourceObject("String1Message_HierarchyFilter_VMModal_aspx_cs_Include").ToString();
            }
            else
            {
                uxInclude.Text = GetLocalResourceObject("String1Message_HierarchyFilter_VMModal_aspx_cs_Exclude").ToString();
            }
        }
    }
}
