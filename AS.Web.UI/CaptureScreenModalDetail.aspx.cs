using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using System.Data;

public partial class CaptureScreenModalDetail : NonReportPage
{
    string _RecordID = string.Empty;
    DateTime _ReportDate = DateTime.Today;

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            if (SecureQueryString != null)
            {
                _RecordID = SecureQueryString["RecordID"];
                string Temp = this.SecureQueryString["ReportDate"];

                if (!DateTime.TryParse(Temp, out _ReportDate))
                {
                    IsIntruderDetected = true;
                    return;
                }
            }
            
            BindCaptureDetailModal();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    private void BindCaptureDetailModal()
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(0);
        _params.Add(new FilterParameter("@RecordID", _RecordID, DbType.AnsiString));
        _params.Add(new FilterParameter("@BeginDate", _ReportDate, DbType.Date));
        _params.Add(new FilterParameter("@EndDate", _ReportDate, DbType.Date));
        _params.AddLanguageID();
        DataTable dt = WebServices.CsReportServices.GetReports("spa_cs_GetCaptureSearchDetail", _params);
        uxCaptureDetail.DataSource = dt;
        uxCaptureDetail.DataBind();
    }
}
