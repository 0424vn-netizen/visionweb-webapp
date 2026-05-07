<%@ Page Title="STATEMENT DETAILS" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="StatementDetail_PROCARE.aspx.cs" Inherits="As.VisionWeb.Web.StatementDetail_PROCARE" meta:resourcekey="PageResource1" %>

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
                                        <%# string.IsNullOrEmpty(Eval("RoutingNumber").ToString())? WebSiteConstants.HTML_EM_DASH: Eval("RoutingNumber")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" colspan="3"></td>
                                    <td align="right" colspan="4"></td>
                                    <td align="right" colspan="3" style="padding-right: 20px;">
                                        <as:Literal ID="Literal6" runat="server" Text="Deposit Account Number:" meta:resourcekey="Literal5Resource1"></as:Literal>
                                        <%# string.IsNullOrEmpty(Eval("DepositAcctNumber").ToString())? WebSiteConstants.HTML_EM_DASH:Eval("DepositAcctNumber")%>
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
                                <th title="Transaction Count">
                                    <as:Literal ID="Literal65" runat="server" Text="Transaction Count" meta:resourcekey="Literal115Resource1"></as:Literal>
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
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("TransactionCount"))%>
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
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("TransactionCount"))%>
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
                                            <td class="text-right heading">
                                                <asp:Literal ID="Literal66" runat="server"></asp:Literal>
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
                                <th title="Tran Num">
                                    <as:Literal ID="Literal22" runat="server" Text="Tran Num" meta:resourcekey="Literal19Resource1"></as:Literal>
                                </th>
                                <th title="Batch Amount">
                                    <as:Literal ID="Literal23" runat="server" Text="Batch Amount" meta:resourcekey="Literal20Resource1"></as:Literal>
                                </th>
                                <th title="OcBatch Amount">
                                    <as:Literal ID="Literal24" runat="server" Text="OcBatch Amount" meta:resourcekey="Literal21Resource1"></as:Literal>
                                </th>
                                <th title="Adjust Amount">
                                    <as:Literal ID="Literal25" runat="server" Text="Adjust Amount" meta:resourcekey="Literal22Resource1"></as:Literal>
                                </th>
                                <th title="Charge Back Amount">
                                    <as:Literal ID="Literal26" runat="server" Text="Charge Back Amount" meta:resourcekey="Literal23Resource1"></as:Literal>
                                </th>
                                <th title="Fee Paid">
                                    <as:Literal ID="Literal27" runat="server" Text="FeePaid" meta:resourcekey="Literal24Resource1"></as:Literal>
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
                                            <%# Eval("TranNum")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("BatchAmount")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("OcBatchAmount")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("AdjustAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("ChargeBackAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("FeePaid"))%>
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
                                            <%# Eval("TranNum")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("BatchAmount")%>
                                        </td>
                                        <td class="text-center">
                                            <%# Eval("OcBatchAmount")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatInteger(Eval("AdjustAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("ChargeBackAmount"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("FeePaid"))%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrency(Eval("NetDeposit"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                                        <tr class="total-fees">
                                            <td class="text-center heading" colspan="6">
                                                <as:Literal ID="Literal30" runat="server" Text="DEPOSIT TOTALS" meta:resourcekey="Literal18Resource2"></as:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrDeposit_FeePaid" runat="server"></asp:Literal>
                                            </td>
                                            <td class="text-right heading">
                                                <asp:Literal ID="uxLtrDeposit_NetDeposit" runat="server"></asp:Literal>
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
                                <col class="w-10" />
                                <col class="w-10" />
                            </colgroup>
                            <tr>
                                <th title="Description">
                                    <as:Literal ID="Literal54" runat="server" Text="Description" meta:resourcekey="LiteralCountResource1"></as:Literal>
                                </th>
                                <th title="$ Amount">
                                    <as:Literal ID="Literal55" runat="server" Text="$ Amount" meta:resourcekey="LiteralDAmountResource1"></as:Literal>
                                </th>
                                <th title="Count">
                                    <as:Literal ID="Literal168" runat="server" Text="Count" meta:resourcekey="LiteralRatePerResource1"></as:Literal>
                                </th>
                                <th title="Dis Rate">
                                    <as:Literal ID="Literal167" runat="server" Text="Dis Rate" meta:resourcekey="LiteralRatePerItemResource1"></as:Literal>
                                </th>
                                <th title="Tran Fee">
                                    <as:Literal ID="Literal56" runat="server" Text="Tran Fee" meta:resourcekey="Literal50Resource1"></as:Literal>
                                </th>
                                <th title="Other Fee">
                                    <as:Literal ID="Literal162" runat="server" Text="Other Fee" meta:resourcekey="LiteralFeesPaidResource1"></as:Literal>
                                </th>
                            </tr>

                            <tr class="box-add-note">
                                <td colspan="7" class="text-center heading">
                                    <as:Literal ID="Literal177" runat="server" Text="Other Fees" meta:resourcekey="LiteralOrtherFeesResource1"></as:Literal>
                                </td>
                            </tr>
                            <as:ASRepeater ID="uxOtherFees" runat="server" OnItemDataBound="uxOtherFees_ItemDataBound"
                                NumberOfColumns="7" Visible="true">
                                <ItemTemplate>
                                    <tr class="Row">
                                        <td class="text-left">
                                            <%# Eval("Description")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                        </td>
                                        <td class="text-center">
                                            <%# FormatInteger(Eval("Count"))%>
                                        </td>

                                        <td class="text-right">
                                            <%# Eval("DisRate")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("TranFees")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("OtherFee")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AltRow">
                                        <td class="text-left">
                                            <%# Eval("Description")%>
                                        </td>
                                        <td class="text-right">
                                            <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                        </td>
                                        <td class="text-center">
                                            <%# FormatInteger(Eval("Count"))%>
                                        </td>

                                        <td class="text-right">
                                            <%# Eval("DisRate")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("TranFees")%>
                                        </td>
                                        <td class="text-right">
                                            <%# Eval("OtherFee")%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="PlaceHolder4" runat="server">
                                        <tr class="total-fees">
                                            <td align="right" class="heading text-right " colspan="5"><b>
                                                <as:Literal ID="Literal179" runat="server" Text="Total Other Fees"></as:Literal></b>
                                            </td>
                                            <td align="right" class="text-right heading">

                                                <asp:Literal ID="txtFeeTotalFees" runat="server"></asp:Literal></b>
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
                        <td align="right" class="text-right heading" colspan="6">
                            <as:Literal ID="Literal169" runat="server" Text="Process Fees" meta:resourcekey="LiteralDISCOUNTDUEResource1"></as:Literal>
                        </td>
                        <td align="right" class="text-right heading">
                            <b>
                                <asp:Literal ID="txtFeeDISCOUNTDUE" runat="server"></asp:Literal></b>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" class="text-right heading" colspan="6">
                            <as:Literal ID="Literal170" runat="server" Text="Auth Fees" meta:resourcekey="LiteralMINDISCOUNTDUEResource1"></as:Literal>
                        </td>
                        <td align="right" class="text-right heading">
                            <b>
                                <asp:Literal ID="txtFeeMINDISCOUNTDUE" runat="server"></asp:Literal></b>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" class="text-right heading" colspan="6">
                            <as:Literal ID="Literal60" runat="server" Text="Other Fees" meta:resourcekey="Literal53Resource1"></as:Literal>
                        </td>
                        <td align="right" class="text-right heading"><b>
                            <asp:Literal ID="uxLtrNetDiscountDue" runat="server" meta:resourcekey="uxLtrNetDiscountDueResource1"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="text-right heading" colspan="6"><b>
                            <as:Literal ID="Literal61" runat="server" Text="TOTAL" meta:resourcekey="Literal54Resource1"></as:Literal></b>
                        </td>
                        <td align="right" class="text-right heading"><b>
                            <asp:Literal ID="uxLtrFeesDue" runat="server" meta:resourcekey="uxLtrFeesDueResource1"></asp:Literal></b>
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
