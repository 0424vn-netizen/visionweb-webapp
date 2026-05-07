<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="forgotPassword1.aspx.cs" Inherits="_mps_forgotpassword1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm">
        <div class="row">
            <div class="col-md-12">
                <p>Please verify your User Name and Email.</p>
                <table class="ASTable">
                    <colgroup>
                        <col width="90px" />
                    </colgroup>
                    <tr class="AltRow">
                        <td class="heading " >
                            <as:ValidatorLabel ID="txtUsernameLabel" runat="server" Text="User Name:" ApplyFor="uxUsername" />
                        </td>
                        <td>
                            <as:TextBox autocomplete="off" ID="uxUsername" CssClass="form-control" runat="server" MaxLength="50" autocorrect="off"></as:TextBox>
                            <as:ValidatorMessage runat="server" ID="txtUserNameErrMsg" ApplyFor="uxUsername" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading">
                            <as:ValidatorLabel ID="ValidatorLabel1" runat="server" Text="Email:" ApplyFor="uxEmail" />
                        </td>
                        <td>
                            <as:TextBox  autocomplete="off" ID="uxEmail" runat="server" CssClass="form-control" MaxLength="200" autocorrect="off"></as:TextBox>
                            <as:ValidatorMessage runat="server" ID="ValidatorMessage1" ApplyFor="uxEmail" />
                        </td>
                    </tr>
                    <tr> 
                        <td colspan="2" class="red text-center">
                            If you are having difficulties, please call Customer Service.
                        </td>
                    </tr>
                </table>
            </div>
        </div> 
        <div class="row">
            <div class="col-md-12 action-container text-right">
                <as:Button ID="btnContinue" CssClass="btn btn-default" runat="server" Text="Continue" OnClick="btnContinue_Click"
                    OnClientClick="return ValidateInput();" /> 
            </div>
        </div>
    </as:ASModalContainer>
    <as:Validator ID="Validator1" runat="server" ValidationFunction="doValidateInput" MessageType="Inline">
        <Items>
            <as:BasicValidationItem ControlToValidateID="uxUsername" Rule="Required" Message="This field is a required field." />
            <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Required" Message="This field is a required field." />
            <as:BasicValidationItem ControlToValidateID="uxEmail" Rule="Email" Message="You have entered an incorrect email address format.  Please try again." />
        </Items>
    </as:Validator>
    <as:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
        <script src="<%= ResolveUrl("~/")%>res/js/forgotPassword1.js"></script>
    </as:RadCodeBlock>
</asp:Content>
