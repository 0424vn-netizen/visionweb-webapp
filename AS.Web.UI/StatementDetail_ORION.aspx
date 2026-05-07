<%@ Page Title="STATEMENT DETAILS" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="StatementDetail_ORION.aspx.cs" Inherits="StatementDetail_ORION" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagPrefix="as" Namespace="AS.Controls.Global" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-10 text-left">
                <uc:PageTitle ID="uxReportTitle" runat="server" ReportTitle="<img src='App_Themes/Orion/images/logo.jpg' alt='logo' />" meta:resourcekey="uxReportTitleResource1" />
            </div>
            <div class="col-md-2 hidden-print">
                <div class="report-export-no-title pull-right on-top">
                    <span class="export-separator">|</span><a href="#" title="Printer Friendly Version" onclick="PrintStatementPage()"><as:Literal ID="ltPRINT" runat="server" Text="PRINT" meta:resourcekey="ltPRINTResource1"></as:Literal></a>
                </div>
                <div class="report-export-no-title dropdown on-top pull-right">
                    <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle" id="A1" runat="server"><as:Literal ID="Literal1" runat="server" Text="EXPORT" meta:resourcekey="Literal1Resource1"></as:Literal></a>
                    <ul class="dropdown-menu">
                        <li id="Li1" runat="server">
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="uxExportExcel_Click" meta:resourcekey="LinkButton1Resource1">Excel</asp:LinkButton></li>
                    </ul>
                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="uxExporterPlh" runat="server">
            <as:PlaceHolder ID="uxPlaceHolderStyle" runat="server" Visible="false">
                <style type="text/css">
                    
                    .ASTable th {
                        border-bottom-width: thin;
                        border-bottom-style: solid;
                    }
                    .heading {
                        font-weight: 700;
                    }
                </style>
            </as:PlaceHolder>

            <!-- Merchant Information -->
            <as:Container runat="server" ID="uxFilteringTable" Width="100%">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h5" data-toggle="collapse" data-target="#MerchantInfoID"><as:Literal ID="Literal2" runat="server" Text="Merchant Information" meta:resourcekey="Literal2Resource1"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div id="MerchantInfoID" class="in">
                    <div class="statement-box">
                        <table class="ASTable no-border">
                            <asp:Repeater ID="uxMerchantInfo" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td class="text-left valign-top" colspan="3">
                                            <%#Eval("Hierachy")%><br />
                                            <%=ContactInformation.ClientName %><br />
                                            <%=ContactInformation.Address1%><br />
                                            <%=ContactInformation.Address2%><br />
                                            <%=ContactInformation.Zip%><br />
                                            <%=ContactInformation.Phone%>&nbsp; 
                                        </td>
                                        <td class="text-right" colspan="3"><as:Literal ID="Literal3" runat="server" Text="CREDIT CARD MERCHANT STATEMENT" meta:resourcekey="Literal3Resource1"></as:Literal><br />
                                            <as:Literal ID="Literal4" runat="server" Text="DATE:" meta:resourcekey="Literal4Resource1"></as:Literal>
                                    <%# Eval("ReportDate", "{0:MM/dd/yyyy}") %><br />
                                            <as:Literal ID="Literal56" runat="server" Text="CODES:" meta:resourcekey="Literal56Resource1"></as:Literal>
                                    <%# Eval("HoldSw")%>&nbsp; &nbsp;&nbsp;
                                            <as:Literal ID="Literal57" runat="server" Text="FORM: 9" meta:resourcekey="Literal57Resource1"></as:Literal>
                                            &nbsp;&nbsp;&nbsp;&nbsp;<%#Eval("TransitNumber")%><br />
                                            <as:Literal ID="Literal58" runat="server" Text="MERCHANT:" meta:resourcekey="Literal58Resource1"></as:Literal>
                                    <%# Eval("MerchantNumber")%><br />
                                            <%# Eval("DDANumber")%><br />
                                            <br />
                                            <br />
                                            <%# Eval("Name")%><br />
                                            <%# Eval("Attention")%><br />
                                            <%# Eval("Address1")%>&nbsp;<%# Eval("Address2")%><br /><%# Eval("City")%>&nbsp;<%# Eval("State")%>&nbsp;<%# Eval("Zip")%></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </as:Container>

            <!-- DEPOSIT -->
            <as:Panel ID="pnlDepositLineBreakExport" runat="server" Visible="false">
                <br />
            </as:Panel>
            <as:Panel ID="pnlDeposit" runat="server" meta:resourcekey="pnlDepositResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h2GridTitle" data-toggle="collapse" data-target="#DepositID"><as:Literal ID="Literal4" runat="server" Text="Deposits" meta:resourcekey="Literal4Resource2"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div id="DepositID" class="in">
                    <table class="ASTable">
                        <tr>
                            <th title="Day"><as:Literal ID="Literal5" runat="server" Text="Day" meta:resourcekey="Literal5Resource1"></as:Literal></th>
                            <th title="Reference Number"><as:Literal ID="Literal6" runat="server" Text="Ref No." meta:resourcekey="Literal6Resource1"></as:Literal></th>
                            <th title="Items"><as:Literal ID="Literal7" runat="server" Text="Items" meta:resourcekey="Literal7Resource1"></as:Literal></th>
                            <th title="Sales Amount"><as:Literal ID="Literal8" runat="server" Text="Sales" meta:resourcekey="Literal8Resource1"></as:Literal></th>
                            <th title="Credits Amount"><as:Literal ID="Literal9" runat="server" Text="Credits" meta:resourcekey="Literal9Resource1"></as:Literal></th>
                            <th title="Discount Amount"><as:Literal ID="Literal10" runat="server" Text="Disc" meta:resourcekey="Literal10Resource1"></as:Literal></th>
                            <th title="Non Bank Memo"><as:Literal ID="Literal11" runat="server" Text="Non Bank Memo" meta:resourcekey="Literal11Resource1"></as:Literal></th>
                            <th title="Net Deposit"><as:Literal ID="Literal12" runat="server" Text="Net Deposit" meta:resourcekey="Literal12Resource1"></as:Literal></th>
                        </tr>
                        <as:ASRepeater ID="uxDeposits" runat="server" OnItemDataBound="uxDeposits_ItemDataBound" NumberOfColumns="8">
                            <ItemTemplate>
                                <tr class="Row">
                                    <td class="text-center"><%# FormatDate2(Eval("Day"))%></td>
                                    <td class="text-center"><%# Eval("ReferenceNumber")%></td>
                                    <td class="text-right"><%# FormatInteger(Eval("Items"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Sales"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Credits"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Disc"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("NonBankMemo"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("NetDeposit"))%></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="AltRow">
                                    <td class="text-center"><%# FormatDate2(Eval("Day"))%></td>
                                    <td class="text-center"><%# Eval("ReferenceNumber")%></td>
                                    <td class="text-right"><%# FormatInteger(Eval("Items"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Sales"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Credits"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Disc"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("NonBankMemo"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("NetDeposit"))%></td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <asp:PlaceHolder ID="uxFooter" runat="server">
                                    <tr class="rgFooter">
                                        <td class="heading"><as:Literal ID="Literal10" runat="server" Text="DEPOSIT TOTALS" meta:resourcekey="Literal10Resource2"></as:Literal></td>
                                        <td class="heading">&nbsp;</td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrItems" runat="server" meta:resourcekey="uxLtrItemsResource1"></asp:Literal></span></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrSales" runat="server" meta:resourcekey="uxLtrSalesResource1"></asp:Literal></span></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrCredits" runat="server" meta:resourcekey="uxLtrCreditsResource1"></asp:Literal></span></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrDisc" runat="server" meta:resourcekey="uxLtrDiscResource1"></asp:Literal></span></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrNonBankMemo" runat="server" meta:resourcekey="uxLtrNonBankMemoResource1"></asp:Literal></span></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrNetDeposits" runat="server" meta:resourcekey="uxLtrNetDepositsResource1"></asp:Literal></span></td>
                                    </tr>
                                </asp:PlaceHolder>
                            </FooterTemplate>
                        </as:ASRepeater>
                    </table>
                </div>
            </as:Panel>

            <!-- Deposit Item Sumary -->
            <as:Panel ID="pnlDepositItemLineBreakExport" runat="server" Visible="false">
                <br />
            </as:Panel>
            <as:Panel ID="pnlDepositItem" runat="server" meta:resourcekey="pnlDepositItemResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h1" data-toggle="collapse" data-target="#DepositItemSummaryID"><as:Literal ID="Literal13" runat="server" Text="Deposits Item Summary" meta:resourcekey="Literal13Resource1"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div id="DepositItemSummaryID" class="in">
                    <asp:Repeater ID="uxRepeaterDepositSumary1" runat="server">
                        <ItemTemplate>
                            <table class="w-100">
                                <colgroup>
                                    <col/>
                                    <col class="w-5" />
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td>
                                        <table class="ASTable">
                                            <tr>
                                                <th title="Tickets"><as:Literal ID="Literal13" runat="server" Text="Tickets" meta:resourcekey="Literal13Resource2"></as:Literal>
                                                </th>
                                                <th title="Number"><as:Literal ID="Literal14" runat="server" Text="Number" meta:resourcekey="Literal14Resource1"></as:Literal>
                                                </th>
                                                <th title="Amount"><as:Literal ID="Literal15" runat="server" Text="Amount" meta:resourcekey="Literal15Resource1"></as:Literal>
                                                </th>
                                            </tr>
                                            <tr class="Row">
                                                <td><as:Literal ID="Literal16" runat="server" Text="SALES" meta:resourcekey="Literal16Resource1"></as:Literal>
                                                </td>
                                                <td class="text-right">
                                                    <%# Eval("SaleCount") %>
                                                </td>
                                                <td class="text-right">
                                                    <%# AS.Common.Formater.FormatData.FormatCurrency(Eval("SaleAmount"), SessionManager.CurrencyFortmat)%>
                                                </td>
                                            </tr>
                                            <tr class="AltRow">
                                                <td><as:Literal ID="Literal17" runat="server" Text="CREDITS" meta:resourcekey="Literal17Resource1"></as:Literal>
                                                </td>
                                                <td class="text-right">
                                                    <%# Eval("ReturnCount") %>
                                                </td>
                                                <td class="text-right">
                                                    <%# AS.Common.Formater.FormatData.FormatCurrency(Eval("ReturnAmount"), SessionManager.CurrencyFortmat)%>
                                                </td>
                                            </tr>
                                            <tr class="rgFooter">
                                                <td class="heading"><as:Literal ID="Literal18" runat="server" Text="TOTAL" meta:resourcekey="Literal18Resource1"></as:Literal>
                                                </td>
                                                <td class="text-right heading">

                                                    <span class="heading"><%# SumDepositSummaryNumber(Eval("SaleCount"), Eval("ReturnCount")) %></span>
                                                </td>
                                                <td class="text-right heading">

                                                    <span class="heading"><%# AS.Common.Formater.FormatData.FormatCurrency(SumDepositSummaryAmount(Eval("SaleAmount"), Eval("ReturnAmount")), SessionManager.CurrencyFortmat)%></span>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                    <td></td>
                                    <td>
                                        <table class="ASTable">
                                            <tr>
                                                <th title="Adjustment"><as:Literal ID="Literal19" runat="server" Text="ADJ" meta:resourcekey="Literal19Resource1"></as:Literal>
                                                </th>
                                                <th title="Number"><as:Literal ID="Literal20" runat="server" Text="Number" meta:resourcekey="Literal20Resource1"></as:Literal>
                                                </th>
                                                <th title="Amount"><as:Literal ID="Literal21" runat="server" Text="Amount" meta:resourcekey="Literal21Resource1"></as:Literal>
                                                </th>
                                            </tr>
                                            <tr class="Row">
                                                <td><as:Literal ID="Literal22" runat="server" Text="DB ADJ" meta:resourcekey="Literal22Resource1"></as:Literal>
                                                </td>
                                                <td class="text-right">
                                                    <%# Eval("DebitAdjustCount")%>
                                                </td>
                                                <td class="text-right">
                                                    <%# AS.Common.Formater.FormatData.FormatCurrency(Eval("DebitAdjustAmount"), SessionManager.CurrencyFortmat)%>
                                                </td>
                                            </tr>
                                            <tr class="AltRow">
                                                <td><as:Literal ID="Literal23" runat="server" Text="CR ADJ" meta:resourcekey="Literal23Resource1"></as:Literal>
                                                </td>
                                                <td class="text-right">
                                                    <%# Eval("CreditAdjCount")%>
                                                </td>
                                                <td class="text-right">
                                                    <%# AS.Common.Formater.FormatData.FormatCurrency(Eval("CreditAdjustAMount"), SessionManager.CurrencyFortmat)%>
                                                </td>
                                            </tr>
                                            <tr class="rgFooter">
                                                <td class="heading"><as:Literal ID="Literal24" runat="server" Text="TOTAL" meta:resourcekey="Literal24Resource1"></as:Literal>
                                                </td>
                                                <td class="text-right heading">

                                                    <span class="heading"><%# SumDepositSummaryNumber(Eval("DebitAdjustCount"), Eval("CreditAdjCount")) %></span>
                                                </td>
                                                <td class="text-right heading">

                                                    <span class="heading"><%# AS.Common.Formater.FormatData.FormatCurrency(SumDepositSummaryAmount(Eval("DebitAdjustAmount"), Eval("CreditAdjustAMount")), SessionManager.CurrencyFortmat) %></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </as:Panel>

            <!--Card Sumary-->
            <as:Panel ID="pnlCardSumaryLineBreakExport" runat="server" Visible="false">
                <br />
            </as:Panel>
            <as:Panel ID="pnlCardSumary" runat="server" meta:resourcekey="pnlCardSumaryResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h2" data-toggle="collapse" data-target="#CardSummaryID"><as:Literal ID="Literal24" runat="server" Text="Card Summary" meta:resourcekey="Literal24Resource2"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div id="CardSummaryID" class="in">
                    <table class="ASTable">
                        <tr>
                            <th title="Date"><as:Literal ID="Literal25" runat="server" Text="Date" meta:resourcekey="Literal25Resource1"></as:Literal></th>
                            <th title="Batch Sequence"><as:Literal ID="Literal26" runat="server" Text="Ref" meta:resourcekey="Literal26Resource1"></as:Literal></th>
                            <th title="Mastercard"><as:Literal ID="Literal27" runat="server" Text="Mastercard" meta:resourcekey="Literal27Resource1"></as:Literal></th>
                            <th title="Discover"><as:Literal ID="Literal28" runat="server" Text="Discover" meta:resourcekey="Literal28Resource1"></as:Literal></th>
                            <th title="Visa"><as:Literal ID="Literal29" runat="server" Text="Visa" meta:resourcekey="Literal29Resource1"></as:Literal></th>
                            <th title="Amex"><as:Literal ID="Literal30" runat="server" Text="Amex" meta:resourcekey="Literal30Resource1"></as:Literal></th>
                            <th title="Diners"><as:Literal ID="Literal31" runat="server" Text="Diners" meta:resourcekey="Literal31Resource1"></as:Literal></th>
                            <th title="Others"><as:Literal ID="Literal32" runat="server" Text="Others" meta:resourcekey="Literal32Resource1"></as:Literal></th>
                            <th title="Debit"><as:Literal ID="Literal33" runat="server" Text="Debit" meta:resourcekey="Literal33Resource1"></as:Literal></th>
                        </tr>
                        <as:ASRepeater ID="uxCardSumary" runat="server" OnItemDataBound="uxCardSumary_ItemDataBound" NumberOfColumns="9">
                            <ItemTemplate>
                                <tr class="Row">
                                    <td class="text-center"><%# FormatDate(Eval("PostDate"))%></td>
                                    <td class="text-center"><%# Eval("BatchSequence")%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("MasterCard"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Discover"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Visa"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Amex"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Diners"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Others"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Debit"))%></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="AltRow">
                                    <td class="text-center"><%# FormatDate(Eval("PostDate"))%></td>
                                    <td class="text-center"><%# Eval("BatchSequence")%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("MasterCard"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Discover"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Visa"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Amex"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Diners"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Others"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Debit"))%></td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <asp:PlaceHolder ID="uxFooter" runat="server">
                                    <tr class="rgFooter">
                                        <td class="heading"><as:Literal ID="Literal33" runat="server" Text="TOTAL" meta:resourcekey="Literal33Resource2"></as:Literal></td>
                                        <td class="heading">&nbsp;</td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrMasterCard" runat="server" meta:resourcekey="uxLtrMasterCardResource1"></asp:Literal></span></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrDiscover" runat="server" meta:resourcekey="uxLtrDiscoverResource1"></asp:Literal></span></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrVisa" runat="server" meta:resourcekey="uxLtrVisaResource1"></asp:Literal></span></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrAmex" runat="server" meta:resourcekey="uxLtrAmexResource1"></asp:Literal></span></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrDiners" runat="server" meta:resourcekey="uxLtrDinersResource1"></asp:Literal></span></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrOthers" runat="server" meta:resourcekey="uxLtrOthersResource1"></asp:Literal></span></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrDebit" runat="server" meta:resourcekey="uxLtrDebitResource1"></asp:Literal></span></td>
                                    </tr>
                                </asp:PlaceHolder>
                            </FooterTemplate>
                        </as:ASRepeater>
                    </table>
                </div>
            </as:Panel>

            <!--Settlement Discount-->
            <as:Panel ID="pnlSettlementDiscountLineBreakExport" runat="server" Visible="false">
                <br />
            </as:Panel>
            <as:Panel ID="pnlSettlementDiscount" runat="server" meta:resourcekey="pnlSettlementDiscountResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h3" data-toggle="collapse" data-target="#SettlementDiscountID"><as:Literal ID="Literal34" runat="server" Text="Settlement/Discount" meta:resourcekey="Literal34Resource1"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div id="SettlementDiscountID" class="in">
                    <table class="ASTable">
                        <tr>
                            <th title="Description"><as:Literal ID="Literal35" runat="server" Text="Description" meta:resourcekey="Literal35Resource1"></as:Literal></th>
                            <th title="Items"><as:Literal ID="Literal36" runat="server" Text="Items" meta:resourcekey="Literal36Resource1"></as:Literal></th>
                            <th title="Amount"><as:Literal ID="Literal37" runat="server" Text="Amount" meta:resourcekey="Literal37Resource1"></as:Literal></th>
                            <th title="Average Ticket"><as:Literal ID="Literal38" runat="server" Text="Avg Ticket" meta:resourcekey="Literal38Resource1"></as:Literal></th>
                            <th title="Discount Rate"><as:Literal ID="Literal39" runat="server" Text="Disc Rate" meta:resourcekey="Literal39Resource1"></as:Literal></th>
                            <th title="Item Rate"><as:Literal ID="Literal40" runat="server" Text="Item Rate" meta:resourcekey="Literal40Resource1"></as:Literal></th>
                            <th title="Fee Amount"><as:Literal ID="Literal41" runat="server" Text="Fee Amount" meta:resourcekey="Literal41Resource1"></as:Literal></th>
                        </tr>
                        <as:ASRepeater ID="uxSettlementDiscount" runat="server" OnItemDataBound="uxSettlementDiscount_ItemDataBound" NumberOfColumns="7">
                            <ItemTemplate>
                                <tr class="Row">
                                    <td class="text-left"><%# Eval("Description")%></td>
                                    <td class="text-right"><%# FormatInteger(Eval("Items"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Amount"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("AverageTicket"))%></td>
                                    <td class="text-right"><%# FormatNumber4Digits(Eval("DiscountRate"))%></td>
                                    <td class="text-right"><%# FormatNumber4Digits(Eval("ItemRate"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("FeeAmount"))%></td>
                                </tr>
                                <as:PlaceHolder ID="uxInterCh1" runat="server" Visible='<%# CheckInterChange(Eval("Interchange")) %>'>
                                    <tr class="Row">
                                        <td class="text-left"><as:Literal ID="Literal41" runat="server" Text="INTER-CHG" meta:resourcekey="Literal41Resource4"></as:Literal></td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td class="text-right"><%# FormatCurrency(Eval("Interchange"))%></td>
                                    </tr>
                                </as:PlaceHolder>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="AltRow">
                                    <td class="text-left"><%# Eval("Description")%></td>
                                    <td class="text-right"><%# FormatInteger(Eval("Items"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Amount"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("AverageTicket"))%></td>
                                    <td class="text-right"><%# FormatNumber4Digits(Eval("DiscountRate"))%></td>
                                    <td class="text-right"><%# FormatNumber4Digits(Eval("ItemRate"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("FeeAmount"))%></td>
                                </tr>
                                <as:PlaceHolder ID="uxInterCh2" runat="server" Visible='<%# CheckInterChange(Eval("Interchange")) %>'>
                                    <tr class="AltRow">
                                        <td class="text-left"><as:Literal ID="Literal41" runat="server" Text="INTER-CHG" meta:resourcekey="Literal41Resource2"></as:Literal></td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td class="text-right"><%# FormatCurrency(Eval("Interchange"))%></td>
                                    </tr>
                                </as:PlaceHolder>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <asp:PlaceHolder ID="uxFooter" runat="server">
                                    <tr class="rgFooter">
                                        <td class="heading"><as:Literal ID="Literal41" runat="server" Text="TOTAL" meta:resourcekey="Literal41Resource3"></as:Literal></td>
                                        <td class="heading">&nbsp;</td>
                                        <td class="heading"></td>
                                        <td class="heading"></td>
                                        <td class="heading"></td>
                                        <td class="heading"></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrFeeAmount" runat="server" meta:resourcekey="uxLtrFeeAmountResource1"></asp:Literal></span></td>
                                    </tr>
                                </asp:PlaceHolder>
                            </FooterTemplate>
                        </as:ASRepeater>
                    </table>
                </div>
            </as:Panel>

            <!--Surcharge-->
            <as:Panel ID="pnlSurChargeLineBreakExport" runat="server" Visible="false">
                <br />
            </as:Panel>
            <as:Panel ID="pnlSurcharge" runat="server" meta:resourcekey="pnlSurchargeResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h4" data-toggle="collapse" data-target="#SurchargeID"><as:Literal ID="Literal42" runat="server" Text="Surcharge" meta:resourcekey="Literal42Resource1"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div id="SurchargeID" class="in">
                    <table class="ASTable">
                        <tr>
                            <th title="Rate"><as:Literal ID="Literal43" runat="server" Text="Rate" meta:resourcekey="Literal43Resource1"></as:Literal></th>
                            <th title="Items"><as:Literal ID="Literal44" runat="server" Text="Items" meta:resourcekey="Literal44Resource1"></as:Literal></th>
                            <th title="Volume"><as:Literal ID="Literal45" runat="server" Text="Volume" meta:resourcekey="Literal45Resource1"></as:Literal></th>
                            <th title="Amount"><as:Literal ID="Literal46" runat="server" Text="Amount" meta:resourcekey="Literal46Resource1"></as:Literal></th>
                        </tr>
                        <as:ASRepeater ID="uxSurcharge" runat="server" OnItemDataBound="uxSurcharge_ItemDataBound" NumberOfColumns="4">
                            <ItemTemplate>
                                <tr class="Row">
                                    <td class="text-left"><%# Eval("Description")%></td>
                                    <td class="text-right"><%# FormatInteger(Eval("Items"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Volume"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("SurchargeAmount"))%></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="AltRow">
                                    <td class="text-left"><%# Eval("Description")%></td>
                                    <td class="text-right"><%# FormatInteger(Eval("Items"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Volume"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("SurchargeAmount"))%></td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <asp:PlaceHolder ID="uxFooter" runat="server">
                                    <tr class="rgFooter">
                                        <td class="heading"><as:Literal ID="Literal46" runat="server" Text="TOTAL" meta:resourcekey="Literal46Resource2"></as:Literal></td>
                                        <td class="heading">&nbsp;</td>
                                        <td class="heading"></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrAmount" runat="server" meta:resourcekey="uxLtrAmountResource1"></asp:Literal></span></td>
                                    </tr>
                                </asp:PlaceHolder>
                            </FooterTemplate>
                        </as:ASRepeater>
                    </table>
                </div>
            </as:Panel>

            <!--Others Fees-->
            <as:Panel ID="pnlOtherFeeLineBreakExport" runat="server" Visible="false">
                <br />
            </as:Panel>
            <as:Panel ID="pnlOtherFee" runat="server" meta:resourcekey="pnlOtherFeeResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h6" data-toggle="collapse" data-target="#OtherFeesID"><as:Literal ID="Literal47" runat="server" Text="Other Fees" meta:resourcekey="Literal47Resource1"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div id="OtherFeesID" class="in">
                    <table class="ASTable">
                        <tr>
                            <th title="Card Type"><as:Literal ID="Literal48" runat="server" Text="Card" meta:resourcekey="Literal48Resource1"></as:Literal></th>
                            <th title="Charge Type"><as:Literal ID="Literal49" runat="server" Text="Charge" meta:resourcekey="Literal49Resource1"></as:Literal></th>
                            <th title="Description"><as:Literal ID="Literal50" runat="server" Text="Description" meta:resourcekey="Literal50Resource1"></as:Literal></th>
                            <th title="Number"><as:Literal ID="Literal51" runat="server" Text="Number" meta:resourcekey="Literal51Resource1"></as:Literal></th>
                            <th title="Rate"><as:Literal ID="Literal52" runat="server" Text="Rate" meta:resourcekey="Literal52Resource1"></as:Literal></th>
                            <th title="Amount"><as:Literal ID="Literal53" runat="server" Text="Fees" meta:resourcekey="Literal53Resource1"></as:Literal></th>
                        </tr>
                        <as:ASRepeater ID="uxOtherFee" runat="server" OnItemDataBound="uxOtherFee_ItemDataBound" NumberOfColumns="6">
                            <ItemTemplate>
                                <tr class="Row">
                                    <td class="text-center"><%# Eval("CardTypeCode")%></td>
                                    <td class="text-center"><%# Eval("ChargeType")%></td>
                                    <td class="text-left"><%# Eval("ShortDescription")%></td>
                                    <td class="text-right"><%# FormatInteger(Eval("Tickets"))%></td>
                                    <td class="text-right"><%# FormatNumber4Digits(Eval("Rate"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Amount"))%></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="AltRow">
                                    <td class="text-center"><%# Eval("CardTypeCode")%></td>
                                    <td class="text-center"><%# Eval("ChargeType")%></td>
                                    <td class="text-left"><%# Eval("ShortDescription")%></td>
                                    <td class="text-right"><%# FormatInteger(Eval("Tickets"))%></td>
                                    <td class="text-right"><%# FormatNumber4Digits(Eval("Rate"))%></td>
                                    <td class="text-right"><%# FormatCurrency(Eval("Amount"))%></td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <asp:PlaceHolder ID="uxFooter" runat="server">
                                    <tr class="rgFooter">
                                        <td class="heading"><as:Literal ID="Literal53" runat="server" Text="TOTAL OTHER FEES" meta:resourcekey="Literal53Resource2"></as:Literal></td>
                                        <td class="heading">&nbsp;</td>
                                        <td class="heading"></td>
                                        <td class="heading"></td>
                                        <td class="heading"></td>
                                        <td class="text-right heading">
                                            <span class="heading"><asp:Literal ID="uxLtrAmount" runat="server" meta:resourcekey="uxLtrAmountResource2"></asp:Literal></span></td>
                                    </tr>
                                </asp:PlaceHolder>
                            </FooterTemplate>
                        </as:ASRepeater>
                    </table>
                </div>

            </as:Panel>

            <div class="height-24"></div>
            <div class="statement-box inline-block pull-right">
                <table class="statement-info-table ASTable no-border">
                    <tbody>
                        <as:PlaceHolder ID="uxMinBillAdj" runat="server">
                            <tr>
                                <td><as:Literal ID="Literal54" runat="server" Text="Minimum Billing Adjustment:" meta:resourcekey="Literal54Resource1"></as:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="uxMinBillAdjustment" runat="server" meta:resourcekey="uxMinBillAdjustmentResource1"></asp:Literal>
                                </td>
                            </tr>
                        </as:PlaceHolder>
                        <tr>
                            <td><as:Literal ID="Literal55" runat="server" Text="Your Account Has Been Debited:" meta:resourcekey="Literal55Resource1"></as:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="uxTotalAmount" runat="server" meta:resourcekey="uxTotalAmountResource1"></asp:Literal>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </asp:PlaceHolder>


        <div class="height-18"></div>
        <div class="row hidden-print">
            <div class="col-md-12 text-right form-action-container">
                <as:Button ID="uxClose" runat="server" Text="Close" CssClass="btn btn-default" OnClientClick="return parent.HidePopupModal();" meta:resourcekey="uxCloseResource1" />
            </div>
        </div>

        <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
            <script>
                function PrintStatementPage() {
                    if (document.queryCommandSupported('print')) {
                        document.execCommand('print', false, null);
                    } else {
                        window.focus();
                        if (window.print)
                            try { setTimeout('window.print()', 500); } catch (ex) { }
                        else
                            alert("Sorry, your browser doesn't support this feature.\n Please go to \"File\" and choose the \"Print\" option ");
                    }
                }
            </script>
        </tek:RadCodeBlock>
    </as:ASModalContainer>
</asp:Content>

