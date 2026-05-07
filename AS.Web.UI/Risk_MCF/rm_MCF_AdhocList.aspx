<%@ Page Title="Adhoc Results" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_AdhocList.aspx.cs" Inherits="As.VisionWeb.Web.RiskManagementAdhocList" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTile" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="Export" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="ProgressBar" Src="~/UserControls/ProgressBar.ascx" TagPrefix="uc" %>
<%@ Register TagName="SiteID_Selector" Src="~/UserControls/SiteID_Selector.ascx"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxAssignmentGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxAssignmentGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnReview">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="btnReview" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnRebind">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxAssignmentGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnChangeMerchantWorked">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="btnChangeMerchantWorked" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxLnkCreateAssignemnt">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxLnkCreateAssignemnt" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnViewResult">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="pnlReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btPost">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="btPost" />
                    <tek:AjaxUpdatedControl ControlID="uxGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <%-- Page Title --%>
    <div class="row">
        <div class="col-md-12 no-margin-bottom">
            <uc:PageTile ID="uxPageTitle" runat="server" ReportTitle="Adhoc Results" meta:resourcekey="uxPageTitleResource1" HasFilteringOption="false" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 action-container text-left">
            <as:LinkButton ID="uxLnkCreateAssignemnt" runat="server" Text="Create new adhoc report"
                OnClick="uxLnkCreateAssignemnt_Click" CssClass="btn btn-default" meta:resourcekey="uxLnkCreateAssignemntResource1" />
        </div>
        <div class="col-xs-6 action-container text-right">
            <as:LinkButton ID="uxLikShowColorLegend" runat="server" Text="Color Legend"
                OnClientClick="ShowPopupModal('rm_MCF_ColorLegend.aspx','auto'); return false;"
                CssClass="btn btn-default" meta:resourcekey="uxLikShowColorLegendResource1" />
        </div>
    </div>
    <div class="height-20"></div>
    <%-- Grid Report--%>
    <div class="row">
        <div class="col-md-12">
            <as:ASGrid ID="uxAssignmentGrid" runat="server" AutoGenerateColumns="False" ShowHeader="true"
                PageSize="10" AllowPaging="true" GridLines="None" ShowFooter="false" ShowToolTip="true" CssClass="in" meta:resourcekey="uxAssignmentGridResource1">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn HeaderText="ID" HeaderTooltip="ID" Visible="true" ASFormat="Integer" HeaderStyle-Width="60px"
                            ItemStyle-HorizontalAlign="Center" UniqueName="ID" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn DataField="AssignmentID" UniqueName="AssignmentID" AllowSorting="false"
                            ASFormat="StaticString" Display="false" meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Adhoc Name" DataField="AssignmentName" UniqueName="AssignmentName"
                            HeaderTooltip="Adhoc Name" AllowSorting="false" ASFormat="StaticString" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Proc. Status" DataField="ProcessingStatus" UniqueName="ProcessingStatus"
                            AllowSorting="false" HeaderTooltip="Processing Status" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Last Proc. Status Date" DataField="ProcessingStatusDate"
                            UniqueName="ProcessingStatusDate" AllowSorting="false" HeaderTooltip="Last Processing Status Date"
                            ASFormat="DateAndTime12Hours" ItemStyle-Wrap="false" HeaderStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResource5">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>

                            <ItemStyle Wrap="False"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Result" DataField="AssignmentID" UniqueName="Result"
                            AllowSorting="false" ASFormat="StaticString" HeaderTooltip="Click to view result"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource6">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </as:ASGridBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </div>
    </div>

    <%-- Barometer Report --%>
    <div class="row">
        <div class="col-md-12">
            <as:Panel ID="pnlReportGrid" runat="server" meta:resourcekey="pnlReportGridResource1">
                <uc:Export ID="uxExporter" runat="server" GridID="uxGrid" OnExportingReportHeader="uxGrid_DoReportHeader" ShowPDF="false"/>
                <as:ASGrid ID="uxGrid" runat="server" AllowPaging="True" HeaderStyle-Width="50px" IsAutoExportTemplate="true"
                    AllowSorting="True" AutoGenerateColumns="False" ASPagingMethod="SPASingleMethod"
                    AllowSortFilterWhenExport="True" GridName="Barometer Report" Width="100%" OnInit="uxGrid_Init" OnPreRender="uxGrid_PreRender"
                    meta:resourcekey="uxGridResource1" SortingSettings-SortToolTip="">
                    <ClientSettings>
                        <Scrolling FrozenColumnsCount="2" />
                    </ClientSettings>
                    <MasterTableView EnableColumnsViewState="False" NoDetailRecordsText="No data found." NoMasterRecordsText="No data found." PageSize="20">
                        <Columns>                          
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto"
                                ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="MerchantName" HeaderStyle-Width="220px"
                                FilterControlAltText="Filter MerchantName column" HeaderText="Merchant Name(*)" 
                                IsResetTotal="False"  SortExpression="MerchantName" 
                                UniqueName="MerchantName">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="merchantName" HorizontalAlign="Left" Wrap="True" />
                            </as:ASGridBoundColumn>                           
                            <as:ASGridBoundColumn HeaderText="BA" DataField="BusinessAge" UniqueName="BusinessAge"
                                ItemStyle-CssClass="item2" HeaderStyle-Width="80px"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                                ItemStyle-Wrap="true" HeaderTooltip="Business Age" meta:resourcekey="ASGridBoundColumnResource0">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A"
                                ASFormat="Integer" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No"
                                DataField="RiskScore" FilterControlAltText="Filter RiskScore column" HeaderText="RS" HeaderStyle-Width="60px"
                                HeaderTooltip="Risk Score" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource9"
                                SortExpression="RiskScore" UniqueName="RiskScore">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Percentage0Digits" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" 
                                DataField="VolumePercent" FilterControlAltText="Filter VolumePercent column" 
                                HeaderText="V%" HeaderTooltip="Volume %" IsResetTotal="False" HeaderStyle-Width="80px"
                                meta:resourcekey="ASGridBoundColumnResource10" SortExpression="VolumePercent" UniqueName="VolumePercent">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>


                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" HeaderStyle-Width="80px"
                                ASDefaultNullValue="&mdash;" ASFormat="Percentage" ASIsTotalColumn="False" ASTotalFormat="Auto" 
                                ASTrueFalseText="Yes/No" DataField="ContractualVolume" FilterControlAltText="Filter ContractualVolume column" 
                                HeaderText="CV%" HeaderTooltip="Contractual Volume %" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResourceContractualVolume" SortExpression="ContractualVolume" UniqueName="ContractualVolume">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Width="120px" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>


                            <as:ASGridBoundColumn AllowEncodeOnExporting="False"  ASDefaultNullValue="N/A" ASFormat="Percentage0Digits"
                                 ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No"
                                 DataField="AverageTicketPercent" FilterControlAltText="Filter AverageTicketPercent column" 
                                HeaderText="AT%" HeaderTooltip="Average Ticket %" IsResetTotal="False" 
                                meta:resourcekey="ASGridBoundColumnResource11" SortExpression="AverageTicketPercent" 
                                UniqueName="AverageTicketPercent" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Percentage0Digits" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No"
                                 DataField="AuthorizationPercent" FilterControlAltText="Filter AuthorizationPercent column" HeaderText="A%"
                                 HeaderTooltip="Authorization %" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource12" 
                                SortExpression="AuthorizationPercent" UniqueName="AuthorizationPercent" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Number" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No"
                                 DataField="AttritionScore" FilterControlAltText="Filter AttritionScore column" HeaderText="ARS | P187"
                                 HeaderTooltip="ARS | P187" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource32" 
                                SortExpression="AttritionScore" UniqueName="AttritionScore" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>

                              <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Number" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No"
                                 DataField="ReserveScore" FilterControlAltText="Filter RiskReserveScore column" HeaderText="RRS | P188"
                                 HeaderTooltip="RRS | P188" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource33" 
                                SortExpression="ReserveScore" UniqueName="ReserveScore" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Percentage0Digits" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" 
                                DataField="DeclinedAuthorizationPercent" FilterControlAltText="Filter DeclinedAuthorizationPercent column" 
                                HeaderText="DA%" HeaderTooltip="Declined Authorization %" IsResetTotal="False" 
                                meta:resourcekey="ASGridBoundColumnResource13" SortExpression="DeclinedAuthorizationPercent" 
                                UniqueName="DeclinedAuthorizationPercent" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Integer" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" 
                                DataField="RepeatAuthorizationCount" FilterControlAltText="Filter RepeatAuthorizationCount column"
                                 HeaderText="#RA" HeaderTooltip="Repeat Authorization Count" IsResetTotal="False" 
                                meta:resourcekey="ASGridBoundColumnResource14" SortExpression="RepeatAuthorizationCount" 
                                UniqueName="RepeatAuthorizationCount" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Integer" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No"
                                 DataField="TodayForeignCardCount" FilterControlAltText="Filter TodayForeignCardCount column"
                                 HeaderText="FC" HeaderTooltip="Today's Foreign Card Count" IsResetTotal="False"
                                 meta:resourcekey="ASGridBoundColumnResource15" SortExpression="TodayForeignCardCount" 
                                UniqueName="TodayForeignCardCount" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A"
                                 ASFormat="Percentage0Digits" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" 
                                DataField="KeyPercent" FilterControlAltText="Filter KeyPercent column" HeaderText="K%" 
                                HeaderTooltip="Keyed Percent" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource16" 
                                SortExpression="KeyPercent" UniqueName="KeyPercent" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="True" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Percentage0Digits" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" 
                                DataField="EvenDollarTransactionPercent" FilterControlAltText="Filter EvenDollarTransactionPercent column" 
                                HeaderText="=%" HeaderTooltip="Even Dollar Transaction %" IsResetTotal="False" 
                                meta:resourcekey="ASGridBoundColumnResource17" SortExpression="EvenDollarTransactionPercent" 
                                UniqueName="EvenDollarTransactionPercent" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Percentage0Digits" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" 
                                DataField="DuplicateDollarTransactionPercent" FilterControlAltText="Filter DuplicateDollarTransactionPercent column" 
                                HeaderText="DD%" HeaderTooltip="Duplicate Dollar Transaction %" IsResetTotal="False" 
                                meta:resourcekey="ASGridBoundColumnResource18" SortExpression="DuplicateDollarTransactionPercent" 
                                UniqueName="DuplicateDollarTransactionPercent" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Integer" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="DuplicateBin"
                                 FilterControlAltText="Filter DuplicateBin column" HeaderText="DB" HeaderTooltip="Duplicate Bin" IsResetTotal="False"
                                 meta:resourcekey="ASGridBoundColumnResource19" SortExpression="DuplicateBin" UniqueName="DuplicateBin" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Integer" 
                                ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="NegativeBatchCount" 
                                FilterControlAltText="Filter NegativeBatchCount column" HeaderText="#-" HeaderTooltip="Negative Batch Count"
                                 IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource20" SortExpression="NegativeBatchCount"
                                 UniqueName="NegativeBatchCount" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Integer" 
                                ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="ZeroBatchCount" 
                                FilterControlAltText="Filter ZeroBatchCount column" HeaderText="#0" HeaderTooltip="Zero Batch Count" 
                                IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource21" SortExpression="ZeroBatchCount" 
                                UniqueName="ZeroBatchCount" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Currency"
                                ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="TodayVolume" HeaderStyle-Width="80px"
                                FilterControlAltText="Filter TodayVolume column" HeaderText="TV" HeaderTooltip="Today's Total Volume"
                                IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource22" SortExpression="TodayVolume" UniqueName="TodayVolume">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Currency"
                                 ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="TodayFirstTimeRetrievalVolume" 
                                FilterControlAltText="Filter TodayFirstTimeRetrievalVolume column" HeaderText="RV"
                                 HeaderTooltip="Today's First Time Retrieval Volume" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource23"
                                 SortExpression="TodayFirstTimeRetrievalVolume" UniqueName="TodayFirstTimeRetrievalVolume" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Currency" 
                                ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="TodayChargebackVolume" 
                                FilterControlAltText="Filter TodayChargebackVolume column" HeaderText="CB" HeaderTooltip="Today's Chargeback Volume" 
                                IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource24" SortExpression="TodayChargebackVolume"
                                 UniqueName="TodayChargebackVolume" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Percentage0Digits" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="ReturnPercent"
                                 FilterControlAltText="Filter ReturnPercent column" HeaderText="R%" HeaderTooltip="Return %" IsResetTotal="False"
                                 meta:resourcekey="ASGridBoundColumnResource25" SortExpression="ReturnPercent" UniqueName="ReturnPercent" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn ASDefaultNullValue="N/A" 
                                ASFormat="Currency" DataField="ACHReturnAmount"
                                 SortExpression="ACHReturnAmount" UniqueName="ACHReturnAmount" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Currency"
                                ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="TodayHighestTransactionAmount"
                                FilterControlAltText="Filter TodayHighestTransactionAmount column" HeaderText="MT$" HeaderStyle-Width="80px"
                                HeaderTooltip="Today's Highest Transaction Amount" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource26"
                                SortExpression="TodayHighestTransactionAmount" UniqueName="TodayHighestTransactionAmount">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Integer" ASIsTotalColumn="False" 
                                ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="TodayTransactionCount" FilterControlAltText="Filter TodayTransactionCount column"
                                 HeaderText="#T" HeaderTooltip="Today's Transaction Count" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource27" 
                                SortExpression="TodayTransactionCount" UniqueName="TodayTransactionCount" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Integer"
                                 ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="TodayBatchCount" 
                                FilterControlAltText="Filter TodayBatchCount column" HeaderText="#B" HeaderTooltip="Today's Batch Count" 
                                IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource28" SortExpression="TodayBatchCount" 
                                UniqueName="TodayBatchCount" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Integer"
                                 ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="SingleCardTransToday" 
                                FilterControlAltText="Filter SingleCardTransToday column" HeaderText="SC" HeaderTooltip="Similar Card Transactions Today" 
                                IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource29" SortExpression="SingleCardTransToday" 
                                UniqueName="SingleCardTransToday" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="StaticString"
                                 ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="SICCode" 
                                FilterControlAltText="Filter SIC column" HeaderText="SIC" HeaderTooltip="Standard Industry Code" IsResetTotal="False" 
                                meta:resourcekey="ASGridBoundColumnResource30" SortExpression="SICCode" UniqueName="SICCode" >
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto"
                                ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="ProfileDescription" HeaderStyle-Width="80px"
                                FilterControlAltText="Filter Profile column" HeaderText="Profile" HeaderTooltip="Profile Description"
                                IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource31" SortExpression="ProfileDescription" UniqueName="ProfileDescription">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Left" Wrap="True" />
                            </as:ASGridBoundColumn>

                             <as:ASGridBoundColumn AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" 
                                ASFormat="Percentage0Digits" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" DataField="TodayPrepaidCardSalesPercent"
                                 FilterControlAltText="Filter TodayPrepaidCardSalesPercent column" HeaderText="R%" HeaderTooltip="Return %" IsResetTotal="False"
                                 meta:resourcekey="ASGridBoundColumnResource25" SortExpression="TodayPrepaidCardSalesPercent" UniqueName="TodayPrepaidCardSalesPercent" HeaderStyle-Width="80px">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </as:ASGridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </as:Panel>
        </div>
    </div>

    <div class="display-none">
        <as:Button ID="btnRebind" runat="Server" OnClick="btnRebind_Click" IsStandardButton="False" meta:resourcekey="btnRebindResource1" />
        <as:Button ID="btnViewResult" runat="Server" OnClick="btnViewResult_Click" IsStandardButton="False" meta:resourcekey="btnViewResultResource1" />
        <as:Button ID="btnReview" runat="Server" OnClick="btnReview_Click" IsStandardButton="False" meta:resourcekey="btnReviewResource1" />
    </div>
    <as:HiddenField ID="hddUpdateMerchantWorked" runat="server" />
    <as:HiddenField ID="hddAssignID" runat="Server" />
    <as:ASRadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var rm_AdhocList_hddAssignID = '<%=hddAssignID.ClientID %>';
            var rm_AdhocList_btnViewResult = '<%=btnViewResult.ClientID %>';
            var rm_AdhocList_btnRebind = '<%=btnRebind.ClientID %>';
            var rm_AdhocList_btnReview = '<%=btnReview.ClientID %>';
            var rm_AdhocList_hddUpdateMerchantWorked = '<%=hddUpdateMerchantWorked.ClientID %>';
            var rm_AdhocList_uxGrid = '<%=uxGrid.ClientID%>';
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_AdhocList.js"></script>
    </as:ASRadCodeBlock>
</asp:Content>
