using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class rm_MCF_MerchantsInProfile : ReportPage
{
    #region Constants

    private const string MERCHANT_NUMBER = "MerchantNumber";

    #endregion Constants

    #region Fields

    private string _merchantIntruderQuery = string.Empty;

    #endregion Fields

    #region Properties

    private string MerchantIntruderQuery
    {
        get
        {

            if (_merchantIntruderQuery == string.Empty)
            {
                _merchantIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(
                    uxReportGrid.ID, new string[] { "MerchantNumber" });
            }
            return _merchantIntruderQuery;
        }
    }

    #endregion Properties

    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void DoGridNeedDataSource(ASGrid sender, GridNeedDataSourceEventArgs e)
    {
        string spaName = "spa_RM_MCF_GetMerchantsInProfile";
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserReportingParams(false);
        parames.Add(new FilterParameter("@ProfileID", SecureQueryString["RecordID"], DbType.Int32));
        parames.AddLanguageID();
        ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(
            WebServices.RiskServices,
            WebSiteConstants.GET_REPORT_METHOD_NAME,
            new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });
    }

    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            DataRowView dv = e.Item.DataItem as DataRowView;
            string url = RiskGeneral.BuildMerchantHyperlinkInRisk((SecurePage)Page,
                dv[MERCHANT_NUMBER], true, MerchantIntruderQuery,
                GeneralFuncsLib.NvlString(dv[MERCHANT_NUMBER]), true);
            item[MERCHANT_NUMBER].Text = VeraCodeSolution.DoVeraCode(url);
        }
    }

    #endregion Methods
}
