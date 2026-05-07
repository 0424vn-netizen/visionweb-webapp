<%@ Page Title="Assignment Volumne Summary" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_MgmtReport_AssignmentVolume.aspx.cs" Inherits="rm_MCF_MgmtReport_AssignmentVolume" meta:resourcekey="PageResource1" %>

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
        VisibleAssignmentFilter="true"></uc:ReportFilter>
    <%--<uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Risk Management - Reports - Assignment Volume Summary" />--%>
    <as:PlaceHolder ID="fgfg" runat="server">
        <div class="row">
            <div class="col-xs-10">
                <h2 class="grid-title on-top" data-toggle="collapse" data-target="#ciAssignmentVolumeSummaryGrid">
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
                            <asp:Literal ID="LiteralExport" runat="server" meta:resourcekey="LiteralExportResource1">EXPORT</asp:Literal>
                        </a>
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
        <div class="in" id="ciAssignmentVolumeSummaryGrid">
            <as:HierarchyGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" AllowSorting="false"
                GridLines="None" AllowPaging="true" ShowHeader="false"
                ShowFooter="false" AllowCustomPaging="true" OnNeedDataSource="DoGridNeedDataSource"
                OnItemDataBound="DoItemDataBound" OnDetailTableDataBind="DoDetailTableDataBind"
                CssClass="wrapper-table in" meta:resourcekey="uxReportGridResource1">
                <MasterTableView Name="DateView" DataKeyNames="ReportDate" TableLayout="Auto">
                    <DetailTables>
                        <tek:GridTableView Name="DetailView" ShowHeader="true" ShowFooter="false" AllowCustomPaging="true"
                            AllowSorting="true" meta:resourcekey="GridTableViewResource1">
                            <Columns>
                                <as:HierarchyBoundColumn UniqueName="AssignmentID" HeaderText="AssignmentID" DataField="AssignmentID"
                                    Visible="false" meta:resourcekey="HierarchyBoundColumnResource1">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:HierarchyBoundColumn>
                                <as:HierarchyBoundColumn UniqueName="AssignmentName" HeaderText="Assignment" HeaderTooltip="Assignment Name"
                                    DataField="AssignmentName" ASFormat="DynamicString" meta:resourcekey="HierarchyBoundColumnResource2">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:HierarchyBoundColumn>
                                <as:HierarchyBoundColumn UniqueName="TotalAssigned" HeaderText="Total Assigned" HeaderTooltip="Total Assigned"
                                    DataField="TotalAssigned" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource3">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:HierarchyBoundColumn>
                                <as:HierarchyBoundColumn UniqueName="TotalWorked" HeaderText="Total Worked" HeaderTooltip="Total Worked"
                                    DataField="TotalWorked" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource4">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:HierarchyBoundColumn>
                                <as:HierarchyBoundColumn UniqueName="PercentWorked" HeaderText="% Worked" HeaderTooltip="% Worked"
                                    DataField="PercentWorked" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource5">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:HierarchyBoundColumn>
                                <as:HierarchyBoundColumn UniqueName="CasesClosed" HeaderText="Cases Closed" HeaderTooltip="Cases Closed"
                                    DataField="CasesClosed" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource6">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:HierarchyBoundColumn>
                                <as:HierarchyBoundColumn UniqueName="PercentCasesClosed" HeaderText="% Closed" HeaderTooltip="% Closed"
                                    DataField="PercentCasesClosed" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource7">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:HierarchyBoundColumn>
                                <as:HierarchyBoundColumn UniqueName="CasesOpened" HeaderText="Cases Opened" HeaderTooltip="Cases Opened"
                                    DataField="CasesOpened" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource8">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:HierarchyBoundColumn>
                                <as:HierarchyBoundColumn UniqueName="PercentCasesOpened" HeaderText="% Opened" HeaderTooltip="% Opened"
                                    DataField="PercentCasesOpened" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource9">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:HierarchyBoundColumn>
                                <as:HierarchyBoundColumn UniqueName="PercentNotWorked" HeaderText="% Pending" HeaderTooltip="% Pending"
                                    DataField="PercentNotWorked" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource10">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:HierarchyBoundColumn>
                                <as:HierarchyBoundColumn UniqueName="TotalPending" HeaderText="Total Pending" HeaderTooltip="Total Pending"
                                    DataField="TotalPending" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource11">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:HierarchyBoundColumn>
                            </Columns>
                        </tek:GridTableView>
                    </DetailTables>
                    <Columns>
                        <as:HierarchyBoundColumn UniqueName="ReportDate" HeaderText="" DataField="ReportDate"
                            ItemStyle-HorizontalAlign="left" meta:resourcekey="HierarchyBoundColumnResource12">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </as:HierarchyBoundColumn>
                        <as:HierarchyBoundColumn UniqueName="TotalAssigned" HeaderText="" HeaderTooltip="Assignment Volume"
                            DataField="TotalAssigned"
                            ItemStyle-HorizontalAlign="left" meta:resourcekey="HierarchyBoundColumnResource13">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </as:HierarchyBoundColumn>
                        <as:HierarchyBoundColumn UniqueName="PercentWorked" HeaderText="" HeaderTooltip="Worked"
                            DataField="PercentWorked" ASFormat="Percentage" ItemStyle-HorizontalAlign="left" meta:resourcekey="HierarchyBoundColumnResource14">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </as:HierarchyBoundColumn>
                        <as:HierarchyBoundColumn UniqueName="PercentNotWorked" HeaderText="" HeaderTooltip="Not Worked"
                            DataField="PercentNotWorked" ASFormat="Percentage" ItemStyle-HorizontalAlign="left" meta:resourcekey="HierarchyBoundColumnResource15">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </as:HierarchyBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:HierarchyGrid>
        </div>

    </as:PlaceHolder>
</asp:Content>

