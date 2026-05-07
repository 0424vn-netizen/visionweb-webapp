<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="BatchDetailModal.aspx.cs" Inherits="BatchDetailModal" Title="Batch Details" meta:resourcekey="PageResource1" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-9">
                <h3 class="modal-title">
                    <asp:Label ID="ltrMerchantInfor" runat="server" meta:resourcekey="ltrMerchantInforResource1"></asp:Label>
                </h3>
            </div>
            <div class="col-md-3 text-right">
                <asp:Panel ID="uxPrinterpnl" runat="server">
                    <asp:HyperLink runat="server" Text="Print" CssClass="font-weight-bold font-12" NavigateUrl="javascript:window.print();" meta:resourcekey="ResourcePrinterLink" ToolTip="Printer Friendly Version"></asp:HyperLink>
                </asp:Panel>
            </div>
        </div>

        <as:PlaceHolder ID="uxComboTerminalPlaceHolder" runat="server" Visible="false">
            <as:ASRadComboBox ID="uxComboTerminal" runat="server" Width="250px" AutoPostBack="true"
                OnSelectedIndexChanged="uxComboTerminal_SelectedIndexChanged" meta:resourcekey="uxComboTerminalResource1" />
        </as:PlaceHolder>
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" ShowWord="false"
            IsOnTop="true" />
        <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AllowPaging="True" AppendHeaderforPrinter="true"
            AllowSorting="True" AutoGenerateColumns="False" AllowFilteringByColumn="false" HeaderStyle-Width="100px" IsCacheTemplateFile="false"
            ShowFooter="true" ASPagingMethod="SPASingleMethod" ShowPageTotal="false" VisiblePageTotal="false" IsAutoExportTemplate="true"
            VisibleReportTotal="true" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn UniqueName="TransactionDate" HeaderText="Trans Date" DataField="TransactionDate"
                        HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ReportDate" DataField="ReportDate" Visible="false"
                        Display="false" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionTime" HeaderText="Trans Time" DataField="TransactionTime" HeaderStyle-Width="100px"
                        HeaderTooltip="Transaction Time" SortExpression="TransactionTime" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionCode" HeaderText="Trans Code" DataField="TransactionDescription" HeaderStyle-Width="100px"
                        HeaderTooltip="Transaction Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource36">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn UniqueName="TerminalNumber" HeaderText="Terminal #" DataField="TerminalNumber" HeaderStyle-Width="120px"
                        HeaderTooltip="Terminal Number" SortExpression="TerminalNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="File Source" HeaderTooltip="File Source" DataField="FileSource" HeaderStyle-Width="200px" ItemStyle-CssClass="word-break"
                        UniqueName="FileSource" SortExpression="FileSource" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="KeyedEntry" HeaderText="Keyed" DataField="KeyedEntry" HeaderStyle-Width="80px"
                        HeaderTooltip="KEYED or SWIPED" SortExpression="Keyed" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EMVIndicator" HeaderText="EMV" DataField="EMVIndicator" HeaderStyle-Width="80px"
                        HeaderTooltip="EMV" SortExpression="EMVIndicator" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource32">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="CardType" HeaderText="Card Type" DataField="CardType"
                        HeaderTooltip="Card Type" SortExpression="CardType" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Card #" HeaderStyle-Width="140px"
                        HeaderTooltip="Card Number" SortExpression="AccountNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialCardNumber" UniqueName="PartialCardNumber" HeaderStyle-Width="140px"
                        ItemStyle-Wrap="false" HeaderText="Card #" HeaderTooltip="Card Number" SortExpression="PartialCardNumber"
                        Visible="false" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="RoutingAccountNumber" UniqueName="RoutingAccountNumber" HeaderStyle-Width="140px"
                        ItemStyle-Wrap="false" HeaderText="Routing/Account Number" HeaderTooltip="Routing/Account Number" SortExpression="RoutingAccountNumber"
                        Visible="false" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource40">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialRoutingACC" UniqueName="PartialRoutingACC" HeaderStyle-Width="140px"
                        ItemStyle-Wrap="false" SortExpression="PartialRoutingACC"
                        Visible="false" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource40">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ExpirationDate" DataField="ExpirationDate" HeaderText="Exp Date" HeaderStyle-Width="100px"
                        HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AuthorizationNumber" HeaderText="Auth #" DataField="AuthorizationNumber" HeaderStyle-Width="60px"
                        HeaderTooltip="Authorization Number" SortExpression="AuthorizationNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionAmount" HeaderText="Trans Amount" DataField="TransactionAmount" HeaderStyle-Width="100px"
                        HeaderTooltip="Transaction Amount" ASIsTotalColumn="true" SortExpression="TransactionAmount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource33" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalTransactionAmount" UniqueName="OriginalTransactionAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                        ASFormat="Number2Digit" meta:resourcekey="ASGridBoundColumnResource34" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address" HeaderStyle-CssClass="text-center"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource35" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>

                    <as:ASGridBoundColumn UniqueName="Voucher" HeaderText="Voucher" DataField="Voucher"
                        AllowSorting="false" HeaderTooltip="Printable voucher" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <as:PlaceHolder ID="uxCardSummaryHeader" runat="server" Visible="false">
            <div class="PageTitle">
                <as:Literal ID="ltBATCHDETAILSCARDSUMMARY" runat="server" Text="BATCH DETAILS CARD SUMMARY" meta:resourcekey="ltBATCHDETAILSCARDSUMMARYResource1"></as:Literal>
            </div>
            <br />
            <asp:Label ID="ltrMerInfoCardSummary" runat="server" CssClass="MerchantInfor" meta:resourcekey="ltrMerInfoCardSummaryResource1"></asp:Label><br />
        </as:PlaceHolder>
        <uc:UxExport ID="uxExportCardSummaryTop" runat="server" GridID="uxCardMerchantGrid" />
        <as:ASGrid ID="uxCardMerchantGrid" runat="server" AutoGenerateColumns="false" AppendHeaderforPrinter="true" IsCacheTemplateFile="false"
            IsIntruder="false" AllowSorting="True" ASPagingMethod="SPASingleMethod" AllowPaging="True" IsAutoExportTemplate="true"
            PageSize="10" ShowPageTotal="false" ShowReportTotal="true" CssClass="in" meta:resourcekey="uxCardMerchantGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Card Type" HeaderTooltip="Card Type" DataField="CardDescription" HeaderStyle-Width="150px"
                        UniqueName="CardType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Terminal #" HeaderTooltip="Terminal Number" DataField="TerminalNumber" HeaderStyle-Width="110px"
                        UniqueName="TerminalNr" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Transaction Code" HeaderTooltip="Transaction Code"
                        DataField="TransactionDescription" UniqueName="TransactionCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Tran Cnt." HeaderTooltip="Transaction Count" DataField="TransactionCount"
                        UniqueName="TransactionCount" ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Total" HeaderTooltip="Total Transaction Amount"
                        DataField="NetAmount" UniqueName="TotalTransactionAmount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <uc:UxExport ID="uxExportTransaction" runat="server" ShowWord="false" GridID="uxTransaction" />
        <as:ASGrid ID="uxTransaction" runat="server" GridLines="None" AllowPaging="True" AppendHeaderforPrinter="true"
            AllowSorting="True" AutoGenerateColumns="False" ShowPageTotal="false" AllowFilteringByColumn="false" IsCacheTemplateFile="false"
            ShowFooter="true" ASPagingMethod="SPASingleMethod" GridName="Voided/Rejected Transactions" IsAutoExportTemplate="true"
            VisiblePageTotal="false" VisibleReportTotal="true" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxTransactionResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn UniqueName="ReportDate" HeaderText="Report Date" DataField="ReportDate"
                        SortExpression="ReportDate" HeaderTooltip="Report Date" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="BatchNumber" HeaderText="Batch #" DataField="BatchNumber" HeaderStyle-Width="100px"
                        HeaderTooltip="Batch Number" SortExpression="BatchNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionDate" HeaderText="Trans Date" DataField="TransactionDate"
                        HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionTime" HeaderText="Trans Time" DataField="TransactionTime" HeaderStyle-Width="100px"
                        HeaderTooltip="Transaction Time" SortExpression="TransactionTime" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource23">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionCode" HeaderText="Trans Code" DataField="TransactionDescription" HeaderStyle-Width="100px"
                        HeaderTooltip="Transaction Code" SortExpression="TransactionDescription" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource24">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource36" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn UniqueName="KeyedEntry" HeaderText="Keyed" DataField="KeyedEntry" HeaderStyle-Width="60px" HeaderTooltip="KEYED or SWIPED"
                        SortExpression="KeyedEntry" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EMVIndicator" HeaderText="EMV" DataField="EMVIndicator" HeaderStyle-Width="60px"
                        HeaderTooltip="EMV" SortExpression="EMVIndicator" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource32">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="CardType" HeaderText="Card Type" DataField="CardType"
                        HeaderTooltip="Card Type" SortExpression="CardType" ASFormat="StaticString"
                        HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource26">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Card #" HeaderStyle-Width="140px"
                        HeaderTooltip="Card Number" SortExpression="AccountNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource27">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialCardNumber" UniqueName="PartialCardNumber" HeaderStyle-Width="140px"
                        ItemStyle-Wrap="false" HeaderText="Card #" HeaderTooltip="Card Number" SortExpression="PartialCardNumber"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource28">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AuthorizationNumber" HeaderText="Auth #" DataField="AuthorizationNumber" HeaderStyle-Width="100px"
                        HeaderTooltip="Authorization Number" SortExpression="AuthorizationNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource29">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ReasonCode" HeaderText="RC" DataField="ReasonCode" HeaderStyle-Width="100px"
                        HeaderTooltip="Reason Code" SortExpression="ReasonCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource30">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionAmount" HeaderText="Trans Amount" DataField="TransactionAmount"
                        HeaderTooltip="Transactions Amount" ASIsTotalColumn="true" SortExpression="TransactionAmount" HeaderStyle-Width="100px"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource31">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource33" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalTransactionAmount" UniqueName="OriginalTransactionAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                        ASFormat="Number2Digit" meta:resourcekey="ASGridBoundColumnResource34" HeaderStyle-Width="120px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderStyle-CssClass="text-center" DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource35" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                </Columns>
            </MasterTableView>
        </as:ASGrid>

        <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxReportGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="uxTransaction">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxTransaction" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="uxCardMerchantGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxCardMerchantGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </tek:RadAjaxManagerProxy>
    </as:ASModalContainer>
</asp:Content>
