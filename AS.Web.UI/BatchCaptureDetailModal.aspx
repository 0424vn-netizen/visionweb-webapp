<%@ Page Title="Batch Capture Details" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="BatchCaptureDetailModal.aspx.cs" Inherits="BatchCaptureDetailModal" meta:resourcekey="PageResource1" %>


<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Batch Capture Details" meta:resourcekey="uxPageTitleResource1" />
    <div style="font-weight: bold; text-align: left; padding-left: 4px;">
        <as:Literal ID="ltMerchant" runat="server" Text="Merchant:" meta:resourcekey="ltMerchantResource1"></as:Literal>
        <as:Literal ID="uxMerchantInfo" runat="server" meta:resourcekey="uxMerchantInfoResource1"></as:Literal>
    </div>
    <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" GridHeader="" />

    <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true"
        AllowSorting="true" ASPagingMethod="SPASingleMethod" ShowPageTotal="false" CssClass="in" meta:resourcekey="uxReportGridResource1">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Card Type" HeaderTooltip="Card Type" UniqueName="CardType"
                    DataField="CardType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="Card #" HeaderTooltip="Card Number" UniqueName="AccountNumber"
                    DataField="AccountNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Card #" HeaderTooltip="Card Number" UniqueName="PartialCardNumber"
                    DataField="PartialCardNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Exp Date" HeaderTooltip="Expiration Date" UniqueName="ExpDate"
                    DataField="ExpirationDate" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Entry Mode" HeaderTooltip="Entry Mode" UniqueName="EntryMode"
                    DataField="EntryMode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Trans Type" HeaderTooltip="Transaction Type" UniqueName="TransType"
                    DataField="TransactionType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Description" HeaderTooltip="Description" UniqueName="Description"
                    DataField="Description" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Trans Date" HeaderTooltip="Transaction Date" UniqueName="TransDate"
                    DataField="TransactionDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Trans Time" HeaderTooltip="Transaction Time" UniqueName="TransTime"
                    DataField="TransactionTime" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Trans Amt" HeaderTooltip="Transaction Amount"
                    UniqueName="TransAmount" DataField="TransactionAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource10">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Auth ID" HeaderTooltip="Authorization ID" UniqueName="AuthID"
                    DataField="AuthorizationNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Term ID" HeaderTooltip="Terminal ID" UniqueName="TermID"
                    DataField="TerminalNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Mkt Code" HeaderTooltip="Market Data Code" UniqueName="MktCode"
                    DataField="MarketCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Error Code" HeaderTooltip="Error Code" UniqueName="ErrorCode"
                    DataField="ErrorCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource14">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="MOTO/EC Ind" HeaderTooltip="MOTO/EC Ind" UniqueName="MOTO"
                    DataField="MOTO" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource15">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
    <uc:UxExport ID="uxExporterBottom" runat="server" GridID="uxReportGrid" IsBottom="true" />
</asp:Content>

