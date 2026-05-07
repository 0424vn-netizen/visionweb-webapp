using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using AS.Web.Business;
using AS.Web.UI;
using Telerik.Web.UI;
using System.Data;
using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.UserControls;
using AS.Common.WebUI;

[PagePermission("VoiRejDecRpt,MSVoiRejDecRpt")]
public partial class VoidsRejects : ReportPage
{
    #region Enums

    enum DataBindAction
    {
        BindDrilldownGrid,
        BindReportGrid,
    }


    #endregion
    const string IMAGE = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\" return ShowPopupModal('{0}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";

    string REPORT_HEADER = string.Empty;
    string _CardSearchIntruderQuery = string.Empty;
    private string _Target = "_parent";

    private string _GridTitle
    {
        get
        {
            return GeneralFuncsLib.GetFullGridTitleName(ReportFilter) + " " + GeneralFuncsLib.GetDateFilterText(ReportFilter);
        }
    }
    private string _GridDrillDownHeader
    {
        get
        {
            return GeneralFuncsLib.GetGridDrilldownColumnName(ReportFilter.CurrentValue.HierarchyMode, ReportFilter.CurrentValue.Value);
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        REPORT_HEADER = GetLocalResourceObject("VoidsRejectsDeclines_aspx_cs_VRD").ToString() + " ";
        if (SessionManager.ClientFrameInfo != string.Empty) _Target = SessionManager.ClientFrameInfo;
        IsBindDataOnLoad = true;
        this.uxToolTipMana.TargetControls.Clear();
        //46652 - AW Multi-currency Transaction Display
        GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
    }

    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        base.DoNeedExportConfig(sender, exportConfig);
        _isExporting = true;
        if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
        {
            uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = true;
            if (uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("VoidsRejectsDeclines_aspx_cs_MerchantNumber").ToString()
                || uxDrilldownGrid.Columns.FindByUniqueName("DrilldownColumn").HeaderText == GetLocalResourceObject("VoidsRejectsDeclines_aspx_cs_Merchant").ToString()
                || GeneralFuncsLib.IsMerchantMode(ReportFilter.CurrentValue.HierarchyMode))
            {
                uxDrilldownGrid.Columns.FindByUniqueName("EntityName").HeaderText = GetLocalResourceObject("VoidsRejectsDeclines_aspx_cs_MerchantName").ToString();
            }
        }
        if (uxReportGrid.Visible)
        {
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("ReasonCode").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("ReasonCodeDescription").Visible = true;
            //46652 - AW Multi-currency Transaction Display
            GeneralFuncsLib.ShowHideAWTransactionDetail(uxReportGrid);
        }
        //if (sender == uxExporterBottom)
        //    uxExporterBottom.GridHeader = uxExporterTop.GridHeader;
        //if (sender == uxExporterDrilldownBottom)
        //    uxExporterDrilldownBottom.GridHeader = uxExporterDrilldownTop.GridHeader;
        //base.DoNeedExportConfig(sender, exportConfig);
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
                // Auth Link
                if (!dataRow["AuthorizationNumber"].ToString().Trim().IsNullOrEmpty())
                {
                    dataItem["AuthorizationNumber"].Text = VeraCodeSolution.DoVeraCode(
                        GeneralFuncsLib.BuildAuthUrlPopupModal(
                           (SecurePage)Page, dataRow["AuthorizationNumber"],
                           AuthIntruderQuery, ReportFilter.CurrentValue,
                           ((String)dataRow["AuthorizationNumber"]).ToString())
                           );
                }

                queryString = BuildSecureQueryString("cn=" + dataRow["PartialCardNumber"] + "&cnf=" + dataRow["AccountNumber"] + "&merch=" + ReportFilter.CurrentValue.Value);
                string urlCardDetail = "CardHistoryModal.aspx?" + queryString;
                string urlCard = "<a class=\"link\" href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";

                if (CheckCSViewFullCard())
                {
                    dataItem["AccountNumber"].Text = VeraCodeSolution.DoVeraCode(urlCard + dataRow["AccountNumber"].ToString() + "</a>");
                }
                else
                {
                    if (GeneralFuncsLib.HasIPForFullCard((SecurePage)this.Page))
                    {
                        dataItem["PartialCardNumber"].Text = VeraCodeSolution.DoVeraCode(
                            String.Format(IMAGE, BuildUrlForFullCard(dataRow["RecordId"].ToString(), dataRow["IssueBank"] != null ? dataRow["IssueBank"].ToString() : string.Empty, dataRow["ReportDate"].ToString())) + VeraCodeSolution.DoVeraCode(urlCard + dataRow["PartialCardNumber"].ToString() + "</a>")
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
                dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardDescription"].ToString());
                dataItem["TransactionCode"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionCode"].ToString());

                dataItem["TransactionDate"].Text = VeraCodeSolution.DoVeraCode(dataRow["TransactionDate"].ToString().Split(' ')[0]);
                dataItem["TransactionTime"].Text = VeraCodeSolution.DoVeraCode(dataRow["TransactionTime"].ToString());
                dataItem["KeyedEntry"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["EntryModeDescription"].ToString());
            }
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible = GeneralFuncsLib.SHOW_PROCESSINGDATA_ENTITYNAME();
        base.OnPreRender(e);
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
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserReportingParams();
        parames.AddHierarchyFilterParams((ReportPage)this);

        string spaName = string.Empty;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDrilldownGrid:
                {
                    spaName = WebSiteConstants.GET_REPORT_SPA_NAME;
                    parames.Add(new FilterParameter("@ReportType", "VoidRejects", DbType.AnsiString));

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
                    spaName = "spa_GetVoidRejectDetail";
                    uxExporterTop.GridTitle = VeraCodeSolution.ValidateResponseData(REPORT_HEADER);
                    uxExporterTop.GridSubTitle = VeraCodeSolution.ValidateResponseData(_GridTitle);
                }
                break;
        }
        ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parames) });
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

    private string _AuthIntruderQuery = string.Empty;
    private string AuthIntruderQuery
    {
        get
        {
            if (this._AuthIntruderQuery == string.Empty) this._AuthIntruderQuery = GeneralFuncsLib.BuildIntruderInfoForQueryStr(this.uxReportGrid.ID, new string[] { "AuthorizationNumber" });
            return this._AuthIntruderQuery;
        }
    }

    private string BuildUrlForFullCard(string encryptedCC, string issueBank, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=VoidRejectDetail" + "&cn=" + Server.UrlEncode(encryptedCC) + "&issue=" + issueBank + "&reportdate=" + ReportDate);
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

}
