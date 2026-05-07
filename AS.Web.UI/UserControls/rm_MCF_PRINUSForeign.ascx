<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_PRINUSForeign.ascx.cs"
    Inherits="UserControls_rm_MCF_PRINUSForeign" %>


<div class="control-inline">
    <as:RadioButton ID="rdPRINUSForeignNA" runat="server" GroupName="rdPRINUSForeign" Checked="True" tracking-key="USPrincipal" tracking-type="radio-text"
        Text="All" meta:resourcekey="rdPRINUSForeignAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:RadioButton ID="rdPRINUSForeignYes" runat="server" GroupName="rdPRINUSForeign" tracking-key="USPrincipal" tracking-type="radio-text"
        Text="Yes " meta:resourcekey="rdPRINUSForeignYesResource1" Value=""/>
</div>
<div class="control-inline">
    <as:RadioButton ID="rdPRINUSForeignNo" runat="server" GroupName="rdPRINUSForeign" tracking-key="USPrincipal" tracking-type="radio-text"
        Text="No " meta:resourcekey="rdPRINUSForeignNoResource1" Value=""/>
</div>