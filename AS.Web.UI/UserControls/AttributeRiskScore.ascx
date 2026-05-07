<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttributeRiskScore.ascx.cs" Inherits="UserControls_AttributeRiskScore" %>

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
            </UpdatedControls>
        </tek:AjaxSetting>

        <tek:AjaxSetting AjaxControlID="uxOperand">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxMetric" LoadingPanelID="uxloading" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxUpdate">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxAttributeRiskScoreMsg" LoadingPanelID="uxloading" />
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
                                <as:RadComboBox ID="uxAttributeNameList" runat="server" meta:resourcekey="ComboboxResourceAttributeName" CssClass="normal-italic"
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
                            <as:RadTextBox ID="uxFrom" runat="server" Enabled="false" onkeypress="return DefaultEnterOnTextBox(event);" CssClass="form-control" Width="100%" meta:resourcekey="uxParameterIndicatorFromResource1" />

                        </td>
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="ValidatorLabel2" runat="server" Text="To" ApplyFor="uxTo" CssClass="risk-scores-first ml-8" meta:resourcekey="LabelResourceTo" />
                        </td>
                        <td class="risk-form-row text-left text-nowrap">
                            <as:RadTextBox ID="uxTo" runat="server" Enabled="false" onkeypress="return DefaultEnterOnTextBox(event);" CssClass="form-control" Width="100%" meta:resourcekey="uxParameterIndicatorFromResource1" />

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
                            <as:RadComboBox ID="uxMetric" runat="server" Enabled="false" CheckBoxes="false" DataTextField="DataText" DataValueField="DataValue"
                                Width="100%" />
                            <as:PlaceHolder ID="uxMetricListPanel" runat="server" Visible="false"> 
                                <as:TextBox runat="server" ID="uxMetricListText" Width="60%" />
                                <asp:HiddenField runat="server" ID="uxMetricListTextHide" />
                                <a href="#" runat="server" id="btnlinkFindOwner" class="btn link-btn risk-scores-first btnlinkFindOwner" onclick="return ShowPopupModal('FindOwners.aspx', 'auto'); return false;">
                                    <asp:Literal ID="Literal2" runat="server" Text="Find" meta:resourcekey="LiteralResourceFind" /></a>

                            </as:PlaceHolder>
                        </td>
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="ValidatorLabel5" runat="server" Text="Score" ApplyFor="uxScore" CssClass="risk-scores-first ml-8" meta:resourcekey="LabelResourceScore" />
                        </td>
                        <td class="risk-form-row text-left text-nowrap">
                            <as:RadTextBox ID="uxScore" runat="server" Enabled="false" onkeypress="return DefaultEnterOnTextBox(event);" CssClass="form-control " Width="100%" meta:resourcekey="uxParameterIndicatorFromResource1" />

                        </td>
                    </tr>

                    <tr>
                        <td colspan="11" class="risk-form-action text-right">
                            <div id="uxRiskScoreButtons" runat="server">
                                <as:Button runat="server" ID="uxUpdate" Text="Submit" CssClass="btn btn-default"
                                    OnClick="uxUpdate_Click" OnClientClick="return doValidationAttribute();" IsStandardButton="False" meta:resourcekey="uxUpdateResource" />
                                <as:Button runat="server" ID="uxCancel" CommandName="Cancel" Text="Cancel" CausesValidation="False"
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
                    <as:ValidatorMessage runat="server" ID="ValidatorMessage2" ApplyFor="uxFrom" Message="" ShowOnLoad="False" />
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
        <as:CustomValidationItem ControlToValidateID="uxTo" ClientValidationFunction="ValidateGreaterOrEqualThan" OnValidateInput="uxToGreaterOrEqualThan_ValidateInput" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ToMustBeGreaterThanFrom" />
        <as:BasicValidationItem ControlToValidateID="uxTo" Rule="GreaterOrEqualThan" MinValue="1" MaxValue="999999" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorToNumberGreaterOrEqualThan" />
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
         
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/risk/AttributeRiskScore.js"></script>
</as:ASRadCodeBlock>
