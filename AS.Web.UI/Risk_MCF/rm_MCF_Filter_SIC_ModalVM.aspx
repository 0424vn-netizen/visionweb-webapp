<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_Filter_SIC_ModalVM.aspx.cs" Inherits="rm_MCF_Filter_SIC_ModalVM" Title="Selected SIC" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_FilterSICVM.ascx" TagName="Risk_FilterSICVM" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <uc:Risk_FilterSICVM ID="uxSICView" runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 action-container text-right">
                <as:Button runat="server" Text="Close" ID="uxBtnClose" OnClientClick="parent.ClosePopupModal(1); return false;" CssClass="btn btn-default" meta:resourcekey="uxBtnCloseResource1" />
            </div>
        </div>
    </as:ASModalContainer>
    <as:ASRadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            parent.setModalID('SICModal');
        </script>
    </as:ASRadCodeBlock>
</asp:Content>
