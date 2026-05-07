<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="CaseModal1.aspx.cs" Inherits="CaseModal1" Title="Case Management" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <div style="padding:10px; width:300px; height:110px;">
        <as:Container ID="asContainter" runat="server" HeaderText="Case Management" Width="100%" FooterControlID="" FooterText="" HeaderControlID="" TemplateName="ascontainer_greyborder.tpl" meta:resourcekey="asContainterResource1">
            <div style="text-align:center;">
                <as:Literal ID="uxMessage" runat="server" meta:resourcekey="uxMessageResource1"></as:Literal>
            </div>
        </as:Container>
        <div style="text-align:right; margin-top:10px;">
            <as:Button ID="uxClose" runat="server" Text="Close" IsStandardButton="False" meta:resourcekey="uxCloseResource1" />
        </div>
    </div>
        <tek:RadCodeBlock runat="server" ID="RadCodeBlock1">
        <script type="text/javascript" language="javascript">
            function doClose(url) {
                parent.location.href = url;
            }
            function master_closeModalEvent() {
                document.getElementById('<%=uxClose.ClientID %>').click();
            }
        </script>
    </tek:RadCodeBlock>
</asp:Content>

