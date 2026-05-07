<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MerchantDetails_FultonDemo.ascx.cs"
    Inherits="UserControls_MIF_MerchantDetails_FultonDemo" %>
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
                    <td class="heading valign-middle">Email:
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
                                                <asp:Literal ID="ltrSiteAccess" runat="server" Text='<%# HasMSProductEnvironment()?Eval("SiteAccess"):"Not Available" %>' />
                                            </div>
                                            <div class="pull-right opt-in-out-button">
                                                <as:PlaceHolder runat="server" ID="PlaceHolder1">
                                                    <as:Button ID="uxSiteAccess" runat="server" Text='<%# SetStatusText(Eval("SiteAccess")) %>'
                                                        Visible='<%# SetVisible(Eval("SiteAccess")) %>' OnClientClick='<%# SetURLForSiteAccessButton() %>' IsStandardButton="False" meta:resourcekey="uxSiteAccessResource1" />
                                                </as:PlaceHolder>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal17" runat="server" Text="Tax ID" meta:resourcekey="LiteralResource15" /></td>
                                        <td>
                                            <%#CheckPermisson(Eval("TaxID"), WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal18" runat="server" Text="SIC/MCC:" meta:resourcekey="LiteralResource16" /></td>
                                        <td>
                                            <%#FormatSIC(Eval("SICCode"), Eval("SICCodeDesc"))%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
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
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal23" runat="server" Text="Bank Name:" meta:resourcekey="LiteralResource21" /></td>
                                        <td>
                                            <%# Eval("BankName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal24" runat="server" Text="Routing #:" meta:resourcekey="LiteralResource22" /></td>
                                        <td>
                                            <%# GetRoutingNumber(Eval("RoutingNumber").ToString(), Eval("PartialRoutingNumber").ToString())%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal25" runat="server" Text="DDA #:" meta:resourcekey="LiteralResource23" /></td>
                                        <td>
                                            <%#CheckPermisson(Eval("DDANumber"), WebSiteConstants.SEC_PERMISSION_DDA)%>
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
                                            <asp:Literal ID="Literal32" runat="server" Text="Corporate:" meta:resourcekey="LiteralResource30" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxHCorp" runat="server" Visible='<%# CheckHierarchy("CORP") && !Eval("Corporate").IsNullOrEmpty() %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;5801&#039;, &#039;CORP&#039;, &#039;<%# Eval("Corporate") %>&#039;);'>
                                                    <%# Eval("Corporate")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHCorp" runat="server" Visible='<%# !CheckHierarchy("CORP") %>'
                                                Text='<%# Eval("Corporate") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal33" runat="server" Text="Bank:" meta:resourcekey="LiteralResource31" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxHBank" runat="server" Visible='<%# CheckHierarchy("FBBANK") && !Eval("Bank").IsNullOrEmpty() %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;5802&#039;, &#039;FBBANK&#039;, &#039;<%# Eval("Bank") %>&#039;);'>
                                                    <%# Eval("Bank")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHBank" runat="server" Visible='<%# !CheckHierarchy("FBBANK") %>'
                                                Text='<%# Eval("Bank") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal34" runat="server" Text="Region:" meta:resourcekey="LiteralResource32" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxHRegion" runat="server" Visible='<%# CheckHierarchy("REG") && !Eval("Region").IsNullOrEmpty() %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;5803&#039;, &#039;REG&#039;, &#039;<%# Eval("Region") %>&#039;);'>
                                                    <%# Eval("Region")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHRegion" runat="server" Visible='<%# !CheckHierarchy("REG") %>'
                                                Text='<%# Eval("Region") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal35" runat="server" Text="Association Sub-Group:" meta:resourcekey="LiteralResource33" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxHAssoSubGrp" runat="server" Visible='<%# CheckHierarchy("ASSOSUBGROUP") && !Eval("ASSOSUBGROUP").IsNullOrEmpty() %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;5804&#039;, &#039;ASSOSUBGROUP&#039;, &#039;<%# Eval("ASSOSUBGROUP") %>&#039;);'>
                                                    <%# Eval("ASSOSUBGROUP")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHAssoSubGrp" runat="server" Visible='<%# !CheckHierarchy("ASSOSUBGROUP") %>'
                                                Text='<%# Eval("ASSOSUBGROUP") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal36" runat="server" Text="Association:" meta:resourcekey="LiteralResource34" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxHAsso" runat="server" Visible='<%# CheckHierarchy("FBASSO") && !Eval("Association").IsNullOrEmpty() %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;5805&#039;, &#039;FBASSO&#039;, &#039;<%# Eval("Association") %>&#039;);'>
                                                    <%# Eval("Association")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHAsso" runat="server" Visible='<%# !CheckHierarchy("FBASSO") %>'
                                                Text='<%# Eval("Association") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="Row">
                                        <td colspan="2" class="hierarchy">
                                            <b><%= GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_SalesHierarchy") %></b> </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal37" runat="server" Text="Sales Agent Regional Manager:" meta:resourcekey="LiteralResource35" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxHSarm" runat="server" Visible='<%# CheckHierarchy("SARM") %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;5806&#039;, &#039;SARM&#039;, &#039;<%# Eval("SARM") %>&#039;);'>
                                                    <%# Eval("SARM")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHSarm" runat="server" Visible='<%# !CheckHierarchy("SARM") %>'
                                                Text='<%# Eval("SARM") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal38" runat="server" Text="Sales Agent Manager:" meta:resourcekey="LiteralResource36" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxHSam" runat="server" Visible='<%# CheckHierarchy("SAM") %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;5807&#039;, &#039;SAM&#039;, &#039;<%# Eval("SalesAgentManager") %>&#039;);'>
                                                    <%# Eval("SalesAgentManager")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHSam" runat="server" Visible='<%# !CheckHierarchy("SAM") %>' Text='<%# Eval("SalesAgentManager") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal39" runat="server" Text="Sales Agent:" meta:resourcekey="LiteralResource37" /></td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# CheckHierarchy("FBSALESAGENT") %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;5808&#039;, &#039;FBSALESAGENT&#039;, &#039;<%# Eval("SalesAgent") %>&#039;);'>
                                                    <%# Eval("SalesAgent")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal1" runat="server" Visible='<%# !CheckHierarchy("FBSALESAGENT") %>'
                                                Text='<%# Eval("SalesAgent") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal40" runat="server" Text="Chain:" meta:resourcekey="LiteralResource38" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxPlaceChain" runat="server">
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;5809&#039;, &#039;CHAIN&#039;, &#039;<%# Eval("Chain") %>&#039;);'>
                                                    <%# Eval("Chain")%></a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxPlaceChainNew" runat="server" Visible='<%# CheckPermissionAddEditChain(Eval("SiteAccess")) %>'>
                                                <a href="#" onclick="return ShowPopupModal('CreateNewChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")) %>','auto');">Create New Chain</a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxPlaceExistChain" runat="server" Visible='<%# CheckPermissionAddEditChain(Eval("SiteAccess")) %>'>
                                                <asp:Literal ID="Literal11" runat="server" Text="or" meta:resourcekey="LiteralResourceOR"></asp:Literal> <a href="#" onclick="return ShowPopupModal('ModifyMerchantChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")+"&Chain="+Eval("Chain")) %>','auto');">Add to Existing Chain</a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHChain" runat="server" Text='<%# Eval("Chain") %>'></as:Literal></td>
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
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MIF_MerchantDetails_FIS.js"></script>
    </as:RadCodeBlock>
</asp:PlaceHolder>
