<%@ Page Title="Confirmation" Language="C#" MasterPageFile="~/MasterPagePopup.master" 
    AutoEventWireup="true" CodeFile="ConfirmNodataModal.aspx.cs" Inherits="ConfirmNodataModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container">
        <div class="row">
            <div class="col-md-12">
                <p>
                    <as:Literal ID="msgNoDataResource" runat="server" meta:resourcekey="msgNoDataResource"></as:Literal>
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxClose" CssClass="btn btn-default" runat="server" OnClientClick="return parent.HidePopupModal();" meta:resourcekey="uxCloseResource" />
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>
