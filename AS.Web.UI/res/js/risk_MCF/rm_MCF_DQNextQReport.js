function AccountClick(param) {
    $get(rm_MCF_DQNextQReport_uxAccountValue).value = param;
    $get(rm_MCF_DQNextQReport_uxAccount).click();
}
function ShowPopupAcctNumber(actt) {
    document.getElementById(rm_MCF_DQNextQReport_uxHiddenAccountNumberClick).value = actt;
    document.getElementById(rm_MCF_DQNextQReport_uxAccountNumberClick).click();
    return false;
}
function openPopupCardWindow(url) {
    openPopupWindow(url, 'CardHistoryWindow');
    return false;
}
function doOpenNewPopup(encodeURL) {
    return parent.ShowPopupModal(encodeURL, 'auto');
}
function doOpenCustomViewPopup(url) {
    registerCloseCustomViewModalEvent();
    return doOpenNewPopup(url);
}
