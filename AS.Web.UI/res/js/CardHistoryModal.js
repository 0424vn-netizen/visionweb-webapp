
function LoadAuth() {
    $("#uxPanelAuth").show();
    document.getElementById(uxShowAuthID).click();
}
function LoadChargeback() {
    $("#uxPanelChargeback").show();
    document.getElementById(uxShowChargebackID).click();
}
function LoadRetrieval() {
    $("#uxPanelRetrieval").show();
    document.getElementById(uxShowRetrievalID).click();
}
function ajaxRequestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}

function ajaxResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
    $('[data-hover="dropdown"]').dropdownHover();
}

function OpenDetailModal(e) {
    document.getElementById(hdShowDetailModalID).value = $(e).attr("modalparam");
    document.getElementById(uxShowModalDetailID).click();

}

$(document).ready(function () {
    setTimeout("LoadAuth();", 300);
});