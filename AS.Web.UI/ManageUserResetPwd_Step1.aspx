<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageUserResetPwd_Step1.aspx.cs"
    MasterPageFile="~/MasterPagePopup.master" Inherits="_mps_ResetPasswordForCS1"
    Title="Reset Password" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <p><as:Literal ID="ltAreYouSure" runat="server" Text="Are you sure that you would like to reset the password for this user?" meta:resourcekey="ltAreYouSureResource1"></as:Literal></p>
                <table class="ASTable">
                    <tr class="AltRow">
                        <td class="heading" style="width: 35%;"><as:Literal ID="ltUserName" runat="server" Text="User Name:" meta:resourcekey="ltUserNameResource1"></as:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="uxUsername" runat="server" MaxLength="30" Enabled="false" CssClass="form-control" meta:resourcekey="uxUsernameResource1" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading">
                            <as:ValidatorLabel ID="txtEmailLabel" runat="server" Text="Enter Email of User" ApplyFor="uxEmail" meta:resourcekey="txtEmailLabelResource1"/>
                        </td>
                        <td>
                            <asp:TextBox ID="uxEmail" runat="server" MaxLength="200" CssClass="form-control" meta:resourcekey="uxEmailResource1" />
                            <div class="bottom-error">
                                <as:ValidatorMessage runat="server" ID="ctrlMsg" ApplyFor="uxEmail" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxContinue" CssClass="btn btn-default" runat="server" Text="Continue"
                    ValidationGroup="ValidatePage" OnClick="uxContinue_Click" OnClientClick="return ValidateData();" meta:resourcekey="uxContinueResource1" />
                <as:Button ID="uxCancel" Text="Cancel" CssClass="btn btn-default" runat="server" OnClientClick="return closeMe();" meta:resourcekey="uxCancelResource1" />
            </div>
        </div> 
    </as:ASModalContainer>
    <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
        <Items>
            <as:RegExValidationItem ControlToValidateID="uxEmail" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" meta:resourcekey="BasicValidator3Resource1" />
            <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Required" Message="Please enter email" meta:resourcekey="BasicValidator1Resource1"/>
            <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Minlength" MinLength="2"
                Message="email must be greater than 2 characters" meta:resourcekey="BasicValidator2Resource1"/>
            <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Email" Message="Wrong email format" meta:resourcekey="BasicValidator3Resource1"/>
        </Items>
    </as:Validator>
    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript">
            var uxEmail_ClientID = '<%= uxEmail.ClientID %>';
            var uxModalContainer_ClientID = '<%= uxModalContainer.ClientID %>';
            var uxContinue_ClientID = '<%= uxContinue.ClientID %>';
            
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/usermaintenance/ResetPasswordForCS1.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
