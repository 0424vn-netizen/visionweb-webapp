<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MIF_MechantDetails_CAYAN.ascx.cs" Inherits="UserControls_MIF_MechantDetails_Cayan" %>
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
            <table class="ASTable">
                <colgroup>
                    <col style="width: 10%" />
                    <col style="width: 23.3%" />
                    <col style="width: 10%" />
                    <col style="width: 23.3%" />
                    <col style="width: 10%" />
                    <col style="width: 23.4%" />
                </colgroup>
                <tr class="Row">
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal2" runat="server" Text="Merchant ID:" meta:resourcekey="LiteralResource2" />
                    </td>
                    <td class="valign-top">
                        <%= BindValue("MerchantNumber")%>
                    </td>
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal8" runat="server" Text="Address:" meta:resourcekey="LiteralResource8" />
                    </td>
                    <td class="valign-top">
                        <%= BindAddress(BindValue("Address1"), BindValue("Address2"), BindValue("Address3"), BindValue("City"), BindValue("State"), BindValue("Zip"))%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal3" runat="server" Text="Contact:" meta:resourcekey="LiteralResource3" />
                    </td>
                    <td class="valign-top">
                        <%= BindValue("Contact")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal5" runat="server" Text="Merchant Name:" meta:resourcekey="LiteralResource5" />
                    </td>
                    <td>
                        <%= BindValue("MerchantName")%>
                    </td>

                    <td class="heading valign-top">
                        <asp:Literal ID="Literal72" runat="server" Text="City/State/Zip:" meta:resourcekey="LiteralResource181" />
                    </td>
                    <td>
                        <%= string.Format("{0}/{1}/{2}", BindValue("City"), BindValue("State"), BindValue("Zip"))%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal9" runat="server" Text="Email:" meta:resourcekey="LiteralResource9" />
                    </td>
                    <td colspan="3">
                        <%= BindValue("Email")%>
                    </td>
                </tr>

                <tr class="Row">
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal73" runat="server" Text="Client Name:" meta:resourcekey="LiteralResource37" />
                    </td>
                    <td>
                        <%= BindValue("ClientName")%>
                    </td>
                    <td class="heading valign-top">
                        <asp:Literal ID="Literal74" runat="server" Text="Bankcard City:" meta:resourcekey="LiteralResource182" />
                    </td>
                    <td>
                        <%= BindValue("BankcardCity")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal6" runat="server" Text="Phone:" meta:resourcekey="LiteralResource6" />
                    </td>
                    <td>
                        <%= FormatPhone(BindValue("Phone"))%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal75" runat="server" Text="ReOpen Date:" meta:resourcekey="LiteralResource183" />
                    </td>
                    <td>
                        <%= BindValue("ReOpenedDate")%>
                    </td>

                    <td class="heading valign-top">
                        <asp:Literal ID="Literal76" runat="server" Text="Bankcard State:" meta:resourcekey="LiteralResource184" />
                    </td>
                    <td>
                        <%= BindValue("BankcardState")%>
                    </td>
                    <td class="heading  valign-top text-nowrap">
                        <asp:Literal ID="Literal7" runat="server" Text="Last Batch Activity:" meta:resourcekey="LiteralResource7" />
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkLastBatch"
                            OnClick="lnkLastBatch_Click">  
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal78" runat="server" Text="Last NonMon Date:" meta:resourcekey="LiteralResource185" />
                    </td>
                    <td>
                        <%= BindValue("LastNonMonDate")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal79" runat="server" Text="Retail City:" meta:resourcekey="LiteralResource186" />
                    </td>
                    <td>
                        <%= BindValue("RetailCity")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal80" runat="server" Text="Contact Phone:" meta:resourcekey="LiteralResource187" />
                    </td>
                    <td>
                        <%= BindValue("ContactPhone")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal81" runat="server" Text="Last Active Date:" meta:resourcekey="LiteralResource188" />
                    </td>
                    <td>
                        <%= BindValue("LastActiveDate")%>
                    </td>

                    <td class="heading valign-top">
                        <asp:Literal ID="Literal82" runat="server" Text="Retail State:" meta:resourcekey="LiteralResource189" />
                    </td>
                    <td>
                        <%= BindValue("RetailState")%>
                    </td>

                    <td class="heading valign-top">
                        <asp:Literal ID="Literal85" runat="server" Text="Legal Name:" meta:resourcekey="LiteralResource190" />
                    </td>
                    <td>
                        <%= BindValue("LegalName")%>
                    </td>
                </tr>

                <tr class="Row">
                    <td class="heading  valign-top text-nowrap">
                        <asp:Literal ID="Literal86" runat="server" Text="Last Review Date:" meta:resourcekey="LiteralResource191" />
                    </td>
                    <td>
                        <%= BindValue("LastReviewDate")%>
                    </td>
                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal4" runat="server" Text="Status:" meta:resourcekey="LiteralResource4" />
                    </td>
                    <td>
                        <%= BindValue("Status")%>
                    </td>

                    <td class="heading  valign-top">
                        <asp:Literal ID="Literal87" runat="server" Text="CS Phone:" meta:resourcekey="LiteralResource192" />
                    </td>
                    <td>
                        <%= BindValue("CSPhone")%>
                    </td>
                </tr>
                <asp:Panel ID="uxPnlUserID" runat="server" Visible="false">
                    <tr class="AltRow">
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
    <!-- -->
    <div class="row">
        <div class="col-md-6">
            <as:ExportKeyValueTable IsMultiExport="true" TargetID="uxBusinessInformationCayan" ScrollID="businessinfo" ID="uxExportBusinessInformation"
                runat="server" Title="Business Information" DataSourceID="uxBusinessInformationCayan" meta:resourcekey="LiteralResource10"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxBusinessInformationCayan" runat="server" OnNeedDataSource="GetDataSource" OnItemDataBound="uxBusinessInformationCayan_ItemDataBound">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="CorporateName" UniqueName="CorporateName" meta:resourcekey="LiteralResource13" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="CorporateAddress" UniqueName="CorporateAddress" meta:resourcekey="LiteralResource14" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="Fax" UniqueName="Fax" meta:resourcekey="LiteralResource15" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="OpenDate" UniqueName="OpenDate" meta:resourcekey="LiteralResource16" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="ClosedDate" UniqueName="ClosedDate" meta:resourcekey="LiteralResource17" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="Status" UniqueName="Status" meta:resourcekey="LiteralResource18" FormatType="DynamicString" />
                    <as:KeyValueTableTemplateRow DataField="SiteAccess" UniqueName="SiteAccessTemplate" meta:resourcekey="LiteralResource19">
                        <ItemTemplate>
                            <div class="opt-in-out-text mt-1x">
                                <asp:Literal ID="ltrSiteAccess" runat="server" Text='<%# Eval("SiteAccess")%>' />
                            </div>
                            <div class="pull-right opt-in-out-button">
                                <as:Button ID="uxSiteAccess" runat="server" Text='<%# SetStatusText(Eval("SiteAccess")) %>' CssClass="btn btn-default" Visible='<%# SetVisible(Eval("SiteAccess")) %>' OnClientClick='<%# SetURLForSiteAccessButton() %>' IsStandardButton="False" meta:resourcekey="uxSiteAccessResource1" />
                            </div>
                        </ItemTemplate>
                    </as:KeyValueTableTemplateRow>
                    <%--<as:KeyValueTableBoundRow DataField="SiteAccess" UniqueName="SiteAccess" meta:resourcekey="LiteralResource19" FormatType="DynamicString" />--%>
                    <as:KeyValueTableBoundRow DataField="TaxID" UniqueName="TaxID" meta:resourcekey="LiteralResource20" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="SICDescription" UniqueName="SICDescription" meta:resourcekey="LiteralResource21" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="DepositType" UniqueName="DepositType" meta:resourcekey="LiteralResource22" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="ETCType" UniqueName="ETCType" meta:resourcekey="LiteralResource23" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="ETCCutoff" UniqueName="ETCCutoff" meta:resourcekey="LiteralResource24" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="SeasonalMerchant" UniqueName="SeasonalMerchant" meta:resourcekey="LiteralResource25" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="StatementMailFlagDesc" UniqueName="StatementMailFlagDesc" meta:resourcekey="LiteralResource26" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="StatementICPrintOptions" UniqueName="StatementICPrintOptions" meta:resourcekey="LiteralResource27" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="StatementOnlineDebitFeePrintOptionDesc" UniqueName="StatementOnlineDebitFeePrintOptionDesc" meta:resourcekey="LiteralResource28" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="StatementHoldFlag" UniqueName="StatementHoldFlag" meta:resourcekey="LiteralResource29" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="Misc1" UniqueName="Misc1" meta:resourcekey="LiteralResource30" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="Misc2" UniqueName="Misc2" meta:resourcekey="LiteralResource31" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="TinTypeDesc" UniqueName="TinTypeDesc" meta:resourcekey="LiteralResource32" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="SparkExclDesc" UniqueName="SparkExclDesc" meta:resourcekey="LiteralResource33" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="GGe4IndicatorEffectiveDate" UniqueName="GGe4IndicatorEffectiveDate" meta:resourcekey="LiteralResource193" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="GGe4TransamourServiceCode" UniqueName="GGe4TransamourServiceCode" meta:resourcekey="LiteralResource194" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="MCVIPOSFlags" UniqueName="MCVIPOSFlags" meta:resourcekey="LiteralResource195" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="ChargebackAdviceSW" UniqueName="ChargebackAdviceSW" meta:resourcekey="LiteralResource196" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="RetrievalFlag" UniqueName="RetrievalFlag" meta:resourcekey="LiteralResource197" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="OneTimeExpenseFlag" UniqueName="OneTimeExpenseFlag" meta:resourcekey="LiteralResource198" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="OneTimeIncomeFlag" UniqueName="OneTimeIncomeFlag" meta:resourcekey="LiteralResource199" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="12BLetterType" UniqueName="12BLetterType" meta:resourcekey="LiteralResource200" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="12BMailFlag" UniqueName="12BMailFlag" meta:resourcekey="LiteralResource201" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="MerchantCSPhoneNumber" UniqueName="MerchantCSPhoneNumber" meta:resourcekey="LiteralResource202" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="TransArmorSecurityLevel" UniqueName="TransArmorSecurityLevel" meta:resourcekey="LiteralResource203" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="TransArmorEncryptionType" UniqueName="TransArmorEncryptionType" meta:resourcekey="LiteralResource204" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="TransArmorTokenIndicator" UniqueName="TransArmorTokenIndicator" meta:resourcekey="LiteralResource205" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="TransArmorTokenType" UniqueName="TransArmorTokenType" meta:resourcekey="LiteralResource206" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="EMVIndicator" UniqueName="EMVIndicator" meta:resourcekey="LiteralResource207" FormatType="DynamicString" />
                </RowCollection>
            </as:KeyValueTable>
        </div>
        <div class="col-md-6">
            <!-- Risk Info --> 
            <uc:RiskInfo ID="uxRiskInfo" runat="server" />

            <!-- Bank Information -->
            <as:ExportKeyValueTable IsMultiExport="true"
                TargetID="uxBankInformation"
                ScrollID="bankinfo" ID="uxExportBankInformation"
                runat="server" Title="Bank Information"
                DataSourceID="uxBankInformation"
                meta:resourcekey="LiteralResource44"></as:ExportKeyValueTable>
            <as:KeyValueTable ID="uxBankInformation" runat="server" OnNeedDataSource="GetDataSource" OnItemDataBound="uxBankInformation_ItemDataBound">
                <RowCollection>
                    <as:KeyValueTableBoundRow DataField="BankName" UniqueName="BankName" meta:resourcekey="LiteralResource47" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="RoutingNumber" UniqueName="RoutingNumber" meta:resourcekey="LiteralResource48" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="PartialRoutingNumber" UniqueName="PartialRoutingNumber" meta:resourcekey="LiteralResource48" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="DDANumber" UniqueName="DDANumber" meta:resourcekey="LiteralResource49" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="DaysHold" UniqueName="DaysHold" meta:resourcekey="LiteralResource50" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="MonthlyACHDesc" UniqueName="MonthlyACHDesc" meta:resourcekey="LiteralResource51" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="DisbursementDepositDesc" UniqueName="DisbursementDepositDesc" meta:resourcekey="LiteralResource52" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="DisbursementAdjustmentsDesc" UniqueName="DisbursementAdjustmentsDesc" meta:resourcekey="LiteralResource53" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="DisbursementDiscountDesc" UniqueName="DisbursementDiscountDesc" meta:resourcekey="LiteralResource54" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="DisbursementGenericDebitDesc" UniqueName="DisbursementGenericDebitDesc" meta:resourcekey="LiteralResource55" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="DisbursementDetailDesc" UniqueName="DisbursementDetailDesc" meta:resourcekey="LiteralResource56" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="AssessmentCode" UniqueName="AssessmentCode" meta:resourcekey="LiteralResource176" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="ConfirmLtrSuppRtrlCode" UniqueName="ConfirmLtrSuppRtrlCode" meta:resourcekey="LiteralResource177" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="ConfirmLtrSuppRtrlPrint" UniqueName="ConfirmLtrSuppRtrlPrint" meta:resourcekey="LiteralResource178" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="TapeRefFlag" UniqueName="TapeRefFlag" meta:resourcekey="LiteralResource179" FormatType="DynamicString" />
                    <as:KeyValueTableBoundRow DataField="FinancialAcctType" UniqueName="FinancialAcctType" meta:resourcekey="LiteralResource180" FormatType="DynamicString" />
                </RowCollection>
            </as:KeyValueTable>

            <!-- Other Card Information -->
            <div class="row" id="othercardinformation">
                <div class="col-xs-12" data-toggle="collapse" data-target="#ciOtherCardInformation">
                    <h2 class="grid-title">
                        <asp:Literal ID="Literal27" runat="server" Text="Other Card Information" meta:resourcekey="LiteralResource57" /></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 in" id="ciOtherCardInformation">
                    <asp:Repeater runat="server" ID="uxOtherCardInformation">
                        <ItemTemplate>
                            <table class="ASTable">
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal40" runat="server" Text="AMEX:" meta:resourcekey="LiteralResource60" /></td>
                                    <td>
                                        <%# Eval("AMEX")%>
                                    <td class="heading">
                                        <asp:Literal ID="Literal41" runat="server" Text="Pin Debit:" meta:resourcekey="LiteralResource61" /></td>
                                    <td>
                                        <%# Eval("PinDebit") %>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal42" runat="server" Text="AMEXONPOINT:" meta:resourcekey="LiteralResource62" /></td>
                                    <td>
                                        <%# Eval("AMEXONPOINT")%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal43" runat="server" Text="JCB:" meta:resourcekey="LiteralResource63" /></td>
                                    <td>
                                        <%# Eval("JCB")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal57" runat="server" Text="AMEX PASS THROUGH:" meta:resourcekey="LiteralResource127" /></td>
                                    <td>
                                        <%# Eval("AMEXPassThrough")%>
                                    <td class="heading">
                                        <asp:Literal ID="Literal71" runat="server" Text="Pin Debit:" meta:resourcekey="LiteralResource65" /></td>
                                    <td>
                                        <%# Eval("WrightExpress")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal175" runat="server" Text="Discover:" meta:resourcekey="LiteralResource64" /></td>
                                    <td>
                                        <%# Eval("Discover")%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal176" runat="server" Text="Voyager:" meta:resourcekey="LiteralResource67" /></td>
                                    <td>
                                        <%# Eval("Voyager")%>
                                    </td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal240" runat="server" Text="DiscoverFullAQC:" meta:resourcekey="LiteralResource66" /></td>
                                    <td>
                                        <%# Eval("DiscoverFullAQC")%>
                                    <td class="heading">
                                        <asp:Literal ID="Literal241" runat="server" Text="Paypal:" meta:resourcekey="LiteralResourceOtherCard_Paypal_Header" /></td>
                                    <td>
                                        <%# Eval("PayPalEnabledDI")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <!-- End Other Card Information -->
            <!--Merchant Card Information – PIN Debit-->
            <div class="row" id="merchantcardpindebitinfo">
                <div class="col-xs-12" data-toggle="collapse" data-target="#ciMerchantCardPinDebitInformation">
                    <h2 class="grid-title">
                        <asp:Literal ID="Literal155" runat="server" Text="Merchant Card Information – PIN Debit" meta:resourcekey="LiteralResource154" /></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 in table-responsive" id="ciMerchantCardPinDebitInformation">
                    <asp:Repeater ID="uxMerchantCardPinDebitInformation" runat="server">
                        <ItemTemplate>
                            <table class="ASTable">
                                <tr>
                                    <th class="heading"></th>
                                    <th>
                                        <asp:Literal ID="Literal171" runat="server" Text="Other Volume %" meta:resourcekey="LiteralResource170" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal172" runat="server" Text="Other Item Rate" meta:resourcekey="LiteralResource171" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal173" runat="server" Text="Online Debit Fee Flag" meta:resourcekey="LiteralResource172" />
                                    </th>
                                </tr>
                                <tr class="Row" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("ACCEL") %>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal158" runat="server" Text="ACCEL" meta:resourcekey="LiteralResource157" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeACCEL")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateACCEL")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescACCEL") %>'><%# Eval("OnlDbtFeeFlagACCEL")%></span>
                                    </td>
                                </tr>
                                <tr class="AltRow" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("AFFN")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal159" runat="server" Text="AFFN" meta:resourcekey="LiteralResource158" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeAFFN")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateAFFN")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescAFFN") %>'><%# Eval("OnlDbtFeeFlagAFFN")%></span>
                                    </td>
                                </tr>
                                <tr class="Row" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("ALASKA OPTION")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal160" runat="server" Text="ALASKA OPTION" meta:resourcekey="LiteralResource159" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeAlaskaOption")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateAlaskaOption")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescAlaskaOption") %>'><%# Eval("OnlDbtFeeFlagAlaskaOpt")%></span>
                                    </td>
                                </tr>
                                <tr class="AltRow" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("CU24")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal161" runat="server" Text="CU24" meta:resourcekey="LiteralResource160" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeCU24")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateCU24")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescCU24") %>'><%# Eval("OnlDbtFeeFlagCU24")%></span>
                                    </td>
                                </tr>
                                <tr class="Row" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("INTERLINK")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal162" runat="server" Text="INTERLINK" meta:resourcekey="LiteralResource161" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeInterlink")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateInterlink")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescInterlink") %>'><%# Eval("OnlDbtFeeFlagInterlink")%></span>
                                    </td>
                                </tr>
                                <tr class="AltRow" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("JEANIE")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal163" runat="server" Text="JEANIE" meta:resourcekey="LiteralResource162" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeJeanie")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateJeanie")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescJeanie") %>'><%# Eval("OnlDbtFeeFlagJeanie")%></span>
                                    </td>
                                </tr>
                                <tr class="Row" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("MAESTRO")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal164" runat="server" Text="MAESTRO" meta:resourcekey="LiteralResource163" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeMaestro")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateMaestro")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescMaestro") %>'><%# Eval("OnlDbtFeeFlagMaestro")%></span>
                                    </td>
                                </tr>
                                <tr class="AltRow" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("NYCE")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal165" runat="server" Text="NYCE" meta:resourcekey="LiteralResource164" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeNYCE")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateNYCE")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescNYCE") %>'><%# Eval("OnlDbtFeeFlagNYCE")%></span>
                                    </td>
                                </tr>
                                <tr class="Row" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("PULSE")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal166" runat="server" Text="PULSE" meta:resourcekey="LiteralResource165" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumePULSE")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRatePULSE")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescPULSE") %>'><%# Eval("OnlDbtFeeFlagPULSE")%></span>
                                    </td>
                                </tr>
                                <tr class="AltRow" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("SHAZAM")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal167" runat="server" Text="SHAZAM" meta:resourcekey="LiteralResource166" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeShazam")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateShazam")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescShazam") %>'><%# Eval("OnlDbtFeeFlagShazam")%></span>
                                    </td>
                                </tr>
                                <tr class="Row" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("STAR013")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal168" runat="server" Text="STAR" meta:resourcekey="LiteralResource167" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeStar013")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateStar013")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescStar013") %>'><%# Eval("OnlDbtFeeFlagStar013")%></span>
                                    </td>
                                </tr>
                                <tr class="AltRow" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("STAR018")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal169" runat="server" Text="STAR" meta:resourcekey="LiteralResource168" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeStar018")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateStar018")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescStar018") %>'><%# Eval("OnlDbtFeeFlagStar018")%></span>
                                    </td>
                                </tr>
                                <tr class="Row" runat="server" visible='<%# CheckMerchantCardInformationPinDebit("STAR021")%>'>
                                    <td class="font-weight-bold">
                                        <asp:Literal ID="Literal170" runat="server" Text="STAR" meta:resourcekey="LiteralResource169" />
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeStar021")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateStar021")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("OnlDbtFeeFlagDescStar021") %>'><%# Eval("OnlDbtFeeFlagStar021")%></span>
                                    </td>
                                </tr>

                            </table>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:PlaceHolder ID="uxMerchantCardPinDebitInformationNorecords" runat="server" Visible="false">
                        <div class="NoRecords">
                            <asp:Literal ID="Literal10" runat="server" Text="No records to display." meta:resourcekey="AS_HierarchyGrid_NoRecord"></asp:Literal>
                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>
            <!-- hierarchyinfo -->
            <div class="row" id="hierarchyinfo">
                <div class="col-xs-12" data-toggle="collapse" data-target="#ciHierarchyInformation">
                    <h2 class="grid-title">
                        <asp:Literal ID="Literal34" runat="server" Text="Hierarchy Information" meta:resourcekey="LiteralResource34" /></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 in" id="ciHierarchyInformation">
                    <asp:Repeater runat="server" ID="uxHierarchyInformation" OnItemDataBound="uxHierarchyInformation_ItemDataBound">
                        <ItemTemplate>
                            <table class="ASTable">
                                <tr class="Row">
                                    <td class="heading valign-middle">
                                        <asp:Literal ID="Literal37" runat="server" Text="Client Name:" meta:resourcekey="LiteralResource37" /></td>
                                    <td colspan="3">
                                        <%# Eval("ClientName")%>
                                    </td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal38" runat="server" Text="Client Login:" meta:resourcekey="LiteralResource38" /></td>
                                    <td>
                                        <%# Eval("ClientLogin")%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal39" runat="server" Text="Sales Agent:" meta:resourcekey="LiteralResource39" /></td>
                                    <td>
                                        <as:PlaceHolder ID="uxHSalesAgent" runat="server" Visible='<%# CheckHierarchy("SALESAGENT") %>'>
                                            <a href="#" id="uxLinkHSalesAgent" runat="server"><%# Eval("SalesAgent")%></a>
                                        </as:PlaceHolder>
                                        <as:Literal ID="uxLHSalesAgent" runat="server" Visible='<%# !CheckHierarchy("SALESAGENT") %>' Text='<%# Eval("SalesAgent") %>'></as:Literal></td>
                                </tr>
                                <tr class="Row">
                                    <td class="heading">
                                        <asp:Literal ID="Literal40" runat="server" Text="Sys/Prin/Agent:" meta:resourcekey="LiteralResource40" /></td>
                                    <td>
                                        <as:PlaceHolder ID="uxHAgent" runat="server" Visible='<%# CheckHierarchy("AGENT") %>'>
                                            <a href="#" id="uxLinkHAgent" runat="server"><%# Eval("SysPrinAgent")%></a>
                                        </as:PlaceHolder>
                                        <as:Literal ID="uxLHAgent" runat="server" Visible='<%# !CheckHierarchy("AGENT") %>' Text='<%# Eval("SysPrinAgent") %>'></as:Literal></td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal41" runat="server" Text="Headqrtr Merchant:" meta:resourcekey="LiteralResource41" /></td>
                                    <td>
                                        <as:PlaceHolder ID="uxHHeadquaster" runat="server" Visible='<%# CheckHierarchy("Headquarter") %>'>
                                            <a href="#" id="uxLinkHHeadquaster" runat="server"><%# Eval("Headquarter")%></a>
                                        </as:PlaceHolder>
                                        <as:Literal ID="uxLHHeadquarter" runat="server" Visible='<%# !CheckHierarchy("HEADQUARTER") %>' Text='<%# Eval("Headquarter") %>'></as:Literal></td>
                                </tr>
                                <tr class="AltRow">
                                    <td class="heading">
                                        <asp:Literal ID="Literal42" runat="server" Text="Merchant Type:" meta:resourcekey="LiteralResource42" /></td>
                                    <td>
                                        <%# Eval("MerchantType")%>
                                    </td>
                                    <td class="heading">
                                        <asp:Literal ID="Literal43" runat="server" Text="Chain Code:" meta:resourcekey="LiteralResource43" /></td>
                                    <td>
                                        <as:PlaceHolder ID="uxChainCode" runat="server" Visible='<%# CheckHierarchy("CHAIN") %>'>
                                            <a href="#" id="uxLinkChainCode" runat="server"><%# Eval("ChainCode")%></a>
                                        </as:PlaceHolder>
                                        <as:Literal ID="lblChainCode" runat="server" Visible='<%# !CheckHierarchy("CHAIN") %>' Text='<%# Eval("ChainCode") %>'></as:Literal></td>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

        </div>
        <!-- -->
    </div>




    <asp:PlaceHolder ID="phdTransaction" runat="server">
        <!-- transaction -->
        <div class="row" id="transactionEstGrid">
            <div class="col-md-12" data-toggle="collapse" data-target="#ciTransactionEstGrid">
                <h2 class="grid-title">
                    <asp:Literal ID="Literal183" runat="server" Text="Transaction Estimates" meta:resourcekey="LiteralResourceTransactionEstimates_Title"></asp:Literal>
                </h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 in" id="ciTransactionEstGrid">
                <asp:Repeater runat="server" ID="uxTransactionEstGrid">
                    <ItemTemplate>
                        <table class="ASTable">
                            <tr class="Row">
                                <td class="heading" style="width: 35%">

                                    <asp:Literal ID="Literal184" runat="server" Text="Chain Hdqtr Pct:" meta:resourcekey="LiteralResourceTransactionEstimates_ChainHdqtrPct_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# FormatPercent(Eval("ChainHdqtrPct"))%>
                                </td>
                                <td class="heading" style="width: 25%">
                                    <asp:Literal ID="Literal185" runat="server" Text="Exclude Adj & Chargebacks:" meta:resourcekey="LiteralResourceTransactionEstimates_ExcludeAdjChargebacks_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# Eval("ExcludeAdjChargebacks")%>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading" style="width: 35%">
                                    <asp:Literal ID="Literal186" runat="server" Text="Daily Auth Decline Pct:" meta:resourcekey="LiteralResourceTransactionEstimates_DailyAuthDeclinePct_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# FormatPercent(Eval("DailyAuthDeclinePct"))%>
                                </td>
                                <td class="heading" style="width: 25%">

                                    <asp:Literal ID="Literal187" runat="server" Text="# Daily Trans Per Card:" meta:resourcekey="LiteralResourceTransactionEstimates_DailyTransPerCard_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# FormatInteger(Eval("NumberDailyTransPerCard"))%>
                                </td>
                            </tr>
                            <tr class="Row">
                                <td class="heading" style="width: 35%">
                                    <asp:Literal ID="Literal188" runat="server" Text="MTD Chargeback Pct:" meta:resourcekey="LiteralResourceTransactionEstimates_MTDChargebackPct_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# FormatPercent(Eval("MTDChargebackPct"))%>
                                </td>
                                <td class="heading" style="width: 25%">
                                    <asp:Literal ID="Literal189" runat="server" Text="Max Auth Amt:" meta:resourcekey="LiteralResourceTransactionEstimates_MaxAuthAmt_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# FormatCurrency(Eval("MaxAuthAmt"))%>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading" style="width: 35%">
                                    <asp:Literal ID="Literal190" runat="server" Text="Daily Auths Count:" meta:resourcekey="LiteralResourceTransactionEstimates_DailyAuthsCount_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# FormatInteger(Eval("DailyAuthsCount"))%>
                                </td>
                                <td class="heading" style="width: 25%">
                                    <asp:Literal ID="Literal191" runat="server" Text="Round Discount:" meta:resourcekey="LiteralResourceTransactionEstimates_RoundDiscount_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# Eval("RoundDiscount")%>
                                </td>
                            </tr>

                            <tr class="Row">
                                <td class="heading" style="width: 35%">
                                    <asp:Literal ID="Literal192" runat="server" Text="Daily Auths Card Pct:" meta:resourcekey="LiteralResourceTransactionEstimates_DailyAuthsCardPct_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# FormatPercent(Eval("DailyAuthsCardPct"))%>
                                </td>
                                <td class="heading" style="width: 25%">
                                    <asp:Literal ID="Literal193" runat="server" Text="# Auths Same Amt:" meta:resourcekey="LiteralResourceTransactionEstimates_AuthsSameAmt_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# FormatInteger(Eval("NumberAuthsSameAmt"))%>
                                </td>
                            </tr>

                            <tr class="AltRow">
                                <td class="heading" style="width: 35%">
                                    <asp:Literal ID="Literal194" runat="server" Text="Below Floor Amt:" meta:resourcekey="LiteralResourceTransactionEstimates_BelowFloorAmt_Header"></asp:Literal>
                                </td>
                                <td style="width: 15%">
                                    <%# FormatPercent(Eval("BelowFloorAmt"))%>
                                </td>
                                <td class="heading" style="width: 25%">
                                    <asp:Literal ID="Literal195" runat="server" Text="Voice Auth Pct:" meta:resourcekey="LiteralResourceTransactionEstimates_VoiceAuthPct_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# FormatPercent(Eval("VoiceAuthPct"))%>
                                </td>
                            </tr>

                            <tr class="Row">
                                <td class="heading" style="width: 35%">
                                    <asp:Literal ID="Literal196" runat="server" Text="Same Amt Pct:" meta:resourcekey="LiteralResourceTransactionEstimates_SameAmtPct_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# FormatPercent(Eval("SameAmtPct"))%>
                                </td>
                                <td class="heading" style="width: 25%">
                                    <asp:Literal ID="Literal197" runat="server" Text="Daily Retrieval Count:" meta:resourcekey="LiteralResourceTransactionEstimates_DailyRetrievalCount_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# FormatInteger(Eval("DailyRetrievalCount"))%>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading" style="width: 35%">
                                    <asp:Literal ID="Literal198" runat="server" Text="Ach Inc Excl:" meta:resourcekey="LiteralResourceTransactionEstimates_AchIncExcl_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# Eval("AchIncExcl")%>
                                </td>
                                <td class="heading" style="width: 25%">
                                    <asp:Literal ID="Literal199" runat="server" Text="Daily Retrieval Amt:" meta:resourcekey="LiteralResourceTransactionEstimates_DailyRetrievalAmt_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# FormatCurrency(Eval("DailyRetrievalAmt"))%>
                                </td>
                            </tr>

                            <tr class="Row">
                                <td class="heading" style="width: 35%">
                                    <asp:Literal ID="Literal200" runat="server" Text="Return Vol Pct:" meta:resourcekey="LiteralResourceTransactionEstimates_ReturnVolPct_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# FormatPercent(Eval("ReturnVolPct"))%>
                                </td>
                                <td class="heading" style="width: 25%">
                                    <asp:Literal ID="Literal201" runat="server" Text="Batch Return Count:" meta:resourcekey="LiteralResourceTransactionEstimates_BatchReturnCount_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# FormatInteger(Eval("BatchReturnCount"))%>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading" style="width: 35%">

                                    <asp:Literal ID="Literal202" runat="server" Text="Return Count Pct:" meta:resourcekey="LiteralResourceTransactionEstimates_ReturnCountPct_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# FormatPercent(Eval("ReturnCountPct"))%>
                                </td>
                                <td class="heading" style="width: 25%">
                                    <asp:Literal ID="Literal203" runat="server" Text="Batch Return Amt:" meta:resourcekey="LiteralResourceTransactionEstimates_BatchReturnAmt_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# FormatCurrency(Eval("BatchReturnAmt"))%>
                                </td>
                            </tr>

                            <tr class="Row">
                                <td class="heading" style="width: 35%">
                                    <asp:Literal ID="Literal204" runat="server" Text="Max # TKTOs:" meta:resourcekey="LiteralResourceTransactionEstimates_MaxTKTOs_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# FormatInteger(Eval("MaxNumberTKTOs"))%>
                                </td>
                                <td class="heading" style="width: 25%">
                                    <asp:Literal ID="Literal205" runat="server" Text="Max Return Amt:" meta:resourcekey="LiteralResourceTransactionEstimates_MaxReturnAmt_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# FormatCurrency(Eval("MaxReturnAmt"))%>
                                </td>
                            </tr>
                            <tr class="AltRow">
                                <td class="heading" style="width: 35%">
                                    <asp:Literal ID="Literal206" runat="server" Text="# Chargeback Occurrences:" meta:resourcekey="LiteralResourceTransactionEstimates_ChargebackOccurrences_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# FormatInteger(Eval("NumberChargebackOccurrences"))%>
                                </td>
                                <td class="heading" style="width: 25%">
                                    <asp:Literal ID="Literal207" runat="server" Text="Fund Lmt Day:" meta:resourcekey="LiteralResourceTransactionEstimates_FundLmtDay_Header"></asp:Literal>

                                </td>
                                <td style="width: 25%">
                                    <%# Eval("FundLmtDay")%>
                                </td>
                            </tr>

                            <tr class="Row">
                                <td class="heading" style="width: 35%">
                                    <asp:Literal ID="Literal208" runat="server" Text="# Weekly Batches:" meta:resourcekey="LiteralResourceTransactionEstimates_WeeklyBatches_Header"></asp:Literal>

                                </td>
                                <td style="width: 15%">
                                    <%# FormatInteger(Eval("NumberWeeklyBatches"))%>
                                </td>
                                <td class="heading" style="width: 25%">

                                    <asp:Literal ID="Literal209" runat="server" Text="Limit 30 Day:" meta:resourcekey="LiteralResourceTransactionEstimates_Limit30Day_Header"></asp:Literal>


                                </td>
                                <td style="width: 25%">
                                    <%# Eval("Limit30Day")%>
                                </td>
                            </tr>

                        </table>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:PlaceHolder>

    <!--Merchant Card Information-->
    <asp:PlaceHolder runat="server">
        <div class="row" id="merchantcardinfo">
            <div class="col-xs-12" data-toggle="collapse" data-target="#ciMerchantCardInformation">
                <h2 class="grid-title">
                    <asp:Literal ID="Literal118" runat="server" Text="Merchant Card Information" meta:resourcekey="LiteralResource118" /></h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 in" id="ciMerchantCardInformation">
                <div class="table-responsive">
                    <asp:Repeater runat="server" ID="uxMerchantCardInformation">
                        <ItemTemplate>
                            <table class="ASTable" id="merchantcardinformation">
                                <tr class="nobackground">
                                    <th style="width: 10%"></th>
                                    <th>
                                        <asp:Literal ID="Literal136" runat="server" Text="Proc SW" meta:resourcekey="LiteralResource135" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal137" runat="server" Text="Fee Class" meta:resourcekey="LiteralResource136" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal138" runat="server" Text="Qual Rate" meta:resourcekey="LiteralResource137" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal139" runat="server" Text="Mid-Qual Rate" meta:resourcekey="LiteralResource138" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal140" runat="server" Text="Non-Qual Rate" meta:resourcekey="LiteralResource139" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal141" runat="server" Text="Interchange Fee Flag" meta:resourcekey="LiteralResource140" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal142" runat="server" Text="Dues & Assessment Flag" meta:resourcekey="LiteralResource141" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal143" runat="server" Text="Dues & Assessments Vol < 1.000" meta:resourcekey="LiteralResource142" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal144" runat="server" Text="Dues & Assessments Item < 1.000" meta:resourcekey="LiteralResource143" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal145" runat="server" Text="Dues & Assessments Vol > 1.000" meta:resourcekey="LiteralResource144" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal146" runat="server" Text="Dues & Assessments Item > 1.000" meta:resourcekey="LiteralResource145" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal147" runat="server" Text="Merchant Pricing Grid" meta:resourcekey="LiteralResource146" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal148" runat="server" Text="Tiered Discount Grid" meta:resourcekey="LiteralResource147" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal149" runat="server" Text="ERR %" meta:resourcekey="LiteralResource148" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal150" runat="server" Text="Other Volume %" meta:resourcekey="LiteralResource149" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal151" runat="server" Text="Other Item Rate" meta:resourcekey="LiteralResource150" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal152" runat="server" Text="Discount Method" meta:resourcekey="LiteralResource151" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal153" runat="server" Text="Amex OnePT Rate" meta:resourcekey="LiteralResource152" />
                                    </th>
                                    <th>
                                        <asp:Literal ID="Literal154" runat="server" Text="Amex OnePT Per Item Fee" meta:resourcekey="LiteralResource153" />
                                    </th>
                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("MC")%>'>
                                    <td>
                                        <asp:Literal ID="Literal120" runat="server" Text="MC" meta:resourcekey="LiteralResource119" />
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateMC")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescMC") %>'><%#Eval("InterchangeFeeFlagMC")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescMC") %>'><%#Eval("DuesAssessmentFlagMC")%></span>
                                    </td>
                                    <td>
                                        <%# Eval("DueAsmtVolLower1MC")%>
                                    </td>
                                    <td>
                                        <%# Eval("DueAsmtItemLower1MC")%>
                                    </td>
                                    <td>
                                        <%# Eval("DueAsmtVolGreater1MC")%>
                                    </td>
                                    <td>
                                        <%# Eval("DueAsmtItemGreater1MC")%>
                                    </td>
                                    <td>
                                        <%# Eval("MerchantPricingGridMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeMC")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateMC")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescMC") %>'><%# Eval("DiscountMethodMC")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("MC DB")%>'>
                                    <td>
                                        <asp:Literal ID="Literal121" runat="server" Text="MC DB" meta:resourcekey="LiteralResource120" />
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateMCDB")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescMCDB") %>'><%#Eval("InterchangeFeeFlagMCDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescMCDB") %>'><%#Eval("DuesAssessmentFlagMCDB")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridMCDB")%>
                                    </td>

                                    <td>
                                        <%# Eval("TieredDiscountGridMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeMCDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateMCDB")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescMCDB") %>'><%# Eval("DiscountMethodMCDB")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("VISA")%>'>
                                    <td>
                                        <asp:Literal ID="Literal122" runat="server" Text="VISA" meta:resourcekey="LiteralResource121" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateVS")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescVS") %>'><%#Eval("InterchangeFeeFlagVS")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescVS") %>'><%#Eval("DuesAssessmentFlagVS")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeVS")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateVS")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescVS") %>'><%# Eval("DiscountMethodVS")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>

                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("VISA DB")%>'>
                                    <td>
                                        <asp:Literal ID="Literal123" runat="server" Text="VISA DB" meta:resourcekey="LiteralResource122" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateVSDB")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescVSDB") %>'><%#Eval("InterchangeFeeFlagVSDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescVSDB") %>'><%#Eval("DuesAssessmentFlagVSDB")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeVSDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateVSDB")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescVSDB") %>'><%# Eval("DiscountMethodVSDB")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>

                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("DISC FULL ACQ")%>'>
                                    <td>
                                        <asp:Literal ID="Literal124" runat="server" Text="DISC FULL ACQ" meta:resourcekey="LiteralResource123" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateDI")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescDI") %>'><%#Eval("InterchangeFeeFlagDI")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescDI") %>'><%# Eval("DuesAssessmentFlagDI")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeDI")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateDI")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescDI") %>'><%# Eval("DiscountMethodDI")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>

                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("DISC FULL ACQ DB")%>'>
                                    <td>
                                        <asp:Literal ID="Literal125" runat="server" Text="DISC FULL ACQ DB" meta:resourcekey="LiteralResource124" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateDIDB")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescDIDB") %>'><%#Eval("InterchangeFeeFlagDIDB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescDIDB") %>'><%# Eval("DuesAssessmentFlagDIDB")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeDIDB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateDIDB")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescDIDB") %>'><%# Eval("DiscountMethodDIDB")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr crunat="server" visible='<%# CheckMerchantCardInformation("DISC PASS THRU")%>'>
                                    <td>
                                        <asp:Literal ID="Literal126" runat="server" Text="DISC PASS THRU" meta:resourcekey="LiteralResource125" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateDIPassThru")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescDIPassThru") %>'><%#Eval("InterchangeFeeFlagDIPassThru")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescDIPassThru") %>'><%# Eval("DuesAssessmentFlagDIPassThru")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeDIPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateDIPassThru")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescDIPassThru") %>'><%# Eval("DiscountMethodDIPassThru")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("AMEXONEPT")%>'>
                                    <td>
                                        <asp:Literal ID="Literal127" runat="server" Text="AMEXONEPT" meta:resourcekey="LiteralResource126" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateAmexOnePT")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescAmexOnePT") %>'><%#Eval("InterchangeFeeFlagAmexOnePT")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescAmexOnePT") %>'><%# Eval("DuesAssessmentFlagAmexOnePT")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateAmexOnePT")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescAmexOnePT") %>'><%# Eval("DiscountMethodAmexOnePT")%></span>
                                    </td>
                                    <td>
                                        <%# Eval("AmexOnePTRateAmexOnePT")%>
                                    </td>
                                    <td>
                                        <%# Eval("AmexOnePTPerItemFeeAmexOnePT")%>
                                    </td>

                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("AMEX PASS THRU")%>'>
                                    <td>
                                        <asp:Literal ID="Literal128" runat="server" Text="AMEX PASS THRU" meta:resourcekey="LiteralResource127" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWAmesPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassAmesPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateAmesPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateAmesPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateAmesPassThru")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescAmesPassThru") %>'><%#Eval("InterchangeFeeFlagAmesPassThru")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescAmesPassThru") %>'><%# Eval("DuesAssessmentFlagAmesPassThru")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridAmesPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridAmesPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRAmesPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeAmesPassThru")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateAmesPassThru")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescAmesPassThru") %>'><%# Eval("DiscountMethodAmesPassThru")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("AMEX")%>'>
                                    <td class="text-left">
                                        <asp:Literal ID="Literal242" runat="server" Text="AMEX" meta:resourcekey="LiteralResourceMerchantCardInfo_CardAMEX_Header" />
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateAmexBlue")%>
                                    </td>
                                    <td id="InterchangeFeeFlagAmexBlue">
                                        <span>
                                            <%# Eval("InterchangeFeeFlagAmexBlue")%></span>
                                        <tek:RadToolTip ID="RadToolTip83" runat="server" Visible='<%#Eval("InterchangeFeeFlagDescAmexBlue")!=DBNull.Value %>' TargetControlID="InterchangeFeeFlagAmexBlue" RelativeTo="Element"
                                            Position="Center" RenderInPageRoot="true">
                                            <%#Eval("InterchangeFeeFlagDescAmexBlue") %>
                                        </tek:RadToolTip>
                                    </td>
                                    <td id="DuesAssessmentFlagAmexBlue">
                                        <span>
                                            <%# Eval("DuesAssessmentFlagAmexBlue")%></span>
                                        <tek:RadToolTip ID="RadToolTip84" runat="server" Visible='<%#Eval("DuesAndAssessmentFlagDescAmexBlue")!=DBNull.Value %>' TargetControlID="DuesAssessmentFlagAmexBlue" RelativeTo="Element"
                                            Position="Center" RenderInPageRoot="true">
                                            <%#Eval("DuesAndAssessmentFlagDescAmexBlue") %>
                                        </tek:RadToolTip>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeAmexBlue")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateAmexBlue")%>
                                    </td>
                                    <td id="DiscountMethodAmexBlue">
                                        <span>
                                            <%# Eval("DiscountMethodAmexBlue")%></span>
                                        <tek:RadToolTip ID="RadToolTip85" runat="server" Visible='<%#Eval("DiscountMethodDescAmexBlue")!=DBNull.Value %>' TargetControlID="DiscountMethodAmexBlue" RelativeTo="Element"
                                            Position="Center" RenderInPageRoot="true">
                                            <%#Eval("DiscountMethodDescAmexBlue") %>
                                        </tek:RadToolTip>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>

                                <tr runat="server" visible='<%# CheckMerchantCardInformation("WRIGHT EXPRESS")%>'>
                                    <td>
                                        <asp:Literal ID="Literal12" runat="server" Text="WRIGHT EXPRESS" meta:resourcekey="LiteralResource65" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateWrightExpress")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescWrightExpress") %>'><%#Eval("InterchangeFeeFlagWrightExpress")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescWrightExpress") %>'><%# Eval("DuesAssessmentFlagWrightExpress")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeWrightExpress")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateWrightExpress")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescWrightExpress") %>'><%# Eval("DiscountMethodWrightExpress")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>

                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("VOYAGER")%>'>
                                    <td>
                                        <asp:Literal ID="Literal129" runat="server" meta:resourcekey="LiteralResource128" Text="VOYAGER"></asp:Literal>

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateVoyager")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescVoyager") %>'><%#Eval("InterchangeFeeFlagVoyager")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescVoyager") %>'><%# Eval("DuesAssessmentFlagVoyager")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeVoyager")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateVoyager")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescVoyager") %>'><%# Eval("DiscountMethodVoyager")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("GENERIC DEBIT")%>'>
                                    <td>
                                        <asp:Literal ID="Literal130" runat="server" Text="GENERIC DEBIT" meta:resourcekey="LiteralResource129" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateGenericDebit")%>
                                    </td>

                                    <td>
                                        <%# Eval("NonQualRateGenericDebit")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescGenericDebit") %>'><%#Eval("InterchangeFeeFlagGenericDebit")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescGenericDebit") %>'><%# Eval("DuesAssessmentFlagGenericDebit")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeGenericDebit")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateGenericDebit")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescGenericDebit") %>'><%# Eval("DiscountMethodGenericDebit")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>

                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("DINERS")%>'>
                                    <td>
                                        <asp:Literal ID="Literal131" runat="server" Text="DINERS" meta:resourcekey="LiteralResource130" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateDinner")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescDinner") %>'><%#Eval("InterchangeFeeFlagDinner")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescDinner") %>'><%# Eval("DuesAssessmentFlagDinner")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeDinner")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateDinner")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescDinner") %>'><%# Eval("DiscountMethodDinner")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>

                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("EBT CASH/B")%>'>
                                    <td>
                                        <asp:Literal ID="Literal132" runat="server" Text="EBT CASH/B" meta:resourcekey="LiteralResource131" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescEBT_CASH_B") %>'><%#Eval("InterchangeFeeFlagEBT_CASH_B")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescEBT_CASH_B") %>'><%# Eval("DuesAssessmentFlagEBT_CASH_B")%></span>
                                    </td>
                                    <td></td>

                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERREBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateEBT_CASH_B")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescEBT_CASH_B") %>'><%# Eval("DiscountMethodEBT_CASH_B")%></span>
                                    </td>
                                    <td></td>

                                    <td></td>


                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("EBT F/STMP")%>'>
                                    <td>
                                        <asp:Literal ID="Literal133" runat="server" Text="EBT F/STMP" meta:resourcekey="LiteralResource132" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateEBT_F_STMP")%>
                                    </td>

                                    <td>
                                        <%# Eval("MidQualRateEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescEBT_F_STMP") %>'><%#Eval("InterchangeFeeFlagEBT_F_STMP")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescEBT_F_STMP") %>'><%# Eval("DuesAssessmentFlagEBT_F_STMP")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>

                                    <td>
                                        <%# Eval("MerchantPricingGridEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERREBT_F_STMP")%>
                                    </td>

                                    <td>
                                        <%# Eval("OtherVolumeEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateEBT_F_STMP")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescEBT_F_STMP") %>'><%# Eval("DiscountMethodEBT_F_STMP")%></span>
                                    </td>

                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("EBT-TAPE")%>'>
                                    <td>
                                        <asp:Literal ID="Literal134" runat="server" Text="EBT-TAPE" meta:resourcekey="LiteralResource133" />

                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassEBT_TAPE")%>
                                    </td>

                                    <td>
                                        <%# Eval("QualRateEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescEBT_TAPE") %>'><%#Eval("InterchangeFeeFlagEBT_TAPE")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescEBT_TAPE") %>'><%# Eval("DuesAssessmentFlagEBT_TAPE")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERREBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateEBT_TAPE")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescEBT_TAPE") %>'><%# Eval("DiscountMethodEBT_TAPE")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr runat="server" visible='<%# CheckMerchantCardInformation("JCB")%>'>
                                    <td>
                                        <asp:Literal ID="Literal135" runat="server" Text="JCB" meta:resourcekey="LiteralResource134" />
                                    </td>
                                    <td>
                                        <%# Eval("ProcessSWJCB")%>
                                    </td>
                                    <td>
                                        <%# Eval("FeeClassJCB")%>
                                    </td>
                                    <td>
                                        <%# Eval("QualRateJCB")%>
                                    </td>
                                    <td>
                                        <%# Eval("MidQualRateJCB")%>
                                    </td>
                                    <td>
                                        <%# Eval("NonQualRateJCB")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("InterchangeFeeFlagDescJCB") %>'><%#Eval("InterchangeFeeFlagJCB")%></span>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DuesAndAssessmentFlagDescJCB") %>'><%# Eval("DuesAssessmentFlagJCB")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <%# Eval("MerchantPricingGridJCB")%>
                                    </td>
                                    <td>
                                        <%# Eval("TieredDiscountGridJCB")%>
                                    </td>
                                    <td>
                                        <%# Eval("ERRJCB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumeJCB")%>
                                    </td>
                                    <td>
                                        <%# Eval("OtherItemRateJCB")%>
                                    </td>
                                    <td>
                                        <span title='<%# Eval("DiscountMethodDescJCB") %>'><%# Eval("DiscountMethodJCB")%></span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:PlaceHolder ID="uxMerchantCardInformationNoRecords" runat="server" Visible="false">
                        <table class="ASTable" id="merchantcardinformation">
                            <tr class="nobackground">
                                <th style="width: 10%"></th>
                                <th>
                                    <asp:Literal ID="Literal136" runat="server" Text="Proc SW" meta:resourcekey="LiteralResource135" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal137" runat="server" Text="Fee Class" meta:resourcekey="LiteralResource136" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal138" runat="server" Text="Qual Rate" meta:resourcekey="LiteralResource137" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal139" runat="server" Text="Mid-Qual Rate" meta:resourcekey="LiteralResource138" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal140" runat="server" Text="Non-Qual Rate" meta:resourcekey="LiteralResource139" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal141" runat="server" Text="Interchange Fee Flag" meta:resourcekey="LiteralResource140" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal142" runat="server" Text="Dues & Assessment Flag" meta:resourcekey="LiteralResource141" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal143" runat="server" Text="Dues & Assessments Vol < 1.000" meta:resourcekey="LiteralResource142" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal144" runat="server" Text="Dues & Assessments Item < 1.000" meta:resourcekey="LiteralResource143" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal145" runat="server" Text="Dues & Assessments Vol > 1.000" meta:resourcekey="LiteralResource144" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal146" runat="server" Text="Dues & Assessments Item > 1.000" meta:resourcekey="LiteralResource145" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal147" runat="server" Text="Merchant Pricing Grid" meta:resourcekey="LiteralResource146" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal148" runat="server" Text="Tiered Discount Grid" meta:resourcekey="LiteralResource147" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal149" runat="server" Text="ERR %" meta:resourcekey="LiteralResource148" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal150" runat="server" Text="Other Volume %" meta:resourcekey="LiteralResource149" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal151" runat="server" Text="Other Item Rate" meta:resourcekey="LiteralResource150" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal152" runat="server" Text="Discount Method" meta:resourcekey="LiteralResource151" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal153" runat="server" Text="Amex OnePT Rate" meta:resourcekey="LiteralResource152" />
                                </th>
                                <th>
                                    <asp:Literal ID="Literal154" runat="server" Text="Amex OnePT Per Item Fee" meta:resourcekey="LiteralResource153" />
                                </th>
                            </tr>
                            <tr>
                                <td class="AltRow" colspan="20">
                                    <asp:Literal ID="Literal174" runat="server" Text="No records to display." meta:resourcekey="AS_HierarchyGrid_NoRecord"></asp:Literal>

                                </td>
                            </tr>
                        </table>

                    </asp:PlaceHolder>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>

    <!-- Merchant Card Info PL -->

    <asp:PlaceHolder runat="server" ID="uxMerchantCardPrivateLabelInfo">
        <div class="row" id="privateLabel">
            <div class="col-md-12" data-toggle="collapse" data-target="#uxMerchCardInfoPrivateLabel">
                <a></a>
                <h2 class="grid-title">
                    <asp:Literal ID="Literal249" runat="server" Text="Merchant Card Information - Private Label" meta:resourcekey="LiteralResourceMerchCardInfoPrivateLabel_Title"></asp:Literal>
                </h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 in" id="uxMerchCardInfoPrivateLabel">

                <table class="ASTable" id="Table1">
                    <tr>
                        <th style="width: 80px;"></th>
                        <th style="width: 50px;">
                            <asp:Literal ID="Literal250" runat="server" Text="Proc SW" meta:resourcekey="LiteralResource135"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal251" runat="server" Text="Fee Class" meta:resourcekey="LiteralResource136"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal252" runat="server" Text="Qual Rate" meta:resourcekey="LiteralResource137"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal253" runat="server" Text="MidQual Rate" meta:resourcekey="LiteralResource138"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal254" runat="server" Text="NonQual Rate" meta:resourcekey="LiteralResource139"></asp:Literal>
                        </th>
                        <th style="width: 50px;">
                            <asp:Literal ID="Literal255" runat="server" Text="D&A Flag" meta:resourcekey="LiteralResourceMerchantCardInfo_DAFlag_Header"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="Literal256" runat="server" Text="Disc Method" meta:resourcekey="LiteralResourceMerchantCardInfo_DiscMethod_Header"></asp:Literal>

                        </th>
                        <th>
                            <asp:Literal ID="Literal257" runat="server" Text="Other Vol %" meta:resourcekey="LiteralResourceMerchantCardInfo_OtherVol_Header"></asp:Literal>

                        </th>
                        <th>
                            <asp:Literal ID="Literal258" runat="server" Text="Other Item Rate" meta:resourcekey="LiteralResource150"></asp:Literal>

                        </th>
                        <th>
                            <asp:Literal ID="Literal259" runat="server" Text="Merch Pricing Grid" meta:resourcekey="LiteralResourceMerchantCardInfo_MerchPricingGrid_Header"></asp:Literal>

                        </th>
                        <th>
                            <asp:Literal ID="Literal260" runat="server" Text="Rebate Rate 1" meta:resourcekey="LiteralResourceMerchantCardInfo_RebateRate1_Header"></asp:Literal>

                        </th>
                        <th>
                            <asp:Literal ID="Literal261" runat="server" Text="Rebate Rate 2" meta:resourcekey="LiteralResourceMerchantCardInfo_RebateRate2_Header"></asp:Literal>

                        </th>
                    </tr>
                    <asp:Repeater ID="rptPrivateLabel" runat="server">
                        <ItemTemplate>
                            <tr class="Row">
                                <td>
                                    <%# Eval( "CardType")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("ProcessSW")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("FeeClass")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("QualRate")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("MQualRate")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("NQualRate")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("DuesAssessmentFlag")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("DiscountMethod")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("OtherVolume")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("OtherItemRate")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("MerchantPricingGrid")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("RebateRate1") %>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("RebateRate2") %>&nbsp;
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="AltRow">
                                <td>
                                    <%# Eval( "CardType")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval( "ProcessSW")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval( "FeeClass")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval( "QualRate")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval( "MQualRate")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval( "NQualRate")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval( "DuesAssessmentFlag")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval( "DiscountMethod")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval( "OtherVolume")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval( "OtherItemRate")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval( "MerchantPricingGrid")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("RebateRate1") %>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("RebateRate2") %>&nbsp;
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>

                <asp:PlaceHolder ID="plhPrivateLabelNoRecords" runat="server" Visible="false">
                    <div class="NoRecords">
                        <asp:Literal ID="Literal248" runat="server" Text="No records to display." meta:resourcekey="AS_HierarchyGrid_NoRecord"></asp:Literal>
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
    </asp:PlaceHolder>

    <!-- Pricing Information -->
    <as:PlaceHolder runat="server" ID="PlaceHolder5">
        <!---->
        <div class="row" id="pricinginfo">
            <div class="col-xs-12" data-toggle="collapse" data-target="#ciPricingInformation">
                <h2 class="grid-title">
                    <asp:Literal ID="Literal68" runat="server" Text="Pricing Information" meta:resourcekey="LiteralResource68" /></h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 in" id="ciPricingInformation">
                <asp:Repeater ID="uxPricingInformation" runat="server">
                    <ItemTemplate>
                        <div class="border-wrapper">
                            <table class="ASTable no-border">
                                <tr class="Row valign-bottom pricing-label">
                                    <td rowspan="18" class="heading valign-top text-center">
                                        <asp:Literal ID="Literal268" runat="server" Text="Income Factors" meta:resourcekey="LiteralResource77" /></td>
                                    <td>
                                        <asp:Literal ID="Literal83" runat="server" Text="Daily Deposit" meta:resourcekey="LiteralResource71" /></td>
                                    <td>
                                        <asp:Literal ID="Literal84" runat="server" Text="Weekly Deposit" meta:resourcekey="LiteralResource72" /></td>
                                    <td>
                                        <asp:Literal ID="Literal263" runat="server" Text="Daily Auth Amount" meta:resourcekey="LiteralResource73" /></td>
                                    <td>
                                        <asp:Literal ID="Literal264" runat="server" Text="Avg. Ticket Amount" meta:resourcekey="LiteralResource74" /></td>
                                    <td>
                                        <asp:Literal ID="Literal265" runat="server" Text="Auth Grid" meta:resourcekey="LiteralResource75" /></td>
                                    <td>
                                        <asp:Literal ID="Literal266" runat="server" Text="User Defined Grid" meta:resourcekey="LiteralResource76" /></td>
                                    <td>
                                        <asp:Literal ID="Literal267" runat="server" Text="Merchant Fee Control Grid" meta:resourcekey="LiteralResourcePricing_FeeControl_Header" /></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td>
                                        <%# Eval("DailyDeposit")%>
                                    </td>
                                    <td>
                                        <%# Eval("WeeklyDeposit")%>
                                    </td>
                                    <td>
                                        <%# Eval("DailyAuthAmount")%>
                                    </td>
                                    <td>
                                        <%# Eval("AvgTicketAmount")%>
                                    </td>
                                    <td>
                                        <%# Eval("AuthGrid")%>
                                    </td>
                                    <td>
                                        <%# Eval("UserDefinedGrid")%>
                                    </td>
                                    <td>
                                        <%# Eval("MFCGridID")%>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-label">
                                    <td>
                                        <asp:Literal ID="Literal269" runat="server" Text="Account Fee 1" meta:resourcekey="LiteralResource78" /></td>
                                    <td>
                                        <asp:Literal ID="Literal270" runat="server" Text="Account Fee 2" meta:resourcekey="LiteralResource79" /></td>
                                    <td>
                                        <asp:Literal ID="Literal271" runat="server" Text="Account Fee 3" meta:resourcekey="LiteralResource80" /></td>
                                    <td>
                                        <asp:Literal ID="Literal272" runat="server" Text="Account Fee 4" meta:resourcekey="LiteralResource81" /></td>
                                    <td>
                                        <asp:Literal ID="Literal273" runat="server" Text="Account Fee 5" meta:resourcekey="LiteralResource82" /></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td>
                                        <%# FormatCurrency(Eval("AccountFee1")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("AccountFee2")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("AccountFee3")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("AccountFee4")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("AccountFee5"))%>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-label">
                                    <td>
                                        <asp:Literal ID="Literal274" runat="server" Text="Recurring Fee Flag" meta:resourcekey="LiteralResource83" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal275" runat="server" Text="Recur Fee Ind" meta:resourcekey="LiteralResource84" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal276" runat="server" Text="Recur Fee Amt" meta:resourcekey="LiteralResource85" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal277" runat="server" Text="Recur Fee Desc" meta:resourcekey="LiteralResource86" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal278" runat="server" Text="Stmt Bundle" meta:resourcekey="LiteralResourcePricing_StmtBundle_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal279" runat="server" Text="Bundle Pct" meta:resourcekey="LiteralResource88" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal280" runat="server" Text="Bundle Rate" meta:resourcekey="LiteralResource90" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal281" runat="server" Text="ACH Reject Fee" meta:resourcekey="LiteralResourcePricing_ACHRejectFee_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal282" runat="server" Text="eIDS Fee" meta:resourcekey="LiteralResource_Pricing_eIDSFee_Header" />
                                    </td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td>
                                        <%# Eval("RecurFeeFlag")%>
                                    </td>
                                    <td>
                                        <%# Eval("RecurFeeInd")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("RecurFeeAmt")) %>
                                    </td>
                                    <td>
                                        <%# Eval("RecurFeeDesc")%>
                                    </td>
                                    <td>
                                        <%# Eval("StmtBundleOption")%>
                                    </td>
                                    <td>
                                        <%# Eval("BundlePct")%>
                                    </td>
                                    <td>
                                        <%# Eval("BundleRate")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ACHRejectFee"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("eIDSFee"))%>
                                    </td>
                                </tr>
                                <tr class="Row pricing-label">
                                    <td>
                                        <asp:Literal ID="Literal11" runat="server" Text="Batch Header Fee" meta:resourcekey="LiteralResource90" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal13" runat="server" Text="Return Transaction" meta:resourcekey="LiteralResource91" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal14" runat="server" Text="Chargeback Fee" meta:resourcekey="LiteralResource92" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal15" runat="server" Text="Retrieval Fee" meta:resourcekey="LiteralResource93" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal16" runat="server" Text="Sales Trans Fee" meta:resourcekey="LiteralResource94" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal17" runat="server" Text="Other Volume %" meta:resourcekey="LiteralResource95" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal18" runat="server" Text="Other Item Charge" meta:resourcekey="LiteralResource96" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal19" runat="server" Text="eIDS Indicator" meta:resourcekey="LiteralResourcePricing_eIDSIndicator_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal20" runat="server" Text="Retrieval Fax Flag" meta:resourcekey="LiteralResourcePricing_RetrievalFaxFlag_Header" />
                                    </td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td>
                                        <%# FormatCurrency(Eval("BatchHeaderFee"),4)%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ReturnTransFee"),4)%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ChargebackFee"),4)%>
                                    </td>
                                    <td>
                                        <%# Eval("RetrievalFee")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("SaleTransFee"),4) %>
                                    </td>
                                    <td>
                                        <%# Eval("OtherVolumnPercent")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("OtherItemCharge"),4)%>
                                    </td>
                                    <td>
                                        <%# Eval("eIDSIndicator")%>
                                    </td>
                                    <td>
                                        <%# Eval("RetrievalFaxFlag")%>
                                    </td>
                                </tr>
                                <tr class="Row pricing-label">
                                    <td>
                                        <asp:Literal ID="Literal21" runat="server" Text="Website Usage" meta:resourcekey="LiteralResource97" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal22" runat="server" Text="IVR Usage" meta:resourcekey="LiteralResource98" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal23" runat="server" Text="Unreg Pct" meta:resourcekey="LiteralResource99" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal24" runat="server" Text="Unreg Rate" meta:resourcekey="LiteralResource100" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal25" runat="server" Text="Reg Pct" meta:resourcekey="LiteralResource101" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal26" runat="server" Text="Reg Rate" meta:resourcekey="LiteralResource102" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal28" runat="server" Text="Payeezy Setup Fee" meta:resourcekey="LiteralResourcePricing_PayeezySetupFee_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal29" runat="server" Text="Payeezy Monthly Fee" meta:resourcekey="LiteralResourcePricing_PayeezyMonthlyFee_Header" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td>
                                        <%# FormatCurrency(Eval("WebsiteUsage"),2)%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("IVRUsage"),2)%>
                                    </td>
                                    <td>
                                        <%# Eval("UnregPct")%>
                                    </td>
                                    <td>
                                        <%#  Eval("UnregRate")%>
                                    </td>
                                    <td>
                                        <%# Eval("RegPct")%>
                                    </td>
                                    <td>
                                        <%# Eval("RegRate")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("GGe4SetupFee"),4)%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("GGe4MonthlyFee"),4)%>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-label">
                                    <td>
                                        <asp:Literal ID="Literal30" runat="server" Text="R/C Chg" meta:resourcekey="LiteralResourcePricing_RCChg_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal31" runat="server" Text="Term Chg" meta:resourcekey="LiteralResourcePricing_TermChg_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal32" runat="server" Text="Term Chg Start Date" meta:resourcekey="LiteralResourcePricing_TermChgStartDate_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal33" runat="server" Text="Term Chg Stop Date" meta:resourcekey="LiteralResourcePricing_TermChgStopDate_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal35" runat="server" Text="Imprint Chg" meta:resourcekey="LiteralResourcePricing_ImprintChg_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal36" runat="server" Text="Intra Fee Flg" meta:resourcekey="LiteralResourcePricing_IntraFeeFlg_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal44" runat="server" Text="Float Flg" meta:resourcekey="LiteralResourcePricing_FloatFlg_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal45" runat="server" Text="Acct Chg2 Start Date" meta:resourcekey="LiteralResourcePricing_AcctChg2StartDate_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal46" runat="server" Text="Acct Chg2 Stop Date" meta:resourcekey="LiteralResourcePricing_AcctChg2StopDate_Header" />
                                    </td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td>
                                        <%# FormatCurrency(Eval("RCChg"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("TermChg"))%>
                                    </td>
                                    <td>
                                        <%# Eval("TermChgStartDate")%>
                                    </td>
                                    <td>
                                        <%#  Eval("TermChgStopDate")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ImprintChg"))%>
                                    </td>
                                    <td>
                                        <%# Eval("IntraFeeFlg")%>
                                    </td>
                                    <td>
                                        <%# Eval("FloatFlg")%>
                                    </td>
                                    <td>
                                        <%# Eval("AcctChg2StartDate")%>
                                    </td>
                                    <td>
                                        <%# Eval("AcctChg2StopDate")%>
                                    </td>
                                </tr>
                                <tr class="Row pricing-label">
                                    <td>
                                        <asp:Literal ID="Literal47" runat="server" Text="Print Chg" meta:resourcekey="LiteralResourcePricing_PrintChg_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal48" runat="server" Text="Help Desk Chg" meta:resourcekey="LiteralResourcePricing_HelpDeskChg_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal49" runat="server" Text="Asst Serv Chg" meta:resourcekey="LiteralResourcePricing_AsstServChg_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal50" runat="server" Text="ETC Conf Ltr Chg" meta:resourcekey="LiteralResourcePricing_ETCConfLtrChg_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal51" runat="server" Text="One Time Chg" meta:resourcekey="LiteralResourcePricing_OneTimeChg_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal52" runat="server" Text="Merchant Adv Chg" meta:resourcekey="LiteralResourcePricing_MerchantAdvChg_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal53" runat="server" Text="Ach Chg" meta:resourcekey="LiteralResourcePricing_AchChg_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal54" runat="server" Text="Acct Chg1 Start Date" meta:resourcekey="LiteralResourcePricing_AcctChg1StartDate_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal55" runat="server" Text="Acct Chg1 Stop Date" meta:resourcekey="LiteralResourcePricing_AcctChg1StopDate_Header" />

                                    </td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td>
                                        <%# FormatCurrency(Eval("PrintChg"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("HelpDeskChg"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("AsstServChg"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ETCConfLtrChg"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("OneTimeChg"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("MerchantAdvChg"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ACHChg"))%>
                                    </td>
                                    <td>
                                        <%# Eval("AcctChg1StartDate")%>
                                    </td>
                                    <td>
                                        <%# Eval("AcctChg1StopDate")%>
                                    </td>
                                </tr>
                                <tr class="Row pricing-label">
                                    <td>
                                        <asp:Literal ID="Literal56" runat="server" Text="Min Vol Fee Flag" meta:resourcekey="LiteralResourcePricing_MinVolFeeFlag_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal58" runat="server" Text="Min Vol Fee Chg" meta:resourcekey="LiteralResourcePricing_MinVolFeeChg_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal59" runat="server" Text="One Time Setup Fee Amt" meta:resourcekey="LiteralResourcePricing_OneTimeSetupFeeAmt_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal60" runat="server" Text="Acct Chg3 Start Date" meta:resourcekey="LiteralResourcePricing_AcctChg3StartDate_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal61" runat="server" Text="Acct Chg3 Stop Date" meta:resourcekey="LiteralResourcePricing_AcctChg3StopDate_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal62" runat="server" Text="Acct Chg4 Start Date" meta:resourcekey="LiteralResourcePricing_AcctChg4StartDate_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal63" runat="server" Text="Acct Chg4 Stop Date" meta:resourcekey="LiteralResourcePricing_AcctChg4StopDate_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal64" runat="server" Text="Star Debit Network Fee" meta:resourcekey="LiteralResourcePricing_StarDebitNetworkFee_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal65" runat="server" Text="Pulse Debit Network Fee" meta:resourcekey="LiteralResourcePricing_PulseDebitNetworkFee_Header" />
                                    </td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td>
                                        <%# Eval("MinVolFeeFlag")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("MinVolFeeChg"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("OneTimeSetupFeeAmt"))%>
                                    </td>
                                    <td>
                                        <%#  Eval("AcctChg3StartDate")%>
                                    </td>
                                    <td>
                                        <%# Eval("AcctChg3StopDate")%>
                                    </td>
                                    <td>
                                        <%# Eval("AcctChg4StartDate")%>
                                    </td>
                                    <td>
                                        <%# Eval("AcctChg4StopDate")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("StarDebitNetworkFee"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("PulseDebitNetworkFee"))%>
                                    </td>
                                </tr>
                                <tr class="Row pricing-label">
                                    <td>
                                        <asp:Literal ID="Literal66" runat="server" Text="MFC Other Fee 1" meta:resourcekey="LiteralResourcePricing_MFCOtherFee1_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal67" runat="server" Text="MFC Other Fee 2" meta:resourcekey="LiteralResourcePricing_MFCOtherFee2_Header" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row pricing-value">
                                    <td><%# FormatCurrency(Eval("MFCOtherFee1"))%> </td>
                                    <td><%# FormatCurrency(Eval("MFCOtherFee2"))%></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <%--End Income Factors--%>
                                <tr class="AltRow pricing-label">
                                    <td rowspan="6" class="heading valign-top text-center">
                                        <asp:Literal ID="Literal69" runat="server" Text="Expense Factors:" meta:resourcekey="LiteralResourcePricing_ExpenseFactors_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal70" runat="server" Text="Batch Cst" meta:resourcekey="LiteralResourcePricing_BatchCst_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal77" runat="server" Text="IC Item Cst" meta:resourcekey="LiteralResourcePricing_ICItemCst_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal88" runat="server" Text="Intra Item Cst" meta:resourcekey="LiteralResourcePricing_IntraItemCst_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal89" runat="server" Text="Tape Cst" meta:resourcekey="LiteralResourcePricingTapeCst_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal90" runat="server" Text="Term Cst" meta:resourcekey="LiteralResourcePricingTermCst_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal91" runat="server" Text="R/C Cst" meta:resourcekey="LiteralResourcePricingRCCst_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal92" runat="server" Text="Chgbk Cst" meta:resourcekey="LiteralResourcePricingChgbkCst_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal93" runat="server" Text="Imprnt Cst" meta:resourcekey="LiteralResourcePricingImprntCst_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal94" runat="server" Text="Other Pct Cst" meta:resourcekey="LiteralResourcePricingOtherPctCst_Header" />
                                    </td>
                                </tr>
                                <tr class="AltRow pricing-value">
                                    <td>
                                        <%# FormatCurrency(Eval("BatchCst")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ICItemCst")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("IntraItemCst")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("TapeCst")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("TermCst"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("RCCst"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ChgbkCst"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ImprntCst"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("OtherPctCst"))%>
                                    </td>
                                </tr>
                                <tr class="AltRow pricing-label valign-bottom">
                                    <td>
                                        <asp:Literal ID="Literal95" runat="server" Text="WB Cst M/C" meta:resourcekey="LiteralResourcePricingWBCstMC_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal96" runat="server" Text="WB Cst VS" meta:resourcekey="LiteralResourcePricingWBCstVS_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal97" runat="server" Text="Fix Cst" meta:resourcekey="LiteralResourcePricingFixCst_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal98" runat="server" Text="Item Cst" meta:resourcekey="LiteralResourcePricingItemCst_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal99" runat="server" Text="ETC Item Cst" meta:resourcekey="LiteralResourcePricingETCItemCst_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal100" runat="server" Text="Auth Exp Grid ID" meta:resourcekey="LiteralResourcePricingAuthExpGridID_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal101" runat="server" Text="One Time Cst" meta:resourcekey="LiteralResourcePricingOneTimeCst_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal102" runat="server" Text="12B Ltr Cst" meta:resourcekey="LiteralResourcePricing12BLtrCst_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal103" runat="server" Text="Print Cst" meta:resourcekey="LiteralResourcePricingPrintCst_Header" />

                                    </td>
                                </tr>
                                <tr class="AltRow pricing-value">
                                    <td>
                                        <%# FormatCurrency(Eval("WBCstMC")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("WBCstVS")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("FixCst")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ItemCst")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ETCItemCst"))%>
                                    </td>
                                    <td>
                                        <%# Eval("AuthExpGridID")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("OneTimeCst"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("12BLtrCst"))%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("PrintCst"))%>
                                    </td>
                                </tr>
                                <tr class="AltRow pricing-label valign-bottom">
                                    <td>
                                        <asp:Literal ID="Literal104" runat="server" Text="Help Desk Cst" meta:resourcekey="LiteralResourcePricingHelpDeskCst_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal105" runat="server" Text="Asst Serv Cst" meta:resourcekey="LiteralResourcePricingAsstServCst_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal106" runat="server" Text="ETC Conf Ltr Cst" meta:resourcekey="LiteralResourcePricingETCConfLtrCst_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal107" runat="server" Text="User Dep Exp Grid ID" meta:resourcekey="LiteralResourcePricingUserDepExpGridID_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal108" runat="server" Text="AVS Cst" meta:resourcekey="LiteralResourcePricingAVSCst_Header" />

                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="AltRow pricing-value">
                                    <td>
                                        <%# FormatCurrency(Eval("HelpDeskCst")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("AsstServCst")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ETCConfLtrCst")) %>
                                    </td>
                                    <td>
                                        <%# Eval("UserDepExpGridID") %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("AVSCst"))%>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <%--Begin Visa Assoc Fees:--%>
                                <tr class="Row pricing-label">
                                    <td rowspan="4" class="heading valign-top text-center">
                                        <asp:Literal ID="Literal109" runat="server" Text="Visa Assoc Fees:" meta:resourcekey="LiteralResourcePricingVisaAssocFees_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal110" runat="server" Text="Acqr Pr Fee" meta:resourcekey="LiteralResource104" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal111" runat="server" Text="Misuse Auth Fee" meta:resourcekey="LiteralResource105" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal112" runat="server" Text="ISA" meta:resourcekey="LiteralResource106" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal113" runat="server" Text="Zero Floor" meta:resourcekey="LiteralResource107" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal114" runat="server" Text="Intl Acq" meta:resourcekey="LiteralResource108" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal115" runat="server" Text="Trans Integrity Fee" meta:resourcekey="LiteralResourcePricingTransIntegrityFee_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal116" runat="server" Text="FANF Fee Flag" meta:resourcekey="LiteralResourcePricingFANFFeeFlag_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal117" runat="server" Text="FANF CP Surcharge" meta:resourcekey="LiteralResourcePricingFANFCPSurcharge_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal119" runat="server" Text="FANF CNP Surcharge" meta:resourcekey="LiteralResourcePricingFANFCNPSurcharge_Header" />

                                    </td>
                                </tr>
                                <tr class="Row visa pricing-value">
                                    <td>
                                        <%# Eval("AcqrPrFee")%>
                                    </td>
                                    <td>
                                        <%# Eval("MisuseAuthFee")%>
                                    </td>
                                    <td>
                                        <%# Eval("ISA")%>
                                    </td>
                                    <td>
                                        <%# Eval("ZeroFloor")%>
                                    </td>
                                    <td>
                                        <%# Eval("IntlAcq")%>
                                    </td>
                                    <td id="TransIntegrityFee" runat="server">
                                        <span>
                                            <%# Eval("TransIntegrityFee")%></span>
                                        <tek:RadToolTip ID="RadToolTip78" runat="server" Visible='<%#Eval("TransIntegrityFee")!=DBNull.Value %>' TargetControlID="TransIntegrityFee" RelativeTo="Element"
                                            Position="Center" RenderInPageRoot="true">
                                            <%# ExportTooltip(Eval("TransIntegrityFee").ToString(), 1)%>
                                        </tek:RadToolTip>
                                    </td>
                                    <td id="NPFFeeFlag" runat="server">
                                        <span>
                                            <%# Eval("NPFFeeFlag")%></span>
                                        <tek:RadToolTip ID="RadToolTip79" runat="server" Visible='<%#Eval("NPFFeeFlag")!=DBNull.Value %>' TargetControlID="NPFFeeFlag" RelativeTo="Element"
                                            Position="Center" RenderInPageRoot="true">
                                            <%# ExportTooltip(Eval("NPFFeeFlag").ToString(), 2)%>
                                        </tek:RadToolTip>
                                    </td>
                                    <td>
                                        <%# Eval("NPFCPSurcharge")%>
                                    </td>
                                    <td>
                                        <%# Eval("NPFCNPSurcharge")%>
                                    </td>
                                </tr>
                                <tr class="Row visa pricing-label valign-bottom">
                                    <td>
                                        <asp:Literal ID="Literal156" runat="server" Text="Kilobyte Fee Indicator" meta:resourcekey="LiteralResourcePricingKilobyteFeeIndicator_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal157" runat="server" Text="Kilobyte Surcharge" meta:resourcekey="LiteralResourcePricingKilobyteSurcharge_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal177" runat="server" Text="Process Fee Income" meta:resourcekey="LiteralResourcePricingProcessFeeIncome_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal178" runat="server" Text="Process Fee Expense" meta:resourcekey="LiteralResourcePricingProcessFeeExpense_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal179" runat="server" Text="BIN/ICA Fee Income" meta:resourcekey="LiteralResourcePricingBINICAFeeIncome_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal180" runat="server" Text="BIN/ICA Fee Expense" meta:resourcekey="LiteralResourcePricingBINICAFeeExpense_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal181" runat="server" Text="AFD Non-Participation Fee" meta:resourcekey="LiteralResourcePricingAFDNonParticipationFee_Header" />

                                    </td>
                                    <td></td>
                                    <td></td>

                                </tr>
                                <tr class="Row visa pricing-value">
                                    <td>
                                        <%# Eval("KilobyteFeeIndicator")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("KilobyteSurcharge")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ProcessFeeExpense")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("ProcessFeeIncome")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("BINICAFeeIncome")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("BINICAFeeExpense")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("AFDNonPartFee")) %>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <%--MC Assoc Fees:--%>
                                <tr class="AltRow pricing-label">
                                    <td rowspan="6" class="heading valign-top text-center">
                                        <asp:Literal ID="Literal182" runat="server" Text="MC Assoc Fees:" meta:resourcekey="LiteralResourcePricingMCAssocFees_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal210" runat="server" Text="NABU Fee" meta:resourcekey="LiteralResource110" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal211" runat="server" Text="Cross Border Fee" meta:resourcekey="LiteralResource111" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal212" runat="server" Text="Acq Support Fee" meta:resourcekey="LiteralResource112" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal213" runat="server" Text="Reversal Fee" meta:resourcekey="LiteralResource113" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal214" runat="server" Text="Kilobyte Fee Indicator" meta:resourcekey="LiteralResourcePricingKilobyteFeeIndicator_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal215" runat="server" Text="Kilobyte Surcharge" meta:resourcekey="LiteralResourcePricingKilobyteSurcharge_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal216" runat="server" Text="Process Fee Income" meta:resourcekey="LiteralResourcePricingProcessFeeIncome_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal217" runat="server" Text="Process Fee Expense" meta:resourcekey="LiteralResourcePricingProcessFeeExpense_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal218" runat="server" Text="BIN/ICA Fee Income" meta:resourcekey="LiteralResourcePricingBINICAFeeIncome_Header" />
                                    </td>
                                </tr>
                                <tr class="AltRow mc pricing-value">
                                    <td>
                                        <%# Eval("NABUFee")%>
                                    </td>
                                    <td>
                                        <%# Eval("CrossBorderFee")%>
                                    </td>
                                    <td>
                                        <%# Eval("AcqSupportFee")%>
                                    </td>
                                    <td>
                                        <%# Eval("ReversalFee")%>
                                    </td>
                                    <td>
                                        <%# Eval("MC_KilobyteFeeIndicator")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("MC_KilobyteSurcharge")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("MC_ProcessFeeIncome")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("MC_ProcessFeeExpense")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("MC_BINICAFeeIncome")) %>
                                    </td>
                                </tr>
                                <tr class="AltRow mc pricing-label valign-bottom">
                                    <td>
                                        <asp:Literal ID="Literal219" runat="server" Text="BIN/ICA Fee Expense" meta:resourcekey="LiteralResourcePricingBINICAFeeExpense_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal220" runat="server" Text="CVC2 Fee Indicator" meta:resourcekey="LiteralResourcePricingCVC2FeeIndicator_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal221" runat="server" Text="CVC2 Surcharge" meta:resourcekey="LiteralResourcePricingCVC2FeeSurcharge_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal222" runat="server" Text="License Per Item Income" meta:resourcekey="LiteralResourcePricingLicensePerItemIncome_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal223" runat="server" Text="License Per Item Expense" meta:resourcekey="LiteralResourcePricingLicensePerItemExpense_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal224" runat="server" Text="License Rate Income" meta:resourcekey="LiteralResourcePricingLicenseRateIncome_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal225" runat="server" Text="License Rate Expense" meta:resourcekey="LiteralResourcePricingLicenseRateExpense_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal226" runat="server" Text="License Flat Income" meta:resourcekey="LiteralResourcePricingLicenseFlatIncome_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal227" runat="server" Text="License Flat Expense" meta:resourcekey="LiteralResourcePricingLicenseFlatExpense_Header" />
                                    </td>
                                </tr>
                                <tr class="AltRow mc pricing-value">
                                    <td>
                                        <%# FormatCurrency(Eval("MC_BINICAFeeExpense")) %>
                                    </td>
                                    <td>
                                        <%# Eval("CVC2FeeIndicator")%>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("CVC2Surcharge")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("LicensePerItemIncome")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("LicensePerItemExpense")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("LicenseRateIncome")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("LicenseRateExpense")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("LicenseFlatIncome")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("LicenseFlatExpense")) %>
                                    </td>
                                </tr>
                                <tr class="AltRow mc pricing-label valign-bottom">
                                    <td>
                                        <asp:Literal ID="Literal228" runat="server" Text="License Flat Occurrence Indicator" meta:resourcekey="LiteralResourcePricingLicenseFlatOccurrenceIndicator_Header" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal229" runat="server" Text="MC ICA AVS Income" meta:resourcekey="LiteralResourcePricingMCICAAVSIncome_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal230" runat="server" Text="MC ICA AVS Expense" meta:resourcekey="LiteralResourcePricingMCICAAVSExpense_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal231" runat="server" Text="AVS Acquirer MC Fee" meta:resourcekey="LiteralResourcePricingAVSAcquirerMCFee_Header" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="AltRow mc pricing-value">
                                    <td>
                                        <%# Eval("LicenseFlatOccurrenceIndicator")%>
                                    </td>
                                    <td>
                                        <%# Format_MC_ICA_AVS(Eval("MCICAAVSIncome")) %>
                                    </td>
                                    <td>
                                        <%# Format_MC_ICA_AVS(Eval("MCICAAVSExpense")) %>
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("AVSAcquirerMCFee")) %>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <%--Begin Disc Full Acq Assoc:--%>
                                <tr class="Row pricing-label">
                                    <td rowspan="2" class="heading valign-top text-center">
                                        <asp:Literal ID="Literal232" runat="server" Text="Disc Full Acq Assoc:" meta:resourcekey="LiteralResourcePricingDiscFullAcqAssoc_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal233" runat="server" Text="INTL Process Flag" meta:resourcekey="LiteralResource115" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal234" runat="server" Text="INTL Service Flag" meta:resourcekey="LiteralResource116" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal235" runat="server" Text="Data Usage Flag" meta:resourcekey="LiteralResource117" />

                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal236" runat="server" Text="Auth Fee flag" meta:resourcekey="LiteralResourcePricingAuthFeeflag_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal237" runat="server" Text="Auth Surcharge" meta:resourcekey="LiteralResourcePricingAuthSurcharge_Header" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="Row acq pricing-value">
                                    <td id="INTLProcessFlag" runat="server">
                                        <span>
                                            <%# Eval("INTLProcessFlag")%></span>
                                        <tek:RadToolTip ID="RadToolTip75" runat="server" Visible='<%#Eval("INTLProcessFlagDesc")!=DBNull.Value %>' TargetControlID="INTLProcessFlag" RelativeTo="Element"
                                            Position="Center" RenderInPageRoot="true">
                                            <%# Eval("INTLProcessFlagDesc")%>
                                        </tek:RadToolTip>
                                    </td>
                                    <td id="INTLServiceFlag" runat="server">
                                        <span>
                                            <%# Eval("INTLServiceFlag")%></span>
                                        <tek:RadToolTip ID="RadToolTip76" runat="server" Visible='<%#Eval("INTLServiceFlagDesc")!=DBNull.Value %>' TargetControlID="INTLServiceFlag" RelativeTo="Element"
                                            Position="Center" RenderInPageRoot="true">
                                            <%# Eval("INTLServiceFlagDesc")%>
                                        </tek:RadToolTip>
                                    </td>
                                    <td id="DataUsageFlag" runat="server">
                                        <span>
                                            <%# Eval("DataUsageFlag")%></span>
                                        <tek:RadToolTip ID="RadToolTip77" runat="server" Visible='<%#Eval("DataUsageFlagDesc")!=DBNull.Value %>' TargetControlID="DataUsageFlag" RelativeTo="Element"
                                            Position="Center" RenderInPageRoot="true">
                                            <%# Eval("DataUsageFlagDesc")%>
                                        </tek:RadToolTip>
                                    </td>
                                    <td>
                                        <%# Eval("AuthFeeFlag")%>
                                       
                                    </td>
                                    <td>
                                        <%# FormatCurrency(Eval("AuthSurcharge"))%>                                                                                                                         
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <%--Beign AMEX Assoc Fees:--%>
                                <tr class="AltRow pricing-label">
                                    <td rowspan="2" class="heading valign-top text-center">
                                        <asp:Literal ID="Literal238" runat="server" Text="AMEX Assoc Fees:" meta:resourcekey="LiteralResourcePricingAMEXAssocFees_Header" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal239" runat="server" Text="AVS Network Fee" meta:resourcekey="LiteralResourcePricingAMEXNetworkFee_Header" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="AltRow amex pricing-value">
                                    <td><%# FormatCurrency(Eval("AVSNetworkFee")) %></td>
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
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </as:PlaceHolder>
</asp:PlaceHolder>

<as:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        $("table#merchantcardinformation tr[class!='nobackground']:odd").addClass('AltRow');
        $("table#merchantcardinformation tr[class!='nobackground']:even").addClass('Row');
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MIF_MerchantDetails_CAYAN.js"></script>
</as:RadCodeBlock>
