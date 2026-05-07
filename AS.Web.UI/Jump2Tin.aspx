<%@ Page Title="CompliAssure" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Jump2Tin.aspx.cs" Inherits="Jump2Tin" meta:resourcekey="PageResource1" %>    
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="Jum2Tin" Src="~/UserControls/Jump2Tin.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Site Jump - CompliAssure" HasFilteringOption="false" meta:resourcekey="uxPageTitleResource1" />
<uc:Jum2Tin ID="uxJump2Tin" runat="server" />
</asp:Content>

