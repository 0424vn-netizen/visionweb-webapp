<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="ConfirmUserGroupModal.aspx.cs" Inherits="ConfirmUserGroupModal"
    Title="Validation" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md">
        <div class="row" id="uxMessages-container">
            <div class="col-xs-12">
                <p>
                    <as:Literal ID="ltlConfirm" runat="server" meta:resourcekey="ltlConfirmResource"></as:Literal>
                </p>
                <div id="lstDocument" runat="server"></div>
                <div id="lstDocumentFull" class="hide" runat="server"></div>
            </div>
            <div class="col-xs-12 form-action-container text-right">
                <as:Button runat="server" CssClass="btn btn-default" ID="uxOK" OnClientClick="return parent.ClosePopupModal(1);" meta:resourcekey="btnOk" />
            </div>
        </div>
    </as:ASModalContainer>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock2">
        <script type="text/javascript">
            var lstDocument = '<%= lstDocument.ClientID %>';
            var lstDocumentFull = '<%= lstDocumentFull.ClientID %>';
            function loadMore() {
                $('#' + lstDocument).addClass("hide");
                $('#' + lstDocumentFull).removeClass("hide");
                AdjustModalSize();
            }
        </script>
    </tek:RadCodeBlock>
</asp:Content>
