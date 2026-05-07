function ajaxRequestStart(sender, args) {
    if (args.get_eventTarget().indexOf('uxExportTop') != -1) {
        args.set_enableAjax(false);
    }
}