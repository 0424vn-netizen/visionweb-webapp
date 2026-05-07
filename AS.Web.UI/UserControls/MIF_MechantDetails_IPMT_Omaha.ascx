<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MechantDetails_IPMT_Omaha.ascx.cs" Inherits="UserControls_MIF_MechantDetails_IPMT_Omaha" %>
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
                        <asp:Literal ID="Literal3" runat="server" Text="Contact:" meta:resourcekey="LiteralResource3" />
                    </td>
                    <td>
                        <%= BindValue("Contact")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal4" runat="server" Text="Status:" meta:resourcekey="LiteralResource4" />
                    </td>
                    <td>
                        <%= BindValue("Status")%>
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
                        <asp:Label runat="server" ID="lnkLastBatch" Text="" meta:resourcekey="lnkLastBatchResource1"></asp:Label>
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
                            <as:ValidatorLabel ID="ValidatorLabel1" runat="server" Text="Relationship Manager:" CssClass="no-marpad-im"
                                ApplyFor="uxtxtRelationShipManager" meta:resourcekey="MIF_MerchantDetail_FDRASCX_Text_RelationshipManager">
                            </as:ValidatorLabel>
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
                                <as:CustomValidationItem ClientValidationFunction="CheckPrefix" ControlToValidateID="uxtxtRelationShipManager"
                                    Message="Prefix or suffix (for example Mr./Ms/Sir/Jr.) are not allowed."
                                    meta:resourcekey="MIF_MerchantDetail_FDRASCX_Text_UnallowPrefixSuffix" />
                                <as:CustomValidationItem ClientValidationFunction="CheckPrefixSpecial" ControlToValidateID="uxtxtRelationShipManager"
                                    Message="Relationship Manager field must be 30 characters or less and may not include special characters"
                                    meta:resourcekey="MIF_MerchantDetail_FDRASCX_Text_AtLeast30NoSpecialChars" />
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

    <asp:Repeater ID="rptMerchantInfo" runat="server">
        <ItemTemplate>
            <div class="row">
                <div class="col-md-6">
                    <div class="row" id="businessinfo">
                        <div class="col-xs-12" data-toggle="collapse" data-target="#ciBusinessInformation">
                            <h2 class="grid-title">
                                <asp:Literal ID="Literal10" runat="server" Text="Business Information" meta:resourcekey="LiteralResource10" /></h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 in" id="ciBusinessInformation">
                            <table class="ASTable">
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal13" runat="server" Text="Corporate Name:" meta:resourcekey="LiteralResource13" /></td>
                                    <td>
                                        <%# Eval("CorporateName") %>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal14" runat="server" Text="Corporate Address:" meta:resourcekey="LiteralResource14" /></td>
                                    <td><%# Eval("CorporateAddress")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal15" runat="server" Text="Fax:" meta:resourcekey="LiteralResource15" /></td>
                                    <td><%# FormatPhone(Eval("Fax"))%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal16" runat="server" Text="Open Date:" meta:resourcekey="LiteralResource16" /></td>
                                    <td>
                                        <%# FormatDate(Eval("OpenDate"))%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal17" runat="server" Text="Closed Date:" meta:resourcekey="LiteralResource17" /></td>
                                    <td>
                                        <%# FormatDate(Eval("ClosedDate"))%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal18" runat="server" Text="Status:" meta:resourcekey="LiteralResource18" /></td>
                                    <td>
                                        <%# Eval("Status")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading valign-middle">
                                        <asp:Literal ID="Literal19" runat="server" Text="Site Access:" meta:resourcekey="LiteralResource19" /></td>
                                    <td>
                                        <div class="opt-in-out-text mt-1x">
                                            <asp:Literal ID="ltrSiteAccess" runat="server" Text='<%#Eval("SiteAccess")%>' />
                                        </div>
                                        <div class="pull-right opt-in-out-button">
                                            <as:Button ID="uxSiteAccess" runat="server" Text='<%# SetStatusText(Eval("SiteAccess")) %>' CssClass="btn btn-default" Visible='<%# SetVisible(Eval("SiteAccess")) %>' OnClientClick='<%# SetURLForSiteAccessButton() %>' IsStandardButton="False" meta:resourcekey="uxSiteAccessResource1" />
                                        </div>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal20" runat="server" Text="Tax ID:" meta:resourcekey="LiteralResource20" /></td>
                                    <td>
                                        <%#CheckPermisson(Eval("TaxID"),
                                        WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal21" runat="server" Text="SIC/MCC:" meta:resourcekey="LiteralResource21" /></td>
                                    <td><%# FormatSIC(Eval("SICMCC"), Eval("SICDescription"))%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal22" runat="server" Text="Deposit Type:" meta:resourcekey="LiteralResource22" /></td>
                                    <td><%# Eval("DepositType")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal23" runat="server" Text="ETC Type:" meta:resourcekey="LiteralResource23" /></td>
                                    <td><%# Eval("ETCType")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal24" runat="server" Text="ETC Cutoff:" meta:resourcekey="LiteralResource24" /></td>
                                    <td><%# Eval("ETCCutoff")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal25" runat="server" Text="Seasonal Merchant:" meta:resourcekey="LiteralResource25" /></td>
                                    <td><%#
                                        Eval("SeasonalMerchant")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal26" runat="server" Text="Statement Mail Flag:" meta:resourcekey="LiteralResource26" /></td>
                                    <td><span title='<%# Eval("StatementMailFlagDesc") %>'><%#Eval("StatementMailFlag")%></span>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal27" runat="server" Text="Statement I/C Print Options:" meta:resourcekey="LiteralResource27" /></td>
                                    <td><span title='<%# Eval("StatementICPrintOptionsDesc") %>'><%#Eval("StatementICPrintOptions")%></span>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal28" runat="server" Text="Statement Online Debit Fee Print Option:" meta:resourcekey="LiteralResource28" /></td>
                                    <td><span title='<%# Eval("StatementOnlineDebitFeePrintOptionDesc") %>'><%#Eval("StatementOnlineDebitFeePrintOption")%> </span>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal29" runat="server" Text="Statement Hold Flag:" meta:resourcekey="LiteralResource29" /></td>
                                    <td><%#Eval("StatementHoldFlag")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal30" runat="server" Text="Misc. 1:" meta:resourcekey="LiteralResource30" /></td>
                                    <td><%# Eval("Misc1")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal31" runat="server" Text="Misc. 2:" meta:resourcekey="LiteralResource31" /></td>
                                    <td><%# Eval("Misc2")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal32" runat="server" Text="Tin Type:" meta:resourcekey="LiteralResource32" /></td>
                                    <td><%# Eval("TinTypeDesc")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal33" runat="server" Text="Spark Excl ID:" meta:resourcekey="LiteralResource33" /></td>
                                    <td><span title='<%# Eval("SparkExclDesc") %>'>
                                        <%# Eval("SparkExclID")%> </span>
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
                                <h2 class="grid-title">
                                    <asp:Literal ID="Literal34" runat="server" Text="Hierarchy Information" meta:resourcekey="LiteralResource34" /></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciHierarchyInformation">
                                <table class="ASTable" id="uxTableOMAHA" runat="server">
                                    <tr class="Row">
                                        <td class="heading valign-middle">
                                            <asp:Literal ID="Literal37" runat="server" Text="Sys:" meta:resourcekey="LiteralResource176" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxSys" runat="server" Visible='<%# CheckHierarchy("SYS") %>'>
                                                <a href="#" id="uxLinkSys" runat="server"><%= BindValue("Sys")%></a>
                                            </as:PlaceHolder>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal38" runat="server" Text="Sys/Prin:" meta:resourcekey="LiteralResource177" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxSysPrin" runat="server" Visible='<%# CheckHierarchy("SYSPRIN") %>'>
                                                <a href="#" id="uxLinkSysPrin" runat="server"><%= BindValue("SysPrin")%></a>
                                            </as:PlaceHolder>
                                        </td>
                                    </tr>

                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal40" runat="server" Text="Sys/Prin/Agent:" meta:resourcekey="LiteralResource40" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxSysPrinAgent" runat="server" Visible='<%# CheckHierarchy("AGENT") %>'>
                                                <a href="#" id="uxLinkSysPrinAgent" runat="server"><%# BindValue("SysPrinAgent")%></a>
                                            </as:PlaceHolder>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div class="row" id="bankinfo">
                        <div class="col-xs-12" data-toggle="collapse" data-target="#ciBankInformation">
                            <h2 class="grid-title">
                                <asp:Literal ID="Literal44" runat="server" Text="Bank Information" meta:resourcekey="LiteralResource44" /></h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 in" id="ciBankInformation">
                            <table class="ASTable">
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal47" runat="server" Text="Bank Name:" meta:resourcekey="LiteralResource47" /></td>
                                    <td>
                                        <%# Eval("BankName")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal48" runat="server" Text="Routing #:" meta:resourcekey="LiteralResource48" /></td>
                                    <td>
                                        <%# GetRoutingNumber(Eval("RoutingNumber").ToString(),Eval("PartialRoutingNumber").ToString())%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal49" runat="server" Text="DDA #:" meta:resourcekey="LiteralResource49" /></td>
                                    <td>
                                        <%#CheckPermisson(Eval("DDANumber"), WebSiteConstants.SEC_PERMISSION_DDA)%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal50" runat="server" Text="Days Hold:" meta:resourcekey="LiteralResource50" /></td>
                                    <td>
                                        <%# Eval("DaysHold")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal51" runat="server" Text="ACH Monthly:" meta:resourcekey="LiteralResource51" /></td>
                                    <td>
                                        <span title='<%# Eval("MonthlyACHDesc") %>'><%#Eval("MonthlyACH")%> </span>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal52" runat="server" Text="Disbursement Method: Deposit:" meta:resourcekey="LiteralResource52" /></td>
                                    <td>
                                        <span title='<%# Eval("DisbursementDepositDesc") %>'><%#Eval("DisbursementDeposit")%></span>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal53" runat="server" Text="Disbursement Method: Adjustments:" meta:resourcekey="LiteralResource53" /></td>
                                    <td>
                                        <span title='<%# Eval("DisbursementAdjustmentsDesc") %>'><%#Eval("DisbursementAdjustments")%></span>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal54" runat="server" Text="Disbursement Method: Discount:" meta:resourcekey="LiteralResource54" /></td>
                                    <td>
                                        <span title='<%# Eval("DisbursementDiscountDesc") %>'><%#Eval("DisbursementDiscount")%></span>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal55" runat="server" Text="Disbursement Method: Generic Debit:" meta:resourcekey="LiteralResource55" /></td>
                                    <td>
                                        <span title='<%# Eval("DisbursementGenericDebitDesc") %>'><%#Eval("DisbursementGenericDebit")%> </span>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal56" runat="server" Text="Disbursement Detail:" meta:resourcekey="LiteralResource56" /></td>
                                    <td>
                                        <span title='<%# Eval("DisbursementDetailDesc") %>'><%#Eval("DisbursementDetail")%> </span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="row" id="othercardinfo">
                        <div class="col-xs-12" data-toggle="collapse" data-target="#ciOtherCardInformation">
                            <h2 class="grid-title">
                                <asp:Literal ID="Literal57" runat="server" Text="Other Card Information" meta:resourcekey="LiteralResource57" /></h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 in" id="ciOtherCardInformation">
                            <table class="ASTable">
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal60" runat="server" Text="AMEX" meta:resourcekey="LiteralResource60" /></td>
                                    <td>
                                        <%# Eval("AMEX")%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal61" runat="server" Text="Pin Debit" meta:resourcekey="LiteralResource61" /></td>
                                    <td>
                                        <%# Eval("PinDebit")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal62" runat="server" Text="AMEX ONEPOINT" meta:resourcekey="LiteralResource62" /></td>
                                    <td>
                                        <%# Eval("AMEXONPOINT")%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal63" runat="server" Text="JCB" meta:resourcekey="LiteralResource63" /></td>
                                    <td>
                                        <%# Eval("JCB")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal64" runat="server" Text="Discover" meta:resourcekey="LiteralResource64" /></td>
                                    <td>
                                        <%# Eval("Discover")%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal65" runat="server" Text="Wright Express" meta:resourcekey="LiteralResource65" /></td>
                                    <td>
                                        <%# Eval("WrightExpress")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal66" runat="server" Text="Discover Full AQC" meta:resourcekey="LiteralResource66" /></td>
                                    <td>
                                        <%# Eval("DiscoverFullAQC")%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal67" runat="server" Text="Voyager" meta:resourcekey="LiteralResource67" /></td>
                                    <td>
                                        <%# Eval("Voyager")%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <as:PlaceHolder runat="server" ID="PlaceHolder5">
                <!--Pricing Information-->
                <div class="row" id="pricinginfo">
                    <div class="col-xs-12" data-toggle="collapse" data-target="#ciPricingInformation">
                        <h2 class="grid-title">
                            <asp:Literal ID="Literal68" runat="server" Text="Pricing Information" meta:resourcekey="LiteralResource68" /></h2>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 in" id="ciPricingInformation">
                        <table class="ASTable text-center no-border">
                            <tbody>
                                <tr class="Row pricing-label valign-bottom">
                                    <td class="pricing-header valign-top" rowspan="2"></td>
                                    <td>
                                        <asp:Literal ID="Literal71" runat="server" Text="Daily Deposit" meta:resourcekey="LiteralResource71" /></td>
                                    <td>
                                        <asp:Literal ID="Literal72" runat="server" Text="Weekly Deposit" meta:resourcekey="LiteralResource72" /></td>
                                    <td>
                                        <asp:Literal ID="Literal73" runat="server" Text="Daily Auth Amount" meta:resourcekey="LiteralResource73" /></td>
                                    <td>
                                        <asp:Literal ID="Literal74" runat="server" Text="Avg. Ticket Amount" meta:resourcekey="LiteralResource74" /></td>
                                    <td>
                                        <asp:Literal ID="Literal75" runat="server" Text="Auth Grid" meta:resourcekey="LiteralResource75" /></td>
                                    <td>
                                        <asp:Literal ID="Literal76" runat="server" Text="User Defined Grid" meta:resourcekey="LiteralResource76" /></td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td><%# Eval("DailyDeposit")%></td>
                                    <td><%# Eval("WeeklyDeposit")%></td>
                                    <td><%# Eval("DailyAuthAmount")%></td>
                                    <td><%# Eval("AvgTicketAmount")%></td>
                                    <td><%# Eval("AuthGrid")%></td>
                                    <td><%# Eval("UserDefinedGrid")%></td>
                                    <td></td>
                                </tr>
                                <tr class="AltRow pricing-label valign-bottom">
                                    <td class="pricing-header valign-top" rowspan="8">
                                        <asp:Literal ID="Literal77" runat="server" Text="Income Factors" meta:resourcekey="LiteralResource77" /></td>
                                    <td>
                                        <asp:Literal ID="Literal78" runat="server" Text="Account Fee 1" meta:resourcekey="LiteralResource78" /></td>
                                    <td>
                                        <asp:Literal ID="Literal79" runat="server" Text="Account Fee 2" meta:resourcekey="LiteralResource79" /></td>
                                    <td>
                                        <asp:Literal ID="Literal80" runat="server" Text="Account Fee 3" meta:resourcekey="LiteralResource80" /></td>
                                    <td>
                                        <asp:Literal ID="Literal81" runat="server" Text="Account Fee 4" meta:resourcekey="LiteralResource81" /></td>
                                    <td>
                                        <asp:Literal ID="Literal82" runat="server" Text="Account Fee 5" meta:resourcekey="LiteralResource82" /></td>
                                    <td>
                                        <asp:Literal ID="Literal83" runat="server" Text="Recurring Fee Flag" meta:resourcekey="LiteralResource83" /></td>
                                    <td>
                                        <asp:Literal ID="Literal84" runat="server" Text="Recur Fee Ind" meta:resourcekey="LiteralResource84" /></td>
                                </tr>
                                <tr class="AltRow pricing-value">
                                    <td><%# FormatCurrency(Eval("AccountFee1")) %></td>
                                    <td><%# FormatCurrency(Eval("AccountFee2")) %></td>
                                    <td><%# FormatCurrency(Eval("AccountFee3")) %></td>
                                    <td><%# FormatCurrency(Eval("AccountFee4")) %></td>
                                    <td><%# FormatCurrency(Eval("AccountFee5")) %></td>
                                    <td><%# Eval("RecurFeeFlag") %></td>
                                    <td><%# Eval("RecurFeeInd") %></td>
                                </tr>
                                <tr class="AltRow pricing-label valign-bottom">
                                    <td>
                                        <asp:Literal ID="Literal85" runat="server" Text="Recur Fee Amt" meta:resourcekey="LiteralResource85" /></td>
                                    <td>
                                        <asp:Literal ID="Literal86" runat="server" Text="Recur Fee Desc" meta:resourcekey="LiteralResource86" /></td>
                                    <td>
                                        <asp:Literal ID="Literal87" runat="server" Text="Stmt Bundle Option" meta:resourcekey="LiteralResource87" /></td>
                                    <td>
                                        <asp:Literal ID="Literal88" runat="server" Text="Bundle Pct" meta:resourcekey="LiteralResource88" /></td>
                                    <td>
                                        <asp:Literal ID="Literal89" runat="server" Text="Bundle Rate" meta:resourcekey="LiteralResource89" /></td>
                                    <td>
                                        <asp:Literal ID="Literal90" runat="server" Text="Batch Header Fee" meta:resourcekey="LiteralResource90" /></td>
                                    <td>
                                        <asp:Literal ID="Literal91" runat="server" Text="Return Transaction Fee" meta:resourcekey="LiteralResource91" /></td>
                                </tr>
                                <tr class="AltRow pricing-value">
                                    <td><%# FormatCurrency(Eval("RecurFeeAmt")) %></td>
                                    <td><%# Eval("RecurFeeDesc")%></td>
                                    <td><%# Eval("StmtBundleOption")%></td>
                                    <td><%# Eval("BundlePct")%></td>
                                    <td><%# Eval("BundleRate")%></td>
                                    <td><%#FormatCurrency(Eval("BatchHeaderFee"), 4)%></td>
                                    <td><%# FormatCurrency(Eval("ReturnTransFee"),4)%></td>
                                </tr>
                                <tr class="AltRow pricing-label valign-bottom">
                                    <td>
                                        <asp:Literal ID="Literal92" runat="server" Text="Chargeback Fee" meta:resourcekey="LiteralResource92" /></td>
                                    <td>
                                        <asp:Literal ID="Literal93" runat="server" Text="Retrieval Fee" meta:resourcekey="LiteralResource93" /></td>
                                    <td>
                                        <asp:Literal ID="Literal94" runat="server" Text="Sales Trans Fee" meta:resourcekey="LiteralResource94" /></td>
                                    <td>
                                        <asp:Literal ID="Literal95" runat="server" Text="Other Volume %" meta:resourcekey="LiteralResource95" /></td>
                                    <td>
                                        <asp:Literal ID="Literal96" runat="server" Text="Other Item Charge" meta:resourcekey="LiteralResource96" /></td>
                                    <td>
                                        <asp:Literal ID="Literal97" runat="server" Text="Website Usage" meta:resourcekey="LiteralResource97" /></td>
                                    <td>
                                        <asp:Literal ID="Literal98" runat="server" Text="IVR Usage" meta:resourcekey="LiteralResource98" /></td>
                                </tr>
                                <tr class="AltRow pricing-value">
                                    <td><%# FormatCurrency(Eval("ChargebackFee"),4)%></td>
                                    <td><%# Eval("RetrievalFee")%></td>
                                    <td><%# FormatCurrency(Eval("SaleTransFee"),4) %></td>
                                    <td><%# Eval("OtherVolumnPercent")%></td>
                                    <td><%# FormatCurrency(Eval("OtherItemCharge"),4)%></td>
                                    <td><%# FormatCurrency(Eval("WebsiteUsage"),2)%></td>
                                    <td><%# FormatCurrency(Eval("IVRUsage"),2)%></td>
                                </tr>
                                <tr class="AltRow pricing-label valign-bottom">
                                    <td>
                                        <asp:Literal ID="Literal99" runat="server" Text="Unreg Pct" meta:resourcekey="LiteralResource99" /></td>
                                    <td>
                                        <asp:Literal ID="Literal100" runat="server" Text="Unreg Rate" meta:resourcekey="LiteralResource100" /></td>
                                    <td>
                                        <asp:Literal ID="Literal101" runat="server" Text="Reg Pct" meta:resourcekey="LiteralResource101" /></td>
                                    <td>
                                        <asp:Literal ID="Literal102" runat="server" Text="Reg Rate" meta:resourcekey="LiteralResource102" /></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="AltRow pricing-value">
                                    <td><%# Eval("UnregPct")%></td>
                                    <td><%#  Eval("UnregRate")%></td>
                                    <td><%# Eval("RegPct")%></td>
                                    <td><%# Eval("RegRate")%></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-label valign-bottom">
                                    <td class="pricing-header valign-top" rowspan="2">
                                        <asp:Literal ID="Literal103" runat="server" Text="Visa Assoc Fees" meta:resourcekey="LiteralResource103" /></td>
                                    <td>
                                        <asp:Literal ID="Literal104" runat="server" Text="Acqr Pr Fee" meta:resourcekey="LiteralResource104" /></td>
                                    <td>
                                        <asp:Literal ID="Literal105" runat="server" Text="Misuse Auth Fee" meta:resourcekey="LiteralResource105" /></td>
                                    <td>
                                        <asp:Literal ID="Literal106" runat="server" Text="ISA" meta:resourcekey="LiteralResource106" /></td>
                                    <td>
                                        <asp:Literal ID="Literal107" runat="server" Text="Zero Floor" meta:resourcekey="LiteralResource107" /></td>
                                    <td>
                                        <asp:Literal ID="Literal108" runat="server" Text="Intl Acq" meta:resourcekey="LiteralResource108" /></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td><%# Eval("AcqrPrFee")%></td>
                                    <td><%# Eval("MisuseAuthFee")%></td>
                                    <td><%# Eval("ISA")%></td>
                                    <td><%# Eval("ZeroFloor")%></td>
                                    <td><%# Eval("IntlAcq")%></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="AltRow pricing-label valign-bottom">
                                    <td class="pricing-header valign-top" rowspan="2">
                                        <asp:Literal ID="Literal109" runat="server" Text="MC Assoc Fees" meta:resourcekey="LiteralResource109" /></td>
                                    <td>
                                        <asp:Literal ID="Literal110" runat="server" Text="NABU Fee" meta:resourcekey="LiteralResource110" /></td>
                                    <td>
                                        <asp:Literal ID="Literal111" runat="server" Text="Cross Border Fee" meta:resourcekey="LiteralResource111" /></td>
                                    <td>
                                        <asp:Literal ID="Literal112" runat="server" Text="Acq Support Fee" meta:resourcekey="LiteralResource112" /></td>
                                    <td>
                                        <asp:Literal ID="Literal113" runat="server" Text="Reversal Fee" meta:resourcekey="LiteralResource113" /></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="AltRow pricing-value">
                                    <td><%# Eval("NABUFee")%></td>
                                    <td><%# Eval("CrossBorderFee")%></td>
                                    <td><%# Eval("AcqSupportFee")%></td>
                                    <td><%# Eval("ReversalFee")%></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-label valign-bottom">
                                    <td class="pricing-header valign-top" rowspan="2">
                                        <asp:Literal ID="Literal114" runat="server" Text="Disc Full Acq Assoc Fees" meta:resourcekey="LiteralResource114" /></td>
                                    <td>
                                        <asp:Literal ID="Literal115" runat="server" Text="INTL Process Flag" meta:resourcekey="LiteralResource115" /></td>
                                    <td>
                                        <asp:Literal ID="Literal116" runat="server" Text="INTL Service Flag" meta:resourcekey="LiteralResource116" /></td>
                                    <td>
                                        <asp:Literal ID="Literal117" runat="server" Text="Data Usage Flag" meta:resourcekey="LiteralResource117" /></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td>
                                        <span title='<%# Eval("INTLProcessFlagDesc") %>'><%#Eval("INTLProcessFlag")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("INTLServiceFlagDesc") %>'><%#Eval("INTLServiceFlag")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DataUsageFlagDesc") %>'><%#Eval("DataUsageFlag")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <!--Merchant Card Information-->
                <div class="row" id="merchantcardinfo">
                    <div class="col-xs-12" data-toggle="collapse" data-target="#ciMerchantCardInformation">
                        <h2 class="grid-title">
                            <asp:Literal ID="Literal118" runat="server" Text="Merchant Card Information" meta:resourcekey="LiteralResource118" /></h2>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 in" id="ciMerchantCardInformation">
                        <div class="table-responsive">
                            <table class="ASTable">
                                <tr class="Row">
                                    <td></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal120" runat="server" Text="MC" meta:resourcekey="LiteralResource119" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal121" runat="server" Text="MC DB" meta:resourcekey="LiteralResource120" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal122" runat="server" Text="VISA" meta:resourcekey="LiteralResource121" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal123" runat="server" Text="VISA DB" meta:resourcekey="LiteralResource122" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal124" runat="server" Text="DISC FULL ACQ" meta:resourcekey="LiteralResource123" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal125" runat="server" Text="DISC FULL ACQ DB" meta:resourcekey="LiteralResource124" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal126" runat="server" Text="DISC PASS THRU" meta:resourcekey="LiteralResource125" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal127" runat="server" Text="AMEXONEPT" meta:resourcekey="LiteralResource126" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal128" runat="server" Text="AMEX PASS THRU" meta:resourcekey="LiteralResource127" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal179" runat="server" Text="AMEX" meta:resourcekey="LiteralResource60"/></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal1" runat="server" Text=">WRIGHT EXPRESS" meta:resourcekey="Literal1Resource1" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal129" runat="server" meta:resourcekey="LiteralResource128" Text="VOYAGER"></asp:Literal></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal130" runat="server" Text="GENERIC DEBIT" meta:resourcekey="LiteralResource129" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal131" runat="server" Text="DINERS" meta:resourcekey="LiteralResource130" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal132" runat="server" Text="EBT CASH/B" meta:resourcekey="LiteralResource131" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal133" runat="server" Text="EBT F/STMP" meta:resourcekey="LiteralResource132" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal134" runat="server" Text="EBT-TAPE" meta:resourcekey="LiteralResource133" /></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal135" runat="server" Text="JCB" meta:resourcekey="LiteralResource134" /></td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal136" runat="server" Text="Proc SW" meta:resourcekey="LiteralResource135" /></td>
                                    <td>
                                        <%# Eval("ProcessSWMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWAmesPassThru")%>
                                    </td>
                                     <td>
                                        <%# Eval("ProcessSWAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWJCB")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal137" runat="server" Text="Fee Class" meta:resourcekey="LiteralResource136" /></td>
                                    <td>
                                        <%# Eval("FeeClassMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassAmesPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassJCB")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal138" runat="server" Text="Qual Rate" meta:resourcekey="LiteralResource137" /></td>
                                    <td>
                                        <%# Eval("QualRateMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateAmesPassThru")%>
                                    </td>
                                     <td>
                                        <%# Eval("QualRateAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateJCB")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal139" runat="server" Text="Mid-Qual Rate" meta:resourcekey="LiteralResource138" /></td>
                                    <td>
                                        <%# Eval("MidQualRateMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateAmesPassThru")%>
                                    </td>
                                     <td>
                                        <%# Eval("MidQualRateAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateJCB")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal140" runat="server" Text="Non-Qual Rate" meta:resourcekey="LiteralResource139" /></td>
                                    <td>
                                        <%# Eval("NonQualRateMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateAmesPassThru")%>
                                    </td>
                                     <td>
                                        <%# Eval("NonQualRateAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateJCB")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal141" runat="server" Text="Interchange Fee Flag" meta:resourcekey="LiteralResource140" /></td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescMC") %>'><%#Eval("InterchangeFeeFlagMC")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescMCDB") %>'><%#Eval("InterchangeFeeFlagMCDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescVS") %>'><%#Eval("InterchangeFeeFlagVS")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescVSDB") %>'><%#Eval("InterchangeFeeFlagVSDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescDI") %>'><%#Eval("InterchangeFeeFlagDI")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescDIDB") %>'><%#Eval("InterchangeFeeFlagDIDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescDIPassThru") %>'><%#Eval("InterchangeFeeFlagDIPassThru")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescAmexOnePT") %>'><%#Eval("InterchangeFeeFlagAmexOnePT")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescAmesPassThru") %>'><%#Eval("InterchangeFeeFlagAmesPassThru")%></span>
                                    </td>
                                     <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescAmexBlue") %>'><%#Eval("InterchangeFeeFlagAmexBlue")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescWrightExpress") %>'><%#Eval("InterchangeFeeFlagWrightExpress")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescVoyager") %>'><%#Eval("InterchangeFeeFlagVoyager")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescGenericDebit") %>'><%#Eval("InterchangeFeeFlagGenericDebit")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescDinner") %>'><%#Eval("InterchangeFeeFlagDinner")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescEBT_CASH_B") %>'><%#Eval("InterchangeFeeFlagEBT_CASH_B")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescEBT_F_STMP") %>'><%#Eval("InterchangeFeeFlagEBT_F_STMP")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescEBT_TAPE") %>'><%#Eval("InterchangeFeeFlagEBT_TAPE")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescJCB") %>'><%#Eval("InterchangeFeeFlagJCB")%></span>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal142" runat="server" Text="Dues & Assessment Flag" meta:resourcekey="LiteralResource141" /></td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescMC") %>'><%#Eval("DuesAssessmentFlagMC")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescMCDB") %>'><%#Eval("DuesAssessmentFlagMCDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescVS") %>'><%#Eval("DuesAssessmentFlagVS")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescVSDB") %>'><%#Eval("DuesAssessmentFlagVSDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescDI") %>'><%# Eval("DuesAssessmentFlagDI")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescDIDB") %>'><%# Eval("DuesAssessmentFlagDIDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescDIPassThru") %>'><%# Eval("DuesAssessmentFlagDIPassThru")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescAmexOnePT") %>'><%# Eval("DuesAssessmentFlagAmexOnePT")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescAmesPassThru") %>'><%# Eval("DuesAssessmentFlagAmesPassThru")%></span>
                                    </td>
                                     <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescAmexBlue") %>'><%# Eval("DuesAssessmentFlagAmexBlue")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescWrightExpress") %>'><%# Eval("DuesAssessmentFlagWrightExpress")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescVoyager") %>'><%# Eval("DuesAssessmentFlagVoyager")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescGenericDebit") %>'><%# Eval("DuesAssessmentFlagGenericDebit")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescDinner") %>'><%# Eval("DuesAssessmentFlagDinner")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescEBT_CASH_B") %>'><%# Eval("DuesAssessmentFlagEBT_CASH_B")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescEBT_F_STMP") %>'><%# Eval("DuesAssessmentFlagEBT_F_STMP")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescEBT_TAPE") %>'><%# Eval("DuesAssessmentFlagEBT_TAPE")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescJCB") %>'><%# Eval("DuesAssessmentFlagJCB")%></span>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal143" runat="server" Text="Dues & Assessments Vol < 1.000" meta:resourcekey="LiteralResource142" /></td>
                                    <td>
                                        <%# Eval("DueAsmtVolLower1MC")%>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal144" runat="server" Text="Dues & Assessments Item < 1.000" meta:resourcekey="LiteralResource143" /></td>
                                    <td>
                                        <%# Eval("DueAsmtItemLower1MC")%>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal145" runat="server" Text="Dues & Assessments Vol > 1.000" meta:resourcekey="LiteralResource144" /></td>
                                    <td>
                                        <%# Eval("DueAsmtVolGreater1MC")%>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal146" runat="server" Text="Dues & Assessments Item > 1.000" meta:resourcekey="LiteralResource145" /></td>
                                    <td>
                                        <%# Eval("DueAsmtItemGreater1MC")%>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal147" runat="server" Text="Merchant Pricing Grid" meta:resourcekey="LiteralResource146" /></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridAmesPassThru")%>
                                    </td>
                                     <td>
                                        <%# Eval("MerchantPricingGridAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridJCB")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal148" runat="server" Text="Tiered Discount Grid" meta:resourcekey="LiteralResource147" /></td>
                                    <td>
                                        <%# Eval("TieredDiscountGridMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridAmesPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridJCB")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal149" runat="server" Text="ERR %" meta:resourcekey="LiteralResource148" /></td>
                                    <td>
                                        <%# Eval("ERRMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRAmesPassThru")%>
                                    </td>
                                     <td>
                                        <%# Eval("ERRAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERREBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERREBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERREBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRJCB")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal150" runat="server" Text="Other Volume %" meta:resourcekey="LiteralResource149" /></td>
                                    <td>
                                        <%# Eval("OtherVolumeMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeAmesPassThru")%>
                                    </td>
                                     <td>
                                        <%# Eval("OtherVolumeAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeJCB")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal151" runat="server" Text="Other Item Rate" meta:resourcekey="LiteralResource150" /></td>
                                    <td>
                                        <%# Eval("OtherItemRateMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateAmesPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateJCB")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal152" runat="server" Text="Discount Method" meta:resourcekey="LiteralResource151" /></td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescMC") %>'><%# Eval("DiscountMethodMC")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescMCDB") %>'><%# Eval("DiscountMethodMCDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescVS") %>'><%# Eval("DiscountMethodVS")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescVSDB") %>'><%# Eval("DiscountMethodVSDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescDI") %>'><%# Eval("DiscountMethodDI")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescDIDB") %>'><%# Eval("DiscountMethodDIDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescDIPassThru") %>'><%# Eval("DiscountMethodDIPassThru")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescAmexOnePT") %>'><%# Eval("DiscountMethodAmexOnePT")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescAmesPassThru") %>'><%# Eval("DiscountMethodAmesPassThru")%></span>
                                    </td>
                                     <td>
                                        <span title='<%# Eval("DiscountMethodDescAmexBlue") %>'><%# Eval("DiscountMethodAmexBlue")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescWrightExpress") %>'><%# Eval("DiscountMethodWrightExpress")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescVoyager") %>'><%# Eval("DiscountMethodVoyager")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescGenericDebit") %>'><%# Eval("DiscountMethodGenericDebit")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescDinner") %>'><%# Eval("DiscountMethodDinner")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescEBT_CASH_B") %>'><%# Eval("DiscountMethodEBT_CASH_B")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescEBT_F_STMP") %>'><%# Eval("DiscountMethodEBT_F_STMP")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescEBT_TAPE") %>'><%# Eval("DiscountMethodEBT_TAPE")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescJCB") %>'><%# Eval("DiscountMethodJCB")%></span>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal153" runat="server" Text="Amex OnePT Rate" meta:resourcekey="LiteralResource152" /></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("AmexOnePTRateAmexOnePT")%>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal154" runat="server" Text="Amex OnePT Per Item Fee" meta:resourcekey="LiteralResource153" /></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("AmexOnePTPerItemFeeAmexOnePT")%>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>

                    </div>
                </div>
                <!--Merchant Card Information – PIN Debit-->
                <div class="row" id="merchantcardpindebitinfo">
                    <div class="col-xs-12" data-toggle="collapse" data-target="#ciMerchantCardPinDebitInformation">
                        <h2 class="grid-title">
                            <asp:Literal ID="Literal155" runat="server" Text="Merchant Card Information – PIN Debit" meta:resourcekey="LiteralResource154" /></h2>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 in table-responsive" id="ciMerchantCardPinDebitInformation">
                        <table class="ASTable">
                            <tr class="Row">
                                <td class="heading"></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal158" runat="server" Text="ACCEL" meta:resourcekey="LiteralResource157" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal159" runat="server" Text="AFFN" meta:resourcekey="LiteralResource158" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal160" runat="server" Text="ALASKA OPTION" meta:resourcekey="LiteralResource159" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal161" runat="server" Text="CU24" meta:resourcekey="LiteralResource160" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal162" runat="server" Text="INTERLINK" meta:resourcekey="LiteralResource161" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal163" runat="server" Text="JEANIE" meta:resourcekey="LiteralResource162" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal164" runat="server" Text="MAESTRO" meta:resourcekey="LiteralResource163" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal165" runat="server" Text="NYCE" meta:resourcekey="LiteralResource164" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal166" runat="server" Text="PULSE" meta:resourcekey="LiteralResource165" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal167" runat="server" Text="SHAZAM" meta:resourcekey="LiteralResource166" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal168" runat="server" Text="STAR" meta:resourcekey="LiteralResource167" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal169" runat="server" Text="STAR" meta:resourcekey="LiteralResource168" /></td>
                                <td class="heading">
                                    <asp:Literal ID="Literal170" runat="server" Text="STAR" meta:resourcekey="LiteralResource169" /></td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading">
                                    <asp:Literal ID="Literal171" runat="server" Text="Other Volume %" meta:resourcekey="LiteralResource170" /></td>
                                <td>
                                    <%# Eval("OtherVolumeACCEL")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumeAFFN")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumeAlaskaOption")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumeCU24")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumeInterlink")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumeJeanie")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumeMaestro")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumeNYCE")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumePULSE")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumeShazam")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumeStar13")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumeStar18")%>
                                </td>
                                <td>
                                    <%# Eval("OtherVolumeStar21")%>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading">
                                    <asp:Literal ID="Literal172" runat="server" Text="Other Item Rate" meta:resourcekey="LiteralResource171" /></td>
                                <td>
                                    <%# Eval("OtherItemRateACCEL")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRateAFFN")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRateAlaskaOption")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRateCU24")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRateInterlink")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRateJeanie")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRateMaestro")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRateNYCE")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRatePULSE")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRateShazam")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRateStar13")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRateStar18")%>
                                </td>
                                <td>
                                    <%# Eval("OtherItemRateStar21")%>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading">
                                    <asp:Literal ID="Literal173" runat="server" Text="Online Debit Fee Flag" meta:resourcekey="LiteralResource172" /></td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescACCEL") %>'><%# Eval("OnlDbtFeeFlagACCEL")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescAFFN") %>'><%# Eval("OnlDbtFeeFlagAFFN")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescAlaskaOption") %>'><%# Eval("OnlDbtFeeFlagAlaskaOpt")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescCU24") %>'><%# Eval("OnlDbtFeeFlagCU24")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescInterlink") %>'><%# Eval("OnlDbtFeeFlagInterlink")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescJeanie") %>'><%# Eval("OnlDbtFeeFlagJeanie")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescMaestro") %>'><%# Eval("OnlDbtFeeFlagMaestro")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescNYCE") %>'><%# Eval("OnlDbtFeeFlagNYCE")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescPULSE") %>'><%# Eval("OnlDbtFeeFlagPULSE")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescShazam") %>'><%# Eval("OnlDbtFeeFlagShazam")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescStar13") %>'><%# Eval("OnlDbtFeeFlagStar13")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescStar18") %>'><%# Eval("OnlDbtFeeFlagStar18")%></span>
                                </td>
                                <td>
                                    <span title='<%# Eval("OnlDbtFeeFlagDescStar21") %>'><%# Eval("OnlDbtFeeFlagStar21")%></span>
                                </td>
                            </tr>

                        </table>
                    </div>
                </div>
            </as:PlaceHolder>

        </ItemTemplate>
    </asp:Repeater>
</asp:PlaceHolder>

<as:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var MIF_MerchantDetails_FDR_uxtxtRelationShipManager = '#<%= uxtxtRelationShipManager.ClientID %>';
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MIF_MerchantDetails_FDR.js"></script>
</as:RadCodeBlock>
