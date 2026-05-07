function ReloadRadGridAll() {
    $('#' + uxReloadGrid_ClientID).click();
}
function DoClose() {
    HidePopupModal();
    ReloadRadGridAll();
}