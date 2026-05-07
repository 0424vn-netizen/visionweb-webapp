function ajaxRequestStart(sender, args) {
    if (typeof (UxExporter_OnRequestStart) == 'function')
        UxExporter_OnRequestStart(sender, args);
}
function ajaxResponseEnd(sender, args) {
    if (typeof (UxExporter_OnResponseEnd) == 'function')
        UxExporter_OnResponseEnd(sender, args);
}
function doClose(url) {
    parent.location.href = url;
}
function doReadMessage(messageID) {
    document.getElementById(uxMessageID_ClientID).value = messageID;
    document.getElementById(uxReadMesage_ClientID).click();
}