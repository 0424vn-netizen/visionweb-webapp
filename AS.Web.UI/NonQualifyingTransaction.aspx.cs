using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Data;
using Telerik.Web.UI;

[PagePermission("NonTransRpt,MSNonTransRpt")]
public partial class QualifyingTransaction : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindDrilldownGrid,
        BindReportGrid,
    }


    #endregion
    const string IMAGE = "<a class=\"image-link\" href=\"#\" onclick=\" return ShowPopupModal('{0}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";
    string _CardSearchIntruderQuery = string.Empty;
    string _ReasonCodeDescription = string.Empty;
    string REPORT_HEADER = string.Empty;
    private string _Target = "_parent";

    private string _GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter) + " " + GeneralFuncsLib.GetDateFilterText(ReportFilter);
        }
    }
    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterDrilldownTop");
        //this.ExporterIDs.Add("uxExporterDrilldownBottom");
        this.ExporterIDs.Add("uxExporterTop");
        //this.ExporterIDs.Add("uxExporterBottom"); 
        base.PageInitialize();
    }

    private string _GridDrillDownHeader
    {
        get
        {
            return GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        REPORT_HEADER = GetLocalResourceObject("NonQualifyingTransaction_aspx_cs_NonQualifying").ToString() + " ";
        IsBindDataOnLoad = true;
        this.uxToolTipMana.TargetControls.Clear();
        if (SessionManager.ClientFrameInfo != string.Empty) _Target = SessionManager.ClientFrameInfo;
    }

    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;        
        ShowHideColumns(true);

        if (uxDrilldownGrid.Visible)
        {
            //if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
            //{
                uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = true;
                if (uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("NonQualifyingTransaction_aspx_cs_MerchantNumber").ToString()
                || uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("NonQualifyingTransaction_aspx_cs_Merchant").ToString()
                || GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
                {
                    uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderText = GetLocalResourceObject("NonQualifyingTransaction_aspx_cs_MerchantName").ToString();
                }
            //}
        }
        //if (sender == uxExporterBottom)
        //    uxExporterBottom.GridHeader = uxExporterTop.GridHeader;
        //if (sender == uxExporterDrilldownBottom)
        //    uxExporterDrilldownBottom.GridHeader = uxExporterDrilldownTop.GridHeader;
        base.DoNeedExportConfig(sender, exportConfig);
        string fileName = ((UxExport)sender).GridTitle.ToString() + _GridTitle;
        exportConfig.FileName = fileName.Replace(" ", "");
        exportConfig.ReportHeader = fileName;
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (sender == uxDrilldownGrid && uxDrilldownGrid.Visible)
            OnDataBindControls(DataBindAction.BindDrilldownGrid, sender);
        if (sender == uxReportGrid && uxReportGrid.Visible)
            OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (IsIntruderDetected) return;
        string queryString = string.Empty, urlAuthDetail = string.Empty, url = string.Empty;
        GridDataItem dataItem = e.Item as GridDataItem;
        DataRowView dataRow = e.Item.DataItem as DataRowView;
        if (e.Item is GridDataItem)
        {
            if (sender == uxDrilldownGrid && sender.Visible)
            {
                
                if (!GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME())
                {
                    if (GeneralFuncsLib.IsMerchantModeToSetTooltip(ReportFilter) && dataRow.DataView.Table.Columns["EntityName"] != null)
                    {
                        dataItem["DrilldownColumn"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntityName"].ToString());
                    }
                }
                dataItem["EntityName"].Text = VeraCodeSolution.DoVeraCode(dataRow["EntityName"].ToString());
                 
            }
            else
            {
                dataItem["AuthorizationNumber"].Text = VeraCodeSolution.DoVeraCode(
                    GeneralFuncsLib.BuildAuthUrlPopupModal(
                    (SecurePage)Page, dataRow["AuthorizationNumber"], AuthIntruderQuery,
                    ReportFilter.CurrentValue, ((String)dataRow["AuthorizationNumber"]).ToString())
                    );

                if (sender == uxReportGrid)
                {
                    queryString = BuildSecureQueryString("cn=" + dataRow["PartialCardNumber"] + "&cnf=" + dataRow["AccountNumber"] + "&merch=" + ReportFilter.CurrentValue.Value);
                    string urlCardDetail = "CardHistoryModal.aspx?" + queryString;
                    string urlCard = "<a href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";

                    if (CheckCSViewFullCard())
                    {
                        dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["AccountNumber"].ToString() + "</a>");
                    }
                    else
                    {
                        if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this.Page))
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(
                                String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString()
                                , dataRow["IssueBank"].ToString(), dataRow["ReportDate"].ToString()))
                                + VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>")
                                );
                        }
                        else
                        {
                            dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>");
                        }
                    }

                    if (!VeraCodeSolution.DoVeraCode(dataRow["ReasonCodeDescription"].ToString()).IsNullOrEmpty())
                    {
                        dataItem["ReasonCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["ReasonCodeDescription"] != null ? dataRow["ReasonCodeDescription"].ToString() : "test_ReasonDes <br /> dskjflksf");
                        uxToolTipMana.TargetControls.Add(dataItem["ReasonCode"].ClientID, dataRow["ReasonCodeDescription"].ToString(), true);
                    }
                    dataItem["QualificationCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["QualificationDescription"].ToString());
                    dataItem["TransactionCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionCode"].ToString().Trim());

                }
            }
        }
    }

    protected override void DoSwitchView()
    {
        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader);
        uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderTooltip = VeraCodeSolution.DoVeraCode(_GridDrillDownHeader);
        GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);
        if (GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode)
            && !string.IsNullOrEmpty(ReportFilter.CurrentValue.Value))
        {
            uxDrilldownGrid.Visible = false;
            uxReportGrid.Visible = true;
            if (CheckCSViewFullCard())
            {
                uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
                uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
            }
            else
            {
                uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
                uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
            }
        }
        else
        {
            uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText = GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
            uxDrilldownGrid.Visible = true;
            uxReportGrid.Visible = false;
            if (SessionManager.CurrentUser.ASClient == WebSiteConstants.ORION_CLIENT)//For FileSource is GLOBAL
            {
                uxDrilldownGrid.Columns.FindByUniqueName("UpgradeCount").Visible = false;
                uxDrilldownGrid.Columns.FindByUniqueName("FeeCount").Visible = false;
                uxDrilldownGrid.Columns.FindByUniqueName("WarningsCount").Visible = true;
                uxDrilldownGrid.Columns.FindByUniqueName("CashAdvanceAmount").Visible = true;
                uxDrilldownGrid.Columns.FindByUniqueName("TransCount").Visible = true;
                uxDrilldownGrid.Columns.FindByUniqueName("TransactionCount").Visible = false;
            }
            else
            {
                uxDrilldownGrid.Columns.FindByUniqueName("UpgradeCount").Visible = true;
                uxDrilldownGrid.Columns.FindByUniqueName("FeeCount").Visible = true;
                uxDrilldownGrid.Columns.FindByUniqueName("WarningsCount").Visible = false;
                uxDrilldownGrid.Columns.FindByUniqueName("CashAdvanceAmount").Visible = false;
                uxDrilldownGrid.Columns.FindByUniqueName("TransCount").Visible = false;
                uxDrilldownGrid.Columns.FindByUniqueName("TransactionCount").Visible = true;
            }
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserReportingParams();
        parames.AddHierarchyFilterParams(this);

        string spaName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDrilldownGrid:
                {
                    spaName = WebSiteConstants.GET_REPORT_SPA_NAME;
                    parames.Add(new FilterParameter("@ReportType", "TransQualification", DbType.AnsiString));
                    uxExporterDrilldownTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                    uxExporterDrilldownTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                    GeneralFuncsLib.VisibleHierarchyNameByFilteringMode(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value, uxDrilldownGrid, uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText);   
                }
                break;
            case DataBindAction.BindReportGrid:
                {
                    parames.AddLoggedInUserPrimaryUserID();
                    if (CheckCSViewFullCard())
                    {
                        parames.AddDecryptDataParams("AccountNumber", _isExporting);
                    }
                    parames.AddLanguageID();
                    spaName = "spa_GetTransQualificationDetail";
                    uxExporterTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                    uxExporterTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
        }
        ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });
    }

    private string _AuthIntruderQuery = string.Empty;
    private string AuthIntruderQuery
    {
        get
        {
            if (this._AuthIntruderQuery == string.Empty) this._AuthIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.uxReportGrid.ID, new string[] { "AuthorizationNumber" });
            return this._AuthIntruderQuery;
        }
    }

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

    private string BuildUrlForFullCard(string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=TransQualificationDetail" + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate);
        queryString = "FullCC.aspx?" + queryString;
        return queryString;
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
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME();
        ShowHideColumns();        
    }

    private void ShowHideColumns(bool isExport = false)
    {
        if (isExport)
        {
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        }
        uxReportGrid.Columns.FindByUniqueName("PurchaseID").Visible = SessionManager.CurrentClient == WebSiteConstants.SPR_CLIENT;
    }
}
