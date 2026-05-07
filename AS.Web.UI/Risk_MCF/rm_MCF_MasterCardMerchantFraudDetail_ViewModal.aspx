<%@ Page Title="MasterCard Merchant Fraud - Detail" Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MasterCardMerchantFraudDetail_ViewModal.aspx.cs" Inherits="rm_MCF_MasterCardMerchantFraudDetail_ViewModal" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    <tek:AjaxUpdatedControl ControlID="uxExport" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-xxl">
        <div style="margin-top: -20px">
            <uc:UxExport ID="uxExport"
                ShowExcel="true" ShowCSV="true" ShowPDF="false" ShowWord="false"
                GridID="uxReportGrid"
                GridTitle="MasterCard Merchant Fraud - Detail" runat="server"
                OnNeedExportConfig="uxExport_OnNeedExportConfig" />

            <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                GridLines="None" AllowPaging="True" ASPagingMethod="SPASingleMethod1" ShowPageTotal="false"
                ShowReportTotal="false"
                XOverFlowable="true" HeaderStyle-Width="100px" IsAutoExportTemplate="true" AllowSortFilterWhenExport="true"
                CssClass="in" meta:resourcekey="uxReportGridResource1"
                AllowSorting="true" AllowFilteringByColumn="true"
                OnItemDataBound="uxReportGrid_ItemDataBound"
                OnSortCommand="uxReportGrid_SortCommand"
                FilteringByColumnWithDataFieldAllow="true"
                FilteringByColumnWithDataFieldConfigFile="App_Data\FilteringByColumnWithDataFieldConfig\MasterCardMerchantFraudDetailGridConfig.json"
                BuildFilterExpressionWithSquareBrackets="true"
                EnableBuildFilterExpressionEnhancement="true"
                EnableFilterItemsPersistence="true"
                EnableSortItemsPersistence="true"
                OnPreRender="uxReportGrid_PreRender"
                OnDataSourceReady="uxReportGrid_DataSourceReady">
                <MasterTableView>
                    <Columns>
                        <as:ASGridTemplateColumn HeaderText="Transaction Date" HeaderTooltip="Transaction Date" DataField="TransactionDate" UniqueName="TransactionDate"
                            HeaderStyle-Width="130px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="Date"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="TransactionDate">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDate(Eval("TransactionDate")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Issuer" HeaderTooltip="Issuer" DataField="Issuer" UniqueName="Issuer"
                            HeaderStyle-Width="550px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="Issuer">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("Issuer")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Acquirer" HeaderTooltip="Acquirer" DataField="AcquirerID" UniqueName="AcquirerID"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="AcquirerID">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("AcquirerID")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Merchant ID" HeaderTooltip="Merchant ID" DataField="MerchantNumber" UniqueName="MerchantNumber"
                            HeaderStyle-Width="120px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="MerchantNumber">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("MerchantNumber")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Merchant Name" HeaderTooltip="Merchant Name" DataField="MerchantName" UniqueName="MerchantName"
                            HeaderStyle-Width="300px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="MerchantName">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("MerchantName")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Fraud Type" HeaderTooltip="Fraud Type" DataField="FraudType" UniqueName="FraudType"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="FraudType">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("FraudType")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Fraud Subtype" HeaderTooltip="Fraud Subtype" DataField="FraudSubType" UniqueName="FraudSubType"
                             HeaderStyle-Width="110px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="FraudSubType">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("FraudSubType")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Card Number" HeaderTooltip="Card Number" DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                            HeaderStyle-CssClass="PartialCardNumber"
                            HeaderStyle-Width="130px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="PartialCardNumber">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("PartialCardNumber")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Transaction Amount" HeaderTooltip="Transaction Amount" DataField="TransactionAmount" UniqueName="TransactionAmount"
                            HeaderStyle-Width="150px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="Currency"
                            ItemStyle-HorizontalAlign="Right"
                            SortExpression="TransactionAmount">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatCurrency(Eval("TransactionAmount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Post Date" HeaderTooltip="Post Date" DataField="PostDate" UniqueName="PostDate"
                            HeaderStyle-Width="130px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="Date"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="PostDate">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDate(Eval("PostDate")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Settle Date" HeaderTooltip="Settle Date" DataField="SettleDate" UniqueName="SettleDate"
                            HeaderStyle-Width="130px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="Date"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="SettleDate">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDate(Eval("SettleDate")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Fraud Report Date" HeaderTooltip="Fraud Report Date" DataField="FraudReportDate" UniqueName="FraudReportDate"
                            HeaderStyle-Width="140px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="Date"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="FraudReportDate">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDate(Eval("FraudReportDate")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Auth Response Code" HeaderTooltip="Auth Response Code" DataField="AuthResponseCode" UniqueName="AuthResponseCode"
                            HeaderStyle-Width="150px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="AuthResponseCode">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("AuthResponseCode")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="POS Entry Mode Code" HeaderTooltip="POS Entry Mode Code" DataField="POSEntryMode" UniqueName="POSEntryMode"
                            HeaderStyle-Width="155px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="POSEntryMode">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("POSEntryMode")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Terminal ID" HeaderTooltip="Terminal ID" DataField="TerminalID" UniqueName="TerminalID"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="TerminalID">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("TerminalID")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Terminal Capability Code" HeaderTooltip="Terminal Capability Code" DataField="TerminalCapability" UniqueName="TerminalCapability"
                            HeaderStyle-Width="180px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="TerminalCapability">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("TerminalCapability")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Acquirer Reference Number" HeaderTooltip="Acquirer Reference Number" DataField="AcquirerReferenceNumber" UniqueName="AcquirerReferenceNumber"
                            HeaderStyle-Width="190px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="AcquirerReferenceNumber">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("AcquirerReferenceNumber")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="CVC Code" HeaderTooltip="CVC Code" DataField="CVCCode" UniqueName="CVCCode"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="CVCCode">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("CVCCode")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Secure Code" HeaderTooltip="Secure Code" DataField="SecureCode" UniqueName="SecureCode"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="SecureCode">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("SecureCode")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Cardholder Presence Code" HeaderTooltip="Cardholder Presence Code" DataField="CardholderPresence" UniqueName="CardholderPresence"
                            HeaderStyle-Width="180px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="CardholderPresence">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("CardholderPresence")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </div>
    </as:ASModalContainer>
    <as:RadCodeBlock ID="JavaScript" runat="server">
        <script type="text/javascript">
            var uxReportGrid_ClientID = '<%=uxReportGrid.ClientID %>';
        </script>
        <script src="<%=ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_MasterCardMerchantFraudDetail.js"></script>
    </as:RadCodeBlock>
</asp:Content>
