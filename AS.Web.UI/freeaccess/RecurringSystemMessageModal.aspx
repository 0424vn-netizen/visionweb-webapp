<%@ Page Title="Message" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="RecurringSystemMessageModal.aspx.cs" Inherits="freeaccess_RecurringSystemMessageModal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <style type="text/css">
        .container {
            border: none;
        }
    </style>
    <as:Panel runat="server" ID="uxPanel">
        <div class="row">
            <div class="col-md-12">
                <as:Literal ID="text" runat="server" meta:resourcekey="txtMessage"></as:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button ID="uxButton1" CssClass="btn btn-default" runat="server" Text="Close" OnClientClick="self.close()" meta:resourcekey="uxButton1Resource1" />
            </div>
        </div>
    </as:Panel>
</asp:Content>

