<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_DiscoverRegistration.ascx.cs"
    Inherits="UserControls_rm_MCF_DiscoverRegistration" %>


<div class="control-inline">
    <as:RadioButton ID="rdDiscoverRegistrationNA" runat="server" GroupName="rdDiscoverRegistration" Checked="True" tracking-key="DiscoverRegistration" tracking-type="radio-text"
        Text="All" meta:resourcekey="rdDiscoverRegistrationAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:RadioButton ID="rdDiscoverRegistrationYes" runat="server" GroupName="rdDiscoverRegistration" tracking-key="DiscoverRegistration" tracking-type="radio-text"
        Text="Yes " meta:resourcekey="rdDiscoverRegistrationYesResource1" Value=""/>
</div>
<div class="control-inline">
    <as:RadioButton ID="rdDiscoverRegistrationNo" runat="server" GroupName="rdDiscoverRegistration" tracking-key="DiscoverRegistration" tracking-type="radio-text"
        Text="No " meta:resourcekey="rdDiscoverRegistrationNoResource1" Value=""/>
</div>