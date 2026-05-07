
function ConfirmDelete() {
    if (confirm(rm_MgmtReport_Extracts_ConfirmDelete))
        return true;
    return false;
}

function doDowloadFile(docid, fileName) {
    $("#" + uxhdDocId).val(docid);
    $("#" + uxhdFileName).val(fileName);
    $get(uxbtnDownload).click();
    //$("#" + uxbtnDownload).click();
}

var isIncludedSpecialCharacter = false;
function triggerCheckAndRemoveSpecialCharacters() {
    var uxMerchantNumber = document.getElementById(MerchantAlertHistory_ReportFilter_uxMerchantNumber)
    if (isIncludeSpecialCharacters(uxMerchantNumber)) {
        isIncludedSpecialCharacter = true;
        CheckMerchantNumber();
        removeSpecialCharacters(uxMerchantNumber);
    }
    var uxMerchantName = document.getElementById(MerchantAlertHistory_ReportFilter_uxMerchantName)
    if (isIncludeSpecialCharacters(uxMerchantName)) {
        isIncludedSpecialCharacter = true;
        CheckMerchantName();
        removeSpecialCharacters(uxMerchantName);
    }
}

$(document).ready(function () {
    setInterval(function () {
        triggerCheckAndRemoveSpecialCharacters();
        if (isIncludedSpecialCharacter) {
            isIncludedSpecialCharacter = false;
            return;
        }
        document.getElementById(btnRefreshGrid).click();
    }, 60000);
}) 