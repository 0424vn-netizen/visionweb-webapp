function OpenForgotWndMsg(url) {
    GetRadWindow().BrowserWindow.doOpenForgotWndMsg(url);
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function ValidateInput() { 
    if (!doValidateInput()) { 
        var modalWindow = null;
        if (window.radWindow)
            modalWindow = window.radWindow;
        else if (window.frameElement != null && window.frameElement.radWindow)
            modalWindow = window.frameElement.radWindow;

        if (modalWindow != null) {
            modalWindow.autoSize(true);
        }
        return false;
    }
    return true;
}