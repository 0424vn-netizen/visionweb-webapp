<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <strong>Test Decryption</strong><br />
    Enter an encrypted value: <asp:TextBox ID="uxEncryptedString" runat="server" Width="300px"></asp:TextBox>
    <asp:Button ID="uxDecrypt" runat="server" Text="Decrypt" 
            onclick="uxDecrypt_Click" />
    <br />
    <strong>Test Encryption</strong><br />
    Enter a value: <asp:TextBox ID="uxClearText" runat="server"></asp:TextBox> 
        <asp:Button ID="uxEncrypt" runat="server" Text="Encrypt" 
            onclick="uxEncrypt_Click" />
        
    <br />
    <strong>Result:</strong><br />
    <asp:Literal ID="uxResult" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
