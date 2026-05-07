function CheckPrefix() {
    var regName1 = /^(mr|ms|sir|jr|mrs)+[\s+.]+(\w+|\s)/i;
    var regName2 = /^[A-Za-z0-9\s\.\-]+[\s|.]+(mr|ms|sir|jr|mrs|mr.|ms.|sir.|jr.|mrs.|mr\s+.|ms\s+.|sir\s+.|jr\s+.|mrs\s+.)$/i;
    var regName3 = /^(mr|ms|sir|jr|mrs|mr.|ms.|sir.|jr.|mrs.|mr\s+.|ms\s+.|sir\s+.|jr\s+.|mrs\s+.)/i;
    var reg2 = /^[A-Za-z0-9\-\s+.]*$/;
    if (!regName1.test($.trim($(MIF_MerchantDetails_TNBCI_uxtxtRelationShipManager).val().toLowerCase()))
        && !regName2.test($.trim($(MIF_MerchantDetails_TNBCI_uxtxtRelationShipManager).val().toLowerCase()))
        && !regName3.test($.trim($(MIF_MerchantDetails_TNBCI_uxtxtRelationShipManager).val().toLowerCase()))) {
        if (reg2.test($.trim($(MIF_MerchantDetails_TNBCI_uxtxtRelationShipManager).val()))) {
            return true;
        }
        else {
            alert(Text_AtLeast30NoSpecialChars);
            return false;
        }
    }
    else {
        alert(Text_UnallowPrefixSuffix);
        return false;
    }
}
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