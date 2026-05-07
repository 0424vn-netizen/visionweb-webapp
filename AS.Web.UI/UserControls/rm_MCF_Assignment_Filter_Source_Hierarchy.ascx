<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_Filter_Source_Hierarchy.ascx.cs"
    Inherits="UserControls_rm_MCF_Assignment_Filter_Source_Hierarchy" %>

<%@ Register Src="~/UserControls/rm_MCF_Filter_Lead_Source.ascx" TagName="LeadSource" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Filter_Referral_Source.ascx" TagName="ReferralSource" TagPrefix="uc" %>
<as:RadAjaxManagerProxy ID="RadAjaxManagerProxyReview" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="btnRefreshLeadSource">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divLeadSource" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshReferralSource">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divReferralSource" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>
<as:PlaceHolder ID="uxGroupSourceHierachyEdit" runat="server" Visible="false">
    <tr class="section-heading">
        <td colspan="3">
            <as:Literal ID="uxltlSourceHierachyEdit" runat="server" Text="Source - Hierarchy" meta:resourcekey="uxltlSourceHierachyEdit"></as:Literal>
        </td>
    </tr>
</as:PlaceHolder>
<as:PlaceHolder ID="uxGroupSourceHierachyView" runat="server"  Visible="false">
    <tr class="section-heading">
        <td colspan="2">
            <as:Literal ID="uxltlSourceHierachyView" runat="server" Text="Source - Hierarchy" meta:resourcekey="uxltlSourceHierachyEdit"></as:Literal>
        </td>
    </tr>
</as:PlaceHolder>
<as:PlaceHolder ID="uxLeadSource" runat="server" Visible="False">
    <tr class="Row">
        <td class="heading">
            <as:Literal ID="uxLableReadSource" runat="server" Text="Lead Source:" meta:resourcekey="uxLableReadSource"></as:Literal>
        </td>
        <td>
            <div>
                <div id="divLeadSource" runat="server">
                    <asp:Label  tracking-key="LeadSource" tracking-type="hierarchy" ID="uxltlLeadSource" runat="server" Text="N/A" meta:resourcekey="uxltlLeadSourceReSource"></asp:Label>
                </div>
            </div>
        </td>
        <as:PlaceHolder runat="server" ID="uxplhLeadSource">
            <td class="action-column">
                <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_Lead_Source_Modal.aspx', 480, 725); return false;">
                    <as:Literal ID="Literal1" runat="server" Text="Edit" meta:resourcekey="uxResourceEdit"></as:Literal>
                </a>
            </td>
        </as:PlaceHolder>
    </tr>
</as:PlaceHolder>
<as:PlaceHolder ID="uxReferralSource" runat="server" Visible="False">
    <tr class="AltRow">
        <td class="heading">
            <as:Literal ID="uxLableReferralSource" runat="server" Text="Referral Source:" meta:resourcekey="uxLableReferralSource"></as:Literal>
        </td>
        <td>
            <div>
                <div id="divReferralSource" runat="server">
                    <asp:Label  tracking-key="ReferralSource" tracking-type="hierarchy" ID="uxltlReferralSource" runat="server" Text="N/A" meta:resourcekey="uxltlReferralSourceReSource"></asp:Label>
                </div>
            </div>
        </td>
        <as:PlaceHolder runat="server" ID="uxplhReferralSource">
            <td class="action-column">
                <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_Referral_Source_Modal.aspx', 480, 725); return false;">
                    <as:Literal ID="Literal4" runat="server" Text="Edit" meta:resourcekey="uxResourceEdit"></as:Literal>
                </a>
            </td>
        </as:PlaceHolder>
    </tr>
</as:PlaceHolder>
<div class="display-none">
    <as:Button ID="btnRefreshLeadSource" runat="server" OnClick="btnRefreshLeadSource_Click" IsStandardButton="False" />
    <as:Button ID="btnRefreshReferralSource" runat="server" OnClick="btnRefreshReferralSource_Click" IsStandardButton="False" />
</div>
<as:ASRadCodeBlock ID="ASRadCodeBlock3" runat="server">
    <script type="text/javascript">
        var rm_MCF_Assignment_Filter_Source_Hierarchy_btnRefreshLeadSource = '<%= btnRefreshLeadSource.ClientID %>';
        var rm_MCF_Assignment_Filter_Source_Hierarchy_btnRefreshReferralSource = '<%= btnRefreshReferralSource.ClientID %>';
        var rm_MCF_Assignment_Filter_Lead_Source = '<%= divLeadSource.ClientID%>';
        var rm_MCF_Assigment_Filter_Referal_Source = '<%= divReferralSource.ClientID%>'
        
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_Filter_Source_Hierarchy.js"></script>
</as:ASRadCodeBlock>
