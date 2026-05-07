<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_DetectionManageCustomViewsModal.aspx.cs" Inherits="rm_MCF_DetectionManageCustomViewsModal" %>

<%@ Register Src="~/UserControls/rm_MCF_DetectionCustomizeColumnsModal.ascx" TagPrefix="uc" TagName="CustomizeColumnsModal" %>

<asp:Content ContentPlaceHolderID="ContentPage" runat="Server" ID="uxMainModalContent">
    <tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="rbDisplayType">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCustomViewsGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnButton">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCustomViewsGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <as:ASModalContainer ID="uxModalContainer" runat="server" ContainerCssClass="container" WidthCssClass="modal-lg">
        <uc:CustomizeColumnsModal runat="server" ID="uxCustomizeColumnsModal" />
    </as:ASModalContainer>
    <tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
    </tek:RadCodeBlock>
</asp:Content>

