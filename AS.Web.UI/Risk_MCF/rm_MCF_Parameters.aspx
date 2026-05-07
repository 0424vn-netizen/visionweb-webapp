<%@ Page Title="Parameters" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="rm_MCF_Parameters.aspx.cs" Inherits="rm_MCF_Parameters" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTile" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxParameter" Src="~/UserControls/rm_MCF_Parameter.ascx" TagPrefix="uc" %>
<%--<%@ Register TagName="SiteID_Selector" Src="~/UserControls/SiteID_Selector.ascx" TagPrefix="uc" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:PageTile ID="uxPageTitle" runat="server" ReportTitle="Parameters" HasFilteringOption="false" meta:resourcekey="uxPageTitleResource1" />
    <div class="row parameter-data">
        <div class="col-md-12">
            <div class="box">
                <i>
                    <b><asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1" Text=" Default Parameters:"></asp:Literal></b><br />
                    <as:Literal runat="server" ID="uxNoteMPS" meta:resourcekey="uxNoteMPSResource1"></as:Literal>
                    <asp:Literal ID="Literal2" runat="server" meta:resourcekey="Literal2Resource1" Text="** A threshold of $0.00 will display all merchants who meet the parameter. A specified threshold will only display the merchants who meet the parameter and the minimum dollar threshold."></asp:Literal>
                </i>
            </div>
            <div class="height-12"></div>
            <uc:UxParameter ID="uxParam" runat="Server" ShowRiskScore="false" RiskScoreEnabled="false" ShowRowNumberColumn="false"
                OnNeedDataSource="uxParam_NeedDataSource" OnNeedExportConfig="uxParam_NeedExportConfig" ShowExport="true"
                TempDisableDuplicates="false" OnProcessSave="uxParam_ProcessSave" />
            <div class="height-18"></div>
            <div class="height-20"></div>
        </div>
    </div>
    
    <div id="fixCommandArea">
        <div id="commandArea">
            <div class="height-12"></div>
                <as:Button ID="uxBntSave" runat="server" Text="Save" OnClientClick="btnSave_Click(); return false;" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxBntSaveResource1" />
            <div class="height-24"></div>
        </div>
    </div>
    

    <as:ASRadCodeBlock ID="radCodeBlock" runat="server">
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Parameters.js"></script>
    </as:ASRadCodeBlock>

</asp:Content>

