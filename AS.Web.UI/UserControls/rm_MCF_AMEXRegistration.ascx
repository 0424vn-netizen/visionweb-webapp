<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_AMEXRegistration.ascx.cs"
    Inherits="UserControls_rm_MCF_AMEXRegistration" %>


<div class="control-inline">
    <as:RadioButton ID="rdAMEXRegistrationNA" runat="server" GroupName="rdAMEXRegistration" Checked="True" tracking-key="AMEXRegistration" tracking-type="radio-text"
        Text="All" meta:resourcekey="rdAMEXRegistrationAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:RadioButton ID="rdAMEXRegistrationYes" runat="server" GroupName="rdAMEXRegistration" tracking-key="AMEXRegistration" tracking-type="radio-text"
        Text="Yes " meta:resourcekey="rdAMEXRegistrationYesResource1" Value=""/>
</div>
<div class="control-inline">
    <as:RadioButton ID="rdAMEXRegistrationNo" runat="server" GroupName="rdAMEXRegistration" tracking-key="AMEXRegistration" tracking-type="radio-text"
        Text="No " meta:resourcekey="rdAMEXRegistrationNoResource1" Value=""/>
</div>