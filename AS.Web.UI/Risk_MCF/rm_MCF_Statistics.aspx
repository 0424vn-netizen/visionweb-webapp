<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="rm_MCF_Statistics.aspx.cs" Inherits="rm_MCF_Statistics" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="AdvancedFilter" Src="~/UserControls/UxAdvancedFilter.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxSatisticsReport">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxSatisticsReport" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting> 
            <tek:AjaxSetting AjaxControlID="uxDelete">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxSatisticsReport" LoadingPanelID="uxLoadingPanelCustom" />
                    <tek:AjaxUpdatedControl ControlID="hddTotalRecords"  />
                </UpdatedControls>
            </tek:AjaxSetting> 
             <tek:AjaxSetting AjaxControlID="uxDownloadReport">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxSatisticsReport" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="btnRefresh">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxSatisticsReport"  />
                    <tek:AjaxUpdatedControl ControlID="hddTotalRecords"  />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
        
    </tek:RadAjaxManagerProxy>
    <div class="main-content">
        <uc:AdvancedFilter ID="uxAdvancedFilter" runat="server" />
        <%-- ReportTitle --%>
        <div class="row">
            <div class="col-md-12 no-margin-bottom">
                <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Statistics" HasFilteringOption="false" meta:resourcekey="ReportTitle" />
                <div class="lbltext-advanced-satatistics">
                    <as:LinkButton runat="server" ID="btnAdvancedFilter" OnClientClick="statistics.statisticsFilter.show(); return false;"
                        Text="Advanced Filter" meta:resourcekey="AdvancedFilter"></as:LinkButton>
                </div>
            </div>
        </div>
        <div class="height-25"></div>
        <as:ASGrid ID="uxSatisticsReport" runat="server" AutoGenerateColumns="false" HeaderStyle-Width="80px"
            ASPagingMethod="SPAMultiMethod" AllowSorting="true" AllowPaging="false" IsAutoExportTemplate="true"
            OnNeedDataSource="uxSatisticsReport_NeedDataSource" OnItemDataBound="uxSatisticsReport_DataBound" auto
            ShowToolTip="true" CssClass="in" meta:resourcekey="uxSatisticsReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridTemplateColumn HeaderText="Report Type" DataField="ReportType"
                        UniqueName="ReportType" HeaderStyle-Width="238px" HeaderStyle-HorizontalAlign="Center"
                        SortExpression="ReportType" HeaderTooltip="Report Type"
                        meta:resourcekey="HierarchyTemplateColumnResource1">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" OnClientClick='<%# "doDowloadFile(\""+Eval("DocumentID")+"\",\""+Eval("FileName")+"\")"%>'
                                ID="uxDownloadReport" Text='<%# Eval("ReportType") %>'></asp:LinkButton>
                            <asp:Literal runat="server" ID="uxLitReportType" Text='<%#Eval("ReportType") %>'></asp:Literal>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>

                    <as:ASGridBoundColumn UniqueName="DateTime" HeaderText="Date Time" DataField="CreatedDate"
                        HeaderStyle-Width="236px"
                        HeaderTooltip="Date Time" ItemStyle-HorizontalAlign="Center" ASFormat="DateAndTime12Hours"
                        HeaderStyle-HorizontalAlign="Center" meta:resourcekey="HierarchyTemplateColumnResource2">
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Status" DataField="Status" UniqueName="Status" Visible="false">
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Status" DataField="StatusDesc" UniqueName="StatusDesc"
                        HeaderStyle-Width="156px"
                        ASFormat="StaticString" SortExpression="StatusDesc" HeaderTooltip="Status"
                        meta:resourcekey="HierarchyTemplateColumnResource3">
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Configuration" DataField="Configuration" UniqueName="Configuration"
                        HeaderStyle-Width="316px" meta:resourcekey="HierarchyTemplateColumnResource4" ASDefaultNullValue="—" ASFormat="DynamicString">
                    </as:ASGridBoundColumn>

                    <as:ASGridTemplateColumn UniqueName="Delete" ItemStyle-HorizontalAlign="Center" HeaderText="Delete"
                        HeaderStyle-Width="200px"
                        HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Delete" meta:resourcekey="HierarchyTemplateColumnResource5">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="uxDelete" Text="Delete" CommandArgument='<%#Eval("RecordID") + "," + Eval("DocumentID")%>' OnCommand="DeleteFileExport" OnClientClick="return ConfirmDelete();" meta:resourcekey="LinkButton2Resource1"></asp:LinkButton>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <as:LinkButton runat="server" ID="btnRefresh" CssClass="hide" OnClick="btnRefresh_Click" Text="Refresh"></as:LinkButton>
        <asp:HiddenField runat="server" ID="uxDocId" />
        <asp:HiddenField runat="server" ID="uxFileName" />
        <asp:HiddenField runat="server" ID="hddTotalRecords" />
        <as:Button runat="server" ID="uxbtnDownload" CssClass="display-none" OnClientClick="return validateDocId();" OnClick="uxbtnDownload_Click" />
        <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
            <script type="text/javascript">
                var rm_MgmtReport_Extracts_ConfirmDelete = "<%=Resources.MessageManager.ConfirmDeleteFile%>";
                var statistics = {
                    statisticsFilter: new advancedFilter.create({
                        sectionName: '<%= FilterPageEnums.PortfolioStatistics %>',
                        hddDateAvailableFilter: '<%= _FilterDate.ToString() %>',
                        btnRefresh : '<%= btnRefresh.ClientID %>',
                        totalRecords: '<%= hddTotalRecords.ClientID %>',
                    }),
                };

                var uxhdDocId = '<%= uxDocId.ClientID%>';
                var uxhdFileName = '<%= uxFileName.ClientID%>';
                var uxbtnDownload = '<%= uxbtnDownload.ClientID%>';
                var hddTotalRecords = '<%= hddTotalRecords.ClientID%>';
            </script>
            <script src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_Statistics.js"></script>
        </as:ASRadCodeBlock>
    </div>
</asp:Content>



