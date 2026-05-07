using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Web.UI;
using System.Data;
using Telerik.Web.UI;
public partial class AuthorizationDetail : ReportPage
{
    enum DataBindAction
    {
        BindReportGrid
    }
    #region Properties

    string _CardSearchIntruderQuery = string.Empty;
    private string _Target = "_parent";

    private string CardSearchIntruderQuery
    {
        get
        {

            if (_CardSearchIntruderQuery == string.Empty)
            {
                _CardSearchIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxReportGrid.ID, new string[] { "AccountNumber" });
            }
            return _CardSearchIntruderQuery;
        }
    }

    //39992 - EMS Transaction Not Displayed for MID
    public bool IsNotInMif
    {
        get
        {
            if (IsSecureQueryString)
            {
                if (SecureQueryString["isNotInMif"].IsNotNullData())
                {
                    return SecureQueryString["isNotInMif"].ToLower().Equals("true");
                }
            }
            return false;
        }
    }

    int index = 0;
    string _MerchantNumber = string.Empty;
    const string IMAGE = "<a class=\"image-link\" class=\"atab\"href=\"#\" style=\"cursor:pointer\" onclick=\" return parent.ShowPopupModalChild({0}, '{1}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";

    private bool IsShowRoutingAccount
    {
        get
        {
            if (ViewState["ShowRoutingAccount"] != null)
                return (bool)(ViewState["ShowRoutingAccount"]);
            else
            {
                ViewState["ShowRoutingAccount"] = GeneralFuncsLib.Show_RoutingAccountNumber;
                return (bool)ViewState["ShowRoutingAccount"];
            }
        }
        set
        {
            ViewState["ShowRoutingAccount"] = value;
        }
    }

    #endregion
    protected override void DoSwitchView()
    {
        if (CheckCSViewFullCard())
        {
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;

            if (IsShowRoutingAccount)
            {
                uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = false;
            }
        }
        else
        {
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

            if (IsShowRoutingAccount)
            {
                uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
            }
        }
    }


    private string GridTitle()
    {
        string HierarchyValue = string.Empty;
        if (string.IsNullOrEmpty(_MerchantNumber))
            HierarchyValue = this.SavedReportFilterValue.Value;
        else
            HierarchyValue = _MerchantNumber;
        string GridTitleName = string.Empty;

        string entityName = GeneralFuncsLib.GetMerchantName(HierarchyValue, IsNotInMif);
        if(entityName.IsNullOrEmpty())
            GridTitleName = HierarchyValue;
        else
            GridTitleName = HierarchyValue + " - " + entityName;

        return GridTitleName;
    }
    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterAuthorizationDetailBottom");
        base.PageInitialize();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        if (SessionManager.ClientFrameInfo != string.Empty)
            _Target = SessionManager.ClientFrameInfo;
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        if (IsSecureQueryString)
        {
            _MerchantNumber = SecureQueryString["merch"];
            if (SecureQueryString["idx"] != null)
                int.TryParse(SecureQueryString["idx"].ToString(), out index);
        }

        if (string.IsNullOrEmpty(_MerchantNumber))
        {
            _MerchantNumber = this.SavedReportFilterValue.Value.Trim();
        }

        uxMerchantInfo.Text = VeraCodeSolution.DoVeraCode(GridTitle());
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (sender == uxReportGrid && uxReportGrid.Visible)
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }
    private bool CheckCSViewFullCard()
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
        {
            return true;
        }
        return false;
    }
    bool _isExporting = false;
    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        string spaName = string.Empty;
        string AuthNumber = SecureQueryString["AuthNumber"];
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    parameters.AddLoggedInUserReportingParams();
                    parameters.Add("@HierarchyFilterMode", IsNotInMif ? "MERCHANTNUMBER" : this.SavedReportFilterValue.HierarchyMode, DbType.AnsiString);
                    parameters.Add("@HierarchyFilterValue", _MerchantNumber, System.Data.DbType.String);
                    parameters.Add("@IsNotInMif", IsNotInMif, DbType.Boolean);

                    parameters.Add("@DateFilterMode", this.SavedReportFilterValue.DateOption, DbType.Int32);
                    parameters.Add("@BeginDate", this.SavedReportFilterValue.DateOptionValue.From, DbType.DateTime);
                    parameters.Add("@EndDate", this.SavedReportFilterValue.DateOptionValue.To, DbType.DateTime);
                    parameters.Add("@RiskClient", false, DbType.Boolean);
                    parameters.Add("@AuthNumber", AuthNumber, DbType.String);
                    parameters.AddLoggedInUserPrimaryUserID();
                    spaName = "spa_GetAuthorizationDetail";
                    if (CheckCSViewFullCard())
                    {
                        parameters.AddDecryptDataParams("AccountNumber", _isExporting);
                        if(IsShowRoutingAccount)
                            parameters.AddDecryptDataParams("RoutingAccountNumber", _isExporting);
                    }
                    parameters.AddLanguageID();
                    (sender as ASGrid).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                    uxExporter.GridHeader = GetLocalResourceObject("Text_AuthNumber").ToString() + ": " + AuthNumber;
                    uxExporter.GridSubTitle = GetLocalResourceObject("Text_AuthNumber").ToString() + ": " + AuthNumber;
                }
                break;
        }
    }
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

        if (IsShowRoutingAccount)
        {
            uxReportGrid.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
        }
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);

        base.DoNeedExportConfig(sender, exportConfig);
        if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
        {
            exportConfig.ReportHeader = GetLocalResourceObject("Text_AuthDetails").ToString() + ":" + "\r\n" + GetLocalResourceObject("Text_Merchant").ToString() + ": " + uxMerchantInfo.Text + "\r\n" + GetLocalResourceObject("Text_AuthNumber").ToString() + ": " + SecureQueryString["AuthNumber"];

        }
        else
        {
            exportConfig.ReportHeader = GetLocalResourceObject("Text_AuthDetails").ToString() + ":" + Environment.NewLine + GetLocalResourceObject("Text_Merchant").ToString() + ": " + uxMerchantInfo.Text + Environment.NewLine + GetLocalResourceObject("Text_AuthNumber").ToString() + ": " + SecureQueryString["AuthNumber"];
        }


        exportConfig.FileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("Text_AuthDetail_FileName").ToString() + GetLocalResourceObject("Text_Merchant").ToString() + ":" + uxMerchantInfo.Text + GetLocalResourceObject("Text_Auth_FileName").ToString() + ":" + SecureQueryString["AuthNumber"]);

    }
    protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            if (sender == uxReportGrid)
            {
                string urlCardSearch = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialCardNumber"].ToString(), dataRow["AccountNumber"].ToString(), _MerchantNumber, true, IsNotInMif);

                string urlRoutingSearch = string.Empty;
                if (IsShowRoutingAccount)
                {
                    urlRoutingSearch = GeneralFuncsLib.BuildCardUrl((SecurePage)this.Page, dataRow["PartialRoutingACC"].ToString(), dataRow["RoutingAccountNumber"].ToString(), _MerchantNumber, true, IsNotInMif);
                }

                if (CheckCSViewFullCard())
                {
                    dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCardSearch + dataItem["AccountNumber"].Text + "</a>");
                    if (IsShowRoutingAccount)
                        dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlRoutingSearch + dataItem["RoutingAccountNumber"].Text + "</a>");
                }
                else
                {

                    if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this))
                    {
                        dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(String.Format(IMAGE, (index + 1), BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString())) + urlCardSearch + dataRow["PartialCardNumber"] + "</a>");

                        if (IsShowRoutingAccount)
                            dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(String.Format(IMAGE, (index + 1), BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString())) + urlRoutingSearch + dataRow["PartialRoutingACC"] + "</a>");
                    }
                    else
                    {
                        dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCardSearch + dataItem["PartialCardNumber"].Text + "</a>");

                        if (IsShowRoutingAccount)
                            dataItem["PartialRoutingACC"].Text = VeraCodeSolution.DoVeraCode(urlRoutingSearch + dataItem["PartialRoutingACC"].Text + "</a>");
                    }

                }

            }
            dataItem["TransactionCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionCode"].ToString());
            dataItem["AVS"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["AVSDescription"].ToString());
            dataItem["CVV"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CVVDescription"].ToString());
            dataItem["AuthSource"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["AuthorizationSourceDescription"].ToString());
            dataItem["CustID"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CustIDDescription"].ToString());
            dataItem["MOTO"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["MOTODescription"].ToString());
            dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardDescription"].ToString());
            dataItem["Approved"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ApprovedDescription"].ToString());
            dataItem["ResponseCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ResponseCodeDescription"].ToString());
            dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntryModeDescription"].ToString());
            if (dataRow["Approved"].ToString() == "D")
            {
                dataItem["Approved"].Text = string.Format(WebSiteConstants.DECLINED_TEXT, dataItem["Approved"].Text);                
            }
        }
    }
    private string BuildUrlForFullCard(string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=AuthDetail" + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate + "&idx=" + (index + 1) + "&isNotInMif=" + IsNotInMif);
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }
}
