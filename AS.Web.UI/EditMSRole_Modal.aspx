<%@ Page Title="Edit MS Role" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="EditMSRole_Modal.aspx.cs" Inherits="_mps_EditMSRole_Modal" ValidateRequest="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="UserControls/ManageMSRole.ascx" TagName="ManageMSRole" TagPrefix="uc" %>


<asp:Content ContentPlaceHolderID="head" runat="server" ID="uxHeader">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPage" runat="server" ID="uxMainModalContent">
    <as:ASModalContainer ID="uxModalContainer" runat="server" Width="660px" ContainerCssClass="container" WidthCssClass="modal-xxl"> 
        <uc:ManageMSRole ID="uxManageMSRole" runat="server" />
    </as:ASModalContainer>
</asp:Content>
