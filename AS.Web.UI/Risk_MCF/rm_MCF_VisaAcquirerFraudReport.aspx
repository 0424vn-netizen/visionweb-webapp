<%@ Page Title="Visa Acquirer Fraud Report" Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_VisaAcquirerFraudReport.aspx.cs" Inherits="rm_MCF_VisaAcquirerFraudReport" MasterPageFile="~/MasterPage.master" meta:resourcekey="PageResource1" %>


<%@ Register Src="~/UserControls/rm_MCF_AcquirerFraudReport.ascx" TagName="AcquirerFraudReport" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:AcquirerFraudReport ID="acquirerFraudReport" runat="server" CardType="Visa" />
</asp:Content>
