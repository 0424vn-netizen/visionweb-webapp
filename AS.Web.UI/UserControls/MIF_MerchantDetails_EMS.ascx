<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MerchantDetails_EMS.ascx.cs"
    Inherits="UserControls_MIF_MerchantDetails_EMS" %>
<%@ Register TagPrefix="uc" TagName="RiskInfo" Src="~/UserControls/MIF_RiskInformationSection.ascx" %>

<asp:PlaceHolder ID="phdMerchantDetail" runat="server">
    <div class="row" id="merchinfo">
        <div class="col-md-9" data-toggle="collapse" data-target="#ciMerchantInformation">
            <h2 class="grid-title mt-3x"><asp:Literal ID="Literal1" runat="server" Text="Merchant Information" meta:resourcekey="Resource1" />
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
                    <td class="heading valign-middle"><asp:Literal ID="Literal2" runat="server" Text="Merchant ID:" meta:resourcekey="LiteralResource1" />
                    </td>
                    <td>
                        <%= BindValue("MerchantNumber")%>
                    </td>
                    <td class="heading valign-middle"><asp:Literal ID="Literal3" runat="server" Text="Contact:" meta:resourcekey="LiteralResource2"/>
                    </td>
                    <td>
                        <%= BindValue("Contact")%>
                    </td>
                    <td class="heading valign-middle"><asp:Literal ID="Literal4" runat="server" Text="Status:" meta:resourcekey="LiteralResource3"/>
                    </td>
                    <td>
                        <%= BindValue("Status")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-middle"><asp:Literal ID="Literal5" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource4"/>
                    </td>
                    <td>
                        <%= BindValue("MerchantName")%>
                    </td>
                    <td class="heading valign-middle"><asp:Literal ID="Literal6" runat="server" Text="Phone:" meta:resourcekey="LiteralResource5"/>
                    </td>
                    <td>
                        <%= FormatPhone(BindValue("Phone"))%>
                    </td>
                    <td class="heading valign-middle"><asp:Literal ID="Literal7" runat="server" Text="Last Batch Activity:" meta:resourcekey="LiteralResource6"/>
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkLastBatch" Text=""
                            OnClick="lnkLastBatch_Click" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-middle"><asp:Literal ID="Literal8" runat="server" Text="Address:" meta:resourcekey="LiteralResource7"/>
                    </td>
                    <td rowspan="2">
                        <%= BindAddress(BindValue("Address1"), BindValue("Address2"), BindValue("Address3"), BindValue("City"), BindValue("State"), BindValue("Zip"))%>
                    </td>
                    <td class="heading valign-middle"><asp:Literal ID="Literal9" runat="server" Text="Email:" meta:resourcekey="LiteralResource8"/>
                    </td>
                    <td colspan="3">
                        <%= BindValue("Email")%>
                    </td>
                </tr>
                <tr class="Row">
                </tr>
                <as:Panel runat="server" ID="uxpnlRMCtrls">
                    <as:Literal runat="server" ID="uxlbRelationshipManager"></as:Literal>
                    <as:LinkButton ID="uxbtnAdd" OnClick="uxbtnAdd_click" Text="Add RM" runat="server" meta:resourcekey="uxbtnAddResource1" />
                    <as:LinkButton ID="uxbtnEdit" OnClick="uxbtnEdit_click" runat="server" Text="Edit" meta:resourcekey="uxbtnEditResource1"/>
                    <as:Panel ID="uxpnlSaveCancel" runat="server" Visible="false">
                        <as:TextBox runat="server" ID="uxtxtRelationShipManager" MaxLength="30" />
                        <as:LinkButton ID="uxbtnSave" OnClick="uxbtnSave_click" OnClientClick="return CheckPrefix();" runat="server" Text="Save" meta:resourcekey="uxbtnSaveResource1"/>
                        /
                    <as:LinkButton ID="uxbtnCancel" OnClick="uxbtnCancel_click" runat="server" Text="Cancel" meta:resourcekey="uxbtnCancelResource1"/>
                    </as:Panel>
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

    <asp:Repeater ID="rptMerchantInfo" runat="server" EnableViewState="true" OnItemDataBound="rptMerchantInfo_ItemDataBound">
        <ItemTemplate>
            <div class="row">
                <div class="col-md-6">
                    <asp:PlaceHolder ID="phdBusinessInfo" runat="server">
                        <div class="row" id="businessinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciBusinessInformation">
                                <h2 class="grid-title"><asp:Literal ID="Literal10" runat="server" Text="Business Information" meta:resourcekey="LiteralResource9" /></h2>
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
                                        <td class="heading"><asp:Literal ID="Literal13" runat="server" Text="Status:" meta:resourcekey="LiteralResource12"/>
                                        </td>
                                        <td>
                                            <%# Eval("Status")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal14" runat="server" Text="Date:" meta:resourcekey="LiteralResource13"/>
                                        </td>
                                        <td>
                                            <%# FormatDate(Eval("OpenDate"))%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading"><asp:Literal ID="Literal15" runat="server" Text="Federal Tax ID:" meta:resourcekey="LiteralResource14"/>
                                        </td>
                                        <td>
                                            <%# CheckPermisson(Eval("TaxID"), WebSiteConstants.SEC_PERMISSION_TAX_ID)%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading valign-middle"><asp:Literal ID="Literal16" runat="server" Text="Site Access:" meta:resourcekey="LiteralResource15"/>
                                        </td>
                                        <td>
                                            <div class="<%# HasMSProductEnvironment()? "mt-1x" :"" %>">
                                                <asp:Literal ID="ltrSiteAccess"  runat="server" Text='<%# HasMSProductEnvironment()?Eval("SiteAccess"):"Not Available"%>' />
                                            </div>
                                            <div class="pull-right opt-in-out-button">
                                                <as:PlaceHolder runat="server" ID="PlaceHolder1" Visible='<%# !IsCaseManagement %>'>
                                                    <as:Button ID="uxSiteAccess" runat="server" Text='<%# SetStatusText(Eval("SiteAccess"))%>'
                                                        Visible='<%# SetVisible(Eval("SiteAccess"))%>' OnClientClick='<%# SetURLForSiteAccessButton()%>' />
                                                </as:PlaceHolder>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading"><asp:Literal ID="Literal17" runat="server" Text="Average Sales Amount:" meta:resourcekey="LiteralResource16"/>
                                        </td>
                                        <td>
                                            <%# Eval("AVGSalesAmount")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal18" runat="server" Text="Customer Type:" meta:resourcekey="LiteralResource17"/>
                                        </td>
                                        <td>
                                            <%# Eval("CustomerType")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading"><asp:Literal ID="Literal19" runat="server" Text="Lease Number:" meta:resourcekey="LiteralResource18"/>
                                        </td>
                                        <td>
                                            <%# Eval("LeaseNumber")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal20" runat="server" Text="Lessor:" meta:resourcekey="LiteralResource19"/>
                                        </td>
                                        <td>
                                            <%# Eval("Lessor")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading"><asp:Literal ID="Literal21" runat="server" Text="Gift Card Customer Number:" meta:resourcekey="LiteralResource20"/>
                                        </td>
                                        <td>
                                            <%# Eval("GiftCardCustomerNumber")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal22" runat="server" Text="Gold (CDP):" meta:resourcekey="LiteralResource21"/>
                                        </td>
                                        <td>
                                            <%# Eval("Gold")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading"><asp:Literal ID="Literal23" runat="server" Text="Social Security #:" meta:resourcekey="LiteralResource22"/>
                                        </td>
                                        <td>
                                            <%# Eval("SocialSecurityNumber")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal24" runat="server" Text="Risk Plan:" meta:resourcekey="LiteralResource23"/>
                                        </td>
                                        <td>
                                            <%# Eval("RiskPlan")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading"><asp:Literal ID="Literal25" runat="server" Text="Lease Date:" meta:resourcekey="LiteralResource24"/>
                                        </td>
                                        <td>
                                            <%# Eval("LeaseDate")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal26" runat="server" Text="Remaining Payment:" meta:resourcekey="LiteralResource25"/>
                                        </td>
                                        <td>
                                            <%# Eval("RemainingPayment")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading"><asp:Literal ID="Literal27" runat="server" Text="On Hold:" meta:resourcekey="LiteralResource26"/>
                                        </td>
                                        <td>
                                            <%# Eval("OnHold")%>
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
                                <h2 class="grid-title"><asp:Literal ID="Literal28" runat="server" Text="Hierarchy Information" meta:resourcekey="LiteralResource27"/></h2>
                            </div>
                            </div><div class="row">
                            <div class="col-md-12 in" id="ciHierarchyInformation">
                                <table class="ASTable">
                                    <colgroup>
                                        <col class="w-20" />
                                        <col class="w-30" />
                                        <col class="w-20" />
                                        <col class="w-30" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading"><asp:Literal ID="Literal31" runat="server" Text="Client Name:" meta:resourcekey="LiteralResource30"/>
                                        </td>
                                        <td colspan="3">
                                            <%# Eval("ClientName")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal32" runat="server" Text="Client Login:" meta:resourcekey="LiteralResource31"/>
                                        </td>
                                        <td>
                                            <%# Eval("ClientLogin")%>
                                        </td>
                                        <td class="heading"><asp:Literal ID="Literal33" runat="server" Text="ISO:" meta:resourcekey="LiteralResource32"/>
                                        </td>
                                        <td>
                                            <%# Eval("ISO")%>
                                        </td>
                                    </tr>
                                    <tr class="Row">
                                        <td class="heading"><asp:Literal ID="Literal34" runat="server" Text="Sales Office:" meta:resourcekey="LiteralResource33"/>
                                        </td>
                                        <td>
                                            <%# Eval("SalesOffice")%>
                                        </td>
                                        <td class="heading"><asp:Literal ID="Literal35" runat="server" Text="Bank Number:" meta:resourcekey="LiteralResource34"/>
                                        </td>
                                        <td>
                                            <%# Eval("BankNumber")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal36" runat="server" Text="Merchant Type:" meta:resourcekey="LiteralResource35"/>
                                        </td>
                                        <td colspan="3">
                                            <%# Eval("MerchantType")%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phdBankInformation" runat="server">
                        <div class="row" id="bankinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciBankInformation">
                                <h2 class="grid-title"><asp:Literal ID="Literal37" runat="server" Text="Bank Information" meta:resourcekey="LiteralResource36"/></h2>
                            </div>
                            </div><div class="row">
                            <div class="col-md-12 in" id="ciBankInformation">
                                <table class="ASTable">
                                    <colgroup>
                                        <col class="w-40" />
                                        <col class="w-60" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading"><asp:Literal ID="Literal40" runat="server" Text="Transit / Routing #:" meta:resourcekey="LiteralResource39"/>
                                        </td>
                                        <td>
                                            <%# Eval("RoutingNumber") %>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal41" runat="server" Text="Acct #:" meta:resourcekey="LiteralResource40"/>
                                        </td>
                                        <td>
                                            <%#CheckPermisson(Eval("AccountNumber"), WebSiteConstants.SEC_PERMISSION_DDA)%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phdTerminalInformation" runat="server">
                        <div class="row" id="terminalinfo">
                            <div class="col-xs-12" data-toggle="collapse" data-target="#ciTerminalInformation">
                                <h2 class="grid-title"><asp:Literal ID="Literal42" runat="server" Text="Terminal Information" meta:resourcekey="LiteralResource41"/></h2>
                            </div>
                            </div><div class="row">
                            <div class="col-md-12 in" id="ciTerminalInformation">
                                <table class="ASTable">
                                    <colgroup>
                                        <col class="w-40" />
                                        <col class="w-60" />
                                    </colgroup>
                                    <tr class="Row">
                                        <td class="heading"><asp:Literal ID="Literal45" runat="server" Text="Terminal:" meta:resourcekey="LiteralResource44"/>
                                        </td>
                                        <td>
                                            <%# Eval("Terminal")%>
                                        </td>
                                    </tr>
                                    <tr class="AltRow">
                                        <td class="heading"><asp:Literal ID="Literal46" runat="server" Text="Other Card Types:" meta:resourcekey="LiteralResource45"/>
                                        </td>
                                        <td>
                                            <as:Literal ID="uxOtherCardType" runat="server"></as:Literal></td></tr></table></div></div></asp:PlaceHolder></div></div></ItemTemplate></asp:Repeater><as:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var MIF_MerchantDetails_FIS_uxtxtRelationShipManager = '#<%= uxtxtRelationShipManager.ClientID %>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MIF_MerchantDetails_FIS.js"></script>
    </as:RadCodeBlock>
</asp:PlaceHolder>
