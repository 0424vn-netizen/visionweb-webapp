<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Retrievals"
    CodeFile="Retrievals.aspx.cs" Inherits="gen_Retrievals" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:ReportFiltering ID="ReportFiltering1" runat="server" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true"
        HasFilteringOption="true" />
    <uc:UxExport ID="uxExportDrilldownTop" runat="server" GridID="uxDrilldownGrid" IsOnTop="true" />
    <as:ASGrid ID="uxDrilldownGrid" runat="server" AllowPaging="true" GridLines="None"
         AppendHeaderforPrinter="true" IsAutoExportTemplate="true"
        ShowPageTotal="false" Width="100%" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxDrilldownGridResource1">
        <MasterTableView>
            <PagerStyle AlwaysVisible="true" />
            <Columns>
                <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="Entity" UniqueName="DrilldownColumn"
                    HeaderTooltip="Merchant ID" ASFormat="StaticString" SortExpression="Entity" meta:resourcekey="ASGridBoundColumnResource1">
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
                <as:ASGridBoundColumn HeaderText="# Trans" DataField="TransactionCount" ASFormat="Integer"
                    UniqueName="TransactionCount" SortExpression="TransactionCount" HeaderTooltip="Transaction Count"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Sales" DataField="SaleAmount" ASFormat="Currency"
                    UniqueName="Sales" HeaderTooltip="Sales Amount" SortExpression="SaleAmount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Returns" DataField="ReturnAmount" ASFormat="Currency"
                    UniqueName="Returns" HeaderTooltip="Returns Amount" SortExpression="ReturnAmount"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource5">
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
                <as:ASGridBoundColumn HeaderText="# Retrievals" DataField="RetrievalCount" ASFormat="Integer"
                    UniqueName="RetrievalsCount" HeaderTooltip="Retrievals Transaction Count" SortExpression="RetrievalCount"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Retrievals" DataField="RetrievalAmount" ASFormat="Currency"
                    UniqueName="RetrievalsAmount" HeaderTooltip="Retrievals Amount" SortExpression="RetrievalAmount"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
    <div id="uxPanelFaxNumber" runat="server">
        <i>
            <as:Literal ID="ltFaxNumber" runat="server" Text="** Hold the mouse over RC to view details. **" meta:resourcekey="ltFaxNumberResource1"></as:Literal></i>
        <div class="height-18"></div>
    </div>
    <as:PlaceHolder ID="uxRetrievalsPlaceHolder" runat="server">
        <uc:UxExport ID="uxExportReportGrid" runat="server" GridID="uxReportGrid" IsOnTop="true" />
        <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="true" GridLines="None" ShowPageTotal="false" 
             AppendHeaderforPrinter="true" IsAutoExportTemplate="true"
            Width="100%" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" XOverFlowable="true" HeaderStyle-Width="120px" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                        HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" UniqueName="TransDate"
                        SortExpression="TransactionDate" HeaderTooltip="Transaction Date" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Date Received" DataField="DateReceived" UniqueName="DateReceived"
                        SortExpression="DateReceived" HeaderTooltip="Date Retrieval Received" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Due Date" DataField="DueDate" UniqueName="DueDate"
                        SortExpression="DueDate" HeaderTooltip="Due Date" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardType" UniqueName="Type"
                        SortExpression="CardType" HeaderTooltip="Card Type" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="CardNumber" UniqueName="CardNumber" HeaderStyle-Width="140px"
                        SortExpression="PartialCardNumber" HeaderTooltip="Card Number" ASFormat="StaticString"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                        SortExpression="PartialCardNumber" HeaderTooltip="Card Number" ASFormat="StaticString" HeaderStyle-Width="140px"
                        ItemStyle-Wrap="false" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

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
                        HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Acquirer Reference #" DataField="ReferenceNumber" UniqueName="ReferenceNumber" HeaderStyle-Width="180px"
                        SortExpression="ReferenceNumber" HeaderTooltip="Acquirer Reference Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource17">
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
                        HeaderTooltip="Reason Code" SortExpression="ReasonCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Request Type" DataField="RequestType" UniqueName="RequestType"
                        HeaderTooltip="Request Type" SortExpression="RequestType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Amount" DataField="TransactionAmount" UniqueName="TransactionAmount"
                        HeaderTooltip="Transaction Amount" SortExpression="TransactionAmount" ASIsTotalColumn="true"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource20">
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
        <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/Retrievals.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
