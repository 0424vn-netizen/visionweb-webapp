function ConfirmDelete() {
    if (confirm(exports_ConfirmDelete))
        return true;
    return false;
}

function doDownloadFile(docId, fileName) {
    $("#" + uxhdDocId).val(docId);
    $("#" + uxhdFileName).val(fileName);
    $get(uxbtnDownload).click();
}

function onRefreshExportGrid() {
    var intervalRefresh = setInterval(function () {
        document.getElementById(btnRefreshGrid).click();
        clearInterval(intervalRefresh);
    }, 60000);
}