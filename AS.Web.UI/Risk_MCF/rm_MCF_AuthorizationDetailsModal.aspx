<%@ Page Title="Authorization Details" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_AuthorizationDetailsModal.aspx.cs" Inherits="rm_MCF_AuthorizationDetailsModal" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" Width="1200px" ContainerCssClass="container" WidthCssClass="modal-xxl">
        <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxReportGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </tek:RadAjaxManagerProxy>
        <h3 class="modal-title"><asp:Literal ID="rm_AuthorizationDetailsModal_aspx" runat="server" meta:resourcekey="rm_AuthorizationDetailsModal_aspxResource1"> Merchant:</asp:Literal>
        <as:Literal ID="uxMerchantInfo" runat="server" meta:resourcekey="uxMerchantInfoResource1"></as:Literal>
        </h3>
        <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxReportGrid" IsOnTop="true" ShowPDF="false" />
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="False" ShowHeader="true" IsAutoExportTemplate="true"
            PageSize="20" AllowSorting="true" AllowPaging="true" ShowPageTotal="false"   HeaderStyle-Width="90px"
            ASPagingMethod="SPASingleMethod" MasterTableView-TableLayout="Auto" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn UniqueName="ReportDate" HeaderText="Report Date" DataField="ReportDate"
                        HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionDate" HeaderText="Trans Date" DataField="TransactionDate"
                        HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date"
                        meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionCode" HeaderText="Trans Code" DataField="TransactionDescription" 
                        HeaderTooltip="Transaction Code" SortExpression="TransactionDescription" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="KeyedEntry" HeaderText="Keyed" DataField="KeyedEntry" HeaderTooltip="KEYED or SWIPED"
                        SortExpression="KeyedEntry" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EMVIndicator" HeaderText="EMV" DataField="EMVIndicator"
                        HeaderTooltip="EMV" SortExpression="EMVIndicator" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="CardType" HeaderText="Card Type" DataField="CardType"
                        HeaderTooltip="Card Type Code" SortExpression="CardType" ASFormat="StaticString"
                        meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AccountNumber" HeaderText="Card #" DataField="AccountNumber"  HeaderStyle-Width="140px"
                        HeaderTooltip="Card Number" SortExpression="AccountNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="PartialAccountNumber" HeaderText="Card #" DataField="PartialAccountNumber"  HeaderStyle-Width="140px"
                        HeaderTooltip="Card Number" SortExpression="PartialAccountNumber" ASFormat="StaticString"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ExpirationDate" HeaderText="Exp Date" DataField="ExpirationDate"
                        HeaderTooltip="Expired Date" SortExpression="ExpirationDate" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AuthorizationNumber" HeaderText="Auth #" DataField="AuthorizationNumber" HeaderStyle-Width="100px"
                        HeaderTooltip="Authorization Number" SortExpression="AuthorizationNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AuthorizationAmount" HeaderText="Auth Amount" Visible="false"
                        ASIsTotalColumn="true" DataField="AuthorizationAmount" HeaderTooltip="Authorization Amount"
                        SortExpression="AuthorizationAmount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="TransactionAmount" HeaderText="Trans Amount" ASIsTotalColumn="true"
                        DataField="TransactionAmount" HeaderTooltip="Transaction Amount" SortExpression="TransactionAmount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AVS" HeaderText="AVS" DataField="AVS" HeaderTooltip="Address Verification System"
                        SortExpression="AVS" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="CVV" HeaderText="CVV" DataField="CVV" HeaderTooltip="Cardholder Verification Value"
                        SortExpression="CVV" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="AuthSource" HeaderText="Auth Source" DataField="AuthorizationSource"
                        HeaderTooltip="Authorization Source" HeaderStyle-Width="100px"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="CustID" HeaderText="Cust ID" DataField="CustID"
                        HeaderTooltip="Customer ID"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="MOTO" HeaderText="MOTO" DataField="MOTO" HeaderTooltip="Mail/Telephone Order"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:ASModalContainer>
</asp:Content>
