<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActivationReporDeleteFilterModal.aspx.cs" Inherits="ActivationReporDeleteFilterModal" 
    MasterPageFile="~/MasterPagePopup.master" Title="Warning" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxNotInMifDetailGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxNotInMifDetailGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-sm">
        <div class="row">
            <div class="col-xs-12">
                <as:Literal ID="ltDeleteFilterMsg" runat="server" Text="Are you sure you want to delete this filter?" meta:resourcekey="ltDeleteFilterMsgResources"></as:Literal>
            </div>
        </div>
        <div class="height-8"></div>
        <div class="row">
            <div class="col-xs-12 form-action-container text-right">
                <as:Button class="btn btn-default" ID="uxBtnDelete" Text="Delete" runat="server" OnClientClick="onDeleteFilter()" meta:resourcekey="uxBtnDeleteResource" />
                <as:Button class="btn btn-default" ID="Button1" Text="Close" OnClientClick="ClosePopupModal();" runat="server" meta:resourcekey="bntCloseResource" />
            </div>
        </div>
    </as:ASModalContainer>
    <script>
        function onDeleteFilter()
        {
            parent.IsDeleteFilter = true;
            ClosePopupModal();
        }
    </script>
</asp:Content>