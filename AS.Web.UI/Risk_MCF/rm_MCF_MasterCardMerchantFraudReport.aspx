<%@ Page Title="MasterCard Merchant Fraud Report" Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MasterCardMerchantFraudReport.aspx.cs" Inherits="rm_MCF_MasterCardMerchantFraudReport" MasterPageFile="~/MasterPage.master" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_MerchantFraudReport.ascx" TagName="MerchantFraudReport" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:MerchantFraudReport ID="merchantFraudReport" runat="server" CardType="MasterCard" />
</asp:Content>
