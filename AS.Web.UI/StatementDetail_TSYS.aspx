<%@ Page Title="STATEMENT DETAILS" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="StatementDetail_TSYS.aspx.cs" Inherits="StatementDetail_TSYS" meta:resourcekey="PageResource1" %>

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
        <%--WRFC address information--%>
        <as:Panel ID="uxPnlClientInfo" runat="server" meta:resourcekey="uxPnlClientInfoResource1">
            <div class="row">
                <div class="col-md-12 text-left">
                    <strong>
                        <asp:Literal ID="uxClientInfo" runat="server" meta:resourcekey="uxClientInfoResource1"></asp:Literal>
                    </strong>
                </div>
            </div>
        </as:Panel>

        <asp:PlaceHolder ID="uxExporterPlh" runat="server">
            <as:PlaceHolder ID="uxPlaceHolderStyle" runat="server" Visible="false">
                <html>
                <head>
                    <meta http-equiv="content-type" content="application/xhtml+xml; charset=UTF-8" />
                    <style type="text/css">
                        .ReportTotal {
                            font-weight: bold;
                        }

                        .ASTable th {
                            border-bottom-width: thin;
                            border-bottom-style: solid;
                        }
                    </style>
                </head>
                <body>
            </as:PlaceHolder>
            <div style="text-align: left;">
                <as:Container runat="server" ID="uxFilteringTable" Width="100%" HeaderText="Merchant Mailing Information" meta:resourcekey="uxFilteringTableResource1">
                    <table style="width: 100%">
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
                                <table>
                                    <tr>
                                        <td align="right" colspan="5"><b>
                                            <as:Literal ID="Literal7" runat="server" Text="AMOUNT DEDUCTED FROM ACCOUNT:" meta:resourcekey="Literal5Resource2"></as:Literal>

                                        </b></td>
                                        <td align="left" colspan="5"><b>
                                            <asp:Literal ID="uxAmountDeducted" runat="server" meta:resourcekey="uxAmountDeductedResource1"></asp:Literal></b></td>
                                    </tr>
                                </table>
                                <!-- End -->
                            </td>
                        </tr>
                    </table>
                    <div style="clear: both;">
                    </div>
                </as:Container>
            </div>
            <br />
            <br />

            <!-- STATEMENT MESSAGE -->
            <as:Panel ID="pnlMessage" runat="server" meta:resourcekey="pnlMessageResource1">
                <div style="margin-top: -17px; text-align: center;">
                    <table width="100%" cellpadding="0" cellspacing="0" style="table-layout: auto; empty-cells: show;">
                        <tr>
                            <td colspan="10" rowspan="4">
                                <asp:Literal ID="uxMessage" runat="server" meta:resourcekey="uxMessageResource1"></asp:Literal>
                            </td>
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
                                        <%# FormatInteger(Eval("SaleCount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("SaleAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatInteger(Eval("CreditCount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("CreditAmount"))%>
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
                                        <%# FormatInteger(Eval("SaleCount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("SaleAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatInteger(Eval("CreditCount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("CreditAmount"))%>
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
                                    <tr>
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
                                        <%# FormatInteger(Eval("SaleCount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("SaleAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("CreditAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("DiscountPD"))%>
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
                                        <%# FormatInteger(Eval("SaleCount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("SaleAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("CreditAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("DiscountPD"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("NetDeposit"))%>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                                    <tr>
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
                                        <%# FormatInteger(Eval("SaleCount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("SaleAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("CreditAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("DiscountPD"))%>
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
                                        <%# FormatInteger(Eval("SaleCount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("SaleAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("CreditAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("DiscountPD"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("NetDeposit"))%>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <asp:PlaceHolder ID="PlaceHolder2" runat="server">
                                    <tr>
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
                                        <%# Eval("RefNumber")%>
                                    </td>
                                    <td class="text-center">
                                        <%# Eval("TranType")%>
                                    </td>
                                    <td class="text-center">
                                        <%# Eval("PlanType")%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatInteger(Eval("SaleCount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("SaleAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("CreditAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("DiscountPD"))%>
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
                                        <%# FormatInteger(Eval("SaleCount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("SaleAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("CreditAmount"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("DiscountPD"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("NetDeposit"))%>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <asp:PlaceHolder ID="PlaceHolder3" runat="server">
                                    <tr>
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
            <as:Panel ID="pnlBreakLineExportFees" runat="server" Visible="false">
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
                            <col class="w-25" />
                            <col class="w-40" />
                            <col class="w-25" />
                        </colgroup>
                        <tr>
                            <th title="Number">
                                <as:Literal ID="Literal54" runat="server" Text="Number" meta:resourcekey="Literal48Resource1"></as:Literal>
                            </th>
                            <th title="Amount">
                                <as:Literal ID="Literal55" runat="server" Text="Amount" meta:resourcekey="Literal49Resource1"></as:Literal>
                            </th>
                            <th title="Description">
                                <as:Literal ID="Literal56" runat="server" Text="Description" meta:resourcekey="Literal50Resource1"></as:Literal>
                            </th>
                            <th title="Total">
                                <as:Literal ID="Literal57" runat="server" Text="Total" meta:resourcekey="Literal51Resource1"></as:Literal>
                            </th>
                        </tr>
                        <as:ASRepeater ID="uxFees" runat="server" OnItemDataBound="uxFees_ItemDataBound"
                            NumberOfColumns="4" Visible="true">
                            <ItemTemplate>
                                <tr class="Row">
                                    <td class="text-center">
                                        <%# FormatInteger2Digits(Eval("Number"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                    </td>
                                    <td class="text-left">
                                        <%# Eval("Description")%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("Total"))%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="AltRow">
                                    <td class="text-center">
                                        <%# FormatInteger2Digits(Eval("Number"))%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                    </td>
                                    <td class="text-left">
                                        <%# Eval("Description")%>
                                    </td>
                                    <td class="text-right">
                                        <%# FormatCurrency(Eval("Total"))%>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <asp:PlaceHolder ID="PlaceHolder4" runat="server">
                                    <tr>
                                        <td class="text-center heading" colspan="3"><b>
                                            <as:Literal ID="Literal58" runat="server" Text="TOTAL FEES DUE" meta:resourcekey="Literal51Resource2"></as:Literal></b>
                                        </td>
                                        <td class="text-right heading">
                                            <b>
                                                <asp:Literal ID="uxLtrFees_Total" runat="server" meta:resourcekey="uxLtrFees_TotalResource1"></asp:Literal></b>
                                        </td>
                                    </tr>
                                </asp:PlaceHolder>
                            </FooterTemplate>
                        </as:ASRepeater>
                        <tr id="divNullData" runat="server" visible="false">
                            <td colspan="4">No data found.</td>
                        </tr>
                    </table>
                </div>
            </as:Panel>

            <!-- Summary-->
            <div class="height-24"></div>
            <table class="TotalBox pull-right">
                <tr>
                    <td style="text-align: right;" colspan="3">
                        <as:Literal ID="Literal59" runat="server" Text="DISCOUNT PAID" meta:resourcekey="Literal52Resource1"></as:Literal>
                    </td>
                    <td style="text-align: right; width: 120px">
                        <asp:Literal ID="uxLtrDiscountPaid" runat="server" meta:resourcekey="uxLtrDiscountPaidResource1"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" colspan="3">
                        <as:Literal ID="Literal60" runat="server" Text="NET DISCOUNT DUE" meta:resourcekey="Literal53Resource1"></as:Literal>
                    </td>
                    <td style="text-align: right; width: 120px">
                        <asp:Literal ID="uxLtrNetDiscountDue" runat="server" meta:resourcekey="uxLtrNetDiscountDueResource1"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" colspan="3">
                        <as:Literal ID="Literal61" runat="server" Text="FEES DUE" meta:resourcekey="Literal54Resource1"></as:Literal>
                    </td>
                    <td style="text-align: right; width: 100px">
                        <asp:Literal ID="uxLtrFeesDue" runat="server" meta:resourcekey="uxLtrFeesDueResource1"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" colspan="3">
                        <as:Literal ID="Literal62" runat="server" Text="FEES PAID" meta:resourcekey="Literal55Resource1"></as:Literal>
                    </td>
                    <td style="text-align: right; width: 120px">
                        <asp:Literal ID="uxLtrFeesPaid" runat="server" meta:resourcekey="uxLtrFeesPaidResource1"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" colspan="3">
                        <as:Literal ID="Literal63" runat="server" Text="NET FEES DUE" meta:resourcekey="Literal56Resource1"></as:Literal>
                    </td>
                    <td style="text-align: right; width: 120px">
                        <asp:Literal ID="uxLtrNetFeesDue" runat="server" meta:resourcekey="uxLtrNetFeesDueResource1"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" colspan="3">
                        <as:Literal ID="Literal64" runat="server" Text="AMOUNT DEDUCTED" meta:resourcekey="Literal57Resource1"></as:Literal>
                    </td>
                    <td style="text-align: right; width: 120px">
                        <asp:Literal ID="uxLtrAmountDeducted" runat="server" meta:resourcekey="uxLtrAmountDeductedResource1"></asp:Literal>
                    </td>
                </tr>
            </table>
            </body>
            </html>
        </asp:PlaceHolder>

        <%--Export PDF--%>
        <asp:PlaceHolder ID="uxContentPdf" runat="server" Visible="false">
            <style type="text/css">
                .pdf-exporting {
                    font-size: 8px;
                }

                    .pdf-exporting .text-big {
                        font-size: 10px !important;
                    }

                    .pdf-exporting table.no-border {
                        border: 0px;
                    }

                    .pdf-exporting .title {
                        font-size: 10px;
                    }

                    .pdf-exporting .title-report {
                        font-weight: 700;
                        font-size: 10px;
                    }

                    .pdf-exporting th {
                        background: #676767;
                        color: #fff;
                        font-weight: bold;
                    }

                    .pdf-exporting table {
                        border-collapse: collapse;
                        border: solid 1px #676767;
                        table-layout: auto;
                    }

                    .pdf-exporting .report-table td {
                        border-right: solid 1pt #676767;
                    }

                        .pdf-exporting .report-table td.first-col {
                            border-left: solid 1px #676767;
                        }

                    .pdf-exporting .total-box {
                        float: right;
                        font-weight: 800;
                        font-size: 10px;
                        padding: 10px;
                    }

                    .pdf-exporting .footer {
                        background-color: #CCC;
                        font-weight: bold;
                        border-bottom: 1px solid #676767;
                    }
            </style>
            <div class="pdf-exporting">
                <%-- Logo --%>
                <div style="float: left;" id="Logo" runat="server">
                    <img alt="logo" src="App_Themes/WRFC/images/logo.jpg" runat="server" id="uxImageLogo" />&nbsp;<br />
                </div>
                <br style="clear: both;" />
                <br />
                <%--WRFC address information--%>
                <div style="font-size: 10px;">
                    <asp:Literal ID="uxClientInfoPdf" runat="server" meta:resourcekey="uxClientInfoPdfResource1"></asp:Literal>
                </div>
                <br />
                <%-- Merchant Information --%>
                <table width="100%" cellpadding="3px" class="text-big">
                    <tr>
                        <th colspan="3" class="title" style="text-align: left;">
                            <as:Literal ID="Literal65" runat="server" Text="Merchant Mailing Information" meta:resourcekey="Literal58Resource1"></as:Literal>
                        </th>
                    </tr>
                    <asp:Repeater ID="uxMerchantInfoPdf" runat="server" Visible="true">
                        <ItemTemplate>
                            <tr>
                                <td align="left" valign="top">
                                    <%# Eval("MailingName")%>
                                </td>
                                <td align="right"></td>
                                <td align="right">
                                    <as:Literal ID="Literal66" runat="server" Text="Processing Month:" meta:resourcekey="Literal58Resource2"></as:Literal>
                                    <%# Eval("ProcessingMonth") %>&nbsp;&nbsp;&nbsp;
                                <%# Eval("Bank") %>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <%# FormatAddress(Eval("MailingAddr1"), Eval("MailingAddr2"), Eval("MailingAddr3"), 1)%>
                                </td>
                                <td align="right"></td>
                                <td align="right">
                                    <as:Literal ID="Literal67" runat="server" Text="Association Number:" meta:resourcekey="Literal59Resource1"></as:Literal>
                                    <%# Eval("AssociationNumber")%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <%# FormatAddress(Eval("MailingAddr1"), Eval("MailingAddr2"), Eval("MailingAddr3"), 2)%>
                                </td>
                                <td align="right"></td>
                                <td align="right">
                                    <as:Literal ID="Literal68" runat="server" Text="Merchant ID:" meta:resourcekey="Literal60Resource1"></as:Literal>
                                    <%# Eval("MerchantNumber")%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <%# FormatAddress(Eval("MailingAddr1"), Eval("MailingAddr2"), Eval("MailingAddr3"), 3)%>
                                </td>
                                <td align="right"></td>
                                <td align="right">
                                    <as:Literal ID="Literal69" runat="server" Text="Routing Number:" meta:resourcekey="Literal61Resource1"></as:Literal>
                                    <%# Eval("RoutingNumber")%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top"></td>
                                <td align="right"></td>
                                <td align="right">
                                    <as:Literal ID="Literal70" runat="server" Text="Deposit Account Number:" meta:resourcekey="Literal62Resource1"></as:Literal>
                                    <%# Eval("DepositAcctNumber")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td align="center" colspan="3" style="padding-top: 10px;">
                            <b>
                                <as:Literal ID="Literal71" runat="server" Text="AMOUNT DEDUCTED FROM ACCOUNT:" meta:resourcekey="Literal62Resource2"></as:Literal>
                                <asp:Literal ID="uxAmountDeductedPdf" runat="server" meta:resourcekey="uxAmountDeductedPdfResource1"></asp:Literal>
                            </b>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <!-- STATEMENT MESSAGE -->
                <table width="100%" cellpadding="10px" cellspacing="0" class="text-big no-border">
                    <tr>
                        <td style="text-align: center">
                            <asp:Literal ID="uxMessagePdf" runat="server" meta:resourcekey="uxMessagePdfResource1"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <!-- PLAN SUMMARY -->
                <br />
                <br />
                <div style="text-align: center;">
                    <label class="title-report">
                        <b>
                            <as:Literal ID="Literal72" runat="server" Text="PLAN SUMMARY" meta:resourcekey="Literal63Resource1"></as:Literal></b>
                    </label>
                </div>
                <table width="100%" class="report-table" cellpadding="3px" cellspacing="0px" border="0">
                    <tr class="first-col">
                        <th style="text-align: center;" title="Plan Type" class="first-col">
                            <as:Literal ID="Literal73" runat="server" Text="PL" meta:resourcekey="Literal64Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Sales Count">
                            <as:Literal ID="Literal74" runat="server" Text="# SALES" meta:resourcekey="Literal65Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Sales Amount">
                            <as:Literal ID="Literal75" runat="server" Text="$ SALES" meta:resourcekey="Literal66Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Credits Count">
                            <as:Literal ID="Literal76" runat="server" Text="# CREDITS" meta:resourcekey="Literal67Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Credits Amount">
                            <as:Literal ID="Literal77" runat="server" Text="$ CREDITS" meta:resourcekey="Literal68Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Net Sales">
                            <as:Literal ID="Literal78" runat="server" Text="NET SALES" meta:resourcekey="Literal69Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Average Ticket">
                            <as:Literal ID="Literal79" runat="server" Text="AVG TKT" meta:resourcekey="Literal70Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="DISC P/I">
                            <as:Literal ID="Literal80" runat="server" Text="DISC P/I" meta:resourcekey="Literal71Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Percentage">
                            <as:Literal ID="Literal81" runat="server" Text="%" meta:resourcekey="Literal72Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Discount Due">
                            <as:Literal ID="Literal82" runat="server" Text="DISCOUNT DUE" meta:resourcekey="Literal73Resource1"></as:Literal>
                        </th>
                    </tr>
                    <as:ASRepeater ID="uxPlanSummaryPdf" runat="server" OnItemDataBound="uxPlanSummaryPdf_ItemDataBound"
                        NumberOfColumns="8">
                        <ItemTemplate>
                            <tr>
                                <td align="center" class="first-col">
                                    <%# Eval("PlanType")%>
                                </td>
                                <td align="right">
                                    <%# FormatInteger(Eval("SaleCount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("SaleAmount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatInteger(Eval("CreditCount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("CreditAmount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("NetAmount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("AverageTicket"))%>
                                </td>
                                <td align="right">
                                    <%# Eval("Disc_PI")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Percentage")%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("DiscountDue"))%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:PlaceHolder ID="PlaceHolder5" runat="server">
                                <tr class="footer">
                                    <td align="center" class="first-col">**
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrPlan_SaleCount_Pdf" runat="server" meta:resourcekey="uxLtrPlan_SaleCount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrPlan_SaleAmount_Pdf" runat="server" meta:resourcekey="uxLtrPlan_SaleAmount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrPlan_CreditCount_Pdf" runat="server" meta:resourcekey="uxLtrPlan_CreditCount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrPlan_CreditAmount_Pdf" runat="server" meta:resourcekey="uxLtrPlan_CreditAmount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrPlan_NetAmount_Pdf" runat="server" meta:resourcekey="uxLtrPlan_NetAmount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrPlan_AvgTkt_Pdf" runat="server" meta:resourcekey="uxLtrPlan_AvgTkt_PdfResource1"></asp:Literal>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    </td>
                                <td align="right">
                                    <asp:Literal ID="uxLtrPlan_DiscountDue_Pdf" runat="server" meta:resourcekey="uxLtrPlan_DiscountDue_PdfResource1"></asp:Literal>
                                </td>
                                </tr>
                            </asp:PlaceHolder>
                        </FooterTemplate>
                    </as:ASRepeater>
                </table>
                <!-- DEPOSIT -->
                <br />
                <br />
                <div style="text-align: center;">
                    <label class="title-report">
                        <b>
                            <as:Literal ID="Literal83" runat="server" Text="DEPOSITS" meta:resourcekey="Literal74Resource1"></as:Literal></b></label>
                </div>
                <table width="100%" class="report-table" cellpadding="3px" cellspacing="0px" border="0">
                    <tr>
                        <th style="text-align: center;" title="Day" class="first-col">
                            <as:Literal ID="Literal84" runat="server" Text="DAY" meta:resourcekey="Literal75Resource1"></as:Literal>
                        </th>
                        <th style="text-align: left;" title="Reference Number" colspan="2">
                            <as:Literal ID="Literal85" runat="server" Text="REF NUMBER" meta:resourcekey="Literal76Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Tran Code">
                            <as:Literal ID="Literal86" runat="server" Text="TRAN CODE" meta:resourcekey="Literal77Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Plan Code">
                            <as:Literal ID="Literal87" runat="server" Text="PLAN CODE" meta:resourcekey="Literal78Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Sales Count">
                            <as:Literal ID="Literal88" runat="server" Text="# SALES" meta:resourcekey="Literal79Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Sales Amount">
                            <as:Literal ID="Literal89" runat="server" Text="$ SALES" meta:resourcekey="Literal80Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Credits Amount">
                            <as:Literal ID="Literal90" runat="server" Text="$ CREDITS" meta:resourcekey="Literal81Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Discount Paid">
                            <as:Literal ID="Literal91" runat="server" Text="DISCOUNT PD" meta:resourcekey="Literal82Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Net Deposit">
                            <as:Literal ID="Literal92" runat="server" Text="NET DEPOSIT" meta:resourcekey="Literal83Resource1"></as:Literal>
                        </th>
                    </tr>
                    <as:ASRepeater ID="uxDepositPdf" runat="server" OnItemDataBound="uxDepositsPdf_ItemDataBound"
                        NumberOfColumns="8">
                        <ItemTemplate>
                            <tr>
                                <td align="center" class="first-col">
                                    <%# Eval("Day")%>
                                </td>
                                <td align="center" colspan="2">
                                    <%# Eval("RefNumber")%>
                                </td>
                                <td align="center">
                                    <%# Eval("TranType")%>
                                </td>
                                <td align="center">
                                    <%# Eval("PlanType")%>
                                </td>
                                <td align="right">
                                    <%# FormatInteger(Eval("SaleCount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("SaleAmount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("CreditAmount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("DiscountPD"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("NetDeposit"))%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:PlaceHolder ID="PlaceHolder6" runat="server">
                                <tr class="footer">
                                    <td colspan="5" align="center" class="first-col">
                                        <as:Literal ID="Literal93" runat="server" Text="DEPOSIT TOTALS" meta:resourcekey="Literal75Resource2"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrDeposit_SaleCount_Pdf" runat="server" meta:resourcekey="uxLtrDeposit_SaleCount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrDeposit_SaleAmount_Pdf" runat="server" meta:resourcekey="uxLtrDeposit_SaleAmount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrDeposit_CreditAmount_Pdf" runat="server" meta:resourcekey="uxLtrDeposit_CreditAmount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrDeposit_DiscountPD_Pdf" runat="server" meta:resourcekey="uxLtrDeposit_DiscountPD_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrDeposit_NetDeposit_Pdf" runat="server" meta:resourcekey="uxLtrDeposit_NetDeposit_PdfResource1"></asp:Literal>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                        </FooterTemplate>
                    </as:ASRepeater>
                </table>
                <br />
                <br />
                <!-- ADJUSTMENTS -->
                <div style="text-align: center;">
                    <label class="title-report">
                        <b>
                            <as:Literal ID="Literal94" runat="server" Text="ADJUSTMENTS" meta:resourcekey="Literal84Resource1"></as:Literal></b></label>
                </div>
                <table width="100%" class="report-table" cellpadding="3px" cellspacing="0px">
                    <tr>
                        <th style="text-align: center;" title="Day" class="first-col">
                            <as:Literal ID="Literal95" runat="server" Text="DAY" meta:resourcekey="Literal85Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Reference Number">
                            <as:Literal ID="Literal96" runat="server" Text="REF NUMBER" meta:resourcekey="Literal86Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Tran Code">
                            <as:Literal ID="Literal97" runat="server" Text="TRAN CODE" meta:resourcekey="Literal87Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Plan Code">
                            <as:Literal ID="Literal98" runat="server" Text="PLAN CODE" meta:resourcekey="Literal88Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Sales Count">
                            <as:Literal ID="Literal99" runat="server" Text="# SALES" meta:resourcekey="Literal89Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Sales Amount">
                            <as:Literal ID="Literal100" runat="server" Text="$ SALES" meta:resourcekey="Literal90Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Credits Amount">
                            <as:Literal ID="Literal101" runat="server" Text="$ CREDITS" meta:resourcekey="Literal91Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Discount Paid">
                            <as:Literal ID="Literal102" runat="server" Text="DISCOUNT PD" meta:resourcekey="Literal92Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Net Deposit">
                            <as:Literal ID="Literal103" runat="server" Text="NET DEPOSIT" meta:resourcekey="Literal93Resource1"></as:Literal>
                        </th>
                    </tr>
                    <as:ASRepeater ID="uxAdjustmentsPdf" runat="server" OnItemDataBound="uxAdjustmentsPdf_ItemDataBound"
                        NumberOfColumns="9">
                        <ItemTemplate>
                            <tr style="vertical-align: top;">
                                <td align="center" class="first-col">
                                    <%# Eval("Day")%>
                                </td>
                                <td align="center">
                                    <%# Eval("RefNumber")%>
                                </td>
                                <td align="center">
                                    <%# Eval("TranType")%>
                                </td>
                                <td align="center">
                                    <%# Eval("PlanType")%>
                                </td>
                                <td align="right">
                                    <%# FormatInteger(Eval("SaleCount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("SaleAmount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("CreditAmount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("DiscountPD"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("NetDeposit"))%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:PlaceHolder ID="PlaceHolder7" runat="server">
                                <tr style="text-align: left;" class="footer">
                                    <td colspan="4" align="center" class="first-col">
                                        <as:Literal ID="Literal104" runat="server" Text="ADJUSTMENT TOTALS" meta:resourcekey="Literal85Resource2"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrAdjustment_SaleCount_Pdf" runat="server" meta:resourcekey="uxLtrAdjustment_SaleCount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrAdjustment_SaleAmount_Pdf" runat="server" meta:resourcekey="uxLtrAdjustment_SaleAmount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrAdjustment_CreditAmount_Pdf" runat="server" meta:resourcekey="uxLtrAdjustment_CreditAmount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrAdjustment_DiscountPD_Pdf" runat="server" meta:resourcekey="uxLtrAdjustment_DiscountPD_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrAdjustment_NetDeposit_Pdf" runat="server" meta:resourcekey="uxLtrAdjustment_NetDeposit_PdfResource1"></asp:Literal>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                        </FooterTemplate>
                    </as:ASRepeater>
                </table>

                <br />
                <br />
                <!-- CHARGEBACK -->
                <div style="text-align: center;">
                    <label class="title-report">
                        <b>
                            <as:Literal ID="Literal105" runat="server" Text="CHARGEBACKS" meta:resourcekey="Literal94Resource1"></as:Literal></b></label>
                </div>
                <table width="100%" class="report-table" cellpadding="3px" cellspacing="0px">
                    <tr>
                        <th style="text-align: center;" title="Day" class="first-col">
                            <as:Literal ID="Literal106" runat="server" Text="DAY" meta:resourcekey="Literal95Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Reference Number">
                            <as:Literal ID="Literal107" runat="server" Text="REF NUMBER" meta:resourcekey="Literal96Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Tran Code">
                            <as:Literal ID="Literal108" runat="server" Text="TRAN CODE" meta:resourcekey="Literal97Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Plan Code">
                            <as:Literal ID="Literal109" runat="server" Text="PLAN CODE" meta:resourcekey="Literal98Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Sales Count">
                            <as:Literal ID="Literal110" runat="server" Text="# SALES" meta:resourcekey="Literal99Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Sales Amount">
                            <as:Literal ID="Literal111" runat="server" Text="$ SALES" meta:resourcekey="Literal100Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Credits Amount">
                            <as:Literal ID="Literal112" runat="server" Text="$ CREDITS" meta:resourcekey="Literal101Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Discount Paid">
                            <as:Literal ID="Literal113" runat="server" Text="DISCOUNT PD" meta:resourcekey="Literal102Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Net Deposit">
                            <as:Literal ID="Literal114" runat="server" Text="NET DEPOSIT" meta:resourcekey="Literal103Resource1"></as:Literal>
                        </th>
                    </tr>
                    <as:ASRepeater ID="uxChargebacksPdf" runat="server" OnItemDataBound="uxChargebacksPdf_ItemDataBound"
                        NumberOfColumns="9">
                        <ItemTemplate>
                            <tr style="vertical-align: top;">
                                <td align="center" class="first-col">
                                    <%# Eval("Day")%>
                                </td>
                                <td align="center">
                                    <%# Eval("RefNumber")%>
                                </td>
                                <td align="center">
                                    <%# Eval("TranType")%>
                                </td>
                                <td align="center">
                                    <%# Eval("PlanType")%>
                                </td>
                                <td align="right">
                                    <%# FormatInteger(Eval("SaleCount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("SaleAmount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("CreditAmount"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("DiscountPD"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("NetDeposit"))%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:PlaceHolder ID="PlaceHolder8" runat="server">
                                <tr style="text-align: left;" class="footer">
                                    <td colspan="4" align="center" class="first-col">
                                        <as:Literal ID="Literal115" runat="server" Text="CHARGEBACK TOTALS" meta:resourcekey="Literal95Resource2"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrChargeback_SaleCount_Pdf" runat="server" meta:resourcekey="uxLtrChargeback_SaleCount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrChargeback_SaleAmount_Pdf" runat="server" meta:resourcekey="uxLtrChargeback_SaleAmount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrChargeback_CreditAmount_Pdf" runat="server" meta:resourcekey="uxLtrChargeback_CreditAmount_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrChargeback_DiscountPD_Pdf" runat="server" meta:resourcekey="uxLtrChargeback_DiscountPD_PdfResource1"></asp:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrChargeback_NetDeposit_Pdf" runat="server" meta:resourcekey="uxLtrChargeback_NetDeposit_PdfResource1"></asp:Literal>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                        </FooterTemplate>
                    </as:ASRepeater>
                </table>
                <br />
                <br />
                <!-- FEES -->
                <div style="text-align: center;">
                    <label class="title-report">
                        <b>
                            <as:Literal ID="Literal116" runat="server" Text="FEES" meta:resourcekey="Literal104Resource1"></as:Literal></b>
                    </label>
                </div>
                <table width="100%" class="report-table" cellpadding="3px" cellspacing="0px">
                    <tr>
                        <th style="text-align: center;" title="Number" class="first-col">
                            <as:Literal ID="Literal117" runat="server" Text="NUMBER" meta:resourcekey="Literal105Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Amount">
                            <as:Literal ID="Literal118" runat="server" Text="AMOUNT" meta:resourcekey="Literal106Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Description">
                            <as:Literal ID="Literal119" runat="server" Text="DESCRIPTION" meta:resourcekey="Literal107Resource1"></as:Literal>
                        </th>
                        <th style="text-align: center;" title="Total">
                            <as:Literal ID="Literal120" runat="server" Text="TOTAL" meta:resourcekey="Literal108Resource1"></as:Literal>
                        </th>
                    </tr>
                    <as:ASRepeater ID="uxFeesPdf" runat="server" OnItemDataBound="uxFeesPdf_ItemDataBound"
                        NumberOfColumns="4" Visible="true">
                        <ItemTemplate>
                            <tr style="vertical-align: top;">
                                <td align="center" class="first-col">
                                    <%# FormatInteger2Digits(Eval("Number"))%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrencyHasZeroValue(Eval("Amount"))%>
                                </td>
                                <td align="left">
                                    <%# Eval("Description")%>
                                </td>
                                <td align="right">
                                    <%# FormatCurrency(Eval("Total"))%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:PlaceHolder ID="PlaceHolder9" runat="server">
                                <tr style="text-align: left;" class="footer">
                                    <td colspan="3" class="first-col">
                                        <as:Literal ID="Literal121" runat="server" Text="TOTAL FEES DUE" meta:resourcekey="Literal108Resource2"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <asp:Literal ID="uxLtrFees_Total_Pdf" runat="server" meta:resourcekey="uxLtrFees_Total_PdfResource1"></asp:Literal>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                        </FooterTemplate>
                    </as:ASRepeater>
                    <tr id="divNullData_pdf" runat="server" visible="false">
                        <td colspan="4">No data found.</td>
                    </tr>
                </table>
                <!-- Summary-->
                <br />
                <br />
                <table class="total-box no-border" cellpadding="3px" cellspacing="0px">
                    <tr>
                        <td style="text-align: right;">
                            <as:Literal ID="Literal122" runat="server" Text="DISCOUNT PAID" meta:resourcekey="Literal109Resource1"></as:Literal>
                        </td>
                        <td style="text-align: right; padding-left: 20px;">
                            <asp:Literal ID="uxLtrDiscountPaidPdf" runat="server" meta:resourcekey="uxLtrDiscountPaidPdfResource1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <as:Literal ID="Literal123" runat="server" Text="NET DISCOUNT DUE" meta:resourcekey="Literal110Resource1"></as:Literal>
                        </td>
                        <td style="text-align: right; padding-left: 20px;">
                            <asp:Literal ID="uxLtrNetDiscountDuePdf" runat="server" meta:resourcekey="uxLtrNetDiscountDuePdfResource1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <as:Literal ID="Literal124" runat="server" Text="FEES DUE" meta:resourcekey="Literal111Resource1"></as:Literal>
                        </td>
                        <td style="text-align: right; padding-left: 20px;">
                            <asp:Literal ID="uxLtrFeesDuePdf" runat="server" meta:resourcekey="uxLtrFeesDuePdfResource1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <as:Literal ID="Literal125" runat="server" Text="FEES PAID" meta:resourcekey="Literal112Resource1"></as:Literal>
                        </td>
                        <td style="text-align: right; padding-left: 20px;">
                            <asp:Literal ID="uxLtrFeesPaidPdf" runat="server" meta:resourcekey="uxLtrFeesPaidPdfResource1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <as:Literal ID="Literal126" runat="server" Text="NET FEES DUE" meta:resourcekey="Literal113Resource1"></as:Literal>
                        </td>
                        <td style="text-align: right; padding-left: 20px;">
                            <asp:Literal ID="uxLtrNetFeesDuePdf" runat="server" meta:resourcekey="uxLtrNetFeesDuePdfResource1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <as:Literal ID="Literal127" runat="server" Text="AMOUNT DEDUCTED" meta:resourcekey="Literal114Resource1"></as:Literal>
                        </td>
                        <td style="text-align: right; padding-left: 20px;">
                            <asp:Literal ID="uxLtrAmountDeductedPdf" runat="server" meta:resourcekey="uxLtrAmountDeductedPdfResource1"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:PlaceHolder>
        <%--EndExport PDF--%>
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
