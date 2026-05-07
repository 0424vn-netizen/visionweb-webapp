<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Chargebacks.aspx.cs" Inherits="gen_Chargebacks" Title="Chargebacks" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <uc:ReportFiltering ID="uxRptFiltering" runat="server" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true"
        HasFilteringOption="true" />
    <uc:UxExport ID="uxExportDrilldownTop" runat="server" GridID="uxDrilldownGrid" IsOnTop="true" />
    <as:ASGrid ID="uxDrilldownGrid" runat="server" AllowPaging="true" GridLines="None"
        AppendHeaderforPrinter="true" IsAutoExportTemplate="true" HeaderStyle-Width="150px" XOverFlowable="true"
        AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" ShowPageTotal="false" CssClass="in" meta:resourcekey="uxDrilldownGridResource1">
        <MasterTableView>
            <PagerStyle AlwaysVisible="true" />
            <Columns>
                <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="Entity" UniqueName="DrilldownColumn"
                    ASFormat="StaticString" SortExpression="Entity" HeaderTooltip="Merchant ID" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Entity Name" DataField="EntityName" UniqueName="EntityName" HeaderStyle-Width="250px"
                    SortExpression="EntityName" HeaderTooltip="Entity Name" ASFormat="DynamicString"
                    Visible="false" meta:resourcekey="ASGridBoundColumnResource2" >
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="# Trans" DataField="TransactionCount" ASFormat="Integer" HeaderStyle-Width="120px"
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
                <as:ASGridBoundColumn UniqueName="RepresentedCBCount" HeaderText="# Chargebacks" HeaderStyle-Width="120px"
                    DataField="RepresentedCBCount" HeaderTooltip="# Chargebacks" SortExpression="RepresentedCBCount"
                    ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="# 1st Chargebacks" DataField="ChargebackCount" HeaderStyle-Width="120px"
                    ASFormat="Integer" UniqueName="ChargebacksCount" HeaderTooltip="# 1st Chargebacks"
                    SortExpression="ChargebackCount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="1st Chargebacks" DataField="ChargebackAmount" ASFormat="Currency" HeaderStyle-Width="150px"
                    UniqueName="ChargebacksAmount" HeaderTooltip="1st Chargebacks" SortExpression="ChargebackAmount"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource9">
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

                <as:ASGridBoundColumn UniqueName="PostChargebackRDRCount" HeaderText="Post RDR #" HeaderStyle-Width="120px"
                    DataField="PostChargebackRDRCount" HeaderTooltip="Post RDR Count" SortExpression="PostChargebackRDRCount" Visible="false"
                    ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource41">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="Post RDR $" DataField="PostChargebackRDRAmount" UniqueName="PostChargebackRDRAmount"
                    HeaderTooltip="Post RDR Amount" SortExpression="PostChargebackRDRAmount" ASFormat="Currency" Visible="false" HeaderStyle-Width="150px"
                    ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource42">
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
            <as:Literal ID="ltHoldTheMouseToViewDetail" runat="server" Text="** Hold the mouse over RC to view details. **" meta:resourcekey="ltHoldTheMouseToViewDetailResource1"></as:Literal></i>
        <div class="height-18"></div>
    </div>
    <as:PlaceHolder ID="uxChargebacks" runat="server">
        <uc:UxExport ID="uxExportReportGrid" runat="server" GridID="uxReportGrid" IsOnTop="true" />
        <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="true" GridLines="None" Width="100%"
            HeaderStyle-Width="120px" AppendHeaderforPrinter="true" IsAutoExportTemplate="true"
            ShowPageTotal="false" AutoGenerateColumns="false" XOverFlowable="true"  ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <PagerStyle AlwaysVisible="true" />
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" DataField="ReportDate" UniqueName="ReportDate"
                        HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransactionDate" UniqueName="TransactionDate"
                        HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="OriginalTransactionID" UniqueName="OriginalTransactionID" HeaderText="Trans Id" HeaderTooltip="Trans Id"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource28">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardType" UniqueName="CardType"
                        SortExpression="CardType" HeaderTooltip="Card Type" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="CardNumber" UniqueName="CardNumber" HeaderStyle-Width="140px"
                        SortExpression="PartialCardNumber" HeaderTooltip="Cardholder Number" ASFormat="StaticString"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" ItemStyle-Wrap="false" HeaderStyle-Width="140px"
                        UniqueName="PartialCardNumber" SortExpression="PartialCardNumber" HeaderTooltip="Card Number"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource14">
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
                        HeaderTooltip="Expiration Date" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="CBType" HeaderText="CB Type" DataField="CBType"
                        HeaderTooltip="CB Type" SortExpression="CBType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="CBTypeDesc" HeaderText="CB Type Description" DataField="CBTypeDesc"
                        HeaderTooltip="CB Type Description" SortExpression="CBTypeDesc" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RC" DataField="ReasonCode" UniqueName="ReasonCode" ItemStyle-CssClass="ellipsis"
                        HeaderTooltip="CB Reason Code" SortExpression="ReasonCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Reason Text" DataField="ReasonText" UniqueName="ReasonText" HeaderStyle-Width="180px"
                        HeaderTooltip="Reason Text" SortExpression="ReasonText" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Disposition" HeaderText="Disposition" DataField="Disposition" HeaderStyle-Width="160px" ItemStyle-CssClass="text-wrapped"
                        HeaderTooltip="Disposition" SortExpression="Disposition" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Acquirer Reference Number" DataField="ReferenceNumber" UniqueName="ReferenceNumber" HeaderStyle-Width="180px"
                        SortExpression="ReferenceNumber" HeaderTooltip="Acquirer Reference Number" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
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
                        HeaderTooltip="CB Sequence Number" SortExpression="CBSeqNo" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="RDR $" DataField="FirstChargebackRDRAmount" UniqueName="FirstChargebackRDRAmount"
                        HeaderTooltip="RDR Amount" SortExpression="FirstChargebackRDRAmount" ASFormat="Currency" Visible="false"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource40">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Post RDR $" DataField="PostChargebackRDRAmount" UniqueName="PostChargebackRDRAmount"
                        HeaderTooltip="RDR Amount" SortExpression="PostChargebackRDRAmount" ASFormat="Currency" Visible="false"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource42">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="RepresentedCBAmount" HeaderText="CB Amount" DataField="RepresentedCBAmount"
                        ASIsTotalColumn="true" HeaderTooltip="Chargeback Amount" SortExpression="RepresentedCBAmount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource23">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn UniqueName="TransactionAmount" HeaderText="1st CB Amount" DataField="TransactionAmount"
                        ASIsTotalColumn="true" HeaderTooltip="1st CB Amount" ASDefaultNullValue="" SortExpression="TransactionAmount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource24">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                    <as:ASGridBoundColumn DataField="CurrencyCode" UniqueName="CurrencyCode" HeaderText="Base Currency Type" HeaderTooltip="Base Currency Type"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="OriginalTransactionAmount" UniqueName="OriginalTransactionAmount" HeaderText="Base Currency Amount" HeaderTooltip="Base Currency Amount"
                        ASFormat="Number2Digit" meta:resourcekey="ASGridBoundColumnResource26">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                   <as:ASGridTemplateColumn HeaderStyle-CssClass="text-center" DataField="IPAddress" UniqueName="IPAddress" HeaderText="IP Address" HeaderTooltip="IP Address"
                        ItemStyle-CssClass="ellipsis text-center" meta:resourcekey="ASGridBoundColumnResource27" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="uxIPAddress" runat="server" Text='<%# Eval("IPAddress") %>' ToolTip='<%# Eval("IPAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </as:ASGridTemplateColumn>
                    <%--46652 - AW Multi-currency Transaction Display--%>
                </Columns>
            </MasterTableView>
        </as:ASGrid>

    </as:PlaceHolder>
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" LoadingPanelID="uxLoadingPanelCustom" />
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
        <script type="text/javascript" src="<%= ResolveUrl("~/")%>res/js/Chargebacks.js"></script>
    </tek:RadCodeBlock>
</asp:Content>
