<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Parameter_MerchantFilter.ascx.cs"
    Inherits="UserControls_rm_MCF_Parameter_MerchantFilter" %>
<style type="text/css">
    .tbMerchantFilter td
    {
        border-width: 0px !important;
    }
    .cellwidth
    {
        width: 30px;
    }
</style>
<table border="0" cellpadding="0" cellspacing="0" class="tbMerchantFilter">
    <tr>
        <td>
            <as:Literal ID="ltFrom" runat="server" Text="From" meta:resourcekey="ltFromResource1"></as:Literal>
        </td>
        <td class="cellwidth">
            <as:TextBox ID="txtFrom" runat="server" Width="100%" MaxLength="16" onkeypress="return txtParameterValue_KeyPress(event);"
                onblur="return txtParameterValue_Blur(event);" onfocus="return txtParameterValue_Focus(event);" HintCss="hint" meta:resourcekey="txtFromResource1"></as:TextBox>
        </td>
        <td>
            <as:Literal ID="Literal1" runat="server" Text="To" meta:resourcekey="Literal1Resource1"></as:Literal>
        </td>
        <td class="cellwidth">
            <as:TextBox ID="txtTo" runat="server" Width="100%" MaxLength="16" onkeypress="return txtParameterValue_KeyPress(event);"
                onblur="return txtParameterValue_Blur(event);" onfocus="return txtParameterValue_Focus(event);" HintCss="hint" meta:resourcekey="txtToResource1"></as:TextBox>
        </td>
        <td>
            <as:Literal ID="Literal2" runat="server" Text="days" meta:resourcekey="Literal2Resource1"></as:Literal>
        </td>
    </tr>
</table>
