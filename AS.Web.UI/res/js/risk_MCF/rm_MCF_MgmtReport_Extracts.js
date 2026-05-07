function ConfirmDelete() {
    if (confirm(rm_MgmtReport_Extracts_ConfirmDelete))
        return true;
    return false;
}


// Refresh grid after 1 minute
function onRefreshStatementGrid() {
    var intervalRefresh = setInterval(function () {
        document.getElementById(btnRefreshGrid).click();
        clearInterval(intervalRefresh);
    }, 60000);
}

function doDowloadFile(docid, fileName) {
    $("#" + uxhdDocId).val(docid);
    $("#" + uxhdFileName).val(fileName);
    $get(uxbtnDownload).click();
    //$("#" + uxbtnDownload).click();
}
