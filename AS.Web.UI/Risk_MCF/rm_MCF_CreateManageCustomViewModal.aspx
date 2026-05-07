<%@ Page Title="Create Custom View" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_CreateManageCustomViewModal.aspx.cs" Inherits="rm_MCF_CreateManageCustomViewModal" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_CustomColumnsModal.ascx" TagPrefix="uc" TagName="CustomColumnsModal" %>

<asp:Content ContentPlaceHolderID="ContentPage" runat="Server" ID="uxMainModalContent">
    <as:ASModalContainer ID="uxModalContainer" runat="server" ContainerCssClass="container" WidthCssClass="modal-lg">
        <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManagerProxy">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxCustomColumnsModal">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxCustomColumnsModal" LoadingPanelID="uxLoadingPanelCustom" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>

        </tek:RadAjaxManagerProxy>
        <uc:CustomColumnsModal runat="server" ID="uxCustomColumnsModal" />
    </as:ASModalContainer>
</asp:Content>
