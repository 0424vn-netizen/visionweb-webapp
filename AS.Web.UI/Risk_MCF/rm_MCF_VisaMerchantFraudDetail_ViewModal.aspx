<%@ Page Title="Visa Merchant Fraud - Detail" Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_VisaMerchantFraudDetail_ViewModal.aspx.cs" Inherits="rm_MCF_VisaMerchantFraudDetail_ViewModal" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" %>

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
                GridTitle="Visa Merchant Fraud - Detail" runat="server"
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
                FilteringByColumnWithDataFieldConfigFile="App_Data\FilteringByColumnWithDataFieldConfig\VisaMerchantFraudDetailGridConfig.json"
                BuildFilterExpressionWithSquareBrackets="true"
                EnableBuildFilterExpressionEnhancement="true"
                EnableFilterItemsPersistence="true"
                EnableSortItemsPersistence="true"
                OnPreRender="uxReportGrid_PreRender"
                OnDataSourceReady="uxReportGrid_DataSourceReady">
                <MasterTableView>
                    <Columns>
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

                        <as:ASGridTemplateColumn HeaderText="Fraud Amount" HeaderTooltip="Fraud Amount" DataField="FraudAmount" UniqueName="FraudAmount"
                            HeaderStyle-Width="140px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="Currency"
                            ItemStyle-HorizontalAlign="Right"
                            SortExpression="FraudAmount">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatCurrency(Eval("FraudAmount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Purchase Date" HeaderTooltip="Purchase Date" DataField="PurchaseDate" UniqueName="PurchaseDate"
                            HeaderStyle-Width="130px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="Date"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="PurchaseDate">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDate(Eval("PurchaseDate")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Fraud Post Date" HeaderTooltip="Fraud Post Date" DataField="FraudPostDate" UniqueName="FraudPostDate"
                            HeaderStyle-Width="130px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="Date"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="FraudPostDate">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDate(Eval("FraudPostDate")) %>' runat="server"></asp:Literal>
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

                        <as:ASGridTemplateColumn HeaderText="Fraud Type" HeaderTooltip="Fraud Type" DataField="FraudType" UniqueName="FraudType"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="FraudType">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("FraudType")) %>' runat="server"></asp:Literal>
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

                        <as:ASGridTemplateColumn HeaderText="ECI/MOTO" HeaderTooltip="ECI/MOTO" DataField="ECIMOTO" UniqueName="ECIMOTO"                            
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="ECIMOTO">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("ECIMOTO")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="POS Entry Mode" HeaderTooltip="POS Entry Mode" DataField="POSEntryMode" UniqueName="POSEntryMode"
                            HeaderStyle-Width="130px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="POSEntryMode">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("POSEntryMode")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="POS Terminal Capability" HeaderTooltip="POS Terminal Capability" DataField="POSTerminalCapability" UniqueName="POSTerminalCapability"
                            HeaderStyle-Width="180px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="POSTerminalCapability">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("POSTerminalCapability")) %>' runat="server"></asp:Literal>
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
        <script src="<%=ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_VisaMerchantFraudDetail.js"></script>
    </as:RadCodeBlock>
</asp:Content>
