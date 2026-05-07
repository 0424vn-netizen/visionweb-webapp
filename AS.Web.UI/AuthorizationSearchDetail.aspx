<%@ Page Title="Authorizations Details" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="AuthorizationSearchDetail.aspx.cs" Inherits="AuthorizationSearchDetail" meta:resourcekey="PageResource1" %>


<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Authorizations Details" meta:resourcekey="uxPageTitleResource1" />
    <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" GridHeader="Authorizations Details" meta:resourcekey="uxExporterResource1" />
    <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true"
        AllowSorting="true" ASPagingMethod="SPASingleMethod" ShowPageTotal="false" CssClass="in" meta:resourcekey="uxReportGridResource1">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Merchant ID" HeaderTooltip="Merchant ID" UniqueName="MerchantNumber"
                    DataField="MerchantNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Merchant Name" HeaderTooltip="Merchant Name" UniqueName="MerchantName"
                    DataField="MerchantName" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Description" HeaderTooltip="Description" UniqueName="Description"
                    DataField="Description" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Auth ID" HeaderTooltip="Authorization ID" UniqueName="AuthID"
                    DataField="AuthID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
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
                <as:ASGridBoundColumn HeaderText="Trans Date" HeaderTooltip="Transaction Date" UniqueName="TransDate"
                    DataField="TransactionDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Trans Time" HeaderTooltip="Transaction Time" UniqueName="TransTime"
                    DataField="TransactionTime" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Exp Date" HeaderTooltip="Expiration Date" UniqueName="ExpDate"
                    DataField="ExpirationDate" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Auth Amt" HeaderTooltip="Authorization Amount"
                    UniqueName="AuthAmount" DataField="AuthorizationAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource10">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Netwk ID" HeaderTooltip="Network ID" UniqueName="NetwkID"
                    DataField="NetworkID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="AVS" HeaderTooltip="AVS Response Code"
                    UniqueName="AVS" DataField="AVS" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="Capture" meta:resourcekey="ASGridBoundColumnResource13">
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

