<%@ Page Title="Create Custom View" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_CustomizeColumnsModal.aspx.cs" Inherits="rm_MCF_CustomizeColumnsModal" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/UserControls/rm_MCF_CustomizeColumnsModal.ascx" TagPrefix="uc" TagName="CustomizeColumnsModal" %>

<asp:Content ContentPlaceHolderID="ContentPage" runat="Server" ID="uxMainModalContent">    
    <as:ASModalContainer ID="uxModalContainer" runat="server" ContainerCssClass="container" WidthCssClass="modal-lg">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManagerProxy">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxCustomizeColumnsModal">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxCustomizeColumnsModal" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>

</tek:RadAjaxManagerProxy>
            <uc:CustomizeColumnsModal runat="server" ID="uxCustomizeColumnsModal" />
    </as:ASModalContainer>
    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
    <script type="text/javascript">
    </script>
</tek:RadCodeBlock>
</asp:Content>
