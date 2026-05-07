<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="EditASUserConfirmModal.aspx.cs" Inherits="EditASUserConfirmModal" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <p>
                    <as:Literal ID="ltAreYouSure" runat="server" Text="Are you sure you want to save changes to this user?" meta:resourcekey="ltAreYouSureResource"></as:Literal>
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxSave" CssClass="btn btn-default" runat="server" Text="Continue"
                    ValidationGroup="ValidatePage" OnClientClick="parent.SaveChanged(); return false;" meta:resourcekey="uxSaveResource" />
                <as:Button ID="uxCancel" Text="Cancel" CssClass="btn btn-default" runat="server" OnClientClick="return parent.HidePopupModal();" meta:resourcekey="uxCancelResource" />
            </div>
        </div>
    </as:ASModalContainer>

    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript"> 
        </script>
    </tek:RadCodeBlock>
</asp:Content>
