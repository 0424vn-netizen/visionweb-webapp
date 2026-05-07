<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_Filter_Transactional.ascx.cs"
    Inherits="UserControls_rm_MCF_Assignment_Filter_Transactional" %>


<as:Validator ID="uxValidateTransactionalFilter" runat="server" MessageType="Inline"
    ValidationFunction="ValidateTransactionalFilterExt" MessageContainerClientID="" meta:resourcekey="uxValidateTransactionalFilterResource1">
    <Items>
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Required" ClientValidationFunction="Validate60dayTransCnt_Required" ControlToValidateID="txtBetweenFrom" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.To_Greater" ClientValidationFunction="Validate60dayTransCnt_Between_Greater" ControlToValidateID="txtBetweenFrom" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Required" ClientValidationFunction="ValidateContractualVol_Required" ControlToValidateID="txtContractualVol" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Required" ClientValidationFunction="ValidateTodaySaleAmount_Required" ControlToValidateID="txtBetweenFrom1" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.To_Greater" ClientValidationFunction="ValidateTodaySaleAmount_Between_Greater" ControlToValidateID="txtBetweenFrom1" />

    </Items>
</as:Validator>

<tr class="section-heading">
    <as:ASRadCodeBlock ID="ASRadCodeBlock7" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"3":"2"%>">
            <as:Literal ID="ltTransactional" runat="server" Text="Transactional" meta:resourcekey="ltTransactionalResource1"></as:Literal>
        </td>
    </as:ASRadCodeBlock>
</tr>
<tr class="AltRow">
    <td class="heading">
        <as:ValidatorLabel ID="VltLbl60dayTransCnt" runat="server" CssClass="control-label" Text="60 Day Transaction Count:" ApplyFor="txtBetweenFrom" meta:resourcekey="VltLbl60dayTransCntResource1"></as:ValidatorLabel>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock5" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <div class="control-inline">
                <as:CheckBox ID="cb60dayTransCnt" tracking-required="true" tracking-key="60DayTransactionCount" tracking-type="checkbox" runat="server" onclick="cb60dayTransCnt_Changed();" meta:resourcekey="cb60dayTransCntResource1" />
            </div>
            <div class="control-inline">
                <as:RadComboBox ID="cbxTransCntType" runat="server" Width="148px" OnClientSelectedIndexChanged="cbxTransCntType_OnClientSelectedIndexChanged"
                    MaxHeight="300" meta:resourcekey="cbxTransCntTypeResource1">
                </as:RadComboBox>
            </div>
            <div class="control-inline last">
                <div id="cidBetween" style="vertical-align: middle;">
                    <as:RadNumericTextBox ID="txtBetweenFrom" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="60DayTransactionCountBetweenFrom"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtBetweenFromResource1">
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

                    <label><as:Literal ID="ltAnd" runat="server" Text="And" meta:resourcekey="ltAndResource1"></as:Literal> </label>

                    <as:RadNumericTextBox ID="txtBetweenTo" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="60DayTransactionCountBetweenTo"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtBetweenToResource1">
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

                <div id="cidGreater">
                    <as:RadNumericTextBox ID="txtGt" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="60DayTransactionCountGreaterThan"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtGtResource1">
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
                <div id="cidLess">
                    <as:RadNumericTextBox ID="txtLt" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="60DayTransactionCountLessThan"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtLtResource1">
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
                <as:ValidatorMessage runat="server" ID="SixtydayTransCntMsg" ApplyFor="txtBetweenFrom" />
            </div>
        </td>
    </as:ASRadCodeBlock>
</tr>
<tr class="Row">
    <td class="heading">
        <as:ValidatorLabel ID="TodaySaleAmount" runat="server" CssClass="control-label" ApplyFor="txtBetweenFrom1" meta:resourcekey="todaySaleAmountResource1"></as:ValidatorLabel>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">

            <div class="control-inline">
                <as:CheckBox ID="chbTodaySaleAmount" tracking-required="true" tracking-key="TodaySalesAmount" tracking-type="checkbox" runat="server" onclick="todaySaleAmount_Changed();" />
            </div>
            <div class="control-inline">
                <as:RadComboBox ID="cbTodaySaleAmount" runat="server" Width="148px" OnClientSelectedIndexChanged="cbTodaySaleAmount_OnClientSelectedIndexChanged" MaxHeight="300">
                </as:RadComboBox>
            </div>
            <div class="control-inline last">
                <div id="cidBetween1" style="vertical-align: middle;">
                    <as:RadNumericTextBox ID="txtBetweenFrom1" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="TodaySalesAmountBetweenFrom"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" >
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

                    <label><as:Literal ID="Literal1" runat="server" Text="And" meta:resourcekey="ltAndResource1"></as:Literal> </label>

                    <as:RadNumericTextBox ID="txtBetweenTo1" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="TodaySalesAmountBetweenTo"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtBetweenToResource1">
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

                <div id="cidGreater1">
                    <as:RadNumericTextBox ID="txtGreaterThan1" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="TodaySalesAmountGreaterThan"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtGtResource1">
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
                <div id="cidLess1">
                    <as:RadNumericTextBox ID="txtLessThan1" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="TodaySalesAmountLessThan"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtLtResource1">
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
                <as:ValidatorMessage runat="server" ID="vlTodaySaleAmount" ApplyFor="txtBetweenFrom1" />
            </div>
        </td>
    </as:ASRadCodeBlock>
</tr>

<tr class="AltRow">
    <td class="heading">
        <as:ValidatorLabel ID="VltLblContractualVol" runat="server" CssClass="control-label" Text="Today’s Total Sales Amount Ratio:" ApplyFor="txtContractualVol" meta:resourcekey="VltLblContractualVolResource1"></as:ValidatorLabel>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock2" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
            <div class="control-inline">
                <as:CheckBox ID="cbContractualVol" runat="server" onclick="cbContractualVol_Changed();" meta:resourcekey="cbContractualVolResource1" />
            </div>
            <div class="control-inline" style="width: 150px;">
                <label class="control-label">
                    <as:Literal ID="Literal2" runat="server" Text=">=" meta:resourcekey="Literal2Resource1"></as:Literal>
                </label>
            </div>
            <div class="control-inline last">
                <span>
                    <as:RadNumericTextBox ID="txtContractualVol" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" tracking-key="SalesContractualVolume"
                        MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtContractualVolResource1">
                        <NegativeStyle Resize="None"></NegativeStyle>

                        <NumberFormat PositivePattern="n" NegativePattern="-n" DecimalDigits="0" />

                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                        <FocusedStyle Resize="None"></FocusedStyle>

                        <DisabledStyle Resize="None"></DisabledStyle>

                        <InvalidStyle Resize="None"></InvalidStyle>

                        <HoveredStyle Resize="None"></HoveredStyle>

                        <EnabledStyle Resize="None"></EnabledStyle>
                    </as:RadNumericTextBox>
                </span>
                <label class="control-label last">
                    <as:Literal ID="Literal3" runat="server" Text="% of the Contractual Daily Net Amount" meta:resourcekey="Literal3Resource1"></as:Literal>
                </label>                               
            </div>
           <div style="padding-left:266px;">
                <as:Literal ID="Literal4" runat="server" meta:resourcekey="Literal4Resource1"></as:Literal>
               <div>
                   <as:ValidatorMessage runat="server" ID="ContractualVolValidatorMessage" ApplyFor="txtContractualVol" />
               </div>
            </div>
        </td>
    </as:ASRadCodeBlock>
</tr>

<as:ASRadCodeBlock ID="ASRadCodeBlock3" runat="server">
    <script type="text/javascript">
        var Risk_Assignment_Filter_Transactional_cb60dayTransCnt = '<%= cb60dayTransCnt.ClientID %>';
        var Risk_Assignment_Filter_Transactional_txtBetweenFrom = '<%= txtBetweenFrom.ClientID %>';
        var Risk_Assignment_Filter_Transactional_txtBetweenTo = '<%= txtBetweenTo.ClientID %>';
        var Risk_Assignment_Filter_Transactional_txtGt = '<%= txtGt.ClientID %>';
        var Risk_Assignment_Filter_Transactional_txtLt = '<%= txtLt.ClientID %>';
        var Risk_Assignment_Filter_Transactional_cbxTransCntType = '<%=cbxTransCntType.ClientID %>';
        var Risk_Assignment_Filter_Transactional_cbContractualVol = '<%= cbContractualVol.ClientID %>';
        var Risk_Assignment_Filter_Transactional_txtContractualVol = '<%= txtContractualVol.ClientID %>';
        var Risk_Assignment_Filter_Transactional_SixtydayTransCntMsg = '<%= SixtydayTransCntMsg.ClientID %>';

        var Risk_Assignment_Filter_Transactional_chbTodaySaleAmount = '<%= chbTodaySaleAmount.ClientID %>';
        var Risk_Assignment_Filter_Transactional_cbTodaySaleAmount = '<%= cbTodaySaleAmount.ClientID %>';
        var Risk_Assignment_Filter_Transactional_txtBetweenFrom1 = '<%= txtBetweenFrom1.ClientID %>';
        var Risk_Assignment_Filter_Transactional_txtBetweenTo1 = '<%= txtBetweenTo1.ClientID %>';
        var Risk_Assignment_Filter_Transactional_txtGreaterThan1 = '<%= txtGreaterThan1.ClientID %>';
        var Risk_Assignment_Filter_Transactional_txtLessThan1 = '<%= txtLessThan1.ClientID %>';

    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_Filter_Transactional.js"></script>

</as:ASRadCodeBlock>
