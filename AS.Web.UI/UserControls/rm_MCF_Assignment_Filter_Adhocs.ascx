<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_Filter_Adhocs.ascx.cs"
    Inherits="UserControls_rm_MCF_Assignment_Filter_Adhocs" %>
<%@ Register Src="~/UserControls/rm_MCF_ApprovalDate.ascx" TagName="Risk_ApprovalDate" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FirstBatchDate.ascx" TagName="Risk_FirstBatchDate" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterHierarchy.ascx" TagName="Hierarchy" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterState.ascx" TagName="State" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterSIC.ascx" TagName="SICCode" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterZipCode.ascx" TagName="ZipCode" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterProfile.ascx" TagName="Profile" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterMerchantRank.ascx" TagName="MerchantRank" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterACHHoldDays.ascx" TagName="ACHHoldDays" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterOwnerLastName.ascx" TagName="OwnerLastName" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_FilterZipCode.ascx" TagName="ZipCode2" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_MerchantFundingStatus.ascx" TagName="MerchantFundingStatus" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Filter_Transactional.ascx" TagName="Transactional" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Filter_ECommerce.ascx" TagName="ECommerce" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Filter_Source_Hierarchy.ascx" TagName="SourceHierarchy" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_WatchStatus.ascx" TagName="Risk_WatchStatus" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Filter_HRCode_Modal.ascx" TagName="HRCode" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Assignment_Filter_Extend.ascx" TagName="Risk_ExtendFilter" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_Filter_MerchantClassifications_Modal.ascx" TagName="MerchantClassifications" TagPrefix="uc" %>
<as:RadAjaxManagerProxy ID="RadAjaxManagerProxyReview" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="btnRefreshState">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divState" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshSIC">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divSIC" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshProfile">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divProfile" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshMerchantRank">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divMerchantRank" />
                <tek:AjaxUpdatedControl ControlID="hdnCountSelectedFilter" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshACHHoldDays">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divAchHoldDays" />
                <tek:AjaxUpdatedControl ControlID="hdnCountSelectedFilter" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshOwnerLastName">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divOwnerLastName" />
                <tek:AjaxUpdatedControl ControlID="hdnCountSelectedFilter" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshZipCode">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divZipCode" />
                <tek:AjaxUpdatedControl ControlID="hdnCountSelectedFilter" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshMerchantFundingStatus">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divMerchantFunding" />
                <tek:AjaxUpdatedControl ControlID="hdnCountSelectedFilter" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnRefreshHighRisk">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divHRCode" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnCalculateMerchantCount">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlMerchantCountOnFilter" LoadingPanelID="customLoadingPanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="uxBtnMerchantCount">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="pnlMerchantCountOnFilter" LoadingPanelID="customLoadingPanel" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnbtnRefreshMC">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxMerchantClassificationsLabel" LoadingPanelID="customLoadingPanel" />
            </UpdatedControls>
        </tek:AjaxSetting>

        <tek:AjaxSetting AjaxControlID="btnRefreshGverifyCode">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divGverifyCode" />
            </UpdatedControls>
        </tek:AjaxSetting>

        <tek:AjaxSetting AjaxControlID="btnGauthenticateCode">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divGauthenCode" />
            </UpdatedControls>
        </tek:AjaxSetting>
        <tek:AjaxSetting AjaxControlID="btnG2Compass">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="divG2Compass" />
            </UpdatedControls>
        </tek:AjaxSetting>

    </AjaxSettings>
</as:RadAjaxManagerProxy>
<!--use custom for merchant count -->
<as:RadAjaxLoadingPanel ID="customLoadingPanel" runat="server" BackgroundPosition="Center">
</as:RadAjaxLoadingPanel>

<div class="height-22"></div>
<div class="row">
    <div class="col-md-12" data-toggle="collapse" data-target="#uxAssignmentFiltersGrid">
        <h2 class="grid-title on-top">
            <as:Literal ID="ltAdhocFilter" Text="Adhoc Filters" runat="server" meta:resourcekey="ltAdhocFilterResource1"></as:Literal>
        </h2>
    </div>
</div>
<div class="row">
    <div class="col-md-12 in" id="uxAssignmentFiltersGrid">
        <table class="ASTable" style="table-layout: fixed;">
            <colgroup>
                <col style="width: 17%;" />
                <col />
                <as:ASRadCodeBlock ID="ASRadCodeBlock6" runat="server">
                    <%=FeatureMode == WebSiteEnums.FeatureMode.Edit ? " <col class=\"action-column\"/>" : ""%>
                </as:ASRadCodeBlock>
            </colgroup>
            <tr class="section-heading">
                <td class="heading valign-middle" style="width: 17%;">
                    <as:Literal ID="ltSummary" runat="server" Text="Summary" meta:resourcekey="ltSummaryResource1"></as:Literal></td>
                <as:ASRadCodeBlock ID="ASRadCodeBlock2" runat="server">
                    <td class="normal-font-weight" colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
                </as:ASRadCodeBlock>
                <label class="first last">
                    <as:Literal ID="Literal1" runat="server" Text="Merchant Count:" meta:resourcekey="Literal1Resource1"></as:Literal></label>
                <div class="control-inline">
                    <label id="pnlMerchantCountOnFilter" runat="server">0</label>
                </div>
                <div class="control-inline">
                    <asp:Panel ID="plhRefesh" runat="server" meta:resourcekey="plhRefeshResource1">
                        <asp:Button runat="server" ID="uxBtnMerchantCount" OnClientClick="CalculateMerchantCount(); return false;" Text="Refresh" CssClass="btn btn-default" meta:resourcekey="uxBtnMerchantCountResource1" />
                    </asp:Panel>
                </div>

                </td> 
            </tr>
            <tr class="Row">
                <td class="heading">
                    <label class="control-label">
                        <as:Literal ID="Literal2" runat="server" Text="Watch Status:" meta:resourcekey="Literal2Resource1"></as:Literal></label>
                </td>
                <as:ASRadCodeBlock ID="ASRadCodeBlock3" runat="server">
                    <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
                        <uc:Risk_WatchStatus ID="uxWatchStatus" runat="server" />
                    </td>
                </as:ASRadCodeBlock>
            </tr>
            <tr class="AltRow">
                <td class="heading">
                    <as:ValidatorLabel ID="ValidatorLabel1" runat="server" CssClass="control-label" Text="Merchants Number:" ApplyFor="txtMerchantNumber" meta:resourcekey="ValidatorLabel1Resource1"></as:ValidatorLabel>
                </td>
                <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
                    <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
                        <asp:TextBox ID="txtMerchantNumber" runat="server" Width="160px" MaxLength="16"
                            onkeypress="MerchantRange_OnKeyPress(event);" CssClass="form-control" meta:resourcekey="txtMerchantNumberResource1"></asp:TextBox>
                        <as:ValidatorMessage runat="server" ID="txtMerchantNumberMsg" ApplyFor="txtMerchantNumber" />
                    </td>
                </as:ASRadCodeBlock>
            </tr>          
            <tr class="AltRow">
                <td class="heading">
                    <label class="control-label">
                        <as:Literal ID="Literal3" runat="server" Text="Approval Date:" meta:resourcekey="Literal3Resource1"></as:Literal></label>
                </td>
                <as:ASRadCodeBlock ID="ASRadCodeBlock5" runat="server">
                    <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
                        <uc:Risk_ApprovalDate ID="uxApprovalDate" runat="server" />
                    </td>
                </as:ASRadCodeBlock>
            </tr>
            <tr class="AltRow">
                <td class="heading">
                    <label class="control-label">
                        <as:Literal ID="Literal18" runat="server" Text="First Batch Date:" meta:resourcekey="Literal41Resource1"></as:Literal></label>
                </td>
                <as:ASRadCodeBlock ID="ASRadCodeBlock7" runat="server">
                    <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
                        <uc:Risk_FirstBatchDate ID="uxFirstBatchDate" runat="server" />
                    </td>
                </as:ASRadCodeBlock>
            </tr>

            <tr class="Row">                
                 <td class="heading">
                    <label class="control-label">
                        <as:Literal ID="LitExcludeMerchantsClosedStatus" runat="server" meta:resourcekey="ExcludeMerchantsClosedStatus"></as:Literal></label>
                </td>
                <td colspan="<%# FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">                                    
                    <as:CheckBox ID="chkExcludeMerchantsClosedStatus" runat="server" tracking-required="true" tracking-key="236" tracking-type="checkbox" />                   
                </td>               
            </tr>

            <as:PlaceHolder ID="plhAllMerchants" runat="server" Visible="False">
                <td class="heading">                   
                    <label class="control-label">
                        <as:Literal ID="Literal11" runat="server" Text="All Merchants:" meta:resourcekey="Literal6Resource1"></as:Literal></label>
                </td>
                <as:ASRadCodeBlock ID="ASRadCodeBlock4" runat="server">
                    <td colspan="<%=FeatureMode == WebSiteEnums.FeatureMode.Edit ?"2":"1"%>">
                        <as:CheckBox ID="chkAllMerchants" runat="server" meta:resourcekey="chkAllMerchantsResource1" />
                    </td>
                </as:ASRadCodeBlock>
            </as:PlaceHolder>
            <!--HIERARCHY GROUP-->
            <asp:Repeater ID="uxHierarchyFilterRepeater" runat="server" OnItemDataBound="uxHierarchyFilterRepeater_ItemDataBound">
                <ItemTemplate>
                    <as:PlaceHolder ID="uxGroupHierarchyEdit" runat="server">
                        <tr class="section-heading">
                            <td colspan="3">
                                <as:Literal ID="uxGroupHierarchyTitleEdit" runat="server" meta:resourcekey="uxGroupHierarchyTitleEditResource2"></as:Literal>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="uxGroupHierarchyView" runat="server">
                        <tr class="section-heading">
                            <td colspan="2">
                                <as:Literal ID="uxGroupHierarchyTitleView" runat="server" meta:resourcekey="uxGroupHierarchyTitleViewResource2"></as:Literal>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                    <tr class="AltRow">
                        <td class="heading">
                            <%#Eval("HierarchyFilterText") %>:
                        </td>
                        <td>
                            <div>
                                <as:Panel ID="divHierarchy" runat="server" meta:resourcekey="divHierarchyResource2">
                                    <as:Literal ID="lblHierarchy" runat="server" Text="N/A" meta:resourcekey="lblHierarchyResource2"></as:Literal>
                                </as:Panel>
                            </div>
                        </td>
                        <as:PlaceHolder runat="server" ID="plhHierarchy">
                            <td class="action-column">
                                <a href="#" onclick='return CallHierarchyFilterModal(&#039;rm_MCF_Filter_Hierarchy_Modal.aspx?<%# HierarchyFilterQueryString(Eval("HierarchyFilterMode").ToString(),"ctl00_ContentPage_uxFilters_uxHierarchyFilterRepeater_ctl" + GetItemIndexAsString(Container.ItemIndex) + "_btnRefreshHierarchy") %>&#039;, &#039;auto&#039;); return false;'>
                                    <as:Literal ID="ltEdit" runat="server" Text="Edit" meta:resourcekey="ltEditResource1" />
                                </a>
                                <as:Button ID="btnRefreshHierarchy" runat="server" OnCommand="btnRefreshHierarchy_Command"
                                    CssClass="display-none" CommandArgument='<%# Eval("HierarchyFilterMode") %>' IsStandardButton="False" meta:resourcekey="btnRefreshHierarchyResource2" />
                            </td>
                        </as:PlaceHolder>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <as:PlaceHolder ID="uxGroupHierarchyEdit" runat="server">
                        <tr class="section-heading">
                            <td colspan="3">
                                <as:Literal ID="uxGroupHierarchyTitleEdit" runat="server" meta:resourcekey="uxGroupHierarchyTitleEditResource1"></as:Literal>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="uxGroupHierarchyView" runat="server">
                        <tr class="section-heading">
                            <td colspan="2">
                                <as:Literal ID="uxGroupHierarchyTitleView" runat="server" meta:resourcekey="uxGroupHierarchyTitleViewResource1"></as:Literal>
                            </td>
                        </tr>
                    </as:PlaceHolder>
                    <tr class="Row">
                        <td class="heading">
                            <%#Eval("HierarchyFilterText") %>:
                        </td>
                        <td>
                            <div>
                                <as:Panel ID="divHierarchy" runat="server" meta:resourcekey="divHierarchyResource1">
                                    <as:Literal ID="lblHierarchy" runat="server" Text="N/A" meta:resourcekey="lblHierarchyResource1"></as:Literal>
                                </as:Panel>
                            </div>
                        </td>
                        <as:PlaceHolder runat="server" ID="plhHierarchy">
                            <td class="action-column">
                                <a href="#" onclick='return CallHierarchyFilterModal(&#039;rm_MCF_Filter_Hierarchy_Modal.aspx?<%# HierarchyFilterQueryString(Eval("HierarchyFilterMode").ToString(),"ctl00_ContentPage_uxFilters_uxHierarchyFilterRepeater_ctl" + GetItemIndexAsString(Container.ItemIndex) + "_btnRefreshHierarchy") %>&#039;, &#039;auto&#039;); return false;'>
                                    <as:Literal ID="ltEdit1" runat="server" Text="Edit" meta:resourcekey="ltEditResource1" />
                                </a>
                                <as:Button ID="btnRefreshHierarchy" runat="server" OnCommand="btnRefreshHierarchy_Command"
                                    CssClass="display-none" CommandArgument='<%# Eval("HierarchyFilterMode") %>' IsStandardButton="False" meta:resourcekey="btnRefreshHierarchyResource1" />
                            </td>
                        </as:PlaceHolder>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
            <uc:SourceHierarchy runat="server" ID="uxSourceHierarchyFilter"></uc:SourceHierarchy>
            <as:PlaceHolder ID="uxGroupDemographicEdit" runat="server" Visible="False">
                <tr class="section-heading">
                    <td colspan="3">
                        <as:Literal ID="Literal4" runat="server" Text="Demographic" meta:resourcekey="Literal4Resource1"></as:Literal>
                    </td>
                </tr>
            </as:PlaceHolder>
            <as:PlaceHolder ID="uxGroupDemographicView" runat="server" Visible="False">
                <tr class="section-heading">
                    <td colspan="2">
                        <as:Literal ID="Literal5" runat="server" Text="Demographic" meta:resourcekey="Literal5Resource1"></as:Literal>
                    </td>
                </tr>
            </as:PlaceHolder>
            <tr class="AltRow">
                <td class="heading">
                    <as:Literal ID="Literal12" runat="server" Text="State:" meta:resourcekey="Risk_Assignment_Filter_Adhocs_ascx_State"></as:Literal>
                </td>
                <td>
                    <div>
                        <div id="divState" runat="server">
                            <as:Literal ID="lblState" runat="server" Text="N/A" meta:resourcekey="lblStateResource1"></as:Literal>
                        </div>
                    </div>
                </td>
                <as:PlaceHolder runat="server" ID="plhState">
                    <td class="action-column">
                        <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_State_Modal.aspx', 480, 725); return false;">
                            <as:Literal ID="Literal7" runat="server" Text="Edit" meta:resourcekey="Literal7Resource1"></as:Literal>
                        </a>
                    </td>
                </as:PlaceHolder>
            </tr>
            <tr class="Row">
                <td class="heading">
                    <as:Literal ID="Literal6" runat="server" Text="SIC:" meta:resourcekey="Literal6Resource1"></as:Literal></td>
                <td>
                    <div id="divSIC" runat="server">
                        <as:Literal ID="lblSIC" runat="server" Text="N/A" meta:resourcekey="lblSICResource1"></as:Literal>
                    </div>
                </td>
                <as:PlaceHolder runat="server" ID="plhSIC">
                    <td class="action-column">
                        <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_SIC_Modal.aspx', 480, 725); return false;">
                            <as:Literal ID="Literal8" runat="server" Text="Edit" meta:resourcekey="Literal8Resource1"></as:Literal>
                        </a>
                    </td>
                </as:PlaceHolder>
            </tr>
            <%--Begin 36801 – MCPS – VW Supplemental MIF – FE--%>

            <asp:Panel ID="pnMerchantRank" runat="server" Visible="false">
                <tr class="Row">
                    <td class="heading">
                        <as:Literal ID="Literal15" runat="server" Text="Merchant Rank:" meta:resourcekey="Literal19Resource1"></as:Literal></td>
                    <td>
                        <div id="divMerchantRank" runat="server">
                            <as:Literal ID="lbMerchantRank" runat="server" Text="N/A" meta:resourcekey="lblMerchantRankResource1"></as:Literal>
                        </div>
                    </td>
                    <as:PlaceHolder runat="server" ID="plhMerchantRank">
                        <td class="action-column">
                            <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_MerchantRank_Modal.aspx', 480, 725); return false;">
                                <as:Literal ID="Literal20" runat="server" Text="Edit" meta:resourcekey="Literal7Resource1"></as:Literal></a>
                        </td>
                    </as:PlaceHolder>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnACHHoldDay" runat="server" Visible="false">
                <tr class="AltRow">
                    <td class="heading">
                        <as:Literal ID="Literal21" runat="server" Text="ACH Hold Days:" meta:resourcekey="Literal21Resource1"></as:Literal></td>
                    <td>
                        <div id="divAchHoldDays" runat="server">
                            <as:Literal ID="lbAchHoldDays" runat="server" Text="N/A" meta:resourcekey="lblAchHoldDayResource1"></as:Literal>
                        </div>
                    </td>
                    <as:PlaceHolder runat="server" ID="plhAchHoldDay">
                        <td class="action-column">
                            <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_ACHHoldDays_Modal.aspx', 480, 725); return false;">
                                <as:Literal ID="Literal23" runat="server" Text="Edit" meta:resourcekey="Literal7Resource1"></as:Literal></a>
                        </td>
                    </as:PlaceHolder>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnOwnerLastName" runat="server" Visible="false">
                <tr class="Row">
                    <td class="heading">
                        <as:Literal ID="Literal24" runat="server" Text="Owner Last Name:" meta:resourcekey="Literal24Resource1"></as:Literal></td>
                    <td>
                        <div id="divOwnerLastName" runat="server">
                            <as:Literal ID="lbOwnerLastName" runat="server" Text="N/A" meta:resourcekey="lblOwnerLastNameResource1"></as:Literal>
                        </div>
                    </td>
                    <as:PlaceHolder runat="server" ID="plhOwnerLastName">
                        <td class="action-column">
                            <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_OwnerLastName_Modal.aspx', 480, 725); return false;">
                                <as:Literal ID="Literal26" runat="server" Text="Edit" meta:resourcekey="Literal7Resource1"></as:Literal></a>
                        </td>
                    </as:PlaceHolder>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnZip3" runat="server" Visible="false">
                <tr class="AltRow">
                    <td class="heading">
                        <as:Literal ID="Literal27" runat="server" Text="Zip 3:" meta:resourcekey="Literal27Resource1"></as:Literal></td>
                    <td>
                        <div id="divZipCode" runat="server">
                            <as:Literal ID="lbZipCode" runat="server" Text="N/A" meta:resourcekey="lblZipCodeResource1"></as:Literal>
                        </div>
                    </td>
                    <as:PlaceHolder runat="server" ID="plhZipCode">
                        <td class="action-column">
                            <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_ZipCode_Modal.aspx', 480, 725); return false;">
                                <as:Literal ID="Literal29" runat="server" Text="Edit" meta:resourcekey="Literal7Resource1"></as:Literal></a>
                        </td>
                    </as:PlaceHolder>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnMerchantFundingStatus" runat="server" Visible="false">
                <tr class="Row">
                    <td class="heading">
                        <as:Literal ID="Literal30" runat="server" Text="Merchant Funding Status:" meta:resourcekey="Literal30Resource1"></as:Literal></td>
                    <td>
                        <div id="divMerchantFunding" runat="server">
                            <as:Literal ID="lbMerchantFundingStatus" runat="server" Text="N/A" meta:resourcekey="lblMerchantRankResource1"></as:Literal>
                        </div>
                    </td>
                    <as:PlaceHolder runat="server" ID="plhMerchantFundingStatus">
                        <td class="action-column">
                            <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_MerchantFundingStatus_Modal.aspx', 480, 725); return false;">
                                <as:Literal ID="Literal32" runat="server" Text="Edit" meta:resourcekey="Literal7Resource1"></as:Literal></a>
                        </td>
                    </as:PlaceHolder>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnCreditScore" runat="server" Visible="false">
                <tr class="AltRow">
                    <td class="heading">
                        <as:Literal ID="Literal33" runat="server" Text="Credit Score:" meta:resourcekey="Literal33Resource1"></as:Literal></td>
                    <td>
                        <table>
                            <tr>
                                <td rowspan="2">
                                    <as:Literal ID="Literal35" runat="server" Text="FICO Credit Score" meta:resourcekey="Literal35Resource1"></as:Literal>
                                </td>

                                <td>
                                    <as:CheckBox ID="uxChkIsFrom" Width="50px" runat="server" onclick="visibleCreditScoreCb(this, 'uxCbCreditScoreFrom')" Text="" meta:resourcekey="lbIsGreater" />
                                    <as:ASRadComboBox ID="uxCbCreditScoreFrom" runat="server" DataTextField="DataText" DataValueField="DataKey" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <as:CheckBox ID="uxChkIsTo" Width="50px" runat="server" onclick="visibleCreditScoreCb(this, 'uxCbCreditScoreTo')" Text="" meta:resourcekey="lbIsSmaller" />
                                    <as:ASRadComboBox ID="uxCbCreditScoreTo" runat="server" DataTextField="DataText" DataValueField="DataKey" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <as:PlaceHolder runat="server" ID="plhCreditScore">
                        <td class="action-column"></td>
                    </as:PlaceHolder>
                </tr>
            </asp:Panel>
            <tr class="AltRow">
                <td class="heading">
                    <as:Literal ID="Literal9" runat="server" Text="Profile:" meta:resourcekey="Literal9Resource1"></as:Literal></td>
                <td>
                    <div id="divProfile" runat="server">
                        <as:Literal ID="lblProfile" runat="server" Text="N/A" meta:resourcekey="lblProfileResource1"></as:Literal>
                    </div>
                </td>
                <as:PlaceHolder runat="server" ID="plhProfile">
                    <td class="action-column">
                        <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_Profile_Modal.aspx', 480, 725); return false;">
                            <as:Literal ID="Literal10" runat="server" Text="Edit" meta:resourcekey="Literal10Resource1"></as:Literal>
                        </a>
                    </td>
                </as:PlaceHolder>
            </tr>
            <uc:ECommerce runat="server" ID="uxECommerceFilter"></uc:ECommerce>
            <%--End--%>
            <as:PlaceHolder ID="uxHighRisk" runat="server" Visible="false">
                <as:PlaceHolder ID="uxHighRiskView" runat="server" Visible="False">
                    <tr class="section-heading">
                        <td colspan="2">
                            <as:Literal ID="Literal13" runat="server" Text="High Risk" meta:resourcekey="Literal12Resource1"></as:Literal>
                        </td>
                    </tr>
                </as:PlaceHolder>
                <as:PlaceHolder ID="uxHighRiskEdit" runat="server" Visible="False">
                    <tr class="section-heading">
                        <td colspan="3">
                            <as:Literal ID="Literal14" runat="server" Text="High Risk" meta:resourcekey="Literal12Resource1"></as:Literal>
                        </td>
                    </tr>
                </as:PlaceHolder>
                <as:PlaceHolder runat="server" ID="uxHRCode">
                    <tr class="Row">
                        <td class="heading">
                            <as:Literal ID="Literal16" runat="server" Text="HR Code:" meta:resourcekey="Literal11Resource1"></as:Literal></td>
                        <td>
                            <div id="divHRCode" runat="server">
                                <as:Literal ID="lblHRCode" runat="server" Text="N/A" meta:resourcekey="lblHRCodeResource1"></as:Literal>
                            </div>
                        </td>
                        <as:PlaceHolder runat="server" ID="plhHRCode" Visible="False">
                            <td class="action-column">
                                <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_HRCode_Modal.aspx', 480, 725); return false;">
                                    <as:Literal ID="Literal17" runat="server" Text="Edit" meta:resourcekey="Literal10Resource1"></as:Literal></a>
                            </td>
                        </as:PlaceHolder>
                    </tr>
                </as:PlaceHolder>
            </as:PlaceHolder>
            <tr id="pnlMerchantClassifications" runat="server" visible="true">
                <td class="heading">
                    <as:Literal ID="Literal25" runat="server" Text="Merchant Classifications:" meta:resourcekey="Literal14ResourceMerchantClassifications"></as:Literal></td>
                <td>
                    <div id="div1" runat="server">
                        <as:Literal ID="uxMerchantClassificationsLabel" runat="server" Text="N/A" meta:resourcekey="lblHRCodeResource1"></as:Literal>
                    </div>
                </td>
                <td id="pnlbtnEditMerchantClassifications" class="action-column" runat="server" visible="true">
                    <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_MerchantClassifications_Modal.aspx', 480, 725); return false;">
                        <as:Literal ID="Literal31" runat="server" Text="Edit" meta:resourcekey="Literal10Resource1"></as:Literal></a>
                </td>
            </tr>

            <tr id="pnlGverifyCode" runat="server" visible="false">
                <td class="heading">
                    <as:Literal ID="Literal19" runat="server" Text="Gverify Code:" meta:resourcekey="LiteralResourceGverifyCode"></as:Literal></td>
                <td>
                    <div id="divGverifyCode" runat="server">
                        <asp:Label tracking-key="GverifyCode" tracking-type="hierarchy" ID="lblGverifyCode" runat="server" Text="N/A" meta:resourcekey="lblGverifyCodeResource1"></asp:Label>
                    </div>
                </td>

                <td class="action-column" runat="server" id="pnlGverifyCodeEdit">
                    <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_GverifyCode_Modal.aspx', 480, 725); return false;">
                        <as:Literal ID="Literal22" runat="server" Text="Edit" meta:resourcekey="Literal10Resource1"></as:Literal></a>
                </td>
            </tr>

            <tr id="pnlGauthenticateCode" runat="server" visible="false">
                <td class="heading">
                    <as:Literal ID="Literal28" runat="server" Text="Gauthenticate Code:" meta:resourcekey="LiteralResourceGauthenticateCode"></as:Literal></td>
                <td>
                    <div id="divGauthenCode" runat="server">
                        <asp:Label tracking-key="GauthenticateCode" tracking-type="hierarchy" ID="lblGauthenticateCode" runat="server" Text="N/A" meta:resourcekey="lblGauthenticateCodeResource1"></asp:Label>
                    </div>
                </td>

                <td class="action-column" runat="server" id="pnlGauthenticateCodeEdit">
                    <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_GauthenticateCode_Modal.aspx', 480, 725); return false;">
                        <as:Literal ID="Literal34" runat="server" Text="Edit" meta:resourcekey="Literal10Resource1"></as:Literal></a>
                </td>
            </tr>


            <tr id="pnlG2Compass" runat="server" visible="false">
                <td class="heading">
                    <as:Literal ID="Literal36" runat="server" Text="G2 Compass Auto Approval Indicator:" meta:resourcekey="LiteralResourceG2Compass"></as:Literal></td>
                <td>
                    <div id="divG2Compass" runat="server">
                        <asp:Label tracking-key="G2Compass" tracking-type="hierarchy" ID="lblG2Compass" runat="server" Text="N/A" meta:resourcekey="lblG2CompassResource1"></asp:Label>
                    </div>
                </td>

                <td class="action-column" runat="server" id="pnlG2CompassEdit">
                    <a href="#" onclick="return CallFilterModal1('rm_MCF_Filter_G2CompassAutoApprovalIndicator_Modal.aspx', 480, 725); return false;">
                        <as:Literal ID="Literal37" runat="server" Text="Edit" meta:resourcekey="Literal10Resource1"></as:Literal></a>
                </td>
            </tr>

            <uc:Risk_ExtendFilter ID="Risk_ExtendFilter" runat="server" />

            <uc:Transactional ID="uxTransactionalFilter" runat="server"></uc:Transactional>

        </table>
        <div class="display-none">
            <!--invisible buttons-->
            <as:Button ID="btnCalculateMerchantCount" runat="server" OnClick="btnCalculateMerchantCount_Click" IsStandardButton="False" meta:resourcekey="btnCalculateMerchantCountResource1" />

            <as:Button ID="btnRefreshState" runat="server" OnClick="btnRefreshState_Click" IsStandardButton="False" meta:resourcekey="btnRefreshStateResource1" />
            <as:Button ID="btnRefreshSIC" runat="server" OnClick="btnRefreshSIC_Click" IsStandardButton="False" meta:resourcekey="btnRefreshSICResource1" />
            <as:Button ID="btnRefreshProfile" runat="server" OnClick="btnRefreshProfile_Click" IsStandardButton="False" meta:resourcekey="btnRefreshProfileResource1" />
            <as:Button ID="btnRefreshMerchantRank" runat="server" OnClick="btnRefreshMerchantRank_Click" IsStandardButton="False" meta:resourcekey="btnRefreshMerchantRankResource1" />
            <as:Button ID="btnRefreshACHHoldDays" runat="server" OnClick="btnRefreshACHHoldDays_Click" IsStandardButton="False" meta:resourcekey="btnRefreshMerchantRankResource1" />
            <as:Button ID="btnRefreshOwnerLastName" runat="server" OnClick="btnRefreshOwnerLastName_Click" IsStandardButton="False" meta:resourcekey="btnRefreshOwnerLastNameResource1" />
            <as:Button ID="btnRefreshZipCode" runat="server" OnClick="btnRefreshZipCode_Click" IsStandardButton="False" meta:resourcekey="btnRefreshZipCode2Resource1" />
            <as:Button ID="btnRefreshMerchantFundingStatus" runat="server" OnClick="btnRefreshMerchantFundingStatus_Click" IsStandardButton="False" meta:resourcekey="btnRefreshMerchantFundingStatusResource1" />
            <as:HiddenField ID="hdnCountSelectedFilter" runat="server" />
            <as:Button ID="btnRefreshHighRisk" runat="server" OnClick="btnRefreshHighRisk_Click" IsStandardButton="False" meta:resourcekey="btnRefreshHighRiskResource1" />
            <as:Button ID="btnbtnRefreshMC" runat="server" OnClick="btnbtnRefreshMC_Click" IsStandardButton="False" />

            <as:Button ID="btnRefreshGverifyCode" runat="server" OnClick="btnRefreshGverifyCode_Click" IsStandardButton="False" meta:resourcekey="btnRefreshGverifyCodeResource1" />
            <as:Button ID="btnGauthenticateCode" runat="server" OnClick="btnGauthenticateCode_Click" IsStandardButton="False" meta:resourcekey="btnRefreshGauthenticateCodeResource1" />
            <as:Button ID="btnG2Compass" runat="server" OnClick="btnG2Compass_Click" IsStandardButton="False" meta:resourcekey="btnRefreshG2CompassResource1" />

        </div>
    </div>
</div>



<as:HiddenField runat="server" ID="uxMarketData" Value="1" />

<as:RadCodeBlock ID="radCodeBlock2" runat="server">
    <script type="text/javascript">

        var Risk_Assignment_Filters_Adhoc_btnRefreshState = '<%= btnRefreshState.ClientID %>';
        var Risk_Assignment_Filters_Adhoc_btnRefreshSIC = '<%= btnRefreshSIC.ClientID %>';
        var Risk_Assignment_Filters_Adhoc_btnRefreshProfile = '<%= btnRefreshProfile.ClientID %>';
        var Risk_Assignment_Filters_Adhoc_pnlMerchantCountOnFilter = '<%=pnlMerchantCountOnFilter.ClientID %>';
        var Risk_Assignment_Filters_hdnCountSelectedFilter = '<%=hdnCountSelectedFilter.ClientID %>';
        var Risk_Assignment_Filters_chkAllMerchants = '<%= chkAllMerchants.ClientID %>';
        var Risk_Assignment_Filters_Adhoc_btnRefreshHighRisk = '<%=btnRefreshHighRisk.ClientID %>';
        var Risk_Assignment_Filters_Adhoc_btnCalculateMerchantCount = '<%=btnCalculateMerchantCount.ClientID %>';

        var Risk_Assignment_Filters_btnRefreshMerchantRank = '<%=btnRefreshMerchantRank.ClientID %>';
        var Risk_Assignment_Filters_btnRefreshACHHoldDays = '<%=btnRefreshACHHoldDays.ClientID %>';
        var Risk_Assignment_Filters_btnRefreshOwnerLastName = '<%=btnRefreshOwnerLastName.ClientID %>';
        var Risk_Assignment_Filters_btnRefreshZipCode = '<%=btnRefreshZipCode.ClientID %>';
        var Risk_Assignment_Filters_btnRefreshMerchantFundingStatus = '<%=btnRefreshMerchantFundingStatus.ClientID %>';
        var Risk_Assignment_Filters_uxChkIsTo = '<%=uxChkIsFrom.ClientID %>';

        var Risk_Assignment_Filters_uxChkIsFrom = '<%= uxChkIsTo.ClientID %>';

        var Risk_Assignment_Filters_Adhoc_Required = '<%=Resources.ValMsg.Assignment_Filter_Required %>';

        var Risk_Assignment_Filters_Adhoc_QueryString = '?<%=QueryString %>';

        var Risk_Assignment_Filters_Resource_Message = '<%= string.Format(GetLocalResourceObject("Risk_Assignment_Filters_Resource_Message").ToString(), "_NumberOfMerchant_") %>';
        var Risk_Assignment_Filters_btnbtnRefreshMC = '<%=btnbtnRefreshMC.ClientID %>';
        var Risk_Assignment_Filters_btnRefreshGverifyCode = '<%=btnRefreshGverifyCode.ClientID %>';
        var Risk_Assignment_Filters_btnGauthenticateCode = '<%=btnGauthenticateCode.ClientID %>';
        var Risk_Assignment_Filters_btnG2Compass = '<%=btnG2Compass.ClientID %>';

        function masterAjax_responseEnd(sender, args) {
            //rm_MCF_Assignment_Filter_Transactional
            if (typeof loadControl == 'function') {
                setTimeout(loadControl, 100);
            }

            //rm_MCF_Assignment_Filter_DaystoFund
            if (typeof loadControlDaytoFund == 'function') {
                setTimeout(loadControlDaytoFund, 100);
            }
        }
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_Assignment_Filters_Adhoc.js"></script>
</as:RadCodeBlock>
