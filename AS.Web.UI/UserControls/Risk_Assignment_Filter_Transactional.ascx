<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Risk_Assignment_Filter_Transactional.ascx.cs"
    Inherits="UserControls_Risk_Assignment_Filter_Transactional" %>


<as:Validator ID="uxValidateTransactionalFilter" runat="server" MessageType="Inline"
    ValidationFunction="ValidateTransactionalFilterExt" MessageContainerClientID="" meta:resourcekey="uxValidateTransactionalFilterResource1">
    <Items>
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Required" ClientValidationFunction="Validate60dayTransCnt_Required" ControlToValidateID="txtBetweenFrom" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.To_Greater" ClientValidationFunction="Validate60dayTransCnt_Between_Greater" ControlToValidateID="txtBetweenFrom" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Required" ClientValidationFunction="ValidateMinimumVol_Required" ControlToValidateID="txtMinimumVol" />
        <as:CustomValidationItem ResMessage="Resources.ValMsg.Required" ClientValidationFunction="ValidateContractualVol_Required" ControlToValidateID="txtContractualVol" />

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
                <as:CheckBox ID="cb60dayTransCnt" runat="server" onclick="cb60dayTransCnt_Changed();" meta:resourcekey="cb60dayTransCntResource1" />
            </div>
            <div class="control-inline">
                <as:RadComboBox ID="cbxTransCntType" runat="server"
                    Width="148px" OnClientSelectedIndexChanged="cbxTransCntType_OnClientSelectedIndexChanged"
                    MaxHeight="300" meta:resourcekey="cbxTransCntTypeResource1">
                </as:RadComboBox>
            </div>
            <div class="control-inline last">
                <div id="cidBetween" style="vertical-align: middle;">
                    <as:RadNumericTextBox ID="txtBetweenFrom" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);"
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

                    <label>
                        <as:Literal ID="ltAnd" runat="server" Text="And" meta:resourcekey="ltAndResource1"></as:Literal>
                    </label>

                    <as:RadNumericTextBox ID="txtBetweenTo" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);"
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
                    <as:RadNumericTextBox ID="txtGt" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);"
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
                    <as:RadNumericTextBox ID="txtLt" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);"
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
        <as:ValidatorLabel ID="VltLblMinimumVol" runat="server" CssClass="control-label" Text="Minimum Volume:" ApplyFor="txtMinimumVol" meta:resourcekey="VltLblMinimumVolResource1"></as:ValidatorLabel>
    </td>
    <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
        <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">

            <div class="control-inline">
                <as:CheckBox ID="cbMinimumVol" runat="server" onclick="cbMinimumVol_Changed();" meta:resourcekey="cbMinimumVolResource1" />
            </div>
            <div class="control-inline" style="width: 140px;">
                <label class="control-label last">
                    <as:Literal ID="Literal1" runat="server" Text=">=" meta:resourcekey="Literal1Resource1"></as:Literal>
                </label>

            </div>
            <div class="control-inline last">
                <span>
                    <label class="control-label last"><%=SessionManager.CurrencySymbol %></label>
                    <as:RadNumericTextBox ID="txtMinimumVol"
                        runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);" MaxLength="9" CssClass="form-control" LabelCssClass="" LabelWidth="64px" meta:resourcekey="txtMinimumVolResource1">
                        <NegativeStyle Resize="None"></NegativeStyle>

                        <NumberFormat PositivePattern="n" NegativePattern="n" DecimalDigits="0" />

                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                        <FocusedStyle Resize="None"></FocusedStyle>

                        <DisabledStyle Resize="None"></DisabledStyle>

                        <InvalidStyle Resize="None"></InvalidStyle>

                        <HoveredStyle Resize="None"></HoveredStyle>

                        <EnabledStyle Resize="None"></EnabledStyle>
                    </as:RadNumericTextBox></span>
            </div>
            <div class="control-inline">
                <as:ValidatorMessage runat="server" ID="MinimumVolValidatorMessage" ApplyFor="txtMinimumVol" />
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
                    <as:Literal ID="Literal2" runat="server" Text="Today's Sales Volume >=" meta:resourcekey="Literal2Resource1"></as:Literal>
                </label>
            </div>
            <div class="control-inline last">
                <span>
                    <as:RadNumericTextBox ID="txtContractualVol" runat="server" Width="70px" onkeypress="MerchantRange_OnKeyPress(event);"
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
           <br />
            <div class="control-inline" style="width: 250px;">               
            </div>
            <div class="control-inline last">                  
                <label class="control-label last">                   
                    <as:Literal ID="Literal4" runat="server" Text="Today's Sales Amount Ratio = Actual Daily Total Sales Amount / Contractual Daily Net Amount" 
                        meta:resourcekey="Literal4Resource1"></as:Literal>
                </label>
            </div>

            <div class="control-inline">
                <as:ValidatorMessage runat="server" ID="ContractualVolValidatorMessage" ApplyFor="txtContractualVol" />
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
        var Risk_Assignment_Filter_Transactional_cbMinimumVol = '<%= cbMinimumVol.ClientID %>';
        var Risk_Assignment_Filter_Transactional_txtMinimumVol = '<%= txtMinimumVol.ClientID %>';
        var Risk_Assignment_Filter_Transactional_cbContractualVol = '<%= cbContractualVol.ClientID %>';
        var Risk_Assignment_Filter_Transactional_txtContractualVol = '<%= txtContractualVol.ClientID %>';
        var Risk_Assignment_Filter_Transactional_SixtydayTransCntMsg = '<%= SixtydayTransCntMsg.ClientID %>';
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk/Risk_Assignment_Filter_Transactional.js"></script>

</as:ASRadCodeBlock>
