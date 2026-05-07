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
using AS.Controls.UserControls;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Common;
using AS.Controls.Exporter;
using Telerik.Web.UI;

public partial class MessageGrid : GlobalUserControl
{
    public enum ViewMessageType : int
    {
        TodayMessage,
        PastMessage,
        None
    }
    protected enum DataBindAction
    {
        BindGrid
    }

    protected DateTime StartDate
    {
        get
        {
            if (ViewState["_StartDate"] == null)
                return DateTime.Now;
            else
                return Convert.ToDateTime(ViewState["_StartDate"]);
        }
        set { ViewState["_StartDate"] = value; }
    }
    protected DateTime EndDate
    {
        get
        {
            if (ViewState["_EndDate"] == null)
                return DateTime.Now;
            else
                return Convert.ToDateTime(ViewState["_EndDate"]);
        }
        set { ViewState["_EndDate"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            this.ViewMode = ViewMessageType.TodayMessage;
            // this.ViewMode = ViewMessageType.None;
        }
    }
    protected ViewMessageType ViewMode
    {
        get
        {
            if (ViewState["ViewMessageType"] == null)
            {
                return ViewMessageType.None;
            }
            else
            {
                return (ViewMessageType)Enum.Parse(typeof(ViewMessageType), ViewState["ViewMessageType"].ToString(), true);
            }
        }
        set
        {
            ViewState["ViewMessageType"] = value;
        }
    }
   
    protected override void OnDataBindControls(Enum type, object sender)
    {
        base.OnDataBindControls(type, sender);
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindGrid:
                FilterParameterCollection parameters = new FilterParameterCollection();
                string spaName = string.Empty;
                switch (this.ViewMode)
                {
                    case ViewMessageType.TodayMessage:
                    case ViewMessageType.PastMessage:
                        
                        spaName = "spa_GetMessage";
                        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                        parameters.Add(new FilterParameter("@BeginDate", StartDate, DbType.DateTime));
                        parameters.Add(new FilterParameter("@EndDate", EndDate, DbType.DateTime));
                        uxGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                        break;
                    case ViewMessageType.None:
                        uxGrid.DataSource = new DataTable("blank");
                        return;

                }                
                break;
        }

    }
     
    protected void uxGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindGrid);
    }

    public void Rebind(ViewMessageType Type, DateTime dtmStartDate, DateTime dtmEndDate)
    {
        ViewMode = Type;
        DateTime startDate = new DateTime(dtmStartDate.Year, dtmStartDate.Month, dtmStartDate.Day, 0, 0, 0);
        DateTime endDate = new DateTime(dtmEndDate.Year, dtmEndDate.Month, dtmEndDate.Day, 23, 59, 59);
        StartDate = startDate;
        EndDate = endDate;
        uxGrid.Rebind();
    }

    protected void uxExporter_NeedExportConfig(object sender, ExportConfig exportConfig)
    {
        exportConfig.ReportHeader = GetLocalResourceObject("MessageGridCS_Text_TodayMsg").ToString();
        exportConfig.FileName = GetLocalResourceObject("MessageGridCS_Text_TodayMsg").ToString().Replace(" ", null);

    }
}
