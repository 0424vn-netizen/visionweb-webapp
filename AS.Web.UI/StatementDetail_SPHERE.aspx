<%@ Page Title="STATEMENT DETAILS" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="StatementDetail_SPHERE.aspx.cs" Inherits="As.VisionWeb.Web.StatementDetail_SPHERE" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagPrefix="as" Namespace="AS.Controls.Global" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <div class="row" id="uxContent" runat="server" visible="true">
            <div class="col-md-9 text-left">
                <uc:PageTitle ID="uxReportTitle" runat="server" ReportTitle="<img src='App_Themes/{0}/img/logo.png' alt='logo' />" meta:resourcekey="uxReportTitleResource1" />
            </div>
            <div class="col-md-3">
                <div class="report-export-no-title pull-right on-top">
                    <span class="export-separator">|</span><a href="#" title="Printer Friendly Version" onclick="window.print()"><as:Literal ID="ltPRINT" runat="server" Text="PRINT" meta:resourcekey="ltPRINTResource1"></as:Literal></a>
                </div>
                <div class="report-export-no-title dropdown on-top pull-right">
                    <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle" id="litExport" runat="server">
                        <as:Literal ID="Literal1" runat="server" Text="EXPORT" meta:resourcekey="Literal1Resource1"></as:Literal></a>
                    <ul class="dropdown-menu">
                        <li id="uxLiExcel" runat="server">
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="ButtonExcel_Click" meta:resourcekey="LinkButton1Resource1">Excel</asp:LinkButton></li>
                        <li id="uxLiPDF" runat="server">
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="ButtonPDF_Click" meta:resourcekey="LinkButton2Resource1">PDF</asp:LinkButton></li>
                    </ul>
                </div>
            </div>
        </div>
        <as:PlaceHolder ID="uxContentExport" runat="server">
            <asp:PlaceHolder ID="uxExporterPlh" runat="server">
                <as:PlaceHolder ID="uxPlaceHolderStylePDF" runat="server" Visible="false">
                  <html>
                   <head>
                        <style type="text/css">
                            * {
                                font-size: 9px;
                            }

                            .ReportTotal {
                                font-weight: bold;
                            }

                            .ASTable th {
                                border-bottom-width: thin;
                                border-bottom-style: solid;
                            }

                            .ASTable td {
                                border-bottom-width: thin;
                                border-bottom-style: solid;
                            }

                            .text-right {
                                text-align: right;
                            }

                            h2 {
                                font-size: 12px;
                                text-align: center;
                            }

                            .text-center {
                                text-align: center;
                            }

                            table tr th {
                                font-size: 9px;
                            }

                            table tr td {
                                font-size: 8px;
                            }

                            .text-big {
                                font-size: 10px !important;
                            }

                            table.no-border {
                                border: 0px;
                            }

                            .title {
                                font-size: 10px;
                            }

                            .title-report {
                                font-weight: 700;
                                font-size: 10px;
                            }

                            table th {
                                background: #676767;
                                color: #fff;
                                font-weight: bold;
                            }

                            table {
                                border-collapse: collapse;
                                border: solid 1px #676767;
                                table-layout: auto;
                                width: 100%;
                            }

                            .report-table td {
                                border-right: solid 1pt #676767;
                            }

                                .report-table td.first-col {
                                    border-left: solid 1px #676767;
                                }

                            .TotalBox {
                                font-weight: 800;
                                font-size: 10px;
                                padding: 10px;
                                border: 0px !important;
                            }

                            .footer {
                                background-color: #CCC;
                                font-weight: bold;
                                border-bottom: 1px solid #676767;
                            }

                            .ContainerPanelHeader {
                                background-color: #CCC;
                                font-weight: bold;
                                border: solid #676767;
                                border-width: 1px 1px 0px 1px;
                                font-weight: 800;
                                font-size: 9px;
                                padding: 5px;
                                margin-top: 10px;
                            }

                            .height-24 {
                                display: block;
                                height: 24px;
                            }

                            .box-add-note {
                                text-align: center;
                                background-color: #CCC;
                                font-weight: bold;
                            }
                        </style>
                   
                  </head>
                      <body>
                </as:PlaceHolder>
                <as:PlaceHolder ID="uxPlaceHolderStyleExcel" runat="server" Visible="false">
                  <html>
                    <head>                    
                        <style type="text/css">
                            * {
                                font-size: 11px;
                            }

                            .ReportTotal {
                                font-weight: bold;
                            }

                            .ASTable th {
                                border-bottom-width: thin;
                                border-bottom-style: solid;
                            }

                            .ASTable td {
                                border-bottom-width: thin;
                                border-bottom-style: solid;
                                font-size: 13px !important;
                            }

                            .text-right {
                                text-align: right;
                            }

                            h2 {
                                font-size: 14px;
                                text-align: center;
                            }

                            .text-center {
                                text-align: center;
                            }

                            table tr th {
                                font-size: 13px;
                            }

                            table tr td {
                                font-size: 13px;
                            }

                            .text-big {
                                font-size: 14px !important;
                            }

                            .no-border {
                                border-width: 0 !important;
                            }

                            .title {
                                font-size: 13px;
                                font-weight: 700;
                            }

                            .title-report {
                                font-weight: 700;
                                font-size: 13px;
                            }

                            table th {
                                background: #676767;
                                color: #fff;
                                font-weight: bold;
                            }

                            table {
                                border-collapse: collapse;
                                border: solid 1px #676767;
                                table-layout: auto;
                                width: 100%;
                            }

                            .report-table td {
                                border-right: solid 1pt #676767;
                            }

                                .report-table td.first-col {
                                    border-left: solid 1px #676767;
                                }

                            .TotalBox {
                                font-weight: 800;
                                font-size: 12px;
                                padding: 10px;
                                border: 0px !important;
                            }

                            .footer {
                                background-color: #CCC;
                                font-weight: bold;
                                border-bottom: 1px solid #676767;
                            }

                            .ContainerPanelHeader {
                                background: #676767;
                                font-weight: bold;
                                border: solid 1px #676767;
                                border-width: 1px 1px 0px 1px;
                                font-size: 14px;
                                width: 100%;
                                display: block;
                            }

                            .height-24 {
                                display: block;
                                height: 24px;
                            }

                            .box-add-note {
                                text-align: center;
                                background-color: #CCC;
                                font-weight: bold;
                            }

                            .total-fees {
                                font-weight: bold;
                            }
                        </style>
                    </head>
                    <body>
                   
                </as:PlaceHolder>
                <%--WRFC address information--%>
                <as:Panel ID="uxPnlClientLogo" runat="server" Visible="false">
                    <div class="row" style="display: block;">
                        <div class="col-md-12 text-left">
                            <as:Literal ID="uxLogo" runat="server" />
                        </div>
                    </div>
                </as:Panel>
                <as:Panel ID="uxPnlClientInfo" runat="server" meta:resourcekey="uxPnlClientInfoResource1">
                    <div class="row">
                        <div class="col-md-12 text-left">
                            <strong>
                                <asp:Literal ID="uxClientInfo" runat="server" meta:resourcekey="uxClientInfoResource1"></asp:Literal>
                            </strong>
                        </div>
                    </div>
                </as:Panel>


                <div style="text-align: left;">

                    <table style="width: 100%">
                        <tr>
                            <th align="left" colspan="10">
                                <as:Literal runat="server" Text="Merchant Mailing Information" meta:resourcekey="uxFilteringTableResource1"></as:Literal>
                            </th>
                        </tr>
                        <asp:Repeater ID="uxMerchantInfo" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td align="left" valign="top" colspan="3" style="padding-left: 6px;">
                                        <%# Eval("MailingName")%>
                                    </td>
                                    <td align="right" colspan="4"></td>
                                    <td align="right" colspan="3" style="padding-right: 20px;">
                                        <as:Literal ID="Literal2" runat="server" Text="Processing Month:" meta:resourcekey="Literal2Resource1"></as:Literal>
                                        <%# Eval("ProcessingMonth") %>&nbsp;&nbsp;&nbsp;
                                        <%# Eval("Bank") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" colspan="3" style="padding-left: 6px;">
                                        <%# FormatAddress(Eval("MailingAddr1"), Eval("MailingAddr2"), Eval("MailingAddr3"), 1)%>
                                    </td>
                                    <td align="right" colspan="4"></td>
                                    <td align="right" colspan="3" style="padding-right: 20px;">
                                        <as:Literal ID="Literal3" runat="server" Text="Association Number:" meta:resourcekey="Literal1Resource2"></as:Literal>
                                        <%# Eval("AssociationNumber")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" colspan="3" style="padding-left: 6px;">
                                        <%# FormatAddress(Eval("MailingAddr1"), Eval("MailingAddr2"), Eval("MailingAddr3"), 2)%>
                                    </td>
                                    <td align="right" colspan="4"></td>
                                    <td align="right" colspan="3" style="padding-right: 20px;">
                                        <as:Literal ID="Literal4" runat="server" Text="Merchant ID:" meta:resourcekey="Literal3Resource1"></as:Literal>
                                        <%# Eval("MerchantNumber")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" colspan="3" style="padding-left: 6px;">
                                        <%# FormatAddress(Eval("MailingAddr1"), Eval("MailingAddr2"), Eval("MailingAddr3"), 3)%>
                                    </td>
                                    <td align="right" colspan="4"></td>
                                    <td align="right" colspan="3" style="padding-right: 20px;">
                                        <as:Literal ID="Literal5" runat="server" Text="Routing Number:" meta:resourcekey="Literal4Resource1"></as:Literal>
                                        <%# Eval("RoutingNumber")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" colspan="3"></td>
                                    <td align="right" colspan="4"></td>
                                    <td align="right" colspan="3" style="padding-right: 20px;">
                                        <as:Literal ID="Literal6" runat="server" Text="Deposit Account Number:" meta:resourcekey="Literal5Resource1"></as:Literal>
                                        <%# Eval("DepositAcctNumber")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="10">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="10">
                                <!--Implement for export excel: text is red-->
                                <table class="no-border">
                                    <tr>
                                        <td align="center" colspan="10"><b>
                                            <as:Literal ID="Literal7" runat="server" Text="AMOUNT DEDUCTED FROM ACCOUNT:" meta:resourcekey="Literal5Resource2"></as:Literal>
                                            <asp:Literal ID="uxAmountDeducted" runat="server" meta:resourcekey="uxAmountDeductedResource1"></asp:Literal></b>

                                        </td>
                                    </tr>
                                </table>
                                <!-- End -->
                            </td>
                        </tr>
                    </table>
                    <div style="clear: both;">
                    </div>

                </div>
                <br />
                <br />

                <!-- STATEMENT MESSAGE -->
                <as:Panel ID="pnlMessage" runat="server" meta:resourcekey="pnlMessageResource1">
                    <div style="text-align: center;">
                        <table class="no-border w-100">
                            <tr>
                                <td colspan="10"></td>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    <asp:Literal ID="uxMessage" runat="server" meta:resourcekey="uxMessageResource1"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10"></td>
                            </tr>
                        </table>
                    </div>
                </as:Panel>
                <as:Panel ID="pnlBreakLineExportPlanSummary" runat="server" Visible="false">
                    <br />
                </as:Panel>
                <!-- PLAN SUMMARY -->
                <as:Panel ID="pnlPlanSummary" runat="server" meta:resourcekey="pnlPlanSummaryResource1">
                    <div class="row">
                        <div class="col-xs-12">
                            <h2 class="grid-title" id="h2" data-toggle="collapse" data-target="#PlanSummaryID">
                                <as:Literal ID="Literal8" runat="server" Text="Plan Summary" meta:resourcekey="Literal6Resource1"></as:Literal>
                            </h2>
                        </div>
                    </div>
                    <div id="PlanSummaryID" class="in">
                        <table class="ASTable">
                            <tr>
                                <th title="Plan Type">
                                    <as:Literal ID="Literal9" runat="server" Text="PL" meta:resourcekey="Literal7Resource1"></as:Literal>
                                </th>
                                <th title="Sales Count">
                                    <as:Literal ID="Literal10" runat="server" Text="# Sales" meta:resourcekey="Literal8Resource1"></as:Literal>
                                </th>
                                <th title="Sales Amount">
                                    <as:Literal ID="Literal11" runat="server" Text="$ Sales" meta:resourcekey="Literal9Resource1"></as:Literal>
                                </th>
                                <th title="Credits Count">
                                    <as:Literal ID="Literal12" runat="server" Text="# Credits" meta:resourcekey="Literal10Resource1"></as:Literal>
                                </th>
                                <th title="Credits Amount">
                                    <as:Literal ID="Literal13" runat="server" Text="$ Credits" meta:resourcekey="Literal11Resource1"></as:Literal>
                                </th>
                                <th title="Net Sales">
                                    <as:Literal ID="Literal14" runat="server" Text="Net Sales" meta:resourcekey="Literal12Resource1"></as:Literal>
                                </th>
                                <th title="Average Ticket">
                                    <as:Literal ID="Literal15" runat="server" Text="Avg Tkt" meta:resourcekey="Literal13Resource1"></as:Literal>
                                </th>
                                <th title="DISC P/I">
                                    <as:Literal ID="Literal16" runat="server" Text="Disc P/I" meta:resourcekey="Literal14Resource1"></as:Literal>
                                </th>
                                <th title="Percentage">
                                    <as:Literal ID="Literal17" runat="server" Text="%" meta:resourcekey="Literal15Resource1"></as:Literal>
                                </th>
                                <th title="Discount Due">
                                    <as:Literal ID="Literal18" runat="server" Text="Discount Due" meta:resourcekey="Literal16Resource1"></as:Literal>
                                </th>
                            </tr>
                            <as:ASRepeater ID="uxPlanSummary" runat="server" OnItemDataBound="uxPlanSummary_ItemDataBound"
                                NumberOfColumns="10">
                                <ItemTemplate>
                                    <tr class="Row">
                                        <td class="text-center">
                                            <%# Eval("PlanType")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("SalesCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("SalesAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("CreditsCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("CreditsAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("NetAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("AverageTicket"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("Disc_PI")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("Percentage")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("DiscountDue"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AltRow">
                                        <td class="text-center">
                                            <%# Eval("PlanType")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("SalesCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("SalesAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("CreditsCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("CreditsAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("NetAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("AverageTicket"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("Disc_PI")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("Percentage")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("DiscountDue"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="uxFooter" runat="server">
                                        <tr class="total-fees">
                                            <td class="text-center">
                                                <as:Literal ID="Literal19" runat="server" Text="**" meta:resourcekey="Literal7Resource2"></as:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrPlan_SaleCount" runat="server" meta:resourcekey="uxLtrPlan_SaleCountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrPlan_SaleAmount" runat="server" meta:resourcekey="uxLtrPlan_SaleAmountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrPlan_CreditCount" runat="server" meta:resourcekey="uxLtrPlan_CreditCountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrPlan_CreditAmount" runat="server" meta:resourcekey="uxLtrPlan_CreditAmountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrPlan_NetAmount" runat="server" meta:resourcekey="uxLtrPlan_NetAmountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrPlan_AvgTkt" runat="server" meta:resourcekey="uxLtrPlan_AvgTktResource1"></asp:Literal>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrPlan_DiscountDue" runat="server" meta:resourcekey="uxLtrPlan_DiscountDueResource1"></asp:Literal>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>
                </as:Panel>
                <as:Panel ID="pnlBreakLineExportDeposit" runat="server" Visible="false">
                    <br />
                </as:Panel>
                <!-- DEPOSIT -->
                <as:Panel ID="pnlDeposit" runat="server" meta:resourcekey="pnlDepositResource1">
                    <div class="row">
                        <div class="col-xs-12">
                            <h2 class="grid-title" id="h2GridTitle" data-toggle="collapse" data-target="#DepositID">
                                <as:Literal ID="Literal20" runat="server" Text="Deposits" meta:resourcekey="Literal17Resource1"></as:Literal>
                            </h2>
                        </div>
                    </div>
                    <div id="DepositID" class="in">
                        <table class="ASTable">
                            <tr>
                                <th title="Day">
                                    <as:Literal ID="Literal21" runat="server" Text="Day" meta:resourcekey="Literal18Resource1"></as:Literal>
                                </th>
                                <th title="Reference Number">
                                    <as:Literal ID="Literal22" runat="server" Text="Ref Number" meta:resourcekey="Literal19Resource1"></as:Literal>
                                </th>
                                <th title="Tran Code">
                                    <as:Literal ID="Literal23" runat="server" Text="Tran Code" meta:resourcekey="Literal20Resource1"></as:Literal>
                                </th>
                                <th title="Plan Code">
                                    <as:Literal ID="Literal24" runat="server" Text="Plan Code" meta:resourcekey="Literal21Resource1"></as:Literal>
                                </th>
                                <th title="Sales Count">
                                    <as:Literal ID="Literal25" runat="server" Text="# Sales" meta:resourcekey="Literal22Resource1"></as:Literal>
                                </th>
                                <th title="Sales Amount">
                                    <as:Literal ID="Literal26" runat="server" Text="$ Sales" meta:resourcekey="Literal23Resource1"></as:Literal>
                                </th>
                                <th title="Credits Amount">
                                    <as:Literal ID="Literal27" runat="server" Text="$ Credits" meta:resourcekey="Literal24Resource1"></as:Literal>
                                </th>
                                <th title="Discount Paid">
                                    <as:Literal ID="Literal28" runat="server" Text="Discount PD" meta:resourcekey="Literal25Resource1"></as:Literal>
                                </th>
                                <th title="Net Deposit">
                                    <as:Literal ID="Literal29" runat="server" Text="Net Deposit" meta:resourcekey="Literal26Resource1"></as:Literal>
                                </th>
                            </tr>
                            <as:ASRepeater ID="uxDeposit" runat="server" OnItemDataBound="uxDeposits_ItemDataBound"
                                NumberOfColumns="9">
                                <ItemTemplate>
                                    <tr class="Row">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("RefNumber")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("TranType")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("PlanType")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("SalesCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("SalesAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("CreditsAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("DiscPaiD"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("NetDeposit"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AltRow">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("RefNumber")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("TranType")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("PlanType")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("SalesCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("SalesAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("CreditsAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("DiscPaiD"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("NetDeposit"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                                        <tr class="total-fees">
                                            <td class="text-center heading" colspan="4">
                                                <as:Literal ID="Literal30" runat="server" Text="DEPOSIT TOTALS" meta:resourcekey="Literal18Resource2"></as:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrDeposit_SaleCount" runat="server" meta:resourcekey="uxLtrDeposit_SaleCountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrDeposit_SaleAmount" runat="server" meta:resourcekey="uxLtrDeposit_SaleAmountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrDeposit_CreditAmount" runat="server" meta:resourcekey="uxLtrDeposit_CreditAmountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrDeposit_DiscountPD" runat="server" meta:resourcekey="uxLtrDeposit_DiscountPDResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrDeposit_NetDeposit" runat="server" meta:resourcekey="uxLtrDeposit_NetDepositResource1"></asp:Literal>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>
                </as:Panel>
                <as:Panel ID="pnlBreakLineExportAdustments" runat="server" Visible="false">
                    <br />
                </as:Panel>
                <!-- ADJUSTMENTS -->
                <as:Panel ID="pnlAdustments" runat="server" meta:resourcekey="pnlAdustmentsResource1">
                    <div class="row">
                        <div class="col-xs-12">
                            <h2 class="grid-title" id="h1" data-toggle="collapse" data-target="#AjustmentsID">
                                <as:Literal ID="Literal31" runat="server" Text="Adjustments" meta:resourcekey="Literal27Resource1"></as:Literal>
                            </h2>
                        </div>
                    </div>
                    <div id="AjustmentsID" class="in">
                        <table class="ASTable">
                            <tr>
                                <th title="Day">
                                    <as:Literal ID="Literal32" runat="server" Text="Day" meta:resourcekey="Literal28Resource1"></as:Literal>
                                </th>
                                <th title="Reference Number">
                                    <as:Literal ID="Literal33" runat="server" Text="Ref Number" meta:resourcekey="Literal29Resource1"></as:Literal>
                                </th>
                                <th title="Tran Code">
                                    <as:Literal ID="Literal34" runat="server" Text="Tran Code" meta:resourcekey="Literal30Resource1"></as:Literal>
                                </th>
                                <th title="Plan Code">
                                    <as:Literal ID="Literal35" runat="server" Text="Plan Code" meta:resourcekey="Literal31Resource1"></as:Literal>
                                </th>
                                <th title="Sales Count">
                                    <as:Literal ID="Literal36" runat="server" Text="# Sales" meta:resourcekey="Literal32Resource1"></as:Literal>
                                </th>
                                <th title="Sales Amount">
                                    <as:Literal ID="Literal37" runat="server" Text="$ Sales" meta:resourcekey="Literal33Resource1"></as:Literal>
                                </th>
                                <th title="Credits Amount">
                                    <as:Literal ID="Literal38" runat="server" Text="$ Credits" meta:resourcekey="Literal34Resource1"></as:Literal>
                                </th>
                                <th title="Discount Paid">
                                    <as:Literal ID="Literal39" runat="server" Text="Discount PD" meta:resourcekey="Literal35Resource1"></as:Literal>
                                </th>
                                <th title="Net Deposit">
                                    <as:Literal ID="Literal40" runat="server" Text="Net Deposit" meta:resourcekey="Literal36Resource1"></as:Literal>
                                </th>
                            </tr>
                            <as:ASRepeater ID="uxAdjustments" runat="server" OnItemDataBound="uxAdjustments_ItemDataBound"
                                NumberOfColumns="9">
                                <ItemTemplate>
                                    <tr class="Row">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("RefNumber")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("TranType")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("PlanType")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("SalesCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("SalesAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("CreditsAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("DiscPaiD"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("NetDeposit"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AltRow">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("RefNumber")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("TranType")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("PlanType")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("SalesCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("SalesAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("CreditsAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("DiscPaiD"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("NetDeposit"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder2" runat="server">
                                        <tr class="total-fees">
                                            <td class="text-center heading" colspan="4">
                                                <as:Literal ID="Literal41" runat="server" Text="ADJUSTMENTS TOTALS" meta:resourcekey="Literal28Resource2"></as:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrAdjustment_SaleCount" runat="server" meta:resourcekey="uxLtrAdjustment_SaleCountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrAdjustment_SaleAmount" runat="server" meta:resourcekey="uxLtrAdjustment_SaleAmountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrAdjustment_CreditAmount" runat="server" meta:resourcekey="uxLtrAdjustment_CreditAmountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrAdjustment_DiscountPD" runat="server" meta:resourcekey="uxLtrAdjustment_DiscountPDResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrAdjustment_NetDeposit" runat="server" meta:resourcekey="uxLtrAdjustment_NetDepositResource1"></asp:Literal>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>
                </as:Panel>
                <!-- IDINE -->
                <as:Panel ID="pnlBreakLineExportIDINE" runat="server" Visible="false">
                    <br />
                </as:Panel>
                <as:Panel ID="Panel1" runat="server" meta:resourcekey="pnlIDINEResource1">
                    <div class="row">
                        <div class="col-xs-12">
                            <h2 class="grid-title" id="h5" data-toggle="collapse" data-target="#IDINEID">
                                <as:Literal ID="Literal128" runat="server" Text="Idine" meta:resourcekey="LiteralIDINEResource"></as:Literal>
                            </h2>
                        </div>
                    </div>
                    <div id="IDINEID" class="in">
                        <table class="ASTable">
                            <tr>
                                <th title="Day">
                                    <as:Literal ID="Literal129" runat="server" Text="Day" meta:resourcekey="Literal38Resource1"></as:Literal>
                                </th>
                                <th title="Reference Number">
                                    <as:Literal ID="Literal130" runat="server" Text="Ref Number" meta:resourcekey="Literal39Resource1"></as:Literal>
                                </th>
                                <th title="Tran Code">
                                    <as:Literal ID="Literal131" runat="server" Text="Tran Code" meta:resourcekey="Literal40Resource1"></as:Literal>
                                </th>
                                <th title="Plan Code">
                                    <as:Literal ID="Literal132" runat="server" Text="Plan Code" meta:resourcekey="Literal41Resource1"></as:Literal>
                                </th>
                                <th title="Sales Count">
                                    <as:Literal ID="Literal133" runat="server" Text="# Sales" meta:resourcekey="Literal42Resource1"></as:Literal>
                                </th>
                                <th title="Sales Amount">
                                    <as:Literal ID="Literal134" runat="server" Text="$ Sales" meta:resourcekey="Literal43Resource1"></as:Literal>
                                </th>
                                <th title="Credits Amount">
                                    <as:Literal ID="Literal135" runat="server" Text="$ Credits" meta:resourcekey="Literal44Resource1"></as:Literal>
                                </th>
                                <th title="Discount Paid">
                                    <as:Literal ID="Literal136" runat="server" Text="Discount PD" meta:resourcekey="Literal45Resource1"></as:Literal>
                                </th>
                                <th title="Net Deposit">
                                    <as:Literal ID="Literal137" runat="server" Text="Net Deposit" meta:resourcekey="Literal46Resource1"></as:Literal>
                                </th>
                            </tr>
                            <as:ASRepeater ID="uxIDINE" runat="server" OnItemDataBound="uxIDINE_ItemDataBound"
                                NumberOfColumns="9">
                                <ItemTemplate>
                                    <tr class="rgRow">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("RefNumber")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("TranType")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("PlanType")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("SalesCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("SalesAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("CreditsAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("DiscPaid"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("NetDeposit"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="rgAltRow">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("RefNumber")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("TranType")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("PlanType")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("SalesCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("SalesAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("CreditsAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("DiscPaid"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("NetDeposit"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder3" runat="server">
                                        <tr class="total-fees">
                                            <td class="text-center heading" colspan="4">
                                                <as:Literal ID="Literal52" runat="server" Text="IDINE TOTALS" meta:resourcekey="LiteralIDINETOTALSResource1"></as:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrIDINE_SaleCount" runat="server"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrIDINE_SaleAmount" runat="server"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrIDINE_CreditAmount" runat="server"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrIDINE_DiscountPD" runat="server"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrIDINE_NetDeposit" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>
                </as:Panel>


                <as:Panel ID="pnlBreakLineExportChargeBack" runat="server" Visible="false">
                    <br />
                </as:Panel>
                <!-- CHARGEBACK -->
                <as:Panel ID="pnlChargeBack" runat="server" meta:resourcekey="pnlChargeBackResource1">
                    <div class="row">
                        <div class="col-xs-12">
                            <h2 class="grid-title" id="h3" data-toggle="collapse" data-target="#ChargeBacksID">
                                <as:Literal ID="Literal42" runat="server" Text="Chargebacks" meta:resourcekey="Literal37Resource1"></as:Literal>
                            </h2>
                        </div>
                    </div>
                    <div id="ChargeBacksID" class="in">
                        <table class="ASTable">
                            <tr>
                                <th title="Day">
                                    <as:Literal ID="Literal43" runat="server" Text="Day" meta:resourcekey="Literal38Resource1"></as:Literal>
                                </th>
                                <th title="Reference Number">
                                    <as:Literal ID="Literal44" runat="server" Text="Ref Number" meta:resourcekey="Literal39Resource1"></as:Literal>
                                </th>
                                <th title="Tran Code">
                                    <as:Literal ID="Literal45" runat="server" Text="Tran Code" meta:resourcekey="Literal40Resource1"></as:Literal>
                                </th>
                                <th title="Plan Code">
                                    <as:Literal ID="Literal46" runat="server" Text="Plan Code" meta:resourcekey="Literal41Resource1"></as:Literal>
                                </th>
                                <th title="Sales Count">
                                    <as:Literal ID="Literal47" runat="server" Text="# Sales" meta:resourcekey="Literal42Resource1"></as:Literal>
                                </th>
                                <th title="Sales Amount">
                                    <as:Literal ID="Literal48" runat="server" Text="$ Sales" meta:resourcekey="Literal43Resource1"></as:Literal>
                                </th>
                                <th title="Credits Amount">
                                    <as:Literal ID="Literal49" runat="server" Text="$ Credits" meta:resourcekey="Literal44Resource1"></as:Literal>
                                </th>
                                <th title="Discount Paid">
                                    <as:Literal ID="Literal50" runat="server" Text="Discount PD" meta:resourcekey="Literal45Resource1"></as:Literal>
                                </th>
                                <th title="Net Deposit">
                                    <as:Literal ID="Literal51" runat="server" Text="Net Deposit" meta:resourcekey="Literal46Resource1"></as:Literal>
                                </th>
                            </tr>
                            <as:ASRepeater ID="uxChargebacks" runat="server" OnItemDataBound="uxChargebacks_ItemDataBound"
                                NumberOfColumns="9">
                                <ItemTemplate>
                                    <tr class="rgRow">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("ReferenceNumber")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("TranType")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("PlanType")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("SalesCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("SalesAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("CreditsAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("DiscountPaid"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("NetDeposit"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="rgAltRow">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("ReferenceNumber")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("TranType")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("PlanType")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("SalesCount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("SalesAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("CreditsAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("DiscountPaid"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("NetDeposit"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder3" runat="server">
                                        <tr class="total-fees">
                                            <td class="text-center heading" colspan="4">
                                                <as:Literal ID="Literal52" runat="server" Text="CHARGEBACK TOTALS" meta:resourcekey="Literal46Resource2"></as:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrChargeback_SaleCount" runat="server" meta:resourcekey="uxLtrChargeback_SaleCountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrChargeback_SaleAmount" runat="server" meta:resourcekey="uxLtrChargeback_SaleAmountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrChargeback_CreditAmount" runat="server" meta:resourcekey="uxLtrChargeback_CreditAmountResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrChargeback_DiscountPD" runat="server" meta:resourcekey="uxLtrChargeback_DiscountPDResource1"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrChargeback_NetDeposit" runat="server" meta:resourcekey="uxLtrChargeback_NetDepositResource1"></asp:Literal>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>
                </as:Panel>

                <!-- BACKUP WITHHOLDING -->
                <as:Panel ID="pnlBreakLineExportBACKUPWITHHOLDING" runat="server" Visible="false">
                    <br />
                </as:Panel>
                <as:Panel ID="pnlBACKUPWITHHOLDING" runat="server" meta:resourcekey="pnlBACKUPWITHHOLDINGResource">
                    <div class="row">
                        <div class="col-xs-12">
                            <h2 class="grid-title" id="h6" data-toggle="collapse" data-target="#BackupwithholdingId">
                                <as:Literal ID="Literal138" runat="server" Text="Backup Withholdings" meta:resourcekey="LiteralResourceBackupWithHoldings"></as:Literal>

                            </h2>
                            <h4 class="grid-title">
                                <as:Literal ID="Literal144" runat="server" Text="Details" meta:resourcekey="LiteralResourceDetail"></as:Literal>
                            </h4>
                        </div>
                    </div>
                    <div id="BackupwithholdingId" class="in">
                        <table class="ASTable">
                            <tr>
                                <th title="Day">
                                    <as:Literal ID="Literal139" runat="server" Text="Day" meta:resourcekey="Literal38Resource1"></as:Literal>
                                </th>
                                <th title="Fed Withholding">
                                    <as:Literal ID="Literal140" runat="server" Text="Fed Withholding" meta:resourcekey="LiteralFEDWITHHOLDINGResource1"></as:Literal>
                                </th>
                                <th title="State Withholding">
                                    <as:Literal ID="Literal141" runat="server" Text="State Withholding" meta:resourcekey="LiteralSTATEWITHHOLDINGResource1"></as:Literal>
                                </th>
                                <th title="Fed Deferment">
                                    <as:Literal ID="Literal142" runat="server" Text="Fed Deferment" meta:resourcekey="LiteralFEDDEFERMENTResource1"></as:Literal>
                                </th>
                                <th title="State Deferment">
                                    <as:Literal ID="Literal143" runat="server" Text="State Deferment" meta:resourcekey="LiteralSTATEDEFERMENTResource1"></as:Literal>
                                </th>
                            </tr>
                            <as:ASRepeater ID="uxBackupwithholdingDetail" runat="server"
                                NumberOfColumns="9">
                                <ItemTemplate>
                                    <tr class="rgRow">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("FedWithholding")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("StateWithholding")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("FedDeferment")%>
                                        </td>
                                        <td class="text-right">
                                            <%# (Eval("StateDeferment"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="rgAltRow">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("FedWithholding")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("StateWithholding")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("FedDeferment")%>
                                        </td>
                                        <td class="text-right">
                                            <%# (Eval("StateDeferment"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>
                    <!-- -->
                    <div class="row">
                        <div class="col-xs-12">
                            <h4 class="grid-title" id="h7" data-toggle="collapse" data-target="#BackupwithholdingId">
                                <span>
                                    <as:Literal ID="Literal146" runat="server" Text="Monthly" meta:resourcekey="LiteralResourceMonthly"></as:Literal>
                                </span>
                            </h4>
                        </div>
                    </div>
                    <div id="Div1" class="in">
                        <table class="ASTable">
                            <tr>
                                <th title="Fed Withholding">
                                    <as:Literal ID="Literal148" runat="server" Text="Fed Withholding" meta:resourcekey="LiteralFEDWITHHOLDINGResource1"></as:Literal>
                                </th>
                                <th title="State Withholding">
                                    <as:Literal ID="Literal149" runat="server" Text="State Withholding" meta:resourcekey="LiteralSTATEWITHHOLDINGResource1"></as:Literal>
                                </th>
                                <th title="Fed Deferment">
                                    <as:Literal ID="Literal150" runat="server" Text="Fed Deferment" meta:resourcekey="LiteralFEDDEFERMENTResource1"></as:Literal>
                                </th>
                                <th title="State Deferment">
                                    <as:Literal ID="Literal151" runat="server" Text="State Deferment" meta:resourcekey="LiteralSTATEDEFERMENTResource1"></as:Literal>
                                </th>
                            </tr>
                            <as:ASRepeater ID="uxBackupwithholdingMonthly" runat="server"
                                NumberOfColumns="9">
                                <ItemTemplate>
                                    <tr class="rgRow">
                                        <td class="text-center">
                                            <%# Eval("FedWithholdingMonthly")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("StateWithholdingMonthly")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("FedDefermentMonthly")%>
                                        </td>
                                        <td class="text-right">
                                            <%# (Eval("StateDefermentMonthly"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="rgAltRow">
                                        <td class="text-center">
                                            <%# Eval("FedWithholdingMonthly")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("StateWithholdingMonthly")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("FedDefermentMonthly")%>
                                        </td>
                                        <td class="text-right">
                                            <%# (Eval("StateDefermentMonthly"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>

                    <!-- -->
                    <div class="row">
                        <div class="col-xs-12">
                            <h4 class="grid-title">
                                <as:Literal ID="Literal145" runat="server" Text="Overall" meta:resourcekey="LiteralResourceOverall"></as:Literal>
                            </h4>
                        </div>
                    </div>

                    <div id="Div2" class="in">
                        <table class="ASTable">
                            <tr>
                                <th title="Fed Withholding">
                                    <as:Literal ID="Literal147" runat="server" Text="Fed Withholding" meta:resourcekey="LiteralFEDWITHHOLDINGResource1"></as:Literal>
                                </th>
                                <th title="State Withholding">
                                    <as:Literal ID="Literal152" runat="server" Text="State Withholding" meta:resourcekey="LiteralSTATEWITHHOLDINGResource1"></as:Literal>
                                </th>
                                <th title="Fed Deferment">
                                    <as:Literal ID="Literal153" runat="server" Text="Fed Deferment" meta:resourcekey="LiteralFEDDEFERMENTResource1"></as:Literal>
                                </th>
                                <th title="State Deferment">
                                    <as:Literal ID="Literal154" runat="server" Text="State Deferment" meta:resourcekey="LiteralSTATEDEFERMENTResource1"></as:Literal>
                                </th>
                            </tr>
                            <as:ASRepeater ID="uxBackupwithholdingOverall" runat="server"
                                NumberOfColumns="9">
                                <ItemTemplate>
                                    <tr class="rgRow">
                                        <td class="text-center">
                                            <%# Eval("FedWithholdingOverall")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("StateWithholdingOverall")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("FedDefermentOverall")%>
                                        </td>
                                        <td class="text-right">
                                            <%# (Eval("StateDefermentOverall"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="rgAltRow">
                                        <td class="text-center">
                                            <%# Eval("FedWithholdingOverall")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("StateWithholdingOverall")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("FedDefermentOverall")%>
                                        </td>
                                        <td class="text-right">
                                            <%# (Eval("StateDefermentOverall"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>

                    <!-- -->
                </as:Panel>


                <as:Panel ID="pnlBreakLineExportCashAdvance" runat="server" Visible="false">
                    <br />
                </as:Panel>
                <!-- CashAdvance -->
                <as:Panel ID="pnlCashAdvance" runat="server" meta:resourcekey="pnlCashAdvanceResource1">
                    <div class="row">
                        <div class="col-xs-12">
                            <h2 class="grid-title" id="h9" data-toggle="collapse" data-target="#CashAdvance">
                                <as:Literal ID="Literal155" runat="server" Text="Cash Advance" meta:resourcekey="LiteralCashAdvanceResource"></as:Literal>
                            </h2>
                        </div>
                    </div>
                    <div id="CashAdvance" class="in">
                        <table class="ASTable">
                            <colgroup>
                                <col class="w-20" />
                                <col class="w-20" />
                                <col class="w-20" />
                                <col class="w-20" />
                                <col class="w-20" />
                            </colgroup>
                            <tr>
                                <th title="Vender ID">
                                    <as:Literal ID="Literal156" runat="server" Text="Vendor ID" meta:resourcekey="LiteralVenderIDResource1"></as:Literal>
                                </th>
                                <th title="Day">
                                    <as:Literal ID="Literal157" runat="server" Text="Day" meta:resourcekey="Literal85Resource1"></as:Literal>
                                </th>
                                <th title="$ Deposit amount">
                                    <as:Literal ID="Literal158" runat="server" Text="$ Deposit amount" meta:resourcekey="LiteralDepositAmountResource1"></as:Literal>
                                </th>
                                <th title="Rate">
                                    <as:Literal ID="Literal159" runat="server" Text="Rate" meta:resourcekey="LiteralRateResource1"></as:Literal>
                                </th>
                                <th title="$ Amount Deducted">
                                    <as:Literal ID="Literal160" runat="server" Text="$ Amount Deducted" meta:resourcekey="LiteralAmountDeductedResource1"></as:Literal>
                                </th>
                            </tr>
                            <as:ASRepeater ID="uxCashAdvance" runat="server" OnItemDataBound="uxCashAdvance_ItemDataBound"
                                NumberOfColumns="5">
                                <ItemTemplate>
                                    <tr class="rgRow">
                                        <td class="text-center">
                                            <%# Eval("VendorID")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("DepositAmount")%>
                                        </td>
                                        <td class="text-right">
                                            <%# (Eval("Rate"))%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("AmountDeducted")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="rgAltRow">
                                        <td class="text-center">
                                            <%# Eval("VendorID")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("DepositAmount")%>
                                        </td>
                                        <td class="text-right">
                                            <%# (Eval("Rate"))%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("AmountDeducted")%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder3" runat="server">
                                        <tr class="total-fees">
                                            <td class="text-center heading" colspan="2">
                                                <as:Literal ID="Literal52" runat="server" Text="CASH ADVANCE TOTALS" meta:resourcekey="LiteralCASHADVANCETOTALSResource1"></as:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxCashAdvance_DepositAmount" runat="server"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxCashAdvance_Rate" runat="server"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxCashAdvance_AmountDeducted" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>
                </as:Panel>


                <as:Panel ID="pnlReserveFundExport" runat="server" Visible="false">
                    <br />
                </as:Panel>
                <!-- Reserve Fund -->
                <as:Panel ID="pnlReserveFund" runat="server" meta:resourcekey="pnlReserveFundResource1">
                    <div class="row">
                        <div class="col-xs-12">
                            <h2 class="grid-title" id="h10" data-toggle="collapse" data-target="#ReserveFund">
                                <as:Literal ID="Literal161" runat="server" Text="Reserve Fund" meta:resourcekey="LiteralReserveFundResource"></as:Literal>
                            </h2>
                        </div>
                    </div>
                    <div id="ReserveFund" class="in">
                        <table class="ASTable">
                            <colgroup>
                                <col class="w-10" />
                                <col class="w-25" />
                                <col class="w-40" />
                                <col class="w-25" />
                            </colgroup>
                            <tr>
                                <th title="Day">
                                    <as:Literal ID="Literal163" runat="server" Text="Day" meta:resourcekey="LiteralDayResource1"></as:Literal>
                                </th>
                                <th title="Amount Reserved">
                                    <as:Literal ID="Literal164" runat="server" Text="Amount Reserved" meta:resourcekey="LiteralAmountReservedResource1"></as:Literal>
                                </th>
                                <th title="Amount Released">
                                    <as:Literal ID="Literal165" runat="server" Text="Amount Released" meta:resourcekey="LiteralAmountReleasedResource1"></as:Literal>
                                </th>
                                <th title="Released Balance">
                                    <as:Literal ID="Literal166" runat="server" Text="Released Balance" meta:resourcekey="LiteralReleasedBalanceResource1"></as:Literal>
                                </th>
                            </tr>
                            <as:ASRepeater ID="uxReserveFund" runat="server" OnItemDataBound="uxReserveFund_ItemDataBound"
                                NumberOfColumns="9">
                                <ItemTemplate>
                                    <tr class="rgRow">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("AmountReserved")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("AmountRelease")%>
                                        </td>
                                        <td class="text-right">
                                            <%# (Eval("ReserveBalance"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="rgAltRow">
                                        <td class="text-center">
                                            <%# Eval("Day")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("AmountReserved")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("AmountRelease")%>
                                        </td>
                                        <td class="text-right">
                                            <%# (Eval("ReserveBalance"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder3" runat="server">
                                        <tr class="total-fees">
                                            <td class="text-center heading" colspan="2">
                                                <as:Literal ID="Literal52" runat="server" Text="RESERVE FUND TOTALS" meta:resourcekey="LiteralReserveFundTOTALSResource1"></as:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxReserveFund_AmountReserved" runat="server"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxReserveFund_AmountReleased" runat="server"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxReserveFund_ReleasedBalance" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>
                </as:Panel>


                <as:Panel ID="pnlBreakLineExportFees" runat="server" Visible="true">
                    <br />
                </as:Panel>
                <!-- FEES -->
                <as:Panel ID="pnlFees" runat="server" meta:resourcekey="pnlFeesResource1">
                    <div class="row">
                        <div class="col-xs-12">
                            <h2 class="grid-title" id="h4" data-toggle="collapse" data-target="#FeesID">
                                <as:Literal ID="Literal53" runat="server" Text="Fees" meta:resourcekey="Literal47Resource1"></as:Literal>
                            </h2>
                        </div>
                    </div>
                    <div id="FeesID" class="in">
                        <table class="ASTable">
                            <colgroup>
                                <col class="w-10" />
                                <col class="w-10" />
                                <col class="w-10" />
                                <col class="w-10" />
                                <col class="w-40" />
                                <col class="w-10" />
                                <col class="w-10" />
                            </colgroup>
                            <tr>
                                <th title="Count">
                                    <as:Literal ID="Literal54" runat="server" Text="Count" meta:resourcekey="LiteralCountResource1"></as:Literal>
                                </th>
                                <th title="$ Amount">
                                    <as:Literal ID="Literal55" runat="server" Text="$ Amount" meta:resourcekey="LiteralDAmountResource1"></as:Literal>
                                </th>
                                <th title="Rate">
                                    <as:Literal ID="Literal168" runat="server" Text="Rate %" meta:resourcekey="LiteralRatePerResource1"></as:Literal>
                                </th>
                                <th title="Rate Per Item">
                                    <as:Literal ID="Literal167" runat="server" Text="Rate Per Item" meta:resourcekey="LiteralRatePerItemResource1"></as:Literal>
                                </th>
                                <th title="Description">
                                    <as:Literal ID="Literal56" runat="server" Text="Description" meta:resourcekey="Literal50Resource1"></as:Literal>
                                </th>
                                <th title="Fees Paid">
                                    <as:Literal ID="Literal162" runat="server" Text="Fees Paid" meta:resourcekey="LiteralFeesPaidResource1"></as:Literal>
                                </th>
                                <th title="Total">
                                    <as:Literal ID="Literal57" runat="server" Text="Total" meta:resourcekey="Literal51Resource1"></as:Literal>
                                </th>
                            </tr>
                            <tr class="box-add-note">
                                <td colspan="7" class="text-center heading">
                                    <as:Literal ID="Literal172" runat="server" Text="Auth Fees" meta:resourcekey="LiteralAuthFeesResource1"></as:Literal>
                                </td>
                            </tr>
                            <as:ASRepeater ID="uxAuthFees" runat="server" OnItemDataBound="uxAuthFees_ItemDataBound"
                                NumberOfColumns="7" Visible="true">
                                <ItemTemplate>
                                    <tr class="Row">
                                        <td class="text-center">
                                            <%# FormatInteger(Eval("Count"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePercent")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePerItem")%>
                                        </td>
                                        <td class="text-left">
                                            <%# Eval("Description")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("FeesPaid")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("Total"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <td class="text-center">
                                        <%# FormatInteger(Eval("Count"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("RatePercent")%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("RatePerItem")%>
                                    </td>
                                    <td class="text-left">
                                        <%# Eval("Description")%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("FeesPaid")%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("Total"))%>
                                    </td>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder4" runat="server">
                                        <tr class="total-fees">
                                            <td align="right" class="heading text-right" colspan="6"><b>
                                                <as:Literal ID="Literal179" runat="server" Text="Total Auth Fees" meta:resourcekey="LiteralTotalAuthFeesResource1"></as:Literal></b>
                                            </td>
                                            <td align="right" class="text-right heading">
                                                <b>
                                                    <asp:Literal ID="txtFeeTotalFees" runat="server"></asp:Literal></b>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>
                            <tr class="box-add-note">
                                <td colspan="7" class="text-center heading">
                                    <as:Literal ID="Literal173" runat="server" Text="Interchange Fees" meta:resourcekey="LiteralInterchangeFeesResource1"></as:Literal>
                                </td>
                            </tr>
                            <as:ASRepeater ID="uxInterchangeFees" runat="server" OnItemDataBound="uxInterchangeFees_ItemDataBound"
                                NumberOfColumns="7" Visible="true">
                                <ItemTemplate>
                                    <tr class="Row">
                                        <td class="text-center">
                                            <%# FormatInteger(Eval("Count"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePercent")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePerItem")%>
                                        </td>
                                        <td class="text-left">
                                            <%# Eval("Description")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("FeesPaid")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("Total"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <td class="text-center">
                                        <%# FormatInteger(Eval("Count"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("RatePercent")%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("RatePerItem")%>
                                    </td>
                                    <td class="text-left">
                                        <%# Eval("Description")%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("FeesPaid")%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("Total"))%>
                                    </td>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder4" runat="server">
                                        <tr class="total-fees">
                                            <td align="right" class="heading text-right" colspan="6"><b>
                                                <as:Literal ID="Literal179" runat="server" Text="Total Interchange Fees" meta:resourcekey="LiteralTotalInterchangeFeesResource1"></as:Literal></b>
                                            </td>
                                            <td align="right" class="text-right heading">
                                                <b>
                                                    <asp:Literal ID="txtFeeTotalFees" runat="server"></asp:Literal></b>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>

                            <tr class="box-add-note">
                                <td colspan="7" class="text-center heading">
                                    <as:Literal ID="Literal174" runat="server" Text="Transaction Fees" meta:resourcekey="LiteralTransactionFeesResource1"></as:Literal>
                                </td>
                            </tr>
                            <as:ASRepeater ID="uxTransactionFees" runat="server" OnItemDataBound="uxTransactionFees_ItemDataBound"
                                NumberOfColumns="7" Visible="true">
                                <ItemTemplate>
                                    <tr class="Row">
                                        <td class="text-center">
                                            <%# FormatInteger(Eval("Count"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePercent")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePerItem")%>
                                        </td>
                                        <td class="text-left">
                                            <%# Eval("Description")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("FeesPaid")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("Total"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <td class="text-center">
                                        <%# FormatInteger(Eval("Count"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("RatePercent")%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("RatePerItem")%>
                                    </td>
                                    <td class="text-left">
                                        <%# Eval("Description")%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("FeesPaid")%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("Total"))%>
                                    </td>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder4" runat="server">
                                        <tr class="total-fees">
                                            <td align="right" class="heading text-right" colspan="6"><b>
                                                <as:Literal ID="Literal179" runat="server" Text="Total Transaction Fees" meta:resourcekey="LiteralTotalTransactionFeesResource1"></as:Literal></b>
                                            </td>
                                            <td align="right" class="text-right heading">
                                                <b>
                                                    <asp:Literal ID="txtFeeTotalFees" runat="server"></asp:Literal></b>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>

                            <tr class="box-add-note">
                                <td colspan="7" class="text-center heading">
                                    <as:Literal ID="Literal175" runat="server" Text="Card Brand Fees" meta:resourcekey="LiteralCardBrandFeesResource1"></as:Literal>
                                </td>
                            </tr>
                            <as:ASRepeater ID="uxCardBrandFees" runat="server" OnItemDataBound="uxCardBrandFees_ItemDataBound"
                                NumberOfColumns="7" Visible="true">
                                <ItemTemplate>
                                    <tr class="Row">
                                        <td class="text-center">
                                            <%# FormatInteger(Eval("Count"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePercent")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePerItem")%>
                                        </td>
                                        <td class="text-left">
                                            <%# Eval("Description")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("FeesPaid")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("Total"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <td class="text-center">
                                        <%# FormatInteger(Eval("Count"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("RatePercent")%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("RatePerItem")%>
                                    </td>
                                    <td class="text-left">
                                        <%# Eval("Description")%>
                                    </td>
                                    <td class="text-right">
                                        <%# Eval("FeesPaid")%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("Total"))%>
                                    </td>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder4" runat="server">
                                        <tr class="total-fees">
                                            <td align="right" class="heading text-right " colspan="6"><b>
                                                <as:Literal ID="Literal179" runat="server" Text="Total Card Brand Fees" meta:resourcekey="LiteralTotalCardBrandFeesResource1"></as:Literal></b>
                                            </td>
                                            <td align="right" class="text-right heading">
                                                <b>
                                                    <asp:Literal ID="txtFeeTotalFees" runat="server"></asp:Literal></b>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>

                            <tr class="box-add-note">
                                <td colspan="7" class="text-center heading">
                                    <as:Literal ID="Literal177" runat="server" Text="Other Fees" meta:resourcekey="LiteralOrtherFeesResource1"></as:Literal>
                                </td>
                            </tr>


                            <as:ASRepeater ID="uxOtherFees" runat="server" OnItemDataBound="uxOtherFees_ItemDataBound"
                                NumberOfColumns="7" Visible="true">
                                <ItemTemplate>
                                    <tr class="Row">
                                        <td class="text-center">
                                            <%# FormatInteger(Eval("Count"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePercent")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePerItem")%>
                                        </td>
                                        <td class="text-left">
                                            <%# Eval("Description")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("FeesPaid")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("Total"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AltRow">
                                        <td class="text-center">
                                            <%# FormatInteger(Eval("Count"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePercent")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("RatePerItem")%>
                                        </td>
                                        <td class="text-left">
                                            <%# Eval("Description")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("FeesPaid")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("Total"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder4" runat="server">
                                        <tr class="total-fees">
                                            <td align="right" class="heading text-right " colspan="6"><b>
                                                <as:Literal ID="Literal179" runat="server" Text="Total Other Fees" meta:resourcekey="LiteralTotalOtherFeesResource1"></as:Literal></b>
                                            </td>
                                            <td align="right" class="text-right heading">
                                                <b>
                                                    <asp:Literal ID="txtFeeTotalFees" runat="server"></asp:Literal></b>
                                            </td>
                                        </tr>
                                        <tr class="total-fees">
                                            <td align="right" class="heading text-right" colspan="6"><b>
                                                <as:Literal ID="Literal58" runat="server" Text="TOTAL FEES DUE" meta:resourcekey="Literal51Resource2"></as:Literal></b>
                                            </td>
                                            <td align="right" class="text-right heading">
                                                <b>
                                                    <asp:Literal ID="uxLtrFeesDueTotal" runat="server" meta:resourcekey="uxLtrFees_TotalResource1"></asp:Literal></b>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>
                </as:Panel>

                <!-- Summary-->
                <div class="height-24"></div>
                <table class="TotalBox pull-right">
                    <colgroup>
                        <col />
                        <col />
                        <col />
                        <col />
                        <col />
                        <col />
                        <col class="w-30" />
                    </colgroup>
                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal169" runat="server" Text="DISCOUNT DUE" meta:resourcekey="LiteralDISCOUNTDUEResource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading">
                            <b>
                                <asp:Literal ID="txtFeeDISCOUNTDUE" runat="server"></asp:Literal></b>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal170" runat="server" Text="MIN DISCOUNT DUE" meta:resourcekey="LiteralMINDISCOUNTDUEResource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading">
                            <b>
                                <asp:Literal ID="txtFeeMINDISCOUNTDUE" runat="server"></asp:Literal></b>
                        </td>
                    </tr>


                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal59" runat="server" Text="DISCOUNT PAID" meta:resourcekey="Literal52Resource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading"><b>
                            <asp:Literal ID="uxLtrDiscountPaid" runat="server" meta:resourcekey="uxLtrDiscountPaidResource1"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal60" runat="server" Text="NET DISCOUNT DUE" meta:resourcekey="Literal53Resource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading"><b>
                            <asp:Literal ID="uxLtrNetDiscountDue" runat="server" meta:resourcekey="uxLtrNetDiscountDueResource1"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal61" runat="server" Text="FEES DUE" meta:resourcekey="Literal54Resource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading"><b>
                            <asp:Literal ID="uxLtrFeesDue" runat="server" meta:resourcekey="uxLtrFeesDueResource1"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal62" runat="server" Text="FEES PAID" meta:resourcekey="Literal55Resource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading"><b>
                            <asp:Literal ID="uxLtrFeesPaid" runat="server" meta:resourcekey="uxLtrFeesPaidResource1"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal63" runat="server" Text="NET FEES DUE" meta:resourcekey="Literal56Resource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading"><b>
                            <asp:Literal ID="uxLtrNetFeesDue" runat="server" meta:resourcekey="uxLtrNetFeesDueResource1"></asp:Literal></b>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal176" runat="server" Text="NEXT PROCESSING FEE DUE" meta:resourcekey="LiteralNEXTPROCESSINGFEESDUEResource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading">
                            <b>
                                <asp:Literal ID="txtxFeeNEXTPROCESSINGFEESDUE" runat="server"></asp:Literal></b>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal64" runat="server" Text="AMOUNT DEDUCTED" meta:resourcekey="Literal57Resource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading"><b>
                            <asp:Literal ID="uxLtrAmountDeducted" runat="server" meta:resourcekey="uxLtrAmountDeductedResource1"></asp:Literal></b>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal178" runat="server" Text="AMOUNT DUE" meta:resourcekey="LiteralAMOUNTDUEResource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading">
                            <b>
                                <asp:Literal ID="txtFeeAmountDUE" runat="server"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal171" runat="server" Text="AMOUNT CREDITED" meta:resourcekey="LiteralAMOUNCREDITEDResource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading">
                            <b>
                                <asp:Literal ID="txtFeeAMOUNCREDITED" runat="server"></asp:Literal></b>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>

            <as:PlaceHolder ID="uxExportFooter" runat="server" Visible="false">
              </body>
    </html>
            </as:PlaceHolder>

        </as:PlaceHolder>
        <div class="height-18"></div>
        <div class="row hidden-print">
            <div class="col-md-12 text-right form-action-container">
                <as:Button ID="uxClose" runat="server" Text="Close" CssClass="btn btn-default" OnClientClick="return parent.HidePopupModal();" meta:resourcekey="uxCloseResource1" />
            </div>
        </div>
        <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
            <script type="text/javascript">
                $().ready(function () {
                    if (!$('table#tblSettlementDiscount tr:odd').hasClass('rgNoRecords')) {
                        $('table#tblSettlementDiscount tr:odd').removeClass('rgAltRow').addClass('rgRow');
                        $('table#tblSettlementDiscount tr:even').removeClass('rgAltRow').addClass('rgAltRow');
                    }
                });
            </script>
        </tek:RadCodeBlock>
    </as:ASModalContainer>
</asp:Content>
