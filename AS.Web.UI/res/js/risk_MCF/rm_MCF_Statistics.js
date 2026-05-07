
function ConfirmDelete() {
    if (confirm(rm_MgmtReport_Extracts_ConfirmDelete))
        return true;
    return false;
}

var IS_VALID_DOWNLOAD = false;

//All download request are valid when they pass throught this function
function doDowloadFile(docid, fileName) {
    IS_VALID_DOWNLOAD = true;
    $("#" + uxhdDocId).val(docid);
    $("#" + uxhdFileName).val(fileName);
    $get(uxbtnDownload).click();
    //$("#" + uxbtnDownload).click();
}

function UpdateTotalRecord(total) {
    $('#' + hddTotalRecords).val(total);
}

function validateDocId() {
    var isValid = IS_VALID_DOWNLOAD;
    IS_VALID_DOWNLOAD = false;
    return isValid;
}

$(document).ready(function () {
    setInterval(function () {
        // When Advanced Filer open: not refresh grid.
        if ($('#popupContentOverlay').hasClass('hide') && !$("#advFilterPortfolioStatistics").hasClass("is-open")) {
            document.getElementById(statistics.statisticsFilter._btnRefresh).click();
        }
    }, 60000);
})