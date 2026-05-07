<%@ Page Title="Case Management" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="JumpToCase.aspx.cs" Inherits="JumpToCase" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" Runat="Server">
   <div id="uxProgress" style="padding: auto; position: fixed; vertical-align: middle;
        text-align: center; z-index: 9999; height: 100%; width: 100%; top: 0px; left: 0px; bottom:0px;
        right: 0px; background: #FFF url('res/Images/loading.gif') no-repeat center center">        
    </div>
    <asp:Literal ID="uxMsg" runat="server" meta:resourcekey="uxMsgResource1"/>
     <div style="display:none">
        <asp:Button runat="server" ID="uxRePost" OnClick="uxRePost_Click" meta:resourcekey="uxRePostResource1" />
        <asp:HiddenField runat="server" ID="uxResponseValue" />
    </div>
    
    <script type="text/javascript">
        function response(data) {
            var gateFullUrl = "<%=BuildGateUrl(false) %>";
            var isIframe = <%=IsIframeSupported.ToString().ToLower() %>;
           
            if ($.trim(data) == "1" && gateFullUrl != "") {                 
                if(isIframe){                                    
                    parent.document.getElementById("uxIframeCaseHitory").src = gateFullUrl; 
                }                   
                else {
                    window.location.href = gateFullUrl;
                }
            }
            else {
                $("#<%=uxResponseValue.ClientID %>").val(data);
                document.getElementById("<%=uxRePost.ClientID %>").click();
            }
        }
    </script>  
    <asp:Literal runat="server" ID="uxScript" meta:resourcekey="uxScriptResource1"></asp:Literal>   
    
</asp:Content>

