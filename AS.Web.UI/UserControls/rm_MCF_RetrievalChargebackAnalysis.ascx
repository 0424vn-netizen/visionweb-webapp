<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_RetrievalChargebackAnalysis.ascx.cs" Inherits="UserControls_rm_MCF_RetrievalChargebackAnalysis" %>

<div id="header">
    <table class="info" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr class="filter_top_bg">
            <th class="largetitle-corner-topleft"colspan="6" class="border_bottom" width="100%" ><as:Literal ID="ltRtCb" runat="server" Text="Retrieval/Chargeback Analysis" meta:resourcekey="ltRtCbResource1"></as:Literal></th>
            <th class="largetitle-corner-topright"> </th>           
        </tr>
        <tr class="caption_top">
            <td rowspan="2" width="15%" class="left" >
                &nbsp;
            </td>
            <td colspan="3" class="title" width="42%">
                <as:Literal ID="Literal1" runat="server" Text="Chargebacks" meta:resourcekey="Literal1Resource1"></as:Literal>
            </td>
            <td colspan="3" class="title" width="42%">
                <as:Literal ID="Literal2" runat="server" Text="Retrievals" meta:resourcekey="Literal2Resource1"></as:Literal>
            </td>
        </tr>
        <tr class="caption valuecenter">
            <td width="14%">
                <as:Literal ID="Literal3" runat="server" Text="Quantity" meta:resourcekey="Literal3Resource1"></as:Literal>
            </td>
            <td width="14%">
                <as:Literal ID="Literal4" runat="server" Text="Amount" meta:resourcekey="Literal4Resource1"></as:Literal>
            </td>
            <td width="14%">
                <as:Literal ID="Literal5" runat="server" Text="Ratio to Sales" meta:resourcekey="Literal5Resource1"></as:Literal>
            </td>
             <td width="14%">
                <as:Literal ID="Literal6" runat="server" Text="Quantity" meta:resourcekey="Literal6Resource1"></as:Literal>
            </td>
            <td width="14%">
                <as:Literal ID="Literal7" runat="server" Text="Amount" meta:resourcekey="Literal7Resource1"></as:Literal>
            </td>
            <td width="14%">
                <as:Literal ID="Literal8" runat="server" Text="Ratio to Sales" meta:resourcekey="Literal8Resource1"></as:Literal>
            </td>
        </tr>
        <tr>
            <td class="caption left" width="100px">
                <as:Literal ID="Literal9" runat="server" Text="Daily" meta:resourcekey="Literal9Resource1"></as:Literal>
            </td>
            <td class="valueright">
                <as:Literal ID="uxDailyQuantityChargebacks" runat="server" meta:resourcekey="uxDailyQuantityChargebacksResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxDailyAmountChargebacks" runat="server" meta:resourcekey="uxDailyAmountChargebacksResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxDailyRatioSalesChargebacks" runat="server" meta:resourcekey="uxDailyRatioSalesChargebacksResource1" />%&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxDailyQuantityRetrievals" runat="server" meta:resourcekey="uxDailyQuantityRetrievalsResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxDailyAmountRetrievals" runat="server" meta:resourcekey="uxDailyAmountRetrievalsResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxDailyRatioSalesRetrievals" runat="server" meta:resourcekey="uxDailyRatioSalesRetrievalsResource1" />%&nbsp;
            </td>
        </tr>
        <tr class="AltRow">
            <td class="caption left">
                <as:Literal ID="Literal10" runat="server" Text="MTD" meta:resourcekey="Literal10Resource1"></as:Literal>
            </td>
            <td class="valueright">
                <as:Literal ID="uxMTDQuantityChargebacks" runat="server" meta:resourcekey="uxMTDQuantityChargebacksResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxMTDAmountChargebacks" runat="server" meta:resourcekey="uxMTDAmountChargebacksResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxMTDRatioSalesChargebacks" runat="server" meta:resourcekey="uxMTDRatioSalesChargebacksResource1" />%&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxMTDQuantityRetrievals" runat="server" meta:resourcekey="uxMTDQuantityRetrievalsResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxMTDAmountRetrievals" runat="server" meta:resourcekey="uxMTDAmountRetrievalsResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxMTDRatioSalesRetrievals" runat="server" meta:resourcekey="uxMTDRatioSalesRetrievalsResource1" />%&nbsp;
            </td>
        </tr>
        <tr>
            <td class="caption left">
                <as:Literal ID="Literal11" runat="server" Text="3 Months" meta:resourcekey="Literal11Resource1"></as:Literal>
            </td>
            <td class="valueright">
                <as:Literal ID="ux3MonthsQuantityChargebacks" runat="server" meta:resourcekey="ux3MonthsQuantityChargebacksResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="ux3MonthsAmountChargebacks" runat="server" meta:resourcekey="ux3MonthsAmountChargebacksResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="ux3MonthsRatioSalesChargebacks" runat="server" meta:resourcekey="ux3MonthsRatioSalesChargebacksResource1" />%&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="ux3MonthsQuantityRetrievals" runat="server" meta:resourcekey="ux3MonthsQuantityRetrievalsResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="ux3MonthsAmountRetrievals" runat="server" meta:resourcekey="ux3MonthsAmountRetrievalsResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="ux3MonthsRatioSalesRetrievals" runat="server" meta:resourcekey="ux3MonthsRatioSalesRetrievalsResource1" />%&nbsp;
            </td>
        </tr>
        <tr class="AltRow">
            <td class="caption left">
                <as:Literal ID="Literal12" runat="server" Text="YTD" meta:resourcekey="Literal12Resource1"></as:Literal>
            </td>
            <td class="valueright">
                <as:Literal ID="uxYTDQuantityChargebacks" runat="server" meta:resourcekey="uxYTDQuantityChargebacksResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxYTDAmountChargebacks" runat="server" meta:resourcekey="uxYTDAmountChargebacksResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxYTDRatioSalesChargebacks" runat="server" meta:resourcekey="uxYTDRatioSalesChargebacksResource1" />%&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxYTDQuantityRetrievals" runat="server" meta:resourcekey="uxYTDQuantityRetrievalsResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxYTDAmountRetrievals" runat="server" meta:resourcekey="uxYTDAmountRetrievalsResource1" />&nbsp;
            </td>
            <td class="valueright">
                <as:Literal ID="uxYTDRatioSalesRetrievals" runat="server" meta:resourcekey="uxYTDRatioSalesRetrievalsResource1" />%&nbsp;
            </td>
        </tr>
    </table>
</div>
