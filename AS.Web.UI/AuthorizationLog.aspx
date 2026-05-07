<%@ Page Title="Authorization Log" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AuthorizationLog.aspx.cs" Inherits="AuthorizationLog" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:ReportFiltering ID="uxReportFiltering" runat="server" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true"
        HasFilteringOption="true" />
    <as:PlaceHolder ID="uxDrilldownPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporterDrilldown" runat="server" GridID="uxDrilldownGrid" ShowWord="false"
            ShowPDF="true" IsOnTop="true" />
        <as:ASGrid ID="uxDrilldownGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true"
            GridLines="None" AllowPaging="true" ShowReportTotal="true" Visible="false" AppendHeaderforPrinter="true"
            ASPagingMethod="SPASingleMethod" meta:resourcekey="uxDrilldownGridResource1" IsAutoExportTemplate="true">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn DataField="Entity" UniqueName="DrilldownColumn" HeaderText="" HeaderStyle-Width="200px"
                        HeaderTooltip="" ASFormat="StaticString" SortExpression="Entity" meta:resourcekey="ASGridBoundColumnResource1">
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
                    <as:ASGridBoundColumn UniqueName="TransactionCount" HeaderText="# Trans" DataField="TransactionCount"
                        ASFormat="Integer" HeaderTooltip="Transaction Count" SortExpression="TransactionCount"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="SaleAmount" HeaderText="Sales" DataField="SaleAmount"
                        ASFormat="Currency" ASIsTotalColumn="true" HeaderTooltip="Sales Amount" SortExpression="SaleAmount" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ReturnAmount" HeaderText="Returns" DataField="ReturnAmount"
                        ASIsTotalColumn="true" HeaderTooltip="Returns Amount" SortExpression="ReturnAmount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="NetAmount" HeaderText="Net" DataField="NetAmount"
                        ASIsTotalColumn="true" HeaderTooltip="Net Amount" SortExpression="NetAmount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationCount" UniqueName="AuthCount" HeaderText="# Auth"
                        HeaderTooltip="Authorization Count" ASIsTotalColumn="true" ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationAmount" UniqueName="AuthAmount" HeaderText="Auth Amt"
                        HeaderTooltip="Authorization Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <as:PlaceHolder ID="uxReportPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" ShowWord="false"
            ShowPDF="true" IsOnTop="true" />
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true"
            AllowPaging="true" Width="100%" ASPagingMethod="SPASingleMethod" ShowPageTotal="false" AppendHeaderforPrinter="true"
            Visible="false" meta:resourcekey="uxReportGridResource1" HeaderStyle-Width="100px" CssClass="in" IsAutoExportTemplate="true">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn DataField="ReportDate" UniqueName="ReportDate" HeaderText="Report Date"
                        HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionDate" UniqueName="TransDate" HeaderText="Trans Date"
                        ASFormat="Date" HeaderTooltip="Transaction Date" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionTime" UniqueName="TransTime" HeaderText="Trans Time" HeaderStyle-Width="80px"
                        HeaderTooltip="Transaction Time" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource32">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="TransCodeDescription" UniqueName="TransCode" HeaderText="Trans Code"
                        HeaderTooltip="Transaction Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="KeyedEntry" UniqueName="KeyedEntry" HeaderText="Keyed" HeaderStyle-Width="70px"
                        ASFormat="StaticString" HeaderTooltip="KEYED or SWIPED" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="EMVIndicator" UniqueName="EMVIndicator" HeaderText="EMV" HeaderStyle-Width="70px"
                        ASFormat="StaticString" HeaderTooltip="EMV" meta:resourcekey="ASGridBoundColumnResource28">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="CardType" UniqueName="CardType" HeaderText="Type"
                        ASFormat="StaticString" HeaderTooltip="Card Type" HeaderStyle-Width="55px" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Card #" HeaderStyle-Width="140px"
                        HeaderTooltip="Card Number" SortExpression="PartialCardNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialCardNumber" UniqueName="PartialCardNumber" HeaderStyle-Width="140px"
                        ASFormat="StaticString" ItemStyle-Wrap="false" HeaderText="Card #" HeaderTooltip="Card Number"
                        SortExpression="PartialCardNumber" Visible="false" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="RoutingAccountNumber" UniqueName="RoutingAccountNumber"  HeaderStyle-Width="140px"
                        SortExpression="RoutingAccountNumber" ASFormat="StaticString" meta:resourcekey="RoutingAccountNumber" Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialRoutingACC" UniqueName="PartialRoutingACC" HeaderStyle-Width="140px"
                        ASFormat="StaticString" ItemStyle-Wrap="false" 
                        SortExpression="PartialRoutingACC" Visible="false" meta:resourcekey="RoutingAccountNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ExpirationDate" UniqueName="ExpirationDate" HeaderText="Exp Date" HeaderStyle-Width="100px"
                        HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationNumber" UniqueName="AuthNumber" HeaderText="Auth #" HeaderStyle-Width="80px"
                        HeaderTooltip="Authorization Number" SortExpression="AuthorizationNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationAmount" UniqueName="AuthAmount" HeaderText="Auth Amount" HeaderStyle-Width="100px"
                        HeaderTooltip="Authorization Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionAmount" UniqueName="TransactionAmount" HeaderStyle-Width="100px"
                        HeaderText="Trans Amount" HeaderTooltip="Transaction Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource29">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalAuthorizationAmount" UniqueName="OriginalAuthorizationAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource30">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderStyle-CssClass="text-center" DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource31" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>

                    <as:ASGridBoundColumn DataField="Approved" UniqueName="Approved" HeaderText="A/D"
                        HeaderTooltip="Approved/Declined" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ResponseCode" UniqueName="ResponseCode" HeaderText="RC"
                        HeaderTooltip="Response Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AVS" UniqueName="AVS" HeaderText="AVS" HeaderTooltip="Address Verification System"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource23">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="CVV" UniqueName="CVV" HeaderText="CVV" HeaderTooltip="Cardholder Verification Value"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource24">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationSource" UniqueName="AuthSource" HeaderText="Auth Source"
                        HeaderTooltip="Authorization Source" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="CustID" UniqueName="CustID" HeaderText="Cust ID"
                        HeaderTooltip="Customer ID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource26">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MOTO" UniqueName="MOTO" HeaderText="MOTO" HeaderTooltip="Mail/Telephone Order"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource27">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxDrilldownGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxDrilldownGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/AuthorizationLog.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
