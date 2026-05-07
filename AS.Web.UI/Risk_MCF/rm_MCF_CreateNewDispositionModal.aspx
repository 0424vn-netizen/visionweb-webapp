<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1"
    AutoEventWireup="true" CodeFile="rm_MCF_CreateNewDispositionModal.aspx.cs" Inherits="rm_MCF_CreateNewDispositionModal" %>

<%@ Register TagName="NewDisposition" Src="~/UserControls/rm_MCF_NewDisposition.ascx" TagPrefix="uc" %>

<asp:Content ContentPlaceHolderID="ContentPage" runat="Server" ID="uxMainModalContent">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-lg">
        <uc:NewDisposition ID="uxUpdateProfile" runat="server" />
    </as:ASModalContainer>
</asp:Content>
