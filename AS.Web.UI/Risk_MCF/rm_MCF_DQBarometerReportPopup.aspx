<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_DQBarometerReportPopup.aspx.cs" EnableEventValidation="false"
    Inherits="As.VisionWeb.Web.RiskMangementBaorometerReportModal" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<%@ Register TagName="DetectionQueueRainbowReport" Src="~/UserControls/rm_MCF_DetectionQueueRainbowReport.ascx" TagPrefix="uc" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="nrt_dbBarometerReport">
        <as:PlaceHolder ID="ads" runat="server">
            <div class="row">
                <div class="col-md-12 no-margin-bottom">
                    <div class="row d-flex space-between">
                        <div class="col-md-10">
                            <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Barometer Report" HasFilteringOption="true" meta:resourcekey="PageResource1" />
                        </div>

                        <div class="col-md-2">
                            <asp:LinkButton ID="uxRefreshBtn" CssClass="pull-right mt-10" Text="Refresh Page" OnClientClick="refreshDataEvent(); return false;" runat="server" meta:resourcekey="RefreshPageResource"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="height-30"></div>
                </div>
            </div>
            <!--FLAT or RAINBOW REPORT-->
            <as:Panel ID="pnlFlatAndRainbowReport" runat="server" meta:resourcekey="pnlFlatAndRainbowReportResource1" CssClass="pos-relative">
                <!--BAROMETER REPORT-->
                <uc:DetectionQueueRainbowReport ID="uxRainbowReport" runat="Server" />
            </as:Panel>
            <as:ASRadCodeBlock ID="radCodeBlock1" runat="Server">
                <script>
                    var msgRisk_HasNoWorkQueueAs = '<%=GetLocalResourceObject("Risk_HasNoWorkQueueAs").ToString() %>';
                    var applyFilteredId = '<%=ApplyFilterId%>';
                </script>
                <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_DQBarometerReportPopup.js"></script>
            </as:ASRadCodeBlock>
        </as:PlaceHolder>
    </div>
</asp:Content>

