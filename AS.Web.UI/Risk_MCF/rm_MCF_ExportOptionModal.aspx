<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_ExportOptionModal.aspx.cs" MasterPageFile="~/MasterPagePopup.master"
    Inherits="rm_MCF_ExportOptionModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-sm" ContainerCssClass="container" Width="">
        <h2 class="modal-title">
            <asp:Label ID="lbExportOptions" runat="server" Text="Export Options" meta:resourcekey="lbExportOptionsResource"></asp:Label>
        </h2>
        <div class="row">
            <div class="col-md-12 text-left">
                <div class="radio">
                    <label>
                        <input type="radio" id="radAll" name="exportOption" checked="true"  value="allColumns" />
                        <%= GetLocalResourceObject("exportAllFields") %>
                    </label>
                </div>
                <div class="radio  no-margin-bottom">
                    <label>
                        <input type="radio"  id="radDisplay" name="exportOption"  value="displayColumns" />
                        <%= GetLocalResourceObject("exportDislayFields") %>
                    </label>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-right form-action-container">
                <as:Button ID="btnExportMultiSections" runat="server" OnClientClick="return uxSubmit_Click()" Text="Submit" CssClass="btn btn-default" meta:resourcekey="uxSubmitResource" />
                <as:Button ID="btnCancel" runat="server" OnClientClick="return parent.HidePopupModal();" Text="Cancel" CssClass="btn btn-default" meta:resourcekey="uxCancelResource" />
            </div>
        </div>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_ExportOptions.js"></script>
    </as:ASModalContainer>
</asp:Content>
