
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MerchantNoteToEdit.aspx.cs" Inherits="rm_MCF_MerchantNoteToEdit" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/UserControls/rm_MCF_MerchantNoteToEdit.ascx" TagName="ucMerchantNoteToEdit" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" ContainerCssClass="container" WidthCssClass="modal-md">
        <uc:ucMerchantNoteToEdit runat="server" ID="ucMerchantNoteToEdit" />
    </as:ASModalContainer>
</asp:Content>


