<%@ Page Title="DETAILED TRANSACTIONS HISTORY" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="rm_MCF_NegativeOptionReportDetailModal.aspx.cs" Inherits="rm_MCF_DetectionManageCustomViewsModal" %>

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
                GridTitle="Detailed Transactions History" runat="server"
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
                FilteringByColumnWithDataFieldConfigFile="App_Data\FilteringByColumnWithDataFieldConfig\NegativeOptionReportDetailGridConfig.json"
                BuildFilterExpressionWithSquareBrackets="true"
                EnableBuildFilterExpressionEnhancement="true"
                EnableFilterItemsPersistence="true"
                EnableSortItemsPersistence="true"
                OnPreRender="uxReportGrid_PreRender"
                OnDataSourceReady="uxReportGrid_DataSourceReady">
                <MasterTableView>
                    <Columns>
                        <as:ASGridTemplateColumn HeaderText="Report Date" HeaderTooltip="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px" ASExportFormat="Date"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="ReportDate">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDate(Eval("ReportDate")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="ISO" HeaderTooltip="ISO" DataField="ISONumber" UniqueName="ISONumber"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="ISONumber">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("ISONumber")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridBoundColumn UniqueName="MerchantNumber" DataField="MerchantNumber" HeaderText="Merchant Number" HeaderTooltip="Merchant Number"
                            ASFormat="StaticString" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </as:ASGridBoundColumn>

                        <as:ASGridTemplateColumn HeaderText="Merchant Name" HeaderTooltip="Merchant Name" DataField="MerchantName" UniqueName="MerchantName" HeaderStyle-Width="300px"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="MerchantName">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("MerchantName")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="MCC" HeaderTooltip="Merchant Category Code" DataField="MCC" UniqueName="MCC"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="MCC">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("MCC")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Sub MCC" HeaderTooltip="Sub Merchant Category Code" DataField="SubMCC" UniqueName="SubMCC"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="SubMCC">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("SubMCC")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Trans Date" HeaderTooltip="Transaction Date" DataField="TransDate" UniqueName="TransDate"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px" ASExportFormat="Date"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="TransDate">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDate(Eval("TransDate")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Trans Time" HeaderTooltip="Transaction Time" DataField="TransTime" UniqueName="TransTime"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="TransTime">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("TransTime")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Trans Code" HeaderTooltip="Transaction Code" DataField="TransCode" UniqueName="TransCode"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="TransCode">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("TransCode")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Keyed" HeaderTooltip="KEYED OR SWIPED" DataField="Keyed" UniqueName="Keyed"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="Keyed">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("Keyed")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="EMV" HeaderTooltip="EMV" DataField="EMV" UniqueName="EMV"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="EMV">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("EMV")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Card Type" HeaderTooltip="Card Type" DataField="CardType" UniqueName="CardType"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="CardType">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("CardType")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Exp Date" HeaderTooltip="Expired Date" DataField="ExpirationDate" UniqueName="ExpirationDate"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center" AllowFiltering="false">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("ExpirationDate")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Auth #" HeaderTooltip="Authorization Number" DataField="AuthNumber" UniqueName="AuthNumber"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="AuthNumber">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("AuthNumber")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Auth Amount" HeaderTooltip="Authorization Amount" DataField="AuthAmount" UniqueName="AuthAmount"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="Currency" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right"
                            SortExpression="AuthAmount">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatCurrency(Eval("AuthAmount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="A/D" HeaderTooltip="Approved/Declined" DataField="ADF" UniqueName="ADF"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="ADF">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("ADF")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="RC" HeaderTooltip="Response Code" DataField="ResponseCode" UniqueName="ResponseCode"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="ResponseCode">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("ResponseCode")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="AVS" HeaderTooltip="Address Verification System" DataField="AVS" UniqueName="AVS"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="AVS">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("AVS")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="CVV" HeaderTooltip="Cardholder Verification Value" DataField="CVV" UniqueName="CVV"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="CVV">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("CVV")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Auth Source" HeaderTooltip="Authorization Source" DataField="AuthSource" UniqueName="AuthSource"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="DynamicString"
                            ItemStyle-HorizontalAlign="Left"
                            SortExpression="AuthSource">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("AuthSource")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="CustID" HeaderTooltip="Customer ID" DataField="CustomerID" UniqueName="CustomerID"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="CustomerID">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("CustomerID")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="MOTO" HeaderTooltip="Mail/Telephone Order" DataField="MOTO" UniqueName="MOTO"
                            HeaderStyle-HorizontalAlign="Center" ASExportFormat="StaticString"
                            ItemStyle-HorizontalAlign="Center"
                            SortExpression="MOTO">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("MOTO")) %>' runat="server"></asp:Literal>
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
        <script src="<%=ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_NegativeOptionReportDetail.js"></script>
    </as:RadCodeBlock>
</asp:Content>
