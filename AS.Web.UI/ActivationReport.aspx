<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActivationReport.aspx.cs" Inherits="ActivationReport" MasterPageFile="~/MasterPage.master" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ActivationReport_Filtering.ascx" TagName="ActivationFilter" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxAddFilter">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxPnDateFilter" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxBtnDeleteFilter">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxPnDateFilter" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxBtnUpdateBatchAmount">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxPnBatchAmount" LoadingPanelID="uxInvisiblePanel" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxSearch">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxPlActivationReport" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <!--Filtering Options-->
    <div class="row collapse report-filter-panel">
        <div class="col-md-12 report-filter">
            <div class="filter-block">
                <as:Panel ID="uxPnDateFilter" runat="server">
                    <table>
                        <tr>
                            <td></td>
                            <td class="text-left">
                                <as:Literal ID="ltSearchBy" runat="server" Text="Search by:" meta:resourcekey="ltSearchByResources"></as:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td class="text-left pr-4">
                                <as:RadComboBox ID="uxDateTypeFirstFilter" runat="server"></as:RadComboBox>
                            </td>
                            <td class="text-left">
                                <as:RadComboBox ID="uxDateFirstFilter" OnClientSelectedIndexChanged="ChangeDateOption" data-date-id="divDateFirst" data-date-range-id="divDateRangeFirst" runat="server"></as:RadComboBox>
                            </td>
                            <td class="text-left">
                                <div class="filter-item" id="divDateFirst">
                                    <as:RadDatePicker ID="uxDateFirst" runat="server" Width="128px">
                                        <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                        <DateInput ID="DateInput4" runat="server" onclick="ShowCalendar('1', true)" onkeypress="return SearchEnterOnTextbox(event);" />
                                    </as:RadDatePicker>
                                </div>
                                <div id="divDateRangeFirst" style="display: none;">
                                    <div class="filter-item">
                                        <as:RadDatePicker ID="uxFromDateFirst" runat="server" Width="128px">
                                            <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                            <DateInput ID="DateInput5" runat="server" onclick="ShowCalendar('2', true)" onkeypress="return SearchEnterOnTextbox(event);" />
                                        </as:RadDatePicker>
                                    </div>
                                    <div class="filter-item">
                                        <as:RadDatePicker ID="uxToDateFirst" runat="server" Width="128px">
                                            <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                            <DateInput ID="DateInput6" runat="server" onclick="ShowCalendar('3', true)" onkeypress="return SearchEnterOnTextbox(event);" />
                                        </as:RadDatePicker>
                                    </div>
                                </div>
                            </td>
                            <td></td>
                        </tr>
                        <as:PlaceHolder ID="uxPlSecondFilter" runat="server">
                            <tr>
                                <td class="text-left pr-4">
                                    <as:RadComboBox ID="uxOperator" Width="65px" runat="server"></as:RadComboBox>
                                </td>
                                <td class="text-left">
                                    <as:RadComboBox ID="uxDateTypeSecondFilter" runat="server"></as:RadComboBox>
                                </td>
                                <td class="text-left">
                                    <as:RadComboBox ID="uxDateSecondFilter" OnClientSelectedIndexChanged="ChangeDateOption" data-date-id="divDateSecond" data-date-range-id="divDateRangeSecond" runat="server"></as:RadComboBox>
                                </td>
                                <td class="text-left">
                                    <div class="filter-item" id="divDateSecond">
                                        <as:RadDatePicker ID="uxDateSecond" runat="server" Width="128px">
                                            <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                            <DateInput ID="DateInput1" runat="server" onclick="ShowCalendar('1', false)" onkeypress="return SearchEnterOnTextbox(event);" />
                                        </as:RadDatePicker>
                                    </div>
                                    <div id="divDateRangeSecond" style="display: none;">
                                        <div class="filter-item">
                                            <as:RadDatePicker ID="uxFromDateSecond" runat="server" Width="128px">
                                                <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                                <DateInput ID="DateInput2" runat="server" onclick="ShowCalendar('2', false)" onkeypress="return SearchEnterOnTextbox(event);" />
                                            </as:RadDatePicker>
                                        </div>
                                        <div class="filter-item">
                                            <as:RadDatePicker ID="uxToDateSecond" runat="server" Width="128px">
                                                <Calendar FastNavigationStep="12" ShowRowHeaders="false" />
                                                <DateInput ID="DateInput3" runat="server" onclick="ShowCalendar('3', false)" onkeypress="return SearchEnterOnTextbox(event);" />
                                            </as:RadDatePicker>
                                        </div>
                                    </div>
                                </td>
                                <td class="text-left pl-3x">
                                    <as:LinkButton ID="uxDeleteFilter" runat="server" OnClientClick="ShowPopupModal('ActivationReporDeleteFilterModal.aspx', 'auto'); return false;" Text="Delete" meta:resourcekey="uxDeleteFilterResources"></as:LinkButton>
                                </td>
                            </tr>
                        </as:PlaceHolder>
                        <tr>
                            <td></td>
                            <td class="text-left">
                                <as:LinkButton ID="uxAddFilter" OnClick="uxAddFilter_Click" OnClientClick="OnAddFilter()" runat="server" CssClass="display-inline mb-10" Text="+ Add Filter" meta:resourcekey="uxAddFilterResources"></as:LinkButton>
                            </td>
                        </tr>
                    </table>
                </as:Panel>
                <table id="tblHieararchyFilter">
                    <as:PlaceHolder ID="uxPlHierarySection" runat="server">
                        <uc:ActivationFilter ID="uxActivationHirarchyFilter" runat="server" OnDoSearch="uxActivationHirarchyFilter_DoSearch" />
                    </as:PlaceHolder>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <div class="text-right">
                                <as:Button ID="uxSearch" runat="server" Text="Search" OnClientClick="return ValidateData();"
                                    OnClick="uxSearch_Click" CssClass="btn btn-default mt-2x mb-2x mr-1x" IsStandardButton="False" meta:resourcekey="uxSearchResource1" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter">
                <as:Literal ID="Literal1" runat="server" Text="FILTER" meta:resourcekey="uxLiteralFilterResource1"></as:Literal></span>
        </div>
    </div>

    <as:PlaceHolder ID="uxPlActivationReport" runat="server">
        <uc:UxExport ID="uxExporter" runat="server" ShowPDF="false" IsOnTop="true" GridID="uxReportGrid" GridTitle="Activation Report" GridSubTitle="Open Date of " meta:resourcekey="uxGridTitleResource" />

        <as:Panel ID="uxPnBatchAmount" runat="server">
            <div class="row">
                <div class="col-md-4">
                    <as:Literal ID="uxBatchAmountText" runat="server" Text="Minimum Batch Amount: " meta:resourcekey="uxBatchAmountTextResources"></as:Literal>
                    <asp:Label ID="uxBatchAmount" CssClass="inline-block ml-1x" runat="server" Text="" meta:resourcekey="uxAmountResources"></asp:Label>
                    <as:LinkButton ID="uxEditAmount" runat="server" CssClass="ml-3x" Text="Edit Amount" OnClientClick="ShowPopupModal('ActivationReportEditAmountModal.aspx', 'auto'); return false;" meta:resourcekey="uxEditAmountResources"></as:LinkButton>
                </div>
                <div class="col-md-8 text-right text-italic text-default-gray">
                    <as:Literal ID="uxBatchAmountInfo" runat="server" Visible="false" Text="The updated minimum batch amount will be applied when a new transaction file is sent." meta:resourcekey="BatchAmountInfoResources"></as:Literal>
                </div>
            </div>
        </as:Panel>
        <div class="height-20"></div>
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowPaging="true" IsCacheTemplateFile="false" IsAutoExportTemplate="true" OnItemDataBound="uxReportGrid_ItemDataBound"
            OnNeedDataSource="uxReportGrid_NeedDataSource" AllowSorting="true" AllowExportAtWebServices="true" AllowSortFilterWhenExport="true"
            ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn UniqueName="MerchantNumber" HeaderText="Merchant ID" DataField="MerchantNumber" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        AllowEncodeOnExporting="true" ASFormat="DynamicString" HeaderTooltip="Merchant ID" ItemStyle-HorizontalAlign="Center"
                        SortExpression="MerchantNumber" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="MerchantName" HeaderText="Merchant Name"
                        DataField="MerchantName" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" ItemStyle-Width="220px"
                        HeaderTooltip="Merchant Name" SortExpression="MerchantName" ASFormat="StaticString"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ActivityStatus" HeaderText="Activity Status" DataField="ActivityStatus" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                        HeaderTooltip="Activity Status" SortExpression="ActivityStatus" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center"
                        AllowEncodeOnExporting="true" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>

    <div class="hide">
        <as:Button ID="uxBtnDeleteFilter" runat="server" OnClick="uxBtnDeleteFilter_Click" />
        <as:Button ID="uxBtnUpdateBatchAmount" runat="server" OnClick="uxBtnUpdateBatchAmount_Click" />
    </div>
    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var uxFromDateFirst_ClientID = "<%=uxFromDateFirst.ClientID%>";
            var uxToDateFirst_ClientID = "<%=uxToDateFirst.ClientID%>";
            var uxDateFirst_ClientID = "<%= uxDateFirst.ClientID %>";
            var uxFromDateSecond_ClientID = "<%=uxFromDateSecond.ClientID%>";
            var uxToDateSecond_ClientID = "<%=uxToDateSecond.ClientID%>";
            var uxDateSecond_ClientID = "<%= uxDateSecond.ClientID %>";
            var uxBtnDeleteFilter_ClientID = "<%= uxBtnDeleteFilter.ClientID %>";
            var uxBtnUpdateBatchAmount_ClientID = "<%= uxBtnUpdateBatchAmount.ClientID%>";
            var uxDateFirstFilter_ClientID = "<%= uxDateFirstFilter.ClientID %>";
            var uxDateSecondFilter_ClientID = "<%= uxDateSecondFilter.ClientID %>";
            var uxAddFilter_ClientID = "<%= uxAddFilter.ClientID%>";
            var uxActivationHirarchyFilter_ClientID = "<%= uxActivationHirarchyFilter.ClientID%>";

            var Msg_V1 = "<%=Resources.MessageManager.ReportFilter_V1%>";
            var Msg_V9 = "<%=Resources.MessageManager.ReportFilter_V9%>";
            var Msg_V10 = "<%=Resources.MessageManager.ValidationMessages_V10%>";
            var uxSearch_ClientID = "<%=uxSearch.ClientID%>";
            var Msg_V2 = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>";
            var Msg_V3 = "<%=Resources.MessageManager.ReportFilter_V3%>";
            var uxDailyResource1_text = '<%= GetLocalResourceObject("uxDailyResource1.Text").ToString()%>';
            var uxMonthlyResource1_text = '<%= GetLocalResourceObject("uxMonthlyResource1.Text").ToString()%>';
            var uxRangeResource1_text = '<%= GetLocalResourceObject("uxRangeResource1.Text").ToString()%>';

            var DateRangeText = "<%= GetLocalResourceObject("DateRangeResource").ToString()%>";
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/ActivationReport.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
