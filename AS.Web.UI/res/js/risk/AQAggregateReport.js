function ajaxRequestStart(sender, args) {
    if (args.get_eventTarget().indexOf('uxExporterAggregateTop') != -1
        || args.get_eventTarget().indexOf('uxExporterAggregateBottom') != -1) {
        args.set_enableAjax(false);
    }
}
function ChangeAQMerchantWorked(chk, volume) {
    //Asynch updating Worked status
    PageMethods.UpdateMerchantWorkedStatus(chk.checked + ";" + chk.value);

    var curentTR = $(chk).parents('tr');
    var workLast30 = parseInt(curentTR.children(".workLast30").html());
    if (chk.checked) {
        curentTR.children(".merchantName").css("background-color", "LightPink");
        curentTR.children(".workLast30").html(workLast30 + 1);
    }
    else {
        curentTR.children(".merchantName").css("background-color", $(curentTR.children()[0]).css("background-color"));
        curentTR.children(".workLast30").html(workLast30 - 1);
    }
}

function setTabActive1(isGridView) {
    var isGridViewMode = isGridView == '1';
    $(".view-mode-options > .btn-option").removeClass("active");
    if (isGridViewMode) {
        $(".view-mode-options > .btn-option-gridview").addClass("active");
    } else {
        $(".view-mode-options > .btn-option-cardview").addClass("active");
    }
}