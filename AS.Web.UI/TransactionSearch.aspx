<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TransactionSearch.aspx.cs" Inherits="TransactionSearch" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExportTop" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxReportTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/TransactionSearch_Filtering.ascx" TagName="TransSearchFilter"
    TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <script src="res/js/jquery/jquery.format.1.05.js" type="text/javascript"></script>
    <tek:RadAjaxManagerProxy runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <!--Filtering Options-->
    <div class="row collapse report-filter-panel">
        <div class="col-md-12 report-filter trans-filter">
            <div class="filter-block text-right">
                <table>
                    <colgroup>
                        <col style="width: 277px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="text-right">
                            <div class="filter-item text-nowrap">
                                <asp:RadioButton ID="uxDaily" runat="server" Text="Daily" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxDailyResource1" />
                                <asp:RadioButton ID="uxMonthly" runat="server" Text="Monthly" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxMonthlyResource1" />
                                <asp:RadioButton ID="uxRange" runat="server" Text="Date Range" GroupName="Date" onclick="ChangeDateOption(this)"
                                    onkeypress="javascript:return false;" CssClass="date-item" meta:resourcekey="uxRangeResource1" />
                            </div>
                        </td>
                        <td colspan="2" class="text-left">
                            <div id="divDate">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxDate" ShowPopupOnFocus="true" runat="server" Width="128px">
                                        <Calendar ID="Calendar1" FastNavigationStep="12" ShowRowHeaders="false" runat="server" />
                                        <DateInput ID="DateInput1" runat="server" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                            <div id="divDateRange" style="display: none">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxFromDate" ShowPopupOnFocus="true" runat="server" Width="128px">
                                        <Calendar ID="Calendar2" FastNavigationStep="12" ShowRowHeaders="false" runat="server" />
                                        <DateInput ID="DateInput2" runat="server" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxEndDate" ShowPopupOnFocus="true" runat="server" Width="128px">
                                        <Calendar ID="Calendar3" FastNavigationStep="12" ShowRowHeaders="false" runat="server" />
                                        <DateInput ID="DateInput3" runat="server" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <div id="js-TransactionFilter" class="filter-block border">
                    <div class="titlebox">
                        <as:Literal ID="ltChooseatleastone" runat="server" Text="Choose at least one" meta:resourcekey="ltChooseatleastoneResource1"></as:Literal>
                    </div>
                    <table>
                        <as:PlaceHolder ID="phdMerchantLevel2" runat="server">

                            <uc:TransSearchFilter ID="uxFilterOption" runat="server" OnDoSearch="uxFilterOption_DoSearch" />

                        </as:PlaceHolder>
                        <asp:PlaceHolder ID="phdMerchantLevel1" runat="server">
                            <tr id="cidFull">
                                <td class="text-right">
                                    <label for="txtFullCard">
                                        <as:Literal ID="ltFullCardNumber" runat="server" Text="Full Card Number:" meta:resourcekey="ltFullCardNumberResource1"></as:Literal></label>
                                </td>
                                <td class="text-left">
                                    <div class="filter-item">
                                        <as:TextBox ID="txtFullCard" runat="server" MaxLength="20" CssClass="rf_TextBox"
                                            onkeypress="return SearchEnterOnTextbox(event);"
                                            Width="420px" meta:resourcekey="txtFullCardResource1"></as:TextBox>
                                    </div>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="uxCardNumberPanel" runat="server">
                            <tr id="cidPartial">
                                <td class="text-right">
                                    <label class="filter-item" for="uxFirst6">
                                        <as:Literal ID="ltCardFirst6" runat="server" meta:resourcekey="ltCardFirst6Resource1"></as:Literal></label>
                                </td>
                                <td class="text-left">
                                    <div class="filter-item ">
                                        <as:TextBox ID="uxFirst6" runat="server" MaxLength="8" CssClass="rf_TextBox"
                                            onkeypress="return SearchEnterOnTextbox(event);"
                                            Width="130px" meta:resourcekey="uxFirst6Resource1"></as:TextBox>
                                    </div>
                                    <div class="filter-item pull-right">
                                        <label for="uxLast4">
                                            <as:Literal ID="ltAndOr" runat="server" Text="AND / OR    (Last 4):" meta:resourcekey="ltAndOrResource1"></as:Literal></label>
                                        <as:TextBox ID="uxLast4" runat="server" MaxLength="4" CssClass="rf_TextBox"
                                            onkeypress="return SearchEnterOnTextbox(event);"
                                            Width="130px" meta:resourcekey="uxLast4Resource1"></as:TextBox>
                                    </div>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="uxRoutingAccountPanel" runat="server" Visible="false">
                            <tr id="cidRoutingAccountPartial">
                                <td class="text-right">
                                    <label class="filter-item" for="uxFirst6">
                                        <as:Literal ID="Literal1" runat="server" Text="Routing Number (First 6):" meta:resourcekey="ltRoutingFirst6Resource1"></as:Literal></label>
                                </td>
                                <td class="text-left">
                                    <div class="filter-item ">
                                        <as:TextBox ID="txtRouting" runat="server" MaxLength="6" CssClass="rf_TextBox"
                                            onkeypress="return SearchEnterOnTextbox(event);"
                                            Width="130px" meta:resourcekey="uxFirst6Resource1"></as:TextBox>
                                    </div>
                                    <div class="filter-item pull-right">
                                        <label for="txtAccount">
                                            <as:Literal ID="Literal2" runat="server" Text="AND / OR  Account Number  (Last 4):" meta:resourcekey="ltAccountResource1"></as:Literal></label>
                                        <as:TextBox ID="txtAccount" runat="server" MaxLength="4" CssClass="rf_TextBox"
                                            onkeypress="return SearchEnterOnTextbox(event);"
                                            Width="130px" meta:resourcekey="uxLast4Resource1"></as:TextBox>
                                    </div>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td class="text-right">
                                <label class="filter-item" for="uxAuthNumber">
                                    <as:Literal ID="ltAuthNumber" runat="server" Text="Authorization Number:" meta:resourcekey="ltAuthNumberResource1"></as:Literal></label>
                            </td>
                            <td class="text-left">
                                <div class="filter-item">
                                    <as:TextBox ID="uxAuthNumber" runat="server" MaxLength="16" CssClass="rf_TextBox"
                                        onkeypress="return SearchEnterOnTextbox(event);" Width="420px" HintCss="hint" meta:resourcekey="uxAuthNumberResource1"></as:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-right">
                                <label class="filter-item" for="uxTransAmount">
                                    <as:Literal ID="ltTransAmount" runat="server" Text="Transaction Amount:" meta:resourcekey="ltTransAmountResource1"></as:Literal></label>
                            </td>
                            <td class="text-left">
                                <div class="filter-item">
                                    <tek:RadComboBox ID="uxTransAmount" OnClientSelectedIndexChanged="transAmount_OnClientSelectedIndexChanged"
                                        runat="server" Width="120px">
                                        <Items>
                                            <as:RadComboBoxItem Text="Equal To" meta:resourcekey="radcomboboxEqualTo" Value="EqualTo" />
                                            <as:RadComboBoxItem Text="Between" meta:resourcekey="radcomboboxBetween" Value="Between" />
                                            <as:RadComboBoxItem Text="Greater Than" meta:resourcekey="radcomboboxGreaterThan" Value="GreaterThan" />
                                            <as:RadComboBoxItem Text="Less Than" meta:resourcekey="radcomboboxLessThan" Value="LessThan" />
                                            <as:RadComboBoxItem Text="+/- $5.00" meta:resourcekey="radcomboboxPlusMinus5" Value="PlusMinus5" />
                                        </Items>
                                    </tek:RadComboBox>
                                </div>
                                <div class="filter-item">
                                    <tek:RadNumericTextBox ID="uxTransAmountFrom" runat="server" Type="Currency"
                                        NegativeStyle-ForeColor="Red" CssClass="rf_TextBox trans-amount form-control"
                                        Width="120px">
                                        <ClientEvents OnKeyPress="SearchEnterOnRadNumeric" />
                                    </tek:RadNumericTextBox>
                                </div>
                                <label class="option filter-label">
                                    <as:Literal ID="ltAnd" runat="server" Text="AND" meta:resourcekey="ltAndResource1"></as:Literal></label>
                                <div class="filter-item pull-right">
                                    <tek:RadNumericTextBox ID="uxTransAmountTo" runat="server" Type="Currency"
                                        NegativeStyle-ForeColor="Red" CssClass="rf_TextBox trans-amount form-control"
                                        Width="119px">
                                        <ClientEvents OnKeyPress="SearchEnterOnRadNumeric" />
                                    </tek:RadNumericTextBox>
                                </div>
                            </td>

                        </tr>

                    </table>

                </div>
                <table>
                    <colgroup>
                        <col style="width: 344px" />
                        <col style="width: 362px" />
                    </colgroup>
                    <tr>
                        <td></td>
                        <td class="text-right">
                            <div class="filter-item">
                                <asp:Button ID="uxSearch" runat="server" Text="Search" OnClientClick="return ValidateData();"
                                    OnClick="uxSearch_Click" ValidationGroup="ValidatePage_cs" CssClass="btn btn-default" meta:resourcekey="uxSearchResource1" />
                            </div>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter">
                <as:Literal ID="ltFILTER" runat="server" Text="FILTER" meta:resourcekey="ltFILTERResource1"></as:Literal></span>
        </div>
    </div>
    <div class="row row-table">
        <div class="col-md-12 no-margin-bottom">
            <uc:UxReportTitle ID="uxReportTitle1" runat="server" />
            <uc:UxReportTitle ID="uxReportTitle" runat="server" ReportTitle="Transaction Search"
                HasFilteringOption="true" HasShowHierarchy="false" meta:resourcekey="uxReportTitleResource1" />
        </div>
    </div>
    <div class="height-24"></div>
    <div>
        <uc:UxExportTop ID="uxExporter" runat="server" GridID="uxReportGrid" IsOnTop="true" OnNeedExportConfig="uxExporter_NeedExportConfig" />
        <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" XOverFlowable="true" AppendHeaderforPrinter="true"
            GridName="Transaction Search List" AllowPaging="True" AllowSorting="True" Visible="true"
            AutoGenerateColumns="false" VisiblePageTotal="false" VisibleReportTotal="true" HeaderStyle-Width="80px"
            ASPagingMethod="SPASingleMethod" AllowFilteringByColumn="false" ShowFooter="true" ItemStyle-CssClass="word-break"
            AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxReportGridResource1" IsAutoExportTemplate="true" AllowExportAtWebServices="true">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn DataField="MerchantNumber" UniqueName="MerchantNumber" HeaderText="Merchant ID"
                        HeaderTooltip="Merchant ID" ASFormat="StaticString" SortExpression="MerchantNumber" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MerchantName" UniqueName="MerchantName" HeaderText="Merchant Name" HeaderStyle-Width="150px"
                        HeaderTooltip="Merchant Name" ASFormat="DynamicString" SortExpression="MerchantName" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ReportDate" UniqueName="ReportDate" HeaderText="Report Date"
                        HeaderTooltip="Report Date" ASFormat="Date" SortExpression="ReportDate" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="BatchNumber" UniqueName="BatchNumber" HeaderText="Batch #"
                        HeaderTooltip="Batch Number" ASFormat="StaticString" ItemStyle-Width="100px" SortExpression="BatchNumber" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>

                        <ItemStyle Width="95px"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionDate" UniqueName="TransactionDate" HeaderText="Trans Date"
                        HeaderTooltip="Transaction Date" ASFormat="Date" SortExpression="TransactionDate" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionTime" UniqueName="TransactionTime" HeaderText="Trans Time"
                        HeaderTooltip="Transaction Time" ASFormat="Auto" SortExpression="TransactionTime" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="CardType" UniqueName="CardType" HeaderText="Card Type"
                        HeaderTooltip="Card Type" ASFormat="StaticString" SortExpression="CardType" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Ctry" DataField="CountryCode" UniqueName="CountryCode"
                        HeaderTooltip="Country Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource23">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Card #" HeaderStyle-Width="140px"
                        HeaderTooltip="Card Number" ASFormat="StaticString" SortExpression="AccountNumber" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialCardNumber" UniqueName="PartialCardNumber" HeaderStyle-Width="140px"
                        HeaderText="Card #" HeaderTooltip="Card Number" ASFormat="StaticString" Visible="false"
                        SortExpression="PartialCardNumber" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Dupe" DataField="DupeCount" UniqueName="DupeCount" meta:resourcekey="ASGridBoundColumnResource24"
                        SortExpression="DupeCount" HeaderTooltip="Last 30 Days Count" ASFormat="Integer">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ExpirationDate" DataField="ExpirationDate" HeaderText="Exp Date"
                        HeaderTooltip="Expiration Date" ASFormat="StaticString" SortExpression="ExpirationDate" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ShortDescription" UniqueName="TransactionCode" HeaderText="Trans Code"
                        HeaderTooltip="Transaction Code" ASFormat="StaticString" SortExpression="ShortDescription" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                        <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                     <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="TransactionAmount" UniqueName="TransactionAmount" HeaderStyle-Width="120px"
                        HeaderText="Trans Amount" HeaderTooltip="Transaction Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" SortExpression="TransactionAmount" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MatchCode" UniqueName="MatchCode" HeaderText="Matched"
                        HeaderTooltip="Credit Matched to Previous Sale" ASFormat="StaticString" SortExpression="MatchCode" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn><as:ASGridBoundColumn UniqueName="ADF" HeaderText="A/D/F" DataField="ADF"
                        HeaderTooltip="Approved/Declined or Forced Sale" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AuthorizationNumber" UniqueName="AuthorizationNumber"
                        HeaderText="Auth #" HeaderTooltip="Authorization Number" ASFormat="StaticString"
                        SortExpression="AuthorizationNumber" ItemStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Width="80px"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AVS" HeaderText="AVS" DataField="AVS"
                        HeaderTooltip="Address Vertification System" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource27">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="KeyedEntry" UniqueName="KeyedEntry" HeaderText="Keyed" HeaderTooltip="KEYED or SWIPED"
                        SortExpression="KeyedEntry" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="EMV" DataField="EMVIndicator" HeaderTooltip="EMV"
                            UniqueName="EMVIndicator" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Settled" HeaderText="Settled" DataField="Settled"
                        HeaderTooltip="Settled" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource29">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="FileSource" UniqueName="FileSource" HeaderText="File Source" HeaderStyle-Width="200px" ItemStyle-CssClass="word-break"
                        HeaderTooltip="File Source" ASFormat="StaticString" SortExpression="FileSource" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TerminalNumber" UniqueName="TerminalNumber" HeaderText="Terminal #" HeaderStyle-Width="130px"
                        HeaderTooltip="Terminal Number" ASFormat="StaticString" SortExpression="TerminalNumber" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="RoutingAccountNumber" UniqueName="RoutingAccountNumber" HeaderStyle-Width="140px"
                        ASFormat="StaticString" SortExpression="RoutingAccountNumber" meta:resourcekey="RoutingAccountNumber" Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialRoutingACC" UniqueName="PartialRoutingACC" HeaderStyle-Width="140px"
                        ASFormat="StaticString" Visible="false"
                        SortExpression="PartialRoutingACC" meta:resourcekey="RoutingAccountNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                     <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource19" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalTransactionAmount" UniqueName="OriginalTransactionAmount" HeaderText="Base Currency Amount" 
                        HeaderTooltip="Base Currency Amount" SortExpression="OriginalTransactionAmount" ASFormat="Number2Digit" meta:resourcekey="ASGridBoundColumnResource20" HeaderStyle-Width="100px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderStyle-CssClass="text-center" DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource21" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <as:ASRadToolTip ID="tltDupeCount" meta:resourcekey="ASGridBoundColumnResource24" RenderMode="Lightweight" Position="BottomCenter" RelativeTo="Element"
            TargetControlID="Account Used Count" runat="server" IsClientID="true" RenderInPageRoot="true" CssClass="RadTooltip" AutoCloseDelay="0">
        </as:ASRadToolTip>
    </div>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var TransactionSearch_txtFullCard = "<%= txtFullCard.ClientID %>";

            var TransactionSearch_uxLast4 = '<%= uxLast4.ClientID %>';
            var TransactionSearch_uxFirst6 = '<%= uxFirst6.ClientID %>';
            var TransactionSearch_uxFromDate = '<%= uxFromDate.ClientID %>';
            var TransactionSearch_uxEndDate = '<%= uxEndDate.ClientID %>';
            var TransactionSearch_uxDate = '<%= uxDate.ClientID %>';
            var TransactionSearch_uxRange = '<%= uxRange.ClientID %>';
            var TransactionSearch_uxDaily = '<%= uxDaily.ClientID %>';
            var TransactionSearch_uxMonthly = '<%= uxMonthly.ClientID %>';
            var TransactionSearch_uxAuthNumber = '<%= uxAuthNumber.ClientID %>';
            var TransactionSearch_uxFilterOption = '<%= uxFilterOption.ClientID %>';


            var TransactionSearch_uxTransAmountFrom = '<%= uxTransAmountFrom.ClientID %>';
            var TransactionSearch_uxTransAmountTo = '<%= uxTransAmountTo.ClientID %>';
            var TransactionSearch_uxTransOperators = '<%= uxTransAmount.ClientID %>';

            var TransactionSearch_uxSearch = '<%= uxSearch.ClientID %>';

            var TransactionSearch_ReportFilter_V1 = '<%=Resources.MessageManager.ReportFilter_V1%>';
            var TransactionSearch_ReportFilter_V9 = '<%=Resources.MessageManager.ReportFilter_V9%>';
            var TransactionSearch_ReportFilter_V6 = '<%=Resources.MessageManager.ReportFilter_V6%>';
            var TransactionSearch_ReportFilter_V8 = '<%=Resources.MessageManager.ReportFilter_V8%>';
            var TransactionSearch_ReportFilter_V3 = '<%=Resources.MessageManager.ReportFilter_V3%>';
            var TransactionSearch_ReportFilter_V2 = '<%=Resources.MessageManager.ReportFilter_V2%>';
            var TransactionSearch_ReportFilter_V21 = '<%=Resources.MessageManager.ReportFilter_V21%>';
            var TransactionSearch_ReportFilter_V7 = '<%=Resources.MessageManager.ReportFilter_V7 %>';
            var TransactionSearch_ReportFilter_V4 = '<%=Resources.MessageManager.ReportFilter_V4%>';
            var TransactionSearch_Generic_FromGreaterTo = '<%=Resources.MessageManager.Generic_FromGreaterTo%>';
            var TransactionSearch_ValidationMessages_V10 = "<%=Resources.MessageManager.ValidationMessages_V10 %>";
            var TransactionSearch_ReportFilter_ReportDate_InvaidDate = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>";
            var TransactionSearch_FILTERING_OPTIONS_DATERANGE = '<%= GeneralFuncsLib.HasExtendedSetting("FILTERING_OPTIONS_DATERANGE") %>';

            var TransactionSearch_js_Mode_IsMerchant = '<%= (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant) ? true : false %>';

            var TransactionSearch_js_Alert1 = '<%= GetLocalResourceObject("TransactionSearch_js_Alert1").ToString()%>';
            var TransactionSearch_js_Alert2 = '<%= GetLocalResourceObject("TransactionSearch_js_Alert2").ToString()%>';
            var TransactionSearch_js_Alert3 = '<%= GetLocalResourceObject("TransactionSearch_js_Alert3").ToString()%>';
            var TransactionSearch_js_Alert4 = '<%= GeneralFuncsLib.Show_RoutingAccountNumber ? GetLocalResourceObject("TransactionSearch_js_Alert4_Routing").ToString() : GetLocalResourceObject("TransactionSearch_js_Alert4").ToString()%>';
            var TransactionSearch_js_Alert5 = '<%= GeneralFuncsLib.Show_RoutingAccountNumber ? GetLocalResourceObject("TransactionSearch_js_Alert5_Routing").ToString() : GetLocalResourceObject("TransactionSearch_js_Alert5").ToString()%>';
            var TransactionSearch_js_Alert6 = '<%= GeneralFuncsLib.Show_RoutingAccountNumber ? GetLocalResourceObject("TransactionSearch_js_Alert6_Routing").ToString() : GetLocalResourceObject("TransactionSearch_js_Alert6").ToString()%>';
            var TransactionSearch_js_Alert7 = '<%= GeneralFuncsLib.Show_RoutingAccountNumber ? GetLocalResourceObject("TransactionSearch_js_Alert7_Routing").ToString() : GetLocalResourceObject("TransactionSearch_js_Alert7").ToString() %>';
            var TransactionSearch_js_Daily = '<%= GetLocalResourceObject("uxDailyResource1.Text").ToString()%>';
            var TransactionSearch_js_Monthly = '<%= GetLocalResourceObject("uxMonthlyResource1.Text").ToString()%>';
            var TransactionSearch_js_Range = '<%= GetLocalResourceObject("uxRangeResource1.Text").ToString()%>';
            var TransactionSearch_js_FromDate = '<%= GetLocalResourceObject("TransactionSearch_js_FromDate").ToString()%>';
            var TransactionSearch_js_ToDate = '<%= GetLocalResourceObject("TransactionSearch_js_ToDate").ToString()%>';
            var TransactionSearch_js_FCN = '<%= GetLocalResourceObject("ltFullCardNumberResource1.Text").ToString()%>';
            var TransactionSearch_js_TheLengthMustBe12_20 = '<%= GetLocalResourceObject("TransactionSearch_js_TheLengthMustBe12_20").ToString()%>';
            var TransactionSearch_js_CardNumberF6L4 = '<%= GetLocalResourceObject("TransactionSearch_js_CardNumberF6L4").ToString()%>';
            var TransactionSearch_js_CardFirst6 = '<%= GetLocalResourceObject("ltCardFirst6Resource1.Text").ToString()%>';
            var TransactionSearch_js_CardNumberL4 = '<%= GetLocalResourceObject("TransactionSearch_js_CardNumberL4").ToString()%>';
            var TransactionSearch_js_TransactionAmountTo = '<%= GetLocalResourceObject("TransactionSearch_js_TransactionAmountTo").ToString()%>';
            var TransactionSearch_js_TransactionAmountFrom = '<%= GetLocalResourceObject("TransactionSearch_js_TransactionAmountFrom").ToString()%>';
            var TransactionSearch_js_AuthNumber = '<%= GetLocalResourceObject("ltAuthNumberResource1.Text").ToString()%>';
            var IsMSSystems = '<%= SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS %>';
            var TransactionSearch_NegativePattern = '<%= string.Format("({0}n)", SessionManager.CurrencySymbol)%>';

            var TransactionSearch_txtRouting = '<%= txtRouting.ClientID %>';
            var TransactionSearch_txtAccount = '<%= txtAccount.ClientID %>';
            var TransactionSearch_js_RoutingNumberF6L4 = '<%= GetLocalResourceObject("TransactionSearch_js_RoutingNumberF6L4").ToString()%>';
            var TransactionSearch_js_RoutingFirst6 = '<%= GetLocalResourceObject("ltRoutingFirst6Resource1.Text").ToString()%>';
            var TransactionSearch_js_AccountNumberL4 = '<%= GetLocalResourceObject("TransactionSearch_js_AccountNumberL4").ToString()%>';
            var isRouting = '<%= GeneralFuncsLib.Show_RoutingAccountNumber %>' == "True";
            var reportFilter_CardNumberLength = '<%=Resources.MessageManager.ReportFilter_CardNumberLength%>';
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/TransactionSearch.js"></script>
    </tek:RadCodeBlock>

</asp:Content>

