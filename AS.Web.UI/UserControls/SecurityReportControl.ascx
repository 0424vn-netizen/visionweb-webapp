<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SecurityReportControl.ascx.cs" Inherits="UserControls_SecurityReportControl" %>


<asp:PlaceHolder runat="server" ID="phlStatisticsReport">
    <div class="row">
        <div class="col-xs-12">
            <h2 class="grid-title inline-block" data-toggle="collapse" data-target="#cidStatisticsGrid">
                <span class="text-muted">
                    <asp:Literal runat="server" ID="Literal5" Text="Statistics" meta:resourcekey="StatisticsReport" /></span>
            </h2>
        </div>
    </div>
    <div id="cidStatisticsGrid" class="in">
        <asp:Repeater ID="uxStatisticsReport" runat="server">
            <ItemTemplate>
                <table class="ASTable">
                    <colgroup>
                        <col />
                        <col />
                        <col />
                        <col />
                        <col />
                        <col />
                        <col class="w-15" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr class="Row">
                            <td colspan="4" class="heading valign-middle">
                                <div class="row">
                                    <div class="col-xs-4">
                                        <as:Literal ID="txtMerchantNumber" runat="server" meta:resourcekey="txtMerchantNumberResource1"></as:Literal>
                                    </div>
                                    <div class="col-xs-6">
                                        <span style='color: <%# MerchantNameColor(Eval("ActiveDayFlag")) %>'>
                                            <%#Eval("MerchantName") %></span>
                                        <asp:ImageButton ID="uxCwinButton" ImageUrl="~/res/img/Change_Main_Window16.png"
                                            runat="server" OnClick="uxCwinButton_Click" meta:resourcekey="uxCwinButtonResource1" />
                                    </div>
                                    <div class="col-xs-2 negative">
                                        <%#Eval("Watch")%>
                                    </div>
                                </div>
                            </td>
                            <td class="heading valign-middle">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderApprDate" Text="Appr. Date:" meta:resourcekey="lituxMerchantInfoHeaderApprDateResource1" /></td>
                            <td>
                                <%#ConvertDate(Eval("ApprovalDate"))%>
                            </td>
                            <td class="heading"><%#Eval("HierarchyName")==""? "":Eval("HierarchyName")+":" %></td>
                            <td>
                                <%#Eval("HierarchyValue")%>
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderChainNO" Text="Chain #:" meta:resourcekey="lituxMerchantInfoHeaderChainNOResource1" />

                            </td>
                            <td>
                                <%#Eval("ChainNumber")%>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderCityState" Text="City/State:" meta:resourcekey="lituxMerchantInfoHeaderCityStateResource1" />

                            </td>
                            <td>
                                <%#Eval("CityState")%>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderStatus" Text="Status:" meta:resourcekey="lituxMerchantInfoHeaderStatusResource1" />

                            </td>
                            <td>
                                <%#Eval("Status")%>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderAuth" Text="Auth $, #, %:" meta:resourcekey="lituxMerchantInfoHeaderAuthResource1" />

                            </td>
                            <td>
                                <div class="row">
                                    <div class="col-xs-5">
                                        <asp:Label ID="uxTodayAuthorizationVolume" runat="server" meta:resourcekey="uxTodayAuthorizationVolumeResource1"></asp:Label>
                                    </div>
                                    <div class="col-xs-3">
                                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("TodayAuthorizationCount"), 0)%>
                                    </div>
                                    <div class="col-xs-4">
                                        <%#AS.Common.Formater.FormatData.FormatPercent(Eval("TodayAuthorizationPercent"), 2)%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderTodayVol" Text="Today's Vol:" meta:resourcekey="lituxMerchantInfoHeaderTodayVolResource1" /></td>
                            <td>
                                <asp:Label ID="uxTodayVolume" runat="server" meta:resourcekey="uxTodayVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading" title="Average Daily Volume">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderAvgDailyVol" Text="Avg DailyVol:" meta:resourcekey="lituxMerchantInfoHeaderAvgDailyVolResource1" /></td>
                            <td>
                                <asp:Label ID="uxAVGDailyVolume" runat="server" meta:resourcekey="uxAVGDailyVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderProfile" Text="Profile:" meta:resourcekey="lituxMerchantInfoHeaderProfileResource1" /></td>
                            <td>
                                <asp:Label ID="lblProfile" runat="server" Text="Label" meta:resourcekey="lblProfileResource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderAuthDecl" Text="Auth Decl%:" meta:resourcekey="lituxMerchantInfoHeaderAuthDeclResource1" /></td>
                            <td>
                                <%#AS.Common.Formater.FormatData.FormatNumber(Eval("DeclinedAuthorizationPercent"), 2)%>%
                            </td>
                        </tr>
                        <tr class="AltRow">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderTodayAvgTkt" Text="Today's Avg Tkt:" meta:resourcekey="lituxMerchantInfoHeaderTodayAvgTktResource1" /></td>
                            <td>
                                <asp:Label ID="uxTodayAverageTicket" runat="server" meta:resourcekey="uxTodayAverageTicketResource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Panel ID="Panel1" runat="server" ToolTip="Expected Average Ticket" meta:resourcekey="lituxMerchantInfoHeaderExpAvgTktResource2">
                                    <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpAvgTkt" Text="Exp AvgTkt:" meta:resourcekey="lituxMerchantInfoHeaderExpAvgTktResource1" />
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Label ID="uxExpectedAverageTicket" runat="server" meta:resourcekey="uxExpectedAverageTicketResource1"></asp:Label>
                            </td>
                            <td class="heading" title="BackEnd Processor">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderBEProc" Text="BE Proc:" meta:resourcekey="lituxMerchantInfoHeaderBEProcResource1" /></td>
                            <td>
                                <%#Eval("BackEndProcessor")%>
                            </td>
                            <td class="heading" title="Repeat Authorization Count">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderNoRptAuth" Text="# Rpt Auth:" meta:resourcekey="lituxMerchantInfoHeaderNoRptAuthResource1" /></td>
                            <td>
                                <%#AS.Common.Formater.FormatData.FormatNumber(Eval("RepeatAuthorizationCount"), 0)%>
                            </td>
                        </tr>
                        <tr class="Row">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMTDVol" Text="MTD Vol:" meta:resourcekey="lituxMerchantInfoHeaderMTDVolResource1" /></td>
                            <td>
                                <asp:Label ID="uxMTDVolume" runat="server" meta:resourcekey="uxMTDVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading" title="Expected Monthly Volume">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpMV" Text="Exp MV:" meta:resourcekey="lituxMerchantInfoHeaderExpMVResource1" /></td>
                            <td>
                                <asp:Label ID="uxExpectedMonthlyVolume" runat="server" meta:resourcekey="uxExpectedMonthlyVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading" title="Monthly Volume Last Month">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMv1" Text="MV-1:" meta:resourcekey="lituxMerchantInfoHeaderMv1Resource1" /></td>
                            <td>
                                <asp:Label ID="uxMV1" runat="server" meta:resourcekey="uxMV1Resource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderSalesRtn" Text="Sales, Rtns:" meta:resourcekey="lituxMerchantInfoHeaderSalesRtnResource1" /></td>
                            <td>
                                <div class="row">
                                    <div class="col-xs-5">
                                        <asp:Label ID="uxTodaySaleAmount" runat="server" meta:resourcekey="uxTodaySaleAmountResource1"></asp:Label>
                                    </div>
                                    <div class="col-xs-7">
                                        <asp:Label ID="uxTodayReturnAmount" runat="server" meta:resourcekey="uxTodayReturnAmountResource1"></asp:Label>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr class="">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderYTDVol" Text="YTD Vol:" meta:resourcekey="lituxMerchantInfoHeaderYTDVolResource1" /></td>
                            <td>
                                <asp:Label ID="uxYTDVolume" runat="server" meta:resourcekey="uxYTDVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading" title="Expected Yearly Volume">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpYV" Text="Exp YV:" meta:resourcekey="lituxMerchantInfoHeaderExpYVResource1" /></td>
                            <td>
                                <asp:Label ID="uxExpectedYearlyVolume" runat="server" meta:resourcekey="uxExpectedYearlyVolumeResource1"></asp:Label>
                            </td>
                            <td class="heading" title="Monthly Volume 2 Months Ago">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMV2" Text="MV-2:" meta:resourcekey="lituxMerchantInfoHeaderMV2Resource1" /></td>
                            <td>
                                <asp:Label ID="uxMV2" runat="server" meta:resourcekey="uxMV2Resource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpVSActKeyed" Text="Exp vs Act%Keyed:" meta:resourcekey="lituxMerchantInfoHeaderExpVSActKeyedResource1" />
                            </td>
                            <td>
                                <div class="row">
                                    <div class="col-xs-5">
                                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("ExpectedPercentSWP"), 0)%>%
                                    </div>
                                    <div class="col-xs-7">
                                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("KeyPercent"), 0)%>%
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr class="">
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderSICCode" Text="SIC Code:" meta:resourcekey="lituxMerchantInfoHeaderSICCodeResource1" /></td>
                            <td colspan="3">
                                <%#Eval("SIC")%>
                            </td>
                            <td class="heading" title="Monthly Volume 3 Months Ago">MV-3:</td>
                            <td>
                                <asp:Label ID="uxMV3" runat="server" meta:resourcekey="uxMV3Resource1"></asp:Label>
                            </td>
                            <td class="heading">
                                <asp:Literal runat="server" ID="lituxMerchantInfoHeaderRiskScore" Text="Risk Score:" meta:resourcekey="lituxMerchantInfoHeaderRiskScoreResource1" /></td>
                            <td>
                                <%#AS.Common.Formater.FormatData.FormatNumber(Eval("RiskScore"), 0)%>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:PlaceHolder>
<%--<as:Button runat="server" ID="uxShowBarometer" OnClick="uxShowBarometer_Click" CssClass="hide" IsStandardButton="False" meta:resourcekey="uxShowBarometerResource1" />--%>
<as:PlaceHolder ID="uxPlcBarometer" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <h2 class="grid-title inline-block" data-toggle="collapse" data-target="#cidBarometerGrid">
                <span class="text-muted">
                    <asp:Literal runat="server" ID="Literal1" Text="Barometer Report" meta:resourcekey="BarometerReport" /></span>
            </h2>
        </div>
    </div>
    <div id="cidBarometerGrid" class="in">
        <as:ASGrid ID="uxBarometerGrid" runat="server" GridLines="None" AllowPaging="false" XOverFlowable="false"
            AllowSorting="false" AutoGenerateColumns="false" OnItemDataBound="uxBarometerGrid_ItemDataBound"
            Visible="true" CssClass="in" meta:resourcekey="uxBarometerGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="BA" DataField="BusinessAge" UniqueName="BusinessAge"
                        ItemStyle-CssClass="item2"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                        ItemStyle-Wrap="true" HeaderTooltip="Business Age" meta:resourcekey="ASGridBoundColumnResource0">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>                   
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RS" DataField="RiskScore" UniqueName="RiskScore"
                        HeaderTooltip="Risk Score" ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="V%" DataField="VolumePercent" UniqueName="VolumePercent"
                        HeaderTooltip="Volume %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="AT%" DataField="AverageTicketPercent" UniqueName="AverageTicketPercent"
                        HeaderTooltip="Average Ticket %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="A%" DataField="AuthorizationPercent" UniqueName="AuthorizationPercent"
                        HeaderTooltip="Authorization %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource4">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="DA%" DataField="DeclinedAuthorizationPercent" UniqueName="DeclinedAuthorizationPercent"
                        HeaderTooltip="Declined Authorization %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="#RA" DataField="RepeatAuthorizationCount" UniqueName="RepeatAuthorizationCount"
                        HeaderTooltip="Repeat Authorization Count" meta:resourcekey="ASGridBoundColumnResource6">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="FC" DataField="TodayForeignCardCount" UniqueName="TodayForeignCardCount"
                        HeaderTooltip="Today's Foreign Card Count" meta:resourcekey="ASGridBoundColumnResource7">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="K%" DataField="KeyPercent" UniqueName="KeyPercent"
                        HeaderTooltip="Keyed Percent" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource8">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="=%" DataField="EvenDollarTransactionPercent" UniqueName="EvenDollarTransactionPercent"
                        HeaderTooltip="Even Dollar Transaction %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource9">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="DD%" DataField="DuplicateDollarTransactionPercent"
                        UniqueName="DuplicateDollarTransactionPercent"
                        HeaderTooltip="Duplicate Dollar Transaction %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource10">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <tek:GridBoundColumn HeaderText="DB" DataField="DuplicateBin" UniqueName="DuplicateBin"
                        HeaderTooltip="Duplicate Bin" meta:resourcekey="GridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>
                    </tek:GridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="#-" DataField="NegativeBatchCount" UniqueName="NegativeBatchCount"
                        HeaderTooltip="Negative Batch Count" meta:resourcekey="ASGridBoundColumnResource11">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="#0" DataField="ZeroBatchCount" UniqueName="ZeroBatchCount"
                        HeaderTooltip="Zero Batch Count" meta:resourcekey="ASGridBoundColumnResource12">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="TV" DataField="TodayVolume" UniqueName="TodayVolume"
                        HeaderTooltip="Today's Total Volume" meta:resourcekey="ASGridBoundColumnResource13">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="RV" DataField="TodayFirstTimeRetrievalVolume" UniqueName="TodayFirstTimeRetrievalVolume"
                        HeaderTooltip="Today's First Time Retrieval Volume" meta:resourcekey="ASGridBoundColumnResource14">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="CB" DataField="TodayChargebackVolume" UniqueName="TodayChargebackVolume"
                        HeaderTooltip="Today's Chargeback Volume" meta:resourcekey="ASGridBoundColumnResource15">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="R%" DataField="ReturnPercent" UniqueName="ReturnPercent"
                        HeaderTooltip="Return %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource16">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="MT$" DataField="TodayHighestTransactionAmount"
                        UniqueName="TodayHighestTransactionAmount" HeaderTooltip="Today's Highest Transaction Amount" meta:resourcekey="ASGridBoundColumnResource17">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="#T" DataField="TodayTransactionCount" UniqueName="TodayTransactionCount"
                        HeaderTooltip="Today's Transaction Count" meta:resourcekey="ASGridBoundColumnResource18">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="#B" DataField="TodayBatchCount" UniqueName="TodayBatchCount"
                        HeaderTooltip="Today's Batch Count" meta:resourcekey="ASGridBoundColumnResource19">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="SC" DataField="SingleCardTransToday" UniqueName="SingleCardTransToday"
                        HeaderTooltip="Similar Card Transactions Today" meta:resourcekey="ASGridBoundColumnResource20">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="SIC" DataField="SICCode" UniqueName="SIC" meta:resourcekey="ASGridBoundColumnResource21">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Profile" DataField="ProfileDescription" UniqueName="ProfileDescription"
                        HeaderTooltip="Profile Description" meta:resourcekey="ASGridBoundColumnResource22">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
    </div>
</as:PlaceHolder>
<div class="row">
    <div class="col-xs-12">
        <%--<as:Button runat="server" ID="uxShowChargeback" OnClick="uxShowChargeback_Click" CssClass="hide" IsStandardButton="False" meta:resourcekey="uxShowChargebackResource1" />--%>
        <as:Panel ID="uxPanelChargeback" runat="server" meta:resourcekey="uxPanelChargebackResource1">
            <div class="row">
                <div class="col-xs-12">
                    <h2 class="grid-title" data-toggle="collapse" data-target="#cidChargeBackGrid">
                        <span class="text-muted">
                            <asp:Literal runat="server" ID="litHeaderChargeback90days" Text="Chargebacks (90 days)" meta:resourcekey="litHeaderChargeback90daysResource1" /></span>
                    </h2>
                </div>
            </div>
            <div id="cidChargeBackGrid" class="in">
                <asp:PlaceHolder ID="plhuxChargeback" runat="server">
                    <as:ASGrid ID="uxChargeback" runat="server" AutoGenerateColumns="False" ShowPageTotal="False"
                        ShowReportTotal="True" ShowFooter="True" XOverFlowable="false" AllowSorting="false"
                        OnItemDataBound="uxChargeback_ItemDataBound" CssClass="grid-transaction freeze-table-no-pager in freeze-table"
                        meta:resourcekey="uxChargebackResource1">
                        <MasterTableView EnableColumnsViewState="False" NoDetailRecordsText="No data found." NoMasterRecordsText="No data found." DataKeyNames="RecordID,PartialAccountNumber,ReportDate">
                            <Columns> 
                                <as:ASGridBoundColumn DataField="PartialAccountNumber" UniqueName="PartialAccountNumber" ItemStyle-CssClass="border-left" FooterStyle-CssClass="border-left"
                                    HeaderText="Acct #" HeaderTooltip="Card #" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto"
                                    ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter PartialAccountNumber column"
                                    IsResetTotal="False" HeaderStyle-Width="150" meta:resourcekey="ASGridBoundColumnResource37">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="TransactionDate" UniqueName="TransactionDate" HeaderText="Trans Dt"
                                    HeaderTooltip="Transaction Date" ASFormat="Date" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter TransactionDate column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource38">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="ReasonCode" UniqueName="ReasonCodeDesc" HeaderText="RC Desc" HeaderTooltip="Reason Code" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter ReasonCodeDesc column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource39">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="Keyed" UniqueName="Keyed" HeaderText="Key" HeaderTooltip="Keyed" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter Keyed column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource40">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="TransactionAmount" UniqueName="TransactionAmount"
                                    HeaderText="Trans Amt" HeaderTooltip="Transaction Amount" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter TransactionAmount column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource41">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </as:ASGridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </as:ASGrid>
                </asp:PlaceHolder>
            </div>
        </as:Panel>

    </div>
</div>
<div class="row">
    <div class="col-xs-12">
        <%--<as:Button runat="server" ID="uxShowTransaction" OnClick="uxShowTransaction_Click" CssClass="hide" IsStandardButton="False" meta:resourcekey="uxShowTransactionResource1" />--%>
        <as:Panel ID="uxPanelTransaction" runat="server" meta:resourcekey="uxPanelTransactionResource1">
            <div class="row">
                <div class="col-xs-12">
                    <h2 class="grid-title" data-toggle="collapse" data-target="#cidTransGrid">
                        <span class="text-muted">
                            <asp:Literal runat="server" ID="litHeaderTransactionToday" Text="Transactions (Today)" meta:resourcekey="litHeaderTransactionTodayResource1" /></span>
                    </h2>
                </div>
            </div>
            <div id="cidTransGrid" class="in">
                <asp:PlaceHolder ID="plhReportGridTransactions" runat="server">
                    <as:ASGrid ID="uxReportGridTransactions" runat="server" AutoGenerateColumns="False" XOverFlowable="false"
                        ShowPageTotal="False" ShowFooter="True" AllowSorting="False" CssClass=" grid-transaction freeze-table-no-pager in freeze-table"
                        OnItemDataBound="uxReportGridTransactions_ItemDataBound"
                        meta:resourcekey="uxReportGridTransactionsResource1">
                        <MasterTableView EnableColumnsViewState="False" NoDetailRecordsText="No data found." NoMasterRecordsText="No data found." DataKeyNames="RecordID,PartialAccountNumber,ReportDate">
                            <Columns>
                                <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Acct #" Visible="false"
                                    HeaderStyle-Width="150" HeaderTooltip="Card #" AllowSorting="False" AllowEncodeOnExporting="False"
                                    AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No"
                                    FilterControlAltText="Filter AccountNumber column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource23">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="PartialAccountNumber" UniqueName="PartialAccountNumber" HeaderStyle-Width="150" ItemStyle-CssClass="border-left" FooterStyle-CssClass="border-left"
                                    HeaderText="Acct #" HeaderTooltip="Card #" AllowSorting="False" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False"
                                    ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No"
                                    FilterControlAltText="Filter PartialAccountNumber column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource24">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="CardType" UniqueName="CardType" HeaderText="CT"
                                    HeaderTooltip="Card Type" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter CardType column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource25">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn DataField="CountryCode" UniqueName="CountryCode" HeaderText="Ctry"
                                    HeaderTooltip="Country" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter CardType column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource45">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn DataField="FileSource" UniqueName="FileSource" HeaderText="Source" HeaderStyle-Width="200px" ItemStyle-CssClass="word-break"
                                    HeaderTooltip="File Source" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter FileSource column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource26">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="TransactionDate" UniqueName="TransactionDate" HeaderText="Trans Dt"
                                    HeaderTooltip="Transaction Date" ASFormat="Date" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter TransactionDate column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource27">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="TransactionTime" UniqueName="TransactionTime" HeaderText="Time"
                                    HeaderTooltip="Transaction Time" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter TransactionTime column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource28">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn HeaderText="Dupe" DataField="DupeCount" UniqueName="DupeCount"
                                    HeaderTooltip="Last 30 Days Count" ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource46">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn DataField="AuthorizationNumber" UniqueName="AuthorizationNumber"
                                    HeaderText="Auth #" HeaderTooltip="Authorization #" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter AuthorizationNumber column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource29">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="Keyed" UniqueName="Keyed" HeaderText="Key" HeaderTooltip="Keyed" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter Keyed column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource30">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="TransactionDescription" UniqueName="TransactionType"
                                    HeaderText="Trans" HeaderTooltip="Transaction Type" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter TransactionType column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource31">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="MatchCode" UniqueName="MatchCode" HeaderText="Match" HeaderTooltip="Return Match" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter Match column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource32">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="TransactionAmount" UniqueName="TransactionAmount"
                                    HeaderText="Trans Amt" HeaderTooltip="Transaction Amount" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter TransactionAmount column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource33">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="DuplicateFlag" UniqueName="DuplicateFlag" Visible="False" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter DuplicateFlag column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource34">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="HighestTransactionAmountFlag" UniqueName="HighestTransactionAmountFlag"
                                    Visible="False" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto" ASTrueFalseText="Yes/No" FilterControlAltText="Filter HighestTransactionAmountFlag column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource35">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </as:ASGridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </as:ASGrid>
                </asp:PlaceHolder>
            </div>
        </as:Panel>
    </div>

</div>

<as:HiddenField ID="uxAccountValue" runat="server" />
<as:Button ID="uxAccount" runat="server" IsStandardButton="True" OnClick="uxAccount_Click" CssClass="hide" meta:resourcekey="uxAccountResource1" />

<as:HiddenField ID="uxHiddenAccountNumberClick" runat="server" />
<as:Button ID="uxAccountNumberClick" runat="server" IsStandardButton="True" OnClick="uxAccountNumberClick_Click"
    CssClass="hide" meta:resourcekey="uxAccountNumberClickResource1" />
<as:Button ID="uxAccReload" runat="server" IsStandardButton="True" CssClass="hide" meta:resourcekey="uxAccReloadResource1" />



<as:ASRadCodeBlock runat="server" ID="uxRadCodeBlock1">

    <script type="text/javascript">
        function AccountClick(param) {
            $get("<%=uxAccountValue.ClientID %>").value = param;
            $get("<%=uxAccount.ClientID %>").click();
        }
        function ShowPopupAcctNumber(actt) {
            document.getElementById("<%=uxHiddenAccountNumberClick.ClientID %>").value = actt;
                document.getElementById("<%=uxAccountNumberClick.ClientID %>").click();
                return false;
            }
            function openPopupCardWindow(url) {
                openPopupWindow(url, 'CardHistoryWindow');
                return false;
            }

            var rm_SecurityReport_js_TextNotAllowed = '<%= GetLocalResourceObject("rm_SecurityReport_js_TextNotAllowed").ToString() %>';
            var rm_SecurityReport_js_ValidationSpecialCharacter = '<%= GetLocalResourceObject("rm_SecurityReport_js_ValidationSpecialCharacter").ToString() %>';

    </script>
</as:ASRadCodeBlock>
