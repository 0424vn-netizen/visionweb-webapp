<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditAttributeRiskScore.ascx.cs" Inherits="UserControls_EditAttributeRiskScore" %>
<as:RadAjaxManagerProxy runat="server" ID="uxRadManager">
    <AjaxSettings>

        <tek:AjaxSetting AjaxControlID="uxUpdate">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxAttributeRiskScoreMsg" LoadingPanelID="uxloading" />
            </UpdatedControls>
        </tek:AjaxSetting>

    </AjaxSettings>
</as:RadAjaxManagerProxy>


<asp:Panel ID="uxloading" runat="server" />
<asp:HiddenField ID="uxhdRedId" runat="server" Value='<%# Eval( "RecordID" ).ToString() %>' />
<asp:HiddenField ID="uxhdAttributeId" runat="server" Value='<%# Eval( "AttributeID" ).ToString() %>' />
<asp:HiddenField ID="uxhdRangeName" runat="server" Value='<%# Eval( "RangeName" ).ToString() %>' />
<asp:HiddenField ID="uxhdIsBinded" runat="server" />

<div class="text-center">
    <div class="risk-form text-right">
        <div class="inline-block">
            <div class="risk-form-row">
                <as:ValidatorLabel ID="uxRangeNameLabel" runat="server" Text="Range Name" ApplyFor="uxRangeName" CssClass="control-inline-label ml-0-i on-top" meta:resourcekey="LabelResourceRangeName" />
            </div>
            <div class="risk-form-row">
                <div id="Div1" runat="server" visible="false">
                    <as:ValidatorLabel ID="uxParamListLabel" runat="server" Text="Attribute Name" ApplyFor="uxAttributeNameList" CssClass="control-inline-label first" meta:resourcekey="LabelResourceAttributeName" />
                    <div class="control-inline ">
                        <as:RadComboBox ID="uxAttributeNameList" CssClass="uxAttributeNameList" Enabled="false" runat="server" meta:resourcekey="ComboboxResourceAttributeName"
                            Width="295px" EmptyMessage="Select Attribute Name" commandname="ChangeIndex" AutoPostBack="true" DataTextField="DataText" DataValueField="DataValue" OnSelectedIndexChanged="uxAttributeNameList_SelectedIndexChanged" />

                    </div>
                </div>
                <as:ValidatorLabel ID="ValidatorLabel1" runat="server" Text="From" ApplyFor="uxFrom" CssClass="control-inline-label ml-0-i" meta:resourcekey="LabelResourceFrom" />
            </div>
        </div>
        <div class="inline-block">
            <div class="risk-form-row">
                <div class="control-inline" style="width: 100%;">
                    <as:RadTextBox ID="uxRangeName" runat="server" Enabled="false" MaxLength="50" onkeypress="return DefaultEnterOnTextBox(event);" CssClass="uxRangeName form-control" Width="100%" meta:resourcekey="uxParameterIndicatorFromResource1" />

                </div>
            </div>
            <div class="risk-form-row">
                <div class="control-inline text-left">
                    <as:RadTextBox ID="uxFrom" runat="server" Enabled="false" onkeypress="return DefaultEnterOnTextBox(event);" CssClass="form-control" Width="80px" meta:resourcekey="uxParameterIndicatorFromResource1" />

                </div>
                <as:ValidatorLabel ID="ValidatorLabel2" runat="server" Text="To" ApplyFor="uxTo" CssClass="control-inline-label" meta:resourcekey="LabelResourceTo" />
                <div class="control-inline">
                    <as:RadTextBox ID="uxTo" runat="server" Enabled="false" onkeypress="return DefaultEnterOnTextBox(event);" CssClass="form-control" Width="80px" meta:resourcekey="uxParameterIndicatorFromResource1" />

                </div>
                <label class="font-weight-bold control-inline-label">
                    <asp:Literal ID="Literal1" runat="server" Text="or" meta:resourcekey="LabelResourceOr" />
                </label>
                <as:ValidatorLabel ID="ValidatorLabel3" runat="server" Text="Operand" ApplyFor="uxOperand" CssClass="control-inline-label" meta:resourcekey="LabelResourceOperand" />
                <div class="control-inline">
                    <as:RadComboBox ID="uxOperand" runat="server" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="uxOperand_SelectedIndexChanged"
                        Width="100px" DataTextField="DataText" DataValueField="DataValue" />
                </div>
                <as:ValidatorLabel ID="uxValidatorLabeluxMetric" runat="server" Text="Metric" ApplyFor="uxMetric" CssClass="control-inline-label" meta:resourcekey="LabelResourceMetric" />
                <as:ValidatorLabel ID="uxValidatorLabeluxMetricListText" runat="server" Text="Metric" ApplyFor="uxMetricListText" CssClass="control-inline-label" meta:resourcekey="LabelResourceMetric" />
                <div class="control-inline">
                    <as:RadComboBox ID="uxMetric" runat="server" Enabled="false" CheckBoxes="false" DataTextField="DataText" DataValueField="DataValue"
                        Width="152px" />
                    <as:PlaceHolder ID="uxMetricListPanel" runat="server" Visible="false">
                        <as:TextBox runat="server" ID="uxMetricListText" Width="60%" />
                        <asp:HiddenField runat="server" ID="uxMetricListTextHide" />
                        <a href="#" runat="server" id="btnlinkFindOwner" class="btn-link" onclick="return ShowPopupModal('FindOwners.aspx', 'auto'); return false;">
                            <asp:Literal ID="Literal2" runat="server" Text="Find" meta:resourcekey="LiteralResourceFind" /></a>
                    </as:PlaceHolder>
                </div>
                <as:ValidatorLabel ID="ValidatorLabel5" runat="server" Text="Score" ApplyFor="uxScore" CssClass="control-inline-label" meta:resourcekey="LabelResourceScore" />
                <div class="control-inline">
                    <as:RadTextBox ID="uxScore" runat="server" Enabled="false" onkeypress="return DefaultEnterOnTextBox(event);" CssClass="form-control" Width="80px" meta:resourcekey="uxParameterIndicatorFromResource1" />
                </div>
            </div>
        </div>
        <div class="risk-form-action text-right">
            <div id="uxRiskScoreButtons" runat="server">
                <as:Button runat="server" ID="uxUpdate" Text="Submit" OnClick="uxUpdate_Click" CssClass="btn btn-default" OnClientClick="return doValidationAttributeEdit();" meta:resourcekey="uxUpdateResource" />
                <as:Button runat="server" ID="uxCancel" CommandName="Cancel" Text="Cancel" CausesValidation="False"
                    CssClass="btn btn-default" data-toggle="collapse" IsStandardButton="False" meta:resourcekey="uxCancelResource" />
            </div>
        </div>


        <div class="text-center">
            <div class="bottom-error">
                <as:ValidatorMessage runat="server" ID="uxParamListMsg" ApplyFor="uxAttributeNameList" Message="" ShowOnLoad="False" />
            </div>
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
                <asp:HiddenField ID="uxFakeControlValidate" runat="server" />
                <as:ValidatorMessage runat="server" ID="uxAttributeRiskScoreMsg" ApplyFor="uxFakeControlValidate" Message="" />
            </div>
        </div>
    </div>
</div>



<tek:RadScriptBlock runat="server" ID="RadCodeBlock1">
    <as:Validator ID="ctrlValidatorAttribute" runat="server" ValidationFunction="doValidationAttributeEdit" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
        <Items>
            <as:CustomValidationItem ControlToValidateID="uxAttributeNameList" ClientValidationFunction="ValidateAttributeNameEdit" OnValidateInput="AttributeName_ValidateInput" meta:resourcekey="RiskScore_ascx_AttributeRequired" />
        </Items>
    </as:Validator>

    <as:Validator ID="ctrlValidatorFromTo" runat="server" ValidationFunction="doValidationFromToEdit" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
        <Items>
            <as:BasicValidationItem ControlToValidateID="uxRangeName" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_RangeNameRequired" />
            <as:CustomValidationItem ControlToValidateID="uxRangeName" ClientValidationFunction="checkRangeNameEdit" OnValidateInput="checkRangeName_ValidateInput" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_RangeNameInvalid" />
            <as:BasicValidationItem ControlToValidateID="uxFrom" Rule="GreaterOrEqualThan" MinValue="0" MaxValue="999999" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorFromNumberGreaterOrEqualThan" />
            <as:BasicValidationItem ControlToValidateID="uxFrom" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorFromRequired" />
            <as:BasicValidationItem ControlToValidateID="uxFrom" Rule="Number" MinLength="0" MaxLength="9" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorFromNumber" />
            <as:BasicValidationItem ControlToValidateID="uxFrom" Rule="Integer" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorFromNumberGreaterOrEqualThan" />
            <as:BasicValidationItem ControlToValidateID="uxTo" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorToRequired" />
            <as:BasicValidationItem ControlToValidateID="uxTo" Rule="Number" MinLength="0" MaxLength="9" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorToNumber" />
            <as:CustomValidationItem ControlToValidateID="uxTo" ClientValidationFunction="ValidateGreaterOrEqualThanEdit" OnValidateInput="uxToGreaterOrEqualThan_ValidateInput" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ToMustBeGreaterThanFrom" />
            <as:BasicValidationItem ControlToValidateID="uxTo" Rule="GreaterOrEqualThan" MinValue="0" MaxValue="999999" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorToNumberGreaterOrEqualThan" />
            <as:BasicValidationItem ControlToValidateID="uxTo" Rule="Integer" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_IndicatorToNumberGreaterOrEqualThan" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreRequired" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Number" MinLength="0" MaxLength="9" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumber" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="GreaterOrEqualThan" MinValue="0" MaxValue="999999" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Integer" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />

        </Items>
    </as:Validator>

    <as:Validator ID="ctrlValidatorOperand" runat="server" ValidationFunction="doValidationOperandEdit" ClientIDMode="Static" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
        <Items>
            <as:BasicValidationItem ControlToValidateID="uxRangeName" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_RangeNameRequired" />
            <as:CustomValidationItem ControlToValidateID="uxRangeName" ClientValidationFunction="checkRangeNameEdit" OnValidateInput="checkRangeName_ValidateInput" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_RangeNameInvalid" />

        </Items>
    </as:Validator>

    <as:Validator ID="ctrlValidatorMetricCombobox" runat="server" ValidationFunction="doValidatorMetricComboboxEdit" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
        <Items>
            <as:CustomValidationItem ControlToValidateID="uxMetric" ClientValidationFunction="Metric_ValidateInputEdit" OnValidateInput="uxMetric_ValidateInput" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_MetricRequired" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreRequired" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Number" MinLength="0" MaxLength="9" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumber" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="GreaterOrEqualThan" MinValue="0" MaxValue="999999" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Integer" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
        </Items>
    </as:Validator>
    <as:Validator ID="ctrlValidatorMetricModal" runat="server" ValidationFunction="doValidatorMetricModalEdit" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
        <Items>
            <as:BasicValidationItem ControlToValidateID="uxMetricListText" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_MetricRequired" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Required" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreRequired" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Number" MinLength="0" MaxLength="9" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumber" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="GreaterOrEqualThan" MinValue="0" MaxValue="999999" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
            <as:BasicValidationItem ControlToValidateID="uxScore" Rule="Integer" IsInAjaxPanel="true" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
        </Items>
    </as:Validator>



    <script type="text/javascript">
        var uxAttributeNameListEdit_ClientID = "<%=uxAttributeNameList.ClientID%>";
        var uxRangeNameEdit_ClientID = "<%=uxRangeName.ClientID%>";
        var uxFromEdit_ClientID = "<%=uxFrom.ClientID%>";
        var uxToEdit_ClientID = "<%=uxTo.ClientID%>";
        var attrEdit_RiskScore_uxUpdate_ClientID = "<%=uxUpdate.UniqueID%>";
        var uxMetricEdit_ClientID = "<%=uxMetric.ClientID%>";
        var uxMetricListTextEdit_ClientID = '<%= uxMetricListText.ClientID%>'; 
        var uxMetricListTextEditHide_ClientID = '<%= uxMetricListTextHide.ClientID%>';
        var msgitemsSelected = '<%= GetLocalResourceObject("LiteralResourceFindMoreitem").ToString() %>';
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/") %>res/js/risk/EditAttributeRiskScore.js"></script>

</tek:RadScriptBlock>





