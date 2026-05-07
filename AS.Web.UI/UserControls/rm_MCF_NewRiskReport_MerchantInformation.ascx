<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_NewRiskReport_MerchantInformation.ascx.cs" Inherits="UserControls_rm_MCF_NewRiskReport_MerchantInformation" %>

<as:RadAjaxManagerProxy ID="RadAjaxProxy" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxProfile">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxProfile" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxWatch">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPnlWatch" />
                <tek:AjaxUpdatedControl ControlID="uxWatch" />
                <tek:AjaxUpdatedControl ControlID="uxMultiWatchList" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxWorked">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPnWorked" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxShowModalDetail">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxShowModalDetail" LoadingPanelID="uxInvisiblePanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>

</as:RadAjaxManagerProxy>
<div id="uxMerchantInformationGrid" class="in">
    <%-----------------------Merchant info--------------------------------%>
    <div class="row" id="merchinfo">
        <div class="col-xs-9" data-toggle="collapse" data-target="#uxMerchantInformationGrid">
            <h2 class="grid-title on-top mb-1x text-nowrap">
                <span class="text-muted" style="margin-top: 0 !important">
                    <as:Literal ID="uxMerchantName" runat="server"></as:Literal>

                    <as:PlaceHolder ID="uxPlhSwitchMID" runat="server">
                        <asp:HyperLink ID="uxSwitchMID" runat="server" CssClass="font-size-default link-back link ml-4x" meta:resourcekey="uxLiteralSwitchToTSYSResource" Text="Switch to TSYS MID"></asp:HyperLink>
                    </as:PlaceHolder>
                </span>
            </h2>

            <h2 class="grid-title">
                <as:Literal ID="Literal13" runat="server" Text="Merchant Information" meta:resourcekey="Literal27Resource1"></as:Literal>
            </h2>

        </div>
        <div class="col-xs-3 text-right">
            <h2 class="grid-title on-top mb-1x text-nowrap">
                <asp:HyperLink ID="uxGoBack" runat="server" NavigateUrl="#" Visible="false" Text=""
                    CssClass="font-size-default link-back link ml-4x" meta:resourcekey="uxGoBackResource1" />
            </h2>
        </div>
    </div>
    <div class="height-12"></div>
    <div class="row">

        <div class="col-md-12">
            <div class="status-merchant">
                <div class="row">
                    <div class="col-md-3" id="divWatch">
                        <span class="title">
                            <as:Literal ID="Literal18" runat="server" Text="Watch:" meta:resourcekey="Literal18Resource1"></as:Literal></span>
                        <div class="inline-block" id="ciPnlWatch">
                            <div runat="server" id="uxPnlWatch">
                                <as:CheckBox ID="uxWatch" AutoPostBack="true" runat="server" meta:resourcekey="uxWatchResource1" OnCheckedChanged="uxChecked_OnClick" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3" id="divStatus">
                        <span class="title">
                            <as:Literal ID="Literal11" runat="server" Text="Status:" meta:resourcekey="Literal11Resource1"></as:Literal></span>
                        <as:Literal ID="uxStatus" runat="server" meta:resourcekey="uxStatusResource1" />
                    </div>
                    <div class="col-md-3 hide" id="divFundingStatus">
                        <span class="title">
                            <as:Literal ID="Literal15" runat="server" Text="Funding Status:" meta:resourcekey="Literal15Resource1"></as:Literal></span>
                        <as:Literal ID="uxFundingStatus" runat="server" meta:resourcekey="uxFundingStatusResource1" />
                    </div>
                    <div class="col-md-3" id="divProfile">
                        <span class="title">
                            <as:Literal ID="Literal9" runat="server" Text="Profile:" meta:resourcekey="Literal9Resource1"></as:Literal></span>
                        <div class="inline-block">
                            <as:RadComboBox ID="uxProfile" runat="server" EnableEmbeddedBaseStylesheet="false" AutoPostBack="true"
                                DataValueField="DataKey" DataTextField="DataText" Width="200px" OnSelectedIndexChanged="uxSelected_OnClick">
                            </as:RadComboBox>
                            <as:Literal ID="uxProfileReadOnly" runat="server" meta:resourcekey="uxProfileReadOnlyResource1" />
                        </div>
                    </div>
                    <div class="col-md-3" id="divMultiWatch">
                        <span class="title">
                            <as:Literal ID="Literal19" runat="server" Text="Multi-Watch:" meta:resourcekey="Literal19Resource1"></as:Literal></span>
                        <div class="inline-block none-available">
                            <as:Literal ID="uxMultiWatchList" runat="server" Text="N/A" meta:resourcekey="uxMultiWatchListResource1" />
                        </div>
                    </div>
                    <div class="col-md-3" id="divWorked">
                        <span class="title">
                            <as:Literal ID="Literal20" runat="server" Text="# Worked:" meta:resourcekey="Literal20Resource"></as:Literal></span>
                        <div class="inline-block" id="ciPnWorked">
                            <div runat="server" id="uxPnWorked">
                                <as:Literal ID="uxWorked" runat="server" Text="N/A" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3" id="divParameterWorked">
                        <span class="title">
                            <as:Literal ID="Literal27" runat="server" Text="# Parameter Worked:" meta:resourcekey="Literal25Resource"></as:Literal></span>
                        <div class="inline-block" id="Div2">
                            <div runat="server" id="Div3">
                                <as:Literal ID="uxParameterWorked" runat="server" Text="N/A" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3" id="divWKD30">
                        <span class="title">
                            <as:Literal ID="Literal25" runat="server" Text="#Worked in 30 Days:" meta:resourcekey="Literal24Resource1"></as:Literal></span>
                        <div class="inline-block">
                            <as:Literal ID="uxWKD30" runat="server" />
                        </div>
                    </div>
                </div>
                <as:PlaceHolder ID="uxPlhRiskLevel" runat="server" Visible="false">
                    <div class="col-md-3" id="divRiskLevel">
                        <span class="title">
                            <as:Literal ID="uxLabelRiskLevel" runat="server" Text="Risk Level:" meta:resourcekey="RiskLevel"></as:Literal>
                        </span>
                        <as:Literal ID="uxRiskLevel" runat="server"></as:Literal>
                    </div>
                </as:PlaceHolder>
            </div>
        </div>
        <div id="pnlClassificationRiskScore" class="col-md-12" runat="server" visible="false">
            <div class="data-merchant width-auto">
                <tek:RadCodeBlock runat="server">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="item">
                                <label class="pb-3x-im">
                                    <as:Literal ID="Literal21" runat="server" Text="Classification" meta:resourcekey="ltrClassificationResource"></as:Literal></label>
                                <%= BindValue("MerchantClassificationName")%>
                            </div>

                            <div class="item" id="uxGVerifyCode" runat="server" visible="false">
                                <label class="pb-3x-im">
                                    <as:Literal ID="Literal32" runat="server" Text="Gverify Code" meta:resourcekey="ltrGverifyCodeResource"></as:Literal></label>
                                <%= BindValue("GVerifyCode")%>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="item">
                                <label class="pb-3x-im">
                                    <as:Literal ID="Literal22" runat="server" Text="Multiplier" meta:resourcekey="ltrMultiplierResource"></as:Literal>
                                </label>
                                <%= BindValue("Multiplier")%>
                            </div>

                            <div class="item" id="uxGAuthenticateCode" runat="server" visible="false">
                                <label class="pb-3x-im">
                                    <as:Literal ID="Literal33" runat="server" Text="Gauthenticate Code:" meta:resourcekey="ltrGauthenticateCodeResource"></as:Literal></label>
                                <%= BindValue("GAuthenticateCode")%>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="item">
                                <label class="pb-3x-im">
                                    <as:Literal ID="Literal23" runat="server" Text="Total Risk Score:" meta:resourcekey="ltrTotalRiskScoreResource"></as:Literal>
                                </label>
                                <as:PlaceHolder ID="uxLinkTotalRS" runat="server" Visible='true'>
                                    <a href="#" runat="server" id="lnkTotalRS"><%= BindValue("TotalRS")%></a>
                                </as:PlaceHolder>
                                <as:Literal ID="uxTextTotalRS" runat="server" Visible="false"></as:Literal>
                            </div>

                            <div class="item" id="uxG2CompassAutoApprovalIndicator" runat="server" visible="false">
                                <label class="pb-3x-im">
                                    <as:Literal ID="Literal34" runat="server" Text="G2 Compass Auto Approval Indicator:" meta:resourcekey="ltrAutoApprovalIndicatorResource"></as:Literal></label>
                                <%= BindValue("G2CompassAutoApprovalIndicator")%>
                            </div>
                        </div>
                    </div>
                </tek:RadCodeBlock>
            </div>
        </div>
    </div>
    <div class="data-merchant">
        <div class="row">
            <div class="col-md-3">
                <div class="item" runat="server" id="divHrCode" visible="false">
                    <label>
                        <as:Literal ID="ltHRCode" runat="server" Text="HR Code:" meta:resourcekey="LiteralHRCodeResource1"></as:Literal></label>
                    <as:Literal ID="uxHrCode" runat="server" meta:resourcekey="uxHRCodeResource1" />
                </div>
                <div class="item" runat="server" id="divACHDelay" visible="false">
                    <label>
                        <as:Literal ID="ltACHDelay" runat="server" Text="ACH Delay:" meta:resourcekey="ltACHDelayResource1"></as:Literal></label>
                    <as:Literal ID="uxACHDelay" runat="server" meta:resourcekey="uxACHDelayResource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal ID="Literal10" runat="server" Text="Appr. Date:" meta:resourcekey="Literal3Resource1"></as:Literal></label>
                    <as:Literal ID="uxApprovalDate" runat="server" meta:resourcekey="uxApprovalDateResource1" />
                </div>

                <%--42397 - VW - MCPS - Add new Parameter P176 for First Batch Rule --%>
                <as:PlaceHolder ID="uxPlFirstBatch" runat="server">
                    <div class="item">
                        <label>
                            <as:Literal ID="Literal24" runat="server" Text="First Batch Amount:" meta:resourcekey="Literal20Resource1"></as:Literal></label>
                        <as:Literal ID="uxFirstBatchAmount" runat="server" meta:resourcekey="Literal20Resource1" />
                    </div>
                    <div class="item">
                        <label>
                            <as:Literal ID="Literal26" runat="server" Text="First Batch Date:" meta:resourcekey="Literal21Resource1"></as:Literal></label>
                        <as:Literal ID="uxFirstBatchDate" runat="server" meta:resourcekey="Literal21Resource1" />
                    </div>
                </as:PlaceHolder>

                <as:PlaceHolder ID="uxPlhConversionDate" runat="server" Visible="false">
                    <div class="item">
                        <label>
                            <asp:Literal ID="Literal3" runat="server" Text="Conversion Date:" meta:resourcekey="ConversionDateResource" /></label>
                        <as:Literal ID="uxConversionDate" runat="server" />
                    </div>
                </as:PlaceHolder>

                <div class="item">
                    <label>
                        <as:Literal ID="Literal4" runat="server" Text="Last Batch:" meta:resourcekey="Literal4Resource1"></as:Literal></label>
                    <as:Literal ID="uxLastActiveDate" runat="server" meta:resourcekey="uxLastActiveDateResource1" />
                </div>
                <as:PlaceHolder ID="uxLastStatement" runat="server">
                    <div class="item">
                        <label>
                            <as:Literal ID="Literal7" runat="server" Text="Last Statement:" meta:resourcekey="Literal7Resource1"></as:Literal></label>
                        <as:Literal ID="uxLastStatementDate" runat="server" meta:resourcekey="uxLastStatementDateResource1" />
                    </div>
                </as:PlaceHolder>
                <div runat="server" id="pnlMarketData" class="item">
                    <label>
                        <as:Literal runat="server" ID="Literal2" Text="Market Data:" meta:resourcekey="Literal2Resource1" /></label>
                    <as:Literal ID="uxMarketData" runat="server" meta:resourcekey="uxMarketDataResource1" />
                </div>
                <div runat="server" id="pnlRelationshipManager" class="item">
                    <label>
                        <as:Literal runat="server" ID="Literal1" Text="Relationship Manager:" meta:resourcekey="Literal1Resource1" />
                    </label>
                    <as:Literal ID="uxRelationshipManager" runat="server" meta:resourcekey="uxRelationshipManagerResource1" />
                </div>

                <div class="item" id="uxFutureDeliveryIndicator" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal30" meta:resourcekey="uxFutureDeliveryIndicatorResource1" Text="Future Delivery Indicator:" /></label>
                    <as:Literal ID="lblFutureDeliveryIndicator" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>

                <div class="item" id="uxFutureDeliveryDayMaximum" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal31" meta:resourcekey="uxFutureDeliveryDayMaximumResource1" Text="Future Delivery Day Maximum:" /></label>
                    <as:Literal ID="lblFutureDeliveryDayMaximum" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>

                <div class="item" id="uxActualDeliveryDaysItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal68" meta:resourcekey="uxActualDeliveryDaysResource1" Text="Actual Delivery Days:" /></label>
                    <as:Literal ID="uxActualDeliveryDays" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxStatusDateItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal37" meta:resourcekey="uxStatusDateResource1" Text="Status Date:" /></label>
                    <as:Literal ID="uxStatusDate" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxRiskLevelMIItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal38" meta:resourcekey="uxRiskLevelMIResource1" Text="Risk Level:" /></label>
                    <as:Literal ID="uxRiskLevelMI" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxDescGoodServiceItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal40" meta:resourcekey="uxDescGoodServiceResource1" Text="Description of goods and services:" /></label>
                    <as:Literal ID="uxDescGoodService" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
            </div>
            <div class="col-md-5" id="uxSecondCol">
                <div class="item">
                    <label>
                        <as:Literal ID="Literal12" runat="server" Text="SIC/MCC:" meta:resourcekey="Literal12Resource1"></as:Literal></label>
                    <as:Literal ID="uxSIC" runat="server" meta:resourcekey="uxSICResource1" />
                </div>
                <div class="item hide" id="divCreditScore">
                    <label>
                        <as:Literal ID="Literal16" runat="server" Text="Credit Score:" meta:resourcekey="Literal16Resource1"></as:Literal></label>
                    <as:Literal ID="uxCreditScore" runat="server" meta:resourcekey="uxSICResource1" />
                </div>
                <div runat="server" id="pnlSysPrinAgent" class="item" visible="true">
                    <label>
                        <as:Literal ID="ltSYSPRINAgent" runat="server" Text="SYS/PRIN/Agent:" meta:resourcekey="ltSYSPRINAgentResource1"></as:Literal></label>
                    <as:Literal ID="uxSYSPRINAgent" runat="server" meta:resourcekey="uxSYSPRINAgentResource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal ID="Literal6" runat="server" Text="Owner:" meta:resourcekey="Literal6Resource1"></as:Literal></label>
                    <ul class="ownerList">
                        <li>
                            <as:Literal ID="uxOwner" runat="server" meta:resourcekey="uxOwnerResource1" /></li>
                        <li>
                            <as:Literal ID="uxOwner2" runat="server" Visible="false"></as:Literal></li>
                        <li>
                            <as:Literal ID="uxOwner3" runat="server" Visible="false"></as:Literal></li>
                        <li>
                            <as:Literal ID="uxOwner4" runat="server" Visible="false"></as:Literal></li>
                        <li>
                            <as:Literal ID="uxOwner5" runat="server" Visible="false"></as:Literal></li>
                    </ul>
                </div>
                <div class="item">
                    <label>
                        <as:Literal ID="Literal8" runat="server" Text="Phone:" meta:resourcekey="Literal8Resource1"></as:Literal></label>
                    <as:Literal ID="uxPhone" runat="server" meta:resourcekey="uxPhoneResource1" />
                </div>
                <%--TK25937 – FIS - Merchant Profile/ Risk Report Enhancement--%>
                <div runat="server" id="pnlEmail" class="item">
                    <label>
                        <as:Literal ID="Literal14" runat="server" Text="Email:" meta:resourcekey="Literal14Resource1"></as:Literal></label>
                    <as:Literal ID="uxEmail" runat="server" meta:resourcekey="uxEmailResource1" />
                </div>
                <%--End TK25937 – FIS - Merchant Profile/ Risk Report Enhancement--%>
                <div class="item">
                    <label>
                        <as:Literal ID="Literal5" runat="server" Text="Address:" meta:resourcekey="Literal5Resource1"></as:Literal>
                    </label>
                    <as:Literal ID="uxAddress" runat="server" meta:resourcekey="uxAddress1Resource1" />
                </div>
                <div class="item" id="divUrl">
                    <label>
                        <as:Literal ID="Literal17" runat="server" Text="URL:" meta:resourcekey="Literal17Resource1"></as:Literal>
                    </label>
                    <as:Literal ID="uxUrl" runat="server" meta:resourcekey="uxAddress1Resource1" /><br />
                    <asp:LinkButton ID="uxLinkButtonViewWebSites" runat="server" CssClass="heading link-back fw-normal"
                        OnClientClick="ShowPopupModal('../WebsiteModal.aspx','auto'); return false;" Text="View All Websites" meta:resourcekey="ViewAllWebsites" />
                </div>

                <div class="item" id="uxSeasonalIndicator" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal35" meta:resourcekey="uxSeasonalIndicatorResource1" Text="Seasonal Indicator:" /></label>
                    <as:Literal ID="lblSeasonalIndicator" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>

                <div class="item" id="uxSeasonalActiveMonths" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal36" meta:resourcekey="uxSeasonalActiveMonthsResource1" Text="Seasonal Active Months:" /></label>
                    <as:Literal ID="lblSeasonalActiveMonths" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxSubMCCItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal39" meta:resourcekey="uxSubMCCResource1" Text="SubMCC:" /></label>
                    <as:Literal ID="uxSubMCC" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxAdvDepositItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal42" meta:resourcekey="uxAdvDepositResource1" Text="Adv. Deposit %:" /></label>
                    <as:Literal ID="uxAdvDeposit" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxStatusNameItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal44" meta:resourcekey="uxStatusNameResource1" Text="Status Name:" /></label>
                    <as:Literal ID="uxStatusName" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>

            </div>
            <div class="col-md-3" id="uxThirdCol" runat="server" visible="false">
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="Literal41" meta:resourcekey="uxReserveIndicatorResource1" /></label>
                    <as:Literal ID="uxReserveIndicator" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="Literal45" meta:resourcekey="uxReserveTargetResource1" /></label>
                    <as:Literal ID="uxReserveTarget" runat="server" meta:resourcekey="uxHierarchyValue2Resource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="Literal47" meta:resourcekey="uxReservePercentResource1" /></label>
                    <as:Literal ID="uxReservePercent" runat="server" meta:resourcekey="uxHierarchyValue2Resource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="Literal51" meta:resourcekey="uxEsquireDirectAcctResource1" /></label>
                    <as:Literal ID="uxEsquireDirectAcct" runat="server" meta:resourcekey="uxHierarchyValue4Resource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="Literal53" meta:resourcekey="uxAgentNameResource1" /></label>
                    <as:Literal ID="uxAgentName" runat="server" meta:resourcekey="uxHierarchyValue5Resource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="Literal55" meta:resourcekey="uxHighRiskRegistrationResource1" /></label>
                    <as:Literal ID="uxHighRiskRegistration" runat="server" meta:resourcekey="uxHierarchyValue5Resource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="Literal57" meta:resourcekey="uxNegativeDatabaseResource1" /></label>
                    <as:Literal ID="uxNegativeDatabase" runat="server" meta:resourcekey="uxHierarchyValue5Resource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="Literal59" meta:resourcekey="uxApprovedMCCResource1" /></label>
                    <as:Literal ID="uxApprovedMCC" runat="server" meta:resourcekey="uxHierarchyValue5Resource1" />
                </div>
            </div>
            <div class="col-md-4" id="uxLastCol">
                <div class="item" id="uxRiskRatingItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal46" meta:resourcekey="uxRiskRatingResource1" /></label>
                    <as:Literal ID="uxRiskRating" runat="server" meta:resourcekey="uxRiskRatingValue1Resource1" />
                </div>
                <div class="item" id="uxHierarchy1Item" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy1" meta:resourcekey="uxHierarchy1Resource1" /></label>
                    <as:Literal ID="uxHierarchyValue1" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxHierarchy2Item" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy2" meta:resourcekey="uxHierarchy2Resource1" /></label>
                    <as:Literal ID="uxHierarchyValue2" runat="server" meta:resourcekey="uxHierarchyValue2Resource1" />
                </div>
                <div class="item" id="uxHierarchy3Item" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy3" meta:resourcekey="uxHierarchy3Resource1" />
                        <as:Literal runat="server" ID="uxServicedByLabel" Visible="False" meta:resourcekey="uxServicedByLabelResource1" /></label>
                    <as:Literal ID="uxHierarchyValue3" runat="server" meta:resourcekey="uxHierarchyValue3Resource1" />
                    <as:Literal ID="uxServicedByValue" runat="server" Visible="False" meta:resourcekey="uxServicedByValueResource1" />
                </div>
                <div class="item" id="uxHierarchy4Item" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy4" meta:resourcekey="uxHierarchy4Resource1" /></label>
                    <as:Literal ID="uxHierarchyValue4" runat="server" meta:resourcekey="uxHierarchyValue4Resource1" />
                </div>
                <div class="item" id="uxHierarchy5Item" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy5" meta:resourcekey="uxHierarchy5Resource1" /></label>
                    <as:Literal ID="uxHierarchyValue5" runat="server" meta:resourcekey="uxHierarchyValue5Resource1" />
                </div>
                <div class="item" id="uxHierarchy6Item" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy6" meta:resourcekey="uxHierarchy5Resource1" /></label>
                    <as:Literal ID="uxHierarchyValue6" runat="server" meta:resourcekey="uxHierarchyValue5Resource1" />
                </div>


                <div class="item" id="uxDaystoFund" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal28" meta:resourcekey="uxDaystoFundDaysResource1" Text="Days to Fund:" /></label>
                    <as:Literal ID="lblDaystoFund" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>

                <div class="item" id="uxFICOScore" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal29" meta:resourcekey="uxFICOScoreResource1" Text="FICO Score:" /></label>
                    <as:Literal ID="lblFICOScore" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxExpeditedFundingItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal43" meta:resourcekey="uxExpeditedFundingResource1" Text="Expedited Funding:" /></label>
                    <as:Literal ID="uxExpeditedFunding" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxAdvDepositsDaysItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal48" meta:resourcekey="uxAdvDepositsDaysResource1" Text="Adv. Deposits Days:" /></label>
                    <as:Literal ID="uxAdvDepositsDays" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxApprovedNDXDaysItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal50" meta:resourcekey="uxApprovedNDXDaysResource1" Text="Approved NDX Days:" /></label>
                    <as:Literal ID="uxApprovedNDXDays" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxOriginallyBoardedAnnualVolumeItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal160" meta:resourcekey="uxOriginallyBoardedAnnualVolumeResource1" Text="Originally Boarded Annual Volume:" /></label>
                    <as:Literal ID="uxOriginallyBoardedAnnualVolume" runat="server" meta:resourcekey="uxOriginallyBoardedAnnualVolumeValueResource1" />
                </div>
                <div class="item" id="uxOriginallyBoardedAverageTicketItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal162" meta:resourcekey="uxOriginallyBoardedAverageTicketResource1" Text="Originally Boarded Average Ticket:" /></label>
                    <as:Literal ID="uxOriginallyBoardedAverageTicket" runat="server" meta:resourcekey="uxOriginallyBoardedAverageTicketValueResource1" />
                </div>
                <div class="item" id="uxOriginallyBoardedHighTicketItem" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal164" meta:resourcekey="uxOriginallyBoardedHighTicketResource1" Text="Originally Boarded High Ticket:" /></label>
                    <as:Literal ID="uxOriginallyBoardedHighTicket" runat="server" meta:resourcekey="uxOriginallyBoardedHighTicketValueResource1" />
                </div>


                <div class="item" id="uxSeasonalIndicatorStandard" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal54" meta:resourcekey="uxSeasonalIndicatorResource1" Text="Seasonal Indicator:" /></label>
                    <as:Literal ID="lblSeasonalIndicatorStandard" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item" id="uxRiskRatingStandard" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal49" meta:resourcekey="uxRiskRatingResource1" /></label>
                    <as:Literal ID="lblRiskRatingStandard" runat="server" meta:resourcekey="uxRiskRatingValue1Resource1" />
                </div>

                <div class="item" id="uxFICOScoreStandard" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal58" Text="Credit Score:" /></label>
                    <as:Literal ID="lblFICOScoreStandard" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>

                <div class="item" id="uxDaystoFundStandard" runat="server" visible="false">
                    <label>
                        <as:Literal runat="server" ID="Literal61" meta:resourcekey="uxDaystoFundDaysResource1" Text="Days to Fund:" /></label>
                    <as:Literal ID="lblDaystoFundStandard" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>

            </div>
        </div>
    </div>

    <div class="data-merchant data-elements" id="dataEmlemets" runat="server" visible="false">
        <div class="row">
            <div class="col-md-3">
                <div class="item" runat="server">
                    <label>
                        <as:Literal ID="reservelbl" runat="server" Text="Reserve %:"></as:Literal>
                    </label>
                    <as:Literal ID="reserveVal" runat="server" />
                </div>
                <div class="item" runat="server">
                    <label>
                        <as:Literal ID="reserveDollarAmountlbl" runat="server" Text="Reserve dollar amount:"></as:Literal>

                    </label>
                    <as:Literal ID="reserveDollarAmountVal" runat="server" />
                </div>
                <div class="item">
                    <label class="padding_bottom_0px">
                        <as:Literal ID="RDRlbl" runat="server" Text="RDR Count:"></as:Literal>

                    </label>
                    <as:Literal ID="RDRVal" runat="server" />
                </div>
            </div>
            <div class="col-md-5" id="DataElementSecondCol">
                <div class="item">
                    <label>
                        <as:Literal ID="AppIDlbl" runat="server" Text="App ID:"></as:Literal>
                    </label>
                    <as:Literal ID="AppVal" runat="server" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal ID="Legallbl" runat="server" Text="Legal:"></as:Literal>
                    </label>
                    <as:Literal ID="LegalVal" runat="server" />
                </div>

                <div class="item">
                    <label class="padding_bottom_0px">
                        <as:Literal ID="ProductorServicelbl" runat="server" Text="Product or Services:"></as:Literal>
                    </label>
                    <as:Literal ID="ProductorServiceVal" runat="server" />
                </div>

            </div>
            <div class="col-md-3" id="Div10" runat="server">
                <div class="item">
                    <label>
                        <as:Literal ID="Saleslbl" runat="server" Text="Swiped / MOTO / Internet:"></as:Literal>
                    </label>
                    <as:Literal ID="salesVal" runat="server" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal ID="B2Blbl" runat="server" Text="B2B / B2C / B2G:"></as:Literal>
                    </label>
                    <as:Literal ID="B2BVal" runat="server" />
                </div>
                <div class="item">
                    <label class="padding_bottom_0px">
                        <as:Literal ID="Governmentlbl" runat="server" Text="Local / National / International:"></as:Literal>
                    </label>
                    <as:Literal ID="GovernmentVal" runat="server" />
                </div>
            </div>
            <div class="col-md-4" id="uxLastCol">
            </div>


        </div>
    </div>
    <div class="display-none">
        <as:HiddenField ID="hdShowDetailModal" runat="server" />
        <as:HiddenField ID="hdActionModal" runat="server" />
        <as:Button runat="server" ID="uxShowModalDetail" OnClick="UxShowModalDetail_Click" meta:resourcekey="uxShowAuthResource1" />
    </div>
    <as:RadCodeBlock ID="JavaScript" runat="server">
        <link href="../res/css/risk-report.css" rel="stylesheet" />
        <script type="text/javascript">
            var mfc_merchant_info_clientId = "<%=SessionManager.CurrentClient %>";
            var mfc_merchant_info_uxShowModalDetailID = "<%=uxShowModalDetail.ClientID %>";
            var mfc_merchant_info_hdActionModalID = "<%=hdActionModal.ClientID %>";
            $(document).ready(function () {
                if (mfc_merchant_info_clientId === '200') {
                    $("#uxSecondCol").removeClass('col-md-5').addClass('col-md-3');
                    $("#uxLastCol").removeClass('col-md-4').addClass('col-md-3');
                }
            });
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/rm_MCF_NewRiskReport_MerchantInformation.js"></script>
    </as:RadCodeBlock>
</div>
