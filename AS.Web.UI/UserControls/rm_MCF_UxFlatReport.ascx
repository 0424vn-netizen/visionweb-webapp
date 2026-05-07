<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_UxFlatReport.ascx.cs" Inherits="As.VisionWeb.Web.FlatReportControl" %>
<%@ Register TagName="DetectionQueueAssignmentList" Src="~/UserControls/rm_MCF_DetectionQueueAssignmentList.ascx" TagPrefix="uc" %>
<%@ Register TagName="Chargeback" Src="~/UserControls/rm_MCF_Chargebacks.ascx" TagPrefix="uc" %>
<%@ Register TagName="Transaction" Src="~/UserControls/rm_MCF_Transaction.ascx" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_CustomView.ascx" TagName="CustomView" TagPrefix="uc" %>

<as:RadAjaxManagerProxy ID="uxRadAjaxManagerProxy" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxRefreshPage">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlFlatReportInfo" />
                <tek:AjaxUpdatedControl ControlID="pnlNoDataFound" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnFilterWorkingStatus">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlFlatReportInfo" />
                <tek:AjaxUpdatedControl ControlID="pnlNoDataFound" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnReloadCustomView">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="cidBarometerGrid" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnSecurityRebindCustomView">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxCustomView" />
                <tek:AjaxUpdatedControl ControlID="cidBarometerGrid" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnReloadRequeue">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="cidBarometerGrid" />
                <tek:AjaxUpdatedControl ControlID="uxWork" LoadingPanelID="uxInvisiblePanel" />
                <tek:AjaxUpdatedControl ControlID="hddCurrentStatus" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnLoadChargebacks">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlChargeback" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnLoadTransaction">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlTransaction" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxOpenWarning">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxOpenWarning" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxRefreshBtn">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlAssignmentList" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxAccountNumberClick">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxHiddenAccountNumberClick" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxAuthClick">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxHiddenAuthClick" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxReloadAssignment">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlAssignmentList" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnHddNext">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlFlatReportInfo" />
                <tek:AjaxUpdatedControl ControlID="pnlNoDataFound" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<as:Panel ID="pnlAssignmentList" runat="server" CssClass="pos-relative">
</as:Panel>
<div class="row" id="uxWorkedNotWorked">
    <div class="col-xs-10 dark-blue d-flex">

        <div class="mr-10" runat="server" id="optAll">
            <a href="#" onclick="ChangeWorkedStatusOption(this, -1); return false;" class="btn-bp-merchant btn-work-js">
                <asp:Literal ID="Literal2" runat="server" meta:resourcekey="optAllMerchantResource"></asp:Literal></a>
        </div>

        <div class="mr-10" runat="server" id="optNotWorked">
            <a href="#" onclick="ChangeWorkedStatusOption(this, 0); return false;" class="btn-bp-merchant btn-work-selected btn-work-js">
                <asp:Literal ID="Literal1" runat="server" meta:resourcekey="optNotWorkedMerchantResource"></asp:Literal></a>
        </div>
        <div class="mr-10" runat="server" id="optWorked">
            <a href="#" onclick="return ChangeWorkedStatusOption(this, 1);" class="btn-bp-merchant btn-work-js">
                <asp:Literal ID="Literal3" runat="server" meta:resourcekey="optWorkedMerchantResource"></asp:Literal></a>
        </div>
        <div class="mr-10" runat="server" id="optWIPByMe">
            <a href="#" onclick="return ChangeWorkedStatusOption(this, 2);" class="btn-bp-merchant btn-work-js">
                <asp:Literal ID="Literal4" runat="server" meta:resourcekey="optWIPByMeMerchantResource"></asp:Literal></a>
        </div>
        <div class="mr-10" runat="server" id="optWIPByOrthers">
            <a href="#" onclick="return ChangeWorkedStatusOption(this, 3);" class="btn-bp-merchant btn-work-js">
                <asp:Literal ID="Literal5" runat="server" meta:resourcekey="optWIPByOrthersMerchantResource"></asp:Literal></a>
        </div>
        <div class="mr-10" runat="server" id="optRequeued">
            <a href="#" onclick="return ChangeWorkedStatusOption(this, 4);" class="btn-bp-merchant btn-work-js">
                <asp:Literal ID="Literal6" runat="server" meta:resourcekey="optRequeuedMerchantResource"></asp:Literal></a>
        </div>
    </div>

    <as:Button ID="btnFilterWorkingStatus" CssClass="hide" runat="server" OnClick="btnFilterWorkingStatus_Click" />
    <as:HiddenField runat="server" ID="filterWorkingStatus" Value="0" />
</div>

<div class="height-30"></div>
<div class="row">
    <as:ASRadCodeBlock ID="ASRadCodeBlock2" runat="server">
        <div class="col-xs-10" data-toggle="collapse" id="ucAssTitle" data-target="#<%= pnlFlatReportInfo.ClientID %>">
            <h2 class="grid-title on-top">
                <as:Literal ID="litGridTitle" runat="server" meta:resourcekey="litGridTitleResource1" />
            </h2>
        </div>
    </as:ASRadCodeBlock>
    <div class="col-xs-2">
        <div class="report-export on-top dropdown pull-right" id="pnlExporter" runat="server">
            <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle" id="litExport" runat="server">
                <as:Literal ID="ltExport" runat="server" Text="EXPORT" meta:resourcekey="ltExportResource1"></as:Literal></a>
            <ul class="dropdown-menu">
                <li id="uxLiExcel" runat="server">
                    <asp:LinkButton ID="imgExcel" runat="server" OnClick="ImageButtonExcel_Click" meta:resourcekey="imgExcelResource1">Excel</asp:LinkButton></li>
            </ul>
        </div>
    </div>
</div>

<div class="height-30"></div>
<as:Panel runat="server" ID="pnlFlatReportInfo" CssClass="in">
    <!--Merchant Information-->
    <asp:Panel ID="uxMerchantPanel" runat="server" CssClass="in">
        <asp:Repeater ID="uxMerchantInfo" runat="server" OnItemDataBound="uxMerchantInfo_ItemDataBound">
            <ItemTemplate>
                <div data-selector="security-item">
                    <h2 class="grid-title on-top no-toggle"><%# Eval("MerchantName") %></h2>
                    <h2 class="grid-title on-top no-toggle inline-block">
                        <span class="text-muted">
                            <as:Literal ID="litGridSubTitle2" runat="server" meta:resourcekey="litGridTitle2" />
                        </span>
                    </h2>
                </div>
                <table class="ASTable mif-table detection-queue no-background-heading no-border in">
                    <colgroup>
                        <col class="w-20"/>
                        <col class="w-20"/>
                        <col class="w-35"/>
                        <col class="w-25"/>
                    </colgroup>
                    <tr class="Row">
                        <td class="heading text-nowrap">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMerchantNumber" Text="Merchant Number" meta:resourcekey="lituxMerchantInfoHeaderMerchantNumberResource1" />
                        </td>
                        <td class="heading text-nowrap">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderCityState" Text="City/State" meta:resourcekey="lituxMerchantInfoHeaderCityStateResource1" />
                        </td>
                        <td class="heading text-nowrap">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderSICCode" Text="SIC Code" meta:resourcekey="lituxMerchantInfoHeaderSICCodeResource1" />
                        </td>
                        <td class="heading text-nowrap">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderBEProc" Text="BackEnd Processor" meta:resourcekey="lituxMerchantInfoHeaderBEProcResource1" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="reportrisk-td-line-height">
                            <asp:LinkButton runat="server" ID="uxMerchantNumLink" CssClass="navbar-link" Text=""></asp:LinkButton>
                        </td>
                        <td class="reportrisk-td-line-height">
                            <%# Eval("CityState") != DBNull.Value ? Eval("CityState") : WebSiteConstants.HTML_EM_DASH_ENCODE  %>
                        </td>
                        <td class="reportrisk-td-line-height header-risk-merchant-info text-ellipsis">
                            <asp:Label ID="lblSIC" runat="server"></asp:Label>
                        </td>
                        <td class="reportrisk-td-line-height">
                            <%# Eval("BackEndProcessor") != DBNull.Value ? Eval("BackEndProcessor") : WebSiteConstants.HTML_EM_DASH_ENCODE  %>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading text-nowrap">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderStatus" Text="Status" meta:resourcekey="lituxMerchantInfoHeaderStatusResource1" />
                        </td>
                        <td class="heading text-nowrap">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderApprDate" Text="Approval Date" meta:resourcekey="lituxMerchantInfoHeaderApprDateResource1" />
                        </td>
                        <td class="heading text-nowrap">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderChainNO" Text="Chain Number" meta:resourcekey="lituxMerchantInfoHeaderChainNOResource1" />
                        </td>
                        <td class="heading text-nowrap header-risk-merchant-info text-ellipsis">
                            <asp:Label ID="lblHierarchyName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="reportrisk-td-line-height">
                            <%# Eval("Status") != DBNull.Value ? Eval("Status") : WebSiteConstants.HTML_EM_DASH_ENCODE  %>
                        </td>
                        <td class="reportrisk-td-line-height">
                            <%#ConvertDate(Eval("ApprovalDate"))%>
                        </td>
                        <td class="reportrisk-td-line-height">
                            <%# Eval("ChainNumber") != DBNull.Value ? Eval("ChainNumber") : WebSiteConstants.HTML_EM_DASH_ENCODE  %>
                        </td>
                        <td class="reportrisk-td-line-height header-risk-merchant-info text-ellipsis">
                            <asp:Label ID="lblHierarchyValue" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div class="row mt-3x">
                    <div class="col-md-4">
                        <table class="ASTable mif-table detection-queue no-background-heading child-table detection-queue-statistic-risk-table">
                            <tr>
                                <td colspan="2" class="no-cell-border text-gray-light font-20">
                                    <h3 class="grid-title reportrisk-grid-title no-toggle mb-10 mt-0 font-20">
                                        <as:Literal ID="lituxMerchantInfoHeaderRisk" runat="server" meta:resourcekey="lituxMerchantInfoHeaderRiskResource1" />
                                    </h3>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading text-nowrap col-heading">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel14" runat="server" ToolTip="Risk Score" meta:resourcekey="lituxMerchantInfoHeaderRiskScoreResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderRiskScore" Text="Risk Score" meta:resourcekey="lituxMerchantInfoHeaderRiskScoreResource1" />
                                    </asp:Panel>
                                </td>
                                <td class="col-data">
                                    <%#AS.Common.Formater.FormatData.FormatNumber(Eval("RiskScore"), 0)%>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading text-nowrap col-heading">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel13" runat="server" ToolTip="Merchant Classification" meta:resourcekey="lituxMerchantInfoHeaderMerchantClassificationResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMerchantClassification" Text="Merchant Classification" meta:resourcekey="lituxMerchantInfoHeaderMerchantClassificationResource1" />
                                    </asp:Panel>
                                </td>
                                <td class="col-data">
                                    <asp:Label ID="lblClassificationName" CssClass="text-ellipsis" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading text-nowrap col-heading">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel12" runat="server" ToolTip="Profile" meta:resourcekey="lituxMerchantInfoHeaderProfileResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderProfile" Text="Profile" meta:resourcekey="lituxMerchantInfoHeaderProfileResource1" />
                                    </asp:Panel>
                                </td>
                                <td class="col-data">
                                    <asp:Label ID="lblProfile" CssClass="text-ellipsis" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading text-nowrap col-heading">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel11" runat="server" ToolTip="Times Worked" meta:resourcekey="lituxMerchantInfoHeaderTimesWorkedResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderTimesWorked" Text="Times Worked" meta:resourcekey="lituxMerchantInfoHeaderTimesWorkedResource1" />
                                    </asp:Panel>
                                </td>
                                <td class="col-data">
                                    <span class="item-worked" data-worked="true"><%# Eval("Worked") %></span>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading text-nowrap col-heading">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel10" runat="server" ToolTip="Parameters Worked" meta:resourcekey="lituxMerchantInfoHeaderParametersOfWorkedResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderParametersOfWorked" Text="Parameters Worked" meta:resourcekey="lituxMerchantInfoHeaderParametersOfWorkedResource1" />
                                    </asp:Panel>
                                </td>
                                <td class="col-data">
                                    <span class="item-para-worked" data-para-worked="true"><%# Eval("ParametersWorked") %></span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-md-4">
                        <table class="ASTable mif-table detection-queue no-background-heading child-table">
                            <tr>
                                <td colspan="2" class="no-cell-border text-gray-light font-20">
                                    <h3 class="grid-title reportrisk-grid-title no-toggle mt-0 font-20">
                                        <as:Literal ID="lituxMerchantInfoHeaderHistoricPerformance" runat="server" meta:resourcekey="lituxMerchantInfoHeaderHistoricPerformanceResource1" />
                                    </h3>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel1" runat="server" ToolTip="MTD Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMTDNetAmtResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMTDNetAmt" Text="MTD Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMTDNetAmtResource1" />
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Label ID="uxMTDVolume" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel6" runat="server" ToolTip="Mo 1 Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMv1Resource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMv1" Text="Mo 1 Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMv1Resource1" />
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Label ID="uxMV1" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel7" runat="server" ToolTip="Mo 2 Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMV2Resource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMV2" Text="Mo 2 Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMV2Resource1" />
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Label ID="uxMV2" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel9" runat="server" ToolTip="Mo 3 Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMV3Resource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMV3" Text="Mo 3 Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMV3Resource1" />
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Label ID="uxMV3" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel2" runat="server" ToolTip="YTD Net Amt" meta:resourcekey="lituxMerchantInfoHeaderYTDVolResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderYTDVol" Text="YTD Net Amt" meta:resourcekey="lituxMerchantInfoHeaderYTDVolResource1" />
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Label ID="uxYTDVolume" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-md-4">
                        <table class="ASTable mif-table detection-queue no-background-heading child-table in">
                            <tr>
                                <td class="no-cell-border text-gray-light font-20">
                                    <h3 class="grid-title reportrisk-grid-title no-toggle mb-10 mt-0 font-20">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMetrics" Text="Metrics" meta:resourcekey="lituxMerchantInfoHeaderMetricsResource1" />
                                    </h3>
                                </td>
                                <td class="text-nowrap no-cell-border">
                                    <div class="mb-0">
                                        <strong>
                                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderContract" Text="Contract" meta:resourcekey="lituxMerchantInfoHeaderContractResource1" />
                                        </strong>
                                    </div>
                                </td>
                                <td class="text-nowrap no-cell-border">
                                    <div class="mb-0">
                                        <strong>
                                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderTodays" Text="Today" meta:resourcekey="lituxMerchantInfoHeaderTodaysResource1" />
                                        </strong>
                                    </div>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel15" runat="server" ToolTip="Monthly Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMonthlyNetAmtResource2">
                                        <asp:Literal runat="server" ID="Literal9" Text="Monthly Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMonthlyNetAmtResource1" />
                                    </asp:Panel>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="uxMonthlyNetAmtContract" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel3" runat="server" ToolTip="Keyed Trans Pct" meta:resourcekey="lituxMerchantInfoHeaderKeyedTransPctResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderKeyedTransPct" Text="Keyed Trans Pct" meta:resourcekey="lituxMerchantInfoHeaderKeyedTransPctResource1" />
                                    </asp:Panel>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="uxKeyedTransPctContract" runat="server"></asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="uxKeyedTransPctToday" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel4" runat="server" ToolTip="Avg Trans Amt" meta:resourcekey="lituxMerchantInfoHeaderAvgTransAmtResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderAvgTransAmt" Text="Avg Trans Amt" meta:resourcekey="lituxMerchantInfoHeaderAvgTransAmtResource1" />
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Label ID="uxExpectedAverageTicket" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="uxTodayAverageTicket" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel5" runat="server" ToolTip="Largest Trans Amt" meta:resourcekey="lituxMerchantInfoHeaderLargestTransAmtResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderLargestTransAmt" Text="Largest Trans Amt" meta:resourcekey="lituxMerchantInfoHeaderLargestTransAmtResource1" />
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Label ID="uxContractHighestTicket" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="uxTodayHighestTransactionAmount" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel8" runat="server" ToolTip="Net Amt" meta:resourcekey="lituxMerchantInfoHeaderTodayVolResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderTodayVol" Text="Net Amt" meta:resourcekey="lituxMerchantInfoHeaderTodayVolResource1" />
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Label ID="uxDailyVolume" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="uxTodayVolume" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </asp:Panel>

    <!--Barometer Report-->
    <as:PlaceHolder ID="uxPlcBarometer" runat="server">
        <div class="row">
            <div class="col-xs-10">
                <h2 class="grid-title inline-block mt-10" data-toggle="collapse" data-target="#uxBarometerContainer">
                    <span class="text-muted">
                        <asp:Literal runat="server" ID="Literal7" Text="Barometer Report" meta:resourcekey="litHeaderBarometerReportResource1" /></span>
                </h2>
                <as:RadCodeBlock runat="server" ID="uxRadCodeBlock2">
                    <span class="info-link" onclick="return openPopupWindowOnMenu(this,'<%= BuildUrlImage(MerchantNumber) %>','DQMCFWindow2');"></span>
                </as:RadCodeBlock>
            </div>
            <div class="col-xs-2 color-legend-nextqueue text-right mt-20">
                <as:PlaceHolder ID="PlaceHolder1" runat="server">
                    <a href="#item" onclick="return OpenFlagColorTable();" class="btn btn-default">
                        <asp:Literal ID="Literal8" runat="server" meta:resourcekey="rm_DetectionQueue_aspx_ColorLegendResource1"></asp:Literal></a>
                </as:PlaceHolder>
            </div>
        </div>
        <div id="uxBarometerContainer" class="in">
            <div class="row mt-5">
                <div class="col-xs-12">
                    <uc:CustomView runat="server" ID="uxCustomView" PageMode="DetectionQueue" PageSection="SecurityReport" />
                </div>
            </div>
            <div class="height-14"></div>
            <div id="cidBarometerGrid" class="in barometer-report-nextqueue" style="" runat="server">
                <table class="ASTable">
                    <colgroup>
                        <col>
                        <col>
                        <col>
                        <col>
                        <col>
                        <col>
                        <col class="w-15">
                        <col>
                    </colgroup>
                    <tbody>
                        <as:ASRepeater ID="uxBarometerGrid" runat="server" OnItemDataBound="uxBarometerGrid_ItemDataBound">
                            <ItemTemplate>
                            </ItemTemplate>
                        </as:ASRepeater>
                    </tbody>
                </table>
            </div>
        </div>
    </as:PlaceHolder>

    <!--Transactions(Today); Chargebacks(90days); Daily FC Analysis-->
    <div class="height-24" data-loading-chargeback="true"></div>
    <asp:Panel ID="pnlChargeback" runat="server" CssClass="in" Style="min-height: 50px">
    </asp:Panel>

    <div class="height-24" data-loading-transaction="true"></div>
    <asp:Panel ID="pnlTransaction" runat="server" CssClass="in" Style="min-height: 50px">
    </asp:Panel>

    <as:Panel runat="server" ID="pnlWork">
        <div id="actionBarSecurity" class="action-bar-container">
            <div class="row mt-3x">
                <div class="col-xs-1 security-work-content">
                    <div runat="server" id="uxWork" class="security-work-item"></div>
                </div>
                <div class="col-xs-5 requeue-content">
                    <div id="colRQColumn" runat="server" class="requeue-item">
                        <input type="checkbox" id="chkItemCV" runat="server" title="Checkbox to assign" />
                        <span class="lbl-requeue-item">
                            <as:Literal ID="Label2" runat="server" meta:resourcekey="RQResource"></as:Literal>
                        </span>
                    </div>
                    <div class="ml-14 hide" id="btnRequeueBoundary">
                        <as:Button ID="btnAddWorkQueue" runat="server" Text="Add To Work Queue" CssClass="btn btn-default as-inline mb-2" meta:resourcekey="btnAddToWorkQueueResource" />
                    </div>
                    <div class="ml-10 hide" id="divRemoveWorkQueue">
                        <as:Button ID="btnRemoveWorkQueue" runat="server" Text="Remove From Work Queue" CssClass="btn btn-default as-inline mb-2" meta:resourcekey="btnRemoveFromQueueResource" />
                    </div>
                </div>
                <div class="col-xs-6 security-paging">
                    <as:Button ID="btnPrevious" runat="server" IsStandardButton="True" OnClick="btnPrevious_Click" CssClass="btn btn-default mr-10" meta:resourcekey="btnPrevious" />
                    <as:Button ID="btnNext" runat="server" IsStandardButton="True" OnClick="btnNext_Click" CssClass="btn btn-default" meta:resourcekey="btnNext" />
                    <as:HiddenField ID="hddCurrentPageNo" runat="server" />
                    <as:HiddenField ID="hddCurrentStatus" runat="server" />
                </div>
            </div>
        </div>
    </as:Panel>

    <div class="display-none">
        <as:HiddenField ID="uxAccountValue" runat="server" />
        <as:Button ID="uxAccount" runat="server" IsStandardButton="True" OnClick="uxAccount_Click" meta:resourcekey="uxAccountResource1" />

    </div>
</as:Panel>

<as:Panel runat="server" ID="pnlNoDataFound" Visible="false">
    <div style="min-height: 50px">
        <div class="security-no-data-found">
            <as:Literal runat="server" ID="nodata" meta:resourcekey="lblNodataFound"></as:Literal>
        </div>
    </div>
</as:Panel>

<as:LinkButton ID="uxRefreshPage" runat="server" OnClick="uxRefreshPage_Click"></as:LinkButton>
<asp:Button ID="btnReloadCustomView" CssClass="hide" runat="server" OnClick="btnReloadCustomView_Click"></asp:Button>
<asp:Button ID="btnSecurityRebindCustomView" CssClass="hide" runat="server" OnClick="btnReloadCustomView_Click"></asp:Button>
<asp:Button ID="btnReloadRequeue" CssClass="hide" runat="server" OnClick="btnReloadRequeue_Click"></asp:Button>
<asp:Button ID="btnLoadChargebacks" CssClass="hide" runat="server" OnClick="btnLoadChargebacks_Click"></asp:Button>
<asp:Button ID="btnLoadTransaction" CssClass="hide" runat="server" OnClick="btnLoadTransaction_Click"></asp:Button>

<asp:LinkButton ID="uxOpenWarning" CssClass="hide" runat="server" OnClick="uxOpenWarning_Click"></asp:LinkButton>
<as:HiddenField ID="hddMessage" runat="server" />
<as:HiddenField ID="uxHiddenAccountNumberClick" runat="server" />
<as:Button ID="uxAccountNumberClick" runat="server" IsStandardButton="True" OnClick="uxAccountNumberClick_Click" CssClass="hide" />
<as:HiddenField ID="uxHiddenAuthClick" runat="server" />
<as:Button ID="uxAuthClick" runat="server" IsStandardButton="True" OnClick="uxAuthClick_Click" CssClass="hide" />
<asp:Button ID="uxReloadAssignment" CssClass="hide" runat="server" OnClick="uxRefreshBtn_Click"></asp:Button>
<asp:Button ID="btnHddNext" CssClass="hide" runat="server" OnClick="btnNext_Click"></asp:Button>

<as:ASRadCodeBlock ID="uxRadCodeBlock" runat="server">
    <script type="text/javascript">
        var rm_MCF_UxFlatReport_filterWorkingStatus = '<%= filterWorkingStatus.ClientID %>';
        var rm_MCF_UxFlatReport_btnRebindReportGrid = '<%= btnFilterWorkingStatus.ClientID %>';
        var pnlAssignmentList_ClientID = '<%= pnlAssignmentList.ClientID %>';
        var uxRefreshPage_ClientID = '<%= uxRefreshPage.ClientID %>';
        var lblWorkStatus = "<%=RM_MCF_GeneralFuncsLib.GetResourceValue("WorkStatus_Text").ToString()%>";
        var lblWorkInProgressStatus = "<%=RM_MCF_GeneralFuncsLib.GetResourceValue("WorkInProgressStatus_Text").ToString()%>";
        var lblWorkedStatus = "<%=RM_MCF_GeneralFuncsLib.GetResourceValue("WorkedStatus_Text").ToString()%>";
        var uxOpenWarning_ClientID = "<%=uxOpenWarning.ClientID%>";
        var hddMessage_ClientID = "<%=hddMessage.ClientID%>";
        var pnlExporter_ClientID = "<%=pnlExporter.ClientID%>";
        var applyFilterId_value = '<%= Page.SecureQueryString["ApplyFilterId"]%>';

        var rm_MCF_UxFlatReport_uxHiddenAccountNumberClick = '<%=uxHiddenAccountNumberClick.ClientID %>';
        var rm_MCF_UxFlatReport_uxAccountNumberClick = "<%=uxAccountNumberClick.ClientID %>";
        var rm_MCF_UxFlatReport_uxHiddenAuthClick = '<%=uxHiddenAuthClick.ClientID %>';
        var rm_MCF_UxFlatReport_uxAuthClick = "<%=uxAuthClick.ClientID %>";
        var rm_MCF_UxFlatReport_uxReloadAssignment = '<%=uxReloadAssignment.ClientID%>';
        var rm_MCF_UxFlatReport_chkItemCV = '<%=chkItemCV.ClientID%>';
        var rm_MCF_UxFlatReport_btnLoadChargebacks = '<%=btnLoadChargebacks.ClientID%>';
        var rm_MCF_UxFlatReport_btnLoadTransaction = '<%=btnLoadTransaction.ClientID%>';
        var rm_MCF_UxFlatReport_colRQColumn = '<%=colRQColumn.ClientID%>';

        var rm_MCF_UxFlatReport_btnReloadCustomView = '<%= btnReloadCustomView.ClientID %>';
        var rm_MCF_UxFlatReport_btnSecurityRebindCustomView = '<%= btnSecurityRebindCustomView.ClientID %>';
        var rm_MCF_UxFlatReport_btnReloadRequeue = '<%= btnReloadRequeue.ClientID %>';
        var rm_MCF_UxFlatReport_ReportDate = '<%= ReportDate %>';
        var rm_MCF_UxFlatReport_IsCSViewFullCard = '<%= IsCSViewFullCard %>';
        var rm_MCF_UxFlatReport_pnlTransaction = '<%= pnlTransaction.ClientID %>';
        var rm_MCF_UxFlatRepor_hddCurrentStatus = '<%= hddCurrentStatus.ClientID %>';
        var rm_MCF_UxFlatReport_btnHddNext = '<%= btnHddNext.ClientID %>';
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_UxFlatReport.js"></script>
</as:ASRadCodeBlock>
