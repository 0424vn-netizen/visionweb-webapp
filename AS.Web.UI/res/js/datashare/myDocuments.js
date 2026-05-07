
function doDowloadFile(docid, fileName) {
    $("#" + uxhdDocId).val(docid);
    $("#" + uxhdFileName).val(fileName);
    $("#" + uxbtnDownload).click();
}
