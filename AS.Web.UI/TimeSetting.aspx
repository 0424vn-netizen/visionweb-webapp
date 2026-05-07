<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TimeSetting.aspx.cs" Inherits="TimeSetting" meta:resourcekey="PageResource1"%>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="TimeSettingDetail" Src="~/UserControls/TimeSettingDetail.ascx" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <div class="row">
        <div class="col-md-12">
            <uc:PageTitle ID="uxReportTitle" HasFilteringOption="false" runat="server" meta:resourcekey="uxReportTitleResource1" />
        </div>
    </div>
    <uc:TimeSettingDetail ID="uxTimeSetting" runat="server" /> 
</asp:Content>

