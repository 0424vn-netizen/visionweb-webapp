<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_DQSecurityReportPopup.aspx.cs"
    Inherits="As.VisionWeb.Web.RiskManagementSecurityReportModal" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="FlatReport" Src="~/UserControls/rm_MCF_UxFlatReport.ascx" TagPrefix="uc" %>
<%@ Register TagName="Transaction" Src="~/UserControls/rm_MCF_Transaction.ascx" TagPrefix="uc" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="nrt_dbBarometerReport security-main-content">
        <as:PlaceHolder ID="ads" runat="server">
            <div class="row">
                <div class="col-md-12 no-margin-bottom">
                    <div class="row d-flex space-between">
                        <div class="col-md-10">
                            <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Security Report" HasFilteringOption="true" meta:resourcekey="PageResource1" />
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
                <!--FLAT REPORT-->
                <uc:FlatReport ID="uxFlatReport" runat="Server" Visible="true" />
            </as:Panel>
        </as:PlaceHolder>

        <div id="uxProgress" style="display: none; padding: auto; position: fixed; vertical-align: middle; text-align: center; z-index: 9999; height: 100%; width: 100%; top: 0px; left: 0px; bottom: 0px; right: 0px; background: #FFF url('../res/images/loading.gif') no-repeat center center; opacity: .7;">
        </div>
    </div>
   <as:ASRadToolTip ID="uxTooltipDupeCount" meta:resourcekey="DupeCount" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
        TargetControlID="" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
    </as:ASRadToolTip>

    <as:ASRadCodeBlock ID="radCodeBlock1" runat="Server">
        <script>
            var msgRisk_HasNoWorkQueueAs = '<%=GetLocalResourceObject("Risk_HasNoWorkQueueAs").ToString() %>';
            var applyFilteredId = '<%= ApplyFilterId%>';
            var uxTooltipDupeCount;
            Sys.Application.add_load(function () {
                uxTooltipDupeCount = $find("<%= uxTooltipDupeCount.ClientID %>");
            });
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_DQSecurityReportPopup.js"></script>
    </as:ASRadCodeBlock>
</asp:Content>


