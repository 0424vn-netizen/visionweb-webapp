<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_HighNDX.ascx.cs"
    Inherits="UserControls_rm_MCF_HighNDX" %>


<div class="control-inline">
    <as:RadioButton ID="rdHighNDXNA" runat="server" GroupName="rdHighNDX" Checked="True" tracking-key="NonDeliveryExposure" tracking-type="radio-text"
        Text="All" meta:resourcekey="rdHighNDXAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:RadioButton ID="rdHighNDXYes" runat="server" GroupName="rdHighNDX" tracking-key="NonDeliveryExposure" tracking-type="radio-text"
        Text="Yes " meta:resourcekey="rdHighNDXYesResource1" Value=""/>
</div>
<div class="control-inline">
    <as:RadioButton ID="rdHighNDXNo" runat="server" GroupName="rdHighNDX" tracking-key="NonDeliveryExposure" tracking-type="radio-text"
        Text="No " meta:resourcekey="rdHighNDXNoResource1" Value=""/>
</div>