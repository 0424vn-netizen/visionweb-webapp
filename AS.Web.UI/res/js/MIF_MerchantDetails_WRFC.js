function CheckPrefix() {
    var regName1 = /^(mr|ms|sir|jr|mrs)+[\s+.]+(\w+|\s)/i;
    var regName2 = /^[A-Za-z0-9\s\.\-]+[\s|.]+(mr|ms|sir|jr|mrs|mr.|ms.|sir.|jr.|mrs.|mr\s+.|ms\s+.|sir\s+.|jr\s+.|mrs\s+.)$/i;
    var regName3 = /^(mr|ms|sir|jr|mrs|mr.|ms.|sir.|jr.|mrs.|mr\s+.|ms\s+.|sir\s+.|jr\s+.|mrs\s+.)/i;
    if (!regName1.test($.trim($(MIF_MerchantDetails_WRFC_uxtxtRelationShipManager).val().toLowerCase())) && !regName2.test($.trim($(MIF_MerchantDetails_WRFC_uxtxtRelationShipManager).val().toLowerCase())) && !regName3.test($.trim($(MIF_MerchantDetails_WRFC_uxtxtRelationShipManager).val().toLowerCase())))
        return true;
    else
        return false;
}
function CheckPrefixSpecial() {
    var reg2 = /^[A-Za-z0-9\-\s+.]*$/;
    if (reg2.test($.trim($(MIF_MerchantDetails_WRFC_uxtxtRelationShipManager).val())))
        return true;
    else
        return false;
}