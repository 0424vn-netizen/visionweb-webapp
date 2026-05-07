<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_NewRiskReportTransactionHistory.ascx.cs"
    Inherits="UserControls_rm_MCF_NewRiskReportTransactionHistory" %>
<%@ Register TagName="NewUxExport" Src="~/UserControls/rm_MCF_Report_UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxExportQueue" Src="~/UserControls/UxExportQueue.ascx" TagPrefix="uc" %>
<tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxTransactionDetails">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divTransHistory" LoadingPanelID="uxLoadingPanelCustom" />
                <tek:AjaxUpdatedControl ControlID="uxExportQueueTransactionHistory" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxTransDateRange">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divTransHistory" LoadingPanelID="uxLoadingPanelCustom" />
                <tek:AjaxUpdatedControl ControlID="uxExportTransactionHistory" LoadingPanelID="uxLoadingPanelCustom" />
                <tek:AjaxUpdatedControl ControlID="uxExportQueueTransactionHistory" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxViewColumns">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divTransHistory" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRebind">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divTransHistory" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
<div class="row ipmt">
    <div class="col-md-10">
        <h2 class="grid-title control-inline" id="uxTitle" runat="server" data-toggle="collapse" data-target="#uxTransactionHistory">
            <as:Literal ID="ltTransactionHistory" runat="server" Text="Transaction History" meta:resourcekey="ltTransactionHistoryResource1"></as:Literal>
        </h2>
        <asp:Literal ID="uxTransHistoryHeader" runat="server" meta:resourcekey="uxTransHistoryHeaderResource1"></asp:Literal>
    </div>
    <div class="col-md-2">
        <uc:NewUxExport ID="uxExportTransactionHistory" runat="server"
            GridID="uxTransactionDetails" Visible="False"
            IsOnTop="true" GridTitle="Transaction History"
            ShowWord="false" ShowPDF="false"
            ShowExcel="true" ShowCSV="true"
            FileName="Risk Management - Risk Analysis - Transaction History"
            OnNeedExportConfig="uxExport_OnNeedExportConfig"
            meta:resourcekey="uxExportTransactionHistoryResource" />
        <uc:UxExportQueue ID="uxExportQueueTransactionHistory" runat="server"
            GridID="uxTransactionDetails"
            ShowPDF="false"
            Visible="false"
            PageName="TransactionHistory"/>

    </div>
</div>
<div id="divTransHistory" runat="server">
    <div class="row ipmt mt-5x mb-24">
        <div class="col-md-4">
            <span class="title control-inline narrow">
                <as:Literal ID="Literal9" runat="server" Text="View:" meta:resourcekey="lblViewResource"></as:Literal></span>
            <div class="inline-block">
                <as:RadComboBox ID="uxViewColumns" Filter="Contains" MarkFirstMatch="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="uxViewColumns_SelectedIndexChanged"
                    DataValueField="CustomViewID" DataTextField="ViewName" Width="200px">
                </as:RadComboBox>
            </div>
            <b class="dark-blue ml-4x">
                <asp:LinkButton runat="server" ID="uxCustomizeColumnLink" CssClass="link-back" Text="Customize" meta:resourcekey="Literal16Resource1"></asp:LinkButton></b>
        </div>
        <div class="col-md-8 text-right text-dark-gray">

            <as:Literal ID="Literal1" runat="server" Text="Days:" meta:resourcekey="Literal1Resource1"></as:Literal>

            <div class="control-inline last-item">
                <tek:RadComboBox ID="uxTransDateRange" runat="server"
                    MarkFirstMatch="true" EnableEmbeddedBaseStylesheet="false" Width="60px" AutoPostBack="true"
                    OnSelectedIndexChanged="uxTransDateRange_SelectedIndexChanged">
                </tek:RadComboBox>
            </div>
        </div>
        <div style="display: none">
            <asp:Button runat="server" ID="btnRebind" OnClick="btnRebind_Click" />
        </div>
    </div>
    <div class="height-8"></div>
    <div class="row">
        <div class="col-md-12">
            <div id="uxTransactionHistory" class="in">
                <as:ASGrid ID="uxTransactionDetails" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                    GridLines="None" AllowPaging="True" ASPagingMethod="SPASingleMethod1" ShowPageTotal="false"
                    ShowReportTotal="false" AllowSorting="true" AllowFilteringByColumn="true"
                    FilteringByColumnWithDataFieldAllow="true"
                    FilteringByColumnWithDataFieldConfigFile="App_Data\FilteringByColumnWithDataFieldConfig\TransactionHistoryGridConfig.json"
                    BuildFilterExpressionWithSquareBrackets="true"
                    EnableBuildFilterExpressionEnhancement="true"
                    OnNeedDataSource="uxTransactionDetails_NeedDataSource"
                    OnDataSourceReady="uxTransactionDetails_DataSourceReady" visiblereporttotal="true"
                    visiblepagetotal="false" OnItemDataBound="uxTransactionDetails_ItemDataBound" OnPreRender="uxTransactionDetails_PreRender"
                    XOverFlowable="true" HeaderStyle-Width="100px" IsAutoExportTemplate="true" AllowSortFilterWhenExport="true"
                    EnableFilterItemsPersistence="true"
                    EnableSortItemsPersistence="true"
                    OnSortCommand="uxTransactionDetails_SortCommand" CssClass="in" meta:resourcekey="uxTransactionDetailsResource1">
                    <MasterTableView>
                        <Columns>
                            <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                                DataType="System.DateTime"
                                HeaderTooltip="Report Date" ASFormat="Date" meta:resourcekey="ReportDate">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransDate" UniqueName="TransDate"
                                DataType="System.DateTime"
                                HeaderTooltip="Transaction Date" ASFormat="Date" meta:resourcekey="TransDate">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Card" DataField="CardType" UniqueName="CardType"
                                HeaderTooltip="Card Type Code" ASFormat="StaticString" meta:resourcekey="CardType">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Ctry" DataField="CountryCode" UniqueName="CountryCode"
                                HeaderTooltip="Country Code" ASFormat="StaticString" meta:resourcekey="CountryCode">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Acct Number" DataField="CardNumber" UniqueName="CardNumber" HeaderStyle-Width="160px"
                                AllowFiltering="false"
                                SortExpression="PartialCardNumber" HeaderTooltip="Account Number" ASFormat="StaticString"
                                Visible="false" meta:resourcekey="CardNumber">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Width="160px"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Acct Number" DataField="PartialCardNumber" HeaderStyle-Width="160px"
                                AllowFiltering="false"
                                UniqueName="PartialCardNumber" SortExpression="PartialCardNumber" HeaderTooltip="Account Number"
                                ASFormat="StaticString" ItemStyle-Wrap="false" meta:resourcekey="PartialCardNumber">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Width="160px"></HeaderStyle>

                                <ItemStyle Wrap="False"></ItemStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="BIN" DataField="BinNumber" UniqueName="BinNumber"
                                HeaderTooltip="Bank Identification Number (six or eight digits)" ASFormat="StaticString" meta:resourcekey="BinNumber">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Dupe" DataField="DupeCount" UniqueName="DupeCount" meta:resourcekey="ASGridBoundColumnResource22"
                                SortExpression="DupeCount" HeaderTooltip="Last 30 Days Count" ASFormat="Integer" DataType="System.Int16">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="ExpirationDate" DataField="ExpirationDate" HeaderText="Expire"
                                AllowFiltering="false"
                                HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ExpirationDate">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="TransType" HeaderText="Type" DataField="TransType"
                                HeaderTooltip="Transaction Type" ASFormat="StaticString" meta:resourcekey="TransType">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="Amount" HeaderText="Amount" DataField="Amount"
                                HeaderTooltip="Amount" ASFormat="Currency" DataType="System.Decimal" meta:resourcekey="Amount">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn UniqueName="MatchCode" HeaderText="Match" DataField="MatchCode"
                                HeaderTooltip="Match" ASFormat="StaticString" meta:resourcekey="MatchCode">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn UniqueName="ADF" HeaderText="A/D/F" DataField="ADF"
                                HeaderTooltip="Approved/Declined or Forced Sale" ASFormat="StaticString" meta:resourcekey="ADF">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="ResponseCode" HeaderText="RC" DataField="ResponseCode"
                                HeaderTooltip="Response Code" ASFormat="StaticString" meta:resourcekey="ResponseCode">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="AuthCode" HeaderText="Auth Code" DataField="AuthCode"
                                HeaderTooltip="Authorization Code" ASFormat="StaticString" meta:resourcekey="AuthCode">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="AVS" HeaderText="AVS" DataField="AVS"
                                HeaderTooltip="Address Vertification System" ASFormat="StaticString" meta:resourcekey="AVS">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="CVV" HeaderText="CVV" DataField="CVV"
                                HeaderTooltip="Cardholder Verification Value" ASFormat="StaticString" meta:resourcekey="CVV">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="KeyedEntry" HeaderText="Keyed" DataField="KeyedEntry" HeaderTooltip="KEYED or SWIPED"
                                ASFormat="StaticString" meta:resourcekey="KeyedEntry">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="EMVIndicator" HeaderText="EMV" DataField="EMVIndicator" HeaderTooltip="EMV"
                                ASFormat="StaticString" meta:resourcekey="EMVIndicator">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="SettleType" HeaderText="Settled" DataField="SettleType"
                                HeaderTooltip="Settled" ASFormat="StaticString" meta:resourcekey="SettleType">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="FileSource" HeaderText="File Source" DataField="FileSource" HeaderStyle-Width="200px" ItemStyle-CssClass="word-break"
                                HeaderTooltip="File Source" ASFormat="DynamicString" meta:resourcekey="FileSource">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="TerminalNumber" HeaderText="Terminal #" DataField="TerminalNumber"
                                HeaderTooltip="Terminal Number" ASFormat="StaticString" meta:resourcekey="TerminalNumber">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <HeaderStyle Width="100px"></HeaderStyle>
                </as:ASGrid>

            </div>
        </div>
    </div>
</div>
<tek:RadCodeBlock ID="radCodeBlock" runat="server">
    <script type="text/javascript">
        var rm_Button_TransactionHistory_Rebind_ClientID = '<%= btnRebind.ClientID %>';
        var rm_TransactionHistory_Flaglink = '<%=FlagLink%>';
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_NewRiskReport_TransactionHistory.js">
    </script>
    <style>
        .sort_ipmt input[type=radio] {
            display: none;
        }

        a.rmLink {
            border-radius: unset !important;
        }

        .RadMenu .rmGroup:before {
            background-color: unset !important;
        }

        .RadMenu_Default .rmGroup .rmLink:hover {
            background-color: unset !important;
        }

        .RadMenu ul.rmGroup {
            top: 14px !important;
        }

        .RadGrid_Default .rgFilterActive {
            background: transparent url('<%=ResolveUrl("~/res/img/filter_ico_over.png") %>') center center no-repeat !important;
        }

        .RadMenu .rmGroup .rmText {
            margin: unset !important;
        }

        .RadMenu_Default .rmLink .rmText {
            margin: unset !important;
        }

        .RadMenu_Default .rmFirst {
            margin: unset !important;
        }
    </style>
</tek:RadCodeBlock>
