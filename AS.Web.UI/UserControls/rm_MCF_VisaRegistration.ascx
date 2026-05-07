<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_VisaRegistration.ascx.cs"
    Inherits="UserControls_rm_MCF_VisaRegistration" %>


<div class="control-inline">
    <as:RadioButton ID="rdVisaRegistrationNA" runat="server" GroupName="rdVisaRegistration" Checked="True" tracking-key="VisaRegistration" tracking-type="radio-text"
        Text="All" meta:resourcekey="rdVisaRegistrationAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:RadioButton ID="rdVisaRegistrationYes" runat="server" GroupName="rdVisaRegistration" tracking-key="VisaRegistration" tracking-type="radio-text"
        Text="Yes " meta:resourcekey="rdVisaRegistrationYesResource1" Value=""/>
</div>
<div class="control-inline">
    <as:RadioButton ID="rdVisaRegistrationNo" runat="server" GroupName="rdVisaRegistration" tracking-key="VisaRegistration" tracking-type="radio-text"
        Text="No " meta:resourcekey="rdVisaRegistrationNoResource1" Value=""/>
</div>
