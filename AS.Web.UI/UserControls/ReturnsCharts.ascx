<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReturnsCharts.ascx.cs" Inherits="UserControls_ReturnsCharts" %>
<div class="row">
    <div class="col-md-12 text-center">
        <div class="row">
            <as:KendoChart ID="uxSalesReturnsChart" runat="server" OnNeedDataSource="uxSalesReturnsChart_NeedDataSource" CssClass="col-md-4" EmbedJquery="False" meta:resourcekey="uxSalesReturnsChartResource1" PageSize="0">
                <ChartProperties>
                    theme: $(document).data("kendoSkin") || "metro",
                    title: {
                        text: '<%= GetLocalResourceObject("ReturnChart_ascx_ChartTitle").ToString() %>'
                    },
                    legend: {
                        visible: false
                    },
                    seriesDefaults: {
                        type: "pie",
                        labels: {
                            visible: false,
                            format: "{0}%"
                        }
                    },
                    series: [{
                        type: "pie",
                        field: "value",
                        categoryField: "label",
                        colorField: "color"
                    }],
                    tooltip: {
                        visible: true,
                        template:
                            "${ category } - ${ value }%"
                    },
                    chartArea: {
                        height: 200,
                        border: {
                            width: 1,
                            color: "#DDDDDD"
                        },
                        margin: {
                            left: 18,
                            right: 18,
                            bottom: 5
                        }
                    }
            
                </ChartProperties>
            </as:KendoChart>
            <as:KendoChart ID="uxMatchedReturnsChart" runat="server" OnNeedDataSource="uxMatchedReturnsChart_NeedDataSource" CssClass="col-md-4" EmbedJquery="False" meta:resourcekey="uxMatchedReturnsChartResource1" PageSize="0">
                <ChartProperties>
                    theme: $(document).data("kendoSkin") || "metro",
                    title: {
                        text: '<%= GetLocalResourceObject("ReturnChart_ascx_MatchedChartTitle").ToString() %>'
                    },
                    legend: {
                        visible: false
                    },
                    seriesDefaults: {
                        type: "pie",
                        labels: {
                            visible: false,
                            format: "{0}%"
                        }
                    },
                    series: [{
                        type: "pie",
                        field: "value",
                        categoryField: "label",
                        colorField: "color"
                    }],
                    tooltip: {
                        visible: true,
                        template:
                            "${ category } - ${ value }%"
                    },
                    chartArea: {
                        height: 200,
                        border: {
                            width: 1,
                            color: "#DDDDDD"
                        },
                        margin: {
                            left: 18,
                            right: 18,
                            bottom: 5
                        }
                    }
                </ChartProperties>
            </as:KendoChart>
        </div>
    </div>
</div>
