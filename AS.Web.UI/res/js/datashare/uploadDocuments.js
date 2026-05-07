function uxFilterStatus_Checked() {
    document.getElementById(uxChangeFilterStatus_ClientID).click();
}

function confirmDelete(docId) {
    $('#' + hdDeleteDocument).val(docId);
    ShowPopupModal('DeleteDocumentModal.aspx', 'auto');
    return false;
}

function deleteDocument() {
    HidePopupModal();
    document.getElementById(btnDelete).click();
}

function doDowloadFile(docid, fileName) {
    $("#" + uxhdDocId).val(docid);
    $("#" + uxhdFileName).val(fileName);
    $("#" + uxbtnDownload).click();
}

function reloadDocuments() {
    HidePopupModal();
    document.getElementById(uxChangeFilterStatus_ClientID).click();
}

function openUploadDocumentModal() {
    ShowPopupModal('UploadDocumentsModal.aspx', 'auto');
    return false;
}