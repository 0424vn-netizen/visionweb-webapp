<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_WatchStatus.ascx.cs"
    Inherits="UserControls_rm_MCF_WatchStatus" %>


<div class="control-inline">
    <as:RadioButton ID="rdWatchStatusNA" runat="server" GroupName="rdWatchStatus" Checked="True" tracking-key="WatchStatus" tracking-type="radio-text" tracking-more="2"
        Text="All" meta:resourcekey="rdWatchStatusAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:RadioButton ID="rdWatchStatusOn" runat="server" GroupName="rdWatchStatus" tracking-key="WatchStatus" tracking-type="radio-text" tracking-more="1"
        Text="Merchants on Watch" meta:resourcekey="rdWatchStatusOnResource1" Value=""/>
</div>
<div class="control-inline">
    <as:RadioButton ID="rdWatchStatusOff" runat="server" GroupName="rdWatchStatus" tracking-key="WatchStatus" tracking-type="radio-text" tracking-more="0"
        Text="Merchants Not on Watch " meta:resourcekey="rdWatchStatusOffResource1" Value=""/>
</div>

<as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
    <script type="text/javascript">
        var Risk_Assignment_Filters_rdWacthStatusNA = '<%= rdWatchStatusNA.ClientID %>';
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_WatchStatus.js"></script>
</as:ASRadCodeBlock>
