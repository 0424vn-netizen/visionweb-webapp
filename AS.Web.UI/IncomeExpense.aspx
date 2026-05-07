<%@ Page Title="Income/Expense Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="IncomeExpense.aspx.cs" Inherits="IncomeExpense" meta:resourcekey="PageResource1" %>

<%@ Register TagName="IncomeFiltering" Src="~/UserControls/IncomeFiltering.ascx" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:IncomeFiltering ID="uxReportFiltering" runat="server" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true" HasFilteringOption="true" />
    <tek:RadTabStrip ID="uxTabView" runat="server" meta:resourcekey="uxTabViewResource1" CssClass="as-animated-tabstrip">
        <Tabs>
            <tek:RadTab Text="By Entity" Value="ByEntity" Selected="true" meta:resourcekey="RadTabResource1">
            </tek:RadTab>
            <tek:RadTab Text="By Period" Value="ByPeriod" meta:resourcekey="RadTabResource2">
            </tek:RadTab>
            <tek:RadTab Text="By Merchant" Value="ByMerchant" meta:resourcekey="RadTabResource3">
            </tek:RadTab>
        </Tabs>
    </tek:RadTabStrip>
    <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxDrilldownGrid" OnNeedExportConfig="DoNeedExportConfig" />
    <as:ASGrid ID="uxDrilldownGrid" runat="server" AllowPaging="True" AllowSorting="True" HeaderStyle-Width="110" IsAutoExportTemplate="true"
        AutoGenerateColumns="False" AllowAutoCalculateTotalOnExport="false" AllowFilteringByColumn="false" AppendHeaderforPrinter="true"
        ASPagingMethod="SPASingleMethod" ShowPageTotal="false" ShowReportTotal="true" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxDrilldownGridResource1">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Entity" DataField="Entity" UniqueName="DrilldownColumn" 
                    SortExpression="Entity" HeaderTooltip="Entity" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="EntityName" HeaderText="Entity Name" HeaderTooltip="Entity Name"
                    DataField="EntityName" SortExpression="EntityName" ASFormat="DynamicString" Visible="false" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="Month" HeaderText="Month" HeaderTooltip="Month"
                    DataField="Entity" SortExpression="EntitySorting" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Open Date" HeaderTooltip="Open Date" DataField="OpenDate"
                    UniqueName="OpenDate" ASFormat="Date" SortExpression="OpenDate" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="MCC" DataField="SICcode" UniqueName="SICcode"
                    SortExpression="SICcode" HeaderTooltip="MCC" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="State" DataField="State" UniqueName="State"
                    SortExpression="State" HeaderTooltip="State" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Zip" DataField="Zip" UniqueName="Zip"
                    SortExpression="Zip" HeaderTooltip="Zip" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="ChargeBacksNumber" HeaderText="# CB" HeaderStyle-Width="80"
                    HeaderTooltip="Chargebacks Count" DataField="ChargeBacksNumber" SortExpression="ChargeBacksNumber"
                    ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="ChargeBacksAmount" HeaderText="$ CB"
                    HeaderTooltip="Chargebacks Amount" DataField="ChargeBacksAmount" SortExpression="ChargeBacksAmount"
                    ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="CB% ($)" HeaderTooltip="Chargebacks Amount Percentage" DataField="ChargeBackPercent" HeaderStyle-Width="80"
                    UniqueName="ChargeBackPercent" ASFormat="Percentage" SortExpression="ChargeBackPercent" meta:resourcekey="ASGridBoundColumnResource10">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="CB% (#)" HeaderTooltip="Chargebacks Count Percentage" DataField="ChargeBackNumberPercent" HeaderStyle-Width="80"
                    UniqueName="ChargeBackNumberPercent" ASFormat="Percentage" SortExpression="ChargeBackNumberPercent" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="SalesNumber" HeaderText="# Sales" 
                    HeaderTooltip="Number of Gross Sales" DataField="SalesNumber" SortExpression="SalesNumber" 
                    ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource12">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="SalesAmount" HeaderText="$ Sales" HeaderStyle-Width="120"
                    HeaderTooltip="Amount of Gross Sales" DataField="SalesAmount" SortExpression="SalesAmount"
                    ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource13">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="CreditsNumber" HeaderText="# Credits"
                    HeaderTooltip="Number of Credits" DataField="CreditsNumber" SortExpression="CreditsNumber"
                    ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource14">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="CreditsAmount" HeaderText="$ Credits"
                    HeaderTooltip="Amount of Credits" DataField="CreditsAmount" SortExpression="CreditsAmount"
                    ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource15">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Credit% ($)" HeaderTooltip="Credit Amount Percentage" DataField="CreditPercent"
                    UniqueName="CreditPercent" ASFormat="Percentage" SortExpression="CreditPercent" meta:resourcekey="ASGridBoundColumnResource16">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Credit% (#)" HeaderTooltip="Credit Count Percentage" DataField="CreditNumberPercent"
                    UniqueName="CreditNumberPercent" ASFormat="Percentage" SortExpression="CreditNumberPercent" meta:resourcekey="ASGridBoundColumnResource17">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="TotalIncome" HeaderText="Income" HeaderTooltip="Income"
                    DataField="TotalIncome" SortExpression="TotalIncome" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource18">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="TotalExpense" HeaderText="Expense" HeaderTooltip="Expense"
                    DataField="TotalExpense" SortExpression="TotalExpense" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource19">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="NetProfit" HeaderText="Net Profit" HeaderTooltip="Net Profit"
                    DataField="NetProfit" SortExpression="NetProfit" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource20">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/IncomeExpense.js"> </script>
</asp:Content>
