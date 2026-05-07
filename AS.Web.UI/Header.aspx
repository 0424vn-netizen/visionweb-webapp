<%@ Page Title="Landing Page" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    Inherits="NonReportPage" meta:resourcekey="PageResource1" EnableEventValidation="false" %>
    
<script runat="server">
    //38605: Risk keep alive to auto clear Work staus: WIP by me
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void KeepAlive()
    {
        RM_MCF_GeneralFuncsLib.KeepAliveWIP();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // 58567 - Recurring System Message for Merchants
        GeneralFuncsLib.SetCookie("is_open_recurring_system_message", GeneralFuncsLib.IsOpenRecurringSystemMessage().ToString());
    }

    [System.Web.Services.WebMethod]
    public static void CreateCookie(string key, string value)
    {
        GeneralFuncsLib.SetCookie(key, value);
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server" Visible="false">
    [Main-Content]   
</asp:Content>

