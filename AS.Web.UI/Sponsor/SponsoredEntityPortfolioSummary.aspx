<%@ Page Title="Risk Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SponsoredEntityPortfolioSummary.aspx.cs" Inherits="As.VisionWeb.Web.SponsoredEntityPortfolioSummary" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="RadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxSponsoredEntityGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxSponsoredEntityGrid" LoadingPanelID="uxLoadingPanelCustom" />
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
                                    DataValueField="EntityNumber" DataTextField="EntityName" Width="370px" CssClass="hide">
                                </as:RadComboBox>
                                <div class="filter-item" id="uxMultiEntities">
                                    <div class="multichooser-wrapper">
                                        <as:MultiChooser IsInTelerikAjax="true" IsSearchContains="true" Width="370px" CssClass="form-control"
                                            ID="uxEntityList_Data" AutoPostBack="false" runat="server" Placeholder=" "
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
        <uc:UxExport ID="uxExportuxSponsoredEntity" runat="server" GridID="uxSponsoredEntityGrid"
            FileName="SponsoredEntityPortfolioSummary" OnNeedExportConfig="uxExportuxSponsoredEntity_NeedExportConfig"
            ShowWord="false" ShowPDF="false" meta:resourcekey="uxExportSponsoredEntityPortfolioSummary" />
        <as:ASGrid ID="uxSponsoredEntityGrid" runat="server" AutoGenerateColumns="False" ASPagingMethod="SPASingleMethod2"
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
                    <as:ASGridTemplateColumn HeaderText="SponsoredNumber" DataField="SponsoredNumber" HeaderStyle-Width="280px"
                        UniqueName="SponsoredNumber" SortExpression="SponsoredNumber" HeaderTooltip="SponsoredNumber"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" meta:resourcekey="SponsoredEntityName">
                        <ItemTemplate>
                            <as:Literal ID="uxSponsoredEntityName" runat="server"></as:Literal>
                            <a id="uxGotoSubsite" runat="server" class="image-link">
                                 <img class="vertical-top " src="/res/images/icon-file.png" />  
                            </a>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn HeaderText="EligibleMerchantCount" DataField="EligibleMerchantCount" UniqueName="EligibleMerchantCount"
                        HeaderTooltip="EligibleMerchantCount" ASFormat="Integer"
                        meta:resourcekey="EligibleMerchantCount" ASIsTotalColumn="true">
                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="AlertedCount" DataField="AlertedCount" UniqueName="AlertedCount"
                        HeaderTooltip="Batch Number" ASFormat="Integer" meta:resourcekey="AlertedCount" ASIsTotalColumn="true">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="AlertedPercentage" DataField="AlertedPercentage" UniqueName="AlertedPercentage"
                        ASFormat="Percentage" SortExpression="AlertedPercentage"
                        HeaderTooltip="Transaction Count" meta:resourcekey="AlertedPercentage" ASIsTotalColumn="true">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="AlertedNetAmount" DataField="AlertedNetAmount" UniqueName="AlertedNetAmount"
                        ASFormat="Currency" SortExpression="AlertedNetAmount"
                        HeaderTooltip="Keyed Entry Count" meta:resourcekey="AlertedNetAmount" ASIsTotalColumn="true">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="WorkedCount" DataField="WorkedCount" UniqueName="WorkedCount"
                        HeaderTooltip="WorkedCount" ASFormat="Integer" meta:resourcekey="WorkedCount" ASIsTotalColumn="true">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="WorkedPercentage" DataField="WorkedPercentage" UniqueName="WorkedPercentage"
                        ASFormat="Percentage" SortExpression="WorkedPercentage"
                        HeaderTooltip="WorkedPercentage" meta:resourcekey="WorkedPercentage" ASIsTotalColumn="true">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="WorkedNetAmount" DataField="WorkedNetAmount" UniqueName="WorkedNetAmount"
                        ASFormat="Currency" SortExpression="WorkedNetAmount"
                        HeaderTooltip="WorkedNetAmount" meta:resourcekey="WorkedNetAmount" ASIsTotalColumn="true">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="CasesOpenedCount" DataField="CasesOpenedCount" UniqueName="CasesOpenedCount"
                        ASFormat="Integer" SortExpression="CasesOpenedCount"
                        HeaderTooltip="CasesOpenedCount" meta:resourcekey="CasesOpenedCount" ASIsTotalColumn="true">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="CasesClosedCount" DataField="CasesClosedCount" UniqueName="CasesClosedCount"
                        ASFormat="Integer" SortExpression="CasesClosedCount"
                        HeaderTooltip="CasesClosedCount" meta:resourcekey="CasesClosedCount" ASIsTotalColumn="true">
                        <HeaderStyle HorizontalAlign="Center" CssClass="mh" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="AlertedSubsiteAssignmentCount" DataField="AlertedSubsiteAssignmentCount" UniqueName="AlertedSubsiteAssignmentCount"
                        ASFormat="Integer" SortExpression="AlertedSubsiteAssignmentCount" ASIsTotalColumn="true"
                        HeaderTooltip="AlertedSubsiteAssignmentCount" meta:resourcekey="AlertedSubsiteAssignmentCount" HeaderStyle-Width="120px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="ViolatedParameterCount" DataField="ViolatedParameterCount" UniqueName="ViolatedParameterCount"
                        ASFormat="Integer" SortExpression="ViolatedParameterCount" ASIsTotalColumn="true"
                        HeaderTooltip="ViolatedParameterCount" meta:resourcekey="ViolatedParameterCount" HeaderStyle-Width="120px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
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
            var CasesMsg = "<%= GetLocalResourceObject("Cases").ToString()%>";
            var WorkedMsg = "<%= GetLocalResourceObject("Worked").ToString()%>";
            var DisinctMerchantDesc = "<%= GetLocalResourceObject("DisinctMerchantDesc").ToString()%>";
            var ReportFilter_NotGreaterThanToDay = '<%=Resources.MessageManager.ReportFilter_V9%>';
            var invalidUserMsg = '<%=GetLocalResourceObject("InvalidUserAccount").ToString()%>';
            var currentPageURL = "<%=ResolveUrl("~/Sponsor/SponsoredEntityPortfolioSummary.aspx") %>";
        </script>
        <script src="<%= ResolveUrl("~/")%>res/js/sponsor/sponsor-summary.js"></script>
    </as:RadCodeBlock>
</asp:Content>
