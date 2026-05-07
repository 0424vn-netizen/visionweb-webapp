<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_AutoApprovedIndicator.ascx.cs"
    Inherits="UserControls_rm_MCF_AutoApprovedIndicator" %>


<div class="control-inline">
    <as:RadioButton ID="rdAutoApprovedIndicatorNA" runat="server" GroupName="rdAutoApprovedIndicator" Checked="True" tracking-key="AutoApproved" tracking-type="radio-text"
        Text="All" meta:resourcekey="rdAutoApprovedIndicatorAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:RadioButton ID="rdAutoApprovedIndicatorYes" runat="server" GroupName="rdAutoApprovedIndicator" tracking-key="AutoApproved" tracking-type="radio-text"
        Text="Yes " meta:resourcekey="rdAutoApprovedIndicatorYesResource1" Value=""/>
</div>
<div class="control-inline">
    <as:RadioButton ID="rdAutoApprovedIndicatorNo" runat="server" GroupName="rdAutoApprovedIndicator" tracking-key="AutoApproved" tracking-type="radio-text"
        Text="No " meta:resourcekey="rdAutoApprovedIndicatorNoResource1" Value=""/>
</div>

<as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
    <script type="text/javascript">
        var Risk_Assignment_Filters_rdAutoApprovedIndicatorNA = '<%= rdAutoApprovedIndicatorNA.ClientID %>';
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_AutoApprovedIndicator.js"></script>
</as:ASRadCodeBlock>
