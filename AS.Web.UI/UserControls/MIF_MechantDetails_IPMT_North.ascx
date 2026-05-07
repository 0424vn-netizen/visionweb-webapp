<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MechantDetails_IPMT_North.ascx.cs" Inherits="UserControls_MIF_MechantDetails_IPMT_North" %>
<%@ Register TagPrefix="uc" TagName="RiskInfo" Src="~/UserControls/MIF_RiskInformationSection.ascx" %>


<asp:PlaceHolder ID="phdMerchantDetail" runat="server">
    <div class="row" id="merchinfo">
        <div class="col-md-9" data-toggle="collapse" data-target="#ciMerchantInformation">
            <h2 class="grid-title mt-3x">
                <asp:Literal ID="Literal1" runat="server" Text="Merchant Information" meta:resourcekey="Literal1" />
            </h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 in" id="ciMerchantInformation">
            <table class="ASTable">
                <tr class="Row">
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal2" runat="server" Text="Internal Merchant ID:" meta:resourcekey="Literal2" />
                    </td>
                    <td>
                        <%= BindValue("InternalMerchantNumber")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal3" runat="server" Text="Address:" meta:resourcekey="Literal3" />
                    </td>
                    <td>
                        <%= BindValue("AffiliateAddress")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal4" runat="server" Text="Phone:" meta:resourcekey="Literal4" />
                    </td>
                    <td>
                        <%= FormatPhone(BindValue("MerchantPhone"))%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal5" runat="server" Text="External Merchant ID:" meta:resourcekey="Literal5" />
                    </td>
                    <td>
                        <%= BindValue("ExternalMerchantNumber")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal6" runat="server" Text="City:" meta:resourcekey="Literal6" />
                    </td>
                    <td>
                        <%= BindValue("AffiliateCity")%>
                    </td>
                    <td class="heading  valign-top text-nowrap">
                        <asp:Literal ID="Literal7" runat="server" Text="Fax Ind:" meta:resourcekey="Literal7" />
                    </td>
                    <td>
                        <%= BindValue("FaxType")%>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal8" runat="server" Text="External Agent Ind:" meta:resourcekey="Literal8" />
                    </td>
                    <td>
                        <%= BindValue("ExternalAgentInd")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal9" runat="server" Text="State:" meta:resourcekey="Literal9" />
                    </td>
                    <td>
                        <%= BindValue("AffiliateState")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal10" runat="server" Text="Fax #:" meta:resourcekey="Literal10" />
                    </td>
                    <td>
                        <%= FormatPhone(BindValue("FaxNumber"))%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal11" runat="server" Text="Channel Name:" meta:resourcekey="Literal11" />
                    </td>
                    <td>
                        <%= BindValue("ChannelName")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal12" runat="server" Text="Zip 1:" meta:resourcekey="Literal12" />
                    </td>
                    <td>
                        <%= BindValue("Zip1")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal13" runat="server" Text="Primary Email:" meta:resourcekey="Literal13" />
                    </td>
                    <td>
                        <%= BindValue("PrimaryEmail")%>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal14" runat="server" Text="DBA Name:" meta:resourcekey="Literal14" />
                    </td>
                    <td>
                        <%= BindValue("DBAName")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal15" runat="server" Text="Zip 2:" meta:resourcekey="Literal15" />
                    </td>
                    <td>
                        <%= BindValue("Zip2")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal16" runat="server" Text="Descriptor Name:" meta:resourcekey="Literal16" />
                    </td>
                    <td>
                        <%= BindValue("DescriptorName")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal17" runat="server" Text="Attention:" meta:resourcekey="Literal17" />
                    </td>
                    <td>
                        <%= BindValue("Attention")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal18" runat="server" Text="Country:" meta:resourcekey="Literal18" />
                    </td>
                    <td>
                        <%= BindValue("Country")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal19" runat="server" Text="Descriptor Phone:" meta:resourcekey="Literal19" />
                    </td>
                    <td>
                        <%= FormatPhone(BindValue("DescriptorPhone"))%>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal20" runat="server" Text="URL:" meta:resourcekey="Literal20" />
                    </td>
                    <td>
                        <%= BindValue("URL")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal21" runat="server" Text="Status:" meta:resourcekey="Literal21" />
                    </td>
                    <td>
                        <%= BindValue("Status")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal22" runat="server" Text="Descriptor City:" meta:resourcekey="Literal22" />
                    </td>
                    <td>
                        <%= BindValue("DescriptorCity")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal23" runat="server" Text="Paperless Merchant Indicator:" meta:resourcekey="Literal23" />
                    </td>
                    <td>
                        <%= BindValue("PaperlessMerchantInd")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal24" runat="server" Text="Store #:" meta:resourcekey="Literal24" />
                    </td>
                    <td>
                        <%= BindValue("StoreNumber")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal25" runat="server" Text="Descriptor State:" meta:resourcekey="Literal25" />
                    </td>
                    <td>
                        <%= BindValue("DescriptorState")%>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal26" runat="server" Text="Seasonal:" meta:resourcekey="Literal26" />
                    </td>
                    <td colspan="5">
                        <%= BindValue("Seasonal")%>
                    </td>
                </tr>
                <asp:Panel ID="uxPnlUserID" runat="server" Visible="false">
                    <tr class="AltRow">
                        <td class="heading text-nowrap valign-middle">
                            <asp:Literal ID="uxlblUserID" runat="server" Text="User ID:" />
                        </td>
                        <td>
                            <div class="opt-in-out-text mt-1x">
                                <%=BindValue("UserID")%>
                            </div>
                            <div class="pull-right opt-in-out-button">
                                <asp:Panel ID="uxPnlSiteAccess" runat="server">
                                    <as:Button ID="uxSiteAccess" runat="server" Text="Site Access" OnClick="uxSiteAccess_click" IsStandardButton="False" CssClass="btn btn-default" meta:resourcekey="uxSiteAccessResource1" />
                                </asp:Panel>
                            </div>
                        </td>
                        <td colspan="4"></td>
                    </tr>
                </asp:Panel>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <%--Billing Information--%>
            <as:ExportKeyValueTable IsMultiExport="true" TargetID="uxBillingInformation" ScrollID="billinginfo" ID="uxExportBillingInformation" runat="server" Title="Billing Information" DataSourceID="uxBillingInformation" meta:resourcekey="uxExportBillingInformation"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxBillingInformation" runat="server" OnNeedDataSource="GetDataSource">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="DBAName" UniqueName="DBAName" CellTitle="Name :" FormatType="DynamicString" meta:resourcekey="DBAName" />
                    <as:KeyValueTableBoundRow DataField="Attention" UniqueName="Attention" FormatType="DynamicString" CellTitle="Attention :" meta:resourcekey="Attention" />
                    <as:KeyValueTableBoundRow DataField="AffiliateAddress" UniqueName="AffiliateAddress" FormatType="DynamicString" CellTitle="Address :" meta:resourcekey="AffiliateAddress" />
                    <as:KeyValueTableBoundRow DataField="AffiliateCity" UniqueName="AffiliateCity" FormatType="DynamicString" CellTitle="City :" meta:resourcekey="AffiliateCity" />
                    <as:KeyValueTableBoundRow DataField="AffiliateState" UniqueName="AffiliateState" FormatType="DynamicString" CellTitle="State :" meta:resourcekey="AffiliateState" />
                    <as:KeyValueTableBoundRow DataField="Zip1" UniqueName="Zip1" FormatType="DynamicString" CellTitle="Zip 1:" meta:resourcekey="Zip1" />
                    <as:KeyValueTableBoundRow DataField="Zip2" UniqueName="Zip2" FormatType="DynamicString" CellTitle="Zip 2:" meta:resourcekey="Zip2" />
                    <as:KeyValueTableBoundRow DataField="MerchantPhone" UniqueName="MerchantPhone" FormatType="Phone" CellTitle="Phone :" meta:resourcekey="MerchantPhone" />
                </RowCollection>
            </as:KeyValueTable>
        </div>
        <%--Hierarchy Information--%>
        <div class="col-md-6">
            <!-- Risk Info --> 
            <uc:RiskInfo ID="uxRiskInfo" runat="server" />

            <div class="row" id="hierarchyinfo">
                <div class="col-xs-12" data-toggle="collapse" data-target="#ciHierarchyInformation">
                    <h2 class="grid-title">
                        <asp:Literal ID="Literal28" runat="server" Text="Hierarchy Information" meta:resourcekey="Literal28" /></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 in" id="ciHierarchyInformation">
                    <table class="ASTable">
                        <tr class="Row">
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal29" runat="server" Text="Client Name:" meta:resourcekey="Literal29" />
                            </td>
                            <td>
                                <%= BindValue("ClientName")%>
                            </td>
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal30" runat="server" Text="Chain:" meta:resourcekey="Literal30" />
                            </td>
                            <td>
                                <as:PlaceHolder ID="uxChain" runat="server" Visible='<%# CheckHierarchy("Chain") %>'>
                                    <a href="#" id="uxLinkChain" runat="server"><%= BindValue("Chain")%></a>
                                </as:PlaceHolder>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading valign-top">
                                <asp:Literal ID="Literal32" runat="server" Text="Client Login:" meta:resourcekey="Literal32" />
                            </td>
                            <td>
                                <%= BindValue("ClientLogin")%>
                            </td>
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal33" runat="server" Text="Sales Agent:" meta:resourcekey="Literal33" />
                            </td>
                            <td>
                                <%= BindValue("SalesAgent")%>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading valign-top">
                                <asp:Literal ID="Literal36" runat="server" Text="Marker Bank:" meta:resourcekey="Literal36" />
                            </td>
                            <td>
                                <%= BindValue("MarkerBank")%>
                            </td>
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal37" runat="server" Text="Entitlement Sales Agent:" meta:resourcekey="Literal37" />
                            </td>
                            <td>
                                <%= BindValue("EntitlementSalesAgent")%>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading valign-top">
                                <asp:Literal ID="Literal39" runat="server" Text="Business:" meta:resourcekey="Literal39" />
                            </td>
                            <td>
                                <%= BindValue("Business")%>
                            </td>
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal40" runat="server" Text="Relationship Mananger:" meta:resourcekey="Literal40" />
                            </td>
                            <td>
                                <%= BindValue("RelationshipManager")%>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading valign-top">
                                <asp:Literal ID="Literal42" runat="server" Text="Bank:" meta:resourcekey="Literal42" />
                            </td>
                            <td>
                                <as:PlaceHolder ID="uxBank" runat="server" Visible='<%# CheckHierarchy("Bank") %>'>
                                    <a href="#" id="uxLinkBank" runat="server"><%= BindValue("Bank")%></a>
                                </as:PlaceHolder>
                            </td>
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal43" runat="server" Text="Master Sales Agent:" meta:resourcekey="Literal43" />
                            </td>
                            <td>
                                <%= BindValue("MasterSalesAgent")%>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading valign-top">
                                <asp:Literal ID="Literal46" runat="server" Text="Agent:" meta:resourcekey="Literal46" />
                            </td>
                            <td>
                                <as:PlaceHolder ID="uxAgent" runat="server" Visible='<%# CheckHierarchy("Agent") %>'>
                                    <a href="#" id="uxLinkAgent" runat="server"><%= BindValue("Agent")%></a>
                                </as:PlaceHolder>
                            </td>
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal47" runat="server" Text="Branch Number:" meta:resourcekey="Literal47" />
                            </td>
                            <td>
                                <%= BindValue("HBranchNumber")%>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading valign-top">
                                <asp:Literal ID="Literal49" runat="server" Text="Corp:" meta:resourcekey="Literal49" />
                            </td>
                            <td colspan="3">
                                <as:PlaceHolder ID="uxNCorp" runat="server" Visible='<%# CheckHierarchy("Corp") %>'>
                                    <a href="#" id="uxLinkNCorp" runat="server"><%= BindValue("Corp")%></a>
                                </as:PlaceHolder>
                            </td>
                        </tr>

                    </table>
                </div>
            </div>

        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <%--Business Information--%>
            <as:ExportKeyValueTable IsMultiExport="true" ScrollID="businessinfo" ID="uxExportBusinessInformation" runat="server" Title="Business Information" TargetID="uxBusinessInformation" DataSourceID="uxBusinessInformation" meta:resourcekey="uxExportBusinessInformation"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxBusinessInformation" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="CorporateName" UniqueName="CorporateName" CellTitle="Corporate Name:" FormatType="DynamicString" meta:resourcekey="CorporateName" />
                    <as:KeyValueTableBoundRow DataField="CorporateAddress" UniqueName="CorporateAddress" FormatType="DynamicString" CellTitle="Corporate Address:" meta:resourcekey="CorporateAddress" />
                    <as:KeyValueTableBoundRow DataField="CorporateAttention" UniqueName="CorporateAttention" FormatType="DynamicString" CellTitle="Corporate Attention:" meta:resourcekey="CorporateAttention" />
                    <as:KeyValueTableBoundRow DataField="CorporateCity" UniqueName="CorporateCity" CellTitle="Corporate City:" FormatType="DynamicString" meta:resourcekey="CorporateCity" />
                    <as:KeyValueTableBoundRow DataField="CorporateCountry" UniqueName="CorporateCountry" FormatType="DynamicString" CellTitle="Corporate Country:" meta:resourcekey="CorporateCountry" />
                    <as:KeyValueTableBoundRow DataField="CorporatePostalZip" UniqueName="CorporatePostalZip" FormatType="DynamicString" CellTitle="Corporate Postal Zip:" meta:resourcekey="CorporatePostalZip" />
                    <as:KeyValueTableBoundRow DataField="CorporateTelephone" UniqueName="CorporateTelephone" CellTitle="Corporate Telephone:" FormatType="Phone" meta:resourcekey="CorporateTelephone" />
                    <as:KeyValueTableBoundRow DataField="ContractDate" UniqueName="ContractDate" FormatType="Date" CellTitle="Contract Date:" meta:resourcekey="ContractDate" />
                    <as:KeyValueTableBoundRow DataField="ApprovalDate" UniqueName="ApprovalDate" FormatType="Date" CellTitle="ApprovalDate:" meta:resourcekey="ApprovalDate" />
                    <as:KeyValueTableBoundRow DataField="ClosedDate" UniqueName="ClosedDate" CellTitle="ClosedDate:" FormatType="Date" meta:resourcekey="ClosedDate" />
                    <as:KeyValueTableBoundRow DataField="SiteAccess" UniqueName="SiteAccess" FormatType="DynamicString" CellTitle="Site Access:" meta:resourcekey="SiteAccess" />
                    <as:KeyValueTableBoundRow DataField="LastCancellationDate" UniqueName="LastCancellationDate" FormatType="Date" CellTitle="Last Cancellation date:" meta:resourcekey="LastCancellationDate" />
                    <as:KeyValueTableBoundRow DataField="SIC" UniqueName="SIC" FormatType="DynamicString" CellTitle="SIC:" meta:resourcekey="SIC" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Billing--%>
            <as:ExportKeyValueTable ScrollID="billing" ID="uxExportBilling" Title="Billing" runat="server" meta:resourcekey="uxExportBilling" CssClass="parent-item"
                TargetID="uxBillback,uxInterchangeCompliance,uxBillingPricingType,uxSupplyBilling,uxEarlyTerminatioFees,uxImprinter,uxFees"></as:ExportKeyValueTable>

            <%--Billback--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportBillback" IsChild="true" runat="server" Title="Billback" DataSourceID="uxBillback" meta:resourcekey="uxExportBillback"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxBillback" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="RebateOn" UniqueName="RebateOn" CellTitle="Rebate On:" FormatType="DynamicString" meta:resourcekey="RebateOn" />
                    <as:KeyValueTableBoundRow DataField="AutoRebate" UniqueName="AutoRebate" FormatType="DynamicString" CellTitle="Auto Rebate:" meta:resourcekey="AutoRebate" />
                    <as:KeyValueTableBoundRow DataField="AutoBillback" UniqueName="AutoBillback" FormatType="DynamicString" CellTitle="Auto Billback:" meta:resourcekey="AutoBillback" />
                    <as:KeyValueTableBoundRow DataField="RetailerRateTable" UniqueName="RetailerRateTable" CellTitle="Retailer Rate Table:" FormatType="DynamicString" meta:resourcekey="RetailerRateTable" />
                    <as:KeyValueTableBoundRow DataField="SurchargeExcep" UniqueName="SurchargeExcep" FormatType="DynamicString" CellTitle="Surcharge Excep:" meta:resourcekey="SurchargeExcep" />
                    <as:KeyValueTableBoundRow DataField="BillBackOn" UniqueName="BillBackOn" FormatType="DynamicString" CellTitle="Bill back on:" meta:resourcekey="BillBackOn" />
                    <as:KeyValueTableBoundRow DataField="BillbackRounding" UniqueName="BillbackRounding" CellTitle="Bill back rounding:" FormatType="DynamicString" meta:resourcekey="BillbackRounding" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Interchange/Compliance--%>
            <as:ExportKeyValueTable IsMultiExport="true" IsChild="true" ID="uxExportInterchangeCompliance" runat="server" Title="Interchange/Compliance" DataSourceID="uxInterchangeCompliance" meta:resourcekey="uxExportInterchangeCompliance"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxInterchangeCompliance" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="MCMID" UniqueName="MCMID" CellTitle="MC MID:" FormatType="DynamicString" meta:resourcekey="MCMID" />
                    <as:KeyValueTableBoundRow DataField="FireSafetyInd" UniqueName="FireSafetyInd" FormatType="DynamicString" CellTitle="Fire Safety Ind:" meta:resourcekey="FireSafetyInd" />
                    <as:KeyValueTableBoundRow DataField="VIMercVerValue" UniqueName="VIMercVerValue" FormatType="DynamicString" CellTitle="VI Mer Ver Value:" meta:resourcekey="VIMercVerValue" />
                    <as:KeyValueTableBoundRow DataField="QuasiCashAcceptFlag" UniqueName="QuasiCashAcceptFlag" CellTitle="Quasi Cash Accept Flag:" FormatType="DynamicString" meta:resourcekey="QuasiCashAcceptFlag" />
                    <as:KeyValueTableBoundRow DataField="VolTierINTCH" UniqueName="VolTierINTCH" FormatType="DynamicString" CellTitle="Vol.Tier INTCH:" meta:resourcekey="VolTierINTCH" />
                    <as:KeyValueTableBoundRow DataField="ZeroInterchangeIndicator" UniqueName="ZeroInterchangeIndicator" FormatType="DynamicString" CellTitle="Zero Interchange Indicator:" meta:resourcekey="ZeroInterchangeIndicator" />
                    <as:KeyValueTableBoundRow DataField="ThirdPartyProcessorCode" UniqueName="ThirdPartyProcessorCode" CellTitle="Third Party Processor Code:" FormatType="DynamicString" meta:resourcekey="ThirdPartyProcessorCode" />
                    <as:KeyValueTableBoundRow DataField="AuthType" UniqueName="AuthType" FormatType="DynamicString" CellTitle="Auth Type:" meta:resourcekey="AuthType" />
                    <as:KeyValueTableBoundRow DataField="MotoEcommerce" UniqueName="MotoEcommerce" FormatType="DynamicString" CellTitle="MOTO/Ecommerce:" meta:resourcekey="MotoEcommerce" />
                    <as:KeyValueTableBoundRow DataField="PurchLrgTkt" UniqueName="PurchLrgTkt" CellTitle="Purchase Lrg.Tkt:" FormatType="DynamicString" meta:resourcekey="PurchLrgTkt" />
                    <as:KeyValueTableBoundRow DataField="FANFInd" UniqueName="FANFInd" FormatType="DynamicString" CellTitle="FANF Ind:" meta:resourcekey="FANFInd" />
                    <as:KeyValueTableBoundRow DataField="ProgramRegistrationID" UniqueName="ProgramRegistrationID" FormatType="DynamicString" CellTitle="Program Registration Id:" meta:resourcekey="ProgramRegistrationID" />
                    <as:KeyValueTableBoundRow DataField="SalesTaxIndicator" UniqueName="SalesTaxIndicator" FormatType="DynamicString" CellTitle="Sales Tax Indicator:" meta:resourcekey="SalesTaxIndicator" />
                    <as:KeyValueTableBoundRow DataField="ThirdPartyProcessorText" UniqueName="ThirdPartyProcessorText" CellTitle="Third Party Processor Text:" FormatType="DynamicString" meta:resourcekey="ThirdPartyProcessorText" />
                    <as:KeyValueTableBoundRow DataField="VIRelationParticipant" UniqueName="VIRelationParticipant" FormatType="DynamicString" CellTitle="VI Relation Participant:" meta:resourcekey="VIRelationParticipant" />
                    <as:KeyValueTableBoundRow DataField="VIDebitAccept" UniqueName="VIDebitAccept" FormatType="DynamicString" CellTitle="VI Debit Accept:" meta:resourcekey="VIDebitAccept" />
                    <as:KeyValueTableBoundRow DataField="VIMVIReserve" UniqueName="VIMVIReserve" CellTitle="VI MVI Reserve:" FormatType="DynamicString" meta:resourcekey="VIMVIReserve" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Pricing Type--%>
            <as:ExportKeyValueTable IsMultiExport="true" IsChild="true" ID="uxExportBillingPricingType" runat="server" Title="Pricing Type" DataSourceID="uxBillingPricingType" meta:resourcekey="uxExportBillingPricingType"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxBillingPricingType" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="MerchantTypeCode" UniqueName="MerchantTypeCode" CellTitle="Merchant Type Code:" FormatType="DynamicString" meta:resourcekey="MerchantTypeCode" />
                    <as:KeyValueTableBoundRow DataField="TMPRequest" UniqueName="TMPRequest" FormatType="DynamicString" CellTitle="TMP Request:" meta:resourcekey="TMPRequest" />
                    <as:KeyValueTableBoundRow DataField="CardholderID" UniqueName="CardholderID" FormatType="DynamicString" CellTitle="TMP Cardholder Id:" meta:resourcekey="CardholderID" />
                    <as:KeyValueTableBoundRow DataField="TMPEntryMode" UniqueName="TMPEntryMode" CellTitle="TMP Entry Mode:" FormatType="DynamicString" meta:resourcekey="TMPEntryMode" />
                    <as:KeyValueTableBoundRow DataField="TMPServiceLevel" UniqueName="TMPServiceLevel" FormatType="DynamicString" CellTitle="TMP Service Level:" meta:resourcekey="TMPServiceLevel" />
                    <as:KeyValueTableBoundRow DataField="TMPAuthorizationSource" UniqueName="TMPAuthorizationSource" FormatType="DynamicString" CellTitle="TMP Authorization Source:" meta:resourcekey="TMPAuthorizationSource" />
                    <as:KeyValueTableBoundRow DataField="TMPPOSTerminalCap" UniqueName="TMPPOSTerminalCap" CellTitle="TMP POS Terminal Cap:" FormatType="DynamicString" meta:resourcekey="TMPPOSTerminalCap" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Supply Billing--%>
            <as:ExportKeyValueTable IsMultiExport="true" IsChild="true" ID="uxExportSupplyBilling" runat="server" Title="Supply Billing" DataSourceID="uxSupplyBilling" meta:resourcekey="uxExportSupplyBilling"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxSupplyBilling" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="SupplyBillInd" UniqueName="SupplyBillInd" CellTitle="Supply Billing Ind:" FormatType="DynamicString" meta:resourcekey="SupplyBillInd" />
                    <as:KeyValueTableBoundRow DataField="SupplyShBillInd" UniqueName="SupplyShBillInd" FormatType="DynamicString" CellTitle="Supply S/H Billing Ind:" meta:resourcekey="SupplyShBillInd" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Early Termination Fees--%>
            <as:ExportKeyValueTable IsMultiExport="true" IsChild="true" ID="uxExportEarlyTerminatioFees" runat="server" Title="Early Termination Fees" DataSourceID="uxEarlyTerminatioFees" meta:resourcekey="uxExportEarlyTerminatioFees"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxEarlyTerminatioFees" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="ETFFlag" UniqueName="ETFFlag" CellTitle="ETF Flag:" FormatType="DynamicString" meta:resourcekey="ETFFlag" />
                    <as:KeyValueTableBoundRow DataField="ETFTermDate" UniqueName="ETFTermDate" FormatType="Date" CellTitle="ETF Term Date:" meta:resourcekey="ETFTermDate" />
                    <as:KeyValueTableBoundRow DataField="ETFAmount" UniqueName="ETFAmount" FormatType="DynamicString" CellTitle="ETF Amount:" meta:resourcekey="ETFAmount" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Imprinter--%>
            <as:ExportKeyValueTable IsMultiExport="true" IsChild="true" ID="uxExportImprinter" runat="server" Title="Imprinter" DataSourceID="uxImprinter" meta:resourcekey="uxExportImprinter"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxImprinter" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="ImprinterQtyToBill" UniqueName="ImprinterQtyToBill" CellTitle="Imprinter Qty to Bill:" FormatType="DynamicString" meta:resourcekey="ImprinterQtyToBill" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Fees--%>
            <as:ExportKeyValueTable IsMultiExport="true" IsChild="true" ID="uxExportFees" runat="server" Title="Fees" DataSourceID="uxFees" meta:resourcekey="uxExportFees"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxFees" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="DelayedBillingStartDate" UniqueName="DelayedBillingStartDate" CellTitle="Delayed Billing Start Date:" FormatType="Date" meta:resourcekey="DelayedBillingStartDate" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Merchant Card Information--%>
            <as:ExportKeyValueTable IsMultiExport="true" ScrollID="merchantcardinfo" ID="uxExportMerchantCardInformation" FileName="Merchant Card Information" Title="Merchant Card Information" runat="server"
                TargetID="uxMerchantCardInformation" DataSourceID="uxMerchantCardInformation" meta:resourcekey="uxExportMerchantCardInformation"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxMerchantCardInformation" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="SalesLimit" UniqueName="SalesLimit" CellTitle="Sales Limit:" FormatType="DynamicString" meta:resourcekey="SalesLimit" />
                    <as:KeyValueTableBoundRow DataField="CreditLimit" UniqueName="CreditLimit" FormatType="DynamicString" CellTitle="Credit limit:" meta:resourcekey="CreditLimit" />
                    <as:KeyValueTableBoundRow DataField="AuthorizationLimit" UniqueName="AuthorizationLimit" FormatType="DynamicString" CellTitle="Authorization Limit:" meta:resourcekey="AuthorizationLimit" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Account Cancellation--%>
            <as:ExportKeyValueTable IsMultiExport="true" ScrollID="accountcancellation" ID="uxExportAccountCancellation" Title="Account Cancellation" runat="server"
                TargetID="uxAccountCancellation" DataSourceID="uxAccountCancellation" meta:resourcekey="uxExportAccountCancellation"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxAccountCancellation" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="CloseReasonCode" UniqueName="CloseReasonCode" CellTitle="Close Reason Code:" FormatType="DynamicString" meta:resourcekey="CloseReasonCode" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Transaction Processing--%>
            <as:ExportKeyValueTable ScrollID="transactionprocessing" ID="uxExportTransactionProcessing" Title="Transaction Processing" runat="server"
                TargetID="uxTerminalNetworkInformation,uxPTSSettings,uxAuthorizationReversals,uxSignatureCapture,uxTrustKeeper" meta:resourcekey="uxExportTransactionProcessing" CssClass="parent-item"></as:ExportKeyValueTable>

            <%--Terminal Network Information--%>
            <as:ExportKeyValueTable IsMultiExport="true" IsChild="true" ID="uxExportTerminalNetworkInformation" runat="server" Title="Terminal Network Information" DataSourceID="uxTerminalNetworkInformation" meta:resourcekey="uxExportTerminalNetworkInformation"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxTerminalNetworkInformation" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="PrimaryNetwork" UniqueName="PrimaryNetwork" CellTitle="Primary Network:" FormatType="DynamicString" meta:resourcekey="PrimaryNetwork" />
                    <as:KeyValueTableBoundRow DataField="SecondaryNetwork" UniqueName="SecondaryNetwork" CellTitle="Secondary Network:" FormatType="DynamicString" meta:resourcekey="SecondaryNetwork" />
                    <as:KeyValueTableBoundRow DataField="SecurityCode" UniqueName="SecurityCode" CellTitle="Primary Security Code:" FormatType="DynamicString" meta:resourcekey="SecurityCode" />
                    <as:KeyValueTableBoundRow DataField="SecondarySecurityCode" UniqueName="SecondarySecurityCode" CellTitle="Secondary Security Code:" FormatType="DynamicString" meta:resourcekey="SecondarySecurityCode" />
                </RowCollection>
            </as:KeyValueTable>

            <%--PTS Settings--%>
            <as:ExportKeyValueTable IsMultiExport="true" IsChild="true" ID="uxExportPTSSettings" runat="server" Title="PTS Settings" DataSourceID="uxPTSSettings" meta:resourcekey="uxExportPTSSettings"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxPTSSettings" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="PTSIndicator" UniqueName="PTSIndicator" CellTitle="PTS Indicator:" FormatType="DynamicString" meta:resourcekey="PTSIndicator" />
                    <as:KeyValueTableBoundRow DataField="MasterCardCA" UniqueName="MasterCardCA" CellTitle="Mastercard CA:" FormatType="DynamicString" meta:resourcekey="MasterCardCA" />
                    <as:KeyValueTableBoundRow DataField="CumCrd" UniqueName="CumCrd" CellTitle="CUM Crd:" FormatType="DynamicString" meta:resourcekey="CumCrd" />
                    <as:KeyValueTableBoundRow DataField="MgmtFeeDate" UniqueName="MgmtFeeDate" CellTitle="Mgmt Fee Date:" FormatType="Date" meta:resourcekey="MgmtFeeDate" />
                    <as:KeyValueTableBoundRow DataField="CreditBINInclusion" UniqueName="CreditBINInclusion" CellTitle="Credit BIN Inclusion:" FormatType="DynamicString" meta:resourcekey="CreditBINInclusion" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Authorization Reversals--%>
            <as:ExportKeyValueTable IsMultiExport="true" IsChild="true" ID="uxExportAuthorizationReversals" runat="server" Title="Authorization Reversals" DataSourceID="uxAuthorizationReversals" meta:resourcekey="uxExportAuthorizationReversals"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxAuthorizationReversals" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="RVSLTimezone" UniqueName="RVSLTimezone" CellTitle="RVSL Timezone:" FormatType="DynamicString" meta:resourcekey="RVSLTimezone" />
                    <as:KeyValueTableBoundRow DataField="RVSLDaylightSavingInd" UniqueName="RVSLDaylightSavingInd" CellTitle="RVSL Daylight Saving Ind:" FormatType="DynamicString" meta:resourcekey="RVSLDaylightSavingInd" />
                    <as:KeyValueTableBoundRow DataField="RVSL24hrProcessInd" UniqueName="RVSL24hrProcessInd" CellTitle="RVSL 24hr Process:" FormatType="DynamicString" meta:resourcekey="RVSL24hrProcessInd" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Signature Capture--%>
            <as:ExportKeyValueTable IsMultiExport="true" IsChild="true" ID="uxExportSignatureCapture" runat="server" Title="Signature Capture" DataSourceID="uxSignatureCapture" meta:resourcekey="uxExportSignatureCapture"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxSignatureCapture" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="SignatureCapture" UniqueName="SignatureCapture" CellTitle="Signature Capture:" FormatType="DynamicString" meta:resourcekey="SignatureCapture" />
                    <as:KeyValueTableBoundRow DataField="AcceptSigCapRej" UniqueName="AcceptSigCapRej" CellTitle="Sig-Cap Accept Rejects:" FormatType="DynamicString" meta:resourcekey="AcceptSigCapRej" />
                </RowCollection>
            </as:KeyValueTable>

            <%--TrustKeeper--%>
            <as:ExportKeyValueTable IsMultiExport="true" IsChild="true" ID="uxExportTrustKeeper" runat="server" Title="TrustKeeper" DataSourceID="uxTrustKeeper" meta:resourcekey="uxExportTrustKeeper"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxTrustKeeper" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="TrustKeeperIndicator" UniqueName="TrustKeeperIndicator" CellTitle="Trust Keeper Indicator:" FormatType="DynamicString" meta:resourcekey="TrustKeeperIndicator" />
                    <as:KeyValueTableBoundRow DataField="LastChangeDate" UniqueName="LastChangeDate" CellTitle="Last Change Date:" FormatType="Date" meta:resourcekey="LastChangeDate" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Discount Information--%>
            <as:ExportKeyValueTable IsMultiExport="true" TargetID="uxDiscountInformation" ScrollID="discountinfo" ID="uxExportDiscountInformation" runat="server" Title="Discount Information" DataSourceID="uxDiscountInformation" meta:resourcekey="uxExportDiscountInformation"></as:ExportKeyValueTable>
            <as:KeyValueTable IsMultiExport="true" ID="uxDiscountInformation" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="MCDiscountOption" UniqueName="MCDiscountOption" CellTitle="M/C Discount option:" FormatType="DynamicString" meta:resourcekey="MCDiscountOption" />
                    <as:KeyValueTableBoundRow DataField="VisaDiscountOption" UniqueName="VisaDiscountOption" CellTitle="Visa Discount option:" FormatType="DynamicString" meta:resourcekey="VisaDiscountOption" />
                    <as:KeyValueTableBoundRow DataField="DiscoverDiscountOption" UniqueName="DiscoverDiscountOption" CellTitle="Discover Discount option:" FormatType="DynamicString" meta:resourcekey="DiscoverDiscountOption" />
                    <as:KeyValueTableBoundRow DataField="AmexDiscountOption" UniqueName="AmexDiscountOption" CellTitle="AMEX Discount option:" FormatType="DynamicString" meta:resourcekey="AmexDiscountOption" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Processing Dates--%>
            <as:ExportKeyValueTable IsMultiExport="true" TargetID="uxProcessingDates" ScrollID="processingdates" ID="uxExportProcessingDates" runat="server" Title="Processing Dates" DataSourceID="uxProcessingDates" meta:resourcekey="uxExportProcessingDates"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxProcessingDates" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="ActivationDate" UniqueName="ActivationDate" CellTitle="Activation Date:" FormatType="Date" meta:resourcekey="ActivationDate" />
                    <as:KeyValueTableBoundRow DataField="FirstPostingDate" UniqueName="FirstPostingDate" CellTitle="First Posting Date:" FormatType="Date" meta:resourcekey="FirstPostingDate" />
                    <as:KeyValueTableBoundRow DataField="LastPostingDate" UniqueName="LastPostingDate" CellTitle="Last Posting Date:" FormatType="Date" meta:resourcekey="LastPostingDate" />
                    <as:KeyValueTableBoundRow DataField="LastSettleDate" UniqueName="LastSettleDate" CellTitle="Last Settle Date:" FormatType="Date" meta:resourcekey="LastSettleDate" />
                    <as:KeyValueTableBoundRow DataField="RevenueBooked" UniqueName="RevenueBooked" CellTitle="Revenue Booked:" FormatType="Date" meta:resourcekey="RevenueBooked" />
                    <as:KeyValueTableBoundRow DataField="MemBilledDate" UniqueName="MemBilledDate" CellTitle="Membership Billed Date:" FormatType="Date" meta:resourcekey="MemBilledDate" />                     
                </RowCollection>
            </as:KeyValueTable>

            <%--Program & Products--%>
            <as:ExportKeyValueTable TargetID="uxProgramServicesParticipation,cirocessingDates,uxGlobalePricing,uxPayeezy,uxTransArmor,ciProgramDetails" ScrollID="programproducts" ID="uxExportProgramProducts" runat="server" Title="Program & Products" meta:resourcekey="uxExportProgramProducts" CssClass="parent-item"></as:ExportKeyValueTable>

            <%--Program/Services Participation--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportProgramServicesParticipation" IsChild="true" runat="server" FileName="Program/Services Participation" Title="Program/Services Participation" DataSourceID="uxProgramServicesParticipation" meta:resourcekey="uxExportProgramServicesParticipation"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxProgramServicesParticipation" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="MultiCurrency" UniqueName="MultiCurrency" CellTitle="Multi-Currency:" FormatType="DynamicString" meta:resourcekey="MultiCurrency" />
                    <as:KeyValueTableBoundRow DataField="Domestic_Foreign" UniqueName="Domestic_Foreign" CellTitle="Domestic/Foreign:" FormatType="DynamicString" meta:resourcekey="Domestic_Foreign" />
                    <as:KeyValueTableBoundRow DataField="EMVIndicator" UniqueName="EMVIndicator" CellTitle="EMV Indicator:" FormatType="DynamicString" meta:resourcekey="EMVIndicator" />
                    <as:KeyValueTableBoundRow DataField="BundleID" UniqueName="BundleID" CellTitle="Bundle Id:" FormatType="DynamicString" meta:resourcekey="BundleID" />
                    <as:KeyValueTableBoundRow DataField="VIIRAMEligible" UniqueName="VIIRAMEligible" CellTitle="VI IRAM Eligible:" FormatType="DynamicString" meta:resourcekey="VIIRAMEligible" />
                    <as:KeyValueTableBoundRow DataField="MCIRAMEligible" UniqueName="MCIRAMEligible" CellTitle="MC IRAM Eligible:" FormatType="DynamicString" meta:resourcekey="MCIRAMEligible" />
                    <as:KeyValueTableBoundRow DataField="MagneticStripeLoyality" UniqueName="MagneticStripeLoyality" CellTitle="Magnetic Stripe Loyalty:" FormatType="DynamicString" meta:resourcekey="MagneticStripeLoyality" />
                    <as:KeyValueTableBoundRow DataField="FraudScoring" UniqueName="FraudScoring" CellTitle="Fraud Scoring:" FormatType="DynamicString" meta:resourcekey="FraudScoring" />
                    <as:KeyValueTableBoundRow DataField="FraudScoringEffDate" UniqueName="FraudScoringEffDate" CellTitle="Fraud Scoring Effective Date:" FormatType="Date" meta:resourcekey="FraudScoringEffDate" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Global ePricing--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportGlobalePricing" IsChild="true" runat="server" Title="Global ePricing" DataSourceID="uxGlobalePricing" meta:resourcekey="uxExportGlobalePricing"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxGlobalePricing" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="HostedRecurringPymts" UniqueName="HostedRecurringPymts" CellTitle="Hosted Recurring Pymts:" FormatType="DynamicString" meta:resourcekey="HostedRecurringPymts" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Payeezy--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportPayeezy" IsChild="true" runat="server" Title="Payeezy" DataSourceID="uxPayeezy" meta:resourcekey="uxExportPayeezy"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxPayeezy" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="PayeezyIndicator" UniqueName="PayeezyIndicator" CellTitle="Payeezy Indicator:" FormatType="DynamicString" meta:resourcekey="PayeezyIndicator" />
                    <as:KeyValueTableBoundRow DataField="PayeezyEffectiveDate" UniqueName="PayeezyEffectiveDate" CellTitle="Payeezy Effective Date:" FormatType="Date" meta:resourcekey="PayeezyEffectiveDate" />
                </RowCollection>
            </as:KeyValueTable>

            <%--TransArmor--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportTransArmor" IsChild="true" runat="server" Title="TransArmor" DataSourceID="uxTransArmor" meta:resourcekey="uxExportTransArmor"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxTransArmor" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="ServiceLevel" UniqueName="ServiceLevel" CellTitle="Service Level:" FormatType="DynamicString" meta:resourcekey="ServiceLevel" />
                    <as:KeyValueTableBoundRow DataField="AlianceKeyIDLevel" UniqueName="AlianceKeyIDLevel" CellTitle="Alliance Key ID/Label:" FormatType="DynamicString" meta:resourcekey="AlianceKeyIDLevel" />
                    <as:KeyValueTableBoundRow DataField="TransArmorTokenType" UniqueName="TransArmorTokenType" CellTitle="TransArmor Token Type:" FormatType="DynamicString" meta:resourcekey="TransArmorTokenType" />
                    <as:KeyValueTableBoundRow DataField="MultiPayTokenHierarchyLevel" UniqueName="MultiPayTokenHierarchyLevel" CellTitle="Multi Pay Token Hierarchy Level:" FormatType="DynamicString" meta:resourcekey="MultiPayTokenHierarchyLevel" />
                    <as:KeyValueTableBoundRow DataField="MultiPayTokenOverrideInd" UniqueName="MultiPayTokenOverrideInd" CellTitle="Multi Pay Token Override Ind:" FormatType="DynamicString" meta:resourcekey="MultiPayTokenOverrideInd" />
                    <as:KeyValueTableBoundRow DataField="EncryptionType" UniqueName="EncryptionType" CellTitle="Encryption Type:" FormatType="DynamicString" meta:resourcekey="EncryptionType" />
                    <as:KeyValueTableBoundRow DataField="VSPDomain" UniqueName="VSPDomain" CellTitle="VSP Domain:" FormatType="DynamicString" meta:resourcekey="VSPDomain" />
                    <as:KeyValueTableBoundRow DataField="VerifoneVSPBrand" UniqueName="VerifoneVSPBrand" CellTitle="Verifone VSP Brand:" FormatType="DynamicString" meta:resourcekey="VerifoneVSPBrand" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Program Details--%>          
            <div class="row">
                <div class="col-xs-12">
                    <h2 class="grid-title">
                        <span class="text-muted cursor-default">
                            <asp:Literal ID="Literal68" runat="server" Text="Program Details" meta:resourcekey="Literal68" />
                        </span>
                    </h2>
                </div>
                <div class="col-md-12 in" id="ciProgramDetails">
                    <table class="ASTable">
                        <tr>
                            <th>
                                <span class="text-muted">
                                    <asp:Literal ID="Literal31" runat="server" Text="Program Code" meta:resourcekey="Literal31" />
                                </span>
                            </th>
                            <th>
                                <span class="text-muted">
                                    <asp:Literal ID="Literal34" runat="server" Text="Program Name" meta:resourcekey="Literal34" />
                                </span>
                            </th>
                            <th>
                                <span class="text-muted">
                                    <asp:Literal ID="Literal35" runat="server" Text="Insert Timestamp" meta:resourcekey="Literal35" />
                                </span>
                            </th>
                            <th>
                                <span class="text-muted">
                                    <asp:Literal ID="Literal38" runat="server" Text="Update Timestamp" meta:resourcekey="Literal38" />
                                </span>
                            </th>
                        </tr>
                         <tr class="Row" id="programDetailNodata" runat="server" visible="false">
                             <td colspan="5" class="valign-top">
                                 <asp:Literal ID="Literal70" runat="server" Text="No data bound." meta:resourcekey="Literal70" />
                             </td>
                        </tr>
                        <asp:Repeater ID="rptProgramDetail" runat="server">
                            <ItemTemplate>
                                <tr  class="<%#Container.ItemIndex  % 2 == 1 ? "AltRow" : "Row" %>">
                                    <td class="heading  valign-top">
                                        <%# Eval("ProgramCode")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProgramName")%>
                                    </td>
                                    <td>
                                        <%# FormatDate(Eval("InsertTimeStamp"))%>
                                    </td>
                                    <td>
                                        <%# FormatDate(Eval("UpdateTimeStamp"))%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <%--Legal / IRS Information--%>
            <as:ExportKeyValueTable IsMultiExport="true" TargetID="uxLegalIRSInformation" ScrollID="legalirsinfo" ID="uxExportLegalIRSInformation" runat="server" FileName="Legal / IRS Information" Title="Legal / IRS Information" DataSourceID="uxLegalIRSInformation" meta:resourcekey="uxLegalIRSInformation"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxLegalIRSInformation" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="LegalName" UniqueName="LegalName" CellTitle="Legal Name:" FormatType="DynamicString" meta:resourcekey="LegalName" />
                    <as:KeyValueTableBoundRow DataField="Contact" UniqueName="Contact" CellTitle="Contact (First Middle Last):" FormatType="DynamicString" meta:resourcekey="Contact" />
                    <as:KeyValueTableBoundRow DataField="Title" UniqueName="Title" CellTitle="Title:" FormatType="DynamicString" meta:resourcekey="Title" />
                    <as:KeyValueTableBoundRow DataField="IRSFilingName" UniqueName="IRSFilingName" CellTitle="IRS Filing Name:" FormatType="DynamicString" meta:resourcekey="IRSFilingName" />
                    <as:KeyValueTableBoundRow DataField="TinType" UniqueName="TinType" CellTitle="Tin Type:" FormatType="DynamicString" meta:resourcekey="TinType" />
                    <as:KeyValueTableBoundRow DataField="TaxId" UniqueName="TaxId" CellTitle="Tax Id:" FormatType="DynamicString" meta:resourcekey="TaxId" />
                    <as:KeyValueTableBoundRow DataField="TurfTaxUPDReason" UniqueName="TurfTaxUPDReason" CellTitle="Turf Tax Update Reason:" FormatType="DynamicString" meta:resourcekey="TurfTaxUPDReason" />
                    <as:KeyValueTableBoundRow DataField="TurfEFFDate" UniqueName="TurfEFFDate" CellTitle="Turf EFF Date:" FormatType="Date" meta:resourcekey="TurfEFFDate" />
                    <as:KeyValueTableBoundRow DataField="TurfUPDDate" UniqueName="TurfUPDDate" CellTitle="Turf Update Date:" FormatType="Date" meta:resourcekey="TurfUPDDate" />
                    <as:KeyValueTableBoundRow DataField="SparkExclusion" UniqueName="SparkExclusion" CellTitle="Spark Exclusion:" FormatType="DynamicString" meta:resourcekey="SparkExclusion" />
                    <as:KeyValueTableBoundRow DataField="PayeeFlag" UniqueName="PayeeFlag" CellTitle="Payee Flag:" FormatType="DynamicString" meta:resourcekey="PayeeFlag" />
                    <as:KeyValueTableBoundRow DataField="TaxEffectiveYear" UniqueName="TaxEffectiveYear" CellTitle="Tax Effective Year:" FormatType="DynamicString" meta:resourcekey="TaxEffectiveYear" />
                    <as:KeyValueTableBoundRow DataField="TinValidationCode" UniqueName="TinValidationCode" CellTitle="Tin Validation Code:" FormatType="DynamicString" meta:resourcekey="TinValidationCode" />
                    <as:KeyValueTableBoundRow DataField="TinValidationDate" UniqueName="TinValidationDate" CellTitle="TIN Validation Date:" FormatType="Date" meta:resourcekey="TinValidationDate" />
                    <as:KeyValueTableBoundRow DataField="BUWNotificationFlag" UniqueName="BUWNotificationFlag" CellTitle="BUW Notification Flag:" FormatType="DynamicString" meta:resourcekey="BUWNotificationFlag" />
                    <as:KeyValueTableBoundRow DataField="BUWNotificationDate" UniqueName="BUWNotificationDate" CellTitle="BUW Notification Date:" FormatType="Date" meta:resourcekey="BUWNotificationDate" />
                    <as:KeyValueTableBoundRow DataField="CP2100ReceivedDate" UniqueName="CP2100ReceivedDate" CellTitle="CP2100 Received Date:" FormatType="Date" meta:resourcekey="CP2100ReceivedDate" />
                    <as:KeyValueTableBoundRow DataField="CP2100EffectiveDate" UniqueName="CP2100EffectiveDate" CellTitle="CP2100 Effective Date:" FormatType="Date" meta:resourcekey="CP2100EffectiveDate" />
                    <as:KeyValueTableBoundRow DataField="BUWNotification" UniqueName="BUWNotification" CellTitle="BUW Notification:" FormatType="DynamicString" meta:resourcekey="BUWNotification" />
                    <as:KeyValueTableBoundRow DataField="BUWReasonCd" UniqueName="BUWReasonCd" CellTitle="BUW Reason Code:" FormatType="DynamicString" meta:resourcekey="BUWReasonCd" />
                    <as:KeyValueTableBoundRow DataField="BUWEffDt" UniqueName="BUWEffDt" CellTitle="BUW Effective Date:" FormatType="Date" meta:resourcekey="BUWEffDt" />
                    <as:KeyValueTableBoundRow DataField="FedBUWPercentage" UniqueName="FedBUWPercentage" CellTitle="Fed BUW Percentage:" FormatType="DynamicString" meta:resourcekey="FedBUWPercentage" />
                    <as:KeyValueTableBoundRow DataField="BUWStopDt" UniqueName="BUWStopDt" CellTitle="BUW Stop Date:" FormatType="Date" meta:resourcekey="BUWStopDt" />
                    <as:KeyValueTableBoundRow DataField="StBUWPercentage" UniqueName="StBUWPercentage" CellTitle="St BUW Percentage:" FormatType="DynamicString" meta:resourcekey="StBUWPercentage" />
                    <as:KeyValueTableBoundRow DataField="FormCertStat" UniqueName="FormCertStat" CellTitle="Form Cert Stat:" FormatType="DynamicString" meta:resourcekey="FormCertStat" />
                    <as:KeyValueTableBoundRow DataField="FormCertStatRcvdDt" UniqueName="FormCertStatRcvdDt" CellTitle="Form Cert Stat Received Date:" FormatType="Date" meta:resourcekey="FormCertStatRcvdDt" />
                    <as:KeyValueTableBoundRow DataField="Last4DDANumber" UniqueName="Last4DDANumber" CellTitle="Last4 DDA Number:" FormatType="DynamicString" meta:resourcekey="Last4DDANumber" />
                    <as:KeyValueTableBoundRow DataField="Last4TaxId" UniqueName="Last4TaxId" CellTitle="Last 4 Tax Id:" FormatType="DynamicString" meta:resourcekey="Last4TaxId" />
                    <as:KeyValueTableBoundRow DataField="TaxExemptInd" UniqueName="TaxExemptInd" CellTitle="Tax Exempt Ind:" FormatType="DynamicString" meta:resourcekey="TaxExemptInd" />
                    <as:KeyValueTableBoundRow DataField="BranchDepInd" UniqueName="BranchDepInd" CellTitle="Branch Dep Ind:" FormatType="DynamicString" meta:resourcekey="BranchDepInd" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Banking Information--%>
            <as:ExportKeyValueTable IsMultiExport="true" TargetID="uxBankingInformation" ScrollID="bankinfo" ID="uxExportBankingInformation" runat="server" Title="Banking Information" DataSourceID="uxBankingInformation" meta:resourcekey="uxExportBankingInformation"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxBankingInformation" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="BankBranch" UniqueName="BankBranch" CellTitle="Bank Branch:" FormatType="DynamicString" meta:resourcekey="BankBranch" />
                    <as:KeyValueTableBoundRow DataField="DelayDays" UniqueName="DelayDays" CellTitle="Delay days:" FormatType="DynamicString" meta:resourcekey="DelayDays" />
                    <as:KeyValueTableBoundRow DataField="Last4ABA" UniqueName="Last4ABA" CellTitle="Last 4 ABA:" FormatType="DynamicString" meta:resourcekey="Last4ABA" />
                    <as:KeyValueTableBoundRow DataField="DDA1" UniqueName="DDA1" CellTitle="DDA 1:" FormatType="DynamicString" meta:resourcekey="DDA1" />
                    <as:KeyValueTableBoundRow DataField="ABA1" UniqueName="ABA1" CellTitle="ABA 1 (EncryptedRoutingNumber):" FormatType="DynamicString" meta:resourcekey="ABA1" />
                    <as:KeyValueTableBoundRow DataField="Type1" UniqueName="Type1" CellTitle="Type 1:" FormatType="DynamicString" meta:resourcekey="Type1" />
                    <as:KeyValueTableBoundRow DataField="NachDate1" UniqueName="NachDate1" CellTitle="Nach date 1:" FormatType="Date" meta:resourcekey="NachDate1" />
                    <as:KeyValueTableBoundRow DataField="OffsetInd1" UniqueName="OffsetInd1" CellTitle="Offset Ind 1:" FormatType="DynamicString" meta:resourcekey="OffsetInd1" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Funding Information--%>
            <as:ExportKeyValueTable TargetID="uxFundingDetails,uxACH,uxFundingExclusion,uxRevolvingPaymentsPlan,uxBankwireInformation,uxSplitFunding,ciFundingCategory" meta:resourcekey="uxExportFundingInformation"
                ScrollID="fundinginfo" ID="uxExportFundingInformation" runat="server" CssClass="parent-item" Title="Funding Information"></as:ExportKeyValueTable>

            <%--Funding Category--%>
            <div class="row">
                <div class="col-xs-12">
                    <h2 class="grid-title">
                        <span class="text-muted cursor-default">
                            <asp:Literal ID="Literal69" runat="server" Text="Funding Category" meta:resourcekey="Literal69" />
                        </span>
                    </h2>
                </div>
                <div class="col-md-12 in" id="ciFundingCategory">
                    <table class="ASTable">
                        <tr>
                            <th>
                                <span class="text-muted"></span>
                            </th>
                            <th>
                                <span class="text-muted">
                                    <asp:Literal ID="Literal44" runat="server" Text="DDAs" meta:resourcekey="Literal44" />
                                </span>
                            </th>
                            <th>
                                <span class="text-muted">
                                    <asp:Literal ID="Literal45" runat="server" Text="Transaction Rollup" meta:resourcekey="Literal45" />
                                </span>
                            </th>
                            <th>
                                <span class="text-muted">
                                    <asp:Literal ID="Literal48" runat="server" Text="Divert Funding" meta:resourcekey="Literal48" />
                                </span>
                            </th>
                        </tr>
                        <tr class="Row">
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal41" runat="server" Text="Deposits:" meta:resourcekey="Literal41" />
                            </td>
                            <td>
                                <%= BindValue("Deposits")%>
                            </td>
                            <td>
                                <%= BindValue("TransRUDeposits")%>
                            </td>
                            <td>
                                <%= BindValue("DivertDeposits")%>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal50" runat="server" Text="Non Bank Adjustment:" meta:resourcekey="Literal50" />
                            </td>
                            <td>
                                <%= BindValue("NonBankAdjustment")%>
                            </td>
                            <td>
                                <%= BindValue("TransRUNonBankAdjustment")%>
                            </td>
                            <td>
                                <%= BindValue("DivertNonBankAdjustment")%>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal51" runat="server" Text="Deposit Adjustment:" meta:resourcekey="Literal51" />
                            </td>
                            <td>
                                <%= BindValue("DepositAdjustment")%>
                            </td>
                            <td>
                                <%= BindValue("TransRUDepositAdjustment")%>
                            </td>
                            <td>
                                <%= BindValue("DivertDepositAdjustment")%>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal52" runat="server" Text="Chargebacks:" meta:resourcekey="Literal52" />
                            </td>
                            <td>
                                <%= BindValue("Chargebacks")%>
                            </td>
                            <td>
                                <%= BindValue("TransRUChargebacks")%>
                            </td>
                            <td>
                                <%= BindValue("DivertChargebacks")%>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal53" runat="server" Text="Chargeback/reversal:" meta:resourcekey="Literal53" />
                            </td>
                            <td>
                                <%= BindValue("Chargeback_Reversal")%>
                            </td>
                            <td>
                                <%= BindValue("TransRUChargeback_Reversal")%>
                            </td>
                            <td>
                                <%= BindValue("DivertChargeback_Reversal")%>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal54" runat="server" Text="Interchange/Amts:" meta:resourcekey="Literal54" />
                            </td>
                            <td>
                                <%= BindValue("Interchange_Asmt")%>
                            </td>
                            <td>
                                <%= BindValue("TransRUInterchange_Asmt")%>
                            </td>
                            <td>
                                <%= BindValue("DivertInterchange_Asmt")%>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal55" runat="server" Text="Disc./Serv:" meta:resourcekey="Literal55" />
                            </td>
                            <td>
                                <%= BindValue("DiscServ")%>
                            </td>
                            <td>
                                <%= BindValue("TransRUDiscServ")%>
                            </td>
                            <td>
                                <%= BindValue("DivertDiscServ")%>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal56" runat="server" Text="Fees:" meta:resourcekey="Literal56" />
                            </td>
                            <td>
                                <%= BindValue("Fees")%>
                            </td>
                            <td>
                                <%= BindValue("TransRUFees")%>
                            </td>
                            <td>
                                <%= BindValue("DivertFees")%>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading  valign-top">
                                <asp:Literal ID="Literal57" runat="server" Text="Financial Adjustment:" meta:resourcekey="Literal57" />
                            </td>
                            <td>
                                <%= BindValue("FinancialAdjustment")%>
                            </td>
                            <td>
                                <%= BindValue("TransRUFinancialAdjustment")%>
                            </td>
                            <td>
                                <%= BindValue("DivertFinancialAdjustment")%>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <%--Funding Details--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportFundingDetails" IsChild="true" runat="server" Title="Funding Details"
                DataSourceID="uxFundingDetails" meta:resourcekey="uxExportFundingDetails"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxFundingDetails" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="ConvenienceFeeABA_DDA" UniqueName="ConvenienceFeeABA_DDA" CellTitle="Convenience Fee Funding:" FormatType="DynamicString" meta:resourcekey="ConvenienceFeeABA_DDA" />
                    <as:KeyValueTableBoundRow DataField="BranchDepositor" UniqueName="BranchDepositor" CellTitle="Branch Depositor:" FormatType="DynamicString" meta:resourcekey="BranchDepositor" />
                    <as:KeyValueTableBoundRow DataField="PayCode" UniqueName="PayCode" CellTitle="Pay Code:" FormatType="DynamicString" meta:resourcekey="PayCode" />
                    <as:KeyValueTableBoundRow DataField="ClearingBankPlatformCode" UniqueName="ClearingBankPlatformCode" CellTitle="Clearingbank Platform Code:" FormatType="DynamicString" meta:resourcekey="ClearingBankPlatformCode" />
                    <as:KeyValueTableBoundRow DataField="ClearingBankICANum" UniqueName="ClearingBankICANum" CellTitle="Clearingbank ICA Num:" FormatType="DynamicString" meta:resourcekey="ClearingBankICANum" />
                    <as:KeyValueTableBoundRow DataField="ClearingbankCode_Description1" UniqueName="ClearingbankCode_Description1" CellTitle="Clearing Bank:" FormatType="DynamicString" meta:resourcekey="ClearingbankCode_Description1" />
                    <as:KeyValueTableBoundRow DataField="ClearBankEffectiveDate1" UniqueName="ClearBankEffectiveDate1" CellTitle="Effective Date:" FormatType="Date" meta:resourcekey="ClearBankEffectiveDate1" />
                    <as:KeyValueTableBoundRow DataField="ClearingbankCode_Description2" UniqueName="ClearingbankCode_Description2" CellTitle="Clearing Bank2:" FormatType="DynamicString" meta:resourcekey="ClearingbankCode_Description2" />
                    <as:KeyValueTableBoundRow DataField="ClearBankEffectiveDate2" UniqueName="ClearBankEffectiveDate2" CellTitle="Effective Date:" FormatType="Date" meta:resourcekey="ClearBankEffectiveDate2" />
                    <as:KeyValueTableBoundRow DataField="FundBankCode_Description1" UniqueName="FundBankCode_Description1" CellTitle="Funding Bank:" FormatType="DynamicString" meta:resourcekey="FundBankCode_Description1" />
                    <as:KeyValueTableBoundRow DataField="FundBankEffectiveDate1" UniqueName="FundBankEffectiveDate1" CellTitle="Effective Date:" FormatType="Date" meta:resourcekey="FundBankEffectiveDate1" />
                    <as:KeyValueTableBoundRow DataField="FundBankCode_Description2" UniqueName="FundBankCode_Description2" CellTitle="Funding Bank 2:" FormatType="DynamicString" meta:resourcekey="FundBankCode_Description2" />
                    <as:KeyValueTableBoundRow DataField="FundBankEffectiveDate2" UniqueName="FundBankEffectiveDate2" CellTitle="Effective Date:" FormatType="Date" meta:resourcekey="FundBankEffectiveDate2" />
                </RowCollection>
            </as:KeyValueTable>

            <%--ACH--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportACH" IsChild="true" runat="server" Title="ACH" FileName="ACH" DataSourceID="uxACH" meta:resourcekey="uxExportACH"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxACH" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="ACHDelayDays" UniqueName="ACHDelayDays" CellTitle="ACH Delay Days:" FormatType="DynamicString" meta:resourcekey="ACHDelayDays" />
                    <as:KeyValueTableBoundRow DataField="ACHSuspenseHoldFlag" UniqueName="ACHSuspenseHoldFlag" CellTitle="ACH Suspense Hold Flag:" FormatType="DynamicString" meta:resourcekey="ACHSuspenseHoldFlag" />
                    <as:KeyValueTableBoundRow DataField="ACHSuspenseReleaseFlag" UniqueName="ACHSuspenseReleaseFlag" CellTitle="ACH Suspense Release Flag:" FormatType="DynamicString" meta:resourcekey="ACHSuspenseReleaseFlag" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Funding Exclusion--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportFundingExclusion" IsChild="true" runat="server" Title="Funding Exclusion" DataSourceID="uxFundingExclusion" meta:resourcekey="uxExportFundingExclusion"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxFundingExclusion" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="ExclusionIndicator" UniqueName="ExclusionIndicator" CellTitle="Exclusion Indicator:" FormatType="DynamicString" meta:resourcekey="ExclusionIndicator" />
                    <as:KeyValueTableBoundRow DataField="DailyLimit" UniqueName="DailyLimit" CellTitle="Day Limit:" FormatType="DynamicString" meta:resourcekey="DailyLimit" />
                    <as:KeyValueTableBoundRow DataField="DailyOverride" UniqueName="DailyOverride" CellTitle="Daily Override:" FormatType="DynamicString" meta:resourcekey="DailyOverride" />
                    <as:KeyValueTableBoundRow DataField="DailyDate" UniqueName="DailyDate" CellTitle="Daily Date:" FormatType="DynamicString" meta:resourcekey="DailyDate" />
                    <as:KeyValueTableBoundRow DataField="Last30DayLimit" UniqueName="Last30DayLimit" CellTitle="30 Day Limit:" FormatType="DynamicString" meta:resourcekey="Last30DayLimit" />
                    <as:KeyValueTableBoundRow DataField="Last30DayOverride" UniqueName="Last30DayOverride" CellTitle="30 Day Override:" FormatType="DynamicString" meta:resourcekey="Last30DayOverride" />
                    <as:KeyValueTableBoundRow DataField="Last30DayDate" UniqueName="Last30DayDate" CellTitle="30 Day Date:" FormatType="DynamicString" meta:resourcekey="Last30DayDate" />
                    <as:KeyValueTableBoundRow DataField="TempDailyLimit" UniqueName="TempDailyLimit" CellTitle="Temp Daily Limit:" FormatType="DynamicString" meta:resourcekey="TempDailyLimit" />
                    <as:KeyValueTableBoundRow DataField="Temp30DayLimit" UniqueName="Temp30DayLimit" CellTitle="Temp 30 Day Limit:" FormatType="DynamicString" meta:resourcekey="Temp30DayLimit" />
                    <as:KeyValueTableBoundRow DataField="TempFromDate" UniqueName="TempFromDate" CellTitle="Temp From Date:" FormatType="Date" meta:resourcekey="TempFromDate" />
                    <as:KeyValueTableBoundRow DataField="TempToDate" UniqueName="TempToDate" CellTitle="Temp to Date:" FormatType="Date" meta:resourcekey="TempToDate" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Funding Exclusion--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportRevolvingPaymentsPlan" IsChild="true" runat="server" Title="Revolving Payments Plan" DataSourceID="uxRevolvingPaymentsPlan" meta:resourcekey="uxExportRevolvingPaymentsPlan"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxRevolvingPaymentsPlan" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="LastCharge" UniqueName="LastCharge" CellTitle="Last Charge:" FormatType="DynamicString" meta:resourcekey="LastCharge" />
                    <as:KeyValueTableBoundRow DataField="MinReserve" UniqueName="MinReserve" CellTitle="Min Reserve:" FormatType="DynamicString" meta:resourcekey="MinReserve" />
                    <as:KeyValueTableBoundRow DataField="RPPPercent" UniqueName="RPPPercent" CellTitle="RPP %:" FormatType="Percentage" meta:resourcekey="RPPPercent" />
                    <as:KeyValueTableBoundRow DataField="MinAmount" UniqueName="MinAmount" CellTitle="Min Amount:" FormatType="DynamicString" meta:resourcekey="MinAmount" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Funding Exclusion--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportBankwireInformation" IsChild="true" runat="server" FileName="Bankwire Information" Title="Bankwire Information" DataSourceID="uxBankwireInformation" meta:resourcekey="uxExportBankwireInformation"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxBankwireInformation" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="BankwireBeneBank" UniqueName="BankwireBeneBank" CellTitle="Bankwire Bene Bank:" FormatType="DynamicString" meta:resourcekey="BankwireBeneBank" />
                    <as:KeyValueTableBoundRow DataField="BankwireBene" UniqueName="BankwireBene" CellTitle="Bankwire Bene:" FormatType="DynamicString" meta:resourcekey="BankwireBene" />
                    <as:KeyValueTableBoundRow DataField="BankwireInfoForBene" UniqueName="BankwireInfoForBene" CellTitle="Bankwire Info For Bene:" FormatType="DynamicString" meta:resourcekey="BankwireInfoForBene" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Funding Exclusion--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExporSplitFunding" IsChild="true" runat="server" Title="Split Funding" DataSourceID="uxSplitFunding" meta:resourcekey="uxExporSplitFunding"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxSplitFunding" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="CashAdvanceVend" UniqueName="CashAdvanceVend" CellTitle="Cash Advance Vend:" FormatType="DynamicString" meta:resourcekey="CashAdvanceVend" />
                    <as:KeyValueTableBoundRow DataField="Participation" UniqueName="Participation" CellTitle="Participation:" FormatType="DynamicString" meta:resourcekey="Participation" />
                    <as:KeyValueTableBoundRow DataField="FundingSplitPct" UniqueName="FundingSplitPct" CellTitle="Fund Split Pct:" FormatType="DynamicString" meta:resourcekey="FundingSplitPct" />
                    <as:KeyValueTableBoundRow DataField="FundingAdvanceAmount" UniqueName="FundingAdvanceAmount" CellTitle="Fund Advance Amt:" FormatType="DynamicString" meta:resourcekey="FundingAdvanceAmount" />
                    <as:KeyValueTableBoundRow DataField="FundingPayToMerchant" UniqueName="FundingPayToMerchant" CellTitle="Fund Pay To Merch:" FormatType="DynamicString" meta:resourcekey="FundingPayToMerchant" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Reporting--%>
            <as:ExportKeyValueTable ScrollID="reporting" TargetID="uxProcessingEmail,uxStatement,uxReportingSettings,uxChargebackInformation" ID="uxExportReporting" runat="server" Title="Reporting" meta:resourcekey="uxExportReporting" CssClass="parent-item"></as:ExportKeyValueTable>

            <%--Processing Email--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportProcessingEmail" IsChild="true" runat="server" FileName="Processing Email" Title="Processing Email" DataSourceID="uxProcessingEmail" meta:resourcekey="uxExportProcessingEmail"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxProcessingEmail" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailTypeCD1" UniqueName="ProcessingEmailTypeCD1" CellTitle="Email Type Code 1:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailTypeCD1" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactName1" UniqueName="ProcessingEmailContactName1" CellTitle="Contact Name 1:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailContactName1" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailVal1" UniqueName="ProcessingEmailVal1" CellTitle="Email/URL Address 1:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailVal1" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactPhone1" UniqueName="ProcessingEmailContactPhone1" CellTitle="Phone No 1:" FormatType="Phone" meta:resourcekey="ProcessingEmailContactPhone1" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailTypeCD2" UniqueName="ProcessingEmailTypeCD2" CellTitle="Email Type Code 2:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailTypeCD2" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactName2" UniqueName="ProcessingEmailContactName2" CellTitle="Contact Name 2:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailContactName2" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailVal2" UniqueName="ProcessingEmailVal2" CellTitle="Email/URL Address 2:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailVal2" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactPhone2" UniqueName="ProcessingEmailContactPhone2" CellTitle="Phone No 2:" FormatType="Phone" meta:resourcekey="ProcessingEmailContactPhone2" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailTypeCD3" UniqueName="ProcessingEmailTypeCD3" CellTitle="Email Type Code 3:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailTypeCD3" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactName3" UniqueName="ProcessingEmailContactName3" CellTitle="Contact Name 3:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailContactName3" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailVal3" UniqueName="ProcessingEmailVal3" CellTitle="Email/URL Address 3:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailVal3" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactPhone3" UniqueName="ProcessingEmailContactPhone3" CellTitle="Phone No 3:" FormatType="Phone" meta:resourcekey="ProcessingEmailContactPhone3" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailTypeCD4" UniqueName="ProcessingEmailTypeCD4" CellTitle="Email Type Code 4:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailTypeCD4" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactName4" UniqueName="ProcessingEmailContactName4" CellTitle="Contact Name 4:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailContactName4" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailVal4" UniqueName="ProcessingEmailVal4" CellTitle="Email/URL Address 4:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailVal4" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactPhone4" UniqueName="ProcessingEmailContactPhone4" CellTitle="Phone No 4:" FormatType="Phone" meta:resourcekey="ProcessingEmailContactPhone4" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailTypeCD5" UniqueName="ProcessingEmailTypeCD5" CellTitle="Email Type Code 5:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailTypeCD5" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactName5" UniqueName="ProcessingEmailContactName5" CellTitle="Contact Name 5:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailContactName5" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailVal5" UniqueName="ProcessingEmailVal5" CellTitle="Email/URL Address 5:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailVal5" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactPhone5" UniqueName="ProcessingEmailContactPhone5" CellTitle="Phone No 5:" FormatType="Phone" meta:resourcekey="ProcessingEmailContactPhone5" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailTypeCD6" UniqueName="ProcessingEmailTypeCD6" CellTitle="Email Type Code 6:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailTypeCD6" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactName6" UniqueName="ProcessingEmailContactName6" CellTitle="Contact Name 6:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailContactName6" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailVal6" UniqueName="ProcessingEmailVal6" CellTitle="Email/URL Address 6:" FormatType="DynamicString" meta:resourcekey="ProcessingEmailVal6" />
                    <as:KeyValueTableBoundRow DataField="ProcessingEmailContactPhone6" UniqueName="ProcessingEmailContactPhone6" CellTitle="Phone No 6:" FormatType="Phone" meta:resourcekey="ProcessingEmailContactPhone6" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Processing Email--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportStatement" IsChild="true" runat="server" Title="Statement" DataSourceID="uxStatement" meta:resourcekey="uxExportStatement"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxStatement" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="RecapCode" UniqueName="RecapCode" CellTitle="Recap Code:" FormatType="DynamicString" meta:resourcekey="RecapCode" />
                    <as:KeyValueTableBoundRow DataField="PayCycle" UniqueName="PayCycle" CellTitle="Paycycle:" FormatType="DynamicString" meta:resourcekey="PayCycle" />
                    <as:KeyValueTableBoundRow DataField="stmtFaxNumber" UniqueName="stmtFaxNumber" CellTitle="Fax #:" FormatType="Phone" meta:resourcekey="stmtFaxNumber" />
                    <as:KeyValueTableBoundRow DataField="StatementType" UniqueName="StatementType" CellTitle="Statement Type:" FormatType="DynamicString" meta:resourcekey="StatementType" />
                    <as:KeyValueTableBoundRow DataField="DeliveryMethod" UniqueName="DeliveryMethod" CellTitle="Delivery Method:" FormatType="DynamicString" meta:resourcekey="DeliveryMethod" />
                    <as:KeyValueTableBoundRow DataField="ExternalPrintInd" UniqueName="ExternalPrintInd" CellTitle="External Print Ind:" FormatType="DynamicString" meta:resourcekey="ExternalPrintInd" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Processing Email--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportReportingSettings" IsChild="true" runat="server" Title="Reporting Settings" DataSourceID="uxReportingSettings" meta:resourcekey="uxExportReportingSettings"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxReportingSettings" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="EnhancedReporting" UniqueName="EnhancedReporting" CellTitle="Enhanced Reporting:" FormatType="DynamicString" meta:resourcekey="EnhancedReporting" />
                    <as:KeyValueTableBoundRow DataField="RejectNotification" UniqueName="RejectNotification" CellTitle="Reject Notification:" FormatType="DynamicString" meta:resourcekey="RejectNotification" />
                    <as:KeyValueTableBoundRow DataField="ConfirmationIndicator" UniqueName="ConfirmationIndicator" CellTitle="Confirmation Indicator:" FormatType="DynamicString" meta:resourcekey="ConfirmationIndicator" />
                    <as:KeyValueTableBoundRow DataField="ReportFreq" UniqueName="ReportFreq" CellTitle="Report Freq:" FormatType="DynamicString" meta:resourcekey="ReportFreq" />
                    <as:KeyValueTableBoundRow DataField="MediaType" UniqueName="MediaType" CellTitle="Media Type:" FormatType="DynamicString" meta:resourcekey="MediaType" />
                    <as:KeyValueTableBoundRow DataField="FiscalDate" UniqueName="FiscalDate" CellTitle="Fiscal Date:" FormatType="Date" meta:resourcekey="FiscalDate" />
                    <as:KeyValueTableBoundRow DataField="NotificationInd" UniqueName="NotificationInd" CellTitle="Notification Ind:" FormatType="DynamicString" meta:resourcekey="NotificationInd" />
                    <as:KeyValueTableBoundRow DataField="FundingNotificatonMthd" UniqueName="FundingNotificatonMthd" CellTitle="Funding Notification Mthd:" FormatType="DynamicString" meta:resourcekey="FundingNotificatonMthd" />
                    <as:KeyValueTableBoundRow DataField="FundingNotificationContact" UniqueName="FundingNotificationContact" CellTitle="Contact:" FormatType="DynamicString" meta:resourcekey="FundingNotificationContact" />
                    <as:KeyValueTableBoundRow DataField="FundingNotificationPhone" UniqueName="FundingNotificationPhone" CellTitle="Phone #:" FormatType="Phone" meta:resourcekey="FundingNotificationPhone" />
                    <as:KeyValueTableBoundRow DataField="FundingNotificationFax" UniqueName="FundingNotificationFax" CellTitle="Fax #:" FormatType="Phone" meta:resourcekey="FundingNotificationFax" />
                </RowCollection>
            </as:KeyValueTable>

            <%--Processing Email--%>
            <as:ExportKeyValueTable IsMultiExport="true" ID="uxExportChargebackInformation" IsChild="true" runat="server" Title="Chargeback Information" DataSourceID="uxChargebackInformation" meta:resourcekey="uxExportChargebackInformation"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxChargebackInformation" OnNeedDataSource="GetDataSource" runat="server">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="ChargebackRetMediaAddress" UniqueName="ChargebackRetMediaAddress" CellTitle="Chargeback/Ret Media Address:" FormatType="DynamicString" meta:resourcekey="ChargebackRetMediaAddress" />
                    <as:KeyValueTableBoundRow DataField="HardCopyReportDestination" UniqueName="HardCopyReportDestination" CellTitle="Hardcopy Report Destination:" FormatType="DynamicString" meta:resourcekey="HardCopyReportDestination" />
                    <as:KeyValueTableBoundRow DataField="HoldChargebacks" UniqueName="HoldChargebacks" CellTitle="Hold Chargebacks:" FormatType="DynamicString" meta:resourcekey="HoldChargebacks" />
                    <as:KeyValueTableBoundRow DataField="DisputeDisposition" UniqueName="DisputeDisposition" CellTitle="Dispute Disposition:" FormatType="DynamicString" meta:resourcekey="DisputeDisposition" />
                    <as:KeyValueTableBoundRow DataField="PreNoteInd" UniqueName="PreNoteInd" CellTitle="Pre-Note Ind:" FormatType="DynamicString" meta:resourcekey="PreNoteInd" />
                    <as:KeyValueTableBoundRow DataField="PreNoteDays" UniqueName="PreNoteDays" CellTitle="Pre-Note Days:" FormatType="DynamicString" meta:resourcekey="PreNoteDays" />
                    <as:KeyValueTableBoundRow DataField="ExcessiveChargebackInd" UniqueName="ExcessiveChargebackInd" CellTitle="Excessive Chargeback Indd:" FormatType="DynamicString" meta:resourcekey="ExcessiveChargebackInd" />
                </RowCollection>
            </as:KeyValueTable>
        </div>
    </div>
</asp:PlaceHolder>

<as:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MIF_MerchantDetails_NORTH.js"></script>
</as:RadCodeBlock>
