<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Score.ascx.cs" Inherits="UserControls_rm_MCF_Score" %>


<div class="row">
    <div class="col-md-12">
        <div class="risk-form-container">
            <div class="risk-form form-inline valign-middle">
                <table style="width: 680px;" id="risk-form-table">
                    <colgroup>
                        <col style="width: 80px" />
                        <col />
                        <col />
                        <col />
                        <col />
                        <col />
                        <col />
                        <col style="width: 100px" />
                    </colgroup>
                    <tr>
                        <td class="risk-form-row">
                            <as:ValidatorLabel ID="uxParamListLabel" runat="server" Text="Parameter:" ApplyFor="uxParamList" CssClass="risk-scores-first" meta:resourcekey="uxParamListLabelResource1" />
                        </td>
                        <td colspan="7" class="risk-form-row">
                            <div class="pos-relative">
                                <as:RadComboBox ID="uxParamList" runat="server" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Select Attribute Name" meta:resourcekey="ComboboxResourceParameter"
                                    Width="100%" Height="300px" OnClientSelectedIndexChanged="uxParamList_OnClientSelectedIndexChanged"
                                    OnItemDataBound="uxParamList_ItemDataBound" />
                                <div class="label-error-left">
                                    <as:ValidatorMessage runat="server" ID="uxParamListMsg" ApplyFor="uxParamList" Message="" ShowOnLoad="False" />
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr id="RiskScoreContent">
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="uxParameterIndicatorFromLabel" runat="server" Text="From:" ApplyFor="uxParameterIndicatorFrom" CssClass="risk-scores-first" meta:resourcekey="uxParameterIndicatorFromLabelResource1" />
                        </td>
                        <td class="risk-form-row text-left text-nowrap">
                            <span id="uxOpenBracketFrom" class="display-none">
                                <as:ValidatorLabel runat="server" Text="( " ApplyFor="uxParameterIndicatorFrom" CssClass="red risk-scores-first" meta:resourcekey="uxParameterIndicatorFromLabelResource2" />
                            </span>
                            <asp:TextBox ID="uxParameterIndicatorFrom" runat="server" MaxLength="14" onkeypress="" onblur="" CssClass="form-control" Width="100px" meta:resourcekey="uxParameterIndicatorFromResource1" />
                            <span id="uxCloseBracketFrom" class="display-none">
                                <as:ValidatorLabel runat="server" Text=")" ApplyFor="uxParameterIndicatorFrom" CssClass="red risk-scores-first" meta:resourcekey="uxParameterIndicatorFromLabelResource3" />
                            </span>
                        </td>
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="uxParameterIndicatorToLabel" runat="server" Text="To:" ApplyFor="uxParameterIndicatorTo" CssClass="control-inline-label" meta:resourcekey="uxParameterIndicatorToLabelResource1" />
                        </td>
                        <td class="risk-form-row text-left text-nowrap">
                            <span id="uxOpenBracketTo" class="display-none">
                                <as:ValidatorLabel runat="server" Text="(" ApplyFor="uxParameterIndicatorTo" CssClass="red risk-scores-first" meta:resourcekey="uxParameterIndicatorToLabelResource2" />
                            </span>
                            <as:TextBox ID="uxParameterIndicatorTo" runat="server" MaxLength="14" onkeypress="" onblur="" CssClass="form-control" Width="100px" HintCss="hint" meta:resourcekey="uxParameterIndicatorToResource1" />
                            <span id="uxCloseBracketTo" class="display-none">
                                <as:ValidatorLabel runat="server" Text=")" ApplyFor="uxParameterIndicatorTo" CssClass="red risk-scores-first" meta:resourcekey="uxParameterIndicatorToLabelResource3" />
                            </span>
                        </td>
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="uxParameterThresholdLabel" runat="server" Text="Threshold:" ApplyFor="uxParameterThreshold" CssClass="control-inline-label" meta:resourcekey="uxParameterThresholdLabelResource1" />
                        </td>
                        <td class="risk-form-row text-left">
                            <div class="inline-block text-nowrap">
                                <label class="label-low" id="uxLow">
                                    <as:Literal ID="ltLow" runat="server" Text="Low $" meta:resourcekey="ltLowResource1"></as:Literal></label>
                                <span id="uxOpenBracketThreshold" class="display-none">
                                    <as:ValidatorLabel runat="server" Text="(" ApplyFor="uxParameterThreshold" CssClass="red risk-scores-first" meta:resourcekey="uxParameterThresholdLabelResource2" />
                                </span>
                                <as:TextBox ID="uxParameterThreshold" runat="server" MaxLength="9" Enabled="False" CssClass="form-control inline-block" onkeypress="return InputOnTextBox(event);" onblur="checkInputCharacterScore(this);" Width="100px" HintCss="hint" meta:resourcekey="uxParameterThresholdResource1" />
                                <span id="uxCloseBracketThreshold" class="display-none">
                                    <as:ValidatorLabel runat="server" Text=")" ApplyFor="uxParameterThreshold" CssClass="red risk-scores-first" meta:resourcekey="uxParameterThresholdLabelResource3" />
                                </span>
                                <span id="uxLabThresholdHigh">
                                    <label class="label-high">
                                        <as:Literal ID="Literal1" runat="server" Text="High $" meta:resourcekey="Literal1Resource1"></as:Literal></label>
                                    <span id="uxOpenBracketThresholdHigh" class="display-none">
                                        <as:ValidatorLabel runat="server" Text="(" ApplyFor="uxParameterThresholdHigh" CssClass="red risk-scores-first" meta:resourcekey="uxParameterThresholdLabelResource2" />
                                    </span>
                                    <as:TextBox ID="uxParameterThresholdHigh" runat="server" MaxLength="9" Enabled="False" CssClass="form-control inline-block" Width="100px" HintCss="hint" meta:resourcekey="uxParameterThresholdHighResource1" />
                                    <span id="uxCloseBracketThresholdHigh" class="display-none">
                                        <as:ValidatorLabel runat="server" Text=")" ApplyFor="uxParameterThresholdHigh" CssClass="red risk-scores-first" meta:resourcekey="uxParameterThresholdLabelResource3" />
                                    </span>
                                </span>
                            </div>
                        </td>
                        <td class="risk-form-row text-right">
                            <as:ValidatorLabel ID="uxParameterScoreLabel" runat="server" Text="Score:" ApplyFor="uxParameterScore" CssClass="control-inline-label" meta:resourcekey="uxParameterScoreLabelResource1" />
                        </td>
                        <td class="risk-form-row text-right text-nowrap">
                            <span id="uxOpenBracketScore" class="display-none">
                                <as:ValidatorLabel runat="server" Text="(" ApplyFor="uxParameterScore" CssClass="red risk-scores-first" meta:resourcekey="uxParameterThresholdLabelResource2" />
                            </span>
                            <as:TextBox ID="uxParameterScore" runat="server" MaxLength="9" onkeypress="return InputOnTextBox(event);" onblur="checkInputCharacterScore(this);" CssClass="form-control" Width="100px" HintCss="hint" meta:resourcekey="uxParameterScoreResource1" />
                            <span id="uxCloseBracketScore" class="display-none">
                                <as:ValidatorLabel runat="server" Text=")" ApplyFor="uxParameterScore" CssClass="red risk-scores-first" meta:resourcekey="uxParameterThresholdLabelResource3" />
                            </span>
                        </td>
                    </tr>
                    <tr id="RiskScoreButton">
                        <td></td>
                        <td colspan="5" class="text-left">
                            <div class="pt-9" id="uxDecimalNote">
                                <i class="text-muted">
                                    <as:Literal ID="Literal3" runat="server" meta:resourcekey="RiskScore_ascx_uxDecimalNote1"></as:Literal></i>
                                <br />
                                <i class="text-muted" id="uxDecimalNoteToDataType">
                                    <as:Literal ID="Literal2" runat="server" meta:resourcekey="RiskScore_ascx_uxDecimalNote2"></as:Literal></i>
                            </div>
                        </td>
                        <td colspan="2" class="risk-form-action text-right">
                            <as:Button runat="server" ID="uxUpdate" Text="Submit" CommandName="Update" CssClass="btn btn-default"
                                OnClick="uxSubmit_Click" OnClientClick="return doValidation();" IsStandardButton="False" meta:resourcekey="uxUpdateResource1" /><!--
                            --><as:Button runat="server" ID="uxCancel" CommandName="Cancel" Text="Cancel" CausesValidation="False"
                                CssClass="btn btn-default" OnClientClick="clearText(); return false;" data-target=".create-rs-form" data-toggle="collapse" IsStandardButton="False" meta:resourcekey="uxCancelResource1" />
                        </td>
                    </tr>
                </table>
                <div class="bottom-error">
                    <label id="uxParameterIndicatorFromPrecisionMsg" class="error"></label>
                    <as:ValidatorMessage runat="server" ID="uxParameterIndicatorFromMsg" ApplyFor="uxParameterIndicatorFrom" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <label id="uxParameterIndicatorToPrecisionMsg" class="error"></label>
                    <as:ValidatorMessage runat="server" ID="uxParameterIndicatorToMsg" ApplyFor="uxParameterIndicatorTo" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="uxParameterThresholdMsg" ApplyFor="uxParameterThreshold" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="uxParameterThresholdHighMsg" ApplyFor="uxParameterThresholdHigh" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="uxParameterScoreMsg" ApplyFor="uxParameterScore" Message="" ShowOnLoad="False" />
                </div>
            </div>
        </div>
    </div>
</div>

<as:Validator ID="ctrlValidator" runat="server" ValidationFunction="doValidation" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorResource1">
    <Items>
        <as:CustomValidationItem ControlToValidateID="uxParamList" ClientValidationFunction="CheckComboBox" ResMessage="Resources.ValMsg.Required" />
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorFrom" ClientValidationFunction="paramIndicatorFromRequiredForCreate" Message=" "/>
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorFrom" ClientValidationFunction="CheckIndicatorFromGreaterZero" Message=" " />
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorFrom" ClientValidationFunction="paramIndicatorFromValidForCreate" meta:resourcekey="RiskScore_ascx_IndicatorFromNumberGreaterOrEqualThan" />
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorFrom" ClientValidationFunction="CheckValidFromPrecisionForCreate" Message=" " />

        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorTo" ClientValidationFunction="paramIndicatorToRequiredForCreate" Message=" " />
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorTo" ClientValidationFunction="CheckIndicatorToGreaterZero" Message=" " />
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorTo" ClientValidationFunction="paramIndicatorToValidForCreate" meta:resourcekey="RiskScore_ascx_IndicatorToNumberGreaterOrEqualThan" />
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorTo" ClientValidationFunction="CheckValidToPrecisionForCreate" Message=" " />


        <%--[42397] - Fixbug 37367--%>
        <%--<as:BasicValidationItem ControlToValidateID="uxParameterIndicatorFrom" Rule="Integer" meta:resourcekey="RiskScore_ascx_IndicatorFromNumberGreaterOrEqualThan" />
         <as:BasicValidationItem ControlToValidateID="uxParameterIndicatorTo" Rule="Integer" meta:resourcekey="RiskScore_ascx_IndicatorToNumberGreaterOrEqualThan" />--%>

        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorTo" ClientValidationFunction="compareFromToForCreate" meta:resourcekey="RiskScore_ascx_ToMustBeGreaterThanFrom" />
        <as:CustomValidationItem ControlToValidateID="uxParameterThreshold" ClientValidationFunction="paramThresholdValidForCreate" meta:resourcekey="RiskScore_ascx_ThresholdNumber" />


        <as:CustomValidationItem ControlToValidateID="uxParameterThreshold" ClientValidationFunction="paramThresholdLowRequiredForCreate" meta:resourcekey="RiskScore_ascx_ThresholdLowRequired" />
        <as:CustomValidationItem ControlToValidateID="uxParameterThresholdHigh" ClientValidationFunction="paramThresholdHighRequiredForCreate" Meta:resourcekey="RiskScore_ascx_ThresholdHighRequired" />
        <as:CustomValidationItem ControlToValidateID="uxParameterThresholdHigh" ClientValidationFunction="paramThresholdHighValidForCreate" meta:resourcekey="RiskScore_ascx_ThresholdHighNumber" />

        <as:CustomValidationItem ControlToValidateID="uxParameterThreshold" ClientValidationFunction="paramThresholdLowGreaterThanZero_ForCreate" meta:resourcekey="RiskScore_ascx_LowGreaterThanZero" />

        <as:CustomValidationItem ControlToValidateID="uxParameterThreshold" ClientValidationFunction="paramThresholdHigh_gt_Low_ForCreate" meta:resourcekey="RiskScore_ascx_HighMustBeGreaterThanLow" />


        <as:CustomValidationItem ControlToValidateID="uxParameterScore" ClientValidationFunction="paramIndicatorScoreRequiredForCreate" meta:resourcekey="RiskScore_ascx_ScoreRequired" />
        <as:CustomValidationItem ControlToValidateID="uxParameterScore" ClientValidationFunction="ValidateIntergerForCreate" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
        <as:CustomValidationItem ControlToValidateID="uxParameterScore" ClientValidationFunction="paramIndicatorScoreValidForCreate" meta:resourcekey="RiskScore_ascx_ScoreNumber" />
        <as:CustomValidationItem ControlToValidateID="uxParameterScore" ClientValidationFunction="ValidateExistingForCreate" ResMessage="Resources.ValMsg.RiskScore_Overlap" />
    </Items>
</as:Validator>


<as:HiddenField runat="server" ID="uxHasHighThreadhold" />

<as:ASRadCodeBlock runat="server" ID="RadCodeBlock1">
    <script type="text/javascript">
        var riskScore_uxParamList = "<%=uxParamList.ClientID%>";
        var riskScore_uxParameterIndicatorFrom = "<%=uxParameterIndicatorFrom.ClientID%>";
        var riskScore_uxParameterIndicatorTo = "<%=uxParameterIndicatorTo.ClientID%>";
        var riskScore_uxParameterThreshold = "<%=uxParameterThreshold.ClientID%>";
        var riskScore_uxParameterThresholdHigh = "<%=uxParameterThresholdHigh.ClientID%>";
        var riskScore_uxHasHighThreadhold = "<%=uxHasHighThreadhold.ClientID%>";


        var riskScore_uxParameterScore = "<%=uxParameterScore.ClientID%>";
        var riskScore_uxUpdate = "<%=uxUpdate.ClientID%>";

        var RiskScore_js_na = '<%= GetLocalResourceObject("RiskScore_js_na").ToString() %>';
        var riskScore_ascx_uxDecimalNoteToDataTypeMsg = '<%= GetLocalResourceObject("RiskScore_ascx_uxDecimalNoteToDataType.Text").ToString() %>';
        var RiskScore_uxParameterIndicatorFromPrecisionMsg = '<%= GetLocalResourceObject("RiskScore_ascx_IndicatorFromPrecision.Message").ToString() %>';
        var RiskScore_uxParameterIndicatorToPrecisionMsg = '<%= GetLocalResourceObject("RiskScore_ascx_IndicatorToPrecision.Message").ToString() %>';
        var riskScore_ascx_IndicatorFromNumberGreaterZeroPoint1Msg = '<%= GetLocalResourceObject("RiskScore_ascx_IndicatorFromNumberGreaterZeroPoint1.Message").ToString() %>';
        var riskScore_ascx_IndicatorFromNumberGreaterMinByPrecisionMsg = '<%= GetLocalResourceObject("RiskScore_ascx_IndicatorFromNumberGreaterMinByPrecision.Message").ToString() %>';
        var riskScore_ascx_IndicatorToNumberGreaterZeroPoint1Msg = '<%= GetLocalResourceObject("RiskScore_ascx_IndicatorToNumberGreaterZeroPoint1.Message").ToString() %>';
        var riskScore_ascx_IndicatorToNumberGreaterMinByPrecisionMsg = '<%= GetLocalResourceObject("RiskScore_ascx_IndicatorToNumberGreaterMinByPrecision.Message").ToString() %>';
        var RiskScore_uxParameterIndicatorFromEmptyMsg = '<%= GetLocalResourceObject("RiskScore_ascx_IndicatorFromRequired.Message").ToString() %>';
        var RiskScore_uxParameterIndicatorToEmptyMsg = '<%= GetLocalResourceObject("RiskScore_ascx_IndicatorToRequired.Message").ToString() %>';

        var riskParameterKeysAllowDecimal = <%= RiskParameterKeysAllowDecimal %>;

    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Score.js">
    </script>
</as:ASRadCodeBlock>
