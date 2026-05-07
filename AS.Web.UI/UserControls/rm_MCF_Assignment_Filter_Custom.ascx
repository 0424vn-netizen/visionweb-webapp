<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_Filter_Custom.ascx.cs"
    Inherits="UserControls_rm_MCF_Assignment_Filter_Custom" %>


<%@ Register Src="~/UserControls/rm_MCF_AcqAgentSharedLiability.ascx" TagName="AcqAgentSharedLiability" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_AMEXRegistration.ascx" TagName="AMEXRegistration" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_AutoApprovedIndicator.ascx" TagName="AutoApprovedIndicator" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_CBproducing.ascx" TagName="CBproducing" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_ComplianceRegulatoryReputational.ascx" TagName="ComplianceRegulatoryReputational" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_DiscoverRegistration.ascx" TagName="DiscoverRegistration" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_HighNDX.ascx" TagName="HighNDX" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_MCRegistration.ascx" TagName="MCRegistration" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_PersonalGuarantee.ascx" TagName="PersonalGuarantee" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_PRINUSForeign.ascx" TagName="PRINUSForeign" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_VisaRegistration.ascx" TagName="VisaRegistration" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/rm_MCF_FilterRiskLevel.ascx" TagName="FilterRiskLevel" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterRiskCategory.ascx" TagName="FilterRiskCategory" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterCampaignID.ascx" TagName="FilterCampaignID" TagPrefix="uc" %>


<as:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    <ajaxsettings>
        <tek:AjaxSetting AjaxControlID="uxRebindRiskLevel">
            <updatedcontrols>
                <tek:AjaxUpdatedControl ControlID="divRiskLevel" />
            </updatedcontrols>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxRebindRiskCategory">
            <updatedcontrols>
                <tek:AjaxUpdatedControl ControlID="divRiskCategory" />
            </updatedcontrols>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxRebindRiskCampaignID">
            <updatedcontrols>
                <tek:AjaxUpdatedControl ControlID="divRiskCampaignID" />
            </updatedcontrols>
        </tek:AjaxSetting>
    </ajaxsettings>
</as:RadAjaxManagerProxy>

<tr class="Row">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="ltAutoApprovedIndicator" runat="server" Text="Auto-Approved Indicator:" meta:resourcekey="AutoApprovedIndicator" />
        </label>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock9" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <uc:AutoApprovedIndicator ID="uxAutoApprovedIndicator" runat="server"></uc:AutoApprovedIndicator>
        </td>
    </as:ASRadCodeBlock>
</tr>

<as:PlaceHolder ID="plhAcqAgentSharedLiability" runat="server">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="ltAcqAgentSharedLiability" runat="server" Text="Acq Agent Shared Liability:" meta:resourcekey="AcqAgentSharedLiability" />
        </label>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock10" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <uc:AcqAgentSharedLiability ID="uxAcqAgentSharedLiability" runat="server"></uc:AcqAgentSharedLiability>
        </td>
    </as:ASRadCodeBlock>
</as:PlaceHolder>

<tr class="Row">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="ltrRiskLevel" runat="server" Text="Risk Level:" meta:resourcekey="RiskLevel" />
        </label>
    </td>
    <td>
        <div>
            <div id="divRiskLevel" runat="server">
                <asp:Label ID="lblRiskLevel" tracking-key="RiskLevel" tracking-type="hierarchy" runat="server" Text="N/A" meta:resourcekey="lblRiskLevelResource1"></asp:Label>
            </div>
        </div>
    </td>
    <as:PlaceHolder runat="server" ID="plhRiskLevel">
        <td class="action-column">
            <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_RiskLevel_Modal.aspx', 480, 725); return false;">
                <as:Literal ID="Literal9" runat="server" Text="Edit" meta:resourcekey="Literal9Resource1"></as:Literal>
            </a>
        </td>
    </as:PlaceHolder>
</tr>

<tr class="AltRow">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="ltrRiskCategory" runat="server" Text="Risk Category:" meta:resourcekey="RiskCategory" />
        </label>
    </td>
    <td>
        <div>
            <div id="divRiskCategory" runat="server">
                <asp:Label ID="lblRiskCategory" tracking-key="RiskCategory" tracking-type="hierarchy" runat="server" Text="N/A" meta:resourcekey="lblRiskCategoryResource1"></asp:Label>
            </div>
        </div>
    </td>
    <as:PlaceHolder runat="server" ID="plhRiskCategory">
        <td class="action-column">
            <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_RiskCategory_Modal.aspx', 480, 725); return false;">
                <as:Literal ID="Literal3" runat="server" Text="Edit" meta:resourcekey="Literal9Resource1"></as:Literal>
            </a>
        </td>
    </as:PlaceHolder>
</tr>

<tr class="Row">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="ltrCampaignID" runat="server" Text="Campaign ID:" meta:resourcekey="CampaignID" />
        </label>
    </td>
    <td>
        <div>
            <div id="divCampaignID" runat="server">
                <asp:Label ID="lblCampaignID" tracking-key="CampaignID" tracking-type="hierarchy" runat="server" Text="N/A" meta:resourcekey="lblRiskCategoryResource1"></asp:Label>
            </div>
        </div>
    </td>
    <as:PlaceHolder runat="server" ID="plhCampaignID">
        <td class="action-column">
            <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_CampaignID_Modal.aspx', 480, 725); return false;">
                <as:Literal ID="Literal1" runat="server" Text="Edit" meta:resourcekey="Literal9Resource1"></as:Literal>
            </a>
        </td>
    </as:PlaceHolder>
</tr>

<as:PlaceHolder ID="PlaceHolder2" runat="server">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="Literal41" runat="server" Text="Visa Registration:" meta:resourcekey="VisaRegistration" />
        </label>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock14" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <uc:VisaRegistration ID="uxVisaRegistration" runat="server"></uc:VisaRegistration>
        </td>
    </as:ASRadCodeBlock>
</as:PlaceHolder>

<tr class="Row">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="Literal42" runat="server" Text="MC Registration:" meta:resourcekey="MCRegistration" />
        </label>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock15" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <uc:MCRegistration ID="uxMCRegistration" runat="server"></uc:MCRegistration>
        </td>
    </as:ASRadCodeBlock>
</tr>

<as:PlaceHolder ID="PlaceHolder3" runat="server">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="Literal43" runat="server" Text="Discover Registration:" meta:resourcekey="DiscoverRegistration" />
        </label>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock16" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <uc:DiscoverRegistration ID="uxDiscoverRegistration" runat="server"></uc:DiscoverRegistration>
        </td>
    </as:ASRadCodeBlock>
</as:PlaceHolder>

<tr class="Row">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="Literal44" runat="server" Text="AMEX Registration:" meta:resourcekey="AMEXRegistration" />
        </label>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock17" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <uc:AMEXRegistration ID="uxAMEXRegistration" runat="server"></uc:AMEXRegistration>
        </td>
    </as:ASRadCodeBlock>
</tr>

<as:PlaceHolder ID="PlaceHolder4" runat="server">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="Literal45" runat="server" Text="High NDX:" meta:resourcekey="HighNDX" />
        </label>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock18" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <uc:HighNDX ID="uxHighNDX" runat="server"></uc:HighNDX>
        </td>
    </as:ASRadCodeBlock>
</as:PlaceHolder>

<tr class="Row">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="Literal46" runat="server" Text="CB producing:" meta:resourcekey="CBproducing" />
        </label>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock19" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <uc:CBproducing ID="uxCBproducing" runat="server"></uc:CBproducing>
        </td>
    </as:ASRadCodeBlock>
</tr>

<tr class="AltRow">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="Literal47" runat="server" Text="Compliance/regulatory/ reputational:" meta:resourcekey="ComplianceRegulatoryReputational" />
        </label>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock20" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <uc:ComplianceRegulatoryReputational ID="uxComplianceRegulatoryReputational" runat="server"></uc:ComplianceRegulatoryReputational>
        </td>
    </as:ASRadCodeBlock>
</tr>

<tr class="Row">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="Literal48" runat="server" Text="PRIN US vs Foreign:" meta:resourcekey="PRINUSForeign" />
        </label>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock21" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <uc:PRINUSForeign ID="uxPRINUSForeign" runat="server"></uc:PRINUSForeign>
        </td>
    </as:ASRadCodeBlock>
</tr>

<as:PlaceHolder ID="PlaceHolder6" runat="server">
    <td class="heading">
        <label class="control-label">
            <as:Literal ID="Literal49" runat="server" Text="Personal Guarantee Y or N:" meta:resourcekey="PersonalGuarantee" />
        </label>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock22" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <uc:PersonalGuarantee ID="uxPersonalGuarantee" runat="server"></uc:PersonalGuarantee>
        </td>
    </as:ASRadCodeBlock>
</as:PlaceHolder>

<div class="display-none">
    <!--invisible buttons-->
    <as:Button ID="uxRebindRiskLevel" runat="server" OnClick="uxRebindRiskLevel_Click" IsStandardButton="False" meta:resourcekey="uxRebindRiskLevelResource1" />
    <as:Button ID="uxRebindRiskCategory" runat="server" OnClick="uxRebindRiskCategory_Click" IsStandardButton="False" meta:resourcekey="uxRebindRiskCategoryResource1" />
    <as:Button ID="uxRebindCampaignID" runat="server" OnClick="uxRebindCampaignID_Click" IsStandardButton="False" meta:resourcekey="uxRebindCampaignIDResource1" />
</div>

<as:ASRadCodeBlock ID="ASRadCodeBlock3" runat="server">
    <script type="text/javascript">

        var Risk_Assignment_Info_uxRebindRiskLevel = '<%= uxRebindRiskLevel.ClientID %>';
        var Risk_Assignment_Info_uxRebindRiskCategory = '<%= uxRebindRiskCategory.ClientID %>';
        var Risk_Assignment_Info_uxRebindCampaignID = '<%= uxRebindCampaignID.ClientID %>';

    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_Filter_CampaignID.js"></script>

</as:ASRadCodeBlock>
