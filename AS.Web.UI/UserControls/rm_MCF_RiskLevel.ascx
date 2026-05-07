<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_RiskLevel.ascx.cs"
    Inherits="UserControls_rm_MCF_RiskLevel" %>


<div class="control-inline">
    <as:CheckBox ID="rdRiskLevelLowLv1" CssClass="text-requeue-all" runat="server" GroupName="chkRiskLevel" tracking-key="RiskLevel" tracking-type="checkbox-text"
        Text="Level 1 Low" meta:resourcekey="rdRiskLevelAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:CheckBox ID="rdRiskLevelHighLv1" CssClass="text-requeue-all" runat="server" GroupName="rdRiskLevel" tracking-key="RiskLevel" tracking-type="checkbox-text"
        Text="Level 1 High" meta:resourcekey="rdRiskLevelYesResource1" Value=""/>
</div>
<div class="control-inline">
    <as:CheckBox ID="rdRiskLevelLowLv2" CssClass="text-requeue-all" runat="server" GroupName="chkRiskLevel" tracking-key="RiskLevel" tracking-type="checkbox-text"
        Text="Level 2 Low" meta:resourcekey="rdRiskLevelAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:CheckBox ID="rdRiskLevelHighLv2" CssClass="text-requeue-all" runat="server" GroupName="rdRiskLevel" tracking-key="RiskLevel" tracking-type="checkbox-text"
        Text="Level 2 High" meta:resourcekey="rdRiskLevelYesResource1" Value=""/>
</div>
