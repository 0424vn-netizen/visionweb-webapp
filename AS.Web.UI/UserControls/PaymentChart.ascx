<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PaymentChart.ascx.cs" Inherits="UserControls_PaymentChart" %>

<div class="row in report-graphs-panel">
    <div class="col-md-12 text-center">
        <div class="row">
            <as:KendoChart runat="server" ID="uxDailyDepositsChart" OnNeedDataSource="uxDailyDepositsChart_NeedDataSource" CssClass="col-md-12" PageSize="30" EmbedJquery="False" meta:resourcekey="uxDailyDepositsChartResource1">
                <ChartProperties>
                    theme: $(document).data("kendoSkin") || "metro",
                    title: {
                        text: '<%= GetLocalResourceObject("PaymentChart_ascx_ChartTitle").ToString() %>',
                        align: "left",
                        padding: {
                            left: -10
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
                            format: "<%=SessionManager.CurrencySymbol %>{0}"
                        }
                    },
                    series: [{
                        type: "column",
                        field: "NetDepositAmount",
                        colorField: "Color"
                    }],
                    valueAxis: [{
                        labels: {
                            template : "#= formatCurrency(value, '<%= SessionManager.CurrencySymbol%>') #",
                            format: "c2",
                            step: 2,
                            skip: 1
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
					        labels:
					        {
						        template : "#= formatLabel(value,'') #"
					        }     
                    }],
                    chartArea: {
                        height: 345,
                        margin: {                                
                            bottom: 20
                        } 
                    },
                    tooltip: {
                        visible: true,
                        format: "c2",
                        template : "#= formatTooltip(value,'<%=SessionManager.CurrencySymbol %>') #"
                    }
                </ChartProperties>
            </as:KendoChart>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-12 text-center line-height-zero graphs-panel" data-target=".report-graphs-panel" data-toggle="collapse">
        <span class="btn btn-link btn-report-filter" onclick="refreshChart();">
            <asp:Literal ID="Literal1" runat="server" Text="GRAPH" meta:resourcekey="PaymentChartASCX_Text_Graph" /></span>
    </div>
</div>
<tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
    <script type="text/javascript">
        var uxDailyVolumeChart_ClientID = '<%= uxDailyDepositsChart.ClientID %>';
        var uxKeyedSwipedChart_ClientID = "";
        var isHasNegativeData = '<%= _IsHasNagativeData%>';          
    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/DailyVolumeChart.js"></script>
</tek:RadCodeBlock>
