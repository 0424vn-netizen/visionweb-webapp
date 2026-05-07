<%@ Application Language="C#" %>
<%@ Import Namespace="System.Security.Principal" %>

<script runat="server">

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

        Exception error = Server.GetLastError();
        if (error.InnerException != null) error = error.InnerException;
        AS.Common.Logger.LoggerManager.Error(error);

    }
    
</script>
