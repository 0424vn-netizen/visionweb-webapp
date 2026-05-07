$(function () {
    var DailyVolumeChartObj = $('#' + uxDailyVolumeChart_ClientID).data("kendoChart");
    if (uxKeyedSwipedChart_ClientID != "") {
        var isShowKeyedSwipedChart = $('#' + uxKeyedSwipedChart_ClientID).length > 0;
        if (isShowKeyedSwipedChart) {
            DailyVolumeChartObj.options.chartArea.height = 350;
            //VolumeCardTypeChartObj.refresh();
        }
    }
    if (isHasNegativeData == "True") {
        DailyVolumeChartObj.options.categoryAxis = [{
            majorTicks: {
                visible: false
            },
            axisCrossingValue: [0, 30]
        }, DailyVolumeChartObj.options.categoryAxis];
        DailyVolumeChartObj.options.valueAxis[0].axisCrossingValues = [0, Number.NEGATIVE_INFINITY];
    }

    DailyVolumeChartObj.refresh();
});

function formatLabel(value, c) {
    var arr = value.split(" ");
    if (arr.length > 1)
        return String.format("<tspan style='position:absolute' dx='-10' dy='0'>{2}{0} </tspan><tspan style='position:absolute' dx='-20' dy='15'>{1}</tspan>", arr[1], arr[0], c);
    else {
        var val = value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        if (value < 0)
            return String.format("<tspan style='color:red' dx='0' dy='0'>({1}{0})</tspan>", val.replace("-", ""), c);
    else
            return String.format("<tspan dx='0' dy='0'>{1}{0}</tspan>", val, c);
}
}

function formatCurrency(value, c) {
    var val = value.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    if (value < 0)
        return "(" + c + val.replace("-", "") + ")";
    return String.format("{1}{0}", val, c);
}

function formatTooltip(value, c) {
    //var val = kendo.toString(value, "c2");
    var val = value.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    if (value < 0)
        return "<font color='red'>(" + c + val.replace("-", "") + ")</font>";
    return c + val;
}