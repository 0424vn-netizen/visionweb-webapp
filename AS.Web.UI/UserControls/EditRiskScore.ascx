<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditRiskScore.ascx.cs" Inherits="UserControls_EditRiskScore" %>

<as:HiddenField ID="uxRecordID" runat="server" Value='<%# Eval( "RecordID" ).ToString() %>' />
<as:HiddenField ID="uxParameterKey" runat="server" Value='<%# Eval( "ParameterKey" ).ToString() %>' />
<as:HiddenField ID="uxParameterThresholdEnabled" runat="server" Value='<%# Eval( "ParameterThresholdEnabled" ).ToString() %>' />
<as:HiddenField ID="uxParameterIsThresholdNegative" runat="server" Value='<%# Eval("IsThresholdNegative") != DBNull.Value && Eval("IsThresholdNegative").ToString() != string.Empty && ((bool)Eval("IsThresholdNegative")) ? "1" : "0" %>' />
<as:HiddenField ID="uxParameterIsIndicatorNegative" runat="server" Value='<%# Eval("IsIndicatorNegative") != DBNull.Value && Eval("IsIndicatorNegative").ToString() != string.Empty && ((bool)Eval("IsIndicatorNegative")) ? "1" : "0" %>' />
<as:HiddenField ID="uxParameterIsNullIndicator" runat="server" Value='<%# Eval("ParameterIndicator") != DBNull.Value && Eval("ParameterIndicator").ToString() != string.Empty ? "1" : "0" %>' />
<as:HiddenField ID="uxParameterThresholdTypeHigh" runat="server" Value='<%# Eval("ParameterThresholdType") != DBNull.Value && Eval("ParameterThresholdType").ToString() != string.Empty ? "1" : "0" %>' />
<%--//[42397] - Fixbug 37367--%>
<as:HiddenField ID="hddParameterPrecision" runat="server" Value='<%# !string.IsNullOrEmpty(Eval("ParameterPrecision").ToString()) && Eval("ParameterPrecision").ToString() != "0" ? "1" : "0" %>' />

<as:HiddenField runat="server" ID="uxHasHighThreadhold" Value="1" />

<div class="text-center">
    <div class="risk-form">
        <div class="risk-form-row">
            <asp:HiddenField ID="uxParameterPrecision" runat="server" />
            <asp:HiddenField ID="uxParameterIndicatorNegative" runat="server" />
            <as:ValidatorLabel ID="uxParameterIndicatorFromLabel" runat="server" Text="From:" ApplyFor="uxParameterIndicatorFrom" CssClass="control-inline-label first" meta:resourcekey="EditRiskScoreASCX_Text_From" />

            <div class="control-inline">
                <asp:Label ID="uxLabBeginFrom" runat="server" CssClass="red" Visible="False" meta:resourcekey="uxLabBeginFromResource1">(</asp:Label>
                <as:TextBox Width="100px" MaxLength="9" ID="uxParameterIndicatorFrom"
                    Text='<%# Eval("ParameterIndicatorFrom") %>' runat="server" HintCss="hint" meta:resourcekey="uxParameterIndicatorFromResource1">
                </as:TextBox>
                <asp:Label ID="uxLabEndFrom" runat="server" CssClass="red" Visible="False" meta:resourcekey="uxLabEndFromResource1">)</asp:Label>
            </div>

            <as:ValidatorLabel ID="uxParameterIndicatorToLabel" runat="server" Text="To:" ApplyFor="uxParameterIndicatorTo" CssClass="control-inline-label" meta:resourcekey="EditRiskScoreASCX_Text_To" />

            <div class="control-inline">
                <asp:Label ID="uxLabBeginTo" runat="server" Style="color: Red" Visible="False" meta:resourcekey="uxLabBeginToResource1">(</asp:Label>
                <as:TextBox Width="100px" MaxLength="9" ID="uxParameterIndicatorTo"
                    Text='<%# Eval( "ParameterIndicatorTo" ) %>' runat="server" HintCss="hint" meta:resourcekey="uxParameterIndicatorToResource1">
                </as:TextBox>
                <asp:Label ID="uxLabEndTo" runat="server" Style="color: Red" Visible="False" meta:resourcekey="uxLabEndToResource1">)</asp:Label>
            </div>

            <as:ValidatorLabel ID="uxParameterThresholdLabel" runat="server" Text="Threshold:" ApplyFor="uxParameterThreshold" CssClass="control-inline-label" meta:resourcekey="EditRiskScoreASCX_Text_Threshold" />
            <div class="control-inline">

                <label class="label-low">
                    <as:Literal ID="uxLow" runat="server" Visible='<%# Eval( "ParameterThresholdType").ToString()=="LowHigh"?true:false %>'
                        Text="Low $" meta:resourcekey="uxLowResource1">
                    </as:Literal>
                </label>

                <asp:Label ID="uxLabBeginThreshold" runat="server" Style="color: Red" Visible="False" meta:resourcekey="uxLabBeginThresholdResource1">(</asp:Label>
                <as:TextBox Width="100px" MaxLength="9" ID="uxParameterThreshold"
                    Text='<%# Eval( "ParameterThreshold" ) %>' runat="server" HintCss="hint" meta:resourcekey="uxParameterThresholdResource1">
                </as:TextBox>
                <asp:Label ID="uxLabEndThreshold" runat="server" Style="color: Red" Visible="False" meta:resourcekey="uxLabEndThresholdResource1">)</asp:Label>

                <as:PlaceHolder ID="uxPlcParamThresholdHigh" runat="server" Visible='<%# Eval( "ParameterThresholdType").ToString()=="LowHigh"?true:false %>'>
                    <label class="label-high">
                        <asp:Literal ID="Literal1" runat="server" Text="High $" meta:resourcekey="LiteralResource1" />
                    </label>

                    <asp:Label ID="uxLabBeginThresholdHigh" runat="server" Style="color: Red" Visible="false" meta:resourcekey="uxLabBeginThresholdHighResource1">(</asp:Label>
                    <as:TextBox Width="100px" MaxLength="9" ID="uxParameterThresholdHigh"
                        Text='<%# Eval( "ParameterThresholdHigh" ) %>' runat="server" ValidationGroup="ValidateData" meta:resourcekey="uxParameterThresholdHighResource1">
                    </as:TextBox>
                    <asp:Label ID="uxLabEndThresholdHigh" runat="server" Style="color: Red" Visible="false" meta:resourcekey="uxLabEndThresholdHighResource1">)</asp:Label>
                </as:PlaceHolder>
            </div>

            <as:ValidatorLabel ID="uxParameterScoreLabel" runat="server" Text="Score:" ApplyFor="uxParameterScore" CssClass="control-inline-label" meta:resourcekey="EditRiskScoreASCX_Text_Score" />

            <div class="control-inline">
                <asp:Label ID="uxLabBeginScore" runat="server" Style="color: Red" Visible="False" meta:resourcekey="uxLabBeginScoreResource1">(</asp:Label>
                <as:TextBox Width="100px" MaxLength="9" ID="uxParameterScore" Text='<%# Eval( "ParameterScore" ) %>'
                    runat="server" ValidationGroup="ValidateData" HintCss="hint" meta:resourcekey="uxParameterScoreResource1">
                </as:TextBox>
                <asp:Label ID="uxLabEndScore" runat="server" Style="color: Red" Visible="False" meta:resourcekey="uxLabEndScoreResource1">)</asp:Label>
            </div>
            <div class="risk-form-action-inline">
                <asp:Button CssClass="btn btn-default" runat="server" Text="Submit" CommandName="Update" ID="uxSubmit" OnClientClick="return doValidationForEdit();" meta:resourcekey="uxSubmitResource1" />
                <asp:Button CssClass="btn btn-default" runat="server" Text="Cancel" CommandName="Cancel" ID="uxCancel" meta:resourcekey="uxCancelResource1" />
            </div>
            <div class="text-center">
                <div class="bottom-error">
                    <as:ValidatorMessage runat="server" ID="uxParameterIndicatorFromMsg" ApplyFor="uxParameterIndicatorFrom" Message="" ShowOnLoad="False" />
                </div>
                <div class="bottom-error">
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

<as:Validator ID="ctrlValidatorForEdit" runat="server" ValidationFunction="doValidationForEdit" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="ctrlValidatorForEditResource1">
    <Items>
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorFrom" ClientValidationFunction="paramIndicatorFromRequiredForEdit" ResMessage="Resources.ValMsg.CustomRequired" ResParams="Indicator From" />
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorFrom" ClientValidationFunction="paramIndicatorFromValidForEdit" meta:resourcekey="RiskScore_ascx_IndicatorFromNumberGreaterOrEqualThan" />
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorFrom" ClientValidationFunction="CheckValidFromPrecisionForEdit" meta:resourcekey="RiskScore_ascx_IndicatorFromPrecision" />


        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorTo" ClientValidationFunction="paramIndicatorToRequiredForEdit" ResMessage="Resources.ValMsg.CustomRequired" ResParams="Indicator To" />
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorTo" ClientValidationFunction="paramIndicatorToValidForEdit" meta:resourcekey="RiskScore_ascx_IndicatorToNumberGreaterOrEqualThan" />
        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorTo" ClientValidationFunction="CheckValidToPrecisionForEdit" meta:resourcekey="RiskScore_ascx_IndicatorToPrecision" />

        <as:CustomValidationItem ControlToValidateID="uxParameterIndicatorTo" ClientValidationFunction="compareFromToForEdit" ResMessage="Resources.ValMsg.CustomTo_Greater" ResParams="Indicator To,Indicator From" />
        <as:CustomValidationItem ControlToValidateID="uxParameterThreshold" ClientValidationFunction="paramThresholdValidForEdit" ResMessage="Resources.ValMsg.CustomNumber" ResParams="Threshold" />

        <%-- 42733 - Bug #37105--%>
        <%--<as:BasicValidationItem ControlToValidateID="uxParameterIndicatorFrom" Rule="Integer" meta:resourcekey="RiskScore_ascx_IndicatorFromNumberGreaterOrEqualThan" />
        <as:BasicValidationItem ControlToValidateID="uxParameterIndicatorTo" Rule="Integer" meta:resourcekey="RiskScore_ascx_IndicatorToNumberGreaterOrEqualThan" />--%>


        <as:CustomValidationItem ControlToValidateID="uxParameterThreshold" ClientValidationFunction="paramThresholdLowRequiredForEdit" ResMessage="Resources.ValMsg.CustomRequired" ResParams="Threshold Low $" />
        <as:CustomValidationItem ControlToValidateID="uxParameterThresholdHigh" ClientValidationFunction="paramThresholdHighRequiredForEdit" ResMessage="Resources.ValMsg.CustomRequired" ResParams="Threshold High $" />
        <as:CustomValidationItem ControlToValidateID="uxParameterThresholdHigh" ClientValidationFunction="paramThresholdHighValidForEdit" ResMessage="Resources.ValMsg.CustomNumber" ResParams="Threshold High $" />

        <as:CustomValidationItem ControlToValidateID="uxParameterThreshold" ClientValidationFunction="paramThresholdLowGreaterThanZero_ForEdit" ResMessage="Resources.ValMsg.CustomGreaterThanZero" ResParams="Low $" />

        <as:CustomValidationItem ControlToValidateID="uxParameterThreshold" ClientValidationFunction="paramThresholdHigh_gt_Low_ForEdit" ResMessage="Resources.ValMsg.CustomTo_Greater" ResParams="High $,Low $" />



        <as:CustomValidationItem ControlToValidateID="uxParameterScore" ClientValidationFunction="paramIndicatorScoreRequiredForEdit" ResMessage="Resources.ValMsg.CustomRequired" ResParams="Score" />
        <as:CustomValidationItem ControlToValidateID="uxParameterScore" ClientValidationFunction="paramIndicatorScoreValidForEdit" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />
        <as:CustomValidationItem ControlToValidateID="uxParameterScore" ClientValidationFunction="ValidateExistingForEdit" ResMessage="Resources.ValMsg.RiskScore_Overlap" />
        <as:CustomValidationItem ControlToValidateID="uxParameterScore" ClientValidationFunction="ValidateIntergerForEdit" meta:resourcekey="RiskScore_ascx_ScoreNumberGreaterOrEqualThan" />

    </Items>
</as:Validator>

<as:ASRadCodeBlock ID="RadCodeBlock" runat="server">
    <script type="text/javascript">
        var EditRiskScore_uxSubmit = '<%=uxSubmit.ClientID%>';
        var EditRiskScore_uxParameterIndicatorFrom = '<%=uxParameterIndicatorFrom.ClientID%>';
        var EditRiskScore_uxParameterIndicatorTo = '<%=uxParameterIndicatorTo.ClientID%>';
        var EditRiskScore_uxParameterThreshold = '<%=uxParameterThreshold.ClientID%>';
        var EditRiskScore_uxParameterThresholdHigh = '<%=uxParameterThresholdHigh.ClientID%>';

        var EditRiskScore_uxParameterScore = '<%=uxParameterScore.ClientID%>';
        var EditRiskScore_uxRecordID = '<%=uxRecordID.ClientID%>';
        var EditRiskScore_uxParameterKey = '<%=uxParameterKey.ClientID%>';
        var EditRiskScore_uxParameterIsThresholdNegative = '<%=uxParameterIsThresholdNegative.ClientID%>';
        var EditRiskScore_uxParameterIsIndicatorNegative = '<%=uxParameterIsIndicatorNegative.ClientID%>';
        var EditRiskScore_uxParameterIsNullIndicator = '<%=uxParameterIsNullIndicator.ClientID%>';
        var EditRiskScore_uxHasHighThreadhold = '<%=uxHasHighThreadhold.ClientID%>';
        //[42397] - Fixbug 37367
        var EditRiskScore_hddParameterPrecision = '<%=hddParameterPrecision.ClientID%>';

        var EditRiskScore_uxParameterPrecision = '<%=uxParameterPrecision.ClientID%>';
        var EditRiskScore_uxParameterIndicatorNegative = '<%=uxParameterIndicatorNegative.ClientID%>';

        

    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk/EditRiskScore.js">
    </script>
</as:ASRadCodeBlock>
