<%@ Page Language="C#" AutoEventWireup="true" CodeFile="forgotPasswordMsg.aspx.cs" Title="Forgot Password"
    Inherits="_mps_fdc_forgotPasswordMsg" MasterPageFile="~/MasterPagePopup.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm">
        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <asp:Literal runat="server" ID="uxMessage" />
                </div>
            </div>
        </div> 
        <div class="row">
            <div class="col-md-12 action-container text-right">
                <asp:PlaceHolder ID="uxPhdTryAgain" runat="server" Visible="false">
                    <as:Button ID="uxTryAgain" CssClass="btn btn-default" Text="Try Again" runat="server" />
                </asp:PlaceHolder>
                <as:Button ID="uxClose" runat="server" Text="Close Window" class="btn btn-default"
                    OnClientClick="CloseMsgBox();" />
            </div>
        </div>
    </as:ASModalContainer>
     <script src="<%= ResolveUrl("~/")%>res/js/forgotPasswordMsg.js"></script>
</asp:Content>
