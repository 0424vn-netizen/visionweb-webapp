<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Merchant Profile"
    CodeFile="MerchantProfile.aspx.cs" Inherits="gen_MerchantProfile" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_FDR.ascx" TagName="MIF_MerchantDetails_FDR"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="UserControls/ReportFiltering.ascx" TagName="ReportFiltering" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/MessageEditor.ascx" TagName="MessageEditor" TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_TSYS.ascx" TagName="MIF_MerchantDetails_TSYS"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_PLANET.ascx" TagName="MIF_MerchantDetails_PLANET"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_MPS.ascx" TagName="MIF_MerchantDetails_MPS"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_ORI.ascx" TagName="MIF_MerchantDetails_ORI"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_FIS.ascx" TagName="MIF_MerchantDetails_FIS"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_EMS.ascx" TagName="MIF_MerchantDetails_EMS"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_Fulton.ascx" TagName="MIF_MerchantDetails_Fulton"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_PPI.ascx" TagName="MIF_MerchantDetails_PPI"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_WRFC.ascx" TagName="MIF_MerchantDetails_WRFC"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_TNBCI.ascx" TagName="MIF_MerchantDetails_TNBCI"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_NORTH.ascx" TagName="MIF_MerchantDetails_NORTH"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_NTS.ascx" TagName="MIF_MerchantDetails_NTS"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_FISMPS.ascx" TagName="MIF_MerchantDetails_FISMPS"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MechantDetails_IPMT_Omaha.ascx" TagName="MIF_MechantDetails_IPMT_Omaha"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MechantDetails_IPMT_North.ascx" TagName="MIF_MechantDetails_IPMT_North"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MechantDetails_CAYAN.ascx" TagName="MIF_MechantDetails_CAYAN" TagPrefix="uc" %>
<%--42782 – VW – CAYAN - Implement New TSYS Processing Platform - add--%>
<%@ Register Src="UserControls/MIF_MechantDetails_TSYS_CAYAN.ascx" TagName="MIF_MechantDetails_TSYS_CAYAN" TagPrefix="uc" %>

<%@ Register Src="UserControls/MIF_MechantDetails_SOFTEK.ascx" TagName="MIF_MechantDetails_SOFTEK"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_TNBCI_TSYS.ascx" TagName="MIF_MechantDetails_TNBCI_TSYS"
    TagPrefix="uc" %>
<%-- Remove payline code--%>
<%@ Register Src="UserControls/MIF_MerchantDetails_MONETARY.ascx" TagName="MIF_MerchantDetails_MONETARY"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_CLEARENT.ascx" TagName="MIF_MerchantDetails_CLEARENT"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_Greenbox.ascx" TagName="MIF_MerchantDetails_GREENBOX"
    TagPrefix="uc" %>

<%@ Register Src="UserControls/MIF_MerchantDetails_SignaPay.ascx" TagName="MIF_MerchantDetails_SignaPay"
    TagPrefix="uc" %>

<%@ Register Src="UserControls/MIF_MerchantDetails_ALLIEDWALLET.ascx" TagName="MIF_MerchantDetails_ALLIEDWALLET"
    TagPrefix="uc" %>

<%@ Register Src="UserControls/MIF_MerchantDetails_FultonDemo.ascx" TagName="MIF_MerchantDetails_FultonDemo"
    TagPrefix="uc" %>

<%@ Register Src="~/UserControls/MIF_MechantDetails_SPHERE.ascx" TagName="MIF_MerchantDetails_SPHERE"
    TagPrefix="uc" %>


<%@ Register Src="UserControls/MIF_MerchantDetails_RS2.ascx" TagName="MIF_MerchantDetails_RS2"
    TagPrefix="uc" %>

<%@ Register Src="UserControls/MIF_MerchantDetails_FIPS.ascx" TagName="MIF_MerchantDetails_FIPS"
    TagPrefix="uc" %>

<%@ Register Src="UserControls/MIF_MerchantDetails_PAYA.ascx" TagName="MIF_MerchantDetails_PAYA"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/MIF_MerchantDetails_MLS.ascx" TagName="MIF_MerchantDetails_MLS"
    TagPrefix="uc" %>

<%@ Register Src="UserControls/MIF_MerchantDetails_PCS.ascx" TagName="MIF_MerchantDetails_PCS"
    TagPrefix="uc" %>

<%@ Register Src="~/UserControls/MecchantProfileNavigator.ascx" TagPrefix="uc" TagName="MecchantProfileNavigator" %>
<%--TK39919 - Add Merchant Note--%>
<%@ Register Src="~/UserControls/MerchantNote.ascx" TagPrefix="uc" TagName="MerchantNote" %>
<%--44894 - VW- Merchant Note Default Preferences via User Mgmt Settings--%>
<%@ Register Src="~/UserControls/uxCaseHistory.ascx" TagPrefix="uc" TagName="CaseHistory" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxGridCSRComment">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGridCSRComment" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxAddComment">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGridCSRComment" />
                    <tek:AjaxUpdatedControl ControlID="uxExporterCommentTop" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxDrilldownGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxDrilldownGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <%--TK39919 - add action when filter--%>
    <uc:ReportFiltering ID="uxReportFiltering" runat="server" Mode="Mode1" OnFiltering="uxReportFiltering_Filtering" />

    <asp:PlaceHolder ID="uxPanelMerchantList" runat="server">
        <div class="row">
            <div class="col-xs-9">
                <uc:PageTitle ID="uxPageTitle2" runat="server" ReportTitle="" HasShowHierarchy="true"
                    HasFilteringOption="true" />

                <uc:PageTitle ID="uxPageTitleCM2" Visible="false" runat="server" PageTitle="Merchant Profile"
                    ReportTitle="Merchant Profile" meta:resourcekey="uxPageTitleCM2Resource1" />
            </div>
            <div class="col-xs-3 text-right">
                <asp:HyperLink ID="uxGoBack2" runat="server" NavigateUrl="#" Visible="false" Text=""
                    CssClass="link-back" meta:resourcekey="uxGoBack2Resource1" />
            </div>
        </div>
        <uc:UxExport ID="uxExportTop" GridID="uxDrilldownGrid" runat="server" IsOnTop="true" />
        <as:ASGrid ID="uxDrilldownGrid" runat="server" AllowPaging="True" AllowSorting="True" IsCacheTemplateFile="false"
            AutoGenerateColumns="False" AllowAutoCalculateTotalOnExport="false" AllowFilteringByColumn="false" OnPreRender="uxDrilldownGrid_PreRender"
            ASPagingMethod="SPASingleMethod" ShowPageTotal="false" ShowReportTotal="false" IsAutoExportTemplate="true"
            AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxDrilldownGridResource1">
            <MasterTableView>
                <Columns>
                    <%--For ALL Client--%>
                    <as:ASGridBoundColumn UniqueName="DrilldownColumn" HeaderText="Merchant ID" HeaderTooltip="Merchant ID"
                        DataField="Entity" SortExpression="Entity" ASFormat="StaticString" HeaderStyle-Width="130px" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="MerchantNumber" HeaderText="Merchant ID" HeaderTooltip="Merchant ID" HeaderStyle-Width="130px"
                        DataField="Entity" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="DBAName" HeaderText="Merchant Name" HeaderTooltip="Merchant Name" HeaderStyle-Width="150px"
                        DataField="EntityName" SortExpression="EntityName" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <%-- For Allied Wallet--%>
                    <as:ASGridBoundColumn UniqueName="CompanyName" HeaderTooltip="Company Name" ASFormat="DynamicString" HeaderStyle-Width="200px"
                        DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName" Visible="true" meta:resourcekey="ASGridBoundColumnResource64">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%-- End For Allied Wallet--%>



                    <%-- For FIPS Wallet--%>
                    <as:ASGridBoundColumn UniqueName="TenantName" HeaderTooltip="Tenant" ASFormat="DynamicString" HeaderStyle-Width="200px"
                        DataField="EntityNumber1" HeaderText="Tenant" SortExpression="TenantName" Visible="true" meta:resourcekey="ASGridBoundColumnResource92">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                    <as:ASGridBoundColumn UniqueName="SubMerchantName" HeaderTooltip="Sub-Merchant" ASFormat="DynamicString" HeaderStyle-Width="200px"
                        DataField="EntityNumber2" HeaderText="Sub-Merchant" SortExpression="SubMerchantName" Visible="true" meta:resourcekey="ASGridBoundColumnResource93">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%-- End For FIPS Wallet--%>

                    <as:ASGridBoundColumn UniqueName="SPHEREBackEndProcessor" HeaderText="Process Platform" HeaderTooltip="Process Platform" HeaderStyle-Width="130px"
                        DataField="BackEndProcessor" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource63">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPRBackEndProcessor" HeaderText="Process Platform" HeaderTooltip="Process Platform" HeaderStyle-Width="130px"
                        DataField="BackEndProcessor" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource63">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <%--For Cayan Client--%>
                    <as:ASGridBoundColumn UniqueName="CorporateName" HeaderText="Corporate Name" HeaderTooltip="Corporate Name" HeaderStyle-Width="130px"
                        DataField="CorporateName" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource56">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EntityNumber5" HeaderText="Bank" HeaderTooltip="Bank" HeaderStyle-Width="150px"
                        DataField="EntityNumber5" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EntityNumber6" HeaderText="Association" HeaderTooltip="Association" HeaderStyle-Width="150px"
                        DataField="EntityNumber6" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--END for Cayan Client--%>
                    <%--END For ALL Client--%>


                    <as:ASGridBoundColumn UniqueName="GreenboxBusinessChain" HeaderText="Business Chain" HeaderTooltip="Business Chain" HeaderStyle-Width="130px"
                        DataField="EntityNumber1" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource94">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                     <as:ASGridBoundColumn UniqueName="GreenboxBankChain" HeaderText="Bank Chain" HeaderTooltip="Bank Chain" HeaderStyle-Width="130px"
                        DataField="EntityNumber2" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource95">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                      <as:ASGridBoundColumn UniqueName="GreenboxCorporateChain" HeaderText="Corporate Chain" HeaderTooltip="Corporate Chain" HeaderStyle-Width="130px"
                        DataField="EntityNumber3" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource96">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                     <as:ASGridBoundColumn UniqueName="GreenboxAgentChain" HeaderText="Agent Chain" HeaderTooltip="Agent Chain" HeaderStyle-Width="130px"
                        DataField="EntityNumber4" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource97">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>



                    <%--For eVANCE Client--%>
                    <as:ASGridBoundColumn UniqueName="eVANCESponBank" HeaderText="Sponsoring Bank" HeaderTooltip="Sponsoring Bank" HeaderStyle-Width="130px"
                        DataField="EntityNumber1" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource58">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--  // Remove payline code--%>
                    <as:ASGridBoundColumn UniqueName="MONETARYBank" HeaderText="Bank" HeaderTooltip="Bank" HeaderStyle-Width="130px"
                        DataField="EntityNumber1" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="eVANCESales" HeaderText="Sales Branch" HeaderTooltip="Sales" HeaderStyle-Width="150px"
                        DataField="EntityNumber2" ASFormat="DynamicString" Visible="false" meta:resourcekey="ASGridBoundColumnResource59">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="eVANCEAssociation" HeaderText="Association" HeaderTooltip="Association" HeaderStyle-Width="130px"
                        DataField="EntityNumber3" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--END for eVANCE Client--%>

                    <%--For Clearent Client--%>
                    <as:ASGridBoundColumn UniqueName="ClearentSponBank" HeaderText="Sponsoring Bank" HeaderTooltip="Sponsoring Bank" HeaderStyle-Width="130px"
                        DataField="EntityNumber1" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource58">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ClearentSponBankBIN" HeaderText="Sponsoring Bank BIN" HeaderTooltip="Sponsoring Bank BIN" HeaderStyle-Width="130px"
                        DataField="EntityNumber2" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource62">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ClearentProcessPlatform" HeaderText="Process Platform" HeaderTooltip="Process Platform" HeaderStyle-Width="130px"
                        DataField="EntityNumber3" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource63">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="ResellerName" HeaderText="Reseller" HeaderTooltip="Reseller" HeaderStyle-Width="130px"
                        DataField="EntityNumber4" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource65">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="PartnerName" HeaderText="Partner" HeaderTooltip="Partner" HeaderStyle-Width="130px"
                        DataField="EntityNumber5" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource66">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--END for Clearent Client--%>

                    <%--For FIS Client--%>
                    <as:ASGridBoundColumn UniqueName="Bank" HeaderTooltip="Bank" ASFormat="StaticString" HeaderStyle-Width="70px"
                        DataField="EntityNumber1" HeaderText="Bank" Visible="true" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="Group" HeaderTooltip="Group" ASFormat="StaticString" HeaderStyle-Width="130px"
                        DataField="EntityNumber2" HeaderText="Group" Visible="false" meta:resourcekey="ASGridBoundColumnResource90">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="Association" HeaderTooltip="Association" ASFormat="StaticString" HeaderStyle-Width="130px"
                        DataField="REGN" HeaderText="Association" Visible="true" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                    <as:ASGridBoundColumn UniqueName="RS2Bank" HeaderText="Bank" HeaderTooltip="Bank" HeaderStyle-Width="130px"
                        DataField="EntityNumber1" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="RS2BankName" HeaderText="Bank Name" HeaderTooltip="Bank Name" HeaderStyle-Width="130px"
                        DataField="BankName" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource_BankName">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="Acquirer" HeaderText="Acquirer" HeaderTooltip="Acquirer" HeaderStyle-Width="130px"
                        DataField="EntityNumber2" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource_Acquirer">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="AcquirerName" HeaderText="Acquirer Name" HeaderTooltip="Acquirer Name" HeaderStyle-Width="130px"
                        DataField="AcquirerName" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource_AcquirerName">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SubAcquirer" HeaderText="Sub-Acquirer" HeaderTooltip="Sub-Acquirer" HeaderStyle-Width="130px"
                        DataField="EntityNumber3" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource_SubAcquirer">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SubAcquirerName" HeaderText="Sub-Acquirer Name" HeaderTooltip="Sub-Acquirer Name" HeaderStyle-Width="130px"
                        DataField="SubAcquirerName" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource_SubAcquirerName">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                    <as:ASGridBoundColumn UniqueName="MONETARYAssociation" HeaderText="Association" HeaderTooltip="Association" HeaderStyle-Width="130px"
                        DataField="EntityNumber2" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPHEREGroup" HeaderText="Group" HeaderTooltip="Group" HeaderStyle-Width="130px"
                        DataField="EntityNumber5" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResourceGroup">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                     <as:ASGridBoundColumn UniqueName="SPRGroup" HeaderText="Group" HeaderTooltip="Group" HeaderStyle-Width="130px"
                         DataField="EntityNumber5" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResourceGroup">
                         <ColumnValidationSettings>
                             <ModelErrorMessage Text=""></ModelErrorMessage>
                         </ColumnValidationSettings>

                         <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                     </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPHEREAssociation" HeaderText="Association" HeaderTooltip="Association" HeaderStyle-Width="130px"
                        DataField="EntityNumber3" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                     <as:ASGridBoundColumn UniqueName="SPRAssociation" HeaderText="Association" HeaderTooltip="Association" HeaderStyle-Width="130px"
                         DataField="EntityNumber3" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource5">
                         <ColumnValidationSettings>
                             <ModelErrorMessage Text=""></ModelErrorMessage>
                         </ColumnValidationSettings>

                         <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                     </as:ASGridBoundColumn>
                    
                    <as:ASGridBoundColumn UniqueName="SPHEREFundingMethod" HeaderText="Funding Method" HeaderTooltip="Funding Method" HeaderStyle-Width="130px"
                        DataField="EntityNumber2" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResourceFundingMethod">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPRFundingMethod" HeaderText="Funding Method" HeaderTooltip="Funding Method" HeaderStyle-Width="130px"
                        DataField="EntityNumber2" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResourceFundingMethod">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="ReferralPartner" HeaderText="Referral Partner" HeaderTooltip="Referral Partner" HeaderStyle-Width="130px"
                        DataField="EntityNumber4" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResourceReferralPartner">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPHEREReferralSource" HeaderText="Referral Source" HeaderTooltip="Referral Source" HeaderStyle-Width="160px"
                        DataField="EntityNumber6" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResourceReferralSource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPRReferralSource" HeaderText="Referral Source" HeaderTooltip="Referral Source" HeaderStyle-Width="160px"
                        DataField="EntityNumber6" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResourceReferralSource">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPHEREAddress" HeaderTooltip="Address" ASFormat="DynamicString" HeaderStyle-Width="200px"
                        DataField="Address" HeaderText="Address" SortExpression="Address" Visible="true" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPRAddress" HeaderTooltip="Address" ASFormat="DynamicString" HeaderStyle-Width="200px"
                        DataField="Address" HeaderText="Address" SortExpression="Address" Visible="true" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <%--eVANCE--%>
                    <as:ASGridBoundColumn UniqueName="eVANCEChain" HeaderText="Chain" HeaderTooltip="Chain"
                        DataField="ChainNumber" SortExpression="ChainNumber" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource39" HeaderStyle-Width="110px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Agent" HeaderText="Agent" HeaderTooltip="Agent"
                        DataField="EntityNumber4" SortExpression="EntityNumber4" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource17" HeaderStyle-Width="130px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="Chain" HeaderText="Chain" HeaderTooltip="Chain"
                        DataField="EntityNumber5" SortExpression="EntityNumber5" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource39" HeaderStyle-Width="150px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="MICChain" HeaderText="MIC Chain" HeaderTooltip="MIC Chain"
                        DataField="ChainNumber" SortExpression="ChainNumber" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource38" HeaderStyle-Width="110px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <%--END For FIS Client--%>
                    <%--For PAYA Client--%>
                    <as:ASGridBoundColumn UniqueName="PayaChain" HeaderText="Chain" HeaderTooltip="Chain"
                        DataField="ChainNumber" SortExpression="ChainNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource39" HeaderStyle-Width="150px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="PAYACSTaxID" HeaderTooltip="Tax ID" ASFormat="StaticString" HeaderStyle-Width="130px"
                        DataField="EncryptedTaxID" HeaderText="Tax ID" SortExpression="EncryptedTaxID"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource57">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="PAYAMSTaxID" HeaderTooltip="Tax ID" ASFormat="StaticString" HeaderStyle-Width="90px"
                        DataField="PartialTaxID" HeaderText="Tax ID" SortExpression="PartialTaxID"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource57">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="PAYAMCCSIC" HeaderText="MCC/SIC" HeaderTooltip="Merchant Category Code/Standard Industrial Classification" HeaderStyle-Width="110px"
                        DataField="MCCSIC" SortExpression="MCCSIC" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="PAYAPhone" HeaderTooltip="Phone" ASFormat="Phone" DataField="Phone" HeaderStyle-Width="100px"
                        HeaderText="Phone" SortExpression="Phone" Visible="true" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="PAYAEmail" HeaderTooltip="Email" ASFormat="DynamicString" HeaderStyle-Width="220px"
                        DataField="Email" HeaderText="Email" SortExpression="Email" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="PAYAOpenDate" HeaderTooltip="Open Date" ASFormat="Date"
                        DataField="OpenDate" HeaderText="Open Date" Visible="false" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="PAYAStatus" ASFormat="StaticString" DataField="MerchantStatus"
                        HeaderTooltip="Status" HeaderText="Status" SortExpression="MerchantStatus" meta:resourcekey="ASGridBoundColumnResource44"
                        HeaderStyle-Width="80px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>


                    <%--End for Paya Client--%>

                    <%--For IPMT Client--%>
                    <as:ASGridBoundColumn UniqueName="IPMTSysPrinAgent" HeaderText="SYS/PRIN/AGENT" HeaderTooltip="SYS/PRIN/AGENT" HeaderStyle-Width="140px"
                        DataField="EntityNumber3" SortExpression="EntityNumber3" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="NBANK" HeaderText="Bank" HeaderTooltip="Bank"
                        DataField="EntityNumber4" SortExpression="EntityNumber4" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource4" HeaderStyle-Width="110px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="NAGENT" HeaderText="Agent" HeaderTooltip="Agent"
                        DataField="EntityNumber5" SortExpression="EntityNumber5" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource17" HeaderStyle-Width="110px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="NCORP" HeaderText="Corporate" HeaderTooltip="Corporate"
                        DataField="EntityNumber6" SortExpression="EntityNumber6" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource42" HeaderStyle-Width="110px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="NCHAIN" HeaderText="Chain" HeaderTooltip="Chain"
                        DataField="EntityNumber7" SortExpression="EntityNumber7" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource41" HeaderStyle-Width="110px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--END For IPMT Client--%>
                    <%--For FDR Client--%>
                    <as:ASGridBoundColumn UniqueName="SysPrinAgent" HeaderText="SYS/PRIN/AGENT" HeaderTooltip="SYS/PRIN/AGENT" HeaderStyle-Width="120px"
                        DataField="EntityNumber3" SortExpression="EntityNumber3" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="SalesAgent" HeaderText="Sales Agent" HeaderTooltip="Sales Agent"
                        DataField="EntityNumber4" SortExpression="EntityNumber4" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Headquarter" HeaderText="Headquarter" HeaderTooltip="Headquarter"
                        DataField="Headquarter" SortExpression="Headquarter" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--END For FDR Client--%>
                    <%--For CAYAN Client--%>
                    <as:ASGridBoundColumn UniqueName="CayanSalesAgent" HeaderText="Sales Agent" HeaderTooltip="Sales Agent" HeaderStyle-Width="110px"
                        DataField="EntityNumber4" SortExpression="EntityNumber4" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--45292 - Add Hierarchy for Sales Agents--%>
                    <as:ASGridBoundColumn UniqueName="CayanSubSalesAgent" HeaderText="Sub-Sales Agent" HeaderTooltip="Sub-Sales Agent" HeaderStyle-Width="110px"
                        DataField="EntityNumber7" SortExpression="EntityNumber7" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource67">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--End 45292 - Add Hierarchy for Sales Agents--%>
                    <as:ASGridBoundColumn UniqueName="CayanChain" HeaderText="Chain" HeaderTooltip="Chain" HeaderStyle-Width="110px"
                        DataField="ChainNumber" SortExpression="ChainNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource39">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--END For CAYAN Client--%>
                    <%--For TSYS Client--%>
                    <as:ASGridBoundColumn UniqueName="ISO" HeaderText="ISO" HeaderTooltip="ISO" DataField="EntityNumber5"
                        SortExpression="EntityNumber5" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="SALESOFFICE" HeaderText="Sales Office" HeaderTooltip="Sales Office"
                        DataField="EntityNumber6" SortExpression="EntityNumber6" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="BANKNUMBER" HeaderText="Bank Number" HeaderTooltip="Bank Number"
                        DataField="EntityNumber7" SortExpression="EntityNumber7" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--END For TSYS Client--%>
                    <%--For MPS Client--%>
                    <as:ASGridBoundColumn UniqueName="CHN" HeaderTooltip="Chain Number" ASFormat="StaticString" HeaderStyle-Width="80px"
                        DataField="ChainNumber" HeaderText="Chain" SortExpression="ChainNumber" Visible="true" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="SecondaryAccessChain" HeaderTooltip="Secondary Access Chain" ASFormat="StaticString" HeaderStyle-Width="100px"
                        DataField="EntityNumber4" HeaderText="Secondary Access Chain" SortExpression="EntityNumber4" Visible="true" meta:resourcekey="ASGridBoundColumnResource91">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--For SNET Client--%>
                    <as:ASGridBoundColumn UniqueName="PartnerID" HeaderTooltip="Partner ID" ASFormat="StaticString"
                        DataField="EntityNumber1" HeaderText="Partner ID" SortExpression="EntityNumber1" HeaderStyle-Width="100px"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPHERECSTaxID" HeaderTooltip="Tax ID" ASFormat="StaticString" HeaderStyle-Width="130px"
                        DataField="EncryptedTaxID" HeaderText="Tax ID" SortExpression="EncryptedTaxID"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource57">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPRCSTaxID" HeaderTooltip="Tax ID" ASFormat="StaticString" HeaderStyle-Width="130px"
                        DataField="EncryptedTaxID" HeaderText="Tax ID" SortExpression="EncryptedTaxID"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource57">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="SPHEREMSTaxID" HeaderTooltip="Tax ID" ASFormat="StaticString" HeaderStyle-Width="90px"
                        DataField="PartialTaxID" HeaderText="Tax ID" SortExpression="PartialTaxID"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource57">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="SPRMSTaxID" HeaderTooltip="Tax ID" ASFormat="StaticString" HeaderStyle-Width="90px"
                        DataField="PartialTaxID" HeaderText="Tax ID" SortExpression="PartialTaxID"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource57">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                    </as:ASGridBoundColumn>


                    <as:ASGridBoundColumn UniqueName="SPHEREMCCSIC" HeaderText="MCC/SIC" HeaderTooltip="Merchant Category Code/Standard Industrial Classification" HeaderStyle-Width="110px"
                        DataField="MCCSIC" SortExpression="MCCSIC" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                    <as:ASGridBoundColumn UniqueName="SPRMCCSIC" HeaderText="MCC/SIC" HeaderTooltip="Merchant Category Code/Standard Industrial Classification" HeaderStyle-Width="110px"
                        DataField="MCCSIC" SortExpression="MCCSIC" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                    <as:ASGridBoundColumn UniqueName="Phone" HeaderTooltip="Phone" ASFormat="Phone" DataField="Phone" HeaderStyle-Width="100px"
                        HeaderText="Phone" SortExpression="Phone" Visible="true" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPHEREEmail" HeaderTooltip="Email" ASFormat="DynamicString" HeaderStyle-Width="220px"
                        DataField="Email" HeaderText="Email" SortExpression="Email" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SPREmail" HeaderTooltip="Email" ASFormat="DynamicString" HeaderStyle-Width="220px"
                        DataField="Email" HeaderText="Email" SortExpression="Email" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="Last4TaxID" HeaderTooltip="Last 4 Tax-ID" ASFormat="StaticString"
                        DataField="Last4TaxID" HeaderText="Last 4 Tax-ID" SortExpression="Last4TaxID"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--End For MPS Client--%>
                    <%--For PLANET Client--%>
                    <as:ASGridBoundColumn UniqueName="SalesUnit" HeaderTooltip="Sales Unit" ASFormat="DynamicString"
                        DataField="EntityNumber9" HeaderText="Sales Unit" SortExpression="EntityNumber9"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="PlanetAgent" HeaderTooltip="Agent" ASFormat="DynamicString"
                        DataField="EntityNumber10" HeaderText="Agent" SortExpression="EntityNumber10"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--End For PLANET Client--%>
                    <%--For ALL Client--%>
                    <as:ASGridBoundColumn UniqueName="MCCSIC" HeaderText="MCC/SIC" HeaderTooltip="Merchant Category Code/Standard Industrial Classification" HeaderStyle-Width="110px"
                        DataField="MCCSIC" SortExpression="MCCSIC" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Address" HeaderTooltip="Address" ASFormat="DynamicString" HeaderStyle-Width="200px"
                        DataField="Address" HeaderText="Address" SortExpression="Address" Visible="true" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--eVance--%>
                    <as:ASGridBoundColumn UniqueName="CSTaxID" HeaderTooltip="Tax ID" ASFormat="StaticString" HeaderStyle-Width="130px"
                        DataField="EncryptedTaxID" HeaderText="Tax ID" SortExpression="EncryptedTaxID"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource57">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="MSTaxID" HeaderTooltip="Tax ID" ASFormat="StaticString" HeaderStyle-Width="90px"
                        DataField="PartialTaxID" HeaderText="Tax ID" SortExpression="PartialTaxID"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource57">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="OpenDate" HeaderTooltip="Open Date" ASFormat="Date"
                        DataField="OpenDate" HeaderText="Open Date" Visible="false" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="MerchantStatus" HeaderText="Merchant Status" HeaderTooltip="Merchant Status"
                        DataField="MerchantStatus" SortExpression="MerchantStatus" ASFormat="StaticString"
                        HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--eVANCE--%>
                    <as:ASGridBoundColumn UniqueName="eVANCEStatus" ASFormat="StaticString" DataField="MerchantStatus"
                        HeaderTooltip="Status" HeaderText="Status" SortExpression="MerchantStatus" meta:resourcekey="ASGridBoundColumnResource44"
                        HeaderStyle-Width="80px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="eVANCEEmail" HeaderTooltip="Email" ASFormat="DynamicString" HeaderStyle-Width="220px" ItemStyle-CssClass="word-break"
                        DataField="Email" HeaderText="Email" SortExpression="Email" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="SIGNAPAY_MCCSIC" HeaderText="MCC/SIC" HeaderTooltip="Merchant Category Code/Standard Industrial Classification" HeaderStyle-Width="110px"
                        DataField="MCCSIC" Visible="false" SortExpression="MCCSIC" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                    <as:ASGridBoundColumn UniqueName="Status" ASFormat="StaticString" DataField="Status"
                        HeaderTooltip="Online Status" HeaderText="Online Status" SortExpression="Status"
                        HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--For FIS --%>
                    <as:ASGridBoundColumn UniqueName="OptIn" ASFormat="StaticString" DataField="OptIn"
                        HeaderTooltip="Opt In" HeaderText="Opt In" Visible="false" HeaderStyle-Width="60px" meta:resourcekey="ASGridBoundColumnResource23">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--End For FIS --%>
                    <as:ASGridBoundColumn HeaderText="User Name" DataField="UserID" UniqueName="UserID" HeaderStyle-Width="190px"
                        FilterControlWidth="160px" SortExpression="UserID" ASFormat="StaticString" HeaderTooltip="User Name" meta:resourcekey="ASGridBoundColumnResource60">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="left" />
                    </as:ASGridBoundColumn>
                    <%-- For Clearent--%>
                    <as:ASGridBoundColumn UniqueName="MCCSIC_Clearent" HeaderText="MCC/SIC" HeaderTooltip="Merchant Category Code/Standard Industrial Classification" HeaderStyle-Width="110px"
                        DataField="MCCSIC" SortExpression="MCCSIC" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%-- End For Clearent--%>

                    <as:ASGridBoundColumn UniqueName="LastBatchActivity" HeaderTooltip="Last Batch Activity" HeaderStyle-Width="80px"
                        ASFormat="Date" DataField="LastBatchActivity" HeaderText="Last Batch Activity"
                        SortExpression="LastBatchActivity" Visible="true" meta:resourcekey="ASGridBoundColumnResource24">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Email" HeaderTooltip="Email" ASFormat="DynamicString" HeaderStyle-Width="220px"
                        DataField="Email" HeaderText="Email" SortExpression="Email" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%-- END For ALL Client--%>
                    <%--For FULTON --%>
                    <as:ASGridBoundColumn UniqueName="ApprovalDate_Fulton" ASFormat="Date" DataField="ApprovalDate"
                        HeaderTooltip="Approval Date" HeaderText="Approval Date" Visible="false" meta:resourcekey="ASGridBoundColumnResource55">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ClosedDate_Fulton" ASFormat="Date" DataField="ClosedDate"
                        HeaderTooltip="Closed Date" HeaderText="Closed Date" Visible="false" meta:resourcekey="ASGridBoundColumnResource43">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="StatusExport_Fulton" ASFormat="StaticString" DataField="StatusExport"
                        HeaderTooltip="Status" HeaderText="Status" Visible="false" meta:resourcekey="ASGridBoundColumnResource44">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="SiteAccess_Fulton" ASFormat="StaticString" DataField="SiteAccess"
                        HeaderTooltip="Site Access" HeaderText="Site Access" Visible="false" meta:resourcekey="ASGridBoundColumnResource45">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EmailAddress_Fulton" ASFormat="StaticString" DataField="EmailAddress"
                        HeaderTooltip="Email Address" HeaderText="Email Address" Visible="false" meta:resourcekey="ASGridBoundColumnResource46">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EntityNumber1_Fulton" ASFormat="StaticString" DataField="EntityNumber1"
                        HeaderTooltip="Corporate" HeaderText="Corporate" Visible="false" meta:resourcekey="ASGridBoundColumnResource47">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="REGN_Fulton" ASFormat="StaticString" DataField="REGN"
                        HeaderTooltip="Bank" HeaderText="Bank" Visible="false" meta:resourcekey="ASGridBoundColumnResource48">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="PRIN_Fulton" ASFormat="StaticString" DataField="PRIN"
                        HeaderTooltip="Region" HeaderText="Region" Visible="false" meta:resourcekey="ASGridBoundColumnResource49">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EntityNumber4_Fulton" ASFormat="StaticString" DataField="EntityNumber4"
                        HeaderTooltip="Association Sub-Group" HeaderText="Association Sub-Group" Visible="false" meta:resourcekey="ASGridBoundColumnResource50">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EntityNumber5_Fulton" ASFormat="StaticString" DataField="EntityNumber5"
                        HeaderTooltip="Association" HeaderText="Association" Visible="false" meta:resourcekey="ASGridBoundColumnResource51">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EntityNumber6_Fulton" ASFormat="StaticString" DataField="EntityNumber6"
                        HeaderTooltip="Sales Agent Regional Manager" HeaderText="Sales Agent Regional Manager" Visible="false" meta:resourcekey="ASGridBoundColumnResource52">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EntityNumber7_Fulton" ASFormat="StaticString" DataField="EntityNumber7"
                        HeaderTooltip="Sales Agent Manager" HeaderText="Sales Agent Manager" Visible="false" meta:resourcekey="ASGridBoundColumnResource53">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="EntityNumber8_Fulton" ASFormat="StaticString" DataField="EntityNumber8"
                        HeaderTooltip="Sales Agent" HeaderText="Sales Agent" Visible="false" meta:resourcekey="ASGridBoundColumnResource54">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--End For FULTON --%>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="uxPanelDetail" runat="server" Visible="False">
        <div class="mif-container">
            <div class="mif-sidebar-container">
                <uc:MecchantProfileNavigator runat="server" ID="MecchantProfileNavigator" />
            </div>
            <div class="mif-content-container">
                <div class="row">
                    <div class="col-xs-9">
                        <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true"
                            HasFilteringOption="true" HasMarginBottom="true" />

                        <uc:PageTitle ID="uxPageTitleCM" Visible="false" runat="server" PageTitle="Merchant Profile"
                            ReportTitle="Merchant Profile" HasMarginBottom="true" meta:resourcekey="uxPageTitleCMResource1" />
                    </div>
                    <div class="col-xs-3 text-right">
                        <asp:HyperLink ID="uxGoBack" runat="server" NavigateUrl="#" Visible="false" Text=""
                            CssClass="link-back" meta:resourcekey="uxGoBackResource1" />
                    </div>
                </div>



                <div class="pos-relative">
                    <div class="pull-right">
                        <as:Button ID="btnExportMulti" runat="server" OnClientClick="return ShowExportModal(this)" Text="Export Multi Section" CssClass="btn btn-default report-export" meta:resourcekey="btnExportMultiResource" />
                        <as:Button ID="btnExportMultiSections" runat="server" />
                        <as:HiddenField runat="server" ID="hdfExportMultiSections" />
                    </div>
                    <%-- MerchantDetail --%>
                    <uc:MIF_MerchantDetails_FDR ID="uxMIF_MerchantDetails_FDR" runat="server" />
                    <uc:MIF_MerchantDetails_TSYS ID="uxMIF_MerchantDetails_TSYS" runat="server" />
                    <uc:MIF_MerchantDetails_PLANET ID="uxMIF_MerchantDetails_PLANET" runat="server" />
                    <uc:MIF_MerchantDetails_MPS ID="uxMIF_MerchantDetails_MPS" runat="server" />
                    <uc:MIF_MerchantDetails_ORI ID="uxMIF_MerchantDetails_ORI" runat="server" />
                    <uc:MIF_MerchantDetails_FIS ID="uxMIF_MerchantDetails_FIS" runat="server" />
                    <uc:MIF_MerchantDetails_EMS ID="uxMIF_MerchantDetails_EMS" runat="server" />
                    <uc:MIF_MerchantDetails_Fulton ID="uxMIF_MerchantDetails_Fulton" runat="server" />
                    <uc:MIF_MerchantDetails_PPI ID="uxMIF_MerchantDetails_PPI" runat="server" />
                    <uc:MIF_MerchantDetails_WRFC ID="uxMIF_MerchantDetails_WRFC" runat="server" />
                    <uc:MIF_MerchantDetails_TNBCI ID="uxMIF_MerchantDetails_TNBCI" runat="server" />
                    <uc:MIF_MerchantDetails_NORTH ID="uxMIF_MerchantDetails_NORTH" runat="server" />
                    <uc:MIF_MerchantDetails_NTS ID="uxMIF_MerchantDetails_NTS" runat="server" />
                    <uc:MIF_MerchantDetails_FISMPS ID="uxMIF_MerchantDetails_FISMPS" runat="server" />
                    <uc:MIF_MechantDetails_IPMT_Omaha ID="uxMIF_MechantDetails_IPMT_Omaha" runat="server" />
                    <uc:MIF_MechantDetails_IPMT_North ID="uxMIF_MechantDetails_IPMT_North" runat="server" />
                    <uc:MIF_MechantDetails_CAYAN ID="uxMIF_MechantDetails_CAYAN" runat="server" />
                    <uc:MIF_MechantDetails_TSYS_CAYAN ID="uxMIF_MechantDetails_TSYS_CAYAN" runat="server" />

                    <uc:MIF_MechantDetails_SOFTEK ID="uxMIF_MechantDetails_SOFTEK" runat="server" />
                    <uc:MIF_MechantDetails_TNBCI_TSYS ID="uxMIF_MechantDetails_TNBCI_TSYS" runat="server" />

                    <uc:MIF_MerchantDetails_MONETARY ID="uxMIF_MechantDetails_MONETARY" runat="server" />
                    <uc:MIF_MerchantDetails_CLEARENT ID="uxMIF_MerchantDetails_CLEARENT" runat="server" />
                    <uc:MIF_MerchantDetails_GREENBOX ID="uxMIF_MerchantDetails_GREENBOX" runat="server" />
                    <uc:MIF_MerchantDetails_SignaPay ID="uxMIF_MerchantDetails_SignaPay" runat="server" />
                    <uc:MIF_MerchantDetails_ALLIEDWALLET ID="uxMIF_MerchantDetails_ALLIEDWALLET" runat="server" />
                    <uc:MIF_MerchantDetails_FultonDemo ID="uxMIF_MerchantDetails_FultonDemo" runat="server" />
                    <uc:MIF_MerchantDetails_SPHERE ID="uxMIF_MerchantDetails_SPHERE" runat="server" />
                    <uc:MIF_MerchantDetails_RS2 ID="uxMIF_MerchantDetails_RS2" runat="server" />
                    <uc:MIF_MerchantDetails_FIPS ID="uxMIF_MerchantDetails_FIPS" runat="server" />
                    <uc:MIF_MerchantDetails_PAYA ID="uxMIF_MerchantDetails_PAYA" runat="server" />
                    <uc:MIF_MerchantDetails_MLS ID="uxMIF_MerchantDetails_MLS" runat="server" />
                    <uc:MIF_MerchantDetails_PCS ID="uxMIF_MerchantDetails_PCS" runat="server" />
                </div>
                <%-- MemoSection --%>
                <div class="row" id="memosection">
                    <div class="col-md-12">
                        <as:PlaceHolder runat="server" ID="uxMemoSection">
                            <uc:UxExport ID="uxExportMemolistTop" runat="server" GridID="uxMemoGrid" ShowPDF="false"
                                ShowCSV="false" GridHeader="Merchant Memos" ShowWord="false" ShowExcel="true" meta:resourcekey="uxExportMemolistTopResource1" />
                            <as:ASGrid ID="uxMemoGrid" runat="server" ASPagingMethod="SPASingleMethod" AutoGenerateColumns="false"
                                AllowPaging="True" AllowSorting="True" PageSize="10" ShowPageTotal="false" ShowReportTotal="false" CssClass="in" meta:resourcekey="uxMemoGridResource1">
                                <MasterTableView>
                                    <Columns>
                                        <as:ASGridBoundColumn SortExpression="DateLastActive" UniqueName="DateLastActive"
                                            HeaderText="Date Last Active" DataField="DateLastActive" ASFormat="Date" HeaderTooltip="Date Last Active"
                                            HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource26">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="SeqNo" UniqueName="SeqNo" HeaderText="Seq No"
                                            ASFormat="StaticString" DataField="SeqNo" HeaderTooltip="Seq No" meta:resourcekey="ASGridBoundColumnResource27">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="ClerkCode" UniqueName="ClerkCode" HeaderText="Clerk Code"
                                            ASFormat="StaticString" DataField="ClerkCode" HeaderTooltip="Clerk Code" meta:resourcekey="ASGridBoundColumnResource28">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="MemoData" UniqueName="MemoData" HeaderText="Memo Data"
                                            ASFormat="DynamicString" DataField="MemoData" HeaderTooltip="Memo Data" ItemStyle-HorizontalAlign="Left" meta:resourcekey="ASGridBoundColumnResource29">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="TimeEntered" UniqueName="TimeEntered" HeaderText="Time Entered"
                                            ASFormat="StaticString" DataField="TimeEntered" HeaderTooltip="Time Entered" meta:resourcekey="ASGridBoundColumnResource30">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="TermID" UniqueName="TermID" HeaderText="Term ID"
                                            ASFormat="StaticString" DataField="TermID" HeaderTooltip="Term ID" meta:resourcekey="ASGridBoundColumnResource31">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="OpCode" UniqueName="OpCode" HeaderText="Op Code"
                                            DataField="OpCode" ASFormat="StaticString" HeaderTooltip="Op Code" meta:resourcekey="ASGridBoundColumnResource32">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="LastAction" UniqueName="LastAction" HeaderText="Last Action"
                                            DataField="LastAction" ASFormat="StaticString" HeaderTooltip="Last Action" meta:resourcekey="ASGridBoundColumnResource33">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                        <as:ASGridBoundColumn SortExpression="LastUpdated" UniqueName="LastUpdated" HeaderText="Last Updated"
                                            DataField="LastUpdated" ASFormat="Date" HeaderTooltip="Last Updated" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource34">
                                            <ColumnValidationSettings>
                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                            </ColumnValidationSettings>

                                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                        </as:ASGridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </as:ASGrid>
                        </as:PlaceHolder>
                    </div>
                </div>
                <%--TK39919 - Merchant note --%>
                <div class="row">
                    <div class="col-md-12">
                        <uc:MerchantNote ID="ucMerchantNote" runat="server" />
                    </div>
                </div>
                <asp:PlaceHolder ID="uxPanelCaseHistory" runat="server">
                    <div class="row" id="casehistory">
                        <div class="col-md-12">
                            <uc:CaseHistory runat="server" ID="ucCaseHistory" />
                        </div>
                    </div>
                </asp:PlaceHolder>
                <%--<asp:PlaceHolder ID="uxPanelCaseHistory" runat="server">
                    <div id="casehistory"></div>
                    <iframe id="uxIframeCaseHitory" width="100%" height="400px" style="overflow-x: hidden;"
                        scrolling="<%= scrollingValue %>" frameborder="0" src="JumpToCase.aspx?type=15&p=1&xmlFilter=<%= GeneralFuncsLib.BuildXmlFilter(Tuple.Create("CaseType", ""))%>"
                        enableviewstate="false"></iframe>
                    <script type="text/javascript">
                        var MerchantProfile_CaseMgmtDomain = "<%= ConfigurationManager.AppSettings["ForceHttpsOnCm"]=="1"? WebSiteSettings.CaseMgmtDomain.Replace("http://","https://"): WebSiteSettings.CaseMgmtDomain%>";
                    </script>
                    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MerchantProfile_Case.js"></script>
                </asp:PlaceHolder>--%>
            </div>
        </div>
        <asp:Button ID="uxReloadHierachy" OnClick="uxReloadHierachy_Click" runat="server" CssClass="hide" />
    </asp:PlaceHolder>

    <!-- -->

    <div id="cidNonKeepFilterStatePage"></div>
    <% //CR #7485 - Dont mantain filter state of MIF page %>
    <script type="text/javascript">
        var uxSourceList = '<%= ucMerchantNote.FindControl("uxSourceList").ClientID%>'
        var uxRoleList = '<%= ucMerchantNote.FindControl("uxRoleList").ClientID%>'
        var uxAddedByList = '<%= ucMerchantNote.FindControl("uxAddedByList").ClientID%>'
        var uxStatuses = '<%= ucCaseHistory.FindControl("uxStatuses").ClientID%>'
        var uxTypes = '<%= ucCaseHistory.FindControl("uxTypes").ClientID%>'
        var uxPriorityLevel = '<%= ucCaseHistory.FindControl("uxPriorityLevel").ClientID%>'
        var AddNote_uxApplyFilter = '<%= ucMerchantNote.FindControl("uxApplyFilter").ClientID%>';
        var uxFinishSaveDefaultSettingCH = '<%= ucCaseHistory.FindControl("uxFinishSaveDefaultSettingCH").ClientID%>';
        var uxFinishSaveDefaultSettingMN = '<%= ucMerchantNote.FindControl("uxFinishSaveDefaultSettingMN").ClientID%>';
        var uxReloadHierachyId = '<%=uxReloadHierachy.ClientID%>';
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/MerchantProfile.js"></script>
    <script src="<% =ResolveUrl("~")%>res/js/common/sidenav.js"></script>

</asp:Content>
