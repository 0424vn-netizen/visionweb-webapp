<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MerchantDetails_PCS.ascx.cs" Inherits="UserControls_MIF_MerchantDetails_PCS" %>
<%@ Register TagPrefix="uc" TagName="RiskInfo" Src="~/UserControls/MIF_RiskInformationSection.ascx" %>


<asp:PlaceHolder ID="phdMerchantDetail" runat="server">
    <div class="row" id="merchinfo">
        <div class="col-md-9" data-toggle="collapse" data-target="#ciMerchantInformation">
            <h2 class="grid-title mt-3x">
                <as:Literal meta:resourcekey="ltMerchantInfoResource1" ID="ltMerchantInfo" runat="server" Text="Merchant Information"></as:Literal>
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
                        <as:Literal meta:resourcekey="Literal1Resource1" ID="Literal1" runat="server" Text="Merchant ID:"></as:Literal>
                    </td>
                    <td>
                        <%= BindValue("MerchantNumber")%>
                    </td>
                    <td class="heading valign-middle">
                        <as:Literal meta:resourcekey="Literal2Resource1" ID="Literal2" runat="server" Text="Contact:"></as:Literal>
                    </td>
                    <td>
                        <%= BindValue("Contact")%>
                    </td>
                    <td class="heading valign-middle">
                        <as:Literal meta:resourcekey="Literal3Resource1" ID="Literal3" runat="server" Text="Status:"></as:Literal>
                    </td>
                    <td>
                        <%= BindValue("Status")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-middle">
                        <as:Literal meta:resourcekey="Literal4Resource1" ID="Literal4" runat="server" Text="Merchant Name:"></as:Literal>
                    </td>
                    <td>
                        <%= BindValue("MerchantName")%>
                    </td>
                    <td class="heading valign-middle">
                        <as:Literal meta:resourcekey="Literal5Resource1" ID="Literal5" runat="server" Text="Phone:"></as:Literal>
                    </td>
                    <td>
                        <%= FormatPhone(BindValue("Phone"))%>
                    </td>
                    <td class="heading valign-middle">
                        <as:Literal meta:resourcekey="Literal6Resource1" ID="Literal6" runat="server" Text="Last Batch Activity:"></as:Literal>
                    </td>
                    <td>
                        <as:LinkButton runat="server" ID="lnkLastBatch" Text="" OnClick="lnkLastBatch_Click" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-middle">
                        <as:Literal meta:resourcekey="Literal7Resource1" ID="Literal7" runat="server" Text="Address:"></as:Literal>
                    </td>
                    <td>
                        <%= BindAddress(BindValue("Address1"), BindValue("Address2"), BindValue("Address3"), BindValue("City"), BindValue("State"), BindValue("Zip"))%>
                    </td>
                    <td class="heading valign-middle">
                        <as:Literal meta:resourcekey="Literal81Resource1" ID="Literal81" runat="server" Text="Email:"></as:Literal>
                    </td>
                    <td colspan="3">
                        <%= BindValue("Email")%>
                    </td>
                </tr>

                <tr class="AltRow">
                    <as:Panel runat="server" ID="uxPnlRelationshipManager">
                        <td class="heading valign-middle">
                            <as:Literal meta:resourcekey="Literal9Resource1" ID="Literal9" runat="server" Text="Relationship Manager:"></as:Literal>
                        </td>
                        <td colspan="5">
                            <as:Literal runat="server" ID="uxlbRelationshipManager"></as:Literal>
                            <as:LinkButton ID="uxbtnAdd" OnClick="uxbtnAdd_click" Text="Add RM" runat="server" meta:resourcekey="uxbtnAddResource1" />
                            <as:LinkButton ID="uxbtnEdit" OnClick="uxbtnEdit_click" runat="server" Text="Edit" meta:resourcekey="uxbtnEditResource1" />
                            <as:Panel ID="uxpnlSaveCancel" runat="server" Visible="false">
                                <as:TextBox runat="server" ID="uxtxtRelationShipManager" MaxLength="30" />
                                <as:LinkButton ID="uxbtnSave" OnClick="uxbtnSave_click" OnClientClick="return CheckPrefix();"
                                    runat="server" Text="Save" meta:resourcekey="uxbtnSaveResource1" />
                                /
                            <as:LinkButton ID="uxbtnCancel" OnClick="uxbtnCancel_click" runat="server" Text="Cancel" meta:resourcekey="uxbtnCancelResource1" />
                            </as:Panel>
                        </td>
                    </as:Panel>
                </tr>
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

    <asp:Repeater ID="rptMerchantInfo" runat="server" EnableViewState="true">
        <ItemTemplate>
            <div class="row">
                <div class="col-md-6">
                    <as:PlaceHolder ID="phdBusinessInfo" runat="server">
                        <div class="row" id="businessinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciBusinessInformation">
                                <h2 class="grid-title">
                                    <as:Literal meta:resourcekey="Literal8Resource1" ID="Literal8" runat="server" Text="Business Information"></as:Literal></h2>
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
                                            <as:Literal meta:resourcekey="Literal11Resource1" ID="Literal11" runat="server" Text="Approval Date:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# FormatDate(Eval("ApprovalDate"))%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal12Resource1" ID="Literal12" runat="server" Text="Closed Date:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# FormatDate(Eval("ClosedDate"))%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal13Resource1" ID="Literal13" runat="server" Text="Status:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# Eval("Status")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal14Resource1" ID="Literal14" runat="server" Text="Tax ID:"></as:Literal>
                                        </td>
                                        <td>
                                            <%#CheckPermisson(Eval("TaxID"), WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal15Resource1" ID="Literal15" runat="server" Text="SIC/MCC:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# FormatSIC(Eval("SICMCC"), Eval("SICDescription"))%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal16Resource1" ID="Literal16" runat="server" Text="Average Ticket:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# FormatCurrency(Eval("AverageTicket"))%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal17Resource1" ID="Literal17" runat="server" Text="Annual Volume:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# FormatCurrency(Eval("AnnualVolume")) %>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="phdBankInfo" runat="server">
                        <div class="row" id="bankinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciBankInformation">
                                <h2 class="grid-title">
                                    <as:Literal meta:resourcekey="Literal18Resource1" ID="Literal18" runat="server" Text="Bank Information"></as:Literal></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciBankInformation">
                                <table class="ASTable">
                                    <tr class="Row">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal20Resource1" ID="Literal20" runat="server" Text="Bank Name:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# Eval("BankName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal21Resource1" ID="Literal21" runat="server" Text="Routing #:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# GetRoutingNumber(Eval("RoutingNumber").ToString(), Eval("PartialRoutingNumber").ToString())%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal22Resource1" ID="Literal22" runat="server" Text="DDA #:"></as:Literal>
                                        </td>
                                        <td>
                                            <%#CheckPermisson(Eval("DDANumber"), WebSiteConstants.SEC_PERMISSION_DDA)%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </as:PlaceHolder>
                </div>
                <div class="col-md-6">
                    <!-- Risk Info -->
                    <uc:RiskInfo ID="uxRiskInfo" runat="server" />
                    <as:PlaceHolder ID="phdHierarchyInfo" runat="server">
                        <div class="row" id="hierarchyinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciHierarchyInformation">
                                <h2 class="grid-title">
                                    <as:Literal meta:resourcekey="Literal23Resource1" ID="Literal23" runat="server" Text="Hierarchy Information"></as:Literal></h2>
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
                                            <as:Literal meta:resourcekey="Literal25Resource1" ID="Literal25" runat="server" Text="Client Name:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# Eval("ClientName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal26Resource1" ID="Literal26" runat="server" Text="Client Login:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# Eval("ClientLogin")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal27Resource1" ID="Literal27" runat="server" Text="Sales Agent:"></as:Literal>
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxHSalesAgent" runat="server" Visible='<%# CheckHierarchy("SALESAGENT") %>'>
                                                <a href="#" id="uxLinkHSalesAgent" runat="server">
                                                    <%# Eval("SalesAgent")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHSalesAgent" runat="server" Visible='<%# !CheckHierarchy("SALESAGENT") %>'
                                                Text='<%# Eval("SalesAgent")%>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal28Resource1" ID="Literal28" runat="server" Text="Sys/Prin/Agent:"></as:Literal>
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxHAgent" runat="server" Visible='<%# CheckHierarchy("AGENT") %>'>
                                                <a href="#" id="uxLinkHAgent" runat="server">
                                                    <%# Eval("SysPrinAgent")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHAgent" runat="server" Visible='<%# !CheckHierarchy("AGENT") %>'
                                                Text='<%# Eval("SysPrinAgent")%>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal29Resource1" ID="Literal29" runat="server" Text="Headqrtr Merchant:"></as:Literal>
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxHHeadquaster" runat="server" Visible='<%# CheckHierarchy("HEADQUARTER") %>'>
                                                <a href="#" id="uxLinkHHeadquaster" runat="server">
                                                    <%# Eval("Headquarter")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHHeadquarter" runat="server" Visible='<%# !CheckHierarchy("HEADQUARTER") %>'
                                                Text='<%# Eval("Headquarter")%>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal30Resource1" ID="Literal30" runat="server" Text="Merchant Type:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# Eval("MerchantType")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal31Resource1" ID="Literal31" runat="server" Text="Chain Code:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# Eval("ChainCode")%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </as:PlaceHolder>
                    <as:PlaceHolder runat="server" ID="phdOtherCardInfo">
                        <div class="row" id="accountInfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciAccountInformation">
                                <h2 class="grid-title">
                                    <as:Literal meta:resourcekey="Literal32Resource1" ID="Literal32" runat="server" Text="Other Card Information"></as:Literal></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciAccountInformation">
                                <table class="ASTable">
                                    <tr class="Row">
                                        <td class="heading">
                                            <as:Literal meta:resourcekey="Literal34Resource1" ID="Literal34" runat="server" Text="AMEX MID:"></as:Literal></td>
                                        <td class="heading"><%# Eval("AMEXMID")%></td>

                                    </tr>
                                    <tr class="AltRow">
                                        <td>
                                            <as:Literal meta:resourcekey="Literal35Resource1" ID="Literal35" runat="server" Text="Discover MID:"></as:Literal>
                                        </td>
                                        <td>
                                            <%# Eval("DiscoverMID")%>
                                        </td>

                                    </tr>
                                </table>
                            </div>
                        </div>
                    </as:PlaceHolder>
                </div>


            </div>
        </ItemTemplate>
    </asp:Repeater>
    <as:RadCodeBlock ID="ScriptManagement" runat="server">
         <script type="text/javascript">
            var MIF_MerchantDetails_WRFC_uxtxtRelationShipManager = '#<%= uxtxtRelationShipManager.ClientID %>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MIF_MerchantDetails_NORTH.js"></script>
    </as:RadCodeBlock>
</asp:PlaceHolder>
