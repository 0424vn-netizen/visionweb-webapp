<%@ Page Title="Worked Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_MgmtReport_WorkedDetail.aspx.cs" Inherits="rm_MCF_MgmtReport_WorkedDetail" meta:resourcekey="PageResource1" %>

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
    <as:PlaceHolder ID="derr" runat="server">
        <uc:ReportFilter ID="uxReportFilter" runat="server" OnSubmitFiltering="OnSearchEvent"
            VisibleUserGroupFilter="true"></uc:ReportFilter>
        <%-- <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Risk Management - Reports - Worked Detail" />  --%>

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
            GridLines="None" AllowPaging="true" Visible="false" Width="100%" ShowHeader="false"
            CssClass="wrapper-table in"
            ShowFooter="false" AllowCustomPaging="true" OnNeedDataSource="DoGridNeedDataSource"
            OnItemDataBound="DoItemDataBound" OnDetailTableDataBind="DoDetailTableDataBind"
            OnItemCommand="DoItemCommand" OnDataBound="DoDataBound" meta:resourcekey="uxReportAgentResource1">
            <MasterTableView Name="Agent" DataKeyNames="UserID">
                <DetailTables>
                    <tek:GridTableView Name="WorkedDetail" ShowHeader="true" ShowFooter="false" AllowCustomPaging="true" meta:resourcekey="GridTableViewResource1">
                        <Columns>
                            <as:HierarchyBoundColumn UniqueName="WorkedOn" DataField="WorkedOn" HeaderText="Worked On"
                                HeaderTooltip="Worked On" ASFormat="DateAndTime12Hours" HeaderStyle-Width="150px" meta:resourcekey="HierarchyBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyTemplateColumn SortExpression="MerchantNumber" UniqueName="MerchantNumber"
                                HeaderText="Merchant ID" HeaderTooltip="Merchant ID" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="140px" meta:resourcekey="HierarchyTemplateColumnResource1">
                                <ItemTemplate>
                                    <as:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("MerchantNumber") %>'
                                        OnCommand="uxMerchant_Command" Text='<%# Eval("MerchantNumber") %>' CommandName="MerchantNumberClick" meta:resourcekey="LinkButton1Resource1"></as:LinkButton>
                                </ItemTemplate>

                                <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </as:HierarchyTemplateColumn>
                            <as:HierarchyTemplateColumn SortExpression="MerchantName" UniqueName="MerchantName"
                                HeaderText="Merchant Name" HeaderTooltip="Merchant Name" ItemStyle-HorizontalAlign="Left" meta:resourcekey="HierarchyTemplateColumnResource2">
                                <ItemTemplate>
                                    <as:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("MerchantNumber") %>'
                                        OnCommand="uxMerchant_Command" Text='<%# Eval("MerchantName") %>' CommandName="MerchantNameClick" meta:resourcekey="LinkButton2Resource1"></as:LinkButton>
                                </ItemTemplate>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </as:HierarchyTemplateColumn>
                            <as:HierarchyBoundColumn UniqueName="CaseNumber" DataField="CaseNumber" HeaderText="Case #"
                                HeaderTooltip="Case Number" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="InvestigationOpenedOn" DataField="InvestigationOpenedOn"
                                HeaderText="Investigation Opened On" HeaderTooltip="Investigation Opened On"
                                ASFormat="DateAndTime12Hours" meta:resourcekey="HierarchyBoundColumnResource3">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="StatusDesc" DataField="StatusDesc" HeaderText="Status"
                                HeaderTooltip="Status" HeaderStyle-Width="80px" meta:resourcekey="HierarchyBoundColumnResource4">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="InvestigationClosedOn" DataField="InvestigationClosedOn"
                                HeaderText="Investigation Closed On" HeaderTooltip="Investigation Closed On"
                                ASFormat="DateAndTime12Hours" HeaderStyle-Width="150px" meta:resourcekey="HierarchyBoundColumnResource5">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="ResolutionDesc" DataField="ResolutionDesc" HeaderText="Resolution"
                                HeaderTooltip="Resolution" ASFormat="DynamicString" meta:resourcekey="HierarchyBoundColumnResource6">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:HierarchyBoundColumn>
                        </Columns>
                    </tek:GridTableView>
                </DetailTables>
                <Columns>
                    <as:HierarchyBoundColumn UniqueName="FullName" HeaderText="" DataField="FullName"
                        ASFormat="DynamicString" ItemStyle-Font-Bold="true" meta:resourcekey="HierarchyBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Font-Bold="True"></ItemStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="TotalWorked" HeaderText="" DataField="TotalWorked"
                        ASFormat="Integer" ItemStyle-HorizontalAlign="Left" meta:resourcekey="HierarchyBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </as:HierarchyBoundColumn>
                </Columns>
            </MasterTableView>
        </as:HierarchyGrid>
        <as:HierarchyGrid ID="uxReportGroup" runat="server" AutoGenerateColumns="false" AllowSorting="true"
            GridLines="None" AllowPaging="true" Visible="false" Width="100%" ShowHeader="false"
            CssClass="wrapper-table in"
            OnNeedDataSource="DoGridNeedDataSource" ShowFooter="false" AllowCustomPaging="true"
            OnItemDataBound="DoItemDataBound" OnDetailTableDataBind="DoDetailTableDataBind"
            OnItemCommand="DoItemCommand" OnDataBound="DoDataBound" meta:resourcekey="uxReportGroupResource1">
            <MasterTableView Name="Group" DataKeyNames="GroupID">
                <DetailTables>
                    <tek:GridTableView Name="Agent" DataKeyNames="UserID" ShowHeader="false" ShowFooter="false"
                        AllowCustomPaging="true" meta:resourcekey="GridTableViewResource3">
                        <DetailTables>
                            <tek:GridTableView Name="WorkedDetail" ShowHeader="true" ShowFooter="false" AllowCustomPaging="true"
                                AllowSorting="true" meta:resourcekey="GridTableViewResource2">
                                <Columns>
                                    <as:HierarchyBoundColumn UniqueName="WorkedOn" DataField="WorkedOn" HeaderText="Worked On"
                                        HeaderTooltip="Worked On" ASFormat="DateAndTime12Hours" meta:resourcekey="HierarchyBoundColumnResource9">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyTemplateColumn SortExpression="MerchantNumber" UniqueName="MerchantNumber"
                                        HeaderText="Merchant ID" HeaderTooltip="Merchant ID" ItemStyle-HorizontalAlign="Center" meta:resourcekey="HierarchyTemplateColumnResource3">
                                        <ItemTemplate>
                                            <as:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# Eval("MerchantNumber") %>'
                                                OnCommand="uxMerchant_Command" Text='<%# Eval("MerchantNumber") %>'
                                                CommandName="MerchantNumberClick" meta:resourcekey="LinkButton3Resource1"></as:LinkButton>
                                        </ItemTemplate>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </as:HierarchyTemplateColumn>
                                    <as:HierarchyTemplateColumn SortExpression="MerchantName" UniqueName="MerchantName"
                                        HeaderText="Merchant Name" HeaderTooltip="Merchant Name"
                                        ItemStyle-HorizontalAlign="Left" meta:resourcekey="HierarchyTemplateColumnResource4">
                                        <ItemTemplate>
                                            <as:LinkButton ID="LinkButton4" runat="server" CommandArgument='<%# Eval("MerchantNumber") %>'
                                                OnCommand="uxMerchant_Command" Text='<%# Eval("MerchantName") %>' CommandName="MerchantNameClick" meta:resourcekey="LinkButton4Resource1"></as:LinkButton>
                                        </ItemTemplate>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </as:HierarchyTemplateColumn>
                                    <as:HierarchyBoundColumn UniqueName="CaseNumber" DataField="CaseNumber" HeaderText="Case #"
                                        HeaderTooltip="Case Number" ASFormat="Integer" meta:resourcekey="HierarchyBoundColumnResource10">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="InvestigationOpenedOn" DataField="InvestigationOpenedOn"
                                        HeaderText="Investigation Opened On" HeaderTooltip="Investigation Opened On"
                                        ASFormat="DateAndTime12Hours" meta:resourcekey="HierarchyBoundColumnResource11">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="StatusDesc" DataField="StatusDesc"
                                        HeaderText="Status" HeaderTooltip="Status" meta:resourcekey="HierarchyBoundColumnResource12">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="InvestigationClosedOn" DataField="InvestigationClosedOn"
                                        HeaderText="Investigation Closed On" HeaderTooltip="Investigation Closed On"
                                        ASFormat="DateAndTime12Hours" meta:resourcekey="HierarchyBoundColumnResource13">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                    <as:HierarchyBoundColumn UniqueName="ResolutionDesc" DataField="ResolutionDesc"
                                        HeaderText="Resolution" HeaderTooltip="Resolution" ASFormat="DynamicString" meta:resourcekey="HierarchyBoundColumnResource14">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text=""></ModelErrorMessage>
                                        </ColumnValidationSettings>

                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </as:HierarchyBoundColumn>
                                </Columns>
                            </tek:GridTableView>
                        </DetailTables>
                        <Columns>
                            <as:HierarchyBoundColumn UniqueName="FullName" HeaderText="" DataField="FullName"
                                ASFormat="DynamicString" ItemStyle-Font-Bold="true" meta:resourcekey="HierarchyBoundColumnResource15">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle Font-Bold="True"></ItemStyle>
                            </as:HierarchyBoundColumn>
                            <as:HierarchyBoundColumn UniqueName="TotalWorked" HeaderText="" DataField="TotalWorked"
                                ASFormat="Integer" ItemStyle-HorizontalAlign="Left" meta:resourcekey="HierarchyBoundColumnResource16">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </as:HierarchyBoundColumn>
                        </Columns>
                    </tek:GridTableView>
                </DetailTables>
                <Columns>
                    <as:HierarchyBoundColumn UniqueName="GroupName" HeaderText="" DataField="GroupName"
                        ItemStyle-Font-Bold="true" ASFormat="DynamicString" meta:resourcekey="HierarchyBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Font-Bold="True"></ItemStyle>
                    </as:HierarchyBoundColumn>
                    <as:HierarchyBoundColumn UniqueName="TotalWorked" HeaderText="" DataField="TotalWorked"
                        ItemStyle-HorizontalAlign="Left" meta:resourcekey="HierarchyBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </as:HierarchyBoundColumn>
                </Columns>
            </MasterTableView>
        </as:HierarchyGrid>
    </as:PlaceHolder>
</asp:Content>

