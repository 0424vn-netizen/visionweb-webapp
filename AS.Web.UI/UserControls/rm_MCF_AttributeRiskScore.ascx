<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_AttributeRiskScore.ascx.cs" Inherits="UserControls_rm_MCF_AttributeRiskScore" %>

<as:RadAjaxManagerProxy runat="server" ID="uxRadManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxAttributeNameList">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxRangeName" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxFrom" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxTo" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxOperand" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxMetric" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxScore" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxRiskScoreButtons" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxMetricListPanel" LoadingPanelID="uxloading" />

                <tek:AjaxUpdatedControl ControlID="uxValidatorLabeluxMetric" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxValidatorLabeluxMetricListText" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxMetricFrom" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxMetricTo" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxValidatorLabelMetricFrom" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxValidatorLabelMetricTo" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxValidatorLabeluxMetric" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="hddMinValueMetric" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="hddMaxValueMetric" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="hddMinValueFromTo" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="hddMaxValueFromTo" LoadingPanelID="uxloading" />
            </UpdatedControls>
        </tek:AjaxSetting>

        <tek:AjaxSetting AjaxControlID="uxOperand">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxMetric" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxMetricFrom" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxMetricTo" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxValidatorLabelMetricFrom" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxValidatorLabelMetricTo" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxValidatorLabeluxMetric" LoadingPanelID="uxloading" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxUpdate">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxAttributeRiskScoreMsg" LoadingPanelID="uxloading" />
            </UpdatedControls>
        </tek:AjaxSetting>
         <tek:AjaxSetting AjaxControlID="uxCancel">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxRangeName" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxFrom" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxTo" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxOperand" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxMetric" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxScore" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxRiskScoreButtons" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxMetricListPanel" LoadingPanelID="uxloading" />

                <tek:AjaxUpdatedControl ControlID="uxValidatorLabeluxMetric" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxValidatorLabeluxMetricListText" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxMetricFrom" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxMetricTo" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxValidatorLabelMetricFrom" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxValidatorLabelMetricTo" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="uxValidatorLabeluxMetric" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="hddMinValueMetric" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="hddMaxValueMetric" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="hddMinValueFromTo" LoadingPanelID="uxloading" />
                <tek:AjaxUpdatedControl ControlID="hddMaxValueFromTo" LoadingPanelID="uxloading" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>
<asp:Panel ID="uxloading" runat="server" />
<div class="row">
    <div class="col-md-12">
        <div class="risk-form-container">
            <div class="risk-form form-inline valign-middle">
                <table style="width: 852px" id="risk-form-table">
                    <colgroup>
                        <col style="width: 120px;" />
                        <col style="width: 80px;" />
                        <col />
                        <col style="width: 80px;" />
                        <col style="width: 50px;" />
                        <col />
                        <col style="width: 100px;" />
                        <col />
                        <col style="width: 152px;" />
                        <col />
                        <col style="width: 80px;" />
                    </colgroup>
                    <tr>
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="uxParamListLabel" runat="server" Text="Attribute Name" ApplyFor="uxAttributeNameList" CssClass="risk-scores-first" meta:resourcekey="LabelResourceAttributeName" />
                        </td>
                        <td class="risk-form-row" colspan="10">
                            <div class="pos-relative">
                                <as:RadComboBox ID="uxAttributeNameList" runat="server" Filter="Contains" MarkFirstMatch="true" meta:resourcekey="ComboboxResourceAttributeName" CssClass="normal-italic"
                                    Width="100%" EmptyMessage="Select Attribute Name" AutoPostBack="true" DataTextField="DataText" DataValueField="DataValue" OnSelectedIndexChanged="uxAttributeNameList_SelectedIndexChanged" />
                                <div class="label-error-left">
                                    <as:ValidatorMessage runat="server" ID="uxParamListMsg" ApplyFor="uxAttributeNameList" Message="" ShowOnLoad="False" />
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="uxRangeNameLabel" runat="server" Text="Range Name" ApplyFor="uxRangeName" CssClass="risk-scores-first" meta:resourcekey="LabelResourceRangeName" />
                        </td>
                        <td class="risk-form-row text-left" colspan="10">
                            <div class="pos-relative">
                                <asp:TextBox ID="uxRangeName" runat="server" Enabled="false" MaxLength="50" onkeypress="return DefaultEnterOnTextBox(event);" CssClass="form-control" Width="100%" meta:resourcekey="uxParameterIndicatorFromResource1" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="ValidatorLabel1" runat="server" Text="From" ApplyFor="uxFrom" CssClass="risk-scores-first" meta:resourcekey="LabelResourceFrom" />
                        </td>
                        <td class="risk-form-row text-left text-nowrap">
                            <as:RadTextBox ID="uxFrom" runat="server" Enabled="false" MaxLength="9" onkeypress="return InputOnTextBox(event);" ClientEvents-OnBlur="checkInputCharacter" CssClass="form-control" Width="100%" meta:resourcekey="uxParameterIndicatorFromResource1" />

                        </td>
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="ValidatorLabel2" runat="server" Text="To" ApplyFor="uxTo" CssClass="risk-scores-first ml-8" meta:resourcekey="LabelResourceTo" />
                        </td>
                        <td class="risk-form-row text-left text-nowrap">
                            <as:RadTextBox ID="uxTo" runat="server" Enabled="false" MaxLength="9" onkeypress="return InputOnTextBox(event);" ClientEvents-OnBlur="checkInputCharacter" CssClass="form-control" Width="100%" meta:resourcekey="uxParameterIndicatorFromResource1" />
                            <as:HiddenField ID="hddMinValueFromTo" runat="server" />
                            <as:HiddenField ID="hddMaxValueFromTo" runat="server" />

                        </td>
                        <td class="risk-form-row text-center">
                            <label class="mr-0 font-weight-bold">
                                <asp:Literal ID="Literal1" runat="server" Text="or" meta:resourcekey="LabelResourceOr" /></label>
                        </td>
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="ValidatorLabel3" runat="server" Text="Operand" ApplyFor="uxOperand" CssClass="risk-scores-first" meta:resourcekey="LabelResourceOperand" />
                        </td>

                        <td class="risk-form-row text-left text-nowrap">
                            <as:RadComboBox ID="uxOperand" runat="server" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="uxOperand_SelectedIndexChanged"
                                Width="100%" DataTextField="DataText" DataValueField="DataValue" />

                        </td>

                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="uxValidatorLabeluxMetric" runat="server" Text="Metric" ApplyFor="uxMetric" CssClass="risk-scores-first ml-8" meta:resourcekey="LabelResourceMetric" />
                            <as:ValidatorLabel ID="uxValidatorLabeluxMetricListText" Visible="false" runat="server" Text="Metric" ApplyFor="uxMetricListText" CssClass="risk-scores-first ml-8" meta:resourcekey="LabelResourceMetric" />
                        </td>
                        <td class="risk-form-row text-left">
                            <as:RadComboBox ID="uxMetric" runat="server" Filter="Contains" MarkFirstMatch="true" Enabled="false" CheckBoxes="false" DataTextField="DataText" DataValueField="DataValue" OnClientBlur="MetricOnBlur" OnClientItemChecked="MetricItemCheck"
                                Width="100%" />
                            <as:PlaceHolder ID="uxMetricListPanel" runat="server" Visible="false">
                                <as:TextBox runat="server" ID="uxMetricListText" Width="60%" onblur="checkInputCharacterMetric(this);" />
                                <asp:HiddenField runat="server" ID="uxMetricListTextHide" />
                                <a href="#" runat="server" id="btnlinkFindOwner" class="btn link-btn risk-scores-first btnlinkFindOwner" onclick="return ShowPopupModal('FindOwners.aspx', 'auto'); return false;">
                                    <asp:Literal ID="Literal2" runat="server" Text="Find" meta:resourcekey="LiteralResourceFind" /></a>

                            </as:PlaceHolder>
                            <div class="flex-box">
                                <as:ValidatorLabel ID="uxValidatorLabelMetricFrom" Visible="false" runat="server" Text="From" ApplyFor="uxMetricFrom" CssClass="risk-scores-first" meta:resourcekey="LabelResourceFrom" />
                                <as:RadTextBox ID="uxMetricFrom" Visible="false" runat="server" Width="30%" Enabled="true" MaxLength="9" onkeypress="return InputOnTextBox(event);" ClientEvents-OnBlur="checkInputCharacter" CssClass="form-control" meta:resourcekey="uxParameterIndicatorFromResource1" />
                                <as:ValidatorLabel ID="uxValidatorLabelMetricTo" Visible="false" runat="server" Text="To" ApplyFor="uxMetricTo" CssClass="risk-scores-first" meta:resourcekey="LabelResourceTo" />
                                <as:RadTextBox ID="uxMetricTo" Visible="false" runat="server" Width="30%" Enabled="true" MaxLength="9" onkeypress="return InputOnTextBox(event);" ClientEvents-OnBlur="checkInputCharacter" CssClass="form-control" meta:resourcekey="uxParameterIndicatorFromResource1" />
                                <as:HiddenField ID="hddMinValueMetric" runat="server" />
                                <as:HiddenField ID="hddMaxValueMetric" runat="server" />
                            </div>
                        </td>
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="ValidatorLabel5" runat="server" Text="Score" ApplyFor="uxScore" CssClass="risk-scores-first ml-8" meta:resourcekey="LabelResourceScore" />
                        </td>
                        <td class="risk-form-row text-left text-nowrap">
                            <as:RadTextBox ID="uxScore" runat="server" Enabled="false" MaxLength="9" onkeypress="return InputOnTextBox(event);" ClientEvents-OnBlur="checkInputCharacter" CssClass="form-control " Width="100%" meta:resourcekey="uxParameterIndicatorFromResource1" />

                        </td>
                    </tr>

                    <tr>
                        <td colspan="11" class="risk-form-action text-right">
                            <div id="uxRiskScoreButtons" runat="server">
                                <as:Button runat="server" ID="uxUpdate" Text="Submit" CssClass="btn btn-default"
                                    OnClick="uxUpdate_Click" OnClientClick="return doValidationAttributeCustom();" IsStandardButton="False" meta:resourcekey="uxUpdateResource" />
                                <as:Button runat="server" ID="uxCancel" CommandName="Cancel" Text="Cancel" CausesValidation="False" OnClick="uxCancel_Click"
                                    CssClass="btn btn-default" OnClientClick="clearError(); ClearFormValue(); " data-target=".create-attribute-rs-form" data-toggle="collapse" IsStandardButton="False" meta:resourcekey="uxCancelResource" />
                            </div>
                            <div class="bottom-error">
                                <asp:HiddenField ID="uxFakeControlValidate" runat="server" />

                            </div>
                        </td>
                    </tr>
                </table>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="ValidatorMessage1" ApplyFor="uxRangeName" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="ValidatorMessage2"  ApplyFor="uxFrom" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="ValidatorMessage3" ApplyFor="uxTo" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="ValidatorMessage4" ApplyFor="uxOperand" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="ValidatorMessage5" ApplyFor="uxMetric" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="ValidatorMessage7" ApplyFor="uxMetricListText" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="ValidatorMessage8" ApplyFor="uxMetricFrom" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="ValidatorMessage9" ApplyFor="uxMetricTo" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="ValidatorMessage6" ApplyFor="uxScore" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="uxAttributeRiskScoreMsg" ApplyFor="uxFakeControlValidate" Message="" ShowOnLoad="False" />
                </div>
            </div>
        </div>
    </div>
</div>
<as:Validator ID="ctrlValidatorAttribute" runat="server" ValidationFunction="doValidationAttribute" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <Items>
        <as:CustomValidationItem ControlToValidateID="uxAttributeNameList" ClientValidationFunction="ValidateAttributeName" OnValidateInput="AttributeName_ValidateInput" meta:resourcekey="RiskScore_ascx_AttributeRequired" />
    </Items>
</as:Validator>
<as:Validator ID="ctrlValidatorFromTo" runat="server" ValidationFunction="doValidationFromTo" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxRangeName" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_RangeNameRequired" />
        <as:CustomValidationItem ControlToValidateID="uxRangeName" ClientValidationFunction="checkRangeName" OnValidateInput="checkRangeName_ValidateInput" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_RangeNameInvalid" />
        <as:BasicValidationItem ControlToValidateID="uxFrom" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorFromRequired" />
        <as:BasicValidationItem ControlToValidateID="uxFrom" Rule="Number" MinLength="0" MaxLength="9" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorFromNumber" />
        <as:BasicValidationItem ControlToValidateID="uxFrom" Rule="GreaterOrEqualThan" MinValue="0" MaxValue="999999" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorFromNumberGreaterOrEqualThan" />
        <as:BasicValidationItem ControlToValidateID="uxFrom" Rule="Integer" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorFromNumberGreaterOrEqualThan" />
        <as:BasicValidationItem ControlToValidateID="uxTo" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorToRequired" />
        <as:BasicValidationItem ControlToValidateID="uxTo" Rule="Number" MinLength="0" MaxLength="9" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorToNumber" />
        <%--<as:CustomValidationItem ControlToValidateID="uxTo" ClientValidationFunction="ValidateGreaterOrEqualThan" OnValidateInput="uxToGreaterOrEqualThan_ValidateInput" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ToMustBeGreaterThanFrom" />--%>
        <as:BasicValidationItem ControlToValidateID="uxTo" Rule="GreaterOrEqualThan" MinValue="0" MaxValue="999999" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorToNumberGreaterOrEqualThan" />
        <as:BasicValidationItem ControlToValidateID="uxTo" Rule="Integer" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorToNumberGreaterOrEqualThan" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreRequired" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Number" MinLength="0" MaxLength="9" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumber" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="GreaterOrEqualThan" MinValue="0" MaxValue="999999" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Integer" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />


    </Items>
</as:Validator>
<as:Validator ID="ctrlValidatorOperand" runat="server" ValidationFunction="doValidationOperand" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxRangeName" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_RangeNameRequired" />
        <as:CustomValidationItem ControlToValidateID="uxRangeName" ClientValidationFunction="checkRangeName" OnValidateInput="checkRangeName_ValidateInput" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_RangeNameInvalid" />

        <as:BasicValidationItem ControlToValidateID="uxOperand" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_OperandRequired" />
        <%-- <as:CustomValidationItem ControlToValidateID="uxMetric" ClientValidationFunction="Metric_ValidateInput" OnValidateInput="uxMetric_ValidateInput" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_MetricRequired" />--%>
    </Items>
</as:Validator>
<as:Validator ID="ctrlValidatorMetricCombobox" runat="server" ValidationFunction="doValidatorMetricCombobox" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <Items>
        <as:CustomValidationItem ControlToValidateID="uxMetric" ClientValidationFunction="Metric_ValidateInput" OnValidateInput="uxMetric_ValidateInput" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_MetricRequired" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreRequired" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Number" MinLength="0" MaxLength="9" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumber" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="GreaterOrEqualThan" MinValue="0" MaxValue="999999" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Integer" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
    </Items>
</as:Validator>
<as:Validator ID="ctrlValidatorMetricModal" runat="server" ValidationFunction="doValidatorMetricModal" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <Items>
        <as:CustomValidationItem ControlToValidateID="uxMetricListText" ClientValidationFunction="uxMetricListText_ValidateInput" IsInAjaxPanel="true" OnValidateInput="uxMetricListText_ValidateInput" meta:resourcekey="RiskScore_ascx_MetricRequired" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreRequired" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Number" MinLength="0" MaxLength="9" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumber" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="GreaterOrEqualThan" MinValue="0" MaxValue="999999" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
        <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Integer" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
    </Items>
</as:Validator>

<as:Validator ID="ctrlValidatorRangeName" runat="server" ValidationFunction="doValidationRangeName" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <Items>
        <as:CustomValidationItem ControlToValidateID="uxRangeName" ClientValidationFunction="checkRangeName" OnValidateInput="checkRangeName_ValidateInput" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_RangeNameInvalid" />
    </Items>
</as:Validator>

<as:ASRadCodeBlock runat="server" ID="RadCodeBlock1">
    <script type="text/javascript">
        var uxAttributeNameList_ClientID = "<%=uxAttributeNameList.ClientID%>";
        var uxRangeName_ClientID = "<%=uxRangeName.ClientID %>";
        var uxFrom_ClientID = "<%=uxFrom.ClientID%>";
        var uxTo_ClientID = "<%=uxTo.ClientID%>";
        var attr_RiskScore_uxUpdate_ClientID = "<%=uxUpdate.UniqueID%>";
        var uxMetric_ClientID = "<%=uxMetric.ClientID%>";
        var uxOperand_ClientID = "<%= uxOperand.ClientID%>";
        var uxScore_ClientID = "<%= uxScore.ClientID%>";
        var uxMetricListText_ClientID = '<%= uxMetricListText.ClientID%>';
        var uxMetricListTextHide_ClientID = '<%= uxMetricListTextHide.ClientID%>';
        var msgitemsSelected = '<%= GetLocalResourceObject("LiteralResourceFindMoreitem").ToString() %>';
        var uxMetricFrom_ClientID = "<%=uxMetricFrom.ClientID%>";
        var uxMetricTo_ClientID = "<%=uxMetricTo.ClientID%>";
        var uxMetricFrom_Label_ClientID = "<%=uxValidatorLabelMetricFrom.ClientID%>";
        var uxMetricTo_Label_ClientID = "<%=uxValidatorLabelMetricTo.ClientID%>";

        var hhdMinValueMetric_ClientID = "<%=hddMinValueMetric.ClientID%>";
        var hhdMaxValueMetric_ClientID = "<%=hddMaxValueMetric.ClientID%>";
        var hhdMinValueFromTo_ClientID = "<%=hddMinValueFromTo.ClientID%>";
        var hhdMaxValueFromTo_ClientID = "<%=hddMaxValueFromTo.ClientID%>";
        var messageRequired = "<%= GetLocalResourceObject("RiskScore_ascx_IndicatorFromRequired.Message") %>";
        var messageMetric_From = "<%= GetLocalResourceObject("RiskScore_ascx_MetricFromBetween.Message") %>";
        var messageMetric_To = "<%= GetLocalResourceObject("RiskScore_ascx_MetricToBetween.Message") %>";
        var messageMetric_3 = "<%= GetLocalResourceObject("RiskScore_ascx_MetricRequired_Between.Message") %>"; 
        var messageScoreRequired = "<%= GetLocalResourceObject("RiskScore_ascx_ScoreRequired.Message") %>"; 
        var messageScoreGreaterOrEqual = "<%= GetLocalResourceObject("RiskScore_ascx_ScoreNumberGreaterOrEqualThan.Message") %>";
        var messageToMustGreaterThanorEqualFrom = "<%= GetLocalResourceObject("RiskScore_ascx_ToMustBeGreaterThanorEqualFrom.Message") %>";
        var messageToMustGreaterThanFrom = "<%= GetLocalResourceObject("RiskScore_ascx_ToMustBeGreaterThanFrom.Message") %>";
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/risk_MCF/rm_MCF_AttributeRiskScore.js"></script>
</as:ASRadCodeBlock>
