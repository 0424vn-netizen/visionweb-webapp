<%@ Page Title="Find Merchants" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_MgmtReport_MerchantFilter.aspx.cs" Inherits="rm_MCF_MgmtReport_MerchantFilter" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_MgmtReport_MerchantFilter.ascx" TagName="MerchantFilter" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer runat="server" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">
        <uc:MerchantFilter ID="uxMerchantFilter" runat="server" />
    </as:ASModalContainer>
</asp:Content>

