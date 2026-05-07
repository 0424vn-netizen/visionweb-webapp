
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MerchantNoteToDelete.aspx.cs" Inherits="rm_MCF_MerchantNoteToDelete" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/UserControls/rm_MCF_MerchantNoteToDelete.ascx" TagName="ucMerchantNoteToDelete" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" ContainerCssClass="container" WidthCssClass="modal-md">
        <uc:ucMerchantNoteToDelete runat="server" ID="ucMerchantNoteToDelete" />
    </as:ASModalContainer>
</asp:Content>


