<%@ Page Title="Merchant Retrievals/Chargebacks" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_RetCbDetail.aspx.cs" Inherits="rm_MCF_RetCbDetail" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ReportFiltering.ascx" TagName="ReportFiltering"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/UxExport.ascx" TagName="UxExport" TagPrefix="uc" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxRetrievalGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxRetrievalGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxCBGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCBGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <div class="row collapse report-filter-panel">
        <div class="col-md-12 report-filter">
            <div class="filter-block text-left">
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="LiteralDate" runat="server" meta:resourcekey="LiteralDateResource1"> Date:</asp:Literal></td>
                        <td>
                            <div class="filter-item">
                                <as:RadDatePicker ID="uxDateFilter" runat="server" ShowPopupOnFocus="true" meta:resourcekey="uxDateFilterResource1">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" FastNavigationStep="12" ShowRowHeaders="False"></Calendar>

                                    <DateInput ID="uxDateFilterInput" DateFormat="MM/dd/yyyy" runat="Server" LabelWidth="64px" Width="">
                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>

                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>

                                        <FocusedStyle Resize="None"></FocusedStyle>

                                        <DisabledStyle Resize="None"></DisabledStyle>

                                        <InvalidStyle Resize="None"></InvalidStyle>

                                        <HoveredStyle Resize="None"></HoveredStyle>

                                        <EnabledStyle Resize="None"></EnabledStyle>
                                    </DateInput>

                                    <DatePopupButton ImageUrl="" HoverImageUrl="" CssClass=""></DatePopupButton>
                                </as:RadDatePicker>
                            </div>
                        </td>
                        <td>
                            <div class="filter-item">
                                <as:Button runat="server" ID="uxSearch" OnClick="uxSearch_Click" OnClientClick="return ValidateDate()"
                                    Text="Search"
                                    CssClass="btn btn-default" meta:resourcekey="uxSearchResource1" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter">
                <asp:Literal ID="LiteralFilter" runat="server" meta:resourcekey="LiteralFilterResource1">FILTER</asp:Literal>
            </span>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 no-margin-bottom">
            <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Merchant Retrievals/Chargebacks" meta:resourcekey="uxPageTitleResource1" />
        </div>
        <div class="height-18"></div>
    </div>

    <div class="row">
        <div class="col-md-12 text-right">
            <a href="#" onclick="uxGoBack_Click(); return false;" class="link-back">
                <asp:Literal ID="LiteralBackToRefReport" runat="server" meta:resourcekey="LiteralBackToRefReportResource1"> Back to RET/CB Report</asp:Literal>
            </a>
            <asp:Button ID="uxButtonGoBack" runat="server" OnClick="uxButtonGoBack_Click" CssClass="display-none" meta:resourcekey="uxButtonGoBackResource1" />
        </div>
    </div>
    <div class="height-14"></div>
    <as:PlaceHolder ID="uxRetrievalPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporterRetrieval" runat="server" GridID="uxRetrievalGrid" IsOnTop="true" />
        <as:ASGrid ID="uxRetrievalGrid" runat="server" AutoGenerateColumns="False" ShowHeader="true" IsAutoExportTemplate="true"
            AllowSorting="true" AllowPaging="true" HeaderStyle-Width="125px" XOverFlowable="true" GridName="Retrieval" ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxRetrievalGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" HeaderTooltip="Process Date" DataField="ReportDate"
                        UniqueName="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" HeaderTooltip="Transaction Date" DataField="TransactionDate"
                        UniqueName="TransactionDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Type" HeaderTooltip="Card Type" DataField="CardType"
                        UniqueName="CardType" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" HeaderTooltip="Card Number" DataField="AccountNumber"
                        UniqueName="AccountNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" HeaderTooltip="Card Number" DataField="PartialAccountNumber"
                        UniqueName="PartialAccountNumber" Visible="false" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Amount" HeaderTooltip="Retrieval Amount" DataField="Amount"
                        UniqueName="Amount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RC #" HeaderTooltip="Reason Code" DataField="ReasonCode"
                        UniqueName="ReasonCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Acquirer Reference #" HeaderStyle-Width="185px" HeaderTooltip="Acquirer Reference Number" DataField="ReferenceNumber"
                        UniqueName="ReferenceNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="MCOMM Claim ID" HeaderTooltip="MCOMMClaimID" DataField="MCOMMClaimID"
                        UniqueName="MCOMMClaimID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceMCOMMClaimID">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="VROLCase #" HeaderTooltip="VROL Case Number" DataField="VROLCaseNumber"
                        UniqueName="VROLCaseNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceVROLCaseNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>


                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>
    <as:PlaceHolder ID="uxCBPlaceHolder" runat="server">
        <uc:UxExport ID="uxExporterCB" runat="server" GridID="uxCBGrid" />
        <as:ASGrid ID="uxCBGrid" runat="server" AutoGenerateColumns="False" ShowHeader="true"
            AllowSorting="true" AllowPaging="true" XOverFlowable="true" GridName="Chargeback" IsAutoExportTemplate="true"
            ASPagingMethod="SPASingleMethod" CssClass="in" meta:resourcekey="uxCBGridResource1" HeaderStyle-Width="125px">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" HeaderTooltip="Process Date" DataField="ReportDate"
                        UniqueName="ReportDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" HeaderTooltip="Transaction Date" DataField="TransactionDate"
                        UniqueName="TransactionDate" ASFormat="Date" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Type" HeaderTooltip="Card Type" DataField="CardType"
                        UniqueName="CardType" ASFormat="StaticString" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" HeaderTooltip="Card Number" DataField="AccountNumber"
                        UniqueName="AccountNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" HeaderTooltip="Card Number" DataField="PartialAccountNumber"
                        UniqueName="PartialAccountNumber" Visible="false" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="RepresentedCBAmount" HeaderText="Represented CB Amount"
                        DataField="RepresentedCBAmount" HeaderTooltip="Represented CB Amount" SortExpression="RepresentedCBAmount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="FirstChargebackAmount" HeaderText="1st CB Amount"
                        DataField="FirstChargebackAmount" HeaderTooltip="1st CB Amount" SortExpression="FirstChargebackAmount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RC #" HeaderTooltip="Reason Code" DataField="ReasonCode"
                        UniqueName="ReasonCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Acquirer Reference #" HeaderStyle-Width="185px" HeaderTooltip="Acquirer Reference Number" DataField="ReferenceNumber"
                        UniqueName="ReferenceNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource17">
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

                    <as:ASGridBoundColumn HeaderText="MCOMM Claim ID" HeaderTooltip="MCOMMClaimID" DataField="MCOMMClaimID"
                        UniqueName="MCOMMClaimID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceMCOMMClaimID">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="VROLCase #" HeaderTooltip="VROL Case Number" DataField="VROLCaseNumber"
                        UniqueName="VROLCaseNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceVROLCaseNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="FirstChargebackRDRAmount" HeaderText="RDR $" HeaderStyle-Width="150px"
                        DataField="FirstChargebackRDRAmount" HeaderTooltip="RDR Amount" SortExpression="FirstChargebackAmount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResourceFirstChargebackRDRAmount" Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="PostChargebackRDRAmount" HeaderText="Post RDR $" HeaderStyle-Width="150px"
                        DataField="PostChargebackRDRAmount" HeaderTooltip="Post RDR Amount" SortExpression="FirstChargebackAmount"
                        ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResourcePostChargebackRDRAmount" Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </as:PlaceHolder>

    <as:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var rm_RetCbDetail_uxButtonGoBack = "<%=uxButtonGoBack.ClientID %>";
            var rm_RetCbDetail_uxDateFilter = "<%=uxDateFilter.ClientID %>";
            var rm_RetCbDetail_uxDateFilter_MaxDate = "<%=uxDateFilter.MaxDate %>";
            var rm_RetCbDetail_js_Alert1 = '<%=GetLocalResourceObject("rm_RetCbDetail_js_Alert1").ToString()%>';
            var rm_RetCbDetail_js_Alert2 = '<%=GetLocalResourceObject("rm_RetCbDetail_js_Alert2").ToString()%>';
            var rm_RetCbDetail_js_Alert3 = '<%=GetLocalResourceObject("rm_RetCbDetail_js_Alert3").ToString()%>';
            var funcValidate = 'ValidateDate';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/risk_MCF/rm_MCF_RetCbDetail.js"></script>
    </as:RadCodeBlock>
</asp:Content>
