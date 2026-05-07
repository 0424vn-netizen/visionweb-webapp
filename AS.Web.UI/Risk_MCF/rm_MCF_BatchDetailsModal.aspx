<%@ Page Title="Batch Details" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_BatchDetailsModal.aspx.cs" Inherits="rm_MCF_BatchDetailsModal" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxBatchDetailGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxBatchDetailGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </tek:RadAjaxManagerProxy>
        <h3 class="modal-title">
            <as:Literal ID="rm_BatchDetailsModal_aspx" runat="server" meta:resourcekey="rm_BatchDetailsModal_aspxResource1">Merchant:</as:Literal>
            <as:Literal ID="uxMerchantInfo" runat="server" meta:resourcekey="uxMerchantInfoResource1"></as:Literal>
        </h3>
        <as:PlaceHolder ID="uxComboTerminalPlaceHolder" runat="server" Visible="false">
            <as:ASRadComboBox ID="uxComboTerminal" runat="server" Width="250px" AutoPostBack="true"
                OnSelectedIndexChanged="uxComboTerminal_SelectedIndexChanged" meta:resourcekey="uxComboTerminalResource1" />
        </as:PlaceHolder>
        <as:PlaceHolder ID="uxBatchModalPlaceHolder" runat="server">
            <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxBatchDetailGrid" IsOnTop="true" />
            <as:ASGrid ID="uxBatchDetailGrid" runat="server" AllowPaging="true" AutoGenerateColumns="false" IsAutoExportTemplate="true"
                ShowPageTotal="false" AllowSorting="true" ASPagingMethod="SPASingleMethod" PageSize="20"
                IsIntruder="true" IntruderSourceName="BatchDetail" CssClass="in" meta:resourcekey="uxBatchDetailGridResource1">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn DataField="TransactionDate" UniqueName="TransactionDate" HeaderText="Trans Date"
                            HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="TransCodeDescription" UniqueName="TransactionCode"
                            HeaderText="Trans Code" HeaderTooltip="Transaction Code" SortExpression="TransCodeDescription"
                            ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="TerminalNr" HeaderText="Terminal #" DataField="TerminalNumber" HeaderStyle-Width="150px"
                            HeaderTooltip="Terminal Number" SortExpression="TerminalNumber" ASFormat="StaticString"
                            Visible="false" meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="KeyedEntry" UniqueName="KeyedEntry" HeaderText="Keyed" HeaderTooltip="KEYED or SWIPED"
                            SortExpression="KeyedEntry" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="EMVIndicator" UniqueName="EMVIndicator" HeaderText="EMV" HeaderTooltip="EMV"
                            SortExpression="EMVIndicator" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource17">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="CardType" UniqueName="CardType" HeaderText="Card Type"
                            HeaderTooltip="Card Type Code" SortExpression="CardType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Card #"  HeaderStyle-Width="150px"
                            HeaderTooltip="Card Number" SortExpression="AccountNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="PartialAccountNumber" UniqueName="PartialAccountNumber" HeaderStyle-Width="150px"
                            HeaderText="Card #" Visible="false" meta:resourcekey="ASGridBoundColumnResource7">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ExpirationDate" DataField="ExpirationDate" HeaderText="Exp Date"
                            HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="AuthorizationNumber" UniqueName="AuthorizationNumber"
                            HeaderText="Auth #" HeaderTooltip="Authorization Number" SortExpression="AuthorizationNumber"
                            ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="TransactionAmount" UniqueName="TransactionAmount"
                            HeaderText="Trans Amount" HeaderTooltip="Transaction Amount" ASIsTotalColumn="true"
                            SortExpression="TransactionAmount" ASFormat="Currency" FooterStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource10">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <FooterStyle HorizontalAlign="Right"></FooterStyle>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="Voucher" UniqueName="Voucher" HeaderText="Voucher"
                            AllowSorting="false" HeaderTooltip="Transaction Voucher" meta:resourcekey="ASGridBoundColumnResource11">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </as:PlaceHolder>

        <as:PlaceHolder ID="uxCardSummary" runat="server" Visible="false">
            <uc:UxExport ID="uxExportCardSummaryTop" runat="server" GridID="uxCardMerchantGrid" />
            <as:ASGrid ID="uxCardMerchantGrid" runat="server" AutoGenerateColumns="false"
                IsIntruder="false" AllowSorting="True" ASPagingMethod="SPASingleMethod" AllowPaging="True"
                PageSize="10" ShowPageTotal="false" ShowReportTotal="true" meta:resourcekey="uxCardMerchantGridResource1">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn HeaderText="Card Type" HeaderTooltip="Card Type" DataField="CardDescription"
                            UniqueName="CardType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Terminal #" HeaderTooltip="Terminal Number" DataField="TerminalNumber"
                            UniqueName="TerminalNr" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource13">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Transaction Code" HeaderTooltip="Transaction Code"
                            DataField="TransactionDescription" UniqueName="TransactionCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource14">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Tran Cnt." HeaderTooltip="Transaction Count" DataField="TransactionCount"
                            UniqueName="TransactionCount" ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource15">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Total" HeaderTooltip="Total Transaction Amount"
                            DataField="NetAmount" UniqueName="TotalTransactionAmount" ASFormat="Currency"
                            ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource16">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </as:PlaceHolder>
    </as:ASModalContainer>
</asp:Content>
