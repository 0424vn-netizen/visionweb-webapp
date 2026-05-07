<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_DQNextQReport.ascx.cs" Inherits="UserControls_rm_MCF_DQNextQReport" %>
<%@ Register TagName="ASPager" Src="~/UserControls/ASPager.ascx" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_CustomView.ascx" TagName="CustomView" TagPrefix="uc" %>

<as:RadAjaxManagerProxy runat="server" ID="uxRadManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxAccountNumberClick">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxAccReload" />
            </UpdatedControls>
        </tek:AjaxSetting>

        <tek:AjaxSetting AjaxControlID="uxRebindBarometer">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="cidBarometerGrid" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<asp:PlaceHolder runat="server" ID="phlMerchantInfo">
    <div class="row hide" runat="server" id="uxDivMerchantInfoTitle">
        <div class="col-xs-10">
            <h2 class="grid-title inline-block" data-toggle="collapse" data-target="#cidMerchantInfo">
                <span class="text-muted">
                    <asp:Literal ID="uxMerchantInfoTitle" Visible="false" runat="server" meta:resourcekey="litGridTitle3" Text="Merchant Information"></asp:Literal>
                </span>
            </h2>
        </div>
    </div>
    <div id="cidMerchantInfo" class="in">
        <asp:Repeater ID="uxMerchantInfo" runat="server" OnItemDataBound="uxMerchantInfo_ItemDataBound">
            <ItemTemplate>
                <table class="ASTable mif-table detection-queue no-background-heading no-border in">
                    <colgroup>
                        <col class="w-20"/>
                        <col class="w-20"/>
                        <col class="w-35"/>
                        <col class="w-25"/>
                    </colgroup>
                    <tr class="Row">
                        <td class="heading text-nowrap">
                            <div><%# GetLocalResourceObject("MerchantNumber") %></div>
                        </td>
                        <td class="heading text-nowrap">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderCityState" Text="City/State" meta:resourcekey="lituxMerchantInfoHeaderCityStateResource1" />
                        </td>
                        <td class="heading text-nowrap">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderSICCode" Text="SIC Code" meta:resourcekey="lituxMerchantInfoHeaderSICCodeResource1" />
                        </td>
                        <td class="heading text-nowrap">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderBEProc" Text="SIC Code" meta:resourcekey="lituxMerchantInfoHeaderBEProcResource1" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="reportrisk-td-line-height">
                            <asp:LinkButton runat="server" ID="uxMerchantNumLink" CssClass="navbar-link" Text=""></asp:LinkButton>
                            <as:Literal ID="txtMerchantNumber" runat="server" meta:resourcekey="txtMerchantNumberResource1"></as:Literal>
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
                <!--Statistic_Information-->
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
                                    <h3 class="grid-title reportrisk-grid-title no-toggle mb-10 mt-0 font-20">
                                        <as:Literal ID="lituxMerchantInfoHeaderHistoricPerformance" runat="server" meta:resourcekey="lituxMerchantInfoHeaderHistoricPerformanceResource1" />
                                    </h3>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel2" runat="server" ToolTip="MTD Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMTDNetAmtResource2">
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
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel3" runat="server" ToolTip="YTD Net Amt" meta:resourcekey="lituxMerchantInfoHeaderYTDVolResource2">
                                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderYTD" Text="YTD Net Amt" meta:resourcekey="lituxMerchantInfoHeaderYTDVolResource1" />
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Label ID="uxYTDVolume" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-md-4">
                        <table class="ASTable mif-table detection-queue no-background-heading child-table">
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
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel16" runat="server" ToolTip="Monthly Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMonthlyNetAmtResource2">
                                        <asp:Literal runat="server" ID="Literal9" Text="Monthly Net Amt" meta:resourcekey="lituxMerchantInfoHeaderMonthlyNetAmtResource1" />
                                    </asp:Panel>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="uxMonthlyNetAmtContract" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading header-risk-merchant-info">
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel4" runat="server" ToolTip="Keyed Trans Pct" meta:resourcekey="lituxMerchantInfoHeaderKeyedTransPctResource2">
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
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel5" runat="server" ToolTip="Avg Trans Amt" meta:resourcekey="lituxMerchantInfoHeaderAvgTransAmtResource2">
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
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel8" runat="server" ToolTip="Largest Trans Amt" meta:resourcekey="lituxMerchantInfoHeaderLargestTransAmtResource2">
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
                                    <asp:Panel CssClass="text-ellipsis" ID="Panel15" runat="server" ToolTip="Net Amt" meta:resourcekey="lituxMerchantInfoHeaderTodayVolResource2">
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
                <!--/Statistic_Information-->
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:PlaceHolder>

<%--<as:Button runat="server" ID="uxShowBarometer" OnClick="uxShowBarometer_Click" CssClass="hide" IsStandardButton="False" meta:resourcekey="uxShowBarometerResource1" />--%>
<as:PlaceHolder ID="uxPlcBarometer" runat="server">
    <div class="row">
        <div class="col-xs-10">
            <h2 class="grid-title inline-block mt-10" data-toggle="collapse" data-target="#cidBarometerGridWrapper">
                <span class="text-muted">
                    <asp:Literal runat="server" ID="Literal1" Text="Barometer Report" meta:resourcekey="BarometerReport" /></span>
            </h2>

            <as:RadCodeBlock runat="server" ID="uxRadCodeBlock2">
                <span class="info-link" onclick="return openPopupWindowOnMenu(this, '<%=urlImage %>','DQMCFWindow2');"></span>
            </as:RadCodeBlock>
        </div>
        <div class="col-xs-2 color-legend-nextqueue text-right mt-20">
            <as:PlaceHolder ID="uxPlaceHolderColorLegend" runat="server">
                <a data-remove-export="colorlegend" href="#item" onclick="return OpenFlagColorTable();" class="btn btn-default">
                    <asp:Literal ID="rm_DetectionQueue_aspx_ColorLegend" runat="server" meta:resourcekey="rm_DetectionQueue_aspx_ColorLegendResource1" Text="Color Legend"></asp:Literal>
                </a>
            </as:PlaceHolder>
        </div>
        <asp:Panel runat="server" ID="uxPnlCustomview">
            <div class="col-xs-12 list-type-view mt-2x">
                <div class="row">
                    <div class="col-md-10">
                        <div class="inline-block">
                            <uc:CustomView runat="server" ID="uxCustomView" PageMode="DetectionQueue" PageSection="NextQueue" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="height-14"></div>
    <div id="cidBarometerGridWrapper">
        <!--Barometer-->
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
        <!--/Barometer-->
    </div>

</as:PlaceHolder>
<div class="row">
    <div class="col-xs-12">
        <%--<as:Button runat="server" ID="uxShowChargeback" OnClick="uxShowChargeback_Click" CssClass="hide" IsStandardButton="False" meta:resourcekey="uxShowChargebackResource1" />--%>
        <as:Panel ID="uxPanelChargeback" runat="server" meta:resourcekey="uxPanelChargebackResource1">
            <div class="row" id="divChargeback">
                <div class="col-xs-12">
                    <h2 class="grid-title" data-toggle="collapse" data-target="#cidChargeBackGrid">
                        <span class="text-muted">
                            <asp:Literal runat="server" ID="litHeaderChargeback90days" Text="Chargebacks (90 days)" meta:resourcekey="litHeaderChargeback90daysResource1" /></span>
                    </h2>
                </div>
            </div>
            <div id="cidChargeBackGrid" class="in">
                <!--Chargeback-->
                <asp:PlaceHolder ID="plhuxChargeback" runat="server">
                    <table class="ASTable grid-transaction freeze-table-no-pager in freeze-table" id="chargebackTbl">
                        <colgroup>
                            <col style="width: 150px;" />
                            <col />
                            <col />
                            <col />
                            <col />
                        </colgroup>
                        <tr>
                            <as:TableColumnHeader ID="uxAccountNumberHeader" Visible="false" HeaderText="" meta:resourcekey="ASGridBoundColumnResource36" runat="server" />
                            <as:TableColumnHeader ID="uxPartialAccountNumberHeader" HeaderText="" meta:resourcekey="ASGridBoundColumnResource37" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader44" HeaderText="" meta:resourcekey="ASGridBoundColumnResource47" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader45" HeaderText="" meta:resourcekey="ASGridBoundColumnResource48" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader26" HeaderText="" meta:resourcekey="ASGridBoundColumnResource38" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader27" HeaderText="" meta:resourcekey="ASGridBoundColumnResource39" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader28" HeaderText="" meta:resourcekey="ASGridBoundColumnResource40" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader29" HeaderText="" meta:resourcekey="ASGridBoundColumnResource41" runat="server" />
                        </tr>
                        <as:ASRepeater ID="uxChargeback" runat="server" NumberOfColumns="7" OnItemDataBound="uxChargeback_ItemDataBound" OnPreRender="uxChargeback_PreRender">
                            <ItemTemplate>
                                <tr class='<%# Container.ItemIndex %2 == 0 ? "Row" : "AltRow" %>'>
                                    <as:TableColumnContent ID="uxAccountNumber" UniqueName="AccountNumber" Visible="false" ASFormat="Auto" Alignment="Center" runat="server" />
                                    <as:TableColumnContent ID="uxPartialAccountNumber" UniqueName="PartialAccountNumber" ASFormat="Auto" Alignment="Center" runat="server" />
                                    <as:TableColumnContent ID="uxCardType" UniqueName="CardType" Alignment="Center" runat="server" />
                                    <as:TableColumnContent ID="uxReportDate" UniqueName="ReportDate" ASFormat="Date" Alignment="Center" runat="server" />
                                    <as:TableColumnContent ID="uxTransactionDate" UniqueName="TransactionDate" ASFormat="Date" Alignment="Center" runat="server" />
                                    <as:TableColumnContent ID="uxReasonCode" UniqueName="ReasonCode" Alignment="Left" Text="" runat="server" />
                                    <as:TableColumnContent ID="uxKeyed" UniqueName="Keyed" Alignment="Center" Text="" runat="server" />
                                    <as:TableColumnContent ID="uxTransactionAmount" UniqueName="TransactionAmount" Alignment="Right" Text="" runat="server" />
                                </tr>
                            </ItemTemplate>
                        </as:ASRepeater>
                        <asp:PlaceHolder ID="uxChargebackFooter" runat="server">
                            <tr class="Footer">
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <as:TableColumnContent ID="uxKeyedTotal" UniqueName="KeyedTotal" Alignment="Center" Text="" runat="server" />
                                <as:TableColumnContent ID="uxTransactionAmountTotal" UniqueName="TransactionAmountTotal" Alignment="Right" Text="" runat="server" />
                            </tr>
                        </asp:PlaceHolder>
                    </table>
                </asp:PlaceHolder>
                <asp:Panel ID="uxPnlPagerCB" runat="server"></asp:Panel>
                <!--/Chargeback-->
            </div>
        </as:Panel>
    </div>
</div>
<div class="row">
    <div class="col-xs-12">
        <%--<as:Button runat="server" ID="uxShowTransaction" OnClick="uxShowTransaction_Click" CssClass="hide" IsStandardButton="False" meta:resourcekey="uxShowTransactionResource1" />--%>
        <as:Panel ID="uxPanelTransaction" runat="server" meta:resourcekey="uxPanelTransactionResource1">
            <div class="row" id="divTransaction">
                <div class="col-xs-12">
                    <h2 class="grid-title" data-toggle="collapse" data-target="#cidTransGrid">
                        <span class="text-muted">
                            <asp:Literal runat="server" ID="litHeaderTransactionToday" Text="Transactions (Today)" meta:resourcekey="litHeaderTransactionTodayResource1" /></span>
                    </h2>
                </div>
            </div>

            <div id="cidTransGrid" class="in">
                <!--Transaction-->
                <asp:PlaceHolder ID="plhReportGridTransactions" runat="server">
                    <table class="ASTable grid-transaction freeze-table-no-pager in freeze-table" id="transactionTbl">
                        <colgroup>
                            <col style="width: 150px;" />
                            <col />
                            <col />
                            <col />
                            <col />
                            <col />
                            <col />
                            <col />
                            <col />
                            <col />
                            <col />
                            <col />
                        </colgroup>
                        <tr>
                            <as:TableColumnHeader ID="TableColumnHeader24" Visible="false" HeaderText="" meta:resourcekey="ASGridBoundColumnResource23" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader25" HeaderText="" meta:resourcekey="ASGridBoundColumnResource24" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader30" HeaderText="" meta:resourcekey="ASGridBoundColumnResource25" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader31" HeaderText="" meta:resourcekey="ASGridBoundColumnResource45" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader32" HeaderText="" meta:resourcekey="ASGridBoundColumnResource26" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader33" HeaderText="" meta:resourcekey="ASGridBoundColumnResource27" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader34" HeaderText="" meta:resourcekey="ASGridBoundColumnResource28" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader35" HeaderTooltipID="TableColumnHeader35" HeaderText="" meta:resourcekey="ASGridBoundColumnResource46" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader36" HeaderText="" meta:resourcekey="ASGridBoundColumnResource29" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader37" HeaderText="" meta:resourcekey="ASGridBoundColumnResource30" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader38" HeaderText="" meta:resourcekey="ASGridBoundColumnResource31" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader39" HeaderText="" meta:resourcekey="ASGridBoundColumnResource32" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader40" HeaderText="" meta:resourcekey="ASGridBoundColumnResource33" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader41" HeaderText="" Visible="false" meta:resourcekey="ASGridBoundColumnResource34" runat="server" />
                            <as:TableColumnHeader ID="TableColumnHeader42" HeaderText="" Visible="false" meta:resourcekey="ASGridBoundColumnResource35" runat="server" />
                        </tr>
                        <as:ASRepeater ID="uxReportGridTransactions" runat="server" NumberOfColumns="12" OnItemDataBound="uxReportGridTransactions_ItemDataBound" OnPreRender="uxReportGridTransactions_PreRender">
                            <ItemTemplate>
                                <tr class='<%# Container.ItemIndex %2 == 0 ? "Row" : "AltRow" %>'>
                                    <as:TableColumnContent ID="uxAccountNumber" UniqueName="AccountNumber" Visible="false" ASFormat="Auto" Alignment="Center" runat="server" />
                                    <as:TableColumnContent ID="uxPartialAccountNumber" UniqueName="PartialAccountNumber" ASFormat="Auto" Alignment="Center" runat="server" />
                                    <as:TableColumnContent ID="uxCardType" UniqueName="CardType" ASFormat="Auto" Alignment="Center" runat="server" />
                                    <as:TableColumnContent ID="uxCountryCode" UniqueName="CountryCode" Alignment="Center" Text="" runat="server" />
                                    <as:TableColumnContent ID="uxFileSource" UniqueName="FileSource" Alignment="Center" Text="" runat="server" />
                                    <as:TableColumnContent ID="uxTransactionDate" UniqueName="TransactionDate" Alignment="Center" Text="" runat="server" />
                                    <as:TableColumnContent ID="uxTransactionTime" UniqueName="TransactionTime" ASFormat="Auto" Alignment="Center" runat="server" />
                                    <as:TableColumnContent ID="uxDupeCount" UniqueName="DupeCount" Alignment="Center" Text="" runat="server" />
                                    <as:TableColumnContent ID="uxAuthorizationNumber" UniqueName="AuthorizationNumber" Alignment="Center" Text="" runat="server" />
                                    <as:TableColumnContent ID="uxKeyed" UniqueName="Keyed" Alignment="Center" Text="" runat="server" />
                                    <as:TableColumnContent ID="uxTransactionType" UniqueName="TransactionDescription" ASFormat="Auto" Alignment="Center" runat="server" />
                                    <as:TableColumnContent ID="uxMatch" UniqueName="MatchCode" Alignment="Center" Text="" runat="server" />
                                    <as:TableColumnContent ID="uxTransactionAmount" UniqueName="TransactionAmount" Alignment="Right" Text="" runat="server" />
                                    <as:TableColumnContent ID="uxDuplicateFlag" Visible="false" UniqueName="DuplicateFlag" Alignment="Right" Text="" runat="server" />
                                    <as:TableColumnContent ID="uxHighestTransactionAmountFlag" Visible="false" UniqueName="HighestTransactionAmountFlag" Alignment="Right" Text="" runat="server" />
                                </tr>
                            </ItemTemplate>
                        </as:ASRepeater>
                        <asp:PlaceHolder ID="uxReportGridTransactionsFooter" runat="server">
                            <tr class="Footer">
                                <as:TableColumnContent ID="uxPartialAccountNumberTotal" CssClass="heading" UniqueName="PartialAccountNumberTotal" Alignment="Left" Text="" runat="server" />
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <as:TableColumnContent ID="uxTransKeyedTotal" UniqueName="TransKeyedTotal" Alignment="Center" Text="" runat="server" />
                                <td></td>
                                <td></td>
                                <as:TableColumnContent ID="uxTransactionAmountFooterTotal" UniqueName="TransactionAmountTotal" Alignment="Right" Text="" runat="server" />
                            </tr>
                        </asp:PlaceHolder>
                    </table>
                </asp:PlaceHolder>
                <asp:Panel ID="uxPnlPager" runat="server"></asp:Panel>
                <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                <as:ASRadToolTip ID="tltDupeCount" meta:resourcekey="DupeCount" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
                    TargetControlID="TableColumnHeader35" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
                </as:ASRadToolTip>
                <%--<uc:ASPager ID="uxPager" runat="server" OnPageChanged="uxPager_PageChanged" Visible="false" />--%>
                <!--/Transaction-->
            </div>
        </as:Panel>
    </div>

</div>

<as:HiddenField ID="uxAccountValue" runat="server" />
<as:Button ID="uxAccount" runat="server" IsStandardButton="True" OnClick="uxAccount_Click" CssClass="hide" meta:resourcekey="uxAccountResource1" />

<as:HiddenField ID="uxHiddenAccountNumberClick" runat="server" />
<as:Button ID="uxAccountNumberClick" runat="server" IsStandardButton="True" OnClick="uxAccountNumberClick_Click"
    CssClass="hide" meta:resourcekey="uxAccountNumberClickResource1" />
<as:Button ID="uxAccReload" runat="server" IsStandardButton="True" CssClass="hide" meta:resourcekey="uxAccReloadResource1" />
<as:Button ID="uxRebindBarometer" runat="server" OnClick="uxRebindBarometer_Click" CssClass="hide" />

<as:ASRadCodeBlock runat="server" ID="uxRadCodeBlock1">
    <script type="text/javascript">
        var pagerID = '<%= uxPnlPager.ClientID%>';
        var pagerIDCB = '<%= uxPnlPagerCB.ClientID%>';
        var uxRebindBarometerID = '<%= uxRebindBarometer.ClientID%>';
        var rm_MCF_DQNextQReport_uxHiddenAccountNumberClick = '<%=uxHiddenAccountNumberClick.ClientID %>';
        var rm_MCF_DQNextQReport_uxAccountNumberClick = "<%=uxAccountNumberClick.ClientID %>";
        var rm_MCF_DQNextQReport_uxAccountValue = "<%=uxAccountValue.ClientID %>";
        var rm_MCF_DQNextQReport_uxAccount = "<%=uxAccount.ClientID %>";

    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_DQNextQReport.js"></script>
</as:ASRadCodeBlock>
