<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" meta:resourcekey="PageResource2" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxExportTable" Src="~/UserControls/UxExportTable.ascx" TagPrefix="uc" %>

<%@ Register Src="UserControls/MTDChart.ascx" TagName="MTDChart" TagPrefix="uc" %>
<%@ Register Src="UserControls/YTDChart.ascx" TagName="YTDChart" TagPrefix="uc" %>
<%@ Register Src="UserControls/Last12MonthsChart.ascx" TagName="Last12MonthsChart" TagPrefix="uc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <%if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_TAB_MENU_DASHBOARD").ToLower().Equals("true"))
      {%>
    <div class="top-dashboard">
        <h1 class="section-heading"><% =GetLocalResourceObject("Dashboard_aspx_cs_MYDASHBOARD") %> </h1>
        <div class="dashboard-tab">
            <%if (ShowTabByPermission(WebSiteConstants.SEC_PERMISSION_DASHBOARD
                  + "," + WebSiteConstants.SEC_PERMISSION_DASHBOARD_MS
                  + "," + WebSiteConstants.SEC_PERMISSION_CASE_MANAGEMENT_DASHBOARD
                  + "," + WebSiteConstants.SEC_PERMISSION_CASE_MANAGEMENT_DASHBOARD_MS
                  + "," + WebSiteConstants.SEC_PERMISSION_MARGIN_ANALYSIS_DASHBOARD
                  + ",ViewPortfolioDashboard"
                  + "," + WebSiteConstants.SEC_PERMISSION_ALICE_DASHBOARD, 3))
              {%>
            <a class="" href="<% =Dashboard_URL %>/Dashboard/General"><% =GetLocalResourceObject("Dashboard_aspx_cs_General") %></a>
            <% } %>
            <%if (SessionManager.CurrentUserPermissions.Contains("," + WebSiteConstants.SEC_PERMISSION_DASHBOARD + ",")
                  || SessionManager.CurrentUserPermissions.Contains("," + WebSiteConstants.SEC_PERMISSION_DASHBOARD_MS + ","))
              { %>
            <a class="active" href="<%=ResolveUrl("~")  %>Dashboard.aspx"><% =GetLocalResourceObject("Dashboard_aspx_cs_Reporting") %></a>
            <% } %>                     
            <%if (SessionManager.CurrentUserPermissions.Contains("," + WebSiteConstants.SEC_PERMISSION_CASE_MANAGEMENT_DASHBOARD + ",")
                  || SessionManager.CurrentUserPermissions.Contains("," + WebSiteConstants.SEC_PERMISSION_CASE_MANAGEMENT_DASHBOARD_MS + ","))
              { %>
            <a href="<% =Dashboard_URL %>/Dashboard/CaseManagement"><% =GetLocalResourceObject("Dashboard_aspx_cs_CaseManagement") %></a>
            <% } %>
            <%if (SessionManager.CurrentUserPermissions.Contains("," + WebSiteConstants.SEC_PERMISSION_MARGIN_ANALYSIS_DASHBOARD + ","))
              { %>
            <a class="" href="<% =Dashboard_URL %>/Dashboard/MarginAnalysis"><% =GetLocalResourceObject("Dashboard_aspx_cs_MarginAnalysis") %></a>
            <% } %>
            <%if (SessionManager.CurrentUserPermissions.Contains("," + WebSiteConstants.SEC_PERMISSION_ALICE_DASHBOARD + ","))
              { %>
            <a class="" href="<% =Dashboard_URL %>/Dashboard/Boarding"><% =GetLocalResourceObject("Dashboard_aspx_cs_Alice") %></a>
            <% } %>
            <%if (SessionManager.CurrentUserPermissions.Contains(",ViewPortfolioDashboard,"))
              { %>
            <a class="" href="<% =Dashboard_URL %>/Dashboard/RiskPortfolio">PORTFOLIO</a>
            <% } %>
        </div>
    </div>
    <%}
      else
      { %>
    <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Dashboard" HasShowHierarchy="true"
        HasFilteringOption="false" HasMarginBottom="false" meta:resourcekey="uxPageTitleResource1" />
    <%} %>
    <div class="in report-graphs-panel">
        <div class="row">
            <div class="col-md-12">
                <tek:RadTabStrip ID="uxTabView" OnClientTabSelected="tabSelected" runat="server" CssClass="as-animated-tabstrip">
                    <Tabs>
                        <tek:RadTab Text="MTD" Value="bymtd" Selected="true" meta:resourcekey="MTDResource1">
                        </tek:RadTab>
                        <tek:RadTab Text="YTD" Value="byytd" meta:resourcekey="YTDResource1">
                        </tek:RadTab>
                        <tek:RadTab Text="12 Months" Value="by12months" Visible="false" meta:resourcekey="monthsResource1">
                        </tek:RadTab>
                    </Tabs>
                </tek:RadTabStrip>
            </div>
        </div>
        <div id="bymtd" class="uc-content">
            <uc:MTDChart ID="uxMTDChart" runat="server" />
        </div>
        <div id="byytd" class="hide uc-content">
            <uc:YTDChart ID="uxYTDChart" runat="server" />
        </div>
        <% if (Has12MonthsChart)
           { %>
        <div id="by12months" class="hide uc-content">
            <uc:Last12MonthsChart ID="uxLast12MonthsChart" runat="server" />
        </div>
        <% } %>
    </div>
    <div class="row">
        <div class="col-md-12 text-center line-height-zero graphs-panel" data-target=".report-graphs-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter" onclick="refreshChart();">
                <asp:Literal ID="Literal41" runat="server" Text="GRAPH" meta:resourcekey="DashboardASPX_Text_Graph" /></span>
        </div>
    </div>
    <%--<uc:DashBoardChart ID="uxDashBoardChart" runat="server" />--%>
    <as:PlaceHolder ID="uxPanelHierarchy" runat="server">
        <div class="row mt-9x dashboard-info">
            <div class="col-md-4 mt-8x">
                <div>
                    <!--Monthly Card Volume (former Card Volume - Rolling 12 Months)-->
                    <uc:UxExportTable ID="UxExportMonthlyCardVolume" runat="server" GridID="uxMonthlyCardVolumeGrid"
                        Title="Monthly Volume" SubTitle="" OnExportCSV="UxExportMonthlyCardVolume_ExportCSV"
                        OnExportExcel="UxExportMonthlyCardVolume_ExportExcel" ShowPDF="true"
                        OnExportPdf="UxExportMonthlyCardVolume_ExportPdf" meta:resourcekey="UxExportMonthlyCardVolumeResource1" />
                    <div class="in mt-5x" id="uxMonthlyCardVolumeGrid">
                        <table class="monthly-table" id="tblcardVolumeRolling13Months">
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>
                                        <as:Literal ID="Literal1" runat="server" Text="Gross Sales" meta:resourcekey="Literal1Resource1" /></th>
                                    <th class="text-right">
                                        <as:Literal ID="Literal2" runat="server" Text="# Trans" meta:resourcekey="Literal2Resource1" /></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="uxMonthlyCardVolume" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td runat="server" id="tdMonths">
                                                <%# Eval("Months") %>
                                            </td>
                                            <td class="text-right"
                                                runat="server" id="tdAmount">
                                                <%# FormatCurrency(Eval("Volume")) %>
                                            </td>
                                            <td class="text-right"
                                                runat="server" id="tdCount">
                                                <%# FormatInteger(Eval("TransactionCount"))%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>

            <div class="col-md-8">

                <div class="box box-md">
                    <uc:UxExportTable ID="UxExportVolumeAnalysis" runat="server" GridID="uxVolumeAnalysisGrid" Title="Volume Analysis"
                        OnExportCSV="UxExportVolumeAnalysis_ExportCSV" OnExportExcel="UxExportVolumeAnalysis_ExportExcel" ShowPDF="true"
                        OnExportPdf="UxExportVolumeAnalysis_ExportPdf" meta:resourcekey="UxExportVolumeAnalysisResource1" />
                    <div class="in" id="uxVolumeAnalysisGrid">
                        <table class="ASTable" id="tblVolumeAnalysis">
                            <tr class="TotalCell">
                                <th></th>
                                <th>
                                    <as:Literal ID="Literal6" runat="server" Text="MTD" meta:resourcekey="Literal6Resource1" /></th>
                                <th>
                                    <as:Literal ID="Literal7" runat="server" Text="YTD" meta:resourcekey="Literal7Resource1" /></th>
                                <th>
                                    <as:Literal ID="Literal8" runat="server" Text="Previous Year" meta:resourcekey="Literal8Resource1" /></th>
                            </tr>
                            <tr>
                                <td class="text-left">
                                    <as:Literal ID="Literal9" runat="server" Text="Gross Sales" meta:resourcekey="Literal9Resource1" />
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[0]["SaleAmount"]) %>
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[1]["SaleAmount"]) %>
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[2]["SaleAmount"]) %>
                                </td>
                            </tr>
                            <tr class="section-separator">
                                <td class="text-left indented">
                                    <as:Literal ID="Literal10" runat="server" Text="Transactions" meta:resourcekey="Literal10Resource1" />
                                </td>
                                <td class="numeric">
                                    <%=FormatInteger(_SalesData.Rows[0]["SaleCount"]) %>
                                </td>
                                <td class="numeric">
                                    <%=FormatInteger(_SalesData.Rows[1]["SaleCount"]) %>
                                </td>
                                <td class="numeric">
                                    <%=FormatInteger(_SalesData.Rows[2]["SaleCount"]) %>
                                </td>
                            </tr>

                            <tr>
                                <td class="text-left">
                                    <as:Literal ID="Literal11" runat="server" Text="Returns" meta:resourcekey="Literal11Resource1" />
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[0]["ReturnAmount"])%>
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[1]["ReturnAmount"])%>
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[2]["ReturnAmount"])%>
                                </td>
                            </tr>
                            <asp:Panel runat="server" ID="FT_ReturnCount" Visible="false">
                                <tr>
                                    <td class="text-left indented">
                                        <as:Literal ID="Literal38" runat="server" Text="Returns Count" meta:resourcekey="Literal38Resource1" />
                                    </td>
                                    <td class="numeric">
                                        <%=FormatInteger(_SalesData.Rows[0]["ReturnCount"]) %>                                
                                    </td>
                                    <td class="numeric">
                                        <%=FormatInteger(_SalesData.Rows[1]["ReturnCount"]) %>
                                    </td>
                                    <td class="numeric">
                                        <%=FormatInteger(_SalesData.Rows[2]["ReturnCount"]) %>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr class="section-separator">
                                <td class="text-left indented">
                                    <as:Literal ID="Literal12" runat="server" Text="% Sales" meta:resourcekey="Literal12Resource1" />
                                </td>
                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[0]["ReturnPercent"])%>
                                </td>
                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[1]["ReturnPercent"])%>
                                </td>
                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[2]["ReturnPercent"])%>
                                </td>
                            </tr>

                            <tr class="section-separator">
                                <td class="text-left">
                                    <as:Literal ID="Literal21" runat="server" Text="Net Volume" meta:resourcekey="Literal21Resource1" />
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[0]["NetAmount"]) %>
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[1]["NetAmount"]) %>
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[2]["NetAmount"]) %>
                                </td>
                            </tr>

                            <tr>
                                <td class="text-left">
                                    <as:Literal ID="Literal13" runat="server" Text="Chargebacks" meta:resourcekey="Literal13Resource1" />
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[0]["ChargebackAmount"])%>
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[1]["ChargebackAmount"])%>
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[2]["ChargebackAmount"])%>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-left indented">
                                    <as:Literal ID="Literal14" runat="server" Text="Transactions" meta:resourcekey="Literal14Resource1" />
                                </td>
                                <td class="numeric">
                                    <%=FormatInteger(_SalesData.Rows[0]["ChargebackCount"]) %>
                                </td>
                                <td class="numeric">
                                    <%=FormatInteger(_SalesData.Rows[1]["ChargebackCount"]) %>
                                </td>
                                <td class="numeric">
                                    <%=FormatInteger(_SalesData.Rows[2]["ChargebackCount"]) %>
                                </td>

                            </tr>
                            <tr class="section-separator">
                                <td class="text-left indented">
                                    <as:Literal ID="Literal15" runat="server" Text="% Sales" meta:resourcekey="Literal15Resource1" />
                                </td>
                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[0]["ChargeBackPercent"]) %>
                                </td>
                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[1]["ChargeBackPercent"]) %>
                                </td>
                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[2]["ChargeBackPercent"]) %>
                                </td>
                            </tr>

                            <tr>
                                <td class="text-left">
                                    <as:Literal ID="Literal16" runat="server" Text="Retrievals" meta:resourcekey="Literal16Resource1" />
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[0]["RetrievalAmount"])%>
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[1]["RetrievalAmount"])%>
                                </td>
                                <td class="currency">
                                    <%= FormatCurrency(_SalesData.Rows[2]["RetrievalAmount"])%>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-left indented">
                                    <as:Literal ID="Literal17" runat="server" Text="Transactions" meta:resourcekey="Literal17Resource1" />
                                </td>
                                <td class="numeric">
                                    <%=FormatInteger(_SalesData.Rows[0]["RetrievalCount"]) %>                                
                                </td>
                                <td class="numeric">
                                    <%=FormatInteger(_SalesData.Rows[1]["RetrievalCount"]) %>
                                </td>
                                <td class="numeric">
                                    <%=FormatInteger(_SalesData.Rows[2]["RetrievalCount"]) %>
                                </td>
                            </tr>
                            <tr class="section-separator">
                                <td class="text-left indented">
                                    <as:Literal ID="Literal18" runat="server" Text="% Sales" meta:resourcekey="Literal18Resource1" />
                                </td>

                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[0]["RetrievalPercent"])%>
                                </td>
                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[1]["RetrievalPercent"]) %>
                                </td>
                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[2]["RetrievalPercent"]) %>
                                </td>
                            </tr>
                            <asp:Panel runat="server" ID="FT_KeyedAmount" Visible="false">
                                <tr>
                                    <td class="text-left">
                                        <as:Literal ID="Literal39" runat="server" Text="Keyed $" meta:resourcekey="Literal39Resource1" />
                                    </td>
                                    <td class="currency">
                                        <%= FormatCurrency(_SalesData.Rows[0]["KeyedAmount"]) %>
                                    </td>
                                    <td class="currency">
                                        <%= FormatCurrency(_SalesData.Rows[1]["KeyedAmount"]) %>
                                    </td>
                                    <td class="currency">
                                        <%= FormatCurrency(_SalesData.Rows[2]["KeyedAmount"]) %>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td class="text-left" id="keyedCount">
                                    <as:Literal ID="Literal19" runat="server" Text="Keyed" meta:resourcekey="Literal19Resource1" />
                                </td>
                                <td class="numeric">
                                    <%= FormatInteger(_SalesData.Rows[0]["KeyedCount"]) %>
                                </td>
                                <td class="numeric">
                                    <%= FormatInteger(_SalesData.Rows[1]["KeyedCount"]) %>
                                </td>
                                <td class="numeric">
                                    <%= FormatInteger(_SalesData.Rows[2]["KeyedCount"]) %>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-left indented">
                                    <as:Literal ID="Literal20" runat="server" Text="% Trans" meta:resourcekey="Literal20Resource1" />
                                </td>
                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[0]["KeyedPercent"]) %>
                                </td>
                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[1]["KeyedPercent"]) %>
                                </td>
                                <td class="numeric">
                                    <%= FormatPercent(_SalesData.Rows[2]["KeyedPercent"]) %>
                                </td>
                            </tr>


                        </table>
                    </div>
                </div>

                <div class="box box-md mt-10x">
                    <div class="row db-chart-wp">
                        <div class="col-xs-3">
                            <input class="snob"
                                data-font="Lato"
                                data-font-weight="400"
                                data-thickness=".18"
                                data-min="0"
                                value="<%= FormatPercent(_SalesData.Rows[0]["KeyedPercent"]) %>"
                                data-max="100"
                                data-readonly="true"
                                data-text-legend="<%= GetLocalResourceObject("Dashboard_aspx_KEYED").ToString() %>"
                                data-color-text-legend="#333"
                                data-font-size-legend="12px Lato,Helvetica,Arial,sans-serif"
                                data-is-show-border="true"
                                data-inputcolor="#239BBD"
                                data-fgcolor="#239BBD"
                                data-bgcolor="#F6F6F6"
                                data-width="185">
                            <sup class="snob-custom-format">%</sup>
                        </div>
                        <div class="col-xs-3 text-center">
                            <div class="percent-chart">
                                <span class="percent-value">
                                    <%= FormatPercent(_SalesData.Rows[0]["ReturnPercent"],true)%>
                                </span>
                                <br />
                                <span class="percent-desc">
                                    <as:Literal ID="Literal22" runat="server" Text="RETURNS" meta:resourcekey="Literal22Resource1" /></span>
                            </div>
                        </div>
                        <div class="col-xs-3 text-center">
                            <div class="percent-chart">
                                <span class="percent-value">
                                    <%= FormatPercent(_SalesData.Rows[0]["ChargeBackPercent"],true)%>
                                </span>
                                <br />
                                <span class="percent-desc">
                                    <as:Literal ID="Literal23" runat="server" Text="CHARGEBACKS" meta:resourcekey="Literal23Resource1" /></span>
                            </div>
                        </div>
                        <div class="col-xs-3 text-center">
                            <div class="percent-chart">
                                <span class="percent-value">
                                    <%= FormatPercent(_SalesData.Rows[0]["RetrievalPercent"],true)%>
                                </span>
                                <br />
                                <span class="percent-desc">
                                    <as:Literal ID="Literal24" runat="server" Text="RETRIEVALS" meta:resourcekey="Literal24Resource1" /></span>
                            </div>
                        </div>
                    </div>
                </div>
                <!--Card Volume-->
                <div class="box box-md mt-10x">
                    <uc:UxExportTable ID="uxExportCardVolume" runat="server" GridID="uxCardVolumeGrid" Title="Card Volume" ShowPDF="true"
                        OnExportPdf="uxExportCardVolume_ExportPdf" OnExportExcel="uxExportCardVolume_ExportExcel"
                        OnExportCSV="uxExportCardVolume_ExportCSV" meta:resourcekey="uxExportCardVolumeResource1" />
                    <div class="in" id="uxCardVolumeGrid">
                        <table class="ASTable" id="tblCardVolume">
                            <tr>
                                <th rowspan="2"></th>
                                <th colspan="2">
                                    <as:Literal ID="Literal25" runat="server" Text="MTD" meta:resourcekey="Literal25Resource1" /></th>
                                <th colspan="2">
                                    <as:Literal ID="Literal26" runat="server" Text="YTD" meta:resourcekey="Literal26Resource1" /></th>
                                <th colspan="2">
                                    <as:Literal ID="Literal27" runat="server" Text="Previous Year" meta:resourcekey="Literal27Resource1" /></th>
                            </tr>
                            <tr>
                                <th>
                                    <as:Literal ID="Literal28" runat="server" Text="Gross Sales" meta:resourcekey="Literal28Resource1" /></th>
                                <th>
                                    <as:Literal ID="Literal29" runat="server" Text="# Trans" meta:resourcekey="Literal29Resource1" /></th>
                                <th>
                                    <as:Literal ID="Literal30" runat="server" Text="Gross Sales" meta:resourcekey="Literal30Resource1" /></th>
                                <th>
                                    <as:Literal ID="Literal31" runat="server" Text="# Trans" meta:resourcekey="Literal31Resource1" /></th>
                                <th>
                                    <as:Literal ID="Literal32" runat="server" Text="Gross Sales" meta:resourcekey="Literal32Resource1" /></th>
                                <th>
                                    <as:Literal ID="Literal33" runat="server" Text="# Trans" meta:resourcekey="Literal33Resource1" /></th>
                            </tr>

                            <asp:Repeater ID="rptCardVolume" runat="server">
                                <ItemTemplate>
                                    <tr class="Row">
                                        <td class="text-left">
                                            <%# BuildCardType(Eval("CardType")) %>
                                        </td>
                                        <td class="currency">
                                            <%#FormatCurrency(Eval("MTDSalesVolume"))%>
                                        </td>
                                        <td class="numeric">
                                            <%#FormatInteger(Eval("MTDSalesTransaction"))%>
                                        </td>
                                        <td class="currency">
                                            <%#FormatCurrency(Eval("YTDSalesVolume"))%>
                                        </td>
                                        <td class="numeric">
                                            <%#FormatInteger(Eval("YTDSalesTransaction"))%>
                                        </td>
                                        <td class="currency">
                                            <%#FormatCurrency(Eval("Last12MonthsSalesVolume"))%>
                                        </td>
                                        <td class="numeric">
                                            <%#FormatInteger(Eval("Last12MonthsSalesTransaction"))%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="AltRow">
                                        <td class="text-left">
                                            <%# BuildCardType(Eval("CardType")) %>
                                        </td>
                                        <td class="currency">
                                            <%#FormatCurrency(Eval("MTDSalesVolume"))%>
                                        </td>
                                        <td class="numeric">
                                            <%#FormatInteger(Eval("MTDSalesTransaction"))%>
                                        </td>
                                        <td class="currency">
                                            <%#FormatCurrency(Eval("YTDSalesVolume"))%>
                                        </td>
                                        <td class="numeric">
                                            <%#FormatInteger(Eval("YTDSalesTransaction"))%>
                                        </td>
                                        <td class="currency">
                                            <%#FormatCurrency(Eval("Last12MonthsSalesVolume"))%>
                                        </td>
                                        <td class="numeric">
                                            <%#FormatInteger(Eval("Last12MonthsSalesTransaction"))%>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                        </table>
                        <asp:PlaceHolder ID="uxCardVolumeNoRecords" runat="server" Visible="false">
                            <div class="NoRecords">
                                <as:Literal ID="Literal34" runat="server" Text="No records to display." meta:resourcekey="Literal34Resource1" />
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </div>

                <div id="pnlMerchantApproval" runat="server" class="box box-md mt-10x col-xs-11">
                    <div class="row">
                        <div id="pnlMA" runat="server" class="col-xs-5">
                            <div class="row">
                                <div class="col-xs-12">
                                    <h2 class="grid-title no-toggle">
                                        <as:Literal ID="Literal3" runat="server" Text="Merchant Approvals" meta:resourcekey="Literal3Resource1" />
                                    </h2>
                                </div>
                            </div>
                            <div class="in row mbi-5 mt-11x" id="uxTotalMerchantGrid">
                                <div class="col-xs-6">
                                    <span class="text-size-xxl text-dark-gray line-height-sm"><%= _TotalOpenMerchants%></span><br />
                                    <span class="text-size-sm">
                                        <as:Literal ID="Literal4" runat="server" Text="OPEN" meta:resourcekey="Literal4Resource1" /></span>
                                </div>
                                <asp:Panel runat="server" ID="uxPnlApproval">
                                    <div class="col-xs-6 text-right">
                                        <span class="text-size-xxl text-dark-gray line-height-sm"><%= _MerchantApprovals%></span><br />
                                        <span class="text-size-sm">
                                            <as:Literal ID="Literal5" runat="server" Text="APPROVALS" meta:resourcekey="Literal5Resource1" />
                                        </span>
                                    </div>
                                </asp:Panel>
                                <asp:Panel runat="server" ID="uxPnlClosedMerchant" Visible="false">
                                    <div class="col-xs-6 text-right">
                                        <span class="text-size-xxl text-dark-gray line-height-sm"><%= _TotalClosedMerchants%></span><br />
                                        <span class="text-size-sm">
                                            <as:Literal ID="Literal40" runat="server" Text="CLOSED" meta:resourcekey="Literal40Resource1" />
                                        </span>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <asp:Panel runat="server" ID="FT_MerchantApproval" Visible="false" class="col-xs-7">
                            <div class="in row mbi-5 mt-11x">
                                <div class="col-xs-12">
                                    <table class="ASTable" id="Table1">
                                        <tr class="TotalCell">
                                            <th></th>
                                            <th>
                                                <as:Literal ID="Literal35" runat="server" Text="Previous Day" meta:resourcekey="Literal35Resource1" /></th>
                                            <th>
                                                <as:Literal ID="Literal36" runat="server" Text="MTD" meta:resourcekey="Literal6Resource1" /></th>
                                            <th>
                                                <as:Literal ID="Literal37" runat="server" Text="YTD" meta:resourcekey="Literal7Resource1" /></th>
                                        </tr>
                                        <asp:Repeater ID="rptMerchantApproval" runat="server">
                                            <ItemTemplate>
                                                <tr class="Row">
                                                    <td class="text-left">
                                                        <%# BuildCardType(Eval("MerchantStatus")) %>
                                                    </td>
                                                    <td class="numeric">
                                                        <%#FormatInteger(Eval("TodayTotalMerchant"))%>
                                                    </td>
                                                    <td class="numeric">
                                                        <%#FormatInteger(Eval("MTDTotalMerchant"))%>
                                                    </td>
                                                    <td class="numeric">
                                                        <%#FormatInteger(Eval("YTDTotalMerchant"))%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="AltRow">
                                                    <td class="text-left">
                                                        <%# BuildCardType(Eval("MerchantStatus")) %>
                                                    </td>
                                                    <td class="numeric">
                                                        <%#FormatInteger(Eval("TodayTotalMerchant"))%>
                                                    </td>
                                                    <td class="numeric">
                                                        <%#FormatInteger(Eval("MTDTotalMerchant"))%>
                                                    </td>
                                                    <td class="numeric">
                                                        <%#FormatInteger(Eval("YTDTotalMerchant"))%>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </as:PlaceHolder>
    <as:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script src="res/js/jquery/jquery.snob.js"></script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/Dashboard.js"></script>
        <script type="text/javascript">
            var isFultonClient = '<%= IsFultonClient %>';
        </script>
    </as:RadCodeBlock>
</asp:Content>
