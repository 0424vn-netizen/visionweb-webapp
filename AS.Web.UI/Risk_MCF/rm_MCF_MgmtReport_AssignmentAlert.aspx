<%@ Page Title="Assignment Alert Summary" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_MgmtReport_AssignmentAlert.aspx.cs" Inherits="rm_MCF_MgmtReport_AssignmentAlert" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_MgmtReport_ReportFilter.ascx" TagName="ReportFilter"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <uc:ReportFilter ID="uxReportFilter" runat="server" OnSubmitFiltering="OnSearchEvent"
        VisibleAssignmentFilter="true" VisibleParameterFilter="true"></uc:ReportFilter>
    <%--<uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Risk Management - Reports - Assignment Alert Summary" />--%>
    <div class="row">
        <div class="col-xs-10">
            <h2 class="grid-title on-top" data-toggle="collapse" data-target="#ciAssignmentAlertSummaryGrid">
                <as:Literal ID="litGridHeader" runat="server" meta:resourcekey="litGridHeaderResource1" />
                <span class="text-muted">
                    <asp:Literal ID="litGridSubTitle" runat="server" meta:resourcekey="litGridSubTitleResource1"></asp:Literal>
                </span>
            </h2>
        </div>
        <as:PlaceHolder ID="uxExporter" runat="server">
            <div class="col-xs-2">
                <div class="report-export dropdown pull-right on-top" runat="server" id="divExport">
                    <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle"
                        id="A1" runat="server">
                        <asp:Literal ID="LiteralExport" runat="server" meta:resourcekey="LiteralExportResource1"> EXPORT </asp:Literal></a>
                    <ul class="dropdown-menu">
                        <li id="uxLiExcel" runat="server">
                            <asp:LinkButton ID="imgExcel" runat="server" OnClick="ButtonExcel_Click" meta:resourcekey="imgExcelResource1">Excel</asp:LinkButton>
                        </li>
                        <li id="uxLiCSV" runat="server">
                            <asp:LinkButton ID="imgCSV" runat="server" OnClick="ButtonCSV_Click" meta:resourcekey="imgCSVResource1">CSV</asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>
        </as:PlaceHolder>
    </div>
    <div class="in" id="ciAssignmentAlertSummaryGrid">
        <as:HierarchyGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowSorting="false"
            GridLines="None" AllowPaging="true" ShowHeader="false"
            ShowFooter="false" AllowCustomPaging="true" OnNeedDataSource="DoGridNeedDataSource" OnPreRender="uxReportGrid_PreRender"
            OnItemDataBound="DoItemDataBound" OnDetailTableDataBind="DoDetailTableDataBind"
            CssClass="wrapper-table in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView Name="DateView" DataKeyNames="ReportDate" ExpandCollapseColumn-HeaderStyle-Width="28px"
                TableLayout="Auto">
                <DetailTables>
                    <tek:GridTableView Name="Assignments" DataKeyNames="ReportDate,AssignmentID" ShowHeader="true"
                        ShowFooter="false" AllowCustomPaging="true" TableLayout="Auto" AllowSorting="true" meta:resourcekey="GridTableViewResource2">
                        <DetailTables>
                            <tek:GridTableView Name="Parameters" ShowHeadersWhenNoRecords="true"
                                ShowHeader="true" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true" meta:resourcekey="GridTableViewResource1">
                                <Columns>
                                    <as:HierarchyBoundColumn SortExpression="ParameterKey" UniqueName="ParameterKey"
                                        HeaderText="P #" HeaderTooltip="Parameter Key" DataField="ParameterKey" ASFormat="StaticString" meta:resourcekey="HierarchyBoundColumnResource1">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn SortExpression="ParameterName" UniqueName="ParameterName"
                                        HeaderText="Parameter Name" HeaderTooltip="Parameter Name" DataField="ParameterName"
                                        ASFormat="DynamicString" meta:resourcekey="HierarchyBoundColumnResource2">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn SortExpression="ParameterIndicator" UniqueName="ParameterIndicator"
                                        HeaderText="P %/#/$" HeaderTooltip="P %/#/$" DataField="ParameterIndicator"
                                        ItemStyle-HorizontalAlign="Right" meta:resourcekey="HierarchyBoundColumnResource3">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn SortExpression="ParameterThreshold" UniqueName="ParameterThreshold"
                                        HeaderText="P TH" HeaderTooltip="Parameter Threshold" DataField="ParameterThreshold"
                                        ItemStyle-HorizontalAlign="Right" meta:resourcekey="HierarchyBoundColumnResource4">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="TotalAssigned" HeaderText="Total Violations"
                                        HeaderTooltip="Total Violations" DataField="TotalAssigned" SortExpression="TotalAssigned"
                                        ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource5">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                </Columns>
                            </tek:GridTableView>
                        </DetailTables>
                        <Columns>
                            <as:HierarchyBoundColumn SortExpression="AssignmentName" UniqueName="AssignmentName"
                                HeaderText="Assignment" HeaderTooltip="Assignment Name" DataField="AssignmentName"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" meta:resourcekey="HierarchyBoundColumnResource6">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalAssigned" HeaderText="Total Violations"
                                HeaderTooltip="Total Violations" DataField="TotalAssigned" SortExpression="TotalAssigned"
                                ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource7">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalDistinctAssigned" HeaderText="Distinct # of Merchants"
                                HeaderTooltip="Distinct # of Merchants" DataField="TotalDistinctAssigned" SortExpression="TotalDistinctAssigned"
                                ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource8">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalWorked" HeaderText="Total Merchants Worked"
                                HeaderTooltip="Total Merchants Worked" DataField="TotalWorked" SortExpression="TotalWorked"
                                ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource9">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="PercentWorked" HeaderText="% Worked" HeaderTooltip="% Worked"
                                DataField="PercentWorked" SortExpression="PercentWorked" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource10">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="PercentNotWorked" HeaderText="% Pending" HeaderTooltip="% Pending"
                                DataField="PercentNotWorked" SortExpression="PercentNotWorked" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource11">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                        </Columns>
                    </tek:GridTableView>
                </DetailTables>

                <ExpandCollapseColumn>
                    <HeaderStyle Width="28px"></HeaderStyle>
                </ExpandCollapseColumn>
                <Columns>
                    <as:HierarchyBoundColumn UniqueName="ReportDate" HeaderText="" DataField="ReportDate"
                        ASFormat="Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="99%" meta:resourcekey="HierarchyBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="99%"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </as:HierarchyBoundColumn>
                </Columns>
            </MasterTableView>
        </as:HierarchyGrid>
    </div>
</asp:Content>

