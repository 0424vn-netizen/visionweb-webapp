<%@ Page Title="Issuing Bank" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IssuingBank.aspx.cs" Inherits="IssuingBank" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/IssuingBank.ascx" TagName="IssuingBank" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <as:Panel ID="Panel1" runat="server" DefaultButton="uxSearchReport" meta:resourcekey="PanelResource1">
        <div class="row collapse report-filter-panel">
            <div class="col-md-12 report-filter">
                <div class="issuing-search-group">
                    <label>
                        <as:Literal ID="ltEnterSixNumber" runat="server" Text="Enter the Bank Identification Number (BIN):" meta:resourcekey="ltEnterSixNumberResource1"></as:Literal>
                    </label>
                    <div>
                        <div class="filter-item text-left">
                            <as:TextBox ID="uxBinNumber" runat="server" MaxLength="8" CssClass="filter-bin" Width="335px" HintCss="hint" meta:resourcekey="uxBinNumberResource1"></as:TextBox>
                        </div>
                        <div class="filter-item valign-bottom">
                            <as:Button ID="uxSearchReport" runat="server" Text="Submit" OnClientClick="return validateInputValue()"
                                OnClick="uxSearchReport_Click" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSearchReportResource1" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </as:Panel>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter">
                <as:Literal ID="ltFilter" runat="server" Text="FILTER" meta:resourcekey="ltFilterResource1"></as:Literal></span>
        </div>
    </div>
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Issuing Bank and Card Information" meta:resourcekey="uxPageTitleResource1" />
    <div ID="Literal1" runat="server" Visible="false"><%=GetLocalResourceObject("BinNumberRequired") %></div>
    <uc:IssuingBank ID="uxIssuingBank" runat="server" Visible="False" />
    <as:RadCodeBlock runat="server" ID="uxRadCodeBlock">
        <script>
            var uxBinNumber_ID = '<%= uxBinNumber.ClientID%>';
            var binNumberSpecialResource = "<%= GetLocalResourceObject("BinNumberSpecialResource1") %>";
            var first6_Or8 = "<%= Resources.MessageManager.First6_Or8 %>";
            var binNumberRequired = "<%=  GetLocalResourceObject("BinNumberRequired") %>";
        </script>
        <script type="text/javascript" src='<%= ResolveUrl("~/") %>res/js/IssuingBank.js'></script>
    </as:RadCodeBlock>
</asp:Content>

