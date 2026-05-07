function UxExportQueue_DoExport(fileType) {
    var filterParams = document.getElementById(UxExportQueue_FilterParamsId).value;

    var data = new FormData();
    data.append('pageName', UxExportQueue_PageName);
    data.append('fileName', UxExportQueue_FileName);
    data.append('fileType', fileType);
    data.append('filterParams', filterParams);

    fetch(UxExportQueue_HandlerUrl, { method: 'POST', body: data })
        .then(function (response) {
            if (response.ok) {
                ShowPopupModal(UxExportQueue_NotifyUrl, 'auto');
            }
        });
}