using System;
using System.Collections;
using System.Collections.Generic;
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
using Telerik.Web.UI;
using AS.Common.DBManager;
using AS.Common;
using AS.Controls.UserControls;
using AS.Controls.Exporter;

public partial class UserControls_rm_MCF_EscalationList : GlobalUserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uxEscalationGrid.IsIntruder = true;
        uxEscalationGrid.IntruderSourceName = GeneralFuncsLib.GetRequestFileName() +uxEscalationGrid.ID;
    }

    string _MerchantIntruderQuery = string.Empty;
    private string MerchantIntruderQuery
    {
        get
        {

            if (_MerchantIntruderQuery == string.Empty)
            {
                _MerchantIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxEscalationGrid.ID, new string[] { "MerchantNumber" });
            }
            return _MerchantIntruderQuery;
        }
    }
    protected void uxExport_NeedExportConfig(object sender, ExportConfig exportConfig)
    {

        exportConfig.FileName = GeneralFuncsLib.GetLegalFileName(GetLocalResourceObject("RiskEscalationList_ascx_cs_OpenEscalations").ToString());
        exportConfig.ReportHeader = GetLocalResourceObject("RiskEscalationList_ascx_cs_OpenEscalationsHeader").ToString();
    }
    protected void uxGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);

        parameters.Add(new FilterParameter("@AssignedToList", string.Empty, DbType.String));
        parameters.Add(new FilterParameter("@ResolutionList", ResolutionList, DbType.String));
        parameters.Add(new FilterParameter("@StatusList", StatusList, DbType.String));
        parameters.Add(new FilterParameter("@OpenClosedCode", OpenCode, DbType.String));
        parameters.Add(new FilterParameter("@OpenClosedFromDate", new DateTime(1900, 1, 1), DbType.DateTime));
        parameters.Add(new FilterParameter("@OpenClosedToDate", new DateTime(3000, 1, 1), DbType.DateTime));
        parameters.Add(new FilterParameter("@KeyType", string.Empty, DbType.String));
        parameters.Add(new FilterParameter("@KeyValue", string.Empty, DbType.String));
        parameters.Add(new FilterParameter("@stOrder", "EscalationDate", DbType.String));

        uxEscalationGrid.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_GetEscalation", parameters);
    }

    protected void uxGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            ReportPage rp = new ReportPage();;
            string queryString = rp.BuildSecureQueryString(string.Format("merchantnumber={0}{1}", GeneralFuncsLib.NvlString(dataRow["MerchantNumber"].ToString()), MerchantIntruderQuery));
            string url = string.Format("<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"parent.location = 'rm_MCF_RiskReport.aspx?{0}';\">", queryString);

            dataItem["MerchantName"].Text = VeraCodeSolution.DoVeraCode(url + GeneralFuncsLib.NvlString(dataRow["MerchantName"].ToString()) + "</a>");
            dataItem["MerchantName"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["MerchantNumber"].ToString());
        }

    }

    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {
        UxExport uxExport = sender as UxExport;
        exportConfig.FileName = uxExport.Attributes["FileName"].Replace(' ', '_');
    }

    public string OpenCode
    {
        get;
        set;
    }

    public string StatusList
    {
        get;
        set;
    }

    public string ResolutionList
    {
        get;
        set;
    }
}
