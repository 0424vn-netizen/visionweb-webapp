<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="RetrievalsChargebacksDetail.aspx.cs"
    Inherits="RetrievalsChargebacksDetail" Title="Retrieval/Chargeback Details" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" ContainerCssClass="container" Width="" WidthCssClass="modal-xxl">
        <h3 class="modal-title">
            <span class="gridtitle">
                <as:Literal ID="ltMerchant" runat="server" Text="Merchant:" meta:resourcekey="ltMerchantResource1"></as:Literal>
            </span>
            <asp:Literal ID="uxMerchantInfo" runat="server" meta:resourcekey="uxMerchantInfoResource1"></asp:Literal>
        </h3>
        <div runat="server" id="tblRetrieval">
            <uc:UxExport ID="uxExporter" runat="server" GridID="uxReportGrid" GridHeader="RETRIEVALS"
                IsOnTop="true" meta:resourcekey="uxExporterResource1" />
            <as:ASGrid ID="uxReportGrid" runat="server" GridLines="None" AllowPaging="True" AllowSorting="True"
                AppendHeaderforPrinter="true" IsAutoExportTemplate="true"
                AutoGenerateColumns="False" ASPagingMethod="SPASingleMethod" VisiblePageTotal="false"
                GridName="Retrievals" ShowFooter="false" ShowPageTotal="false" CssClass="in" meta:resourcekey="uxReportGridResource1">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn UniqueName="ReportDate" HeaderText="Report Date" DataField="ReportDate"
                            HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource1">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="TransactionDate" HeaderText="Trans Date" DataField="TransactionDate"
                            HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date"
                            meta:resourcekey="ASGridBoundColumnResource2">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="DateReceived" HeaderText="Date Received" DataField="DateReceived"
                            HeaderTooltip="Date Retrieval Received" SortExpression="DateReceived" ASFormat="Date"
                            meta:resourcekey="ASGridBoundColumnResource3">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="DueDate" HeaderText="Due Date" DataField="DueDate"
                            HeaderTooltip="Due Date" SortExpression="DueDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource4">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="CardType" HeaderText="Card Type" DataField="CardType"
                            HeaderTooltip="Card Type" SortExpression="CardType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="CardNumber" HeaderText="Card #" DataField="CardNumber"
                            ItemStyle-Wrap="false"
                            HeaderTooltip="Card Number" SortExpression="CardNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>

                            <ItemStyle Wrap="False"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                            SortExpression="PartialCardNumber" HeaderTooltip="Card Number" ASFormat="StaticString"
                            Visible="false" meta:resourcekey="ASGridBoundColumnResource7">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
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

                        <as:ASGridBoundColumn UniqueName="ReasonCode" HeaderText="RC" DataField="ReasonCode"
                            HeaderTooltip="Reason Code" SortExpression="ReasonCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="RequestType" HeaderText="Request Type" DataField="RequestType"
                            HeaderTooltip="Request Type" SortExpression="RequestType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="TransactionAmount" HeaderText="Trans Amount"
                            DataField="TransactionAmount" HeaderTooltip="Transaction Amount" SortExpression="TransactionAmount"
                            ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource10">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </div>
        <div runat="server" id="tblChargeback">
            <uc:UxExport ID="uxExportChargebacksTop" runat="server" GridHeader="CHARGEBACKS"
                GridID="uxReportChargebacks" IsOnTop="true" meta:resourcekey="uxExportChargebacksTopResource1" />
            <as:ASGrid ID="uxReportChargebacks" runat="server" GridLines="None" AllowPaging="True"
                AppendHeaderforPrinter="true" IsAutoExportTemplate="true"
                AllowSorting="True" AutoGenerateColumns="False" ASPagingMethod="SPASingleMethod"
                VisiblePageTotal="false" GridName="Chargebacks" ShowFooter="false" ShowPageTotal="false" CssClass="in" meta:resourcekey="uxReportChargebacksResource1">
                <MasterTableView>
                    <Columns>
                        <as:ASGridBoundColumn UniqueName="ReportDate" HeaderText="Report Date" DataField="ReportDate"
                            HeaderTooltip="Report Date" SortExpression="ReportDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource11">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="TransactionDate" HeaderText="Trans Date" DataField="TransactionDate"
                            HeaderTooltip="Transaction Date" SortExpression="TransactionDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource12">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="CardType" HeaderText="Card Type" DataField="CardType"
                            HeaderTooltip="Card Type" SortExpression="CardType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="CardNumber" HeaderText="Card #" DataField="CardNumber"
                            HeaderTooltip="Cardholder Number" SortExpression="CardNumber" ASFormat="StaticString"
                            ItemStyle-Wrap="false" meta:resourcekey="ASGridBoundColumnResource14">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>

                            <ItemStyle Wrap="False"></ItemStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" UniqueName="PartialCardNumber"
                            SortExpression="PartialCardNumber" HeaderTooltip="Card Number" ASFormat="StaticString"
                            Visible="false" meta:resourcekey="ASGridBoundColumnResource15">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
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
                        <as:ASGridBoundColumn UniqueName="CBType" HeaderText="CB Type" DataField="CBType"
                            HeaderTooltip="CB Type" SortExpression="CBType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource16">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="CBTypeDesc" HeaderText="CB Type Description"
                            DataField="CBTypeDesc" HeaderTooltip="CB Type Description" SortExpression="CBTypeDesc"
                            ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource17">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ReasonCode" HeaderText="CB Reason Code" DataField="ReasonCode" ItemStyle-CssClass="ellipsis"
                            HeaderTooltip="Reason Code" SortExpression="ReasonCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="ReasonText" HeaderText="Reason Text" DataField="ReasonText"
                            HeaderTooltip="Reason Text" SortExpression="ReasonText" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource19">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="140px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="Disposition" HeaderText="Disposition" DataField="Disposition"
                            HeaderTooltip="Disposition" SortExpression="Disposition" ASFormat="StaticString"
                            ItemStyle-Wrap="false" meta:resourcekey="ASGridBoundColumnResource20">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="190px"></HeaderStyle>

                            <ItemStyle Wrap="False"></ItemStyle>
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
                            HeaderTooltip="CB Sequence Number" SortExpression="CBSeqNo" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource21">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="RepresentedCBAmount" HeaderText="CB Amount"
                            DataField="RepresentedCBAmount" ASIsTotalColumn="true" HeaderTooltip="Chargeback Amount"
                            SortExpression="RepresentedCBAmount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource22">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                        <as:ASGridBoundColumn UniqueName="TransactionAmount" HeaderText="1st CB Amount"
                            DataField="TransactionAmount" ASIsTotalColumn="true" HeaderTooltip="1st CB Amount"
                            SortExpression="TransactionAmount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource23">
                            <ColumnValidationSettings>
                                <ModelErrorMessage Text=""></ModelErrorMessage>
                            </ColumnValidationSettings>

                            <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                        </as:ASGridBoundColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </div>
        <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxReportGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="uxReportChargebacks">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxReportChargebacks" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </tek:RadAjaxManagerProxy>
    </as:ASModalContainer>
</asp:Content>

