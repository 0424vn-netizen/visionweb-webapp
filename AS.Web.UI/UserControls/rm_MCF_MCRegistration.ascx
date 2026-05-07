<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MCRegistration.ascx.cs"
    Inherits="UserControls_rm_MCF_MCRegistration" %>


<div class="control-inline">
    <as:RadioButton ID="rdMCRegistrationNA" runat="server" GroupName="rdMCRegistration" Checked="True" tracking-key="MCRegistration" tracking-type="radio-text"
        Text="All" meta:resourcekey="rdMCRegistrationAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:RadioButton ID="rdMCRegistrationYes" runat="server" GroupName="rdMCRegistration" tracking-key="MCRegistration" tracking-type="radio-text"
        Text="Yes " meta:resourcekey="rdMCRegistrationYesResource1" Value=""/>
</div>
<div class="control-inline">
    <as:RadioButton ID="rdMCRegistrationNo" runat="server" GroupName="rdMCRegistration" tracking-key="MCRegistration" tracking-type="radio-text"
        Text="No " meta:resourcekey="rdMCRegistrationNoResource1" Value=""/>
</div>
