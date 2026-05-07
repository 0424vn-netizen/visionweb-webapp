<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MerchantDetails_PAYA.ascx.cs" Inherits="UserControls_MIF_MerchantDetails_PAYA" %>
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
                <%-- <tr class="AltRow">
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal19" runat="server" Text="User ID:" meta:resourcekey="LiteralResourceUserID" />
                    </td>
                    <td>
                        <%= BindValueEmpDash("URL")%>
                    </td>
                    <td colspan="5"></td>
                </tr>--%>
                <as:Panel runat="server" ID="uxPnlRelationshipManager">
                    <tr class="Row">
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
                                            <asp:Literal ID="Literal22" runat="server" Text="Open Date:" meta:resourcekey="LiteralResourceOpenDate" /></td>
                                        <td>
                                            <%# FormatDate(Eval("OpenDate"))%>
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
                                                <asp:Literal ID="ltrSiteAccess" runat="server" Text='<%# HasMSProductEnvironment()?Eval("SiteAccess"): GetLocalResourceObject("MIF_MerchantDetails_FULTONCS_Text_NotAvailable").ToString() %>' />
                                            </div>
                                            <div class="pull-right opt-in-out-button" visible='<%# !IsCaseManagement %>'>
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
                                            <asp:Literal ID="Literal13" runat="server" Text="Average Sales Amount:" meta:resourcekey="LiteralAverageSaleResource" /></td>
                                        <td>
                                            <%#FormatCurrency(Eval("Avg_Ticket_Amt"))%>
                                        </td>
                                    </tr>

                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal42" runat="server" Text="Business Type:" meta:resourcekey="LiteralBusinessTypeResource" /></td>
                                        <td>
                                            <%#Eval("BusinessType")%>
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
                                <table class="ASTable" runat="server" id="uxBankCardInfo">
                                    <colgroup>
                                        <col class="w-45" />
                                        <col class="w-55" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal24" runat="server" Text="Office:" meta:resourcekey="Literal_OfiiceResource" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxBankOffice" runat="server" Visible='<%# CheckDisplayHierachyMS("OFFICE") %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;7601&#039;, &#039;OFFICE&#039;, &#039;<%# Eval("EntityNumber1") %>&#039;);'><%# Eval("EntityNumber1")%></a>
                                            </as:PlaceHolder>

                                            <as:Literal ID="Literal1" runat="server" Visible='<%# !CheckDisplayHierachyMS("OFFICE") %>'
                                                Text='<%# Eval("EntityNumber1") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal25" runat="server" Text="Association:" meta:resourcekey="LiteralResource28" /></td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# CheckDisplayHierachyMS("ASSO") %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;7602&#039;, &#039;ASSO&#039;, &#039;<%# Eval("EntityNumber2") %>&#039;);'><%# Eval("EntityNumber2")%></a>
                                            </as:PlaceHolder>

                                            <as:Literal ID="Literal21" runat="server" Visible='<%# !CheckDisplayHierachyMS("ASSO") %>'
                                                Text='<%# Eval("EntityNumber2") %>'></as:Literal>
                                        </td>
                                    </tr>

                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal41" runat="server" Text="Contractor:" meta:resourcekey="Literal_ContractorResource" />
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# CheckDisplayHierachyMS("CONTRACTOR") %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;7603&#039;, &#039;CONTRACTOR&#039;, &#039;<%# Eval("EntityNumber3") %>&#039;);'><%# Eval("EntityNumber3")%></a>
                                            </as:PlaceHolder>

                                            <as:Literal ID="Literal35" runat="server" Visible='<%# !CheckDisplayHierachyMS("CONTRACTOR") %>'
                                                Text='<%# Eval("EntityNumber3") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal19" runat="server" Text="Chain:" meta:resourcekey="LiteralResource38" />
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxChain" runat="server" Visible='<%# CheckHierarchy("CHAIN") %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;7611&#039;, &#039;CHAIN&#039;, &#039;<%# Eval("Chain") %>&#039;);'><%# Eval("Chain")%></a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxPlaceChainNew" runat="server" Visible='<%# !CheckPermissionAddEditChain(Eval("SiteAccess")) %>'>
                                                <a href="#" onclick="return ShowPopupModal('CreateNewChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")) %>','auto');">
                                                    <asp:Literal ID="Literal27" runat="server" Text="Create New Chain" meta:resourcekey="LiteralResource56" />
                                                </a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxPlaceExistChain" runat="server" Visible='<%# CheckPermissionAddEditChain(Eval("SiteAccess")) %>'>
                                                <asp:Literal ID="Literal11" runat="server" Text=" or " meta:resourcekey="LiteralResourceOR"></asp:Literal>
                                                <a href="#" onclick="return ShowPopupModal('ModifyMerchantChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")+"&Chain="+Eval("Chain")) %>','auto');">
                                                    <asp:Literal ID="Literal28" runat="server" Text="Add to Existing Chain" meta:resourcekey="LiteralResource57" />
                                                </a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHChain" runat="server" Text='<%# Eval("Chain") %>'></as:Literal></td>
                                    </tr>
                                     <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal40" runat="server" Text="Secondary Chain:" meta:resourcekey="Literal_SecondaryChainResource" />
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# CheckDisplayHierachyMS("CORPID") %>'>
                                            <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;7604&#039;, &#039;CORPID&#039;, &#039;<%# Eval("EntityNumber4") %>&#039;);'><%# Eval("EntityNumber4")%></a>
                                             </as:PlaceHolder>
                                            <as:Literal ID="Literal43" runat="server" Visible='<%# !CheckDisplayHierachyMS("CORPID") %>'
                                                Text='<%# Eval("EntityNumber4") %>'></as:Literal>
                                           </td>
                                    </tr>
                                </table>
                                <table class="ASTable" runat="server" id="uxAchInfor">
                                    <colgroup>
                                        <col class="w-45" />
                                        <col class="w-55" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal12" runat="server" Text="ISO:" meta:resourcekey="LiteralResource27" /></td>
                                        <td>
                                             <as:PlaceHolder ID="PlaceHolder5" runat="server" Visible='<%# CheckDisplayHierachyMS("ISOID") %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;7605&#039;, &#039;ISOID&#039;, &#039;<%# Eval("EntityNumber5") %>&#039;);'><%# Eval("EntityNumber5")%></a>
                                             </as:PlaceHolder>
                                           <as:Literal ID="Literal44" runat="server" Visible='<%# !CheckDisplayHierachyMS("ISOID") %>'
                                                Text='<%# Eval("EntityNumber5") %>'></as:Literal>
                                           
                                        </td>
                                    </tr>

                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal23" runat="server" Text="Agent:" meta:resourcekey="LiteralResource31" />
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder6" runat="server" Visible='<%# CheckDisplayHierachyMS("AGENT") %>'>
                                            <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;7607&#039;, &#039;AGENT&#039;, &#039;<%# Eval("EntityNumber7") %>&#039;);'><%# Eval("EntityNumber7")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal45" runat="server" Visible='<%# !CheckDisplayHierachyMS("AGENT") %>'
                                                Text='<%# Eval("EntityNumber7") %>'></as:Literal>
                                         </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal29" runat="server" Text="Merchant:" meta:resourcekey="LiteralResource34" /></td>
                                        <td>
                                             <as:PlaceHolder ID="PlaceHolder7" runat="server" Visible='<%# CheckDisplayHierachyMS("MERCHANT") %>'>
                                            <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;7608&#039;, &#039;MERCHANT&#039;, &#039;<%# Eval("EntityNumber8") %>&#039;);'><%# Eval("EntityNumber8")%></a>
                                             </as:PlaceHolder>
                                            <as:Literal ID="Literal46" runat="server" Visible='<%# !CheckDisplayHierachyMS("MERCHANT") %>'
                                              Text='<%# Eval("EntityNumber8") %>'></as:Literal>
                                        
                                         </td>
                                    </tr>

                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal39" runat="server" Text="Parent Regional:" meta:resourcekey="LiteralResource_ParentRegional" /></td>
                                        <td>
                                             <as:PlaceHolder ID="PlaceHolder8" runat="server" Visible='<%# CheckDisplayHierachyMS("PREG") %>'>
                                            <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;7609&#039;, &#039;PREG&#039;, &#039;<%# Eval("EntityNumber9") %>&#039;);'><%# Eval("EntityNumber9")%></a>
                                            </as:PlaceHolder>

                                             <as:Literal ID="Literal47" runat="server" Visible='<%# !CheckDisplayHierachyMS("PREG") %>'
                                              Text='<%# Eval("EntityNumber9") %>'></as:Literal>

                                         </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal30" runat="server" Text="Chain:" meta:resourcekey="LiteralResource38" />
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="uxACHChain" runat="server" Visible='<%# CheckHierarchy("CHAIN") %>'>
                                                <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;7611&#039;, &#039;CHAIN&#039;, &#039;<%# Eval("Chain") %>&#039;);'><%# Eval("Chain")%></a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxACHPlaceChainNew" runat="server" Visible='<%# !CheckPermissionAddEditChain(Eval("SiteAccess")) %>'>
                                                <a href="#" onclick="return ShowPopupModal('CreateNewChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")) %>','auto');">
                                                    <asp:Literal ID="Literal31" runat="server" Text="Create New Chain" meta:resourcekey="LiteralResource56" />
                                                </a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxACHPlaceExistChain" runat="server" Visible='<%# CheckPermissionAddEditChain(Eval("SiteAccess")) %>'>
                                                <asp:Literal ID="Literal32" runat="server" Text=" or " meta:resourcekey="LiteralResourceOR"></asp:Literal>
                                                <a href="#" onclick="return ShowPopupModal('ModifyMerchantChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")+"&Chain="+Eval("Chain")) %>','auto');">
                                                    <asp:Literal ID="Literal33" runat="server" Text="Add to Existing Chain" meta:resourcekey="LiteralResource57" />
                                                </a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxACHLHChain" runat="server" Text='<%# Eval("Chain") %>'></as:Literal></td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal48" runat="server" Text="Secondary Chain:" meta:resourcekey="Literal_SecondaryChainResource" />
                                        </td>
                                        <td>
                                            <as:PlaceHolder ID="PlaceHolder9" runat="server" Visible='<%# CheckDisplayHierachyMS("CORPID") %>'>
                                            <a href="#" onclick='return rf_SubmitReportFilterValues(&#039;7604&#039;, &#039;CORPID&#039;, &#039;<%# Eval("EntityNumber4") %>&#039;);'><%# Eval("EntityNumber4")%></a>
                                             </as:PlaceHolder>
                                            <as:Literal ID="Literal49" runat="server" Visible='<%# !CheckDisplayHierachyMS("CORPID") %>'
                                                Text='<%# Eval("EntityNumber4") %>'></as:Literal>
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
                                            <asp:Literal ID="Literal56" runat="server" Text="Routing #:" meta:resourcekey="LiteralResource22" /></td>
                                        <td>
                                            <%# GetRoutingNumber(Eval("RoutingNumber").ToString(), Eval("PartialRoutingNumber").ToString()) %>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal57" runat="server" Text="DDA #:" meta:resourcekey="LiteralResource23" /></td>
                                        <td>
                                            <%#GetDDANumber(Eval("DDANumber").ToString(), Eval("PartialDDANumber").ToString(),WebSiteConstants.SEC_PERMISSION_DDA)%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div class="row" id="othercardinfo">
                        <div class="col-xs-9" data-toggle="collapse" data-target="#ciOtherCardInformation">
                            <h2 class="grid-title">
                                <asp:Literal ID="Literal34" runat="server" Text="Other Card Information" meta:resourcekey="lb_MerchantDetails_PAYACS_Text_OtherCardInfo" /></h2>
                        </div>
                        <div class="col-xs-3">
                            <div class="report-export dropdown pull-right" runat="server" id="div3" visible='<%# !IsCaseManagement %>'>
                                <ul class="dropdown-menu">
                                    <li id="Li3" runat="server">
                                        <asp:LinkButton ID="LinkButton3" runat="server" meta:resourcekey="LinkButton3Resource1">
                                            <asp:Literal ID="Literal36" runat="server" Text="Excel" meta:resourcekey="LiteralResource36" />
                                        </asp:LinkButton></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 in" id="ciOtherCardInformation">
                            <table class="ASTable">
                                <colgroup>
                                    <col style="width: 115px" />
                                </colgroup>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal37" runat="server" Text="AMEX MID:" meta:resourcekey="LiteralAmexResource" /></td>
                                    <td>
                                        <%# Eval("AMEX")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal38" runat="server" Text="Discover MID:" meta:resourcekey="LiteralDiscoverResource" /></td>
                                    <td>
                                        <%# Eval("Discover")%>
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
            var MIF_MerchantDetails_FIS_uxtxtRelationShipManager = '#<%= uxtxtRelationShipManager.ClientID %>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MIF_MerchantDetails_PAYA.js"></script>
    </as:RadCodeBlock>
</asp:PlaceHolder>
