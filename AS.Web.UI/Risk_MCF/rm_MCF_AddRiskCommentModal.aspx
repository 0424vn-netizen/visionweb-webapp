<%@ Page Title="Add Risk Comment" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_AddRiskCommentModal.aspx.cs" Inherits="rm_MCF_AddRiskCommentModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="modal-md">
        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title"><asp:Literal ID="rm_AddRiskCommentModal_aspx_MerchantName" runat="server" meta:resourcekey="rm_AddRiskCommentModal_aspx_MerchantNameResource1" Text=" Merchant Name: "></asp:Literal>
                    <asp:Literal ID="uxMerchantName" runat="server" meta:resourcekey="uxMerchantNameResource1"></asp:Literal>
                </h3>
                <div class="height-12"></div>
                <h3 class="modal-title">
                    <span class="text-muted"><asp:Literal ID="rm_AddRiskCommentModal_aspx_MerchantNum" runat="server" meta:resourcekey="rm_AddRiskCommentModal_aspx_MerchantNumResource1" Text="Merchant ID: "></asp:Literal>
                    <asp:Literal ID="uxMerchantNumber" runat="server" meta:resourcekey="uxMerchantNumberResource1"></asp:Literal>
                    </span>
                </h3>
                <div class="height-6"></div>
            </div>
        </div>
       
        <div class="row">
            <div class="col-md-12">
                <div class="inline-block">
                <asp:CheckBox ID="uxManageComment" runat="server" meta:resourcekey="uxManageCommentResource1"/> 
                    </div><div class="control-inline"><label><asp:Literal ID="rm_AddRiskCommentModal_aspx_ManagementComment" runat="server" meta:resourcekey="rm_AddRiskCommentModal_aspx_ManagementCommentResource1" Text="Management Comment"></asp:Literal></label></div>
                <div class=" height-6"></div>
                <tek:RadEditor ID="uxComment" runat="server" EditModes="Design" StripFormattingOptions="MSWordRemoveAll" OnClientLoad="OnClientLoad"
                    ToolsFile="~/App_Data/RadEditorConfig.xml" Height="200px" Width="100%" Font-Names="Arial" OnClientPasteHtml="onHtmlPaste">
                    <CssFiles>
                        <tek:EditorCssFile Value="~/res/css/Editor.css" />
                    </CssFiles>
                </tek:RadEditor>
                <div class="bottom-error">
                    <label class="error display-none" id="ciCommentsMsg"></label>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-6 text-left">
                <div class="height-6"></div>
               <div> <span id="counter"></span></div>
            </div>
            <div class="col-xs-6 text-right form-action-container">
                <asp:Button ID="uxSubmit" runat="server" Text="Submit" OnClick="uxSubmit_Click" class="btn btn-default" OnClientClick="return Validate();" meta:resourcekey="uxSubmitResource1" />
                <asp:Button ID="uxCancel" OnClientClick="window.close(); return false;" Text="Cancel" class="btn btn-default"  runat="server" meta:resourcekey="uxCancelResource1"  ></asp:Button>
            </div>
        </div>
        </div>
    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var rm_AddRiskCommentModal_uxComment = "<%=uxComment.ClientID %>";   
            var rm_AddRiskCommentModal_IsIEBrowser = "<%=IsIEBrowser.ToString()%>";
            var rm_AddRiskCommentModal_js_msg1 = '<%= GetLocalResourceObject("rm_AddRiskCommentModal_js_msg1").ToString() %>';
            var rm_AddRiskCommentModal_js_characters = '<%= GetLocalResourceObject("rm_AddRiskCommentModal_js_characters").ToString() %>';
            var rm_AddRiskCommentModal_js_CharacterUpper = '<%= GetLocalResourceObject("rm_AddRiskCommentModal_js_CharacterUpper").ToString() %>';
            var rm_AddRiskCommentModal_js_msg2 = '<%= GetLocalResourceObject("rm_AddRiskCommentModal_js_msg2").ToString() %>';
            
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_AddRiskCommentModal.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
