<%@ Page Title="View Full Credit Card Number" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="FullCC.aspx.cs" Inherits="FullCC" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <table class="ASTable">
                    <tr class="Row">
                        <td class="w-40 heading"><as:Literal ID="ltCreditCardNumber" runat="server" Text="Credit Card Number:" meta:resourcekey="ltCreditCardNumberResource1"></as:Literal></td>
                        <td>
                            <as:Literal ID="ltrFullCC" runat="server" meta:resourcekey="ltrFullCCResource1"></as:Literal>
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltIssuingBank" runat="server" Text="Issuing Bank:" meta:resourcekey="ltIssuingBankResource1"></as:Literal></td>
                        <td>
                            <as:Literal ID="ltrIssueBank" runat="server" meta:resourcekey="ltrIssueBankResource1"></as:Literal>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltCountry" runat="server" Text="Country:" meta:resourcekey="ltCountryResource1"></as:Literal></td>
                        <td>
                            <as:Literal ID="ltrCountry" runat="server" meta:resourcekey="ltrCountryResource1"></as:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 action-container text-right">
                <as:Button ID="uxClose" runat="server" Text="Close" OnClick="uxClose_Click" CssClass="btn btn-default" meta:resourcekey="uxCloseResource1" />
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>

