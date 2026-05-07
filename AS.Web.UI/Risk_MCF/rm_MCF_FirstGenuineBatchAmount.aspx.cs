using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using AS.Controls.Pages;

[PagePermission("RskFGBA,MSRskFGBA")]
public partial class rm_MCF_FirstGenuineBatchAmount : ReportPage
{
    public enum DataBindAction
    {
        BindList
    }

    public enum PostBackAction
    {
        Add
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindList, uxGenuineBatchAmountGrid);
        }
    }


    protected void uxBntSaveBatchAmount_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Add);
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);

        exportConfig.FileName = GeneralFuncsLib.FormatFileName(GetLocalResourceObject("ExprotFileName").ToString());
        exportConfig.ReportHeader = GetLocalResourceObject("ExportTitle").ToString();
    }
    protected void uxGenuineBatchAmountGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindList, uxGenuineBatchAmountGrid);

    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);

                    DataTable dt = WebServices.RiskServices.GetReports("spa_RM_MCF_Get_SettingValue", parameters);
                    uxHdCurrentValue.Value = dt.Rows[0]["SettingValue"].ToString();
                    uxHdSettingId.Value = dt.Rows[0]["SettingID"].ToString();
                    uxBatchAmount.Text = dt.Rows[0]["SettingValue"].ToString();

                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_RM_MCF_Get_LogSetting", ReportServices.ConvertToFilterParamWSArray(parameters) });
                    break;
                }
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Add:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    FilterParameterCollection outParameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@UserRecId", SessionManager.CurrentUser.RecId, DbType.Guid));
                    parameters.Add(new FilterParameter("@SettingValue", uxBatchAmount.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@SettingID", uxHdSettingId.Value, DbType.AnsiString));

                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_RM_MCF_Save_Setting", parameters, out outParameters);
                    uxBatchAmount.Text = string.Empty;
                    uxGenuineBatchAmountGrid.Rebind();
                    break;
                }
        }
    }
}