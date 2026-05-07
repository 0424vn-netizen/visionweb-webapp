<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewRiskReport_TransactionHistory.ascx.cs"
    Inherits="UserControls_NewRiskTransactionHistory" %>
<%@ Register TagName="NewUxExport" Src="~/UserControls/RiskReport_UxExport.ascx" TagPrefix="uc" %>
<style>
    .sort_ipmt input[type=radio] {
        display: none;
    }
</style>
<tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxTransactionDetails">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxTransactionDetails" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxOrderBy1" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxOrderBy2" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxOrderBy3" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxTransDateRange">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxTransactionDetails" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxExportTransactionHistory" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxOrderBy1">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxTransactionDetails" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optAsc1" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optDesc1" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxOrderBy2">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxTransactionDetails" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optAsc2" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optDesc2" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxOrderBy3">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxTransactionDetails" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optAsc3" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optDesc3" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="optAsc1">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxTransactionDetails" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optAsc1" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optDesc1" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="optDesc1">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxTransactionDetails" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optAsc1" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optDesc1" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="optAsc2">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxTransactionDetails" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optAsc2" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optDesc2" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="optDesc2">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxTransactionDetails" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optAsc2" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optDesc2" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="optAsc3">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxTransactionDetails" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optAsc3" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optDesc3" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="optDesc3">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxTransactionDetails" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optAsc3" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="optDesc3" UpdatePanelRenderMode="Inline" LoadingPanelID="uxInvisiblePanel" />
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
        <uc:NewUxExport ID="uxExportTransactionHistory" runat="server" GridID="uxTransactionDetails" Visible="False" IsOnTop="true" GridTitle="Transaction History"
            ShowWord="false" ShowPDF="false" FileName="Risk Management - Risk Analysis - Transaction History" OnNeedExportConfig="uxExport_OnNeedExportConfig" meta:resourcekey="uxExportTransactionHistoryResource" />
    </div>
</div>
<div class="height-8"></div>
<div class="row">
    <div class="col-md-12">
        <div id="uxTransactionHistory" class="in">
            <table class="trans-filter-sort dark-blue w-100 ipmt">
                <tr class="sort_ipmt">
                    <td class="text-nowrap group-sort w-30">
                        <span class="label">
                            <as:Literal ID="Literal2" runat="server" Text="Sort 1:" meta:resourcekey="Literal2Resource1"></as:Literal></span>
                        <div class="control-inline orderby narrow  pull-left">
                            <tek:RadComboBox ID="uxOrderBy1" runat="server" Width="120px" OnClientSelectedIndexChanged="uxOrderBy_ClientSelectedIndexChanged"
                                EnableEmbeddedBaseStylesheet="false" AutoPostBack="true" OnSelectedIndexChanged="uxOrderBy_SelectedIndexChanged">
                            </tek:RadComboBox>
                        </div>
                        <div class="control-inline dark-blue radio-option">
                            <asp:RadioButton ID="optAsc1" runat="server" GroupName="OrderBy1" Text="Ascend" AutoPostBack="True" CssClass="ascendSort"
                                OnCheckedChanged="optAscDesc_CheckedChanged" />
                        </div>
                        <div class="control-inline dark-blue radio-option">
                            <asp:RadioButton ID="optDesc1" runat="server" GroupName="OrderBy1" Text="Descend" AutoPostBack="True" CssClass="descendSort"
                                OnCheckedChanged="optAscDesc_CheckedChanged" />
                        </div>
                    </td>
                    <td class="text-nowrap group-sort w-30">
                        <span class="label">
                            <as:Literal ID="Literal3" runat="server" Text="Sort 2:" meta:resourcekey="Literal3Resource1"></as:Literal>
                        </span>
                        <div class="control-inline orderby narrow pull-left">
                            <tek:RadComboBox ID="uxOrderBy2" runat="server" Width="120px" OnClientSelectedIndexChanged="uxOrderBy_ClientSelectedIndexChanged"
                                EnableEmbeddedBaseStylesheet="false" AutoPostBack="true" OnSelectedIndexChanged="uxOrderBy_SelectedIndexChanged">
                            </tek:RadComboBox>
                        </div>
                        <div class="control-inline dark-blue radio-option">
                            <as:RadioButton ID="optAsc2" runat="server" GroupName="OrderBy2" Text="Ascend" AutoPostBack="True" CssClass="ascendSort"
                                OnCheckedChanged="optAscDesc_CheckedChanged" Value="" />
                        </div>
                        <div class="control-inline dark-blue radio-option">
                            <as:RadioButton ID="optDesc2" runat="server" GroupName="OrderBy2" Text="Descend" AutoPostBack="True" CssClass="descendSort"
                                OnCheckedChanged="optAscDesc_CheckedChanged" Value="" />
                        </div>
                    </td>
                    <td class="text-nowrap group-sort w-30">
                        <span class="label text-nowrap">
                            <as:Literal ID="Literal4" runat="server" Text="Sort 3:" meta:resourcekey="Literal4Resource1"></as:Literal>
                        </span>
                        <div class="control-inline orderby narrow  pull-left">
                            <tek:RadComboBox ID="uxOrderBy3" runat="server" Width="120px"
                                EnableEmbeddedBaseStylesheet="false" AutoPostBack="true" OnSelectedIndexChanged="uxOrderBy_SelectedIndexChanged" OnClientSelectedIndexChanged="uxOrderBy_ClientSelectedIndexChanged">
                            </tek:RadComboBox>
                        </div>
                        <div class="control-inline dark-blue radio-option">
                            <asp:RadioButton ID="optAsc3" runat="server" GroupName="OrderBy3" Text="Ascend" AutoPostBack="True" CssClass="ascendSort"
                                OnCheckedChanged="optAscDesc_CheckedChanged" />
                        </div>
                        <div class="control-inline dark-blue radio-option">
                            <asp:RadioButton ID="optDesc3" runat="server" GroupName="OrderBy3" Text="Descend" AutoPostBack="True" CssClass="descendSort"
                                OnCheckedChanged="optAscDesc_CheckedChanged" />
                        </div>
                    </td>
                    <td class="text-nowrap text-right w-10">
                        <span class="label vertical-middle float-none">
                            <as:Literal ID="Literal1" runat="server" Text="Days:" meta:resourcekey="Literal1Resource1"></as:Literal>
                        </span>
                        <div class="control-inline last-item">
                            <tek:RadComboBox ID="uxTransDateRange" runat="server"
                                MarkFirstMatch="true" EnableEmbeddedBaseStylesheet="false" Width="60px" AutoPostBack="true"
                                OnSelectedIndexChanged="uxTransDateRange_SelectedIndexChanged">
                            </tek:RadComboBox>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="height-10"></div>

            <as:ASGrid ID="uxTransactionDetails" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                GridLines="None" AllowPaging="True" ASPagingMethod="SPASingleMethod1" ShowPageTotal="false"
                ShowReportTotal="false" AllowSorting="true" OnNeedDataSource="uxTransactionDetails_NeedDataSource"
                OnDataSourceReady="uxTransactionDetails_DataSourceReady" visiblereporttotal="true"
                visiblepagetotal="false" OnItemDataBound="uxTransactionDetails_ItemDataBound"
                XOverFlowable="true" HeaderStyle-Width="100px" IsAutoExportTemplate="true"
                OnSortCommand="uxTransactionDetails_SortCommand" CssClass="in" meta:resourcekey="uxTransactionDetailsResource1">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                            HeaderTooltip="Report Date" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransDate" UniqueName="TransDate"
                            HeaderTooltip="Transaction Date" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Card" DataField="CardType" UniqueName="CardType"
                            HeaderTooltip="Card Type Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Ctry" DataField="CountryCode" UniqueName="CountryCode"
                            HeaderTooltip="Country Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Acct Number" DataField="CardNumber" UniqueName="CardNumber" HeaderStyle-Width="160px"
                            SortExpression="PartialCardNumber" HeaderTooltip="Account Number" ASFormat="StaticString"
                            Visible="false" meta:resourcekey="ASGridBoundColumnResource5">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="160px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Acct Number" DataField="PartialCardNumber" HeaderStyle-Width="160px"
                            UniqueName="PartialCardNumber" SortExpression="PartialCardNumber" HeaderTooltip="Account Number"
                            ASFormat="StaticString" ItemStyle-Wrap="false" meta:resourcekey="ASGridBoundColumnResource6">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="160px"></HeaderStyle>

                            <ItemStyle Wrap="False"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Dupe" DataField="DupeCount" UniqueName="DupeCount" meta:resourcekey="ASGridBoundColumnResource22"
                            SortExpression="DupeCount" HeaderTooltip="Last 30 Days Count" ASFormat="Integer">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ExpirationDate" DataField="ExpirationDate" HeaderText="Expire"
                            HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="TransType" HeaderText="Type" DataField="TransType"
                            HeaderTooltip="Transaction Type" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="Amount" HeaderText="Amount" DataField="Amount"
                            HeaderTooltip="Amount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource9">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>

                        <as:ASGridBoundColumn UniqueName="MatchCode" HeaderText="Match" DataField="MatchCode"
                            HeaderTooltip="Match" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceMatch">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>

                        <as:ASGridBoundColumn UniqueName="ADF" HeaderText="A/D/F" DataField="ADF"
                            HeaderTooltip="Approved/Declined or Forced Sale" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource10">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ResponseCode" HeaderText="RC" DataField="ResponseCode"
                            HeaderTooltip="Response Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource11">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="AuthCode" HeaderText="Auth Code" DataField="AuthCode"
                            HeaderTooltip="Authorization Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="AVS" HeaderText="AVS" DataField="AVS"
                            HeaderTooltip="Address Vertification System" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="CVV" HeaderText="CVV" DataField="CVV"
                            HeaderTooltip="Cardholder Verification Value" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource14">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="KeyedEntry" HeaderText="Keyed" DataField="KeyedEntry" HeaderTooltip="KEYED or SWIPED"
                            ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource15">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="EMVIndicator" HeaderText="EMV" DataField="EMVIndicator" HeaderTooltip="EMV"
                            ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource21">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="Settled" HeaderText="Settled" DataField="SettleType"
                            HeaderTooltip="Settled" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource16">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="FileSource" HeaderText="File Source" DataField="FileSource" HeaderStyle-Width="200px" ItemStyle-CssClass="word-break"
                            HeaderTooltip="File Source" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource17">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="BatchCount" HeaderText="# Batch" DataField="BatchCnt"
                            HeaderTooltip="Batch Count" ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource18">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="BatchAmount" HeaderText="Batch $" DataField="BatchAmt"
                            HeaderTooltip="Batch Amount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource19">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="TerminalNumber" HeaderText="Terminal #" DataField="TerminalNumber"
                            HeaderTooltip="Terminal Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource20">
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


<tek:RadCodeBlock ID="radCodeBlock" runat="server">
    <script type="text/javascript">
        var optAsc1 = '<%= optAsc1.ClientID %>';
        var optDesc1 = '<%= optDesc1.ClientID %>';
        var optAsc2 = '<%= optAsc2.ClientID %>';
        var optDesc2 = '<%= optDesc2.ClientID %>';
        var optAsc3 = '<%= optAsc3.ClientID %>';
        var optDesc3 = '<%= optDesc3.ClientID %>';
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk/NewRiskReport_TransactionHistory.js">
    </script>
</tek:RadCodeBlock>