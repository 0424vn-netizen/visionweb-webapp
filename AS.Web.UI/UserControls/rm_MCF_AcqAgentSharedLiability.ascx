<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_AcqAgentSharedLiability.ascx.cs"
    Inherits="UserControls_rm_MCF_AcqAgentSharedLiability" %>


<div class="control-inline">
    <as:RadioButton ID="rdAcqAgentSharedLiabilityNA" CssClass="test" runat="server" GroupName="rdAcqAgentSharedLiability" Checked="True" tracking-key="AgentSharedLiability" tracking-type="radio-text"
        Text="All" meta:resourcekey="rdAcqAgentSharedLiabilityAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:RadioButton ID="rdAcqAgentSharedLiabilityYes" runat="server" GroupName="rdAcqAgentSharedLiability" tracking-key="AgentSharedLiability" tracking-type="radio-text"
        Text="Yes " meta:resourcekey="rdAcqAgentSharedLiabilityYesResource1" Value=""/>
</div>
<div class="control-inline">
    <as:RadioButton ID="rdAcqAgentSharedLiabilityNo" runat="server" GroupName="rdAcqAgentSharedLiability" tracking-key="AgentSharedLiability" tracking-type="radio-text"
        Text="No "  meta:resourcekey="rdAcqAgentSharedLiabilityNoResource1" Value=""/>
</div>

<as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
    <script type="text/javascript">
        var Risk_Assignment_Filters_rdAcqAgentSharedLiabilityNA = '<%= rdAcqAgentSharedLiabilityNA.ClientID %>';
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_AcqAgentSharedLiability.js"></script>
</as:ASRadCodeBlock>
