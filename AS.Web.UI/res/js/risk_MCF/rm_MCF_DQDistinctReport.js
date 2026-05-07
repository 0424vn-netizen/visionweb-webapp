function ajaxRequestStart(sender, args) {
    if (args.get_eventTarget().indexOf('uxExporterDistinctTop') != -1
        || args.get_eventTarget().indexOf('uxExporterDistinctBottom') != -1) {
        args.set_enableAjax(false);
    }
}

function setTabActive(isGridView) {
    var isGridViewMode = isGridView == '1';
    $(".view-mode-options > .btn-option").removeClass("active");
    if (isGridViewMode) {
        $(".view-mode-options > .btn-option-gridview").addClass("active");
    } else {
        $(".view-mode-options > .btn-option-cardview").addClass("active");
    }
}