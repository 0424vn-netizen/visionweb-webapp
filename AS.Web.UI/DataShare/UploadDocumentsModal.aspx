<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadDocumentsModal.aspx.cs" Inherits="UploadDocumentsModal"
    MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxUploadManagement.ascx" TagName="UploadDocument" TagPrefix="uc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="new-datashare-model" id="newdatasharemodal">
        <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-xl">
            <h3 class="report-title-no-filter" style="margin: 0">
                <as:Literal ID="Literal3" runat="server" meta:resourcekey="ltUploadDocumentResources"></as:Literal>
            </h3>
            <div class="height-15"></div>
            <table class="ASTable form-inline" id="table1">
                <as:PlaceHolder runat="server" ID="phDocumentName" Visible="false">
                    <tr class="AltRow">
                        <td class="heading valign-middle w-20">
                            <as:Literal ID="Literal1" runat="server" Text="Document Name" meta:resourcekey="ltDocumentNameResources"></as:Literal>
                        </td>
                        <td>
                            <as:RadTextBox onkeypress="if(event.keyCode==13){return false;}" Width="100%" ID="uxDocumentName" MaxLength="500" runat="server" CssClass=""></as:RadTextBox>
                        </td>
                    </tr>
                </as:PlaceHolder>
                <tr class="Row">
                    <td class="heading valign-middle w-20">
                        <as:Literal ID="ltShareUsers" runat="server" Text="Share With Users" meta:resourcekey="ltShareUsersResources"></as:Literal>
                    </td>
                    <td>
                        <as:MultiChooser runat="server" ID="uxShareUsers" onChange="AdjustModalSize();" Width="100%" Placeholder="Select a user"></as:MultiChooser>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-middle w-20">
                        <as:Literal ID="ltShareGroups" runat="server" Text="Share With Groups" meta:resourcekey="ltShareGroupsResources"></as:Literal>
                    </td>
                    <td>
                        <as:MultiChooser runat="server" onChange="AdjustModalSize();" ID="uxShareGroups" Width="100%" Placeholder="Select a group"></as:MultiChooser>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-middle w-20">
                        <as:Literal ID="ltExpirationDate" runat="server" Text="Expiration Date" meta:resourcekey="ltExpirationDateResources"></as:Literal>
                    </td>
                    <td>
                        <tek:RadDatePicker Width="251px" EnableTyping="false" Height="24px" ID="uxExpirationDate" runat="server" meta:resourcekey="uxDateResource1"></tek:RadDatePicker>
                    </td>
                </tr>
            </table>

            <as:PlaceHolder runat="server" ID="phUploadFile">
                <div class="height-15"></div>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="alert-file">
                            <as:Literal ID="msgLt" runat="server" meta:resourcekey="ltInfoFile"></as:Literal>
                        </div>
                        <uc:UploadDocument runat="server" ID="uxUpload" />
                    </div>
                </div>
            </as:PlaceHolder>
            <as:PlaceHolder runat="server" ID="phFileInfo" Visible="false">
                <div class="height-15"></div>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="item flex-box attach-file-info">
                            <div class="mr-10">
                                <img runat="server" id="uxFileType" src="../res/images/IconsFile/doc.svg" />
                            </div>
                            <div style="width: 100%">
                                <div class="ellipsis" style="max-width: 600px" id="uxTitleFileName" runat="server">
                                    <asp:Label runat="server" ID="uxFileName"></asp:Label>
                                </div>
                                <div class="fileSize" id="uxFileSize" runat="server">
                                </div>
                                <div class="fileIssue" id="fileIssue" runat="server">
                                    <as:RadTextBox Width="100%" EmptyMessage="Description" onkeypress="if(event.keyCode==13){return false;}"
                                        ClientEvents-OnBlur="inputDescription"
                                        EmptyMessageStyle-CssClass="text-emptymessage-attachment" CssClass="text-note-attachment" runat="server"
                                        MaxLength="500" ID="uxDescription" OnPreRender="uxDescription_PreRender"
                                        autocomplete="off" EmptyMessageStyle-Font-Italic="true" TextMode="SingleLine" Rows="1" Height="26px">
                                    </as:RadTextBox>

                                    <label class="file-discription error" data-message="descriptionSpecialCharacter">
                                        <as:Literal ID="Literal2" runat="server" meta:resourcekey="msg_NotSpecialCharacters"></as:Literal>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="height-10"></div>
            </as:PlaceHolder>
            <div class="row">
                <div class="col-xs-12 form-action-container text-right">
                    <as:Button class="btn btn-default" ID="btnSubmit" Text="Submit" OnClientClick="return validateSubmit()" runat="server" meta:resourcekey="btnSubmitResource" />
                    <as:Button class="btn btn-default" ID="btnCancel" Text="Cancel" OnClientClick="ClosePopupModal(); return false;" runat="server" meta:resourcekey="btnCancelResource" />
                </div>
            </div>
            <div style="display: none;">
                <div id="uxValidSummary">
                </div>
            </div>
            <as:Validator ID="uxValidator" runat="server" MessageType="Summary" MessageContainerClientID="uxValidSummary" ValidationFunction="validaInput" IgnoreServerValidation="False" meta:resourcekey="uxValidatorResource1">
                <Items>
                    <as:CustomValidationItem ControlToValidateID="uxShareUsers" IsInAjaxPanel="true" ClientValidationFunction="CheckRequireShareUserorGroup" Message="<%$ Resources:msg_RequireShareUserorGroupResources %>" />
                </Items>
            </as:Validator>
            <as:Button class="btn btn-default hide" ID="btnConfirmSubmit" OnClick="btnSubmit_Click" Text="Submit" runat="server" meta:resourcekey="btnSubmitResource" />
            <tek:RadCodeBlock runat="server">
                <script>
                    var uxSubmitID = "<%=btnSubmit.ClientID%>";
                    var uxShareWithUsersID = '<%= uxShareUsers.ClientID%>';
                    var uxShareWithGroupsID = '<%= uxShareGroups.ClientID%>';
                    var msg_RequireDocument = "<%= GetLocalResourceObject("msg_RequireDocument") %>";
                    var msg_Invalid_File_Type = "<%= GetGlobalResourceObject("ValMsg", "Invalid_File_Type") %>";
                    var msg_File_Size_Over_50MB = "<%= GetGlobalResourceObject("ValMsg", "File_Size_Over") %>";
                    var msg_Invalid_File_Type_Multi = "<%= GetGlobalResourceObject("ValMsg", "Invalid_File_Type_Multi") %>";
                    var msg_File_Size_Over_50MB_Multi = "<%= GetGlobalResourceObject("ValMsg", "File_Size_Over_Multi") %>";
                    var isUpdateMode = '<%= IsUpdateMode%>';
                    var btnConfirmSubmit = "<%=btnConfirmSubmit.ClientID%>";
                </script>
                <script src="<%= ResolveUrl("~/")%>res/js/dataShare/uploadDocumentsModal.js"></script>
            </tek:RadCodeBlock>
        </as:ASModalContainer>
    </div>
</asp:Content>
