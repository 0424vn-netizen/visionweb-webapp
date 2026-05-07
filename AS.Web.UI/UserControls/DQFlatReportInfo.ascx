<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DQFlatReportInfo.ascx.cs"
    Inherits="UserControls_DQFlatReportInfo" %>


<!--Merchant Information-->
<h2 class="grid-title inline-block" data-toggle="collapse" data-target="#<%# uxMerchantPanel.ClientID %>">
    <span class="text-muted">
        <asp:Literal ID="Literal1" runat="server" meta:resourcekey="litGridTitle2" Text="Statistics"></asp:Literal></span>

</h2>
<asp:Panel ID="uxMerchantPanel" runat="server" CssClass="in">
    <asp:Repeater ID="uxMerchantInfo" runat="server" OnItemDataBound="uxMerchantInfo_ItemDataBound">
        <ItemTemplate>
            <%--<h2 class="text-muted">
                <as:Literal ID="litGridSubTitle2" runat="server" meta:resourcekey="litGridTitle2" />
        </h2>--%>
            <table class="ASTable mif-table detection-queue">
                <tr class="Row">
                    <td>
                        <div class="inline-block">
                            <input runat="server" type="checkbox" id="chkMerchantWorked" value='<%# Eval("MerchantNumber") %>'
                                onclick="ChangeMerchantWorked(this);" checked='<%# Convert.ToBoolean(Eval("Worked")) %>' />
                            &nbsp;<strong class="valign-bottom"><as:Literal ID="txtMerchantNumber" runat="server" meta:resourcekey="txtMerchantNumberResource1"></as:Literal></strong>
                        </div>
                    </td>
                    <td>
                        <div class="inline-block" id="colRQColumn" runat="server">
                            <input type="checkbox" id="chkItemCV" runat="server" title="Checkbox to assign" />
                            <asp:Label ID="Label2" CssClass="control-inline chk-item-cv" runat="server">RQ</asp:Label>
                        </div>
                    </td>
                    <td colspan="2">
                        <div class="inline-block">
                            <strong><%#Eval("MerchantName") %></strong>
                        </div>
                        <div class="inline-block text-right pull-right">
                            <strong class="red"><%#Eval("Watch")%></strong>
                        </div>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderApprDate" Text="Appr. Date:" meta:resourcekey="lituxMerchantInfoHeaderApprDateResource1" />
                    </td>
                    <td>
                        <%#ConvertDate(Eval("ApprovalDate"))%>
                    </td>
                    <td class="heading">
                        <%# (Eval("HierarchyName") != null && String.IsNullOrEmpty(Eval("HierarchyName").ToString())) ? "" : (Eval("HierarchyName") + ":")%>
                    </td>
                    <td colspan="3">
                        <%#Eval("HierarchyValue")%>
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderChainNO" Text="Chain #:" meta:resourcekey="lituxMerchantInfoHeaderChainNOResource1" />
                    </td>
                    <td>
                        <%#Eval("ChainNumber")%>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderCityState" Text="City/State:" meta:resourcekey="lituxMerchantInfoHeaderCityStateResource1" />
                    </td>
                    <td>
                        <%#Eval("CityState")%>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderStatus" Text="Status:" meta:resourcekey="lituxMerchantInfoHeaderStatusResource1" />
                    </td>
                    <td>
                        <%#Eval("Status")%>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderAuth" Text="Auth $, #, %:" meta:resourcekey="lituxMerchantInfoHeaderAuthResource1" />
                    </td>
                    <td class="text-left">
                        <asp:Label ID="uxTodayAuthorizationVolume" runat="server" meta:resourcekey="uxTodayAuthorizationVolumeResource1"></asp:Label>
                    </td>
                    <td class="text-left">
                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("TodayAuthorizationCount"), 0)%>
                    </td>
                    <td class="text-left">
                        <%#AS.Common.Formater.FormatData.FormatPercent(Eval("TodayAuthorizationPercent"), 2)%>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderTodayVol" Text="Today's Vol:" meta:resourcekey="lituxMerchantInfoHeaderTodayVolResource1" />
                    </td>
                    <td>
                        <asp:Label ID="uxTodayVolume" runat="server" meta:resourcekey="uxTodayVolumeResource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Panel ID="Panel2" runat="server" ToolTip="Average Daily Volume" meta:resourcekey="lituxMerchantInfoHeaderAvgDailyVolResource2">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderAvgDailyVol" Text="Avg DailyVol:" meta:resourcekey="lituxMerchantInfoHeaderAvgDailyVolResource1" />
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Label ID="uxAVGDailyVolume" runat="server" meta:resourcekey="uxAVGDailyVolumeResource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderProfile" Text="Profile:" meta:resourcekey="lituxMerchantInfoHeaderProfileResource1" />
                    </td>
                    <td>
                        <asp:Label ID="lblProfile" runat="server" Text="Label" meta:resourcekey="lblProfileResource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderAuthDecl" Text="Auth Decl%:" meta:resourcekey="lituxMerchantInfoHeaderAuthDeclResource1" />
                    </td>
                    <td colspan="3">
                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("DeclinedAuthorizationPercent"), 2)%>%
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderTodayAvgTkt" Text="Today's Avg Tkt:" meta:resourcekey="lituxMerchantInfoHeaderTodayAvgTktResource1" />
                    </td>
                    <td>
                        <asp:Label ID="uxTodayAverageTicket" runat="server" meta:resourcekey="uxTodayAverageTicketResource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Panel runat="server" ToolTip="Expected Average Ticket" meta:resourcekey="lituxMerchantInfoHeaderExpAvgTktResource2">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpAvgTkt" Text="Exp AvgTkt:" meta:resourcekey="lituxMerchantInfoHeaderExpAvgTktResource1" />
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Label ID="uxExpectedAverageTicket" runat="server" meta:resourcekey="uxExpectedAverageTicketResource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Panel ID="Panel3" runat="server" ToolTip="BackEnd Processor" meta:resourcekey="lituxMerchantInfoHeaderBEProcResource2">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderBEProc" Text="BE Proc:" meta:resourcekey="lituxMerchantInfoHeaderBEProcResource1" />
                        </asp:Panel>
                    </td>
                    <td>
                        <%#Eval("BackEndProcessor")%>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Panel ID="Panel4" runat="server" ToolTip="Repeat Authorization Count" meta:resourcekey="lituxMerchantInfoHeaderNoRptAuthResource2">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderNoRptAuth" Text="# Rpt Auth:" meta:resourcekey="lituxMerchantInfoHeaderNoRptAuthResource1" />
                        </asp:Panel>
                    </td>
                    <td colspan="3">
                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("RepeatAuthorizationCount"), 0)%>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMTDVol" Text="MTD Vol:" meta:resourcekey="lituxMerchantInfoHeaderMTDVolResource1" />
                    </td>
                    <td>
                        <asp:Label ID="uxMTDVolume" runat="server" meta:resourcekey="uxMTDVolumeResource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Panel ID="Panel5" runat="server" ToolTip="Expected Monthly Volume" meta:resourcekey="lituxMerchantInfoHeaderExpMVResource2">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpMV" Text="Exp MV:" meta:resourcekey="lituxMerchantInfoHeaderExpMVResource1" />
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Label ID="uxExpectedMonthlyVolume" runat="server" meta:resourcekey="uxExpectedMonthlyVolumeResource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Panel ID="Panel6" runat="server" ToolTip="Monthly Volume Last Month" meta:resourcekey="lituxMerchantInfoHeaderMv1Resource2">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMv1" Text="MV-1:" meta:resourcekey="lituxMerchantInfoHeaderMv1Resource1" />
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Label ID="uxMV1" runat="server" meta:resourcekey="uxMV1Resource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderSalesRtn" Text="Sales, Rtns:" meta:resourcekey="lituxMerchantInfoHeaderSalesRtnResource1" />
                    </td>
                    <td class="text-left">
                        <asp:Label ID="uxTodaySaleAmount" runat="server" meta:resourcekey="uxTodaySaleAmountResource1"></asp:Label>
                    </td>
                    <td colspan="2" class="text-left">
                        <asp:Label ID="uxTodayReturnAmount" runat="server" meta:resourcekey="uxTodayReturnAmountResource1"></asp:Label>
                    </td>

                </tr>
                <tr class="AltRow">
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderYTDVol" Text="YTD Vol:" meta:resourcekey="lituxMerchantInfoHeaderYTDVolResource1" />
                    </td>
                    <td>
                        <asp:Label ID="uxYTDVolume" runat="server" meta:resourcekey="uxYTDVolumeResource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Panel ID="Panel8" runat="server" ToolTip="Expected Yearly Volume" meta:resourcekey="lituxMerchantInfoHeaderExpYVResource2">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpYV" Text="Exp YV:" meta:resourcekey="lituxMerchantInfoHeaderExpYVResource1" />
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Label ID="uxExpectedYearlyVolume" runat="server" meta:resourcekey="uxExpectedYearlyVolumeResource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Panel ID="Panel7" runat="server" ToolTip="Monthly Volume 2 Months Ago" meta:resourcekey="lituxMerchantInfoHeaderMV2Resource2">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMV2" Text="MV-2:" meta:resourcekey="lituxMerchantInfoHeaderMV2Resource1" />
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Label ID="uxMV2" runat="server" meta:resourcekey="uxMV2Resource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderExpVSActKeyed" Text="Exp vs Act%Keyed:" meta:resourcekey="lituxMerchantInfoHeaderExpVSActKeyedResource1" />
                    </td>
                    <td class="text-left">
                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("ExpectedPercentSWP"), 0)%>%
                    </td>
                    <td colspan="2" class="text-left">
                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("KeyPercent"), 0)%>%
                    </td>

                </tr>
                <tr class="Row">
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderSICCode" Text="SIC Code:" meta:resourcekey="lituxMerchantInfoHeaderSICCodeResource1" />
                    </td>
                    <td colspan="3">
                        <%#Eval("SIC")%>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Panel ID="Panel1" runat="server" ToolTip="Monthly Volume 3 Months Ago" meta:resourcekey="lituxMerchantInfoHeaderMV3Resource2">
                            <asp:Literal runat="server" ID="lituxMerchantInfoHeaderMV3" Text="MV-3:" meta:resourcekey="lituxMerchantInfoHeaderMV3Resource1" />
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Label ID="uxMV3" runat="server" meta:resourcekey="uxMV3Resource1"></asp:Label>
                    </td>
                    <td class="heading text-nowrap">
                        <asp:Literal runat="server" ID="lituxMerchantInfoHeaderRiskScore" Text="Risk Score:" meta:resourcekey="lituxMerchantInfoHeaderRiskScoreResource1" />
                    </td>
                    <td colspan="3">
                        <%#AS.Common.Formater.FormatData.FormatNumber(Eval("RiskScore"), 0)%>
                    </td>

                </tr>
            </table>
        </ItemTemplate>

    </asp:Repeater>
</asp:Panel>
<!--Barometer Report-->
<h2 class="text-muted">
    <span class="text-muted grid-toggle"
        onclick="javascript:toggle('<%=barometer.ClientID%>', this)"><%#GetLocalResourceObject("litHeaderBarometerReportResource1.Text") %></span>

    <span class="info-link" onclick="return OpenInstanceWindow('<%#buildUrlImage(Eval("MerchantNumber").ToString()) %>','DQWindow');"></span>


</h2>
<div id="barometer" runat="server">
    <as:ASGrid ID="uxBarometerGrid" runat="server" GridLines="None" AllowPaging="false" XOverFlowable="false"
        ShowPageTotal="false" ShowReportTotal="false" ShowFooter="false" ItemStyle-Width="100px"
        AllowSorting="false" AutoGenerateColumns="false" OnItemDataBound="uxBarometerGrid_ItemDataBound" CssClass="in" meta:resourcekey="uxBarometerGridResource1">
        <MasterTableView TableLayout="Auto">
            <Columns>
                <as:ASGridBoundColumn HeaderText="BA" DataField="BusinessAge" UniqueName="BusinessAge"
                    ItemStyle-CssClass="item2"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Business Age" meta:resourcekey="ASGridBoundColumnResource0">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="RS" DataField="RiskScore" UniqueName="RiskScore"
                    ItemStyle-CssClass="item2"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Risk Score" ASFormat="Integer"
                    meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True" CssClass="item2"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="V%" DataField="VolumePercent" UniqueName="VolumePercent"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Volume %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="CV%" DataField="ContractualVolume" UniqueName="ContractualVolume"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Contractual Volume %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResourceContractualVolume">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True" Width="120px"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>



                <as:ASGridBoundColumn HeaderText="AT%" DataField="AverageTicketPercent" UniqueName="AverageTicketPercent"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Average Ticket %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="A%" DataField="AuthorizationPercent" UniqueName="AuthorizationPercent"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Authorization %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="DA%" DataField="DeclinedAuthorizationPercent" UniqueName="DeclinedAuthorizationPercent"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Declined Authorization %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="#RA" DataField="RepeatAuthorizationCount" UniqueName="RepeatAuthorizationCount"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Repeat Authorization Count" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="FC" DataField="TodayForeignCardCount" UniqueName="TodayForeignCardCount"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Today's Foreign Card Count" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="K%" DataField="KeyPercent" UniqueName="KeyPercent"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Keyed Percent" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="=%" DataField="EvenDollarTransactionPercent" UniqueName="EvenDollarTransactionPercent"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Even Dollar Transaction %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="DD%" DataField="DuplicateDollarTransactionPercent"
                    UniqueName="DuplicateDollarTransactionPercent" HeaderStyle-HorizontalAlign="Right"
                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true" ItemStyle-Wrap="true"
                    HeaderTooltip="Duplicate Dollar Transaction %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource10">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <tek:GridBoundColumn HeaderText="DB" DataField="DuplicateBin" UniqueName="DuplicateBin"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Duplicate Bin" meta:resourcekey="GridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </tek:GridBoundColumn>
                <as:ASGridBoundColumn HeaderText="#-" DataField="NegativeBatchCount" UniqueName="NegativeBatchCount"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Negative Batch Count" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="#0" DataField="ZeroBatchCount" UniqueName="ZeroBatchCount"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Zero Batch Count" meta:resourcekey="ASGridBoundColumnResource12">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="TV" DataField="TodayVolume" UniqueName="TodayVolume"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Today's Total Volume" meta:resourcekey="ASGridBoundColumnResource13">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="RV$" DataField="TodayFirstTimeRetrievalVolume" UniqueName="TodayFirstTimeRetrievalVolume"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Today's First Time Retrieval Volume" meta:resourcekey="ASGridBoundColumnResource14">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="CB" DataField="TodayChargebackVolume" UniqueName="TodayChargebackVolume"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Today's Chargeback Volume" meta:resourcekey="ASGridBoundColumnResource15">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="R%" DataField="ReturnPercent" UniqueName="ReturnPercent"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="false" HeaderTooltip="Return %" ASFormat="Percentage" meta:resourcekey="ASGridBoundColumnResource16">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="False"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="MT$" DataField="TodayHighestTransactionAmount"
                    UniqueName="TodayHighestTransactionAmount" HeaderStyle-HorizontalAlign="Right"
                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true" ItemStyle-Wrap="true"
                    HeaderTooltip="Today's Highest Transaction Amount" meta:resourcekey="ASGridBoundColumnResource17">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="#T" DataField="TodayTransactionCount" UniqueName="TodayTransactionCount"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Today's Transaction Count" meta:resourcekey="ASGridBoundColumnResource18">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="#B" DataField="TodayBatchCount" UniqueName="TodayBatchCount"
                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Today's Batch Count" meta:resourcekey="ASGridBoundColumnResource19">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="SC" DataField="SingleCardTransToday" UniqueName="SingleCardTransToday"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true"
                    ItemStyle-Wrap="true" HeaderTooltip="Similar Card Transactions Today" meta:resourcekey="ASGridBoundColumnResource20">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="SIC" DataField="SICCode" UniqueName="SIC" HeaderStyle-HorizontalAlign="Right"
                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Wrap="true" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource21">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Right" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Profile" DataField="ProfileDescription" UniqueName="ProfileDescription"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left" Visible="true"
                    HeaderStyle-Wrap="true" ItemStyle-Wrap="true" HeaderTooltip="Profile Description" meta:resourcekey="ASGridBoundColumnResource22">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Wrap="True"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Left" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
</div>

<!--Transactions(Today); Chargebacks(90days); Daily FC Analysis-->


<h2 class="text-muted">
    <span class="text-muted grid-toggle"
        onclick="javascript:toggle('<%=chargebacks.ClientID%>', this)"><%# GetLocalResourceObject("litHeaderChargeback90daysResource1.Text") %></span>
</h2>
<div id="chargebacks" runat="server">

    <as:ASGrid ID="uxChargeback" runat="server" AutoGenerateColumns="false" ShowPageTotal="false"
        ShowReportTotal="true" ShowFooter="true" AllowPaging="false" AllowCustomPaging="false" XOverFlowable="false" AllowSorting="false"
        OnItemDataBound="uxChargeback_ItemDataBound" OnPreRender="uxChargeback_PreRender" CssClass="in" meta:resourcekey="uxChargebackResource1">
        <%-- <ClientSettings AllowExpandCollapse="true" AllowRowHide="true">
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="300px" />
                </ClientSettings>--%>
        <MasterTableView Width="100%">
            <Columns>
                <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Acct #"
                    HeaderTooltip="Card #" AllowSorting="false" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource36">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="PartialAccountNumber" UniqueName="PartialAccountNumber"
                    ItemStyle-Wrap="false"
                    HeaderText="Acct #" HeaderTooltip="Card #" AllowSorting="false" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" Visible="false" meta:resourcekey="ASGridBoundColumnResource37">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center" Wrap="False"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="CardType" UniqueName="CardType" HeaderText="CT"
                    HeaderTooltip="Card Type" AllowSorting="false" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource38">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="ReportDate" UniqueName="ReportDate" HeaderText="Rep Dt"
                    HeaderTooltip="Report Date" ASFormat="Date" AllowSorting="false" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource39">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="TransactionDate" UniqueName="TransactionDate" HeaderText="Trans Dt"
                    HeaderTooltip="Transaction Date" ASFormat="Date" AllowSorting="false" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource40">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="ReasonCode" UniqueName="ReasonCodeDesc" HeaderText="RC Desc"
                    ItemStyle-Wrap="true" HeaderTooltip="Reason Code" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Left" meta:resourcekey="ASGridBoundColumnResource41">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Left" Wrap="True"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="Keyed" UniqueName="Keyed" HeaderText="Key" HeaderTooltip="Keyed"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource42">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <FooterStyle HorizontalAlign="Center"></FooterStyle>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="TransactionAmount" UniqueName="TransactionAmount"
                    HeaderText="Trans Amt" HeaderTooltip="Transaction Amount" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource43">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <FooterStyle HorizontalAlign="Right"></FooterStyle>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>

</div>


<h2 class="text-muted">
    <span class="text-muted grid-toggle"
        onclick="javascript:toggle('<%=transactions.ClientID%>', this)"><%# GetLocalResourceObject("litHeaderTransactionTodayResource1.Text") %></span>
</h2>
<div id="transactions" runat="server">

    <as:ASGrid ID="uxReportGridTransactions" runat="server" AutoGenerateColumns="false"
        ShowFooter="true" AllowPaging="false" AllowCustomPaging="false" XOverFlowable="true" AllowSorting="false"
        ShowReportTotal="true" OnItemDataBound="uxReportGridTransactions_ItemDataBound" HeaderStyle-Width="80px"
        OnNeedDataSource="uxReportGridTransactions_NeedDataSource" OnPreRender="uxReportGridTransactions_PreRender" CssClass="grid-transaction in" meta:resourcekey="uxReportGridTransactionsResource1">
        <%--  <ClientSettings AllowExpandCollapse="true" AllowRowHide="true">
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="500px" />
            </ClientSettings>--%>
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn DataField="AccountNumber" UniqueName="AccountNumber" HeaderText="Acct #" HeaderStyle-Width="160px"
                    HeaderTooltip="Card #" AllowSorting="false" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" Visible="false" meta:resourcekey="ASGridBoundColumnResource23">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="PartialAccountNumber" UniqueName="PartialAccountNumber" HeaderStyle-Width="160px"
                    ItemStyle-Wrap="false"
                    HeaderText="Acct #" HeaderTooltip="Card #" AllowSorting="false" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource24">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center" Wrap="False"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="CardType" UniqueName="CardType" HeaderText="CT" AllowSorting="false"
                    HeaderTooltip="Card Type" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-Width="60px" meta:resourcekey="ASGridBoundColumnResource25">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>


                <as:ASGridBoundColumn DataField="CountryCode" UniqueName="CountryCode" HeaderText="Ctry" HeaderStyle-Width="60px" AllowSorting="false"
                    HeaderTooltip="Country" AllowEncodeOnExporting="False" AllowKeepWhiteSpaces="False" ASDefaultNullValue="N/A" ASFormat="Auto" ASIsTotalColumn="False" ASTotalFormat="Auto"
                    ASTrueFalseText="Yes/No" FilterControlAltText="Filter CardType column" IsResetTotal="False" meta:resourcekey="ASGridBoundColumnResource26">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </as:ASGridBoundColumn>


                <as:ASGridBoundColumn DataField="FileSource" UniqueName="FileSource" HeaderText="Source" HeaderStyle-Width="200px" ItemStyle-CssClass="word-wrapped" AllowSorting="false"
                    HeaderTooltip="File Source" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource27">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="TransactionDate" UniqueName="TransactionDate" HeaderText="Trans Dt"
                    HeaderTooltip="Transaction Date" ASFormat="Date" AllowSorting="false" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" meta:resourcekey="ASGridBoundColumnResource28">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="TransactionTime" UniqueName="TransactionTime" HeaderText="Time"
                    HeaderTooltip="Transaction Time" AllowSorting="false" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource29">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="DupeCount" UniqueName="DupeCount" AllowSorting="false"
                    HeaderText="Dupe" HeaderTooltip="Last 30 Days Count" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource30">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="AuthorizationNumber" UniqueName="AuthorizationNumber" AllowSorting="false"
                    HeaderText="Auth #" HeaderTooltip="Authorization #" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource31">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="Keyed" UniqueName="Keyed" HeaderText="Key" HeaderTooltip="Keyed" HeaderStyle-Width="60px" AllowSorting="false"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource32">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <FooterStyle HorizontalAlign="Center"></FooterStyle>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="TransactionDescription" UniqueName="TransactionType" AllowSorting="false"
                    HeaderText="Trans Type" HeaderTooltip="Transaction Type" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource33">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="MatchCode" UniqueName="MatchCode" HeaderText="Match" HeaderTooltip="Return Match" AllowSorting="false"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" meta:resourcekey="ASGridBoundColumnResource34">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="TransactionAmount" UniqueName="TransactionAmount" AllowSorting="false"
                    HeaderText="Trans Amt" HeaderTooltip="Transaction Amount" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource35">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <FooterStyle HorizontalAlign="Right"></FooterStyle>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="DuplicateFlag" AllowSorting="false" UniqueName="DuplicateFlag" Visible="false" meta:resourcekey="ASGridBoundColumnResource50">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="HighestTransactionAmountFlag" UniqueName="HighestTransactionAmountFlag" AllowSorting="false"
                    Visible="false" meta:resourcekey="ASGridBoundColumnResource51">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>

</div>



<div class="display-none">
    <as:HiddenField ID="uxAccountValue" runat="server" />
    <as:Button ID="uxAccount" runat="server" IsStandardButton="True" OnClick="uxAccount_Click" meta:resourcekey="uxAccountResource1" />

</div>

<script type="text/javascript" language="javascript">

    function toggle(divID, link) {
        var obj = document.getElementById(divID);
        if (obj.style.display == '') {
            obj.style.display = 'none';
        }
        else {
            obj.style.display = '';
        }
    }

    function AccountClick(param) {
        $get("<%=uxAccountValue.ClientID %>").value = param;
        $get("<%=uxAccount.ClientID %>").click();
    }
</script>

