<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditRole_Modal.aspx.cs"
    Inherits="_mps_EditRole_Modal" Title="Edit Role" MasterPageFile="~/MasterPagePopup.master"
    ValidateRequest="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="UserControls/ManageRole.ascx" TagName="ManageRole" TagPrefix="uc" %>
<asp:Content ContentPlaceHolderID="head" runat="server" ID="uxHeader">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPage" runat="server" ID="uxMainModalContent">
    <as:ASModalContainer ID="uxModalContainer" runat="server" Width="660px" ContainerCssClass="container" WidthCssClass="modal-xxl"> 
        <uc:ManageRole ID="uxManageRole" runat="server" /> 
    </as:ASModalContainer>
</asp:Content>
