<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RetrievalChargeBackChart.ascx.cs" Inherits="UserControls_RetrievalChargeBackChart" %>
<div class="row in report-graphs-panel mt-4x">
    <as:KendoChart runat="server" ID="uxRTCBChart" OnNeedDataSource="uxRTCBChart_NeedDataSource" CssClass="col-md-offset-4 col-md-4 center" EmbedJquery="False" meta:resourcekey="uxRTCBChartResource1" PageSize="0">
        <ChartProperties>
            theme: $(document).data("kendoSkin") || "metro",
            title: {
                text: '<%= GetLocalResourceObject("RetrievalChargeBackChartASCX_Text_ChartTitle").ToString()  %>'
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
                field: "value",
                colorField: "color"
            }],
            tooltip: {
                visible: true,
                format:"c2",
                template : "#= formatTooltip(value, '<%= SessionManager.CurrencySymbol%>') #"
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
            },
            valueAxis: {
                labels: {
                    format: "c2",
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
</div>
<div class="row">
    <div class="col-md-12 text-center line-height-zero graphs-panel" data-target=".report-graphs-panel" data-toggle="collapse">
        <span class="btn btn-link btn-report-filter" onclick="refreshChart();">
            <asp:Literal ID="Literal1" runat="server" Text="GRAPH" meta:resourcekey="RetrievalChargeBackChartASCX_Text_Graph" /></span>
    </div>
</div>
<script>
    function formatTooltip(value, c) {
        var val = value.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        if (value < 0)
            return "<font color='red'>(" + c + " " + val.replace("-", "") + ")</font>";
        return c + val;
    }
</script>