<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BatchChart.ascx.cs" Inherits="UserControls_BatchChart" %>

<div class="row in report-graphs-panel">
    <div class="col-md-9">
        <as:KendoChart runat="server" ID="uxDailyVolumeChart" OnNeedDataSource="uxDailyVolumeChart_NeedDataSource" CssClass="max-width" PageSize="30" EmbedJquery="False" meta:resourcekey="uxDailyVolumeChartResource1">
            <ChartProperties>
                theme: $(document).data("kendoSkin") || "metro",
                    title: {
                        text: '<%=GetLocalResourceObject("BatchChart_ascx_cs_ChartTitle").ToString() %>',
                        align: "left",
                        padding: {
                            left: -10,
                            top: -10,
                        }
                    },
                    legend: {
                        position: "top"
                    },
                    seriesDefaults: {
                        type: "column",
                        gap: 1,
                        labels: {
                            visible: false,
                            format: '<%= SessionManager.CurrencySymbol%>{0}'
                        }
                    },
                    series: [{
                        field: "NetVolume",
                        width: 2,
                        markers: {
                            size: 4
                        },
                        colorField: "Color"
                    }],
                    valueAxis: [{                     
                        labels: {
                            template : "#= formatCurrency(value, '<%= SessionManager.CurrencySymbol%>') #",
                            format: "c2",
                            step: 2,
                            skip: 1,
                        },
                        majorTicks: {
                            visible: false
                        } 
                    },{
                        labels:{
                            visible: false
                        },
                        majorTicks: {
                            visible: false
                        }
                    }],
                    categoryAxis: [{
                        field: "Day",
                        majorGridLines: {
                            visible: false
                        },   
                        axisCrossingValue: [0, 30],                 
                        majorTicks: {
                            visible: false
                        },
                        labels:{
                            template : "#= formatLabel(value) #"
                        }                                                  
                    }],
                    tooltip: {
                            visible: true,
                            format: "c2",
                            template : "#= formatTooltip(value, '<%= SessionManager.CurrencySymbol%>') #"
                    
                        },
                    chartArea: {
                        height: 250,
                        margin: {                                
                            bottom: 20
                        }                 
                    }
            </ChartProperties>
        </as:KendoChart>
    </div>
    <div class="col-md-3">
        <as:KendoChart runat="server" ID="uxVolumeCardTypeChart" OnNeedDataSource="uxVolumeCardTypeChart_NeedDataSource" CssClass="max-width" EmbedJquery="False" meta:resourcekey="uxVolumeCardTypeChartResource1" PageSize="0">
            <ChartProperties>
                theme: $(document).data("kendoSkin") || "metro",
            title: {
                text: '<%=GetLocalResourceObject("BatchChart_ascx_cs_LegendTitle").ToString() %>'
            },
            legend: {
                visible: false
            },
            seriesDefaults: {
                type: "bar",
                labels: {
                    visible: false,
                    format: "{0}%"
                }
            },
            series: [{
                type: "bar",
                field: "displayValue",
                colorField: "color"
            }],
            tooltip: {
                visible: true,
                template:
                 "#= dataItem.value #%",
            },
            chartArea: {
                height: 150                
            },
            valueAxis: {
                labels: {
                    format: "{0}%",
                    visible: false
                },
                visible: false,
                majorGridLines: {
                    visible: false
                }
            },
            categoryAxis: {
                field: "label",
                majorGridLines: {
                    visible: false
                },
                line: {
                    visible: false
                }
            }
            </ChartProperties>
        </as:KendoChart>
        <asp:PlaceHolder ID="pnlKeyedSwipedChart" runat="server">
            <div class="right-chart">
                <as:KendoChart runat="server" Visible="false" ID="uxKeyedSwipedChart" OnNeedDataSource="uxKeyedSwipedChart_NeedDataSource" CssClass="max-width" meta:resourcekey="uxKeyedSwipedChartResource1">
                    <ChartProperties>
                        theme: $(document).data("kendoSkin") || "metro",
                               title: {
                                    text: '<%=GetLocalResourceObject("BatchChart_ascx_cs_SwipedChartTitle").ToString() %>'
                                },
                                legend: {
                                    visible: true,
                                    position: "bottom",
                                },
                                seriesDefaults: {
                                    type: "donut",
                                    startAngle: 180
                                },
                                series:[{
                                    type: "donut",
                                            field: "value",
                                            categoryField: "label",
                                            colorField: "color"
                                }],
                                tooltip: {
                                        visible: true,
                                        template: "#= category #: #= value #%"
                                    },
                               chartArea: {
                                    height: 200                                    
                                }

                    </ChartProperties>
                </as:KendoChart>
            </div>
        </asp:PlaceHolder>
    </div>
</div>
<div class="row">
    <div class="col-md-12 text-center line-height-zero graphs-panel" data-target=".report-graphs-panel" data-toggle="collapse">
        <span class="btn btn-link btn-report-filter" onclick="refreshChart();">
            <asp:Literal ID="Literal1" runat="server" Text="GRAPH" meta:resourcekey="BatchChartASCX_Text_Graph" /></span>
    </div>
</div>
<tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
    <script type="text/javascript">
        var uxDailyVolumeChart_ClientID = '<%= uxDailyVolumeChart.ClientID %>';
        var uxKeyedSwipedChart_ClientID = '<%= uxKeyedSwipedChart.ClientID %>';
        var isHasNegativeData = '<%= _IsHasNagativeData%>';          
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/DailyVolumeChart.js"></script>
</tek:RadCodeBlock>

