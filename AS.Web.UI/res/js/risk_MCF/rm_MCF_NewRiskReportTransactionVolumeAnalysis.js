Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequestTransAnalysis);
function doOpenNewPopup(encodeURL) {
    return parent.ShowPopupModal(encodeURL, 'auto');
}

parent.registerCloseCustomViewModalEvent = function () {
    $get(rm_Button_Rebind_ClientID).click();
}

function setExportOption(isAllFields) {
    $("#" + rm_MCF_NewRiskReportTransactionVolumeAnalysis_hddExportOption).val(isAllFields);
}

function removeScrollTransAnalysis() {
    let gridContainer = $('#uxTransVolumeAnalysisGrid');
    let gridDataContainer = gridContainer.find(".rgDataDiv");
    gridDataContainer.css("max-height", "inherit"); // remove scroll for transaction analysis grid
}

function onEndRequestTransAnalysis(sender, args) {
    removeScrollTransAnalysis(); 
}
$(document).ready(function () {
    removeScrollTransAnalysis();
});