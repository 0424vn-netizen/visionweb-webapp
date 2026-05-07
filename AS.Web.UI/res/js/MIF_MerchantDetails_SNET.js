$(document).ready(function () {
    if ($("#trUserID").length) {
        var rows = $("#tblMerchantInformation tr").length;
        if (rows % 2 == 0) {
            $("table#tblMerchantInformation tr:last").addClass("AltRow");
        }
        else {
            $("table#tblMerchantInformation tr:last").addClass("Row");
        }
    }
});