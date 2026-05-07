<%@ Page Title="PAYMENT DETAILS" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="PaymentHistoryDetails.aspx.cs" Inherits="PaymentHistoryDetails" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <h3 class="modal-title">
            <as:Literal ID="uxTitle" runat="server" meta:resourcekey="uxTitleResource1"></as:Literal>
        </h3>
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" ShowWord="false" ShowPDF="true" IsOnTop="true" />
        <as:ASGrid ID="uxReportGrid" runat="server" Width="100%" AllowPaging="true" AllowSorting="true"  AppendHeaderforPrinter="true" 
            ASPagingMethod="SPASingleMethod" AutoGenerateColumns="false" ShowPageTotal="false" CssClass="in" 
            meta:resourcekey="uxReportGridResource1" IsAutoExportTemplate="true">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" SortExpression="ReportDate"
                        UniqueName="ReportDate" ASFormat="Date" HeaderTooltip="Report Date" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Deposit Date" DataField="TransmissionDate" SortExpression="TransmissionDate"
                        UniqueName="DepositDate" ASFormat="Date" HeaderTooltip="Deposit Date" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Code" DataField="TransactionCodeDes" SortExpression="TransactionCodeDes"
                        UniqueName="TransactionCode" HeaderTooltip="Transaction Code" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Type" DataField="TransactionType" SortExpression="TransactionType"
                        UniqueName="TransactionType" HeaderTooltip="Transaction Type" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Routing #" DataField="RoutingNumber" SortExpression="RoutingNumber"
                        UniqueName="RoutingNumber" HeaderTooltip="Routing Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Routing #" DataField="PartialRoutingNumber" SortExpression="RoutingNumber"
                        UniqueName="PartialRoutingNumber" HeaderTooltip="Routing Number" ASFormat="StaticString" Visible="false" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="DDA #" DataField="DDANumber" SortExpression="DDANumber"
                        UniqueName="DDANumber" HeaderTooltip="Direct Deposit Account Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="DDA #" DataField="PartialDDANumber" SortExpression="PartialDDANumber"
                        UniqueName="PartialDDANumber" HeaderTooltip="Direct Deposit Account Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trace #" DataField="TraceNumber" SortExpression="TraceNumber"
                        UniqueName="TraceNumber" HeaderTooltip="Deposit Trace Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Deposit Amount" DataField="DepositAmount" SortExpression="DepositAmount"
                        UniqueName="DepositAmount" ASFormat="Currency" HeaderTooltip="Deposit Amount"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:ASModalContainer>
</asp:Content>

