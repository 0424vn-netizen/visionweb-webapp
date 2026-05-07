<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_Filter_ECommerce.ascx.cs"
    Inherits="UserControls_rm_MCF_Assignment_Filter_ECommerce" %>

<asp:Panel runat="server" ID="uxECommerce">
    <tr class="AltRow">
        <td class="heading">
            <as:ValidatorLabel ID="uxValidatorLableECommerce" runat="server" CssClass="control-label" Text="% E-Commerce:" ApplyFor="uxECommerceFrom" meta:resourcekey="uxValidatorLableECommerceResource1"></as:ValidatorLabel>
        </td>
        <as:ASRadCodeBlock ID="ASRadCodeBlock5" runat="server">
            <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
                <div class="control-inline">
                    <as:CheckBox ID="uxchkEcommerce" runat="server" tracking-required="true" tracking-key="Ecommerce" tracking-type="checkbox" onclick="Assignment_Filter_ECommerce.chkECommerce_Changed();" />
                </div>
                <div class="control-inline">
                    <as:RadComboBox ID="uxCboECommerceType" runat="server" tracking-key="ECommerceType" tracking-type="comboboxIndex"
                        Width="148px" OnClientSelectedIndexChanged="Assignment_Filter_ECommerce.uxCboECommerceType_OnClientSelectedIndexChanged"
                        MaxHeight="300" meta:resourcekey="uxCbxECommerceTypeResource1">
                    </as:RadComboBox>
                </div>
                <div class="control-inline last" style="vertical-align:middle">
                    <div class="flex-box" >
                        <div id="idfromEcommerce">
                            <as:RadNumericTextBox ID="uxECommerceFrom" runat="server" Width="70px" MinValue="0" tracking-key="ECommerceBetweenFrom"
                                MaxLength="3" CssClass="form-control" LabelCssClass="" LabelWidth="64px">
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
                        <div id="idAndEcommerce">
                            <label>
                                <as:Literal ID="uxECommerceltAnd" runat="server" Text="And" meta:resourcekey="ltAndResource1"></as:Literal>
                            </label>
                        </div>
                        <div id="idToEcommerce" class="ml-5">
                            <as:RadNumericTextBox ID="uxECommerceTo" runat="server" Width="70px" MinValue="0" tracking-key="ECommerceBetweenTo"
                                MaxLength="3" CssClass="form-control" LabelCssClass="" LabelWidth="64px">
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
                </div>
                <div class="control-inline">
                    <as:ValidatorMessage runat="server" ID="UxMsgECommerce" ApplyFor="uxECommerceFrom" />
                </div>
            </td>
        </as:ASRadCodeBlock>
    </tr>
</asp:Panel>
<asp:Panel runat="server" ID="uxKeyed">
    <tr class="Row">
        <td class="heading">
            <as:ValidatorLabel ID="uxValidatorLableKeyed" runat="server" CssClass="control-label" Text="% Keyed:" ApplyFor="uxKeyedFrom" meta:resourcekey="uxValidatorLableKeyedResource1"></as:ValidatorLabel>
        </td>
        <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
            <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
                <div class="control-inline">
                    <as:CheckBox ID="uxchkKeyed" runat="server" tracking-required="true" tracking-key="Keyed" tracking-type="checkbox" onclick="Assignment_Filter_ECommerce.chkKeyed_Changed();" />
                </div>
                <div class="control-inline">
                    <as:RadComboBox ID="uxCboKeyedType" runat="server"  tracking-key="KeyedType" tracking-type="comboboxIndex"
                        Width="148px" OnClientSelectedIndexChanged="Assignment_Filter_ECommerce.uxCboKeyedType_OnClientSelectedIndexChanged"
                        MaxHeight="300" meta:resourcekey="uxCbxKeyedTypeResource1">
                    </as:RadComboBox>
                </div>
                <div class="control-inline last" style="vertical-align:middle">
                    <div class="flex-box">
                        <div id="idfromKeyed">
                            <as:RadNumericTextBox ID="uxKeyedFrom" runat="server" Width="70px" MinValue="0" tracking-key="KeyedBetweenFrom"
                                MaxLength="3" CssClass="form-control" LabelCssClass="" LabelWidth="64px">
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
                        <div id="idAndKeyed">
                            <label>
                                <as:Literal ID="uxKeyedltAnd" runat="server" Text="And" meta:resourcekey="ltAndResource1"></as:Literal>
                            </label>
                        </div>
                        <div id="idToKeyed" class="ml-5">
                            <as:RadNumericTextBox ID="uxKeyedTo" runat="server" Width="70px" MinValue="0" tracking-key="KeyedBetweenTo"
                                MaxLength="3" CssClass="form-control" LabelCssClass="" LabelWidth="64px">
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
                </div>
                <div class="control-inline">
                    <as:ValidatorMessage runat="server" ID="UxMsgKeyed" ApplyFor="uxKeyedFrom" />
                </div>
            </td>
        </as:ASRadCodeBlock>
    </tr>
</asp:Panel>
<asp:Panel runat="server" ID="uxSwiped">
    <tr class="AltRow">
        <td class="heading">
            <as:ValidatorLabel ID="uxValidatorLableSwiped" runat="server" CssClass="control-label" Text="% Swiped:" ApplyFor="uxSwipedFrom" meta:resourcekey="uxValidatorLableSwipedResource1"></as:ValidatorLabel>
        </td>
        <as:ASRadCodeBlock ID="ASRadCodeBlock2" runat="server">
            <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
                <div class="control-inline">
                    <as:CheckBox ID="uxchkSwiped" runat="server" tracking-required="true" tracking-key="Swiped" tracking-type="checkbox" onclick="Assignment_Filter_ECommerce.chkSwiped_Changed();" />
                </div>
                <div class="control-inline">
                    <as:RadComboBox ID="uxCboSwipedType" runat="server" tracking-key="SwipedType" tracking-type="comboboxIndex"
                        Width="148px" OnClientSelectedIndexChanged="Assignment_Filter_ECommerce.uxCboSwipedType_OnClientSelectedIndexChanged"
                        MaxHeight="300" meta:resourcekey="uxCbxSwipedTypeResource1">
                    </as:RadComboBox>
                </div>
                <div class="control-inline last" style="vertical-align:middle">
                    <div class="flex-box">
                        <div id="idfromSwiped">
                            <as:RadNumericTextBox ID="uxSwipedFrom" runat="server" Width="70px" MinValue="0" tracking-key="SwipedBetweenFrom"
                                MaxLength="3" CssClass="form-control" LabelCssClass="" LabelWidth="64px">
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
                        <div id="idAndSwiped">
                            <label>
                                <as:Literal ID="uxSwipedltAnd" runat="server" Text="And" meta:resourcekey="ltAndResource1"></as:Literal>
                            </label>
                        </div>
                        <div id="idToSwiped" class="ml-5">
                            <as:RadNumericTextBox ID="uxSwipedTo" runat="server" Width="70px" MinValue="0" tracking-key="SwipedBetweenTo"
                                MaxLength="3" CssClass="form-control" LabelCssClass="" LabelWidth="64px">
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
                </div>
                <div class="control-inline">
                    <as:ValidatorMessage runat="server" ID="UxMsgSwiped" ApplyFor="uxSwipedFrom" />
                </div>
            </td>
        </as:ASRadCodeBlock>
    </tr>
</asp:Panel>
<as:ASRadCodeBlock ID="ASRadCodeBlock3" runat="server">
    <script type="text/javascript">

        var Risk_Assignment_Filter_ECommerce_And = '<%= uxECommerceltAnd.ClientID %>';
        var Risk_Assignment_Filter_ECommerce_uxFrom = '<%= uxECommerceFrom.ClientID %>';
        var Risk_Assignment_Filter_ECommerce_uxTo = '<%= uxECommerceTo.ClientID %>';
        var Risk_Assignment_Filter_ECommerce_uxchk = '<%= uxchkEcommerce.ClientID %>';
        var Risk_Assignment_Filter_ECommerce_uxcbo = '<%= uxCboECommerceType.ClientID %>';

        var Risk_Assignment_Filter_Keyed_And = '<%= uxKeyedltAnd.ClientID %>';
        var Risk_Assignment_Filter_Keyed_uxFrom = '<%= uxKeyedFrom.ClientID %>';
        var Risk_Assignment_Filter_Keyed_uxTo = '<%= uxKeyedTo.ClientID %>';
        var Risk_Assignment_Filter_Keyed_uxchk = '<%= uxchkKeyed.ClientID %>';
        var Risk_Assignment_Filter_Keyed_uxcbo = '<%= uxCboKeyedType.ClientID %>';

        var Risk_Assignment_Filter_Swiped_And = '<%= uxSwipedltAnd.ClientID %>';
        var Risk_Assignment_Filter_Swiped_uxFrom = '<%= uxSwipedFrom.ClientID %>';
        var Risk_Assignment_Filter_Swiped_uxTo = '<%= uxSwipedTo.ClientID %>';
        var Risk_Assignment_Filter_Swiped_uxchk = '<%= uxchkSwiped.ClientID %>';
        var Risk_Assignment_Filter_Swiped_uxcbo = '<%= uxCboSwipedType.ClientID %>';
        var Risk_Assignment_Filter_ECommerce_Required = '<%=GetGlobalResourceObject("ValMsg","Required")%>';
        var Risk_Assignment_Filter_ECommerce_To_Greater = '<%=GetGlobalResourceObject("ValMsg","To_Greater")%>';
        var Risk_Assignment_Filter_ECommerce ='<%=GetLocalResourceObject("Risk_Msg_ECommerce_Between")%>'
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_Filter_ECommerce.js"></script>

</as:ASRadCodeBlock>
