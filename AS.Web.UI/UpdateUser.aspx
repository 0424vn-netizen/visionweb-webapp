<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" Title="Edit User"
    CodeFile="UpdateUser.aspx.cs" Inherits="As.VisionWeb.Web.UpdateUser" meta:resourcekey="PageResource1" %>

<%@ Register Src="UserControls/ManageUser.ascx" TagName="ManageUser" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title">
                    <asp:Literal ID="lblHeader" runat="server" Text="Update User" meta:resourcekey="lblHeaderResource1"></asp:Literal>
                </h3>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12"> 
                <uc:ManageUser ID="uxManageUser" runat="server" IsUpdateMode="true" /> 
            </div>
        </div>
    </as:ASModalContainer> 
</asp:Content>
