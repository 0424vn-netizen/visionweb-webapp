<%@ Page Title="Hierarchy Info" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="HierarchyInformation.aspx.cs"
    Inherits="HierarchyInformation" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:ReportFiltering ID="uxReportFiltering" runat="server" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Hierarchy Information" meta:resourcekey="uxPageTitleResource1" />
    <uc:UxExport ID="uxExportTop" GridID="uxDrilldownGrid" runat="server" IsOnTop="true" />
    <as:ASGrid ID="uxDrilldownGrid" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" AllowAutoCalculateTotalOnExport="false" AllowFilteringByColumn="false"
        ASPagingMethod="SPASingleMethod" ShowPageTotal="false" ShowReportTotal="false" XOverFlowable="false"
        AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxDrilldownGridResource1">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn UniqueName="DrilldownColumn" HeaderText="Entity" HeaderTooltip="Entity"
                    DataField="Entity" SortExpression="Entity" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <%--For Hierarchy mode--%>
                <as:ASGridBoundColumn UniqueName="EntityName" HeaderText="Entity Name" HeaderTooltip="Entity Name"
                    DataField="EntityName" SortExpression="EntityName" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="Contact" HeaderText="Contact" HeaderTooltip="Contact"
                    DataField="Contact" SortExpression="Contact" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="OptedIn" HeaderText="# Opted In" HeaderTooltip="Number Opted In"
                    DataField="OptedIn" SortExpression="OptedIn" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="OptedOut" HeaderText="# Opted Out" HeaderTooltip="Number Opted Out"
                    DataField="OptedOut" SortExpression="OptedOut" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="Closed" HeaderText="# Closed" HeaderTooltip="Number Closed"
                    DataField="Closed" SortExpression="Closed" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="Status" HeaderText="Status" HeaderTooltip="Status"
                    DataField="Status" SortExpression="Status" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <%--For Merchant mode--%>
                <as:ASGridBoundColumn UniqueName="MerchantName" HeaderText="Merchant Name" HeaderTooltip="Merchant Name"
                    DataField="MerchantName" SortExpression="MerchantName" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="Chain" HeaderText="Chain" HeaderTooltip="Chain"
                    DataField="ChainNumber" SortExpression="ChainNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="MerchantStatus" HeaderText="Merchant Status" HeaderTooltip="Merchant Status"
                    DataField="MerchantStatus" SortExpression="MerchantStatus" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource10">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="OpenDate" HeaderText="Open Date" HeaderTooltip="Open Date"
                    DataField="OpenDate" SortExpression="OpenDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="ClosedDate" HeaderText="Closed Date" HeaderTooltip="Closed Date"
                    DataField="ClosedDate" SortExpression="ClosedDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource12">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="OnlineStatus" HeaderText="Online Status" HeaderTooltip="Online Status"
                    DataField="OnlineStatus" SortExpression="OnlineStatus" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
</asp:Content>
