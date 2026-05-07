<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManageProfile.aspx.cs" Inherits="As.VisionWeb.Web.ManageProfile" ValidateRequest="false" Title="Update My Profile" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="UserProfile" Src="~/UserControls/UserProfile.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <uc:PageTitle ID="uxReportTitle" HasFilteringOption="false" runat="server" ReportTitle="Update My Profile" meta:resourcekey="uxReportTitleResource1" />
        </div>
    </div>
    <uc:UserProfile ID="uxUpdateProfile" runat="server" /> 
</asp:Content>
