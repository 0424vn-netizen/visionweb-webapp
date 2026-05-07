<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateNewASUser.aspx.cs" Title="USER MAINTENANCE - Create User"
    Inherits="_mps_CreateNewASUser" MasterPageFile="~/MasterPage.master" meta:resourcekey="PageResource1" %>

<%@ Register Src="UserControls/ManageASUser.ascx" TagName="ManageUser" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <div class="row">
        <div class="col-md-12 no-margin-bottom">
            <uc:PageTitle ID="uxReportTitle" HasFilteringOption="false" runat="server" ReportTitle="Create Aperia User" meta:resourcekey="uxCreateAperiaUserResourceTitle" />
            <div class="height-22"></div>
        </div>
    </div> 
    <uc:ManageUser ID="uxManageUser" runat="server" /> 
</asp:Content>
