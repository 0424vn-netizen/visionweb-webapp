<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MerchantDetails_CLEARENT.ascx.cs" Inherits="UserControls_MIF_MerchantDetails_CLEARENT" %>
<%@ Register TagPrefix="uc" TagName="RiskInfo" Src="~/UserControls/MIF_RiskInformationSection.ascx" %>

<asp:PlaceHolder ID="phdMerchantDetail" runat="server">
    <div class="row" id="merchinfo">
        <div class="col-md-9" data-toggle="collapse" data-target="#ciMerchantInformation">
            <h2 class="grid-title mt-3x">
                <asp:Literal ID="Literal1" runat="server" Text="Merchant Information" meta:resourcekey="LiteralResource1" />
            </h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 in" id="ciMerchantInformation">
            <table class="ASTable" id="tblMerchantInformation">
                <colgroup>
                    <col style="width: 110px" />
                    <col style="width: 320px" />
                    <col style="width: 65px" />
                    <col />
                    <col style="width: 125px" />
                    <col />
                </colgroup>
                <tr class="Row">
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal2" runat="server" Text="Merchant ID:" meta:resourcekey="LiteralResource2" />
                    </td>
                    <td>
                        <%= BindValue("MerchantNumber")%>
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal3" runat="server" Text="Contact:" meta:resourcekey="LiteralResource3" />
                    </td>
                    <td>
                        <%= BindValue("Contact")%>
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal4" runat="server" Text="Status:" meta:resourcekey="LiteralResource4" />
                    </td>
                    <td>
                        <%= BindValue("Status")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal5" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource5" />
                    </td>
                    <td>
                        <%= BindValue("MerchantName")%>
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal6" runat="server" Text="Phone:" meta:resourcekey="LiteralResource6" />
                    </td>
                    <td>
                        <%= FormatPhone(BindValue("Phone"))%>
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal7" runat="server" Text="Last Batch Activity:" meta:resourcekey="LiteralResource7" />
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkLastBatch"
                            OnClick="lnkLastBatch_Click" meta:resourcekey="lnkLastBatchResource1" />
                        <asp:Literal ID="ltrLastBatch" Visible="false" runat="server" Text="<%# WebSiteConstants.HTML_EM_DASH_ENCODE %>" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal22" runat="server" Text="Owner Name:" meta:resourcekey="LiteralResource58" />
                    </td>
                    <td>
                        <%= BindValue("OwnerName")%>
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal8" runat="server" Text="Address:" meta:resourcekey="LiteralResource46" />
                    </td>
                    <td>
                        <%= BindAddress(BindValueNoEMDash("Address1"), BindValueNoEMDash("Address2"), BindValueNoEMDash("Address3"), BindValueNoEMDash("City"), BindValueNoEMDash("State"), BindValueNoEMDash("Zip"))%>
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal34" runat="server" Text="Email:" meta:resourcekey="LiteralResource49" />
                    </td>
                    <td>
                        <%= BindValue("Email")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal23" runat="server" Text="Owner SSN:" meta:resourcekey="LiteralResource59" />
                    </td>
                    <td>
                        <%= CheckPermisson(BindValue("OwnerSSN"), WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal28" runat="server" Text="Fax:" meta:resourcekey="LiteralResource60" />
                    </td>
                    <td>
                        <%= FormatPhone(BindValueNoEMDash("Fax"))%>
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal59" runat="server" Text="WebSite:" meta:resourcekey="LiteralResource88" />
                    </td>
                    <td>
                        <a id="uxLinkWebsite" runat="server" href="#" target="_blank" visible="false"><%= BindValue("MerchantWebsite")%></a>
                        <asp:Literal ID="ltrLinkWebsite" Visible="true" runat="server" Text="<%# WebSiteConstants.HTML_EM_DASH_ENCODE %>" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <asp:Repeater ID="rptMerchantInfo" runat="server" EnableViewState="true" OnItemDataBound="uxMerchantInfoItemDataBound">
        <ItemTemplate>
            <div class="row">
                <div class="col-md-6">
                    <asp:PlaceHolder ID="phdBusinessInfo" runat="server">
                        <div class="row" id="businessinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciBusinessInformation">
                                <h2 class="grid-title">
                                    <asp:Literal ID="Literal10" runat="server" Text="Business Information" meta:resourcekey="LiteralResource8" /></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciBusinessInformation">
                                <table class="ASTable">
                                    <colgroup>
                                        <col class="w-40" />
                                        <col class="w-60" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal19" runat="server" Text="Corporate Name:" meta:resourcekey="LiteralResource61" /></td>
                                        <td>
                                            <%# BindValue("CorporateName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal27" runat="server" Text="Corporate Address:" meta:resourcekey="LiteralResource62" /></td>
                                        <td>
                                            <%# BindAddress(Eval("CorporateAddress"),string.Empty, string.Empty, Eval("CorporateCity"), Eval("CorporateState"), Eval("CorporateZip"))%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal35" runat="server" Text="Business Type:" meta:resourcekey="LiteralResource63" /></td>
                                        <td>
                                            <%# BindValue("BusinessType")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal13" runat="server" Text="Approval Date:" meta:resourcekey="LiteralResource11" /></td>
                                        <td>
                                            <%# FormatDate(Eval("ApprovalDate"))%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal14" runat="server" Text="Closed Date:" meta:resourcekey="LiteralResource12" /></td>
                                        <td>
                                            <%# FormatDate(Eval("ClosedDate"))%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal15" runat="server" Text="Status:" meta:resourcekey="LiteralResource13" /></td>
                                        <td>
                                            <%# BindValue("ActivityStatus")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal36" runat="server" Text="Seasonal:" meta:resourcekey="LiteralResource64" /></td>
                                        <td>
                                            <%# BindValue("Seasonal")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading valign-middle">
                                            <asp:Literal ID="Literal16" runat="server" Text="Site Access:" meta:resourcekey="LiteralResource14" /></td>
                                        <td>
                                            <div class=" <%# HasMSProductEnvironment()? "mt-1x" :"" %>">
                                                <asp:Literal ID="ltrSiteAccess" runat="server" Text='<%# HasMSProductEnvironment()? BindValue("SiteAccess") : GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_NotAvailable").ToString() %>' />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal17" runat="server" Text="Tax ID" meta:resourcekey="LiteralResource15" /></td>
                                        <td>
                                            <%# CheckPermisson(BindValue("TaxID"), WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal18" runat="server" Text="SIC/MCC:" meta:resourcekey="LiteralResource16" /></td>
                                        <td>
                                            <%#FormatSIC(BindValue("SICCode"), BindValue("SICCodeDesc"))%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phdContractualInfo" runat="server">
                        <div class="row" id="contractualInfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciContractualInfo">
                                <h2 class="grid-title">
                                    <asp:Literal ID="Literal21" runat="server" Text="Contractual Information" meta:resourcekey="LiteralResource80" /></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciContractualInfo">
                                <table class="ASTable">
                                    <colgroup>
                                        <col class="w-40" />
                                        <col class="w-60" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal43" runat="server" Text="Annual Volume:" meta:resourcekey="LiteralResource74" /></td>
                                        <td>
                                            <%# FormatCurrency(Eval("AnnualVolume"))%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal47" runat="server" Text="Average Ticket:" meta:resourcekey="LiteralResource75" /></td>
                                        <td>
                                            <%# FormatCurrency(Eval("AverageTicket"))%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal48" runat="server" Text="High Ticket:" meta:resourcekey="LiteralResource76" /></td>
                                        <td>
                                            <%# FormatCurrency(Eval("HighestTicket"))%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal49" runat="server" Text="%MOTO:" meta:resourcekey="LiteralResource77" /></td>
                                        <td>
                                            <%# FormatPercent(Eval("Moto"))%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal50" runat="server" Text="%eCommerce:" meta:resourcekey="LiteralResource78" /></td>
                                        <td>
                                            <%# FormatPercent(Eval("eCommerce"))%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal51" runat="server" Text="%Card Not Present:" meta:resourcekey="LiteralResource79" /></td>
                                        <td>
                                            <%# FormatPercent(Eval("CardNotPresent"))%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                </div>
                <div class="col-md-6">
                    <!-- Risk Info -->
                    <uc:RiskInfo ID="uxRiskInfo" runat="server" />
                    <asp:PlaceHolder ID="phdHierarchy" runat="server">
                        <div class="row" id="hierarchyinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciHierarchyInformation">
                                <h2 class="grid-title">
                                    <asp:Literal ID="Literal26" runat="server" Text="Hierarchy Information" meta:resourcekey="LiteralResource24" /></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciHierarchyInformation">
                                <table class="ASTable">
                                    <colgroup>
                                        <col class="w-45" />
                                        <col class="w-55" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal29" runat="server" Text="Sponsoring Bank:" meta:resourcekey="LiteralResource65" /></td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# CheckHierarchy(HierarchyMode.SPONSORING_BANK) %>'>
                                                <a href="#" id="uxLinkSpoBank" runat="server"><%= BindValue("SponsoringBank")%></a>
                                                <as:Literal ID="uxMDashSponsoringBank" runat="server" Visible='false' Text='<%# BindValue("SponsoringBank") %>'></as:Literal>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal38" runat="server" Visible='<%# !CheckHierarchy(HierarchyMode.SPONSORING_BANK) %>' Text='<%# BindValue("SponsoringBank") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal60" runat="server" Text="Sponsoring Bank User ID:" meta:resourcekey="lblSponsoringBankUserID" /></td>
                                        <td>
                                            <as:Literal ID="Literal61" runat="server" Text='<%# BindValue("SponsoringBankUserID") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal30" runat="server" Text="Sponsoring Bank BIN:" meta:resourcekey="LiteralResource66" /></td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# CheckHierarchy(HierarchyMode.SPONSORING_BANK_BIN) %>'>
                                                <a href="#" id="uxLinkSpoBankBin" runat="server"><%= BindValue("SponsoringBankBIN")%></a>
                                                <as:Literal ID="uxMDashSponsoringBankBIN" runat="server" Visible='false' Text='<%# BindValue("SponsoringBankBIN") %>'></as:Literal>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal39" runat="server" Visible='<%# !CheckHierarchy(HierarchyMode.SPONSORING_BANK_BIN) %>' Text='<%# BindValue("SponsoringBankBIN") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal37" runat="server" Text="Sales Channel:" meta:resourcekey="LiteralResource67" /></td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# CheckHierarchy(HierarchyMode.PROCESSING_PLATFORM) %>'>
                                                <a href="#" id="uxLinkProPlatform" runat="server"><%= BindValue("ProcessingPlatform")%></a>
                                                <as:Literal ID="uxMDashProcessingPlatform" runat="server" Visible='false' Text='<%# BindValue("ProcessingPlatform") %>'></as:Literal>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal42" runat="server" Visible='<%# !CheckHierarchy(HierarchyMode.PROCESSING_PLATFORM) %>' Text='<%# BindValue("ProcessingPlatform") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal64" runat="server" Text="Sales Channel User ID:" meta:resourcekey="lblSalesChannelUserID" /></td>
                                        <td>
                                            <as:Literal ID="Literal65" runat="server" Text='<%# BindValue("SalesChannelUserID") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal24" runat="server" Text="Reseller:" meta:resourcekey="LiteralResource86" /></td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# CheckHierarchy(HierarchyMode.RESELLER) %>'>
                                                <a href="#" id="uxLinkResellerName" runat="server"><%= BindValue("ResellerName")%></a>
                                                <as:Literal ID="uxMDashResellerName" runat="server" Visible='false' Text='<%# BindValue("ResellerName") %>'></as:Literal>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal25" runat="server" Visible='<%# !CheckHierarchy(HierarchyMode.RESELLER) %>' Text='<%# BindValue("ResellerName") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal62" runat="server" Text="Reseller User ID:" meta:resourcekey="lblResellerUserID" /></td>
                                        <td>
                                            <as:Literal ID="Literal63" runat="server" Text='<%# BindValue("ResellerUserID") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal54" runat="server" Text="Partner:" meta:resourcekey="LiteralResource87" /></td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder5" runat="server" Visible='<%# CheckHierarchy(HierarchyMode.PARTNER) %>'>
                                                <a href="#" id="uxLinkPartnerName" runat="server"><%= BindValue("PartnerName")%></a>
                                                <as:Literal ID="uxMDashPartnerName" runat="server" Visible='false' Text='<%# BindValue("PartnerName") %>'></as:Literal>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal58" runat="server" Visible='<%# !CheckHierarchy(HierarchyMode.PARTNER) %>' Text='<%# BindValue("PartnerName") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal40" runat="server" Text="Chain:" meta:resourcekey="LiteralResource38" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxChain" runat="server" Visible='<%# CheckHierarchy("CHAIN") %>'>
                                                <a href="#" id="uxLinkChain" runat="server"><%= BindValue("Chain")%></a>
                                                <as:Literal ID="uxMDashChain" runat="server" Visible='false' Text='<%# BindValue("Chain") %>'></as:Literal>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHChain" runat="server" Visible='<%# !CheckHierarchy("CHAIN") %>' Text='<%# BindValue("Chain") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="hierarchy" colspan="2">
                                            <b>
                                                <asp:Literal ID="Literal31" runat="server" Text="Processor Information" meta:resourcekey="LiteralResource84" /></b>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal32" runat="server" Text="FrontEnd Processor:" meta:resourcekey="LiteralResource68" />
                                        </td>
                                        <td>
                                            <%# BindValue("FrontEndProcessor")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal33" runat="server" Text="FrontEnd MID:" meta:resourcekey="LiteralResource69" /></td>
                                        <td>
                                            <%# BindValue("FrontEndMID")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal11" runat="server" Text="BackEnd Processor:" meta:resourcekey="LiteralResource70" /></td>
                                        <td>
                                            <%# BindValue("BackEndProcessor")%>    
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal12" runat="server" Text="BackEnd MID:" meta:resourcekey="LiteralResource71" /></td>
                                        <td>
                                            <%# BindValue("BackEndMID")%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="uxAccountExport">
                        <div class="row" id="accountInfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciAccountInformation">
                                <h2 class="grid-title">
                                    <asp:Literal ID="Literal41" runat="server" Text="Account Information" meta:resourcekey="LiteralResource39" /></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciAccountInformation">
                                <table class="ASTable">
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal44" runat="server" Text="Account Information" meta:resourcekey="LiteralResource42" /></td>
                                        <td class="heading">
                                            <asp:Literal ID="Literal45" runat="server" Text="Program" meta:resourcekey="LiteralResource72" /></td>
                                        <td class="heading">
                                            <asp:Literal ID="Literal46" runat="server" Text="Account Number" meta:resourcekey="LiteralResource73" /></td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td>
                                            <asp:Literal ID="Literal9" runat="server" Text="American Express" meta:resourcekey="LiteralResource81" />
                                        <td>
                                            <%# BindValue("AmexProgram")  %>
                                        </td>
                                        <td>
                                            <%# BindValue("AMEXAccountNumber")  %>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td>
                                            <asp:Literal ID="Literal52" runat="server" Text="Discover" meta:resourcekey="LiteralResource82" />
                                        <td>
                                            <%# BindValue("DiscoverProgram")  %>
                                        </td>
                                        <td>
                                            <%# BindValue("DiscoverAccountNumber")  %>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td>
                                            <asp:Literal ID="Literal53" runat="server" Text="Diners Club" meta:resourcekey="LiteralResource83" />
                                        <td>
                                            <%# BindValue("DinersClubProgram")  %>
                                        </td>
                                        <td>
                                            <%# BindValue("DinersClubAccountNumber")  %>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phdBankInfo" runat="server">
                        <div class="row" id="bankinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciBankInformation">
                                <h2 class="grid-title">
                                    <asp:Literal ID="Literal20" runat="server" Text="Bank Information" meta:resourcekey="LiteralResource18" /></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciBankInformation">
                                <table class="ASTable">
                                    <colgroup>
                                        <col class="w-40" />
                                        <col class="w-60" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal55" runat="server" Text="Bank Name:" meta:resourcekey="LiteralResource85" /></td>
                                        <td>
                                            <%# BindValue("MerchantBankName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal56" runat="server" Text="Routing #:" meta:resourcekey="LiteralResource22" /></td>
                                        <td>
                                            <%# GetRoutingNumber(Eval("RoutingNumber").ToString(), Eval("PartialRoutingNumber").ToString()) %>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal57" runat="server" Text="DDA #:" meta:resourcekey="LiteralResource23" /></td>
                                        <td>
                                            <%# CheckPermisson(Eval("DDANumber").ToString(), WebSiteConstants.SEC_PERMISSION_DDA) %>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:PlaceHolder>
