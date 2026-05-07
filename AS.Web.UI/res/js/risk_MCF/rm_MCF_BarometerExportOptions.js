
function uxSubmit_Click() {
    var isAll = $('input[name=exportOption]:checked').val() == 'allColumns';
    var exportType = parent.GetExportType();
    parent.ExportClick(exportType, isAll);
    parent.HidePopupModal();
}