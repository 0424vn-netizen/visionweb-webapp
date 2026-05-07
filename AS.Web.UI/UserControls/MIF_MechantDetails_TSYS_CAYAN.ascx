<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MechantDetails_TSYS_CAYAN.ascx.cs" Inherits="UserControls_MIF_MechantDetails_TSYS_CAYAN" %>
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
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal8" runat="server" Text="Address:" meta:resourcekey="LiteralResource46" />
                    </td>
                    <td>
                        <%= BindAddress(BindValue("Address1"), BindValue("Address2"), BindValue("Address3"), BindValue("City"), BindValue("State"), BindValue("Zip"))%>
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal34" runat="server" Text="Email:" meta:resourcekey="LiteralResource49" />
                    </td>
                    <td colspan="3">
                        <%= BindValue("Email")%>
                    </td>
                </tr>
                <as:Panel runat="server" ID="uxPnlRelationshipManager">
                    <tr class="AltRow">
                        <td class="heading valign-middle">
                            <asp:Literal ID="Literal9" runat="server" Text="Relationship Manager:" />
                        </td>
                        <td colspan="5">
                            <as:Literal runat="server" ID="uxlbRelationshipManager"></as:Literal>
                            <as:LinkButton ID="uxbtnAdd" OnClick="uxbtnAdd_click" Text="Add RM" runat="server" />
                            <as:LinkButton ID="uxbtnEdit" OnClick="uxbtnEdit_click" runat="server" Text="Edit" />
                            <as:Panel ID="uxpnlSaveCancel" runat="server" Visible="false">
                                <as:TextBox runat="server" ID="uxtxtRelationShipManager" MaxLength="30" />
                                <as:LinkButton ID="uxbtnSave" OnClick="uxbtnSave_click" OnClientClick="return CheckPrefix();"
                                    runat="server" Text="Save" />
                                /
                                <as:LinkButton ID="uxbtnCancel" OnClick="uxbtnCancel_click" runat="server" Text="Cancel" />
                            </as:Panel>
                        </td>
                    </tr>
                </as:Panel>
                <asp:Panel ID="uxPnlUserID" runat="server" Visible="false">
                    <tr id="trUserID">
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
                                            <asp:Literal ID="Literal13" runat="server" Text="Approval Date:" meta:resourcekey="LiteralResource11" /></td>
                                        <td>
                                            <%# FormatDate(Eval("ApprovalDate"))%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal14" runat="server" Text="Closed Date:" meta:resourcekey="LiteralResource12" /></td>
                                        <td>
                                            <%# FormatDate(Eval("ClosedDate"))%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal15" runat="server" Text="Status:" meta:resourcekey="LiteralResource13" /></td>
                                        <td>
                                            <%# Eval("ActivityStatus")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading valign-middle">
                                            <asp:Literal ID="Literal16" runat="server" Text="Site Access:" meta:resourcekey="LiteralResource14" /></td>
                                        <td>
                                            <div class="opt-in-out-text <%# HasMSProductEnvironment()? "mt-1x" :"" %>">
                                                <asp:Literal ID="ltrSiteAccess" runat="server" Text='<%# HasMSProductEnvironment()? Eval("SiteAccess") : GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_NotAvailable").ToString() %>' />
                                            </div>
                                            <%--<div class="pull-right opt-in-out-button" visible='<%# !IsCaseManagement %>'>
                                                <as:PlaceHolder runat="server" ID="PlaceHolder1">
                                                    <as:Button ID="uxSiteAccess" runat="server" Text='<%# SetStatusText(Eval("SiteAccess")) %>'
                                                        Visible='<%# SetVisible(Eval("SiteAccess")) %>' OnClientClick='<%# SetURLForSiteAccessButton() %>' IsStandardButton="False" meta:resourcekey="uxSiteAccessResource1" />
                                                </as:PlaceHolder>
                                            </div>--%>
                                        </td>
                                    </tr>
                                     <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal23" runat="server" Text="LegalAddress" meta:resourcekey="LiteralResource50" /></td>
                                        <td>
                                            <%= BindAddress(BindValue("LegalAddress"), string.Empty, string.Empty, BindValue("LegalCity"), BindValue("LegalState"), BindValue("LegalZip"))%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal17" runat="server" Text="Tax ID" meta:resourcekey="LiteralResource15" /></td>
                                        <td>
                                            <%#CheckPermisson(Eval("TaxID"), WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal18" runat="server" Text="SIC/MCC:" meta:resourcekey="LiteralResource16" /></td>
                                        <td>
                                            <%#FormatSIC(Eval("SICCode"), Eval("SICCodeDesc"))%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal19" runat="server" Text="Average Sales Amount:" meta:resourcekey="LiteralResource17" /></td>
                                        <td>
                                            <%#FormatCurrency(Eval("Avg_Ticket_Amt"))%>
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
                                    <asp:Repeater ID="rptBank" runat="server" EnableViewState="true">
                                        <HeaderTemplate>

                                            <tr class="Row">
                                                <td class="heading" style="width: 50%;">
                                                    <asp:Literal ID="Literal24" runat="server" Text="Routing #:" meta:resourcekey="LiteralResource22" />
                                                </td>
                                                <td class="heading">
                                                    <asp:Literal ID="Literal25" runat="server" Text="DDA #:" meta:resourcekey="LiteralResource23" />
                                                </td>
                                            </tr>                                            
                                        </HeaderTemplate>
                                        
                                        <ItemTemplate>
                                            <tr class="<%# Container.ItemIndex % 2  ==0 ? "AltRow":"Row" %>">
                                                <td>
                                                    <%# GetRoutingNumber(Eval("RoutingNumber").ToString(), Eval("PartialRoutingNumber").ToString())%>&nbsp;
                                                </td>
                                                <td>
                                                    <%#GetDDANumber(Eval("DDANumber").ToString(), Eval("PartialDDANumber").ToString())%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>                                        
                                        <FooterTemplate>
                                            <tr id="trEmpty" runat="server" visible="false">
                                                <td>
                                                    &mdash;
                                                </td>
                                                <td>
                                                    &mdash;
                                                </td>
                                            </tr>
                                        </FooterTemplate>
                                    </asp:Repeater>
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
                                            <asp:Literal ID="Literal29" runat="server" Text="Client Name:" meta:resourcekey="LiteralResource27" /></td>
                                        <td>
                                            <%# Eval("ClientName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal30" runat="server" Text="Client Login:" meta:resourcekey="LiteralResource28" /></td>
                                        <td>
                                            <%# Eval("ClientLogin")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="hierarchy" colspan="2">
                                            <b>
                                                <asp:Literal ID="Literal31" runat="server" Text="Merchant Hierarchy" meta:resourcekey="LiteralResource29" /></b>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal32" runat="server" Text="Bank:" meta:resourcekey="LiteralResource31" />
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxBank" runat="server" Visible='<%# CheckHierarchy("CAYANBNK") %>'>
                                                <a href="#" id="uxLinkBank" runat="server"><%= BindValue("EntityNumber5")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal12" runat="server" Text='<%# Eval("EntityNumber5") %>' Visible='<%# !CheckHierarchy("CAYANBNK") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal33" runat="server" Text="Association:" meta:resourcekey="LiteralResource34" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxAssociation" runat="server" Visible='<%# CheckHierarchy("CAYANASSO") %>'>
                                                <a href="#" id="uxLinkAssociation" runat="server"><%= BindValue("EntityNumber6")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal21" runat="server" Text='<%# Eval("EntityNumber6") %>' Visible='<%# !CheckHierarchy("CAYANASSO") %>'></as:Literal>
                                        </td>
                                    </tr>

                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal40" runat="server" Text="Chain:" meta:resourcekey="LiteralResource38" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxChain" runat="server" Visible='<%# CheckHierarchy("CHAIN") %>'>
                                                <a href="#" id="uxLinkChain" runat="server"><%= BindValue("Chain")%></a>
                                            </as:PlaceHolder>
                                          <!--TK: 42782 - Branding them and Chain logic-->
                                            <as:Literal ID="uxLHChain" runat="server" Text='<%# Eval("Chain") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal11" runat="server" Text="Sales Agent:" meta:resourcekey="LiteralResource37" /></td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# CheckHierarchy("SALESAGENT") %>'>
                                                <a href="#" id="uxLinkSalesAgent" runat="server"><%= BindValue("EntityNumber4")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxSalesAgent" runat="server" Visible='<%# !CheckHierarchy("SALESAGENT") %>' Text='<%# Eval("EntityNumber4") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal22" runat="server" Text="Sub-Sales Agent:" meta:resourcekey="LiteralResource60" /></td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# CheckHierarchy("CAYANSSA") %>'>
                                                <a href="#" id="uxLinkSubSalesAgent" runat="server"><%= BindValue("EntityNumber7")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxSubSalesAgent" runat="server" Visible='<%# !CheckHierarchy("CAYANSSA") %>' Text='<%# Eval("EntityNumber7") %>'></as:Literal></td>
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
                                            <asp:Literal ID="Literal45" runat="server" Text="Status" meta:resourcekey="LiteralResource43" /></td>
                                        <td class="heading">
                                            <asp:Literal ID="Literal46" runat="server" Text="Effective Date" meta:resourcekey="LiteralResource44" /></td>
                                    </tr>
                                    <tr class="Row">
                                        <td>
                                            <asp:Literal ID="Literal47" runat="server" Text="Discover MAP" meta:resourcekey="LiteralResource45" /></td>
                                        <td>
                                            <%# FormatStatusAccountInfo(Eval("DiscoverRetained")) %>
                                        </td>
                                        <td class="text-left">
                                            <%# FormatEffectiveDate(Eval("DiscoverRetainedEffectiveDate"), Eval("DiscoverRetained"))  %>
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
    <as:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var MIF_MerchantDetails_FIS_uxtxtRelationShipManager = '#<%= uxtxtRelationShipManager.ClientID %>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MIF_MechantDetails_TSYS_CAYAN.js"></script>
    </as:RadCodeBlock>
</asp:PlaceHolder>
