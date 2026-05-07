<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Title="Batch History"
    CodeFile="BatchHistory.aspx.cs" Inherits="gen_BatchHistory" meta:resourcekey="PageResource1" %>

<%@ Register TagName="ReportFiltering" Src="~/UserControls/ReportFiltering.ascx"
    TagPrefix="uc" %>
<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/BatchChart.ascx" TagName="BatchChart" TagPrefix="Chart" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">

    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxDrilldownGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxDrilldownGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxBatchMerchantGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxBatchMerchantGrid" LoadingPanelID="uxLoadingPanelCustom" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxVoidedRejectedTransGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxVoidedRejectedTransGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxTranQualificationGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxTranQualificationGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxCardMerchantGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCardMerchantGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

    <uc:ReportFiltering ID="uxReportFiltering" runat="server" Mode="Mode3" />
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="" HasShowHierarchy="true"
        HasFilteringOption="true" />
    <div>
        <asp:HyperLink runat="server" ID="uxReturnUrl" meta:resourcekey="uxReturnUrlResource1"></asp:HyperLink>
    </div>

    <div class="chart-container">
        <Chart:BatchChart ID="uxChart" runat="server" />
    </div>

    <as:PlaceHolder ID="uxPanelHierarchy" runat="server">
        <uc:UxExport ID="uxExporter" runat="server" GridID="uxDrilldownGrid" />
        <as:ASGrid ID="uxDrilldownGrid" runat="server" AllowPaging="true" GridLines="None" MasterTableView-TableLayout="Fixed"
            IntruderSourceName="uxDrilldownGrid" AppendHeaderforPrinter="true" IsAutoExportTemplate="true"
            AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" ShowPageTotal="false"
            ShowReportTotal="true" CssClass="in" meta:resourcekey="uxDrilldownGridResource1" XOverFlowable="true" HeaderStyle-Width="120px">

            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="DrilldownColumn" DataField="Entity" UniqueName="DrilldownColumn"
                        SortExpression="Entity" HeaderTooltip="DrilldownColumn" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1"
                        HeaderStyle-Width="210px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Entity Name" DataField="EntityName" UniqueName="EntityName"
                        SortExpression="EntityName" HeaderTooltip="Entity Name" ASFormat="DynamicString"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource2" HeaderStyle-Width="270px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="% Keyed" DataField="KeyedPercent" UniqueName="KeyedPercent"
                        HeaderTooltip="Percentage of Keyed" SortExpression="KeyedPercent" ASFormat="Percentage"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource3" HeaderStyle-Width="90px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Avg. Trans" DataField="AvgTrans" UniqueName="AvgTrans" HeaderStyle-Width="90px"
                        HeaderTooltip="Average Transaction" SortExpression="AvgTrans" ASIsTotalColumn="true"
                        ASFormat="Currency"
                        meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Trans" DataField="TransactionCount" UniqueName="TransactionCount"
                        HeaderTooltip="Transaction Count" SortExpression="TransactionCount" ASIsTotalColumn="true" HeaderStyle-Width="110px"
                        ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Sales" HeaderTooltip="Settled Sales" DataField="BankCardSaleAmount"
                        UniqueName="BankCardSaleAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Returns" HeaderTooltip="Settled Returns" DataField="BankCardReturnAmount"
                        UniqueName="BankCardReturnAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net" HeaderTooltip="Settled Net" DataField="BankCardNetAmount"
                        UniqueName="BankCardNetAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Sales" HeaderTooltip="Non Settled Sales" DataField="NonBankCardSaleAmount"
                        UniqueName="NonBankCardSaleAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Returns" HeaderTooltip="Non Settled Returns"
                        DataField="NonBankCardReturnAmount"
                        UniqueName="NonBankCardReturnAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net" HeaderTooltip="Non Settled Net" DataField="NonBankCardNetAmount"
                        UniqueName="NonBankCardNetAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Sales" HeaderTooltip="Total Sales" DataField="SaleAmount"
                        UniqueName="SaleAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Returns" HeaderTooltip="Total Returns" DataField="ReturnAmount"
                        UniqueName="ReturnAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net" HeaderTooltip="Total Net" DataField="NetAmount"
                        UniqueName="NetAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>

    </as:PlaceHolder>
    <as:PlaceHolder ID="uxPanelMerchantDetail" runat="server">
        <uc:UxExport ID="uxExporterBatchMerchantGridTop" runat="server" GridID="uxBatchMerchantGrid"
            ShowExcel="true" ShowPDF="true" />
        <as:ASGrid ID="uxBatchMerchantGrid" runat="server" AutoGenerateColumns="false"
            ASPagingMethod="SPASingleMethod" AllowSorting="True" AllowPaging="True" AppendHeaderforPrinter="true"
            IntruderSourceName="uxBatchMerchantGrid" IsIntruder="true" ShowPageTotal="false" IsAutoExportTemplate="true"
            ShowReportTotal="true" HeaderStyle-Width="100px" CssClass="in" meta:resourcekey="uxBatchMerchantGridResource1">

            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" HeaderTooltip="Report Date" DataField="ReportDate"
                        UniqueName="ReportDate" ASFormat="Date" SortExpression="ReportDate" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Terminal #" HeaderTooltip="Business Terminal Number"
                        DataField="TerminalNumber" UniqueName="TerminalNumber" SortExpression="TerminalNumber"
                        ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="130px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="File Source" HeaderTooltip="File Source" DataField="FileSource"
                        UniqueName="FileSource" SortExpression="FileSource" Visible="false" ASFormat="StaticString" HeaderStyle-Width="200px" ItemStyle-CssClass="word-break" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Batch #" HeaderTooltip="Batch Number" DataField="BatchNumber"
                        UniqueName="ExBatchNumber" SortExpression="BatchNumber" ASFormat="StaticString"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridTemplateColumn UniqueName="BatchNumber" HeaderText="Batch #" HeaderTooltip="Batch Number"
                        SortExpression="BatchNumber" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource1">
                        <ItemTemplate>
                            <as:LinkButton ID="uxLinkButton" runat="server" OnCommand="uxLinkButton_Command"
                                CommandArgument='<%# Eval("BatchNumber") + ";" + Eval("ReportDate") + ";" + Eval("TerminalNumber") %>'
                                Text='<%# Eval("BatchNumber") %>' meta:resourcekey="uxLinkButtonResource1"></as:LinkButton>
                        </ItemTemplate>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn HeaderText="% Keyed" HeaderTooltip="Percentage of Keyed" DataField="KeyedPercent"
                        UniqueName="KeyedPercent" SortExpression="KeyedPercent" ASFormat="Percentage"
                        ASIsTotalColumn="true" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Avg. Trans" HeaderTooltip="Average Transaction"
                        DataField="AvgTrans" UniqueName="AverageTransaction" ASFormat="Currency" SortExpression="AvgTrans"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="# Trans" HeaderTooltip="Transaction Count" DataField="TransactionCount"
                        UniqueName="TransactionCount" ASFormat="Integer" SortExpression="TransactionCount"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Sales" HeaderTooltip="Settled Sales" DataField="BankCardSaleAmount"
                        UniqueName="BankCardSaleAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Returns" HeaderTooltip="Settled Returns" DataField="BankCardReturnAmount"
                        UniqueName="BankCardReturnAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource23">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net" HeaderTooltip="Settled Net" DataField="BankCardNetAmount"
                        UniqueName="BankCardNetAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource24">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Sales" HeaderTooltip="Non Settled Sales" DataField="NonBankCardSaleAmount"
                        UniqueName="NonBankCardSaleAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource25">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Returns" HeaderTooltip="Non Settled Returns"
                        DataField="NonBankCardReturnAmount" UniqueName="NonBankCardReturnAmount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource26">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net" HeaderTooltip="Non Settled Net" DataField="NonBankCardNetAmount"
                        UniqueName="NonBankCardNetAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource27">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Sales" HeaderTooltip="Total Sales" DataField="SaleAmount"
                        UniqueName="SaleAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource28">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Returns" HeaderTooltip="Total Returns" DataField="ReturnAmount"
                        UniqueName="ReturnAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource29">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Net" HeaderTooltip="Total Net" DataField="NetAmount"
                        UniqueName="NetAmount" ASFormat="Currency" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource30">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>

            </MasterTableView>

            <HeaderStyle Width="100px"></HeaderStyle>

        </as:ASGrid>

        <uc:UxExport ID="uxExportCardSummaryTop" runat="server" GridID="uxCardMerchantGrid" />
        <as:ASGrid ID="uxCardMerchantGrid" runat="server" AutoGenerateColumns="false" Skin="Default" AppendHeaderforPrinter="true"
            IsIntruder="false" AllowSorting="True" ASPagingMethod="SPASingleMethod" AllowPaging="True" IsAutoExportTemplate="true"
            PageSize="10" ShowPageTotal="false" ShowReportTotal="true" CssClass="in" meta:resourcekey="uxCardMerchantGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Card Type" HeaderTooltip="Card Type" DataField="CardDescription" HeaderStyle-Width="200px"
                        UniqueName="CardType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource31">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Transaction Code" HeaderTooltip="Transaction Code"
                        DataField="TransactionDescription" UniqueName="TransactionCode" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource32">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Tran Cnt." HeaderTooltip="Transaction Count" DataField="TransactionCount"
                        UniqueName="TransactionCount" ASFormat="Integer" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource33">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Total" HeaderTooltip="Total Transaction Amount"
                        DataField="NetAmount" UniqueName="TotalTransactionAmount" ASFormat="Currency"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource34">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>

        <uc:UxExport ID="uxExporterCardMerchantGridTop" runat="server" GridID="uxVoidedRejectedTransGrid"
            ShowExcel="true" ShowPDF="true" />
        <as:ASGrid ID="uxVoidedRejectedTransGrid" runat="server" AutoGenerateColumns="false" IsAutoExportTemplate="true"
            Skin="Default" IsIntruder="false" AllowSorting="True" AppendHeaderforPrinter="true"
            ASPagingMethod="SPASingleMethod" AllowPaging="True" ShowPageTotal="false" ShowReportTotal="true"
            IntruderSourceName="uxVoidedRejectedTransGrid" CssClass="in" meta:resourcekey="uxVoidedRejectedTransGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" HeaderTooltip="Report Date" DataField="ReportDate"
                        UniqueName="ReportDate" ASFormat="Date" SortExpression="ReportDate" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource35">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Batch #" HeaderTooltip="Batch Number" DataField="BatchNumber"
                        UniqueName="BatchNumber" ASFormat="StaticString" SortExpression="BatchNumber" meta:resourcekey="ASGridBoundColumnResource36">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" HeaderTooltip="Transaction Date" DataField="TransactionDate"
                        UniqueName="TransactionDate" ASFormat="Date" SortExpression="TransactionDate"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource37">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Time" HeaderTooltip="Transaction Time" DataField="TransactionTime"
                        UniqueName="TransactionTime" ASFormat="StaticString" SortExpression="TransactionTime"
                        HeaderStyle-Width="90px" meta:resourcekey="ASGridBoundColumnResource38">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Code" HeaderTooltip="Transaction Code" DataField="TransactionDescription"
                        UniqueName="TransactionCode" ASFormat="StaticString" SortExpression="TransactionDescription" meta:resourcekey="ASGridBoundColumnResource39">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Keyed" HeaderTooltip="KEYED or SWIPED" DataField="KeyedEntry"
                        UniqueName="KeyedEntry" ASFormat="StaticString" SortExpression="KeyedEntry" meta:resourcekey="ASGridBoundColumnResource40">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn DataField="EMVIndicator" UniqueName="EMVIndicator" HeaderText="EMV" HeaderStyle-Width="70px"
                        ASFormat="StaticString" HeaderTooltip="EMV" meta:resourcekey="ASGridBoundColumnResource60">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card Type" HeaderTooltip="Card Type" DataField="CardType"
                        UniqueName="CardType" ASFormat="StaticString" SortExpression="CardType" HeaderStyle-Width="80px" meta:resourcekey="ASGridBoundColumnResource41">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" HeaderTooltip="Card Number" DataField="AccountNumber" HeaderStyle-Width="140px"
                        UniqueName="AccountNumber" ASFormat="StaticString" SortExpression="PartialCardNumber"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource42">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" HeaderTooltip="Card Number" DataField="PartialCardNumber" HeaderStyle-Width="140px"
                        ItemStyle-Wrap="false" UniqueName="PartialCardNumber" ASFormat="StaticString"
                        SortExpression="PartialCardNumber" meta:resourcekey="ASGridBoundColumnResource43">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Auth #" HeaderTooltip="Authorization Number" DataField="AuthorizationNumber"
                        UniqueName="AuthNumber" ASFormat="StaticString" SortExpression="AuthorizationNumber" meta:resourcekey="ASGridBoundColumnResource44">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RC" HeaderTooltip="Reason Code" DataField="ReasonCode"
                        UniqueName="ReasonCode" ASFormat="StaticString" SortExpression="ReasonCode" meta:resourcekey="ASGridBoundColumnResource45">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RC" HeaderTooltip="Reason Code" DataField="ReasonCodeDescription" Visible="false"
                        UniqueName="ReasonCodeDescription" ASFormat="StaticString" SortExpression="ReasonCodeDescription" meta:resourcekey="ASGridBoundColumnResource45">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Amount" HeaderTooltip="Transaction Amount"
                        DataField="TransactionAmount" UniqueName="TransactionAmount" ASFormat="Currency"
                        SortExpression="TransactionAmount" ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource46">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>

        <uc:UxExport ID="uxExportTranQualificationTop" runat="server" GridID="uxTranQualificationGrid"
            ShowExcel="true" ShowPDF="true" />
        <tek:RadToolTipManager ID="uxToolTipMana" runat="server" AutoTooltipify="false" meta:resourcekey="uxToolTipManaResource1" />
        <as:ASGrid ID="uxTranQualificationGrid" runat="server" AutoGenerateColumns="false"
            Skin="Default" IsIntruder="false" AllowSorting="True"
            AppendHeaderforPrinter="true" IsAutoExportTemplate="true"
            ASPagingMethod="SPASingleMethod" AllowPaging="True" ShowPageTotal="false" ShowReportTotal="true"
            IntruderSourceName="uxTranQualificationGrid" CssClass="in" meta:resourcekey="uxTranQualificationGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Report Date" HeaderTooltip="Report Date" DataField="ReportDate"
                        UniqueName="ReportDate" ASFormat="Date" SortExpression="ReportDate" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource47">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Date" HeaderTooltip="Transaction Date" DataField="TransactionDate"
                        UniqueName="TransactionDate" ASFormat="Date" SortExpression="TransactionDate"
                        HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource48">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Code" HeaderTooltip="Transaction Code" DataField="TransactionDescription"
                        UniqueName="TransactionCode" ASFormat="StaticString" SortExpression="TransactionDescription" meta:resourcekey="ASGridBoundColumnResource49">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans ID" HeaderTooltip="Transaction ID" DataField="TransactionID" HeaderStyle-Width="130px"
                        UniqueName="TransactionID" ASFormat="StaticString" SortExpression="TransactionID" meta:resourcekey="ASGridBoundColumnResource50">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Qual Code" HeaderTooltip="Qualification Code" DataField="QualificationCode"
                        UniqueName="QualificationCode" ASFormat="StaticString" SortExpression="QualificationCode" meta:resourcekey="ASGridBoundColumnResource51">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Fee Rate" HeaderTooltip="Fee Rate" DataField="FeeRate"
                        UniqueName="FeeRate" ASFormat="StaticString" SortExpression="FeeRate" meta:resourcekey="ASGridBoundColumnResource52">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" HeaderTooltip="Card Number" DataField="AccountNumber" HeaderStyle-Width="140px"
                        UniqueName="AccountNumber" ASFormat="StaticString" SortExpression="PartialCardNumber"
                        Visible="false" meta:resourcekey="ASGridBoundColumnResource53">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Card #" HeaderTooltip="Card Number" DataField="PartialCardNumber" HeaderStyle-Width="140px"
                        ItemStyle-Wrap="false"
                        UniqueName="PartialCardNumber" ASFormat="StaticString" SortExpression="PartialCardNumber" meta:resourcekey="ASGridBoundColumnResource54">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle Wrap="False"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Auth Date" HeaderTooltip="Authorization Date" DataField="AuthorizationDate"
                        UniqueName="AuthorizationDate" ASFormat="StaticString" SortExpression="AuthorizationDate" meta:resourcekey="ASGridBoundColumnResource55">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Auth #" HeaderTooltip="Authorization Number" DataField="AuthorizationNumber"
                        UniqueName="AuthNumber" ASFormat="StaticString" SortExpression="AuthorizationNumber" meta:resourcekey="ASGridBoundColumnResource56">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RC" HeaderTooltip="Reason Code" DataField="ReasonCode"
                        UniqueName="ReasonCode" ASFormat="StaticString" SortExpression="ReasonCode" meta:resourcekey="ASGridBoundColumnResource57">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Auth Amount" HeaderTooltip="Authorization Amount"
                        DataField="AuthorizationAmount"
                        UniqueName="AuthorizationAmount" ASFormat="Currency" SortExpression="AuthorizationAmount" meta:resourcekey="ASGridBoundColumnResource58">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Trans Amount" HeaderTooltip="Transaction Amount"
                        DataField="TransactionAmount"
                        UniqueName="TransactionAmount" ASFormat="Currency" SortExpression="TransactionAmount"
                        ASIsTotalColumn="true" meta:resourcekey="ASGridBoundColumnResource59">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>

    </as:PlaceHolder>
    <as:RadCodeBlock ID="ScriptManagement" runat="server">
        <script type="text/javascript">
            var uxDrilldownGridID = '<%= uxDrilldownGrid.ClientID %>';
            var uxBatchMerchantGridID = '<%= uxBatchMerchantGrid.ClientID %>';
            var BH_js_BankCard = '<%= GetLocalResourceObject("BatchHistory_js_Settled").ToString()%>';
            var BH_js_NonBankCard = '<%= GetLocalResourceObject("BatchHistory_js_NonSettled").ToString()%>';
            var BH_js_Total = '<%= GetLocalResourceObject("BatchHistory_js_Total").ToString()%>';
            var colCount = <%= getBatchMerchantGridColumns(uxBatchMerchantGrid.Columns.Cast<GridColumn>().Where(c => c.Visible == true).Count()) %>;
            var isEnableEntityName = '<%= uxDrilldownGrid.Columns.FindByUniqueName("EntityName").Visible  %>';
        </script>
        <script src="<%=ResolveUrl("~") %>res/js/BatchHistory.js" type="text/javascript"></script>
    </as:RadCodeBlock>
</asp:Content>
