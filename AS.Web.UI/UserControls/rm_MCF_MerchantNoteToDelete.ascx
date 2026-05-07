<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MerchantNoteToDelete.ascx.cs" Inherits="UserControls_rm_MCF_MerchantNoteToDelete" %>
<div class="height-8"></div>
<div class="height-8"></div>
<asp:PlaceHolder ID="PlaceHolder2" runat="server">
    <div class="row">
        <div class="col-md-12">
            <as:Literal ID="Literal1" runat="server" Text="Are you sure you want to delete this note?" meta:resourcekey="lstMsgDeleteNoteResource1"></as:Literal>
        </div>
    </div>
</asp:PlaceHolder>
<div class="height-8"></div>
<div class="row">
    <div class="col-xs-12 form-action-container text-right">
        <as:Button class="btn btn-default" runat="server" ID="btnSubmitMerchantNoteToDelete"  Text="DELETE" OnClick="uxDelete_Click"  meta:resourcekey="uxDeleteResource1"/>
        <as:Button class="btn btn-default" runat="server" ID="btnCancelMerchantNoteToDelete" Text="CANCEL" OnClientClick="return parent.HidePopupModal();" meta:resourcekey="uxCancelResource" />
    </div>
</div>
<div class="height-24"></div>