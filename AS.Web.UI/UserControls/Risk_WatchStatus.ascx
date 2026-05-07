<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_WatchStatus.ascx.cs"
    Inherits="UserControls_Risk_WatchStatus" %>


<div class="control-inline">
    <as:RadioButton ID="rdWatchStatusNA" runat="server" GroupName="rdWatchStatus" Checked="True"
        Text="N/A" meta:resourcekey="rdWatchStatusNAResource1" Value="" />
</div>
<div class="control-inline">
    <as:RadioButton ID="rdWatchStatusOn" runat="server" GroupName="rdWatchStatus"
        Text="Merchants on Watch" meta:resourcekey="rdWatchStatusOnResource1" Value=""/>
</div>
<div class="control-inline">
    <as:RadioButton ID="rdWatchStatusOff" runat="server" GroupName="rdWatchStatus"
        Text="Merchants Not on Watch " meta:resourcekey="rdWatchStatusOffResource1" Value=""/>
</div>

<as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
    <script type="text/javascript">
        var Risk_Assignment_Filters_rdWacthStatusNA = '<%= rdWatchStatusNA.ClientID %>';
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/risk/Risk_WatchStatus.js"></script>
</as:ASRadCodeBlock>
