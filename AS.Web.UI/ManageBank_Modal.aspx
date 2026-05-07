<%@ Page Language="C#" AutoEventWireup="true" Title="Add Banks" CodeFile="ManageBank_Modal.aspx.cs" Inherits="ManageBank_Modal" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/UserControls/Lead/LeadManageBank.ascx" TagName="ManageBank" TagPrefix="uc" %>

<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12"> 
                <uc:AddBank runat="server" ID="uxAddBank" RoleId="return GetSelectedRole()" />
            </div>
        </div>
    </as:ASModalContainer> 
</asp:Content>--%>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="uxHeader">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPage" runat="server" ID="uxMainModalContent1">
    <as:ASModalContainer ID="uxModalContainer1" runat="server" Width="660px" ContainerCssClass="container" 
        WidthCssClass="modal-xxl"> 
         <uc:ManageBank runat="server" ID="uxManageBank" RoleId="return GetSelectedRole()" />
    </as:ASModalContainer>
</asp:Content>