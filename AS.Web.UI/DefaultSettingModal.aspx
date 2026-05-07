<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefaultSettingModal.aspx.cs" Inherits="DefaultSettingModal" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/UserControls/MerchantNoteDefaultSetting.ascx" TagName="ucMerchantNoteDefaultSetting" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/CaseHistoryDefaultSetting.ascx" TagName="ucCaseHistoryDefaultSetting" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" ContainerCssClass="container" WidthCssClass="modal-md">
        <uc:ucMerchantNoteDefaultSetting runat="server" ID="ucMerchantNoteDefaultSetting" />
        <uc:ucCaseHistoryDefaultSetting runat="server" ID="ucCaseHistoryDefaultSetting" />
    </as:ASModalContainer>
</asp:Content>

