using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using AS.Common.DBManager;
using System.Data;
using AS.Controls.Grid;
using AS.Web.Business;
using Telerik.Web.UI;
using AS.Common;
[PagePermission("UserAccessReport,MSUserAccessReport")]
public partial class UserAccessDetail : ReportPage
{
    enum DataBindAction
    {
        BindReportGrid
    } 
     
    string UserID
    {
        get
        {
            if (SecureQueryString["UserID"] != null)
                return  SecureQueryString["UserID"].ToString();
            else
                return string.Empty;
        } 
    }
    string UserName { get; set; }
    string UserNameFull { get; set; }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        RegisterEvent();
        PageType = SecurePageType.Modal;
    }

    protected void RegisterEvent()
    {
        uxReportGrid.ItemDataBound += (s, e) =>
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                dataItem["LogInSuccess"].Text = dataRow["LogInSuccess"].Equals("Y") ? GetLocalResourceObject("UserAccessDetail_aspx_cs_LogInSuccess").ToString() : dataRow["LogInSuccess"].ToString();
            }
        };
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsSecureQueryString)
            return;
        IsBindDataOnLoad = true;
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
       

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = (GetLocalResourceObject("UserAccessDetail_aspx_cs_ExportFilename").ToString() + " " + UserName + " - " + UserNameFull).Replace(" ", "");
        exportConfig.ReportHeader = GetLocalResourceObject("UserAccessDetail_aspx_cs_ExportFilename").ToString() + " " + UserName + " - " + UserNameFull;
    } 

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                { 
                    parameters.Add(new FilterParameter("@DateFilterMode", SessionManager.UserAccessFilterOption.DateFilterMode, DbType.Int32));
                    parameters.Add(new FilterParameter("@BeginDate", SessionManager.UserAccessFilterOption.FromDate, DbType.Date));
                    parameters.Add(new FilterParameter("@EndDate", SessionManager.UserAccessFilterOption.ToDate, DbType.Date));
                    parameters.Add(new FilterParameter("@UserRedID", UserID, DbType.String));
                    DataTable tb = WebServices.SecurityServices.GetReports("spa_SEC_GetSessionDetail", parameters);
                    uxReportGrid.DataSource = tb;
                    if (tb.Rows.Count > 0)
                    {
                        header.Text = GetLocalResourceObject("UserAccessDetail_aspx_cs_ExportFilename").ToString() + " <b>" + tb.Rows[0]["UserID"] + " - " + tb.Rows[0]["UserNameFull"] + "</b>";
                        UserNameFull = tb.Rows[0]["UserNameFull"].ToString();
                        UserName = tb.Rows[0]["UserID"].ToString();

                    } 

                }
                break;
        }
    }


}
