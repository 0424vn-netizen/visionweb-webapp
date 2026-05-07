<%@ Page Title="Risk Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_RiskReport.aspx.cs" Inherits="rm_MCF_RiskReport" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/rm_MCF_NewRiskReport_MerchantInformation.ascx" TagName="RiskReportMerchantInformation"
    TagPrefix="uc" %>
<%@ Register TagName="RiskReportTransactionVolumeAnalysis" Src="~/UserControls/rm_MCF_NewRiskReportTransactionVolumeAnalysis.ascx"
    TagPrefix="uc" %>
<%@ Register TagName="TransactionHistory" Src="~/UserControls/rm_MCF_NewRiskReportTransactionHistory.ascx"
    TagPrefix="uc" %>
<%@ Register TagName="NewUxExport" Src="~/UserControls/rm_MCF_Report_UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="ACHReturnDetail" Src="~/UserControls/rm_MCF_ACHReturnDetails.ascx" TagPrefix="uc" %>
<%--39919 - Add--%>
<%@ Register Src="~/UserControls/rm_MCF_MerchantNote.ascx" TagPrefix="uc" TagName="MerchantNote" %>
<%--44894 - VW- Merchant Note Default Preferences via User Mgmt Settings--%>
<%@ Register Src="~/UserControls/uxCaseHistory.ascx" TagPrefix="uc" TagName="CaseHistory" %>
<%@ Register Src="~/UserControls/RelatedMerchants.ascx" TagPrefix="uc" TagName="RelatedMerchants" %>
<%@ Register Src="~/UserControls/rm_MCF_NewRiskReport_ForwardDelivery.ascx" TagPrefix="uc" TagName="ForwardDelivery" %>
<%@ Register Src="~/UserControls/rm_MCF_Chargebacks.ascx" TagPrefix="uc" TagName="Chargebacks" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <style>
        a.rwIcon {
            position: unset !important;
            margin: 0px !important;
            line-height: normal;
            width: 16px !important;
            height: 16px !important;
            font-size: unset !important;
            margin: 3px 5px 0 0 !important;
        }

        .RadWindow {
            padding: unset !important;
            border-style: unset !important;
        }

            .RadWindow.rwMinimizedWindow {
                height: 29.5px !important;
            }

        .padding_bottom_0px {
            padding-bottom: 0 !important;
        }
    </style>
    <tek:RadAjaxManagerProxy runat="server" ID="RadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="btnComment">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxRiskCommentsExport" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxBatchHistoryGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxBatchHistoryGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxRiskReportTransactionVolumeAnalysis">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxRiskReportTransactionVolumeAnalysis" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxBatchDateRange">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxBatchHistoryGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxExportBatchHistory" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxInvestigate">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxEscHistoryGrid" LoadingPanelID="uxLoadingPanelCustom" />
                    <tek:AjaxUpdatedControl ControlID="uxExportEscHistory" LoadingPanelID="uxInvisiblePanel" />
                    <tek:AjaxUpdatedControl ControlID="uxInvestigationForm" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnClickLink">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="hddValueLink" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxSubmit">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxPanelAddComment" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxChargebackGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxChargebackGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxAccountNumberClick">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxHiddenAccountNumberClick" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <as:Panel runat="server" ID="uxFilteringOptionsContainer"
        Width="100%" TemplateName="ascontainer_greyborder.tpl" meta:resourcekey="uxFilteringOptionsContainerResource1">
        <div class="row collapse report-filter-panel">
            <div class="col-md-12 report-filter">
                <div class="filter-block text-left">
                    <div class="filter-item">
                        <label>
                            <as:Literal ID="ltMerchantName" runat="server" Text="Merchant Name or Merchant ID" meta:resourcekey="ltMerchantNameResource1"></as:Literal></label>
                        <div>
                            <as:RadComboBox ID="uxMerchantList" runat="server" EnableEmbeddedBaseStylesheet="False"
                                DataValueField="DataKey" DataTextField="DataText" Width="370px"
                                AppendDataBoundItems="True" EnableLoadOnDemand="True"
                                EmptyMessage="enter at least 3 letters of the merchant numbers or names" OnItemsRequested="uxMerchantList_OnItemsRequested"
                                OnClientTextChange="uxMerchantList_OnClientTextChange" OnClientSelectedIndexChanged="uxMerchantList_OnClientSelectedIndexChanged"
                                MaxLength="100" OnClientKeyPressing="OnClientKeyPressing" meta:resourcekey="uxMerchantListResource1">
                                <Items>
                                    <tek:RadComboBoxItem runat="server" meta:resourcekey="RadComboBoxItemResource1" />
                                </Items>
                            </as:RadComboBox>
                        </div>
                    </div>
                </div>
                <div class="filter-block text-left">
                    <div class="filter-item">
                        <label>
                            <as:Literal ID="Literal1" runat="server" Text="Merchant ID" meta:resourcekey="Literal1Resource1"></as:Literal></label>
                        <div>
                            <as:RadTextBox ID="uxMerchantNumber" runat="server" Width="250px" MaxLength="16"
                                CssClass="rf_TextBox" LabelCssClass="" LabelWidth="64px" meta:resourcekey="uxMerchantNumberResource1" Resize="None">
                                <EmptyMessageStyle Resize="None" />
                                <ReadOnlyStyle Resize="None" />
                                <FocusedStyle Resize="None" />
                                <DisabledStyle Resize="None" />
                                <InvalidStyle Resize="None" />
                                <HoveredStyle Resize="None" />
                                <EnabledStyle Resize="None" />
                            </as:RadTextBox>
                        </div>
                    </div>
                    <div class="filter-item valign-bottom">
                        <as:Button ID="uxSearchButton" runat="server" Text="Submit" OnClick="uxSearchButton_OnClick"
                            OnClientClick="return ValidateData();" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSearchButtonResource1" />
                    </div>
                </div>
            </div>
        </div>
    </as:Panel>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter">
                <as:Literal ID="Literal2" runat="server" Text="FILTER" meta:resourcekey="Literal2Resource1"></as:Literal></span>
        </div>
    </div>

    <div class="mif-container fluid" id="MainContainer" runat="server">
        <div class="mif-content-container">
            <as:PlaceHolder ID="uxInfo" runat="server" Visible="False">
                <div class="mt-5x mb-3x">
                    <div class="form-risk-report ipmt">
                        <div class="mt-m-2x">
                            <h1 class="report-title">
                                <as:Literal ID="uxPageTitle" runat="server" Text="Risk Report" meta:resourcekey="uxReportTitleResource1"></as:Literal>
                            </h1>
                        </div>
                        <div class="risk-report-new-case">
                            <as:Button ID="uxbtnOpenNewCase3" runat="server" Text="Open New Case" CssClass="btn btn-default" Visible="false" meta:resourcekey="uxbtnOpenNewCaseResourceKey" class="btn btn-default" IsStandardButton="False" />
                        </div>
                        <div class="nav-report dark-blue mt-m-2x hide">
                            <asp:PlaceHolder ID="uxInvestigationPanel" runat="server">
                                <asp:HyperLink runat="server" ID="uxInvestigationAnchor" NavigateUrl="#investigationAnchor" CssClass="link-back" meta:resourcekey="uxInvestigationAnchorResource1" Text="Investigation" />
                                |
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" ID="PlaceHolder1">
                                <asp:HyperLink runat="server" ID="uxAddNoteAnchor" NavigateUrl="#addnote" CssClass="link-back" meta:resourcekey="uxAddNoteAnchorResource1" Text="Add Note" />
                                |
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" ID="PlaceHolder2">
                                <asp:HyperLink runat="server" ID="uxMerchantNoteAnchor" NavigateUrl="#merchantnotes" CssClass="link-back" meta:resourcekey="uxMerchantNoteAnchorResource1" Text="Merchant Note" />
                                |
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" ID="uxPlCaseHistory">
                                <asp:HyperLink runat="server" ID="uxLinkCaseHistory" NavigateUrl="#casehistory" CssClass="link-back" meta:resourcekey="uxCaseHistoryAnchorResource1" Text="Case History" />
                            </asp:PlaceHolder>

                            <asp:HyperLink runat="server" ID="uxRiskCommentAnchor" NavigateUrl="#riskcommentsAnchor" CssClass="link-back" meta:resourcekey="uxRiskCommentAnchorResource1" Text="Risk Comments" />
                            <asp:PlaceHolder runat="server" ID="uxPlSecurityReport">|
                                <asp:HyperLink runat="server" ID="uxSecurityReportLink" CssClass="link-back" meta:resourcekey="SecurityReportResource" Text="Security Report" />
                            </asp:PlaceHolder>

                            <asp:PlaceHolder runat="server" ID="uxPlMerchantProfileLink">|
                             <asp:HyperLink runat="server" ID="uxMerchantProfile" CssClass="link-back" NavigateUrl="#" onclick="redirectTo()" meta:resourcekey="MerchantProfileResource" Text="Merchant Profile" />
                            </asp:PlaceHolder>
                        </div>
                    </div>
                    <div class="form-merchant-info">
                        <uc:RiskReportMerchantInformation ID="uxRiskReportMerchantInformation" runat="server" />
                    </div>
                    <div class="report-add-note">
                        <div class="row" id="addnote">
                            <div class="col-md-12">
                                <h2 class="grid-title" data-toggle="collapse" data-target="#ciAddNote">
                                    <as:Literal ID="Literal3" runat="server" Text="Add Note" meta:resourcekey="uxAddNoteTitleResource1"></as:Literal>
                                </h2>

                                <asp:Panel runat="server" ID="uxPanelAddComment">
                                    <tek:RadEditor ID="uxComment" runat="server" EditModes="Design" ContentFilters="ConvertCharactersToEntities, ConvertToXhtml, FixEnClosingP"
                                        StripFormattingOptions="MSWordRemoveAll" OnClientLoad="OnClientLoad"
                                        ToolsFile="~/App_Data/RadEditorConfig.xml" Height="200px" Width="100%" RenderMode="Lightweight" Font-Names="Arial" OnClientPasteHtml="onHtmlPaste" EnableResize="true">
                                        <CssFiles>
                                            <tek:EditorCssFile Value="~/res/css/Editor.css" />
                                        </CssFiles>
                                    </tek:RadEditor>
                                </asp:Panel>
                                <div class="bottom-error">
                                    <label class="error display-none" id="ciCommentsMsg"></label>
                                </div>

                                <div class="box-footer-editor">
                                    <span id="counter"></span>
                                    <div class="ml-auto text-right">
                                        <asp:Button ID="uxSubmit" runat="server" Text="Submit" OnClick="uxSubmit_Click" OnClientClick="return Validate();" class="btn btn-default" meta:resourcekey="uxSubmitResource1" />
                                        <%--[43454] - Encrypting the comments field in VW Case Management --%>
                                        <asp:HiddenField ID="hdCardDetected" runat="server" />
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>
                </div>

                <div class="risk-report-bg">
                    <div class="content risk-report-tab">
                        <div class="in report-mng-panel">
                            <div class="row">
                                <div class="col-md-12">
                                    <tek:RadTabStrip ID="uxTabView" OnClientTabSelected="tabSelected" runat="server" CssClass="as-animated-tabstrip" SelectedIndex="0">
                                        <Tabs>
                                            <tek:RadTab Text="Transactions" Value="transactions" Selected="true" meta:resourcekey="transactionsResource1">
                                            </tek:RadTab>
                                            <tek:RadTab Text="Notes and Case History" Value="notescase" meta:resourcekey="notescaseResource1">
                                            </tek:RadTab>
                                            <tek:RadTab Text="Batch and ACH Returns" Value="batchach" meta:resourcekey="batchachResource1">
                                            </tek:RadTab>
                                            <tek:RadTab Text="Related Merchants" Value="relatedMerchants">
                                            </tek:RadTab>
                                            <tek:RadTab Text="Forward Delivery" Value="forwardDelivery">
                                            </tek:RadTab>
                                            <tek:RadTab Text="Chargebacks (90 days)" Value="chargebacks90Days">
                                            </tek:RadTab>
                                        </Tabs>
                                    </tek:RadTabStrip>
                                </div>
                            </div>
                            <div class="risk-report-tab-content">
                                <div id="transactions" class="uc-content">
                                    <%-----------------------Transaction volumne analysis info--------------------------------%>
                                    <uc:RiskReportTransactionVolumeAnalysis ID="uxRiskReportTransactionVolumeAnalysis" runat="server" />

                                    <%-----------------------Transaction info--------------------------------%>
                                    <uc:TransactionHistory ID="uxTransactionHistory" runat="server" IsBuildLink="false"></uc:TransactionHistory>
                                </div>
                                <div id="notescase" class="hide uc-content">
                                    <%-----------------------Merchant notes--------------------------------%>
                                    <div>
                                        <uc:MerchantNote ID="ucMerchantNote" runat="server" />
                                    </div>
                                    <%-----------------------Case History--------------------------------%>
                                    <asp:PlaceHolder ID="uxPanelCaseHistory" runat="server">
                                        <div class="row" id="casehistory">
                                            <div class="col-md-12">
                                                <uc:CaseHistory runat="server" ID="ucCaseHistory" />
                                            </div>
                                        </div>
                                    </asp:PlaceHolder>
                                </div>
                                <div id="batchach" class="hide uc-content">
                                    <%-----------------------Batchhistory info--------------------------------%>
                                    <div class="row">
                                        <div class="col-md-10">
                                            <h2 class="grid-title">
                                                <span data-toggle="collapse" data-target="#batchhistory">
                                                    <as:Literal ID="Literal5" runat="server" Text="Batch History" meta:resourcekey="Literal7Resource1"></as:Literal>
                                                </span>
                                            </h2>
                                        </div>
                                        <div class="col-md-2 text-right">
                                            <uc:NewUxExport ID="uxExportBatchHistory" runat="server" GridID="uxBatchHistoryGrid"
                                                GridHeader="Batch History" GridTitle="Batch History"
                                                FileName="Risk Management - Merchant Information - Risk Report - Batch History"
                                                ShowWord="false" ShowPDF="false" meta:resourcekey="uxExportBatchHistoryResourcekey1" />
                                        </div>
                                    </div>
                                    <div id="batchhistory" class="in">
                                        <div class="text-right mt-2x">
                                            <div class="control-inline mt-1x">
                                                <as:Literal ID="Literal16" runat="server" meta:resourcekey="LiteralDaysResource1" Text="Days:"></as:Literal>
                                            </div>
                                            <div class="pull-right mb-3x ">
                                                <tek:RadComboBox ID="uxBatchDateRange" runat="server"
                                                    MarkFirstMatch="true" EnableEmbeddedBaseStylesheet="false" Width="60px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="uxBatchDateRange_SelectedIndexChanged">
                                                </tek:RadComboBox>
                                            </div>
                                        </div>
                                        <as:ASGrid ID="uxBatchHistoryGrid" runat="server" AutoGenerateColumns="False" ASPagingMethod="SPASingleMethod1"
                                            AllowSorting="true" AllowPaging="true" GridLines="None" IsIntruder="true" IntruderSourceName="uxBatchHistoryGrid"
                                            GridName="Risk Management - Merchant Information - Risk Report - Batch History" IsAutoExportTemplate="true"
                                            ShowPageTotal="false" XOverFlowable="true" HeaderStyle-Width="100px" CssClass="in" meta:resourcekey="uxBatchHistoryGridResource1"
                                            AllowSortFilterWhenExport="true">
                                            <MasterTableView>
                                                <Columns>
                                                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                                                        HeaderTooltip="Report Date" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource37">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <%--<as:ASGridBoundColumn HeaderText="Batch Date" DataField="BatchDate" UniqueName="BatchDate"
                     HeaderTooltip="Batch Date" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource67">
                     <ColumnValidationSettings>
                         <ModelErrorMessage Text=""></ModelErrorMessage>
                     </ColumnValidationSettings>

                     <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                 </as:ASGridBoundColumn>--%>
                                                    <as:ASGridBoundColumn HeaderText="Terminal #" DataField="TerminalNumber" UniqueName="TerminalNumber"
                                                        HeaderTooltip="Termial Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource38">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="Source" DataField="Source" UniqueName="Source"
                                                        HeaderTooltip="File Source" ASFormat="StaticString" HeaderStyle-Width="200px" meta:resourcekey="ASGridBoundColumnResource39">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center" Width="200px"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="Batch #" DataField="BatchNumber" UniqueName="BatchNumber"
                                                        HeaderTooltip="Batch Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource40">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="# Trans" DataField="TotalTransactionCount" UniqueName="TotalTransactionCount"
                                                        ASFormat="Integer" SortExpression="TotalTransactionCount" ASIsTotalColumn="true"
                                                        HeaderTooltip="Transaction Count" meta:resourcekey="ASGridBoundColumnResource41">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="# Keyed" DataField="TotalKeyedEntryCount" UniqueName="TotalKeyedEntryCount"
                                                        ASFormat="Integer" ASIsTotalColumn="true" SortExpression="TotalKeyedEntryCount"
                                                        HeaderTooltip="Keyed Entry Count" meta:resourcekey="ASGridBoundColumnResource42">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="Sales" DataField="TotalBankCardSalesAmount" UniqueName="TotalBankCardSalesAmount"
                                                        ASIsTotalColumn="true" HeaderTooltip="Bank Card Sales" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource43">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="Returns" DataField="TotalBankCardReturnsAmount"
                                                        ASFormat="Currency" UniqueName="TotalBankCardReturnsAmount" ASIsTotalColumn="true"
                                                        HeaderTooltip="Bank Card Returns" meta:resourcekey="ASGridBoundColumnResource44">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="Net" DataField="TotalBankCardNetAmount" UniqueName="TotalBankCardNetAmount"
                                                        ASIsTotalColumn="true" HeaderTooltip="Bank Card Net" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource45">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="Sales" DataField="TotalNonBankCardSalesAmount"
                                                        ASFormat="Currency" UniqueName="TotalNonBankCardSalesAmount" ASIsTotalColumn="true"
                                                        HeaderTooltip="Non Bank Card Sales" meta:resourcekey="ASGridBoundColumnResource46">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="Returns" DataField="TotalNonBankCardReturnsAmount"
                                                        ASFormat="Currency" UniqueName="TotalNonBankCardReturnsAmount" ASIsTotalColumn="true"
                                                        HeaderTooltip="Non Bank Card Returns" meta:resourcekey="ASGridBoundColumnResource47">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="Net" DataField="TotalNonBankCardNetAmount" UniqueName="TotalNonBankCardNetAmount"
                                                        ASIsTotalColumn="true" ASFormat="Currency" HeaderTooltip="Non Bank Card Net" meta:resourcekey="ASGridBoundColumnResource48">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="Sales" DataField="TotalSaleAmount" UniqueName="TotalSaleAmount"
                                                        ASFormat="Currency" SortExpression="TotalSaleAmount" ASIsTotalColumn="true" HeaderTooltip="Total Sales" meta:resourcekey="ASGridBoundColumnResource49">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="Returns" DataField="TotalReturnAmount" UniqueName="TotalReturnAmount"
                                                        SortExpression="TotalReturnAmount" ASFormat="Currency" ASIsTotalColumn="true"
                                                        HeaderTooltip="Total Returns" meta:resourcekey="ASGridBoundColumnResource50">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                    <as:ASGridBoundColumn HeaderText="Net" DataField="TotalNetAmount" UniqueName="TotalNetAmount"
                                                        ASFormat="Currency" SortExpression="TotalNetAmount" ASIsTotalColumn="true" HeaderTooltip="Total Net" meta:resourcekey="ASGridBoundColumnResource51">
                                                        <ColumnValidationSettings>
                                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                                        </ColumnValidationSettings>

                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    </as:ASGridBoundColumn>
                                                </Columns>
                                            </MasterTableView>

                                            <HeaderStyle Width="100px"></HeaderStyle>

                                        </as:ASGrid>
                                    </div>


                                    <%-----------------------ACH Return Detail--------------------------------%>
                                    <uc:ACHReturnDetail ID="uxACHReturnDetail" runat="server" Visible="false"></uc:ACHReturnDetail>
                                    <asp:PlaceHolder ID="uxInvestigationGridPanel" runat="server">
                                        <%-----------------------Investigation info--------------------------------%>
                                        <span id="investigationAnchor"></span>
                                        <div class="row ipmt">
                                            <div class="col-md-10">
                                                <h2 class="grid-title control-inline" data-toggle="collapse" data-target="#investigation">
                                                    <as:Literal ID="Literal17" runat="server" Text="Investigation Comments" meta:resourcekey="Literal16Resource1"></as:Literal>
                                                </h2>
                                            </div>
                                            <div class="col-md-2">
                                                <uc:NewUxExport ID="uxExportEscHistory" runat="server" GridID="uxEscHistoryGrid" GridHeader="Investigation Comments"
                                                    FileName="Risk Management - Merchant Information - Risk Report - Investigation Comments"
                                                    ShowPDF="false" ShowWord="false" meta:resourcekey="uxExportEscHistoryResource1" />
                                            </div>
                                        </div>
                                        <div id="investigation" class="in">
                                            <as:PlaceHolder ID="addInvestigation" runat="server">
                                                <as:Button ID="uxAddInvestigation" runat="server" Text="Add Investigation"
                                                    CssClass="btn btn-default mb-5x mt-3x js-investigation" meta:resourcekey="Literal6Resource1" OnClientClick="showWin(); return false;" />
                                            </as:PlaceHolder>
                                            <as:ASGrid ID="uxEscHistoryGrid" runat="server" AutoGenerateColumns="False" ShowHeader="true" IsAutoExportTemplate="true"
                                                AllowSorting="False" GridLines="None" ShowFooter="false" SortingSettings-SortToolTip="" MasterTableView-TableLayout="Fixed" XOverFlowable="true"
                                                ShowToolTip="true" AllowPaging="false" ASPagingMethod="None" AllowExportAtWebServices="false" HeaderStyle-Width="100px" CssClass="in" meta:resourcekey="uxEscHistoryGridResource1">
                                                <SortingSettings SortToolTip=""></SortingSettings>

                                                <MasterTableView AllowSorting="false">
                                                    <Columns>
                                                        <as:ASGridBoundColumn HeaderText="Tkt" DataField="EscalationID" UniqueName="EscalationID"
                                                            ASFormat="StaticString" SortExpression="EscalationID" HeaderStyle-Width="60px" meta:resourcekey="ASGridBoundColumnResource52">
                                                            <ColumnValidationSettings>
                                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                                            </ColumnValidationSettings>

                                                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                                        </as:ASGridBoundColumn>
                                                        <as:ASGridBoundColumn HeaderText="Open Date" DataField="EscalationDate" UniqueName="EscalationDate"
                                                            ASFormat="DateAndTime12Hours" SortExpression="EscalationDate" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource53">
                                                            <ColumnValidationSettings>
                                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                                            </ColumnValidationSettings>

                                                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                                        </as:ASGridBoundColumn>
                                                        <as:ASGridBoundColumn HeaderText="Assigned To" DataField="AssignedTo" UniqueName="AssignedTo"
                                                            ASFormat="StaticString" HeaderTooltip="" SortExpression="AssignedTo" meta:resourcekey="ASGridBoundColumnResource54">
                                                            <ColumnValidationSettings>
                                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                                            </ColumnValidationSettings>

                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        </as:ASGridBoundColumn>
                                                        <as:ASGridBoundColumn HeaderText="Follow Up Date" DataField="FollowupDate" UniqueName="FollowupDate"
                                                            ASFormat="Date" HeaderTooltip="Follow Up Date" SortExpression="FollowupDate" meta:resourcekey="ASGridBoundColumnResource55">
                                                            <ColumnValidationSettings>
                                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                                            </ColumnValidationSettings>

                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        </as:ASGridBoundColumn>
                                                        <as:ASGridBoundColumn HeaderText="Closed" DataField="Closed" UniqueName="Closed"
                                                            ASFormat="StaticString" SortExpression="Closed" HeaderStyle-Width="60px" meta:resourcekey="ASGridBoundColumnResource56">
                                                            <ColumnValidationSettings>
                                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                                            </ColumnValidationSettings>

                                                            <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                                        </as:ASGridBoundColumn>
                                                        <as:ASGridBoundColumn HeaderText="Changed Date" DataField="ChangeDate" UniqueName="ChangeDate"
                                                            ASFormat="DateAndTime12Hours" SortExpression="ChangeDate" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource57">
                                                            <ColumnValidationSettings>
                                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                                            </ColumnValidationSettings>

                                                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                                        </as:ASGridBoundColumn>
                                                        <as:ASGridBoundColumn HeaderText="Changed By" DataField="ChangedBy" UniqueName="ChangedBy"
                                                            ASFormat="StaticString" SortExpression="ChangedBy" meta:resourcekey="ASGridBoundColumnResource58">
                                                            <ColumnValidationSettings>
                                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                                            </ColumnValidationSettings>

                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        </as:ASGridBoundColumn>
                                                        <as:ASGridBoundColumn HeaderText="Reason" DataField="Reason" UniqueName="Reason"
                                                            ASFormat="StaticString" SortExpression="Reason" meta:resourcekey="ASGridBoundColumnResource59">
                                                            <ColumnValidationSettings>
                                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                                            </ColumnValidationSettings>

                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        </as:ASGridBoundColumn>
                                                        <as:ASGridBoundColumn HeaderText="Status" DataField="Status" UniqueName="Status"
                                                            HeaderStyle-Width="80px" ASFormat="StaticString" SortExpression="Status" meta:resourcekey="ASGridBoundColumnResource60">
                                                            <ColumnValidationSettings>
                                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                                            </ColumnValidationSettings>

                                                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                                        </as:ASGridBoundColumn>
                                                        <as:ASGridBoundColumn HeaderText="Resolution" DataField="Resolution" UniqueName="Resolution"
                                                            ASFormat="StaticString" HeaderTooltip="" SortExpression="Resolution" meta:resourcekey="ASGridBoundColumnResource61">
                                                            <ColumnValidationSettings>
                                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                                            </ColumnValidationSettings>

                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        </as:ASGridBoundColumn>
                                                        <as:ASGridBoundColumn HeaderText="Savings/Loss" DataField="SavingsLossAmt" UniqueName="SavingLoss"
                                                            HeaderStyle-HorizontalAlign="Center" ASFormat="Currency" HeaderTooltip="Savings/Loss" HeaderStyle-Width="120px"
                                                            SortExpression="SavingsLossAmt" ItemStyle-HorizontalAlign="Right" Visible="false" meta:resourcekey="ASGridBoundColumnSavingsLossAmt">
                                                        </as:ASGridBoundColumn>
                                                        <as:ASGridBoundColumn HeaderText="Comments" DataField="Comments" UniqueName="Comments" HeaderStyle-CssClass="text-center"
                                                            ASFormat="DynamicString" HeaderTooltip="" SortExpression="Comments" HeaderStyle-Width="300px" meta:resourcekey="ASGridBoundColumnResource62">
                                                            <ColumnValidationSettings>
                                                                <ModelErrorMessage Text=""></ModelErrorMessage>
                                                            </ColumnValidationSettings>
                                                        </as:ASGridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>

                                                <HeaderStyle Width="100px"></HeaderStyle>

                                            </as:ASGrid>
                                        </div>

                                    </asp:PlaceHolder>
                                </div>

                                <div id="relatedMerchants" class="hide uc-content">
                                    <%-----------------------Related Merchants--------------------------------%>
                                    <div>
                                        <uc:RelatedMerchants ID="uxRelatedMerchants" runat="server" />
                                    </div>
                                </div>
                                <div id="forwardDelivery" class="hide uc-content">
                                    <%-----------------------Forward Delivery--------------------------------%>
                                    <div>
                                        <uc:ForwardDelivery ID="uxForwardDelivery" runat="server" />
                                    </div>
                                </div>
                                <div id="chargebacks90Days" class="hide uc-content">
                                    <%-----------------------Charbacks 90 Days--------------------------------%>
                                    <div class="height-24" data-loading-chargeback="true"></div>
                                    <asp:Panel ID="pnlChargeback" runat="server" CssClass="in" Style="min-height: 50px">
                                        <uc:Chargebacks ID="uxChargebacks" runat="server" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </as:PlaceHolder>

        </div>
    </div>
    <tek:RadWindow runat="server" ID="AddInvestigationModal" Behaviors="Move, Minimize, Close"
        AutoSize="true" OnClientCommand="winOnClientCommand" Modal="false"
        OnClientDragStart="winOnClientDragStart" VisibleStatusbar="false" OnClientActivate="winOnClientActivate"
        KeepInScreenBounds="true" OnClientDragEnd="winOnClientDragEnd"
        OnClientClose="winOnClientClose" OnClientBeforeClose="winOnClientBeforeClose">
        <ContentTemplate>
            <as:PlaceHolder ID="plhInvestigation" runat="server">
                <div id="contentInvestigation">
                    <asp:Panel runat="server" ID="uxInvestigationForm" meta:resourcekey="uxInvestigationFormResource1">
                        <table class="params-item">
                            <colgroup>
                                <col style="width: 130px" />
                                <col />
                            </colgroup>
                            <tr id="uxInvStartedDateInput" runat="server" visible="false">
                                <td>
                                    <label class="first">
                                        <as:Literal ID="Literal7" runat="server" Text="Date Investigation Started:" meta:resourcekey="Literal7"></as:Literal>
                                    </label>
                                </td>
                                <td class="dark-blue">
                                    <as:Literal ID="uxStartedDate" runat="server" />
                                </td>
                            </tr>
                            <tr id="uxInvCloseInput" runat="server" visible="false">
                                <td>
                                    <label>
                                        <as:Literal ID="Literal8" runat="server" Text="Close Investigation:" meta:resourcekey="Literal8"></as:Literal>
                                    </label>
                                </td>
                                <td style="padding-top: 8px">
                                    <as:CheckBox ID="uxCloseInvestigation" onclick="clickCloseInves()" runat="server" />
                                </td>
                            </tr>
                            <!-- Reason -->
                            <tr>
                                <td>
                                    <label id="ciReason">
                                        <as:Literal ID="Literal9" runat="server" Text="Reason:" meta:resourcekey="Literal9Resource1"></as:Literal></label>
                                </td>
                                <td>
                                    <div class="control-inline-no-margin">
                                        <as:RadComboBox ID="uxReasonList" runat="server" OnClientDropDownOpening="riskCbxCustomDropdown"
                                            DataValueField="EscalationReasonID" DataTextField="Reason" Width="232px" meta:resourcekey="uxReasonListResource1">
                                        </as:RadComboBox>
                                    </div>
                                    <div class="bottom-error">
                                        <label class="error display-none" id="ciReasonMsg"></label>
                                    </div>
                                </td>
                            </tr>
                            <!-- -->
                            <tr>
                                <td>
                                    <label id="ciStatus">
                                        <as:Literal ID="Literal10" runat="server" Text="Status:" meta:resourcekey="Literal10Resource1"></as:Literal></label>
                                </td>
                                <td>
                                    <div class="control-inline-no-margin">
                                        <as:RadComboBox ID="uxStatusList" runat="server" OnClientDropDownOpening="riskCbxCustomDropdown"
                                            DataValueField="EscalationStatusID" DataTextField="Status" Width="232px" meta:resourcekey="uxStatusListResource1">
                                        </as:RadComboBox>
                                    </div>
                                    <div class="bottom-error">
                                        <label class="error display-none" id="ciStatusMsg"></label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="ciAssigned">
                                        <as:Literal ID="Literal11" runat="server" Text="Assigned To:" meta:resourcekey="Literal11Resource1"></as:Literal></label>
                                </td>
                                <td>
                                    <div class="control-inline-no-margin">
                                        <as:RadComboBox ID="uxAssignedToList" runat="server" DataValueField="DataKey" DataTextField="DataText" OnClientDropDownOpening="riskCbxCustomDropdown"
                                            Width="232px" meta:resourcekey="uxAssignedToListResource1">
                                        </as:RadComboBox>
                                    </div>
                                    <div class="bottom-error">
                                        <label class="error display-none" id="ciAssignedMsg"></label>
                                    </div>
                                </td>
                            </tr>
                            <!-- Follow up Date -->
                            <tr runat="server" id="pnlFollowupdate">
                                <td>
                                    <label id="ciFollowUpdate">
                                        <as:Literal ID="Literal12" runat="server" Text="Follow Up Date:" meta:resourcekey="Literal12Resource1"></as:Literal></label>
                                </td>
                                <td style="padding-top: 8px">
                                    <div class="control-inline narrow valign-middle">
                                        <as:CheckBox ID="uxIsFollowupDate" onclick="clickFollowUpdate()" runat="server" />
                                    </div>
                                    <div class="control-inline-no-margin">
                                        <as:RadDatePicker runat="server" ID="uxFollowUpdate">
                                            <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" FastNavigationStep="12" ShowRowHeaders="False"></Calendar>

                                            <DateInput DisplayDateFormat="M/d/yyyy" DateFormat="M/d/yyyy" LabelWidth="64px" Width="">
                                                <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                                <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                                <FocusedStyle Resize="None"></FocusedStyle>

                                                <DisabledStyle Resize="None"></DisabledStyle>

                                                <InvalidStyle Resize="None"></InvalidStyle>

                                                <HoveredStyle Resize="None"></HoveredStyle>

                                                <EnabledStyle Resize="None"></EnabledStyle>
                                            </DateInput>

                                            <DatePopupButton CssClass="" ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                        </as:RadDatePicker>
                                    </div>
                                    <div class="bottom-error">
                                        <label class="error display-none" id="ciFollowUpdateMsg"></label>
                                    </div>
                                </td>
                            </tr>
                            <!-- -->
                            <tr id="uxInvResolution" runat="server" visible="false">
                                <td>
                                    <label id="ciResolution">
                                        <as:Literal ID="Literal13" runat="server" Text="Resolution:" meta:resourcekey="ResolutionResource1"></as:Literal></label>
                                </td>
                                <td>
                                    <div class="control-inline-no-margin">
                                        <as:RadComboBox ID="uxResolutionList" runat="server" DataValueField="ResolutionID" OnClientDropDownOpening="riskCbxCustomDropdown"
                                            DataTextField="Resolution" Width="232px">
                                        </as:RadComboBox>
                                    </div>
                                    <div class="bottom-error">
                                        <label class="error display-none" id="ciResolutionMsg"></label>
                                    </div>
                                </td>
                            </tr>
                            <tr id="uxSavingsLossInvestigation" runat="server" visible="false">
                                <td>
                                    <label id="Label1">
                                        <as:Literal ID="Literal15" runat="server" Text="Saving/Loss:" meta:resourcekey="GainLossResource1"></as:Literal></label>
                                </td>
                                <td style="padding-top: 8px">
                                    <tek:RadNumericTextBox ID="uxSavingsLoss" Width="232px" runat="server" MaxLength="9" Type="Currency" NegativeStyle-ForeColor="Red">
                                    </tek:RadNumericTextBox>
                                </td>
                            </tr>
                        </table>
                        <as:TextBox ID="uxComments" CssClass="rf_TextBox" runat="server" Height="100px" Width="100%"
                            TextMode="MultiLine" MaxLength="1000" meta:resourcekey="uxCommentsResource1"></as:TextBox>
                        <div class="bottom-error">
                            <label class="error display-none" id="cidMessageComments"></label>
                        </div>
                        <div id="buttons">
                            <div class="text-right">
                                <as:Button ID="uxInvestigate" runat="server" Text="Investigate" OnClick="uxInvestigate_OnClick"
                                    OnClientClick="return ValidateInvestigate(this);" CssClass="btn btn-default" meta:resourcekey="uxInvestigateResource1" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <as:RadAjaxPanel ID="uxAjaxpnl" runat="server" meta:resourcekey="uxAjaxpnlResource1">
                </as:RadAjaxPanel>
            </as:PlaceHolder>
        </ContentTemplate>
    </tek:RadWindow>

    <as:Button ID="btnProcess" runat="server" Style="display: none;" IsStandardButton="True"
        OnClick="btnProcess_Click" meta:resourcekey="btnProcessResource1" />
    <as:HiddenField ID="hddProcessData" runat="server" />
    <as:Button ID="btnClickLink" runat="server" Style="display: none;" IsStandardButton="True"
        OnClick="btnClickLink_Click" meta:resourcekey="btnProcessResource1" />
    <as:HiddenField ID="hddValueLink" runat="server" />
    <as:HiddenField ID="uxHiddenAccountNumberClick" runat="server" />
    <as:Button ID="uxAccountNumberClick" runat="server" IsStandardButton="True" OnClick="uxAccountNumberClick_Click" CssClass="hide" />
    <as:RadCodeBlock ID="JavaScript" runat="server">
        <script type="text/javascript">
            var uxCommentsClientID = '<%=uxComments.ClientID%>';
            var rm_RiskReport_uxBtnAddInvestigation = '<%= uxAddInvestigation.ClientID %>';
            var rm_RiskReport_uxCloseInvestigation = '<%= uxCloseInvestigation.ClientID %>';
            var rm_RiskReport_IsCheckFollowUpdate = '<%= IsCheckFollowUpdate %>';
            var rm_RiskReport_CurrentFollowUpdateDate = '<%= CurrentFollowUpdateDate %>';
            var rm_RiskReport_uxIsFollowupDate = '<%= uxIsFollowupDate.ClientID %>';
            var rm_RiskReport_uxFollowUpdate = '<%= uxFollowUpdate.ClientID %>';
            var rm_RiskReport_uxReasonList = '<%= uxReasonList.ClientID %>';
            var rm_RiskReport_uxStatusList = '<%= uxStatusList.ClientID %>';
            var rm_RiskReport_uxAssignedToList = '<%= uxAssignedToList.ClientID %>';
            var rm_RiskReport_uxResolutionList = '<%= uxResolutionList.ClientID %>';
            var rm_RiskReport_IsRequiredReason = '<%= IsRequiredReason.ToString().ToLower() %>';
            var addInvestigationModal = '<%= AddInvestigationModal.ClientID %>';
            var rm_RiskReport_uxMerchantList = '<%= uxMerchantList.ClientID %>';
            var rm_RiskReport_uxMerchantNumber = '<%= uxMerchantNumber.ClientID %>';
            var rm_RiskReport_uxSearchButton = '<%= uxSearchButton.ClientID %>';
            var rm_RiskReport_hddProcessData = '<%= hddProcessData.ClientID %>';
            var rm_RiskReport_btnProcess = '<%= btnProcess.ClientID %>';
            var rm_RiskReport_uxBatchHistoryGrid = '<%= uxBatchHistoryGrid.ClientID %>';
            var rm_RiskReport_uxFilteringOptionsContainer = '<%= uxFilteringOptionsContainer.ClientID %>';
            var rm_RiskReport_Generic_CheckLengthOfMerchantNumber = "<%=Resources.MessageManager.Generic_CheckLengthOfMerchantNumber %>";
            var rm_RiskReport_Generic_CheckMerchantNumber_Numeric = "<%=Resources.MessageManager.Generic_CheckMerchantNumber_Numeric %>";
            var rm_RiskReport_js = '<%= GetLocalResourceObject("Literal1Resource1.Text").ToString() %>';
            var rm_RiskReport_InvalidFormatMerchantID_js = '<%= GetLocalResourceObject("uxInvalidFormatMerchantID.Text").ToString() %>';
            var rm_RiskReport_js_MustSelectReason = '<%= GetLocalResourceObject("rm_RiskReport_js_MustSelectReason").ToString() %>';
            var rm_RiskReport_js_MustSelectStatus = '<%= GetLocalResourceObject("rm_RiskReport_js_MustSelectStatus").ToString() %>';
            var rm_RiskReport_js_MustSelectAssignedTo = '<%= GetLocalResourceObject("rm_RiskReport_js_MustSelectAssignedTo").ToString() %>';
            var rm_RiskReport_js_MustSelectResolution = '<%= GetLocalResourceObject("rm_RiskReport_js_MustSelectResolution").ToString() %>';
            var rm_RiskReport_js_FollowUpDateFormat = '<%= GetLocalResourceObject("rm_RiskReport_js_FollowUpDateFormat").ToString() %>';
            var rm_RiskReport_js_FollowUpDateNotPast = '<%= GetLocalResourceObject("rm_RiskReport_js_FollowUpDateNotPast").ToString() %>';
            var rm_RiskReport_js_CommentNotExceed1000 = '<%= GetLocalResourceObject("rm_RiskReport_js_CommentNotExceed1000").ToString() %>';
            var rm_RiskReport_js_GridHeaderBS = '<%= GetLocalResourceObject("rm_RiskReport_js_GridHeaderBS").ToString() %>';
            var rm_RiskReport_js_GridHeaderBC = '<%= GetLocalResourceObject("rm_RiskReport_js_GridHeaderBC").ToString() %>';
            var rm_RiskReport_js_GridHeaderNBC = '<%= GetLocalResourceObject("rm_RiskReport_js_GridHeaderNBC").ToString() %>';
            var rm_RiskReport_js_GridHeaderToTal = '<%= GetLocalResourceObject("rm_RiskReport_js_GridHeaderToTal").ToString() %>';
            var rm_RiskReport_js_TextNotAllowed = '<%= GetLocalResourceObject("rm_RiskReport_js_TextNotAllowed").ToString() %>';
            var rm_RiskReport_js_ValidationSpecialCharacter = '<%= GetLocalResourceObject("rm_RiskReport_js_ValidationSpecialCharacter").ToString() %>';
            var rm_RiskReport_js_TitleAddInvestigation = '<%= GetLocalResourceObject("Literal6Resource1.Text").ToString() %>';
            var IS_IPMT_CLIENT = '<%= SessionManager.CurrentClient.Equals(WebSiteConstants.IPMT_CLIENT) %>';

            var uxSourceList = '<%= ucMerchantNote.FindControl("uxSourceList").ClientID%>'
            var uxRoleList = '<%= ucMerchantNote.FindControl("uxRoleList").ClientID%>'
            var uxAddedByList = '<%= ucMerchantNote.FindControl("uxAddedByList").ClientID%>'
            var uxStatuses = '<%= ucCaseHistory.FindControl("uxStatuses").ClientID%>'
            var uxTypes = '<%= ucCaseHistory.FindControl("uxTypes").ClientID%>'
            var uxPriorityLevel = '<%= ucCaseHistory.FindControl("uxPriorityLevel").ClientID%>'
            var AddNote_uxApplyFilter = '<%= ucMerchantNote.FindControl("uxApplyFilter").ClientID%>';
            var uxFinishSaveDefaultSettingCH = '<%= ucCaseHistory.FindControl("uxFinishSaveDefaultSettingCH").ClientID%>';
            var uxFinishSaveDefaultSettingMN = '<%= ucMerchantNote.FindControl("uxFinishSaveDefaultSettingMN").ClientID%>';
            var rm_RiskReport_hddValueLink = '<%= hddValueLink.ClientID %>';
            var rm_RiskReport_btnClickLink = '<%= btnClickLink.ClientID %>';
            var rm_RiskReport_MerchantProfileURL = '<%= ResolveUrl("~/")%>MerchantProfile.aspx';
            var MerchantProfile_uxComment = "<%=uxComment.ClientID %>";
            var AddNote_js_Required = '<%= GetLocalResourceObject("AddNote_js_Required").ToString() %>';
            var AddNote_js_msg1 = '<%= GetLocalResourceObject("AddNote_js_msg1").ToString() %>';
            var AddNote_js_Characters = '<%= GetLocalResourceObject("AddNote_js_Characters").ToString() %>';
            var AddNote_uxSubmit = "<%=uxSubmit.UniqueID %>";

            var rm_MCF_RiskReport_uxHiddenAccountNumberClick = '<%=uxHiddenAccountNumberClick.ClientID %>';
            var rm_MCF_RiskReport_uxAccountNumberClick = "<%=uxAccountNumberClick.ClientID %>";
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_RiskReport.js"></script>
        <script src="<% =ResolveUrl("~")%>res/js/common/sidenav.js"></script>
    </as:RadCodeBlock>
</asp:Content>
