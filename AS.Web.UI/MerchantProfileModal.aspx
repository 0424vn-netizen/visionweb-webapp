<%@ Page Title="Merchant Information" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="MerchantProfileModal.aspx.cs" Inherits="MerchantProfileModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-md" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title">
                    <as:Literal runat="server" ID="uxTitle" meta:resourcekey="uxTitleResource1" />
                </h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <as:Literal ID="ltAreYouSure" runat="server" Text="Are you sure you would like to opt" meta:resourcekey="ltAreYouSureResource1"></as:Literal>
                    <as:Literal runat="server" ID="idMsg" meta:resourcekey="idMsgResource1" />
                    <div class="height-12"></div>
                    <div class="dark-blue">
                        <strong>
                            <as:Literal runat="server" ID="idMerchantNum" Text="Empty Text" meta:resourcekey="idMerchantNumResource1" /></strong>
                    </div>
                    <asp:PlaceHolder runat="server" ID="idEmailHolder">
                        <div class="height-10"></div>
                        <as:ValidatorLabel ID="ValidatorLabel1" runat="server" ApplyFor="idEmail" Text="Enter Email of merchant you would like to have the password sent to:" meta:resourcekey="MerchantProfileModal_aspx_EmailMerchantYouSentTo">
                        </as:ValidatorLabel>
                        <as:TextBox runat="server" ID="idEmail" Width="100%" CssClass="form-control" meta:resourcekey="idEmailResource1" />
                        <div class="bottom-error">
                            <as:ValidatorMessage ID="ValidatorMessage1" runat="server" ApplyFor="idEmail"></as:ValidatorMessage>
                        </div>
                        <div class="height-16"></div>
                        <as:ValidatorLabel ID="ValidatorLabel2" runat="server" ApplyFor="uxMaxSec" Text="Maximum number of secondary users:" meta:resourcekey="uxValidatorLabel1"></as:ValidatorLabel>
                        <as:TextBox ID="uxMaxSec" runat="server" MaxLength="4" Text="0" Width="100%" CssClass="form-control" meta:resourcekey="uxMaxSecResource1" />
                        <div class="bottom-error">
                            <as:ValidatorMessage ID="ValidatorMessage2" runat="server" ApplyFor="uxMaxSec"></as:ValidatorMessage>
                        </div>

                    </asp:PlaceHolder>
                    <div>
                        <div class="height-16"></div>
                        <as:ValidatorLabel ID="ValidatorLabel3" runat="server" ApplyFor="uxCommentText" Text="Add Comment:" meta:resourcekey="uxValidatorLabel2"></as:ValidatorLabel>
                        <as:TextBox ID="uxCommentText" runat="server" TextMode="MultiLine" MaxLength="500"
                            onkeyup="checkComment()" Height="80" CssClass="form-control" Width="100%" meta:resourcekey="uxCommentTextResource1" />
                        <div class="bottom-error">
                            <as:ValidatorMessage ID="ValidatorMessage3" runat="server" ApplyFor="uxCommentText"></as:ValidatorMessage>
                        </div>
                        <%= GetLocalResourceObject("MerchantProfileModal_aspx_TextLimitInComment").ToString() %>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-right form-action-container">
                <as:Button ID="uxSubmit" runat="server" Text="Submit" OnClick="uxSubmit_Click" CssClass="btn btn-default" OnClientClick="return validator();" meta:resourcekey="uxSubmitResource1" />
                <as:Button ID="uxCancel" runat="server" Text="Cancel" OnClientClick="return parent.HidePopupModal()" CssClass="btn btn-default" meta:resourcekey="uxCancelResource1" />
            </div>
        </div>
        <as:Validator ID="Validator1" runat="server" ValidationFunction="dovalitation" MessageType="Inline" meta:resourcekey="ValidatorResource1">
            <Items>
                <as:BasicValidationItem ControlToValidateID="idEmail" Rule="Required"  ResMessage="Resources.ValMsg.Required" />
                <as:BasicValidationItem ControlToValidateID="idEmail" Rule="Email"  ResMessage="Resources.ValMsg.InvalidEmail" />
                <as:BasicValidationItem ControlToValidateID="uxMaxSec" Rule="Required"  ResMessage="Resources.ValMsg.Required" />
                <as:BasicValidationItem ControlToValidateID="uxMaxSec" Rule="Number"  ResMessage="Resources.ValMsg.Number" />
                <as:BasicValidationItem ControlToValidateID="uxMaxSec" Rule="GreaterOrEqualThan" MinValue="0"  Message="The predefined number must be greater than or equal to 0." meta:resourcekey="MerchantProfileModal_aspx_BV_NumberMustBe" />
                <as:BasicValidationItem ControlToValidateID="uxCommentText" Rule="Required"  ResMessage="Resources.ValMsg.Required" />
                <as:BasicValidationItem ControlToValidateID="uxCommentText" Rule="StringUnaccept" Pattern="<>"  ResMessage="Resources.ValMsg.InvalidCharacter" />
                <as:BasicValidationItem ControlToValidateID="uxCommentText" Rule="Maxlength" MaxLength="1000"  ResMessage="Resources.ValMsg.MaxLength" ResParams="1000" />
            </Items>
        </as:Validator>
    </as:ASModalContainer>
    <script type="text/javascript">
        var MerchantProfileModal_uxCommentText = $("#<%=uxCommentText.ClientID %>");
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MerchantProfileModal.js"></script>
</asp:Content>
