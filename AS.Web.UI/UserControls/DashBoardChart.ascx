<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DashBoardChart.ascx.cs" Inherits="UserControls_DashBoardChart" %>
<div class="dashboard">
    <div class="row">
        <div class="col-md-12">
            <div id="netChart">
                <div class="chart-title hide">
                    <asp:Literal ID="Literal1" runat="server" Text="Net Volume" meta:resourcekey="LiteralResource1" /></div>
                <as:KendoChart runat="server" ID="uxNetVolumeChart" OnNeedDataSource="uxNetVolumeChart_NeedDataSource" CssClass="max-width" EmbedJquery="False" PageSize="0">
                    <ChartProperties>
                        theme: "metro",

                        legend: {
                            position: "bottom"
                        },
                        seriesDefaults: {
                            type: "area",
                            labels: {
                                visible: false,
                                background: "transparent", 
                                format: '<%= SessionManager.CurrencySymbol%>{0}'
                      
                            },
                            opacity: 0.2
                        },
                        series: [{
                            field: "SaleAmountPrevious",
                            markers: {
                                visible: true,
                                size: 4,
								border: {
									width: 1
								}
                            },					
							line: {
								width: 1,
								color: "#AAAAAA",
							},
                            color: "#AAAAAA",
                            tooltip: {
                                visible: true,
                                format: "c2",
                                template: function(e){
                                    return e.dataItem.TooltipPrevious;
                                }
                            },
                            visible: <%=ChartType == DashBoardChartType.Last12Months ? "false" : "true" %>
                        }, {
                            field: "SaleAmount",
                            markers: {
                                visible: true,
                                size: 4,
								border: {
									width: 1
								}
                            },
                            color: "#3DCC8A",
                            tooltip: {
                                visible: true,
                                format: "c2",
                                template: function(e){
                                    return e.dataItem.TooltipCurrent;
                                }
                            },
							line: {
								width: 1,
								color: "#3DCC8A",
							}
                        }],
                        valueAxis: {
                            labels: {
                                template : "#= formatCurrency(value, '<%= SessionManager.CurrencySymbol%>') #",
                                format: "c2",
                                step: 2
                            },
                            majorGridLines: {
                                visible: true
                            },
                            majorTicks: {
                                visible: true
                            }
                        },
                        categoryAxis: {
                            field: "XValue",
                            majorGridLines: {
                                visible: true
                            }

                        },
                        chartArea: {
                            height: 240
                        }
                                
                    </ChartProperties>
                </as:KendoChart>
            </div>
        </div>
    </div>
    <div class="row dashboard-stat">
        <div class="col-xs-4 item">
            <p id="current-value" class="highlight">
                <%= AS.Common.VeraCodeSolution.DoVeraCode(AS.Common.Formater.FormatData.FormatCurrency(_grossSalesCurrent, SessionManager.CurrencyFortmat)) %>
            </p>
            <p class="title-small">
                <as:Literal ID="ltGrossSales" runat="server" Text="Gross Sales" meta:resourcekey="ltGrossSalesResource1"></as:Literal></p>
            <p class="title-small"><%= AS.Common.VeraCodeSolution.DoVeraCode(_textSalesCurrent)%></p>

        </div>
        <%if (ChartType != DashBoardChartType.Last12Months)
          { %>
        <div class="col-xs-4 item">
            <p id="current-NinetyDayvalue" class="highlight-muted">
                <%= AS.Common.VeraCodeSolution.DoVeraCode(AS.Common.Formater.FormatData.FormatCurrency(_grossSalesPrevious, SessionManager.CurrencyFortmat)) %>
            </p>
            <p class="title-small">
                <as:Literal ID="Literal2" runat="server" Text="Gross Sales" meta:resourcekey="Literal2Resource1"></as:Literal></p>
            <p class="title-small"><%= AS.Common.VeraCodeSolution.DoVeraCode(_textSalesPrevious)%></p>

        </div>
        <% } %>
    </div>
</div>
<script type="text/javascript">
    function formatCurrency(value, c) {
        return String.format("{1}{0}", value, c);
    } 
</script>
