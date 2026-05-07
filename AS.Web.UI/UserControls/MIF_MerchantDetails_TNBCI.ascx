<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MerchantDetails_TNBCI.ascx.cs"
    Inherits="UserControls_MIF_MerchantDetails_TNBCI" %>
<%@ Register TagPrefix="uc" TagName="RiskInfo" Src="~/UserControls/MIF_RiskInformationSection.ascx" %>

<asp:PlaceHolder ID="phdMerchantDetail" runat="server">
    <div class="row" id="merchinfo">
        <div class="col-md-9" data-toggle="collapse" data-target="#ciMerchantInformation">
            <h2 class="grid-title mt-3x"><asp:Literal ID="Literal1" runat="server" Text="Merchant Information" meta:resourcekey="LiteralResource1"/>
            </h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 in" id="ciMerchantInformation">
            <table class="ASTable" id="tblMerchantInformation">
                <colgroup>
                    <col style="width: 120px" />
                    <col style="width: 350px" />
                    <col style="width: 150px" />
                </colgroup>
                <tr class="Row">
                    <td class="heading valign-middle"><asp:Literal ID="Literal2" runat="server" Text="Merchant ID:" meta:resourcekey="LiteralResource2"/>
                    </td>
                    <td>
                        <%= BindValue("MerchantNumber")%>
                    </td>
                    <td class="heading valign-middle"><asp:Literal ID="Literal3" runat="server" Text="Contact:" meta:resourcekey="LiteralResource3"/>
                    </td>
                    <td>
                        <%= BindValue("Contact")%>
                    </td>
                    <td class="heading valign-middle"><asp:Literal ID="Literal4" runat="server" Text="Status:" meta:resourcekey="LiteralResource4"/>
                    </td>
                    <td>
                        <%= BindValue("Status")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-middle"><asp:Literal ID="Literal5" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource5"/>
                    </td>
                    <td>
                        <%= BindValue("MerchantName")%>
                    </td>
                    <td class="heading valign-middle"><asp:Literal ID="Literal6" runat="server" Text="Phone:" meta:resourcekey="LiteralResource6"/>
                    </td>
                    <td>
                        <%= FormatPhone(BindValue("Phone"))%>
                    </td>
                    <td class="heading valign-middle"><asp:Literal ID="Literal7" runat="server" Text="Last Batch Activity:" meta:resourcekey="LiteralResource7"/>
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkLastBatch" Text=""
                            OnClick="lnkLastBatch_Click" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-middle"><asp:Literal ID="Literal8" runat="server" Text="Address:" meta:resourcekey="LiteralResource8"/>
                    </td>
                    <td>
                        <%= BindAddress(BindValue("Address1"), BindValue("Address2"), BindValue("Address3"), BindValue("City"), BindValue("State"), BindValue("Zip"))%>
                    </td>
                    <td class="heading valign-middle"><asp:Literal ID="Literal9" runat="server" Text="Email:" meta:resourcekey="LiteralResource9"/>
                    </td>
                    <td colspan="3">
                        <%= BindValue("Email")%>
                    </td>
                </tr>
                <tr class="AltRow" runat="server" id="pnlRelationshipmanager">
                    <td class="heading valign-middle">
                        <as:Panel runat="server" ID="uxpnlRMLabel">
                            <asp:Literal ID="Literal10" runat="server" Text="Relationship Manager:" meta:resourcekey="LiteralResource35"/>
                        </as:Panel>
                    </td>
                    <td colspan="5">
                        <as:Panel runat="server" ID="uxpnlRMCtrls" Style="margin-top: 2px;">
                            <as:Literal runat="server" ID="uxlbRelationshipManager"></as:Literal>
                            <as:LinkButton ID="uxbtnAdd" OnClick="uxbtnAdd_click" Text="Add RM" runat="server" meta:resourcekey="uxbtnAddResource1"/>
                            <as:LinkButton ID="uxbtnEdit" OnClick="uxbtnEdit_click" runat="server" Text="Edit" meta:resourcekey="uxbtnEditResource1"/>
                            <as:Panel ID="uxpnlSaveCancel" runat="server" Visible="false">
                                <as:TextBox runat="server" ID="uxtxtRelationShipManager" MaxLength="30" />
                                <as:LinkButton ID="uxbtnSave" OnClick="uxbtnSave_click" OnClientClick="return CheckPrefix();"
                                    runat="server" Text="Save" meta:resourcekey="uxbtnSaveResource1"/>
                                <as:LinkButton ID="uxbtnCancel" OnClick="uxbtnCancel_click" runat="server" Text="Cancel" meta:resourcekey="uxbtnCancelResource1"/>
                            </as:Panel>
                        </as:Panel>
                    </td>
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
    <asp:Repeater ID="rptMerchantInfo" runat="server" EnableViewState="true" OnItemDataBound="uxMerchantInfoItemDataBound">
        <ItemTemplate>
            <div class="row">
                <div class="col-md-6">
                    <div class="row" id="businessinfo">
                        <div class="col-xs-12" data-toggle="collapse" data-target="#ciBusinessInformation">
                            <h2 class="grid-title"><asp:Literal ID="Literal11" runat="server" Text="Business Information" meta:resourcekey="LiteralResource10"/></h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 in" id="ciBusinessInformation">
                            <table class="ASTable">
                                <tr class="Row">
                                    <td class="heading"><asp:Literal ID="Literal14" runat="server" Text="Approval Date:" meta:resourcekey="LiteralResource13"/>
                                    </td>
                                    <td>
                                        <%#
                                        FormatDate(Eval("ApprovalDate"))%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading"><asp:Literal ID="Literal15" runat="server" Text="Closed Date:" meta:resourcekey="LiteralResource14"/>
                                    </td>
                                    <td>
                                        <%# FormatDate(Eval("ClosedDate"))%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading"><asp:Literal ID="Literal16" runat="server" Text="Status:" meta:resourcekey="LiteralResource15"/>
                                    </td>
                                    <td>
                                        <%# Eval("Status")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading"><asp:Literal ID="Literal17" runat="server" Text="Tax ID:" meta:resourcekey="LiteralResource16"/>
                                    </td>
                                    <td>
                                        <%#CheckPermisson(Eval("TaxID"), WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading"><asp:Literal ID="Literal18" runat="server" Text="SIC/MCC:" meta:resourcekey="LiteralResource17"/>
                                    </td>
                                    <td>
                                        <%#
                                        FormatSIC(Eval("SICCode"), Eval("SICCodeDesc"))%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading"><asp:Literal ID="Literal19" runat="server" Text="Average Sales Amount:" meta:resourcekey="LiteralResource18"/>
                                    </td>
                                    <td>
                                        <%#
                                        FormatCurrency(Eval("Avg_Ticket_Amt"))%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <!-- Risk Info -->
                    <uc:RiskInfo ID="uxRiskInfo" runat="server" />
                    <asp:PlaceHolder ID="phdHierarchy" runat="server">
                        <div class="row" id="hierarchyinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciHierarchyInformation">
                                <h2 class="grid-title"><asp:Literal ID="Literal20" runat="server" Text="Hierarchy Information" meta:resourcekey="LiteralResource19"/></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciHierarchyInformation">
                                <table class="ASTable">
                                    <colgroup>
                                        <col style="width: 90px" />
                                        <col />
                                        <col style="width: 90px" />
                                        <col />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading valign-middle"><asp:Literal ID="Literal23" runat="server" Text="Client Name:" meta:resourcekey="LiteralResource22"/>
                                        </td>
                                        <td colspan="3">
                                            <%# Eval("ClientName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal24" runat="server" Text="Client Login:" meta:resourcekey="LiteralResource23"/>
                                        </td>
                                        <td>
                                            <%# Eval("ClientLogin")%>
                                        </td>
                                        <td class="heading"><asp:Literal ID="Literal25" runat="server" Text="Corporate:" meta:resourcekey="LiteralResource24"/>
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxHCorporate" runat="server" Visible='<%# CheckHierarchy("TNCORP") %>'>
                                                <a href="#" onclick="return rf_SubmitReportFilterValues('4914', 'TNCORP', '<%#Eval("Corporate") %>');">
                                                    <%#Eval("Corporate") %></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uLHCorporate" runat="server" Visible='<%# !CheckHierarchy("TNCORP") %>'
                                                Text='<%#Eval("Corporate") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading valign-middle"><asp:Literal ID="Literal26" runat="server" Text="Region:" meta:resourcekey="LiteralResource25"/>
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxHRegion" runat="server" Visible='<%# CheckHierarchy("TNREG") %>'>
                                                <a href="#" onclick="return rf_SubmitReportFilterValues('4915', 'TNREG', '<%#Eval("Region") %>');">
                                                    <%#Eval("Region")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxHLRegion" runat="server" Visible='<%# !CheckHierarchy("TNREG") %>'
                                                Text='<%#Eval("Region") %>'></as:Literal>
                                        </td>
                                        <td class="heading"><asp:Literal ID="Literal27" runat="server" Text="Principal:" meta:resourcekey="LiteralResource26"/>
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxHPrincipal" runat="server" Visible='<%# CheckHierarchy("TNPRIN") %>'>
                                                <a href="#" onclick="return rf_SubmitReportFilterValues('4916', 'TNPRIN', '<%#Eval("Principal") %>');">
                                                    <%#Eval("Principal")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHPrincipal" runat="server" Visible='<%# !CheckHierarchy("TNPRIN") %>'
                                                Text='<%#Eval("Principal") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal28" runat="server" Text="Association:" meta:resourcekey="LiteralResource27"/>
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxHAssociation" runat="server" Visible='<%# CheckHierarchy("TNASSO") %>'>
                                                <a href="#" onclick="return rf_SubmitReportFilterValues('4917', 'TNASSO', '<%#Eval("Association") %>');">
                                                    <%#Eval("Association")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHAssociation" runat="server" Visible='<%# !CheckHierarchy("TNASSO") %>'
                                                Text='<%#Eval("Association") %>'></as:Literal>
                                        </td>
                                        <td class="heading">
                                            <as:Literal ID="ltChain" runat="server" Text="Chain:" meta:resourcekey="ltChainResource"></as:Literal>
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxPlaceChain" runat="server">
                                                <a href="#" onclick="return rf_SubmitReportFilterValues('4918', 'CHAIN', '<%# Eval("Chain")%>');">
                                                    <%# Eval("Chain")%></a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxPlaceChainNew" runat="server" Visible='<%# CheckPermissionAddEditChain(Eval("SiteAccess")) %>'>
                                                <a href="#" onclick="return ShowPopupModal('CreateNewChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")) %>','auto');">
                                                    <asp:Literal ID="Literal29" runat="server" Text="Create New Chain" meta:resourcekey="LiteralResource28"/></a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxPlaceExistChain" runat="server" Visible='<%# CheckPermissionAddEditChain(Eval("SiteAccess")) %>'> 
                                                <asp:Literal runat="server" Text="or" meta:resourcekey="LiteralResourceOR"></asp:Literal> 
                                                <a href="#" onclick="return ShowPopupModal('ModifyMerchantChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")+"&Chain="+Eval("Chain")) %>','auto');">
                                                    <asp:Literal ID="Literal30" runat="server" Text="Add to Existing Chain" meta:resourcekey="LiteralResource29"/></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHChain" runat="server" Text='<%# Eval("Chain")%>'></as:Literal>
                                        </td>  
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div class="row" id="bankinfo">
                        <div class="col-xs-12" data-toggle="collapse" data-target="#ciBankInformation">
                            <h2 class="grid-title"><asp:Literal ID="Literal31" runat="server" Text="Bank Information" meta:resourcekey="LiteralResource30"/></h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 in" id="ciBankInformation">
                            <table class="ASTable">
                                <colgroup>
                                    <col style="width: 100px" />
                                </colgroup>
                                <tr class="Row">
                                    <td class="heading"><asp:Literal ID="Literal34" runat="server" Text="Routing #:" meta:resourcekey="LiteralResource33"/>
                                    </td>
                                    <td>
                                        <%# GetRoutingNumber(Eval("RoutingNumber").ToString(), Eval("PartialRoutingNumber").ToString())%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading"><asp:Literal ID="Literal35" runat="server" Text="DDA #:" meta:resourcekey="LiteralResource34"/>
                                    </td>
                                    <td>
                                        <%#CheckPermisson(Eval("DDANumber"), WebSiteConstants.SEC_PERMISSION_DDA)%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <as:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var MIF_MerchantDetails_TNBCI_uxtxtRelationShipManager = '#<%= uxtxtRelationShipManager.ClientID %>';
            var Text_AtLeast30NoSpecialChars = '<%=GetLocalResourceObject("MIF_MerchantDetails_TNBCIJS_Text_AtLeast30NoSpecialChars").ToString()%>';
            var Text_UnallowPrefixSuffix = '<%=GetLocalResourceObject("MIF_MerchantDetail_TNBCIJS_Text_UnallowPrefixSuffix").ToString()%>';

        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MIF_MerchantDetails_TNBCI.js"></script>
    </as:RadCodeBlock>
</asp:PlaceHolder>