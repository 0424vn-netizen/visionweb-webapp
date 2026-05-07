<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="AuthorizationDetailsModal.aspx.cs"
    Inherits="AuthorizationDetail" Title="AUTHORIZATION DETAILS" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl">
        <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxReportGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </tek:RadAjaxManagerProxy>
        <h3 class="modal-title">
            <as:Literal ID="uxMerchantText" runat="server" Text="Merchant:" meta:resourcekey="uxMerchantTextResource1"></as:Literal>&nbsp;
         <as:Literal ID="uxMerchantInfo" runat="server" meta:resourcekey="uxMerchantInfoResource1"></as:Literal>
        </h3>
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" ShowWord="false"
            IsOnTop="true" />
        <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AllowSortFilterWhenExport="true"
            AppendHeaderforPrinter="true" IsAutoExportTemplate="true"
            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" GridName="Authorization Details"
            VisiblePageTotal="false" ShowPageTotal="false" ShowFooter="true" ASPagingMethod="SPASingleMethod" XOverFlowable="true" HeaderStyle-Width="100px" CssClass="in"
            meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn UniqueName="ReportDate" HeaderText="Report Date" DataField="ReportDate"
                        HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="90px" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionDate" HeaderText="Trans Date" DataField="TransactionDate"
                        HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="90px" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionTime" HeaderText="Trans Time" DataField="TransactionTime"
                        HeaderTooltip="Transaction Time" SortExpression="TransactionTime" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionCode" HeaderText="Trans Code" DataField="TransCodeDescription"
                        HeaderTooltip="Transaction Code" SortExpression="TransCodeDescription" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource24">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn UniqueName="KeyedEntry" HeaderText="Keyed" DataField="KeyedEntry"
                        HeaderTooltip="KEYED or SWIPED" SortExpression="KeyedEntry" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="EMV" DataField="EMVIndicator" SortExpression="EMVIndicator"
                        UniqueName="EMVIndicator" HeaderTooltip="EMV" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="CardType" HeaderText="Card Type" DataField="CardType"
                        HeaderTooltip="Card Type" SortExpression="CardType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Card #"
                        HeaderTooltip="Card Number" SortExpression="PartialCardNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                        ItemStyle-Wrap="false" HeaderText="Card #" HeaderTooltip="Card Number" SortExpression="PartialCardNumber"
                        ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>

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
                    <as:ASGridBoundColumn UniqueName="ExpirationDate" HeaderText="Exp Date" DataField="ExpirationDate"
                        HeaderTooltip="Expired Date" SortExpression="ExpirationDate" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AuthorizationNumber" HeaderText="Auth #" DataField="AuthorizationNumber"
                        HeaderTooltip="Authorization Number" SortExpression="AuthorizationNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AuthorizationAmount" HeaderText="Auth Amount" ASIsTotalColumn="true" HeaderStyle-Width="120px"
                        DataField="AuthorizationAmount" HeaderTooltip="Authorization Amount" SortExpression="AuthorizationAmount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionAmount" HeaderText="Trans Amount" ASDefaultNullValue=""
                        ASIsTotalColumn="true" DataField="TransactionAmount" HeaderTooltip="Transaction Amount"
                        SortExpression="TransactionAmount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalAuthorizationAmount" UniqueName="OriginalAuthorizationAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderStyle-CssClass="text-center" DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource23" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn UniqueName="Approved" HeaderText="A/D" DataField="Approved"
                        HeaderTooltip="Approved/Declined" SortExpression="Approved" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ResponseCode" UniqueName="ResponseCode"
                        HeaderText="RC" HeaderTooltip="Response Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AVS" HeaderText="AVS" DataField="AVS" HeaderTooltip="Address Verification System"
                        SortExpression="AVS" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="CVV" HeaderText="CVV" DataField="CVV" HeaderTooltip="Cardholder Verification Value"
                        SortExpression="CVV" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AuthSource" HeaderText="Auth Source" DataField="AuthorizationSource"
                        HeaderTooltip="Authorization Source" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="CustID" HeaderText="Cust ID" DataField="CustID"
                        HeaderTooltip="Customer ID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" />
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="MOTO" HeaderText="MOTO" DataField="MOTO" HeaderTooltip="Mail/Telephone Order"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" />
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>

    </as:ASModalContainer>
</asp:Content>

