<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_Filter_Extend.ascx.cs"
    Inherits="UserControls_rm_MCF_Assignment_Filter_DaystoFund" %>


<as:Validator ID="uxValidateDaystoFundFilter" runat="server" MessageType="Inline"
    ValidationFunction="ValidateDaystoFundFilterExt" MessageContainerClientID="" meta:resourcekey="uxValidateDaystoFundFilterResource1">
    <Items>
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Required" ClientValidationFunction="ValidateDaystoFund_Required" ControlToValidateID="txtDTFBetweenFrom" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.To_Greater" ClientValidationFunction="ValidateDaystoFund_Between_Greater" ControlToValidateID="txtDTFBetweenFrom" />
    </Items>
</as:Validator>

<as:PlaceHolder ID="pnlFutureDeliveryIndicator" runat="server">
    <tr>
        <td class="heading">
            <label class="control-label">
                <as:Literal ID="Literal38" runat="server" Text="Future Delivery Indicator:" meta:resourcekey="FutureDeliveryIndicatorResource1"></as:Literal></label>
        </td>

        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">

            <div class="control-inline">
                <as:RadioButton ID="rdWatchStatusNA" runat="server" GroupName="rdWatchStatus" Checked="True" tracking-key="FutureDeliveryIndicator" tracking-type="radio-text" tracking-more="-1"
                    Text="All" meta:resourcekey="rdWatchStatusAllResource1" Value="" />
            </div>
            <div class="control-inline">
                <as:RadioButton ID="rdWatchStatusOn" runat="server" GroupName="rdWatchStatus" tracking-key="FutureDeliveryIndicator" tracking-type="radio-text" tracking-more="1"
                    Text="Merchants on Watch" meta:resourcekey="rdWatchStatusOnResource1" Value="" />
            </div>
            <div class="control-inline">
                <as:RadioButton ID="rdWatchStatusOff" runat="server" GroupName="rdWatchStatus" tracking-key="FutureDeliveryIndicator" tracking-type="radio-text" tracking-more="0"
                    Text="Merchants Not on Watch " meta:resourcekey="rdWatchStatusOffResource1" Value="" />
            </div>

        </td>

    </tr>
</as:PlaceHolder>


<asp:Panel runat="server" ID="pnlDaystoFund">
    <tr>
        <td class="heading">
            <as:ValidatorLabel ID="VltLblDaystoFund" runat="server" CssClass="control-label" Text="Days to Fund:" ApplyFor="txtDTFBetweenFrom" meta:resourcekey="VltLblDaystoFundResource1"></as:ValidatorLabel>
        </td>

        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <div class="control-inline">
                <as:CheckBox ID="cbDaystoFund" tracking-required="true" tracking-key="DaystoFund" tracking-type="checkbox" runat="server" onclick="cbDaystoFund_Changed();" meta:resourcekey="cbDaystoFundResource1" />
            </div>
            <div class="control-inline">
                <as:RadComboBox ID="cbxDaystoFund" runat="server" Width="148px" OnClientSelectedIndexChanged="cbxDaystoFund_OnClientSelectedIndexChanged"
                    MaxHeight="300" meta:resourcekey="cbxDaystoFundResource1">
                </as:RadComboBox>
            </div>
            <div class="control-inline last">
                <div id="cidDTFBetween" style="vertical-align: middle;">
                    <as:RadNumericTextBox ID="txtDTFBetweenFrom" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="DaystoFundBetweenFrom"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtDTFBetweenFromResource1">
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
                        <as:Literal ID="ltDTFAnd" runat="server" Text="And" meta:resourcekey="ltAndResource1"></as:Literal>
                    </label>

                    <as:RadNumericTextBox ID="txtDTFBetweenTo" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="DaystoFundBetweenTo"
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

                <div id="cidDTFGreater">
                    <as:RadNumericTextBox ID="txtDTFGt" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="DaystoFundGreaterThan"
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
                <div id="cidDTFLess">
                    <as:RadNumericTextBox ID="txtDTFLt" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="DaystoFundLessThan"
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
                <as:ValidatorMessage runat="server" ID="SixtydayTransCntMsg" ApplyFor="txtDTFBetweenFrom" />
            </div>
        </td>

    </tr>
</asp:Panel>


<asp:Panel ID="pnCreditScoreCustom" runat="server">
    <%--Visible="false"--%>
    <tr class="AltRow">
        <td class="heading">
            <as:Literal ID="Literal39" runat="server" Text="Credit Score:" meta:resourcekey="Literal33Resource1"></as:Literal></td>
        <td>
            <table>
                <tr>
                    <td rowspan="2">
                        <as:Literal ID="Literal40" runat="server" Text="FICO Credit Score" meta:resourcekey="Literal35Resource1"></as:Literal>
                    </td>
                    <td>
                        <as:CheckBox ID="uxChkIsFromCustom" Width="50px" runat="server" onclick="visibleCreditScoreCb(this, 'uxCbCreditScoreFromCustom')" Text=""
                             tracking-key="CreditScoreGreater" tracking-type="checkbox" tracking-refer="uxCbCreditScoreFromCustom" tracking-refer-type="combobox" meta:resourcekey="lbIsGreater" />
                        <as:ASRadComboBox ID="uxCbCreditScoreFromCustom" runat="server" DataTextField="DataText" DataValueField="DataKey" Enabled="false" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <as:CheckBox ID="uxChkIsToCustom" Width="50px" runat="server" onclick="visibleCreditScoreCb(this, 'uxCbCreditScoreToCustom')" Text=""
                            tracking-key="CreditScoreSmaller" tracking-type="checkbox" tracking-refer="uxCbCreditScoreToCustom" tracking-refer-type="combobox" meta:resourcekey="lbIsSmaller" />
                        <as:ASRadComboBox ID="uxCbCreditScoreToCustom" runat="server" DataTextField="DataText" DataValueField="DataKey" Enabled="false" />
                    </td>
                </tr>
            </table>
        </td>
        <as:PlaceHolder runat="server" ID="plhCreditScoreCustom">
            <td></td>
        </as:PlaceHolder>
    </tr>
</asp:Panel>


<as:ASRadCodeBlock ID="ASRadCodeBlock3" runat="server">
    <script type="text/javascript">
        var Risk_Assignment_Filter_DaystoFund_cbxDaystoFund = '<%=cbxDaystoFund.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_cbDaystoFund = '<%= cbDaystoFund.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFrom = '<%= txtDTFBetweenFrom.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_txtDTFBetweenTo = '<%= txtDTFBetweenTo.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_txtDTFGt = '<%= txtDTFGt.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_txtDTFLt = '<%= txtDTFLt.ClientID %>';
        var Risk_Assignment_Filter_DaystoFund_SixtydayTransCntMsg = '<%= SixtydayTransCntMsg.ClientID %>';

        var Risk_Assignment_Filters_rdFutureDeliveryStatusNA = '<%= rdWatchStatusNA.ClientID %>';

        var Risk_Assignment_Filters_uxChkIsFromCustom = '<%= uxChkIsFromCustom.ClientID%>';
        var Risk_Assignment_Filters_uxChkIsToCustom = '<%= uxChkIsToCustom.ClientID%>';

    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_Filter_Extend.js"></script>

</as:ASRadCodeBlock>
