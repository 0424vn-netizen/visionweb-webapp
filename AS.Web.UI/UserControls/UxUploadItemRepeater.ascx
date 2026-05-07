<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UxUploadItemRepeater.ascx.cs" Inherits="UserControls_UxUploadItemRepeater" %>

<div class="borderStyle" id="uxItemUpload" runat="server">
    <div class="item" data-upload="<%=IdFileClient %>">
        <div class="file-info">
            <div class="flex-box">
                <div>
                    <img src="<%= FileType%>" />
                </div>
                <div class="item-upload">
                    <div class="ellipsis" style="max-width: 600px">
                        <asp:Label runat="server" ID="lblText"></asp:Label>
                    </div>
                    <div class="uploading" id="uploading" runat="server">
                        <as:Literal ID="ltUploading" runat="server" meta:resourcekey="ltUploading"></as:Literal>
                    </div>
                    <div class="fileSize" id="fileSize" runat="server">
                        <span class="filename"><%= FormatFize() %></span>
                        <span>
                            <img src="/res/images/icon-download-done.png" /></span>
                    </div>
                    <div class="fileIssue" id="fileIssue" runat="server">
                        <asp:Label runat="server" ID="lblIssueFile"></asp:Label>
                    </div>
                </div>
                <div style="min-width: 20px; height: 20px; position: relative; margin: auto;">
                    <asp:Button runat="server" ID="btnDelete" Text="" OnClick="btnDelete_OnClick" CssClass="hidden" />
                    <a class="icon-delete" onclick="deleteFileClick('<%#btnDelete.ClientID %>');"></a>
                    <asp:HiddenField runat="server" ID="hdfId" />
                </div>
            </div>
        </div>


        <div style="min-width: 300px; position: relative; margin: 5px 0; margin-left: 45px; margin-right: 12px;">
            <as:RadTextBox Width="100%" EmptyMessage="Description" onkeypress="if(event.keyCode==13){return false;}" OnPreRender="txtNote_PreRender"
                 ClientEvents-OnBlur="inputDescription" data-parent="<%#uxItemUpload.ClientID%>"
                EmptyMessageStyle-CssClass="text-emptymessage-attachment" CssClass="text-note-attachment" runat="server"
                MaxLength="500" ID="txtNote"
                autocomplete="off" EmptyMessageStyle-Font-Italic="true" TextMode="SingleLine" Rows="1" Height="26px">
            </as:RadTextBox>
            <label class="file-discription error" data-message="<%#txtNote.ClientID %>">
                <as:Literal ID="Literal2" runat="server" meta:resourcekey="msg_NotSpecialCharacters"></as:Literal>
            </label>
        </div>

    </div>
    <div class="progress" id="progressBar" runat="server" style="background-color: #E7E7E7; position: relative;">
        <div class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100" style="width: 0%; background-color: #00A8FB; height: 2px; position: absolute; left: 0; bottom: 0px;"></div>
    </div>
</div>

<as:Validator runat="server" MessageType="Summary" MessageContainerClientID="uxValidSummary" ID="uxValidator">
    <Items>
        <as:BasicValidationItem ControlToValidateID="txtNote" Rule="Maxlength" MaxLength="500" Message="<%$ Resources:msgMaxLength %>" />
    </Items>
</as:Validator>
