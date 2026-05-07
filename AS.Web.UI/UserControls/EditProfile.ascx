<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditProfile.ascx.cs" Inherits="UserControls_EditProfile" %>
<div class="row">
    <div class="col-md-12 text-center">
        <div class="risk-form edit-risk-form text-right">
            <div class="risk-form-row">
                <asp:HiddenField ID="uxProfileID" runat="server" />
                <as:ValidatorLabel ID="uxProfileTextLabel" ApplyFor="uxProfileText" runat="server" Text="Profile:" CssClass="control-label" meta:resourcekey="EditProfileASCX_Text_Profile" />
                <div class="control-inline">
                    <as:TextBox Width="300px" MaxLength="50" ID="uxProfileText" CssClass="form-control" runat="server" HintCss="hint" meta:resourcekey="uxProfileTextResource1"></as:TextBox>
                    <div class="label-error-left" style="width:300px">
                        <as:ValidatorMessage runat="server" ID="uxProfileTextErrMsg" ApplyFor="uxProfileText" Message="" ShowOnLoad="False" />
                    </div>
                </div>
            </div>
            <div class="risk-form-row">
                <span class="valign-top">
                    <as:ValidatorLabel ID="uxDescriptionLabel" ApplyFor="uxDescription" runat="server" Text="Description:" CssClass="control-label"  meta:resourcekey="EditProfileASCX_Text_Description" />
                </span>
                <div class="control-inline">
                    <as:TextBox Width="300px" TextMode="MultiLine" Rows="6" CssClass="form-control" MaxLength="500" ID="uxDescription" runat="server" HintCss="hint" meta:resourcekey="uxDescriptionResource1"></as:TextBox>
                    <div class="label-error-left" style="width:300px">
                        <as:ValidatorMessage runat="server" ID="uxDescriptionErrMsg" ApplyFor="uxDescription" Message="" ShowOnLoad="False" />
                    </div>
                </div>
            </div>
            <div class="risk-form-action">
                <as:Button ID="uxUpdate" Text="Submit" runat="server" CssClass="btn btn-default"
                    OnClick="uxUpdate_Click" OnClientClick="return doValidationOnEdit();" IsStandardButton="False" meta:resourcekey="uxUpdateResource1"></as:Button>
                <as:Button ID="uxCancel" Text="Cancel" runat="server" CssClass="btn btn-default"
                    CausesValidation="False" OnClick="uxCancel_Click" IsStandardButton="False" meta:resourcekey="uxCancelResource1"></as:Button>
            </div>
        </div>
    </div>
</div>
<as:Validator ID="Validator1" runat="server" ValidationFunction="doValidationOnEdit" MessageContainerClientID="" MessageType="Inline" meta:resourcekey="Validator1Resource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="uxProfileText" Rule="Required" ResMessage="Resources.MessageManager.Field_RequireAndUnique" />
        <as:BasicValidationItem ControlToValidateID="uxProfileText" Rule="Maxlength" MaxLength="50" ResMessage="Resources.MessageManager.Generic_FieldLengthExceeded" ResParams="50" />
        <as:BasicValidationItem ControlToValidateID="uxDescription" Rule="Maxlength" MaxLength="500" ResMessage="Resources.MessageManager.Generic_FieldLengthExceeded" ResParams="500" />
    </Items>
</as:Validator>
