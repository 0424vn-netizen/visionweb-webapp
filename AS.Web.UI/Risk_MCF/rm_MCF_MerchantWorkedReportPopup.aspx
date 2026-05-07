<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MerchantWorkedReportPopup.aspx.cs" MasterPageFile="~/MasterPagePopup.master" 
    Title="MERCHANT WORKED REPORT" meta:resourcekey="PageResource1" Inherits="rm_MCF_MerchantWorkedReportPopup" %>

<%@ Register Src="~/UserControls/rm_MCF_MerchantWorked.ascx" TagName="UxMerchantWorked" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl">
        <uc:UxMerchantWorked ID="UxMerchantWorked" Type="MerchantWorkedModal" runat="server"></uc:UxMerchantWorked>
    </as:ASModalContainer>
</asp:Content>
