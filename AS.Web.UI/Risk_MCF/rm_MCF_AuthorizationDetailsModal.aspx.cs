using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

[PagePermission("RskPort,MSRskPort")]
public partial class rm_MCF_AuthorizationDetailsModal : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindReportGrid
    }

    #endregion

    #region Const
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string AUTHORIZATION_NUMBER = "AuthorizationNumber";
    private const string IMAGE = "<a class=\"atab\"href=\"#\" style=\"cursor:pointer\" onclick=\" return parent.ShowPopupModalChild('{0}','auto');\"><img src='../res/images/information.gif' border=\"0\"/></a>&nbsp; &nbsp;";
    #endregion

    string _MerchantNumber = string.Empty;
    string _AuthorizationNumber = string.Empty;
    string _MerchantName = string.Empty;
    string _TransactionDate = string.Empty;
    int _Index = 0;
    private void ProcessQueryString()
    {
        _MerchantNumber = SecureQueryString["merch"];
        _AuthorizationNumber = SecureQueryString["AuthNumber"];
        _MerchantName = SecureQueryString["merchname"];
        if (string.IsNullOrEmpty(_MerchantName))
            _MerchantName = GeneralFuncsLib.GetMerchantName(_MerchantNumber);
        _TransactionDate = SecureQueryString["transactiondate"];
        if (SecureQueryString["idx"] != null)
            int.TryParse(SecureQueryString["idx"].ToString(), out _Index);
    }

    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterTop");
        base.PageInitialize();
        IsBindDataOnLoad = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (IsIntruderDetected) return;
        ProcessQueryString();
        if (!IsPostBack)
        {
            uxMerchantInfo.Text = VeraCodeSolution.DoVeraCode(_MerchantNumber + (!_MerchantName.IsNullOrEmpty() ? " - " + _MerchantName : string.Empty));
            uxExporterTop.GridSubTitle = uxExporterTop.GridHeader = VeraCodeSolution.ValidateResponseData(string.Format(GetLocalResourceObject("rm_AuthorizationDetailsModal_aspx_cs_Text1").ToString(), _AuthorizationNumber));
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    bool _IsExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _IsExporting = true;
        uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = true;
        base.DoNeedExportConfig(sender, exportConfig);
        if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
        {
            exportConfig.ReportHeader = GetLocalResourceObject("rm_AuthorizationDetailsModal_aspx_cs_Text2").ToString() + "\r\n" + 
                GetLocalResourceObject("rm_AuthorizationDetailsModal_aspx_cs_Text3").ToString() + " " + uxMerchantInfo.Text + "\r\n" + 
                GetLocalResourceObject("rm_AuthorizationDetailsModal_aspx_cs_Text4").ToString() + " " + _AuthorizationNumber;
        }
        else
        {
            exportConfig.ReportHeader = GetLocalResourceObject("rm_AuthorizationDetailsModal_aspx_cs_Text2").ToString() + Environment.NewLine + GetLocalResourceObject("rm_AuthorizationDetailsModal_aspx_cs_Text3").ToString() + " " + uxMerchantInfo.Text + Environment.NewLine + GetLocalResourceObject("rm_AuthorizationDetailsModal_aspx_cs_Text4").ToString() + " " + _AuthorizationNumber;
        }

        exportConfig.FileName = GeneralFuncsLib.FormatFileName("RiskManagement-RiskAnalysis-AuthorizationDetails");
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && uxReportGrid.Visible)
        {
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
        }
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardDescription"].ToString());

            string queryString = BuildSecureQueryString("cn=" + dataRow["PartialAccountNumber"] + "&cnf=" + dataRow["AccountNumber"] + "&merch=" + _MerchantNumber + "&isRisk=1");
            string urlCardDetail = ResolveUrl("~/") + "CardHistoryModal.aspx?" + queryString;
            string urlCardSearch = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";

            if (CheckCSViewFullCard())
            {
                if (!dataRow["AccountNumber"].ToString().IsNullOrEmpty())
                    dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCardSearch + dataItem["AccountNumber"].Text + "</a>");
            }
            else
            {

                if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
                {
                    dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssuingBank"].ToString(), dataRow["ReportDate"].ToString())) + urlCardSearch + dataRow["PartialAccountNumber"] + "</a>");
                }
                else
                {
                    dataItem["PartialAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCardSearch + dataItem["PartialAccountNumber"].Text + "</a>");
                }

            }
            dataItem["TransactionCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionCode"].ToString());

            dataItem["AVS"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["AVSDescription"].ToString());
            dataItem["CVV"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CVVDescription"].ToString());
            dataItem["AuthSource"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["AuthorizationSourceDescription"].ToString());
            dataItem["CustID"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CustIDDescription"].ToString());
            dataItem["MOTO"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["MOTODescription"].ToString());
            dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardDescription"].ToString());
            dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntryModeDescription"].ToString());
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                DateTime transDate = DateTime.Now.Date;
                transDate = Convert.ToDateTime(_TransactionDate);

                FilterParameterCollection parames = new FilterParameterCollection();
                parames.AddLoggedInUserReportingParams(true);
                parames.Add(new FilterParameter("@MerchantNumber", _MerchantNumber, DbType.AnsiString));
                parames.Add(new FilterParameter("@AuthNumber", _AuthorizationNumber, DbType.AnsiString));
                parames.Add(new FilterParameter("@TransactionDate", transDate, DbType.Date));

                if (CheckCSViewFullCard())
                {
                    parames.AddDecryptDataParams("AccountNumber", _IsExporting);
                }
                parames.AddLanguageID();
                string spaName = "spa_RM_MCF_GetAuthorizationDetails";
                ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });

                break;
        }
    }

    bool CheckCSViewFullCard()
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
            return true;
        return false;
    }

    protected override void DoSwitchView()
    {
        if (CheckCSViewFullCard())
        {
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = false;
        }
        else
        {
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialAccountNumber").Visible = true;
        }
    }

    private string CardSearchIntruderQuery(ASGrid grid)
    {
        string _CardSearchIntruderQuery = string.Empty;
        if (_CardSearchIntruderQuery == string.Empty)
        {
            _CardSearchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(grid.ID, new string[] { "AccountNumber" });
        }
        return _CardSearchIntruderQuery;
    }

    private string BuildUrlForFullCard(string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=AuthDetail" + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate + "&idx=" + (_Index + 1));
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }
}