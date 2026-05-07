<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SSO.aspx.cs" Inherits="As.VisionWeb.Web.SingleSignOn" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>VisionWeb</title>    
</head>
<body>
    <form id="form1" runat="server">
        <asp:PlaceHolder runat="server" ID="uxPlhLoading" Visible="true">
            <div class="loading-full-bg">Loading...</div>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="uxPlhInfo" Visible="false">
            The Autologin Failed
        </asp:PlaceHolder>
    </form>
</body>
</html>