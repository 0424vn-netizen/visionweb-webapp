<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_ExportQueueNotifyModal.aspx.cs" MasterPageFile="~/MasterPagePopup.master"
    Inherits="rm_MCF_ExportQueueNotifyModal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container" Width="">
        <h2 class="modal-title">
            <asp:Label ID="lbTitle" runat="server" Text="Export Request"></asp:Label>
        </h2>
        <div class="row">
            <div class="col-md-12">
                <p>
                    <asp:Label ID="lbMessage" runat="server" Text="Your export is being prepared. Once it’s ready, you can download it from the Exports page."></asp:Label>
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-right form-action-container">
                <as:Button ID="btnOK" runat="server" OnClientClick="return parent.HidePopupModal();" Text="OK" CssClass="btn btn-default" />
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>