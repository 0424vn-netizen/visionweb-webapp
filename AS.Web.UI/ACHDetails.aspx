<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ACHDetails.aspx.cs" Inherits="ACHDetails" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxDrilldownGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxDrilldownGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <uc:ReportFiltering ID="ReportFiltering1" runat="server" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true" HasFilteringOption="true" />

    <uc:UxExport ID="uxExporterDrilldownTop" runat="server" GridID="uxDrilldownGrid" IsOnTop="true" ShowWord="false" ShowPDF="true" />
    <as:ASGrid ID="uxDrilldownGrid" runat="server" AllowPaging="true" GridLines="None"
        Width="100%" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" ShowPageTotal="false" CssClass="in" meta:resourcekey="uxDrilldownGridResource1">
        <MasterTableView>
            <PagerStyle AlwaysVisible="true" />
            <Columns>
                <as:ASGridBoundColumn HeaderText="Coporate" DataField="Entity" HeaderTooltip=""
                    UniqueName="DrilldownColumn" ASFormat="StaticString" SortExpression="Entity" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Entity Name" DataField="EntityName" UniqueName="EntityName"
                    SortExpression="EntityName" HeaderTooltip="Entity Name" ASFormat="DynamicString" Visible="false" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="# Trans" DataField="TransactionCount" ASFormat="Integer"
                    UniqueName="TransactionCount" SortExpression="TransactionCount" HeaderTooltip="Transaction Count"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Sales" DataField="SaleAmount" ASFormat="Currency"
                    UniqueName="Sales" HeaderTooltip="Sales" SortExpression="SaleAmount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Returns" DataField="ReturnAmount" ASFormat="Currency"
                    UniqueName="Returns" HeaderTooltip="Returns Amount" SortExpression="ReturnAmount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Net" DataField="NetAmount" ASFormat="Currency"
                    UniqueName="Net" HeaderTooltip="Net Amount" SortExpression="NetAmount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Deposit Amt" DataField="DepositAmount" ASFormat="Currency"
                    UniqueName="DepositAmount" HeaderTooltip="Deposit Amount" SortExpression="DepositAmount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
    <uc:UxExport ID="uxExporterReportTop" runat="server" GridID="uxReportGrid" IsOnTop="true" ShowWord="false" ShowPDF="true" />
    <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="true" GridLines="None"
        Width="100%" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" ShowPageTotal="false"
        OnItemCommand="uxReportGrid_ItemCommand" CssClass="in" meta:resourcekey="uxReportGridResource1">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" SortExpression="ReportDate"
                    UniqueName="ReportDate" ASFormat="Date" HeaderTooltip="Report Date" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Deposit Date" DataField="DepositDate" SortExpression="DepositDate"
                    UniqueName="DepositDate" ASFormat="Date" HeaderTooltip="Deposit Date" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Routing #" DataField="RoutingNumber" SortExpression="RoutingNumber"
                    UniqueName="RoutingNumber" HeaderTooltip="Routing Number" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource10">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Routing #" DataField="PartialRoutingNumber" SortExpression="RoutingNumber"
                    UniqueName="PartialRoutingNumber" HeaderTooltip="Routing Number" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="DDA #" DataField="DDANumber" SortExpression="PartialDDANumber"
                    UniqueName="DDANumber" HeaderTooltip="Direct Deposit Acount Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="DDA #" DataField="PartialDDANumber" SortExpression="PartialDDANumber"
                    UniqueName="PartialDDANumber" HeaderTooltip="Direct Deposit Acount Number" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource13">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="# Deposits" DataField="DepositCount" SortExpression="DepositCount"
                    UniqueName="DepositCount" HeaderTooltip="Deposits Transaction Count" ASFormat="Integer"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource14">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Deposits" DataField="DepositAmount" SortExpression="DepositAmount"
                    UniqueName="DepositAmount" HeaderTooltip="Deposits Amount" ASFormat="Currency"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource15">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="# Debits" DataField="DebitCount" SortExpression="DebitCount"
                    UniqueName="DebitCount" HeaderTooltip="Debits Transaction Count" ASFormat="Integer"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource16">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Debits" DataField="DebitAmount" SortExpression="DebitAmount"
                    UniqueName="DebitAmount" HeaderTooltip="Debits Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource17">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Net Deposit" DataField="NetDepositAmount" SortExpression="NetDepositAmount"
                    UniqueName="NetDepositAmount" HeaderTooltip="Net Deposits Amount" ASFormat="Currency"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource18">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
</asp:Content>

