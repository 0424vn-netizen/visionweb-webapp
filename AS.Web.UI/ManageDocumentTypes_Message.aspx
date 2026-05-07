<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="ManageDocumentTypes_Message.aspx.cs" Inherits="ManageDocumentTypes_Message" meta:resourcekey="pageTitleResource" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" Width="500px" ContainerCssClass="container">
        <div class="row">
            <div class="col-xs-12">
                <asp:Literal runat="server" Text="" meta:resourcekey="MessageResource" />
            </div>
            <div class="row">
                <div class="col-xs-12 form-action-container text-right">
                    <as:Button ID="btnCancel" CssClass="btn btn-default" runat="server" OnClientClick="parent.ClosePopupModal(1);;" Text="Close" meta:resourcekey="btnCloseResource1" />
                </div>
            </div>
        </div>
    </as:ASModalContainer>  
</asp:Content>

