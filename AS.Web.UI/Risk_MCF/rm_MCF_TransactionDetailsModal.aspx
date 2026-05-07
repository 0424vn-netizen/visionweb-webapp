<%@ Page Title="TRANSACTION DETAILS" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_TransactionDetailsModal.aspx.cs" Inherits="rm_MCF_TransactionDetailsModal" meta:resourcekey="PageResource1" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:UxExport ID="uxExportTop" runat="server" GridID="uxTransactionDetails" IsOnTop="true" />
    <as:ASGrid ID="uxTransactionDetails" runat="server" AutoGenerateColumns="False" ShowHeader="true" IsAutoExportTemplate="true"
        AllowSorting="true" AllowPaging="true" ShowPageTotal="false" ASPagingMethod="SPASingleMethod" 
        MasterTableView-TableLayout="Auto" CssClass="in" meta:resourcekey="uxTransactionDetailsResource1">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Account #" DataField="FullAccountNumber" UniqueName="FullAccountNumber" HeaderStyle-Width="150px"
                    HeaderTooltip="Account Number" ASFormat="StaticString" SortExpression="AccountNumber" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Account #" DataField="AccountNumber" UniqueName="AccountNumber" HeaderStyle-Width="150px"
                    HeaderTooltip="Account Number" ASFormat="StaticString" SortExpression="AccountNumber" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardTypeCode" SortExpression="CardTypeCode"
                    UniqueName="CardTypeCode" ASFormat="StaticString" HeaderTooltip="Card Type Code"
                    HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Ctry" DataField="CountryCode" SortExpression="CountryCode"
                    UniqueName="CountryCode" ASFormat="StaticString" HeaderTooltip="Country Code" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Source" DataField="FileSource" SortExpression="FileSource" HeaderStyle-Width="200px" ItemStyle-CssClass="word-break"
                    UniqueName="FileSource" ASFormat="StaticString" HeaderTooltip="File Source" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" SortExpression="TransactionDate"
                    UniqueName="TransactionDate" ASFormat="Date" HeaderTooltip="Transaction Date"
                    HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Dupe" DataField="DupeCount" SortExpression="DupeCount" 
                    UniqueName="DupeCount" ASFormat="Integer" HeaderTooltip="Last 30 Days Count" meta:resourcekey="ASGridBoundColumnResource14">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="Auth #" DataField="AuthorizationNumber" SortExpression="AuthorizationNumber"
                    UniqueName="AuthorizationNumber" HeaderTooltip="Authorization Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Keyed" DataField="KeyedEntry" SortExpression="KeyedEntry"
                    UniqueName="KeyedEntry" HeaderTooltip="KEYED or SWIPED" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="EMV" DataField="EMVIndicator" SortExpression="EMVIndicator"
                    UniqueName="EMVIndicator" HeaderTooltip="EMV" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Trans Type" DataField="TransShortDescription"
                    UniqueName="TransactionCode" HeaderTooltip="Transaction Type" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Match to Sales" DataField="MatchCode" SortExpression="MatchCode"
                    UniqueName="MatchedFlag" HeaderTooltip="Type of Match to Sales" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource10">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Amount" DataField="TransactionAmount" SortExpression="TransactionAmount"
                    UniqueName="TransactionAmount" HeaderTooltip="Transaction Amount" ASFormat="Currency" HeaderStyle-Width="100"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="TopAmount" UniqueName="TopAmount" Visible="false" meta:resourcekey="ASGridBoundColumnResource12">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
    <as:ASRadCodeBlock ID="uxRadCode" runat="server">
        <script type="text/javascript">
            var IsIEBrowser = "<%=GeneralFuncsLib.GetIEBrowserMode().ToString()%>";
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_TransactionDetailsModal.js"></script>
    </as:ASRadCodeBlock>
</asp:Content>
