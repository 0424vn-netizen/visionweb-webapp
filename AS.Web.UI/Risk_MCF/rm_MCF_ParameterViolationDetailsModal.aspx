<%@ Page Title="Parameter Violation Detail" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_ParameterViolationDetailsModal.aspx.cs" Inherits="rm_MCF_ParameterViolationDetailsModal" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<asp:Content ContentPlaceHolderID="ContentPage" runat="server" ID="uxMainModalContent">
    <as:ASModalContainer ID="uxModalContainer" runat="server" ContainerCssClass="container" WidthCssClass="modal-xxl">
        <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
            <AjaxSettings>

                <tek:AjaxSetting AjaxControlID="uxParameterViolationDetails">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxParameterViolationDetails" LoadingPanelID="uxLoadingPanelCustom" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </tek:RadAjaxManagerProxy>
        <div class="row">
            <div class="col-md-12">
                <div runat="server">
                    <i>
                        <as:Literal runat="server" Text="**This report includes the latest processed data that may have been received after this risk alert."></as:Literal></i>
                    <div class="height-18"></div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="modal-title title-auto-queue font-24 font-weight-bold">
                    <as:Literal ID="ltrMerchantInfor" runat="server" meta:resourcekey="ltrMerchantInforResource1"></as:Literal>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-10">
                <h2 id="uxh2Export" runat="server" class="grid-title on-top" data-toggle="collapse"></h2>
            </div>
        </div>

        <uc:UxExport ID="uxExportTop" runat="server" GridID="uxParameterViolationDetails" IsOnTop="true" ShowCSV="false" ShowPDF="false" />
        <div class="height-10"></div>

        <as:ASGrid ID="uxParameterViolationDetails" runat="server" AutoGenerateColumns="false"
            AllowFilteringByColumn="false" AllowPaging="True" IsAutoExportTemplate="true"
            VisiblePageTotal="false" VisibleReportTotal="false" AllowSorting="true" ShowPageTotal="false"
            ASPagingMethod="SPASingleMethod"
            IsIntruder="false"
            ShowFooter="true" ShowReportTotal="false" CssClass="in" meta:resourcekey="uxParameterViolationDetailsResource1">
            <MasterTableView>
                <Columns>
                </Columns>
            </MasterTableView>
        </as:ASGrid>

        <as:ASRadCodeBlock ID="uxRadCode" runat="server">
            <script type="text/javascript">
                var IsIEBrowser = "<%=GeneralFuncsLib.GetIEBrowserMode().ToString()%>";
            </script>
            <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_ParameterViolationDetailsModal.js"></script>
        </as:ASRadCodeBlock>

    </as:ASModalContainer>
</asp:Content>
