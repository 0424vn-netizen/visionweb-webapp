function Callback(sender, e) {
    UxExport_OnResponseEnd(sender, e);
}

function ajaxRequestStart(sender, args) {
    var target = sender.__EVENTTARGET;
    if (target.indexOf("imgExcel") >= 0 || target.indexOf("imgCSV") >= 0) {
        args.set_enableAjax(false);
    }
}

function WhenResponseEnd(sender, eventArgs) {
    var gridID = uxReportGrid_ClientID;
    ShowHideExportControl(gridID);
}

FilteringByColumnWithDataFieldModule.setInvalidMessage('Invalid value. Please enter a valid value.');