<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gate.aspx.cs" Inherits="gate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Gate</title>
    <tek:RadCodeBlock ID="rcBlock" runat="server">
        <script src="<%=ResolveUrl("~/")%>res/js/jquery/jquery-3.6.0.min.js" type="text/javascript"></script>
        <script src="<%=ResolveUrl("~/")%>res/js/jquery/jquery-migrate-3.3.2.min.js" type="text/javascript"></script>
        <%--<script type="text/javascript">
            function requestHeader() {
                $.ajax({
                    type: "GET",
                    url: "<%=ResolveUrl("~/")%>Header.aspx",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                });
            }
        </script>--%>
    </tek:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
