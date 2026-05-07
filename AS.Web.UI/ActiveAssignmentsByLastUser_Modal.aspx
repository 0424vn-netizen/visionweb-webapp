<%@ Page Title="ACTIVE ASSIGNMENTS" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="ActiveAssignmentsByLastUser_Modal.aspx.cs" Inherits="_mps_ActiveAssignmentsByLastUser_Modal" meta:resourcekey="PageResource1" %>
<%@ Register TagName="AssignmentList" Src="~/UserControls/AssignmentListSimple.ascx" TagPrefix="uc" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="uxHeader">   
  
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPage" runat="server" ID="uxMainModalContent">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">
    <asp:Label ID="uxMessage" runat="server" Font-Bold="true" meta:resourcekey="uxMessageResource1" />
    <br /><br /> 
    <uc:AssignmentList ID="uxAssignmentList" runat="server" />
    </as:ASModalContainer>
</asp:Content>