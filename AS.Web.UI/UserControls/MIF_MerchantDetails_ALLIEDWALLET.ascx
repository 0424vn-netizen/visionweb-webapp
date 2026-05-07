<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MerchantDetails_ALLIEDWALLET.ascx.cs" Inherits="UserControls_MIF_MerchantDetails_ALLIEDWALLET" %>
<%@ Register TagPrefix="uc" TagName="RiskInfo" Src="~/UserControls/MIF_RiskInformationSection.ascx" %>

<tek:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <%--<tek:AjaxSetting AjaxControlID="btnSwitchToUpdateStatus">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPanelViewStatus" />
                <tek:AjaxUpdatedControl ControlID="uxPanelUpdateStatus" />
            </UpdatedControls>
        </tek:AjaxSetting>--%>
        <%--<tek:AjaxSetting AjaxControlID="btnCancel">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPanelViewStatus" />
                <tek:AjaxUpdatedControl ControlID="uxPanelUpdateStatus" />
            </UpdatedControls>
        </tek:AjaxSetting>--%>
        <%--<tek:AjaxSetting AjaxControlID="btnUpdateStatus">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPanelViewStatus" />
                <tek:AjaxUpdatedControl ControlID="uxPanelUpdateStatus" />
            </UpdatedControls>
        </tek:AjaxSetting>--%>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
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
                    <col />
                    <col style="width: 135px" />
                    <col style="width: 35%" />
                </colgroup>
                <tr class="Row">
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal2" runat="server" Text="Merchant ID:" meta:resourcekey="LiteralResource2" />
                    </td>
                    <td>
                        <asp:Literal ID="lbMerchantNumber" runat="server" />
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal7" runat="server" Text="Reseller Name:" meta:resourcekey="LiteralResourceResellerName" />
                    </td>
                    <td>
                        <asp:Literal ID="lbResellerName" runat="server" />
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal5" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource5" />
                    </td>
                    <td>
                        <asp:Literal ID="lbMerchantName" runat="server" />
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal6" runat="server" Text="Reseller Phone:" meta:resourcekey="LiteralResourceResellerPhone" />
                    </td>
                    <td>
                        <asp:Literal ID="lbResellerPhone" runat="server" />
                    </td>
                </tr>

                <tr class="Row">
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal3" runat="server" Text="Agent Referral:" meta:resourcekey="LiteralResourceAgentReferral" />
                    </td>
                    <td>
                        <asp:Literal ID="lbAgentReferral" runat="server" />
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal4" runat="server" Text="Reseller Email:" meta:resourcekey="LiteralResourceResellerEmail" />
                    </td>
                    <td>
                        <asp:Literal ID="lbResellerEmail" runat="server" />
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal8" runat="server" Text="Status:" meta:resourcekey="LiteralResource4" />
                    </td>
                    <td>
                        <asp:Panel ID="uxPanelViewStatus" runat="server">
                            <span class="text-left inline-block">
                                <asp:Literal ID="uxCurrentStatus" runat="server" />
                            </span>
                            <%-- <span class="text-right inline-block f-right">
                                <asp:LinkButton ID="btnSwitchToUpdateStatus" runat="server" CssClass="heading link-back fw-normal ml-8x mr-10" OnClick="btnSwitchToUpdateStatus_Click" Text="Update Status" meta:resourcekey="LiteralResourceUpdateStatus" />
                            </span>--%>
                        </asp:Panel>
                        <%-- <asp:Panel ID="uxPanelUpdateStatus" runat="server" Visible="false">
                            <span class="text-left inline-block">
                                <tek:RadComboBox ID="uxcbbStatus" runat="server" Width="250px" />
                            </span>
                            <span class="text-right inline-block f-right">
                                <asp:LinkButton ID="btnUpdateStatus" runat="server" CssClass="heading link-back fw-normal" Text="Save" OnClick="btnUpdateStatus_Click" meta:resourcekey="LiteralResourceSave" />
                                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="heading link-back fw-normal ml-20 mr-10" OnClick="btnCancel_Click" meta:resourcekey="LiteralResourceCancel" />
                            </span>
                        </asp:Panel>--%>
                    </td>
                    <td class="heading valign-middle">
                        <asp:Literal ID="Literal20" runat="server" Text="Last Batch Activity:" meta:resourcekey="lnkLastBatchResource" />
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkLastBatch" Text="" OnClick="lnkLastBatch_Click"  Visible="false" />
                        <asp:Literal ID="lbLastBatchEmdash" runat="server" Text="—" Visible="true" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <asp:PlaceHolder ID="rptMerchantInfo" runat="server">

        <div class="row">
            <div class="col-md-12">
                <asp:PlaceHolder ID="phdBusinessInfo" runat="server">
                    <div class="row" id="businessinfo">
                        <div class="col-xs-12" data-toggle="collapse" data-target="#ciBusinessInformation">
                            <h2 class="grid-title">
                                <asp:Literal ID="Literal10" runat="server" Text="Business Information" meta:resourcekey="LiteralResource10" /></h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 in" id="ciBusinessInformation">
                            <table class="ASTable">
                                <colgroup>
                                    <col style="width: 220px" />
                                    <col />
                                    <col style="width: 220px" />
                                    <col style="width: 28%" />
                                </colgroup>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal16" runat="server" Text="Open Date:" meta:resourcekey="LiteralResource16" /></td>
                                    <td>
                                        <%= FormatDate(BindValueAsObject("OpenDate"))%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal9" runat="server" Text="SIC/MCC:" meta:resourcekey="LiteralResourceSICMCC" /></td>
                                    <td>
                                        <%=  string.Format("{0} - {1}", BindValue("SICMCC"), BindValue("SICDescription"))%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal11" runat="server" Text="Merchant Corporate/Legal Name:" meta:resourcekey="LiteralResourceMerchantCorporateLegalName" /></td>
                                    <td>
                                        <%= BindValue("CompanyName")%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal12" runat="server" Text="Business Type:" meta:resourcekey="LiteralResourceBusinessType" /></td>
                                    <td>
                                        <%= BindValue("BusinessTypeNotes")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal13" runat="server" Text="Country:" meta:resourcekey="LiteralResourceCountry" /></td>
                                    <td>
                                        <%= BindValue("CountryName")%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal14" runat="server" Text="Business Registration #:" meta:resourcekey="LiteralResourceBusinessRegistration" /></td>
                                    <td>
                                        <%= BindValue("BusinessRegistration")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal15" runat="server" Text="Business Address:" meta:resourcekey="LiteralResourceBusinessAddress" /></td>
                                    <td>
                                        <%= BindAddress(BindValueNoEMDash("Line1_Business"), BindValueNoEMDash("Line2_Business"), BindValueNoEMDash("Address3"), BindValueNoEMDash("City"), BindValueNoEMDash("State"), BindValueNoEMDash("Zip"))%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal18" runat="server" Text="Product Sold:" meta:resourcekey="LiteralResourceProductSold" /></td>
                                    <td>
                                        <%= BindValue("ProductSold")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal19" runat="server" Text="Business Email:" meta:resourcekey="LiteralResourceBusinessEmail" /></td>
                                    <td>
                                        <%= BindValue("BusinessEmail")%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal17" runat="server" Text="Closed Date:" meta:resourcekey="LiteralResource1ClosedDate" /></td>
                                    <td>
                                        <%= FormatDate(BindValueAsObject("ClosedDate"))%>
                                    </td>
                                </tr>
                                <asp:Repeater ID="uxWebSites" runat="server" OnItemDataBound="uxWebSites_ItemDataBound">
                                    <ItemTemplate>
                                        <tr id="uxwebSiteRow" class="Row" runat="server">
                                            <td class="heading">
                                                <asp:Literal ID="lbBusinessWebsite" runat="server" Text="Business Websites:" meta:resourcekey="BusinessWebsites" /></td>
                                            <td>
                                                <as:Literal ID="uxBusinessWebsite" runat="server"></as:Literal><br />
                                                <asp:LinkButton ID="uxLinkButtonViewWebSites" runat="server" CssClass="heading link-back fw-normal"
                                                    OnClientClick="ShowPopupModal('WebsiteModal.aspx','auto'); return false;" Text="View All" meta:resourcekey="MIF_ViewWebsiteModal" />

                                            </td>
                                            <td class="heading">
                                                <asp:Literal ID="uxlbRiskLevel" runat="server" Text="Risk Level:" meta:resourcekey="MIF_RiskLevel" />
                                            </td>
                                            <td class="riskLevel">
                                                <asp:Literal ID="uxRiskLevel" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
    </asp:PlaceHolder>
    <!--Risk Info-->
    <div class="row">
        <div class="col-md-12">
            <uc:RiskInfo ID="uxRiskInfo" runat="server" />
        </div>
    </div>
</asp:PlaceHolder>
