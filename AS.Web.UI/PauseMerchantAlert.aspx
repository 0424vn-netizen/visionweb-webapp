<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PauseMerchantAlert.aspx.cs" Inherits="PauseMerchantAlert" %>

<%@ Register TagName="PauseMerchantAlert" Src="~/UserControls/rm_MCF_PauseMerchantAlert.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:PauseMerchantAlert ID="uxPauseMerchantAlert" runat="server" />
</asp:Content>
