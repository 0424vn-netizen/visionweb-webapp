<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="SensitiveDataDetectedModal.aspx.cs" Inherits="SensitiveDataDetectedModal"
    meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container">
        <asp:Label runat="server" meta:resourcekey="lblDescription"></asp:Label>
        <div id="content"></div>
        <table class="ASTable form-inline table-checkbox" id="uiTable">
            <tbody>
                <tr class="nobackground" id="header">
                    <th></th>
                </tr>
            </tbody>
        </table>
        <div class="row">
            <div class="col-md-12 form-action-container text-right">
                <as:Button class="btn btn-default" ID="btnSubmit" disabled="disabled" Text="Submit" OnClientClick="submitNote(); return false;" runat="server" meta:resourcekey="btnSubmit" />
                <as:Button class="btn btn-default" ID="btnDisregard" Text="Disregard" OnClientClick="submitNote(true); return false;;" runat="server" meta:resourcekey="btnDisregard" />
            </div>
        </div>
        <asp:HiddenField ID="hdCardSelected" runat="server" />

        <as:ASRadCodeBlock ID="radCodeBlock" runat="server">
            <script type="text/javascript">
                var cardKeyList = "";
                var hdCardSelected = '<%=hdCardSelected.ClientID %>';
                var btnSubmit = '<%=btnSubmit.ClientID %>';
            </script>
            <script src="<%= ResolveUrl("~/")%>res/js/SensitiveDataDetectedModal.js"></script>
        </as:ASRadCodeBlock>
    </as:ASModalContainer>
</asp:Content>

