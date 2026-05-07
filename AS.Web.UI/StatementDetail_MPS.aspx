<%@ Page Title="STATEMENT DETAILS" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="StatementDetail_MPS.aspx.cs" Inherits="StatementDetail_MPS" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <as:Literal ID="uxHeader" Visible="false" Text="No data to display" runat="server" meta:resourcekey="uxHeaderResource1"></as:Literal>
        <div class="row">
            <div class="col-md-9 text-left">
                <uc:PageTitle ID="uxReportTitle" runat="server" ReportTitle="<img src='App_Themes/MPS/images/logo.png' alt='logo' />" meta:resourcekey="uxReportTitleResource1" />
            </div>
            <div class="col-md-3 text-right">
                <div class="hidden-print" runat="server" id="divExport">
                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/res/Images/printer.jpg" Width="24px"
                        Height="24px" runat="server" ToolTip="Printer Friendly Version" OnClientClick="PrintStatementPage();return false;" meta:resourcekey="ImageButton1Resource1"></asp:ImageButton>
                </div>
            </div>
        </div>
        <as:PlaceHolder runat="server" ID="uxExportPanel">
            <div class="row">
                <div class="col-md-12 in" id="MerchantInfoID">
                    <table class="ASTable">
                        <tr>
                            <td>
                                <table class="ASTable">
                                    <colgroup>
                                        <col />
                                        <col width="300px" />
                                    </colgroup>
                                    <tr class="no-border">
                                        <asp:Repeater ID="uxMerchantInfo" runat="server">
                                            <ItemTemplate>
                                                <td valign="top">
                                                    <table class="ASTable">
                                                        <tr class="no-border">
                                                            <td>
                                                                <%=ContactInformation.Address1 %>
                                                                <%# ContactInformation.Address2==""?"":"<br />" + ContactInformation.Address2+"<br />"%>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td><%=ContactInformation.Zip %></td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td><%=ContactInformation.Phone%></td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td><%#Eval("Hierachy")%></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td valign="top">
                                                    <table class="ASTable">
                                                        <tr class="no-border">
                                                            <td>
                                                                <as:Literal ID="ltSomethingonDetail" runat="server" Text="CREDIT CARD MERCHANT STATEMENT" meta:resourcekey="ltSomethingonDetailResource1"></as:Literal></td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td>
                                                                <as:Literal ID="Literal1" runat="server" Text="DATE:" meta:resourcekey="Literal1Resource1"></as:Literal><%# Eval("ReportDate", "{0:MM/dd/yyyy}")%></td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td>
                                                                <as:Literal ID="Literal2" runat="server" Text="CODES:" meta:resourcekey="Literal2Resource1"></as:Literal><%# Eval("HoldSw")%>&nbsp; &nbsp;&nbsp;FORM: 9&nbsp;&nbsp;&nbsp;&nbsp;<%#Eval("TransitNumber")%></td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td>
                                                                <as:Literal ID="Literal3" runat="server" Text="MERCHANT:" meta:resourcekey="Literal3Resource1"></as:Literal>
                                                                <%# Eval("MerchantNumber")%></td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td>
                                                                <%# Eval("DDANumber")%>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td>
                                                                <%# Eval("Name")%></td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td>
                                                                <%# Eval("Attention")%></td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td>
                                                                <%# Eval("Address1")%>&nbsp;<%# Eval("Address2")%></td>
                                                        </tr>
                                                        <tr class="no-border">
                                                            <td>
                                                                <%# Eval("City")%>&nbsp;<%# Eval("State")%>&nbsp;<%# Eval("Zip")%></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <!-- DEPOSIT -->
            <as:Panel ID="pnlDeposit" runat="server" meta:resourcekey="pnlDepositResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h2GridTitle" data-toggle="collapse" data-target="#DepositID">
                            <as:Literal ID="Literal3" runat="server" Text="Deposits" meta:resourcekey="Literal3Resource2"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div id="DepositID" class="in">
                    <as:ASGrid ID="uxDeposits" FooterStyle-HorizontalAlign="Left" EnableEmbeddedSkins="false" BorderStyle="Solid"
                        ShowFooter="true" AllowPaging="false" AllowSortFilterWhenExport="true" GridName="DEPOSITS" XOverFlowable="false"
                        ASPagingMethod="SPASingleMethod" AutoGenerateColumns="false" runat="server" ShowPageTotal="false"
                        AllowSorting="false" OnItemDataBound="uxDeposits_ItemDataBound" CssClass="in" meta:resourcekey="uxDepositsResource1">
                        <MasterTableView>
                            <Columns>
                                <as:ASGridBoundColumn UniqueName="Day" DataField="Day" HeaderText="Day"
                                    HeaderTooltip="Day" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource1">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="RefNo." DataField="ReferenceNumber" HeaderText="Ref No."
                                    HeaderTooltip="Reference Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource2">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Items" DataField="Items" HeaderText="Items"
                                    HeaderTooltip="Items" ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource3">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Sales" DataField="Sales" HeaderText="Sales"
                                    HeaderTooltip="Sales Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource4">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Credits" DataField="Credits" HeaderText="Credits"
                                    HeaderTooltip="Credits Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource5">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Disc" DataField="Disc" HeaderText="Disc"
                                    HeaderTooltip="Discount Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource6">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="NetDeposit" DataField="NetDeposit" HeaderText="Net Deposit"
                                    HeaderTooltip="Net Deposit" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                            </Columns>

                            <EditFormSettings>
                                <EditColumn InsertImageUrl="Update.gif" UpdateImageUrl="Update.gif" CancelImageUrl="Cancel.gif"></EditColumn>
                            </EditFormSettings>

                        </MasterTableView>

                        <FooterStyle HorizontalAlign="Left"></FooterStyle>

                        <FilterMenu EnableEmbeddedSkins="False"></FilterMenu>

                        <HeaderContextMenu EnableEmbeddedSkins="False"></HeaderContextMenu>
                    </as:ASGrid>
                </div>
            </as:Panel>
            <!-- Deposit Item Sumary -->
            <as:Panel ID="pnlDepositItem" runat="server" meta:resourcekey="pnlDepositItemResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h1" data-toggle="collapse" data-target="#DepositSummaryID">
                            <as:Literal ID="Literal4" runat="server" Text="Deposits Item Summary" meta:resourcekey="Literal4Resource1"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div class="row in" id="DepositSummaryID">
                    <asp:Repeater ID="uxRepeaterDepositSumary1" runat="server">
                        <ItemTemplate>
                            <div class="col-md-6">
                                <table class="ASTable">
                                    <tr>
                                        <th class="heading" title="Tickets">
                                            <as:Literal ID="Literal4" runat="server" Text="Tickets" meta:resourcekey="Literal4Resource2"></as:Literal></th>
                                        <th class="heading" title="Number">
                                            <as:Literal ID="Literal5" runat="server" Text="Number" meta:resourcekey="Literal5Resource1"></as:Literal></th>
                                        <th class="heading" title="Amount">
                                            <as:Literal ID="Literal6" runat="server" Text="Amount" meta:resourcekey="Literal6Resource1"></as:Literal></th>
                                    </tr>
                                    <tr class="Row">
                                        <td>
                                            <as:Literal ID="Literal7" runat="server" Text="SALES" meta:resourcekey="Literal7Resource1"></as:Literal></td>
                                        <td class="text-right"><%# Eval("SaleCount") %></td>
                                        <td class="text-right"><%# AS.Common.Formater.FormatData.FormatCurrency(Eval("SaleAmount"), SessionManager.CurrencyFortmat)%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <as:Literal ID="Literal8" runat="server" Text="CREDITS" meta:resourcekey="Literal8Resource1"></as:Literal></td>
                                        <td class="text-right">
                                            <%# Eval("ReturnCount") %></td>
                                        <td class="text-right">
                                            <%# AS.Common.Formater.FormatData.FormatCurrency(Eval("ReturnAmount"), SessionManager.CurrencyFortmat)%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="heading">
                                            <as:Literal ID="Literal9" runat="server" Text="TOTAL" meta:resourcekey="Literal9Resource1"></as:Literal></td>
                                        <td class="text-right heading">
                                            <%# SumDepositSummaryNumber(Eval("SaleCount"), Eval("ReturnCount")) %>
                                        </td>
                                        <td class="text-right heading">
                                            <%# AS.Common.Formater.FormatData.FormatCurrency(SumDepositSummaryAmount(Eval("SaleAmount"), Eval("ReturnAmount")), SessionManager.CurrencyFortmat)%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-md-6">
                                <table class="ASTable">
                                    <tr>
                                        <th class="heading" title="Adjustment">
                                            <as:Literal ID="Literal10" runat="server" Text="ADJ" meta:resourcekey="Literal10Resource1"></as:Literal></th>
                                        <th class="heading" title="Number">
                                            <as:Literal ID="Literal11" runat="server" Text="Number" meta:resourcekey="Literal11Resource1"></as:Literal></th>
                                        <th class="heading" title="Amount">
                                            <as:Literal ID="Literal12" runat="server" Text="Amount" meta:resourcekey="Literal12Resource1"></as:Literal></th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <as:Literal ID="Literal13" runat="server" Text="DB ADJ" meta:resourcekey="Literal13Resource1"></as:Literal></td>
                                        <td class="text-right">
                                            <%# Eval("DebitAdjustCount")%>
                                        </td>
                                        <td class="text-right">
                                            <%# AS.Common.Formater.FormatData.FormatCurrency(Eval("DebitAdjustAmount"), SessionManager.CurrencyFortmat)%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <as:Literal ID="Literal14" runat="server" Text="CR ADJ" meta:resourcekey="Literal14Resource1"></as:Literal></td>
                                        <td class="text-right">
                                            <%# Eval("CreditAdjCount")%>
                                        </td>
                                        <td class="text-right">
                                            <%# AS.Common.Formater.FormatData.FormatCurrency(Eval("CreditAdjustAMount"), SessionManager.CurrencyFortmat)%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <as:Literal ID="Literal15" runat="server" Text="TOTAL" meta:resourcekey="Literal15Resource1"></as:Literal></td>
                                        <td class="text-right heading">
                                            <%# SumDepositSummaryNumber(Eval("DebitAdjustCount"), Eval("CreditAdjCount"))%>
                                        </td>
                                        <td class="text-right heading">
                                            <%#  AS.Common.Formater.FormatData.FormatCurrency(SumDepositSummaryAmount(Eval("DebitAdjustAmount"), Eval("CreditAdjustAMount")), SessionManager.CurrencyFortmat)%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </as:Panel>

            <!--Card Sumary-->
            <as:Panel ID="pnlCardSumary" runat="server" meta:resourcekey="pnlCardSumaryResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h2" data-toggle="collapse" data-target="#CardSummaryID">
                            <as:Literal ID="Literal15" runat="server" Text="Card Summary" meta:resourcekey="Literal15Resource2"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div class="in" id="CardSummaryID">
                    <as:ASGrid ID="uxCardSumary" runat="server" GridLines="None" ShowPageTotal="false"
                        AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod"
                        FooterStyle-HorizontalAlign="Left" CssClass="in" meta:resourcekey="uxCardSumaryResource1">
                        <MasterTableView>
                            <Columns>
                                <as:ASGridBoundColumn UniqueName="Date" DataField="PostDate" HeaderText="Date" HeaderTooltip="Date" meta:resourcekey="ASGridBoundColumnResource8">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Ref" ASFormat="StaticString" DataField="BatchSequence"
                                    HeaderText="Ref" HeaderTooltip="Batch Sequence" meta:resourcekey="ASGridBoundColumnResource9">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Mastercard" ASFormat="Currency" DataField="MasterCard"
                                    HeaderText="Mastercard" HeaderTooltip="Mastercard" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource10">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Discover" ASFormat="Currency" DataField="Discover"
                                    HeaderText="Discover" HeaderTooltip="Discover" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource11">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Visa" ASFormat="Currency" DataField="Visa" HeaderText="Visa"
                                    HeaderTooltip="Visa" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource12">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Amex" ASFormat="Currency" DataField="Amex" HeaderText="Amex"
                                    HeaderTooltip="Amex" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource13">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Diners" ASFormat="Currency" DataField="Diners"
                                    HeaderText="Diners" HeaderTooltip="Diners" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource14">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Others" ASFormat="Currency" DataField="Others"
                                    HeaderText="Others" HeaderTooltip="Others" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource15">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                            </Columns>
                        </MasterTableView>

                        <FooterStyle HorizontalAlign="Left"></FooterStyle>

                    </as:ASGrid>
                </div>
            </as:Panel>

            <!--Settlement Discount-->
            <as:Panel ID="pnlSettlementDiscount" runat="server" meta:resourcekey="pnlSettlementDiscountResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h3" data-toggle="collapse" data-target="#SettlementID">
                            <as:Literal ID="Literal16" runat="server" Text="Settlement/Discount" meta:resourcekey="Literal16Resource1"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 in" id="SettlementID">
                        <table class="ASTable">
                            <tr>
                                <th class="heading" title="Description">
                                    <as:Literal ID="ltDescription" runat="server" Text="Description" meta:resourcekey="ltDescriptionResource1"></as:Literal>

                                </th>
                                <th class="heading" style="text-align: center;" title="Items">
                                    <as:Literal ID="Literal17" runat="server" Text="Items" meta:resourcekey="Literal17Resource1"></as:Literal></th>
                                <th class="heading" style="text-align: center;" title="Amount">
                                    <as:Literal ID="Literal18" runat="server" Text="Amount" meta:resourcekey="Literal18Resource1"></as:Literal></th>
                                <th class="heading" style="text-align: center;" title="Average Ticket">
                                    <as:Literal ID="Literal19" runat="server" Text="Avg Ticket" meta:resourcekey="Literal19Resource1"></as:Literal>
                                </th>
                                <th class="heading" style="text-align: center;" title="Discount Rate">
                                    <as:Literal ID="Literal20" runat="server" Text="Disc Rate" meta:resourcekey="Literal20Resource1"></as:Literal>
                                </th>
                                <th class="heading" style="text-align: center;" title="Item Rate">
                                    <as:Literal ID="Literal21" runat="server" Text="Item Rate" meta:resourcekey="Literal21Resource1"></as:Literal></th>
                                <th class="heading" style="text-align: center;" title="Fee Amount">
                                    <as:Literal ID="Literal22" runat="server" Text="Fee Amount" meta:resourcekey="Literal22Resource1"></as:Literal></th>
                            </tr>
                            <as:ASRepeater ID="uxSettlementDiscount" runat="server" OnItemDataBound="uxSettlementDiscount_ItemDataBound"
                                NumberOfColumns="7">
                                <ItemTemplate>
                                    <tr class="Row">
                                        <td align="left"><%# Eval("Description")%></td>
                                        <td align="right"><%# FormatInteger(Eval("Items"))%></td>
                                        <td align="right"><%# FormatCurrency(Eval("Amount"))%></td>
                                        <td align="right"><%# FormatCurrency(Eval("AverageTicket"))%></td>
                                        <td align="right"><%# FormatNumber4Digits(Eval("DiscountRate"))%></td>
                                        <td align="right"><%# FormatNumber4Digits(Eval("ItemRate"))%></td>
                                        <td align="right"><%# FormatCurrency(Eval("FeeAmount"))%></td>
                                    </tr>
                                    <as:PlaceHolder ID="uxInterCh1" runat="server" Visible='<%# CheckInterChange(Eval("Interchange")) %>'>
                                        <tr class="Row">
                                            <td align="left">
                                                <as:Literal ID="Literal22" runat="server" Text="INTER-CHG" meta:resourcekey="Literal22Resource4"></as:Literal></td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td align="right"><%# FormatCurrency(Eval("Interchange"))%></td>
                                        </tr>
                                    </as:PlaceHolder>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AltRow">
                                        <td align="left"><%# Eval("Description")%></td>
                                        <td align="right"><%# FormatInteger(Eval("Items"))%></td>
                                        <td align="right"><%# FormatCurrency(Eval("Amount"))%></td>
                                        <td align="right"><%# FormatCurrency(Eval("AverageTicket"))%></td>
                                        <td align="right"><%# FormatNumber4Digits(Eval("DiscountRate"))%></td>
                                        <td align="right"><%# FormatNumber4Digits(Eval("ItemRate"))%></td>
                                        <td align="right"><%# FormatCurrency(Eval("FeeAmount"))%></td>
                                    </tr>
                                    <as:PlaceHolder ID="uxInterCh2" runat="server" Visible='<%# CheckInterChange(Eval("Interchange")) %>'>
                                        <tr class="AltRow">
                                            <td align="left">
                                                <as:Literal ID="Literal22" runat="server" Text="INTER-CHG" meta:resourcekey="Literal22Resource2"></as:Literal></td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td align="right"><%# FormatCurrency(Eval("Interchange"))%></td>
                                        </tr>
                                    </as:PlaceHolder>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    <asp:PlaceHolder ID="uxFooter" runat="server">
                                        <tr class="rgFooter" style="text-align: left;">
                                            <td class="ReportTotal heading">
                                                <as:Literal ID="Literal22" runat="server" Text="TOTAL" meta:resourcekey="Literal22Resource3"></as:Literal></td>
                                            <td class="ReportTotal">&nbsp;</td>
                                            <td class="ReportTotal"></td>
                                            <td class="ReportTotal"></td>
                                            <td class="ReportTotal"></td>
                                            <td class="ReportTotal"></td>
                                            <td class="ReportTotal heading" align="right">
                                                <asp:Literal ID="uxLtrFeeAmount" runat="server" meta:resourcekey="uxLtrFeeAmountResource1"></asp:Literal></td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </FooterTemplate>
                            </as:ASRepeater>
                        </table>
                    </div>
                </div>
            </as:Panel>

            <!--Surcharge-->
            <as:Panel ID="pnlSurcharge" runat="server" meta:resourcekey="pnlSurchargeResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h4" data-toggle="collapse" data-target="#SurchargeID">
                            <as:Literal ID="Literal23" runat="server" Text="Surcharge" meta:resourcekey="Literal23Resource1"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div class="in" id="SurchargeID">
                    <as:ASGrid ID="uxSurcharge" Width="100%" FooterStyle-HorizontalAlign="Left" AllowSorting="false"
                        AllowPaging="false" GridName="Surcharge" ShowPageTotal="false" XOverFlowable="false"
                        AutoGenerateColumns="false" runat="server" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxSurchargeResource1">
                        <MasterTableView Width="100%">
                            <Columns>
                                <as:ASGridBoundColumn UniqueName="Rate" ASFormat="DynamicString" HeaderTooltip="Rate"
                                    DataField="Description" HeaderText="Rate" meta:resourcekey="ASGridBoundColumnResource16">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Items" ASFormat="Integer" HeaderTooltip="Items"
                                    DataField="Items" HeaderText="Items" meta:resourcekey="ASGridBoundColumnResource17">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Volume" ASFormat="Currency" HeaderTooltip="Volume"
                                    DataField="Volume" HeaderText="Volume" meta:resourcekey="ASGridBoundColumnResource18">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Amount" ASFormat="Currency" DataField="SurchargeAmount"
                                    HeaderText="Amount" ASIsTotalColumn="true"
                                    HeaderTooltip="Amount" meta:resourcekey="ASGridBoundColumnResource19">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                            </Columns>
                        </MasterTableView>

                        <FooterStyle HorizontalAlign="Left"></FooterStyle>

                    </as:ASGrid>
                </div>
            </as:Panel>

            <!--Others Fees-->
            <as:Panel ID="pnlOtherFee" runat="server" meta:resourcekey="pnlOtherFeeResource1">
                <div class="row">
                    <div class="col-xs-12">
                        <h2 class="grid-title" id="h5" data-toggle="collapse" data-target="#OtherFeesID">
                            <as:Literal ID="Literal24" runat="server" Text="Other Fees" meta:resourcekey="Literal24Resource1"></as:Literal>
                        </h2>
                    </div>
                </div>
                <div class="in" id="OtherFeesID">
                    <as:ASGrid ID="uxOtherFee" Width="100%" FooterStyle-HorizontalAlign="Left" AllowSorting="false"
                        AllowPaging="false" GridName="Deposits" ShowPageTotal="false" XOverFlowable="false"
                        AutoGenerateColumns="false" runat="server" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxOtherFeeResource1">
                        <MasterTableView Width="100%">
                            <Columns>
                                <as:ASGridBoundColumn UniqueName="Card" ASFormat="StaticString" HeaderTooltip="Card Type"
                                    DataField="CardTypeCode" HeaderText="Card" meta:resourcekey="ASGridBoundColumnResource20">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Charge" ASFormat="StaticString"
                                    DataField="ChargeType" HeaderText="Charge" HeaderTooltip="Charge Type" meta:resourcekey="ASGridBoundColumnResource21">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Description" ASFormat="DynamicString" HeaderTooltip="Description"
                                    DataField="ShortDescription" HeaderText="Description" meta:resourcekey="ASGridBoundColumnResource22">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Number" ASFormat="Integer" HeaderTooltip="Number"
                                    DataField="Tickets" HeaderText="Number" meta:resourcekey="ASGridBoundColumnResource23">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Rate" ASFormat="Number4Digits" HeaderTooltip="Rate"
                                    DataField="Rate" HeaderText="Rate" meta:resourcekey="ASGridBoundColumnResource24">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn UniqueName="Fees" ASFormat="Currency" HeaderTooltip="Amount"
                                    DataField="Amount" HeaderText="Fees" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource25">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                            </Columns>
                        </MasterTableView>

                        <FooterStyle HorizontalAlign="Left"></FooterStyle>

                    </as:ASGrid>
                </div>
            </as:Panel>
            <table style="float: right; font-weight: 800;">
                <as:PlaceHolder ID="uxMinBillAdj" runat="server">
                    <tr>
                        <td style="text-align: right;">
                            <as:Literal ID="Literal25" runat="server" Text="Minimum Billing Adjustment:" meta:resourcekey="Literal25Resource1"></as:Literal>
                        </td>
                        <td style="text-align: right; width: 100px;">
                            <%= MinBillAdjustment%>
                        </td>
                    </tr>
                </as:PlaceHolder>
                <tr>
                    <td style="text-align: right;">
                        <as:Literal ID="Literal26" runat="server" Text="Your Account Has Been Debited:" meta:resourcekey="Literal26Resource1"></as:Literal>
                    </td>
                    <td style="text-align: right; width: 100px;">
                        <%= total_debit%>
                    </td>
                </tr>
            </table>
            <!--Message-->
            <as:Panel ID="pnlMessage" runat="server" Visible="false" meta:resourcekey="pnlMessageResource1">
                <br />
                <br />
                <div style="margin-top: -12px; text-align: left;">
                    <label style="font-weight: 800;">&nbsp;<as:Literal ID="Literal27" runat="server" Text="MESSAGE" meta:resourcekey="Literal27Resource1"></as:Literal></label>
                </div>
                <div style="background-color: #F2F2F2; padding: 12px; text-align: justify;">
                    <asp:Literal ID="uxMessage" runat="server" Text="" meta:resourcekey="uxMessageResource1"></asp:Literal>
                </div>
            </as:Panel>
            <div style="clear: both;">
            </div>

        </as:PlaceHolder>

        <div class="height-18"></div>
        <div class="row">
            <div class="col-md-12 text-right no-margin-action-container">
                <as:Button runat="server" ID="uxClose" Text="Close" OnClientClick="return parent.HidePopupModal();"
                    CssClass="btn btn-default" meta:resourcekey="uxCloseResource1" />
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

