<%@ Page Title="MasterCard Acquirer Fraud Report" Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MasterCardAcquirerFraudReport.aspx.cs" Inherits="rm_MCF_MasterCardAcquirerFraudReport" MasterPageFile="~/MasterPage.master" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_AcquirerFraudReport.ascx" TagName="AcquirerFraudReport" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:AcquirerFraudReport ID="acquirerFraudReport" runat="server" CardType="MasterCard" />
</asp:Content>