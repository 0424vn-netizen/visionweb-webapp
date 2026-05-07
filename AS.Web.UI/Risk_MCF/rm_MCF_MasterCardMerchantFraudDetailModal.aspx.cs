using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Risk_MCF_rm_MCF_MasterCardMerchantFraudDetailModal : NonReportPage
{
    private string GridTitle
    {
        get
        {
            return string.Format(GetLocalResourceObject("GridTitle.Text").ToString());
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        uxExporter.GridSubTitle = string.Format("({0})", DateTime.Now.ToString("MM/dd/yyyy"));
        uxExporter.GridTitle = GridTitle;
    }

    protected override void OnLoad(EventArgs e)
    {
        PageType = SecurePageType.Modal;
        var merchantNumber = SecureQueryString != null ? SecureQueryString.Get("merchantnumber") : string.Empty;
        base.OnLoad(e);
    }

    protected void uxReportGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(null, uxReportGridM);
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection _parames = new FilterParameterCollection();
        _parames.AddLoggedInUserReportingParams(true);
        _parames.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));
        uxReportGridM.DataSource = new DataTable();
    }
}