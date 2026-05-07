<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RiskACHRejectAnalysis.ascx.cs" Inherits="UserControls_RiskACHRejectAnalysis" %>

<div id="header">
    <table class="info" cellspacing="0" cellpadding="0" width="100%">
        <tr>
            <th class="largetitle-corner-topleft" colspan="4" ><as:Literal ID="ltACHAnalysis" runat="server" Text="ACH Analysis" meta:resourcekey="ltACHAnalysisResource1"></as:Literal></th>
                    <th class="largetitle-corner-topright"> </th>    
        </tr>
        <tr class="caption_top">
            <td rowspan="2" class="left" >&nbsp;</td>
            <td colspan="2"  class="title"><as:Literal ID="Literal1" runat="server" Text="7 Days" meta:resourcekey="Literal1Resource1"></as:Literal></td>
            <td colspan="2"  class="title"><as:Literal ID="Literal2" runat="server" Text="6 Months" meta:resourcekey="Literal2Resource1"></as:Literal></td>
        </tr>
        <tr class="caption valuecenter">
            <td><as:Literal ID="Literal3" runat="server" Text="Quantity" meta:resourcekey="Literal3Resource1"></as:Literal></td>
            <td><as:Literal ID="Literal4" runat="server" Text="Amount" meta:resourcekey="Literal4Resource1"></as:Literal></td>
            <td><as:Literal ID="Literal5" runat="server" Text="Quantity" meta:resourcekey="Literal5Resource1"></as:Literal></td>
            <td><as:Literal ID="Literal6" runat="server" Text="Amount" meta:resourcekey="Literal6Resource1"></as:Literal></td>
        </tr>
        <tr>
            <td class="caption left"><as:Literal ID="Literal7" runat="server" Text="Debits" meta:resourcekey="Literal7Resource1"></as:Literal></td>
            <td class="valueright"><as:Literal ID="ux7DaysQuantityDebits" runat="server" meta:resourcekey="ux7DaysQuantityDebitsResource1" />&nbsp;</td>
            <td class="valueright"><as:Literal ID="ux7DaysAmountDebits" runat="server" meta:resourcekey="ux7DaysAmountDebitsResource1" />&nbsp;</td>
            <td class="valueright"><as:Literal ID="ux6MonthsQuantityDebits" runat="server" meta:resourcekey="ux6MonthsQuantityDebitsResource1" />&nbsp;</td>
            <td class="valueright"><as:Literal ID="ux6MonthsAmountDebits" runat="server" meta:resourcekey="ux6MonthsAmountDebitsResource1" />&nbsp;</td>
        </tr>
        <tr class="AltRow">
            <td class="caption left"><as:Literal ID="Literal8" runat="server" Text="Credits" meta:resourcekey="Literal8Resource1"></as:Literal></td>
            <td class="valueright"><as:Literal ID="ux7DaysQuantityCredits" runat="server" meta:resourcekey="ux7DaysQuantityCreditsResource1" />&nbsp;</td>
            <td class="valueright"><as:Literal ID="ux7DaysAmountCredits" runat="server" meta:resourcekey="ux7DaysAmountCreditsResource1" />&nbsp;</td>
            <td class="valueright"><as:Literal ID="ux6MonthsQuantityCredits" runat="server" meta:resourcekey="ux6MonthsQuantityCreditsResource1" />&nbsp;</td>
            <td class="valueright"><as:Literal ID="ux6MonthsAmountCredits" runat="server" meta:resourcekey="ux6MonthsAmountCreditsResource1" />&nbsp;</td>
        </tr>
    </table>
</div>
