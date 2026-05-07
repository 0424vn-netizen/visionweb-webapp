<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MerchantNoteToEdit.ascx.cs" Inherits="UserControls_rm_MCF_MerchantNoteToEdit" %>

<tek:RadAjaxManagerProxy runat="server" ID="RadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxSubmit">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPanelEditComment" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
<div class="row" id="editnote" runat="server">
    <div class="col-md-12">
        <h2 class="grid-title edit-note-title" data-toggle="collapse" data-target="#ciEditNote">
            <as:Literal ID="Literal2" runat="server" Text="Add Note" meta:resourcekey="uxEditNoteTitleResource1"></as:Literal>
        </h2>
        <div class="box-edit-note in" id="ciEditNote">
            <div class="box-editor">
                <span class="text-node-editor">
                    <as:Literal ID="ltDocument1" runat="server" Visible="false" Text="Enter new comment (Limit 7000 characters per comment)" meta:resourcekey="ltDocument1Resource1"></as:Literal></span>
                <asp:Panel runat="server" ID="uxPanelEditComment">
                    <tek:RadEditor ID="uxComment" runat="server" EditModes="Design" ContentFilters="ConvertCharactersToEntities, ConvertToXhtml, FixEnClosingP"
                        StripFormattingOptions="MSWordRemoveAll" OnClientLoad="OnClientLoad"
                        ToolsFile="~/App_Data/RadEditorConfig.xml" Height="200px" Width="100%" Font-Names="Arial" OnClientPasteHtml="onHtmlPaste">
                        <CssFiles>
                            <tek:EditorCssFile Value="~/res/css/Editor.css" />
                        </CssFiles>
                    </tek:RadEditor>
                </asp:Panel>
                <div class="count-character-comment">
                    <span id="textRemaining"></span>
                </div>
                <div class="box-footer-editor">
                    <div class="bottom-error">
                        <span id="ciCommentsMsg" class="error display-none"></span>
                    </div>
                </div>
                <div class="ml-auto text-right">
                    <asp:Button ID="uxSubmit" runat="server" Text="Submit" OnClick="UxSubmit_Click" OnClientClick="return Validate();" class="btn btn-default btn-risk-note-submit" meta:resourcekey="uxSubmitResource1" />
                    <as:Button ID="btnCancel" runat="server" CssClass="btn btn-default btn-risk-note-cancel" OnClientClick="DoClose();" Text="Cancel" meta:resourcekey="uxCancelResource1" />
                     <asp:HiddenField ID="hdCardDetected" runat="server" />
                </div>
            </div>
        </div>
    </div>
</div>

<tek:RadCodeBlock ID="RadCodeBlock2" runat="server">
    <link href="../res/css/riskmerchantnote.css" rel="stylesheet" />
    <script>
        var EditNote_hdCardDetected = "<% =hdCardDetected.ClientID %>";
        var EditNote_uxCommentID = "<% =uxComment.ClientID %>";
        var EditNote_uxSubmitID = "<% =uxSubmit.ClientID %>";
        var EditNote_uxSubmitUniqueID = "<% =uxSubmit.UniqueID %>";        
        var EditNote_js_Characters ='<%= GetLocalResourceObject("EditNote_js_Characters") %>';
        var textAlert = '<%= GetLocalResourceObject("validateCommentLengthResource") %>';
        var EditNote_js_Required = '<%= GetLocalResourceObject("EditNote_js_Required") %>';
        var EditNote_js_msg1 = '<%= GetLocalResourceObject("EditNote_js_msg1") %>';
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/rm_MCF_MerchantNoteToEdit.js"></script>
</tek:RadCodeBlock>


