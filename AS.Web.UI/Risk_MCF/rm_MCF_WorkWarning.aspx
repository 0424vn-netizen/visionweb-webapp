<%@ Page Title="Warning" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_WorkWarning.aspx.cs" Inherits="rm_MCF_WorkWarning" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container">
        <div class="row">
            <div class="col-md-12">
                <p>
                    <as:Literal ID="uxLtWarning" runat="server" Text=""></as:Literal>
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxRefresh" CssClass="btn btn-default" runat="server" Text="Refresh" OnClick="uxRefresh_Click" meta:resourcekey="uxRefreshResource" />
                <as:Button ID="uxCancel" Text="Cancel" CssClass="btn btn-default" runat="server" OnClientClick="return parent.HidePopupModal();" meta:resourcekey="uxCancelResource" />
            </div>
        </div>
        <script>
            parent.closeCurrentDisposition();
        </script>
    </as:ASModalContainer>
</asp:Content>
