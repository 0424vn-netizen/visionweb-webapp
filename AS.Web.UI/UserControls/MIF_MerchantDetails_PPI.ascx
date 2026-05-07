<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MerchantDetails_PPI.ascx.cs"
    Inherits="UserControls_MIF_MerchantDetails_PPI" %>
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
                    <td class="heading">
                        <asp:Literal ID="Literal2" runat="server" Text="Merchant ID:" meta:resourcekey="LiteralResource2" />
                    </td>
                    <td>
                        <%= BindValue("MerchantNumber")%>
                    </td>
                    <td class="heading">
                        <asp:Literal ID="Literal3" runat="server" Text="Contact:" meta:resourcekey="LiteralResource3" />
                    </td>
                    <td>
                        <%= BindValue("Contact")%>
                    </td>
                    <td class="heading">
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
                        <asp:LinkButton runat="server" ID="lnkLastBatch" Text="" OnClick="lnkLastBatch_Click" meta:resourcekey="lnkLastBatchResource1" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal8" runat="server" Text="Address:" meta:resourcekey="LiteralResource8" />
                    </td>
                    <td>
                        <%= BindAddress(BindValue("Address1"), BindValue("Address2"), BindValue("Address3"), BindValue("City"), BindValue("State"), BindValue("Zip"))%>
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal108" runat="server" Text="Email:" meta:resourcekey="LiteralResource108" />
                    </td>
                    <td colspan="3">
                        <%= BindValue("Email")%>
                    </td>
                </tr>
                <% if (uxpnlRMLabel.Visible)
                   { %>
                <tr class="AltRow">
                    <td class="heading valign-middle">
                        <as:Panel runat="server" ID="uxpnlRMLabel" meta:resourcekey="uxpnlRMLabelResource1">
                            <as:ValidatorLabel ID="ValidatorLabel1" runat="server" Text="Relationship Manager:" ApplyFor="uxtxtRelationShipManager" meta:resourcekey="MIF_MerchantDetail_PPIASCX_ValidatorLabel_RelationshipManager"></as:ValidatorLabel>
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
                                <as:CustomValidationItem ClientValidationFunction="CheckPrefix" ControlToValidateID="uxtxtRelationShipManager" Message="Prefix or suffix (for example Mr./Ms/Sir/Jr.) are not allowed." meta:resourcekey="MIF_MerchantDetail_PPIJS_Text_UnallowPrefixSuffix" />
                                <as:CustomValidationItem ClientValidationFunction="CheckPrefixSpecial" ControlToValidateID="uxtxtRelationShipManager" Message="Relationship Manager field must be 30 characters or less and may not include special characters" meta:resourcekey="MIF_MerchantDetails_PPIJS_Text_AtLeast30NoSpecialChars" />
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
                                <asp:Literal ID="Literal9" runat="server" Text="Business Information" meta:resourcekey="LiteralResource9" /></h2>
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
                                        <asp:Literal ID="Literal12" runat="server" Text="Approval Date:" meta:resourcekey="LiteralResource12" /></td>
                                    <td>
                                        <%#
                                        FormatDate(Eval("ApprovalDate"))%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal13" runat="server" Text="Closed Date:" meta:resourcekey="LiteralResource13" /></td>
                                    <td>
                                        <%# FormatDate(Eval("ClosedDate"))%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal14" runat="server" Text="Status:" meta:resourcekey="LiteralResource14" /></td>
                                    <td>
                                        <%# Eval("ActivityStatus")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading valign-middle">
                                        <asp:Literal ID="Literal15" runat="server" Text="Site Access:" meta:resourcekey="LiteralResource15" /></td>
                                    <td>
                                        <div class="opt-in-out-text <%# HasMSProductEnvironment()? "mt-1x" :"" %>">
                                            <asp:Literal ID="ltrSiteAccess" runat="server" Text='<%# HasMSProductEnvironment()?Eval("SiteAccess"):"Not Available" %>' />
                                        </div>
                                        <div class="pull-right opt-in-out-button">
                                            <as:PlaceHolder runat="server" ID="PlaceHolder1" Visible='<%# !IsCaseManagement %>'>
                                                <as:Button ID="uxSiteAccess" runat="server" Text='<%# SetStatusText(Eval("SiteAccess")) %>' CssClass="btn btn-default"
                                                    Visible='<%# SetVisible(Eval("SiteAccess")) %>' OnClientClick='<%# SetURLForSiteAccessButton() %>' IsStandardButton="False" meta:resourcekey="uxSiteAccessResource1" />
                                            </as:PlaceHolder>
                                        </div>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal16" runat="server" Text="Tax ID:" meta:resourcekey="LiteralResource16" /></td>
                                    <td>
                                        <%#CheckPermisson(Eval("TaxID"),
                                        WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal17" runat="server" Text="SIC/MCC:" meta:resourcekey="LiteralResource17" /></td>
                                    <td>
                                        <%#
                                        FormatSIC(Eval("SICCode"), Eval("SICCodeDesc"))%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal18" runat="server" Text="Average Sales Amount:" meta:resourcekey="LiteralResource18" /></td>
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
                                <h2 class="grid-title">
                                    <asp:Literal ID="Literal19" runat="server" Text="Hierarchy Information" meta:resourcekey="LiteralResource19" /></h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 in" id="ciHierarchyInformation">
                                <table class="ASTable">
                                    <colgroup>
                                        <col class="w-40" />
                                        <col class="w-60" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal22" runat="server" Text="Client Name:" meta:resourcekey="LiteralResource22" /></td>
                                        <td>
                                            <%# Eval("HifBankName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading">
                                            <asp:Literal ID="Literal23" runat="server" Text="Client Login:" meta:resourcekey="LiteralResource23" /></td>
                                        <td>
                                            <%# Eval("ClientLogin")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading">
                                            <asp:Literal ID="Literal24" runat="server" Text="Chain:" meta:resourcekey="LiteralResource24" /></td>
                                        <td>
                                            <as:PlaceHolder ID="uxPlaceChain" runat="server">
                                                <a href="#" onclick="return rf_SubmitReportFilterValues('8910', 'CHAIN', '<%# Eval("Chain") %>');"><%# Eval("Chain")%></a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxPlaceChainNew" runat="server" Visible='<%# CheckPermissionAddEditChain(Eval("SiteAccess")) %>'>
                                                <a href="#" onclick="return ShowPopupModal('CreateNewChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")) %>','auto');">
                                                    <asp:Literal ID="Literal25" runat="server" Text="Create New Chain" meta:resourcekey="LiteralResource25" /></a>
                                            </as:PlaceHolder>
                                            <as:PlaceHolder ID="uxPlaceExistChain" runat="server" Visible='<%# CheckPermissionAddEditChain(Eval("SiteAccess")) %>'>
                                                <asp:Literal ID="Literal10" runat="server" Text="or" meta:resourcekey="LiteralResourceOR"></asp:Literal>  <a href="#" onclick="return ShowPopupModal('ModifyMerchantChainModal.aspx?'+'<%# Page.BuildSecureQueryString("MerchNum="+Eval("MerchantNumber")+"&Chain="+Eval("Chain")) %>','auto');">
                                                    <asp:Literal ID="Literal26" runat="server" Text="Add to Existing Chain" meta:resourcekey="LiteralResource26" /></a>
                                            </as:PlaceHolder>
                                            <as:Literal ID="uxLHChain" runat="server" Text='<%# Eval("Chain") %>'></as:Literal></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div class="row" id="bankinfo">
                        <div class="col-xs-12" data-toggle="collapse" data-target="#ciBankInformation">
                            <h2 class="grid-title">
                                <asp:Literal ID="Literal27" runat="server" Text="Bank Information" meta:resourcekey="LiteralResource27" /></h2>
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
                                        <asp:Literal ID="Literal30" runat="server" Text="Bank Name:" meta:resourcekey="LiteralResource30" /></td>
                                    <td>
                                        <%# Eval("BankName")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal31" runat="server" Text="Routing #:" meta:resourcekey="LiteralResource31" /></td>
                                    <td>
                                        <%# GetRoutingNumber(Eval("RoutingNumber").ToString(), Eval("PartialRoutingNumber").ToString())%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal32" runat="server" Text="DDA #:" meta:resourcekey="LiteralResource32" /></td>
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
            var MIF_MerchantDetails_PPI_uxtxtRelationShipManager = '#<%= uxtxtRelationShipManager.ClientID %>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MIF_MerchantDetails_PPI.js"></script>
    </as:RadCodeBlock>
</asp:PlaceHolder>

