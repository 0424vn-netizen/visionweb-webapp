<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_PersonalGuarantee.ascx.cs"
    Inherits="UserControls_rm_MCF_PersonalGuarantee" %>


<div class="control-inline">
    <as:RadioButton ID="rdPersonalGuaranteeNA" runat="server" GroupName="rdPersonalGuarantee" Checked="True" tracking-key="PersonalGuarantee" tracking-type="radio-text"
        Text="All" meta:resourcekey="rdPersonalGuaranteeAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:RadioButton ID="rdPersonalGuaranteeYes" runat="server" GroupName="rdPersonalGuarantee" tracking-key="PersonalGuarantee" tracking-type="radio-text"
        Text="Yes " meta:resourcekey="rdPersonalGuaranteeYesResource1" Value=""/>
</div>
<div class="control-inline">
    <as:RadioButton ID="rdPersonalGuaranteeNo" runat="server" GroupName="rdPersonalGuarantee" tracking-key="PersonalGuarantee" tracking-type="radio-text"
        Text="No " meta:resourcekey="rdPersonalGuaranteeNoResource1" Value=""/>
</div>