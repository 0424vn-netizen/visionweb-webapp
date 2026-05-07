<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddNewMerchant.aspx.cs" Inherits="AddNewMerchant" MasterPageFile="~/MasterPage.master" meta:resourcekey="PageResource1" %>

<asp:Content ID="uxContentpage" ContentPlaceHolderID="ContentPage" runat="server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxCountry">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxStateProvince" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxRemoveBusinessWeb">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="pnBusinessWebsite" LoadingPanelID="uxInvisiblePanel" />
                    <tek:AjaxUpdatedControl ControlID="uxAddBWLink" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxAddBusinessWeb">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="pnBusinessWebsite" LoadingPanelID="uxInvisiblePanel" />
                    <tek:AjaxUpdatedControl ControlID="uxAddBWLink" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <div class="row">
        <div class="col-md-10">
            <h1 class="report-title-no-filter cursor-default">
                <asp:Literal ID="lblReportTitle" runat="server" Text="Add New Merchant" meta:resourcekey="lblReportTitleResource1"></asp:Literal>
            </h1>
        </div>
        <div class="col-md-7" id="add-merchant">
            <h2 class="grid-title on-top cursor-default">Merchant Information</h2>
            <table class="ASTable form-inline" id="tblMerchantInfo">
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltMerchantNum" ApplyFor="uxMerchantNum" runat="server" Text="Merchant ID:" meta:resourcekey="ltMerchantNumResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxMerchantNum" numeric-only="true" start-with-zero="false" CssClass="form-control" MaxLength="5" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                        <as:ValidatorMessage ID="msgMerchantNum" ApplyFor="uxMerchantNum" runat="server"></as:ValidatorMessage>
                        <label style="" class="error" id="lbMsgMerchantNum"></label>
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltBDAName" ApplyFor="uxDBAName" runat="server" Text="DBA Name:" meta:resourcekey="ltBDANameResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxDBAName" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltAgentReferral" ApplyFor="uxAgentReferral" runat="server" Text="Agent Referral:" meta:resourcekey="ltAgentReferralResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxAgentReferral" MaxLength="50" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltResellerName" ApplyFor="uxResellerName" runat="server" Text="Reseller Name:" meta:resourcekey="ltResellerNameResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxResellerName" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltResellerPhone" ApplyFor="uxResellerPhone" runat="server" Text="Reseller Phone:" meta:resourcekey="ltResellerNameResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxResellerPhone" MaxLength="14" display-masked='Phone' CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltResellerEmail" ApplyFor="uxResellerEmail" runat="server" Text="Reseller Email:" meta:resourcekey="ltResellerEmailResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxResellerEmail" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
            </table>

            <h2 class="grid-title cursor-default">Business Information</h2>
            <table class="ASTable form-inline" id="tblBusinessInfo">
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltMerchantCorporate" ApplyFor="uxMerchantCorporate" runat="server" Text="Merchant Corporate/Legal Name:" meta:resourcekey="ltMerchantCorporateResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxMerchantCorporate" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltCountry" ApplyFor="uxCountry" runat="server" Text="Country:" meta:resourcekey="ltCountryResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:RadComboBox ID="uxCountry" runat="server" Height="200px" Width="80%" Filter="Contains" OnSelectedIndexChanged="uxCountry_SelectedIndexChanged" AutoPostBack="true"
                            OnClientKeyPressing="uxCountry_OnClientKeyPressing" DataTextField="CountryName" DataValueField="CountryCode" meta:resourcekey="uxResource1" />
                        <as:ValidatorMessage ApplyFor="uxCountry" runat="server"></as:ValidatorMessage>
                    </td>

                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltBusinessAddLine1" ApplyFor="uxBusinessAddLine1" runat="server" Text="Business Address Line 1:" meta:resourcekey="ltBusinessAddLine1Resource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxBusinessAddLine1" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltBusinessAddLine2" ApplyFor="uxBusinessAddLine2" runat="server" Text="Business Address Line 2:" meta:resourcekey="ltBusinessAddLine2Resource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxBusinessAddLine2" MaxLength="50" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltCity" ApplyFor="uxCity" runat="server" Text="City:" meta:resourcekey="ltCityResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxCity" MaxLength="50" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltStateProvince" ApplyFor="uxStateProvince" runat="server" Text="State/Province:" meta:resourcekey="ltStateProvinceResource1"></as:ValidatorLabel>
                    </td>
                    <td>

                        <as:RadComboBox ID="uxStateProvince" runat="server" Filter="Contains" Height="200px" Width="80%" Enabled="false"
                            OnClientKeyPressing="uxStateProvince_OnClientKeyPressing" DataTextField="Description" DataValueField="State" meta:resourcekey="uxResource1" />
                        <as:ValidatorMessage ID="msgStateProvice" ApplyFor="uxStateProvince" runat="server"></as:ValidatorMessage>
                        <label style="" class="error" id="mgsErrorState"></label>

                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltZip" runat="server" ApplyFor="uxZip" Text="Zip/Postal Code:" meta:resourcekey="ltZipResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxZip" MaxLength="9" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltBusinessEmail" ApplyFor="uxBusinessEmail" runat="server" Text="Business Email:" meta:resourcekey="ltBusinessEmailResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxBusinessEmail" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltBusinessWebsite" ApplyFor="uxBusinessWebsite" runat="server" Text="Business Website:" meta:resourcekey="ltBusinessWebsiteResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxBusinessWebsite" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                        <as:ValidatorMessage ID="msgBusinessWebsite" ApplyFor="uxBusinessWebsite" runat="server"></as:ValidatorMessage>
                    </td>
                </tr>
            </table>
            <as:Panel ID="pnBusinessWebsite" runat="server">
                <table class="ASTable form-inline" id="tblBusinessWebsite">
                    <%--Business Website--%>
                    <tr class="b-website hide" runat="server" id="trBusinessWebsite2" data-id="trBusinessWebsite2" data-bw="uxBusinessWebsite2">
                        <td class="heading w-30">
                            <as:ValidatorLabel ID="ltBusinessWebsite2" ApplyFor="uxBusinessWebsite2" runat="server" Text="Business Website 2:" meta:resourcekey="ltBusinessWebsite2Resource1"></as:ValidatorLabel>
                        </td>
                        <td>
                            <as:TextBox ID="uxBusinessWebsite2" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                            <as:ValidatorMessage ID="msgBusinessWebsite2" ApplyFor="uxBusinessWebsite2" runat="server"></as:ValidatorMessage>
                            <as:LinkButton ID="LinkButton3" runat="server" CssClass="heading link-back bw-remove" Text="Remove" OnClientClick="return removeBusinessWebsite(this);" meta:resourcekey="uxRemoveLink"></as:LinkButton>
                        </td>
                    </tr>
                    <tr class="b-website hide" runat="server" id="trBusinessWebsite3" data-id="trBusinessWebsite3" data-bw="uxBusinessWebsite3">
                        <td class="heading w-30">
                            <as:ValidatorLabel ID="ltBusinessWebsite3" ApplyFor="uxBusinessWebsite3" runat="server" Text="Business Website 3:" meta:resourcekey="ltBusinessWebsite3Resource1"></as:ValidatorLabel>
                        </td>
                        <td>
                            <as:TextBox ID="uxBusinessWebsite3" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                            <as:ValidatorMessage ID="msgBusinessWebsite3" ApplyFor="uxBusinessWebsite3" runat="server"></as:ValidatorMessage>
                            <as:LinkButton ID="LinkButton4" runat="server" CssClass="heading link-back bw-remove" Text="Remove" OnClientClick="return removeBusinessWebsite(this);" meta:resourcekey="uxRemoveLink"></as:LinkButton>
                        </td>
                    </tr>
                    <tr class="b-website hide" runat="server" id="trBusinessWebsite4" data-id="trBusinessWebsite4" data-bw="uxBusinessWebsite4">
                        <td class="heading w-30">
                            <as:ValidatorLabel ID="ltBusinessWebsite4" ApplyFor="uxBusinessWebsite4" runat="server" Text="Business Website 4:" meta:resourcekey="ltBusinessWebsite4Resource1"></as:ValidatorLabel>
                        </td>
                        <td>
                            <as:TextBox ID="uxBusinessWebsite4" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                            <as:ValidatorMessage ID="msgBusinessWebsite4" ApplyFor="uxBusinessWebsite4" runat="server"></as:ValidatorMessage>
                            <as:LinkButton ID="LinkButton5" runat="server" CssClass="heading link-back bw-remove" Text="Remove" OnClientClick="return removeBusinessWebsite(this);" meta:resourcekey="uxRemoveLink"></as:LinkButton>
                        </td>
                    </tr>
                    <tr class="b-website hide" runat="server" data-bw="uxBusinessWebsite5" id="trBusinessWebsite5" data-id="trBusinessWebsite5">
                        <td class="heading w-30">
                            <as:ValidatorLabel ID="ltBusinessWebsite5" ApplyFor="uxBusinessWebsite5" runat="server" Text="Business Website 5:" meta:resourcekey="ltBusinessWebsite5Resource1"></as:ValidatorLabel>
                        </td>
                        <td>
                            <as:TextBox ID="uxBusinessWebsite5" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                            <as:ValidatorMessage ID="msgBusinessWebsite5" ApplyFor="uxBusinessWebsite5" runat="server"></as:ValidatorMessage>
                            <as:LinkButton ID="LinkButton6" runat="server" CssClass="heading link-back bw-remove" Text="Remove" OnClientClick="return removeBusinessWebsite(this);" meta:resourcekey="uxRemoveLink"></as:LinkButton>
                        </td>
                    </tr>
                    <tr class="b-website hide" runat="server" data-bw="uxBusinessWebsite6" id="trBusinessWebsite6" data-id="trBusinessWebsite6">
                        <td class="heading w-30">
                            <as:ValidatorLabel ID="ltBusinessWebsite6" ApplyFor="uxBusinessWebsite6" runat="server" Text="Business Website 6:" meta:resourcekey="ltBusinessWebsite6Resource1"></as:ValidatorLabel>
                        </td>
                        <td>
                            <as:TextBox ID="uxBusinessWebsite6" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                            <as:ValidatorMessage ID="msgBusinessWebsite6" ApplyFor="uxBusinessWebsite6" runat="server"></as:ValidatorMessage>
                            <as:LinkButton ID="LinkButton7" runat="server" CssClass="heading link-back bw-remove" Text="Remove" OnClientClick="return removeBusinessWebsite(this);" meta:resourcekey="uxRemoveLink"></as:LinkButton>
                        </td>
                    </tr>
                    <tr class="b-website hide" runat="server" data-bw="uxBusinessWebsite7" id="trBusinessWebsite7" data-id="trBusinessWebsite7">
                        <td class="heading w-30">
                            <as:ValidatorLabel ID="ltBusinessWebsite7" ApplyFor="uxBusinessWebsite7" runat="server" Text="Business Website 7:" meta:resourcekey="ltBusinessWebsite7Resource1"></as:ValidatorLabel>
                        </td>
                        <td>
                            <as:TextBox ID="uxBusinessWebsite7" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                            <as:ValidatorMessage ID="msgBusinessWebsite7" ApplyFor="uxBusinessWebsite7" runat="server"></as:ValidatorMessage>
                            <as:LinkButton ID="LinkButton8" runat="server" CssClass="heading link-back bw-remove" Text="Remove" OnClientClick="return removeBusinessWebsite(this);" meta:resourcekey="uxRemoveLink"></as:LinkButton>
                        </td>
                    </tr>
                    <tr class="b-website hide" runat="server" data-bw="uxBusinessWebsite8" id="trBusinessWebsite8" data-id="trBusinessWebsite8">
                        <td class="heading w-30">
                            <as:ValidatorLabel ID="ltBusinessWebsite8" ApplyFor="uxBusinessWebsite8" runat="server" Text="Business Website 8:" meta:resourcekey="ltBusinessWebsite8Resource1"></as:ValidatorLabel>
                        </td>
                        <td>
                            <as:TextBox ID="uxBusinessWebsite8" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                            <as:ValidatorMessage ID="msgBusinessWebsite8" ApplyFor="uxBusinessWebsite8" runat="server"></as:ValidatorMessage>
                            <as:LinkButton ID="LinkButton9" runat="server" CssClass="heading link-back bw-remove" Text="Remove" OnClientClick="return removeBusinessWebsite(this);" meta:resourcekey="uxRemoveLink"></as:LinkButton>
                        </td>
                    </tr>
                    <tr class="b-website hide" runat="server" data-bw="uxBusinessWebsite9" id="trBusinessWebsite9" data-id="trBusinessWebsite9">
                        <td class="heading w-30">
                            <as:ValidatorLabel ID="ltBusinessWebsite9" ApplyFor="uxBusinessWebsite9" runat="server" Text="Business Website 9:" meta:resourcekey="ltBusinessWebsite9Resource1"></as:ValidatorLabel>
                        </td>
                        <td>
                            <as:TextBox ID="uxBusinessWebsite9" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                            <as:ValidatorMessage ID="msgBusinessWebsite9" ApplyFor="uxBusinessWebsite9" runat="server"></as:ValidatorMessage>
                            <as:LinkButton ID="LinkButton10" runat="server" Text="Remove" CssClass="heading link-back bw-remove" OnClientClick="return removeBusinessWebsite(this);" meta:resourcekey="uxRemoveLink"></as:LinkButton>
                        </td>
                    </tr>
                    <tr class="b-website hide" runat="server" data-bw="uxBusinessWebsite10" id="trBusinessWebsite10" data-id="trBusinessWebsite10">
                        <td class="heading w-30">
                            <as:ValidatorLabel ID="ltBusinessWebsite10" ApplyFor="uxBusinessWebsite10" runat="server" Text="Business Website 10:" meta:resourcekey="ltBusinessWebsite10Resource1"></as:ValidatorLabel>
                        </td>
                        <td>
                            <as:TextBox ID="uxBusinessWebsite10" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                            <as:ValidatorMessage ID="msgBusinessWebsite10" ApplyFor="uxBusinessWebsite10" runat="server"></as:ValidatorMessage>
                            <as:LinkButton ID="LinkButton11" runat="server" Text="Remove" CssClass="heading link-back bw-remove" OnClientClick="return removeBusinessWebsite(this);" meta:resourcekey="uxRemoveLink"></as:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="heading w-30"></td>
                        <td>
                            <as:LinkButton ID="uxAddBWLink" runat="server" Text="+ Additional Website" CssClass="heading link-back fw-normal" OnClientClick="return addBusinessWebsite(this);" meta:resourcekey="uxAddLink"></as:LinkButton>
                        </td>
                    </tr>
                </table>
            </as:Panel>
            <%--End Business Website--%>
            <table class="ASTable form-inline" id="tblBusinessFooter">
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltSicMcc" ApplyFor="uxSicMcc" runat="server" Text="SIC/MCC:" meta:resourcekey="ltSicMccResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:RadComboBox ID="uxSicMcc" runat="server" Filter="Contains" Height="200px" Width="80%" DataTextField="Description" DataValueField="SICCode" 
                            OnClientKeyPressing="uxSicMcc_OnClientKeyPressing" meta:resourcekey="uxResource1" />
                        <as:ValidatorMessage ID="ValidatorMessage1" ApplyFor="uxSicMcc" runat="server"></as:ValidatorMessage>
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltBusinessType" ApplyFor="uxBusinessType" runat="server" Text="Business Type:" meta:resourcekey="ltBusinessTypeResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxBusinessType" MaxLength="100" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltBusinessRegistration" ApplyFor="uxBusinessRegistration" runat="server" Text="Business Registration #:" meta:resourcekey="ltBusinessRegistrationResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxBusinessRegistration" MaxLength="20" numeric-only="true" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
                <tr>
                    <td class="heading w-30">
                        <as:ValidatorLabel ID="ltProductSold" ApplyFor="uxProductSold" runat="server" Text="Product Sold:" meta:resourcekey="ltProductSoldResource1"></as:ValidatorLabel>
                    </td>
                    <td>
                        <as:TextBox ID="uxProductSold" MaxLength="50" CssClass="form-control" runat="server" Width="80%" meta:resourcekey="uxResource1" />
                    </td>
                </tr>
            </table>
            <div class="row">
                <div class="col-md-12 form-action-container text-right">
                    <as:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-default"
                        OnClick="btnSubmit_Click"
                        OnClientClick="return ValidateAddNewMerchant();" meta:resourcekey="btnSubmitResource1" />
                </div>
            </div>
        </div>
    </div>
    <div class="hide">
        <as:HiddenField ID="hdBusinessWebId" runat="server" />
        <as:HiddenField ID="hdHideTrCount" runat="server" />
        <as:HiddenField ID="hdTrBusinessWebId" runat="server" />
        <as:Button runat="server" ID="uxRemoveBusinessWeb" OnClick="uxRemoveBusinessWeb_Click" />
        <as:Button runat="server" ID="uxAddBusinessWeb" OnClick="uxAddBusinessWeb_Click" />
    </div>

    <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInput" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
        <Items>
            <%--Merchant ID--%>
            <as:BasicValidationItem ControlToValidateID="uxMerchantNum" Rule="Required" ResMessage="Resources.ValMsg.Required" />
            <as:CustomValidationItem ControlToValidateID="uxMerchantNum" ClientValidationFunction="checkFixed5" ResMessage="Resources.ValMsg.FixedLengthValidationMsg" ResParams="5" />
            <%--DBA Name--%>
            <as:BasicValidationItem ControlToValidateID="uxDBAName" Rule="Required" ResMessage="Resources.ValMsg.Required" />
            <as:BasicValidationItem ControlToValidateID="uxDBAName" Rule="Maxlength" MaxLength="100" ResMessage="Resources.ValMsg.MaxLength" ResParams="100" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxDBAName" ResMessage="Resources.ValMsg.StringUnacceptMsg" ResParams="DBA Name"/>
            <%--Agent Referral--%>
            <as:BasicValidationItem ControlToValidateID="uxAgentReferral" Rule="Maxlength" MaxLength="50" ResMessage="Resources.ValMsg.MaxLength" ResParams="50" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxAgentReferral" ResMessage="Resources.ValMsg.StringUnacceptMsg" ResParams="Agent Referral"/>
            <%--Reseller Name--%>
            <as:BasicValidationItem ControlToValidateID="uxResellerName" Rule="Maxlength" MaxLength="100" ResMessage="Resources.ValMsg.MaxLength" ResParams="100" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxResellerName" ResMessage="Resources.ValMsg.StringUnacceptMsg" ResParams="Reseller Name"/>
            <%--Reseller Phone--%>
            <as:BasicValidationItem ControlToValidateID="uxResellerPhone" Rule="Maxlength" MaxLength="14"
                ResMessage="Resources.ValMsg.MaxLength" ResParams="14" />
            <as:CustomValidationItem ClientValidationFunction="validatePhone" ControlToValidateID="uxResellerPhone" ResMessage="Resources.ValMsg.ManageProfile_InvalidFormat" ResParams="Reseller Phone" />
            <%--Reseller Mail--%>
            <as:BasicValidationItem ControlToValidateID="uxResellerEmail" Rule="Maxlength" MaxLength="100" ResMessage="Resources.ValMsg.MaxLength" ResParams="100" />
            <as:BasicValidationItem ControlToValidateID="uxResellerEmail" Rule="Email" ResMessage="Resources.ValMsg.InvalidEmail" />
            <%--Merchant Corporate/Legal Name--%>
            <as:BasicValidationItem ControlToValidateID="uxMerchantCorporate" Rule="Required" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true" />
            <as:BasicValidationItem ControlToValidateID="uxMerchantCorporate" Rule="Maxlength" MaxLength="100" ResMessage="Resources.ValMsg.MaxLength" ResParams="100" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxMerchantCorporate" ResMessage="Resources.ValMsg.StringUnacceptMsg" ResParams="Merchant Corporate/Legal Name"/>
            <%--Country--%>
            <as:BasicValidationItem ControlToValidateID="uxCountry" Rule="Required" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true" />
            <%--Business Address Line 1--%>
            <as:BasicValidationItem ControlToValidateID="uxBusinessAddLine1" Rule="Required" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true" />
            <as:BasicValidationItem ControlToValidateID="uxBusinessAddLine1" Rule="Maxlength" MaxLength="100" ResMessage="Resources.ValMsg.MaxLength" ResParams="100" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxBusinessAddLine1" ResMessage="Resources.ValMsg.StringUnacceptMsg" ResParams="Business Address Line 1"/>
            <%--Business Address Line 2--%>
            <as:BasicValidationItem ControlToValidateID="uxBusinessAddLine2" Rule="Maxlength" MaxLength="50" ResMessage="Resources.ValMsg.MaxLength" ResParams="50"/>
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxBusinessAddLine2" ResMessage="Resources.ValMsg.StringUnacceptMsg" ResParams="Business Address Line 2" />
            <%--City--%>
            <as:BasicValidationItem ControlToValidateID="uxCity" Rule="Required" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true" />
            <as:BasicValidationItem ControlToValidateID="uxCity" Rule="Maxlength" MaxLength="50" ResMessage="Resources.ValMsg.MaxLength" ResParams="50" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxCity" ResMessage="Resources.ValMsg.StringUnacceptMsg" ResParams="City" />
            <%--Zip/Postal Code--%>
            <as:BasicValidationItem ControlToValidateID="uxZip" Rule="Required" ResMessage="Resources.ValMsg.Required"/>
            <as:BasicValidationItem ControlToValidateID="uxZip" Rule="Maxlength" MaxLength="9" ResMessage="Resources.ValMsg.MaxLength" ResParams="9" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxZip" ResMessage="Resources.ValMsg.StringUnacceptMsg" ResParams="Zip/Postal Code" />
            <%--Business Mail--%>
            <as:BasicValidationItem ControlToValidateID="uxBusinessEmail" Rule="Required" ResMessage="Resources.ValMsg.Required"/>
            <as:BasicValidationItem ControlToValidateID="uxBusinessEmail" Rule="Email" ResMessage="Resources.ValMsg.InvalidEmail"/>
            <as:BasicValidationItem ControlToValidateID="uxBusinessEmail" Rule="Maxlength" MaxLength="100" ResMessage="Resources.ValMsg.MaxLength" ResParams="100" />
            <%--Business Website--%>
            <as:BasicValidationItem ControlToValidateID="uxBusinessWebsite" Rule="Required" ResMessage="Resources.ValMsg.Required" />
            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite" ClientValidationFunction="validationURL" ResMessage="Resources.ValMsg.InvalidURL" IsInAjaxPanel="true"/>

            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite2" ClientValidationFunction="businessWebRequired" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite2" ClientValidationFunction="validationURL" ResMessage="Resources.ValMsg.InvalidURL" IsInAjaxPanel="true"/>

            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite3" ClientValidationFunction="businessWebRequired" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite3" ClientValidationFunction="validationURL" ResMessage="Resources.ValMsg.InvalidURL" IsInAjaxPanel="true"/>

            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite4" ClientValidationFunction="businessWebRequired" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite4" ClientValidationFunction="validationURL" ResMessage="Resources.ValMsg.InvalidURL" IsInAjaxPanel="true"/>

            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite5" ClientValidationFunction="businessWebRequired" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite5" ClientValidationFunction="validationURL" ResMessage="Resources.ValMsg.InvalidURL" IsInAjaxPanel="true"/>

            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite6" ClientValidationFunction="businessWebRequired" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite6" ClientValidationFunction="validationURL" ResMessage="Resources.ValMsg.InvalidURL" IsInAjaxPanel="true"/>

            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite7" ClientValidationFunction="businessWebRequired" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite7" ClientValidationFunction="validationURL" ResMessage="Resources.ValMsg.InvalidURL" IsInAjaxPanel="true"/>

            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite7" ClientValidationFunction="businessWebRequired" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite7" ClientValidationFunction="validationURL" ResMessage="Resources.ValMsg.InvalidURL" IsInAjaxPanel="true"/>

            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite8" ClientValidationFunction="businessWebRequired" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite8" ClientValidationFunction="validationURL" ResMessage="Resources.ValMsg.InvalidURL" IsInAjaxPanel="true"/>

            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite9" ClientValidationFunction="businessWebRequired" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite9" ClientValidationFunction="validationURL" ResMessage="Resources.ValMsg.InvalidURL" IsInAjaxPanel="true"/>

            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite10" ClientValidationFunction="businessWebRequired" ResMessage="Resources.ValMsg.Required" IsInAjaxPanel="true"/>
            <as:CustomValidationItem ControlToValidateID="uxBusinessWebsite10" ClientValidationFunction="validationURL" ResMessage="Resources.ValMsg.InvalidURL" IsInAjaxPanel="true"/>

            <%--SIC/MCC--%>
            <as:BasicValidationItem ControlToValidateID="uxSicMcc" Rule="Required" ResMessage="Resources.ValMsg.Required" />
            <%--Business Type--%>
            <as:BasicValidationItem ControlToValidateID="uxBusinessType" Rule="Required" ResMessage="Resources.ValMsg.Required" />
            <as:BasicValidationItem ControlToValidateID="uxBusinessType" Rule="Maxlength" MaxLength="100" ResMessage="Resources.ValMsg.MaxLength" ResParams="100" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxBusinessType" ResMessage="Resources.ValMsg.StringUnacceptMsg" ResParams="Business Type" />
            <%--Business Registration #--%>
            <as:BasicValidationItem ControlToValidateID="uxBusinessRegistration" Rule="Maxlength" MaxLength="20" ResMessage="Resources.ValMsg.MaxLength" ResParams="20" />
            <%--Product Sold--%>
            <as:BasicValidationItem ControlToValidateID="uxProductSold" Rule="Maxlength" MaxLength="50" ResMessage="Resources.ValMsg.MaxLength" ResParams="50" />
            <as:BasicValidationItem Rule="StringUnaccept" Pattern="<>#&" ControlToValidateID="uxProductSold" ResMessage="Resources.ValMsg.StringUnacceptMsg" ResParams="Product Sold" />
        </Items>
    </as:Validator>
    <as:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var uxMerchantNum_ClientID = "<%= uxMerchantNum.ClientID%>";
            var hdBusinessWebId_ClientID = "<%= hdBusinessWebId.ClientID%>";
            var hdTrBusinessWebId_ClientID = "<%= hdTrBusinessWebId.ClientID%>";
            var uxRemoveBusinessWeb_ClientID = "<%= uxRemoveBusinessWeb.ClientID%>";
            var uxAddBusinessWeb_ClientID = "<%= uxAddBusinessWeb.ClientID%>";
            var MgsExistMerchant = "<%= Resources.ValMsg.DuplicateMerchantMsg%>";
            var MgsInvalidState = "<%= Resources.ValMsg.InvalidState%>";
            var msgMerchantNum_ClientID = "<%= msgMerchantNum.ClientID%>";
            var ltMerchantNum_ClientID = "<%= ltMerchantNum.ClientID%>";
            var IsOpenCase = "<%= IsOpenCase%>";
            var uxAddBWLink_ClientID = "<%= uxAddBWLink.ClientID%>";
            var hdHideTrCount_ClientID = "<%= hdHideTrCount.ClientID%>";
            var uxResellerPhone_ClientID = "<%= uxResellerPhone.ClientID%>";
            var uxStateProvince_ClientID = "<%= uxStateProvince.ClientID%>";
            var uxCountry_ClientID = "<%= uxCountry.ClientID%>";
            var uxSicMcc_ClientID = "<%= uxSicMcc.ClientID%>";
            
        </script>
        <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/jquery/jquery.mask.min.js"></script>
        <script src="<%= ResolveUrl("~/")%>res/js/AddNewMerchant.js"></script>
    </as:RadCodeBlock>
</asp:Content>
