<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CaseHistoryDefaultSetting.ascx.cs" Inherits="UserControls_CaseHistoryDefaultSetting" %>

<as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
    <AjaxSettings>        
        <tek:AjaxSetting AjaxControlID="uxTypes">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxStatuses" />
                <tek:AjaxUpdatedControl ControlID="uxPriorityLevel" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<div class="row">
    <div class="col-xs-12">
        <span class="control-label">
            <as:Literal ID="uxTitle" runat="server" meta:resourcekey="uxTitleResource1"></as:Literal></span>
    </div>
</div>
<div class="height-8"></div>
<div class="height-8"></div>
<div class="row">
    <div class="col-xs-2">
        <as:Literal ID="Literal4" runat="server" Text="Type:" meta:resourcekey="uxCaseHistoryTypeFilter"></as:Literal>
    </div>
    <div class="col-xs-10">
        <div class="multichooser-wrapper">
            <as:MultiChooser IsSearchContains="true" IsInTelerikAjax="true" OnTextChanged="ChangeStatusAndPriorities" Width="100%" CssClass="form-control" ID="uxTypes" AutoPostBack="true" runat="server" Placeholder=" " meta:resourcekey="uxSourceResource1"></as:MultiChooser>
        </div>
    </div>
</div>
<div class="height-8"></div>
<div class="row">
    <div class="col-xs-2">
        <as:Literal ID="Literal2" runat="server" Text="Status:" meta:resourcekey="uxCaseHistoryStatusFilter"></as:Literal>
    </div>
    <div class="col-xs-10">
        <div class="multichooser-wrapper">
            <as:MultiChooser IsInTelerikAjax="true" IsSearchContains="true" Width="100%" CssClass="form-control" ID="uxStatuses" AutoPostBack="false" runat="server" Placeholder=" " meta:resourcekey="uxSourceResource1"></as:MultiChooser>
        </div>
    </div>
</div>
<div class="height-8"></div>
<div class="row">
    <div class="col-xs-2">
        <as:Literal ID="Literal1" runat="server" Text="Priority:" meta:resourcekey="uxCaseHistoryPriorityFilter"></as:Literal>
    </div>
    <div class="col-xs-10">
        <div class="multichooser-wrapper">
            <as:MultiChooser IsInTelerikAjax="true" IsSearchContains="true" Width="100%" CssClass="form-control" ID="uxPriorityLevel" AutoPostBack="false" runat="server" Placeholder=" " meta:resourcekey="uxSourceResource1"></as:MultiChooser>
        </div>
    </div>
</div>
<div class="height-8"></div>
<div class="row">
    <div class="col-12 text-right form-action-container">
        <asp:Button ID="uxSubmit" OnClick="uxSubmit_Click" runat="server" OnClientClick="return onCHClientSubmit();" Text="Submit" CssClass="btn btn-default" meta:resourcekey="bntSubmitResource" />
        <asp:Button ID="uxCancel" meta:resourcekey="bntCancelResource" OnClientClick=" parent.HidePopupModal();" class="btn btn-default" runat="server" Text="Cancel"
            CausesValidation="false" UseSubmitBehavior="false"></asp:Button>
    </div>
</div>
<div class="height-150"></div>
<div class="height-20"></div>
<as:RadCodeBlock ID="radCodeBlock2" runat="server">
    <script type="text/javascript">
        function onCHClientSubmit() {
            parent.isCMSubmitAddDefaultSetting = true;
            return true;
        }
    </script>
</as:RadCodeBlock>
