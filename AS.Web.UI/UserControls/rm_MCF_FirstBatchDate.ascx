<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_FirstBatchDate.ascx.cs" Inherits="UserControls_rm_MCF_FirstBatchDate" %>

<table id="tbl-filter-date">
    <tr>
        <td><as:CheckBox ID="chkIsNewMerchantOnly1" runat="server" onclick="FirstBatchNewMerchantOnlyChecked()" Text="New Merchant Only" tracking-key="FirstBatchDateNewMerchantOnly" tracking-type="checkbox" 
            tracking-refer="cbNewMerchantOnly1" tracking-refer-type="combobox" meta:resourcekey="chkIsNewMerchantOnlyResource1" /></td>
        <td><as:ASRadComboBox ID="cbNewMerchantOnly1" runat="server" Width="250px" DataTextField="DataText" DataValueField="DataKey" Enabled="false" /></td>
    </tr>
    <tr>
        <td><as:CheckBox ID="chkIsEstablishedMerchants1" runat="server" onclick="FirstBatchEstablishedMerchantsChecked()" Text="Established Merchants" tracking-key="FirstBatchDateEstablishedMerchants" tracking-type="checkbox" 
            tracking-refer="cbEstablishedMerchants1" tracking-refer-type="combobox" meta:resourcekey="chkIsEstablishedMerchantsResource1" /></td>
        <td><as:ASRadComboBox ID="cbEstablishedMerchants1" runat="server" Width="250px" DataTextField="DataText" DataValueField="DataKey" Enabled="false" /></td>
    </tr>
</table>

<as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
    <script type="text/javascript">
        var Risk_FistBatchDate_chkIsNewMerchantOnly = '<%= chkIsNewMerchantOnly1.ClientID %>';
        var Risk_FistBatchDate_chkIsEstablishedMerchants = '<%= chkIsEstablishedMerchants1.ClientID %>';
        var Risk_FistBatchDate_cbNewMerchantOnly = '<%= cbNewMerchantOnly1.ClientID %>';
        var Risk_FistBatchDate_cbEstablishedMerchants = '<%= cbEstablishedMerchants1.ClientID %>';
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_FirstBatchDate.js"></script>
</as:ASRadCodeBlock>
