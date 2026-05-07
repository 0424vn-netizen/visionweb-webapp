<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DQNextQReport.ascx.cs" Inherits="UserControls_DQNextQReport" %>
<%@ Register TagName="ASPager" Src="~/UserControls/ASPager.ascx" TagPrefix="uc" %>

<as:RadAjaxManagerProxy runat="server" ID="uxRadManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxAccountNumberClick">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxAccReload" />
            </UpdatedControls>
        </tek:AjaxSetting>        
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<asp:PlaceHolder runat="server" ID="phlMerchantInfo">
    <div class="row hide" runat="server" id="uxDivMerchantInfoTitle">
        <div class="col-xs-10">
            <h2 class="grid-title inline-block" data-toggle="collapse" data-target="#cidMerchantInfo">
                <span class="text-muted">
                    <%--<asp:Literal runat="server" ID="Literal2" Text="Barometer Report" meta:resourcekey="BarometerReport" />--%>
                    <asp:Literal ID="uxMerchantInfoTitle" Visible="false" runat="server" meta:resourcekey="rm_DQNextQReport_aspx_Text1Resource1" Text="Statistics"></asp:Literal>
                </span>
            </h2>
        </div>
    </div>
    <div id="cidMerchantInfo" class="in">
        <asp:Repeater ID="uxMerchantInfo" runat="server" OnItemDataBound="uxMerchantInfo_ItemDataBound">
            <ItemTemplate>
                <table class="ASTable">
                    <colgroup>
                        <col />
                        <col />
                        <col />
                        <col />
                        <col />
                        <col />
                        <col class="w-15" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr class="Row">
                            <td class="heading valign-middle">
                                <asp:LinkButton runat="server" ID="uxMerchantNumLink" CssClass="navbar-link" Text="" meta:resourcekey="uxRiskReportResource1"></asp:LinkButton>
                                <as:Literal ID="txtMerchantNumber" runat="server" meta:resourcekey="txtMerchantNumberResource1"></as:Literal>
                            </td>
                            <td colspan="3" class="heading valign-middle">
                                <div class="row">
                                    <%--<div class="col-xs-4">
                                    <as:Literal ID="txtMerchantNumber" runat="server" meta:resourcekey="txtMerchantNumberResource1"></as:Literal>
                                </div>--%>
                                    <div class="col-xs-6">
                                        <span>
                                            <%#Eval("MerchantName") %></span>
                                        <asp:ImageButton ID="uxCwinButton" ImageUrl="~/res/img/Change_Main_Window16.png"
                                            runat="server" OnClientClick="MerchantNumberClick(); return false;" meta:resourcekey="uxCwinButtonResource1" />
                                    </div>
                                    <div class="col-xs-2 negative">
                                        <%#Eval("Watch")%>
                                    </div>
                                </div>
                            </td>
                            <td class="heading valign-middle">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderApprDate" Text="Appr. Date:" meta:resourcekey="lituxMerchantInfoHeaderApprDateResource1" /></td>
                            <td>
                                <%#ConvertDate(Eval("ApprovalDate"))%>
                            </td>
                            <td class="heading"><%#Eval("HierarchyName")==""? "":Eval("HierarchyName")+":" %></td>
                            <td>
                                <%#Eval("HierarchyValue")%>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderChainNO" Text="Chain #:" meta:resourcekey="lituxMerchantInfoHeaderChainNOResource1" /></td>
                            <td>
                                <%#Eval("ChainNumber")%>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderCityState" Text="City/State:" meta:resourcekey="lituxMerchantInfoHeaderCityStateResource1" /></td>
                            <td>
                                <%#Eval("CityState")%>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderStatus" Text="Status:" meta:resourcekey="lituxMerchantInfoHeaderStatusResource1" /></td>
                            <td>
                                <%#Eval("Status")%>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderAuth" Text="Auth $, #, %:" meta:resourcekey="lituxMerchantInfoHeaderAuthResource1" /></td>
                            <td>
                                <div class="row">
                                    <div class="col-xs-5">
                                        <asp:Label ID="uxTodayAuthorizationVolume" runat="server" meta:resourcekey="uxTodayAuthorizationVolumeResource1"></asp:Label>
                                    </div>
                                    <div class="col-xs-3">
                                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("TodayAuthorizationCount"), 0)%>
                                    </div>
                                    <div class="col-xs-4">
                                        <%#AS.Common.Formater.FormatData.FormatPercent(Eval("TodayAuthorizationPercent"), 2)%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderTodayVol" Text="Today's Vol:" meta:resourcekey="lituxMerchantInfoHeaderTodayVolResource1" /></td>
                            <td>
                                <asp:Label ID="uxTodayVolume" runat="server" meta:resourcekey="uxTodayVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading" title="Average Daily Volume" meta:resourcekey="tdAverageDailyVolume" runat="server">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderAvgDailyVol" Text="Avg DailyVol:" meta:resourcekey="lituxMerchantInfoHeaderAvgDailyVolResource1" /></td>
                            <td>
                                <asp:Label ID="uxAVGDailyVolume" runat="server" meta:resourcekey="uxAVGDailyVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderProfile" Text="Profile:" meta:resourcekey="lituxMerchantInfoHeaderProfileResource1" /></td>
                            <td>
                                <asp:Label ID="lblProfile" runat="server" Text="Label" meta:resourcekey="lblProfileResource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderAuthDecl" Text="Auth Decl%:" meta:resourcekey="lituxMerchantInfoHeaderAuthDeclResource1" /></td>
                            <td>
                                <%#AS.Common.Formater.FormatData.FormatNumber(Eval("DeclinedAuthorizationPercent"), 2)%>%
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderTodayAvgTkt" Text="Today's Avg Tkt:" meta:resourcekey="lituxMerchantInfoHeaderTodayAvgTktResource1" /></td>
                            <td>
                                <asp:Label ID="uxTodayAverageTicket" runat="server" meta:resourcekey="uxTodayAverageTicketResource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Panel runat="server" ToolTip="Expected Average Ticket" meta:resourcekey="lituxMerchantInfoHeaderExpAvgTktResource2">
                                    <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpAvgTkt" Text="Exp AvgTkt:" meta:resourcekey="lituxMerchantInfoHeaderExpAvgTktResource1" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Label ID="uxExpectedAverageTicket" runat="server" meta:resourcekey="uxExpectedAverageTicketResource1"></asp:Label>
                            </td>
                            <td class="heading" title="BackEnd Processor" meta:resourcekey="tdBackEndProcessor" runat="server">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderBEProc" Text="BE Proc:" meta:resourcekey="lituxMerchantInfoHeaderBEProcResource1" /></td>
                            <td>
                                <%#Eval("BackEndProcessor")%>
                            </td>
                            <td class="heading" title="Repeat Authorization Count" meta:resourcekey="tdRepeatAuthorizationCount" runat="server">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderNoRptAuth" Text="# Rpt Auth:" meta:resourcekey="lituxMerchantInfoHeaderNoRptAuthResource1" /></td>
                            <td>
                                <%#AS.Common.Formater.FormatData.FormatNumber(Eval("RepeatAuthorizationCount"), 0)%>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMTDVol" Text="MTD Vol:" meta:resourcekey="lituxMerchantInfoHeaderMTDVolResource1" /></td>
                            <td>
                                <asp:Label ID="uxMTDVolume" runat="server" meta:resourcekey="uxMTDVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading" title="Expected Monthly Volume" meta:resourcekey="tdExpectedMonthlyVolume" runat="server">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpMV" Text="Exp MV:" meta:resourcekey="lituxMerchantInfoHeaderExpMVResource1" /></td>
                            <td>
                                <asp:Label ID="uxExpectedMonthlyVolume" runat="server" meta:resourcekey="uxExpectedMonthlyVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading" title="Monthly Volume Last Month" meta:resourcekey="tdMonthlyVolumeLastMonth" runat="server">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMv1" Text="MV-1:" meta:resourcekey="lituxMerchantInfoHeaderMv1Resource1" /></td>
                            <td>
                                <asp:Label ID="uxMV1" runat="server" meta:resourcekey="uxMV1Resource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderSalesRtn" Text="Sales, Rtns:" meta:resourcekey="lituxMerchantInfoHeaderSalesRtnResource1" /></td>
                            <td>
                                <div class="row">
                                    <div class="col-xs-5">
                                        <asp:Label ID="uxTodaySaleAmount" runat="server" meta:resourcekey="uxTodaySaleAmountResource1"></asp:Label>
                                    </div>
                                    <div class="col-xs-7">
                                        <asp:Label ID="uxTodayReturnAmount" runat="server" meta:resourcekey="uxTodayReturnAmountResource1"></asp:Label>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr class="">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderYTDVol" Text="YTD Vol:" meta:resourcekey="lituxMerchantInfoHeaderYTDVolResource1" /></td>
                            <td>
                                <asp:Label ID="uxYTDVolume" runat="server" meta:resourcekey="uxYTDVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading" title="Expected Yearly Volume" meta:resourcekey="tdExpectedYearlyVolume" runat="server">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpYV" Text="Exp YV:" meta:resourcekey="lituxMerchantInfoHeaderExpYVResource1" /></td>
                            <td>
                                <asp:Label ID="uxExpectedYearlyVolume" runat="server" meta:resourcekey="uxExpectedYearlyVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading" title="Monthly Volume 2 Months Ago" meta:resourcekey="tdMonthlyVolume2MonthsAgo" runat="server">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMV2" Text="MV-2:" meta:resourcekey="lituxMerchantInfoHeaderMV2Resource1" /></td>
                            <td>
                                <asp:Label ID="uxMV2" runat="server" meta:resourcekey="uxMV2Resource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpVSActKeyed" Text="Exp vs Act%Keyed:" meta:resourcekey="lituxMerchantInfoHeaderExpVSActKeyedResource1" />
                            </td>
                            <td>
                                <div class="row">
                                    <div class="col-xs-5">
                                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("ExpectedPercentSWP"), 0)%>%
                                    </div>
                                    <div class="col-xs-7">
                                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("KeyPercent"), 0)%>%
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr class="">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderSICCode" Text="SIC Code:" meta:resourcekey="lituxMerchantInfoHeaderSICCodeResource1" /></td>
                            <td colspan="3">
                                <%#Eval("SIC")%>
                            </td>
                            <td class="heading" title="Monthly Volume 3 Months Ago" meta:resourcekey="tdMonthlyVolume3MonthsAgo" runat="server">MV-3:</td>
                            <td>
                                <asp:Label ID="uxMV3" runat="server" meta:resourcekey="uxMV3Resource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderRiskScore" Text="Risk Score:" meta:resourcekey="lituxMerchantInfoHeaderRiskScoreResource1" /></td>
                            <td>
                                <%#AS.Common.Formater.FormatData.FormatNumber(Eval("RiskScore"), 0)%>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:PlaceHolder>
<%--<as:Button runat="server" ID="uxShowBarometer" OnClick="uxShowBarometer_Click" CssClass="hide" IsStandardButton="False" meta:resourcekey="uxShowBarometerResource1" />--%>
<as:PlaceHolder ID="uxPlcBarometer" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <h2 class="grid-title inline-block" data-toggle="collapse" data-target="#cidBarometerGrid">
                <span class="text-muted">
                    <asp:Literal runat="server" ID="Literal1" Text="Barometer Report" meta:resourcekey="BarometerReport" /></span>
            </h2>
            <as:RadCodeBlock runat="server" ID="uxRadCodeBlock2">
                <span class="info-link" onclick="return OpenInstanceWindow('<%=urlImage %>','DQWindow');"></span>
            </as:RadCodeBlock>
        </div>
    </div>
    <div id="cidBarometerGrid" class="in">
        <table class="ASTable">
            <tr>
                <as:TableColumnHeader ID="TableColumnHeader0" HeaderText="" meta:resourcekey="ASGridBoundColumnResource0" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader1" HeaderText="" meta:resourcekey="ASGridBoundColumnResource1" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader2" HeaderText="" meta:resourcekey="ASGridBoundColumnResource2" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader43" HeaderText="" meta:resourcekey="ASGridBoundColumnResourceCV" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader3" HeaderText="" meta:resourcekey="ASGridBoundColumnResource3" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader4" HeaderText="" meta:resourcekey="ASGridBoundColumnResource4" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader5" HeaderText="" meta:resourcekey="ASGridBoundColumnResource5" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader6" HeaderText="" meta:resourcekey="ASGridBoundColumnResource6" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader7" HeaderText="" meta:resourcekey="ASGridBoundColumnResource7" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader8" HeaderText="" meta:resourcekey="ASGridBoundColumnResource8" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader9" HeaderText="" meta:resourcekey="ASGridBoundColumnResource9" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader10" HeaderText="" meta:resourcekey="ASGridBoundColumnResource10" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader11" HeaderText="" meta:resourcekey="GridBoundColumnResource11" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader12" HeaderText="" meta:resourcekey="ASGridBoundColumnResource11" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader13" HeaderText="" meta:resourcekey="ASGridBoundColumnResource12" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader14" HeaderText="" meta:resourcekey="ASGridBoundColumnResource13" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader15" HeaderText="" meta:resourcekey="ASGridBoundColumnResource14" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader16" HeaderText="" meta:resourcekey="ASGridBoundColumnResource15" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader17" HeaderText="" meta:resourcekey="ASGridBoundColumnResource16" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader18" HeaderText="" meta:resourcekey="ASGridBoundColumnResource17" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader19" HeaderText="" meta:resourcekey="ASGridBoundColumnResource18" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader20" HeaderText="" meta:resourcekey="ASGridBoundColumnResource19" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader21" HeaderText="" meta:resourcekey="ASGridBoundColumnResource20" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader22" HeaderText="" meta:resourcekey="ASGridBoundColumnResource21" runat="server" />
                <as:TableColumnHeader ID="TableColumnHeader23" HeaderText="" meta:resourcekey="ASGridBoundColumnResource22" runat="server" />
            </tr>
            <as:ASRepeater ID="uxBarometerGrid" runat="server" OnItemDataBound="uxBarometerGrid_ItemDataBound" NumberOfColumns="23">
                <ItemTemplate>
                    <tr class='<%# Container.ItemIndex %2 == 0 ? "Row" : "AltRow"  %>'>
                        <as:TableColumnContent ID="uxBA" DataField="BusinessAge" Alignment="Center" runat="server" />
                        <as:TableColumnContent ID="uxRiskScore" DataField="RiskScore" ASFormat="Integer" Alignment="Right" runat="server" />
                        <as:TableColumnContent ID="uxVolumePercent" DataField="VolumePercent" ASFormat="Percentage" Alignment="Right" runat="server" />
                        <as:TableColumnContent ID="uxContractualVolume" DataField="ContractualVolume" ASFormat="Percentage" Alignment="Right" runat="server" />
                        <as:TableColumnContent ID="uxAverageTicketPercent" DataField="AverageTicketPercent" ASFormat="Percentage" Alignment="Right" runat="server" />
                        <as:TableColumnContent ID="uxAuthorizationPercent" DataField="AuthorizationPercent" ASFormat="Percentage" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxDeclinedAuthorizationPercent" DataField="DeclinedAuthorizationPercent" ASFormat="Percentage" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxRepeatAuthorizationCount" DataField="RepeatAuthorizationCount" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxTodayForeignCardCount" DataField="TodayForeignCardCount" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxKeyPercent" DataField="KeyPercent" ASFormat="Percentage" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxEvenDollarTransactionPercent" DataField="EvenDollarTransactionPercent" ASFormat="Percentage" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxDuplicateDollarTransactionPercent" DataField="DuplicateDollarTransactionPercent" ASFormat="Percentage" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxDuplicateBin" DataField="DuplicateBin" Alignment="Left" Text="" runat="server" />
                        <as:TableColumnContent ID="uxNegativeBatchCount" DataField="NegativeBatchCount" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxZeroBatchCount" DataField="ZeroBatchCount" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxTodayVolume" DataField="TodayVolume" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxTodayFirstTimeRetrievalVolume" DataField="TodayFirstTimeRetrievalVolume" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxTodayChargebackVolume" DataField="TodayChargebackVolume" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxReturnPercent" DataField="ReturnPercent" ASFormat="Percentage" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxTodayHighestTransactionAmount" DataField="TodayHighestTransactionAmount" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxTodayTransactionCount" DataField="TodayTransactionCount" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxTodayBatchCount" DataField="TodayBatchCount" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxSingleCardTransToday" DataField="SingleCardTransToday" Alignment="Right" Text="" runat="server" />
                        <as:TableColumnContent ID="uxSIC" DataField="SIC" Alignment="Center" Text="" runat="server" />
                        <as:TableColumnContent ID="uxProfileDescription" DataField="ProfileDescription" Alignment="Center" Text="" runat="server" />
                    </tr>
                </ItemTemplate>
            </as:ASRepeater>
        </table>
    </div>
</as:PlaceHolder>
<div class="row">
    <div class="col-xs-12">
        <%--<as:Button runat="server" ID="uxShowChargeback" OnClick="uxShowChargeback_Click" CssClass="hide" IsStandardButton="False" meta:resourcekey="uxShowChargebackResource1" />--%>
        <as:Panel ID="uxPanelChargeback" runat="server" meta:resourcekey="uxPanelChargebackResource1">
            <div class="row">
                <div class="col-xs-12">
                    <h2 class="grid-title" data-toggle="collapse" data-target="#cidChargeBackGrid">
                        <span class="text-muted">
                            <asp:Literal runat="server" ID="litHeaderChargeback90days" Text="Chargebacks (90 days)" meta:resourcekey="litHeaderChargeback90daysResource1" /></span>
                    </h2>
                </div>
            </div>
            <div id="cidChargeBackGrid" class="in">
                <asp:PlaceHolder ID="plhuxChargeback" runat="server">
                    <table class="ASTable grid-transaction freeze-table-no-pager in freeze-table">
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
            </div>
        </as:Panel>
    </div>
</div>
<div class="row">
    <div class="col-xs-12">
        <%--<as:Button runat="server" ID="uxShowTransaction" OnClick="uxShowTransaction_Click" CssClass="hide" IsStandardButton="False" meta:resourcekey="uxShowTransactionResource1" />--%>
        <as:Panel ID="uxPanelTransaction" runat="server" meta:resourcekey="uxPanelTransactionResource1">
            <div class="row">
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
                            <as:TableColumnHeader ID="TableColumnHeader35" HeaderText="" meta:resourcekey="ASGridBoundColumnResource46" runat="server" />
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

<as:ASRadCodeBlock runat="server" ID="uxRadCodeBlock1">
    <script type="text/javascript">
        var pagerID = '<%= uxPnlPager.ClientID%>';

        function AccountClick(param) {
            $get("<%=uxAccountValue.ClientID %>").value = param;
            $get("<%=uxAccount.ClientID %>").click();
        }
        function ShowPopupAcctNumber(actt) {
            document.getElementById("<%=uxHiddenAccountNumberClick.ClientID %>").value = actt;
            document.getElementById("<%=uxAccountNumberClick.ClientID %>").click();
            return false;
        }
        function openPopupCardWindow(url) {
            openPopupWindow(url, 'CardHistoryWindow');
            return false;
        }        
    </script>

</as:ASRadCodeBlock>
