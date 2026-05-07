<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_RiskCategory.ascx.cs"
    Inherits="UserControls_rm_MCF_RiskCategory" %>


<div class="control-inline">
    <as:CheckBox ID="rdRiskCategoryLow" CssClass="text-requeue-all" runat="server" GroupName="chkRiskCategory" tracking-key="RiskCategory" tracking-type="checkbox-text"
        Text="Low" meta:resourcekey="rdRiskCategoryAllResource1" Value="" />
</div>
<div class="control-inline">
    <as:CheckBox ID="rdRiskCategoryModerate" CssClass="text-requeue-all" runat="server" GroupName="chkRiskCategory" tracking-key="RiskCategory" tracking-type="checkbox-text"
        Text="Moderate" meta:resourcekey="rdRiskCategoryYesResource1" Value=""/>
</div>
<div class="control-inline">
    <as:CheckBox ID="rdRiskCategoryHigh" CssClass="text-requeue-all" runat="server" GroupName="chkRiskCategory" tracking-key="RiskCategory" tracking-type="checkbox-text"
        Text="High" meta:resourcekey="rdRiskCategoryAllResource1" Value="" />
</div>
