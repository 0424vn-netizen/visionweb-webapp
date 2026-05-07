<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="DeleteAssignmentModal.aspx.cs" Inherits="DeleteAssignmentModal"
    Title="Validation" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm">
        <div class="row" id="uxMessages-container">
            <div class="col-xs-12">
                <p>
                    <as:Literal ID="ltlConfirm" runat="server" meta:resourcekey="ltlConfirmResource"></as:Literal>
                </p>
            </div>
            <div class="col-xs-12 form-action-container text-right">
                <as:Button runat="server" CssClass="btn btn-default" ID="uxOK" Text="OK" OnClientClick="return parent.deleteAssignmentConfirm();" meta:resourcekey="btnOk" />
                <as:Button runat="server" CssClass="btn btn-default" ID="uxCancel" Text="Cancel" OnClientClick="return parent.HidePopupModal();" meta:resourcekey="btnCancel" />
            </div>
        </div>
    </as:ASModalContainer>
</asp:Content>
