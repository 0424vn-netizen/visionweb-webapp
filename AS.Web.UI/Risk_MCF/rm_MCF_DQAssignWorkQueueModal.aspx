<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="rm_MCF_DQAssignWorkQueueModal.aspx.cs" Inherits="rm_MCF_DQAssignWorkQueueModal"
    Title="Re-queue Alerts" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <as:RadAjaxManagerProxy ID="RadAjaxManagerDQ" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxCbkRemoveRequeue">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxComboWorkQueueList" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnRequeue">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="btnRequeue" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" Width="550px" ContainerCssClass="container min-space" WidthCssClass="modal-xxl">
        <div class="row">
            <div class="col-xs-12">
                <label style="font-weight:bold;">
                    <as:Literal ID="uxMerchantCount" runat="server" meta:resourcekey="uxMerchantCountResource1" />
                </label>
               <asp:Literal ID="rm_DQAssignWorkQueueModal_aspx_Text1" runat="server" meta:resourcekey="rm_DQAssignWorkQueueModal_aspx_Text1Resource1"> merchant(s) selected</asp:Literal>
            </div>
        </div>
        <div class="height-8"></div>
        <div class="row">
            <div class="col-xs-3 " style="font-weight:bold;">
               <asp:Literal ID="rm_DQAssignWorkQueueModal_aspx_Text2" runat="server" meta:resourcekey="rm_DQAssignWorkQueueModal_aspx_Text2Resource1"> Re-queue To:</asp:Literal>
            </div>
            <div class="col-xs-6">
                <as:RadComboBox ID="uxComboWorkQueueList" runat="server" EnableEmbeddedSkins="false" Width="100%" meta:resourcekey="uxComboWorkQueueListResource1" />
            </div>
            <div class="col-xs-3 ml-m-6x">
                <as:Button ID="btnRequeue" runat="server" Text="Submit" CssClass="btn btn-default" OnClick="btnRequeue_Click" meta:resourcekey="btnRequeueResource1" />
            </div>
        </div>
        <div class="height-8"></div>
        <div class="row">
            <div class="col-xs-3">
                &nbsp;
            </div>
            <div class="col-xs-9">
                <div class="control-inline">
                <as:CheckBox ID="uxCbkRemoveRequeue" runat="server" Text="Remove from Re-queue" AutoPostBack="true"
                    OnCheckedChanged="uxCbkRemoveRequeue_CheckedChanged" meta:resourcekey="uxCbkRemoveRequeueResource1" />
                </div>
            </div>
        </div>
        <div class="height-8"></div>
    </as:ASModalContainer>

</asp:Content>
