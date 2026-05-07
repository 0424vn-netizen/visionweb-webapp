<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Payment History"
    CodeFile="PaymentHistory.aspx.cs" Inherits="PaymentHistory" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PaymentChart.ascx" TagName="PaymentChart" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:ReportFiltering ID="uxReportFiltering" runat="server" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true" HasFilteringOption="true" />

     <div class="chart-container">
    <uc:PaymentChart ID="uxPaymentChart" runat="server" />
         </div>
    
    <as:PlaceHolder ID="uxDrilldownPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporterDrilldownTop" runat="server" GridID="uxDrilldownGrid" ShowWord="false" ShowPDF="true" />
        <as:ASGrid ID="uxDrilldownGrid" runat="server" Width="100%" GridName="Payment History" AutoGenerateColumns="false" MasterTableView-TableLayout="Fixed"
            XOverFlowable="true" AppendHeaderforPrinter="true" ASPagingMethod="SPASingleMethod" AllowPaging="true" AllowSorting="true" PageSize="10"
            ShowPageTotal="false" ShowReportTotal="true" CssClass="in" meta:resourcekey="uxDrilldownGridResource1" IsAutoExportTemplate="true">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="" DataField="Entity" UniqueName="DrilldownColumn" HeaderStyle-Width="180px"
                        SortExpression="Entity" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Entity Name" DataField="EntityName" UniqueName="EntityName" HeaderStyle-Width="250px"
                        SortExpression="EntityName" HeaderTooltip="Entity Name" ASFormat="DynamicString" Visible="false" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Deposits" DataField="DepositCount" SortExpression="DepositCount"
                        UniqueName="DepositCount" HeaderTooltip="Deposits Transaction Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Deposits" DataField="DepositAmount" SortExpression="DepositAmount"
                        UniqueName="DepositAmount" HeaderTooltip="Deposits Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Debits" DataField="DebitCount" SortExpression="DebitCount"
                        UniqueName="DebitCount" HeaderTooltip="Debits Transaction Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Debits" DataField="DebitAmount" SortExpression="DebitAmount"
                        UniqueName="DebitAmount" HeaderTooltip="Debits Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net Deposits" DataField="NetDeposit" SortExpression="NetDeposit"
                        UniqueName="NetDeposit" HeaderTooltip="Net Deposits Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <as:PlaceHolder ID="uxReportPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporterTop" runat="server" ShowWord="false" ShowPDF="true" GridID="uxReportGrid" />
        <as:ASGrid ID="uxReportGrid" runat="server" GridName="Payment Summary" ASPagingMethod="SPASingleMethod" Visible="false"
            AllowSorting="true" AllowPaging="true" Width="100%" AutoGenerateColumns="false" MasterTableView-TableLayout="Fixed" XOverFlowable="true"
            AppendHeaderforPrinter="true" OnItemCommand="uxReportGrid_ItemCommand" meta:resourcekey="uxReportGridResource1" 
            IsAutoExportTemplate="true"> 
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
                    <as:ASGridBoundColumn HeaderText="Trans Type" DataField="TransType"
                        UniqueName="TransType" ASFormat="StaticString" HeaderTooltip="Transaction Type" Visible="false" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Routing #" DataField="RoutingNumber" SortExpression="RoutingNumber"
                        UniqueName="RoutingNumber" HeaderTooltip="Routing Number" ASFormat="StaticString" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Routing #" DataField="PartialRoutingNumber" SortExpression="RoutingNumber"
                        UniqueName="PartialRoutingNumber" HeaderTooltip="Routing Number" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="DDA #" DataField="DDANumber" SortExpression="PartialDDANumber"
                        UniqueName="DDANumber" HeaderTooltip="Direct Deposit Account Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="DDA #" DataField="PartialDDANumber" SortExpression="PartialDDANumber"
                        UniqueName="PartialDDANumber" HeaderTooltip="Direct Deposit Account Number" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="# Deposits" DataField="DepositCount" SortExpression="DepositCount"
                        UniqueName="DepositCount" HeaderTooltip="Deposits Transaction Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Deposits" DataField="DepositAmount" SortExpression="DepositAmount"
                        UniqueName="DepositAmount" HeaderTooltip="Deposits Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Debits" DataField="DebitCount" SortExpression="DebitCount"
                        UniqueName="DebitCount" HeaderTooltip="Debits Transaction Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Debits" DataField="DebitAmount" SortExpression="DebitAmount"
                        UniqueName="DebitAmount" HeaderTooltip="Debits Amount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net Deposit" DataField="NetDepositAmount" SortExpression="NetDepositAmount"
                        UniqueName="NetDepositAmount" HeaderTooltip="Net Deposits Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxDrilldownGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxDrilldownGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/PaymentHistory.js"></script>
    </tek:RadCodeBlock>
</asp:Content>

