function ajaxRequestStart(sender, args) {
    if (args.get_eventTarget().indexOf('uxExporter') != -1) {
        args.set_enableAjax(false);
    }
}

function reloadRainbowReport() {
    var grid = $find(detectionQueueRainbowReport_uxReportGrid).get_masterTableView();
    grid.rebind();
}

function setTabActive( isGridView) {
    var isGridViewMode = isGridView == '1';
    $(".view-mode-options > .btn-option").removeClass("active");
    if (isGridViewMode) {
        $(".view-mode-options > .btn-option-gridview").addClass("active");
    } else {
        $(".view-mode-options > .btn-option-cardview").addClass("active");
    }
}