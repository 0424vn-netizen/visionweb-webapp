<%@ Page Title="Card History" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="CardHistoryModal.aspx.cs" Inherits="CardHistoryModal" meta:resourcekey="PageResource1" %>
<%@ Register Src="~/UserControls/CardHistoryNavigator.ascx" TagPrefix="uc" TagName="CardHistoryNavigator" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <%--<div class="mif-sidebar-container">
            <uc:CardHistoryNavigator runat="server" ID="CardHistoryNavigator" />
        </div>--%>
    <uc:PageTitle ID="uxReportTitle" runat="server" ReportTitle="Card #:" meta:resourcekey="uxReportTitleResource1" />
    <div runat="server" id="sdasdasdas">
        <a id="NavCardHistory" class="anchor-link-no-padding"></a>
        <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxReportGrid" GridTitle="Card History" meta:resourcekey="uxExporterTopResource1"
            IsOnTop="true" ShowExcel="true" ShowPDF="true" ShowWord="false" />
        <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" AllowFilteringByColumn="false" ShowFooter="true" HeaderStyle-Width="100px"
            ASPagingMethod="SPASingleMethod1" ShowPageTotal="false" VisiblePageTotal="false" AppendHeaderforPrinter="true" XOverFlowable="true"
            VisibleReportTotal="true" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxReportGridResource1"
            IsAutoExportTemplate="true">
            <MasterTableView>
                <PagerStyle AlwaysVisible="true" />
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" HeaderTooltip="Report Date"
                        UniqueName="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="MerchantNumber" HeaderTooltip="Merchant ID"
                        UniqueName="MerchantNumber" ASFormat="StaticString" HeaderStyle-Width="140px" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Merchant Name" DataField="MerchantName" HeaderTooltip="Merchant Name"
                        UniqueName="MerchantName" ASFormat="DynamicString" HeaderStyle-Width="250px" meta:resourcekey="ASGridBoundColumnResourceMerchantName">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="250px"></HeaderStyle>
                    </as:ASGridBoundColumn>
					
					<as:ASGridBoundColumn HeaderText=" Status" DataField="MerchantStatus" HeaderTooltip="Merchant Status as of Today"
                        UniqueName="MerchantStatus" ASFormat="StaticString" HeaderStyle-Width="120px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
					
                    <as:ASGridBoundColumn HeaderText="Batch #" DataField="BatchNumber" HeaderTooltip="Batch Number" HeaderStyle-Width="100px"
                        UniqueName="BatchNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" HeaderTooltip="Transaction Date"
                        UniqueName="TransDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Time" DataField="TransactionTime" HeaderTooltip="Transaction Time"
                        UniqueName="TransTime" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource57">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn HeaderText="Trans Code" DataField="TransactionDescription"
                        HeaderTooltip="Transaction Code" UniqueName="TransCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Keyed" DataField="KeyedEntry" HeaderTooltip="KEYED or SWIPED"
                        UniqueName="KeyedEntry" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="EMV" DataField="EMVIndicator" HeaderTooltip="EMV"
                        UniqueName="EMVIndicator" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource53">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardType" HeaderTooltip="CardType"
                        UniqueName="CardType" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                        HeaderText="Card #" HeaderTooltip="Card Number" SortExpression="PartialCardNumber"
                        Visible="false" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Auth #" DataField="AuthorizationNumber" HeaderTooltip="Auth Number"
                        UniqueName="AuthorizationNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Amount" DataField="TransactionAmount" HeaderTooltip="Transaction Amount"
                        UniqueName="TransAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource54">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalTransactionAmount" UniqueName="OriginalTransactionAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                        ASFormat="Number2Digit" meta:resourcekey="ASGridBoundColumnResource55">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address" HeaderStyle-CssClass="text-center"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource56" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="ForcedTrans" UniqueName="ForcedTrans" HeaderText="Forced Trans"
                        HeaderTooltip="Forced Transaction" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn DataField="ForeignCard" UniqueName="ForeignCard" HeaderText="Foreign Card"
                        HeaderTooltip="Foreign Card" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                    <as:ASGridBoundColumn DataField="Owner" UniqueName="Owner" HeaderText="Owner"
                        HeaderTooltip="Owner" ASFormat="StaticString" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourceOwner">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Contact" UniqueName="Contact" HeaderText="Contact"
                        HeaderTooltip="Contact" ASFormat="StaticString" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourceContact">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="City" UniqueName="City" HeaderText="City" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="City" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceCity">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="State" UniqueName="State" HeaderText="State" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="State" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceState">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Zip" UniqueName="Zip" HeaderText="Zip" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="Zip" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceZip">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Phone" UniqueName="Phone" HeaderText="Phone #"
                        HeaderTooltip="Phone #" ASFormat="Phone" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourcePhone">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </div>

    <div class="display-none">
        <as:Button runat="server" ID="uxShowAuth" OnClick="uxShowAuth_Click" meta:resourcekey="uxShowAuthResource1" />
        <as:Button runat="server" ID="uxShowModalDetail" OnClick="uxShowModalDetail_Click" meta:resourcekey="uxShowAuthResource1" />

    </div>
    <div id="uxPanelAuth" class="display-none">
        <a id="NavAuthorizationHistory" class="anchor-link-no-padding"></a>
        <uc:UxExport ID="uxExporterAuthTop" runat="server" GridID="uxAuthorizationGrid" GridTitle="Authorization History"
            meta:resourcekey="uxExporterAuthTopResource1" />
        <as:ASGrid ID="uxAuthorizationGrid" runat="server" GridLines="None" AllowPaging="True" HeaderStyle-Width="90px"
            AllowSorting="True" AutoGenerateColumns="False" AllowFilteringByColumn="false" AppendHeaderforPrinter="true" XOverFlowable="true"
            ShowFooter="true" ASPagingMethod="SPASingleMethod1" ShowPageTotal="false" VisiblePageTotal="false" IsAutoExportTemplate="true"
            VisibleReportTotal="true" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxAuthorizationGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn DataField="ReportDate" UniqueName="ReportDate" HeaderText="Report Date"
                        HeaderTooltip="Report Date" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MerchantNumber" UniqueName="MerchantNumber" HeaderText="Merchant ID" HeaderStyle-Width="120px"
                        HeaderTooltip="Merchant ID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MerchantName" UniqueName="MerchantName" HeaderText="Merchant Name" HeaderStyle-Width="250px"
                        HeaderTooltip="Merchant Name" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
					<as:ASGridBoundColumn HeaderText=" Status" DataField="MerchantStatus" HeaderTooltip="Merchant Status as of Today"
                        UniqueName="MerchantStatus" ASFormat="StaticString" HeaderStyle-Width="120px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" HeaderTooltip="Transaction Date"
                        UniqueName="TransDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Time" DataField="TransactionTime" HeaderTooltip="Transaction Time"
                        UniqueName="TransTime" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Code" DataField="TransCodeDescription" HeaderTooltip="Transaction Code"
                        UniqueName="TransCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource57">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn HeaderText="Keyed" DataField="KeyedEntry" HeaderTooltip="KEYED or SWIPED"
                        UniqueName="KeyedEntry" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="EMV" DataField="EMVIndicator" HeaderTooltip="EMV"
                        UniqueName="EMVIndicator" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource53">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardType" HeaderTooltip="Card Type"
                        UniqueName="CardType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                        HeaderText="Card #" HeaderTooltip="Card Number" SortExpression="PartialCardNumber"
                        Visible="false" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Exp Date" DataField="ExpirationDate" HeaderTooltip="Expiration Date"
                        UniqueName="ExpDate" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource23">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationNumber" UniqueName="AuthorizationNumber" HeaderStyle-Width="100px"
                        HeaderText="Auth #" HeaderTooltip="Authorization Number" SortExpression="AuthorizationNumber"
                        ASFormat="StaticString" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" meta:resourcekey="ASGridBoundColumnResource24">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationAmount" UniqueName="AuthAmount" HeaderText="Auth Amount" HeaderStyle-Width="100px"
                        HeaderTooltip="Authorization Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionAmount" UniqueName="TransactionAmount"
                        HeaderText="Trans Amount" HeaderTooltip="Transaction Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource26">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource54">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalAuthorizationAmount" UniqueName="OriginalAuthorizationAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource55">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address" HeaderStyle-CssClass="text-center"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource56" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="Approved" UniqueName="Approved" HeaderText="A/D"
                        HeaderTooltip="Approved/Declined" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource27">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ResponseCode" UniqueName="ResponseCode" HeaderText="RC"
                        HeaderTooltip="Response Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource28">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AVS" UniqueName="AVS" HeaderText="AVS" HeaderTooltip="Address Verification System"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource29">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="CVV" UniqueName="CVV" HeaderText="CVV" HeaderTooltip="Cardholder Verification Value"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource30">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationSource" UniqueName="AuthSource" HeaderText="Auth Source"
                        HeaderTooltip="Authorization Source" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource31">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="CustID" UniqueName="CustID" HeaderText="Cust ID"
                        HeaderTooltip="Customer ID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource32">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MOTO" UniqueName="MOTO" HeaderText="MOTO" HeaderTooltip="Mail/Telephone Order"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource33">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                    <as:ASGridBoundColumn DataField="Owner" UniqueName="Owner" HeaderText="Owner"
                        HeaderTooltip="Owner" ASFormat="StaticString" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourceOwner">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Contact" UniqueName="Contact" HeaderText="Contact"
                        HeaderTooltip="Contact" ASFormat="StaticString" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourceContact">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="City" UniqueName="City" HeaderText="City" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="City" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceCity">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="State" UniqueName="State" HeaderText="State" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="State" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceState">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Zip" UniqueName="Zip" HeaderText="Zip" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="Zip" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceZip">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Phone" UniqueName="Phone" HeaderText="Phone #"
                        HeaderTooltip="Phone #" ASFormat="Phone" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourcePhone">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </div>
    <div class="display-none">
        <as:Button runat="server" ID="uxShowChargeback" OnClick="uxShowChargeback_Click" meta:resourcekey="uxShowChargebackResource1" />
    </div>
    <div id="uxPanelChargeback" class="display-none">

        <a id="NavChargebackHistory" class="anchor-link-no-padding"></a>
        <uc:UxExport ID="uxExportChargebackTop" runat="server" GridID="uxChargebackReportGrid"
            GridTitle="Chargeback History" IsBottom="false" meta:resourcekey="uxExportChargebackTopResource1" />
        <as:ASGrid ID="uxChargebackReportGrid" runat="server" GridLines="None" IsAutoExportTemplate="true"
            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AllowFilteringByColumn="false" XOverFlowable="true"
            ShowFooter="true" ASPagingMethod="SPASingleMethod1" ShowPageTotal="false" VisiblePageTotal="false" AppendHeaderforPrinter="true"
            VisibleReportTotal="true" AllowSortFilterWhenExport="true" MasterTableView-TableLayout="Auto" CssClass="in" meta:resourcekey="uxChargebackReportGridResource1">
            <MasterTableView>
                <PagerStyle AlwaysVisible="true" />
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                        HeaderTooltip="Report Date" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource34">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="MerchantNumber" UniqueName="MerchantNumber"
                        HeaderTooltip="Merchant ID" ASFormat="StaticString" HeaderStyle-Width="140px" meta:resourcekey="ASGridBoundColumnResource35">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Merchant Name" DataField="MerchantName" UniqueName="MerchantName" HeaderStyle-Width="250px"
                        HeaderTooltip="Merchant Name" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource36">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
					<as:ASGridBoundColumn HeaderText=" Status" DataField="MerchantStatus" HeaderTooltip="Merchant Status as of Today"
                        UniqueName="MerchantStatus" ASFormat="StaticString" HeaderStyle-Width="120px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" UniqueName="TransactionDate"
                        HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource37">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource57" HeaderStyle-Width="80px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn HeaderText="Type" DataField="CardType" UniqueName="CardType"
                        SortExpression="CardType" HeaderTooltip="Card Type" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource38">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" UniqueName="PartialCardNumber" HeaderStyle-Width="100px"
                        HeaderTooltip="Card Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource39">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RC #" DataField="ReasonCode" UniqueName="ReasonCode" HeaderStyle-Width="80px"
                        HeaderTooltip="CB Reason Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource40">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Acquirer Reference #" DataField="ReferenceNumber" HeaderStyle-Width="185px" UniqueName="ReferenceNumber"
                        SortExpression="ReferenceNumber" HeaderTooltip="Acquirer Reference Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource41">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="CB Reference #" HeaderTooltip="CB Reference Number" HeaderStyle-Width="185px" DataField="ChargebackReferenceNumber"
                        UniqueName="ChargebackReferenceNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceCBReferenceNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="MCOMM Claim ID" HeaderStyle-Width="120px" HeaderTooltip="MCOMMClaimID" DataField="MCOMMClaimID"
                        UniqueName="MCOMMClaimID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceMCOMMClaimID">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="VROLCase #" HeaderStyle-Width="120px" HeaderTooltip="VROL Case Number" DataField="VROLCaseNumber"
                        UniqueName="VROLCaseNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceVROLCaseNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="RepresentedCBAmount" HeaderText="Represented CB Amount"
                        DataField="RepresentedCBAmount" ASIsTotalColumn="true" HeaderTooltip="Represented Chargeback Amount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource42" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionAmount" HeaderText="1st CB Amount" DataField="TransactionAmount" HeaderStyle-Width="100px"
                        ASIsTotalColumn="true" HeaderTooltip="1st CB Amount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource43">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource54" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalTransactionAmount" UniqueName="OriginalTransactionAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                        ASFormat="Number2Digit" meta:resourcekey="ASGridBoundColumnResource55" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderStyle-CssClass="text-center" DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource56" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>


                    <as:ASGridBoundColumn DataField="Owner" UniqueName="Owner" HeaderText="Owner"
                        HeaderTooltip="Owner" ASFormat="StaticString" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourceOwner">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Contact" UniqueName="Contact" HeaderText="Contact"
                        HeaderTooltip="Contact" ASFormat="StaticString" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourceContact">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="City" UniqueName="City" HeaderText="City" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="City" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceCity">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="State" UniqueName="State" HeaderText="State" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="State" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceState">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Zip" UniqueName="Zip" HeaderText="Zip" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="Zip" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceZip">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Phone" UniqueName="Phone" HeaderText="Phone #"
                        HeaderTooltip="Phone #" ASFormat="Phone" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourcePhone">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </div>

    <div class="display-none">
        <as:Button runat="server" ID="uxShowRetrieval" OnClick="uxShowRetrieval_Click" meta:resourcekey="uxShowRetrievalResource1" />
    </div>
    <div id="uxPanelRetrieval" class="display-none">
        <a id="NavRetrievalHistoty" class="anchor-link-no-padding"></a>
        <uc:UxExport ID="uxExporterRetrievalTop" runat="server" GridID="uxRetrievalsReportGrid"
            IsBottom="false" ShowCSV="true" ShowExcel="true" GridTitle="Retrieval History" meta:resourcekey="uxExporterRetrievalTopResource1" />
        <as:ASGrid ID="uxRetrievalsReportGrid" runat="server" GridLines="None" AppendHeaderforPrinter="true"
            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AllowFilteringByColumn="false" XOverFlowable="true"
            ShowFooter="true" ASPagingMethod="SPASingleMethod1" ShowPageTotal="false" VisiblePageTotal="false" IsAutoExportTemplate="true"
            VisibleReportTotal="true" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxRetrievalsReportGridResource1">
            <MasterTableView>
                <PagerStyle AlwaysVisible="true" />
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                        HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource44">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="MerchantNumber" UniqueName="MerchantNumber"
                        HeaderTooltip="Merchant ID" ASFormat="StaticString" HeaderStyle-Width="140px" meta:resourcekey="ASGridBoundColumnResource45">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Merchant Name" DataField="MerchantName" UniqueName="MerchantName" HeaderStyle-Width="250px"
                        HeaderTooltip="Merchant Name" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource46">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
					<as:ASGridBoundColumn HeaderText=" Status" DataField="MerchantStatus" HeaderTooltip="Merchant Status as of Today"
                        UniqueName="MerchantStatus" ASFormat="StaticString" HeaderStyle-Width="120px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" UniqueName="TransDate"
                        HeaderTooltip="Transaction Date" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource47">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Type" DataField="CardType" UniqueName="CardType"
                        HeaderTooltip="Card Type" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource48">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                        HeaderTooltip="Card Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource49">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Amount" DataField="TransactionAmount" HeaderStyle-Width="120px" UniqueName="TransactionAmount"
                        HeaderTooltip="Transaction Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource50">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RC #" DataField="ReasonCode" HeaderStyle-Width="100px" UniqueName="ReasonCode"
                        HeaderTooltip="Reason Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource51">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Acquirer Reference #" HeaderStyle-Width="185px" DataField="ReferenceNumber" UniqueName="ReferenceNumber"
                        HeaderTooltip="Acquirer Reference Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource52">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="MCOMM Claim ID" HeaderStyle-Width="120px" HeaderTooltip="MCOMMClaimID" DataField="MCOMMClaimID"
                        UniqueName="MCOMMClaimID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceMCOMMClaimID">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="VROLCase #" HeaderStyle-Width="120px" HeaderTooltip="VROL Case Number" DataField="VROLCaseNumber"
                        UniqueName="VROLCaseNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceVROLCaseNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn DataField="Owner" UniqueName="Owner" HeaderText="Owner"
                        HeaderTooltip="Owner" ASFormat="StaticString" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourceOwner">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Contact" UniqueName="Contact" HeaderText="Contact"
                        HeaderTooltip="Contact" ASFormat="StaticString" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourceContact">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="City" UniqueName="City" HeaderText="City" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="City" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceCity">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="State" UniqueName="State" HeaderText="State" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="State" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceState">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Zip" UniqueName="Zip" HeaderText="Zip" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="Zip" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceZip">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Phone" UniqueName="Phone" HeaderText="Phone #"
                        HeaderTooltip="Phone #" ASFormat="Phone" HeaderStyle-Width="150px" ItemStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResourcePhone">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </div>

    <as:HiddenField ID="hdShowDetailModal" runat="server" />


    <as:RadAjaxManagerProxy runat="server" ID="uxRadManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxAuthorizationGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxAuthorizationGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxChargebackReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxChargebackReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxRetrievalsReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxRetrievalsReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxShowAuth">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxAuthorizationGrid" LoadingPanelID="uxLoadingPanelCustom" />
                    <tek:AjaxUpdatedControl ControlID="uxExporterAuthTop" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxShowRetrieval">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxRetrievalsReportGrid" />
                    <tek:AjaxUpdatedControl ControlID="uxExporterRetrievalTop" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxShowChargeback">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxChargebackReportGrid" />
                    <tek:AjaxUpdatedControl ControlID="uxExportChargebackTop" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>

            <tek:AjaxSetting AjaxControlID="uxShowModalDetail">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxShowModalDetail" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <tek:RadCodeBlock ID="uxRadCodeBlock1" runat="server">
        <script type="text/javascript">
            var uxShowAuthID = "<%=uxShowAuth.ClientID %>";
            var uxShowChargebackID = "<%=uxShowChargeback.ClientID %>";
            var uxShowRetrievalID = "<%=uxShowRetrieval.ClientID %>";
            var uxPanelChargebackID = "uxPanelChargeback";
            var uxPanelRetrievalID = "uxPanelRetrieval";
            var uxShowModalDetailID = "<%=uxShowModalDetail.ClientID %>";
            var hdShowDetailModalID =  "<%=hdShowDetailModal.ClientID %>";
        </script>
        <script type="text/javascript" src="<% =ResolveUrl("~")%>res/js/CardHistoryModal.js"> </script>


    </tek:RadCodeBlock>
</asp:Content>
