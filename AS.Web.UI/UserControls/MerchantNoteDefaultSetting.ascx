<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MerchantNoteDefaultSetting.ascx.cs" Inherits="UserControls_MerchantNoteDefaultSetting" %>

<div class="row">
    <div class="col-xs-12">
        <as:Literal ID="uxTitle" runat="server" meta:resourcekey="uxTitleResource1"></as:Literal>
    </div>
</div>
<div class="height-8"></div>
<div class="height-8"></div>
<div class="row">
    <div class="col-xs-2">
        <asp:Literal ID="lbSource" Text="Source:" runat="server" meta:resourcekey="lbSourceResource" />
    </div>
    <div class="col-xs-10">
        <div class="multichooser-wrapper">
            <as:MultiChooser IsSearchContains="true" Width="100%" CssClass="form-control" ID="uxSourceList" AutoPostBack="false" runat="server" Placeholder=" " meta:resourcekey="uxSourceResource1"></as:MultiChooser>
        </div>
    </div>
</div>
<div class="height-8"></div>
<div class="row">
    <div class="col-xs-12 form-action-container text-right">
        <as:Button class="btn btn-default" ID="uxSubmit" Text="Submit" OnClientClick="return onClientSubmit();" OnClick="uxSubmit_Click" runat="server" meta:resourcekey="bntSubmitResource" />
        <as:Button class="btn btn-default" ID="uxCancel" Text="Cancel" OnClientClick="ClosePopupModal();" runat="server" meta:resourcekey="bntCancelResource" />
    </div>
</div>
<div class="height-24"></div>
<as:RadCodeBlock ID="radCodeBlock2" runat="server">
    <script type="text/javascript">
        function onClientSubmit() {
            parent.isSubmitAddDefaultSetting = true;
            return true;
        }
    </script>
</as:RadCodeBlock>
