<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RelatedMerchants.ascx.cs" Inherits="UserControls_RelatedMerchants" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxRelatedMerchant">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxRelatedMerchant" LoadingPanelID="uxLoadingPanelCustom" />
                <tek:AjaxUpdatedControl ControlID="uxExportTop" LoadingPanelID="uxLoadingPanelCustom" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>

<div class="row" id="relatedMerchant">
    <div class="col-md-12">
        <uc:UxExport ID="uxExportTop" 
            ShowPDF="false" ShowWord="false" ShowCSV="false"
            GridID="uxRelatedMerchant" 
            GridTitle="Related Merchants" runat="server"            
            OnNeedExportConfig="uxExport_OnNeedExportConfig"/>

        <div class="row">
            <div class="col-xs-12" data-toggle="collapse" runat="server" id="uxGridTitle">
                <h2 class="grid-title on-top" runat="server" id="h2GridTitle">
                    <span class="text-muted">
                        <asp:Literal ID="litGridSubTitle" Text="Last updated date time:" runat="server"></asp:Literal>
                    </span>
                </h2>
            </div>
        </div>

        <div>
            <as:ASGrid ID="uxRelatedMerchant" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                GridLines="None" AllowPaging="True" ASPagingMethod="SPASingleMethod1" ShowPageTotal="false"
                ShowReportTotal="false"
                XOverFlowable="true" HeaderStyle-Width="100px" IsAutoExportTemplate="true" AllowSortFilterWhenExport="true"
                CssClass="in" meta:resourcekey="uxRelatedMerchantResource1"
                OnNeedDataSource="uxRelatedMerchant_NeedDataSource"
                OnItemDataBound="uxRelatedMerchant_ItemDataBound"
                OnPreRender="uxRelatedMerchant_PreRender">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn UniqueName="MerchantNumber" DataField="MerchantNumber" HeaderText="Merchant ID" HeaderTooltip="Merchant ID" ASFormat="StaticString" HeaderStyle-Width="130px" AllowSorting="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </as:ASGridBoundColumn>

                        <as:ASGridBoundColumn UniqueName="MerchantName" DataField="MerchantName" HeaderText="Merchant Name" HeaderTooltip="Merchant Name" ASFormat="StaticString" HeaderStyle-Width="300px" AllowSorting="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </as:ASGridBoundColumn>     
                        <as:ASGridBoundColumn UniqueName="MerchantNameExport" DataField="MerchantNameExport" HeaderText="Merchant Name" HeaderTooltip="Merchant Name" ASFormat="StaticString" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </as:ASGridBoundColumn>     

                        <as:ASGridTemplateColumn HeaderStyle-CssClass="EncryptedOwnerSSN" HeaderText="SSN" HeaderTooltip="SSN" DataField="EncryptedOwnerSSN" UniqueName="EncryptedOwnerSSN"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("EncryptedOwnerSSN")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderStyle-CssClass="PartialOwnerSSN" HeaderText="SSN" HeaderTooltip="SSN" DataField="PartialOwnerSSN" UniqueName="PartialOwnerSSN"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("PartialOwnerSSN")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                         <as:ASGridBoundColumn UniqueName="PartialOwnerSSNExport" DataField="PartialOwnerSSNExport" HeaderText="SSN" HeaderTooltip="SSN" ASFormat="StaticString" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </as:ASGridBoundColumn>  

                        <as:ASGridTemplateColumn HeaderStyle-CssClass="EncryptedTaxId" HeaderText="TIN" HeaderTooltip="TIN" DataField="EncryptedTaxId" UniqueName="EncryptedTaxId"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("EncryptedTaxId")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderStyle-CssClass="PartialTaxId" HeaderText="TIN" HeaderTooltip="TIN" DataField="PartialTaxId" UniqueName="PartialTaxId"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("PartialTaxId")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                        <as:ASGridBoundColumn UniqueName="PartialTaxIdExport" DataField="PartialTaxIdExport" HeaderText="TIN" HeaderTooltip="TIN" ASFormat="StaticString" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </as:ASGridBoundColumn>  

                        <as:ASGridTemplateColumn HeaderStyle-CssClass="EncryptedDDANumber" HeaderText="DDA Number" HeaderTooltip="DDA Number" DataField="EncryptedDDANumber" UniqueName="EncryptedDDANumber"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("EncryptedDDANumber")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderStyle-CssClass="PartialDDANumber" HeaderText="DDA Number" HeaderTooltip="DDA Number" DataField="PartialDDANumber" UniqueName="PartialDDANumber"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("PartialDDANumber")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Owners Name" HeaderTooltip="Owners Name" DataField="OwnerName" UniqueName="OwnerName"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="250px"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("OwnerName")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                         <as:ASGridBoundColumn UniqueName="OwnerNameExport" DataField="OwnerNameExport" HeaderText="Owners Name" HeaderTooltip="Owners Name" ASFormat="StaticString" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </as:ASGridBoundColumn>

                        <as:ASGridTemplateColumn HeaderText="Phone #" HeaderTooltip="Phone #" DataField="PhoneNumber" UniqueName="PhoneNumber"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatPhone(Eval("PhoneNumber")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                        <as:ASGridBoundColumn UniqueName="PhoneNumberExport" DataField="PhoneNumberExport" HeaderText="Phone #" HeaderTooltip="Phone #" ASFormat="Phone" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </as:ASGridBoundColumn>

                        <as:ASGridTemplateColumn HeaderText="Address" HeaderTooltip="Address" DataField="Address" UniqueName="Address"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="450px"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("Address")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                         <as:ASGridBoundColumn UniqueName="AddressExport" DataField="AddressExport" HeaderText="Address" HeaderTooltip="Address" ASFormat="StaticString" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </as:ASGridBoundColumn>

                        <as:ASGridTemplateColumn HeaderText="Email Address" HeaderTooltip="Email Address" DataField="Email" UniqueName="Email"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="250px"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("Email")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                         <as:ASGridBoundColumn UniqueName="EmailExport" DataField="EmailExport" HeaderText="Email Address" HeaderTooltip="Email Address" ASFormat="StaticString" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </as:ASGridBoundColumn>

                        <as:ASGridTemplateColumn HeaderText="Corporate Name" HeaderTooltip="Corporate Name" DataField="CorporateName" UniqueName="CorporateName"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="270px"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("CorporateName")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                        <as:ASGridBoundColumn UniqueName="CorporateNameExport" DataField="CorporateNameExport" HeaderText="Corporate Name" HeaderTooltip="Corporate Name" ASFormat="StaticString" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </as:ASGridBoundColumn>

                        <as:ASGridTemplateColumn HeaderText="Corporate Address" HeaderTooltip="Corporate Address" DataField="CorporateAddress" UniqueName="CorporateAddress"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="450px"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("CorporateAddress")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                        <as:ASGridBoundColumn UniqueName="CorporateAddressExport" DataField="CorporateAddressExport" HeaderText="Corporate Address" HeaderTooltip="Corporate Address" ASFormat="StaticString" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </as:ASGridBoundColumn>

                        <as:ASGridTemplateColumn HeaderText="URL" HeaderTooltip="URL" DataField="URL" UniqueName="URL"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("URL")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                         <as:ASGridBoundColumn UniqueName="URLExport" DataField="URLExport" HeaderText="URL" HeaderTooltip="URL" ASFormat="StaticString" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </as:ASGridBoundColumn>

                        <as:ASGridTemplateColumn HeaderText="Chain ID" HeaderTooltip="Chain ID" DataField="ChainID" UniqueName="ChainID"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("ChainID")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                          <as:ASGridBoundColumn UniqueName="ChainIDExport" DataField="ChainIDExport" HeaderText="Chain ID" HeaderTooltip="Chain ID" ASFormat="StaticString" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </as:ASGridBoundColumn>

                        <as:ASGridTemplateColumn HeaderText="Status" HeaderTooltip="Status" DataField="Status" UniqueName="Status"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("Status")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Approved Date" HeaderTooltip="Approved Date" DataField="ApprovedDate" UniqueName="ApprovedDate"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px" ASExportFormat="Date"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDate(Eval("ApprovedDate")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Closed Date" HeaderTooltip="Closed Date" DataField="ClosedDate" UniqueName="ClosedDate"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px" ASExportFormat="Date"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDate(Eval("ClosedDate")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="ISO" HeaderTooltip="ISO" DataField="ISO" UniqueName="ISO"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="180px"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("ISO")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Agent/SalesRep" HeaderTooltip="Agent/SalesRep" DataField="AgentSalesRep" UniqueName="AgentSalesRep"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("AgentSalesRep")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Reserve Balance" HeaderTooltip="Reserve Balance" DataField="ReserveBalance" UniqueName="ReserveBalance"
                            ASExportFormat="Currency"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatCurrency(Eval("ReserveBalance")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Today' Report Date" HeaderTooltip="Today' Report Date" DataField="TodayReportDate" UniqueName="TodayReportDate"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px" ASExportFormat="Date"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDate(Eval("TodayReportDate")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                         <as:ASGridTemplateColumn HeaderText="Sales #" HeaderTooltip="Sales #" DataField="SaleCount" UniqueName="SaleCount"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("SaleCount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                        <as:ASGridTemplateColumn HeaderText="Sales Total $" HeaderTooltip="Sales Total $" DataField="SaleAmount" UniqueName="SaleAmount"
                            ASExportFormat="Currency"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatCurrency(Eval("SaleAmount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Avg Sales Trans $" HeaderTooltip="Avg Sales Trans $" DataField="AvgSaleAmount" UniqueName="AvgSaleAmount"
                            ASExportFormat="Currency"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatCurrency(Eval("AvgSaleAmount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Return #" HeaderTooltip="Return #" DataField="ReturnCount" UniqueName="ReturnCount"
                            ASExportFormat="Integer"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("ReturnCount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Return %  by #" HeaderTooltip="Return %  by #" DataField="ReturnCountRatio" UniqueName="ReturnCountRatio"
                            ASExportFormat="Percentage"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatPercent(Eval("ReturnCountRatio")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Return Total $" HeaderTooltip="Return Total $" DataField="ReturnAmount" UniqueName="ReturnAmount"
                            ASExportFormat="Currency"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatCurrencyOrEmDashOrNegative(Eval("ReturnAmount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Return %  by $" HeaderTooltip="Return %  by $" DataField="ReturnAmountRatio" UniqueName="ReturnAmountRatio"
                            ASExportFormat="Percentage"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatPercent(Eval("ReturnAmountRatio")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Avg Return Trans $" HeaderTooltip="Avg Return Trans $" DataField="AvgReturnAmount" UniqueName="AvgReturnAmount"
                            ASExportFormat="Currency"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatCurrencyOrEmDashOrNegative(Eval("AvgReturnAmount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>


                        <as:ASGridTemplateColumn HeaderText="CB #" HeaderTooltip="CB #" DataField="ChargebackCount" UniqueName="ChargebackCount"
                            ASExportFormat="Integer"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatDataOrEmDash(Eval("ChargebackCount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="CB % by #" HeaderTooltip="CB % by #" DataField="ChargebackCountRatio" UniqueName="ChargebackCountRatio"
                            ASExportFormat="Percentage"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatPercent(Eval("ChargebackCountRatio")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="CB Total $" HeaderTooltip="CB Total $" DataField="ChargebackAmount" UniqueName="ChargebackAmount"
                            ASExportFormat="Currency"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatCurrency(Eval("ChargebackAmount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="CB % by $" HeaderTooltip="CB % by $" DataField="ChargebackAmountRatio" UniqueName="ChargebackAmountRatio"
                            ASExportFormat="Percentage"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatPercent(Eval("ChargebackAmountRatio")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Avg CB Trans $" HeaderTooltip="Avg CB Trans $" DataField="AvgChargebackAmount" UniqueName="AvgChargebackAmount"
                            ASExportFormat="Currency"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatCurrency(Eval("AvgChargebackAmount")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Foreign % by #" HeaderTooltip="Foreign % by #" DataField="ForeignCardCountRatio" UniqueName="ForeignCardCountRatio"
                            ASExportFormat="Percentage"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatPercent(Eval("ForeignCardCountRatio")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>

                        <as:ASGridTemplateColumn HeaderText="Foreign % by $" HeaderTooltip="Foreign % by $" DataField="ForeignCardAmountRatio" UniqueName="ForeignCardAmountRatio"
                            ASExportFormat="Percentage"
                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                            ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal Text='<%# FormatTemplateHelper.FormatPercent(Eval("ForeignCardAmountRatio")) %>' runat="server"></asp:Literal>
                            </ItemTemplate>
                        </as:ASGridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </div>
    </div>
</div>
<as:HiddenField runat="server" ID="hfLastUpdatedDateTime" />