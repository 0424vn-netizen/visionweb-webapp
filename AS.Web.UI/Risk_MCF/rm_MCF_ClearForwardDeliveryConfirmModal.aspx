<%@ Page Title="Confirmation" Language="C#" MasterPageFile="~/MasterPagePopup.master" 
    AutoEventWireup="true" CodeFile="rm_MCF_ClearForwardDeliveryConfirmModal.aspx.cs" Inherits="rm_MCF_ClearForwardDeliveryConfirmModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container">
        <div class="row">
            <div class="col-md-12">
                <p>
                    <as:Literal ID="ltAreYouSure" runat="server" Text="You are about to permanently clear the Client Timeliness Days, NDX and  associated calculated values. Do you wish to proceed?" meta:resourcekey="ltAreYouSureResource"></as:Literal>
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxDelete" CssClass="btn btn-default" runat="server" Text="OK" OnClientClick="parent.ClearForwardDelivery(); return false;" meta:resourcekey="uxDeleteResource" />
                <as:Button ID="uxCancel" Text="Cancel" CssClass="btn btn-default" runat="server" OnClientClick="return parent.HidePopupModal();" meta:resourcekey="uxCancelResource" />
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>
