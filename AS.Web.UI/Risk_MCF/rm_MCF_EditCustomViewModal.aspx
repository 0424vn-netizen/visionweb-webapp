<%@ Page Title="Reoder Full View Columns" Language="C#" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1"
    AutoEventWireup="true" CodeFile="rm_MCF_EditCustomViewModal.aspx.cs" Inherits="rm_MCF_EditCustomViewModal" %>

<asp:Content ContentPlaceHolderID="ContentPage" runat="server" ID="uxMainModalContent">
   <as:ASModalContainer ID="uxModalContainer" runat="server" ContainerCssClass="container" WidthCssClass="modal-md">
    <div class="row customizeModal ">
        <div class="col-xs-12">
            <div class="unused-title">
                <as:Literal ID="lbColumnDisplayed" runat="server" Text="Columns Displayed" meta:resourcekey="uxColumnDisplayed"></as:Literal>
            </div>
            <div>
                <as:RadListBox runat="server" ID="uxRightGrid" Width="100%" Height="291" CssClass="uxRightGrid-background" 
                    OnClientLoad="EditColumnModal.uxGridListBox_OnClientLoad" AllowAutomaticUpdates="true"
                    EnableDragAndDrop="true" OnClientDragging="EditColumnModal.uxGridListBox_OnClientDragging" OnClientDragStart="EditColumnModal.uxGridListBox_OnClientDragStart"
                    OnClientReordered="EditColumnModal.uxGridListBox_OnClientDropped" PersistClientChanges="true"
                    AllowReorder="true" ButtonSettings-ShowReorder="false" DataTextField="ColumnText" DataValueField="ColumnName">
                    <ClientItemTemplate>
                    <span class="item-left-drop-ico DroppedItem"><a class="drop-col-item"></a></span>
                    <span class="item-left-ico"><a class="DragItem drag-col-item"></a></span>
                    <span class="col-name col-name-style">#= Text#</span>
                    <span class="" onclick="EditColumnModal.RemoveItem(this);"><a class="RemoveItem"></a></span>
                    </ClientItemTemplate>
                </as:RadListBox>
            </div>
        </div>
    </div>
    <div class="row">
        <as:HiddenField ID="txtViewName" runat="server"/>
        <div class="col-xs-12 text-right form-action-container">
            <asp:Button ID="uxSubmit" meta:resourcekey="uxSubmitResource" class="btn btn-default" OnClientClick="EditColumnModal.ValidateDisplayedColumn();" runat="server" OnClick="uxSubmit_Click" Text="Submit"></asp:Button>
            <as:Button runat="server" meta:resourcekey="uxCancelResource" class="btn btn-default" ID="uxCancel" Text="Cancel" OnClientClick="parent.ClosePopupModal(2);" IsStandardButton="False" />
        </div>
    </div>
    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var uxRightGridListBox = '<%= uxRightGrid.ClientID%>';
            var ViewType = '<%= ViewType %>';
            js_TextComfirmCancel = "Are you sure that you want to cancel the changes?";
            js_TextShowMinColumn = "Please make sure that at least one column is in the Displayed Columns box.";
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_EditCustomViewModal.js"></script>
    </tek:RadCodeBlock>
    </as:ASModalContainer>
</asp:Content>
