<%@ Page Title="Detection Queue" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="rm_MCF_DetectionQueue.aspx.cs" Inherits="rm_MCF_DetectionQueue" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="DetectionQueueAssignmentList" Src="~/UserControls/rm_MCF_DetectionQueueAssignmentList.ascx" TagPrefix="uc" %>
<%@ Register TagName="AdvancedFilter" Src="~/UserControls/UxAdvancedFilter.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <div class="main-content">
        <as:PlaceHolder ID="ads" runat="server">
            <as:RadAjaxManagerProxy ID="RadAjaxManager" runat="server">
                <AjaxSettings>
                    <tek:AjaxSetting AjaxControlID="btnChangeAllOrMy">
                        <UpdatedControls>
                            <tek:AjaxUpdatedControl ControlID="uxAssignmentList" />
                        </UpdatedControls>
                    </tek:AjaxSetting>
                    <tek:AjaxSetting AjaxControlID="btnRefresh">
                        <UpdatedControls>
                            <tek:AjaxUpdatedControl ControlID="pnlAssignmentInfo" />
                            <tek:AjaxUpdatedControl ControlID="uxbtn_BarometerReport" LoadingPanelID="uxInvisiblePanel" />
                            <tek:AjaxUpdatedControl ControlID="uxbtn_SecurityReport" LoadingPanelID="uxInvisiblePanel" />
                            <tek:AjaxUpdatedControl ControlID="uxbtn_NextQueue" LoadingPanelID="uxInvisiblePanel" />
                        </UpdatedControls>
                    </tek:AjaxSetting>
                    <tek:AjaxSetting AjaxControlID="uxRefreshBtn">
                        <UpdatedControls>
                            <tek:AjaxUpdatedControl ControlID="pnlAssignmentInfo" />
                        </UpdatedControls>
                    </tek:AjaxSetting>
                    <tek:AjaxSetting AjaxControlID="btnRefreshAssignmentGrid">
                        <UpdatedControls>
                            <tek:AjaxUpdatedControl ControlID="pnlAssignmentInfo" LoadingPanelID="uxInvisiblePanel" />
                        </UpdatedControls>
                    </tek:AjaxSetting>
                </AjaxSettings>
            </as:RadAjaxManagerProxy>

            <!-- Advanced Filter -->
            <uc:AdvancedFilter ID="uxAdvancedFilter" runat="server" />

            <!--FILTERING OPTIONS-->
            <div class="row collapse report-filter-panel">
                <div class="col-md-12 report-filter">
                    <div class="filter-block" id="filterBlock">
                        <table>
                            <tr>
                                <td class="text-right"></td>
                                <td class="text-left" colspan="2">
                                    <div id="cidAssignmentOption" runat="server">
                                        <div class="filter-item">
                                            <as:RadioButton runat="server" Text="All Assignments" ID="optAllMerchantAssignments" GroupName="AssignmentOption"
                                                onclick="ChangeAllOrMyAssignment()" CssClass="date-item first" meta:resourcekey="optAllMerchantAssignmentsResource1" />
                                            <as:RadioButton runat="server" Text="My Assignments" ID="optMyAssignments" GroupName="AssignmentOption"
                                                onclick="ChangeAllOrMyAssignment()" Checked="true" CssClass="date-item" meta:resourcekey="optMyAssignmentsResource1" />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right">
                                    <label class="filter-label">
                                        <asp:Literal ID="rm_DetectionQueue_aspx_AssignmentName" runat="server" meta:resourcekey="rm_DetectionQueue_aspx_AssignmentNameResource1"> Assignment Name:</asp:Literal></label>
                                </td>
                                <td class="text-left" colspan="2">
                                    <div class="filter-item">
                                        <as:RadComboBox ID="uxAssignmentList" runat="server" Height="150px" Width="250px" EnableTextSelection="true" Filter="Contains" MarkFirstMatch="true"
                                            MaxHeight="230px" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnClientKeyPressing="uxAssignmentList_OnClientKeyPressing"
                                            OnItemDataBound="uxAssignmentList_ItemDataBound" OnClientLoad="uxAssignmentList_OnClientSelectedIndexChanged" OnClientSelectedIndexChanged="uxAssignmentList_OnClientSelectedIndexChanged" meta:resourcekey="uxAssignmentListResource1">
                                        </as:RadComboBox>
                                    </div>
                                </td>

                            </tr>
                            <tr>
                                <td class="text-right">
                                    <label class="filter-label">
                                        <asp:Literal ID="rm_DetectionQueue_aspx_Date" runat="server" meta:resourcekey="rm_DetectionQueue_aspx_DateResource1">Date:</asp:Literal></label></td>
                                <td class="text-left" colspan="2">
                                    <div class="filter-item">
                                        <as:RadDatePicker ID="uxReportDate" runat="server" ShowPopupOnFocus="true" Width="128px" meta:resourcekey="uxReportDateResource1">
                                            <Calendar FastNavigationStep="12" ShowRowHeaders="false">
                                            </Calendar>
                                            <DateInput ID="DateInput1" DateFormat="MM/dd/yyyy" runat="Server"
                                                onkeypress="return DefaultEnterOnTextBox(event);" LabelWidth="64px" Width="">
                                                <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                                <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                                <FocusedStyle Resize="None"></FocusedStyle>

                                                <DisabledStyle Resize="None"></DisabledStyle>

                                                <InvalidStyle Resize="None"></InvalidStyle>

                                                <HoveredStyle Resize="None"></HoveredStyle>

                                                <EnabledStyle Resize="None"></EnabledStyle>
                                            </DateInput>

                                            <DatePopupButton ImageUrl="" HoverImageUrl="" CssClass=""></DatePopupButton>
                                        </as:RadDatePicker>
                                    </div>
                                </td>
                            </tr>
                            <tr id="clFilterOrderBy">
                                <td class="text-right">
                                    <label class="filter-label">
                                        <asp:Literal ID="rm_DetectionQueue_aspx_OrderBy" runat="server" meta:resourcekey="rm_DetectionQueue_aspx_OrderByResource1">Order By:</asp:Literal></label></td>
                                <td class="text-left">
                                    <div class="filter-item">
                                        <as:RadComboBox ID="uxOrderBy1" EnableTextSelection="true" Filter="Contains" MarkFirstMatch="true"
                                            runat="server" Width="250px" Height="250px" meta:resourcekey="uxOrderBy1Resource1" />
                                    </div>
                                </td>
                                <td class="text-left">
                                    <as:RadioButton ID="optAsc1" runat="server" GroupName="OrderBy1" Text="Ascending" CssClass="date-item" meta:resourcekey="optAsc1Resource1" />
                                    <as:RadioButton ID="optDesc1" runat="server" GroupName="OrderBy1" Text="Descending" Checked="true" CssClass="date-item" meta:resourcekey="optDesc1Resource1" />
                                </td>
                            </tr>
                            <tr id="clFilterOrderBy">
                                <td class="text-right">
                                    <label class="filter-label">
                                        <asp:Literal ID="rm_DetectionQueue_aspx_And" runat="server" meta:resourcekey="rm_DetectionQueue_aspx_AndResource1">And:</asp:Literal></label></td>
                                <td class="text-left">
                                    <div class="filter-item">
                                        <as:RadComboBox ID="uxOrderBy2" EnableTextSelection="true" Filter="Contains" MarkFirstMatch="true"
                                            runat="server" Width="250px" Height="250px" meta:resourcekey="uxOrderBy2Resource1">
                                        </as:RadComboBox>
                                    </div>
                                </td>
                                <td class="text-left">
                                    <as:RadioButton ID="optAsc2" runat="server" GroupName="OrderBy2" Text="Ascending" CssClass="date-item" meta:resourcekey="optAsc1Resource1" />
                                    <as:RadioButton ID="optDesc2" runat="server" GroupName="OrderBy2" Text="Descending" Checked="true" CssClass="date-item" meta:resourcekey="optDesc2Resource1" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td class="text-right">
                                    <div class="filter-item">
                                        <as:Button ID="uxSearchDetectionQueue" runat="server" Text="Submit" CssClass="btn btn-default"
                                            OnClick="uxSearchDetectionQueue_Click" OnClientClick="return uxSearchDetectionQueue_ClientClick();" meta:resourcekey="uxSearchDetectionQueueResource1" />

                                    </div>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
                    <span class="btn btn-link btn-report-filter">
                        <asp:Literal ID="rm_DetectionQueue_aspx_Filter" runat="server" meta:resourcekey="rm_DetectionQueue_aspx_FilterResource1">FILTER</asp:Literal></span>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 no-margin-bottom">
                    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Detection Queue" HasFilteringOption="true" meta:resourcekey="uxPageTitleResource1" />
                    <div style="position: absolute; right: 15px; top: 3px" runat="server" visible="false" id="advFilter">
                        <as:LinkButton runat="server" ID="btnRefresh" OnClick="btnRefresh_Click" OnClientClick="refreshClient();" Text="Refresh" Style="padding-right: 15px" meta:resourcekey="btnRefreshResource"></as:LinkButton>
                        <as:LinkButton runat="server" ID="btnAdvancedFilter" OnClientClick="detectionQueue.detectionQueueFilter.show(); return false;" Text="Advanced Filter" meta:resourcekey="btnAdvancedFilterResource"></as:LinkButton>
                    </div>
                </div>
            </div>
            <div id="apply-filter" class="apply-filter hide" data-show-adv-filter-item="true">
                <div class="height-14"></div>
                <div class="display-flex">
                    <label class="lbl-filter">
                        <asp:Literal ID="Literal11" runat="server" meta:resourcekey="lblFilter" Text="Filter:"></asp:Literal>
                    </label>
                    <div class="advanced-filter-item">
                        <span data-filter-item="true"></span>
                        <a class="filter-choice-close" onclick="detectionQueue.detectionQueueFilter.clearApplyAdvFilter(); refreshClient();"></a>
                    </div>
                </div>
                <as:HiddenField ID="hddApplyFilterId" runat="server" />
            </div>
            <div class="height-18"></div>
            <!--ASSIGNMENT INFO-->

            <as:Panel ID="pnlAssignmentInfo" runat="server" meta:resourcekey="pnlAssignmentInfoResource1">
                <uc:DetectionQueueAssignmentList ID="grdAssignment" runat="Server" Visible="false" FromPage="DetectionQueue" />
            </as:Panel>

            <%--BUTTON ACTION--%>
            <as:Panel ID="pnlgroupButton" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-12 d-flex justify-content-end mt-5x" data-selector="">
                        <as:Button ID="uxbtn_BarometerReport" runat="server" Text="Barometer Report" CssClass="btn btn-default"
                            meta:resourcekey="optRainbowResource1" />

                        <as:Button ID="uxbtn_SecurityReport" runat="server" Text="Security Report" CssClass="btn btn-default ml-20"
                            meta:resourcekey="optSecurityResource1" />


                        <as:LinkButton ID="uxbtn_NextQueue" runat="server" Text="Next Queue" CssClass="btn btn-default ml-20" OnClick="uxbtn_NextQueue_Click"
                            meta:resourcekey="optNextQResource1" />
                    </div>
                </div>
            </as:Panel>

            <div class="display-none">
                <as:Button ID="btnChangeAllOrMy" runat="server" Text="" OnClick="btnChangeAllOrMy_Click" meta:resourcekey="btnChangeAllOrMyResource1" />
            </div>
            <as:Button ID="btnRefreshAssignmentGrid" CssClass="hide" runat="server" OnClick="btnRefreshAssignmentGrid_Click" />
            <as:ASRadCodeBlock ID="radCodeBlock1" runat="Server">
                <script type="text/javascript">
                    var detectionQueue = {
                        detectionQueueFilter: new advancedFilter.create({
                            sectionName: '<%= FilterPageEnums.DetectionQueue %>',
                            btnRefresh: '<%= btnRefresh.ClientID%>',
                            hddApplyFilterId: '<%= hddApplyFilterId.ClientID%>',
                            hddDateAvailableFilter: '<%= _ReportDate.ToString()%>',
                            query: ''
                        }),
                    };

                    var rm_DetectionQueue_btnChangeAllOrMy = '<%= btnChangeAllOrMy.ClientID %>';
                    var rm_DetectionQueue_uxReportDate = '<%=uxReportDate.ClientID %>';
                    var rm_DetectionQueue_uxSearchDetectionQueue = '<%=uxSearchDetectionQueue.ClientID %>';
                    var rm_DetectionQueue_uxAssignmentList_ClientID = '<%=uxAssignmentList.ClientID%>';
                    var uxbtn_BarometerReport_ClientID = '<%=uxbtn_BarometerReport.ClientID%>';
                    var uxbtn_SecurityReport_ClientID = '<%=uxbtn_SecurityReport.ClientID%>';
                    var uxbtn_NextQueue_ClientID = '<%=uxbtn_NextQueue.ClientID%>';
                    var uxbtn_btnRefreshAssignmentGrid_ClientID = '<%=btnRefreshAssignmentGrid.ClientID%>';

                    var rm_DetectionQueue_AssignmentType = '<%=ASSIGNMENT_TYPE %>';
                    var rm_DetectionQueue_enum_DetectionQueue = '<%=(int)WebSiteEnums.AssignmentType.DetectionQueue %>';
                    var rm_DetectionQueue_enum_WorkQueue = '<%=(int)WebSiteEnums.AssignmentType.WorkQueue %>';
                    var rm_DetectionQueue_enum_DetectionQueueDistinct = '<%=(int)WebSiteEnums.AssignmentType.DetectionQueueDistinct %>';
                    var rm_DetectionQueue_enum_AggregateQueue = '<%=(int)WebSiteEnums.AssignmentType.AggregateQueue %>';

                </script>
                <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_DetectionQueue.js"></script>
            </as:ASRadCodeBlock>
        </as:PlaceHolder>
    </div>
</asp:Content>

