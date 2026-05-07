<%@ Page Title="Worked Summary" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_MgmtReport_WorkedSummary.aspx.cs" Inherits="rm_MCF_MgmtReport_WorkedSummary" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/rm_MCF_MgmtReport_ReportFilter.ascx" TagName="ReportFilter"
    TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportAgent">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportAgent" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxReportGroup">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGroup" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:PlaceHolder ID="dfdj" runat="server">
        <uc:ReportFilter ID="uxReportFilter" runat="server" OnSubmitFiltering="OnSearchEvent"
            VisibleUserGroupFilter="true"></uc:ReportFilter>
        <%--<uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Risk Management - Reports - Worked Summary" />--%>

        <div class="row">
            <div class="col-xs-10" data-toggle="collapse" runat="server" id="uxGridTitle">
                <h2 class="grid-title on-top" runat="server" id="h2GridTitle">
                    <asp:Literal ID="litGridTitle" runat="server" meta:resourcekey="litGridTitleResource1"></asp:Literal>
                    <span class="text-muted">
                        <asp:Literal ID="litGridSubTitle" runat="server" meta:resourcekey="litGridSubTitleResource1"></asp:Literal>
                    </span>
                </h2>
            </div>
            <div class="col-xs-2">
                <div class="report-export dropdown pull-right on-top" runat="server" id="divExport">
                    <a href="#" data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle"
                        id="A1" runat="server">
                        <asp:Literal ID="LiteralExport" runat="server" meta:resourcekey="LiteralExportResource1"> EXPORT</asp:Literal></a>
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
        </div>
        <as:HierarchyGrid ID="uxReportAgent" runat="server" AutoGenerateColumns="false" AllowSorting="true"
            GridLines="None" AllowPaging="true" Visible="false" Width="100%" ShowHeader="true"
            ShowFooter="false" AllowCustomPaging="true" OnNeedDataSource="DoGridNeedDataSource"
            OnDetailTableDataBind="DoDetailTableDataBind" CssClass="wrapper-table in" meta:resourcekey="uxReportAgentResource1">
            <MasterTableView Name="Agent" DataKeyNames="UserID">
                <DetailTables>
                    <tek:GridTableView Name="WorkedDetail" ShowHeader="true" ShowFooter="false" AllowCustomPaging="true" meta:resourcekey="GridTableViewResource1">
                        <Columns>
                            <as:HierarchyBoundColumn UniqueName="ReportDate" HeaderText="Report Date" HeaderTooltip="Report Date"
                                DataField="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="HierarchyBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="AssignmentName" HeaderText="Assignment" HeaderTooltip="Assignment Name"
                                DataField="AssignmentName" ASFormat="DynamicString" meta:resourcekey="HierarchyBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalAssigned" HeaderText="Total Assigned" HeaderTooltip="Total Assigned"
                                DataField="TotalAssigned" ASFormat="Integer" ItemStyle-HorizontalAlign="Right" meta:resourcekey="HierarchyBoundColumnResource3">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalWorked" HeaderText="Total Worked" HeaderTooltip="Total Worked"
                                DataField="TotalWorked" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource4">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalWorkedPrior" HeaderText="Total Worked Prior To Date Range"
                                HeaderTooltip="Total Worked Prior To Date Range" DataField="TotalWorkedPrior"
                                ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource5">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="PercentWorked" HeaderText="% Worked" HeaderTooltip="% Worked"
                                DataField="PercentWorked" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource6">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="CasesClosed" HeaderText="Cases Closed" HeaderTooltip="Cases Closed"
                                DataField="CasesClosed" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource7">
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
                            <as:HierarchyBoundColumn UniqueName="PercentPending" HeaderText="% Pending" HeaderTooltip="% Pending"
                                DataField="PercentPending" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource9">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalPending" HeaderText="Total Pending" HeaderTooltip="Total Pending"
                                DataField="TotalPending" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource10">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                        </Columns>
                    </tek:GridTableView>
                </DetailTables>
                <Columns>
                    <as:HierarchyBoundColumn UniqueName="UserFullName" HeaderText="Agent" HeaderTooltip="Agent"
                        DataField="UserFullName" ASFormat="DynamicString" ItemStyle-Font-Bold="true" meta:resourcekey="HierarchyBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Font-Bold="True"></ItemStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="TotalAssigned" HeaderText="Total Assigned" HeaderTooltip="Total Assigned"
                        DataField="TotalAssigned" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="TotalWorked" HeaderText="Total Worked" HeaderTooltip="Total Worked"
                        DataField="TotalWorked" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="TotalWorkedPrior" HeaderText="Total Worked Prior To Date Range"
                        HeaderTooltip="Total Worked Prior To Date Range" DataField="TotalWorkedPrior"
                        ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="PercentWorked" HeaderText="% Worked" HeaderTooltip="% Worked"
                        DataField="PercentWorked" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="CasesClosed" HeaderText="Cases Closed" HeaderTooltip="Cases Closed"
                        DataField="CasesClosed" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="CasesOpened" HeaderText="Cases Opened" HeaderTooltip="Cases Opened"
                        DataField="CasesOpened" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="PercentPending" HeaderText="% Pending" HeaderTooltip="% Pending"
                        DataField="PercentPending" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="TotalPending" HeaderText="Total Pending" HeaderTooltip="Total Pending"
                        DataField="TotalPending" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                </Columns>
            </MasterTableView>
        </as:HierarchyGrid>
        <as:HierarchyGrid ID="uxReportGroup" runat="server" AutoGenerateColumns="false" AllowSorting="true"
            GridLines="None" AllowPaging="true" Visible="false" Width="100%" ShowHeader="true"
            ShowFooter="false" AllowCustomPaging="true" OnNeedDataSource="DoGridNeedDataSource"
            OnDetailTableDataBind="DoDetailTableDataBind" CssClass="wrapper-table in" meta:resourcekey="uxReportGroupResource1">
            <MasterTableView Name="Group" DataKeyNames="GroupID">
                <DetailTables>
                    <tek:GridTableView Name="Agent" DataKeyNames="UserID" ShowHeader="true" ShowFooter="false"
                        AllowCustomPaging="true" meta:resourcekey="GridTableViewResource3">
                        <DetailTables>
                            <tek:GridTableView Name="WorkedDetail" ShowHeader="true" ShowFooter="false" AllowCustomPaging="true" meta:resourcekey="GridTableViewResource2">
                                <Columns>
                                    <as:HierarchyBoundColumn SortExpression="ReportDate" UniqueName="ReportDate" HeaderText="Report Date"
                                        HeaderTooltip="Report Date" DataField="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="HierarchyBoundColumnResource20">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn SortExpression="AssignmentName" UniqueName="AssignmentName"
                                        HeaderText="Assignment" HeaderTooltip="Assignment Name" DataField="AssignmentName"
                                        ASFormat="DynamicString" meta:resourcekey="HierarchyBoundColumnResource21">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="TotalAssigned" HeaderText="Total Assigned" HeaderTooltip="Total Assigned"
                                        DataField="TotalAssigned" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource22">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="TotalWorked" HeaderText="Total Worked" HeaderTooltip="Total Worked"
                                        DataField="TotalWorked" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource23">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="TotalWorkedPrior" HeaderText="Total Worked Prior To Date Range"
                                        HeaderTooltip="Total Worked Prior To Date Range" DataField="TotalWorkedPrior"
                                        ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource24">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="PercentWorked" HeaderText="% Worked" HeaderTooltip="% Worked"
                                        DataField="PercentWorked" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource25">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="CasesClosed" HeaderText="Cases Closed" HeaderTooltip="Cases Closed"
                                        DataField="CasesClosed" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource26">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="CasesOpened" HeaderText="Cases Opened" HeaderTooltip="Cases Opened"
                                        DataField="CasesOpened" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource27">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="PercentPending" HeaderText="% Pending" HeaderTooltip="% Pending"
                                        DataField="PercentPending" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource28">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="TotalPending" HeaderText="Total Pending" HeaderTooltip="Total Pending"
                                        DataField="TotalPending" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource29">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                </Columns>
                            </tek:GridTableView>
                        </DetailTables>
                        <Columns>
                            <as:HierarchyBoundColumn UniqueName="UserFullName" HeaderText="Agent" HeaderTooltip="Agent"
                                DataField="UserFullName" ASFormat="DynamicString" ItemStyle-Font-Bold="true" meta:resourcekey="HierarchyBoundColumnResource30">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle Font-Bold="True"></ItemStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalAssigned" HeaderText="Total Assigned" HeaderTooltip="Total Assigned"
                                DataField="TotalAssigned" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource31">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalWorked" HeaderText="Total Worked" HeaderTooltip="Total Worked"
                                DataField="TotalWorked" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource32">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalWorkedPrior" HeaderText="Total Worked Prior To Date Range"
                                HeaderTooltip="Total Worked Prior To Date Range" DataField="TotalWorkedPrior"
                                ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource33">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="PercentWorked" HeaderText="% Worked" HeaderTooltip="% Worked"
                                DataField="PercentWorked" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource34">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="CasesClosed" HeaderText="Cases Closed" HeaderTooltip="Cases Closed"
                                DataField="CasesClosed" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource35">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="CasesOpened" HeaderText="Cases Opened" HeaderTooltip="Cases Opened"
                                DataField="CasesOpened" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource36">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="PercentPending" HeaderText="% Pending" HeaderTooltip="% Pending"
                                DataField="PercentPending" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource37">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalPending" HeaderText="Total Pending" HeaderTooltip="Total Pending"
                                DataField="TotalPending" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource38">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                        </Columns>
                    </tek:GridTableView>
                </DetailTables>
                <Columns>
                    <as:HierarchyBoundColumn UniqueName="GroupName" HeaderText="Group" HeaderTooltip="Group"
                        DataField="GroupName" ASFormat="DynamicString" ItemStyle-Font-Bold="true" meta:resourcekey="HierarchyBoundColumnResource39">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Font-Bold="True"></ItemStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="TotalAssigned" HeaderText="Total Assigned" HeaderTooltip="Total Assigned"
                        DataField="TotalAssigned" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource40">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="TotalWorked" HeaderText="Total Worked" HeaderTooltip="Total Worked"
                        DataField="TotalWorked" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource41">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="TotalWorkedPrior" HeaderText="Total Worked Prior To Date Range"
                        HeaderTooltip="Total Worked Prior To Date Range" DataField="TotalWorkedPrior"
                        ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource42">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="PercentWorked" HeaderText="% Worked" HeaderTooltip="% Worked"
                        DataField="PercentWorked" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource43">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="CasesClosed" HeaderText="Cases Closed" HeaderTooltip="Cases Closed"
                        DataField="CasesClosed" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource44">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="CasesOpened" HeaderText="Cases Opened" HeaderTooltip="Cases Opened"
                        DataField="CasesOpened" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource45">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="PercentPending" HeaderText="% Pending" HeaderTooltip="% Pending"
                        DataField="PercentPending" ASFormat="Percentage" meta:resourcekey="HierarchyBoundColumnResource46">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="TotalPending" HeaderText="Total Pending" HeaderTooltip="Total Pending"
                        DataField="TotalPending" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource47">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:HierarchyBoundColumn>
                </Columns>
            </MasterTableView>
        </as:HierarchyGrid>
    </as:PlaceHolder>
</asp:Content>
