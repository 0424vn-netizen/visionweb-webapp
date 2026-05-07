<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UxUploadManagement.ascx.cs" Inherits="UserControls_UxUploadManagement" %>
<%@ Register Src="~/UserControls/uxUploadItemRepeater.ascx" TagName="UploadItemRepeater" TagPrefix="uc" %>

<div class="mr10t">
    <div class="drap-drop-file" onclick="uploadManagement.openModelUpload(this)">
        <div class="file-upload-wrapper">
            <div id="drapAndDrop" class="drap-and-drop">
                <div>
                    <img height="30" width="25" src="../res/images/icon-drap-and-drop.png" />
                </div>
                <div class="mr10t">
                    <asp:Label CssClass="text" ID="uxDrapLable" runat="server" meta:resourcekey="drapAndDropHere"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <tek:RadAjaxPanel runat="server" ID="UpdatePanel" LoadingPanelID="uxLoadingPanelCustom">
        <tek:RadAsyncUpload OnClientFileUploaded="uploadManagement.submitUploadFile" OnClientValidationFailed="uploadManagement.OnClientValidationFailed"
            MultipleFileSelection="Automatic" DropZones=".drap-drop-file" OnClientFileDropped="uploadManagement.OnClientFileDropped" 
            OnClientFilesSelected="uploadManagement.onFileSelected" runat="server" ID="uploadFile" 
            OnClientFileUploading="uploadManagement.onFileUploading"
            OnClientProgressUpdating="uploadManagement.onProgressUpdating" OnClientFilesUploaded="uploadManagement.onFileUploaded" OnClientFileUploadFailed="uploadManagement.onFileUploadFailed"
            CssClass="hidden">
        </tek:RadAsyncUpload>
        <div class="list-white" id="list-white">
            <asp:Repeater runat="server" ID="uxRepeater" OnItemDataBound="uxRepeater_OnItemDataBound" OnPreRender="uxRepeater_PreRender">
                <ItemTemplate>
                    <uc:UploadItemRepeater runat="server" ID="uxItemRepeater" OnRemoveItem="uxItemRepeater_OnRemoveItem" />
                </ItemTemplate>
            </asp:Repeater>
            <div class="uxProgress" style="display: none; vertical-align: middle; text-align: center; z-index: 9999; height: 50px; width: 100%; top: 0px; left: 0px; bottom: 0px; right: 0px; background: #FFF url('../../res/images/loading.gif') no-repeat center center;"></div>
        </div>
    </tek:RadAjaxPanel>
    <as:HiddenField ID="uxhiddenFields" runat="server" />
    <asp:Button runat="server" OnClick="btnSubmit_OnClick" ID="btnSubmitTempFile" CssClass="hidden btn btn-default ml-10x" />
    <asp:Button runat="server" OnClick="btnClick_OnClick" ID="btnSubmitUpload" CssClass="hidden btn btn-default ml-10x" />
    <asp:Button runat="server" OnClick="btnClearAllUpload_Click" ID="btnClearAllUpload" CssClass="hidden btn btn-default ml-10x" />
</div>

<tek:RadAjaxLoadingPanel runat="server" ID="uxLoadingPanelCustom" CssClass="uxLoadingPanelCustom">
</tek:RadAjaxLoadingPanel>
<as:RadAjaxManagerProxy ID="uxAjaxManager1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="btnSubmitTempFile">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxRepeater" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnSubmitUpload">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxhiddenFields" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnClearAllUpload">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxRepeater" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>
<tek:RadCodeBlock runat="server">
    <script>
        var submitUpload = "<%=btnSubmitUpload.ClientID%>";
        var asynUpload = "<%=uploadFile.ClientID%>";
        var btnSubmit_Upload = "<%=btnSubmitTempFile.ClientID%>";
        var ux_upload_uxhiddenFields = "<%=uxhiddenFields.ClientID%>";
        var ux_upload_btnClearAllUpload = "<%=btnClearAllUpload.ClientID%>";
        var maxFiles = "<%= GeneralFuncsLib.DataShare_MaximumFiles %>";
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/dataShare/uploadManagement.js"></script>

</tek:RadCodeBlock>
