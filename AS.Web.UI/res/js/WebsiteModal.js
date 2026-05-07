
function closeMe() {
    return parent.HidePopupModal();
}
function masterAjax_responseEnd(sender, args) {

    setTimeout("AdjustModalSize();", 500);
}