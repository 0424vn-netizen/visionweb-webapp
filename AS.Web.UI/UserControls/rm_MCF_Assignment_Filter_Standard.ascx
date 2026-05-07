<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_Filter_Standard.ascx.cs" Inherits="UserControls_rm_MCF_Assignment_Filter_Standard" %>

<%@ Register Src="~/UserControls/rm_MCF_FilterRiskRating.ascx" TagName="FilterRiskRating" TagPrefix="uc" %>

<as:RadAjaxManagerProxy ID="RadAjaxManagerProxyReview2" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxRebindRiskRating">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divRiskRatingStandard" />
                <tek:AjaxUpdatedControl ControlID="hdnCountSelectedFilter" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<as:Validator ID="uxValidateDaystoFundFilterStandard" runat="server" MessageType="Inline"
    ValidationFunction="ValidateDaystoFundFilterStandard" MessageContainerClientID="" meta:resourcekey="uxValidateDaystoFundFilterResource1">
    <Items>
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Required" ClientValidationFunction="ValidateDaystoFundStandard_Required" ControlToValidateID="txtDTFBetweenFromStandard" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.To_Greater" ClientValidationFunction="ValidateDaystoFundStandard_Between_Greater" ControlToValidateID="txtDTFBetweenFromStandard" />
    </Items>
</as:Validator>

<as:PlaceHolder ID="pnlRiskRatingStandard" runat="server">
    <tr>
        <td class="heading">
            <as:Literal ID="Literal38" runat="server" Text="Risk Rating:"></as:Literal></td>
        <td>
            <div id="divRiskRatingStandard" runat="server">
                <asp:Label tracking-key="RiskRatingStandard" tracking-type="hierarchy" ID="lblRiskRating" runat="server" Text="N/A"></asp:Label>
            </div>
        </td>

        <td class="action-column" runat="server" id="pnlRiskRatingStandardEdit">
            <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_RiskRating_Modal.aspx', 480, 725); return false;">
                <as:Literal ID="Literal1" runat="server" Text="Edit"></as:Literal>
            </a>
        </td>
    </tr>
</as:PlaceHolder>

<asp:Panel ID="pnCreditScoreStandard" runat="server">
    <tr class="AltRow">
        <td class="heading">
            <as:Literal ID="Literal39" runat="server" Text="Credit Score:" meta:resourcekey="Literal33Resource1"></as:Literal></td>
        <td>
            <table>
                <tr>
                    <td rowspan="2">
                        <as:Literal ID="Literal40" runat="server" Text="Credit Score"></as:Literal>
                    </td>
                    <td>
                        <as:CheckBox ID="uxChkIsFromStandard" Width="50px" runat="server" onclick="visibleCreditScoreCb(this, 'uxCbCreditScoreFromStandard')" Text=""
                            tracking-key="CreditScoreStandardGreater" tracking-type="checkbox" tracking-refer="uxCbCreditScoreFromStandard" tracking-refer-type="combobox" meta:resourcekey="lbIsGreater" />
                        <as:ASRadComboBox ID="uxCbCreditScoreFromStandard" runat="server" DataTextField="DataText" DataValueField="DataKey" Enabled="false" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <as:CheckBox ID="uxChkIsToStandard" Width="50px" runat="server" onclick="visibleCreditScoreCb(this, 'uxCbCreditScoreToStandard')" Text=""
                            tracking-key="CreditScoreStandardSmaller" tracking-type="checkbox" tracking-refer="uxCbCreditScoreToStandard" tracking-refer-type="combobox" meta:resourcekey="lbIsSmaller" />
                        <as:ASRadComboBox ID="uxCbCreditScoreToStandard" runat="server" DataTextField="DataText" DataValueField="DataKey" Enabled="false" />
                    </td>
                </tr>
            </table>
        </td>
        <as:PlaceHolder runat="server" ID="plhCreditScoreStandard">
            <td></td>
        </as:PlaceHolder>
    </tr>
</asp:Panel>

<asp:Panel runat="server" ID="pnlDaystoFundStandard">
    <tr>
        <td class="heading">
            <as:ValidatorLabel ID="VltLblDaystoFundStandard" runat="server" CssClass="control-label" Text="Days to Fund:" ApplyFor="txtDTFBetweenFromStandard" meta:resourcekey="VltLblDaystoFundStandardResource1"></as:ValidatorLabel>
        </td>

        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <div class="control-inline">
                <as:CheckBox ID="cbDaystoFundStandard" tracking-required="true" tracking-key="DaystoFundStandard" tracking-type="checkbox" runat="server" onclick="cbDaystoFundStandard_Changed();" meta:resourcekey="cbDaystoFundStandardResource1" />
            </div>
            <div class="control-inline">
                <as:RadComboBox ID="cbxDaystoFundStandard" runat="server" Width="148px" OnClientSelectedIndexChanged="cbxDaystoFundStandard_OnClientSelectedIndexChanged"
                    MaxHeight="300" meta:resourcekey="cbxDaystoFundStandardResource1">
                </as:RadComboBox>
            </div>
            <div class="control-inline last">
                <div id="cidDTFBetweenStandard" style="vertical-align: middle;">
                    <as:RadNumericTextBox ID="txtDTFBetweenFromStandard" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="DaystoFundStandardBetweenFrom"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtDTFBetweenFromStandardResource1">
                        <NegativeStyle Resize="None"></NegativeStyle>

                        <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" />

                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                        <FocusedStyle Resize="None"></FocusedStyle>

                        <DisabledStyle Resize="None"></DisabledStyle>

                        <InvalidStyle Resize="None"></InvalidStyle>

                        <HoveredStyle Resize="None"></HoveredStyle>

                        <EnabledStyle Resize="None"></EnabledStyle>
                    </as:RadNumericTextBox>

                    <label>
                        <as:Literal ID="ltDTFAndStandard" runat="server" Text="And" meta:resourcekey="ltAndResource1"></as:Literal>
                    </label>

                    <as:RadNumericTextBox ID="txtDTFBetweenToStandard" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="DaystoFundStandardBetweenTo"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtDTFBetweenToResource1">
                        <NegativeStyle Resize="None"></NegativeStyle>

                        <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" />

                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                        <FocusedStyle Resize="None"></FocusedStyle>

                        <DisabledStyle Resize="None"></DisabledStyle>

                        <InvalidStyle Resize="None"></InvalidStyle>

                        <HoveredStyle Resize="None"></HoveredStyle>

                        <EnabledStyle Resize="None"></EnabledStyle>
                    </as:RadNumericTextBox>
                </div>

                <div id="cidDTFGreaterStandard">
                    <as:RadNumericTextBox ID="txtDTFGtStandard" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="DaystoFundStandardGreaterThan"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtDTFGtResource1">
                        <NegativeStyle Resize="None"></NegativeStyle>

                        <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" />

                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                        <FocusedStyle Resize="None"></FocusedStyle>

                        <DisabledStyle Resize="None"></DisabledStyle>

                        <InvalidStyle Resize="None"></InvalidStyle>

                        <HoveredStyle Resize="None"></HoveredStyle>

                        <EnabledStyle Resize="None"></EnabledStyle>
                    </as:RadNumericTextBox>
                </div>
                <div id="cidDTFLessStandard">
                    <as:RadNumericTextBox ID="txtDTFLtStandard" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="DaystoFundStandardLessThan"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtDTFLtResource1">
                        <NegativeStyle Resize="None"></NegativeStyle>

                        <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" />

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
                <as:ValidatorMessage runat="server" ID="SixtydayTransCntMsgStandard" ApplyFor="txtDTFBetweenFromStandard" />
            </div>
        </td>

    </tr>
</asp:Panel>

<div class="display-none">
    <as:Button ID="uxRebindRiskRating" runat="server" OnClick="uxRebindRiskRating_Click" IsStandardButton="False" meta:resourcekey="uxRebindRiskRatingResource1" />
</div>


<as:ASRadCodeBlock ID="ASRadCodeBlock3" runat="server">
    <script type="text/javascript">
        var Risk_Assignment_Filter_DaystoFund_cbxDaystoFundStandard = '<%=cbxDaystoFundStandard.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_cbDaystoFundStandard = '<%= cbDaystoFundStandard.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFromStandard = '<%= txtDTFBetweenFromStandard.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_txtDTFBetweenToStandard = '<%= txtDTFBetweenToStandard.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_txtDTFGtStandard = '<%= txtDTFGtStandard.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_txtDTFLtStandard = '<%= txtDTFLtStandard.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_SixtydayTransCntMsgStandard = '<%= SixtydayTransCntMsgStandard.ClientID %>';

        var Risk_Assignment_Filters_uxChkIsFromStandard = '<%= uxChkIsFromStandard.ClientID%>';
        var Risk_Assignment_Filters_uxChkIsToStandard = '<%= uxChkIsToStandard.ClientID%>';
        var Risk_Assignment_Filters_uxRebindRiskRating = '<%= uxRebindRiskRating.ClientID %>';

    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_Filter_Standard.js"></script>
</as:ASRadCodeBlock>
