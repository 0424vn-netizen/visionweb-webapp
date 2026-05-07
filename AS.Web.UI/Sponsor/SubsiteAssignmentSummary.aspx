<%@ Page Title="Risk Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SubsiteAssignmentSummary.aspx.cs" Inherits="As.VisionWeb.Web.SubsiteAssignmentSummary" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="RadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxSponsoredEntityGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxSponsoredEntityGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxEntityList_Data">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxSubsiteAssignment_Data" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxEntityList">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxSubsiteAssignment" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <as:Panel runat="server" ID="uxFilteringOptionsContainer"
        Width="100%" TemplateName="ascontainer_greyborder.tpl" meta:resourcekey="uxFilteringOptionsContainerResource1">
        <div class="row collapse report-filter-panel">
            <div class="col-md-12 report-filter trans-filter">
                <div class="report-filter-default">
                    <div class="filter-left">
                        <div class="filter-item">
                            <asp:RadioButton ID="uxDaily" runat="server" Text="Daily" GroupName="Date" onclick="ChangeDateOption(this)"
                                onkeypress="javascript:return false;" CssClass="date_item date-item" meta:resourcekey="uxDaily" Checked="true" />
                            <asp:RadioButton ID="uxDateRange" runat="server" Text="Date Range" GroupName="Date" onclick="ChangeDateOption(this)"
                                onkeypress="javascript:return false;" CssClass="date_item date-item" meta:resourcekey="uxRange" />
                        </div>
                        <div>
                            <asp:Label ID="lbSponsorEntityName" runat="server" CssClass="filter-item" meta:resourcekey="lblSponsoredEntityName"></asp:Label>
                        </div>
                        <div class="mt-10">
                            <asp:Label ID="lblSubsiteAssignmentName" runat="server" meta:resourcekey="lblSubsiteAssignmentName"></asp:Label>
                        </div>
                    </div>
                    <div class="filter-right sponsord-filter">
                        <div class="filter-item">
                            <div class="display-inline">
                                <as:RadDatePicker ID="uxBeginDate" ShowPopupOnFocus="true" runat="server" Width="128px">
                                    <Calendar ID="Calendar2" FastNavigationStep="12" ShowRowHeaders="false" runat="server" />
                                    <DateInput ID="DateInput2" runat="server" />
                                </as:RadDatePicker>
                            </div>
                            <div class="display-inline hide" id="uxEndDate-wrapper">
                                <as:RadDatePicker ID="uxEndDate" ShowPopupOnFocus="true" runat="server" Width="128px">
                                    <Calendar ID="Calendar3" FastNavigationStep="12" ShowRowHeaders="false" runat="server" />
                                    <DateInput ID="DateInput3" runat="server" />
                                </as:RadDatePicker>
                            </div>
                            <div class="display-block">
                                <as:RadComboBox ID="uxEntityList" runat="server" EnableEmbeddedBaseStylesheet="False" Filter="Contains"
                                    DataValueField="EntityNumber" AutoPostBack="true" DataTextField="EntityName" Width="370px" CssClass="hide" OnSelectedIndexChanged="uxEntityList_SelectedIndexChanged">
                                </as:RadComboBox>
                                <div class="filter-item" id="uxMultiEntities">
                                    <div class="multichooser-wrapper">
                                        <as:MultiChooser IsInTelerikAjax="true" IsSearchContains="true" Width="370px" CssClass="form-control"
                                            ID="uxEntityList_Data" AutoPostBack="true" runat="server"  Placeholder=" " OnTextChanged="uxEntityList_Data_TextChanged"
                                            meta:resourcekey="uxSourceResource1">
                                        </as:MultiChooser>
                                    </div>
                                </div>
                            </div>
                            <div class="display-block">
                                <as:RadComboBox ID="uxSubsiteAssignment" runat="server" EnableEmbeddedBaseStylesheet="False" Filter="Contains"
                                    DataValueField="AssignmentID" DataTextField="AssignmentName" Width="370px">
                                </as:RadComboBox>
                                <div class="filter-item" id="uxMultiSubsiteAssignment">
                                    <div class="multichooser-wrapper">
                                        <as:MultiChooser IsInTelerikAjax="true" IsSearchContains="true" Width="370px" CssClass="form-control"
                                            ID="uxSubsiteAssignment_Data" AutoPostBack="false" runat="server" Placeholder=" "
                                            meta:resourcekey="uxSourceResource1">
                                        </as:MultiChooser>
                                    </div>
                                </div>
                                <div class="display-inline">
                                    <as:Button ID="uxSearchButton" runat="server" Text="Submit" OnClick="uxSearchButton_OnClick"
                                        OnClientClick="return ValidateData();" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="Search" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </as:Panel>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter">
                <as:Literal ID="Literal2" runat="server" Text="FILTER" meta:resourcekey="Filter"></as:Literal></span>
        </div>
    </div>
    <div class="main-content">
        <div class="text-right">
            <a id="uxLinkSponsoredEntityPortfolioSummary" runat="server"><%= GetLocalResourceObject("SponsoredEntityPortfolioSummary.Text").ToString()%></a>
        </div>
        <uc:UxExport ID="uxExportuxSponsoredEntity" runat="server" GridID="uxSponsoredEntityGrid"
            FileName="SponsoredEntityPortfolioSummary"
            ShowWord="false" ShowPDF="false" meta:resourcekey="uxExportSponsoredEntityPortfolioSummary" OnNeedExportConfig="uxExportuxSponsoredEntity_NeedExportConfig" />
        <as:ASGrid ID="uxSponsoredEntityGrid" runat="server" AutoGenerateColumns="False" ASPagingMethod="SPASingleMethod1"
            AllowSorting="true" AllowPaging="true" GridLines="None" IsIntruder="true" AllowFilteringByColumn="false"
            IsAutoExportTemplate="true"
            ShowPageTotal="false" XOverFlowable="true" HeaderStyle-Width="100px" CssClass="in" meta:resourcekey="uxSponsoredEntityGrid"
            AllowSortFilterWhenExport="false" OnNeedDataSource="uxSponsoredEntityGrid_NeedDataSource">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                        HeaderTooltip="Report Date" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ReportDate">
                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                     <as:ASGridBoundColumn HeaderText="SponsoredName" DataField="SponsoredName" UniqueName="SponsoredName"
                        HeaderTooltip="SponsoredName" ASFormat="StaticString" HeaderStyle-Width="120px"
                        meta:resourcekey="SponsoredEntityName">
                        <HeaderStyle HorizontalAlign="Center" Width="200px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="AssignmentName" DataField="AssignmentName" UniqueName="AssignmentName"
                        HeaderTooltip="AssignmentName" ASFormat="StaticString" HeaderStyle-Width="120px"
                        meta:resourcekey="AssignmentName">
                        <HeaderStyle HorizontalAlign="Center" Width="200px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="EligibleCount" DataField="EligibleCount" UniqueName="EligibleCount"
                        HeaderTooltip="EligibleCount" ASFormat="Integer" HeaderStyle-Width="120px"
                        meta:resourcekey="EligibleCount">
                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="AlertedCount" DataField="AlertedCount" UniqueName="AlertedCount"
                        HeaderTooltip="Batch Number" ASFormat="Integer" meta:resourcekey="AlertedCount">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="AlertedNetAmount" DataField="AlertedNetAmount" UniqueName="AlertedNetAmount"
                        ASFormat="Currency" SortExpression="AlertedNetAmount"
                        HeaderTooltip="Keyed Entry Count" meta:resourcekey="AlertedNetAmount">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="ReadyToWorkCount" DataField="ReadyToWorkCount" UniqueName="ReadyToWorkCount"
                        HeaderTooltip="ReadyToWorkCount" ASFormat="Integer" meta:resourcekey="ReadyToWorkCount">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="ReadyToWorkNetAmount" DataField="ReadyToWorkNetAmount" UniqueName="ReadyToWorkNetAmount"
                        ASFormat="Currency" SortExpression="ReadyToWorkNetAmount"
                        HeaderTooltip="WorkedNetAmount" meta:resourcekey="ReadyToWorkNetAmount">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="WorkedCount" DataField="WorkedCount" UniqueName="WorkedCount"
                        HeaderTooltip="WorkedCount" ASFormat="Integer" meta:resourcekey="WorkedCount">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="WorkedNetAmount" DataField="WorkedNetAmount" UniqueName="WorkedNetAmount"
                        ASFormat="Currency" SortExpression="WorkedNetAmount"
                        HeaderTooltip="WorkedNetAmount" meta:resourcekey="WorkedNetAmount">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="PercentageWorked" DataField="PercentageWorked" UniqueName="PercentageWorked"
                        ASFormat="Percentage" SortExpression="PercentageWorked"
                        HeaderTooltip="PercentageWorked" meta:resourcekey="PercentageWorked">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="LastProcessCompletionDate" DataField="LastProcessCompletionDate" UniqueName="LastProcessCompletionDate"
                        ASFormat="DateAndTime" SortExpression="LastProcessCompletionDate"
                        HeaderTooltip="LastProcessCompletionDate" meta:resourcekey="LastProcessCompletionDate">
                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </div>
    <as:RadCodeBlock ID="JavaScript" runat="server">
        <script type="text/javascript">          
            var rm_RiskReport_MerchantProfileURL = '<%= ResolveUrl("~/")%>MerchantProfile.aspx';
            var uxSponsoredEntityGrid = '<%= uxSponsoredEntityGrid.ClientID %>';
            var uxEntityList = '<%= uxEntityList.ClientID %>';
            var uxEntityList_DatauxEntityList_Data = '<%= uxEntityList_Data.ClientID %>';
            var uxBeginDate = '<%= uxBeginDate.ClientID %>';
            var uxEndDate = '<%= uxEndDate.ClientID %>';
            var uxDaily = '<%= uxDaily.ClientID %>';
            var uxDateRange = '<%= uxDateRange.ClientID %>';
            var Msg_V2 = "<%=Resources.MessageManager.ReportFilter_ReportDate_InvaidDate %>";
            var Msg_V3 = "<%=Resources.MessageManager.ReportFilter_V3%>";
            var dateRangeMsg = "<%= GetLocalResourceObject("uxRange.Text").ToString()%>";
            var uxEntityList_Data = "<%= uxEntityList_Data.ClientID%>";
            var AlertedMsg = "<%= GetLocalResourceObject("Alerted").ToString()%>";
            var ReadyToWorkMsg = "<%= GetLocalResourceObject("ReadyToWork").ToString()%>";
            var WorkedMsg = "<%= GetLocalResourceObject("Worked").ToString()%>";
            var uxSubsiteAssignment = "<%= uxSubsiteAssignment.ClientID%>";
            var ReportFilter_NotGreaterThanToDay = '<%=Resources.MessageManager.ReportFilter_V9%>';
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/sponsor/subsite-summary.js"></script>
    </as:RadCodeBlock>
</asp:Content>
