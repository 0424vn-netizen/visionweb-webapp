<%@ Page Language="C#" AutoEventWireup="true" CodeFile="forgotPassword2.aspx.cs" Title="Forgot Password"
    MasterPageFile="~/MasterPagePopup.master" Inherits="_mps_forgotpassword2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm">
        <div class="row">
            <div class="col-md-12">
                <i>Please answer the security validation question below.</i>
                <table class="ASTable">
                    <tr class="AltRow">
                        <td class="heading">Question:</td>
                        <td>
                            <asp:Literal ID="uxQuestion" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td  class="heading"><as:ValidatorLabel ID="txtAnswerLabel" runat="server" Text="Answer:" ApplyFor="uxAnswer" />
                        </td>
                        <td >
                            <as:TextBox ID="uxAnswer" runat="server" CssClass="form-control" MaxLength="50" autocomplete="off"/>
                            <as:ValidatorMessage ID="txtAnswerErrMsg" runat="server" ApplyFor="uxAnswer" ></as:ValidatorMessage>
                        </td>
                    </tr> 
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 action-container text-right">
                <as:Button ID="uxContinue" CssClass="btn btn-default" runat="server" Text="Continue"
                    OnClientClick="return ValidateAndAdjustModal();" OnClick="uxContinue_Click" />
            </div>
        </div>
    </as:ASModalContainer> 
    <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput">
        <Items>
            <as:BasicValidationItem ControlToValidateID="uxAnswer" Rule="Required" Message="This field is a required field." />
        </Items>
    </as:Validator>
    <as:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script src="<%= ResolveUrl("~/")%>res/js/forgotPassword2.js"></script>
    </as:RadCodeBlock>
</asp:Content>
