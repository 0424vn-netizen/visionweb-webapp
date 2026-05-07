<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Returns"
    CodeFile="ReturnReport.aspx.cs" Inherits="gen_RiskReport" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxReturnChart" Src="~/UserControls/ReturnsCharts.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxDrilldownGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxDrilldownGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxNonVerifiedGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxNonVerifiedGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxVerifiedGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxVerifiedGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <uc:ReportFiltering ID="uxReportFiltering" runat="server" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true"
        HasFilteringOption="true" />
    <!--Chart-->
    <div class="chart-container">
        <uc:UxReturnChart runat="server" ID="uxChart" />
    </div>
    <uc:UxExport ID="uxExporter" runat="server" GridID="uxDrilldownGrid" />
    <as:ASGrid ID="uxDrilldownGrid" runat="server" AllowPaging="true" AllowSorting="true" AppendHeaderforPrinter="true" MasterTableView-TableLayout="Fixed" XOverFlowable="true"
        AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" ShowPageTotal="false" CssClass="in" meta:resourcekey="uxDrilldownGridResource1"
        ClientSettings-ClientEvents-OnDataBound="OnGridDataBound()" IsAutoExportTemplate="true">
        <MasterTableView>
            <PagerStyle AlwaysVisible="true" />
            <Columns>
                <as:ASGridBoundColumn HeaderText="Entity" DataField="Entity" UniqueName="DrilldownColumn" HeaderStyle-Width="180px"
                    SortExpression="Entity" HeaderTooltip="Entity" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Entity Name" DataField="EntityName" UniqueName="EntityName"
                    SortExpression="EntityName" HeaderTooltip="Entity Name" ASFormat="DynamicString"
                    Visible="false" meta:resourcekey="ASGridBoundColumnResource2" HeaderStyle-Width="250px">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="# Sales" DataField="SaleCount" UniqueName="SaleCount"
                    HeaderTooltip="Sales Count" SortExpression="SaleCount" ASIsTotalColumn="true"
                    ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Sales" DataField="SaleAmount" UniqueName="SaleAmount" ASFormat="Currency"
                    HeaderTooltip="Sales Amount" SortExpression="SaleAmount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="# Returns" DataField="ReturnCount" UniqueName="ReturnCount"
                    HeaderTooltip="Returns Transaction Count" SortExpression="ReturnCount" ASIsTotalColumn="true"
                    ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Matched" DataField="FullReturn" UniqueName="FullReturn" ASFormat="Currency"
                    HeaderTooltip="“MATCHED” Returns Amount" SortExpression="FullReturn" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Partial Match" DataField="PartialReturn" UniqueName="PartialReturn"
                    HeaderTooltip="“PARTIAL MATCH” Returns Amount" SortExpression="PartialReturn" ASFormat="Currency"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Unmatched" DataField="NoMatch" UniqueName="NoMatch" ASFormat="Currency"
                    HeaderTooltip="“UNMATCHED” Returns Amount" SortExpression="NoMatch" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Net" DataField="NetAmount" UniqueName="NetAmount" ASFormat="Currency"
                    HeaderTooltip="Net Amount" SortExpression="NetAmount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
    <!-- Merchant Level View CS -->
    <as:PlaceHolder ID="uxCSMerchantViewPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporterReport" GridID="uxReportGrid" runat="server" />
        <as:ASGrid ID="uxReportGrid" runat="server" Width="100%" AutoGenerateColumns="false" IsAutoExportTemplate="true"
            IntruderSourceName="uxReportGrid" IsIntruder="true" AllowPaging="true" AllowSorting="true" AppendHeaderforPrinter="true"
            Visible="false" ASPagingMethod="SPASingleMethod" ShowPageTotal="false" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" SortExpression="ReportDate"
                        UniqueName="ReportDate" ASFormat="Date" HeaderTooltip="Report Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Terminal #" DataField="TerminalNumber" SortExpression="TerminalNumber" HeaderStyle-Width="100px"
                        UniqueName="TerminalNumber" ASFormat="StaticString" HeaderTooltip="Terminal Number" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Batch #" DataField="BatchNumber" SortExpression="BatchNumber" HeaderStyle-Width="100px"
                        UniqueName="BatchNumber" HeaderTooltip="Batch Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" SortExpression="TransactionDate"
                        UniqueName="TransactionDate" HeaderTooltip="Transaction Date" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Time" DataField="TransactionTime" SortExpression="TransactionTime" HeaderStyle-Width="100px"
                        UniqueName="TransactionTime" HeaderTooltip="Transaction Time" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                     <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource46" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn HeaderText="Keyed" DataField="Keyed" SortExpression="Keyed" HeaderStyle-Width="60px"
                        UniqueName="Keyed" ASFormat="StaticString" HeaderTooltip="KEYED or SWIPED" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardType" SortExpression="CardType"
                        UniqueName="CardType" ASFormat="StaticString" HeaderTooltip="Card Type" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="AccountNumber" SortExpression="PartialCardNumber" HeaderStyle-Width="150px"
                        ItemStyle-Wrap="false"
                        UniqueName="AccountNumber" ASFormat="StaticString" HeaderTooltip="Card Number" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" SortExpression="PartialCardNumber" HeaderStyle-Width="150px"
                        ItemStyle-Wrap="false"
                        UniqueName="PartialCardNumber" ASFormat="StaticString" HeaderTooltip="Card Number"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="RoutingAccountNumber" SortExpression="RoutingAccountNumber" HeaderStyle-Width="150px"
                        ItemStyle-Wrap="false" UniqueName="RoutingAccountNumber" ASFormat="StaticString" meta:resourcekey="RoutingAccountNumber" Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialRoutingACC" SortExpression="PartialRoutingACC" HeaderStyle-Width="150px"
                        ItemStyle-Wrap="false" UniqueName="PartialRoutingACC" ASFormat="StaticString"
                        Visible="false" meta:resourcekey="RoutingAccountNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ExpirationDate" DataField="ExpirationDate" HeaderText="Exp Date" HeaderStyle-Width="80px"
                        HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Amount" DataField="TransactionAmount" SortExpression="TransactionAmount" HeaderStyle-Width="100px"
                        UniqueName="TransactionAmount" ASFormat="Currency" HeaderTooltip="Transaction Amount"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                     <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource43" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalTransactionAmount" UniqueName="OriginalTransactionAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                        ASFormat="Number2Digit" meta:resourcekey="ASGridBoundColumnResource44" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderStyle-CssClass="text-center" DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource45" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn HeaderText="Matched" DataField="MatchCode" SortExpression="MatchCode" HeaderStyle-Width="100px"
                        UniqueName="MatchCode" ASFormat="StaticString" HeaderTooltip="Matched" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <!-- Merchant Level View MS -->
    <as:PlaceHolder ID="uxMSMerchantViewPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporterNonVerifiedTop" runat="server" GridID="uxNonVerifiedGrid" />
        <as:ASGrid ID="uxNonVerifiedGrid" runat="server" AutoGenerateColumns="false" ShowPageTotal="false" AppendHeaderforPrinter="true"
            ASPagingMethod="SPASingleMethod" AllowSorting="True" AllowPaging="True" IntruderSourceName="uxNonVerifiedGrid"
            IsIntruder="true" CssClass="in" meta:resourcekey="uxNonVerifiedGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                        HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Terminal #" DataField="TerminalNumber" SortExpression="TerminalNumber"
                        UniqueName="TerminalNumber" ASFormat="StaticString" HeaderTooltip="Terminal Number" meta:resourcekey="ASGridBoundColumnResource23">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Batch #" DataField="BatchNumber" SortExpression="BatchNumber"
                        UniqueName="BatchNumber" HeaderTooltip="Batch Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource24">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" UniqueName="TransactionDate"
                        HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Time" DataField="TransactionTime" SortExpression="TransactionTime"
                        UniqueName="TransactionTime" HeaderTooltip="Transaction Time" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource26">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Keyed" DataField="Keyed" UniqueName="Keyed" HeaderTooltip="Keyed"
                        SortExpression="Keyed" meta:resourcekey="ASGridBoundColumnResource27">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardType" UniqueName="CardType"
                        HeaderTooltip="Card Type" SortExpression="CardType" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource28">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="AccountNumber" UniqueName="AccountNumber"
                        HeaderTooltip="Card Number" Visible="false" SortExpression="AccountNumber" meta:resourcekey="ASGridBoundColumnResource29">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                        HeaderTooltip="Card Number" SortExpression="PartialCardNumber" meta:resourcekey="ASGridBoundColumnResource29">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="RoutingAccountNumber" SortExpression="RoutingAccountNumber" HeaderStyle-Width="150px"
                        ItemStyle-Wrap="false" UniqueName="RoutingAccountNumber" Visible="false" ASFormat="StaticString" meta:resourcekey="RoutingAccountNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialRoutingACC" SortExpression="PartialRoutingACC" HeaderStyle-Width="150px"
                        ItemStyle-Wrap="false" UniqueName="PartialRoutingACC" ASFormat="StaticString"
                        Visible="false" meta:resourcekey="RoutingAccountNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ExpirationDate" DataField="ExpirationDate" HeaderText="Exp Date"
                        HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource30">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Amount" DataField="TransactionAmount" UniqueName="TransactionAmount"
                        HeaderTooltip="Transaction Amount" SortExpression="TransactionAmount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource31">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderText="Verify" HeaderTooltip="Verify" UniqueName="VerifyReturns"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource1">
                        <HeaderTemplate>
                            <as:Literal ID="lblVerify" runat="server" Text="Verify" meta:resourcekey="lblVerifyResource1"></as:Literal>
                            <as:CheckBox onclick="javascript:return doVerifyReturns('-1');" ID="chkAllVerify"
                                runat="server" meta:resourcekey="chkAllVerifyResource1" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <as:CheckBox ID="chkVerify" runat="server" meta:resourcekey="chkVerifyResource1" />
                        </ItemTemplate>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridTemplateColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>

        <uc:UxExport ID="uxExporterVerifiedTop" runat="server" GridID="uxVerifiedGrid" />
        <as:ASGrid ID="uxVerifiedGrid" runat="server" AutoGenerateColumns="false" ShowPageTotal="false"
            ASPagingMethod="SPASingleMethod" AllowSorting="True" AllowPaging="True" IntruderSourceName="uxVerifiedGrid" AppendHeaderforPrinter="true"
            IsIntruder="true" CssClass="in" meta:resourcekey="uxVerifiedGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                        HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource32">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Terminal #" DataField="TerminalNumber" SortExpression="TerminalNumber"
                        UniqueName="TerminalNumber" ASFormat="StaticString" HeaderTooltip="Terminal Number" meta:resourcekey="ASGridBoundColumnResource33">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Batch #" DataField="BatchNumber" SortExpression="BatchNumber"
                        UniqueName="BatchNumber" HeaderTooltip="Batch Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource34">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" UniqueName="TransactionDate"
                        HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource35">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Time" DataField="TransactionTime" SortExpression="TransactionTime"
                        UniqueName="TransactionTime" HeaderTooltip="Transaction Time" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource36">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Keyed" DataField="Keyed" UniqueName="Keyed" HeaderTooltip="Keyed"
                        SortExpression="Keyed" meta:resourcekey="ASGridBoundColumnResource37">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardType" UniqueName="CardType"
                        HeaderTooltip="Card Type" SortExpression="CardType" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource38">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="AccountNumber" UniqueName="AccountNumber"
                        HeaderTooltip="Card Number" Visible="false" SortExpression="AccountNumber" meta:resourcekey="ASGridBoundColumnResource29">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                        ItemStyle-Wrap="false"
                        HeaderTooltip="Card Number" SortExpression="PartialCardNumber" meta:resourcekey="ASGridBoundColumnResource39">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="RoutingAccountNumber" SortExpression="RoutingAccountNumber" HeaderStyle-Width="150px"
                        ItemStyle-Wrap="false" UniqueName="RoutingAccountNumber" ASFormat="StaticString" meta:resourcekey="RoutingAccountNumber" Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialRoutingACC" SortExpression="PartialRoutingACC" HeaderStyle-Width="150px"
                        ItemStyle-Wrap="false" UniqueName="PartialRoutingACC" ASFormat="StaticString"
                        Visible="false" meta:resourcekey="RoutingAccountNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ExpirationDate" DataField="ExpirationDate" HeaderText="Exp Date"
                        HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource40">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Amount" DataField="TransactionAmount" UniqueName="TransactionAmount"
                        HeaderTooltip="Transaction Amount" SortExpression="TransactionAmount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource41">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Matched" DataField="MatchCode" UniqueName="MatchCode"
                        HeaderTooltip="Matched" SortExpression="MatchCode" meta:resourcekey="ASGridBoundColumnResource42">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <as:HiddenField ID="uxHidArgs" runat="server" />
    <as:HiddenField ID="uxHidClick" runat="server" />
    <as:Button ID="uxVerifyMerchant" IsStandardButton="True" runat="server" OnClick="btnVerify_Click"
        Style="display: none;" meta:resourcekey="uxVerifyMerchantResource1" />
    <tek:RadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            var ReturnReport_uxHidArgs = "<%=uxHidArgs.ClientID%>";
            var ReturnReport_uxHidClick = "<%=uxHidClick.ClientID%>";
            var ReturnReport_uxVerifyMerchant = "<%=uxVerifyMerchant.ClientID%>";
            var ReturnReport_uxDrilldownGrid = "<%=uxDrilldownGrid.ClientID%>";

            var ReturnReport_Returns_ComfirmVerify_All = "<%=Resources.MessageManager.Returns_ComfirmVerify_All%>";
            var ReturnReport_Returns_ComfirmVerify = "<%=Resources.MessageManager.Returns_ComfirmVerify%>";
            var ReturnReport_js_Returns = '<%= GetLocalResourceObject("ReturnReport_aspx_cs_Return").ToString()%>';
            var isEnableEntityName = '<%= uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible  %>';
        </script>
        <script src="<%=ResolveUrl("~") %>res/js/ReturnReport.js" type="text/javascript"></script>
    </tek:RadCodeBlock>
</asp:Content>
