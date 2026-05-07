<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_ApprovalDate.ascx.cs" Inherits="UserControls_Risk_ApprovalDate" %>

<table id="tbl-filter-date">
    <tr>
        <td><as:CheckBox ID="chkIsNewMerchantOnly" runat="server" onclick="NewMerchantOnlyChecked()" Text="New Merchant Only" meta:resourcekey="chkIsNewMerchantOnlyResource1" /></td>
        <td><as:ASRadComboBox ID="cbNewMerchantOnly" runat="server" Width="250px" DataTextField="DataText" DataValueField="DataKey" Enabled="false" /></td>
    </tr>
    <tr>
        <td><as:CheckBox ID="chkIsEstablishedMerchants" runat="server" onclick="EstablishedMerchantsChecked()" Text="Established Merchants" meta:resourcekey="chkIsEstablishedMerchantsResource1" /></td>
        <td><as:ASRadComboBox ID="cbEstablishedMerchants" runat="server" Width="250px" DataTextField="DataText" DataValueField="DataKey" Enabled="false" /></td>
    </tr>
</table>

<as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
    <script type="text/javascript">
        var Risk_ApprovalDate_chkIsNewMerchantOnly = '<%= chkIsNewMerchantOnly.ClientID %>';
        var Risk_ApprovalDate_chkIsEstablishedMerchants = '<%= chkIsEstablishedMerchants.ClientID %>';
        var Risk_ApprovalDate_cbNewMerchantOnly = '<%= cbNewMerchantOnly.ClientID %>';
        var Risk_ApprovalDate_cbEstablishedMerchants = '<%= cbEstablishedMerchants.ClientID %>';
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk/Risk_ApprovalDate.js"></script>
</as:ASRadCodeBlock>
