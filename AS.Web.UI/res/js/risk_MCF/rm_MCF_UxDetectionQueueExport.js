function DoStopPropagation() {
    $('.no-collapsable').unbind("click");
    $('.no-collapsable').click(function (e) {
        e.stopPropagation();
    });
}
function masterAjax_responseEnd(sender, args) {
    DoStopPropagation();
}
function ShowExportModal(obj, exportType) {
    $('#' + uxCustomizeColumnsExportTop_hddExportType).val(exportType);
    return ShowPopupModal('rm_MCF_BarometerExportOptionModal.aspx', 'auto');
}
function GetExportType() {
    return $('#' + uxCustomizeColumnsExportTop_hddExportType).val();
}
function ExportClick(typeExport, isAll) {
    parent.setExportOption(isAll);
    if (typeExport == 'EXCEL')
        document.getElementById(uxCustomizeColumnsExportTop_imgExcelHide).click();
    if (typeExport == 'CSV')
        document.getElementById(uxCustomizeColumnsExportTop_imgCSVHide).click();
    if (typeExport == 'PDF')
        document.getElementById(uxCustomizeColumnsExportTop_imgPDFHide).click();
}

$(document).ready(function () {
    DoStopPropagation();
});