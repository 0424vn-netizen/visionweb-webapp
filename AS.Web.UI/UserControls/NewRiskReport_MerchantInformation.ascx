<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewRiskReport_MerchantInformation.ascx.cs" Inherits="UserControls_NewRiskReportMerchantInformation" %>

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
        <tek:AjaxSetting AjaxControlID="uxAuthMonitorExclude">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPnAuthMonitorExclude" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxWorked">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxPnWorked" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>
<div id="uxMerchantInformationGrid" class="in">
    <div class="row">
        <div class="col-md-12">
            <h2 class="grid-title mt-m-1x mb-1x" style="cursor: default;">
                <span class="text-muted">
                    <as:Literal ID="uxMerchantName" runat="server" meta:resourcekey="uxMerchantNameResource1" />
                </span>
            </h2>
        </div>
    </div>
    <div class="row">

        <div class="col-md-12">
            <div class="status-merchant">
                <div class="row">
                    <div class="col-md-2" id="divStatus">
                        <span class="title">
                            <as:Literal ID="Literal11" runat="server" Text="Status:" meta:resourcekey="Literal11Resource1"></as:Literal></span>
                        <as:Literal ID="uxStatus" runat="server" meta:resourcekey="uxStatusResource1" />
                    </div>
                    <%--36801 – MCPS – VW Supplemental MIF – FE--%>
                    <div class="col-md-3 hide" id="divFundingStatus">
                        <span class="title">
                            <as:Literal ID="Literal15" runat="server" Text="Funding Status:" meta:resourcekey="Literal15Resource1"></as:Literal></span>
                        <as:Literal ID="uxFundingStatus" runat="server" meta:resourcekey="uxFundingStatusResource1" />
                    </div>
                    <%--End--%>
                    <div class="col-md-4 hide" id="divAuthMonitorExclude">
                        <span class="title">
                            <as:Literal ID="Literal13" runat="server" Text="Auth Monitor Exclude:" meta:resourcekey="Literal13Resource"></as:Literal></span>
                        <div class="inline-block" id="ciPnAuthMonitorExclude">
                            <div runat="server" id="uxPnAuthMonitorExclude">
                                <as:CheckBox ID="uxAuthMonitorExclude" AutoPostBack="true" runat="server" OnCheckedChanged="uxAuthMonitorExclude_CheckedChanged" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4" id="divProfile">
                        <span class="title">
                            <as:Literal ID="Literal9" runat="server" Text="Profile:" meta:resourcekey="Literal9Resource1"></as:Literal></span>
                        <div class="inline-block">
                            <as:RadComboBox ID="uxProfile" runat="server" EnableEmbeddedBaseStylesheet="false" AutoPostBack="true"
                                DataValueField="DataKey" DataTextField="DataText" Width="200px" OnSelectedIndexChanged="uxSelected_OnClick">
                            </as:RadComboBox>
                            <as:Literal ID="uxProfileReadOnly" runat="server" meta:resourcekey="uxProfileReadOnlyResource1" />
                        </div>
                    </div>
                    <as:PlaceHolder ID="uxPlhRiskLevel" runat="server" Visible="false">
                        <div class="col-md-3" id="divRiskLevel">
                            <span class="title">
                                <as:Literal ID="uxLabelRiskLevel" runat="server" Text="Risk Level:" meta:resourcekey="RiskLevel"></as:Literal>
                            </span>
                            <as:Literal ID="uxRiskLevel" runat="server" ></as:Literal>
                        </div>
                    </as:PlaceHolder>
                </div>
                <div class="row">
                    <%--39336 - Risk Report - Update UI for Watch/Multi-Watch--%>
                    <div class="col-md-2" id="divWatch">
                        <span class="title">
                            <as:Literal ID="Literal18" runat="server" Text="Watch:" meta:resourcekey="Literal18Resource1"></as:Literal></span>
                        <div class="inline-block" id="ciPnlWatch">
                            <div runat="server" id="uxPnlWatch">
                                <as:CheckBox ID="uxWatch" AutoPostBack="true" runat="server" meta:resourcekey="uxWatchResource1" OnCheckedChanged="uxChecked_OnClick" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 hide" id="divWorked">
                        <span class="title">
                            <as:Literal ID="Literal20" runat="server" Text="Worked:" meta:resourcekey="Literal20Resource"></as:Literal></span>
                        <div class="inline-block" id="ciPnWorked">
                            <div runat="server" id="uxPnWorked">
                                <as:CheckBox ID="uxWorked" AutoPostBack="true" runat="server" OnCheckedChanged="uxWorked_CheckedChanged" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4" id="divMultiWatch">
                        <span class="title">
                            <as:Literal ID="Literal19" runat="server" Text="Multi-Watch:" meta:resourcekey="Literal19Resource1"></as:Literal></span>
                        <div class="inline-block none-available">
                            <as:Literal ID="uxMultiWatchList" runat="server" Text="N/A" meta:resourcekey="uxMultiWatchListResource1" />
                        </div>
                    </div>
                    <%--End--%>
                    <div class="col-md-3" id="divWKD30">
                        <span class="title">
                            <as:Literal ID="Literal25" runat="server" Text="#Worked in 30 Days:" meta:resourcekey="Literal24Resource1"></as:Literal></span>
                        <div class="inline-block">
                            <as:Literal ID="uxWKD30" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="pnlClassificationRiskScore" class="col-md-12" runat="server" visible="false">
            <div class="data-merchant">
                <tek:RadCodeBlock runat="server">
                    <div class="row">
                        <div class="col-md-5">
                            <div class="item">
                                <label class="pb-3x-im">
                                    <as:Literal ID="Literal21" runat="server" Text="Classification" meta:resourcekey="ltrClassificationResource"></as:Literal></label>
                                <%= BindValue("MerchantClassificationName")%>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="item">
                                <label class="pb-3x-im">
                                    <as:Literal ID="Literal22" runat="server" Text="Multiplier" meta:resourcekey="ltrMultiplierResource"></as:Literal>
                                </label>
                                <%= BindValue("Multiplier")%>
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


            </div>
            <div class="col-md-5">
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
                        <li><as:Literal ID="uxOwner" runat="server" meta:resourcekey="uxOwnerResource1" /></li>
                        <li><as:Literal ID="uxOwner2" runat="server" Visible="false"></as:Literal></li>
                        <li><as:Literal ID="uxOwner3" runat="server" Visible="false"></as:Literal></li>
                        <li><as:Literal ID="uxOwner4" runat="server" Visible="false"></as:Literal></li>
                        <li><as:Literal ID="uxOwner5" runat="server" Visible="false"></as:Literal></li>
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
                         OnClientClick ="ShowPopupModal('../WebsiteModal.aspx','auto'); return false;" Text = "View All Websites" meta:resourcekey="ViewAllWebsites" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy1" meta:resourcekey="uxHierarchy1Resource1" /></label>
                    <as:Literal ID="uxHierarchyValue1" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy2" meta:resourcekey="uxHierarchy2Resource1" /></label>
                    <as:Literal ID="uxHierarchyValue2" runat="server" meta:resourcekey="uxHierarchyValue2Resource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy3" meta:resourcekey="uxHierarchy3Resource1" />
                        <as:Literal runat="server" ID="uxServicedByLabel" Visible="False" meta:resourcekey="uxServicedByLabelResource1" /></label>
                    <as:Literal ID="uxHierarchyValue3" runat="server" meta:resourcekey="uxHierarchyValue3Resource1" />
                    <as:Literal ID="uxServicedByValue" runat="server" Visible="False" meta:resourcekey="uxServicedByValueResource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy4" meta:resourcekey="uxHierarchy4Resource1" /></label>
                    <as:Literal ID="uxHierarchyValue4" runat="server" meta:resourcekey="uxHierarchyValue4Resource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy5" meta:resourcekey="uxHierarchy5Resource1" /></label>
                    <as:Literal ID="uxHierarchyValue5" runat="server" meta:resourcekey="uxHierarchyValue5Resource1" />
                </div>
                <div class="item">
                    <label>
                        <as:Literal runat="server" ID="uxHierarchy6" meta:resourcekey="uxHierarchy5Resource1" /></label>
                    <as:Literal ID="uxHierarchyValue6" runat="server" meta:resourcekey="uxHierarchyValue5Resource1" />
                </div>
            </div>


        </div>
    </div>
    <%--    <div class="row">
        <div class="col-md-12 action-container text-right">
            <as:Button ID="uxSave" runat="server" Text="Save Merchant Info" CssClass="btn btn-default"
                OnClick="uxSave_OnClick" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />
        </div>
    </div>--%>
</div>
