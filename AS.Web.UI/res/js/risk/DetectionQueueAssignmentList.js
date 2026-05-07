

function SetHeaderGrid() {
    var assignmentType = rm_AssignmentList_assignmentType_Client;
    var hasQueuingMechanism = rm_AssignmentList_hasQueuingMechanism_Client;

    if (hasQueuingMechanism) {
        switch (assignmentType) {
            case rm_AssignmentList_enumDetectionQueue_Client:
                addGroupHeadersForStaticRadGrid(rm_AssignmentList_uxAssignmentList_ClientID,
                    [['', 1, 'GridHeader_FirstColumn rgHeader mh'],
                    ['', 1, 'rgHeader mh'],
                    [Text_MerchantCount, 3, 'rgHeader mh'],

                    [Text_Worked, 2, 'rgHeader mh'],
                    [Text_ReQueued, 2, 'rgHeader mh'],
                    ['', 1, 'GridHeader_LastColumn rgHeader mh']]);
                break;
            case rm_AssignmentList_enumWorkQueue_Client:
                addGroupHeadersForStaticRadGrid(rm_AssignmentList_uxAssignmentList_ClientID,
                    [['', 1, 'GridHeader_FirstColumn rgHeader mh'],
                    ['', 1, 'rgHeader mh'],
                    [Text_MerchantCount, 3, 'rgHeader mh'],

                    [Text_Worked, 2, 'rgHeader mh'],

                    ['', 1, 'GridHeader_LastColumn rgHeader mh']]);
                break;
            case rm_AssignmentList_enumDetectionQueueDistinct_Client:

                break;
        }
    }
    else {
        addGroupHeadersForStaticRadGrid(rm_AssignmentList_uxAssignmentList,
            [
                ['', 1, 'rgHeader mh'],
                [Text_MerchantCount + ' ', 3, 'rgHeader mh'],
                [Text_Worked + ' ', 2, 'rgHeader mh'],
                ['', 1, 'rgHeader mh']
            ]);
    }
}

$(document).ready(function () {

    SetHeaderGrid();
    if (typeof initAllProgressBar == 'function') {
        initAllProgressBar();
    }
});

function masterAjax_responseEnd(sender, args) {
    SetHeaderGrid();
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



function UpdateAssigmentSummary(todayVolume, isWorked) {
    var uxWorkedCount = $(".workedCount");
    var uxWorkedVolume = $(".workedVolume");
    var uxAlertCount = $(".alertCount");

    var workedCount = parseInt(uxWorkedCount.html().replace(",", ""));
    var workedVolume = parseFloat($("#uxWorkedVolumeValue").val());
    var alertCount = parseInt(uxAlertCount.html().replace(",", ""));

    todayVolume = parseFloat(todayVolume.toString());

    if (isWorked) {
        workedCount += 1;
        workedVolume += todayVolume;
    }
    else {
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
