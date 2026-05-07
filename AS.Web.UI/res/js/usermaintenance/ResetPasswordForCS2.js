function closeMe(reload) {
    if ($(uxhidReload_ClientID).val() == '1')
        parent.doRebindUserList();
    var loc = new String(parent.location);
    var ind = loc.indexOf("MerchantProfile");
    if (ind > 0)
        parent.UpdateResult();
    return parent.HidePopupModal();
}