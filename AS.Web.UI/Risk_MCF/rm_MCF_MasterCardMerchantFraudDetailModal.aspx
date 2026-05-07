<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_MasterCardMerchantFraudDetailModal.aspx.cs" Inherits="Risk_MCF_rm_MCF_MasterCardMerchantFraudDetailModal" meta:resourcekey="PageResource1"%>

<%--head--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<%@ Import Namespace="AS.Common.Utilities" %>

<%@ Register Src="~/UserControls/rm_MCF_CustomListBox.ascx" TagName="RiskCustomListBox" TagPrefix="uc" %>

<%--content--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <as:PlaceHolder ID="uxPlActivationReport" runat="server">
                    <uc:UxExport ID="uxExporter" runat="server" ShowPDF="false" IsOnTop="true" GridID="uxReportGridM" />
                    <div class="height-20"></div>
                    <as:ASGrid ID="uxReportGridM" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" IsCacheTemplateFile="false" IsAutoExportTemplate="true"
                        AllowSorting="true" AllowExportAtWebServices="true" AllowSortFilterWhenExport="true"
                        AllowFilteringByColumn="true" OnNeedDataSource="uxReportGrid_NeedDataSource"
                        ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxReportGridResource1">
                        <MasterTableView>
                            <Columns>
                                <as:ASGridBoundColumn UniqueName="Acquirer" DataField="Acquirer" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="Acquirer" meta:resourcekey="GridColumnAcquirer">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="Issuer" DataField="Issuer" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="Issuer" meta:resourcekey="GridColumnIssuer">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="MerchantID" DataField="MerchantID" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="MerchantID" meta:resourcekey="GridColumnMerchantID">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="MerchantName" DataField="MerchantName" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="MerchantName" meta:resourcekey="GridColumnMerchantName">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="TransactionAmount" DataField="TransactionAmount" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="TransactionAmount" meta:resourcekey="GridColumnTransactionAmount">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="TransactionDate" DataField="TransactionDate" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="TransactionDate" meta:resourcekey="GridColumnTransactionDate">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="PostDate" DataField="PostDate" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="PostDate" meta:resourcekey="GridColumnPostDate">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="SettleDate" DataField="SettleDate" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="SettleDate" meta:resourcekey="GridColumnSettleDate">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="FraudReportDate" DataField="FraudReportDate" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="FraudReportDate" meta:resourcekey="GridColumnFraudReportDate">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="AccountNumber" DataField="AccountNumber" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="AccountNumber" meta:resourcekey="GridColumnAccountNumber">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="FraudType" DataField="FraudType" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="FraudType" meta:resourcekey="GridColumnFraudType">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="FraudSubType" DataField="FraudSubType" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="FraudSubType" meta:resourcekey="GridColumnFraudSubType">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="AuthResponseCode" DataField="AuthResponseCode" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="AuthResponseCode" meta:resourcekey="GridColumnAuthResponseCode">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="POSEntryModeCode" DataField="POSEntryModeCode" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="POSEntryModeCode" meta:resourcekey="GridColumnPOSEntryModeCode">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="TerminalID" DataField="TerminalID" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="TerminalID" meta:resourcekey="GridColumnTerminalID">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="TerminalCapCode" DataField="TerminalCapCode" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="TerminalCapCode" meta:resourcekey="GridColumnTerminalCapCode">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="AcquirerRefNumber" DataField="AcquirerRefNumber" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="AcquirerRefNumber" meta:resourcekey="GridColumnAcquirerRefNumber">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="CVCCode" DataField="CVCCode" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="CVCCode" meta:resourcekey="GridColumnCVCCode">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="SecureCode" DataField="SecureCode" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="SecureCode" meta:resourcekey="GridColumnSecureCode">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn UniqueName="CardholderPresenceCode" DataField="CardholderPresenceCode" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                                    AllowEncodeOnExporting="true" ASFormat="DynamicString" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="CardholderPresenceCode" meta:resourcekey="GridColumnCardholderPresenceCode">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </as:ASGrid>
                </as:PlaceHolder>
            </div>
        </div>
    </as:ASModalContainer>

</asp:Content>
