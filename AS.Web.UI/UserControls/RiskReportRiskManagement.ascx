<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RiskReportRiskManagement.ascx.cs"
    Inherits="UserControls_RiskReportRiskManagement" %>
<div class="height-16"></div>
<div class="row">
    <div class="col-md-7 on-top">
        <div class="row">
            <div class="col-md-12">
                <div class="row row-table">
                    <div class="col-xs-7">
                        <h2 class="grid-title-small on-top grid-toggle" data-toggle="collapse" data-target="#uxVolumeTicket"><asp:Literal ID="Literal1" runat="server" Text="Volume/Ticket Analysis" meta:resourcekey="Literal1Resource1" /></h2>
                    </div>
                    <div class="col-xs-5 td-bottom-10 text-muted text-right">
                        <asp:Literal ID="Literal2" runat="server" Text="Days Active" meta:resourcekey="Literal2Resource1" /> <b class="table-controls">
                            <asp:Literal ID="uxActiveDays" runat="server" Text="0" meta:resourcekey="uxActiveDaysResource1" /></b> <asp:Literal ID="Literal3" runat="server" Text="of the last 90" meta:resourcekey="Literal3Resource1" />
                    </div>
                </div>
                <div class="in" id="uxVolumeTicket">
                <table class="ASTable">
                    <tr>
                        <th colspan="4"><asp:Literal ID="Literal4" runat="server" Text="Volume" meta:resourcekey="Literal4Resource1" />
                        </th>
                        <th colspan="3"><asp:Literal ID="Literal5" runat="server" Text="Ticket" meta:resourcekey="Literal5Resource1" />
                        </th>
                    </tr>
                    <tr>
                        <th></th>
                        <th><asp:Literal ID="Literal6" runat="server" Text="Current" meta:resourcekey="Literal6Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal7" runat="server" Text="Average" meta:resourcekey="Literal7Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal8" runat="server" Text="Change" meta:resourcekey="Literal8Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal9" runat="server" Text="Current" meta:resourcekey="Literal9Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal10" runat="server" Text="Average" meta:resourcekey="Literal10Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal11" runat="server" Text="Change" meta:resourcekey="Literal11Resource1" />
                        </th>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><asp:Literal ID="Literal12" runat="server" Text="Daily" meta:resourcekey="Literal12Resource1" />
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxDailyCurrentVolume" runat="server" meta:resourcekey="uxDailyCurrentVolumeResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxDailyAverageVolume" runat="server" meta:resourcekey="uxDailyAverageVolumeResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxDailyChangeVolume" runat="server" meta:resourcekey="uxDailyChangeVolumeResource1" />%&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxDailyCurrentTicket" runat="server" meta:resourcekey="uxDailyCurrentTicketResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxDailyAverageTicket" runat="server" meta:resourcekey="uxDailyAverageTicketResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxDailyChangeTicket" runat="server" meta:resourcekey="uxDailyChangeTicketResource1" />%&nbsp;
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><asp:Literal ID="Literal13" runat="server" Text="7 Days" meta:resourcekey="Literal13Resource1" />
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux7DaysCurrentVolume" runat="server" meta:resourcekey="ux7DaysCurrentVolumeResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux7DaysAverageVolume" runat="server" meta:resourcekey="ux7DaysAverageVolumeResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="ux7DaysChangeVolume" runat="server" meta:resourcekey="ux7DaysChangeVolumeResource1" />%&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux7DaysCurrentTicket" runat="server" meta:resourcekey="ux7DaysCurrentTicketResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux7DaysAverageTicket" runat="server" meta:resourcekey="ux7DaysAverageTicketResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="ux7DaysChangeTicket" runat="server" meta:resourcekey="ux7DaysChangeTicketResource1" />%&nbsp;
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><asp:Literal ID="Literal14" runat="server" Text="MTD" meta:resourcekey="Literal14Resource1" />
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxMTDCurrentVolume" runat="server" meta:resourcekey="uxMTDCurrentVolumeResource1" />&nbsp;
                        </td>
                        <td class="text-right"><asp:Literal ID="Literal15" runat="server" Text="N/A" meta:resourcekey="Literal15Resource1" />
                        </td>
                        <td class="text-right"><asp:Literal ID="Literal16" runat="server" Text="N/A" meta:resourcekey="Literal16Resource1" />
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxMTDCurrentTicket" runat="server" meta:resourcekey="uxMTDCurrentTicketResource1" />&nbsp;
                        </td>
                        <td class="text-right"><asp:Literal ID="Literal17" runat="server" Text="N/A" meta:resourcekey="Literal17Resource1" />
                        </td>
                        <td class="text-right"><asp:Literal ID="Literal18" runat="server" Text="N/A" meta:resourcekey="Literal18Resource1" />
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><asp:Literal ID="Literal23" runat="server" Text="YTD" meta:resourcekey="Literal23Resource1" />
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxYTDCurrentVolume" runat="server" meta:resourcekey="uxYTDCurrentVolumeResource1" />&nbsp;
                        </td>
                        <td class="text-right"><asp:Literal ID="Literal19" runat="server" Text="N/A" meta:resourcekey="Literal19Resource1" />
                        </td>
                        <td class="text-right"><asp:Literal ID="Literal20" runat="server" Text="N/A" meta:resourcekey="Literal20Resource1" />
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxYTDCurrentTicket" runat="server" meta:resourcekey="uxYTDCurrentTicketResource1" />&nbsp;
                        </td>
                        <td class="text-right"><asp:Literal ID="Literal21" runat="server" Text="N/A" meta:resourcekey="Literal21Resource1" />
                        </td>
                        <td class="text-right"><asp:Literal ID="Literal22" runat="server" Text="N/A" meta:resourcekey="Literal22Resource1" />
                        </td>
                    </tr>
                </table>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <h2 class="grid-title-small grid-toggle" data-toggle="collapse" data-target="#uxRetrivalChargeback"><asp:Literal ID="Literal24" runat="server" Text="Retrieval/Chargeback Analysis" meta:resourcekey="Literal24Resource1" /></h2>
                <div class="in" id="uxRetrivalChargeback">
                <table class="ASTable">
                    <tr>
                        <th>&nbsp;
                        </th>
                        <th colspan="3"><asp:Literal ID="Literal25" runat="server" Text="Chargebacks" meta:resourcekey="Literal25Resource1" />
                        </th>
                        <th colspan="3"><asp:Literal ID="Literal26" runat="server" Text="Retrievals" meta:resourcekey="Literal26Resource1" />
                        </th>
                    </tr>
                    <tr>
                        <th>&nbsp;
                        </th>
                        <th><asp:Literal ID="Literal27" runat="server" Text="Quantity" meta:resourcekey="Literal27Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal28" runat="server" Text="Amount" meta:resourcekey="Literal28Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal29" runat="server" Text="Ratio to Sales" meta:resourcekey="Literal29Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal30" runat="server" Text="Quantity" meta:resourcekey="Literal30Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal31" runat="server" Text="Amount" meta:resourcekey="Literal31Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal32" runat="server" Text="Ratio to Sales" meta:resourcekey="Literal32Resource1" />
                        </th>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><asp:Literal ID="Literal33" runat="server" Text="Daily" meta:resourcekey="Literal33Resource1" />
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxDailyQuantityChargebacks" runat="server" meta:resourcekey="uxDailyQuantityChargebacksResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxDailyAmountChargebacks" runat="server" meta:resourcekey="uxDailyAmountChargebacksResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxDailyRatioSalesChargebacks" runat="server" meta:resourcekey="uxDailyRatioSalesChargebacksResource1" />%&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxDailyQuantityRetrievals" runat="server" meta:resourcekey="uxDailyQuantityRetrievalsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxDailyAmountRetrievals" runat="server" meta:resourcekey="uxDailyAmountRetrievalsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxDailyRatioSalesRetrievals" runat="server" meta:resourcekey="uxDailyRatioSalesRetrievalsResource1" />%&nbsp;
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><asp:Literal ID="Literal34" runat="server" Text="MTD" meta:resourcekey="Literal34Resource1" />
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxMTDQuantityChargebacks" runat="server" meta:resourcekey="uxMTDQuantityChargebacksResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxMTDAmountChargebacks" runat="server" meta:resourcekey="uxMTDAmountChargebacksResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxMTDRatioSalesChargebacks" runat="server" meta:resourcekey="uxMTDRatioSalesChargebacksResource1" />%&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxMTDQuantityRetrievals" runat="server" meta:resourcekey="uxMTDQuantityRetrievalsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxMTDAmountRetrievals" runat="server" meta:resourcekey="uxMTDAmountRetrievalsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxMTDRatioSalesRetrievals" runat="server" meta:resourcekey="uxMTDRatioSalesRetrievalsResource1" />%&nbsp;
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading" title="This value includes the data from the previous 3 full calendar months and does not include month to date data.">
                            <asp:Literal ID="Literal35" runat="server" Text="Previous 3 Months" meta:resourcekey="Literal35Resource1" />
                        </td>
                        <td class="text-right">
                            <as:Literal ID="ux3MonthsQuantityChargebacks" runat="server" meta:resourcekey="ux3MonthsQuantityChargebacksResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux3MonthsAmountChargebacks" runat="server" meta:resourcekey="ux3MonthsAmountChargebacksResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="ux3MonthsRatioSalesChargebacks" runat="server" meta:resourcekey="ux3MonthsRatioSalesChargebacksResource1" />%&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="ux3MonthsQuantityRetrievals" runat="server" meta:resourcekey="ux3MonthsQuantityRetrievalsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux3MonthsAmountRetrievals" runat="server" meta:resourcekey="ux3MonthsAmountRetrievalsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="ux3MonthsRatioSalesRetrievals" runat="server" meta:resourcekey="ux3MonthsRatioSalesRetrievalsResource1" />%&nbsp;
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><asp:Literal ID="Literal36" runat="server" Text="YTD" meta:resourcekey="Literal36Resource1" />
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxYTDQuantityChargebacks" runat="server" meta:resourcekey="uxYTDQuantityChargebacksResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxYTDAmountChargebacks" runat="server" meta:resourcekey="uxYTDAmountChargebacksResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxYTDRatioSalesChargebacks" runat="server" meta:resourcekey="uxYTDRatioSalesChargebacksResource1" />%&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxYTDQuantityRetrievals" runat="server" meta:resourcekey="uxYTDQuantityRetrievalsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxYTDAmountRetrievals" runat="server" meta:resourcekey="uxYTDAmountRetrievalsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxYTDRatioSalesRetrievals" runat="server" meta:resourcekey="uxYTDRatioSalesRetrievalsResource1" />%&nbsp;
                        </td>
                    </tr>
                </table>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-5">
                <h2 class="grid-title-small grid-toggle" data-toggle="collapse" data-target="#uxAuthorizations"><asp:Literal ID="Literal37" runat="server" Text="Authorizations (% of Sales)" meta:resourcekey="Literal37Resource1" /></h2>
                <div class="in" id="uxAuthorizations">
                <table class="ASTable">
                    <colgroup>
                        <col style="width:60px" />
                    </colgroup>
                    <tr>
                        <th>&nbsp;
                        </th>
                        <th><asp:Literal ID="Literal38" runat="server" Text="Daily" meta:resourcekey="Literal38Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal39" runat="server" Text="7 Day" meta:resourcekey="Literal39Resource1" />
                        </th>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><asp:Literal ID="Literal40" runat="server" Text="# Auths" meta:resourcekey="Literal40Resource1" />
                        <td class="text-right">
                            <asp:Literal ID="uxDailyPercentAuthCount" runat="server" meta:resourcekey="uxDailyPercentAuthCountResource1" />%
                        </td>
                        <td class="text-right">
                            <asp:Literal ID="ux7DaysPercentAuthCount" runat="server" meta:resourcekey="ux7DaysPercentAuthCountResource1" />%
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><asp:Literal ID="Literal41" runat="server" Text="Volume" meta:resourcekey="Literal41Resource1" />
                        </td>
                        <td class="text-right">
                            <asp:Literal ID="uxDailyDaysPercentAuthAmount" runat="server" meta:resourcekey="uxDailyDaysPercentAuthAmountResource1" />%
                        </td>
                        <td class="text-right">
                            <asp:Literal ID="ux7DaysPercentAuthAmount" runat="server" meta:resourcekey="ux7DaysPercentAuthAmountResource1" />%
                        </td>
                    </tr>
                </table>
                </div>
            </div>
            <div class="col-xs-7">
                <h2 class="grid-title-small grid-toggle" data-toggle="collapse" data-target="#uxCardAnalysis"><asp:Literal ID="Literal42" runat="server" Text="Daily Foreign Card Analysis" meta:resourcekey="Literal42Resource1" /></h2>
                <div class="in" id="uxCardAnalysis">
                <table class="ASTable">
                    <tr>
                        <th><asp:Literal ID="Literal43" runat="server" Text="Total $" meta:resourcekey="Literal43Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal44" runat="server" Text="Count" meta:resourcekey="Literal44Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal45" runat="server" Text="Daily Vol%" meta:resourcekey="Literal45Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal46" runat="server" Text="7 Day Vol%" meta:resourcekey="Literal46Resource1" />
                        </th>
                    </tr>
                    <tr class="Row">
                        <td class="text-right">
                            <as:Literal ID="uxDailyForeignCardVolume" runat="server" meta:resourcekey="uxDailyForeignCardVolumeResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxDailyForeignCardCount" runat="server" meta:resourcekey="uxDailyForeignCardCountResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxDailyPercentFCVolume" runat="server" meta:resourcekey="uxDailyPercentFCVolumeResource1" />%&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="ux7DaysPercentFCVolume" runat="server" meta:resourcekey="ux7DaysPercentFCVolumeResource1" />%&nbsp;
                        </td>
                    </tr>
                </table>
                </div>
            </div>
        </div>
    </div>
    <div class="col-md-5">
        <as:PlaceHolder ID="uxContainerContractual" runat="server" Visible="False">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="grid-title-small grid-toggle on-top" data-toggle="collapse" data-target="#uxContractual"><asp:Literal ID="Literal47" runat="server" Text="Contractual" meta:resourcekey="Literal47Resource1" /></h2>
                    <div class="in" id="uxContractual">
                        <table class="ASTable">
                            <colgroup>
                                <col style="width: 120px" />
                            </colgroup>
                            <tr class="Row">
                                <td class="heading"><asp:Literal ID="Literal48" runat="server" Text="Annual Volume:" meta:resourcekey="Literal48Resource1" />
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxAnnualBankCardVolume" runat="server" meta:resourcekey="uxAnnualBankCardVolumeResource1" />&nbsp;
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading"><asp:Literal ID="Literal49" runat="server" Text="Average Ticket:" meta:resourcekey="Literal49Resource1" />
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxAverageTicket" runat="server" meta:resourcekey="uxAverageTicketResource1" />&nbsp;
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading"><asp:Literal ID="Literal50" runat="server" Text="Keyed %:" meta:resourcekey="Literal50Resource1" />
                                </td>
                                <td class="text-right">
                                    <as:Literal ID="uxPercentKeyed" runat="server" meta:resourcekey="uxPercentKeyedResource1" />&nbsp;
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading"><asp:Literal ID="Literal51" runat="server" Text="Max Auth:" meta:resourcekey="Literal51Resource1" />
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxMaxAuth" runat="server" meta:resourcekey="uxMaxAuthResource1" />&nbsp;
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading"><asp:Literal ID="Literal52" runat="server" Text="Max Sale:" meta:resourcekey="Literal52Resource1" />
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxMaxSaleAmount" runat="server" meta:resourcekey="uxMaxSaleAmountResource1" />&nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </as:PlaceHolder>
        <as:PlaceHolder ID="uxContainerNewContractual" runat="server" Visible="False">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="grid-title-small grid-toggle on-top" data-toggle="collapse" data-target="#uxNewContractual"><asp:Literal ID="Literal53" runat="server" Text="Contractual" meta:resourcekey="Literal53Resource1" /></h2>
                    <div class="in" id="uxNewContractual">
                        <table class="ASTable">
                            <tr>
                                <th>&nbsp;
                                </th>
                                <th><asp:Literal ID="Literal54" runat="server" Text="Contractual" meta:resourcekey="Literal54Resource1" />
                                </th>
                                <th><asp:Literal ID="Literal55" runat="server" Text="Actual" meta:resourcekey="Literal55Resource1" />
                                </th>
                                <th><asp:Literal ID="Literal56" runat="server" Text="Monthly" meta:resourcekey="Literal56Resource1" />
                                </th>
                            </tr>
                            <tr class="Row">
                                <td class="heading"><asp:Literal ID="Literal57" runat="server" Text="Annual Volume:" meta:resourcekey="Literal57Resource1" />
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxAnnualVolContractual" runat="server" meta:resourcekey="uxAnnualVolContractualResource1" />&nbsp;
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxAnnualVolActual" runat="server" meta:resourcekey="uxAnnualVolActualResource1" />&nbsp;
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxAnnualVolMonthly" runat="server" meta:resourcekey="uxAnnualVolMonthlyResource1" />&nbsp;
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading"><asp:Literal ID="Literal58" runat="server" Text="Average Ticket:" meta:resourcekey="Literal58Resource1" />
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxAverageTicketContractual" runat="server" meta:resourcekey="uxAverageTicketContractualResource1" />&nbsp;
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxAverageTicketActual" runat="server" meta:resourcekey="uxAverageTicketActualResource1" />&nbsp;
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxAverageTicketMonthly" runat="server" meta:resourcekey="uxAverageTicketMonthlyResource1" />&nbsp;
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading"><asp:Literal ID="Literal59" runat="server" Text="Average #:" meta:resourcekey="Literal59Resource1" />
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxAverageContractual" runat="server" meta:resourcekey="uxAverageContractualResource1" />&nbsp;
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxAverageActual" runat="server" meta:resourcekey="uxAverageActualResource1" />&nbsp;
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxAverageMonthly" runat="server" meta:resourcekey="uxAverageMonthlyResource1" />&nbsp;
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading"><asp:Literal ID="Literal60" runat="server" Text="Max Sale:" meta:resourcekey="Literal60Resource1" />
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxMaxSaleContractual" runat="server" meta:resourcekey="uxMaxSaleContractualResource1" />&nbsp;
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxMaxSaleActual" runat="server" meta:resourcekey="uxMaxSaleActualResource1" />&nbsp;
                                </td>
                                <td class="text-right">
                                    <asp:Label ID="uxMaxSaleMonthly" runat="server" meta:resourcekey="uxMaxSaleMonthlyResource1" />&nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </as:PlaceHolder>
        <div class="row">
            <div class="col-md-12">
                <h2 class="grid-title-small grid-toggle" data-toggle="collapse" data-target="#uxKeyAnalusis"><asp:Literal ID="Literal61" runat="server" Text="Keyed Analysis" meta:resourcekey="Literal61Resource1" /></h2>
                <div class="horizontal-scrollable in" id="uxKeyAnalusis">
                <table class="ASTable">
                    <tr>
                        <th>&nbsp;
                        </th>
                        <th><asp:Literal ID="Literal62" runat="server" Text="Today" meta:resourcekey="Literal62Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal63" runat="server" Text="7 Days" meta:resourcekey="Literal63Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal64" runat="server" Text="MTD" meta:resourcekey="Literal64Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal65" runat="server" Text="YTD" meta:resourcekey="Literal65Resource1" />
                        </th>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><asp:Literal ID="Literal66" runat="server" Text="% of Trans" meta:resourcekey="Literal66Resource1" />
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxDailyPercentKeyedCount" runat="server" meta:resourcekey="uxDailyPercentKeyedCountResource1" />%&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="ux7DaysPercentKeyedCount" runat="server" meta:resourcekey="ux7DaysPercentKeyedCountResource1" />%&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxMTDPercentKeyedCount" runat="server" meta:resourcekey="uxMTDPercentKeyedCountResource1" />%&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxYTDPercentKeyedCount" runat="server" meta:resourcekey="uxYTDPercentKeyedCountResource1" />%&nbsp;
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><asp:Literal ID="Literal67" runat="server" Text="Volume" meta:resourcekey="Literal67Resource1" />
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxDailyKeyedVolume" runat="server" meta:resourcekey="uxDailyKeyedVolumeResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux7DaysKeyedVolume" runat="server" meta:resourcekey="ux7DaysKeyedVolumeResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxMTDKeyedVolume" runat="server" meta:resourcekey="uxMTDKeyedVolumeResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="uxYTDKeyedVolume" runat="server" meta:resourcekey="uxYTDKeyedVolumeResource1" />&nbsp;
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><asp:Literal ID="Literal68" runat="server" Text="# Trans" meta:resourcekey="Literal68Resource1" />
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxDailyKeyedCount" runat="server" meta:resourcekey="uxDailyKeyedCountResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="ux7DaysKeyedCount" runat="server" meta:resourcekey="ux7DaysKeyedCountResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxMTDKeyedCount" runat="server" meta:resourcekey="uxMTDKeyedCountResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <as:Literal ID="uxYTDKeyedCount" runat="server" meta:resourcekey="uxYTDKeyedCountResource1" />&nbsp;
                        </td>
                    </tr>
                </table>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h2 class="grid-title-small grid-toggle" data-toggle="collapse" data-target="#uxACHAnalysis"><asp:Literal ID="Literal69" runat="server" Text="ACH Analysis" meta:resourcekey="Literal69Resource1" /></h2>
                <div class="horizontal-scrollable in" id="uxACHAnalysis">
                <table class="ASTable">
                    <tr>
                        <th rowspan="2">&nbsp;
                        </th>
                        <th colspan="2"><asp:Literal ID="Literal70" runat="server" Text="7 Days" meta:resourcekey="Literal70Resource1" />
                        </th>
                        <th colspan="2"><asp:Literal ID="Literal71" runat="server" Text="6 Months" meta:resourcekey="Literal71Resource1" />
                        </th>
                    </tr>
                    <tr>
                        <th><asp:Literal ID="Literal72" runat="server" Text="Quantity" meta:resourcekey="Literal72Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal73" runat="server" Text="Amount" meta:resourcekey="Literal73Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal74" runat="server" Text="Quantity" meta:resourcekey="Literal74Resource1" />
                        </th>
                        <th><asp:Literal ID="Literal75" runat="server" Text="Amount" meta:resourcekey="Literal75Resource1" />
                        </th>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><asp:Literal ID="Literal76" runat="server" Text="Debits" meta:resourcekey="Literal76Resource1" />
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux7DaysQuantityDebits" runat="server" meta:resourcekey="ux7DaysQuantityDebitsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux7DaysAmountDebits" runat="server" meta:resourcekey="ux7DaysAmountDebitsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux6MonthsQuantityDebits" runat="server" meta:resourcekey="ux6MonthsQuantityDebitsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux6MonthsAmountDebits" runat="server" meta:resourcekey="ux6MonthsAmountDebitsResource1" />&nbsp;
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><asp:Literal ID="Literal77" runat="server" Text="Credits" meta:resourcekey="Literal77Resource1" />
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux7DaysQuantityCredits" runat="server" meta:resourcekey="ux7DaysQuantityCreditsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux7DaysAmountCredits" runat="server" meta:resourcekey="ux7DaysAmountCreditsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux6MonthsQuantityCredits" runat="server" meta:resourcekey="ux6MonthsQuantityCreditsResource1" />&nbsp;
                        </td>
                        <td class="text-right">
                            <asp:Label ID="ux6MonthsAmountCredits" runat="server" meta:resourcekey="ux6MonthsAmountCreditsResource1" />&nbsp;
                        </td>
                    </tr>
                </table>
                </div>
            </div>
        </div>
    </div>
</div>















