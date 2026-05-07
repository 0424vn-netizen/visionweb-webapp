<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MessageEditor.ascx.cs"
    Inherits="UserControls_MessageEditor" %>
<div id="uxHeader" runat="server">
    <asp:Literal ID="Literal1" runat="server" Text="Enter new Comment (Limit 1000 characters per comment)" meta:resourcekey="LiteralResource1" />
</div>
<as:TextBox ID="uxMessage" runat="server" TextMode="MultiLine" MaxLength="1000" CssClass="form-control"
    Height="120px" Width="100%" onKeyDown="CountRemainingCharacter();" onKeyUp="CountRemainingCharacter();" HintCss="hint" meta:resourcekey="uxMessageResource1"></as:TextBox>
<div class="bottom-error">
    <as:ValidatorMessage ID="ValidatorMessage1" runat="server" ApplyFor="uxMessage" Message="" ShowOnLoad="False"></as:ValidatorMessage>
</div>
<span id="uxRemainingCharacter" runat="server" class="text-muted"><asp:Literal ID="Literal2" runat="server" Text="You have 1000 characters remaining for your comment." meta:resourcekey="LiteralResource2" /></span>
    
<as:Validator runat="server" ID="uxValidator" ValidationFunction="MessageEditor_ValidateMessage" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="uxValidatorResource1">
    <Items>
        <as:BasicValidationItem Rule="Required" ControlToValidateID="uxMessage" meta:resourcekey="BasicValidator1Resource1"/>
        <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>" ControlToValidateID="uxMessage" meta:resourcekey="BasicValidator2Resource1"/>
    </Items>
</as:Validator>
<tek:RadCodeBlock ID="RadCodeBlock" runat="server">
    <script type="text/javascript">
        var MessageEditor_uxRemainingCharacter = $("#<%=uxRemainingCharacter.ClientID %>");
        var MessageEditor_uxMessage = $("#<%=uxMessage.ClientID %>");
        var MessageEditor_MaxCharacter = "<%=this.MaxCharacter %>";
        var MessageEditor_idMessage = "<%=uxMessage.ClientID %>";

        var Text_YouHave = '<%=GetLocalResourceObject("MessageEditorJS_Text_YouHave").ToString()%>';
        var Text_CharsRemaining = '<%=GetLocalResourceObject("MessageEditorJS_Text_CharsRemaining").ToString()%>';
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MessageEditor.js"></script>
</tek:RadCodeBlock>
