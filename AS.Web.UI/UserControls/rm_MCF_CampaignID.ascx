<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_CampaignID.ascx.cs"
    Inherits="UserControls_rm_MCF_CampaignID" %>

<as:Validator ID="uxValidatorCampaign" runat="server" MessageType="Inline"
    ValidationFunction="validateRiskCampaignID" MessageContainerClientID="" meta:resourcekey="ValidatorCampaignResource1">
    <Items>
        <as:BasicValidationItem Rule="Number" ResMessage="Resources.ValMsg.Number" ControlToValidateID="txtCompaignID" />
        <as:CustomValidationItem ResMessage="Resources.LanguageResource.RiskScoreFromRequired" ClientValidationFunction="ValidateRiskCampaignRequired" ControlToValidateID="txtCompaignID" />
    </Items>
</as:Validator>

<tr class="Row">
    <td class="heading">
        <as:ValidatorLabel ID="Literal40" runat="server" CssClass="control-label" ApplyFor="txtCompaignID" Text="Campaign ID:" meta:resourcekey="CampaignID" />
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock13" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            
            <div id="markupCompaign" />
            <div class="control-inline">
                <as:CheckBox ID="cbCompaignID" runat="server" onclick="cbCampaignID_Changed();" />
            </div>
            <div class="control-inline last">
                <div id="cidCompaignID">
                    <as:RadNumericTextBox ID="txtCompaignID" runat="server" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="CompaignID"
                        MaxLength="15" CssClass="form-control">
                        <NegativeStyle Resize="None"></NegativeStyle>
                        
                        <NumberFormat DecimalDigits="0" ZeroPattern="0" />

                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                        <FocusedStyle Resize="None"></FocusedStyle>

                        <DisabledStyle Resize="None"></DisabledStyle>

                        <InvalidStyle Resize="None"></InvalidStyle>

                        <HoveredStyle Resize="None"></HoveredStyle>

                        <EnabledStyle Resize="None"></EnabledStyle>
                    </as:RadNumericTextBox>
                </div>
            </div>
            <div class="control-inline">
                <as:ValidatorMessage runat="server" ID="txtCompaignIDMsg" ApplyFor="txtCompaignID" ShowOnLoad="False" />
            </div>
        </td>
    </as:ASRadCodeBlock>
</tr>


<as:ASRadCodeBlock ID="ASRadCodeBlock3" runat="server">
    <script type="text/javascript">
        var Risk_Assignment_Filter_CampaignID_cbCampaignID = '<%= cbCompaignID.ClientID %>';
        var Risk_Assignment_Filter_CampaignID_txtCampaignID = '<%= txtCompaignID.ClientID %>';

    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_Filter_CampaignID.js"></script>

</as:ASRadCodeBlock>
