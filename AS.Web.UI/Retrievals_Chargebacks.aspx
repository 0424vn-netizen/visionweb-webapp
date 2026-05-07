<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Retrievals/Chargebacks"
    CodeFile="Retrievals_Chargebacks.aspx.cs" Inherits="gen_Retrievals_Chargebacks" meta:resourcekey="PageResource1" %>

<%@ Register TagName="ReportFiltering" Src="~/UserControls/ReportFiltering.ascx"
    TagPrefix="uc" %>
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/RetrievalChargeBackChart.ascx" TagName="RTCBChart"
    TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:ReportFiltering ID="ReportFiltering1" runat="server" Mode="Mode2" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true"
        HasFilteringOption="true" />
    <div class="chart-container">
        <uc:RTCBChart ID="uxChart" runat="server" />
    </div>
    <uc:UxExport ID="uxExporterDrilldownTop" runat="server" GridID="uxDrilldownGrid"
        IsBottom="false" ShowCSV="true" ShowExcel="true" />

    <as:ASGrid ID="uxDrilldownGrid" runat="server" AllowPaging="true" GridLines="None" AppendHeaderforPrinter="true"
        AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" ShowPageTotal="false" CssClass="in" HeaderStyle-Width="165px" XOverFlowable="true"
        meta:resourcekey="uxDrilldownGridResource1" IsAutoExportTemplate="true">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Hierachy" DataField="Entity" UniqueName="DrilldownColumn"
                    SortExpression="Entity" meta:resourcekey="ASGridBoundColumnResource1">
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
                <as:ASGridBoundColumn HeaderText="# Trans" DataField="TransactionCount" UniqueName="TransactionCount"
                    SortExpression="TransactionCount" HeaderTooltip="Transaction Count" ASFormat="Integer"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Sales" DataField="SaleAmount" UniqueName="Sales"
                    HeaderTooltip="Sales Amount" SortExpression="SaleAmount" ASFormat="Currency"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Returns" DataField="ReturnAmount" UniqueName="Returns"
                    HeaderTooltip="Returns Amount" SortExpression="ReturnAmount" ASFormat="Currency"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Net" DataField="NetAmount" UniqueName="Net" HeaderTooltip="Net Amount"
                    SortExpression="NetAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="# Retrievals" DataField="RetrievalCount" UniqueName="RetrievalsCount"
                    HeaderTooltip="Retrievals Transaction Count" SortExpression="RetrievalCount" HeaderStyle-Width="65px"
                    ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Retrievals" DataField="RetrievalAmount" UniqueName="RetrievalsAmount"
                    HeaderTooltip="Retrievals Amount" SortExpression="RetrievalAmount" ASFormat="Currency"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="RepresentedCBCount" HeaderText="# Chargebacks" HeaderStyle-Width="65px"
                    DataField="RepresentedCBCount" HeaderTooltip="# Chargebacks" SortExpression="RepresentedCBCount"
                    ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="# 1st Chargebacks" DataField="ChargebackCount" HeaderStyle-Width="70px"
                    UniqueName="ChargebacksCount" HeaderTooltip="# 1st Chargebacks" SortExpression="ChargebackCount"
                    ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource10">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="1st Chargebacks" DataField="ChargebackAmount" UniqueName="ChargebacksAmount"
                    HeaderTooltip="1st Chargebacks" SortExpression="ChargebackAmount" ASFormat="Currency"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn UniqueName="FirstChargebackRDRCount" HeaderText="RDR #" HeaderStyle-Width="120px"
                    DataField="FirstChargebackRDRCount" HeaderTooltip="RDR Count" SortExpression="FirstChargebackRDRCount" Visible="false"
                    ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource39">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="RDR $" DataField="FirstChargebackRDRAmount" UniqueName="FirstChargebackRDRAmount" HeaderStyle-Width="150px"
                    HeaderTooltip="RDR Amount" SortExpression="FirstChargebackRDRAmount" ASFormat="Currency" Visible="false"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource40">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn UniqueName="PostChargebackRDRCount" HeaderText="Post RDR #"
                    DataField="PostChargebackRDRCount" HeaderTooltip="Post RDR Count" SortExpression="PostChargebackRDRCount" Visible="false" HeaderStyle-Width="120px"
                    ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource41">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="Post RDR $" DataField="PostChargebackRDRAmount" UniqueName="PostChargebackRDRAmount" HeaderStyle-Width="150px"
                    HeaderTooltip="Post RDR Amount" SortExpression="PostChargebackRDRAmount" ASFormat="Currency" Visible="false"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource42">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>

    <div id="uxPanelFaxNumber" runat="server" class="row">
        <div class="col-md-12">
            <div class="height-18"></div>
            <i>
                <as:Literal ID="Literal1" runat="server" Text="** Hold the mouse over RC to view details. **" meta:resourcekey="Literal1Resource1"></as:Literal></i>
            <div class="height-18"></div>
        </div>
    </div>
    <as:PlaceHolder ID="uxRetrievalsChargebacksPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporterRetrievalTop" runat="server" GridID="uxReportGrid" IsBottom="false"
            ShowCSV="true" ShowExcel="true" IsOnTop="true" />
        <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="true" GridLines="None" AppendHeaderforPrinter="true"
            ShowPageTotal="false" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" CssClass="in"
            meta:resourcekey="uxReportGridResource1" XOverFlowable="true" HeaderStyle-Width="120px" IsAutoExportTemplate="true">
            <MasterTableView>
                <PagerStyle AlwaysVisible="true" />
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                        HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" UniqueName="TransDate"
                        SortExpression="TransactionDate" HeaderTooltip="Transaction Date" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Date Received" DataField="DateReceived" UniqueName="DateReceived"
                        SortExpression="DateReceived" HeaderTooltip="Date Retrieval Received" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Due Date" DataField="DueDate" UniqueName="DueDate"
                        SortExpression="DueDate" HeaderTooltip="Due Date" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardType" UniqueName="CardType"
                        SortExpression="CardType" HeaderTooltip="Card Type" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="CardNumber" UniqueName="CardNumber"
                        SortExpression="PartialCardNumber" HeaderTooltip="Card Number" ASFormat="StaticString"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                        ItemStyle-Wrap="false"
                        SortExpression="PartialCardNumber" HeaderTooltip="Card Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="RoutingAccountNumber" UniqueName="RoutingAccountNumber" HeaderStyle-Width="140px"
                        ASFormat="StaticString" SortExpression="RoutingAccountNumber" Visible="false" meta:resourcekey="RoutingAccountNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="PartialRoutingACC" UniqueName="PartialRoutingACC" HeaderStyle-Width="140px"
                        ASFormat="StaticString" Visible="false"
                        SortExpression="PartialRoutingACC" meta:resourcekey="RoutingAccountNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="ExpirationDate" DataField="ExpirationDate" HeaderText="Exp Date"
                        HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Acquirer Reference #" DataField="ReferenceNumber" UniqueName="ReferenceNumber" HeaderStyle-Width="190px"
                        SortExpression="ReferenceNumber" HeaderTooltip="Acquirer Reference Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                    <as:ASGridBoundColumn HeaderText="MCOMM Claim ID" HeaderStyle-Width="120px" HeaderTooltip="MCOMMClaimID" DataField="MCOMMClaimID"
                        UniqueName="MCOMMClaimID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceMCOMMClaimID">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="VROLCase #" HeaderStyle-Width="120px" HeaderTooltip="VROL Case Number" DataField="VROLCaseNumber"
                        UniqueName="VROLCaseNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceVROLCaseNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="RC" DataField="ReasonCode" UniqueName="ReasonCode"
                        HeaderTooltip="Reason Code" SortExpression="ReasonCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Request Type" DataField="RequestType" UniqueName="RequestType"
                        HeaderTooltip="Request Type" SortExpression="RequestType" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Amount" DataField="TransactionAmount" UniqueName="TransactionAmount"
                        HeaderTooltip="Transaction Amount" SortExpression="TransactionAmount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource23">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <uc:UxExport ID="uxExportChargebackTop" runat="server" GridID="uxChargebackReportGrid"
        IsBottom="false" />
    <as:ASGrid ID="uxChargebackReportGrid" runat="server" AllowPaging="true" GridLines="None"
        AppendHeaderforPrinter="true" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" ShowPageTotal="false"
        CssClass="in" meta:resourcekey="uxChargebackReportGridResource1" IsAutoExportTemplate="true">
        <MasterTableView>
            <PagerStyle AlwaysVisible="true" />
            <Columns>
                <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                    HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource24">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" UniqueName="TransactionDate"
                    HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource25">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <%--46652 - AW Multi-currency Transaction Display--%>
                <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                    ASFormat="StaticString" HeaderStyle-Width="100px">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <%--46652 - AW Multi-currency Transaction Display--%>
                <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardType" UniqueName="CardType"
                    SortExpression="CardType" HeaderTooltip="Card Type" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource26">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Card #" DataField="CardNumber" UniqueName="CardNumber"
                    SortExpression="PartialCardNumber" HeaderTooltip="Cardholder Number" ASFormat="StaticString"
                    Visible="false" meta:resourcekey="ASGridBoundColumnResource27">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                    SortExpression="PartialCardNumber" HeaderTooltip="Card Number"
                    ItemStyle-Wrap="false"
                    ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource28">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>

                    <ItemStyle Wrap="False"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="RoutingAccountNumber" UniqueName="RoutingAccountNumber" HeaderStyle-Width="140px"
                    ASFormat="StaticString" SortExpression="RoutingAccountNumber" Visible="false" meta:resourcekey="RoutingAccountNumber">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="PartialRoutingACC" UniqueName="PartialRoutingACC" HeaderStyle-Width="140px"
                    ASFormat="StaticString" Visible="false"
                    SortExpression="PartialRoutingACC" meta:resourcekey="RoutingAccountNumber">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="ExpirationDate" DataField="ExpirationDate" HeaderText="Exp Date"
                    HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource29">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="CBType" HeaderText="CB Type" DataField="CBType"
                    HeaderTooltip="CB Type" SortExpression="CBType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource30">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="CBTypeDesc" HeaderText="CB Type Description" DataField="CBTypeDesc"
                    HeaderTooltip="CB Type Description" SortExpression="CBTypeDesc" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource31">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="RC" DataField="ReasonCode" UniqueName="ReasonCode" HeaderStyle-Width="80px"
                    HeaderTooltip="CB Reason Code" SortExpression="ReasonCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource32">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Reason Text" DataField="ReasonText" UniqueName="ReasonText"
                    HeaderTooltip="Reason Text" SortExpression="ReasonText" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource33">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="200px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="Disposition" HeaderText="Disposition" DataField="Disposition"
                    HeaderTooltip="Disposition" SortExpression="Disposition" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource34">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="190px"></HeaderStyle>

                    <ItemStyle Wrap="False"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Acquirer Reference #" DataField="ReferenceNumber" UniqueName="ReferenceNumber"
                    SortExpression="ReferenceNumber" HeaderTooltip="Acquirer Reference Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource35">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="190px"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="CB Reference #" HeaderTooltip="CB Reference Number" HeaderStyle-Width="185px" DataField="ChargebackReferenceNumber"
                    UniqueName="ChargebackReferenceNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceCBReferenceNumber">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="MCOMM Claim ID" HeaderStyle-Width="120px" HeaderTooltip="MCOMMClaimID" DataField="MCOMMClaimID"
                    UniqueName="MCOMMClaimID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceMCOMMClaimID">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="VROLCase #" HeaderStyle-Width="120px" HeaderTooltip="VROL Case Number" DataField="VROLCaseNumber"
                    UniqueName="VROLCaseNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceVROLCaseNumber">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn UniqueName="CBSeqNo" HeaderText="CB Sequence Number" DataField="CBSeqNo"
                    HeaderTooltip="CB Sequence Number" SortExpression="CBSeqNo" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource36">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="RDR $" DataField="FirstChargebackRDRAmount" UniqueName="FirstChargebackRDRAmount"
                    HeaderTooltip="RDR Amount" SortExpression="FirstChargebackRDRAmount" ASFormat="Currency" Visible="false"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource40">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="Post RDR $" DataField="PostChargebackRDRAmount" UniqueName="PostChargebackRDRAmount"
                    HeaderTooltip="RDR Amount" SortExpression="PostChargebackRDRAmount" ASFormat="Currency" Visible="false"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource42">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn UniqueName="RepresentedCBAmount" HeaderText="CB Amount" DataField="RepresentedCBAmount"
                    ASIsTotalColumn="true" HeaderTooltip="Chargeback Amount" SortExpression="RepresentedCBAmount"
                    ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource37">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn UniqueName="TransactionAmount" HeaderText="1st CB Amount" DataField="TransactionAmount"
                    ASIsTotalColumn="true" HeaderTooltip="1st CB Amount" ASDefaultNullValue="" SortExpression="TransactionAmount"
                    ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource38">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                </as:ASGridBoundColumn>
                <%--46652 - AW Multi-currency Transaction Display--%>
                <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                    ASFormat="StaticString" HeaderStyle-Width="100px">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="OriginalTransactionAmount" UniqueName="OriginalTransactionAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                    ASFormat="Number2Digit" HeaderStyle-Width="100px">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridTemplateColumn HeaderStyle-CssClass="text-center" DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address"
                    ItemStyle-CssClass="ellipsis text-center" HeaderStyle-Width="120px">
                    <ItemTemplate>
                        <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                    </ItemTemplate>
                </as:ASGridTemplateColumn>
                <%--46652 - AW Multi-currency Transaction Display--%>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
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
            <tek:AjaxSetting AjaxControlID="uxChargebackReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxChargebackReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/Retrievals_Chargebacks.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
