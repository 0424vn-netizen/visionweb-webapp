<%@ Page Title="Manage Assignments" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_Assignment_Management.aspx.cs" Inherits="rm_MCF_Assignment_Management"
    EnableEventValidation="false" meta:resourcekey="PageResource2" %>

<%@ Register TagName="PageTile" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="Export" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register TagName="ProgressBar" Src="~/UserControls/ProgressBar.ascx" TagPrefix="uc" %>
<%@ Register TagName="SiteID_Selector" Src="~/UserControls/SiteID_Selector.ascx"
    TagPrefix="uc" %>
<%@ Register TagName="AdvancedFilter" Src="~/UserControls/UxAdvancedFilter.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="main-content">
        <as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
            <ajaxsettings>
                <tek:AjaxSetting AjaxControlID="btnRefresh">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxAssignmentGrid" />
                        <tek:AjaxUpdatedControl ControlID="uxUserGroupGrid" />
                        <tek:AjaxUpdatedControl ControlID="uxSummaryTable" />
                        <tek:AjaxUpdatedControl ControlID="uxAssignmentActiveTypeRadioButtons" LoadingPanelID="uxHiddenLoadingPanel" />
                        <tek:AjaxUpdatedControl ControlID="uxExporter" LoadingPanelID="uxHiddenLoadingPanel" />
                        <tek:AjaxUpdatedControl ControlID="divPagerTop" LoadingPanelID="uxHiddenLoadingPanel" />
                        <tek:AjaxUpdatedControl ControlID="exportGroup" LoadingPanelID="uxHiddenLoadingPanel" />
                    </updatedcontrols>
                </tek:AjaxSetting>
            </ajaxsettings>
            <ajaxsettings>
                <tek:AjaxSetting AjaxControlID="btnRebind">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxAssignmentGrid" />
                        <tek:AjaxUpdatedControl ControlID="uxUserGroupGrid" />
                        <tek:AjaxUpdatedControl ControlID="uxSummaryTable" />
                        <tek:AjaxUpdatedControl ControlID="uxAssignmentActiveTypeRadioButtons" LoadingPanelID="uxHiddenLoadingPanel" />
                        <tek:AjaxUpdatedControl ControlID="uxExporter" LoadingPanelID="uxHiddenLoadingPanel" />
                        <tek:AjaxUpdatedControl ControlID="divPagerTop" LoadingPanelID="uxHiddenLoadingPanel" />
                        <tek:AjaxUpdatedControl ControlID="exportGroup" LoadingPanelID="uxHiddenLoadingPanel" />
                        <tek:AjaxUpdatedControl ControlID="exportGroup" LoadingPanelID="uxHiddenLoadingPanel" />
                    </updatedcontrols>
                </tek:AjaxSetting>
            </ajaxsettings>
            <ajaxsettings>
                <tek:AjaxSetting AjaxControlID="uxAssignmentGrid">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxAssignmentGrid" />
                    </updatedcontrols>
                </tek:AjaxSetting>
            </ajaxsettings>
            <ajaxsettings>
                <tek:AjaxSetting AjaxControlID="lnkAssignmentName">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="hddOpenEditAssignmentID" />
                    </updatedcontrols>
                </tek:AjaxSetting>
            </ajaxsettings>
            <ajaxsettings>
                <tek:AjaxSetting AjaxControlID="uxUserGroupGrid">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxUserGroupGrid" />
                        <tek:AjaxUpdatedControl ControlID="divPagerTop" />
                    </updatedcontrols>
                </tek:AjaxSetting>
            </ajaxsettings>
            <ajaxsettings>
                <tek:AjaxSetting AjaxControlID="btnDeleteAssignment">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxUserGroupGrid" />
                        <tek:AjaxUpdatedControl ControlID="uxAssignmentGrid" />
                        <tek:AjaxUpdatedControl ControlID="uxComboAssignment" />
                        <tek:AjaxUpdatedControl ControlID="divPagerTop" />
                        <tek:AjaxUpdatedControl ControlID="exportGroup" LoadingPanelID="uxHiddenLoadingPanel" />
                    </updatedcontrols>
                </tek:AjaxSetting>

                <tek:AjaxSetting AjaxControlID="btnExtendAssignment">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxUserGroupGrid" />
                        <tek:AjaxUpdatedControl ControlID="uxAssignmentGrid" />
                    </updatedcontrols>
                </tek:AjaxSetting>

                <tek:AjaxSetting AjaxControlID="uxLnkCreateAssignemnt">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxLnkCreateAssignemnt" />
                        <tek:AjaxUpdatedControl ControlID="uxUserGroupGrid" />
                    </updatedcontrols>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="uxFilterType">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxFilterValuePanelAssignment" LoadingPanelID="uxHiddenLoadingPanel" />
                    </updatedcontrols>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="uxReportDate">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxFilterValuePanelAssignment" LoadingPanelID="uxHiddenLoadingPanel" />
                    </updatedcontrols>
                </tek:AjaxSetting>

                <tek:AjaxSetting AjaxControlID="uxOpenEditAssignment">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxOpenEditAssignment" LoadingPanelID="uxHiddenLoadingPanel" />
                    </updatedcontrols>
                </tek:AjaxSetting>


                <tek:AjaxSetting AjaxControlID="uxOpenDuplicateAssignment">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxOpenDuplicateAssignment" LoadingPanelID="uxHiddenLoadingPanel" />
                    </updatedcontrols>
                </tek:AjaxSetting>


                <tek:AjaxSetting AjaxControlID="uxBtnAssignmentFilter">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="uxAssignmentGrid" />
                        <tek:AjaxUpdatedControl ControlID="uxUserGroupGrid" />
                        <tek:AjaxUpdatedControl ControlID="uxSummaryTable" />
                        <tek:AjaxUpdatedControl ControlID="uxAssignmentActiveTypeRadioButtons" LoadingPanelID="uxHiddenLoadingPanel" />
                        <tek:AjaxUpdatedControl ControlID="uxExporter" LoadingPanelID="uxHiddenLoadingPanel" />
                        <tek:AjaxUpdatedControl ControlID="divPagerTop" LoadingPanelID="uxHiddenLoadingPanel" />
                        <tek:AjaxUpdatedControl ControlID="exportGroup" LoadingPanelID="uxHiddenLoadingPanel" />
                    </updatedcontrols>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="btnOpenConfirmModal">
                    <updatedcontrols>
                        <tek:AjaxUpdatedControl ControlID="btnOpenConfirmModal" LoadingPanelID="uxHiddenLoadingPanel" />
                        <tek:AjaxUpdatedControl ControlID="hddAssignmentDetete" LoadingPanelID="uxHiddenLoadingPanel" />
                    </updatedcontrols>
                </tek:AjaxSetting>
            </ajaxsettings>
        </as:RadAjaxManagerProxy>

        <as:RadAjaxLoadingPanel runat="server" ID="uxHiddenLoadingPanel" Visible="false" />

        <uc:AdvancedFilter ID="uxAdvancedFilter" runat="server" />

        <div runat="server" id="uxFilterOption">
            <div class="row collapse report-filter-panel">
                <div class="col-md-12 report-filter">
                    <div class="filter-block">
                        <table>
                            <tr>
                                <td class="text-right">
                                    <label class="filter-label">
                                        <asp:Literal ID="Literal10" runat="server" meta:resourcekey="rm_Assignment_Management_aspx_DateResource1" Text="Date:"></asp:Literal></label>
                                </td>
                                <td class="text-left">
                                    <div class="filter-item">
                                        <as:RadDatePicker ID="uxReportDate" runat="server" ShowPopupOnFocus="true" Width="128px" ClientEvents-OnDateSelected="uxDateFilter_SelectedChange" OnSelectedDateChanged="uxReportDate_SelectedDateChanged" AutoPostBack="true" meta:resourcekey="uxReportDateResource1">
                                            <calendar fastnavigationstep="12" showrowheaders="false">
                                            </calendar>
                                            <dateinput id="DateInput2" dateformat="MM/dd/yyyy" runat="Server"
                                                labelwidth="64px" width="">
                                                <emptymessagestyle resize="None"></emptymessagestyle>

                                                <readonlystyle resize="None"></readonlystyle>

                                                <focusedstyle resize="None"></focusedstyle>

                                                <disabledstyle resize="None"></disabledstyle>

                                                <invalidstyle resize="None"></invalidstyle>

                                                <hoveredstyle resize="None"></hoveredstyle>

                                                <enabledstyle resize="None"></enabledstyle>
                                            </dateinput>

                                            <datepopupbutton imageurl="" hoverimageurl="" cssclass=""></datepopupbutton>
                                        </as:RadDatePicker>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right"></td>
                                <td class="text-left">
                                    <div class="row">
                                        <div class="col-md-5 mr-5x">
                                            <div class="filter-item">
                                                <as:ASRadComboBox ID="uxFilterType" runat="server" Width="250" AutoPostBack="true" OnSelectedIndexChanged="uxFilterType_SelectedIndexChanged"
                                                    OnClientSelectedIndexChanged="uxFilterType_SelectedIndexChanged" Filter="Contains" MarkFirstMatch="true" meta:resourcekey="uxFilterTypeResource1">
                                                </as:ASRadComboBox>
                                            </div>
                                        </div>

                                        <div class="col-md-5">
                                            <div id="uxFilterValuePanelUser" class="filter-item display-none" runat="server">
                                                <as:ASRadComboBox ID="uxComboUser" Filter="Contains" MarkFirstMatch="true" runat="server" MaxHeight="250px" Width="250" meta:resourcekey="uxComboUserResource1" />
                                            </div>
                                            <div id="uxFilterValuePanelGroup" class="filter-item display-none" runat="server">
                                                <as:ASRadComboBox ID="uxComboGroup" OnClientSelectedIndexChanged="uxGroupBy_SelectedIndexChanged"
                                                    runat="server" MaxHeight="250px" Width="250" Filter="Contains" MarkFirstMatch="true" meta:resourcekey="uxComboGroupResource1" />
                                            </div>
                                            <div id="uxFilterValuePanelAssignment" class="filter-item display-none" runat="server">
                                                <as:ASRadComboBox ID="uxComboAssignment" OnClientSelectedIndexChanged="uxAssignmentList_SelectedIndexChanged"
                                                    runat="server" MaxHeight="250px" Sort="Ascending" Width="250" Filter="Contains" MarkFirstMatch="true" meta:resourcekey="uxComboAssignmentResource1" />
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right">
                                    <label class="filter-label">
                                        <asp:Literal ID="rm_Assignment_Management_aspx_GroupBy" runat="server" meta:resourcekey="rm_Assignment_Management_aspx_GroupByResource1" Text=" Group By:"></asp:Literal></label>
                                </td>
                                <td class="text-left">
                                    <div class="filter-item">
                                        <as:ASRadComboBox ID="uxGroupBy" Filter="Contains" MarkFirstMatch="true" runat="server" Width="250" AutoPostBack="false" meta:resourcekey="uxGroupByResource1">
                                            <Items>
                                                <as:ASRadComboBoxItem Text="Assignment" Value="Assignment" Selected="true" meta:resourcekey="ASRadComboBoxItemResource5" />
                                                <as:ASRadComboBoxItem Text="Group" Value="Group" meta:resourcekey="ASRadComboBoxItemResource6" />
                                                <as:ASRadComboBoxItem Text="User" Value="User" meta:resourcekey="ASRadComboBoxItemResource7" />
                                            </Items>
                                        </as:ASRadComboBox>
                                    </div>
                                    <div class="filter-item">
                                        <asp:Button ID="uxBtnSubmit" runat="server" Text="Submit" CssClass="btn btn-default"
                                            CausesValidation="False" OnClientClick="return ValidateFilters();" meta:resourcekey="uxBtnSubmitResource2" />
                                        <asp:Button ID="uxRealSubmit" runat="server" Text="Submit" CssClass="display-none" UseSubmitBehavior="false"
                                            CausesValidation="False" OnClick="uxBtnSubmit_Click" meta:resourcekey="uxRealSubmitResource2" />
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
                        <asp:Literal ID="rm_Assignment_Management_aspx_Filter" runat="server" meta:resourcekey="rm_Assignment_Management_aspx_FilterResource1" Text=" FILTER"></asp:Literal>
                    </span>
                </div>
            </div>


        </div>

        <%-- ReportTitle --%>
        <div class="row">
            <div class="col-md-12 no-margin-bottom">
                <uc:PageTile ID="uxPageTitle" runat="server" ReportTitle="Manage Assignments" meta:resourcekey="rm_Assignment_Management_aspx_ReportTitleResource1" HasFilteringOption="true" />
                <div style="position: absolute; right: 15px; top: 3px">
                    <as:LinkButton runat="server" ID="btnRefresh" OnClick="btnRefresh_Click"
                        Text="Refresh" Style="padding-right: 15px" meta:resourcekey="btnRefreshResource">
                    </as:LinkButton>
                    <as:LinkButton runat="server" ID="btnAdvancedFilter" OnClientClick="assignment.assignmentFilter.show(); return false;"
                        meta:resourcekey="btnAdvancedFilterResource" Text="Advanced Filter">
                    </as:LinkButton>
                </div>
            </div>
        </div>
        <div class="height-14"></div>
        <%-- Hyperlink Create--%>
        <as:PlaceHolder ID="plhActionPanel" runat="server">
            <div class="row">
                <div class="col-md-12 no-margin-action-container">
                    <span class="control-inline last">
                        <as:LinkButton ID="uxLnkCreateAssignemnt" runat="server" Text="Create New Assignment"
                            CssClass="btn btn-default" OnClick="uxLnkCreateAssignemnt_Click" meta:resourcekey="uxLnkCreateAssignemntResource2" />
                    </span>
                    <as:LinkButton ID="uxLnkShowAssignmentProcessingStatus" runat="server" Text="Show Assignment Processing Status"
                        CssClass="btn btn-default" OnClientClick="ShowPopupModal('rm_MCF_AssignmentProcessingStatus.aspx','auto'); return false;" meta:resourcekey="uxLnkShowAssignmentProcessingStatusResource2" />
                </div>
            </div>
        </as:PlaceHolder>

        <div id="apply-filter" class="apply-filter hide" data-show-adv-filter-item="true">
            <div class="height-14"></div>
            <div class="display-flex">
                <label class="lbl-filter">
                    <asp:Literal ID="Literal11" runat="server" meta:resourcekey="lblFilter" Text="Filter:"></asp:Literal>
                </label>
                <div class="advanced-filter-item">
                    <span data-filter-item="true"></span>
                    <a class="filter-choice-close" onclick="assignment.assignmentFilter.clearApplyAdvFilter();"></a>
                </div>
            </div>
            <as:HiddenField ID="hddApplyFilterId" runat="server" />
        </div>

        <%-- Distinct Grid --%>
        <div class="height-18"></div>
        <div class="row">
            <div class="col-md-12 no-margin-bottom d-flex mb-20">
                <asp:Label ID="ucDistinctMerchant" CssClass="assignment-sub-title" Text="Distinct Merchant Summary" runat="server" meta:resourcekey="DistinctMerchantResource1"></asp:Label>
            </div>

            <div class="col-md-6">
                <table id="uxSummaryTable" class="ASTable" runat="server">
                    <tr>
                        <th class="no-border"></th>

                        <th>
                            <asp:Label ID="ucEligible" runat="server" Text="Eligible" meta:resourcekey="Literal1Resource1"></asp:Label>
                            <tek:RadToolTip ID="uxEligibleToolTip" runat="server" AutoCloseDelay="0" HideDelay="0" RenderMode="Lightweight" RelativeTo="Element" Position="BottomCenter" TargetControlID="ucEligible">
                            </tek:RadToolTip>
                        </th>
                        <th>
                            <asp:Label ID="ucAlerted" runat="server" Text="Alerted" meta:resourcekey="Literal2Resource1"></asp:Label>
                            <tek:RadToolTip ID="uxAlertedToolTip" runat="server" AutoCloseDelay="0" HideDelay="0" RenderMode="Lightweight" RelativeTo="Element" Position="BottomCenter" TargetControlID="ucAlerted">
                            </tek:RadToolTip>
                        </th>
                        <th>
                            <asp:Label ID="ucWorked" runat="server" Text="Worked" meta:resourcekey="Literal3Resource1"></asp:Label>
                            <tek:RadToolTip ID="uxWorkedToolTip" runat="server" AutoCloseDelay="0" HideDelay="0" RenderMode="Lightweight" RelativeTo="Element" Position="BottomCenter" TargetControlID="ucWorked">
                            </tek:RadToolTip>
                        </th>
                        <th>
                            <asp:Label ID="ucRemaining" runat="server" Text="Remaining" meta:resourcekey="Literal4Resource1"></asp:Label>
                            <tek:RadToolTip ID="uxRemainingToolTip" AutoCloseDelay="0" HideDelay="0" runat="server" RenderMode="Lightweight" RelativeTo="Element" Position="BottomCenter" TargetControlID="ucRemaining">
                            </tek:RadToolTip>
                        </th>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading">
                            <asp:Literal ID="Literal5" runat="server" meta:resourcekey="Literal5Resource1" Text="Distinct Merchant Count"></asp:Literal></td>
                        <td class="text-right">&nbsp;<asp:Literal ID="uxCountTotal" runat="server" meta:resourcekey="uxCountTotalResource1" /></td>
                        <td class="text-right">&nbsp;<asp:Literal ID="uxCountAlerted" runat="server" meta:resourcekey="uxCountAlertedResource1" /></td>
                        <td class="text-right">&nbsp;<asp:Literal ID="uxCountWorked" runat="server" meta:resourcekey="uxCountWorkedResource1" /></td>
                        <td class="text-right">&nbsp;<asp:Literal ID="uxCountRemaining" runat="server" meta:resourcekey="uxCountRemainingResource1" />
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading">
                            <asp:Literal ID="Literal6" runat="server" meta:resourcekey="Literal6Resource1" Text="Distinct Merchant Volume"></asp:Literal>
                        </td>
                        <td class="text-right">&nbsp;
                            <asp:Literal ID="uxVolumeTotal" runat="server" meta:resourcekey="uxVolumeTotalResource1" />
                        </td>
                        <td class="text-right">&nbsp;<asp:Literal ID="uxVolumeAlerted" runat="server" meta:resourcekey="uxVolumeAlertedResource1" />
                        </td>
                        <td class="text-right">&nbsp;<asp:Literal ID="uxVolumeWorked" runat="server" meta:resourcekey="uxVolumeWorkedResource1" /></td>
                        <td class="text-right">&nbsp;<asp:Literal ID="uxVolumeRemaining" runat="server" meta:resourcekey="uxVolumeRemainingResource1" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="height-20"></div>

        <div class="row">
            <div class="col-md-12 d-flex mb-10">
                <asp:Label ID="ucAssignmentSummary" CssClass="assignment-sub-title" Text="Assignment Summary" runat="server" meta:resourcekey="AssignmentSummaryResource1"></asp:Label>

                <div class="image-link" style="cursor: pointer; line-height: 30px; margin-left: 8px;" runat="server" id="AssignmentSummaryInfo">
                    <img src='<%=ResolveUrl("~")%>res/images/icon_gray_dot.png' />

                </div>

                <tek:RadToolTip ID="uxAssignmentSummaryInfo" runat="server" AutoCloseDelay="0" HideDelay="0" RenderMode="Lightweight" RelativeTo="Element" Position="BottomCenter" TargetControlID="AssignmentSummaryInfo">
                </tek:RadToolTip>
            </div>

            <div class="col-md-6">
                <%-- Filter Radio--%>
                <div class="row radio-button-list dark-blue" id="uxAssignmentActiveTypeRadioButtons"
                    runat="server">
                    <div class="col-md-12">
                        <div class="control-inline">
                            <label class="first">
                                <asp:Literal ID="Literal8" runat="server" meta:resourcekey="Literal8Resource1" Text="Assignments:"></asp:Literal></label>
                        </div>
                        <div class="control-inline">
                            <as:RadioButton ID="uxAll" runat="server" Text="All" GroupName="FilterDate"
                                xValue="0" onclick="RefreshContent();" meta:resourcekey="uxAllResource2" Value="" />
                        </div>
                        <div class="control-inline">
                            <as:RadioButton ID="uxActive" runat="server" Checked="True" Text="Active" GroupName="FilterDate"
                                xValue="1" onclick="RefreshContent();" meta:resourcekey="uxActiveResource2" Value="" />
                        </div>
                        <div class="control-inline">
                            <as:RadioButton ID="uxExpire" runat="server" Text="Expired" GroupName="FilterDate"
                                xValue="2" onclick="RefreshContent();" meta:resourcekey="uxExpireResource2" Value="" />
                        </div>
                        <asp:Button ID="uxBtnAssignmentFilter" runat="server" Text="Submit" CssClass="display-none"
                            CausesValidation="False" OnClick="uxBtnAssignmentFilter_Click" meta:resourcekey="uxRealSubmitResource2" />
                    </div>
                </div>
                <asp:Panel CssClass="height-18" runat="server" ID="uxNoDataPadding" meta:resourcekey="uxNoDataPaddingResource1">
                </asp:Panel>
            </div>
            <div class="col-md-6">
                <div class="height-12"></div>
                <%-- Grid Normal--%>
                <uc:Export ID="uxExporter" runat="server" GridID="uxAssignmentGrid" OnExportingReportHeader="uxAssignmentGrid_DoReportHeader"
                    ShowPDF="false" ShowWord="false" IsOnTop="true" />

                <div class="row">
                    <div class="col-xs-10">
                    </div>
                    <div class="col-xs-2">
                        <div class="report-export-no-title dropdown pull-right on-top" runat="server" id="exportGroup">
                            <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle"
                                id="litExport" runat="server">
                                <asp:Literal ID="Literal9" runat="server" meta:resourcekey="Literal9Resource1" Text="EXPORT"></asp:Literal></a>
                            <ul class="dropdown-menu">
                                <li id="uxLiExcel" runat="server">
                                    <as:LinkButton ID="imgExcel" runat="server" OnClick="ExportData" meta:resourcekey="imgExcelResource2">Excel</as:LinkButton>
                                </li>
                                <li id="uxLiCSV" runat="server">
                                    <as:LinkButton ID="imgCSV" runat="server" OnClick="ExportData" meta:resourcekey="imgCSVResource2">CSV</as:LinkButton>
                                </li>
                                <li id="uxLiWord" runat="server">
                                    <as:LinkButton Visible="false" ID="imgPDF" runat="server" OnClick="ExportData" meta:resourcekey="imgPDFResource2">PDF</as:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="height-18" id="uxGroupByGridPadding" visible="false" runat="server"></div>
            </div>
        </div>
        <div id="divPagerTop" runat="server" class="mbi-5">
            <as:Literal ID="litPager" runat="server"></as:Literal>
        </div>
        <%--        <div class="assignment-helptext">
            <asp:Literal ID="ucHelpText" runat="server" meta:resourcekey="AssignmentHelpText"></asp:Literal>
        </div>--%>
        <as:ASGrid ID="uxAssignmentGrid" runat="server" AutoGenerateColumns="false" HeaderStyle-Width="80px" IsCacheTemplateFile="false"
            ASPagingMethod="None" AllowSorting="true" AllowPaging="true" GridLines="None" IsAutoExportTemplate="true" ClientSettings-ClientEvents-OnDataBound="SetHeaderAssGrid()"
            OnSortCommand="uxAssignmentGrid_OnSortCommand" ShowToolTip="true" CssClass="in" meta:resourcekey="uxAssignmentGridResource2">
            <mastertableview>
                <columns>
                    <as:ASGridBoundColumn DataField="AssignmentID" UniqueName="AssignmentID" Visible="false" meta:resourcekey="ASGridBoundColumnResource17">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridTemplateColumn HeaderText="Duplicate" DataField="AssignmentID" UniqueName="Duplicate"
                        HeaderTooltip="Click to delete assignment" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-CssClass="action-column" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource9">
                        <itemtemplate>
                            <as:Literal ID="lbDuplicate" runat="server" meta:resourcekey="lbDuplicateResource1" />
                        </itemtemplate>

                        <headerstyle horizontalalign="Center" cssclass="action-column"></headerstyle>

                        <itemstyle horizontalalign="Center"></itemstyle>
                    </as:ASGridTemplateColumn>

                    <as:ASGridTemplateColumn HeaderText="Assignment Name" DataField="AssignmentName" HeaderStyle-Width="230px"
                        UniqueName="AssignmentName" SortExpression="AssignmentName" HeaderTooltip="The assignment name hyperlink navigates to the Manage Assignment modal."
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" meta:resourcekey="ASGridTemplateColumnResource5">
                        <itemtemplate>
                            <as:LinkButton ID="lnkAssignmentName" runat="server" OnClientClick='<%# "return openEditAssignment(\""+Eval("AssignmentID")+"\",\""+Eval("AssignmentType")+"\")"%>'
                                Value='<%# Eval("AssignmentID") %>' Text='<%# Eval("AssignmentName") %>' meta:resourcekey="lnkAssignmentNameResource1">
                            </as:LinkButton>
                            <as:PlaceHolder ID="uxIconFutureStartDate" runat="server" Visible="false">
                                <img src='<%# ResolveUrl("~/") %>res/img/icon-future-start-date.png' class="rcCalPopup"
                                    alt='<%# GetLocalResourceObject("rm_Assignment_Management_aspx_img_icon_future_startDate").ToString() %>'
                                    title='<%# GetLocalResourceObject("rm_Assignment_Management_aspx_img_icon_future_startDate").ToString() %>'
                                    style="cursor: pointer;" />
                            </as:PlaceHolder>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Left"></itemstyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn DataField="FutureStartDate" UniqueName="FutureStartDate" HeaderStyle-Width="120px"
                        ItemStyle-HorizontalAlign="Center" HeaderText="Future Start Date" SortExpression="FutureStartDate"
                        HeaderTooltip="Future Start Date" HeaderStyle-HorizontalAlign="Center" Visible="false" meta:resourcekey="FutureStartDateASGridBoundColumnResource">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Type" DataField="AssignmentType" UniqueName="AssignmentType"
                        HeaderTooltip="Assignment Type" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Count" DataField="TotalMerchantCount" UniqueName="TotalMerchantCount" ASFormat="Integer" HeaderStyle-Width="100px"
                        HeaderTooltip="Number of distinct merchants that qualify for an assignment based on the assignment filters selected (a merchant can re-alert for an assignment, but is counted as eligible once)"
                        ItemStyle-HorizontalAlign="Right" SortExpression="TotalMerchantCount" ASDefaultNullValue="—" meta:resourcekey="ASGridBoundColumnResource19">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Count" DataField="AlertMerchantCount" ASFormat="Integer"
                        UniqueName="AlertMerchantCount" HeaderTooltip="Number of distinct merchants that have alerted or re-alerted based on the assignment parameter configurations"
                        SortExpression="AlertMerchantCount" meta:resourcekey="ASGridBoundColumnResource20" ASDefaultNullValue="—">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net Amount" DataField="AlertMerchantVolume" UniqueName="AlertMerchantVolume" HeaderStyle-Width="120px"
                        ItemStyle-Wrap="true" ASFormat="Currency" HeaderTooltip="Cumulative amount associated with the Alerted Count" ASDefaultNullValue="—"
                        SortExpression="AlertMerchantVolume" meta:resourcekey="ASGridBoundColumnResource21" ItemStyle-Width="120px">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>

                        <itemstyle wrap="True"></itemstyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Count" DataField="WKCount" ASFormat="Integer"
                        UniqueName="WKCount" HeaderTooltip="Number of distinct merchants that have alerted or re-alerted, but have not been worked"
                        SortExpression="WKCount" meta:resourcekey="ASGridBoundColumnResource54" ASDefaultNullValue="—">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net Amount" DataField="WKVolume" UniqueName="WKVolume" HeaderStyle-Width="120px"
                        ItemStyle-Wrap="true" ASFormat="Currency" HeaderTooltip="Cumulative amount associated with  the Ready to Work - Count"
                        SortExpression="WKVolume" meta:resourcekey="ASGridBoundColumnResource55" ItemStyle-Width="120px" ASDefaultNullValue="—">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>

                        <itemstyle wrap="True"></itemstyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Count" DataField="WIPCount" UniqueName="WIPCount"
                        ASFormat="Integer" SortExpression="WIPCount" HeaderTooltip="Number of distinct merchants marked as Work in Progress"
                        meta:resourcekey="ASGridBoundColumnResource59" ASDefaultNullValue="—">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net Amount" DataField="WIPVolume" UniqueName="WIPVolume"
                        ASFormat="Currency" SortExpression="WIPVolume" HeaderTooltip="Cumulative amount associated with the Work in Progress - Count"
                        meta:resourcekey="ASGridBoundColumnResource58" HeaderStyle-Width="120px" ASDefaultNullValue="—">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Count" DataField="WorkedCount" UniqueName="WorkedCount"
                        ASFormat="Integer" SortExpression="WorkedCount" HeaderTooltip="Number of distinct merchants worked"
                        meta:resourcekey="ASGridBoundColumnResource22" ASDefaultNullValue="—">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net Amount" DataField="WorkedVolume" UniqueName="WorkedVolume"
                        ASFormat="Currency" SortExpression="WorkedVolume" HeaderTooltip="Cumulative amount associated with the Worked - Count"
                        meta:resourcekey="ASGridBoundColumnResource23" HeaderStyle-Width="120px" ASDefaultNullValue="—">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn HeaderText="Percent Worked" DataField="CompletePercent" SortExpression="CompletePercent"
                        ASExportFormat="Percentage" UniqueName="CompleteTemplate" HeaderTooltip="Percent of merchants worked in relation to the number of merchants alerted = Worked Count  / Alerted Count " HeaderStyle-Width="120px"
                        ItemStyle-CssClass="percent-column" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        meta:resourcekey="ASGridTemplateColumnResource6" ASDefaultNullValue="—">
                        <itemtemplate>
                            <uc:ProgressBar ID="progressBar" runat="server" />
                        </itemtemplate>

                        <headerstyle horizontalalign="Center"></headerstyle>

                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn HeaderText="Count" DataField="RequeueCount" UniqueName="RequeueCount" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="no-border"
                        ASFormat="Integer" HeaderTooltip="Number of distinct merchants that have been added to a Work Queue Assignment from a Detection Queue Assignment using the Re-Queue or Auto Queue feature" ASDefaultNullValue="—" meta:resourcekey="ASGridBoundColumnResource24">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net Amount" DataField="RequeueVolume" UniqueName="RequeueVolume" ItemStyle-HorizontalAlign="Right"
                        ASFormat="Currency" HeaderTooltip="Cumulative amount associated with the Re-queued Count" ASDefaultNullValue="—" meta:resourcekey="ASGridBoundColumnResource25" HeaderStyle-Width="120px">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn DataField="StartDate" UniqueName="StartDateTemplate"
                        HeaderText="Start Date" SortExpression="StartDate" HeaderStyle-Width="120px"
                        HeaderTooltip="The first date the assignment will be run" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="StartDateASGridTemplateColumnResource">
                        <itemtemplate>
                            <div id='<%# Eval("AssignmentID") %>' onmouseover="GetAssignementID(this)">
                                <as:PlaceHolder ID="uxStartDateCalendar" runat="server">
                                    <asp:TextBox ID="uxStartDate" ReadOnly="True" onkeypress="return false;" Width="85px"
                                        onclick="showPopup(this, event);" onfocus="showPopup(this, event);" onblur="parseDate(this, event)"
                                        runat="server" meta:resourcekey="uxStartDateResource1" />
                                    <img src='<%# ResolveUrl("~/") %>res/img/calendar_ico.png' class="rcCalPopup"
                                        alt='<%# GetLocalResourceObject("rm_Assignment_Management_aspx_img_alt1_startDate").ToString() %>'
                                        onclick="showPopup(this, event, '<%# AS.Web.Business.Shared.Constants.ManageAssignmentConstanst.EVENT_FIELD_START_DATE %>', '<%# Eval("ExpirationDate") %>')" style="cursor: pointer;" />
                                </as:PlaceHolder>
                            </div>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridTemplateColumn DataField="ExpirationDate" UniqueName="ExpirationDateTemplate" ItemStyle-Wrap="false"
                        ItemStyle-HorizontalAlign="Center" HeaderText="Expiration Date" SortExpression="ExpirationDate" HeaderStyle-Width="120px"
                        HeaderTooltip="The last date that the assignment is scheduled to execute" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource7">
                        <itemtemplate>

                            <div id='<%# Eval("AssignmentID") %>' onmouseover="GetAssignementID(this)">
                                <as:PlaceHolder ID="uxCalendar" runat="server">
                                    <asp:TextBox ID="uxExpirationDate" ReadOnly="True" onkeypress="return false;" Width="85px"
                                        onclick="showPopup(this, event);" onfocus="showPopup(this, event);" onblur="parseDate(this, event)"
                                        runat="server" meta:resourcekey="uxExpirationDateResource1" />
                                    <img src='<%# ResolveUrl("~/") %>res/img/calendar_ico.png' class="rcCalPopup" alt='<%# GetLocalResourceObject("rm_Assignment_Management_aspx_img_alt1").ToString() %>'
                                        onclick="showPopup(this, event, '<%# AS.Web.Business.Shared.Constants.ManageAssignmentConstanst.EVENT_FIELD_EXPIRATION_DATE %>', '<%# Eval("StartDate") %>')" style="cursor: pointer;">
                                </as:PlaceHolder>
                                <as:Literal ID="uxExpirationDateNever" Text="Never Expire" Visible="False" runat="server" meta:resourcekey="uxExpirationDateNeverResource2" />
                            </div>
                        </itemtemplate>

                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center" wrap="False"></itemstyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn DataField="StartDate" UniqueName="StartDate" HeaderStyle-Width="120px"
                        ItemStyle-HorizontalAlign="Center" HeaderText="Start Date" SortExpression="StartDate"
                        HeaderTooltip="Start Date" HeaderStyle-HorizontalAlign="Center" Visible="false" meta:resourcekey="StartDateASGridBoundColumnResource">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="StartDateExport" UniqueName="StartDateExport" HeaderStyle-Width="120px"
                        ItemStyle-HorizontalAlign="Center" HeaderText="Start Date" SortExpression="StartDateExport"
                        HeaderTooltip="Start Date" HeaderStyle-HorizontalAlign="Center" Visible="false" meta:resourcekey="StartDateASGridBoundColumnResource">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ExpirationDate" UniqueName="ExpirationDate" Visible="false" meta:resourcekey="ASGridBoundColumnResource29">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>
                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ExpirationDateExport" UniqueName="ExpirationDateExport" HeaderStyle-Width="120px"
                        ItemStyle-HorizontalAlign="Center" HeaderText="Expiration Date" SortExpression="ExpirationDateExport"
                        HeaderTooltip="Expiration Date" HeaderStyle-HorizontalAlign="Center" Visible="false" meta:resourcekey="ASGridBoundColumnResource28">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>

                        <itemstyle horizontalalign="Center"></itemstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="ExpirationDate" UniqueName="ExpirationDate" Visible="false" meta:resourcekey="ASGridBoundColumnResource29">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Proc Status" DataField="ProcessingStatus" UniqueName="ProcessingStatus" HeaderStyle-Width="110px"
                        HeaderStyle-HorizontalAlign="Center" HeaderTooltip="The processing status of the assignment" SortExpression="ProcessingStatus"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource30">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Last Proc Status Date" DataField="ProcessingStatusDate" HeaderStyle-Width="110px"
                        UniqueName="ProcessingStatusDate" HeaderTooltip="The date and time of the processing status"
                        SortExpression="ProcessingStatusDate" ASFormat="DateAndTime12Hours"
                        meta:resourcekey="ASGridBoundColumnResource31" ASDefaultNullValue="—">

                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridTemplateColumn HeaderText="Delete" DataField="AssignmentID" UniqueName="Delete"
                        HeaderTooltip="Click to delete assignment" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-CssClass="action-column" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource8">
                        <itemtemplate>
                            <as:Literal ID="lbDelete" runat="server" meta:resourcekey="lbDeleteResource1" />
                        </itemtemplate>

                        <headerstyle horizontalalign="Center" cssclass="action-column"></headerstyle>

                        <itemstyle horizontalalign="Center"></itemstyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn DataField="IsReAssign" UniqueName="IsReAssign" Visible="false" meta:resourcekey="ASGridBoundColumnResource32">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>
                    </as:ASGridBoundColumn>
                </columns>
            </mastertableview>
        </as:ASGrid>
        <as:HiddenField ID="uxListOfGridId" runat="server" />

        <%-- Grid User Group--%>


        <%--  <div id="exportGroup" runat="server" style="padding-right: 1px; padding-top: 0px;
        float: right;">
        <span>Export:</span>
        <as:Button ID="imgExcel" runat="server" ToolTip="Export to excel" CssClass="exportExcel"
            IsStandardButton="true" OnClick="ExportData" />
        <as:Button ID="imgCSV" runat="server" ToolTip="Export to CSV" CssClass="exportCsv"
            IsStandardButton="true" OnClick="ExportData" />
        <as:Button ID="imgPDF" runat="server" ToolTip="Export to PDF" CssClass="exportPdf"
            IsStandardButton="true" OnClick="ExportData" />
    </div>
    <div style="clear: both;">
    </div>--%>
        <as:ASGrid ID="uxUserGroupGrid" ShowGroupPanel="false" ShowStatusBar="false" runat="server"
            CssClass="wrapper-table in mt-3x" XOverFlowable="false"
            AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" GridLines="None"
            Visible="false" OnDetailTableDataBind="uxUserGroupGrid_DetailTableDataBind" ClientSettings-ClientEvents-OnDataBound="SetHeaderAssGrid()"
            ShowHeader="false" OnSortCommand="uxUserGroupGrid_SortCommand" OnItemCreated="uxUserGroupGrid_ItemCreated"
            OnItemEvent="uxUserGroupGrid_ItemEvent" OnPreRender="uxUserGroupGrid_PreRender" meta:resourcekey="uxUserGroupGridResource1">
            <mastertableview name="Master" datakeynames="UserID" expandcollapsecolumn-itemstyle-backcolor="White"
                allowmulticolumnsorting="false" expandcollapsecolumn-display="true" itemstyle-verticalalign="Middle"
                itemstyle-horizontalalign="Center" hierarchydefaultexpanded="true" tablelayout="Auto"
                itemstyle-cssclass="rgAltRow">
                <expandcollapsecolumn visible="false" display="false" headerstyle-width="1px" itemstyle-width="1px">
                    <itemstyle backcolor="White"></itemstyle>
                </expandcollapsecolumn>
                <detailtables>
                    <tek:GridTableView Name="Detail" runat="server" ShowHeader="true" PagerStyle-AlwaysVisible="true"
                        AllowSorting="true" AllowPaging="false" CssClass="detail-table-of-hierarchy-grid" meta:resourcekey="GridTableViewResource1">
                        <parenttablerelation>
                            <tek:GridRelationFields DetailKeyField="UserID" MasterKeyField="UserID" />
                        </parenttablerelation>
                        <columns>
                            <as:ASGridBoundColumn DataField="AssignmentID" UniqueName="AssignmentID" Display="false" meta:resourcekey="ASGridBoundColumnResource33">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="UserID" DataField="UserID" Visible="false" meta:resourcekey="ASGridBoundColumnResource34">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="GroupID" DataField="GroupID" Visible="false" meta:resourcekey="ASGridBoundColumnResource35">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridTemplateColumn HeaderText="Duplicate" DataField="AssignmentID" UniqueName="Duplicate"
                                HeaderTooltip="Click to delete assignment" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-CssClass="action-column" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource9">
                                <itemtemplate>
                                    <as:Literal ID="lbDuplicate" runat="server" meta:resourcekey="lbDuplicateResource1" />
                                </itemtemplate>

                                <headerstyle horizontalalign="Center" cssclass="action-column"></headerstyle>

                                <itemstyle horizontalalign="Center"></itemstyle>
                            </as:ASGridTemplateColumn>

                            <tek:GridTemplateColumn HeaderText="Assignment Name" DataField="AssignmentName" UniqueName="AssignmentName"
                                HeaderStyle-HorizontalAlign="Center" HeaderTooltip="The assignment name hyperlink navigates to the Manage Assignment modal."
                                SortExpression="AssignmentName" ItemStyle-HorizontalAlign="Left" meta:resourcekey="GridTemplateColumnResource9">
                                <itemtemplate>
                                    <as:LinkButton ID="lnkAssignmentName" runat="server" CommandName="AssignmentID" OnCommand="lnkEditUserGroup_Command"
                                        CommandArgument='<%# Eval("AssignmentID") %>' Text='<%# Eval("AssignmentName") %>' meta:resourcekey="lnkAssignmentNameResource2">
                                    </as:LinkButton>
                                    <as:PlaceHolder ID="uxIconFutureStartDate" runat="server" Visible="false">
                                        <img src='<%# ResolveUrl("~/") %>res/img/icon-future-start-date.png' class="rcCalPopup"
                                            alt='<%# GetLocalResourceObject("rm_Assignment_Management_aspx_img_icon_future_startDate").ToString() %>'
                                            title='<%# GetLocalResourceObject("rm_Assignment_Management_aspx_img_icon_future_startDate").ToString() %>'
                                            style="cursor: pointer;" />
                                    </as:PlaceHolder>
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Left"></itemstyle>
                            </tek:GridTemplateColumn>
                            <as:ASGridBoundColumn DataField="FutureStartDate" UniqueName="FutureStartDate" HeaderStyle-Width="120px"
                                ItemStyle-HorizontalAlign="Center" HeaderText="Future Start Date" SortExpression="FutureStartDate"
                                HeaderTooltip="Future Start Date" HeaderStyle-HorizontalAlign="Center" Visible="false" meta:resourcekey="FutureStartDateASGridBoundColumnResource">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Type" DataField="AssignmentType" UniqueName="AssignmentType"
                                HeaderTooltip="Assignment Type" ItemStyle-HorizontalAlign="center" meta:resourcekey="ASGridBoundColumnResource36">

                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>

                                <itemstyle horizontalalign="Center"></itemstyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn HeaderText="Count" DataField="TotalMerchantCount" ASFormat="Integer"
                                HeaderTooltip="Number of distinct merchants that qualify for an assignment based on the assignment filters selected (a merchant can re-alert for an assignment, but is counted as eligible once)"
                                SortExpression="TotalMerchantCount" ItemStyle-HorizontalAlign="Right" ASDefaultNullValue="—" meta:resourcekey="ASGridBoundColumnResource37">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>
                            <%--Alerted--%>
                            <as:ASGridBoundColumn HeaderText="Count" DataField="AlertMerchantCount" UniqueName="AlertMerchantCount" ASDefaultNullValue="—"
                                ASFormat="Integer" ItemStyle-HorizontalAlign="Right" HeaderTooltip="Number of distinct merchants that have alerted or re-alerted based on the assignment parameter configurations"
                                SortExpression="AlertMerchantCount" meta:resourcekey="ASGridBoundColumnResource38">

                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>

                                <itemstyle horizontalalign="Right"></itemstyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn HeaderText="Net Amount" DataField="AlertMerchantVolume" UniqueName="Volume" ASDefaultNullValue="—"
                                ASFormat="Currency" ItemStyle-Wrap="true" HeaderTooltip="Cumulative amount associated with the Alerted Count"
                                SortExpression="AlertMerchantVolume" meta:resourcekey="ASGridBoundColumnResource39">

                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>

                                <itemstyle wrap="True"></itemstyle>
                            </as:ASGridBoundColumn>
                            <%--Ready to Work--%>
                            <as:ASGridBoundColumn HeaderText="Count" DataField="WKCount" ASFormat="Integer" ASDefaultNullValue="—"
                                UniqueName="WKCount" HeaderTooltip="Number of distinct merchants that have alerted or re-alerted, but have not been worked"
                                SortExpression="WKCount" meta:resourcekey="ASGridBoundColumnResource54">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Net Amount" DataField="WKVolume" UniqueName="WKVolume" HeaderStyle-Width="120px"
                                ItemStyle-Wrap="true" ASFormat="Currency" HeaderTooltip="Cumulative amount associated with the Ready to Work - Count"
                                SortExpression="WKVolume" meta:resourcekey="ASGridBoundColumnResource55" ItemStyle-Width="120px" ASDefaultNullValue="—">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>

                                <itemstyle wrap="True"></itemstyle>
                            </as:ASGridBoundColumn>
                            <%--Work in process--%>
                            <as:ASGridBoundColumn HeaderText="Count" DataField="WIPCount" UniqueName="WIPCount" ASDefaultNullValue="—"
                                ASFormat="Integer" SortExpression="WIPCount" HeaderTooltip="Number of distinct merchants marked as Work in Progress" meta:resourcekey="ASGridBoundColumnResource60">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Net Amount" DataField="WIPVolume" UniqueName="WIPVolume" ASDefaultNullValue="—"
                                ASFormat="Currency" SortExpression="WIPVolume" HeaderTooltip="Cumulative amount associated with the Work in Progress - Count" meta:resourcekey="ASGridBoundColumnResource53" HeaderStyle-Width="120px">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>

                            <%--Worked--%>
                            <as:ASGridBoundColumn HeaderText="Count" DataField="WorkedCount" UniqueName="WorkedCount" ASDefaultNullValue="—"
                                SortExpression="WorkedCount" HeaderTooltip="Number of distinct merchants worked" meta:resourcekey="ASGridBoundColumnResource40">

                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn HeaderText="Net Amount" DataField="WorkedVolume" UniqueName="WorkedVolume" ASDefaultNullValue="—"
                                ASFormat="Currency" ItemStyle-HorizontalAlign="Right" SortExpression="WorkedVolume"
                                HeaderTooltip="Cumulative amount associated with the Worked - Count" meta:resourcekey="ASGridBoundColumnResource41">

                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>

                                <itemstyle horizontalalign="Right"></itemstyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridTemplateColumn HeaderText="Percent Worked" DataField="CompletePercent" SortExpression="CompletePercent"
                                UniqueName="CompleteTemplate" HeaderTooltip="Percent of merchants worked in relation to the number of merchants alerted = Worked Count / Alerted Count"
                                ItemStyle-CssClass="percent-column" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                meta:resourcekey="GridTemplateColumnResource10" ASDefaultNullValue="—">
                                <itemtemplate>
                                    <uc:ProgressBar ID="progressBar" runat="server" />
                                </itemtemplate>

                                <headerstyle horizontalalign="Center"></headerstyle>

                                <itemstyle horizontalalign="Center" cssclass="percent-column"></itemstyle>
                            </as:ASGridTemplateColumn>

                            <%--Requeued--%>
                            <as:ASGridBoundColumn HeaderText="Count" DataField="RequeueCount" UniqueName="RequeueCount" ItemStyle-HorizontalAlign="Right"
                                HeaderTooltip="Number of distinct merchants that have been added to a Work Queue Assignment from a Detection Queue Assignment using the Re-Queue or Auto Queue feature" ASDefaultNullValue="—" meta:resourcekey="ASGridBoundColumnResource42">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Net Amount" DataField="RequeueVolume" UniqueName="RequeueVolume" ItemStyle-HorizontalAlign="Right"
                                HeaderTooltip="Cumulative amount associated with the Re-queued Count" ASDefaultNullValue="—" meta:resourcekey="ASGridBoundColumnResource43">

                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridTemplateColumn DataField="StartDate" UniqueName="StartDateTemplate"
                                HeaderText="Start Date" SortExpression="StartDate" HeaderStyle-Width="120px"
                                HeaderTooltip="The first date the assignment will be run" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="StartDateASGridTemplateColumnResource">
                                <itemtemplate>
                                    <div id='<%# Eval("AssignmentID") %>' onmouseover="GetAssignementID(this)">
                                        <as:PlaceHolder ID="uxStartDateCalendar" runat="server">
                                            <asp:TextBox ID="uxStartDate" ReadOnly="True" onkeypress="return false;" Width="85px"
                                                onclick="showPopup(this, event);" onfocus="showPopup(this, event);" onblur="parseDate(this, event)"
                                                runat="server" meta:resourcekey="uxStartDateResource1" />
                                            <img src='<%# ResolveUrl("~/") %>res/img/calendar_ico.png' class="rcCalPopup"
                                                alt='<%# GetLocalResourceObject("rm_Assignment_Management_aspx_img_alt1_startDate").ToString() %>'
                                                onclick="showPopup(this, event, '<%# AS.Web.Business.Shared.Constants.ManageAssignmentConstanst.EVENT_FIELD_START_DATE %>', '<%# Eval("ExpirationDate") %>')" style="cursor: pointer;" />
                                        </as:PlaceHolder>
                                    </div>
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </as:ASGridTemplateColumn>
                            <tek:GridTemplateColumn DataField="ExpirationDate" UniqueName="ExpirationDateTemplate" HeaderStyle-Width="120px"
                                ItemStyle-HorizontalAlign="Center" HeaderText="Expiration Date" SortExpression="ExpirationDate"
                                HeaderTooltip="The last date that the assignment is scheduled to execute" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="GridTemplateColumnResource11">
                                <itemtemplate>
                                    <div id='<%# Eval("AssignmentID") %>' onmouseover="GetAssignementID(this)">
                                        <as:PlaceHolder ID="uxCalendar" runat="server">
                                            <asp:TextBox ID="uxExpirationDate" ReadOnly="True" onkeypress="return false;" Width="86px"
                                                onclick="showPopup(this, event);" onfocus="showPopup(this, event);" onblur="parseDate(this, event)"
                                                runat="server" meta:resourcekey="uxExpirationDateResource2" />
                                            <img src='<%# ResolveUrl("~/") %>res/img/calendar_ico.png' class="rcCalPopup" alt='<%# GetLocalResourceObject("rm_Assignment_Management_aspx_img_alt1").ToString() %>'
                                                onclick="showPopup(this, event, '<%# AS.Web.Business.Shared.Constants.ManageAssignmentConstanst.EVENT_FIELD_EXPIRATION_DATE %>', '<%# Eval("StartDate") %>')" style="cursor: pointer;">
                                        </as:PlaceHolder>
                                        <as:Literal ID="uxExpirationDateNever" Text="Never Expire" Visible="False" runat="server" meta:resourcekey="uxExpirationDateNeverResource3" />
                                    </div>
                                </itemtemplate>

                                <headerstyle horizontalalign="Center"></headerstyle>

                                <itemstyle horizontalalign="Center"></itemstyle>

                            </tek:GridTemplateColumn>
                            <as:ASGridBoundColumn DataField="StartDate" UniqueName="StartDate" HeaderStyle-Width="120px"
                                ItemStyle-HorizontalAlign="Center" HeaderText="Start Date" SortExpression="StartDate"
                                HeaderTooltip="Start Date" HeaderStyle-HorizontalAlign="Center" Visible="false" meta:resourcekey="StartDateASGridBoundColumnResource">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn DataField="StartDateExport" UniqueName="StartDateExport" HeaderStyle-Width="120px"
                                ItemStyle-HorizontalAlign="Center" HeaderText="Start Date" SortExpression="StartDateExport"
                                HeaderTooltip="Start Date" HeaderStyle-HorizontalAlign="Center" Visible="false" meta:resourcekey="StartDateASGridBoundColumnResource">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn DataField="ExpirationDateExport" UniqueName="ExpirationDateExport" HeaderStyle-Width="120px"
                                ItemStyle-HorizontalAlign="Center" HeaderText="Expiration Date" SortExpression="ExpirationDateExport"
                                HeaderTooltip="Expiration Date" HeaderStyle-HorizontalAlign="Center" Visible="false" meta:resourcekey="ASGridBoundColumnResource46">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>

                                <itemstyle horizontalalign="Center"></itemstyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn DataField="ExpirationDate" UniqueName="ExpirationDate" Visible="false" meta:resourcekey="ASGridBoundColumnResource47">

                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn HeaderText="Proc Status" DataField="ProcessingStatus" UniqueName="ProcessingStatus" HeaderStyle-Width="110px"
                                HeaderTooltip="The processing status of the assignment" SortExpression="ProcessingStatus" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource48">

                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>

                                <itemstyle horizontalalign="Center"></itemstyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn HeaderText="Last Proc Status Date" DataField="ProcessingStatusDate"
                                UniqueName="ProcessingStatusDate" HeaderTooltip="The date and time of the processing status"
                                ASFormat="DateAndTime12Hours" SortExpression="ProcessingStatusDate" ASDefaultNullValue="—"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource49">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>

                                <itemstyle horizontalalign="Center"></itemstyle>
                            </as:ASGridBoundColumn>
                            <tek:GridTemplateColumn HeaderText="Delete" DataField="AssignmentID" UniqueName="Delete"
                                HeaderTooltip="Click to delete assignment" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center" meta:resourcekey="GridTemplateColumnResource12">
                                <itemtemplate>
                                    <as:Literal ID="lbDelete" runat="server" meta:resourcekey="lbDeleteResource2" />
                                </itemtemplate>

                                <headerstyle horizontalalign="Center"></headerstyle>

                                <itemstyle horizontalalign="Center"></itemstyle>
                            </tek:GridTemplateColumn>
                            <as:ASGridBoundColumn DataField="IsReAssign" UniqueName="IsReAssign" Visible="false" meta:resourcekey="ASGridBoundColumnResource50">
                                <columnvalidationsettings>
                                    <modelerrormessage text=""></modelerrormessage>
                                </columnvalidationsettings>

                                <headerstyle horizontalalign="Center"></headerstyle>
                            </as:ASGridBoundColumn>
                        </columns>

                        <pagerstyle alwaysvisible="True"></pagerstyle>
                    </tek:GridTableView>
                </detailtables>
                <columns>
                    <as:ASGridBoundColumn UniqueName="UserName" DataField="UserName" HeaderText="" ItemStyle-CssClass="group-heading text-left" meta:resourcekey="ASGridBoundColumnResource51">
                        <columnvalidationsettings>
                            <modelerrormessage text=""></modelerrormessage>
                        </columnvalidationsettings>

                        <headerstyle horizontalalign="Center"></headerstyle>

                        <itemstyle cssclass="group-heading text-left"></itemstyle>
                    </as:ASGridBoundColumn>
                </columns>

                <itemstyle horizontalalign="Center" verticalalign="Middle" cssclass="rgAltRow"></itemstyle>

            </mastertableview>
        </as:ASGrid>


        <div class="display-none">
            <as:HiddenField ID="hddAssignmentID" runat="Server" />
            <as:HiddenField ID="hddOpenEditAssignmentID" runat="Server" />
            <as:HiddenField ID="hddOpenEditAssignmentType" runat="Server" />
            <as:HiddenField ID="hddExtendDate" runat="Server" />
            <as:HiddenField ID="hddFieldTypeOfDateChange" runat="Server" />
            <as:HiddenField ID="hf1" runat="Server" />
            <as:HiddenField ID="hf2" runat="Server" />
            <as:HiddenField ID="hddFilter" runat="server" />
            <as:HiddenField ID="hddAssigmentDelete" runat="Server" />
            <as:Button ID="btnOpenConfirmModal" runat="Server" OnClick="btnOpenConfirmModal_Click" IsStandardButton="False" meta:resourcekey="btnDeleteAssignmentResource1" />
            <as:Button ID="btnDeleteAssignment" runat="Server" OnClick="btnDeleteAssignment_Click" IsStandardButton="False" meta:resourcekey="btnDeleteAssignmentResource1" />
            <as:Button ID="btnExtendAssignment" runat="Server" OnClick="btnExtendAssignment_Click" IsStandardButton="False" meta:resourcekey="btnExtendAssignmentResource1" />
            <as:Button ID="btnRebind" runat="server" Text="Rebind" OnClick="btnRebind_Click" IsStandardButton="False" meta:resourcekey="btnRebindResource2" />
            <asp:Button ID="uxOpenEditAssignment" runat="server" Text="Button" OnClick="uxOpenEditAssignment_Click" />
            <asp:Button ID="uxOpenDuplicateAssignment" runat="server" Text="Button" OnClick="uxOpenDuplicateAssignment_Click" />
            <as:RadDatePicker ID="uxSharedDatePicker" runat="Server" Width="100px" meta:resourcekey="uxSharedDatePickerResource1">
                <calendar id="Calendar3" fastnavigationstep="12" showrowheaders="false" runat="server">
                </calendar>
                <dateinput id="DateInput1" readonly="true" runat="server" dateformat="MM/dd/yyyy"
                    onkeypress="return false;" labelwidth="64px" width="">
                    <emptymessagestyle resize="None"></emptymessagestyle>

                    <readonlystyle resize="None"></readonlystyle>

                    <focusedstyle resize="None"></focusedstyle>

                    <disabledstyle resize="None"></disabledstyle>

                    <invalidstyle resize="None"></invalidstyle>

                    <hoveredstyle resize="None"></hoveredstyle>

                    <enabledstyle resize="None"></enabledstyle>
                </dateinput>

                <datepopupbutton imageurl="" hoverimageurl="" cssclass=""></datepopupbutton>
                <clientevents ondateselected="ExtendAssignment" />
            </as:RadDatePicker>
        </div>
       
    </div>
    <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
        <script type="text/javascript">
            var assignment = {
                assignmentFilter: new advancedFilter.create({
                    sectionName: '<%= FilterPageEnums.ManageAssignments %>',
                    btnRefresh: '<%= btnRefresh.ClientID%>',
                    hddApplyFilterId: '<%= hddApplyFilterId.ClientID%>',
                    hddDateAvailableFilter: '<%= _filterDate.ToString() %>',
                }),
            };

            var rm_Assignment_Management_uxFilterValuePanelUser = '<%= uxFilterValuePanelUser.ClientID %>';
            var rm_Assignment_Management_uxFilterValuePanelGroup = '<%= uxFilterValuePanelGroup.ClientID %>';
            var rm_Assignment_Management_uxFilterValuePanelAssignment = '<%= uxFilterValuePanelAssignment.ClientID %>';

            var rm_Assignment_Management_uxFilterType = '<%=uxFilterType.ClientID%>';
            var rm_Assignment_Management_uxComboUser = '<%= uxComboUser.ClientID %>';
            var rm_Assignment_Management_uxComboGroup = '<%= uxComboGroup.ClientID %>';
            var rm_Assignment_Management_uxComboAssignment = '<%= uxComboAssignment.ClientID %>';

            var rm_Assignment_Management_uxAssignmentGrid = '<%= uxAssignmentGrid.ClientID %>';
            var rm_Assignment_Management_uxSubmit = '<%=uxBtnSubmit.ClientID%>';
            var rm_Assignment_Management_uxProxyButton = '<%=uxRealSubmit.ClientID%>';
            var rm_Assignment_Management_uxGroupBy = '<%=uxGroupBy.ClientID%>';
            var rm_Assignment_Management_uxReportDate = '<%=uxReportDate.ClientID%>';

            var rm_Assignment_Management_hddFilter = '<%=hddFilter.ClientID%>';
            var rm_Assignment_Management_hddAssignmentID = '<%=hddAssignmentID.ClientID %>';
            var rm_Assignment_Management_hddOpenEditAssignmentID = '<%=hddOpenEditAssignmentID.ClientID %>';
            var rm_Assignment_Management_hf1 = '<%=hf1.ClientID%>';
            var rm_Assignment_Management_hf2 = '<%=hf2.ClientID%>';
            var rm_Assignment_Management_hddExtendDate = '<%=hddExtendDate.ClientID %>';
            var rm_Assignment_Management_btnDeleteAssignment = '<%=btnDeleteAssignment.ClientID %>';
            var rm_Assignment_Management_btnRebind = '<%=btnRebind.ClientID %>';
            var rm_Assignment_Management_uxSharedDatePicker = '<%=uxSharedDatePicker.ClientID %>';
            var rm_Assignment_Management_btnExtendAssignment = '<%=btnExtendAssignment.ClientID %>';
            var rm_Assignment_Management_uxListOfGridId = '<%=uxListOfGridId.ClientID %>';
            var rm_Assignment_Management_btnRefresh = '<%=btnRefresh.ClientID %>';
            var rm_Assignment_Management_uxOpenEditAssignment = '<%=uxOpenEditAssignment.ClientID %>';
            var rm_Assignment_Management_uxOpenDuplicateAssignment = '<%=uxOpenDuplicateAssignment.ClientID %>';
            var rm_Assignment_Management_hddFieldTypeOfDateChange = '<%=hddFieldTypeOfDateChange.ClientID %>';

            var rm_Assignment_Management_hasQueuingMechanism = '<%=HasQueuingMechanism %>'.toLowerCase() == 'true';
            var rm_Assignment_Management_EnabledByReadOnly = '<%=EnabledByReadOnly%>'.toLowerCase() == 'true';
            var rm_Assignment_Management_js_DeleteAssignment = '<%= GetLocalResourceObject("rm_Assignment_Management_js_DeleteAssignment").ToString()%>';
            var rm_Assignment_Management_js_ExpirationDate = '<%= GetLocalResourceObject("rm_Assignment_Management_js_ExpirationDate").ToString()%>';
            var rm_Assignment_Management_js_UsermustSelect = '<%= GetLocalResourceObject("rm_Assignment_Management_js_UsermustSelect").ToString()%>';
            var rm_Assignment_Management_js_GroupmustSelect = '<%= GetLocalResourceObject("rm_Assignment_Management_js_GroupmustSelect").ToString()%>';
            var rm_Assignment_Management_js_AssignmentmustSelect = '<%= GetLocalResourceObject("rm_Assignment_Management_js_AssignmentmustSelect").ToString()%>';
            var rm_Assignment_Management_js_GridHeaderMerchantCount = '<%= GetLocalResourceObject("rm_Assignment_Management_js_GridHeaderMerchantCount").ToString()%>';
            var rm_Assignment_Management_js_GridHeaderWorked = '<%= GetLocalResourceObject("rm_Assignment_Management_js_GridHeaderWorked").ToString()%>';
            var rm_Assignment_Management_js_GridHeaderWorkInProgress = '<%= GetLocalResourceObject("rm_Assignment_Management_js_GridHeaderWorkInProgress").ToString()%>';
            var rm_Assignment_Management_js_GridHeaderRe_queued = '<%= GetLocalResourceObject("rm_Assignment_Management_js_GridHeaderRe_queued").ToString()%>';
            var rm_Assignment_Management_js_ReportedDate_Invalid = '<%= GetLocalResourceObject("rm_Assignment_Management_js_ReportedDate_Invalid").ToString()%>';
            var rm_Assignment_Management_js_ReportedDate_GreaterToday = '<%= GetLocalResourceObject("rm_Assignment_Management_js_ReportedDate_GreaterToday").ToString()%>';
            var rm_Assignment_Management_js_GridHeaderEligible = '<%= GetLocalResourceObject("rm_Assignment_Management_js_GridHeaderEligible").ToString()%>';
            var rm_Assignment_Management_js_GridHeaderAlerted = '<%= GetLocalResourceObject("rm_Assignment_Management_js_GridHeaderAlerted").ToString()%>';
            var rm_Assignment_Management_js_GridHeaderReadyToWork = '<%= GetLocalResourceObject("rm_Assignment_Management_js_GridHeaderReadyToWork").ToString()%>';
            var rm_Assignment_Management_js_GridHeaderDistinctMerchantsCurrentStatus = '<%= GetLocalResourceObject("rm_Assignment_Management_js_GridHeaderDistinctMerchantsCurrentStatus").ToString()%>';
            var rm_Assignment_Management_js_StartDateOutOfRangeTooltip = '<%= GetLocalResourceObject("StartDateOutOfRangeTooltip").ToString()%>';
            var rm_Assignment_Management_js_StartDate = '<%= GetLocalResourceObject("rm_Assignment_Management_js_StartDate").ToString()%>'

            var rm_Assignment_Management_uxBtnAssignmentFilter = '<%=uxBtnAssignmentFilter.ClientID %>';
            var rm_Assignment_Management_hddOpenEditAssignmentType = '<%=hddOpenEditAssignmentType.ClientID %>'
            var rm_Assignment_Management_DateTimeToNow = '<%= GetDateToNow().ToString("MM/dd/yyyy") %>'
            var rm_Assignment_Management_DateTime_MaxValue = '<%= DateTime.MaxValue.ToString("MM/dd/yyyy") %>';
            var rm_Assignment_Management_hddAssignmentDetete = '<%= hddAssigmentDelete.ClientID %>';
            var rm_Assignment_Management_btnOpenConfirmModal = '<%= btnOpenConfirmModal.ClientID %>';
        </script>
        <script src="<%=ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_Assignment_Management.js"></script>
    </as:ASRadCodeBlock>
</asp:Content>
