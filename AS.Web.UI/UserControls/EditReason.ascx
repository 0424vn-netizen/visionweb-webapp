<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditReason.ascx.cs" Inherits="UserControls_EditReason" %>
<div class="row">
    <div class="col-md-12 text-center">
        <div class="risk-form edit-risk-form">
            <div class="risk-form-row">
                <asp:HiddenField ID="uxReasonID" runat="server" />
                <as:ValidatorLabel ID="uxEscalationTextLabel" runat="server" ApplyFor="uxEscalationText" Text="Reason:" CssClass="control-label" meta:resourcekey="EditReasonASCX_Text_Reason"></as:ValidatorLabel>
                <div class="control-inline">
                    <as:TextBox Width="400px" MaxLength="100" ID="uxEscalationText" CssClass="form-control"
                        runat="server" ValidationGroup="Validate" HintCss="hint" meta:resourcekey="uxEscalationTextResource1"></as:TextBox>
                    <div class="bottom-error text-left">
                        <as:ValidatorMessage ID="uxEscalationTextErrMsg" runat="server" ApplyFor="uxEscalationText" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                    </div>
                </div>
                <div class="risk-form-action-inline valign-top">
                    <as:Button ID="uxEscalationUpdate" Text="Submit" runat="server" CssClass="btn btn-default"
                        OnClientClick="return doValidationOnEdit();" OnClick="uxEscalationUpdate_Click" IsStandardButton="False" meta:resourcekey="uxEscalationUpdateResource1"></as:Button>
                    <as:Button ID="uxEscalationCancel" Text="Cancel" runat="server" CssClass="btn btn-default"
                        CausesValidation="False" OnClick="uxEscalationCancel_Click" IsStandardButton="False" meta:resourcekey="uxEscalationCancelResource1"></as:Button>
                </div>
            </div>
        </div>
    </div>
</div>
<as:Validator ID="Validator2" runat="server" ValidationFunction="doValidationOnEdit" MessageContainerClientID="" MessageType="Inline" meta:resourcekey="Validator2Resource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxEscalationText" Rule="Required" ResMessage="Resources.MessageManager.Field_RequireAndUnique" />
        <as:BasicValidationItem ControlToValidateID="uxEscalationText" Rule="Maxlength" MaxLength="100" ResMessage="Resources.MessageManager.Generic_FieldLengthExceeded" ResParams="100" />
    </Items>
</as:Validator>
