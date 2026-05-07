<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MerchantWorkedReport.aspx.cs"  meta:resourcekey="PageResource1"
    MasterPageFile="~/MasterPage.master" Inherits="rm_MCF_MerchantWorkedReport" Title="MERCHANT WORKED REPORT" %>

<%@ Register Src="~/UserControls/rm_MCF_MerchantWorked.ascx" TagName="UxMerchantWorked" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:UxMerchantWorked ID="UxMerchantWorked" Type="MerchantWorkedPage" runat="server"></uc:UxMerchantWorked>
</asp:Content>
