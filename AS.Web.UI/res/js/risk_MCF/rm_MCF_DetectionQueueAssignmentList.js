function SetHeaderGrid() {
    var secondRowHeaders =
        [
            ['', 1, 'GridHeader_FirstColumn rgHeader border-lr'],
            ['', 1, 'rgHeader border-right'],
            [Text_Eligible, 1, 'rgHeader mh'],
            [Text_Alerted, 2, 'rgHeader mh'],
            [Text_ReadyToWork, 2, 'rgHeader mh'],
            [Text_Wip, 2, 'rgHeader mh'],
            [Text_Worked, 2, 'rgHeader mh'],
            ['', 1, 'rgHeader mh border-right'],
            ['', 1, 'rgHeader border-right'],
            ['Re-queued', 2, 'GridHeader_LastColumn rgHeader mh']
        ];
    var firstRowHeaders =
        [
            ['', 1, 'GridHeader_FirstColumn rgHeader mh border-right'],
            ['', 1, 'rgHeader mh'],
            [Text_DistinctMerchantsCurrentStatus, 10, 'rgHeader mh'],
            ['', 1, 'rgHeader mh border-right'],
            ['', 2, 'rgHeader mh'],

        ]
    if (!rm_MCF_DetectionQueueAssignmentList_hasQueuingMechanism) {
        secondRowHeaders =
            [
                ['', 1, 'GridHeader_FirstColumn rgHeader border-lr'],
                ['', 1, 'rgHeader border-right'],
                [Text_Eligible, 1, 'rgHeader mh'],
                [Text_Alerted, 2, 'rgHeader mh'],
                [Text_ReadyToWork, 2, 'rgHeader mh'],
                [Text_Wip, 2, 'rgHeader mh'],
                [Text_Worked, 2, 'rgHeader mh'],
                ['', 1, 'rgHeader border-right'],
                ['', 1, 'GridHeader_LastColumn rgHeader border-right']
            ];
        firstRowHeaders =
            [
                ['', 1, 'GridHeader_FirstColumn rgHeader mh border-right'],
                ['', 1, 'rgHeader mh'],
                [Text_DistinctMerchantsCurrentStatus, 9, 'rgHeader mh'],
                ['', 1, 'rgHeader mh border-right'],
                ['', 1, 'rgHeader mh'],

            ]
    }
    addGroupHeadersForStaticRadGrid(rm_AssignmentList_uxAssignmentList_ClientID, secondRowHeaders, false, true);
    addGroupHeadersForStaticRadGrid(rm_AssignmentList_uxAssignmentList_ClientID, firstRowHeaders, false);
}

function SetHeaderAssignmentSummaryScreenWidth() {
    var screenWidth = $(window).width();
    if (screenWidth <= 1280) {
        var gridHeaderID = $find(rm_AssignmentList_uxAssignmentList).GridHeaderDiv.children[0].id;
        var gridDataID = $find(rm_AssignmentList_uxAssignmentList).GridDataDiv.children[0].id;
        var headerCols = "#" + gridHeaderID + " > colgroup > col";
        var dataCols = "#" + gridDataID + " > colgroup > col";
        $(headerCols).each(function (i) {
            var colNum = i - 1;
            var $colHeader = $(headerCols).eq(colNum);
            var $colData = $(dataCols).eq(colNum);
            if (colNum === 0) {
                $colHeader.attr("style", "width:150px");
                $colData.attr("style", "width:150px");
            }
            if (colNum === 1) {
                $colHeader.attr("style", "width:45px");
                $colData.attr("style", "width:45px");
            }
            if (colNum === 4 || colNum === 6 || colNum === 8 || colNum === 10 || colNum === 14) {
                $colHeader.attr("style", "width:98px");
                $colData.attr("style", "width:98px");
            }
        });
    }
}

$(document).ready(function () {
    SetHeaderGrid();
    SetHeaderAssignmentSummaryScreenWidth();
    if (typeof initAllProgressBar == 'function') {
        initAllProgressBar();
    }
});

function masterAjax_responseEnd(sender, args) {
    SetHeaderGrid();
    SetHeaderAssignmentSummaryScreenWidth();
    if (typeof initAllProgressBar == 'function') {
        initAllProgressBar();
    }
}

function UpdateRequeuedMerchantSummary(requeuedCount, requeuedVolume) {
    var uxRequeuedCountField = $(".requeuedCount");
    var uxRequeuedVolumeField = $(".requeuedVolume");

    uxRequeuedCountField.html(addCommas(requeuedCount));
    uxRequeuedVolumeField.html(requeuedVolume.toString());
    uxRequeuedVolumeField.formatCurrency();
}

function UpdateAssigmentSummary(todayVolume, state, currentState, unCheckLastItem) {
    var uxWorkedCount = $(".workedCount");
    var uxWorkedVolume = $(".workedVolume");
    var uxAlertCount = $(".alertCount");

    var workedCount = parseInt(uxWorkedCount.html().replace(",", ""));
    var workedVolume = parseFloat($("#uxWorkedVolumeValue").val());
    var alertCount = parseInt(uxAlertCount.html().replace(",", ""));

    todayVolume = parseFloat(todayVolume.toString());

    if (currentState != 2 && (state == 2 || state == 3)) {
        workedCount += 1;
        workedVolume += todayVolume;
    }
    else if ((unCheckLastItem && currentState == 2) || ((state == 0 || state == 1) && currentState == 2)) {
        workedCount -= 1;
        workedVolume -= todayVolume;
    }
    workedVolume = workedVolume.toFixed(2);
    uxWorkedCount.html(addCommas(workedCount.toString()));
    uxWorkedVolume.html(workedVolume.toString());
    uxWorkedVolume.formatCurrency();
    $("#uxWorkedVolumeValue").val(workedVolume.toString());

    resetProgressBar(workedCount, alertCount);
}
function resetProgressBar(count, total) {
    var percentage = 0;
    if (total <= 0 && count > 0) {
        percentage = 100;
    }
    else if (total > 0) {
        percentage = (count * 100) / total;
    }
    percentage = Math.round(percentage * Math.pow(10, 2)) / Math.pow(10, 2);
    var htmlPer = percentage.toString();
    if (htmlPer.length <= 2) {
        htmlPer += ".00";
    }

    var uxProgress = $("div.progressbar-wrapper");
    var tdHeight = $(uxProgress).parent().height();
    var tdWidth = $(uxProgress).parent().width();
    $(uxProgress).css("height", tdHeight);
    $(uxProgress).find(".ProgressBarDiv").height(tdHeight);

    var percentText = $(uxProgress).find(".percent-text");
    var textHeight = $(percentText).height();
    var textWidth = $(percentText).width();
    $(percentText).css({ "left": tdWidth / 2 - textWidth / 2, "top": tdHeight / 2 - textHeight / 2 });

    //set text
    $(percentText).html(htmlPer + "%");
    // Set color and width
    var staticClass = "ProgressBarDiv";
    var ProgressBarDiv = $(uxProgress).find(".ProgressBarDiv")
    ProgressBarDiv.removeClass();
    if (percentage <= 25) {
        ProgressBarDiv.addClass(staticClass + " " + "progressbar-red");
    }
    else if (percentage > 25 && percentage <= 74) {
        ProgressBarDiv.addClass(staticClass + " " + "progressbar-yellow");
    }
    else {
        ProgressBarDiv.addClass(staticClass + " " + "progressbar-green");
    }
    ProgressBarDiv.css("width", percentage / 100 * tdWidth);
}

function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function refreshAssigmentList() {
    $get(rm_AssignmentList_btnRefresh).click();
}

function viewAssignment(assignmentId) {
    $get(rm_AssignmentList_hddAssignmentValue).value = assignmentId;
    $get(rm_AssignmentList_btnViewAssignment).click();

    if (rm_AssignmentList_FromPage == 'DetectionQueue') {
        setTimeout('parent.refreshAssignmentGrid();', 500);
    }

    return false;
}
$(window).resize(function () { initAllProgressBar(); SetHeaderAssignmentSummaryScreenWidth();})
