<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateNewUser.aspx.cs" Title="Create User"
    Inherits="As.VisionWeb.Web.CreateNewUser" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" %>

<%@ Register Src="UserControls/ManageUser.ascx" TagName="ManageUser" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title">
                    <asp:Literal ID="lblHeader" runat="server" Text="Create New User" meta:resourcekey="lblHeaderResource1"></asp:Literal>
                </h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12"> 
                <uc:ManageUser ID="uxManageUser" runat="server" IsUpdateMode="false" /> 
            </div>
        </div>
    </as:ASModalContainer> 
</asp:Content>
