<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Non-Qualifying Transactions"
    CodeFile="NonQualifyingTransaction.aspx.cs" Inherits="QualifyingTransaction" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:ReportFiltering ID="uxReportFiltering" runat="server" />
    <%--<uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Trans History - Non-Qualifying Transactions" />--%>
    <as:PlaceHolder ID="uxDrilldownPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporterDrilldownTop" runat="server" GridID="uxDrilldownGrid"
            ShowWord="false" ShowPDF="true" IsOnTop="true" />
        <as:ASGrid ID="uxDrilldownGrid" runat="server" Width="100%" GridName="TransactionQualification"   
             AppendHeaderforPrinter="true" AutoGenerateColumns="false" IsAutoExportTemplate="true"
            ASPagingMethod="SPASingleMethod" AllowPaging="true" AllowSorting="true" ShowPageTotal="false" CssClass="in" meta:resourcekey="uxDrilldownGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="" DataField="Entity" UniqueName="DrilldownColumn"
                        SortExpression="Entity" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Entity Name" DataField="EntityName" UniqueName="EntityName"
                        SortExpression="EntityName" HeaderTooltip="Entity Name" ASFormat="DynamicString"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Warnings" DataField="WarningCount" SortExpression="WarningCount"
                        UniqueName="WarningsCount" HeaderTooltip="Warnings Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Downgrades" DataField="DowngradeCount" SortExpression="DowngradeCount"
                        UniqueName="DowngradeCount" HeaderTooltip="Downgrades Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Upgrades" DataField="UpgradeCount" SortExpression="UpgradeCount"
                        UniqueName="UpgradeCount" HeaderTooltip="Upgrades Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Fee" DataField="FeeCount" SortExpression="FeeCount"
                        UniqueName="FeeCount" HeaderTooltip="Fee Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Trans" DataField="TransactionCount" SortExpression="TransactionCount"
                        UniqueName="TransactionCount" HeaderTooltip="Transaction Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Trans" DataField="TransCount" SortExpression="TransCount"
                        UniqueName="TransCount" HeaderTooltip="Transaction Count" ASFormat="Integer"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Sales" DataField="SaleAmount" SortExpression="SaleAmount"
                        UniqueName="SaleAmount" HeaderTooltip="Sales Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Cash Advances" DataField="CashAdvanceAmount" SortExpression="CashAdvanceAmount"
                        UniqueName="CashAdvanceAmount" HeaderTooltip="Cash Advance Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Returns" DataField="ReturnAmount" SortExpression="ReturnAmount"
                        UniqueName="ReturnAmount" HeaderTooltip="Returns Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net" DataField="NetAmount" SortExpression="NetAmount"
                        UniqueName="NetAmount" HeaderTooltip="Net Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource12">
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
        <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxReportGrid" IsOnTop="true"
            ShowWord="false" ShowPDF="true" />
        <tek:RadToolTipManager ID="uxToolTipMana" runat="server" AutoTooltipify="false" meta:resourcekey="uxToolTipManaResource1" />
        <as:ASGrid ID="uxReportGrid" runat="server" GridName="TransactionQualification" ASPagingMethod="SPASingleMethod"
            AllowSorting="true" AllowPaging="true" Width="100%" AutoGenerateColumns="false"  AppendHeaderforPrinter="true" 
            ShowPageTotal="false" CssClass="in" meta:resourcekey="uxReportGridResource1" IsAutoExportTemplate="true">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" SortExpression="ReportDate"
                        UniqueName="ReportDate" ASFormat="Date" HeaderTooltip="Report Date" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" SortExpression="TransactionDate"
                        UniqueName="TransactionDate" ASFormat="StaticString" HeaderTooltip="Transaction Date" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Code" DataField="TransactionDescription"
                        SortExpression="TransactionDescription" UniqueName="TransactionCode" HeaderTooltip="Transaction Code"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans ID" DataField="TransactionID" SortExpression="TransactionID"
                        UniqueName="TransactionID" HeaderTooltip="Transaction ID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Qual Code" DataField="QualificationCode" SortExpression="QualificationCode"
                        UniqueName="QualificationCode" HeaderTooltip="Qualification Code" ASFormat="StaticString"
                        Visible="true" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Fee Rate" DataField="FeeRate" SortExpression="FeeRate"
                        UniqueName="FeeRate" HeaderTooltip="Fee Rate" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Card #"
                        HeaderTooltip="Card Number" SortExpression="PartialCardNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                        ItemStyle-Wrap="false" HeaderText="Card #" HeaderTooltip="Card Number" SortExpression="PartialCardNumber"
                        ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Auth Date" DataField="AuthorizationDate" SortExpression="AuthorizationDate"
                        UniqueName="AuthorizationDate" HeaderTooltip="Authorization Date" ASFormat="StaticString"
                        ASIsTotalColumn="false" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Auth #" DataField="AuthorizationNumber" SortExpression="AuthorizationNumber"
                        UniqueName="AuthorizationNumber" HeaderTooltip="Authorization Number" ASFormat="StaticString"
                        ASIsTotalColumn="false" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Purchase ID" DataField="PurchaseID" SortExpression="PurchaseID"
                        UniqueName="PurchaseID" HeaderTooltip="Purchase ID" ASFormat="StaticString"
                        ASIsTotalColumn="false" meta:resourcekey="ASGridBoundColumnPurchaseID">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="210px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RC" DataField="ReasonCode" SortExpression="ReasonCode"
                        UniqueName="ReasonCode" HeaderTooltip="Reason Code" ASFormat="StaticString" ASIsTotalColumn="false" meta:resourcekey="ASGridBoundColumnResource23">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Auth Amount" DataField="AuthorizationAmount" SortExpression="AuthorizationAmount"
                        UniqueName="AuthorizationAmount" HeaderTooltip="Authorization Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource24">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Amount" DataField="TransactionAmount" SortExpression="TransactionAmount"
                        UniqueName="TransactionAmount" HeaderTooltip="Transaction Amount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/NonQualifyingTransaction.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
