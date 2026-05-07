<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MechantDetails_SOFTEK.ascx.cs" Inherits="UserControls_MIF_MechantDetails_SOFTEK" %>
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
                    <col style="width: 150px" />
                    <col style="width: 350px" />
                    <col style="width: 150px" />
                </colgroup>
                <tr class="Row">
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal2" runat="server" Text="Merchant ID:" meta:resourcekey="LiteralResource2" />
                    </td>
                    <td>
                        <%= BindValue("MerchantNumber")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal3" runat="server" Text="Contact:" meta:resourcekey="LiteralResource3"   />
                    </td>
                    <td>
                        <%= BindValue("Contact")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal4" runat="server" Text="Status:" meta:resourcekey="LiteralResource4" />
                    </td>
                    <td>
                        <%= BindValue("ActivityStatus")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal5" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource5" />
                    </td>
                    <td>
                        <%= BindValue("MerchantName")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal6" runat="server" Text="Phone:" meta:resourcekey="LiteralResource6" />
                    </td>
                    <td>
                        <%= FormatPhone(BindValue("Phone"))%>
                    </td>
                    <td class="heading  valign-top text-nowrap">
                        <asp:Literal ID="Literal7" runat="server" Text="Last Batch Activity:" meta:resourcekey="LiteralResource7" />
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkLastBatch" Text="" OnClick="lnkLastBatch_Click" meta:resourcekey="lnkLastBatchResource1" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal8" runat="server" Text="Address:" meta:resourcekey="LiteralResource8" />
                    </td>
                    <td>
                        <%= BindAddress(BindValue("Address1"), BindValue("Address2"), BindValue("Address3"), BindValue("City"), BindValue("State"), BindValue("Zip"))%>
                        
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal9" runat="server" Text="Email:" meta:resourcekey="LiteralResource9" />
                    </td>
                    <td colspan="3" class="valign-top">
                        <%= BindValue("Email")%>
                    </td>
                </tr>
                <% if (uxpnlRMLabel.Visible)
                   { %>
                <tr class="AltRow">
                    <td class="heading valign-middle">
                        <as:Panel runat="server" ID="uxpnlRMLabel" meta:resourcekey="uxpnlRMLabelResource1">
                            <as:ValidatorLabel ID="ValidatorLabel1" runat="server" Text="Relationship Manager:" ApplyFor="uxtxtRelationShipManager" meta:resourcekey="MIF_MerchantDetail_ORIASCX_ValidatorLabel_RelationshipManager"></as:ValidatorLabel>
                        </as:Panel>
                    </td>
                    <td colspan="5" class="valign-middle">
                        <as:Panel runat="server" ID="uxpnlRMCtrls" meta:resourcekey="uxpnlRMCtrlsResource1">
                            <as:Panel runat="server" ID="uxpnlAddEdit" CssClass="opt-in-out-text w-100" meta:resourcekey="uxpnlAddEditResource1">
                                <as:Literal runat="server" ID="uxlbRelationshipManager" meta:resourcekey="uxlbRelationshipManagerResource1"></as:Literal>
                                <div class="pull-right">
                                    <as:LinkButton ID="uxbtnAdd" OnClick="uxbtnAdd_click" Text="Add RM" runat="server" CssClass="btn btn-default" meta:resourcekey="uxbtnAddResource1" />
                                    <as:LinkButton ID="uxbtnEdit" OnClick="uxbtnEdit_click" runat="server" Text="Edit" CssClass="btn btn-default" meta:resourcekey="uxbtnEditResource1" />
                                </div>
                            </as:Panel>
                            <as:Panel ID="uxpnlSaveCancel" runat="server" Visible="false" meta:resourcekey="uxpnlSaveCancelResource1">
                                <div class="opt-in-out-text w-70">
                                    <as:TextBox runat="server" ID="uxtxtRelationShipManager" MaxLength="30" CssClass="form-control" meta:resourcekey="uxtxtRelationShipManagerResource1" />
                                </div>
                                <div class="pull-right opt-in-out-button">
                                    <as:LinkButton ID="uxbtnSave" OnClick="uxbtnSave_click" OnClientClick="return dovalidation();" runat="server" Text="Save" CssClass="btn btn-default" meta:resourcekey="uxbtnSaveResource1" />
                                    <as:LinkButton ID="uxbtnCancel" OnClick="uxbtnCancel_click" runat="server" Text="Cancel" CssClass="btn btn-default" meta:resourcekey="uxbtnCancelResource1" />
                                </div>
                            </as:Panel>
                            <div class="bottom-error">
                                <as:ValidatorMessage ID="ValidatorMessage1" ApplyFor="uxtxtRelationShipManager" runat="server"></as:ValidatorMessage>
                            </div>
                        </as:Panel>
                        <as:Validator ID="Validator1" runat="server" ValidationFunction="dovalidation" MessageType="Inline" meta:resourcekey="Validator1Resource1">
                            <Items>
                                <as:CustomValidationItem ClientValidationFunction="CheckPrefix" ControlToValidateID="uxtxtRelationShipManager" Message="Prefix or suffix (for example Mr./Ms/Sir/Jr.) are not allowed." meta:resourcekey="MIF_MerchantDetail_ORIJS_Text_UnallowPrefixSuffix" />
                                <as:CustomValidationItem ClientValidationFunction="CheckPrefixSpecial" ControlToValidateID="uxtxtRelationShipManager" Message="Relationship Manager field must be 30 characters or less and may not include special characters" meta:resourcekey="MIF_MerchantDetails_ORIJS_Text_AtLeast30NoSpecialChars" />
                            </Items>
                        </as:Validator>

                    </td>
                </tr>
                <%} %>
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
                            <h2 class="grid-title">
                                <asp:Literal ID="Literal10" runat="server" Text="Business Information" meta:resourcekey="LiteralResourceBusinessInfo" />
                            </h2>
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
                                        <asp:Literal ID="Literal12" runat="server" Text="Corporate Name:" meta:resourcekey="LiteralResource12" /></td>
                                    <td>
                                        <%# Eval("CorporateName") %>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal13" runat="server" Text="Billing Address:" meta:resourcekey="LiteralResource13" /></td>
                                    <td>
                                        <%# Eval("CorporateAddress") %>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal14" runat="server" Text="Phone:" meta:resourcekey="LiteralResource14" /></td>
                                    <td>
                                        <%# FormatPhone(Eval("Phone"))%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal15" runat="server" Text="Fax:" meta:resourcekey="LiteralResource15" /></td>
                                    <td>
                                        <%# FormatPhone(Eval("Fax"))%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal16" runat="server" Text="Approval Date:" meta:resourcekey="LiteralResource16" /></td>
                                    <td>
                                        <%# FormatDate(Eval("ApprovalDate"))%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal17" runat="server" Text="Closed Date:" meta:resourcekey="LiteralResource17" /></td>
                                    <td>
                                        <%# FormatDate(Eval("ClosedDate"))%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal18" runat="server" Text="Status:" meta:resourcekey="LiteralResource18" /></td>
                                    <td>
                                        <%# Eval("ActivityStatus")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading valign-middle">
                                        <asp:Literal ID="Literal19" runat="server" Text="Site Access:" meta:resourcekey="LiteralResource19" /></td>
                                    <td>
                                        <div class="opt-in-out-text w-50 <%# HasMSProductEnvironment()? "mt-1x" :"" %>">
                                            <asp:Literal ID="ltrSiteAccess" runat="server" Text='<%# HasMSProductEnvironment()?Eval("SiteAccess"):GetLocalResourceObject("MIF_MerchantDetails_ORIJS_Text_NA").ToString() %>' />
                                        </div>

                                        <as:PlaceHolder runat="server" ID="PlaceHolder1" Visible='<%# !IsCaseManagement %>'>
                                            <div class="opt-in-out-button pull-right">
                                                <as:Button ID="uxSiteAccess" runat="server" Text='<%# SetStatusText(Eval("SiteAccess")) %>'
                                                    Visible='<%# SetVisible(Eval("SiteAccess")) %>' OnClientClick='<%# SetURLForSiteAccessButton() %>' CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSiteAccessResource1" />
                                            </div>
                                        </as:PlaceHolder>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal20" runat="server" Text="Tax ID:" meta:resourcekey="LiteralResource20" /></td>
                                    <td>
                                        <%# CheckPermisson(Eval("TaxID"), WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal21" runat="server" Text="SIC/MCC:" meta:resourcekey="LiteralResource21" /></td>
                                    <td>
                                        <%# FormatSIC(Eval("SICCode"), Eval("SICCodeDesc"))%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal22" runat="server" Text="Average Sales Amount:" meta:resourcekey="LiteralResource22" /></td>
                                    <td>
                                        <%# FormatCurrency(Eval("Avg_Ticket_Amt"))%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                    <asp:PlaceHolder ID="phdHierarchy" runat="server">
                        <div class="row" id="hierarchyinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciHierarchyInformation">
                                <h2 class="grid-title">
                                    <asp:Literal ID="Literal23" runat="server" Text="Hierarchy Information" meta:resourcekey="LiteralResource23" /></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciHierarchyInformation">
                                <table class="ASTable">
                                    <colgroup>
                                        <col class="w-20" />
                                        <col class="w-30" />
                                        <col class="w-20" />
                                        <col class="w-30" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal26" runat="server" Text="Client Name:" meta:resourcekey="LiteralResource26" /></td>
                                        <td colspan="3">
                                            <%# Eval("ClientName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal27" runat="server" Text="Client Login:" meta:resourcekey="LiteralResource27" /></td>
                                        <td>
                                            <%# Eval("ClientLogin")%>
                                        </td>
                                        <td class="heading">
                                            <asp:Literal ID="Literal28" runat="server" Text="Corporate:" meta:resourcekey="LiteralResource59"/></td>
                                        <td>
                                            <as:PlaceHolder ID="uxCorporate" runat="server" Visible='<%# CheckHierarchy("SFTCORP") %>'>
                                                <a href="#" id="uxLinkCorporate" runat="server"><%= BindValue("EntityNumber1")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal34" runat="server" Text='<%# Eval("EntityNumber1") %>' Visible='<%# !CheckHierarchy("SFTCORP") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal29" runat="server" Text="Region:" meta:resourcekey="LiteralResource60"/></td>
                                        <td>
                                            <as:PlaceHolder ID="uxRegion" runat="server" Visible='<%# CheckHierarchy("SFTREGN") %>'>
                                                <a href="#" id="uxLinkRegion" runat="server"><%= BindValue("EntityNumber2")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal35" runat="server" Text='<%# Eval("EntityNumber2") %>' Visible='<%# !CheckHierarchy("SFTREGN") %>'></as:Literal>

                                        </td>
                                        <td class="heading">
                                            <asp:Literal ID="Literal11" runat="server" Text="Principal:" meta:resourcekey="LiteralResource61"/></td>
                                        <td>
                                            <as:PlaceHolder ID="uxPrincipal" runat="server" Visible='<%# CheckHierarchy("SFTPRIN") %>'>
                                                <a href="#" id="uxLinkPrincipal" runat="server"><%= BindValue("EntityNumber3")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal40" runat="server" Text='<%# Eval("EntityNumber3") %>' Visible='<%# !CheckHierarchy("SFTPRIN") %>'></as:Literal>

                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal24" runat="server" Text="Service Rep:" meta:resourcekey="LiteralResource62"/></td>
                                        <td>
                                            <as:PlaceHolder ID="uxServiceRep" runat="server" Visible='<%# CheckHierarchy("SFTSERVICEREP") %>'>
                                                <a href="#" id="uxLinkServiceRep" runat="server"><%= BindValue("EntityNumber4")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal41" runat="server" Text='<%# Eval("EntityNumber4") %>' Visible='<%# !CheckHierarchy("SFTSERVICEREP") %>'></as:Literal>

                                        </td>
                                        <td class="heading">
                                            <asp:Literal ID="Literal25" runat="server" Text="Association:" meta:resourcekey="LiteralResource58"/></td>
                                        <td>
                                            <as:PlaceHolder ID="uxAssc" runat="server" Visible='<%# CheckHierarchy("SFTASSC") %>'>
                                                <a href="#" id="uxLinkAssc" runat="server"><%= BindValue("EntityNumber5")%></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="Literal56" runat="server" Text='<%# Eval("EntityNumber5") %>' Visible='<%# !CheckHierarchy("SFTASSC") %>'></as:Literal>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal30" runat="server" Text="Chain:" meta:resourcekey="LiteralResource30" /></td>
                                        <td colspan="3">
                                            <as:PlaceHolder ID="uxChain" runat="server">
                                                <a href="#" id="uxLinkChain" runat="server"><%= BindValue("Chain")%></a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxPlaceChainNew" runat="server" Visible='<%# CheckPermissionAddEditChain(Eval("SiteAccess")) %>'>
                                                <a href="#" onclick="return ShowPopupModal('CreateNewChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")) %>','auto');">
                                                    <asp:Literal ID="Literal31" runat="server" Text="Create New Chain" meta:resourcekey="LiteralResource56" /></a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxPlaceExistChain" runat="server" Visible='<%# CheckPermissionAddEditChain(Eval("SiteAccess")) %>'>
                                                or <a href="#" onclick="return ShowPopupModal('ModifyMerchantChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")+"&Chain="+Eval("Chain")) %>','auto');">
                                                    <asp:Literal ID="Literal32" runat="server" Text="Add to Existing Chain" meta:resourcekey="LiteralResource57" /></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHChain" runat="server" Text='<%# Eval("Chain") %>'></as:Literal>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                </div>
                <div class="col-md-6">
                    <div class="row" id="bankinfo">
                        <div class="col-xs-12" data-toggle="collapse" data-target="#ciBankInformation">
                            <h2 class="grid-title">
                                <asp:Literal ID="Literal33" runat="server" Text="Bank Information" meta:resourcekey="LiteralResource31" /></h2>
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
                                        <asp:Literal ID="Literal36" runat="server" Text="Bank Name:" meta:resourcekey="LiteralResource34" /></td>
                                    <td>
                                        <%# Eval("BankName")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal37" runat="server" Text="Routing #:" meta:resourcekey="LiteralResource35" /></td>
                                    <td>
                                        <%# GetRoutingNumber(Eval("RoutingNumber").ToString(), Eval("PartialRoutingNumber").ToString())%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal38" runat="server" Text="DDA #:" meta:resourcekey="LiteralResource36" /></td>
                                    <td>
                                        <%#CheckPermisson(Eval("DDANumber"), WebSiteConstants.SEC_PERMISSION_DDA)%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                    <!-- Risk Info --> 
                    <uc:RiskInfo ID="uxRiskInfo" runat="server" />

                    <as:PlaceHolder runat="server" ID="PlaceHolder2" Visible='<%# !IsCaseManagement %>'>
                        <!--Pricing Information-->
                        <div class="row" id="pricinginfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciPricingInformation">
                                <h2 class="grid-title">
                                    <asp:Literal ID="Literal39" runat="server" Text="Pricing Information" meta:resourcekey="LiteralResource37" /></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciPricingInformation">
                                <table class="ASTable">
                                    <colgroup>
                                        <col class="w-40" />
                                        <col class="w-30" />
                                        <col class="w-30" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal42" runat="server" Text="Weekly Deposit" meta:resourcekey="LiteralResource40" /></td>
                                        <td colspan="2">
                                            <%# FormatCurrency(Eval("weekly_deposit_amt"))%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal43" runat="server" Text="Auth Grid" meta:resourcekey="LiteralResource41" /></td>
                                        <td colspan="2">
                                            <%# Eval("auth_inc_grid_id")%>
                                        </td>
                                    </tr>
                                    <tr class="section-heading">
                                        <td>&nbsp; </td>
                                        <td>
                                            <asp:Literal ID="Literal44" runat="server" Text="Mastercard" meta:resourcekey="LiteralResource42" /></td>
                                        <td>
                                            <asp:Literal ID="Literal45" runat="server" Text="Visa" meta:resourcekey="LiteralResource43" /></td>
                                    </tr>
                                    <!-- -->
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal57" runat="server" Text="Discount Rate:" meta:resourcekey="LiteralResourceDiscountRate" /></td>
                                        <td>
                                            <%# FormatPercentText(Eval("MCDiscountRate"), "N/A")%>
                                        </td>
                                        <td>
                                            <%# FormatPercentText(Eval("VSDiscountRate"), "N/A")%>
                                        </td>
                                    </tr>
                                    <!-- -->
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal46" runat="server" Text="Qual Rate:" meta:resourcekey="LiteralResource44" /></td>
                                        <td>
                                            <%# FormatPercent(Eval("MCQualRate"), "N/A")%>
                                        </td>
                                        <td>
                                            <%# FormatPercent(Eval("VSQualRate"), "N/A")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal47" runat="server" Text="Mid-Qual Rate:" meta:resourcekey="LiteralResource45" /></td>
                                        <td>
                                            <%# FormatPercent(Eval("MCMQualRate"), "N/A")%>
                                        </td>
                                        <td>
                                            <%# FormatPercent(Eval("VSMQualRate"), "N/A")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal48" runat="server" Text="Non-Qual Rate:" meta:resourcekey="LiteralResource46" /></td>
                                        <td>
                                            <%# FormatPercent(Eval("MCNQualRate"), "N/A")%>
                                        </td>
                                        <td>
                                            <%# FormatPercent(Eval("VSNQualRate"), "N/A")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal49" runat="server" Text="Min Monthly Fee:" meta:resourcekey="LiteralResource47" /></td>
                                        <td colspan="2">
                                            <%# FormatCurrency(Eval("acct_fee_chg2", "$0.00"))%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal50" runat="server" Text="Merch. Service Fee:" meta:resourcekey="LiteralResource48" /></td>
                                        <td colspan="2">
                                            <%# FormatCurrency(Eval("acct_fee_chg", "$0.00"))%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal51" runat="server" Text="Deposit Type:" meta:resourcekey="LiteralResource49" /></td>
                                        <td colspan="2">
                                            <%# Eval("deposit_type")%>
                                        </td>
                                    </tr>
                                    <tr class="section-heading">
                                        <td colspan="3">
                                            <asp:Literal ID="Literal52" runat="server" Text="Fee Classes" meta:resourcekey="LiteralResource50" /></td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal53" runat="server" Text="Fee Flag 1:" meta:resourcekey="LiteralResource51" /></td>
                                        <td colspan="2">
                                            <%# Eval("FeeFlag1")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal54" runat="server" Text="Fee Flag 2:" meta:resourcekey="LiteralResource52" /></td>
                                        <td colspan="2">
                                            <%# Eval("FeeFlag2")%>
                                        </td>
                                    </tr>
                                    <tr class="section-heading">
                                        <td colspan="3">
                                            <asp:Literal ID="Literal55" runat="server" Text="Other Card Types" meta:resourcekey="LiteralResource53" /></td>
                                    </tr>
                                    <asp:Repeater ID="uxOtherCardTypes" runat="server">
                                        <ItemTemplate>
                                            <tr class="Row">
                                                <td class="heading">
                                                    <%# Eval("Card_type")%>
                                                </td>
                                                <td colspan="2">
                                                    <%# Eval("MerchantNumber")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="AltRow">
                                                <td class="heading">
                                                    <%# Eval("Card_type")%>
                                                </td>
                                                <td colspan="2">
                                                    <%# Eval("MerchantNumber")%>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                    </as:PlaceHolder>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <as:ASRadCodeBlock runat="server" ID="uxRadCodeBlock1">
        <script type="text/javascript">
            var MIF_MerchantDetails_ORI_uxtxtRelationShipManager = '#<%= uxtxtRelationShipManager.ClientID %>';
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~") %>res/js/MIF_MerchantDetails_ORI.js"></script>
    </as:ASRadCodeBlock>
</asp:PlaceHolder>
