<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AdvancedFilterPage.aspx.cs" Inherits="AdvancedFilterPage" %>

<%@ Register TagName="AdvancedFilter" Src="~/UserControls/UxAdvancedFilter.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:AdvancedFilter ID="uxAdvancedFilter" runat="server" />
</asp:Content>
