<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ExportOptionModal.aspx.cs" MasterPageFile="~/MasterPagePopup.master"
    Inherits="ExportOptionModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container" Width="">
        <h3 class="modal-title font-20">
            <asp:Label ID="lbExportOptions" runat="server" Text="Export Options" meta:resourcekey="lbExportOptionsResource"></asp:Label>
        </h3>
        <div class="row">
            <div class="col-md-12 text-left">
                <div class="box">
                    <div class="checkbox-list align-center-checkbox">
                        <label class="font-size-default">
                            <input id="btnSelectAll" type="checkbox" onclick="doSetSelectedAll(this.checked)" />
                            <asp:Label ID="lbSelectAll" runat="server" Text="Select All" meta:resourcekey="lbSelectAllResource" CssClass="default-font-family">
                            </asp:Label>
                        </label>
                    </div>
                    <div id="optionExport" class="checkbox-list align-center-checkbox">
                    </div>
                </div>
            </div>
        </div> 
        <div class="row">
            <div class="col-md-12 text-right form-action-container">
                <as:Button ID="btnExportMultiSections" runat="server" OnClientClick="return uxSubmit_Click(this)" Text="Submit" CssClass="btn btn-default" meta:resourcekey="uxSubmitResource" />
                <as:Button ID="btnCancel" runat="server" OnClientClick="return uxCancel_Click();" Text="Cancel" CssClass="btn btn-default" meta:resourcekey="uxCancelResource" />
            </div>
        </div>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/ExportOptions.js"></script>
        <script type="text/javascript">
            var msgValidation = '<%= GetLocalResourceObject("msgValidationNoChecked.Text").ToString() %>';
        </script>
    </as:ASModalContainer>
</asp:Content>
