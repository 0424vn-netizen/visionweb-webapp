<%@ Page Title="Portfolio Credits" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_PortfolioCredits.aspx.cs" Inherits="rm_MCF_PortfolioCredits" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxPortfolioGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxPortfolioGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxChangeMerchantWorked">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxChangeMerchantWorked" />
                    <%--<tek:AjaxUpdatedControl ControlID="uxPortfolioGrid" />--%>
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnChangeWorkedStatusOption">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="btnChangeWorkedStatusOption" />
                    <tek:AjaxUpdatedControl ControlID="uxPortfolioGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <div class="row collapse report-filter-panel">
        <div class="col-md-12 report-filter">
            <div class="filter-block">
                <as:Panel ID="uxFilteringTable" runat="server" Width="100%" meta:resourcekey="uxFilteringTableResource1">
                    <table>
                        <colgroup>
                            <col />
                            <col />
                            <col style="width: 30px" />
                        </colgroup>
                        <tr>
                            <td class="text-right">
                                <asp:Literal ID="LiteralDate" runat="server" meta:resourcekey="LiteralDateResource1"> Date:</asp:Literal>
                            </td>
                            <td class="text-left" colspan="2">
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxBeginDate" runat="server" Style="vertical-align: middle;" meta:resourcekey="uxBeginDateResource1">
                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" FastNavigationStep="12" ShowRowHeaders="False"></Calendar>

                                        <DateInput ID="DateInput2" runat="server" LabelWidth="64px" Width="">
                                            <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                            <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                            <FocusedStyle Resize="None"></FocusedStyle>

                                            <DisabledStyle Resize="None"></DisabledStyle>

                                            <InvalidStyle Resize="None"></InvalidStyle>

                                            <HoveredStyle Resize="None"></HoveredStyle>

                                            <EnabledStyle Resize="None"></EnabledStyle>
                                        </DateInput>

                                        <DatePopupButton ImageUrl="" HoverImageUrl="" CssClass=""></DatePopupButton>
                                    </as:RadDatePicker>
                                </div>
                                <div class="filter-item">
                                    <as:RadDatePicker ID="uxEndDate" runat="server" Style="vertical-align: middle;" meta:resourcekey="uxEndDateResource1">
                                        <Calendar>
                                        </Calendar>
                                        <DateInput ID="DateInput1" runat="server" LabelWidth="64px" Width="">
                                            <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                            <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                            <FocusedStyle Resize="None"></FocusedStyle>

                                            <DisabledStyle Resize="None"></DisabledStyle>

                                            <InvalidStyle Resize="None"></InvalidStyle>

                                            <HoveredStyle Resize="None"></HoveredStyle>

                                            <EnabledStyle Resize="None"></EnabledStyle>
                                        </DateInput>

                                        <DatePopupButton ImageUrl="" HoverImageUrl="" CssClass=""></DatePopupButton>
                                    </as:RadDatePicker>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-right">
                                <asp:Literal ID="Literal1" runat="server" meta:resourcekey="Literal1Resource1">Transaction Amount Less Than:</asp:Literal>
                                <span class="red">(</span>
                            </td>
                            <td class="text-left">
                                <div class="filter-item">
                                    <asp:TextBox ID="uxTransactionAmount" runat="server" Width="248px" Text="50" MaxLength="18" CssClass="red rf_TextBox" meta:resourcekey="uxTransactionAmountResource1"></asp:TextBox>
                                </div>
                            </td>
                            <td class="red text-left">
                                <span class="red">)<%=SessionManager.CurrencySymbol %></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-right">
                                <asp:Literal ID="Literal2" runat="server" meta:resourcekey="Literal2Resource1">Option:</asp:Literal>
                            </td>
                            <td class="text-left" colspan="2">
                                <as:RadComboBox ID="uxOption" runat="server" Filter="Contains" MarkFirstMatch="true" CssClass="filter-item" Width="248px" meta:resourcekey="uxOptionResource1">
                                    <Items>
                                        <as:RadComboBoxItem Text="All" meta:resourcekey="RadComboBoxItemResource1" />
                                        <as:RadComboBoxItem Text="Matched" meta:resourcekey="RadComboBoxItemResource2" />
                                        <as:RadComboBoxItem Text="Partial Match" meta:resourcekey="RadComboBoxItemResource3" />
                                        <as:RadComboBoxItem Text="Unmatched" meta:resourcekey="RadComboBoxItemResource4" />
                                    </Items>
                                </as:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="filter-item pull-right">
                                    <as:Button ID="uxSearch" runat="server" Text="Search" OnClientClick="return ValidateData();"
                                        OnClick="uxSearch_Click" CssClass="btn btn-default" meta:resourcekey="uxSearchResource1" />
                                </div>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </as:Panel>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter">
                <asp:Literal ID="Literal3" runat="server" meta:resourcekey="Literal3Resource1">FILTER</asp:Literal></span>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 no-margin-bottom">
            <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Portfolio Credits" meta:resourcekey="uxPageTitleResource1" />
        </div>
    </div>
    <!--OPTION: NOT WORKED, WORKED, ALL-->
    <div class="height-16"></div>
    <div runat="server" id="uxWorkedNotWorked" class="row">
        <div class="col-md-12 dark-blue">
            <label class="first">
                <asp:Literal ID="LiteralWorkStatus" runat="server" meta:resourcekey="LiteralWorkStatusResource1"> Review Status:</asp:Literal></label>
            <div class="control-inline">
                <as:RadioButton ID="optNotWorkedMerchant" runat="server" GroupName="WORKGROUP" Text="Not Reviewed"
                    onclick="ChangeWorkedStatusOption();" meta:resourcekey="optNotWorkedMerchantResource1" />
            </div>
            <div class="control-inline">
                <as:RadioButton ID="optWorkedMerchant" runat="server" GroupName="WORKGROUP" Text="Reviewed"
                    onclick="ChangeWorkedStatusOption();" meta:resourcekey="optWorkedMerchantResource1" />
            </div>
            <div class="control-inline">
                <as:RadioButton ID="optAllMerchant" runat="server" GroupName="WORKGROUP" Text="All"
                    onclick="ChangeWorkedStatusOption();" Checked="true" meta:resourcekey="optAllMerchantResource1" />
            </div>
        </div>
    </div>
    <as:Button ID="uxChangeMerchantWorked" runat="server" IsStandardButton="true" OnClick="uxChangeMerchantWorked_Click"
        CssClass="display-none" meta:resourcekey="uxChangeMerchantWorkedResource1" />
    <as:HiddenField ID="uxHiddenMerchantWorked" runat="server" />
    <as:Button ID="uxAccount" runat="server" IsStandardButton="true" OnClick="uxAccount_Click"
        CssClass="display-none" meta:resourcekey="uxAccountResource1" />
    <as:HiddenField ID="uxHdAccount" runat="server" />
    <as:HiddenField ID="uxHiddenMerchantNumber" runat="server" />
    <as:Button ID="btnChangeWorkedStatusOption" runat="server" Text="" OnClick="btnChangeWorkedStatusOption_Click"
        CssClass="display-none" IsStandardButton="true" meta:resourcekey="btnChangeWorkedStatusOptionResource1" />

    <as:PlaceHolder ID="plhContainer" runat="server">
        <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxPortfolioGrid" IsOnTop="true" ShowPDF="false"
            GridHeader="Risk Management - Risk Analysis - Portfolio Credits" meta:resourcekey="uxExporterTopResource1" />
        <as:ASGrid ID="uxPortfolioGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" IsAutoExportTemplate="true"
            AllowPaging="true" ASPagingMethod="SPASingleMethod" GridName="PorfolioCredits" CssClass="in" meta:resourcekey="uxPortfolioGridResource1">
            <MasterTableView>
                <Columns>

                    <as:ASGridTemplateColumn UniqueName="CheckBoxColumn" HeaderText=" " ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource1" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <span class="checkbox-middle">
                                <input type="checkbox" class="workedBox" id="cidMerchantWorked" runat="server" value='<%# Eval("RecordID") %>'
                                    onclick="return ChangeMerchantWorked(this);" checked='<%# Convert.ToBoolean(Eval("Worked")) %>' />
                            </span>
                        </ItemTemplate>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn DataField="ReportDate" UniqueName="ReportDate" HeaderText="Report Date"
                        HeaderTooltip="Report Date" ASFormat="Date" SortExpression="ReportDate" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MerchantNumber" UniqueName="MerchantNumber" HeaderText="Merchant ID"
                        ASFormat="StaticString" HeaderTooltip="Merchant ID" Visible="false" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MerchantName" UniqueName="MerchantName" HeaderText="Merchant Name"
                        ASFormat="DynamicString" HeaderTooltip="Merchant Name" SortExpression="MerchantName" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="CardTypeCode" HeaderText="Card Type" UniqueName="CardTypeCode"
                        HeaderTooltip="Card Type" ASFormat="StaticString" SortExpression="CardTypeCode"
                        HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Keyed" HeaderText="Keyed" UniqueName="Keyed" HeaderTooltip="Keyed"
                        ASFormat="StaticString" SortExpression="Keyed" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="MatchCode" HeaderText="Match to Sales" UniqueName="MatchCode"
                        HeaderTooltip="Type of Match to Sales" ASFormat="StaticString" SortExpression="MatchCode" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialAccountNumber" HeaderText="Card #" UniqueName="PartialAccountNumber" HeaderStyle-Width="140px"
                        HeaderTooltip="Card Number" ASFormat="StaticString" Visible="false" SortExpression="PartialAccountNumber"
                        ItemStyle-CssClass="cardNumber" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle CssClass="cardNumber"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AccountNumber" HeaderText="Card #" UniqueName="AccountNumber" HeaderStyle-Width="140px"
                        HeaderTooltip="Card Number" ASFormat="StaticString" SortExpression="PartialAccountNumber"
                        ItemStyle-CssClass="cardNumber" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle CssClass="cardNumber"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" UniqueName="FullCardNumber" HeaderTooltip="Card Number" HeaderStyle-Width="140px"
                        ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="BatchNumber" HeaderText="Batch #" UniqueName="BatchNumber"
                        HeaderTooltip="Batch Number" ASFormat="StaticString" SortExpression="BatchNumber" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="TransactionDate" UniqueName="TransactionDate" HeaderText="Transaction Date"
                        HeaderTooltip="Transaction Date" ASFormat="Date" SortExpression="TransactionDate" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="TransactionAmount" UniqueName="TransactionAmount"
                        HeaderText="Transaction Amount" HeaderTooltip="Transaction Amount" ASFormat="Currency"
                        SortExpression="TransactionAmount" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalTransactionAmount" UniqueName="OriginalTransactionAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                        ASFormat="Number2Digit" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderStyle-CssClass="text-center" DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource17" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="RecordID" UniqueName="RecordID" Visible="false" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="Worked" UniqueName="Worked" Display="false" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>

    <as:RadCodeBlock runat="server" ID="uxRadCodeBlock">
        <script type="text/javascript">
            var rm_PortfolioCredits_uxHiddenMerchantWorked = "<%=uxHiddenMerchantWorked.ClientID %>";
            var rm_PortfolioCredits_uxChangeMerchantWorked = "<%=uxChangeMerchantWorked.ClientID %>";
            var rm_PortfolioCredits_uxHdAccount = "<%=uxHdAccount.ClientID %>";
            var rm_PortfolioCredits_uxHiddenMerchantNumber = "<%=uxHiddenMerchantNumber.ClientID %>";
            var rm_PortfolioCredits_uxAccount = "<%=uxAccount.ClientID %>";
            var rm_PortfolioCredits_uxBeginDate = "<%=uxBeginDate.ClientID %>";
            var rm_PortfolioCredits_uxEndDate = "<%=uxEndDate.ClientID %>";
            var rm_PortfolioCredits_uxTransactionAmount = "<%=uxTransactionAmount.ClientID %>";
            var rm_PortfolioCredits_uxFilteringTable = "<%=uxFilteringTable.ClientID %>";
            var rm_PortfolioCredits_uxSearch = "<%=uxSearch.ClientID %>";
            var rm_PortfolioCredits_btnChangeWorkedStatusOption = "<%=btnChangeWorkedStatusOption.ClientID %>";

            var rm_PortfolioCredits_ValidationMessages_V2 = "<%=Resources.MessageManager.ValidationMessages_V2 %>";
            var rm_PortfolioCredits_ReportFilter_V3 = "<%=Resources.MessageManager.ReportFilter_V3 %>";
            var rm_PortfolioCredits_ValidationMessages_V1b = "<%=Resources.MessageManager.ValidationMessages_V1b%>";
            var rm_PortfolioCredits_ReportFilter_ReportDate_InvaidDate = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate%>";
            var rm_PortfolioCredits_js_String1 = '<%=GetLocalResourceObject("rm_PortfolioCredits_js_String1").ToString()%>';
            var rm_PortfolioCredits_js_String2 = '<%=GetLocalResourceObject("rm_PortfolioCredits_js_String2").ToString()%>';
            var rm_PortfolioCredits_js_String3 = '<%=GetLocalResourceObject("rm_PortfolioCredits_js_String3").ToString()%>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_PortfolioCredits.js"></script>
    </as:RadCodeBlock>
</asp:Content>
