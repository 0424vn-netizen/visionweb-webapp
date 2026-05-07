<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" 
    AutoEventWireup="true" CodeFile="LoadSavedFilterConfirmModal.aspx.cs" Inherits="LoadSavedFilterConfirmModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container">
        <div class="row">
            <div class="col-md-12">
                <p>
                    <as:Literal ID="ltAreYouSure" runat="server" meta:resourcekey="ltAreYouSureResource"></as:Literal>
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxContinue" CssClass="btn btn-default" runat="server" Text="Delete" OnClientClick="parent.advancedFilter.currentFilter.continueLoadSavedFilter(); return false;" meta:resourcekey="uxContinueResource" />
                <as:Button ID="uxCancel" Text="Cancel" CssClass="btn btn-default" runat="server" OnClientClick="return parent.HidePopupModal();" meta:resourcekey="uxCancelResource" />
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>
