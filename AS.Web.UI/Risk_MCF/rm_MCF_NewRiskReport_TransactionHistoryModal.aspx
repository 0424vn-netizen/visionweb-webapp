<%@ Page Title="Transaction History" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="rm_MCF_NewRiskReport_TransactionHistoryModal.aspx.cs" Inherits="rm_MCF_NewRiskReport_TransactionHistoryModal" meta:resourcekey="PageResource1" %>

<%@ Register TagName="TransactionHistory" Src="~/UserControls/rm_MCF_NewRiskReportTransactionHistory.ascx" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <div>
        <uc:TransactionHistory ID="uxTransactionHistory" runat="server" isOntop="true"></uc:TransactionHistory>
    </div>
    <as:RadCodeBlock ID="uxRadCode" runat="server">
        <script type="text/javascript">
            var rm_TransactionHistoryModal_IsIEBrowser = "<%=IsIEBrowser.ToString()%>";
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_TransactionHistoryModal.js"></script>
    </as:RadCodeBlock>
</asp:Content>
