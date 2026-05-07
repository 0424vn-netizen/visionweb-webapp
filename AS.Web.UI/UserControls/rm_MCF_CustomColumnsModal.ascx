<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_CustomColumnsModal.ascx.cs" Inherits="UserControls_rm_MCF_CustomColumnsModal" %>

<%--<div class="line-height-sm">
            <as:Literal ID="lbNoteChoose" runat="server" Text="Choose up to 14 columns" meta:resourcekey="lbNoteChooseResource"></as:Literal>
        </div>--%>

<div class="row customizeModal">
    <div class="col-xs-6">
        <h4 class="title-auto-queue on-top">
            <asp:Label ID="lbtNameCustomView" meta:resourcekey="lbtNameCustomViewResource" runat="server" Text="Name Your Custom View:"></asp:Label>
        </h4>
        <as:TextBox ID="txtNameCustomView" CssClass="form-control" runat="server" MaxLength="100" />
        <div class="bottom-error">
            <as:ValidatorMessage runat="server" ID="txtNameErrMsg" ApplyFor="txtNameCustomView" />
        </div>
        <as:HiddenField ID="txtOldNameCustomView" runat="server" />
    </div>

    <div class="col-xs-6">
        <asp:Panel ID="uxViewTypePanel" runat="server">
            <h4 id="H1" class="title-auto-queue on-top">
                <asp:Label runat="server" ID="lblViewType" Text="View Type" meta:resourcekey="txtViewType"></asp:Label></h4>
            <as:RadioButton ID="rdPublicView" runat="server" Text="Public View" GroupName="ViewType" CssClass="control-inline" meta:resourcekey="rdPublicViewResource"
                onkeypress="javascript:return false;" Value="" />
            <as:RadioButton ID="rdPrivateView" runat="server" Checked="true" Text="Private View" GroupName="ViewType" CssClass="control-inline" meta:resourcekey="rdPrivateViewResource"
                onkeypress="javascript:return false;" Value="" />
        </asp:Panel>
    </div>
</div>

<div class="row customizeModal">
    <div class="col-xs-6">
        <h4 class="title-auto-queue mt-5x">
            <as:Literal ID="lbUnused" runat="server" meta:resourcekey="lbUnusedResource"></as:Literal>
        </h4>
        <as:RadListBox runat="server" ID="uxLeftGrid" TransferToID="uxRightGrid" TransferMode="Move" RenderMode="Lightweight" Width="100%" Height="291"
            ButtonSettings-AreaWidth="35px" OnClientLoad="CustomColumnModal.uxGridListBox_OnClientLoad" CssClass="ux-two-grid" OnItemDataBound="uxLeftGrid_ItemDataBound"
            DataTextField="ColumnText" DataValueField="Key" AutoPostBackOnTransfer="true">
            <ClientItemTemplate>
                <div class="display-flex space-between unused-item" >
                    <span class="col-name col-name-style" id="#= Value#" data-tooltip="#= Attributes.IdTooltip#" onmouseout="hoverUnused(this)">#= Text#</span>
                    <span class="MoveToRight item-right-ico" onclick="CustomColumnModal.MoveToRight(this);"><a class="add-col-item"></a></span>
                </div>
            </ClientItemTemplate>
        </as:RadListBox>
    </div>
    <div class="col-xs-6">
        <h4 class="title-auto-queue mt-5x">
            <as:Literal ID="lbColumnDisplayed" runat="server" meta:resourcekey="lbColumnDisplayedResource"></as:Literal>
        </h4>
        <div>
            <as:RadListBox runat="server" ID="uxRightGrid" Width="100%" Height="291" CssClass="uxRightGrid-background" TransferMode="Move"
                OnClientLoad="CustomColumnModal.uxGridListBox_OnClientLoad" AllowAutomaticUpdates="true"
                EnableDragAndDrop="true" OnClientDropping="CustomColumnModal.uxGridListBox_OnClientDropping" OnClientDragging="CustomColumnModal.uxGridListBox_OnClientDragging" OnClientDragStart="CustomColumnModal.uxGridListBox_OnClientDragStart"
                OnClientReordered="CustomColumnModal.uxGridListBox_OnClientDropped" PersistClientChanges="true"
                AllowReorder="true" ButtonSettings-ShowReorder="false" DataTextField="ColumnText" DataValueField="Key" OnItemDataBound="uxRightGrid_ItemDataBound">
                <ClientItemTemplate>
                <div class="#= Attributes.IsDefault# display-flex space-between">              
                    <span class="item-left-drop-ico DroppedItem"><a class="drop-col-item"></a></span>
                    <span class="item-left-ico"><a class="DragItem drag-col-item" style="max-width:20px"></a></span>
                    <span class="col-name col-name-style" id="#= Value#" data-tooltip="#= Attributes.IdTooltip#" onmouseout="hoverUnused(this)">#= Text#</span>
                    <span class="item-right-ico" onclick="CustomColumnModal.MoveToLeft(this);"><a class="MoveToLeft remove-col-item"></a></span>
                    <span class="item-not-customizable">#= Attributes.IsNotCustomizable#</span>
                   </div>
                </ClientItemTemplate>
            </as:RadListBox>
        </div>
    </div>
</div>

<div class="row">
    <div class="col-xs-6 display-flex space-between mt-2x">
        <asp:Label ID="Label1" runat="server" Text="" meta:resourcekey="lbNoteChooseResource"></asp:Label>
        <div>
            <asp:Label ID="ucCountColumnSelected" runat="server" Text=""></asp:Label>
            <asp:Label ID="ucTextColumnSelected" runat="server" Text="" meta:resourcekey="lblSelectedResource"></asp:Label>
        </div>
    </div>
    <div class="col-xs-6 text-right form-action-container">
        <asp:Button ID="uxSubmit" meta:resourcekey="uxSubmitResource" runat="server" class="btn btn-default" OnClientClick="if(!CustomColumnModal.ValidateDisplayedColumn()){return false;}" OnClick="uxSubmit_Click"
            Text="Submit" />
        <asp:Button ID="uxCancel" meta:resourcekey="uxCancelResource" OnClientClick="parent.ClosePopupModal(2);" class="btn btn-default" runat="server" Text="Cancel"
            CausesValidation="false" UseSubmitBehavior="false"></asp:Button>
        <asp:Button ID="uxSaveEdit" runat="server" class="hidden btn btn-default" OnClick="uxSaveEdit_Click" />
    </div>
</div>

<as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
    <Items>
        <as:BasicValidationItem ControlToValidateID="txtNameCustomView" Rule="Required" ResMessage="Resources.UserMaintenanceMessage.Required" />
        <as:RegExValidationItem ControlToValidateID="txtNameCustomView" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" meta:resourcekey="ValidationMessages_V1" />
        <as:CustomValidationItem ControlToValidateID="txtNameCustomView" ClientValidationFunction="CheckFullView" meta:resourcekey="ValidationMessages_DuplicateFullView" />
    </Items>
</as:Validator>
<tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
    <script type="text/javascript">
        var full_View_Text = '<%= FULL_VIEW %>';
        var uxLeftGridListBox = '<%= uxLeftGrid.ClientID%>',
            uxRightGridListBox = '<%= uxRightGrid.ClientID%>';
        var ucCountColumnSelected = '<%= ucCountColumnSelected.ClientID%>';
        var js_TextComfirmCancel = '<%= GetLocalResourceObject("js_TextComfirmCancel").ToString() %>';
        var js_TextShowMaxColumn = '<%= GetLocalResourceObject("js_TextShowMaxColumn").ToString() %>';
        var js_TextShowMinColumn = '<%= GetLocalResourceObject("js_TextShowMinColumn").ToString() %>';
        var js_TextShowNoteMaxColumn = '<%= GetLocalResourceObject("js_TextShowNoteMaxColumn").ToString() %>';
        var js_Message_DuplicateFullView = '<%= GetLocalResourceObject("ValidationMessages_DuplicateFullView.Message").ToString() %>';
        var txtNameView_Client_ID = '<%= txtNameCustomView.ClientID %>';
        var js_button_uxSaveEdit = '<%= uxSaveEdit.ClientID%>';
        var rm_MCF_CustomColumnsModal_PageMode = '<%= PageMode%>';
    </script>

    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_CustomColumnsModal.js"></script>
</tek:RadCodeBlock>
