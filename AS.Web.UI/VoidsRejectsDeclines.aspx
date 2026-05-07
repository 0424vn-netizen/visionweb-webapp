<%@ Page Title="Voids/Rejects/Declines" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="VoidsRejectsDeclines.aspx.cs" Inherits="VoidsRejects" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:ReportFiltering ID="uxReportFiltering" runat="server" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true"
        HasFilteringOption="true" />
    <as:PlaceHolder ID="uxDrilldownPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporterDrilldownTop" runat="server" GridID="uxDrilldownGrid"
            ShowWord="false" ShowPDF="true" IsOnTop="true" />
        <as:ASGrid ID="uxDrilldownGrid" runat="server" Width="100%" GridName="Voided/Rejected"
            AutoGenerateColumns="false" AppendHeaderforPrinter="true" IsAutoExportTemplate="true"
            ASPagingMethod="SPASingleMethod" AllowPaging="true" AllowSorting="true" ShowPageTotal="false" CssClass="in" meta:resourcekey="uxDrilldownGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="" DataField="Entity" UniqueName="DrilldownColumn"
                        SortExpression="Entity" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Entity Name" DataField="EntityName" UniqueName="EntityName"
                        SortExpression="EntityName" HeaderTooltip="Entity Name" ASFormat="DynamicString"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource2" >
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Trans" DataField="TransactionCount" SortExpression="TransactionCount"
                        UniqueName="TransactionCount" HeaderTooltip="Transaction Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Sales" DataField="SaleAmount" SortExpression="SaleAmount"
                        UniqueName="SaleAmount" HeaderTooltip="Sale Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Returns" DataField="ReturnAmount" SortExpression="ReturnAmount"
                        UniqueName="ReturnAmount" HeaderTooltip="Return Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net" DataField="NetAmount" SortExpression="NetAmount"
                        UniqueName="NetAmount" HeaderTooltip="Net Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Voids/Rejects" DataField="VoidRejectCount" SortExpression="VoidRejectCount"
                        UniqueName="VoidRejectCount" HeaderTooltip="Voids/Rejects Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Voids/Rejects" DataField="VoidRejectAmount" SortExpression="VoidRejectAmount"
                        UniqueName="VoidRejectAmount" HeaderTooltip="Voids/Rejects Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
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
        <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxReportGrid" IsOnTop="true"
            ShowWord="false" ShowPDF="true" />
        <tek:RadToolTipManager ID="uxToolTipMana" runat="server" AutoTooltipify="false" meta:resourcekey="uxToolTipManaResource1" />
        <as:ASGrid ID="uxReportGrid" runat="server" GridName="Voided/Rejected" ASPagingMethod="SPASingleMethod"
            AllowSorting="true" AllowPaging="true" Width="100%" AutoGenerateColumns="false" AppendHeaderforPrinter="true"
            ShowPageTotal="false" CssClass="in" meta:resourcekey="uxReportGridResource1" IsAutoExportTemplate="true">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" SortExpression="ReportDate"
                        UniqueName="ReportDate" ASFormat="Date" HeaderTooltip="Transaction Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" SortExpression="TransactionDate" HeaderStyle-Width="80px"
                        UniqueName="TransactionDate" ASFormat="Date" HeaderTooltip="Transaction Date" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Time" DataField="TransactionTime" SortExpression="TransactionTime" HeaderStyle-Width="80px"
                        UniqueName="TransactionTime" ASFormat="StaticString" HeaderTooltip="Transaction Time" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Code" DataField="TransactionDescription"
                        SortExpression="TransactionDescription" HeaderStyle-Width="80px"
                        UniqueName="TransactionCode" HeaderTooltip="Transaction Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource25" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn HeaderText="Batch #" DataField="BatchNumber" SortExpression="BatchNumber" HeaderStyle-Width="100px"
                        UniqueName="BatchNumber" HeaderTooltip="Batch Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Keyed" DataField="KeyedEntry" SortExpression="KeyedEntry" HeaderStyle-Width="100px"
                        UniqueName="KeyedEntry" HeaderTooltip="KEY OR SWIPED" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="EMVIndicator" UniqueName="EMVIndicator" HeaderText="EMV" HeaderStyle-Width="70px"
                        ASFormat="StaticString" HeaderTooltip="EMV" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardType" SortExpression="CardType"
                        UniqueName="CardType" HeaderTooltip="Card Type" ASFormat="StaticString" Visible="true"
                        HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Card #"
                        HeaderTooltip="Card Number" SortExpression="PartialCardNumber" ASFormat="StaticString"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource16" HeaderStyle-Width="140px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialCardNumber" UniqueName="PartialCardNumber" HeaderStyle-Width="140px"
                        ItemStyle-Wrap="false" HeaderText="Card #" HeaderTooltip="Card Number" SortExpression="PartialCardNumber"
                        ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Auth #" DataField="AuthorizationNumber" SortExpression="AuthorizationNumber" HeaderStyle-Width="80px"
                        UniqueName="AuthorizationNumber" HeaderTooltip="Authorization Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RC" DataField="ReasonCode" SortExpression="ReasonCode" HeaderStyle-Width="80px"
                        UniqueName="ReasonCode" HeaderTooltip="Reason Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RC" HeaderTooltip="Reason Code" DataField="ReasonCodeDescription" Visible="false"
                        UniqueName="ReasonCodeDescription" ASFormat="StaticString" SortExpression="ReasonCodeDescription" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Amount" DataField="TransactionAmount" SortExpression="TransactionAmount"
                        UniqueName="TransactionAmount" HeaderTooltip="Transaction Amount" ASFormat="Currency" HeaderStyle-Width="100px"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource22" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalTransactionAmount" UniqueName="OriginalTransactionAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                        ASFormat="Number2Digit" meta:resourcekey="ASGridBoundColumnResource23" HeaderStyle-Width="120px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderStyle-CssClass="text-center" DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource24" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
</asp:Content>
