<%@ Page Title="STATEMENT DETAILS" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="StatementDetails_TOTAL.aspx.cs" Inherits="StatementDetails_TOTAL" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        table {
            border-spacing: 0;
            border-collapse: collapse;
        }

        table th, table td {
            padding: 5px 10px;
        }

        table th {
            background-color: white;
            color:#818386;
            text-align: center;
            border-bottom: 1px solid #818386;
        }

        table th.first_col {
            border-left: 1px solid #818386;
        }

        table.tb_processing_fees td.first_col {
            border-left: none;
        }

        table th.last_col, table td.last_col {
            border-right: 1px solid #818386;
        }

        table td {
            border-left: 1px solid #818386;
            color: #3e3e3e;
        }

        table tr.last_row td {
            border-top: 1px solid #818386;
            border-bottom: 1px solid #818386;
            background-color: white;
            font-weight: bold;
            font-style:italic;
        }

        table tr.last_row_alt td {
            border-bottom: 1px solid white;
            background-color: #C6D3DA;
            font-weight: bold;
        }

        .buttonbg {
            background-color: #32338C !important;
        }

        .middle_div {
            position: relative;
            top: 50%;
            -webkit-transform: translateY(-50%);
            -ms-transform: translateY(-50%);
            transform: translateY(-50%);
        }

        #statementHeader {
            width: 100%;
        }

        .alt_row {
            background-color: #E0E1E3;
        }

        .export_pdf {
            text-align: right;
            padding: 0 0 10px;
            position: relative;
            
        }

        .header_left {
            float: left;
            height: 115px;
            width: 59%;
        }

        .logo_cupcake_total {
            height: 84px;
            width: 100%;
            background-color: #0060a9;
        }

        .break_line {
            height: 1px;
            border: none;
            width: 100%;
            background-color: #0060A9;
        }

        .break_space36 {
            height: 36px;
        }

        .break_space33 {
            height: 33px;
        }

        .clear {
            clear: both;
        }

        .float_left {
            float: left;
        }

        .client_info {
            float: left;
            color: white;
            padding-left: 20px;
            padding-top: 5px;
            line-height: 18px;
        }

        .header_right {
            width: 39%;
            float: right;
            height: 115px;
            padding-left: 10px;
        }

        .hearder_row {
            width: 100%;
        }

        .header_row_label {
            font-weight: bold;
            text-align: right;
            padding: 5px 10px 5px 0px;
            width: 130px;
            display: inline-block;
        }

        .header_row_value {
            text-align: right;
            text-align: left;
            padding: 5px 10px;
        }

        .merchant_info_left {
            float: left;
            width: 58%;
        }

        .merchant_info_content {
            padding: 55px 0px;
            font-size: 14px;
            font-weight: bold;
            color: #1A1818;
        }

        .merchant_info_content div {
            padding: 5px 50px;
        }

        .mic_label {
            width: 120px;
            display: inline-block;
        }

        .mic_value {
            font-weight: bold;
            padding-left: 10px;
        }

        .merchant_info_right {
            float: right;
            text-align: left;
            width: 41%;
        }

        .padding_top20 {
            padding-top: 20px;
        }

        .padding_top15 {
            padding-top: 15px;
        }

        .merchant_info_title {
            font-size: 30px;
            font-weight: 400;
            color: #1a1818;
            padding-bottom: 8px;
        }

        .statement_grid_title {
            font-size: 16px;
            font-weight: 400;
            padding: 10px;
            color: white;
            background-color: #0060a9;
        }

        .statement_grid {
            border: 1px solid #818386;
            margin-top: 10px;
        }

        .statement_grid_header {
            background-color: #818386;
            color: white;
            font-size: 13px;
            padding: 8px;
            text-transform: uppercase;
            font-weight: 700;
        }

        .statement_grid_row {
            font-size: 14px;
            padding: 8px;
        }

        .padding_bottom_0px {
            padding-bottom: 0 !important;
        }

        .statement_grid_row .row_label {
            text-align: left;
        }

        .statement_grid_row .row_value {
            text-align: right;
            float: right;
        }

        .total_multi_grid {
            text-align: right;
            background-color: white;
            font-weight: bold;
            border-bottom: 1px solid #818386;
            padding: 10px;
            font-family: Arial;
            font-size: 12px;
        }

        .merchant_news {
            padding: 10px;
            margin-top: 5px;
            font-size: 14px;
        }

        .tb_grid {
            font-family:Arial;
            font-size: 12px;
            width: 100%;
        }

        .text_align_left {
            text-align: left;
        }

        .processing_fees {
            border: 1px solid #818386;
            border-bottom: none !important;
        }

        .processing_fees .grid_title {
            background-color: #818386;
            padding: 4px 10px;
            font-weight: bold;
            color: white;
            font-family: Arial;
            font-size: 12px;
        }

        .no_data_found {
            border-bottom: 1px solid #818386;
            border-right: 1px solid #818386;
        }

        .processing_fees .no_data_found {
            border-right: none;
        }

        .text_align_right {
            text-align: right;
        }

        .text_align_center {
            text-align: center;
        }

        .font_weight_bold {
            font-weight: bold;
        }

        .groovv_service {
            font-weight: bold;
            position:relative;
        }
        .groovv_service .reg {
            position:absolute;
            bottom:0;
        }
        .cursor_pointer {
            cursor:pointer;
        }
        .export_pdf_button {
            position:absolute;
            top:2px;
            right: 95px;
        }
        .export_text {
            display:inline-block; 
            cursor: pointer; 
            font-size:12px; 
            color:#6b7983;
            font-weight: 700;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
    <div class="export_pdf">
        <asp:ImageButton ID="uxExportPDF" runat="server" ImageUrl="res/images/PDFico-4reskin.png" OnClick="uxExportPDF_Click"
            ToolTip="Export to PDF" CssClass="export_pdf_button" meta:resourcekey="uxExportPDFResource1" />
        <span class="export_text" onclick="ExportPDF_TOTALStatement()"><as:Literal ID="ltExportToPDFText" runat="server" Text="EXPORT TO PDF" meta:resourcekey="ltExportToPDFTextResource1"></as:Literal></span>
        <div style="float: left;" id="Logo" runat="server" visible="false">
            <img alt="logo" src="App_Themes/MPS/images/logo.jpg" runat="server" id="uxImageLogo" />
        </div>
    </div>
    <div class="break_line"></div>
    <div id="statementHeader">
        <div class="header_left">
            <div class="middle_div logo_cupcake_total">
                <div class="float_left">
                    <img src="App_Themes/TOTAL/img/logo_Gbig.png" alt="Logo" />
                </div>
                <div class="client_info">
                    <span class="groovv_service">
                        <as:Literal ID="ltClientname" runat="server"></as:Literal>
                    </span>
                    <br />
                    <as:PlaceHolder ID="plhdAdd1" runat="server">
                        <as:Literal ID="ltClientAddress1" runat="server"></as:Literal>
                        <br />
                    </as:PlaceHolder>
                    <as:PlaceHolder ID="plhdAdd2" runat="server">
                        <as:Literal ID="ltClientAddress2" runat="server"></as:Literal>
                        <br />
                    </as:PlaceHolder>
                    <as:Literal ID="ltClientContact" runat="server"></as:Literal>
                </div>
            </div>
        </div>
        <div class="header_right">
            <div class="middle_div">
                <div class="hearder_row">
                    <span class="header_row_label"><as:Literal ID="ltMerchantIDLabel" runat="server" Text="Merchant ID" meta:resourcekey="ltMerchantIDLabelResource1"></as:Literal></span>
                    <span class="header_row_value">
                        <as:Literal ID="ltMerchantID" runat="server"></as:Literal></span>
                </div>
                <div class="hearder_row">
                    <span class="header_row_label"><as:Literal ID="Literal1" runat="server" Text="Statement Date" meta:resourcekey="Literal1Resource1" /></span>
                    <span class="header_row_value">
                        <as:Literal ID="ltReportDate" runat="server"></as:Literal></span>
                </div>
                <div class="hearder_row">
                    <span class="header_row_label"><as:Literal ID="Literal2" runat="server" Text="Questions" meta:resourcekey="Literal2Resource1" /></span>
                    <span class="header_row_value">
                        <as:Literal ID="ltQuestion" runat="server"></as:Literal></span>
                </div>
            </div>
        </div>
    </div>
    <div class="clear"></div>
    <div class="break_line"></div>
    <div id="cupcakeMerchantInfo">
        <div class="merchant_info_left">
            <div class="merchant_info_content">
                <div>
                    <as:Literal ID="ltBusinessName" runat="server"></as:Literal>
                </div>
                <div>
                    <as:Literal ID="ltOwner" runat="server"></as:Literal>
                </div>
                <div>
                    <as:Literal ID="ltAddress1" runat="server"></as:Literal>
                </div>
                <as:Panel ID="pnMerchantAddress2" runat="server">
                    <div>
                        <as:Literal ID="ltAddress2" runat="server"></as:Literal>
                    </div>
                </as:Panel>
                <div>
                    <as:Literal ID="ltZipCodeCity" runat="server"></as:Literal>
                </div>
            </div>
        </div>
        <div class="merchant_info_right">
            <div class="padding_top15">
                <div class="merchant_info_title">
                    <as:Literal ID="Literal3" runat="server" Text="Monthly Statement" meta:resourcekey="Literal3Resource1" />
                </div>
                <div class="statement_grid_title">
                    <as:Literal ID="Literal4" runat="server" Text="STATEMENT AT A GLANCE" meta:resourcekey="Literal4Resource1" />
                </div>
                <div>
                    <div class="statement_grid_header">
                        <as:Literal ID="Literal5" runat="server" Text="DEPOSITS" meta:resourcekey="Literal5Resource1" />
                    </div>
                    <div class="statement_grid_row cursor_pointer" onclick="window.location='#detailOfDepositsByDay';">
                        <span class="row_label"><as:Literal ID="Literal6" runat="server" Text="Sales" meta:resourcekey="Literal6Resource1" /></span>
                        <span class="row_value">
                            <as:Literal ID="ltDepositsSale" runat="server"></as:Literal></span>
                    </div>
                    <div class="statement_grid_row alt_row cursor_pointer" onclick="window.location='#detailOfDepositsByDay';">
                        <span class="row_label"><as:Literal ID="Literal7" runat="server" Text="Refunds" meta:resourcekey="Literal7Resource1" /></span>
                        <span class="row_value">
                            <as:Literal ID="ltDepositsRefunds" runat="server"></as:Literal></span>
                    </div>
                    <div class="break_line"></div>
                    <div class="statement_grid_row font_weight_bold cursor_pointer" onclick="window.location='#detailOfDepositsByDay';">
                        <span class="row_label"><as:Literal ID="Literal8" runat="server" Text="Total Deposits To Your Account" meta:resourcekey="Literal8Resource1" /></span>
                        <span class="row_value">
                            <as:Literal ID="ltTotalDepositsToYourAccount" runat="server"></as:Literal></span>
                    </div>
                    <div class="statement_grid_header">
                        <as:Literal ID="Literal9" runat="server" Text="BILLED TO YOUR ACCOUNT" meta:resourcekey="Literal9Resource1" />
                    </div>
                    <div class="statement_grid_row cursor_pointer" onclick="window.location='#monthlyFeesAndPromotions';">
                        <span class="row_label"><as:Literal ID="Literal10" runat="server" Text="Monthly Fees and Promotions" meta:resourcekey="Literal10Resource1" /></span>
                        <span class="row_value">
                            <as:Literal ID="ltMonthlyFeesAndPromotions" runat="server"></as:Literal></span>
                    </div>
                    <div class="statement_grid_row alt_row cursor_pointer" onclick="window.location='#processingfees';">
                        <span class="row_label"><as:Literal ID="Literal11" runat="server" Text="Processing Fees" meta:resourcekey="Literal11Resource1" /></span>
                        <span class="row_value">
                            <as:Literal ID="ltProcessingFees" runat="server"></as:Literal></span>
                    </div>
                    <div class="statement_grid_row cursor_pointer" onclick="window.location='#eventDrivenFees';">
                        <span class="row_label"><as:Literal ID="Literal12" runat="server" Text="Event-Driven Fees" meta:resourcekey="Literal12Resource1" /></span>
                        <span class="row_value">
                            <as:Literal ID="ltEventFees" runat="server"></as:Literal></span>
                    </div>
                    <div class="break_line"></div>
                    <div class="statement_grid_row font_weight_bold padding_bottom_0px">
                        <span class="row_label"><as:Literal ID="Literal13" runat="server" Text="Total Processing Fees Billed To Your Account" meta:resourcekey="Literal13Resource1" /></span>
                        <span class="row_value">
                            <as:Literal ID="ltTotalFeesBill" runat="server"></as:Literal></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="clear"></div>
    <div class="break_space33"></div>
    <div id="inTheNew">
        <div class="statement_grid_title">
            <as:Literal ID="Literal14" runat="server" Text="IN THE NEWS" meta:resourcekey="Literal14Resource1" />
        </div>
        <div class="merchant_news padding_bottom_0px">
            <as:Literal ID="ltMessages" runat="server"></as:Literal>
        </div>
    </div>
    <div class="clear"></div>
    <div class="break_space33"></div>
    <div id="detailOfDepositsByDay">
        <div class="statement_grid_title">
            <as:Literal ID="Literal15" runat="server" Text="DETAIL OF DEPOSITS BY DAY" meta:resourcekey="Literal15Resource1" />
        </div>
        <table class="tb_grid">
            <tr>
                <th class="first_col">Date</th>
                <th><as:Literal ID="Literal16" runat="server" Text="Reference Number" meta:resourcekey="Literal16Resource1" /></th>
                <th><as:Literal ID="Literal17" runat="server" Text="Number Of Transactions" meta:resourcekey="Literal17Resource1" /></th>
                <th><as:Literal ID="Literal18" runat="server" Text="Sales" meta:resourcekey="Literal18Resource1" /></th>
                <th><as:Literal ID="Literal19" runat="server" Text="Refunds" meta:resourcekey="Literal19Resource1" /></th>
                <th class="last_col"><as:Literal ID="Literal20" runat="server" Text="Total Deposit" meta:resourcekey="Literal20Resource1" /></th>
            </tr>
            <as:Panel ID="pnNoData_DetailDepositByDay" runat="server" Visible="true">
                <tr>
                    <td colspan="6" class="no_data_found"><as:Literal ID="Literal21" runat="server" Text="No data found." meta:resourcekey="Literal21Resource1" /></td>
                </tr>
            </as:Panel>
            <asp:Repeater ID="rptDetailOfDepositsByDay" runat="server" OnItemDataBound="rptDetailOfDepositsByDay_ItemDataBound">
                <ItemTemplate>
                    <tr class="alt_row">
                        <td class="text_align_center"><%# FormatDate(Eval("Day")) %></td>
                        <td class="text_align_center"><%# Eval("ReferenceNumber") %></td>
                        <td class="text_align_center"><%# FormatInteger(Eval("Items")) %></td>
                        <td class="text_align_right"><%# FormatCurrency(Eval("Sales")) %></td>
                        <td class="text_align_right"><%# FormatCurrency(Eval("Credits")) %></td>
                        <td class="last_col text_align_right"><%# FormatCurrency(Eval("NetDeposit")) %></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr>
                        <td class="text_align_center"><%# FormatDate(Eval("Day")) %></td>
                        <td class="text_align_center"><%# Eval("ReferenceNumber") %></td>
                        <td class="text_align_center"><%# FormatInteger(Eval("Items")) %></td>
                        <td class="text_align_right"><%# FormatCurrency(Eval("Sales")) %></td>
                        <td class="text_align_right"><%# FormatCurrency(Eval("Credits")) %></td>
                        <td class="last_col text_align_right"><%# FormatCurrency(Eval("NetDeposit")) %></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    <tr class="last_row">
                        <td colspan="2" class="text_align_left"><as:Literal ID="Literal210" runat="server" Text="Total Sales + Refunds" meta:resourcekey="Literal210Resource1" /></td>
                        <td class="text_align_center">
                            <as:Literal ID="ltTotalDODBD_NumberOfTrans" runat="server"></as:Literal></td>
                        <td class="text_align_right">
                            <as:Literal ID="ltTotalDODBD_Sales" runat="server"></as:Literal></td>
                        <td class="text_align_right">
                            <as:Literal ID="ltTotalDODBD_Refunds" runat="server"></as:Literal></td>
                        <td class="last_col text_align_right">
                            <as:Literal ID="ltTotalDODBD_TotalDeposit" runat="server"></as:Literal></td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="clear"></div>
    <div class="break_space36"></div>
    <div id="monthlyFeesAndPromotions">
        <div class="statement_grid_title">
            <as:Literal ID="Literal22" runat="server" Text="MONTHLY FEES AND PROMOTIONS" meta:resourcekey="Literal22Resource1" />
        </div>
        <table class="tb_grid">
            <tr>
                <th class="first_col"><as:Literal ID="Literal23" runat="server" Text="Fee Description" meta:resourcekey="Literal23Resource1" /></th>
                <th><as:Literal ID="Literal24" runat="server" Text="Qty" meta:resourcekey="Literal24Resource1" /></th>
                <th><as:Literal ID="Literal25" runat="server" Text="Item Fee" meta:resourcekey="Literal25Resource1" /></th>
                <th class="last_col"><as:Literal ID="Literal26" runat="server" Text="Total Fee" meta:resourcekey="Literal26Resource1" /></th>
            </tr>
            <as:Panel ID="pnNoData_MonthlyFeesPromotions" runat="server" Visible="true">
                <tr>
                    <td colspan="4" class="no_data_found"><as:Literal ID="Literal27" runat="server" Text="No data found." meta:resourcekey="Literal27Resource1" /></td>
                </tr>
            </as:Panel>
            <asp:Repeater ID="rptMonthlyFeesAndPromotion" runat="server" OnItemDataBound="rptMonthlyFeesAndPromotion_ItemDataBound">
                <ItemTemplate>
                    <tr class="alt_row">
                        <td><%# Eval("FeeDescription") %></td>
                        <td class="text_align_center"><%# FormatInteger(Eval("Count")) %></td>
                        <td class="text_align_right"><%# FormatCurrency(Eval("ItemFee")) %></td>
                        <td class="last_col text_align_right"><%# FormatCurrency(Eval("Total")) %></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr>
                        <td><%# Eval("FeeDescription") %></td>
                        <td class="text_align_center"><%# FormatInteger(Eval("Count")) %></td>
                        <td class="text_align_right"><%# FormatCurrency(Eval("ItemFee")) %></td>
                        <td class="last_col text_align_right"><%# FormatCurrency(Eval("Total")) %></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    <tr class="last_row">
                        <td colspan="3" class="text_align_left"><as:Literal ID="Literal321" runat="server" Text="Total" meta:resourcekey="Literal321Resource1" /></td>
                        <td class="last_col text_align_right">
                            <as:Literal ID="ltMonthlyFeeAndPromotions_TT" runat="server"></as:Literal></td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="clear"></div>
    <div class="break_space36"></div>
    <div id="processingfees">
        <div class="statement_grid_title">
            <as:Literal ID="Literal28" runat="server" Text="PROCESSING FEES" meta:resourcekey="Literal28Resource1" />
        </div>
        <div class="processing_fees">
            <table class="tb_grid tb_processing_fees">
                <tr>
                    <th width="40%"><as:Literal ID="Literal29" runat="server" Text="Card Type" meta:resourcekey="Literal29Resource1" /></th>
                    <th width="14%"><as:Literal ID="Literal30" runat="server" Text="Rate" meta:resourcekey="Literal30Resource1" /></th>
                    <th width="15%"><as:Literal ID="Literal31" runat="server" Text="Sales" meta:resourcekey="Literal31Resource1" /></th>
                    <th width="15%"><as:Literal ID="Literal32" runat="server" Text="Refunds" meta:resourcekey="Literal32Resource1" /></th>
                    <th width="15%"><as:Literal ID="Literal33" runat="server" Text="Total Fee" meta:resourcekey="Literal33Resource1" /></th>
                </tr>
            </table>
            <div class="grid_title">
                <as:Literal ID="Literal34" runat="server" Text="PROCESSING RATE FEE" meta:resourcekey="Literal34Resource1" />
            </div>
            <table class="tb_grid tb_processing_fees">
                <colgroup>
                    <col width="40%" />
                    <col width="14%" />
                    <col width="15%" />
                    <col width="15%" />
                    <col width="15%" />
                </colgroup>
                <as:Panel ID="pnNoData_ProcessingRate" runat="server">
                    <tr class="no_data_found">
                        <td colspan="5"><as:Literal ID="Literal35" runat="server" Text="No data found." meta:resourcekey="Literal35Resource1" /></td>
                    </tr>
                </as:Panel>
                <asp:Repeater ID="rptProcessingRate" runat="server" OnItemDataBound="rptProcessingRate_ItemDataBound">
                    <ItemTemplate>
                        <tr class="alt_row">
                            <td class="first_col"><%#Eval("CardType") %></td>
                            <td class="text_align_right"><%# FormatPercent(Eval("Rate")) %></td>
                            <td class="text_align_right"><%# FormatCurrency(Eval("Sales")) %></td>
                            <td class="text_align_right"><%# FormatCurrency(Eval("Refund")) %></td>
                            <td class="text_align_right"><%# FormatCurrency(Eval("TotalFee")) %></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td class="first_col"><%#Eval("CardType") %></td>
                            <td class="text_align_right"><%# FormatPercent(Eval("Rate")) %></td>
                            <td class="text_align_right"><%# FormatCurrency(Eval("Sales")) %></td>
                            <td class="text_align_right"><%# FormatCurrency(Eval("Refund")) %></td>
                            <td class="text_align_right"><%# FormatCurrency(Eval("TotalFee")) %></td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        <tr class="last_row">
                            <td class="text_align_left"><as:Literal ID="Literal320" runat="server" Text="Total" meta:resourcekey="Literal320Resource1" /></td>
                            <td></td>
                            <td class="text_align_right">
                                <as:Literal ID="ltPRSumOfSale" runat="server"></as:Literal></td>
                            <td class="text_align_right">
                                <as:Literal ID="ltPRSumOfRefund" runat="server"></as:Literal></td>
                            <td class="text_align_right">
                                <as:Literal ID="ltPRSumOfTotalFee" runat="server"></as:Literal></td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>


            </table>
            <div class="grid_title">
                <as:Literal ID="Literal36" runat="server" Text="TRANSACTION FEE" meta:resourcekey="Literal36Resource1" />
            </div>
            <table class="tb_grid tb_processing_fees">
                <colgroup>
                    <col width="40%" />
                    <col width="14%" />
                    <col width="15%" />
                    <col width="15%" />
                    <col width="15%" />
                </colgroup>
                <as:Panel ID="pnNoData_TransactionFee" runat="server">
                    <tr class="no_data_found">
                        <td colspan="5"><as:Literal ID="Literal37" runat="server" Text="No data found." meta:resourcekey="Literal37Resource1" /></td>
                    </tr>
                </as:Panel>
                <asp:Repeater ID="rptTransactionFees" runat="server" OnItemDataBound="rptTransactionFees_ItemDataBound">
                    <ItemTemplate>
                        <tr class="alt_row">
                            <td class="first_col"><%#Eval("CardType") %></td>
                            <td class="text_align_right"><%# FormatCurrency(Eval("Rate")) %></td>
                            <td class="text_align_center"><%# FormatInteger(Eval("Sales")) %></td>
                            <td class="text_align_center"><%# FormatInteger(Eval("Refund")) %></td>
                            <td class="text_align_right"><%# FormatCurrency(Eval("TotalFee")) %></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td class="first_col"><%#Eval("CardType") %></td>
                            <td class="text_align_right"><%# FormatCurrency(Eval("Rate")) %></td>
                            <td class="text_align_center"><%# FormatInteger(Eval("Sales")) %></td>
                            <td class="text_align_center"><%# FormatInteger(Eval("Refund")) %></td>
                            <td class="text_align_right"><%# FormatCurrency(Eval("TotalFee")) %></td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        <tr class="last_row">
                            <td class="text_align_left"><as:Literal ID="Literal370" runat="server" Text="Total" meta:resourcekey="Literal370Resource1" /></td>
                            <td></td>
                            <td class="text_align_center">
                                <as:Literal ID="ltTFSumOfSale" runat="server"></as:Literal></td>
                            <td class="text_align_center">
                                <as:Literal ID="ltTFSumOfRefund" runat="server"></as:Literal></td>
                            <td class="text_align_right">
                                <as:Literal ID="ltTFSumOfTotalFee" runat="server"></as:Literal></td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
            <div class="total_multi_grid">
                <span style="float:left;"><as:Literal ID="ltTotalProcFees" runat="server" Text="TOTAL PROCESSING FEES" meta:resourcekey="ltTotalProcFeesResource1" ></as:Literal></span>
                <as:Literal ID="ltTotalFees" runat="server"></as:Literal>
            </div>
        </div>
    </div>
    <div class="clear"></div>
    <div class="break_space36"></div>
    <div id="eventDrivenFees">
        <div class="statement_grid_title">
            <as:Literal ID="ltEventDrivenFees" runat="server" Text="EVENT-DRIVEN FEES" meta:resourcekey="ltEventDrivenFeesResource1" ></as:Literal>
        </div>
        <table class="tb_grid">
            <tr>
                <th class="first_col"><as:Literal ID="ltEDFType" runat="server" Text="Type" meta:resourcekey="ltEDFTypeResource1" ></as:Literal></th>
                <th><as:Literal ID="ltEDFRate" runat="server" Text="Rate" meta:resourcekey="ltEDFRateResource1" ></as:Literal></th>
                <th><as:Literal ID="ltEDFQty" runat="server" Text="Qty" meta:resourcekey="ltEDFQtyResource1" ></as:Literal></th>
                <th class="last_col"><as:Literal ID="ltEDFTotalCharge" runat="server" Text="Total Charge" meta:resourcekey="ltEDFTotalChargeResource1" ></as:Literal></th>
            </tr>
            <as:Panel ID="pnNoData_EventDrivenFees" runat="server" Visible="true">
                <tr>
                    <td colspan="4" class="no_data_found"><as:Literal ID="ltEDFNoData" runat="server" Text="No data found." meta:resourcekey="ltEDFNoDataResource1" ></as:Literal></td>
                </tr>
            </as:Panel>
            <asp:Repeater ID="rptEventDrivenFees" runat="server" OnItemDataBound="rptEventDrivenFees_ItemDataBound">
                <ItemTemplate>
                    <tr class="alt_row">
                        <td><%# Eval("Type") %></td>
                        <td class="text_align_right"><%# FormatCurrency(Eval("Rate")) %></td>
                        <td class="text_align_center"><%# FormatInteger(Eval("Qty")) %></td>
                        <td class="last_col text_align_right"><%# FormatCurrency(Eval("TotalCharge")) %> </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr>
                        <td><%# Eval("Type") %></td>
                        <td class="text_align_right"><%# FormatCurrency(Eval("Rate")) %></td>
                        <td class="text_align_center"><%# FormatInteger(Eval("Qty")) %></td>
                        <td class="last_col text_align_right"><%# FormatCurrency(Eval("TotalCharge")) %> </td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    <tr class="last_row">
                        <td colspan="3" class="text_align_left"><as:Literal ID="Literal38" runat="server" Text="Total" meta:resourcekey="Literal38Resource1" /></td>
                        <td class="last_col text_align_right">
                            <as:Literal ID="ltEventDrivenFees_TTCharge" runat="server"></as:Literal></td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="height-18"></div>
    <div class="row">
        <div class="col-md-12 text-right no-margin-action-container">
            <as:Button runat="server" ID="uxClose" Text="Close" OnClientClick="return parent.HidePopupModal();"
                CssClass="btn btn-default" meta:resourcekey="uxCloseResource1" />
        </div>
    </div>
    </as:ASModalContainer>
    <tek:RadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            var StatementDetails_TOTAL_uxExportPDF_ClientID = '<%= uxExportPDF.ClientID %>';
            function ExportPDF_TOTALStatement() {
                $('#' + StatementDetails_TOTAL_uxExportPDF_ClientID).click();
            }
        </script>
    </tek:RadCodeBlock>
</asp:Content>

